using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    
    public class CaseSiteEntity
    {
        #region Constructors

        public CaseSiteEntity()
        {

            Row_Type = string.Empty;
            SiteAGENCY = string.Empty;
            SiteDEPT = string.Empty;
            SitePROG = string.Empty;
            SiteYEAR = string.Empty;
            SiteNUMBER = string.Empty;
            SiteROOM = string.Empty;
            SiteAM_PM = string.Empty;
            SiteNAME = string.Empty;
            SiteSTREET = string.Empty;
            SiteCITY = string.Empty;
            SiteSTATE = string.Empty;
            SiteZIP = string.Empty;
            SiteZIP_PLUS = string.Empty;
            SitePHONE = string.Empty;
            SiteFAX = string.Empty;
            SiteOTHER_PHONE = string.Empty;
            SITE_FUNDED_SLOTS = string.Empty;
            SiteNOCHILD_PLA = string.Empty;
            SiteNOCHILD_WID = string.Empty;
            SiteNOCHILD_PLA_SN = string.Empty;
            SiteNOCHILD_WID_SN = string.Empty;
            SiteDIR_CODE = string.Empty;
            SiteDIR_LITERAL = string.Empty;
            SiteFW_CODE = string.Empty;
            SiteFW_LITERAL = string.Empty;
            SiteDIR_TITLE = string.Empty;
            SiteLICENSE_NO = string.Empty;
            SiteLICENSE_DATES = string.Empty;
            SiteCLASS_ROOMS = string.Empty;
            SiteTEACHERS_B = string.Empty;
            SiteTEACHERS_AIDS_B = string.Empty;
            SiteTEACHERS_HEAD_B = string.Empty;
            SiteCENTER_AIDS_B = string.Empty;
            SiteVAN_DRIVERS_B = string.Empty;
            SitePOS_EXP1_B = string.Empty;
            SitePOS_EXP2_B = string.Empty;
            SitePOS_EXP3_B = string.Empty;
            SitePOS_EXP4_B = string.Empty;
            SitePOS_EXP5_B = string.Empty;
            SitePOS_EXP6_B = string.Empty;
            SitePOS_EXP7_B = string.Empty;
            SiteTEACHERS_A = string.Empty;
            SiteTEACHERS_AIDS_A = string.Empty;
            SiteTEACHERS_HEAD_A = string.Empty;
            SiteCENTER_AIDS_A = string.Empty;
            SiteVAN_DRIVERS_A = string.Empty;
            SitePOS_EXP1_A = string.Empty;
            SitePOS_EXP2_A = string.Empty;
            SitePOS_EXP3_A = string.Empty;
            SitePOS_EXP4_A = string.Empty;
            SitePOS_EXP5_A = string.Empty;
            SitePOS_EXP6_A = string.Empty;
            SitePOS_EXP7_A = string.Empty;
            SiteOUTSIDE_PLAY = string.Empty;
            SiteOUTSIDE_PLAY_WAIVED = string.Empty;
            SiteOPEN_DATE = string.Empty;
            SiteCLOSE_DATE = string.Empty;
            SiteOPEN_DAYS = string.Empty;
            SiteSTART_TIME = string.Empty;
            SiteEND_TIME = string.Empty;
            SiteCLASS_START = string.Empty;
            SiteCLASS_END = string.Empty;
            SiteCLASS_DAYS = string.Empty;
            SiteTRAN_AREA = string.Empty;
            SiteRENTAL_FEE = string.Empty;
            SiteUTILITY_FEE = string.Empty;
            SiteL_FSPR_DT = string.Empty;
            SiteL_FSPR_NT = string.Empty;
            SiteL_FSSV_DT = string.Empty;
            SiteL_FSSV_NT = string.Empty;
            SiteL_FSST_DT = string.Empty;
            SiteL_FSST_NT = string.Empty;
            SiteL_HLDP_DT = string.Empty;
            SiteL_HLDP_NT = string.Empty;
            SiteL_FIDP_DT = string.Empty;
            SiteL_FIDP_NT = string.Empty;
            SiteL_FIEX_DT = string.Empty;
            SiteL_FIEX_NT = string.Empty;
            SiteL_SMAL_DT = string.Empty;
            SiteL_SMAL_NT = string.Empty;
            SiteL_HESY_DT = string.Empty;
            SiteL_HESY_NT = string.Empty;
            SiteL_WASY_DT = string.Empty;
            SiteL_WASY_NT = string.Empty;
            SiteL_DLED_DT = string.Empty;
            SiteL_DLED_NT = string.Empty;
            SiteL_BUIL_DT = string.Empty;
            SiteL_BUIL_NT = string.Empty;
            SiteL_EXP1_DT = string.Empty;
            SiteL_EXP1_NT = string.Empty;
            SiteL_EXP2_DT = string.Empty;
            SiteL_EXP2_NT = string.Empty;
            SiteL_EXP3_DT = string.Empty;
            SiteL_EXP3_NT = string.Empty;
            SiteLANGUAGE = string.Empty;
            SiteLANGUAGE_OTHER = string.Empty;
            SiteREP_SEQ = string.Empty;
            SiteREP_OPT1 = string.Empty;
            SiteREP_OPT2 = string.Empty;
            SiteMEAL_AREA = string.Empty;
            SiteCOMMENT = string.Empty;
            //SiteRT1 = string.Empty;
            //SiteRT2 = string.Empty;
            SiteFUND1 = string.Empty;
            SiteFUND2 = string.Empty;
            SiteFUND3 = string.Empty;
            SiteFUND4 = string.Empty;
            SiteFUND5 = string.Empty;
            SiteHEDTEACHER = string.Empty;
            SiteASSISTANT1 = string.Empty;
            SiteASSISTANT2 = string.Empty;
            SiteC_DIRECTOR = string.Empty;
            SiteACTIVE = string.Empty;
            SitePROJECT = string.Empty;
            SiteDATE_ADD = string.Empty;
            SiteADD_OPERATOR = string.Empty;
            SiteDATE_LSTC = string.Empty;
            SiteLSTC_OPERATOR = string.Empty;
            SITE_FUND_SLOT1 = string.Empty;
            SITE_FUND_SLOT2 = string.Empty;
            SITE_FUND_SLOT3 = string.Empty;
            SITE_FUND_SLOT4 = string.Empty;
            SITE_FUND_SLOT5 = string.Empty;
            SITE_HOMEBASE=String.Empty;  

        }

        public CaseSiteEntity(bool Intialize)
        {
            if (Intialize)
            {
                Row_Type = null;
                SiteAGENCY = null;
                SiteDEPT = null;
                SitePROG = null;
                SiteYEAR = null;
                SiteNUMBER = null;
                SiteROOM = null;
                SiteAM_PM = null;
                SiteNAME = null;
                SiteSTREET = null;
                SiteCITY = null;
                SiteSTATE = null;
                SiteZIP = null;
                SiteZIP_PLUS = null;
                SitePHONE = null;
                SiteFAX = null;
                SiteOTHER_PHONE = null;
                SITE_FUNDED_SLOTS = null;
                SiteNOCHILD_PLA = null;
                SiteNOCHILD_WID = null;
                SiteNOCHILD_PLA_SN = null;
                SiteNOCHILD_WID_SN = null;
                SiteDIR_CODE = null;
                SiteDIR_LITERAL = null;
                SiteFW_CODE = null;
                SiteFW_LITERAL = null;
                SiteDIR_TITLE = null;
                SiteLICENSE_NO = null;
                SiteLICENSE_DATES = null;
                SiteCLASS_ROOMS = null;
                SiteTEACHERS_B = null;
                SiteTEACHERS_AIDS_B = null;
                SiteTEACHERS_HEAD_B = null;
                SiteCENTER_AIDS_B = null;
                SiteVAN_DRIVERS_B = null;
                SitePOS_EXP1_B = null;
                SitePOS_EXP2_B = null;
                SitePOS_EXP3_B = null;
                SitePOS_EXP4_B = null;
                SitePOS_EXP5_B = null;
                SitePOS_EXP6_B = null;
                SitePOS_EXP7_B = null;
                SiteTEACHERS_A = null;
                SiteTEACHERS_AIDS_A = null;
                SiteTEACHERS_HEAD_A = null;
                SiteCENTER_AIDS_A = null;
                SiteVAN_DRIVERS_A = null;
                SitePOS_EXP1_A = null;
                SitePOS_EXP2_A = null;
                SitePOS_EXP3_A = null;
                SitePOS_EXP4_A = null;
                SitePOS_EXP5_A = null;
                SitePOS_EXP6_A = null;
                SitePOS_EXP7_A = null;
                SiteOUTSIDE_PLAY = null;
                SiteOUTSIDE_PLAY_WAIVED = null;
                SiteOPEN_DATE = null;
                SiteCLOSE_DATE = null;
                SiteOPEN_DAYS = null;
                SiteSTART_TIME = null;
                SiteEND_TIME = null;
                SiteCLASS_START = null;
                SiteCLASS_END = null;
                SiteCLASS_DAYS = null;
                SiteTRAN_AREA = null;
                SiteRENTAL_FEE = null;
                SiteUTILITY_FEE = null;
                SiteL_FSPR_DT = null;
                SiteL_FSPR_NT = null;
                SiteL_FSSV_DT = null;
                SiteL_FSSV_NT = null;
                SiteL_FSST_DT = null;
                SiteL_FSST_NT = null;
                SiteL_HLDP_DT = null;
                SiteL_HLDP_NT = null;
                SiteL_FIDP_DT = null;
                SiteL_FIDP_NT = null;
                SiteL_FIEX_DT = null;
                SiteL_FIEX_NT = null;
                SiteL_SMAL_DT = null;
                SiteL_SMAL_NT = null;
                SiteL_HESY_DT = null;
                SiteL_HESY_NT = null;
                SiteL_WASY_DT = null;
                SiteL_WASY_NT = null;
                SiteL_DLED_DT = null;
                SiteL_DLED_NT = null;
                SiteL_BUIL_DT = null;
                SiteL_BUIL_NT = null;
                SiteL_EXP1_DT = null;
                SiteL_EXP1_NT = null;
                SiteL_EXP2_DT = null;
                SiteL_EXP2_NT = null;
                SiteL_EXP3_DT = null;
                SiteL_EXP3_NT = null;
                SiteLANGUAGE = null;
                SiteLANGUAGE_OTHER = null;
                SiteREP_SEQ = null;
                SiteREP_OPT1 = null;
                SiteREP_OPT2 = null;
                SiteMEAL_AREA = null;
                SiteCOMMENT = null;
                //SiteRT1 = null;
                //SiteRT2 = null;
                SiteFUND1 = null;
                SiteFUND2 = null;
                SiteFUND3 = null;
                SiteFUND4 = null;
                SiteFUND5 = null;
                SiteHEDTEACHER = null;
                SiteASSISTANT1 = null;
                SiteASSISTANT2 = null;
                SiteC_DIRECTOR = null;
                SiteACTIVE = null;
                SitePROJECT = null;
                SiteDATE_ADD = null;
                SiteADD_OPERATOR = null;
                SiteDATE_LSTC = null;
                SiteLSTC_OPERATOR = null;
                SITE_FUND_SLOT1 = null;
                SITE_FUND_SLOT2 = null;
                SITE_FUND_SLOT3 = null;
                SITE_FUND_SLOT4 = null;
                SITE_FUND_SLOT5 = null;
                SITE_HOMEBASE = null;
            }
        }

        public CaseSiteEntity(CaseSiteEntity Entity)
        {
            if (Entity != null)
            {
                SiteNUMBER = Entity.SiteNUMBER;
                SiteROOM = Entity.SiteROOM;
                // Seq = Entity.Seq;
                SiteAM_PM = Entity.SiteAM_PM;
                //Sel_Count = Entity.Sel_Count;
            }
        }

        public CaseSiteEntity(DataRow row)
        {
            if (row != null)
            {
                Row_Type = "U";
                SiteAGENCY = row["SITE_AGENCY"].ToString().Trim();
                SiteDEPT = row["SITE_DEPT"].ToString().Trim();
                SitePROG = row["SITE_PROG"].ToString().Trim();
                SiteYEAR = row["SITE_YEAR"].ToString().Trim();
                SiteNUMBER = row["SITE_NUMBER"].ToString().Trim();
                SiteROOM = row["SITE_ROOM"].ToString().Trim();
                SiteAM_PM = row["SITE_AM_PM"].ToString().Trim();
                SiteNAME = row["SITE_NAME"].ToString().Trim();
                SiteSTREET = row["SITE_STREET"].ToString().Trim();
                SiteCITY = row["SITE_CITY"].ToString().Trim();
                SiteSTATE = row["SITE_STATE"].ToString().Trim();
                SiteZIP = row["SITE_ZIP"].ToString().Trim();
                SiteZIP_PLUS = row["SITE_ZIP_PLUS"].ToString().Trim();
                SitePHONE = row["SITE_PHONE"].ToString().Trim();
                SiteFAX = row["SITE_FAX"].ToString().Trim();
                SiteOTHER_PHONE = row["SITE_OTHER_PHONE"].ToString().Trim();
                SITE_FUNDED_SLOTS = row["SITE_FUNDED_SLOTS"].ToString().Trim();
                SiteNOCHILD_PLA = row["SITE_NOCHILD_PLA"].ToString().Trim();
                SiteNOCHILD_WID = row["SITE_NOCHILD_WID"].ToString().Trim();
                SiteNOCHILD_PLA_SN = row["SITE_NOCHILD_PLA_SN"].ToString().Trim();
                SiteNOCHILD_WID_SN = row["SITE_NOCHILD_WID_SN"].ToString().Trim();
                SiteDIR_CODE = row["SITE_DIR_CODE"].ToString().Trim();
                SiteDIR_LITERAL = row["SITE_DIR_LITERAL"].ToString().Trim();
                SiteFW_CODE = row["SITE_FW_CODE"].ToString().Trim();
                SiteFW_LITERAL = row["SITE_FW_LITERAL"].ToString().Trim();
                SiteDIR_TITLE = row["SITE_DIR_TITLE"].ToString().Trim();
                SiteLICENSE_NO = row["SITE_LICENSE_NO"].ToString().Trim();
                SiteLICENSE_DATES = row["SITE_LICENSE_DATES"].ToString().Trim();
                SiteCLASS_ROOMS = row["SITE_CLASS_ROOMS"].ToString().Trim();
                SiteTEACHERS_B = row["SITE_TEACHERS_B"].ToString().Trim();
                SiteTEACHERS_AIDS_B = row["SITE_TEACHERS_AIDS_B"].ToString().Trim();
                SiteTEACHERS_HEAD_B = row["SITE_TEACHERS_HEAD_B"].ToString().Trim();
                SiteCENTER_AIDS_B = row["SITE_CENTER_AIDS_B"].ToString().Trim();
                SiteVAN_DRIVERS_B = row["SITE_VAN_DRIVERS_B"].ToString().Trim();
                SitePOS_EXP1_B = row["SITE_POS_EXP1_B"].ToString().Trim();
                SitePOS_EXP2_B = row["SITE_POS_EXP2_B"].ToString().Trim();
                SitePOS_EXP3_B = row["SITE_POS_EXP3_B"].ToString().Trim();
                SitePOS_EXP4_B = row["SITE_POS_EXP4_B"].ToString().Trim();
                SitePOS_EXP5_B = row["SITE_POS_EXP5_B"].ToString().Trim();
                SitePOS_EXP6_B = row["SITE_POS_EXP6_B"].ToString().Trim();
                SitePOS_EXP7_B = row["SITE_POS_EXP7_B"].ToString().Trim();
                SiteTEACHERS_A = row["SITE_TEACHERS_A"].ToString().Trim();
                SiteTEACHERS_AIDS_A = row["SITE_TEACHERS_AIDS_A"].ToString().Trim();
                SiteTEACHERS_HEAD_A = row["SITE_TEACHERS_HEAD_A"].ToString().Trim();
                SiteCENTER_AIDS_A = row["SITE_CENTER_AIDS_A"].ToString().Trim();
                SiteVAN_DRIVERS_A = row["SITE_VAN_DRIVERS_A"].ToString().Trim();
                SitePOS_EXP1_A = row["SITE_POS_EXP1_A"].ToString().Trim();
                SitePOS_EXP2_A = row["SITE_POS_EXP2_A"].ToString().Trim();
                SitePOS_EXP3_A = row["SITE_POS_EXP3_A"].ToString().Trim();
                SitePOS_EXP4_A = row["SITE_POS_EXP4_A"].ToString().Trim();
                SitePOS_EXP5_A = row["SITE_POS_EXP5_A"].ToString().Trim();
                SitePOS_EXP6_A = row["SITE_POS_EXP6_A"].ToString().Trim();
                SitePOS_EXP7_A = row["SITE_POS_EXP7_A"].ToString().Trim();
                SiteOUTSIDE_PLAY = row["SITE_OUTSIDE_PLAY"].ToString().Trim();
                SiteOUTSIDE_PLAY_WAIVED = row["SITE_OUTSIDE_PLAY_WAIVED"].ToString().Trim();
                SiteOPEN_DATE = row["SITE_OPEN_DATE"].ToString().Trim();
                SiteCLOSE_DATE = row["SITE_CLOSE_DATE"].ToString().Trim();
                SiteOPEN_DAYS = row["SITE_OPEN_DAYS"].ToString().Trim();
                SiteSTART_TIME = row["SITE_START_TIME"].ToString().Trim();
                SiteEND_TIME = row["SITE_END_TIME"].ToString().Trim();
                SiteCLASS_START = row["SITE_CLASS_START"].ToString().Trim();
                SiteCLASS_END = row["SITE_CLASS_END"].ToString().Trim();
                SiteCLASS_DAYS = row["SITE_CLASS_DAYS"].ToString().Trim();
                SiteTRAN_AREA = row["SITE_TRAN_AREA"].ToString().Trim();
                SiteRENTAL_FEE = row["SITE_RENTAL_FEE"].ToString().Trim();
                SiteUTILITY_FEE = row["SITE_UTILITY_FEE"].ToString().Trim();
                SiteL_FSPR_DT = row["SITE_L_FSPR_DT"].ToString().Trim();
                SiteL_FSPR_NT = row["SITE_L_FSPR_NT"].ToString().Trim();
                SiteL_FSSV_DT = row["SITE_L_FSSV_DT"].ToString().Trim();
                SiteL_FSSV_NT = row["SITE_L_FSSV_NT"].ToString().Trim();
                SiteL_FSST_DT = row["SITE_L_FSST_DT"].ToString().Trim();
                SiteL_FSST_NT = row["SITE_L_FSST_NT"].ToString().Trim();
                SiteL_HLDP_DT = row["SITE_L_HLDP_DT"].ToString().Trim();
                SiteL_HLDP_NT = row["SITE_L_HLDP_NT"].ToString().Trim();
                SiteL_FIDP_DT = row["SITE_L_FIDP_DT"].ToString().Trim();
                SiteL_FIDP_NT = row["SITE_L_FIDP_NT"].ToString().Trim();
                SiteL_FIEX_DT = row["SITE_L_FIEX_DT"].ToString().Trim();
                SiteL_FIEX_NT = row["SITE_L_FIEX_NT"].ToString().Trim();
                SiteL_SMAL_DT = row["SITE_L_SMAL_DT"].ToString().Trim();
                SiteL_SMAL_NT = row["SITE_L_SMAL_NT"].ToString().Trim();
                SiteL_HESY_DT = row["SITE_L_HESY_DT"].ToString().Trim();
                SiteL_HESY_NT = row["SITE_L_HESY_NT"].ToString().Trim();
                SiteL_WASY_DT = row["SITE_L_WASY_DT"].ToString().Trim();
                SiteL_WASY_NT = row["SITE_L_WASY_NT"].ToString().Trim();
                SiteL_DLED_DT = row["SITE_L_DLED_DT"].ToString().Trim();
                SiteL_DLED_NT = row["SITE_L_DLED_NT"].ToString().Trim();
                SiteL_BUIL_DT = row["SITE_L_BUIL_DT"].ToString().Trim();
                SiteL_BUIL_NT = row["SITE_L_BUIL_NT"].ToString().Trim();
                SiteL_EXP1_DT = row["SITE_L_EXP1_DT"].ToString().Trim();
                SiteL_EXP1_NT = row["SITE_L_EXP1_NT"].ToString().Trim();
                SiteL_EXP2_DT = row["SITE_L_EXP2_DT"].ToString().Trim();
                SiteL_EXP2_NT = row["SITE_L_EXP2_NT"].ToString().Trim();
                SiteL_EXP3_DT = row["SITE_L_EXP3_DT"].ToString().Trim();
                SiteL_EXP3_NT = row["SITE_L_EXP3_NT"].ToString().Trim();
                SiteLANGUAGE = row["SITE_LANGUAGE"].ToString().Trim();
                SiteLANGUAGE_OTHER = row["SITE_LANGUAGE_OTHER"].ToString().Trim();
                SiteREP_SEQ = row["SITE_REP_SEQ"].ToString().Trim();
                SiteREP_OPT1 = row["SITE_REP_OPT1"].ToString().Trim();
                SiteREP_OPT2 = row["SITE_REP_OPT2"].ToString().Trim();
                SiteMEAL_AREA = row["SITE_MEAL_AREA"].ToString().Trim();
                SiteCOMMENT = row["SITE_COMMENT"].ToString().Trim();
                //SiteRT1 = row["SITE_RT1"].ToString();
                //SiteRT2 = row["SITE_RT2"].ToString();
                SiteFUND1 = row["SITE_FUND1"].ToString().Trim();
                SiteFUND2 = row["SITE_FUND2"].ToString().Trim();
                SiteFUND3 = row["SITE_FUND3"].ToString().Trim();
                SiteFUND4 = row["SITE_FUND4"].ToString().Trim();
                SiteFUND5 = row["SITE_FUND5"].ToString().Trim();
                SiteHEDTEACHER = row["SITE_HEDTEACHER"].ToString().Trim();
                SiteASSISTANT1 = row["SITE_ASSISTANT1"].ToString().Trim();
                SiteASSISTANT2 = row["SITE_ASSISTANT2"].ToString().Trim();
                SiteC_DIRECTOR = row["SITE_C_DIRECTOR"].ToString().Trim();
                SiteACTIVE = row["SITE_ACTIVE"].ToString().Trim();
                SitePROJECT = row["SITE_PROJECT"].ToString().Trim();
                SiteDATE_ADD = row["SITE_DATE_ADD"].ToString().Trim();
                SiteADD_OPERATOR = row["SITE_ADD_OPERATOR"].ToString().Trim();
                SiteDATE_LSTC = row["SITE_DATE_LSTC"].ToString().Trim().Trim();
                SiteLSTC_OPERATOR = row["SITE_LSTC_OPERATOR"].ToString().Trim();
                SITE_FUND_SLOT1 = row["SITE_FUND_SLOT1"].ToString().Trim();
                SITE_FUND_SLOT2 = row["SITE_FUND_SLOT2"].ToString().Trim();
                SITE_FUND_SLOT3 = row["SITE_FUND_SLOT3"].ToString().Trim();
                SITE_FUND_SLOT4 = row["SITE_FUND_SLOT4"].ToString().Trim();
                SITE_FUND_SLOT5 = row["SITE_FUND_SLOT5"].ToString().Trim();
                SITE_HOMEBASE = row["SITE_HOMEBASE"].ToString().Trim();

            }

        }

        public CaseSiteEntity(DataRow row, string strTable)
        {
            if (strTable=="SiteHie")
            {
                SiteNUMBER = row["SITE_NUMBER"].ToString();
                SiteNAME = row["SITE_NAME"].ToString();
                SiteCITY = row["SITE_CITY"].ToString();
                SiteACTIVE = row["SITE_ACTIVE"].ToString();
                InActiveFlag = "false";
            }
            else if (strTable == "Site")
            {
                SiteNUMBER = row["SITE_NUMBER"].ToString();
                SiteNAME = row["SITE_NAME"].ToString();
                SiteCITY = row["SITE_CITY"].ToString();
                InActiveFlag = "false";
            }
            else if (strTable == "PIRMISC")
            {
                SiteNUMBER = row["SITE_NUMBER"].ToString();
                SiteROOM = row["SITE_ROOM"].ToString();
                SiteAM_PM = row["SITE_AM_PM"].ToString();
                SiteNAME = row["SITE_NAME"].ToString();
            }

        }

        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string SiteAGENCY { get; set; }
        public string SiteDEPT { get; set; }
        public string SitePROG { get; set; }
        public string SiteYEAR { get; set; }
        public string SiteNUMBER { get; set; }
        public string SiteROOM { get; set; }
        public string SiteAM_PM { get; set; }
        public string SiteNAME { get; set; }
        public string SiteSTREET { get; set; }
        public string SiteCITY { get; set; }
        public string SiteSTATE { get; set; }
        public string SiteZIP { get; set; }
        public string SiteZIP_PLUS { get; set; }
        public string SitePHONE { get; set; }
        public string SiteFAX { get; set; }
        public string SiteOTHER_PHONE { get; set; }
        public string SITE_FUNDED_SLOTS { get; set; }
        public string SiteNOCHILD_PLA { get; set; }
        public string SiteNOCHILD_WID { get; set; }
        public string SiteNOCHILD_PLA_SN { get; set; }
        public string SiteNOCHILD_WID_SN { get; set; }
        public string SiteDIR_CODE { get; set; }
        public string SiteDIR_LITERAL { get; set; }
        public string SiteFW_CODE { get; set; }
        public string SiteFW_LITERAL { get; set; }
        public string SiteDIR_TITLE { get; set; }
        public string SiteLICENSE_NO { get; set; }
        public string SiteLICENSE_DATES { get; set; }
        public string SiteCLASS_ROOMS { get; set; }
        public string SiteTEACHERS_B { get; set; }
        public string SiteTEACHERS_AIDS_B { get; set; }
        public string SiteTEACHERS_HEAD_B { get; set; }
        public string SiteCENTER_AIDS_B { get; set; }
        public string SiteVAN_DRIVERS_B { get; set; }
        public string SitePOS_EXP1_B { get; set; }
        public string SitePOS_EXP2_B { get; set; }
        public string SitePOS_EXP3_B { get; set; }
        public string SitePOS_EXP4_B { get; set; }
        public string SitePOS_EXP5_B { get; set; }
        public string SitePOS_EXP6_B { get; set; }
        public string SitePOS_EXP7_B { get; set; }
        public string SiteTEACHERS_A { get; set; }
        public string SiteTEACHERS_AIDS_A { get; set; }
        public string SiteTEACHERS_HEAD_A { get; set; }
        public string SiteCENTER_AIDS_A { get; set; }
        public string SiteVAN_DRIVERS_A { get; set; }
        public string SitePOS_EXP1_A { get; set; }
        public string SitePOS_EXP2_A { get; set; }
        public string SitePOS_EXP3_A { get; set; }
        public string SitePOS_EXP4_A { get; set; }
        public string SitePOS_EXP5_A { get; set; }
        public string SitePOS_EXP6_A { get; set; }
        public string SitePOS_EXP7_A { get; set; }
        public string SiteOUTSIDE_PLAY { get; set; }
        public string SiteOUTSIDE_PLAY_WAIVED { get; set; }
        public string SiteOPEN_DATE { get; set; }
        public string SiteCLOSE_DATE { get; set; }
        public string SiteOPEN_DAYS { get; set; }
        public string SiteSTART_TIME { get; set; }
        public string SiteEND_TIME { get; set; }
        public string SiteCLASS_START { get; set; }
        public string SiteCLASS_END { get; set; }
        public string SiteCLASS_DAYS { get; set; }
        public string SiteTRAN_AREA { get; set; }
        public string SiteRENTAL_FEE { get; set; }
        public string SiteUTILITY_FEE { get; set; }
        public string SiteL_FSPR_DT { get; set; }
        public string SiteL_FSPR_NT { get; set; }
        public string SiteL_FSSV_DT { get; set; }
        public string SiteL_FSSV_NT { get; set; }
        public string SiteL_FSST_DT { get; set; }
        public string SiteL_FSST_NT { get; set; }
        public string SiteL_HLDP_DT { get; set; }
        public string SiteL_HLDP_NT { get; set; }
        public string SiteL_FIDP_DT { get; set; }
        public string SiteL_FIDP_NT { get; set; }
        public string SiteL_FIEX_DT { get; set; }
        public string SiteL_FIEX_NT { get; set; }
        public string SiteL_SMAL_DT { get; set; }
        public string SiteL_SMAL_NT { get; set; }
        public string SiteL_HESY_DT { get; set; }
        public string SiteL_HESY_NT { get; set; }
        public string SiteL_WASY_DT { get; set; }
        public string SiteL_WASY_NT { get; set; }
        public string SiteL_DLED_DT { get; set; }
        public string SiteL_DLED_NT { get; set; }
        public string SiteL_BUIL_DT { get; set; }
        public string SiteL_BUIL_NT { get; set; }
        public string SiteL_EXP1_DT { get; set; }
        public string SiteL_EXP1_NT { get; set; }
        public string SiteL_EXP2_DT { get; set; }
        public string SiteL_EXP2_NT { get; set; }
        public string SiteL_EXP3_DT { get; set; }
        public string SiteL_EXP3_NT { get; set; }
        public string SiteLANGUAGE { get; set; }
        public string SiteLANGUAGE_OTHER { get; set; }
        public string SiteREP_SEQ { get; set; }
        public string SiteREP_OPT1 { get; set; }
        public string SiteREP_OPT2 { get; set; }
        public string SiteMEAL_AREA { get; set; }
        public string SiteCOMMENT { get; set; }
        //public string SiteRT1 { get; set; }
        //public string SiteRT2 { get; set; }
        public string SiteFUND1 { get; set; }
        public string SiteFUND2 { get; set; }
        public string SiteFUND3 { get; set; }
        public string SiteFUND4 { get; set; }
        public string SiteFUND5 { get; set; }
        public string SiteHEDTEACHER { get; set; }
        public string SiteASSISTANT1 { get; set; }
        public string SiteASSISTANT2 { get; set; }
        public string SiteC_DIRECTOR { get; set; }
        public string SiteACTIVE { get; set; }
        public string SitePROJECT { get; set; }
        public string SiteDATE_ADD { get; set; }
        public string SiteADD_OPERATOR { get; set; }
        public string SiteDATE_LSTC { get; set; }
        public string SiteLSTC_OPERATOR { get; set; }
        public string SITE_FUND_SLOT1 { get; set; }
        public string SITE_FUND_SLOT2 { get; set; }
        public string SITE_FUND_SLOT3 { get; set; }
        public string SITE_FUND_SLOT4 { get; set; }
        public string SITE_FUND_SLOT5 { get; set; }
        public string SITE_HOMEBASE { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string InActiveFlag { get; set; }
        #endregion
    }

    public class SiteScheduleEntity
    {
        #region Constructors

        public SiteScheduleEntity()
        {

            Row_Type = string.Empty;
            ATTM_AGENCY = string.Empty;
            ATTM_DEPT = string.Empty;
            ATTM_PROG = string.Empty;
            ATTM_YEAR = string.Empty;
            ATTM_MONTH = string.Empty;
            ATTM_SITE = string.Empty;
            ATTM_ROOM = string.Empty;
            ATTM_AMPM = string.Empty;
            ATTM_FUND = string.Empty;
            ATTM_CALENDER_YEAR = string.Empty;
            ATTM_ID = string.Empty;
            ATTM_DATE_ADD = string.Empty;
            ATTM_ADD_OPERATOR = string.Empty;
            ATTM_DATE_LSTC = string.Empty;
            ATTM_LSTC_OPERATOR = string.Empty;
            

        }

        public SiteScheduleEntity(bool Intialize)
        {
            if (Intialize)
            {
                Row_Type = null;
                ATTM_AGENCY = null;
                ATTM_DEPT = null;
                ATTM_PROG = null;
                ATTM_YEAR = null;
                ATTM_MONTH = null;
                ATTM_SITE = null;
                ATTM_ROOM = null;
                ATTM_AMPM = null;
                ATTM_FUND = null;
                ATTM_CALENDER_YEAR = null;
                ATTM_ID = null;
                ATTM_DATE_ADD = null;
                ATTM_ADD_OPERATOR = null;
                ATTM_DATE_LSTC = null;
                ATTM_LSTC_OPERATOR = null;
                
            }
        }

        public SiteScheduleEntity(DataRow row)
        {
            if (row != null)
            {
                Row_Type = "U";
                ATTM_AGENCY = row["ATTM_AGENCY"].ToString();
                ATTM_DEPT = row["ATTM_DEPT"].ToString();
                ATTM_PROG = row["ATTM_PROG"].ToString();
                ATTM_YEAR = row["ATTM_YEAR"].ToString();
                ATTM_MONTH = row["ATTM_MONTH"].ToString();
                ATTM_SITE = row["ATTM_SITE"].ToString();
                ATTM_ROOM = row["ATTM_ROOM"].ToString();
                ATTM_AMPM = row["ATTM_AMPM"].ToString();
                ATTM_FUND = row["ATTM_FUND"].ToString();
                ATTM_CALENDER_YEAR = row["ATTM_CALENDER_YEAR"].ToString();
                ATTM_ID = row["ATTM_ID"].ToString();
                ATTM_DATE_ADD = row["ATTM_DATE_ADD"].ToString();
                ATTM_ADD_OPERATOR = row["ATTM_ADD_OPERATOR"].ToString();
                ATTM_DATE_LSTC = row["ATTM_DATE_LSTC"].ToString();
                ATTM_LSTC_OPERATOR = row["ATTM_LSTC_OPERATOR"].ToString();
                
            }

        }

        

        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string ATTM_AGENCY { get; set; }
        public string ATTM_DEPT { get; set; }
        public string ATTM_PROG { get; set; }
        public string ATTM_MONTH { get; set; }
        public string ATTM_YEAR { get; set; }
        public string ATTM_SITE { get; set; }
        public string ATTM_ROOM { get; set; }
        public string ATTM_AMPM { get; set; }
        public string ATTM_FUND { get; set; }
        public string ATTM_CALENDER_YEAR { get; set; }
        public string ATTM_ID { get; set; }
        public string ATTM_DATE_ADD { get; set; }
        public string ATTM_ADD_OPERATOR { get; set; }
        public string ATTM_DATE_LSTC { get; set; }
        public string ATTM_LSTC_OPERATOR { get; set; }
        
        public string Mode { get; set; }
        public string Type { get; set; }
        public string InActiveFlag { get; set; }
        #endregion
    }


    public class ChildATTMSEntity
    {
        #region Constructors

        public ChildATTMSEntity()
        {

            Row_Type = string.Empty;
            ATTMS_ID = string.Empty;
            ATTMS_DAY = string.Empty;
            ATTMS_WEEK = string.Empty;
            ATTMS_STATUS = string.Empty;
        }

        public ChildATTMSEntity(bool Intialize)
        {
            if (Intialize)
            {
                Row_Type = null;
                ATTMS_ID = null;
                ATTMS_DAY = null;
                ATTMS_WEEK = null;
                ATTMS_STATUS = null;
            }
        }

        public ChildATTMSEntity(ChildATTMSEntity Entity)
        {
            Row_Type = Entity.Row_Type;
            ATTMS_ID = Entity.ATTMS_ID;
            ATTMS_DAY = Entity.ATTMS_DAY;
            ATTMS_WEEK = Entity.ATTMS_WEEK;
            ATTMS_STATUS = Entity.ATTMS_STATUS;
            IsDatafill = Entity.IsDatafill;
        }

        public ChildATTMSEntity(DataRow row)
        {
            if (row != null)
            {
                Row_Type = "U";
                ATTMS_ID = row["ATTMS_ID"].ToString();
                ATTMS_DAY = row["ATTMS_DAY"].ToString();
                ATTMS_WEEK = row["ATTMS_WEEK"].ToString();
                ATTMS_STATUS = row["ATTMS_STATUS"].ToString();
                IsDatafill = "N";
            }

        }        

        public ChildATTMSEntity(DataRow row,string strTable)
        {
            if (row != null)
            {
                if (strTable == "ALL")
                {
                    Row_Type = "U";
                    ATTMS_ID = row["ATTMS_ID"].ToString().Trim();
                    ATTMS_DAY = row["ATTMS_DAY"].ToString();
                    ATTMS_WEEK = row["ATTMS_WEEK"].ToString().Trim();
                    ATTMS_STATUS = row["ATTMS_STATUS"].ToString().Trim();
                    ATTM_MONTH = row["ATTM_MONTH"].ToString().Trim();
                    ATTM_CALENDER_YEAR = row["ATTM_CALENDER_YEAR"].ToString().Trim();
                    AttM_Site = row["ATTM_SITE"].ToString().Trim();
                    AttM_Room = row["ATTM_ROOM"].ToString().Trim();
                    AttM_AMPM = row["ATTM_AMPM"].ToString().Trim();
                    AttM_AgyStatus = row["AGY_6"].ToString().Trim();
                    Attm_Fund = row["ATTM_FUND"].ToString();
                }
                else
                {
                    Row_Type = "U";
                    ATTMS_ID = row["ATTMS_ID"].ToString();
                    ATTMS_DAY = row["ATTMS_DAY"].ToString();
                    ATTMS_WEEK = row["ATTMS_WEEK"].ToString();
                    ATTMS_STATUS = row["ATTMS_STATUS"].ToString();
                    ATTM_MONTH = row["ATTM_MONTH"].ToString();
                    ATTM_CALENDER_YEAR = row["ATTM_CALENDER_YEAR"].ToString();
                    AGY_2 = row["AGY_2"].ToString();
                    Attm_Fund = row["ATTM_FUND"].ToString();
                }
            }

        }

        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string ATTMS_ID { get; set; }
        public string ATTMS_DAY { get; set; }
        public string ATTMS_WEEK { get; set; }
        public string ATTMS_STATUS { get; set; }
        public string ATTM_MONTH { get; set; }
        public string ATTM_CALENDER_YEAR { get; set; }
        public string AGY_2 { get; set; }
        public string AttM_Site { get; set; }
        public string AttM_Room { get; set; }
        public string AttM_AMPM { get; set; }
        public string AttM_AgyStatus { get; set; }
        public string Mode { get; set; }
        public string IsDatafill { get; set; }
        public string InActiveFlag { get; set; }
        public string Attm_Fund { get; set; }
        #endregion
    }




    public class Temp_SiteScheduleEntity
    {
        #region Constructors

        public Temp_SiteScheduleEntity()
        {

            Row_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            YEAR = string.Empty;
            MONTH = string.Empty;
            SITE = string.Empty;
            SCH_Type = string.Empty;
            Site_Name = string.Empty;
            MONTH_No = string.Empty;
            ROOM = string.Empty;
            AMPM = string.Empty;
            FUND = string.Empty;
            CALENDER_YEAR = string.Empty;
            ATTM_ID = string.Empty;
            ATTM_DATE_ADD = string.Empty;
            ATTM_ADD_OPERATOR = string.Empty;
            ATTM_DATE_LSTC = string.Empty;
            ATTM_LSTC_OPERATOR = string.Empty;


        }

        public Temp_SiteScheduleEntity(bool Intialize)
        {
            if (Intialize)
            {
                Row_Type = null;
                Agency = null;
                Dept = null;
                Prog = null;
                YEAR = null;
                MONTH = null;
                SCH_Type = null;
                Site_Name = null;
                MONTH_No = null;
                SITE = null;
                ROOM = null;
                AMPM = null;
                FUND = null;
                CALENDER_YEAR = null;
                ATTM_ID = null;
                ATTM_DATE_ADD = null;
                ATTM_ADD_OPERATOR = null;
                ATTM_DATE_LSTC = null;
                ATTM_LSTC_OPERATOR = null;

            }
        }

        public Temp_SiteScheduleEntity(Temp_SiteScheduleEntity Entity)
        {
            Row_Type = Entity.Row_Type;
            Agency = Entity.Agency;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            YEAR = Entity.YEAR;
            MONTH = Entity.MONTH;
            SCH_Type = Entity.SCH_Type;
            Site_Name = Entity.Site_Name;
            MONTH_No = Entity.MONTH_No;
            SITE = Entity.SITE;
            ROOM = Entity.ROOM;
            AMPM = Entity.AMPM;
            FUND = Entity.FUND;
            CALENDER_YEAR = Entity.CALENDER_YEAR;
            ATTM_ID = Entity.ATTM_ID;
            ATTM_DATE_ADD = Entity.ATTM_DATE_ADD;
            ATTM_ADD_OPERATOR = Entity.ATTM_ADD_OPERATOR;
            ATTM_DATE_LSTC = Entity.ATTM_DATE_LSTC;
            ATTM_LSTC_OPERATOR = Entity.ATTM_LSTC_OPERATOR;

            
        }



        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string MONTH { get; set; }
        public string MONTH_No { get; set; }
        public string YEAR { get; set; }
        public string SCH_Type { get; set; }
        public string Site_Name { get; set; }
        public string SITE { get; set; }
        public string ROOM { get; set; }
        public string AMPM { get; set; }
        public string FUND { get; set; }
        public string CALENDER_YEAR { get; set; }
        public string ATTM_ID { get; set; }
        public string ATTM_DATE_ADD { get; set; }
        public string ATTM_ADD_OPERATOR { get; set; }
        public string ATTM_DATE_LSTC { get; set; }
        public string ATTM_LSTC_OPERATOR { get; set; }

        public string Mode { get; set; }
        public string Type { get; set; }
        public string InActiveFlag { get; set; }
        #endregion
    }

    public class ReportSiteEntity
    {
        #region Constructors

        public ReportSiteEntity()
        {

            Row_Type = string.Empty;
            SiteAGENCY = string.Empty;
            SiteDEPT = string.Empty;
            SitePROG = string.Empty;
            SiteYEAR = string.Empty;
            SiteNUMBER = string.Empty;
            SiteROOM = string.Empty;
            SiteAM_PM = string.Empty;
            SiteNAME = string.Empty;
            SiteSTREET = string.Empty;
            SiteCITY = string.Empty;
            SiteSTATE = string.Empty;
            SiteZIP = string.Empty;
            SitePHONE = string.Empty;
            SiteFAX = string.Empty;
            SiteOTHER_PHONE = string.Empty;
            SITE_FUNDED_SLOTS = string.Empty;
            SiteSTART_TIME = string.Empty;
            SiteEND_TIME = string.Empty;
            SiteCLASS_START = string.Empty;
            SiteCLASS_END = string.Empty;
            SiteTRAN_AREA = string.Empty;
            SiteLANGUAGE = string.Empty;
            SiteLANGUAGE_OTHER = string.Empty;
            SiteMEAL_AREA = string.Empty;
            SiteFUND1 = string.Empty;
            SiteFUND2 = string.Empty;
            SiteFUND3 = string.Empty;
            SiteFUND4 = string.Empty;
            SiteFUND5 = string.Empty;
            SITEACTIVE = string.Empty;
        }

        public ReportSiteEntity(bool Intialize)
        {
            if (Intialize)
            {
                Row_Type = null;
                SiteAGENCY = null;
                SiteDEPT = null;
                SitePROG = null;
                SiteYEAR = null;
                SiteNUMBER = null;
                SiteROOM = null;
                SiteAM_PM = null;
                SiteNAME = null;
                SiteSTREET = null;
                SiteCITY = null;
                SiteSTATE = null;
                SiteZIP = null;
                SitePHONE = null;
                SiteFAX = null;
                SiteOTHER_PHONE = null;
                SITE_FUNDED_SLOTS = null;
                
                SiteSTART_TIME = null;
                SiteEND_TIME = null;
                SiteCLASS_START = null;
                SiteCLASS_END = null;
                SiteTRAN_AREA = null;
                SiteLANGUAGE = null;
                SiteLANGUAGE_OTHER = null;
                SiteMEAL_AREA = null;
                SiteFUND1 = null;
                SiteFUND2 = null;
                SiteFUND3 = null;
                SiteFUND4 = null;
                SiteFUND5 = null;
                SITEACTIVE = null;
            }
        }

        public ReportSiteEntity(CaseSiteEntity Entity)
        {
            if (Entity != null)
            {
                SiteNUMBER = Entity.SiteNUMBER;
                SiteROOM = Entity.SiteROOM;
                // Seq = Entity.Seq;
                SiteAM_PM = Entity.SiteAM_PM;
                //Sel_Count = Entity.Sel_Count;
            }
        }

        public ReportSiteEntity(DataRow row)
        {
            if (row != null)
            {
                Row_Type = "U";
                SiteAGENCY = row["SITE_AGENCY"].ToString().Trim();
                SiteDEPT = row["SITE_DEPT"].ToString().Trim();
                SitePROG = row["SITE_PROG"].ToString().Trim();
                SiteYEAR = row["SITE_YEAR"].ToString().Trim();
                SiteNUMBER = row["SITE_NUMBER"].ToString().Trim();
                SiteROOM = row["SITE_ROOM"].ToString().Trim();
                SiteAM_PM = row["SITE_AM_PM"].ToString().Trim();
                SiteNAME = row["SITE_NAME"].ToString().Trim();
                SiteSTREET = row["SITE_STREET"].ToString().Trim();
                SiteCITY = row["SITE_CITY"].ToString().Trim();
                SiteSTATE = row["SITE_STATE"].ToString().Trim();
                SiteZIP = row["SITE_ZIP"].ToString().Trim();
                SitePHONE = row["SITE_PHONE"].ToString().Trim();
                SiteFAX = row["SITE_FAX"].ToString().Trim();
                SiteOTHER_PHONE = row["SITE_OTHER_PHONE"].ToString().Trim();
                SITE_FUNDED_SLOTS = row["SITE_FUNDED_SLOTS"].ToString().Trim();
                SiteSTART_TIME = row["SITE_START_TIME"].ToString().Trim();
                SiteEND_TIME = row["SITE_END_TIME"].ToString().Trim();
                SiteCLASS_START = row["SITE_CLASS_START"].ToString().Trim();
                SiteCLASS_END = row["SITE_CLASS_END"].ToString().Trim();
                SiteTRAN_AREA = row["SITE_TRAN_AREA"].ToString().Trim();
                SiteLANGUAGE = row["SITE_LANGUAGE"].ToString().Trim();
                SiteLANGUAGE_OTHER = row["SITE_LANGUAGE_OTHER"].ToString().Trim();
                SiteMEAL_AREA = row["SITE_MEAL_AREA"].ToString().Trim();
                SiteFUND1 = row["SITE_FUND1"].ToString().Trim();
                SiteFUND2 = row["SITE_FUND2"].ToString().Trim();
                SiteFUND3 = row["SITE_FUND3"].ToString().Trim();
                SiteFUND4 = row["SITE_FUND4"].ToString().Trim();
                SiteFUND5 = row["SITE_FUND5"].ToString().Trim();
                SITEACTIVE = row["SITE_ACTIVE"].ToString().Trim();

            }

        }

       
        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string SiteAGENCY { get; set; }
        public string SiteDEPT { get; set; }
        public string SitePROG { get; set; }
        public string SiteYEAR { get; set; }
        public string SiteNUMBER { get; set; }
        public string SiteROOM { get; set; }
        public string SiteAM_PM { get; set; }
        public string SiteNAME { get; set; }
        public string SiteSTREET { get; set; }
        public string SiteCITY { get; set; }
        public string SiteSTATE { get; set; }
        public string SiteZIP { get; set; }
        public string SitePHONE { get; set; }
        public string SiteFAX { get; set; }
        public string SiteOTHER_PHONE { get; set; }
        public string SITE_FUNDED_SLOTS { get; set; }
        public string SiteSTART_TIME { get; set; }
        public string SiteEND_TIME { get; set; }
        public string SiteCLASS_START { get; set; }
        public string SiteCLASS_END { get; set; }
        public string SiteTRAN_AREA { get; set; }
        public string SiteLANGUAGE { get; set; }
        public string SiteLANGUAGE_OTHER { get; set; }

        public string SiteMEAL_AREA { get; set; }
        public string SiteFUND1 { get; set; }
        public string SiteFUND2 { get; set; }
        public string SiteFUND3 { get; set; }
        public string SiteFUND4 { get; set; }
        public string SiteFUND5 { get; set; }
        public string SITEACTIVE { get; set; }
        

        #endregion
    }

}
