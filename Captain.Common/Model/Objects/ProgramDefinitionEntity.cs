/**********************************************************************************************************
 * Class Name   : HierarchyEntity
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Entity object to extend ObjectUsersType.
 **********************************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

#endregion

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class ProgramDefinitionEntity
    {
        #region Constructors

        public ProgramDefinitionEntity()
        {
            AgencyName = string.Empty;
            DeptName = string.Empty;
            ProgramName = string.Empty;
            Mode = string.Empty;
            Code = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            ShortName = string.Empty;
            DepAGCY = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            Address3 = string.Empty;
            City = string.Empty;
            Phone = string.Empty;
            UsedFlag = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            ZipPlus = string.Empty;
            DepProg = string.Empty;
            DepYear = string.Empty;
            DepFirstName = string.Empty;
            DepMI = string.Empty;
            DepLastName = string.Empty;
            DepGNO = string.Empty;
            DepDI = string.Empty;
            DepAppNo = string.Empty;
            DepGenerateApps = string.Empty;
            DepAddressEdit = string.Empty;
            DepThreshold = string.Empty;
            DepAutoRefer = string.Empty;
            DepJUVFromAge = string.Empty;
            DepJUVToAge = string.Empty;
            DepSENFromAge = string.Empty;
            DepSENToAge = string.Empty;
            DepFEDUsed = string.Empty;
            DepCMIUsed = string.Empty;
            DepSMIUsed = string.Empty;
            DepHUDUsed = string.Empty;
            DepINCLSIM = string.Empty;
            DepIncomeTypes = string.Empty;
            DepReleataionTypes = string.Empty;
            DepCalcEligibility = string.Empty;
            DepUnitCalc = string.Empty;
            Account = string.Empty;
            Free1 = string.Empty;
            Free2 = string.Empty;
            Free3 = string.Empty;
            Free4 = string.Empty;
            Free5 = string.Empty;
            Free6 = string.Empty;
            Reduced1 = string.Empty;
            Reduced2 = string.Empty;
            Reduced3 = string.Empty;
            Reduced4 = string.Empty;
            Reduced5 = string.Empty;
            Reduced6 = string.Empty;
            Paid1 = string.Empty;
            Paid2 = string.Empty;
            Paid3 = string.Empty;
            Paid4 = string.Empty;
            Paid5 = string.Empty;
            Paid6 = string.Empty;
            Type = string.Empty;
            Source = string.Empty;
            HSS = string.Empty;
            PRODUPSSN = string.Empty;
            StartMonth = string.Empty;
            EndMonth = string.Empty;
            CaseTypeEdit = string.Empty;
            SelectedClient = string.Empty;
            IncomeVerMsg = string.Empty;
            IncomeTypeOnly = string.Empty;
            ProDupMEM = string.Empty;
            IncomeWeek = string.Empty;
            ZipSearch = string.Empty;
            AutoInActivation = string.Empty;
            SSNReasonFlag = string.Empty;
            SIMPointsMethod = string.Empty;
            IntakeEdit = string.Empty;
            IntakeFDate = string.Empty;
            IntakeTDate = string.Empty;
            LoadProgramQuestions = string.Empty;
            SecretProgram = string.Empty;
            FuelCheckNo = string.Empty;
            FuelPayNo = string.Empty;
            FuelCheckDate = string.Empty;
            FuelVoucherNo = string.Empty;
            IncomeDateValidate = string.Empty;
            DepIncExcIntUsed1 = string.Empty;
            DepIncExcIntUsed2 = string.Empty;
            DepIncExcIntUsed3 = string.Empty;
            DepIncExcIntDefault1 = string.Empty;
            DepIncExcIntDefault2 = string.Empty;
            DepIncExcIntDefault3 = string.Empty;
            DepIncExcIntFactr1 = string.Empty;
            DepIncExcIntFactr2 = string.Empty;
            DepIncExcIntFactr3 = string.Empty;
            DepIncludeIncVer = string.Empty;        
            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            DepIntakeProg = string.Empty;
            DepFax = string.Empty;
            DepAttendanceTimes = string.Empty;
            DepSsnAutoAssign = string.Empty;
            DepSerpostPAYCAT = string.Empty;
            PRODUPAPP_BY = string.Empty;
            PRODUPMEM_BY = String.Empty;

            emailID = string.Empty;
            //Search_Cntrl = string.Empty;
        }

        public ProgramDefinitionEntity(DataRow userHierarchy)
        {
            if (userHierarchy != null)
            {
                DataRow row = userHierarchy;
                AgencyName = row["AGENCYNAME"].ToString();
                DeptName   = row["DEPTNAME"].ToString();
                ProgramName = row["PROGRAMNAME"].ToString();
                Agency = row["DEP_AGENCY"].ToString();
                Dept   = row["DEP_DEPT"].ToString();
                Prog   = row["DEP_PROGRAM"].ToString();
                ShortName = row["DEP_SHORT_NAME"].ToString();
                DepAGCY = row["DEP_AGCY"].ToString();
                Address1 = row["DEP_ADDR1"].ToString();
                Address2 = row["DEP_ADDR2"].ToString();
                Address3 = row["DEP_ADDR3"].ToString();
                City = row["DEP_CITY"].ToString();
                Phone = row["DEP_PHONE"].ToString();
                //UsedFlag = row["DEP_STATE"].ToString();
                State = row["DEP_STATE"].ToString();
                Zip = row["DEP_ZIP"].ToString();
                ZipPlus = row["DEP_ZIP_PLUS"].ToString();
                DepProg = row["DEP_PROG"].ToString();
                DepYear = row["DEP_YEAR"].ToString();
                DepFirstName = row["DEP_D_FN"].ToString();
                DepMI = row["DEP_D_MI"].ToString();
                DepLastName = row["DEP_D_LN"].ToString();
                DepGNO = row["DEP_G_NO"].ToString();
                DepDI = row["DEP_DI"].ToString();
                DepAppNo = row["DEP_APP_NO"].ToString();
                DepGenerateApps = row["DEP_GENERATE_APPS"].ToString();
                DepAddressEdit = row["DEP_ADDRESS_EDIT"].ToString();
                DepThreshold = row["DEP_THRESHOLD"].ToString();
                DepAutoRefer = row["DEP_AUTO_REFER"].ToString();
                DepJUVFromAge = row["DEP_JUV_FROM_AGE"].ToString();
                DepJUVToAge = row["DEP_JUV_TO_AGE"].ToString();
                DepSENFromAge = row["DEP_SEN_FROM_AGE"].ToString();
                DepSENToAge = row["DEP_SEN_TO_AGE"].ToString();
                DepFEDUsed = row["DEP_FED_USED"].ToString();
                DepCMIUsed = row["DEP_CMI_USED"].ToString();
                DepSMIUsed = row["DEP_SMI_USED"].ToString();
                DepHUDUsed = row["DEP_HUD_USED"].ToString();

                DepINCLSIM = row["DEP_INCL_SIM"].ToString();
                DepIncomeTypes = row["DEP_INCOME_ARRAY"].ToString();
                DepReleataionTypes = row["DEP_RELATION_ARRAY"].ToString();
                DepCalcEligibility = row["DEP_CALC_ELIG"].ToString();
                DepUnitCalc = row["DEP_UNIT_CALC"].ToString();
                Account = row["CNTL_ACCOUNT"].ToString();
                Free1 = row["CNTL_FREE_1"].ToString();
                Free2 = row["CNTL_FREE_2"].ToString();
                Free3 = row["CNTL_FREE_3"].ToString();
                Free4 = row["CNTL_FREE_4"].ToString();
                Free5 = row["CNTL_FREE_5"].ToString();
                Free6 = row["CNTL_FREE_6"].ToString();
                Reduced1 = row["CNTL_REDUCED_1"].ToString();
                Reduced2 = row["CNTL_REDUCED_2"].ToString();
                Reduced3 = row["CNTL_REDUCED_3"].ToString();
                Reduced4 = row["CNTL_REDUCED_4"].ToString();
                Reduced5 = row["CNTL_REDUCED_5"].ToString();
                Reduced6 = row["CNTL_REDUCED_6"].ToString();
                Paid1 = row["CNTL_PAID_1"].ToString();
                Paid2 = row["CNTL_PAID_2"].ToString();
                Paid3 = row["CNTL_PAID_3"].ToString();
                Paid4 = row["CNTL_PAID_4"].ToString();
                Paid5 = row["CNTL_PAID_5"].ToString();
                Paid6 = row["CNTL_PAID_6"].ToString();
                Type = row["CNTL_TYPE"].ToString();
                Source = row["CNTL_SOURCE"].ToString();
                HSS = row["CASEDEP_HSS"].ToString();
                PRODUPSSN = row["DEP_PRO_DUP_SSN"].ToString();
                StartMonth = row["CASEDEP_STARTMONTH"].ToString();
                EndMonth = row["CASEDEP_ENDMONTH"].ToString();
                CaseTypeEdit = row["DEP_CASETYPE_EDIT"].ToString();
                SelectedClient = row["DEP_SELECTED_CLIENT"].ToString();
                IncomeVerMsg = row["DEP_INC_VER_MSG"].ToString();
                IncomeTypeOnly = row["DEP_INCOME_TYPE_ONLY"].ToString();
                ProDupMEM = row["DEP_PRO_DUP_MEM"].ToString();
                IncomeWeek = row["DEP_INCOME_WEEK"].ToString();
                ZipSearch = row["DEP_ZIP_SRCH"].ToString();
                AutoInActivation = row["DEP_AUTO_INACTIVATION"].ToString();
                SSNReasonFlag = row["DEP_SSN_REASON_FLAG"].ToString();
                SIMPointsMethod = row["DEP_SIM_POINTS_METHOD"].ToString();
                IntakeEdit = row["DEP_INTAKE_EDIT"].ToString();
                IntakeFDate = row["DEP_INTAKE_FDATE"].ToString();
                IntakeTDate = row["DEP_INTAKE_TDATE"].ToString();
                LoadProgramQuestions = row["DEP_LOAD_PROGRAM_QUESTIONS"].ToString();
                SecretProgram = row["DEP_SECRET_PROGRAM"].ToString();
                FuelCheckNo = row["DEP_FUEL_CHECK_NO"].ToString();
                FuelPayNo = row["DEP_FUEL_PAY_NO"].ToString();
                FuelCheckDate = row["DEP_FUEL_CHECK_DATE"].ToString();
                FuelVoucherNo = row["DEP_FUEL_VOUCH_NO"].ToString();
                IncomeDateValidate = row["DEP_INCOME_DATE_VALIDATE"].ToString();
                DepIncExcIntUsed1 = row["DEP_INC_EXC_INT_USED_1"].ToString();
                DepIncExcIntUsed2 = row["DEP_INC_EXC_INT_USED_2"].ToString();
                DepIncExcIntUsed3 = row["DEP_INC_EXC_INT_USED_3"].ToString();
                DepIncExcIntDefault1 = row["DEP_INC_EXC_INT_DEFLT_1"].ToString();
                DepIncExcIntDefault2 = row["DEP_INC_EXC_INT_DEFLT_2"].ToString();
                DepIncExcIntDefault3 = row["DEP_INC_EXC_INT_DEFLT_3"].ToString();
                DepIncExcIntFactr1 = row["DEP_INC_EXC_INT_FACTR_1"].ToString();
                DepIncExcIntFactr2 = row["DEP_INC_EXC_INT_FACTR_2"].ToString();
                DepIncExcIntFactr3 = row["DEP_INC_EXC_INT_FACTR_3"].ToString();
                DepIncludeIncVer = row["DEP_INCLD_INCVER"].ToString();               
                DateLSTC = row["DEP_DATE_LSTC"].ToString();
                LSTCOperator = row["DEP_LSTC_OPERATOR"].ToString();
                DateAdd = row["DEP_DATE_ADD"].ToString();
                AddOperator = row["DEP_ADD_OPERATOR"].ToString();
                DepHsAgeMethod = row["DEP_HS_AGE_METHOD"].ToString();
                DepIntakeProg = row["DEP_INTAKE_PROG"].ToString();
                DepFax = row["DEP_FAX"].ToString();
                DepAttendanceTimes = row["DEP_ATT_TIMES"].ToString();
                DepSsnAutoAssign = row["DEP_SSN_AUTOASSIGN"].ToString();
                DepSerpostPAYCAT = row["DEP_SERPOST_PAYCAT"].ToString();
                PRODUPAPP_BY = row["DEP_PRO_DUP_APP_BY"].ToString();
                PRODUPMEM_BY = row["DEP_PRO_DUP_MEM_BY"].ToString();

                emailID = row["DEP_EMAIL"].ToString();

                //Search_Cntrl = row["DEP_SEARCH_CNTL"].ToString();
            }
        }
      
        #endregion

        #region Properties

        public string AgencyName { get; set; }
        public string DeptName { get; set; }
        public string ProgramName { get; set; }
        public string Mode { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string ShortName { get; set; }
        public string DepAGCY { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string UsedFlag { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipPlus { get; set; }
        public string DepProg { get; set; }
        public string DepYear { get; set; }
        public string DepFirstName { get; set; }
        public string DepMI { get; set; }
        public string DepLastName { get; set; }
        public string DepGNO { get; set; }
        public string DepDI { get; set; }
        public string DepAppNo { get; set; }
        public string DepGenerateApps { get; set; }
        public string DepAddressEdit { get; set; }
        public string DepThreshold { get; set; }
        public string DepAutoRefer { get; set; }
        public string DepJUVFromAge { get; set; }
        public string DepJUVToAge { get; set; }
        public string DepSENFromAge { get; set; }
        public string DepSENToAge { get; set; }
        public string DepFEDUsed { get; set; }
        public string DepCMIUsed { get; set; }
        public string DepSMIUsed { get; set; }
        public string DepHUDUsed { get; set; }

        public string DepINCLSIM { get; set; }
        public string DepIncomeTypes { get; set; }
        public string DepReleataionTypes { get; set; }
        public string DepCalcEligibility { get; set; }
        public string DepUnitCalc { get; set; }
        public string Account { get; set; }
        public string Free1 { get; set; }
        public string Free2 { get; set; }
        public string Free3 { get; set; }
        public string Free4 { get; set; }
        public string Free5 { get; set; }
        public string Free6 { get; set; }
        public string Reduced1 { get; set; }
        public string Reduced2 { get; set; }
        public string Reduced3 { get; set; }
        public string Reduced4 { get; set; }
        public string Reduced5 { get; set; }
        public string Reduced6 { get; set; }
        public string Paid1 { get; set; }
        public string Paid2 { get; set; }
        public string Paid3 { get; set; }
        public string Paid4 { get; set; }
        public string Paid5 { get; set; }
        public string Paid6 { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string HSS { get; set; }
        public string PRODUPSSN { get; set; }
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public string CaseTypeEdit { get; set; }
        public string SelectedClient { get; set; }
        public string IncomeVerMsg { get; set; }
        public string IncomeTypeOnly { get; set; }
        public string ProDupMEM { get; set; }
        public string IncomeWeek { get; set; }
        public string ZipSearch { get; set; }
        public string AutoInActivation { get; set; }
        public string SSNReasonFlag { get; set; }
        public string SIMPointsMethod { get; set; }
        public string IntakeEdit { get; set; }
        public string IntakeFDate { get; set; }
        public string IntakeTDate { get; set; }
        public string LoadProgramQuestions { get; set; }
        public string SecretProgram { get; set; }
        public string FuelCheckNo { get; set; }
        public string FuelPayNo { get; set; }
        public string FuelCheckDate { get; set; }
        public string FuelVoucherNo { get; set; }
        public string IncomeDateValidate { get; set; }
        public string DepIncExcIntUsed1 { get; set; }
        public string DepIncExcIntUsed2 { get; set; }
        public string DepIncExcIntUsed3 { get; set; }
        public string DepIncExcIntDefault1 { get; set; }
        public string DepIncExcIntDefault2 { get; set; }
        public string DepIncExcIntDefault3 { get; set; }
        public string DepIncExcIntFactr1 { get; set; }
        public string DepIncExcIntFactr2 { get; set; }
        public string DepIncExcIntFactr3 { get; set; }
        public string DepIncludeIncVer { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string DepHsAgeMethod { get; set; }
        public string DepIntakeProg { get; set; }
        public string DepFax { get; set; }
        public string DepAttendanceTimes { get; set; }
        public string DepSsnAutoAssign { get; set; }
        public string DepSerpostPAYCAT { get; set; }

        public string PRODUPAPP_BY { get; set; }
        public string PRODUPMEM_BY { get; set; }

        public string emailID
        {
        get; set;
        }
        //public string Search_Cntrl
        //{
        //    get; set;
        //}

        #endregion

    }
}
