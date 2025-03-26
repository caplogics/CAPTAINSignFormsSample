using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captain.Common.Model.Objects
{

    public class SaldefEntity
    {
        #region Constructors

        public SaldefEntity()
        {
            SALD_ID = string.Empty;
            SALD_TYPE = string.Empty;
            SALD_HIE = string.Empty;
            SALD_NAME = string.Empty;
            SALD_SPS = string.Empty;
            SALD_SERVICES = string.Empty;
            SALD_ACTIVE = string.Empty;
            SALD_ADD_OPERATOR = string.Empty;
            SALD_DATE_ADD = string.Empty;
            SALD_LSTC_OPERATOR = string.Empty;
            SALD_DATE_LSTC = string.Empty;
            SALD_BOILERPLATE = string.Empty;
            SALD_SIGN_REQURED = string.Empty;
            SALD_5QUEST = string.Empty;
            Mode = string.Empty;
        }

        public SaldefEntity(bool Intialize)
        {
            SALD_ID = 
            SALD_TYPE = 
            SALD_HIE = 
            SALD_NAME = 
            SALD_SPS = 
            SALD_SERVICES = 
            SALD_ACTIVE = 
            SALD_ADD_OPERATOR = 
            SALD_DATE_ADD = 
            SALD_LSTC_OPERATOR = 
            SALD_DATE_LSTC =
            SALD_BOILERPLATE =
            SALD_SIGN_REQURED =
            SALD_5QUEST=
            Mode = null;
        }

        public SaldefEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                SALD_ID = row["SALD_ID"].ToString();
                SALD_TYPE = row["SALD_TYPE"].ToString();
                SALD_HIE = row["SALD_HIE"].ToString();
                SALD_NAME = row["SALD_NAME"].ToString();
                SALD_SPS = row["SALD_SPS"].ToString();
                SALD_SERVICES = row["SALD_SERVICES"].ToString();
                SALD_ACTIVE = row["SALD_ACTIVE"].ToString();
                SALD_ADD_OPERATOR = row["SALD_ADD_OPERATOR"].ToString();
                SALD_DATE_ADD = row["SALD_DATE_ADD"].ToString();
                SALD_LSTC_OPERATOR = row["SALD_LSTC_OPERATOR"].ToString();
                SALD_DATE_LSTC = row["SALD_DATE_LSTC"].ToString();
                SALD_BOILERPLATE = row["SALD_BOILERPLATE"].ToString();
                SALD_SIGN_REQURED = row["SALD_SIGN_REQURED"].ToString();

                SALD_5QUEST = row["SALD_5QUEST"].ToString();

                Mode = string.Empty;

            }

        }

        #endregion

        #region Properties

        public string SALD_ID { get; set; }
        public string SALD_TYPE { get; set; }
        public string SALD_HIE { get; set; }
        public string SALD_NAME { get; set; }
        public string SALD_SPS { get; set; }
        public string SALD_SERVICES { get; set; }
        public string SALD_ACTIVE { get; set; }
        public string SALD_ADD_OPERATOR { get; set; }
        public string SALD_DATE_ADD { get; set; }
        public string SALD_LSTC_OPERATOR { get; set; }
        public string SALD_DATE_LSTC { get; set; }
        public string SALD_BOILERPLATE { get; set; }
        public string SALD_SIGN_REQURED { get; set; }

        public string SALD_5QUEST { get; set; }

        public string Mode { get; set; }

        #endregion

    }

    public class SalquesEntity
    {
        #region Constructors

        public SalquesEntity()
        {
            SALQ_ID = string.Empty;
            SALQ_SALD_ID = string.Empty;
            SALQ_GRP_CODE = string.Empty;
            SALQ_GRP_SEQ = string.Empty;
            SALQ_SEQ = string.Empty;
          //  SALQ_CODE = string.Empty;
            SALQ_TYPE = string.Empty;
            SALQ_DESC = string.Empty;
            SALQ_ADD_OPERATOR = string.Empty;
            SALQ_DATE_ADD = string.Empty;
            SALQ_LSTC_OPERATOR = string.Empty;
            SALQ_DATE_LSTC = string.Empty;
            SALQ_REQ = string.Empty;
            Mode = string.Empty;
        }

        public SalquesEntity(bool Intialize)
        {
            SALQ_ID = 
            SALQ_SALD_ID = 
            SALQ_GRP_CODE = 
            SALQ_GRP_SEQ = 
            SALQ_SEQ = 
          //  SALQ_CODE = 
            SALQ_TYPE = 
            SALQ_DESC =
            SALQ_REQ=
            SALQ_ADD_OPERATOR = 
            SALQ_DATE_ADD = 
            SALQ_LSTC_OPERATOR = 
            SALQ_DATE_LSTC = 
            Mode = null;
        }

        public SalquesEntity(DataRow row, string strType)
        {
            if (row != null)
            {

                SALQ_ID = row["SALQ_ID"].ToString();
                SALQ_SALD_ID = row["SALQ_SALD_ID"].ToString();
                SALQ_GRP_CODE = row["SALQ_GRP_CODE"].ToString();
                SALQ_GRP_SEQ = row["SALQ_GRP_SEQ"].ToString();
                SALQ_SEQ = row["SALQ_SEQ"].ToString();
              //  SALQ_CODE = row["SALQ_CODE"].ToString();
                SALQ_TYPE = row["SALQ_TYPE"].ToString();
                SALQ_DESC = row["SALQ_DESC"].ToString();
                SALQ_ADD_OPERATOR = row["SALQ_ADD_OPERATOR"].ToString();
                SALQ_DATE_ADD = row["SALQ_DATE_ADD"].ToString();
                SALQ_LSTC_OPERATOR = row["SALQ_LSTC_OPERATOR"].ToString();
                SALQ_DATE_LSTC = row["SALQ_DATE_LSTC"].ToString();

                SALQ_REQ = row["SALQ_REQ"].ToString();
                Mode = string.Empty;

            }

        }

        #endregion

        #region Properties

        public string SALQ_ID { get; set; }
        public string SALQ_SALD_ID { get; set; }
        public string SALQ_GRP_CODE { get; set; }
        public string SALQ_GRP_SEQ { get; set; }
        public string SALQ_SEQ { get; set; }
        //public string SALQ_CODE { get; set; }
        public string SALQ_TYPE { get; set; }
        public string SALQ_DESC { get; set; }
        public string SALQ_ADD_OPERATOR { get; set; }
        public string SALQ_DATE_ADD { get; set; }
        public string SALQ_LSTC_OPERATOR { get; set; }
        public string SALQ_DATE_LSTC { get; set; }
        
        public string SALQ_REQ { get; set; }

        public string Mode { get; set; }

        #endregion

    }

    public class SalqrespEntity
    {
        #region Constructors

        public SalqrespEntity()
        {
            SALQR_Q_ID = string.Empty;
            SALQR_CODE = string.Empty;
            SALQR_SEQ = string.Empty;
            SALQR_DESC = string.Empty;
            SALQR_ADD_OPERATOR = string.Empty;
            SALQR_DATE_ADD = string.Empty;
            SALQR_LSTC_OPERATOR = string.Empty;
            SALQR_DATE_LSTC = string.Empty;
            Mode = string.Empty;
        }
        public SalqrespEntity(SalqrespEntity entity)
        {
            SALQR_Q_ID = entity.SALQR_Q_ID;
            SALQR_CODE = entity.SALQR_CODE;
            SALQR_SEQ = entity.SALQR_SEQ;
            SALQR_DESC = entity.SALQR_DESC;
            SALQR_ADD_OPERATOR = entity.SALQR_ADD_OPERATOR;
            SALQR_DATE_ADD = entity.SALQR_DATE_ADD;
            SALQR_LSTC_OPERATOR = entity.SALQR_LSTC_OPERATOR;
            SALQR_DATE_LSTC = entity.SALQR_DATE_LSTC;
            Mode = string.Empty;
        }

        public SalqrespEntity(bool Intialize)
        {
            SALQR_Q_ID = 
            SALQR_CODE = 
            SALQR_SEQ = 
            SALQR_DESC = 
            SALQR_ADD_OPERATOR = 
            SALQR_DATE_ADD = 
            SALQR_LSTC_OPERATOR = 
            SALQR_DATE_LSTC = 
            Mode = null;
        }

        public SalqrespEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                SALQR_Q_ID = row["SALQR_Q_ID"].ToString();
                SALQR_CODE = row["SALQR_CODE"].ToString();
                SALQR_SEQ = row["SALQR_SEQ"].ToString();
                SALQR_DESC = row["SALQR_DESC"].ToString();
                SALQR_ADD_OPERATOR = row["SALQR_ADD_OPERATOR"].ToString();
                SALQR_DATE_ADD = row["SALQR_DATE_ADD"].ToString();
                SALQR_LSTC_OPERATOR = row["SALQR_LSTC_OPERATOR"].ToString();
                SALQR_DATE_LSTC = row["SALQR_DATE_LSTC"].ToString();
                Mode = string.Empty;

            }

        }

        #endregion

        #region Properties

        public string SALQR_Q_ID { get; set; }
        public string SALQR_CODE { get; set; }
        public string SALQR_SEQ { get; set; }
        public string SALQR_DESC { get; set; }
        public string SALQR_ADD_OPERATOR { get; set; }
        public string SALQR_DATE_ADD { get; set; }
        public string SALQR_LSTC_OPERATOR { get; set; }
        public string SALQR_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        public string Changed { get; set; }
        public string RecType { get; set; }
        #endregion

    }

    public class SALACTEntity
    {
        #region Constructors

        public SALACTEntity()
        {
            SALACT_ID =
            SALACT_SALID =
            SALACT_Q_ID =
            SALACT_TYPE =
            SALACT_STATUS =
            SALACT_LOCATION =
            SALACT_RECIPIENT =
            SALACT_ATTN =
            SALACT_TIME_IN = 
            SALACT_TIME_OUT =
            SALACT_TIME_SPENT = 
            SALACT_Q_TYPE =
            SALACT_SEQ =
            SALACT_NUM_RESP =
            SALACT_DATE_RESP =
            SALACT_MULTI_RESP =
            SALACT_ADD_OPERATOR =
            SALACT_DATE_ADD =
            SALACT_LSTC_OPERATOR =
            SALACT_DATE_LSTC =
            Mode = string.Empty;
        }
        public SALACTEntity(SALACTEntity entity)
        {
            SALACT_ID = entity.SALACT_ID;
            SALACT_SALID = entity.SALACT_SALID;
            SALACT_Q_ID = entity.SALACT_Q_ID;
            SALACT_TYPE = entity.SALACT_TYPE;
            SALACT_STATUS = entity.SALACT_STATUS;
            SALACT_LOCATION = entity.SALACT_LOCATION;
            SALACT_RECIPIENT = entity.SALACT_RECIPIENT;
            SALACT_ATTN = entity.SALACT_ATTN;
            SALACT_TIME_IN = entity.SALACT_TIME_IN;
            SALACT_TIME_OUT = entity.SALACT_TIME_OUT;
            SALACT_TIME_SPENT = entity.SALACT_TIME_SPENT;
            SALACT_Q_TYPE = entity.SALACT_Q_TYPE;
            SALACT_SEQ = entity.SALACT_SEQ;
            SALACT_NUM_RESP = entity.SALACT_NUM_RESP;
            SALACT_DATE_RESP = entity.SALACT_DATE_RESP;
            SALACT_MULTI_RESP = entity.SALACT_MULTI_RESP;
            SALACT_ADD_OPERATOR = entity.SALACT_ADD_OPERATOR;
            SALACT_DATE_ADD = entity.SALACT_DATE_ADD;
            SALACT_LSTC_OPERATOR = entity.SALACT_LSTC_OPERATOR;
            SALACT_DATE_LSTC = entity.SALACT_DATE_LSTC;
            Mode = string.Empty;
        }

        public SALACTEntity(bool Intialize)
        {
            SALACT_ID =
            SALACT_SALID =
            SALACT_Q_ID =
            SALACT_TYPE =
            SALACT_STATUS =
            SALACT_LOCATION =
            SALACT_RECIPIENT =
            SALACT_ATTN =
            SALACT_TIME_IN =
            SALACT_TIME_OUT =
            SALACT_TIME_SPENT=
            SALACT_Q_TYPE =
            SALACT_SEQ =
            SALACT_NUM_RESP =
            SALACT_DATE_RESP =
            SALACT_MULTI_RESP =
            SALACT_ADD_OPERATOR =
            SALACT_DATE_ADD =
            SALACT_LSTC_OPERATOR =
            SALACT_DATE_LSTC =
            Mode = null;
        }

        public SALACTEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                SALACT_ID = row["SALACT_ID"].ToString();
                SALACT_SALID = row["SALACT_SALID"].ToString();
                SALACT_Q_ID = row["SALACT_Q_ID"].ToString();
                SALACT_TYPE = row["SALACT_TYPE"].ToString();
                SALACT_STATUS = row["SALACT_STATUS"].ToString();
                SALACT_LOCATION = row["SALACT_LOCATION"].ToString();
                SALACT_RECIPIENT = row["SALACT_RECIPIENT"].ToString();
                SALACT_ATTN = row["SALACT_ATTN"].ToString();

                SALACT_TIME_IN = row["SALACT_TIME_IN"].ToString();
                SALACT_TIME_OUT = row["SALACT_TIME_OUT"].ToString();
                SALACT_TIME_SPENT = row["SALACT_TIME_SPENT"].ToString();

                SALACT_Q_TYPE = row["SALACT_Q_TYPE"].ToString();
                SALACT_SEQ = row["SALACT_SEQ"].ToString();
                SALACT_NUM_RESP = row["SALACT_NUM_RESP"].ToString();
                SALACT_DATE_RESP = row["SALACT_DATE_RESP"].ToString();
                SALACT_MULTI_RESP = row["SALACT_MULTI_RESP"].ToString();
                SALACT_ADD_OPERATOR = row["SALACT_ADD_OPERATOR"].ToString();
                SALACT_DATE_ADD = row["SALACT_DATE_ADD"].ToString();
                SALACT_LSTC_OPERATOR = row["SALACT_LSTC_OPERATOR"].ToString();
                SALACT_DATE_LSTC = row["SALACT_DATE_LSTC"].ToString();
                Mode = string.Empty;

            }

        }

        #endregion

        #region Properties

        public string SALACT_ID { get; set; }
        public string SALACT_SALID { get; set; }
        public string SALACT_Q_ID { get; set; }
        public string SALACT_TYPE { get; set; }
        public string SALACT_STATUS { get; set; }
        public string SALACT_LOCATION { get; set; }
        public string SALACT_RECIPIENT { get; set; }
        public string SALACT_ATTN { get; set; }

        public string SALACT_TIME_IN { get; set; }
        public string SALACT_TIME_OUT { get; set; }
        public string SALACT_TIME_SPENT { get; set; }

        public string SALACT_Q_TYPE { get; set; }
        public string SALACT_SEQ { get; set; }
        public string SALACT_NUM_RESP { get; set; }
        public string SALACT_DATE_RESP { get; set; }
        public string SALACT_MULTI_RESP { get; set; }
        
        public string SALACT_ADD_OPERATOR { get; set; }
        public string SALACT_DATE_ADD { get; set; }
        public string SALACT_LSTC_OPERATOR { get; set; }
        public string SALACT_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        public string Changed { get; set; }
        public string RecType { get; set; }
        #endregion

    }

    public class SALQLNKEntity
    {
        #region Constructors

        public SALQLNKEntity()
        {           
            SALQL_Q_ID = string.Empty;
            SALQL_GROUP = string.Empty;
            SALQL_LINKQ = string.Empty;
            SALQL_REQ = string.Empty;           
            SALQL_ENABLE = string.Empty;
            SALQL_DISABLE = string.Empty;
            SALQL_SEQ = string.Empty;
            SALQL_LSTC_OPERATOR = string.Empty;
            SALQL_DATE_LSTC = string.Empty;
            Type = string.Empty;
            Mode = string.Empty;

        }

        public SALQLNKEntity(bool Intialize)
        {
            if (Intialize)
            {               
                SALQL_Q_ID = null;
                SALQL_GROUP = null;
                SALQL_LINKQ = null;
                SALQL_REQ = null;
                SALQL_ENABLE = null;
                SALQL_DISABLE = null;
                SALQL_SEQ = null;
                SALQL_LSTC_OPERATOR = null;
                SALQL_DATE_LSTC = null;
                Type = null;
                Mode = null;
            }
        }

        public SALQLNKEntity(DataRow Preassess, string strType)
        {
            if (Preassess != null)
            {
                DataRow row = Preassess;

                if (strType == "MASTER")
                {
                    //PREDIMENTION_ID = row["PREDIMENTION_ID"].ToString();
                    //PREDIMENTION_DESC = row["PREDIMENTION_DESC"].ToString();
                    //PREDIMENTION_SORT = row["PREDIMENTION_SORT"].ToString();
                    //Type = string.Empty.ToString();
                }
                else
                {
                    SALQL_Q_ID = row["SALQL_Q_ID"].ToString();
                    SALQL_GROUP = row["SALQL_GROUP"].ToString();
                    SALQL_LINKQ = row["SALQL_LINKQ"].ToString();
                    SALQL_REQ = row["SALQL_REQ"].ToString();                  
                    SALQL_ENABLE = row["SALQL_ENABLE"].ToString();
                    SALQL_DISABLE = row["SALQL_DISABLE"].ToString();
                    SALQL_LSTC_OPERATOR = row["SALQL_LSTC_OPERATOR"].ToString();
                    SALQL_DATE_LSTC = row["SALQL_DATE_LSTC"].ToString();
                    Type = string.Empty.ToString();

                }

            }

        }

        #endregion

        #region Properties

        public string SALQL_Q_ID { get; set; }
        public string SALQL_GROUP { get; set; }
        public string SALQL_LINKQ { get; set; }
        public string SALQL_REQ { get; set; }     
        public string SALQL_ENABLE { get; set; }
        public string SALQL_DISABLE { get; set; }
        public string SALQL_SEQ { get; set; }
        public string SALQL_LSTC_OPERATOR { get; set; }
        public string SALQL_DATE_LSTC { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }

        #endregion

    }

    public class CALCONTEntity
    {
        #region Constructors

        public CALCONTEntity()
        {
            CALCONT_ID =
            CALCONT_SALID =
            CALCONT_Q_ID =
            CALCONT_Q_TYPE =
            CALCONT_SEQ =
            CALCONT_NUM_RESP =
            CALCONT_DATE_RESP =
            CALCONT_MULTI_RESP =
            CALCONT_ADD_OPERATOR =
            CALCONT_DATE_ADD =
            CALCONT_LSTC_OPERATOR =
            CALCONT_DATE_LSTC =
            Mode = string.Empty;
        }
        public CALCONTEntity(CALCONTEntity entity)
        {
            CALCONT_ID = entity.CALCONT_ID;
            CALCONT_SALID = entity.CALCONT_SALID;
            CALCONT_Q_ID = entity.CALCONT_Q_ID;
            CALCONT_Q_TYPE = entity.CALCONT_Q_TYPE;
            CALCONT_SEQ = entity.CALCONT_SEQ;
            CALCONT_NUM_RESP = entity.CALCONT_NUM_RESP;
            CALCONT_DATE_RESP = entity.CALCONT_DATE_RESP;
            CALCONT_MULTI_RESP = entity.CALCONT_MULTI_RESP;
            CALCONT_ADD_OPERATOR = entity.CALCONT_ADD_OPERATOR;
            CALCONT_DATE_ADD = entity.CALCONT_DATE_ADD;
            CALCONT_LSTC_OPERATOR = entity.CALCONT_LSTC_OPERATOR;
            CALCONT_DATE_LSTC = entity.CALCONT_DATE_LSTC;
            Mode = string.Empty;
        }

        public CALCONTEntity(bool Intialize)
        {
            CALCONT_ID =
            CALCONT_SALID =
            CALCONT_Q_ID =
            CALCONT_Q_TYPE =
            CALCONT_SEQ =
            CALCONT_NUM_RESP =
            CALCONT_DATE_RESP =
            CALCONT_MULTI_RESP =
            CALCONT_ADD_OPERATOR =
            CALCONT_DATE_ADD =
            CALCONT_LSTC_OPERATOR =
            CALCONT_DATE_LSTC =
            Mode = null;
        }

        public CALCONTEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                CALCONT_ID = row["CALCONT_ID"].ToString();
                CALCONT_SALID = row["CALCONT_SALID"].ToString();
                CALCONT_Q_ID = row["CALCONT_Q_ID"].ToString();
                
                CALCONT_Q_TYPE = row["CALCONT_Q_TYPE"].ToString();
                CALCONT_SEQ = row["CALCONT_SEQ"].ToString();
                CALCONT_NUM_RESP = row["CALCONT_NUM_RESP"].ToString();
                CALCONT_DATE_RESP = row["CALCONT_DATE_RESP"].ToString();
                CALCONT_MULTI_RESP = row["CALCONT_MULTI_RESP"].ToString();
                CALCONT_ADD_OPERATOR = row["CALCONT_ADD_OPERATOR"].ToString();
                CALCONT_DATE_ADD = row["CALCONT_DATE_ADD"].ToString();
                CALCONT_LSTC_OPERATOR = row["CALCONT_LSTC_OPERATOR"].ToString();
                CALCONT_DATE_LSTC = row["CALCONT_DATE_LSTC"].ToString();
                Mode = string.Empty;

            }

        }

        #endregion

        #region Properties

        public string CALCONT_ID { get; set; }
        public string CALCONT_SALID { get; set; }
        public string CALCONT_Q_ID { get; set; }
        
        public string CALCONT_Q_TYPE { get; set; }
        public string CALCONT_SEQ { get; set; }
        public string CALCONT_NUM_RESP { get; set; }
        public string CALCONT_DATE_RESP { get; set; }
        public string CALCONT_MULTI_RESP { get; set; }

        public string CALCONT_ADD_OPERATOR { get; set; }
        public string CALCONT_DATE_ADD { get; set; }
        public string CALCONT_LSTC_OPERATOR { get; set; }
        public string CALCONT_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        public string Changed { get; set; }
        public string RecType { get; set; }
        #endregion

    }


}
