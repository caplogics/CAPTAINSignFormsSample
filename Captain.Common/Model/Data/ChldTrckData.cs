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
    public class ChldTrckData
    {
        public ChldTrckData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateDelChldTrck(ChldTrckEntity trckEntity)
        {
            bool boolSuccess = false;           
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();



                if (trckEntity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_AGENCY", trckEntity.Agency));
                if (trckEntity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_DEPT", trckEntity.Dept));
                if (trckEntity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_PROGRAM", trckEntity.Prog));
                if (trckEntity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPONENT", trckEntity.COMPONENT));
                if (trckEntity.TASK != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_TASK", trckEntity.TASK));
                if (trckEntity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_SEQ", trckEntity.SEQ));
                if (trckEntity.TASKDESCRIPTION != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_TASK_DESCRIPTION", trckEntity.TASKDESCRIPTION));
                //if (trckEntity.ENTRYYN != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_ENTRY_YN", trckEntity.ENTRYYN));
                //if (trckEntity.ENTRY != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_ENTRY", trckEntity.ENTRY));
                //if (trckEntity.NEXTYN != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_NEXT_YN", trckEntity.NEXTYN));
                //if (trckEntity.NEXTTASK != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_NEXT_TASK", trckEntity.NEXTTASK));
                //if (trckEntity.NEXTDAYS != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_NEXT_DAYS", trckEntity.NEXTDAYS));
                //if (trckEntity.RESPONSE != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE", trckEntity.RESPONSE));
                //if (trckEntity.RESPONSEVAL1 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_VAL1", trckEntity.RESPONSEVAL1));
                //if (trckEntity.RESPONSELIT1 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_LIT1", trckEntity.RESPONSELIT1));
                //if (trckEntity.RESPONSEVAL2 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_VAL2", trckEntity.RESPONSEVAL2));
                //if (trckEntity.RESPONSELIT2 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_LIT2", trckEntity.RESPONSELIT2));
                //if (trckEntity.RESPONSEVAL3 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_VAL3", trckEntity.RESPONSEVAL3));
                //if (trckEntity.RESPONSELIT3 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_LIT3", trckEntity.RESPONSELIT3));
                //if (trckEntity.RESPONSEVAL4 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_VAL4", trckEntity.RESPONSEVAL4));
                //if (trckEntity.RESPONSELIT4 != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_RESPONSE_LIT4", trckEntity.RESPONSELIT4));
                //if (trckEntity.NXTASKDESC != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_NXTASK_DESC", trckEntity.NXTASKDESC));
                //if (trckEntity.QUESTION != string.Empty) 
                //    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION", trckEntity.QUESTION));

                if (trckEntity.COMPONENTOWNER1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPONENT_OWNER1", trckEntity.COMPONENTOWNER1));
                if (trckEntity.COMPONENTOWNER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPONENT_OWNER2", trckEntity.COMPONENTOWNER2));
                if (trckEntity.COMPONENTOWNER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPONENT_OWNER3", trckEntity.COMPONENTOWNER3));

                if (trckEntity.CustQCodes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_CUSTQ_CODES", trckEntity.CustQCodes));
               
                if (trckEntity.COUNT != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COUNT", trckEntity.COUNT));
                if (trckEntity.GCHARTCODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_GCHART_CODE", trckEntity.GCHARTCODE));
                if (trckEntity.QUESTIONR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_R", trckEntity.QUESTIONR));
                if (trckEntity.QUESTIONE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_E", trckEntity.QUESTIONE));
                if (trckEntity.CASENOTESR  != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_R", trckEntity.CASENOTESR));
                if (trckEntity.CASENOTESE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_E", trckEntity.CASENOTESE));
                if (trckEntity.SBCBR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_R", trckEntity.SBCBR));
                if (trckEntity.SBCBE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_E", trckEntity.SBCBE));
                if (trckEntity.ADDRESSR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_R", trckEntity.ADDRESSR));
                if (trckEntity.ADDRESSE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_E", trckEntity.ADDRESSE));
                if (trckEntity.COMPLETER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_R", trckEntity.COMPLETER));
                if (trckEntity.COMPLETEE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_E", trckEntity.COMPLETEE));
                if (trckEntity.FOLLOWUPR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_R", trckEntity.FOLLOWUPR));
                if (trckEntity.FOLLOWUPE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_E", trckEntity.FOLLOWUPE));
                if (trckEntity.FOLLOWUPCR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_R", trckEntity.FOLLOWUPCR));
                if (trckEntity.FOLLOWUPCE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_E", trckEntity.FOLLOWUPCE));
                if (trckEntity.DIAGNOSER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_R", trckEntity.DIAGNOSER));
                if (trckEntity.DIAGNOSEE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_E", trckEntity.DIAGNOSEE));
                if (trckEntity.SSRR != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_R", trckEntity.SSRR));
                if (trckEntity.SSRE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_E", trckEntity.SSRE));
                if (trckEntity.WHERER != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_WHERE_R", trckEntity.WHERER));
                if (trckEntity.WHEREE != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_WHERE_E", trckEntity.WHEREE));
                if (trckEntity.FUNDR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FUND_R", trckEntity.FUNDR));
                if (trckEntity.FUNDE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FUND_E", trckEntity.FUNDE));
                if (trckEntity.QUESTIONR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_R1", trckEntity.QUESTIONR1));
                if (trckEntity.CASENOTESR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_R1", trckEntity.CASENOTESR1));
                if (trckEntity.SBCBR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_R1", trckEntity.SBCBR1));
                if (trckEntity.ADDRESSR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_R1", trckEntity.ADDRESSR1));
                if (trckEntity.COMPLETER1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_R1", trckEntity.COMPLETER1));
                if (trckEntity.FOLLOWUPR1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_R1", trckEntity.FOLLOWUPR1));
                if (trckEntity.FOLLOWUPCR1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_R1", trckEntity.FOLLOWUPCR1));
                if (trckEntity.DIAGNOSER1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_R1", trckEntity.DIAGNOSER1));
                if (trckEntity.SSRR1 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_R1", trckEntity.SSRR1));
                if (trckEntity.QUESTIONR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_R2", trckEntity.QUESTIONR2));
                if (trckEntity.CASENOTESR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_R2", trckEntity.CASENOTESR2));
                if (trckEntity.SBCBR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_R2", trckEntity.SBCBR2));
                if (trckEntity.ADDRESSR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_R2", trckEntity.ADDRESSR2));
                if (trckEntity.COMPLETER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_R2", trckEntity.COMPLETER2));
                if (trckEntity.FOLLOWUPR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_R2", trckEntity.FOLLOWUPR2));
                if (trckEntity.FOLLOWUPCR2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_R2", trckEntity.FOLLOWUPCR2));
                if (trckEntity.DIAGNOSER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_R2", trckEntity.DIAGNOSER2));
                if (trckEntity.SSRR2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_R2", trckEntity.SSRR2));
                if (trckEntity.WHERER2 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_WHERE_R2", trckEntity.WHERER2));
                if (trckEntity.QUESTIONR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_R3", trckEntity.QUESTIONR3));
                if (trckEntity.CASENOTESR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_R3", trckEntity.CASENOTESR3));
                if (trckEntity.SBCBR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_R3", trckEntity.SBCBR3));
                if (trckEntity.ADDRESSR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_R3", trckEntity.ADDRESSR3));
                if (trckEntity.COMPLETER3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_R3", trckEntity.COMPLETER3));
                if (trckEntity.FOLLOWUPR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_R3", trckEntity.FOLLOWUPR3));
                if (trckEntity.FOLLOWUPCR3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_R3", trckEntity.FOLLOWUPCR3));
                if (trckEntity.DIAGNOSER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_R3", trckEntity.DIAGNOSER3));
                if (trckEntity.SSRR3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_R3", trckEntity.SSRR3));
                if (trckEntity.WHERER3 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_WHERE_R3", trckEntity.WHERER3));
                if (trckEntity.QUESTIONR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_QUESTION_R4", trckEntity.QUESTIONR4));
                if (trckEntity.CASENOTESR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_CASENOTES_R4", trckEntity.CASENOTESR4));
                if (trckEntity.SBCBR4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SBCB_R4", trckEntity.SBCBR4));
                if (trckEntity.ADDRESSR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_ADDRESS_R4", trckEntity.ADDRESSR4));
                if (trckEntity.COMPLETER4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_COMPLETE_R4", trckEntity.COMPLETER4));
                if (trckEntity.FOLLOWUPR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUP_R4", trckEntity.FOLLOWUPR4));
                if (trckEntity.FOLLOWUPCR4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_FOLLOWUPC_R4", trckEntity.FOLLOWUPCR4));
                if (trckEntity.DIAGNOSER4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCK_DIAGNOSE_R4", trckEntity.DIAGNOSER4));
                if (trckEntity.SSRR4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_SSR_R4", trckEntity.SSRR4));
                if (trckEntity.WHERER4 != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@TRCK_WHERE_R4", trckEntity.WHERER4));          
              
                    sqlParamList.Add(new SqlParameter("@TRCK_ADD_OPERATOR", trckEntity.ADDOPERATOR)); 
                    sqlParamList.Add(new SqlParameter("@TRCK_LSTC_OPERATOR", trckEntity.LSTCOPERATOR));


                sqlParamList.Add(new SqlParameter("@Mode", trckEntity.Mode));
                boolSuccess = ChldTrckDB.InsertUpdateDelChldTrck(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public List<ChldTrckEntity> GetCasetrckDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string TaskDescription)
        {
            List<ChldTrckEntity> CaseTrckDetails = new List<ChldTrckEntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.ChldTrckDB.GetChldTrckDetails(ApplType, ApplCode, ApplProg, Component, Task, TaskDescription);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckDetails.Add(new ChldTrckEntity(row));
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


        public bool InsertUpdateDelChldTrckR(ChldTrckREntity trckrEntity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (trckrEntity.Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_AGENCY", trckrEntity.Agency));
                if (trckrEntity.Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_DEPT", trckrEntity.Dept));
                if (trckrEntity.Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_PROG", trckrEntity.Prog));
                if (trckrEntity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_COMPONENT", trckrEntity.COMPONENT));
                if (trckrEntity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_TASK", trckrEntity.TASK));
                if (trckrEntity.FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_FUND", trckrEntity.FUND));
                if (trckrEntity.REQUIREYEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_REQUIRE_EVERYEAR", trckrEntity.REQUIREYEAR));
                if (trckrEntity.INTAKEENTRY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_INTAKE_ENTRY", trckrEntity.INTAKEENTRY));
                if (trckrEntity.ENTERDAYS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_ENTRY_DAYS", trckrEntity.ENTERDAYS));
                if (trckrEntity.NXTACTION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_NXTACTION", trckrEntity.NXTACTION));
                if (trckrEntity.NEXTTASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_NEXT_TASK", trckrEntity.NEXTTASK));
                if (trckrEntity.NEXTDAYS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TRCKR_NEXT_DAYS", trckrEntity.NEXTDAYS));
            

                sqlParamList.Add(new SqlParameter("@TRCKR_ADD_OPERATOR", trckrEntity.ADDOPERATOR));
                sqlParamList.Add(new SqlParameter("@TRCKR_LSTC_OPERATOR", trckrEntity.LSTCOPERATOR));


                sqlParamList.Add(new SqlParameter("@Mode", trckrEntity.Mode));



                boolSuccess = ChldTrckDB.InsertUpdateDelChldTrckR(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public List<ChldTrckREntity> GetCasetrckrDetails(string ApplType, string ApplCode, string ApplProg, string Component, string Task, string TaskFund)
        {
            List<ChldTrckREntity> CaseTrckRDetails = new List<ChldTrckREntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.ChldTrckDB.GetChldTrckRDetails(ApplType, ApplCode, ApplProg, Component, Task, TaskFund);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckRDetails.Add(new ChldTrckREntity(row));
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



        public bool InsertUpdateDelChldmedi(ChldMediEntity chldmedientity,out string seq)
        {
            string strOutSeq = string.Empty;     
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (chldmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_AGENCY", chldmedientity.AGENCY));
                if (chldmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_DEPT", chldmedientity.DEPT));
                if (chldmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_PROG", chldmedientity.PROG));
                if (chldmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_YEAR", chldmedientity.YEAR));
                if (chldmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_APP_NO", chldmedientity.APP_NO));
                if (chldmedientity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_TASK", chldmedientity.TASK));
                if (chldmedientity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SEQ", chldmedientity.SEQ));
                if (chldmedientity.ADDRESSED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_ADDRESSED_DATE", chldmedientity.ADDRESSED_DATE));
                if (chldmedientity.FOLLOWUP_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_FOLLOWUP_DATE", chldmedientity.FOLLOWUP_DATE));
                if (chldmedientity.COMPLETED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_COMPLETED_DATE", chldmedientity.COMPLETED_DATE));
                if (chldmedientity.SBCB_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SBCB_DATE", chldmedientity.SBCB_DATE));
                if (chldmedientity.COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_COMPONENT", chldmedientity.COMPONENT));
                if (chldmedientity.SN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SN", chldmedientity.SN));
                if (chldmedientity.ANSWER1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_ANSWER1", chldmedientity.ANSWER1));
                if (chldmedientity.ANSWER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_ANSWER2", chldmedientity.ANSWER2));
                if (chldmedientity.ANSWER3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_ANSWER3", chldmedientity.ANSWER3));
                if (chldmedientity.DIAGNOSIS_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_DIAGNOSIS_DATE", chldmedientity.DIAGNOSIS_DATE));
                if (chldmedientity.SPECIAL_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SPECIAL_DATE", chldmedientity.SPECIAL_DATE));
                if (chldmedientity.SPECIAL_WHERE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SPECIAL_WHERE", chldmedientity.SPECIAL_WHERE));
                if (chldmedientity.FOLLOWUPC_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_FOLLOWUPC_DATE", chldmedientity.FOLLOWUPC_DATE));
                sqlParamList.Add(new SqlParameter("@MEDI_ADD_OPERATOR", chldmedientity.ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@MEDI_LSTC_OPERATOR", chldmedientity.LSTC_OPERATOR));
                if (chldmedientity.MediFund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_FUND", chldmedientity.MediFund));
                sqlParamList.Add(new SqlParameter("@Mode", chldmedientity.Mode));                
                SqlParameter sqlOutSeq= new SqlParameter("@OutSeq", SqlDbType.Int);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);
                boolSuccess = ChldTrckDB.InsertUpdateDelChldMedi(sqlParamList);
                strOutSeq = sqlOutSeq.Value.ToString();
            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }


            seq = strOutSeq;
            return boolSuccess;
        }

        public List<ChldMediEntity> GetChldMediDetails(string Agency, string Dept, string Prog,string Year, string ApplNo, string Task, string Seq)
        {
            List<ChldMediEntity> ChldMediDetails = new List<ChldMediEntity>();
            try
            {
                DataSet ChldMedids = Captain.DatabaseLayer.ChldTrckDB.GetChldMediDetails(Agency, Dept, Prog, Year, ApplNo, Task, Seq);
                if (ChldMedids != null && ChldMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMedids.Tables[0].Rows)
                    {
                        ChldMediDetails.Add(new ChldMediEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMediDetails;
            }

            return ChldMediDetails;
        }
        public List<ChldMediEntity> GetChldMediDetails1(string Agency, string Dept, string Prog, string Year, string ApplNo, string Task, string Seq)
        {
            List<ChldMediEntity> ChldMediDetails = new List<ChldMediEntity>();
            try
            {
                DataSet ChldMedids = Captain.DatabaseLayer.ChldTrckDB.GetChldMediDetails(Agency, Dept, Prog, Year, ApplNo, Task, Seq);
                if (ChldMedids != null && ChldMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMedids.Tables[0].Rows)
                    {
                        if (row["TSKUPLD_ID"].ToString() != "")
                        {
                            string xx = "00";
                        }

                        ChldMediDetails.Add(new ChldMediEntity(row,0));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMediDetails;
            }

            return ChldMediDetails;
        }


        public bool InsertUpdateDelChldmedResp(ChldMedRespEntity chldmedientity)
        {
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (chldmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_AGENCY", chldmedientity.AGENCY));
                if (chldmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_DEPT", chldmedientity.DEPT));
                if (chldmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_PROG", chldmedientity.PROG));
                if (chldmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_YEAR", chldmedientity.YEAR));
                if (chldmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_APP_NO", chldmedientity.APP_NO));               
                //  if (chldmedientity.TASK != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MEDRSP_TASK", chldmedientity.TASK));
                //if (chldmedientity.SEQ != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MEDRSP_SEQ", chldmedientity.SEQ));
                if (chldmedientity.QUE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_Que", chldmedientity.QUE));
                if (chldmedientity.RESP_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_RESP_SEQ", chldmedientity.RESP_SEQ));
                if (chldmedientity.NUM_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_NUM_RESP", chldmedientity.NUM_RESP));
                if (chldmedientity.ALPHA_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_ALPHA_RESP", chldmedientity.ALPHA_RESP));
                if (chldmedientity.DATE_RESP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDRSP_DATE_RESP", chldmedientity.DATE_RESP));
                sqlParamList.Add(new SqlParameter("@MEDRSP_ADD_OPERATOR", chldmedientity.ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@MEDRSP_LSTC_OPERATOR", chldmedientity.LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", chldmedientity.Mode));
                boolSuccess = ChldTrckDB.InsertUpdateDelChldMedResp(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public bool INSUPDEL_TASKUPLDS(string TSKUPLD_ID, string TSKUPLD_AGENCY, string TSKUPLD_DEPT, string TSKUPLD_PROGRAM, string TSKUPLD_YEAR, string TSKUPLD_APPNO
        , string TSKUPLD_TASK, string TSKUPLD_ORIG_NAME, string TSKUPLD_UPLOAD_AS, string TSKUPLD_UPLOAD_BY,  string TSKUPLD_DELETED_BY, string TSKUPLD_LSTC_OPERATOR
        , string MEDI_SEQNO, string MEDI_COMPONENT, string DOCFORMAT, string MODE, out string seq)
        {
            string strOutSeq = string.Empty;
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (TSKUPLD_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_ID", TSKUPLD_ID));
                if (TSKUPLD_AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_AGENCY", TSKUPLD_AGENCY));
                if (TSKUPLD_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_DEPT", TSKUPLD_DEPT));
                if (TSKUPLD_PROGRAM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_PROGRAM", TSKUPLD_PROGRAM));
                if (TSKUPLD_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_YEAR", TSKUPLD_YEAR));
                if (TSKUPLD_APPNO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_APPNO", TSKUPLD_APPNO));
                if (TSKUPLD_TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_TASK", TSKUPLD_TASK));
                if (TSKUPLD_ORIG_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_ORIG_NAME", TSKUPLD_ORIG_NAME));
              
                if (TSKUPLD_UPLOAD_AS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_UPLOAD_AS", TSKUPLD_UPLOAD_AS));

                if (DOCFORMAT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DOCFORMAT", DOCFORMAT));

                //***********************************************************************************//

                if (TSKUPLD_UPLOAD_BY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_UPLOAD_BY", TSKUPLD_UPLOAD_BY));

                if (TSKUPLD_DELETED_BY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_DELETED_BY", TSKUPLD_DELETED_BY));

                if (TSKUPLD_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TSKUPLD_LSTC_OPERATOR", TSKUPLD_LSTC_OPERATOR));

                //***********************************************************************************//

                if (MEDI_SEQNO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SEQNO", MEDI_SEQNO));

                if (MEDI_COMPONENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_COMPONENT", MEDI_COMPONENT));

                sqlParamList.Add(new SqlParameter("@MODE", MODE));

                SqlParameter sqlOutSeq = new SqlParameter("@OutSeq", SqlDbType.Int);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);
                boolSuccess = ChldTrckDB.INSUPDEL_TASKUPLDS(sqlParamList);
                strOutSeq = sqlOutSeq.Value.ToString();

                
            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }

            seq = strOutSeq;
            return boolSuccess;
        }




        public List<ChldMedRespEntity> GetChldMedRespDetails(string Agency, string Dept, string Prog, string Year, string ApplNo, string Task, string Seq,string Que)
        {
            List<ChldMedRespEntity> ChldMedRespDetails = new List<ChldMedRespEntity>();
            try
            {
                DataSet ChldMedids = Captain.DatabaseLayer.ChldTrckDB.GetChldMedRespDetails(Agency, Dept, Prog, Year, ApplNo,Que);
                if (ChldMedids != null && ChldMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMedids.Tables[0].Rows)
                    {
                        ChldMedRespDetails.Add(new ChldMedRespEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMedRespDetails;
            }

            return ChldMedRespDetails;
        }


        public List<ChldTrckEntity> Browse_CasetrckDetails(string RepSeq)
        {
            List<ChldTrckEntity> CaseTrckDetails = new List<ChldTrckEntity>();
            try
            {
                DataSet Casetrck = Captain.DatabaseLayer.ChldTrckDB.BrowseTrckDetails(RepSeq);
                if (Casetrck != null && Casetrck.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Casetrck.Tables[0].Rows)
                    {
                        CaseTrckDetails.Add(new ChldTrckEntity(row));
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

        public List<HSSB2106Report_Entity> GetChldTrck_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Enrl_Stat, string FrmAge, string ToAge, string SortCol)
        {
            List<HSSB2106Report_Entity> TemplateDetails = new List<HSSB2106Report_Entity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.ChldTrckDB.GetHSSB2106_Report(Agency, Dept, Prog, Year, AppNo, Age, MulSites, FundHie,Active, Enrl_Stat, FrmAge, ToAge, SortCol);
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

        public List<HSSB2106Report_Entity> GetHSSB2124_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string Age, string MulSites, string FundHie, string Active, string Enrl_Stat, string FrmAge, string ToAge, string SortCol,string ExcludeIntakes)
        {
            List<HSSB2106Report_Entity> TemplateDetails = new List<HSSB2106Report_Entity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.ChldTrckDB.GetHSSB0124_GridApp(Agency, Dept, Prog, Year, AppNo, Age, MulSites, FundHie, Active, Enrl_Stat, FrmAge, ToAge, SortCol, ExcludeIntakes);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new HSSB2106Report_Entity(row,"HSSB0124"));
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

        public List<ChldMediEntity> GetChldMedi_Report(string Agency, string Dept, string Prog, string Year, string AppNo, string SBCB_Date,string Task,string Type,string strsequnce=null)
        {
            List<ChldMediEntity> TemplateDetails = new List<ChldMediEntity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.ChldTrckDB.GetHSSB2106_ChldMedi(Agency, Dept, Prog, Year, AppNo, SBCB_Date, Task, Type, strsequnce);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new ChldMediEntity(row,string.Empty));
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


        public bool Updatemedifix(ChldMediEntity chldmedientity)
        {
            string strOutSeq = string.Empty;
            bool boolSuccess = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (chldmedientity.AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_AGENCY", chldmedientity.AGENCY));
                if (chldmedientity.DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_DEPT", chldmedientity.DEPT));
                if (chldmedientity.PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_PROG", chldmedientity.PROG));
                if (chldmedientity.YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_YEAR", chldmedientity.YEAR));
                if (chldmedientity.APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_APP_NO", chldmedientity.APP_NO));
                if (chldmedientity.TASK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_TASK", chldmedientity.TASK));
                if (chldmedientity.SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_SEQ", chldmedientity.SEQ));                
                if (chldmedientity.ANSWER2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MEDI_ANSWER2", chldmedientity.ANSWER2));
              
                boolSuccess = ChldTrckDB.UpdateMediFix(sqlParamList);
               
            }
            catch (Exception ex)
            {
                boolSuccess = false;
            }


           
            return boolSuccess;
        }

        public List<ChldMediEntity> GetMediFix()
        {
            List<ChldMediEntity> ChldMediDetails = new List<ChldMediEntity>();
            try
            {
                DataSet ChldMedids = Captain.DatabaseLayer.ChldTrckDB.GetMediFix();
                if (ChldMedids != null && ChldMedids.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMedids.Tables[0].Rows)
                    {
                        ChldMediDetails.Add(new ChldMediEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMediDetails;
            }

            return ChldMediDetails;
        }


    }
}
