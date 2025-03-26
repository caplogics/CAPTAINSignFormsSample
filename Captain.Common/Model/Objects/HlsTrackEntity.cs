using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class HlsTrckEntity
    {
        #region Constructors

        public HlsTrckEntity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            COMPONENT = string.Empty;
            TASK = string.Empty;
            TASKDESCRIPTION = string.Empty;
            ENTRYYN = string.Empty;
            ENTRY = string.Empty;
            NEXTYN = string.Empty;
            NEXTTASK = string.Empty;
            NEXTDAYS = string.Empty;
            RESPONSE = string.Empty;
            RESPONSEVAL1 = string.Empty;
            RESPONSELIT1 = string.Empty;
            RESPONSEVAL2 = string.Empty;
            RESPONSELIT2 = string.Empty;
            RESPONSEVAL3 = string.Empty;
            RESPONSELIT3 = string.Empty;
            RESPONSEVAL4 = string.Empty;
            RESPONSELIT4 = string.Empty;
            NXTASKDESC = string.Empty;
            QUESTION = string.Empty;
            COMPONENTOWNER1 = string.Empty;
            COMPONENTOWNER2 = string.Empty;
            COMPONENTOWNER3 = string.Empty;
            ENTRYEVERYYEAR = string.Empty;
            COUNT = string.Empty;
            GCHARTCODE = string.Empty;
            GCHARTSEL = string.Empty;
            QUESTIONR = string.Empty;
            QUESTIONE = string.Empty;
            CASENOTESR = string.Empty;
            CASENOTESE = string.Empty;
            SBCBR = string.Empty;
            SBCBE = string.Empty;
            ADDRESSR = string.Empty;
            ADDRESSE = string.Empty;
            COMPLETER = string.Empty;
            COMPLETEE = string.Empty;
            FOLLOWUPR = string.Empty;
            FOLLOWUPE = string.Empty;
            FOLLOWUPCR = string.Empty;
            FOLLOWUPCE = string.Empty;
            DIAGNOSER = string.Empty;
            DIAGNOSEE = string.Empty;
            SSRR = string.Empty;
            SSRE = string.Empty;
            WHERER = string.Empty;
            WHEREE = string.Empty;
            QUESTIONR1 = string.Empty;
            CASENOTESR1 = string.Empty;
            SBCBR1 = string.Empty;
            ADDRESSR1 = string.Empty;
            COMPLETER1 = string.Empty;
            FOLLOWUPR1 = string.Empty;
            FOLLOWUPCR1 = string.Empty;
            DIAGNOSER1 = string.Empty;
            SSRR1 = string.Empty;
            WHERER1 = string.Empty;
            QUESTIONR2 = string.Empty;
            CASENOTESR2 = string.Empty;
            SBCBR2 = string.Empty;
            ADDRESSR2 = string.Empty;
            COMPLETER2 = string.Empty;
            FOLLOWUPR2 = string.Empty;
            FOLLOWUPCR2 = string.Empty;
            DIAGNOSER2 = string.Empty;
            SSRR2 = string.Empty;
            WHERER2 = string.Empty;
            QUESTIONR3 = string.Empty;
            CASENOTESR3 = string.Empty;
            SBCBR3 = string.Empty;
            ADDRESSR3 = string.Empty;
            COMPLETER3 = string.Empty;
            FOLLOWUPR3 = string.Empty;
            FOLLOWUPCR3 = string.Empty;
            DIAGNOSER3 = string.Empty;
            SSRR3 = string.Empty;
            WHERER3 = string.Empty;
            QUESTIONR4 = string.Empty;
            CASENOTESR4 = string.Empty;
            SBCBR4 = string.Empty;
            ADDRESSR4 = string.Empty;
            COMPLETER4 = string.Empty;
            FOLLOWUPR4 = string.Empty;
            FOLLOWUPCR4 = string.Empty;
            DIAGNOSER4 = string.Empty;
            SSRR4 = string.Empty;
            WHERER4 = string.Empty;
            DATEADD = string.Empty;
            ADDOPERATOR = string.Empty;
            DATELSTC = string.Empty;
            LSTCOPERATOR = string.Empty;
            CustQCodes = string.Empty;

            Mode = string.Empty;
            Rep_Seq = string.Empty;
        }

        public HlsTrckEntity(bool Initialize)
        {
            Rec_Type = null;
            Agency = null;
            Dept = null;
            Prog = null;
            COMPONENT = null;
            TASK = null;
            TASKDESCRIPTION = null;
            ENTRYYN = null;
            ENTRY = null;
            NEXTYN = null;
            NEXTTASK = null;
            NEXTDAYS = null;
            RESPONSE = null;
            RESPONSEVAL1 = null;
            RESPONSELIT1 = null;
            RESPONSEVAL2 = null;
            RESPONSELIT2 = null;
            RESPONSEVAL3 = null;
            RESPONSELIT3 = null;
            RESPONSEVAL4 = null;
            RESPONSELIT4 = null;
            NXTASKDESC = null;
            QUESTION = null;
            COMPONENTOWNER1 = null;
            COMPONENTOWNER2 = null;
            COMPONENTOWNER3 = null;
            ENTRYEVERYYEAR = null;
            COUNT = null;
            GCHARTCODE = null;
            GCHARTSEL = null;
            QUESTIONR = null;
            QUESTIONE = null;
            CASENOTESR = null;
            CASENOTESE = null;
            SBCBR = null;
            SBCBE = null;
            ADDRESSR = null;
            ADDRESSE = null;
            COMPLETER = null;
            COMPLETEE = null;
            FOLLOWUPR = null;
            FOLLOWUPE = null;
            FOLLOWUPCR = null;
            FOLLOWUPCE = null;
            DIAGNOSER = null;
            DIAGNOSEE = null;
            SSRR = null;
            SSRE = null;
            WHERER = null;
            WHEREE = null;
            QUESTIONR1 = null;
            CASENOTESR1 = null;
            SBCBR1 = null;
            ADDRESSR1 = null;
            COMPLETER1 = null;
            FOLLOWUPR1 = null;
            FOLLOWUPCR1 = null;
            DIAGNOSER1 = null;
            SSRR1 = null;
            WHERER1 = null;
            QUESTIONR2 = null;
            CASENOTESR2 = null;
            SBCBR2 = null;
            ADDRESSR2 = null;
            COMPLETER2 = null;
            FOLLOWUPR2 = null;
            FOLLOWUPCR2 = null;
            DIAGNOSER2 = null;
            SSRR2 = null;
            WHERER2 = null;
            QUESTIONR3 = null;
            CASENOTESR3 = null;
            SBCBR3 = null;
            ADDRESSR3 = null;
            COMPLETER3 = null;
            FOLLOWUPR3 = null;
            FOLLOWUPCR3 = null;
            DIAGNOSER3 = null;
            SSRR3 = null;
            WHERER3 = null;
            QUESTIONR4 = null;
            CASENOTESR4 = null;
            SBCBR4 = null;
            ADDRESSR4 = null;
            COMPLETER4 = null;
            FOLLOWUPR4 = null;
            FOLLOWUPCR4 = null;
            DIAGNOSER4 = null;
            SSRR4 = null;
            WHERER4 = null;
            DATEADD = null;
            ADDOPERATOR = null;
            DATELSTC = null;
            LSTCOPERATOR = null;
            Mode = null;
            Rep_Seq = null;
        }

        public HlsTrckEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["HLSTRCK_Agency"].ToString();
                Dept = row["HLSTRCK_Dept"].ToString();
                Prog = row["HLSTRCK_PROGRAM"].ToString();
                COMPONENT = row["HLSTRCK_COMPONENT"].ToString().Trim();
                TASK = row["HLSTRCK_TASK"].ToString();
                TASKDESCRIPTION = row["HLSTRCK_TASK_DESC"].ToString();
                SEQ = row["HLSTRCK_SEQ"].ToString();
                //ENTRYYN = row["HLSTRCK_ENTRY_YN"].ToString();
                //ENTRY = row["HLSTRCK_ENTRY"].ToString();
                //NEXTYN = row["HLSTRCK_NEXT_YN"].ToString();
                //NEXTTASK = row["HLSTRCK_NEXT_TASK"].ToString();
                //NEXTDAYS = row["HLSTRCK_NEXT_DAYS"].ToString();
                //RESPONSE = row["HLSTRCK_RESPONSE"].ToString();
                //RESPONSEVAL1 = row["HLSTRCK_RESPONSE_VAL1"].ToString();
                //RESPONSELIT1 = row["HLSTRCK_RESPONSE_LIT1"].ToString();
                //RESPONSEVAL2 = row["HLSTRCK_RESPONSE_VAL2"].ToString();
                //RESPONSELIT2 = row["HLSTRCK_RESPONSE_LIT2"].ToString();
                //RESPONSEVAL3 = row["HLSTRCK_RESPONSE_VAL3"].ToString();
                //RESPONSELIT3 = row["HLSTRCK_RESPONSE_LIT3"].ToString();
                //RESPONSEVAL4 = row["HLSTRCK_RESPONSE_VAL4"].ToString();
                //RESPONSELIT4 = row["HLSTRCK_RESPONSE_LIT4"].ToString();               
                //QUESTION = row["HLSTRCK_QUESTION"].ToString();
                //ENTRYEVERYYEAR = row["HLSTRCK_ENTRY_EVERYYEAR"].ToString();
                COMPONENTOWNER1 = row["HLSTRCK_COMPNT_OWNER1"].ToString();
                COMPONENTOWNER2 = row["HLSTRCK_COMPNT_OWNER2"].ToString();
                COMPONENTOWNER3 = row["HLSTRCK_COMPNT_OWNER3"].ToString();
                COUNT = row["HLSTRCK_COUNT"].ToString();
                GCHARTCODE = row["HLSTRCK_GCHART_CODE"].ToString();
                GCHARTSEL = row["HLSTRCK_GCHART_SEL"].ToString();
                QUESTIONR = row["HLSTRCK_QUESTION_R"].ToString();
                QUESTIONE = row["HLSTRCK_QUESTION_E"].ToString();
                CASENOTESR = row["HLSTRCK_CASENOTES_R"].ToString();
                CASENOTESE = row["HLSTRCK_CASENOTES_E"].ToString();
                SBCBR = row["HLSTRCK_SBCB_R"].ToString();
                SBCBE = row["HLSTRCK_SBCB_E"].ToString();
                ADDRESSR = row["HLSTRCK_ADDRESS_R"].ToString();
                ADDRESSE = row["HLSTRCK_ADDRESS_E"].ToString();
                COMPLETER = row["HLSTRCK_COMPLETE_R"].ToString();
                COMPLETEE = row["HLSTRCK_COMPLETE_E"].ToString();
                FOLLOWUPR = row["HLSTRCK_FOLLOWUP_R"].ToString();
                FOLLOWUPE = row["HLSTRCK_FOLLOWUP_E"].ToString();
                FOLLOWUPCR = row["HLSTRCK_FOLLOWUPC_R"].ToString();
                FOLLOWUPCE = row["HLSTRCK_FOLLOWUPC_E"].ToString();
                DIAGNOSER = row["HLSTRCK_DIAGNOSE_R"].ToString();
                DIAGNOSEE = row["HLSTRCK_DIAGNOSE_E"].ToString();
                SSRR = row["HLSTRCK_SSR_R"].ToString();
                SSRE = row["HLSTRCK_SSR_E"].ToString();
                WHERER = row["HLSTRCK_WHERE_R"].ToString();
                WHEREE = row["HLSTRCK_WHERE_E"].ToString();
                QUESTIONR1 = row["HLSTRCK_QUESTION_R1"].ToString();
                CASENOTESR1 = row["HLSTRCK_CASENOTES_R1"].ToString();
                SBCBR1 = row["HLSTRCK_SBCB_R1"].ToString();
                ADDRESSR1 = row["HLSTRCK_ADDRESS_R1"].ToString();
                COMPLETER1 = row["HLSTRCK_COMPLETE_R1"].ToString();
                FOLLOWUPR1 = row["HLSTRCK_FOLLOWUP_R1"].ToString();
                FOLLOWUPCR1 = row["HLSTRCK_FOLLOWUPC_R1"].ToString();
                DIAGNOSER1 = row["HLSTRCK_DIAGNOSE_R1"].ToString();
                SSRR1 = row["HLSTRCK_SSR_R1"].ToString();
                WHERER1 = row["HLSTRCK_WHERE_R1"].ToString();
                QUESTIONR2 = row["HLSTRCK_QUESTION_R2"].ToString();
                CASENOTESR2 = row["HLSTRCK_CASENOTES_R2"].ToString();
                SBCBR2 = row["HLSTRCK_SBCB_R2"].ToString();
                ADDRESSR2 = row["HLSTRCK_ADDRESS_R2"].ToString();
                COMPLETER2 = row["HLSTRCK_COMPLETE_R2"].ToString();
                FOLLOWUPR2 = row["HLSTRCK_FOLLOWUP_R2"].ToString();
                FOLLOWUPCR2 = row["HLSTRCK_FOLLOWUPC_R2"].ToString();
                DIAGNOSER2 = row["HLSTRCK_DIAGNOSE_R2"].ToString();
                SSRR2 = row["HLSTRCK_SSR_R2"].ToString();
                WHERER2 = row["HLSTRCK_WHERE_R2"].ToString();
                QUESTIONR3 = row["HLSTRCK_QUESTION_R3"].ToString();
                CASENOTESR3 = row["HLSTRCK_CASENOTES_R3"].ToString();
                SBCBR3 = row["HLSTRCK_SBCB_R3"].ToString();
                ADDRESSR3 = row["HLSTRCK_ADDRESS_R3"].ToString();
                COMPLETER3 = row["HLSTRCK_COMPLETE_R3"].ToString();
                FOLLOWUPR3 = row["HLSTRCK_FOLLOWUP_R3"].ToString();
                FOLLOWUPCR3 = row["HLSTRCK_FOLLOWUPC_R3"].ToString();
                DIAGNOSER3 = row["HLSTRCK_DIAGNOSE_R3"].ToString();
                SSRR3 = row["HLSTRCK_SSR_R3"].ToString();
                WHERER3 = row["HLSTRCK_WHERE_R3"].ToString();
                QUESTIONR4 = row["HLSTRCK_QUESTION_R4"].ToString();
                CASENOTESR4 = row["HLSTRCK_CASENOTES_R4"].ToString();
                SBCBR4 = row["HLSTRCK_SBCB_R4"].ToString();
                ADDRESSR4 = row["HLSTRCK_ADDRESS_R4"].ToString();
                COMPLETER4 = row["HLSTRCK_COMPLETE_R4"].ToString();
                FOLLOWUPR4 = row["HLSTRCK_FOLLOWUP_R4"].ToString();
                FOLLOWUPCR4 = row["HLSTRCK_FOLLOWUPC_R4"].ToString();
                DIAGNOSER4 = row["HLSTRCK_DIAGNOSE_R4"].ToString();
                SSRR4 = row["HLSTRCK_SSR_R4"].ToString();
                WHERER4 = row["HLSTRCK_WHERE_R4"].ToString();
                DATEADD = row["HLSTRCK_DATE_ADD"].ToString();
                ADDOPERATOR = row["HLSTRCK_ADD_OPERATOR"].ToString();
                DATELSTC = row["HLSTRCK_DATE_LSTC"].ToString();
                LSTCOPERATOR = row["HLSTRCK_LSTC_OPERATOR"].ToString();
                CustQCodes = row["HLSTRCK_CUSTQ_CODES"].ToString();
                Mode = string.Empty;
                Rep_Seq = string.Empty;
            }
        }


        public HlsTrckEntity(string Task, string strTaskDesc, string strEveryYear, string strEntrydays, string NextYr, string NextTask, string strNextDays, string sbcbDate, string CompletDt, string FollowupDt, string AddresDt, string strIntakeEntry, string strCustQues)
        {
            TASK = Task;
            TASKDESCRIPTION = strTaskDesc;
            ENTRYYN = strEveryYear;
            ENTRY = strEntrydays;
            NEXTYN = NextYr;
            NEXTTASK = NextTask;
            NEXTDAYS = strNextDays;
            SBCBDT = sbcbDate;
            CompletDT = CompletDt;
            AddressDT = AddresDt;
            FollowDT = FollowupDt;
            IntakeType = strIntakeEntry;
            CustQCodes = strCustQues;
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Rep_Seq { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string COMPONENT { get; set; }
        public string TASK { get; set; }
        public string TASKDESCRIPTION { get; set; }
        public string SEQ { get; set; }
        public string ENTRYYN { get; set; }
        public string ENTRY { get; set; }
        public string NEXTYN { get; set; }
        public string NEXTTASK { get; set; }
        public string NEXTDAYS { get; set; }
        public string RESPONSE { get; set; }
        public string RESPONSEVAL1 { get; set; }
        public string RESPONSELIT1 { get; set; }
        public string RESPONSEVAL2 { get; set; }
        public string RESPONSELIT2 { get; set; }
        public string RESPONSEVAL3 { get; set; }
        public string RESPONSELIT3 { get; set; }
        public string RESPONSEVAL4 { get; set; }
        public string RESPONSELIT4 { get; set; }
        public string NXTASKDESC { get; set; }
        public string QUESTION { get; set; }
        public string COMPONENTOWNER1 { get; set; }
        public string COMPONENTOWNER2 { get; set; }
        public string COMPONENTOWNER3 { get; set; }
        public string ENTRYEVERYYEAR { get; set; }
        public string COUNT { get; set; }
        public string GCHARTCODE { get; set; }
        public string GCHARTSEL { get; set; }
        public string QUESTIONR { get; set; }
        public string QUESTIONE { get; set; }
        public string CASENOTESR { get; set; }
        public string CASENOTESE { get; set; }
        public string SBCBR { get; set; }
        public string SBCBE { get; set; }
        public string ADDRESSR { get; set; }
        public string ADDRESSE { get; set; }
        public string COMPLETER { get; set; }
        public string COMPLETEE { get; set; }
        public string FOLLOWUPR { get; set; }
        public string FOLLOWUPE { get; set; }
        public string FOLLOWUPCR { get; set; }
        public string FOLLOWUPCE { get; set; }
        public string DIAGNOSER { get; set; }
        public string DIAGNOSEE { get; set; }
        public string SSRR { get; set; }
        public string SSRE { get; set; }
        public string WHERER { get; set; }
        public string WHEREE { get; set; }
        public string QUESTIONR1 { get; set; }
        public string CASENOTESR1 { get; set; }
        public string SBCBR1 { get; set; }
        public string ADDRESSR1 { get; set; }
        public string COMPLETER1 { get; set; }
        public string FOLLOWUPR1 { get; set; }
        public string FOLLOWUPCR1 { get; set; }
        public string DIAGNOSER1 { get; set; }
        public string SSRR1 { get; set; }
        public string WHERER1 { get; set; }
        public string QUESTIONR2 { get; set; }
        public string CASENOTESR2 { get; set; }
        public string SBCBR2 { get; set; }
        public string ADDRESSR2 { get; set; }
        public string COMPLETER2 { get; set; }
        public string FOLLOWUPR2 { get; set; }
        public string FOLLOWUPCR2 { get; set; }
        public string DIAGNOSER2 { get; set; }
        public string SSRR2 { get; set; }
        public string WHERER2 { get; set; }
        public string QUESTIONR3 { get; set; }
        public string CASENOTESR3 { get; set; }
        public string SBCBR3 { get; set; }
        public string ADDRESSR3 { get; set; }
        public string COMPLETER3 { get; set; }
        public string FOLLOWUPR3 { get; set; }
        public string FOLLOWUPCR3 { get; set; }
        public string DIAGNOSER3 { get; set; }
        public string SSRR3 { get; set; }
        public string WHERER3 { get; set; }
        public string QUESTIONR4 { get; set; }
        public string CASENOTESR4 { get; set; }
        public string SBCBR4 { get; set; }
        public string ADDRESSR4 { get; set; }
        public string COMPLETER4 { get; set; }
        public string FOLLOWUPR4 { get; set; }
        public string FOLLOWUPCR4 { get; set; }
        public string DIAGNOSER4 { get; set; }
        public string SSRR4 { get; set; }
        public string WHERER4 { get; set; }
        public string DATEADD { get; set; }
        public string ADDOPERATOR { get; set; }
        public string DATELSTC { get; set; }
        public string LSTCOPERATOR { get; set; }
        public string Mode { get; set; }
        public string CustQCodes { get; set; }
        public string SBCBDT { get; set; }
        public string CompletDT { get; set; }
        public string FollowDT { get; set; }
        public string AddressDT { get; set; }
        public string IntakeType { get; set; }
        #endregion
    }

    public class HlsTrckREntity
    {
        #region Constructors

        public HlsTrckREntity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            COMPONENT = string.Empty;
            TASK = string.Empty;
            FUND = string.Empty;
            REQUIREYEAR = string.Empty;
            INTAKEENTRY = string.Empty;
            ENTERDAYS = string.Empty;
            NEXTTASK = string.Empty;
            NEXTDAYS = string.Empty;
            NXTACTION = string.Empty;
            DATEADD = string.Empty;
            ADDOPERATOR = string.Empty;
            DATELSTC = string.Empty;
            LSTCOPERATOR = string.Empty;
            Mode = string.Empty;
            CompletDt = string.Empty;
        }

        public HlsTrckREntity(bool Initialize)
        {
            Rec_Type = null;
            Agency = null;
            Dept = null;
            Prog = null;
            COMPONENT = null;
            TASK = null;
            FUND = null;
            REQUIREYEAR = null;
            INTAKEENTRY = null;
            ENTERDAYS = null;
            NEXTTASK = null;
            NEXTDAYS = null;
            NXTACTION = null;
            DATEADD = null;
            ADDOPERATOR = null;
            DATELSTC = null;
            LSTCOPERATOR = null;
            Mode = null;
            CompletDt = null;
        }

        public HlsTrckREntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["HLSTRCKR_Agency"].ToString().Trim();
                Dept = row["HLSTRCKR_Dept"].ToString().Trim();
                Prog = row["HLSTRCKR_PROG"].ToString().Trim();
                COMPONENT = row["HLSTRCKR_COMPONENT"].ToString().Trim();
                TASK = row["HLSTRCKR_TASK"].ToString().Trim();
                FUND = row["HLSTRCKR_FUND"].ToString().Trim();
                REQUIREYEAR = row["HLSTRCKR_REQUIRE_EVERYEAR"].ToString().Trim();
                INTAKEENTRY = row["HLSTRCKR_INTAKE_ENTRY"].ToString().Trim();
                ENTERDAYS = row["HLSTRCKR_ENTRY_DAYS"].ToString().Trim();
                NEXTTASK = row["HLSTRCKR_NEXT_TASK"].ToString().Trim();
                NEXTDAYS = row["HLSTRCKR_NEXT_DAYS"].ToString().Trim();
                NXTACTION = row["HLSTRCKR_NXTACTION"].ToString().Trim();
                DATEADD = row["HLSTRCKR_DATE_ADD"].ToString().Trim();
                ADDOPERATOR = row["HLSTRCKR_ADD_OPERATOR"].ToString().Trim();
                DATELSTC = row["HLSTRCKR_DATE_LSTC"].ToString().Trim().Trim();
                LSTCOPERATOR = row["HLSTRCKR_LSTC_OPERATOR"].ToString().Trim();
                Mode = string.Empty;
                CompletDt = string.Empty;
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string COMPONENT { get; set; }
        public string TASK { get; set; }
        public string FUND { get; set; }
        public string REQUIREYEAR { get; set; }
        public string INTAKEENTRY { get; set; }
        public string ENTERDAYS { get; set; }
        public string NEXTTASK { get; set; }
        public string NEXTDAYS { get; set; }
        public string NXTACTION { get; set; }
        public string DATEADD { get; set; }
        public string ADDOPERATOR { get; set; }
        public string DATELSTC { get; set; }
        public string LSTCOPERATOR { get; set; }
        public string Mode { get; set; }
        public string CompletDt { get; set; }
        #endregion
    }


    public class HlsMediEntity
    {
        #region Constructors

        public HlsMediEntity()
        {
            Rec_Type = string.Empty;
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROG = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            TASK = string.Empty;
            SEQ = string.Empty;
            ADDRESSED_DATE = string.Empty;
            FOLLOWUP_DATE = string.Empty;
            COMPLETED_DATE = string.Empty;
            SBCB_DATE = string.Empty;
            COMPONENT = string.Empty;
            SN = string.Empty;
            ANSWER1 = string.Empty;
            ANSWER2 = string.Empty;
            ANSWER3 = string.Empty;
            DIAGNOSIS_DATE = string.Empty;
            SPECIAL_DATE = string.Empty;
            SPECIAL_WHERE = string.Empty;
            FOLLOWUPC_DATE = string.Empty;
            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;
            DATE_LSTC = string.Empty;
            LSTC_OPERATOR = string.Empty;
            Task_Desc = string.Empty;
            TrackSeq = string.Empty;
            Mode = string.Empty;
            XMLANSWER1 = string.Empty;
        }

        public HlsMediEntity(bool Initialize)
        {
            Rec_Type = null;
            AGENCY = null;
            DEPT = null;
            PROG = null;
            YEAR = null;
            APP_NO = null;
            TASK = null;
            SEQ = null;
            ADDRESSED_DATE = null;
            FOLLOWUP_DATE = null;
            COMPLETED_DATE = null;
            SBCB_DATE = null;
            COMPONENT = null;
            SN = null;
            ANSWER1 = null;
            ANSWER2 = null;
            ANSWER3 = null;
            DIAGNOSIS_DATE = null;
            SPECIAL_DATE = null;
            SPECIAL_WHERE = null;
            FOLLOWUPC_DATE = null;
            DATE_ADD = null;
            ADD_OPERATOR = null;
            DATE_LSTC = null;
            LSTC_OPERATOR = null;
            Task_Desc = null;
            TrackSeq = null;
            Mode = null;
            XMLANSWER1 = null;
        }

        public HlsMediEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["HLSMEDI_AGENCY"].ToString();
                DEPT = row["HLSMEDI_DEPT"].ToString();
                PROG = row["HLSMEDI_PROG"].ToString();
                YEAR = row["HLSMEDI_YEAR"].ToString();
                APP_NO = row["HLSMEDI_APP_NO"].ToString();
                TASK = row["HLSMEDI_TASK"].ToString();
                SEQ = row["HLSMEDI_SEQ"].ToString();
                ADDRESSED_DATE = row["HLSMEDI_ADDRESSED_DATE"].ToString();
                FOLLOWUP_DATE = row["HLSMEDI_FOLLOWUP_DATE"].ToString();
                COMPLETED_DATE = row["HLSMEDI_COMPLETED_DATE"].ToString();
                SBCB_DATE = row["HLSMEDI_SBCB_DATE"].ToString();
                COMPONENT = row["HLSMEDI_COMPONENT"].ToString();
                SN = row["HLSMEDI_SN"].ToString();
                ANSWER1 = row["HLSMEDI_ANSWER1"].ToString();
                ANSWER2 = row["HLSMEDI_ANSWER2"].ToString();
                ANSWER3 = row["HLSMEDI_ANSWER3"].ToString();
                DIAGNOSIS_DATE = row["HLSMEDI_DIAGNOSIS_DATE"].ToString();
                SPECIAL_DATE = row["HLSMEDI_SPECIAL_DATE"].ToString();
                SPECIAL_WHERE = row["HLSMEDI_SPECIAL_WHERE"].ToString();
                FOLLOWUPC_DATE = row["HLSMEDI_FOLLOWUPC_DATE"].ToString();
                DATE_ADD = row["HLSMEDI_DATE_ADD"].ToString();
                ADD_OPERATOR = row["HLSMEDI_ADD_OPERATOR"].ToString();
                DATE_LSTC = row["HLSMEDI_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["HLSMEDI_LSTC_OPERATOR"].ToString();
                XMLANSWER1 = row["HLSMEDI_XMLANSWER"].ToString();
                Mode = string.Empty;
            }
        }

        public HlsMediEntity(DataRow row, string strtable)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["HLSMEDI_AGENCY"].ToString();
                DEPT = row["HLSMEDI_DEPT"].ToString();
                PROG = row["HLSMEDI_PROG"].ToString();
                YEAR = row["HLSMEDI_YEAR"].ToString();
                APP_NO = row["HLSMEDI_APP_NO"].ToString();
                TASK = row["HLSMEDI_TASK"].ToString();
                SEQ = row["HLSMEDI_SEQ"].ToString();
                ADDRESSED_DATE = row["HLSMEDI_ADDRESSED_DATE"].ToString();
                FOLLOWUP_DATE = row["HLSMEDI_FOLLOWUP_DATE"].ToString();
                FOLLOWUPC_DATE = row["HLSMEDI_FOLLOWUPC_DATE"].ToString();
                DIAGNOSIS_DATE = row["HLSMEDI_DIAGNOSIS_DATE"].ToString();
                COMPLETED_DATE = row["HLSMEDI_COMPLETED_DATE"].ToString();
                SBCB_DATE = row["HLSMEDI_SBCB_DATE"].ToString();
                ANSWER1 = row["HLSMEDI_ANSWER1"].ToString();
                ANSWER2 = row["HLSMEDI_ANSWER2"].ToString();
                ANSWER3 = row["HLSMEDI_ANSWER3"].ToString();
                CustQuestions = row["HLSTRCK_CUSTQ_CODES"].ToString();
                Task_Desc = row["Task_Desc"].ToString();
                NotesDesc = row["NotesDesc"].ToString();
                TrackSeq = row["HLSTRCK_SEQ"].ToString();
                XMLANSWER1 = row["HLSMEDI_XMLANSWER"].ToString();
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROG { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        public string TASK { get; set; }
        public string SEQ { get; set; }
        public string ADDRESSED_DATE { get; set; }
        public string FOLLOWUP_DATE { get; set; }
        public string COMPLETED_DATE { get; set; }
        public string SBCB_DATE { get; set; }
        public string COMPONENT { get; set; }
        public string SN { get; set; }
        public string ANSWER1 { get; set; }
        public string ANSWER2 { get; set; }
        public string ANSWER3 { get; set; }
        public string DIAGNOSIS_DATE { get; set; }
        public string SPECIAL_DATE { get; set; }
        public string SPECIAL_WHERE { get; set; }
        public string FOLLOWUPC_DATE { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string DATE_LSTC { get; set; }
        public string LSTC_OPERATOR { get; set; }
        public string Task_Desc { get; set; }
        public string Mode { get; set; }
        public string CustQuestions { get; set; }
        public string NotesDesc { get; set; }
        public string TrackSeq { get; set; }
        public string XMLANSWER1 { get; set; }
        #endregion
    }


    public class HlsMedRespEntity
    {
        #region Constructors

        public HlsMedRespEntity()
        {
            Rec_Type = string.Empty;
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROG = string.Empty;
            YEAR = string.Empty;
            APP_NO = string.Empty;
            //TASK = string.Empty;
            //SEQ = string.Empty;
            QUE = string.Empty;
            RESP_SEQ = string.Empty;
            NUM_RESP = string.Empty;
            ALPHA_RESP = string.Empty;
            DATE_RESP = string.Empty;
            DATE_LSTC = string.Empty;
            LSTC_OPERATOR = string.Empty;
            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;
            Mode = string.Empty;
        }

        public HlsMedRespEntity(bool Initialize)
        {
            Rec_Type = null;
            AGENCY = null;
            DEPT = null;
            PROG = null;
            YEAR = null;
            APP_NO = null;
            //TASK = null;
            //SEQ = null;
            QUE = null;
            RESP_SEQ = null;
            NUM_RESP = null;
            ALPHA_RESP = null;
            DATE_RESP = null;
            DATE_LSTC = null;
            LSTC_OPERATOR = null;
            DATE_ADD = null;
            ADD_OPERATOR = null;
            Mode = null;
        }

        public HlsMedRespEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["HLSMEDRSP_AGENCY"].ToString();
                DEPT = row["HLSMEDRSP_DEPT"].ToString();
                PROG = row["HLSMEDRSP_PROG"].ToString();
                YEAR = row["HLSMEDRSP_YEAR"].ToString();
                APP_NO = row["HLSMEDRSP_APP_NO"].ToString();
                QUE = row["HLSMEDRSP_QUE"].ToString();
                RESP_SEQ = row["HLSMEDRSP_RESP_SEQ"].ToString();
                NUM_RESP = row["HLSMEDRSP_NUM_RESP"].ToString();
                ALPHA_RESP = row["HLSMEDRSP_ALPHA_RESP"].ToString();
                DATE_RESP = row["HLSMEDRSP_DATE_RESP"].ToString();
                DATE_ADD = row["HLSMEDRSP_DATE_ADD"].ToString();
                ADD_OPERATOR = row["HLSMEDRSP_ADD_OPERATOR"].ToString();
                DATE_LSTC = row["HLSMEDRSP_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["HLSMEDRSP_LSTC_OPERATOR"].ToString();
                Mode = string.Empty;
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROG { get; set; }
        public string YEAR { get; set; }
        public string APP_NO { get; set; }
        //public string TASK { get; set; }
        //public string SEQ { get; set; }
        public string QUE { get; set; }
        public string RESP_SEQ { get; set; }
        public string NUM_RESP { get; set; }
        public string ALPHA_RESP { get; set; }
        public string DATE_RESP { get; set; }
        public string DATE_LSTC { get; set; }
        public string LSTC_OPERATOR { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        #endregion
    }
}
