using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class ChldMstEntity
    {

        #region Constructors

        public ChldMstEntity()
        {

            ChldMstAgency = string.Empty;
            ChldMstDept = string.Empty;
            ChldMstProgram = string.Empty;
            ChldMstYr = string.Empty;
            ApplNo = string.Empty;
            FamilySeq = string.Empty;
            FundSource = string.Empty;
            AltFundSrc = string.Empty;
            ClassPrefer = string.Empty;
            Pob = string.Empty;
            BirthCert = string.Empty;
            ChldReat = string.Empty;
            NextYearPrep = string.Empty;
            PreClient = string.Empty;
            Transport = string.Empty;
            MedSecurity = string.Empty;
            MedCoverage = string.Empty;
            MedCoverType = string.Empty;
            MedPlan = string.Empty;
            InsCat = string.Empty;
            MedInsurer = string.Empty;
            DoctorName = string.Empty;
            DoctorAddress = string.Empty;
            DoctorPhone = string.Empty;
            DentalCoverage = string.Empty;
            DentalPlan = string.Empty;
            DentistName = string.Empty;
            DentalInsurer = string.Empty;
            DentistAddress = string.Empty;
            DentistPhone = string.Empty;
            Disability = string.Empty;
            DisabilityType = string.Empty;
            DiagnosisDate = string.Empty;
            Allergies = string.Empty;
            DietaryRes = string.Empty;
            Medications = string.Empty;
            MedicalCond = string.Empty;
            HhConcerns = string.Empty;
            DevlpmntlConcern = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            DateAdd = string.Empty;


        }

        public ChldMstEntity(DataRow row)
        {
            if (row != null)
            {

                ChldMstAgency = row["CHLDMST_AGENCY"].ToString().Trim();
                ChldMstDept = row["CHLDMST_DEPT"].ToString().Trim();
                ChldMstProgram = row["CHLDMST_PROGRAM"].ToString().Trim();
                ChldMstYr = row["CHLDMST_YEAR"].ToString().Trim();
                ApplNo = row["CHLDMST_APP_NO"].ToString().Trim();
                // FamilySeq = row["CHLDMST_FAMILY_SEQ"].ToString().Trim();
                FundSource = row["CHLDMST_FUND_SOURCE"].ToString().Trim();
                AltFundSrc = row["CHLDMST_ALT_FUND_SRC"].ToString().Trim();
                ClassPrefer = row["CHLDMST_CLASS_PREFER"].ToString().Trim();
                Pob = row["CHLDMST_POB"].ToString().Trim();
                BirthCert = row["CHLDMST_BIRTH_CERT"].ToString().Trim();
                ChldReat = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                NextYearPrep = row["CHLDMST_NXTYEAR_PREP"].ToString().Trim();
                PreClient = row["CHLDMST_PRE_CLIENT"].ToString().Trim();
                Transport = row["CHLDMST_TRANSPORT"].ToString().Trim();
                MedSecurity = row["CHLDMST_MED_SECURITY"].ToString().Trim();
                MedCoverage = row["CHLDMST_MED_COVERAGE"].ToString().Trim();
                MedCoverType = row["CHLDMST_MED_COVER_TYPE"].ToString().Trim();
                MedPlan = row["CHLDMST_MED_PLAN"].ToString().Trim();
                InsCat = row["CHLDMST_INS_CAT"].ToString().Trim();
                MedInsurer = row["CHLDMST_MED_INSURER"].ToString().Trim();
                DoctorName = row["CHLDMST_DOCTOR_NAME"].ToString().Trim();
                DoctorAddress = row["CHLDMST_DOCTOR_ADDRESS"].ToString().Trim();
                DoctorPhone = row["CHLDMST_DOCTOR_PHONE"].ToString().Trim();
                DentalCoverage = row["CHLDMST_DENTAL_COVERAGE"].ToString().Trim();
                DentalPlan = row["CHLDMST_DENTAL_PLAN"].ToString().Trim();
                DentistName = row["CHLDMST_DENTIST_NAME"].ToString().Trim();
                DentalInsurer = row["CHLDMST_DENTAL_INSURER"].ToString().Trim();
                DentistAddress = row["CHLDMST_DENTIST_ADDRESS"].ToString().Trim();
                DentistPhone = row["CHLDMST_DENTIST_PHONE"].ToString().Trim();
                Disability = row["CHLDMST_DISABILITY"].ToString().Trim();
                DisabilityType = row["CHLDMST_DISABILITY_TYPE"].ToString().Trim();
                DiagnosisDate = row["CHLDMST_DIAGNOSIS_DATE"].ToString().Trim();
                Allergies = row["CHLDMST_ALLERGIES"].ToString().Trim();
                DietaryRes = row["CHLDMST_DIETARY_RES"].ToString().Trim();
                Medications = row["CHLDMST_MEDICATIONS"].ToString().Trim();
                MedicalCond = row["CHLDMST_MEDICAL_COND"].ToString().Trim();
                HhConcerns = row["CHLDMST_HH_CONCERNS"].ToString().Trim();
                DevlpmntlConcern = row["CHLDMST_DEVLPMNTL_CONCERN"].ToString().Trim();
                AddOperator = row["CHLDMST_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CHLDMST_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CHLDMST_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["CHLDMST_DATE_ADD"].ToString().Trim();
                PickOff = row["CHLDMST_PICKUP"].ToString();
                DropOff = row["CHLDMST_DROPOFF"].ToString();

            }

        }

        public ChldMstEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                if (strTable == "case2530Report")
                {

                    ChldMstAgency = row["CHLDMST_AGENCY"].ToString().Trim();
                    ChldMstDept = row["CHLDMST_DEPT"].ToString().Trim();
                    ChldMstProgram = row["CHLDMST_PROGRAM"].ToString().Trim();
                    ChldMstYr = row["CHLDMST_YEAR"].ToString().Trim();
                    ApplNo = row["CHLDMST_APP_NO"].ToString().Trim();
                    // FamilySeq = row["CHLDMST_FAMILY_SEQ"].ToString().Trim();
                    MedCoverage = row["CHLDMST_MED_COVERAGE"].ToString().Trim();
                    MedCoverType = row["CHLDMST_MED_COVER_TYPE"].ToString().Trim();
                    InsCat = row["CHLDMST_INS_CAT"].ToString().Trim();
                    DentalCoverage = row["CHLDMST_DENTAL_COVERAGE"].ToString().Trim();
                    Disability = row["CHLDMST_DISABILITY"].ToString().Trim();
                    DiagnosisDate = row["CHLDMST_DIAGNOSIS_DATE"].ToString().Trim();

                }

            }

        }



        #endregion

        #region Properties

        public string ChldMstAgency { get; set; }
        public string ChldMstDept { get; set; }
        public string ChldMstProgram { get; set; }
        public string ChldMstYr { get; set; }
        public string ApplNo { get; set; }
        public string FamilySeq { get; set; }
        public string FundSource { get; set; }
        public string AltFundSrc { get; set; }
        public string ClassPrefer { get; set; }
        public string Pob { get; set; }
        public string BirthCert { get; set; }
        public string ChldReat { get; set; }
        public string NextYearPrep { get; set; }
        public string PreClient { get; set; }
        public string Transport { get; set; }
        public string MedSecurity { get; set; }
        public string MedCoverage { get; set; }
        public string MedCoverType { get; set; }
        public string MedPlan { get; set; }
        public string InsCat { get; set; }
        public string MedInsurer { get; set; }
        public string DoctorName { get; set; }
        public string DoctorAddress { get; set; }
        public string DoctorPhone { get; set; }
        public string DentalCoverage { get; set; }
        public string DentalPlan { get; set; }
        public string DentistName { get; set; }
        public string DentalInsurer { get; set; }
        public string DentistAddress { get; set; }
        public string DentistPhone { get; set; }
        public string Disability { get; set; }
        public string DisabilityType { get; set; }
        public string DiagnosisDate { get; set; }
        public string Allergies { get; set; }
        public string DietaryRes { get; set; }
        public string Medications { get; set; }
        public string MedicalCond { get; set; }
        public string HhConcerns { get; set; }
        public string DevlpmntlConcern { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string DateAdd { get; set; }
        public string Mode { get; set; }
        public string PickOff { get; set; }
        public string DropOff { get; set; }

        #endregion

    }

    public class ChldMstEMEMEntitty
    {

        #region Constructors

        public ChldMstEMEMEntitty()
        {

            ChldMstAgency = string.Empty;
            ChldMstDept = string.Empty;
            ChldMstProgram = string.Empty;
            ChldMstYr = string.Empty;
            ApplNo = string.Empty;
            FamilySeq = string.Empty;
            EMERName = string.Empty;
            EmerRel = string.Empty;
            EmerAddress1 = string.Empty;
            EmerAddress2 = string.Empty;
            EmerPhone1 = string.Empty;
            EmerPhone2 = string.Empty;


        }

        public ChldMstEMEMEntitty(DataRow row)
        {
            if (row != null)
            {
                ChldMstAgency = row["EMER_AGENCY"].ToString().Trim();
                ChldMstDept = row["EMER_DEPT"].ToString().Trim();
                ChldMstProgram = row["EMER_PROG"].ToString().Trim();
                ChldMstYr = row["EMER_YEAR"].ToString().Trim();
                ApplNo = row["EMER_APP_NO"].ToString().Trim();
                FamilySeq = row["EMER_SEQ"].ToString().Trim();
                EMERName = row["EMER_NAME"].ToString().Trim();
                EmerRel = row["EMER_REL"].ToString().Trim();
                EmerAddress1 = row["EMER_ADDR1"].ToString().Trim();
                EmerAddress2 = row["EMER_ADDR2"].ToString().Trim();
                EmerPhone1 = row["EMER_PHONE1"].ToString().Trim();
                EmerPhone2 = row["EMER_PHONE2"].ToString().Trim();

            }

        }



        #endregion

        #region Properties

        public string ChldMstAgency { get; set; }
        public string ChldMstDept { get; set; }
        public string ChldMstProgram { get; set; }
        public string ChldMstYr { get; set; }
        public string ApplNo { get; set; }
        public string FamilySeq { get; set; }
        public string EMERName { get; set; }
        public string EmerRel { get; set; }
        public string EmerAddress1 { get; set; }
        public string EmerAddress2 { get; set; }
        public string EmerPhone1 { get; set; }
        public string EmerPhone2 { get; set; }
        public string Emode { get; set; }

        #endregion
    }

    public class CaseCondEntitty
    {

        #region Constructors

        public CaseCondEntitty()
        {

            CaseCondAgency = string.Empty;
            CaseCondDept = string.Empty;
            CaseCondProgram = string.Empty;
            CaseCondYr = string.Empty;
            ApplNo = string.Empty;
            Allergy = string.Empty;
            DietRestrct = string.Empty;
            Medications = string.Empty;
            MedConds = string.Empty;
            HHConcerns = string.Empty;
            DevlConcerns = string.Empty;



        }

        public CaseCondEntitty(DataRow row)
        {
            if (row != null)
            {
                CaseCondAgency = row["CASECOND_AGENCY"].ToString().Trim();
                CaseCondDept = row["CASECOND_DEPT"].ToString().Trim();
                CaseCondProgram = row["CASECOND_PROG"].ToString().Trim();
                CaseCondYr = row["CASECOND_YEAR"].ToString().Trim();
                ApplNo = row["CASECOND_APP_NO"].ToString().Trim();
                Allergy = row["CASECOND_ALLERGY"].ToString().Trim();
                DietRestrct = row["CASECOND_DIET_RESTRCT"].ToString().Trim();
                Medications = row["CASECOND_MEDICATIONS"].ToString().Trim();
                MedConds = row["CASECOND_MED_CONDS"].ToString().Trim();
                HHConcerns = row["CASECOND_HH_CONCRNS"].ToString().Trim();
                DevlConcerns = row["CASECOND_DEVL_CONCRNS"].ToString().Trim();
                AddOperator = row["CASECOND_ADD_OPERATOR"].ToString().Trim();
                DateLstc = row["CASECOND_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["CASECOND_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["CASECOND_DATE_ADD"].ToString().Trim();

            }

        }



        #endregion

        #region Properties

        public string CaseCondAgency { get; set; }
        public string CaseCondDept { get; set; }
        public string CaseCondProgram { get; set; }
        public string CaseCondYr { get; set; }
        public string ApplNo { get; set; }
        public string Allergy { get; set; }
        public string DietRestrct { get; set; }
        public string Medications { get; set; }
        public string MedConds { get; set; }
        public string HHConcerns { get; set; }
        public string DevlConcerns { get; set; }
        public string Emode { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string DateAdd { get; set; }

        #endregion
    }


    public class CASEBIOEntitty
    {

        #region Constructors

        public CASEBIOEntitty()
        {

            BIO_AGENCY = string.Empty;
            BIO_DEPT = string.Empty;
            BIO_PROGRAM = string.Empty;
            BIO_YEAR = string.Empty;
            BIO_APP_NO = string.Empty;
            BIO_MOT_FNAME = string.Empty;
            BIO_MOT_LNAME = string.Empty;
            BIO_MOT_STREET = string.Empty;
            BIO_MOT_CITY = string.Empty;
            BIO_MOT_STATE = string.Empty;
            BIO_MOT_ZIP = string.Empty;
            BIO_MOT_ZIPPLUS = string.Empty;
            BIO_FAT_FNAME = string.Empty;
            BIO_FAT_LNAME = string.Empty;
            BIO_FAT_STREET = string.Empty;
            BIO_FAT_CITY = string.Empty;
            BIO_FAT_STATE = string.Empty;
            BIO_FAT_ZIP = string.Empty;
            BIO_FAT_ZIPPLUS = string.Empty;
            BIO_BRIDGE = string.Empty;

            BIO_MOT_PHONE = string.Empty;
            BIO_MOT_CELL = string.Empty;
            BIO_FAT_PHONE = string.Empty;
            BIO_FAT_CELL = string.Empty;
            BIO_MOT_RELATION = string.Empty;
            BIO_FAT_RELATION = string.Empty;

            BIO_DATE_ADD = string.Empty;
            BIO_ADD_OPERATOR = string.Empty;
            BIO_DATE_LSTC = string.Empty;
            BIO_LSTC_OPERATOR = string.Empty;



        }

        public CASEBIOEntitty(DataRow row)
        {
            if (row != null)
            {
                BIO_AGENCY = row["BIO_AGENCY"].ToString().Trim();
                BIO_DEPT = row["BIO_DEPT"].ToString().Trim();
                BIO_PROGRAM = row["BIO_PROGRAM"].ToString().Trim();
                BIO_YEAR = row["BIO_YEAR"].ToString().Trim();
                BIO_APP_NO = row["BIO_APP_NO"].ToString().Trim();
                BIO_MOT_FNAME = row["BIO_MOT_FNAME"].ToString().Trim();
                BIO_MOT_LNAME = row["BIO_MOT_LNAME"].ToString().Trim();
                BIO_MOT_STREET = row["BIO_MOT_STREET"].ToString().Trim();
                BIO_MOT_CITY = row["BIO_MOT_CITY"].ToString().Trim();
                BIO_MOT_STATE = row["BIO_MOT_STATE"].ToString().Trim();
                BIO_MOT_ZIP = row["BIO_MOT_ZIP"].ToString().Trim();
                BIO_MOT_ZIPPLUS = row["BIO_MOT_ZIPPLUS"].ToString().Trim();
                BIO_FAT_FNAME = row["BIO_FAT_FNAME"].ToString().Trim();
                BIO_FAT_LNAME = row["BIO_FAT_LNAME"].ToString().Trim();
                BIO_FAT_STREET = row["BIO_FAT_STREET"].ToString().Trim();
                BIO_FAT_CITY = row["BIO_FAT_CITY"].ToString().Trim();
                BIO_FAT_STATE = row["BIO_FAT_STATE"].ToString().Trim();
                BIO_FAT_ZIP = row["BIO_FAT_ZIP"].ToString().Trim();
                BIO_FAT_ZIPPLUS = row["BIO_FAT_ZIPPLUS"].ToString().Trim();
                BIO_BRIDGE = row["BIO_BRIDGE"].ToString().Trim();

                BIO_MOT_PHONE = row["BIO_MOT_PHONE"].ToString().Trim();
                BIO_MOT_CELL = row["BIO_MOT_CELL"].ToString().Trim();
                BIO_FAT_PHONE = row["BIO_FAT_PHONE"].ToString().Trim();
                BIO_FAT_CELL = row["BIO_FAT_CELL"].ToString().Trim();
                BIO_MOT_RELATION = row["BIO_MOT_RELATION"].ToString().Trim();
                BIO_FAT_RELATION = row["BIO_FAT_RELATION"].ToString().Trim();

                BIO_DATE_ADD = row["BIO_DATE_ADD"].ToString().Trim();
                BIO_ADD_OPERATOR = row["BIO_ADD_OPERATOR"].ToString().Trim();
                BIO_DATE_LSTC = row["BIO_DATE_LSTC"].ToString().Trim();
                BIO_LSTC_OPERATOR = row["BIO_LSTC_OPERATOR"].ToString().Trim();

            }

        }



        #endregion

        #region Properties

        public string BIO_AGENCY { get; set; }
        public string BIO_DEPT { get; set; }
        public string BIO_PROGRAM { get; set; }
        public string BIO_YEAR { get; set; }
        public string BIO_APP_NO { get; set; }
        public string BIO_MOT_FNAME { get; set; }
        public string BIO_MOT_LNAME { get; set; }
        public string BIO_MOT_STREET { get; set; }
        public string BIO_MOT_CITY { get; set; }
        public string BIO_MOT_STATE { get; set; }
        public string BIO_MOT_ZIP { get; set; }
        public string BIO_MOT_ZIPPLUS { get; set; }
        public string BIO_FAT_FNAME { get; set; }
        public string BIO_FAT_LNAME { get; set; }
        public string BIO_FAT_STREET { get; set; }
        public string BIO_FAT_CITY { get; set; }
        public string BIO_FAT_STATE { get; set; }
        public string BIO_FAT_ZIP { get; set; }
        public string BIO_FAT_ZIPPLUS { get; set; }
        public string BIO_BRIDGE { get; set; }

        public string BIO_MOT_PHONE { get; set; }
        public string BIO_MOT_CELL { get; set; }
        public string BIO_FAT_PHONE { get; set; }
        public string BIO_FAT_CELL { get; set; }
        public string BIO_MOT_RELATION { get; set; }
        public string BIO_FAT_RELATION { get; set; }



        public string BIO_DATE_ADD { get; set; }
        public string BIO_ADD_OPERATOR { get; set; }
        public string BIO_DATE_LSTC { get; set; }
        public string BIO_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion
    }
}
