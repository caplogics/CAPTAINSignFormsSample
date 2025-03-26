using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{

    public class FLDCNTLEntity
    {
        #region Constructors

        public FLDCNTLEntity()
        {
            ScrCode = string.Empty;
            ScrHie = string.Empty;
            Valid = string.Empty;
            AddDate = string.Empty;
            AddOpr = string.Empty;
            ChgDate = string.Empty;
            ChgOpr = string.Empty;
        }

        public FLDCNTLEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["FLD_SCR_CODE"].ToString().Trim();
                ScrHie = row["FLD_SCR_HIE"].ToString().Trim();
                Valid = row["FLD_SCR_VALID"].ToString().Trim();
                AddDate = row["FLD_DATE_ADD"].ToString().Trim();
                AddOpr = row["FLD_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["FLD_DATE_LSTC"].ToString().Trim();
                ChgOpr = row["FLD_LSTC_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string ScrCode { get; set; }
        public string ScrHie { get; set; }
        public string Valid { get; set; }
        public string AddDate { get; set; }
        public string AddOpr { get; set; }
        public string ChgDate { get; set; }
        public string ChgOpr { get; set; }

        #endregion
    }

    public class FldcntlHieEntity
    {
        #region Constructors

        public FldcntlHieEntity()
        {
            ScrCode = string.Empty;
            ScrHie = string.Empty;
            FldCode = string.Empty;
            Active = string.Empty;
            Enab = string.Empty;
            Req = string.Empty;
            Shared = string.Empty;
            FldDesc = string.Empty;
        }

        public FldcntlHieEntity(DataRow AgyTabControl)
        {

            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["FLDH_SCR_CODE"].ToString().Trim();
                ScrHie = row["FLDH_SCR_HIE"].ToString().Trim();
                FldCode = row["FLDH_CODE"].ToString().Trim();
                Active = row["FLDH_ACTIVE"].ToString().Trim();
                Enab = row["FLDH_ENABLED"].ToString().Trim();
                Req = row["FLDH_REQUIRED"].ToString().Trim();
                Shared = row["FLDH_SHARED"].ToString().Trim();
                FldDesc = row["STF_DESC"].ToString().Trim();
            }
        }


        #endregion

        #region Properties

        public string ScrCode { get; set; }
        public string ScrHie { get; set; }
        public string FldCode { get; set; }
        public string Active { get; set; }
        public string Enab { get; set; }
        public string Req { get; set; }
        public string Shared { get; set; }
        public string FldDesc { get; set; }

        #endregion
    }


    public class CustfldsEntity
    {
        #region Constructors

        public CustfldsEntity()
        {
            ScrCode = string.Empty;
            CustCode = string.Empty;
            CustDesc = string.Empty;
            RespType = string.Empty;
            Sub_Screen = string.Empty;
            MemAccess = string.Empty;
            Equalto = string.Empty;
            Greater = string.Empty;
            Less = string.Empty;
            Alpha = string.Empty;
            Other = string.Empty;
            Question = string.Empty;
            FutureDate = string.Empty;
            AddDate = string.Empty;
            AddOpr = string.Empty;
            ChgDate = string.Empty;
            ChdOpr = string.Empty;
            CustSeq = string.Empty;
            UpdateType = string.Empty;
            Active = string.Empty;
            custSpanishDesc = string.Empty;
            custSendtoPip = string.Empty;
            custPIPActive = string.Empty;
        }

        public CustfldsEntity(bool Intialize)
        {
            if (Intialize)
            {
                ScrCode = null;
                CustCode = null;
                CustDesc = null;
                RespType = null;
                Sub_Screen = null;
                MemAccess = null;
                Equalto = null;
                Greater = null;
                Less = null;
                Alpha = null;
                Other = null;
                Question = null;
                FutureDate = null;
                AddDate = null;
                AddOpr = null;
                ChgDate = null;
                ChdOpr = null;
                CustSeq = null;
                UpdateType = null;
                Active = null;
                custSpanishDesc = null;
                custSendtoPip = null;
                custPIPActive = null;
            }
        }

        public CustfldsEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["CUST_SCR_CODE"].ToString().Trim();
                CustCode = row["CUST_CODE"].ToString().Trim();
                CustDesc = row["CUST_DESC"].ToString().Trim();
                RespType = row["CUST_RESP_TYPE"].ToString().Trim();
                Sub_Screen = row["CUST_SUB_SCR"].ToString().Trim();
                MemAccess = row["CUST_MEM_ACCESS"].ToString().Trim();
                Equalto = row["CUST_EQUAL"].ToString().Trim();
                Greater = row["CUST_GREATER"].ToString().Trim();
                Less = row["CUST_LESSTHAN"].ToString().Trim();
                Alpha = row["CUST_ALPHA"].ToString().Trim();
                Other = row["CUST_OTHER"].ToString().Trim();
                Question = row["CUST_ABBR_QUESTION"].ToString().Trim();
                FutureDate = row["CUST_ALLOW_FDATE"].ToString().Trim();
                AddDate = row["CUST_DATE_ADD"].ToString().Trim();
                AddOpr = row["CUST_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["CUST_DATE_LSTC"].ToString().Trim();
                ChdOpr = row["CUST_LSTC_OPERATOR"].ToString().Trim();
                CustSeq = row["CUST_SEQ"].ToString().Trim();
                Active = row["CUST_ACTIVE"].ToString().Trim();
                custSpanishDesc = row["CUST_SDESC"].ToString().Trim();
                custSendtoPip = string.Empty;
                if (row.Table.Columns.Contains("CUST_SEND_PIP"))
                    custSendtoPip = row["CUST_SEND_PIP"].ToString().Trim();
                if (row.Table.Columns.Contains("CUST_PIP_ACTIVE"))
                    custPIPActive = row["CUST_PIP_ACTIVE"].ToString().Trim();
                UpdateType = "U";
            }
        }

        public CustfldsEntity(DataRow AgyTabControl, string strEmpty)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["CUST_SCR_CODE"].ToString().Trim();
                CustCode = row["CUST_CODE"].ToString().Trim();
                CustDesc = row["CUST_DESC"].ToString().Trim();
                RespType = row["CUST_RESP_TYPE"].ToString().Trim();
                Sub_Screen = row["CUST_SUB_SCR"].ToString().Trim();
                MemAccess = row["CUST_MEM_ACCESS"].ToString().Trim();
                Equalto = row["CUST_EQUAL"].ToString().Trim();
                Greater = row["CUST_GREATER"].ToString().Trim();
                Less = row["CUST_LESSTHAN"].ToString().Trim();
                Alpha = row["CUST_ALPHA"].ToString().Trim();
                Other = row["CUST_OTHER"].ToString().Trim();
                Question = row["CUST_ABBR_QUESTION"].ToString().Trim();
                FutureDate = row["CUST_ALLOW_FDATE"].ToString().Trim();
                AddDate = row["CUST_DATE_ADD"].ToString().Trim();
                AddOpr = row["CUST_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["CUST_DATE_LSTC"].ToString().Trim();
                ChdOpr = row["CUST_LSTC_OPERATOR"].ToString().Trim();
                CustSeq = row["CUST_SEQ"].ToString().Trim();
                Active = row["CUST_ACTIVE"].ToString().Trim();
                custReqData = row["FLDH_REQUIRED"].ToString().Trim();
                UpdateType = "U";
            }
        }

        #endregion

        #region Properties

        public string ScrCode { get; set; }
        public string CustCode { get; set; }
        public string CustDesc { get; set; }
        public string RespType { get; set; }
        public string Sub_Screen { get; set; }
        public string MemAccess { get; set; }
        public string Equalto { get; set; }
        public string Greater { get; set; }
        public string Less { get; set; }
        public string Alpha { get; set; }
        public string Other { get; set; }
        public string Question { get; set; }
        public string FutureDate { get; set; }
        public string AddDate { get; set; }
        public string AddOpr { get; set; }
        public string ChgDate { get; set; }
        public string ChdOpr { get; set; }
        public string CustSeq { get; set; }
        public string UpdateType { get; set; }
        public string Active { get; set; }
        public string custReqData { get; set; }
        public string custSpanishDesc { get; set; }
        public string custSendtoPip { get; set; }
        public string custPIPActive { get; set; }

        #endregion
    }

    public class FLDSCRSEntity
    {
        #region Constructors

        public FLDSCRSEntity()
        {
            Called_By =
            Scr_Code =
            Scr_Sub_Code =
            Scr_Desc =
            Cust_Ques_SW = string.Empty;
        }

        public FLDSCRSEntity(bool Initialize)
        {
            if (Initialize)
            {
                Called_By =
                Scr_Code =
                Scr_Sub_Code =
                Scr_Desc =
                Cust_Ques_SW = null;
            }
        }

        public FLDSCRSEntity(DataRow row)
        {
            if (row != null)
            {
                Called_By = row["FLDSCRS_CLDBY"].ToString().Trim();
                Scr_Code = row["FLDSCRS_CODE"].ToString().Trim();
                Scr_Sub_Code = row["FLDSCRS_SUB"].ToString().Trim();
                Scr_Desc = row["FLDSCRS_DESC"].ToString().Trim();
                Cust_Ques_SW = row["FLDSCRS_CUST"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Called_By { get; set; }
        public string Scr_Code { get; set; }
        public string Scr_Sub_Code { get; set; }
        public string Scr_Desc { get; set; }
        public string Cust_Ques_SW { get; set; }

        #endregion
    }

    public class Stdflds
    {
        #region Constructors

        public Stdflds()
        {
            ScrCode = string.Empty;
            StfCode = string.Empty;
            StftDesc = string.Empty;
            RespType = string.Empty;
            Agycode = string.Empty;
            SubScrCode = string.Empty;
        }

        public Stdflds(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["STF_SCR_CODE"].ToString().Trim();
                StfCode = row["STF_FLD_CODE"].ToString().Trim();
                StftDesc = row["STF_DESC"].ToString().Trim();
                RespType = row["STF_RESP_TYPE"].ToString().Trim();
                Agycode = row["STF_AGY_CODE"].ToString().Trim();
                SubScrCode = row["STF_SUB_SCR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string ScrCode { get; set; }
        public string StfCode { get; set; }
        public string StftDesc { get; set; }
        public string RespType { get; set; }
        public string Agycode { get; set; }
        public string SubScrCode { get; set; }
        public string Greater { get; set; }
        public string Less { get; set; }
        public string Alpha { get; set; }
        public string Other { get; set; }
        public string Question { get; set; }
        public string FutureDate { get; set; }
        public string AddDate { get; set; }
        public string AddOpr { get; set; }
        public string ChgDate { get; set; }
        public string ChdOpr { get; set; }
        public string CustSeq { get; set; }

        #endregion
    }


    public class CustRespEntity
    {
        #region Constructors

        public CustRespEntity()
        {
            RecType = string.Empty;
            ScrCode = string.Empty;
            ResoCode = string.Empty;
            RespSeq = string.Empty;
            RespDesc = string.Empty;
            DescCode = string.Empty;
            AddDate = string.Empty;
            AddOpr = string.Empty;
            ChgDate = string.Empty;
            ChgOpr = string.Empty;
            Changed = string.Empty;
            Points = string.Empty;
            RspStatus = string.Empty;
            RespNCode = string.Empty;
            RespDefault = string.Empty;
            RespSpanishDesc = string.Empty;
        }

        public CustRespEntity(bool Intialize)
        {
            if (Intialize)
            {
                RecType = null;
                ScrCode = null;
                ResoCode = null;
                RespSeq = null;
                RespDesc = null;
                DescCode = null;
                AddDate = null;
                AddOpr = null;
                ChgDate = null;
                ChgOpr = null;
                Changed = null;
                Points = null;
                RspStatus = null;
                RespNCode = null;
                RespDefault = null;
                RespSpanishDesc = null;
            }
        }

        public CustRespEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "U";
                ScrCode = row["RSP_SCR_CODE"].ToString();
                ResoCode = row["RSP_CUST_CODE"].ToString().Trim();
                RespSeq = row["RSP_SEQ"].ToString().Trim();
                RespDesc = row["RSP_DESC"].ToString().Trim();
                DescCode = row["RSP_RESP_CODE"].ToString().Trim();
                AddDate = row["RSP_DATE_ADD"].ToString().Trim();
                AddOpr = row["RSP_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["RSP_DATE_LSTC"].ToString().Trim();
                ChgOpr = row["RSP_LSTC_OPERATOR"].ToString().Trim();
                Points = row["RSP_POINTS"].ToString().Trim();
                RspStatus = row["RSP_STATUS"].ToString().Trim();
                RespNCode = row["RSP_RESP_NCODE"].ToString();
                RespDefault = row["RSP_DEFAULT"].ToString();
                RespSpanishDesc = row["RSP_SDESC"].ToString();
                Changed = "N";
            }
        }

        public CustRespEntity(DataRow AgyTabControl, string strtype)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "U";
                ScrCode = row["RSP_SCR_CODE"].ToString();
                ResoCode = row["RSP_CUST_CODE"].ToString().Trim();
                RespSeq = row["RSP_SEQ"].ToString().Trim();
                RespDesc = row["RSP_DESC"].ToString().Trim();
                DescCode = row["RSP_RESP_CODE"].ToString().Trim();
                AddDate = row["RSP_DATE_ADD"].ToString().Trim();
                AddOpr = row["RSP_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["RSP_DATE_LSTC"].ToString().Trim();
                ChgOpr = row["RSP_LSTC_OPERATOR"].ToString().Trim();
                Points = row["RSP_POINTS"].ToString().Trim();
                RspStatus = row["RSP_STATUS"].ToString().Trim();
                RespNCode = row["RSP_RESP_NCODE"].ToString();
                RespDefault = row["RSP_DEFAULT"].ToString();
                RespSpanishDesc = row["RSP_SDESC"].ToString();
                if (strtype == "FIELDCONTROL")
                    status = row["STATUS"].ToString().Trim();
                Changed = "N";
            }
        }

        public CustRespEntity(CustRespEntity Entity)
        {
            if (Entity != null)
            {
                RecType = Entity.RecType;
                ScrCode = Entity.ScrCode;
                ResoCode = Entity.ResoCode;
                RespSeq = Entity.RespSeq;
                RespDesc = Entity.RespDesc;
                DescCode = Entity.DescCode;
                AddDate = Entity.AddDate;
                AddOpr = Entity.AddOpr;
                ChgDate = Entity.ChgDate;
                ChgOpr = Entity.ChgOpr;
                Changed = Entity.Changed;
                Points = Entity.Points;
                RspStatus = Entity.RspStatus;
                RespNCode = Entity.RespNCode;
                RespDefault = Entity.RespDefault;
                RespSpanishDesc = Entity.RespSpanishDesc;
            }
        }

        public CustRespEntity(DataRow AgyTabControl, string strtype, string strtable)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "U";
                Points = string.Empty;
                RspStatus = string.Empty;
                ResoCode = row["AGYQ_RESP_QCODE"].ToString().Trim();
                RespSeq = row["AGYQ_RESP_SEQ"].ToString().Trim();
                RespDesc = row["AGYQ_RESP_DESC"].ToString().Trim();
                DescCode = row["AGYQ_RESP_CODE"].ToString().Trim();
                AddDate = row["AGYQ_RESP_DATE_ADD"].ToString().Trim();
                AddOpr = row["AGYQ_RESP_ADD_OPERATOR"].ToString().Trim();
                ChgDate = row["AGYQ_RESP_DATE_LSTC"].ToString().Trim();
                ChgOpr = row["AGYQ_RESP_LSTC_OPERATOR"].ToString().Trim();
                Changed = "N";
            }
        }

        #endregion

        #region Properties

        public string RecType { get; set; }
        public string ScrCode { get; set; }
        public string ResoCode { get; set; }
        public string RespSeq { get; set; }
        public string RespDesc { get; set; }
        public string DescCode { get; set; }
        public string AddDate { get; set; }
        public string AddOpr { get; set; }
        public string ChgDate { get; set; }
        public string ChgOpr { get; set; }
        public string Changed { get; set; }
        public string Points { get; set; }
        public string status { get; set; }
        public string RspStatus { get; set; }
        public string RespNCode { get; set; }
        public string RespDefault { get; set; }
        public string RespSpanishDesc { get; set; }

        #endregion
    }



    public class FLDDESCHieEntity
    {
        #region Constructors

        public FLDDESCHieEntity()
        {
            RecType = string.Empty;
            ScrCode = string.Empty;
            ScrHie = string.Empty;
            FldCode = string.Empty;
            Active = string.Empty;
            Enab = string.Empty;
            Req = string.Empty;
            Shared = string.Empty;
            FldDesc = string.Empty;
            SubScr = string.Empty;
            Changed = string.Empty;
        }

        public FLDDESCHieEntity(DataRow AgyTabControl)
        {

            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "U";
                ScrCode = row["FLDH_SCR_CODE"].ToString().Trim();
                ScrHie = row["FLDH_SCR_HIE"].ToString().Trim();
                FldCode = row["FLDH_CODE"].ToString().Trim();
                Active = row["FLDH_ACTIVE"].ToString().Trim();
                Enab = row["FLDH_ENABLED"].ToString().Trim();
                Req = row["FLDH_REQUIRED"].ToString().Trim();
                Shared = row["FLDH_SHARED"].ToString().Trim();
                FldDesc = row["STF_DESC"].ToString().Trim();
                SubScr = row["STF_SUB_SCR"].ToString().Trim();
                Changed = "N";
            }
        }


        #endregion

        #region Properties

        public string RecType { get; set; }
        public string ScrCode { get; set; }
        public string ScrHie { get; set; }
        public string FldCode { get; set; }
        public string Active { get; set; }
        public string Enab { get; set; }
        public string Req { get; set; }
        public string Shared { get; set; }
        public string FldDesc { get; set; }
        public string SubScr { get; set; }
        public string Changed { get; set; }

        #endregion
    }


    public class FLDCNTLHIEAddEntity
    {
        #region Constructors

        public FLDCNTLHIEAddEntity()
        {
            RecType = string.Empty;
            ScrCode = string.Empty;
            FldCode = string.Empty;
            FldDesc = string.Empty;
            SubScr = string.Empty;
            Active = string.Empty;
            Enab = string.Empty;
            Req = string.Empty;
            Shared = string.Empty;
            Changed = string.Empty;
        }

        public FLDCNTLHIEAddEntity(DataRow AgyTabControl)
        {

            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "I";
                ScrCode = null;
                FldCode = row["Code"].ToString().Trim();
                FldDesc = row["FDesc"].ToString().Trim();
                SubScr = row["SubScr"].ToString().Trim();
                Active = "A";
                Enab = "N";
                Req = "N";
                Shared = "N";
                Changed = "N";
            }
        }


        #endregion

        #region Properties

        public string RecType { get; set; }
        public string ScrCode { get; set; }
        public string FldCode { get; set; }
        public string FldDesc { get; set; }
        public string SubScr { get; set; }
        public string Active { get; set; }
        public string Enab { get; set; }
        public string Req { get; set; }
        public string Shared { get; set; }
        public string Changed { get; set; }

        #endregion
    }

    public class CustomQuestionsandAnswers
    {
        #region Constructors

        public CustomQuestionsandAnswers()
        {
            Mode = string.Empty;
            Question = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Active = string.Empty;
            IsApplicant = false;
            ResponseValue = string.Empty;
            ResponseCode = string.Empty;
        }

        public CustomQuestionsandAnswers(string mode, string custCode, string question, string agency, string dept, string program, string active, bool isApplicant, string responseValue, string responseCode)
        {
            Mode = mode;
            CustCode = custCode.Trim();
            Question = question.Trim();
            Agency = agency.Trim();
            Dept = dept.Trim();
            Program = program.Trim();
            Active = active.Trim();
            IsApplicant = isApplicant;
            ResponseValue = responseValue.Trim();
            ResponseCode = responseCode.Trim();
        }

        #endregion

        #region Properties

        public string Mode { get; set; }
        public string CustCode { get; set; }
        public string Question { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Active { get; set; }
        public bool IsApplicant { get; set; }
        public string ResponseValue { get; set; }
        public string ResponseCode { get; set; }

        #endregion
    }

    public class ScafldsEntity
    {
        #region Constructors

        public ScafldsEntity()
        {
            ScrCode = string.Empty;
            ScafldCode = string.Empty;
            ScaDesc = string.Empty;
        }

        public ScafldsEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                ScrCode = row["SCA_SCR_CODE"].ToString().Trim();
                ScafldCode = row["SCA_FLD_CODE"].ToString().Trim();
                ScaDesc = row["SCA_DESC"].ToString().Trim();

            }
        }

        #endregion

        #region Properties

        public string ScrCode { get; set; }
        public string ScafldCode { get; set; }
        public string ScaDesc { get; set; }

        #endregion
    }


    public class ScaFldsHieEntity
    {
        #region Constructors

        public ScaFldsHieEntity()
        {
            RecType = string.Empty;
            ScrCode = string.Empty;
            ScrHie = string.Empty;
            ScahCode = string.Empty;
            Active = string.Empty;
            Sel = string.Empty;
            FldDesc = string.Empty;
            DateAdd = string.Empty;
            DateLstc = string.Empty;
            AddOperator = string.Empty;
            LstcOperator = string.Empty;
            scaDesc = string.Empty;
            Msg = string.Empty;
        }

        public ScaFldsHieEntity(DataRow AgyTabControl)
        {

            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                RecType = "U";
                ScrCode = row["SCAH_SCR_CODE"].ToString().Trim();
                ScrHie = row["SCAH_SCR_HIE"].ToString().Trim();
                ScahCode = row["SCAH_CODE"].ToString().Trim();
                Active = row["SCAH_ACTIVE"].ToString().Trim();
                Sel = row["SCAH_SEL"].ToString().Trim();
                DateAdd = row["SCAH_DATE_ADD"].ToString().Trim();
                DateLstc = row["SCAH_DATE_LSTC"].ToString().Trim();
                AddOperator = row["SCAH_ADD_OPERATOR"].ToString().Trim();
                LstcOperator = row["SCAH_LSTC_OPERATOR"].ToString().Trim();
                scaDesc = row["SCA_DESC"].ToString().Trim();
                Msg = row["SCAH_MSG"].ToString().Trim();

            }
        }


        #endregion

        #region Properties

        public string RecType { get; set; }
        public string ScrCode { get; set; }
        public string ScrHie { get; set; }
        public string ScahCode { get; set; }
        public string Active { get; set; }
        public string Sel { get; set; }
        public string FldDesc { get; set; }
        public string DateAdd { get; set; }
        public string DateLstc { get; set; }
        public string AddOperator { get; set; }
        public string LstcOperator { get; set; }
        public string scaDesc { get; set; }
        public string Msg { get; set; }
        #endregion
    }

    public class PMTFLDCNTLHEntity
    {
        #region Constructors

        public PMTFLDCNTLHEntity()
        {
            PMFLDH_SCR_CODE = string.Empty;
            PMFLDH_SCR_HIE = string.Empty;
            PMFLDH_SP = string.Empty;
            PMFLDH_BRANCH = string.Empty;
            PMFLDH_CURR_GRP = string.Empty;
            PMFLDH_CA_CODE = string.Empty;
            PMFLDH_CODE = string.Empty;
            PMFLDH_ACTIVE = string.Empty;
            PMFLDH_ENABLED = string.Empty;
            PMFLDH_REQUIRED = string.Empty;
            PMFLDH_CATG = string.Empty;
            Mode = string.Empty;
        }

        public PMTFLDCNTLHEntity(DataRow datarow)
        {

            if (datarow != null)
            {
                DataRow row = datarow;
                PMFLDH_SCR_CODE = row["PMFLDH_SCR_CODE"].ToString().Trim();
                PMFLDH_SCR_HIE = row["PMFLDH_SCR_HIE"].ToString().Trim();
                PMFLDH_SP = row["PMFLDH_SP"].ToString().Trim();
                PMFLDH_BRANCH = row["PMFLDH_BRANCH"].ToString().Trim();
                PMFLDH_CURR_GRP = row["PMFLDH_CURR_GRP"].ToString().Trim();
                PMFLDH_CA_CODE = row["PMFLDH_CA_CODE"].ToString().Trim();
                PMFLDH_CODE = row["PMFLDH_CODE"].ToString().Trim();
                PMFLDH_ACTIVE = row["PMFLDH_ACTIVE"].ToString().Trim();
                PMFLDH_ENABLED = row["PMFLDH_ENABLED"].ToString().Trim();
                PMFLDH_REQUIRED = row["PMFLDH_REQUIRED"].ToString().Trim();
                PMFLDH_CATG = row["PMFLDH_CATG"].ToString().Trim(); ;

            }
        }

        public PMTFLDCNTLHEntity(string scrcode, string strHie, string strCode, string stractive, string strenable, string strRequired, string strCATG)
        {
            PMFLDH_SCR_CODE = scrcode;
            PMFLDH_SCR_HIE = strHie;
            PMFLDH_SP = string.Empty;
            PMFLDH_BRANCH = string.Empty;
            PMFLDH_CURR_GRP = string.Empty;
            PMFLDH_CA_CODE = string.Empty;
            PMFLDH_CODE = strCode;
            PMFLDH_ACTIVE = stractive;
            PMFLDH_ENABLED = strenable;
            PMFLDH_REQUIRED = strRequired;
            PMFLDH_CATG = strCATG;
            Mode = string.Empty;
        }

        #endregion

        #region Properties

        public string PMFLDH_SCR_CODE { get; set; }
        public string PMFLDH_SCR_HIE { get; set; }
        public string PMFLDH_SP { get; set; }
        public string PMFLDH_BRANCH { get; set; }
        public string PMFLDH_CURR_GRP { get; set; }
        public string PMFLDH_CA_CODE { get; set; }
        public string PMFLDH_CODE { get; set; }
        public string PMFLDH_ACTIVE { get; set; }
        public string PMFLDH_ENABLED { get; set; }
        public string PMFLDH_REQUIRED { get; set; }
        public string PMFLDH_CATG { get; set; }
        public string Mode { get; set; }
        #endregion
    }

    public class PMTSTDFLDSEntity
    {
        #region Constructors

        public PMTSTDFLDSEntity()
        {
            PSTF_SCR_CODE = string.Empty;
            PSTF_FLD_CATG = string.Empty;
            PSTF_FLD_CODE = string.Empty;
            PSTF_DESC = string.Empty;
            PSTF_RESP_TYPE = string.Empty;
            PSTF_AGY_CODE = string.Empty;
            PSTF_SUB_SCR = string.Empty;
            Mode = string.Empty;
        }

        public PMTSTDFLDSEntity(DataRow datarow)
        {

            if (datarow != null)
            {
                DataRow row = datarow;
                PSTF_SCR_CODE = row["PSTF_SCR_CODE"].ToString().Trim();
                PSTF_FLD_CATG = row["PSTF_FLD_CATG"].ToString().Trim();
                PSTF_FLD_CODE = row["PSTF_FLD_CODE"].ToString().Trim();
                PSTF_DESC = row["PSTF_DESC"].ToString().Trim();
                PSTF_RESP_TYPE = row["PSTF_RESP_TYPE"].ToString().Trim();
                PSTF_AGY_CODE = row["PSTF_AGY_CODE"].ToString().Trim();
                PSTF_SUB_SCR = row["PSTF_SUB_SCR"].ToString().Trim();

            }
        }


        #endregion

        #region Properties

        public string PSTF_SCR_CODE { get; set; }
        public string PSTF_FLD_CATG { get; set; }
        public string PSTF_FLD_CODE { get; set; }
        public string PSTF_DESC { get; set; }
        public string PSTF_RESP_TYPE { get; set; }
        public string PSTF_AGY_CODE { get; set; }
        public string PSTF_SUB_SCR { get; set; }
        public string Mode { get; set; }
        #endregion
    }
}
