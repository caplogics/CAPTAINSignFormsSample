using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;
using Captain.Common.Views.UserControls;
using DevExpress.ExpressApp.Validation;

namespace Captain.Common.Model.Objects
{
    public class AdhocTableEntity
    {
        #region Constructors

        public AdhocTableEntity()
        {

            Table_name = string.Empty;
            Tot_Columns = string.Empty;
            Created_Date = string.Empty;
            Modifide_Date = string.Empty;

        }

        public AdhocTableEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Table_name = row["name"].ToString();
                Tot_Columns = row["max_column_id_used"].ToString();
                Created_Date = row["create_date"].ToString();
                Modifide_Date = row["modify_date"].ToString();
            }
        }

 

        #endregion

        #region Properties

        public string Table_name { get; set; }
        public string Tot_Columns { get; set; }
        public string Created_Date { get; set; }
        public string Modifide_Date { get; set; }

        #endregion

    }


    public class AdhocColumnEntity
    {
        #region Constructors

        public AdhocColumnEntity()
        {

            Table_name = string.Empty;
            Column_name = string.Empty;
            Column_Position = string.Empty;
            Is_Nullable = string.Empty;
            DataType = string.Empty;
            string_MaxLen = string.Empty;
            Numeric_Precision = string.Empty;
            Numeric_Radix = string.Empty;
            Numeric_Scale = string.Empty;

        }

        public AdhocColumnEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Table_name = row["TABLE_NAME"].ToString().Trim();
                Column_name = row["COLUMN_NAME"].ToString().Trim();
                Column_Position = row["ORDINAL_POSITION"].ToString();
                Is_Nullable = row["IS_NULLABLE"].ToString();
                DataType = row["DATA_TYPE"].ToString();
                string_MaxLen = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                Numeric_Precision = row["NUMERIC_PRECISION"].ToString();
                Numeric_Radix = row["NUMERIC_PRECISION_RADIX"].ToString();
                Numeric_Scale = row["NUMERIC_SCALE"].ToString();
            }
        }



        #endregion

        #region Properties

        public string Table_name { get; set; }
        public string Column_name { get; set; }
        public string Column_Position { get; set; }
        public string Is_Nullable { get; set; }
        public string DataType { get; set; }
        public string string_MaxLen { get; set; }
        public string Numeric_Precision { get; set; }
        public string Numeric_Radix { get; set; }
        public string Numeric_Scale { get; set; }

        #endregion

    }

    public class AdhocSel_CriteriaEntity
    {
        #region Constructors

        public AdhocSel_CriteriaEntity()
        {
            Can_Add_Col = string.Empty;
            Table_ID = string.Empty;
            Table_name = string.Empty;
            Column_Disp_Name = string.Empty;
            Column_Name = string.Empty;
            Col_Ordinal = string.Empty;
            Disp_Data_Type = string.Empty;
            Data_Type = string.Empty;
            Disp_Code_Length = string.Empty;
            Disp_Desc_Length = string.Empty;
            Display = string.Empty;
            Description = string.Empty;
            Count = string.Empty;
            Sort = string.Empty;
            Break = string.Empty;
            Sort_Order = string.Empty;
            Break_Order = string.Empty;
            EqualTo = string.Empty;
            NotEqualTo = string.Empty;
            LessThan = string.Empty;
            GreaterThan = string.Empty;
            AgyCode = string.Empty;
            Disp_Position = string.Empty;
            Attributes = string.Empty;
            Get_Nulls = string.Empty;

            Max_Display_Width = string.Empty;
            Col_Format_Type = string.Empty;
            Col_Master_Code = string.Empty;
            Criteria_SW = string.Empty;
            Countable_SW = string.Empty;
            Col_Catg = string.Empty;
        }

        public AdhocSel_CriteriaEntity(bool Initialize )
        {
            if (Initialize)
            {
                Can_Add_Col = null;
                Table_ID = null;
                Table_name = null;
                Column_Disp_Name = null;
                Column_Name = null;
                Col_Ordinal = null;
                Disp_Data_Type = null;
                Data_Type = null;
                Disp_Code_Length = null;
                Disp_Desc_Length = null;
                Display = null;
                Description = null;
                Count = null;
                Sort = null;
                Break = null;
                Sort_Order = null;
                Break_Order = null;
                EqualTo = null;
                NotEqualTo = null;
                LessThan = null;
                GreaterThan = null;
                AgyCode = null;
                Disp_Position = null;
                Attributes = null;
                Get_Nulls = null;

                Max_Display_Width = null;
                Col_Format_Type = null;
                Col_Master_Code = null;
                Criteria_SW = null;
                Countable_SW = null;
                Col_Catg = null;
            }
        }


        public AdhocSel_CriteriaEntity(AdhocSel_CriteriaEntity Entity)
        {
            if (Entity != null)
            {
                Can_Add_Col = Entity.Can_Add_Col;

                Table_ID = Entity.Table_ID;
                Table_name = Entity.Table_name;
                Column_Disp_Name = Entity.Column_Disp_Name;
                Column_Name = Entity.Column_Name;
                Col_Ordinal = Entity.Col_Ordinal;
                Disp_Data_Type = Entity.Disp_Data_Type;
                Data_Type = Entity.Data_Type;
                Disp_Code_Length = Entity.Disp_Code_Length;
                Disp_Desc_Length = Entity.Disp_Desc_Length;
                Display = Entity.Display;
                Description = Entity.Description;
                Count = Entity.Count;
                Sort = Entity.Sort;
                Break = Entity.Break;
                Sort_Order = Entity.Sort_Order;
                Break_Order = Entity.Break_Order;
                EqualTo = Entity.EqualTo;
                NotEqualTo = Entity.NotEqualTo;
                LessThan = Entity.LessThan;
                GreaterThan = Entity.GreaterThan;
                AgyCode = Entity.AgyCode;
                Disp_Position = Entity.Disp_Position;
                Attributes = Entity.Attributes;
                Get_Nulls = Entity.Get_Nulls;

                Max_Display_Width = Entity.Max_Display_Width;
                Col_Format_Type = Entity.Col_Format_Type;
                Col_Master_Code = Entity.Col_Master_Code;
                Criteria_SW = Entity.Criteria_SW;
                Countable_SW = Entity.Countable_SW;
                Col_Catg = Entity.Col_Catg;
            }
        }

        public AdhocSel_CriteriaEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;
                Table_name = row["name"].ToString();
            }
        }



        #endregion

        #region Properties

        public string Can_Add_Col { get; set; }
        public string Table_ID { get; set; }
        public string Table_name { get; set; }
        public string Column_Disp_Name { get; set; }
        public string Column_Name { get; set; }
        public string Col_Ordinal { get; set; }
        public string Disp_Code_Length { get; set; }
        public string Disp_Desc_Length { get; set; }
        public string Disp_Data_Type { get; set; }
        public string Data_Type { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }
        public string Count { get; set; }
        public string Sort { get; set; }
        public string Break { get; set; }
        public string Sort_Order { get; set; }
        public string Break_Order { get; set; }
        public string EqualTo { get; set; }
        public string NotEqualTo { get; set; }
        public string LessThan { get; set; }
        public string GreaterThan { get; set; }
        public string AgyCode { get; set; }
        public string Disp_Position { get; set; }
        public string Attributes { get; set; }
        public string Get_Nulls { get; set; }

        public string Max_Display_Width { get; set; }
        public string Col_Format_Type { get; set; }
        public string Col_Master_Code { get; set; }
        public string Criteria_SW { get; set; }
        public string Countable_SW { get; set; }

        public string Col_Catg { get; set; }

        #endregion

    }

    public class ADHOCFLDEntity
    {
        #region Constructors

        public ADHOCFLDEntity()
        {
            Col_Code = string.Empty;
            Col_Disp_Name = string.Empty;
            Col_Desc_Length = string.Empty;
            Col_Format_Type = string.Empty;
            Col_Resp_Type = string.Empty;
            Col_AgyCode = string.Empty;
            Active = string.Empty;
            Act_Col_Name = string.Empty;
            Col_Code_Length = string.Empty;
            Have_Criteria = string.Empty;
            Have_Count = string.Empty;

            Col_Catgeory = string.Empty;

        }

        public ADHOCFLDEntity(bool Initialize)
        {
            if (Initialize)
            {
                Col_Code = null;
                Col_Disp_Name = null;
                Col_Desc_Length = null;
                Col_Format_Type = null;
                Col_Resp_Type = null;
                Col_AgyCode = null;
                Active = null;
                Act_Col_Name = null;
                Col_Code_Length = null;
                Have_Criteria = null;
                Have_Count = null;

                Col_Catgeory = null;
            }
        }


        public ADHOCFLDEntity(ADHOCFLDEntity Entity)
        {
            if (Entity != null)
            {
                Col_Code = Entity.Col_Code;
                Col_Disp_Name = Entity.Col_Disp_Name;
                Col_Desc_Length = Entity.Col_Desc_Length;
                Col_Format_Type = Entity.Col_Format_Type;
                Col_Resp_Type = Entity.Col_Format_Type;
                Col_AgyCode = Entity.Col_AgyCode;
                Active = Entity.Active;
                Act_Col_Name = Entity.Act_Col_Name;
                Col_Code_Length = Entity.Col_Code_Length;
                Have_Criteria = Entity.Have_Criteria;
                Have_Count = Entity.Have_Count;

                Col_Catgeory = Entity.Col_Catgeory;
            }
        }

        public ADHOCFLDEntity(DataRow row)
        {
            if (row != null)
            {
                Col_Code = row["ADHOCFLD_CODE"].ToString().Trim();
                Col_Disp_Name = row["ADHOCFLD_NAME"].ToString().Trim();
                Col_Format_Type = row["ADHOCFLD_FLD_TYPE"].ToString().Trim();
                Col_Resp_Type = row["ADHOCFLD_RESP_TYPE"].ToString().Trim();
                Col_AgyCode = row["ADHOCFLD_AGYCODE"].ToString().Trim();
                Active = row["ADHOCFLD_ACTIVE"].ToString().Trim();
                Act_Col_Name = row["ADHOCFLD_FIELD"].ToString().Trim();
                Col_Desc_Length = row["ADHOCFLD_DESC_LEN"].ToString().Trim();
                Col_Code_Length = row["ADHOCFLD_CODE_LEN"].ToString().Trim();
                Have_Criteria = row["ADHOCFLD_CRITERIA"].ToString().Trim();
                Have_Count = row["ADHOCFLD_COUNTABLE"].ToString().Trim();

                Col_Catgeory = row["ADHOCFLD_PAY_CATG"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string Col_Code { get; set; }
        public string Col_Disp_Name { get; set; }
        public string Col_Desc_Length { get; set; }
        public string Col_Format_Type { get; set; }
        public string Col_Resp_Type { get; set; }
        public string Col_AgyCode { get; set; }
        public string Active { get; set; }
        public string Act_Col_Name { get; set; }
        public string Col_Code_Length { get; set; }
        public string Have_Criteria { get; set; }
        public string Have_Count { get; set; }
        public string Col_Catgeory { get; set; }

        #endregion

    }
    
    public class ADHOCFLSEntity
    {
        #region Constructors

        public ADHOCFLSEntity()
        {
            Category =
            Module = 
            Table_Code = 
            Table_Name = 
            Table_Desc = 
            Active_Stat = 
            Primary_Key_Length = 
            Priv_Table_Key_Length = 
            Primary_Key = 
            Column_Perfix = 
            Assoc_Tables = string.Empty;
        }

        public ADHOCFLSEntity(bool Initialize)
        {
            if (Initialize)
            {
                Category =
                Module = 
                Table_Code = 
                Table_Name = 
                Table_Desc = 
                Active_Stat =
                Primary_Key_Length = 
                Priv_Table_Key_Length = 
                Primary_Key = 
                Column_Perfix = null;
            }
        }

        public ADHOCFLSEntity(ADHOCFLSEntity Entity)
        {
            if (Entity != null)
            {
                Category = Entity.Category;
                Module = Entity.Module;
                Table_Code = Entity.Table_Code;
                Table_Name = Entity.Table_Name;
                Table_Desc = Entity.Table_Desc;
                Active_Stat = Entity.Active_Stat;
                Primary_Key_Length = Entity.Primary_Key_Length;
                Priv_Table_Key_Length = Entity.Priv_Table_Key_Length;
                Primary_Key = Entity.Primary_Key;
                Column_Perfix = Entity.Column_Perfix;
                Assoc_Tables = Entity.Assoc_Tables;
            }
        }

        public ADHOCFLSEntity(DataRow row)
        {
            if (row != null)
            {
                Category = row["ADHOCFLS_CATEGORY"].ToString().Trim();
                Module = row["ADHOCFLS_MODULE"].ToString().Trim();
                Table_Code = row["ADHOCFLS_CODE"].ToString().Trim();
                Table_Name = row["ADHOCFLS_NAME"].ToString().Trim();
                Table_Desc = row["ADHOCFLS_DESCRIPTION"].ToString().Trim();
                Active_Stat = row["ADHOCFLS_ACTIVE"].ToString().Trim();
                Primary_Key_Length = row["ADHOCFLS_PRIMARY_LEN"].ToString().Trim();
                Column_Perfix = row["ADHOCFLS_Column_Prefix"].ToString().Trim();
                Priv_Table_Key_Length = row["ADHOCFLS_PREVKEY_LEN"].ToString().Trim();
                Assoc_Tables = string.Empty;
                //Assoc_Tables = row["ADHOCFLS_ASOC_TABLES"].ToString().Trim();
                Primary_Key = row["ADHOCFLS_PRIMARY_KEY"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Category { get; set; }
        public string Module { get; set; }
        public string Table_Code { get; set; }
        public string Table_Name { get; set; }
        public string Table_Desc { get; set; }
        public string Active_Stat { get; set; }
        public string Primary_Key_Length { get; set; }
        public string Priv_Table_Key_Length { get; set; }
        public string Primary_Key { get; set; }
        public string Column_Perfix { get; set; }
        public string Assoc_Tables { get; set; }

        #endregion

    }

    public class ADHOCCTGEntity
    {
        #region Constructors

        public ADHOCCTGEntity()
        {
            Catg_Code = 
            Module = 
            Catg_Desc = string.Empty;
        }

        public ADHOCCTGEntity(bool Initialize)
        {
            if (Initialize)
            {
                Catg_Code = 
                Module = 
                Catg_Desc = null;
            }
        }

        public ADHOCCTGEntity(ADHOCCTGEntity Entity)
        {
            if (Entity != null)
            {
                Catg_Code = Entity.Catg_Code;
                Module = Entity.Module;
                Catg_Desc = Entity.Catg_Desc;
            }
        }

        public ADHOCCTGEntity(DataRow row)
        {
            if (row != null)
            {
                Catg_Code = row["ADHOCCTG_CODE"].ToString().Trim();
                Module = row["ADHOCCTG_MODULE"].ToString().Trim();
                Catg_Desc = row["ADHOCCTG_DESC"].ToString().Trim();
            }
        }

        #endregion

        #region Properties

        public string Catg_Code { get; set; }
        public string Module { get; set; }
        public string Catg_Desc { get; set; }

        #endregion
    }

    public class AGYTABSEntity
    {
        #region Constructors

        public AGYTABSEntity()
        {
            Tabs_Type = 
            Table_Code = 
            Code_Desc = 
            Active =
            Default =
            Hierarchy = 
            Code_Sel = string.Empty;
            Equal_Code = string.Empty;
        }

        public AGYTABSEntity(bool Initialize)
        {
            if (Initialize)
            {
                Tabs_Type = 
                Table_Code =
                Code_Desc = 
                Active = 
                Default = 
                Hierarchy = 
                Code_Sel = 
                Equal_Code = null;
            }
        }

        public AGYTABSEntity(AGYTABSEntity Entity)
        {
            if (Entity != null)
            {
                Tabs_Type = Entity.Tabs_Type;
                Table_Code = Entity.Table_Code.Trim();
                Code_Desc = Entity.Code_Desc.Trim();
                Active = Entity.Active;
                Default = Entity.Default;
                Hierarchy = Entity.Hierarchy;

                Code_Sel = Entity.Code_Sel;
                Equal_Code = Entity.Equal_Code;

            }
        }

        public AGYTABSEntity(DataRow row)
        {
            if (row != null)
            {
                Tabs_Type = row["AGYS_TYPE"].ToString().Trim();
                Table_Code = row["AGYS_CODE"].ToString().Trim();
                Code_Desc = row["AGYS_DESC"].ToString().Trim();
                Active = row["AGYS_ACTIVE"].ToString().Trim();
                Default = row["AGYS_DEFAULT"].ToString().Trim();
                Hierarchy = row["AGYS_HIERARCHY"].ToString().Trim();

                Code_Sel = " ";
                Equal_Code = " ";
            }
        }

        public AGYTABSEntity(DataRow row, string From_Agy)
        {
            Code_Desc = row["LookUpDesc"].ToString().Trim();
            Table_Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            Default = row["AGY_DEFAULT"].ToString().Trim();
            Hierarchy = row["Hierarchy"].ToString().Trim();

            Code_Sel = " ";
            Equal_Code = " ";
        }

        public AGYTABSEntity(DataRow row, string From_Agy,string RepFormat)
        {
            Code_Desc = row["LookUpDesc"].ToString().Trim();
            Table_Code = row["Code"].ToString().Trim();
            Active = row["Active"].ToString().Trim();
            //Default = row["AGY_DEFAULT"].ToString().Trim();
            //Hierarchy = row["Hierarchy"].ToString().Trim();

            Code_Sel = " ";
            Equal_Code = " ";
        }

        #endregion

        #region Properties

        public string Tabs_Type { get; set; }
        public string Table_Code { get; set; }
        public string Code_Desc { get; set; }
        public string Active { get; set; }
        public string Default { get; set; }
        public string Hierarchy { get; set; }

        public string Code_Sel { get; set; }
        public string Equal_Code { get; set; }

        #endregion

    }

    public class Adhoc_SummaryEntity
    {
        #region Constructors

        public Adhoc_SummaryEntity()
        {
            Col_Code = string.Empty;
            Col_Disp_Name = string.Empty;
            Col_Desc_Length = string.Empty;
            Col_Format_Type = string.Empty;
            Col_Resp_Type = string.Empty;
            Col_AgyCode = string.Empty;
            Active = string.Empty;
            Act_Col_Name = string.Empty;
            Col_Code_Length = string.Empty;
        }

        public Adhoc_SummaryEntity(bool Initialize)
        {
            if (Initialize)
            {
                Col_Code = null;
                Col_Disp_Name = null;
                Col_Desc_Length = null;
                Col_Format_Type = null;
                Col_Resp_Type = null;
                Col_AgyCode = null;
                Active = null;
                Act_Col_Name = null;
                Col_Code_Length = null;
            }
        }


        public Adhoc_SummaryEntity(Adhoc_SummaryEntity Entity)
        {
            if (Entity != null)
            {
                Col_Code = Entity.Col_Code;
                Col_Disp_Name = Entity.Col_Disp_Name;
                Col_Desc_Length = Entity.Col_Desc_Length;
                Col_Format_Type = Entity.Col_Format_Type;
                Col_Resp_Type = Entity.Col_Format_Type;
                Col_AgyCode = Entity.Col_AgyCode;
                Active = Entity.Active;
                Act_Col_Name = Entity.Act_Col_Name;
                Col_Code_Length = Entity.Col_Code_Length;
            }
        }

        public Adhoc_SummaryEntity(DataRow row)
        {
            if (row != null)
            {
                Col_Code = row["ADHOCFLD_CODE"].ToString().Trim();
                Col_Disp_Name = row["ADHOCFLD_NAME"].ToString().Trim();
                Col_Format_Type = row["ADHOCFLD_FLD_TYPE"].ToString().Trim();
                Col_Resp_Type = row["ADHOCFLD_RESP_TYPE"].ToString().Trim();
                Col_AgyCode = row["ADHOCFLD_AGYCODE"].ToString().Trim();
                Active = row["ADHOCFLD_ACTIVE"].ToString().Trim();
                Act_Col_Name = row["ADHOCFLD_FIELD"].ToString().Trim();
                Col_Desc_Length = row["ADHOCFLD_DESC_LEN"].ToString().Trim();
                Col_Code_Length = row["ADHOCFLD_CODE_LEN"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string Col_Code { get; set; }
        public string Col_Disp_Name { get; set; }
        public string Col_Desc_Length { get; set; }
        public string Col_Format_Type { get; set; }
        public string Col_Resp_Type { get; set; }
        public string Col_AgyCode { get; set; }
        public string Active { get; set; }
        public string Act_Col_Name { get; set; }
        public string Col_Code_Length { get; set; }

        #endregion

    }

    public class ControlCard_Entity
    {
        #region Constructors

        public ControlCard_Entity()
        {
            Rowtype = string.Empty;
            Scr_Code = string.Empty;
            UserID = string.Empty;
            Card_ID = string.Empty;
            Card_DESC = string.Empty;
            Card_1 = string.Empty;
            Card_2 = string.Empty;
            Card_3 = string.Empty;
            Module = string.Empty;
            ADD_Date = string.Empty;
            LSTC_Date = string.Empty;
            ADHASSOC_Count = string.Empty;
        }

        public ControlCard_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rowtype = null;
                Scr_Code = null;
                UserID = null;
                Card_ID = null;
                Card_DESC = null;
                Card_1 = null;
                Card_2 = null;
                Card_3 = null;
                Module = null;
                ADD_Date = null;
                LSTC_Date = null;
                ADHASSOC_Count = null;
            }
        }


        public ControlCard_Entity(ControlCard_Entity Entity)
        {
            if (Entity != null)
            {
                Rowtype = Entity.Rowtype;
                Scr_Code = Entity.Scr_Code;
                UserID = Entity.UserID;
                Card_ID = Entity.Card_ID;
                Card_DESC = Entity.Card_DESC;
                Card_1 = Entity.Card_1;
                Card_2 = Entity.Card_2;
                Card_3 = Entity.Card_3;
                Module = Entity.Module;
                ADD_Date = Entity.ADD_Date;
                LSTC_Date = Entity.LSTC_Date;
                ADHASSOC_Count = Entity.ADHASSOC_Count;
            }
        }

        public ControlCard_Entity(DataRow row)
        {
            if (row != null)
            {
                Rowtype = "U";
                Scr_Code = row["CCF_PROG_NAME"].ToString().Trim();
                UserID = row["CCF_EMP_CODE"].ToString().Trim();
                Card_ID = row["CCF_ID"].ToString().Trim();
                Card_DESC = row["CCF_DESC"].ToString().Trim();
                Card_1 = row["CCF_CONTROL_CARD_1"].ToString().Trim();
                Card_2 = row["CCF_CONTROL_CARD_2"].ToString().Trim();
                Card_3 = row["CCF_CONTROL_CARD_3"].ToString().Trim();
                Module = row["CCF_MODULE"].ToString().Trim();
                //ADD_Date = row["CCF_DATE_CREATED"].ToString().Trim();
                //LSTC_Date = row["CCF_LAST_UPDATED"].ToString().Trim();
                ADD_Date = row["CCF_DATE_ADD"].ToString().Trim();
                LSTC_Date = row["CCF_DATE_LSTC"].ToString().Trim();

                ADHASSOC_Count = row["ADHASSOC_Count"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string Rowtype { get; set; }
        public string Scr_Code { get; set; }
        public string UserID { get; set; }
        public string Card_ID { get; set; }
        public string Card_DESC { get; set; }
        public string Card_1 { get; set; }
        public string Card_2 { get; set; }
        public string Card_3 { get; set; }
        public string Module { get; set; }
        public string ADD_Date { get; set; }
        public string LSTC_Date { get; set; }
        public string ADHASSOC_Count { get; set; }

        #endregion

    }

    public class AdhocAssoc_Entity
    {
        #region Constructors

        public AdhocAssoc_Entity()
        {
            Rowtype = string.Empty;
            Adh_ID = string.Empty;
            Adh_ID_UserID = string.Empty;
            Adh_Assc_Desc = string.Empty;
            Adh_Assc_UserID = string.Empty;
            Adh_Assc_AddDate = string.Empty;
            Adh_Assc_AddOpr = string.Empty;
            Adh_Assc_Module = string.Empty;
        }

        public AdhocAssoc_Entity(bool Initialize)
        {
            if (Initialize)
            {
                Rowtype = null;
                Adh_ID = null;
                Adh_ID_UserID = null;
                Adh_Assc_Desc = null;
                Adh_Assc_UserID = null;
                Adh_Assc_AddDate = null;
                Adh_Assc_AddOpr = null;
                Adh_Assc_Module = null;
            }
        }


        public AdhocAssoc_Entity(AdhocAssoc_Entity Entity)
        {
            if (Entity != null)
            {
                Rowtype = Entity.Rowtype;
                Adh_ID = Entity.Adh_ID;
                Adh_ID_UserID = Entity.Adh_ID_UserID;
                Adh_Assc_Desc = Entity.Adh_Assc_Desc;
                Adh_Assc_UserID = Entity.Adh_Assc_UserID;
                Adh_Assc_AddDate = Entity.Adh_Assc_AddDate;
                Adh_Assc_AddOpr = Entity.Adh_Assc_AddOpr;
                Adh_Assc_Module = Entity.Adh_Assc_Module;
            }
        }

        public AdhocAssoc_Entity(DataRow row)
        {
            if (row != null)
            {
                Rowtype = "U";
                Adh_ID = row["ADHASSOC_ID"].ToString().Trim();
                Adh_ID_UserID = row["ADHASSOC_ID_USER"].ToString().Trim();
                Adh_Assc_Desc = row["ADHASSOC_DESC"].ToString().Trim();
                Adh_Assc_UserID = row["ADHASSOC_USER"].ToString().Trim();
                Adh_Assc_AddDate = row["ADHASSOC_ADD_DATE"].ToString().Trim();
                Adh_Assc_AddOpr = row["ADHASSOC_ADD_OPERATOR"].ToString().Trim();
                Adh_Assc_Module = row["ADHASSOC_MODULE"].ToString().Trim();
                
            }
        }



        #endregion

        #region Properties

        public string Rowtype { get; set; }
        public string Adh_ID { get; set; }
        public string Adh_ID_UserID { get; set; }
        public string Adh_Assc_Desc { get; set; }
        public string Adh_Assc_UserID { get; set; }
        public string Adh_Assc_AddDate { get; set; }
        public string Adh_Assc_AddOpr { get; set; }
        public string Adh_Assc_Module { get; set; }

        #endregion

    }


    public class DG_ResTab_Entity
    {
        #region Constructors

        public DG_ResTab_Entity()
        {
            Can_Add = string.Empty;
            Column_Name = string.Empty;
            Disp_Name = string.Empty;
            Text_Align = string.Empty;
            Disp_Width = string.Empty;
        }

        public DG_ResTab_Entity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Can_Add = row["name"].ToString();
                Column_Name = row["name"].ToString();
                Disp_Name = row["max_column_id_used"].ToString();
                Text_Align = row["create_date"].ToString();
                Disp_Width = row["create_date"].ToString();

            }
        }

        public DG_ResTab_Entity(string Disp, string Column, string Display, string Align, string Width)
        {

            Can_Add = Disp;
            Column_Name = Column;
            Disp_Name = Display;
            Text_Align = Align;
            Disp_Width = Width;
        }


        #endregion

        #region Properties

        public string Can_Add { get; set; }
        public string Column_Name { get; set; }
        public string Disp_Name { get; set; }
        public string Text_Align { get; set; }
        public string Disp_Width { get; set; }

        #endregion

    }

    public class DG_Browse_Entity
    {
        #region Constructors

        public DG_Browse_Entity()
        {
            Attribute = 
            CA_MS_Sw = 
            Ms_DriveColumn_Sw =
            RepRange=
            Rep_From_Date = 
            Rep_To_Date = 
            Rep_Period_FDate = 
            Rep_Period_TDate = 
            Stat_Detail = 
            Mst_Secret_Sw = 
            Mst_CaseType_Sw = 
            Mst_Acive_Sw = 
            Mst_Site = 
            Mst_Poverty_Low = 
            Mst_Poverty_High = 
            ZipCode = 
            County = 
            DG_Count_Sw = 
            Hierarchy = 
            Activity_Prog =
            UserName =
            CAMS_Filter =
            CA_Fund_Filter =
            DateType=
            PM_Rep_Format = CaseMssite = IncVerSwitch = alertCodes = string.Empty;
        }

        public DG_Browse_Entity(bool Initialize)
        {
            if (Initialize)
            {
                //EXEC CaseDemographics_SP 'SYSTE', 'MSDATE', '2011-07-01', '2012-10-30', '2011-07-01', '2012-10-30', 'B', '**', 'B', 'LC', 0, 999, '1', '030399', 'SYSTEM'
                Attribute = "SYSTEM";
                CA_MS_Sw = "MS";
                Ms_DriveColumn_Sw = "MSDATE";
                RepRange = "B";
                Rep_From_Date = DateTime.Today.ToShortDateString();
                Rep_To_Date = DateTime.Today.ToShortDateString();
                Rep_Period_FDate = DateTime.Today.ToShortDateString();
                Rep_Period_TDate = DateTime.Today.ToShortDateString();
                Stat_Detail = "N";
                Mst_Secret_Sw = "B";
                Mst_CaseType_Sw = "**";
                Mst_Acive_Sw = "B";
                Mst_Site = "**";
                Mst_Poverty_Low = "0";
                Mst_Poverty_High = "999";
                ZipCode = "**";
                County = "**";
                DG_Count_Sw = "OBO";
                Hierarchy = "******";
                Activity_Prog = "******";
                UserName = "SYSTEM";
                CAMS_Filter = "**";
                CA_Fund_Filter = "**";
                PM_Rep_Format = "PM";
                OutComeSwitch = "A";
                CaseMssite = "**";
                IncVerSwitch = "N";
                DateType = "R";

                alertCodes = "**";
            }
        }


        //public DG_Browse_Entity(string Disp, string Column, string Display, string Align, string Width)
        //{
        //    Attribute = Disp;
        //    Ms_DriveColumn_Sw = Disp;
        //    Rep_From_Date = Disp;
        //    Rep_To_Date = Disp;
        //    Rep_Period_FDate = Disp;
        //    Rep_Period_TDate = Disp;
        //    Stat_Detail = Disp;
        //    Mst_Secret_Sw = Disp;
        //    Mst_CaseType_Sw = Disp;
        //    Mst_Acive_Sw = Disp;
        //    Mst_Site = Disp;
        //    Mst_Poverty_Low = Disp;
        //    Mst_Poverty_High = Disp;
        //    DG_Count_Sw = Disp;
        //    Hierarchy = Disp;
        //    UserName = Disp;
        //}


        #endregion

        #region Properties

        public string Attribute { get; set; }
        public string CA_MS_Sw { get; set; }
        public string Ms_DriveColumn_Sw { get; set; }
        public string RepRange { get; set; }
        public string Rep_From_Date { get; set; }
        public string Rep_To_Date { get; set; }
        public string Rep_Period_FDate { get; set; }
        public string Rep_Period_TDate { get; set; }
        public string Stat_Detail { get; set; }
        public string Mst_Secret_Sw { get; set; }
        public string Mst_CaseType_Sw { get; set; }
        public string Mst_Acive_Sw { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_Poverty_Low { get; set; }
        public string Mst_Poverty_High { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string DG_Count_Sw { get; set; }
        public string Hierarchy { get; set; }
        public string Activity_Prog { get; set; }
        public string UserName { get; set; }
        public string CAMS_Filter { get; set; }
        public string CA_Fund_Filter { get; set; }
        public string PM_Rep_Format { get; set; }
        public string RngMainCode { get; set; }
        public string OutComeSwitch { get; set; }
        public string CaseMssite { get; set; }
        public string IncVerSwitch { get; set; }

        public string DateType { get; set; }

        public string alertCodes
        {
            get; set;
        }
        #endregion

    }

    public class PM_Browse_Entity
    {
        #region Constructors

        public PM_Browse_Entity()
        {
            Category = string.Empty;
            Ms_DriveColumn_Sw = string.Empty;
            Ref_From_Date = string.Empty;
            Ref_To_Date = string.Empty;
            Rep_Period_FDate = string.Empty;
            Rep_Period_TDate = string.Empty;
            Stat_Detail = string.Empty;
            Mst_Secret_Sw = string.Empty;
            Mst_CaseType_Sw = string.Empty;
            Mst_Acive_Sw = string.Empty;
            Mst_Site = string.Empty;
            Mst_Poverty_Low = string.Empty;
            Mst_Poverty_High = string.Empty;
            Groups = string.Empty;
            Rep_Format = string.Empty;
            Hierarchy = string.Empty;
            UserName = string.Empty;
        }

        public PM_Browse_Entity(bool Initialize)
        {
            if (Initialize)
            {
                //EXEC Get_PerMeasures_Counts 'SYSTEM', 'MSDATE', '2011-10-01', '2012-09-30', '2012-01-01', '2012-01-31', 'B', '**', 'B', '**', 0, 999, 'SNP', '**', '010299', 'SYSTEM'
                Category = "SYSTEM";
                Ms_DriveColumn_Sw = "MSDATE";
                Ref_From_Date = DateTime.Today.ToShortDateString();
                Ref_To_Date = DateTime.Today.ToShortDateString();
                Rep_Period_FDate = DateTime.Today.ToShortDateString();
                Rep_Period_TDate = DateTime.Today.ToShortDateString();
                Stat_Detail = "N";
                Mst_Secret_Sw = "B";
                Mst_CaseType_Sw = "**";
                Mst_Acive_Sw = "B";
                Mst_Site = "**";
                Mst_Poverty_Low = "0";
                Mst_Poverty_High = "999";
                Groups = "**";
                Rep_Format = "OBO";
                Hierarchy = "******";
                UserName = "SYSTEM";
            }
        }


        //public PM_Browse_Entity(string Disp, string Column, string Display, string Align, string Width)
        //{
        //    Attribute = Disp;
        //    Ms_DriveColumn_Sw = Disp;
        //    Rep_From_Date = Disp;
        //    Rep_To_Date = Disp;
        //    Rep_Period_FDate = Disp;
        //    Rep_Period_TDate = Disp;
        //    Stat_Detail = Disp;
        //    Mst_Secret_Sw = Disp;
        //    Mst_CaseType_Sw = Disp;
        //    Mst_Acive_Sw = Disp;
        //    Mst_Site = Disp;
        //    Mst_Poverty_Low = Disp;
        //    Mst_Poverty_High = Disp;
        //    DG_Count_Sw = Disp;
        //    Hierarchy = Disp;
        //    UserName = Disp;
        //}


        #endregion

        #region Properties

        public string Category { get; set; }
        public string Ms_DriveColumn_Sw { get; set; }
        public string Ref_From_Date { get; set; }
        public string Ref_To_Date { get; set; }
        public string Rep_Period_FDate { get; set; }
        public string Rep_Period_TDate { get; set; }
        public string Stat_Detail { get; set; }
        public string Mst_Secret_Sw { get; set; }
        public string Mst_CaseType_Sw { get; set; }
        public string Mst_Acive_Sw { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_Poverty_Low { get; set; }
        public string Mst_Poverty_High { get; set; }
        public string Groups { get; set; }
        public string Rep_Format { get; set; }
        public string Hierarchy { get; set; }
        public string UserName { get; set; }
        #endregion

    }

    public class DG_Bypass_Entity
    {
        #region Constructors

        public DG_Bypass_Entity()
        {
            Can_Add = string.Empty;
            Column_Name = string.Empty;
            Disp_Name = string.Empty;
            Text_Align = string.Empty;
            Disp_Width = string.Empty;
        }

        public DG_Bypass_Entity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Can_Add = row["name"].ToString();
                Column_Name = row["name"].ToString();
                Disp_Name = row["max_column_id_used"].ToString();
                Text_Align = row["create_date"].ToString();
                Disp_Width = row["create_date"].ToString();

            }
        }

        public DG_Bypass_Entity(string Disp, string Column, string Display, string Align, string Width)
        {

            Can_Add = Disp;
            Column_Name = Column;
            Disp_Name = Display;
            Text_Align = Align;
            Disp_Width = Width;
        }


        #endregion

        #region Properties

        public string Can_Add { get; set; }
        public string Column_Name { get; set; }
        public string Disp_Name { get; set; }
        public string Text_Align { get; set; }
        public string Disp_Width { get; set; }

        #endregion

    }


    public class DG_SNP_Bypass_Entity
    {
        #region Constructors

        public DG_SNP_Bypass_Entity()
        {
            Can_Add = string.Empty;
            Column_Name = string.Empty;
            Disp_Name = string.Empty;
            Text_Align = string.Empty;
            Disp_Width = string.Empty;
        }

        public DG_SNP_Bypass_Entity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Can_Add = row["name"].ToString();
                Column_Name = row["name"].ToString();
                Disp_Name = row["max_column_id_used"].ToString();
                Text_Align = row["create_date"].ToString();
                Disp_Width = row["create_date"].ToString();

            }
        }

        public DG_SNP_Bypass_Entity(string Disp, string Column, string Display, string Align, string Width)
        {

            Can_Add = Disp;
            Column_Name = Column;
            Disp_Name = Display;
            Text_Align = Align;
            Disp_Width = Width;
        }


        #endregion

        #region Properties

        public string Can_Add { get; set; }
        public string Column_Name { get; set; }
        public string Disp_Name { get; set; }
        public string Text_Align { get; set; }
        public string Disp_Width { get; set; }

        #endregion

    }

    public class Adhoc_ADDCUSTEntity
    {
        #region Constructors

        public Adhoc_ADDCUSTEntity()
        {
            Act_Cust_Code = string.Empty;
            Cust_Desc = string.Empty;
            Cust_Resp_Type = string.Empty;
        }

        public Adhoc_ADDCUSTEntity(bool Intialize)
        {
            if (Intialize)
            {
                Act_Cust_Code = null;
                Cust_Desc = null;
                Cust_Resp_Type = null;
            }
        }

        public Adhoc_ADDCUSTEntity(DataRow CustResponse)
        {
            if (CustResponse != null)
            {
                Act_Cust_Code = CustResponse["ACT_CODE"].ToString();
                Cust_Desc = CustResponse["CUST_DESC"].ToString();
                Cust_Resp_Type = CustResponse["CUST_RESP_TYPE"].ToString();
            }

        }

        //Added by Sudheer on 01/07/2016
        public Adhoc_ADDCUSTEntity(DataRow CustResponse,string data)
        {
            if (CustResponse != null)
            {
                if (data == "SERCUST")
                {
                    Act_Cust_Code = CustResponse["SER_CODE"].ToString();
                    Cust_Desc = CustResponse["CUST_DESC"].ToString();
                    Cust_Resp_Type = CustResponse["CUST_RESP_TYPE"].ToString();
                }
                else
                {
                    Act_Cust_Code = CustResponse["PRES_CODE"].ToString();
                    Cust_Desc = CustResponse["CUST_DESC"].ToString();
                    Cust_Resp_Type = CustResponse["CUST_RESP_TYPE"].ToString();
                }
            }

        }

        #endregion

        #region Properties

        public string Act_Cust_Code { get; set; }
        public string Cust_Desc { get; set; }
        public string Cust_Resp_Type { get; set; }

        #endregion

    }
    public class SO_Browse_Entity
    {
        #region Constructors

        public SO_Browse_Entity()
        {
            Attribute =
            CA_MS_Sw =
            Ms_DriveColumn_Sw =
            RepRange =
            Rep_From_Date =
            Rep_To_Date =
            Rep_Period_FDate =
            Rep_Period_TDate =
            Stat_Detail =
            Mst_Secret_Sw =
            Mst_CaseType_Sw =
            Mst_Acive_Sw =
            Mst_Site =
            Mst_Poverty_Low =
            Mst_Poverty_High =
            ZipCode =
            County =
            DG_Count_Sw =
            Hierarchy =
            Activity_Prog =
            UserName =
            CAMS_Filter =
            CA_Fund_Filter =
            DateType =
            PM_Rep_Format = CaseMssite = IncVerSwitch = Results = string.Empty;
        }

        public SO_Browse_Entity(bool Initialize)
        {
            if (Initialize)
            {
                //EXEC CaseDemographics_SP 'SYSTE', 'MSDATE', '2011-07-01', '2012-10-30', '2011-07-01', '2012-10-30', 'B', '**', 'B', 'LC', 0, 999, '1', '030399', 'SYSTEM'
                Attribute = "SYSTEM";
                CA_MS_Sw = "MS";
                Ms_DriveColumn_Sw = "MSDATE";
                RepRange = "B";
                Rep_From_Date = DateTime.Today.ToShortDateString();
                Rep_To_Date = DateTime.Today.ToShortDateString();
                Rep_Period_FDate = DateTime.Today.ToShortDateString();
                Rep_Period_TDate = DateTime.Today.ToShortDateString();
                Stat_Detail = "N";
                Mst_Secret_Sw = "B";
                Mst_CaseType_Sw = "**";
                Mst_Acive_Sw = "B";
                Mst_Site = "**";
                Mst_Poverty_Low = "0";
                Mst_Poverty_High = "999";
                ZipCode = "**";
                County = "**";
                DG_Count_Sw = "OBO";
                Hierarchy = "******";
                Activity_Prog = "******";
                UserName = "SYSTEM";
                CAMS_Filter = "**";
                CA_Fund_Filter = "**";
                PM_Rep_Format = "PM";
                OutComeSwitch = "A";
                CaseMssite = "**";
                IncVerSwitch = "N";
                DateType = "R";

                Results = "A";
            }
        }


        //public DG_Browse_Entity(string Disp, string Column, string Display, string Align, string Width)
        //{
        //    Attribute = Disp;
        //    Ms_DriveColumn_Sw = Disp;
        //    Rep_From_Date = Disp;
        //    Rep_To_Date = Disp;
        //    Rep_Period_FDate = Disp;
        //    Rep_Period_TDate = Disp;
        //    Stat_Detail = Disp;
        //    Mst_Secret_Sw = Disp;
        //    Mst_CaseType_Sw = Disp;
        //    Mst_Acive_Sw = Disp;
        //    Mst_Site = Disp;
        //    Mst_Poverty_Low = Disp;
        //    Mst_Poverty_High = Disp;
        //    DG_Count_Sw = Disp;
        //    Hierarchy = Disp;
        //    UserName = Disp;
        //}


        #endregion

        #region Properties

        public string Attribute { get; set; }
        public string CA_MS_Sw { get; set; }
        public string Ms_DriveColumn_Sw { get; set; }
        public string RepRange { get; set; }
        public string Rep_From_Date { get; set; }
        public string Rep_To_Date { get; set; }
        public string Rep_Period_FDate { get; set; }
        public string Rep_Period_TDate { get; set; }
        public string Stat_Detail { get; set; }
        public string Mst_Secret_Sw { get; set; }
        public string Mst_CaseType_Sw { get; set; }
        public string Mst_Acive_Sw { get; set; }
        public string Mst_Site { get; set; }
        public string Mst_Poverty_Low { get; set; }
        public string Mst_Poverty_High { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string DG_Count_Sw { get; set; }
        public string Hierarchy { get; set; }
        public string Activity_Prog { get; set; }
        public string UserName { get; set; }
        public string CAMS_Filter { get; set; }
        public string CA_Fund_Filter { get; set; }
        public string PM_Rep_Format { get; set; }
        public string RngMainCode { get; set; }
        public string OutComeSwitch { get; set; }
        public string CaseMssite { get; set; }
        public string IncVerSwitch { get; set; }

        public string Q1_START_DATE { get; set; }
        public string Q1_END_DATE { get; set; }
        public string Q2_START_DATE { get; set; }
        public string Q2_END_DATE { get; set; }
        public string Q3_START_DATE { get; set; }
        public string Q3_END_DATE { get; set; }
        public string Q4_START_DATE { get; set; }
        public string Q4_END_DATE { get; set; }

        public string DateType { get; set; }

        public string Results
        {
            get; set;
        }
        #endregion

    }

    public class Headstart_Template
    {
        #region Constructors

        public Headstart_Template()
        {
            Tabs_Type = string.Empty;
            Table_Code = string.Empty;
            Code_Desc = string.Empty;
            Code_Desc_Tag = string.Empty;
            Agy_2 = string.Empty;
            AGY_3 = string.Empty;

        }

        public Headstart_Template(bool Initialize)
        {
            if (Initialize)
            {
                Tabs_Type = null;
                Table_Code = null;
                Code_Desc = null;
                Code_Desc_Tag = null;
                Agy_2 = null;
                AGY_3 = null;
            }
        }


        public Headstart_Template(Headstart_Template Entity)
        {
            if (Entity != null)
            {
                Tabs_Type = Entity.Tabs_Type;
                Table_Code = Entity.Table_Code;
                Code_Desc = Entity.Code_Desc;
                Code_Desc_Tag = Entity.Code_Desc_Tag;
                Agy_2 = Entity.Agy_2;
                AGY_3 = Entity.AGY_3;
            }
        }

        public Headstart_Template(DataRow row)
        {
            if (row != null)
            {
                Tabs_Type = row["AGY_TYPE"].ToString().Trim();
                Table_Code = row["AGY_CODE"].ToString().Trim();
                Code_Desc = row["AGY_DESC"].ToString().Trim();
                Code_Desc_Tag = row["AGY_1"].ToString().Trim();
                Agy_2 = row["AGY_2"].ToString().Trim();
                AGY_3 = row["AGY_3"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string Tabs_Type { get; set; }
        public string Table_Code { get; set; }
        public string Code_Desc { get; set; }
        public string Code_Desc_Tag { get; set; }
        public string Agy_2 { get; set; }
        public string AGY_3 { get; set; }

        #endregion

    }

    public class PM_ResTab_Entity
    {
        #region Constructors

        public PM_ResTab_Entity()
        {
            Group_Code = string.Empty;
            Table_Code = string.Empty;
            Table_Desc = string.Empty;
            Unit_Count = string.Empty;
            Exp_To_Achive = string.Empty;
            Res_Head1 = string.Empty;
            Res_Head2 = string.Empty;
            Res_Head3 = string.Empty;
            Res_Head4 = string.Empty;
            Res_Head5 = string.Empty;
            Achiv_Percentage = string.Empty;
            Cost = string.Empty;
            Count_Type = string.Empty;
            Row_Type = string.Empty;
        }

        public PM_ResTab_Entity(bool initialize)
        {
            if (initialize)
            {
                Group_Code = null;
                Table_Code = null;
                Table_Desc = null;
                Unit_Count = null;
                Exp_To_Achive = null;
                Res_Head1 = null;
                Res_Head2 = null;
                Res_Head3 = null;
                Res_Head4 = null;
                Res_Head5 = null;
                Achiv_Percentage = null;
                Cost = null;
                Count_Type = null;
                Row_Type = null;
            }
        }

        public PM_ResTab_Entity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Group_Code = row["Res_Group"].ToString();
                Table_Code = row["Res_Table"].ToString();
                Table_Desc = row["Res_Table_Desc"].ToString();
                Unit_Count = row["Res_Unit_Cnt"].ToString();
                Exp_To_Achive = row["Res_Exp_To_Achive"].ToString();
                Res_Head1 = row["Res_Hed1_Cnt"].ToString();
                Res_Head2 = row["Res_Hed2_Cnt"].ToString();
                Res_Head3 = row["Res_Hed3_Cnt"].ToString();
                Res_Head4 = row["Res_Hed4_Cnt"].ToString();
                Res_Head5 = row["Res_Hed5_Cnt"].ToString();
                Achiv_Percentage = row["Res_Per_Achived"].ToString();
                Cost = row["Res_Cost"].ToString();
                Count_Type = row["Res_Count_Type"].ToString();
                Row_Type = row["Res_Row_Type"].ToString();

            }
        }

        public PM_ResTab_Entity(PM_ResTab_Entity Entity)
        {

            Group_Code = Entity.Group_Code;
            Table_Code = Entity.Table_Code;
            Table_Desc = Entity.Table_Desc;
            Unit_Count = Entity.Unit_Count;
            Exp_To_Achive = Entity.Exp_To_Achive;
            Res_Head1 = Entity.Res_Head1;
            Res_Head2 = Entity.Res_Head2;
            Res_Head3 = Entity.Res_Head3;
            Res_Head4 = Entity.Res_Head4;
            Res_Head5 = Entity.Res_Head5;
            Achiv_Percentage = Entity.Achiv_Percentage;
            Cost = Entity.Cost;
            Count_Type = Entity.Count_Type;
            Row_Type = Entity.Row_Type;
        }


        #endregion

        #region Properties

        public string Group_Code { get; set; }
        public string Table_Code { get; set; }
        public string Table_Desc { get; set; }
        public string Unit_Count { get; set; }
        public string Exp_To_Achive { get; set; }
        public string Res_Head1 { get; set; }
        public string Res_Head2 { get; set; }
        public string Res_Head3 { get; set; }
        public string Res_Head4 { get; set; }
        public string Res_Head5 { get; set; }
        public string Achiv_Percentage { get; set; }
        public string Cost { get; set; }
        public string Count_Type { get; set; }
        public string Row_Type { get; set; }

        #endregion
    }

    public class ReportLogEntity
    {
        #region Constructors

        public ReportLogEntity()
        {
            REP_ID = string.Empty;
            REP_PROG_NAME = string.Empty;
            REP_EMP_CODE = string.Empty;
            REP_DATA = string.Empty;
            REP_FILE_NAME = string.Empty;
            REP_DATE_ADD = string.Empty;
            REP_MODULE_CODE = string.Empty;
        }




        public ReportLogEntity(ReportLogEntity Entity)
        {
            if (Entity != null)
            {
                REP_ID = Entity.REP_ID;
                REP_PROG_NAME = Entity.REP_PROG_NAME;
                REP_EMP_CODE = Entity.REP_EMP_CODE;
                REP_DATA = Entity.REP_DATA;
                REP_FILE_NAME = Entity.REP_FILE_NAME;
                REP_MODULE_CODE = Entity.REP_MODULE_CODE;
                REP_DATE_ADD = Entity.REP_DATE_ADD;               
            }
        }

        public ReportLogEntity(DataRow row)
        {
            if (row != null)
            {
                REP_ID = row["REP_ID"].ToString();
                REP_PROG_NAME = row["REP_PROG_NAME"].ToString().Trim();
                REP_EMP_CODE = row["REP_EMP_CODE"].ToString().Trim();
                REP_DATA = row["REP_DATA"].ToString().Trim();
                if (REP_DATA != string.Empty)
                {
                    Rep_Table = CommonFunctions.Convert_XMLstring_To_Datatable(REP_DATA);
                }
                REP_FILE_NAME = row["REP_FILE_NAME"].ToString().Trim();
                REP_MODULE_CODE = row["REP_MODULE_CODE"].ToString().Trim();
                REP_DATE_ADD = row["REP_DATE_ADD"].ToString().Trim();
               
            }
        }



        #endregion

        #region Properties

        public string REP_ID { get; set; }
        public string REP_PROG_NAME { get; set; }
        public string REP_EMP_CODE { get; set; }
        public string REP_DATA { get; set; }
        public string REP_FILE_NAME { get; set; }
        public string REP_MODULE_CODE { get; set; }
        public string REP_DATE_ADD { get; set; }
        public DataTable Rep_Table { get; set; }
       
        #endregion

    }

    public class Client_InqTotal_entity
    {
        #region Constructors

        public Client_InqTotal_entity()
        {
            Sno = 
            Total_Desc = 
            Total_Cnt = string.Empty;
        }

        public Client_InqTotal_entity(bool Intialize)
        {
            if (Intialize)
            {
                Sno = 
                Total_Desc = 
                Total_Cnt = null;
            }
        }

        public Client_InqTotal_entity(DataRow CustResponse)
        {
            if (CustResponse != null)
            {
                Sno = CustResponse["Sno"].ToString();
                Total_Desc = CustResponse["Description"].ToString();
                Total_Cnt = CustResponse["Counts"].ToString();
            }

        }

        #endregion

        #region Properties

        public string Sno { get; set; }
        public string Total_Desc { get; set; }
        public string Total_Cnt { get; set; }

        #endregion

    }


}
