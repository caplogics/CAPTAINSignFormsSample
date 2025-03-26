using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{

    public class ChldTrckEntity
    {
        #region Constructors

        public ChldTrckEntity()
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
            FUNDR = string.Empty;
            FUNDE = string.Empty;
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

        public ChldTrckEntity(bool Initialize)
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
            FUNDR = null;
            FUNDE = null;
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

        public ChldTrckEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["TRCK_Agency"].ToString();
                Dept = row["TRCK_Dept"].ToString();
                Prog = row["TRCK_PROGRAM"].ToString();
                COMPONENT = row["TRCK_COMPONENT"].ToString().Trim();
                TASK = row["TRCK_TASK"].ToString();
                TASKDESCRIPTION = row["TRCK_TASK_DESC"].ToString();
                SEQ = row["TRCK_SEQ"].ToString();
                //ENTRYYN = row["TRCK_ENTRY_YN"].ToString();
                //ENTRY = row["TRCK_ENTRY"].ToString();
                //NEXTYN = row["TRCK_NEXT_YN"].ToString();
                //NEXTTASK = row["TRCK_NEXT_TASK"].ToString();
                //NEXTDAYS = row["TRCK_NEXT_DAYS"].ToString();
                //RESPONSE = row["TRCK_RESPONSE"].ToString();
                //RESPONSEVAL1 = row["TRCK_RESPONSE_VAL1"].ToString();
                //RESPONSELIT1 = row["TRCK_RESPONSE_LIT1"].ToString();
                //RESPONSEVAL2 = row["TRCK_RESPONSE_VAL2"].ToString();
                //RESPONSELIT2 = row["TRCK_RESPONSE_LIT2"].ToString();
                //RESPONSEVAL3 = row["TRCK_RESPONSE_VAL3"].ToString();
                //RESPONSELIT3 = row["TRCK_RESPONSE_LIT3"].ToString();
                //RESPONSEVAL4 = row["TRCK_RESPONSE_VAL4"].ToString();
                //RESPONSELIT4 = row["TRCK_RESPONSE_LIT4"].ToString();               
                //QUESTION = row["TRCK_QUESTION"].ToString();
                //ENTRYEVERYYEAR = row["TRCK_ENTRY_EVERYYEAR"].ToString();
                COMPONENTOWNER1 = row["TRCK_COMPNT_OWNER1"].ToString();
                COMPONENTOWNER2 = row["TRCK_COMPNT_OWNER2"].ToString();
                COMPONENTOWNER3 = row["TRCK_COMPNT_OWNER3"].ToString();
                COUNT = row["TRCK_COUNT"].ToString();
                GCHARTCODE = row["TRCK_GCHART_CODE"].ToString();
                GCHARTSEL = row["TRCK_GCHART_SEL"].ToString();
                QUESTIONR = row["TRCK_QUESTION_R"].ToString();
                QUESTIONE = row["TRCK_QUESTION_E"].ToString();
                CASENOTESR = row["TRCK_CASENOTES_R"].ToString();
                CASENOTESE = row["TRCK_CASENOTES_E"].ToString();
                SBCBR = row["TRCK_SBCB_R"].ToString();
                SBCBE = row["TRCK_SBCB_E"].ToString();
                ADDRESSR = row["TRCK_ADDRESS_R"].ToString();
                ADDRESSE = row["TRCK_ADDRESS_E"].ToString();
                COMPLETER = row["TRCK_COMPLETE_R"].ToString();
                COMPLETEE = row["TRCK_COMPLETE_E"].ToString();
                FOLLOWUPR = row["TRCK_FOLLOWUP_R"].ToString();
                FOLLOWUPE = row["TRCK_FOLLOWUP_E"].ToString();
                FOLLOWUPCR = row["TRCK_FOLLOWUPC_R"].ToString();
                FOLLOWUPCE = row["TRCK_FOLLOWUPC_E"].ToString();
                DIAGNOSER = row["TRCK_DIAGNOSE_R"].ToString();
                DIAGNOSEE = row["TRCK_DIAGNOSE_E"].ToString();
                SSRR = row["TRCK_SSR_R"].ToString();
                SSRE = row["TRCK_SSR_E"].ToString();
                WHERER = row["TRCK_WHERE_R"].ToString();
                WHEREE = row["TRCK_WHERE_E"].ToString();
                FUNDR = row["TRCK_FUND_R"].ToString();
                FUNDE = row["TRCK_FUND_E"].ToString();
                QUESTIONR1 = row["TRCK_QUESTION_R1"].ToString();
                CASENOTESR1 = row["TRCK_CASENOTES_R1"].ToString();
                SBCBR1 = row["TRCK_SBCB_R1"].ToString();
                ADDRESSR1 = row["TRCK_ADDRESS_R1"].ToString();
                COMPLETER1 = row["TRCK_COMPLETE_R1"].ToString();
                FOLLOWUPR1 = row["TRCK_FOLLOWUP_R1"].ToString();
                FOLLOWUPCR1 = row["TRCK_FOLLOWUPC_R1"].ToString();
                DIAGNOSER1 = row["TRCK_DIAGNOSE_R1"].ToString();
                SSRR1 = row["TRCK_SSR_R1"].ToString();
                WHERER1 = row["TRCK_WHERE_R1"].ToString();
                QUESTIONR2 = row["TRCK_QUESTION_R2"].ToString();
                CASENOTESR2 = row["TRCK_CASENOTES_R2"].ToString();
                SBCBR2 = row["TRCK_SBCB_R2"].ToString();
                ADDRESSR2 = row["TRCK_ADDRESS_R2"].ToString();
                COMPLETER2 = row["TRCK_COMPLETE_R2"].ToString();
                FOLLOWUPR2 = row["TRCK_FOLLOWUP_R2"].ToString();
                FOLLOWUPCR2 = row["TRCK_FOLLOWUPC_R2"].ToString();
                DIAGNOSER2 = row["TRCK_DIAGNOSE_R2"].ToString();
                SSRR2 = row["TRCK_SSR_R2"].ToString();
                WHERER2 = row["TRCK_WHERE_R2"].ToString();
                QUESTIONR3 = row["TRCK_QUESTION_R3"].ToString();
                CASENOTESR3 = row["TRCK_CASENOTES_R3"].ToString();
                SBCBR3 = row["TRCK_SBCB_R3"].ToString();
                ADDRESSR3 = row["TRCK_ADDRESS_R3"].ToString();
                COMPLETER3 = row["TRCK_COMPLETE_R3"].ToString();
                FOLLOWUPR3 = row["TRCK_FOLLOWUP_R3"].ToString();
                FOLLOWUPCR3 = row["TRCK_FOLLOWUPC_R3"].ToString();
                DIAGNOSER3 = row["TRCK_DIAGNOSE_R3"].ToString();
                SSRR3 = row["TRCK_SSR_R3"].ToString();
                WHERER3 = row["TRCK_WHERE_R3"].ToString();
                QUESTIONR4 = row["TRCK_QUESTION_R4"].ToString();
                CASENOTESR4 = row["TRCK_CASENOTES_R4"].ToString();
                SBCBR4 = row["TRCK_SBCB_R4"].ToString();
                ADDRESSR4 = row["TRCK_ADDRESS_R4"].ToString();
                COMPLETER4 = row["TRCK_COMPLETE_R4"].ToString();
                FOLLOWUPR4 = row["TRCK_FOLLOWUP_R4"].ToString();
                FOLLOWUPCR4 = row["TRCK_FOLLOWUPC_R4"].ToString();
                DIAGNOSER4 = row["TRCK_DIAGNOSE_R4"].ToString();
                SSRR4 = row["TRCK_SSR_R4"].ToString();
                WHERER4 = row["TRCK_WHERE_R4"].ToString();
                DATEADD = row["TRCK_DATE_ADD"].ToString();
                ADDOPERATOR = row["TRCK_ADD_OPERATOR"].ToString();
                DATELSTC = row["TRCK_DATE_LSTC"].ToString();
                LSTCOPERATOR = row["TRCK_LSTC_OPERATOR"].ToString();
                CustQCodes = row["TRCK_CUSTQ_CODES"].ToString();
                Mode = string.Empty;
                Rep_Seq = string.Empty;
            }
        }


        public ChldTrckEntity(string Task, string strTaskDesc, string strEveryYear, string strEntrydays, string NextYr, string NextTask, string strNextDays, string sbcbDate, string CompletDt, string FollowupDt, string AddresDt,string strIntakeEntry,string strCustQues)
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

        //public ChldTrckEntity(ChldTrckEntity chldtrckdata)
        //{
        //    Rec_Type = chldtrckdata.Rec_Type;
        //    Agency = chldtrckdata.Agency;
        //    Dept = chldtrckdata.Dept;
        //    Prog = chldtrckdata.Prog;
        //    COMPONENT = chldtrckdata.COMPONENT;
        //    TASK = chldtrckdata.TASK;
        //    TASKDESCRIPTION = chldtrckdata.TASKDESCRIPTION;
        //    ENTRYYN = chldtrckdata.ENTRYYN;
        //    ENTRY = chldtrckdata.ENTRY;
        //    NEXTYN = chldtrckdata.NEXTYN;
        //    NEXTTASK = chldtrckdata.NEXTTASK;
        //    NEXTDAYS = chldtrckdata.NEXTDAYS;
        //    RESPONSE = chldtrckdata.RESPONSE;
        //    RESPONSEVAL1 = chldtrckdata.RESPONSEVAL1;
        //    RESPONSELIT1 = chldtrckdata.RESPONSELIT1;
        //    RESPONSEVAL2 = chldtrckdata.RESPONSEVAL2;
        //    RESPONSELIT2 = chldtrckdata.RESPONSELIT2;
        //    RESPONSEVAL3 = chldtrckdata.RESPONSEVAL3;
        //    RESPONSELIT3 = chldtrckdata.RESPONSELIT3;
        //    RESPONSEVAL4 = chldtrckdata.RESPONSEVAL4;
        //    RESPONSELIT4 = chldtrckdata.RESPONSELIT4;
        //    NXTASKDESC = chldtrckdata.NXTASKDESC;
        //    QUESTION = chldtrckdata.QUESTION;
        //    COMPONENTOWNER1 = chldtrckdata.COMPONENTOWNER1;
        //    COMPONENTOWNER2 = chldtrckdata.COMPONENTOWNER2;
        //    COMPONENTOWNER3 = chldtrckdata.COMPONENTOWNER3;
        //    ENTRYEVERYYEAR = chldtrckdata.ENTRYEVERYYEAR;
        //    COUNT = chldtrckdata.COUNT;
        //    GCHARTCODE = chldtrckdata.GCHARTCODE;
        //    GCHARTSEL = chldtrckdata.GCHARTSEL;
        //    QUESTIONR = chldtrckdata.QUESTIONR;
        //    QUESTIONE = chldtrckdata.QUESTIONE;
        //    CASENOTESR = chldtrckdata.CASENOTESR;
        //    CASENOTESE = chldtrckdata.CASENOTESE;
        //    SBCBR = chldtrckdata.SBCBR;
        //    SBCBE = chldtrckdata.SBCBE;
        //    ADDRESSR = chldtrckdata.ADDRESSR;
        //    ADDRESSE = chldtrckdata.ADDRESSE;
        //    COMPLETER = chldtrckdata.COMPLETER;
        //    COMPLETEE = chldtrckdata.COMPLETEE;
        //    FOLLOWUPR = chldtrckdata.FOLLOWUPR;
        //    FOLLOWUPE = chldtrckdata.FOLLOWUPE;
        //    FOLLOWUPCR = chldtrckdata.FOLLOWUPCR;
        //    FOLLOWUPCE = chldtrckdata.FOLLOWUPCE;
        //    DIAGNOSER = chldtrckdata.DIAGNOSER;
        //    DIAGNOSEE = chldtrckdata.DIAGNOSEE;
        //    SSRR = chldtrckdata.SSRR;
        //    SSRE = chldtrckdata.SSRE;
        //    WHERER = chldtrckdata.WHERER;
        //    WHEREE = chldtrckdata.WHEREE;
        //    QUESTIONR1 = chldtrckdata.QUESTIONR1;
        //    CASENOTESR1 = chldtrckdata.CASENOTESR1;
        //    SBCBR1 = chldtrckdata.SBCBR1;
        //    ADDRESSR1 = chldtrckdata.ADDRESSR1;
        //    COMPLETER1 = chldtrckdata.COMPLETER1;
        //    FOLLOWUPR1 = chldtrckdata.FOLLOWUPR1;
        //    FOLLOWUPCR1 = chldtrckdata.FOLLOWUPCR1;
        //    DIAGNOSER1 = chldtrckdata.DIAGNOSER1;
        //    SSRR1 = chldtrckdata.SSRR1;
        //    WHERER1 = chldtrckdata.WHERER1;
        //    QUESTIONR2 = chldtrckdata.QUESTIONR2;
        //    CASENOTESR2 = chldtrckdata.CASENOTESR2;
        //    SBCBR2 = chldtrckdata.SBCBR2;
        //    ADDRESSR2 = chldtrckdata.ADDRESSR2;
        //    COMPLETER2 = chldtrckdata.COMPLETER2;
        //    FOLLOWUPR2 = chldtrckdata.FOLLOWUPR2;
        //    FOLLOWUPCR2 = chldtrckdata.FOLLOWUPCR2;
        //    DIAGNOSER2 = chldtrckdata.DIAGNOSER2;
        //    SSRR2 = chldtrckdata.SSRR2;
        //    WHERER2 = chldtrckdata.WHERER2;
        //    QUESTIONR3 = chldtrckdata.QUESTIONR3;
        //    CASENOTESR3 = chldtrckdata.CASENOTESR3;
        //    SBCBR3 = chldtrckdata.SBCBR3;
        //    ADDRESSR3 = chldtrckdata.ADDRESSR3;
        //    COMPLETER3 = chldtrckdata.COMPLETER3;
        //    FOLLOWUPR3 = chldtrckdata.FOLLOWUPR3;
        //    FOLLOWUPCR3 = chldtrckdata.FOLLOWUPCR3;
        //    DIAGNOSER3 = chldtrckdata.DIAGNOSER3;
        //    SSRR3 = chldtrckdata.SSRR3;
        //    WHERER3 = chldtrckdata.WHERER3;
        //    QUESTIONR4 = chldtrckdata.QUESTIONR4;
        //    CASENOTESR4 = chldtrckdata.CASENOTESR4;
        //    SBCBR4 = chldtrckdata.SBCBR4;
        //    ADDRESSR4 = chldtrckdata.ADDRESSR4;
        //    COMPLETER4 = chldtrckdata.COMPLETER4;
        //    FOLLOWUPR4 = chldtrckdata.FOLLOWUPR4;
        //    FOLLOWUPCR4 = chldtrckdata.FOLLOWUPCR4;
        //    DIAGNOSER4 = chldtrckdata.DIAGNOSER4;
        //    SSRR4 = chldtrckdata.SSRR4;
        //    WHERER4 = chldtrckdata.WHERER4;
        //    DATEADD = chldtrckdata.DATEADD;
        //    ADDOPERATOR = chldtrckdata.ADDOPERATOR;
        //    DATELSTC = chldtrckdata.DATELSTC;
        //    LSTCOPERATOR = chldtrckdata.LSTCOPERATOR;
        //    CustQCodes = chldtrckdata.CustQCodes;

        //    Mode = chldtrckdata.Mode;
        //    Rep_Seq = chldtrckdata.Rep_Seq;
        //}

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
        // murali added on 10/14/2020
           public string FUNDR { get; set; }
           public string FUNDE { get; set; }
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

    public class ChldTrckREntity
    {
        #region Constructors

        public ChldTrckREntity()
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

        public ChldTrckREntity(bool Initialize)
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

        public ChldTrckREntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Agency = row["TRCKR_Agency"].ToString().Trim();
                Dept = row["TRCKR_Dept"].ToString().Trim();
                Prog = row["TRCKR_PROG"].ToString().Trim();
                COMPONENT = row["TRCKR_COMPONENT"].ToString().Trim();
                TASK = row["TRCKR_TASK"].ToString().Trim();
                FUND = row["TRCKR_FUND"].ToString().Trim();
                REQUIREYEAR = row["TRCKR_REQUIRE_EVERYEAR"].ToString().Trim();
                INTAKEENTRY = row["TRCKR_INTAKE_ENTRY"].ToString().Trim();
                ENTERDAYS = row["TRCKR_ENTRY_DAYS"].ToString().Trim();
                NEXTTASK = row["TRCKR_NEXT_TASK"].ToString().Trim();
                NEXTDAYS = row["TRCKR_NEXT_DAYS"].ToString().Trim();
                NXTACTION = row["TRCKR_NXTACTION"].ToString().Trim();
                DATEADD = row["TRCKR_DATE_ADD"].ToString().Trim();
                ADDOPERATOR = row["TRCKR_ADD_OPERATOR"].ToString().Trim();
                DATELSTC = row["TRCKR_DATE_LSTC"].ToString().Trim().Trim();
                LSTCOPERATOR = row["TRCKR_LSTC_OPERATOR"].ToString().Trim();
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


    public class ChldMediEntity
    {
        #region Constructors

        public ChldMediEntity()
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
            MediFund = string.Empty;

            TSKUPLD_ID = string.Empty;
            TSKUPLD_AGENCY = string.Empty;
            TSKUPLD_DEPT = string.Empty;
            TSKUPLD_PROGRAM = string.Empty;
            TSKUPLD_YEAR = string.Empty;
            TSKUPLD_APPNO = string.Empty;
            TSKUPLD_TASK = string.Empty;
            TSKUPLD_ORIG_NAME = string.Empty;
            TSKUPLD_UPLOAD_BY = string.Empty;
            TSKUPLD_DATE_UPLOAD = string.Empty;
            TSKUPLD_UPLOAD_AS = string.Empty;
            TSKUPLD_DELETED_BY = string.Empty;
            TSKUPLD_DATE_DETLETED = string.Empty;
            TSKUPLD_LSTC_OPERATOR = string.Empty;
            TSKUPLD_DATE_LSTC = string.Empty;
        }

        public ChldMediEntity(bool Initialize)
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
            MediFund = null;

            TSKUPLD_ID = null;
            TSKUPLD_AGENCY = null;
            TSKUPLD_DEPT = null;
            TSKUPLD_PROGRAM = null;
            TSKUPLD_YEAR = null;
            TSKUPLD_APPNO = null;
            TSKUPLD_TASK = null;
            TSKUPLD_ORIG_NAME = null;
            TSKUPLD_UPLOAD_BY = null;
            TSKUPLD_DATE_UPLOAD = null;
            TSKUPLD_UPLOAD_AS = null;
            TSKUPLD_DELETED_BY = null;
            TSKUPLD_DATE_DETLETED = null;
            TSKUPLD_LSTC_OPERATOR = null;
            TSKUPLD_DATE_LSTC = null;
        }

        public ChldMediEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["MEDI_AGENCY"].ToString();
                DEPT = row["MEDI_DEPT"].ToString();
                PROG = row["MEDI_PROG"].ToString();
                YEAR = row["MEDI_YEAR"].ToString();
                APP_NO = row["MEDI_APP_NO"].ToString();
                TASK = row["MEDI_TASK"].ToString();
                SEQ = row["MEDI_SEQ"].ToString();
                ADDRESSED_DATE = row["MEDI_ADDRESSED_DATE"].ToString();
                FOLLOWUP_DATE = row["MEDI_FOLLOWUP_DATE"].ToString();
                COMPLETED_DATE = row["MEDI_COMPLETED_DATE"].ToString();
                SBCB_DATE = row["MEDI_SBCB_DATE"].ToString();
                COMPONENT = row["MEDI_COMPONENT"].ToString();
                SN = row["MEDI_SN"].ToString();
                ANSWER1 = row["MEDI_ANSWER1"].ToString();
                ANSWER2 = row["MEDI_ANSWER2"].ToString();
                ANSWER3 = row["MEDI_ANSWER3"].ToString();
                DIAGNOSIS_DATE = row["MEDI_DIAGNOSIS_DATE"].ToString();
                SPECIAL_DATE = row["MEDI_SPECIAL_DATE"].ToString();
                SPECIAL_WHERE = row["MEDI_SPECIAL_WHERE"].ToString();
                FOLLOWUPC_DATE = row["MEDI_FOLLOWUPC_DATE"].ToString();
                DATE_ADD = row["MEDI_DATE_ADD"].ToString();
                ADD_OPERATOR = row["MEDI_ADD_OPERATOR"].ToString();
                DATE_LSTC = row["MEDI_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["MEDI_LSTC_OPERATOR"].ToString();
                MediFund = row["MEDI_FUND"].ToString();
                Mode = string.Empty;               
            }
        }

        public ChldMediEntity(DataRow row,int val)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["MEDI_AGENCY"].ToString();
                DEPT = row["MEDI_DEPT"].ToString();
                PROG = row["MEDI_PROG"].ToString();
                YEAR = row["MEDI_YEAR"].ToString();
                APP_NO = row["MEDI_APP_NO"].ToString();
                TASK = row["MEDI_TASK"].ToString();
                SEQ = row["MEDI_SEQ"].ToString();
                ADDRESSED_DATE = row["MEDI_ADDRESSED_DATE"].ToString();
                FOLLOWUP_DATE = row["MEDI_FOLLOWUP_DATE"].ToString();
                COMPLETED_DATE = row["MEDI_COMPLETED_DATE"].ToString();
                SBCB_DATE = row["MEDI_SBCB_DATE"].ToString();
                COMPONENT = row["MEDI_COMPONENT"].ToString();
                SN = row["MEDI_SN"].ToString();
                ANSWER1 = row["MEDI_ANSWER1"].ToString();
                ANSWER2 = row["MEDI_ANSWER2"].ToString();
                ANSWER3 = row["MEDI_ANSWER3"].ToString();
                DIAGNOSIS_DATE = row["MEDI_DIAGNOSIS_DATE"].ToString();
                SPECIAL_DATE = row["MEDI_SPECIAL_DATE"].ToString();
                SPECIAL_WHERE = row["MEDI_SPECIAL_WHERE"].ToString();
                FOLLOWUPC_DATE = row["MEDI_FOLLOWUPC_DATE"].ToString();
                DATE_ADD = row["MEDI_DATE_ADD"].ToString();
                ADD_OPERATOR = row["MEDI_ADD_OPERATOR"].ToString();
                DATE_LSTC = row["MEDI_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["MEDI_LSTC_OPERATOR"].ToString();
                MediFund = row["MEDI_FUND"].ToString();
                Mode = string.Empty;

                TSKUPLD_ID = row["TSKUPLD_ID"].ToString();
                TSKUPLD_AGENCY = row["TSKUPLD_AGENCY"].ToString();
                TSKUPLD_DEPT = row["TSKUPLD_DEPT"].ToString();
                TSKUPLD_PROGRAM = row["TSKUPLD_PROGRAM"].ToString();
                TSKUPLD_YEAR = row["TSKUPLD_YEAR"].ToString();
                TSKUPLD_APPNO = row["TSKUPLD_APPNO"].ToString();
                TSKUPLD_TASK = row["TSKUPLD_TASK"].ToString();
                TSKUPLD_ORIG_NAME = row["TSKUPLD_ORIG_NAME"].ToString();
                TSKUPLD_UPLOAD_BY = row["TSKUPLD_UPLOAD_BY"].ToString();
                TSKUPLD_DATE_UPLOAD = row["TSKUPLD_DATE_UPLOAD"].ToString();
                TSKUPLD_UPLOAD_AS = row["TSKUPLD_UPLOAD_AS"].ToString();
                TSKUPLD_DELETED_BY = row["TSKUPLD_DELETED_BY"].ToString();
                TSKUPLD_DATE_DETLETED = row["TSKUPLD_DATE_DETLETED"].ToString();
                TSKUPLD_LSTC_OPERATOR = row["TSKUPLD_LSTC_OPERATOR"].ToString();
                TSKUPLD_DATE_LSTC = row["TSKUPLD_DATE_LSTC"].ToString();

            }
        }


        public ChldMediEntity(DataRow row,string strtable)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["MEDI_AGENCY"].ToString();
                DEPT = row["MEDI_DEPT"].ToString();
                PROG = row["MEDI_PROG"].ToString();
                YEAR = row["MEDI_YEAR"].ToString();
                APP_NO = row["MEDI_APP_NO"].ToString();
                TASK = row["MEDI_TASK"].ToString();
                SEQ = row["MEDI_SEQ"].ToString();
                ADDRESSED_DATE = row["MEDI_ADDRESSED_DATE"].ToString();
                FOLLOWUP_DATE = row["MEDI_FOLLOWUP_DATE"].ToString();
                FOLLOWUPC_DATE = row["MEDI_FOLLOWUPC_DATE"].ToString();
                DIAGNOSIS_DATE = row["MEDI_DIAGNOSIS_DATE"].ToString();
                COMPLETED_DATE = row["MEDI_COMPLETED_DATE"].ToString();
                SBCB_DATE = row["MEDI_SBCB_DATE"].ToString();
                ANSWER1 = row["MEDI_ANSWER1"].ToString();
                ANSWER2 = row["MEDI_ANSWER2"].ToString();
                ANSWER3 = row["MEDI_ANSWER3"].ToString();
                CustQuestions = row["TRCK_CUSTQ_CODES"].ToString();
                Task_Desc = row["Task_Desc"].ToString();
                NotesDesc = row["NotesDesc"].ToString();
                TrackSeq = row["TRCK_SEQ"].ToString();
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
        public string MediFund { get; set; }

        public string TSKUPLD_ID { get; set; }
        public string TSKUPLD_AGENCY { get; set; }
        public string TSKUPLD_DEPT { get; set; }
        public string TSKUPLD_PROGRAM { get; set; }
        public string TSKUPLD_YEAR { get; set; }
        public string TSKUPLD_APPNO { get; set; }
        public string TSKUPLD_TASK { get; set; }
        public string TSKUPLD_ORIG_NAME { get; set; }
        public string TSKUPLD_UPLOAD_BY { get; set; }
        public string TSKUPLD_DATE_UPLOAD { get; set; }
        public string TSKUPLD_UPLOAD_AS { get; set; }
        public string TSKUPLD_DELETED_BY { get; set; }
        public string TSKUPLD_DATE_DETLETED { get; set; }
        public string TSKUPLD_LSTC_OPERATOR { get; set; }
        public string TSKUPLD_DATE_LSTC { get; set; }

        #endregion
    }


    public class ChldMedRespEntity
    {
        #region Constructors

        public ChldMedRespEntity()
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

        public ChldMedRespEntity(bool Initialize)
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

        public ChldMedRespEntity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                AGENCY = row["MEDRSP_AGENCY"].ToString();
                DEPT = row["MEDRSP_DEPT"].ToString();
                PROG = row["MEDRSP_PROG"].ToString();
                YEAR = row["MEDRSP_YEAR"].ToString();
                APP_NO = row["MEDRSP_APP_NO"].ToString();               
                QUE = row["MEDRSP_QUE"].ToString();                
                RESP_SEQ = row["MEDRSP_RESP_SEQ"].ToString();
                NUM_RESP = row["MEDRSP_NUM_RESP"].ToString();
                ALPHA_RESP = row["MEDRSP_ALPHA_RESP"].ToString();
                DATE_RESP = row["MEDRSP_DATE_RESP"].ToString();
                DATE_ADD = row["MEDRSP_DATE_ADD"].ToString();
                ADD_OPERATOR = row["MEDRSP_ADD_OPERATOR"].ToString();
                DATE_LSTC = row["MEDRSP_DATE_LSTC"].ToString();
                LSTC_OPERATOR = row["MEDRSP_LSTC_OPERATOR"].ToString();
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

    public class HSSB2106Report_Entity
    {

        #region Constructors

        public HSSB2106Report_Entity()
        {
            Agy = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            Ssn = string.Empty;
            Fname = string.Empty;
            Lname = string.Empty;
            Mname = string.Empty;
            Relation = string.Empty;
            Phone = string.Empty;
            Cell_Phn = string.Empty;
            Enrolled = string.Empty;
            Hno = string.Empty;
            Street = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            ZipPlus = string.Empty;
            FamSeq = string.Empty;
            DOB = string.Empty;
            Age = string.Empty;
            Intake_date = string.Empty;
            Site = string.Empty;
            Elig_Date = string.Empty;
            Race_Desc = string.Empty;
            Ethnic_Desc = string.Empty;
            Lang_Desc = string.Empty;
            //LangOth_Desc = string.Empty;
            Classfication = string.Empty;
            FamIncome = string.Empty;
            NoInHH = string.Empty;
            WaitList = string.Empty;
            ChldMstFund = string.Empty;
            NextYear = string.Empty;
            PreClient = string.Empty;
            AMonths = string.Empty;

            Repeater = string.Empty;
            DoctorName = string.Empty;
            DoctorAddress = string.Empty;
            DentistName = string.Empty;
            DentistAddress = string.Empty;
            Med_Coverage = string.Empty;
            Med_Plan = string.Empty;
            Med_Insurer = string.Empty;
            Dental_Coverage = string.Empty;
            Dental_Plan = string.Empty;
            Dental_insurer = string.Empty;
            Med_Covertype = string.Empty;

            Enrl_Site = string.Empty;
            Enrl_Room = string.Empty;
            Enrl_AMPM = string.Empty;
            Enrl_Status = string.Empty;
            Enrl_Date = string.Empty;
            Enrl_Fund = string.Empty;
            Enrl_fund_date = string.Empty;
            Enrolled_date = string.Empty;
            withdraw_date = string.Empty;
            LastAttn_date = string.Empty;
            StartAttn_date = string.Empty;
            Funds = string.Empty;
        }

        public HSSB2106Report_Entity(bool Intialize)
        {
            if (Intialize)
            {
                Agy = null;
                Dept = null;
                Prog = null;
                Year = null;
                AppNo = null;
                Ssn = null;
                Fname = null;
                Lname = null;
                Mname = null;
                Relation = null;
                Phone = null;
                Cell_Phn = null;
                Enrolled = null;
                Hno = null;
                Street = null;
                Apt = null;
                Flr = null;
                City = null;
                State = null;
                Zip = null;
                ZipPlus = null;
                FamSeq = null;
                DOB = null;
                Age = null;
                Intake_date = null;
                Site = null;
                Elig_Date = null;
                Race_Desc = null;
                Ethnic_Desc = null;
                Lang_Desc = null;
                Classfication = null;
                FamIncome = null;
                NoInHH = null;
                WaitList = null;
                ChldMstFund = null;
                NextYear = null;
                PreClient = null;
                AMonths = null;
                //LangOth_Desc = null;

                Repeater = null;
                DoctorName = null;
                DoctorAddress = null;
                DentistName = null;
                DentistAddress = null;
                Med_Coverage = null;
                Med_Plan = null;
                Med_Insurer = null;
                Dental_Coverage = null;
                Dental_Plan = null;
                Dental_insurer = null;
                Med_Covertype = null;

                Enrl_Site = null;
                Enrl_Room = null;
                Enrl_AMPM = null;
                Enrl_Status = null;
                Enrl_Date = null;
                Enrl_Fund = null;
                Enrl_fund_date = null;
                Enrolled_date = null;
                withdraw_date = null;
                LastAttn_date = null;
                StartAttn_date = null;
                Funds = null;
            }


        }

        public HSSB2106Report_Entity(HSSB2106Report_Entity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                Ssn = Entity.Ssn;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                Mname = Entity.Mname;
                Relation = Entity.Relation;
                Phone = Entity.Phone;
                Cell_Phn = Entity.Cell_Phn;
                Enrolled=  Entity.Enrolled;
                Hno = Entity.Hno;
                Street = Entity.Street;
                Apt = Entity.Apt;
                Flr = Entity.Flr;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                ZipPlus = Entity.ZipPlus;
                FamSeq = Entity.FamSeq;
                DOB = Entity.DOB;
                Age = Entity.Age;
                Intake_date = Entity.DOB;
                Site = Entity.Site;
                Elig_Date = Entity.Elig_Date;
                Race_Desc = Entity.Race_Desc;
                Ethnic_Desc = Entity.Ethnic_Desc;
                Lang_Desc = Entity.Lang_Desc;
                //LangOth_Desc = Entity.LangOth_Desc;
                Classfication = Entity.Classfication;
                FamIncome = Entity.FamIncome;
                NoInHH = Entity.NoInHH;
                WaitList = Entity.WaitList;
                ChldMstFund = Entity.ChldMstFund;
                NextYear = Entity.NextYear;
                PreClient = Entity.PreClient;
                AMonths = Entity.AMonths;

                Repeater = Entity.Repeater;
                DoctorName = Entity.DoctorName;
                DoctorAddress = Entity.DoctorAddress;
                DentistName = Entity.DentistName;
                DentistAddress = Entity.DentistAddress;
                Med_Coverage = Entity.Med_Coverage;
                Med_Plan = Entity.Med_Plan;
                Med_Insurer = Entity.Med_Insurer;
                Dental_Coverage = Entity.Dental_Coverage;
                Dental_Plan = Entity.Dental_Plan;
                Dental_insurer = Entity.Dental_insurer;
                Med_Covertype = Entity.Med_Covertype;
                
                Enrl_Site = Entity.Enrl_Site;
                Enrl_Room = Entity.Enrl_Room;
                Enrl_AMPM = Entity.Enrl_AMPM;
                Enrl_Status = Entity.Enrl_Status;
                Enrl_Date = Entity.Enrl_Date;
                Enrl_Fund = Entity.Enrl_Fund;
                Enrl_fund_date = Entity.Enrl_fund_date;
                Enrolled_date = Entity.Enrolled_date;
                withdraw_date = Entity.withdraw_date;
                LastAttn_date = Entity.LastAttn_date;
                StartAttn_date = Entity.StartAttn_date;
                Funds = Entity.Funds;
            }
        }


        public HSSB2106Report_Entity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim();
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                Cell_Phn = row["Cell"].ToString().Trim();
                Enrolled = row["Mult_Fund_Site_List"].ToString().Trim();
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                Apt = row["Apt"].ToString().Trim();
                Flr = row["Flr"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                ZipPlus = row["Zip_Plus"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();

                DOB = row["DOB"].ToString().Trim();
                Age = row["AGE"].ToString().Trim();
                Intake_date = row["MST_INTAKE_DATE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Race_Desc = row["RACE_DESC"].ToString().Trim();
                Ethnic_Desc = row["ETHNIC_DESC"].ToString().Trim();
                Lang_Desc = row["LANG_DESC"].ToString().Trim();
                //LangOth_Desc = row["LANGOTH_DESC"].ToString().Trim();

                Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                DoctorName = row["CHLDMST_DOCTOR_NAME"].ToString().Trim();
                DoctorAddress = row["CHLDMST_DOCTOR_ADDRESS"].ToString().Trim();
                DentistName = row["CHLDMST_DENTIST_NAME"].ToString().Trim();
                DentistAddress = row["CHLDMST_DENTIST_ADDRESS"].ToString().Trim();
                Med_Coverage = row["CHLDMST_MED_COVERAGE"].ToString().Trim();
                Med_Plan = row["CHLDMST_MED_PLAN"].ToString().Trim();
                Med_Insurer = row["CHLDMST_MED_INSURER"].ToString().Trim();
                Dental_Coverage = row["CHLDMST_DENTAL_COVERAGE"].ToString().Trim();
                Dental_Plan = row["CHLDMST_DENTAL_PLAN"].ToString().Trim();
                Dental_insurer = row["CHLDMST_DENTAL_INSURER"].ToString().Trim();
                Med_Covertype = row["CHLDMST_MED_COVER_TYPE"].ToString().Trim();

                Enrl_Site = row["ESITE"].ToString().Trim();
                Enrl_Room = row["EROOM"].ToString();
                Enrl_AMPM = row["EAMPM"].ToString().Trim();
                Enrl_Status = row["ESTATUS"].ToString().Trim();
                Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                Enrl_Fund = row["EFUND"].ToString().Trim();
                Enrl_fund_date = row["EFUND_DATE"].ToString().Trim();
                Enrolled_date = row["ENRLD_DATE"].ToString().Trim();
                withdraw_date = row["WDRAW_DATE"].ToString().Trim();
                LastAttn_date = row["Last_ATTN_DATE"].ToString().Trim();
                StartAttn_date = row["Start_ATTN_DATE"].ToString().Trim();
            }
        }

        public HSSB2106Report_Entity(DataRow row,string Screen)
        {
            if (row != null)
            {
                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["AppNo"].ToString().Trim();
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                if (Screen != "HSSB0124")
                {
                    Cell_Phn = row["Cell"].ToString().Trim();
                    Enrolled = row["Mult_Fund_Site_List"].ToString().Trim();
                }
                Hno = row["Hno"].ToString().Trim();
                Street = row["Street"].ToString().Trim();
                Apt = row["Apt"].ToString().Trim();
                Flr = row["Flr"].ToString().Trim();
                City = row["City"].ToString().Trim();
                State = row["State1"].ToString().Trim();
                Zip = row["Zip"].ToString().Trim();
                ZipPlus = row["Zip_Plus"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();
                AMonths = row["AGEINMONTHS"].ToString().Trim();

                DOB = row["DOB"].ToString().Trim();
                Age = row["AGE"].ToString().Trim();
                Intake_date = row["MST_INTAKE_DATE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Race_Desc = row["RACE_DESC"].ToString().Trim();
                Ethnic_Desc = row["ETHNIC_DESC"].ToString().Trim();
                Lang_Desc = row["LANG_DESC"].ToString().Trim();
                Classfication = row["MST_CLASSIFICATION"].ToString().Trim();
                FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                NoInHH = row["MST_NO_INHH"].ToString().Trim();
                WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                ChldMstFund = row["CHLDMST_FUND_SOURCE"].ToString().Trim();
                NextYear = row["MST_NEXTYEAR"].ToString().Trim();
                PreClient = row["CHLDMST_PRE_CLIENT"].ToString().Trim();
                ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
                //LangOth_Desc = row["LANGOTH_DESC"].ToString().Trim();

                Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                DoctorName = row["CHLDMST_DOCTOR_NAME"].ToString().Trim();
                DoctorAddress = row["CHLDMST_DOCTOR_ADDRESS"].ToString().Trim();
                DentistName = row["CHLDMST_DENTIST_NAME"].ToString().Trim();
                DentistAddress = row["CHLDMST_DENTIST_ADDRESS"].ToString().Trim();
                Med_Coverage = row["CHLDMST_MED_COVERAGE"].ToString().Trim();
                Med_Plan = row["CHLDMST_MED_PLAN"].ToString().Trim();
                Med_Insurer = row["CHLDMST_MED_INSURER"].ToString().Trim();
                Dental_Coverage = row["CHLDMST_DENTAL_COVERAGE"].ToString().Trim();
                Dental_Plan = row["CHLDMST_DENTAL_PLAN"].ToString().Trim();
                Dental_insurer = row["CHLDMST_DENTAL_INSURER"].ToString().Trim();
                Med_Covertype = row["CHLDMST_MED_COVER_TYPE"].ToString().Trim();

                Enrl_Site = row["ESITE"].ToString().Trim();
                Enrl_Room = row["EROOM"].ToString();
                Enrl_AMPM = row["EAMPM"].ToString().Trim();
                Enrl_Status = row["ESTATUS"].ToString().Trim();
                Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                Enrl_Fund = row["EFUND"].ToString().Trim();
                Enrl_fund_date = row["EFUND_DATE"].ToString().Trim();
                Enrolled_date = row["ENRLD_DATE"].ToString().Trim();
                withdraw_date = row["WDRAW_DATE"].ToString().Trim();
                LastAttn_date = row["Last_ATTN_DATE"].ToString().Trim();
                StartAttn_date = row["Start_ATTN_DATE"].ToString().Trim();
                Funds = row["FUND"].ToString().Trim();
            }
        }

        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string Ssn { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Relation { get; set; }
        public string Phone { get; set; }
        public string Cell_Phn
        {
            get; set;
        }
        public string Enrolled
        {
            get; set;
        }
        public string Hno { get; set; }

        public string Street { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipPlus { get; set; }
        public string ActiveStatus { get; set; }
        
        public string FamSeq { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        public string AMonths { get; set; }
        public string Intake_date { get; set; }
        public string Site { get; set; }
        public string Elig_Date { get; set; }
        public string Race_Desc { get; set; }
        public string Ethnic_Desc { get; set; }
        public string Lang_Desc { get; set; }
        public string Classfication { get; set; }
        public string WaitList { get; set; }
        public string NoInHH { get; set; }
        public string FamIncome { get; set; }
        public string ChldMstFund { get; set; }
        public string NextYear { get; set; }
        public string PreClient { get; set; }

        public string Repeater { get; set; }
        public string DoctorName { get; set; }
        public string DoctorAddress { get; set; }
        public string DentistName { get; set; }
        public string DentistAddress { get; set; }
        public string Med_Coverage { get; set; }
        public string Med_Plan { get; set; }
        public string Med_Insurer { get; set; }
        public string Dental_Coverage { get; set; }
        public string Dental_Plan { get; set; }
        public string Dental_insurer { get; set; }
        public string Med_Covertype { get; set; }

        public string Enrl_Site { get; set; }
        public string Enrl_Room { get; set; }
        public string Enrl_AMPM { get; set; }
        public string Enrl_Status { get; set; }
        public string Enrl_Date { get; set; }
        public string Enrl_Fund { get; set; }
        public string Enrl_fund_date { get; set; }
        public string Enrolled_date { get; set; }
        public string withdraw_date { get; set; }
        public string LastAttn_date { get; set; }
        public string StartAttn_date { get; set; }
        public string Funds { get; set; }

        #endregion

    }

   
}
