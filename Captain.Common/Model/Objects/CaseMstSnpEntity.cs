using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace Captain.Common.Model.Objects
{
    public class CaseMstSnpEntity
    {

        #region Constructors

        public CaseMstSnpEntity()
        {
             ApplNo = string.Empty;
             Ssno = string.Empty;
             NameixLast = string.Empty;
             NameixFi = string.Empty;
             NameixMi = string.Empty;
             Area = string.Empty;
             Phone = string.Empty;
             AltBdate = string.Empty;
             ExpireWorkDate = string.Empty;
             Hn = string.Empty;
             Street = string.Empty;
             City = string.Empty;
             State = string.Empty;
             Alias = string.Empty;
             Agency = string.Empty;
             Dept = string.Empty;
             Program = string.Empty;
             Year = string.Empty;
             FamilySeq = string.Empty;
             DateLstc = string.Empty;
             SnpActive = string.Empty;
             Snp_Sex = 
             Mst_Intake_Worker = 
             Mst_Suffix = 
             Mst_Apt = 
             MsT_Floor =
             Mst_Heating_Source = 
             MsT_Zip =
             Mst_Zip_Plus = 
             Mst_Cell_Phone = 
             Mst_Language = string.Empty;
        }

        public CaseMstSnpEntity(DataRow row)
        {
            if (row != null)
            {
                ApplNo = row["MST_APP_NO"].ToString().Trim();
                Ssno = row["SNP_SSNO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();
                Area = row["MST_AREA"].ToString().Trim();
                Phone = row["MST_PHONE"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                Hn = row["MST_HN"].ToString().Trim();
                Street = row["MST_STREET"].ToString().Trim();
                City = row["MST_CITY"].ToString().Trim();
                State = row["MST_STATE"].ToString().Trim();
                Alias = row["SNP_ALIAS"].ToString().Trim();
                Agency = row["SNP_AGENCY"].ToString().Trim();
                Dept = row["SNP_DEPT"].ToString().Trim();
                Program = row["SNP_PROGRAM"].ToString().Trim();
                Year = row["SNP_YEAR"].ToString().Trim();               
                FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                SnpActive = row["SNP_STATUS"].ToString().Trim();

                Snp_Sex = row["SNP_SEX"].ToString().Trim();
                Mst_Intake_Worker = row["MST_INTAKE_WORKER"].ToString().Trim();
                Mst_Suffix = row["MST_SUFFIX"].ToString().Trim();
                Mst_Apt = row["MST_APT"].ToString().Trim();
                MsT_Floor = row["MST_FLR"].ToString().Trim();
                Mst_Heating_Source = row["MST_SOURCE"].ToString().Trim();
                MsT_Zip = row["MST_ZIP"].ToString().Trim();
                Mst_Zip_Plus = row["MST_ZIPPLUS"].ToString().Trim();
                Mst_Cell_Phone = row["MST_CELL_PHONE"].ToString().Trim();                
                Mst_Email = row["MST_EMAIL"].ToString().Trim();
                Mst_Language = row["MST_LANGUAGE"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string ApplNo { get; set; }
        public string Ssno { get; set; }
        public string NameixLast { get; set; }
        public string NameixFi { get; set; }
        public string NameixMi { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string AltBdate { get; set; }
        public string ExpireWorkDate { get; set; }
        public string Hn { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Alias { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string FamilySeq { get; set; }
        public string DateLstc { get; set; }
        public string SnpActive { get; set; }
        public string Snp_Sex { get; set; }
        public string Mst_Intake_Worker { get; set; }
        public string Mst_Suffix { get; set; }
        public string Mst_Apt { get; set; }
        public string MsT_Floor { get; set; }
        public string Mst_Heating_Source { get; set; }
        public string MsT_Zip { get; set; }
        public string Mst_Zip_Plus { get; set; }
        public string Mst_Cell_Phone { get; set; }
        public string Mst_Email { get; set; }
        public string Mst_Language { get; set; }


        #endregion
    }

    public class LPMQEntity
    {

        #region Constructors

        public LPMQEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            ApplNo = string.Empty;
            NameixLast = string.Empty;
            NameixFi = string.Empty;
            NameixMi = string.Empty;

            LPM_0001 = string.Empty;
            LPM_0002 = string.Empty;
            LPM_0003 = string.Empty;
            LPM_0004 = string.Empty;
            LPM_0005 = string.Empty;
            LPM_0006 = string.Empty;
            LPM_0007 = string.Empty;
            LPM_0008 = string.Empty;
            LPM_0009 = string.Empty;
            LPM_0010 = string.Empty;
            LPM_0011 = string.Empty;
            LPM_0012 = string.Empty;
            LPM_0013 = string.Empty;
            LPM_0014 = string.Empty;
            LPM_0015 = string.Empty;
            LPM_0016 = string.Empty;
            LPM_0017 = string.Empty;

            LPBType = string.Empty;

            Ssno = string.Empty;
            Area = string.Empty;
            Phone = string.Empty;
            AltBdate = string.Empty;
            ExpireWorkDate = string.Empty;
            Hn = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Alias = string.Empty;
            
            FamilySeq = string.Empty;
            DateLstc = string.Empty;
            SnpActive = string.Empty;
            Snp_Sex =
            Mst_Intake_Worker =
            Mst_Suffix =
            Mst_Apt =
            MsT_Floor =
            Mst_Heating_Source =
            MsT_Zip =
            Mst_Zip_Plus =
            Mst_Email =
            Mst_Cell_Phone = string.Empty;
        }

        public LPMQEntity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Program = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                ApplNo = row["MST_APP_NO"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();
                NameixMi = row["SNP_NAME_IX_MI"].ToString().Trim();

                LPM_0001 = row["MST_LPM_0001"].ToString().Trim();
                LPM_0002 = row["MST_LPM_0002"].ToString().Trim();
                LPM_0003 = row["MST_LPM_0003"].ToString().Trim();
                LPM_0004 = row["MST_LPM_0004"].ToString().Trim();
                LPM_0005 = row["MST_LPM_0005"].ToString().Trim();
                LPM_0006 = row["MST_LPM_0006"].ToString().Trim();
                LPM_0007 = row["MST_LPM_0007"].ToString().Trim();
                LPM_0008 = row["MST_LPM_0008"].ToString().Trim();
                LPM_0009 = row["MST_LPM_0009"].ToString().Trim();
                LPM_0010 = row["MST_LPM_0010"].ToString().Trim();
                LPM_0011 = row["MST_LPM_0011"].ToString().Trim();
                LPM_0012 = row["MST_LPM_0012"].ToString().Trim();
                LPM_0013 = row["MST_LPM_0013"].ToString().Trim();
                LPM_0014 = row["MST_LPM_0014"].ToString().Trim();
                LPM_0015 = row["MST_LPM_0015"].ToString().Trim();
                LPM_0016 = row["MST_LPM_0016"].ToString().Trim();
                LPM_0017 = row["MST_LPM_0017"].ToString().Trim();
                Mst_Heating_Source = row["MST_SOURCE"].ToString().Trim();
                Mst_Email = row["MST_EMAIL"].ToString().Trim();

                // LPBType = row["LPB_TYPE"].ToString().Trim();

                //Ssno = row["SNP_SSNO"].ToString().Trim();
                Area = row["MST_AREA"].ToString().Trim();
                Phone = row["MST_PHONE"].ToString().Trim();
                //AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                //ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();
                Hn = row["MST_HN"].ToString().Trim();
                Street = row["MST_STREET"].ToString().Trim();
                City = row["MST_CITY"].ToString().Trim();
                State = row["MST_STATE"].ToString().Trim();
                //Alias = row["SNP_ALIAS"].ToString().Trim();

                //FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                //DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                //SnpActive = row["SNP_STATUS"].ToString().Trim();

                //Snp_Sex = row["SNP_SEX"].ToString().Trim();
                //Mst_Intake_Worker = row["MST_INTAKE_WORKER"].ToString().Trim();
                Mst_Suffix = row["MST_SUFFIX"].ToString().Trim();
                Mst_Apt = row["MST_APT"].ToString().Trim();
                MsT_Floor = row["MST_FLR"].ToString().Trim();

                MsT_Zip = row["MST_ZIP"].ToString().Trim();
                Mst_Zip_Plus = row["MST_ZIPPLUS"].ToString().Trim();
                //Mst_Cell_Phone = row["MST_CELL_PHONE"].ToString().Trim();
            }
        }

        #endregion

        #region Properties
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string ApplNo { get; set; }
        public string NameixLast { get; set; }
        public string NameixFi { get; set; }
        public string NameixMi { get; set; }

        public string LPM_0001 { get; set; }
        public string LPM_0002 { get; set; }
        public string LPM_0003 { get; set; }
        public string LPM_0004 { get; set; }
        public string LPM_0005 { get; set; }
        public string LPM_0006 { get; set; }
        public string LPM_0007 { get; set; }
        public string LPM_0008 { get; set; }
        public string LPM_0009 { get; set; }
        public string LPM_0010 { get; set; }
        public string LPM_0011 { get; set; }
        public string LPM_0012 { get; set; }
        public string LPM_0013 { get; set; }
        public string LPM_0014 { get; set; }
        public string LPM_0015 { get; set; }
        public string LPM_0016 { get; set; }
        public string LPM_0017 { get; set; }

        public string LPBType { get; set; }
        
        public string Ssno { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string AltBdate { get; set; }
        public string ExpireWorkDate { get; set; }
        public string Hn { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Alias { get; set; }
        
        public string FamilySeq { get; set; }
        public string DateLstc { get; set; }
        public string SnpActive { get; set; }
        public string Snp_Sex { get; set; }
        public string Mst_Intake_Worker { get; set; }
        public string Mst_Suffix { get; set; }
        public string Mst_Apt { get; set; }
        public string MsT_Floor { get; set; }
        public string Mst_Heating_Source { get; set; }
        public string MsT_Zip { get; set; }
        public string Mst_Zip_Plus { get; set; }
        public string Mst_Cell_Phone { get; set; }
        public string Mst_Email { get; set; }

        #endregion
    }

    public class CASB0016Entity
    {

        #region Constructors

        public CASB0016Entity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            ApplNo = string.Empty;

            Foreign_ID = string.Empty;
            AccountName = string.Empty;
            NameixLast = string.Empty;
            NameixFi = string.Empty;

            Hn = string.Empty;
            Street = string.Empty;
            Mst_Suffix =
            Mst_Apt =
            MsT_Floor =
            City = string.Empty;
            State = string.Empty;
            MsT_Zip =
            Mst_Zip_Plus =
            Phone = string.Empty;
            AltBdate = string.Empty;
            ExpireWorkDate = string.Empty;
            Mst_Email =
            RaceDesc =
            EthinicDesc =
            GenderDesc = string.Empty;

            //Alias = string.Empty;
            //Agency = string.Empty;
            //Dept = string.Empty;
            //Program = string.Empty;
            //Year = string.Empty;
            //FamilySeq = string.Empty;
            //DateLstc = string.Empty;
            //SnpActive = string.Empty;
            //Snp_Sex =
            //Mst_Intake_Worker =

            //Mst_Heating_Source =

            //Mst_Cell_Phone =
            //Mst_Language = string.Empty;
        }

        public CASB0016Entity(DataRow row)
        {
            if (row != null)
            {
                Agency = row["MST_AGENCY"].ToString().Trim();
                Dept = row["MST_DEPT"].ToString().Trim();
                Program = row["MST_PROGRAM"].ToString().Trim();
                Year = row["MST_YEAR"].ToString().Trim();
                ApplNo = row["MST_APP_NO"].ToString().Trim();
                //Ssno = row["SNP_SSNO"].ToString().Trim();

                Foreign_ID = row["Foreign_ID"].ToString().Trim();
                AccountName = row["ACCOUNT_NAME"].ToString().Trim();
                NameixLast = row["SNP_NAME_IX_LAST"].ToString().Trim();
                NameixFi = row["SNP_NAME_IX_FI"].ToString().Trim();

                Hn = row["MST_HN"].ToString().Trim();
                Street = row["MST_STREET"].ToString().Trim();
                Mst_Suffix = row["MST_SUFFIX"].ToString().Trim();
                Mst_Apt = row["MST_APT"].ToString().Trim();
                MsT_Floor = row["MST_FLR"].ToString().Trim();

                Phone = row["MST_PHONE"].ToString().Trim();
                AltBdate = row["SNP_ALT_BDATE"].ToString().Trim();
                ExpireWorkDate = row["SNP_EXPIRE_WORK_DATE"].ToString().Trim();


                City = row["MST_CITY"].ToString().Trim();
                State = row["MST_STATE"].ToString().Trim();
                MsT_Zip = row["MST_ZIP"].ToString().Trim();
                Mst_Zip_Plus = row["MST_ZIPPLUS"].ToString().Trim();
                Mst_Email = row["MST_EMAIL"].ToString().Trim();

                //Alias = row["SNP_ALIAS"].ToString().Trim();

                //FamilySeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                //DateLstc = row["SNP_DATE_LSTC"].ToString().Trim();
                //SnpActive = row["SNP_STATUS"].ToString().Trim();

                //Snp_Sex = row["SNP_SEX"].ToString().Trim();
                //Mst_Intake_Worker = row["MST_INTAKE_WORKER"].ToString().Trim();

                //Mst_Heating_Source = row["MST_SOURCE"].ToString().Trim();

                //Mst_Cell_Phone = row["MST_CELL_PHONE"].ToString().Trim();

                //Mst_Language = row["MST_LANGUAGE"].ToString().Trim();

                RaceDesc = row["RACE_DESC"].ToString().Trim();
                EthinicDesc = row["ETHNIC_DESC"].ToString().Trim();
                GenderDesc = row["GENDER_DESC"].ToString().Trim();

            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string ApplNo { get; set; }


        public string Foreign_ID { get; set; }
        public string AccountName { get; set; }
        public string NameixLast { get; set; }
        public string NameixFi { get; set; }

        public string Hn { get; set; }
        public string Street { get; set; }
        public string Mst_Suffix { get; set; }
        public string Mst_Apt { get; set; }
        public string MsT_Floor { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MsT_Zip { get; set; }
        public string Mst_Zip_Plus { get; set; }


        public string Phone { get; set; }
        public string Mst_Email { get; set; }

        public string AltBdate { get; set; }
        public string ExpireWorkDate { get; set; }

        //public string Alias { get; set; }

        //public string FamilySeq { get; set; }
        //public string DateLstc { get; set; }
        //public string SnpActive { get; set; }
        //public string Snp_Sex { get; set; }
        //public string Mst_Intake_Worker { get; set; }


        //public string Mst_Heating_Source { get; set; }

        //public string Mst_Cell_Phone { get; set; }

        //public string Mst_Language { get; set; }

        public string RaceDesc { get; set; }
        public string EthinicDesc { get; set; }
        public string GenderDesc { get; set; }


        #endregion
    }
}
