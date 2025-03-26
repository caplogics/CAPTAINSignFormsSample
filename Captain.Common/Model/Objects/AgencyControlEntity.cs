using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class AgencyControlEntity
    {
        #region Constructors

        public AgencyControlEntity()
        {
                     
            Street = string.Empty;
            City = string.Empty;
            Zip1 = string.Empty;
            Zip2 = string.Empty;
            State = string.Empty;
            MainPhone = string.Empty;
            OtherPhone = String.Empty;
            FaxNumbe = string.Empty;
            HoursFrom = string.Empty;
            MinsFrom = string.Empty;
            AmPmFrom = string.Empty;
            HoursTo = string.Empty;
            MinsTo = string.Empty;
            AmPmTo = string.Empty;
            VoucherNo = string.Empty;
            FuelPayNo = string.Empty;
            FuelCheckNo = string.Empty;
            FuelCheckDate = string.Empty;
            FuelVoucherNo = string.Empty;
            AgencySystemID = string.Empty;
            AgencyID = string.Empty;
            EditZip = "0";
            ClearCaprep = "0";
            SearchDataBase = string.Empty;
            SearchBy = string.Empty;
            SearchFor = string.Empty;
            SearchCaseType = string.Empty;
            CheckDate = string.Empty;
            CasemngrCombo = string.Empty;
            ServinqCaseHie = string.Empty;
            Path = string.Empty;
            NextService = string.Empty;
            ImportPath = string.Empty;
            ExportPath = string.Empty;
            LastModifiedDate = string.Empty;
            LastOperator = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            AgencyCode = string.Empty;
            Mode = string.Empty;
            AgyName = string.Empty;
            Tmsb20 = string.Empty;
            XMLPath = string.Empty;
            XMLHierarchy = string.Empty;
            CAPVoucher = string.Empty;
            TaxExemption = string.Empty;
            ForceAllPwdBy = string.Empty;
            ForceAllPwdOn = string.Empty;
            AcrVocherNo = string.Empty;           
            IncVerfication = string.Empty;
            Inccntl18noinc = string.Empty;
            IncMethods = string.Empty;
            MainEXT = string.Empty;
            CaseNotesstamp = string.Empty;
            ForcePwd = string.Empty;
            ForcePwdDays = string.Empty;
            AgencySystemID2 = string.Empty;
            AllowClientINQ = string.Empty;
            MatAssesment = string.Empty;
            SSNPoint = DOBPoint = FirstNamePoint = LastNamePoint = SSN = DOB = FirstName = LastName = ClientRules = string.Empty;
             SearchHit = string.Empty;
             SearchRating = string.Empty; SiteSecurity = string.Empty;
            RefConn = string.Empty;
            SitesData = string.Empty;
            DelAppSwitch = string.Empty;
            RomaSwitch = string.Empty;
            DefIntakeDtSwitch = string.Empty;
            PrintBatchNo = string.Empty;
            ReportSwitch = string.Empty;
            VerSwitch = string.Empty;
            CAOBO = string.Empty;
            SearchCurAgySwitch = string.Empty;
            ProgressNotesSwitch = string.Empty;
            DeepSearchSwitch = string.Empty;
            CentralHierarchy = string.Empty;
            SpanishSwitch = string.Empty;
            PIPSwitch = string.Empty;
            MailAddressSwitch = string.Empty;
            FTypeSwitch = string.Empty;
            QuickPostServices = string.Empty;
        

            PaymentVoucherNumber = string.Empty;
            WipeBMALSP = string.Empty;
            BenefitFrom = string.Empty;
            ClidSmash = string.Empty;
            ClidYear = string.Empty;
            ClidFrom = string.Empty;
            ClidTo = string.Empty;
            ClidSSN = string.Empty;
            ClidClid = string.Empty;
            ClidDateStamp = string.Empty;
            MemberActivity = string.Empty;
            TMS201SoftEdit = string.Empty;
            ShowIntakeSwitch = string.Empty;
            MostRecentintake = string.Empty;
            PaymentCategorieService = string.Empty;
            ServicePlanHiecontrol = string.Empty;
            LoginMFA = string.Empty;
            SerPlanAllow = string.Empty;
            SsnDobMMenu = string.Empty;
            BulkpostTemp = string.Empty;
            WorkerFUP = string.Empty;
            LnkAppSwitch = string.Empty;
            AgyVendor = string.Empty;
            ReverseFeed=string.Empty;
            TXAlienSwitch=string.Empty;
            HealthInsSingleSel = string.Empty;
            // CalQuescontprogram = string.Empty;
            CEAPPostUsage = string.Empty;
            //CEAPStrtChk = string.Empty;

            Search_Controls = string.Empty;
            ACR_POV_WITH_DEC = string.Empty;
            ACR_POV_DONT_RNDUP = string.Empty;
        }

        public AgencyControlEntity(DataRow row)
        {
            if (row != null)
            {

                Street = row["ACR_STREET"].ToString();
                City = row["ACR_CITY"].ToString();
                Zip1 = row["ACR_ZIP1"].ToString();
                Zip2 = row["ACR_ZIP2"].ToString();
                State = row["ACR_STATE"].ToString();
                MainPhone = row["ACR_MAIN_PHONE"].ToString();
                OtherPhone = row["ACR_OTHER_PHONE"].ToString();
                FaxNumbe = row["ACR_FAX_NUMBER"].ToString();
                HoursFrom = row["ACR_HOURS_FROM"].ToString();
                HoursTo = row["ACR_HOURS_TO"].ToString();
                VoucherNo = row["ACR_VOUCHER_NO"].ToString();
                FuelPayNo = row["ACR_FUEL_PAY_NO"].ToString();
                FuelCheckNo = row["ACR_FUEL_CHECK_NO"].ToString();
                FuelCheckDate = row["ACR_FUEL_CHECK_DATE"].ToString();
                FuelVoucherNo = row["ACR_FUEL_VOUCH_NO"].ToString();
                AgencySystemID = row["ACR_XML_AGENCYSYSTEM_ID"].ToString();
                AgencyID = row["ACR_XML_AGENCY_ID"].ToString();
                EditZip = row["ACR_EDIT_ZIP"].ToString();
                ClearCaprep = row["ACR_CLEAR_CAPREP"].ToString();
                SearchDataBase = row["ACR_SEARCH_DATABASE"].ToString();
                SearchBy = row["ACR_SEARCH_BY"].ToString();
                SearchFor = row["ACR_SEARCH_FOR"].ToString();
                SearchCaseType = row["ACR_SEARCH_CASETYPE"].ToString();
                CheckDate = row["ACR_CHECK_DATE"].ToString();
                Path = row["ACR_PATH"].ToString();
                NextService = row["ACR_NEXT_SERVICE"].ToString();
                ImportPath = row["ACR_IMP_PATH"].ToString();
                ExportPath = row["ACR_EXP_PATH"].ToString();
                LastModifiedDate = row["ACR_DATE_LSTC"].ToString();
                LastOperator = row["ACR_LSTC_OPERATOR"].ToString();
                AddDate = row["ACR_DATE_ADD"].ToString();
                AddOperator = row["ACR_ADD_OPERATOR"].ToString();
                AgencyCode = row["ACR_AGENCY_CODE"].ToString();
                CasemngrCombo = row["ACR_CASEMNGR_COMBO"].ToString();
                ServinqCaseHie = row["ACR_SERVINQ_CASEHIE"].ToString();
                AgyShortName = row["ACR_SHORT_NAME"].ToString();
                AgyName = row["ACR_NAME"].ToString();
                Tmsb20 = row["ACR_FUEL_TMSB20"].ToString();
                XMLPath = row["ACR_XML_PATH"].ToString();
                XMLHierarchy = row["ACR_XML_HIERARCHY"].ToString();
                CAPVoucher = row["ACR_CA_PVOUCHER"].ToString();
                TaxExemption = row["ACR_CA_TAX_EXMNO"].ToString();
                ForceAllPwdBy = row["ACR_FORCE_ALL_PWD_BY"].ToString();
                ForceAllPwdOn = row["ACR_FORCE_ALL_PWD_ON"].ToString();
                AcrVocherNo = row["ACR_NXT_VOUCHNO"].ToString();
                IncVerfication = row["ACR_INC_VER"].ToString();
                Inccntl18noinc = row["ACR_18_NOINC"].ToString();
                MainEXT = row["ACR_MAIN_EXT"].ToString();
                IncMethods = row["ACR_INC_METHODS"].ToString();
                CaseNotesstamp = row["ACR_CASENOTES_STAMP"].ToString();
                ForcePwd = row["ACR_FORCE_PWD"].ToString();
                ForcePwdDays = row["ACR_FORCE_PWD_DAYS"].ToString().Trim() == string.Empty ? "0" : row["ACR_FORCE_PWD_DAYS"].ToString();
                AgencySystemID2 = row["ACR_XML_AGENCYSYSTEM_ID2"].ToString();
                AllowClientINQ = row["ACR_ALLOW_CILENT_INQ"].ToString();
                MatAssesment = row["ACR_MAT_IND_ASSMNTS"].ToString();
                SSN = row["ACR_SSN"].ToString();
                DOB = row["ACR_DOB"].ToString();
                FirstName = row["ACR_FIRSTNAME"].ToString();
                LastName = row["ACR_LASTNAME"].ToString();
                ClientRules = row["ACR_CLIENT_RULES"].ToString();
                SSNPoint = row["ACR_SSN_Point"].ToString();
                DOBPoint = row["ACR_DOB_Point"].ToString();
                FirstNamePoint = row["ACR_FIRSTNAME_Point"].ToString();
                LastNamePoint = row["ACR_LASTNAME_Point"].ToString();
                DOBLastNamePoint = row["ACR_DOBLNAME_Point"].ToString();
                SSNLastNamePoint = row["ACR_SSNLNAME_Point"].ToString();
                RefConn = row["ACR_REF_CONN"].ToString();
                SearchHit = row["ACR_SEARCH_HIT"].ToString();
                SearchRating = row["ACR_SEARCH_RATING"].ToString();
                SiteSecurity = row["ACR_SITE_SEC"].ToString();
                SitesData = string.Empty;
                DelAppSwitch = row["ACR_APPDEL_SWITCH"].ToString();
                RomaSwitch = row["ACR_ROMA_SWITCH"].ToString();
                DefIntakeDtSwitch = row["ACR_INTAKEDT_SWITCH"].ToString();
                ReportSwitch = row["ACR_REPORT_SWITCH"].ToString();
                DOBFirstNamePoint = row["ACR_DOBFNAME_Point"].ToString();
                VerSwitch = row["ACR_INC_VERSWITCH"].ToString();
                CAOBO = row["ACR_CAOBO"].ToString();
                SearchCurAgySwitch = row["ACR_CUR_AGYSWITCH"].ToString();
                ProgressNotesSwitch = row["ACR_PRONOTES_SWITCH"].ToString();
                DeepSearchSwitch = row["ACR_DEEP_SEARCH"].ToString();

                CentralHierarchy= row["ACR_CENT_HIE"].ToString();

                SpanishSwitch = row["ACR_SPAN_SWITCH"].ToString();

                PIPSwitch = row["ACR_PIP_SWITCH"].ToString();

                MailAddressSwitch = row["ACR_MAIL_ADSSWITCH"].ToString();

                FTypeSwitch = row["ACR_FTYPE_SWITCH"].ToString();

                QuickPostServices = row["ACR_QK_SER"].ToString();

                PaymentVoucherNumber= row["ACR_NXT_VOUCHNO"].ToString();

                WipeBMALSP = row["ACR_WIPE_BMALPS"].ToString();

                BenefitFrom = row["ACR_BENEFIT_FROM"].ToString();

                ClidSmash = row["ACR_CLID_SMASH"].ToString();
                ClidYear = row["ACR_CLID_YEAR"].ToString();
                ClidFrom = row["ACR_CLID_FROM"].ToString();
                ClidTo = row["ACR_CLID_TO"].ToString();
                ClidSSN = row["ACR_CLID_SSN"].ToString();
                ClidClid = row["ACR_CLID_CLID"].ToString();
                ClidDateStamp = row["ACR_CLID_DATESTAMP"].ToString();
                FamilyIdHie = row["ACR_FIXFAMID_HIE"].ToString();
                FamilyIdDuplvl = row["ACR_FIXFAMID_DUPLVL"].ToString();
                MemberActivity  = row["ACR_MEM_ACTIVTY"].ToString();
                TMS201SoftEdit = row["ACR_VEND_ACCT_SOFT_EDIT"].ToString();
                ShowIntakeSwitch = row["ACR_SHOW_INTHIE"].ToString();
                MostRecentintake = row["ACR_RECENT_INTAKE"].ToString();
                PaymentCategorieService = row["ACR_PAYCAT_SERPOST"].ToString();
                ServicePlanHiecontrol = row["ACR_SERPLAN_HIES"].ToString();
                LoginMFA = row["ACR_LOGIN_MFA"].ToString();
                SerPlanAllow = row["ACR_SERPLAN_ALLOW"].ToString();
                SsnDobMMenu = row["ACR_SSNDOB_MMenu"].ToString();
                BulkpostTemp = row["ACR_BULKPOST_TMPLT"].ToString();
                WorkerFUP = row["ACR_WRKR_FUP"].ToString();
                LnkAppSwitch = row["ACR_LNKAPP"].ToString();
                AgyVendor = row["ACR_AGY_VEND"].ToString();
                ReverseFeed = row["ACR_REVFEED"].ToString();
                TXAlienSwitch = row["ACR_TX_ALIEN_FORM"].ToString();
                // CalQuescontprogram = row["ACR_QUES_PROG"].ToString();

                HealthInsSingleSel = row["ACR_HEALTH_INS_SEL"].ToString();

                CEAPPostUsage = row["ACR_CEAP_USAGE"].ToString();
                //CEAPStrtChk = row["ACR_CEAP_STRTCHK"].ToString();

                Search_Controls = row["ACR_SEARCH_CNTL"].ToString();
                ACR_POV_WITH_DEC = row["ACR_POV_WITH_DEC"].ToString();
                ACR_POV_DONT_RNDUP = row["ACR_POV_DONT_RNDUP"].ToString();
            }
        }
        #endregion

        #region Properties

   
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip1 { get; set; }
        public string Zip2 { get; set; }
        public string State { get; set; }
        public string MainPhone { get; set; }
        public string OtherPhone { get; set; }
        public string FaxNumbe { get; set; }
        public string HoursFrom { get; set; }
        public string MinsFrom { get; set; }
        public string AmPmFrom { get; set; }
        public string HoursTo { get; set; }
        public string MinsTo { get; set; }
        public string AmPmTo { get; set; }
        public string VoucherNo { get; set; }
        public string FuelPayNo { get; set; }
        public string FuelCheckNo { get; set; }
        public string FuelCheckDate { get; set; }
        public string FuelVoucherNo { get; set; }
        public string AgencySystemID { get; set; }
        public string AgencyID { get; set; }
        public string EditZip { get; set; }
        public string ClearCaprep { get; set; }
        public string SearchDataBase { get; set; }
        public string SearchBy { get; set; }
        public string SearchFor { get; set; }
        public string SearchCaseType { get; set; }
        public string CheckDate { get; set; }
        public string Path { get; set; }
        public string NextService { get; set; }
        public string ImportPath { get; set; }
        public string ExportPath { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastOperator { get; set; }
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string AgencyCode { get; set; }
        public string Mode { get; set; }
        public string ServinqCaseHie { get; set; }
        public string CasemngrCombo { get; set; }
        public string AgyShortName { get; set; }
        public string AgyName { get; set; }
        public string Tmsb20 { get; set; }
        public string XMLPath { get; set; }
        public string XMLHierarchy { get; set; }
        public string CAPVoucher { get; set; }
        public string TaxExemption { get; set; }
        public string ForceAllPwdBy { get; set; }
        public string ForceAllPwdOn { get; set; }
        public string AcrVocherNo { get; set; }      
        public string IncVerfication { get; set; }
        public string Inccntl18noinc { get; set; }
        public string IncMethods { get; set; }
        public string MainEXT { get; set; }
        public string CaseNotesstamp { get; set; }
        public string ForcePwd { get; set; }
        public string ForcePwdDays { get; set; }
        public string AgencySystemID2 { get; set; }
        public string AllowClientINQ { get; set; }
        public string MatAssesment { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSNPoint { get; set; }
        public string DOBPoint { get; set; }
        public string FirstNamePoint { get; set; }
        public string LastNamePoint { get; set; }
        public string DOBLastNamePoint { get; set; }
        public string SSNLastNamePoint { get; set; }
        public string ClientRules { get; set; }
        public string RefConn { get; set; }
        public string SearchHit { get; set; }
        public string SearchRating { get; set; }
        public string SiteSecurity { get; set; }
        public string SitesData { get; set; }
        public string DelAppSwitch { get; set; }
        public string RomaSwitch { get; set; }
        public string DefIntakeDtSwitch { get; set; }
        public string PrintBatchNo { get; set; }
        public string ReportSwitch { get; set; }
        public string DOBFirstNamePoint { get; set; }
        public string VerSwitch { get; set; }
        public string CAOBO { get; set; }
        public string SearchCurAgySwitch { get; set; }
        public string ProgressNotesSwitch { get; set; }
        public string DeepSearchSwitch { get; set; }
        public string CentralHierarchy { get; set; }
        public string SpanishSwitch { get; set; }
        public string PIPSwitch { get; set; }
        public string MailAddressSwitch { get; set; }
        public string FTypeSwitch { get; set; }
        public string QuickPostServices { get; set; }

        public string PaymentVoucherNumber { get; set; }
        public string WipeBMALSP { get; set; }
        public string BenefitFrom { get; set; }

        public string ClidSmash { get; set; }
        public string ClidYear { get; set; }
        public string ClidFrom { get; set; }
        public string ClidTo { get; set; }
        public string ClidSSN { get; set; }
        public string ClidClid { get; set; }
        public string ClidDateStamp { get; set; }
        public string FamilyIdHie { get; set; }
        public string FamilyIdDuplvl { get; set; }
        public string MemberActivity { get; set; }
        public string TMS201SoftEdit { get; set; }
        public string ShowIntakeSwitch { get; set; }
        public string MostRecentintake { get; set; }
        public string PaymentCategorieService { get; set; }
        public string ServicePlanHiecontrol { get; set; }
        public string LoginMFA { get; set; }
        public string SerPlanAllow { get; set; }
        public string SsnDobMMenu { get; set; }
        public string BulkpostTemp { get; set; }
        public string WorkerFUP { get; set; }
        public string LnkAppSwitch { get; set; }

        public string AgyVendor { get; set; }

        public string ReverseFeed { get; set; }

        public string TXAlienSwitch { get; set; }

        public string HealthInsSingleSel { get; set; }

        public string CEAPPostUsage { get; set; }

        public string Search_Controls
        {
            get;
            set;
        }

        public string ACR_POV_WITH_DEC { get; set; }
        public string ACR_POV_DONT_RNDUP { get; set; }


        //public string CEAPStrtChk { get; set; }


        // public string CalQuescontprogram { get; set; }
        #endregion

    }
}
