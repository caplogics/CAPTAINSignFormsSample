using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Captain.Common.Utilities;
using DevExpress.Office.DigitalSignatures.Internal;

namespace Captain.Common.Model.Objects
{
    public class CaseMstEntity
    {
        #region Constructors

        public CaseMstEntity()
        {

            ApplAgency = string.Empty;
            ApplDept = string.Empty;
            ApplProgram = string.Empty;
            ApplYr = string.Empty;
            ApplNo = string.Empty;
            FamilySeq = string.Empty;
            FamilyId = string.Empty;
            ClientId = string.Empty;
            Ssn = string.Empty;
            Bic = string.Empty;
            NickName = string.Empty;
            EthnicOther = string.Empty;
            State = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Hn = string.Empty;
            Direction = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            Zip = string.Empty;
            Zipplus = string.Empty;
            Precinct = string.Empty;
            Area = string.Empty;
            Phone = string.Empty;
            NextYear = string.Empty;
            Classification = string.Empty;
            Language = string.Empty;
            LanguageOt = string.Empty;
            IntakeWorker = string.Empty;
            IntakeDate = string.Empty;
            InitialDate = string.Empty;
            CaseType = string.Empty;
            Housing = string.Empty;
            FamilyType = string.Empty;
            Site = string.Empty;
            Juvenile = string.Empty;
            Senior = string.Empty;
            Secret = string.Empty;
            CaseReviewDate = string.Empty;
            CompleteDate = string.Empty;
            AlertCodes = string.Empty;
            ParentStatus = string.Empty;
            IntakeHrs = string.Empty;
            IntakeMns = string.Empty;
            IntakeScs = string.Empty;
            FinHrs = string.Empty;
            FinMns = string.Empty;
            FinScs = string.Empty;
            SimHrs = string.Empty;
            SimMns = string.Empty;
            SimScs = string.Empty;
            MedHrs = string.Empty;
            MedMns = string.Empty;
            MedScs = string.Empty;
            Rank1 = string.Empty;
            Rank2 = string.Empty;
            Rank3 = string.Empty;
            Rank4 = string.Empty;
            Rank5 = string.Empty;
            Rank6 = string.Empty;
            Position1 = string.Empty;
            Position2 = string.Empty;
            Position3 = string.Empty;
            TownShip = string.Empty;
            IntakeTime1 = string.Empty;
            SsnFlag = string.Empty;
            StateCase = string.Empty;
            Verifier = string.Empty;
            EligDate = string.Empty;
            CatElig = string.Empty;
            MealElig = string.Empty;
            VerifyW2 = string.Empty;
            VerifyCheckStub = string.Empty;
            VerifyTaxReturn = string.Empty;
            VerifyLetter = string.Empty;
            VerifyOther = string.Empty;
            ReverifyDate = string.Empty;
            IncomeTypes = string.Empty;
            Poverty = string.Empty;
            WaitList = string.Empty;
            ActiveStatus = string.Empty;
            TotalRank = string.Empty;
            NoInhh = string.Empty;
            FamIncome = string.Empty;
            NoInProg = string.Empty;
            ProgIncome = string.Empty;
            OutOfService = string.Empty;
            Hud = string.Empty;
            Smi = string.Empty;
            Cmi = string.Empty;
            County = string.Empty;
            AddressYears = string.Empty;
            MessagePhone = string.Empty;
            CellPhone = string.Empty;
            FaxNumber = string.Empty;
            TtyNumber = string.Empty;
            Email = string.Empty;
            BestContact = string.Empty;
            AboutUs = string.Empty;
            ImportDate = string.Empty;
            DateAdded = string.Empty;
            ExpRent = string.Empty;
            ExpWater = string.Empty;
            ExpElectric = string.Empty;
            ExpHeat = string.Empty;
            Debtcc = string.Empty;
            DebtLoans = string.Empty;
            DebtMed = string.Empty;
            DebtOther = string.Empty;
            ExpTotal = string.Empty;
            ExpLivexpense = string.Empty;
            ExpCaseWorker = string.Empty;
            Dwelling = string.Empty;
            HeatIncRent = string.Empty;
            Source = string.Empty;
            RollOver = string.Empty;
            RiskValue = string.Empty;
            SubShouse = string.Empty;
            SubStype = string.Empty;
            VerFund = string.Empty;
            OmbScreen = string.Empty;
            CbCaseManager = string.Empty;
            CaseManager = string.Empty;
            VerifyOthCmb = string.Empty;
            SimPrint = string.Empty;
            SimPrintDate = string.Empty;
            CbFraud = string.Empty;
            FraudDate = string.Empty;
            DateAdd1 = string.Empty;
            AddOperator1 = string.Empty;
            DateLstc1 = string.Empty;
            LstcOperator1 = string.Empty;
            TimesUpdated1 = string.Empty;
            DateAdd2 = string.Empty;
            AddOperator2 = string.Empty;
            DateLstc2 = string.Empty;
            LstcOperator2 = string.Empty;
            TimesUpdated2 = string.Empty;
            DateAdd3 = string.Empty;
            AddOperator3 = string.Empty;
            DateLstc3 = string.Empty;
            LstcOperator3 = string.Empty;
            TimesUpdated3 = string.Empty;
            DateAdd4 = string.Empty;
            AddOperator4 = string.Empty;
            DateLstc4 = string.Empty;
            LstcOperator4 = string.Empty;
            TimesUpdated4 = string.Empty;
            Type = string.Empty;

            AssignWorkerDesc = string.Empty;
            CaseTypeDesc = string.Empty;
            TownShipDesc = string.Empty;
            CountyDesc = string.Empty;
            HousingSituvationDesc = string.Empty;
            PrimaryLanDesc = string.Empty;
            SecondaryLanDesc = string.Empty;
            FamilyTypeDesc = string.Empty;
            WaitingListDesc = string.Empty;
            ContactusDesc = string.Empty;
            AboutUsDesc = string.Empty;
            CasworkerHousDesc = string.Empty;
            ExpCaseworkerDesc = string.Empty;
            DebtMisc = string.Empty;
            AsetLiq = string.Empty;
            AsetMisc = string.Empty;
            AsetOth = string.Empty;
            AsetPhy = string.Empty;
            ExpMisc = string.Empty;
            DebtTotal = string.Empty;
            AsetTotal = string.Empty;
            AsetRatio = string.Empty;
            DebIncmRation = string.Empty;
            MiddleName = string.Empty;
            PressTotal = string.Empty;
            PressCat = string.Empty;
            PressGrp = string.Empty;
            ModuleCode = string.Empty;
            MstNCashBen = string.Empty;
            //PJob = string.Empty;
            //PHSD = string.Empty;
            //PSkills = string.Empty;
            //PHousing = string.Empty;
            //PTransport = string.Empty;
            //PChldCare = string.Empty;
            //PCCENRL = string.Empty;
            //PELDCARE = string.Empty;
            //PECNEED = string.Empty;
            //PECHINS = string.Empty;
            //PAHINS = string.Empty;
            //PRWENG = string.Empty;
            //PCURRDSS = string.Empty;
            //PRECVDSS = string.Empty;

            LPM0001 = string.Empty;
            LPM0002 = string.Empty;
            LPM0003 = string.Empty;
            LPM0004 = string.Empty;
            LPM0005 = string.Empty;
            LPM0006 = string.Empty;
            LPM0007 = string.Empty;
            LPM0008 = string.Empty;
            LPM0009 = string.Empty;
            LPM0010 = string.Empty;
            LPM0011 = string.Empty;
            LPM0012 = string.Empty;
            LPM0013 = string.Empty;
            LPM0014 = string.Empty;
            LPM0015 = string.Empty;
            LPM0016 = string.Empty;
            LPM0017 = string.Empty;
            ApplictionDate = string.Empty;
            ApplictionType = string.Empty;
            VERIFY_SELF_DECL = string.Empty;
            //PJobDesc = string.Empty;
            //PHSDDesc = string.Empty;
            //PSkillsDesc = string.Empty;
            //PHousingDesc = string.Empty;
            //PTransportDesc = string.Empty;
            //PChldCareDesc = string.Empty;
            //PCCENRLDesc = string.Empty;
            //PELDCAREDesc = string.Empty;
            //PECNEEDDesc = string.Empty;
            //PECHINSDesc = string.Empty;
            //PAHINSDesc = string.Empty;
            //PRWENGDesc = string.Empty;
            //PCURRDSSDesc = string.Empty;
            //PRECVDSSDesc = string.Empty;
        }

        public CaseMstEntity(DataRow row)
        {
            if (row != null)
            {
                ApplAgency = row["MST_AGENCY"].ToString().Trim();
                ApplDept = row["MST_DEPT"].ToString().Trim();
                ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                ApplYr = row["MST_YEAR"].ToString().Trim();
                ApplNo = row["MST_APP_NO"].ToString().Trim();
                FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                FamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                ClientId = row["MST_CLIENT_ID"].ToString().Trim();
                Ssn = row["MST_SSN"].ToString().Trim();
                Bic = row["MST_BIC"].ToString().Trim();
                NickName = row["MST_NICKNAME"].ToString().Trim();
                EthnicOther = row["MST_ETHNIC_OTHER"].ToString().Trim();
                State = row["MST_STATE"].ToString().Trim();
                City = row["MST_CITY"].ToString().Trim();
                Street = row["MST_STREET"].ToString().Trim();
                Suffix = row["MST_SUFFIX"].ToString().Trim();
                Hn = row["MST_HN"].ToString().Trim();
                Direction = row["MST_DIRECTION"].ToString().Trim();
                Apt = row["MST_APT"].ToString().Trim();
                Flr = row["MST_FLR"].ToString().Trim();
                Zip = row["MST_ZIP"].ToString().Trim();
                Zipplus = row["MST_ZIPPLUS"].ToString().Trim();
                Precinct = row["MST_PRECINCT"].ToString().Trim();
                Area = row["MST_AREA"].ToString().Trim();
                Phone = row["MST_PHONE"].ToString().Trim();
                NextYear = row["MST_NEXTYEAR"].ToString().Trim();
                Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                Language = row["MST_LANGUAGE"].ToString().Trim();
                LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                InitialDate = row["MST_INITIAL_DATE"].ToString().Trim();
                CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                Housing = row["MST_HOUSING"].ToString().Trim();
                FamilyType = row["MST_FAMILY_TYPE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Juvenile = row["MST_JUVENILE"].ToString().Trim();
                Senior = row["MST_SENIOR"].ToString().Trim();
                Secret = row["MST_SECRET"].ToString().Trim();
                CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                CompleteDate = row["MST_CASE_COMPLETE_DATE"].ToString().Trim();
                AlertCodes = row["MST_ALERT_CODES"].ToString().Trim();
                ParentStatus = row["MST_PARENT_STATUS"].ToString().Trim();
                IntakeHrs = row["MST_INTAKE_HRS"].ToString().Trim();
                IntakeMns = row["MST_INTAKE_MNS"].ToString().Trim();
                IntakeScs = row["MST_INTAKE_SCS"].ToString().Trim();
                FinHrs = row["MST_FIN_HRS"].ToString().Trim();
                FinMns = row["MST_FIN_MNS"].ToString().Trim();
                FinScs = row["MST_FIN_SCS"].ToString().Trim();
                SimHrs = row["MST_SIM_HRS"].ToString().Trim();
                SimMns = row["MST_SIM_MNS"].ToString().Trim();
                SimScs = row["MST_SIM_SCS"].ToString().Trim();
                MedHrs = row["MST_MED_HRS"].ToString().Trim();
                MedMns = row["MST_MED_MNS"].ToString().Trim();
                MedScs = row["MST_MED_SCS"].ToString().Trim();
                Rank1 = row["MST_RANK1"].ToString().Trim();
                Rank2 = row["MST_RANK2"].ToString().Trim();
                Rank3 = row["MST_RANK3"].ToString().Trim();
                Rank4 = row["MST_RANK4"].ToString().Trim();
                Rank5 = row["MST_RANK5"].ToString().Trim();
                Rank6 = row["MST_RANK6"].ToString().Trim();
                Position1 = row["MST_POSITION1"].ToString().Trim();
                Position2 = row["MST_POSITION2"].ToString().Trim();
                Position3 = row["MST_POSITION3"].ToString().Trim();
                TownShip = row["MST_TOWNSHIP"].ToString().Trim();
                IntakeTime1 = row["MST_INTAKE_TIME1"].ToString().Trim();
                SsnFlag = row["MST_SSN_FLAG"].ToString().Trim();
                StateCase = row["MST_STATE_CASE"].ToString().Trim();
                Verifier = row["MST_VERIFIER"].ToString().Trim();
                EligDate = row["MST_ELIG_DATE"].ToString().Trim();
                CatElig = row["MST_CAT_ELIG"].ToString().Trim();
                MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                VerifyW2 = row["MST_VERIFY_W2"].ToString().Trim();
                VerifyCheckStub = row["MST_VERIFY_CHECK_STUB"].ToString().Trim();
                VerifyTaxReturn = row["MST_VERIFY_TAX_RETURN"].ToString().Trim();
                VerifyLetter = row["MST_VERIFY_LETTER"].ToString().Trim();
                VerifyOther = row["MST_VERIFY_OTHER"].ToString().Trim();
                ReverifyDate = row["MST_REVERIFY_DATE"].ToString().Trim();
                IncomeTypes = row["MST_INCOME_TYPES"].ToString().Trim();
                Poverty = row["MST_POVERTY"].ToString().Trim();
                WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
                TotalRank = row["MST_TOTAL_RANK"].ToString().Trim();
                NoInhh = row["MST_NO_INHH"].ToString().Trim();
                FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                NoInProg = row["MST_NO_INPROG"].ToString().Trim();
                ProgIncome = row["MST_PROG_INCOME"].ToString().Trim();
                OutOfService = row["MST_OUT_OF_SERVICE"].ToString().Trim();
                Hud = row["MST_HUD"].ToString().Trim();
                Smi = row["MST_SMI"].ToString().Trim();
                Cmi = row["MST_CMI"].ToString().Trim();
                County = row["MST_COUNTY"].ToString().Trim();
                AddressYears = row["MST_ADDRESS_YEARS"].ToString().Trim();
                MessagePhone = row["MST_MESSAGE_PHONE"].ToString().Trim();
                CellPhone = row["MST_CELL_PHONE"].ToString().Trim();
                FaxNumber = row["MST_FAX_NUMBER"].ToString().Trim();
                TtyNumber = row["MST_TTY_NUMBER"].ToString().Trim();
                Email = row["MST_EMAIL"].ToString().Trim();
                BestContact = row["MST_BEST_CONTACT"].ToString().Trim();
                AboutUs = row["MST_ABOUT_US"].ToString().Trim();
                ImportDate = row["MST_IMPORT_DATE"].ToString().Trim();
                DateAdded = row["MST_DATE_ADDED"].ToString().Trim();
                ExpRent = row["MST_EXP_RENT"].ToString().Trim();
                ExpWater = row["MST_EXP_WATER"].ToString().Trim();
                ExpElectric = row["MST_EXP_ELECTRIC"].ToString().Trim();
                ExpHeat = row["MST_EXP_HEAT"].ToString().Trim();
                Debtcc = row["MST_DEBT_CC"].ToString().Trim();
                DebtLoans = row["MST_DEBT_LOANS"].ToString().Trim();
                DebtMed = row["MST_DEBT_MED"].ToString().Trim();
                DebtOther = row["MST_DEBT_OTH"].ToString().Trim();
                ExpTotal = row["MST_EXP_TOTAL"].ToString().Trim();
                ExpLivexpense = row["MST_EXP_LIVEXPENSE"].ToString().Trim();
                ExpCaseWorker = row["MST_EXP_CASEWORKER"].ToString().Trim();
                Dwelling = row["MST_DWELLING"].ToString().Trim();
                HeatIncRent = row["MST_HEAT_INC_RENT"].ToString().Trim();
                Source = row["MST_SOURCE"].ToString().Trim();
                RollOver = row["MST_ROLLOVER"].ToString().Trim();
                RiskValue = row["MST_RISK_VALUE"].ToString().Trim();
                SubShouse = row["MST_SUBSHOUSE"].ToString().Trim();
                SubStype = row["MST_SUBSTYPE"].ToString().Trim();
                VerFund = row["MST_VER_FUND"].ToString().Trim();
                OmbScreen = row["MST_OMB_SCREEN"].ToString().Trim();
                CbCaseManager = row["MST_CB_CASE_MANAGER"].ToString().Trim();
                CaseManager = row["MST_CASE_MANAGER"].ToString().Trim();
                VerifyOthCmb = row["MST_VERIFY_OTH_CMB"].ToString().Trim();
                SimPrint = row["MST_SIM_PRINT"].ToString().Trim();
                SimPrintDate = row["MST_SIM_PRINT_DATE"].ToString().Trim();
                CbFraud = row["MST_CB_FRAUD"].ToString().Trim();
                FraudDate = row["MST_FRAUD_DATE"].ToString().Trim();
                DateAdd1 = row["MST_DATE_ADD_1"].ToString().Trim();
                AddOperator1 = row["MST_ADD_OPERATOR_1"].ToString().Trim();
                DateLstc1 = row["MST_DATE_LSTC_1"].ToString().Trim();
                LstcOperator1 = row["MST_LSTC_OPERATOR_1"].ToString().Trim();
                TimesUpdated1 = row["MST_TIMES_UPDATED_1"].ToString().Trim();
                DateAdd2 = row["MST_DATE_ADD_2"].ToString().Trim();
                AddOperator2 = row["MST_ADD_OPERATOR_2"].ToString().Trim();
                DateLstc2 = row["MST_DATE_LSTC_2"].ToString().Trim();
                LstcOperator2 = row["MST_LSTC_OPERATOR_2"].ToString().Trim();
                TimesUpdated2 = row["MST_TIMES_UPDATED_2"].ToString().Trim();
                DateAdd3 = row["MST_DATE_ADD_3"].ToString().Trim();
                AddOperator3 = row["MST_ADD_OPERATOR_3"].ToString().Trim();
                DateLstc3 = row["MST_DATE_LSTC_3"].ToString().Trim();
                LstcOperator3 = row["MST_LSTC_OPERATOR_3"].ToString().Trim();
                TimesUpdated3 = row["MST_TIMES_UPDATED_3"].ToString().Trim();
                DateAdd4 = row["MST_DATE_ADD_4"].ToString().Trim();
                AddOperator4 = row["MST_ADD_OPERATOR_4"].ToString().Trim();
                DateLstc4 = row["MST_DATE_LSTC_4"].ToString().Trim();
                LstcOperator4 = row["MST_LSTC_OPERATOR_4"].ToString().Trim();
                //DateLstc5 = row["MST_DATE_LSTC_5"].ToString().Trim();
                //LstcOperator5 = row["MST_LSTC_OPERATOR_5"].ToString().Trim();
                TimesUpdated4 = row["MST_TIMES_UPDATED_4"].ToString().Trim();
                PressTotal = row["MST_PRESS_TOTAL"].ToString().Trim();
                PressCat = row["MST_PRESS_CAT"].ToString().Trim();
                PressGrp = row["MST_PRESS_GRP"].ToString().Trim();

                DebtMisc = row["MST_DEBT_MISC"].ToString().Trim();
                AsetLiq = row["MST_ASET_LIQ"].ToString().Trim();
                AsetMisc = row["MST_ASET_MISC"].ToString().Trim();
                AsetOth = row["MST_ASET_OTH"].ToString().Trim();
                AsetPhy = row["MST_ASET_PHY"].ToString().Trim();
                ExpMisc = row["MST_EXP_MISC"].ToString().Trim();
                DebtTotal = row["MST_DEBT_TOTAL"].ToString().Trim();
                AsetTotal = row["MST_ASET_TOTAL"].ToString().Trim();
                AsetRatio = row["MST_DEB_ASET_RATIO"].ToString().Trim();
                DebIncmRation = row["MST_DEB_INCM_RATIO"].ToString().Trim();

                //PJob = row["MST_PRESS_JOB"].ToString().Trim();
                //PHSD = row["MST_PRESS_HSD"].ToString().Trim();
                //PSkills = row["MST_PRESS_SKILLS"].ToString().Trim();
                //PHousing = row["MST_PRESS_HOUSING"].ToString().Trim();
                //PTransport = row["MST_PRESS_TRANSPORT"].ToString().Trim();
                //PChldCare = row["MST_PRESS_CHLDCARE"].ToString().Trim();
                //PCCENRL = row["MST_PRESS_CCENRL"].ToString().Trim();
                //PELDCARE = row["MST_PRESS_ELDRCARE"].ToString().Trim();
                //PECNEED = row["MST_PRESS_ECNEED"].ToString().Trim();
                //PECHINS = row["MST_PRESS_CHINS"].ToString().Trim();
                //PAHINS = row["MST_PRESS_AHINS"].ToString().Trim();
                //PRWENG = row["MST_PRESS_RW_ENG"].ToString().Trim();
                //PCURRDSS = row["MST_PRESS_CURR_DSS"].ToString().Trim();
                //PRECVDSS = row["MST_PRESS_RECV_DSS"].ToString().Trim();


                LPM0001 = row["MST_LPM_0001"].ToString().Trim();
                LPM0002 = row["MST_LPM_0002"].ToString().Trim();
                LPM0003 = row["MST_LPM_0003"].ToString().Trim();
                LPM0004 = row["MST_LPM_0004"].ToString().Trim();
                LPM0005 = row["MST_LPM_0005"].ToString().Trim();
                LPM0006 = row["MST_LPM_0006"].ToString().Trim();
                LPM0007 = row["MST_LPM_0007"].ToString().Trim();
                LPM0008 = row["MST_LPM_0008"].ToString().Trim();
                LPM0009 = row["MST_LPM_0009"].ToString().Trim();
                LPM0010 = row["MST_LPM_0010"].ToString().Trim();
                LPM0011 = row["MST_LPM_0011"].ToString().Trim();
                LPM0012 = row["MST_LPM_0012"].ToString().Trim();
                LPM0013 = row["MST_LPM_0013"].ToString().Trim();
                LPM0014 = row["MST_LPM_0014"].ToString().Trim();
                LPM0015 = row["MST_LPM_0015"].ToString().Trim();
                LPM0016 = row["MST_LPM_0016"].ToString().Trim();
                LPM0017 = row["MST_LPM_0017"].ToString().Trim();
                MstNCashBen = row["MST_NCASHBEN"].ToString().Trim();
                ApplictionType = row["MST_APPLICANT_TYPE"].ToString().Trim();
                ApplictionDate = row["MST_APPLICANT_DATE"].ToString().Trim();

                HomePhone_NA = row["MST_HOME_NA"].ToString().Trim();
                CellPhone_NA = row["MST_CELL_NA"].ToString().Trim();
                MessagePhone_NA = row["MST_MESSAGE_NA"].ToString().Trim();
                Email_NA = row["MST_EMAIL_NA"].ToString().Trim();
                VERIFY_SELF_DECL = row["MST_VERIFY_SELF_DECL"].ToString().Trim();


                AssignWorkerDesc = string.Empty;
                CaseTypeDesc = string.Empty;
                TownShipDesc = string.Empty;
                CountyDesc = string.Empty;
                HousingSituvationDesc = string.Empty;
                PrimaryLanDesc = string.Empty;
                SecondaryLanDesc = string.Empty;
                FamilyTypeDesc = string.Empty;
                WaitingListDesc = string.Empty;
                ContactusDesc = string.Empty;
                AboutUsDesc = string.Empty;
                CasworkerHousDesc = string.Empty;
                ExpCaseworkerDesc = string.Empty;
                ModuleCode = string.Empty;
                //PJobDesc = string.Empty;
                //PHSDDesc = string.Empty;
                //PSkillsDesc = string.Empty;
                //PHousingDesc = string.Empty;
                //PTransportDesc = string.Empty;
                //PChldCareDesc = string.Empty;
                //PCCENRLDesc = string.Empty;
                //PELDCAREDesc = string.Empty;
                //PECNEEDDesc = string.Empty;
                //PECHINSDesc = string.Empty;
                //PAHINSDesc = string.Empty;
                //PRWENGDesc = string.Empty;
                //PCURRDSSDesc = string.Empty;
                //PRECVDSSDesc = string.Empty;

            }

        }

        public CaseMstEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "case2530Report")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                    NickName = row["Name"].ToString().Trim();
                    Age = row["Age"].ToString().Trim();
                    AddressDetails = row["Address"].ToString().Trim();
                    IntakeDate = row["MST_Intake_Date"].ToString().Trim();
                    Secret = row["MST_SECRET"].ToString().Trim();
                    AlertCodes = row["MST_ALERT_CODES"].ToString().Trim();
                    CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                    Secret = row["MST_SECRET"].ToString().Trim();
                    Juvenile = row["MST_JUVENILE"].ToString().Trim();
                    Senior = row["MST_SENIOR"].ToString().Trim();
                    IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                    Zip = row["MST_ZIP"].ToString().Trim();
                    County = row["MST_COUNTY"].ToString().Trim();
                    Language = row["MST_LANGUAGE"].ToString().Trim();
                    LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                    FamilyType = row["MST_FAMILY_TYPE"].ToString().Trim();
                    WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                    Housing = row["MST_HOUSING"].ToString().Trim();
                    Site = row["MST_SITE"].ToString().Trim();
                    InitialDate = row["MST_INITIAL_DATE"].ToString().Trim();
                    CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                    AddressYears = row["MST_ADDRESS_YEARS"].ToString().Trim();
                    BestContact = row["MST_BEST_CONTACT"].ToString().Trim();
                    AboutUs = row["MST_ABOUT_US"].ToString().Trim();
                    EligDate = row["MST_ELIG_DATE"].ToString().Trim();
                    Verifier = row["MST_VERIFIER"].ToString().Trim();
                    VerifyW2 = row["MST_VERIFY_W2"].ToString().Trim();
                    VerifyCheckStub = row["MST_VERIFY_CHECK_STUB"].ToString().Trim();
                    VerifyTaxReturn = row["MST_VERIFY_TAX_RETURN"].ToString().Trim();
                    VerifyLetter = row["MST_VERIFY_LETTER"].ToString().Trim();
                    VerifyOther = row["MST_VERIFY_OTHER"].ToString().Trim();
                    ProgIncome = row["MST_PROG_INCOME"].ToString().Trim();
                    FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                    NoInProg = row["MST_NO_INPROG"].ToString().Trim();
                    Poverty = row["MST_POVERTY"].ToString().Trim();
                    Hud = row["MST_HUD"].ToString().Trim();
                    Cmi = row["MST_CMI"].ToString().Trim();
                    Smi = row["MST_SMI"].ToString().Trim();
                    ReverifyDate = row["MST_REVERIFY_DATE"].ToString().Trim();
                    IncomeTypes = row["MST_INCOME_TYPES"].ToString().Trim();
                    ExpRent = row["MST_EXP_RENT"].ToString().Trim();
                    ExpWater = row["MST_EXP_WATER"].ToString().Trim();
                    ExpElectric = row["MST_EXP_ELECTRIC"].ToString().Trim();
                    ExpHeat = row["MST_EXP_HEAT"].ToString().Trim();
                    Debtcc = row["MST_DEBT_CC"].ToString().Trim();
                    DebtLoans = row["MST_DEBT_LOANS"].ToString().Trim();
                    DebtMed = row["MST_DEBT_MED"].ToString().Trim();
                    // = row["MST_EXP_EXP3"].ToString().Trim();
                    ExpTotal = row["MST_EXP_TOTAL"].ToString().Trim();
                    ExpLivexpense = row["MST_EXP_LIVEXPENSE"].ToString().Trim();
                    ExpCaseWorker = row["MST_EXP_CASEWORKER"].ToString().Trim();
                    MstNCashBen = row["MST_NCASHBEN"].ToString().Trim();
                    VERIFY_SELF_DECL = row["MST_VERIFY_SELF_DECL"].ToString().Trim();
                    CatElig = row["MST_CAT_ELIG"].ToString().Trim();

                    //PJob = row["MST_PRESS_JOB"].ToString().Trim();
                    //PHSD = row["MST_PRESS_HSD"].ToString().Trim();
                    //PSkills = row["MST_PRESS_SKILLS"].ToString().Trim();
                    //PHousing = row["MST_PRESS_HOUSING"].ToString().Trim();
                    //PTransport = row["MST_PRESS_TRANSPORT"].ToString().Trim();
                    //PChldCare = row["MST_PRESS_CHLDCARE"].ToString().Trim();
                    //PCCENRL = row["MST_PRESS_CCENRL"].ToString().Trim();
                    //PELDCARE = row["MST_PRESS_ELDRCARE"].ToString().Trim();
                    //PECNEED = row["MST_PRESS_ECNEED"].ToString().Trim();
                    //PECHINS = row["MST_PRESS_CHINS"].ToString().Trim();
                    //PAHINS = row["MST_PRESS_AHINS"].ToString().Trim();
                    //PRWENG = row["MST_PRESS_RW_ENG"].ToString().Trim();
                    //PCURRDSS = row["MST_PRESS_CURR_DSS"].ToString().Trim();
                    //PRECVDSS = row["MST_PRESS_RECV_DSS"].ToString().Trim();

                    DateAdd1 = row["MST_DATE_ADD_1"].ToString().Trim();
                    PointsOnly = 0;
                    PointsCategory = string.Empty;

                    //snpAgency = row["SNP_AGENCY"].ToString().Trim();
                    //snpDept = row["SNP_DEPT"].ToString().Trim();
                    //snpProgram = row["SNP_PROGRAM"].ToString().Trim();
                    //snpYear = row["SNP_YEAR"].ToString().Trim();
                    //snpApp = row["SNP_APP"].ToString().Trim();
                    //snpFamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                    //snpMemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                    //snpAltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                    //snpStatus = row["SNP_STATUS"].ToString().Trim();
                    //snpAge = row["SNP_AGE"].ToString().Trim();
                    //snpEthnic = row["SNP_ETHNIC"].ToString().Trim();
                    //snpEducation = row["SNP_EDUCATION"].ToString().Trim();
                    //snpHealthIns = row["SNP_HEALTH_INS"].ToString().Trim();
                    //snpVet = row["SNP_VET"].ToString().Trim();
                    //snpDisable = row["SNP_DISABLE"].ToString().Trim();
                    //snpFootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
                    //snpFarmer = row["SNP_FARMER"].ToString().Trim();
                    //snpSex = row["SNP_SEX"].ToString().Trim();
                    //snpWic = row["SNP_WIC"].ToString().Trim();
                    //snpResident = row["SNP_RESIDENT"].ToString().Trim();
                    //snpPregnant = row["SNP_PREGNANT"].ToString().Trim();
                    //snpMaritalStatus = row["SNP_MARITAL_STATUS"].ToString().Trim();
                    //snpSchoolDistrict = row["SNP_SCHOOL_DISTRICT"].ToString().Trim();
                    //snpLegalTowork = row["SNP_LEGAL_TO_WORK"].ToString().Trim();
                    //snpExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                    //snpEmployed = row["SNP_EMPLOYED"].ToString().Trim();
                    //snpLastWorkDate = row["SNP_LAST_WORK_DATE"].ToString().Trim();
                    //snpWorkLimit = row["SNP_WORK_LIMIT"].ToString().Trim();
                    //snpNumberOfcjobs = row["SNP_NUMBER_OF_C_JOBS"].ToString().Trim();
                    //snpNumberofLvjobs = row["SNP_NUMBER_OF_LV_JOBS"].ToString().Trim();
                    //snpFullTimeHours = row["SNP_FULL_TIME_HOURS"].ToString().Trim();
                    //snpPartTimeHours = row["SNP_PART_TIME_HOURS"].ToString().Trim();
                    //snpSeasonalEmploy = row["SNP_SEASONAL_EMPLOY"].ToString().Trim();
                    //snpIstShift = row["SNP_1ST_SHIFT"].ToString().Trim();
                    //snpIIndShift = row["SNP_2ND_SHIFT"].ToString().Trim();
                    //snpIIIrdShift = row["SNP_3RD_SHIFT"].ToString().Trim();
                    //snpRShift = row["SNP_R_SHIFT"].ToString().Trim();
                    //snpJobTitle = row["SNP_JOB_TITLE"].ToString().Trim();
                    //snpJobCategory = row["SNP_JOB_CATEGORY"].ToString().Trim();
                    //snpHourlyWage = row["SNP_HOURLY_WAGE"].ToString().Trim();
                    //snpPayFrequency = row["SNP_PAY_FREQUENCY"].ToString().Trim();
                    //snpHireDate = row["SNP_HIRE_DATE"].ToString().Trim();
                    //snpTranserv = row["SNP_TRANSERV"].ToString().Trim();
                    //snpRelitran = row["SNP_RELITRAN"].ToString().Trim();
                    //snpDrvlic = row["SNP_DRVLIC"].ToString().Trim();
                    //snpRace = row["SNP_RACE"].ToString().Trim();    




                }

