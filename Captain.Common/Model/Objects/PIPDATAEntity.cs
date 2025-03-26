using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captain.Common.Model.Objects
{
   public class PIPDocEntity
    {
        #region Constructors

        public PIPDocEntity()
        {
            PIPDOCUPLD_ID = string.Empty;
            PIPDOCUPLD_AGENCY = string.Empty;
            PIPDOCUPLD_AGY = string.Empty;
            PIPDOCUPLD_REG_ID = string.Empty;
          //  PIPDOCUPLD_CONFNO = string.Empty;
            PIPDOCUPLD_PIP_ID = string.Empty;
            PIPDOCUPLD_DOCNAME = string.Empty;
            PIPDOCUPLD_DOCTYPE = string.Empty;
            PIPDOCUPLD_DATE_ADD = string.Empty;
            //PIPDOCUPLD_DATE_LSTC = string.Empty;
            PIPDOCUPLD_DATE_DRAGD = string.Empty;
            PIPDOCUPLD_DRAGD_AS = string.Empty;
            PIPDOCUPLD_DRAGD_BY = string.Empty;
            PIPDOCUPLD_DATE_VERIFIED = string.Empty;
            PIPDOCUPLD_VERIFIED_STAT = string.Empty;
            PIPDOCUPLD_VERIFIED_BY = string.Empty;
            PIPDOCUPLD_REMARKS = string.Empty;
            PIPDOCUPLD_MAIL_ON = string.Empty;
            PIPDOCUPLD_LINK_DOC = string.Empty; ;
            PIPDOCUPLD_FNAME = string.Empty;
            PIPDOCUPLD_LNAME = string.Empty; ;
            PIPDOCUPLD_ATSTN_DATE = string.Empty; ;
            PIPDOCUPLD_SECURITY = string.Empty;
        }

       
        public PIPDocEntity(DataRow Row)
        {
            if (Row != null)
            {
                
                PIPDOCUPLD_ID = Row["PIPDOCUPLD_ID"].ToString().Trim();
                PIPDOCUPLD_AGENCY = Row["PIPDOCUPLD_AGENCY"].ToString().Trim();
                PIPDOCUPLD_AGY = Row["PIPDOCUPLD_AGY"].ToString().Trim();
                PIPDOCUPLD_REG_ID = Row["PIPDOCUPLD_REG_ID"].ToString().Trim();
               // PIPDOCUPLD_CONFNO = Row["PIPDOCUPLD_CONFNO"].ToString().Trim();
                PIPDOCUPLD_PIP_ID = Row["PIPDOCUPLD_PIP_ID"].ToString().Trim();
                PIPDOCUPLD_DOCNAME = Row["PIPDOCUPLD_DOCNAME"].ToString().Trim();
                PIPDOCUPLD_DOCTYPE = Row["PIPDOCUPLD_DOCTYPE"].ToString().Trim();
                PIPDOCUPLD_DATE_ADD = Row["PIPDOCUPLD_DATE_ADD"].ToString().Trim();
               // PIPDOCUPLD_DATE_LSTC = Row["PIPDOCUPLD_DATE_LSTC"].ToString().Trim();
                PIPDOCUPLD_DATE_DRAGD = Row["PIPDOCUPLD_DATE_DRAGD"].ToString().Trim();
                PIPDOCUPLD_DRAGD_AS = Row["PIPDOCUPLD_DRAGD_AS"].ToString().Trim();
                PIPDOCUPLD_DRAGD_BY = Row["PIPDOCUPLD_DRAGD_BY"].ToString().Trim();
                PIPDOCUPLD_DATE_VERIFIED = Row["PIPDOCUPLD_DATE_VERIFIED"].ToString().Trim();
                PIPDOCUPLD_VERIFIED_STAT = Row["PIPDOCUPLD_VERIFIED_STAT"].ToString().Trim();
                PIPDOCUPLD_VERIFIED_BY = Row["PIPDOCUPLD_VERIFIED_BY"].ToString().Trim();
                PIPDOCUPLD_REMARKS = Row["PIPDOCUPLD_REMARKS"].ToString().Trim();
                PIPDOCUPLD_MAIL_ON = Row["PIPDOCUPLD_MAIL_ON"].ToString().Trim();
                PIPDOCUPLD_LINK_DOC = Row["PIPDOCUPLD_LINK_DOC"].ToString().Trim();
                PIPDOCUPLD_FNAME = Row["PIPDOCUPLD_FNAME"].ToString().Trim();
                PIPDOCUPLD_LNAME = Row["PIPDOCUPLD_LNAME"].ToString().Trim();
              //  PIPDOCUPLD_ATSTN_DATE = Row["PIPDOCUPLD_ATSTN_DATE"].ToString().Trim();
                PIPDOCUPLD_SECURITY = Row["PIPDOCUPLD_SECURITY"].ToString().Trim();
            }
        }






        #endregion

        #region Properties

        public string PIPDOCUPLD_ID { get; set; }
        public string PIPDOCUPLD_AGENCY { get; set; }
        public string PIPDOCUPLD_AGY { get; set; }
        public string PIPDOCUPLD_REG_ID { get; set; }
       // public string PIPDOCUPLD_CONFNO { get; set; }
        public string PIPDOCUPLD_PIP_ID { get; set; }
        public string PIPDOCUPLD_DOCNAME { get; set; }
        public string PIPDOCUPLD_DOCTYPE { get; set; }
        public string PIPDOCUPLD_SECURITY { get; set; }
        public string PIPDOCUPLD_DATE_ADD { get; set; }
       // public string PIPDOCUPLD_DATE_LSTC { get; set; }
        public string PIPDOCUPLD_DATE_DRAGD { get; set; }
        public string PIPDOCUPLD_DRAGD_AS { get; set; }
        public string PIPDOCUPLD_DRAGD_BY { get; set; }
        public string PIPDOCUPLD_DATE_VERIFIED { get; set; }
        public string PIPDOCUPLD_VERIFIED_STAT { get; set; }
        public string PIPDOCUPLD_VERIFIED_BY { get; set; }
        public string PIPDOCUPLD_REMARKS { get; set; }
        public string PIPDOCUPLD_MAIL_ON { get; set; }
        public string PIPDOCUPLD_LINK_DOC { get; set; }
        public string PIPDOCUPLD_FNAME { get; set; }
        public string PIPDOCUPLD_LNAME { get; set; }
        public string PIPDOCUPLD_ATSTN_DATE { get; set; }
        public string Mode { get; set; }

        #endregion
    }

    public class PIPCAPLNK
    {
        #region Constructors

        public PIPCAPLNK()
        {
            PIPCAPLNK_AGENCY = string.Empty;
            PIPCAPLNK_DEPT = string.Empty;
            PIPCAPLNK_PROGRAM = string.Empty;
            PIPCAPLNK_YEAR = string.Empty;
            PIPCAPLNK_APP = string.Empty;
            PIPCAPLNK_FAMILY_SEQ = string.Empty;
            PIPCAPLNK_PIP_AGENCY = string.Empty;
            PIPCAPLNK_PIP_AGY = string.Empty;
            PIPCAPLNK_REGID = string.Empty;
            PIPCAPLNK_PIP_ID = string.Empty;
            PIPCAPLNK_TYPE = string.Empty;
        }


        public PIPCAPLNK(DataRow Row)
        {
            if (Row != null)
            {
                PIPCAPLNK_AGENCY = Row["PIPCAPLNK_AGENCY"].ToString().Trim();
                PIPCAPLNK_DEPT = Row["PIPCAPLNK_DEPT"].ToString().Trim();
                PIPCAPLNK_PROGRAM = Row["PIPCAPLNK_PROGRAM"].ToString().Trim();
                PIPCAPLNK_YEAR = Row["PIPCAPLNK_YEAR"].ToString().Trim();
                PIPCAPLNK_APP = Row["PIPCAPLNK_APP"].ToString().Trim();
                PIPCAPLNK_FAMILY_SEQ = Row["PIPCAPLNK_FAMILY_SEQ"].ToString().Trim();
                PIPCAPLNK_PIP_AGENCY = Row["PIPCAPLNK_PIP_AGENCY"].ToString().Trim();
                PIPCAPLNK_PIP_AGY = Row["PIPCAPLNK_PIP_AGY"].ToString().Trim();
                PIPCAPLNK_REGID = Row["PIPCAPLNK_REGID"].ToString().Trim();
                PIPCAPLNK_PIP_ID = Row["PIPCAPLNK_PIP_ID"].ToString().Trim();
                PIPCAPLNK_TYPE = Row["PIPCAPLNK_TYPE"].ToString().Trim();
            }
        }






        #endregion

        #region Properties

        public string PIPCAPLNK_AGENCY { get; set; }
        public string PIPCAPLNK_DEPT { get; set; }
        public string PIPCAPLNK_PROGRAM { get; set; }
        public string PIPCAPLNK_YEAR { get; set; }
        public string PIPCAPLNK_APP { get; set; }
        public string PIPCAPLNK_FAMILY_SEQ { get; set; }
        public string PIPCAPLNK_PIP_AGENCY { get; set; }
        public string PIPCAPLNK_PIP_AGY { get; set; }
        public string PIPCAPLNK_REGID { get; set; }
        public string PIPCAPLNK_PIP_ID { get; set; }
        public string PIPCAPLNK_TYPE { get; set; }
        public string Mode { get; set; }

        #endregion
    }

    public class PIPDocVerEntity
    {
        #region Constructors

        public PIPDocVerEntity()
        {
            PIPDOCVER_ID = string.Empty;
            PIPDOCVER_DOC_ID = string.Empty;
            PIPDOCVER_DATE = string.Empty;
            PIPDOCVER_STATUS = string.Empty;
            PIPDOCVER_BY = string.Empty;
            PIPDOCVER_Remarks = string.Empty;
            PIPDOCVER_MAIL_ON = string.Empty;
            PIPDOCVER_DATE_LSTC = string.Empty;
            PIPDOCVER_REG_ID = string.Empty;
            PIPDOCVER_PIP_ID = string.Empty;
            PIPDOCVER_MSG_READ = string.Empty;
        }


        public PIPDocVerEntity(DataRow Row)
        {
            if (Row != null)
            {
                PIPDOCVER_ID = Row["PIPDOCVER_ID"].ToString();
                PIPDOCVER_DOC_ID = Row["PIPDOCVER_DOC_ID"].ToString().Trim();
                PIPDOCVER_DATE = Row["PIPDOCVER_DATE"].ToString().Trim();
                PIPDOCVER_STATUS = Row["PIPDOCVER_STATUS"].ToString().Trim();
                PIPDOCVER_BY = Row["PIPDOCVER_BY"].ToString().Trim();
                PIPDOCVER_Remarks = Row["PIPDOCVER_Remarks"].ToString().Trim();
                PIPDOCVER_MAIL_ON = Row["PIPDOCVER_MAIL_ON"].ToString().Trim();
                PIPDOCVER_DATE_LSTC = Row["PIPDOCVER_DATE_LSTC"].ToString().Trim();
                PIPDOCVER_REG_ID = Row["PIPDOCVER_REG_ID"].ToString().Trim();
                PIPDOCVER_PIP_ID = Row["PIPDOCVER_PIP_ID"].ToString().Trim();
                PIPDOCVER_MSG_READ = Row["PIPDOCVER_MSG_READ"].ToString().Trim();
            }
        }






        #endregion

        #region Properties

        public string PIPDOCVER_ID { get; set; }
        public string PIPDOCVER_DOC_ID { get; set; }
        public string PIPDOCVER_DATE { get; set; }
        public string PIPDOCVER_STATUS { get; set; }
        public string PIPDOCVER_BY { get; set; }
        public string PIPDOCVER_Remarks { get; set; }
        public string PIPDOCVER_MAIL_ON { get; set; }
        public string PIPDOCVER_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        public string PIPDOCVER_REG_ID { get; set; }
        public string PIPDOCVER_PIP_ID { get; set; }
        public string PIPDOCVER_MSG_READ { get; set; }

        #endregion
    }

    public class PIPEmailHist
    {
        #region Constructors

        public PIPEmailHist()
        {
            PIPMAILH_ID = string.Empty;
            PIPMAILH_REG_ID = string.Empty;
            PIPMAILH_UPD_ID = string.Empty;
            PIPMAILH_VER_ID = string.Empty;
            PIPMAILH_MISC_MSG = string.Empty;
            PIPMAILH_SENT_BY = string.Empty;
            PIPMAILH_DATE_SENT = string.Empty;
            PIPMAILH_READ = string.Empty;
        }


        public PIPEmailHist(DataRow Row)
        {
            if (Row != null)
            {
                PIPMAILH_ID = Row["PIPMAILH_ID"].ToString().Trim();
                PIPMAILH_REG_ID = Row["PIPMAILH_REG_ID"].ToString().Trim();
                PIPMAILH_UPD_ID = Row["PIPMAILH_UPD_ID"].ToString().Trim();
                PIPMAILH_VER_ID = Row["PIPMAILH_VER_ID"].ToString().Trim();
                PIPMAILH_MISC_MSG = Row["PIPMAILH_MISC_MSG"].ToString().Trim();
                PIPMAILH_SENT_BY = Row["PIPMAILH_SENT_BY"].ToString().Trim();
                PIPMAILH_DATE_SENT = Row["PIPMAILH_DATE_SENT"].ToString().Trim();
                PIPMAILH_READ = Row["PIPMAILH_READ"].ToString().Trim();
            }
        }






        #endregion

        #region Properties

        public string PIPMAILH_ID { get; set; }
        public string PIPMAILH_REG_ID { get; set; }
        public string PIPMAILH_UPD_ID { get; set; }
        public string PIPMAILH_VER_ID { get; set; }
        public string PIPMAILH_MISC_MSG { get; set; }
        public string PIPMAILH_SENT_BY { get; set; }
        public string PIPMAILH_DATE_SENT { get; set; }
        public string PIPMAILH_READ { get; set; }

        #endregion
    }

}
