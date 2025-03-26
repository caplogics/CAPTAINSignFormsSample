using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;
using Captain.Common.Utilities;


namespace Captain.Common.Model.Data
{
    public class CaseMstData
    {
        public CaseMstData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateCaseMst(CaseMstEntity MstEntity, out string strApplNo, out string strClientId, out string strFamilyId, out string strSSNO, out string strMSG)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            string strNewClientId = string.Empty;
            string strNewFamilyId = string.Empty;
            string strNewSSNO = string.Empty;
            string strMessage = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", MstEntity.ApplAgency));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", MstEntity.ApplDept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", MstEntity.ApplProgram));
                if (MstEntity.ApplYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_YEAR", MstEntity.ApplYr));
                sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                SqlParameter sqlApplNo = new SqlParameter("@MST_APP_NO_OUT", SqlDbType.VarChar, 10);
                sqlApplNo.Value = MstEntity.ApplNo;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);


                if (MstEntity.FamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_SEQ", MstEntity.FamilySeq));

                if (MstEntity.FamilyId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_ID", MstEntity.FamilyId));

                if (MstEntity.ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLIENT_ID", MstEntity.ClientId));

                if (MstEntity.Ssn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN", MstEntity.Ssn));
                sqlParamList.Add(new SqlParameter("@MST_BIC", MstEntity.Bic));
                if (MstEntity.NickName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NICKNAME", MstEntity.NickName));

                if (MstEntity.EthnicOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ETHNIC_OTHER", MstEntity.EthnicOther));

                if (MstEntity.State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE", MstEntity.State));

                if (MstEntity.City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CITY", MstEntity.City));

                if (MstEntity.Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STREET", MstEntity.Street));

                if (MstEntity.Suffix != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUFFIX", MstEntity.Suffix));

                if (MstEntity.Hn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HN", MstEntity.Hn));

                if (MstEntity.Direction != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DIRECTION", MstEntity.Direction));

                if (MstEntity.Apt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APT", MstEntity.Apt));

                if (MstEntity.Flr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FLR", MstEntity.Flr));

                if (MstEntity.Zip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIP", MstEntity.Zip));

                if (MstEntity.Zipplus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIPPLUS", MstEntity.Zipplus));

                if (MstEntity.Precinct != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PRECINCT", MstEntity.Precinct));

                if (MstEntity.Area != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_AREA", MstEntity.Area));

                if (MstEntity.Phone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PHONE", MstEntity.Phone));

                if (MstEntity.NextYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NEXTYEAR", MstEntity.NextYear));

                if (MstEntity.Classification != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLASSIFICATION", MstEntity.Classification));

                if (MstEntity.Language != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE", MstEntity.Language));

                if (MstEntity.LanguageOt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE_OT", MstEntity.LanguageOt));

                if (MstEntity.IntakeWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_WORKER", MstEntity.IntakeWorker));

                if (MstEntity.IntakeDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_DATE", MstEntity.IntakeDate));

                if (MstEntity.InitialDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INITIAL_DATE", MstEntity.InitialDate));

                if (MstEntity.CaseType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_TYPE", MstEntity.CaseType));

                if (MstEntity.Housing != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HOUSING", MstEntity.Housing));

                if (MstEntity.FamilyType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_TYPE", MstEntity.FamilyType));

                if (MstEntity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SITE", MstEntity.Site));

                if (MstEntity.Juvenile != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_JUVENILE", MstEntity.Juvenile));

                if (MstEntity.Senior != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SENIOR", MstEntity.Senior));

                if (MstEntity.Secret != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SECRET", MstEntity.Secret));

                if (MstEntity.CaseReviewDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_REVIEW_DATE", MstEntity.CaseReviewDate));

                if (MstEntity.AlertCodes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ALERT_CODES", MstEntity.AlertCodes));

                if (MstEntity.ParentStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PARENT_STATUS", MstEntity.ParentStatus));

                if (MstEntity.IntakeHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_HRS", MstEntity.IntakeHrs));

                if (MstEntity.IntakeMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_MNS", MstEntity.IntakeMns));

                if (MstEntity.IntakeScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_SCS", MstEntity.IntakeScs));

                if (MstEntity.FinHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_HRS", MstEntity.FinHrs));

                if (MstEntity.FinMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_MNS", MstEntity.FinMns));

                if (MstEntity.FinScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_SCS", MstEntity.FinScs));

                if (MstEntity.SimHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_HRS", MstEntity.SimHrs));

                if (MstEntity.SimMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_MNS", MstEntity.SimMns));

                if (MstEntity.SimScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_SCS", MstEntity.SimScs));

                if (MstEntity.MedHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_HRS", MstEntity.MedHrs));

                if (MstEntity.MedMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_MNS", MstEntity.MedMns));

                if (MstEntity.MedScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_SCS", MstEntity.MedScs));

                if (MstEntity.TownShip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOWNSHIP", MstEntity.TownShip));

                if (MstEntity.Rank1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK1", MstEntity.Rank1));

                if (MstEntity.Rank2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK2", MstEntity.Rank2));

                if (MstEntity.Rank3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK3", MstEntity.Rank3));

                if (MstEntity.Rank4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK4", MstEntity.Rank4));

                if (MstEntity.Rank5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK5", MstEntity.Rank5));

                if (MstEntity.Rank6 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK6", MstEntity.Rank6));

                if (MstEntity.Position1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION1", MstEntity.Position1));

                if (MstEntity.Position2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION2", MstEntity.Position2));

                if (MstEntity.Position3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION3", MstEntity.Position3));

                if (MstEntity.IntakeTime1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_TIME1", MstEntity.IntakeTime1));

                if (MstEntity.SsnFlag != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN_FLAG", MstEntity.SsnFlag));

                if (MstEntity.StateCase != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE_CASE", MstEntity.StateCase));

                if (MstEntity.Verifier != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFIER", MstEntity.Verifier));

                if (MstEntity.EligDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ELIG_DATE", MstEntity.EligDate));

                if (MstEntity.CatElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CAT_ELIG", MstEntity.CatElig));

                if (MstEntity.MealElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MEAL_ELIG", MstEntity.MealElig));

                if (MstEntity.VerifyW2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_W2", MstEntity.VerifyW2));

                if (MstEntity.VerifyCheckStub != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_CHECK_STUB", MstEntity.VerifyCheckStub));

                if (MstEntity.VerifyTaxReturn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_TAX_RETURN", MstEntity.VerifyTaxReturn));

                if (MstEntity.VerifyLetter != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_LETTER", MstEntity.VerifyLetter));

                if (MstEntity.VerifyOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTHER", MstEntity.VerifyOther));

                if (MstEntity.ReverifyDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_REVERIFY_DATE", MstEntity.ReverifyDate));

                if (MstEntity.IncomeTypes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INCOME_TYPES", MstEntity.IncomeTypes));

                if (MstEntity.Poverty != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POVERTY", MstEntity.Poverty));

                if (MstEntity.WaitList != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_WAIT_LIST", MstEntity.WaitList));

                if (MstEntity.ActiveStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ACTIVE_STATUS", MstEntity.ActiveStatus));

                if (MstEntity.TotalRank != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOTAL_RANK", MstEntity.TotalRank));

                if (MstEntity.NoInhh != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INHH", MstEntity.NoInhh));

                if (MstEntity.FamIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAM_INCOME", MstEntity.FamIncome));

                if (MstEntity.NoInProg != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INPROG", MstEntity.NoInProg));

                if (MstEntity.ProgIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PROG_INCOME", MstEntity.ProgIncome));

                if (MstEntity.OutOfService != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OUT_OF_SERVICE", MstEntity.OutOfService));

                if (MstEntity.Hud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HUD", MstEntity.Hud));

                if (MstEntity.Smi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SMI", MstEntity.Smi));

                if (MstEntity.Cmi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CMI", MstEntity.Cmi));

                if (MstEntity.County != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_COUNTY", MstEntity.County));

                if (MstEntity.AddressYears != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADDRESS_YEARS", MstEntity.AddressYears));

                if (MstEntity.MessagePhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MESSAGE_PHONE", MstEntity.MessagePhone));

                if (MstEntity.CellPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CELL_PHONE", MstEntity.CellPhone));

                if (MstEntity.FaxNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAX_NUMBER", MstEntity.FaxNumber));

                if (MstEntity.TtyNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TTY_NUMBER", MstEntity.TtyNumber));

                if (MstEntity.Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EMAIL", MstEntity.Email));

                if (MstEntity.BestContact != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_BEST_CONTACT", MstEntity.BestContact));

                if (MstEntity.AboutUs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ABOUT_US", MstEntity.AboutUs));

                if (MstEntity.ImportDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_IMPORT_DATE", MstEntity.ImportDate));

                if (MstEntity.DateAdded != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DATE_ADDED", MstEntity.DateAdded));

                if (MstEntity.ExpRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_RENT", MstEntity.ExpRent));

                if (MstEntity.ExpWater != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_WATER", MstEntity.ExpWater));

                if (MstEntity.ExpElectric != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_ELECTRIC", MstEntity.ExpElectric));

                if (MstEntity.ExpHeat != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_HEAT", MstEntity.ExpHeat));

                if (MstEntity.ExpMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_MISC", MstEntity.ExpMisc));

                if (MstEntity.Debtcc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_CC", MstEntity.Debtcc));

                if (MstEntity.DebtLoans != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_LOANS", MstEntity.DebtLoans));

                if (MstEntity.DebtMed != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MED", MstEntity.DebtMed));

                if (MstEntity.DebtOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_OTH", MstEntity.DebtOther));

                if (MstEntity.DebtMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MISC", MstEntity.DebtMisc));

                if (MstEntity.DebtTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_TOTAL", MstEntity.DebtTotal));

                if (MstEntity.AsetPhy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_PHY", MstEntity.AsetPhy));

                if (MstEntity.AsetLiq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_LIQ", MstEntity.AsetLiq));

                if (MstEntity.AsetOth != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_OTH", MstEntity.AsetOth));

                if (MstEntity.AsetTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_TOTAL", MstEntity.AsetTotal));

                if (MstEntity.AsetMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_MISC", MstEntity.AsetMisc));

                if (MstEntity.AsetRatio != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_ASET_RATIO", MstEntity.AsetRatio));

                if (MstEntity.DebIncmRation != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_INCM_RATIO", MstEntity.DebIncmRation));

                if (MstEntity.ExpTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_TOTAL", MstEntity.ExpTotal));

                if (MstEntity.ExpLivexpense != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_LIVEXPENSE", MstEntity.ExpLivexpense));

                if (MstEntity.ExpCaseWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_CASEWORKER", MstEntity.ExpCaseWorker));

                if (MstEntity.Dwelling != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DWELLING", MstEntity.Dwelling));

                if (MstEntity.HeatIncRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HEAT_INC_RENT", MstEntity.HeatIncRent));

                if (MstEntity.Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SOURCE", MstEntity.Source));

                if (MstEntity.RollOver != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ROLLOVER", MstEntity.RollOver));

                if (MstEntity.RiskValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RISK_VALUE", MstEntity.RiskValue));

                if (MstEntity.SubShouse != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSHOUSE", MstEntity.SubShouse));

                if (MstEntity.SubStype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSTYPE", MstEntity.SubStype));

                if (MstEntity.VerFund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VER_FUND", MstEntity.VerFund));

                if (MstEntity.OmbScreen != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OMB_SCREEN", MstEntity.OmbScreen));

                if (MstEntity.CbCaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_CASE_MANAGER", MstEntity.CbCaseManager));

                if (MstEntity.CaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_MANAGER", MstEntity.CaseManager));

                if (MstEntity.VerifyOthCmb != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTH_CMB", MstEntity.VerifyOthCmb));

                if (MstEntity.SimPrint != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT", MstEntity.SimPrint));

                if (MstEntity.SimPrintDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT_DATE", MstEntity.SimPrintDate));

                if (MstEntity.CbFraud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_FRAUD", MstEntity.CbFraud));

                if (MstEntity.FraudDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FRAUD_DATE", MstEntity.FraudDate));

                if (MstEntity.AddOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_1", MstEntity.AddOperator1));

                if (MstEntity.LstcOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_1", MstEntity.LstcOperator1));

                if (MstEntity.TimesUpdated1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_1", MstEntity.TimesUpdated1));

                if (MstEntity.AddOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_2", MstEntity.AddOperator2));

                if (MstEntity.LstcOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_2", MstEntity.LstcOperator2));

                if (MstEntity.TimesUpdated2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_2", MstEntity.TimesUpdated2));

                if (MstEntity.AddOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_3", MstEntity.AddOperator3));

                if (MstEntity.LstcOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_3", MstEntity.LstcOperator3));

                if (MstEntity.TimesUpdated3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_3", MstEntity.TimesUpdated3));

                if (MstEntity.AddOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_4", MstEntity.AddOperator4));

                if (MstEntity.LstcOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_4", MstEntity.LstcOperator4));

                if (MstEntity.TimesUpdated4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_4", MstEntity.TimesUpdated4));

                //if (MstEntity.PJob != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_JOB", MstEntity.PJob));

                //if (MstEntity.PHSD != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HSD", MstEntity.PHSD));

                //if (MstEntity.PSkills != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_SKILLS", MstEntity.PSkills));
                //if (MstEntity.PHousing != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HOUSING", MstEntity.PHousing));
                //if (MstEntity.PTransport != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_TRANSPORT", MstEntity.PTransport));
                //if (MstEntity.PChldCare != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHLDCARE", MstEntity.PChldCare));
                //if (MstEntity.PCCENRL != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CCENRL", MstEntity.PCCENRL));
                //if (MstEntity.PELDCARE != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ELDRCARE", MstEntity.PELDCARE));
                //if (MstEntity.PECNEED != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ECNEED", MstEntity.PECNEED));

                //if (MstEntity.PECHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHINS", MstEntity.PECHINS));

                //if (MstEntity.PAHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_AHINS", MstEntity.PAHINS));

                //if (MstEntity.PRWENG != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RW_ENG", MstEntity.PRWENG));

                //if (MstEntity.PCURRDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CURR_DSS", MstEntity.PCURRDSS));

                //if (MstEntity.PRECVDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RECV_DSS", MstEntity.PRECVDSS));

                if (MstEntity.LPM0001 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0001", MstEntity.LPM0001));

                if (MstEntity.LPM0002 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0002", MstEntity.LPM0002));

                if (MstEntity.LPM0003 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0003", MstEntity.LPM0003));

                if (MstEntity.LPM0004 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0004", MstEntity.LPM0004));

                if (MstEntity.LPM0005 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0005", MstEntity.LPM0005));

                if (MstEntity.LPM0006 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0006", MstEntity.LPM0006));

                if (MstEntity.LPM0007 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0007", MstEntity.LPM0007));

                if (MstEntity.LPM0008 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0008", MstEntity.LPM0008));

                if (MstEntity.LPM0009 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0009", MstEntity.LPM0009));

                if (MstEntity.LPM0010 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0010", MstEntity.LPM0010));

                if (MstEntity.LPM0011 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0011", MstEntity.LPM0011));

                if (MstEntity.ApplictionType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APPLICANT_TYPE", MstEntity.ApplictionType));

                if (MstEntity.ApplictionDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APPLICANT_DATE", MstEntity.ApplictionDate));


                sqlParamList.Add(new SqlParameter("@Mode", MstEntity.Mode));

                if (MstEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", MstEntity.Type));

                if (!string.IsNullOrEmpty(MstEntity.ModuleCode.Trim()))
                {
                    sqlParamList.Add(new SqlParameter("@ModuleCode", MstEntity.ModuleCode));
                }

                SqlParameter sqlClientId = new SqlParameter("@MST_ClientId_OUT", SqlDbType.VarChar, 10);
                sqlClientId.Value = MstEntity.ClientId;
                sqlClientId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlClientId);
                SqlParameter sqlFamilyId = new SqlParameter("@MST_FamilyId_OUT", SqlDbType.VarChar, 10);
                sqlFamilyId.Value = MstEntity.FamilyId;
                sqlFamilyId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlFamilyId);
                SqlParameter sqlMSTSSNO = new SqlParameter("@MST_SSNNO_OUT", SqlDbType.VarChar, 10);
                sqlMSTSSNO.Value = MstEntity.FamilyId;
                sqlMSTSSNO.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlMSTSSNO);

                boolSuccess = CaseMst.InsertUpdateCASEMST(sqlParamList);
                strNewApplNo = sqlApplNo.Value.ToString();
                strNewClientId = sqlClientId.Value.ToString();
                strNewFamilyId = sqlFamilyId.Value.ToString();
                strNewSSNO = sqlMSTSSNO.Value.ToString();

                if (boolSuccess == false)
                {
                    CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), "Recore not modified Some error", MstEntity.LstcOperator1);
                    //CaseMst.InsertErrorLog("Case2001SNPAPPLICANTERROR", ErrorLogSNP(SnpAppEntity), "Recore not modified Some error");
                }



            }
            catch (Exception ex)
            {

                boolSuccess = false;
                strMessage = ex.Message;

                CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), strMessage, MstEntity.LstcOperator1);

            }


            strApplNo = strNewApplNo;
            strClientId = strNewClientId;
            strFamilyId = strNewFamilyId;
            strSSNO = strNewSSNO;
            strMSG = strMessage;
            return boolSuccess;
        }

        public bool InsertUpdateCaseMst(CaseMstEntity MstEntity, out string strApplNo, out string strClientId, out string strFamilyId, out string strSSNO, out string strMSG, CaseSnpEntity SnpAppEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            string strNewClientId = string.Empty;
            string strNewFamilyId = string.Empty;
            string strNewSSNO = string.Empty;
            string strMessage = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", MstEntity.ApplAgency));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", MstEntity.ApplDept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", MstEntity.ApplProgram));
                if (MstEntity.ApplYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_YEAR", MstEntity.ApplYr));
                sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                SqlParameter sqlApplNo = new SqlParameter("@MST_APP_NO_OUT", SqlDbType.VarChar, 10);
                sqlApplNo.Value = MstEntity.ApplNo;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);


                if (MstEntity.FamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_SEQ", MstEntity.FamilySeq));

                if (MstEntity.FamilyId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_ID", MstEntity.FamilyId));

                if (MstEntity.ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLIENT_ID", MstEntity.ClientId));

                if (MstEntity.Ssn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN", MstEntity.Ssn));
                sqlParamList.Add(new SqlParameter("@MST_BIC", MstEntity.Bic));
                if (MstEntity.NickName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NICKNAME", MstEntity.NickName));

                if (MstEntity.EthnicOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ETHNIC_OTHER", MstEntity.EthnicOther));

                if (MstEntity.State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE", MstEntity.State));

                if (MstEntity.City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CITY", MstEntity.City));

                if (MstEntity.Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STREET", MstEntity.Street));

                if (MstEntity.Suffix != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUFFIX", MstEntity.Suffix));

                if (MstEntity.Hn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HN", MstEntity.Hn));

                if (MstEntity.Direction != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DIRECTION", MstEntity.Direction));

                if (MstEntity.Apt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APT", MstEntity.Apt));

                if (MstEntity.Flr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FLR", MstEntity.Flr));

                if (MstEntity.Zip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIP", MstEntity.Zip));

                if (MstEntity.Zipplus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIPPLUS", MstEntity.Zipplus));

                if (MstEntity.Precinct != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PRECINCT", MstEntity.Precinct));

                if (MstEntity.Area != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_AREA", MstEntity.Area));

                if (MstEntity.Phone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PHONE", MstEntity.Phone));

                if (MstEntity.NextYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NEXTYEAR", MstEntity.NextYear));

                if (MstEntity.Classification != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLASSIFICATION", MstEntity.Classification));

                if (MstEntity.Language != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE", MstEntity.Language));

                if (MstEntity.LanguageOt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE_OT", MstEntity.LanguageOt));

                if (MstEntity.IntakeWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_WORKER", MstEntity.IntakeWorker));

                if (MstEntity.IntakeDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_DATE", MstEntity.IntakeDate));

                if (MstEntity.InitialDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INITIAL_DATE", MstEntity.InitialDate));

                if (MstEntity.CaseType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_TYPE", MstEntity.CaseType));

                if (MstEntity.Housing != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HOUSING", MstEntity.Housing));

                if (MstEntity.FamilyType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_TYPE", MstEntity.FamilyType));

                if (MstEntity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SITE", MstEntity.Site));

                if (MstEntity.Juvenile != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_JUVENILE", MstEntity.Juvenile));

                if (MstEntity.Senior != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SENIOR", MstEntity.Senior));

                if (MstEntity.Secret != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SECRET", MstEntity.Secret));

                if (MstEntity.CaseReviewDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_REVIEW_DATE", MstEntity.CaseReviewDate));

                if (MstEntity.CompleteDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_COMPLETE_DATE", MstEntity.CompleteDate));


                if (MstEntity.AlertCodes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ALERT_CODES", MstEntity.AlertCodes));

                if (MstEntity.ParentStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PARENT_STATUS", MstEntity.ParentStatus));

                if (MstEntity.IntakeHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_HRS", MstEntity.IntakeHrs));

                if (MstEntity.IntakeMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_MNS", MstEntity.IntakeMns));

                if (MstEntity.IntakeScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_SCS", MstEntity.IntakeScs));

                if (MstEntity.FinHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_HRS", MstEntity.FinHrs));

                if (MstEntity.FinMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_MNS", MstEntity.FinMns));

                if (MstEntity.FinScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_SCS", MstEntity.FinScs));

                if (MstEntity.SimHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_HRS", MstEntity.SimHrs));

                if (MstEntity.SimMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_MNS", MstEntity.SimMns));

                if (MstEntity.SimScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_SCS", MstEntity.SimScs));

                if (MstEntity.MedHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_HRS", MstEntity.MedHrs));

                if (MstEntity.MedMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_MNS", MstEntity.MedMns));

                if (MstEntity.MedScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_SCS", MstEntity.MedScs));

                if (MstEntity.TownShip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOWNSHIP", MstEntity.TownShip));

                if (MstEntity.Rank1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK1", MstEntity.Rank1));

                if (MstEntity.Rank2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK2", MstEntity.Rank2));

                if (MstEntity.Rank3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK3", MstEntity.Rank3));

                if (MstEntity.Rank4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK4", MstEntity.Rank4));

                if (MstEntity.Rank5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK5", MstEntity.Rank5));

                if (MstEntity.Rank6 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK6", MstEntity.Rank6));

                if (MstEntity.Position1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION1", MstEntity.Position1));

                if (MstEntity.Position2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION2", MstEntity.Position2));

                if (MstEntity.Position3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION3", MstEntity.Position3));

                if (MstEntity.IntakeTime1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_TIME1", MstEntity.IntakeTime1));

                if (MstEntity.SsnFlag != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN_FLAG", MstEntity.SsnFlag));

                if (MstEntity.StateCase != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE_CASE", MstEntity.StateCase));

                if (MstEntity.Verifier != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFIER", MstEntity.Verifier));

                if (MstEntity.EligDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ELIG_DATE", MstEntity.EligDate));

                if (MstEntity.CatElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CAT_ELIG", MstEntity.CatElig));

                if (MstEntity.MealElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MEAL_ELIG", MstEntity.MealElig));

                if (MstEntity.VerifyW2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_W2", MstEntity.VerifyW2));

                if (MstEntity.VerifyCheckStub != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_CHECK_STUB", MstEntity.VerifyCheckStub));

                if (MstEntity.VerifyTaxReturn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_TAX_RETURN", MstEntity.VerifyTaxReturn));

                if (MstEntity.VerifyLetter != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_LETTER", MstEntity.VerifyLetter));

                if (MstEntity.VerifyOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTHER", MstEntity.VerifyOther));

                if (MstEntity.ReverifyDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_REVERIFY_DATE", MstEntity.ReverifyDate));

                if (MstEntity.IncomeTypes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INCOME_TYPES", MstEntity.IncomeTypes));

                if (MstEntity.Poverty != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POVERTY", MstEntity.Poverty));

                if (MstEntity.WaitList != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_WAIT_LIST", MstEntity.WaitList));

                if (MstEntity.ActiveStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ACTIVE_STATUS", MstEntity.ActiveStatus));

                if (MstEntity.TotalRank != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOTAL_RANK", MstEntity.TotalRank));

                if (MstEntity.NoInhh != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INHH", MstEntity.NoInhh));

                if (MstEntity.FamIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAM_INCOME", MstEntity.FamIncome));

                if (MstEntity.NoInProg != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INPROG", MstEntity.NoInProg));

                if (MstEntity.ProgIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PROG_INCOME", MstEntity.ProgIncome));

                if (MstEntity.OutOfService != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OUT_OF_SERVICE", MstEntity.OutOfService));

                if (MstEntity.Hud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HUD", MstEntity.Hud));

                if (MstEntity.Smi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SMI", MstEntity.Smi));

                if (MstEntity.Cmi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CMI", MstEntity.Cmi));

                if (MstEntity.County != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_COUNTY", MstEntity.County));

                if (MstEntity.AddressYears != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADDRESS_YEARS", MstEntity.AddressYears));

                if (MstEntity.MessagePhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MESSAGE_PHONE", MstEntity.MessagePhone));

                if (MstEntity.CellPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CELL_PHONE", MstEntity.CellPhone));

                if (MstEntity.FaxNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAX_NUMBER", MstEntity.FaxNumber));

                if (MstEntity.TtyNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TTY_NUMBER", MstEntity.TtyNumber));

                if (MstEntity.Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EMAIL", MstEntity.Email));

                if (MstEntity.BestContact != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_BEST_CONTACT", MstEntity.BestContact));

                if (MstEntity.AboutUs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ABOUT_US", MstEntity.AboutUs));

                if (MstEntity.ImportDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_IMPORT_DATE", MstEntity.ImportDate));

                if (MstEntity.DateAdded != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DATE_ADDED", MstEntity.DateAdded));

                if (MstEntity.ExpRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_RENT", MstEntity.ExpRent));

                if (MstEntity.ExpWater != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_WATER", MstEntity.ExpWater));

                if (MstEntity.ExpElectric != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_ELECTRIC", MstEntity.ExpElectric));

                if (MstEntity.ExpHeat != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_HEAT", MstEntity.ExpHeat));

                if (MstEntity.ExpMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_MISC", MstEntity.ExpMisc));

                if (MstEntity.Debtcc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_CC", MstEntity.Debtcc));

                if (MstEntity.DebtLoans != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_LOANS", MstEntity.DebtLoans));

                if (MstEntity.DebtMed != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MED", MstEntity.DebtMed));

                if (MstEntity.DebtOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_OTH", MstEntity.DebtOther));

                if (MstEntity.DebtMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MISC", MstEntity.DebtMisc));

                if (MstEntity.DebtTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_TOTAL", MstEntity.DebtTotal));

                if (MstEntity.AsetPhy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_PHY", MstEntity.AsetPhy));

                if (MstEntity.AsetLiq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_LIQ", MstEntity.AsetLiq));

                if (MstEntity.AsetOth != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_OTH", MstEntity.AsetOth));

                if (MstEntity.AsetTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_TOTAL", MstEntity.AsetTotal));

                if (MstEntity.AsetMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_MISC", MstEntity.AsetMisc));

                if (MstEntity.AsetRatio != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_ASET_RATIO", MstEntity.AsetRatio));

                if (MstEntity.DebIncmRation != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_INCM_RATIO", MstEntity.DebIncmRation));

                if (MstEntity.ExpTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_TOTAL", MstEntity.ExpTotal));

                if (MstEntity.ExpLivexpense != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_LIVEXPENSE", MstEntity.ExpLivexpense));

                if (MstEntity.ExpCaseWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_CASEWORKER", MstEntity.ExpCaseWorker));

                if (MstEntity.Dwelling != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DWELLING", MstEntity.Dwelling));

                if (MstEntity.HeatIncRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HEAT_INC_RENT", MstEntity.HeatIncRent));

                if (MstEntity.Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SOURCE", MstEntity.Source));

                if (MstEntity.RollOver != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ROLLOVER", MstEntity.RollOver));

                if (MstEntity.RiskValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RISK_VALUE", MstEntity.RiskValue));

                if (MstEntity.SubShouse != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSHOUSE", MstEntity.SubShouse));

                if (MstEntity.SubStype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSTYPE", MstEntity.SubStype));

                if (MstEntity.VerFund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VER_FUND", MstEntity.VerFund));

                if (MstEntity.OmbScreen != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OMB_SCREEN", MstEntity.OmbScreen));

                if (MstEntity.CbCaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_CASE_MANAGER", MstEntity.CbCaseManager));

                if (MstEntity.CaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_MANAGER", MstEntity.CaseManager));

                if (MstEntity.VerifyOthCmb != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTH_CMB", MstEntity.VerifyOthCmb));

                if (MstEntity.SimPrint != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT", MstEntity.SimPrint));

                if (MstEntity.SimPrintDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT_DATE", MstEntity.SimPrintDate));

                if (MstEntity.CbFraud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_FRAUD", MstEntity.CbFraud));

                if (MstEntity.FraudDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FRAUD_DATE", MstEntity.FraudDate));

                if (MstEntity.AddOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_1", MstEntity.AddOperator1));

                if (MstEntity.LstcOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_1", MstEntity.LstcOperator1));

                if (MstEntity.TimesUpdated1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_1", MstEntity.TimesUpdated1));

                if (MstEntity.AddOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_2", MstEntity.AddOperator2));

                if (MstEntity.LstcOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_2", MstEntity.LstcOperator2));

                if (MstEntity.TimesUpdated2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_2", MstEntity.TimesUpdated2));

                if (MstEntity.AddOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_3", MstEntity.AddOperator3));

                if (MstEntity.LstcOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_3", MstEntity.LstcOperator3));

                if (MstEntity.TimesUpdated3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_3", MstEntity.TimesUpdated3));

                if (MstEntity.AddOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_4", MstEntity.AddOperator4));

                if (MstEntity.LstcOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_4", MstEntity.LstcOperator4));

                if (MstEntity.TimesUpdated4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_4", MstEntity.TimesUpdated4));

                //if (MstEntity.PJob != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_JOB", MstEntity.PJob));

                //if (MstEntity.PHSD != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HSD", MstEntity.PHSD));

                //if (MstEntity.PSkills != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_SKILLS", MstEntity.PSkills));
                //if (MstEntity.PHousing != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HOUSING", MstEntity.PHousing));
                //if (MstEntity.PTransport != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_TRANSPORT", MstEntity.PTransport));
                //if (MstEntity.PChldCare != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHLDCARE", MstEntity.PChldCare));
                //if (MstEntity.PCCENRL != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CCENRL", MstEntity.PCCENRL));
                //if (MstEntity.PELDCARE != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ELDRCARE", MstEntity.PELDCARE));
                //if (MstEntity.PECNEED != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ECNEED", MstEntity.PECNEED));

                //if (MstEntity.PECHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHINS", MstEntity.PECHINS));

                //if (MstEntity.PAHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_AHINS", MstEntity.PAHINS));

                //if (MstEntity.PRWENG != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RW_ENG", MstEntity.PRWENG));

                //if (MstEntity.PCURRDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CURR_DSS", MstEntity.PCURRDSS));

                //if (MstEntity.PRECVDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RECV_DSS", MstEntity.PRECVDSS));

                if (MstEntity.LPM0001 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0001", MstEntity.LPM0001));

                if (MstEntity.LPM0002 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0002", MstEntity.LPM0002));

                if (MstEntity.LPM0003 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0003", MstEntity.LPM0003));

                if (MstEntity.LPM0004 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0004", MstEntity.LPM0004));

                if (MstEntity.LPM0005 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0005", MstEntity.LPM0005));

                if (MstEntity.LPM0006 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0006", MstEntity.LPM0006));

                if (MstEntity.LPM0007 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0007", MstEntity.LPM0007));

                if (MstEntity.LPM0008 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0008", MstEntity.LPM0008));

                if (MstEntity.LPM0009 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0009", MstEntity.LPM0009));

                if (MstEntity.LPM0010 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0010", MstEntity.LPM0010));

                if (MstEntity.LPM0011 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0011", MstEntity.LPM0011));

                if (MstEntity.LPM0012 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0012", MstEntity.LPM0012));

                if (MstEntity.LPM0013 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0013", MstEntity.LPM0013));

                if (MstEntity.LPM0014 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0014", MstEntity.LPM0014));

                if (MstEntity.LPM0015 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0015", MstEntity.LPM0015));

                if (MstEntity.LPM0016 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0016", MstEntity.LPM0016));

                if (MstEntity.LPM0017 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0017", MstEntity.LPM0017));

                if (MstEntity.ApplictionType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APPLICANT_TYPE", MstEntity.ApplictionType));

                if (MstEntity.ApplictionDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APPLICANT_DATE", MstEntity.ApplictionDate));


                if (MstEntity.HomePhone_NA != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HOME_NA", MstEntity.HomePhone_NA));

                if (MstEntity.CellPhone_NA != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CELL_NA", MstEntity.CellPhone_NA));

                if (MstEntity.MessagePhone_NA != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MESSAGE_NA", MstEntity.MessagePhone_NA));

                if (MstEntity.Email_NA != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EMAIL_NA", MstEntity.Email_NA));


                sqlParamList.Add(new SqlParameter("@Mode", MstEntity.Mode));

                if (MstEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", MstEntity.Type));

                if (!string.IsNullOrEmpty(MstEntity.ModuleCode.Trim()))
                {
                    sqlParamList.Add(new SqlParameter("@ModuleCode", MstEntity.ModuleCode));
                }

                SqlParameter sqlClientId = new SqlParameter("@MST_ClientId_OUT", SqlDbType.VarChar, 10);
                sqlClientId.Value = MstEntity.ClientId;
                sqlClientId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlClientId);
                SqlParameter sqlFamilyId = new SqlParameter("@MST_FamilyId_OUT", SqlDbType.VarChar, 10);
                sqlFamilyId.Value = MstEntity.FamilyId;
                sqlFamilyId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlFamilyId);
                SqlParameter sqlMSTSSNO = new SqlParameter("@MST_SSNNO_OUT", SqlDbType.VarChar, 10);
                sqlMSTSSNO.Value = MstEntity.FamilyId;
                sqlMSTSSNO.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlMSTSSNO);

                boolSuccess = CaseMst.InsertUpdateCASEMST(sqlParamList);
                strNewApplNo = sqlApplNo.Value.ToString();
                strNewClientId = sqlClientId.Value.ToString();
                strNewFamilyId = sqlFamilyId.Value.ToString();
                strNewSSNO = sqlMSTSSNO.Value.ToString();

                if (boolSuccess == false)
                {
                    CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), "Record not modified Some error",MstEntity.LstcOperator1);
                    if (SnpAppEntity.Mode == Consts.Common.Add)
                    {
                        SnpAppEntity.App = strNewApplNo;
                        SnpAppEntity.ClientId = strNewClientId;
                        if (strNewSSNO != string.Empty)
                            SnpAppEntity.Ssno = strNewSSNO;

                        CaseMst.InsertErrorLog("Case2001SNPAPPLICANTINSERTERROR", ErrorLogSNP(SnpAppEntity), "Applicant record not inserted some error", MstEntity.LstcOperator1);
                    }
                    else
                    {
                        CaseMst.InsertErrorLog("Case2001SNPAPPLICANTUPDATEERROR", ErrorLogSNP(SnpAppEntity), "Applicant record not updated some error", MstEntity.LstcOperator1);
                    }

                }

            }
            catch (Exception ex)
            {
                boolSuccess = false;
                strMessage = ex.Message;
                CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), strMessage, MstEntity.LstcOperator1);
                CaseMst.InsertErrorLog("Case2001SNPAPPLICANTINSERTERROR", ErrorLogSNP(SnpAppEntity), strMessage, MstEntity.LstcOperator1);
            }


            strApplNo = strNewApplNo;
            strClientId = strNewClientId;
            strFamilyId = strNewFamilyId;
            strSSNO = strNewSSNO;
            strMSG = strMessage;
            return boolSuccess;
        }


        public bool InsertUpdateCaseMstLeanIntake(CaseMstEntity MstEntity, out string strApplNo, out string strClientId, out string strFamilyId, out string strSSNO, out string strMSG, CaseSnpEntity SnpAppEntity,string strFName,string strDob)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            string strNewClientId = string.Empty;
            string strNewFamilyId = string.Empty;
            string strNewSSNO = string.Empty;
            string strMessage = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", MstEntity.ApplAgency));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", MstEntity.ApplDept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", MstEntity.ApplProgram));
                if (MstEntity.ApplYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_YEAR", MstEntity.ApplYr));
                sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                SqlParameter sqlApplNo = new SqlParameter("@MST_APP_NO_OUT", SqlDbType.VarChar, 10);
                sqlApplNo.Value = MstEntity.ApplNo;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);


                if (MstEntity.FamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_SEQ", MstEntity.FamilySeq));

                if (MstEntity.FamilyId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_ID", MstEntity.FamilyId));

                if (MstEntity.ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLIENT_ID", MstEntity.ClientId));

                if (MstEntity.Ssn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN", MstEntity.Ssn));
                sqlParamList.Add(new SqlParameter("@MST_BIC", MstEntity.Bic));
                if (MstEntity.NickName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NICKNAME", MstEntity.NickName));

                if (MstEntity.EthnicOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ETHNIC_OTHER", MstEntity.EthnicOther));

                if (MstEntity.State != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE", MstEntity.State));

                if (MstEntity.City != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CITY", MstEntity.City));

                if (MstEntity.Street != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STREET", MstEntity.Street));

                if (MstEntity.Suffix != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUFFIX", MstEntity.Suffix));

                if (MstEntity.Hn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HN", MstEntity.Hn));

                if (MstEntity.Direction != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DIRECTION", MstEntity.Direction));

                if (MstEntity.Apt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APT", MstEntity.Apt));

                if (MstEntity.Flr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FLR", MstEntity.Flr));

                if (MstEntity.Zip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIP", MstEntity.Zip));

                if (MstEntity.Zipplus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ZIPPLUS", MstEntity.Zipplus));

                if (MstEntity.Precinct != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PRECINCT", MstEntity.Precinct));

                if (MstEntity.Area != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_AREA", MstEntity.Area));

                if (MstEntity.Phone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PHONE", MstEntity.Phone));

                if (MstEntity.NextYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NEXTYEAR", MstEntity.NextYear));

                if (MstEntity.Classification != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CLASSIFICATION", MstEntity.Classification));

                if (MstEntity.Language != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE", MstEntity.Language));

                if (MstEntity.LanguageOt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LANGUAGE_OT", MstEntity.LanguageOt));

                if (MstEntity.IntakeWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_WORKER", MstEntity.IntakeWorker));

                if (MstEntity.IntakeDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_DATE", MstEntity.IntakeDate));

                if (MstEntity.InitialDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INITIAL_DATE", MstEntity.InitialDate));

                if (MstEntity.CaseType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_TYPE", MstEntity.CaseType));

                if (MstEntity.Housing != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HOUSING", MstEntity.Housing));

                if (MstEntity.FamilyType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAMILY_TYPE", MstEntity.FamilyType));

                if (MstEntity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SITE", MstEntity.Site));

                if (MstEntity.Juvenile != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_JUVENILE", MstEntity.Juvenile));

                if (MstEntity.Senior != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SENIOR", MstEntity.Senior));

                if (MstEntity.Secret != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SECRET", MstEntity.Secret));

                if (MstEntity.CaseReviewDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_REVIEW_DATE", MstEntity.CaseReviewDate));

                if (MstEntity.AlertCodes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ALERT_CODES", MstEntity.AlertCodes));

                if (MstEntity.ParentStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PARENT_STATUS", MstEntity.ParentStatus));

                if (MstEntity.IntakeHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_HRS", MstEntity.IntakeHrs));

                if (MstEntity.IntakeMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_MNS", MstEntity.IntakeMns));

                if (MstEntity.IntakeScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_SCS", MstEntity.IntakeScs));

                if (MstEntity.FinHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_HRS", MstEntity.FinHrs));

                if (MstEntity.FinMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_MNS", MstEntity.FinMns));

                if (MstEntity.FinScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FIN_SCS", MstEntity.FinScs));

                if (MstEntity.SimHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_HRS", MstEntity.SimHrs));

                if (MstEntity.SimMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_MNS", MstEntity.SimMns));

                if (MstEntity.SimScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_SCS", MstEntity.SimScs));

                if (MstEntity.MedHrs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_HRS", MstEntity.MedHrs));

                if (MstEntity.MedMns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_MNS", MstEntity.MedMns));

                if (MstEntity.MedScs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MED_SCS", MstEntity.MedScs));

                if (MstEntity.TownShip != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOWNSHIP", MstEntity.TownShip));

                if (MstEntity.Rank1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK1", MstEntity.Rank1));

                if (MstEntity.Rank2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK2", MstEntity.Rank2));

                if (MstEntity.Rank3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK3", MstEntity.Rank3));

                if (MstEntity.Rank4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK4", MstEntity.Rank4));

                if (MstEntity.Rank5 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK5", MstEntity.Rank5));

                if (MstEntity.Rank6 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RANK6", MstEntity.Rank6));

                if (MstEntity.Position1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION1", MstEntity.Position1));

                if (MstEntity.Position2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION2", MstEntity.Position2));

                if (MstEntity.Position3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POSITION3", MstEntity.Position3));

                if (MstEntity.IntakeTime1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INTAKE_TIME1", MstEntity.IntakeTime1));

                if (MstEntity.SsnFlag != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SSN_FLAG", MstEntity.SsnFlag));

                if (MstEntity.StateCase != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_STATE_CASE", MstEntity.StateCase));

                if (MstEntity.Verifier != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFIER", MstEntity.Verifier));

                if (MstEntity.EligDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ELIG_DATE", MstEntity.EligDate));

                if (MstEntity.CatElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CAT_ELIG", MstEntity.CatElig));

                if (MstEntity.MealElig != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MEAL_ELIG", MstEntity.MealElig));

                if (MstEntity.VerifyW2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_W2", MstEntity.VerifyW2));

                if (MstEntity.VerifyCheckStub != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_CHECK_STUB", MstEntity.VerifyCheckStub));

                if (MstEntity.VerifyTaxReturn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_TAX_RETURN", MstEntity.VerifyTaxReturn));

                if (MstEntity.VerifyLetter != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_LETTER", MstEntity.VerifyLetter));

                if (MstEntity.VerifyOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTHER", MstEntity.VerifyOther));

                if (MstEntity.ReverifyDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_REVERIFY_DATE", MstEntity.ReverifyDate));

                if (MstEntity.IncomeTypes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_INCOME_TYPES", MstEntity.IncomeTypes));

                if (MstEntity.Poverty != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_POVERTY", MstEntity.Poverty));

                if (MstEntity.WaitList != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_WAIT_LIST", MstEntity.WaitList));

                if (MstEntity.ActiveStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ACTIVE_STATUS", MstEntity.ActiveStatus));

                if (MstEntity.TotalRank != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TOTAL_RANK", MstEntity.TotalRank));

                if (MstEntity.NoInhh != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INHH", MstEntity.NoInhh));

                if (MstEntity.FamIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAM_INCOME", MstEntity.FamIncome));

                if (MstEntity.NoInProg != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NO_INPROG", MstEntity.NoInProg));

                if (MstEntity.ProgIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_PROG_INCOME", MstEntity.ProgIncome));

                if (MstEntity.OutOfService != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OUT_OF_SERVICE", MstEntity.OutOfService));

                if (MstEntity.Hud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HUD", MstEntity.Hud));

                if (MstEntity.Smi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SMI", MstEntity.Smi));

                if (MstEntity.Cmi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CMI", MstEntity.Cmi));

                if (MstEntity.County != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_COUNTY", MstEntity.County));

                if (MstEntity.AddressYears != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADDRESS_YEARS", MstEntity.AddressYears));

                if (MstEntity.MessagePhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_MESSAGE_PHONE", MstEntity.MessagePhone));

                if (MstEntity.CellPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CELL_PHONE", MstEntity.CellPhone));

                if (MstEntity.FaxNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FAX_NUMBER", MstEntity.FaxNumber));

                if (MstEntity.TtyNumber != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TTY_NUMBER", MstEntity.TtyNumber));

                if (MstEntity.Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EMAIL", MstEntity.Email));

                if (MstEntity.BestContact != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_BEST_CONTACT", MstEntity.BestContact));

                if (MstEntity.AboutUs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ABOUT_US", MstEntity.AboutUs));

                if (MstEntity.ImportDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_IMPORT_DATE", MstEntity.ImportDate));

                if (MstEntity.DateAdded != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DATE_ADDED", MstEntity.DateAdded));

                if (MstEntity.ExpRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_RENT", MstEntity.ExpRent));

                if (MstEntity.ExpWater != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_WATER", MstEntity.ExpWater));

                if (MstEntity.ExpElectric != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_ELECTRIC", MstEntity.ExpElectric));

                if (MstEntity.ExpHeat != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_HEAT", MstEntity.ExpHeat));

                if (MstEntity.ExpMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_MISC", MstEntity.ExpMisc));

                if (MstEntity.Debtcc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_CC", MstEntity.Debtcc));

                if (MstEntity.DebtLoans != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_LOANS", MstEntity.DebtLoans));

                if (MstEntity.DebtMed != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MED", MstEntity.DebtMed));

                if (MstEntity.DebtOther != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_OTH", MstEntity.DebtOther));

                if (MstEntity.DebtMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_MISC", MstEntity.DebtMisc));

                if (MstEntity.DebtTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEBT_TOTAL", MstEntity.DebtTotal));

                if (MstEntity.AsetPhy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_PHY", MstEntity.AsetPhy));

                if (MstEntity.AsetLiq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_LIQ", MstEntity.AsetLiq));

                if (MstEntity.AsetOth != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_OTH", MstEntity.AsetOth));

                if (MstEntity.AsetTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_TOTAL", MstEntity.AsetTotal));

                if (MstEntity.AsetMisc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ASET_MISC", MstEntity.AsetMisc));

                if (MstEntity.AsetRatio != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_ASET_RATIO", MstEntity.AsetRatio));

                if (MstEntity.DebIncmRation != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DEB_INCM_RATIO", MstEntity.DebIncmRation));

                if (MstEntity.ExpTotal != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_TOTAL", MstEntity.ExpTotal));

                if (MstEntity.ExpLivexpense != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_LIVEXPENSE", MstEntity.ExpLivexpense));

                if (MstEntity.ExpCaseWorker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_EXP_CASEWORKER", MstEntity.ExpCaseWorker));

                if (MstEntity.Dwelling != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DWELLING", MstEntity.Dwelling));

                if (MstEntity.HeatIncRent != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_HEAT_INC_RENT", MstEntity.HeatIncRent));

                if (MstEntity.Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SOURCE", MstEntity.Source));

                if (MstEntity.RollOver != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ROLLOVER", MstEntity.RollOver));

                if (MstEntity.RiskValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_RISK_VALUE", MstEntity.RiskValue));

                if (MstEntity.SubShouse != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSHOUSE", MstEntity.SubShouse));

                if (MstEntity.SubStype != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SUBSTYPE", MstEntity.SubStype));

                if (MstEntity.VerFund != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VER_FUND", MstEntity.VerFund));

                if (MstEntity.OmbScreen != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OMB_SCREEN", MstEntity.OmbScreen));

                if (MstEntity.CbCaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_CASE_MANAGER", MstEntity.CbCaseManager));

                if (MstEntity.CaseManager != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CASE_MANAGER", MstEntity.CaseManager));

                if (MstEntity.VerifyOthCmb != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_VERIFY_OTH_CMB", MstEntity.VerifyOthCmb));

                if (MstEntity.SimPrint != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT", MstEntity.SimPrint));

                if (MstEntity.SimPrintDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_SIM_PRINT_DATE", MstEntity.SimPrintDate));

                if (MstEntity.CbFraud != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_CB_FRAUD", MstEntity.CbFraud));

                if (MstEntity.FraudDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FRAUD_DATE", MstEntity.FraudDate));

                if (MstEntity.AddOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_1", MstEntity.AddOperator1));

                if (MstEntity.LstcOperator1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_1", MstEntity.LstcOperator1));

                if (MstEntity.TimesUpdated1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_1", MstEntity.TimesUpdated1));

                if (MstEntity.AddOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_2", MstEntity.AddOperator2));

                if (MstEntity.LstcOperator2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_2", MstEntity.LstcOperator2));

                if (MstEntity.TimesUpdated2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_2", MstEntity.TimesUpdated2));

                if (MstEntity.AddOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_3", MstEntity.AddOperator3));

                if (MstEntity.LstcOperator3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_3", MstEntity.LstcOperator3));

                if (MstEntity.TimesUpdated3 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_3", MstEntity.TimesUpdated3));

                if (MstEntity.AddOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR_4", MstEntity.AddOperator4));

                if (MstEntity.LstcOperator4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LSTC_OPERATOR_4", MstEntity.LstcOperator4));

                if (MstEntity.TimesUpdated4 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_TIMES_UPDATED_4", MstEntity.TimesUpdated4));

                //if (MstEntity.PJob != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_JOB", MstEntity.PJob));

                //if (MstEntity.PHSD != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HSD", MstEntity.PHSD));

                //if (MstEntity.PSkills != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_SKILLS", MstEntity.PSkills));
                //if (MstEntity.PHousing != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_HOUSING", MstEntity.PHousing));
                //if (MstEntity.PTransport != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_TRANSPORT", MstEntity.PTransport));
                //if (MstEntity.PChldCare != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHLDCARE", MstEntity.PChldCare));
                //if (MstEntity.PCCENRL != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CCENRL", MstEntity.PCCENRL));
                //if (MstEntity.PELDCARE != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ELDRCARE", MstEntity.PELDCARE));
                //if (MstEntity.PECNEED != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_ECNEED", MstEntity.PECNEED));

                //if (MstEntity.PECHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CHINS", MstEntity.PECHINS));

                //if (MstEntity.PAHINS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_AHINS", MstEntity.PAHINS));

                //if (MstEntity.PRWENG != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RW_ENG", MstEntity.PRWENG));

                //if (MstEntity.PCURRDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_CURR_DSS", MstEntity.PCURRDSS));

                //if (MstEntity.PRECVDSS != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@MST_PRESS_RECV_DSS", MstEntity.PRECVDSS));

                if (MstEntity.LPM0001 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0001", MstEntity.LPM0001));

                if (MstEntity.LPM0002 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0002", MstEntity.LPM0002));

                if (MstEntity.LPM0003 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0003", MstEntity.LPM0003));

                if (MstEntity.LPM0004 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0004", MstEntity.LPM0004));

                if (MstEntity.LPM0005 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0005", MstEntity.LPM0005));

                if (MstEntity.LPM0006 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0006", MstEntity.LPM0006));

                if (MstEntity.LPM0007 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0007", MstEntity.LPM0007));

                if (MstEntity.LPM0008 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0008", MstEntity.LPM0008));

                if (MstEntity.LPM0009 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0009", MstEntity.LPM0009));

                if (MstEntity.LPM0010 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0010", MstEntity.LPM0010));

                if (MstEntity.LPM0011 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_LPM_0011", MstEntity.LPM0011));



                if (strFName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_FNAME", strFName));

                if (strDob != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_DOB", strDob));

                sqlParamList.Add(new SqlParameter("@Mode", MstEntity.Mode));

                if (MstEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", MstEntity.Type));

                if (!string.IsNullOrEmpty(MstEntity.ModuleCode.Trim()))
                {
                    sqlParamList.Add(new SqlParameter("@ModuleCode", MstEntity.ModuleCode));
                }

                SqlParameter sqlClientId = new SqlParameter("@MST_ClientId_OUT", SqlDbType.VarChar, 10);
                sqlClientId.Value = MstEntity.ClientId;
                sqlClientId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlClientId);
                SqlParameter sqlFamilyId = new SqlParameter("@MST_FamilyId_OUT", SqlDbType.VarChar, 10);
                sqlFamilyId.Value = MstEntity.FamilyId;
                sqlFamilyId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlFamilyId);
                SqlParameter sqlMSTSSNO = new SqlParameter("@MST_SSNNO_OUT", SqlDbType.VarChar, 10);
                sqlMSTSSNO.Value = MstEntity.FamilyId;
                sqlMSTSSNO.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlMSTSSNO);

                boolSuccess = CaseMst.InsertUpdateCASEMSTLeanIntake(sqlParamList);
                strNewApplNo = sqlApplNo.Value.ToString();
                strNewClientId = sqlClientId.Value.ToString();
                strNewFamilyId = sqlFamilyId.Value.ToString();
                strNewSSNO = sqlMSTSSNO.Value.ToString();

                if (boolSuccess == false)
                {
                    CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), "Record not modified Some error", MstEntity.LstcOperator1);
                    if (SnpAppEntity.Mode == Consts.Common.Add)
                    {
                        SnpAppEntity.App = strNewApplNo;
                        SnpAppEntity.ClientId = strNewClientId;
                        if (strNewSSNO != string.Empty)
                            SnpAppEntity.Ssno = strNewSSNO;

                        CaseMst.InsertErrorLog("Case2001SNPAPPLICANTINSERTERROR", ErrorLogSNP(SnpAppEntity), "Applicant record not inserted some error", MstEntity.LstcOperator1);
                    }
                    else
                    {
                        CaseMst.InsertErrorLog("Case2001SNPAPPLICANTUPDATEERROR", ErrorLogSNP(SnpAppEntity), "Applicant record not updated some error", MstEntity.LstcOperator1);
                    }

                }

            }
            catch (Exception ex)
            {
                boolSuccess = false;
                strMessage = ex.Message;
                CaseMst.InsertErrorLog("Case2001MST", ErrorLogMst(MstEntity, strNewClientId, strNewFamilyId, strNewSSNO), strMessage, MstEntity.LstcOperator1);
                CaseMst.InsertErrorLog("Case2001SNPAPPLICANTINSERTERROR", ErrorLogSNP(SnpAppEntity), strMessage, MstEntity.LstcOperator1);
            }


            strApplNo = strNewApplNo;
            strClientId = strNewClientId;
            strFamilyId = strNewFamilyId;
            strSSNO = strNewSSNO;
            strMSG = strMessage;
            return boolSuccess;
        }



        public bool InsertCasemstLog(CaseMstEntity MstEntity, string strColumnName, string strOldData, string strNewData, string struserid)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", MstEntity.ApplAgency));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", MstEntity.ApplDept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", MstEntity.ApplProgram));
                if (MstEntity.ApplYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_YEAR", MstEntity.ApplYr));

                sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                if (strColumnName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_COLUMN_NAME", strColumnName));

                if (strOldData != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_OLD_DATA", strOldData));

                if (strNewData != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_NEW_DATA", strNewData));


                if (struserid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_ADD_OPERATOR", struserid));

                if (MstEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", MstEntity.Type));

                boolSuccess = CaseMst.INSERTCASEMSTLOG(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;


            }



            return boolSuccess;
        }

        public bool INSERTUPDATEFIXSNPAUDIT(string strAgency,string strDept,string strProgram,string strYear,string strApplNo,string strFamilySeq,string strType,string strOldSSN,string strSSN,string strOldFamilyId,string strFamilyId,string strOldClientId,string strClientId,string strFamilyReason,string strClientReason,string strSSNReason,string strUserId,string strSwitchType)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@FIX_SAGENCY", strAgency));
                sqlParamList.Add(new SqlParameter("@FIX_SDEPT", strDept));
                sqlParamList.Add(new SqlParameter("@FIX_SPROG", strProgram));
                if (strYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_SYEAR", strYear));

                if (strApplNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_SAPP_NO", strApplNo));

                if (strFamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_SFAMILY_SEQ", strFamilySeq));

                if (strType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_TYPE", strType));

                if (strOldSSN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_OLDSSN", strOldSSN));


                if (strSSN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_SSN", strSSN));

                if (strOldFamilyId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_OLDFAMILY_ID", strOldFamilyId));

                if (strFamilyId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_FAMILY_ID", strFamilyId));

                if (strOldClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_OLDCLIENT_ID", strOldClientId));

                if (strClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_CLIENT_ID", strClientId));

                if (strFamilyReason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_FREASON", strFamilyReason));

                if (strClientReason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_CREASON", strClientReason));

                if (strSSNReason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_SREASON", strSSNReason));

                if (strUserId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FIX_ADD_OPERATOR", strUserId));

                if (strSwitchType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", strSwitchType));
                

                boolSuccess = CaseMst.INSERTUPDATEFIXSNPAUDIT(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;


            }



            return boolSuccess;
        }

        public bool CAPS_CASEUSAGE_INSUPDEL(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@USAGE_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@USAGE_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@USAGE_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@USAGE_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@USAGE_APP_NO", custEntity.ACTAPPNO));

                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SP_CODE", custEntity.ACTCODE));
                if (custEntity.ACTRESPSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SP_SEQ", custEntity.ACTRESPSEQ));
                if (custEntity.USAGE_PRIM_12HIST != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PRIM_12HIST", custEntity.USAGE_PRIM_12HIST));
                if (custEntity.USAGE_SEC_12HIST != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SEC_12HIST", custEntity.USAGE_SEC_12HIST));
                if (custEntity.USAGE_MONTH != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_MONTH", custEntity.USAGE_MONTH));
                if (custEntity.USAGE_PMON_SW != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PMON_SW", custEntity.USAGE_PMON_SW));
                if (custEntity.USAGE_SMON_SW != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SMON_SW", custEntity.USAGE_SMON_SW));

                if (custEntity.lstcoperator != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_LSTC_OPERATOR", custEntity.lstcoperator));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_ADD_OPERATOR", custEntity.addoperator));
                if (custEntity.USAGE_PRIM_PAYMENT != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PRIM_PAYMENT", custEntity.USAGE_PRIM_PAYMENT));
                if (custEntity.USAGE_PRIM_USAGE != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PRIM_USAGE", custEntity.USAGE_PRIM_USAGE));
                if (custEntity.USAGE_SEC_PAYMENT != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SEC_PAYMENT", custEntity.USAGE_SEC_PAYMENT));
                if (custEntity.USAGE_SEC_USAGE != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SEC_USAGE", custEntity.USAGE_SEC_USAGE));
                if (custEntity.USAGE_TOTAL != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_TOTAL", custEntity.USAGE_TOTAL));
                if (custEntity.USAGE_PSOURCE != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PSOURCE", custEntity.USAGE_PSOURCE));
                if (custEntity.USAGE_SSOURCE != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SSOURCE", custEntity.USAGE_SSOURCE));
                if (custEntity.USAGE_PRIM_VEND != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PRIM_VEND", custEntity.USAGE_PRIM_VEND));
                if (custEntity.USAGE_SEC_VEND != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SEC_VEND", custEntity.USAGE_SEC_VEND));

                if (custEntity.USAGE_LUMP_PRIM != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_LUMP_PRIM", custEntity.USAGE_LUMP_PRIM));
                if (custEntity.USAGE_LUMP_SEC != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_LUMP_SEC", custEntity.USAGE_LUMP_SEC));
                if (custEntity.USAGE_PRIM_ACTID != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_PRIM_ACTID", custEntity.USAGE_PRIM_ACTID));
                if (custEntity.USAGE_SEC_ACTID != string.Empty) sqlParamList.Add(new SqlParameter("@USAGE_SEC_ACTID", custEntity.USAGE_SEC_ACTID));

                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                CaseSnpData.CAPS_CASEUSAGE_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public string ErrorLogMst(CaseMstEntity MstEntity, string strNewClientId, string strNewFamilyId, string strNewSSNO)
        {

            StringBuilder strXmbulider = new StringBuilder();

            strXmbulider.Append("<TableData>");
            strXmbulider.Append("<FieldName>MST_AGENCY =  " + MstEntity.ApplAgency + "</FieldName>");
            strXmbulider.Append("<FieldName>MST_DEPT =  " + MstEntity.ApplDept + "</FieldName>");
            strXmbulider.Append("<FieldName>MST_PROGRAM =  " + MstEntity.ApplProgram + "</FieldName>");
            if (MstEntity.ApplYr != string.Empty)
                strXmbulider.Append("<FieldName>MST_YEAR =  " + MstEntity.ApplYr + "</FieldName>");
            if (MstEntity.ApplNo != string.Empty)
                strXmbulider.Append("<FieldName>MST_APP_NO =  " + MstEntity.ApplNo + "</FieldName>");




            if (MstEntity.FamilySeq != string.Empty)
                strXmbulider.Append("<FieldName>MST_FAMILY_SEQ =  " + MstEntity.FamilySeq + "</FieldName>");

            if (MstEntity.FamilyId != string.Empty)
                strXmbulider.Append("<FieldName>MST_FAMILY_ID =  " + MstEntity.FamilyId + "</FieldName>");

            if (MstEntity.ClientId != string.Empty)
                strXmbulider.Append("<FieldName>MST_CLIENT_ID =  " + MstEntity.ClientId + "</FieldName>");

            if (MstEntity.Ssn != string.Empty)
                strXmbulider.Append("<FieldName>MST_SSN =  " + MstEntity.Ssn + "</FieldName>");
            if (MstEntity.Bic != string.Empty)
                strXmbulider.Append("<FieldName>MST_BIC =  " + MstEntity.Bic + "</FieldName>");
            if (MstEntity.NickName != string.Empty)
                strXmbulider.Append("<FieldName>MST_NICKNAME =  " + MstEntity.NickName + "</FieldName>");

            if (MstEntity.EthnicOther != string.Empty)
                strXmbulider.Append("<FieldName>MST_ETHNIC_OTHER =  " + MstEntity.EthnicOther + "</FieldName>");

            if (MstEntity.State != string.Empty)
                strXmbulider.Append("<FieldName>MST_STATE =  " + MstEntity.State + "</FieldName>");

            if (MstEntity.City != string.Empty)
                strXmbulider.Append("<FieldName>MST_CITY =  " + MstEntity.City + "</FieldName>");

            if (MstEntity.Street != string.Empty)
                strXmbulider.Append("<FieldName>MST_STREET =  " + MstEntity.Street + "</FieldName>");

            if (MstEntity.Suffix != string.Empty)
                strXmbulider.Append("<FieldName>MST_SUFFIX =  " + MstEntity.Suffix + "</FieldName>");

            if (MstEntity.Hn != string.Empty)
                strXmbulider.Append("<FieldName>MST_HN =  " + MstEntity.Hn + "</FieldName>");

            if (MstEntity.Direction != string.Empty)
                strXmbulider.Append("<FieldName>MST_DIRECTION =  " + MstEntity.Direction + "</FieldName>");

            if (MstEntity.Apt != string.Empty)
                strXmbulider.Append("<FieldName>MST_APT =  " + MstEntity.Apt + "</FieldName>");

            if (MstEntity.Flr != string.Empty)
                strXmbulider.Append("<FieldName>MST_FLR =  " + MstEntity.Flr + "</FieldName>");

            if (MstEntity.Zip != string.Empty)
                strXmbulider.Append("<FieldName>MST_ZIP =  " + MstEntity.Zip + "</FieldName>");

            if (MstEntity.Zipplus != string.Empty)
                strXmbulider.Append("<FieldName>MST_ZIPPLUS =  " + MstEntity.Zipplus + "</FieldName>");

            if (MstEntity.Precinct != string.Empty)
                strXmbulider.Append("<FieldName>MST_PRECINCT =  " + MstEntity.Precinct + "</FieldName>");

            if (MstEntity.Area != string.Empty)
                strXmbulider.Append("<FieldName>MST_AREA =  " + MstEntity.Area + "</FieldName>");

            if (MstEntity.Phone != string.Empty)
                strXmbulider.Append("<FieldName>MST_PHONE =  " + MstEntity.Phone + "</FieldName>");

            if (MstEntity.NextYear != string.Empty)
                strXmbulider.Append("<FieldName>MST_NEXTYEAR =  " + MstEntity.NextYear + "</FieldName>");

            if (MstEntity.Classification != string.Empty)
                strXmbulider.Append("<FieldName>MST_CLASSIFICATION =  " + MstEntity.Classification + "</FieldName>");

            if (MstEntity.Language != string.Empty)
                strXmbulider.Append("<FieldName>MST_LANGUAGE =  " + MstEntity.Language + "</FieldName>");

            if (MstEntity.LanguageOt != string.Empty)
                strXmbulider.Append("<FieldName>MST_LANGUAGE_OT =  " + MstEntity.LanguageOt + "</FieldName>");

            if (MstEntity.IntakeWorker != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_WORKER =  " + MstEntity.IntakeWorker + "</FieldName>");

            if (MstEntity.IntakeDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_DATE =  " + MstEntity.IntakeDate + "</FieldName>");

            if (MstEntity.InitialDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_INITIAL_DATE =  " + MstEntity.InitialDate + "</FieldName>");

            if (MstEntity.CaseType != string.Empty)
                strXmbulider.Append("<FieldName>MST_CASE_TYPE =  " + MstEntity.CaseType + "</FieldName>");

            if (MstEntity.Housing != string.Empty)
                strXmbulider.Append("<FieldName>MST_HOUSING =  " + MstEntity.Housing + "</FieldName>");

            if (MstEntity.FamilyType != string.Empty)
                strXmbulider.Append("<FieldName>MST_FAMILY_TYPE =  " + MstEntity.FamilyType + "</FieldName>");

            if (MstEntity.Site != string.Empty)
                strXmbulider.Append("<FieldName>MST_SITE =  " + MstEntity.Site + "</FieldName>");

            if (MstEntity.Juvenile != string.Empty)
                strXmbulider.Append("<FieldName>MST_JUVENILE =  " + MstEntity.Juvenile + "</FieldName>");

            if (MstEntity.Senior != string.Empty)
                strXmbulider.Append("<FieldName>MST_SENIOR =  " + MstEntity.Senior + "</FieldName>");

            if (MstEntity.Secret != string.Empty)
                strXmbulider.Append("<FieldName>MST_SECRET =  " + MstEntity.Secret + "</FieldName>");

            if (MstEntity.CaseReviewDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_CASE_REVIEW_DATE =  " + MstEntity.CaseReviewDate + "</FieldName>");

            if (MstEntity.AlertCodes != string.Empty)
                strXmbulider.Append("<FieldName>MST_ALERT_CODES =  " + MstEntity.AlertCodes + "</FieldName>");

            if (MstEntity.ParentStatus != string.Empty)
                strXmbulider.Append("<FieldName>MST_PARENT_STATUS =  " + MstEntity.ParentStatus + "</FieldName>");

            if (MstEntity.IntakeHrs != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_HRS =  " + MstEntity.IntakeHrs + "</FieldName>");

            if (MstEntity.IntakeMns != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_MNS =  " + MstEntity.IntakeMns + "</FieldName>");

            if (MstEntity.IntakeScs != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_SCS =  " + MstEntity.IntakeScs + "</FieldName>");

            if (MstEntity.FinHrs != string.Empty)
                strXmbulider.Append("<FieldName>MST_FIN_HRS =  " + MstEntity.FinHrs + "</FieldName>");

            if (MstEntity.FinMns != string.Empty)
                strXmbulider.Append("<FieldName>MST_FIN_MNS =  " + MstEntity.FinMns + "</FieldName>");

            if (MstEntity.FinScs != string.Empty)
                strXmbulider.Append("<FieldName>MST_FIN_SCS =  " + MstEntity.FinScs + "</FieldName>");

            if (MstEntity.SimHrs != string.Empty)
                strXmbulider.Append("<FieldName>MST_SIM_HRS =  " + MstEntity.SimHrs + "</FieldName>");

            if (MstEntity.SimMns != string.Empty)
                strXmbulider.Append("<FieldName>MST_SIM_MNS =  " + MstEntity.SimMns + "</FieldName>");

            if (MstEntity.SimScs != string.Empty)
                strXmbulider.Append("<FieldName>MST_SIM_SCS =  " + MstEntity.SimScs + "</FieldName>");

            if (MstEntity.MedHrs != string.Empty)
                strXmbulider.Append("<FieldName>MST_MED_HRS =  " + MstEntity.MedHrs + "</FieldName>");

            if (MstEntity.MedMns != string.Empty)
                strXmbulider.Append("<FieldName>MST_MED_MNS =  " + MstEntity.MedMns + "</FieldName>");

            if (MstEntity.MedScs != string.Empty)
                strXmbulider.Append("<FieldName>MST_MED_SCS =  " + MstEntity.MedScs + "</FieldName>");

            if (MstEntity.TownShip != string.Empty)
                strXmbulider.Append("<FieldName>MST_TOWNSHIP =  " + MstEntity.TownShip + "</FieldName>");

            if (MstEntity.Rank1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK1 =  " + MstEntity.Rank1 + "</FieldName>");

            if (MstEntity.Rank2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK2 =  " + MstEntity.Rank2 + "</FieldName>");

            if (MstEntity.Rank3 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK3 =  " + MstEntity.Rank3 + "</FieldName>");

            if (MstEntity.Rank4 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK4 =  " + MstEntity.Rank4 + "</FieldName>");

            if (MstEntity.Rank5 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK5 =  " + MstEntity.Rank5 + "</FieldName>");

            if (MstEntity.Rank6 != string.Empty)
                strXmbulider.Append("<FieldName>MST_RANK6 =  " + MstEntity.Rank6 + "</FieldName>");

            if (MstEntity.Position1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_POSITION1 =  " + MstEntity.Position1 + "</FieldName>");

            if (MstEntity.Position2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_POSITION2 =  " + MstEntity.Position2 + "</FieldName>");

            if (MstEntity.Position3 != string.Empty)
                strXmbulider.Append("<FieldName>MST_POSITION3 =  " + MstEntity.Position3 + "</FieldName>");

            if (MstEntity.IntakeTime1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_INTAKE_TIME1 =  " + MstEntity.IntakeTime1 + "</FieldName>");

            if (MstEntity.SsnFlag != string.Empty)
                strXmbulider.Append("<FieldName>MST_SSN_FLAG =  " + MstEntity.SsnFlag + "</FieldName>");

            if (MstEntity.StateCase != string.Empty)
                strXmbulider.Append("<FieldName>MST_STATE_CASE =  " + MstEntity.StateCase + "</FieldName>");

            if (MstEntity.Verifier != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFIER =  " + MstEntity.Verifier + "</FieldName>");

            if (MstEntity.EligDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_ELIG_DATE =  " + MstEntity.EligDate + "</FieldName>");

            if (MstEntity.CatElig != string.Empty)
                strXmbulider.Append("<FieldName>MST_CAT_ELIG =  " + MstEntity.CatElig + "</FieldName>");

            if (MstEntity.MealElig != string.Empty)
                strXmbulider.Append("<FieldName>MST_MEAL_ELIG =  " + MstEntity.MealElig + "</FieldName>");

            if (MstEntity.VerifyW2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_W2 =  " + MstEntity.VerifyW2 + "</FieldName>");

            if (MstEntity.VerifyCheckStub != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_CHECK_STUB =  " + MstEntity.VerifyCheckStub + "</FieldName>");

            if (MstEntity.VerifyTaxReturn != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_TAX_RETURN =  " + MstEntity.VerifyTaxReturn + "</FieldName>");

            if (MstEntity.VerifyLetter != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_LETTER =  " + MstEntity.VerifyLetter + "</FieldName>");

            if (MstEntity.VerifyOther != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_OTHER =  " + MstEntity.VerifyOther + "</FieldName>");

            if (MstEntity.ReverifyDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_REVERIFY_DATE =  " + MstEntity.ReverifyDate + "</FieldName>");

            if (MstEntity.IncomeTypes != string.Empty)
                strXmbulider.Append("<FieldName>MST_INCOME_TYPES =  " + MstEntity.IncomeTypes + "</FieldName>");

            if (MstEntity.Poverty != string.Empty)
                strXmbulider.Append("<FieldName>MST_POVERTY =  " + MstEntity.Poverty + "</FieldName>");

            if (MstEntity.WaitList != string.Empty)
                strXmbulider.Append("<FieldName>MST_WAIT_LIST =  " + MstEntity.WaitList + "</FieldName>");

            if (MstEntity.ActiveStatus != string.Empty)
                strXmbulider.Append("<FieldName>MST_ACTIVE_STATUS =  " + MstEntity.ActiveStatus + "</FieldName>");

            if (MstEntity.TotalRank != string.Empty)
                strXmbulider.Append("<FieldName>MST_TOTAL_RANK =  " + MstEntity.TotalRank + "</FieldName>");

            if (MstEntity.NoInhh != string.Empty)
                strXmbulider.Append("<FieldName>MST_NO_INHH =  " + MstEntity.NoInhh + "</FieldName>");

            if (MstEntity.FamIncome != string.Empty)
                strXmbulider.Append("<FieldName>MST_FAM_INCOME =  " + MstEntity.FamIncome + "</FieldName>");

            if (MstEntity.NoInProg != string.Empty)
                strXmbulider.Append("<FieldName>MST_NO_INPROG =  " + MstEntity.NoInProg + "</FieldName>");

            if (MstEntity.ProgIncome != string.Empty)
                strXmbulider.Append("<FieldName>MST_PROG_INCOME =  " + MstEntity.ProgIncome + "</FieldName>");

            if (MstEntity.OutOfService != string.Empty)
                strXmbulider.Append("<FieldName>MST_OUT_OF_SERVICE =  " + MstEntity.OutOfService + "</FieldName>");

            if (MstEntity.Hud != string.Empty)
                strXmbulider.Append("<FieldName>MST_HUD =  " + MstEntity.Hud + "</FieldName>");

            if (MstEntity.Smi != string.Empty)
                strXmbulider.Append("<FieldName>MST_SMI =  " + MstEntity.Smi + "</FieldName>");

            if (MstEntity.Cmi != string.Empty)
                strXmbulider.Append("<FieldName>MST_CMI =  " + MstEntity.Cmi + "</FieldName>");

            if (MstEntity.County != string.Empty)
                strXmbulider.Append("<FieldName>MST_COUNTY =  " + MstEntity.County + "</FieldName>");

            if (MstEntity.AddressYears != string.Empty)
                strXmbulider.Append("<FieldName>MST_ADDRESS_YEARS =  " + MstEntity.AddressYears + "</FieldName>");

            if (MstEntity.MessagePhone != string.Empty)
                strXmbulider.Append("<FieldName>MST_MESSAGE_PHONE =  " + MstEntity.MessagePhone + "</FieldName>");

            if (MstEntity.CellPhone != string.Empty)
                strXmbulider.Append("<FieldName>MST_CELL_PHONE =  " + MstEntity.CellPhone + "</FieldName>");

            if (MstEntity.FaxNumber != string.Empty)
                strXmbulider.Append("<FieldName>MST_FAX_NUMBER =  " + MstEntity.FaxNumber + "</FieldName>");

            if (MstEntity.TtyNumber != string.Empty)
                strXmbulider.Append("<FieldName>MST_TTY_NUMBER =  " + MstEntity.TtyNumber + "</FieldName>");

            if (MstEntity.Email != string.Empty)
                strXmbulider.Append("<FieldName>MST_EMAIL =  " + MstEntity.Email + "</FieldName>");

            if (MstEntity.BestContact != string.Empty)
                strXmbulider.Append("<FieldName>MST_BEST_CONTACT =  " + MstEntity.BestContact + "</FieldName>");

            if (MstEntity.AboutUs != string.Empty)
                strXmbulider.Append("<FieldName>MST_ABOUT_US =  " + MstEntity.AboutUs + "</FieldName>");

            if (MstEntity.ImportDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_IMPORT_DATE =  " + MstEntity.ImportDate + "</FieldName>");

            if (MstEntity.DateAdded != string.Empty)
                strXmbulider.Append("<FieldName>MST_DATE_ADDED =  " + MstEntity.DateAdded + "</FieldName>");

            if (MstEntity.ExpRent != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_RENT =  " + MstEntity.ExpRent + "</FieldName>");

            if (MstEntity.ExpWater != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_WATER =  " + MstEntity.ExpWater + "</FieldName>");

            if (MstEntity.ExpElectric != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_ELECTRIC =  " + MstEntity.ExpElectric + "</FieldName>");

            if (MstEntity.ExpHeat != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_HEAT =  " + MstEntity.ExpHeat + "</FieldName>");

            if (MstEntity.ExpMisc != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_MISC =  " + MstEntity.ExpMisc + "</FieldName>");

            if (MstEntity.Debtcc != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_CC =  " + MstEntity.Debtcc + "</FieldName>");

            if (MstEntity.DebtLoans != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_LOANS =  " + MstEntity.DebtLoans + "</FieldName>");

            if (MstEntity.DebtMed != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_MED =  " + MstEntity.DebtMed + "</FieldName>");

            if (MstEntity.DebtOther != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_OTH =  " + MstEntity.DebtOther + "</FieldName>");

            if (MstEntity.DebtMisc != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_MISC =  " + MstEntity.DebtMisc + "</FieldName>");

            if (MstEntity.DebtTotal != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEBT_TOTAL =  " + MstEntity.DebtTotal + "</FieldName>");

            if (MstEntity.AsetPhy != string.Empty)
                strXmbulider.Append("<FieldName>MST_ASET_PHY =  " + MstEntity.AsetPhy + "</FieldName>");

            if (MstEntity.AsetLiq != string.Empty)
                strXmbulider.Append("<FieldName>MST_ASET_LIQ =  " + MstEntity.AsetLiq + "</FieldName>");

            if (MstEntity.AsetOth != string.Empty)
                strXmbulider.Append("<FieldName>MST_ASET_OTH =  " + MstEntity.AsetOth + "</FieldName>");

            if (MstEntity.AsetTotal != string.Empty)
                strXmbulider.Append("<FieldName>MST_ASET_TOTAL =  " + MstEntity.AsetTotal + "</FieldName>");

            if (MstEntity.AsetMisc != string.Empty)
                strXmbulider.Append("<FieldName>MST_ASET_MISC =  " + MstEntity.AsetMisc + "</FieldName>");

            if (MstEntity.AsetRatio != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEB_ASET_RATIO =  " + MstEntity.AsetRatio + "</FieldName>");

            if (MstEntity.DebIncmRation != string.Empty)
                strXmbulider.Append("<FieldName>MST_DEB_INCM_RATIO =  " + MstEntity.DebIncmRation + "</FieldName>");

            if (MstEntity.ExpTotal != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_TOTAL =  " + MstEntity.ExpTotal + "</FieldName>");

            if (MstEntity.ExpLivexpense != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_LIVEXPENSE =  " + MstEntity.ExpLivexpense + "</FieldName>");

            if (MstEntity.ExpCaseWorker != string.Empty)
                strXmbulider.Append("<FieldName>MST_EXP_CASEWORKER =  " + MstEntity.ExpCaseWorker + "</FieldName>");

            if (MstEntity.Dwelling != string.Empty)
                strXmbulider.Append("<FieldName>MST_DWELLING =  " + MstEntity.Dwelling + "</FieldName>");

            if (MstEntity.HeatIncRent != string.Empty)
                strXmbulider.Append("<FieldName>MST_HEAT_INC_RENT =  " + MstEntity.HeatIncRent + "</FieldName>");

            if (MstEntity.Source != string.Empty)
                strXmbulider.Append("<FieldName>MST_SOURCE =  " + MstEntity.Source + "</FieldName>");

            if (MstEntity.RollOver != string.Empty)
                strXmbulider.Append("<FieldName>MST_ROLLOVER =  " + MstEntity.RollOver + "</FieldName>");

            if (MstEntity.RiskValue != string.Empty)
                strXmbulider.Append("<FieldName>MST_RISK_VALUE =  " + MstEntity.RiskValue + "</FieldName>");

            if (MstEntity.SubShouse != string.Empty)
                strXmbulider.Append("<FieldName>MST_SUBSHOUSE =  " + MstEntity.SubShouse + "</FieldName>");

            if (MstEntity.SubStype != string.Empty)
                strXmbulider.Append("<FieldName>MST_SUBSTYPE =  " + MstEntity.SubStype + "</FieldName>");

            if (MstEntity.VerFund != string.Empty)
                strXmbulider.Append("<FieldName>MST_VER_FUND =  " + MstEntity.VerFund + "</FieldName>");

            if (MstEntity.OmbScreen != string.Empty)
                strXmbulider.Append("<FieldName>MST_OMB_SCREEN =  " + MstEntity.OmbScreen + "</FieldName>");

            if (MstEntity.CbCaseManager != string.Empty)
                strXmbulider.Append("<FieldName>MST_CB_CASE_MANAGER =  " + MstEntity.CbCaseManager + "</FieldName>");

            if (MstEntity.CaseManager != string.Empty)
                strXmbulider.Append("<FieldName>MST_CASE_MANAGER =  " + MstEntity.CaseManager + "</FieldName>");

            if (MstEntity.VerifyOthCmb != string.Empty)
                strXmbulider.Append("<FieldName>MST_VERIFY_OTH_CMB =  " + MstEntity.VerifyOthCmb + "</FieldName>");

            if (MstEntity.SimPrint != string.Empty)
                strXmbulider.Append("<FieldName>MST_SIM_PRINT =  " + MstEntity.SimPrint + "</FieldName>");

            if (MstEntity.SimPrintDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_SIM_PRINT_DATE =  " + MstEntity.SimPrintDate + "</FieldName>");

            if (MstEntity.CbFraud != string.Empty)
                strXmbulider.Append("<FieldName>MST_CB_FRAUD =  " + MstEntity.CbFraud + "</FieldName>");

            if (MstEntity.FraudDate != string.Empty)
                strXmbulider.Append("<FieldName>MST_FRAUD_DATE =  " + MstEntity.FraudDate + "</FieldName>");

            if (MstEntity.AddOperator1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_ADD_OPERATOR_1 =  " + MstEntity.AddOperator1 + "</FieldName>");

            if (MstEntity.LstcOperator1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LSTC_OPERATOR_1 =  " + MstEntity.LstcOperator1 + "</FieldName>");

            if (MstEntity.TimesUpdated1 != string.Empty)
                strXmbulider.Append("<FieldName>MST_TIMES_UPDATED_1 =  " + MstEntity.TimesUpdated1 + "</FieldName>");

            if (MstEntity.AddOperator2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_ADD_OPERATOR_2 =  " + MstEntity.AddOperator2 + "</FieldName>");

            if (MstEntity.LstcOperator2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LSTC_OPERATOR_2 =  " + MstEntity.LstcOperator2 + "</FieldName>");

            if (MstEntity.TimesUpdated2 != string.Empty)
                strXmbulider.Append("<FieldName>MST_TIMES_UPDATED_2 =  " + MstEntity.TimesUpdated2 + "</FieldName>");

            if (MstEntity.AddOperator3 != string.Empty)
                strXmbulider.Append("<FieldName>MST_ADD_OPERATOR_3 =  " + MstEntity.AddOperator3 + "</FieldName>");

            if (MstEntity.LstcOperator3 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LSTC_OPERATOR_3 =  " + MstEntity.LstcOperator3 + "</FieldName>");

            if (MstEntity.TimesUpdated3 != string.Empty)
                strXmbulider.Append("<FieldName>MST_TIMES_UPDATED_3 =  " + MstEntity.TimesUpdated3 + "</FieldName>");

            if (MstEntity.AddOperator4 != string.Empty)
                strXmbulider.Append("<FieldName>MST_ADD_OPERATOR_4 =  " + MstEntity.AddOperator4 + "</FieldName>");

            if (MstEntity.LstcOperator4 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LSTC_OPERATOR_4 =  " + MstEntity.LstcOperator4 + "</FieldName>");

            if (MstEntity.TimesUpdated4 != string.Empty)
                strXmbulider.Append("<FieldName>MST_TIMES_UPDATED_4 =  " + MstEntity.TimesUpdated4 + "</FieldName>");

            //if (MstEntity.PJob != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_JOB =  " + MstEntity.PJob + "</FieldName>");

            //if (MstEntity.PHSD != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_HSD =  " + MstEntity.PHSD + "</FieldName>");

            //if (MstEntity.PSkills != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_SKILLS =  " + MstEntity.PSkills + "</FieldName>");
            //if (MstEntity.PHousing != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_HOUSING =  " + MstEntity.PHousing + "</FieldName>");
            //if (MstEntity.PTransport != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_TRANSPORT =  " + MstEntity.PTransport + "</FieldName>");
            //if (MstEntity.PChldCare != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_CHLDCARE =  " + MstEntity.PChldCare + "</FieldName>");
            //if (MstEntity.PCCENRL != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_CCENRL =  " + MstEntity.PCCENRL + "</FieldName>");

            //if (MstEntity.PELDCARE != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_ELDRCARE =  " + MstEntity.PELDCARE + "</FieldName>");
            //if (MstEntity.PECNEED != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_ECNEED =  " + MstEntity.PECNEED + "</FieldName>");

            //if (MstEntity.PECHINS != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_CHINS =  " + MstEntity.PECHINS + "</FieldName>");

            //if (MstEntity.PAHINS != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_AHINS =  " + MstEntity.PAHINS + "</FieldName>");

            //if (MstEntity.PRWENG != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_RW_ENG =  " + MstEntity.PRWENG + "</FieldName>");

            //if (MstEntity.PCURRDSS != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_CURR_DSS =  " + MstEntity.PCURRDSS + "</FieldName>");

            //if (MstEntity.PRECVDSS != string.Empty)
            //    strXmbulider.Append("<FieldName>MST_PRESS_RECV_DSS =  " + MstEntity.PRECVDSS + "</FieldName>");

            if (MstEntity.LPM0001 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0001 =  " + MstEntity.LPM0001 + "</FieldName>");

            if (MstEntity.LPM0002 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0002 =  " + MstEntity.LPM0002 + "</FieldName>");

            if (MstEntity.LPM0003 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0003 =  " + MstEntity.LPM0003 + "</FieldName>");

            if (MstEntity.LPM0004 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0004 =  " + MstEntity.LPM0004 + "</FieldName>");

            if (MstEntity.LPM0005 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0005 =  " + MstEntity.LPM0005 + "</FieldName>");

            if (MstEntity.LPM0006 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0006 =  " + MstEntity.LPM0006 + "</FieldName>");

            if (MstEntity.LPM0007 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0007 =  " + MstEntity.LPM0007 + "</FieldName>");

            if (MstEntity.LPM0008 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0008 =  " + MstEntity.LPM0008 + "</FieldName>");

            if (MstEntity.LPM0009 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0009 =  " + MstEntity.LPM0009 + "</FieldName>");

            if (MstEntity.LPM0010 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0010 =  " + MstEntity.LPM0010 + "</FieldName>");

            if (MstEntity.LPM0011 != string.Empty)
                strXmbulider.Append("<FieldName>MST_LPM_0011 =  " + MstEntity.LPM0011 + "</FieldName>");

            strXmbulider.Append("<FieldName>Mode =  " + MstEntity.Mode + "</FieldName>");

            if (MstEntity.Type != string.Empty)
                strXmbulider.Append("<FieldName>Type =  " + MstEntity.Type + "</FieldName>");
            if (strNewClientId != string.Empty)
                strXmbulider.Append("<FieldName>MST_ClientId_OUT =  " + strNewClientId + "</FieldName>");
            if (strNewFamilyId != string.Empty)
                strXmbulider.Append("<FieldName>MST_FamilyId_OUT =  " + strNewFamilyId + "</FieldName>");
            if (strNewSSNO != string.Empty)
                strXmbulider.Append("<FieldName>MST_SSNNO_OUT =  " + strNewSSNO + "</FieldName>");
            strXmbulider.Append("<FieldName>DateTime =  " + DateTime.Now.Date.ToString() + "</FieldName>");

            strXmbulider.Append("</TableData>");


            return strXmbulider.ToString();
        }


        public string ErrorLogSNP(CaseSnpEntity SnpEntity)
        {

            StringBuilder strXmbulider = new StringBuilder();

            strXmbulider.Append("<TableData>");

            strXmbulider.Append("<FieldName>SNP_AGENCY =  " + SnpEntity.Agency + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_DEPT =  " + SnpEntity.Dept + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_PROGRAM =  " + SnpEntity.Program + "</FieldName>");
            if (SnpEntity.Year != string.Empty)
                strXmbulider.Append("<FieldName>SNP_YEAR =  " + SnpEntity.Year + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_APP =  " + SnpEntity.App + "</FieldName>");
            if (SnpEntity.FamilySeq != string.Empty) strXmbulider.Append("<FieldName>SNP_FAMILY_SEQ =  " + SnpEntity.FamilySeq + "</FieldName>");

            if (SnpEntity.MemberCode != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_MEMBER_CODE =  " + SnpEntity.MemberCode + "</FieldName>");
            }
            if (SnpEntity.ClientId != string.Empty)
                strXmbulider.Append("<FieldName>SNP_CLIENT_ID =  " + SnpEntity.ClientId + "</FieldName>");

            //SNP_CLIENT_ID
            strXmbulider.Append("<FieldName>SNP_SSNO =  " + SnpEntity.Ssno + "</FieldName>");
            //SNP_SS_BIC
            if (SnpEntity.SsBic != string.Empty)
                strXmbulider.Append("<FieldName>SNP_SS_BIC =  " + SnpEntity.SsBic + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_NAME_IX_LAST =  " + SnpEntity.NameixLast + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_NAME_IX_FI =  " + SnpEntity.NameixFi + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_NAME_IX_MI =  " + SnpEntity.NameixMi + "</FieldName>");
            if (SnpEntity.AltBdate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_ALT_BDATE =  " + SnpEntity.AltBdate + "</FieldName>");
            if (SnpEntity.AltLName != string.Empty)
                strXmbulider.Append("<FieldName>SNP_ALT_LNAME =  " + SnpEntity.AltLName + "</FieldName>");
            if (SnpEntity.AltFi != string.Empty)
                strXmbulider.Append("<FieldName>SNP_ALT_FI =  " + SnpEntity.AltFi + "</FieldName>");
            //SNP_ALT_LNAME
            //SNP_ALT_FI
            strXmbulider.Append("<FieldName>SNP_ALIAS =  " + SnpEntity.Alias + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_STATUS =  " + SnpEntity.Status + "</FieldName>");
            if (SnpEntity.Sex != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_SEX =  " + SnpEntity.Sex + "</FieldName>");
            }
            if (!SnpEntity.Age.Equals(string.Empty)) strXmbulider.Append("<FieldName>SNP_AGE =  " + SnpEntity.Age + "</FieldName>");

            if (SnpEntity.Ethnic != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_ETHNIC =  " + SnpEntity.Ethnic + "</FieldName>");
            }
            if (SnpEntity.Education != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_EDUCATION =  " + SnpEntity.Education + "</FieldName>");
            }
            //SNP_INCOME_BASIS
            if (SnpEntity.IncomeBasis != string.Empty)
                strXmbulider.Append("<FieldName>SNP_INCOME_BASIS =  " + SnpEntity.IncomeBasis + "</FieldName>");
            if (SnpEntity.HealthIns != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_HEALTH_INS =  " + SnpEntity.HealthIns + "</FieldName>");
            }

            if (SnpEntity.Vet != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_VET =  " + SnpEntity.Vet + "</FieldName>");
            }

            if (SnpEntity.Disable != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_DISABLE =  " + SnpEntity.Disable + "</FieldName>");
            }

            if (SnpEntity.FootStamps != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_FOOD_STAMPS =  " + SnpEntity.FootStamps + "</FieldName>");
            }

            if (SnpEntity.Farmer != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_FARMER =  " + SnpEntity.Farmer + "</FieldName>");
            }
            //SNP_APPL_DATE
            if (SnpEntity.ApplDate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_APPL_DATE =  " + SnpEntity.ApplDate + "</FieldName>");

            //SNP_APPL_TIME
            if (SnpEntity.ApplTime != string.Empty)
                strXmbulider.Append("<FieldName>SNP_APPL_TIME =  " + SnpEntity.ApplTime + "</FieldName>");
            //SNP_AMPM
            if (SnpEntity.Ampm != string.Empty)
                strXmbulider.Append("<FieldName>SNP_AMPM =  " + SnpEntity.Ampm + "</FieldName>");
            //SNP_INTAKE_DATE
            if (SnpEntity.InitialDate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_INTAKE_DATE =  " + SnpEntity.InitialDate + "</FieldName>");
            //SNP_SITE
            if (SnpEntity.Site != string.Empty)
                strXmbulider.Append("<FieldName>SNP_SITE =  " + SnpEntity.Site + "</FieldName>");
            //SNP_TOT_INCOME
            if (SnpEntity.TotIncome != string.Empty)
                strXmbulider.Append("<FieldName>SNP_TOT_INCOME =  " + SnpEntity.TotIncome + "</FieldName>");
            //SNP_PROG_INCOME
            if (SnpEntity.ProgIncome != string.Empty)
                strXmbulider.Append("<FieldName>SNP_PROG_INCOME =  " + SnpEntity.ProgIncome + "</FieldName>");
            //SNP_CLAIM_SSNO
            if (SnpEntity.ClaimSsno != string.Empty)
                strXmbulider.Append("<FieldName>SNP_CLAIM_SSNO =  " + SnpEntity.ClaimSsno + "</FieldName>");
            //SNP_CLAIM_SS_BIC
            if (SnpEntity.ClaimSsbic != string.Empty)
                strXmbulider.Append("<FieldName>SNP_CLAIM_SS_BIC =  " + SnpEntity.ClaimSsbic + "</FieldName>");
            //SNP_WAGEM
            if (SnpEntity.Wagem != string.Empty)
                strXmbulider.Append("<FieldName>SNP_WAGEM =  " + SnpEntity.Wagem + "</FieldName>");
            if (SnpEntity.Wic != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_WIC =  " + SnpEntity.Wic + "</FieldName>");
            }
            //SNP_STUDENT
            if (SnpEntity.Student != string.Empty)
                strXmbulider.Append("<FieldName>SNP_STUDENT =  " + SnpEntity.Student + "</FieldName>");

            if (SnpEntity.Resident != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_RESIDENT =  " + SnpEntity.Resident + "</FieldName>");
            }

            if (SnpEntity.Pregnant != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_PREGNANT =  " + SnpEntity.Pregnant + "</FieldName>");
            }

            if (SnpEntity.MaritalStatus != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_MARITAL_STATUS =  " + SnpEntity.MaritalStatus + "</FieldName>");
            }

            if (SnpEntity.SchoolDistrict != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_SCHOOL_DISTRICT =  " + SnpEntity.SchoolDistrict + "</FieldName>");
            }
            if (SnpEntity.AlienRegNo != string.Empty)
                strXmbulider.Append("<FieldName>SNP_ALIEN_REG_NO =  " + SnpEntity.AlienRegNo + "</FieldName>");

            if (SnpEntity.LegalTowork != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_LEGAL_TO_WORK =  " + SnpEntity.LegalTowork + "</FieldName>");
            }
            if (SnpEntity.ExpireWorkDate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EXPIRE_WORK_DATE =  " + SnpEntity.ExpireWorkDate + "</FieldName>");

            //SNP_EMPLOYED
            if (SnpEntity.Employed != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPLOYED =  " + SnpEntity.Employed + "</FieldName>");
            //SNP_LAST_WORK_DATE
            if (SnpEntity.LastWorkDate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_LAST_WORK_DATE =  " + SnpEntity.LastWorkDate + "</FieldName>");
            //SNP_WORK_LIMIT
            if (SnpEntity.WorkLimit != string.Empty)
                strXmbulider.Append("<FieldName>SNP_WORK_LIMIT =  " + SnpEntity.WorkLimit + "</FieldName>");
            //SNP_EXPLAIN_WORK_LIMIT
            if (SnpEntity.ExplainWorkLimit != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EXPLAIN_WORK_LIMIT =  " + SnpEntity.ExplainWorkLimit + "</FieldName>");
            //SNP_NUMBER_OF_C_JOBS
            if (SnpEntity.NumberOfcjobs != string.Empty)
                strXmbulider.Append("<FieldName>SNP_NUMBER_OF_C_JOBS =  " + SnpEntity.NumberOfcjobs + "</FieldName>");
            //SNP_NUMBER_OF_LV_JOBS
            if (SnpEntity.NumberofLvjobs != string.Empty)
                strXmbulider.Append("<FieldName>SNP_NUMBER_OF_LV_JOBS =  " + SnpEntity.NumberofLvjobs + "</FieldName>");
            //SNP_FULL_TIME_HOURS
            if (SnpEntity.FullTimeHours != string.Empty)
                strXmbulider.Append("<FieldName>SNP_FULL_TIME_HOURS =  " + SnpEntity.FullTimeHours + "</FieldName>");
            //SNP_PART_TIME_HOURS
            if (SnpEntity.PartTimeHours != string.Empty)
                strXmbulider.Append("<FieldName>SNP_PART_TIME_HOURS =  " + SnpEntity.PartTimeHours + "</FieldName>");
            //SNP_SEASONAL_EMPLOY
            if (SnpEntity.SeasonalEmploy != string.Empty)
                strXmbulider.Append("<FieldName>SNP_SEASONAL_EMPLOY =  " + SnpEntity.SeasonalEmploy + "</FieldName>");
            //SNP_1ST_SHIFT
            if (SnpEntity.IstShift != string.Empty)
                strXmbulider.Append("<FieldName>SNP_1ST_SHIFT =  " + SnpEntity.IstShift + "</FieldName>");
            //SNP_2ND_SHIFT
            if (SnpEntity.IIndShift != string.Empty)
                strXmbulider.Append("<FieldName>SNP_2ND_SHIFT =  " + SnpEntity.IIndShift + "</FieldName>");
            //SNP_3RD_SHIFT
            if (SnpEntity.IIIrdShift != string.Empty)
                strXmbulider.Append("<FieldName>SNP_3RD_SHIFT =  " + SnpEntity.IIIrdShift + "</FieldName>");
            //SNP_R_SHIFT
            if (SnpEntity.RShift != string.Empty)
                strXmbulider.Append("<FieldName>SNP_R_SHIFT =  " + SnpEntity.RShift + "</FieldName>");
            //SNP_EMPLOYER_NAME
            if (SnpEntity.EmployerName != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPLOYER_NAME =  " + SnpEntity.EmployerName + "</FieldName>");
            //SNP_EMPLOYER_STREET
            if (SnpEntity.EmployerStreet != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPLOYER_STREET =  " + SnpEntity.EmployerStreet + "</FieldName>");
            //SNP_EMPLOYER_CITY
            if (SnpEntity.EmployerCity != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPLOYER_CITY =  " + SnpEntity.EmployerCity + "</FieldName>");
            //SNP_JOB_TITLE
            if (SnpEntity.JobTitle != string.Empty)
                strXmbulider.Append("<FieldName>SNP_JOB_TITLE =  " + SnpEntity.JobTitle + "</FieldName>");
            //SNP_JOB_CATEGORY
            if (SnpEntity.JobCategory != string.Empty)
                strXmbulider.Append("<FieldName>SNP_JOB_CATEGORY =  " + SnpEntity.JobCategory + "</FieldName>");
            //SNP_HOURLY_WAGE
            if (SnpEntity.HourlyWage != string.Empty)
                strXmbulider.Append("<FieldName>SNP_HOURLY_WAGE =  " + SnpEntity.HourlyWage + "</FieldName>");
            //SNP_PAY_FREQUENCY
            if (SnpEntity.PayFrequency != string.Empty)
                strXmbulider.Append("<FieldName>SNP_PAY_FREQUENCY =  " + SnpEntity.PayFrequency + "</FieldName>");
            //SNP_HIRE_DATE
            if (SnpEntity.HireDate != string.Empty)
                strXmbulider.Append("<FieldName>SNP_HIRE_DATE =  " + SnpEntity.HireDate + "</FieldName>");
            //SNP_TRANSERV
            if (SnpEntity.Transerv != string.Empty)
                strXmbulider.Append("<FieldName>SNP_TRANSERV =  " + SnpEntity.Transerv + "</FieldName>");

            if (SnpEntity.Relitran != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_RELITRAN =  " + SnpEntity.Relitran + "</FieldName>");
            }

            if (SnpEntity.Drvlic != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_DRVLIC =  " + SnpEntity.Drvlic + "</FieldName>");
            }

            if (SnpEntity.Race != string.Empty)
            {
                strXmbulider.Append("<FieldName>SNP_RACE =  " + SnpEntity.Race + "</FieldName>");
            }
            //SNP_EMPL_AREA
            if (SnpEntity.EmplPhone != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPL_Phone =  " + SnpEntity.EmplPhone + "</FieldName>");

            if (SnpEntity.EmplExt != string.Empty)
                strXmbulider.Append("<FieldName>SNP_EMPL_EXT =  " + SnpEntity.EmplExt + "</FieldName>");

            strXmbulider.Append("<FieldName>SNP_DOB_NA =  " + SnpEntity.DobNa + "</FieldName>");
            //SNP_SSN_REASON
            if (SnpEntity.SsnReason != string.Empty)
                strXmbulider.Append("<FieldName>SNP_SSN_REASON =  " + SnpEntity.SsnReason + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_EXCLUDE =  " + SnpEntity.Exclude + "</FieldName>");
            //SNP_BLIND
            if (SnpEntity.Blind != string.Empty)
                strXmbulider.Append("<FieldName>SNP_BLIND =  " + SnpEntity.Blind + "</FieldName>");
            //SNP_ABLE_TO_WORK
            if (SnpEntity.AbleTowork != string.Empty)
                strXmbulider.Append("<FieldName>SNP_ABLE_TO_WORK =  " + SnpEntity.AbleTowork + "</FieldName>");
            //SNP_REC_MEDICARE
            if (SnpEntity.RecMedicare != string.Empty)
                strXmbulider.Append("<FieldName>SNP_REC_MEDICARE =  " + SnpEntity.RecMedicare + "</FieldName>");
            //SNP_PURCHASE_FOOD
            if (SnpEntity.PurchaseFood != string.Empty)
                strXmbulider.Append("<FieldName>SNP_PURCHASE_FOOD =  " + SnpEntity.PurchaseFood + "</FieldName>");
            //SNP_VEHICLE_VALUE
            if (SnpEntity.VechicleValue != string.Empty)
                strXmbulider.Append("<FieldName>SNP_VEHICLE_VALUE =  " + SnpEntity.VechicleValue + "</FieldName>");
            //SNP_OTHER_VEHICLE_VALUE
            if (SnpEntity.OtherVehicleValue != string.Empty)
                strXmbulider.Append("<FieldName>SNP_OTHER_VEHICLE_VALUE =  " + SnpEntity.OtherVehicleValue + "</FieldName>");
            //SNP_OTHER_ASSET_VALUE
            if (SnpEntity.OtherAssetValue != string.Empty)
                strXmbulider.Append("<FieldName>SNP_OTHER_ASSET_VALUE =  " + SnpEntity.OtherAssetValue + "</FieldName>");

            if (SnpEntity.SsnSearchType != string.Empty)
                strXmbulider.Append("<FieldName>SsnSearchType =  " + SnpEntity.SsnSearchType + "</FieldName>");
            if (SnpEntity.AltAgency != string.Empty)
                strXmbulider.Append("<FieldName>ALT_AGENCY =  " + SnpEntity.AltAgency + "</FieldName>");
            if (SnpEntity.AltDept != string.Empty)
                strXmbulider.Append("<FieldName>ALT_DEPT =  " + SnpEntity.AltDept + "</FieldName>");
            if (SnpEntity.AltProgram != string.Empty)
                strXmbulider.Append("<FieldName>ALT_PROGRAM =  " + SnpEntity.AltProgram + "</FieldName>");
            if (SnpEntity.AltYear != string.Empty)
                strXmbulider.Append("<FieldName>ALT_YEAR =  " + SnpEntity.AltYear + "</FieldName>");
            if (SnpEntity.AltApp != string.Empty)
                strXmbulider.Append("<FieldName>ALT_APP =  " + SnpEntity.AltApp + "</FieldName>");
            if (SnpEntity.AltFamilySeq != string.Empty)
                strXmbulider.Append("<FieldName>ALT_FAMILY_SEQ =  " + SnpEntity.AltFamilySeq + "</FieldName>");
            if (SnpEntity.Type != string.Empty)
                strXmbulider.Append("<FieldName>Type =  " + SnpEntity.Type + "</FieldName>");



            strXmbulider.Append("<FieldName>SNP_ADD_OPERATOR =  " + SnpEntity.AddOperator + "</FieldName>");
            strXmbulider.Append("<FieldName>SNP_LSTC_OPERATOR =  " + SnpEntity.LstcOperator + "</FieldName>");

            if (SnpEntity.MilitaryStatus != string.Empty)
                strXmbulider.Append("<FieldName>SNP_MILITARY_STATUS =  " + SnpEntity.MilitaryStatus + "</FieldName>");

            if (SnpEntity.Health_Codes != string.Empty)
                strXmbulider.Append("<FieldName>SNP_HEALTH_CODES =  " + SnpEntity.Health_Codes + "</FieldName>");

            if (SnpEntity.WorkStatus != string.Empty)
                strXmbulider.Append("<FieldName>SNP_WORK_STAT =  " + SnpEntity.WorkStatus + "</FieldName>");

            if (SnpEntity.NonCashBenefits != string.Empty)
                strXmbulider.Append("<FieldName>SNP_NCASHBEN =  " + SnpEntity.NonCashBenefits + "</FieldName>");

            if (SnpEntity.Youth != string.Empty)
                strXmbulider.Append("<FieldName>SNP_YOUTH =  " + SnpEntity.Youth + "</FieldName>");




            strXmbulider.Append("<FieldName>DateTime =  " + DateTime.Now.Date.ToString() + "</FieldName>");

            strXmbulider.Append("</TableData>");

            return strXmbulider.ToString();


        }


        public bool InsertSNPDETAILS(CaseSnpEntity SnpEntity, out string strOutFamilySeq)
        {
            bool boolstatus = false;
            string strNewFamilySeq = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SNP_AGENCY", SnpEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SNP_DEPT", SnpEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SNP_PROGRAM", SnpEntity.Program));
                if (SnpEntity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_YEAR", SnpEntity.Year));
                sqlParamList.Add(new SqlParameter("@SNP_APP", SnpEntity.App));
                if (SnpEntity.FamilySeq != string.Empty) sqlParamList.Add(new SqlParameter("@SNP_FAMILY_SEQ", SnpEntity.FamilySeq));

                if (SnpEntity.MemberCode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MEMBER_CODE", SnpEntity.MemberCode));
                }
                if (SnpEntity.ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLIENT_ID", SnpEntity.ClientId));

                //SNP_CLIENT_ID
                sqlParamList.Add(new SqlParameter("@SNP_SSNO", SnpEntity.Ssno));
                //SNP_SS_BIC
                if (SnpEntity.SsBic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SS_BIC", SnpEntity.SsBic));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_LAST", SnpEntity.NameixLast));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_FI", SnpEntity.NameixFi));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_MI", SnpEntity.NameixMi));
                if (SnpEntity.AltBdate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_BDATE", SnpEntity.AltBdate));
                if (SnpEntity.AltLName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_LNAME", SnpEntity.AltLName));
                if (SnpEntity.AltFi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_FI", SnpEntity.AltFi));
                //SNP_ALT_LNAME
                //SNP_ALT_FI
                sqlParamList.Add(new SqlParameter("@SNP_ALIAS", SnpEntity.Alias));
                sqlParamList.Add(new SqlParameter("@SNP_STATUS", SnpEntity.Status));
                if (SnpEntity.Sex != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SEX", SnpEntity.Sex));
                }
                if (!SnpEntity.Age.Equals(string.Empty)) sqlParamList.Add(new SqlParameter("@SNP_AGE", SnpEntity.Age));

                if (SnpEntity.Ethnic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_ETHNIC", SnpEntity.Ethnic));
                }
                if (SnpEntity.Education != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_EDUCATION", SnpEntity.Education));
                }
                //SNP_INCOME_BASIS
                if (SnpEntity.IncomeBasis != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_INCOME_BASIS", SnpEntity.IncomeBasis));
                if (SnpEntity.HealthIns != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HEALTH_INS", SnpEntity.HealthIns));
                }

                if (SnpEntity.Vet != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_VET", SnpEntity.Vet));
                }

                if (SnpEntity.Disable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_DISABLE", SnpEntity.Disable));
                }

                if (SnpEntity.FootStamps != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_FOOD_STAMPS", SnpEntity.FootStamps));
                }

                if (SnpEntity.Farmer != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_FARMER", SnpEntity.Farmer));
                }
                //SNP_APPL_DATE
                if (SnpEntity.ApplDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_APPL_DATE", SnpEntity.ApplDate));

                //SNP_APPL_TIME
                if (SnpEntity.ApplTime != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_APPL_TIME", SnpEntity.ApplTime));
                //SNP_AMPM
                if (SnpEntity.Ampm != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_AMPM", SnpEntity.Ampm));
                //SNP_INTAKE_DATE
                if (SnpEntity.InitialDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_INTAKE_DATE", SnpEntity.InitialDate));
                //SNP_SITE
                if (SnpEntity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SITE", SnpEntity.Site));
                //SNP_TOT_INCOME
                if (SnpEntity.TotIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_TOT_INCOME", SnpEntity.TotIncome));
                //SNP_PROG_INCOME
                if (SnpEntity.ProgIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PROG_INCOME", SnpEntity.ProgIncome));
                //SNP_CLAIM_SSNO
                if (SnpEntity.ClaimSsno != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLAIM_SSNO", SnpEntity.ClaimSsno));
                //SNP_CLAIM_SS_BIC
                if (SnpEntity.ClaimSsbic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLAIM_SS_BIC", SnpEntity.ClaimSsbic));
                //SNP_WAGEM
                if (SnpEntity.Wagem != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_WAGEM", SnpEntity.Wagem));
                if (SnpEntity.Wic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_WIC", SnpEntity.Wic));
                }
                //SNP_STUDENT
                if (SnpEntity.Student != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_STUDENT", SnpEntity.Student));

                if (SnpEntity.Resident != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RESIDENT", SnpEntity.Resident));
                }

                if (SnpEntity.Pregnant != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_PREGNANT", SnpEntity.Pregnant));
                }

                if (SnpEntity.MaritalStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MARITAL_STATUS", SnpEntity.MaritalStatus));
                }

                if (SnpEntity.SchoolDistrict != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SCHOOL_DISTRICT", SnpEntity.SchoolDistrict));
                }
                if (SnpEntity.AlienRegNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALIEN_REG_NO", SnpEntity.AlienRegNo));

                if (SnpEntity.LegalTowork != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_LEGAL_TO_WORK", SnpEntity.LegalTowork));
                }
                if (SnpEntity.ExpireWorkDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EXPIRE_WORK_DATE", SnpEntity.ExpireWorkDate));

                //SNP_EMPLOYED
                if (SnpEntity.Employed != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYED", SnpEntity.Employed));
                //SNP_LAST_WORK_DATE
                if (SnpEntity.LastWorkDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_LAST_WORK_DATE", SnpEntity.LastWorkDate));
                //SNP_WORK_LIMIT
                if (SnpEntity.WorkLimit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_WORK_LIMIT", SnpEntity.WorkLimit));
                //SNP_EXPLAIN_WORK_LIMIT
                if (SnpEntity.ExplainWorkLimit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EXPLAIN_WORK_LIMIT", SnpEntity.ExplainWorkLimit));
                //SNP_NUMBER_OF_C_JOBS
                if (SnpEntity.NumberOfcjobs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NUMBER_OF_C_JOBS", SnpEntity.NumberOfcjobs));
                //SNP_NUMBER_OF_LV_JOBS
                if (SnpEntity.NumberofLvjobs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NUMBER_OF_LV_JOBS", SnpEntity.NumberofLvjobs));
                //SNP_FULL_TIME_HOURS
                if (SnpEntity.FullTimeHours != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_FULL_TIME_HOURS", SnpEntity.FullTimeHours));
                //SNP_PART_TIME_HOURS
                if (SnpEntity.PartTimeHours != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PART_TIME_HOURS", SnpEntity.PartTimeHours));
                //SNP_SEASONAL_EMPLOY
                if (SnpEntity.SeasonalEmploy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SEASONAL_EMPLOY", SnpEntity.SeasonalEmploy));
                //SNP_1ST_SHIFT
                if (SnpEntity.IstShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_1ST_SHIFT", SnpEntity.IstShift));
                //SNP_2ND_SHIFT
                if (SnpEntity.IIndShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_2ND_SHIFT", SnpEntity.IIndShift));
                //SNP_3RD_SHIFT
                if (SnpEntity.IIIrdShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_3RD_SHIFT", SnpEntity.IIIrdShift));
                //SNP_R_SHIFT
                if (SnpEntity.RShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_R_SHIFT", SnpEntity.RShift));
                //SNP_EMPLOYER_NAME
                if (SnpEntity.EmployerName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_NAME", SnpEntity.EmployerName));
                //SNP_EMPLOYER_STREET
                if (SnpEntity.EmployerStreet != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_STREET", SnpEntity.EmployerStreet));
                //SNP_EMPLOYER_CITY
                if (SnpEntity.EmployerCity != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_CITY", SnpEntity.EmployerCity));
                //SNP_JOB_TITLE
                if (SnpEntity.JobTitle != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_JOB_TITLE", SnpEntity.JobTitle));
                //SNP_JOB_CATEGORY
                if (SnpEntity.JobCategory != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_JOB_CATEGORY", SnpEntity.JobCategory));
                //SNP_HOURLY_WAGE
                if (SnpEntity.HourlyWage != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_HOURLY_WAGE", SnpEntity.HourlyWage));
                //SNP_PAY_FREQUENCY
                if (SnpEntity.PayFrequency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PAY_FREQUENCY", SnpEntity.PayFrequency));
                //SNP_HIRE_DATE
                if (SnpEntity.HireDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_HIRE_DATE", SnpEntity.HireDate));
                //SNP_TRANSERV
                if (SnpEntity.Transerv != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_TRANSERV", SnpEntity.Transerv));

                if (SnpEntity.Relitran != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RELITRAN", SnpEntity.Relitran));
                }

                if (SnpEntity.Drvlic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_DRVLIC", SnpEntity.Drvlic));
                }

                if (SnpEntity.Race != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RACE", SnpEntity.Race));
                }
                //SNP_EMPL_AREA
                if (SnpEntity.EmplPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPL_Phone", SnpEntity.EmplPhone));
                ////SNP_EMPL_EXCH
                //if (SnpEntity.EmplExch != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SNP_EMPL_EXCH", SnpEntity.EmplExch));
                ////SNP_EMPL_TEL
                //if (SnpEntity.EmplTel != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SNP_EMPL_TEL", SnpEntity.EmplTel));
                //SNP_EMPL_EXT
                if (SnpEntity.EmplExt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPL_EXT", SnpEntity.EmplExt));

                sqlParamList.Add(new SqlParameter("@SNP_DOB_NA", SnpEntity.DobNa));
                //SNP_SSN_REASON
                if (SnpEntity.SsnReason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SSN_REASON", SnpEntity.SsnReason));
                sqlParamList.Add(new SqlParameter("@SNP_EXCLUDE", SnpEntity.Exclude));
                //SNP_BLIND
                if (SnpEntity.Blind != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_BLIND", SnpEntity.Blind));
                //SNP_ABLE_TO_WORK
                if (SnpEntity.AbleTowork != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ABLE_TO_WORK", SnpEntity.AbleTowork));
                //SNP_REC_MEDICARE
                if (SnpEntity.RecMedicare != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_REC_MEDICARE", SnpEntity.RecMedicare));
                //SNP_PURCHASE_FOOD
                if (SnpEntity.PurchaseFood != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PURCHASE_FOOD", SnpEntity.PurchaseFood));
                //SNP_VEHICLE_VALUE
                if (SnpEntity.VechicleValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_VEHICLE_VALUE", SnpEntity.VechicleValue));
                //SNP_OTHER_VEHICLE_VALUE
                if (SnpEntity.OtherVehicleValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_OTHER_VEHICLE_VALUE", SnpEntity.OtherVehicleValue));
                //SNP_OTHER_ASSET_VALUE
                if (SnpEntity.OtherAssetValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_OTHER_ASSET_VALUE", SnpEntity.OtherAssetValue));

                if (SnpEntity.SsnSearchType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SsnSearchType", SnpEntity.SsnSearchType));
                if (SnpEntity.AltAgency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_AGENCY", SnpEntity.AltAgency));
                if (SnpEntity.AltDept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_DEPT", SnpEntity.AltDept));
                if (SnpEntity.AltProgram != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_PROGRAM", SnpEntity.AltProgram));
                if (SnpEntity.AltYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_YEAR", SnpEntity.AltYear));
                if (SnpEntity.AltApp != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_APP", SnpEntity.AltApp));
                if (SnpEntity.AltFamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_FAMILY_SEQ", SnpEntity.AltFamilySeq));
                if (SnpEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", SnpEntity.Type));

                sqlParamList.Add(new SqlParameter("@SNP_ADD_OPERATOR", SnpEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@SNP_LSTC_OPERATOR", SnpEntity.LstcOperator));
                if (SnpEntity.MilitaryStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MILITARY_STATUS", SnpEntity.MilitaryStatus));
                }
                if (SnpEntity.Health_Codes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HEALTH_CODES", SnpEntity.Health_Codes));
                }
                if (SnpEntity.WorkStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_WORK_STAT", SnpEntity.WorkStatus));
                }
                if (SnpEntity.NonCashBenefits != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_NCASHBEN", SnpEntity.NonCashBenefits));
                }
                if (SnpEntity.Youth != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_YOUTH", SnpEntity.Youth));
                }
                if (SnpEntity.SnpSuffix != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SUFFIX", SnpEntity.SnpSuffix));
                }
                if (SnpEntity.Snp_HH_ExcludeMem != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HH_EXCLUDE", SnpEntity.Snp_HH_ExcludeMem));
                }
                SqlParameter sqlFamilySeqOut = new SqlParameter("@SNP_OUT_FAMILY_SEQ", SqlDbType.VarChar, 10);
                sqlFamilySeqOut.Value = SnpEntity.FamilySeq;
                sqlFamilySeqOut.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlFamilySeqOut);

                boolstatus = CaseSnpData.InsertSNPDETAILS(sqlParamList);
                strNewFamilySeq = sqlFamilySeqOut.Value.ToString();
                if (boolstatus == false)
                {
                    CaseMst.InsertErrorLog("Case2001SNP", ErrorLogSNP(SnpEntity), "Record not modified some error", SnpEntity.LstcOperator);
                }
            }

            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("Case2001SNP", ErrorLogSNP(SnpEntity), ex.Message, SnpEntity.LstcOperator);
                boolstatus = false;
            }
            strOutFamilySeq = strNewFamilySeq;
            return boolstatus;
        }

        public bool InsertSNPDETAILSLeanIntake(CaseSnpEntity SnpEntity, out string strOutFamilySeq)
        {
            bool boolstatus = false;
            string strNewFamilySeq = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SNP_AGENCY", SnpEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SNP_DEPT", SnpEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SNP_PROGRAM", SnpEntity.Program));
                if (SnpEntity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_YEAR", SnpEntity.Year));
                sqlParamList.Add(new SqlParameter("@SNP_APP", SnpEntity.App));
                if (SnpEntity.FamilySeq != string.Empty) sqlParamList.Add(new SqlParameter("@SNP_FAMILY_SEQ", SnpEntity.FamilySeq));

                if (SnpEntity.MemberCode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MEMBER_CODE", SnpEntity.MemberCode));
                }
                if (SnpEntity.ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLIENT_ID", SnpEntity.ClientId));

                //SNP_CLIENT_ID
                sqlParamList.Add(new SqlParameter("@SNP_SSNO", SnpEntity.Ssno));
                //SNP_SS_BIC
                if (SnpEntity.SsBic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SS_BIC", SnpEntity.SsBic));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_LAST", SnpEntity.NameixLast));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_FI", SnpEntity.NameixFi));
                sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_MI", SnpEntity.NameixMi));
                if (SnpEntity.AltBdate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_BDATE", SnpEntity.AltBdate));
                if (SnpEntity.AltLName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_LNAME", SnpEntity.AltLName));
                if (SnpEntity.AltFi != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_FI", SnpEntity.AltFi));
                //SNP_ALT_LNAME
                //SNP_ALT_FI
                sqlParamList.Add(new SqlParameter("@SNP_ALIAS", SnpEntity.Alias));
                sqlParamList.Add(new SqlParameter("@SNP_STATUS", SnpEntity.Status));
                if (SnpEntity.Sex != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SEX", SnpEntity.Sex));
                }
                if (!SnpEntity.Age.Equals(string.Empty)) sqlParamList.Add(new SqlParameter("@SNP_AGE", SnpEntity.Age));

                if (SnpEntity.Ethnic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_ETHNIC", SnpEntity.Ethnic));
                }
                if (SnpEntity.Education != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_EDUCATION", SnpEntity.Education));
                }
                //SNP_INCOME_BASIS
                if (SnpEntity.IncomeBasis != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_INCOME_BASIS", SnpEntity.IncomeBasis));
                if (SnpEntity.HealthIns != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HEALTH_INS", SnpEntity.HealthIns));
                }

                if (SnpEntity.Vet != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_VET", SnpEntity.Vet));
                }

                if (SnpEntity.Disable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_DISABLE", SnpEntity.Disable));
                }

                if (SnpEntity.FootStamps != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_FOOD_STAMPS", SnpEntity.FootStamps));
                }

                if (SnpEntity.Farmer != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_FARMER", SnpEntity.Farmer));
                }
                //SNP_APPL_DATE
                if (SnpEntity.ApplDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_APPL_DATE", SnpEntity.ApplDate));

                //SNP_APPL_TIME
                if (SnpEntity.ApplTime != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_APPL_TIME", SnpEntity.ApplTime));
                //SNP_AMPM
                if (SnpEntity.Ampm != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_AMPM", SnpEntity.Ampm));
                //SNP_INTAKE_DATE
                if (SnpEntity.InitialDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_INTAKE_DATE", SnpEntity.InitialDate));
                //SNP_SITE
                if (SnpEntity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SITE", SnpEntity.Site));
                //SNP_TOT_INCOME
                if (SnpEntity.TotIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_TOT_INCOME", SnpEntity.TotIncome));
                //SNP_PROG_INCOME
                if (SnpEntity.ProgIncome != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PROG_INCOME", SnpEntity.ProgIncome));
                //SNP_CLAIM_SSNO
                if (SnpEntity.ClaimSsno != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLAIM_SSNO", SnpEntity.ClaimSsno));
                //SNP_CLAIM_SS_BIC
                if (SnpEntity.ClaimSsbic != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLAIM_SS_BIC", SnpEntity.ClaimSsbic));
                //SNP_WAGEM
                if (SnpEntity.Wagem != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_WAGEM", SnpEntity.Wagem));
                if (SnpEntity.Wic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_WIC", SnpEntity.Wic));
                }
                //SNP_STUDENT
                if (SnpEntity.Student != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_STUDENT", SnpEntity.Student));

                if (SnpEntity.Resident != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RESIDENT", SnpEntity.Resident));
                }

                if (SnpEntity.Pregnant != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_PREGNANT", SnpEntity.Pregnant));
                }

                if (SnpEntity.MaritalStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MARITAL_STATUS", SnpEntity.MaritalStatus));
                }

                if (SnpEntity.SchoolDistrict != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SCHOOL_DISTRICT", SnpEntity.SchoolDistrict));
                }
                if (SnpEntity.AlienRegNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALIEN_REG_NO", SnpEntity.AlienRegNo));

                if (SnpEntity.LegalTowork != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_LEGAL_TO_WORK", SnpEntity.LegalTowork));
                }
                if (SnpEntity.ExpireWorkDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EXPIRE_WORK_DATE", SnpEntity.ExpireWorkDate));

                //SNP_EMPLOYED
                if (SnpEntity.Employed != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYED", SnpEntity.Employed));
                //SNP_LAST_WORK_DATE
                if (SnpEntity.LastWorkDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_LAST_WORK_DATE", SnpEntity.LastWorkDate));
                //SNP_WORK_LIMIT
                if (SnpEntity.WorkLimit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_WORK_LIMIT", SnpEntity.WorkLimit));
                //SNP_EXPLAIN_WORK_LIMIT
                if (SnpEntity.ExplainWorkLimit != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EXPLAIN_WORK_LIMIT", SnpEntity.ExplainWorkLimit));
                //SNP_NUMBER_OF_C_JOBS
                if (SnpEntity.NumberOfcjobs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NUMBER_OF_C_JOBS", SnpEntity.NumberOfcjobs));
                //SNP_NUMBER_OF_LV_JOBS
                if (SnpEntity.NumberofLvjobs != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NUMBER_OF_LV_JOBS", SnpEntity.NumberofLvjobs));
                //SNP_FULL_TIME_HOURS
                if (SnpEntity.FullTimeHours != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_FULL_TIME_HOURS", SnpEntity.FullTimeHours));
                //SNP_PART_TIME_HOURS
                if (SnpEntity.PartTimeHours != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PART_TIME_HOURS", SnpEntity.PartTimeHours));
                //SNP_SEASONAL_EMPLOY
                if (SnpEntity.SeasonalEmploy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SEASONAL_EMPLOY", SnpEntity.SeasonalEmploy));
                //SNP_1ST_SHIFT
                if (SnpEntity.IstShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_1ST_SHIFT", SnpEntity.IstShift));
                //SNP_2ND_SHIFT
                if (SnpEntity.IIndShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_2ND_SHIFT", SnpEntity.IIndShift));
                //SNP_3RD_SHIFT
                if (SnpEntity.IIIrdShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_3RD_SHIFT", SnpEntity.IIIrdShift));
                //SNP_R_SHIFT
                if (SnpEntity.RShift != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_R_SHIFT", SnpEntity.RShift));
                //SNP_EMPLOYER_NAME
                if (SnpEntity.EmployerName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_NAME", SnpEntity.EmployerName));
                //SNP_EMPLOYER_STREET
                if (SnpEntity.EmployerStreet != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_STREET", SnpEntity.EmployerStreet));
                //SNP_EMPLOYER_CITY
                if (SnpEntity.EmployerCity != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPLOYER_CITY", SnpEntity.EmployerCity));
                //SNP_JOB_TITLE
                if (SnpEntity.JobTitle != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_JOB_TITLE", SnpEntity.JobTitle));
                //SNP_JOB_CATEGORY
                if (SnpEntity.JobCategory != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_JOB_CATEGORY", SnpEntity.JobCategory));
                //SNP_HOURLY_WAGE
                if (SnpEntity.HourlyWage != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_HOURLY_WAGE", SnpEntity.HourlyWage));
                //SNP_PAY_FREQUENCY
                if (SnpEntity.PayFrequency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PAY_FREQUENCY", SnpEntity.PayFrequency));
                //SNP_HIRE_DATE
                if (SnpEntity.HireDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_HIRE_DATE", SnpEntity.HireDate));
                //SNP_TRANSERV
                if (SnpEntity.Transerv != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_TRANSERV", SnpEntity.Transerv));

                if (SnpEntity.Relitran != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RELITRAN", SnpEntity.Relitran));
                }

                if (SnpEntity.Drvlic != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_DRVLIC", SnpEntity.Drvlic));
                }

                if (SnpEntity.Race != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_RACE", SnpEntity.Race));
                }
                //SNP_EMPL_AREA
                if (SnpEntity.EmplPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPL_Phone", SnpEntity.EmplPhone));
                ////SNP_EMPL_EXCH
                //if (SnpEntity.EmplExch != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SNP_EMPL_EXCH", SnpEntity.EmplExch));
                ////SNP_EMPL_TEL
                //if (SnpEntity.EmplTel != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@SNP_EMPL_TEL", SnpEntity.EmplTel));
                //SNP_EMPL_EXT
                if (SnpEntity.EmplExt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EMPL_EXT", SnpEntity.EmplExt));
                if(SnpEntity.DobNa!=string.Empty)
                sqlParamList.Add(new SqlParameter("@SNP_DOB_NA", SnpEntity.DobNa));
                //SNP_SSN_REASON
                if (SnpEntity.SsnReason != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SSN_REASON", SnpEntity.SsnReason));
                sqlParamList.Add(new SqlParameter("@SNP_EXCLUDE", SnpEntity.Exclude));
                //SNP_BLIND
                if (SnpEntity.Blind != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_BLIND", SnpEntity.Blind));
                //SNP_ABLE_TO_WORK
                if (SnpEntity.AbleTowork != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ABLE_TO_WORK", SnpEntity.AbleTowork));
                //SNP_REC_MEDICARE
                if (SnpEntity.RecMedicare != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_REC_MEDICARE", SnpEntity.RecMedicare));
                //SNP_PURCHASE_FOOD
                if (SnpEntity.PurchaseFood != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_PURCHASE_FOOD", SnpEntity.PurchaseFood));
                //SNP_VEHICLE_VALUE
                if (SnpEntity.VechicleValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_VEHICLE_VALUE", SnpEntity.VechicleValue));
                //SNP_OTHER_VEHICLE_VALUE
                if (SnpEntity.OtherVehicleValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_OTHER_VEHICLE_VALUE", SnpEntity.OtherVehicleValue));
                //SNP_OTHER_ASSET_VALUE
                if (SnpEntity.OtherAssetValue != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_OTHER_ASSET_VALUE", SnpEntity.OtherAssetValue));

                if (SnpEntity.SsnSearchType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SsnSearchType", SnpEntity.SsnSearchType));
                if (SnpEntity.AltAgency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_AGENCY", SnpEntity.AltAgency));
                if (SnpEntity.AltDept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_DEPT", SnpEntity.AltDept));
                if (SnpEntity.AltProgram != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_PROGRAM", SnpEntity.AltProgram));
                if (SnpEntity.AltYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_YEAR", SnpEntity.AltYear));
                if (SnpEntity.AltApp != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_APP", SnpEntity.AltApp));
                if (SnpEntity.AltFamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALT_FAMILY_SEQ", SnpEntity.AltFamilySeq));
                if (SnpEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", SnpEntity.Type));

                sqlParamList.Add(new SqlParameter("@SNP_ADD_OPERATOR", SnpEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@SNP_LSTC_OPERATOR", SnpEntity.LstcOperator));
                if (SnpEntity.MilitaryStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_MILITARY_STATUS", SnpEntity.MilitaryStatus));
                }
                if (SnpEntity.Health_Codes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HEALTH_CODES", SnpEntity.Health_Codes));
                }
                if (SnpEntity.WorkStatus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_WORK_STAT", SnpEntity.WorkStatus));
                }
                if (SnpEntity.NonCashBenefits != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_NCASHBEN", SnpEntity.NonCashBenefits));
                }
                if (SnpEntity.Youth != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_YOUTH", SnpEntity.Youth));
                }
                if (SnpEntity.SnpSuffix != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_SUFFIX", SnpEntity.SnpSuffix));
                }
                if (SnpEntity.Snp_HH_ExcludeMem != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@SNP_HH_EXCLUDE", SnpEntity.Snp_HH_ExcludeMem));
                }
                SqlParameter sqlFamilySeqOut = new SqlParameter("@SNP_OUT_FAMILY_SEQ", SqlDbType.VarChar, 10);
                sqlFamilySeqOut.Value = SnpEntity.FamilySeq;
                sqlFamilySeqOut.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlFamilySeqOut);

                boolstatus = CaseSnpData.InsertSNPDETAILSLeanIntake(sqlParamList);
                strNewFamilySeq = sqlFamilySeqOut.Value.ToString();
                if (boolstatus == false)
                {
                    CaseMst.InsertErrorLog("Case2001SNP", ErrorLogSNP(SnpEntity), "Record not modified some error", SnpEntity.LstcOperator);
                }
            }

            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("Case2001SNP", ErrorLogSNP(SnpEntity), ex.Message, SnpEntity.LstcOperator);
                boolstatus = false;
            }
            strOutFamilySeq = strNewFamilySeq;
            return boolstatus;
        }


        /// <summary>
        /// DELETE  CASESNP information. 
        /// </summary>
        public string DeleteCaseSNP(CaseSnpEntity caseSnpEntity,string strUserId)
        {
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SNP_AGENCY", caseSnpEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SNP_DEPT", caseSnpEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SNP_PROGRAM", caseSnpEntity.Program));
                sqlParamList.Add(new SqlParameter("@SNP_YEAR", caseSnpEntity.Year));
                sqlParamList.Add(new SqlParameter("@SNP_APP", caseSnpEntity.App));
                sqlParamList.Add(new SqlParameter("@SNP_FAMILY_SEQ", caseSnpEntity.FamilySeq));
                sqlParamList.Add(new SqlParameter("@UserId", caseSnpEntity.LstcOperator));

                SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlmsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlmsg);
                if (CaseSnpData.DeleteCASESNP(sqlParamList))
                {
                    CaseMst.InsertErrorLog("SNPDeleteSucess", ErrorLogSNP(caseSnpEntity), "Record Delete",strUserId);
                }
                strMsg = sqlmsg.Value.ToString();

            }
            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("SNPDeleteError", ErrorLogSNP(caseSnpEntity), ex.Message,strUserId);
                return strMsg;
            }

            return strMsg;
        }

        public bool InsertUpdateADDCUST(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@ACT_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@ACT_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@ACT_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@ACT_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@ACT_APP_NO", custEntity.ACTAPPNO));
                if (custEntity.ACTSNPFAMILYSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_SNP_FAMILY_SEQ", custEntity.ACTSNPFAMILYSEQ));
                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_CODE", custEntity.ACTCODE));
                if (custEntity.ACTRESPSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_RESP_SEQ", custEntity.ACTRESPSEQ));
                if (custEntity.ACTNUMRESP != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_NUM_RESP", custEntity.ACTNUMRESP));
                if (custEntity.ACTALPHARESP != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_ALPHA_RESP", custEntity.ACTALPHARESP));
                if (custEntity.ACTDATERESP != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_DATE_RESP", custEntity.ACTDATERESP));
                if (custEntity.ACTMULTRESP != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_MULT_RESP", custEntity.ACTMULTRESP));
                if (custEntity.lstcoperator != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_LSTC_OPERATOR", custEntity.lstcoperator));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@ACT_ADD_OPERATOR", custEntity.addoperator));
                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                CaseSnpData.InsertUpdateADDCUST(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool InsertUpdateSERCUST(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SER_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@SER_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@SER_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@SER_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@SER_APP_NO", custEntity.ACTAPPNO));
                if (custEntity.ACTSNPFAMILYSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@SER_FUND", custEntity.ACTSNPFAMILYSEQ));
                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@SER_CODE", custEntity.ACTCODE));
                if (custEntity.ACTRESPSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@SER_RESP_SEQ", custEntity.ACTRESPSEQ));
                if (custEntity.ACTNUMRESP != string.Empty) sqlParamList.Add(new SqlParameter("@SER_NUM_RESP", custEntity.ACTNUMRESP));
                if (custEntity.ACTALPHARESP != string.Empty) sqlParamList.Add(new SqlParameter("@SER_ALPHA_RESP", custEntity.ACTALPHARESP));
                if (custEntity.ACTDATERESP != string.Empty) sqlParamList.Add(new SqlParameter("@SER_DATE_RESP", custEntity.ACTDATERESP));
                if (custEntity.ACTMULTRESP != string.Empty) sqlParamList.Add(new SqlParameter("@SER_MULT_RESP", custEntity.ACTMULTRESP));
                if (custEntity.lstcoperator != string.Empty) sqlParamList.Add(new SqlParameter("@SER_LSTC_OPERATOR", custEntity.lstcoperator));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@SER_ADD_OPERATOR", custEntity.addoperator));
                if (custEntity.SER_ELEC != string.Empty) sqlParamList.Add(new SqlParameter("@SER_ELEC", custEntity.SER_ELEC));
                if (custEntity.SER_KWH != string.Empty) sqlParamList.Add(new SqlParameter("@SER_KWH", custEntity.SER_KWH));
                if (custEntity.SER_GAS != string.Empty) sqlParamList.Add(new SqlParameter("@SER_GAS", custEntity.SER_GAS));
                if (custEntity.SER_CCF != string.Empty) sqlParamList.Add(new SqlParameter("@SER_CCF", custEntity.SER_CCF));
                if (custEntity.SER_ANNUAL != string.Empty) sqlParamList.Add(new SqlParameter("@SER_ANNUAL", custEntity.SER_ANNUAL));
                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                CaseSnpData.InsertUpdateSERCUST(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool InsertUpdatePRESRESP(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PRES_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@PRES_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@PRES_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@PRES_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@PRES_APP_NO", custEntity.ACTAPPNO));
                if (custEntity.ACTSNPFAMILYSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_SNP_FAMILY_SEQ", custEntity.ACTSNPFAMILYSEQ));
                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_CODE", custEntity.ACTCODE));
                if (custEntity.ACTRESPSEQ != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_RESP_SEQ", custEntity.ACTRESPSEQ));
                if (custEntity.ACTNUMRESP != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_NUM_RESP", custEntity.ACTNUMRESP));
                if (custEntity.ACTALPHARESP != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_ALPHA_RESP", custEntity.ACTALPHARESP));
                if (custEntity.ACTDATERESP != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_DATE_RESP", custEntity.ACTDATERESP));
                //if (custEntity.ACTMULTRESP != string.Empty) 
                    sqlParamList.Add(new SqlParameter("@PRES_MULT_RESP", custEntity.ACTMULTRESP));
                if (custEntity.lstcoperator != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_LSTC_OPERATOR", custEntity.lstcoperator));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_ADD_OPERATOR", custEntity.addoperator));
                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                if (custEntity.PRESPOINTS != string.Empty) sqlParamList.Add(new SqlParameter("@PRES_POINTS", custEntity.PRESPOINTS));
                CaseSnpData.InsertUpdatePRESRESP(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool InsertUpdateDIMSCORE(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@DIMSCOR_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@DIMSCOR_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@DIMSCOR_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@DIMSCOR_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@DIMSCOR_APP_NO", custEntity.ACTAPPNO));
                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@DIMSCOR_CODE", custEntity.ACTCODE));
                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                if (custEntity.PRESPOINTS != string.Empty) sqlParamList.Add(new SqlParameter("@DIMSCOR_SCORE", custEntity.PRESPOINTS));
                if (custEntity.lstcoperator != string.Empty) sqlParamList.Add(new SqlParameter("@DIMSCOR_LSTC_OPERATOR", custEntity.lstcoperator));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@DIMSCOR_ADD_OPERATOR", custEntity.addoperator));
                if (custEntity.CUSTOTHER != string.Empty) sqlParamList.Add(new SqlParameter("@GROUPCODE", custEntity.CUSTOTHER));
                CaseSnpData.InsertUpdateDIMSCORE(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool CAPS_INTKINCOMP_INSUPDEL(CustomQuestionsEntity custEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@INTK_AGENCY", custEntity.ACTAGENCY));
                sqlParamList.Add(new SqlParameter("@INTK_DEPT", custEntity.ACTDEPT));
                sqlParamList.Add(new SqlParameter("@INTK_PROGRAM", custEntity.ACTPROGRAM));
                sqlParamList.Add(new SqlParameter("@INTK_YEAR", custEntity.ACTYEAR));
                sqlParamList.Add(new SqlParameter("@INTK_APP", custEntity.ACTAPPNO));
                if (custEntity.ACTCODE != string.Empty) sqlParamList.Add(new SqlParameter("@INTK_INCOMP", custEntity.ACTCODE));
                if (custEntity.ACTALPHARESP != string.Empty) sqlParamList.Add(new SqlParameter("@INTK_DETAILS", custEntity.ACTALPHARESP));

                if (custEntity.Mode != string.Empty) sqlParamList.Add(new SqlParameter("@Mode", custEntity.Mode));
                if (custEntity.addoperator != string.Empty) sqlParamList.Add(new SqlParameter("@INTK_OPERATOR", custEntity.addoperator));

                if (custEntity.LetterDate.Trim() != string.Empty) sqlParamList.Add(new SqlParameter("@INTK_LETTER_DATE", custEntity.LetterDate));
                if (custEntity.INTKWorker.Trim() != string.Empty) sqlParamList.Add(new SqlParameter("@INTK_WORKER", custEntity.INTKWorker));

                CaseSnpData.CAPS_INTKINCOMP_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        //internal List<CaseMstSnpEntity> GetSsnSearch(string SearchCat, string SearchFor, string Ssn, string Fname, string Lname, string Alias, string Phone,
        //                                     string HNo, string Street, string City, string State)
        //{
        //    List<CaseMstSnpEntity> CaseMstSnp = new List<CaseMstSnpEntity>();
        //    try
        //    {
        //        DataSet SsnData = Captain.DatabaseLayer.SSN_Search.MainMenuSearch(SearchCat, SearchFor, Ssn, Fname, Lname, Alias, Phone, HNo, Street, City, State);
        //        if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in SsnData.Tables[0].Rows)
        //            {
        //                //CaseMstSnp.Add(new CaseMstSnpEntity(row));
        //                CaseMstSnp.Add(new CaseMstSnpEntity(row));
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return CaseMstSnp;
        //    }

        //    return CaseMstSnp;
        //}

        public List<CaseMstSnpEntity> GetSSNSearch(string searchFor, string SSN, string firstName, string lastName, string HN, string street, string city, string state, string phone, string alias, string isDuplicate, string Agency, string Dept, string Prog, string Year, string userName, string Hierachy,string strDob)
        {
            List<CaseMstSnpEntity> CaseMstSnp = new List<CaseMstSnpEntity>();
            try
            {
                DataSet SsnData = Captain.DatabaseLayer.CaseSnpData.SSNSearch(searchFor, SSN, firstName, lastName, HN, street, city, state, phone, alias, isDuplicate, Agency, Dept, Prog, Year, userName, Hierachy,strDob);
                if (SsnData != null && SsnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SsnData.Tables[0].Rows)
                    {
                        CaseMstSnp.Add(new CaseMstSnpEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return CaseMstSnp;
            }

            return CaseMstSnp;
        }

        public CaseSnpEntity GetCaseSnpDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            CaseSnpEntity CaseSnpDetails = null;
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSnpData.GetCaseSnpDetails(agency, dep, program, year, app, seq);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    CaseSnpDetails = new CaseSnpEntity(CaseSnp.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public List<CaseSnpEntity> GetCaseSnpDetails(string agency, string dep, string program, string year, string app)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSnpData.GetCaseSnpDetails(agency, dep, program, year, app, string.Empty);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public DataSet GetCaseSnpDetailsDataset(string agency, string dep, string program, string year, string app)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Captain.DatabaseLayer.CaseSnpData.GetCaseSnpDetails(agency, dep, program, year, app, string.Empty);

            }
            catch (Exception ex)
            {
                //
                return ds;
            }

            return ds;
        }

        public CaseMstEntity GetCaseMST(string agency, string dep, string program, string year, string app)
        {
            CaseMstEntity caseMstEntity = null;
            try
            {
                DataSet CaseMST = Captain.DatabaseLayer.CaseSnpData.GetCaseMST(agency, dep, program, year, app);
                if (CaseMST != null && CaseMST.Tables[0].Rows.Count > 0)
                {
                    caseMstEntity = new CaseMstEntity(CaseMST.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return caseMstEntity;
            }

            return caseMstEntity;
        }

        public List<CaseMstEntity> GetCaseMst()
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();
            try
            {
                DataSet CaseMstData = CaseMst.GetCASEMST();
                if (CaseMstData != null && CaseMstData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstData.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;
        }

        public List<CaseMstEntity> GetCaseMstAll(string Agency, string Dept, string Program, string Year, string AppNo, string ssnNo, string strName, string strSite, string strType, string strTable)
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();
            try
            {
                DataSet CaseMstData = CaseMst.GetCASEMSTALL(Agency, Dept, Program, Year, AppNo, ssnNo, strName, strSite, strType);
                if (CaseMstData != null && CaseMstData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstData.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;
        }

        public List<CaseMstEntity> GetCasbLTRBAll(string Agency, string Dept, string Program, string Year, string Worker, string strSite, string Bundle)
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();
            try
            {
                DataSet CaseMstData = CaseMst.GetCASBLTRB(Agency, Dept, Program, Year, Worker, strSite, Bundle);
                if (CaseMstData != null && CaseMstData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstData.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row, "MSTALLSNP"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;
        }


        public List<CaseServicesEntity> GetCaseServices()
        {
            List<CaseServicesEntity> CaseSerProfile = new List<CaseServicesEntity>();
            try
            {
                DataSet CaseSerData = CaseMst.GetSelectServices();
                if (CaseSerData != null && CaseSerData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSerData.Tables[0].Rows)
                    {
                        CaseSerProfile.Add(new CaseServicesEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSerProfile;
            }

            return CaseSerProfile;
        }


        public List<CaseMstEntity> GetCaseMstadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();
            try
            {
                DataSet CaseMstData = CaseMst.GetCASEMSTadpyn(Agency, Dept, Program, Year, AppNo);
                if (CaseMstData != null && CaseMstData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstData.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;
        }

        public List<CaseSnpEntity> GetCaseSnp()
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnpData = CaseMst.GetCASESNP();
                if (CaseSnpData != null && CaseSnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpData.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public List<CaseSnpEntity> GetCaseSnpadpyn(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnpData = CaseMst.GetCASESNPadpyn(Agency, Dept, Program, Year, AppNo);
                if (CaseSnpData != null && CaseSnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpData.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public DataSet GetCaseSnpadpynDataset(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            DataSet CaseSnpData = new DataSet();
            try
            {
                CaseSnpData = CaseMst.GetCASESNPadpyn(Agency, Dept, Program, Year, AppNo);

            }
            catch (Exception ex)
            {
                //
                return CaseSnpData;
            }

            return CaseSnpData;
        }

        public List<CaseSnpEntity> GetCaseSnpMstadpyn(string Agency, string Dept, string Program, string Year, string AppNo, string strTable)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnpData = CaseMst.GetCASESNPMSTadpyn(Agency, Dept, Program, Year, AppNo);
                if (CaseSnpData != null && CaseSnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpData.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public List<CaseSnpEntity> GetCaseSnpReportcase2530(string Agency, string Dept, string Program, string Year, string AppNo, string strApplicatDetails)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnpData = CaseMst.GetCASESNPadpyncase2530Report(Agency, Dept, Program, Year, AppNo, strApplicatDetails);
                if (CaseSnpData != null && CaseSnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpData.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row, "case2530Report"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public List<CaseMstEntity> GetCaseMstReportcase2530(string Agency, string Dept, string Program, string Year, string AppNo, string securty, string orderby, string Type,string CaseType, string Fromdate, string Todate)
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();
            try
            {
                DataSet CaseMstData = CaseMst.GetMstSnpCase2530Report(Agency, Dept, Program, Year, AppNo, securty, orderby, Type, CaseType, Fromdate, Todate);
                if (CaseMstData != null && CaseMstData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstData.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row, "case2530Report"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;
        }

        //public List<CaseIncomeEntity> GetCaseIncome()
        //{
        //    List<CaseIncomeEntity> CaseIncomeProfile = new List<CaseIncomeEntity>();
        //    try
        //    {
        //        DataSet CaseIncomeData = CaseMst.GetCASEINCOME();
        //        if (CaseIncomeData != null && CaseIncomeData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in CaseIncomeData.Tables[0].Rows)
        //            {
        //                CaseIncomeProfile.Add(new CaseIncomeEntity(row, string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return CaseIncomeProfile;
        //    }

        //    return CaseIncomeProfile;
        //}

        public List<CaseIncomeEntity> GetCaseIncomeadpynf(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo)
        {
            List<CaseIncomeEntity> CaseIncomeProfile = new List<CaseIncomeEntity>();
            try
            {
                DataSet CaseIncomeData = CaseMst.GetCASEINCOMEadpynf(Agency, Dept, Program, Year, AppNo, FamilySeqNo);
                if (CaseIncomeData != null && CaseIncomeData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseIncomeData.Tables[0].Rows)
                    {
                        CaseIncomeProfile.Add(new CaseIncomeEntity(row, "I"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseIncomeProfile;
            }

            return CaseIncomeProfile;
        }

        public List<CaseIncomeEntity> GetCaseIncomeadpynfs(string Agency, string Dept, string Program, string Year, string AppNo, string FamilySeqNo, string Seq)
        {
            List<CaseIncomeEntity> CaseIncomeProfile = new List<CaseIncomeEntity>();
            try
            {
                DataSet CaseIncomeData = CaseMst.GetCASEINCOMEadpynfs(Agency, Dept, Program, Year, AppNo, FamilySeqNo, Seq);
                if (CaseIncomeData != null && CaseIncomeData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseIncomeData.Tables[0].Rows)
                    {
                        CaseIncomeProfile.Add(new CaseIncomeEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseIncomeProfile;
            }

            return CaseIncomeProfile;
        }

        public List<HierarchyEntity> GetCaseWorker(string Type, string strAgency, string strDept, string strProg)
        {
            List<HierarchyEntity> hierachyentity = new List<HierarchyEntity>();
            try
            {
                DataSet caseWorker = CaseMst.GetCaseWorker(Type, strAgency, strDept, strProg);
                if (caseWorker != null && caseWorker.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in caseWorker.Tables[0].Rows)
                    {
                        hierachyentity.Add(new HierarchyEntity(row, string.Empty, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return hierachyentity;
            }

            return hierachyentity;
        }

        public List<CustomQuestionsEntity> GetCustomQuestionAnswers(CaseSnpEntity caseSNPEntity)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                if (caseSNPEntity != null)
                {
                    DataSet custResponse = CaseSnpData.GetCustomQuestionAnswers(caseSNPEntity.Agency, caseSNPEntity.Dept, caseSNPEntity.Program, caseSNPEntity.Year, caseSNPEntity.App, caseSNPEntity.FamilySeq, string.Empty);
                    if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in custResponse.Tables[0].Rows)
                        {
                            customEntity.Add(new CustomQuestionsEntity(row, "Response"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }


        public List<CustomQuestionsEntity> GetSERCustomQuestionAnswers(string agency, string dep, string program, string year, string app, string fund, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GetSERCustomQuestionAnswers(agency, dep, program, year, app, fund, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "SERQUES"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }



        public List<CustomQuestionsEntity> GetCustomQuestionAnswers(CaseSnpEntity caseSNPEntity, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GetCustomQuestionAnswers(caseSNPEntity.Agency, caseSNPEntity.Dept, caseSNPEntity.Program, caseSNPEntity.Year, caseSNPEntity.App, caseSNPEntity.FamilySeq, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "Response"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }

        public List<CustomQuestionsEntity> GetPreassesQuestionAnswers(CaseSnpEntity caseSNPEntity, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GetCustomQuestionAnswers(caseSNPEntity.Agency, caseSNPEntity.Dept, caseSNPEntity.Program, caseSNPEntity.Year, caseSNPEntity.App, caseSNPEntity.FamilySeq, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "PREASSES"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }

        public List<CustomQuestionsEntity> GetCustomQuestionAnswersRank(string Agency, string Dept, string Program, string Year, string Appl, string FamilSeq, string strApplicantDetais, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GetCustomQuestionAnswersRank(Agency, Dept, Program, Year, Appl, FamilSeq, strApplicantDetais, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "case2530Report"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }

        public List<CustomQuestionsEntity> GetPreassesQuestionAnswersRank(string Agency, string Dept, string Program, string Year, string Appl, string FamilSeq, string strApplicantDetais, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GetCustomQuestionAnswersRank(Agency, Dept, Program, Year, Appl, FamilSeq, strApplicantDetais, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "Pres2530Report"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }


        public List<CustomQuestionsEntity> GETPREACTIVEQUESXML(string Agency, string Dept, string Program, string Year, string strType)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.GETPREACTIVEQUESXML(Agency, Dept, Program, Year, strType);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "PREQUES"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }


        public bool InsertCaseMst(CaseMstEntity CaseMst)
        {
            bool boolsuccess;

            //try
            //{
            //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_AGENCY", CaseMst.GdlApplyAgency));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_DEPT", CaseMst.GdlApplDept));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_PROGRAM", CaseMst.GdlApplProgram));
            //    sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseMst.GdlType));
            //    if (CaseMst.GdlCounty != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseMst.GdlCounty));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseMst.GdlStartDate));
            //    sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseMst.GdlEndDate));
            //    if (CaseMst.GdlNoHouseHolds != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseMst.GdlNoHouseHolds));
            //    }
            //    if (CaseMst.Gdl1Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseMst.Gdl1Value));
            //    }
            //    if (CaseMst.Gdl2Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseMst.Gdl2Value));
            //    }
            //    if (CaseMst.Gdl3Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseMst.Gdl3Value));
            //    }
            //    if (CaseMst.Gdl4Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseMst.Gdl4Value));
            //    }
            //    if (CaseMst.Gdl5Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseMst.Gdl5Value));
            //    }
            //    if (CaseMst.Gdl6Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseMst.Gdl6Value));
            //    }
            //    if (CaseMst.Gdl7Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseMst.Gdl7Value));
            //    }
            //    if (CaseMst.Gdl8Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseMst.Gdl8Value));
            //    }
            //    if (CaseMst.Gdl9Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseMst.Gdl9Value));
            //    }
            //    if (CaseMst.Gdl10Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseMst.Gdl10Value));
            //    }
            //    if (CaseMst.Gdl11Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseMst.Gdl11Value));
            //    }
            //    if (CaseMst.Gdl12Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseMst.Gdl12Value));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseMst.GdlAddOperator));
            //    sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseMst.GdlLstcOperator));
            //    sqlParamList.Add(new SqlParameter("@Mode", CaseMst.Mode));
            //    boolsuccess = MasterPoverty.InsertCASEGDL(sqlParamList);

            //}
            //catch (Exception ex)
            //{
            //    //
            //    return false;
            boolsuccess = false;
            //}

            return boolsuccess;
        }

        public bool UpdateCaseMst(CaseMstEntity CaseMst)
        {
            bool boolsuccess;

            //try
            //{
            //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_AGENCY", CaseMst.GdlApplyAgency));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_DEPT", CaseMst.GdlApplDept));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_PROGRAM", CaseMst.GdlApplProgram));
            //    sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseMst.GdlType));
            //    if (CaseMst.GdlCounty != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseMst.GdlCounty));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseMst.GdlStartDate));
            //    sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseMst.GdlEndDate));
            //    if (CaseMst.GdlNoHouseHolds != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseMst.GdlNoHouseHolds));
            //    }
            //    if (CaseMst.Gdl1Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseMst.Gdl1Value));
            //    }
            //    if (CaseMst.Gdl2Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseMst.Gdl2Value));
            //    }
            //    if (CaseMst.Gdl3Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseMst.Gdl3Value));
            //    }
            //    if (CaseMst.Gdl4Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseMst.Gdl4Value));
            //    }
            //    if (CaseMst.Gdl5Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseMst.Gdl5Value));
            //    }
            //    if (CaseMst.Gdl6Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseMst.Gdl6Value));
            //    }
            //    if (CaseMst.Gdl7Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseMst.Gdl7Value));
            //    }
            //    if (CaseMst.Gdl8Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseMst.Gdl8Value));
            //    }
            //    if (CaseMst.Gdl9Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseMst.Gdl9Value));
            //    }
            //    if (CaseMst.Gdl10Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseMst.Gdl10Value));
            //    }
            //    if (CaseMst.Gdl11Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseMst.Gdl11Value));
            //    }
            //    if (CaseMst.Gdl12Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseMst.Gdl12Value));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseMst.GdlAddOperator));
            //    sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseMst.GdlLstcOperator));
            //    boolsuccess = MasterPoverty.UpdateCASEGDL(sqlParamList);

            //}
            //catch (Exception ex)
            //{
            //    //
            //    return false;
            boolsuccess = false;
            //}

            return boolsuccess;
        }

        public bool InsertCaseSnp(CaseSnpEntity CaseSnp)
        {
            bool boolsuccess;

            //try
            //{
            //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_AGENCY", CaseSnp.GdlApplyAgency));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_DEPT", CaseSnp.GdlApplDept));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_PROGRAM", CaseSnp.GdlApplProgram));
            //    sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseSnp.GdlType));
            //    if (CaseSnp.GdlCounty != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseSnp.GdlCounty));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseSnp.GdlStartDate));
            //    sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseSnp.GdlEndDate));
            //    if (CaseSnp.GdlNoHouseHolds != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseSnp.GdlNoHouseHolds));
            //    }
            //    if (CaseSnp.Gdl1Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseSnp.Gdl1Value));
            //    }
            //    if (CaseSnp.Gdl2Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseSnp.Gdl2Value));
            //    }
            //    if (CaseSnp.Gdl3Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseSnp.Gdl3Value));
            //    }
            //    if (CaseSnp.Gdl4Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseSnp.Gdl4Value));
            //    }
            //    if (CaseSnp.Gdl5Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseSnp.Gdl5Value));
            //    }
            //    if (CaseSnp.Gdl6Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseSnp.Gdl6Value));
            //    }
            //    if (CaseSnp.Gdl7Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseSnp.Gdl7Value));
            //    }
            //    if (CaseSnp.Gdl8Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseSnp.Gdl8Value));
            //    }
            //    if (CaseSnp.Gdl9Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseSnp.Gdl9Value));
            //    }
            //    if (CaseSnp.Gdl10Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseSnp.Gdl10Value));
            //    }
            //    if (CaseSnp.Gdl11Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseSnp.Gdl11Value));
            //    }
            //    if (CaseSnp.Gdl12Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseSnp.Gdl12Value));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseSnp.GdlAddOperator));
            //    sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseSnp.GdlLstcOperator));
            //    sqlParamList.Add(new SqlParameter("@Mode", CaseSnp.Mode));
            //    boolsuccess = MasterPoverty.InsertCASEGDL(sqlParamList);

            //}
            //catch (Exception ex)
            //{
            //    //
            //    return false;
            boolsuccess = false;
            //}

            return boolsuccess;
        }

        public bool UpdateCaseSnp(CaseSnpEntity CaseSnp)
        {
            bool boolsuccess;

            //try
            //{
            //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_AGENCY", CaseSnp.GdlApplyAgency));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_DEPT", CaseSnp.GdlApplDept));
            //    sqlParamList.Add(new SqlParameter("@GDL_APPL_PROGRAM", CaseSnp.GdlApplProgram));
            //    sqlParamList.Add(new SqlParameter("@GDL_TYPE", CaseSnp.GdlType));
            //    if (CaseSnp.GdlCounty != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_COUNTY", CaseSnp.GdlCounty));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_START_DATE", CaseSnp.GdlStartDate));
            //    sqlParamList.Add(new SqlParameter("@GDL_END_DATE", CaseSnp.GdlEndDate));
            //    if (CaseSnp.GdlNoHouseHolds != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_NO_HOUSEHOLDS", CaseSnp.GdlNoHouseHolds));
            //    }
            //    if (CaseSnp.Gdl1Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_1_VALUE", CaseSnp.Gdl1Value));
            //    }
            //    if (CaseSnp.Gdl2Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_2_VALUE", CaseSnp.Gdl2Value));
            //    }
            //    if (CaseSnp.Gdl3Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_3_VALUE", CaseSnp.Gdl3Value));
            //    }
            //    if (CaseSnp.Gdl4Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_4_VALUE", CaseSnp.Gdl4Value));
            //    }
            //    if (CaseSnp.Gdl5Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_5_VALUE", CaseSnp.Gdl5Value));
            //    }
            //    if (CaseSnp.Gdl6Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_6_VALUE", CaseSnp.Gdl6Value));
            //    }
            //    if (CaseSnp.Gdl7Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_7_VALUE", CaseSnp.Gdl7Value));
            //    }
            //    if (CaseSnp.Gdl8Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_8_VALUE", CaseSnp.Gdl8Value));
            //    }
            //    if (CaseSnp.Gdl9Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_9_VALUE", CaseSnp.Gdl9Value));
            //    }
            //    if (CaseSnp.Gdl10Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_10_VALUE", CaseSnp.Gdl10Value));
            //    }
            //    if (CaseSnp.Gdl11Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_11_VALUE", CaseSnp.Gdl11Value));
            //    }
            //    if (CaseSnp.Gdl12Value != string.Empty)
            //    {
            //        sqlParamList.Add(new SqlParameter("@GDL_12_VALUE", CaseSnp.Gdl12Value));
            //    }
            //    sqlParamList.Add(new SqlParameter("@GDL_ADD_OPERATOR", CaseSnp.GdlAddOperator));
            //    sqlParamList.Add(new SqlParameter("@GDL_LSTC_OPERATOR", CaseSnp.GdlLstcOperator));
            //    boolsuccess = MasterPoverty.UpdateCASEGDL(sqlParamList);

            //}
            //catch (Exception ex)
            //{
            //    //
            //    return false;
            boolsuccess = false;
            //}

            return boolsuccess;
        }

        public bool InsertCaseIncome(CaseIncomeEntity CaseIncome)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@INCOME_AGENCY", CaseIncome.Agency));
                sqlParamList.Add(new SqlParameter("@INCOME_DEPT", CaseIncome.Dept));
                sqlParamList.Add(new SqlParameter("@INCOME_PROGRAM", CaseIncome.Program));
                if (CaseIncome.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_YEAR", CaseIncome.Year));
                }
                sqlParamList.Add(new SqlParameter("@INCOME_APP", CaseIncome.App));
                sqlParamList.Add(new SqlParameter("@INCOME_FAMILY_SEQ", CaseIncome.FamilySeq));
                if (CaseIncome.Seq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_SEQ", CaseIncome.Seq));
                }
                if (CaseIncome.Exclude != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_EXCLUDE", CaseIncome.Exclude));
                }
                if (CaseIncome.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_TYPE", CaseIncome.Type));
                }
                if (CaseIncome.Interval != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_INTERVAL", CaseIncome.Interval));
                }
                if (CaseIncome.Val1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL1", CaseIncome.Val1));
                }
                if (CaseIncome.Date1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE1", CaseIncome.Date1));
                }
                if (CaseIncome.Val2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL2", CaseIncome.Val2));
                }
                if (CaseIncome.Date2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE2", CaseIncome.Date2));
                }
                if (CaseIncome.Val3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL3", CaseIncome.Val3));
                }
                if (CaseIncome.Date3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE3", CaseIncome.Date3));
                }
                if (CaseIncome.Val4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL4", CaseIncome.Val4));
                }
                if (CaseIncome.Date4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE4", CaseIncome.Date4));
                }
                if (CaseIncome.Val5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL5", CaseIncome.Val5));
                }
                if (CaseIncome.Date5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE5", CaseIncome.Date5));
                }
                if (CaseIncome.Factor != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_FACTOR", CaseIncome.Factor));
                }
                if (CaseIncome.Source != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_SOURCE", CaseIncome.Source));
                }
                if (CaseIncome.Verifier != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VERIFIER", CaseIncome.Verifier));
                }
                if (CaseIncome.HowVerified != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HOW_VERIFIED", CaseIncome.HowVerified));
                }
                if (CaseIncome.TotIncome != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_TOT_INCOME", CaseIncome.TotIncome));
                }
                if (CaseIncome.ProgIncome != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_PROG_INCOME", CaseIncome.ProgIncome));
                }

                sqlParamList.Add(new SqlParameter("@INCOME_LSTC_OPERATOR", CaseIncome.LstcOperator));
                sqlParamList.Add(new SqlParameter("@INCOME_ADD_OPERATOR", CaseIncome.AddOperator));

                if (CaseIncome.HrRate1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HR_RATE1", CaseIncome.HrRate1));
                }
                if (CaseIncome.HrRate2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HR_RATE2", CaseIncome.HrRate2));
                }
                if (CaseIncome.HrRate3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HR_RATE3", CaseIncome.HrRate3));
                }
                if (CaseIncome.HrRate4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HR_RATE4", CaseIncome.HrRate4));
                }
                if (CaseIncome.HrRate5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HR_RATE5", CaseIncome.HrRate5));
                }
                if (CaseIncome.Average != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_AVG", CaseIncome.Average));
                }
                if (CaseIncome.VerMethod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VER_METHOD", CaseIncome.VerMethod));
                }

                sqlParamList.Add(new SqlParameter("@Mode", CaseIncome.Mode));
                boolsuccess = CaseMst.InsertCASEINCOME(sqlParamList);
                if (boolsuccess == false)
                {
                    CaseMst.InsertErrorLog("CaseINCOME", ErrorLogIncome(CaseIncome), "Record not modified some error", CaseIncome.LstcOperator);
                }

            }
            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("CaseINCOME", ErrorLogIncome(CaseIncome), ex.Message, CaseIncome.LstcOperator);
                //
                return false;
            }

            return boolsuccess;
        }



        public string ErrorLogIncome(CaseIncomeEntity CaseIncome)
        {
            StringBuilder strXmbulider = new StringBuilder();

            strXmbulider.Append("<TableData>");

            strXmbulider.Append("<FieldName>INCOME_AGENCY =  " + CaseIncome.Agency + "</FieldName>");
            strXmbulider.Append("<FieldName>INCOME_DEPT =  " + CaseIncome.Dept + "</FieldName>");
            strXmbulider.Append("<FieldName>INCOME_PROGRAM =  " + CaseIncome.Program + "</FieldName>");
            if (CaseIncome.Year != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_YEAR =  " + CaseIncome.Year + "</FieldName>");
            }
            strXmbulider.Append("<FieldName>INCOME_APP =  " + CaseIncome.App + "</FieldName>");
            strXmbulider.Append("<FieldName>INCOME_FAMILY_SEQ =  " + CaseIncome.FamilySeq + "</FieldName>");
            if (CaseIncome.Seq != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_SEQ =  " + CaseIncome.Seq + "</FieldName>");
            }
            if (CaseIncome.Exclude != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_EXCLUDE =  " + CaseIncome.Exclude + "</FieldName>");
            }
            if (CaseIncome.Type != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_TYPE =  " + CaseIncome.Type + "</FieldName>");
            }
            if (CaseIncome.Interval != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_INTERVAL =  " + CaseIncome.Interval + "</FieldName>");
            }
            if (CaseIncome.Val1 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VAL1 =  " + CaseIncome.Val1 + "</FieldName>");
            }
            if (CaseIncome.Date1 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_DATE1 =  " + CaseIncome.Date1 + "</FieldName>");
            }
            if (CaseIncome.Val2 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VAL2 =  " + CaseIncome.Val2 + "</FieldName>");
            }
            if (CaseIncome.Date2 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_DATE2 =  " + CaseIncome.Date2 + "</FieldName>");
            }
            if (CaseIncome.Val3 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VAL3 =  " + CaseIncome.Val3 + "</FieldName>");
            }
            if (CaseIncome.Date3 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_DATE3 =  " + CaseIncome.Date3 + "</FieldName>");
            }
            if (CaseIncome.Val4 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VAL4 =  " + CaseIncome.Val4 + "</FieldName>");
            }
            if (CaseIncome.Date4 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_DATE4 =  " + CaseIncome.Date4 + "</FieldName>");
            }
            if (CaseIncome.Val5 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VAL5 =  " + CaseIncome.Val5 + "</FieldName>");
            }
            if (CaseIncome.Date5 != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_DATE5 =  " + CaseIncome.Date5 + "</FieldName>");
            }
            if (CaseIncome.Factor != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_FACTOR =  " + CaseIncome.Factor + "</FieldName>");
            }
            if (CaseIncome.Source != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_SOURCE =  " + CaseIncome.Source + "</FieldName>");
            }
            if (CaseIncome.Verifier != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_VERIFIER =  " + CaseIncome.Verifier + "</FieldName>");
            }
            if (CaseIncome.HowVerified != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_HOW_VERIFIED =  " + CaseIncome.HowVerified + "</FieldName>");
            }
            if (CaseIncome.TotIncome != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_TOT_INCOME =  " + CaseIncome.TotIncome + "</FieldName>");
            }
            if (CaseIncome.ProgIncome != string.Empty)
            {
                strXmbulider.Append("<FieldName>INCOME_PROG_INCOME =  " + CaseIncome.ProgIncome + "</FieldName>");
            }

            strXmbulider.Append("<FieldName>INCOME_LSTC_OPERATOR =  " + CaseIncome.LstcOperator + "</FieldName>");
            strXmbulider.Append("<FieldName>INCOME_ADD_OPERATOR =  " + CaseIncome.AddOperator + "</FieldName>");
            strXmbulider.Append("<FieldName>Mode =  " + CaseIncome.Mode + "</FieldName>");

            strXmbulider.Append("<FieldName>DateTime =  " + DateTime.Now.Date.ToString() + "</FieldName>");

            strXmbulider.Append("</TableData>");

            return strXmbulider.ToString();
        }

        public bool UpdateCaseIncome(CaseIncomeEntity CaseIncome)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@INCOME_AGENCY", CaseIncome.Agency));
                sqlParamList.Add(new SqlParameter("@INCOME_DEPT", CaseIncome.Dept));
                sqlParamList.Add(new SqlParameter("@INCOME_PROGRAM", CaseIncome.Program));
                sqlParamList.Add(new SqlParameter("@INCOME_YEAR", CaseIncome.Year));

                sqlParamList.Add(new SqlParameter("@INCOME_APP", CaseIncome.App));
                sqlParamList.Add(new SqlParameter("@INCOME_FAMILY_SEQ", CaseIncome.FamilySeq));
                if (CaseIncome.Seq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_SEQ", CaseIncome.Seq));
                }
                if (CaseIncome.Exclude != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_EXCLUDE", CaseIncome.Exclude));
                }
                if (CaseIncome.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_TYPE", CaseIncome.Type));
                }
                if (CaseIncome.Interval != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_INTERVAL", CaseIncome.Interval));
                }
                if (CaseIncome.Val1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL1", CaseIncome.Val1));
                }
                if (CaseIncome.Date1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE1", CaseIncome.Date1));
                }
                if (CaseIncome.Val2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL2", CaseIncome.Val2));
                }
                if (CaseIncome.Date2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE2", CaseIncome.Date2));
                }
                if (CaseIncome.Val3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL3", CaseIncome.Val3));
                }
                if (CaseIncome.Date3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE3", CaseIncome.Date3));
                }
                if (CaseIncome.Val4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL4", CaseIncome.Val4));
                }
                if (CaseIncome.Date4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE4", CaseIncome.Date4));
                }
                if (CaseIncome.Val5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VAL5", CaseIncome.Val5));
                }
                if (CaseIncome.Date5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_DATE5", CaseIncome.Date5));
                }
                if (CaseIncome.Factor != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_FACTOR", CaseIncome.Factor));
                }
                if (CaseIncome.Source != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_SOURCE", CaseIncome.Source));
                }
                if (CaseIncome.Verifier != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_VERIFIER", CaseIncome.Verifier));
                }
                if (CaseIncome.HowVerified != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_HOW_VERIFIED", CaseIncome.HowVerified));
                }
                if (CaseIncome.TotIncome != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_TOT_INCOME", CaseIncome.TotIncome));
                }
                if (CaseIncome.ProgIncome != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_PROG_INCOME", CaseIncome.ProgIncome));
                }

                sqlParamList.Add(new SqlParameter("@INCOME_LSTC_OPERATOR", CaseIncome.LstcOperator));

                sqlParamList.Add(new SqlParameter("@INCOME_DATE_LSTC", CaseIncome.DateLstc));
                sqlParamList.Add(new SqlParameter("@INCOME_ADD_OPERATOR", CaseIncome.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", CaseIncome.Mode));
                boolsuccess = CaseMst.UpdateCASEINCOME(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool DeleteCaseIncome(CaseIncomeEntity CaseIncome)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@INCOME_AGENCY", CaseIncome.Agency));
                sqlParamList.Add(new SqlParameter("@INCOME_DEPT", CaseIncome.Dept));
                sqlParamList.Add(new SqlParameter("@INCOME_PROGRAM", CaseIncome.Program));
                if (CaseIncome.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_YEAR", CaseIncome.Year));
                }
                sqlParamList.Add(new SqlParameter("@INCOME_APP", CaseIncome.App));
                sqlParamList.Add(new SqlParameter("@INCOME_FAMILY_SEQ", CaseIncome.FamilySeq));
                if (CaseIncome.Seq != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@INCOME_SEQ", CaseIncome.Seq));
                }

                boolsuccess = CaseMst.DeleteCASEINCOME(sqlParamList);
                if (boolsuccess == false)
                {
                    CaseMst.InsertErrorLog("CaseINCOMEDelete", ErrorLogIncome(CaseIncome), "Recore not modified some error",CaseIncome.LstcOperator);
                }

            }
            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("CaseINCOMEDelete", ErrorLogIncome(CaseIncome), ex.Message, CaseIncome.LstcOperator);
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateDelCaseDiff(CaseDiffEntity CaseDiff)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@DIFF_AGENCY", CaseDiff.Agency));
                sqlParamList.Add(new SqlParameter("@DIFF_DEPT", CaseDiff.Dept));
                sqlParamList.Add(new SqlParameter("@DIFF_PROGRAM", CaseDiff.Program));
                if (CaseDiff.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_YEAR", CaseDiff.Year));
                }
                if (CaseDiff.AppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_APP_NO", CaseDiff.AppNo));
                }
                if (CaseDiff.State != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_STATE", CaseDiff.State));
                }
                if (CaseDiff.City != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_CITY", CaseDiff.City));
                }
                if (CaseDiff.Street != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_STREET", CaseDiff.Street));
                }
                if (CaseDiff.Suffix != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_SUFFIX", CaseDiff.Suffix));
                }
                if (CaseDiff.Hn != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_HN", CaseDiff.Hn));
                }
                if (CaseDiff.Apt != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_APT", CaseDiff.Apt));
                }
                if (CaseDiff.Flr != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_FLR", CaseDiff.Flr));
                }
                if (CaseDiff.Zip != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_ZIP", CaseDiff.Zip));
                }
                if (CaseDiff.ZipPlus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_ZIPPLUS", CaseDiff.ZipPlus));
                }
                if (CaseDiff.Direction != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_DIRECTION", CaseDiff.Direction));
                }
                if (CaseDiff.IncareFirst != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_INCARE_FIRST", CaseDiff.IncareFirst));
                }
                if (CaseDiff.IncareLast != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_INCARE_LAST", CaseDiff.IncareLast));
                }
                if (CaseDiff.County != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_COUNTY", CaseDiff.County));
                }
                if (CaseDiff.Phone != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DIFF_PHONE", CaseDiff.Phone));
                }
                sqlParamList.Add(new SqlParameter("@DIFF_ADD_OPERATOR", CaseDiff.AddOperator));
                sqlParamList.Add(new SqlParameter("@DIFF_LSTC_OPERATOR", CaseDiff.LstcOperator));

                boolsuccess = CaseMst.InsertUpdateDelCASEDiff(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateDelLandlord(CaseDiffEntity CaseDiff)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@LLR_AGENCY", CaseDiff.Agency));
                sqlParamList.Add(new SqlParameter("@LLR_DEPT", CaseDiff.Dept));
                sqlParamList.Add(new SqlParameter("@LLR_PROGRAM", CaseDiff.Program));
                if (CaseDiff.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_YEAR", CaseDiff.Year));
                }
                if (CaseDiff.AppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_APP_NO", CaseDiff.AppNo));
                }
                if (CaseDiff.State != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_STATE", CaseDiff.State));
                }
                if (CaseDiff.City != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_CITY", CaseDiff.City));
                }
                if (CaseDiff.Street != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_STREET", CaseDiff.Street));
                }
                if (CaseDiff.Suffix != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_SUFFIX", CaseDiff.Suffix));
                }
                if (CaseDiff.Hn != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_HN", CaseDiff.Hn));
                }
                if (CaseDiff.Apt != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_APT", CaseDiff.Apt));
                }
                if (CaseDiff.Flr != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_FLR", CaseDiff.Flr));
                }
                if (CaseDiff.Zip != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_ZIP", CaseDiff.Zip));
                }
                if (CaseDiff.ZipPlus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_ZIPPLUS", CaseDiff.ZipPlus));
                }
                if (CaseDiff.Direction != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_DIRECTION", CaseDiff.Direction));
                }
                if (CaseDiff.IncareFirst != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_INCARE_FIRST", CaseDiff.IncareFirst));
                }
                if (CaseDiff.IncareLast != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_INCARE_LAST", CaseDiff.IncareLast));
                }
                if (CaseDiff.County != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_COUNTY", CaseDiff.County));
                }
                if (CaseDiff.Phone != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@LLR_PHONE", CaseDiff.Phone));
                }
                sqlParamList.Add(new SqlParameter("@LLR_ADD_OPERATOR", CaseDiff.AddOperator));
                sqlParamList.Add(new SqlParameter("@LLR_LSTC_OPERATOR", CaseDiff.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", CaseDiff.Mode));

                boolsuccess = CaseMst.InsertUpdateDelLandlord(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateDelMSTSER(CaseMSTSER CaseMSTSER)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@MSTSER_AGENCY", CaseMSTSER.Agency));
                sqlParamList.Add(new SqlParameter("@MSTSER_DEPT", CaseMSTSER.Dept));
                sqlParamList.Add(new SqlParameter("@MSTSER_PROGRAM", CaseMSTSER.Program));
                if (CaseMSTSER.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@MSTSER_YEAR", CaseMSTSER.Year));
                }
                if (CaseMSTSER.AppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@MSTSER_APP_NO", CaseMSTSER.AppNo));
                }
                if (CaseMSTSER.Service != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@MSTSER_SERVICE", CaseMSTSER.Service));
                }
                if (CaseMSTSER.Mode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@Mode", CaseMSTSER.Mode));
                }
                sqlParamList.Add(new SqlParameter("@MSTSER_ADD_OPERATOR", CaseMSTSER.AddOperator));
                sqlParamList.Add(new SqlParameter("@MSTSER_LSTC_OPERATOR", CaseMSTSER.LstcOperator));

                boolsuccess = CaseMst.InsertUpdateDelMSTSER(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public CaseDiffEntity GetCaseDiffadpya(string Agency, string Dept, string Program, string Year, string AppNo, string strTabletype)
        {

            CaseDiffEntity CaseDiffProfile = null;
            try
            {
                DataSet CaseDiffData = CaseMst.GetCASEDiffadpya(Agency, Dept, Program, Year, AppNo);
                if (CaseDiffData != null && CaseDiffData.Tables[0].Rows.Count > 0)
                {
                    CaseDiffProfile = new CaseDiffEntity(CaseDiffData.Tables[0].Rows[0], strTabletype);
                }
            }
            catch (Exception ex)
            {
                //
                return CaseDiffProfile;
            }

            return CaseDiffProfile;
        }

        public CaseDiffEntity GetLandlordadpya(string Agency, string Dept, string Program, string Year, string AppNo, string strTabletype)
        {

            CaseDiffEntity CaseDiffProfile = null;
            try
            {
                DataSet CaseDiffData = CaseMst.GetLandlordadpya(Agency, Dept, Program, Year, AppNo);
                if (CaseDiffData != null && CaseDiffData.Tables[0].Rows.Count > 0)
                {
                    CaseDiffProfile = new CaseDiffEntity(CaseDiffData.Tables[0].Rows[0], strTabletype);
                }
            }
            catch (Exception ex)
            {
                //
                return CaseDiffProfile;
            }

            return CaseDiffProfile;
        }

        public bool InsertUpdateDelCaseVer(CaseVerEntity CaseVer, out string strMsg)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@VER_AGENCY", CaseVer.Agency));
                sqlParamList.Add(new SqlParameter("@VER_DEPT", CaseVer.Dept));
                sqlParamList.Add(new SqlParameter("@VER_PROGRAM", CaseVer.Program));
                if (CaseVer.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_YEAR", CaseVer.Year));
                }
                if (CaseVer.AppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_APP_NO", CaseVer.AppNo));
                }
                if (CaseVer.VerifyDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_DATE", CaseVer.VerifyDate));
                }
                if (CaseVer.Verifier != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFIER", CaseVer.Verifier));
                }
                if (CaseVer.ReverifyDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_REVERIFY_DATE", CaseVer.ReverifyDate));
                }
                if (CaseVer.VerOmb != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_OMB", CaseVer.VerOmb));
                }
                if (CaseVer.VerHud != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_HUD", CaseVer.VerHud));
                }
                if (CaseVer.VerSmi != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_SMI", CaseVer.VerSmi));
                }
                if (CaseVer.VerCmi != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CMI", CaseVer.VerCmi));
                }
                if (CaseVer.CatElig != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CAT_ELIG", CaseVer.CatElig));
                }
                if (CaseVer.VerifyW2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_W2", CaseVer.VerifyW2));
                }
                if (CaseVer.VerifyCheckStub != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_CHECK_STUB", CaseVer.VerifyCheckStub));
                }
                if (CaseVer.VerifyTaxReturn != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_TAX_RETURN", CaseVer.VerifyTaxReturn));
                }
                if (CaseVer.VerifyLetter != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_LETTER", CaseVer.VerifyLetter));
                }
                if (CaseVer.VerifyOther != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_OTHER", CaseVer.VerifyOther));
                }
                if (CaseVer.VerifySelfDecl != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_SELF_DECL", CaseVer.VerifySelfDecl));
                }
                if (CaseVer.IncomeAmount != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_INCOME_AMOUNT", CaseVer.IncomeAmount));
                }
                if (CaseVer.NoInhh != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_NO_INHH", CaseVer.NoInhh));
                }
                if (CaseVer.FundSource != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_FUND_SOURCE", CaseVer.FundSource));
                }
                if (CaseVer.MealElig != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_MEAL_ELIG", CaseVer.MealElig));
                }
                if (CaseVer.Classification != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CLASSIFICATION", CaseVer.Classification));
                }
                if (CaseVer.VerifyOthCMB != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_OTH_CMB", CaseVer.VerifyOthCMB));
                }
                sqlParamList.Add(new SqlParameter("@VER_ADD_OPERATOR", CaseVer.AddOperator));
                sqlParamList.Add(new SqlParameter("@VER_LSTC_OPERATOR", CaseVer.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", CaseVer.Mode));
                sqlParamList.Add(new SqlParameter("@MstModify", CaseVer.MstModify));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                boolsuccess = CaseMst.InsertUpdateDelCASEVer(sqlParamList);

                if (boolsuccess == false)
                {
                    CaseMst.InsertErrorLog("CaseVER", ErrorLogCaseVer(CaseVer), "Record not modified some error", CaseVer.LstcOperator);
                }
                if (CaseVer.Mode.ToUpper() == "DELETE")
                {
                    CaseMst.InsertErrorLog("CaseVERDelete", ErrorLogCaseVer(CaseVer), "Delete", CaseVer.LstcOperator);
                }
                strMsg = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                CaseMst.InsertErrorLog("CaseVER", ErrorLogCaseVer(CaseVer), ex.Message, CaseVer.LstcOperator);
                strMsg = string.Empty;
                return false;
            }

            return boolsuccess;
        }


        public string ErrorLogCaseVer(CaseVerEntity CaseVer)
        {

            StringBuilder strXmbulider = new StringBuilder();

            strXmbulider.Append("<TableData>");

            strXmbulider.Append("<FieldName>VER_AGENCY =  " + CaseVer.Agency + "</FieldName>");
            strXmbulider.Append("<FieldName>VER_DEPT =  " + CaseVer.Dept + "</FieldName>");
            strXmbulider.Append("<FieldName>VER_PROGRAM =  " + CaseVer.Program + "</FieldName>");
            if (CaseVer.Year != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_YEAR =  " + CaseVer.Year + "</FieldName>");
            }
            if (CaseVer.AppNo != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_APP_NO =  " + CaseVer.AppNo + "</FieldName>");
            }
            if (CaseVer.VerifyDate != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_DATE =  " + CaseVer.VerifyDate + "</FieldName>");
            }
            if (CaseVer.Verifier != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFIER =  " + CaseVer.Verifier + "</FieldName>");
            }
            if (CaseVer.ReverifyDate != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_REVERIFY_DATE =  " + CaseVer.ReverifyDate + "</FieldName>");
            }
            if (CaseVer.VerOmb != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_OMB =  " + CaseVer.VerOmb + "</FieldName>");
            }
            if (CaseVer.VerHud != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_HUD =  " + CaseVer.VerHud + "</FieldName>");
            }
            if (CaseVer.VerSmi != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_SMI =  " + CaseVer.VerSmi + "</FieldName>");
            }
            if (CaseVer.VerCmi != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_CMI =  " + CaseVer.VerCmi + "</FieldName>");
            }
            if (CaseVer.CatElig != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_CAT_ELIG =  " + CaseVer.CatElig + "</FieldName>");
            }
            if (CaseVer.VerifyW2 != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_W2 =  " + CaseVer.VerifyW2 + "</FieldName>");
            }
            if (CaseVer.VerifyCheckStub != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_CHECK_STUB =  " + CaseVer.VerifyCheckStub + "</FieldName>");
            }
            if (CaseVer.VerifyTaxReturn != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_TAX_RETURN =  " + CaseVer.VerifyTaxReturn + "</FieldName>");
            }
            if (CaseVer.VerifyLetter != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_LETTER =  " + CaseVer.VerifyLetter + "</FieldName>");
            }
            if (CaseVer.VerifyOther != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_OTHER =  " + CaseVer.VerifyOther + "</FieldName>");
            }
            if (CaseVer.IncomeAmount != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_INCOME_AMOUNT =  " + CaseVer.IncomeAmount + "</FieldName>");
            }
            if (CaseVer.NoInhh != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_NO_INHH =  " + CaseVer.NoInhh + "</FieldName>");
            }
            if (CaseVer.FundSource != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_FUND_SOURCE =  " + CaseVer.FundSource + "</FieldName>");
            }
            if (CaseVer.MealElig != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_MEAL_ELIG =  " + CaseVer.MealElig + "</FieldName>");
            }
            if (CaseVer.Classification != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_CLASSIFICATION =  " + CaseVer.Classification + "</FieldName>");
            }
            if (CaseVer.VerifyOthCMB != string.Empty)
            {
                strXmbulider.Append("<FieldName>VER_VERIFY_OTH_CMB =  " + CaseVer.VerifyOthCMB + "</FieldName>");
            }
            strXmbulider.Append("<FieldName>VER_ADD_OPERATOR =  " + CaseVer.AddOperator + "</FieldName>");
            strXmbulider.Append("<FieldName>VER_LSTC_OPERATOR =  " + CaseVer.LstcOperator + "</FieldName>");
            strXmbulider.Append("<FieldName>Mode =  " + CaseVer.Mode + "</FieldName>");
            strXmbulider.Append("<FieldName>MstModify =  " + CaseVer.MstModify + "</FieldName>");

            strXmbulider.Append("<FieldName>DateTime =  " + DateTime.Now.Date.ToString() + "</FieldName>");

            strXmbulider.Append("</TableData>");

            return strXmbulider.ToString();
        }

        public CaseVerEntity GetCaseveradpynd(string Agency, string Dept, string Program, string Year, string AppNo, string verifyDate, string strTabletype)
        {

            CaseVerEntity CaseVerProfile = null;
            try
            {
                DataSet CaseVerData = CaseMst.GetCASEVeradpynd(Agency, Dept, Program, Year, AppNo, verifyDate);
                if (CaseVerData != null && CaseVerData.Tables[0].Rows.Count > 0)
                {
                    CaseVerProfile = new CaseVerEntity(CaseVerData.Tables[0].Rows[0], strTabletype);
                }
            }
            catch (Exception ex)
            {
                //
                return CaseVerProfile;
            }

            return CaseVerProfile;
        }

        public List<CaseVerEntity> GetCASEVeradpyalst(string Agency, string Dept, string Program, string Year, string AppNo, string verifyDate, string strTabletype)
        {
            List<CaseVerEntity> CaseVerProfile = new List<CaseVerEntity>();
            try
            {
                DataSet CaseVerData = CaseMst.GetCASEVeradpynd(Agency, Dept, Program, Year, AppNo, verifyDate);
                if (CaseVerData != null && CaseVerData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseVerData.Tables[0].Rows)
                    {
                        CaseVerProfile.Add(new CaseVerEntity(row, strTabletype));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseVerProfile;
            }

            return CaseVerProfile;

        }

        public List<CaseSnpEntity> GetCaseSnpSSno(string strssnNO,string DOB, string FName,string ProdupSw)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();

            try
            {
                DataSet CaseSnpDataDetails = CaseSnpData.GetCaseSnpSSNO(strssnNO,DOB,FName,ProdupSw);
                if (CaseSnpDataDetails != null && CaseSnpDataDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpDataDetails.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public List<CaseMstEntity> GetCaseMstSSno(string strSsno, string DOB, string FName, string ProdupSw)
        {
            List<CaseMstEntity> CaseMstProfile = new List<CaseMstEntity>();

            try
            {
                DataSet CaseMstDataDetails = CaseMst.GetCaseMstSSNO(strSsno, DOB, FName, ProdupSw);
                if (CaseMstDataDetails != null && CaseMstDataDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseMstDataDetails.Tables[0].Rows)
                    {
                        CaseMstProfile.Add(new CaseMstEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseMstProfile;
            }

            return CaseMstProfile;


        }

        public List<CaseSiteEntity> GetCaseSite(string Agency, string Dept, string Program, string strTable)
        {
            List<CaseSiteEntity> CaseSiteProfile = new List<CaseSiteEntity>();
            try
            {
                DataSet CaseSiteData = CaseMst.GetSiteByHIE(Agency, Dept, Program);
                if (CaseSiteData != null && CaseSiteData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSiteData.Tables[0].Rows)
                    {
                        CaseSiteProfile.Add(new CaseSiteEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSiteProfile;
            }

            return CaseSiteProfile;
        }

        public List<CaseSiteEntity> GetCaseSiteYear(string Agency, string Dept, string Program,string Year, string strTable)
        {
            List<CaseSiteEntity> CaseSiteProfile = new List<CaseSiteEntity>();
            try
            {
                DataSet CaseSiteData = CaseMst.GetSiteByHIEYEAR(Agency, Dept, Program,Year);
                if (CaseSiteData != null && CaseSiteData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSiteData.Tables[0].Rows)
                    {
                        CaseSiteProfile.Add(new CaseSiteEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSiteProfile;
            }

            return CaseSiteProfile;
        }

        public List<CaseSiteEntity> GetCaseSiteAll()
        {
            List<CaseSiteEntity> CaseSiteProfile = new List<CaseSiteEntity>();
            try
            {
                DataSet CaseSiteData = CaseMst.GetSiteall();
                if (CaseSiteData != null && CaseSiteData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSiteData.Tables[0].Rows)
                    {
                        CaseSiteProfile.Add(new CaseSiteEntity(row, "Site"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSiteProfile;
            }

            return CaseSiteProfile;
        }

        public List<SqlParameter> Prepare_CASESite_SqlParameters_List(CaseSiteEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Row_Type));

                sqlParamList.Add(new SqlParameter("@SITE_AGENCY", Entity.SiteAGENCY));
                if (!string.IsNullOrEmpty(Entity.SiteDEPT))
                    sqlParamList.Add(new SqlParameter("@SITE_DEPT", Entity.SiteDEPT));
                if (!string.IsNullOrEmpty(Entity.SitePROG))
                    sqlParamList.Add(new SqlParameter("@SITE_PROG", Entity.SitePROG));
                if (!string.IsNullOrEmpty(Entity.SiteYEAR))
                    sqlParamList.Add(new SqlParameter("@SITE_YEAR", Entity.SiteYEAR));
                if (!string.IsNullOrEmpty(Entity.SiteNUMBER))
                    sqlParamList.Add(new SqlParameter("@SITE_NUMBER", Entity.SiteNUMBER));
                if (!string.IsNullOrEmpty(Entity.SiteROOM))
                    sqlParamList.Add(new SqlParameter("@SITE_ROOM", Entity.SiteROOM));
                if (!string.IsNullOrEmpty(Entity.SiteAM_PM))
                    sqlParamList.Add(new SqlParameter("@SITE_AM_PM", Entity.SiteAM_PM));
                if (!string.IsNullOrEmpty(Entity.SiteNAME))
                    sqlParamList.Add(new SqlParameter("@SITE_NAME", Entity.SiteNAME));
                if (!string.IsNullOrEmpty(Entity.SiteSTREET))
                    sqlParamList.Add(new SqlParameter("@SITE_STREET", Entity.SiteSTREET));
                if (!string.IsNullOrEmpty(Entity.SiteCITY))
                    sqlParamList.Add(new SqlParameter("@SITE_CITY", Entity.SiteCITY));
                if (!string.IsNullOrEmpty(Entity.SiteSTATE))
                    sqlParamList.Add(new SqlParameter("@SITE_STATE", Entity.SiteSTATE));
                if (!string.IsNullOrEmpty(Entity.SiteZIP))
                    sqlParamList.Add(new SqlParameter("@SITE_ZIP", Entity.SiteZIP));

                if (!string.IsNullOrEmpty(Entity.SiteZIP_PLUS))
                    sqlParamList.Add(new SqlParameter("@SITE_ZIP_PLUS", Entity.SiteZIP_PLUS));
                if (!string.IsNullOrEmpty(Entity.SitePHONE))
                    sqlParamList.Add(new SqlParameter("@SITE_PHONE", Entity.SitePHONE));
                if (!string.IsNullOrEmpty(Entity.SiteFAX))
                    sqlParamList.Add(new SqlParameter("@SITE_FAX", Entity.SiteFAX));
                if (!string.IsNullOrEmpty(Entity.SiteOTHER_PHONE))
                    sqlParamList.Add(new SqlParameter("@SITE_OTHER_PHONE", Entity.SiteOTHER_PHONE));
                if (!string.IsNullOrEmpty(Entity.SITE_FUNDED_SLOTS))
                    sqlParamList.Add(new SqlParameter("@SITE_FUNDED_SLOTS", Entity.SITE_FUNDED_SLOTS));
                if (!string.IsNullOrEmpty(Entity.SiteNOCHILD_PLA))
                    sqlParamList.Add(new SqlParameter("@SITE_NOCHILD_PLA", Entity.SiteNOCHILD_PLA));

                if (!string.IsNullOrEmpty(Entity.SiteNOCHILD_WID))
                    sqlParamList.Add(new SqlParameter("@SITE_NOCHILD_WID", Entity.SiteNOCHILD_WID));
                if (!string.IsNullOrEmpty(Entity.SiteNOCHILD_PLA_SN))
                    sqlParamList.Add(new SqlParameter("@SITE_NOCHILD_PLA_SN", Entity.SiteNOCHILD_PLA_SN));
                if (!string.IsNullOrEmpty(Entity.SiteNOCHILD_WID_SN))
                    sqlParamList.Add(new SqlParameter("@SITE_NOCHILD_WID_SN", Entity.SiteNOCHILD_WID_SN));
                if (!string.IsNullOrEmpty(Entity.SiteDIR_CODE))
                    sqlParamList.Add(new SqlParameter("@SITE_DIR_CODE", Entity.SiteDIR_CODE));
                if (!string.IsNullOrEmpty(Entity.SiteDIR_LITERAL))
                    sqlParamList.Add(new SqlParameter("@SITE_DIR_LITERAL", Entity.SiteDIR_LITERAL));
                if (!string.IsNullOrEmpty(Entity.SiteFW_CODE))
                    sqlParamList.Add(new SqlParameter("@SITE_FW_CODE", Entity.SiteFW_CODE));

                if (!string.IsNullOrEmpty(Entity.SiteFW_LITERAL))
                    sqlParamList.Add(new SqlParameter("@SITE_FW_LITERAL", Entity.SiteFW_LITERAL));
                if (!string.IsNullOrEmpty(Entity.SiteDIR_TITLE))
                    sqlParamList.Add(new SqlParameter("@SITE_DIR_TITLE", Entity.SiteDIR_TITLE));
                if (!string.IsNullOrEmpty(Entity.SiteLICENSE_NO))
                    sqlParamList.Add(new SqlParameter("@SITE_LICENSE_NO", Entity.SiteLICENSE_NO));
                if (!string.IsNullOrEmpty(Entity.SiteLICENSE_DATES))
                    sqlParamList.Add(new SqlParameter("@SITE_LICENSE_DATES", Entity.SiteLICENSE_DATES));
                if (!string.IsNullOrEmpty(Entity.SiteCLASS_ROOMS))
                    sqlParamList.Add(new SqlParameter("@SITE_CLASS_ROOMS", Entity.SiteCLASS_ROOMS));
                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_B))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_B", Entity.SiteTEACHERS_B));

                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_AIDS_B))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_AIDS_B", Entity.SiteTEACHERS_AIDS_B));
                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_HEAD_B))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_HEAD_B", Entity.SiteTEACHERS_HEAD_B));
                if (!string.IsNullOrEmpty(Entity.SiteCENTER_AIDS_B))
                    sqlParamList.Add(new SqlParameter("@SITE_CENTER_AIDS_B", Entity.SiteCENTER_AIDS_B));
                if (!string.IsNullOrEmpty(Entity.SiteVAN_DRIVERS_B))
                    sqlParamList.Add(new SqlParameter("@SITE_VAN_DRIVERS_B", Entity.SiteVAN_DRIVERS_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP1_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP1_B", Entity.SitePOS_EXP1_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP2_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP2_B", Entity.SitePOS_EXP2_B));

                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP3_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP3_B", Entity.SitePOS_EXP3_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP4_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP4_B", Entity.SitePOS_EXP4_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP5_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP5_B", Entity.SitePOS_EXP5_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP6_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP6_B", Entity.SitePOS_EXP6_B));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP7_B))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP7_B", Entity.SitePOS_EXP7_B));
                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_A))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_A", Entity.SiteTEACHERS_A));

                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_AIDS_A))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_AIDS_A", Entity.SiteTEACHERS_AIDS_A));
                if (!string.IsNullOrEmpty(Entity.SiteTEACHERS_HEAD_A))
                    sqlParamList.Add(new SqlParameter("@SITE_TEACHERS_HEAD_A", Entity.SiteTEACHERS_HEAD_A));
                if (!string.IsNullOrEmpty(Entity.SiteCENTER_AIDS_A))
                    sqlParamList.Add(new SqlParameter("@SITE_CENTER_AIDS_A", Entity.SiteCENTER_AIDS_A));
                if (!string.IsNullOrEmpty(Entity.SiteVAN_DRIVERS_A))
                    sqlParamList.Add(new SqlParameter("@SITE_VAN_DRIVERS_A", Entity.SiteVAN_DRIVERS_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP1_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP1_A", Entity.SitePOS_EXP1_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP2_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP2_A", Entity.SitePOS_EXP2_A));

                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP3_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP3_A", Entity.SitePOS_EXP3_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP4_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP4_A", Entity.SitePOS_EXP4_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP5_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP5_A", Entity.SitePOS_EXP5_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP6_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP6_A", Entity.SitePOS_EXP6_A));
                if (!string.IsNullOrEmpty(Entity.SitePOS_EXP7_A))
                    sqlParamList.Add(new SqlParameter("@SITE_POS_EXP7_A", Entity.SitePOS_EXP7_A));
                if (!string.IsNullOrEmpty(Entity.SiteOUTSIDE_PLAY))
                    sqlParamList.Add(new SqlParameter("@SITE_OUTSIDE_PLAY", Entity.SiteOUTSIDE_PLAY));

                if (!string.IsNullOrEmpty(Entity.SiteOUTSIDE_PLAY_WAIVED))
                    sqlParamList.Add(new SqlParameter("@SITE_OUTSIDE_PLAY_WAIVED", Entity.SiteOUTSIDE_PLAY_WAIVED));
                if (!string.IsNullOrEmpty(Entity.SiteOPEN_DATE))
                    sqlParamList.Add(new SqlParameter("@SITE_OPEN_DATE", Entity.SiteOPEN_DATE));
                if (!string.IsNullOrEmpty(Entity.SiteCLOSE_DATE))
                    sqlParamList.Add(new SqlParameter("@SITE_CLOSE_DATE", Entity.SiteCLOSE_DATE));
                if (!string.IsNullOrEmpty(Entity.SiteOPEN_DAYS))
                    sqlParamList.Add(new SqlParameter("@SITE_OPEN_DAYS", Entity.SiteOPEN_DAYS));
                if (!string.IsNullOrEmpty(Entity.SiteSTART_TIME))
                    sqlParamList.Add(new SqlParameter("@SITE_START_TIME", Entity.SiteSTART_TIME));
                if (!string.IsNullOrEmpty(Entity.SiteEND_TIME))
                    sqlParamList.Add(new SqlParameter("@SITE_END_TIME", Entity.SiteEND_TIME));

                if (!string.IsNullOrEmpty(Entity.SiteCLASS_START))
                    sqlParamList.Add(new SqlParameter("@SITE_CLASS_START", Entity.SiteCLASS_START));
                if (!string.IsNullOrEmpty(Entity.SiteCLASS_END))
                    sqlParamList.Add(new SqlParameter("@SITE_CLASS_END", Entity.SiteCLASS_END));
                if (!string.IsNullOrEmpty(Entity.SiteCLASS_DAYS))
                    sqlParamList.Add(new SqlParameter("@SITE_CLASS_DAYS", Entity.SiteCLASS_DAYS));
                if (!string.IsNullOrEmpty(Entity.SiteTRAN_AREA))
                    sqlParamList.Add(new SqlParameter("@SITE_TRAN_AREA", Entity.SiteTRAN_AREA));
                if (!string.IsNullOrEmpty(Entity.SiteRENTAL_FEE))
                    sqlParamList.Add(new SqlParameter("@SITE_RENTAL_FEE", decimal.Parse(Entity.SiteRENTAL_FEE)));
                if (!string.IsNullOrEmpty(Entity.SiteUTILITY_FEE))
                    sqlParamList.Add(new SqlParameter("@SITE_UTILITY_FEE", decimal.Parse(Entity.SiteUTILITY_FEE)));

                if (!string.IsNullOrEmpty(Entity.SiteL_FSPR_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSPR_DT", Entity.SiteL_FSPR_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FSPR_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSPR_NT", Entity.SiteL_FSPR_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FSSV_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSSV_DT", Entity.SiteL_FSSV_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FSSV_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSSV_NT", Entity.SiteL_FSSV_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FSST_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSST_DT", Entity.SiteL_FSST_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FSST_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FSST_NT", Entity.SiteL_FSST_NT));

                if (!string.IsNullOrEmpty(Entity.SiteL_HLDP_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_HLDP_DT", Entity.SiteL_HLDP_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_HLDP_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_HLDP_NT", Entity.SiteL_HLDP_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FIDP_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FIDP_DT", Entity.SiteL_FIDP_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FIDP_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FIDP_NT", Entity.SiteL_FIDP_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FIEX_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FIEX_DT", Entity.SiteL_FIEX_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_FIEX_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_FIEX_NT", Entity.SiteL_FIEX_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_SMAL_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_SMAL_DT", Entity.SiteL_SMAL_DT));

                if (!string.IsNullOrEmpty(Entity.SiteL_SMAL_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_SMAL_NT", Entity.SiteL_SMAL_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_HESY_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_HESY_DT", Entity.SiteL_HESY_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_HESY_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_HESY_NT", Entity.SiteL_HESY_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_WASY_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_WASY_DT", Entity.SiteL_WASY_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_WASY_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_WASY_NT", Entity.SiteL_WASY_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_DLED_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_DLED_DT", Entity.SiteL_DLED_DT));

                if (!string.IsNullOrEmpty(Entity.SiteL_DLED_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_DLED_NT", Entity.SiteL_DLED_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_BUIL_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_BUIL_DT", Entity.SiteL_BUIL_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_BUIL_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_BUIL_NT", Entity.SiteL_BUIL_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_EXP1_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP1_DT", Entity.SiteL_EXP1_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_EXP1_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP1_NT", Entity.SiteL_EXP1_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_EXP2_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP2_DT", Entity.SiteL_EXP2_DT));

                if (!string.IsNullOrEmpty(Entity.SiteL_EXP2_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP2_NT", Entity.SiteL_EXP2_NT));
                if (!string.IsNullOrEmpty(Entity.SiteL_EXP3_DT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP3_DT", Entity.SiteL_EXP3_DT));
                if (!string.IsNullOrEmpty(Entity.SiteL_EXP3_NT))
                    sqlParamList.Add(new SqlParameter("@SITE_L_EXP3_NT", Entity.SiteL_EXP3_NT));
                if (!string.IsNullOrEmpty(Entity.SiteLANGUAGE))
                    sqlParamList.Add(new SqlParameter("@SITE_LANGUAGE", Entity.SiteLANGUAGE));
                if (!string.IsNullOrEmpty(Entity.SiteLANGUAGE_OTHER))
                    sqlParamList.Add(new SqlParameter("@SITE_LANGUAGE_OTHER", Entity.SiteLANGUAGE_OTHER));
                if (!string.IsNullOrEmpty(Entity.SiteREP_SEQ))
                    sqlParamList.Add(new SqlParameter("@SITE_REP_SEQ", Entity.SiteREP_SEQ));

                if (!string.IsNullOrEmpty(Entity.SiteREP_OPT1))
                    sqlParamList.Add(new SqlParameter("@SITE_REP_OPT1", Entity.SiteREP_OPT1));
                if (!string.IsNullOrEmpty(Entity.SiteREP_OPT2))
                    sqlParamList.Add(new SqlParameter("@SITE_REP_OPT2", Entity.SiteREP_OPT2));
                if (!string.IsNullOrEmpty(Entity.SiteMEAL_AREA))
                    sqlParamList.Add(new SqlParameter("@SITE_MEAL_AREA", Entity.SiteMEAL_AREA));
                if (!string.IsNullOrEmpty(Entity.SiteCOMMENT))
                    sqlParamList.Add(new SqlParameter("@SITE_COMMENT", Entity.SiteCOMMENT));
                //if (!string.IsNullOrEmpty(Entity.SiteRT1))
                //    sqlParamList.Add(new SqlParameter("@SITE_RT1", Entity.SiteRT1));
                //if (!string.IsNullOrEmpty(Entity.SiteRT2))
                //    sqlParamList.Add(new SqlParameter("@SITE_RT2", Entity.SiteRT2));

                if (!string.IsNullOrEmpty(Entity.SiteFUND1))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND1", Entity.SiteFUND1));
                if (!string.IsNullOrEmpty(Entity.SiteFUND2))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND2", Entity.SiteFUND2));
                if (!string.IsNullOrEmpty(Entity.SiteFUND3))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND3", Entity.SiteFUND3));
                if (!string.IsNullOrEmpty(Entity.SiteFUND4))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND4", Entity.SiteFUND4));
                if (!string.IsNullOrEmpty(Entity.SiteFUND5))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND5", Entity.SiteFUND5));
                if (!string.IsNullOrEmpty(Entity.SiteHEDTEACHER))
                    sqlParamList.Add(new SqlParameter("@SITE_HEDTEACHER", Entity.SiteHEDTEACHER));

                if (!string.IsNullOrEmpty(Entity.SiteASSISTANT1))
                    sqlParamList.Add(new SqlParameter("@SITE_ASSISTANT1", Entity.SiteASSISTANT1));
                if (!string.IsNullOrEmpty(Entity.SiteASSISTANT2))
                    sqlParamList.Add(new SqlParameter("@SITE_ASSISTANT2", Entity.SiteASSISTANT2));
                if (!string.IsNullOrEmpty(Entity.SiteC_DIRECTOR))
                    sqlParamList.Add(new SqlParameter("@SITE_C_DIRECTOR", Entity.SiteC_DIRECTOR));
                if (!string.IsNullOrEmpty(Entity.SiteACTIVE))
                    sqlParamList.Add(new SqlParameter("@SITE_ACTIVE", Entity.SiteACTIVE));
                if (!string.IsNullOrEmpty(Entity.SitePROJECT))
                    sqlParamList.Add(new SqlParameter("@SITE_PROJECT", Entity.SitePROJECT));
                if (!string.IsNullOrEmpty(Entity.SiteLSTC_OPERATOR))
                    sqlParamList.Add(new SqlParameter("@SITE_LSTC_OPERATOR", Entity.SiteLSTC_OPERATOR));
                if (!string.IsNullOrEmpty(Entity.SITE_FUND_SLOT1))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND_SLOT1", Entity.SITE_FUND_SLOT1));
                if (!string.IsNullOrEmpty(Entity.SITE_FUND_SLOT2))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND_SLOT2", Entity.SITE_FUND_SLOT2));
                if (!string.IsNullOrEmpty(Entity.SITE_FUND_SLOT3))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND_SLOT3", Entity.SITE_FUND_SLOT3));
                if (!string.IsNullOrEmpty(Entity.SITE_FUND_SLOT4))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND_SLOT4", Entity.SITE_FUND_SLOT4));
                if (!string.IsNullOrEmpty(Entity.SITE_FUND_SLOT5))
                    sqlParamList.Add(new SqlParameter("@SITE_FUND_SLOT5", Entity.SITE_FUND_SLOT5));

                if (!string.IsNullOrEmpty(Entity.SITE_HOMEBASE))
                    sqlParamList.Add(new SqlParameter("@SITE_HOMEBASE", Entity.SITE_HOMEBASE));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@SITE_DATE_ADD", Entity.SiteDATE_ADD));
                    sqlParamList.Add(new SqlParameter("@SITE_ADD_OPERATOR", Entity.SiteADD_OPERATOR));
                    sqlParamList.Add(new SqlParameter("@SITE_DATE_LSTC", Entity.SiteDATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CaseSiteEntity> Browse_CASESITE(CaseSiteEntity Entity, string Opretaion_Mode)
        {
            List<CaseSiteEntity> CASESiteProfile = new List<CaseSiteEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESite_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESITE]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESiteProfile.Add(new CaseSiteEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESiteProfile; }

            return CASESiteProfile;
        }

        public List<CaseSiteEntity> Browse_CASESITE_PIR(CaseSiteEntity Entity, string Opretaion_Mode, string StrTable)
        {
            List<CaseSiteEntity> CASESiteProfile = new List<CaseSiteEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESite_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_CASESITE]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESiteProfile.Add(new CaseSiteEntity(row, StrTable));
                }
            }
            catch (Exception ex)
            { return CASESiteProfile; }

            return CASESiteProfile;
        }


        public List<ReportSiteEntity> HSSB2110_Report(string Agency, string Dept, string Program, string Year)
        {
            List<ReportSiteEntity> CaseSiteProfile = new List<ReportSiteEntity>();
            try
            {
                DataSet CaseSiteData = SPAdminDB.HSSB2110_SiteReprt(Agency, Dept, Program, Year);
                if (CaseSiteData != null && CaseSiteData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSiteData.Tables[0].Rows)
                    {
                        CaseSiteProfile.Add(new ReportSiteEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSiteProfile;
            }

            return CaseSiteProfile;
        }

        public List<SqlParameter> Prepare_CASEMSTSER_SqlParameters_List(CaseMSTSER Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (!string.IsNullOrEmpty(Entity.Agency))
                    sqlParamList.Add(new SqlParameter("@MSTSER_AGENCY", Entity.Agency));
                if (!string.IsNullOrEmpty(Entity.Dept))
                    sqlParamList.Add(new SqlParameter("@MSTSER_DEPT", Entity.Dept));
                if (!string.IsNullOrEmpty(Entity.Program))
                    sqlParamList.Add(new SqlParameter("@MSTSER_PROGRAM", Entity.Program));
                if (!string.IsNullOrEmpty(Entity.Year))
                    sqlParamList.Add(new SqlParameter("@MSTSER_YEAR", Entity.Year));
                if (!string.IsNullOrEmpty(Entity.AppNo))
                    sqlParamList.Add(new SqlParameter("@MSTSER_APP_NO", Entity.AppNo));
                if (!string.IsNullOrEmpty(Entity.Service))
                    sqlParamList.Add(new SqlParameter("@MSTSER_SERVICE", Entity.Service));


                if (!string.IsNullOrEmpty(Entity.LstcOperator))
                    sqlParamList.Add(new SqlParameter("@MSTSER_LSTC_OPERATOR", Entity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@MSTSER_DATE_ADD", Entity.DateAdd));
                sqlParamList.Add(new SqlParameter("@MSTSER_ADD_OPERATOR", Entity.AddOperator));
                sqlParamList.Add(new SqlParameter("@MSTSER_DATE_LSTC", Entity.DateLstc));


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CaseMSTSER> Browse_MSTSER(CaseMSTSER Entity, string Opretaion_Mode)
        {
            List<CaseMSTSER> MSTSERProfile = new List<CaseMSTSER>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASEMSTSER_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_MSTSER]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        MSTSERProfile.Add(new CaseMSTSER(row));
                }
            }
            catch (Exception ex)
            { return MSTSERProfile; }

            return MSTSERProfile;
        }


        public List<SqlParameter> Prepare_ADDCUST_SqlParameters_List(AddCustEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.ACTAGENCY))
                    sqlParamList.Add(new SqlParameter("@ACT_AGENCY", Entity.ACTAGENCY));
                if (!string.IsNullOrEmpty(Entity.ACTDEPT))
                    sqlParamList.Add(new SqlParameter("@ACT_DEPT", Entity.ACTDEPT));
                if (!string.IsNullOrEmpty(Entity.ACTPROGRAM))
                    sqlParamList.Add(new SqlParameter("@ACT_PROGRAM", Entity.ACTPROGRAM));
                if (!string.IsNullOrEmpty(Entity.ACTYEAR))
                    sqlParamList.Add(new SqlParameter("@ACT_YEAR", Entity.ACTYEAR));
                if (!string.IsNullOrEmpty(Entity.ACTAPPNO))
                    sqlParamList.Add(new SqlParameter("@ACT_APP_NO", Entity.ACTAPPNO));
                if (!string.IsNullOrEmpty(Entity.ACTSNPFAMILYSEQ))
                    sqlParamList.Add(new SqlParameter("@ACT_SNP_FAMILY_SEQ", Entity.ACTSNPFAMILYSEQ));
                if (!string.IsNullOrEmpty(Entity.ACTCODE))
                    sqlParamList.Add(new SqlParameter("@ACT_CODE", Entity.ACTCODE));
                if (!string.IsNullOrEmpty(Entity.ACTRESPSEQ))
                    sqlParamList.Add(new SqlParameter("@ACT_RESP_SEQ", Entity.ACTRESPSEQ));
                if (!string.IsNullOrEmpty(Entity.ACTNUMRESP))
                    sqlParamList.Add(new SqlParameter("@ACT_NUM_RESP", Entity.ACTNUMRESP));
                if (!string.IsNullOrEmpty(Entity.ACTALPHARESP))
                    sqlParamList.Add(new SqlParameter("@ACT_ALPHA_RESP", Entity.ACTALPHARESP));
                if (!string.IsNullOrEmpty(Entity.ACTDATERESP))
                    sqlParamList.Add(new SqlParameter("@ACT_DATE_RESP", Entity.ACTDATERESP));
                if (!string.IsNullOrEmpty(Entity.ACTMULTRESP))
                    sqlParamList.Add(new SqlParameter("@ACT_MULT_RESP", Entity.ACTMULTRESP));

                if (!string.IsNullOrEmpty(Entity.lstcoperator))
                    sqlParamList.Add(new SqlParameter("@ACT_LSTC_OPERATOR", Entity.lstcoperator));
                sqlParamList.Add(new SqlParameter("@ACT_DATE_ADD", Entity.adddate));
                sqlParamList.Add(new SqlParameter("@ACT_ADD_OPERATOR", Entity.addoperator));
                sqlParamList.Add(new SqlParameter("@ACT_DATE_LSTC", Entity.lstcdate));


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<AddCustEntity> Browse_ADDCUST(AddCustEntity Entity, string Opretaion_Mode)
        {
            List<AddCustEntity> MSTSERProfile = new List<AddCustEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ADDCUST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ADDCUST]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        MSTSERProfile.Add(new AddCustEntity(row));
                }
            }
            catch (Exception ex)
            { return MSTSERProfile; }

            return MSTSERProfile;
        }

        public bool UpdateCASESITE(CaseSiteEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CASESite_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_CASESITE", out Sql_Reslut_Msg);  //
                Msg = DeleteMsg.Value.ToString();
                //Sql_Reslut_Msg = SP_Result_Msg.Value.ToString();
            }
            catch (Exception ex)
            { boolsuccess = false; }

            return boolsuccess;
        }

        public long GetMaxApplicantNo(string Agency, string Dept, string Program, string Year)
        {
            return CaseMst.GetCASEMSTMaxApplNo(Agency, Dept, Program, Year);
        }

        public List<CaseSnpEntity> GetCaseSnpImageUploadDetails(string strssnNO, string strHierachy, string strUserName, string strInccurHie, string strMember, string strDuplicate, string strApp, string strFamilySeq, string strBypassSecrect)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();

            try
            {
                DataSet CaseSnpDataDetails = CaseSnpData.GetCaseSnpImageUploadDetails(strssnNO, strHierachy, strUserName, strInccurHie, strMember, strDuplicate, strApp, strFamilySeq, strBypassSecrect);
                if (CaseSnpDataDetails != null && CaseSnpDataDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpDataDetails.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row, "ImageUpload"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }


        public List<CaseHistEntity> GetCaseHistDetails(string tableName, string Key, string Screen)
        {
            List<CaseHistEntity> CaseHistDetails = new List<CaseHistEntity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.CaseMst.GetCaseHistDetails(tableName, Key, Screen);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        CaseHistDetails.Add(new CaseHistEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseHistDetails;
            }

            return CaseHistDetails;
        }

        public bool InsertCaseHist(CaseHistEntity HistEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@HIST_TBLNAME", HistEntity.HistTblName));
                sqlParamList.Add(new SqlParameter("@HIST_TBLKEY", HistEntity.HistTblKey));
                sqlParamList.Add(new SqlParameter("@HIST_SCREEN", HistEntity.HistScreen));
                if (HistEntity.HistSubScr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@HIST_SUBSCR", HistEntity.HistSubScr));
                sqlParamList.Add(new SqlParameter("@HIST_CHANGES", HistEntity.HistChanges));
                sqlParamList.Add(new SqlParameter("@HIST_LSTC_OPERATOR", HistEntity.LstcOperator));
                boolSuccess = CaseMst.InsertCaseHist(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public List<Headstart_Template> GetHeadstartTemplate(string AgyType, string AgyCode)
        {
            List<Headstart_Template> TemplateDetails = new List<Headstart_Template>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.CaseMst.GetHeadTemplate(AgyType, AgyCode);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new Headstart_Template(row));
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


        public List<MSTSNP_Entity> GetChildList(string Agency, string Dept, string Prog, string Year, string SortCol, string Sites, string Repeaters, string BaseAge)
        {
            List<MSTSNP_Entity> TemplateDetails = new List<MSTSNP_Entity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.CaseMst.GetChild_List(Agency, Dept, Prog, Year, SortCol, Sites, Repeaters, BaseAge);
                DataTable dt = CaseHist.Tables[0];
                if (dt != null)
                {
                    DataView dv = new DataView(dt);
                    switch (SortCol)
                    {
                        case "1": dv.Sort = "Lname,Fname"; break;
                        case "2": dv.Sort = "AppNo"; break;
                        case "3": dv.Sort = "MST_SITE,State1,City,Street,Hno"; break;
                        case "4": dv.Sort = "State1,City,Street,Hno,Lname,Fname"; break;
                        case "5": dv.Sort = "ESITE,EROOM,EAMPM,Lname,Fname"; break;
                        case "6": dv.Sort = "MST_SITE,Lname,Fname"; break;
                        case "7": dv.Sort = "MST_SITE,Lname,Fname"; break;
                        case "8": dv.Sort = "MST_SITE,AppNo"; break;
                        case "9": dv.Sort = "G1_LName,G1_FName"; break;
                    }
                    dt = dv.ToTable();
                }
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TemplateDetails.Add(new MSTSNP_Entity(row));
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

        public bool UpdateCaseMstRanks(string strDetailsXMl, string strType)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@ApplicantDetails", strDetailsXMl));
                sqlParamList.Add(new SqlParameter("@Type", strType));
                CaseMst.UpdateCaseMstRanks(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public List<SqlParameter> Prepare_CASESER_SqlParameters_List(CaseServicesEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                sqlParamList.Add(new SqlParameter("@CAC_SERVICE", Entity.Service));
                if (!string.IsNullOrEmpty(Entity.Fund))
                    sqlParamList.Add(new SqlParameter("@CAC_FUND", Entity.Fund));
                if (!string.IsNullOrEmpty(Entity.Seq))
                    sqlParamList.Add(new SqlParameter("@CAC_SEQ", Entity.Seq));
                if (!string.IsNullOrEmpty(Entity.Agency))
                    sqlParamList.Add(new SqlParameter("@CAC_AGENCY", Entity.Agency));
                if (!string.IsNullOrEmpty(Entity.Dept))
                    sqlParamList.Add(new SqlParameter("@CAC_DEPT", Entity.Dept));
                if (!string.IsNullOrEmpty(Entity.Program))
                    sqlParamList.Add(new SqlParameter("@CAC_PROGRAM", Entity.Program));
                if (!string.IsNullOrEmpty(Entity.Desc))
                    sqlParamList.Add(new SqlParameter("@CAC_DESC", Entity.Desc));
                if (!string.IsNullOrEmpty(Entity.Application))
                    sqlParamList.Add(new SqlParameter("@CAC_APPLICATION", Entity.Application));
                if (!string.IsNullOrEmpty(Entity.Zero))
                    sqlParamList.Add(new SqlParameter("@CAC_ZERO", Entity.Zero));
                if (!string.IsNullOrEmpty(Entity.Vendor))
                    sqlParamList.Add(new SqlParameter("@CAC_VENDOR", Entity.Vendor));
                if (!string.IsNullOrEmpty(Entity.Authorizations))
                    sqlParamList.Add(new SqlParameter("@CAC_AUTHORIZATIONS", Entity.Authorizations));
                if (!string.IsNullOrEmpty(Entity.Mi))
                    sqlParamList.Add(new SqlParameter("@CAC_MI", Entity.Mi));

                if (!string.IsNullOrEmpty(Entity.Outofpoverty))
                    sqlParamList.Add(new SqlParameter("@CAC_OUTOFPOVERTY", Entity.Outofpoverty));
                if (!string.IsNullOrEmpty(Entity.Active))
                    sqlParamList.Add(new SqlParameter("@CAC_ACTIVE", Entity.Active));
                if (!string.IsNullOrEmpty(Entity.DateLstc))
                    sqlParamList.Add(new SqlParameter("@CAC_DATE_LSTC", Entity.DateLstc));
                if (!string.IsNullOrEmpty(Entity.LstcOperator))
                    sqlParamList.Add(new SqlParameter("@CAC_LSTC_OPERATOR", Entity.LstcOperator));
                if (!string.IsNullOrEmpty(Entity.DataAdd))
                    sqlParamList.Add(new SqlParameter("@CAC_DATE_ADD", Entity.DataAdd));
                if (!string.IsNullOrEmpty(Entity.AddOperator))
                    sqlParamList.Add(new SqlParameter("@CAC_ADD_OPERATOR", Entity.AddOperator));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CaseServicesEntity> Browse_CASESER(CaseServicesEntity Entity, string Opretaion_Mode)
        {
            List<CaseServicesEntity> CASESiteProfile = new List<CaseServicesEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CASESER_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_Caseser]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASESPM(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESiteProfile.Add(new CaseServicesEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESiteProfile; }

            return CASESiteProfile;
        }

        //public List<MSTSNP_Entity> GetChildList(string Agency, string Dept, string Prog, string Year, string SortCol)
        //{
        //    List<MSTSNP_Entity> TemplateDetails = new List<MSTSNP_Entity>();
        //    try
        //    {
        //        DataSet CaseHist = Captain.DatabaseLayer.CaseMst.GetChild_List(Agency, Dept, Prog, Year, SortCol);
        //        if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in CaseHist.Tables[0].Rows)
        //            {
        //                TemplateDetails.Add(new MSTSNP_Entity(row));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return TemplateDetails;
        //    }


        //    return TemplateDetails;
        //}


        public List<CaseIncomeEntity> Browse_Caseincome(string Agency, string Dept, string Program, string Year, string AppNo)
        {
            List<CaseIncomeEntity> TemplateDetails = new List<CaseIncomeEntity>();
            try
            {
                DataSet CaseHist = Captain.DatabaseLayer.CaseMst.GetCASEINCOME(Agency, Dept, Program, Year, AppNo);
                if (CaseHist != null && CaseHist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHist.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new CaseIncomeEntity(row, null));
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

        public List<MembersEntity> GetMembers(string Agency, string Dept, string Prog, string Year)
        {
            List<MembersEntity> MainmenuDetails = new List<MembersEntity>();
            try
            {
                DataSet MainMenu_Table = Captain.DatabaseLayer.CaseMst.GetMembers(Agency, Dept, Prog, Year);
                if (MainMenu_Table != null && MainMenu_Table.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MainMenu_Table.Tables[0].Rows)
                        MainmenuDetails.Add(new MembersEntity(row));
                }
            }
            catch (Exception ex)
            {
                return MainmenuDetails;
            }
            return MainmenuDetails;
        }


        public bool UpdateMstPositions(CaseMstEntity MstEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MST_AGENCY", MstEntity.ApplAgency));
                sqlParamList.Add(new SqlParameter("@MST_DEPT", MstEntity.ApplDept));
                sqlParamList.Add(new SqlParameter("@MST_PROGRAM", MstEntity.ApplProgram));
                if (MstEntity.ApplYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_YEAR", MstEntity.ApplYr));
                if (MstEntity.ApplNo != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_APP_NO", MstEntity.ApplNo));

                if (MstEntity.Appxml != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MST_AppXML", MstEntity.Appxml));

                if (MstEntity.StaffMember != string.Empty)
                    sqlParamList.Add(new SqlParameter("@StaffMember", MstEntity.StaffMember));

                if (MstEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", MstEntity.Type));

                if (MstEntity.Position != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Postions", MstEntity.Position));

                boolSuccess = CaseMst.UpdateMstPositions(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }



            return boolSuccess;
        }


        public List<CaseSnpEntity> GETXMLDATA0023Snp(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strType, string App)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.AgyTab.GETXMLDATA0023(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, strType, string.Empty, string.Empty, App);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row, "TMSB0023"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public List<CaseSnpEntity> GetSnpFixclinetId(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string dob, string strkey, string strType,string strFromdate,string strTodate,string strAgency)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseMst.GetSnpFixclinetId(Year, Count, SSno, ClientId, FirstName, LastName, dob, strkey, strType,strFromdate,strTodate, strAgency);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row, "FIXCLIENT"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public List<CaseSnpEntity> GetSnpFixFamilyId(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string dob, string strkey, string strType, string strTable,string strFromdate, string strTodate,string strAgency)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseMst.GetSnpFixclinetId(Year, Count, SSno, ClientId, FirstName, LastName, dob, strkey, strType,strFromdate,strTodate, strAgency);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public List<CaseSnpEntity> GetSnpFixclinetIdHie(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string dob, string strkey, string strType, string strTable,string strAgency,string strDept,string strProgram, string strPrYear, string strFromdate, string strTodate)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseMst.GetSnpFixclinetIdHie(Year, Count, SSno, ClientId, FirstName, LastName, dob, strkey, strType,strAgency,strDept,strProgram,strPrYear, strFromdate,strTodate);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }


        public List<CaseSnpEntity> GetSnpFixclinetIdAddDate(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string dob, string strkey, string strType,string strFromdate, string strTodate,string strAgency)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseMst.GetSnpFixclinetId(Year, Count, SSno, ClientId, FirstName, LastName, dob, strkey, strType,strFromdate,strTodate,strAgency);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new CaseSnpEntity(row, "FIXCLIENTADDDATE"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }



        public List<CaseSnpEntity> GetSnpFixSSN(string Year, string Count, string SSno, string ClientId, string FirstName, string LastName, string dob, string strkey, string strType,string strFromDate,string strTodate,string strAgency)
        {
            List<CaseSnpEntity> CaseSnpDetails = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseMst.GetSnpFixclinetId(Year, Count, SSno, ClientId, FirstName, LastName, dob, strkey, strType,strFromDate,strTodate, strAgency);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    if (strType == "SSNFName")
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "FIXSSNFName"));
                        }
                    }
                    else if (strType == "OnlySSN")
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "OnlySSN"));
                        }
                    }
                    else if(strType == "CLIENTFNameDOB" || strType == "ERAP")
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "CLIENTFNameDOB"));
                        }
                    }
                    else if (strType == "CLIENTFNameDOBALL")
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "CLIENTFNameDOB"));
                        }
                    }
                    else if(strType == "OnlyClientId")
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "OnlyClientId"));
                        }
                    }
                    else
                    {
                        foreach (DataRow row in CaseSnp.Tables[0].Rows)
                        {
                            CaseSnpDetails.Add(new CaseSnpEntity(row, "FIXSSN#"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }


        public bool UpdateSNPClientId(string SSno, string ClientId, string FirstName, string LastName, string dob, string strType, string strMode,string strLstcoperator)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (SSno != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SSNO", SSno));

                if (ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLIENT_ID", ClientId));
                if (FirstName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_FI", FirstName));
                if (LastName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_LAST", LastName));
                if (dob != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_BDATE", dob));

                if (strType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", strType));

                if (strMode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", strMode));

                if (strLstcoperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LstcOperator", strLstcoperator));

                boolSuccess = CaseMst.UpdateSNPClientId(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }



            return boolSuccess;
        }

        public bool UpdateSNPClientId(string SSno, string ClientId, string FirstName, string LastName, string dob, string strType, string strMode,string strAgency,string strDept,string strProgram,string strYear,string strApp,string strSeq,string strLstcoperator,string strOldFamilyId)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (SSno != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_SSNO", SSno));

                if (ClientId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_CLIENT_ID", ClientId));
                if (FirstName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_FI", FirstName));
                if (LastName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_NAME_IX_LAST", LastName));
                if (dob != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_ALT_BDATE", dob));

                if (strType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", strType));

                if (strMode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", strMode));

                if (strAgency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Agency", strAgency));

                if (strDept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Dept", strDept));

                if (strProgram  != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Program", strProgram));

                if (strYear != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Year", strYear));

                if (strApp != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ApplNo", strApp));

                if (strSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Familyseq", strSeq));

                if (strLstcoperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LstcOperator", strLstcoperator));

                if (strOldFamilyId  != string.Empty)
                    sqlParamList.Add(new SqlParameter("@OldFamilyid", strOldFamilyId));


                boolSuccess = CaseMst.UpdateSNPClientId(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }



            return boolSuccess;
        }

        public bool UpdateClientIds(string Year,string Count, string SSno, string ClientId, string FirstName, string LastName, string strdob, string strkey, string Type, string strFromdate, string strTodate, string strLstcoperator)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Count != string.Empty)
                {
                    SqlParameter agencyParm = new SqlParameter("@Count", Count);
                    sqlParamList.Add(agencyParm);
                }
                if (SSno != string.Empty)
                {
                    SqlParameter deptParm = new SqlParameter("@ssno", SSno);
                    sqlParamList.Add(deptParm);
                }
                if (ClientId != string.Empty)
                {
                    SqlParameter programParm = new SqlParameter("@ClientId", ClientId);
                    sqlParamList.Add(programParm);
                }
                if (Year != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@YEAR", Year);
                    sqlParamList.Add(typeParm);
                }
                if (FirstName != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@FirstName", FirstName);
                    sqlParamList.Add(typeParm);
                }
                if (LastName != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@LastName", LastName);
                    sqlParamList.Add(typeParm);
                }
                if (strdob != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@Dob", strdob);
                    sqlParamList.Add(typeParm);
                }

                if (strkey != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@Key", strkey);
                    sqlParamList.Add(typeParm);
                }

                if (Type != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@Type", Type);
                    sqlParamList.Add(typeParm);
                }
                if (strFromdate != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@FromDate", strFromdate);
                    sqlParamList.Add(typeParm);
                }
                if (strTodate != string.Empty)
                {
                    SqlParameter typeParm = new SqlParameter("@ToDate", strTodate);
                    sqlParamList.Add(typeParm);
                }

                if (strLstcoperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@LstcOperator", strLstcoperator));

                boolSuccess = CaseMst.UpdateClientIdS(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }



            return boolSuccess;
        }


        public bool UpdateCaseMstSnpIncome(CaseSnpEntity snpEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MstAgency", snpEntity.Agency));
                sqlParamList.Add(new SqlParameter("@MstDept", snpEntity.Dept));
                sqlParamList.Add(new SqlParameter("@MstProgram", snpEntity.Program));
                if (snpEntity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MstYear", snpEntity.Year));
                sqlParamList.Add(new SqlParameter("@MstApplication", snpEntity.App));


                if (snpEntity.FamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MstFamilySeq", snpEntity.FamilySeq));

                if (snpEntity.Exclude != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SNP_EXCLUDE", snpEntity.Exclude));


                boolSuccess = CaseMst.UpdateCASEMSTSNPINCOME(sqlParamList);

            }
            catch (Exception ex)
            {


                boolSuccess = false;



            }



            return boolSuccess;
        }


        public List<CustomQuestionsEntity> GETDIMSCORE(string agency, string dep, string program, string year, string app, string strMode, string strFilterApp)
        {
            List<CustomQuestionsEntity> customdata = new List<CustomQuestionsEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSnpData.GETDIMSCORE(agency, dep, program, year, app, strMode, strFilterApp);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        customdata.Add(new CustomQuestionsEntity(row, "DIMENSION"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customdata;
            }

            return customdata;
        }


        public List<CustomQuestionsEntity> GETDIMSCORE(string agency, string dep, string program, string year, string app, string strMode, string strFilterApp, string strFrom, string strTo, string strDimensioncode, string strGroupcode, string strFromdate, string strTodate)
        {
            List<CustomQuestionsEntity> customdata = new List<CustomQuestionsEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSnpData.GETDIMSCORE(agency, dep, program, year, app, strMode, strFilterApp, strFrom, strTo, strDimensioncode, strGroupcode, strFromdate, strTodate, string.Empty);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        customdata.Add(new CustomQuestionsEntity(row, "DIMSCOREREPORT"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customdata;
            }

            return customdata;
        }


        public List<LPMQEntity> GetLPMQ0001(string agency, string dep, string program, string year, string Heatsource)
        {
            List<LPMQEntity> CaseSnpDetails = new List<LPMQEntity>();
            try
            {
                DataSet CaseSnp = Captain.DatabaseLayer.CaseSnpData.GetLPMQ0001(agency, dep, program, year, Heatsource);
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpDetails.Add(new LPMQEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpDetails;
            }

            return CaseSnpDetails;
        }

        public bool InsertUpdateDelRNGCounts(RNGCOUNTSEnitity RngCountEntity)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@RNGC_AGENCY", RngCountEntity.RNGC_AGENCY));
                sqlParamList.Add(new SqlParameter("@RNGC_DEPT", RngCountEntity.RNGC_DEPT));
                sqlParamList.Add(new SqlParameter("@RNGC_PROGRAM", RngCountEntity.RNGC_PROGRAM));
                if (RngCountEntity.RNGC_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_YEAR", RngCountEntity.RNGC_YEAR));
                sqlParamList.Add(new SqlParameter("@RNGC_APP", RngCountEntity.RNGC_APP));
                if (RngCountEntity.RNGC_ELIG_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ELIG_DATE", RngCountEntity.RNGC_ELIG_DATE));

                if (RngCountEntity.RNGC_MST_FAMILY_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_MST_FAMILY_SEQ", RngCountEntity.RNGC_MST_FAMILY_SEQ));

                if (RngCountEntity.RNGC_SNP_FAMILY_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_SNP_FAMILY_SEQ", RngCountEntity.RNGC_SNP_FAMILY_SEQ));


                if (RngCountEntity.RNGC_FAMILY_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_FAMILY_ID", RngCountEntity.RNGC_FAMILY_ID));

                if (RngCountEntity.RNGC_MEMBER_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_MEMBER_CODE", RngCountEntity.RNGC_MEMBER_CODE));

                sqlParamList.Add(new SqlParameter("@RNGC_NAME_IX_FI", RngCountEntity.RNGC_NAME_IX_FI));
                if (RngCountEntity.RNGC_NAME_IX_LAST != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_NAME_IX_LAST", RngCountEntity.RNGC_NAME_IX_LAST));
                if (RngCountEntity.@RNGC_NAME_IX_MI != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_NAME_IX_MI", RngCountEntity.@RNGC_NAME_IX_MI));

                if (RngCountEntity.RNGC_CLIENT_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_CLIENT_ID", RngCountEntity.RNGC_CLIENT_ID));
                if (RngCountEntity.RNGC_FAMILY_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_FAMILY_TYPE", RngCountEntity.RNGC_FAMILY_TYPE));
                if (RngCountEntity.RNGC_NO_INPROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_NO_INPROG", RngCountEntity.RNGC_NO_INPROG));

                if (RngCountEntity.RNGC_HOUSING != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_HOUSING", RngCountEntity.RNGC_HOUSING));

                if (!RngCountEntity.RNGC_POVERTY.Equals(string.Empty))
                    sqlParamList.Add(new SqlParameter("@RNGC_POVERTY", RngCountEntity.RNGC_POVERTY));

                if (RngCountEntity.RNCG_COUNTY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNCG_COUNTY", RngCountEntity.RNCG_COUNTY));

                if (RngCountEntity.RNGC_CASE_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_CASE_TYPE", RngCountEntity.RNGC_CASE_TYPE));

                if (RngCountEntity.RNGC_ACTIVE_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ACTIVE_STATUS", RngCountEntity.RNGC_ACTIVE_STATUS));
                if (RngCountEntity.RNGC_SITE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_SITE", RngCountEntity.RNGC_SITE));


                if (RngCountEntity.RNGC_ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ZIP", RngCountEntity.RNGC_ZIP));


                if (RngCountEntity.RNGC_ZIPPLUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ZIPPLUS", RngCountEntity.RNGC_ZIPPLUS));


                if (RngCountEntity.RNGC_INTAKE_WORKER != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_INTAKE_WORKER", RngCountEntity.RNGC_INTAKE_WORKER));


                if (RngCountEntity.RNGC_SECRET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_SECRET", RngCountEntity.RNGC_SECRET));


                if (RngCountEntity.RNGC_INCOME_TYPES != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_INCOME_TYPES", RngCountEntity.RNGC_INCOME_TYPES));


                if (RngCountEntity.RNGC_NCASHBEN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_NCASHBEN", RngCountEntity.RNGC_NCASHBEN));

                if (RngCountEntity.RNGC_ALT_BDATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ALT_BDATE", RngCountEntity.RNGC_ALT_BDATE));

                if (RngCountEntity.RNGC_SEX != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_SEX", RngCountEntity.RNGC_SEX));

                if (RngCountEntity.RNGC_AGE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_AGE", RngCountEntity.RNGC_AGE));

                if (RngCountEntity.RNGC_ETHNIC != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_ETHNIC", RngCountEntity.RNGC_ETHNIC));

                if (RngCountEntity.RNGC_EDUCATION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_EDUCATION", RngCountEntity.RNGC_EDUCATION));

                if (RngCountEntity.RNGC_HEALTH_INS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_HEALTH_INS", RngCountEntity.RNGC_HEALTH_INS));

                if (RngCountEntity.RNGC_HEALTH_CODES != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_HEALTH_CODES", RngCountEntity.RNGC_HEALTH_CODES));

                if (RngCountEntity.RNGC_VET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_VET", RngCountEntity.RNGC_VET));
                if (RngCountEntity.RNGC_MILITARY_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_MILITARY_STATUS", RngCountEntity.RNGC_MILITARY_STATUS));

                if (RngCountEntity.RNGC_DISABLE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_DISABLE", RngCountEntity.RNGC_DISABLE));

                if (RngCountEntity.RNGC_FOOD_STAMPS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_FOOD_STAMPS", RngCountEntity.RNGC_FOOD_STAMPS));


                if (RngCountEntity.RNGC_FARMER != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_FARMER", RngCountEntity.RNGC_FARMER));


                if (RngCountEntity.RNGC_WORK_STAT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_WORK_STAT", RngCountEntity.RNGC_WORK_STAT));


                if (RngCountEntity.RNGC_EMPLOYED != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_EMPLOYED", RngCountEntity.RNGC_EMPLOYED));

                if (RngCountEntity.RNGC_DOB_NA != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_DOB_NA", RngCountEntity.RNGC_DOB_NA));

                if (RngCountEntity.RNGC_RACE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RNGC_RACE", RngCountEntity.RNGC_RACE));

                sqlParamList.Add(new SqlParameter("@Mode", RngCountEntity.Mode));


                boolstatus = CaseMst.InsertupdatedelRngCounts(sqlParamList);

            }

            catch (Exception ex)
            {

                boolstatus = false;
            }

            return boolstatus;
        }

        public bool ConvertionTableData(string strType, string strConditions)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Type", strType));
                sqlParamList.Add(new SqlParameter("@Conditions", strConditions));

                boolstatus = CaseMst.ConvertionTableData(sqlParamList);

            }

            catch (Exception ex)
            {

                boolstatus = false;
            }

            return boolstatus;
        }

        public bool CheckRequiredCase2001ControlsData(string HIE, List<CaseSnpEntity> casesnplist, CaseMstEntity casemst2001data, out string strAllReqFields)
        {
            bool boolrequiredsnp = true;
            string strFields = string.Empty;
            strAllReqFields = string.Empty;
            string strReqFields = string.Empty;
            string strSnpReqFields = string.Empty;
            List<FldcntlHieEntity> Cntl2001Entity = Model.FieldControls.GetFLDCNTLHIE("CASE2001", HIE, "FLDCNTL", "Y");

            foreach (CaseSnpEntity casesnpdata in casesnplist)
            {
                strReqFields = string.Empty;
                strSnpReqFields = string.Empty;
                foreach (FldcntlHieEntity entity in Cntl2001Entity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;
                    if (casesnpdata.FamilySeq == casemst2001data.FamilySeq)
                    {
                        switch (entity.FldCode)
                        {
                            case Consts.CASE2001.AssignedWorker:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.IntakeWorker.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.HN:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Hn.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Street:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Street.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Suffix:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Suffix.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Floor:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Flr.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Direction:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Direction.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Apartment:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Apt.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Precinct:
                                //if (enabled) { txtPrecinct.Enabled = lblPrecinct.Enabled = true; if (required) lblPrecinctReq.Visible = true; } else { txtPrecinct.Enabled = lblPrecinct.Enabled = false; lblPrecinctReq.Visible = false; }
                                //if (((ListItem)cmbOutService.SelectedItem).Value.ToString() != "I" && ((ListItem)cmbOutService.SelectedItem).Value.ToString() != "0")
                                //{
                                //    lblPrecinctReq.Visible = false;
                                //}
                                break;
                            case Consts.CASE2001.CaseType:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.CaseType.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.City:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.City.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.ZipCode:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Zip.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Township:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.TownShip.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.County:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.County.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.State:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.State.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.NoOfYearsAtThisAddress:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AddressYears.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Active:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ActiveStatus.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Secret:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Secret.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.InitialDate:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.InitialDate.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.IntakeDate:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.IntakeDate.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.CaseReviewDate:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.CaseReviewDate.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.HousingSituation:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Housing.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.PrimaryLanguage:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Language.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.SecondaryLanguage:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.LanguageOt.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.FamilyType:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.FamilyType.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.WaitingList:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.WaitList.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Site:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Site.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Phone:
                                if (required)
                                {
                                    if (!casemst2001data.HomePhone_NA.Equals("Y"))
                                    {
                                        if (string.IsNullOrEmpty(casemst2001data.Phone.Trim()))
                                        {
                                            boolrequiredsnp = false;
                                            strReqFields = strReqFields + entity.FldDesc + ",";
                                        }
                                    }
                                }
                                break;
                            case Consts.CASE2001.Message:
                                if (required)
                                {
                                    if (!casemst2001data.MessagePhone_NA.Equals("Y"))
                                    {
                                        if (string.IsNullOrEmpty(casemst2001data.MessagePhone.Trim()))
                                        {
                                            boolrequiredsnp = false;
                                            strReqFields = strReqFields + entity.FldDesc + ",";
                                        }
                                    }
                                }
                                break;
                            case Consts.CASE2001.TTY:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.TtyNumber.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Cell:
                                if (required)
                                {
                                    if (!casemst2001data.CellPhone_NA.Equals("Y"))
                                    {
                                        if (string.IsNullOrEmpty(casemst2001data.CellPhone.Trim()))
                                        {
                                            boolrequiredsnp = false;
                                            strReqFields = strReqFields + entity.FldDesc + ",";
                                        }
                                    }
                                }
                                break;
                            case Consts.CASE2001.Fax:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.FaxNumber.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Email:
                                if (required)
                                {
                                    if (!casemst2001data.Email_NA.Equals("Y"))
                                    {
                                        if (string.IsNullOrEmpty(casemst2001data.Email.Trim()))
                                        {
                                            boolrequiredsnp = false;
                                            strReqFields = strReqFields + entity.FldDesc + ",";
                                        }
                                    }
                                   
                                }
                                break;
                            case Consts.CASE2001.WhatIsTheBestWayToContact:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.BestContact.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.HowDidYouFindOutAboutUs:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AboutUs.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            //case Consts.CASE2001.WhatServicesAreYouInquiringAbout:
                            //    if (required)
                            //    {
                            //        if (string.IsNullOrEmpty(casemst2001data.What.Trim()))
                            //        {
                            //            boolrequiredsnp = false;
                            //            goto reqvalidation;
                            //            break;
                            //        }
                            //    }
                            //    break;
                            case Consts.CASE2001.RecentMortage:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpRent.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.WaterSewer:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpWater.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Electric:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpElectric.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Heating:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpHeat.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.CreditCardDebit:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Debtcc.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.LoansDebt:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.DebtLoans.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.MedicalDebit:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.DebtMed.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.OtherDebit:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.DebtOther.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.VerifiedthatallHouseholdExpensesentered:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpCaseWorker.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Dwelling:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Dwelling.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.Primaryopfh:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.HeatIncRent.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.PrimarySourceHeat:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.Source.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.MiscExpenses:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.ExpMisc.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.MiscDebit:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.DebtMisc.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.PhysicalAssets:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AsetPhy.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.LiquidAssets:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AsetLiq.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.OtherAssets:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AsetOth.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                            case Consts.CASE2001.MiscAssets:
                                if (required)
                                {
                                    if (string.IsNullOrEmpty(casemst2001data.AsetMisc.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strReqFields = strReqFields + entity.FldDesc + ",";
                                    }
                                }
                                break;
                        }
                    }


                    switch (entity.FldCode)
                    {
                        case Consts.CASE2001.SSN:
                            if (required)
                            {
                                if (casesnpdata.Ssno == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                    //goto reqvalidation;
                                    //break;
                                }
                            }
                            break;
                        case Consts.CASE2001.FirstName:
                            if (required)
                            {
                                if (casesnpdata.NameixFi == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.LastName:
                            if (required)
                            {
                                if (casesnpdata.NameixLast == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.MI:
                            if (required)
                            {
                                if (casesnpdata.NameixMi == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.DateOfBirth:
                            if (required)
                            {
                                if (!casesnpdata.DobNa.Equals("1"))
                                {
                                    if (casesnpdata.AltBdate == string.Empty)
                                    {
                                        boolrequiredsnp = false;
                                        strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                    }
                                }
                            }
                            break;
                        case Consts.CASE2001.Alias:
                            if (required)
                            {
                                if (casesnpdata.Alias == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Relation:
                            if (required)
                            {
                                if (casesnpdata.MemberCode == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Ethnicity:
                            if (required)
                            {
                                if (casesnpdata.Ethnic == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Race:
                            if (required)
                            {
                                if (casesnpdata.Race == string.Empty)
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Education:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Education.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.School:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.SchoolDistrict.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Gender:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Sex.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.AreYouPregnant:
                            if (required)
                            {
                                if (casesnpdata.Sex.Trim().ToString() == "F")
                                {
                                    if (string.IsNullOrEmpty(casesnpdata.Pregnant.Trim()))
                                    {
                                        boolrequiredsnp = false;
                                        strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                    }
                                }
                            }
                            break;
                        case Consts.CASE2001.MaritalStatus:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.MaritalStatus.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.HealthInsurance:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.HealthIns.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                                else
                                {
                                    if (casesnpdata.HealthIns.Trim() == "Y")
                                    {
                                        if (string.IsNullOrEmpty(casesnpdata.Health_Codes.Trim()))
                                        {
                                            boolrequiredsnp = false;
                                            strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                        }
                                    }
                                }
                            }
                            break;
                        case Consts.CASE2001.VeteranCode:
                            if (required)
                            {
                                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
                                //{
                                if (string.IsNullOrEmpty(casesnpdata.MilitaryStatus.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                                //}
                                //else
                                //{
                                //    if (string.IsNullOrEmpty(casesnpdata.Vet.Trim()))
                                //    {
                                //        boolrequiredsnp = false;
                                //        goto reqvalidation;
                                //        break;
                                //    }
                                //}
                            }
                            break;
                        case Consts.CASE2001.FoodStamps:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.FootStamps.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Disabled:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Disable.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Farmer:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Farmer.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.WorkStatus:
                            if (required)
                            {
                                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
                                //{
                                if (string.IsNullOrEmpty(casesnpdata.WorkStatus.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                                //}
                            }
                            break;
                        case Consts.CASE2001.NonCashBenefits:
                            if (required)
                            {
                                //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
                                //{
                                if (string.IsNullOrEmpty(casesnpdata.NonCashBenefits.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                                // }
                            }
                            break;
                        //case Consts.CASE2001.Youth:
                        //    if (required)
                        //    {
                        //        //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "COI" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OK" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "SCCAP")
                        //        //{
                        //        if (string.IsNullOrEmpty(casesnpdata.Youth.Trim()))
                        //        {
                        //            boolrequiredsnp = false;
                        //            goto reqvalidation;
                        //            break;
                        //        }
                        //        // }
                        //    }
                        //    break;
                        case Consts.CASE2001.WIC:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Wic.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.ReliableTransport:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Relitran.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.DriversLicense:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Drvlic.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.Resident:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Resident.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        //case Consts.CASE2001.AlienRegNo:
                        //    if (enabled) { txtAlienRegNo.Enabled = lblAlienRegNo.Enabled = true; if (required) AlienRegNo.Visible = true; } else { txtAlienRegNo.Enabled = lblAlienRegNo.Enabled = false; AlienRegNo.Visible = false; }
                        //    break;
                        //case Consts.CASE2001.LegalToWork:
                        //    if (enabled) { cmbLegaltowork.Enabled = lblLegalToWork.Enabled = true; if (required) LegalToWork.Visible = true; } else { cmbLegaltowork.Enabled = lblLegalToWork.Enabled = false; LegalToWork.Visible = false; }
                        //    break;
                        //case Consts.CASE2001.ExpirationDate:
                        //    if (enabled) { dtExpirationdate.Enabled = lblExpirationDate.Enabled = true; if (required) ExpirationDateReq.Visible = true; } else { dtExpirationdate.Enabled = lblExpirationDate.Enabled = false; ExpirationDateReq.Visible = false; }
                        //    break;
                        case Consts.CASE2001.SnpActive:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Status.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.ExcludeMember:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Exclude.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        //case Consts.CASE2001.SSNReason:
                        //    if (enabled) { cmbSSNReason.Enabled = lblSSNReason.Enabled = true; if (required) lblSSNReasonReq.Visible = true; } else { cmbSSNReason.Enabled = false; }
                        //    break;

                        // ******************************************************* Employee Details ********************************************** //
                        case Consts.CASE2001.EEmployed:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.Employed.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        //case Consts.CASE2001.ELastWorkDate:
                        //    if (enabled) { dtElastDateWorked.Enabled = lblDOB.Enabled = true; if (required) DOBReq.Visible = true; } else { dtBirth.Enabled = lblDOB.Enabled = checkNA.Enabled = false; DOBReq.Visible = false; }
                        //    break;
                        case Consts.CASE2001.EWorkLimit:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.WorkLimit.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        //case Consts.CASE2001.EExplainworkLimit:
                        //    if (enabled) { txtEExp.Enabled = lblAlias.Enabled = true; if (required) AliasReq.Visible = true; } else { txtAlias.Enabled = lblAlias.Enabled = false; AliasReq.Visible = false; }
                        //    break;
                        case Consts.CASE2001.ENumberofcjobs:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.NumberOfcjobs.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.ENumberofLvjobs:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.NumberofLvjobs.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EFullTimeHours:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.FullTimeHours.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EPartTimeHours:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.PartTimeHours.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.ESeasonalEmploy:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.SeasonalEmploy.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        // chk
                        case Consts.CASE2001.EShiftWorked1st:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.IstShift.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EShiftWorked2nd:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.IIndShift.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EShiftWorked3rd:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.IIIrdShift.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EShiftWorkedRotating:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.RShift.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EEmployerName:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.EmployerName.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EEmployerStreet:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.EmployerStreet.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EEmployerCity:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.EmployerCity.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EPhone:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.EmplPhone.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EExt:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.EmplExt.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EJobTitle:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.JobTitle.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EJobCategory:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.JobCategory.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EHourlyWage:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.HourlyWage.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EPayFreQuency:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.PayFrequency.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                        case Consts.CASE2001.EHireDate:
                            if (required)
                            {
                                if (string.IsNullOrEmpty(casesnpdata.HireDate.Trim()))
                                {
                                    boolrequiredsnp = false;
                                    strSnpReqFields = strSnpReqFields + entity.FldDesc + ",";
                                }
                            }
                            break;
                    }

                }
                if (strSnpReqFields != string.Empty)
                {
                    strSnpReqFields = LookupDataAccess.GetMemberName(casesnpdata.NameixFi, casesnpdata.NameixMi, casesnpdata.NameixLast, "1") + "   Member Missing Fields : \n" + strSnpReqFields + "\n";
                }
                if (casesnpdata.FamilySeq == casemst2001data.FamilySeq)
                {
                    if (strReqFields != string.Empty)
                    {
                        strReqFields = "Case MST Missing Fields : \n" + strReqFields + "\n";
                        strAllReqFields = strAllReqFields + strReqFields + strSnpReqFields;
                    }
                    else
                    {
                        strAllReqFields = strAllReqFields + strSnpReqFields;
                    }
                }
                else
                {
                    strAllReqFields = strAllReqFields + strSnpReqFields;
                }
            }
            //reqvalidation:
            //    return boolrequiredsnp;


            return boolrequiredsnp;
        }

        public bool CheckRequiredCustomQuestionsData(string HIE, CaseSnpEntity casesnpdata)
        {
            bool boolcustomvalid = true;
            List<CustomQuestionsEntity> custQuestions = Model.FieldControls.GetCustomQuestions("CASE2001", "A", HIE, "Sequence", "ACTIVE", "P");
            custQuestions = custQuestions.FindAll(u => u.CUSTREQUIRED.ToUpper() == "Y").ToList();
            List<CustomQuestionsEntity> custResponses = Model.CaseMstData.GetCustomQuestionAnswers(casesnpdata);
            foreach (CustomQuestionsEntity dr in custQuestions)
            {
                string custCode = dr.CUSTCODE.ToString();
                List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                if (response.Count == 0)
                {
                    boolcustomvalid = false;
                    break;
                }
            }
            if (boolcustomvalid)
            {
                List<CustomQuestionsEntity> custQuestionshouse = Model.FieldControls.GetCustomQuestions("CASE2001", "H", HIE, "Sequence", "ACTIVE", string.Empty);
                custQuestionshouse = custQuestionshouse.FindAll(u => u.CUSTREQUIRED.ToUpper() == "Y").ToList();
                foreach (CustomQuestionsEntity dr in custQuestionshouse)
                {
                    string custCode = dr.CUSTCODE.ToString();
                    List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                    if (response.Count == 0)
                    {
                        boolcustomvalid = false;
                        break;
                    }
                }
            }
            return boolcustomvalid;
        }

        public bool CheckRequiredCaseverdata(string HIE, CaseVerEntity caseverdata, out string strVerRequired)
        {
            bool boolrequiredsnp = true;
            strVerRequired = string.Empty;
            List<FldcntlHieEntity> FLDCNTLHieEntity = Model.FieldControls.GetFLDCNTLHIE("CASE2003", HIE, "FLDCNTL", "Y");
            foreach (FldcntlHieEntity entity in FLDCNTLHieEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    //case Consts.CASE2003.FundingSource:
                    //    if (required)
                    //    {
                    //        if (caseverdata.FundSource == string.Empty)
                    //        {   
                    //            boolrequiredsnp = false;
                    //            strVerRequired = strVerRequired + entity.FldDesc + ",";                                  
                    //        }
                    //    }
                    //    break;
                    case Consts.CASE2003.IncomeVerified:
                        if (required)
                        {

                            if ((caseverdata.VerifyW2 == "N") && (caseverdata.VerifyCheckStub == "N") && (caseverdata.VerifyLetter == "N") && (caseverdata.VerifyOther == "N") && (caseverdata.VerifyTaxReturn == "N") && (caseverdata.VerifySelfDecl=="N"))
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.inProgram:
                        if (required)
                        {
                            if (caseverdata.NoInhh == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.Income:
                        if (required)
                        {
                            if (caseverdata.IncomeAmount == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.Meal:
                        if (required)
                        {
                            if (caseverdata.MealElig == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.Categorical:
                        if (required)
                        {
                            if (caseverdata.CatElig == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.ReverificationDate:
                        if (required)
                        {
                            if (caseverdata.ReverifyDate == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.VerificationDate:
                        if (required)
                        {
                            if (caseverdata.VerifyDate == string.Empty)
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                            }
                        }
                        break;
                    case Consts.CASE2003.Verifier:
                        if (required)
                        {
                            if (caseverdata.Verifier == string.Empty || caseverdata.Verifier == "0")
                            {
                                boolrequiredsnp = false;
                                strVerRequired = strVerRequired + entity.FldDesc + ",";
                                //goto reqvalidation;
                                //break;
                            }
                        }
                        break;
                }

            }
            //reqvalidation:
            //    return boolrequiredsnp;

            return boolrequiredsnp;

        }

        public string DisplayIncomeMsgs(string Agency, string Dept, string Program, string strYear, CaseSnpEntity casesnpdata, CaseMstEntity casemst2001data)
        {
            string strIncomeMsg = string.Empty;
            if (casemst2001data != null)
            {
                ProgramDefinitionEntity programEntity = Model.HierarchyAndPrograms.GetCaseDepadp(Agency, Dept, Program);
                strIncomeMsg = "";
                if (programEntity != null)
                {
                    if (programEntity.IncomeVerMsg.Equals("Y"))
                    {
                        if (string.IsNullOrEmpty(casemst2001data.EligDate))
                        {
                            strIncomeMsg = "Income Not Verified";
                        }
                        else
                        {
                            List<CaseVerEntity> caseVerList = Model.CaseMstData.GetCASEVeradpyalst(Agency, Dept, Program, strYear, casemst2001data.ApplNo, string.Empty, string.Empty);
                            if (caseVerList.Count > 0)
                            {
                                if (!(Convert.ToDecimal(casemst2001data.ProgIncome == string.Empty ? "0" : casemst2001data.ProgIncome) == Convert.ToDecimal(caseVerList[0].IncomeAmount == string.Empty ? "0" : caseVerList[0].IncomeAmount)))
                                {
                                    strIncomeMsg = "Household income needs to be reverified as the income was changed";
                                }
                            }
                            else
                            {
                                strIncomeMsg = "Income Not Verified";
                            }
                        }
                    }
                }
            }
            return strIncomeMsg;
        }


        public bool GETCodegen(string strType, out string strCodegenId)
        {
            bool boolSuccess = false;
            string strNewCodegen = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Type", strType));

                SqlParameter sqlApplNo = new SqlParameter("@Id", SqlDbType.Int);
                sqlApplNo.Value = strNewCodegen;
                sqlApplNo.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlApplNo);
                

                boolSuccess = CaseMst.GETCODEGEN(sqlParamList);
                strNewCodegen = sqlApplNo.Value.ToString();
                
            }
            catch (Exception ex)
            {

             
            }
            
            strCodegenId = strNewCodegen;           
            return boolSuccess;
        }

        public string DeleteAllApplicantData(CaseSnpEntity caseSnpEntity)
        {
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@SNP_AGENCY", caseSnpEntity.Agency));
                sqlParamList.Add(new SqlParameter("@SNP_DEPT", caseSnpEntity.Dept));
                sqlParamList.Add(new SqlParameter("@SNP_PROGRAM", caseSnpEntity.Program));
                sqlParamList.Add(new SqlParameter("@SNP_YEAR", caseSnpEntity.Year));
                sqlParamList.Add(new SqlParameter("@SNP_APP", caseSnpEntity.App));

                CaseSnpData.DELETEAPP_ALLTABLESDATA(sqlParamList);
               

            }
            catch (Exception ex)
            {
                
            }

            return strMsg;
        }

        public List<HierarchyEntity> CAPS_ADMNB002_GET(string Type, string strAgency, string strDept, string strProg, string strUserid, string strTableName)
        {
            List<HierarchyEntity> hierachyentity = new List<HierarchyEntity>();
            try
            {
                DataSet caseWorker = CaseMst.CAPS_ADMNB002_GET(Type, strAgency, strDept, strProg, strUserid);
                if (caseWorker != null && caseWorker.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in caseWorker.Tables[0].Rows)
                    {
                        hierachyentity.Add(new HierarchyEntity(row, strTableName, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return hierachyentity;
            }

            return hierachyentity;
        }

        public List<CustomQuestionsEntity> CAPS_CASEUSAGE_GET(string agency, string dep, string program, string year, string app, string strtype)
        {
            List<CustomQuestionsEntity> customEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet custResponse = CaseSnpData.CAPS_CASEUSAGE_GET(agency, dep, program, year, app, strtype);
                if (custResponse != null && custResponse.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in custResponse.Tables[0].Rows)
                    {
                        customEntity.Add(new CustomQuestionsEntity(row, "USAGEQUES"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return customEntity;
            }

            return customEntity;
        }

        public List<CASB0016Entity> GetCASB0016_Report(string agency, string dep, string program, string year, string sort)
        {
            List<CASB0016Entity> CASB0016data = new List<CASB0016Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.CaseSnpData.GetCASB0016_Report(agency, dep, program, year, sort);

                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        CASB0016data.Add(new CASB0016Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return CASB0016data;
            }

            return CASB0016data;
        }
    }
}
