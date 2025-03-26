using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CEAPHISTEntity
    {
        #region Constructors

        public CEAPHISTEntity()
        {
            CeapHistTblName = string.Empty;
            CeapHistTblKey = string.Empty;
            CeapHistSeqNo = string.Empty;
            CeapHistScreen = string.Empty;
            CeapHistSubScr = string.Empty;
            CeapHistChanges = string.Empty;
            CeapHistDateLstc = string.Empty;
            CeapHistLstcOperator = string.Empty;
        }

        public CEAPHISTEntity(DataRow row)
        {
            if (row != null)
            {
                CeapHistTblName = row["CEAP_HIST_TBLNAME"].ToString().Trim();
                CeapHistTblKey = row["CEAP_HIST_TBLKEY"].ToString().Trim();
                CeapHistSeqNo = row["CEAP_HIST_SEQ"].ToString().Trim();
                CeapHistScreen = row["CEAP_HIST_SCREEN"].ToString().Trim();
                CeapHistSubScr = row["CEAP_HIST_SUBSCR"].ToString().Trim();
                CeapHistChanges = row["CEAP_HIST_CHANGES"].ToString().Trim();
                CeapHistDateLstc = row["CEAP_HIST_DATE_LSTC"].ToString().Trim();
                CeapHistLstcOperator = row["CEAP_HIST_LSTC_OPERATOR"].ToString().Trim();              
            }

        }

        #endregion

        #region Properties

        public string CeapHistTblName
        { get; set; }
        public string CeapHistTblKey
        { get; set; }
        public string CeapHistSeqNo
        {
            get; set;
        }
        public string CeapHistScreen
        { get; set; }
        public string CeapHistSubScr
        { get; set;
        }
        public string CeapHistChanges
        {
            get; set;
        }
        public string CeapHistDateLstc
        { get; set; }
        public string CeapHistLstcOperator
        { get; set; }       

        #endregion
    }
}
