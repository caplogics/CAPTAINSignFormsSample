using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CaseHistEntity
    {
        #region Constructors

        public CaseHistEntity()
        {
            HistTblName = string.Empty;
            HistTblKey = string.Empty;
            HistScreen = string.Empty;          
            HistSeqNo = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;          
            HistChanges = string.Empty;
            HistSubScr = string.Empty;
            Mode = string.Empty;
            Type = string.Empty;

        }

        public CaseHistEntity(DataRow row)
        {
            if (row != null)
            {
                HistTblName = row["HIST_TBLNAME"].ToString().Trim();
                HistTblKey = row["HIST_TBLKEY"].ToString().Trim();
                HistScreen = row["HIST_SCREEN"].ToString().Trim();
                HistSeqNo = row["HIST_SEQ"].ToString().Trim();
                HistSubScr = row["HIST_SUBSCR"].ToString().Trim();
                HistChanges = row["HIST_CHANGES"].ToString().Trim();
                DateLstc = row["HIST_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["HIST_LSTC_OPERATOR"].ToString().Trim();              
              
                Mode = string.Empty;
                Type = string.Empty;


            }

        }

        #endregion Constructors

        #region Properties

        public string HistTblName { get; set; }
        public string HistTblKey { get; set; }
        public string HistScreen { get; set; }
        public string HistSubScr { get; set; }
        public string HistSeqNo { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }       
        public string HistChanges { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }

        #endregion Properties
    }
}
