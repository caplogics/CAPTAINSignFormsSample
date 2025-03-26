using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CaseSumEntity
    {
        #region Constructors

        public CaseSumEntity()
        {

            CaseSumAgency = string.Empty;
            CaseSumDept = string.Empty;
            CaseSumProgram = string.Empty;
            CaseSumYear = string.Empty;
            CaseSumApplNo = string.Empty;
            CaseSumSeq = string.Empty;
            CaseSumRefHierachy = string.Empty;
          
            CaseSumRefYear = string.Empty;
            CaseSumRefApplNo = string.Empty;
            CaseSumReferBy = string.Empty;
            CaseSumReferDate = string.Empty;
            CaseSumContactKey = string.Empty;
            CaseSumPoints = string.Empty;
            CaseSumStatusCode = string.Empty;
            CaseSumStatusDate = string.Empty;
            CaseSumNotInterested = string.Empty;
            CaseSumNotInteresDate = string.Empty;
            CaseSumNotInterestBy = string.Empty;
            CaseSumNotInterestReason = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            Name = string.Empty;
            CaseSumXml = null;

        }

        public CaseSumEntity(bool Initialize)
        {

            if (Initialize)
            {
                CaseSumAgency = CaseSumDept = CaseSumProgram = 
                CaseSumYear = CaseSumApplNo = CaseSumSeq = 
                CaseSumRefHierachy = 
                CaseSumRefYear = CaseSumRefApplNo = CaseSumReferBy = 
                CaseSumReferDate = CaseSumContactKey = CaseSumPoints = 
                CaseSumStatusCode = CaseSumStatusDate = CaseSumNotInterested = 
                CaseSumNotInteresDate = CaseSumNotInterestBy = CaseSumNotInterestReason = 
                DateAdd = AddOperator = DateLstc = LstcOperator = Name = CaseSumXml = null;
            }
        }

        public CaseSumEntity(DataRow row)
        {
            if (row != null)
            {
                CaseSumAgency = row["CASESUM_AGENCY"].ToString().Trim();
                CaseSumDept = row["CASESUM_DEPT"].ToString().Trim();
                CaseSumProgram = row["CASESUM_PROGRAM"].ToString().Trim();
                CaseSumYear = row["CASESUM_YEAR"].ToString().Trim();
                CaseSumApplNo = row["CASESUM_APP_NO"].ToString().Trim();
                CaseSumSeq = row["CASESUM_SEQ"].ToString().Trim();
                CaseSumRefHierachy = row["CASESUM_REF_HIERARCHY"].ToString().Trim();              
                CaseSumRefYear = row["CASESUM_REF_YEAR"].ToString().Trim();
                CaseSumRefApplNo = row["CASESUM_REF_APP_NO"].ToString().Trim();
                CaseSumReferBy = row["CASESUM_REFER_BY"].ToString().Trim();
                CaseSumReferDate = row["CASESUM_REFERDATE"].ToString().Trim();
                CaseSumContactKey = row["CASESUM_CONTACT_KEY"].ToString().Trim();
                CaseSumPoints = row["CASESUM_POINTS"].ToString().Trim();
                CaseSumStatusCode = row["CASESUM_STATUS_CODE"].ToString().Trim();
                CaseSumStatusDate = row["CASESUM_STATUS_DATE"].ToString().Trim();
                CaseSumNotInterested = row["CASESUM_NOT_INTERESTED"].ToString().Trim();
                CaseSumNotInteresDate = row["CASESUM_NOT_INTEREST_DATE"].ToString().Trim();
                CaseSumNotInterestBy = row["CASESUM_NOT_INTEREST_BY"].ToString().Trim();
                CaseSumNotInterestReason = row["CASESUM_NOT_INTEREST_REASON"].ToString().Trim();
                DateAdd = row["CASESUM_DATE_ADD"].ToString().Trim();
                AddOperator = row["CASESUM_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CASESUM_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CASESUM_LSTC_OPERATOR"].ToString().Trim();



            }

        }

        public CaseSumEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                CaseSumAgency = row["CASESUM_AGENCY"].ToString().Trim();
                CaseSumDept = row["CASESUM_DEPT"].ToString().Trim();
                CaseSumProgram = row["CASESUM_PROGRAM"].ToString().Trim();
                CaseSumYear = row["CASESUM_YEAR"].ToString().Trim();
                CaseSumApplNo = row["CASESUM_APP_NO"].ToString().Trim();
                CaseSumRefHierachy = row["CASESUM_REF_HIERARCHY"].ToString().Trim();             
                CaseSumRefYear = row["CASESUM_REF_YEAR"].ToString().Trim();
                CaseSumRefApplNo = row["CASESUM_REF_APP_NO"].ToString().Trim();
                CaseSumReferBy = row["CASESUM_REFER_BY"].ToString().Trim();
                CaseSumReferDate = row["CASESUM_REFERDATE"].ToString().Trim();
                CaseSumContactKey = row["CASESUM_CONTACT_KEY"].ToString().Trim();
                CaseSumPoints = row["CASESUM_POINTS"].ToString().Trim();
                CaseSumStatusCode = row["CASESUM_STATUS_CODE"].ToString().Trim();
                CaseSumStatusDate = row["CASESUM_STATUS_DATE"].ToString().Trim();
                CaseSumNotInterested = row["CASESUM_NOT_INTERESTED"].ToString().Trim();
                CaseSumNotInteresDate = row["CASESUM_NOT_INTEREST_DATE"].ToString().Trim();
                CaseSumNotInterestBy = row["CASESUM_NOT_INTEREST_BY"].ToString().Trim();
                CaseSumNotInterestReason = row["CASESUM_NOT_INTEREST_REASON"].ToString().Trim();
                DateAdd = row["CASESUM_DATE_ADD"].ToString().Trim();
                AddOperator = row["CASESUM_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CASESUM_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CASESUM_LSTC_OPERATOR"].ToString().Trim();
                if (strTable == "CASEDEPCONT")
                {
                    Name = row["Name"].ToString().Trim();
                }
                if (strTable == "CaseHieDetails")
                {
                    Hiecode = row["HieCode"].ToString().Trim();
                    HieName = row["Hie_Name"].ToString().Trim();
                    EligStatus = row["EligStatus"].ToString().Trim();
                }


            }

        }

        #endregion

        #region Properties

        public string CaseSumAgency { get; set; }
        public string CaseSumDept { get; set; }
        public string CaseSumProgram { get; set; }
        public string CaseSumYear { get; set; }
        public string CaseSumApplNo { get; set; }
        public string CaseSumSeq { get; set; }
        public string CaseSumRefHierachy { get; set; }
        public string CaseSumRefYear { get; set; }
        public string CaseSumRefApplNo { get; set; }
        public string CaseSumReferBy { get; set; }
        public string CaseSumReferDate { get; set; }
        public string CaseSumContactKey { get; set; }
        public string CaseSumPoints { get; set; }
        public string CaseSumStatusCode { get; set; }
        public string CaseSumStatusDate { get; set; }
        public string CaseSumNotInterested { get; set; }
        public string CaseSumNotInteresDate { get; set; }
        public string CaseSumNotInterestBy { get; set; }
        public string CaseSumNotInterestReason { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Name { get; set; }
        public string CaseSumXml { get; set; }
        public string Hiecode { get; set; }
        public string HieName { get; set; }
        public string EligStatus { get; set; }
        #endregion
    }

    //public class CaseENRLEntity
    //{
    //    #region Constructors

    //    public CaseENRLEntity()
    //    {
    //        CaseENRLAgency = string.Empty;
    //        CaseENRLDept = string.Empty;
    //        CaseENRLProgram = string.Empty;
    //        CaseENRLYear = string.Empty;
    //        CaseENRLApplNo = string.Empty;
    //        CaseENRLStatus = string.Empty;
    //        CaseENRLRefAgency = string.Empty;
    //        CaseENRLRefDept = string.Empty;
    //        CaseENRLRefProgram = string.Empty;
    //        CaseENRLRefYear = string.Empty;
    //        CaseENRLStatus = string.Empty;
    //        CaseENRLEnrolledDate = string.Empty;
    //        CaseENRLWithDrawCode = string.Empty;
    //        CaseENRLWithDrawnDate = string.Empty;
    //        CaseENRLWaitListDate = string.Empty;
    //        CaseENRLDeniedCode = string.Empty;
    //        CaseENRLDeniedDate = string.Empty;
    //        CaseENRLPendingCode = string.Empty;
    //        CaseENRLPendingDate = string.Empty;
    //        CaseENRLRank = string.Empty;
    //        CaseENRLRankChangeCode = string.Empty;
    //        CaseENRLDescription1 = string.Empty;
    //        CaseENRLDescription2 = string.Empty;
    //        DateAdd = string.Empty;
    //        AddOperator = string.Empty;
    //        DateLstc = string.Empty;
    //        LstcOperator = string.Empty;
    //        Name = string.Empty;
    //        CaseENRLXml = null;

    //    }

    //    public CaseENRLEntity(DataRow row)
    //    {
    //        if (row != null)
    //        {
    //            CaseENRLAgency = row["CASEENRL_INTAKE_AGENCY"].ToString().Trim();
    //            CaseENRLDept = row["CASEENRL_INTAKE_DEPT"].ToString().Trim();
    //            CaseENRLProgram = row["CASEENRL_INTAKE_PROG"].ToString().Trim();
    //            CaseENRLYear = row["CASEENRL_INTAKE_YEAR"].ToString().Trim();
    //            CaseENRLApplNo = row["CASEENRL_APP"].ToString().Trim();
    //            CaseENRLSeq = row["CASEENRL_GROUP"].ToString().Trim();
    //            CaseENRLRefAgency = row["CASEENRL_AGENCY"].ToString().Trim();
    //            CaseENRLRefDept = row["CASEENRL_DEPT"].ToString().Trim();
    //            CaseENRLRefProgram = row["CASEENRL_PROG"].ToString().Trim();
    //            CaseENRLRefYear = row["CASEENRL_YEAR"].ToString().Trim();
    //            CaseENRLStatus = row["CASEENRL_STATUS"].ToString().Trim();
    //            CaseENRLEnrolledDate = row["CASEENRL_ENROLLED_DATE"].ToString().Trim();
    //            CaseENRLWithDrawCode = row["CASEENRL_WITHDRAW_CODE"].ToString().Trim();
    //            CaseENRLWithDrawnDate = row["CASEENRL_WITHDRAWN_DATE"].ToString().Trim();
    //            CaseENRLWaitListDate = row["CASEENRL_WAITLIST_DATE"].ToString().Trim();
    //            CaseENRLDeniedCode = row["CASEENRL_DENIED_CODE"].ToString().Trim();
    //            CaseENRLDeniedDate = row["CASEENRL_DENIED_DATE"].ToString().Trim();
    //            CaseENRLPendingCode = row["CASEENRL_PENDING_CODE"].ToString().Trim();
    //            CaseENRLPendingDate = row["CASEENRL_PENDING_DATE"].ToString().Trim();
    //            CaseENRLRank = row["CASEENRL_RANK"].ToString().Trim();
    //            CaseENRLRankChangeCode = row["CASEENRL_RANKCHANGE_CODE"].ToString().Trim();
    //            CaseENRLDescription1 = row["CASEENRL_DESCRIPTION_1"].ToString().Trim();
    //            CaseENRLDescription2 = row["CASEENRL_DESCRIPTION_2"].ToString().Trim();
    //            DateAdd = row["CaseENRL_DATE_ADD"].ToString().Trim();
    //            AddOperator = row["CaseENRL_ADD_OPERATOR"].ToString().Trim();
    //            DateLstc = row["CaseENRL_DATE_LSTC"].ToString().Trim();
    //            LstcOperator = row["CaseENRL_LSTC_OPERATOR"].ToString().Trim();

    //        }

    //    }

    //    public CaseENRLEntity(DataRow row, string strTable)
    //    {
    //        if (row != null)
    //        {
    //            CaseENRLAgency = row["CASEENRL_INTAKE_AGENCY"].ToString();
    //            CaseENRLDept = row["CASEENRL_INTAKE_DEPT"].ToString();
    //            CaseENRLProgram = row["CASEENRL_INTAKE_PROG"].ToString();
    //            CaseENRLYear = row["CASEENRL_INTAKE_YEAR"].ToString();
    //            CaseENRLApplNo = row["CASEENRL_APP"].ToString();
    //            CaseENRLSeq = row["CASEENRL_GROUP"].ToString();
    //            CaseENRLRefAgency = row["CASEENRL_AGENCY"].ToString();
    //            CaseENRLRefDept = row["CASEENRL_DEPT"].ToString();
    //            CaseENRLRefProgram = row["CASEENRL_PROG"].ToString();
    //            CaseENRLRefYear = row["CASEENRL_YEAR"].ToString();
    //            CaseENRLStatus = row["CASEENRL_STATUS"].ToString();
    //            CaseENRLEnrolledDate = row["CASEENRL_ENROLLED_DATE"].ToString();
    //            CaseENRLWithDrawCode = row["CASEENRL_WITHDRAW_CODE"].ToString();
    //            CaseENRLWithDrawnDate = row["CASEENRL_WITHDRAWN_DATE"].ToString();
    //            CaseENRLWaitListDate = row["CASEENRL_WAITLIST_DATE"].ToString();
    //            CaseENRLDeniedCode = row["CASEENRL_DENIED_CODE"].ToString();
    //            CaseENRLDeniedDate = row["CASEENRL_DENIED_DATE"].ToString();
    //            CaseENRLPendingCode = row["CASEENRL_PENDING_CODE"].ToString();
    //            CaseENRLPendingDate = row["CASEENRL_PENDING_DATE"].ToString();
    //            CaseENRLRank = row["CASEENRL_RANK"].ToString();
    //            CaseENRLRankChangeCode = row["CASEENRL_RANKCHANGE_CODE"].ToString();
    //            CaseENRLDescription1 = row["CASEENRL_DESCRIPTION_1"].ToString();
    //            CaseENRLDescription2 = row["CASEENRL_DESCRIPTION_2"].ToString();
    //            DateAdd = row["CaseENRL_DATE_ADD"].ToString();
    //            AddOperator = row["CaseENRL_ADD_OPERATOR"].ToString();
    //            DateLstc = row["CaseENRL_DATE_LSTC"].ToString();
    //            LstcOperator = row["CaseENRL_LSTC_OPERATOR"].ToString();
    //        }

    //    }

    //    #endregion

    //    #region Properties

    //    public string CaseENRLAgency { get; set; }
    //    public string CaseENRLDept { get; set; }
    //    public string CaseENRLProgram { get; set; }
    //    public string CaseENRLYear { get; set; }
    //    public string CaseENRLApplNo { get; set; }
    //    public string CaseENRLSeq { get; set; }
    //    public string CaseENRLRefAgency { get; set; }
    //    public string CaseENRLRefDept { get; set; }
    //    public string CaseENRLRefProgram { get; set; }
    //    public string CaseENRLRefYear { get; set; }
    //    public string CaseENRLStatus { get; set; }
    //    public string CaseENRLEnrolledDate { get; set; }
    //    public string CaseENRLWithDrawCode { get; set; }
    //    public string CaseENRLWithDrawnDate { get; set; }
    //    public string CaseENRLWaitListDate { get; set; }
    //    public string CaseENRLDeniedCode { get; set; }
    //    public string CaseENRLDeniedDate { get; set; }
    //    public string CaseENRLPendingCode { get; set; }
    //    public string CaseENRLPendingDate { get; set; }
    //    public string CaseENRLRank { get; set; }
    //    public string CaseENRLRankChangeCode { get; set; }
    //    public string CaseENRLDescription1 { get; set; }
    //    public string CaseENRLDescription2 { get; set; }
    //    public string DateAdd { get; set; }
    //    public string AddOperator { get; set; }
    //    public string DateLstc { get; set; }
    //    public string LstcOperator { get; set; }
    //    public string Mode { get; set; }
    //    public string Name { get; set; }
    //    public string CaseENRLXml { get; set; }
    //    #endregion
    //}


    public class CaseELIGQUESEntity
    {
        #region Constructors

        public CaseELIGQUESEntity()
        {
            EligQuesCode = string.Empty;
            EligQuesDesc = string.Empty;
            EligRespType = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligFldName = string.Empty;

        }
        // Sudheer
        public CaseELIGQUESEntity(DataRow row)
        {
            EligQuesCode = string.Empty;
            EligQuesDesc = string.Empty;
            EligRespType = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligFldName = string.Empty;

        }

        public CaseELIGQUESEntity(DataRow row,string strTable)
        {
            if (row != null)
            {
                if (strTable == "ELIGQUES")
                {
                    EligQuesCode = row["ELIGQUES_CODE"].ToString().Trim();
                    EligQuesDesc = row["ELIGQUES_DESC"].ToString().Trim();
                    EligRespType = row["ELIGQUES_RESP_TYPE"].ToString().Trim();
                    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
                    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
                    EligFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();                  
                }
                else if(strTable == "PIPCUS")
                {
                    EligQuesCode = row["CUST_CODE"].ToString().Trim();
                    EligQuesDesc = row["CUST_DESC"].ToString().Trim();
                    EligRespType = row["CUST_RESP_TYPE"].ToString().Trim();                   
                    EligFliedType = row["CUST_MEM_ACCESS"].ToString().Trim();
                }
                else
                {
                    EligQuesCode = row["FLDH_CODE"].ToString().Trim();
                    EligQuesDesc = row["CUST_DESC"].ToString().Trim();
                    EligRespType = row["CUST_RESP_TYPE"].ToString().Trim();
                    EligAgyCode = row["ELIGQUES_CUST_CODE"].ToString().Trim();
                    EligFileName = row["FLDH_SCR_CODE"].ToString().Trim();
                    EligFldName = row["FLDH_SCR_HIE"].ToString().Trim();
                    EligFliedType = row["CUST_MEM_ACCESS"].ToString().Trim();
                
                }
            }

        }



        #endregion

        #region Properties

        public string EligQuesCode { get; set; }
        public string EligQuesDesc { get; set; }
        public string EligRespType { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligFldName { get; set; }
        public string EligFliedType { get; set; }

        #endregion
    }
     

    public class CaseELIGEntity
    {
        #region Constructors

        public CaseELIGEntity()
        {
            EligQuesCode = string.Empty;
            EligAgency = string.Empty;
            EligDept = string.Empty;
            EligProgram = string.Empty;
            EligGroupCode = string.Empty;
            EligGroupDesc = string.Empty;
            //EligQuesScreen = string.Empty;
            //EligQuesType = string.Empty;
            EligMemberAccess = string.Empty;
            EligResponseType = string.Empty;
            EligConjunction = string.Empty;
            EligNumEqual = string.Empty;
            EligNumLthan = string.Empty;
            EligNumGthan = string.Empty;
            EligDDTextResp = string.Empty;
            EligPoints = string.Empty;
            Mode = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            CaseEligHXml = string.Empty;
            EligQuestionDesc = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligQuesFldName = string.Empty;
        }

        public CaseELIGEntity(DataRow row)
        {
            if (row != null)
            {

                EligAgency = row["CASEELIG_AGENCY"].ToString();
                EligDept = row["CASEELIG_DEPT"].ToString();
                EligProgram = row["CASEELIG_PROGRAM"].ToString();
                EligGroupCode = row["CASEELIG_GROUP_CODE"].ToString();
                EligGroupSeq = row["CASEELIG_GROUP_SEQ"].ToString();
                EligGroupDesc = row["CASEELIG_GROUP_DESC"].ToString();
                //EligQuesScreen = row["CASEELIG_QUES_SCREEN"].ToString();
                //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
                EligQuesCode = row["CASEELIG_QUES_CODE"].ToString();
                EligMemberAccess = row["CASEELIG_MEM_ACCESS"].ToString();
                EligResponseType = row["CASEELIG_RESP_TYPE"].ToString();
                EligConjunction = row["CASEELIG_CONJN"].ToString();
                EligNumEqual = row["CASEELIG_NUM_EQUAL"].ToString().Trim();
                EligNumGthan = row["CASEELIG_NUM_GTHAN"].ToString().Trim();
                EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
                EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                EligPoints = row["CASEELIG_POINTS"].ToString();
                DateAdd = row["CASEELIG_DATE_ADD"].ToString();
                AddOperator = row["CASEELIG_ADD_OPERATOR"].ToString();
                DateLstc = row["CASEELIG_DATE_LSTC"].ToString();
                LstcOperator = row["CASEELIG_LSTC_OPERATOR"].ToString();
                EligQuestionDesc = row["ELIGQUES_DESC"].ToString();
                EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString();
                EligFileName = row["ELIGQUES_FILENAME"].ToString();
                EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString();
            }

        }
        public CaseELIGEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if(strTable=="Hierarchy")
                {
                    EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
                    EligDept = row["CASEELIG_DEPT"].ToString().Trim();
                    EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
                }
                else
                {
                    EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
                    EligDept = row["CASEELIG_DEPT"].ToString().Trim();
                    EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
                    EligGroupCode = row["CASEELIG_GROUP_CODE"].ToString().Trim();
                    EligGroupSeq = row["CASEELIG_GROUP_SEQ"].ToString().Trim();
                    EligGroupDesc = row["CASEELIG_GROUP_DESC"].ToString().Trim();
                //EligQuesScreen = row["CASEELIG_QUES_SCREEN"].ToString();
                //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
                    EligQuesCode = row["CASEELIG_QUES_CODE"].ToString().Trim();
                    EligMemberAccess = row["CASEELIG_MEM_ACCESS"].ToString().Trim();
                    EligResponseType = row["CASEELIG_RESP_TYPE"].ToString().Trim();
                    EligConjunction = row["CASEELIG_CONJN"].ToString().Trim();
                    EligNumEqual = row["CASEELIG_NUM_EQUAL"].ToString().Trim();
                    EligNumGthan = row["CASEELIG_NUM_GTHAN"].ToString().Trim();
                    EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
                    EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                    EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                    DateAdd = row["CASEELIG_DATE_ADD"].ToString().Trim();
                    AddOperator = row["CASEELIG_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["CASEELIG_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["CASEELIG_LSTC_OPERATOR"].ToString().Trim();
                    if(strTable!="Group")
                    {
                        EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
                        EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
                        EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
                        EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
                    }
                }
            }

        }

        #endregion

        #region Properties

        public string EligAgency { get; set; }
        public string EligDept { get; set; }
        public string EligProgram { get; set; }
        public string EligGroupCode { get; set; }
        public string EligGroupSeq { get; set; }
        public string EligGroupDesc { get; set; }
        //public string EligQuesScreen { get; set; }
        //public string EligQuesType { get; set; }
        public string EligQuesCode { get; set; }
        public string EligMemberAccess { get; set; }
        public string EligResponseType { get; set; }
        public string EligConjunction { get; set; }       
        public string EligNumEqual { get; set; }
        public string EligNumLthan { get; set; }
        public string EligNumGthan { get; set; }
        public string EligDDTextResp { get; set; }
        public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string CaseEligHXml { get; set; }
        public string EligQuestionDesc { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligQuesFldName { get; set; }
        public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class SERVSTOPEntity
    {
        #region Constructors

        public SERVSTOPEntity()
        {
            
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            FDate = string.Empty;
            TDate = string.Empty;
            
            Mode = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            
        }

        public SERVSTOPEntity(DataRow row)
        {
            if (row != null)
            {

                Agency = row["SRST_AGENCY"].ToString();
                Dept = row["SRST_DEPT"].ToString();
                Program = row["SRST_PROG"].ToString();
                FDate = row["SRST_FDATE"].ToString();
                TDate = row["SRST_TDATE"].ToString();
                
                DateAdd = row["SRST_DATE_ADD"].ToString();
                AddOperator = row["SRST_ADD_OPERATOR"].ToString();
                DateLstc = row["SRST_DATE_LSTC"].ToString();
                LstcOperator = row["SRST_LSTC_OPERATOR"].ToString();
                
            }

        }
        

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        
        public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class SERVSTOPHISTEntity
    {
        #region Constructors

        public SERVSTOPHISTEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Seq = string.Empty;
            FDate = string.Empty;
            TDate = string.Empty;

            Mode = string.Empty;
            Type = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

        }

        public SERVSTOPHISTEntity(DataRow row)
        {
            if (row != null)
            {

                Agency = row["SRSTH_AGENCY"].ToString();
                Dept = row["SRSTH_DEPT"].ToString();
                Program = row["SRSTH_PROG"].ToString();
                Seq = row["SRSTH_SEQ"].ToString();
                FDate = row["SRSTH_FDATE"].ToString();
                TDate = row["SRSTH_TDATE"].ToString();

                DateAdd = row["SRSTH_DATE_ADD"].ToString();
                AddOperator = row["SRSTH_ADD_OPERATOR"].ToString();
                DateLstc = row["SRSTH_DATE_LSTC"].ToString();
                LstcOperator = row["SRSTH_LSTC_OPERATOR"].ToString();

                Type = row["SRSTH_TYPE"].ToString();

            }

        }


        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Seq { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }

        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }

        public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class CaseQuestionResult   
    {
        public CaseQuestionResult()
        {
            QuestionId = string.Empty;
            QuestionConjuction = string.Empty;
            QuestionResult = string.Empty;
            QuestionStatus = string.Empty;
            QuestionResultValue = string.Empty;
        }

        public CaseQuestionResult(string strGroupId, string strQuestionId,string strConjunction,string strResult,string strStatus,string strResutValue)
        {
            GroupId = strGroupId;
            QuestionId = strQuestionId;
            QuestionConjuction = strConjunction;
            QuestionResult = strResult;
            QuestionStatus = strStatus;
            QuestionResultValue = strResutValue;
        }

        #region Properties

        public string GroupId { get; set; }
        public string QuestionId { get; set; }
        public string QuestionConjuction { get; set; }
        public string QuestionResult { get; set; }
        public string QuestionStatus { get; set; }
        public string QuestionResultValue{ get; set; }
        
        #endregion

    }

    public class CaseGroupResult
    {
        public CaseGroupResult()
        {
            GroupId = string.Empty;
            QuestionCount = 0;
            GroupResult = string.Empty;
            GroupConjunction = string.Empty;
            GroupStatus = string.Empty;
            GroupResultValue = string.Empty;
        }

        public CaseGroupResult(string strGroupId, int strQuestionCount,string strConjunction, string strResult, string strStatus, string strResutValue)
        {
            GroupId = strGroupId;
            QuestionCount = strQuestionCount;
            GroupConjunction = strConjunction;
            GroupResult = strResult;
            GroupStatus = strStatus;
            GroupResultValue = strResutValue;
        }

        #region Properties

        public string GroupId { get; set; }
        public int QuestionCount { get; set; }
        public string GroupConjunction { get; set; }
        public string GroupResult { get; set; }
        public string GroupStatus { get; set; }
        public string GroupResultValue { get; set; }

        #endregion

    }

    public class SSBGPARAMEntity
    {
        #region Constructors

        public SSBGPARAMEntity()
        {
            SSBGAgency = string.Empty;
            SSBGDept = string.Empty;
            SSBGProgram = string.Empty;
            SSBGYear = string.Empty;
            SSBGSeq = string.Empty;
            //SSBGRPFrom = string.Empty;
            //SSBGRPTo = string.Empty;
            //EligQuesScreen = row["CASEELIG_QUES_SCREEN"].ToString();
            //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
            SSBGGPFrom = string.Empty;
            SSBGGPTo = string.Empty;
            SSBGBudget = string.Empty;
            SSBGAward = string.Empty;
            SSBGRPDate = string.Empty;
            SSBGDesc = string.Empty;
            SSBGCntArea1 = string.Empty;
            SSBGCntArea2 = string.Empty;
            SSBGCntArea3 = string.Empty;
            SSBGCntArea4 = string.Empty;
            SSBGCntArea5 = string.Empty;
            SSBGCntArea1_Chk = string.Empty;
            SSBGCntArea2_Chk = string.Empty;
            SSBGCntArea3_Chk = string.Empty;
            SSBGCntArea4_Chk = string.Empty;
            SSBGCntArea5_Chk = string.Empty;
            SSBGCntArea1_Val = string.Empty;
            SSBGCntArea2_Val = string.Empty;
            SSBGCntArea3_Val = string.Empty;
            SSBGCntArea4_Val = string.Empty;
            SSBGCntArea5_Val = string.Empty;
            SSBGCust1 = string.Empty;
            SSBGCust2 = string.Empty;
            SSBGCust3 = string.Empty;
            SSBGCust4 = string.Empty;
            SSBGCust5 = string.Empty;
            SSBGProgDesc = string.Empty;
            //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
            //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
            //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
            Mode = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            //Month= string.Empty;
            //CntlOblig = string.Empty;
        }

        public SSBGPARAMEntity(DataRow row)
        {
            if (row != null)
            {

                SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
                SSBGDept = row["SBGP_DEPT"].ToString().Trim();
                SSBGProgram = row["SBGP_PROG"].ToString().Trim();
                SSBGSeq = row["SBGP_SEQ"].ToString().Trim();
                //SSBGRPFrom = row["SBGP_RP_FROM"].ToString().Trim();
                //SSBGRPTo = row["SBGP_RP_TO"].ToString().Trim();
                SSBGYear = row["SBGP_YEAR"].ToString();
                //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
                SSBGGPFrom = row["SBGP_GP_FROM"].ToString().Trim();
                SSBGGPTo = row["SBGP_GP_TO"].ToString().Trim();
                SSBGBudget = row["SBGP_BUDGET"].ToString().Trim();
                SSBGAward = row["SBGP_GAWARD"].ToString().Trim();
                SSBGRPDate = row["SBGP_RP_DATE"].ToString().Trim();
                SSBGDesc = row["SBGP_DESC"].ToString().Trim();
                //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
                //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBGP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBGP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBGP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBGP_LSTC_OPERATOR"].ToString().Trim();
                SSBGCntArea1 = row["SBGP_CNTAREA1"].ToString().Trim();
                SSBGCntArea2 = row["SBGP_CNTAREA2"].ToString().Trim();
                SSBGCntArea3 = row["SBGP_CNTAREA3"].ToString().Trim();
                SSBGCntArea4 = row["SBGP_CNTAREA4"].ToString().Trim();
                SSBGCntArea5 = row["SBGP_CNTAREA5"].ToString().Trim();
                SSBGCntArea1_Chk = row["SBGP_CNTAREA1_CHK"].ToString().Trim();
                SSBGCntArea2_Chk = row["SBGP_CNTAREA2_CHK"].ToString().Trim();
                SSBGCntArea3_Chk = row["SBGP_CNTAREA3_CHK"].ToString().Trim();
                SSBGCntArea4_Chk = row["SBGP_CNTAREA4_CHK"].ToString().Trim();
                SSBGCntArea5_Chk = row["SBGP_CNTAREA5_CHK"].ToString().Trim();
                SSBGCntArea1_Val = row["SBGP_CNTAREA1_VAL"].ToString().Trim();
                SSBGCntArea2_Val = row["SBGP_CNTAREA2_VAL"].ToString().Trim();
                SSBGCntArea3_Val = row["SBGP_CNTAREA3_VAL"].ToString().Trim();
                SSBGCntArea4_Val = row["SBGP_CNTAREA4_VAL"].ToString().Trim();
                SSBGCntArea5_Val = row["SBGP_CNTAREA5_VAL"].ToString().Trim();
                SSBGCust1 = row["SBGP_CUST1"].ToString().Trim();
                SSBGCust2 = row["SBGP_CUST2"].ToString().Trim();
                SSBGCust3 = row["SBGP_CUST3"].ToString().Trim();
                SSBGCust4 = row["SBGP_CUST4"].ToString().Trim();
                SSBGCust5 = row["SBGP_CUST5"].ToString().Trim();
                SSBGProgDesc = row["SBGP_PROG_DESC"].ToString().Trim();

            }

        }
        public SSBGPARAMEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "Hierarchy")
                {
                    SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
                    SSBGDept = row["SBGP_DEPT"].ToString().Trim();
                    SSBGProgram = row["SBGP_PROG"].ToString().Trim();
                }
                else
                {
                    SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
                    SSBGDept = row["SBGP_DEPT"].ToString().Trim();
                    SSBGProgram = row["SBGP_PROG"].ToString().Trim();
                    SSBGSeq = row["SBGP_SEQ"].ToString().Trim();
                    //SSBGRPFrom = row["SBGP_RP_FROM"].ToString().Trim();
                    //SSBGRPTo = row["SBGP_RP_TO"].ToString().Trim();
                    SSBGYear = row["SBGP_YEAR"].ToString();
                    //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
                    SSBGGPFrom = row["SBGP_GP_FROM"].ToString().Trim();
                    SSBGGPTo = row["SBGP_GP_TO"].ToString().Trim();
                    SSBGBudget = row["SBGP_BUDGET"].ToString().Trim();
                    SSBGAward = row["SBGP_GAWARD"].ToString().Trim();
                    SSBGRPDate = row["SBGP_RP_DATE"].ToString().Trim();
                    SSBGDesc = row["SBGP_DESC"].ToString().Trim();
                    //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
                    //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                    //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                    DateAdd = row["SBGP_DATE_ADD"].ToString().Trim();
                    AddOperator = row["SBGP_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["SBGP_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["SBGP_LSTC_OPERATOR"].ToString().Trim();
                    SSBGCntArea1 = row["SBGP_CNTAREA1"].ToString().Trim();
                    SSBGCntArea2 = row["SBGP_CNTAREA2"].ToString().Trim();
                    SSBGCntArea3 = row["SBGP_CNTAREA3"].ToString().Trim();
                    SSBGCntArea4 = row["SBGP_CNTAREA4"].ToString().Trim();
                    SSBGCntArea5 = row["SBGP_CNTAREA5"].ToString().Trim();
                    SSBGCntArea1_Chk = row["SBGP_CNTAREA1_CHK"].ToString().Trim();
                    SSBGCntArea2_Chk = row["SBGP_CNTAREA2_CHK"].ToString().Trim();
                    SSBGCntArea3_Chk = row["SBGP_CNTAREA3_CHK"].ToString().Trim();
                    SSBGCntArea4_Chk = row["SBGP_CNTAREA4_CHK"].ToString().Trim();
                    SSBGCntArea5_Chk = row["SBGP_CNTAREA5_CHK"].ToString().Trim();
                    SSBGCntArea1_Val = row["SBGP_CNTAREA1_VAL"].ToString().Trim();
                    SSBGCntArea2_Val = row["SBGP_CNTAREA2_VAL"].ToString().Trim();
                    SSBGCntArea3_Val = row["SBGP_CNTAREA3_VAL"].ToString().Trim();
                    SSBGCntArea4_Val = row["SBGP_CNTAREA4_VAL"].ToString().Trim();
                    SSBGCntArea5_Val = row["SBGP_CNTAREA5_VAL"].ToString().Trim();
                    SSBGCust1 = row["SBGP_CUST1"].ToString().Trim();
                    SSBGCust2 = row["SBGP_CUST2"].ToString().Trim();
                    SSBGCust3 = row["SBGP_CUST3"].ToString().Trim();
                    SSBGCust4 = row["SBGP_CUST4"].ToString().Trim();
                    SSBGCust5 = row["SBGP_CUST5"].ToString().Trim();
                    SSBGProgDesc = row["SBGP_PROG_DESC"].ToString().Trim();

                    //if (strTable != "Group")
                    //{
                    //    EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
                    //    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
                    //    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
                    //    EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
                    //}
                }
            }

        }

        #endregion

        #region Properties

        public string SSBGAgency { get; set; }
        public string SSBGDept { get; set; }
        public string SSBGProgram { get; set; }
        public string SSBGYear { get; set; }
        public string SSBGSeq { get; set; }
        public string SSBGDesc { get; set; }
        //public string EligQuesScreen { get; set; }
        //public string EligQuesType { get; set; }
        //public string SSBGRPFrom { get; set; }
        //public string SSBGRPTo { get; set; }
        public string SSBGGPFrom { get; set; }
        public string SSBGGPTo { get; set; }
        public string SSBGBudget { get; set; }
        public string SSBGAward { get; set; }
        public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string SSBGCntArea1 { get; set; }
        public string SSBGCntArea2 { get; set; }
        public string SSBGCntArea3 { get; set; }
        public string SSBGCntArea4 { get; set; }
        public string SSBGCntArea5 { get; set; }
        public string SSBGCntArea1_Chk { get; set; }
        public string SSBGCntArea2_Chk { get; set; }
        public string SSBGCntArea3_Chk { get; set; }
        public string SSBGCntArea4_Chk { get; set; }
        public string SSBGCntArea5_Chk { get; set; }
        public string SSBGCntArea1_Val { get; set; }
        public string SSBGCntArea2_Val { get; set; }
        public string SSBGCntArea3_Val { get; set; }
        public string SSBGCntArea4_Val { get; set; }
        public string SSBGCntArea5_Val { get; set; }
        public string SSBGCust1 { get; set; }
        public string SSBGCust2 { get; set; }
        public string SSBGCust3 { get; set; }
        public string SSBGCust4 { get; set; }
        public string SSBGCust5 { get; set; }
        public string SSBGProgDesc { get; set; }
        public string Mode { get; set; }
        //public string CaseEligHXml { get; set; }
        //public string EligQuestionDesc { get; set; }
        //public string EligAgyCode { get; set; }
        //public string EligFileName { get; set; }
        //public string EligQuesFldName { get; set; }
        //public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class SSBGMONTHSEntity
    {
        #region Constructors

        public SSBGMONTHSEntity()
        {
            SSBGAgency = string.Empty;
            SSBGDept = string.Empty;
            SSBGProgram = string.Empty;
            SSBGYear = string.Empty;
            SSBGSeq = string.Empty;
            SSBGCode = string.Empty;
            //SSBGRPFrom = string.Empty;
            //SSBGRPTo = string.Empty;
            //EligQuesScreen = row["CASEELIG_QUES_SCREEN"].ToString();
            //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
            SSBGYearValue = string.Empty;
            //SSBGGPTo = string.Empty;
            //SSBGBudget = string.Empty;
            //SSBGAward = string.Empty;
            //SSBGRPDate = string.Empty;
            //SSBGDesc = string.Empty;
            //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
            //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
            //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
            Mode = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            Month = string.Empty;
            CntlOblig = string.Empty;
        }

        public SSBGMONTHSEntity(DataRow row)
        {
            if (row != null)
            {

                SSBGAgency = row["SBGM_AGENCY"].ToString().Trim();
                SSBGDept = row["SBGM_DEPT"].ToString().Trim();
                SSBGProgram = row["SBGM_PROG"].ToString().Trim();
                SSBGSeq = row["SBGM_SEQ"].ToString().Trim();
                SSBGCode = row["SBGM_CODE"].ToString().Trim();
                //SSBGRPTo = row["SBGP_RP_TO"].ToString().Trim();
                SSBGYear = row["SBGM_YEAR"].ToString();
                //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
                SSBGYearValue = row["SBGM_YEAR_VALUE"].ToString().Trim();
                //SSBGGPTo = row["SBGP_GP_TO"].ToString().Trim();
                //SSBGBudget = row["SBGP_BUDGET"].ToString().Trim();
                //SSBGAward = row["SBGP_GAWARD"].ToString().Trim();
                //SSBGRPDate = row["SBGP_RP_DATE"].ToString().Trim();
                //SSBGDesc = row["SBGP_DESC"].ToString().Trim();
                //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
                //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBGM_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBGM_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBGM_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBGM_LSTC_OPERATOR"].ToString().Trim();
                Month = row["SBGM_MONTH"].ToString().Trim();
                CntlOblig = row["SBGM_CONT_OBLIG"].ToString().Trim();
            }

        }
        //public SSBGPARAMEntity(DataRow row, string strTable)
        //{
        //    if (row != null)
        //    {
        //        if (strTable == "Hierarchy")
        //        {
        //            SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
        //            SSBGDept = row["SBGP_DEPT"].ToString().Trim();
        //            SSBGProgram = row["SBGP_PROG"].ToString().Trim();
        //        }
        //        else
        //        {
        //            SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
        //            SSBGDept = row["SBGP_DEPT"].ToString().Trim();
        //            SSBGProgram = row["SBGP_PROG"].ToString().Trim();
        //            SSBGSeq = row["SBGP_SEQ"].ToString().Trim();
        //            //SSBGRPFrom = row["SBGP_RP_FROM"].ToString().Trim();
        //            //SSBGRPTo = row["SBGP_RP_TO"].ToString().Trim();
        //            SSBGYear = row["SBGP_YEAR"].ToString();
        //            //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
        //            SSBGGPFrom = row["SBGP_GP_FROM"].ToString().Trim();
        //            SSBGGPTo = row["SBGP_GP_TO"].ToString().Trim();
        //            SSBGBudget = row["SBGP_BUDGET"].ToString().Trim();
        //            SSBGAward = row["SBGP_GAWARD"].ToString().Trim();
        //            SSBGRPDate = row["SBGP_RP_DATE"].ToString().Trim();
        //            SSBGDesc = row["SBGP_DESC"].ToString().Trim();
        //            //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
        //            //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
        //            //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
        //            DateAdd = row["SBGP_DATE_ADD"].ToString().Trim();
        //            AddOperator = row["SBGP_ADD_OPERATOR"].ToString().Trim();
        //            DateLstc = row["SBGP_DATE_LSTC"].ToString().Trim();
        //            LstcOperator = row["SBGP_LSTC_OPERATOR"].ToString().Trim();
        //            Month = row["SBGP_MONTH"].ToString().Trim();
        //            CntlOblig = row["SBGP_CONT_OBILG"].ToString().Trim();
        //            //if (strTable != "Group")
        //            //{
        //            //    EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
        //            //    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
        //            //    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
        //            //    EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
        //            //}
        //        }
        //    }

        //}

        #endregion

        #region Properties

        public string SSBGAgency { get; set; }
        public string SSBGDept { get; set; }
        public string SSBGProgram { get; set; }
        public string SSBGYear { get; set; }
        public string SSBGSeq { get; set; }
        //public string SSBGDesc { get; set; }
        //public string EligQuesScreen { get; set; }
        //public string EligQuesType { get; set; }
        //public string SSBGRPFrom { get; set; }
        //public string SSBGRPTo { get; set; }
        public string SSBGYearValue { get; set; }
        //public string SSBGGPTo { get; set; }
        //public string SSBGBudget { get; set; }
        //public string SSBGAward { get; set; }
        //public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Month { get; set; }
        public string CntlOblig { get; set; }
        public string Mode { get; set; }
        public string SSBGCode { get; set; }
        //public string EligQuestionDesc { get; set; }
        //public string EligAgyCode { get; set; }
        //public string EligFileName { get; set; }
        //public string EligQuesFldName { get; set; }
        //public string Type { get; set; }
        public string Msg { get; set; }
        #endregion
    }


    public class SSBGTYPESEntity
    {
        #region Constructors

        public SSBGTYPESEntity()
        {
            SBGTID = string.Empty;
            SBGTCode = string.Empty;
            SBGTCodeSeq = string.Empty;
            SBGTGroup = string.Empty;
            SBGTGroupSeq = string.Empty;
            SBGTDesc = string.Empty;
            SBGTQuesCode = string.Empty;
            SBGTMemAccess = string.Empty;
            SBGTRespType = string.Empty;
            SBGTConj = string.Empty;
            SBGTCntlOblig = string.Empty;
            SBGTNumEqual = string.Empty;
            SBGTNumLThan = string.Empty;
            SBGTNumGThan = string.Empty;
            SBGTDateEqual = string.Empty;
            SBGTDateLThan = string.Empty;
            SBGTDateGThan = string.Empty;
            SBGTRESP = string.Empty;
            Mode = string.Empty;
            Type = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

            SBGT_Name = string.Empty;
            SBGT_CNTAREA1 = string.Empty;
            SBGT_CNTAREA2 = string.Empty;
            SBGT_CNTAREA3 = string.Empty;
            SBGT_CNTAREA4 = string.Empty;
            SBGT_CNTAREA5 = string.Empty;
            SBGT_CNTAREA1_VAL = string.Empty;
            SBGT_CNTAREA2_VAL = string.Empty;
            SBGT_CNTAREA3_VAL = string.Empty;
            SBGT_CNTAREA4_VAL = string.Empty;
            SBGT_CNTAREA5_VAL = string.Empty;

            EligQuestionDesc = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligQuesFldName = string.Empty;

        }

        public SSBGTYPESEntity(DataRow row)
        {
            if (row != null)
            {

                SBGTID = row["SBGT_ID"].ToString().Trim();
                SBGTCode = row["SBGT_CODE"].ToString().Trim();
                SBGTCodeSeq = row["SBGT_CODE_SEQ"].ToString().Trim();
                SBGTGroup = row["SBGT_GROUP"].ToString().Trim();
                SBGTGroupSeq = row["SBGT_GROUP_SEQ"].ToString().Trim();
                SBGTDesc = row["SBGT_DESC"].ToString().Trim();
                SBGTQuesCode = row["SBGT_QUES_CODE"].ToString().Trim();
                SBGTMemAccess = row["SBGT_MEM_ACCESS"].ToString().Trim();
                SBGTRespType = row["SBGT_RESP_TYPE"].ToString();
                SBGTConj = row["SBGT_CONJN"].ToString();
                SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
                SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
                SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                SBGT_Name = row["SBGT_NAME"].ToString().Trim();
                //SBGT_CNTAREA2 = row["SBGT_CNTAREA2"].ToString().Trim();
                //SBGT_CNTAREA3 = row["SBGT_CNTAREA3"].ToString().Trim();
                //SBGT_CNTAREA4 = row["SBGT_CNTAREA4"].ToString().Trim();
                //SBGT_CNTAREA5 = row["SBGT_CNTAREA5"].ToString().Trim();
                //SBGT_CNTAREA1_VAL = row["SBGT_CNTAREA1_VAL"].ToString().Trim();
                //SBGT_CNTAREA2_VAL = row["SBGT_CNTAREA2_VAL"].ToString().Trim();
                //SBGT_CNTAREA3_VAL = row["SBGT_CNTAREA3_VAL"].ToString().Trim();
                //SBGT_CNTAREA4_VAL = row["SBGT_CNTAREA4_VAL"].ToString().Trim();
                //SBGT_CNTAREA5_VAL = row["SBGT_CNTAREA5_VAL"].ToString().Trim();
                ////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBGT_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBGT_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBGT_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBGT_LSTC_OPERATOR"].ToString().Trim();
            }

        }

        public SSBGTYPESEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if(strTable=="Hierarchy")
                {
                    //EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
                    //EligDept = row["CASEELIG_DEPT"].ToString().Trim();
                    //EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
                }
                else
                {
                    SBGTID = row["SBGT_ID"].ToString().Trim();
                    SBGTCode = row["SBGT_CODE"].ToString().Trim();
                    SBGTCodeSeq = row["SBGT_CODE_SEQ"].ToString().Trim();
                    SBGTGroup = row["SBGT_GROUP"].ToString().Trim();
                    SBGTGroupSeq = row["SBGT_GROUP_SEQ"].ToString().Trim();
                    SBGTDesc = row["SBGT_DESC"].ToString().Trim();
                    SBGTQuesCode = row["SBGT_QUES_CODE"].ToString().Trim();
                    SBGTMemAccess = row["SBGT_MEM_ACCESS"].ToString().Trim();
                    SBGTRespType = row["SBGT_RESP_TYPE"].ToString();
                    SBGTConj = row["SBGT_CONJN"].ToString();
                    SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
                    SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
                    SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                    SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                    SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                    SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                    SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                    SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                    SBGT_Name = row["SBGT_NAME"].ToString().Trim();
                    //SBGT_CNTAREA2 = row["SBGT_CNTAREA2"].ToString().Trim();
                    //SBGT_CNTAREA3 = row["SBGT_CNTAREA3"].ToString().Trim();
                    //SBGT_CNTAREA4 = row["SBGT_CNTAREA4"].ToString().Trim();
                    //SBGT_CNTAREA5 = row["SBGT_CNTAREA5"].ToString().Trim();
                    //SBGT_CNTAREA1_VAL = row["SBGT_CNTAREA1_VAL"].ToString().Trim();
                    //SBGT_CNTAREA2_VAL = row["SBGT_CNTAREA2_VAL"].ToString().Trim();
                    //SBGT_CNTAREA3_VAL = row["SBGT_CNTAREA3_VAL"].ToString().Trim();
                    //SBGT_CNTAREA4_VAL = row["SBGT_CNTAREA4_VAL"].ToString().Trim();
                    //SBGT_CNTAREA5_VAL = row["SBGT_CNTAREA5_VAL"].ToString().Trim();
                    ////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                    ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                    DateAdd = row["SBGT_DATE_ADD"].ToString().Trim();
                    AddOperator = row["SBGT_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["SBGT_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["SBGT_LSTC_OPERATOR"].ToString().Trim();
                    if(strTable!="Group")
                    {
                        EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
                        EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
                        EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
                        EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
                    }
                }
            }

        }
        //public SSBGTYPESEntity(DataRow row, string strTable)
        //{
        //    if (row != null)
        //    {
        //        if (strTable == "Hierarchy")
        //        {
        //            SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
        //            SSBGDept = row["SBGP_DEPT"].ToString().Trim();
        //            SSBGProgram = row["SBGP_PROG"].ToString().Trim();
        //        }
        //        else
        //        {
        //            SSBGAgency = row["SBGP_AGENCY"].ToString().Trim();
        //            SSBGDept = row["SBGP_DEPT"].ToString().Trim();
        //            SSBGProgram = row["SBGP_PROG"].ToString().Trim();
        //            SSBGSeq = row["SBGP_SEQ"].ToString().Trim();
        //            //SSBGRPFrom = row["SBGP_RP_FROM"].ToString().Trim();
        //            //SSBGRPTo = row["SBGP_RP_TO"].ToString().Trim();
        //            SSBGYear = row["SBGP_YEAR"].ToString();
        //            //EligQuesType = row["CASEELIG_QUES_TYPE"].ToString();
        //            SSBGGPFrom = row["SBGP_GP_FROM"].ToString().Trim();
        //            SSBGGPTo = row["SBGP_GP_TO"].ToString().Trim();
        //            SSBGBudget = row["SBGP_BUDGET"].ToString().Trim();
        //            SSBGAward = row["SBGP_GAWARD"].ToString().Trim();
        //            SSBGRPDate = row["SBGP_RP_DATE"].ToString().Trim();
        //            SSBGDesc = row["SBGP_DESC"].ToString().Trim();
        //            //EligNumLthan = row["CASEELIG_NUM_LTHAN"].ToString().Trim();
        //            //EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
        //            //EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
        //            DateAdd = row["SBGP_DATE_ADD"].ToString().Trim();
        //            AddOperator = row["SBGP_ADD_OPERATOR"].ToString().Trim();
        //            DateLstc = row["SBGP_DATE_LSTC"].ToString().Trim();
        //            LstcOperator = row["SBGP_LSTC_OPERATOR"].ToString().Trim();
        //            //if (strTable != "Group")
        //            //{
        //            //    EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
        //            //    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
        //            //    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
        //            //    EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
        //            //}
        //        }
        //    }

        //}

        #endregion

        #region Properties

        public string SBGTID { get; set; }
        public string SBGTCode { get; set; }
        public string SBGTCodeSeq { get; set; }
        public string SBGTGroup { get; set; }
        public string SBGTGroupSeq { get; set; }
        public string SBGTDesc { get; set; }
        public string SBGTQuesCode { get; set; }
        public string SBGTMemAccess { get; set; }
        public string SBGTRespType { get; set; }
        public string SBGTConj { get; set; }
        public string SBGTCntlOblig { get; set; }
        public string SBGTNumEqual { get; set; }
        public string SBGTNumLThan { get; set; }
        public string SBGTNumGThan { get; set; }
        public string SBGTDateEqual { get; set; }
        public string SBGTDateLThan { get; set; }
        public string SBGTDateGThan { get; set; }
        public string SBGTRESP { get; set; }
        public string SBGT_CNTAREA1 { get; set; }
        public string SBGT_CNTAREA2 { get; set; }
        public string SBGT_CNTAREA3 { get; set; }
        public string SBGT_CNTAREA4 { get; set; }
        public string SBGT_CNTAREA5 { get; set; }
        public string SBGT_CNTAREA1_VAL { get; set; }
        public string SBGT_CNTAREA2_VAL { get; set; }
        public string SBGT_CNTAREA3_VAL { get; set; }
        public string SBGT_CNTAREA4_VAL { get; set; }
        public string SBGT_CNTAREA5_VAL { get; set; }
        public string SBGT_Name { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }

        public string EligQuestionDesc { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligQuesFldName { get; set; }
        #endregion
    }

    public class SSBGGOALSEntity
    {
        #region Constructors

        public SSBGGOALSEntity()
        {
            SBGGID = string.Empty;
            SBGGCode = string.Empty;
            SBGGSeq = string.Empty;
            SBGGDesc = string.Empty;
            SBGGCountType = string.Empty;
            //SBGTQuesCode = string.Empty;
            //SBGTMemAccess = string.Empty;
            //SBGTRespType = string.Empty;
            //SBGTConj = string.Empty;
            //SBGTCntlOblig = string.Empty;
            //SBGTNumEqual = string.Empty;
            //SBGTNumLThan = string.Empty;
            //SBGTNumGThan = string.Empty;
            //SBGTDateEqual = string.Empty;
            //SBGTDateLThan = string.Empty;
            //SBGTDateGThan = string.Empty;
            //SBGTRESP = string.Empty;
            Type2 = string.Empty;
            Mode = string.Empty;
            Type = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

            EligQuestionDesc = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligQuesFldName = string.Empty;

        }

        public SSBGGOALSEntity(DataRow row)
        {
            if (row != null)
            {

                SBGGID = row["SBGG_ID"].ToString().Trim();
                SBGGCode = row["SBGG_CODE"].ToString().Trim();
                SBGGSeq = row["SBGG_SEQ"].ToString().Trim();
                SBGGDesc = row["SBGG_DESC"].ToString().Trim();
                SBGGCountType = row["SBGG_COUNT_TYPE"].ToString().Trim();
                Type2 = row["SBGG_TYPE2"].ToString().Trim();
                //SBGTQuesCode = row["SBGT_QUES_CODE"].ToString().Trim();
                //SBGTMemAccess = row["SBGT_MEM_ACCESS"].ToString().Trim();
                //SBGTRespType = row["SBGT_RESP_TYPE"].ToString();
                //SBGTConj = row["SBGT_CONJN"].ToString();
                //SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
                //SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
                //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBGG_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBGG_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBGG_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBGG_LSTC_OPERATOR"].ToString().Trim();
            }

        }

        public SSBGGOALSEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "Hierarchy")
                {
                    //EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
                    //EligDept = row["CASEELIG_DEPT"].ToString().Trim();
                    //EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
                }
                else
                {
                    SBGGID = row["SBGG_ID"].ToString().Trim();
                    SBGGCode = row["SBGG_CODE"].ToString().Trim();
                    SBGGSeq = row["SBGG_SEQ"].ToString().Trim();
                    SBGGDesc = row["SBGG_DESC"].ToString().Trim();
                    SBGGCountType = row["SBGG_COUNT_TYPE"].ToString().Trim();
                    Type2 = row["SBGG_TYPE2"].ToString().Trim();
                    //SBGTMemAccess = row["SBGT_MEM_ACCESS"].ToString().Trim();
                    //SBGTRespType = row["SBGT_RESP_TYPE"].ToString();
                    //SBGTConj = row["SBGT_CONJN"].ToString();
                    //SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
                    //SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
                    //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                    //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                    //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                    //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                    //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                    //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                    //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                    ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                    DateAdd = row["SBGG_DATE_ADD"].ToString().Trim();
                    AddOperator = row["SBGG_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["SBGG_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["SBGG_LSTC_OPERATOR"].ToString().Trim();
                    //if (strTable != "Group")
                    //{
                    //    EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
                    //    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
                    //    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
                    //    EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
                    //}
                }
            }

        }
     
        #endregion

        #region Properties

        public string SBGGID { get; set; }
        public string SBGGCode { get; set; }
        public string SBGGSeq { get; set; }
        public string SBGGDesc { get; set; }
        public string SBGGCountType { get; set; }
        //public string SBGTMemAccess { get; set; }
        //public string SBGTRespType { get; set; }
        //public string SBGTConj { get; set; }
        //public string SBGTCntlOblig { get; set; }
        //public string SBGTNumEqual { get; set; }
        //public string SBGTNumLThan { get; set; }
        //public string SBGTNumGThan { get; set; }
        //public string SBGTDateEqual { get; set; }
        //public string SBGTDateLThan { get; set; }
        //public string SBGTDateGThan { get; set; }
        //public string SBGTRESP { get; set; }
        ////public string SSBGGPTo { get; set; }
        //public string SSBGBudget { get; set; }
        //public string SSBGAward { get; set; }
        //public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }
        public string Type2 { get; set; }

        public string EligQuestionDesc { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligQuesFldName { get; set; }
        #endregion
    }

    public class SSBGSerPlanEntity
    {
        #region Constructors

        public SSBGSerPlanEntity()
        {
            SBSPID = string.Empty;
            SBSPGoal = string.Empty;
            SBSPOBJ = string.Empty;
            SBSPSerPlan = string.Empty;
            SBSPSeq = string.Empty;
            SBSPSPName = string.Empty;
            SBSP_Assoc_CAMS_Cnt = string.Empty;
            //SBGTConj = string.Empty;
            //SBGTCntlOblig = string.Empty;
            //SBGTNumEqual = string.Empty;
            //SBGTNumLThan = string.Empty;
            //SBGTNumGThan = string.Empty;
            //SBGTDateEqual = string.Empty;
            //SBGTDateLThan = string.Empty;
            //SBGTDateGThan = string.Empty;
            //SBGTRESP = string.Empty;
            Mode = string.Empty;
            Type = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

            EligQuestionDesc = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligQuesFldName = string.Empty;

        }

        public SSBGSerPlanEntity(SSBGSerPlanEntity Entity)
        {
            Mode = Entity.Mode;
            SBSPID = Entity.SBSPID;
            SBSPGoal = Entity.SBSPGoal;
            SBSPOBJ = Entity.SBSPOBJ;
            SBSPSerPlan = Entity.SBSPSerPlan;
            SBSPSeq = Entity.SBSPSeq;
            SBSPSPName = Entity.SBSPSPName;
            //TRAGSM_PROGRAM = Entity.TRAGSM_PROGRAM;
            //Trig_Assoc_CAMS_Cnt = Entity.Trig_Assoc_CAMS_Cnt;

            AddOperator = Entity.AddOperator;
            DateAdd = Entity.DateAdd;
            LstcOperator = Entity.LstcOperator;
            DateLstc = Entity.DateLstc;

        }

        public SSBGSerPlanEntity(DataRow row)
        {
            if (row != null)
            {

                SBSPID = row["SBSP_ID"].ToString().Trim();
                SBSPGoal = row["SBSP_GOAL"].ToString().Trim();
                SBSPOBJ = row["SBSP_OBJ"].ToString().Trim();
                SBSPSerPlan = row["SBSP_SERPLAN"].ToString().Trim();
                SBSPSeq = row["SBSP_SEQ"].ToString().Trim();
                SBSPSPName = row["SP_DESC"].ToString().Trim();
                SBSP_Assoc_CAMS_Cnt = row["Assoc_CAMS_Cnt"].ToString();
                //SBGTConj = row["SBGT_CONJN"].ToString();
                //SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
                //SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
                //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBSP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBSP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBSP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBSP_LSTC_OPERATOR"].ToString().Trim();
            }

        }

        //public SSBGGOALSEntity(DataRow row, string strTable)
        //{
        //    if (row != null)
        //    {
        //        if (strTable == "Hierarchy")
        //        {
        //            //EligAgency = row["CASEELIG_AGENCY"].ToString().Trim();
        //            //EligDept = row["CASEELIG_DEPT"].ToString().Trim();
        //            //EligProgram = row["CASEELIG_PROGRAM"].ToString().Trim();
        //        }
        //        else
        //        {
        //            SBSPID = row["SBGG_ID"].ToString().Trim();
        //            SBGGCode = row["SBGG_CODE"].ToString().Trim();
        //            SBGGSeq = row["SBGG_SEQ"].ToString().Trim();
        //            SBGGDesc = row["SBGG_DESC"].ToString().Trim();
        //            //SBGTQuesCode = row["SBGT_QUES_CODE"].ToString().Trim();
        //            //SBGTMemAccess = row["SBGT_MEM_ACCESS"].ToString().Trim();
        //            //SBGTRespType = row["SBGT_RESP_TYPE"].ToString();
        //            //SBGTConj = row["SBGT_CONJN"].ToString();
        //            //SBGTCntlOblig = row["SBGT_CONT_OBILG"].ToString().Trim();
        //            //SBGTNumEqual = row["SBGT_NUM_EQUAL"].ToString().Trim();
        //            //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
        //            //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
        //            //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
        //            //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
        //            //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
        //            //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
        //            //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
        //            ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
        //            DateAdd = row["SBGG_DATE_ADD"].ToString().Trim();
        //            AddOperator = row["SBGG_ADD_OPERATOR"].ToString().Trim();
        //            DateLstc = row["SBGG_DATE_LSTC"].ToString().Trim();
        //            LstcOperator = row["SBGG_LSTC_OPERATOR"].ToString().Trim();
        //            //if (strTable != "Group")
        //            //{
        //            //    EligQuestionDesc = row["ELIGQUES_DESC"].ToString().Trim();
        //            //    EligAgyCode = row["ELIGQUES_AGY_CODE"].ToString().Trim();
        //            //    EligFileName = row["ELIGQUES_FILENAME"].ToString().Trim();
        //            //    EligQuesFldName = row["ELIGQUES_FLDNAME"].ToString().Trim();
        //            //}
        //        }
        //    }

        //}

        #endregion

        #region Properties

        public string SBSPID { get; set; }
        public string SBSPGoal { get; set; }
        public string SBSPOBJ { get; set; }
        public string SBSPSerPlan { get; set; }
        public string SBSPSeq{ get; set; }
        public string SBSPSPName { get; set; }
        public string SBSP_Assoc_CAMS_Cnt { get; set; }
        //public string SBGTConj { get; set; }
        //public string SBGTCntlOblig { get; set; }
        //public string SBGTNumEqual { get; set; }
        //public string SBGTNumLThan { get; set; }
        //public string SBGTNumGThan { get; set; }
        //public string SBGTDateEqual { get; set; }
        //public string SBGTDateLThan { get; set; }
        //public string SBGTDateGThan { get; set; }
        //public string SBGTRESP { get; set; }
        ////public string SSBGGPTo { get; set; }
        //public string SSBGBudget { get; set; }
        //public string SSBGAward { get; set; }
        //public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }

        public string EligQuestionDesc { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligQuesFldName { get; set; }
        #endregion
    }

    public class SSBGAssnsEntity
    {
        #region Constructors

        public SSBGAssnsEntity()
        {
            SBASID = string.Empty;
            SBASGoal = string.Empty;
            SBASOBJ = string.Empty;
            SBASSerPlan = string.Empty;
            SBASSeq = string.Empty;
            SBASBranch = string.Empty;
            SBAS_org_Grp = string.Empty;
            SBAS_CAMS_Type = string.Empty;
            SBAS_CAMS_Code = string.Empty;
            CAMS_DESC= string.Empty;
            //SBGTNumLThan = string.Empty;
            //SBGTNumGThan = string.Empty;
            //SBGTDateEqual = string.Empty;
            //SBGTDateLThan = string.Empty;
            //SBGTDateGThan = string.Empty;
            //SBGTRESP = string.Empty;
            Mode = string.Empty;
            Type = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

            EligQuestionDesc = string.Empty;
            EligAgyCode = string.Empty;
            EligFileName = string.Empty;
            EligQuesFldName = string.Empty;

        }

        public SSBGAssnsEntity(SSBGAssnsEntity Entity)
        {
            Mode = Entity.Mode;
            SBASID = Entity.SBASID;
            SBASGoal = Entity.SBASGoal;
            SBASOBJ = Entity.SBASOBJ;
            SBASSerPlan = Entity.SBASSerPlan;
            SBASSeq = Entity.SBASSeq;
            SBASBranch = Entity.SBASBranch;
            SBAS_org_Grp = Entity.SBAS_org_Grp;
            SBAS_CAMS_Type= Entity.SBAS_CAMS_Type;
            SBAS_CAMS_Code= Entity.SBAS_CAMS_Code;
            CAMS_DESC = Entity.CAMS_DESC;
            //TRAGSM_PROGRAM = Entity.TRAGSM_PROGRAM;
            //Trig_Assoc_CAMS_Cnt = Entity.Trig_Assoc_CAMS_Cnt;

            AddOperator = Entity.AddOperator;
            DateAdd = Entity.DateAdd;
            LstcOperator = Entity.LstcOperator;
            DateLstc = Entity.DateLstc;

        }

        public SSBGAssnsEntity(DataRow row)
        {
            if (row != null)
            {

                SBASID = row["SBAS_ID"].ToString().Trim();
                SBASGoal = row["SBAS_GOAL"].ToString().Trim();
                SBASOBJ = row["SBAS_OBJ"].ToString().Trim();
                SBASSerPlan = row["SBAS_SERPLAN"].ToString().Trim();
                SBASSeq = row["SBAS_SEQ"].ToString().Trim();
                SBASBranch = row["SBAS_BRANCH"].ToString().Trim();
                SBAS_org_Grp = row["SBAS_ORIG_GRP"].ToString();
                SBAS_CAMS_Type = row["SBAS_CAMS"].ToString();
                SBAS_CAMS_Code = row["SBAS_CAMS_CODE"].ToString().Trim();
                CAMS_DESC = row["CAMS_DESC"].ToString().Trim();
                //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
                //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
                //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
                //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
                //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
                //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
                //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
                ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
                DateAdd = row["SBAS_DATE_ADD"].ToString().Trim();
                AddOperator = row["SBAS_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SBAS_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SBAS_LSTC_OPERATOR"].ToString().Trim();
            }

        }

        #endregion

        #region Properties

        public string SBASID { get; set; }
        public string SBASGoal { get; set; }
        public string SBASOBJ { get; set; }
        public string SBASSerPlan { get; set; }
        public string SBASSeq { get; set; }
        public string SBASBranch { get; set; }
        public string SBAS_org_Grp { get; set; }
        public string SBAS_CAMS_Type { get; set; }
        public string SBGTCntlOblig { get; set; }
        public string SBAS_CAMS_Code { get; set; }
        public string CAMS_DESC { get; set; }
        //public string SBGTNumGThan { get; set; }
        //public string SBGTDateEqual { get; set; }
        //public string SBGTDateLThan { get; set; }
        //public string SBGTDateGThan { get; set; }
        //public string SBGTRESP { get; set; }
        ////public string SSBGGPTo { get; set; }
        //public string SSBGBudget { get; set; }
        //public string SSBGAward { get; set; }
        //public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }

        public string EligQuestionDesc { get; set; }
        public string EligAgyCode { get; set; }
        public string EligFileName { get; set; }
        public string EligQuesFldName { get; set; }
        #endregion
    }

    public class SSBGDemoEntity
    {
        #region Constructors

        public SSBGDemoEntity()
        {
            SSBG18to59 = string.Empty;
            SSBG60Plus = string.Empty;
            SSBGAdults = string.Empty;
            SSBGAsian = string.Empty;
            SSBGBlack = string.Empty;
            SSBGAmerican= string.Empty;
            SSBGFemale = string.Empty;
            SSBGHispanic = string.Empty;
            SSBGMale = string.Empty;
            SSBGMultiRace = string.Empty;
            SSBGNative = string.Empty;
            SSBGNonHispanic = string.Empty;
            SSBGOther = string.Empty;
            SSBGWhite = string.Empty;
            //SBGTDateGThan = string.Empty;
            //SBGTRESP = string.Empty;
            //Mode = string.Empty;
            //Type = string.Empty;
            //DateAdd = string.Empty;
            //AddOperator = string.Empty;
            //DateLstc = string.Empty;
            //LstcOperator = string.Empty;

            //EligQuestionDesc = string.Empty;
            //EligAgyCode = string.Empty;
            //EligFileName = string.Empty;
            //EligQuesFldName = string.Empty;

        }

        public SSBGDemoEntity(bool Intialize)
        {
            SSBG18to59 = 
            SSBG60Plus = 
            SSBGAdults = 
            SSBGAsian = 
            SSBGBlack = 
            SSBGAmerican=
            SSBGFemale =
            SSBGHispanic =
            SSBGMale = 
            SSBGMultiRace =
            SSBGNative = 
            SSBGNonHispanic = 
            SSBGOther = 
            SSBGWhite = "0";
            

        }

        public SSBGDemoEntity(SSBGDemoEntity Entity)
        {
            SSBG18to59 = Entity.SSBG18to59;
            SSBG60Plus = Entity.SSBG60Plus;
            SSBGAdults = Entity.SSBGAdults;
            SSBGAsian = Entity.SSBGAsian;
            SSBGBlack = Entity.SSBGBlack;
            SSBGAmerican= Entity.SSBGAmerican;
            SSBGFemale = Entity.SSBGFemale;
            SSBGHispanic = Entity.SSBGHispanic;
            SSBGMale = Entity.SSBGMale;
            SSBGMultiRace = Entity.SSBGMultiRace;
            SSBGNative = Entity.SSBGNative;
            SSBGNonHispanic = Entity.SSBGNonHispanic;
            SSBGOther = Entity.SSBGOther;
            SSBGWhite = Entity.SSBGWhite;

        }

        //public SSBGAssnsEntity(DataRow row)
        //{
        //    if (row != null)
        //    {

        //        SBASID = row["SBAS_ID"].ToString().Trim();
        //        SBASGoal = row["SBAS_GOAL"].ToString().Trim();
        //        SBASOBJ = row["SBAS_OBJ"].ToString().Trim();
        //        SBASSerPlan = row["SBAS_SERPLAN"].ToString().Trim();
        //        SBASSeq = row["SBAS_SEQ"].ToString().Trim();
        //        SBASBranch = row["SBAS_BRANCH"].ToString().Trim();
        //        SBAS_org_Grp = row["SBAS_ORIG_GRP"].ToString();
        //        SBAS_CAMS_Type = row["SBAS_CAMS"].ToString();
        //        SBAS_CAMS_Code = row["SBAS_CAMS_CODE"].ToString().Trim();
        //        CAMS_DESC = row["CAMS_DESC"].ToString().Trim();
        //        //SBGTNumLThan = row["SBGT_NUM_LTHAN"].ToString().Trim();
        //        //SBGTNumGThan = row["SBGT_NUM_GTHAN"].ToString().Trim();
        //        //SBGTDateEqual = row["SBGT_DATE_EQUAL"].ToString().Trim();
        //        //SBGTDateLThan = row["SBGT_DATE_LTHAN"].ToString().Trim();
        //        //SBGTDateGThan = row["SBGT_DATE_GTHAN"].ToString().Trim();
        //        //SBGTRESP = row["SBGT_DD_TEXT_RESP"].ToString().Trim();
        //        //////EligDDTextResp = row["CASEELIG_DD_TEXT_RESP"].ToString().Trim();
        //        ////EligPoints = row["CASEELIG_POINTS"].ToString().Trim();
        //        DateAdd = row["SBAS_DATE_ADD"].ToString().Trim();
        //        AddOperator = row["SBAS_ADD_OPERATOR"].ToString().Trim();
        //        DateLstc = row["SBAS_DATE_LSTC"].ToString().Trim();
        //        LstcOperator = row["SBAS_LSTC_OPERATOR"].ToString().Trim();
        //    }

        //}

        #endregion

        #region Properties

        public string SSBG18to59 { get; set; }
        public string SSBG60Plus { get; set; }
        public string SSBGAdults { get; set; }
        public string SSBGAmerican { get; set; }
        public string SSBGBlack { get; set; }
        public string SSBGWhite { get; set; }
        public string SSBGAsian { get; set; }
        public string SSBGNative { get; set; }
        public string SSBGOther { get; set; }
        public string SSBGMultiRace { get; set; }
        public string SSBGHispanic { get; set; }
        public string SSBGNonHispanic { get; set; }
        public string SSBGMale { get; set; }
        public string SSBGFemale { get; set; }
        //public string SBGTDateLThan { get; set; }
        //public string SBGTDateGThan { get; set; }
        //public string SBGTRESP { get; set; }
        ////public string SSBGGPTo { get; set; }
        //public string SSBGBudget { get; set; }
        //public string SSBGAward { get; set; }
        //public string SSBGRPDate { get; set; }
        //public string EligDDTextResp { get; set; }
        //public string EligPoints { get; set; }
       
        #endregion
    }

}
