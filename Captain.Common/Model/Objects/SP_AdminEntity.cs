using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.Charts.Native;
using DevExpress.XtraSplashScreen;
using DevExpress.Data.Filtering.Helpers;

using DevExpress.ExpressApp.DC;
using Aspose.Cells;
using DevExpress.XtraEditors.Filtering.Templates;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using Twilio.TwiML.Voice;
using System.Data.Metadata.Edm;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Captain.Common.Views.Forms.Base;

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    ////public class CaseSPMEntity
    ////{
    ////    #region Constructors

    ////    public CaseSPMEntity()
    ////    {
    ////        Agency = string.Empty;
    ////        Dept = string.Empty;
    ////        Prog = string.Empty;
    ////        Year = string.Empty;
    ////        App = string.Empty;
    ////        ServPlan = string.Empty;
    ////        CaseWorker = string.Empty;
    ////        Site = string.Empty;
    ////        StDate = string.Empty;
    ////        EstDate = string.Empty;
    ////        CompDate = string.Empty;
    ////        SelBranches = string.Empty;
    ////        AddLbr = string.Empty;
    ////        DateLstc = string.Empty;
    ////        lstcOperator = string.Empty;
    ////        dateadd = string.Empty;
    ////        addoperator = string.Empty;


    ////    }

    ////    public CaseSPMEntity(DataRow CaseSPM)
    ////    {
    ////        if (CaseSPM != null)
    ////        {
    ////            DataRow row = CaseSPM;
    ////            Agency = row["spm_agency"].ToString();
    ////            Dept = row["spm_dept"].ToString();
    ////            Prog = row["spm_program"].ToString();
    ////            Year = row["spm_year"].ToString();
    ////            App = row["spm_app_no"].ToString();
    ////            ServPlan = row["spm_service_plan"].ToString();
    ////            CaseWorker = row["spm_caseworker"].ToString();
    ////            Site = row["spm_site"].ToString();
    ////            StDate = row["spm_startdate"].ToString();
    ////            EstDate = row["spm_estdate"].ToString();
    ////            CompDate = row["spm_compdate"].ToString();
    ////            SelBranches = row["spm_sel_branches"].ToString();
    ////            AddLbr = row["spm_have_addlbr"].ToString();
    ////            DateLstc = row["spm_date_lstc"].ToString();
    ////            lstcOperator = row["spm_lstc_operator"].ToString();
    ////            dateadd = row["spm_date_add"].ToString();
    ////            addoperator = row["spm_add_operator"].ToString();

    ////        }
    ////    }



    ////    #endregion

    ////    #region Properties

    ////    public string Agency { get; set; }
    ////    public string Dept { get; set; }
    ////    public string Prog { get; set; }
    ////    public string Year { get; set; }
    ////    public string App { get; set; }
    ////    public string ServPlan { get; set; }
    ////    public string CaseWorker { get; set; }
    ////    public string Site { get; set; }
    ////    public string StDate { get; set; }
    ////    public string EstDate { get; set; }
    ////    public string CompDate { get; set; }
    ////    public string SelBranches { get; set; }
    ////    public string AddLbr { get; set; }
    ////    public string DateLstc { get; set; }
    ////    public string lstcOperator { get; set; }
    ////    public string dateadd { get; set; }
    ////    public string addoperator { get; set; }

    ////    #endregion

    //#region Public / Overrides Methods

    //public override bool Equals(object obj)
    //{
    //    bool returnValue = false;

    //    // Check for null values and compare run-time types.
    //    if (obj == null || GetType() != obj.GetType())
    //    {
    //        returnValue = false;
    //    }

    //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
    //    if (zipcode != null)
    //    {
    //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
    //    }
    //    return returnValue;
    //}



    //#endregion
    //}

    public class CASESPM2Entity
    {
        #region Constructors

        public CASESPM2Entity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            ServPlan = string.Empty;
            Spm_Seq =
            Branch = string.Empty;
            Group = string.Empty;
            Type1 = string.Empty;
            CamCd = string.Empty;
            Curr_Group = string.Empty;
            SelOrdinal = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;
            Dateadd = string.Empty;
            addoperator = string.Empty;
            Shift_Count = 0;
            CAMS_Desc =
            CAMS_Active = null;
        }

        public CASESPM2Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Prog =
                Year =
                App =
                ServPlan =
                Spm_Seq =
                Branch =
                Group =
                Type1 =
                CamCd =
                Curr_Group =
                SelOrdinal =
                DateLstc =
                lstcOperator =
                Dateadd =
                addoperator =
                CAMS_Desc =
                CAMS_Active = null;

                Shift_Count = 0;
            }
        }

        public CASESPM2Entity(DataRow CASESPM2)
        {
            if (CASESPM2 != null)
            {
                DataRow row = CASESPM2;
                Rec_Type = "U";
                Agency = row["SPM2_AGENCY"].ToString();
                Dept = row["spm2_dept"].ToString();
                Prog = row["SPM2_PROGRAM"].ToString();
                Year = row["spm2_year"].ToString();
                App = row["spm2_app_no"].ToString();
                ServPlan = row["spm2_service_plan"].ToString();
                Spm_Seq = row["spm2_seq"].ToString();
                Branch = row["spm2_branch"].ToString();
                Group = row["spm2_group"].ToString();
                Type1 = row["spm2_type"].ToString();
                CamCd = row["spm2_cams_code"].ToString();
                Curr_Group = row["spm2_curr_grp"].ToString();
                SelOrdinal = row["spm2_ordinal"].ToString();
                DateLstc = row["spm2_date_lstc"].ToString();
                lstcOperator = row["spm2_lstc_operator"].ToString();
                Dateadd = row["spm2_date_add"].ToString();
                addoperator = row["spm2_add_operator"].ToString();

                CAMS_Desc = row["CAMS_Desc"].ToString();
                CAMS_Active = row["CAMS_Active"].ToString();

                Shift_Count = 0;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public CASESPM2Entity(CASESPM2Entity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            Agency = Entity.Agency;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            Year = Entity.Year;
            App = Entity.App;
            ServPlan = Entity.ServPlan;
            Spm_Seq = Entity.Spm_Seq;
            Branch = Entity.Branch;
            Group = Entity.Group;
            Type1 = Entity.Type1;
            CamCd = Entity.CamCd;
            Curr_Group = Entity.Curr_Group;
            SelOrdinal = Entity.SelOrdinal;
            DateLstc = Entity.DateLstc;
            lstcOperator = Entity.lstcOperator;
            Dateadd = Entity.Dateadd;
            addoperator = Entity.addoperator;
            CAMS_Desc = Entity.CAMS_Desc;
            CAMS_Active = Entity.CAMS_Active;
            Shift_Count = Entity.Shift_Count;
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string ServPlan { get; set; }
        public string Spm_Seq { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string Type1 { get; set; }
        public string CamCd { get; set; }
        public string Curr_Group { get; set; }
        public string SelOrdinal { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string CAMS_Desc { get; set; }
        public string CAMS_Active { get; set; }

        public int Shift_Count { get; set; }

        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class CASESP0Entity
    {
        #region Constructors

        public CASESP0Entity()
        {
            Rec_Type =
            Code =
            Desc =
            Active =
            Allow_Add_Branch =
            Default_Prog =
            Allow_Duplicates =
            BpCode =
            BpDesc =
            B1Code =
            B1Desc =
            B2Code =
            B2Desc =
            B3Code =
            B3Desc =
            B4Code =
            B4Desc =
            B5Code =
            B5Desc =
            Status =
            Legals =
            Funds =
            Validate =
            DateLstc =
            lstcOperator =
            Dateadd =
            addoperator =
            Auto_Post_CA =
            Auto_Post_MS =
            Auto_Post_SP = Sp0ReadOnly = Sp0Notes = Category = PartnerPostingType = NoSPM = UsedFunds=
            Household_Type = Maxbenefit = Transactions = BulkEntry=string.Empty;
        }

        public CASESP0Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type =
                Code =
                Desc =
                Active =
                Allow_Add_Branch =
                Default_Prog =
                Allow_Duplicates =
                BpCode =
                BpDesc =
                B1Code =
                B1Desc =
                B2Code =
                B2Desc =
                B3Code =
                B3Desc =
                B4Code =
                B4Desc =
                B5Code =
                B5Desc =
                Status =
                Legals =
                Funds =
                Validate =
                DateLstc =
                lstcOperator =
                Dateadd =
                addoperator =
                Auto_Post_CA =
                Auto_Post_MS =
                Auto_Post_SP =
                Sp0ReadOnly =
                Sp0Notes =
                Category =
                PartnerPostingType =
                NoSPM =
                Household_Type =
                Maxbenefit =
                Transactions =
                UsedFunds=
                BulkEntry = null;
            }
        }

        public CASESP0Entity(DataRow row)
        {
            if (row != null)
            {
                Rec_Type = "U";
                Code = row["sp0_servicecode"].ToString();
                Desc = row["sp0_description"].ToString();
                Active = row["SP0_ACTIVE"].ToString();
                Allow_Add_Branch = row["SP0_ALLOW_ADLBRANCH"].ToString();
                //if (row["SP0_ALLOW_ADLBRANCH"].ToString() == "1")
                //    Allow_Add_Branch = "Y";
                Allow_Duplicates = row["SP0_ALLOW_DUPS"].ToString();

                Default_Prog = row["sp0_Def_Prog"].ToString();
                BpCode = row["sp0_pbranch_code"].ToString();
                BpDesc = row["sp0_pbranch_desc"].ToString();
                B1Code = row["sp0_branch1_code"].ToString();
                B1Desc = row["sp0_branch1_desc"].ToString();
                B2Code = row["sp0_branch2_code"].ToString();
                B2Desc = row["sp0_branch2_desc"].ToString();
                B3Code = row["sp0_branch3_code"].ToString();
                B3Desc = row["sp0_branch3_desc"].ToString();
                B4Code = row["sp0_branch4_code"].ToString();
                B4Desc = row["sp0_branch4_desc"].ToString();
                B5Code = row["sp0_branch5_code"].ToString();
                B5Desc = row["sp0_branch5_desc"].ToString();
                Status = row["sp0_statuses"].ToString();
                Legals = row["sp0_legals"].ToString();
                Funds = row["sp0_funds"].ToString();
                Validate = row["sp0_validated"].ToString();
                DateLstc = row["sp0_date_lstc"].ToString();
                lstcOperator = row["sp0_lstc_operator"].ToString();
                Dateadd = row["sp0_date_add"].ToString();
                addoperator = row["sp0_add_operator"].ToString();
                Auto_Post_CA = row["Auto_Post_CA"].ToString();
                Auto_Post_MS = row["Auto_Post_MS"].ToString();
                Auto_Post_SP = row["SP0_ALLOW_AUTOPOST"].ToString();
                Sp0ReadOnly = row["SP0_READONLY"].ToString();
                //Spmaddoperator = row["spm_add_operator"].ToString();
                Sp0Notes = row["SP0_NOTES"].ToString();
                Category = row["SP0_Category"].ToString();
                PartnerPostingType = row["SP0_PPTYPE"].ToString();
                NoSPM = row["SP0_NOSPM"].ToString();
                Household_Type = row["SP0_HH_TYPE"].ToString();
                Maxbenefit = row["SP0_MAX_BEN"].ToString();
                Transactions = row["SP0_NO_TRANS"].ToString();
                BulkEntry = row["SP0_BULK_ENTRY"].ToString();
                UsedFunds = row["SP0_USED_FUNDS"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string Allow_Add_Branch { get; set; }
        public string Default_Prog { get; set; }
        public string Allow_Duplicates { get; set; }
        public string BpCode { get; set; }
        public string BpDesc { get; set; }
        public string B1Code { get; set; }
        public string B1Desc { get; set; }
        public string B2Code { get; set; }
        public string B2Desc { get; set; }
        public string B3Code { get; set; }
        public string B3Desc { get; set; }
        public string B4Code { get; set; }
        public string B4Desc { get; set; }
        public string B5Code { get; set; }
        public string B5Desc { get; set; }
        public string Status { get; set; }
        public string Legals { get; set; }
        public string Funds { get; set; }
        public string Validate { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Auto_Post_CA { get; set; }
        public string Auto_Post_MS { get; set; }
        public string Auto_Post_SP { get; set; }
        public string Sp0ReadOnly { get; set; }
        public string Sp0Notes { get; set; }
        public string Category { get; set; }
        public string PartnerPostingType { get; set; }
        public string NoSPM { get; set; }
        public string Household_Type { get; set; }
        public string Maxbenefit { get; set; }
        public string Transactions { get; set; }
        public string BulkEntry { get; set; }

        public string UsedFunds { get; set; }

        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class CASESP1Entity
    {
        #region Constructors

        public CASESP1Entity()
        {
            Rec_Type =
            Sel_Type=
            Code =
            Agency =
            Dept =
            Prog =
            DateLstc =
            lstcOperator =
            Dateadd =
            addoperator =

            SP_Desc =
            SP_validated =
            SP_Allow_Dups =

            Sp0_Active = string.Empty;

        }

        public CASESP1Entity(DataRow CASESP1)
        {
            if (CASESP1 != null)
            {
                DataRow row = CASESP1;
                Rec_Type = "U";
                Sel_Type = string.Empty;
                Code = row["sp1_servicecode"].ToString();
                Agency = row["sp1_agency"].ToString();
                Dept = row["sp1_dept"].ToString();
                Prog = row["sp1_program"].ToString();
                DateLstc = row["sp1_date_lstc"].ToString();
                lstcOperator = row["sp1_lstc_operator"].ToString();
                Dateadd = row["sp1_date_add"].ToString();
                addoperator = row["sp1_add_operator"].ToString();

                SP_Desc = row["sp0_description"].ToString();
                SP_validated = row["sp0_validated"].ToString();
                SP_Allow_Dups = row["SP0_ALLOW_DUPS"].ToString();
                Sp0_Active = row["SP0_ACTIVE"].ToString();
            }
        }

        public CASESP1Entity(DataRow CASESP1, string strtable)
        {
            if (CASESP1 != null)
            {
                DataRow row = CASESP1;
                Rec_Type = "U";
                Code = row["sp1_servicecode"].ToString();
                Agency = row["sp1_agency"].ToString();
                Dept = row["sp1_dept"].ToString();
                Prog = row["sp1_program"].ToString();
                DateLstc = row["sp1_date_lstc"].ToString();
                lstcOperator = row["sp1_lstc_operator"].ToString();
                Dateadd = row["sp1_date_add"].ToString();
                addoperator = row["sp1_add_operator"].ToString();

                SP_Desc = row["sp0_description"].ToString();
                SP_validated = row["sp0_validated"].ToString();
                SP_Allow_Dups = row["SP0_ALLOW_DUPS"].ToString();
                Sp0_Active = row["SP0_ACTIVE"].ToString();

                Allow_Add_Branch = row["SP0_ALLOW_ADLBRANCH"].ToString();
                //if (row["SP0_ALLOW_ADLBRANCH"].ToString() == "1")
                //    Allow_Add_Branch = "Y";
                Allow_Duplicates = row["SP0_ALLOW_DUPS"].ToString();

                Default_Prog = row["sp0_Def_Prog"].ToString();
                BpCode = row["sp0_pbranch_code"].ToString();
                BpDesc = row["sp0_pbranch_desc"].ToString();
                B1Code = row["sp0_branch1_code"].ToString();
                B1Desc = row["sp0_branch1_desc"].ToString();
                B2Code = row["sp0_branch2_code"].ToString();
                B2Desc = row["sp0_branch2_desc"].ToString();
                B3Code = row["sp0_branch3_code"].ToString();
                B3Desc = row["sp0_branch3_desc"].ToString();
                B4Code = row["sp0_branch4_code"].ToString();
                B4Desc = row["sp0_branch4_desc"].ToString();
                B5Code = row["sp0_branch5_code"].ToString();
                B5Desc = row["sp0_branch5_desc"].ToString();
                Status = row["sp0_statuses"].ToString();
                Legals = row["sp0_legals"].ToString();
                Funds = row["sp0_funds"].ToString();
                Validate = row["sp0_validated"].ToString();
                //DateLstc = row["sp0_date_lstc"].ToString();
                //lstcOperator = row["sp0_lstc_operator"].ToString();
                //Dateadd = row["sp0_date_add"].ToString();
                //addoperator = row["sp0_add_operator"].ToString();

                Auto_Post_SP = row["SP0_ALLOW_AUTOPOST"].ToString();
                Sp0ReadOnly = row["SP0_READONLY"].ToString();

                Sp0Notes = row["SP0_NOTES"].ToString();
                Category = row["SP0_Category"].ToString();
                PartnerPostingType = row["SP0_PPTYPE"].ToString();
                NoSPM = row["SP0_NOSPM"].ToString();
                Household_Type = row["SP0_HH_TYPE"].ToString();
                Maxbenefit = row["SP0_MAX_BEN"].ToString();
                Transactions = row["SP0_NO_TRANS"].ToString();

                Sel_Type = string.Empty;

            }
        }

        public CASESP1Entity(string strCode, string strDesc, string stractive,string strSelType)
        {



            //Agency=
            Code = strCode.ToString().Trim();
            SP_Desc = strDesc.ToString().Trim();
            Sp0_Active = stractive.ToString().Trim();
            Sel_Type = strSelType.ToString();
            //GrpDesc = strGrpDesc.ToString().Trim(); ;
            //Hrd1 = strhrd1;
            //Hrd2 = strhrd2;
            //Hrd3 = strhrd3;
            //Hrd4 = strhrd4;
            //Hrd5 = strhrd5;
            //unit_cnt = strunit;
            //ExAchev = strAchived;
            //per_Achived = strperachieved;
            //CalCost = strcallcost;
            //Count_type = strcount;
            //Row_Type = strrowtype;

        }



        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string SP_Desc { get; set; }
        public string SP_validated { get; set; }
        public string SP_Allow_Dups { get; set; }
        public string Sp0_Active { get; set; }

        public string Sel_Type { get; set; }


        public string Allow_Add_Branch { get; set; }
        public string Default_Prog { get; set; }
        public string Allow_Duplicates { get; set; }
        public string BpCode { get; set; }
        public string BpDesc { get; set; }
        public string B1Code { get; set; }
        public string B1Desc { get; set; }
        public string B2Code { get; set; }
        public string B2Desc { get; set; }
        public string B3Code { get; set; }
        public string B3Desc { get; set; }
        public string B4Code { get; set; }
        public string B4Desc { get; set; }
        public string B5Code { get; set; }
        public string B5Desc { get; set; }
        public string Status { get; set; }
        public string Legals { get; set; }
        public string Funds { get; set; }
        public string Validate { get; set; }
        public string Auto_Post_SP { get; set; }
        public string Sp0ReadOnly { get; set; }
        public string Sp0Notes { get; set; }
        public string Category { get; set; }
        public string PartnerPostingType { get; set; }
        public string NoSPM { get; set; }
        public string Household_Type { get; set; }
        public string Maxbenefit { get; set; }
        public string Transactions { get; set; }




        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class CASESP2Entity
    {
        #region Constructors

        public CASESP2Entity()
        {
            ServPlan =
            Branch =
            Type1 =
            CamCd =
            DateLstc =
            lstcOperator =
            Dateadd =
            addoperator =
            Rec_Type =
            CAMS_Desc =
            CAMS_Status =
            SP2_CAMS_Active =
            SP2_Spl_Ins =
            SP2_Auto_Post = string.Empty;
            Shift_Count = 0;
            Orig_Grp =
            Row =
            Curr_Grp = int.MinValue;

            CAMS_Post_Count =
            CAMS_Active =
            SP2_OBF=
            SP2_COUNT_TYPE=
            Existing_CAMS = string.Empty;
        }

        public CASESP2Entity(DataRow CASESP2)
        {
            if (CASESP2 != null)
            {
                DataRow row = CASESP2;
                Rec_Type = "U";
                ServPlan = row["sp2_serviceplan"].ToString();
                Branch = row["sp2_branch"].ToString();
                Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                Type1 = row["sp2_type"].ToString();
                CamCd = row["sp2_cams_code"].ToString();
                Row = int.Parse(row["sp2_row"].ToString());
                Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                DateLstc = row["sp2_date_lstc"].ToString();
                lstcOperator = row["sp2_lstc_operator"].ToString();
                Dateadd = row["sp2_date_add"].ToString();
                addoperator = row["sp2_add_operator"].ToString();
                CAMS_Desc = row["CAMS_DESC"].ToString();
                CAMS_Status = row["CAMS_STATUS"].ToString();
                SP2_CAMS_Active = row["sp2_Active"].ToString();
                SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                Shift_Count = 0;

                CAMS_Post_Count = row["CAMS_COUNT"].ToString();
                CAMS_Active = row["CAMS_Active"].ToString();
                Existing_CAMS = "Y";
                SP2_OBF = row["SP2_OBF"].ToString();
                SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public CASESP2Entity(DataRow CASESP2, string strfiltercode)
        {
            if (CASESP2 != null)
            {
                DataRow row = CASESP2;
                if (strfiltercode == "CASE4006")
                {
                    Rec_Type = "U";
                    ServPlan = row["sp2_serviceplan"].ToString();
                    Branch = row["sp2_branch"].ToString();
                    Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                    Type1 = row["sp2_type"].ToString();
                    CamCd = row["sp2_cams_code"].ToString();
                    Row = int.Parse(row["sp2_row"].ToString());
                    Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                    DateLstc = row["sp2_date_lstc"].ToString();
                    lstcOperator = row["sp2_lstc_operator"].ToString();
                    Dateadd = row["sp2_date_add"].ToString();
                    addoperator = row["sp2_add_operator"].ToString();
                    CAMS_Desc = row["CAMS_DESC"].ToString();
                    SP2_CAMS_Active = row["sp2_Active"].ToString();
                    SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                    SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                    Shift_Count = 0;

                    CAMS_Active = row["CAMS_Active"].ToString();
                    Existing_CAMS = "Y";

                    SP2_OBF = row["SP2_OBF"].ToString();
                    SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                    //Spmaddoperator = row["spm_add_operator"].ToString();
                }
                else if (strfiltercode == "Funnel")
                {
                    Rec_Type = "U";
                    ServPlan = row["sp2_serviceplan"].ToString();
                    Branch = row["sp2_branch"].ToString();
                    Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                    Type1 = row["sp2_type"].ToString();
                    CamCd = row["sp2_cams_code"].ToString();
                    Row = int.Parse(row["sp2_row"].ToString());
                    Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                    DateLstc = row["sp2_date_lstc"].ToString();
                    lstcOperator = row["sp2_lstc_operator"].ToString();
                    Dateadd = row["sp2_date_add"].ToString();
                    addoperator = row["sp2_add_operator"].ToString();
                    //CAMS_Desc = row["CAMS_DESC"].ToString();
                    SP2_CAMS_Active = row["sp2_Active"].ToString();
                    SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                    SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                    SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                    Shift_Count = 0;

                    //CAMS_Active = row["CAMS_Active"].ToString();
                    //Existing_CAMS = "Y";
                    //Spmaddoperator = row["spm_add_operator"].ToString();
                }
                else if (strfiltercode == "ADMN0030")
                {   
                    Rec_Type = "U";
                    ServPlan = row["sp2_serviceplan"].ToString();
                    Branch = row["sp2_branch"].ToString();
                    Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                    Type1 = row["sp2_type"].ToString();
                    CamCd = row["sp2_cams_code"].ToString();
                    Row = int.Parse(row["sp2_row"].ToString());
                    Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                    DateLstc = row["sp2_date_lstc"].ToString();
                    lstcOperator = row["sp2_lstc_operator"].ToString();
                    Dateadd = row["sp2_date_add"].ToString();
                    addoperator = row["sp2_add_operator"].ToString();
                    CAMS_Desc = row["CAMS_DESC"].ToString();
                    CAMS_Status = row["CAMS_STATUS"].ToString();
                    SP2_CAMS_Active = row["sp2_Active"].ToString();
                    SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                    SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                    Shift_Count = 0;

                    //CAMS_Post_Count = row["CAMS_COUNT"].ToString();
                    CAMS_Active = row["CAMS_Active"].ToString();
                    Existing_CAMS = "Y";
                    SP2_OBF = row["SP2_OBF"].ToString();
                    SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                    //Spmaddoperator = row["spm_add_operator"].ToString();
                }
                else if (strfiltercode == "SP2")
                {
                    Rec_Type = "U";
                    ServPlan = row["sp2_serviceplan"].ToString();
                    Branch = row["sp2_branch"].ToString();
                    Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                    Type1 = row["sp2_type"].ToString();
                    CamCd = row["sp2_cams_code"].ToString();
                    Row = int.Parse(row["sp2_row"].ToString());
                    Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                    DateLstc = row["sp2_date_lstc"].ToString();
                    lstcOperator = row["sp2_lstc_operator"].ToString();
                    Dateadd = row["sp2_date_add"].ToString();
                    addoperator = row["sp2_add_operator"].ToString();
                    if (row["sp2_type"].ToString() == "CA")
                        CAMS_Desc = row["CA_DESC"].ToString();
                    else
                        CAMS_Desc = row["MS_DESC"].ToString();
                    SP2_CAMS_Active = row["sp2_Active"].ToString();
                    SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                    SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                    Shift_Count = 0;
                    SP2_OBF = row["SP2_OBF"].ToString();
                    SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                    //CAMS_Active = row["CAMS_Active"].ToString();
                    //Existing_CAMS = "Y";
                    //Spmaddoperator = row["spm_add_operator"].ToString();
                }
                else if (strfiltercode == "Report")
                {
                    Rec_Type = "U";
                    ServPlan = row["sp2_serviceplan"].ToString();
                    Branch = row["sp2_branch"].ToString();
                    Orig_Grp = int.Parse(row["sp2_orig_grp"].ToString());
                    Type1 = row["sp2_type"].ToString();
                    CamCd = row["sp2_cams_code"].ToString();
                    Row = int.Parse(row["sp2_row"].ToString());
                    Curr_Grp = int.Parse(row["sp2_curr_grp"].ToString());
                    DateLstc = row["sp2_date_lstc"].ToString();
                    lstcOperator = row["sp2_lstc_operator"].ToString();
                    Dateadd = row["sp2_date_add"].ToString();
                    addoperator = row["sp2_add_operator"].ToString();
                    if (row["sp2_type"].ToString() == "CA")
                        CAMS_Desc = row["CA_DESC"].ToString();
                    else
                        CAMS_Desc = row["MS_DESC"].ToString();
                    SP2_CAMS_Active = row["sp2_Active"].ToString();
                    SP2_Auto_Post = row["sp2_Auto_Post"].ToString();
                    SP2_Spl_Ins = row["SP2_SPL_INS"].ToString();
                    Shift_Count = 0;
                    SP2_OBF = row["SP2_OBF"].ToString();
                    SP2_COUNT_TYPE = row["SP2_COUNT_TYPE"].ToString();
                    //CAMS_Active = row["CAMS_Active"].ToString();
                    //Existing_CAMS = "Y";
                    //Spmaddoperator = row["spm_add_operator"].ToString();
                }
            }
        }

        public CASESP2Entity(CASESP2Entity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                ServPlan = Entity.ServPlan;
                Branch = Entity.Branch;
                Orig_Grp = Entity.Orig_Grp;
                Type1 = Entity.Type1;
                CamCd = Entity.CamCd;
                Row = Entity.Row;
                Curr_Grp = Entity.Curr_Grp;
                DateLstc = Entity.DateLstc;
                lstcOperator = Entity.lstcOperator;
                Dateadd = Entity.Dateadd;
                addoperator = Entity.addoperator;
                CAMS_Desc = Entity.CAMS_Desc;
                CAMS_Status = Entity.CAMS_Status;
                Shift_Count = Entity.Shift_Count;
                SP2_CAMS_Active = Entity.SP2_CAMS_Active;
                SP2_Auto_Post = Entity.SP2_Auto_Post;
                SP2_Spl_Ins = Entity.SP2_Spl_Ins;
                if (SP2_Auto_Post.Trim() != "Y")
                    SP2_Auto_Post = "N";


                CAMS_Post_Count = Entity.CAMS_Post_Count;
                CAMS_Active = Entity.CAMS_Active;
                Existing_CAMS = Entity.Existing_CAMS;
                SP2_OBF = Entity.SP2_OBF;
                SP2_COUNT_TYPE = Entity.SP2_COUNT_TYPE;
            }
        }


        #endregion

        #region Properties

        public string ServPlan { get; set; }
        public string Branch { get; set; }
        public int Orig_Grp { get; set; }
        public string Type1 { get; set; }
        public string CamCd { get; set; }
        public int Row { get; set; }
        public int Curr_Grp { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Rec_Type { get; set; }
        public string CAMS_Desc { get; set; }
        public string CAMS_Status { get; set; }
        public int Shift_Count { get; set; }
        public string CAMS_Post_Count { get; set; }
        public string CAMS_Active { get; set; }
        public string SP2_CAMS_Active { get; set; }
        public string SP2_Auto_Post { get; set; }
        public string SP2_Spl_Ins { get; set; }
        public string Existing_CAMS { get; set; }

        public string SP2_OBF{ get; set; }
        public string SP2_COUNT_TYPE { get; set; }


        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }


    public class CTAPPRISEENTITY
    {
        #region Constructors

        public CTAPPRISEENTITY()
        {
            MST_AGENCY =
            MST_DEPT =
            MST_PROGRAM =
            MST_YEAR =
            MST_APP_NO =
            NAME =
            STREET =
            SUFIX =
            HNO =
            APPNO =
            CITY =
            ZIP =
            ZIP_PLUS =
            MST_CLIENT_ID =
            LPB_FAP_NO_INHH =

            LPB_FAP_INCOME =
            HEAT =
            HEAT_AMT =
            HEAT_DATE =
            CRISIS =
            CRISIS_AMT =
            CRISIS_DATE =
            OTHERS =
            OTHERS_AMT =
            OTHERS_DATE =
            HEAT_PAY_AMT =
            CRISIS_PAY_AMT =
            OTHER_PAY_AMT =
            AGE5 =
            AGE60 =
            TENANCY =
            FUEL =
            DISABLES =
            FUND =
            MST_HEAT_INC_RENT =
            Area =
            Phone = string.Empty;

        }

        public CTAPPRISEENTITY(DataRow CASESP1)
        {
            if (CASESP1 != null)
            {
                DataRow row = CASESP1;

                MST_AGENCY = row["MST_AGENCY"].ToString();
                MST_DEPT = row["MST_DEPT"].ToString();

                MST_PROGRAM = row["MST_PROGRAM"].ToString();
                MST_YEAR = row["MST_YEAR"].ToString();
                MST_APP_NO = row["MST_APP_NO"].ToString();
                NAME = row["NAME"].ToString();
                STREET = row["STREET"].ToString();
                SUFIX = row["SUFIX"].ToString();
                HNO = row["HNO"].ToString();
                APPNO = row["APPNO"].ToString();
                CITY = row["CITY"].ToString();
                ZIP = row["ZIP"].ToString();
                ZIP_PLUS = row["MST_ZIPPLUS"].ToString();

                MST_CLIENT_ID = row["MST_CLIENT_ID"].ToString();
                LPB_FAP_NO_INHH = row["LPB_FAP_NO_INHH"].ToString();

                LPB_FAP_INCOME = row["LPB_FAP_INCOME"].ToString();
                HEAT = row["HEAT"].ToString();
                HEAT_AMT = row["HEAT_AMT"].ToString();
                HEAT_DATE = row["HEAT_DATE"].ToString();
                CRISIS = row["CRISIS"].ToString();
                CRISIS_AMT = row["CRISIS_AMT"].ToString();
                CRISIS_DATE = row["CRISIS_DATE"].ToString();
                OTHERS = row["OTHERS"].ToString();
                OTHERS_AMT = row["OTHERS_AMT"].ToString();
                OTHERS_DATE = row["OTHERS_DATE"].ToString();
                AGE5 = row["AGE5"].ToString();
                AGE60 = row["AGE60"].ToString();
                DISABLES = row["DISABLES"].ToString();

                TENANCY = row["TENANCY"].ToString();
                FUEL = row["FUEL"].ToString();
                Area = row["MST_AREA"].ToString();
                Phone = row["MST_PHONE"].ToString();

                FUND = row["LPB_FUND_CODE"].ToString();
                MST_HEAT_INC_RENT = row["MST_HEAT_INC_RENT"].ToString();

                HEAT_PAY_AMT = row["HEAT_PAY_AMT"].ToString();
                CRISIS_PAY_AMT = row["CRISIS_PAY_AMT"].ToString();
                OTHER_PAY_AMT = row["OTHER_PAY_AMT"].ToString();

            }
        }


        #endregion

        #region Properties

        public string MST_AGENCY { get; set; }
        public string MST_DEPT { get; set; }
        public string MST_PROGRAM { get; set; }
        public string MST_YEAR { get; set; }
        public string MST_APP_NO { get; set; }
        public string NAME { get; set; }
        public string STREET { get; set; }
        public string SUFIX { get; set; }
        public string HNO { get; set; }
        public string APPNO { get; set; }
        public string CITY { get; set; }
        public string ZIP { get; set; }
        public string ZIP_PLUS { get; set; }
        public string MST_CLIENT_ID { get; set; }
        public string LPB_FAP_NO_INHH { get; set; }
        public string LPB_FAP_INCOME { get; set; }

        public string HEAT { get; set; }
        public string HEAT_AMT { get; set; }
        public string HEAT_DATE { get; set; }

        public string CRISIS { get; set; }
        public string CRISIS_AMT { get; set; }
        public string CRISIS_DATE { get; set; }

        public string OTHERS { get; set; }
        public string OTHERS_AMT { get; set; }
        public string OTHERS_DATE { get; set; }

        public string AGE5 { get; set; }
        public string AGE60 { get; set; }
        public string DISABLES { get; set; }

        public string HEAT_PAY_AMT { get; set; }
        public string CRISIS_PAY_AMT { get; set; }
        public string OTHER_PAY_AMT { get; set; }


        public string TENANCY { get; set; }
        public string FUEL { get; set; }

        public string FUND { get; set; }
        public string MST_HEAT_INC_RENT { get; set; }

        public string Area { get; set; }
        public string Phone { get; set; }


        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }
    public class MSMASTEntity
    {
        #region Constructors

        public MSMASTEntity()
        {
            Code = string.Empty;
            Desc = string.Empty;
            Active = string.Empty;
            AutoPost = string.Empty;
            Type1 = string.Empty;
            //Hie = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;
            Dateadd = string.Empty;
            addoperator = string.Empty;
            Sel_SW = false;
            Can_Delete = true;
            Post_Exists = false;

        }

        public MSMASTEntity(DataRow MSMAST)
        {
            if (MSMAST != null)
            {
                DataRow row = MSMAST;
                Code = row["MS_CODE"].ToString().Trim();
                Desc = row["MS_DESC"].ToString().Trim();
                Active = row["MS_ACTIVE"].ToString();
                AutoPost = row["MS_AUTO_POST"].ToString();
                Type1 = row["MS_TYPE"].ToString();
                //Hie = row["MS_HIERARCHY"].ToString();
                DateLstc = row["MS_DATE_LSTC"].ToString();
                lstcOperator = row["MS_LSTC_OPERATOR"].ToString();
                Dateadd = row["MS_DATE_ADD"].ToString();
                addoperator = row["MS_ADD_OPERATOR"].ToString();
                Sel_SW = false;
                Can_Delete = true;
                InActiveFlag = "false";
                Post_Exists = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public MSMASTEntity(DataRow MSMAST,string Table,string stable)
        {
            if (MSMAST != null)
            {
                DataRow row = MSMAST;
                Code = row["SP2_CAMS_CODE"].ToString().Trim();
                Desc = row["CAMS_DESC"].ToString().Trim();
                Active = row["CAMS_ACTIVE"].ToString();
                //AutoPost = row["MS_AUTO_POST"].ToString();
                //Type1 = row["MS_TYPE"].ToString();
                ////Hie = row["MS_HIERARCHY"].ToString();
                //DateLstc = row["MS_DATE_LSTC"].ToString();
                //lstcOperator = row["MS_LSTC_OPERATOR"].ToString();
                //Dateadd = row["MS_DATE_ADD"].ToString();
                //addoperator = row["MS_ADD_OPERATOR"].ToString();
                //Sel_SW = false;
                //Can_Delete = true;
                //InActiveFlag = "false";
                //Post_Exists = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public MSMASTEntity(DataRow MSMAST, string sel)
        {
            if (MSMAST != null)
            {
                DataRow row = MSMAST;
                Code = row["MS_CODE"].ToString().Trim();
                Desc = row["MS_DESC"].ToString().Trim();
                //Active = row["MS_ACTIVE"].ToString();
                //AutoPost = row["MS_AUTO_POST"].ToString();
                //Type1 = row["MS_TYPE"].ToString();
                ////Hie = row["MS_HIERARCHY"].ToString();
                //DateLstc = row["MS_DATE_LSTC"].ToString();
                //lstcOperator = row["MS_LSTC_OPERATOR"].ToString();
                Dateadd = row["MS_DATE_ADD"].ToString();
                SPM_Code = row["SPM_SERVICE_PLAN"].ToString();
                SP_DESC = row["SP0_DESCRIPTION"].ToString();
                Branch = row["BRANCH_DESC"].ToString();
                SP_Add_Date = row["SP0_DATE_ADD"].ToString();
                //addoperator = row["MS_ADD_OPERATOR"].ToString();
                //Sel_SW = false;
                //Can_Delete = true;
                //InActiveFlag = "false";
                //Post_Exists = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public MSMASTEntity(MSMASTEntity Entity)
        {
            Code = Entity.Code;
            Desc = Entity.Desc;
            Active = Entity.Active;
            AutoPost = Entity.AutoPost;
            Type1 = Entity.Type1;
            DateLstc = Entity.DateLstc;
            lstcOperator = Entity.lstcOperator;
            Dateadd = Entity.Dateadd;
            addoperator = Entity.addoperator;
            Sel_SW = false;
            Launch_Sel_SW = false;
            Can_Delete = true;
            InActiveFlag = "false";
        }


        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string AutoPost { get; set; }
        public string Type1 { get; set; }
        //public string Hie { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_SW { get; set; }
        public bool Launch_Sel_SW { get; set; }
        public bool Can_Delete { get; set; }
        public string InActiveFlag { get; set; }
        public bool Post_Exists { get; set; }


        public string SPM_Code { get; set; }
        public string SP_DESC { get; set; }
        public string Branch { get; set; }
        public string SP_Add_Date { get; set; }
        //public bool Post_Exists { get; set; }

        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class CAMASTEntity
    {
        #region Constructors

        public CAMASTEntity()
        {
            Code = string.Empty;
            //Fund = string.Empty;
            //Seq = string.Empty;
            //Agency = string.Empty;
            //Dept = string.Empty;
            //Prog = string.Empty;
            Desc = string.Empty;
            //Poverty = string.Empty;
            Active = string.Empty;
            AutoPost = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;
            Dateadd = string.Empty;
            addoperator = string.Empty;
            Sel_SW = false;
            Launch_Sel_SW = false;
            Can_Delete = true;
            Post_Exists = false;
            SpanishDesc = string.Empty;
            SendtoPIP = string.Empty;
            PIPActive = string.Empty;
            UOM = string.Empty;
            TransactionAlert = string.Empty;
            VendPaycat = string.Empty;

        }

        public CAMASTEntity(DataRow CAMAST)
        {
            if (CAMAST != null)
            {
                DataRow row = CAMAST;
                Code = row["CA_CODE"].ToString().Trim();
                //Fund = row["CA_FUND"].ToString();
                //Seq = row["CA_SEQ"].ToString();
                //Agency = row["CA_AGENCY"].ToString();
                //Dept = row["CA_DEPT"].ToString();
                //Prog = row["CA_PROGRAM"].ToString();
                Desc = row["CA_DESC"].ToString().Trim();
                //Poverty = row["CA_OUTOFPOVERTY"].ToString();
                Active = row["CA_ACTIVE"].ToString();
                AutoPost = row["CA_AUTO_POST"].ToString();
                DateLstc = row["CA_DATE_LSTC"].ToString();
                lstcOperator = row["CA_LSTC_OPERATOR"].ToString();
                Dateadd = row["CA_DATE_ADD"].ToString();
                addoperator = row["CA_ADD_OPERATOR"].ToString();
                if (row.Table.Columns.Contains("CA_SNAME"))
                    SpanishDesc = row["CA_SNAME"].ToString();
                else
                    SpanishDesc = string.Empty;

                if (row.Table.Columns.Contains("CA_SEND_PIP"))
                    SendtoPIP = row["CA_SEND_PIP"].ToString();
                else
                    SendtoPIP = string.Empty;

                if (row.Table.Columns.Contains("CA_PIP_ACTIVE"))
                    PIPActive = row["CA_PIP_ACTIVE"].ToString();
                else
                    PIPActive = string.Empty;

                if (row.Table.Columns.Contains("CA_UOM"))
                    UOM = row["CA_UOM"].ToString();
                else
                    UOM = string.Empty;

                if (row.Table.Columns.Contains("CA_TRANS_ALERT"))
                    TransactionAlert = row["CA_TRANS_ALERT"].ToString();
                else
                    TransactionAlert = string.Empty;

                if (row.Table.Columns.Contains("CA_VEND_PAY_CAT"))
                    VendPaycat = row["CA_VEND_PAY_CAT"].ToString();
                else
                    VendPaycat = string.Empty;



                Sel_SW = false;
                Can_Delete = true;
                Post_Exists = false;

                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public CAMASTEntity(DataRow CAMAST,string table)
        {
            if (CAMAST != null)
            {
                DataRow row = CAMAST;
                Code = row["SP2_CAMS_CODE"].ToString().Trim();
                //Fund = row["CA_FUND"].ToString();
                //Seq = row["CA_SEQ"].ToString();
                //Agency = row["CA_AGENCY"].ToString();
                //Dept = row["CA_DEPT"].ToString();
                //Prog = row["CA_PROGRAM"].ToString();
                Desc = row["CAMS_DESC"].ToString().Trim();
                //Poverty = row["CA_OUTOFPOVERTY"].ToString();
                Active = row["CAMS_ACTIVE"].ToString();
                //AutoPost = row["CA_AUTO_POST"].ToString();
                //DateLstc = row["CA_DATE_LSTC"].ToString();
                //lstcOperator = row["CA_LSTC_OPERATOR"].ToString();
                //Dateadd = row["CA_DATE_ADD"].ToString();
                //addoperator = row["CA_ADD_OPERATOR"].ToString();
                //if (row.Table.Columns.Contains("CA_SNAME"))
                //    SpanishDesc = row["CA_SNAME"].ToString();
                //else
                //    SpanishDesc = string.Empty;

                //if (row.Table.Columns.Contains("CA_SEND_PIP"))
                //    SendtoPIP = row["CA_SEND_PIP"].ToString();
                //else
                //    SendtoPIP = string.Empty;

                //if (row.Table.Columns.Contains("CA_PIP_ACTIVE"))
                //    PIPActive = row["CA_PIP_ACTIVE"].ToString();
                //else
                //    PIPActive = string.Empty;

                //if (row.Table.Columns.Contains("CA_UOM"))
                //    UOM = row["CA_UOM"].ToString();
                //else
                //    UOM = string.Empty;

                //if (row.Table.Columns.Contains("CA_TRANS_ALERT"))
                //    TransactionAlert = row["CA_TRANS_ALERT"].ToString();
                //else
                //    TransactionAlert = string.Empty;

                //if (row.Table.Columns.Contains("CA_VEND_PAY_CAT"))
                //    VendPaycat = row["CA_VEND_PAY_CAT"].ToString();
                //else
                //    VendPaycat = string.Empty;



                Sel_SW = false;
                Can_Delete = true;
                Post_Exists = false;

                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public CAMASTEntity(CAMASTEntity Entity)
        {
            if (Entity != null)
            {

                Code = Entity.Code;
                //Fund = row["CA_FUND"].ToString();
                //Seq = row["CA_SEQ"].ToString();
                //Agency = row["CA_AGENCY"].ToString();
                //Dept = row["CA_DEPT"].ToString();
                //Prog = row["CA_PROGRAM"].ToString();
                Desc = Entity.Desc;
                //Poverty = row["CA_OUTOFPOVERTY"].ToString();
                Active = Entity.Active;
                AutoPost = Entity.AutoPost;
                DateLstc = Entity.DateLstc;
                lstcOperator = Entity.lstcOperator;
                Dateadd = Entity.Dateadd;
                addoperator = Entity.addoperator;
                SpanishDesc = Entity.SpanishDesc;
                UOM = Entity.UOM;
                TransactionAlert = Entity.TransactionAlert;
                VendPaycat = Entity.VendPaycat;
                Sel_SW = false;
                Can_Delete = true;
                Post_Exists = false;

                //Spmaddoperator = row["spm_add_operator"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Code { get; set; }
        //public string Fund { get; set; }
        //public string Seq { get; set; }
        //public string Agency { get; set; }
        //public string Dept { get; set; }
        //public string Prog { get; set; }
        public string Desc { get; set; }
        //public string Poverty { get; set; }
        public string Active { get; set; }
        public string AutoPost { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_SW { get; set; }
        public bool Launch_Sel_SW { get; set; }
        public bool Can_Delete { get; set; }
        public bool Post_Exists { get; set; }
        public string SpanishDesc { get; set; }
        public string SendtoPIP { get; set; }
        public string PIPActive { get; set; }

        public string UOM { get; set; }

        public string TransactionAlert { get; set; }
        public string VendPaycat { get; set; }

        #endregion


    }

    public class CAPRICESEntity
    {
        #region Constructors

        public CAPRICESEntity()
        {
            CAP_ID = string.Empty;
            Code = string.Empty;
            FDate = string.Empty;
            TDate = string.Empty;
            UnitPrice = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;

        }

        public CAPRICESEntity(DataRow CAMAST)
        {
            if (CAMAST != null)
            {
                DataRow row = CAMAST;
                CAP_ID = row["CAP_ID"].ToString().Trim();
                Code = row["CAP_CODE"].ToString().Trim();
                FDate = row["CAP_FDATE"].ToString().Trim();
                TDate = row["CAP_LDATE"].ToString();
                UnitPrice = row["CAP_PRICE"].ToString();
                DateLstc = row["CAP_DATE_LSTC"].ToString();
                lstcOperator = row["CAP_LSTC_OPERATOR"].ToString();

            }
        }

        public CAPRICESEntity(CAPRICESEntity Entity)
        {
            if (Entity != null)
            {

                Code = Entity.Code;
                FDate = Entity.FDate;
                TDate = Entity.TDate;
                UnitPrice = Entity.UnitPrice;
                DateLstc = Entity.DateLstc;
                lstcOperator = Entity.lstcOperator;

            }
        }

        #endregion

        #region Properties

        public string CAP_ID { get; set; }
        public string Code { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string UnitPrice { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        //public string Dateadd { get; set; }
        //public string addoperator { get; set; }
        public string Mode { get; set; }
        //public bool Sel_SW { get; set; }
        //public bool Launch_Sel_SW { get; set; }
        //public bool Can_Delete { get; set; }
        //public bool Post_Exists { get; set; }
        //public string SpanishDesc { get; set; }
        //public string SendtoPIP { get; set; }
        //public string PIPActive { get; set; }

        //public string UOM { get; set; }

        //public string TransactionAlert { get; set; }
        //public string VendPaycat { get; set; }

        #endregion


    }

    public class Csb14GroupEntity
    {
        #region Constructors

        public Csb14GroupEntity()
        {

            RefFDate = string.Empty;
            RefTDate = string.Empty;
            GrpCode = string.Empty;
            TblCode = string.Empty;
            GrpDesc = string.Empty;
            Hrd1 = string.Empty;
            Incld1 = string.Empty;
            Hrd2 = string.Empty;
            Incld2 = string.Empty;
            Hrd3 = string.Empty;
            Incld3 = string.Empty;
            Hrd4 = string.Empty;
            Incld4 = string.Empty;
            Hrd5 = string.Empty;
            Incld5 = string.Empty;
            CntIndic = string.Empty;
            ExAchev = string.Empty;
            CalCost = string.Empty;
            UseSer = string.Empty;
            Duplicate = string.Empty;
            AFrom = string.Empty;
            Ato = string.Empty;
            Disable = string.Empty;
            GoalCds = string.Empty;

            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;

        }

        public Csb14GroupEntity(bool Intialize)
        {
            if (Intialize)
            {
                RefFDate = null;
                RefTDate = null;
                GrpCode = null;
                TblCode = null;
                GrpDesc = null;
                Hrd1 = null;
                Incld1 = null;
                Hrd2 = null;
                Incld2 = null;
                Hrd3 = null;
                Incld3 = null;
                Hrd4 = null;
                Incld4 = null;
                Hrd5 = null;
                Incld5 = null;
                CntIndic = null;
                ExAchev = null;
                CalCost = null;
                UseSer = null;
                Duplicate = null;
                AFrom = null;
                Ato = null;
                Disable = null;
                GoalCds = null;

                DateLSTC = null;
                LSTCOperator = null;
                DateAdd = null;
                AddOperator = null;
            }
        }

        public Csb14GroupEntity(Csb14GroupEntity Entity)
        {
            RefFDate = Entity.RefFDate;
            RefTDate = Entity.RefTDate;
            GrpCode = Entity.GrpCode;
            TblCode = Entity.GrpDesc;
            GrpDesc = Entity.GrpDesc;
            Hrd1 = Entity.Hrd1;
            Incld1 = Entity.Incld1;
            Hrd2 = Entity.Hrd2;
            Incld2 = Entity.Incld2;
            Hrd3 = Entity.Hrd3;
            Incld3 = Entity.Incld3;
            Hrd4 = Entity.Hrd4;
            Incld4 = Entity.Incld4;
            Hrd5 = Entity.Hrd5;
            Incld5 = Entity.Incld5;
            CntIndic = Entity.CntIndic;
            ExAchev = Entity.ExAchev;
            CalCost = Entity.CalCost;
            UseSer = Entity.UseSer;
            Duplicate = Entity.Duplicate;
            AFrom = Entity.AFrom;
            Ato = Entity.Ato;
            Disable = Entity.Disable;
            GoalCds = Entity.GoalCds;

            DateLSTC = Entity.DateLSTC;
            LSTCOperator = Entity.LSTCOperator;
            DateAdd = Entity.DateAdd;
            AddOperator = Entity.AddOperator;

        }

        public Csb14GroupEntity(DataRow CSB14GRP)
        {
            if (CSB14GRP != null)
            {
                DataRow row = CSB14GRP;

                RefFDate = row["csb14grp_ref_fdate"].ToString();
                RefTDate = row["csb14grp_ref_tdate"].ToString();
                GrpCode = row["csb14grp_group_code"].ToString().Trim();
                TblCode = row["csb14grp_table_code"].ToString().Trim();
                GrpDesc = row["csb14grp_desc"].ToString();
                Hrd1 = row["csb14grp_cnt_hdr1"].ToString();
                Incld1 = row["csb14grp_cnt_incld1"].ToString();
                Hrd2 = row["csb14grp_cnt_hdr2"].ToString();
                Incld2 = row["csb14grp_cnt_incld2"].ToString();
                Hrd3 = row["csb14grp_cnt_hdr3"].ToString();
                Incld3 = row["csb14grp_cnt_incld3"].ToString();
                Hrd4 = row["csb14grp_cnt_hdr4"].ToString();
                Incld4 = row["csb14grp_cnt_incld4"].ToString();
                Hrd5 = row["csb14grp_cnt_hdr5"].ToString();
                Incld5 = row["csb14grp_cnt_incld5"].ToString();
                CntIndic = row["csb14grp_cnt_indicator"].ToString();
                ExAchev = row["csb14grp_expect_achieve"].ToString();
                CalCost = row["csb14grp_calc_cost"].ToString();
                UseSer = row["csb14grp_use_servs"].ToString();
                Duplicate = row["csb14grp_duplicated"].ToString();
                AFrom = row["csb14grp_age_from"].ToString();
                Ato = row["csb14grp_age_to"].ToString();
                Disable = row["csb14grp_disabled"].ToString();
                GoalCds = row["csb14grp_goal_codes"].ToString();

                DateLSTC = row["csb14grp_date_lstc"].ToString();
                LSTCOperator = row["csb14grp_lstc_operator"].ToString();
                DateAdd = row["csb14grp_date_add"].ToString();
                AddOperator = row["csb14grp_add_operator"].ToString();
                InActiveFlag = "false";
            }
        }


        #endregion

        #region Properties

        public string RefFDate { get; set; }
        public string RefTDate { get; set; }
        public string GrpCode { get; set; }
        public string TblCode { get; set; }
        public string GrpDesc { get; set; }
        public string Hrd1 { get; set; }
        public string Incld1 { get; set; }
        public string Hrd2 { get; set; }
        public string Incld2 { get; set; }
        public string Hrd3 { get; set; }
        public string Incld3 { get; set; }
        public string Hrd4 { get; set; }
        public string Incld4 { get; set; }
        public string Hrd5 { get; set; }
        public string Incld5 { get; set; }
        public string CntIndic { get; set; }
        public string ExAchev { get; set; }
        public string CalCost { get; set; }
        public string UseSer { get; set; }
        public string Duplicate { get; set; }
        public string AFrom { get; set; }
        public string Ato { get; set; }
        public string Disable { get; set; }
        public string GoalCds { get; set; }

        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Mode { get; set; }
        public string InActiveFlag { get; set; }
        #endregion

    }

    public class RCsb14GroupEntity
    {
        #region Constructors

        public RCsb14GroupEntity()
        {

            Agency = string.Empty;
            Code = string.Empty;
            Name = string.Empty;
            GrpCode = string.Empty;
            TblCode = string.Empty;
            GrpDesc = string.Empty;
            Hrd1 = string.Empty;
            Incld1 = string.Empty;
            Hrd2 = string.Empty;
            Incld2 = string.Empty;
            Hrd3 = string.Empty;
            Incld3 = string.Empty;
            Hrd4 = string.Empty;
            Incld4 = string.Empty;
            Hrd5 = string.Empty;
            Incld5 = string.Empty;
            CntIndic = string.Empty;
            ExAchev = string.Empty;
            CalCost = string.Empty;
            UseSer = string.Empty;
            Duplicate = string.Empty;
            AFrom = string.Empty;
            Ato = string.Empty;
            Disable = string.Empty;
            GoalCds = string.Empty;

            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            OFdate = string.Empty;
            OTdate = string.Empty;
            unit_cnt = string.Empty;
            per_Achived = string.Empty;
            Count_type = string.Empty;
            Row_Type = string.Empty;
            IndSwitch = string.Empty;
            DomainHie = string.Empty;
            Prog_Switch = string.Empty;
            Active = string.Empty;
            PPR_SW = string.Empty;


        }

        public RCsb14GroupEntity(bool Intialize)
        {
            if (Intialize)
            {
                Agency = null;
                Code = null;
                Name = null;
                GrpCode = null;
                TblCode = null;
                GrpDesc = null;
                Hrd1 = null;
                Incld1 = null;
                Hrd2 = null;
                Incld2 = null;
                Hrd3 = null;
                Incld3 = null;
                Hrd4 = null;
                Incld4 = null;
                Hrd5 = null;
                Incld5 = null;
                CntIndic = null;
                ExAchev = null;
                CalCost = null;
                UseSer = null;
                Duplicate = null;
                AFrom = null;
                Ato = null;
                Disable = null;
                GoalCds = null;

                DateLSTC = null;
                LSTCOperator = null;
                DateAdd = null;
                AddOperator = null;
                OFdate = null;
                OFdate = null;
                unit_cnt = null;
                per_Achived = null;
                Count_type = null;
                Row_Type = null;
                IndSwitch = null;
                DomainHie = null;
                Prog_Switch = null;
                Active = null;
                PPR_SW = null;
            }
        }

        public RCsb14GroupEntity(RCsb14GroupEntity Entity)
        {
            Agency = Entity.Agency;
            Code = Entity.OFdate;
            Name = Entity.Name;
            GrpCode = Entity.GrpCode;
            TblCode = Entity.GrpDesc;
            GrpDesc = Entity.GrpDesc;
            Hrd1 = Entity.Hrd1;
            Incld1 = Entity.Incld1;
            Hrd2 = Entity.Hrd2;
            Incld2 = Entity.Incld2;
            Hrd3 = Entity.Hrd3;
            Incld3 = Entity.Incld3;
            Hrd4 = Entity.Hrd4;
            Incld4 = Entity.Incld4;
            Hrd5 = Entity.Hrd5;
            Incld5 = Entity.Incld5;
            CntIndic = Entity.CntIndic;
            ExAchev = Entity.ExAchev;
            CalCost = Entity.CalCost;
            UseSer = Entity.UseSer;
            Duplicate = Entity.Duplicate;
            AFrom = Entity.AFrom;
            Ato = Entity.Ato;
            Disable = Entity.Disable;
            GoalCds = Entity.GoalCds;

            DateLSTC = Entity.DateLSTC;
            LSTCOperator = Entity.LSTCOperator;
            DateAdd = Entity.DateAdd;
            AddOperator = Entity.AddOperator;
            OFdate = Entity.OFdate;
            OTdate = Entity.OTdate;
            IndSwitch = Entity.IndSwitch;
            DomainHie = Entity.DomainHie;
            Prog_Switch = string.Empty;
            Active = string.Empty;
            PPR_SW = string.Empty;

        }

        public RCsb14GroupEntity(DataRow CSB14GRP)
        {
            if (CSB14GRP != null)
            {
                DataRow row = CSB14GRP;

                Agency = row["RNGGRP_AGENCY"].ToString();
                Code = row["RNGGRP_CODE"].ToString();
                //Name = row["RNGGRP_NAME"].ToString();
                GrpCode = row["RNGgrp_group_code"].ToString().Trim();
                TblCode = row["RNGgrp_table_code"].ToString().Trim();
                GrpDesc = row["RNGgrp_desc"].ToString();
                Hrd1 = row["RNGgrp_cnt_hdr1"].ToString();
                Incld1 = row["RNGgrp_cnt_incld1"].ToString();
                Hrd2 = row["RNGgrp_cnt_hdr2"].ToString();
                Incld2 = row["RNGgrp_cnt_incld2"].ToString();
                Hrd3 = row["RNGgrp_cnt_hdr3"].ToString();
                Incld3 = row["RNGgrp_cnt_incld3"].ToString();
                Hrd4 = row["RNGgrp_cnt_hdr4"].ToString();
                Incld4 = row["RNGgrp_cnt_incld4"].ToString();
                Hrd5 = row["RNGgrp_cnt_hdr5"].ToString();
                Incld5 = row["RNGgrp_cnt_incld5"].ToString();
                CntIndic = row["RNGgrp_cnt_indicator"].ToString();
                ExAchev = row["RNGgrp_expect_achieve"].ToString();
                CalCost = row["RNGgrp_calc_cost"].ToString();
                UseSer = row["RNGgrp_use_servs"].ToString();
                Duplicate = row["RNGgrp_duplicated"].ToString();
                AFrom = row["RNGgrp_age_from"].ToString();
                Ato = row["RNGgrp_age_to"].ToString();
                Disable = row["RNGgrp_disabled"].ToString();
                GoalCds = row["RNGgrp_goal_codes"].ToString();

                DateLSTC = row["RNGgrp_date_lstc"].ToString();
                LSTCOperator = row["RNGgrp_lstc_operator"].ToString();
                DateAdd = row["RNGgrp_date_add"].ToString();
                AddOperator = row["RNGgrp_add_operator"].ToString();
                OFdate = row["RNGGRP_FDATE"].ToString();
                OTdate = row["RNGGRP_TDATE"].ToString();
                InActiveFlag = "false";
                IndSwitch = row["RNGGRP_IND_SWITCH"].ToString();
                DomainHie = row["RNGGRP_HIE"].ToString();
                Prog_Switch = row["RNGGRP_PROG_SWITCH"].ToString();
                Active = row["RNGGRP_ACTIVE"].ToString();
                PPR_SW = row["RNGGRP_PPR_SW"].ToString(); 
            }
        }

        public RCsb14GroupEntity(DataRow CSB14GRP, string strCode, string strTable)
        {
            if (CSB14GRP != null)
            {
                DataRow row = CSB14GRP;

                //Agency=
                Code = strCode.ToString();
                GrpCode = row["Res_Group"].ToString().Trim();
                TblCode = row["Res_Table"].ToString().Trim();
                GrpDesc = row["Res_Table_Desc"].ToString();
                Hrd1 = row["Res_Hed1_Cnt"].ToString();
                Hrd2 = row["Res_Hed2_Cnt"].ToString();
                Hrd3 = row["Res_Hed3_Cnt"].ToString();
                Hrd4 = row["Res_Hed4_Cnt"].ToString();
                Hrd5 = row["Res_Hed5_Cnt"].ToString();
                unit_cnt = row["Res_Unit_Cnt"].ToString();
                ExAchev = row["Res_Exp_To_Achive"].ToString();
                per_Achived = row["Res_Per_Achived"].ToString();
                CalCost = row["Res_Cost"].ToString();
                Count_type = row["Res_Count_Type"].ToString();
                Row_Type = row["Res_Row_Type"].ToString();
            }
        }

        public RCsb14GroupEntity(string strCode, string strGrpCode, string strTblCode, string strGrpDesc, string strhrd1, string strhrd2, string strhrd3, string strhrd4, string strhrd5, string strunit, string strAchived, string strperachieved, string strcallcost, string strcount, string strrowtype)
        {



            //Agency=
            Code = strCode.ToString().Trim();
            GrpCode = strGrpCode.ToString().Trim();
            TblCode = strTblCode.ToString().Trim();
            GrpDesc = strGrpDesc.ToString().Trim(); ;
            Hrd1 = strhrd1;
            Hrd2 = strhrd2;
            Hrd3 = strhrd3;
            Hrd4 = strhrd4;
            Hrd5 = strhrd5;
            unit_cnt = strunit;
            ExAchev = strAchived;
            per_Achived = strperachieved;
            CalCost = strcallcost;
            Count_type = strcount;
            Row_Type = strrowtype;

        }


        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GrpCode { get; set; }
        public string TblCode { get; set; }
        public string GrpDesc { get; set; }
        public string Hrd1 { get; set; }
        public string Incld1 { get; set; }
        public string Hrd2 { get; set; }
        public string Incld2 { get; set; }
        public string Hrd3 { get; set; }
        public string Incld3 { get; set; }
        public string Hrd4 { get; set; }
        public string Incld4 { get; set; }
        public string Hrd5 { get; set; }
        public string Incld5 { get; set; }
        public string CntIndic { get; set; }
        public string ExAchev { get; set; }
        public string CalCost { get; set; }
        public string UseSer { get; set; }
        public string Duplicate { get; set; }
        public string AFrom { get; set; }
        public string Ato { get; set; }
        public string Disable { get; set; }
        public string GoalCds { get; set; }
        public string unit_cnt { get; set; }
        public string per_Achived { get; set; }
        public string Count_type { get; set; }
        public string Row_Type { get; set; }


        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Mode { get; set; }
        public string InActiveFlag { get; set; }

        public string OFdate { get; set; }
        public string OTdate { get; set; }
        public string CopyAgency { get; set; }

        public string IndSwitch { get; set; }

        public string DomainHie { get; set; }

        public string Prog_Switch { get; set; }

        public string Active { get; set; }

        public string PPR_SW { get; set; }


        #endregion

    }

    public class SRCsb14GroupEntity
    {
        #region Constructors

        public SRCsb14GroupEntity()
        {
            Agency = string.Empty;
            Code = string.Empty;
            Name = string.Empty;
            GrpCode = string.Empty;
            TblCode = string.Empty;
            GrpDesc = string.Empty;
            Hrd1 = string.Empty;
            Incld1 = string.Empty;
            Hrd2 = string.Empty;
            Incld2 = string.Empty;
            Hrd3 = string.Empty;
            Incld3 = string.Empty;
            Hrd4 = string.Empty;
            Incld4 = string.Empty;
            Hrd5 = string.Empty;
            Incld5 = string.Empty;
            CntIndic = string.Empty;
            ExAchev = string.Empty;
            CalCost = string.Empty;
            UseSer = string.Empty;
            Duplicate = string.Empty;
            AFrom = string.Empty;
            Ato = string.Empty;
            Disable = string.Empty;
            GoalCds = string.Empty;

            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            OFdate = string.Empty;
            OTdate = string.Empty;
            unit_cnt = string.Empty;
            per_Achived = string.Empty;
            Count_type = string.Empty;
            Row_Type = string.Empty;

            DomainHie = string.Empty;
            Prog_Switch = string.Empty;
            Active = string.Empty;
            PPR_SW = string.Empty;

            CSBG_Domain = string.Empty;

            BenRsnDomainField = string.Empty;
        }

        public SRCsb14GroupEntity(bool Intialize)
        {
            if (Intialize)
            {
                Agency = null;
                Code = null;
                Name = null;
                GrpCode = null;
                TblCode = null;
                GrpDesc = null;
                Hrd1 = null;
                Incld1 = null;
                Hrd2 = null;
                Incld2 = null;
                Hrd3 = null;
                Incld3 = null;
                Hrd4 = null;
                Incld4 = null;
                Hrd5 = null;
                Incld5 = null;
                CntIndic = null;
                ExAchev = null;
                CalCost = null;
                UseSer = null;
                Duplicate = null;
                AFrom = null;
                Ato = null;
                Disable = null;
                GoalCds = null;

                DateLSTC = null;
                LSTCOperator = null;
                DateAdd = null;
                AddOperator = null;
                OFdate = null;
                OFdate = null;
                unit_cnt = null;
                per_Achived = null;
                Count_type = null;
                Row_Type = null;

                DomainHie = null;
                Prog_Switch = null;
                Active = null;
                PPR_SW= null;

                CSBG_Domain = null;
                BenRsnDomainField = null;
            }
        }

        public SRCsb14GroupEntity(SRCsb14GroupEntity Entity)
        {
            Agency = Entity.Agency;
            Code = Entity.OFdate;
            Name = Entity.Name;
            GrpCode = Entity.GrpCode;
            TblCode = Entity.GrpDesc;
            GrpDesc = Entity.GrpDesc;
            Hrd1 = Entity.Hrd1;
            Incld1 = Entity.Incld1;
            Hrd2 = Entity.Hrd2;
            Incld2 = Entity.Incld2;
            Hrd3 = Entity.Hrd3;
            Incld3 = Entity.Incld3;
            Hrd4 = Entity.Hrd4;
            Incld4 = Entity.Incld4;
            Hrd5 = Entity.Hrd5;
            Incld5 = Entity.Incld5;
            CntIndic = Entity.CntIndic;
            ExAchev = Entity.ExAchev;
            CalCost = Entity.CalCost;
            UseSer = Entity.UseSer;
            Duplicate = Entity.Duplicate;
            AFrom = Entity.AFrom;
            Ato = Entity.Ato;
            Disable = Entity.Disable;
            GoalCds = Entity.GoalCds;

            DateLSTC = Entity.DateLSTC;
            LSTCOperator = Entity.LSTCOperator;
            DateAdd = Entity.DateAdd;
            AddOperator = Entity.AddOperator;
            OFdate = Entity.OFdate;
            OTdate = Entity.OTdate;

            DomainHie = Entity.DomainHie;
            Prog_Switch = Entity.Prog_Switch;
            Active = Entity.Active;
            PPR_SW = Entity.PPR_SW;

            CSBG_Domain = Entity.CSBG_Domain;
            BenRsnDomainField = Entity.BenRsnDomainField;
        }

        public SRCsb14GroupEntity(DataRow CSB14GRP)
        {
            if (CSB14GRP != null)
            {
                DataRow row = CSB14GRP;

                Agency = row["RNGSRGRP_AGENCY"].ToString();
                Code = row["RNGSRGRP_CODE"].ToString();
                //Name = row["RNGGRP_NAME"].ToString();
                GrpCode = row["RNGSRgrp_group_code"].ToString().Trim();
                TblCode = row["RNGSRgrp_table_code"].ToString().Trim();
                GrpDesc = row["RNGSRgrp_desc"].ToString();
                Hrd1 = row["RNGSRgrp_cnt_hdr1"].ToString();
                Incld1 = row["RNGSRgrp_cnt_incld1"].ToString();
                Hrd2 = row["RNGSRgrp_cnt_hdr2"].ToString();
                Incld2 = row["RNGSRgrp_cnt_incld2"].ToString();
                Hrd3 = row["RNGSRgrp_cnt_hdr3"].ToString();
                Incld3 = row["RNGSRgrp_cnt_incld3"].ToString();
                Hrd4 = row["RNGSRgrp_cnt_hdr4"].ToString();
                Incld4 = row["RNGSRgrp_cnt_incld4"].ToString();
                Hrd5 = row["RNGSRgrp_cnt_hdr5"].ToString();
                Incld5 = row["RNGSRgrp_cnt_incld5"].ToString();
                CntIndic = row["RNGSRgrp_cnt_indicator"].ToString();
                ExAchev = row["RNGSRgrp_expect_achieve"].ToString();
                CalCost = row["RNGSRgrp_calc_cost"].ToString();
                UseSer = row["RNGSRgrp_use_servs"].ToString();
                Duplicate = row["RNGSRgrp_duplicated"].ToString();
                AFrom = row["RNGSRgrp_age_from"].ToString();
                Ato = row["RNGSRgrp_age_to"].ToString();
                Disable = row["RNGSRgrp_disabled"].ToString();
                GoalCds = row["RNGSRgrp_goal_codes"].ToString();

                DateLSTC = row["RNGSRgrp_date_lstc"].ToString();
                LSTCOperator = row["RNGSRgrp_lstc_operator"].ToString();
                DateAdd = row["RNGSRgrp_date_add"].ToString();
                AddOperator = row["RNGSRgrp_add_operator"].ToString();
                OFdate = row["RNGSRGRP_FDATE"].ToString();
                OTdate = row["RNGSRGRP_TDATE"].ToString();
                InActiveFlag = "false";

                DomainHie = row["RNGSRGRP_HIE"].ToString();
                Prog_Switch = row["RNGSRGRP_PROG_SWITCH"].ToString();
                Active = row["RNGSRGRP_ACTIVE"].ToString();
                PPR_SW = row["RNGSRGRP_PPR_SW"].ToString();

                CSBG_Domain = row["RNGSRGRP_CSBG"].ToString();
                BenRsnDomainField = row["RNGSRGRP_BENFRESN"].ToString();
            }
        }
        public SRCsb14GroupEntity(DataRow SRCSB14GRP, string strCode, string strTable)
        {
            if (SRCSB14GRP != null)
            {
                DataRow row = SRCSB14GRP;

                Code = strCode.ToString();
                GrpCode = row["Res_Group"].ToString().Trim();
                TblCode = row["Res_Table"].ToString().Trim();
                GrpDesc = row["Res_Table_Desc"].ToString();
                //Hrd1 = row["Res_Hed1_Cnt"].ToString();
                //Hrd2 = row["Res_Hed2_Cnt"].ToString();
                //Hrd3 = row["Res_Hed3_Cnt"].ToString();
                //Hrd4 = row["Res_Hed4_Cnt"].ToString();
                //Hrd5 = row["Res_Hed5_Cnt"].ToString();
                unit_cnt = row["Res_Unit_Cnt"].ToString();
                ExAchev = row["Res_Exp_To_Achive"].ToString();
                per_Achived = row["Res_Per_Achived"].ToString();
                CalCost = row["Res_Cost"].ToString();

                OutcomeCode= row["Res_Outcome_code"].ToString();

                Count_type = row["Res_Count_Type"].ToString();
                Row_Type = row["Res_Row_Type"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GrpCode { get; set; }
        public string TblCode { get; set; }
        public string GrpDesc { get; set; }
        public string Hrd1 { get; set; }
        public string Incld1 { get; set; }
        public string Hrd2 { get; set; }
        public string Incld2 { get; set; }
        public string Hrd3 { get; set; }
        public string Incld3 { get; set; }
        public string Hrd4 { get; set; }
        public string Incld4 { get; set; }
        public string Hrd5 { get; set; }
        public string Incld5 { get; set; }
        public string CntIndic { get; set; }
        public string ExAchev { get; set; }
        public string CalCost { get; set; }
        public string UseSer { get; set; }
        public string Duplicate { get; set; }
        public string AFrom { get; set; }
        public string Ato { get; set; }
        public string Disable { get; set; }
        public string GoalCds { get; set; }

        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Mode { get; set; }
        public string InActiveFlag { get; set; }

        public string OFdate { get; set; }
        public string OTdate { get; set; }

        public string unit_cnt { get; set; }
        public string per_Achived { get; set; }
        public string Count_type { get; set; }
        public string Row_Type { get; set; }
        public string CopyAgency { get; set; }

        public string DomainHie { get; set; }

        public string Prog_Switch { get; set; }

        public string Active { get; set; }

        public string OutcomeCode { get; set; }

        public string PPR_SW { get; set; }

        public string CSBG_Domain
        {
            get; set;
        }

        public string BenRsnDomainField
        {
            get; set;
        }

        #endregion

    }

    public class SPCommonEntity
    {
        #region Constructors

        public SPCommonEntity()
        {

            Code = string.Empty;
            Desc = string.Empty;
            Active = string.Empty;
            Default = string.Empty;
            Hierarchy = string.Empty;
            Sel_WS = false;
            ListHierarchy = new List<string>();
        }

        public SPCommonEntity(string code, string desc)
        {
            Code = code.Trim();
            Desc = desc.Trim();
        }

        public SPCommonEntity(DataRow row)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count;)
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
            Sel_WS = false;
        }

        public SPCommonEntity(DataRow row, string ext)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            Ext = ext;
            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count;)
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
            Sel_WS = false;
        }

        public SPCommonEntity(DataRow row, string ext, string AgyType)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            Ext = row["EXT"].ToString().Trim();
            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count;)
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
            Sel_WS = false;
        }

        public SPCommonEntity(DataRow row, string ext,string AgyType,string RNG)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            //Default = row["AGY_DEFAULT"].ToString().Trim();
            //Hierarchy = row["Hierarchy"].ToString().Trim();
            //Ext = ext;
            //ListHierarchy = new List<string>();
            //if (!Hierarchy.Equals(string.Empty))
            //{
            //    int nextIndex = 0;
            //    for (int count = 0; Hierarchy.Length > count;)
            //    {
            //        ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
            //        count = count + 6;
            //        nextIndex = count;
            //    }
            //}
            Sel_WS = false;
        }

        public SPCommonEntity(SPCommonEntity Ent)
        {
            Desc = Ent.Desc;
            Code = Ent.Code;
            Active = Ent.Active;
            Default = Ent.Default;
            Hierarchy = Ent.Hierarchy;
            Ext = Ent.Ext;
            ListHierarchy = Ent.ListHierarchy;
            Sel_WS = false;
        }

        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string Default { get; set; }
        public string Hierarchy { get; set; }
        public string Ext { get; set; }
        public List<string> ListHierarchy { get; set; }
        public bool Sel_WS { get; set; }

        #endregion

    }

    public class Agy_Ext_Entity
    {
        #region Constructors

        public Agy_Ext_Entity()
        {

            Code = string.Empty;
            Desc = string.Empty;
            Active = string.Empty;
            Default = string.Empty;
            Hierarchy = string.Empty;
            Ext_1 = string.Empty;
            Ext_2 = string.Empty;
            Sel_WS = false;
            ListHierarchy = new List<string>();
        }

        public Agy_Ext_Entity(string code, string desc)
        {
            Code = code.Trim();
            Desc = desc.Trim();
        }

        public Agy_Ext_Entity(DataRow row)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            Ext_1 = row["Ext_1"].ToString().Trim();
            Ext_2 = row["Ext_2"].ToString().Trim();

            Ext1_A = row["Ext1_A"].ToString().Trim();
            Ext2_A = row["Ext2_A"].ToString().Trim();

            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count;)
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
            Sel_WS = false;
        }

        public Agy_Ext_Entity(DataRow row, string ext)
        {
            Desc = row["LookUpDesc"].ToString().Trim();
            Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();
            Ext = ext;
            ListHierarchy = new List<string>();
            if (!Hierarchy.Equals(string.Empty))
            {
                int nextIndex = 0;
                for (int count = 0; Hierarchy.Length > count;)
                {
                    ListHierarchy.Add(Hierarchy.Substring(nextIndex, 6));
                    count = count + 6;
                    nextIndex = count;
                }
            }
            Sel_WS = false;
        }

        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Active { get; set; }
        public string Default { get; set; }
        public string Hierarchy { get; set; }
        public string Ext { get; set; }
        public string Ext_1 { get; set; }
        public string Ext_2 { get; set; }
        public string Ext1_A { get; set; }
        public string Ext2_A { get; set; }
        public List<string> ListHierarchy { get; set; }
        public bool Sel_WS { get; set; }

        #endregion

    }




    public class CSB4AsocEntity
    {
        #region Constructors

        public CSB4AsocEntity()
        {
            CatCode = string.Empty;
            DemoCd = string.Empty;
            AgeFrm = string.Empty;
            AgeTo = string.Empty;
            AgytabCds = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;
            Dateadd = string.Empty;
            addoperator = string.Empty;


        }

        public CSB4AsocEntity(DataRow CSB4ASOC)
        {
            if (CSB4ASOC != null)
            {
                DataRow row = CSB4ASOC;
                CatCode = row["CSB4ASOC_CATG_CODE"].ToString();
                DemoCd = row["CSB4ASOC_DEMO_CODE"].ToString();
                AgeFrm = row["CSB4ASOC_AGE_FROM"].ToString();
                AgeTo = row["CSB4ASOC_AGE_TO"].ToString();
                AgytabCds = row["CSB4ASOC_AGYTAB_CODES"].ToString();
                DateLstc = row["CSB4ASOC_DATE_LSTC"].ToString();
                lstcOperator = row["CSB4ASOC_LSTC_OPERATOR"].ToString();
                Dateadd = row["CSB4ASOC_DATE_ADD"].ToString();
                addoperator = row["CSB4ASOC_ADD_OPERATOR"].ToString();


                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string CatCode { get; set; }
        public string DemoCd { get; set; }
        public string AgeFrm { get; set; }
        public string AgeTo { get; set; }
        public string AgytabCds { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Mode { get; set; }



        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class RNG4AsocEntity
    {
        #region Constructors

        public RNG4AsocEntity()
        {
            RNGCatCode = string.Empty;
            RNGDemoCd = string.Empty;
            RNGAgeFrm = string.Empty;
            RNGAgeTo = string.Empty;
            RNGAgytabCds = string.Empty;
            RNGDateLstc = string.Empty;
            RNGlstcOperator = string.Empty;
            RNGDateadd = string.Empty;
            RNGaddoperator = string.Empty;


        }

        public RNG4AsocEntity(DataRow CSB4ASOC)
        {
            if (CSB4ASOC != null)
            {
                DataRow row = CSB4ASOC;
                RNGCatCode = row["RNG4ASOC_CATG_CODE"].ToString();
                RNGDemoCd = row["RNG4ASOC_DEMO_CODE"].ToString();
                RNGAgeFrm = row["RNG4ASOC_AGE_FROM"].ToString();
                RNGAgeTo = row["RNG4ASOC_AGE_TO"].ToString();
                RNGAgytabCds = row["RNG4ASOC_AGYTAB_CODES"].ToString();
                RNGDateLstc = row["RNG4ASOC_DATE_LSTC"].ToString();
                RNGlstcOperator = row["RNG4ASOC_LSTC_OPERATOR"].ToString();
                RNGDateadd = row["RNG4ASOC_DATE_ADD"].ToString();
                RNGaddoperator = row["RNG4ASOC_ADD_OPERATOR"].ToString();


                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string RNGCatCode { get; set; }
        public string RNGDemoCd { get; set; }
        public string RNGAgeFrm { get; set; }
        public string RNGAgeTo { get; set; }
        public string RNGAgytabCds { get; set; }
        public string RNGDateLstc { get; set; }
        public string RNGlstcOperator { get; set; }
        public string RNGDateadd { get; set; }
        public string RNGaddoperator { get; set; }
        public string RNGMode { get; set; }



        #endregion
    }

    public class PopUp_Menu_L1_Entity
    {
        #region Constructors

        public PopUp_Menu_L1_Entity()
        {
            Cat_Code = string.Empty;
            Cat_Desc = string.Empty;
            Can_Add = true;
        }

        public PopUp_Menu_L1_Entity(string category, string cat_desc)
        {
            if (category != null && cat_desc != null)
            {
                Cat_Code = category;
                Cat_Desc = cat_desc;
                Can_Add = true;
            }
        }

        #endregion

        #region Properties

        public string Cat_Code { get; set; }
        public string Cat_Desc { get; set; }
        public bool Can_Add { get; set; }

        #endregion

    }


    public class PopUp_Menu_L2_Entity
    {
        #region Constructors

        public PopUp_Menu_L2_Entity()
        {
            Cat_Code = string.Empty;
            Grp_Code = 0;
            Grp_Desc = string.Empty;
            Grp_Add = true;
        }

        public PopUp_Menu_L2_Entity(string category, int grp_code)
        {
            if (grp_code != null)
            {
                Cat_Code = category;
                Grp_Code = grp_code;
                Grp_Desc = "Group-" + grp_code;
                Grp_Add = true;
            }
        }

        #endregion

        #region Properties

        public string Cat_Code { get; set; }
        public int Grp_Code { get; set; }
        public string Grp_Desc { get; set; }
        public bool Grp_Add { get; set; }

        #endregion
    }

    public class PopUp_Menu_L3_Entity
    {
        #region Constructors

        public PopUp_Menu_L3_Entity()
        {
            Cat_Code = string.Empty;
            Branch = string.Empty;
            Orig_Grp = int.MinValue;
            Type = string.Empty;
            CAMS_Code = string.Empty;
            CAMS_Seq = int.MinValue;
            Curr_Grp = int.MinValue;
            CAMS_Desc = string.Empty;
            Can_Add = true;
            Belongs_To = string.Empty;
        }

        public PopUp_Menu_L3_Entity(string category, CASESP2Entity Entity, int Bel_Grp)
        {
            if (Entity != null)
            {
                Cat_Code = category;
                Branch = Entity.Branch;
                Orig_Grp = Entity.Orig_Grp;
                Type = Entity.Type1;
                CAMS_Code = Entity.CamCd;
                CAMS_Seq = Entity.Row;
                Curr_Grp = Entity.Curr_Grp;
                CAMS_Desc = Entity.CAMS_Desc;
                Can_Add = true;
                Belongs_To = "Group-" + Bel_Grp.ToString();
            }
        }

        public PopUp_Menu_L3_Entity(string category, CASESPM2Entity Entity, int Bel_Grp)
        {
            if (Entity != null)
            {
                Cat_Code = category;
                Branch = Entity.Branch;
                Orig_Grp = int.Parse(Entity.Group);
                Type = Entity.Type1;
                CAMS_Code = Entity.CamCd;
                CAMS_Seq = int.Parse(Entity.SelOrdinal);
                Curr_Grp = int.Parse(Entity.Curr_Group);
                CAMS_Desc = Entity.CAMS_Desc;
                Can_Add = true;
                Belongs_To = "Group-" + Bel_Grp.ToString();
            }
        }


        #endregion

        #region Properties

        public string Cat_Code { get; set; }
        public string Branch { get; set; }
        public int Orig_Grp { get; set; }
        public string Type { get; set; }
        public string CAMS_Code { get; set; }
        public int CAMS_Seq { get; set; }
        public int Curr_Grp { get; set; }
        public string CAMS_Desc { get; set; }
        public bool Can_Add { get; set; }
        public string Belongs_To { get; set; }



        #endregion
    }

    public class Csb14RAEntity
    {
        #region Constructors

        public Csb14RAEntity()
        {
            RefFDate = string.Empty;
            RefTDate = string.Empty;
            GrpCode = string.Empty;
            CntCode = string.Empty;
            ResCode = string.Empty;
            Desc = string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;

        }

        public Csb14RAEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                RefFDate = row["CSB14RA_REF_FDATE"].ToString();
                RefTDate = row["CSB14RA_REF_TDATE"].ToString();
                GrpCode = row["CSB14RA_GROUP_CODE"].ToString();
                CntCode = row["CSB14RA_COUNT_CODE"].ToString();
                ResCode = row["CSB14RA_RESULT_CODE"].ToString();
                Desc = row["CSB14RA_DESC"].ToString();
                DateLstc = row["CSB14RA_DATE_LSTC"].ToString();
                LSTCOperator = row["CSB14RA_LSTC_OPERATOR"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string RefFDate { get; set; }
        public string RefTDate { get; set; }
        public string GrpCode { get; set; }
        public string CntCode { get; set; }
        public string ResCode { get; set; }
        public string Desc { get; set; }
        public string DateLstc { get; set; }
        public string LSTCOperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }


        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class RNGRAEntity
    {
        #region Constructors

        public RNGRAEntity()
        {
            Agency = string.Empty;
            Code = string.Empty;
            //RefTDate = string.Empty;
            GrpCode = string.Empty;
            CntCode = string.Empty;
            ResCode = string.Empty;
            Desc = string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;

        }

        public RNGRAEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                Agency = row["RNGRA_AGENCY"].ToString();
                Code = row["RNGRA_CODE"].ToString();
                //RefTDate = row["RNGRA_REF_TDATE"].ToString();
                GrpCode = row["RNGRA_GROUP_CODE"].ToString();
                CntCode = row["RNGRA_COUNT_CODE"].ToString();
                ResCode = row["RNGRA_RESULT_CODE"].ToString();
                Desc = row["RNGRA_DESC"].ToString();
                DateLstc = row["RNGRA_DATE_LSTC"].ToString();
                LSTCOperator = row["RNGRA_LSTC_OPERATOR"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        //public string RefTDate { get; set; }
        public string GrpCode { get; set; }
        public string CntCode { get; set; }
        public string ResCode { get; set; }
        public string Desc { get; set; }
        public string DateLstc { get; set; }
        public string LSTCOperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }


        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class RNGGAEntity
    {
        #region Constructors

        public RNGGAEntity()
        {
            Agency = string.Empty;
            Code = string.Empty;
            //RefTDate = string.Empty;
            GrpCode = string.Empty;
            CntCode = string.Empty;
            GoalCode = string.Empty;
            Desc = string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Budget = string.Empty;
            Sequence = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Cnt = string.Empty;
            SerSP = string.Empty;
            SP_desc = string.Empty;

        }
        public RNGGAEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc)
        {
            Agency = string.Empty;
            Code = strCode;
            //RefTDate = string.Empty;
            GrpCode = strGrpCode;
            CntCode = strCntCode;
            GoalCode = strGoalCode;
            Desc = strDesc;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;

            Sequence = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Cnt = string.Empty;

        }

        public RNGGAEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc, string SP_Code, string sp__Desc)
        {
            Agency = string.Empty;
            Code = strCode;
            //RefTDate = string.Empty;
            GrpCode = strGrpCode;
            CntCode = strCntCode;
            GoalCode = strGoalCode;
            Desc = strDesc;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;

            Sequence = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Cnt = string.Empty;
            SerSP = SP_Code;
            SP_desc = sp__Desc;

        }

        public RNGGAEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                Agency = row["RNGGA_AGENCY"].ToString();
                Code = row["RNGGA_CODE"].ToString();
                TblCode = row["RNGGA_TABLE_CODE"].ToString();
                GrpCode = row["RNGGA_GROUP_CODE"].ToString();
                CntCode = row["RNGGA_COUNT_CODE"].ToString();
                GoalCode = row["RNGGA_GOAL_CODE"].ToString();
                Desc = row["RNGGA_DESC"].ToString();
                Budget = row["RNGGA_BUDGET"].ToString();
                DateLstc = row["RNGGA_DATE_LSTC"].ToString();
                LSTCOperator = row["RNGGA_LSTC_OPERATOR"].ToString();
                CAMS_Cnt = row["CAMS_COUNT"].ToString();

                Sequence = row["RNGGA_SEQ"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();
                SerSP = row["RNGGA_SP"].ToString();
                SP_desc = row["SP_DESC"].ToString();
            }
        }

        public RNGGAEntity(DataRow CSB14RA, string strTable)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                Agency = row["RNGGA_AGENCY"].ToString();
                Code = row["RNGGA_CODE"].ToString();
                TblCode = row["RNGGA_TABLE_CODE"].ToString();
                GrpCode = row["RNGGA_GROUP_CODE"].ToString();
                CntCode = row["RNGGA_COUNT_CODE"].ToString();
                GoalCode = row["RNGGA_GOAL_CODE"].ToString();
                Desc = row["RNGGA_DESC"].ToString();
                Budget = row["RNGGA_BUDGET"].ToString();
                DateLstc = row["RNGGA_DATE_LSTC"].ToString();
                LSTCOperator = row["RNGGA_LSTC_OPERATOR"].ToString();
                CAMS_Cnt = row["CAMS_COUNT"].ToString();
                Sequence = row["RNGGA_SEQ"].ToString();

                Sel_Switch = true;
                //Spmaddoperator = row["spm_add_operator"].ToString();
                SerSP = row["RNGGA_SP"].ToString();
                SP_desc = row["SP_DESC"].ToString();
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        public string TblCode { get; set; }
        public string GrpCode { get; set; }
        public string CntCode { get; set; }
        public string GoalCode { get; set; }
        public string Desc { get; set; }
        public string Budget { get; set; }
        public string Sequence { get; set; }
        public string DateLstc { get; set; }
        public string LSTCOperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }

        public string CAMS_Cnt { get; set; }
        public string SerSP { get; set; }

        public string SP_desc { get; set; }

        #endregion

        //#region Public / Overrides Methods

        //public override bool Equals(object obj)
        //{
        //    bool returnValue = false;

        //    // Check for null values and compare run-time types.
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        returnValue = false;
        //    }

        //    ZipCodeEntity zipcode = obj as ZipCodeEntity;
        //    if (zipcode != null)
        //    {
        //        returnValue = (zipcode.Zcrzip.Equals(Zcrzip)) && (zipcode.Zcrzip.Equals(Zcrzip));
        //    }
        //    return returnValue;
        //}



        //#endregion
    }

    public class RNGGoalHEntity
    {
        #region Constructors

        public RNGGoalHEntity()
        {
            RNGGAH_AGENCY = string.Empty;
            RNGGAH_CODE = string.Empty;
            RNGGAH_GRP_CODE = string.Empty;
            RNGGAH_TBL_CODE = string.Empty;
            RNGGAH_GOAL_CODE = string.Empty;
            RNGGAH_HIE = string.Empty;
            RNGGAH_TARGET = string.Empty;
            RNGGAH_DATE_LSTC = string.Empty;
            RNGGAH_LSTC_OPERATOR = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;
            Mode = string.Empty;

        }
        public RNGGoalHEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc)
        {
            RNGGAH_AGENCY = string.Empty;
            RNGGAH_CODE = string.Empty;
            RNGGAH_GRP_CODE = string.Empty;
            RNGGAH_TBL_CODE = string.Empty;
            RNGGAH_GOAL_CODE = string.Empty;
            RNGGAH_HIE = string.Empty;
            RNGGAH_TARGET = string.Empty;
            RNGGAH_DATE_LSTC = string.Empty;
            RNGGAH_LSTC_OPERATOR = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;

        }

        public RNGGoalHEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                RNGGAH_AGENCY = row["RNGGAH_AGENCY"].ToString();
                RNGGAH_CODE = row["RNGGAH_CODE"].ToString();
                RNGGAH_GRP_CODE = row["RNGGAH_GRP_CODE"].ToString();
                RNGGAH_TBL_CODE = row["RNGGAH_TBL_CODE"].ToString();
                RNGGAH_GOAL_CODE = row["RNGGAH_GOAL_CODE"].ToString();
                RNGGAH_HIE = row["RNGGAH_HIE"].ToString();
                RNGGAH_TARGET = row["RNGGAH_TARGET"].ToString();
                RNGGAH_DATE_LSTC = row["RNGGAH_DATE_LSTC"].ToString();
                RNGGAH_LSTC_OPERATOR = row["RNGGAH_LSTC_OPERATOR"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public RNGGoalHEntity(DataRow CSB14RA, string strTable)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                RNGGAH_AGENCY = row["RNGGAH_AGENCY"].ToString();
                RNGGAH_CODE = row["RNGGAH_CODE"].ToString();
                RNGGAH_GRP_CODE = row["RNGGAH_GRP_CODE"].ToString();
                RNGGAH_TBL_CODE = row["RNGGAH_TBL_CODE"].ToString();
                RNGGAH_GOAL_CODE = row["RNGGAH_GOAL_CODE"].ToString();
                RNGGAH_HIE = row["RNGGAH_HIE"].ToString();
                RNGGAH_TARGET = row["RNGGAH_TARGET"].ToString();
                RNGGAH_DATE_LSTC = row["RNGGAH_DATE_LSTC"].ToString();
                RNGGAH_LSTC_OPERATOR = row["RNGGAH_LSTC_OPERATOR"].ToString();
                //LSTCOperator = row["RNGGA_LSTC_OPERATOR"].ToString();
                Sel_Switch = true;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string RNGGAH_AGENCY { get; set; }
        public string RNGGAH_CODE { get; set; }
        public string RNGGAH_GRP_CODE { get; set; }
        public string RNGGAH_TBL_CODE { get; set; }
        public string RNGGAH_GOAL_CODE { get; set; }
        public string RNGGAH_HIE { get; set; }
        public string RNGGAH_TARGET { get; set; }
        //public string Budget { get; set; }
        public string RNGGAH_DATE_LSTC { get; set; }
        public string RNGGAH_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }


        #endregion

       
    }

    public class RNGSRGAEntity
    {
        #region Constructors

        public RNGSRGAEntity()
        {
            Agency = string.Empty;
            Code = string.Empty;
            //RefTDate = string.Empty;
            GrpCode = string.Empty;
            CntCode = string.Empty;
            GoalCode = string.Empty;
            Desc = string.Empty;
            Sequence=string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Count = string.Empty;
            SerSP = string.Empty;
            SP_desc = string.Empty;
        }

        public RNGSRGAEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                Agency = row["RNGSRGA_AGENCY"].ToString();
                Code = row["RNGSRGA_CODE"].ToString();
                TblCode = row["RNGSRGA_TABLE_CODE"].ToString();
                GrpCode = row["RNGSRGA_GROUP_CODE"].ToString();
                CntCode = row["RNGSRGA_COUNT_CODE"].ToString();
                GoalCode = row["RNGSRGA_GOAL_CODE"].ToString();
                Desc = row["RNGSRGA_DESC"].ToString();
                Budget = row["RNGSRGA_BUDGET"].ToString();
                DateLstc = row["RNGSRGA_DATE_LSTC"].ToString();
                LSTCOperator = row["RNGSRGA_LSTC_OPERATOR"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();
                Sequence = row["RNGSRGA_SEQ"].ToString();
                CAMS_Count = row["CAMS_COUNT"].ToString();
                SerSP = row["RNGSRGA_SP"].ToString();
                SP_desc= row["SP_DESC"].ToString();
            }
        }
        public RNGSRGAEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc)
        {
            Agency = string.Empty;
            Code = strCode;
            //RefTDate = string.Empty;
            GrpCode = strGrpCode;
            CntCode = strCntCode;
            GoalCode = strGoalCode;
            Desc = strDesc;
            Sequence = string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Count = string.Empty;
            SerSP = string.Empty;
            SP_desc = string.Empty;
        }
        public RNGSRGAEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc,string SP_Code,string SP_Desc,string budget)
        {
            Agency = string.Empty;
            Code = strCode;
            //RefTDate = string.Empty;
            GrpCode = strGrpCode;
            CntCode = strCntCode;
            GoalCode = strGoalCode;
            Desc = strDesc;
            Sequence = string.Empty;
            DateLstc = string.Empty;
            LSTCOperator = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;
            CAMS_Count = string.Empty;
            SerSP = SP_Code;
            SP_desc = SP_Desc;
            Budget = budget;
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        public string TblCode { get; set; }
        public string GrpCode { get; set; }
        public string CntCode { get; set; }
        public string GoalCode { get; set; }
        public string Sequence { get; set; }
        public string Desc { get; set; }
        public string Budget { get; set; }
        public string DateLstc { get; set; }
        public string LSTCOperator { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }

        public string CAMS_Count { get; set; }
        public string SerSP { get; set; }

        public string SP_desc { get; set; }

        #endregion


    }

    public class RNGSRGoalHEntity
    {
        #region Constructors

        public RNGSRGoalHEntity()
        {
            RNGSRGAH_AGENCY = string.Empty;
            RNGSRGAH_CODE = string.Empty;
            RNGSRGAH_GRP_CODE = string.Empty;
            RNGSRGAH_TBL_CODE = string.Empty;
            RNGSRGAH_GOAL_CODE = string.Empty;
            RNGSRGAH_HIE = string.Empty;
            RNGSRGAH_TARGET = string.Empty;
            RNGSRGAH_DATE_LSTC = string.Empty;
            RNGSRGAH_LSTC_OPERATOR = string.Empty;
            Sel_Switch = false;
            Can_Delete = true;
            Mode = string.Empty;

        }
        public RNGSRGoalHEntity(string strCode, string strGrpCode, string strCntCode, string strGoalCode, string strDesc)
        {
            RNGSRGAH_AGENCY = string.Empty;
            RNGSRGAH_CODE = string.Empty;
            RNGSRGAH_GRP_CODE = string.Empty;
            RNGSRGAH_TBL_CODE = string.Empty;
            RNGSRGAH_GOAL_CODE = string.Empty;
            RNGSRGAH_HIE = string.Empty;
            RNGSRGAH_TARGET = string.Empty;
            RNGSRGAH_DATE_LSTC = string.Empty;
            RNGSRGAH_LSTC_OPERATOR = string.Empty;
            Sel_Switch = true;
            Can_Delete = true;
            Mode = string.Empty;

        }

        public RNGSRGoalHEntity(DataRow CSB14RA)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                RNGSRGAH_AGENCY = row["RNGSRGAH_AGENCY"].ToString();
                RNGSRGAH_CODE = row["RNGSRGAH_CODE"].ToString();
                RNGSRGAH_GRP_CODE = row["RNGSRGAH_GRP_CODE"].ToString();
                RNGSRGAH_TBL_CODE = row["RNGSRGAH_TBL_CODE"].ToString();
                RNGSRGAH_GOAL_CODE = row["RNGSRGAH_GOAL_CODE"].ToString();
                RNGSRGAH_HIE = row["RNGSRGAH_HIE"].ToString();
                RNGSRGAH_TARGET = row["RNGSRGAH_TARGET"].ToString();
                RNGSRGAH_DATE_LSTC = row["RNGSRGAH_DATE_LSTC"].ToString();
                RNGSRGAH_LSTC_OPERATOR = row["RNGSRGAH_LSTC_OPERATOR"].ToString();
                Sel_Switch = false;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public RNGSRGoalHEntity(DataRow CSB14RA, string strTable)
        {
            if (CSB14RA != null)
            {
                DataRow row = CSB14RA;
                RNGSRGAH_AGENCY = row["RNGGAH_AGENCY"].ToString();
                RNGSRGAH_CODE = row["RNGGAH_CODE"].ToString();
                RNGSRGAH_GRP_CODE = row["RNGGAH_GRP_CODE"].ToString();
                RNGSRGAH_TBL_CODE = row["RNGGAH_TBL_CODE"].ToString();
                RNGSRGAH_GOAL_CODE = row["RNGGAH_GOAL_CODE"].ToString();
                RNGSRGAH_HIE = row["RNGGAH_HIE"].ToString();
                RNGSRGAH_TARGET = row["RNGGAH_TARGET"].ToString();
                RNGSRGAH_DATE_LSTC = row["RNGGAH_DATE_LSTC"].ToString();
                RNGSRGAH_LSTC_OPERATOR = row["RNGGAH_LSTC_OPERATOR"].ToString();
                //LSTCOperator = row["RNGGA_LSTC_OPERATOR"].ToString();
                Sel_Switch = true;
                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string RNGSRGAH_AGENCY { get; set; }
        public string RNGSRGAH_CODE { get; set; }
        public string RNGSRGAH_GRP_CODE { get; set; }
        public string RNGSRGAH_TBL_CODE { get; set; }
        public string RNGSRGAH_GOAL_CODE { get; set; }
        public string RNGSRGAH_HIE { get; set; }
        public string RNGSRGAH_TARGET { get; set; }
        //public string Budget { get; set; }
        public string RNGSRGAH_DATE_LSTC { get; set; }
        public string RNGSRGAH_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }
        public bool Sel_Switch { get; set; }
        public bool Can_Delete { get; set; }


        #endregion


    }

    public class CASESPMEntity
    {
        #region Constructors

        public CASESPMEntity()
        {
            Rec_Type =
            agency =
            dept =
            program =
            year =
            app_no =
            service_plan =
            Seq =
            caseworker =
            site =
            startdate =
            estdate =
            compdate =
            sel_branches =
            have_addlbr =
            date_lstc =
            lstc_operator =
            date_add =
            add_operator =
            SPM_MassClose =

            SPM_Fund =
            SPM_Amount =
            SPM_Balance =
            SPM_EligStatus =
            SPM_BDC_ID =

            Sp0_Desc =
            Sp0_Validatetd =
            Site_Desc =

            CA_Postings_Cnt =
            MS_Postings_Cnt =
            Def_Program =
            Bulk_Post =
            SPM_Vendor =
            SPM_Account =
            SPM_BillName_Type =
            SPM_Bill_FName =
            SPM_Bill_LName =
            SPM_Gas_Vendor =
            SPM_Gas_Account =
            SPM_Gas_BillName_Type =
            SPM_Gas_Bill_FName =
            SPM_Gas_Bill_LName =

            SPM_Prim_Source=
            SPM_Sec_Source=
            SPM_LUMP_INTRVAL=
            SPM_FIRST_APP=

             Sp0_Active = Spm_Benefit_Reasn = string.Empty;
        }

        public CASESPMEntity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                agency =
                dept =
                program =
                year =
                app_no =
                service_plan =
                Seq =
                caseworker =
                site =
                startdate =
                estdate =
                compdate =
                sel_branches =
                have_addlbr =
                date_lstc =
                lstc_operator =
                date_add =
                add_operator =

                SPM_MassClose =
                SPM_Fund =
                SPM_Amount =
                SPM_Balance =
                SPM_EligStatus =
                SPM_BDC_ID =

                SPM_Vendor =
                SPM_Account =
                SPM_BillName_Type =
                SPM_Bill_FName =
                SPM_Bill_LName =

                SPM_Gas_Vendor =
            SPM_Gas_Account =
            SPM_Gas_BillName_Type =
            SPM_Gas_Bill_FName =
            SPM_Gas_Bill_LName =

            SPM_Prim_Source =
            SPM_Sec_Source =

                Sp0_Desc =
                Sp0_Validatetd =
                Site_Desc =

                CA_Postings_Cnt =
                MS_Postings_Cnt =
                Def_Program =
                Bulk_Post =
                SPM_LUMP_INTRVAL=
                SPM_FIRST_APP=
                  Sp0_Active = Spm_Benefit_Reasn = null;
            }
        }

        public CASESPMEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                agency = row["spm_agency"].ToString();
                dept = row["spm_dept"].ToString();
                program = row["spm_program"].ToString();
                year = row["spm_year"].ToString();
                app_no = row["spm_app_no"].ToString();
                service_plan = row["spm_service_plan"].ToString();
                Seq = row["spm_seq"].ToString();
                caseworker = row["spm_caseworker"].ToString();
                site = row["spm_site"].ToString();
                startdate = row["spm_startdate"].ToString();
                estdate = row["spm_estdate"].ToString();
                compdate = row["spm_compdate"].ToString();
                sel_branches = row["spm_sel_branches"].ToString();
                have_addlbr = row["spm_have_addlbr"].ToString();
                date_lstc = row["spm_date_lstc"].ToString();
                lstc_operator = row["spm_lstc_operator"].ToString();
                date_add = row["spm_date_add"].ToString();
                add_operator = row["spm_add_operator"].ToString();

                SPM_MassClose = row["SPM_MASS_CLOSE"].ToString();

                SPM_Fund = row["SPM_FUND"].ToString();
                SPM_Amount = row["SPM_AMOUNT"].ToString();
                SPM_Balance = row["SPM_BAL_AMT"].ToString();
                SPM_EligStatus = row["SPM_ELIG_STATUS"].ToString();
                SPM_BDC_ID = row["SPM_BDC_ID"].ToString();

                SPM_Vendor = row["SPM_VENDOR"].ToString();
                SPM_Account = row["SPM_ACCOUNT_NO"].ToString();
                SPM_BillName_Type = row["SPM_BILLNAME_TYPE"].ToString();
                SPM_Bill_FName = row["SPM_BILL_FNAME"].ToString();
                SPM_Bill_LName = row["SPM_BILL_LNAME"].ToString();

                SPM_Gas_Vendor = row["SPM_GAS_VENDOR"].ToString();
                SPM_Gas_Account = row["SPM_GAS_ACCOUNT_NO"].ToString();
                SPM_Gas_BillName_Type = row["SPM_GAS_BILLNAME_TYPE"].ToString();
                SPM_Gas_Bill_FName = row["SPM_GAS_BILL_FNAME"].ToString();
                SPM_Gas_Bill_LName = row["SPM_GAS_BILL_LNAME"].ToString();

                SPM_Prim_Source = row["SPM_PRIM_SOURCE"].ToString();
                SPM_Sec_Source = row["SPM_SEC_SOURCE"].ToString();

                Sp0_Desc = row["sp0_description"].ToString();
                Sp0_Validatetd = row["sp0_validated"].ToString();
                Site_Desc = row["SiteDesc"].ToString();


                CA_Postings_Cnt = row["CASEACT_COUNT"].ToString();
                MS_Postings_Cnt = row["CASEMS_COUNT"].ToString();
                Def_Program = row["SPM_DEF_PROGRAM"].ToString();
                Bulk_Post = row["SPM_BULK"].ToString();

                Sp0_Active = row["SP0_ACTIVE"].ToString();
                Spm_Benefit_Reasn = row["SPM_BENEFIT_REASN"].ToString();
                SPM_LUMP_INTRVAL = row["SPM_LUMP_INTRVAL"].ToString();
                SPM_FIRST_APP = row["SPM_FIRST_APP"].ToString();
            }
        }

        public CASESPMEntity(CASESPMEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                agency = Entity.agency;
                dept = Entity.dept;
                program = Entity.program;
                year = Entity.year;
                app_no = Entity.app_no;
                service_plan = Entity.service_plan;
                Seq = Entity.Seq;
                caseworker = Entity.caseworker;
                site = Entity.site;
                startdate = Entity.startdate;
                estdate = Entity.estdate;
                compdate = Entity.compdate;
                sel_branches = Entity.sel_branches;
                have_addlbr = Entity.have_addlbr;
                date_lstc = Entity.date_lstc;
                lstc_operator = Entity.lstc_operator;
                date_add = Entity.date_add;
                add_operator = Entity.add_operator;

                SPM_MassClose = Entity.SPM_MassClose;
                SPM_Fund = Entity.SPM_Fund;
                SPM_Amount = Entity.SPM_Amount;
                SPM_Balance = Entity.SPM_Balance;
                SPM_EligStatus = Entity.SPM_EligStatus;
                SPM_BDC_ID = Entity.SPM_BDC_ID;

                SPM_Vendor = Entity.SPM_Vendor;
                SPM_Account = Entity.SPM_Account;
                SPM_BillName_Type = Entity.SPM_BillName_Type;
                SPM_Bill_FName = Entity.SPM_Bill_FName;
                SPM_Bill_LName = Entity.SPM_Bill_LName;

                SPM_Gas_Vendor = Entity.SPM_Gas_Vendor;
                SPM_Gas_Account = Entity.SPM_Gas_Account;
                SPM_Gas_BillName_Type = Entity.SPM_Gas_BillName_Type;
                SPM_Gas_Bill_FName = Entity.SPM_Gas_Bill_FName;
                SPM_Gas_Bill_LName = Entity.SPM_Gas_Bill_LName;

                Sp0_Desc = Entity.Sp0_Desc;
                Sp0_Validatetd = Entity.Sp0_Validatetd;
                Site_Desc = Entity.Site_Desc;

                SPM_Prim_Source = Entity.SPM_Prim_Source;
                SPM_Sec_Source = Entity.SPM_Sec_Source;

                CA_Postings_Cnt = Entity.CA_Postings_Cnt;
                MS_Postings_Cnt = Entity.CA_Postings_Cnt;
                Def_Program = Entity.Def_Program;
                Bulk_Post = Entity.Bulk_Post;
                Sp0_Active = Entity.Sp0_Active;
                Spm_Benefit_Reasn = Entity.Spm_Benefit_Reasn;
                SPM_LUMP_INTRVAL = Entity.SPM_LUMP_INTRVAL;
                SPM_FIRST_APP=Entity.SPM_FIRST_APP;
            }
        }

        public CASESPMEntity(DataRow CASESPM, string strtype)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                if (strtype == "SEARCHSPM")
                {
                    agency = row["spm_agency"].ToString().Trim();
                    dept = row["spm_dept"].ToString().Trim();
                    program = row["spm_program"].ToString().Trim();
                    year = row["spm_year"].ToString().Trim();
                    app_no = row["spm_app_no"].ToString();
                    service_plan = row["spm_service_plan"].ToString().Trim();
                    Seq = row["spm_seq"].ToString().Trim();
                    startdate = row["spm_startdate"].ToString().Trim();
                    estdate = row["spm_estdate"].ToString().Trim();
                    compdate = row["spm_compdate"].ToString().Trim();
                    FirstName = row["CLIENTNAME"].ToString().Trim();
                    Sp0_Desc = row["SP0_DESCRIPTION"].ToString().Trim();
                    SPM_TrigDate = row["MST_INTAKE_DATE"].ToString().Trim();
                    CA_Postings_Cnt = row["CASEACT_COUNT"].ToString().Trim();
                    MS_Postings_Cnt = row["CASEMS_COUNT"].ToString().Trim();
                    Bulk_Post = row["CASEMS_SWITCH"].ToString().Trim();
                }
                else
                {

                    Rec_Type = "U";
                    agency = row["spm_agency"].ToString().Trim();
                    dept = row["spm_dept"].ToString().Trim();
                    program = row["spm_program"].ToString().Trim();
                    year = row["spm_year"].ToString().Trim();
                    app_no = row["spm_app_no"].ToString();
                    service_plan = row["spm_service_plan"].ToString().Trim();
                    Seq = row["spm_seq"].ToString().Trim();
                    caseworker = row["spm_caseworker"].ToString().Trim();
                    site = row["spm_site"].ToString().Trim();
                    startdate = row["spm_startdate"].ToString().Trim();
                    estdate = row["spm_estdate"].ToString().Trim();
                    compdate = row["spm_compdate"].ToString().Trim();
                    sel_branches = row["spm_sel_branches"].ToString().Trim();
                    have_addlbr = row["spm_have_addlbr"].ToString().Trim();
                    date_lstc = row["spm_date_lstc"].ToString().Trim();
                    lstc_operator = row["spm_lstc_operator"].ToString().Trim();
                    date_add = row["spm_date_add"].ToString().Trim();
                    add_operator = row["spm_add_operator"].ToString().Trim();
                    //SPM_MassClose = row["SPM_MASS_CLOSE"].ToString().Trim();
                    if (strtype == "Trigger")
                    {
                        SPM_TrigCode = row["SPM_TRIG_CODE"].ToString().Trim();
                        SPM_TrigDate = row["SPM_TRIG_DATE"].ToString().Trim();
                        SPM_TrigDateSeq = row["SPM_TRIG_DATE_SEQ"].ToString().Trim();
                    }
                    if (strtype == "SSBG")
                    {
                        FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                        LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                    }
                }
            }
        }


        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string agency { get; set; }
        public string dept { get; set; }
        public string program { get; set; }
        public string year { get; set; }
        public string app_no { get; set; }
        public string service_plan { get; set; }
        public string Seq { get; set; }
        public string caseworker { get; set; }
        public string site { get; set; }
        public string startdate { get; set; }
        public string estdate { get; set; }
        public string compdate { get; set; }
        public string sel_branches { get; set; }
        public string have_addlbr { get; set; }
        public string date_lstc { get; set; }
        public string lstc_operator { get; set; }
        public string date_add { get; set; }
        public string add_operator { get; set; }
        public string Sp0_Desc { get; set; }
        public string Sp0_Validatetd { get; set; }
        public string Site_Desc { get; set; }
        public string CA_Postings_Cnt { get; set; }
        public string MS_Postings_Cnt { get; set; }
        public string Def_Program { get; set; }
        public string Bulk_Post { get; set; }

        public string SPM_TrigCode { get; set; }
        public string SPM_TrigDate { get; set; }
        public string SPM_TrigDateSeq { get; set; }

        public string SPM_MassClose { get; set; }

        public string SPM_Fund { get; set; }
        public string SPM_Amount { get; set; }
        public string SPM_Balance { get; set; }
        public string SPM_EligStatus { get; set; }
        public string SPM_BDC_ID { get; set; }

        public string SPM_Vendor { get; set; }
        public string SPM_Account { get; set; }
        public string SPM_BillName_Type { get; set; }
        public string SPM_Bill_LName { get; set; }
        public string SPM_Bill_FName { get; set; }

        public string SPM_Gas_Vendor { get; set; }
        public string SPM_Gas_Account { get; set; }
        public string SPM_Gas_BillName_Type { get; set; }
        public string SPM_Gas_Bill_LName { get; set; }
        public string SPM_Gas_Bill_FName { get; set; }

        public string SPM_Prim_Source { get; set; }
        public string SPM_Sec_Source { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Sp0_Active { get; set; }

        public string Spm_Benefit_Reasn { get; set; }

        public string SPM_LUMP_INTRVAL { get; set; }
        public string SPM_FIRST_APP { get; set; }

        #endregion
    }

    public class CATemplateEntity
    {
        #region Constructors

        public CATemplateEntity()
        {
            Rec_Type =
            CADesc =
            Benefit_from =
            Template_date =
            Posting_Date =
            Remarks =
            Add_edit =
            SP2_Type =
            CAMS_Code =
            SP2_Operation =
            Branch =
            Group =
            Notes_Sw =
            Notes_Key =
            CA_Seq =
            Dup_desc =
            SP2_Year =
            CAMS_Active_Stat =
            SP2_ID =
            Mem_List =
            Dup_MS_Date =
            AddedXml =
            Exp_Post_Date =
            Can_Post =
            Post_type =
            Vendor =
            Fund =
            UOM =
            Units =
            Rate =
            Amount =
            Program =
            Site =
            CaseWorker =
            string.Empty;
        }

        public CATemplateEntity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
            CADesc =
            Benefit_from =
            Template_date =
            Posting_Date =
            Remarks =
            Add_edit =
            SP2_Type =
            CAMS_Code =
            SP2_Operation =
            Branch =
            Group =
            Notes_Sw =
            Notes_Key =
            CA_Seq =
            Dup_desc =
            SP2_Year =
            CAMS_Active_Stat =
            SP2_ID =
            Mem_List =
            Dup_MS_Date =
            AddedXml =
            Exp_Post_Date =
            Can_Post =
            Post_type =
            Vendor =
            Fund =
            UOM =
            Units =
            Rate =
            Amount =
            Program =
            Site =
            CaseWorker =
                null;
            }
        }

        public CATemplateEntity(CATemplateEntity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            CADesc = Entity.CADesc;
            Benefit_from = Entity.Benefit_from;
            Template_date = Entity.Template_date;
            Posting_Date = Entity.Posting_Date;
            Remarks = Entity.Remarks;
            Add_edit = Entity.Add_edit;
            SP2_Type = Entity.SP2_Type;
            CAMS_Code = Entity.CAMS_Code;
            SP2_Operation = Entity.SP2_Operation;
            Branch = Entity.Branch;
            Group = Entity.Group;
            Notes_Sw = Entity.Notes_Sw;
            Notes_Key = Entity.Notes_Key;
            CA_Seq = Entity.CA_Seq;
            Dup_desc = Entity.Dup_desc;
            SP2_Year = Entity.SP2_Year;
            CAMS_Active_Stat = Entity.CAMS_Active_Stat;
            SP2_ID = Entity.SP2_ID;
            Mem_List = Entity.Mem_List;
            Dup_MS_Date = Entity.Dup_MS_Date;
            AddedXml = Entity.AddedXml;
            Exp_Post_Date = Entity.Exp_Post_Date;
            Can_Post = Entity.Can_Post;
            Post_type = Entity.Post_type;
            Vendor = Entity.Vendor;
            Fund = Entity.Fund;
            UOM = Entity.UOM;
            Units = Entity.Units;
            Rate = Entity.Rate;
            Amount = Entity.Amount;

            Program = Entity.Program;
            Site = Entity.Site;
            CaseWorker = Entity.CaseWorker;

        }

        //public CATemplateEntity(DataRow CASESPM)
        //{
        //    if (CASESPM != null)
        //    {
        //        DataRow row = CASESPM;

        //        Rec_Type = "U";

        //        CADesc = row["SPM_Count"].ToString();
        //        Benefit_from = row["SPM_Count"].ToString();
        //        Template_date = row["SPM_Count"].ToString();
        //        Posting_Date = row["SPM_Count"].ToString();
        //        Remarks = row["SPM_Count"].ToString();
        //        Add_edit = row["SPM_Count"].ToString();
        //        SP2_Type = row["SPM_Count"].ToString();
        //        CAMS_Code = row["SPM_Count"].ToString();
        //        SP2_Operation = row["SPM_Count"].ToString();
        //        Branch = row["SPM_Count"].ToString();
        //        Group = row["SPM_Count"].ToString();
        //        Notes_Sw = row["SPM_Count"].ToString();
        //        Notes_Key = row["SPM_Count"].ToString();
        //        CA_Seq = row["SPM_Count"].ToString();
        //        Dup_desc = row["SPM_Count"].ToString();
        //        SP2_Year = row["SPM_Count"].ToString();
        //        CAMS_Active_Stat = row["SPM_Count"].ToString();
        //        SP2_ID = row["SPM_Count"].ToString();
        //        Mem_List = row["SPM_Count"].ToString();
        //        Dup_MS_Date = row["SPM_Count"].ToString();
        //        AddedXml = row["SPM_Count"].ToString();
        //        Exp_Post_Date = row["SPM_Count"].ToString();
        //        Can_Post = row["SPM_Count"].ToString();
        //        Post_type = row["SPM_Count"].ToString();
        //        Vendor = row["SPM_Count"].ToString();
        //        Fund = row["SPM_Count"].ToString();
        //        UOM = row["SPM_Count"].ToString();
        //        Units = row["SPM_Count"].ToString();
        //        Rate = row["SPM_Count"].ToString();
        //        Amount = row["SPM_Count"].ToString();

        //        //SPM_Count = row["SPM_Count"].ToString();
        //        //agency = row["spm_agency"].ToString();
        //        //dept = row["spm_dept"].ToString();
        //        //program = row["spm_program"].ToString();
        //        //year = row["spm_year"].ToString();
        //        //SPM_app_no = row["SPM_APP_NO"].ToString();
        //        //MST_app_no = row["MST_APP_NO"].ToString();
        //        //Seq = row["spm_seq"].ToString();
        //        //service_plan = row["spm_service_plan"].ToString();
        //        //caseworker = row["spm_caseworker"].ToString();
        //        //site = row["spm_site"].ToString();
        //        //startdate = row["spm_startdate"].ToString();
        //        //estdate = row["spm_estdate"].ToString();
        //        //compdate = row["spm_compdate"].ToString();
        //        //sel_branches = row["spm_sel_branches"].ToString();
        //        //have_addlbr = row["spm_have_addlbr"].ToString();
        //        //date_lstc = row["spm_date_lstc"].ToString();
        //        //lstc_operator = row["spm_lstc_operator"].ToString();
        //        //date_add = row["spm_date_add"].ToString();
        //        //add_operator = row["spm_add_operator"].ToString();

        //        //SNP_First_Name = row["SNP_NAME_IX_FI"].ToString();
        //        //SNP_Last_Name = row["SNP_NAME_IX_LAST"].ToString();
        //        //SNP_Middle_Name = row["SNP_NAME_IX_MI"].ToString();
        //        //Mst_Site = row["MST_SITE"].ToString().Trim();

        //        //Mst_Hno = row["Hno"].ToString();
        //        //MST_Street = row["Street"].ToString();
        //        //MST_City = row["City"].ToString();
        //        //MST_State = row["State1"].ToString();
        //        //MST_Year = row["MST_Year"].ToString();
        //        //MST_Zip = row["Zip"].ToString();
        //        //MST_Case_Type = row["CaseType"].ToString().Trim();
        //        //MST_Intake_Date = row["MST_INTAKE_DATE"].ToString().Trim();
        //        //Post_SW = row["Post_SW"].ToString().Trim();
        //        //Attn_1Day_SW = row["Attn_1Day_Sw"].ToString().Trim();
        //        //CT_Trigger_SW = row["LPB_SW"].ToString().Trim();

        //        //Mst_Active = row["MST_ACTIVE_STATUS"].ToString().Trim();
        //        //Snp_Active = row["SNP_STATUS"].ToString().Trim();
        //        //Enrl_Status = row["ENRL_STATUS"].ToString().Trim();
        //        //Cust_Resp = row["CUSTRESP"].ToString().Trim();
        //        //Disp_Name = "";
        //    }
        //}



        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string CADesc { get; set; }
        public string Benefit_from { get; set; }
        public string Template_date { get; set; }
        public string Posting_Date { get; set; }
        public string Remarks { get; set; }
        public string Add_edit { get; set; }
        public string SP2_Type { get; set; }
        public string CAMS_Code { get; set; }
        public string SP2_Operation { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string Notes_Sw { get; set; }
        public string Notes_Key { get; set; }
        public string CA_Seq { get; set; }
        public string Dup_desc { get; set; }
        public string SP2_Year { get; set; }
        public string CAMS_Active_Stat { get; set; }
        public string SP2_ID { get; set; }
        public string Mem_List { get; set; }
        public string Dup_MS_Date { get; set; }
        public string AddedXml { get; set; }
        public string Exp_Post_Date { get; set; }
        public string Can_Post { get; set; }
        public string Post_type { get; set; }
        public string Vendor { get; set; }
        public string Fund { get; set; }
        public string UOM { get; set; }
        public string Units { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }

        public string Program { get; set; }
        public string Site { get; set; }
        public string CaseWorker { get; set; }


        //public string Mst_Hno { get; set; }
        //public string MST_Street { get; set; }
        //public string MST_City { get; set; }
        //public string MST_State { get; set; }
        //public string MST_Zip { get; set; }
        //public string MST_Case_Type { get; set; }
        //public string MST_Intake_Date { get; set; }
        //public string MST_Year { get; set; }
        //public string Post_SW { get; set; }
        //public string Attn_1Day_SW { get; set; }
        //public string CT_Trigger_SW { get; set; }
        //public string Mst_Active { get; set; }
        //public string Snp_Active { get; set; }
        //public string Disp_Name { get; set; }

        //public string Enrl_Status { get; set; }
        //public string Cust_Resp { get; set; }


        #endregion

    }

    public class TemplateEntity
    {
        public TemplateEntity()
        {
            Rec_Type =
            BTPL_ID =
            BTPL_CODE =
            BTPL_DESC =
            BTPL_SERVICEPLAN =
            BTPL_SPM_SEQ =
            BTPL_BRANCH =
            BTPL_GROUP =
            BTPL_CACODE =
            BTPL_CASEQ =
            BTPL_DATE =
            BTPL_PROG =
            BTPL_SITE =
            BTPL_WORKER =
            BTPL_VENDOR =
            BTPL_FUND =
            BTPL_UOM =
            BTPL_UNITS =
            BTPL_RATE =
            BTPL_AMOUNT =
            BTPL_OBF =
            BTPL_DATE_ADD =
            BTPL_ADD_OPERATOR =
            BTPL_DATE_LSTC =
            BTPL_LSTC_OPERATOR =

            BTPL_SER_FDATE =
            BTPL_SER_TDATE =
            BTPL_SER_SITE =
            BTPL_SER_SORT =
            BTPL_SER_CASETYPE =
            BTPL_SER_TYPE =
            BTPL_SER_ESTATUS =
            BTPL_SER_QUESTION =
            BTPL_SER_RESPONSE =
            BTPL_SPM =

            BTPL_FUND2 =
            BTPL_FUND3 =
            BTPL_UOM2 =
            BTPL_UOM3 =
            BTPL_AMOUNT2 =
            BTPL_AMOUNT3 =
            BTPL_TOTAL =
            BTPL_BILL_PERIOD =
            BTPL_VEND_ACCT =
            BTPL_ARREARS =
            BTPL_ALLOW_POST =
            string.Empty;
        }

        public TemplateEntity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                BTPL_ID =
                BTPL_CODE =
                BTPL_DESC =
                BTPL_SERVICEPLAN =
                BTPL_SPM_SEQ =
                BTPL_BRANCH =
                BTPL_GROUP =
                BTPL_CACODE =
                BTPL_CASEQ =
                BTPL_DATE =
                BTPL_PROG =
                BTPL_SITE =
                BTPL_WORKER =
                BTPL_VENDOR =
                BTPL_FUND =
                BTPL_UOM =
                BTPL_UNITS =
                BTPL_RATE =
                BTPL_AMOUNT =
                BTPL_OBF =
                BTPL_DATE_ADD =
                BTPL_ADD_OPERATOR =
                BTPL_DATE_LSTC =
                BTPL_LSTC_OPERATOR =

                BTPL_SER_FDATE =
                BTPL_SER_TDATE =
                BTPL_SER_SITE =
                BTPL_SER_SORT =
                BTPL_SER_CASETYPE =
                BTPL_SER_TYPE =
                BTPL_SER_ESTATUS =
                BTPL_SER_QUESTION =
                BTPL_SER_RESPONSE =
                BTPL_SPM =
                BTPL_FUND2 =
                BTPL_FUND3 =
                BTPL_UOM2 =
                BTPL_UOM3 =
                BTPL_AMOUNT2 =
                BTPL_AMOUNT3 =
                BTPL_TOTAL =
                BTPL_BILL_PERIOD =
                BTPL_VEND_ACCT =
                BTPL_ARREARS =
                BTPL_ALLOW_POST =
                null;
            }
        }

        public TemplateEntity(TemplateEntity Entity)
        {
            Rec_Type = Entity.Rec_Type;
            BTPL_ID = Entity.BTPL_ID;
            BTPL_CODE = Entity.BTPL_CODE;
            BTPL_DESC = Entity.BTPL_DESC;
            BTPL_SERVICEPLAN = Entity.BTPL_SERVICEPLAN;
            BTPL_SPM_SEQ = Entity.BTPL_SPM_SEQ;
            BTPL_BRANCH = Entity.BTPL_BRANCH;
            BTPL_GROUP = Entity.BTPL_GROUP;
            BTPL_CACODE = Entity.BTPL_CACODE;
            BTPL_CASEQ = Entity.BTPL_CASEQ;
            BTPL_DATE = Entity.BTPL_DATE;
            BTPL_PROG = Entity.BTPL_PROG;
            BTPL_SITE = Entity.BTPL_SITE;
            BTPL_WORKER = Entity.BTPL_WORKER;
            BTPL_VENDOR = Entity.BTPL_VENDOR;
            BTPL_FUND = Entity.BTPL_FUND;
            BTPL_UOM = Entity.BTPL_UOM;
            BTPL_UNITS = Entity.BTPL_UNITS;
            BTPL_RATE = Entity.BTPL_RATE;
            BTPL_AMOUNT = Entity.BTPL_AMOUNT;
            BTPL_OBF = Entity.BTPL_OBF;
            BTPL_DATE_ADD = Entity.BTPL_DATE_ADD;
            BTPL_ADD_OPERATOR = Entity.BTPL_ADD_OPERATOR;
            BTPL_DATE_LSTC = Entity.BTPL_DATE_LSTC;
            BTPL_LSTC_OPERATOR = Entity.BTPL_LSTC_OPERATOR;

            BTPL_SER_FDATE = Entity.BTPL_SER_FDATE;
            BTPL_SER_TDATE = Entity.BTPL_SER_TDATE;
            BTPL_SER_SITE = Entity.BTPL_SER_SITE;
            BTPL_SER_SORT = Entity.BTPL_SER_SORT;
            BTPL_SER_CASETYPE = Entity.BTPL_SER_CASETYPE;
            BTPL_SER_TYPE = Entity.BTPL_SER_TYPE;
            BTPL_SER_ESTATUS = Entity.BTPL_SER_ESTATUS;
            BTPL_SER_QUESTION = Entity.BTPL_SER_QUESTION;
            BTPL_SER_RESPONSE = Entity.BTPL_SER_RESPONSE;
            BTPL_SPM = Entity.BTPL_SPM;
            BTPL_FUND2 = Entity.BTPL_FUND2;
            BTPL_FUND3 = Entity.BTPL_FUND3;
            BTPL_UOM2 = Entity.BTPL_UOM2;
            BTPL_UOM3 = Entity.BTPL_UOM3;
            BTPL_AMOUNT2 = Entity.BTPL_AMOUNT2;
            BTPL_AMOUNT3 = Entity.BTPL_AMOUNT3;
            BTPL_TOTAL = Entity.BTPL_TOTAL;
            BTPL_BILL_PERIOD = Entity.BTPL_BILL_PERIOD;
            BTPL_VEND_ACCT = Entity.BTPL_VEND_ACCT;
            BTPL_ARREARS = Entity.BTPL_ARREARS;
            BTPL_ALLOW_POST = Entity.BTPL_ALLOW_POST;

        }

        //public CATemplateEntity(DataRow CASESPM)
        //{
        //    if (CASESPM != null)
        //    {
        //        DataRow row = CASESPM;

        //        Rec_Type = "U";

        //        CADesc = row["SPM_Count"].ToString();
        //        Benefit_from = row["SPM_Count"].ToString();
        //        Template_date = row["SPM_Count"].ToString();
        //        Posting_Date = row["SPM_Count"].ToString();
        //        Remarks = row["SPM_Count"].ToString();
        //        Add_edit = row["SPM_Count"].ToString();
        //        SP2_Type = row["SPM_Count"].ToString();
        //        CAMS_Code = row["SPM_Count"].ToString();
        //        SP2_Operation = row["SPM_Count"].ToString();
        //        Branch = row["SPM_Count"].ToString();
        //        Group = row["SPM_Count"].ToString();
        //        Notes_Sw = row["SPM_Count"].ToString();
        //        Notes_Key = row["SPM_Count"].ToString();
        //        CA_Seq = row["SPM_Count"].ToString();
        //        Dup_desc = row["SPM_Count"].ToString();
        //        SP2_Year = row["SPM_Count"].ToString();
        //        CAMS_Active_Stat = row["SPM_Count"].ToString();
        //        SP2_ID = row["SPM_Count"].ToString();
        //        Mem_List = row["SPM_Count"].ToString();
        //        Dup_MS_Date = row["SPM_Count"].ToString();
        //        AddedXml = row["SPM_Count"].ToString();
        //        Exp_Post_Date = row["SPM_Count"].ToString();
        //        Can_Post = row["SPM_Count"].ToString();
        //        Post_type = row["SPM_Count"].ToString();
        //        Vendor = row["SPM_Count"].ToString();
        //        Fund = row["SPM_Count"].ToString();
        //        UOM = row["SPM_Count"].ToString();
        //        Units = row["SPM_Count"].ToString();
        //        Rate = row["SPM_Count"].ToString();
        //        Amount = row["SPM_Count"].ToString();

        //        //SPM_Count = row["SPM_Count"].ToString();
        //        //agency = row["spm_agency"].ToString();
        //        //dept = row["spm_dept"].ToString();
        //        //program = row["spm_program"].ToString();
        //        //year = row["spm_year"].ToString();
        //        //SPM_app_no = row["SPM_APP_NO"].ToString();
        //        //MST_app_no = row["MST_APP_NO"].ToString();
        //        //Seq = row["spm_seq"].ToString();
        //        //service_plan = row["spm_service_plan"].ToString();
        //        //caseworker = row["spm_caseworker"].ToString();
        //        //site = row["spm_site"].ToString();
        //        //startdate = row["spm_startdate"].ToString();
        //        //estdate = row["spm_estdate"].ToString();
        //        //compdate = row["spm_compdate"].ToString();
        //        //sel_branches = row["spm_sel_branches"].ToString();
        //        //have_addlbr = row["spm_have_addlbr"].ToString();
        //        //date_lstc = row["spm_date_lstc"].ToString();
        //        //lstc_operator = row["spm_lstc_operator"].ToString();
        //        //date_add = row["spm_date_add"].ToString();
        //        //add_operator = row["spm_add_operator"].ToString();

        //        //SNP_First_Name = row["SNP_NAME_IX_FI"].ToString();
        //        //SNP_Last_Name = row["SNP_NAME_IX_LAST"].ToString();
        //        //SNP_Middle_Name = row["SNP_NAME_IX_MI"].ToString();
        //        //Mst_Site = row["MST_SITE"].ToString().Trim();

        //        //Mst_Hno = row["Hno"].ToString();
        //        //MST_Street = row["Street"].ToString();
        //        //MST_City = row["City"].ToString();
        //        //MST_State = row["State1"].ToString();
        //        //MST_Year = row["MST_Year"].ToString();
        //        //MST_Zip = row["Zip"].ToString();
        //        //MST_Case_Type = row["CaseType"].ToString().Trim();
        //        //MST_Intake_Date = row["MST_INTAKE_DATE"].ToString().Trim();
        //        //Post_SW = row["Post_SW"].ToString().Trim();
        //        //Attn_1Day_SW = row["Attn_1Day_Sw"].ToString().Trim();
        //        //CT_Trigger_SW = row["LPB_SW"].ToString().Trim();

        //        //Mst_Active = row["MST_ACTIVE_STATUS"].ToString().Trim();
        //        //Snp_Active = row["SNP_STATUS"].ToString().Trim();
        //        //Enrl_Status = row["ENRL_STATUS"].ToString().Trim();
        //        //Cust_Resp = row["CUSTRESP"].ToString().Trim();
        //        //Disp_Name = "";
        //    }
        //}

        public TemplateEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                BTPL_ID = row["BTPL_ID"].ToString();
                BTPL_CODE = row["BTPL_CODE"].ToString();
                BTPL_DESC = row["BTPL_DESC"].ToString();
                BTPL_SERVICEPLAN = row["BTPL_SERVICEPLAN"].ToString();
                BTPL_SPM_SEQ = row["BTPL_SPM_SEQ"].ToString();
                BTPL_BRANCH = row["BTPL_BRANCH"].ToString();
                BTPL_GROUP = row["BTPL_GROUP"].ToString();
                BTPL_CACODE = row["BTPL_CACODE"].ToString();
                BTPL_CASEQ = row["BTPL_CASEQ"].ToString();
                BTPL_DATE = row["BTPL_DATE"].ToString();
                BTPL_PROG = row["BTPL_PROG"].ToString();
                BTPL_SITE = row["BTPL_SITE"].ToString();
                BTPL_WORKER = row["BTPL_WORKER"].ToString();
                BTPL_VENDOR = row["BTPL_VENDOR"].ToString();
                BTPL_FUND = row["BTPL_FUND"].ToString();
                BTPL_UOM = row["BTPL_UOM"].ToString();
                BTPL_UNITS = row["BTPL_UNITS"].ToString();
                BTPL_RATE = row["BTPL_RATE"].ToString();
                BTPL_AMOUNT = row["BTPL_AMOUNT"].ToString();

                BTPL_OBF = row["BTPL_OBF"].ToString();

                BTPL_DATE_ADD = row["BTPL_DATE_ADD"].ToString();
                BTPL_ADD_OPERATOR = row["BTPL_ADD_OPERATOR"].ToString();
                BTPL_DATE_LSTC = row["BTPL_DATE_LSTC"].ToString();


                BTPL_LSTC_OPERATOR = row["BTPL_LSTC_OPERATOR"].ToString();


                BTPL_SER_FDATE = row["BTPL_SER_FDATE"].ToString();
                BTPL_SER_TDATE = row["BTPL_SER_TDATE"].ToString();
                BTPL_SER_SITE = row["BTPL_SER_SITE"].ToString();
                BTPL_SER_SORT = row["BTPL_SER_SORT"].ToString();
                BTPL_SER_CASETYPE = row["BTPL_SER_CASETYPE"].ToString();
                BTPL_SER_TYPE = row["BTPL_SER_TYPE"].ToString();
                BTPL_SER_ESTATUS = row["BTPL_SER_ESTATUS"].ToString();
                BTPL_SER_QUESTION = row["BTPL_SER_QUESTION"].ToString();
                BTPL_SER_RESPONSE = row["BTPL_SER_RESPONSE"].ToString();
                BTPL_SPM = row["BTPL_SPM"].ToString();

                BTPL_FUND2 = row["BTPL_FUND2"].ToString();
                BTPL_FUND3 = row["BTPL_FUND3"].ToString();
                BTPL_UOM2 = row["BTPL_UOM2"].ToString();
                BTPL_UOM3 = row["BTPL_UOM3"].ToString();
                BTPL_AMOUNT2 = row["BTPL_AMOUNT2"].ToString();
                BTPL_AMOUNT3 = row["BTPL_AMOUNT3"].ToString();
                BTPL_TOTAL = row["BTPL_TOTAL"].ToString();
                BTPL_BILL_PERIOD = row["BTPL_BILL_PERIOD"].ToString();
                BTPL_VEND_ACCT = row["BTPL_VEND_ACCT"].ToString();
                BTPL_ARREARS = row["BTPL_ARREARS"].ToString();
                BTPL_ALLOW_POST = row["BTPL_ALLOW_POST"].ToString();

                //Def_Program = row["SPM_DEF_PROGRAM"].ToString();
                //Bulk_Post = row["SPM_BULK"].ToString();

                //Sp0_Active = row["SP0_ACTIVE"].ToString();
            }
        }

        #region Properties

        public string Rec_Type { get; set; }
        public string BTPL_ID { get; set; }
        public string BTPL_CODE { get; set; }
        public string BTPL_DESC { get; set; }
        public string BTPL_SERVICEPLAN { get; set; }
        public string BTPL_SPM_SEQ { get; set; }
        public string BTPL_BRANCH { get; set; }
        public string BTPL_GROUP { get; set; }
        public string BTPL_CACODE { get; set; }
        public string BTPL_CASEQ { get; set; }
        public string BTPL_DATE { get; set; }
        public string BTPL_PROG { get; set; }
        public string BTPL_SITE { get; set; }
        public string BTPL_WORKER { get; set; }
        public string BTPL_VENDOR { get; set; }
        public string BTPL_FUND { get; set; }
        public string BTPL_UOM { get; set; }
        public string BTPL_UNITS { get; set; }
        public string BTPL_RATE { get; set; }
        public string BTPL_AMOUNT { get; set; }
        public string BTPL_OBF { get; set; }
        public string BTPL_DATE_ADD { get; set; }
        public string BTPL_ADD_OPERATOR { get; set; }
        public string BTPL_DATE_LSTC { get; set; }
        public string BTPL_LSTC_OPERATOR { get; set; }
        public string BTPL_SER_FDATE { get; set; }
        public string BTPL_SER_TDATE { get; set; }
        public string BTPL_SER_SITE { get; set; }
        public string BTPL_SER_SORT { get; set; }
        public string BTPL_SER_CASETYPE { get; set; }
        public string BTPL_SER_TYPE { get; set; }
        public string BTPL_SER_ESTATUS { get; set; }
        public string BTPL_SER_QUESTION { get; set; }
        public string BTPL_SER_RESPONSE { get; set; }
        public string BTPL_SPM { get; set; }
        public string BTPL_FUND2 { get; set; }
        public string BTPL_FUND3 { get; set; }
        public string BTPL_UOM2 { get; set; }
        public string BTPL_UOM3 { get; set; }
        public string BTPL_AMOUNT2 { get; set; }
        public string BTPL_AMOUNT3 { get; set; }
        public string BTPL_TOTAL { get; set; }
        public string BTPL_BILL_PERIOD { get; set; }
        public string BTPL_VEND_ACCT { get; set; }
        public string BTPL_ARREARS { get; set; }
        public string BTPL_ALLOW_POST { get; set; }
        //public string Vendor { get; set; }
        //public string Fund { get; set; }
        //public string UOM { get; set; }
        //public string Units { get; set; }
        //public string Rate { get; set; }
        //public string Amount { get; set; }

        //public string Program { get; set; }
        //public string Site { get; set; }
        //public string CaseWorker { get; set; }


        #endregion
    }


    public class SP_Bulk_Post_Entity
    {
        #region Constructors

        public SP_Bulk_Post_Entity()
        {
            Rec_Type =
            SPM_Count =
            agency =
            dept =
            program =
            year =
            SPM_app_no =
            MST_app_no =
            Seq =
            service_plan =
            caseworker =
            site =
            startdate =
            estdate =
            compdate =
            sel_branches =
            have_addlbr =
            date_lstc =
            lstc_operator =
            date_add =
            add_operator =
            SNP_First_Name =
            SNP_Last_Name =
            SNP_Middle_Name =
            Mst_Site =
            Mst_Hno =
            MST_Street =
            MST_City =
            MST_State =
            MST_Zip =
            MST_Case_Type =
            MST_Intake_Date =
            MST_Year =
            Post_SW =
            Attn_1Day_SW =
            CT_Trigger_SW =
            Mst_Active =
            Snp_Active =
            Disp_Name =
            Enrl_Status =
            Cust_Resp =
            AppMem =
            FamSeq =
            ClientID =
            Post_Amounts =
            ENRL_DATE =
            string.Empty;
        }

        public SP_Bulk_Post_Entity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                    SPM_Count =
                agency =
                dept =
                program =
                year =
                SPM_app_no =
                MST_app_no =
                Seq =
                service_plan =
                caseworker =
                site =
                startdate =
                estdate =
                compdate =
                sel_branches =
                have_addlbr =
                date_lstc =
                lstc_operator =
                date_add =
                add_operator =

                SNP_First_Name =
                SNP_Last_Name =
                SNP_Middle_Name =
                Mst_Site =
                Mst_Hno =
                MST_Street =
                MST_City =
                MST_State =
                MST_Zip =
                MST_Case_Type =
                MST_Intake_Date =
                MST_Year =
                Post_SW =
                Attn_1Day_SW =
                CT_Trigger_SW =
                Mst_Active =
                Snp_Active =
                Disp_Name =
                Enrl_Status =
                Cust_Resp =
                AppMem =
                FamSeq =
                ClientID =
                Post_Amounts =
                ENRL_DATE =
                null;
            }
        }

        public SP_Bulk_Post_Entity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                SPM_Count = row["SPM_Count"].ToString();
                agency = row["spm_agency"].ToString();
                dept = row["spm_dept"].ToString();
                program = row["spm_program"].ToString();
                year = row["spm_year"].ToString();
                SPM_app_no = row["SPM_APP_NO"].ToString();
                MST_app_no = row["MST_APP_NO"].ToString();
                Seq = row["spm_seq"].ToString();
                service_plan = row["spm_service_plan"].ToString();
                caseworker = row["spm_caseworker"].ToString();
                site = row["spm_site"].ToString();
                startdate = row["spm_startdate"].ToString();
                estdate = row["spm_estdate"].ToString();
                compdate = row["spm_compdate"].ToString();
                sel_branches = row["spm_sel_branches"].ToString();
                have_addlbr = row["spm_have_addlbr"].ToString();
                date_lstc = row["spm_date_lstc"].ToString();
                lstc_operator = row["spm_lstc_operator"].ToString();
                date_add = row["spm_date_add"].ToString();
                add_operator = row["spm_add_operator"].ToString();

                SNP_First_Name = row["SNP_NAME_IX_FI"].ToString();
                SNP_Last_Name = row["SNP_NAME_IX_LAST"].ToString();
                SNP_Middle_Name = row["SNP_NAME_IX_MI"].ToString();
                Mst_Site = row["MST_SITE"].ToString().Trim();

                Mst_Hno = row["Hno"].ToString();
                MST_Street = row["Street"].ToString();
                MST_City = row["City"].ToString();
                MST_State = row["State1"].ToString();
                MST_Year = row["MST_Year"].ToString();
                MST_Zip = row["Zip"].ToString();
                MST_Case_Type = row["CaseType"].ToString().Trim();
                MST_Intake_Date = row["MST_INTAKE_DATE"].ToString().Trim();
                Post_SW = row["Post_SW"].ToString().Trim();
                Attn_1Day_SW = row["Attn_1Day_Sw"].ToString().Trim();
                CT_Trigger_SW = row["LPB_SW"].ToString().Trim();

                Mst_Active = row["MST_ACTIVE_STATUS"].ToString().Trim();
                Snp_Active = row["SNP_STATUS"].ToString().Trim();
                Enrl_Status = row["ENRL_STATUS"].ToString().Trim();
                Cust_Resp = row["CUSTRESP"].ToString().Trim();
                Disp_Name = "";

                AppMem = row["AppNo"].ToString().Trim();
                FamSeq = row["SNP_FAMILY_SEQ"].ToString().Trim();
                ClientID = row["SNP_CLIENT_ID"].ToString().Trim();

                Post_Amounts = row["Post_Amounts"].ToString().Trim();
                ENRL_DATE = row["ENRL_DATE"].ToString().Trim();

            }
        }

        //public SP_Bulk_Post_Entity(SP_Bulk_Post_Entity Entity)
        //{
        //    if (Entity != null)
        //    {
        //        Rec_Type = Entity.Rec_Type;
        //        agency = Entity.agency;
        //        dept = Entity.dept;
        //        program = Entity.program;
        //        year = Entity.year;
        //        app_no = Entity.app_no;
        //        service_plan = Entity.service_plan;
        //        caseworker = Entity.caseworker;
        //        site = Entity.site;
        //        startdate = Entity.startdate;
        //        estdate = Entity.estdate;
        //        compdate = Entity.compdate;
        //        sel_branches = Entity.sel_branches;
        //        have_addlbr = Entity.have_addlbr;
        //        date_lstc = Entity.date_lstc;
        //        lstc_operator = Entity.lstc_operator;
        //        date_add = Entity.date_add;
        //        add_operator = Entity.add_operator;

        //        Sp0_Desc = Entity.Sp0_Desc;
        //        Sp0_Validatetd = Entity.Sp0_Validatetd;
        //        Site_Desc = Entity.Site_Desc;

        //        CA_Postings_Cnt = Entity.CA_Postings_Cnt;
        //        MS_Postings_Cnt = Entity.CA_Postings_Cnt;

        //    }
        //}

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string SPM_Count { get; set; }
        public string agency { get; set; }
        public string dept { get; set; }
        public string program { get; set; }
        public string year { get; set; }
        public string SPM_app_no { get; set; }
        public string MST_app_no { get; set; }
        public string Seq { get; set; }
        public string service_plan { get; set; }
        public string SNP_First_Name { get; set; }
        public string SNP_Last_Name { get; set; }
        public string SNP_Middle_Name { get; set; }
        public string Mst_Site { get; set; }
        public string caseworker { get; set; }
        public string site { get; set; }
        public string startdate { get; set; }
        public string estdate { get; set; }
        public string compdate { get; set; }
        public string sel_branches { get; set; }
        public string have_addlbr { get; set; }
        public string date_lstc { get; set; }
        public string lstc_operator { get; set; }
        public string date_add { get; set; }
        public string add_operator { get; set; }
        public string Sp0_Desc { get; set; }
        public string Sp0_Validatetd { get; set; }
        public string Site_Desc { get; set; }
        public string CA_Postings_Cnt { get; set; }
        public string MS_Postings_Cnt { get; set; }

        public string Mst_Hno { get; set; }
        public string MST_Street { get; set; }
        public string MST_City { get; set; }
        public string MST_State { get; set; }
        public string MST_Zip { get; set; }
        public string MST_Case_Type { get; set; }
        public string MST_Intake_Date { get; set; }
        public string MST_Year { get; set; }
        public string Post_SW { get; set; }
        public string Attn_1Day_SW { get; set; }
        public string CT_Trigger_SW { get; set; }
        public string Mst_Active { get; set; }
        public string Snp_Active { get; set; }
        public string Disp_Name { get; set; }

        public string Enrl_Status { get; set; }
        public string Cust_Resp { get; set; }

        public string AppMem { get; set; }
        public string FamSeq { get; set; }
        public string ClientID { get; set; }

        public string Post_Amounts { get; set; }

        public string ENRL_DATE
        {
            get;set;
        }


        #endregion

    }

    public class Bulk_Post_Entity
    {
        #region Constructors

        public Bulk_Post_Entity()
        {
            Selected =
            MST_app_no =
            year =
            AppName =
                Active =
                MST_Intake_Date =
                App_Address =
                site =
                Start_date =
                Seq =
                PostSw =
                Rowcolor =
                AppMem =
                FamSeq =
                ClientID =
                Post_Amounts =
                ENRL_DATE=
            string.Empty;
        }

        public Bulk_Post_Entity(bool Intialize)
        {
            if (Intialize)
            {
                Selected =
                MST_app_no =
                year =
                AppName =
                Active =
                MST_Intake_Date =
                App_Address =
                site =
                Start_date =
                Seq =
                PostSw =
                Rowcolor =
                AppMem =
                FamSeq =
                ClientID =
                Post_Amounts =
                ENRL_DATE=
                null;
            }
        }

        public Bulk_Post_Entity(Bulk_Post_Entity Entity)
        {
            Selected = Entity.Selected;
            MST_app_no = Entity.MST_app_no;
            year = Entity.year;
            AppName = Entity.AppName;
            Active = Entity.Active;
            MST_Intake_Date = Entity.MST_Intake_Date;
            App_Address = Entity.App_Address;
            site = Entity.site;
            Start_date = Entity.Start_date;
            Seq = Entity.Seq;
            PostSw = Entity.PostSw;
            Rowcolor = Entity.Rowcolor;
            AppMem = Entity.AppMem;
            FamSeq = Entity.FamSeq;
            ClientID = Entity.ClientID;
            Post_Amounts = Entity.Post_Amounts;
            ENRL_DATE = Entity.ENRL_DATE;
        }

        public Bulk_Post_Entity(string selected, string appno, string Year, string appname, string active, string intakedate, string address, string Site, string startdate, string seq, string postsw, string rowcolor, string appmem, string famseq, string clientID, string Amounts,string enrlDate)
        {
            Selected = selected;
            MST_app_no = appno;
            year = Year;
            AppName = appname;
            Active = active;
            MST_Intake_Date = intakedate;
            App_Address = address;
            site = Site;
            Start_date = startdate;
            Seq = seq;
            PostSw = postsw;
            Rowcolor = rowcolor;
            AppMem = appmem;
            FamSeq = famseq;
            ClientID = clientID;
            Post_Amounts = Amounts;
            ENRL_DATE= enrlDate;
        }

        //public Bulk_Post_Entity(DataRow CASESPM)
        //{
        //    if (CASESPM != null)
        //    {
        //        DataRow row = CASESPM;

        //        Rec_Type = "U";
        //        SPM_Count = row["SPM_Count"].ToString();
        //        agency = row["spm_agency"].ToString();
        //        dept = row["spm_dept"].ToString();
        //        program = row["spm_program"].ToString();
        //        year = row["spm_year"].ToString();
        //        SPM_app_no = row["SPM_APP_NO"].ToString();
        //        MST_app_no = row["MST_APP_NO"].ToString();
        //        Seq = row["spm_seq"].ToString();
        //        service_plan = row["spm_service_plan"].ToString();
        //        caseworker = row["spm_caseworker"].ToString();
        //        site = row["spm_site"].ToString();
        //        startdate = row["spm_startdate"].ToString();
        //        estdate = row["spm_estdate"].ToString();
        //        compdate = row["spm_compdate"].ToString();
        //        sel_branches = row["spm_sel_branches"].ToString();
        //        have_addlbr = row["spm_have_addlbr"].ToString();
        //        date_lstc = row["spm_date_lstc"].ToString();
        //        lstc_operator = row["spm_lstc_operator"].ToString();
        //        date_add = row["spm_date_add"].ToString();
        //        add_operator = row["spm_add_operator"].ToString();

        //        SNP_First_Name = row["SNP_NAME_IX_FI"].ToString();
        //        SNP_Last_Name = row["SNP_NAME_IX_LAST"].ToString();
        //        SNP_Middle_Name = row["SNP_NAME_IX_MI"].ToString();
        //        Mst_Site = row["MST_SITE"].ToString().Trim();

        //        Mst_Hno = row["Hno"].ToString();
        //        MST_Street = row["Street"].ToString();
        //        MST_City = row["City"].ToString();
        //        MST_State = row["State1"].ToString();
        //        MST_Year = row["MST_Year"].ToString();
        //        MST_Zip = row["Zip"].ToString();
        //        MST_Case_Type = row["CaseType"].ToString().Trim();
        //        MST_Intake_Date = row["MST_INTAKE_DATE"].ToString().Trim();
        //        Post_SW = row["Post_SW"].ToString().Trim();
        //        Attn_1Day_SW = row["Attn_1Day_Sw"].ToString().Trim();
        //        CT_Trigger_SW = row["LPB_SW"].ToString().Trim();

        //        Mst_Active = row["MST_ACTIVE_STATUS"].ToString().Trim();
        //        Snp_Active = row["SNP_STATUS"].ToString().Trim();
        //        Enrl_Status = row["ENRL_STATUS"].ToString().Trim();
        //        Disp_Name = "";
        //    }
        //}

        //public SP_Bulk_Post_Entity(SP_Bulk_Post_Entity Entity)
        //{
        //    if (Entity != null)
        //    {
        //        Rec_Type = Entity.Rec_Type;
        //        agency = Entity.agency;
        //        dept = Entity.dept;
        //        program = Entity.program;
        //        year = Entity.year;
        //        app_no = Entity.app_no;
        //        service_plan = Entity.service_plan;
        //        caseworker = Entity.caseworker;
        //        site = Entity.site;
        //        startdate = Entity.startdate;
        //        estdate = Entity.estdate;
        //        compdate = Entity.compdate;
        //        sel_branches = Entity.sel_branches;
        //        have_addlbr = Entity.have_addlbr;
        //        date_lstc = Entity.date_lstc;
        //        lstc_operator = Entity.lstc_operator;
        //        date_add = Entity.date_add;
        //        add_operator = Entity.add_operator;

        //        Sp0_Desc = Entity.Sp0_Desc;
        //        Sp0_Validatetd = Entity.Sp0_Validatetd;
        //        Site_Desc = Entity.Site_Desc;

        //        CA_Postings_Cnt = Entity.CA_Postings_Cnt;
        //        MS_Postings_Cnt = Entity.CA_Postings_Cnt;

        //    }
        //}

        #endregion

        #region Properties

        public string Selected { get; set; }
        public string year { get; set; }
        public string MST_app_no { get; set; }
        public string AppName { get; set; }
        public string Active { get; set; }
        public string MST_Intake_Date { get; set; }
        public string App_Address { get; set; }
        public string site { get; set; }
        public string Start_date { get; set; }
        public string Seq { get; set; }
        public string PostSw { get; set; }
        public string Rowcolor { get; set; }
        public string AppMem { get; set; }

        public string FamSeq { get; set; }
        public string ClientID { get; set; }

        public string Post_Amounts { get; set; }

        public string ENRL_DATE { get; set; }

        #endregion

    }

    public class CT_Triggers_Entity
    {
        #region Constructors

        public CT_Triggers_Entity()
        {
            App =
            Name =
            B1 =
            U1 =
            R1 =
            Age =
            Levl_1 =
            Levl_2 =
            Levl_3 =
            Levl_4 =
            Levl_5 =
            string.Empty;
        }

        public CT_Triggers_Entity(bool Intialize)
        {
            if (Intialize)
            {
                App =
                Name =
                B1 =
                U1 =
                R1 =
                Age =
                Levl_1 =
                Levl_2 =
                Levl_3 =
                Levl_4 =
                Levl_5 =
                null;
            }
        }

        public CT_Triggers_Entity(DataRow row)
        {
            if (row != null)
            {
                App = row["MST_APP_NO"].ToString();
                Name = row["Name"].ToString();
                B1 = row["B1"].ToString();
                U1 = row["U1"].ToString();
                R1 = row["R1"].ToString();
                Age = row["Age_60"].ToString();
                Levl_1 = row["Ben_1"].ToString();
                Levl_2 = row["Ben_2"].ToString();
                Levl_3 = row["Ben_3"].ToString();
                Levl_4 = row["Ben_4"].ToString();
                Levl_5 = row["Ben_5"].ToString();
            }
        }

        #endregion

        #region Properties

        public string App { get; set; }
        public string Name { get; set; }
        public string B1 { get; set; }
        public string U1 { get; set; }
        public string R1 { get; set; }
        public string Age { get; set; }
        public string Levl_1 { get; set; }
        public string Levl_2 { get; set; }
        public string Levl_3 { get; set; }
        public string Levl_4 { get; set; }
        public string Levl_5 { get; set; }

        #endregion

    }
    //**********************************************************************************************************

    public class CASEACTEntity
    {
        #region Constructors

        public CASEACTEntity()
        {
            Rec_Type =
            Agency =
            Dept =
            Program =
            Year =
            App_no =
            Service_plan =
            SPM_Seq =
            Branch =
            Group =
            //Type = 
            ACT_Code =
            ACT_ID =
            ACT_Date =
            ACT_Seq =
            Site =
            Fund1 =
            Fund2 =
            Fund3 =
            Caseworker =
            Vendor_No =
            Check_Date =
            Check_No =
            Cost =
            Followup_On =
            Followup_Comp =
            Followup_By =
            Refer_Data =
            Cust_Code1 =
            Cust_Value1 =
            Cust_Code2 =
            Cust_Value2 =
            Cust_Code3 =
            Cust_Value3 =
            Cust_Code4 =
            Cust_Value4 =
            Cust_Code5 =
            Cust_Value5 =
            Bulk =
            Act_PROG =
            Lstc_Date =
            Lsct_Operator =
            Add_Date =
            Add_Operator =
            Notes_Count =
            Units =
            UOM =
            VOUCHNO =
            Curr_Grp =
            ACT_TrigCode =
            ACT_TrigDate =
            ACT_TrigDateSeq =
            ActSeek_Date =
            CA_OBF =
            BillingPeriod =

            ProcessOn =
            Print_Existing =

            BillngType =
            BillngFname =
            BillngLname =
            PaymentNo =
            Account =
            ArrearsAmt =
            LVL1Apprval =
            LVL1AprrvalDate =
            LVL2Apprval =
            LVL2ApprvalDate =
            SentPmtUser =
            SentPmtDate =

            CA_Source =
            Elec_Other =

            IsSave = IsTemplate =
            BundleNo = MstSite = VendorName = RejectCode = RejectBy = RejectDate = BDC_Desc = BDC_FundCode = BDC_ID =
            HNo = Street = Suffix = Floor = Apt = City = State = Zip = BenefitReason = PDOUT = string.Empty;
        }

        public CASEACTEntity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Program =
                Year =
                App_no =
                Service_plan =
                SPM_Seq =
                Branch =
                Group =
                //Type = 
                ACT_Code =
                ACT_ID =
                ACT_Date =
                ACT_Seq =
                Site =
                Fund1 =
                Fund2 =
                Fund3 =
                Caseworker =
                Vendor_No =
                Check_Date =
                Check_No =
                Cost =
                Followup_On =
                Followup_Comp =
                Followup_By =
                Refer_Data =
                Cust_Code1 =
                Cust_Value1 =
                Cust_Code2 =
                Cust_Value2 =
                Cust_Code3 =
                Cust_Value3 =
                Cust_Code4 =
                Cust_Value4 =
                Cust_Code5 =
                Cust_Value5 =
                Bulk =
                Act_PROG =
                Lstc_Date =
                Lsct_Operator =
                Add_Date =
                Add_Operator =
                Notes_Count =
                Units =
                UOM =
                VOUCHNO =
                Curr_Grp =
                ACT_TrigCode =
                ACT_TrigDate =
                ACT_TrigDateSeq =
                ActSeek_Date =
                CA_OBF =
                Rate =
                Amount =
                Amount2 =
                Amount3 =
                Units2 =
                UOM2 =
                Units3 =
                UOM3 =
                BillingPeriod =

                 ProcessOn =
            Print_Existing =

                    BillngType =
                BillngFname =
                BillngLname =
                PaymentNo =
                Account =
                ArrearsAmt =
                LVL1Apprval =
                LVL1AprrvalDate =
                LVL2Apprval =
                LVL2ApprvalDate =
                SentPmtUser =
                SentPmtDate =
                BundleNo =

                CA_Source =
                Elec_Other =
                IsSave = IsTemplate =
                BDC_Desc = BDC_FundCode = BDC_ID =
                  MstSite = BenefitReason = PDOUT = null;
            }
        }

        public CASEACTEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Agency = row["CASEACT_AGENCY"].ToString();
                Dept = row["CASEACT_DEPT"].ToString();
                Program = row["CASEACT_PROGRAM"].ToString();
                Year = row["CASEACT_YEAR"].ToString();
                App_no = row["CASEACT_APP_NO"].ToString();
                Service_plan = row["CASEACT_SERVICE_PLAN"].ToString();
                SPM_Seq = row["CASEACT_SPM_SEQ"].ToString();
                Branch = row["CASEACT_BRANCH"].ToString();
                Group = row["CASEACT_GROUP"].ToString();
                //Type = row["CASEACT_TYPE"].ToString();
                ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString();
                ACT_ID = row["CASEACT_ID"].ToString();
                ACT_Date = row["CASEACT_ACTY_DATE"].ToString();
                ACT_Seq = row["CASEACT_SEQ"].ToString();
                Site = row["CASEACT_SITE"].ToString().Trim();
                Fund1 = row["CASEACT_FUND1"].ToString();
                Fund2 = row["CASEACT_FUND2"].ToString();
                Fund3 = row["CASEACT_FUND3"].ToString();
                Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim();
                Vendor_No = row["CASEACT_VENDOR_NO"].ToString();
                Check_Date = row["CASEACT_CHECK_DT"].ToString();
                Check_No = row["CASEACT_CHECK_NO"].ToString();
                Cost = row["CASEACT_COST"].ToString();
                Followup_On = row["CASEACT_FOLLUP_ON"].ToString();
                Followup_Comp = row["CASEACT_FOLLUP_COMP"].ToString();
                Followup_By = row["CASEACT_FUPBY"].ToString();
                //Refer_Data = row["CASEACT_REF_DATA"].ToString();
                Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString();
                Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString();
                Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString();
                Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString();
                Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString();
                Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString();
                Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString();
                Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString();
                Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString();
                Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString();
                Bulk = row["CASEACT_BULK"].ToString();
                Act_PROG = row["CASEACT_ACTY_PROG"].ToString();
                Lstc_Date = row["CASEACT_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                Add_Date = row["CASEACT_DATE_ADD"].ToString();
                Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
                ActSeek_Date = row["CASEACT_SEEK_DATE"].ToString().Trim();
                Notes_Count = row["NOTES_COUNT"].ToString();
                CA_OBF = row["CASEACT_OBF"].ToString();

                Units = row["CASEACT_UNITS"].ToString();
                UOM = row["CASEACT_UOM"].ToString();
                VOUCHNO = row["CASEACT_VOUCHNO"].ToString();
                Curr_Grp = row["CASEACT_CURR_GRP"].ToString();

                Rate = row["CASEACT_RATE"].ToString();
                Amount = row["CASEACT_AMOUNT"].ToString();
                Amount2 = row["CASEACT_AMOUNT2"].ToString();
                Amount3 = row["CASEACT_AMOUNT3"].ToString();
                Units2 = row["CASEACT_UNITS2"].ToString();
                UOM2 = row["CASEACT_UOM2"].ToString();
                Units3 = row["CASEACT_UNITS3"].ToString();
                UOM3 = row["CASEACT_UOM3"].ToString();

                BundleNo = row["CASEACT_BUNDLE_NO"].ToString();

                BillngType = row["CASEACT_BILL_TYPE"].ToString();
                BillngFname = row["CASEACT_BILL_FNAME"].ToString();
                BillngLname = row["CASEACT_BILL_LNAME"].ToString();

                PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                CA_Source = row["CASEACT_SOURCE"].ToString();
                Elec_Other = row["CASEACT_ELEC_OTHER"].ToString();
                BDC_ID = row["CASEACT_BDC_ID"].ToString();
                BenefitReason = row["CASEACT_BENEFIT_REASN"].ToString();

                PDOUT = row["CASEACT_PDOUT"].ToString();

                Account = row["CASEACT_VEND_ACCT"].ToString().Trim();

            }
        }

        public CASEACTEntity(CASEACTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App_no = Entity.App_no;
                Service_plan = Entity.Service_plan;
                SPM_Seq = Entity.SPM_Seq;
                Branch = Entity.Branch;
                Group = Entity.Group;
                //Type = Entity.Type;
                ACT_Code = Entity.ACT_Code;
                ACT_ID = Entity.ACT_ID;
                ACT_Date = Entity.ACT_Date;
                ACT_Seq = Entity.ACT_Seq;
                Site = Entity.Site;
                Fund1 = Entity.Fund1;
                Fund2 = Entity.Fund2;
                Fund3 = Entity.Fund3;
                Caseworker = Entity.Caseworker;
                Vendor_No = Entity.Vendor_No;
                Check_Date = Entity.Check_Date;
                Check_No = Entity.Check_No;
                Cost = Entity.Cost;
                Followup_On = Entity.Followup_On;
                Followup_Comp = Entity.Followup_Comp;
                Followup_By = Entity.Followup_By;
                Refer_Data = Entity.Refer_Data;
                Cust_Code1 = Entity.Cust_Code1;
                Cust_Value1 = Entity.Cust_Value1;
                Cust_Code2 = Entity.Cust_Code2;
                Cust_Value2 = Entity.Cust_Value2;
                Cust_Code3 = Entity.Cust_Code3;
                Cust_Value3 = Entity.Cust_Value3;
                Cust_Code4 = Entity.Cust_Code4;
                Cust_Value4 = Entity.Cust_Value4;
                Cust_Code5 = Entity.Cust_Code5;
                Cust_Value5 = Entity.Cust_Value5;
                Bulk = Entity.Bulk;
                Act_PROG = Entity.Act_PROG;
                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                Notes_Count = Entity.Notes_Count;
                UOM = Entity.UOM;
                Units = Entity.Units;
                VOUCHNO = Entity.VOUCHNO;
                Curr_Grp = Entity.Curr_Grp;
                ActSeek_Date = Entity.ActSeek_Date;
                CA_OBF = Entity.CA_OBF;
                ACT_TrigCode = Entity.ACT_TrigCode;
                ACT_TrigDate = Entity.ACT_TrigDate;
                ACT_TrigDateSeq = Entity.ACT_TrigDateSeq;

                Rate = Entity.Rate;
                Amount = Entity.Amount;
                Amount2 = Entity.Amount2;
                Amount3 = Entity.Amount3;
                UOM2 = Entity.UOM2;
                Units2 = Entity.Units2;
                UOM3 = Entity.UOM3;
                Units3 = Entity.Units3;


                BillngType = Entity.BillngType;
                BillngFname = Entity.BillngFname;
                BillngLname = Entity.BillngLname;
                PaymentNo = Entity.PaymentNo;

                BillingPeriod = Entity.BillingPeriod;
                Account = Entity.Account;
                ArrearsAmt = Entity.ArrearsAmt;
                LVL1Apprval = Entity.LVL1Apprval;
                LVL1AprrvalDate = Entity.LVL1AprrvalDate;
                LVL2Apprval = Entity.LVL2Apprval;
                LVL2ApprvalDate = Entity.LVL2ApprvalDate;
                SentPmtUser = Entity.SentPmtUser;
                SentPmtDate = Entity.SentPmtDate;
                BundleNo = Entity.BundleNo;

                CA_Source = Entity.CA_Source;
                Elec_Other = Entity.Elec_Other;

                BDC_ID = Entity.BDC_ID;
                BenefitReason = Entity.BenefitReason;
                PDOUT = Entity.PDOUT;

                CADesc = Entity.CADesc;
            }
        }

        public CASEACTEntity(DataRow CASEAct, string strType)
        {
            if (CASEAct != null)
            {
                DataRow row = CASEAct;


                if (strType == "CASE0025")
                {

                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();
                    BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    //BillngType = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngFname = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngLname = row["CASEACT_SENT_PMT_DATE"].ToString();

                    LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    RejectBy = row["CASEACT_REJECT_BY"].ToString();
                    RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();
                    CADesc = row["CA_DESC"].ToString();

                    PDOUT = row["CASEACT_PDOUT"].ToString();



                }
                else if (strType == "CASB0015")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();
                    BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    //BillngType = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngFname = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngLname = row["CASEACT_SENT_PMT_DATE"].ToString();

                    LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();

                    HNo = row["MST_HN"].ToString();
                    Street = row["MST_STREET"].ToString();
                    Suffix = row["MST_SUFFIX"].ToString();
                    Apt = row["MST_APT"].ToString();
                    Floor = row["MST_FLR"].ToString();
                    City = row["MST_CITY"].ToString();
                    State = row["MST_STATE"].ToString();
                    Zip = row["MST_ZIP"].ToString();

                    CADesc = row["CA_DESC"].ToString();
                    //CA_VEND_PAY_CAT = row["CA_VEND_PAY_CAT"].ToString();
                    //AGY_1 = row["AGY_1"].ToString();

                }
                else if (strType == "CASB0019")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();

                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    //SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    //Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    //Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    //Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    //Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    //Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    //CA_OBF = row["CASEACT_OBF"].ToString();
                    //BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    //ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    //LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    BillngType = row["CASEACT_BILL_TYPE"].ToString();
                    BillngFname = row["CASEACT_BILL_FNAME"].ToString();
                    BillngLname = row["CASEACT_BILL_LNAME"].ToString();

                    //LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    //LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    //LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    //RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    //RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();


                    HNo = row["MST_HN"].ToString();
                    Street = row["MST_STREET"].ToString();
                    Suffix = row["MST_SUFFIX"].ToString();
                    Apt = row["MST_APT"].ToString();
                    Floor = row["MST_FLR"].ToString();
                    City = row["MST_CITY"].ToString();
                    State = row["MST_STATE"].ToString();
                    Zip = row["MST_ZIP"].ToString();

                    CADesc = row["CA_DESC"].ToString();
                    //CA_VEND_PAY_CAT = row["CA_VEND_PAY_CAT"].ToString();
                    //AGY_1 = row["AGY_1"].ToString();

                    BDC_Desc = row["BDC_DESCRIPTION"].ToString();
                    BDC_FundCode = row["BDC_FUND_CODE"].ToString();

                    PDOUT = row["CASEACT_PDOUT"].ToString();

                    Source = row["CASEACT_SOURCE"].ToString();

                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString();

                    County = row["MST_COUNTY"].ToString();
                    DIFF_County = row["DIFF_COUNTY"].ToString();
                    Site= row["MST_SITE"].ToString();

                }
                else if (strType == "CASB0020")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();

                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    //SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    //Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    //Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    //Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    //Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    //Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    //CA_OBF = row["CASEACT_OBF"].ToString();
                    //BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    //ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    //LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    //BillngType = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngFname = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngLname = row["CASEACT_SENT_PMT_DATE"].ToString();

                    //LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    //LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    //LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    //RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    //RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();


                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();

                    HNo = row["MST_HN"].ToString();
                    Street = row["MST_STREET"].ToString();
                    Suffix = row["MST_SUFFIX"].ToString();
                    Apt = row["MST_APT"].ToString();
                    Floor = row["MST_FLR"].ToString();
                    City = row["MST_CITY"].ToString();
                    State = row["MST_STATE"].ToString();
                    Zip = row["MST_ZIP"].ToString();

                    MailAddr1 = row["MailAddr1"].ToString();
                    MailAddr2 = row["MailAddr2"].ToString();
                    DIFF_City = row["DIFF_CITY"].ToString();
                    DIFF_County = row["DIFF_COUNTY"].ToString();

                    CADesc = row["CA_DESC"].ToString();
                    //CA_VEND_PAY_CAT = row["CA_VEND_PAY_CAT"].ToString();
                    //AGY_1 = row["AGY_1"].ToString();

                    BDC_Desc = row["BDC_DESCRIPTION"].ToString();
                    BDC_FundCode = row["BDC_FUND_CODE"].ToString();

                    BillngType = row["CASEACT_BILL_TYPE"].ToString();
                    BillngFname = row["CASEACT_BILL_FNAME"].ToString();
                    BillngLname = row["CASEACT_BILL_LNAME"].ToString();

                    CAB_DATE_OPENED = row["CAB_DATE_OPENED"].ToString();
                    CA_Source = row["CASEACT_SOURCE"].ToString();
                    Source = row["AGY_7"].ToString();
                    County = row["COUNTY"].ToString();

                    MstSite = row["MST_SITE"].ToString().Trim();
                }
                else if (strType == "CASB0020CHK")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();

                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    //SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    //Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    //Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    //Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    //Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    //Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    //CA_OBF = row["CASEACT_OBF"].ToString();
                    //BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    //ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    //LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    //BillngType = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngFname = row["CASEACT_SENT_PMT_DATE"].ToString();
                    //BillngLname = row["CASEACT_SENT_PMT_DATE"].ToString();

                    //LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    //LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    //LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    //RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    //RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();


                    HNo = row["MST_HN"].ToString();
                    Street = row["MST_STREET"].ToString();
                    Suffix = row["MST_SUFFIX"].ToString();
                    Apt = row["MST_APT"].ToString();
                    Floor = row["MST_FLR"].ToString();
                    City = row["MST_CITY"].ToString();
                    State = row["MST_STATE"].ToString();
                    Zip = row["MST_ZIP"].ToString();

                    CADesc = row["CA_DESC"].ToString();
                    //CA_VEND_PAY_CAT = row["CA_VEND_PAY_CAT"].ToString();
                    //AGY_1 = row["AGY_1"].ToString();

                    Check_Date = row["CASEACT_CHECK_DT"].ToString();
                    Check_No = row["CASEACT_CHECK_NO"].ToString();
                    PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                    BDC_Desc = row["BDC_DESCRIPTION"].ToString();
                    BDC_FundCode = row["BDC_FUND_CODE"].ToString();

                }

                else if (strType == "Vendor")
                {
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();

                    VendorName = row["CASEVDD_NAME"].ToString();
                    VDD_W9 = row["CASEVDD_W9"].ToString();
                    VDD_FName = row["CASEVDD_FNAME"].ToString();
                    VDD_LName = row["CASEVDD_LNAME"].ToString();

                    VDD_SCode = row["CASEVDD1_SVENDOR_CODE"].ToString();

                }
                else if (strType == "Check")
                {
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    VendorName = row["CASEVDD_NAME"].ToString();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    
                    Check_Date = row["CASEACT_CHECK_DT"].ToString();
                    Check_No = row["CASEACT_CHECK_NO"].ToString();
                    PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    PDOUT = row["CASEACT_PDOUT"].ToString();

                }
                else if (strType == "WtCheck")
                {
                    //SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    //SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    VendorName = row["CASEVDD_NAME"].ToString();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();

                    //Check_Date = row["CASEACT_CHECK_DT"].ToString();
                    Check_No = row["CASEACT_CHECK_NO"].ToString();
                    PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    PDOUT = row["CASEACT_PDOUT"].ToString();

                }
                else if (strType == "Bundle")
                {
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();

                    //VendorName = row["CASEVDD_NAME"].ToString();
                }
                else if (strType == "WorkData")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();

                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();
                }
                //Added by sudheer on 06/17/2021
                else if (strType == "CASBLTRB")
                {

                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    //Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    //Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    if (string.IsNullOrEmpty(Cost.Trim()))
                        Cost = "0.00";

                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();
                    BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    //RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    //RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    BillngType = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BillngFname = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BillngLname = row["CASEACT_SENT_PMT_DATE"].ToString();

                    //PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                    //CA_Source= row["CASEACT_SOURCE"].ToString();
                    //Elec_Other= row["CASEACT_ELEC_OTHER"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();
                    CADesc = row["CA_DESC"].ToString();

                    CA_VEND_PAY_CAT = row["CA_VEND_PAY_CAT"].ToString();
                    AGY_1 = row["AGY_1"].ToString();
                    Source = row["AGY_7"].ToString();
                    County = row["COUNTY"].ToString();


                }
                // Added by Murali on 08/30/2022
                else if (strType == "CEAPB002")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();


                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString();
                    Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString();
                    Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString();
                    Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString();
                    Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString();
                    Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString();
                    Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString();
                    Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString();
                    Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString();
                    Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();

                    CA_Source = row["CASEACT_SOURCE"].ToString();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    CADesc = row["CA_DESC"].ToString().Trim();
                    ////  Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    //  Cost = row["CASEACT_COST"].ToString().Trim();
                    //  Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    //  Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    //  Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    //  Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    //  Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
                }
                else
                {
                    Rec_Type = "U";
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();
                    //Type = row["CASEACT_TYPE"].ToString();
                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();
                    Fund2 = row["CASEACT_FUND2"].ToString().Trim();
                    Fund3 = row["CASEACT_FUND3"].ToString().Trim();
                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Followup_On = row["CASEACT_FOLLUP_ON"].ToString().Trim();
                    Followup_Comp = row["CASEACT_FOLLUP_COMP"].ToString().Trim();
                    Followup_By = row["CASEACT_FUPBY"].ToString().Trim();
                    //Refer_Data = row["CASEACT_REF_DATA"].ToString();
                    Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString().Trim();
                    Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString().Trim();
                    Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString().Trim();
                    Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString().Trim();
                    Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString().Trim();
                    Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString().Trim();
                    Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString().Trim();
                    Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString().Trim();
                    Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString().Trim();
                    Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString().Trim();
                    Bulk = row["CASEACT_BULK"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
                    Curr_Grp = row["CASEACT_CURR_GRP"].ToString().Trim();
                    ActSeek_Date = row["CASEACT_SEEK_DATE"].ToString().Trim();

                    Units = row["CASEACT_UNITS"].ToString();
                    UOM = row["CASEACT_UOM"].ToString();
                    Rate = row["CASEACT_RATE"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();

                    //CaseNotesData = row["CASNOTES_DATA"].ToString().Trim();
                    if (strType == "Trigger")
                    {
                        ACT_TrigCode = row["CASEACT_TRIG_CODE"].ToString().Trim();
                        ACT_TrigDate = row["CASEACT_TRIG_DATE"].ToString().Trim();
                        ACT_TrigDateSeq = row["CASEACT_TRIG_DATE_SEQ"].ToString().Trim();
                    }

                    if (strType == "CASEACT2.0")
                    {
                        Amount = row["CASEACT_AMOUNT"].ToString().Trim();
                        Amount2 = row["CASEACT_AMOUNT2"].ToString().Trim();
                        Amount3 = row["CASEACT_AMOUNT3"].ToString().Trim();
                        Units2 = row["CASEACT_UNITS2"].ToString();
                        UOM2 = row["CASEACT_UOM2"].ToString();
                        Units3 = row["CASEACT_UNITS3"].ToString();
                        UOM3 = row["CASEACT_UOM3"].ToString();
                    }

                    if (strType == "PAYMENT")
                    {
                        Rate = row["CASEACT_RATE"].ToString().Trim();
                        Amount = row["CASEACT_AMOUNT"].ToString().Trim();
                        Amount2 = row["CASEACT_AMOUNT2"].ToString().Trim();
                        Amount3 = row["CASEACT_AMOUNT3"].ToString().Trim();
                        Units2 = row["CASEACT_UNITS2"].ToString();
                        UOM2 = row["CASEACT_UOM2"].ToString();
                        Units3 = row["CASEACT_UNITS3"].ToString();
                        UOM3 = row["CASEACT_UOM3"].ToString();

                        BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                        Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                        ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                        LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                        LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                        LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                        LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                        SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                        SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                        BundleNo = row["CASEACT_BUNDLE_NO"].ToString();

                        BillngType = row["CASEACT_BILL_TYPE"].ToString();
                        BillngFname = row["CASEACT_BILL_FNAME"].ToString();
                        BillngLname = row["CASEACT_BILL_LNAME"].ToString();

                        PaymentNo = row["CASEACT_PAYMENT_NO"].ToString();

                        CA_Source = row["CASEACT_SOURCE"].ToString();
                        Elec_Other = row["CASEACT_ELEC_OTHER"].ToString();
                        BDC_ID = row["CASEACT_BDC_ID"].ToString();
                        BenefitReason = row["CASEACT_BENEFIT_REASN"].ToString();
                        PDOUT = row["CASEACT_PDOUT"].ToString();

                    }


                }
            }
        }

        public CASEACTEntity(DataRow CASEAct, string strType, string strTable)
        {
            if (CASEAct != null)
            {
                DataRow row = CASEAct;

                if (strTable == "SSBG")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();
                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    //ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    //ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    ActSeek_Date = row["CASEACT_SEEK_DATE"].ToString().Trim();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                }
                else
                {
                    Rec_Type = "U";
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();
                    //Type = row["CASEACT_TYPE"].ToString();
                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();
                    Fund2 = row["CASEACT_FUND2"].ToString().Trim();
                    Fund3 = row["CASEACT_FUND3"].ToString().Trim();
                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Followup_On = row["CASEACT_FOLLUP_ON"].ToString().Trim();
                    Followup_Comp = row["CASEACT_FOLLUP_COMP"].ToString().Trim();
                    Followup_By = row["CASEACT_FUPBY"].ToString().Trim();
                    //Refer_Data = row["CASEACT_REF_DATA"].ToString();
                    Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString().Trim();
                    Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString().Trim();
                    Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString().Trim();
                    Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString().Trim();
                    Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString().Trim();
                    Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString().Trim();
                    Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString().Trim();
                    Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString().Trim();
                    Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString().Trim();
                    Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString().Trim();
                    Bulk = row["CASEACT_BULK"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
                    Curr_Grp = row["CASEACT_CURR_GRP"].ToString().Trim();
                    CaseNotesData = row["CASNOTES_DATA"].ToString().Trim();
                    Sp2_RowOrder = row["Sp2_Row"].ToString().Trim();
                }

            }
        }

        public CASEACTEntity(string strbooldata, string strmsdesc, string Comp_date, string Followup, string Img_Edit, string Type1, string CamCd, string C, string Branch, string Orig_Grp, string Notes_Exists, string Notes_KeyACT_Seq, string ACT_Seq, string CAMS_Desc, string Year, string CAMS_Active_Status, string ACT_ID, string Curr_Grp, string BranchData, string CA_Template_SW)
        {
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App_no { get; set; }
        public string Service_plan { get; set; }
        public string SPM_Seq { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        //public string Type { get; set; }
        public string ACT_Code { get; set; }
        public string ACT_ID { get; set; }
        public string ACT_Date { get; set; }
        public string ACT_Seq { get; set; }
        public string Site { get; set; }
        public string Fund1 { get; set; }
        public string Fund2 { get; set; }
        public string Fund3 { get; set; }
        public string Caseworker { get; set; }
        public string Vendor_No { get; set; }
        public string Check_Date { get; set; }
        public string Check_No { get; set; }


        public string Cost { get; set; }
        public string Followup_On { get; set; }
        public string Followup_Comp { get; set; }
        public string Followup_By { get; set; }
        public string Refer_Data { get; set; }
        public string Cust_Code1 { get; set; }
        public string Cust_Value1 { get; set; }
        public string Cust_Code2 { get; set; }
        public string Cust_Value2 { get; set; }
        public string Cust_Code3 { get; set; }
        public string Cust_Value3 { get; set; }
        public string Cust_Code4 { get; set; }
        public string Cust_Value4 { get; set; }
        public string Cust_Code5 { get; set; }
        public string Cust_Value5 { get; set; }
        public string Bulk { get; set; }
        public string Act_PROG { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Notes_Count { get; set; }
        public string Units { get; set; }
        public string UOM { get; set; }
        public string VOUCHNO { get; set; }
        public string Curr_Grp { get; set; }
        public string CaseNotesData { get; set; }
        public string Sp2_RowOrder { get; set; }
        public string ACT_TrigCode { get; set; }
        public string ACT_TrigDate { get; set; }
        public string ACT_TrigDateSeq { get; set; }
        public string ActSeek_Date { get; set; }
        public string CA_OBF { get; set; }

        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Amount2 { get; set; }
        public string Amount3 { get; set; }
        public string Units2 { get; set; }
        public string UOM2 { get; set; }
        public string Units3 { get; set; }
        public string UOM3 { get; set; }

        public string BillingPeriod { get; set; }
        public string Account { get; set; }
        public string ArrearsAmt { get; set; }
        public string LVL1Apprval { get; set; }
        public string LVL1AprrvalDate { get; set; }
        public string LVL2Apprval { get; set; }
        public string LVL2ApprvalDate { get; set; }
        public string SentPmtUser { get; set; }
        public string SentPmtDate { get; set; }
        public string BundleNo { get; set; }

        public string BillngType { get; set; }
        public string BillngFname { get; set; }
        public string BillngLname { get; set; }

        public string PaymentNo { get; set; }

        public string CA_Source { get; set; }
        public string Elec_Other { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MstSite { get; set; }
        public string VendorName { get; set; }
        public string RejectCode { get; set; }
        public string RejectBy { get; set; }
        public string RejectDate { get; set; }

        public string HNo { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public string Apt { get; set; }
        public string Floor { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }


        public string MailAddr1{get;set;}
        public string MailAddr2 { get; set; }
        public string DIFF_City { get; set; }
        public string DIFF_County { get; set; }

        public string CADesc { get; set; }
        public string VDD_W9 { get; set; }
        public string VDD_FName { get; set; }
        public string VDD_LName { get; set; }
        public string VDD_SCode { get; set; }

        public string CA_VEND_PAY_CAT { get; set; }
        public string AGY_1 { get; set; }
        public string Source { get; set; }

        public string County { get; set; }

        public string BDC_Desc { get; set; }
        public string BDC_FundCode { get; set; }
        public string BDC_ID { get; set; }

        public string ProcessOn { get; set; }
        public string Print_Existing { get; set; }
        public string BenefitReason { get; set; }
        public string PDOUT { get; set; }

        public string CAB_DATE_OPENED { get; set; }

        public string IsSave { get; set; }
        public string IsTemplate { get; set; }

        #endregion
    }


    public class CASB0015Entity
    {
        #region Constructors

        public CASB0015Entity()
        {
            Rec_Type =
            Agency =
            Dept =
            Program =
            Year =
            App_no =
            Service_plan =
            SPM_Seq =
            Branch =
            Group =
            //Type = 
            ACT_Code =
            ACT_ID =
            ACT_Date =
            ACT_Seq =
            Site =
            Fund1 =
            Fund2 =
            Fund3 =
            Caseworker =
            Vendor_No =
            Check_Date =
            Check_No =
            Cost =
            Followup_On =
            Followup_Comp =
            Followup_By =
            Refer_Data =
            Cust_Code1 =
            Cust_Value1 =
            Cust_Code2 =
            Cust_Value2 =
            Cust_Code3 =
            Cust_Value3 =
            Cust_Code4 =
            Cust_Value4 =
            Cust_Code5 =
            Cust_Value5 =
            Bulk =
            Act_PROG =
            Lstc_Date =
            Lsct_Operator =
            Add_Date =
            Add_Operator =
            Notes_Count =
            Units =
            UOM =
            VOUCHNO =
            Curr_Grp =
            ACT_TrigCode =
            ACT_TrigDate =
            ACT_TrigDateSeq =
            ActSeek_Date =
            CA_OBF =
            BillingPeriod =
            Account =
            ArrearsAmt =
            LVL1Apprval =
            LVL1AprrvalDate =
            LVL2Apprval =
            LVL2ApprvalDate =
            SentPmtUser =
            SentPmtDate =
            BundleNo = MstSite = VendorName = RejectCode = RejectDate =
            Vend_Addr1 =
                Vend_Addr2 =
                Vend_Addr3 =
                Vdd_City =
                Vdd_State =
                Vdd_Zip =
            string.Empty;
        }

        public CASB0015Entity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Program =
                Year =
                App_no =
                Service_plan =
                SPM_Seq =
                Branch =
                Group =
                //Type = 
                ACT_Code =
                ACT_ID =
                ACT_Date =
                ACT_Seq =
                Site =
                Fund1 =
                Fund2 =
                Fund3 =
                Caseworker =
                Vendor_No =
                Check_Date =
                Check_No =
                Cost =
                Followup_On =
                Followup_Comp =
                Followup_By =
                Refer_Data =
                Cust_Code1 =
                Cust_Value1 =
                Cust_Code2 =
                Cust_Value2 =
                Cust_Code3 =
                Cust_Value3 =
                Cust_Code4 =
                Cust_Value4 =
                Cust_Code5 =
                Cust_Value5 =
                Bulk =
                Act_PROG =
                Lstc_Date =
                Lsct_Operator =
                Add_Date =
                Add_Operator =
                Notes_Count =
                Units =
                UOM =
                VOUCHNO =
                Curr_Grp =
                ACT_TrigCode =
                ACT_TrigDate =
                ACT_TrigDateSeq =
                ActSeek_Date =
                CA_OBF =
                Amount2 =
                Amount3 =
                Units2 =
                UOM2 =
                Units3 =
                UOM3 =
                BillingPeriod =
                Account =
                ArrearsAmt =
                LVL1Apprval =
                LVL1AprrvalDate =
                LVL2Apprval =
                LVL2ApprvalDate =
                SentPmtUser =
                SentPmtDate =
                BundleNo =
                Vend_Addr1 =
                Vend_Addr2 =
                Vend_Addr3 =
                Vdd_City =
                Vdd_State =
                Vdd_Zip =

                MstSite = null;
            }
        }

        //public CASB0015Entity(DataRow CASESPM)
        //{
        //    if (CASESPM != null)
        //    {
        //        DataRow row = CASESPM;

        //        Rec_Type = "U";
        //        Agency = row["CASEACT_AGENCY"].ToString();
        //        Dept = row["CASEACT_DEPT"].ToString();
        //        Program = row["CASEACT_PROGRAM"].ToString();
        //        Year = row["CASEACT_YEAR"].ToString();
        //        App_no = row["CASEACT_APP_NO"].ToString();
        //        Service_plan = row["CASEACT_SERVICE_PLAN"].ToString();
        //        SPM_Seq = row["CASEACT_SPM_SEQ"].ToString();
        //        Branch = row["CASEACT_BRANCH"].ToString();
        //        Group = row["CASEACT_GROUP"].ToString();
        //        //Type = row["CASEACT_TYPE"].ToString();
        //        ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString();
        //        ACT_ID = row["CASEACT_ID"].ToString();
        //        ACT_Date = row["CASEACT_ACTY_DATE"].ToString();
        //        ACT_Seq = row["CASEACT_SEQ"].ToString();
        //        Site = row["CASEACT_SITE"].ToString().Trim();
        //        Fund1 = row["CASEACT_FUND1"].ToString();
        //        Fund2 = row["CASEACT_FUND2"].ToString();
        //        Fund3 = row["CASEACT_FUND3"].ToString();
        //        Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim();
        //        Vendor_No = row["CASEACT_VENDOR_NO"].ToString();
        //        Check_Date = row["CASEACT_CHECK_DT"].ToString();
        //        Check_No = row["CASEACT_CHECK_NO"].ToString();
        //        Cost = row["CASEACT_COST"].ToString();
        //        Followup_On = row["CASEACT_FOLLUP_ON"].ToString();
        //        Followup_Comp = row["CASEACT_FOLLUP_COMP"].ToString();
        //        Followup_By = row["CASEACT_FUPBY"].ToString();
        //        //Refer_Data = row["CASEACT_REF_DATA"].ToString();
        //        Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString();
        //        Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString();
        //        Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString();
        //        Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString();
        //        Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString();
        //        Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString();
        //        Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString();
        //        Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString();
        //        Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString();
        //        Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString();
        //        Bulk = row["CASEACT_BULK"].ToString();
        //        Act_PROG = row["CASEACT_ACTY_PROG"].ToString();
        //        Lstc_Date = row["CASEACT_DATE_LSTC"].ToString();
        //        Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
        //        Add_Date = row["CASEACT_DATE_ADD"].ToString();
        //        Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
        //        ActSeek_Date = row["CASEACT_SEEK_DATE"].ToString().Trim();
        //        Notes_Count = row["NOTES_COUNT"].ToString();
        //        CA_OBF = row["CASEACT_OBF"].ToString();

        //        Units = row["CASEACT_UNITS"].ToString();
        //        UOM = row["CASEACT_UOM"].ToString();
        //        VOUCHNO = row["CASEACT_VOUCHNO"].ToString();
        //        Curr_Grp = row["CASEACT_CURR_GRP"].ToString();

        //        Amount2 = row["CASEACT_AMOUNT2"].ToString();
        //        Amount3 = row["CASEACT_AMOUNT3"].ToString();
        //        Units2 = row["CASEACT_UNITS2"].ToString();
        //        UOM2 = row["CASEACT_UOM2"].ToString();
        //        Units3 = row["CASEACT_UNITS3"].ToString();
        //        UOM3 = row["CASEACT_UOM3"].ToString();

        //    }
        //}

        public CASB0015Entity(CASB0015Entity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App_no = Entity.App_no;
                Service_plan = Entity.Service_plan;
                SPM_Seq = Entity.SPM_Seq;
                Branch = Entity.Branch;
                Group = Entity.Group;
                //Type = Entity.Type;
                ACT_Code = Entity.ACT_Code;
                ACT_ID = Entity.ACT_ID;
                ACT_Date = Entity.ACT_Date;
                ACT_Seq = Entity.ACT_Seq;
                Site = Entity.Site;
                Fund1 = Entity.Fund1;
                Fund2 = Entity.Fund2;
                Fund3 = Entity.Fund3;
                Caseworker = Entity.Caseworker;
                Vendor_No = Entity.Vendor_No;
                Check_Date = Entity.Check_Date;
                Check_No = Entity.Check_No;
                Cost = Entity.Cost;
                Followup_On = Entity.Followup_On;
                Followup_Comp = Entity.Followup_Comp;
                Followup_By = Entity.Followup_By;
                Refer_Data = Entity.Refer_Data;
                Cust_Code1 = Entity.Cust_Code1;
                Cust_Value1 = Entity.Cust_Value1;
                Cust_Code2 = Entity.Cust_Code2;
                Cust_Value2 = Entity.Cust_Value2;
                Cust_Code3 = Entity.Cust_Code3;
                Cust_Value3 = Entity.Cust_Value3;
                Cust_Code4 = Entity.Cust_Code4;
                Cust_Value4 = Entity.Cust_Value4;
                Cust_Code5 = Entity.Cust_Code5;
                Cust_Value5 = Entity.Cust_Value5;
                Bulk = Entity.Bulk;
                Act_PROG = Entity.Act_PROG;
                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                Notes_Count = Entity.Notes_Count;
                UOM = Entity.UOM;
                Units = Entity.Units;
                VOUCHNO = Entity.VOUCHNO;
                Curr_Grp = Entity.Curr_Grp;
                ActSeek_Date = Entity.ActSeek_Date;
                CA_OBF = Entity.CA_OBF;
                ACT_TrigCode = Entity.ACT_TrigCode;
                ACT_TrigDate = Entity.ACT_TrigDate;
                ACT_TrigDateSeq = Entity.ACT_TrigDateSeq;

                Amount2 = Entity.Amount2;
                Amount3 = Entity.Amount3;
                UOM2 = Entity.UOM2;
                Units2 = Entity.Units2;
                UOM3 = Entity.UOM3;
                Units3 = Entity.Units3;

                BillingPeriod = Entity.BillingPeriod;
                Account = Entity.Account;
                ArrearsAmt = Entity.ArrearsAmt;
                LVL1Apprval = Entity.LVL1Apprval;
                LVL1AprrvalDate = Entity.LVL1AprrvalDate;
                LVL2Apprval = Entity.LVL2Apprval;
                LVL2ApprvalDate = Entity.LVL2ApprvalDate;
                SentPmtUser = Entity.SentPmtUser;
                SentPmtDate = Entity.SentPmtDate;
                BundleNo = Entity.BundleNo;

                Vend_Addr1 = Entity.Vend_Addr1;
                Vend_Addr2 = Entity.Vend_Addr2;
                Vend_Addr3 = Entity.Vend_Addr3;
                Vdd_City = Entity.Vdd_City;
                Vdd_State = Entity.Vdd_State;
                Vdd_Zip = Entity.Vdd_Zip;
            }
        }

        public CASB0015Entity(DataRow CASEAct, string strType)
        {
            if (CASEAct != null)
            {
                DataRow row = CASEAct;


                if (strType == "CASE0025")
                {

                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                    Fund1 = row["CASEACT_FUND1"].ToString().Trim();

                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();
                    BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();


                }
                else if (strType == "CASB0015")
                {
                    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                    Dept = row["CASEACT_DEPT"].ToString().Trim();
                    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                    Year = row["CASEACT_YEAR"].ToString().Trim();
                    App_no = row["CASEACT_APP_NO"].ToString();
                    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                    Group = row["CASEACT_GROUP"].ToString().Trim();

                    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();

                    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();
                    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();

                    CA_OBF = row["CASEACT_OBF"].ToString();
                    BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                    Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                    ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                    LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                    LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                    LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                    LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    RejectCode = row["CASEACT_REJECT_CODE"].ToString();
                    RejectDate = row["CASEACT_REJECT_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString();
                    LastName = row["SNP_NAME_IX_LAST"].ToString();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString();

                    VendorName = row["CASEVDD_NAME"].ToString();

                    Vend_Addr1 = row["CASEVDD_ADDR1"].ToString();
                    Vend_Addr2 = row["CASEVDD_ADDR2"].ToString();
                    Vend_Addr3 = row["CASEVDD_ADDR3"].ToString();
                    Vdd_City = row["CASEVDD_CITY"].ToString();
                    Vdd_State = row["CASEVDD_STATE"].ToString();
                    Vdd_Zip = row["CASEVDD_ZIP"].ToString();

                }
                else if (strType == "Bundle")
                {
                    SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                    SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                    BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                    Cost = row["CASEACT_COST"].ToString().Trim();

                    VendorName = row["CASEVDD_NAME"].ToString();
                }
                //else
                //{
                //    Rec_Type = "U";
                //    Agency = row["CASEACT_AGENCY"].ToString().Trim();
                //    Dept = row["CASEACT_DEPT"].ToString().Trim();
                //    Program = row["CASEACT_PROGRAM"].ToString().Trim();
                //    Year = row["CASEACT_YEAR"].ToString().Trim();
                //    App_no = row["CASEACT_APP_NO"].ToString();
                //    Service_plan = row["CASEACT_SERVICE_PLAN"].ToString().Trim();
                //    SPM_Seq = row["CASEACT_SPM_SEQ"].ToString().Trim();
                //    Branch = row["CASEACT_BRANCH"].ToString().Trim();
                //    Group = row["CASEACT_GROUP"].ToString().Trim();
                //    //Type = row["CASEACT_TYPE"].ToString();
                //    ACT_Code = row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                //    ACT_ID = row["CASEACT_ID"].ToString().Trim();
                //    ACT_Date = row["CASEACT_ACTY_DATE"].ToString().Trim();
                //    ACT_Seq = row["CASEACT_SEQ"].ToString().Trim();
                //    Site = row["CASEACT_SITE"].ToString().Trim().Trim();
                //    Fund1 = row["CASEACT_FUND1"].ToString().Trim();
                //    Fund2 = row["CASEACT_FUND2"].ToString().Trim();
                //    Fund3 = row["CASEACT_FUND3"].ToString().Trim();
                //    Caseworker = row["CASEACT_CASEWRKR"].ToString().Trim().Trim();
                //    Vendor_No = row["CASEACT_VENDOR_NO"].ToString().Trim();
                //    Check_Date = row["CASEACT_CHECK_DT"].ToString().Trim();
                //    Check_No = row["CASEACT_CHECK_NO"].ToString().Trim();
                //    Cost = row["CASEACT_COST"].ToString().Trim();
                //    Followup_On = row["CASEACT_FOLLUP_ON"].ToString().Trim();
                //    Followup_Comp = row["CASEACT_FOLLUP_COMP"].ToString().Trim();
                //    Followup_By = row["CASEACT_FUPBY"].ToString().Trim();
                //    //Refer_Data = row["CASEACT_REF_DATA"].ToString();
                //    Cust_Code1 = row["CASEACT_CUST1_CODE"].ToString().Trim();
                //    Cust_Value1 = row["CASEACT_CUST1_VALUE"].ToString().Trim();
                //    Cust_Code2 = row["CASEACT_CUST2_CODE"].ToString().Trim();
                //    Cust_Value2 = row["CASEACT_CUST2_VALUE"].ToString().Trim();
                //    Cust_Code3 = row["CASEACT_CUST3_CODE"].ToString().Trim();
                //    Cust_Value3 = row["CASEACT_CUST3_VALUE"].ToString().Trim();
                //    Cust_Code4 = row["CASEACT_CUST4_CODE"].ToString().Trim();
                //    Cust_Value4 = row["CASEACT_CUST4_VALUE"].ToString().Trim();
                //    Cust_Code5 = row["CASEACT_CUST5_CODE"].ToString().Trim();
                //    Cust_Value5 = row["CASEACT_CUST5_VALUE"].ToString().Trim();
                //    Bulk = row["CASEACT_BULK"].ToString().Trim();
                //    Act_PROG = row["CASEACT_ACTY_PROG"].ToString().Trim();
                //    Lstc_Date = row["CASEACT_DATE_LSTC"].ToString().Trim();
                //    Lsct_Operator = row["CASEACT_LSTC_OPERATOR"].ToString().Trim();
                //    Add_Date = row["CASEACT_DATE_ADD"].ToString().Trim().Trim();
                //    Add_Operator = row["CASEACT_ADD_OPERATOR"].ToString().Trim();
                //    Curr_Grp = row["CASEACT_CURR_GRP"].ToString().Trim();
                //    ActSeek_Date = row["CASEACT_SEEK_DATE"].ToString().Trim();

                //    Units = row["CASEACT_UNITS"].ToString();
                //    UOM = row["CASEACT_UOM"].ToString();

                //    CA_OBF = row["CASEACT_OBF"].ToString();

                //    //CaseNotesData = row["CASNOTES_DATA"].ToString().Trim();
                //    if (strType == "Trigger")
                //    {
                //        ACT_TrigCode = row["CASEACT_TRIG_CODE"].ToString().Trim();
                //        ACT_TrigDate = row["CASEACT_TRIG_DATE"].ToString().Trim();
                //        ACT_TrigDateSeq = row["CASEACT_TRIG_DATE_SEQ"].ToString().Trim();
                //    }

                //    if (strType == "CASEACT2.0")
                //    {
                //        Amount2 = row["CASEACT_AMOUNT2"].ToString().Trim();
                //        Amount3 = row["CASEACT_AMOUNT3"].ToString().Trim();
                //        Units2 = row["CASEACT_UNITS2"].ToString();
                //        UOM2 = row["CASEACT_UOM2"].ToString();
                //        Units3 = row["CASEACT_UNITS3"].ToString();
                //        UOM3 = row["CASEACT_UOM3"].ToString();
                //    }

                //    if (strType == "PAYMENT")
                //    {
                //        Amount2 = row["CASEACT_AMOUNT2"].ToString().Trim();
                //        Amount3 = row["CASEACT_AMOUNT3"].ToString().Trim();
                //        Units2 = row["CASEACT_UNITS2"].ToString();
                //        UOM2 = row["CASEACT_UOM2"].ToString();
                //        Units3 = row["CASEACT_UNITS3"].ToString();
                //        UOM3 = row["CASEACT_UOM3"].ToString();

                //        BillingPeriod = row["CASEACT_BILL_PERIOD"].ToString().Trim();
                //        Account = row["CASEACT_VEND_ACCT"].ToString().Trim();
                //        ArrearsAmt = row["CASEACT_ARREARS"].ToString();
                //        LVL1Apprval = row["CASEACT_LVL1_APRVL"].ToString();

                //        LVL1AprrvalDate = row["CASEACT_LVL1_APRVL_DATE"].ToString();
                //        LVL2Apprval = row["CASEACT_LVL2_APRVL"].ToString();
                //        LVL2ApprvalDate = row["CASEACT_LVL2_APRVL_DATE"].ToString();
                //        SentPmtUser = row["CASEACT_SENT_PMT_USER"].ToString();
                //        SentPmtDate = row["CASEACT_SENT_PMT_DATE"].ToString();
                //        BundleNo = row["CASEACT_BUNDLE_NO"].ToString();
                //    }
                //}
            }
        }




        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App_no { get; set; }
        public string Service_plan { get; set; }
        public string SPM_Seq { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        //public string Type { get; set; }
        public string ACT_Code { get; set; }
        public string ACT_ID { get; set; }
        public string ACT_Date { get; set; }
        public string ACT_Seq { get; set; }
        public string Site { get; set; }
        public string Fund1 { get; set; }
        public string Fund2 { get; set; }
        public string Fund3 { get; set; }
        public string Caseworker { get; set; }
        public string Vendor_No { get; set; }
        public string Check_Date { get; set; }
        public string Check_No { get; set; }


        public string Cost { get; set; }
        public string Followup_On { get; set; }
        public string Followup_Comp { get; set; }
        public string Followup_By { get; set; }
        public string Refer_Data { get; set; }
        public string Cust_Code1 { get; set; }
        public string Cust_Value1 { get; set; }
        public string Cust_Code2 { get; set; }
        public string Cust_Value2 { get; set; }
        public string Cust_Code3 { get; set; }
        public string Cust_Value3 { get; set; }
        public string Cust_Code4 { get; set; }
        public string Cust_Value4 { get; set; }
        public string Cust_Code5 { get; set; }
        public string Cust_Value5 { get; set; }
        public string Bulk { get; set; }
        public string Act_PROG { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Notes_Count { get; set; }
        public string Units { get; set; }
        public string UOM { get; set; }
        public string VOUCHNO { get; set; }
        public string Curr_Grp { get; set; }
        public string CaseNotesData { get; set; }
        public string Sp2_RowOrder { get; set; }
        public string ACT_TrigCode { get; set; }
        public string ACT_TrigDate { get; set; }
        public string ACT_TrigDateSeq { get; set; }
        public string ActSeek_Date { get; set; }
        public string CA_OBF { get; set; }

        public string Amount2 { get; set; }
        public string Amount3 { get; set; }
        public string Units2 { get; set; }
        public string UOM2 { get; set; }
        public string Units3 { get; set; }
        public string UOM3 { get; set; }

        public string BillingPeriod { get; set; }
        public string Account { get; set; }
        public string ArrearsAmt { get; set; }
        public string LVL1Apprval { get; set; }
        public string LVL1AprrvalDate { get; set; }
        public string LVL2Apprval { get; set; }
        public string LVL2ApprvalDate { get; set; }
        public string SentPmtUser { get; set; }
        public string SentPmtDate { get; set; }
        public string BundleNo { get; set; }

        public string Vend_Addr1 { get; set; }
        public string Vend_Addr2 { get; set; }
        public string Vend_Addr3 { get; set; }
        public string Vdd_City { get; set; }
        public string Vdd_State { get; set; }
        public string Vdd_Zip { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MstSite { get; set; }
        public string VendorName { get; set; }
        public string RejectCode { get; set; }
        public string RejectDate { get; set; }

        #endregion
    }


    public class CASEMSEntity
    {
        #region Constructors

        public CASEMSEntity()
        {
            Rec_Type =
            Agency =
            Dept =
            Program =
            Year =
            App_no =
            Service_plan =
            SPM_Seq =
            Branch =
            Group =
            MS_Code =
            ID =
            Date =
            CaseWorker =
            Site =
            Result =
            OBF =
            Bulk =
            Lstc_Date =
            Lsct_Operator =
            Add_Date =
            Add_Operator =

            Notes_Count =
            Acty_PROG =
            Curr_Grp =
            MS_TrigCode =
            MS_TrigDate =
            MS_TrigDateSeq =
            MS_Fund1 =
            MS_Fund2 =
            MS_Fund3 =
            MS_FUP_Date =
            MS_Comp_Date =
            MS_Amount =
            IsSave= IsTemplate=
            Seek_Date = string.Empty;
        }

        public CASEMSEntity(bool Intialize)
        {
            if (Intialize)
            {
                Rec_Type =
                Agency =
                Dept =
                Program =
                Year =
                App_no =
                Service_plan =
                SPM_Seq =
                Branch =
                Group =
                MS_Code =
                ID =
                Date =
                CaseWorker =
                Site =
                Result =
                OBF =
                Bulk =
                Lstc_Date =
                Lsct_Operator =
                Add_Date =
                Add_Operator =
                Notes_Count =
                Acty_PROG =
                Curr_Grp =
                MS_TrigCode =
                MS_TrigDate =
                MS_TrigDateSeq =
                MS_Fund1 =
                MS_Fund2 =
                MS_Fund3 =
                MS_FUP_Date =
                MS_Comp_Date =
                MS_Amount =
                IsSave = IsTemplate= 
                Seek_Date = null;
            }
        }

        public CASEMSEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;
                Rec_Type = "U";
                Agency = row["CASEMS_AGENCY"].ToString().Trim();
                Dept = row["CASEMS_DEPT"].ToString().Trim();
                Program = row["CASEMS_PROGRAM"].ToString().Trim();
                Year = row["CASEMS_YEAR"].ToString();
                App_no = row["CASEMS_APP_NO"].ToString().Trim();
                Service_plan = row["CASEMS_SERVICE_PLAN"].ToString().Trim();
                SPM_Seq = row["CASEMS_SPM_SEQ"].ToString().Trim();
                Branch = row["CASEMS_BRANCH"].ToString().Trim();
                Group = row["CASEMS_GROUP"].ToString().Trim();
                MS_Code = row["CASEMS_MS_CODE"].ToString().Trim();
                ID = row["CASEMS_ID"].ToString().Trim();
                Date = row["CASEMS_DATE"].ToString().Trim();
                CaseWorker = row["CASEMS_CASEWORKER"].ToString().Trim();
                Site = row["CASEMS_SITE"].ToString().Trim();
                Result = row["CASEMS_RESULT"].ToString().Trim();
                OBF = row["CASEMS_OBF"].ToString().Trim();
                Bulk = row["CASEMS_BULK"].ToString().Trim();
                Lstc_Date = row["CASEMS_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEMS_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASEMS_DATE_ADD"].ToString();
                Add_Operator = row["CASEMS_ADD_OPERATOR"].ToString();
                Notes_Count = row["NOTES_COUNT"].ToString();
                Acty_PROG = row["CASEMS_ACTY_PROG"].ToString();
                Curr_Grp = row["CASEMS_CURR_GRP"].ToString();
                Seek_Date = row["CASEMS_SEEK_DATE"].ToString();

                MS_Fund1 = row["CASEMS_FUND1"].ToString();
                MS_Fund2 = row["CASEMS_FUND2"].ToString();
                MS_Fund3 = row["CASEMS_FUND3"].ToString();
                MS_FUP_Date = row["CASEMS_FUP_DATE"].ToString();
                MS_Comp_Date = row["CASEMS_COMP_DATE"].ToString();
                MS_Amount= row["CASEMS_AMOUNT"].ToString();
            }
        }

        public CASEMSEntity(CASEMSEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App_no = Entity.App_no;
                Service_plan = Entity.Service_plan;
                SPM_Seq = Entity.SPM_Seq;
                Branch = Entity.Branch;
                Group = Entity.Group;
                MS_Code = Entity.MS_Code;
                ID = Entity.ID;
                Date = Entity.Date;
                CaseWorker = Entity.CaseWorker;
                Site = Entity.Site;
                Result = Entity.Result;
                OBF = Entity.OBF;
                Bulk = Entity.Bulk;
                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                Notes_Count = Entity.Notes_Count;
                Acty_PROG = Entity.Acty_PROG;
                Curr_Grp = Entity.Curr_Grp;
                Seek_Date = Entity.Seek_Date;

                MS_TrigCode = Entity.MS_TrigCode;
                MS_TrigDate = Entity.MS_TrigDate;
                MS_TrigDateSeq = Entity.MS_TrigDateSeq;

                MS_Fund1 = Entity.MS_Fund1;
                MS_Fund2 = Entity.MS_Fund2;
                MS_Fund3 = Entity.MS_Fund3;
                MS_FUP_Date = Entity.MS_FUP_Date;
                MS_Comp_Date = Entity.MS_Comp_Date;
                MS_Amount = Entity.MS_Amount;

                MSDesc = Entity.MSDesc;
            }
        }


        public CASEMSEntity(DataRow CASEMS, string strType)
        {
            if (CASEMS != null)
            {
                DataRow row = CASEMS;
                Rec_Type = "U";
                Agency = row["CASEMS_AGENCY"].ToString().Trim();
                Dept = row["CASEMS_DEPT"].ToString().Trim();
                Program = row["CASEMS_PROGRAM"].ToString().Trim();
                Year = row["CASEMS_YEAR"].ToString();
                App_no = row["CASEMS_APP_NO"].ToString().Trim();
                Service_plan = row["CASEMS_SERVICE_PLAN"].ToString().Trim();
                SPM_Seq = row["CASEMS_SPM_SEQ"].ToString().Trim();
                Branch = row["CASEMS_BRANCH"].ToString().Trim();
                Group = row["CASEMS_GROUP"].ToString().Trim();
                MS_Code = row["CASEMS_MS_CODE"].ToString().Trim();
                ID = row["CASEMS_ID"].ToString().Trim();
                Date = row["CASEMS_DATE"].ToString().Trim();
                CaseWorker = row["CASEMS_CASEWORKER"].ToString().Trim();
                Site = row["CASEMS_SITE"].ToString().Trim();
                Result = row["CASEMS_RESULT"].ToString().Trim();
                OBF = row["CASEMS_OBF"].ToString().Trim();
                Bulk = row["CASEMS_BULK"].ToString().Trim();
                Lstc_Date = row["CASEMS_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEMS_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASEMS_DATE_ADD"].ToString();
                Add_Operator = row["CASEMS_ADD_OPERATOR"].ToString();
                Curr_Grp = row["CASEMS_CURR_GRP"].ToString();
                Seek_Date = row["CASEMS_SEEK_DATE"].ToString();

                MS_Fund1 = row["CASEMS_FUND1"].ToString();
                MS_Fund2 = row["CASEMS_FUND2"].ToString();
                MS_Fund3 = row["CASEMS_FUND3"].ToString();
                MS_FUP_Date = row["CASEMS_FUP_DATE"].ToString();
                MS_Comp_Date = row["CASEMS_COMP_DATE"].ToString();
                MS_Amount = row["CASEMS_AMOUNT"].ToString();

                if (strType == "Trigger")
                {
                    MS_TrigCode = row["CASEMS_TRIG_CODE"].ToString().Trim();
                    MS_TrigDate = row["CASEMS_TRIG_DATE"].ToString().Trim();
                    MS_TrigDateSeq = row["CASEMS_TRIG_DATE_SEQ"].ToString().Trim();
                }
            }
        }

        public CASEMSEntity(DataRow CASEMS, string strType, string strTable)
        {
            if (CASEMS != null)
            {
                DataRow row = CASEMS;
                if (strTable == "SSBG")
                {
                    Agency = row["CASEMS_AGENCY"].ToString().Trim();
                    Dept = row["CASEMS_DEPT"].ToString().Trim();
                    Program = row["CASEMS_PROGRAM"].ToString().Trim();
                    Year = row["CASEMS_YEAR"].ToString();
                    App_no = row["CASEMS_APP_NO"].ToString().Trim();
                    Service_plan = row["CASEMS_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEMS_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEMS_BRANCH"].ToString().Trim();
                    Group = row["CASEMS_GROUP"].ToString().Trim();
                    MS_Code = row["CASEMS_MS_CODE"].ToString().Trim();
                    //ID = row["CASEMS_ID"].ToString().Trim();
                    Date = row["CASEMS_DATE"].ToString().Trim();
                    Seek_Date = row["CASEMS_SEEK_DATE"].ToString();

                    FirstName = row["SNP_NAME_IX_FI"].ToString().Trim();
                    LastName = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    MiddleName = row["SNP_NAME_IX_MI"].ToString().Trim();
                }
                else
                {
                    Rec_Type = "U";
                    Agency = row["CASEMS_AGENCY"].ToString().Trim();
                    Dept = row["CASEMS_DEPT"].ToString().Trim();
                    Program = row["CASEMS_PROGRAM"].ToString().Trim();
                    Year = row["CASEMS_YEAR"].ToString();
                    App_no = row["CASEMS_APP_NO"].ToString().Trim();
                    Service_plan = row["CASEMS_SERVICE_PLAN"].ToString().Trim();
                    SPM_Seq = row["CASEMS_SPM_SEQ"].ToString().Trim();
                    Branch = row["CASEMS_BRANCH"].ToString().Trim();
                    Group = row["CASEMS_GROUP"].ToString().Trim();
                    MS_Code = row["CASEMS_MS_CODE"].ToString().Trim();
                    ID = row["CASEMS_ID"].ToString().Trim();
                    Date = row["CASEMS_DATE"].ToString().Trim();
                    CaseWorker = row["CASEMS_CASEWORKER"].ToString().Trim();
                    Site = row["CASEMS_SITE"].ToString().Trim();
                    Result = row["CASEMS_RESULT"].ToString().Trim();
                    OBF = row["CASEMS_OBF"].ToString().Trim();
                    Bulk = row["CASEMS_BULK"].ToString().Trim();
                    Lstc_Date = row["CASEMS_DATE_LSTC"].ToString();
                    Lsct_Operator = row["CASEMS_LSTC_OPERATOR"].ToString();
                    Add_Date = row["CASEMS_DATE_ADD"].ToString();
                    Add_Operator = row["CASEMS_ADD_OPERATOR"].ToString();
                    Curr_Grp = row["CASEMS_CURR_GRP"].ToString();
                    //Seek_Date = row["CASEMS_SEEK_DATE"].ToString();

                    MS_Fund1 = row["CASEMS_FUND1"].ToString();
                    MS_Fund2 = row["CASEMS_FUND2"].ToString();
                    MS_Fund3 = row["CASEMS_FUND3"].ToString();

                    MS_FUP_Date = row["CASEMS_FUP_DATE"].ToString();
                    MS_Comp_Date = row["CASEMS_COMP_DATE"].ToString();
                    MS_Amount = row["CASEMS_AMOUNT"].ToString();
                }

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App_no { get; set; }
        public string Service_plan { get; set; }
        public string SPM_Seq { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string MS_Code { get; set; }
        public string ID { get; set; }
        public string Date { get; set; }
        public string CaseWorker { get; set; }
        public string Site { get; set; }
        public string Result { get; set; }
        public string OBF { get; set; }
        public string Bulk { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Notes_Count { get; set; }
        public string Acty_PROG { get; set; }
        public string VOUCHNO { get; set; }
        public string Curr_Grp { get; set; }
        public string Seek_Date { get; set; }

        public string MS_TrigCode { get; set; }
        public string MS_TrigDate { get; set; }
        public string MS_TrigDateSeq { get; set; }

        public string MS_Fund1 { get; set; }
        public string MS_Fund2 { get; set; }
        public string MS_Fund3 { get; set; }
        public string MS_FUP_Date { get; set; }
        public string MS_Comp_Date { get; set; }

        public string MS_Amount { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string IsSave { get; set; }

        public string IsTemplate { get; set; }

        public string MSDesc { get; set; }
        #endregion
    }


    public class CASECONTEntity
    {
        #region Constructors

        public CASECONTEntity()
        {
            Rec_Type = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App_no = string.Empty;
            Seq = string.Empty;
            Contact_No = string.Empty;
            Contact_Name = string.Empty;
            CaseWorker = string.Empty;
            Cont_Date = string.Empty;
            Duration_Type = string.Empty;
            Time = string.Empty;
            Time_Starts = string.Empty;
            Time_Ends = string.Empty;
            Duration = string.Empty;
            //Duration_Min = string.Empty;
            How_Where = string.Empty;
            Language = string.Empty;
            Interpreter = string.Empty;
            Refer_From = string.Empty;
            BillTO = string.Empty;
            BillTo_UOM = string.Empty;
            Cust1_Code = string.Empty;
            Cust1_Value = string.Empty;
            Cust2_Code = string.Empty;
            Cust2_Value = string.Empty;
            Cust3_Code = string.Empty;
            Cust3_Value = string.Empty;
            Bridge = string.Empty;
            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            Notes_Count = string.Empty;
            Cont_Program = string.Empty;

            Contact_ID = string.Empty;
            FollowuponDate = string.Empty;
            FollowupCompleteDate = string.Empty;

        }

        public CASECONTEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Agency = row["CASECONT_AGENCY"].ToString();
                Dept = row["CASECONT_DEPT"].ToString();
                Program = row["CASECONT_PROGRAM"].ToString();
                Year = row["CASECONT_YEAR"].ToString();
                App_no = row["CASECONT_APP_NO"].ToString();
                Seq = row["CASECONT_SEQ"].ToString();

                Contact_No = row["CASECONT_CONTACT_NO"].ToString();
                Contact_Name = row["CASECONT_CONTACT_NAME"].ToString();
                CaseWorker = row["CASECONT_CASEWORKER"].ToString().Trim();
                Cont_Date = row["CASECONT_DATE"].ToString();
                Duration_Type = row["CASECONT_DURATION_TYPE"].ToString();
                Time = row["CASECONT_TIME"].ToString();
                Time_Starts = row["CASECONT_STARTS"].ToString();
                Time_Ends = row["CASECONT_END"].ToString();
                Duration = row["CASECONT_DURATION"].ToString();
                //Duration_Min = row["CASECONT_DURATION_MN"].ToString();
                How_Where = row["CASECONT_HOW_WHERE"].ToString();
                Language = row["CASECONT_INTER_LANG"].ToString().Trim();
                Interpreter = row["CASECONT_INTERPRETER"].ToString().Trim();
                Refer_From = row["CASECONT_REF_FROM_CODE"].ToString();

                BillTO = row["CASECONT_BILLTO_CODE"].ToString();
                BillTo_UOM = row["CASECONT_BILLTO_UOM"].ToString();
                Cust1_Code = row["CASECONT_CUST1_CODE"].ToString();
                Cust1_Value = row["CASECONT_CUST1_VALUE"].ToString();
                Cust2_Code = row["CASECONT_CUST2_CODE"].ToString();
                Cust2_Value = row["CASECONT_CUST2_VALUE"].ToString();
                Cust3_Code = row["CASECONT_CUST3_CODE"].ToString();
                Cust3_Value = row["CASECONT_CUST3_VALUE"].ToString();
                Bridge = row["CASECONT_BRIDGE"].ToString();

                Lstc_Date = row["CASECONT_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASECONT_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASECONT_DATE_ADD"].ToString();
                Add_Operator = row["CASECONT_ADD_OPERATOR"].ToString();

                Notes_Count = row["NOTES_COUNT"].ToString();
                Cont_Program = row["CASECONT_CONTPROG"].ToString();

                Contact_ID = row["CASECONT_ID"].ToString();
                FollowuponDate = row["CASECONT_FUP_DATE"].ToString();
                FollowupCompleteDate = row["CASECONT_COMP_DATE"].ToString();

            }
        }

        public CASECONTEntity(CASECONTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App_no = Entity.App_no;

                Seq = Entity.Seq;

                Contact_No = Entity.Contact_No;
                Contact_Name = Entity.Contact_Name;
                CaseWorker = Entity.CaseWorker;
                Cont_Date = Entity.Cont_Date;
                Duration_Type = Entity.Duration_Type;
                Time = Entity.Time;
                Time_Starts = Entity.Time_Starts;
                Time_Ends = Entity.Time_Ends;
                Duration = Entity.Duration;
                //Duration_Min = Entity.Duration_Min;
                How_Where = Entity.How_Where;
                Language = Entity.Language;
                Interpreter = Entity.Interpreter;
                Refer_From = Entity.Refer_From;

                BillTO = Entity.BillTO;
                BillTo_UOM = Entity.BillTo_UOM;
                Cust1_Code = Entity.Cust1_Code;
                Cust1_Value = Entity.Cust1_Value;
                Cust2_Code = Entity.Cust2_Code;
                Cust2_Value = Entity.Cust2_Value;
                Cust3_Code = Entity.Cust3_Code;
                Cust3_Value = Entity.Cust3_Value;
                Bridge = Entity.Bridge;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                Notes_Count = Entity.Notes_Count;
                Cont_Program = Entity.Cont_Program;

                Contact_ID = Entity.Contact_ID;
            }
        }

        public CASECONTEntity(DataRow CASESPM, string strType)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Agency = row["CASECONT_AGENCY"].ToString();
                Dept = row["CASECONT_DEPT"].ToString();
                Program = row["CASECONT_PROGRAM"].ToString();
                Year = row["CASECONT_YEAR"].ToString();
                App_no = row["CASECONT_APP_NO"].ToString();
                Seq = row["CASECONT_SEQ"].ToString();
                Contact_No = row["CASECONT_CONTACT_NO"].ToString();
                Contact_Name = row["CASECONT_CONTACT_NAME"].ToString();
                CaseWorker = row["CASECONT_CASEWORKER"].ToString().Trim();
                Cont_Date = row["CASECONT_DATE"].ToString();
                Lstc_Date = row["CASECONT_DATE_LSTC"].ToString();
                How_Where = row["CASECONT_HOW_WHERE"].ToString();
                Contact_ID = row["CASECONT_ID"].ToString();

            }
        }

        public CASECONTEntity(DataRow CASESPM, string strtype, string strMode)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Agency = row["CASECONT_AGENCY"].ToString();
                Dept = row["CASECONT_DEPT"].ToString();
                Program = row["CASECONT_PROGRAM"].ToString();
                Year = row["CASECONT_YEAR"].ToString();
                App_no = row["CASECONT_APP_NO"].ToString();
                Seq = row["CASECONT_SEQ"].ToString();

                Contact_No = row["CASECONT_CONTACT_NO"].ToString();
                Contact_Name = row["CASECONT_CONTACT_NAME"].ToString();
                CaseWorker = row["CASECONT_CASEWORKER"].ToString().Trim();
                Cont_Date = row["CASECONT_DATE"].ToString();
                Duration_Type = row["CASECONT_DURATION_TYPE"].ToString();
                Time = row["CASECONT_TIME"].ToString();
                Time_Starts = row["CASECONT_STARTS"].ToString();
                Time_Ends = row["CASECONT_END"].ToString();
                Duration = row["CASECONT_DURATION"].ToString();
                //Duration_Min = row["CASECONT_DURATION_MN"].ToString();
                How_Where = row["CASECONT_HOW_WHERE"].ToString();
                Language = row["CASECONT_INTER_LANG"].ToString().Trim();
                Interpreter = row["CASECONT_INTERPRETER"].ToString().Trim();
                Refer_From = row["CASECONT_REF_FROM_CODE"].ToString();

                BillTO = row["CASECONT_BILLTO_CODE"].ToString();
                BillTo_UOM = row["CASECONT_BILLTO_UOM"].ToString();
                Cust1_Code = row["CASECONT_CUST1_CODE"].ToString();
                Cust1_Value = row["CASECONT_CUST1_VALUE"].ToString();
                Cust2_Code = row["CASECONT_CUST2_CODE"].ToString();
                Cust2_Value = row["CASECONT_CUST2_VALUE"].ToString();
                Cust3_Code = row["CASECONT_CUST3_CODE"].ToString();
                Cust3_Value = row["CASECONT_CUST3_VALUE"].ToString();
                Bridge = row["CASECONT_BRIDGE"].ToString();

                Lstc_Date = row["CASECONT_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASECONT_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASECONT_DATE_ADD"].ToString();
                Add_Operator = row["CASECONT_ADD_OPERATOR"].ToString();

                Notes_Count = row["NOTES_COUNT"].ToString();
                Cont_Program = row["CASECONT_CONTPROG"].ToString();

                //Contact_ID = row["CASECONT_ID"].ToString();

            }
        }



        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App_no { get; set; }
        public string Seq { get; set; }

        public string Contact_ID { get; set; }

        public string Contact_No { get; set; }
        public string Contact_Name { get; set; }
        public string CaseWorker { get; set; }
        public string Cont_Date { get; set; }
        public string Duration_Type { get; set; }
        public string ACT_Date { get; set; }
        public string ACT_Seq { get; set; }
        public string Time { get; set; }
        public string Time_Starts { get; set; }
        public string Time_Ends { get; set; }
        public string Duration { get; set; }
        //public string Duration_Min { get; set; }
        public string How_Where { get; set; }
        public string Language { get; set; }
        public string Interpreter { get; set; }


        public string Refer_From { get; set; }
        public string BillTO { get; set; }
        public string BillTo_UOM { get; set; }
        public string Cust1_Code { get; set; }
        public string Cust1_Value { get; set; }
        public string Cust2_Code { get; set; }
        public string Cust2_Value { get; set; }
        public string Cust3_Code { get; set; }
        public string Cust3_Value { get; set; }
        public string Bridge { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Notes_Count { get; set; }
        public string Cont_Program { get; set; }
        public string FollowuponDate { get; set; }
        public string FollowupCompleteDate { get; set; }

        #endregion
    }


    public class CASEMSOBOEntity
    {
        #region Constructors

        public CASEMSOBOEntity()
        {
            Rec_Type = string.Empty;
            ID = string.Empty;
            Seq = string.Empty;
            CLID = string.Empty;
            Fam_Seq = string.Empty;
        }

        public CASEMSOBOEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                ID = row["CASEMSOBO_ID"].ToString();
                Seq = row["CASEMSOBO_SEQ"].ToString();
                CLID = row["CASEMSOBO_CLID"].ToString();
                Fam_Seq = row["CASEMSOBO_FAM_SEQ"].ToString();
            }
        }

        public CASEMSOBOEntity(CASEMSOBOEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                ID = Entity.ID;
                Seq = Entity.Seq;
                CLID = Entity.CLID;
                Fam_Seq = Entity.Fam_Seq;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string ID { get; set; }
        public string Seq { get; set; }
        public string CLID { get; set; }
        public string Fam_Seq { get; set; }

        #endregion
    }


    public class CAOBOEntity
    {
        #region Constructors

        public CAOBOEntity()
        {
            Rec_Type = string.Empty;
            ID = string.Empty;
            Seq = string.Empty;
            CLID = string.Empty;
            Fam_Seq = string.Empty;
            Amount = string.Empty;
            Desc = string.Empty;
            SchoolDistrict = string.Empty;
            SchoolGrade = string.Empty;
            Status = string.Empty;
            CompltdDate = string.Empty;
            TransUnits = string.Empty;
            RecipentName = string.Empty;
            Gift1 = string.Empty;
            Gift2 = string.Empty;
            Gift3 = string.Empty;
            GiftCard = string.Empty;
            BedSize = string.Empty;
            AirMattress = string.Empty;
            TransUOM = string.Empty;

            ClothSize = string.Empty;
            ShoeSize = string.Empty;
            Quantity = string.Empty;
            UnitPrice = string.Empty;

            Code = string.Empty;
            Branch = string.Empty;
            Group = string.Empty;
            Type = string.Empty;
            BenefitFrom = string.Empty;
        }

        public CAOBOEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                ID = row["CAOBO_ID"].ToString();
                Seq = row["CAOBO_SEQ"].ToString();
                CLID = row["CAOBO_CLID"].ToString();
                Fam_Seq = row["CAOBO_FAM_SEQ"].ToString();
                Amount = row["CAOBO_AMOUNT"].ToString();
                Desc = row["CAOBO_DESC"].ToString();
                SchoolGrade = row["CAOBO_SGRADE"].ToString();
                SchoolDistrict = row["CAOBO_SDISTRICT"].ToString();
                Status = row["CAOBO_STATUS"].ToString();
                CompltdDate = row["CAOBO_COMPDATE"].ToString();
                TransUnits = row["CAOBO_TRANSUNITS"].ToString();
                RecipentName = row["CAOBO_RECPINAME"].ToString();
                Gift1 = row["CAOBO_GIFT1"].ToString();
                Gift2 = row["CAOBO_GIFT2"].ToString();
                Gift3 = row["CAOBO_GIFT3"].ToString();
                GiftCard = row["CAOBO_GIFTCARD"].ToString();
                BedSize = row["CAOBO_BEDSIZE"].ToString();
                AirMattress = row["CAOBO_AIRMATTRESS"].ToString();
                TransUOM = row["CAOBO_TRANSUOM"].ToString();

                ClothSize = row["CAOBO_CLOTHSIZE"].ToString();
                ShoeSize = row["CAOBO_SHOESIZE"].ToString();
                Quantity = row["CAOBO_QUANTITY"].ToString();
                UnitPrice = row["CAOBO_UNITPRICE"].ToString();

            }
        }

        public CAOBOEntity(CAOBOEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                ID = Entity.ID;
                Seq = Entity.Seq;
                CLID = Entity.CLID;
                Fam_Seq = Entity.Fam_Seq;
                Amount = Entity.Amount;
                Desc = Entity.Desc;
                SchoolDistrict = Entity.SchoolGrade;
                SchoolGrade = Entity.SchoolGrade;
                Status = Entity.Status;
                CompltdDate = Entity.CompltdDate;
                TransUnits = Entity.TransUnits;
                RecipentName = Entity.RecipentName;
                Gift1 = Entity.Gift1;
                Gift2 = Entity.Gift2;
                Gift3 = Entity.Gift3;
                GiftCard = Entity.GiftCard;
                BedSize = Entity.BedSize;
                AirMattress = Entity.AirMattress;
                TransUOM = Entity.TransUOM;

                ClothSize = Entity.ClothSize;
                ShoeSize = Entity.ShoeSize;
                Quantity = Entity.Quantity;
                UnitPrice = Entity.UnitPrice;

                Code = Entity.Code;
                Branch = Entity.Branch;
                Group = Entity.Group;
                Type = Entity.Type;
                BenefitFrom = Entity.BenefitFrom;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string ID { get; set; }
        public string Seq { get; set; }
        public string CLID { get; set; }
        public string Fam_Seq { get; set; }
        public string Amount { get; set; }
        public string Desc { get; set; }
        public string SchoolGrade { get; set; }
        public string SchoolDistrict { get; set; }
        public string Status { get; set; }
        public string CompltdDate { get; set; }
        public string TransUnits { get; set; }
        public string RecipentName { get; set; }
        public string Gift1 { get; set; }
        public string Gift2 { get; set; }
        public string Gift3 { get; set; }
        public string GiftCard { get; set; }
        public string BedSize { get; set; }
        public string AirMattress { get; set; }
        public string TransUOM { get; set; }

        public string ClothSize { get; set; }
        public string ShoeSize { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }

        public string Code { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string BenefitFrom { get; set; }


        #endregion
    }

    public class CASEREFEntity
    {
        #region Constructors

        public CASEREFEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            Name1 = string.Empty;
            Name2 = string.Empty;
            IndexBy = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Zip_Plus = string.Empty;
            Area = string.Empty;
            Excgange = string.Empty;
            Telno = string.Empty;
            Active = string.Empty;
            Cont_Fname = string.Empty;
            Cont_Lname = string.Empty;
            Cont_Area = string.Empty;
            Cont_Exchange = string.Empty;
            Cont_TelNO = string.Empty;
            Fax_Area = string.Empty;
            NameIndex = string.Empty;

            Long_Distance = string.Empty;
            Fax_Exchange = string.Empty;
            Fax_Telno = string.Empty;
            Outside = string.Empty;
            Category = string.Empty;
            County = string.Empty;
            From_Hrs = string.Empty;
            To_Hrs = string.Empty;
            Sec = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            Email = string.Empty;
            WebAddress = string.Empty;
        }

        public CASEREFEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                Name1 = null;
                Name2 = null;
                IndexBy = null;
                Street = null;
                City = null;
                State = null;
                Zip = null;
                Zip_Plus = null;
                Area = null;
                Excgange = null;
                Telno = null;
                Active = null;
                Cont_Fname = null;
                Cont_Lname = null;
                Cont_Area = null;
                Cont_Exchange = null;
                Cont_TelNO = null;
                Fax_Area = null;
                NameIndex = null;

                Long_Distance = null;
                Fax_Exchange = null;
                Fax_Telno = null;
                Outside = null;
                Category = null;
                County = null;
                From_Hrs = null;
                To_Hrs = null;
                Sec = null;

                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;

                Email = null;
                WebAddress = null;
            }

        }


        public CASEREFEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Code = row["CASEREF_CODE"].ToString();
                Name1 = row["CASEREF_NAME1"].ToString();
                Name2 = row["CASEREF_NAME2"].ToString();
                IndexBy = row["CASEREF_INDEXBY"].ToString();
                Street = row["CASEREF_STREET"].ToString();
                City = row["CASEREF_CITY"].ToString();
                State = row["CASEREF_STATE"].ToString();
                Zip = row["CASEREF_ZIP"].ToString();
                Zip_Plus = row["CASEREF_ZIP_PLUS"].ToString();
                //Area = row["CASEREF_AREA"].ToString();
                //Excgange = row["CASEREF_EXCHANGE"].ToString();
                Telno = row["CASEREF_TELNO"].ToString();
                Active = row["CASEREF_ACTIVE"].ToString();
                Cont_Fname = row["CASEREF_CONT_FNAME"].ToString();
                Cont_Lname = row["CASEREF_CONT_LNAME"].ToString();
                //Cont_Area = row["CASEREF_CONT_AREA"].ToString();
                //Cont_Exchange = row["CASEREF_CONT_EXCHANGE"].ToString();
                Cont_TelNO = row["CASEREF_CONT_TELNO"].ToString();
                //Fax_Area = row["CASEREF_FAX_AREA"].ToString();
                NameIndex = row["CASEREF_CONT_NAMEINDEX"].ToString();

                Long_Distance = row["CASEREF_LONG_DISTANCE"].ToString();
                //Fax_Exchange = row["CASEREF_FAX_EXCHANGE"].ToString();
                Fax_Telno = row["CASEREF_FAX_TELNO"].ToString();
                Outside = row["CASEREF_OUTSIDE"].ToString();
                Category = row["CASEREF_CATEGORY"].ToString();
                County = row["CASEREF_COUNTY"].ToString();
                From_Hrs = row["CASEREF_FROM_HRS"].ToString();
                To_Hrs = row["CASEREF_TO_HRS"].ToString();
                Sec = row["CASEREF_SEC"].ToString();

                Lstc_Date = row["CASEREF_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEREF_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASEREF_DATE_ADD"].ToString();
                Add_Operator = row["CASEREF_ADD_OPERATOR"].ToString();

                Email = row["CASEREF_EMAIL"].ToString();
                WebAddress = row["CASEREF_WEB_ADDRESS"].ToString();
            }
        }

        public CASEREFEntity(CASEREFEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Name1 = Entity.Name1;
                Name2 = Entity.Name2;
                IndexBy = Entity.IndexBy;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                Zip_Plus = Entity.Zip_Plus;
                Area = Entity.Area;
                Excgange = Entity.Excgange;
                Telno = Entity.Telno;
                Active = Entity.Active;
                Cont_Fname = Entity.Cont_Fname;
                Cont_Lname = Entity.Cont_Lname;
                Cont_Area = Entity.Cont_Area;
                Cont_Exchange = Entity.Cont_Exchange;
                Cont_TelNO = Entity.Cont_TelNO;
                Fax_Area = Entity.Fax_Area;
                NameIndex = Entity.NameIndex;

                Long_Distance = Entity.Long_Distance;
                Fax_Exchange = Entity.Fax_Exchange;
                Fax_Telno = Entity.Fax_Telno;
                Outside = Entity.Outside;
                Category = Entity.Category;
                County = Entity.County;
                From_Hrs = Entity.From_Hrs;
                To_Hrs = Entity.To_Hrs;
                Sec = Entity.Sec;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                Email = Entity.Email;
                WebAddress = Entity.WebAddress;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string IndexBy { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Zip_Plus { get; set; }
        public string Area { get; set; }
        public string Excgange { get; set; }
        public string Telno { get; set; }
        public string Active { get; set; }
        public string Cont_Fname { get; set; }
        public string Cont_Lname { get; set; }
        public string Cont_Area { get; set; }
        public string Cont_Exchange { get; set; }
        public string Cont_TelNO { get; set; }
        public string Long_Distance { get; set; }
        public string Fax_Exchange { get; set; }
        public string Fax_Telno { get; set; }
        public string Outside { get; set; }
        public string Fax_Area { get; set; }
        public string NameIndex { get; set; }


        public string Category { get; set; }
        public string County { get; set; }
        public string From_Hrs { get; set; }
        public string To_Hrs { get; set; }
        public string Sec { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        public string Email { get; set; }

        public string WebAddress { get; set; }

        #endregion
    }

    public class CASEREFSEntity
    {
        #region Constructors

        public CASEREFSEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            //Seq = string.Empty;
            Service = string.Empty;
        }

        public CASEREFSEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                //Seq = null;
                Service = null;
                //Sel_Count = 0;
            }
        }

        public CASEREFSEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "I";
                Code = row["CASEREFS_CODE"].ToString();
                // Seq = row["CASEREFS_SEQ"].ToString();
                Service = row["CASEREFS_SERVICE"].ToString();
            }
        }

        public CASEREFSEntity(CASEREFSEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                // Seq = Entity.Seq;
                Service = Entity.Service;
                //Sel_Count = Entity.Sel_Count;
            }
        }

        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        //public string Seq { get; set; }
        public string Service { get; set; }
        //public int Sel_Count { get; set; }

        #endregion
    }


    public class ACTREFSEntity
    {
        #region Constructors

        public ACTREFSEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            ApplNo = string.Empty;
            Date = string.Empty;
            Type = string.Empty;
            Connected = string.Empty;
            Lsct_Operator = string.Empty;
            Lstc_Date = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            NameIndex = string.Empty;
        }

        public ACTREFSEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                Agency = null;
                Dept = null;
                Program = null;
                Year = null;
                ApplNo = null;
                Date = null;
                Type = null;
                Connected = null;
                Lsct_Operator = null;
                Lstc_Date = null;
                Add_Date = null;
                Add_Operator = null;
                NameIndex = null;
            }
        }

        public ACTREFSEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "I";

                Agency = row["ACTREFS_AGENCY"].ToString(); ;
                Dept = row["ACTREFS_DEPT"].ToString(); ;
                Program = row["ACTREFS_PROGRAM"].ToString();
                Year = row["ACTREFS_YEAR"].ToString();
                ApplNo = row["ACTREFS_APP_NO"].ToString();
                Date = row["ACTREFS_DATE"].ToString();
                Type = row["ACTREFS_TYPE"].ToString();
                Connected = row["ACTREFS_CONNECTED"].ToString();
                Lsct_Operator = row["ACTREFS_LSTC_OPERATOR"].ToString();
                Lstc_Date = row["ACTREFS_DATE_LSTC"].ToString();
                Add_Date = row["ACTREFS_DATE_ADD"].ToString();
                Add_Operator = row["ACTREFS_ADD_OPERATOR"].ToString();
                Code = row["ACTREFS_CODE"].ToString();
                NameIndex = row["ACTREFS_NAMEINDEX"].ToString();
            }
        }

        public ACTREFSEntity(ACTREFSEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                ApplNo = Entity.ApplNo;
                Date = Entity.Date;
                Type = Entity.Type;
                Connected = Entity.Connected;
                Lsct_Operator = Entity.Lsct_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                NameIndex = Entity.NameIndex;
            }
        }

        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string ApplNo { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Connected { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string NameIndex { get; set; }

        #endregion
    }

    public class PARTNEREFEntity
    {
        #region Constructors

        public PARTNEREFEntity()
        {
            Rec_Type = string.Empty;
            RefID = string.Empty;
            Code = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            ApplNo = string.Empty;
            Date = string.Empty;
            Type = string.Empty;
            REPRESENTATIVE = string.Empty;
            FName = string.Empty;
            LName = string.Empty;
            Category = string.Empty;
            Status = string.Empty;
            RefExpDate = string.Empty;
            StatusDate = string.Empty;

            Lsct_Operator = string.Empty;
            Lstc_Date = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            Service = string.Empty;

        }

        public PARTNEREFEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                RefID = null;
                Code = null;
                Agency = null;
                Dept = null;
                Program = null;
                Year = null;
                ApplNo = null;
                Date = null;
                Type = null;
                REPRESENTATIVE = null;
                FName = null;
                LName = null;
                Category = null;
                Status = null;
                RefExpDate = null;
                StatusDate = null;
                Lsct_Operator = null;
                Lstc_Date = null;
                Add_Date = null;
                Add_Operator = null;

                Service = null;

            }
        }

        public PARTNEREFEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "I";

                RefID = row["PREF_ID"].ToString();
                Agency = row["PREF_AGENCY"].ToString(); ;
                Dept = row["PREF_DEPT"].ToString(); ;
                Program = row["PREF_PROGRAM"].ToString();
                Year = row["PREF_YEAR"].ToString();
                ApplNo = row["PREF_APP_NO"].ToString();
                Date = row["PREF_DATE"].ToString();
                Code = row["PREF_CODE"].ToString();
                Type = row["PREF_TYPE"].ToString();
                REPRESENTATIVE = row["PREF_REPRESENTATIVE"].ToString();
                FName = row["PREF_REP_FNAME"].ToString();
                LName = row["PREF_REP_LNAME"].ToString();
                Category = row["PREF_CATEGORY"].ToString();
                Status = row["PREF_STATUS"].ToString();
                RefExpDate = row["PREF_REFEXP_DATE"].ToString();
                StatusDate = row["PREF_STATUS_DATE"].ToString();


                Lsct_Operator = row["PREF_LSTC_OPERATOR"].ToString();
                Lstc_Date = row["PREF_DATE_LSTC"].ToString();
                Add_Date = row["PREF_DATE_ADD"].ToString();
                Add_Operator = row["PREF_ADD_OPERATOR"].ToString();
                //Code = row["ACTREFS_CODE"].ToString();

                Service = row["PREF_SERVICE"].ToString();

            }
        }

        public PARTNEREFEntity(PARTNEREFEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                RefID = Entity.RefID;
                Code = Entity.Code;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                ApplNo = Entity.ApplNo;
                Date = Entity.Date;
                Type = Entity.Type;
                REPRESENTATIVE = Entity.REPRESENTATIVE;
                FName = Entity.FName;
                LName = Entity.LName;
                Category = Entity.Category;
                Status = Entity.Status;
                RefExpDate = Entity.RefExpDate;
                StatusDate = Entity.StatusDate;
                Lsct_Operator = Entity.Lsct_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                Service = Entity.Service;

            }
        }

        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string RefID { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string ApplNo { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string REPRESENTATIVE { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string RefExpDate { get; set; }
        public string StatusDate { get; set; }
        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        //public string NameIndex { get; set; }

        public string Service { get; set; }

        #endregion
    }

    //**********************************************************************************************************

    public class RankCatgEntity
    {
        #region Constructors

        public RankCatgEntity()
        {
            Agency = string.Empty;
            Code = string.Empty;
            SubCode = string.Empty;
            Desc = string.Empty;
            PointsLow = string.Empty;
            PointsHigh = string.Empty;
            HeadStrt = string.Empty;
            DateLstc = string.Empty;
            lstcOperator = string.Empty;
            Dateadd = string.Empty;
            addoperator = string.Empty;

        }

        public RankCatgEntity(bool Intialize)
        {
            if (Intialize)
            {
                Agency = null;
                Code = null;
                SubCode = null;
                Desc = null;
                PointsLow = null;
                PointsHigh = null;
                HeadStrt = null;
                DateLstc = null;
                lstcOperator = null;
                Dateadd = null;
                addoperator = null;
            }
        }

        public RankCatgEntity(DataRow RANKCTG)
        {
            if (RANKCTG != null)
            {
                DataRow row = RANKCTG;
                Agency = row["RANKCTG_AGENCY"].ToString().Trim();
                Code = row["RANKCTG_GRPCTG_CODE"].ToString().Trim();
                SubCode = row["RANKCTG_SUBCTG_CODE"].ToString().Trim();
                Desc = row["RANKCTG_DESC"].ToString().Trim();
                PointsLow = row["RANKCTG_POINTS_LOW"].ToString().Trim();
                PointsHigh = row["RANKCTG_POINTS_HIGH"].ToString().Trim();
                HeadStrt = row["RANKCTG_HS_CTG"].ToString().Trim();
                DateLstc = row["RANKCTG_DATE_LSTC"].ToString().Trim();
                lstcOperator = row["RANKCTG_LSTC_OPERATOR"].ToString().Trim();
                Dateadd = row["RANKCTG_DATE_ADD"].ToString().Trim();
                addoperator = row["RANKCTG_ADD_OPERATOR"].ToString().Trim();
            }
        }
        public RankCatgEntity(DataRow RANKCTG, string strTable)
        {
            if (RANKCTG != null)
            {
                DataRow row = RANKCTG;
                Code = row["PREASSGRP_CODE"].ToString().Trim();
                SubCode = row["PREASSGRP_SUBCODE"].ToString().Trim();
                Desc = row["PREASSGRP_DESC"].ToString().Trim();
                PointsLow = row["PREASSGRP_POINTS_LOW"].ToString().Trim();
                PointsHigh = row["PREASSGRP_POINTS_HIGH"].ToString().Trim();
                HeadStrt = row["PREASSGRP_TYPE"].ToString().Trim();
                DateLstc = row["PREASSGRP_DATE_LSTC"].ToString().Trim();
                lstcOperator = row["PREASSGRP_LSTC_OPERATOR"].ToString().Trim();
                Dateadd = row["PREASSGRP_DATE_ADD"].ToString().Trim();
                addoperator = row["PREASSGRP_ADD_OPERATOR"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Code { get; set; }
        public string SubCode { get; set; }
        public string Desc { get; set; }
        public string PointsLow { get; set; }
        public string PointsHigh { get; set; }
        public string HeadStrt { get; set; }
        public string DateLstc { get; set; }
        public string lstcOperator { get; set; }
        public string Dateadd { get; set; }
        public string addoperator { get; set; }
        public string Mode { get; set; }

        #endregion


    }


    public class RNKCRIT1Entity
    {
        #region Constructors

        public RNKCRIT1Entity()
        {
            Agency = string.Empty;
            //Dept = string.Empty;
            //Prog = string.Empty;
            RankCtg = string.Empty;
            Scr_Cd = string.Empty;
            Fld_cd = string.Empty;
            Fld_type = string.Empty;
            Id = string.Empty;
            Hie = string.Empty;
            CountInd = string.Empty;
            AgeClcInd = string.Empty;
            dateLstc = string.Empty;
            LstcOperator = string.Empty;
            Dateadd = string.Empty;
            Addoperator = string.Empty;
            Fld_desc = string.Empty;

        }

        public RNKCRIT1Entity(DataRow RANKCRIT1)
        {
            if (RANKCRIT1 != null)
            {
                DataRow row = RANKCRIT1;
                Agency = row["RNKCRIT1_AGENCY"].ToString().Trim();
                //Dept = row["RNKCRIT1_DEPT"].ToString();
                //Prog = row["RNKCRIT1_PROGRAM"].ToString();
                //RankCtg = row["RNKCRIT1_RANK_CTG"].ToString();
                //Scr_Cd = row["RNKCRIT1_SCR_CODE"].ToString();
                //Fld_cd = row["RNKCRIT1_FIELD_CODE"].ToString();
                //Fld_type = row["RNKCRIT1_FLD_TYPE"].ToString();
                //Id = row["RNKCRIT1_ID"].ToString();
                //Hie = row["RNKCRIT1_HIERARCHY"].ToString();
                //CountInd = row["RNKCRIT1_COUNTING_INDI"].ToString();
                //AgeClcInd = row["RNKCRIT1_AGE_CLC_INDI"].ToString();
                //dateLstc = row["RNKCRIT1_DATE_LSTC"].ToString();
                //LstcOperator = row["RNKCRIT1_LSTC_OPERATOR"].ToString();
                //Dateadd = row["RNKCRIT1_DATE_ADD"].ToString();
                //Addoperator = row["RNKCRIT1_ADD_OPERATOR"].ToString();
                //Fld_desc = row["FLDSCRS_DESC"].ToString();

                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        //public string Dept { get; set; }
        //public string Prog { get; set; }
        public string RankCtg { get; set; }
        public string Scr_Cd { get; set; }
        public string Fld_cd { get; set; }
        public string Fld_type { get; set; }
        public string Id { get; set; }
        public string Hie { get; set; }
        public string CountInd { get; set; }
        public string AgeClcInd { get; set; }
        public string Dateadd { get; set; }
        public string LstcOperator { get; set; }
        public string dateLstc { get; set; }
        public string Addoperator { get; set; }
        public string Fld_desc { get; set; }
        public string Mode { get; set; }

        #endregion


    }

    public class RNKCRIT1Entity1
    {
        #region Constructors

        public RNKCRIT1Entity1()
        {
            Agency = string.Empty;
            //Dept = string.Empty;
            //Prog = string.Empty;
            RankCtg = string.Empty;
            Scr_Cd = string.Empty;
            Fld_cd = string.Empty;
            Fld_type = string.Empty;
            Id = string.Empty;
            //Hie = string.Empty;
            //CountInd = string.Empty;
            //AgeClcInd = string.Empty;
            dateLstc = string.Empty;
            LstcOperator = string.Empty;
            Dateadd = string.Empty;
            Addoperator = string.Empty;


        }

        public RNKCRIT1Entity1(DataRow RANKCRIT1)
        {
            if (RANKCRIT1 != null)
            {
                DataRow row = RANKCRIT1;
                Agency = row["RNKCRIT1_AGENCY"].ToString().Trim();
                //Dept = row["RNKCRIT1_DEPT"].ToString();
                //Prog = row["RNKCRIT1_PROGRAM"].ToString();
                RankCtg = row["RNKCRIT1_RANK_CTG"].ToString().Trim();
                Scr_Cd = row["RNKCRIT1_SCR_CODE"].ToString().Trim();
                Fld_cd = row["RNKCRIT1_FIELD_CODE"].ToString().Trim();
                Fld_type = row["RNKCRIT1_FLD_TYPE"].ToString().Trim();
                Id = row["RNKCRIT1_ID"].ToString().Trim();
                //Hie = row["RNKCRIT1_HIERARCHY"].ToString();
                //CountInd = row["RNKCRIT1_COUNTING_INDI"].ToString();
                //AgeClcInd = row["RNKCRIT1_AGE_CLC_INDI"].ToString();
                dateLstc = row["RNKCRIT1_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["RNKCRIT1_LSTC_OPERATOR"].ToString().Trim();
                Dateadd = row["RNKCRIT1_DATE_ADD"].ToString().Trim();
                Addoperator = row["RNKCRIT1_ADD_OPERATOR"].ToString().Trim();


                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        //public string Dept { get; set; }
        //public string Prog { get; set; }
        public string RankCtg { get; set; }
        public string Scr_Cd { get; set; }
        public string Fld_cd { get; set; }
        public string Fld_type { get; set; }
        public string Id { get; set; }
        //public string Hie { get; set; }
        //public string CountInd { get; set; }
        //public string AgeClcInd { get; set; }
        public string Dateadd { get; set; }
        public string LstcOperator { get; set; }
        public string dateLstc { get; set; }
        public string Addoperator { get; set; }
        public string Mode { get; set; }

        #endregion

    }

    public class RNKCRIT2Entity
    {
        #region Constructors

        public RNKCRIT2Entity()
        {
            Id = string.Empty;
            Seq = string.Empty;
            Relation = string.Empty;
            CountInd = string.Empty;
            AgeClcInd = string.Empty;
            RespCd = string.Empty;
            RespText = string.Empty;
            EqNum = string.Empty;
            GtNum = string.Empty;
            LtNum = string.Empty;
            EqDate = string.Empty;
            GtDate = string.Empty;
            LtDate = string.Empty;
            Points = string.Empty;
            dateLstc = string.Empty;
            LstcOperator = string.Empty;
            Dateadd = string.Empty;
            Addoperator = string.Empty;


        }

        public RNKCRIT2Entity(DataRow RANKCRIT2)
        {
            if (RANKCRIT2 != null)
            {
                DataRow row = RANKCRIT2;
                Id = row["RNKCRIT2_ID"].ToString().Trim();
                Seq = row["RNKCRIT2_SEQ"].ToString().Trim();
                Relation = row["RNKCRIT2_RELATION"].ToString().Trim();
                CountInd = row["RNKCRIT2_COUNT_IND"].ToString().Trim();
                AgeClcInd = row["RNKCRIT2_AGECLC_IND"].ToString().Trim();
                RespCd = row["RNKCRIT2_RESP_CODE"].ToString().Trim();
                RespText = row["RNKCRIT2_RESP_TEXT"].ToString().Trim();
                EqNum = row["RNKCRIT2_RESP_EQ_NUM"].ToString().Trim();
                GtNum = row["RNKCRIT2_RESP_GT_NUM"].ToString().Trim();
                LtNum = row["RNKCRIT2_RESP_LT_NUM"].ToString().Trim();
                EqDate = row["RNKCRIT2_RESP_EQ_DATE"].ToString().Trim();
                GtDate = row["RNKCRIT2_RESP_GT_DATE"].ToString().Trim();
                LtDate = row["RNKCRIT2_RESP_LT_DATE"].ToString().Trim();
                Points = row["RNKCRIT2_POINTS"].ToString().Trim();
                dateLstc = row["RNKCRIT2_DATE_LSTC"].ToString().Trim();
                LstcOperator = row["RNKCRIT2_LSTC_OPERATOR"].ToString().Trim();
                Dateadd = row["RNKCRIT2_DATE_ADD"].ToString().Trim();
                Addoperator = row["RNKCRIT2_ADD_OPERATOR"].ToString().Trim();


                //Spmaddoperator = row["spm_add_operator"].ToString();

            }
        }

        public RNKCRIT2Entity(DataRow RANKCRIT2, string strTable)
        {
            if (RANKCRIT2 != null)
            {
                if (strTable == "RANKQUES")
                {
                    DataRow row = RANKCRIT2;
                    RankFldName = row["RANKQUES_FLDNAME"].ToString().Trim();
                    RankFldDesc = row["RANKQUES_DESC"].ToString().Trim();
                    RankFldRespType = row["RANKQUES_RESP_TYPE"].ToString().Trim();
                    RankAgyCode = row["RANKQUES_AGY_CODE"].ToString().Trim();
                }
                else
                {
                    DataRow row = RANKCRIT2;
                    Id = row["RNKCRIT2_ID"].ToString().Trim();
                    Seq = row["RNKCRIT2_SEQ"].ToString().Trim();
                    Relation = row["RNKCRIT2_RELATION"].ToString().Trim();
                    CountInd = row["RNKCRIT2_COUNT_IND"].ToString().Trim();
                    AgeClcInd = row["RNKCRIT2_AGECLC_IND"].ToString().Trim();
                    RespCd = row["RNKCRIT2_RESP_CODE"].ToString().Trim();
                    RespText = row["RNKCRIT2_RESP_TEXT"].ToString().Trim();
                    EqNum = row["RNKCRIT2_RESP_EQ_NUM"].ToString().Trim();
                    GtNum = row["RNKCRIT2_RESP_GT_NUM"].ToString().Trim();
                    LtNum = row["RNKCRIT2_RESP_LT_NUM"].ToString().Trim();
                    EqDate = row["RNKCRIT2_RESP_EQ_DATE"].ToString().Trim();
                    GtDate = row["RNKCRIT2_RESP_GT_DATE"].ToString().Trim();
                    LtDate = row["RNKCRIT2_RESP_LT_DATE"].ToString().Trim();
                    Points = row["RNKCRIT2_POINTS"].ToString().Trim();
                    RankFldName = row["RANKQUES_FLDNAME"].ToString().Trim();
                    RankFldDesc = row["RANKQUES_DESC"].ToString().Trim();
                    RankFldRespType = row["RESPTYPE"].ToString().Trim();
                    RankFiledCode = row["RNKCRIT1_FIELD_CODE"].ToString().Trim();
                    RankCategory = row["RNKCRIT1_RANK_CTG"].ToString().Trim();
                }




            }
        }

        #endregion

        #region Properties

        public string Id { get; set; }
        public string Seq { get; set; }
        public string Relation { get; set; }
        public string CountInd { get; set; }
        public string AgeClcInd { get; set; }
        public string RespCd { get; set; }
        public string RespText { get; set; }
        public string EqNum { get; set; }
        public string GtNum { get; set; }
        public string LtNum { get; set; }
        public string EqDate { get; set; }
        public string GtDate { get; set; }
        public string LtDate { get; set; }
        public string Points { get; set; }
        public string Dateadd { get; set; }
        public string LstcOperator { get; set; }
        public string dateLstc { get; set; }
        public string Addoperator { get; set; }
        public string Mode { get; set; }
        public string RankCd { get; set; }
        public string RankFldName { get; set; }
        public string RankFldDesc { get; set; }
        public string RankFldRespType { get; set; }
        public string RankFiledCode { get; set; }
        public string RankCategory { get; set; }
        public string RankAgyCode { get; set; }
        #endregion


    }


    //*****************************************************************************************************

    public class CASEVDDEntity
    {
        #region Constructors

        public CASEVDDEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            Active = string.Empty;
            Name = string.Empty;
            Addr1 = string.Empty;
            Addr2 = string.Empty;
            Addr3 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Phone = string.Empty;
            Tax_Id = string.Empty;
            Tax_type = string.Empty;
            Vdd1099 = string.Empty;
            Cont_Name = string.Empty;
            Cont_Phone = string.Empty;
            Cont_Ext = string.Empty;
            Fax = string.Empty;
            Name_On_Checks = string.Empty;
            Email = string.Empty;
            SPLINSTR = string.Empty;
            W9 = string.Empty;
            FName = string.Empty;
            LName = string.Empty;
            Orginization = string.Empty;
            VDD_Agency = string.Empty;

        }

        public CASEVDDEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                Active = null;
                Name = null;
                Addr1 = null;
                Addr2 = null;
                Addr3 = null;
                City = null;
                State = null;
                Zip = null;
                Phone = null;
                Tax_Id = null;
                Tax_type = null;
                Vdd1099 = null;
                Cont_Name = null;
                Cont_Phone = null;
                Cont_Ext = null;
                Fax = null;
                Name_On_Checks = null;
                Email = null;
                SPLINSTR = null;
                W9 = null;
                FName = null;
                LName = null;
                Orginization = null;
                VDD_Agency = null;
            }

        }


        public CASEVDDEntity(DataRow CASEVDD)
        {
            if (CASEVDD != null)
            {
                DataRow row = CASEVDD;

                Rec_Type = "U";
                Code = row["CASEVDD_CODE"].ToString();
                Active = row["CASEVDD_ACTIVE"].ToString();
                Name = row["CASEVDD_NAME"].ToString();
                Addr1 = row["CASEVDD_ADDR1"].ToString();
                Addr2 = row["CASEVDD_ADDR2"].ToString();
                Addr3 = row["CASEVDD_ADDR3"].ToString();
                City = row["CASEVDD_CITY"].ToString();
                State = row["CASEVDD_STATE"].ToString();
                Zip = row["CASEVDD_ZIP"].ToString();

                Phone = row["CASEVDD_TELEPHONE"].ToString();
                Tax_Id = row["CASEVDD_TAX_ID"].ToString();
                Tax_type = row["CASEVDD_TAX_ID_TYPE"].ToString();
                Vdd1099 = row["CASEVDD_1099"].ToString();
                Cont_Name = row["CASEVDD_CONTACT_NAME"].ToString();
                Cont_Phone = row["CASEVDD_CONTACT_PHONE"].ToString();
                Cont_Ext = row["CASEVDD_CONTACT_EXT"].ToString();
                Fax = row["CASEVDD_FAX"].ToString();
                Name_On_Checks = row["CASEVDD_NAME_ON_CHECKS"].ToString();
                Email = row["CASEVDD_EMAIL"].ToString();
                SPLINSTR = row["CASEVDD_SPECINSTR"].ToString();

                W9 = row["CASEVDD_W9"].ToString();
                FName = row["CASEVDD_FNAME"].ToString();
                LName = row["CASEVDD_LNAME"].ToString();

                Orginization = row["CASEVDD_ORGNZN"].ToString();
                VDD_Agency = row["CASEVDD_AGENCY"].ToString();
            }
        }

        public CASEVDDEntity(CASEVDDEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Active = Entity.Active;
                Name = Entity.Name;
                Addr1 = Entity.Addr1;
                Addr2 = Entity.Addr2;
                Addr3 = Entity.Addr3;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                Phone = Entity.Phone;
                Tax_Id = Entity.Tax_Id;
                Tax_type = Entity.Tax_type;
                Vdd1099 = Entity.Vdd1099;

                Cont_Name = Entity.Cont_Name;
                Cont_Phone = Entity.Cont_Phone;
                Cont_Ext = Entity.Cont_Ext;
                Fax = Entity.Fax;
                Name_On_Checks = Entity.Name_On_Checks;
                Email = Entity.Email;
                SPLINSTR = Entity.SPLINSTR;

                W9 = Entity.W9;
                FName = Entity.FName;
                LName = Entity.LName;
                Orginization = Entity.Orginization;
                VDD_Agency = Entity.VDD_Agency;

            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Active { get; set; }
        public string Name { get; set; }
        public string Addr1 { get; set; }

        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }

        public string Tax_Id { get; set; }
        public string Tax_type { get; set; }
        public string Vdd1099 { get; set; }
        public string Cont_Name { get; set; }
        public string Cont_Phone { get; set; }
        public string Cont_Ext { get; set; }
        public string Fax { get; set; }
        public string Name_On_Checks { get; set; }

        public string Email { get; set; }
        public string SPLINSTR { get; set; }

        public string W9 { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Orginization { get; set; }

        public string VDD_Agency{ get; set; }


        #endregion
    }


    public class CaseVDD1Entity
    {
        #region Constructors

        public CaseVDD1Entity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            IndexBy = string.Empty;
            Name2 = string.Empty;
            Type = string.Empty;
            BULK_CODE = string.Empty;
            BULK_DEL = string.Empty;
            BULK_ALLOT = string.Empty;
            BULK_USED = string.Empty;
            TOTAL_PAID = string.Empty;
            PRI_YR_PAID = string.Empty;
            TOT_APPS = string.Empty;
            SSNO = string.Empty;
            CYCLE = string.Empty;
            ELEC_TRANSFER = string.Empty;
            PRT_PAYREQ = string.Empty;
            AWT_PYMT = string.Empty;
            ACCT_FORMAT = string.Empty;
            USE_VENDOR_CODE = string.Empty;
            YTD_NO_CHECKS = string.Empty;
            DATE_LAST_CHECK = string.Empty;
            MONTHLY_OTHER = string.Empty;
            LAST_CHECK_NO = string.Empty;
            UTILITY_CD1 = string.Empty;
            UTILITY_CD2 = string.Empty;
            STATE_APPS = string.Empty;
            STATE_PAID = string.Empty;
            WAP_APPS = string.Empty;
            WAP_PAID = string.Empty;
            FUEL_TYPE1 = string.Empty;
            FUEL_TYPE2 = string.Empty;
            FUEL_TYPE3 = string.Empty;
            FUEL_TYPE4 = string.Empty;
            FUEL_TYPE5 = string.Empty;
            FUEL_TYPE6 = string.Empty;
            FUEL_TYPE7 = string.Empty;
            FUEL_TYPE8 = string.Empty;
            FUEL_TYPE9 = string.Empty;
            FUEL_TYPE10 = string.Empty;
            TEMP_VENDOR = string.Empty;
            ME_CODE = string.Empty;
            ME_CERT = string.Empty;
            ME_AUTH = string.Empty;
            ME_EDATE = string.Empty;
            DAYS = string.Empty;
            SEL_CYCLE = string.Empty;
            HTOTAL_PAID = string.Empty;
            HPRI_YR_PAID = string.Empty;
            HTOT_APPS = string.Empty;
            HAWT_PYMT = string.Empty;
            HYTD_NO_CHECKS = string.Empty;
            HDATE_LAST_CHECK = string.Empty;
            HMONTHLY_OTHER = string.Empty;
            HLAST_CHECK_NO = string.Empty;
            HSTATE_APPS = string.Empty;
            HSTATE_PAID = string.Empty;
            HWAP_APPS = string.Empty;
            HWAP_PAID = string.Empty;
            SVENDOR_CODE = string.Empty;

            AR = string.Empty;
            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            FuelType = string.Empty;


            VddCode = string.Empty;
            VddActive = string.Empty;
            VddName = string.Empty;
            VddAddr1 = string.Empty;
            VddAddr2 = string.Empty;
            VddAddr3 = string.Empty;
            VddCity = string.Empty;
            VddState = string.Empty;
            VddZip = string.Empty;
            VddPhone = string.Empty;
            VddTax_Id = string.Empty;
            VddTax_type = string.Empty;
            VddVdd1099 = string.Empty;
            VddCont_Name = string.Empty;
            VddCont_Phone = string.Empty;
            VddCont_Ext = string.Empty;
            VddFax = string.Empty;
            VddName_On_Checks = string.Empty;
            EINSSN_TYPE = string.Empty;

        }

        public CaseVDD1Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                IndexBy = null;
                Name2 = null;
                Type = null;
                BULK_CODE = null;
                BULK_DEL = null;
                BULK_ALLOT = null;
                BULK_USED = null;
                TOTAL_PAID = null;
                PRI_YR_PAID = null;
                TOT_APPS = null;
                SSNO = null;
                CYCLE = null;
                ELEC_TRANSFER = null;
                PRT_PAYREQ = null;
                AWT_PYMT = null;
                ACCT_FORMAT = null;
                USE_VENDOR_CODE = null;
                YTD_NO_CHECKS = null;
                DATE_LAST_CHECK = null;
                MONTHLY_OTHER = null;
                LAST_CHECK_NO = null;
                UTILITY_CD1 = null;
                UTILITY_CD2 = null;
                STATE_APPS = null;
                STATE_PAID = null;
                WAP_APPS = null;
                WAP_PAID = null;
                FUEL_TYPE1 = null;
                FUEL_TYPE2 = null;
                FUEL_TYPE3 = null;
                FUEL_TYPE4 = null;
                FUEL_TYPE5 = null;
                FUEL_TYPE6 = null;
                FUEL_TYPE7 = null;
                FUEL_TYPE8 = null;
                FUEL_TYPE9 = null;
                FUEL_TYPE10 = null;
                TEMP_VENDOR = null;
                ME_CODE = null;
                ME_CERT = null;
                ME_AUTH = null;
                ME_EDATE = null;
                DAYS = null;
                SEL_CYCLE = null;
                HTOTAL_PAID = null;
                HPRI_YR_PAID = null;
                HTOT_APPS = null;
                HAWT_PYMT = null;
                HYTD_NO_CHECKS = null;
                HDATE_LAST_CHECK = null;
                HMONTHLY_OTHER = null;
                HLAST_CHECK_NO = null;
                HSTATE_APPS = null;
                HSTATE_PAID = null;
                HWAP_APPS = null;
                HWAP_PAID = null;
                SVENDOR_CODE = null;

                AR = null;
                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
                FuelType = null;
                EINSSN_TYPE = null;
            }

        }


        public CaseVDD1Entity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Code = row["CASEVDD1_CODE"].ToString();
                IndexBy = row["CASEVDD1_INDEXBY"].ToString();
                Name2 = row["CASEVDD1_NAME2"].ToString();
                Type = row["CASEVDD1_TYPE"].ToString();
                BULK_CODE = row["CASEVDD1_BULK_CODE"].ToString();
                BULK_DEL = row["CASEVDD1_BULK_DEL"].ToString();
                BULK_ALLOT = row["CASEVDD1_BULK_ALLOT"].ToString();
                BULK_USED = row["CASEVDD1_BULK_USED"].ToString();
                TOTAL_PAID = row["CASEVDD1_TOTAL_PAID"].ToString();
                PRI_YR_PAID = row["CASEVDD1_PRI_YR_PAID"].ToString();
                TOT_APPS = row["CASEVDD1_TOT_APPS"].ToString();
                SSNO = row["CASEVDD1_SSNO"].ToString();
                CYCLE = row["CASEVDD1_CYCLE"].ToString();
                ELEC_TRANSFER = row["CASEVDD1_ELEC_TRANSFER"].ToString();
                PRT_PAYREQ = row["CASEVDD1_PRT_PAYREQ"].ToString();
                AWT_PYMT = row["CASEVDD1_AWT_PYMT"].ToString();
                ACCT_FORMAT = row["CASEVDD1_ACCT_FORMAT"].ToString();
                USE_VENDOR_CODE = row["CASEVDD1_USE_VENDOR_CODE"].ToString();
                YTD_NO_CHECKS = row["CASEVDD1_YTD_NO_CHECKS"].ToString();
                DATE_LAST_CHECK = row["CASEVDD1_DATE_LAST_CHECK"].ToString();
                MONTHLY_OTHER = row["CASEVDD1_MONTHLY_OTHER"].ToString();
                LAST_CHECK_NO = row["CASEVDD1_LAST_CHECK_NO"].ToString();
                UTILITY_CD1 = row["CASEVDD1_UTILITY_CD1"].ToString();
                UTILITY_CD2 = row["CASEVDD1_UTILITY_CD2"].ToString();
                STATE_APPS = row["CASEVDD1_STATE_APPS"].ToString();
                STATE_PAID = row["CASEVDD1_STATE_PAID"].ToString();
                WAP_APPS = row["CASEVDD1_WAP_APPS"].ToString();
                WAP_PAID = row["CASEVDD1_WAP_PAID"].ToString();
                FUEL_TYPE1 = row["CASEVDD1_FUEL_TYPE1"].ToString();
                FUEL_TYPE2 = row["CASEVDD1_FUEL_TYPE2"].ToString();
                FUEL_TYPE3 = row["CASEVDD1_FUEL_TYPE3"].ToString();
                FUEL_TYPE4 = row["CASEVDD1_FUEL_TYPE4"].ToString();
                FUEL_TYPE5 = row["CASEVDD1_FUEL_TYPE5"].ToString();
                FUEL_TYPE6 = row["CASEVDD1_FUEL_TYPE6"].ToString();
                FUEL_TYPE7 = row["CASEVDD1_FUEL_TYPE7"].ToString();
                FUEL_TYPE8 = row["CASEVDD1_FUEL_TYPE8"].ToString();
                FUEL_TYPE9 = row["CASEVDD1_FUEL_TYPE9"].ToString();
                FUEL_TYPE10 = row["CASEVDD1_FUEL_TYPE10"].ToString();
                TEMP_VENDOR = row["CASEVDD1_TEMP_VENDOR"].ToString();
                ME_CODE = row["CASEVDD1_ME_CODE"].ToString();
                ME_CERT = row["CASEVDD1_ME_CERT"].ToString();
                ME_AUTH = row["CASEVDD1_ME_AUTH"].ToString();
                ME_EDATE = row["CASEVDD1_ME_EDATE"].ToString();
                DAYS = row["CASEVDD1_DAYS"].ToString();
                SEL_CYCLE = row["CASEVDD1_SEL_CYCLE"].ToString();
                HTOTAL_PAID = row["CASEVDD1_HTOTAL_PAID"].ToString();
                HPRI_YR_PAID = row["CASEVDD1_HPRI_YR_PAID"].ToString();
                HTOT_APPS = row["CASEVDD1_HTOT_APPS"].ToString();
                HAWT_PYMT = row["CASEVDD1_HAWT_PYMT"].ToString();
                HYTD_NO_CHECKS = row["CASEVDD1_HYTD_NO_CHECKS"].ToString();
                HDATE_LAST_CHECK = row["CASEVDD1_HDATE_LAST_CHECK"].ToString();
                HMONTHLY_OTHER = row["CASEVDD1_HMONTHLY_OTHER"].ToString();
                HLAST_CHECK_NO = row["CASEVDD1_HLAST_CHECK_NO"].ToString();
                HSTATE_APPS = row["CASEVDD1_HSTATE_APPS"].ToString();
                HSTATE_PAID = row["CASEVDD1_HSTATE_PAID"].ToString();
                HWAP_APPS = row["CASEVDD1_HWAP_APPS"].ToString();
                HWAP_PAID = row["CASEVDD1_HWAP_PAID"].ToString();
                SVENDOR_CODE = row["CASEVDD1_SVENDOR_CODE"].ToString();

                AR = row["CASEVDD1_AR"].ToString();
                Lstc_Date = row["CASEVDD1_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEVDD1_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASEVDD1_DATE_ADD"].ToString();
                Add_Operator = row["CASEVDD1_ADD_OPERATOR"].ToString();
                EINSSN_TYPE = row["CASEVDD1_1099_TYPE"].ToString();
            }
        }


        public CaseVDD1Entity(DataRow CASESPM, string strTable)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Code = row["CASEVDD1_CODE"].ToString();
                IndexBy = row["CASEVDD1_INDEXBY"].ToString();
                Name2 = row["CASEVDD1_NAME2"].ToString();
                Type = row["CASEVDD1_TYPE"].ToString();
                BULK_CODE = row["CASEVDD1_BULK_CODE"].ToString();
                BULK_DEL = row["CASEVDD1_BULK_DEL"].ToString();
                BULK_ALLOT = row["CASEVDD1_BULK_ALLOT"].ToString();
                BULK_USED = row["CASEVDD1_BULK_USED"].ToString();
                TOTAL_PAID = row["CASEVDD1_TOTAL_PAID"].ToString();
                PRI_YR_PAID = row["CASEVDD1_PRI_YR_PAID"].ToString();
                TOT_APPS = row["CASEVDD1_TOT_APPS"].ToString();
                SSNO = row["CASEVDD1_SSNO"].ToString();
                CYCLE = row["CASEVDD1_CYCLE"].ToString();
                ELEC_TRANSFER = row["CASEVDD1_ELEC_TRANSFER"].ToString();
                PRT_PAYREQ = row["CASEVDD1_PRT_PAYREQ"].ToString();
                AWT_PYMT = row["CASEVDD1_AWT_PYMT"].ToString();
                ACCT_FORMAT = row["CASEVDD1_ACCT_FORMAT"].ToString();
                USE_VENDOR_CODE = row["CASEVDD1_USE_VENDOR_CODE"].ToString();
                YTD_NO_CHECKS = row["CASEVDD1_YTD_NO_CHECKS"].ToString();
                DATE_LAST_CHECK = row["CASEVDD1_DATE_LAST_CHECK"].ToString();
                MONTHLY_OTHER = row["CASEVDD1_MONTHLY_OTHER"].ToString();
                LAST_CHECK_NO = row["CASEVDD1_LAST_CHECK_NO"].ToString();
                UTILITY_CD1 = row["CASEVDD1_UTILITY_CD1"].ToString();
                UTILITY_CD2 = row["CASEVDD1_UTILITY_CD2"].ToString();
                STATE_APPS = row["CASEVDD1_STATE_APPS"].ToString();
                STATE_PAID = row["CASEVDD1_STATE_PAID"].ToString();
                WAP_APPS = row["CASEVDD1_WAP_APPS"].ToString();
                WAP_PAID = row["CASEVDD1_WAP_PAID"].ToString();
                FUEL_TYPE1 = row["CASEVDD1_FUEL_TYPE1"].ToString();
                FUEL_TYPE2 = row["CASEVDD1_FUEL_TYPE2"].ToString();
                FUEL_TYPE3 = row["CASEVDD1_FUEL_TYPE3"].ToString();
                FUEL_TYPE4 = row["CASEVDD1_FUEL_TYPE4"].ToString();
                FUEL_TYPE5 = row["CASEVDD1_FUEL_TYPE5"].ToString();
                FUEL_TYPE6 = row["CASEVDD1_FUEL_TYPE6"].ToString();
                FUEL_TYPE7 = row["CASEVDD1_FUEL_TYPE7"].ToString();
                FUEL_TYPE8 = row["CASEVDD1_FUEL_TYPE8"].ToString();
                FUEL_TYPE9 = row["CASEVDD1_FUEL_TYPE9"].ToString();
                FUEL_TYPE10 = row["CASEVDD1_FUEL_TYPE10"].ToString();
                TEMP_VENDOR = row["CASEVDD1_TEMP_VENDOR"].ToString();
                ME_CODE = row["CASEVDD1_ME_CODE"].ToString();
                ME_CERT = row["CASEVDD1_ME_CERT"].ToString();
                ME_AUTH = row["CASEVDD1_ME_AUTH"].ToString();
                ME_EDATE = row["CASEVDD1_ME_EDATE"].ToString();
                DAYS = row["CASEVDD1_DAYS"].ToString();
                SEL_CYCLE = row["CASEVDD1_SEL_CYCLE"].ToString();
                HTOTAL_PAID = row["CASEVDD1_HTOTAL_PAID"].ToString();
                HPRI_YR_PAID = row["CASEVDD1_HPRI_YR_PAID"].ToString();
                HTOT_APPS = row["CASEVDD1_HTOT_APPS"].ToString();
                HAWT_PYMT = row["CASEVDD1_HAWT_PYMT"].ToString();
                HYTD_NO_CHECKS = row["CASEVDD1_HYTD_NO_CHECKS"].ToString();
                HDATE_LAST_CHECK = row["CASEVDD1_HDATE_LAST_CHECK"].ToString();
                HMONTHLY_OTHER = row["CASEVDD1_HMONTHLY_OTHER"].ToString();
                HLAST_CHECK_NO = row["CASEVDD1_HLAST_CHECK_NO"].ToString();
                HSTATE_APPS = row["CASEVDD1_HSTATE_APPS"].ToString();
                HSTATE_PAID = row["CASEVDD1_HSTATE_PAID"].ToString();
                HWAP_APPS = row["CASEVDD1_HWAP_APPS"].ToString();
                HWAP_PAID = row["CASEVDD1_HWAP_PAID"].ToString();
                SVENDOR_CODE = row["CASEVDD1_SVENDOR_CODE"].ToString();

                AR = row["CASEVDD1_AR"].ToString();
                Lstc_Date = row["CASEVDD1_DATE_LSTC"].ToString();
                Lsct_Operator = row["CASEVDD1_LSTC_OPERATOR"].ToString();
                Add_Date = row["CASEVDD1_DATE_ADD"].ToString();
                Add_Operator = row["CASEVDD1_ADD_OPERATOR"].ToString();

                VddCode = row["CASEVDD_CODE"].ToString();
                VddActive = row["CASEVDD_ACTIVE"].ToString();
                VddName = row["CASEVDD_NAME"].ToString();
                VddAddr1 = row["CASEVDD_ADDR1"].ToString();
                VddAddr2 = row["CASEVDD_ADDR2"].ToString();
                VddAddr3 = row["CASEVDD_ADDR3"].ToString();
                VddCity = row["CASEVDD_CITY"].ToString();
                VddState = row["CASEVDD_STATE"].ToString();
                VddZip = row["CASEVDD_ZIP"].ToString();

                VddPhone = row["CASEVDD_TELEPHONE"].ToString();
                VddTax_Id = row["CASEVDD_TAX_ID"].ToString();
                VddTax_type = row["CASEVDD_TAX_ID_TYPE"].ToString();
                VddVdd1099 = row["CASEVDD_1099"].ToString();
                VddCont_Name = row["CASEVDD_CONTACT_NAME"].ToString();
                VddCont_Phone = row["CASEVDD_CONTACT_PHONE"].ToString();
                VddCont_Ext = row["CASEVDD_CONTACT_EXT"].ToString();
                VddFax = row["CASEVDD_FAX"].ToString();
                VddName_On_Checks = row["CASEVDD_NAME_ON_CHECKS"].ToString();
                EINSSN_TYPE = row["CASEVDD1_1099_TYPE"].ToString();

            }
        }


        public CaseVDD1Entity(CaseVDD1Entity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                IndexBy = Entity.IndexBy;
                Name2 = Entity.Name2;
                Type = Entity.Type;
                BULK_CODE = Entity.BULK_CODE;
                BULK_DEL = Entity.BULK_DEL;
                BULK_ALLOT = Entity.BULK_ALLOT;
                BULK_USED = Entity.BULK_USED;
                TOTAL_PAID = Entity.TOTAL_PAID;
                PRI_YR_PAID = Entity.PRI_YR_PAID;
                TOT_APPS = Entity.TOT_APPS;
                SSNO = Entity.SSNO;
                CYCLE = Entity.CYCLE;
                ELEC_TRANSFER = Entity.ELEC_TRANSFER;
                PRT_PAYREQ = Entity.PRT_PAYREQ;
                AWT_PYMT = Entity.AWT_PYMT;
                ACCT_FORMAT = Entity.ACCT_FORMAT;
                USE_VENDOR_CODE = Entity.USE_VENDOR_CODE;
                YTD_NO_CHECKS = Entity.YTD_NO_CHECKS;
                DATE_LAST_CHECK = Entity.DATE_LAST_CHECK;
                MONTHLY_OTHER = Entity.MONTHLY_OTHER;
                LAST_CHECK_NO = Entity.LAST_CHECK_NO;
                UTILITY_CD1 = Entity.UTILITY_CD1;
                UTILITY_CD2 = Entity.UTILITY_CD2;
                STATE_APPS = Entity.STATE_APPS;
                STATE_PAID = Entity.STATE_PAID;
                WAP_APPS = Entity.WAP_APPS;
                WAP_PAID = Entity.WAP_PAID;
                FUEL_TYPE1 = Entity.FUEL_TYPE1;
                FUEL_TYPE2 = Entity.FUEL_TYPE2;
                FUEL_TYPE3 = Entity.FUEL_TYPE3;
                FUEL_TYPE4 = Entity.FUEL_TYPE4;
                FUEL_TYPE5 = Entity.FUEL_TYPE5;
                FUEL_TYPE6 = Entity.FUEL_TYPE6;
                FUEL_TYPE7 = Entity.FUEL_TYPE7;
                FUEL_TYPE8 = Entity.FUEL_TYPE8;
                FUEL_TYPE9 = Entity.FUEL_TYPE9;
                FUEL_TYPE10 = Entity.FUEL_TYPE10;
                TEMP_VENDOR = Entity.TEMP_VENDOR;
                ME_CODE = Entity.ME_CODE;
                ME_CERT = Entity.ME_CERT;
                ME_AUTH = Entity.ME_AUTH;
                ME_EDATE = Entity.ME_EDATE;
                DAYS = Entity.DAYS;
                SEL_CYCLE = Entity.SEL_CYCLE;
                HTOTAL_PAID = Entity.HTOTAL_PAID;
                HPRI_YR_PAID = Entity.HPRI_YR_PAID;
                HTOT_APPS = Entity.HTOT_APPS;
                HAWT_PYMT = Entity.HAWT_PYMT;
                HYTD_NO_CHECKS = Entity.HYTD_NO_CHECKS;
                HDATE_LAST_CHECK = Entity.HDATE_LAST_CHECK;
                HMONTHLY_OTHER = Entity.HMONTHLY_OTHER;
                HLAST_CHECK_NO = Entity.HLAST_CHECK_NO;
                HSTATE_APPS = Entity.HSTATE_APPS;
                HSTATE_PAID = Entity.HSTATE_PAID;
                HWAP_APPS = Entity.HWAP_APPS;
                HWAP_PAID = Entity.HWAP_PAID;
                SVENDOR_CODE = Entity.SVENDOR_CODE;

                AR = Entity.AR;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                FuelType = Entity.FuelType;


                VddCode = Entity.VddCode.ToString();
                VddActive = Entity.VddActive.ToString();
                VddName = Entity.VddName;
                VddAddr1 = Entity.VddAddr1;
                VddAddr2 = Entity.VddAddr2;
                VddAddr3 = Entity.VddAddr3;
                VddCity = Entity.VddCity;
                VddState = Entity.VddState;
                VddZip = Entity.VddZip;

                VddPhone = Entity.VddPhone;
                VddTax_Id = Entity.VddTax_Id;
                VddTax_type = Entity.VddTax_type;
                VddVdd1099 = Entity.VddVdd1099;
                VddCont_Name = Entity.VddCont_Name;
                VddCont_Phone = Entity.VddCont_Phone;
                VddCont_Ext = Entity.VddCont_Ext;
                VddFax = Entity.VddFax;
                VddName_On_Checks = Entity.VddName_On_Checks;
                EINSSN_TYPE = Entity.EINSSN_TYPE;
            }
        }


        public CaseVDD1Entity(CaseVDD1Entity Entity, string strTable)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                IndexBy = Entity.IndexBy;
                Name2 = Entity.Name2;
                Type = Entity.Type;
                BULK_CODE = Entity.BULK_CODE;
                BULK_DEL = Entity.BULK_DEL;
                BULK_ALLOT = Entity.BULK_ALLOT;
                BULK_USED = Entity.BULK_USED;
                TOTAL_PAID = Entity.TOTAL_PAID;
                PRI_YR_PAID = Entity.PRI_YR_PAID;
                TOT_APPS = Entity.TOT_APPS;
                SSNO = Entity.SSNO;
                CYCLE = Entity.CYCLE;
                ELEC_TRANSFER = Entity.ELEC_TRANSFER;
                PRT_PAYREQ = Entity.PRT_PAYREQ;
                AWT_PYMT = Entity.AWT_PYMT;
                ACCT_FORMAT = Entity.ACCT_FORMAT;
                USE_VENDOR_CODE = Entity.USE_VENDOR_CODE;
                YTD_NO_CHECKS = Entity.YTD_NO_CHECKS;
                DATE_LAST_CHECK = Entity.DATE_LAST_CHECK;
                MONTHLY_OTHER = Entity.MONTHLY_OTHER;
                LAST_CHECK_NO = Entity.LAST_CHECK_NO;
                UTILITY_CD1 = Entity.UTILITY_CD1;
                UTILITY_CD2 = Entity.UTILITY_CD2;
                STATE_APPS = Entity.STATE_APPS;
                STATE_PAID = Entity.STATE_PAID;
                WAP_APPS = Entity.WAP_APPS;
                WAP_PAID = Entity.WAP_PAID;
                FUEL_TYPE1 = Entity.FUEL_TYPE1;
                FUEL_TYPE2 = Entity.FUEL_TYPE2;
                FUEL_TYPE3 = Entity.FUEL_TYPE3;
                FUEL_TYPE4 = Entity.FUEL_TYPE4;
                FUEL_TYPE5 = Entity.FUEL_TYPE5;
                FUEL_TYPE6 = Entity.FUEL_TYPE6;
                FUEL_TYPE7 = Entity.FUEL_TYPE7;
                FUEL_TYPE8 = Entity.FUEL_TYPE8;
                FUEL_TYPE9 = Entity.FUEL_TYPE9;
                FUEL_TYPE10 = Entity.FUEL_TYPE10;
                TEMP_VENDOR = Entity.TEMP_VENDOR;
                ME_CODE = Entity.ME_CODE;
                ME_CERT = Entity.ME_CERT;
                ME_AUTH = Entity.ME_AUTH;
                ME_EDATE = Entity.ME_EDATE;
                DAYS = Entity.DAYS;
                SEL_CYCLE = Entity.SEL_CYCLE;
                HTOTAL_PAID = Entity.HTOTAL_PAID;
                HPRI_YR_PAID = Entity.HPRI_YR_PAID;
                HTOT_APPS = Entity.HTOT_APPS;
                HAWT_PYMT = Entity.HAWT_PYMT;
                HYTD_NO_CHECKS = Entity.HYTD_NO_CHECKS;
                HDATE_LAST_CHECK = Entity.HDATE_LAST_CHECK;
                HMONTHLY_OTHER = Entity.HMONTHLY_OTHER;
                HLAST_CHECK_NO = Entity.HLAST_CHECK_NO;
                HSTATE_APPS = Entity.HSTATE_APPS;
                HSTATE_PAID = Entity.HSTATE_PAID;
                HWAP_APPS = Entity.HWAP_APPS;
                HWAP_PAID = Entity.HWAP_PAID;
                SVENDOR_CODE = Entity.SVENDOR_CODE;

                AR = Entity.AR;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                FuelType = Entity.FuelType;
                EINSSN_TYPE = Entity.EINSSN_TYPE;

            }
        }




        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string IndexBy { get; set; }
        public string Name2 { get; set; }
        public string Type { get; set; }


        public string BULK_CODE { get; set; }
        public string BULK_DEL { get; set; }
        public string BULK_ALLOT { get; set; }
        public string BULK_USED { get; set; }
        public string TOTAL_PAID { get; set; }
        public string PRI_YR_PAID { get; set; }
        public string TOT_APPS { get; set; }
        public string SSNO { get; set; }
        public string CYCLE { get; set; }
        public string ELEC_TRANSFER { get; set; }
        public string PRT_PAYREQ { get; set; }
        public string AWT_PYMT { get; set; }
        public string ACCT_FORMAT { get; set; }
        public string USE_VENDOR_CODE { get; set; }
        public string YTD_NO_CHECKS { get; set; }
        public string DATE_LAST_CHECK { get; set; }
        public string MONTHLY_OTHER { get; set; }
        public string LAST_CHECK_NO { get; set; }
        public string UTILITY_CD1 { get; set; }


        public string UTILITY_CD2 { get; set; }
        public string STATE_APPS { get; set; }
        public string STATE_PAID { get; set; }
        public string WAP_APPS { get; set; }
        public string WAP_PAID { get; set; }

        public string FUEL_TYPE1 { get; set; }
        public string FUEL_TYPE2 { get; set; }
        public string FUEL_TYPE3 { get; set; }
        public string FUEL_TYPE4 { get; set; }
        public string FUEL_TYPE5 { get; set; }
        public string FUEL_TYPE6 { get; set; }
        public string FUEL_TYPE7 { get; set; }
        public string FUEL_TYPE8 { get; set; }
        public string FUEL_TYPE9 { get; set; }
        public string FUEL_TYPE10 { get; set; }

        public string TEMP_VENDOR { get; set; }
        public string ME_CODE { get; set; }
        public string ME_CERT { get; set; }
        public string ME_AUTH { get; set; }
        public string ME_EDATE { get; set; }
        public string DAYS { get; set; }
        public string SEL_CYCLE { get; set; }
        public string HTOTAL_PAID { get; set; }
        public string HPRI_YR_PAID { get; set; }
        public string HTOT_APPS { get; set; }
        public string HAWT_PYMT { get; set; }
        public string HYTD_NO_CHECKS { get; set; }
        public string HDATE_LAST_CHECK { get; set; }
        public string HMONTHLY_OTHER { get; set; }

        public string HLAST_CHECK_NO { get; set; }
        public string HSTATE_APPS { get; set; }
        public string HSTATE_PAID { get; set; }
        public string HWAP_APPS { get; set; }
        public string HWAP_PAID { get; set; }

        public string SVENDOR_CODE { get; set; }

        public string AR { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        public string FuelType { get; set; }


        public string VddCode { get; set; }
        public string VddActive { get; set; }
        public string VddName { get; set; }
        public string VddAddr1 { get; set; }

        public string VddAddr2 { get; set; }
        public string VddAddr3 { get; set; }
        public string VddCity { get; set; }
        public string VddState { get; set; }
        public string VddZip { get; set; }
        public string VddPhone { get; set; }

        public string VddTax_Id { get; set; }
        public string VddTax_type { get; set; }
        public string VddVdd1099 { get; set; }
        public string VddCont_Name { get; set; }
        public string VddCont_Phone { get; set; }
        public string VddCont_Ext { get; set; }
        public string VddFax { get; set; }
        public string VddName_On_Checks { get; set; }
        public string EINSSN_TYPE { get; set; }

        #endregion
    }


    public class Csb16DTREntity
    {
        #region Constructors

        public Csb16DTREntity()
        {
            row_Type = string.Empty;
            REF_FDATE = string.Empty;
            REF_TDATE = string.Empty;
            SYS_Date_Range =
            REF_ACTIVE = string.Empty;
            REP_FDATE1 = string.Empty;
            REP_TDATE1 = string.Empty;
            REP_ACTIVE1 = string.Empty;
            REP_FDATE2 = string.Empty;
            REP_TDATE2 = string.Empty;
            REP_ACTIVE2 = string.Empty;
            REP_FDATE3 = string.Empty;
            REP_TDATE3 = string.Empty;
            REP_ACTIVE3 = string.Empty;
            REP_FDATE4 = string.Empty;
            REP_TDATE4 = string.Empty;
            REP_ACTIVE4 = string.Empty;

            DATE_LSTC = string.Empty;
            lstc_operator = string.Empty;
            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;

        }

        public Csb16DTREntity(bool Intialize)
        {

            if (Intialize)
            {
                row_Type = null;
                REF_FDATE = null;
                REF_TDATE = null;
                SYS_Date_Range =
                REF_ACTIVE = null;
                REP_FDATE1 = null;
                REP_TDATE1 = null;
                REP_ACTIVE1 = null;
                REP_FDATE2 = null;
                REP_TDATE2 = null;
                REP_ACTIVE2 = null;
                REP_FDATE3 = null;
                REP_TDATE3 = null;
                REP_ACTIVE3 = null;
                REP_FDATE4 = null;
                REP_TDATE4 = null;
                REP_ACTIVE4 = null;

                DATE_LSTC = null;
                lstc_operator = null;
                DATE_ADD = null;
                ADD_OPERATOR = null;
            }

        }

        public Csb16DTREntity(DataRow CSB16DTR)
        {
            if (CSB16DTR != null)
            {
                DataRow row = CSB16DTR;
                row_Type = "U";
                REF_FDATE = row["C16DTR_REF_FDATE"].ToString();
                REF_TDATE = row["C16DTR_REF_TDATE"].ToString();
                REF_ACTIVE = row["C16DTR_REF_ACTIVE"].ToString();
                REP_FDATE1 = row["C16DTR_REP_FDATE1"].ToString();
                REP_TDATE1 = row["C16DTR_REP_TDATE1"].ToString();
                REP_ACTIVE1 = row["C16DTR_REP_ACTIVE1"].ToString();
                REP_FDATE2 = row["C16DTR_REP_FDATE2"].ToString();
                REP_TDATE2 = row["C16DTR_REP_TDATE2"].ToString();
                REP_ACTIVE2 = row["C16DTR_REP_ACTIVE2"].ToString();
                REP_FDATE3 = row["C16DTR_REP_FDATE3"].ToString();
                REP_TDATE3 = row["C16DTR_REP_TDATE3"].ToString();
                REP_ACTIVE3 = row["C16DTR_REP_ACTIVE3"].ToString();
                REP_FDATE4 = row["C16DTR_REP_FDATE4"].ToString();
                REP_TDATE4 = row["C16DTR_REP_TDATE4"].ToString();
                REP_ACTIVE4 = row["C16DTR_REP_ACTIVE4"].ToString();

                DATE_LSTC = row["C16DTR_DATE_LSTC"].ToString();
                lstc_operator = row["C16DTR_lstc_operator"].ToString();
                DATE_ADD = row["C16DTR_DATE_ADD"].ToString();
                ADD_OPERATOR = row["C16DTR_ADD_OPERATOR"].ToString();
                SYS_Date_Range = string.Empty;
            }
        }


        #endregion

        #region Properties

        public string row_Type { get; set; }
        public string REF_FDATE { get; set; }
        public string REF_TDATE { get; set; }
        public string SYS_Date_Range { get; set; }
        public string REF_ACTIVE { get; set; }
        public string REP_FDATE1 { get; set; }
        public string REP_TDATE1 { get; set; }
        public string REP_ACTIVE1 { get; set; }
        public string REP_FDATE2 { get; set; }
        public string REP_TDATE2 { get; set; }
        public string REP_ACTIVE2 { get; set; }
        public string REP_FDATE3 { get; set; }
        public string REP_TDATE3 { get; set; }
        public string REP_ACTIVE3 { get; set; }
        public string REP_FDATE4 { get; set; }
        public string REP_TDATE4 { get; set; }
        public string REP_ACTIVE4 { get; set; }

        public string DATE_LSTC { get; set; }
        public string lstc_operator { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        #endregion

    }

    public class Funnel_RepEntity
    {
        #region Constructors

        public Funnel_RepEntity()
        {
            CASEACT_BRANCH = string.Empty;
            CASEACT_GROUP = string.Empty;
            SERVICEPLAN = string.Empty;
            CAMS_CODE = string.Empty;
            SNP_APP = string.Empty;
            App_Name = string.Empty;
            ACT_DATE = string.Empty;
            Worker = string.Empty;
            agency = string.Empty;
            dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            FUND = string.Empty;
            ENRL_DATE = string.Empty;
            ESTATUS = string.Empty;
            Site = string.Empty;
        }

        public Funnel_RepEntity(bool Intialize)
        {
            if (Intialize)
            {
                CASEACT_BRANCH = null;
                CASEACT_GROUP = null;
                SERVICEPLAN = null;
                CAMS_CODE = null;
                SNP_APP = null;
                App_Name = null;
                ACT_DATE = null;
                Worker = null;
                agency = null;
                dept = null;
                Prog = null;
                Year = null;
                FUND = null;
                ENRL_DATE = null;
                ESTATUS = null;
                Site = null;
                Cost = null;
            }
        }

        public Funnel_RepEntity(DataRow CASESPM, string CaMs)
        {
            if (CaMs == "CA")
            {
                DataRow row = CASESPM;
                CASEACT_BRANCH = row["CASEACT_BRANCH"].ToString();
                CASEACT_GROUP = row["CASEACT_GROUP"].ToString();
                SERVICEPLAN = row["SERVICEPLAN"].ToString();
                CAMS_CODE = row["CAMS_CODE"].ToString();
                SNP_APP = row["SNP_APP"].ToString();
                App_Name = row["App_Name"].ToString();
                ACT_DATE = row["ACT_DATE"].ToString();
                Worker = row["Worker"].ToString();
                agency = row["agency"].ToString();
                dept = row["dept"].ToString();
                Prog = row["Prog"].ToString();
                Year = row["Year"].ToString();
                FUND = row["FUND"].ToString();
                ENRL_DATE = row["ENRL_DATE"].ToString();
                ESTATUS = row["ESTATUS"].ToString();
                Site = row["S_Site"].ToString();
                Cost = row["CASEACT_COST"].ToString();
            }
            else
            {
                DataRow row = CASESPM;
                CASEACT_BRANCH = row["CASEACT_BRANCH"].ToString();
                CASEACT_GROUP = row["CASEACT_GROUP"].ToString();
                SERVICEPLAN = row["SERVICEPLAN"].ToString();
                CAMS_CODE = row["CAMS_CODE"].ToString();
                SNP_APP = row["SNP_APP"].ToString();
                App_Name = row["App_Name"].ToString();
                ACT_DATE = row["ACT_DATE"].ToString();
                Worker = row["Worker"].ToString();
                agency = row["agency"].ToString();
                dept = row["dept"].ToString();
                Prog = row["Prog"].ToString();
                Year = row["Year"].ToString();
                FUND = row["FUND"].ToString();
                ENRL_DATE = row["ENRL_DATE"].ToString();
                ESTATUS = row["ESTATUS"].ToString();
                Site = row["S_Site"].ToString();
                Cost = row["CASEACT_COST"].ToString();
            }
        }

        public Funnel_RepEntity(Funnel_RepEntity Entity)
        {
            if (Entity != null)
            {
                CASEACT_BRANCH = Entity.CASEACT_BRANCH;
                CASEACT_GROUP = Entity.CASEACT_GROUP;
                SERVICEPLAN = Entity.SERVICEPLAN;
                CAMS_CODE = Entity.CAMS_CODE;
                SNP_APP = Entity.SNP_APP;
                App_Name = Entity.App_Name;
                ACT_DATE = Entity.ACT_DATE;
                Worker = Entity.Worker;
                agency = Entity.agency;
                dept = Entity.dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                FUND = Entity.FUND;
                ENRL_DATE = Entity.ENRL_DATE;
                ESTATUS = Entity.ESTATUS;
                Site = Entity.Site;
                Cost = Entity.Cost;
            }
        }


        #endregion

        #region Properties

        public string CASEACT_BRANCH { get; set; }
        public string CASEACT_GROUP { get; set; }
        public string SERVICEPLAN { get; set; }
        public string CAMS_CODE { get; set; }
        public string SNP_APP { get; set; }
        public string App_Name { get; set; }
        public string ACT_DATE { get; set; }
        public string Worker { get; set; }
        public string agency { get; set; }
        public string dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string FUND { get; set; }
        public string ENRL_DATE { get; set; }
        public string ESTATUS { get; set; }
        public string Cost { get; set; }

        public string Site { get; set; }

        #endregion

    }


    public class CAVoucherEntity
    {
        #region Constructors

        public CAVoucherEntity()
        {
            Desc1 = string.Empty;
            VCode = string.Empty;
            Type = string.Empty;
            Code = string.Empty;
            Desc2 = string.Empty;
            FromDate = string.Empty;
            ToDate = string.Empty;
            Agency = string.Empty;
            Mode = string.Empty;
            //App_Name = string.Empty;
            //ACT_DATE = string.Empty;
            //Worker = string.Empty;
            //agency = string.Empty;
            //dept = string.Empty;
            //Prog = string.Empty;
            //Year = string.Empty;
            //FUND = string.Empty;
            //ENRL_DATE = string.Empty;
            //ESTATUS = string.Empty;
        }

        public CAVoucherEntity(bool Intialize)
        {
            if (Intialize)
            {
                Desc1 = null;
                VCode = null;
                Type = null;
                Code = null;
                Desc2 = null;
                FromDate = null;
                ToDate = null;
                Agency = null;
                Mode = null;
                //App_Name = null;
                //ACT_DATE = null;
                //Worker = null;
                //agency = null;
                //dept = null;
                //Prog = null;
                //Year = null;
                //FUND = null;
                //ENRL_DATE = null;
                //ESTATUS = null;
            }
        }

        public CAVoucherEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;
            Desc1 = row["Desc1"].ToString();
            VCode = row["Vcode"].ToString();
            Type = row["Type"].ToString();
            Code = row["Code"].ToString();
            Desc2 = row["Desc2"].ToString();
            FromDate = row["FromDate"].ToString();
            ToDate = row["ToDate"].ToString();
            Agency = row["AGENCY"].ToString();
            //App_Name = row["App_Name"].ToString();
            //ACT_DATE = row["ACT_DATE"].ToString();
            //Worker = row["Worker"].ToString();
            //agency = row["agency"].ToString();
            //dept = row["dept"].ToString();
            //Prog = row["Prog"].ToString();
            //Year = row["Year"].ToString();
            //FUND = row["FUND"].ToString();
            //ENRL_DATE = row["ENRL_DATE"].ToString();
            //ESTATUS = row["ESTATUS"].ToString();

        }

        public CAVoucherEntity(CAVoucherEntity Entity)
        {
            if (Entity != null)
            {
                Desc1 = Entity.Desc1;
                VCode = Entity.VCode;
                Type = Entity.Type;
                Code = Entity.Code;
                Desc2 = Entity.Desc2;
                FromDate = Entity.FromDate;
                ToDate = Entity.ToDate;
                Agency = Entity.Agency;

                //App_Name = Entity.App_Name;
                //ACT_DATE = Entity.ACT_DATE;
                //Worker = Entity.Worker;
                //agency = Entity.agency;
                //dept = Entity.dept;
                //Prog = Entity.Prog;
                //Year = Entity.Year;
                //FUND = Entity.FUND;
                //ENRL_DATE = Entity.ENRL_DATE;
                //ESTATUS = Entity.ESTATUS;

            }
        }


        #endregion

        #region Properties

        public string Desc1 { get; set; }
        public string VCode { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Desc2 { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Agency { get; set; }
        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class VouchDefEntity
    {
        #region Constructors

        public VouchDefEntity()
        {
            Vouch_Agency = string.Empty;
            Vouch_Type = string.Empty;
            Vouch_Seq = string.Empty;
            Vouch_Length = string.Empty;
            Vouch_Assoc = string.Empty;
            Vouch_Add_operator = string.Empty;
            Vouch_Date_Add = string.Empty;
            Vouch_LSTC_Operator = string.Empty;
            Vouch_Date_Lstc = string.Empty;
            Mode = string.Empty;

        }

        public VouchDefEntity(bool Intialize)
        {
            if (Intialize)
            {

                Vouch_Agency = null;
                Vouch_Type = null;
                Vouch_Seq = null;
                Vouch_Length = null;
                Vouch_Assoc = null;
                Vouch_Add_operator = null;
                Vouch_Date_Add = null;
                Vouch_LSTC_Operator = null;
                Vouch_Date_Lstc = null;
            }
        }

        public VouchDefEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;

            Vouch_Agency = row["Vouch_Agency"].ToString();
            Vouch_Type = row["Vouch_Type"].ToString();
            Vouch_Seq = row["Vouch_Seq"].ToString();
            Vouch_Length = row["Vouch_Length"].ToString();
            Vouch_Assoc = row["Vouch_Assoc"].ToString();
            Vouch_Add_operator = row["Vouch_Add_operator"].ToString();
            Vouch_Date_Add = row["Vouch_Date_Add"].ToString();
            Vouch_LSTC_Operator = row["Vouch_LSTC_Operator"].ToString();
            Vouch_Date_Lstc = row["Vouch_Date_Lstc"].ToString();


        }

        public VouchDefEntity(VouchDefEntity Entity)
        {
            if (Entity != null)
            {
                Vouch_Agency = Entity.Vouch_Agency;
                Vouch_Type = Entity.Vouch_Type;
                Vouch_Seq = Entity.Vouch_Seq;
                Vouch_Length = Entity.Vouch_Length;
                Vouch_Assoc = Entity.Vouch_Assoc;
                Vouch_Add_operator = Entity.Vouch_Add_operator;
                Vouch_Date_Add = Entity.Vouch_Date_Add;
                Vouch_LSTC_Operator = Entity.Vouch_LSTC_Operator;
                Vouch_Date_Lstc = Entity.Vouch_Date_Lstc;

            }
        }


        #endregion

        #region Properties

        public string Vouch_Agency { get; set; }
        public string Vouch_Type { get; set; }
        public string Vouch_Seq { get; set; }
        public string Vouch_Length { get; set; }
        public string Vouch_Assoc { get; set; }
        public string Vouch_Add_operator { get; set; }
        public string Vouch_Date_Add { get; set; }
        public string Vouch_LSTC_Operator { get; set; }
        public string Vouch_Date_Lstc { get; set; }
        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class TRIGSummaryEntity
    {
        #region Constructors

        public TRIGSummaryEntity()
        {
            Trig_Code = string.Empty;
            Trig_Date = string.Empty;
            Trig_Date_Seq = string.Empty;
            Trig_User = string.Empty;
            AppCnt = string.Empty;
            SPMCnt = string.Empty;
            ACTCnt = string.Empty;
            MSCnt = string.Empty;
            Trig_Time = string.Empty;
            Trig_Start_Time = string.Empty;
            //App_Name = string.Empty;
            //ACT_DATE = string.Empty;
            //Worker = string.Empty;
            //agency = string.Empty;
            //dept = string.Empty;
            //Prog = string.Empty;
            //Year = string.Empty;
            //FUND = string.Empty;
            //ENRL_DATE = string.Empty;
            //ESTATUS = string.Empty;
        }

        public TRIGSummaryEntity(bool Intialize)
        {
            if (Intialize)
            {
                Trig_Code = null;
                Trig_Date = null;
                Trig_Date_Seq = null;
                Trig_User = null;
                AppCnt = null;
                SPMCnt = null;
                ACTCnt = null;
                MSCnt = null;
                Trig_Time = null;
                Trig_Start_Time = null;
                //App_Name = null;
                //ACT_DATE = null;
                //Worker = null;
                //agency = null;
                //dept = null;
                //Prog = null;
                //Year = null;
                //FUND = null;
                //ENRL_DATE = null;
                //ESTATUS = null;
            }
        }

        public TRIGSummaryEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;
            Trig_Code = row["TRIG_CODE"].ToString();
            Trig_Date = row["TRIG_DATE"].ToString();
            Trig_Date_Seq = row["TRIG_DATE_SEQ"].ToString();
            Trig_User = row["TRIG_USER"].ToString();
            AppCnt = row["TRIG_NO_APPS"].ToString();
            SPMCnt = row["SPM_COUNT"].ToString();
            ACTCnt = row["CASEACT_COUNT"].ToString();
            MSCnt = row["CASEMS_COUNT"].ToString();
            Trig_Time = row["TRIG_END_TIME"].ToString();
            Trig_Start_Time = row["TRIG_START_TIME"].ToString();
            //App_Name = row["App_Name"].ToString();
            //ACT_DATE = row["ACT_DATE"].ToString();
            //Worker = row["Worker"].ToString();
            //agency = row["agency"].ToString();
            //dept = row["dept"].ToString();
            //Prog = row["Prog"].ToString();
            //Year = row["Year"].ToString();
            //FUND = row["FUND"].ToString();
            //ENRL_DATE = row["ENRL_DATE"].ToString();
            //ESTATUS = row["ESTATUS"].ToString();

        }

        public TRIGSummaryEntity(TRIGSummaryEntity Entity)
        {
            if (Entity != null)
            {
                Trig_Code = Entity.Trig_Code;
                Trig_Date = Entity.Trig_Date;
                Trig_Date_Seq = Entity.Trig_Date_Seq;
                Trig_User = Entity.Trig_User;
                AppCnt = Entity.AppCnt;
                SPMCnt = Entity.SPMCnt;
                ACTCnt = Entity.ACTCnt;
                MSCnt = Entity.MSCnt;
                Trig_Time = Entity.Trig_Time;
                Trig_Start_Time = Entity.Trig_Start_Time;
                //agency = Entity.agency;
                //dept = Entity.dept;
                //Prog = Entity.Prog;
                //Year = Entity.Year;
                //FUND = Entity.FUND;
                //ENRL_DATE = Entity.ENRL_DATE;
                //ESTATUS = Entity.ESTATUS;

            }
        }

        public TRIGSummaryEntity(string strcode, string strDate, string strTime, string strdateseq, string strTrigUser, string strAppcnt, string stringspmcnt, string strActInt, string strMscnt, string strType)
        {
            Trig_Code = strcode;
            Trig_Date = strDate;
            Trig_Date_Seq = strdateseq;
            Trig_User = strTrigUser;
            AppCnt = strAppcnt;
            SPMCnt = stringspmcnt;
            ACTCnt = strActInt;
            MSCnt = strMscnt;
            Trig_Type = strType;
            Trig_Time = strTime;
        }

        public TRIGSummaryEntity(string strcode, string strDate, string strTime, string endTime, string strdateseq, string strTrigUser, string strAppcnt, string stringspmcnt, string strActInt, string strMscnt, string strType)
        {
            Trig_Code = strcode;
            Trig_Date = strDate;
            Trig_Date_Seq = strdateseq;
            Trig_User = strTrigUser;
            AppCnt = strAppcnt;
            SPMCnt = stringspmcnt;
            ACTCnt = strActInt;
            MSCnt = strMscnt;
            Trig_Type = strType;
            Trig_Start_Time = strTime;
            Trig_Time = endTime;
        }


        #endregion

        #region Properties

        public string Trig_Code { get; set; }
        public string Trig_Date { get; set; }
        public string Trig_Date_Seq { get; set; }
        public string Trig_User { get; set; }
        public string AppCnt { get; set; }
        public string SPMCnt { get; set; }
        public string ACTCnt { get; set; }
        public string MSCnt { get; set; }
        public string Trig_Type { get; set; }
        public string Trig_Time { get; set; }
        public string Trig_Start_Time { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }


    public class TRIGDetailEntity
    {
        #region Constructors

        public TRIGDetailEntity()
        {
            AppNo = string.Empty;
            FName = string.Empty;
            LName = string.Empty;
            MName = string.Empty;
            SP_Code = string.Empty;
            Code = string.Empty;
            Desc = string.Empty;
            StartDate = string.Empty;
            //App_Name = string.Empty;
            //ACT_DATE = string.Empty;
            //Worker = string.Empty;
            agency = string.Empty;
            dept = string.Empty;
            Prog = string.Empty;
            Year = string.Empty;
            //FUND = string.Empty;
            //ENRL_DATE = string.Empty;
            //ESTATUS = string.Empty;
        }

        public TRIGDetailEntity(bool Intialize)
        {
            if (Intialize)
            {
                AppNo = null;
                FName = null;
                LName = null;
                MName = null;
                SP_Code = null;
                Code = null;
                Desc = null;
                StartDate = null;
                //App_Name = null;
                //ACT_DATE = null;
                //Worker = null;
                agency = null;
                dept = null;
                Prog = null;
                Year = null;
                //FUND = null;
                //ENRL_DATE = null;
                //ESTATUS = null;
            }
        }

        public TRIGDetailEntity(DataRow CASESPM, string Switch)
        {

            DataRow row = CASESPM;
            if (Switch == "S")
            {
                AppNo = row["MST_APP_NO"].ToString();
                FName = row["SNP_NAME_IX_FI"].ToString();
                LName = row["SNP_NAME_IX_LAST"].ToString();
                MName = row["SNP_NAME_IX_MI"].ToString();
                SP_Code = row["SPM_SERVICE_PLAN"].ToString();
                //Code = row["SPM_COUNT"].ToString();
                Desc = row["SP0_DESCRIPTION"].ToString();
                StartDate = row["SPM_STARTDATE"].ToString();
            }
            if (Switch == "A")
            {
                AppNo = row["MST_APP_NO"].ToString();
                FName = row["SNP_NAME_IX_FI"].ToString();
                LName = row["SNP_NAME_IX_LAST"].ToString();
                MName = row["SNP_NAME_IX_MI"].ToString();
                SP_Code = row["CASEACT_SERVICE_PLAN"].ToString();
                Code = row["CASEACT_ACTIVITY_CODE"].ToString();
                Desc = row["CA_DESC"].ToString();
                StartDate = row["CASEACT_ACTY_DATE"].ToString();
                SPM_SEQ = row["CASEACT_SPM_SEQ"].ToString();
                Branch = row["CASEACT_BRANCH"].ToString();
                Group = row["CASEACT_GROUP"].ToString();
            }
            if (Switch == "M")
            {
                AppNo = row["MST_APP_NO"].ToString();
                FName = row["SNP_NAME_IX_FI"].ToString();
                LName = row["SNP_NAME_IX_LAST"].ToString();
                MName = row["SNP_NAME_IX_MI"].ToString();
                SP_Code = row["CASEMS_SERVICE_PLAN"].ToString();
                Code = row["CASEMS_MS_CODE"].ToString();
                Desc = row["MS_DESC"].ToString();
                StartDate = row["CASEMS_DATE"].ToString();
                SPM_SEQ = row["CASEMS_SPM_SEQ"].ToString();
                Branch = row["CASEMS_BRANCH"].ToString();
                Group = row["CASEMS_GROUP"].ToString();
            }
            //App_Name = row["App_Name"].ToString();
            //ACT_DATE = row["ACT_DATE"].ToString();
            //Worker = row["Worker"].ToString();
            agency = row["MST_AGENCY"].ToString();
            dept = row["MST_DEPT"].ToString();
            Prog = row["MST_PROGRAM"].ToString();
            Year = row["MST_YEAR"].ToString();
            //FUND = row["FUND"].ToString();
            //ENRL_DATE = row["ENRL_DATE"].ToString();
            //ESTATUS = row["ESTATUS"].ToString();

        }

        public TRIGDetailEntity(TRIGDetailEntity Entity)
        {
            if (Entity != null)
            {
                AppNo = Entity.AppNo;
                FName = Entity.FName;
                LName = Entity.LName;
                MName = Entity.MName;
                SP_Code = Entity.SP_Code;
                Code = Entity.Code;
                Desc = Entity.Desc;
                StartDate = Entity.StartDate;
                SPM_SEQ = Entity.SPM_SEQ;
                Branch = Entity.Branch;
                Group = Entity.Group;
                agency = Entity.agency;
                dept = Entity.dept;
                Prog = Entity.Prog;
                Year = Entity.Year;
                //FUND = Entity.FUND;
                //ENRL_DATE = Entity.ENRL_DATE;
                //ESTATUS = Entity.ESTATUS;

            }
        }


        #endregion

        #region Properties

        public string AppNo { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string MName { get; set; }
        public string SP_Code { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string StartDate { get; set; }
        public string SPM_SEQ { get; set; }
        public string Branch { get; set; }
        public string Group { get; set; }
        public string agency { get; set; }
        public string dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class TRIGPARAMEntity
    {

        #region Constructors

        public TRIGPARAMEntity()
        {
            Trigger =
            HIE =
            YEAR =
            SITE =
            SITE_SWT =
            CASEWORKER =
            RESULT =
            OBO = LOADONLY = CUMMILATIVE = string.Empty;
        }


        public TRIGPARAMEntity(DataRow row)
        {
            if (row != null)
            {
                Trigger = row["TRIG_CODE"].ToString();
                HIE = row["HIE"].ToString();
                YEAR = row["YEAR"].ToString();
                SITE = row["SITE"].ToString();
                SITE_SWT = row["SITE_SWT"].ToString();
                CASEWORKER = row["CASEWORKER"].ToString();
                RESULT = row["RESULT"].ToString();
                OBO = row["OBO"].ToString();
                LOADONLY = row["LOADONLY"].ToString();
                CUMMILATIVE = row["CUMMILATIVE"].ToString();

            }
        }

        #endregion

        #region Properties

        public string Trigger { get; set; }
        public string HIE { get; set; }
        public string YEAR { get; set; }
        public string SITE { get; set; }
        public string SITE_SWT { get; set; }
        public string CASEWORKER { get; set; }
        public string RESULT { get; set; }
        public string OBO { get; set; }
        public string LOADONLY { get; set; }
        public string CUMMILATIVE { get; set; }

        #endregion

    }


    public class CAVDatesEntity
    {
        #region Constructors

        public CAVDatesEntity()
        {
            CAVD_ID = string.Empty;
            CAVD_AGENCY = string.Empty;
            CAVD_ACTIVE = string.Empty;
            CAVD_FDate = string.Empty;
            CAVD_TDate = string.Empty;
            CAVD_ADD_OPERATOR = string.Empty;
            CAVD_DATE_ADD = string.Empty;
            CAVD_LSTC_OPERATOR = string.Empty;
            CAVD_DATE_LSTC = string.Empty;
            Mode = string.Empty;

        }

        public CAVDatesEntity(bool Intialize)
        {
            if (Intialize)
            {

                CAVD_ID = null;
                CAVD_AGENCY = null;
                CAVD_ACTIVE = null;
                CAVD_FDate = null;
                CAVD_TDate = null;
                CAVD_ADD_OPERATOR = null;
                CAVD_DATE_ADD = null;
                CAVD_LSTC_OPERATOR = null;
                CAVD_DATE_LSTC = null;
            }
        }

        public CAVDatesEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;

            CAVD_ID = row["CAVD_ID"].ToString();
            CAVD_AGENCY = row["CAVD_AGENCY"].ToString();
            CAVD_ACTIVE = row["CAVD_ACTIVE"].ToString();
            CAVD_FDate = row["CAVD_FDate"].ToString();
            CAVD_TDate = row["CAVD_TDate"].ToString();
            CAVD_ADD_OPERATOR = row["CAVD_ADD_OPERATOR"].ToString();
            CAVD_DATE_ADD = row["CAVD_DATE_ADD"].ToString();
            CAVD_LSTC_OPERATOR = row["CAVD_LSTC_OPERATOR"].ToString();
            CAVD_DATE_LSTC = row["CAVD_DATE_LSTC"].ToString();


        }

        public CAVDatesEntity(CAVDatesEntity Entity)
        {
            if (Entity != null)
            {
                CAVD_ID = Entity.CAVD_ID;
                CAVD_AGENCY = Entity.CAVD_AGENCY;
                CAVD_ACTIVE = Entity.CAVD_ACTIVE;
                CAVD_FDate = Entity.CAVD_FDate;
                CAVD_TDate = Entity.CAVD_TDate;
                CAVD_ADD_OPERATOR = Entity.CAVD_ADD_OPERATOR;
                CAVD_DATE_ADD = Entity.CAVD_DATE_ADD;
                CAVD_LSTC_OPERATOR = Entity.CAVD_LSTC_OPERATOR;
                CAVD_DATE_LSTC = Entity.CAVD_DATE_LSTC;

            }
        }


        #endregion

        #region Properties

        public string CAVD_ID { get; set; }
        public string CAVD_AGENCY { get; set; }
        public string CAVD_ACTIVE { get; set; }
        public string CAVD_FDate { get; set; }
        public string CAVD_TDate { get; set; }
        public string CAVD_ADD_OPERATOR { get; set; }
        public string CAVD_DATE_ADD { get; set; }
        public string CAVD_LSTC_OPERATOR { get; set; }
        public string CAVD_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class CAVACNTFORMATEntity
    {
        #region Constructors

        public CAVACNTFORMATEntity()
        {
            CAVACCF_AGENCY = string.Empty;
            CAVACCF_SEQ = string.Empty;
            CAVACCF_Type = string.Empty;
            CAVACCF_Item = string.Empty;
            CAVACCF_Length = string.Empty;
            CAVACCF_ADD_OPERATOR = string.Empty;
            CAVACCF_DATE_ADD = string.Empty;
            CAVACCF_LSTC_OPERATOR = string.Empty;
            CAVACCF_DATE_LSTC = string.Empty;
            Mode = string.Empty;

        }

        public CAVACNTFORMATEntity(bool Intialize)
        {
            if (Intialize)
            {

                CAVACCF_AGENCY = null;
                CAVACCF_SEQ = null;
                CAVACCF_Type = null;
                CAVACCF_Item = null;
                CAVACCF_Length = null;
                CAVACCF_ADD_OPERATOR = null;
                CAVACCF_DATE_ADD = null;
                CAVACCF_LSTC_OPERATOR = null;
                CAVACCF_DATE_LSTC = null;
            }
        }

        public CAVACNTFORMATEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;

            CAVACCF_AGENCY = row["CAVACCF_AGENCY"].ToString();
            CAVACCF_SEQ = row["CAVACCF_SEQ"].ToString();
            CAVACCF_Type = row["CAVACCF_Type"].ToString();
            CAVACCF_Item = row["CAVACCF_Item"].ToString();
            CAVACCF_Length = row["CAVACCF_Length"].ToString();
            CAVACCF_ADD_OPERATOR = row["CAVACCF_ADD_OPERATOR"].ToString();
            CAVACCF_DATE_ADD = row["CAVACCF_DATE_ADD"].ToString();
            CAVACCF_LSTC_OPERATOR = row["CAVACCF_LSTC_OPERATOR"].ToString();
            CAVACCF_DATE_LSTC = row["CAVACCF_DATE_LSTC"].ToString();


        }

        public CAVACNTFORMATEntity(CAVACNTFORMATEntity Entity)
        {
            if (Entity != null)
            {
                CAVACCF_AGENCY = Entity.CAVACCF_AGENCY;
                CAVACCF_SEQ = Entity.CAVACCF_SEQ;
                CAVACCF_Type = Entity.CAVACCF_Type;
                CAVACCF_Item = Entity.CAVACCF_Item;
                CAVACCF_Length = Entity.CAVACCF_Length;
                CAVACCF_ADD_OPERATOR = Entity.CAVACCF_ADD_OPERATOR;
                CAVACCF_DATE_ADD = Entity.CAVACCF_DATE_ADD;
                CAVACCF_LSTC_OPERATOR = Entity.CAVACCF_LSTC_OPERATOR;
                CAVACCF_DATE_LSTC = Entity.CAVACCF_DATE_LSTC;

            }
        }


        #endregion

        #region Properties

        public string CAVACCF_AGENCY { get; set; }
        public string CAVACCF_SEQ { get; set; }
        public string CAVACCF_Type { get; set; }
        public string CAVACCF_Item { get; set; }
        public string CAVACCF_Length { get; set; }
        public string CAVACCF_ADD_OPERATOR { get; set; }
        public string CAVACCF_DATE_ADD { get; set; }
        public string CAVACCF_LSTC_OPERATOR { get; set; }
        public string CAVACCF_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class CAVASSOCEntity
    {
        #region Constructors

        public CAVASSOCEntity()
        {
            CAVASSOC_AGENCY = string.Empty;
            CAVASSOC_CAVD_ID = string.Empty;
            CAVASSOC_Type = string.Empty;
            CAVASSOC_Code = string.Empty;
            CAVASSOC_vCode = string.Empty;
            CAVASSOC_Desc = string.Empty;
            CAVASSOC_Remarks = string.Empty;
            CAVASSOC_LSTC_OPERATOR = string.Empty;
            CAVASSOC_DATE_LSTC = string.Empty;
            Mode = string.Empty;
            //App_Name = string.Empty;
            //ACT_DATE = string.Empty;
            //Worker = string.Empty;
            //agency = string.Empty;
            //dept = string.Empty;
            //Prog = string.Empty;
            //Year = string.Empty;
            //FUND = string.Empty;
            //ENRL_DATE = string.Empty;
            //ESTATUS = string.Empty;
        }

        public CAVASSOCEntity(bool Intialize)
        {
            if (Intialize)
            {
                CAVASSOC_AGENCY = null;
                CAVASSOC_CAVD_ID = null;
                CAVASSOC_Type = null;
                CAVASSOC_Code = null;
                CAVASSOC_vCode = null;
                CAVASSOC_Desc = null;
                CAVASSOC_Remarks = null;
                CAVASSOC_LSTC_OPERATOR = null;
                CAVASSOC_DATE_LSTC = null;
                Mode = null;

            }
        }

        public CAVASSOCEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;
            CAVASSOC_AGENCY = row["CAVASSOC_AGENCY"].ToString();
            CAVASSOC_CAVD_ID = row["CAVASSOC_CAVD_ID"].ToString();
            CAVASSOC_Type = row["CAVASSOC_Type"].ToString();
            CAVASSOC_Code = row["CAVASSOC_Code"].ToString();
            CAVASSOC_vCode = row["CAVASSOC_vCode"].ToString();
            CAVASSOC_Desc = row["CAVASSOC_Desc"].ToString();
            CAVASSOC_Remarks = row["CAVASSOC_Remarks"].ToString();
            CAVASSOC_LSTC_OPERATOR = row["CAVASSOC_LSTC_OPERATOR"].ToString();
            CAVASSOC_DATE_LSTC = row["CAVASSOC_DATE_LSTC"].ToString();
            //App_Name = row["App_Name"].ToString();
            //ACT_DATE = row["ACT_DATE"].ToString();
            //Worker = row["Worker"].ToString();
            //agency = row["agency"].ToString();
            //dept = row["dept"].ToString();
            //Prog = row["Prog"].ToString();
            //Year = row["Year"].ToString();
            //FUND = row["FUND"].ToString();
            //ENRL_DATE = row["ENRL_DATE"].ToString();
            //ESTATUS = row["ESTATUS"].ToString();

        }

        public CAVASSOCEntity(CAVASSOCEntity Entity)
        {
            if (Entity != null)
            {
                CAVASSOC_AGENCY = Entity.CAVASSOC_AGENCY;
                CAVASSOC_CAVD_ID = Entity.CAVASSOC_CAVD_ID;
                CAVASSOC_Type = Entity.CAVASSOC_Type;
                CAVASSOC_Code = Entity.CAVASSOC_Code;
                CAVASSOC_vCode = Entity.CAVASSOC_vCode;
                CAVASSOC_Desc = Entity.CAVASSOC_Desc;
                CAVASSOC_Remarks = Entity.CAVASSOC_Remarks;
                CAVASSOC_LSTC_OPERATOR = Entity.CAVASSOC_LSTC_OPERATOR;
                CAVASSOC_DATE_LSTC = Entity.CAVASSOC_DATE_LSTC;

                //App_Name = Entity.App_Name;
                //ACT_DATE = Entity.ACT_DATE;
                //Worker = Entity.Worker;
                //agency = Entity.agency;
                //dept = Entity.dept;
                //Prog = Entity.Prog;
                //Year = Entity.Year;
                //FUND = Entity.FUND;
                //ENRL_DATE = Entity.ENRL_DATE;
                //ESTATUS = Entity.ESTATUS;

            }
        }


        #endregion

        #region Properties

        public string CAVASSOC_AGENCY { get; set; }
        public string CAVASSOC_CAVD_ID { get; set; }
        public string CAVASSOC_Type { get; set; }
        public string CAVASSOC_Code { get; set; }
        public string CAVASSOC_vCode { get; set; }
        public string CAVASSOC_Desc { get; set; }
        public string CAVASSOC_Remarks { get; set; }
        public string CAVASSOC_LSTC_OPERATOR { get; set; }
        public string CAVASSOC_DATE_LSTC { get; set; }
        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }


    public class VoucherEntity
    {
        #region Constructors

        public VoucherEntity()
        {
            CAVASSOC_AGENCY = string.Empty;
            CAVASSOC_CAVD_ID = string.Empty;
            CAVASSOC_Type = string.Empty;
            CAVASSOC_Code = string.Empty;
            CAVASSOC_vCode = string.Empty;
            CAVASSOC_Desc = string.Empty;
            CAVASSOC_Remarks = string.Empty;
            CAVASSOC_LSTC_OPERATOR = string.Empty;
            CAVASSOC_DATE_LSTC = string.Empty;

            CAVACCF_AGENCY = string.Empty;
            CAVACCF_SEQ = string.Empty;
            CAVACCF_Type = string.Empty;
            CAVACCF_Item = string.Empty;
            CAVACCF_Length = string.Empty;
            CAVACCF_ADD_OPERATOR = string.Empty;
            CAVACCF_DATE_ADD = string.Empty;
            CAVACCF_LSTC_OPERATOR = string.Empty;
            CAVACCF_DATE_LSTC = string.Empty;

            CAVD_ID = string.Empty;
            CAVD_AGENCY = string.Empty;
            CAVD_ACTIVE = string.Empty;
            CAVD_FDate = string.Empty;
            CAVD_TDate = string.Empty;
            CAVD_ADD_OPERATOR = string.Empty;
            CAVD_DATE_ADD = string.Empty;
            CAVD_LSTC_OPERATOR = string.Empty;
            CAVD_DATE_LSTC = string.Empty;



            Mode = string.Empty;
            //App_Name = string.Empty;
            //ACT_DATE = string.Empty;
            //Worker = string.Empty;
            //agency = string.Empty;
            //dept = string.Empty;
            //Prog = string.Empty;
            //Year = string.Empty;
            //FUND = string.Empty;
            //ENRL_DATE = string.Empty;
            //ESTATUS = string.Empty;
        }

        public VoucherEntity(bool Intialize)
        {
            if (Intialize)
            {
                CAVASSOC_AGENCY = null;
                CAVASSOC_CAVD_ID = null;
                CAVASSOC_Type = null;
                CAVASSOC_Code = null;
                CAVASSOC_vCode = null;
                CAVASSOC_Desc = null;
                CAVASSOC_Remarks = null;
                CAVASSOC_LSTC_OPERATOR = null;
                CAVASSOC_DATE_LSTC = null;

                CAVACCF_AGENCY = null;
                CAVACCF_SEQ = null;
                CAVACCF_Type = null;
                CAVACCF_Item = null;
                CAVACCF_Length = null;
                CAVACCF_ADD_OPERATOR = null;
                CAVACCF_DATE_ADD = null;
                CAVACCF_LSTC_OPERATOR = null;
                CAVACCF_DATE_LSTC = null;

                CAVD_ID = null;
                CAVD_AGENCY = null;
                CAVD_ACTIVE = null;
                CAVD_FDate = null;
                CAVD_TDate = null;
                CAVD_ADD_OPERATOR = null;
                CAVD_DATE_ADD = null;
                CAVD_LSTC_OPERATOR = null;
                CAVD_DATE_LSTC = null;

                Mode = null;

            }
        }

        public VoucherEntity(DataRow CASESPM)
        {

            DataRow row = CASESPM;
            CAVASSOC_AGENCY = row["CAVASSOC_AGENCY"].ToString();
            CAVASSOC_CAVD_ID = row["CAVASSOC_CAVD_ID"].ToString();
            CAVASSOC_Type = row["CAVASSOC_Type"].ToString();
            CAVASSOC_Code = row["CAVASSOC_Code"].ToString();
            CAVASSOC_vCode = row["CAVASSOC_vCode"].ToString();
            CAVASSOC_Desc = row["CAVASSOC_Desc"].ToString();
            CAVASSOC_Remarks = row["CAVASSOC_Remarks"].ToString();
            CAVASSOC_LSTC_OPERATOR = row["CAVASSOC_LSTC_OPERATOR"].ToString();
            CAVASSOC_DATE_LSTC = row["CAVASSOC_DATE_LSTC"].ToString();

            CAVACCF_AGENCY = row["CAVACCF_AGENCY"].ToString();
            CAVACCF_SEQ = row["CAVACCF_SEQ"].ToString();
            CAVACCF_Type = row["CAVACCF_Type"].ToString();
            CAVACCF_Item = row["CAVACCF_Item"].ToString();
            CAVACCF_Length = row["CAVACCF_Length"].ToString();
            CAVACCF_ADD_OPERATOR = row["CAVACCF_ADD_OPERATOR"].ToString();
            CAVACCF_DATE_ADD = row["CAVACCF_DATE_ADD"].ToString();
            CAVACCF_LSTC_OPERATOR = row["CAVACCF_LSTC_OPERATOR"].ToString();
            CAVACCF_DATE_LSTC = row["CAVACCF_DATE_LSTC"].ToString();

            CAVD_ID = row["CAVD_ID"].ToString();
            CAVD_AGENCY = row["CAVD_AGENCY"].ToString();
            CAVD_ACTIVE = row["CAVD_ACTIVE"].ToString();
            CAVD_FDate = row["CAVD_FDate"].ToString();
            CAVD_TDate = row["CAVD_TDate"].ToString();
            CAVD_ADD_OPERATOR = row["CAVD_ADD_OPERATOR"].ToString();
            CAVD_DATE_ADD = row["CAVD_DATE_ADD"].ToString();
            CAVD_LSTC_OPERATOR = row["CAVD_LSTC_OPERATOR"].ToString();
            CAVD_DATE_LSTC = row["CAVD_DATE_LSTC"].ToString();


            //App_Name = row["App_Name"].ToString();
            //ACT_DATE = row["ACT_DATE"].ToString();
            //Worker = row["Worker"].ToString();
            //agency = row["agency"].ToString();
            //dept = row["dept"].ToString();
            //Prog = row["Prog"].ToString();
            //Year = row["Year"].ToString();
            //FUND = row["FUND"].ToString();
            //ENRL_DATE = row["ENRL_DATE"].ToString();
            //ESTATUS = row["ESTATUS"].ToString();

        }

        public VoucherEntity(VoucherEntity Entity)
        {
            if (Entity != null)
            {
                CAVASSOC_AGENCY = Entity.CAVASSOC_AGENCY;
                CAVASSOC_CAVD_ID = Entity.CAVASSOC_CAVD_ID;
                CAVASSOC_Type = Entity.CAVASSOC_Type;
                CAVASSOC_Code = Entity.CAVASSOC_Code;
                CAVASSOC_vCode = Entity.CAVASSOC_vCode;
                CAVASSOC_Desc = Entity.CAVASSOC_Desc;
                CAVASSOC_Remarks = Entity.CAVASSOC_Remarks;
                CAVASSOC_LSTC_OPERATOR = Entity.CAVASSOC_LSTC_OPERATOR;
                CAVASSOC_DATE_LSTC = Entity.CAVASSOC_DATE_LSTC;

                CAVACCF_AGENCY = Entity.CAVACCF_AGENCY;
                CAVACCF_SEQ = Entity.CAVACCF_SEQ;
                CAVACCF_Type = Entity.CAVACCF_Type;
                CAVACCF_Item = Entity.CAVACCF_Item;
                CAVACCF_Length = Entity.CAVACCF_Length;
                CAVACCF_ADD_OPERATOR = Entity.CAVACCF_ADD_OPERATOR;
                CAVACCF_DATE_ADD = Entity.CAVACCF_DATE_ADD;
                CAVACCF_LSTC_OPERATOR = Entity.CAVACCF_LSTC_OPERATOR;
                CAVACCF_DATE_LSTC = Entity.CAVACCF_DATE_LSTC;

                CAVD_ID = Entity.CAVD_ID;
                CAVD_AGENCY = Entity.CAVD_AGENCY;
                CAVD_ACTIVE = Entity.CAVD_ACTIVE;
                CAVD_FDate = Entity.CAVD_FDate;
                CAVD_TDate = Entity.CAVD_TDate;
                CAVD_ADD_OPERATOR = Entity.CAVD_ADD_OPERATOR;
                CAVD_DATE_ADD = Entity.CAVD_DATE_ADD;
                CAVD_LSTC_OPERATOR = Entity.CAVD_LSTC_OPERATOR;
                CAVD_DATE_LSTC = Entity.CAVD_DATE_LSTC;

                //App_Name = Entity.App_Name;
                //ACT_DATE = Entity.ACT_DATE;
                //Worker = Entity.Worker;
                //agency = Entity.agency;
                //dept = Entity.dept;
                //Prog = Entity.Prog;
                //Year = Entity.Year;
                //FUND = Entity.FUND;
                //ENRL_DATE = Entity.ENRL_DATE;
                //ESTATUS = Entity.ESTATUS;

            }
        }


        #endregion

        #region Properties

        public string CAVASSOC_AGENCY { get; set; }
        public string CAVASSOC_CAVD_ID { get; set; }
        public string CAVASSOC_Type { get; set; }
        public string CAVASSOC_Code { get; set; }
        public string CAVASSOC_vCode { get; set; }
        public string CAVASSOC_Desc { get; set; }
        public string CAVASSOC_Remarks { get; set; }
        public string CAVASSOC_LSTC_OPERATOR { get; set; }
        public string CAVASSOC_DATE_LSTC { get; set; }

        public string CAVD_ID { get; set; }
        public string CAVD_AGENCY { get; set; }
        public string CAVD_ACTIVE { get; set; }
        public string CAVD_FDate { get; set; }
        public string CAVD_TDate { get; set; }
        public string CAVD_ADD_OPERATOR { get; set; }
        public string CAVD_DATE_ADD { get; set; }
        public string CAVD_LSTC_OPERATOR { get; set; }
        public string CAVD_DATE_LSTC { get; set; }

        public string CAVACCF_AGENCY { get; set; }
        public string CAVACCF_SEQ { get; set; }
        public string CAVACCF_Type { get; set; }
        public string CAVACCF_Item { get; set; }
        public string CAVACCF_Length { get; set; }
        public string CAVACCF_ADD_OPERATOR { get; set; }
        public string CAVACCF_DATE_ADD { get; set; }
        public string CAVACCF_LSTC_OPERATOR { get; set; }
        public string CAVACCF_DATE_LSTC { get; set; }

        public string Mode { get; set; }
        //public string App_Name { get; set; }
        //public string ACT_DATE { get; set; }
        //public string Worker { get; set; }
        //public string agency { get; set; }
        //public string dept { get; set; }
        //public string Prog { get; set; }
        //public string Year { get; set; }
        //public string FUND { get; set; }
        //public string ENRL_DATE { get; set; }
        //public string ESTATUS { get; set; }


        #endregion

    }

    public class AGCYPARTEntity
    {
        #region Constructors

        public AGCYPARTEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;
            Name = string.Empty;
            Date = string.Empty;
            //IndexBy = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Phone = string.Empty;
            Active = string.Empty;
            County = string.Empty;
            From_Hrs = string.Empty;
            To_Hrs = string.Empty;

            Website = string.Empty;
            Docs = string.Empty;
            //Notes = string.Empty;
            //ProgDiv = string.Empty;
            ORGType = string.Empty;
            Target = string.Empty;

            Boutique = string.Empty;
            AGYP_FORMS_STATUS = string.Empty;

            Counties_Served = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            ServicePlans = string.Empty;

            RefAsoc = string.Empty;
        }

        public AGCYPARTEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                Name = null;
                Date = null;
                Street = null;
                City = null;
                State = null;
                Zip = null;
                Phone = null;
                Active = null;
                County = null;
                From_Hrs = null;
                To_Hrs = null;

                Website = null;
                Docs = null;
                //Notes = null;
                //ProgDiv = null;
                ORGType = null;
                Target = null;

                Boutique = null;
                AGYP_FORMS_STATUS = null;

                Counties_Served = null;

                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
                ServicePlans = null;

                RefAsoc = null;
            }

        }


        public AGCYPARTEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Code = row["AGYP_CODE"].ToString();
                Name = row["AGYP_NAME"].ToString();
                Date = row["AGYP_Date"].ToString();
                Street = row["AGYP_Street"].ToString();
                City = row["AGYP_City"].ToString();
                State = row["AGYP_State"].ToString();
                Zip = row["AGYP_Zip"].ToString();
                Phone = row["AGYP_Phone"].ToString();
                Active = row["AGYP_Active"].ToString();
                County = row["AGYP_COUNTY"].ToString();
                From_Hrs = row["AGYP_FROM_HRS"].ToString();
                To_Hrs = row["AGYP_TO_HRS"].ToString();


                Website = row["AGYP_Website"].ToString();
                Docs = row["AGYP_Docs"].ToString();
                //Notes = row["AGYP_Notes"].ToString();
                //ProgDiv = row["AGYP_PROG_DIV"].ToString();
                ORGType = row["AGYP_ORG_TYPE"].ToString();
                Target = row["AGYP_Target"].ToString();

                Boutique = row["AGYP_BOUTIQUE"].ToString();
                AGYP_FORMS_STATUS = row["AGYP_FORMS_STATUS"].ToString();

                Counties_Served = row["AGYP_COUNTIES_SERVED"].ToString();

                Lstc_Date = row["AGYP_Date_Lstc"].ToString();
                Lsct_Operator = row["AGYP_Lstc_Operator"].ToString();
                Add_Date = row["AGYP_Date_Add"].ToString();
                Add_Operator = row["AGYP_Add_Operator"].ToString();
                ServicePlans = row["AGYP_SERPLANS"].ToString();

                RefAsoc = row["AGYP_REF_ASOC"].ToString();
            }
        }

        public AGCYPARTEntity(AGCYPARTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Name = Entity.Name;
                Date = Entity.Date;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                Phone = Entity.Phone;
                Active = Entity.Active;
                County = Entity.County;
                From_Hrs = Entity.From_Hrs;
                To_Hrs = Entity.To_Hrs;

                Website = Entity.Website;
                Docs = Entity.Docs;
                //Notes = Entity.Notes;
                //ProgDiv = Entity.ProgDiv;
                ORGType = Entity.ORGType;
                Target = Entity.Target;

                Boutique = Entity.Boutique;
                AGYP_FORMS_STATUS = Entity.AGYP_FORMS_STATUS;

                Counties_Served = Entity.Counties_Served;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                ServicePlans = Entity.ServicePlans;

                RefAsoc = Entity.RefAsoc;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        //public string IndexBy { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        //public string Zip_Plus { get; set; }
        //public string Area { get; set; }
        //public string Excgange { get; set; }
        public string Phone { get; set; }
        public string Active { get; set; }
        //public string Cont_Fname { get; set; }
        //public string Cont_Lname { get; set; }
        //public string Cont_Area { get; set; }
        //public string Cont_Exchange { get; set; }
        //public string Cont_TelNO { get; set; }
        //public string Long_Distance { get; set; }
        //public string Fax_Exchange { get; set; }
        //public string Fax_Telno { get; set; }
        //public string Outside { get; set; }
        //public string Fax_Area { get; set; }
        //public string NameIndex { get; set; }


        //public string Category { get; set; }
        public string County { get; set; }
        public string From_Hrs { get; set; }
        public string To_Hrs { get; set; }
        //public string Sec { get; set; }
        public string Website { get; set; }
        public string Docs { get; set; }
        //public string Notes { get; set; }
        //public string ProgDiv { get; set; }
        public string ORGType { get; set; }
        public string Target { get; set; }

        public string Boutique { get; set; }
        public string AGYP_FORMS_STATUS { get; set; }

        public string Counties_Served { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string ServicePlans { get; set; }


        public string RefAsoc
        {
            get;
            set;
        }

        #endregion

    }

    public class AGCYBOTIQEntity
    {
        #region Constructors

        public AGCYBOTIQEntity()
        {
            Rec_Type = string.Empty;
            Code = string.Empty;

            Req_Date = string.Empty;
            Req_Comp_Date = string.Empty;
            Status = string.Empty;
            Footage = string.Empty;
            Shared = string.Empty;
            Shared_desc = string.Empty;
            ItemsNeeded = string.Empty;
            Population = string.Empty;
            Free_Lunch = string.Empty;
            Poverty = string.Empty;
            City = string.Empty;

            County = string.Empty;
            Percentage = string.Empty;
            InvoiceForm = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

        }

        public AGCYBOTIQEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                Code = null;
                Req_Date = null;
                Req_Comp_Date = null;
                Status = null;
                Footage = null;
                Shared = null;
                Shared_desc = null;
                ItemsNeeded = null;
                Population = null;
                Free_Lunch = null;
                Poverty = null;
                City = null;

                County = null;
                Percentage = null;
                InvoiceForm = null;

                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
            }

        }


        public AGCYBOTIQEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                Code = row["AGYB_PART_CODE"].ToString();
                Req_Date = row["AGYB_REQ_DATE"].ToString();
                Req_Comp_Date = row["AGYB_REQ_COMP_DATE"].ToString();
                Status = row["AGYB_STATUS"].ToString();
                Footage = row["AGYB_SQ_FOOTAGE"].ToString();
                Shared = row["AGYB_SP_SHARED"].ToString();
                Shared_desc = row["AGYB_SP_SHARED_DESC"].ToString();
                ItemsNeeded = row["AGYB_ITEMS_NEEDED"].ToString();
                Population = row["AGYB_POPULATION"].ToString();
                Free_Lunch = row["AGYB_FR_LUNCH"].ToString();
                Poverty = row["AGYB_POVERTY"].ToString();
                City = row["AGYB_CITY"].ToString();

                County = row["AGYB_COUNTY"].ToString();
                Percentage = row["AGYB_PERCENTAGE"].ToString();
                InvoiceForm = row["AGYB_INV_FORM"].ToString();


                Lstc_Date = row["AGYB_DATE_LSTC"].ToString();
                Lsct_Operator = row["AGYB_LSTC_OPERATOR"].ToString();
                Add_Date = row["AGYB_DATE_ADD"].ToString();
                Add_Operator = row["AGYB_ADD_OPERATOR"].ToString();
            }
        }

        public AGCYBOTIQEntity(AGCYBOTIQEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Req_Date = Entity.Req_Date;
                Req_Comp_Date = Entity.Req_Comp_Date;
                Status = Entity.Status;
                Footage = Entity.Footage;
                Shared = Entity.Shared;
                Shared_desc = Entity.Shared_desc;
                ItemsNeeded = Entity.ItemsNeeded;
                Population = Entity.Population;
                Free_Lunch = Entity.Free_Lunch;
                Poverty = Entity.Poverty;
                City = Entity.City;

                County = Entity.County;
                Percentage = Entity.Percentage;
                InvoiceForm = Entity.InvoiceForm;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string Code { get; set; }
        public string Req_Date { get; set; }
        public string Req_Comp_Date { get; set; }

        public string Status { get; set; }
        public string Footage { get; set; }
        public string Shared { get; set; }
        public string Shared_desc { get; set; }
        public string ItemsNeeded { get; set; }
        public string Population { get; set; }
        public string Free_Lunch { get; set; }
        public string Poverty { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Percentage { get; set; }
        public string InvoiceForm { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        #endregion

    }

    public class AGCYREPEntity
    {
        #region Constructors

        public AGCYREPEntity()
        {
            Rec_Type = string.Empty;
            PartCode = string.Empty;
            RepCode = string.Empty;
            FName = string.Empty;
            LName = string.Empty;
            Position = string.Empty;
            Prog = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            //Zip_Plus = Entity.Zip_Plus;
            //Area = Entity.Area;
            //Excgange = Entity.Excgange;
            Phone1 = string.Empty;
            Phone2 = string.Empty;
            Ext = string.Empty;
            Email = string.Empty;
            Phn1_NA = string.Empty;
            Phn2_NA = string.Empty;
            Email_NA = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            AGYR_BOUT_CONTACT = string.Empty;
            AGYR_AGY_CONTACT = string.Empty;

            AddRepcode = string.Empty;

            AGYR_REP_ACTIVE = string.Empty;
            AGYR_REP_REQLEAD = string.Empty;
            AGYP_Name = string.Empty;

        }

        public AGCYREPEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                PartCode = null;
                RepCode = null;
                FName = null;
                LName = null;
                Position = null;
                Prog = null;
                Street = null;
                City = null;
                State = null;
                Zip = null;
                //Zip_Plus = Entity.Zip_Plus;
                //Area = Entity.Area;
                //Excgange = Entity.Excgange;
                Phone1 = null;
                Phone2 = null;
                Ext = null;
                Email = null;
                Phn1_NA = null;
                Phn2_NA = null;
                Email_NA = null;

                AGYR_BOUT_CONTACT = null;
                AGYR_AGY_CONTACT = null;

                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
                AGYR_REP_ACTIVE = null;
                AGYR_REP_REQLEAD = null;
                AGYP_Name = null;
            }

        }


        public AGCYREPEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                PartCode = row["AGYR_PART_CODE"].ToString();
                RepCode = row["AGYR_REP_CODE"].ToString();
                FName = row["AGYR_REP_FNAME"].ToString();
                LName = row["AGYR_REP_LNAME"].ToString();
                Position = row["AGYR_REP_POS"].ToString();
                Prog = row["AGYR_REP_PROG"].ToString();
                Street = row["AGYR_REP_Street"].ToString();
                City = row["AGYR_REP_City"].ToString();
                State = row["AGYR_REP_State"].ToString();
                Zip = row["AGYR_REP_Zip"].ToString();
                //Zip_Plus = Entity.Zip_Plus;
                //Area = Entity.Area;
                //Excgange = Entity.Excgange;
                Phone1 = row["AGYR_REP_PHONE1"].ToString();
                Phone2 = row["AGYR_REP_PHONE2"].ToString();
                Ext = row["AGYR_REP_Phone_Ext"].ToString();
                Email = row["AGYR_REP_EMAIL"].ToString();
                Phn1_NA = row["AGYR_REP_PHN1_NA"].ToString();
                Phn2_NA = row["AGYR_REP_PHN2_NA"].ToString();
                Email_NA = row["AGYR_REP_EMAIL_NA"].ToString();
                AGYR_AGY_CONTACT = row["AGYR_AGY_CONTACT"].ToString();


                AGYR_BOUT_CONTACT = row["AGYR_BOUT_CONTACT"].ToString();

                Lstc_Date = row["AGYR_REP_Date_Lstc"].ToString();
                Lsct_Operator = row["AGYR_REP_Lstc_Operator"].ToString();
                Add_Date = row["AGYR_REP_Date_Add"].ToString();
                Add_Operator = row["AGYR_REP_Add_Operator"].ToString();

                AGYR_REP_ACTIVE = row["AGYR_REP_ACTIVE"].ToString();
                AGYR_REP_REQLEAD = row["AGYR_REP_REQLEAD"].ToString();
                AGYP_Name = row["AGYP_NAME"].ToString();
            }
        }

        public AGCYREPEntity(AGCYREPEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                PartCode = Entity.PartCode;
                RepCode = Entity.RepCode;
                FName = Entity.FName;
                LName = Entity.LName;
                Position = Entity.Position;
                Prog = Entity.Prog;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                //Zip_Plus = Entity.Zip_Plus;
                //Area = Entity.Area;
                //Excgange = Entity.Excgange;
                Phone1 = Entity.Phone1;
                Phone2 = Entity.Phone2;
                Ext = Entity.Ext;
                Email = Entity.Email;

                Phn1_NA = Entity.Phn1_NA;
                Phn2_NA = Entity.Phn2_NA;
                Email_NA = Entity.Email_NA;


                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                AGYR_BOUT_CONTACT = Entity.AGYR_BOUT_CONTACT;
                AGYR_AGY_CONTACT = Entity.AGYR_AGY_CONTACT;
                AddRepcode = Entity.AddRepcode;
                AGYR_REP_ACTIVE = Entity.AGYR_REP_ACTIVE;
                AGYR_REP_REQLEAD = Entity.AGYR_REP_REQLEAD;
                AGYP_Name=Entity.AGYP_Name;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string PartCode { get; set; }
        public string RepCode { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Position { get; set; }
        public string Prog { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Ext { get; set; }
        public string Email { get; set; }
        public string Phn1_NA { get; set; }
        public string Phn2_NA { get; set; }
        public string Email_NA { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        public string AGYR_BOUT_CONTACT { get; set; }
        public string AGYR_AGY_CONTACT { get; set; }

        public string AGYR_REP_ACTIVE { get; set; }
        public string AGYR_REP_REQLEAD { get; set; }

        public string AGYP_Name { get; set; }

        public string AddRepcode { get; set; }


        #endregion

    }

    public class AGCYSUBEntity
    {
        #region Constructors

        public AGCYSUBEntity()
        {
            Rec_Type = string.Empty;
            PartCode = string.Empty;
            SubCode = string.Empty;
            SubLocation = string.Empty;
            HN = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            AddSubCode = string.Empty;

        }

        public AGCYSUBEntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                PartCode = null;
                SubCode = null;
                SubLocation = null;
                HN = null;
                Street = null;
                City = null;
                State = null;
                Zip = null;


                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
            }

        }


        public AGCYSUBEntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                PartCode = row["AGYS_PART_CODE"].ToString();
                SubCode = row["AGYS_SUB_CODE"].ToString();
                SubLocation = row["AGYS_SUB_LOCATION"].ToString();
                HN = row["AGYS_SUB_HN"].ToString();
                Street = row["AGYS_SUB_Street"].ToString();
                City = row["AGYS_SUB_CITY"].ToString();
                State = row["AGYS_SUB_STATE"].ToString();
                Zip = row["AGYS_SUB_ZIP"].ToString();

                Lstc_Date = row["AGYS_SUB_Date_Lstc"].ToString();
                Lsct_Operator = row["AGYS_SUB_Lstc_Operator"].ToString();
                Add_Date = row["AGYS_SUB_Date_Add"].ToString();
                Add_Operator = row["AGYS_SUB_Add_Operator"].ToString();
            }
        }

        public AGCYSUBEntity(AGCYSUBEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                PartCode = Entity.PartCode;
                SubCode = Entity.SubCode;
                SubLocation = Entity.SubLocation;
                HN = Entity.HN;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;


                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                AddSubCode = Entity.AddSubCode;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string PartCode { get; set; }
        public string SubCode { get; set; }
        public string SubLocation { get; set; }
        public string HN { get; set; }


        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        public string AddSubCode { get; set; }

        #endregion

    }

    public class AGCYSEREntity
    {
        #region Constructors

        public AGCYSEREntity()
        {
            Rec_Type = string.Empty;
            PartCode = string.Empty;
            SerCode = string.Empty;
            Category = string.Empty;
            Service = string.Empty;
            Location = string.Empty;

            SubLocation = string.Empty;

            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            ServiceDetails = string.Empty;

            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            AddSubCode = string.Empty;

        }

        public AGCYSEREntity(bool Initialize)
        {
            if (Initialize)
            {
                Rec_Type = null;
                PartCode = null;
                SerCode = null;
                Category = null;
                Service = null;
                Location = null;
                SubLocation = null;
                Street = null;
                City = null;
                State = null;
                Zip = null;
                ServiceDetails = null;

                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;
            }

        }


        public AGCYSEREntity(DataRow CASESPM)
        {
            if (CASESPM != null)
            {
                DataRow row = CASESPM;

                Rec_Type = "U";
                PartCode = row["AGYS_PART_CODE"].ToString();
                SerCode = row["AGYS_SER_CODE"].ToString();
                Category = row["AGYS_CATEGORY"].ToString();
                Service = row["AGYS_SERVICE"].ToString();
                Location = row["AGYS_SUBLOC"].ToString();
                SubLocation = row["AGYS_SUB_LOCATION"].ToString();
                Street = row["AGYS_Street"].ToString();
                City = row["AGYS_CITY"].ToString();
                State = row["AGYS_STATE"].ToString();
                Zip = row["AGYS_ZIP"].ToString();
                ServiceDetails = row["AGYS_SER_Details"].ToString();

                Lstc_Date = row["AGYS_Date_Lstc"].ToString();
                Lsct_Operator = row["AGYS_Lstc_Operator"].ToString();
                Add_Date = row["AGYS_Date_Add"].ToString();
                Add_Operator = row["AGYS_Add_Operator"].ToString();
            }
        }

        public AGCYSEREntity(AGCYSEREntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                PartCode = Entity.PartCode;
                SerCode = Entity.SerCode;
                Category = Entity.Category;
                Service = Entity.Service;
                Location = Entity.Location;

                SubLocation = Entity.SubLocation;
                Street = Entity.Street;
                City = Entity.City;
                State = Entity.State;
                Zip = Entity.Zip;
                ServiceDetails = Entity.ServiceDetails;

                Lstc_Date = Entity.Lstc_Date;
                Lsct_Operator = Entity.Lsct_Operator;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;

                AddSubCode = Entity.AddSubCode;
            }
        }


        #endregion

        #region Properties


        public string Rec_Type { get; set; }
        public string PartCode { get; set; }
        public string SerCode { get; set; }

        public string Category { get; set; }
        public string Service { get; set; }
        public string Location { get; set; }
        public string SubLocation { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ServiceDetails { get; set; }

        public string Lstc_Date { get; set; }
        public string Lsct_Operator { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }

        public string AddSubCode { get; set; }

        #endregion

    }

    public class AGCYPARTQUESEntity
    {
        #region Constructors

        public AGCYPARTQUESEntity()
        {
            Mode = string.Empty;
            AGYQ_CODE = string.Empty;
            //AGYQ_SEQ = string.Empty; 
            AGYQ_ACTIVE = string.Empty;
            AGYQ_DESC = string.Empty;
            AGYQ_RESP_TYPE = string.Empty;
            //  AGYQ_SUB_TYPE = string.Empty; 
            AGYQ_ADD_OPERATOR = string.Empty;
            AGYQ_DATE_ADD = string.Empty;
            AGYQ_LSTC_OPERATOR = string.Empty;
            AGYQ_DATE_LSTC = string.Empty;
            AGYQ_REQ = string.Empty;

        }

        public AGCYPARTQUESEntity(bool Initialize)
        {
            if (Initialize)
            {
                AGYQ_CODE = null;
                // AGYQ_SEQ = null;
                AGYQ_ACTIVE = null;
                AGYQ_DESC = null;
                AGYQ_RESP_TYPE = null;

                AGYQ_ADD_OPERATOR = null;
                AGYQ_DATE_ADD = null;
                AGYQ_LSTC_OPERATOR = null;
                AGYQ_DATE_LSTC = null;
                AGYQ_REQ = null;
            }

        }


        public AGCYPARTQUESEntity(DataRow Agcypartques)
        {
            if (Agcypartques != null)
            {
                DataRow row = Agcypartques;

                Mode = "U";
                AGYQ_CODE = row["AGYQ_CODE"].ToString();
                //  AGYQ_SEQ = row["AGYQ_SEQ"].ToString();
                AGYQ_ACTIVE = row["AGYQ_ACTIVE"].ToString();
                AGYQ_DESC = row["AGYQ_DESC"].ToString();
                AGYQ_RESP_TYPE = row["AGYQ_RESP_TYPE"].ToString();
                //  AGYQ_SUB_TYPE = row["AGYQ_SUB_TYPE"].ToString();
                AGYQ_ADD_OPERATOR = row["AGYQ_ADD_OPERATOR"].ToString();
                AGYQ_DATE_ADD = row["AGYQ_DATE_ADD"].ToString();
                AGYQ_LSTC_OPERATOR = row["AGYQ_LSTC_OPERATOR"].ToString();
                AGYQ_DATE_LSTC = row["AGYQ_DATE_LSTC"].ToString();
                AGYQ_REQ = row["AGYQ_REQ"].ToString();
            }
        }

        public AGCYPARTQUESEntity(AGCYPARTQUESEntity Entity)
        {
            if (Entity != null)
            {
                Mode = Entity.Mode;
                AGYQ_CODE = Entity.AGYQ_CODE;
                //  AGYQ_SEQ = Entity.AGYQ_SEQ;
                AGYQ_ACTIVE = Entity.AGYQ_ACTIVE;
                AGYQ_DESC = Entity.AGYQ_DESC;
                AGYQ_RESP_TYPE = Entity.AGYQ_RESP_TYPE;
                //   AGYQ_SUB_TYPE = Entity.AGYQ_SUB_TYPE;
                AGYQ_ADD_OPERATOR = Entity.AGYQ_ADD_OPERATOR;
                AGYQ_DATE_ADD = Entity.AGYQ_DATE_ADD;
                AGYQ_LSTC_OPERATOR = Entity.AGYQ_LSTC_OPERATOR;
                AGYQ_DATE_LSTC = Entity.AGYQ_DATE_LSTC;
                AGYQ_REQ = Entity.AGYQ_REQ;
            }
        }


        #endregion

        #region Properties


        public string Mode { get; set; }
        public string AGYQ_CODE { get; set; }
        //   public string AGYQ_SEQ { get; set; }
        public string AGYQ_ACTIVE { get; set; }
        public string AGYQ_DESC { get; set; }
        public string AGYQ_RESP_TYPE { get; set; }
        //public string AGYQ_SUB_TYPE { get; set; }
        public string AGYQ_ADD_OPERATOR { get; set; }
        public string AGYQ_DATE_ADD { get; set; }
        public string AGYQ_LSTC_OPERATOR { get; set; }
        public string AGYQ_DATE_LSTC { get; set; }
        public string AGYQ_REQ { get; set; }

        #endregion

    }

    public class AGCYSERQASSOCEntity
    {
        #region Constructors

        public AGCYSERQASSOCEntity()
        {
            Mode = string.Empty;
            AGYSERQ_SERCODE = string.Empty;
            AGYSERQ_QUESCODE = string.Empty;
            AGYSERQ_SEQ = string.Empty;
            AGYSERQ_ADD_OPERATOR = string.Empty;
            AGYSERQ_DATE_ADD = string.Empty;
            AGYSERQ_LSTC_OPERATOR = string.Empty;
            AGYSERQ_DATE_LSTC = string.Empty;
            AGYQ_ACTIVE = string.Empty;
            AGYQ_DESC = string.Empty;
            AGYQ_RESP_TYPE = string.Empty;

        }

        public AGCYSERQASSOCEntity(bool Initialize)
        {
            if (Initialize)
            {

                AGYSERQ_SERCODE = null;
                AGYSERQ_QUESCODE = null;
                AGYSERQ_SEQ = null;
                AGYSERQ_ADD_OPERATOR = null;
                AGYSERQ_DATE_ADD = null;
                AGYSERQ_LSTC_OPERATOR = null;
                AGYSERQ_DATE_LSTC = null;
            }

        }


        public AGCYSERQASSOCEntity(DataRow Agcypartques)
        {
            if (Agcypartques != null)
            {
                DataRow row = Agcypartques;

                Mode = "U";
                AGYSERQ_SEQ = row["AGYSERQ_SEQ"].ToString();
                AGYSERQ_SERCODE = row["AGYSERQ_SERCODE"].ToString();
                AGYSERQ_QUESCODE = row["AGYSERQ_QUESCODE"].ToString();
                AGYSERQ_SEQ = row["AGYSERQ_SEQ"].ToString();
                AGYSERQ_ADD_OPERATOR = row["AGYSERQ_ADD_OPERATOR"].ToString();
                AGYSERQ_DATE_ADD = row["AGYSERQ_DATE_ADD"].ToString();
                AGYSERQ_LSTC_OPERATOR = row["AGYSERQ_LSTC_OPERATOR"].ToString();
                AGYSERQ_DATE_LSTC = row["AGYSERQ_DATE_LSTC"].ToString();
                if (row.Table.Columns.Contains("AGYQ_DESC"))
                    AGYQ_DESC = row["AGYQ_DESC"].ToString();
                if (row.Table.Columns.Contains("AGYQ_RESP_TYPE"))
                    AGYQ_RESP_TYPE = row["AGYQ_RESP_TYPE"].ToString();
                if (row.Table.Columns.Contains("AGYQ_ACTIVE"))
                    AGYQ_ACTIVE = row["AGYQ_ACTIVE"].ToString();
            }
        }

        public AGCYSERQASSOCEntity(AGCYSERQASSOCEntity Entity)
        {
            if (Entity != null)
            {
                Mode = Entity.Mode;
                AGYSERQ_SEQ = Entity.AGYSERQ_SEQ;
                AGYSERQ_SERCODE = Entity.AGYSERQ_SERCODE;
                AGYSERQ_QUESCODE = Entity.AGYSERQ_QUESCODE;
                AGYSERQ_SEQ = Entity.AGYSERQ_SEQ;
                AGYSERQ_ADD_OPERATOR = Entity.AGYSERQ_ADD_OPERATOR;
                AGYSERQ_DATE_ADD = Entity.AGYSERQ_DATE_ADD;
                AGYSERQ_LSTC_OPERATOR = Entity.AGYSERQ_LSTC_OPERATOR;
                AGYSERQ_DATE_LSTC = Entity.AGYSERQ_DATE_LSTC;
            }
        }


        #endregion

        #region Properties


        public string Mode { get; set; }
        public string AGYSERQ_SERCODE { get; set; }
        public string AGYSERQ_QUESCODE { get; set; }
        public string AGYSERQ_SEQ { get; set; }
        public string AGYSERQ_ADD_OPERATOR { get; set; }
        public string AGYSERQ_DATE_ADD { get; set; }
        public string AGYSERQ_LSTC_OPERATOR { get; set; }
        public string AGYSERQ_DATE_LSTC { get; set; }
        public string AGYQ_DESC { get; set; }
        public string AGYQ_RESP_TYPE { get; set; }
        public string AGYQ_ACTIVE { get; set; }

        #endregion

    }

    public class CMBDCEntity
    {
        #region Constructors

        public CMBDCEntity()
        {
            BDC_AGENCY = string.Empty;
            BDC_DEPT = string.Empty;
            BDC_PROGRAM = string.Empty;
            BDC_YEAR = string.Empty;
            BDC_DESCRIPTION = string.Empty;
            BDC_FUND = string.Empty;
            BDC_ID = string.Empty;
            BDC_BUDGET = string.Empty;
            BDC_START = string.Empty;
            BDC_END = string.Empty;
            BDC_ALLOW_POSTING = string.Empty;
            BDC_BALANCE = string.Empty;
            BDC_TOT_EXPEND = string.Empty;
            BDC_TOT_COMMIT = string.Empty;
            BDC_DATE_LSTC = string.Empty;
            BDC_LSTC_OPERATOR = string.Empty;
            BDC_DATE_ADD = string.Empty;
            BDC_ADD_OPERATOR = string.Empty;
            BDC_REST_AMOUNT = string.Empty;
            BDC_FUND_CODE = string.Empty;
            BDC_BENEFIT_TYPE = string.Empty;
            BDC_CONTR_NUM = string.Empty;

        }

        public CMBDCEntity(DataRow Row)
        {
            if (Row != null)
            {
                BDC_AGENCY = Row["BDC_AGENCY"].ToString().Trim();
                BDC_DEPT = Row["BDC_DEPT"].ToString().Trim();
                BDC_PROGRAM = Row["BDC_PROGRAM"].ToString().Trim();
                BDC_YEAR = Row["BDC_YEAR"].ToString().Trim();

                BDC_DESCRIPTION = Row["BDC_DESCRIPTION"].ToString().Trim();
                BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                BDC_ID = Row["BDC_ID"].ToString().Trim();
                BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                BDC_START = Row["BDC_START"].ToString().Trim();
                BDC_END = Row["BDC_END"].ToString().Trim();

                BDC_ALLOW_POSTING = Row["BDC_ALLOW_POSTING"].ToString().Trim();
                BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                BDC_TOT_EXPEND = Row["BDC_TOT_EXPEND"].ToString().Trim();
                BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                BDC_DATE_LSTC = Row["BDC_DATE_LSTC"].ToString().Trim();
                BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();
                BDC_DATE_ADD = Row["BDC_DATE_ADD"].ToString().Trim();
                BDC_ADD_OPERATOR = Row["BDC_ADD_OPERATOR"].ToString().Trim();

                BDC_FUND_CODE = Row["BDC_FUND_CODE"].ToString().Trim();
                BDC_BENEFIT_TYPE = Row["BDC_BENEFIT_TYPE"].ToString().Trim();
                BDC_CONTR_NUM = Row["BDC_CONTR_NUM"].ToString().Trim();
                BDC_REST_AMOUNT = string.Empty;
            }
        }

        public CMBDCEntity(DataRow Row, string strType)
        {
            if (Row != null)
            {
                if (strType == "TotBalance")
                {
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                }
                else if (strType == "CalculateBalance")
                {
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                    BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                    BDC_TOT_EXPEND = Row["BDC_TOT_EXPEND"].ToString().Trim();
                    BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                    BDC_REST_AMOUNT = Row["BDC_REST_AMOUNT"].ToString().Trim();

                }
                //else if (strType == "RECORDEXIST")
                //{
                //    BDC_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                //}
                //else if (strType == "RECORDVALIDATION")
                //{
                //    BDC_AGENCY = Row["EMSRES_AGENCY"].ToString().Trim();
                //}
                else
                {
                    BDC_AGENCY = Row["BDC_AGENCY"].ToString().Trim();
                    BDC_DEPT = Row["BDC_DEPT"].ToString().Trim();
                    BDC_PROGRAM = Row["BDC_PROGRAM"].ToString().Trim();
                    BDC_YEAR = Row["BDC_YEAR"].ToString().Trim();

                    BDC_DESCRIPTION = Row["BDC_DESCRIPTION"].ToString().Trim();
                    BDC_FUND = Row["BDC_FUND"].ToString().Trim();
                    BDC_ID = Row["BDC_ID"].ToString().Trim();
                    BDC_BUDGET = Row["BDC_BUDGET"].ToString().Trim();
                    BDC_START = Row["BDC_START"].ToString().Trim();
                    BDC_END = Row["BDC_END"].ToString().Trim();

                    BDC_ALLOW_POSTING = Row["BDC_ALLOW_POSTING"].ToString().Trim();
                    BDC_BALANCE = Row["BDC_BALANCE"].ToString().Trim();
                    BDC_TOT_EXPEND = Row["BDC_TOT_EXPEND"].ToString().Trim();
                    BDC_TOT_COMMIT = Row["BDC_TOT_COMMIT"].ToString().Trim();
                    BDC_DATE_LSTC = Row["BDC_DATE_LSTC"].ToString().Trim();
                    BDC_LSTC_OPERATOR = Row["BDC_LSTC_OPERATOR"].ToString().Trim();
                    BDC_DATE_ADD = Row["BDC_DATE_ADD"].ToString().Trim();
                    BDC_ADD_OPERATOR = Row["BDC_ADD_OPERATOR"].ToString().Trim();

                    BDC_FUND_CODE = Row["BDC_FUND_CODE"].ToString().Trim();
                    if (Row.Table.Columns.Contains("BDC_BENEFIT_TYPE"))
                    {
                        BDC_BENEFIT_TYPE = Row["BDC_BENEFIT_TYPE"].ToString().Trim();
                    }
                    else
                        BDC_BENEFIT_TYPE = string.Empty;
                    if (Row.Table.Columns.Contains("BDC_CONTR_NUM"))
                    {
                        BDC_CONTR_NUM = Row["BDC_CONTR_NUM"].ToString().Trim();
                    }
                    else
                        BDC_CONTR_NUM = string.Empty;

                    BDC_REST_AMOUNT = string.Empty;
                    //if (strType == "EMSB0014")
                    //{
                    //    Available = Row["BAL"].ToString().Trim();
                    //}
                }
            }
        }

        #endregion

        #region Properties

        public string BDC_AGENCY { get; set; }
        public string BDC_DEPT { get; set; }
        public string BDC_PROGRAM { get; set; }
        public string BDC_YEAR { get; set; }
        public string BDC_DESCRIPTION { get; set; }
        public string BDC_FUND { get; set; }
        public string BDC_ID { get; set; }
        public string BDC_BUDGET { get; set; }
        public string BDC_START { get; set; }
        public string BDC_END { get; set; }
        public string BDC_ALLOW_POSTING { get; set; }
        public string BDC_BALANCE { get; set; }
        public string BDC_TOT_EXPEND { get; set; }
        public string BDC_TOT_COMMIT { get; set; }
        public string BDC_DATE_LSTC { get; set; }
        public string BDC_LSTC_OPERATOR { get; set; }
        public string BDC_DATE_ADD { get; set; }
        public string BDC_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string Available { get; set; }
        public string BDC_REST_AMOUNT { get; set; }
        public string BDC_FUND_CODE { get; set; }

        public string BDC_BENEFIT_TYPE { get; set; }
        public string BDC_CONTR_NUM { get; set; }

        #endregion

    }

    public class LETRHISTCEntity
    {
        #region Constructors

        public LETRHISTCEntity()
        {
            AGENCY = string.Empty;
            DEPT = string.Empty;
            PROGRAM = string.Empty;
            YEAR = string.Empty;
            APPNO = string.Empty;
            LETR_CODE = string.Empty;
            DATE = string.Empty;
            SEQ = string.Empty;
            WORKER = string.Empty;

            DATE_ADD = string.Empty;
            ADD_OPERATOR = string.Empty;

        }

        public LETRHISTCEntity(DataRow Row)
        {
            if (Row != null)
            {
                AGENCY = Row["LHIST_AGENCY"].ToString().Trim();
                DEPT = Row["LHIST_DEPT"].ToString().Trim();
                PROGRAM = Row["LHIST_PROG"].ToString().Trim();
                YEAR = Row["LHIST_YEAR"].ToString().Trim();

                APPNO = Row["LHIST_APPNO"].ToString().Trim();
                LETR_CODE = Row["LHIST_CODE"].ToString().Trim();
                DATE = Row["LHIST_DATE"].ToString().Trim();
                SEQ = Row["LHIST_SEQ"].ToString().Trim();
                WORKER = Row["LHIST_WORKER"].ToString().Trim();

                DATE_ADD = Row["LHIST_DATE_ADD"].ToString().Trim();
                ADD_OPERATOR = Row["LHIST_ADD_OPERATOR"].ToString().Trim();


            }
        }


        #endregion

        #region Properties

        public string AGENCY { get; set; }
        public string DEPT { get; set; }
        public string PROGRAM { get; set; }
        public string YEAR { get; set; }
        public string APPNO { get; set; }
        public string LETR_CODE { get; set; }
        public string DATE { get; set; }
        public string SEQ { get; set; }
        public string WORKER { get; set; }
        public string DATE_ADD { get; set; }
        public string ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string Available { get; set; }


        #endregion

    }

    /* public class CABUNDLEENTITY
     {
         #region Constructors

         public CABUNDLEENTITY()
         {
             Rec_Type =
             CAB_AGENCY =
             CAB_DEPT =
             CAB_PROGRAM =
             CAB_YEAR =
             CAB_BUNDLE =
             CAB_STATUS =
             CAB_DATE_OPENED =
             CAB_APP_COUNT =
             CAB_TOT_AMOUNT =
             CAB_FUND =
             CAB_VENDOR =
             CAB_CHK_NO =
             CAB_CHK_DATE =
             CAB_VOUCH_NO =
             CAB_PAY_NUMBER =
             CAB_PARAMS =

             CAB_DATE_LSTC =
             CAB_LSTC_OPERATOR = string.Empty;

         }

         public CABUNDLEENTITY(bool Initalize)
         {
             if (Initalize)
             {
                 Rec_Type =
                 CAB_AGENCY =
                 CAB_DEPT =
                 CAB_PROGRAM =
                 CAB_YEAR =
                 CAB_BUNDLE =
                 CAB_STATUS =
                 CAB_DATE_OPENED =
                 CAB_APP_COUNT =
                 CAB_TOT_AMOUNT =
                 CAB_FUND =
                 CAB_VENDOR =
                 CAB_CHK_NO =
                 CAB_CHK_DATE =
                 CAB_VOUCH_NO =
                 CAB_PAY_NUMBER =
                 CAB_PARAMS =

                 CAB_DATE_LSTC =
                 CAB_LSTC_OPERATOR = null;

             }
         }

         public CABUNDLEENTITY(DataRow Row)
         {
             if (Row != null)
             {
                 Rec_Type = "U";
                 CAB_AGENCY = Row["CAB_AGENCY"].ToString().Trim();
                 CAB_DEPT = Row["CAB_DEPT"].ToString().Trim();
                 CAB_PROGRAM = Row["CAB_PROGRAM"].ToString().Trim();
                 CAB_YEAR = Row["CAB_YEAR"].ToString().Trim();
                 CAB_BUNDLE = Row["CAB_BUNDLE"].ToString().Trim();
                 CAB_STATUS = Row["CAB_STATUS"].ToString().Trim();
                 CAB_DATE_OPENED = Row["CAB_DATE_OPENED"].ToString().Trim();
                 CAB_APP_COUNT = Row["CAB_APP_COUNT"].ToString().Trim();
                 CAB_TOT_AMOUNT = Row["CAB_TOT_AMOUNT"].ToString().Trim();
                 CAB_FUND = Row["CAB_FUND"].ToString().Trim();
                 CAB_VENDOR = Row["CAB_VENDOR"].ToString().Trim();
                 CAB_CHK_NO = Row["CAB_CHK_NO"].ToString().Trim();
                 CAB_CHK_DATE = Row["CAB_CHK_DATE"].ToString().Trim();
                 CAB_VOUCH_NO = Row["CAB_VOUCH_NO"].ToString().Trim();
                 CAB_PAY_NUMBER = Row["CAB_PAY_NUMBER"].ToString().Trim();
                 CAB_PARAMS = Row["CAB_PARAMS"].ToString().Trim();
                 CAB_DATE_LSTC = Row["CAB_DATE_LSTC"].ToString().Trim();
                 CAB_LSTC_OPERATOR = Row["CAB_LSTC_OPERATOR"].ToString().Trim();


             }
         }



         #endregion

         #region Properties

         public string CAB_AGENCY { get; set; }
         public string CAB_DEPT { get; set; }
         public string CAB_PROGRAM { get; set; }
         public string CAB_YEAR { get; set; }
         public string CAB_BUNDLE { get; set; }
         public string CAB_STATUS { get; set; }
         public string CAB_DATE_OPENED { get; set; }
         public string CAB_APP_COUNT { get; set; }
         public string CAB_TOT_AMOUNT { get; set; }
         public string CAB_FUND { get; set; }
         public string CAB_VENDOR { get; set; }
         public string CAB_CHK_NO { get; set; }
         public string CAB_CHK_DATE { get; set; }
         public string CAB_VOUCH_NO { get; set; }
         public string CAB_PAY_NUMBER { get; set; }
         public string CAB_PARAMS { get; set; }
         public string CAB_DATE_LSTC { get; set; }
         public string CAB_LSTC_OPERATOR { get; set; }


         //public string VDN_ADD_OPERATOR { get; set; }
         //public string Mode { get; set; }

         public string Rec_Type { get; set; }

         #endregion

     }
    */   // Commented and brought update - Viaksh 01/03/2023
    public class CABUNDLEENTITY
    {
        #region Constructors

        public CABUNDLEENTITY()
        {
            Rec_Type =
            CAB_AGENCY =
            CAB_DEPT =
            CAB_PROGRAM =
            CAB_YEAR =
            CAB_BUNDLE =
            CAB_STATUS =
            CAB_DATE_OPENED =
            CAB_APP_COUNT =
            CAB_TOT_AMOUNT =
            CAB_FUND =
            CAB_VENDOR =
            CAB_CHK_NO =
            CAB_CHK_DATE =
            CAB_VOUCH_NO =
            CAB_PAY_NUMBER =
            CAB_PARAMS =

            ADJ_Amount =

            CAB_DATE_LSTC =
            CAB_LSTC_OPERATOR =
            CAB_PDOUT=
            CAB_START_NUM=
            CAB_END_NUM = string.Empty;

        }

        public CABUNDLEENTITY(bool Initalize)
        {
            if (Initalize)
            {
                Rec_Type =
                CAB_AGENCY =
                CAB_DEPT =
                CAB_PROGRAM =
                CAB_YEAR =
                CAB_BUNDLE =
                CAB_STATUS =
                CAB_DATE_OPENED =
                CAB_APP_COUNT =
                CAB_TOT_AMOUNT =
                CAB_FUND =
                CAB_VENDOR =
                CAB_CHK_NO =
                CAB_CHK_DATE =
                CAB_VOUCH_NO =
                CAB_PAY_NUMBER =
                CAB_PARAMS =

                ADJ_Amount =

                CAB_DATE_LSTC =
                CAB_LSTC_OPERATOR =
                CAB_PDOUT =
                CAB_START_NUM =
                CAB_END_NUM = null;

            }
        }

        public CABUNDLEENTITY(DataRow Row)
        {
            if (Row != null)
            {
                Rec_Type = "U";
                CAB_AGENCY = Row["CAB_AGENCY"].ToString().Trim();
                CAB_DEPT = Row["CAB_DEPT"].ToString().Trim();
                CAB_PROGRAM = Row["CAB_PROGRAM"].ToString().Trim();
                CAB_YEAR = Row["CAB_YEAR"].ToString().Trim();
                CAB_BUNDLE = Row["CAB_BUNDLE"].ToString().Trim();
                CAB_STATUS = Row["CAB_STATUS"].ToString().Trim();
                CAB_DATE_OPENED = Row["CAB_DATE_OPENED"].ToString().Trim();
                CAB_APP_COUNT = Row["CAB_APP_COUNT"].ToString().Trim();
                CAB_TOT_AMOUNT = Row["CAB_TOT_AMOUNT"].ToString().Trim();
                CAB_FUND = Row["CAB_FUND"].ToString().Trim();
                CAB_VENDOR = Row["CAB_VENDOR"].ToString().Trim();
                CAB_CHK_NO = Row["CAB_CHK_NO"].ToString().Trim();
                CAB_CHK_DATE = Row["CAB_CHK_DATE"].ToString().Trim();
                CAB_VOUCH_NO = Row["CAB_VOUCH_NO"].ToString().Trim();
                CAB_PAY_NUMBER = Row["CAB_PAY_NUMBER"].ToString().Trim();
                CAB_PARAMS = Row["CAB_PARAMS"].ToString().Trim();
                CAB_DATE_LSTC = Row["CAB_DATE_LSTC"].ToString().Trim();
                CAB_LSTC_OPERATOR = Row["CAB_LSTC_OPERATOR"].ToString().Trim();

                if (Row["CAB_PDOUT"].ToString().Trim() == "Y")
                    CAB_PDOUT = "Y";
                else
                    CAB_PDOUT= "N";

                CAB_START_NUM = Row["CAB_START_NUM"].ToString().Trim();
                CAB_END_NUM = Row["CAB_END_NUM"].ToString().Trim();

                ADJ_Amount = Row["ADJ_AMOUNT"].ToString().Trim();


            }
        }



        #endregion

        #region Properties

        public string CAB_AGENCY { get; set; }
        public string CAB_DEPT { get; set; }
        public string CAB_PROGRAM { get; set; }
        public string CAB_YEAR { get; set; }
        public string CAB_BUNDLE { get; set; }
        public string CAB_STATUS { get; set; }
        public string CAB_DATE_OPENED { get; set; }
        public string CAB_APP_COUNT { get; set; }
        public string CAB_TOT_AMOUNT { get; set; }
        public string CAB_FUND { get; set; }
        public string CAB_VENDOR { get; set; }
        public string CAB_CHK_NO { get; set; }
        public string CAB_CHK_DATE { get; set; }
        public string CAB_VOUCH_NO { get; set; }
        public string CAB_PAY_NUMBER { get; set; }
        public string CAB_PARAMS { get; set; }
        public string CAB_DATE_LSTC { get; set; }
        public string CAB_LSTC_OPERATOR { get; set; }

        public string CAB_PDOUT { get; set; }

        public string CAB_START_NUM { get; set; }

        public string CAB_END_NUM { get; set; }

        public string ADJ_Amount { get; set; }

        //public string VDN_ADD_OPERATOR { get; set; }
        //public string Mode { get; set; }

        public string Rec_Type { get; set; }

        #endregion

    }

    public class CEAPCNTLEntity // Commented and brought update - Viaksh 01/03/2023 
    {
        #region Constructors

        public CEAPCNTLEntity()
        {
            /*CPCT_CODE = string.Empty;
            CPCT_YEAR = string.Empty;
            CPCT_PROG_START = string.Empty;
            CPCT_PROG_END = string.Empty;
            CPCT_VUL_SP = string.Empty;
            CPCT_VUL_50 = string.Empty;
            CPCT_VUL_75 = string.Empty;
            CPCT_VUL_150 = string.Empty;
            CPCT_VUL_MAX_INV = string.Empty;
            CPCT_VUL_PRIM_CA = string.Empty;
            CPCT_NONVUL_SP = string.Empty;
            CPCT_NONVUL_50 = string.Empty;
            CPCT_NONVUL_75 = string.Empty;
            CPCT_NONVUL_150 = string.Empty;
            CPCT_NONVUL_MAX_INV = string.Empty;
            CPCT_NONVUL_PRIM_CA = string.Empty;
            CPCT_DATE_ADD = string.Empty;
            CPCT_ADD_OPERATOR = string.Empty;
            CPCT_LSTC_DATE = string.Empty;
            CPCT_LSTC_OPERATOR = string.Empty;

            CPCT_VUL_SSI = string.Empty;
            CPCT_VUL_MTVC = string.Empty;
            CPCT_NONVUL_SSI = string.Empty;
            CPCT_NONVUL_MTVC = string.Empty;

            Rec_Type = string.Empty;*/

            CPCT_CODE = string.Empty;
            CPCT_YEAR = string.Empty;
            CPCT_PROG_START = string.Empty;
            CPCT_PROG_END = string.Empty;
            CPCT_VUL_SP = string.Empty;
            CPCT_VUL_50 = string.Empty;
            CPCT_VUL_75 = string.Empty;
            CPCT_VUL_150 = string.Empty;
            CPCT_VUL_MAX_INV = string.Empty;
            CPCT_VUL_PRIM_CA = string.Empty;
            CPCT_NONVUL_SP = string.Empty;
            CPCT_NONVUL_50 = string.Empty;
            CPCT_NONVUL_75 = string.Empty;
            CPCT_NONVUL_150 = string.Empty;
            CPCT_NONVUL_MAX_INV = string.Empty;
            CPCT_NONVUL_PRIM_CA = string.Empty;
            CPCT_DATE_ADD = string.Empty;
            CPCT_ADD_OPERATOR = string.Empty;
            CPCT_LSTC_DATE = string.Empty;
            CPCT_LSTC_OPERATOR = string.Empty;

            CPCT_VUL_SSI = string.Empty;
            CPCT_VUL_MTVC = string.Empty;
            CPCT_NONVUL_SSI = string.Empty;
            CPCT_NONVUL_MTVC = string.Empty;
            CPCT_HC_SP = string.Empty;
            CPCT_P1_SP = string.Empty;
            CPCT_P2_SP = string.Empty;
            CPCT_P3_SP = string.Empty;
            CPCT_P1_CA1 = string.Empty;
            CPCT_P1_CA2 = string.Empty;
            CPCT_P1_CA3 = string.Empty;
            CPCT_P2_CA1 = string.Empty;
            CPCT_P2_CA2 = string.Empty;
            CPCT_P2_CA3 = string.Empty;
            CPCT_P3_CA1 = string.Empty;
            CPCT_P3_CA2 = string.Empty;
            CPCT_P3_CA3 = string.Empty;

            CPCT_VUL_BENTYPE = string.Empty;
            CPCT_NONVUL_BENTYPE = string.Empty;
            CPCT_CRISIS_BENTYPE = string.Empty;
            CPCT_SUBREP_ID = string.Empty;

            CPCT_VUL_SNAP = string.Empty;
            CPCT_NONVUL_SNAP=string.Empty;

            Rec_Type = string.Empty;

            CPCT_INV_METHOD = string.Empty;

            CPCT_LUMP_SUM = string.Empty;
            CPCT_HC_CA = string.Empty;
            CPCT_VUL_ARR_CA = string.Empty;
            CPCT_NONVUL_ARR_CA = string.Empty;
            CPCT_LUMP_INTRVAL = string.Empty;
            CPCT_USER_CNTL = string.Empty;

            CPCT_CSBG_CONTRACT_NO = string.Empty;
        }

        public CEAPCNTLEntity(DataRow Row)
        {
            if (Row != null)
            {
                /*CPCT_CODE = Row["CPCT_CODE"].ToString().Trim();
                CPCT_YEAR = Row["CPCT_YEAR"].ToString().Trim();
                CPCT_PROG_START = Row["CPCT_PROG_START"].ToString().Trim();
                CPCT_PROG_END = Row["CPCT_PROG_END"].ToString().Trim();

                CPCT_VUL_SP = Row["CPCT_VUL_SP"].ToString().Trim();
                CPCT_VUL_50 = Row["CPCT_VUL_50"].ToString().Trim();
                CPCT_VUL_75 = Row["CPCT_VUL_75"].ToString().Trim();
                CPCT_VUL_150 = Row["CPCT_VUL_150"].ToString().Trim();
                CPCT_VUL_MAX_INV = Row["CPCT_VUL_MAX_INV"].ToString().Trim();
                CPCT_VUL_PRIM_CA = Row["CPCT_VUL_PRIM_CA"].ToString().Trim();

                CPCT_NONVUL_SP = Row["CPCT_NONVUL_SP"].ToString().Trim();
                CPCT_NONVUL_50 = Row["CPCT_NONVUL_50"].ToString().Trim();
                CPCT_NONVUL_75 = Row["CPCT_NONVUL_75"].ToString().Trim();
                CPCT_NONVUL_150 = Row["CPCT_NONVUL_150"].ToString().Trim();
                CPCT_NONVUL_MAX_INV = Row["CPCT_NONVUL_MAX_INV"].ToString().Trim();
                CPCT_NONVUL_PRIM_CA = Row["CPCT_NONVUL_PRIM_CA"].ToString().Trim();
                CPCT_LSTC_DATE = Row["CPCT_LSTC_DATE"].ToString().Trim(); ;
                CPCT_LSTC_OPERATOR = Row["CPCT_LSTC_OPERATOR"].ToString().Trim();
                CPCT_DATE_ADD = Row["CPCT_DATE_ADD"].ToString().Trim();
                CPCT_ADD_OPERATOR = Row["CPCT_ADD_OPERATOR"].ToString().Trim();


                CPCT_VUL_SSI = Row["CPCT_VUL_SSI"].ToString().Trim();
                CPCT_VUL_MTVC = Row["CPCT_VUL_MTVC"].ToString().Trim();
                CPCT_NONVUL_SSI = Row["CPCT_NONVUL_SSI"].ToString().Trim();
                CPCT_NONVUL_MTVC = Row["CPCT_NONVUL_MTVC"].ToString().Trim();*/

                CPCT_CODE = Row["CPCT_CODE"].ToString().Trim();
                CPCT_YEAR = Row["CPCT_YEAR"].ToString().Trim();
                CPCT_PROG_START = Row["CPCT_PROG_START"].ToString().Trim();
                CPCT_PROG_END = Row["CPCT_PROG_END"].ToString().Trim();

                CPCT_VUL_SP = Row["CPCT_VUL_SP"].ToString().Trim();
                CPCT_VUL_50 = Row["CPCT_VUL_50"].ToString().Trim();
                CPCT_VUL_75 = Row["CPCT_VUL_75"].ToString().Trim();
                CPCT_VUL_150 = Row["CPCT_VUL_150"].ToString().Trim();
                CPCT_VUL_MAX_INV = Row["CPCT_VUL_MAX_INV"].ToString().Trim();
                CPCT_VUL_PRIM_CA = Row["CPCT_VUL_PRIM_CA"].ToString().Trim();

                CPCT_NONVUL_SP = Row["CPCT_NONVUL_SP"].ToString().Trim();
                CPCT_NONVUL_50 = Row["CPCT_NONVUL_50"].ToString().Trim();
                CPCT_NONVUL_75 = Row["CPCT_NONVUL_75"].ToString().Trim();
                CPCT_NONVUL_150 = Row["CPCT_NONVUL_150"].ToString().Trim();
                CPCT_NONVUL_MAX_INV = Row["CPCT_NONVUL_MAX_INV"].ToString().Trim();
                CPCT_NONVUL_PRIM_CA = Row["CPCT_NONVUL_PRIM_CA"].ToString().Trim();
                CPCT_LSTC_DATE = Row["CPCT_LSTC_DATE"].ToString().Trim(); ;
                CPCT_LSTC_OPERATOR = Row["CPCT_LSTC_OPERATOR"].ToString().Trim();
                CPCT_DATE_ADD = Row["CPCT_DATE_ADD"].ToString().Trim();
                CPCT_ADD_OPERATOR = Row["CPCT_ADD_OPERATOR"].ToString().Trim();


                CPCT_VUL_SSI = Row["CPCT_VUL_SSI"].ToString().Trim();
                CPCT_VUL_MTVC = Row["CPCT_VUL_MTVC"].ToString().Trim();
                CPCT_NONVUL_SSI = Row["CPCT_NONVUL_SSI"].ToString().Trim();
                CPCT_NONVUL_MTVC = Row["CPCT_NONVUL_MTVC"].ToString().Trim();

                CPCT_HC_SP = Row["CPCT_HC_SP"].ToString().Trim();
                CPCT_P1_SP = Row["CPCT_P1_SP"].ToString().Trim();
                CPCT_P2_SP = Row["CPCT_P2_SP"].ToString().Trim();
                CPCT_P3_SP = Row["CPCT_P3_SP"].ToString().Trim();

                CPCT_P1_CA1 = Row["CPCT_P1_CA1"].ToString().Trim();
                CPCT_P1_CA2 = Row["CPCT_P1_CA2"].ToString().Trim();
                CPCT_P1_CA3 = Row["CPCT_P1_CA3"].ToString().Trim();
                CPCT_P2_CA1 = Row["CPCT_P2_CA1"].ToString().Trim();
                CPCT_P2_CA2 = Row["CPCT_P2_CA2"].ToString().Trim();
                CPCT_P2_CA3 = Row["CPCT_P2_CA3"].ToString().Trim();
                CPCT_P3_CA1 = Row["CPCT_P3_CA1"].ToString().Trim();
                CPCT_P3_CA2 = Row["CPCT_P3_CA2"].ToString().Trim();
                CPCT_P3_CA3 = Row["CPCT_P3_CA3"].ToString().Trim();

                CPCT_VUL_BENTYPE = Row["CPCT_VUL_BENTYPE"].ToString().Trim();
                CPCT_NONVUL_BENTYPE = Row["CPCT_NONVUL_BENTYPE"].ToString().Trim();
                CPCT_CRISIS_BENTYPE = Row["CPCT_CRISIS_BENTYPE"].ToString().Trim();
                CPCT_SUBREP_ID = Row["CPCT_SUBREP_ID"].ToString().Trim();

                CPCT_VUL_SNAP = Row["CPCT_VUL_SNAP"].ToString().Trim();
                CPCT_NONVUL_SNAP = Row["CPCT_NONVUL_SNAP"].ToString().Trim();

                CPCT_INV_METHOD = Row["CPCT_INV_METHOD"].ToString().Trim();

                CPCT_LUMP_SUM = Row["CPCT_LUMP_SUM"].ToString().Trim();
                CPCT_HC_CA = Row["CPCT_HC_CA"].ToString().Trim();
                CPCT_VUL_ARR_CA = Row["CPCT_VUL_ARR_CA"].ToString().Trim();
                CPCT_NONVUL_ARR_CA = Row["CPCT_NONVUL_ARR_CA"].ToString().Trim();
                CPCT_LUMP_INTRVAL = Row["CPCT_LUMP_INTRVAL"].ToString().Trim();
                CPCT_USER_CNTL = Row["CPCT_USER_CNTL"].ToString().Trim();

                CPCT_CSBG_CONTRACT_NO = Row["CPCT_CSBG_CONTRACT_NO"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        /* public string CPCT_CODE { get; set; }
         public string CPCT_YEAR { get; set; }
         public string CPCT_PROG_START { get; set; }
         public string CPCT_PROG_END { get; set; }
         public string CPCT_VUL_SP { get; set; }
         public string CPCT_VUL_50 { get; set; }
         public string CPCT_VUL_75 { get; set; }
         public string CPCT_VUL_150 { get; set; }
         public string CPCT_VUL_MAX_INV { get; set; }
         public string CPCT_VUL_PRIM_CA { get; set; }
         public string CPCT_NONVUL_SP { get; set; }
         public string CPCT_NONVUL_50 { get; set; }
         public string CPCT_NONVUL_75 { get; set; }
         public string CPCT_NONVUL_150 { get; set; }
         public string CPCT_NONVUL_MAX_INV { get; set; }
         public string CPCT_NONVUL_PRIM_CA { get; set; }
         public string CPCT_DATE_ADD { get; set; }
         public string CPCT_ADD_OPERATOR { get; set; }
         public string Mode { get; set; }
         public string Rec_Type { get; set; }
         public string CPCT_LSTC_DATE { get; set; }
         public string CPCT_LSTC_OPERATOR { get; set; }

         public string CPCT_VUL_SSI { get; set; }
         public string CPCT_VUL_MTVC { get; set; }
         public string CPCT_NONVUL_SSI { get; set; }
         public string CPCT_NONVUL_MTVC { get; set; }*/

        public string CPCT_CODE { get; set; }
        public string CPCT_YEAR { get; set; }
        public string CPCT_PROG_START { get; set; }
        public string CPCT_PROG_END { get; set; }
        public string CPCT_VUL_SP { get; set; }
        public string CPCT_VUL_50 { get; set; }
        public string CPCT_VUL_75 { get; set; }
        public string CPCT_VUL_150 { get; set; }
        public string CPCT_VUL_MAX_INV { get; set; }
        public string CPCT_VUL_PRIM_CA { get; set; }
        public string CPCT_NONVUL_SP { get; set; }
        public string CPCT_NONVUL_50 { get; set; }
        public string CPCT_NONVUL_75 { get; set; }
        public string CPCT_NONVUL_150 { get; set; }
        public string CPCT_NONVUL_MAX_INV { get; set; }
        public string CPCT_NONVUL_PRIM_CA { get; set; }
        public string CPCT_DATE_ADD { get; set; }
        public string CPCT_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }
        public string Rec_Type { get; set; }
        public string CPCT_LSTC_DATE { get; set; }
        public string CPCT_LSTC_OPERATOR { get; set; }

        public string CPCT_VUL_SSI { get; set; }
        public string CPCT_VUL_MTVC { get; set; }
        public string CPCT_NONVUL_SSI { get; set; }
        public string CPCT_NONVUL_MTVC { get; set; }

        public string CPCT_HC_SP { get; set; }
        public string CPCT_P1_SP { get; set; }
        public string CPCT_P2_SP { get; set; }
        public string CPCT_P3_SP { get; set; }

        public string CPCT_P1_CA1 { get; set; }
        public string CPCT_P1_CA2 { get; set; }
        public string CPCT_P1_CA3 { get; set; }
        public string CPCT_P2_CA1 { get; set; }
        public string CPCT_P2_CA2 { get; set; }
        public string CPCT_P2_CA3 { get; set; }
        public string CPCT_P3_CA1 { get; set; }
        public string CPCT_P3_CA2 { get; set; }
        public string CPCT_P3_CA3 { get; set; }


        public string CPCT_VUL_BENTYPE { get; set; }
        public string CPCT_NONVUL_BENTYPE { get; set; }
       
        public string CPCT_CRISIS_BENTYPE { get; set; }
        public string CPCT_SUBREP_ID { get; set; }

        public string CPCT_VUL_SNAP { get; set; }
        public string CPCT_NONVUL_SNAP { get; set; }

        public string CPCT_INV_METHOD { get; set; }


        public string CPCT_LUMP_SUM { get; set; }
        public string CPCT_HC_CA { get; set; }
        public string CPCT_VUL_ARR_CA { get; set; }
        public string CPCT_NONVUL_ARR_CA { get; set; }

        public string CPCT_LUMP_INTRVAL { get; set; }
        public string CPCT_USER_CNTL
        {
            get; set;
        }
        public string CPCT_CSBG_CONTRACT_NO
        {
            get; set;
        }

        #endregion

    }

    public class CEAPINVEntity 
    {
        #region Constructors

        public CEAPINVEntity()
        {

            CPINV_CPCT_CODE = string.Empty;
            CPINV_YEAR = string.Empty;
            CPINV_PRIORTY = string.Empty;
            CPINV_LOW = string.Empty;
            CPINV_HIGH = string.Empty;
            CPINV_MAX_INV = string.Empty;
            CPINV_ADD_DATE = string.Empty;
            CPINV_ADD_OPERATOR = string.Empty;
            CPINV_LSTC_DATE = string.Empty;
            CPINV_LSTC_OPEARATOR = string.Empty;

        }

        public CEAPINVEntity(DataRow Row)
        {
            if (Row != null)
            {
                CPINV_CPCT_CODE = Row["CPINV_CPCT_CODE"].ToString().Trim();
                CPINV_YEAR = Row["CPINV_YEAR"].ToString().Trim();
                CPINV_PRIORTY = Row["CPINV_PRIORTY"].ToString().Trim();
                CPINV_LOW = Row["CPINV_LOW"].ToString().Trim();
                CPINV_HIGH = Row["CPINV_HIGH"].ToString().Trim();
                CPINV_MAX_INV = Row["CPINV_MAX_INV"].ToString().Trim();
                CPINV_ADD_DATE = Row["CPINV_ADD_DATE"].ToString().Trim();
                CPINV_ADD_OPERATOR = Row["CPINV_ADD_OPERATOR"].ToString().Trim();
                CPINV_LSTC_DATE = Row["CPINV_LSTC_DATE"].ToString().Trim();
                CPINV_LSTC_OPEARATOR = Row["CPINV_LSTC_OPEARATOR"].ToString().Trim();
            }
        }



        #endregion

        #region Properties
        public string CPINV_CPCT_CODE { get; set; }
        public string CPINV_YEAR { get; set; }
        public string CPINV_PRIORTY { get; set; }
        public string CPINV_LOW { get; set; }
        public string CPINV_HIGH { get; set; }
        public string CPINV_MAX_INV { get; set; }
        public string CPINV_ADD_DATE { get; set; }
        public string CPINV_ADD_OPERATOR { get; set; }
        public string CPINV_LSTC_DATE { get; set; }
        public string CPINV_LSTC_OPEARATOR { get; set; }
        public string Mode { get; set; }
        

        #endregion

    }

    public class CaseActAdjEntity
    {

        #region Constructors

        public CaseActAdjEntity()
        {
            ADJ_AGENCY = string.Empty;
            ADJ_DEPT = string.Empty;
            ADJ_PROGRAM = string.Empty;
            ADJ_YEAR = string.Empty;
            ADJ_APP_NO = string.Empty;
            ADJ_SERVICE_PLAN = string.Empty;
            ADJ_SPM_SEQ = string.Empty;
            ADJ_BRANCH = string.Empty;
            ADJ_GROUP = string.Empty;
            ADJ_ACTIVITY_CODE = string.Empty;
            ADJ_ACT_SEQ = string.Empty;
            ADJ_SEQ = string.Empty;
            ADJ_O_ACTYR_DATE = string.Empty;
            ADJ_O_ACTY_DATE = string.Empty;
            ADJ_O_CASEWRKR = string.Empty;
            ADJ_O_SITE = string.Empty;
            ADJ_O_VENDOR_NO = string.Empty;
            ADJ_O_VEND_ACCT = string.Empty;
            ADJ_O_BILLNAME_TYPE = string.Empty;
            ADJ_O_BILL_LNAME = string.Empty;
            ADJ_O_BILL_FNAME = string.Empty;
            ADJ_O_FUND1 = string.Empty;
            ADJ_O_BUDGET_ID = string.Empty;
            ADJ_O_COST = string.Empty;
            ADJ_O_FOLLUP_ON = string.Empty;
            ADJ_O_FOLLUP_COMP = string.Empty;
            ADJ_O_FUPBY = string.Empty;
            ADJ_O_ACTY_PROG = string.Empty;
            ADJ_N_ACTYR_DATE = string.Empty;
            ADJ_N_ACTY_DATE = string.Empty;
            ADJ_N_CASEWRKR = string.Empty;
            ADJ_N_SITE = string.Empty;
            ADJ_N_VENDOR_NO = string.Empty;
            ADJ_N_VEND_ACCT = string.Empty;
            ADJ_N_BILLNAME_TYPE = string.Empty;
            ADJ_N_BILL_LNAME = string.Empty;
            ADJ_N_BILL_FNAME = string.Empty;
            ADJ_N_FUND1 = string.Empty;
            ADJ_N_BUDGET_ID = string.Empty;
            ADJ_N_COST = string.Empty;
            ADJ_N_FOLLUP_ON = string.Empty;
            ADJ_N_FOLLUP_COMP = string.Empty;
            ADJ_N_FUPBY = string.Empty;
            ADJ_N_ACTY_PROG = string.Empty;
            ADJ_DATE_ADD = string.Empty;
            ADJ_ADD_OPERATOR = string.Empty;
            ADJ_REASON = string.Empty;
            ADJ_Date = string.Empty;
            SPM_O_CASEWRKR = string.Empty;
            SPM_N_CASEWRKR = string.Empty;
            SPM_O_AMOUNT = string.Empty;
            SPM_N_AMOUNT = string.Empty;
            ADJ_O_BILL_PERIOD = string.Empty;
            ADJ_N_BILL_PERIOD = string.Empty;
            ADJ_O_ARREARS = string.Empty;
            ADJ_N_ARREARS = string.Empty;
            ADJ_O_SOURCE = string.Empty;
            ADJ_N_SOURCE = string.Empty;
            SPM_START_DATE = string.Empty;
            ADJ_O_BENEFIT_REASN = string.Empty;
            ADJ_N_BENEFIT_REASN = string.Empty;

        }

        public CaseActAdjEntity(DataRow Row, string strType)
        {
            if (Row != null)
            {
                if (strType == "CASB0021")
                {
                    ADJ_AGENCY = Row["MST_AGENCY"].ToString().Trim();
                    ADJ_DEPT = Row["MST_DEPT"].ToString().Trim();
                    ADJ_PROGRAM = Row["MST_PROGRAM"].ToString().Trim();
                    ADJ_YEAR = Row["MST_YEAR"].ToString().Trim();
                    ADJ_APP_NO = Row["MST_APP_NO"].ToString().Trim();
                    MST_SITE = Row["MST_SITE"].ToString().Trim();
                    ADJ_SERVICE_PLAN = Row["ADJ_SERVICE_PLAN"].ToString().Trim();
                    ADJ_SPM_SEQ = Row["ADJ_SPM_SEQ"].ToString().Trim();
                    ADJ_BRANCH = Row["ADJ_BRANCH"].ToString().Trim();
                    ADJ_GROUP = Row["ADJ_GROUP"].ToString().Trim();
                    ADJ_ACTIVITY_CODE = Row["CASEACT_ACTIVITY_CODE"].ToString().Trim();
                    ADJ_ACT_SEQ = Row["ADJ_ACT_SEQ"].ToString().Trim();
                    ADJ_SEQ = Row["ADJ_SEQ"].ToString().Trim();
                    ADJ_O_ACTYR_DATE = Row["ADJ_O_ACTYR_DATE"].ToString().Trim();
                    ADJ_O_ACTY_DATE = Row["ADJ_O_ACTY_DATE"].ToString().Trim();
                    ADJ_O_CASEWRKR = Row["ADJ_O_CASEWRKR"].ToString().Trim();
                    ADJ_O_SITE = Row["ADJ_O_SITE"].ToString().Trim();
                    ADJ_O_VENDOR_NO = Row["ADJ_O_VENDOR_NO"].ToString().Trim();
                    ADJ_O_VEND_ACCT = Row["ADJ_O_VEND_ACCT"].ToString().Trim();
                    ADJ_O_BILLNAME_TYPE = Row["ADJ_O_BILLNAME_TYPE"].ToString().Trim();
                    ADJ_O_BILL_LNAME = Row["ADJ_O_BILL_LNAME"].ToString().Trim();
                    ADJ_O_BILL_FNAME = Row["ADJ_O_BILL_FNAME"].ToString().Trim();
                    ADJ_O_FUND1 = Row["ADJ_O_FUND1"].ToString().Trim();
                    ADJ_O_BUDGET_ID = Row["ADJ_O_BUDGET_ID"].ToString().Trim();
                    ADJ_O_COST = Row["ADJ_O_COST"].ToString().Trim() == string.Empty?"0": Row["ADJ_O_COST"].ToString();
                    ADJ_O_FOLLUP_ON = Row["ADJ_O_FOLLUP_ON"].ToString().Trim();
                    ADJ_O_FOLLUP_COMP = Row["ADJ_O_FOLLUP_COMP"].ToString().Trim();
                    ADJ_O_FUPBY = Row["ADJ_O_FUPBY"].ToString().Trim();
                    ADJ_O_ACTY_PROG = Row["ADJ_O_ACTY_PROG"].ToString().Trim();
                    ADJ_N_ACTYR_DATE = Row["ADJ_N_ACTYR_DATE"].ToString().Trim();
                    ADJ_N_ACTY_DATE = Row["ADJ_N_ACTY_DATE"].ToString().Trim();
                    ADJ_N_CASEWRKR = Row["ADJ_N_CASEWRKR"].ToString().Trim();
                    ADJ_N_SITE = Row["ADJ_N_SITE"].ToString().Trim();
                    ADJ_N_VENDOR_NO = Row["ADJ_N_VENDOR_NO"].ToString().Trim();
                    ADJ_N_VEND_ACCT = Row["ADJ_N_VEND_ACCT"].ToString().Trim();
                    ADJ_N_BILLNAME_TYPE = Row["ADJ_N_BILLNAME_TYPE"].ToString().Trim();
                    ADJ_N_BILL_LNAME = Row["ADJ_N_BILL_LNAME"].ToString().Trim();
                    ADJ_N_BILL_FNAME = Row["ADJ_N_BILL_FNAME"].ToString().Trim();
                    ADJ_N_FUND1 = Row["ADJ_N_FUND1"].ToString().Trim();
                    ADJ_N_BUDGET_ID = Row["ADJ_N_BUDGET_ID"].ToString().Trim();
                    ADJ_N_COST = Row["ADJ_N_COST"].ToString().Trim() == string.Empty ? "0" : Row["ADJ_N_COST"].ToString(); 
                    ADJ_N_FOLLUP_ON = Row["ADJ_N_FOLLUP_ON"].ToString().Trim();
                    ADJ_N_FOLLUP_COMP = Row["ADJ_N_FOLLUP_COMP"].ToString().Trim();
                    ADJ_N_FUPBY = Row["ADJ_N_FUPBY"].ToString().Trim();
                    ADJ_N_ACTY_PROG = Row["ADJ_N_ACTY_PROG"].ToString().Trim();
                    ADJ_DATE_ADD = Row["ADJ_DATE_ADD"].ToString().Trim();
                    ADJ_ADD_OPERATOR = Row["ADJ_ADD_OPERATOR"].ToString().Trim();
                    ADJ_REASON = Row["ADJ_REASON"].ToString().Trim();
                    ADJ_Date = Row["ADJ_Date"].ToString().Trim();
                    SPM_O_CASEWRKR = Row["SPM_O_CASEWRKR"].ToString().Trim();
                    SPM_N_CASEWRKR = Row["SPM_N_CASEWRKR"].ToString().Trim();
                    SPM_O_AMOUNT = Row["SPM_O_AMOUNT"].ToString().Trim() == string.Empty ? "0" : Row["SPM_O_AMOUNT"].ToString();
                    SPM_N_AMOUNT = Row["SPM_N_AMOUNT"].ToString().Trim() == string.Empty ? "0" : Row["SPM_N_AMOUNT"].ToString();
                    SITE_NAME = Row["SITE_NAME"].ToString().Trim();
                    VendorName = Row["CASEVDD_NAME"].ToString().Trim();
                    SpmName = Row["SP0_DESCRIPTION"].ToString().Trim();
                    ServiceName = Row["CA_DESC"].ToString().Trim();
                    BDC_START = Row["BDC_START"].ToString().Trim();
                    BDC_END = Row["BDC_END"].ToString().Trim();
                    MST_SITE = Row["MST_SITE"].ToString().Trim();
                    SNP_FName = Row["SNP_NAME_IX_FI"].ToString().Trim();
                    SNP_LName = Row["SNP_NAME_IX_LAST"].ToString().Trim();
                    SNP_MName = Row["SNP_NAME_IX_MI"].ToString().Trim();
                    CASEACT_CHECK_DT = Row["CASEACT_CHECK_DT"].ToString().Trim();
                    CASEACT_CHECKNO = Row["CASEACT_CHECK_NO"].ToString().Trim();
                    CASEACT_COST = Row["CASEACT_COST"].ToString().Trim() == string.Empty ? "0" : Row["CASEACT_COST"].ToString();
                    CASEACT_VENDOR_ACCT = Row["CASEACT_VEND_ACCT"].ToString().Trim();
                    BDC_START = Row["BDC_START"].ToString().Trim();
                    BDC_END = Row["BDC_END"].ToString().Trim();
                    SPM_CASE_WRKR = Row["SPM_CASEWORKER"].ToString().Trim();
                    CASEACT_CASEWRKR = Row["CASEACT_CASEWRKR"].ToString().Trim();
                    ADJ_O_BILL_PERIOD = Row["ADJ_O_BILL_PERIOD"].ToString().Trim();
                    ADJ_N_BILL_PERIOD = Row["ADJ_N_BILL_PERIOD"].ToString().Trim();

                    ADJ_O_ARREARS = Row["ADJ_O_ARREARS"].ToString().Trim();
                    ADJ_N_ARREARS = Row["ADJ_N_ARREARS"].ToString().Trim();

                    ADJ_O_SOURCE = Row["ADJ_O_SOURCE"].ToString().Trim();
                    ADJ_N_SOURCE = Row["ADJ_N_SOURCE"].ToString().Trim();

                    SPM_START_DATE = Row["SPM_STARTDATE"].ToString().Trim();

                    ADJ_O_BENEFIT_REASN = Row["ADJ_O_BENEFIT_REASN"].ToString().Trim();
                    ADJ_N_BENEFIT_REASN = Row["ADJ_N_BENEFIT_REASN"].ToString().Trim();
                }
            }

        }

        #endregion

        #region Properties

        public string ADJ_AGENCY { get; set; }
        public string ADJ_DEPT { get; set; }
        public string ADJ_PROGRAM { get; set; }
        public string ADJ_YEAR { get; set; }
        public string ADJ_APP_NO { get; set; }
        public string ADJ_SERVICE_PLAN { get; set; }
        public string ADJ_SPM_SEQ { get; set; }
        public string ADJ_BRANCH { get; set; }
        public string ADJ_GROUP { get; set; }
        public string ADJ_ACTIVITY_CODE { get; set; }
        public string ADJ_ACT_SEQ { get; set; }
        public string ADJ_SEQ { get; set; }
        public string ADJ_O_ACTYR_DATE { get; set; }
        public string ADJ_O_ACTY_DATE { get; set; }
        public string ADJ_O_CASEWRKR { get; set; }
        public string ADJ_O_SITE { get; set; }
        public string ADJ_O_VENDOR_NO { get; set; }
        public string ADJ_O_VEND_ACCT { get; set; }
        public string ADJ_O_BILLNAME_TYPE { get; set; }
        public string ADJ_O_BILL_LNAME { get; set; }
        public string ADJ_O_BILL_FNAME { get; set; }
        public string ADJ_O_FUND1 { get; set; }
        public string ADJ_O_BUDGET_ID { get; set; }
        public string ADJ_O_COST { get; set; }
        public string ADJ_O_FOLLUP_ON { get; set; }
        public string ADJ_O_FOLLUP_COMP { get; set; }
        public string ADJ_O_FUPBY { get; set; }
        public string ADJ_O_ACTY_PROG { get; set; }
        public string ADJ_N_ACTYR_DATE { get; set; }
        public string ADJ_N_ACTY_DATE { get; set; }
        public string ADJ_N_CASEWRKR { get; set; }
        public string ADJ_N_SITE { get; set; }
        public string ADJ_N_VENDOR_NO { get; set; }
        public string ADJ_N_VEND_ACCT { get; set; }
        public string ADJ_N_BILLNAME_TYPE { get; set; }
        public string ADJ_N_BILL_LNAME { get; set; }
        public string ADJ_N_BILL_FNAME { get; set; }
        public string ADJ_N_FUND1 { get; set; }
        public string ADJ_N_BUDGET_ID { get; set; }
        public string ADJ_N_COST { get; set; }
        public string ADJ_N_FOLLUP_ON { get; set; }
        public string ADJ_N_FOLLUP_COMP { get; set; }
        public string ADJ_N_FUPBY { get; set; }
        public string ADJ_N_ACTY_PROG { get; set; }
        public string ADJ_DATE_ADD { get; set; }
        public string ADJ_ADD_OPERATOR { get; set; }
        public string Mode { get; set; }

        public string ADJ_REASON { get; set; }
        public string ADJ_Date { get; set; }

        public string SPM_O_CASEWRKR { get; set; }
        public string SPM_N_CASEWRKR { get; set; }
        public string SPM_O_AMOUNT { get; set; }
        public string SPM_N_AMOUNT { get; set; }

        public string SPM_CASE_WRKR { get; set; }

        public string SITE_NAME { get; set; }

        public string ServiceName { get; set; }
        public string SpmName { get; set; }
        public string MST_SITE { get; set;}

        public string CASEACT_VENDOR { get; set; }
        public string CASEACT_VENDOR_ACCT { get; set; }
        public string CASEACT_CASEWRKR { get; set; }

        public string SNP_LName { get; set; }

        public string SNP_FName { get; set; }

        public string SNP_MName { get; set; }
        public string VendorName { get; set; }

        public string CASEACT_CHECK_DT { get; set; }
        public string CASEACT_CHECKNO { get; set; }
        public string CASEACT_COST { get; set; }

        public string BDC_START { get; set; }
        public string BDC_END { get; set; }

        public string ADJ_O_BILL_PERIOD { get; set; }
        public string ADJ_N_BILL_PERIOD { get; set; }

        public string ADJ_O_ARREARS { get; set; }
        public string ADJ_N_ARREARS { get; set; }

        public string ADJ_O_SOURCE { get; set; }
        public string ADJ_N_SOURCE { get; set; }

        public string ADJ_O_BENEFIT_REASN { get; set; }
        public string ADJ_N_BENEFIT_REASN { get; set; }
        public string SPM_START_DATE { get; set; }


        #endregion
    }

    public class TargetEntity   // Added by Vikash for Service Plan Target Entry on 03/25/2023
    {
        #region  Constructors

        public TargetEntity()
        {
            RNG_Code = string.Empty;
           // SP_Code = string.Empty;
            GRP_Code= string.Empty;
            TBL_Code = string.Empty;
            GOAL_Code = string.Empty;
            CAMS_Type = string.Empty;
            //RNG_Agy = string.Empty;
            SP_Agy = string.Empty;
            SP_Dept = string.Empty;
            SP_Prog = string.Empty;
            Target = string.Empty;
            RNG_GRPDesc = string.Empty;
            RNG_TBLDesc = string.Empty;
            RNG_Desc = string.Empty;
            PROG_Name = string.Empty;
            Budget = string.Empty;
            CAMSCount = string.Empty;
            Active = string.Empty;
            CAActive=string.Empty;
            SP2_Branch = string.Empty;
            SP2_Org_Grp = string.Empty;
            SP2_Curr_Grp = string.Empty;
            SP2_Seq = string.Empty;
            SP2_OBF = string.Empty;
            SP2_COUNT_TYPE = string.Empty;
        }

        public TargetEntity(DataRow Row)
        {
            if (Row != null)
            {
                RNG_Code = Row["RNGSRGA_CODE"].ToString().Trim();
               // SP_Code = Row[""].ToString().Trim();
                GRP_Code = Row["RNGSRGA_GROUP_CODE"].ToString().Trim();
                TBL_Code = Row["RNGSRGA_TABLE_CODE"].ToString().Trim();
                GOAL_Code = Row["RNGSRGA_GOAL_CODE"].ToString().Trim();
                CAMS_Type = Row["CAMS_TYPE"].ToString().Trim();
                //RNG_Agy = Row[""].ToString().Trim();
                SP_Agy = Row["SP1_AGENCY"].ToString().Trim();
                SP_Dept = Row["SP1_DEPT"].ToString().Trim();
                SP_Prog = Row["SP1_PROGRAM"].ToString().Trim();
                //Target = Row["RNGSRGAH_TARGET"].ToString().Trim();
                RNG_GRPDesc = Row["RNGSRGRP_GROUP"].ToString().Trim();
                RNG_TBLDesc = Row["RNGSRGRP_TABLE"].ToString().Trim();
                RNG_Desc = Row["RNGSRGA_DESC"].ToString().Trim();
                PROG_Name = Row["PROG_NAME"].ToString().Trim();
                Budget = Row["RNGSRGA_BUDGET"].ToString().Trim();
                CAMSCount= Row["SCOUNT"].ToString().Trim();
                Active = Row["SP2_ACTIVE"].ToString().Trim();
                CAActive = Row["CA_ACTIVE"].ToString().Trim();
                SP2_Branch = Row["SP2_BRANCH"].ToString().Trim();
                SP2_Org_Grp = Row["SP2_ORIG_GRP"].ToString().Trim();
                SP2_Curr_Grp = Row["SP2_CURR_GRP"].ToString().Trim();
                SP2_Seq = Row["SP2_ROW"].ToString().Trim();

                SP2_OBF = Row["SP2_OBF"].ToString().Trim();
                SP2_COUNT_TYPE = Row["SP2_COUNT_TYPE"].ToString().Trim();
                Target = string.Empty;
            }
        }

        #endregion

        #region Properties

        public string RNG_Code { get; set; }
       // public string SP_Code { get; set; }
        public string GRP_Code { get; set; }
        public string TBL_Code { get; set; }
        public string GOAL_Code { get; set; }
        public string CAMS_Type { get; set; }
        //public string RNG_Agy { get; set; }
        public string SP_Agy { get; set; }
        public string SP_Dept { get; set; }
        public string SP_Prog { get; set; }
        public string Target { get; set; }
        public string RNG_GRPDesc { get; set; }
        public string RNG_TBLDesc { get; set; }
        public string RNG_Desc { get; set; }
        public string PROG_Name { get; set; }

        public string Budget { get; set; }

        public string Active { get; set; }

        public string CAActive { get; set; }

        public string CAMSCount { get; set; }

        public string SP2_Branch { get; set; }
        public string SP2_Org_Grp { get; set; }
        public string SP2_Curr_Grp { get; set; }
        public string SP2_Seq { get; set; }

        public string SP2_OBF { get; set; }
        public string SP2_COUNT_TYPE { get; set; }

        #endregion
    }

    public class SPTargetEntity
    {
        #region  Constructors

        public SPTargetEntity()
        {
            SPT_DT_CODE = string.Empty;
            SPT_AGENCY = string.Empty;
            SPT_SP = string.Empty;
            SPT_TYPE = string.Empty;
            SPT_CAMS_CODE = string.Empty;
            SPT_DESC = string.Empty;
            //RNG_Agy = string.Empty;
            SPT_INTERVENTION = string.Empty;
            SPT_DATE_ADD = string.Empty;
            SPT_ADD_OPERATOR = string.Empty;
            SPT_DATE_LSTC = string.Empty;
            SPT_LSTC_OPERATOR = string.Empty;
            SP0_Bulk = string.Empty;
            Mode = string.Empty;
        }

        public SPTargetEntity(DataRow Row)
        {
            if (Row != null)
            {
                SPT_DT_CODE = Row["SPT_DT_CODE"].ToString().Trim();
                SPT_AGENCY = Row["SPT_AGENCY"].ToString().Trim();
                SPT_SP = Row["SPT_SP"].ToString().Trim();
                SPT_TYPE = Row["SPT_TYPE"].ToString().Trim();
                SPT_CAMS_CODE = Row["SPT_CAMS_CODE"].ToString().Trim();
                SPT_DESC = Row["SPT_DESC"].ToString().Trim();
                //RNG_Agy = Row[""].ToString().Trim();
                SPT_INTERVENTION = Row["SPT_INTERVENTION"].ToString().Trim();
                SPT_DATE_ADD = Row["SPT_DATE_ADD"].ToString().Trim();
                SPT_ADD_OPERATOR = Row["SPT_ADD_OPERATOR"].ToString().Trim();
                SPT_DATE_LSTC = Row["SPT_DATE_LSTC"].ToString().Trim();
                SPT_LSTC_OPERATOR = Row["SPT_LSTC_OPERATOR"].ToString().Trim();
                
            }
        }

        #endregion

        #region Properties

        public string Mode { get; set; }
        public string SPT_DT_CODE { get; set; }
        // public string SP_Code { get; set; }
        public string SPT_AGENCY { get; set; }
        public string SPT_SP { get; set; }
        public string SPT_TYPE { get; set; }
        public string SPT_CAMS_CODE { get; set; }
        public string SPT_DESC { get; set; }
        //public string RNG_Agy { get; set; }
        public string SPT_INTERVENTION { get; set; }
        public string SPT_DATE_ADD { get; set; }
        public string SPT_ADD_OPERATOR { get; set; }
        public string SPT_DATE_LSTC { get; set; }
        public string SPT_LSTC_OPERATOR { get; set; }
        
        public string SP0_Bulk { get; set; }

        #endregion
    }

    #region Added by Vikash on 03/14/2024 as per APC_UPDATE AP DATA SCREEN document
    public class AGCYPARTREPEntity
    {
        #region Constructors

        public AGCYPARTREPEntity()
        {
            Code = string.Empty;
            Name = string.Empty;
            City = string.Empty;
            County = string.Empty;
            ServicePlans = string.Empty;
            RefAsoc = string.Empty;

            Street = string.Empty;
            State = string.Empty;
            Lstc_Date = string.Empty;
            Lsct_Operator = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;

            Boutique = string.Empty;
            Services = string.Empty;

            Active = string.Empty;
            AGYP_FORMS_STATUS = string.Empty;

            RepName = string.Empty;
            RepLName = string.Empty;
            RepPhone = string.Empty;
            RepPhone2 = string.Empty;
            RepEmail = string.Empty;
            RepPosition = string.Empty;
            RepLead = string.Empty;
        }

        public AGCYPARTREPEntity(bool Initialize)
        {
            if (Initialize)
            {
                Code = null;
                Name = null;
                City = null;
                County = null;
                ServicePlans = null;
                RefAsoc = null;

                Street = null;
                State = null;
                Lstc_Date = null;
                Lsct_Operator = null;
                Add_Date = null;
                Add_Operator = null;

                Services = null;
                Boutique = null;

                Active = null;
                AGYP_FORMS_STATUS = null;

                RepName = null;
                RepLName = null;
                RepPhone = null;
                RepPhone2 = null;
                RepEmail = null;
                RepPosition = null;
                RepLead = null;
            }

        }

        public AGCYPARTREPEntity(DataRow row)
        {
            if (row != null)
            {
                Code = row["AGYP_CODE"].ToString();
                Name = row["AGYP_NAME"].ToString();
                City = row["AGYP_City"].ToString();
                County = row["AGYP_COUNTY"].ToString();
                ServicePlans = row["AGYP_SERPLANS"].ToString();
                RefAsoc = row["AGYP_REF_ASOC"].ToString();

                Street = row["AGYP_Street"].ToString();
                State = row["AGYP_State"].ToString();
                Lstc_Date = row["AGYP_Date_Lstc"].ToString();
                Lsct_Operator = row["AGYP_Lstc_Operator"].ToString();
                Add_Date = row["AGYP_Date_Add"].ToString();
                Add_Operator = row["AGYP_Add_Operator"].ToString();

                Boutique = row["AGYP_BOUTIQUE"].ToString();
                Services = row["AGYS_SERVICE"].ToString();

                Active = row["AGYP_Active"].ToString();
                AGYP_FORMS_STATUS = row["AGYP_FORMS_STATUS"].ToString();

                RepName = row["AGYR_REP_FNAME"].ToString();
                RepLName = row["AGYR_REP_LNAME"].ToString();
                RepPhone = row["AGYR_REP_PHONE1"].ToString();
                RepPhone2 = row["AGYR_REP_PHONE2"].ToString();
                RepPosition = row["AGYR_REP_POS"].ToString();
                RepEmail = row["AGYR_REP_EMAIL"].ToString();
                RepLead = row["AGYR_REP_REQLEAD"].ToString();

            }
        }

        #endregion

        #region Properties
        public string Code
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string City
        {
            get; set;
        }
        public string County
        {
            get; set;
        }
        public string Boutique
        {
            get; set;
        }
        public string Services
        {
            get;
            set;
        }
        public string Active
        {
            get; set;
        }
        public string AGYP_FORMS_STATUS
        {
            get; set;
        }
        public string ServicePlans
        {
            get; set;
        }
        public string RefAsoc
        {
            get;
            set;
        }
        public string RepName
        {
            get;
            set;
        }
        public string RepLName
        {
            get;
            set;
        }
        public string RepPhone
        {
            get;
            set;
        }
        public string RepPhone2
        {
            get;
            set;
        }
        public string RepPosition
        {
            get;
            set;
        }
        public string RepEmail
        {
            get;
            set;
        }
        public string RepLead
        {
            get;
            set;
        }

        public string Street
        {
            get; set;
        }
        public string State
        {
            get; set;
        }
        public string Lstc_Date
        {
            get; set;
        }
        public string Lsct_Operator
        {
            get; set;
        }
        public string Add_Date
        {
            get; set;
        }
        public string Add_Operator
        {
            get; set;
        }
        #endregion
    }
    #endregion
}