                else if (strTable == "MSTALLSNP")
                {
                    PressTotal = string.Empty;
                    PressCat = string.Empty;
                    PressGrp = string.Empty;
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                    FamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                    ClientId = row["MST_CLIENT_ID"].ToString().Trim();
                    Ssn = row["MST_SSN"].ToString().Trim();
                    Bic = row["MST_BIC"].ToString().Trim();
                    NickName = row["MST_NICKNAME"].ToString().Trim();
                    EthnicOther = row["MST_ETHNIC_OTHER"].ToString().Trim();
                    State = row["MST_STATE"].ToString().Trim();
                    City = row["MST_CITY"].ToString().Trim();
                    Street = row["MST_STREET"].ToString().Trim();
                    Suffix = row["MST_SUFFIX"].ToString().Trim();
                    Hn = row["MST_HN"].ToString().Trim();
                    Direction = row["MST_DIRECTION"].ToString().Trim();
                    Apt = row["MST_APT"].ToString().Trim();
                    Flr = row["MST_FLR"].ToString().Trim();
                    Zip = row["MST_ZIP"].ToString().Trim();
                    Zipplus = row["MST_ZIPPLUS"].ToString().Trim();
                    Precinct = row["MST_PRECINCT"].ToString().Trim();
                    Area = row["MST_AREA"].ToString().Trim();
                    Phone = row["MST_PHONE"].ToString().Trim();
                    NextYear = row["MST_NEXTYEAR"].ToString().Trim();
                    Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                    Language = row["MST_LANGUAGE"].ToString().Trim();
                    LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                    IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                    IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                    InitialDate = row["MST_INITIAL_DATE"].ToString().Trim();
                    CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                    Housing = row["MST_HOUSING"].ToString().Trim();
                    FamilyType = row["MST_FAMILY_TYPE"].ToString().Trim();
                    Site = row["MST_SITE"].ToString().Trim();
                    Juvenile = row["MST_JUVENILE"].ToString().Trim();
                    Senior = row["MST_SENIOR"].ToString().Trim();
                    Secret = row["MST_SECRET"].ToString().Trim();
                    CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                    CompleteDate = row["MST_CASE_COMPLETE_DATE"].ToString().Trim();
                    AlertCodes = row["MST_ALERT_CODES"].ToString().Trim();
                    ParentStatus = row["MST_PARENT_STATUS"].ToString().Trim();
                    IntakeHrs = row["MST_INTAKE_HRS"].ToString().Trim();
                    IntakeMns = row["MST_INTAKE_MNS"].ToString().Trim();
                    IntakeScs = row["MST_INTAKE_SCS"].ToString().Trim();
                    FinHrs = row["MST_FIN_HRS"].ToString().Trim();
                    FinMns = row["MST_FIN_MNS"].ToString().Trim();
                    FinScs = row["MST_FIN_SCS"].ToString().Trim();
                    SimHrs = row["MST_SIM_HRS"].ToString().Trim();
                    SimMns = row["MST_SIM_MNS"].ToString().Trim();
                    SimScs = row["MST_SIM_SCS"].ToString().Trim();
                    MedHrs = row["MST_MED_HRS"].ToString().Trim();
                    MedMns = row["MST_MED_MNS"].ToString().Trim();
                    MedScs = row["MST_MED_SCS"].ToString().Trim();
                    Rank1 = row["MST_RANK1"].ToString().Trim();
                    Rank2 = row["MST_RANK2"].ToString().Trim();
                    Rank3 = row["MST_RANK3"].ToString().Trim();
                    Rank4 = row["MST_RANK4"].ToString().Trim();
                    Rank5 = row["MST_RANK5"].ToString().Trim();
                    Rank6 = row["MST_RANK6"].ToString().Trim();
                    Position1 = row["MST_POSITION1"].ToString().Trim();
                    Position2 = row["MST_POSITION2"].ToString().Trim();
                    Position3 = row["MST_POSITION3"].ToString().Trim();
                    TownShip = row["MST_TOWNSHIP"].ToString().Trim();
                    IntakeTime1 = row["MST_INTAKE_TIME1"].ToString().Trim();
                    SsnFlag = row["MST_SSN_FLAG"].ToString().Trim();
                    StateCase = row["MST_STATE_CASE"].ToString().Trim();
                    Verifier = row["MST_VERIFIER"].ToString().Trim();
                    EligDate = row["MST_ELIG_DATE"].ToString().Trim();
                    CatElig = row["MST_CAT_ELIG"].ToString().Trim();
                    MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                    VerifyW2 = row["MST_VERIFY_W2"].ToString().Trim();
                    VerifyCheckStub = row["MST_VERIFY_CHECK_STUB"].ToString().Trim();
                    VerifyTaxReturn = row["MST_VERIFY_TAX_RETURN"].ToString().Trim();
                    VerifyLetter = row["MST_VERIFY_LETTER"].ToString().Trim();
                    VerifyOther = row["MST_VERIFY_OTHER"].ToString().Trim();
                    ReverifyDate = row["MST_REVERIFY_DATE"].ToString().Trim();
                    IncomeTypes = row["MST_INCOME_TYPES"].ToString().Trim();
                    Poverty = row["MST_POVERTY"].ToString().Trim();
                    WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                    ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
                    TotalRank = row["MST_TOTAL_RANK"].ToString().Trim();
                    NoInhh = row["MST_NO_INHH"].ToString().Trim();
                    FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                    NoInProg = row["MST_NO_INPROG"].ToString().Trim();
                    ProgIncome = row["MST_PROG_INCOME"].ToString().Trim();
                    OutOfService = row["MST_OUT_OF_SERVICE"].ToString().Trim();
                    Hud = row["MST_HUD"].ToString().Trim();
                    Smi = row["MST_SMI"].ToString().Trim();
                    Cmi = row["MST_CMI"].ToString().Trim();
                    County = row["MST_COUNTY"].ToString().Trim();
                    AddressYears = row["MST_ADDRESS_YEARS"].ToString().Trim();
                    MessagePhone = row["MST_MESSAGE_PHONE"].ToString().Trim();
                    CellPhone = row["MST_CELL_PHONE"].ToString().Trim();
                    FaxNumber = row["MST_FAX_NUMBER"].ToString().Trim();
                    TtyNumber = row["MST_TTY_NUMBER"].ToString().Trim();
                    Email = row["MST_EMAIL"].ToString().Trim();
                    BestContact = row["MST_BEST_CONTACT"].ToString().Trim();
                    AboutUs = row["MST_ABOUT_US"].ToString().Trim();
                    ImportDate = row["MST_IMPORT_DATE"].ToString().Trim();
                    DateAdded = row["MST_DATE_ADDED"].ToString().Trim();
                    ExpRent = row["MST_EXP_RENT"].ToString().Trim();
                    ExpWater = row["MST_EXP_WATER"].ToString().Trim();
                    ExpElectric = row["MST_EXP_ELECTRIC"].ToString().Trim();
                    ExpHeat = row["MST_EXP_HEAT"].ToString().Trim();
                    Debtcc = row["MST_DEBT_CC"].ToString().Trim();
                    DebtLoans = row["MST_DEBT_LOANS"].ToString().Trim();
                    DebtMed = row["MST_DEBT_MED"].ToString().Trim();
                    DebtOther = row["MST_DEBT_OTH"].ToString().Trim();
                    ExpTotal = row["MST_EXP_TOTAL"].ToString().Trim();
                    ExpLivexpense = row["MST_EXP_LIVEXPENSE"].ToString().Trim();
                    ExpCaseWorker = row["MST_EXP_CASEWORKER"].ToString().Trim();
                    Dwelling = row["MST_DWELLING"].ToString().Trim();
                    HeatIncRent = row["MST_HEAT_INC_RENT"].ToString().Trim();
                    Source = row["MST_SOURCE"].ToString().Trim();
                    RollOver = row["MST_ROLLOVER"].ToString().Trim();
                    RiskValue = row["MST_RISK_VALUE"].ToString().Trim();
                    SubShouse = row["MST_SUBSHOUSE"].ToString().Trim();
                    SubStype = row["MST_SUBSTYPE"].ToString().Trim();
                    VerFund = row["MST_VER_FUND"].ToString().Trim();
                    OmbScreen = row["MST_OMB_SCREEN"].ToString().Trim();
                    CbCaseManager = row["MST_CB_CASE_MANAGER"].ToString().Trim();
                    CaseManager = row["MST_CASE_MANAGER"].ToString().Trim();
                    VerifyOthCmb = row["MST_VERIFY_OTH_CMB"].ToString().Trim();
                    SimPrint = row["MST_SIM_PRINT"].ToString().Trim();
                    SimPrintDate = row["MST_SIM_PRINT_DATE"].ToString().Trim();
                    CbFraud = row["MST_CB_FRAUD"].ToString().Trim();
                    FraudDate = row["MST_FRAUD_DATE"].ToString().Trim();
                    DateAdd1 = row["MST_DATE_ADD_1"].ToString().Trim();
                    AddOperator1 = row["MST_ADD_OPERATOR_1"].ToString().Trim();
                    DateLstc1 = row["MST_DATE_LSTC_1"].ToString().Trim();
                    LstcOperator1 = row["MST_LSTC_OPERATOR_1"].ToString().Trim();
                    TimesUpdated1 = row["MST_TIMES_UPDATED_1"].ToString().Trim();
                    DateAdd2 = row["MST_DATE_ADD_2"].ToString().Trim();
                    AddOperator2 = row["MST_ADD_OPERATOR_2"].ToString().Trim();
                    DateLstc2 = row["MST_DATE_LSTC_2"].ToString().Trim();
                    LstcOperator2 = row["MST_LSTC_OPERATOR_2"].ToString().Trim();
                    TimesUpdated2 = row["MST_TIMES_UPDATED_2"].ToString().Trim();
                    DateAdd3 = row["MST_DATE_ADD_3"].ToString().Trim();
                    AddOperator3 = row["MST_ADD_OPERATOR_3"].ToString().Trim();
                    DateLstc3 = row["MST_DATE_LSTC_3"].ToString().Trim();
                    LstcOperator3 = row["MST_LSTC_OPERATOR_3"].ToString().Trim();
                    TimesUpdated3 = row["MST_TIMES_UPDATED_3"].ToString().Trim();
                    DateAdd4 = row["MST_DATE_ADD_4"].ToString().Trim();
                    AddOperator4 = row["MST_ADD_OPERATOR_4"].ToString().Trim();
                    DateLstc4 = row["MST_DATE_LSTC_4"].ToString().Trim();
                    LstcOperator4 = row["MST_LSTC_OPERATOR_4"].ToString().Trim();
                    TimesUpdated4 = row["MST_TIMES_UPDATED_4"].ToString().Trim();

                    DebtMisc = row["MST_DEBT_MISC"].ToString().Trim();
                    AsetLiq = row["MST_ASET_LIQ"].ToString().Trim();
                    AsetMisc = row["MST_ASET_MISC"].ToString().Trim();
                    AsetOth = row["MST_ASET_OTH"].ToString().Trim();
                    AsetPhy = row["MST_ASET_PHY"].ToString().Trim();
                    ExpMisc = row["MST_EXP_MISC"].ToString().Trim();
                    DebtTotal = row["MST_DEBT_TOTAL"].ToString().Trim();
                    AsetTotal = row["MST_ASET_TOTAL"].ToString().Trim();
                    AsetRatio = row["MST_DEB_ASET_RATIO"].ToString().Trim();
                    DebIncmRation = row["MST_DEB_INCM_RATIO"].ToString().Trim();

                    //PJob = row["MST_PRESS_JOB"].ToString().Trim();
                    //PHSD = row["MST_PRESS_HSD"].ToString().Trim();
                    //PSkills = row["MST_PRESS_SKILLS"].ToString().Trim();
                    //PHousing = row["MST_PRESS_HOUSING"].ToString().Trim();
                    //PTransport = row["MST_PRESS_TRANSPORT"].ToString().Trim();
                    //PChldCare = row["MST_PRESS_CHLDCARE"].ToString().Trim();
                    //PCCENRL = row["MST_PRESS_CCENRL"].ToString().Trim();
                    //PELDCARE = row["MST_PRESS_ELDRCARE"].ToString().Trim();
                    //PECNEED = row["MST_PRESS_ECNEED"].ToString().Trim();
                    //PECHINS = row["MST_PRESS_CHINS"].ToString().Trim();
                    //PAHINS = row["MST_PRESS_AHINS"].ToString().Trim();
                    //PRWENG = row["MST_PRESS_RW_ENG"].ToString().Trim();
                    //PCURRDSS = row["MST_PRESS_CURR_DSS"].ToString().Trim();
                    //PRECVDSS = row["MST_PRESS_RECV_DSS"].ToString().Trim();

                    LPM0001 = row["MST_LPM_0001"].ToString().Trim();
                    LPM0002 = row["MST_LPM_0002"].ToString().Trim();
                    LPM0003 = row["MST_LPM_0003"].ToString().Trim();
                    LPM0004 = row["MST_LPM_0004"].ToString().Trim();
                    LPM0005 = row["MST_LPM_0005"].ToString().Trim();
                    LPM0006 = row["MST_LPM_0006"].ToString().Trim();
                    LPM0007 = row["MST_LPM_0007"].ToString().Trim();
                    LPM0008 = row["MST_LPM_0008"].ToString().Trim();
                    LPM0009 = row["MST_LPM_0009"].ToString().Trim();
                    LPM0010 = row["MST_LPM_0010"].ToString().Trim();
                    LPM0011 = row["MST_LPM_0011"].ToString().Trim();
                    LPM0012 = row["MST_LPM_0012"].ToString().Trim();
                    LPM0013 = row["MST_LPM_0013"].ToString().Trim();
                    LPM0014 = row["MST_LPM_0014"].ToString().Trim();
                    LPM0015 = row["MST_LPM_0015"].ToString().Trim();
                    LPM0016 = row["MST_LPM_0016"].ToString().Trim();
                    LPM0017 = row["MST_LPM_0017"].ToString().Trim();
                    MstNCashBen = row["MST_NCASHBEN"].ToString().Trim();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();

                    snpHealthIns = row["SNP_HEALTH_CODES"].ToString().Trim();
                    VERIFY_SELF_DECL = row["MST_VERIFY_SELF_DECL"].ToString().Trim();

                }
                else if (strTable == "TMSB2022")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                }
                else if (strTable == "TRIGGERS")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                    //FamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                    //ClientId = row["MST_CLIENT_ID"].ToString().Trim();
                    //Ssn = row["MST_SSN"].ToString().Trim();
                    //Bic = row["MST_BIC"].ToString().Trim();
                    //NickName = row["MST_NICKNAME"].ToString().Trim();
                    //EthnicOther = row["MST_ETHNIC_OTHER"].ToString().Trim();
                    //State = row["MST_STATE"].ToString().Trim();
                    //City = row["MST_CITY"].ToString().Trim();
                    //Street = row["MST_STREET"].ToString().Trim();
                    //Suffix = row["MST_SUFFIX"].ToString().Trim();
                    //Hn = row["MST_HN"].ToString().Trim();
                    //Direction = row["MST_DIRECTION"].ToString().Trim();
                    //Apt = row["MST_APT"].ToString().Trim();
                    //Flr = row["MST_FLR"].ToString().Trim();
                    //Zip = row["MST_ZIP"].ToString().Trim();
                    //Zipplus = row["MST_ZIPPLUS"].ToString().Trim();
                    //Precinct = row["MST_PRECINCT"].ToString().Trim();
                    //Area = row["MST_AREA"].ToString().Trim();
                    //Phone = row["MST_PHONE"].ToString().Trim();
                    //NextYear = row["MST_NEXTYEAR"].ToString().Trim();
                    //Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                    //Language = row["MST_LANGUAGE"].ToString().Trim();
                    //LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                    //IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                    IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                    //InitialDate = row["MST_INITIAL_DATE"].ToString().Trim();
                    //CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                    Housing = row["MST_HOUSING"].ToString().Trim();
                    //FamilyType = row["MST_FAMILY_TYPE"].ToString().Trim();
                    //Site = row["MST_SITE"].ToString().Trim();
                    //Juvenile = row["MST_JUVENILE"].ToString().Trim();
                    //Senior = row["MST_SENIOR"].ToString().Trim();
                    //Secret = row["MST_SECRET"].ToString().Trim();
                    //CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                    //AlertCodes = row["MST_ALERT_CODES"].ToString().Trim();
                    //ParentStatus = row["MST_PARENT_STATUS"].ToString().Trim();
                    //IntakeHrs = row["MST_INTAKE_HRS"].ToString().Trim();
                    //IntakeMns = row["MST_INTAKE_MNS"].ToString().Trim();
                    //IntakeScs = row["MST_INTAKE_SCS"].ToString().Trim();
                    //FinHrs = row["MST_FIN_HRS"].ToString().Trim();
                    //FinMns = row["MST_FIN_MNS"].ToString().Trim();
                    //FinScs = row["MST_FIN_SCS"].ToString().Trim();
                    //SimHrs = row["MST_SIM_HRS"].ToString().Trim();
                    //SimMns = row["MST_SIM_MNS"].ToString().Trim();
                    //SimScs = row["MST_SIM_SCS"].ToString().Trim();
                    //MedHrs = row["MST_MED_HRS"].ToString().Trim();
                    //MedMns = row["MST_MED_MNS"].ToString().Trim();
                    //MedScs = row["MST_MED_SCS"].ToString().Trim();
                    //Rank1 = row["MST_RANK1"].ToString().Trim();
                    //Rank2 = row["MST_RANK2"].ToString().Trim();
                    //Rank3 = row["MST_RANK3"].ToString().Trim();
                    //Rank4 = row["MST_RANK4"].ToString().Trim();
                    //Rank5 = row["MST_RANK5"].ToString().Trim();
                    //Rank6 = row["MST_RANK6"].ToString().Trim();
                    //Position1 = row["MST_POSITION1"].ToString().Trim();
                    //Position2 = row["MST_POSITION2"].ToString().Trim();
                    //Position3 = row["MST_POSITION3"].ToString().Trim();
                    //TownShip = row["MST_TOWNSHIP"].ToString().Trim();
                    //IntakeTime1 = row["MST_INTAKE_TIME1"].ToString().Trim();
                    //SsnFlag = row["MST_SSN_FLAG"].ToString().Trim();
                    //StateCase = row["MST_STATE_CASE"].ToString().Trim();
                    //Verifier = row["MST_VERIFIER"].ToString().Trim();
                    //EligDate = row["MST_ELIG_DATE"].ToString().Trim();
                    //CatElig = row["MST_CAT_ELIG"].ToString().Trim();
                    //MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                    //VerifyW2 = row["MST_VERIFY_W2"].ToString().Trim();
                    //VerifyCheckStub = row["MST_VERIFY_CHECK_STUB"].ToString().Trim();
                    //VerifyTaxReturn = row["MST_VERIFY_TAX_RETURN"].ToString().Trim();
                    //VerifyLetter = row["MST_VERIFY_LETTER"].ToString().Trim();
                    //VerifyOther = row["MST_VERIFY_OTHER"].ToString().Trim();
                    //ReverifyDate = row["MST_REVERIFY_DATE"].ToString().Trim();
                    //IncomeTypes = row["MST_INCOME_TYPES"].ToString().Trim();
                    //Poverty = row["MST_POVERTY"].ToString().Trim();
                    //WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                    //ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
                    //TotalRank = row["MST_TOTAL_RANK"].ToString().Trim();
                    //NoInhh = row["MST_NO_INHH"].ToString().Trim();
                    //FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                    //NoInProg = row["MST_NO_INPROG"].ToString().Trim();
                    //ProgIncome = row["MST_PROG_INCOME"].ToString().Trim();
                    //OutOfService = row["MST_OUT_OF_SERVICE"].ToString().Trim();
                    //Hud = row["MST_HUD"].ToString().Trim();
                    //Smi = row["MST_SMI"].ToString().Trim();
                    //Cmi = row["MST_CMI"].ToString().Trim();
                    //County = row["MST_COUNTY"].ToString().Trim();
                    //AddressYears = row["MST_ADDRESS_YEARS"].ToString().Trim();
                    //MessagePhone = row["MST_MESSAGE_PHONE"].ToString().Trim();
                    //CellPhone = row["MST_CELL_PHONE"].ToString().Trim();
                    //FaxNumber = row["MST_FAX_NUMBER"].ToString().Trim();
                    //TtyNumber = row["MST_TTY_NUMBER"].ToString().Trim();
                    //Email = row["MST_EMAIL"].ToString().Trim();
                    //BestContact = row["MST_BEST_CONTACT"].ToString().Trim();
                    //AboutUs = row["MST_ABOUT_US"].ToString().Trim();
                    //ImportDate = row["MST_IMPORT_DATE"].ToString().Trim();
                    //DateAdded = row["MST_DATE_ADDED"].ToString().Trim();
                    //ExpRent = row["MST_EXP_RENT"].ToString().Trim();
                    //ExpWater = row["MST_EXP_WATER"].ToString().Trim();
                    //ExpElectric = row["MST_EXP_ELECTRIC"].ToString().Trim();
                    //ExpHeat = row["MST_EXP_HEAT"].ToString().Trim();
                    //Debtcc = row["MST_DEBT_CC"].ToString().Trim();
                    //DebtLoans = row["MST_DEBT_LOANS"].ToString().Trim();
                    //DebtMed = row["MST_DEBT_MED"].ToString().Trim();
                    //DebtOther = row["MST_DEBT_OTH"].ToString().Trim();
                    //ExpTotal = row["MST_EXP_TOTAL"].ToString().Trim();
                    //ExpLivexpense = row["MST_EXP_LIVEXPENSE"].ToString().Trim();
                    //ExpCaseWorker = row["MST_EXP_CASEWORKER"].ToString().Trim();
                    //Dwelling = row["MST_DWELLING"].ToString().Trim();
                    HeatIncRent = row["MST_HEAT_INC_RENT"].ToString().Trim();
                    //Source = row["MST_SOURCE"].ToString().Trim();
                    //RollOver = row["MST_ROLLOVER"].ToString().Trim();
                    //RiskValue = row["MST_RISK_VALUE"].ToString().Trim();
                    //SubShouse = row["MST_SUBSHOUSE"].ToString().Trim();
                    //SubStype = row["MST_SUBSTYPE"].ToString().Trim();
                    //VerFund = row["MST_VER_FUND"].ToString().Trim();
                    //OmbScreen = row["MST_OMB_SCREEN"].ToString().Trim();
                    //CbCaseManager = row["MST_CB_CASE_MANAGER"].ToString().Trim();
                    //CaseManager = row["MST_CASE_MANAGER"].ToString().Trim();
                    //VerifyOthCmb = row["MST_VERIFY_OTH_CMB"].ToString().Trim();
                    //SimPrint = row["MST_SIM_PRINT"].ToString().Trim();
                    //SimPrintDate = row["MST_SIM_PRINT_DATE"].ToString().Trim();
                    //CbFraud = row["MST_CB_FRAUD"].ToString().Trim();
                    //FraudDate = row["MST_FRAUD_DATE"].ToString().Trim();
                    //DateAdd1 = row["MST_DATE_ADD_1"].ToString().Trim();
                    //AddOperator1 = row["MST_ADD_OPERATOR_1"].ToString().Trim();
                    //DateLstc1 = row["MST_DATE_LSTC_1"].ToString().Trim();
                    //LstcOperator1 = row["MST_LSTC_OPERATOR_1"].ToString().Trim();
                    //TimesUpdated1 = row["MST_TIMES_UPDATED_1"].ToString().Trim();
                    //DateAdd2 = row["MST_DATE_ADD_2"].ToString().Trim();
                    //AddOperator2 = row["MST_ADD_OPERATOR_2"].ToString().Trim();
                    //DateLstc2 = row["MST_DATE_LSTC_2"].ToString().Trim();
                    //LstcOperator2 = row["MST_LSTC_OPERATOR_2"].ToString().Trim();
                    //TimesUpdated2 = row["MST_TIMES_UPDATED_2"].ToString().Trim();
                    //DateAdd3 = row["MST_DATE_ADD_3"].ToString().Trim();
                    //AddOperator3 = row["MST_ADD_OPERATOR_3"].ToString().Trim();
                    //DateLstc3 = row["MST_DATE_LSTC_3"].ToString().Trim();
                    //LstcOperator3 = row["MST_LSTC_OPERATOR_3"].ToString().Trim();
                    //TimesUpdated3 = row["MST_TIMES_UPDATED_3"].ToString().Trim();
                    //DateAdd4 = row["MST_DATE_ADD_4"].ToString().Trim();
                    //AddOperator4 = row["MST_ADD_OPERATOR_4"].ToString().Trim();
                    //DateLstc4 = row["MST_DATE_LSTC_4"].ToString().Trim();
                    //LstcOperator4 = row["MST_LSTC_OPERATOR_4"].ToString().Trim();
                    //TimesUpdated4 = row["MST_TIMES_UPDATED_4"].ToString().Trim();

                    //DebtMisc = row["MST_DEBT_MISC"].ToString().Trim();
                    //AsetLiq = row["MST_ASET_LIQ"].ToString().Trim();
                    //AsetMisc = row["MST_ASET_MISC"].ToString().Trim();
                    //AsetOth = row["MST_ASET_OTH"].ToString().Trim();
                    //AsetPhy = row["MST_ASET_PHY"].ToString().Trim();
                    //ExpMisc = row["MST_EXP_MISC"].ToString().Trim();
                    //DebtTotal = row["MST_DEBT_TOTAL"].ToString().Trim();
                    //AsetTotal = row["MST_ASET_TOTAL"].ToString().Trim();
                    //AsetRatio = row["MST_DEB_ASET_RATIO"].ToString().Trim();
                    //DebIncmRation = row["MST_DEB_INCM_RATIO"].ToString().Trim();

                    ////PJob = row["MST_PRESS_JOB"].ToString().Trim();
                    ////PHSD = row["MST_PRESS_HSD"].ToString().Trim();
                    ////PSkills = row["MST_PRESS_SKILLS"].ToString().Trim();
                    ////PHousing = row["MST_PRESS_HOUSING"].ToString().Trim();
                    ////PTransport = row["MST_PRESS_TRANSPORT"].ToString().Trim();
                    ////PChldCare = row["MST_PRESS_CHLDCARE"].ToString().Trim();
                    ////PCCENRL = row["MST_PRESS_CCENRL"].ToString().Trim();
                    ////PELDCARE = row["MST_PRESS_ELDRCARE"].ToString().Trim();
                    ////PECNEED = row["MST_PRESS_ECNEED"].ToString().Trim();
                    ////PECHINS = row["MST_PRESS_CHINS"].ToString().Trim();
                    ////PAHINS = row["MST_PRESS_AHINS"].ToString().Trim();
                    ////PRWENG = row["MST_PRESS_RW_ENG"].ToString().Trim();
                    ////PCURRDSS = row["MST_PRESS_CURR_DSS"].ToString().Trim();
                    ////PRECVDSS = row["MST_PRESS_RECV_DSS"].ToString().Trim();

                    //LPM0001 = row["MST_LPM_0001"].ToString().Trim();
                    //LPM0002 = row["MST_LPM_0002"].ToString().Trim();
                    //LPM0003 = row["MST_LPM_0003"].ToString().Trim();
                    //LPM0004 = row["MST_LPM_0004"].ToString().Trim();
                    //LPM0005 = row["MST_LPM_0005"].ToString().Trim();
                    //LPM0006 = row["MST_LPM_0006"].ToString().Trim();
                    //LPM0007 = row["MST_LPM_0007"].ToString().Trim();
                    //LPM0008 = row["MST_LPM_0008"].ToString().Trim();
                    //LPM0009 = row["MST_LPM_0009"].ToString().Trim();
                    //LPM0010 = row["MST_LPM_0010"].ToString().Trim();
                    //LPM0011 = row["MST_LPM_0011"].ToString().Trim();


                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    snpdobNa = row["SNP_DOB_NA"].ToString().Trim();
                    Age = row["SNP_AGE"].ToString().Trim();

                }
                else if (strTable == "SSBG")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                    //FamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                    //ClientId = row["MST_CLIENT_ID"].ToString().Trim();
                    //Ssn = row["MST_SSN"].ToString().Trim();
                    //Bic = row["MST_BIC"].ToString().Trim();
                    //NickName = row["MST_NICKNAME"].ToString().Trim();
                    //EthnicOther = row["MST_ETHNIC_OTHER"].ToString().Trim();
                    //State = row["MST_STATE"].ToString().Trim();
                    //City = row["MST_CITY"].ToString().Trim();
                    //Street = row["MST_STREET"].ToString().Trim();
                    //Suffix = row["MST_SUFFIX"].ToString().Trim();
                    //Hn = row["MST_HN"].ToString().Trim();
                    //Direction = row["MST_DIRECTION"].ToString().Trim();
                    //Apt = row["MST_APT"].ToString().Trim();
                    //Flr = row["MST_FLR"].ToString().Trim();
                    //Zip = row["MST_ZIP"].ToString().Trim();
                    //Zipplus = row["MST_ZIPPLUS"].ToString().Trim();
                    //Precinct = row["MST_PRECINCT"].ToString().Trim();
                    //Area = row["MST_AREA"].ToString().Trim();
                    //Phone = row["MST_PHONE"].ToString().Trim();
                    //NextYear = row["MST_NEXTYEAR"].ToString().Trim();
                    //Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                    //Language = row["MST_LANGUAGE"].ToString().Trim();
                    //LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                    //IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                    IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                    //InitialDate = row["MST_INITIAL_DATE"].ToString().Trim();
                    //CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                    Housing = row["MST_HOUSING"].ToString().Trim();
                    //FamilyType = row["MST_FAMILY_TYPE"].ToString().Trim();
                    //Site = row["MST_SITE"].ToString().Trim();
                    //Juvenile = row["MST_JUVENILE"].ToString().Trim();
                    //Senior = row["MST_SENIOR"].ToString().Trim();
                    //Secret = row["MST_SECRET"].ToString().Trim();
                    //CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                    //AlertCodes = row["MST_ALERT_CODES"].ToString().Trim();
                    //ParentStatus = row["MST_PARENT_STATUS"].ToString().Trim();
                    //IntakeHrs = row["MST_INTAKE_HRS"].ToString().Trim();
                    //IntakeMns = row["MST_INTAKE_MNS"].ToString().Trim();
                    //IntakeScs = row["MST_INTAKE_SCS"].ToString().Trim();
                    //FinHrs = row["MST_FIN_HRS"].ToString().Trim();
                    //FinMns = row["MST_FIN_MNS"].ToString().Trim();
                    //FinScs = row["MST_FIN_SCS"].ToString().Trim();
                    //SimHrs = row["MST_SIM_HRS"].ToString().Trim();
                    //SimMns = row["MST_SIM_MNS"].ToString().Trim();
                    //SimScs = row["MST_SIM_SCS"].ToString().Trim();
                    //MedHrs = row["MST_MED_HRS"].ToString().Trim();
                    //MedMns = row["MST_MED_MNS"].ToString().Trim();
                    //MedScs = row["MST_MED_SCS"].ToString().Trim();
                    //Rank1 = row["MST_RANK1"].ToString().Trim();
                    //Rank2 = row["MST_RANK2"].ToString().Trim();
                    //Rank3 = row["MST_RANK3"].ToString().Trim();
                    //Rank4 = row["MST_RANK4"].ToString().Trim();
                    //Rank5 = row["MST_RANK5"].ToString().Trim();
                    //Rank6 = row["MST_RANK6"].ToString().Trim();
                    //Position1 = row["MST_POSITION1"].ToString().Trim();
                    //Position2 = row["MST_POSITION2"].ToString().Trim();
                    //Position3 = row["MST_POSITION3"].ToString().Trim();
                    //TownShip = row["MST_TOWNSHIP"].ToString().Trim();
                    //IntakeTime1 = row["MST_INTAKE_TIME1"].ToString().Trim();
                    //SsnFlag = row["MST_SSN_FLAG"].ToString().Trim();
                    //StateCase = row["MST_STATE_CASE"].ToString().Trim();
                    //Verifier = row["MST_VERIFIER"].ToString().Trim();
                    //EligDate = row["MST_ELIG_DATE"].ToString().Trim();
                    //CatElig = row["MST_CAT_ELIG"].ToString().Trim();
                    //MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                    //VerifyW2 = row["MST_VERIFY_W2"].ToString().Trim();
                    //VerifyCheckStub = row["MST_VERIFY_CHECK_STUB"].ToString().Trim();
                    //VerifyTaxReturn = row["MST_VERIFY_TAX_RETURN"].ToString().Trim();
                    //VerifyLetter = row["MST_VERIFY_LETTER"].ToString().Trim();
                    //VerifyOther = row["MST_VERIFY_OTHER"].ToString().Trim();
                    //ReverifyDate = row["MST_REVERIFY_DATE"].ToString().Trim();
                    //IncomeTypes = row["MST_INCOME_TYPES"].ToString().Trim();
                    Poverty = row["MST_POVERTY"].ToString().Trim();
                    //WaitList = row["MST_WAIT_LIST"].ToString().Trim();
                    //ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
                    //TotalRank = row["MST_TOTAL_RANK"].ToString().Trim();
                    //NoInhh = row["MST_NO_INHH"].ToString().Trim();
                    //FamIncome = row["MST_FAM_INCOME"].ToString().Trim();
                    //NoInProg = row["MST_NO_INPROG"].ToString().Trim();
                    //ProgIncome = row["MST_PROG_INCOME"].ToString().Trim();
                    //OutOfService = row["MST_OUT_OF_SERVICE"].ToString().Trim();
                    //Hud = row["MST_HUD"].ToString().Trim();
                    //Smi = row["MST_SMI"].ToString().Trim();
                    //Cmi = row["MST_CMI"].ToString().Trim();
                    //County = row["MST_COUNTY"].ToString().Trim();
                    //AddressYears = row["MST_ADDRESS_YEARS"].ToString().Trim();
                    //MessagePhone = row["MST_MESSAGE_PHONE"].ToString().Trim();
                    //CellPhone = row["MST_CELL_PHONE"].ToString().Trim();
                    //FaxNumber = row["MST_FAX_NUMBER"].ToString().Trim();
                    //TtyNumber = row["MST_TTY_NUMBER"].ToString().Trim();
                    //Email = row["MST_EMAIL"].ToString().Trim();
                    //BestContact = row["MST_BEST_CONTACT"].ToString().Trim();
                    //AboutUs = row["MST_ABOUT_US"].ToString().Trim();
                    //ImportDate = row["MST_IMPORT_DATE"].ToString().Trim();
                    //DateAdded = row["MST_DATE_ADDED"].ToString().Trim();
                    //ExpRent = row["MST_EXP_RENT"].ToString().Trim();
                    //ExpWater = row["MST_EXP_WATER"].ToString().Trim();
                    //ExpElectric = row["MST_EXP_ELECTRIC"].ToString().Trim();
                    //ExpHeat = row["MST_EXP_HEAT"].ToString().Trim();
                    //Debtcc = row["MST_DEBT_CC"].ToString().Trim();
                    //DebtLoans = row["MST_DEBT_LOANS"].ToString().Trim();
                    //DebtMed = row["MST_DEBT_MED"].ToString().Trim();
                    //DebtOther = row["MST_DEBT_OTH"].ToString().Trim();
                    //ExpTotal = row["MST_EXP_TOTAL"].ToString().Trim();
                    //ExpLivexpense = row["MST_EXP_LIVEXPENSE"].ToString().Trim();
                    //ExpCaseWorker = row["MST_EXP_CASEWORKER"].ToString().Trim();
                    //Dwelling = row["MST_DWELLING"].ToString().Trim();
                    HeatIncRent = row["MST_HEAT_INC_RENT"].ToString().Trim();
                    //Source = row["MST_SOURCE"].ToString().Trim();
                    //RollOver = row["MST_ROLLOVER"].ToString().Trim();
                    //RiskValue = row["MST_RISK_VALUE"].ToString().Trim();
                    //SubShouse = row["MST_SUBSHOUSE"].ToString().Trim();
                    //SubStype = row["MST_SUBSTYPE"].ToString().Trim();
                    //VerFund = row["MST_VER_FUND"].ToString().Trim();
                    //OmbScreen = row["MST_OMB_SCREEN"].ToString().Trim();
                    //CbCaseManager = row["MST_CB_CASE_MANAGER"].ToString().Trim();
                    //CaseManager = row["MST_CASE_MANAGER"].ToString().Trim();
                    //VerifyOthCmb = row["MST_VERIFY_OTH_CMB"].ToString().Trim();
                    //SimPrint = row["MST_SIM_PRINT"].ToString().Trim();
                    //SimPrintDate = row["MST_SIM_PRINT_DATE"].ToString().Trim();
                    //CbFraud = row["MST_CB_FRAUD"].ToString().Trim();
                    //FraudDate = row["MST_FRAUD_DATE"].ToString().Trim();
                    //DateAdd1 = row["MST_DATE_ADD_1"].ToString().Trim();
                    //AddOperator1 = row["MST_ADD_OPERATOR_1"].ToString().Trim();
                    //DateLstc1 = row["MST_DATE_LSTC_1"].ToString().Trim();
                    //LstcOperator1 = row["MST_LSTC_OPERATOR_1"].ToString().Trim();
                    //TimesUpdated1 = row["MST_TIMES_UPDATED_1"].ToString().Trim();
                    //DateAdd2 = row["MST_DATE_ADD_2"].ToString().Trim();
                    //AddOperator2 = row["MST_ADD_OPERATOR_2"].ToString().Trim();
                    //DateLstc2 = row["MST_DATE_LSTC_2"].ToString().Trim();
                    //LstcOperator2 = row["MST_LSTC_OPERATOR_2"].ToString().Trim();
                    //TimesUpdated2 = row["MST_TIMES_UPDATED_2"].ToString().Trim();
                    //DateAdd3 = row["MST_DATE_ADD_3"].ToString().Trim();
                    //AddOperator3 = row["MST_ADD_OPERATOR_3"].ToString().Trim();
                    //DateLstc3 = row["MST_DATE_LSTC_3"].ToString().Trim();
                    //LstcOperator3 = row["MST_LSTC_OPERATOR_3"].ToString().Trim();
                    //TimesUpdated3 = row["MST_TIMES_UPDATED_3"].ToString().Trim();
                    //DateAdd4 = row["MST_DATE_ADD_4"].ToString().Trim();
                    //AddOperator4 = row["MST_ADD_OPERATOR_4"].ToString().Trim();
                    //DateLstc4 = row["MST_DATE_LSTC_4"].ToString().Trim();
                    //LstcOperator4 = row["MST_LSTC_OPERATOR_4"].ToString().Trim();
                    //TimesUpdated4 = row["MST_TIMES_UPDATED_4"].ToString().Trim();

                    //DebtMisc = row["MST_DEBT_MISC"].ToString().Trim();
                    //AsetLiq = row["MST_ASET_LIQ"].ToString().Trim();
                    //AsetMisc = row["MST_ASET_MISC"].ToString().Trim();
                    //AsetOth = row["MST_ASET_OTH"].ToString().Trim();
                    //AsetPhy = row["MST_ASET_PHY"].ToString().Trim();
                    //ExpMisc = row["MST_EXP_MISC"].ToString().Trim();
                    //DebtTotal = row["MST_DEBT_TOTAL"].ToString().Trim();
                    //AsetTotal = row["MST_ASET_TOTAL"].ToString().Trim();
                    //AsetRatio = row["MST_DEB_ASET_RATIO"].ToString().Trim();
                    //DebIncmRation = row["MST_DEB_INCM_RATIO"].ToString().Trim();

                    ////PJob = row["MST_PRESS_JOB"].ToString().Trim();
                    ////PHSD = row["MST_PRESS_HSD"].ToString().Trim();
                    ////PSkills = row["MST_PRESS_SKILLS"].ToString().Trim();
                    ////PHousing = row["MST_PRESS_HOUSING"].ToString().Trim();
                    ////PTransport = row["MST_PRESS_TRANSPORT"].ToString().Trim();
                    ////PChldCare = row["MST_PRESS_CHLDCARE"].ToString().Trim();
                    ////PCCENRL = row["MST_PRESS_CCENRL"].ToString().Trim();
                    ////PELDCARE = row["MST_PRESS_ELDRCARE"].ToString().Trim();
                    ////PECNEED = row["MST_PRESS_ECNEED"].ToString().Trim();
                    ////PECHINS = row["MST_PRESS_CHINS"].ToString().Trim();
                    ////PAHINS = row["MST_PRESS_AHINS"].ToString().Trim();
                    ////PRWENG = row["MST_PRESS_RW_ENG"].ToString().Trim();
                    ////PCURRDSS = row["MST_PRESS_CURR_DSS"].ToString().Trim();
                    ////PRECVDSS = row["MST_PRESS_RECV_DSS"].ToString().Trim();

                    //LPM0001 = row["MST_LPM_0001"].ToString().Trim();
                    //LPM0002 = row["MST_LPM_0002"].ToString().Trim();
                    //LPM0003 = row["MST_LPM_0003"].ToString().Trim();
                    //LPM0004 = row["MST_LPM_0004"].ToString().Trim();
                    //LPM0005 = row["MST_LPM_0005"].ToString().Trim();
                    //LPM0006 = row["MST_LPM_0006"].ToString().Trim();
                    //LPM0007 = row["MST_LPM_0007"].ToString().Trim();
                    //LPM0008 = row["MST_LPM_0008"].ToString().Trim();
                    //LPM0009 = row["MST_LPM_0009"].ToString().Trim();
                    //LPM0010 = row["MST_LPM_0010"].ToString().Trim();
                    //LPM0011 = row["MST_LPM_0011"].ToString().Trim();


                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    snpdobNa = row["SNP_DOB_NA"].ToString().Trim();
                    Age = row["SNP_AGE"].ToString().Trim();
                    HeatIncRentDesc = string.Empty;

                }
                else if (strTable == "SSBGADDCUST")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();

                    IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();

                    Housing = row["MST_HOUSING"].ToString().Trim();

                    Poverty = row["MST_POVERTY"].ToString().Trim();

                    HeatIncRent = row["MST_HEAT_INC_RENT"].ToString().Trim();


                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    snpdobNa = row["SNP_DOB_NA"].ToString().Trim();
                    Age = row["SNP_AGE"].ToString().Trim();
                    HeatIncRentDesc = string.Empty;

                    CUST_Code = row["ACT_CODE"].ToString().Trim();

                }
                else if (strTable == "CASE0027")
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["SNP_APP"].ToString().Trim();
                    State = row["MST_STATE"].ToString().Trim();
                    City = row["MST_CITY"].ToString().Trim();
                    Street = row["MST_STREET"].ToString().Trim();
                    Suffix = row["MST_SUFFIX"].ToString().Trim();
                    Hn = row["MST_HN"].ToString().Trim();
                    Direction = row["MST_DIRECTION"].ToString().Trim();
                    Apt = row["MST_APT"].ToString().Trim();
                    Flr = row["MST_FLR"].ToString().Trim();
                    Zip = row["MST_ZIP"].ToString().Trim();
                    Zipplus = row["MST_ZIPPLUS"].ToString().Trim();
                    // IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                    IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                    CaseReviewDate = row["MST_CASE_REVIEW_DATE"].ToString().Trim();
                    Area = row["MST_AREA"].ToString().Trim();
                    Phone = row["MST_PHONE"].ToString().Trim();
                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();

                    ReverifyDate = row["FOLLOWUPDATE"].ToString().Trim();

                    Mode = row["TBTYPE"].ToString().Trim();

                }
                else
                {
                    ApplAgency = row["MST_AGENCY"].ToString().Trim();
                    ApplDept = row["MST_DEPT"].ToString().Trim();
                    ApplProgram = row["MST_PROGRAM"].ToString().Trim();
                    ApplYr = row["MST_YEAR"].ToString().Trim();
                    ApplNo = row["MST_APP_NO"].ToString().Trim();
                    FamilySeq = row["MST_FAMILY_SEQ"].ToString().Trim();
                    //  string strFamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                    // FamilyId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                    FamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                    ClientId = row["MST_CLIENT_ID"].ToString().Trim();
                    Ssn = row["MST_SSN"].ToString().Trim();
                }


            }

        }

        #endregion

        #region Properties

        public string ApplAgency { get; set; }
        public string ApplDept { get; set; }
        public string ApplProgram { get; set; }
        public string ApplYr { get; set; }
        public string ApplNo { get; set; }
        public string FamilySeq { get; set; }
        public string FamilyId { get; set; }
        public string ClientId { get; set; }
        public string Ssn { get; set; }
        public string Bic { get; set; }
        public string NickName { get; set; }
        public string EthnicOther { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Hn { get; set; }
        public string Direction { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string Zip { get; set; }
        public string Zipplus { get; set; }
        public string Precinct { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string NextYear { get; set; }
        public string Classification { get; set; }
        public string Language { get; set; }
        public string LanguageOt { get; set; }
        public string IntakeWorker { get; set; }
        public string IntakeDate { get; set; }
        public string InitialDate { get; set; }
        public string CaseType { get; set; }
        public string Housing { get; set; }
        public string FamilyType { get; set; }
        public string Site { get; set; }
        public string Juvenile { get; set; }
        public string Senior { get; set; }
        public string Secret { get; set; }
        public string CaseReviewDate { get; set; }
        public string AlertCodes { get; set; }
        public string ParentStatus { get; set; }
        public string IntakeHrs { get; set; }
        public string IntakeMns { get; set; }
        public string IntakeScs { get; set; }
        public string FinHrs { get; set; }
        public string FinMns { get; set; }
        public string FinScs { get; set; }
        public string SimHrs { get; set; }
        public string SimMns { get; set; }
        public string SimScs { get; set; }
        public string MedHrs { get; set; }
        public string MedMns { get; set; }
        public string MedScs { get; set; }
        public string Rank1 { get; set; }
        public string Rank2 { get; set; }
        public string Rank3 { get; set; }
        public string Rank4 { get; set; }
        public string Rank5 { get; set; }
        public string Rank6 { get; set; }
        public string Position1 { get; set; }
        public string Position2 { get; set; }
        public string Position3 { get; set; }
        public string TownShip { get; set; }
        public string IntakeTime1 { get; set; }
        public string SsnFlag { get; set; }
        public string StateCase { get; set; }
        public string Verifier { get; set; }
        public string EligDate { get; set; }
        public string CatElig { get; set; }
        public string MealElig { get; set; }
        public string VerifyW2 { get; set; }
        public string VerifyCheckStub { get; set; }
        public string VerifyTaxReturn { get; set; }
        public string VerifyLetter { get; set; }
        public string VerifyOther { get; set; }
        public string ReverifyDate { get; set; }
        public string IncomeTypes { get; set; }
        public string Poverty { get; set; }
        public string WaitList { get; set; }
        public string ActiveStatus { get; set; }
        public string TotalRank { get; set; }
        public string NoInhh { get; set; }
        public string FamIncome { get; set; }
        public string NoInProg { get; set; }
        public string ProgIncome { get; set; }
        public string OutOfService { get; set; }
        public string Hud { get; set; }
        public string Smi { get; set; }
        public string Cmi { get; set; }
        public string County { get; set; }
        public string AddressYears { get; set; }
        public string MessagePhone { get; set; }
        public string CellPhone { get; set; }
        public string FaxNumber { get; set; }
        public string TtyNumber { get; set; }
        public string Email { get; set; }
        public string BestContact { get; set; }
        public string AboutUs { get; set; }
        public string ImportDate { get; set; }
        public string DateAdded { get; set; }
        public string ExpRent { get; set; }
        public string ExpWater { get; set; }
        public string ExpElectric { get; set; }
        public string ExpHeat { get; set; }
        public string ExpMisc { get; set; }
        public string Debtcc { get; set; }
        public string DebtLoans { get; set; }
        public string DebtMed { get; set; }
        public string DebtOther { get; set; }
        public string DebtMisc { get; set; }
        public string AsetPhy { get; set; }
        public string AsetLiq { get; set; }
        public string AsetOth { get; set; }
        public string AsetMisc { get; set; }
        public string ExpTotal { get; set; }
        public string ExpLivexpense { get; set; }
        public string ExpCaseWorker { get; set; }
        public string Dwelling { get; set; }
        public string HeatIncRent { get; set; }
        public string Source { get; set; }
        public string RollOver { get; set; }
        public string RiskValue { get; set; }
        public string SubShouse { get; set; }
        public string SubStype { get; set; }
        public string VerFund { get; set; }
        public string OmbScreen { get; set; }
        public string CbCaseManager { get; set; }
        public string CaseManager { get; set; }
        public string VerifyOthCmb { get; set; }
        public string SimPrint { get; set; }
        public string SimPrintDate { get; set; }
        public string CbFraud { get; set; }
        public string FraudDate { get; set; }
        public string DateAdd1 { get; set; }
        public string AddOperator1 { get; set; }
        public string DateLstc1 { get; set; }
        public string LstcOperator1 { get; set; }
        public string TimesUpdated1 { get; set; }
        public string DateAdd2 { get; set; }
        public string AddOperator2 { get; set; }
        public string DateLstc2 { get; set; }
        public string LstcOperator2 { get; set; }
        public string TimesUpdated2 { get; set; }
        public string DateAdd3 { get; set; }
        public string AddOperator3 { get; set; }
        public string DateLstc3 { get; set; }
        public string LstcOperator3 { get; set; }
        public string TimesUpdated3 { get; set; }
        public string DateAdd4 { get; set; }
        public string AddOperator4 { get; set; }
        public string DateLstc4 { get; set; }
        public string LstcOperator4 { get; set; }
        public string DateLstc5 { get; set; }
        public string LstcOperator5 { get; set; }
        public string TimesUpdated4 { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string ModuleCode { get; set; }

        public string PressTotal { get; set; }
        public string PressCat { get; set; }
        public string PressGrp { get; set; }

        public string AssignWorkerDesc { get; set; }
        public string CaseTypeDesc { get; set; }
        public string TownShipDesc { get; set; }
        public string CountyDesc { get; set; }
        public string HousingSituvationDesc { get; set; }
        public string PrimaryLanDesc { get; set; }
        public string SecondaryLanDesc { get; set; }
        public string FamilyTypeDesc { get; set; }
        public string WaitingListDesc { get; set; }
        public string ContactusDesc { get; set; }
        public string AboutUsDesc { get; set; }
        public string CasworkerHousDesc { get; set; }
        public string ExpCaseworkerDesc { get; set; }
        public string DwellingDesc { get; set; }
        public string HeatIncRentDesc { get; set; }
        public string SourceDesc { get; set; }
        public string SubsTypeDesc { get; set; }
        public string OutofserviceDesc { get; set; }

        public string DebtTotal { get; set; }
        public string AsetTotal { get; set; }
        public string AsetRatio { get; set; }
        public string DebIncmRation { get; set; }

        public string Age { get; set; }

        public string AddressDetails { get; set; }

        public string snpAgency { get; set; }
        public string snpDept { get; set; }
        public string snpProgram { get; set; }
        public string snpYear { get; set; }
        public string snpApp { get; set; }
        public string snpFamilySeq { get; set; }
        public string snpMemberCode { get; set; }
        public string snpAltBdate { get; set; }
        public string snpStatus { get; set; }
        public string snpAge { get; set; }
        public string snpEthnic { get; set; }
        public string snpEducation { get; set; }
        public string snpHealthIns { get; set; }
        public string snpVet { get; set; }
        public string snpDisable { get; set; }
        public string snpFootStamps { get; set; }
        public string snpFarmer { get; set; }
        public string snpSex { get; set; }
        public string snpWic { get; set; }
        public string snpResident { get; set; }
        public string snpPregnant { get; set; }
        public string snpMaritalStatus { get; set; }
        public string snpSchoolDistrict { get; set; }
        public string snpLegalTowork { get; set; }
        public string snpExpireWorkDate { get; set; }
        public string snpEmployed { get; set; }
        public string snpLastWorkDate { get; set; }
        public string snpWorkLimit { get; set; }
        public string snpNumberOfcjobs { get; set; }
        public string snpNumberofLvjobs { get; set; }
        public string snpFullTimeHours { get; set; }
        public string snpPartTimeHours { get; set; }
        public string snpSeasonalEmploy { get; set; }
        public string snpIstShift { get; set; }
        public string snpIIndShift { get; set; }
        public string snpIIIrdShift { get; set; }
        public string snpRShift { get; set; }
        public string snpJobTitle { get; set; }
        public string snpJobCategory { get; set; }
        public string snpHourlyWage { get; set; }
        public string snpPayFrequency { get; set; }
        public string snpHireDate { get; set; }
        public string snpTranserv { get; set; }
        public string snpRelitran { get; set; }
        public string snpDrvlic { get; set; }
        public string snpRace { get; set; }
        public string snpdobNa { get; set; }

        public string Appxml { get; set; }
        public string StaffMember { get; set; }
        public string Position { get; set; }
        public string PointsCategory { get; set; }
        public int PointsOnly { get; set; }
        public string MstNCashBen { get; set; }
        public string CUST_Code { get; set; }

        //public string PJob { get; set; }
        //public string PHSD { get; set; }
        //public string PSkills { get; set; }
        //public string PHousing { get; set; }
        //public string PTransport { get; set; }
        //public string PChldCare { get; set; }
        //public string PCCENRL { get; set; }
        //public string PELDCARE { get; set; }
        //public string PECNEED { get; set; }
        //public string PECHINS { get; set; }
        //public string PAHINS { get; set; }
        //public string PRWENG { get; set; }
        //public string PCURRDSS { get; set; }
        //public string PRECVDSS { get; set; }


        //public string PJobDesc { get; set; }
        //public string PHSDDesc { get; set; }
        //public string PSkillsDesc { get; set; }
        //public string PHousingDesc { get; set; }
        //public string PTransportDesc { get; set; }
        //public string PChldCareDesc { get; set; }
        //public string PCCENRLDesc { get; set; }
        //public string PELDCAREDesc { get; set; }
        //public string PECNEEDDesc { get; set; }
        //public string PECHINSDesc { get; set; }
        //public string PAHINSDesc { get; set; }
        //public string PRWENGDesc { get; set; }
        //public string PCURRDSSDesc { get; set; }
        //public string PRECVDSSDesc { get; set; }

        public string LPM0001 { get; set; }
        public string LPM0002 { get; set; }
        public string LPM0003 { get; set; }
        public string LPM0004 { get; set; }
        public string LPM0005 { get; set; }
        public string LPM0006 { get; set; }
        public string LPM0007 { get; set; }
        public string LPM0008 { get; set; }
        public string LPM0009 { get; set; }
        public string LPM0010 { get; set; }
        public string LPM0011 { get; set; }
        public string LPM0012 { get; set; }
        public string LPM0013 { get; set; }
        public string LPM0014 { get; set; }
        public string LPM0015 { get; set; }
        public string LPM0016 { get; set; }
        public string LPM0017 { get; set; }

        public string ApplictionDate { get; set; }
        public string ApplictionType { get; set; }
        public string ApplictionTypeDesc { get; set; }

        public string MessagePhone_NA { get; set; }
        public string CellPhone_NA { get; set; }
        public string HomePhone_NA { get; set; }
        public string Email_NA { get; set; }

        public string CompleteDate { get; set; }
        public string VERIFY_SELF_DECL { get; set; }


        #endregion
    }

    public class CaseSnpEntity
    {
        #region Constructors

        public CaseSnpEntity()
        {

            RecentMemberSwitch = string.Empty;
            FormatedName = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            FamilySeq = string.Empty;
            MemberCode = string.Empty;
            ClientId = string.Empty;
            Ssno = string.Empty;
            SsBic = string.Empty;
            NameixLast = string.Empty;
            NameixFi = string.Empty;
            NameixMi = string.Empty;
            AltBdate = string.Empty;
            AltLName = string.Empty;
            AltFi = string.Empty;
            Alias = string.Empty;
            Status = string.Empty;
            Sex = string.Empty;
            Age = string.Empty;
            Ethnic = string.Empty;
            Education = string.Empty;
            IncomeBasis = string.Empty;
            HealthIns = string.Empty;
            Vet = string.Empty;
            Disable = string.Empty;
            FootStamps = string.Empty;
            Farmer = string.Empty;
            ApplDate = string.Empty;
            ApplTime = string.Empty;
            Ampm = string.Empty;
            InitialDate = string.Empty;
            Site = string.Empty;
            TotIncome = string.Empty;
            ProgIncome = string.Empty;
            ClaimSsno = string.Empty;
            ClaimSsbic = string.Empty;
            Wagem = string.Empty;
            Wic = string.Empty;
            Student = string.Empty;
            Resident = string.Empty;
            Pregnant = string.Empty;
            MaritalStatus = string.Empty;
            SchoolDistrict = string.Empty;
            AlienRegNo = string.Empty;
            LegalTowork = string.Empty;
            ExpireWorkDate = string.Empty;
            Employed = string.Empty;
            LastWorkDate = string.Empty;
            WorkLimit = string.Empty;
            ExplainWorkLimit = string.Empty;
            NumberOfcjobs = string.Empty;
            NumberofLvjobs = string.Empty;
            FullTimeHours = string.Empty;
            PartTimeHours = string.Empty;
            SeasonalEmploy = string.Empty;
            IstShift = string.Empty;
            IIndShift = string.Empty;
            IIIrdShift = string.Empty;
            RShift = string.Empty;
            EmployerName = string.Empty;
            EmployerStreet = string.Empty;
            EmployerCity = string.Empty;
            JobTitle = string.Empty;
            JobCategory = string.Empty;
            HourlyWage = string.Empty;
            PayFrequency = string.Empty;
            HireDate = string.Empty;
            Transerv = string.Empty;
            Relitran = string.Empty;
            Drvlic = string.Empty;
            Race = string.Empty;
            EmplPhone = string.Empty;
            EmplExt = string.Empty;
            DobNa = string.Empty;
            SsnReason = string.Empty;
            Exclude = string.Empty;
            Blind = string.Empty;
            AbleTowork = string.Empty;
            RecMedicare = string.Empty;
            PurchaseFood = string.Empty;
            VechicleValue = string.Empty;
            OtherVehicleValue = string.Empty;
            OtherAssetValue = string.Empty;
            LstcOperator = string.Empty;
            DateLstc = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            SsnSearchType = string.Empty;
            AltAgency = string.Empty;
            AltDept = string.Empty;
            AltProgram = string.Empty;
            AltYear = string.Empty;
            AltApp = string.Empty;
            AltFamilySeq = string.Empty;
            Type = string.Empty;
            M_Code = string.Empty;

            SsnReasonDesc = string.Empty;
            GenderDesc = string.Empty;
            AreYouPregantDesc = string.Empty;
            MartialStatusDesc = string.Empty;
            RelationDesc = string.Empty;
            EthnicityDesc = string.Empty;
            RaceDesc = string.Empty;
            EducationDesc = string.Empty;
            SchoolDesc = string.Empty;
            ResidentDesc = string.Empty;
            HealthInsuranceDesc = string.Empty;
            VeterncodeDesc = string.Empty;
            FoodStampsDesc = string.Empty;
            DisabledDesc = string.Empty;
            farmerDesc = string.Empty;
            WicDesc = string.Empty;
            ReliableTransportDesc = string.Empty;
            DriverLicenceDesc = string.Empty;
            LegaltoworkDesc = string.Empty;
            ExpirationDateDesc = string.Empty;

            PresentEmployDesc = string.Empty;
            AnyworklimitationDesc = string.Empty;
            SeasonEmployedDesc = string.Empty;
            JobTitleDesc = string.Empty;
            JobCategoryDesc = string.Empty;
            PayFrequencyDesc = string.Empty;
            MilitaryStatus = string.Empty;
            WorkStatus = string.Empty;
            NonCashBenefits = string.Empty;
            Health_Codes = string.Empty;
            MilitaryStatusDesc = string.Empty;
            WorkStatusDesc = string.Empty;
            NonCashBenefitsDesc = string.Empty;
            Youth = string.Empty;
            YouthDesc = string.Empty;
            SnpSuffix = string.Empty;
            Snp_HH_ExcludeMem = string.Empty;
            CurrentAge = string.Empty;
            SnpIncomeTypes = string.Empty;
            Cacount = string.Empty; 
            Mscount = string.Empty;
            Contcount = string.Empty;
        }

        public CaseSnpEntity(DataRow row)
        {
            if (row != null)
            {
                // FormatedName = row["FORMATEDNAME"].ToString().Trim();
                SnpIncomeTypes = string.Empty;
                RecentMemberSwitch = string.Empty;
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString().Trim();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                SsBic = row["SNP_SS_BIC"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                AltLName = row["SNP_ALT_LNAME"].ToString().Trim();
                AltFi = row["SNP_ALT_FI"].ToString().Trim();
                Alias = row["SNP_ALIAS"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                Ethnic = row["SNP_ETHNIC"].ToString().Trim();
                Education = row["SNP_EDUCATION"].ToString().Trim();
                IncomeBasis = row["SNP_INCOME_BASIS"].ToString().Trim();
                HealthIns = row["SNP_HEALTH_INS"].ToString().Trim();
                Vet = row["SNP_VET"].ToString().Trim();
                Disable = row["SNP_DISABLE"].ToString().Trim();
                FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
                Farmer = row["SNP_FARMER"].ToString().Trim();
                ApplDate = row["SNP_APPL_DATE"].ToString().Trim();
                ApplTime = row["SNP_APPL_TIME"].ToString().Trim();
                Ampm = row["SNP_AMPM"].ToString().Trim();
                InitialDate = row["SNP_INTAKE_DATE"].ToString().Trim();
                Site = row["SNP_SITE"].ToString().Trim();
                TotIncome = row["SNP_TOT_INCOME"].ToString().Trim();
                ProgIncome = row["SNP_PROG_INCOME"].ToString().Trim();
                ClaimSsno = row["SNP_CLAIM_SSNO"].ToString().Trim();
                ClaimSsbic = row["SNP_CLAIM_SS_BIC"].ToString().Trim();
                Wagem = row["SNP_WAGEM"].ToString().Trim();
                Wic = row["SNP_WIC"].ToString().Trim();
                Student = row["SNP_STUDENT"].ToString().Trim();
                Resident = row["SNP_RESIDENT"].ToString().Trim();
                Pregnant = row["SNP_PREGNANT"].ToString().Trim();
                MaritalStatus = row["SNP_MARITAL_STATUS"].ToString().Trim();
                SchoolDistrict = row["SNP_SCHOOL_DISTRICT"].ToString().Trim();
                AlienRegNo = row["SNP_ALIEN_REG_NO"].ToString().Trim();
                LegalTowork = row["SNP_LEGAL_TO_WORK"].ToString().Trim();
                ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                Employed = row["SNP_EMPLOYED"].ToString().Trim();
                LastWorkDate = row["SNP_LAST_WORK_DATE"].ToString().Trim();
                WorkLimit = row["SNP_WORK_LIMIT"].ToString().Trim();
                ExplainWorkLimit = row["SNP_EXPLAIN_WORK_LIMIT"].ToString().Trim();
                NumberOfcjobs = row["SNP_NUMBER_OF_C_JOBS"].ToString().Trim();
                NumberofLvjobs = row["SNP_NUMBER_OF_LV_JOBS"].ToString().Trim();
                FullTimeHours = row["SNP_FULL_TIME_HOURS"].ToString().Trim();
                PartTimeHours = row["SNP_PART_TIME_HOURS"].ToString().Trim();
                SeasonalEmploy = row["SNP_SEASONAL_EMPLOY"].ToString().Trim();
                IstShift = row["SNP_1ST_SHIFT"].ToString().Trim();
                IIndShift = row["SNP_2ND_SHIFT"].ToString().Trim();
                IIIrdShift = row["SNP_3RD_SHIFT"].ToString().Trim();
                RShift = row["SNP_R_SHIFT"].ToString().Trim();
                EmployerName = row["SNP_EMPLOYER_NAME"].ToString().Trim();
                EmployerStreet = row["SNP_EMPLOYER_STREET"].ToString().Trim();
                EmployerCity = row["SNP_EMPLOYER_CITY"].ToString().Trim();
                JobTitle = row["SNP_JOB_TITLE"].ToString().Trim();
                JobCategory = row["SNP_JOB_CATEGORY"].ToString().Trim();
                HourlyWage = row["SNP_HOURLY_WAGE"].ToString().Trim();
                PayFrequency = row["SNP_PAY_FREQUENCY"].ToString().Trim();
                HireDate = row["SNP_HIRE_DATE"].ToString().Trim();
                Transerv = row["SNP_TRANSERV"].ToString().Trim();
                Relitran = row["SNP_RELITRAN"].ToString().Trim();
                Drvlic = row["SNP_DRVLIC"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                EmplPhone = row["SNP_EMPL_Phone"].ToString().Trim();
                EmplExt = row["SNP_EMPL_EXT"].ToString().Trim();
                DobNa = row["SNP_DOB_NA"].ToString().Trim();
                SsnReason = row["SNP_SSN_REASON"].ToString().Trim();
                Exclude = row["SNP_EXCLUDE"].ToString().Trim();
                Blind = row["SNP_BLIND"].ToString().Trim();
                AbleTowork = row["SNP_ABLE_TO_WORK"].ToString().Trim();
                RecMedicare = row["SNP_REC_MEDICARE"].ToString().Trim();
                PurchaseFood = row["SNP_PURCHASE_FOOD"].ToString().Trim();
                VechicleValue = row["SNP_VEHICLE_VALUE"].ToString().Trim();
                OtherVehicleValue = row["SNP_OTHER_VEHICLE_VALUE"].ToString().Trim();
                OtherAssetValue = row["SNP_OTHER_ASSET_VALUE"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();

                MilitaryStatus = row["SNP_MILITARY_STATUS"].ToString().Trim();
                WorkStatus = row["SNP_WORK_STAT"].ToString().Trim();
                NonCashBenefits = row["SNP_NCASHBEN"].ToString().Trim();
                Health_Codes = row["SNP_HEALTH_CODES"].ToString().Trim();
                Youth = row["SNP_YOUTH"].ToString().Trim();
                SnpSuffix = row["SNP_SUFFIX"].ToString().Trim();
                Snp_HH_ExcludeMem = row["SNP_HH_EXCLUDE"].ToString().Trim();
                YouthDesc = string.Empty;
                MilitaryStatusDesc = string.Empty;
                WorkStatusDesc = string.Empty;
                NonCashBenefitsDesc = string.Empty;

                M_Code = "M";
                SsnReasonDesc = string.Empty;
                GenderDesc = string.Empty;
                AreYouPregantDesc = string.Empty;
                MartialStatusDesc = string.Empty;
                RelationDesc = string.Empty;
                EthnicityDesc = string.Empty;
                RaceDesc = string.Empty;
                EducationDesc = string.Empty;
                SchoolDesc = string.Empty;
                ResidentDesc = string.Empty;
                HealthInsuranceDesc = string.Empty;
                VeterncodeDesc = string.Empty;
                FoodStampsDesc = string.Empty;
                DisabledDesc = string.Empty;
                farmerDesc = string.Empty;
                WicDesc = string.Empty;
                ReliableTransportDesc = string.Empty;
                DriverLicenceDesc = string.Empty;
                LegaltoworkDesc = string.Empty;
                ExpirationDateDesc = string.Empty;

                PresentEmployDesc = string.Empty;
                AnyworklimitationDesc = string.Empty;
                SeasonEmployedDesc = string.Empty;
                JobTitleDesc = string.Empty;
                JobCategoryDesc = string.Empty;
                PayFrequencyDesc = string.Empty;

                if (row.Table.Columns.Contains("Age"))
                {
                    KAge = row["Age"].ToString();
                }
                else
                    KAge = string.Empty;

                if (row.Table.Columns.Contains("Current_AGE"))
                {
                    CurrentAge = row["Current_AGE"].ToString();
                }
                else
                    CurrentAge = string.Empty;



            }

        }

        public CaseSnpEntity(DataRow row, string strTable)
        {

            if (strTable == "ImageUpload")
            {
                if (row != null)
                {
                    Agency = row["MST_AGENCY"].ToString().Trim();
                    Dept = row["MST_DEPT"].ToString().Trim();
                    Program = row["MST_PROGRAM"].ToString().Trim();
                    AltFamilySeq  = row["Mem_SEQ"].ToString().Trim();
                    Hierachy = row["Hierarchy"].ToString().Trim();
                    Year = row["YEAR"].ToString().Trim();
                    App = row["AppNo"].ToString().Trim();
                    FamilySeq = row["Mem_SEQ"].ToString().Trim();
                    FamilySeq = "0000000".Substring(0, 7 - FamilySeq.Length) + FamilySeq;
                    ProgrameName = row["ProgramName"].ToString().Trim();
                    M_Code = row["Member_Status"].ToString();
                    HierachyStatus = row["Hierarchy_status"].ToString();
                    AltBdate = row["DOB"].ToString();
                    NameixFi = row["Name"].ToString();
                    LstcOperator = row["Lstc_Date"].ToString();
                }
            }
            else if (strTable == "case2530Report")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString().Trim();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                Ethnic = row["SNP_ETHNIC"].ToString().Trim();
                Education = row["SNP_EDUCATION"].ToString().Trim();
                HealthIns = row["SNP_HEALTH_INS"].ToString().Trim();
                Vet = row["SNP_VET"].ToString().Trim();
                Disable = row["SNP_DISABLE"].ToString().Trim();
                FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
                Farmer = row["SNP_FARMER"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                //Site = row["SNP_SITE"].ToString().Trim();
                //TotIncome = row["SNP_TOT_INCOME"].ToString().Trim();
                //ProgIncome = row["SNP_PROG_INCOME"].ToString().Trim();
                //ClaimSsno = row["SNP_CLAIM_SSNO"].ToString().Trim();
                // ClaimSsbic = row["SNP_CLAIM_SS_BIC"].ToString().Trim();
                //  Wagem = row["SNP_WAGEM"].ToString().Trim();
                Wic = row["SNP_WIC"].ToString().Trim();
                //  Student = row["SNP_STUDENT"].ToString().Trim();
                Resident = row["SNP_RESIDENT"].ToString().Trim();
                Pregnant = row["SNP_PREGNANT"].ToString().Trim();
                MaritalStatus = row["SNP_MARITAL_STATUS"].ToString().Trim();
                SchoolDistrict = row["SNP_SCHOOL_DISTRICT"].ToString().Trim();
                // AlienRegNo = row["SNP_ALIEN_REG_NO"].ToString().Trim();
                LegalTowork = row["SNP_LEGAL_TO_WORK"].ToString().Trim();
                ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                Employed = row["SNP_EMPLOYED"].ToString().Trim();
                LastWorkDate = row["SNP_LAST_WORK_DATE"].ToString().Trim();
                WorkLimit = row["SNP_WORK_LIMIT"].ToString().Trim();
                // ExplainWorkLimit = row["SNP_EXPLAIN_WORK_LIMIT"].ToString().Trim();
                NumberOfcjobs = row["SNP_NUMBER_OF_C_JOBS"].ToString().Trim();
                NumberofLvjobs = row["SNP_NUMBER_OF_LV_JOBS"].ToString().Trim();
                FullTimeHours = row["SNP_FULL_TIME_HOURS"].ToString().Trim();
                PartTimeHours = row["SNP_PART_TIME_HOURS"].ToString().Trim();
                SeasonalEmploy = row["SNP_SEASONAL_EMPLOY"].ToString().Trim();
                IstShift = row["SNP_1ST_SHIFT"].ToString().Trim();
                IIndShift = row["SNP_2ND_SHIFT"].ToString().Trim();
                IIIrdShift = row["SNP_3RD_SHIFT"].ToString().Trim();
                RShift = row["SNP_R_SHIFT"].ToString().Trim();
                // EmployerStreet = row["SNP_EMPLOYER_STREET"].ToString().Trim();
                //EmployerCity = row["SNP_EMPLOYER_CITY"].ToString().Trim();
                JobTitle = row["SNP_JOB_TITLE"].ToString().Trim();
                JobCategory = row["SNP_JOB_CATEGORY"].ToString().Trim();
                HourlyWage = row["SNP_HOURLY_WAGE"].ToString().Trim();
                PayFrequency = row["SNP_PAY_FREQUENCY"].ToString().Trim();
                HireDate = row["SNP_HIRE_DATE"].ToString().Trim();
                Transerv = row["SNP_TRANSERV"].ToString().Trim();
                Relitran = row["SNP_RELITRAN"].ToString().Trim();
                Drvlic = row["SNP_DRVLIC"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                // EmplExt = row["SNP_EMPL_EXT"].ToString().Trim();
                //DobNa = row["SNP_DOB_NA"].ToString().Trim();
                //  SsnReason = row["SNP_SSN_REASON"].ToString().Trim();

                MilitaryStatus = row["SNP_MILITARY_STATUS"].ToString().Trim();
                WorkStatus = row["SNP_WORK_STAT"].ToString().Trim();
                Health_Codes = row["SNP_HEALTH_CODES"].ToString().Trim();
                Youth = row["SNP_YOUTH"].ToString().Trim();

            }
            else if (strTable == "CASEMSTSNP")
            {
                Agency = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Program = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                App = row["MST_APP_NO"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                Mst_IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();

            }
            else if (strTable == "CASESITE")
            {
                Agency = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Program = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                App = row["MST_APP_NO"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                Mst_IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();

            }
            else if (strTable == "EMSB0008")
            {
                Agency = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Program = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                App = row["MST_APP_NO"].ToString().Trim();
                Mst_IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                Mst_IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();

            }
            else if (strTable == "TMSB0023")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString().Trim();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
            }
            else if (strTable == "FIXCLIENT")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();
            }
            else if (strTable == "FIXCLIENTADDDATE")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();
                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                LstcOperator= row["SNP_LSTC_OPERATOR"].ToString().Trim();



            }
            else if (strTable == "FIXFAMILYID")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

                string strFamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                ClientId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();
                // Added 23/Jan/2020 Tool tip add and lstc dates and operators

                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();

            }
            else if (strTable == "FIXFAMILYIDDATE")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                string strFamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                ClientId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;

                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();

                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();

                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();


            }
            else if (strTable == "FIXFAMILYIDBENLEVEL")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

                string strFamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                ClientId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();
                // Added 23/Jan/2020 Tool tip add and lstc dates and operators

                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                IncomeBasis = row["BENLEVEL"].ToString().Trim();
                Cacount = row["CACOUNT"].ToString().Trim();
                Mscount = row["MSCOUNT"].ToString().Trim();
                Contcount = row["CONTCOUNT"].ToString().Trim();



            }
            else if (strTable == "FIXFAMILYIDBENLEVELINTAKE")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

                string strFamilyId = row["MST_FAMILY_ID"].ToString().Trim();
                ClientId = "000000000".Substring(0, 9 - strFamilyId.Length) + strFamilyId;
                ClaimSsno = row["SNP_MAXClinetId"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                AltApp = row["AppNo"].ToString().Trim();
                // Added 23/Jan/2020 Tool tip add and lstc dates and operators

                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                IncomeBasis = row["BENLEVEL"].ToString().Trim();
                Cacount = row["CACOUNT"].ToString().Trim();
                Mscount = row["MSCOUNT"].ToString().Trim();
                Contcount = row["CONTCOUNT"].ToString().Trim();
                Mst_IntakeDate = row["MST_INTAKE_DATE"].ToString().Trim();


            }
            else if (strTable == "TMSB2022")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();

            }
            else if (strTable == "TRIGGERS")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                Disable = row["SNP_DISABLE"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                DobNa = row["SNP_DOB_NA"].ToString().Trim();
                FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();

            }
            else if (strTable == "SSBG")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                Disable = row["SNP_DISABLE"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                DobNa = row["SNP_DOB_NA"].ToString().Trim();
                FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                Ethnic = row["SNP_ETHNIC"].ToString().Trim();

            }
            else if (strTable == "FIXSSN#")
            {

                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                // NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

            }
            else if (strTable == "FIXSSNFName")
            {

                NameixMi = string.Empty;
                NameixLast = string.Empty;//row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                // NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

            }
            else if (strTable == "CLIENTFNameDOB")
            {
                Ssno = "ClientId";
                NameixMi = string.Empty;
                NameixLast = string.Empty;//row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                // NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();

            }
            else if (strTable == "OnlySSN")
            {

                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = "SSNNUMBER";
                NameixFi = string.Empty;
                AltBdate = string.Empty;

            }
            else if (strTable == "OnlyClientId")
            {
                Ssno = "ClientId";
                ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
                NameixLast = "Clientid";
                NameixFi = string.Empty;
                AltBdate = string.Empty;
                NameixMi = string.Empty;
            }

            else if (strTable == "EMSB0028")// || strTable == "CEAPB002")
            {
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString().Trim();
                App = row["SNP_APP"].ToString().Trim();
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
                ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                SsBic = row["SNP_SS_BIC"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                AltLName = row["SNP_ALT_LNAME"].ToString().Trim();
                AltFi = row["SNP_ALT_FI"].ToString().Trim();
                Alias = row["SNP_ALIAS"].ToString().Trim();
                Status = row["SNP_STATUS"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                Ethnic = row["SNP_ETHNIC"].ToString().Trim();
                Education = row["SNP_EDUCATION"].ToString().Trim();
                IncomeBasis = row["SNP_INCOME_BASIS"].ToString().Trim();
                HealthIns = row["SNP_HEALTH_INS"].ToString().Trim();
                Vet = row["SNP_VET"].ToString().Trim();
                Disable = row["SNP_DISABLE"].ToString().Trim();
                FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
                Farmer = row["SNP_FARMER"].ToString().Trim();
                ApplDate = row["SNP_APPL_DATE"].ToString().Trim();
                ApplTime = row["SNP_APPL_TIME"].ToString().Trim();
                Ampm = row["SNP_AMPM"].ToString().Trim();
                InitialDate = row["SNP_INTAKE_DATE"].ToString().Trim();
                Site = row["SNP_SITE"].ToString().Trim();
                TotIncome = row["SNP_TOT_INCOME"].ToString().Trim();
                ProgIncome = row["SNP_PROG_INCOME"].ToString().Trim();
                ClaimSsno = row["SNP_CLAIM_SSNO"].ToString().Trim();
                ClaimSsbic = row["SNP_CLAIM_SS_BIC"].ToString().Trim();
                Wagem = row["SNP_WAGEM"].ToString().Trim();
                Wic = row["SNP_WIC"].ToString().Trim();
                Student = row["SNP_STUDENT"].ToString().Trim();
                Resident = row["SNP_RESIDENT"].ToString().Trim();
                Pregnant = row["SNP_PREGNANT"].ToString().Trim();
                MaritalStatus = row["SNP_MARITAL_STATUS"].ToString().Trim();
                SchoolDistrict = row["SNP_SCHOOL_DISTRICT"].ToString().Trim();
                AlienRegNo = row["SNP_ALIEN_REG_NO"].ToString().Trim();
                LegalTowork = row["SNP_LEGAL_TO_WORK"].ToString().Trim();
                ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                Employed = row["SNP_EMPLOYED"].ToString().Trim();
                LastWorkDate = row["SNP_LAST_WORK_DATE"].ToString().Trim();
                WorkLimit = row["SNP_WORK_LIMIT"].ToString().Trim();
                ExplainWorkLimit = row["SNP_EXPLAIN_WORK_LIMIT"].ToString().Trim();
                NumberOfcjobs = row["SNP_NUMBER_OF_C_JOBS"].ToString().Trim();
                NumberofLvjobs = row["SNP_NUMBER_OF_LV_JOBS"].ToString().Trim();
                FullTimeHours = row["SNP_FULL_TIME_HOURS"].ToString().Trim();
                PartTimeHours = row["SNP_PART_TIME_HOURS"].ToString().Trim();
                SeasonalEmploy = row["SNP_SEASONAL_EMPLOY"].ToString().Trim();
                IstShift = row["SNP_1ST_SHIFT"].ToString().Trim();
                IIndShift = row["SNP_2ND_SHIFT"].ToString().Trim();
                IIIrdShift = row["SNP_3RD_SHIFT"].ToString().Trim();
                RShift = row["SNP_R_SHIFT"].ToString().Trim();
                EmployerName = row["SNP_EMPLOYER_NAME"].ToString().Trim();
                EmployerStreet = row["SNP_EMPLOYER_STREET"].ToString().Trim();
                EmployerCity = row["SNP_EMPLOYER_CITY"].ToString().Trim();
                JobTitle = row["SNP_JOB_TITLE"].ToString().Trim();
                JobCategory = row["SNP_JOB_CATEGORY"].ToString().Trim();
                HourlyWage = row["SNP_HOURLY_WAGE"].ToString().Trim();
                PayFrequency = row["SNP_PAY_FREQUENCY"].ToString().Trim();
                HireDate = row["SNP_HIRE_DATE"].ToString().Trim();
                Transerv = row["SNP_TRANSERV"].ToString().Trim();
                Relitran = row["SNP_RELITRAN"].ToString().Trim();
                Drvlic = row["SNP_DRVLIC"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                EmplPhone = row["SNP_EMPL_Phone"].ToString().Trim();
                EmplExt = row["SNP_EMPL_EXT"].ToString().Trim();
                DobNa = row["SNP_DOB_NA"].ToString().Trim();
                SsnReason = row["SNP_SSN_REASON"].ToString().Trim();
                Exclude = row["SNP_EXCLUDE"].ToString().Trim();
                Blind = row["SNP_BLIND"].ToString().Trim();
                AbleTowork = row["SNP_ABLE_TO_WORK"].ToString().Trim();
                RecMedicare = row["SNP_REC_MEDICARE"].ToString().Trim();
                PurchaseFood = row["SNP_PURCHASE_FOOD"].ToString().Trim();
                VechicleValue = row["SNP_VEHICLE_VALUE"].ToString().Trim();
                OtherVehicleValue = row["SNP_OTHER_VEHICLE_VALUE"].ToString().Trim();
                OtherAssetValue = row["SNP_OTHER_ASSET_VALUE"].ToString().Trim();
                LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
                AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();

                MilitaryStatus = row["SNP_MILITARY_STATUS"].ToString().Trim();
                WorkStatus = row["SNP_WORK_STAT"].ToString().Trim();
                NonCashBenefits = row["SNP_NCASHBEN"].ToString().Trim();
                Health_Codes = row["SNP_HEALTH_CODES"].ToString().Trim();
                Youth = row["SNP_YOUTH"].ToString().Trim();
                SnpSuffix = row["SNP_SUFFIX"].ToString().Trim();
                Snp_HH_ExcludeMem = row["SNP_HH_EXCLUDE"].ToString().Trim();

                TANF = row["TANF"].ToString().Trim();
                SSI = row["SSI"].ToString().Trim();
                VABenfitis = row["VABENEFITS"].ToString().Trim();
                VABENNONSERVICE = row["VABENNONSERVICE"].ToString().Trim();

                //Added by Vikash on 01/23/2025
                //SSDI = row["SSDI"].ToString().Trim();
                //PRIVDISINS = row["PRIVDISINS"].ToString().Trim();
                //WORKCOMP = row["WORKCOMP"].ToString().Trim();
                //RETIRINCOME = row["RETIRINCOME"].ToString().Trim();
                //PENSION = row["PENSION"].ToString().Trim();
                //CHILDSUPORT = row["CHILDSUPORT"].ToString().Trim();
                //ALIMONY = row["ALIMONY"].ToString().Trim();
                //UNEMPINS = row["UNEMPINS"].ToString().Trim();
                //EITC = row["EITC"].ToString().Trim();
                //OTHERINCOME = row["OTHERINCOME"].ToString().Trim();
                //UNKNINCOME = row["UNKNINCOME"].ToString().Trim();
            }

            else
            {
                if (row != null)
                {
                    Agency = row["SNP_AGENCY"].ToString().Trim();
                    Dept = row["SNP_DEPT"].ToString().Trim();
                    Program = row["SNP_PROGRAM"].ToString().Trim();
                    Year = row["SNP_YEAR"].ToString().Trim();
                    App = row["SNP_APP"].ToString().Trim();
                    FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                    Ssno = row["SNP_SSNO"].ToString().Trim();
                    AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                    NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                }
            }


        }

        public CaseSnpEntity(string strFirstName, string strLastName, string strMiddleName, string strApplicant)
        {
            SnpIncomeTypes = string.Empty;
            FormatedName = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App = strApplicant;
            FamilySeq = string.Empty;
            MemberCode = string.Empty;
            ClientId = string.Empty;
            Ssno = string.Empty;
            SsBic = string.Empty;
            NameixLast = strLastName;
            NameixFi = strFirstName;
            NameixMi = strMiddleName;
            AltBdate = string.Empty;
            AltLName = string.Empty;
            AltFi = string.Empty;
            Alias = string.Empty;
            Status = string.Empty;
            Sex = string.Empty;
            Age = string.Empty;
            Ethnic = string.Empty;
            Education = string.Empty;
            IncomeBasis = string.Empty;
            HealthIns = string.Empty;
            Vet = string.Empty;
            Disable = string.Empty;
            FootStamps = string.Empty;
            Farmer = string.Empty;
            ApplDate = string.Empty;
            ApplTime = string.Empty;
            Ampm = string.Empty;
            InitialDate = string.Empty;
            Site = string.Empty;
            TotIncome = string.Empty;
            ProgIncome = string.Empty;
            ClaimSsno = string.Empty;
            ClaimSsbic = string.Empty;
            Wagem = string.Empty;
            Wic = string.Empty;
            Student = string.Empty;
            Resident = string.Empty;
            Pregnant = string.Empty;
            MaritalStatus = string.Empty;
            SchoolDistrict = string.Empty;
            AlienRegNo = string.Empty;
            LegalTowork = string.Empty;
            ExpireWorkDate = string.Empty;
            Employed = string.Empty;
            LastWorkDate = string.Empty;
            WorkLimit = string.Empty;
            ExplainWorkLimit = string.Empty;
            NumberOfcjobs = string.Empty;
            NumberofLvjobs = string.Empty;
            FullTimeHours = string.Empty;
            PartTimeHours = string.Empty;
            SeasonalEmploy = string.Empty;
            IstShift = string.Empty;
            IIndShift = string.Empty;
            IIIrdShift = string.Empty;
            RShift = string.Empty;
            EmployerName = string.Empty;
            EmployerStreet = string.Empty;
            EmployerCity = string.Empty;
            JobTitle = string.Empty;
            JobCategory = string.Empty;
            HourlyWage = string.Empty;
            PayFrequency = string.Empty;
            HireDate = string.Empty;
            Transerv = string.Empty;
            Relitran = string.Empty;
            Drvlic = string.Empty;
            Race = string.Empty;
            EmplPhone = string.Empty;
            EmplExt = string.Empty;
            DobNa = string.Empty;
            SsnReason = string.Empty;
            Exclude = string.Empty;
            Blind = string.Empty;
            AbleTowork = string.Empty;
            RecMedicare = string.Empty;
            PurchaseFood = string.Empty;
            VechicleValue = string.Empty;
            OtherVehicleValue = string.Empty;
            OtherAssetValue = string.Empty;
            LstcOperator = string.Empty;
            DateLstc = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            SsnSearchType = string.Empty;
            AltAgency = string.Empty;
            AltDept = string.Empty;
            AltProgram = string.Empty;
            AltYear = string.Empty;
            AltApp = string.Empty;
            AltFamilySeq = string.Empty;
            Type = string.Empty;
            M_Code = string.Empty;

            SsnReasonDesc = string.Empty;
            GenderDesc = string.Empty;
            AreYouPregantDesc = string.Empty;
            MartialStatusDesc = string.Empty;
            RelationDesc = string.Empty;
            EthnicityDesc = string.Empty;
            RaceDesc = string.Empty;
            EducationDesc = string.Empty;
            SchoolDesc = string.Empty;
            ResidentDesc = string.Empty;
            HealthInsuranceDesc = string.Empty;
            VeterncodeDesc = string.Empty;
            FoodStampsDesc = string.Empty;
            DisabledDesc = string.Empty;
            farmerDesc = string.Empty;
            WicDesc = string.Empty;
            ReliableTransportDesc = string.Empty;
            DriverLicenceDesc = string.Empty;
            LegaltoworkDesc = string.Empty;
            ExpirationDateDesc = string.Empty;

            PresentEmployDesc = string.Empty;
            AnyworklimitationDesc = string.Empty;
            SeasonEmployedDesc = string.Empty;
            JobTitleDesc = string.Empty;
            JobCategoryDesc = string.Empty;
            PayFrequencyDesc = string.Empty;
            MilitaryStatus = string.Empty;
            WorkStatus = string.Empty;
            NonCashBenefits = string.Empty;
            Health_Codes = string.Empty;
            MilitaryStatusDesc = string.Empty;
            WorkStatusDesc = string.Empty;
            NonCashBenefitsDesc = string.Empty;
            Youth = string.Empty;
            YouthDesc = string.Empty;
            SnpSuffix = string.Empty;
            Snp_HH_ExcludeMem = string.Empty;
            CurrentAge = string.Empty;
            TANF = string.Empty;
            SSI = string.Empty;
            VABenfitis = string.Empty;
            VABENNONSERVICE = string.Empty;
        }

        public CaseSnpEntity(DataRow row, string strTable, int intsearchcount, string strssnswitch, string strNameswitch, string strLastNameSwitch, string strdobswitch)
        {

            Agency = row["Agency"].ToString().Trim();
            Dept = row["Dept"].ToString().Trim();
            Program = row["Prog"].ToString().Trim();
            Year = row["Snpyear"].ToString().Trim();
            App = row["Applicantno"].ToString().Trim();
            Appdetails = row["AppNo"].ToString().Trim();
            FamilySeq = row["RecFamSeq"].ToString().Trim();
            MemberCode = row["MEM_CODE"].ToString().Trim();
            ClientId = row["ClientId"].ToString().Trim();
            Ssno = row["SSN"].ToString().Trim();
            NameixLast = row["LName"].ToString().Trim();
            NameixFi = row["FName"].ToString().Trim();
            NameixMi = row["MName"].ToString().Trim();
            AltBdate = row["DOB"].ToString().Trim();
            EmplPhone = row["Phone"].ToString().Trim();
            Mst_IntakeWorker = row["MST_INTAKE_WORKER"].ToString().Trim();
            Mst_Hno = row["Hno"].ToString().Trim();
            Mst_Street = row["Street"].ToString().Trim();
            Mst_City = row["City"].ToString().Trim();
            Mst_State = row["State1"].ToString().Trim();
            Mst_Zip = row["Zip"].ToString().Trim();
            Mst_CaseType = row["CaseType"].ToString().Trim();
            Mst_RecKey = row["RecKey"].ToString().Trim();
            SnpAcitveStatus = row["AppStatus"].ToString().Trim();
            Exclude = row["SNP_EXCLUDE"].ToString().Trim();
            Mst_Site = row["MST_SITE"].ToString().Trim();
            Mst_ActiveStatus = row["MST_ACTIVE_STATUS"].ToString().Trim();
            DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
            searchAppCount = intsearchcount;
            SsnSwitch = strssnswitch;
            NameSwitch = strNameswitch;
            DobSwitch = strdobswitch;
            LastNameSwitch = strLastNameSwitch;

        }

        //Vikash added on 01/23/2025 to add parameters for CSBG update in Performance Measures Report
        public CaseSnpEntity(DataRow row, string strTable, string isCSBG)
        {
            Agency = row["SNP_AGENCY"].ToString().Trim();
            Dept = row["SNP_DEPT"].ToString().Trim();
            Program = row["SNP_PROGRAM"].ToString().Trim();
            Year = row["SNP_YEAR"].ToString().Trim();
            App = row["SNP_APP"].ToString().Trim();
            FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
            MemberCode = row["SNP_MEMBER_CODE"].ToString().Trim();
            ClientId = row["SNP_CLIENT_ID"].ToString().Trim();
            Ssno = row["SNP_SSNO"].ToString().Trim();
            SsBic = row["SNP_SS_BIC"].ToString().Trim();
            NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
            NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
            NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
            AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
            AltLName = row["SNP_ALT_LNAME"].ToString().Trim();
            AltFi = row["SNP_ALT_FI"].ToString().Trim();
            Alias = row["SNP_ALIAS"].ToString().Trim();
            Status = row["SNP_STATUS"].ToString().Trim();
            Sex = row["SNP_SEX"].ToString().Trim();
            Age = row["SNP_AGE"].ToString().Trim();
            Ethnic = row["SNP_ETHNIC"].ToString().Trim();
            Education = row["SNP_EDUCATION"].ToString().Trim();
            IncomeBasis = row["SNP_INCOME_BASIS"].ToString().Trim();
            HealthIns = row["SNP_HEALTH_INS"].ToString().Trim();
            Vet = row["SNP_VET"].ToString().Trim();
            Disable = row["SNP_DISABLE"].ToString().Trim();
            FootStamps = row["SNP_FOOD_STAMPS"].ToString().Trim();
            Farmer = row["SNP_FARMER"].ToString().Trim();
            ApplDate = row["SNP_APPL_DATE"].ToString().Trim();
            ApplTime = row["SNP_APPL_TIME"].ToString().Trim();
            Ampm = row["SNP_AMPM"].ToString().Trim();
            InitialDate = row["SNP_INTAKE_DATE"].ToString().Trim();
            Site = row["SNP_SITE"].ToString().Trim();
            TotIncome = row["SNP_TOT_INCOME"].ToString().Trim();
            ProgIncome = row["SNP_PROG_INCOME"].ToString().Trim();
            ClaimSsno = row["SNP_CLAIM_SSNO"].ToString().Trim();
            ClaimSsbic = row["SNP_CLAIM_SS_BIC"].ToString().Trim();
            Wagem = row["SNP_WAGEM"].ToString().Trim();
            Wic = row["SNP_WIC"].ToString().Trim();
            Student = row["SNP_STUDENT"].ToString().Trim();
            Resident = row["SNP_RESIDENT"].ToString().Trim();
            Pregnant = row["SNP_PREGNANT"].ToString().Trim();
            MaritalStatus = row["SNP_MARITAL_STATUS"].ToString().Trim();
            SchoolDistrict = row["SNP_SCHOOL_DISTRICT"].ToString().Trim();
            AlienRegNo = row["SNP_ALIEN_REG_NO"].ToString().Trim();
            LegalTowork = row["SNP_LEGAL_TO_WORK"].ToString().Trim();
            ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
            Employed = row["SNP_EMPLOYED"].ToString().Trim();
            LastWorkDate = row["SNP_LAST_WORK_DATE"].ToString().Trim();
            WorkLimit = row["SNP_WORK_LIMIT"].ToString().Trim();
            ExplainWorkLimit = row["SNP_EXPLAIN_WORK_LIMIT"].ToString().Trim();
            NumberOfcjobs = row["SNP_NUMBER_OF_C_JOBS"].ToString().Trim();
            NumberofLvjobs = row["SNP_NUMBER_OF_LV_JOBS"].ToString().Trim();
            FullTimeHours = row["SNP_FULL_TIME_HOURS"].ToString().Trim();
            PartTimeHours = row["SNP_PART_TIME_HOURS"].ToString().Trim();
            SeasonalEmploy = row["SNP_SEASONAL_EMPLOY"].ToString().Trim();
            IstShift = row["SNP_1ST_SHIFT"].ToString().Trim();
            IIndShift = row["SNP_2ND_SHIFT"].ToString().Trim();
            IIIrdShift = row["SNP_3RD_SHIFT"].ToString().Trim();
            RShift = row["SNP_R_SHIFT"].ToString().Trim();
            EmployerName = row["SNP_EMPLOYER_NAME"].ToString().Trim();
            EmployerStreet = row["SNP_EMPLOYER_STREET"].ToString().Trim();
            EmployerCity = row["SNP_EMPLOYER_CITY"].ToString().Trim();
            JobTitle = row["SNP_JOB_TITLE"].ToString().Trim();
            JobCategory = row["SNP_JOB_CATEGORY"].ToString().Trim();
            HourlyWage = row["SNP_HOURLY_WAGE"].ToString().Trim();
            PayFrequency = row["SNP_PAY_FREQUENCY"].ToString().Trim();
            HireDate = row["SNP_HIRE_DATE"].ToString().Trim();
            Transerv = row["SNP_TRANSERV"].ToString().Trim();
            Relitran = row["SNP_RELITRAN"].ToString().Trim();
            Drvlic = row["SNP_DRVLIC"].ToString().Trim();
            Race = row["SNP_RACE"].ToString().Trim();
            EmplPhone = row["SNP_EMPL_Phone"].ToString().Trim();
            EmplExt = row["SNP_EMPL_EXT"].ToString().Trim();
            DobNa = row["SNP_DOB_NA"].ToString().Trim();
            SsnReason = row["SNP_SSN_REASON"].ToString().Trim();
            Exclude = row["SNP_EXCLUDE"].ToString().Trim();
            Blind = row["SNP_BLIND"].ToString().Trim();
            AbleTowork = row["SNP_ABLE_TO_WORK"].ToString().Trim();
            RecMedicare = row["SNP_REC_MEDICARE"].ToString().Trim();
            PurchaseFood = row["SNP_PURCHASE_FOOD"].ToString().Trim();
            VechicleValue = row["SNP_VEHICLE_VALUE"].ToString().Trim();
            OtherVehicleValue = row["SNP_OTHER_VEHICLE_VALUE"].ToString().Trim();
            OtherAssetValue = row["SNP_OTHER_ASSET_VALUE"].ToString().Trim();
            LstcOperator = row["SNP_LSTC_OPERATOR"].ToString().Trim();
            DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
            DateAdd = row["SNP_DATE_ADD"].ToString().Trim();
            AddOperator = row["SNP_ADD_OPERATOR"].ToString().Trim();

            MilitaryStatus = row["SNP_MILITARY_STATUS"].ToString().Trim();
            WorkStatus = row["SNP_WORK_STAT"].ToString().Trim();
            NonCashBenefits = row["SNP_NCASHBEN"].ToString().Trim();
            Health_Codes = row["SNP_HEALTH_CODES"].ToString().Trim();
            Youth = row["SNP_YOUTH"].ToString().Trim();
            SnpSuffix = row["SNP_SUFFIX"].ToString().Trim();
            Snp_HH_ExcludeMem = row["SNP_HH_EXCLUDE"].ToString().Trim();

            TANF = row["TANF"].ToString().Trim();
            SSI = row["SSI"].ToString().Trim();
            VABenfitis = row["VABENEFITS"].ToString().Trim();
            VABENNONSERVICE = row["VABENNONSERVICE"].ToString().Trim();

            //Added by Vikash on 01/23/2025
            if (isCSBG == "CSBG")
            {
                SSDI = row["SSDI"].ToString().Trim();
                PRIVDISINS = row["PRIVDISINS"].ToString().Trim();
                WORKCOMP = row["WORKCOMP"].ToString().Trim();
                RETIRINCOME = row["RETIRINCOME"].ToString().Trim();
                PENSION = row["PENSION"].ToString().Trim();
                CHILDSUPORT = row["CHILDSUPORT"].ToString().Trim();
                ALIMONY = row["ALIMONY"].ToString().Trim();
                UNEMPINS = row["UNEMPINS"].ToString().Trim();
                EITC = row["EITC"].ToString().Trim();
                OTHERINCOME = row["OTHERINCOME"].ToString().Trim();
                UNKNINCOME = row["UNKNINCOME"].ToString().Trim();
            }
        }

        #endregion

        #region Properties
        public string FormatedName { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string FamilySeq { get; set; }
        public string MemberCode { get; set; }
        public string ClientId { get; set; }
        public string Ssno { get; set; }
        public string SsBic { get; set; }
        public string NameixLast { get; set; }
        public string NameixFi { get; set; }
        public string NameixMi { get; set; }
        public string AltBdate { get; set; }
        public string AltLName { get; set; }
        public string AltFi { get; set; }
        public string Alias { get; set; }
        public string Status { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string Ethnic { get; set; }
        public string Education { get; set; }
        public string IncomeBasis { get; set; }
        public string HealthIns { get; set; }
        public string Vet { get; set; }
        public string Disable { get; set; }
        public string FootStamps { get; set; }
        public string Farmer { get; set; }
        public string ApplDate { get; set; }
        public string ApplTime { get; set; }
        public string Ampm { get; set; }
        public string InitialDate { get; set; }
        public string Site { get; set; }
        public string TotIncome { get; set; }
        public string ProgIncome { get; set; }
        public string ClaimSsno { get; set; }
        public string ClaimSsbic { get; set; }
        public string Wagem { get; set; }
        public string Wic { get; set; }
        public string Student { get; set; }
        public string Resident { get; set; }
        public string Pregnant { get; set; }
        public string MaritalStatus { get; set; }
        public string SchoolDistrict { get; set; }
        public string AlienRegNo { get; set; }
        public string LegalTowork { get; set; }
        public string ExpireWorkDate { get; set; }
        public string Employed { get; set; }
        public string LastWorkDate { get; set; }
        public string WorkLimit { get; set; }
        public string ExplainWorkLimit { get; set; }
        public string NumberOfcjobs { get; set; }
        public string NumberofLvjobs { get; set; }
        public string FullTimeHours { get; set; }
        public string PartTimeHours { get; set; }
        public string SeasonalEmploy { get; set; }
        public string IstShift { get; set; }
        public string IIndShift { get; set; }
        public string IIIrdShift { get; set; }
        public string RShift { get; set; }
        public string EmployerName { get; set; }
        public string EmployerStreet { get; set; }
        public string EmployerCity { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public string HourlyWage { get; set; }
        public string PayFrequency { get; set; }
        public string HireDate { get; set; }
        public string Transerv { get; set; }
        public string Relitran { get; set; }
        public string Drvlic { get; set; }
        public string Race { get; set; }
        public string EmplPhone { get; set; }
        public string EmplExt { get; set; }
        public string DobNa { get; set; }
        public string SsnReason { get; set; }
        public string Exclude { get; set; }
        public string Blind { get; set; }
        public string AbleTowork { get; set; }
        public string RecMedicare { get; set; }
        public string PurchaseFood { get; set; }
        public string VechicleValue { get; set; }
        public string OtherVehicleValue { get; set; }
        public string OtherAssetValue { get; set; }
        public string LstcOperator { get; set; }
        public string DateLstc { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Mode { get; set; }
        public string SsnSearchType { get; set; }
        public string AltAgency { get; set; }
        public string AltDept { get; set; }
        public string AltProgram { get; set; }
        public string AltYear { get; set; }
        public string AltApp { get; set; }
        public string AltFamilySeq { get; set; }
        public string Type { get; set; }
        public string M_Code { get; set; }
        public string Hierachy { get; set; }
        public string ProgrameName { get; set; }
        public string HierachyStatus { get; set; }

        public string SsnReasonDesc { get; set; }
        public string GenderDesc { get; set; }
        public string AreYouPregantDesc { get; set; }
        public string MartialStatusDesc { get; set; }
        public string RelationDesc { get; set; }
        public string EthnicityDesc { get; set; }
        public string RaceDesc { get; set; }
        public string EducationDesc { get; set; }
        public string SchoolDesc { get; set; }
        public string ResidentDesc { get; set; }
        public string HealthInsuranceDesc { get; set; }
        public string VeterncodeDesc { get; set; }
        public string FoodStampsDesc { get; set; }
        public string DisabledDesc { get; set; }
        public string farmerDesc { get; set; }
        public string WicDesc { get; set; }
        public string ReliableTransportDesc { get; set; }
        public string DriverLicenceDesc { get; set; }
        public string LegaltoworkDesc { get; set; }
        public string ExpirationDateDesc { get; set; }
        public string PresentEmployDesc { get; set; }
        public string AnyworklimitationDesc { get; set; }
        public string SeasonEmployedDesc { get; set; }
        public string JobTitleDesc { get; set; }
        public string JobCategoryDesc { get; set; }
        public string PayFrequencyDesc { get; set; }

        public string Mst_IntakeDate { get; set; }
        public string Mst_IntakeWorker { get; set; }
        public string Mst_Hno { get; set; }
        public string Mst_Street { get; set; }
        public string Mst_City { get; set; }
        public string Mst_State { get; set; }
        public string Mst_Zip { get; set; }
        public string Mst_CaseType { get; set; }
        public string Mst_RecKey { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_ActiveStatus { get; set; }
        public string SnpAcitveStatus { get; set; }
        public string Appdetails { get; set; }
        public int searchAppCount { get; set; }
        public string SsnSwitch { get; set; }
        public string NameSwitch { get; set; }
        public string DobSwitch { get; set; }
        public string LastNameSwitch { get; set; }

        public string MilitaryStatus { get; set; }
        public string WorkStatus { get; set; }
        public string NonCashBenefits { get; set; }
        public string Health_Codes { get; set; }
        public string MilitaryStatusDesc { get; set; }
        public string WorkStatusDesc { get; set; }
        public string NonCashBenefitsDesc { get; set; }
        public string YouthDesc { get; set; }
        public string Youth { get; set; }
        public string KAge { get; set; }
        public string SnpSuffix { get; set; }
        public string Snp_HH_ExcludeMem { get; set; }

        public string TANF { get; set; }
        public string SSI { get; set; }
        public string VABenfitis { get; set; }
        public string VABENNONSERVICE { get; set; }

        public string CurrentAge { get; set; }

        public string RecentMemberSwitch { get; set; }

        public string SnpIncomeTypes { get; set; }

        public string Cacount { get; set; }
        public string Mscount { get; set; }
        public string Contcount { get; set; }
    
        public string SSDI
        {
            get;
            set;
        }
        public string PRIVDISINS
        {
            get;
            set;
        }
        public string WORKCOMP
        {
            get;
            set;
        }
        public string RETIRINCOME
        {
            get;
            set;
        }
        public string PENSION
        {
            get;
            set;
        }
        public string CHILDSUPORT
        {
            get;
            set;
        }
        public string ALIMONY
        {
            get;
            set;
        }
        public string UNEMPINS
        {
            get;
            set;
        }
        public string EITC
        {
            get;
            set;
        }
        public string OTHERINCOME
        {
            get;
            set;
        }
        public string UNKNINCOME
        {
            get;
            set;
        }


        #endregion
    }

    public class CaseIncomeEntity
    {
        #region Constructors

        public CaseIncomeEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            FamilySeq = string.Empty;
            Seq = string.Empty;
            Exclude = string.Empty;
            Type = string.Empty;
            Interval = string.Empty;
            Val1 = string.Empty;
            Date1 = string.Empty;
            Val2 = string.Empty;
            Date2 = string.Empty;
            Val3 = string.Empty;
            Date3 = string.Empty;
            Val4 = string.Empty;
            Date4 = string.Empty;
            Val5 = string.Empty;
            Date5 = string.Empty;
            Factor = string.Empty;
            Source = string.Empty;
            Verifier = string.Empty;
            HowVerified = string.Empty;
            TotIncome = string.Empty;
            ProgIncome = string.Empty;
            LstcOperator = string.Empty;
            DateLstc = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            Name = string.Empty;
            HrRate1 = string.Empty;
            HrRate2 = string.Empty;
            HrRate3 = string.Empty;
            HrRate4 = string.Empty;
            HrRate5 = string.Empty;
            Average = string.Empty;
            VerMethod= string.Empty;
        }


        public CaseIncomeEntity(DataRow row, string strType)
        {

            if (row != null)
            {
                if (strType == "TRIGGERS")
                {
                    Agency = row["INCOME_AGENCY"].ToString().Trim();
                    Dept = row["INCOME_DEPT"].ToString().Trim();
                    Program = row["INCOME_PROGRAM"].ToString().Trim();
                    Year = row["INCOME_YEAR"].ToString().Trim();
                    App = row["INCOME_APP"].ToString().Trim();
                    //FamilySeq = row["INCOME_FAMILY_SEQ"].ToString().Trim();
                    //Seq = row["INCOME_SEQ"].ToString().Trim();
                    //Exclude = row["INCOME_EXCLUDE"].ToString().Trim();
                    Type = row["INCOME_TYPE"].ToString().Trim();
                    //Interval = row["INCOME_INTERVAL"].ToString().Trim();
                    //Val1 = row["INCOME_VAL1"].ToString().Trim();
                    //Date1 = row["INCOME_DATE1"].ToString().Trim();
                    //Val2 = row["INCOME_VAL2"].ToString().Trim();
                    //Date2 = row["INCOME_DATE2"].ToString().Trim();
                    //Val3 = row["INCOME_VAL3"].ToString().Trim();
                    //Date3 = row["INCOME_DATE3"].ToString().Trim();
                    //Val4 = row["INCOME_VAL4"].ToString().Trim();
                    //Date4 = row["INCOME_DATE4"].ToString().Trim();
                    //Val5 = row["INCOME_VAL5"].ToString().Trim();
                    //Date5 = row["INCOME_DATE5"].ToString().Trim();
                    //Factor = row["INCOME_FACTOR"].ToString().Trim();
                    //Source = row["INCOME_SOURCE"].ToString().Trim();
                    //Verifier = row["INCOME_VERIFIER"].ToString().Trim();
                    //HowVerified = row["INCOME_HOW_VERIFIED"].ToString().Trim();
                    //TotIncome = row["INCOME_TOT_INCOME"].ToString().Trim();
                    //ProgIncome = row["INCOME_PROG_INCOME"].ToString().Trim();
                    //LstcOperator = row["INCOME_LSTC_OPERATOR"].ToString().Trim();
                    //DateLstc = row["INCOME_DATE_LSTC"].ToString().Trim();
                    //DateAdd = row["INCOME_DATE_ADD"].ToString().Trim();
                    //AddOperator = row["INCOME_ADD_OPERATOR"].ToString().Trim();
                    //HrRate1 = row["INCOME_HR_RATE1"].ToString().Trim();
                    //HrRate2 = row["INCOME_HR_RATE2"].ToString().Trim();
                    //HrRate3 = row["INCOME_HR_RATE3"].ToString().Trim();
                    //HrRate4 = row["INCOME_HR_RATE4"].ToString().Trim();
                    //HrRate5 = row["INCOME_HR_RATE5"].ToString().Trim();
                    //Average = row["INCOME_AVG"].ToString().Trim();
                }
                else
                {
                    Agency = row["INCOME_AGENCY"].ToString().Trim();
                    Dept = row["INCOME_DEPT"].ToString().Trim();
                    Program = row["INCOME_PROGRAM"].ToString().Trim();
                    Year = row["INCOME_YEAR"].ToString().Trim();
                    App = row["INCOME_APP"].ToString().Trim();
                    FamilySeq = row["INCOME_FAMILY_SEQ"].ToString().Trim();
                    Seq = row["INCOME_SEQ"].ToString().Trim();
                    Exclude = row["INCOME_EXCLUDE"].ToString().Trim();
                    Type = row["INCOME_TYPE"].ToString().Trim();
                    Interval = row["INCOME_INTERVAL"].ToString().Trim();
                    Val1 = row["INCOME_VAL1"].ToString().Trim();
                    Date1 = row["INCOME_DATE1"].ToString().Trim();
                    Val2 = row["INCOME_VAL2"].ToString().Trim();
                    Date2 = row["INCOME_DATE2"].ToString().Trim();
                    Val3 = row["INCOME_VAL3"].ToString().Trim();
                    Date3 = row["INCOME_DATE3"].ToString().Trim();
                    Val4 = row["INCOME_VAL4"].ToString().Trim();
                    Date4 = row["INCOME_DATE4"].ToString().Trim();
                    Val5 = row["INCOME_VAL5"].ToString().Trim();
                    Date5 = row["INCOME_DATE5"].ToString().Trim();
                    Factor = row["INCOME_FACTOR"].ToString().Trim();
                    Source = row["INCOME_SOURCE"].ToString().Trim();
                    Verifier = row["INCOME_VERIFIER"].ToString().Trim();
                    HowVerified = row["INCOME_HOW_VERIFIED"].ToString().Trim();
                    TotIncome = row["INCOME_TOT_INCOME"].ToString().Trim();
                    ProgIncome = row["INCOME_PROG_INCOME"].ToString().Trim();
                    LstcOperator = row["INCOME_LSTC_OPERATOR"].ToString().Trim();
                    DateLstc = row["INCOME_DATE_LSTC"].ToString().Trim();
                    DateAdd = row["INCOME_DATE_ADD"].ToString().Trim();
                    AddOperator = row["INCOME_ADD_OPERATOR"].ToString().Trim();
                    HrRate1 = row["INCOME_HR_RATE1"].ToString().Trim();
                    HrRate2 = row["INCOME_HR_RATE2"].ToString().Trim();
                    HrRate3 = row["INCOME_HR_RATE3"].ToString().Trim();
                    HrRate4 = row["INCOME_HR_RATE4"].ToString().Trim();
                    HrRate5 = row["INCOME_HR_RATE5"].ToString().Trim();
                    Average = row["INCOME_AVG"].ToString().Trim();
                    VerMethod = row["INCOME_VER_METHOD"].ToString().Trim();
                    if (strType == "I")
                        Name = row["Name"].ToString().Trim();
                }

            }

        }
        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string FamilySeq { get; set; }
        public string Seq { get; set; }
        public string Exclude { get; set; }
        public string Type { get; set; }
        public string Interval { get; set; }
        public string Val1 { get; set; }
        public string Date1 { get; set; }
        public string Val2 { get; set; }
        public string Date2 { get; set; }
        public string Val3 { get; set; }
        public string Date3 { get; set; }
        public string Val4 { get; set; }
        public string Date4 { get; set; }
        public string Val5 { get; set; }
        public string Date5 { get; set; }
        public string Factor { get; set; }
        public string Source { get; set; }
        public string Verifier { get; set; }
        public string HowVerified { get; set; }
        public string TotIncome { get; set; }
        public string ProgIncome { get; set; }
        public string LstcOperator { get; set; }
        public string DateLstc { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Mode { get; set; }
        public string Name { get; set; }
        public string HrRate1 { get; set; }
        public string HrRate2 { get; set; }
        public string HrRate3 { get; set; }
        public string HrRate4 { get; set; }
        public string HrRate5 { get; set; }
        public string Average { get; set; }

        public string VerMethod { get; set; }
        #endregion

    }

    public class CaseDiffEntity
    {
        #region Constructors

        public CaseDiffEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            State = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            Suffix = string.Empty;
            Hn = string.Empty;
            Apt = string.Empty;
            Flr = string.Empty;
            Zip = string.Empty;
            ZipPlus = string.Empty;
            Direction = string.Empty;
            IncareFirst = string.Empty;
            IncareLast = string.Empty;
            County = string.Empty;
            Phone = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            CountyDesc = string.Empty;

        }

        public CaseDiffEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                if (strType == "Landlord")
                {
                    Agency = row["LLR_AGENCY"].ToString().Trim();
                    Dept = row["LLR_DEPT"].ToString().Trim();
                    Program = row["LLR_PROGRAM"].ToString().Trim();
                    Year = row["LLR_YEAR"].ToString().Trim();
                    AppNo = row["LLR_APP_NO"].ToString().Trim();
                    City = row["LLR_CITY"].ToString().Trim();
                    Street = row["LLR_STREET"].ToString().Trim();
                    Suffix = row["LLR_SUFFIX"].ToString().Trim();
                    State = row["LLR_STATE"].ToString().Trim();
                    Hn = row["LLR_HN"].ToString().Trim();
                    Apt = row["LLR_APT"].ToString().Trim();
                    Flr = row["LLR_FLR"].ToString().Trim();
                    Zip = row["LLR_ZIP"].ToString().Trim();
                    ZipPlus = row["LLR_ZIPPLUS"].ToString().Trim();
                    Direction = row["LLR_DIRECTION"].ToString().Trim();
                    IncareFirst = row["LLR_FIRST_NAME"].ToString().Trim();
                    IncareLast = row["LLR_LAST_NAME"].ToString().Trim();
                    County = row["LLR_COUNTY"].ToString().Trim();
                    Phone = row["LLR_PHONE"].ToString().Trim();
                    DateAdd = row["LLR_DATE_ADD"].ToString().Trim();
                    AddOperator = row["LLR_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["LLR_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["LLR_LSTC_OPERATOR"].ToString().Trim();
                    CountyDesc = string.Empty;
                }
                else
                {
                    Agency = row["DIFF_AGENCY"].ToString().Trim();
                    Dept = row["DIFF_DEPT"].ToString().Trim();
                    Program = row["DIFF_PROGRAM"].ToString().Trim();
                    Year = row["DIFF_YEAR"].ToString().Trim();
                    AppNo = row["DIFF_APP_NO"].ToString().Trim();
                    City = row["DIFF_CITY"].ToString().Trim();
                    Street = row["DIFF_STREET"].ToString().Trim();
                    Suffix = row["DIFF_SUFFIX"].ToString().Trim();
                    State = row["DIFF_STATE"].ToString().Trim();
                    Hn = row["DIFF_HN"].ToString().Trim();
                    Apt = row["DIFF_APT"].ToString().Trim();
                    Flr = row["DIFF_FLR"].ToString().Trim();
                    Zip = row["DIFF_ZIP"].ToString().Trim();
                    ZipPlus = row["DIFF_ZIPPLUS"].ToString().Trim();
                    Direction = row["DIFF_DIRECTION"].ToString().Trim();
                    IncareFirst = row["DIFF_INCARE_FIRST"].ToString().Trim();
                    IncareLast = row["DIFF_INCARE_LAST"].ToString().Trim();
                    County = row["DIFF_COUNTY"].ToString().Trim();
                    Phone = row["DIFF_PHONE"].ToString().Trim();
                    DateAdd = row["DIFF_DATE_ADD"].ToString().Trim();
                    AddOperator = row["DIFF_ADD_OPERATOR"].ToString().Trim();
                    DateLstc = row["DIFF_DATE_LSTC"].ToString().Trim();
                    LstcOperator = row["DIFF_LSTC_OPERATOR"].ToString().Trim();
                    CountyDesc = string.Empty;
                }
            }

        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Hn { get; set; }
        public string Apt { get; set; }
        public string Flr { get; set; }
        public string Zip { get; set; }
        public string ZipPlus { get; set; }
        public string Direction { get; set; }
        public string IncareFirst { get; set; }
        public string IncareLast { get; set; }
        public string County { get; set; }
        public string Phone { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string CountyDesc { get; set; }
        public string Mode { get; set; }
        #endregion

    }

    public class CaseMSTSER
    {
        #region Constructors

        public CaseMSTSER()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            Service = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;

        }

        public CaseMSTSER(bool Intialize)
        {
            if (Intialize)// = true;
            {
                Agency = null;
                Dept = null;
                Program = null;
                Year = null;
                AppNo = null;
                Service = null;
                DateAdd = null;
                AddOperator = null;
                DateLstc = null;
                LstcOperator = null;
            }

        }

        public CaseMSTSER(DataRow row)
        {
            if (row != null)
            {
                Agency = row["MSTSER_AGENCY"].ToString().Trim();
                Dept = row["MSTSER_DEPT"].ToString().Trim();
                Program = row["MSTSER_PROGRAM"].ToString().Trim();
                Year = row["MSTSER_YEAR"].ToString().Trim();
                AppNo = row["MSTSER_APP_NO"].ToString().Trim();
                Service = row["MSTSER_SERVICE"].ToString().Trim();
                DateAdd = row["MSTSER_DATE_ADD"].ToString().Trim();
                AddOperator = row["MSTSER_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["MSTSER_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["MSTSER_LSTC_OPERATOR"].ToString().Trim();
            }

        }


        public CaseMSTSER(DataRow row, string strType)
        {
            if (row != null)
            {
                Agency = row["MSTSER_AGENCY"].ToString().Trim();
                Dept = row["MSTSER_DEPT"].ToString().Trim();
                Program = row["MSTSER_PROGRAM"].ToString().Trim();
                Year = row["MSTSER_YEAR"].ToString().Trim();
                AppNo = row["MSTSER_APP_NO"].ToString().Trim();
                Service = row["MSTSER_SERVICE"].ToString().Trim();
                DateAdd = row["MSTSER_DATE_ADD"].ToString().Trim();
                AddOperator = row["MSTSER_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["MSTSER_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["MSTSER_LSTC_OPERATOR"].ToString().Trim();
            }

        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string State { get; set; }
        public string Service { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        #endregion

    }

    public class CaseVerEntity
    {
        #region Constructors

        public CaseVerEntity()
        {

            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            AppNo = string.Empty;
            VerCmi = string.Empty;
            VerHud = string.Empty;
            Verifier = string.Empty;
            VerifyCheckStub = string.Empty;
            VerifyDate = string.Empty;
            VerifyLetter = string.Empty;
            VerifyOthCMB = string.Empty;
            VerifyOther = string.Empty;
            VerifyTaxReturn = string.Empty;
            VerifyW2 = string.Empty;
            VerOmb = string.Empty;
            VerSmi = string.Empty;
            CatElig = string.Empty;
            FundSource = string.Empty;
            MealElig = string.Empty;
            CatElig = string.Empty;
            NoInhh = string.Empty;
            Classification = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            VerifySelfDecl = string.Empty;

        }

        public CaseVerEntity(DataRow row, string strType)
        {
            if (row != null)
            {
                Agency = row["VER_AGENCY"].ToString().Trim();
                Dept = row["VER_DEPT"].ToString().Trim();
                Program = row["VER_PROGRAM"].ToString().Trim();
                Year = row["VER_YEAR"].ToString().Trim();
                AppNo = row["VER_APP_NO"].ToString().Trim();
                VerifyDate = row["VER_VERIFY_DATE"].ToString().Trim();
                Verifier = row["VER_VERIFIER"].ToString().Trim();
                ReverifyDate = row["VER_REVERIFY_DATE"].ToString().Trim();
                VerOmb = row["VER_OMB"].ToString().Trim();
                VerHud = row["VER_HUD"].ToString().Trim();
                VerSmi = row["VER_SMI"].ToString().Trim();
                VerCmi = row["VER_CMI"].ToString().Trim();
                CatElig = row["VER_CAT_ELIG"].ToString().Trim();
                VerifyW2 = row["VER_VERIFY_W2"].ToString().Trim();
                VerifyCheckStub = row["VER_VERIFY_CHECK_STUB"].ToString().Trim();
                VerifyTaxReturn = row["VER_VERIFY_TAX_RETURN"].ToString().Trim();
                VerifyLetter = row["VER_VERIFY_LETTER"].ToString().Trim();
                VerifyOther = row["VER_VERIFY_OTHER"].ToString().Trim();
                IncomeAmount = row["VER_INCOME_AMOUNT"].ToString().Trim();
                NoInhh = row["VER_NO_INHH"].ToString().Trim();
                FundSource = row["VER_FUND_SOURCE"].ToString().Trim();
                MealElig = row["VER_MEAL_ELIG"].ToString().Trim();
                Classification = row["VER_CLASSIFICATION"].ToString().Trim();
                VerifyOthCMB = row["VER_VERIFY_OTH_CMB"].ToString().Trim();
                DateAdd = row["VER_DATE_ADD"].ToString().Trim();
                AddOperator = row["VER_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["VER_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["VER_LSTC_OPERATOR"].ToString().Trim();
                VerifySelfDecl = row["VER_VERIFY_SELF_DECL"].ToString().Trim();
            }

        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        public string VerifyDate { get; set; }
        public string Verifier { get; set; }
        public string ReverifyDate { get; set; }
        public string VerOmb { get; set; }
        public string VerHud { get; set; }
        public string VerSmi { get; set; }
        public string VerCmi { get; set; }
        public string CatElig { get; set; }
        public string VerifyW2 { get; set; }
        public string VerifyCheckStub { get; set; }
        public string VerifyTaxReturn { get; set; }
        public string VerifyLetter { get; set; }
        public string VerifyOther { get; set; }
        // murali added on 10/09/2020
        public string VerifySelfDecl { get; set; }
        public string IncomeAmount { get; set; }
        public string NoInhh { get; set; }
        public string FundSource { get; set; }
        public string MealElig { get; set; }
        public string Classification { get; set; }
        public string VerifyOthCMB { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string MstModify { get; set; }
        #endregion

    }


    public class MembersEntity
    {

        #region Constructors

        public MembersEntity()
        {
            Agy =
            Dept =
            Prog =
            Year =
            AppNo =
            //App_Or_Mem =
            Ssn =
            Fname =
            Lname =
            //Mname =
            Relation =
            //ClientID =
            Phone =
            //Hno =
            //Street =
            //City =
            //State =
            //Zip =
            //CaseType =
            //Snp_Key =
            FamSeq =
            //Mem_Status =
            DOB =
            //LSTC_Date =
            //SNP_EXCLUDE =
            Site = string.Empty;
            Age = string.Empty;
            //Wrk_Phone = string.Empty;
            //Occupation = string.Empty;
            //Education = string.Empty;

            //Language = string.Empty;
            //LanguageOt = string.Empty;
            //Elig_Date = string.Empty;
            Race = string.Empty;
            Ethinic = string.Empty;
            Sex = string.Empty;
            //Classfication = string.Empty;
            //MealElig = string.Empty;
            //Rank1 = string.Empty;
            //Rank2 = string.Empty;
            //Rank3 = string.Empty;
            //Rank4 = string.Empty;
            //Rank5 = string.Empty;
            //Rank6 = string.Empty;
            Empl_Phone = string.Empty;
            Fam_Income = string.Empty;
            //Income_Exclude = string.Empty;
            //IncomeType = string.Empty;
            //Race_Desc = string.Empty;
            //Ethnic_Desc = string.Empty;
            //Lang_Desc = string.Empty;
            //LangOth_Desc = string.Empty;
            //Education_Desc = string.Empty;
            Agy_2 = string.Empty;
            Disable = string.Empty;
            SSN_Reason = string.Empty;
        }

        public MembersEntity(bool Intialize)
        {
            if (Intialize)
            {
                Agy =
                Dept =
                Prog =
                Year =
                AppNo =
                //App_Or_Mem =
                Ssn =
                Fname =
                Lname =
                //Mname =
                Relation =
                //ClientID =
                Phone =
                //Hno =
                //Street =
                //City =
                //State =
                //Zip =
                //CaseType =
                //Snp_Key =
                FamSeq =
                //Mem_Status =
                DOB =
                //LSTC_Date =
                //SNP_EXCLUDE =
                Site = null;
                Age = null;
                //Wrk_Phone = null;
                //Occupation = null;
                //Education = null;


                //Language = null;
                //LanguageOt = null;
                //Elig_Date = null;
                Race = null;
                Ethinic = null;
                Sex = null;
                //Classfication = null;
                //MealElig = null;
                //Rank1 = null;
                //Rank2 = null;
                //Rank3 = null;
                //Rank4 = null;
                //Rank5 = null;
                //Rank6 = null;
                Empl_Phone = null;
                Fam_Income = null;
                //Income_Exclude=null;
                //IncomeType = null;
                //Race_Desc = null;
                //Ethnic_Desc = null;
                //Lang_Desc = null;
                //LangOth_Desc = null;
                //Education_Desc = null;
                Agy_2 = null;
                Disable = null;
                SSN_Reason = null;
                Status = null;
            }


        }

        public MembersEntity(MembersEntity Entity)
        {
            if (Entity != null)
            {
                Agy = Entity.Agy;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                AppNo = Entity.AppNo;
                //App_Or_Mem = Entity.App_Or_Mem;
                Ssn = Entity.Ssn;
                Fname = Entity.Fname;
                Lname = Entity.Lname;
                //Mname = Entity.Mname;
                Relation = Entity.Relation;
                //ClientID = Entity.ClientID;
                Phone = Entity.Phone;
                //Hno = Entity.Hno;
                //Street = Entity.Street;
                //City = Entity.City;
                //State = Entity.State;
                //Zip = Entity.Zip;
                //CaseType = Entity.CaseType;
                //Snp_Key = Entity.Snp_Key;
                FamSeq = Entity.FamSeq;
                //Mem_Status = Entity.Mem_Status;
                DOB = Entity.DOB;
                //LSTC_Date = Entity.LSTC_Date;
                //SNP_EXCLUDE = Entity.SNP_EXCLUDE;
                Site = Entity.Site;
                Age = Entity.Age;
                //Wrk_Phone = Entity.Wrk_Phone;
                //Occupation = Entity.Occupation;
                //Education = Entity.Education;

                //Language = Entity.Language;
                //LanguageOt = Entity.LanguageOt;
                //Elig_Date = Entity.Elig_Date;
                Race = Entity.Race;
                Ethinic = Entity.Ethinic;
                Sex = Entity.Sex;
                //Classfication = Entity.Classfication;
                //MealElig = Entity.MealElig;
                //Rank1 = Entity.Rank1;
                //Rank2 = Entity.Rank2;
                //Rank3 = Entity.Rank3;
                //Rank4 = Entity.Rank4;
                //Rank5 = Entity.Rank5;
                //Rank6 = Entity.Rank6;
                Empl_Phone = Entity.Empl_Phone;
                Fam_Income = Entity.Fam_Income;
                //Income_Exclude = Entity.Income_Exclude;
                //IncomeType = Entity.IncomeType;
                //Race_Desc = Entity.Race_Desc;
                //Ethnic_Desc = Entity.Ethnic_Desc;
                //Lang_Desc = Entity.Lang_Desc;
                //LangOth_Desc = Entity.LangOth_Desc;
                //Education_Desc = Entity.Education_Desc;
                Agy_2 = Entity.Agy_2;
                Disable = Entity.Disable;
                SSN_Reason = Entity.SSN_Reason;
                Status = Entity.Status;
            }
        }


        public MembersEntity(DataRow row)
        {
            if (row != null)
            {
                //Agy = row["Agency"].ToString().Trim();
                //Dept = row["Dept"].ToString().Trim();
                //Prog = row["Prog"].ToString().Trim();
                //Year = row["SnpYear"].ToString().Trim();
                //AppNo = row["AppNo"].ToString().Trim().Substring(0, 8);
                //App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                //Ssn = row["Ssn"].ToString().Trim();
                //Fname = row["Fname"].ToString().Trim();
                //Lname = row["Lname"].ToString().Trim();
                //Mname = row["Mname"].ToString().Trim();
                //Relation = row["Mem_Code"].ToString().Trim();
                //ClientID = row["ClientID"].ToString().Trim();
                //Phone = row["Phone"].ToString().Trim();
                //Hno = row["Hno"].ToString().Trim();
                //Street = row["Street"].ToString().Trim();
                //City = row["City"].ToString().Trim();
                //State = row["State1"].ToString().Trim();
                //Zip = row["Zip"].ToString().Trim();
                //CaseType = row["CaseType"].ToString().Trim();
                //Snp_Key = row["RecKey"].ToString().Trim();
                //FamSeq = row["RecFamSeq"].ToString().Trim();

                //Mem_Status = row["AppStatus"].ToString().Trim();
                //DOB = row["DOB"].ToString().Trim();
                //LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                //SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                //Site = row["MST_SITE"].ToString().Trim();
                //Age = row["SNP_AGE"].ToString().Trim();
                //Wrk_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                //Occupation = row["SNP_EMPLOYER_NAME"].ToString().Trim();
                //Education = row["SNP_EDUCATION"].ToString().Trim();

                Agy = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
                Year = row["SnpYear"].ToString().Trim();
                AppNo = row["SNP_APP"].ToString().Trim();
                //App_Or_Mem = row["App_Mem_SW"].ToString().Trim();
                Ssn = row["Ssn"].ToString().Trim();
                Fname = row["Fname"].ToString().Trim();
                Lname = row["Lname"].ToString().Trim();
                //Mname = row["Mname"].ToString().Trim();
                Relation = row["Mem_Code"].ToString().Trim();
                //ClientID = row["ClientID"].ToString().Trim();
                Phone = row["Phone"].ToString().Trim();
                //Hno = row["Hno"].ToString().Trim();
                //Street = row["Street"].ToString().Trim();
                //City = row["City"].ToString().Trim();
                //State = row["State1"].ToString().Trim();
                //Zip = row["Zip"].ToString().Trim();
                //CaseType = row["CaseType"].ToString().Trim();
                //Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["RecFamSeq"].ToString().Trim();

                //Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["DOB"].ToString().Trim();
                //LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                //SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                //Wrk_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                //Occupation = row["SNP_EMPLOYER_NAME"].ToString().Trim();
                //Education = row["SNP_EDUCATION"].ToString().Trim();

                //Language = row["MST_LANGUAGE"].ToString().Trim();
                //LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                //Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                Ethinic = row["SNP_ETHNIC"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                //Classfication = row["MST_CLASSIFICATION"].ToString().Trim();
                //MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                //ChldMST_Repeat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                //Rank1 = row["MST_RANK1"].ToString().Trim();
                //Rank2 = row["MST_RANK2"].ToString().Trim();
                //Rank3 = row["MST_RANK3"].ToString().Trim();
                //Rank4 = row["MST_RANK4"].ToString().Trim();
                //Rank5 = row["MST_RANK5"].ToString().Trim();
                //Rank6 = row["MST_RANK6"].ToString().Trim();
                Empl_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                Fam_Income = row["MST_FAM_INCOME"].ToString().Trim();
                //Income_Exclude = row["INCOME_EXCLUDE"].ToString().Trim();
                //IncomeType = row["INCOME_TYPE"].ToString().Trim();
                //Race_Desc = row["RACE_DESC"].ToString().Trim();
                //Ethnic_Desc = row["ETHNIC_DESC"].ToString().Trim();
                //Lang_Desc = row["LANG_DESC"].ToString().Trim();
                //LangOth_Desc = row["LANGOTH_DESC"].ToString().Trim();
                //Education_Desc = row["EDUCATION_DESC"].ToString().Trim();
                Agy_2 = row["AGY_2"].ToString().Trim();
            }
        }

        public MembersEntity(DataRow row, string Column_Name)
        {
            if (Column_Name == "Original")
            {
                Agy = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Prog = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                AppNo = row["MST_APP_NO"].ToString().Trim().Substring(0, 8);
                //App_Or_Mem = row["AppNo"].ToString().Trim().Substring(10, 1);
                Ssn = row["SNP_SSNO"].ToString().Trim();
                Fname = row["SNP_NAME_IX_FI"].ToString().Trim();
                Lname = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //Mname = row["SNP_NAME_IX_MI"].ToString().Trim();
                Relation = row["SNP_MEMBER_CODE"].ToString().Trim();
                //ClientID = row["SNP_CLIENT_ID"].ToString().Trim();
                Phone = row["MST_AREA"].ToString().Trim() + "-" + row["MST_PHONE"].ToString().Trim();
                //Hno = row["MST_HN"].ToString().Trim();
                //Street = row["MST_STREET"].ToString().Trim();
                //City = row["MST_CITY"].ToString().Trim();
                //State = row["MST_STATE"].ToString().Trim();
                //Zip = row["MST_ZIP"].ToString().Trim();
                //CaseType = row["MST_CASE_TYPE"].ToString().Trim();
                //Snp_Key = row["RecKey"].ToString().Trim();
                FamSeq = row["SNP_FAMILY_SEQ"].ToString().Trim();

                //Mem_Status = row["AppStatus"].ToString().Trim();
                DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                //DOB_NA = row["SNP_DOB_NA"].ToString().Trim();
                //LSTC_Date = row["SNP_DATE_LSTC"].ToString().Trim();
                //SNP_EXCLUDE = row["SNP_EXCLUDE"].ToString().Trim();
                Site = row["MST_SITE"].ToString().Trim();
                Age = row["SNP_AGE"].ToString().Trim();
                //Wrk_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                //Occupation = row["SNP_EMPLOYER_NAME"].ToString().Trim();
                //Education = row["SNP_EDUCATION"].ToString().Trim();

                //Language = row["MST_LANGUAGE"].ToString().Trim();
                //LanguageOt = row["MST_LANGUAGE_OT"].ToString().Trim();
                //Elig_Date = row["MST_ELIG_DATE"].ToString().Trim();
                Race = row["SNP_RACE"].ToString().Trim();
                Ethinic = row["SNP_ETHNIC"].ToString().Trim();
                Sex = row["SNP_SEX"].ToString().Trim();
                //Classfication = row["MST_CLASSIFICATION"].ToString().Trim();
                //MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                //ChldMST_Repeat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                //Rank1 = row["MST_RANK1"].ToString().Trim();
                //Rank2 = row["MST_RANK2"].ToString().Trim();
                //Rank3 = row["MST_RANK3"].ToString().Trim();
                //Rank4 = row["MST_RANK4"].ToString().Trim();
                //Rank5 = row["MST_RANK5"].ToString().Trim();
                //Rank6 = row["MST_RANK6"].ToString().Trim();
                Empl_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
                Fam_Income = row["MST_FAM_INCOME"].ToString().Trim();
                //Income_Exclude = row["INCOME_EXCLUDE"].ToString().Trim();
                //IncomeType = row["INCOME_TYPE"].ToString().Trim();
                //Race_Desc = row["RACE_DESC"].ToString().Trim();
                //Ethnic_Desc = row["ETHNIC_DESC"].ToString().Trim();
                //Lang_Desc = row["LANG_DESC"].ToString().Trim();
                //LangOth_Desc = row["LANGOTH_DESC"].ToString().Trim();
                //Education_Desc = row["EDUCATION_DESC"].ToString().Trim();
                Agy_2 = row["AGY_2"].ToString().Trim();
            }
        }

        public MembersEntity(DataRow row, string Column_Name, string Report)
        {

            Agy = row["Agency"].ToString().Trim();
            Dept = row["Dept"].ToString().Trim();
            Prog = row["Prog"].ToString().Trim();
            Year = row["SnpYear"].ToString().Trim();
            AppNo = row["SNP_APP"].ToString().Trim();
            Ssn = row["Ssn"].ToString().Trim();
            Fname = row["Fname"].ToString().Trim();
            Lname = row["Lname"].ToString().Trim();
            Relation = row["Mem_Code"].ToString().Trim();
            Phone = row["Phone"].ToString().Trim();
            FamSeq = row["RecFamSeq"].ToString().Trim();

            DOB = row["DOB"].ToString().Trim();
            Site = row["MST_SITE"].ToString().Trim();
            Age = row["SNP_AGE"].ToString().Trim();
            Race = row["SNP_RACE"].ToString().Trim();
            Ethinic = row["SNP_ETHNIC"].ToString().Trim();
            Sex = row["SNP_SEX"].ToString().Trim();
            Empl_Phone = row["SNP_EMPL_PHONE"].ToString().Trim();
            Fam_Income = row["MST_FAM_INCOME"].ToString().Trim();
            Agy_2 = row["AGY_2"].ToString().Trim();
            Disable = row["SNP_DISABLE"].ToString().Trim();
            SSN_Reason = row["SNP_SSN_REASON"].ToString().Trim();
            Status = row["SNP_STATUS"].ToString().Trim();
        }

        #endregion

        #region Properties
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string AppNo { get; set; }
        //public string App_Or_Mem { get; set; }
        public string Ssn { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        //public string Mname { get; set; }
        public string Relation { get; set; }
        //public string ClientID { get; set; }
        public string Phone { get; set; }
        //public string Hno { get; set; }
        //public string Street { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Zip { get; set; }
        //public string CaseType { get; set; }
        //public string Snp_Key { get; set; }

        public string FamSeq { get; set; }
        //public string Mem_Status { get; set; }
        public string DOB { get; set; }
        //public string DOB_NA { get; set; }
        //public string LSTC_Date { get; set; }
        //public string SNP_EXCLUDE { get; set; }
        public string Site { get; set; }
        public string Age { get; set; }
        //public string Wrk_Phone { get; set; }
        //public string Occupation { get; set; }
        //public string Education { get; set; }

        //public string Language { get; set; }
        //public string LanguageOt { get; set; }
        //public string Elig_Date { get; set; }
        public string Race { get; set; }
        public string Ethinic { get; set; }
        public string Sex { get; set; }
        //public string Classfication { get; set; }
        //public string MealElig { get; set; }
        //public string Rank1 { get; set; }
        //public string Rank2 { get; set; }
        //public string Rank3 { get; set; }
        //public string Rank4 { get; set; }
        //public string Rank5 { get; set; }
        //public string Rank6 { get; set; }
        public string Empl_Phone { get; set; }
        public string Fam_Income { get; set; }
        //public string Income_Exclude { get; set; }
        //public string IncomeType { get; set; }
        //public string Race_Desc { get; set; }
        //public string Ethnic_Desc { get; set; }
        //public string Lang_Desc { get; set; }
        //public string LangOth_Desc { get; set; }
        //public string Education_Desc { get; set; }
        public string Agy_2 { get; set; }
        public string Disable { get; set; }
        public string SSN_Reason { get; set; }
        public string Status { get; set; }

        #endregion

    }

    //public class IMGUPLOGEntity
    //{

    //    #region Constructors

    //    public IMGUPLOGEntity()
    //    {
    //        IMGLOG_USERID = string.Empty;
    //        IMGLOG_APP = string.Empty;
    //        IMGLOG_SCREEN = string.Empty;
    //        IMGLOG_OPERATN = string.Empty;
    //        IMGLOG_FNAME = string.Empty;
    //        IMGLOG_ULoadAs = string.Empty;
    //        IMGLOG_TrnDate = string.Empty;

    //    }

    //    public IMGUPLOGEntity(DataRow row)
    //    {
    //        if (row != null)
    //        {
    //            IMGLOG_USERID = row["IMGLOG_USERID"].ToString().Trim();
    //            IMGLOG_APP = row["IMGLOG_APP"].ToString().Trim();
    //            IMGLOG_SCREEN = row["IMGLOG_SCREEN"].ToString().Trim();
    //            IMGLOG_OPERATN = row["IMGLOG_OPERATN"].ToString().Trim();
    //            IMGLOG_FNAME = row["IMGLOG_FNAME"].ToString().Trim();
    //            IMGLOG_ULoadAs = row["IMGLOG_ULoadAs"].ToString().Trim();
    //            IMGLOG_TrnDate = row["IMGLOG_TrnDate"].ToString().Trim();

    //        }
    //    }

    //    #endregion

    //    #region Properties
    //    public string IMGLOG_USERID { get; set; }
    //    public string IMGLOG_APP { get; set; }
    //    public string IMGLOG_SCREEN { get; set; }
    //    public string IMGLOG_OPERATN { get; set; }
    //    public string IMGLOG_FNAME { get; set; }
    //    public string IMGLOG_ULoadAs { get; set; }
    //    public string IMGLOG_TrnDate { get; set; }

    //    #endregion

    //}

    public class IMGUPLOGNEntity
    {

        #region Constructors

        public IMGUPLOGNEntity()
        {
            IMGLOG_ID = string.Empty;
            IMGLOG_AGY = string.Empty;
            IMGLOG_DEP = string.Empty;
            IMGLOG_PROG = string.Empty;
            IMGLOG_YEAR = string.Empty;
            IMGLOG_APP = string.Empty;
            IMGLOG_FAMILY_SEQ = string.Empty;
            IMGLOG_SCREEN = string.Empty;            
            IMGLOG_SECURITY = string.Empty;
            IMGLOG_TYPE = string.Empty;
            IMGLOG_UPLoadAs = string.Empty;
            IMGLOG_UPLOAD_BY = string.Empty;
            IMGLOG_DATE_UPLOAD = string.Empty;
            IMGLOG_ORIG_FILENAME = string.Empty;
            IMGLOG_DELETED_BY = string.Empty;
            IMGLOG_DATE_DELETED = string.Empty;
            IMGLOG_DELETED_FROM = string.Empty;
            IMGLOG_LSTC_OPERATOR = string.Empty; 
            IMGLOG_DATE_LSTC = string.Empty; 

        }

        public IMGUPLOGNEntity(DataRow row)
        {
            if (row != null)
            {
                IMGLOG_ID = row["IMGLOG_ID"].ToString().Trim();
                IMGLOG_AGY = row["IMGLOG_AGY"].ToString().Trim();
                IMGLOG_DEP = row["IMGLOG_DEP"].ToString().Trim();
                IMGLOG_PROG = row["IMGLOG_PROG"].ToString().Trim();
                IMGLOG_YEAR = row["IMGLOG_YEAR"].ToString().Trim();
                IMGLOG_APP = row["IMGLOG_APP"].ToString().Trim();
                IMGLOG_FAMILY_SEQ = row["IMGLOG_FAMILY_SEQ"].ToString().Trim();
                IMGLOG_SCREEN = row["IMGLOG_SCREEN"].ToString().Trim();                
                IMGLOG_SECURITY = row["IMGLOG_SECURITY"].ToString().Trim();
                IMGLOG_TYPE = row["IMGLOG_TYPE"].ToString().Trim();
                IMGLOG_UPLoadAs = row["IMGLOG_UPLoadAs"].ToString().Trim();
                IMGLOG_UPLOAD_BY = row["IMGLOG_UPLOAD_BY"].ToString().Trim();
                IMGLOG_DATE_UPLOAD = row["IMGLOG_DATE_UPLOAD"].ToString().Trim();
                IMGLOG_ORIG_FILENAME = row["IMGLOG_ORIG_FILENAME"].ToString().Trim();
                IMGLOG_DELETED_BY = row["IMGLOG_DELETED_BY"].ToString().Trim();
                IMGLOG_DATE_DELETED = row["IMGLOG_DATE_DELETED"].ToString().Trim();
                IMGLOG_DELETED_FROM = row["IMGLOG_DELETED_FROM"].ToString().Trim();
                IMGLOG_LSTC_OPERATOR = row["IMGLOG_LSTC_OPERATOR"].ToString().Trim();
                IMGLOG_DATE_LSTC = row["IMGLOG_DATE_LSTC"].ToString().Trim();

            }
        }

        #endregion

        #region Properties
        //public string IMGLOG_USERID { get; set; }
        //public string IMGLOG_APP { get; set; }
        //public string IMGLOG_SCREEN { get; set; }
        //public string IMGLOG_OPERATN { get; set; }
        //public string IMGLOG_FNAME { get; set; }
        //public string IMGLOG_ULoadAs { get; set; }
        //public string IMGLOG_TrnDate { get; set; }
        public string IMGLOG_ID { get; set; }
        public string IMGLOG_AGY { get; set; }
        public string IMGLOG_DEP { get; set; }
        public string IMGLOG_PROG { get; set; }
        public string IMGLOG_YEAR { get; set; }
        public string IMGLOG_APP { get; set; }
        public string IMGLOG_FAMILY_SEQ { get; set; }
        public string IMGLOG_SCREEN { get; set; }        
        public string IMGLOG_SECURITY { get; set; }
        public string IMGLOG_TYPE { get; set; }
        public string IMGLOG_UPLoadAs { get; set; }
        public string IMGLOG_UPLOAD_BY { get; set; }
        public string IMGLOG_DATE_UPLOAD { get; set; }
        public string IMGLOG_DELETED_BY { get; set; }
        public string IMGLOG_DATE_DELETED { get; set; }
        public string IMGLOG_DELETED_FROM { get; set; }        
        public string IMGLOG_ORIG_FILENAME { get; set; }
        public DateTime  IMGLOG_DATE_UPLOAD_Dt { get; set; }
        public string IMGLOG_DATE_LSTC { get; set; }
        public string IMGLOG_LSTC_OPERATOR { get; set; }
        public string MODE { get; set; }

        #endregion

    }

    public class INVDOCLOGEntity
    {

        #region Constructors

        public INVDOCLOGEntity()
        {
            INVDOC_ID = string.Empty;
            INVDOC_AGENCY = string.Empty;
            INVDOC_DEPT = string.Empty;
            INVDOC_PROGRAM = string.Empty;
            INVDOC_YEAR = string.Empty;
            INVDOC_APP = string.Empty;
            INVDOC_SERVICEPLAN = string.Empty;
            INVDOC_SPM_SEQ = string.Empty;
            INVDOC_VEND_TYPE = string.Empty;
            INVDOC_SEQ = string.Empty;
            INVDOC_SERVICE = string.Empty;
            INVDOC_VENDOR = string.Empty;
            INVDOC_UPLOAD_AS = string.Empty;
            INVDOC_UPLOAD_BY = string.Empty;
            INVDOC_DATE_UPLOAD = string.Empty;
            INVDOC_ORIG_NAME = string.Empty;
            INVDOC_DELETED_BY = string.Empty;
            INVDOC_DATE_DELETED = string.Empty;

            INVDOC_LSTC_OPERATOR = string.Empty;
            INVDOC_DATE_LSTC = string.Empty;
            INVDOC_ACTID = string.Empty;
            INVDOC_BUNDLE = string.Empty;
        }

        public INVDOCLOGEntity(DataRow row)
        {
            if (row != null)
            {
                INVDOC_ID = row["INVDOC_ID"].ToString().Trim();
                INVDOC_AGENCY = row["INVDOC_AGENCY"].ToString().Trim();
                INVDOC_DEPT = row["INVDOC_DEPT"].ToString().Trim();
                INVDOC_PROGRAM = row["INVDOC_PROGRAM"].ToString().Trim();
                INVDOC_YEAR = row["INVDOC_YEAR"].ToString().Trim();
                INVDOC_APP = row["INVDOC_APP"].ToString().Trim();
                INVDOC_SERVICEPLAN = row["INVDOC_SERVICEPLAN"].ToString().Trim();
                INVDOC_SPM_SEQ = row["INVDOC_SPM_SEQ"].ToString().Trim();
                INVDOC_VEND_TYPE = row["INVDOC_VEND_TYPE"].ToString().Trim();
                INVDOC_SEQ = row["INVDOC_SEQ"].ToString().Trim();
                INVDOC_SERVICE = row["INVDOC_SERVICE"].ToString().Trim();
                INVDOC_VENDOR = row["INVDOC_VENDOR"].ToString().Trim();
                INVDOC_UPLOAD_AS = row["INVDOC_UPLOAD_AS"].ToString().Trim();
                INVDOC_UPLOAD_BY = row["INVDOC_UPLOAD_BY"].ToString().Trim();
                INVDOC_DATE_UPLOAD = row["INVDOC_DATE_UPLOAD"].ToString().Trim();
                INVDOC_ORIG_NAME = row["INVDOC_ORIG_NAME"].ToString().Trim();
                INVDOC_DELETED_BY = row["INVDOC_DELETED_BY"].ToString().Trim();
                INVDOC_DATE_DELETED = row["INVDOC_DATE_DELETED"].ToString().Trim();

                INVDOC_LSTC_OPERATOR = row["INVDOC_LSTC_OPERATOR"].ToString().Trim();
                INVDOC_DATE_LSTC = row["INVDOC_DATE_LSTC"].ToString().Trim();
                INVDOC_ACTID = row["INVDOC_ACTID"].ToString().Trim();
                INVDOC_BUNDLE = row["INVDOC_BUNDLE"].ToString().Trim();
            }
        }

        #endregion

        #region Properties
        //public string IMGLOG_USERID { get; set; }
        //public string IMGLOG_APP { get; set; }
        //public string IMGLOG_SCREEN { get; set; }
        //public string IMGLOG_OPERATN { get; set; }
        //public string IMGLOG_FNAME { get; set; }
        //public string IMGLOG_ULoadAs { get; set; }
        //public string IMGLOG_TrnDate { get; set; }
        public string INVDOC_ID { get; set; }
        public string INVDOC_AGENCY { get; set; }
        public string INVDOC_DEPT { get; set; }
        public string INVDOC_PROGRAM { get; set; }
        public string INVDOC_YEAR { get; set; }
        public string INVDOC_APP { get; set; }
        public string INVDOC_SERVICEPLAN { get; set; }
        public string INVDOC_SPM_SEQ { get; set; }
        public string INVDOC_VEND_TYPE { get; set; }
        public string INVDOC_SEQ { get; set; }
        public string INVDOC_SERVICE { get; set; }
        public string INVDOC_VENDOR { get; set; }
        public string INVDOC_UPLOAD_AS { get; set; }
        public string INVDOC_UPLOAD_BY { get; set; }
        public string INVDOC_DATE_UPLOAD { get; set; }
        public string INVDOC_DELETED_BY { get; set; }
        public string INVDOC_DATE_DELETED { get; set; }
       // public string IMGLOG_DELETED_FROM { get; set; }
        public string INVDOC_ORIG_NAME { get; set; }
       
        public string INVDOC_ACTID { get; set; }
        public string INVDOC_BUNDLE { get; set; }
        public string INVDOC_DATE_LSTC { get; set; }
        public string INVDOC_LSTC_OPERATOR { get; set; }
        public string MODE { get; set; }

        #endregion

    }


    public class RNGCOUNTSEnitity
    {
        #region Constructors

        public RNGCOUNTSEnitity()
        {
            RNGC_AGENCY = string.Empty;
            RNGC_DEPT = string.Empty;
            RNGC_PROGRAM = string.Empty;
            RNGC_YEAR = string.Empty;
            RNGC_APP = string.Empty;
            RNGC_ELIG_DATE = string.Empty;
            RNGC_MST_FAMILY_SEQ = string.Empty;
            RNGC_SNP_FAMILY_SEQ = string.Empty;
            RNGC_FAMILY_ID = string.Empty;
            RNGC_MEMBER_CODE = string.Empty;
            RNGC_CLIENT_ID = string.Empty;
            RNGC_NAME_IX_FI = string.Empty;
            RNGC_NAME_IX_LAST = string.Empty;
            RNGC_NAME_IX_MI = string.Empty;
            RNGC_FAMILY_TYPE = string.Empty;
            RNGC_NO_INPROG = string.Empty;
            RNGC_HOUSING = string.Empty;
            RNGC_POVERTY = string.Empty;
            RNCG_COUNTY = string.Empty;
            RNGC_CASE_TYPE = string.Empty;
            RNGC_ACTIVE_STATUS = string.Empty;
            RNGC_SITE = string.Empty;
            RNGC_ZIP = string.Empty;
            RNGC_ZIPPLUS = string.Empty;
            RNGC_INTAKE_WORKER = string.Empty;
            RNGC_SECRET = string.Empty;
            RNGC_INCOME_TYPES = string.Empty;
            RNGC_NCASHBEN = string.Empty;
            RNGC_ALT_BDATE = string.Empty;
            RNGC_SEX = string.Empty;
            RNGC_AGE = string.Empty;
            RNGC_ETHNIC = string.Empty;
            RNGC_EDUCATION = string.Empty;
            RNGC_HEALTH_INS = string.Empty;
            RNGC_HEALTH_CODES = string.Empty;
            RNGC_VET = string.Empty;
            RNGC_MILITARY_STATUS = string.Empty;
            RNGC_DISABLE = string.Empty;
            RNGC_FOOD_STAMPS = string.Empty;
            RNGC_FARMER = string.Empty;
            RNGC_WORK_STAT = string.Empty;
            RNGC_EMPLOYED = string.Empty;
            RNGC_DOB_NA = string.Empty;
            RNGC_RACE = string.Empty;
            Mode = string.Empty;

        }

        public RNGCOUNTSEnitity(DataRow row)
        {
            if (row != null)
            {
                RNGC_AGENCY = row["RNGC_AGENCY"].ToString().Trim();
                RNGC_DEPT = row["RNGC_DEPT"].ToString().Trim();
                RNGC_PROGRAM = row["RNGC_PROGRAM"].ToString().Trim();
                RNGC_YEAR = row["RNGC_YEAR"].ToString().Trim();
                RNGC_APP = row["RNGC_APP"].ToString().Trim();
                RNGC_ELIG_DATE = row["RNGC_ELIG_DATE"].ToString().Trim();
                RNGC_MST_FAMILY_SEQ = row["RNGC_MST_FAMILY_SEQ"].ToString().Trim();
                RNGC_SNP_FAMILY_SEQ = row["RNGC_SNP_FAMILY_SEQ"].ToString().Trim();
                RNGC_FAMILY_ID = row["RNGC_FAMILY_ID"].ToString().Trim();
                RNGC_MEMBER_CODE = row["RNGC_MEMBER_CODE"].ToString().Trim();
                RNGC_CLIENT_ID = row["RNGC_CLIENT_ID"].ToString().Trim();
                RNGC_NAME_IX_FI = row["RNGC_NAME_IX_FI"].ToString().Trim();
                RNGC_FAMILY_TYPE = row["RNGC_FAMILY_TYPE"].ToString().Trim();
                RNGC_NO_INPROG = row["RNGC_NO_INPROG"].ToString().Trim();
                RNGC_HOUSING = row["RNGC_HOUSING"].ToString().Trim();
                RNGC_POVERTY = row["RNGC_POVERTY"].ToString().Trim();
                RNCG_COUNTY = row["RNCG_COUNTY"].ToString().Trim();
                RNGC_CASE_TYPE = row["RNGC_CASE_TYPE"].ToString().Trim();
                RNGC_ACTIVE_STATUS = row["RNGC_ACTIVE_STATUS"].ToString().Trim();
                RNGC_SITE = row["RNGC_SITE"].ToString().Trim();
                RNGC_ZIP = row["RNGC_ZIP"].ToString().Trim();
                RNGC_ZIPPLUS = row["RNGC_ZIPPLUS"].ToString().Trim();
                RNGC_INTAKE_WORKER = row["RNGC_INTAKE_WORKER"].ToString().Trim();
                RNGC_SECRET = row["RNGC_SECRET"].ToString().Trim();
                RNGC_INCOME_TYPES = row["RNGC_INCOME_TYPES"].ToString().Trim();
                RNGC_NCASHBEN = row["RNGC_NCASHBEN"].ToString().Trim();
                RNGC_ALT_BDATE = row["RNGC_ALT_BDATE"].ToString().Trim();
                RNGC_SEX = row["RNGC_SEX"].ToString().Trim();
                RNGC_AGE = row["RNGC_AGE"].ToString().Trim();
                RNGC_ETHNIC = row["RNGC_ETHNIC"].ToString().Trim();
                RNGC_EDUCATION = row["RNGC_EDUCATION"].ToString().Trim();
                RNGC_HEALTH_INS = row["RNGC_HEALTH_INS"].ToString().Trim();
                RNGC_HEALTH_CODES = row["RNGC_HEALTH_CODES"].ToString().Trim();
                RNGC_VET = row["RNGC_VET"].ToString().Trim();
                RNGC_MILITARY_STATUS = row["RNGC_MILITARY_STATUS"].ToString().Trim();
                RNGC_DISABLE = row["RNGC_DISABLE"].ToString().Trim();
                RNGC_FOOD_STAMPS = row["RNGC_FOOD_STAMPS"].ToString().Trim();
                RNGC_FARMER = row["RNGC_FARMER"].ToString().Trim();
                RNGC_WORK_STAT = row["RNGC_WORK_STAT"].ToString().Trim();
                RNGC_EMPLOYED = row["RNGC_EMPLOYED"].ToString().Trim();
                RNGC_DOB_NA = row["RNGC_DOB_NA"].ToString().Trim();
                RNGC_RACE = row["RNGC_RACE"].ToString().Trim();
                RNGC_NAME_IX_LAST = row["RNGC_NAME_IX_LAST"].ToString().Trim();
                RNGC_NAME_IX_MI = row["RNGC_NAME_IX_MI"].ToString().Trim();

            }
        }

        #endregion

        #region Properties


        public string RNGC_AGENCY { get; set; }
        public string RNGC_DEPT { get; set; }
        public string RNGC_PROGRAM { get; set; }
        public string RNGC_YEAR { get; set; }
        public string RNGC_APP { get; set; }
        public string RNGC_ELIG_DATE { get; set; }
        public string RNGC_MST_FAMILY_SEQ { get; set; }
        public string RNGC_SNP_FAMILY_SEQ { get; set; }
        public string RNGC_FAMILY_ID { get; set; }
        public string RNGC_MEMBER_CODE { get; set; }
        public string RNGC_CLIENT_ID { get; set; }
        public string RNGC_NAME_IX_FI { get; set; }
        public string RNGC_FAMILY_TYPE { get; set; }
        public string RNGC_NO_INPROG { get; set; }
        public string RNGC_HOUSING { get; set; }
        public string RNGC_POVERTY { get; set; }
        public string RNCG_COUNTY { get; set; }
        public string RNGC_CASE_TYPE { get; set; }
        public string RNGC_ACTIVE_STATUS { get; set; }
        public string RNGC_SITE { get; set; }
        public string RNGC_ZIP { get; set; }
        public string RNGC_ZIPPLUS { get; set; }
        public string RNGC_INTAKE_WORKER { get; set; }
        public string RNGC_SECRET { get; set; }
        public string RNGC_INCOME_TYPES { get; set; }
        public string RNGC_NCASHBEN { get; set; }
        public string RNGC_ALT_BDATE { get; set; }
        public string RNGC_SEX { get; set; }
        public string RNGC_AGE { get; set; }
        public string RNGC_ETHNIC { get; set; }
        public string RNGC_EDUCATION { get; set; }
        public string RNGC_HEALTH_INS { get; set; }
        public string RNGC_HEALTH_CODES { get; set; }
        public string RNGC_VET { get; set; }
        public string RNGC_MILITARY_STATUS { get; set; }
        public string RNGC_DISABLE { get; set; }
        public string RNGC_FOOD_STAMPS { get; set; }
        public string RNGC_FARMER { get; set; }
        public string RNGC_WORK_STAT { get; set; }
        public string RNGC_EMPLOYED { get; set; }
        public string RNGC_DOB_NA { get; set; }
        public string RNGC_RACE { get; set; }
        public string RNGC_NAME_IX_LAST { get; set; }
        public string RNGC_NAME_IX_MI { get; set; }
        public string Mode { get; set; }

        #endregion
    }

    public class LeanIntakeEntity
    {


        #region Constructors

        public LeanIntakeEntity()
        {
            PIP_ID = string.Empty;
            PIP_REG_ID = string.Empty;
            PIP_CONFNO = string.Empty;
            PIP_SSN = string.Empty;
            PIP_EMAIL = string.Empty;
            PIP_FNAME = string.Empty;
            PIP_MNAME = string.Empty;
            PIP_LNAME = string.Empty;
            PIP_DOB = string.Empty;
            PIP_GENDER = string.Empty;
            PIP_MARITAL_STATUS = string.Empty;
            PIP_RELATIONSHIP = string.Empty;
            PIP_ETHNIC = string.Empty;
            PIP_RACE = string.Empty;
            PIP_EDUCATION = string.Empty;
            PIP_DISABLE = string.Empty;
            PIP_WORK_STAT = string.Empty;
            PIP_PRI_LANGUAGE = string.Empty;
            PIP_FAMILY_TYPE = string.Empty;
            PIP_HOUSING = string.Empty;
            PIP_SCHOOL = string.Empty;
            PIP_HEALTH_INS = string.Empty;
            PIP_VETERAN = string.Empty;
            PIP_FOOD_STAMP = string.Empty;
            PIP_FARMER = string.Empty;
            PIP_WIC = string.Empty;
            PIP_RELITRAN = string.Empty;
            PIP_DRVLIC = string.Empty;
            PIP_MILITARY_STATUS = string.Empty;
            //PIP_YOUTH = string.Empty;
            PIP_PREGNANT = string.Empty;
            PIP_HEALTH_CODES = string.Empty;
            PIP_NCASHBEN = string.Empty;
            PIP_INCOME_TYPES = string.Empty;
            // PIP_ADDRESS = string.Empty;
            PIP_AREA = string.Empty;
            PIP_HOME_PHONE = string.Empty;
            PIP_CELL_NUMBER = string.Empty;
            PIP_HOUSENO = string.Empty;
            PIP_DIRECTION = string.Empty;
            PIP_STREET = string.Empty;
            PIP_SUFFIX = string.Empty;
            PIP_APT = string.Empty;
            PIP_FLR = string.Empty;
            PIP_CITY = string.Empty;
            PIP_STATE = string.Empty;
            PIP_ZIP = string.Empty;
            PIP_TOWNSHIP = string.Empty;
            PIP_COUNTY = string.Empty;
            PIP_Precint = string.Empty;
            PIP_SEQ = string.Empty;
            PIP_STATUS = string.Empty;
            PIP_CAP_AGENCY = string.Empty;
            PIP_DEPT = string.Empty;
            PIP_PROGRAM = string.Empty;
            PIP_YEAR = string.Empty;
            PIP_APP_NO = string.Empty;
            PIP_DBNAME = string.Empty;
            PIP_DRAGGED = string.Empty;
            PIP_AGENCY = string.Empty;
            PIP_AGY = string.Empty;
            PIP_ADD_DATE = string.Empty;
            PIP_CHNG_DATE = string.Empty;
            PIP_SERVICES = string.Empty;
            PIP_ID_MODE = string.Empty;
            PIP_USER_ID_MODE = string.Empty;
            PIP_CODE_MODE = string.Empty;
            PIP_SSN_MODE = string.Empty;
            PIP_EMAIL_MODE = string.Empty;
            PIP_FNAME_MODE = string.Empty;
            PIP_MNAME_MODE = string.Empty;
            PIP_LNAME_MODE = string.Empty;
            PIP_DOB_MODE = string.Empty;
            PIP_GENDER_MODE = string.Empty;
            PIP_MARITAL_STATUS_MODE = string.Empty;
            PIP_RELATIONSHIP_MODE = string.Empty;
            PIP_ETHNIC_MODE = string.Empty;
            PIP_RACE_MODE = string.Empty;
            PIP_EDUCATION_MODE = string.Empty;
            PIP_DISABLE_MODE = string.Empty;
            PIP_WORK_STAT_MODE = string.Empty;
            PIP_PRI_LANGUAGE_MODE = string.Empty;
            PIP_FAMILY_TYPE_MODE = string.Empty;
            PIP_HOUSING_MODE = string.Empty;
            PIP_SCHOOL_MODE = string.Empty;
            PIP_HEALTH_INS_MODE = string.Empty;
            PIP_VETERAN_MODE = string.Empty;
            PIP_FOOD_STAMP_MODE = string.Empty;
            PIP_FARMER_MODE = string.Empty;
            PIP_WIC_MODE = string.Empty;
            PIP_RELITRAN_MODE = string.Empty;
            PIP_DRVLIC_MODE = string.Empty;
            PIP_MILITARY_STATUS_MODE = string.Empty;
            PIP_YOUTH_MODE = string.Empty;
            PIP_PREGNANT_MODE = string.Empty;
            PIP_HEALTH_CODES_MODE = string.Empty;
            PIP_NCASHBEN_MODE = string.Empty;
            PIP_INCOME_TYPES_MODE = string.Empty;
            PIP_ADDRESS_MODE = string.Empty;
            PIP_AREA_MODE = string.Empty;
            PIP_HOME_PHONE_MODE = string.Empty;
            PIP_CELL_NUMBER_MODE = string.Empty;
            PIP_HOUSENO_MODE = string.Empty;
            PIP_DIRECTION_MODE = string.Empty;
            PIP_STREET_MODE = string.Empty;
            PIP_SUFFIX_MODE = string.Empty;
            PIP_APT_MODE = string.Empty;
            PIP_FLR_MODE = string.Empty;
            PIP_CITY_MODE = string.Empty;
            PIP_STATE_MODE = string.Empty;
            PIP_ZIP_MODE = string.Empty;
            PIP_TOWNSHIP_MODE = string.Empty;
            PIP_COUNTY_MODE = string.Empty;
            PIP_PRECINT_MODE = string.Empty;
            PIP_SEQ_MODE = string.Empty;
            PIP_STATUS_MODE = string.Empty;
            PIP_AGENCY_MODE = string.Empty;
            PIP_DEPT_MODE = string.Empty;
            PIP_PROGRAM_MODE = string.Empty;
            PIP_YEAR_MODE = string.Empty;
            PIP_APP_NO_MODE = string.Empty;
            PIP_DBNAME_MODE = string.Empty;
            PIP_DRAGGED_MODE = string.Empty;
            PIP_ADD_DATE_MODE = string.Empty;
            PIP_CHNG_DATE_MODE = string.Empty;
            PIP_SERVICES_MODE = string.Empty;
            PIP_Valid_MODE = 0;
            Mode = string.Empty;
            PIP_MEMBER_TYPE = string.Empty;
            PIP_AGY = string.Empty;
            PIP_AGENCY = string.Empty;
            //Pip_service_list_Mode = null;

        }

        public LeanIntakeEntity(DataRow row)
        {
            if (row != null)
            {
                PIP_ID = row["PIP_ID"].ToString().Trim();
                PIP_REG_ID = row["PIP_REG_ID"].ToString().Trim();
                PIP_CONFNO = row["PIPREG_CONFNO"].ToString().Trim();
                PIP_SSN = row["PIP_SSN"].ToString().Trim();
                PIP_EMAIL = row["PIP_EMAIL"].ToString().Trim();
                PIP_FNAME = row["PIP_FNAME"].ToString().Trim();
                PIP_MNAME = row["PIP_MNAME"].ToString().Trim();
                PIP_LNAME = row["PIP_LNAME"].ToString().Trim();
                PIP_DOB = row["PIP_DOB"].ToString().Trim();
                PIP_GENDER = row["PIP_GENDER"].ToString().Trim();
                PIP_MARITAL_STATUS = row["PIP_MARITAL_STATUS"].ToString().Trim();
                PIP_RELATIONSHIP = row["PIP_MEMBER_CODE"].ToString().Trim();
                PIP_ETHNIC = row["PIP_ETHNIC"].ToString().Trim();
                PIP_RACE = row["PIP_RACE"].ToString().Trim();
                PIP_EDUCATION = row["PIP_EDUCATION"].ToString().Trim();
                PIP_DISABLE = row["PIP_DISABLE"].ToString().Trim();
                PIP_WORK_STAT = row["PIP_WORK_STAT"].ToString().Trim();
                PIP_PRI_LANGUAGE = row["PIP_PRI_LANGUAGE"].ToString().Trim();
                PIP_FAMILY_TYPE = row["PIP_FAMILY_TYPE"].ToString().Trim();
                PIP_HOUSING = row["PIP_HOUSING"].ToString().Trim();
                PIP_SCHOOL = row["PIP_SCHOOL"].ToString().Trim();
                PIP_HEALTH_INS = row["PIP_HEALTH_INS"].ToString().Trim();
                PIP_VETERAN = row["PIP_VETERAN"].ToString().Trim();
                PIP_FOOD_STAMP = row["PIP_FOOD_STAMP"].ToString().Trim();
                PIP_FARMER = row["PIP_FARMER"].ToString().Trim();
                PIP_WIC = row["PIP_WIC"].ToString().Trim();
                PIP_RELITRAN = row["PIP_RELITRAN"].ToString().Trim();
                PIP_DRVLIC = row["PIP_DRVLIC"].ToString().Trim();
                PIP_MILITARY_STATUS = row["PIP_MILITARY_STATUS"].ToString().Trim();
                //PIP_YOUTH = row["PIP_YOUTH"].ToString().Trim();
                PIP_PREGNANT = row["PIP_PREGNANT"].ToString().Trim();
                PIP_HEALTH_CODES = row["PIP_HEALTH_CODES"].ToString().Trim();
                PIP_NCASHBEN = row["PIP_NCASHBEN"].ToString().Trim();
                PIP_INCOME_TYPES = row["PIP_INCOME_TYPES"].ToString().Trim();
                //  PIP_ADDRESS = row["PIP_ADDRESS"].ToString().Trim();
                PIP_AREA = row["PIP_AREA"].ToString().Trim();
                PIP_HOME_PHONE = row["PIP_HOME_PHONE"].ToString().Trim();
                PIP_CELL_NUMBER = row["PIP_CELL_NUMBER"].ToString().Trim();
                PIP_HOUSENO = row["PIP_HOUSENO"].ToString().Trim();
                PIP_DIRECTION = row["PIP_DIRECTION"].ToString().Trim();
                PIP_STREET = row["PIP_STREET"].ToString().Trim();
                PIP_SUFFIX = row["PIP_SUFFIX"].ToString().Trim();
                PIP_APT = row["PIP_APT"].ToString().Trim();
                PIP_FLR = row["PIP_FLR"].ToString().Trim();
                PIP_CITY = row["PIP_CITY"].ToString().Trim();
                PIP_STATE = row["PIP_STATE"].ToString().Trim();
                PIP_ZIP = row["PIP_ZIP"].ToString().Trim();
                PIP_TOWNSHIP = row["PIP_TOWNSHIP"].ToString().Trim();
                PIP_COUNTY = row["PIP_COUNTY"].ToString().Trim();
                PIP_SEQ = row["PIP_FAM_SEQ"].ToString().Trim();
                PIP_STATUS = row["PIP_ENTRY"].ToString().Trim();
                PIP_CAP_AGENCY = row["PIP_CAP_AGY"].ToString().Trim();
                PIP_DEPT = row["PIP_CAP_DEPT"].ToString().Trim();
                PIP_PROGRAM = row["PIP_CAP_PROG"].ToString().Trim();
                PIP_YEAR = row["PIP_CAP_YEAR"].ToString().Trim();
                PIP_APP_NO = row["PIP_CAP_APP_NO"].ToString().Trim();
                PIP_DBNAME = row["PIP_AGENCY"].ToString().Trim();
                PIP_AGENCY = row["PIP_AGENCY"].ToString().Trim();
                PIP_AGY = row["PIP_AGY"].ToString().Trim();
                PIP_DRAGGED = row["PIP_DRAGGED"].ToString().Trim();
                PIP_ADD_DATE = row["PIP_DATE_ADD"].ToString().Trim();
                PIP_CHNG_DATE = row["PIP_DATE_LSTC"].ToString().Trim();
                PIP_SERVICES = row["PIP_SERVICES"].ToString().Trim();
                PIP_Precint = string.Empty;

                PIP_ID_MODE = string.Empty;
                PIP_USER_ID_MODE = string.Empty;
                PIP_CODE_MODE = string.Empty;
                PIP_SSN_MODE = string.Empty;
                PIP_EMAIL_MODE = string.Empty;
                PIP_FNAME_MODE = string.Empty;
                PIP_MNAME_MODE = string.Empty;
                PIP_LNAME_MODE = string.Empty;
                PIP_DOB_MODE = string.Empty;
                PIP_GENDER_MODE = string.Empty;
                PIP_MARITAL_STATUS_MODE = string.Empty;
                PIP_RELATIONSHIP_MODE = string.Empty;
                PIP_ETHNIC_MODE = string.Empty;
                PIP_RACE_MODE = string.Empty;
                PIP_EDUCATION_MODE = string.Empty;
                PIP_DISABLE_MODE = string.Empty;
                PIP_WORK_STAT_MODE = string.Empty;
                PIP_PRI_LANGUAGE_MODE = string.Empty;
                PIP_FAMILY_TYPE_MODE = string.Empty;
                PIP_HOUSING_MODE = string.Empty;
                PIP_SCHOOL_MODE = string.Empty;
                PIP_HEALTH_INS_MODE = string.Empty;
                PIP_VETERAN_MODE = string.Empty;
                PIP_FOOD_STAMP_MODE = string.Empty;
                PIP_FARMER_MODE = string.Empty;
                PIP_WIC_MODE = string.Empty;
                PIP_RELITRAN_MODE = string.Empty;
                PIP_DRVLIC_MODE = string.Empty;
                PIP_MILITARY_STATUS_MODE = string.Empty;
                PIP_YOUTH_MODE = string.Empty;
                PIP_PREGNANT_MODE = string.Empty;
                PIP_HEALTH_CODES_MODE = string.Empty;
                PIP_NCASHBEN_MODE = string.Empty;
                PIP_INCOME_TYPES_MODE = string.Empty;
                PIP_ADDRESS_MODE = string.Empty;
                PIP_AREA_MODE = string.Empty;
                PIP_HOME_PHONE_MODE = string.Empty;
                PIP_CELL_NUMBER_MODE = string.Empty;
                PIP_HOUSENO_MODE = string.Empty;
                PIP_DIRECTION_MODE = string.Empty;
                PIP_STREET_MODE = string.Empty;
                PIP_SUFFIX_MODE = string.Empty;
                PIP_APT_MODE = string.Empty;
                PIP_FLR_MODE = string.Empty;
                PIP_CITY_MODE = string.Empty;
                PIP_STATE_MODE = string.Empty;
                PIP_ZIP_MODE = string.Empty;
                PIP_TOWNSHIP_MODE = string.Empty;
                PIP_COUNTY_MODE = string.Empty;
                PIP_PRECINT_MODE = string.Empty;
                PIP_SEQ_MODE = string.Empty;
                PIP_STATUS_MODE = string.Empty;
                PIP_AGENCY_MODE = string.Empty;
                PIP_DEPT_MODE = string.Empty;
                PIP_PROGRAM_MODE = string.Empty;
                PIP_YEAR_MODE = string.Empty;
                PIP_APP_NO_MODE = string.Empty;
                PIP_DBNAME_MODE = string.Empty;
                PIP_DRAGGED_MODE = string.Empty;
                PIP_ADD_DATE_MODE = string.Empty;
                PIP_CHNG_DATE_MODE = string.Empty;
                PIP_SERVICES_MODE = string.Empty;
                PIP_Valid_MODE = 0;
                PIP_MEMBER_TYPE = string.Empty;
                if (PIP_SERVICES != string.Empty)
                {
                    string[] strServices = PIP_SERVICES.Split(',');
                   
                    foreach (string strserv in strServices)
                    {
                        if (strserv.ToString() != string.Empty)
                        {
                            Pip_service_list_Mode.Add(new Utilities.ListItem(strserv, "FALSE"));
                        }
                    }
                }

                if (PIP_INCOME_TYPES != string.Empty)
                {
                    string[] strIncomeTypes = PIP_INCOME_TYPES.Split(',');
                   
                    foreach (string strtype in strIncomeTypes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Income_list_Mode.Add(new Utilities.ListItem(strtype, "FALSE"));
                        }
                    }
                }

                if (PIP_HEALTH_CODES != string.Empty)
                {
                    string[] strHealthcodes = PIP_HEALTH_CODES.Split(',');

                    foreach (string strtype in strHealthcodes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Healthcodes_list_Mode.Add(new Utilities.ListItem(strtype, "FALSE"));
                        }
                    }
                }

                if (PIP_NCASHBEN != string.Empty)
                {
                    string[] strNonCashbenefits = PIP_NCASHBEN.Split(',');

                    foreach (string strtype in strNonCashbenefits)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_NonCashbenefit_list_Mode.Add(new Utilities.ListItem(strtype, "FALSE"));
                        }
                    }
                }


            }
        }

        public LeanIntakeEntity(DataRow row, string strValue)
        {
            if (row != null)
            {
                PIP_ID = row["PIP_ID"].ToString().Trim();
                PIP_REG_ID = row["PIP_REG_ID"].ToString().Trim();
                PIP_CONFNO = row["PIPREG_CONFNO"].ToString().Trim();
                PIP_SSN = row["PIP_SSN"].ToString().Trim();
                PIP_EMAIL = row["PIP_EMAIL"].ToString().Trim();
                PIP_FNAME = row["PIP_FNAME"].ToString().Trim();
                PIP_MNAME = row["PIP_MNAME"].ToString().Trim();
                PIP_LNAME = row["PIP_LNAME"].ToString().Trim();
                PIP_DOB = row["PIP_DOB"].ToString().Trim();
                PIP_GENDER = row["PIP_GENDER"].ToString().Trim();
                PIP_MARITAL_STATUS = row["PIP_MARITAL_STATUS"].ToString().Trim();
                PIP_RELATIONSHIP = row["PIP_MEMBER_CODE"].ToString().Trim();
                PIP_ETHNIC = row["PIP_ETHNIC"].ToString().Trim();
                PIP_RACE = row["PIP_RACE"].ToString().Trim();
                PIP_EDUCATION = row["PIP_EDUCATION"].ToString().Trim();
                PIP_DISABLE = row["PIP_DISABLE"].ToString().Trim();
                PIP_WORK_STAT = row["PIP_WORK_STAT"].ToString().Trim();
                PIP_PRI_LANGUAGE = row["PIP_PRI_LANGUAGE"].ToString().Trim();
                PIP_FAMILY_TYPE = row["PIP_FAMILY_TYPE"].ToString().Trim();
                PIP_HOUSING = row["PIP_HOUSING"].ToString().Trim();
                PIP_SCHOOL = row["PIP_SCHOOL"].ToString().Trim();
                PIP_HEALTH_INS = row["PIP_HEALTH_INS"].ToString().Trim();
                PIP_VETERAN = row["PIP_VETERAN"].ToString().Trim();
                PIP_FOOD_STAMP = row["PIP_FOOD_STAMP"].ToString().Trim();
                PIP_FARMER = row["PIP_FARMER"].ToString().Trim();
                PIP_WIC = row["PIP_WIC"].ToString().Trim();
                PIP_RELITRAN = row["PIP_RELITRAN"].ToString().Trim();
                PIP_DRVLIC = row["PIP_DRVLIC"].ToString().Trim();
                PIP_MILITARY_STATUS = row["PIP_MILITARY_STATUS"].ToString().Trim();
                // PIP_YOUTH = row["PIP_YOUTH"].ToString().Trim();
                PIP_PREGNANT = row["PIP_PREGNANT"].ToString().Trim();
                PIP_HEALTH_CODES = row["PIP_HEALTH_CODES"].ToString().Trim();
                PIP_NCASHBEN = row["PIP_NCASHBEN"].ToString().Trim();
                PIP_INCOME_TYPES = row["PIP_INCOME_TYPES"].ToString().Trim();
                //  PIP_ADDRESS = row["PIP_ADDRESS"].ToString().Trim();
                PIP_AREA = row["PIP_AREA"].ToString().Trim();
                PIP_HOME_PHONE = row["PIP_HOME_PHONE"].ToString().Trim();
                PIP_CELL_NUMBER = row["PIP_CELL_NUMBER"].ToString().Trim();
                PIP_HOUSENO = row["PIP_HOUSENO"].ToString().Trim();
                PIP_DIRECTION = row["PIP_DIRECTION"].ToString().Trim();
                PIP_STREET = row["PIP_STREET"].ToString().Trim();
                PIP_SUFFIX = row["PIP_SUFFIX"].ToString().Trim();
                PIP_APT = row["PIP_APT"].ToString().Trim();
                PIP_FLR = row["PIP_FLR"].ToString().Trim();
                PIP_CITY = row["PIP_CITY"].ToString().Trim();
                PIP_STATE = row["PIP_STATE"].ToString().Trim();
                PIP_ZIP = row["PIP_ZIP"].ToString().Trim();
                PIP_TOWNSHIP = row["PIP_TOWNSHIP"].ToString().Trim();
                PIP_COUNTY = row["PIP_COUNTY"].ToString().Trim();
                PIP_SEQ = row["PIP_FAM_SEQ"].ToString().Trim();
                PIP_STATUS = row["PIP_ENTRY"].ToString().Trim();
                PIP_CAP_AGENCY = row["PIP_CAP_AGY"].ToString().Trim();
                PIP_DEPT = row["PIP_CAP_DEPT"].ToString().Trim();
                PIP_PROGRAM = row["PIP_CAP_PROG"].ToString().Trim();
                PIP_YEAR = row["PIP_CAP_YEAR"].ToString().Trim();
                PIP_APP_NO = row["PIP_CAP_APP_NO"].ToString().Trim();
                PIP_DBNAME = row["PIP_AGENCY"].ToString().Trim();
                PIP_AGENCY = row["PIP_AGENCY"].ToString().Trim();
                PIP_AGY = row["PIP_AGY"].ToString().Trim();
                PIP_DRAGGED = row["PIP_DRAGGED"].ToString().Trim();
                PIP_ADD_DATE = row["PIP_DATE_ADD"].ToString().Trim();
                PIP_CHNG_DATE = row["PIP_DATE_LSTC"].ToString().Trim();
                PIP_SERVICES = row["PIP_SERVICES"].ToString().Trim();
                PIP_Precint = string.Empty;
                PIP_ID_MODE = strValue;
                PIP_USER_ID_MODE = strValue;
                PIP_CODE_MODE = strValue;
                PIP_SSN_MODE = strValue;
                PIP_EMAIL_MODE = strValue;
                PIP_FNAME_MODE = strValue;
                PIP_MNAME_MODE = strValue;
                PIP_LNAME_MODE = strValue;
                PIP_DOB_MODE = strValue;
                PIP_GENDER_MODE = strValue;
                PIP_MARITAL_STATUS_MODE = strValue;
                PIP_RELATIONSHIP_MODE = strValue;
                PIP_ETHNIC_MODE = strValue;
                PIP_RACE_MODE = strValue;
                PIP_EDUCATION_MODE = strValue;
                PIP_DISABLE_MODE = strValue;
                PIP_WORK_STAT_MODE = strValue;
                PIP_PRI_LANGUAGE_MODE = strValue;
                PIP_FAMILY_TYPE_MODE = strValue;
                PIP_HOUSING_MODE = strValue;
                PIP_SCHOOL_MODE = strValue;
                PIP_HEALTH_INS_MODE = strValue;
                PIP_VETERAN_MODE = strValue;
                PIP_FOOD_STAMP_MODE = strValue;
                PIP_FARMER_MODE = strValue;
                PIP_WIC_MODE = strValue;
                PIP_RELITRAN_MODE = strValue;
                PIP_DRVLIC_MODE = strValue;
                PIP_MILITARY_STATUS_MODE = strValue;
                PIP_YOUTH_MODE = strValue;
                PIP_PREGNANT_MODE = strValue;
                PIP_HEALTH_CODES_MODE = strValue;
                PIP_NCASHBEN_MODE = strValue;
                PIP_INCOME_TYPES_MODE = strValue;
                PIP_ADDRESS_MODE = strValue;
                PIP_AREA_MODE = strValue;
                PIP_HOME_PHONE_MODE = strValue;
                PIP_CELL_NUMBER_MODE = strValue;
                PIP_HOUSENO_MODE = strValue;
                PIP_DIRECTION_MODE = strValue;
                PIP_STREET_MODE = strValue;
                PIP_SUFFIX_MODE = strValue;
                PIP_APT_MODE = strValue;
                PIP_FLR_MODE = strValue;
                PIP_CITY_MODE = strValue;
                PIP_STATE_MODE = strValue;
                PIP_ZIP_MODE = strValue;
                PIP_TOWNSHIP_MODE = strValue;
                PIP_COUNTY_MODE = strValue;
                PIP_PRECINT_MODE = strValue;
                PIP_SEQ_MODE = strValue;
                PIP_STATUS_MODE = strValue;
                PIP_AGENCY_MODE = strValue;
                PIP_DEPT_MODE = strValue;
                PIP_PROGRAM_MODE = strValue;
                PIP_YEAR_MODE = strValue;
                PIP_APP_NO_MODE = strValue;
                PIP_DBNAME_MODE = strValue;
                PIP_DRAGGED_MODE = strValue;
                PIP_ADD_DATE_MODE = strValue;
                PIP_CHNG_DATE_MODE = strValue;
                PIP_SERVICES_MODE = strValue;
                PIP_Valid_MODE = 1;
                PIP_MEMBER_TYPE = string.Empty;
                if (PIP_SERVICES != string.Empty)
                {
                    string[] strServices = PIP_SERVICES.Split(',');
                    int intservices = 0;
                    foreach (string strserv in strServices)
                    {
                        if (strserv.ToString() != string.Empty)
                        {
                            Pip_service_list_Mode.Add(new Utilities.ListItem(strserv, strValue));
                        }
                    }
                }
                if (PIP_INCOME_TYPES != string.Empty)
                {
                    string[] strIncomeTypes = PIP_INCOME_TYPES.Split(',');
                    
                    foreach (string strtype in strIncomeTypes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Income_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }
                if (PIP_HEALTH_CODES != string.Empty)
                {
                    string[] strHealthcodes = PIP_HEALTH_CODES.Split(',');

                    foreach (string strtype in strHealthcodes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Healthcodes_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }

                if (PIP_NCASHBEN != string.Empty)
                {
                    string[] strNonCashbenefits = PIP_NCASHBEN.Split(',');

                    foreach (string strtype in strNonCashbenefits)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_NonCashbenefit_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }
            }
        }

        public LeanIntakeEntity(CaseSnpEntity snpdata, CaseMstEntity mstdata)
        {
            if (snpdata != null)
            {
                //PIP_ID = row["PIP_ID"].ToString().Trim();
                //PIP_REG_ID = row["PIP_REG_ID"].ToString().Trim();
                //PIP_CONFNO = row["PIP_CONFNO"].ToString().Trim();
                PIP_SSN = snpdata.Ssno;// row["PIP_SSN"].ToString().Trim();
                PIP_EMAIL = mstdata.Email;// row["PIP_EMAIL"].ToString().Trim();
                PIP_FNAME = snpdata.NameixFi;// row["PIP_FNAME"].ToString().Trim();
                PIP_MNAME = snpdata.NameixMi;//row["PIP_MNAME"].ToString().Trim();
                PIP_LNAME = snpdata.NameixLast; //row["PIP_LNAME"].ToString().Trim();
                PIP_DOB = snpdata.AltBdate;//row["PIP_DOB"].ToString().Trim();
                PIP_GENDER = snpdata.Sex;//row["PIP_GENDER"].ToString().Trim();
                PIP_MARITAL_STATUS = snpdata.MaritalStatus; //row["PIP_MARITAL_STATUS"].ToString().Trim();
                PIP_RELATIONSHIP = snpdata.MemberCode;// row["PIP_MEMBER_CODE"].ToString().Trim();
                PIP_ETHNIC = snpdata.Ethnic;//row["PIP_ETHNIC"].ToString().Trim();
                PIP_RACE = snpdata.Race;// row["PIP_RACE"].ToString().Trim();
                PIP_EDUCATION = snpdata.Education;//row["PIP_EDUCATION"].ToString().Trim();
                PIP_DISABLE = snpdata.Disable;//row["PIP_DISABLE"].ToString().Trim();
                PIP_WORK_STAT = snpdata.WorkStatus;// row["PIP_WORK_STAT"].ToString().Trim();
                PIP_PRI_LANGUAGE = mstdata.Language;// row["PIP_PRI_LANGUAGE"].ToString().Trim();
                PIP_FAMILY_TYPE = mstdata.FamilyType;//row["PIP_FAMILY_TYPE"].ToString().Trim();
                PIP_HOUSING = mstdata.Housing; //row["PIP_HOUSING"].ToString().Trim();
                PIP_SCHOOL = snpdata.SchoolDistrict;// row["PIP_SCHOOL"].ToString().Trim();
                PIP_HEALTH_INS = snpdata.HealthIns;// row["PIP_HEALTH_INS"].ToString().Trim();
                PIP_VETERAN = snpdata.Vet;// row["PIP_VETERAN"].ToString().Trim();
                PIP_FOOD_STAMP = snpdata.FootStamps;// row["PIP_FOOD_STAMP"].ToString().Trim();
                PIP_FARMER = snpdata.Farmer;// row["PIP_FARMER"].ToString().Trim();
                PIP_WIC = snpdata.Wic;// row["PIP_WIC"].ToString().Trim();
                PIP_RELITRAN = snpdata.Relitran;// row["PIP_RELITRAN"].ToString().Trim();
                PIP_DRVLIC = snpdata.Drvlic;// row["PIP_DRVLIC"].ToString().Trim();
                PIP_MILITARY_STATUS = snpdata.MilitaryStatus;// row["PIP_MILITARY_STATUS"].ToString().Trim();
                // PIP_YOUTH = row["PIP_YOUTH"].ToString().Trim();
                PIP_PREGNANT = snpdata.Pregnant;// row["PIP_PREGNANT"].ToString().Trim();
                PIP_HEALTH_CODES = snpdata.Health_Codes;// row["PIP_HEALTH_CODES"].ToString().Trim();
                PIP_NCASHBEN = snpdata.NonCashBenefits;// row["PIP_NCASHBEN"].ToString().Trim();
                PIP_INCOME_TYPES = snpdata.SnpIncomeTypes;// row["PIP_INCOME_TYPES"].ToString().Trim();
                //  PIP_ADDRESS = row["PIP_ADDRESS"].ToString().Trim();
                PIP_AREA = mstdata.Area;// row["PIP_AREA"].ToString().Trim();
                PIP_HOME_PHONE = mstdata.Phone;// row["PIP_HOME_PHONE"].ToString().Trim();
                PIP_CELL_NUMBER = mstdata.CellPhone;// row["PIP_CELL_NUMBER"].ToString().Trim();
                PIP_HOUSENO = mstdata.Hn;// row["PIP_HOUSENO"].ToString().Trim();
                PIP_DIRECTION = mstdata.Direction;// row["PIP_DIRECTION"].ToString().Trim();
                PIP_STREET = mstdata.Street;// row["PIP_STREET"].ToString().Trim();
                PIP_SUFFIX = mstdata.Suffix;// row["PIP_SUFFIX"].ToString().Trim();
                PIP_APT = mstdata.Apt;// row["PIP_APT"].ToString().Trim();
                PIP_FLR = mstdata.Flr;// row["PIP_FLR"].ToString().Trim();
                PIP_CITY = mstdata.City;// row["PIP_CITY"].ToString().Trim();
                PIP_STATE = mstdata.State;// row["PIP_STATE"].ToString().Trim();
                PIP_ZIP = mstdata.Zip;// row["PIP_ZIP"].ToString().Trim();
                PIP_TOWNSHIP = mstdata.TownShip;// row["PIP_TOWNSHIP"].ToString().Trim();
                PIP_COUNTY = mstdata.County;// row["PIP_COUNTY"].ToString().Trim();
                PIP_Precint = mstdata.Precinct;
                PIP_SEQ = snpdata.FamilySeq; //row["PIP_FAM_SEQ"].ToString().Trim();
                PIP_STATUS = snpdata.Status; //row["PIP_ENTRY"].ToString().Trim();
                                             //PIP_CAP_AGENCY = row["PIP_CAP_AGY"].ToString().Trim();
                                             //PIP_DEPT = row["PIP_CAP_DEPT"].ToString().Trim();
                                             //PIP_PROGRAM = row["PIP_CAP_PROG"].ToString().Trim();
                                             //PIP_YEAR = row["PIP_CAP_YEAR"].ToString().Trim();
                                             //PIP_APP_NO = row["PIP_CAP_APP_NO"].ToString().Trim();
                                             //PIP_DBNAME = row["PIP_AGENCY"].ToString().Trim();
                                             //PIP_DRAGGED = row["PIP_DRAGGED"].ToString().Trim();
                                             //PIP_ADD_DATE = row["PIP_DATE_ADD"].ToString().Trim();
                                             //PIP_CHNG_DATE = row["PIP_DATE_LSTC"].ToString().Trim();
                                             // PIP_SERVICES =  row["PIP_SERVICES"].ToString().Trim();

                PIP_USER_ID_MODE = string.Empty;
                PIP_CODE_MODE = string.Empty;
                PIP_SSN_MODE = string.Empty;
                PIP_EMAIL_MODE = string.Empty;
                PIP_FNAME_MODE = string.Empty;
                PIP_MNAME_MODE = string.Empty;
                PIP_LNAME_MODE = string.Empty;
                PIP_DOB_MODE = string.Empty;
                PIP_GENDER_MODE = string.Empty;
                PIP_MARITAL_STATUS_MODE = string.Empty;
                PIP_RELATIONSHIP_MODE = string.Empty;
                PIP_ETHNIC_MODE = string.Empty;
                PIP_RACE_MODE = string.Empty;
                PIP_EDUCATION_MODE = string.Empty;
                PIP_DISABLE_MODE = string.Empty;
                PIP_WORK_STAT_MODE = string.Empty;
                PIP_PRI_LANGUAGE_MODE = string.Empty;
                PIP_FAMILY_TYPE_MODE = string.Empty;
                PIP_HOUSING_MODE = string.Empty;
                PIP_SCHOOL_MODE = string.Empty;
                PIP_HEALTH_INS_MODE = string.Empty;
                PIP_VETERAN_MODE = string.Empty;
                PIP_FOOD_STAMP_MODE = string.Empty;
                PIP_FARMER_MODE = string.Empty;
                PIP_WIC_MODE = string.Empty;
                PIP_RELITRAN_MODE = string.Empty;
                PIP_DRVLIC_MODE = string.Empty;
                PIP_MILITARY_STATUS_MODE = string.Empty;
                PIP_YOUTH_MODE = string.Empty;
                PIP_PREGNANT_MODE = string.Empty;
                PIP_HEALTH_CODES_MODE = string.Empty;
                PIP_NCASHBEN_MODE = string.Empty;
                PIP_INCOME_TYPES_MODE = string.Empty;
                PIP_ADDRESS_MODE = string.Empty;
                PIP_AREA_MODE = string.Empty;
                PIP_HOME_PHONE_MODE = string.Empty;
                PIP_CELL_NUMBER_MODE = string.Empty;
                PIP_HOUSENO_MODE = string.Empty;
                PIP_DIRECTION_MODE = string.Empty;
                PIP_STREET_MODE = string.Empty;
                PIP_SUFFIX_MODE = string.Empty;
                PIP_APT_MODE = string.Empty;
                PIP_FLR_MODE = string.Empty;
                PIP_CITY_MODE = string.Empty;
                PIP_STATE_MODE = string.Empty;
                PIP_ZIP_MODE = string.Empty;
                PIP_TOWNSHIP_MODE = string.Empty;
                PIP_COUNTY_MODE = string.Empty;
                PIP_PRECINT_MODE = string.Empty;
                PIP_SEQ_MODE = string.Empty;
                PIP_STATUS_MODE = string.Empty;
                PIP_AGENCY_MODE = string.Empty;
                PIP_DEPT_MODE = string.Empty;
                PIP_PROGRAM_MODE = string.Empty;
                PIP_YEAR_MODE = string.Empty;
                PIP_APP_NO_MODE = string.Empty;
                PIP_DBNAME_MODE = string.Empty;
                PIP_DRAGGED_MODE = string.Empty;
                PIP_ADD_DATE_MODE = string.Empty;
                PIP_CHNG_DATE_MODE = string.Empty;
                PIP_SERVICES_MODE = string.Empty;
                PIP_Valid_MODE = 0;
                PIP_MEMBER_TYPE = string.Empty;



            }
        }

        public LeanIntakeEntity(CaseSnpEntity snpdata, CaseMstEntity mstdata, string strValue)
        {
            if (snpdata != null)
            {
                //PIP_ID = row["PIP_ID"].ToString().Trim();
                //PIP_REG_ID = row["PIP_REG_ID"].ToString().Trim();
                //PIP_CONFNO = row["PIP_CONFNO"].ToString().Trim();
                PIP_SSN = snpdata.Ssno;// row["PIP_SSN"].ToString().Trim();
                PIP_EMAIL = mstdata.Email;// row["PIP_EMAIL"].ToString().Trim();
                PIP_FNAME = snpdata.NameixFi;// row["PIP_FNAME"].ToString().Trim();
                PIP_MNAME = snpdata.NameixMi;//row["PIP_MNAME"].ToString().Trim();
                PIP_LNAME = snpdata.NameixLast; //row["PIP_LNAME"].ToString().Trim();
                PIP_DOB = snpdata.AltBdate;//row["PIP_DOB"].ToString().Trim();
                PIP_GENDER = snpdata.Sex;//row["PIP_GENDER"].ToString().Trim();
                PIP_MARITAL_STATUS = snpdata.MaritalStatus; //row["PIP_MARITAL_STATUS"].ToString().Trim();
                PIP_RELATIONSHIP = snpdata.MemberCode;// row["PIP_MEMBER_CODE"].ToString().Trim();
                PIP_ETHNIC = snpdata.Ethnic;//row["PIP_ETHNIC"].ToString().Trim();
                PIP_RACE = snpdata.Race;// row["PIP_RACE"].ToString().Trim();
                PIP_EDUCATION = snpdata.Education;//row["PIP_EDUCATION"].ToString().Trim();
                PIP_DISABLE = snpdata.Disable;//row["PIP_DISABLE"].ToString().Trim();
                PIP_WORK_STAT = snpdata.WorkStatus;// row["PIP_WORK_STAT"].ToString().Trim();
                PIP_PRI_LANGUAGE = mstdata.Language;// row["PIP_PRI_LANGUAGE"].ToString().Trim();
                PIP_FAMILY_TYPE = mstdata.FamilyType;//row["PIP_FAMILY_TYPE"].ToString().Trim();
                PIP_HOUSING = mstdata.Housing; //row["PIP_HOUSING"].ToString().Trim();
                PIP_SCHOOL = snpdata.SchoolDistrict; ;// row["PIP_SCHOOL"].ToString().Trim();
                PIP_HEALTH_INS = snpdata.HealthIns;// row["PIP_HEALTH_INS"].ToString().Trim();
                PIP_VETERAN = snpdata.Vet;// row["PIP_VETERAN"].ToString().Trim();
                PIP_FOOD_STAMP = snpdata.FootStamps;// row["PIP_FOOD_STAMP"].ToString().Trim();
                PIP_FARMER = snpdata.Farmer;// row["PIP_FARMER"].ToString().Trim();
                PIP_WIC = snpdata.Wic;// row["PIP_WIC"].ToString().Trim();
                PIP_RELITRAN = snpdata.Relitran;// row["PIP_RELITRAN"].ToString().Trim();
                PIP_DRVLIC = snpdata.Drvlic;// row["PIP_DRVLIC"].ToString().Trim();
                PIP_MILITARY_STATUS = snpdata.MilitaryStatus;// row["PIP_MILITARY_STATUS"].ToString().Trim();
                // PIP_YOUTH = row["PIP_YOUTH"].ToString().Trim();
                PIP_PREGNANT = snpdata.Pregnant;// row["PIP_PREGNANT"].ToString().Trim();
                PIP_HEALTH_CODES = snpdata.Health_Codes;// row["PIP_HEALTH_CODES"].ToString().Trim();
                PIP_NCASHBEN = snpdata.NonCashBenefits;// row["PIP_NCASHBEN"].ToString().Trim();
                PIP_INCOME_TYPES = snpdata.SnpIncomeTypes;// row["PIP_INCOME_TYPES"].ToString().Trim();
                //  PIP_ADDRESS = row["PIP_ADDRESS"].ToString().Trim();
                PIP_AREA = mstdata.Area;// row["PIP_AREA"].ToString().Trim();
                PIP_HOME_PHONE = mstdata.Phone;// row["PIP_HOME_PHONE"].ToString().Trim();
                PIP_CELL_NUMBER = mstdata.CellPhone;// row["PIP_CELL_NUMBER"].ToString().Trim();
                PIP_HOUSENO = mstdata.Hn;// row["PIP_HOUSENO"].ToString().Trim();
                PIP_DIRECTION = mstdata.Direction;// row["PIP_DIRECTION"].ToString().Trim();
                PIP_STREET = mstdata.Street;// row["PIP_STREET"].ToString().Trim();
                PIP_SUFFIX = mstdata.Suffix;// row["PIP_SUFFIX"].ToString().Trim();
                PIP_APT = mstdata.Apt;// row["PIP_APT"].ToString().Trim();
                PIP_FLR = mstdata.Flr;// row["PIP_FLR"].ToString().Trim();
                PIP_CITY = mstdata.City;// row["PIP_CITY"].ToString().Trim();
                PIP_STATE = mstdata.State;// row["PIP_STATE"].ToString().Trim();
                PIP_ZIP = mstdata.Zip;// row["PIP_ZIP"].ToString().Trim();
                PIP_TOWNSHIP = mstdata.TownShip;// row["PIP_TOWNSHIP"].ToString().Trim();
                PIP_COUNTY = mstdata.County;// row["PIP_COUNTY"].ToString().Trim();
                PIP_Precint = mstdata.Precinct;
                PIP_SEQ = snpdata.FamilySeq; //row["PIP_FAM_SEQ"].ToString().Trim();
                PIP_STATUS = snpdata.Status; //row["PIP_ENTRY"].ToString().Trim();
                PIP_CAP_AGENCY = snpdata.Agency.ToString().Trim();
                PIP_DEPT = snpdata.Dept.ToString().Trim();
                PIP_PROGRAM = snpdata.Program.ToString().Trim();
                PIP_YEAR = snpdata.Year.ToString().Trim();
                PIP_APP_NO = snpdata.App.ToString().Trim();
                //PIP_DBNAME = row["PIP_AGENCY"].ToString().Trim();
                //PIP_DRAGGED = row["PIP_DRAGGED"].ToString().Trim();
                //PIP_ADD_DATE = row["PIP_DATE_ADD"].ToString().Trim();
                //PIP_CHNG_DATE = row["PIP_DATE_LSTC"].ToString().Trim();
                PIP_SERVICES = mstdata.OutOfService.Trim();
                if (PIP_SERVICES != string.Empty)
                {
                    string[] strServices = PIP_SERVICES.Split(',');
                    
                    foreach (string strserv in strServices)
                    {
                        if (strserv.ToString() != string.Empty)
                        {
                            Pip_service_list_Mode.Add(new Utilities.ListItem(strserv, strValue));
                        }
                    }
                }
                if (PIP_INCOME_TYPES != string.Empty)
                {
                    string[] strIncomeTypes = PIP_INCOME_TYPES.Split(',');
                    
                    foreach (string strtype in strIncomeTypes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Income_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }
                if (PIP_HEALTH_CODES != string.Empty)
                {
                    string[] strHealthcodes = PIP_HEALTH_CODES.Split(',');

                    foreach (string strtype in strHealthcodes)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_Healthcodes_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }

                if (PIP_NCASHBEN != string.Empty)
                {
                    string[] strNonCashbenefits = PIP_NCASHBEN.Split(',');

                    foreach (string strtype in strNonCashbenefits)
                    {
                        if (strtype.ToString() != string.Empty)
                        {
                            Pip_NonCashbenefit_list_Mode.Add(new Utilities.ListItem(strtype, strValue));
                        }
                    }
                }

                // PIP_SERVICES =  row["PIP_SERVICES"].ToString().Trim();
                PIP_ID_MODE = strValue;
                PIP_USER_ID_MODE = strValue;
                PIP_CODE_MODE = strValue;
                PIP_SSN_MODE = strValue;
                PIP_EMAIL_MODE = strValue;
                PIP_FNAME_MODE = strValue;
                PIP_MNAME_MODE = strValue;
                PIP_LNAME_MODE = strValue;
                PIP_DOB_MODE = strValue;
                PIP_GENDER_MODE = strValue;
                PIP_MARITAL_STATUS_MODE = strValue;
                PIP_RELATIONSHIP_MODE = strValue;
                PIP_ETHNIC_MODE = strValue;
                PIP_RACE_MODE = strValue;
                PIP_EDUCATION_MODE = strValue;
                PIP_DISABLE_MODE = strValue;
                PIP_WORK_STAT_MODE = strValue;
                PIP_PRI_LANGUAGE_MODE = strValue;
                PIP_FAMILY_TYPE_MODE = strValue;
                PIP_HOUSING_MODE = strValue;
                PIP_SCHOOL_MODE = strValue;
                PIP_HEALTH_INS_MODE = strValue;
                PIP_VETERAN_MODE = strValue;
                PIP_FOOD_STAMP_MODE = strValue;
                PIP_FARMER_MODE = strValue;
                PIP_WIC_MODE = strValue;
                PIP_RELITRAN_MODE = strValue;
                PIP_DRVLIC_MODE = strValue;
                PIP_MILITARY_STATUS_MODE = strValue;
                PIP_YOUTH_MODE = strValue;
                PIP_PREGNANT_MODE = strValue;
                PIP_HEALTH_CODES_MODE = strValue;
                PIP_NCASHBEN_MODE = strValue;
                PIP_INCOME_TYPES_MODE = strValue;
                PIP_ADDRESS_MODE = strValue;
                PIP_AREA_MODE = strValue;
                PIP_HOME_PHONE_MODE = strValue;
                PIP_CELL_NUMBER_MODE = strValue;
                PIP_HOUSENO_MODE = strValue;
                PIP_DIRECTION_MODE = strValue;
                PIP_STREET_MODE = strValue;
                PIP_SUFFIX_MODE = strValue;
                PIP_APT_MODE = strValue;
                PIP_FLR_MODE = strValue;
                PIP_CITY_MODE = strValue;
                PIP_STATE_MODE = strValue;
                PIP_ZIP_MODE = strValue;
                PIP_TOWNSHIP_MODE = strValue;
                PIP_COUNTY_MODE = strValue;
                PIP_PRECINT_MODE = strValue;
                PIP_SEQ_MODE = strValue;
                PIP_STATUS_MODE = strValue;
                PIP_AGENCY_MODE = strValue;
                PIP_DEPT_MODE = strValue;
                PIP_PROGRAM_MODE = strValue;
                PIP_YEAR_MODE = strValue;
                PIP_APP_NO_MODE = strValue;
                PIP_DBNAME_MODE = strValue;
                PIP_DRAGGED_MODE = strValue;
                PIP_ADD_DATE_MODE = strValue;
                PIP_CHNG_DATE_MODE = strValue;
                PIP_SERVICES_MODE = strValue;
                if (strValue == "TRUE")
                    PIP_Valid_MODE = 1;
                else
                    PIP_Valid_MODE = 0;
                PIP_MEMBER_TYPE = string.Empty;

            }
        }



        #endregion

        #region Properties

        public string PIP_ID { get; set; }
        public string PIP_REG_ID { get; set; }
        public string PIP_CONFNO { get; set; }
        public string PIP_SSN { get; set; }
        public string PIP_EMAIL { get; set; }
        public string PIP_FNAME { get; set; }
        public string PIP_MNAME { get; set; }
        public string PIP_LNAME { get; set; }
        public string PIP_DOB { get; set; }
        public string PIP_GENDER { get; set; }
        public string PIP_MARITAL_STATUS { get; set; }
        public string PIP_RELATIONSHIP { get; set; }
        public string PIP_ETHNIC { get; set; }
        public string PIP_RACE { get; set; }
        public string PIP_EDUCATION { get; set; }
        public string PIP_DISABLE { get; set; }
        public string PIP_WORK_STAT { get; set; }
        public string PIP_PRI_LANGUAGE { get; set; }
        public string PIP_FAMILY_TYPE { get; set; }
        public string PIP_HOUSING { get; set; }
        public string PIP_SCHOOL { get; set; }
        public string PIP_HEALTH_INS { get; set; }
        public string PIP_VETERAN { get; set; }
        public string PIP_FOOD_STAMP { get; set; }
        public string PIP_FARMER { get; set; }
        public string PIP_WIC { get; set; }
        public string PIP_RELITRAN { get; set; }
        public string PIP_DRVLIC { get; set; }
        public string PIP_MILITARY_STATUS { get; set; }
        // public string PIP_YOUTH { get; set; }
        public string PIP_PREGNANT { get; set; }
        public string PIP_HEALTH_CODES { get; set; }
        public string PIP_NCASHBEN { get; set; }
        public string PIP_INCOME_TYPES { get; set; }
        //  public string PIP_ADDRESS { get; set; }
        public string PIP_AREA { get; set; }
        public string PIP_HOME_PHONE { get; set; }
        public string PIP_CELL_NUMBER { get; set; }
        public string PIP_HOUSENO { get; set; }
        public string PIP_DIRECTION { get; set; }
        public string PIP_STREET { get; set; }
        public string PIP_SUFFIX { get; set; }
        public string PIP_APT { get; set; }
        public string PIP_FLR { get; set; }
        public string PIP_CITY { get; set; }
        public string PIP_STATE { get; set; }
        public string PIP_ZIP { get; set; }
        public string PIP_TOWNSHIP { get; set; }
        public string PIP_COUNTY { get; set; }
        public string PIP_SEQ { get; set; }
        public string PIP_STATUS { get; set; }
        public string PIP_CAP_AGENCY { get; set; }
        public string PIP_DEPT { get; set; }
        public string PIP_PROGRAM { get; set; }
        public string PIP_YEAR { get; set; }
        public string PIP_APP_NO { get; set; }
        public string PIP_DBNAME { get; set; }
        public string PIP_AGENCY { get; set; }
        public string PIP_AGY { get; set; }
        public string PIP_DRAGGED { get; set; }
        public string PIP_ADD_DATE { get; set; }
        public string PIP_CHNG_DATE { get; set; }
        public string PIP_SERVICES { get; set; }
        public string PIP_ID_MODE { get; set; }
        public string PIP_USER_ID_MODE { get; set; }
        public string PIP_CODE_MODE { get; set; }
        public string PIP_SSN_MODE { get; set; }
        public string PIP_EMAIL_MODE { get; set; }
        public string PIP_FNAME_MODE { get; set; }
        public string PIP_MNAME_MODE { get; set; }
        public string PIP_LNAME_MODE { get; set; }
        public string PIP_DOB_MODE { get; set; }
        public string PIP_GENDER_MODE { get; set; }
        public string PIP_MARITAL_STATUS_MODE { get; set; }
        public string PIP_RELATIONSHIP_MODE { get; set; }
        public string PIP_ETHNIC_MODE { get; set; }
        public string PIP_RACE_MODE { get; set; }
        public string PIP_EDUCATION_MODE { get; set; }
        public string PIP_DISABLE_MODE { get; set; }
        public string PIP_WORK_STAT_MODE { get; set; }
        public string PIP_PRI_LANGUAGE_MODE { get; set; }
        public string PIP_FAMILY_TYPE_MODE { get; set; }
        public string PIP_HOUSING_MODE { get; set; }
        public string PIP_SCHOOL_MODE { get; set; }
        public string PIP_HEALTH_INS_MODE { get; set; }
        public string PIP_VETERAN_MODE { get; set; }
        public string PIP_FOOD_STAMP_MODE { get; set; }
        public string PIP_FARMER_MODE { get; set; }
        public string PIP_WIC_MODE { get; set; }
        public string PIP_RELITRAN_MODE { get; set; }
        public string PIP_DRVLIC_MODE { get; set; }
        public string PIP_MILITARY_STATUS_MODE { get; set; }
        public string PIP_YOUTH_MODE { get; set; }
        public string PIP_PREGNANT_MODE { get; set; }
        public string PIP_HEALTH_CODES_MODE { get; set; }
        public string PIP_NCASHBEN_MODE { get; set; }
        public string PIP_INCOME_TYPES_MODE { get; set; }
        public string PIP_ADDRESS_MODE { get; set; }
        public string PIP_AREA_MODE { get; set; }
        public string PIP_HOME_PHONE_MODE { get; set; }
        public string PIP_CELL_NUMBER_MODE { get; set; }
        public string PIP_HOUSENO_MODE { get; set; }
        public string PIP_DIRECTION_MODE { get; set; }
        public string PIP_STREET_MODE { get; set; }
        public string PIP_SUFFIX_MODE { get; set; }
        public string PIP_APT_MODE { get; set; }
        public string PIP_FLR_MODE { get; set; }
        public string PIP_CITY_MODE { get; set; }
        public string PIP_STATE_MODE { get; set; }
        public string PIP_ZIP_MODE { get; set; }
        public string PIP_TOWNSHIP_MODE { get; set; }
        public string PIP_COUNTY_MODE { get; set; }
        public string PIP_PRECINT_MODE { get; set; }
        public string PIP_SEQ_MODE { get; set; }
        public string PIP_STATUS_MODE { get; set; }
        public string PIP_AGENCY_MODE { get; set; }
        public string PIP_DEPT_MODE { get; set; }
        public string PIP_PROGRAM_MODE { get; set; }
        public string PIP_YEAR_MODE { get; set; }
        public string PIP_APP_NO_MODE { get; set; }
        public string PIP_DBNAME_MODE { get; set; }
        public string PIP_DRAGGED_MODE { get; set; }
        public string PIP_ADD_DATE_MODE { get; set; }
        public string PIP_CHNG_DATE_MODE { get; set; }
        public string PIP_SERVICES_MODE { get; set; }
        public int PIP_Valid_MODE { get; set; }
        public string Mode { get; set; }
        public string PIP_MEMBER_TYPE { get; set; }
        public List<ListItem> Pip_service_list_Mode = new List<ListItem>();
        public List<ListItem> Pip_Income_list_Mode = new List<ListItem>();
        public List<ListItem> Pip_NonCashbenefit_list_Mode = new List<ListItem>();
        public List<ListItem> Pip_Healthcodes_list_Mode = new List<ListItem>();
        public string PIP_Precint { get; set; }


        #endregion
    }

    public class PARTNERDOCSEntity
    {

        #region Constructors

        public PARTNERDOCSEntity()
        {
            PRED_ID = string.Empty;
            PRED_OPERATN = string.Empty;
            PRED_FILENAME = string.Empty;
            PRED_DESC = string.Empty;
            PRED_REFER = string.Empty;
            PRED_ULOADAS = string.Empty;
            PRED_USER = string.Empty;
            PRED_TMDATE = string.Empty;

        }

        public PARTNERDOCSEntity(DataRow row)
        {
            if (row != null)
            {
                PRED_ID = row["PRED_ID"].ToString().Trim();
                PRED_OPERATN = row["PRED_OPERATN"].ToString().Trim();
                PRED_FILENAME = row["PRED_FILENAME"].ToString().Trim();
                PRED_DESC = row["PRED_DESC"].ToString().Trim();
                PRED_REFER = row["PRED_REFER"].ToString().Trim();
                PRED_ULOADAS = row["PRED_ULOADAS"].ToString().Trim();
                PRED_USER = row["PRED_USER"].ToString().Trim();
                PRED_TMDATE = row["PRED_TMDATE"].ToString().Trim();

            }
        }

        #endregion

        #region Properties
        public string PRED_ID { get; set; }
        public string PRED_OPERATN { get; set; }
        public string PRED_FILENAME { get; set; }
        public string PRED_DESC { get; set; }
        public string PRED_REFER { get; set; }
        public string PRED_ULOADAS { get; set; }
        public string PRED_USER { get; set; }
        public string PRED_TMDATE { get; set; }

        #endregion

    }

}

