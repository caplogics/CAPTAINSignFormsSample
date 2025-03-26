using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class CaseEnrlEntity
    {
        #region Constructors

        public CaseEnrlEntity()
        {
            Join_Mst_Snp =
            Row_Type =
            Agy =
            Dept =
            Prog =
            Year =
            App =
            Group =
            FundHie =
            Seq =
            ID =
            Rec_Type =
            Status =
            Status_Reason =
            Status_Date =
            Site =
            Room =
            AMPM =
            Enrl_Date =
            Withdraw_Code =
            Withdraw_Date =
            Wiait_Date =
            Denied_Code =
            Denied_Date =
            Pending_Code =
            Pending_Date =
            Rank =
            Rank_Chg_Code =
            Transc_Type =
            Tranfr_Site =
            Tranfr_Room =
            Tranfr_AMPM =
            Parent_Rate =
            Funding_Code =
            Funding_Rate =
            Desc_1 =
            Desc_2 =
            Fund_End_date =
            Rate_EFR_date =
            Add_Date =
            Add_Oper =
            Lstc_Date =
            Lstc_Oper =
            Enrl_Denied =
            Enrl_Hist_Cnt =
            Enrl_Max_Attn_Date =
            Enrl_Min_Attn_Date =
            Enrl_Stat_Attn_FDate =
            Enrl_Stat_Attn_LDate =
            Enrl_Pref_Class =
            Transfer_ID =
            Enrl_Status_Not_Equalto =
            Mult_Fund_Site_List = string.Empty;


            Snp_F_Name = Snp_L_Name = Snp_M_Name = Snp_DOB = Mst_Hno = Mst_App = Mst_Street = Mst_City =
            Mst_State = Mst_Zip = Mst_Zip_Plus = Snp_Sex = Mst_Phone = Mst_Rank1 = Mst_Rank2 = Mst_Rank3 = Mst_Rank4 =
            Mst_Rank5 = Mst_Rank6 = Snp_Ethnic = Snp_Race = MST_Classification = MST_Language = Cond_Cnt = KG_Age = MST_POVERTY =
            MST_FAMILY_ID =
            MST_FAMILY_TYPE =
            MST_INCOME_TYPE = MST_INTAKE_WORKER = string.Empty;

            Asof_Status = Asof_Seq = Asof_From_Date = Asof_TO_Date = Asof_Site = Asof_Room =
            Asof_AMPM = Asof_Date_Add = Asof_Add_Opr = Asof_Date = History_From = Snp_Age = string.Empty;
        }

        public CaseEnrlEntity(bool Initialize)
        {
            if (Initialize)
            {
                Join_Mst_Snp =
                Row_Type =
                Agy =
                Dept =
                Prog =
                Year =
                App =
                Group =
                FundHie =
                Seq =
                ID =
                Rec_Type =
                Status =
                Status_Reason =
                Status_Date =
                Site =
                Room =
                AMPM =
                Enrl_Date =
                Withdraw_Code =
                Withdraw_Date =
                Wiait_Date =
                Denied_Code =
                Denied_Date =
                Pending_Code =
                Pending_Date =
                Rank =
                Rank_Chg_Code =
                Transc_Type =
                Tranfr_Site =
                Tranfr_Room =
                Tranfr_AMPM =
                Parent_Rate =
                Funding_Code =
                Funding_Rate =
                Desc_1 =
                Desc_2 =
                Fund_End_date =
                Rate_EFR_date =
                Add_Date =
                Add_Oper =
                Lstc_Date =
                Lstc_Oper =
                Enrl_Denied =
                Enrl_Hist_Cnt =
                Enrl_Max_Attn_Date =
                Enrl_Min_Attn_Date =
                Enrl_Stat_Attn_FDate =
                Enrl_Stat_Attn_LDate =
                Enrl_Pref_Class =
                Transfer_ID =
                Enrl_Status_Not_Equalto = null;


                Snp_F_Name =
                Snp_L_Name =
                Snp_M_Name =
                Snp_DOB =
                Snp_Sex =
                Snp_Age =
                Snp_Ethnic =
                Snp_Race = MST_Classification = MST_Language = KG_Age = null;

                Mst_Hno =
                Mst_App =
                Mst_Street =
                Mst_City =
                Mst_State =
                Mst_Zip =
                Mst_Zip_Plus =
                Mst_Phone =
                Mst_Site =
                Mst_Rank1 =
                Mst_Rank2 = Mst_Rank3 = Mst_Rank4 =
                Mst_Rank5 = Mst_Rank6 =
                Cond_Cnt =
                Mult_Fund_Site_List = MST_INTAKE_WORKER = null;

                Asof_Status =
                Asof_Seq =
                Asof_From_Date =
                Asof_TO_Date =
                Asof_Site =
                Asof_Room =
                Asof_AMPM =
                Asof_Date_Add =
                Asof_Add_Opr =
                Asof_Date =
                History_From = null;
            }
        }

        public CaseEnrlEntity(CaseEnrlEntity Entity)
        {
            Join_Mst_Snp = Entity.Join_Mst_Snp;
            Row_Type = Entity.Row_Type;
            Agy = Entity.Agy;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            Year = Entity.Year;
            App = Entity.App;
            Group = Entity.Group;
            FundHie = Entity.FundHie;
            Seq = Entity.Seq;
            ID = Entity.ID;
            Rec_Type = Entity.Rec_Type;
            Status = Entity.Status;
            Status_Reason = Entity.Status_Reason;
            Status_Date = Entity.Status_Date;
            Site = Entity.Site;
            Room = Entity.Room;
            AMPM = Entity.AMPM;
            Enrl_Date = Entity.Enrl_Date;
            Withdraw_Code = Entity.Withdraw_Code;
            Withdraw_Date = Entity.Withdraw_Date;
            Wiait_Date = Entity.Wiait_Date;
            Denied_Code = Entity.Denied_Code;
            Denied_Date = Entity.Denied_Date;
            Pending_Code = Entity.Pending_Code;
            Pending_Date = Entity.Pending_Date;
            Rank = Entity.Rank;
            Rank_Chg_Code = Entity.Rank_Chg_Code;
            Transc_Type = Entity.Transc_Type;
            Tranfr_Site = Entity.Tranfr_Site;
            Tranfr_Room = Entity.Tranfr_Room;
            Tranfr_AMPM = Entity.Tranfr_AMPM;
            Parent_Rate = Entity.Parent_Rate;
            Funding_Code = Entity.Funding_Code;
            Funding_Rate = Entity.Funding_Rate;
            Desc_1 = Entity.Desc_1;
            Desc_2 = Entity.Desc_2;
            Fund_End_date = Entity.Fund_End_date;
            Rate_EFR_date = Entity.Rate_EFR_date;
            Add_Date = Entity.Add_Date;
            Add_Oper = Entity.Add_Oper;
            Lstc_Date = Entity.Lstc_Date;
            Lstc_Oper = Entity.Lstc_Oper;
            Enrl_Denied = Entity.Enrl_Denied;
            Enrl_Hist_Cnt = Entity.Enrl_Hist_Cnt;
            Enrl_Max_Attn_Date = Entity.Enrl_Max_Attn_Date;
            Enrl_Min_Attn_Date = Entity.Enrl_Min_Attn_Date;
            Enrl_Stat_Attn_FDate = Entity.Enrl_Stat_Attn_FDate;
            Enrl_Stat_Attn_LDate = Entity.Enrl_Stat_Attn_LDate;
            Enrl_Pref_Class = Entity.Enrl_Pref_Class;
            Transfer_ID = Entity.Transfer_ID;
            Enrl_Status_Not_Equalto = Entity.Enrl_Status_Not_Equalto;

            Snp_F_Name = Entity.Snp_F_Name;
            Snp_L_Name = Entity.Snp_L_Name;
            Snp_M_Name = Entity.Snp_M_Name;
            Snp_DOB = Entity.Snp_DOB;
            Snp_Sex = Entity.Snp_Sex;
            Snp_Age = Entity.Snp_Age;
            Snp_Ethnic = Entity.Snp_Ethnic;
            Snp_Race = Entity.Snp_Race;
            KG_Age = Entity.KG_Age;

            Mst_Hno = Entity.Mst_Hno;
            Mst_App = Entity.Mst_App;
            Mst_Street = Entity.Mst_Street;
            Mst_City = Entity.Mst_City;
            Mst_State = Entity.Mst_State;
            Mst_Zip = Entity.Mst_Zip;
            Mst_Zip_Plus = Entity.Mst_Zip_Plus;
            Mst_Phone = Entity.Mst_Phone;
            Mst_Site = Entity.Mst_Site;
            Mst_Rank1 = Entity.Mst_Rank1;
            Mst_Rank2 = Entity.Mst_Rank2;
            Mst_Rank3 = Entity.Mst_Rank3;
            Mst_Rank4 = Entity.Mst_Rank4;
            Mst_Rank5 = Entity.Mst_Rank5;
            Mst_Rank6 = Entity.Mst_Rank6;
            MST_Classification = Entity.MST_Classification;
            MST_Language = Entity.MST_Language;
            Cond_Cnt = Entity.Cond_Cnt;
            Mult_Fund_Site_List = Entity.Mult_Fund_Site_List;

            MST_POVERTY = Entity.MST_POVERTY;
            MST_FAMILY_ID = Entity.MST_FAMILY_ID;
            MST_FAMILY_TYPE = Entity.MST_FAMILY_TYPE;
            MST_INCOME_TYPE = Entity.MST_INCOME_TYPE;
            MST_INTAKE_WORKER = Entity.MST_INTAKE_WORKER;


            Asof_Status = Entity.Asof_Status;
            Asof_Seq = Entity.Asof_Seq;
            Asof_From_Date = Entity.Asof_From_Date;
            Asof_TO_Date = Entity.Asof_TO_Date;
            Asof_Site = Entity.Asof_Site;
            Asof_Room = Entity.Asof_Room;
            Asof_AMPM = Entity.Asof_AMPM;
            Asof_Date_Add = Entity.Asof_Date;
            Asof_Add_Opr = Entity.Asof_Add_Opr;
            Asof_Date = Entity.Asof_Date;
            History_From = Entity.History_From;

        }

        public CaseEnrlEntity(DataRow row)
        {
            Agy = row["ENRL_AGENCY"].ToString().Trim();
            Dept = row["ENRL_DEPT"].ToString().Trim();
            Prog = row["ENRL_PROG"].ToString().Trim();
            Year = row["ENRL_YEAR"].ToString().Trim();
            App = row["ENRL_APP_NO"].ToString().Trim();
            Group = row["ENRL_GROUP"].ToString().Trim();
            FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
            Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
            Seq = row["ENRL_SEQ"].ToString().Trim();
            ID = row["ENRL_ID"].ToString().Trim();
            Site = row["ENRL_SITE"].ToString().Trim();
            Room = row["ENRL_ROOM"].ToString().Trim();
            AMPM = row["ENRL_AMPM"].ToString().Trim();
            Status = row["ENRL_STATUS"].ToString().Trim();
            Status_Reason = row["ENRL_STATUS_REASN"].ToString().Trim();
            Status_Date = row["ENRL_DATE"].ToString().Trim();
            Enrl_Date = row["ENRL_ENRLD_DATE"].ToString().Trim();
            Withdraw_Code = row["ENRL_WDRAW_CODE"].ToString().Trim();
            Withdraw_Date = row["ENRL_WDRAW_DATE"].ToString().Trim();
            Wiait_Date = row["ENRL_WLIST_DATE"].ToString().Trim();
            Denied_Code = row["ENRL_DENIED_CODE"].ToString().Trim();
            Denied_Date = row["ENRL_DENIED_DATE"].ToString().Trim();
            Pending_Code = row["ENRL_PENDING_CODE"].ToString().Trim();
            Pending_Date = row["ENRL_PENDING_DATE"].ToString().Trim();
            Rank = row["ENRL_RANK"].ToString().Trim();
            Rank_Chg_Code = row["ENRL_RNKCHNG_CODE"].ToString().Trim();
            Transc_Type = row["ENRL_TRAN_TYPE"].ToString().Trim();
            Tranfr_Site = row["ENRL_TRANSFER_SITE"].ToString().Trim();
            Tranfr_Room = row["ENRL_TRANSFER_ROOM"].ToString().Trim();
            Tranfr_AMPM = row["ENRL_TRANSFER_AMPM"].ToString().Trim();
            Parent_Rate = row["ENRL_PARENT_RATE"].ToString().Trim();
            Funding_Code = row["ENRL_FUNDING_CODE"].ToString().Trim();
            Funding_Rate = row["ENRL_FUNDING_RATE"].ToString().Trim();
            Desc_1 = row["ENRL_DESC_1"].ToString().Trim();
            Desc_2 = row["ENRL_DESC_2"].ToString().Trim();
            Fund_End_date = row["ENRL_FUND_END_DATE"].ToString().Trim();
            Rate_EFR_date = row["ENRL_RATE_EFF_DATE"].ToString().Trim();
            Add_Date = row["ENRL_DATE_ADD"].ToString().Trim();
            Add_Oper = row["ENRL_ADD_OPERATOR"].ToString().Trim();
            Lstc_Date = row["ENRL_DATE_LSTC"].ToString().Trim();
            Lstc_Oper = row["ENRL_LSTC_OPERATOR"].ToString().Trim();
            Enrl_Denied = row["ENRL_DENIED"].ToString().Trim();
            //Enrl_Hist_Cnt = row["Hist_Count"].ToString().Trim();
            Enrl_Pref_Class = row["ENRL_PREFERRED_CLASS"].ToString().Trim();
            Transfer_ID = row["ENRL_TRANSFER_ID"].ToString().Trim();
        }

        public CaseEnrlEntity(DataRow row, string Join_SW)
        {
            if (row != null)
            {
                Join_Mst_Snp = Join_SW;
                Row_Type = "U";
                Agy = row["ENRL_AGENCY"].ToString().Trim();
                Dept = row["ENRL_DEPT"].ToString().Trim();
                Prog = row["ENRL_PROG"].ToString().Trim();
                Year = row["ENRL_YEAR"].ToString().Trim();
                App = row["ENRL_APP_NO"].ToString().Trim();
                Group = row["ENRL_GROUP"].ToString().Trim();
                FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                Seq = row["ENRL_SEQ"].ToString().Trim();
                ID = row["ENRL_ID"].ToString().Trim();
                Site = row["ENRL_SITE"].ToString().Trim();
                Room = row["ENRL_ROOM"].ToString().Trim();
                AMPM = row["ENRL_AMPM"].ToString().Trim();
                Status = row["ENRL_STATUS"].ToString().Trim();
                Status_Reason = row["ENRL_STATUS_REASN"].ToString().Trim();
                Status_Date = row["ENRL_DATE"].ToString().Trim();
                Enrl_Date = row["ENRL_ENRLD_DATE"].ToString().Trim();
                Withdraw_Code = row["ENRL_WDRAW_CODE"].ToString().Trim();
                Withdraw_Date = row["ENRL_WDRAW_DATE"].ToString().Trim();
                Wiait_Date = row["ENRL_WLIST_DATE"].ToString().Trim();
                Denied_Code = row["ENRL_DENIED_CODE"].ToString().Trim();
                Denied_Date = row["ENRL_DENIED_DATE"].ToString().Trim();
                Pending_Code = row["ENRL_PENDING_CODE"].ToString().Trim();
                Pending_Date = row["ENRL_PENDING_DATE"].ToString().Trim();
                Rank = row["ENRL_RANK"].ToString().Trim();
                Rank_Chg_Code = row["ENRL_RNKCHNG_CODE"].ToString().Trim();
                Transc_Type = row["ENRL_TRAN_TYPE"].ToString().Trim();
                Tranfr_Site = row["ENRL_TRANSFER_SITE"].ToString().Trim();
                Tranfr_Room = row["ENRL_TRANSFER_ROOM"].ToString().Trim();
                Tranfr_AMPM = row["ENRL_TRANSFER_AMPM"].ToString().Trim();
                Parent_Rate = row["ENRL_PARENT_RATE"].ToString().Trim();
                Funding_Code = row["ENRL_FUNDING_CODE"].ToString().Trim();
                Funding_Rate = row["ENRL_FUNDING_RATE"].ToString().Trim();
                Desc_1 = row["ENRL_DESC_1"].ToString().Trim();
                Desc_2 = row["ENRL_DESC_2"].ToString().Trim();
                Fund_End_date = row["ENRL_FUND_END_DATE"].ToString().Trim();
                Rate_EFR_date = row["ENRL_RATE_EFF_DATE"].ToString().Trim();
                Add_Date = row["ENRL_DATE_ADD"].ToString().Trim();
                Add_Oper = row["ENRL_ADD_OPERATOR"].ToString().Trim();
                Lstc_Date = row["ENRL_DATE_LSTC"].ToString().Trim();
                Lstc_Oper = row["ENRL_LSTC_OPERATOR"].ToString().Trim();
                Enrl_Denied = row["ENRL_DENIED"].ToString().Trim();
                Enrl_Hist_Cnt = row["Hist_Count"].ToString().Trim();
                Enrl_Pref_Class = row["ENRL_PREFERRED_CLASS"].ToString().Trim();
                Transfer_ID = row["ENRL_TRANSFER_ID"].ToString().Trim();
                Enrl_Status_Not_Equalto = string.Empty;
                Mult_Fund_Site_List = row["Mult_Fund_Site_List"].ToString().Trim();

                Snp_F_Name = Snp_L_Name = Snp_M_Name = Snp_DOB = Mst_Hno = Mst_App = Mst_Street = Mst_City =
                Mst_State = Mst_Zip = Mst_Zip_Plus = Snp_Sex = string.Empty;

                if (Join_SW == "Y")
                {
                    Enrl_Max_Attn_Date = row["Attan_Date"].ToString().Trim();
                    Enrl_Min_Attn_Date = row["Attan_FDate"].ToString().Trim();
                    Enrl_Stat_Attn_FDate = row["Attn_FDate"].ToString().Trim();
                    Enrl_Stat_Attn_LDate = row["Attn_LDate"].ToString().Trim();

                    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                    Snp_Sex = row["SNP_SEX"].ToString().Trim();
                    Snp_Age = row["SNP_AGE"].ToString().Trim();
                    Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                    Snp_Ethnic = row["SNP_ETHNIC"].ToString().Trim();
                    Snp_Race = row["SNP_RACE"].ToString().Trim();
                    KG_Age = row["KGarden_Age"].ToString().Trim();

                    Mst_App = row["MST_APP_NO"].ToString().Trim();
                    Mst_Hno = row["MST_HN"].ToString().Trim();
                    Mst_Street = row["MST_STREET"].ToString().Trim();
                    Mst_City = row["MST_CITY"].ToString().Trim();
                    Mst_State = row["MST_STATE"].ToString().Trim();
                    Mst_Zip = row["MST_ZIP"].ToString().Trim();
                    Mst_Zip_Plus = row["MST_ZIPPLUS"].ToString().Trim();
                    Mst_Phone = row["MST_AREA"].ToString().Trim() + row["MST_PHONE"].ToString().Trim();
                    Mst_Site = row["MST_SITE"].ToString().Trim();
                    Mst_Rank1 = row["MST_RANK1"].ToString().Trim();
                    Mst_Rank2 = row["MST_RANK2"].ToString().Trim();
                    Mst_Rank3 = row["MST_RANK3"].ToString().Trim();
                    Mst_Rank4 = row["MST_RANK4"].ToString().Trim();
                    Mst_Rank5 = row["MST_RANK5"].ToString().Trim();
                    Mst_Rank6 = row["MST_RANK6"].ToString().Trim();
                    MST_Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                    MST_Language = row["MST_LANGUAGE"].ToString().Trim();
                    MST_INTAKE_DT = row["MST_INTAKE_DATE"].ToString();
                    Cond_Cnt = row["Cond_Count"].ToString().Trim();
                    MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                    MST_POVERTY = row["MST_POVERTY"].ToString().Trim();
                    MST_FAMILY_ID = row["MST_FAMILY_ID"].ToString().Trim();
                    MST_FAMILY_TYPE = row["MST_FAMILY_TYPE"].ToString().Trim();
                    MST_INCOME_TYPE = row["MST_INCOME_TYPES"].ToString().Trim();
                    MST_INTAKE_WORKER = row["MST_INTAKE_WORKER"].ToString().Trim();


                }

                Asof_Date = string.Empty;

                History_From = row["History_From"].ToString().Trim();
                //Asof_Status = row["MST_ZIP"].ToString().Trim();
                //Asof_Seq = row["MST_ZIP"].ToString().Trim();
                //Asof_From_Date = row["MST_ZIP"].ToString().Trim();
                //Asof_TO_Date = row["MST_ZIP"].ToString().Trim();
                //Asof_Site = row["MST_ZIP"].ToString().Trim();
                //Asof_Room = row["MST_ZIP"].ToString().Trim();
                //Asof_AMPM = row["MST_ZIP"].ToString().Trim();
                //Asof_Date_Add = row["MST_ZIP"].ToString().Trim();
                //Asof_Add_Opr = row["MST_ZIP"].ToString().Trim();
                //Asof_Date = row["MST_ZIP"].ToString().Trim();

            }

        }

        public CaseEnrlEntity(DataRow row, string Join_SW, string strTable)
        {
            if (row != null)
            {
                switch (strTable)
                {
                    case "HSSB2109":
                        MST_Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                        Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        Snp_Age = row["Age"].ToString().Trim();
                        Snp_AgeMonth = row["AgeMonth"].ToString().Trim();
                        Snp_Gardian = row["GardianName"].ToString().Trim();
                        if (row["MST_PHONE"].ToString().Trim() != "" && row["MST_PHONE"].ToString().Length > 6)
                            Snp_TelePhone = row["MST_AREA"].ToString().Trim() + "  " + row["MST_PHONE"].ToString().Substring(0, 3) + " - " + row["MST_PHONE"].ToString().Substring(3, 4);
                        ConsecutiveDays = row["ConsectiveStatus"].ToString().Trim();
                        break;
                    case "HSSB2103":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        Snp_Age = row["Age"].ToString().Trim();
                        Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                        Mst_Zip = row["MST_Zip"].ToString().Trim();
                        MST_WAIT_LIST = row["MST_WAIT_LIST"].ToString().Trim();
                        Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                        AttendedDt = row["Attended"].ToString().Trim();
                        Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                        BusRoot = row["Busrote"].ToString().Trim();

                        break;
                    case "HSSB2114":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        Snp_Age = row["Age"].ToString().Trim();
                        Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                        //  Mst_Zip = row["MST_Zip"].ToString().Trim();
                        // MST_WAIT_LIST = row["MST_WAIT_LIST"].ToString().Trim();
                        //Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                        //AttendedDt = row["Attended"].ToString().Trim();
                        Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                        MSTZIPDATE = row["ZIPDATE"].ToString();
                        MST_INTAKE_DT = row["MST_INTAKE_DATE"].ToString();
                        AttendedDt = row["ATTN_DATE"].ToString();
                        break;
                    case "HSSB2111SUMMARY":
                        Agy = row["ATTN_AGENCY"].ToString().Trim();
                        Dept = row["ATTN_DEPT"].ToString().Trim();
                        Prog = row["ATTN_PROGRAM"].ToString().Trim();
                        Year = row["ATTN_YEAR"].ToString().Trim();
                        Site = row["ATTN_SITE"].ToString().Trim();
                        Breakfast = row["BREAKFAST"].ToString().Trim();
                        Lunch = row["LUNCH"].ToString().Trim();
                        Supper = row["AMSNACKS"].ToString().Trim();
                        Suppliment = row["PMSNACKS"].ToString().Trim();
                        Free = row["FREE"].ToString().Trim();
                        Reduced = row["Reduced"].ToString().Trim();
                        OverIncome = row["OverIncome"].ToString().Trim();
                        ClassDays = row["ClassDays"].ToString();
                        AttandanceDays = row["AttandanceDays"].ToString();
                        break;
                    case "HSSB2111DETAILS":
                        Agy = row["ATTN_AGENCY"].ToString().Trim();
                        Dept = row["ATTN_DEPT"].ToString().Trim();
                        Prog = row["ATTN_PROGRAM"].ToString().Trim();
                        // Year = row["ATTN_YEAR"].ToString().Trim();
                        Site = row["ATTN_SITE"].ToString().Trim();
                        Room = row["ATTN_ROOM"].ToString().Trim();
                        AMPM = row["ATTN_AMPM"].ToString().Trim();
                        FundHie = row["ATTN_FUNDING_SOURCE"].ToString().Trim();
                        EndEnrollment = row["ENDENrollment"].ToString().Trim();
                        FundEnrollment = row["FUNDENROLLMENT"].ToString().Trim();
                        ClassDays = row["ClassDays"].ToString().Trim();
                        AvailbleDays = row["AvailableDays"].ToString().Trim();
                        ExcusedDays = row["ExecusedDays"].ToString().Trim();
                        AttandanceDays = row["AttandanceDays"].ToString().Trim();
                        break;
                    case "PIR20000":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        Enrl_Stat_Attn_FDate = row["FDate"].ToString().Trim();
                        Enrl_Stat_Attn_LDate = row["LDate"].ToString().Trim();
                        Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                        Withdraw_Date = row["ENRL_WDRAW_DATE"].ToString().Trim();
                        break;
                    case "HSSB2108":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        //Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                        // Seq = row["ENRL_SEQ"].ToString().Trim();
                        // ID = row["ENRL_ID"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        // Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        MST_MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                        Snp_Age = row["Age"].ToString();
                        break;
                    case "HSS22001":
                        Agy = row["MST_AGENCY"].ToString().Trim();
                        Dept = row["MST_DEPT"].ToString().Trim();
                        Prog = row["MST_PROGRAM"].ToString().Trim();
                        Year = row["MST_YEAR"].ToString().Trim();
                        App = row["MST_APP_NO"].ToString().Trim();
                        FundHie = row["FUND"].ToString().Trim();
                        //Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                        // Seq = row["ENRL_SEQ"].ToString().Trim();
                        // ID = row["ENRL_ID"].ToString().Trim();

                        ClassPrefer = row["CHLDMST_CLASS_PREFER"].ToString().Trim();
                        Site = row["MST_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        //AMPM = row["ENRL_AMPM"].ToString().Trim();
                        //Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        MST_Language = row["MST_Language"].ToString().Trim();
                        MST_DateAdd1 = row["MST_Date_ADD_1"].ToString().Trim();
                        MST_Position1 = row["MST_Position1"].ToString().Trim();
                        MST_Position2 = row["MST_Position2"].ToString().Trim();
                        MST_Position3 = row["MST_Position3"].ToString().Trim();
                        Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                        MST_INTAKE_DT = row["MST_INTAKE_DATE"].ToString();
                        Yearsonly = row["Years"].ToString();
                        Snp_AgeMonth = row["Months"].ToString();
                        AllMonths = row["AllMonths"].ToString();
                        Mst_PositionCount = string.Empty;
                        Mst_PositionValue = string.Empty;
                        Status = row["App_STATUS"].ToString().Trim();
                        break;
                    case "HSS2001ENRL":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        break;
                    case "HSSB00150":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        Status_Date = row["ENRL_DATE"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Tranfr_Site = row["ENRL_TRANSFER_SITE"].ToString().Trim();
                        Tranfr_Room = row["ENRL_TRANSFER_ROOM"].ToString().Trim();
                        Tranfr_AMPM = row["ENRL_TRANSFER_AMPM"].ToString().Trim();
                        Add_Oper = row["ENRL_ADD_OPERATOR"].ToString().Trim();
                        Add_Date = row["ENRL_DATE_ADD"].ToString().Trim();
                        Lstc_Oper = row["ENRL_LSTC_OPERATOR"].ToString().Trim();
                        break;
                    case "HSSB2111EXCEL":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                        Seq = row["ENRL_SEQ"].ToString().Trim();
                        ID = row["ENRL_ID"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        MST_MealElig = row["MST_CAT_ELIG"].ToString().Trim();
                        Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                        Withdraw_Date = row["ENRL_WDRAW_DATE"].ToString().Trim();
                        Status_Date = row["ENRL_ENRLD_DATE"].ToString().Trim();

                        break;
                    case "":
                        Agy = row["ENRL_AGENCY"].ToString().Trim();
                        Dept = row["ENRL_DEPT"].ToString().Trim();
                        Prog = row["ENRL_PROG"].ToString().Trim();
                        Year = row["ENRL_YEAR"].ToString().Trim();
                        App = row["ENRL_APP_NO"].ToString().Trim();
                        Group = row["ENRL_GROUP"].ToString().Trim();
                        FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                        //Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                        // Seq = row["ENRL_SEQ"].ToString().Trim();
                        // ID = row["ENRL_ID"].ToString().Trim();
                        Site = row["ENRL_SITE"].ToString().Trim();
                        Room = row["ENRL_ROOM"].ToString().Trim();
                        AMPM = row["ENRL_AMPM"].ToString().Trim();
                        Status = row["ENRL_STATUS"].ToString().Trim();
                        Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                        Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                        Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                        MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                        //MST_MealElig = row["MST_MEAL_ELIG"].ToString().Trim();
                        Snp_Age = row["Age"].ToString();
                        break;
                }
                //if (strTable == "HSSB2109")
                //{
                //    MST_Classification = row["MST_CLASSIFICATION"].ToString().Trim();
                //    Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                //    Agy = row["ENRL_AGENCY"].ToString().Trim();
                //    Dept = row["ENRL_DEPT"].ToString().Trim();
                //    Prog = row["ENRL_PROG"].ToString().Trim();
                //    Year = row["ENRL_YEAR"].ToString().Trim();
                //    App = row["ENRL_APP_NO"].ToString().Trim();
                //    Group = row["ENRL_GROUP"].ToString().Trim();
                //    FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                //    Site = row["ENRL_SITE"].ToString().Trim();
                //    Room = row["ENRL_ROOM"].ToString().Trim();
                //    AMPM = row["ENRL_AMPM"].ToString().Trim();
                //    Status = row["ENRL_STATUS"].ToString().Trim();
                //    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                //    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                //    MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                //    Snp_Age = row["Age"].ToString().Trim();
                //    Snp_AgeMonth = row["AgeMonth"].ToString().Trim();
                //    Snp_Gardian = row["GardianName"].ToString().Trim();
                //    if (row["MST_PHONE"].ToString().Trim() != "" && row["MST_PHONE"].ToString().Length>6)
                //        Snp_TelePhone = row["MST_AREA"].ToString().Trim()+"  " + row["MST_PHONE"].ToString().Substring(0, 3) + " - " + row["MST_PHONE"].ToString().Substring(3, 4);
                //    ConsecutiveDays = row["ConsectiveStatus"].ToString().Trim();

                //}
                //else if (strTable == "HSSB2103")
                //{
                //    Agy = row["ENRL_AGENCY"].ToString().Trim();
                //    Dept = row["ENRL_DEPT"].ToString().Trim();
                //    Prog = row["ENRL_PROG"].ToString().Trim();
                //    Year = row["ENRL_YEAR"].ToString().Trim();
                //    App = row["ENRL_APP_NO"].ToString().Trim();
                //    Group = row["ENRL_GROUP"].ToString().Trim();
                //    FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                //    Site = row["ENRL_SITE"].ToString().Trim();
                //    Room = row["ENRL_ROOM"].ToString().Trim();
                //    AMPM = row["ENRL_AMPM"].ToString().Trim();
                //    Status = row["ENRL_STATUS"].ToString().Trim();
                //    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                //    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                //    MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                //    Snp_Age = row["Age"].ToString().Trim();
                //    Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                //    Mst_Zip = row["MST_Zip"].ToString().Trim();
                //    MST_WAIT_LIST = row["MST_WAIT_LIST"].ToString().Trim();
                //    Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                //    AttendedDt = row["Attended"].ToString().Trim();
                //    Enrl_Date = row["ENRL_DATE"].ToString().Trim();

                //}
                //else if (strTable == "HSSB2114")
                //{
                //    Agy = row["ENRL_AGENCY"].ToString().Trim();
                //    Dept = row["ENRL_DEPT"].ToString().Trim();
                //    Prog = row["ENRL_PROG"].ToString().Trim();
                //    Year = row["ENRL_YEAR"].ToString().Trim();
                //    App = row["ENRL_APP_NO"].ToString().Trim();
                //    Group = row["ENRL_GROUP"].ToString().Trim();
                //    FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                //    Site = row["ENRL_SITE"].ToString().Trim();
                //    Room = row["ENRL_ROOM"].ToString().Trim();
                //    AMPM = row["ENRL_AMPM"].ToString().Trim();
                //    Status = row["ENRL_STATUS"].ToString().Trim();
                //    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                //    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                //    MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                //    Snp_Age = row["Age"].ToString().Trim();
                //    Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                //  //  Mst_Zip = row["MST_Zip"].ToString().Trim();
                //   // MST_WAIT_LIST = row["MST_WAIT_LIST"].ToString().Trim();
                //    //Chld_Repeater = row["CHLDMST_CHLD_REPEAT"].ToString().Trim();
                //    //AttendedDt = row["Attended"].ToString().Trim();
                //    Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                //    MSTZIPDATE = row["ZIPDATE"].ToString();
                //    MST_INTAKE_DT = row["MST_INTAKE_DATE"].ToString();
                //    AttendedDt = row["ATTN_DATE"].ToString();

                //}
                //else if (strTable == "HSSB2111SUMMARY")
                //{
                //    Agy = row["ATTN_AGENCY"].ToString().Trim();
                //    Dept = row["ATTN_DEPT"].ToString().Trim();
                //    Prog = row["ATTN_PROGRAM"].ToString().Trim();
                //    Year = row["ATTN_YEAR"].ToString().Trim();                 
                //    Site = row["ATTN_SITE"].ToString().Trim();                                  
                //    Breakfast = row["BREAKFAST"].ToString().Trim();
                //    Lunch = row["LUNCH"].ToString().Trim();
                //    Supper = row["AMSNACKS"].ToString().Trim();
                //    Suppliment = row["PMSNACKS"].ToString().Trim();
                //    Free = row["FREE"].ToString().Trim();
                //    Reduced = row["Reduced"].ToString().Trim();
                //    OverIncome = row["OverIncome"].ToString().Trim();


                //}

                //else if (strTable == "HSSB2111DETAILS")
                //{
                //    Agy = row["ATTN_AGENCY"].ToString().Trim();
                //    Dept = row["ATTN_DEPT"].ToString().Trim();
                //    Prog = row["ATTN_PROGRAM"].ToString().Trim();
                //   // Year = row["ATTN_YEAR"].ToString().Trim();
                //    Site = row["ATTN_SITE"].ToString().Trim();
                //    Room = row["ATTN_ROOM"].ToString().Trim();
                //    AMPM = row["ATTN_AMPM"].ToString().Trim();
                //    FundHie = row["ATTN_FUNDING_SOURCE"].ToString().Trim();   
                //    EndEnrollment = row["ENDENrollment"].ToString().Trim();
                //    FundEnrollment = row["FUNDENROLLMENT"].ToString().Trim();
                //    ClassDays = row["ClassDays"].ToString().Trim();
                //    AvailbleDays = row["AvailableDays"].ToString().Trim();
                //    ExcusedDays = row["ExecusedDays"].ToString().Trim();
                //    AttandanceDays = row["AttandanceDays"].ToString().Trim();



                //}

                //else if (strTable == "PIR20000")
                //{
                //    Agy = row["ENRL_AGENCY"].ToString().Trim();
                //    Dept = row["ENRL_DEPT"].ToString().Trim();
                //    Prog = row["ENRL_PROG"].ToString().Trim();
                //    Year = row["ENRL_YEAR"].ToString().Trim();
                //    App = row["ENRL_APP_NO"].ToString().Trim();                 
                //    FundHie = row["ENRL_FUND_HIE"].ToString().Trim();                  
                //    Site = row["ENRL_SITE"].ToString().Trim();
                //    Room = row["ENRL_ROOM"].ToString().Trim();
                //    AMPM = row["ENRL_AMPM"].ToString().Trim();
                //    Status = row["ENRL_STATUS"].ToString().Trim();
                //    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                //    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                //    Enrl_Stat_Attn_FDate = row["FDate"].ToString().Trim();
                //    Enrl_Stat_Attn_LDate = row["LDate"].ToString().Trim();
                //    Enrl_Date = row["ENRL_DATE"].ToString().Trim();
                //    Withdraw_Date = row["ENRL_WDRAW_DATE"].ToString().Trim();

                //}
                //else
                //{
                //    Agy = row["ENRL_AGENCY"].ToString().Trim();
                //    Dept = row["ENRL_DEPT"].ToString().Trim();
                //    Prog = row["ENRL_PROG"].ToString().Trim();
                //    Year = row["ENRL_YEAR"].ToString().Trim();
                //    App = row["ENRL_APP_NO"].ToString().Trim();
                //    Group = row["ENRL_GROUP"].ToString().Trim();
                //    FundHie = row["ENRL_FUND_HIE"].ToString().Trim();
                //    //Rec_Type = row["ENRL_RECORD_TYPE"].ToString().Trim();
                //    // Seq = row["ENRL_SEQ"].ToString().Trim();
                //    // ID = row["ENRL_ID"].ToString().Trim();
                //    Site = row["ENRL_SITE"].ToString().Trim();
                //    Room = row["ENRL_ROOM"].ToString().Trim();
                //    AMPM = row["ENRL_AMPM"].ToString().Trim();
                //    Status = row["ENRL_STATUS"].ToString().Trim();
                //    Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                //    Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                //    Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                //    MST_ActiveStatus = row["MST_Active_Status"].ToString().Trim();
                //    Snp_Age = row["Age"].ToString();

                //}
            }



        }

        #endregion

        #region Properties

        public string Join_Mst_Snp { get; set; }
        public string Row_Type { get; set; }
        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string Group { get; set; }
        public string FundHie { get; set; }
        public string Seq { get; set; }
        public string ID { get; set; }
        public string Rec_Type { get; set; }
        public string Status { get; set; }
        public string Status_Reason { get; set; }
        public string Status_Date { get; set; }
        public string Site { get; set; }
        public string Room { get; set; }
        public string AMPM { get; set; }
        public string Enrl_Date { get; set; }
        public string Withdraw_Code { get; set; }
        public string Withdraw_Date { get; set; }
        public string Wiait_Date { get; set; }
        public string Denied_Code { get; set; }
        public string Denied_Date { get; set; }
        public string Pending_Code { get; set; }
        public string Pending_Date { get; set; }
        public string Rank { get; set; }
        public string Rank_Chg_Code { get; set; }
        public string Transc_Type { get; set; }
        public string Tranfr_Site { get; set; }
        public string Tranfr_Room { get; set; }
        public string Tranfr_AMPM { get; set; }
        public string Parent_Rate { get; set; }
        public string Funding_Code { get; set; }
        public string Funding_Rate { get; set; }
        public string Desc_1 { get; set; }
        public string Desc_2 { get; set; }
        public string Fund_End_date { get; set; }
        public string Rate_EFR_date { get; set; }
        public string Add_Date { get; set; }
        public string Add_Oper { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Oper { get; set; }
        public string Enrl_Denied { get; set; }
        public string Enrl_Hist_Cnt { get; set; }
        public string Enrl_Max_Attn_Date { get; set; }
        public string Enrl_Min_Attn_Date { get; set; }
        public string Enrl_Stat_Attn_FDate { get; set; }
        public string Enrl_Stat_Attn_LDate { get; set; }
        public string Enrl_Pref_Class { get; set; }
        public string Enrl_Status_Not_Equalto { get; set; }
        public string AllMonths { get; set; }
        public string Transfer_ID { get; set; }
        public string Mult_Fund_Site_List { get; set; }

        public string Snp_F_Name { get; set; }
        public string Snp_L_Name { get; set; }
        public string Snp_M_Name { get; set; }
        public string Snp_DOB { get; set; }
        public string Snp_Age { get; set; }
        public string Snp_Sex { get; set; }
        public string Snp_Ethnic { get; set; }
        public string Snp_Race { get; set; }
        public string Snp_Gardian { get; set; }
        public string Snp_AgeMonth { get; set; }
        public string Snp_TelePhone { get; set; }
        public string Cond_Cnt { get; set; }
        public string KG_Age { get; set; }

        public string Mst_App { get; set; }
        public string Mst_Hno { get; set; }
        public string Mst_Street { get; set; }
        public string Mst_City { get; set; }
        public string Mst_State { get; set; }
        public string Mst_Zip { get; set; }
        public string Mst_Zip_Plus { get; set; }
        public string Mst_Phone { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_Rank1 { get; set; }
        public string Mst_Rank2 { get; set; }
        public string Mst_Rank3 { get; set; }
        public string Mst_Rank4 { get; set; }
        public string Mst_Rank5 { get; set; }
        public string Mst_Rank6 { get; set; }
        public string MST_ActiveStatus { get; set; }
        public string MST_Classification { get; set; }
        public string MST_WAIT_LIST { get; set; }
        public string Chld_Repeater { get; set; }
        public string MSTZIPDATE { get; set; }
        public string MST_Language { get; set; }
        public string MST_INTAKE_DT { get; set; }
        public string MST_MealElig { get; set; }
        public string Asof_Status { get; set; }
        public string Asof_Seq { get; set; }
        public string Asof_From_Date { get; set; }
        public string Asof_TO_Date { get; set; }
        public string Asof_Site { get; set; }
        public string Asof_Room { get; set; }
        public string Asof_AMPM { get; set; }
        public string Asof_Date_Add { get; set; }
        public string Asof_Add_Opr { get; set; }
        public string Asof_Date { get; set; }
        public string History_From { get; set; }
        public string ConsecutiveDays { get; set; }
        public string AttendedDt { get; set; }
        public string Yearsonly { get; set; }
        public string MST_DateAdd1 { get; set; }
        public string MST_Position1 { get; set; }
        public string MST_Position2 { get; set; }
        public string MST_Position3 { get; set; }
        public string ClassPrefer { get; set; }
        public string Mst_PositionCount { get; set; }
        public string Mst_PositionValue { get; set; }
        public string Free { get; set; }
        public string Reduced { get; set; }
        public string OverIncome { get; set; }
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Supper { get; set; }
        public string Suppliment { get; set; }
        public string FundEnrollment { get; set; }
        public string EndEnrollment { get; set; }
        public string ClassDays { get; set; }
        public string AvailbleDays { get; set; }
        public string ExcusedDays { get; set; }
        public string AttandanceDays { get; set; }
        public string BusRoot { get; set; }
        public string MST_POVERTY { get; set; }
        public string MST_FAMILY_ID { get; set; }
        public string MST_FAMILY_TYPE { get; set; }
        public string MST_INCOME_TYPE { get; set; }
        public string MST_INTAKE_WORKER { get; set; }



        #endregion

    }

    public class ENRL_Asof_Entity
    {
        #region Constructors

        public ENRL_Asof_Entity()
        {
            ID = App = FundHie = Status = string.Empty;
            Site = Room = AMPM = Edate = Wdate = string.Empty;
            Enrolled = Withdrawn = Window30 = GetAttn1 = FirstTime = FundSlots = string.Empty;
        }

        public ENRL_Asof_Entity(bool Initialize)
        {
            if (Initialize)
            {
                ID = App = FundHie = Status = null;
                Site = Room = AMPM = Edate = Wdate = null;
                Enrolled = Withdrawn = Window30 = GetAttn1 = FirstTime = FundSlots = null;
            }

        }

        public ENRL_Asof_Entity(ENRL_Asof_Entity Entity)
        {
            ID = Entity.ID;
            App = Entity.App;
            FundHie = Entity.FundHie;
            Status = Entity.Status;
            Site = Entity.Site;
            Room = Entity.Room;
            AMPM = Entity.AMPM;
            Edate = Entity.Edate;
            Wdate = Entity.Wdate;
            Enrolled = Entity.Enrolled;
            Withdrawn = Entity.Withdrawn;
            Window30 = Entity.Window30;
            GetAttn1 = Entity.GetAttn1;
            FirstTime = Entity.FirstTime;
            FundSlots = Entity.FundSlots;
        }

        public ENRL_Asof_Entity(DataRow row)
        {
            if (row != null)
            {
                ID = row["ID"].ToString().Trim();
                App = row["App"].ToString().Trim();
                FundHie = row["Fund_Hie"].ToString().Trim();
                Status = row["App_Status"].ToString().Trim();
                Edate = row["Status_Date"].ToString().Trim();
            }
        }


        public ENRL_Asof_Entity(DataRow row, string Report)
        {
            if (row != null)
            {
                ID = row["ID"].ToString().Trim();
                App = row["App"].ToString().Trim();
                FundHie = row["Fund_Hie"].ToString().Trim();
                Status = row["App_Status"].ToString().Trim();
                Site = row["Site_Num"].ToString().Trim();
                Room = row["Room"].ToString().Trim();
                AMPM = row["AMPM"].ToString().Trim();
                Edate = row["ENRL_DATE"].ToString().Trim();
                Wdate = row["ENRL_WDATE"].ToString().Trim();
                Enrolled = row["Enrolled"].ToString().Trim();
                GetAttn1 = row["Attn1Day"].ToString().Trim();
                FirstTime = row["FirstTime"].ToString().Trim();
                Window30 = row["Window30"].ToString().Trim();
                Withdrawn = row["Withdrawn"].ToString().Trim();
                //FundSlots = row["FundSlots"].ToString().Trim();

                TotEnrlDays = row["TOTENRL_Days"].ToString().Trim();
                TotAttnDays = row["TOTATTN_Days"].ToString().Trim();

            }
        }

        public ENRL_Asof_Entity(DataRow row, string Report, string Datatable)
        {
            if (row != null && Datatable == string.Empty)
            {
                FundHie = row["Fund_Hie"].ToString().Trim();
                Site = row["Site_Num"].ToString().Trim();
                Room = row["Room"].ToString().Trim();
                AMPM = row["AMPM"].ToString().Trim();
                Enrolled = row["Enrolled"].ToString().Trim();
                GetAttn1 = row["Attn1Day"].ToString().Trim();
                FirstTime = row["FirstTime"].ToString().Trim();
                Window30 = row["Window30"].ToString().Trim();
                Withdrawn = row["Withdrawn"].ToString().Trim();
                FundSlots = row["FundSlots"].ToString().Trim();
            }

            if (row != null && Datatable == "Site")
            {
                //FundHie = row["Fund_Hie"].ToString().Trim();
                Site = row["Site_Num"].ToString().Trim();
                //Room = row["Room"].ToString().Trim();
                //AMPM = row["AMPM"].ToString().Trim();
                Enrolled = row["Enrolled"].ToString().Trim();
                GetAttn1 = row["Attn1Day"].ToString().Trim();
                FirstTime = row["FirstTime"].ToString().Trim();
                Window30 = row["Window30"].ToString().Trim();
                Withdrawn = row["Withdrawn"].ToString().Trim();
            }

            if (row != null && Datatable == "Room")
            {
                //FundHie = row["Fund_Hie"].ToString().Trim();
                Site = row["Site_Num"].ToString().Trim();
                Room = row["Room"].ToString().Trim();
                AMPM = row["AMPM"].ToString().Trim();
                Enrolled = row["Enrolled"].ToString().Trim();
                GetAttn1 = row["Attn1Day"].ToString().Trim();
                FirstTime = row["FirstTime"].ToString().Trim();
                Window30 = row["Window30"].ToString().Trim();
                Withdrawn = row["Withdrawn"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string ID { get; set; }
        public string App { get; set; }
        public string FundHie { get; set; }
        public string Status { get; set; }
        public string Site { get; set; }
        public string Room { get; set; }
        public string AMPM { get; set; }
        public string Edate { get; set; }
        public string Wdate { get; set; }
        public string Enrolled { get; set; }
        public string GetAttn1 { get; set; }
        public string FirstTime { get; set; }
        public string Window30 { get; set; }
        public string Withdrawn { get; set; }
        public string FundSlots { get; set; }
        public string TotEnrlDays { get; set; }
        public string TotAttnDays { get; set; }


        #endregion

    }


    public class ENRLHIST_Entity
    {
        #region Constructors

        public ENRLHIST_Entity()
        {
            Row_Type = string.Empty;
            ID = string.Empty;
            Status = string.Empty;
            Seq = string.Empty;
            From_Date = string.Empty;
            TO_Date = string.Empty;
            Fund_Hie = string.Empty;
            Site = string.Empty;
            Room = string.Empty;
            AMPM = string.Empty;
            Date_Add = string.Empty;
            Add_Opr = string.Empty;
            Lstc_Dtae = string.Empty;
            Lstc_Opr = string.Empty;
            Asof_Date = string.Empty;
            Attn_First_Date =
            Attn_LAST_Date = string.Empty;
            Reason = string.Empty;


            Enrl_Agy = Enrl_Dept = Enrl_Prog = Enrl_Year =
            Enrl_App = Module_Type = string.Empty;
        }

        public ENRLHIST_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Row_Type = null;
                ID = null;
                Status = null;
                Seq = null;
                From_Date = null;
                TO_Date = null;
                Fund_Hie = null;
                Site = null;
                Room = null;
                AMPM = null;
                Date_Add = null;
                Add_Opr = null;
                Lstc_Dtae = null;
                Lstc_Opr = null;
                Asof_Date = null;
                Attn_First_Date =
                Attn_LAST_Date = null;
                Reason = null;

                Enrl_Agy = Enrl_Dept = Enrl_Prog = Enrl_Year =
                Enrl_App = Module_Type = null;
            }
        }

        public ENRLHIST_Entity(ENRLHIST_Entity Entity)
        {
            Row_Type = Entity.Row_Type;
            ID = Entity.ID;
            Status = Entity.Status;
            Seq = Entity.Seq;
            From_Date = Entity.From_Date;
            TO_Date = Entity.TO_Date;
            Fund_Hie = Entity.Fund_Hie;
            Site = Entity.Site;
            Room = Entity.Room;
            AMPM = Entity.AMPM;
            Date_Add = Entity.Date_Add;
            Add_Opr = Entity.Add_Opr;
            Lstc_Dtae = Entity.Lstc_Dtae;
            Lstc_Opr = Entity.Lstc_Opr;
            Asof_Date = Entity.Asof_Date;
            Attn_First_Date = Entity.Attn_First_Date;
            Attn_LAST_Date = Entity.Attn_LAST_Date;
            Reason = Entity.Reason;

            Enrl_Agy = Entity.Enrl_Agy;
            Enrl_Dept = Entity.Enrl_Dept;
            Enrl_Prog = Entity.Enrl_Prog;
            Enrl_Year = Entity.Enrl_Year;
            Enrl_App = Entity.Enrl_App;
            Module_Type = Entity.Module_Type;
        }

        public ENRLHIST_Entity(DataRow row, string Asof_Date_SW)
        {
            if (row != null)
            {
                Row_Type = "U";
                ID = row["ENRLHIST_ID"].ToString().Trim();
                Status = row["ENRLHIST_STATUS"].ToString().Trim();
                Seq = row["ENRLHIST_SEQ"].ToString().Trim();
                From_Date = row["ENRLHIST_FDATE"].ToString().Trim();
                TO_Date = row["ENRLHIST_TDATE"].ToString().Trim();
                Fund_Hie = row["ENRL_FUND_HIE"].ToString().Trim();
                Site = row["ENRLHIST_SITE"].ToString().Trim();
                Room = row["ENRLHIST_ROOM"].ToString().Trim();
                AMPM = row["ENRLHIST_AMPM"].ToString().Trim();
                Date_Add = row["ENRLHIST_DATE_ADD"].ToString().Trim();
                Add_Opr = row["ENRLHIST_ADD_OPERATOR"].ToString().Trim();
                Lstc_Dtae = row["ENRLHIST_DATE_LSTC"].ToString().Trim();
                Lstc_Opr = row["ENRLHIST_LSTC_OPERATOR"].ToString().Trim();
                Attn_First_Date = row["Attn_FDate"].ToString().Trim();
                Attn_LAST_Date = row["Attn_LDate"].ToString().Trim();
                Reason = row["ENRLHIST_REASON"].ToString().Trim();

                Asof_Date = Module_Type = string.Empty;

                if (Asof_Date_SW == "Y")
                {
                    Enrl_Agy = row["ENRL_AGENCY"].ToString().Trim();
                    Enrl_Dept = row["ENRL_DEPT"].ToString().Trim();
                    Enrl_Prog = row["ENRL_PROG"].ToString().Trim();
                    Enrl_Year = row["ENRL_YEAR"].ToString().Trim();
                    Enrl_App = row["ENRL_APP_NO"].ToString().Trim();
                }
            }
        }


        #endregion

        #region Properties

        public string Row_Type { get; set; }
        public string ID { get; set; }
        public string Status { get; set; }
        public string Seq { get; set; }
        public string From_Date { get; set; }
        public string TO_Date { get; set; }
        public string Fund_Hie { get; set; }
        public string Site { get; set; }
        public string Room { get; set; }
        public string AMPM { get; set; }
        public string Date_Add { get; set; }
        public string Add_Opr { get; set; }
        public string Lstc_Dtae { get; set; }
        public string Lstc_Opr { get; set; }
        public string Asof_Date { get; set; }
        public string Attn_First_Date { get; set; }
        public string Attn_LAST_Date { get; set; }
        public string Reason { get; set; }

        public string Enrl_Agy { get; set; }
        public string Enrl_Dept { get; set; }
        public string Enrl_Prog { get; set; }
        public string Enrl_Year { get; set; }
        public string Enrl_App { get; set; }
        public string Module_Type { get; set; }

        #endregion

    }


    public class Sum_Referral_Entity
    {
        #region Constructors

        public Sum_Referral_Entity()
        {
            Agy =
            Dept =
            Prog =
            Year =
            App =
            Ssn =
            Ref_Hie =
            //Ref_Agy =
            //Ref_Dept =
            //Ref_Prog = string.Empty;
            Referred_By =
            Referred_Date =
            Referred_Status =
            Expected_App_No =
            Referred_Prog_Name =
            ID = string.Empty;

            Snp_F_Name = Snp_L_Name = Snp_M_Name = Snp_DOB = Mst_Hno = Mst_Street = Mst_City =
            Mst_State = Mst_Zip = Mst_Zip_Plus = Snp_Sex = Mst_Phone = Mst_Rank1 = Mst_Fam_Seq = string.Empty;
        }

        public Sum_Referral_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Agy =
                Dept =
                Prog =
                Year =
                App =
                Ssn =
                Ref_Hie =
                //Ref_Agy =
                //Ref_Dept =
                //Ref_Prog = null;
                Referred_By =
                Referred_Date =
                Referred_Status =
                Expected_App_No =
                Referred_Prog_Name =
                ID = null;

                Snp_F_Name = Snp_L_Name = Snp_M_Name = Snp_DOB = Mst_Hno = Mst_Street = Mst_City =
                Mst_State = Mst_Zip = Mst_Zip_Plus = Snp_Sex = Mst_Phone = Mst_Rank1 = Mst_Fam_Seq = null;

            }
        }

        public Sum_Referral_Entity(Sum_Referral_Entity Entity, string Exp_App_No, string Ref_Prog_Name)
        {
            Agy = Entity.Agy;
            Dept = Entity.Dept;
            Prog = Entity.Prog;
            Year = Entity.Year;
            App = Entity.App;
            Ref_Hie = Entity.Ref_Hie;
            //Ref_Agy = Entity.Ref_Agy;
            //Ref_Dept = Entity.Ref_Dept;
            //Ref_Prog = Entity.Ref_Prog;
            Referred_By = Entity.Referred_By;
            Referred_Date = Entity.Referred_Date;
            Referred_Status = Entity.Referred_Status;

            Ssn = Entity.Ssn;
            Snp_F_Name = Entity.Snp_F_Name;
            Snp_L_Name = Entity.Snp_L_Name;
            Snp_M_Name = Entity.Snp_M_Name;
            Snp_DOB = Entity.Snp_DOB;
            Snp_Sex = Entity.Snp_Sex;
            Snp_Age = Entity.Snp_Age;
            Mst_Hno = Entity.Mst_Hno;
            Mst_Street = Entity.Mst_Street;
            Mst_City = Entity.Mst_City;
            Mst_State = Entity.Mst_State;
            Mst_Zip = Entity.Mst_Zip;
            Mst_Zip_Plus = Entity.Mst_Zip_Plus;
            Mst_Phone = Entity.Mst_Phone;
            Mst_Site = Entity.Mst_Site;
            Mst_Rank1 = Entity.Mst_Rank1;
            Mst_Fam_Seq = Entity.Mst_Fam_Seq;

            Expected_App_No = "No App";
            Referred_Prog_Name = Ref_Prog_Name;
            if (!string.IsNullOrEmpty(Exp_App_No.Trim()))
                Expected_App_No = Exp_App_No;
            ID = Entity.ID;
        }

        public Sum_Referral_Entity(DataRow row)
        {
            if (row != null)
            {
                Agy = row["CASESUM_AGENCY"].ToString().Trim();
                Dept = row["CASESUM_DEPT"].ToString().Trim();
                Prog = row["CASESUM_PROGRAM"].ToString().Trim();
                Year = row["CASESUM_YEAR"].ToString();
                App = row["CASESUM_APP_NO"].ToString().Trim();

                Ref_Hie = row["CASESUM_REF_HIERARCHY"].ToString().Trim();
                //Ref_Agy = row["CASESUM_REF_AGENCY"].ToString().Trim();
                //Ref_Dept = row["CASESUM_REF_DEPT"].ToString().Trim();
                //Ref_Prog = row["CASESUM_REF_PROGRAM"].ToString().Trim();

                Referred_By = row["CASESUM_REFER_BY"].ToString().Trim();
                Referred_Date = row["CASESUM_REFERDATE"].ToString().Trim();
                Referred_Status = row["CASESUM_STATUS_CODE"].ToString().Trim();

                Ssn = row["MST_SSN"].ToString().Trim();

                Snp_F_Name = row["SNP_NAME_IX_FI"].ToString().Trim();
                Snp_L_Name = row["SNP_NAME_IX_LAST"].ToString().Trim();
                Snp_M_Name = row["SNP_NAME_IX_MI"].ToString().Trim();
                Snp_Sex = row["SNP_SEX"].ToString().Trim();
                Snp_Age = row["SNP_AGE"].ToString().Trim();
                Snp_DOB = row["SNP_ALT_BDATE"].ToString().Trim();
                Mst_Hno = row["MST_HN"].ToString().Trim();
                Mst_Street = row["MST_STREET"].ToString().Trim();
                Mst_City = row["MST_CITY"].ToString().Trim();
                Mst_State = row["MST_STATE"].ToString().Trim();
                Mst_Zip = row["MST_ZIP"].ToString().Trim();
                Mst_Zip_Plus = row["MST_ZIPPLUS"].ToString().Trim();
                Mst_Phone = row["MST_AREA"].ToString().Trim() + row["MST_PHONE"].ToString().Trim();
                Mst_Site = row["MST_SITE"].ToString().Trim();
                Mst_Rank1 = row["MST_RANK1"].ToString().Trim();
                Mst_Fam_Seq = row["MST_FAMILY_SEQ"].ToString().Trim();
                Expected_App_No = Referred_Prog_Name = ID = string.Empty;
            }
        }


        #endregion

        #region Properties

        public string Agy { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string Ref_Hie { get; set; }
        //public string Ref_Agy { get; set; }
        //public string Ref_Dept { get; set; }
        //public string Ref_Prog { get; set; }
        public string Referred_By { get; set; }
        public string Referred_Status { get; set; }
        public string Referred_Date { get; set; }
        public string Ssn { get; set; }
        public string Snp_F_Name { get; set; }
        public string Snp_L_Name { get; set; }
        public string Snp_M_Name { get; set; }
        public string Snp_DOB { get; set; }
        public string Snp_Age { get; set; }
        public string Snp_Sex { get; set; }
        public string Mst_Hno { get; set; }
        public string Mst_Street { get; set; }
        public string Mst_City { get; set; }
        public string Mst_State { get; set; }
        public string Mst_Zip { get; set; }
        public string Mst_Zip_Plus { get; set; }
        public string Mst_Phone { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_Rank1 { get; set; }
        public string Mst_Fam_Seq { get; set; }
        public string Expected_App_No { get; set; }
        public string Referred_Prog_Name { get; set; }
        public string ID { get; set; }

        #endregion

    }


    public class PirWorkEntity
    {
        #region Constructors

        public PirWorkEntity()
        {
            PIRWORK_AGENCY = string.Empty;
            PIRWORK_DEPT = string.Empty;
            PIRWORK_PROG = string.Empty;
            PIRWORK_YEAR = string.Empty;
            PIRWORK_APP_NO = string.Empty;
            PIRWORK_FUND = string.Empty;
            PIRWORK_FATTN = string.Empty;
            PIRWORK_LATTN = string.Empty;
            PIRWORK_ATTN_COUNT = string.Empty;
            PIRWORK_ENROLL_DATE = string.Empty;
            PIRWORK_WDRAW_DATE = string.Empty;
            PIRWORK_APP_ACTIVE = string.Empty;
            PIRWORK_APP_FNAME = string.Empty;
            PIRWORK_APP_LNAME = string.Empty;
            PIRWORK_APP_MNAME = string.Empty;
            PIRWORK_APP_SSN = string.Empty;
            PIRWORK_FAMILY_ID = string.Empty;
            PIRWORK_SITE = string.Empty;
            PIRWORK_SITE_ROOM = string.Empty;
            PIRWORK_SITE_AMPM = string.Empty;
            PIRWORK_INCOME_TYPES = string.Empty;
            PIRWORK_POVERTY = string.Empty;
            PIRWORK_FAMILY_TYPE = string.Empty;
            PIRWORK_CHLD_REPEAT = string.Empty;
            PIRWORK_ETHNICITY = string.Empty;
            PIRWORK_RACE = string.Empty;
            PIRWORK_LANGUAGE = string.Empty;
            PIRWORK_SNP_AGE_YY = string.Empty;
            PIRWORK_SNP_AGE_MM = string.Empty;
            PIRWORK_SNP_AGE_WW = string.Empty;
            PIRWORK_SNP_AGE_DD = string.Empty;
            PIRWORK_SNP_AGE_MONS = string.Empty;
            PIRWORK_SNP_AGE_DAYS = string.Empty;
            PIRWORK_FA_AGE_YY = string.Empty;
            PIRWORK_FA_AGE_MM = string.Empty;
            PIRWORK_FA_AGE_WW = string.Empty;
            PIRWORK_FA_AGE_DD = string.Empty;
            PIRWORK_FA_AGE_MONS = string.Empty;
            PIRWORK_FA_AGE_DAYS = string.Empty;
            PIRWORK_LA_AGE_YY = string.Empty;
            PIRWORK_LA_AGE_MM = string.Empty;
            PIRWORK_LA_AGE_WW = string.Empty;
            PIRWORK_LA_AGE_DD = string.Empty;
            PIRWORK_LA_AGE_MONS = string.Empty;
            PIRWORK_LA_AGE_DAYS = string.Empty;
            PIRWORK_KG_AGE_YY = string.Empty;
            PIRWORK_KG_AGE_MM = string.Empty;
            PIRWORK_KG_AGE_WW = string.Empty;
            PIRWORK_KG_AGE_DD = string.Empty;
            PIRWORK_KG_AGE_MONS = string.Empty;
            PIRWORK_KG_AGE_DAYS = string.Empty;
            PIRWORK_CHLD_REPEAT_HS = string.Empty;
            PIRWORK_CHLD_REPEAT_EHS = string.Empty;
            PIRWORK_PREGNANT_MOTHER = string.Empty;
            PIRWORK_WD_AGE_YY = string.Empty;
            PIRWORK_WD_AGE_MM = string.Empty;
            PIRWORK_WD_AGE_WW = string.Empty;
            PIRWORK_WD_AGE_DD = string.Empty;
            PIRWORK_WD_AGE_MONS = string.Empty;
            PIRWORK_WD_AGE_DAYS = string.Empty;

            Present_Days= string.Empty;
            Absent_Days= string.Empty;
            Enroll_Days= string.Empty;
        }

        public PirWorkEntity(DataRow row, bool booldesc, string strMode)
        {
            if (row != null)
            {
                PIRWORK_AGENCY = row["PIRWORK_AGENCY"].ToString().Trim();
                PIRWORK_DEPT = row["PIRWORK_DEPT"].ToString().Trim();
                PIRWORK_PROG = row["PIRWORK_PROG"].ToString().Trim();
                PIRWORK_YEAR = row["PIRWORK_YEAR"].ToString().Trim();
                PIRWORK_APP_NO = row["PIRWORK_APP_NO"].ToString().Trim();
                PIRWORK_FUND = row["PIRWORK_FUND"].ToString().Trim();
                PIRWORK_FATTN = row["PIRWORK_FATTN"].ToString().Trim();
                PIRWORK_LATTN = row["PIRWORK_LATTN"].ToString().Trim();
                PIRWORK_ATTN_COUNT = row["PIRWORK_ATTN_COUNT"].ToString().Trim();
                PIRWORK_ENROLL_DATE = row["PIRWORK_ENROLL_DATE"].ToString().Trim();
                PIRWORK_WDRAW_DATE = row["PIRWORK_WDRAW_DATE"].ToString().Trim();
                PIRWORK_APP_ACTIVE = row["PIRWORK_APP_ACTIVE"].ToString().Trim();
                PIRWORK_APP_FNAME = row["PIRWORK_APP_FNAME"].ToString().Trim();
                PIRWORK_APP_LNAME = row["PIRWORK_APP_LNAME"].ToString().Trim();
                PIRWORK_APP_MNAME = row["PIRWORK_APP_MNAME"].ToString().Trim();
                PIRWORK_APP_SSN = row["PIRWORK_APP_SSN"].ToString().Trim();
                PIRWORK_FAMILY_ID = row["PIRWORK_FAMILY_ID"].ToString().Trim();
                PIRWORK_SITE = row["PIRWORK_SITE"].ToString().Trim();
                PIRWORK_SITE_ROOM = row["PIRWORK_SITE_ROOM"].ToString().Trim();
                PIRWORK_SITE_AMPM = row["PIRWORK_SITE_AMPM"].ToString().Trim();
                PIRWORK_INCOME_TYPES = row["PIRWORK_INCOME_TYPES"].ToString().Trim();
                PIRWORK_POVERTY = row["PIRWORK_POVERTY"].ToString().Trim();
                PIRWORK_FAMILY_TYPE = row["PIRWORK_FAMILY_TYPE"].ToString().Trim();
                PIRWORK_CHLD_REPEAT = row["PIRWORK_CHLD_REPEAT"].ToString().Trim();
                PIRWORK_ETHNICITY = row["PIRWORK_ETHNICITY"].ToString().Trim();
                PIRWORK_RACE = row["PIRWORK_RACE"].ToString().Trim();
                PIRWORK_LANGUAGE = row["PIRWORK_LANGUAGE"].ToString().Trim();
                PIRWORK_SNP_AGE_YY = row["PIRWORK_SNP_AGE_YY"].ToString().Trim();
                PIRWORK_SNP_AGE_MM = row["PIRWORK_SNP_AGE_MM"].ToString().Trim();
                PIRWORK_SNP_AGE_WW = row["PIRWORK_SNP_AGE_WW"].ToString().Trim();
                PIRWORK_SNP_AGE_DD = row["PIRWORK_SNP_AGE_DD"].ToString().Trim();
                PIRWORK_SNP_AGE_MONS = row["PIRWORK_SNP_AGE_MONS"].ToString().Trim();
                PIRWORK_SNP_AGE_DAYS = row["PIRWORK_SNP_AGE_DAYS"].ToString().Trim();
                PIRWORK_FA_AGE_YY = row["PIRWORK_FA_AGE_YY"].ToString().Trim();
                PIRWORK_FA_AGE_MM = row["PIRWORK_FA_AGE_MM"].ToString().Trim();
                PIRWORK_FA_AGE_WW = row["PIRWORK_FA_AGE_WW"].ToString().Trim();
                PIRWORK_FA_AGE_DD = row["PIRWORK_FA_AGE_DD"].ToString().Trim();
                PIRWORK_FA_AGE_MONS = row["PIRWORK_FA_AGE_MONS"].ToString().Trim();
                PIRWORK_FA_AGE_DAYS = row["PIRWORK_FA_AGE_DAYS"].ToString().Trim();
                PIRWORK_LA_AGE_YY = row["PIRWORK_LA_AGE_YY"].ToString().Trim();
                PIRWORK_LA_AGE_MM = row["PIRWORK_LA_AGE_MM"].ToString().Trim();
                PIRWORK_LA_AGE_WW = row["PIRWORK_LA_AGE_WW"].ToString().Trim();
                PIRWORK_LA_AGE_DD = row["PIRWORK_LA_AGE_DD"].ToString().Trim();
                PIRWORK_LA_AGE_MONS = row["PIRWORK_LA_AGE_MONS"].ToString().Trim();
                PIRWORK_LA_AGE_DAYS = row["PIRWORK_LA_AGE_DAYS"].ToString().Trim();
                PIRWORK_KG_AGE_YY = row["PIRWORK_KG_AGE_YY"].ToString().Trim();
                PIRWORK_KG_AGE_MM = row["PIRWORK_KG_AGE_MM"].ToString().Trim();
                PIRWORK_KG_AGE_WW = row["PIRWORK_KG_AGE_WW"].ToString().Trim();
                PIRWORK_KG_AGE_DD = row["PIRWORK_KG_AGE_DD"].ToString().Trim();
                PIRWORK_KG_AGE_MONS = row["PIRWORK_KG_AGE_MONS"].ToString().Trim();
                PIRWORK_KG_AGE_DAYS = row["PIRWORK_KG_AGE_DAYS"].ToString().Trim();
                PIRWORK_CHLD_REPEAT_HS = row["PIRWORK_CHLD_REPEAT_HS"].ToString().Trim();
                PIRWORK_CHLD_REPEAT_EHS = row["PIRWORK_CHLD_REPEAT_EHS"].ToString().Trim();
                PIRWORK_PREGNANT_MOTHER = row["PIRWORK_PREGNANT_MOTHER"].ToString().Trim();
                PIRWORK_WD_AGE_YY = row["PIRWORK_WD_AGE_YY"].ToString().Trim();
                PIRWORK_WD_AGE_MM = row["PIRWORK_WD_AGE_MM"].ToString().Trim();
                PIRWORK_WD_AGE_WW = row["PIRWORK_WD_AGE_WW"].ToString().Trim();
                PIRWORK_WD_AGE_DD = row["PIRWORK_WD_AGE_DD"].ToString().Trim();
                PIRWORK_WD_AGE_MONS = row["PIRWORK_WD_AGE_MONS"].ToString().Trim();
                PIRWORK_WD_AGE_DAYS = row["PIRWORK_WD_AGE_DAYS"].ToString().Trim();
                PIRWORK_WDRAW_CODE = row["PIRWORK_WDRAW_CODE"].ToString().Trim();
                if (strMode == "PIRWORK")
                {
                    Present_Days= row["Present_Days"].ToString().Trim();
                    Absent_Days = row["Absent_Days"].ToString().Trim();
                    Enroll_Days = row["Enroll_Days"].ToString().Trim();
                }
                else if (strMode != string.Empty)
                {

                    PIRWORK_RACEDESC = row["Racedesc"].ToString().Trim();
                    PIRWORK_ETHNICITYDESC = row["Ethnicitydesc"].ToString().Trim();
                    PIRWORK_FAMILYTYPEDESC = row["Familytypedesc"].ToString().Trim();
                    PIRWORK_LANGUAGEDESC = row["Languagedesc"].ToString().Trim();
                    PIRWORK_WDRAWDESC = row["Widrawdesc"].ToString().Trim();
                    PIRWORK_HOUSING = row["HOUSINGTYPE"].ToString().Trim();
                    PIRWORK_HOUSINGDESC = row["Housingtypedesc"].ToString().Trim();
                }
                
            }

        }

        public PirWorkEntity(DataRow row, string strTable)
        {
            if (row != null)
            {
                PIRWORK_AGENCY = row["PIRWORK_AGENCY"].ToString().Trim();
                PIRWORK_DEPT = row["PIRWORK_DEPT"].ToString().Trim();
                PIRWORK_PROG = row["PIRWORK_PROG"].ToString().Trim();
                PIRWORK_YEAR = row["PIRWORK_YEAR"].ToString().Trim();
                PIRWORK_APP_NO = row["PIRWORK_APP_NO"].ToString().Trim();
                PIRWORK_FUND = row["PIRWORK_FUND"].ToString().Trim();
                PIRWORK_FATTN = row["PIRWORK_FATTN"].ToString().Trim();
                PIRWORK_LATTN = row["PIRWORK_LATTN"].ToString().Trim();
                PIRWORK_ATTN_COUNT = row["PIRWORK_ATTN_COUNT"].ToString().Trim();
                PIRWORK_ENROLL_DATE = row["PIRWORK_ENROLL_DATE"].ToString().Trim();
                PIRWORK_WDRAW_DATE = row["PIRWORK_WDRAW_DATE"].ToString().Trim();
                PIRWORK_APP_ACTIVE = row["PIRWORK_APP_ACTIVE"].ToString().Trim();
                PIRWORK_APP_FNAME = row["PIRWORK_APP_FNAME"].ToString().Trim();
                PIRWORK_APP_LNAME = row["PIRWORK_APP_LNAME"].ToString().Trim();
                PIRWORK_APP_MNAME = row["PIRWORK_APP_MNAME"].ToString().Trim();
                PIRWORK_SITE = row["PIRWORK_SITE"].ToString().Trim();
                PIRWORK_SITE_ROOM = row["PIRWORK_SITE_ROOM"].ToString().Trim();
                PIRWORK_SITE_AMPM = row["PIRWORK_SITE_AMPM"].ToString().Trim();
                PIRWORK_APP_SSN = row["PIRWORK_APP_SSN"].ToString().Trim();
                PIRWORK_FAMILY_ID = row["PIRWORK_FAMILY_ID"].ToString().Trim();
                PIRWORK_INCOME_TYPES = row["PIRWORK_INCOME_TYPES"].ToString().Trim();
                PIRWORK_POVERTY = row["PIRWORK_POVERTY"].ToString().Trim();
                PIRWORK_FAMILY_TYPE = row["PIRWORK_FAMILY_TYPE"].ToString().Trim();
                PIRWORK_ETHNICITY = row["PIRWORK_ETHNICITY"].ToString().Trim();
                PIRWORK_RACE = row["PIRWORK_RACE"].ToString().Trim();
                PIRWORK_LANGUAGE = row["PIRWORK_LANGUAGE"].ToString().Trim();
                PIRWORK_KG_AGE_MM = row["PIRWORK_KG_AGE_MM"].ToString().Trim();
                PIRWORK_KG_AGE_YY = row["PIRWORK_KG_AGE_YY"].ToString().Trim();
            }

        }

        #endregion

        #region Properties

        public string PIRWORK_AGENCY { get; set; }
        public string PIRWORK_DEPT { get; set; }
        public string PIRWORK_PROG { get; set; }
        public string PIRWORK_YEAR { get; set; }
        public string PIRWORK_APP_NO { get; set; }
        public string PIRWORK_FUND { get; set; }
        public string PIRWORK_FATTN { get; set; }
        public string PIRWORK_LATTN { get; set; }
        public string PIRWORK_ATTN_COUNT { get; set; }
        public string PIRWORK_ENROLL_DATE { get; set; }
        public string PIRWORK_WDRAW_DATE { get; set; }
        public string PIRWORK_APP_ACTIVE { get; set; }
        public string PIRWORK_APP_FNAME { get; set; }
        public string PIRWORK_APP_LNAME { get; set; }
        public string PIRWORK_APP_MNAME { get; set; }
        public string PIRWORK_APP_SSN { get; set; }
        public string PIRWORK_FAMILY_ID { get; set; }
        public string PIRWORK_SITE { get; set; }
        public string PIRWORK_SITE_ROOM { get; set; }
        public string PIRWORK_SITE_AMPM { get; set; }
        public string PIRWORK_INCOME_TYPES { get; set; }
        public string PIRWORK_POVERTY { get; set; }
        public string PIRWORK_FAMILY_TYPE { get; set; }
        public string PIRWORK_CHLD_REPEAT { get; set; }
        public string PIRWORK_ETHNICITY { get; set; }
        public string PIRWORK_RACE { get; set; }
        public string PIRWORK_LANGUAGE { get; set; }
        public string PIRWORK_SNP_AGE_YY { get; set; }
        public string PIRWORK_SNP_AGE_MM { get; set; }
        public string PIRWORK_SNP_AGE_WW { get; set; }
        public string PIRWORK_SNP_AGE_DD { get; set; }
        public string PIRWORK_SNP_AGE_MONS { get; set; }
        public string PIRWORK_SNP_AGE_DAYS { get; set; }
        public string PIRWORK_FA_AGE_YY { get; set; }
        public string PIRWORK_FA_AGE_MM { get; set; }
        public string PIRWORK_FA_AGE_WW { get; set; }
        public string PIRWORK_FA_AGE_DD { get; set; }
        public string PIRWORK_FA_AGE_MONS { get; set; }
        public string PIRWORK_FA_AGE_DAYS { get; set; }
        public string PIRWORK_LA_AGE_YY { get; set; }
        public string PIRWORK_LA_AGE_MM { get; set; }
        public string PIRWORK_LA_AGE_WW { get; set; }
        public string PIRWORK_LA_AGE_DD { get; set; }
        public string PIRWORK_LA_AGE_MONS { get; set; }
        public string PIRWORK_LA_AGE_DAYS { get; set; }
        public string PIRWORK_KG_AGE_YY { get; set; }
        public string PIRWORK_KG_AGE_MM { get; set; }
        public string PIRWORK_KG_AGE_WW { get; set; }
        public string PIRWORK_KG_AGE_DD { get; set; }
        public string PIRWORK_KG_AGE_MONS { get; set; }
        public string PIRWORK_KG_AGE_DAYS { get; set; }
        public string PIRWORK_CHLD_REPEAT_HS { get; set; }
        public string PIRWORK_CHLD_REPEAT_EHS { get; set; }
        public string PIRWORK_PREGNANT_MOTHER { get; set; }
        public string PIRWORK_WD_AGE_YY { get; set; }
        public string PIRWORK_WD_AGE_MM { get; set; }
        public string PIRWORK_WD_AGE_WW { get; set; }
        public string PIRWORK_WD_AGE_DD { get; set; }
        public string PIRWORK_WD_AGE_MONS { get; set; }
        public string PIRWORK_WD_AGE_DAYS { get; set; }
        public string PIRWORK_Show { get; set; }
        public string Mode { get; set; }
        public string PIRWORK_WDRAW_CODE { get; set; }
        public string PIRWORK_RACEDESC { get; set; }
        public string PIRWORK_ETHNICITYDESC { get; set; }
        public string PIRWORK_FAMILYTYPEDESC { get; set; }
        public string PIRWORK_LANGUAGEDESC { get; set; }
        public string PIRWORK_WDRAWDESC { get; set; }
        public string PIRWORK_HOUSING { get; set; }
        public string PIRWORK_HOUSINGDESC { get; set; }

        public string Present_Days { get; set; }
        public string Absent_Days { get; set; }
        public string Enroll_Days { get; set; }
        #endregion
    }



    public class RoomCount_Entity
    {
        #region Constructors

        public RoomCount_Entity()
        {
            Code = Desc = Count = string.Empty;
        }

        public RoomCount_Entity(bool Initialize)
        {
            if (Initialize)
                Code = Desc = Count = null;
        }

        public RoomCount_Entity(RoomCount_Entity Entity)
        {
            Code = Entity.Code;
            Desc = Entity.Desc;
            Count = Entity.Count;
        }

        public RoomCount_Entity(string code, string desc, string Cnt)
        {
            Code = code;
            Desc = desc;
            Count = Cnt;
        }

        #endregion

        #region Properties

        public string Code { get; set; }
        public string Desc { get; set; }
        public string Count { get; set; }

        #endregion

    }


    public class PirCntl
    {

        #region Constructors

        public PirCntl()
        {
            PIRCNTL_FUND_CODE = string.Empty;
            PIRCNTL_FUND_NAME = string.Empty;
            PIRCNTL_FUND_TYPE = string.Empty;
            PIRCNTL_DATE_ADD = string.Empty;
            PIRCNTL_ADD_OPERATOR = string.Empty;
            PIRCNTL_DATE_LSTC = string.Empty;
            PIRCNTL_LSTC_OPERATOR = string.Empty;
        }

        public PirCntl(DataRow row)
        {
            if (row != null)
            {
                PIRCNTL_FUND_CODE = row["PIRCNTL_FUND_CODE"].ToString().Trim();
                PIRCNTL_FUND_NAME = row["PIRCNTL_FUND_NAME"].ToString().Trim();
                PIRCNTL_FUND_TYPE = row["PIRCNTL_FUND_TYPE"].ToString().Trim();
                PIRCNTL_DATE_ADD = row["PIRCNTL_DATE_ADD"].ToString().Trim();
                PIRCNTL_ADD_OPERATOR = row["PIRCNTL_ADD_OPERATOR"].ToString().Trim();
                PIRCNTL_DATE_LSTC = row["PIRCNTL_DATE_LSTC"].ToString().Trim();
                PIRCNTL_LSTC_OPERATOR = row["PIRCNTL_LSTC_OPERATOR"].ToString().Trim();

            }

        }

        #endregion

        #region Properties

        public string PIRCNTL_FUND_CODE { get; set; }
        public string PIRCNTL_FUND_NAME { get; set; }
        public string PIRCNTL_FUND_TYPE { get; set; }
        public string PIRCNTL_DATE_ADD { get; set; }
        public string PIRCNTL_ADD_OPERATOR { get; set; }
        public string PIRCNTL_DATE_LSTC { get; set; }
        public string PIRCNTL_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion

    }


    public class PirassnEntity
    {

        #region Constructors

        public PirassnEntity()
        {
            PIRASSN_AGENCY = string.Empty;
            PIRASSN_DEPT = string.Empty;
            PIRASSN_PROG = string.Empty;
            PIRASSN_YEAR = string.Empty;
            PIRASSN_Q_FUND = string.Empty;
            PIRASSN_Q_ID = string.Empty;
            PIRASSN_Q_SEC = string.Empty;
            PIRASSN_GRP = string.Empty;
            PIRASSN_SEQ = string.Empty;
            PIRASSN_Q_TYPE = string.Empty;
            PIRASSN_Q_CODE = string.Empty;
            PIRASSN_Q_SCODE = string.Empty;
            PIRASSN_CONJ = string.Empty;
            PIRASSN_TASK = string.Empty;
            PIRASSN_YEAR_TYPE = string.Empty;
            PIRASSN_CHK_RESP = string.Empty;
            PIRASSN_RESPONSE = string.Empty;
            PIRASSN_DATE_TYPE = string.Empty;
            PIRASSN_FDATE = string.Empty;
            PIRASSN_TDATE = string.Empty;
            PIRASSN_CHK_DATE = string.Empty;
            PIRASSN_INTAKE_AGYTAB = string.Empty;
            PIRASSN_INTAKE_CODE = string.Empty;
            PIRASSN_SERVICE = string.Empty;
            PIRASSN_CUSTOM_CODE = string.Empty;
            PIRASSN_CUSTOM_RESP = string.Empty;
            PIRASSN_CUSTOM_SEQ = string.Empty;
            PIRASSN_DATE_ADD = string.Empty;
            PIRASSN_ADD_OPERATOR = string.Empty;
            PIRASSN_DATE_LSTC = string.Empty;
            PIRASSN_LSTC_OPERATOR = string.Empty;
        }

        public PirassnEntity(DataRow row)
        {
            if (row != null)
            {
                PIRASSN_AGENCY = row["PIRASSN_AGENCY"].ToString().Trim();
                PIRASSN_DEPT = row["PIRASSN_DEPT"].ToString().Trim();
                PIRASSN_PROG = row["PIRASSN_PROG"].ToString().Trim();
                PIRASSN_YEAR = row["PIRASSN_YEAR"].ToString().Trim();
                PIRASSN_Q_FUND = row["PIRASSN_Q_FUND"].ToString().Trim();
                PIRASSN_Q_ID = row["PIRASSN_Q_ID"].ToString().Trim();
                PIRASSN_Q_SEC = row["PIRASSN_Q_SEC"].ToString().Trim();
                PIRASSN_GRP = row["PIRASSN_GRP"].ToString().Trim();
                PIRASSN_SEQ = row["PIRASSN_SEQ"].ToString().Trim();
                PIRASSN_Q_TYPE = row["PIRASSN_Q_TYPE"].ToString().Trim();
                PIRASSN_Q_CODE = row["PIRASSN_Q_CODE"].ToString().Trim();
                PIRASSN_Q_SCODE = row["PIRASSN_Q_SCODE"].ToString().Trim();
                PIRASSN_CONJ = row["PIRASSN_CONJ"].ToString().Trim();
                PIRASSN_TASK = row["PIRASSN_TASK"].ToString().Trim();
                PIRASSN_YEAR_TYPE = row["PIRASSN_YEAR_TYPE"].ToString().Trim();
                PIRASSN_CHK_RESP = row["PIRASSN_CHK_RESP"].ToString().Trim();
                PIRASSN_RESPONSE = row["PIRASSN_RESPONSE"].ToString().Trim();
                PIRASSN_DATE_TYPE = row["PIRASSN_DATE_TYPE"].ToString().Trim();
                PIRASSN_FDATE = row["PIRASSN_FDATE"].ToString().Trim();
                PIRASSN_TDATE = row["PIRASSN_TDATE"].ToString().Trim();
                PIRASSN_CHK_DATE = row["PIRASSN_CHK_DATE"].ToString().Trim();
                PIRASSN_INTAKE_AGYTAB = row["PIRASSN_INTAKE_AGYTAB"].ToString().Trim();
                PIRASSN_INTAKE_CODE = row["PIRASSN_INTAKE_CODE"].ToString().Trim();
                PIRASSN_SERVICE = row["PIRASSN_SERVICE"].ToString().Trim();
                PIRASSN_CUSTOM_CODE = row["PIRASSN_CUSTOM_CODE"].ToString().Trim();
                PIRASSN_CUSTOM_RESP = row["PIRASSN_CUSTOM_RESP"].ToString().Trim();
                PIRASSN_CUSTOM_SEQ = row["PIRASSN_CUSTOM_SEQ"].ToString().Trim();
                PIRASSN_DATE_ADD = row["PIRASSN_DATE_ADD"].ToString().Trim();
                PIRASSN_ADD_OPERATOR = row["PIRASSN_ADD_OPERATOR"].ToString().Trim();
                PIRASSN_DATE_LSTC = row["PIRASSN_DATE_LSTC"].ToString().Trim();
                PIRASSN_LSTC_OPERATOR = row["PIRASSN_LSTC_OPERATOR"].ToString().Trim();

            }

        }

        #endregion

        #region Properties

        public string PIRASSN_AGENCY { get; set; }
        public string PIRASSN_DEPT { get; set; }
        public string PIRASSN_PROG { get; set; }
        public string PIRASSN_YEAR { get; set; }
        public string PIRASSN_Q_FUND { get; set; }
        public string PIRASSN_Q_ID { get; set; }
        public string PIRASSN_Q_SEC { get; set; }
        public string PIRASSN_GRP { get; set; }
        public string PIRASSN_SEQ { get; set; }
        public string PIRASSN_Q_TYPE { get; set; }
        public string PIRASSN_Q_CODE { get; set; }
        public string PIRASSN_Q_SCODE { get; set; }
        public string PIRASSN_CONJ { get; set; }
        public string PIRASSN_TASK { get; set; }
        public string PIRASSN_YEAR_TYPE { get; set; }
        public string PIRASSN_CHK_RESP { get; set; }
        public string PIRASSN_RESPONSE { get; set; }
        public string PIRASSN_DATE_TYPE { get; set; }
        public string PIRASSN_FDATE { get; set; }
        public string PIRASSN_TDATE { get; set; }
        public string PIRASSN_CHK_DATE { get; set; }
        public string PIRASSN_INTAKE_AGYTAB { get; set; }
        public string PIRASSN_INTAKE_CODE { get; set; }
        public string PIRASSN_SERVICE { get; set; }
        public string PIRASSN_CUSTOM_CODE { get; set; }
        public string PIRASSN_CUSTOM_RESP { get; set; }
        public string PIRASSN_CUSTOM_SEQ { get; set; }
        public string PIRASSN_DATE_ADD { get; set; }
        public string PIRASSN_ADD_OPERATOR { get; set; }
        public string PIRASSN_DATE_LSTC { get; set; }
        public string PIRASSN_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion

    }

    public class Pirassn2Entity
    {
        #region Constructors

        public Pirassn2Entity()
        {
            PIRASSN_AGENCY = string.Empty;
            PIRASSN_DEPT = string.Empty;
            PIRASSN_PROG = string.Empty;
            PIRASSN_YEAR = string.Empty;
            PIRASSN_Q_FUND = string.Empty;
            PIRASSN_Q_ID = string.Empty;
            PIRASSN_GRP = string.Empty;
            PIRASSN_SEQ = string.Empty;
            PIRASSN_COL_ID = string.Empty;
            PIRASSN_Q_TYPE = string.Empty;
            PIRASSN_CONJ = string.Empty;
            PIRASSN_TASK = string.Empty;
            PIRASSN_YEAR_TYPE = string.Empty;
            PIRASSN_CHK_RESP = string.Empty;
            PIRASSN_RESPONSE = string.Empty;
            PIRASSN_DATE_TYPE = string.Empty;
            PIRASSN_FDATE = string.Empty;
            PIRASSN_TDATE = string.Empty;
            PIRASSN_CHK_DATE = string.Empty;
            PIRASSN_INTAKE_AGYTAB = string.Empty;
            PIRASSN_INTAKE_CODE = string.Empty;
            PIRASSN_SERVICE = string.Empty;
            PIRASSN_CUSTOM_CODE = string.Empty;
            PIRASSN_CUSTOM_RESP = string.Empty;
            PIRASSN_CUSTOM_SEQ = string.Empty;
            PIRASSN_DATE_ADD = string.Empty;
            PIRASSN_ADD_OPERATOR = string.Empty;
            PIRASSN_DATE_LSTC = string.Empty;
            PIRASSN_LSTC_OPERATOR = string.Empty;
        }

        public Pirassn2Entity(DataRow row)
        {
            if (row != null)
            {
                PIRASSN_AGENCY = row["PIRASSN_AGENCY"].ToString().Trim();
                PIRASSN_DEPT = row["PIRASSN_DEPT"].ToString().Trim();
                PIRASSN_PROG = row["PIRASSN_PROG"].ToString().Trim();
                PIRASSN_YEAR = row["PIRASSN_YEAR"].ToString().Trim();
                PIRASSN_Q_FUND = row["PIRASSN_Q_FUND"].ToString().Trim();
                PIRASSN_Q_ID = row["PIRASSN_Q_ID"].ToString().Trim();
                PIRASSN_GRP = row["PIRASSN_GRP"].ToString().Trim();
                PIRASSN_SEQ = row["PIRASSN_SEQ"].ToString().Trim();
                PIRASSN_COL_ID = row["PIRASSN_COL_ID"].ToString().Trim();
                PIRASSN_Q_TYPE = row["PIRASSN_Q_TYPE"].ToString().Trim();
                PIRASSN_CONJ = row["PIRASSN_CONJ"].ToString().Trim();
                PIRASSN_TASK = row["PIRASSN_TASK"].ToString().Trim();
                PIRASSN_YEAR_TYPE = row["PIRASSN_YEAR_TYPE"].ToString().Trim();
                PIRASSN_CHK_RESP = row["PIRASSN_CHK_RESP"].ToString().Trim();
                PIRASSN_RESPONSE = row["PIRASSN_RESPONSE"].ToString().Trim();
                PIRASSN_DATE_TYPE = row["PIRASSN_DATE_TYPE"].ToString().Trim();
                PIRASSN_FDATE = row["PIRASSN_FDATE"].ToString().Trim();
                PIRASSN_TDATE = row["PIRASSN_TDATE"].ToString().Trim();
                PIRASSN_CHK_DATE = row["PIRASSN_CHK_DATE"].ToString().Trim();
                PIRASSN_INTAKE_AGYTAB = row["PIRASSN_INTAKE_AGYTAB"].ToString().Trim();
                PIRASSN_INTAKE_CODE = row["PIRASSN_INTAKE_CODE"].ToString().Trim();
                PIRASSN_SERVICE = row["PIRASSN_SERVICE"].ToString().Trim();
                PIRASSN_CUSTOM_CODE = row["PIRASSN_CUSTOM_CODE"].ToString().Trim();
                PIRASSN_CUSTOM_RESP = row["PIRASSN_CUSTOM_RESP"].ToString().Trim();
                PIRASSN_CUSTOM_SEQ = row["PIRASSN_CUSTOM_SEQ"].ToString().Trim();
                PIRASSN_DATE_ADD = row["PIRASSN_DATE_ADD"].ToString().Trim();
                PIRASSN_ADD_OPERATOR = row["PIRASSN_ADD_OPERATOR"].ToString().Trim();
                PIRASSN_DATE_LSTC = row["PIRASSN_DATE_LSTC"].ToString().Trim();
                PIRASSN_LSTC_OPERATOR = row["PIRASSN_LSTC_OPERATOR"].ToString().Trim();

            }

        }

        #endregion

        #region Properties

        public string PIRASSN_AGENCY { get; set; }
        public string PIRASSN_DEPT { get; set; }
        public string PIRASSN_PROG { get; set; }
        public string PIRASSN_YEAR { get; set; }
        public string PIRASSN_Q_FUND { get; set; }
        public string PIRASSN_Q_ID { get; set; }
        public string PIRASSN_GRP { get; set; }
        public string PIRASSN_SEQ { get; set; }
        public string PIRASSN_COL_ID
        {
            get; set;
        }
        public string PIRASSN_Q_TYPE { get; set; }
       
        public string PIRASSN_CONJ { get; set; }
        public string PIRASSN_TASK { get; set; }
        public string PIRASSN_YEAR_TYPE { get; set; }
        public string PIRASSN_CHK_RESP { get; set; }
        public string PIRASSN_RESPONSE { get; set; }
        public string PIRASSN_DATE_TYPE { get; set; }
        public string PIRASSN_FDATE { get; set; }
        public string PIRASSN_TDATE { get; set; }
        public string PIRASSN_CHK_DATE { get; set; }
        public string PIRASSN_INTAKE_AGYTAB { get; set; }
        public string PIRASSN_INTAKE_CODE { get; set; }
        public string PIRASSN_SERVICE { get; set; }
        public string PIRASSN_CUSTOM_CODE { get; set; }
        public string PIRASSN_CUSTOM_RESP { get; set; }
        public string PIRASSN_CUSTOM_SEQ { get; set; }
        public string PIRASSN_DATE_ADD { get; set; }
        public string PIRASSN_ADD_OPERATOR { get; set; }
        public string PIRASSN_DATE_LSTC { get; set; }
        public string PIRASSN_LSTC_OPERATOR { get; set; }
        public string Mode { get; set; }

        #endregion

    }



}
