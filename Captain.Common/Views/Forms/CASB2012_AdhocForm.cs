#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using System.Xml;
using System.IO;
using System.Threading;
using Wisej.Web;
using DevExpress.XtraBars.Docking.Paint;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB2012_AdhocForm : _iForm
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        double total_Columns_Width = 0.2;
        int total_Columns_Selected_2Display = 0;
        public CASB2012_AdhocForm(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();

            ReportPath = _model.lookupDataAccess.GetReportPath();
            BaseForm = baseForm;
            Privileges = privileges;
            ColumnGridSwitch = string.Empty;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Fill_Dropdowns();

            //this.Size = new System.Drawing.Size(789, 550);
            //this.Text = privileges.Program + " - " + privileges.PrivilegeName;
            this.Text =  privileges.PrivilegeName;

            if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B" || BaseForm.UserProfile.Security == "C")
                Btn_Associations.Visible = true;

            Get_Rank_Table();
            Get_SelDataBase_Tables_Columns();
            //Get_Master_Table_Details();
            Fill_Categories_Combo();
            Get_Master_Table_Details();
            Get_Master_Columns_Details();
            Fill_Tables_Grid();
            ColumnGridSwitch = "1";
            Fill_Columns_Grid();
            Fill_AgyTabs_List();
            Get_Case2001_Cust_List();
            Get_Preasses_Cust_List();
            Get_Sercust_Cust_List();
            Get_CASEHIE_list();
            Get_Case0061_Cust_List();

            if (privileges.ModuleName == "UserReportMaintenance")
                Fill_Userefined_Criteria();

            //FillFuelcntlList();

            //this.panel2.Location = new System.Drawing.Point(-1, -1);
            //this.panel2.Size = new System.Drawing.Size(784, 77);


            //this.panel3.Location = new System.Drawing.Point(-1, 75);
            //this.panel3.Size = new System.Drawing.Size(784, 387);


            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Pb_Edit, "Edit Criteria Selection");
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string ReportPath { get; set; }

        public string Calling_UserID { get; set; }

        public string ColumnGridSwitch { get; set; }

        public string CategoryCode { get; set; }

        #endregion


        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;

        private void Fill_Dropdowns()
        {
            Cmb_Applications.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Non-Secret Only", "1"));
            listItem.Add(new ListItem("Secret Only", "2"));
            listItem.Add(new ListItem("Both Secret & Non-Secret", "3"));
            Cmb_Applications.Items.AddRange(listItem.ToArray());

            Cmb_Group_Sort.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Ascending", "ASC"));
            listItem.Add(new ListItem("Descending", "DESC"));
            Cmb_Group_Sort.Items.AddRange(listItem.ToArray());

            Cmb_Dat_Filter.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("Process All", "*"));
            listItem.Add(new ListItem("Bypass Dup. Clients", "C"));
            listItem.Add(new ListItem("Bypass Dup. Families", "F"));
            Cmb_Dat_Filter.Items.AddRange(listItem.ToArray());


            Cmb_Applications.SelectedIndex = Cmb_Group_Sort.SelectedIndex = Cmb_Dat_Filter.SelectedIndex = 0;
        }


        private void Fill_Userefined_Criteria()
        {
            List<ControlCard_Entity> Card_List = new List<ControlCard_Entity>();
            ControlCard_Entity Card_Entity = new ControlCard_Entity(true);
            Card_Entity.Scr_Code = "CASB0012";
            Card_Entity.Card_ID = Privileges.ViewPriv;
            Card_Entity.UserID = Privileges.LSTCOperator;
            Card_Entity.Module = Privileges.ModuleCode;
            Card_List = _model.AdhocData.Browse_CONTROLCARD(Card_Entity, "Browse");

            if (Card_List.Count > 0)
            {
                DataTable RepCntl_Table = new DataTable();
                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Card_List[0].Card_1);
                Set_Report_Controls(RepCntl_Table);

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Card_List[0].Card_2);
                Set_Criteria_SelCol_List(RepCntl_Table);

                lblAssoc.Visible = true;
                lblAssoc.Text = "Associated by: "+ Card_List[0].UserID.Trim();
            }
            else lblAssoc.Visible = false;

        }

        List<RankCatgEntity> Ranksgrid = new List<RankCatgEntity>();
        private void Get_Rank_Table()
        {
            Ranksgrid = _model.SPAdminData.Browse_RankCtg();
        }

        private string Get_Rank_Desc_4rm_RankCat_Table(string Rank_Code, string Old_Name)
        {
            List<RankCatgEntity> rankCatg = Ranksgrid.FindAll(u => u.Agency.Trim() == Current_Hierarchy.Substring(0, 2) && u.SubCode.Trim() == string.Empty);
            string stragy = Current_Hierarchy.Substring(0, 2);
            if (rankCatg.Count == 0)
            {
                rankCatg = Ranksgrid.FindAll(u => u.Agency.Trim() == "**" && u.SubCode.Trim() == string.Empty);
                stragy = "**";
            }
            string Rank_Desc = Old_Name;
            foreach (RankCatgEntity Ent in rankCatg)
            {
                if (Rank_Code == Ent.Code && stragy == Ent.Agency)
                {
                    Rank_Desc = Ent.Desc.Trim();
                    break;
                }
            }
            return Rank_Desc;
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(890, 25);

            CategoryCode = string.Empty; pnlSP.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y" && Agy!="**" && Dept!="**" && Prog!="**")
            {
                DataSet dsser = Captain.DatabaseLayer.HierarchyPlusProgram.GetSerplanHiesadp(BaseForm.UserID, Agy, Dept, Prog);
                if (dsser.Tables.Count > 0)
                {
                    DataTable dt = dsser.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        var distinctRows = (from DataRow dRow in dt.Rows
                                            where dRow.Field<string>("DEP_INTAKE_PROG") =="Y" 
                                            select dRow["DEP_SERPOST_PAYCAT"]).Distinct();

                        if (distinctRows.ToList().Count > 1)
                        {
                            pnlSP.Visible = true;
                            this.Txt_HieDesc.Size = new System.Drawing.Size(641, 25);
                            FillServicePlanHieCombo(dt);

                        }
                        else
                            CategoryCode = dt.Rows[0]["DEP_SERPOST_PAYCAT"].ToString();
                        //else
                        //    pnlSP.Visible = false;

                    }

                }
                else
                {
                    //pnlSP.Visible = false;
                    CategoryCode = string.Empty;
                }
            }
            else
            {
                List<Agy_Ext_Entity> PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00201", "6", "2", null, null);
                ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(Agy, Dept, Prog);
                
                if (programEntity != null)
                {
                    CategoryCode = programEntity.DepSerpostPAYCAT.Trim();
                }

                if (string.IsNullOrEmpty(CategoryCode.Trim()))
                {
                    if (PPC_List.Count > 0)
                    {
                        PPC_List = PPC_List.FindAll(u => u.Ext_1 != "");
                        if (PPC_List.Count > 0)
                        {
                            foreach (Agy_Ext_Entity Entity in PPC_List)
                            {
                                if (!string.IsNullOrEmpty(Entity.Ext_1.Trim()))
                                {
                                    if (Agy == Entity.Ext_1.Substring(0, 2))
                                    {
                                        CategoryCode = Entity.Ext_2.Trim();
                                    }
                                }
                            }
                        }

                    }
                }
            }

            //if(pnlSP.Visible==false)
            //    this.Txt_HieDesc.Size = new System.Drawing.Size(890, 25);

            Fill_Columns_Grid();

            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                if (dr.Cells["Sel_Col_DispName"].Value.ToString() == "Year")
                {
                    dr.Cells["Sel_Equalto"].Value = (!string.IsNullOrEmpty(Program_Year.Trim()) ? Program_Year : " ");
                    string toolTipText = "Equal to : " + (!string.IsNullOrEmpty(Program_Year.Trim()) ? Program_Year : string.Empty);

                    foreach (DataGridViewCell cell in dr.Cells)
                        cell.ToolTipText = toolTipText;

                    dr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            get_cont_cust_questions();
        }


        private void FillServicePlanHieCombo(DataTable dt)
        {
            cmbSP.Items.Clear();
            if(dt.Rows.Count>0)
            {
                List<ListItem> listItem = new List<ListItem>();
                foreach (DataRow dr in dt.Rows)
                {
                    listItem.Add(new ListItem(dr["Hierarchy"].ToString(), dr["DEP_SERPOST_PAYCAT"].ToString()));
                }
                cmbSP.Items.AddRange(listItem.ToArray());

                cmbSP.SelectedIndex = 0;
            }
        }



        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                        {
                            TempCompareYear = int.Parse(Year);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                            {
                                TempCompareYear = int.Parse(Program_Year);
                                DefHieExist = true;
                            }

                        }
                        List<ListItem> listItem = new List<ListItem>();
                        listItem.Add(new ListItem("", "10"));
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex + 1;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            if (pnlSP.Visible == true)
            {
                if (!string.IsNullOrEmpty(Program_Year.Trim()))
                    this.Txt_HieDesc.Size = new System.Drawing.Size(571, 25);
                else
                    this.Txt_HieDesc.Size = new System.Drawing.Size(641, 25);
            }
            else
            {
                if (!string.IsNullOrEmpty(Program_Year.Trim()))
                    this.Txt_HieDesc.Size = new System.Drawing.Size(820, 25);
                else
                    this.Txt_HieDesc.Size = new System.Drawing.Size(890, 25);
            }
        }


        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }
        }


        List<AdhocTableEntity> Table_List = new List<AdhocTableEntity>();
        List<AdhocColumnEntity> Column_List = new List<AdhocColumnEntity>();
        //AdhocColumnEntity Column_List_1 = new AdhocColumnEntity();
        private void Get_SelDataBase_Tables_Columns()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.ConnectionString = BaseForm.DataBaseConnectionString;

            //string server = builder.DataSource;
            //string database = builder.InitialCatalog;

            //Table_List = _model.AdhocData.Get_Tables_List("COI", "GetTables");            // Run at Server
            //Column_List = _model.AdhocData.Get_Columns_List("COI", "GetColumns");

            Table_List = _model.AdhocData.Get_Tables_List(builder.InitialCatalog, "GetTables");            // Run at Server
            Column_List = _model.AdhocData.Get_Columns_List(builder.InitialCatalog, "GetColumns");

            //Table_List = _model.AdhocData.Get_Tables_List("COI_08172012", "GetTables");     // Run at Local System
            //Column_List = _model.AdhocData.Get_Columns_List("COI_08172012", "GetColumns");
        }


        List<ADHOCFLDEntity> Master_Columns_List = new List<ADHOCFLDEntity>();
        private void Get_Master_Columns_Details()
        {
            ADHOCFLDEntity Search_ADHOCFLD = new ADHOCFLDEntity(true);
            Master_Columns_List = _model.AdhocData.Browse_ADHOCFLD(Search_ADHOCFLD, BaseForm.BusinessModuleID);
        }


        private void Fill_Categories_Combo()
        {
            List<ADHOCCTGEntity> Adhoc_Categories_List = new List<ADHOCCTGEntity>();
            ADHOCCTGEntity Search_ADHOCCTG = new ADHOCCTGEntity(true);
            Search_ADHOCCTG.Module = Privileges.ModuleCode;
            Adhoc_Categories_List = _model.AdhocData.Browse_ADHOCCTG(Search_ADHOCCTG);

            List<ListItem> listItem = new List<ListItem>();
            foreach (ADHOCCTGEntity Entity in Adhoc_Categories_List)
                listItem.Add(new ListItem(Entity.Catg_Code + " - " + Entity.Catg_Desc, Entity.Catg_Code));


            Cmb_Category.Items.Clear();
            Cmb_Category.Items.AddRange(listItem.ToArray());
            Cmb_Category.SelectedIndexChanged -= Cmb_Category_SelectedIndexChanged;
            if (Cmb_Category.Items.Count > 0)
                Cmb_Category.SelectedIndex = 0;

            Cmb_Category.SelectedIndexChanged += Cmb_Category_SelectedIndexChanged;
        }

        private void Fill_Tables_Grid()
        {
            this.Table_Grid.SelectionChanged -= new System.EventHandler(this.Table_Grid_SelectionChanged);
            Table_Grid.Rows.Clear();
            Column_Grid.Rows.Clear();

            if (Table_List.Count > 0)
            {
                string Tmp_Table_ID = "0";
                string Tmp_TableDesc = " ";
                bool Table_Found = false;
                int Tmp_Rows_Count = 0;
                foreach (AdhocTableEntity Entity in Table_List)
                {
                    Tmp_TableDesc = " "; Tmp_Table_ID = "0"; Table_Found = false;
                    //switch (Entity.Table_name)
                    //{
                    //    case "CASESNP":    Tmp_Table_ID = "2"; break;
                    //    case "CASEINCOME": Tmp_Table_ID = "3"; break;
                    //    //case "CASECONT":   Tmp_Table_ID = "4"; break;
                    //}


                    foreach (ADHOCFLSEntity Table in Master_Table_List)
                    {
                        if (Entity.Table_name == Table.Table_Name && Table.Active_Stat == "Y")
                        {
                            Tmp_TableDesc = Table.Table_Desc;
                            Tmp_Table_ID = Table.Table_Code;
                            Table_Found = true;
                            break;
                        }
                    }

                    //if (Entity.Table_name == "CASEMST" || Entity.Table_name == "CASESNP" || Entity.Table_name == "CASEINCOME" )//|| Entity.Table_name == "CASECONT")
                    if (Table_Found)
                    {
                        Table_Grid.Rows.Add(Img_Blank, Entity.Table_name, Tmp_TableDesc, Tmp_Table_ID);
                        Tmp_Rows_Count++;
                    }

                    //if (Entity.Table_name == "ZIPCODE")
                    //    Table_Grid.Rows.Add(Img_Blank, Entity.Table_name);
                }
                Table_Grid.Sort(Table_Grid.Columns[3], ListSortDirection.Ascending);

                if (Tmp_Rows_Count > 0)
                {
                    Table_Grid.CurrentCell = Table_Grid.Rows[0].Cells[1];
                    Table_Grid.Rows[0].Selected = true;
                    Table_Grid_SelectionChanged(Table_Grid, EventArgs.Empty);
                }

            }
            this.Table_Grid.SelectionChanged += new System.EventHandler(this.Table_Grid_SelectionChanged);
            if (Table_Grid.Rows.Count > 0)
                Table_Grid.Rows[0].Selected = true;
        }

        List<AGYTABSEntity> AgyTabs_List = new List<AGYTABSEntity>();
        private void Fill_AgyTabs_List()
        {
            AGYTABSEntity Search_Agytabs = new AGYTABSEntity(true);
            AgyTabs_List = _model.AdhocData.Browse_AGYTABS(Search_Agytabs);            // Run at Server
        }



        private void Fill_Columns_Grid()
        {
            if (ColumnGridSwitch != string.Empty)
            {
                Column_Grid.Rows.Clear();
                if (Column_List != null)
                {

                    if (Column_List.Count > 0)
                    {
                        if (Table_Grid.Rows.Count > 0)
                        {
                            if (Table_Grid.SelectedRows[0].Selected)
                            {
                                string DataType_Desc = null, Max_Length = null, Min_Length = null, Col_Disp_Name = " ", Col_Format_Type, Col_AgyTab_Code,
                                       Can_Count_SW, Can_Criteria_SW;
                                bool Disp_Name_Found = false, Col_Selected_SW = false;

                                string Tmp_table_name = " ", Tmp_DataType = " ", Tmp_Table_D = Table_Grid.CurrentRow.Cells["Table_ID"].Value.ToString().Trim(),
                                    Sel_Table_Name = Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim(); ;
                                int Tmp_Grid_Count = 0;

                                //foreach (AdhocColumnEntity Entity in Column_List)
                                foreach (ADHOCFLDEntity Master in Master_Columns_List)
                                {
                                    if (Master.Col_Code.Substring(0, 2) == Tmp_Table_D)
                                    {
                                        DataType_Desc = Max_Length = Col_Disp_Name = Col_Format_Type = Col_AgyTab_Code = Tmp_table_name = Tmp_DataType = Can_Count_SW = Can_Criteria_SW = " ";
                                        //DataType_Desc = Max_Length = Col_Disp_Name = Col_Format_Type = Col_AgyTab_Code = " ";
                                        Disp_Name_Found = false;

                                        //if (Master.Act_Col_Name == "Name" || Master.Act_Col_Name == "Address" ||
                                        //    Master.Act_Col_Name == "Current_Age" || Master.Act_Col_Name == "Birth_Date_Month")
                                        //    Master.Act_Col_Name = Master.Act_Col_Name;

                                        //switch (Entity.DataType)
                                        //{
                                        //    case "char":
                                        //    case "nvarchar":
                                        //    case "varchar": DataType_Desc = "Text"; 
                                        //                    Max_Length = Entity.string_MaxLen;
                                        //                    break;

                                        //    case "int":
                                        //    case "tinyint":
                                        //    case "smallint":
                                        //    case "numeric":
                                        //    case "decimal": DataType_Desc = "Numeric";
                                        //                    Max_Length = Entity.Numeric_Precision;
                                        //                    break;
                                        //    case "date": 
                                        //    case "datetime": DataType_Desc = "Date"; break;
                                        //    case "time": DataType_Desc = "Time"; break;
                                        //}

                                        Col_Selected_SW = false;

                                        switch ((Master.Col_Code.Substring(0, 2)))
                                        {
                                            case "05": Tmp_table_name = "CASEMST"; break;
                                            case "06": Tmp_table_name = "CASESNP"; break;
                                            case "14": Tmp_table_name = "CASEENRL"; break;
                                            case "15": Tmp_table_name = "CASEENRL"; break;
                                            case "16": Tmp_table_name = "CHLDATTN"; break;
                                            case "11": Tmp_table_name = "CASEMS"; break;
                                            case "10": Tmp_table_name = "CASEACT"; break;
                                        }
                                        Col_Disp_Name = Master.Col_Disp_Name;

                                        if (Master.Col_Code.Substring(0, 2) == "05")
                                        {
                                            if (Col_Disp_Name.Contains("Rank"))
                                                Col_Disp_Name = Get_Rank_Desc_4rm_RankCat_Table(Col_Disp_Name.Substring(5, 1), Col_Disp_Name);
                                        }


                                        //Col_Format_Type = Master.Col_Format_Type;
                                        Col_Format_Type = Master.Col_Resp_Type; // 10092012


                                        if (!string.IsNullOrEmpty(Master.Col_AgyCode))
                                            Col_AgyTab_Code = Master.Col_AgyCode;

                                        Max_Length = "0";
                                        if (!string.IsNullOrEmpty(Master.Col_Desc_Length.Trim()))      // should be based on Code or Description (Col_Code_Length || Col__Desc_Length)
                                            Max_Length = Master.Col_Desc_Length;

                                        Min_Length = "0";
                                        if (!string.IsNullOrEmpty(Master.Col_Code_Length.Trim()))      // should be based on Code or Description (Col_Code_Length || Col__Desc_Length)
                                            Min_Length = Master.Col_Code_Length;

                                        Can_Count_SW = Master.Have_Count;
                                        Can_Criteria_SW = Master.Have_Criteria;

                                        //foreach (ADHOCFLDEntity Master in Master_Columns_List)
                                        if (Master.Active == "A")
                                        {
                                            foreach (AdhocColumnEntity Entity in Column_List)
                                            {
                                                if (Master.Act_Col_Name == Entity.Column_name && Sel_Table_Name == Entity.Table_name && Master.Act_Col_Name != "Address")
                                                {
                                                    Tmp_table_name = Entity.Table_name; //Rao

                                                    //if (Entity.Column_name == )

                                                    switch (Entity.DataType)
                                                    {
                                                        case "char":
                                                        case "nvarchar":
                                                        case "varchar":
                                                            DataType_Desc = "Text";
                                                            //Max_Length = Entity.string_MaxLen;
                                                            break;

                                                        case "int":
                                                        case "tinyint":
                                                        case "smallint":
                                                        case "numeric":
                                                        case "decimal":
                                                            DataType_Desc = "Numeric";
                                                            //Max_Length = Entity.Numeric_Precision;
                                                            break;
                                                        case "date":
                                                        case "datetime": DataType_Desc = "Date"; break;
                                                        case "time": DataType_Desc = "Time"; break;
                                                    }

                                                    if (Entity.Column_name == "CASECONT_TIME" || Entity.Column_name == "TMSAPPT_TIME")
                                                        Entity.DataType = DataType_Desc = "Time";


                                                    Tmp_DataType = Entity.DataType;

                                                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                                                    {
                                                        if (Sel_Col_Entity.Column_Name == Entity.Column_name && Sel_Col_Entity.Col_Catg == Master.Col_Catgeory)
                                                        {
                                                            Col_Selected_SW = true; break;
                                                            //if (string.IsNullOrEmpty(Master.Col_Catgeory.Trim())) { Col_Selected_SW = true; break; }
                                                            //else if (CategoryCode.Trim() != Master.Col_Catgeory.Trim())
                                                            //{ Col_Selected_SW = false; break; }
                                                            //else if (CategoryCode.Trim() == Master.Col_Catgeory.Trim())
                                                            //{ Col_Selected_SW = true; break; }
                                                        }
                                                    }

                                                    Disp_Name_Found = true; break;
                                                }
                                            }
                                        }


                                        if (string.IsNullOrEmpty(DataType_Desc.Trim()))
                                        {
                                            switch (Master.Col_Format_Type)
                                            {
                                                case "A": DataType_Desc = "Text"; break;
                                                case "F":
                                                case "N": DataType_Desc = "Numeric"; break;
                                                case "T": DataType_Desc = "Date"; break;
                                            }

                                        }

                                        foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                                        {
                                            if (Master.Act_Col_Name == Sel_Col_Entity.Column_Name)
                                                switch (Sel_Col_Entity.Column_Name)
                                                {
                                                    case "Name": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "Address": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "Current_Age": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "Birth_Date_Month": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ATTN_Month": Col_Selected_SW = Disp_Name_Found = true; break;

                                                    case "EnrlLumpedSite": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_START_DATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_END_DATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_ATTNSTART_DATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_ATTNEND_DATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_ATTN_DAYS": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "ENRL_STATUS_DATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CASEMSOBO_CLID": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CASEMSOBO_FAM_SEQ": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CASEMSOBO_Name": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_CLID": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_FAM_SEQ": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CASEACTOBO_Name": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_SDISTRICT": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_AMOUNT": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_DESC": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_QUANTITY": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_STATUS": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_COMPDATE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_UNITPRICE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_TRANSUOM": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_RECPINAME": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_GIFT1": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_GIFT2": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_GIFT3": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_GIFTCARD": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_BEDSIZE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_AIRMATTRESS": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_CLOTHSIZE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                    case "CAOBO_SHOESIZE": Col_Selected_SW = Disp_Name_Found = true; break;
                                                }
                                            if (Col_Selected_SW)
                                                break;
                                        }




                                        //////if (Disp_Name_Found && Tmp_table_name == Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim())
                                        //////{
                                        //////    if (Col_Selected_SW)
                                        //////        Column_Grid.Rows.Add(Img_Tick, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "Y", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length);
                                        //////    else
                                        //////        Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length);

                                        //////    Tmp_Grid_Count++;
                                        //////}

                                        if (dt_cont_cust_ques.Rows.Count == 0 && "080042,080046, 080050".Contains(Master.Col_Code))
                                            get_cont_cust_questions();

                                        bool cont_disp_column_flag = true;

                                        if (dt_cont_cust_ques.Rows.Count == 0 && "080042,080046, 080050".Contains(Master.Col_Code))
                                            cont_disp_column_flag = false;

                                        if (dt_cont_cust_ques.Rows.Count > 0 && "080042,080046, 080050".Contains(Master.Col_Code))
                                        {
                                            int loop_cnt = 1; bool col_found = false;
                                            foreach (DataRow cont_dr in dt_cont_cust_ques.Rows)
                                            {
                                                if (cont_dr["FLDH_CODE"].ToString().Contains("C"))
                                                {
                                                    if (Master.Col_Code == "080042" && loop_cnt == 1)
                                                    {
                                                        col_found = true;
                                                    }
                                                    else if (Master.Col_Code == "080046" && loop_cnt == 2)
                                                    {
                                                        col_found = true;
                                                    }
                                                    else if (Master.Col_Code == "080050" && loop_cnt == 3)
                                                    {
                                                        col_found = true;
                                                    }

                                                    if (col_found)
                                                    {
                                                        foreach (CustfldsEntity Ent in Case0061_Cust_List)
                                                        {
                                                            if (cont_dr["FLDH_CODE"].ToString() == Ent.CustCode)
                                                            {
                                                                Col_Disp_Name = Ent.CustDesc; break;
                                                            }
                                                        }

                                                        break;
                                                    }
                                                    loop_cnt++;
                                                }
                                            }

                                            //20160922
                                            //"080042,080046, 080050".Contains(Master.Col_Code)
                                            //if (col_found)
                                            cont_disp_column_flag = col_found;
                                        }

                                        //if (dt_cont_cust_ques.Rows.Count > 0 && "080044,080048, 080052".Contains(Master.Col_Code))
                                        //{
                                        //    int loop_cnt = 1; bool col_found = false; string rtn_ques_code = "";
                                        //    foreach (DataRow cont_dr in dt_cont_cust_ques.Rows)
                                        //    {
                                        //        if (cont_dr["FLDH_CODE"].ToString().Contains("C"))
                                        //        {
                                        //            if (Master.Col_Code == "080044" && loop_cnt == 1)
                                        //            {
                                        //                rtn_ques_code = "080042";
                                        //                col_found = true;
                                        //                break;
                                        //            }
                                        //            else if (Master.Col_Code == "080048" && loop_cnt == 2)
                                        //            {
                                        //                rtn_ques_code = "080046";
                                        //                col_found = true;
                                        //                break;

                                        //            }
                                        //            else if (Master.Col_Code == "080052" && loop_cnt == 3)
                                        //            {
                                        //                rtn_ques_code = "080050";
                                        //                col_found = true;
                                        //                break;
                                        //            }

                                        //            //if (col_found)
                                        //            //{
                                        //            //    rtn_ques_code = cont_dr["FLDH_CODE"].ToString();
                                        //            //    break;
                                        //            //}
                                        //            loop_cnt++;
                                        //        }
                                        //    }

                                        //    col_found = cont_disp_column_flag = false; 
                                        //    if (!string.IsNullOrEmpty(rtn_ques_code))
                                        //    {
                                        //        //int loop_cnt = 1; bool col_found = false;
                                        //        loop_cnt = 1; col_found = false; string tmp_Col_Disp_Name = "";
                                        //        foreach (DataRow cont_dr in dt_cont_cust_ques.Rows)
                                        //        {
                                        //            if (cont_dr["FLDH_CODE"].ToString().Contains("C"))
                                        //            {
                                        //                if (rtn_ques_code == "080042" && loop_cnt == 1)
                                        //                {
                                        //                    col_found = true;
                                        //                    break;
                                        //                }
                                        //                else if (rtn_ques_code == "080046" && loop_cnt == 2)
                                        //                {
                                        //                    col_found = true;
                                        //                    break;
                                        //                }
                                        //                else if (rtn_ques_code == "080050" && loop_cnt == 3)
                                        //                {
                                        //                    col_found = true;
                                        //                    break;
                                        //                }

                                        //                //if (col_found)
                                        //                //{
                                        //                //    foreach (CustfldsEntity Ent in Case0061_Cust_List)
                                        //                //    {
                                        //                //        if (cont_dr["FLDH_CODE"].ToString() == Ent.CustCode)
                                        //                //        {
                                        //                //            tmp_Col_Disp_Name = Ent.CustDesc; break;
                                        //                //        }
                                        //                //    }

                                        //                //    break;
                                        //                //}
                                        //                loop_cnt++;
                                        //            }
                                        //        }

                                        //        //20160922
                                        //        //"080042,080046, 080050".Contains(Master.Col_Code)
                                        //        //if (col_found)
                                        //        cont_disp_column_flag = col_found;
                                        //    }
                                        //}

                                        //Added by sudheer on 07/22/2021 to remove columns in selected criteria based on Category
                                        //if (string.IsNullOrEmpty(CategoryCode.Trim()) && Tmp_table_name=="CASEACT" && Master.Col_Catgeory=="02")
                                        if (string.IsNullOrEmpty(CategoryCode.Trim()) && Tmp_table_name == "CASEACT" && (Master.Col_Catgeory == "02" || Master.Col_Catgeory == "03"))
                                        {
                                            AdhocSel_CriteriaEntity CrtiEntity = new AdhocSel_CriteriaEntity();
                                            if (Criteria_SelCol_List.Count > 0)
                                            {
                                                CrtiEntity = Criteria_SelCol_List.Find(u => u.Column_Name.Trim() == Master.Act_Col_Name.Trim());
                                                if (CrtiEntity != null)
                                                {
                                                    if (Master.Col_Catgeory == CrtiEntity.Col_Catg)
                                                        Criteria_SelCol_List.Remove(CrtiEntity);
                                                }

                                                if (Crit_SelCol_Grid.Rows.Count > 0)
                                                {
                                                    foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                                                    {
                                                        if (dr.Cells["Sel_Org_ColNmae"].Value.ToString().Trim() == Master.Act_Col_Name.Trim())
                                                        {
                                                            //if (!Col_Selected_SW)
                                                            //{
                                                            //    Crit_SelCol_Grid.Rows.Remove(dr);
                                                            //    break;
                                                            //}
                                                            if (CrtiEntity != null)
                                                            {
                                                                if (Master.Col_Catgeory == CrtiEntity.Col_Catg)
                                                                {
                                                                    Crit_SelCol_Grid.Rows.Remove(dr);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(CategoryCode.Trim()) && Tmp_table_name == "CASEACT")
                                        {
                                            if (!string.IsNullOrEmpty(Master.Col_Catgeory.Trim()))
                                            {
                                                if (Master.Col_Catgeory == CategoryCode)
                                                {
                                                    cont_disp_column_flag = true;
                                                }
                                                else
                                                {
                                                    cont_disp_column_flag = false;
                                                    //Added by sudheer on 07/22/2021 to remove columns in selected criteria based on Category
                                                    AdhocSel_CriteriaEntity CrtiEntity = new AdhocSel_CriteriaEntity();
                                                    if (Criteria_SelCol_List.Count > 0)
                                                    {
                                                        CrtiEntity = Criteria_SelCol_List.Find(u => u.Column_Name.Trim() == Master.Act_Col_Name.Trim());
                                                        if (CrtiEntity != null)
                                                        {
                                                            if (Master.Col_Catgeory != CrtiEntity.Col_Catg)
                                                                Criteria_SelCol_List.Remove(CrtiEntity);
                                                        }
                                                    }
                                                    if (Crit_SelCol_Grid.Rows.Count > 0)
                                                    {
                                                        foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                                                        {
                                                            if (dr.Cells["Sel_Org_ColNmae"].Value.ToString().Trim() == Master.Act_Col_Name.Trim())
                                                            {

                                                                //if(!Col_Selected_SW)
                                                                //{
                                                                //    Crit_SelCol_Grid.Rows.Remove(dr);
                                                                //    break;
                                                                //}
                                                                if (CrtiEntity != null)
                                                                {
                                                                    if (Master.Col_Catgeory == CrtiEntity.Col_Catg)
                                                                    {
                                                                        Crit_SelCol_Grid.Rows.Remove(dr);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (cont_disp_column_flag)
                                            {
                                                if (Tmp_table_name == Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() ||
                                                (Tmp_table_name == "CASEMSOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEMS") ||
                                                (Tmp_table_name == "CAOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEACT"))
                                                {
                                                    if (Disp_Name_Found)
                                                    {
                                                        if (Col_Selected_SW)
                                                            Column_Grid.Rows.Add(Img_Tick, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "Y", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                        else
                                                            Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    }
                                                    else
                                                    {
                                                        if (Master.Active == "A")
                                                            Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    }

                                                    Tmp_Grid_Count++;
                                                }
                                            }

                                        }
                                        else if (Tmp_table_name == "CASEACT")
                                        {
                                            if (Master.Col_Catgeory != "02" && Master.Col_Catgeory != "03" && Master.Col_Catgeory != "04")
                                            {
                                                if (Tmp_table_name == Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() ||
                                                    (Tmp_table_name == "CASEMSOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEMS") ||
                                                    (Tmp_table_name == "CAOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEACT"))
                                                {
                                                    if (Disp_Name_Found)
                                                    {
                                                        if (Col_Selected_SW)
                                                            Column_Grid.Rows.Add(Img_Tick, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "Y", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                        else
                                                            Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    }
                                                    else
                                                    {
                                                        if (Master.Active == "A")
                                                            Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    }

                                                    Tmp_Grid_Count++;
                                                }
                                            }
                                        }
                                        else

                                        if (Tmp_table_name != "CASECONT")
                                        {
                                            if (Tmp_table_name == Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() ||
                                                (Tmp_table_name == "CASEMSOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEMS") ||
                                                (Tmp_table_name == "CAOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEACT"))
                                            {
                                                if (Disp_Name_Found)
                                                {
                                                    if (Col_Selected_SW)
                                                        Column_Grid.Rows.Add(Img_Tick, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "Y", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    else
                                                        Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                }
                                                else
                                                {
                                                    if (Master.Active == "A")
                                                        Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                }

                                                Tmp_Grid_Count++;
                                            }
                                        }

                                        if (Tmp_table_name == "CASECONT" && cont_disp_column_flag)
                                        {
                                            if (Tmp_table_name == Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() ||
                                                (Tmp_table_name == "CASEMSOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEMS") ||
                                                (Tmp_table_name == "CAOBO" && Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "CASEACT"))
                                            {
                                                if (Disp_Name_Found)
                                                {
                                                    if (Col_Selected_SW)
                                                        Column_Grid.Rows.Add(Img_Tick, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "Y", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                    else
                                                        Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                }
                                                else
                                                {
                                                    if (Master.Active == "A")
                                                        Column_Grid.Rows.Add(Img_Blank, Col_Disp_Name, DataType_Desc, Max_Length, Master.Act_Col_Name, "N", Tmp_DataType, Col_Format_Type, Col_AgyTab_Code, Master.Col_Code, Min_Length, Can_Criteria_SW, Can_Count_SW, Master.Col_Catgeory);
                                                }

                                                Tmp_Grid_Count++;
                                            }
                                        }
                                    }
                                }

                                if (Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "ADDCUST")
                                    Tmp_Grid_Count += Fill_AddCust_Cust_Ques();

                                //Added by Sudheer for PREASSES
                                if (Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "PRESRESP")
                                    Tmp_Grid_Count += Fill_PreAsses_Cust_Ques();

                                //Added by Sudheer for SERCUST
                                if (Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString().Trim() == "SERCUST")
                                    Tmp_Grid_Count += Fill_SerCust_Cust_Ques();


                                if (Tmp_Grid_Count > 0)
                                {
                                    //Column_Grid.Rows[0].Tag = 0;

                                    Column_Grid.CurrentCell = Column_Grid.Rows[0].Cells[1];

                                    int scrollPosition = 0;
                                    scrollPosition = Column_Grid.CurrentCell.RowIndex;
                                    //int CurrentPage = (scrollPosition / Column_Grid.ItemsPerPage);
                                    // CurrentPage++;
                                    // Column_Grid.CurrentPage = CurrentPage;
                                    //Column_Grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                                }
                            }
                        }
                    }
                }
            }
            if (Column_Grid.Rows.Count > 0)
                Column_Grid.Rows[0].Selected = true;
        }


        private void Table_Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (Table_Grid.Rows.Count > 0)
                Fill_Columns_Grid();
        }


        List<Adhoc_ADDCUSTEntity> Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
        private int Fill_AddCust_Cust_Ques()
        {
            bool Col_Selected_SW = false;
            int Tmp_loop_Cnt = 0;
            Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
            if (Addcust_Cust_Columns.Count <= 0)
                Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_ADDCUST_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

            foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
            {
                if (!string.IsNullOrEmpty(Entity.Cust_Desc))
                {
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        Col_Selected_SW = false;
                        if (Entity.Act_Cust_Code == Sel_Col_Entity.Column_Name && Sel_Col_Entity.Table_name == "ADDCUST")
                        { Col_Selected_SW = true; break; }
                    }
                    string DataTypeDesc = string.Empty; string CountSw = "Y";
                    switch (Entity.Cust_Resp_Type.Trim())
                    {
                        case "C": DataTypeDesc = "Check Box"; CountSw = "N"; break;
                        case "T": DataTypeDesc = "Date"; break;
                        case "D": DataTypeDesc = "Drop Down"; break;
                        case "N": DataTypeDesc = "Numeric"; break;
                        case "X": DataTypeDesc = "Text"; break;
                    }

                    Column_Grid.Rows.Add((Col_Selected_SW ? Img_Tick : Img_Blank), Entity.Cust_Desc, DataTypeDesc, "30", Entity.Act_Cust_Code, (Col_Selected_SW ? "Y" : "N"), "  ", Entity.Cust_Resp_Type, "ADCST", Entity.Act_Cust_Code, "20", "Y", "Y", string.Empty);

                    Tmp_loop_Cnt++;
                }
            }
            return Tmp_loop_Cnt;
        }

        #region PreAsses Questions

        //Added on 01/07/2016

        private int Fill_PreAsses_Cust_Ques()
        {
            bool Col_Selected_SW = false;
            int Tmp_loop_Cnt = 0;
            Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
            if (Addcust_Cust_Columns.Count <= 0)
                Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_PREASSES_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

            foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
            {
                if (!string.IsNullOrEmpty(Entity.Cust_Desc))
                {
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        Col_Selected_SW = false;
                        if (Entity.Act_Cust_Code == Sel_Col_Entity.Column_Name && Sel_Col_Entity.Table_name == "PRESRESP")
                        { Col_Selected_SW = true; break; }
                    }
                    string DataTypeDesc = string.Empty; string CountSw = "Y";
                    switch (Entity.Cust_Resp_Type.Trim())
                    {
                        case "C": DataTypeDesc = "Check Box"; CountSw = "N"; break;
                        case "T": DataTypeDesc = "Date"; break;
                        case "D": DataTypeDesc = "Drop Down"; break;
                        case "N": DataTypeDesc = "Numeric"; break;
                        case "X": DataTypeDesc = "Text"; break;
                    }

                    Column_Grid.Rows.Add((Col_Selected_SW ? Img_Tick : Img_Blank), Entity.Cust_Desc, DataTypeDesc, "30", Entity.Act_Cust_Code, (Col_Selected_SW ? "Y" : "N"), "  ", Entity.Cust_Resp_Type, "PREAS", Entity.Act_Cust_Code, "20", "Y", "Y",string.Empty);

                    Tmp_loop_Cnt++;
                }
            }
            return Tmp_loop_Cnt;
        }

        #endregion

        #region SERCUST Questions for CABAEMS

        //Added on 05/24/2017

        private int Fill_SerCust_Cust_Ques()
        {
            bool Col_Selected_SW = false;
            int Tmp_loop_Cnt = 0;
            Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
            if (Addcust_Cust_Columns.Count <= 0)
                Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_SERCUST_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

            foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
            {
                if (!string.IsNullOrEmpty(Entity.Cust_Desc))
                {
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        Col_Selected_SW = false;
                        if (Entity.Act_Cust_Code == Sel_Col_Entity.Column_Name && Sel_Col_Entity.Table_name == "SERCUST")
                        { Col_Selected_SW = true; break; }
                    }
                    string DataTypeDesc = string.Empty; string CountSw = "Y";
                    switch (Entity.Cust_Resp_Type.Trim())
                    {
                        case "C": DataTypeDesc = "Check Box"; CountSw = "N"; break;
                        case "T": DataTypeDesc = "Date"; break;
                        case "D": DataTypeDesc = "Drop Down"; break;
                        case "N": DataTypeDesc = "Numeric"; break;
                        case "X": DataTypeDesc = "Text"; break;
                    }

                    Column_Grid.Rows.Add((Col_Selected_SW ? Img_Tick : Img_Blank), Entity.Cust_Desc, DataTypeDesc, "30", Entity.Act_Cust_Code, (Col_Selected_SW ? "Y" : "N"), "  ", Entity.Cust_Resp_Type, "SRCST", Entity.Act_Cust_Code, "20", "Y", "Y",string.Empty);

                    Tmp_loop_Cnt++;
                }
            }
            return Tmp_loop_Cnt;
        }

        #endregion

        int Selected_Col_Count = 0;
        List<AdhocSel_CriteriaEntity> Criteria_SelCol_List = new List<AdhocSel_CriteriaEntity>();
        private void Column_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Privileges.ModuleName != "UserReportMaintenance")
            {
                if (Column_Grid.Rows.Count > 0 && e.RowIndex != -1)
                {
                    int ColIdx = Column_Grid.CurrentCell.ColumnIndex;
                    int RowIdx = Column_Grid.CurrentCell.RowIndex;

                    if (!ColGrid_Header_Clicked)
                    {
                        if (e.ColumnIndex == 0)//&& (Mode.Equals("Add") || Mode.Equals("Edit")))
                        {
                            if (Column_Grid.CurrentRow.Cells["Col_Selected"].Value.ToString() == "Y")
                            {
                                Column_Grid.CurrentRow.Cells["Col_Sel"].Value = Img_Blank;
                                Column_Grid.CurrentRow.Cells["Col_Selected"].Value = "N";

                                Selected_Col_Count--;

                                Delete_SelCol_From_List();

                                //foreach (CAMASTEntity Entity in CAMASTList)
                                //{
                                //    if (Entity.Code == Column_Grid.CurrentRow.Cells["CACode"].Value.ToString())
                                //        Entity.Sel_SW = false;
                                //}
                            }
                            else
                            {
                                Column_Grid.CurrentRow.Cells["Col_Sel"].Value = Img_Tick;
                                Column_Grid.CurrentRow.Cells["Col_Selected"].Value = "Y";

                                Selected_Col_Count++;
                                Add_SelCol_To_List();

                                //foreach (CAMASTEntity Entity in CAMASTList)
                                //{
                                //    if (Entity.Code == Column_Grid.CurrentRow.Cells["CACode"].Value.ToString())
                                //        Entity.Sel_SW = true;
                                //}
                            }
                        }
                    }
                    ColGrid_Header_Clicked = false;
                    //else
                    if (Criteria_SelCol_List.Count > 0)
                    {
                        Pb_Edit.Visible = true;
                        Btn_Clear_All.Visible = true;
                    }
                    else
                    {
                        Pb_Edit.Visible = false;
                        Btn_Clear_All.Visible = false;
                    }
                }
            }
        }

        bool ColGrid_Header_Clicked = false;
        private void Column_Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
                ColGrid_Header_Clicked = true;
        }

        int Dsp_Position = 0;
        private void Add_SelCol_To_List()
        {
            string Attributes = "YNNNN";
            AdhocSel_CriteriaEntity Add_Col_entity = new AdhocSel_CriteriaEntity(true);

            Dsp_Position++;
            Add_Col_entity.Can_Add_Col = "Y";
            Add_Col_entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
            Add_Col_entity.Table_ID = Table_Grid.CurrentRow.Cells["Table_ID"].Value.ToString();
            Add_Col_entity.Table_name = Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString();
            Add_Col_entity.Column_Disp_Name = Column_Grid.CurrentRow.Cells["Col_Disp_Name"].Value.ToString();
            Add_Col_entity.Column_Name = Column_Grid.CurrentRow.Cells["Col_OrgName"].Value.ToString();

            //if (Add_Col_entity.Column_Disp_Name == "As Of")
            if (Add_Col_entity.Column_Name == "ENRL_DATE")
            {
                Add_Col_entity.Can_Add_Col = "N"; Attributes = "NNNNN";
            }

            //Add_Col_entity.Col_Ordinal = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            Add_Col_entity.Data_Type = Column_Grid.CurrentRow.Cells["Col_DataType"].Value.ToString();
            Add_Col_entity.Disp_Data_Type = Column_Grid.CurrentRow.Cells["Col_Org_DataType"].Value.ToString();

            Add_Col_entity.Disp_Code_Length = Column_Grid.CurrentRow.Cells["Col_MinLength"].Value.ToString();
            Add_Col_entity.Disp_Desc_Length = Column_Grid.CurrentRow.Cells["Col_MaxLength"].Value.ToString();

            Add_Col_entity.Col_Format_Type = Column_Grid.CurrentRow.Cells["Col_Format_Type"].Value.ToString();

            Add_Col_entity.Col_Catg= Column_Grid.CurrentRow.Cells["Col_Catg"].Value.ToString();


            Add_Col_entity.Description = "N";
            //string MaxLength = Column_Grid.CurrentRow.Cells["Col_MaxLength"].Value.ToString();
            string MaxLength = Column_Grid.CurrentRow.Cells["Col_MaxLength"].Value.ToString();

            if (Add_Col_entity.Col_Format_Type == "D" || Add_Col_entity.Col_Format_Type == "L")
            {
                MaxLength = Column_Grid.CurrentRow.Cells["Col_MinLength"].Value.ToString();

                {
                    MaxLength = Column_Grid.CurrentRow.Cells["Col_MaxLength"].Value.ToString();
                    Attributes = "YYNNN";
                    Add_Col_entity.Description = "Y";
                }

                Attributes = "YYNNN";
                Add_Col_entity.Description = "Y";
            }

            Add_Col_entity.Max_Display_Width = Get_Column_Disp_Width(Add_Col_entity.Data_Type, (string.IsNullOrEmpty(MaxLength.Trim()) ? 0 : int.Parse(MaxLength)), Add_Col_entity.Col_Format_Type);

            Add_Col_entity.AgyCode = Column_Grid.CurrentRow.Cells["Col_AgyCode"].Value.ToString();
            Add_Col_entity.Col_Master_Code = Column_Grid.CurrentRow.Cells["Col_Master_Code"].Value.ToString();


            Add_Col_entity.Criteria_SW = Column_Grid.CurrentRow.Cells["Col_Can_Criteria"].Value.ToString();
            Add_Col_entity.Countable_SW = Column_Grid.CurrentRow.Cells["Col_Can_Count"].Value.ToString();



            //Add_Col_entity.Display = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Count = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Sort = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Break = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Sort_Order = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Break_Order = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.AgyCode = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Disp_Position = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();

            Add_Col_entity.Display = Attributes.Substring(0, 1);
            Add_Col_entity.Description = Attributes.Substring(1, 1);
            Add_Col_entity.Count = Attributes.Substring(2, 1);
            Add_Col_entity.Sort = Attributes.Substring(3, 1);
            //Add_Col_entity.Break = Attributes.Substring(4, 1);
            Add_Col_entity.Break_Order = Attributes.Substring(4, 1);
            Add_Col_entity.Sort_Order = "";


            Add_Col_entity.EqualTo = Add_Col_entity.NotEqualTo = Add_Col_entity.LessThan = Add_Col_entity.GreaterThan = " ";
            Add_Col_entity.Attributes = Attributes;

            int Row_Index = Crit_SelCol_Grid.Rows.Add(Add_Col_entity.Column_Disp_Name, Add_Col_entity.Table_name, Add_Col_entity.Data_Type, Add_Col_entity.Disp_Desc_Length, (Add_Col_entity.Can_Add_Col == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Description == "Y" ? Img_Tick : Img_Blank), Img_Blank, Img_Blank, Img_Blank, Add_Col_entity.Column_Name, Attributes, " ", " ", " ", " ", Add_Col_entity.Table_ID, "0", Add_Col_entity.Col_Format_Type, Add_Col_entity.AgyCode, Add_Col_entity.Col_Master_Code, Add_Col_entity.Criteria_SW, Add_Col_entity.Countable_SW, Add_Col_entity.Disp_Position, "N");

            //Crit_SelCol_Grid.Rows[Row_Index].Tag = 0;

            //if (Add_Col_entity.Col_Format_Type == "D" && Add_Col_entity.Col_Format_Type == "L")
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Desc"].ReadOnly = false;

            if (Add_Col_entity.Column_Disp_Name == "Year" && !string.IsNullOrEmpty(Program_Year.Trim()) && Add_Col_entity.Column_Name != "MEDI_YEAR")
            {
                Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Equalto"].Value = Program_Year;
                Add_Col_entity.EqualTo = Program_Year;
                string toolTipText = "Equal to : " + Program_Year;

                foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                    cell.ToolTipText = toolTipText;

                Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            }

            //if (Add_Col_entity.Column_Disp_Name == "As Of")
            if (Add_Col_entity.Column_Name == "ENRL_DATE")
            {
                Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Equalto"].Value = DateTime.Today.ToShortDateString();
                Add_Col_entity.EqualTo = DateTime.Today.ToShortDateString();
                string toolTipText = "Equal to : " + DateTime.Today.ToShortDateString();

                foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                    cell.ToolTipText = toolTipText;

                Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            }

            //if (Add_Col_entity.Column_Name == "Current_Age")
            //{
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_NotEqualto"].Value = DateTime.Today.ToShortDateString();
            //    string toolTipText = "Age as of          : " + DateTime.Today.ToShortDateString();

            //    foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
            //        cell.ToolTipText = toolTipText;

            //    Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            //}
            
            Criteria_SelCol_List.Add(new AdhocSel_CriteriaEntity(Add_Col_entity));
            if (Crit_SelCol_Grid.Rows.Count > 0)
                Crit_SelCol_Grid.Rows[0].Selected = true;
        }


        private void Add_SelCol_To_List_SERFUND()
        {
            string Attributes = "YNNNN";
            AdhocSel_CriteriaEntity Add_Col_entity = new AdhocSel_CriteriaEntity(true);

            Dsp_Position++;
            Add_Col_entity.Can_Add_Col = "Y";
            Add_Col_entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
            Add_Col_entity.Table_ID = "54";
            Add_Col_entity.Table_name = "SERCUST";
            Add_Col_entity.Column_Disp_Name = "FUND";
            Add_Col_entity.Column_Name = "SER_FUND";

            //if (Add_Col_entity.Column_Disp_Name == "As Of")
            if (Add_Col_entity.Column_Name == "ENRL_DATE")
            {
                Add_Col_entity.Can_Add_Col = "N"; Attributes = "NNNNN";
            }

            //Add_Col_entity.Col_Ordinal = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            Add_Col_entity.Data_Type = "Text";
            Add_Col_entity.Disp_Data_Type = "char";

            Add_Col_entity.Disp_Code_Length = "6";
            Add_Col_entity.Disp_Desc_Length = "20";

            Add_Col_entity.Col_Format_Type = "D";


            Add_Col_entity.Description = "N";
            //string MaxLength = Column_Grid.CurrentRow.Cells["Col_MaxLength"].Value.ToString();
            string MaxLength = "20";

            if (Add_Col_entity.Col_Format_Type == "D" || Add_Col_entity.Col_Format_Type == "L")
            {
                MaxLength = "6";

                {
                    MaxLength = "20";
                    Attributes = "YYNNN";
                    Add_Col_entity.Description = "Y";
                }

                Attributes = "YYNNN";
                Add_Col_entity.Description = "Y";
            }

            Add_Col_entity.Max_Display_Width = Get_Column_Disp_Width(Add_Col_entity.Data_Type, (string.IsNullOrEmpty(MaxLength.Trim()) ? 0 : int.Parse(MaxLength)), Add_Col_entity.Col_Format_Type);

            Add_Col_entity.AgyCode = "00501";
            //Add_Col_entity.Col_Master_Code = Column_Grid.CurrentRow.Cells["Col_Master_Code"].Value.ToString();


            Add_Col_entity.Criteria_SW = "Y";
            Add_Col_entity.Countable_SW = "Y";



            //Add_Col_entity.Display = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Count = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Sort = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Break = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Sort_Order = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Break_Order = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.AgyCode = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();
            //Add_Col_entity.Disp_Position = Column_Grid.CurrentRow.Cells["Col_Sel"].Value.ToString();

            Add_Col_entity.Display = Attributes.Substring(0, 1);
            Add_Col_entity.Description = Attributes.Substring(1, 1);
            Add_Col_entity.Count = Attributes.Substring(2, 1);
            Add_Col_entity.Sort = Attributes.Substring(3, 1);
            //Add_Col_entity.Break = Attributes.Substring(4, 1);
            Add_Col_entity.Break_Order = Attributes.Substring(4, 1);
            Add_Col_entity.Sort_Order = "";


            Add_Col_entity.EqualTo = Add_Col_entity.NotEqualTo = Add_Col_entity.LessThan = Add_Col_entity.GreaterThan = " ";
            Add_Col_entity.Attributes = Attributes;

            //int Row_Index = Crit_SelCol_Grid.Rows.Add(Add_Col_entity.Column_Disp_Name, Add_Col_entity.Table_name, Add_Col_entity.Data_Type, Add_Col_entity.Disp_Desc_Length, (Add_Col_entity.Can_Add_Col == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Description == "Y" ? Img_Tick : Img_Blank), Img_Blank, Img_Blank, Img_Blank, Add_Col_entity.Column_Name, Attributes, " ", " ", " ", " ", Add_Col_entity.Table_ID, "0", Add_Col_entity.Col_Format_Type, Add_Col_entity.AgyCode, Add_Col_entity.Col_Master_Code, Add_Col_entity.Criteria_SW, Add_Col_entity.Countable_SW, Add_Col_entity.Disp_Position, "N");

            //Crit_SelCol_Grid.Rows[Row_Index].Tag = 0;

            //if (Add_Col_entity.Col_Format_Type == "D" && Add_Col_entity.Col_Format_Type == "L")
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Desc"].ReadOnly = false;

            //if (Add_Col_entity.Column_Disp_Name == "Year" && !string.IsNullOrEmpty(Program_Year.Trim()) && Add_Col_entity.Column_Name != "MEDI_YEAR")
            //{
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Equalto"].Value = Program_Year;
            //    Add_Col_entity.EqualTo = Program_Year;
            //    string toolTipText = "Equal to : " + Program_Year;

            //    foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
            //        cell.ToolTipText = toolTipText;

            //    Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            //}

            ////if (Add_Col_entity.Column_Disp_Name == "As Of")
            //if (Add_Col_entity.Column_Name == "ENRL_DATE")
            //{
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_Equalto"].Value = DateTime.Today.ToShortDateString();
            //    Add_Col_entity.EqualTo = DateTime.Today.ToShortDateString();
            //    string toolTipText = "Equal to : " + DateTime.Today.ToShortDateString();

            //    foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
            //        cell.ToolTipText = toolTipText;

            //    Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            //}

            //if (Add_Col_entity.Column_Name == "Current_Age")
            //{
            //    Crit_SelCol_Grid.Rows[Row_Index].Cells["Sel_NotEqualto"].Value = DateTime.Today.ToShortDateString();
            //    string toolTipText = "Age as of          : " + DateTime.Today.ToShortDateString();

            //    foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
            //        cell.ToolTipText = toolTipText;

            //    Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
            //}

            Criteria_SelCol_List.Add(new AdhocSel_CriteriaEntity(Add_Col_entity));
        }



        List<AdhocSel_CriteriaEntity> Criteria_SelCountCol_List = new List<AdhocSel_CriteriaEntity>();
        private void Get_SelCountCol_To_List()
        {
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            {
                if (Entity.Count == "Y")
                    Criteria_SelCountCol_List.Add(new AdhocSel_CriteriaEntity(Entity));
            }
        }

        private string Get_Column_Disp_Width(string Data_Type, int Int_Col_Length, string Format_Type)
        {
            string Max_Disp_Length = "1";

            // Int_Col_Length += 0.5;


            double Float_Col_Length = float.Parse(Int_Col_Length.ToString());
            switch (Format_Type)  // For masking fields increase Field Size
            {
                case "%":                           // %
                    Float_Col_Length += 1; break;
                case "S":                           // SSN#
                    Float_Col_Length += 0.2; break;
                case "Z":                           // ZipCode
                    Float_Col_Length += 0.2; break;
                case "E":                           // Telephone
                    Float_Col_Length += 0.4; break;
            }


            switch (Int_Col_Length)
            {
                case 1: Float_Col_Length = Float_Col_Length + 1; break;
                case 2: Float_Col_Length = Float_Col_Length + 0.5; break;
                case 3: Float_Col_Length = Float_Col_Length + 0.3; break;
                case 4: Float_Col_Length = Float_Col_Length + 0.1; break;
                    //default: Float_Col_Length = float.Parse(Int_Col_Length.ToString()) + 0.008; break;
            }

            switch (Data_Type)
            {
                case " ": Max_Disp_Length = (Float_Col_Length * 0.1).ToString(); break;
                case "Text": Max_Disp_Length = (Float_Col_Length * 0.1).ToString(); break;
                case "Numeric": Max_Disp_Length = (Float_Col_Length * 0.1).ToString(); break;

                case "Date": Max_Disp_Length = "0.8"; break;
            }


            return Max_Disp_Length + "in";
        }

        private void Delete_SelCol_From_List()
        {
            bool Rec_Deleted = false;
            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                if (Column_Grid.CurrentRow.Cells["Col_OrgName"].Value.ToString() == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                    Table_Grid.CurrentRow.Cells["Table_Name"].Value.ToString() == dr.Cells["Sel_TableName"].Value.ToString() &&
                    Column_Grid.CurrentRow.Cells["Col_Master_Code"].Value.ToString() == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                {
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                            Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                            Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                        {
                            Criteria_SelCol_List.Remove(Entity);
                            Dsp_Position--;
                            Rec_Deleted = true;
                            break;
                        }
                    }
                    Crit_SelCol_Grid.Rows.RemoveAt(dr.Index);

                    if (!(Crit_SelCol_Grid.RowCount > 0))
                        Pb_Edit.Visible = false;
                    break;
                }
            }

            if (Rec_Deleted)
            {
                Dsp_Position = 0;
                foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                {
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                            Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                            Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                        {
                            Dsp_Position++;
                            //Entity.Disp_Position = Dsp_Position.ToString();
                            Entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
                        }
                    }
                }
            }

        }


        bool Summary_Sw = false, MSTTab_Sel_Sw = false, SNPTab_Sel_Sw = false, ADDCUSTTab_Sel_Sw = false, PRESRESPTab_Sel_Sw = false, SERCUSTTab_Sel_sw = false, EMSRESTabSel_Sw = false,EMSCLCPMCTabSel_Sw=false,SALACTTable_Sw=false,CALCONTTable_Sw=false,
            CASECONTTab_Sw=false;
        bool SPDBPost_Sw = false, STFMST_Sw = false, STFTRAIN_SW = false;
        //Added by Sudheer on 02/18/2021 for Agency Partner Subtables
        bool PartnerTab_SelSw=false, PartRepTab_SelSw = false, PartSerTab_SelSw = false, PartboutiqueTab_SelSw = false;

        string ADDCUSTTab_Alone_Sel_Sw = "Y", Curr_Age_Asof_Date = DateTime.Today.ToShortDateString(), PRESRESPTab_Alone_Sel_Sw = "Y", SERCUSTTab_Alone_Sel_Sw = "Y";
        bool CASEACTTab_Sel_SW = false, CASEMSTab_Sel_Sw = false;
        private string[] Get_XML_Format_of_Selected_Rows(string Operation)
        {
            MSTTab_Sel_Sw = SNPTab_Sel_Sw = ADDCUSTTab_Sel_Sw = EMSRESTabSel_Sw = SERCUSTTab_Sel_sw = PRESRESPTab_Sel_Sw = false; ADDCUSTTab_Alone_Sel_Sw = "Y";
            PartnerTab_SelSw = false; PartRepTab_SelSw = false; PartSerTab_SelSw = false; PartboutiqueTab_SelSw = false;
            SPDBPost_Sw = false; STFMST_Sw = false; STFTRAIN_SW = false;

            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASEMST")).Count > 0) MSTTab_Sel_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASESNP")).Count > 0) SNPTab_Sel_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("ADDCUST")).Count > 0) ADDCUSTTab_Sel_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("PRESRESP")).Count > 0) PRESRESPTab_Sel_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("EMSRES")).Count > 0) EMSRESTabSel_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SERCUST")).Count > 0) SERCUSTTab_Sel_sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASEACT")).Count > 0) CASEACTTab_Sel_SW = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASEMS")).Count > 0) CASEMSTab_Sel_Sw = true;

            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYPART")).Count > 0) PartnerTab_SelSw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYREP")).Count > 0) PartRepTab_SelSw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYSERVICES")).Count > 0) PartSerTab_SelSw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYBOUTIQUE")).Count > 0) PartboutiqueTab_SelSw = true;

            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SPDBPOST")).Count > 0) SPDBPost_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("STAFFMST")).Count > 0) STFMST_Sw = true;
            if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("STFTRAIN")).Count > 0) STFTRAIN_SW = true;

            //foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
            //{
            //    switch (Sel_Col_Entity.Table_name)
            //    {
            //        case "CASEMST": MSTTab_Sel_Sw = true; break;
            //        case "CASESNP": SNPTab_Sel_Sw = true; break;
            //        case "ADDCUST": ADDCUSTTab_Sel_Sw = true; break;
            //        case "EMSRES": EMSRESTabSel_Sw = true; break;
            //        case "PRESRESP": PRESRESPTab_Sel_Sw = true; break;
            //        case "SERCUST": SERCUSTTab_Sel_sw = true; break;
            //    }

            //    if (Sel_Col_Entity.Table_name != "ADDCUST" || Sel_Col_Entity.Table_name != "PRESRESP" || Sel_Col_Entity.Table_name != "SERCUST")
            //        ADDCUSTTab_Alone_Sel_Sw = "N";

            //    if (MSTTab_Sel_Sw && SNPTab_Sel_Sw && ADDCUSTTab_Alone_Sel_Sw == "N" && SelSercustSw == "N")
            //        break;

            //    if (SERCUSTTab_Sel_sw && SelSercustSw == "N")
            //    {
            //        //if (!MSTTab_Sel_Sw || !SNPTab_Sel_Sw || !EMSRESTabSel_Sw)
            //        break;
            //    }
            //}



            StringBuilder str = new StringBuilder();
            StringBuilder str_Summary = new StringBuilder();

            string[] XML_Strings = new string[2];

            string Empty_Value = "NULL", Atr_List = null, Tmp_Table_Id = null; ;
            string EqualTo = "NULL", NotEqualTo = "NULL", Greater = "NULL", Less = "NULL", Format_Type = "NULL";
            string AgyCode = "NULL", Disp_Position = "0", Sort_Order = "0", Need_Casting = "NULL";
            string Column_name = " ", Tmp_Col_Nmae = " ", Custom_Select = " ", Table_Subscript = " ", Get_Null_Records = "";
            int Tmp_Compare_Int = 1;


            // Summary Variables
            string AgyType = "NULL", Get_Summary_For = "C", Col_Disp_Name = string.Empty, Tml_Sel_Table_Name = "";
            bool MST_Agency_Selected = false, Needto_Select_Agency = false, Site_Agency_Selected = false,
                 ENRL_Asof_Selected = false, Needto_Select_Asof = false, CASEMSobo_Selected = false, CAobo_Selected = false, SNP_CLID_Selected = false, MST_FAMID_Selected = false;


            char Disp_Sw = '0', Desc_Sw = '0', Count_Sw = '0', Sort_Sw = '0', Break_Sw = '0';

            Summary_Sw = false;
            str.Append("<Rows>");
            str_Summary.Append("<Rows>");
            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                Atr_List = dr.Cells["Atributes_List"].Value.ToString();
                Tml_Sel_Table_Name = dr.Cells["Sel_TableName"].Value.ToString();

                Disp_Sw = (Atr_List.Substring(0, 1) == "Y" ? '1' : '0');
                Desc_Sw = (Atr_List.Substring(1, 1) == "Y" ? '1' : '0');
                Count_Sw = (Atr_List.Substring(2, 1) == "Y" ? '1' : '0');
                //Sort_Sw = (Atr_List.Substring(3, 1) == "Y" ? '1' : '0');
                Break_Sw = (Atr_List.Substring(4, 1) == "Y" ? '1' : '0');


                AgyCode = (dr.Cells["Sel_Col_AgyTabCode"].Value.ToString() != " " ? dr.Cells["Sel_Col_AgyTabCode"].Value.ToString() : "NULL");

                if (AgyCode == "SERVS")
                { }
                else
                    EqualTo = (dr.Cells["Sel_Equalto"].Value.ToString() != " " ? dr.Cells["Sel_Equalto"].Value.ToString() : "NULL");

                //ADDED BY SUDHEER ON 10/13/2021
                if(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()=="Y" && AgyCode=="SPHIE" && EqualTo == "NULL")
                {
                    if (ALLSPR_List.Count > 0)
                    {
                        string AGYTAB_Conditions = string.Empty;
                        foreach (HierarchyEntity HieEnt in ALLSPR_List)
                        {
                            AGYTAB_Conditions += ("'" + HieEnt.Agency+HieEnt.Dept+HieEnt.Prog + "'" + ",");
                        }
                        AGYTAB_Conditions = AGYTAB_Conditions.Substring(0, AGYTAB_Conditions.Length - 1);
                        EqualTo = AGYTAB_Conditions;
                    }
                    else
                    {
                        Get_ALLSPR_List();
                        string AGYTAB_Conditions = string.Empty;
                        foreach (HierarchyEntity HieEnt in ALLSPR_List)
                        {
                            AGYTAB_Conditions += ("'" + HieEnt.Agency + HieEnt.Dept + HieEnt.Prog + "'" + ",");
                        }
                        AGYTAB_Conditions = AGYTAB_Conditions.Substring(0, AGYTAB_Conditions.Length - 1);
                        EqualTo = AGYTAB_Conditions;
                    }
                }
                else if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y" && AgyCode == "SPLAN" && EqualTo == "NULL")
                {
                    if(ServicePlan_List.Rows.Count>0)
                    {
                        string AGYTAB_Conditions = string.Empty;
                        foreach (DataRow drSP in ServicePlan_List.Rows)
                        {
                            AGYTAB_Conditions += ("'" + drSP["sp0_servicecode"].ToString().Trim() + "'" + ",");
                        }
                        AGYTAB_Conditions = AGYTAB_Conditions.Substring(0, AGYTAB_Conditions.Length - 1);
                        EqualTo = AGYTAB_Conditions;
                    }
                    else
                    {
                        Get_ServicePlan_list();
                        if(ServicePlan_List.Rows.Count>0)
                        {
                            string AGYTAB_Conditions = string.Empty;
                            foreach (DataRow drSP in ServicePlan_List.Rows)
                            {
                                AGYTAB_Conditions += ("'" + drSP["sp0_servicecode"].ToString().Trim() + "'" + ",");
                            }
                            AGYTAB_Conditions = AGYTAB_Conditions.Substring(0, AGYTAB_Conditions.Length - 1);
                            EqualTo = AGYTAB_Conditions;
                        }
                    }
                }

                Curr_Age_Asof_Date = NotEqualTo = (dr.Cells["Sel_NotEqualto"].Value.ToString() != " " ? dr.Cells["Sel_NotEqualto"].Value.ToString() : "NULL");
                Greater = (dr.Cells["Sel_Greater"].Value.ToString() != " " ? dr.Cells["Sel_Greater"].Value.ToString() : "NULL");
                Less = (dr.Cells["Sel_Less"].Value.ToString() != " " ? dr.Cells["Sel_Less"].Value.ToString() : "NULL");
                Get_Null_Records = (dr.Cells["Sel_Null_Recs"].Value.ToString() != " " ? dr.Cells["Sel_Null_Recs"].Value.ToString() : "NULL");

                //EqualTo = (dr.Cells["Sel_Equalto"].Value.ToString() != " " ? dr.Cells["Sel_Equalto"].Value.ToString() : "NULL");
                //Greater = (dr.Cells["Sel_Greater"].Value.ToString() != " " ? dr.Cells["Sel_Greater"].Value.ToString() : "NULL");
                //Less = (dr.Cells["Sel_Less"].Value.ToString() != " " ? dr.Cells["Sel_Less"].Value.ToString() : "NULL");

                Column_name = dr.Cells["Sel_Org_ColNmae"].Value.ToString();

                //if (dr.Cells["Sel_Col_Format_Type"].Value.ToString().Trim() == "T" && dr.Cells["Sel_Datatype"].Value.ToString().Trim() == "Date")
                //{
                //    EqualTo = (EqualTo != "NULL" ? Convert.ToDateTime(EqualTo).ToShortDateString() : EqualTo);
                //    NotEqualTo = (NotEqualTo != "NULL" ? Convert.ToDateTime(NotEqualTo).ToShortDateString() : NotEqualTo);
                //    Greater = (Greater != "NULL" ? Convert.ToDateTime(Greater).ToShortDateString() : Greater);
                //    Less = (Less != "NULL" ? Convert.ToDateTime(Less).ToShortDateString() : Less);
                //}

                if (Operation == "Generate_Rep")
                {
                    if (Tml_Sel_Table_Name == "ADDCUST" && EqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_Equal = "";
                        foreach (CustRespEntity ent in Addcust_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && EqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_Equal += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_Equal = Tmp_Equal.Substring(0, Tmp_Equal.Length - 1);
                        EqualTo = Tmp_Equal;
                    }

                    if (Tml_Sel_Table_Name == "ADDCUST" && NotEqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_NotEqual = "";
                        foreach (CustRespEntity ent in Addcust_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && NotEqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_NotEqual += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_NotEqual = Tmp_NotEqual.Substring(0, Tmp_NotEqual.Length - 1);
                        NotEqualTo = Tmp_NotEqual;
                    }

                    if (Tml_Sel_Table_Name == "PRESRESP" && EqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_Equal = "";
                        foreach (CustRespEntity ent in Presresp_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && EqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_Equal += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_Equal = Tmp_Equal.Substring(0, Tmp_Equal.Length - 1);
                        EqualTo = Tmp_Equal;
                    }

                    if (Tml_Sel_Table_Name == "PRESRESP" && NotEqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_NotEqual = "";
                        foreach (CustRespEntity ent in Presresp_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && NotEqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_NotEqual += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_NotEqual = Tmp_NotEqual.Substring(0, Tmp_NotEqual.Length - 1);
                        NotEqualTo = Tmp_NotEqual;
                    }

                    if (Tml_Sel_Table_Name == "SERCUST" && EqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_Equal = "";
                        foreach (CustRespEntity ent in SERCust_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && EqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_Equal += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_Equal = Tmp_Equal.Substring(0, Tmp_Equal.Length - 1);
                        EqualTo = Tmp_Equal;
                    }

                    if (Tml_Sel_Table_Name == "SERCUST" && NotEqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_NotEqual = "";
                        foreach (CustRespEntity ent in SERCust_Resp_List)
                        {
                            if (ent.ResoCode == Column_name && NotEqualTo.Trim().Contains("'" + ent.DescCode.Trim() + "'"))
                            {
                                Tmp_NotEqual += "'" + ent.RespDesc + "',";
                            }
                        }

                        Tmp_NotEqual = Tmp_NotEqual.Substring(0, Tmp_NotEqual.Length - 1);
                        NotEqualTo = Tmp_NotEqual;
                    }


                    if (Tml_Sel_Table_Name == "CASEMST" && (Column_name == "MST_PRESS_CAT") && EqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_Equal = "";
                        foreach (DataRow dr1 in Press_Cat.Rows)
                        {
                            if (EqualTo.Trim().Contains("'" + dr1["PREASSGRP_CODE"].ToString().Trim() + "'"))
                            {
                                Tmp_Equal += "'" + dr1["PREASSGRP_DESC"].ToString() + "',";
                            }
                        }

                        if (Tmp_Equal.Length > 0)
                        {
                            Tmp_Equal = Tmp_Equal.Substring(0, Tmp_Equal.Length - 1);
                            EqualTo = Tmp_Equal;
                        }
                    }


                    if (Tml_Sel_Table_Name == "CASEMST" && (Column_name == "MST_PRESS_GRP") && EqualTo != "NULL" && Desc_Sw == '1')
                    {
                        string Tmp_Equal = "";
                        foreach (DataRow dr1 in Press_Grp.Rows)
                        {
                            if (EqualTo.Trim().Contains("'" + dr1["PREASSGRP_SUBCODE"].ToString().Trim() + "'"))
                            {
                                Tmp_Equal += "'" + dr1["PREASSGRP_DESC"].ToString() + "',";
                            }
                        }

                        if (Tmp_Equal.Length > 0)
                        {
                            Tmp_Equal = Tmp_Equal.Substring(0, Tmp_Equal.Length - 1);
                            EqualTo = Tmp_Equal;
                        }
                    }


                }




                if (Column_name == "ENRL_DATE" && EqualTo == "NULL")
                    EqualTo = DateTime.Today.ToShortDateString();

                //Column_name = (dr.Cells["Sel_Col_AgyTabCode"].Value.ToString() == " " ? Tmp_Col_Nmae : "(SELECT AGYS_DESC FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "' AND AGYS_CODE = " + Tmp_Col_Nmae + ") AS " + Tmp_Col_Nmae);

                //Need_Casting = (dr.Cells["Sel_Col_Format_Type"].Value.ToString().Trim() == "SSN#" ? "N" : "NULL");
                Format_Type = dr.Cells["Sel_Col_Format_Type"].Value.ToString().Trim();
                switch (dr.Cells["Sel_Col_Format_Type"].Value.ToString().Trim())
                {
                    //case "S":
                    case "N":
                    case "E": if (Column_name != "MST_PHONE") Need_Casting = "N"; break;

                    case "L":
                    case "D": Need_Casting = (Desc_Sw == '1' ? "C" : "NULL"); break;

                    case "Z":
                        Need_Casting = "C"; break;
                    default: Need_Casting = "NULL"; break;
                }
                if (AgyCode == "ADCST" || AgyCode == "PREAS" || AgyCode == "SRCST")
                    Need_Casting = "NULL";

                if (AgyCode != "NULL" && Need_Casting == "NULL")
                {
                    switch (AgyCode)
                    {
                        //case "HIEAG":
                        //case "HIEDE":
                        //case "HIEPR": //Need_Casting = (Desc_Sw == '1' ? "C" : "NULL"); break;
                        //case "WORKR":
                        //case "SITES":
                        case "CDIFF": if (Cb_Use_DIFF.Checked) Need_Casting = "C"; break;
                        //case "MSTAD": 
                        //case "STFMS":
                        //case "STFMS":
                        case "ADCST":
                        case "PREAS":
                        case "SRCST":
                        case "SERVS":
                        case "DLMTD":
                        case "ZIPLS": Need_Casting = "C"; break;
                        //default: Need_Casting = "N"; break;
                        default: Need_Casting = "NULL"; break;
                    }

                    if (AgyCode.Substring(0, 2) == "DL" && (AgyCode.Substring(4, 1) == "," || AgyCode.Substring(4, 1) == "|"))
                        Need_Casting = "C";
                }

                switch (Column_name)
                {
                    case "Name":
                    case "Address": Need_Casting = "C"; break;
                    case "MST_PHONE": Need_Casting = "C"; break;
                    case "SNP_SSNO": Need_Casting = "C"; break;
                    case "Current_Age": Need_Casting = "C"; break;
                    case "Birth_Date_Month": Need_Casting = "C"; break;
                    case "EnrlLumpedSite": Need_Casting = "C"; break;
                    case "ATTN_Month": Need_Casting = "C"; break;
                    case "TMSAPPT_TIME": Need_Casting = "C"; break;
                    //case "MST_SER": Need_Casting = "C"; break;

                    case "MST_FAMILY_ID": Need_Casting = "C"; break;
                    case "MST_STATE":
                    case "MST_CITY":
                    case "MST_STREET":
                    case "MST_SUFFIX":
                    case "MST_HN":
                    case "MST_APT":
                    case "MST_FLR":
                    case "MST_ZIP":
                    case "MST_DIRECTION": if (Cb_Use_DIFF.Checked) Need_Casting = "C"; break;
                }


                if (Column_name == "CASEMSOBO_Name")
                {
                    bool Act_Selected = false, MS_Selected = false;
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        if (Sel_Col_Entity.Table_name == "CASEACT")
                            Act_Selected = true;

                        if (Sel_Col_Entity.Table_name == "CASEMS")
                            MS_Selected = true;

                    }

                    if (Act_Selected && MS_Selected)
                        Need_Casting = "NULL";

                    if (!Act_Selected && MS_Selected)
                        Need_Casting = "C";


                }

                if (Column_name == "CASEACTOBO_Name")
                {
                    bool Act_Selected = false, MS_Selected = false;
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        if (Sel_Col_Entity.Table_name == "CASEACT")
                            Act_Selected = true;

                        if (Sel_Col_Entity.Table_name == "CASEMS")
                            MS_Selected = true;

                    }

                    if (Act_Selected && MS_Selected)
                        Need_Casting = "NULL";

                    if (Act_Selected && !MS_Selected)
                        Need_Casting = "C";


                }


                foreach (ADHOCFLSEntity Entity in Master_Table_List)
                {
                    if (Entity.Table_Name == dr.Cells["Sel_TableName"].Value.ToString())
                    {
                        Table_Subscript = Entity.Column_Perfix;
                        break;
                    }
                }


                Custom_Select = (Need_Casting == "C" ? Get_Custom_Select_Query(AgyCode, dr.Cells["Sel_TableName"].Value.ToString(), Column_name, Desc_Sw) : "NULL");

                Needto_Select_Agency = false;
                if (Operation == "Generate_Rep")
                {
                    switch (AgyCode)
                    {
                        case "SITES":
                        //case "STFMS": COMMENTED BY SUDHEER ON 11/08/2019 for STAFF postion not getting data
                        case "SERVS":Needto_Select_Agency = true; break;   
                    }

                    if (Needto_Select_Agency && AgyCode != "SITES")
                    {
                        EqualTo = (EqualTo != "NULL" ? Replace_Agency_Code(EqualTo) : "NULL");
                        NotEqualTo = (NotEqualTo != "NULL" ? Replace_Agency_Code(NotEqualTo) : "NULL");
                    }
                }

                Sort_Order = (dr.Cells["Col_Sort_Order"].Value.ToString().Trim());
                Tmp_Table_Id = dr.Cells["Sel_Table_ID"].Value.ToString();
                if (Column_name == "CASEMSOBO_CLID" || Column_name == "CASEMSOBO_FAM_SEQ" || Column_name == "CASEMSOBO_Name")
                    Tml_Sel_Table_Name = "CASEMSOBO";

                if (Column_name == "CAOBO_CLID" || Column_name == "CAOBO_FAM_SEQ" || Column_name == "CASEACTOBO_Name"
                    || Column_name == "CAOBO_SDISTRICT" || Column_name == "CAOBO_STATUS" || Column_name == "CAOBO_COMPDATE" || Column_name == "CAOBO_GIFT1" || Column_name == "CAOBO_GIFT2" || Column_name == "CAOBO_GIFT3" || Column_name == "CAOBO_GIFTCARD" || Column_name== "CAOBO_BEDSIZE"
                    || Column_name == "CAOBO_AIRMATTRESS" || Column_name == "CAOBO_QUANTITY" || Column_name == "CAOBO_TRANSUOM" || Column_name == "CAOBO_UNITPRICE" || Column_name == "CAOBO_AMOUNT" || Column_name == "CAOBO_DESC" || Column_name == "CAOBO_RECPINAME" || Column_name == "CAOBO_CLOTHSIZE" || Column_name == "CAOBO_SHOESIZE")
                    Tml_Sel_Table_Name = "CAOBO";

                //if (dr.Cells["Sel_TableName"].Value.ToString() == "STAFFMST" &&
                //    (Column_name == "STF_DEPT" || Column_name == "STF_PROGRAM"))
                //    EqualTo = "  ";

                if (Column_name == "ENRL_DATE")
                {
                    if (EqualTo != Greater && Greater != "NULL" && EqualTo != "NULL")
                    {
                        str.Append("<Row TABLEID = \"" + Tmp_Table_Id + "\" TABLE = \"" + Tml_Sel_Table_Name + "\" COLNAME = \"" + "ENRL_TODATE" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + dr.Cells["Sel_Datatype"].Value +
                                   "\" DISPLAY = \"" + Disp_Sw + "\" COUNT = \"" + Count_Sw + "\" SORT = \"" + Sort_Order + "\" BREAK = \"" + Break_Sw + "\" EQULATO = \"" + Greater + "\" NOTEQULATO = \"" + NotEqualTo +
                                   "\" LESSTHAN = \"" + Less + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + AgyCode + "\" DISPPOS = \"" + Disp_Position + "\" CASTTO = \"" + Need_Casting +
                                   "\" FORMATTYPE= \"" + Format_Type + "\" CUSTOMSELECT= \"" + Custom_Select + "\" COLUMNPREFIX= \"" + Table_Subscript + "\" />");

                        Greater = "NULL";
                    }

                    if ((EqualTo == Greater && Greater != "NULL" && EqualTo != "NULL") || (Greater != "NULL" && EqualTo == "NULL"))
                    {
                        EqualTo = Greater;
                        Greater = "NULL";
                    }
                }

                str.Append("<Row TABLEID = \"" + Tmp_Table_Id + "\" TABLE = \"" + Tml_Sel_Table_Name + "\" COLNAME = \"" + Column_name + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + dr.Cells["Sel_Datatype"].Value +
                           "\" DISPLAY = \"" + Disp_Sw + "\" COUNT = \"" + Count_Sw + "\" SORT = \"" + Sort_Order + "\" BREAK = \"" + Break_Sw + "\" EQULATO = \"" + EqualTo + "\" NOTEQULATO = \"" + NotEqualTo +
                           "\" LESSTHAN = \"" + Less + "\"  GREATERTHAN = \"" + Greater + "\" ACGYCODE = \"" + AgyCode + "\" DISPPOS = \"" + Disp_Position + "\" CASTTO = \"" + Need_Casting +
                           "\" FORMATTYPE= \"" + Format_Type + "\" CUSTOMSELECT= \"" + Custom_Select + "\" COLUMNPREFIX= \"" + Table_Subscript + "\" IS_NULL= \"" + Get_Null_Records + "\" />");


                //Table_Subscript

                switch (Column_name)
                {
                    case "MST_AGENCY": MST_Agency_Selected = true; break;
                    case "ENRL_DATE": ENRL_Asof_Selected = true; break;
                    case "CASEMSOBO_Name":
                    case "CASEMSOBO_FAM_SEQ":
                    case "CASEMSOBO_CLID": CASEMSobo_Selected = true; break;
                    case "CASEACTOBO_Name":
                    case "CAOBO_FAM_SEQ":
                    case "CAOBO_CLID":
                    case "CAOBO_AMOUNT":
                    case "CAOBO_DESC":
                    case "CAOBO_QUANTITY":
                    case "CAOBO_SDISTRICT":
                    case "CAOBO_STATUS":
                    case "CAOBO_COMPDATE":
                    case "CAOBO_UNITPRICE":
                    case "CAOBO_TRANSUOM":
                    case "CAOBO_RECPINAME":
                    case "CAOBO_GIFT1":
                    case "CAOBO_GIFT2":
                    case "CAOBO_GIFT3":
                    case "CAOBO_GIFTCARD":
                    case "CAOBO_BEDSIZE":
                    case "CAOBO_AIRMATTRESS":
                    case "CAOBO_CLOTHSIZE":
                    case "CAOBO_SHOESIZE": CAobo_Selected = true; break;
                    case "SNP_CLIENT_ID": SNP_CLID_Selected = true; break;
                    case "MST_FAMILY_ID": MST_FAMID_Selected = true; break;
                }

                //if (Column_name == "MST_AGENCY")
                //    MST_Agency_Selected = true;

                //if (Column_name == "ENRL_DATE")
                //    ENRL_Asof_Selected = true;

                //if (Column_name.Contains("CASEMSOBO"))
                //    CASEMSobo_Selected = true;


                switch (AgyCode)
                {
                    case "SITES":
                    case "SERVS":
                    case "STFMS":
                    case "STFTR":
                        Needto_Select_Agency = true; break;
                }


                Need_Casting = Custom_Select = AgyType = "NULL";
                Need_Casting = (Count_Sw == '1' ? "C" : "NULL");
                if (AgyCode != "NULL")
                {
                    Get_Summary_For = (Desc_Sw == '1' ? "D" : "C");
                    Need_Casting = "C";
                    if ((int.TryParse(AgyCode, out Tmp_Compare_Int)) ||
                        ((int.TryParse(AgyCode.Substring(1, 4), out Tmp_Compare_Int)) && AgyCode.Substring(0, 1) == "S") ||
                         (AgyCode == "ADCST") || (AgyCode == "PREAS") || (AgyCode == "SRCST"))
                    {
                        if (AgyCode == "PREAS")
                        {
                            AgyType = (AgyCode == "PREAS" ? Column_name : AgyCode);
                            AgyCode = (AgyCode == "PREAS" ? "PREAS" : "AGYTB");
                        }
                        else if (AgyCode == "SRCST")
                        {
                            AgyType = (AgyCode == "SRCST" ? Column_name : AgyCode);
                            AgyCode = (AgyCode == "SRCST" ? "SRCST" : "AGYTB");
                        }
                        else
                        {
                            AgyType = (AgyCode == "ADCST" ? Column_name : AgyCode);
                            AgyCode = (AgyCode == "ADCST" ? "ADCST" : "AGYTB");
                        }
                        Need_Casting = "NULL";
                    }
                }

                AgyCode = (AgyCode == "CSTRS" ? "NULL" : AgyCode);
                Custom_Select = (Need_Casting == "C" ? Get_Custom_SUmmary_Query(AgyCode, dr.Cells["Sel_TableName"].Value.ToString(), Column_name, Desc_Sw, dr.Cells["Sel_Count_SW"].Value.ToString()) : "NULL");
                Col_Disp_Name = dr.Cells["Sel_Col_DispName"].Value.ToString();

                if (Count_Sw == '1')
                {
                    str_Summary.Append("<Row  TABLEID = \"" + Tmp_Table_Id + "\" TABLE = \"" + dr.Cells["Sel_TableName"].Value.ToString() + "\" COLNAME = \"" + Column_name + "\" DISPCOLNAME = \"" + Col_Disp_Name + "\" DATATYPE = \"" + dr.Cells["Sel_Datatype"].Value +
                        "\"  ACGYCODE = \"" + ("RSP61, RSP62,SAR62, TASKS ".Contains(AgyCode) ? "NULL" : AgyCode) + "\"  ACGYTYPE = \"" + AgyType + "\"  SUMMARYFOR = \"" + Get_Summary_For + "\" CASTTO = \"" + Need_Casting + "\" CUSTOMSELECT= \"" + Custom_Select + "\" COLUMNPREFIX= \"" + Table_Subscript + "\"  />");
                    Summary_Sw = true;
                }


                if (Needto_Select_Agency && Desc_Sw == '1')
                {// If site column of a table is selected, then it's Agency is must to get related site Description from CASESITE
                    Site_Agency_Selected = false;
                    foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                    {
                        if (Sel_Col_Entity.Column_Name == (Table_Subscript + "_AGENCY"))
                        {
                            Site_Agency_Selected = true; break;
                        }
                    }

                    if (!Site_Agency_Selected)
                    {
                        str.Append("<Row TABLEID = \"" + Tmp_Table_Id + "\" TABLE = \"" + dr.Cells["Sel_TableName"].Value.ToString() + "\" COLNAME = \"" + Table_Subscript + "_AGENCY" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Text" +
                                   "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                   "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "A" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                    }
                }
            }


            if (!ENRL_Asof_Selected)
            {// If any of CASEENRL table Columns selected, Without having AsoF date Selected
                Site_Agency_Selected = false;
                foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                {
                    if (Sel_Col_Entity.Table_name == "CASEENRL")
                    {
                        Needto_Select_Asof = true; break;
                    }
                }

                if (Needto_Select_Asof)
                {
                    str.Append("<Row TABLEID = \"" + (Privileges.ModuleCode == "02" ? "14" : "15") + "\" TABLE = \"" + "CASEENRL" + "\" COLNAME = \"" + "ENRL_DATE" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Date" +
                               "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + DateTime.Today.ToShortDateString() + "\" NOTEQULATO = \"" + "NULL" +
                               "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                }
            }

            if (CASEMSobo_Selected)
            {// If any of CASEMSOBO table Columns selected, Without having CASEMS Columns
                CASEMSobo_Selected = false;
                foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                {
                    if (Sel_Col_Entity.Table_name == "CASEMS" && !Sel_Col_Entity.Column_Name.Contains("CASEMSOBO"))
                    {
                        CASEMSobo_Selected = true; break;
                    }
                }

                if (!CASEMSobo_Selected)
                {
                    str.Append("<Row TABLEID = \"" + "10" + "\" TABLE = \"" + "CASEMS" + "\" COLNAME = \"" + "CASEMS_AGENCY" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Date" +
                               "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                               "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                }
            }

            if (CAobo_Selected)
            {// If any of CASEMSOBO table Columns selected, Without having CASEMS Columns
                CAobo_Selected = false;
                foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                {
                    if (Sel_Col_Entity.Table_name == "CASEACT" && !Sel_Col_Entity.Column_Name.Contains("CAOBO"))
                    {
                        CAobo_Selected = true; break;
                    }
                }

                if (!CAobo_Selected)
                {
                    str.Append("<Row TABLEID = \"" + "10" + "\" TABLE = \"" + "CASEACT" + "\" COLNAME = \"" + "CASEACT_AGENCY" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Date" +
                               "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                               "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                }
            }


            if (!SNP_CLID_Selected && ((ListItem)Cmb_Dat_Filter.SelectedItem).Value.ToString() == "C") // Yeswanth 
            {// If any of CASEMSOBO table Columns selected, Without having CASEMS Columns
                if (CASEACTTab_Sel_SW || MSTTab_Sel_Sw)
                {
                    if (MSTTab_Sel_Sw) //sudheer
                    {
                        str.Append("<Row TABLEID = \"" + "06" + "\" TABLE = \"" + "CASESNP" + "\" COLNAME = \"" + "SNP_CLIENT_ID" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Numeric" +
                                   "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                   "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                    }
                    else
                    {
                        str.Append("<Row TABLEID = \"" + "06" + "\" TABLE = \"" + "CASESNP" + "\" COLNAME = \"" + "SNP_CLIENT_ID" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Numeric" +
                                   "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                   "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");

                        //Sudheer
                        str.Append("<Row TABLEID = \"" + "05" + "\" TABLE = \"" + "CASEMST" + "\" COLNAME = \"" + "MST_APP_NO" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Numeric" +
                                   "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                   "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                    }
                }
                else
                {
                    str.Append("<Row TABLEID = \"" + "06" + "\" TABLE = \"" + "CASESNP" + "\" COLNAME = \"" + "SNP_CLIENT_ID" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Numeric" +
                                   "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                   "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                }
            }

            if (((ListItem)Cmb_Dat_Filter.SelectedItem).Value.ToString() == "F") // Yeswanth 
            {
                if (!MST_FAMID_Selected)
                {
                    str.Append("<Row TABLEID = \"" + "05" + "\" TABLE = \"" + "CASEMST" + "\" COLNAME = \"" + "MST_FAMILY_ID" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Numeric" +
                               "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                               "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "D" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");
                }

                if (Operation == "Generate_Rep")
                {
                    List<ListItem> listItem = new List<ListItem>();
                    listItem.Add(new ListItem("MST_AGENCY", "MST_AGENCY"));
                    listItem.Add(new ListItem("MST_DEPT", "MST_DEPT"));
                    listItem.Add(new ListItem("MST_PROGRAM", "MST_PROGRAM"));
                    listItem.Add(new ListItem("MST_YEAR", "MST_YEAR"));
                    listItem.Add(new ListItem("MST_APP_NO", "MST_APP_NO"));

                    bool Proc_Col_Selected = false;
                    foreach (ListItem Lst in listItem)
                    {
                        Proc_Col_Selected = false;
                        foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                        {
                            if (Sel_Col_Entity.Column_Name == Lst.Value.ToString())
                            {
                                Proc_Col_Selected = true; break;
                            }
                        }

                        if (!Proc_Col_Selected)
                        {
                            str.Append("<Row TABLEID = \"" + "05" + "\" TABLE = \"" + "CASEMST" + "\" COLNAME = \"" + Lst.Value.ToString() + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Text" +
                                       "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
                                       "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "A" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");

                        }
                    }
                }
            }


            //if (!MST_Agency_Selected && Needto_Select_Agency)
            //{
            //    str.Append("<Row TABLEID = \"" + "05" + "\" TABLE = \"" + "CASEMST" + "\" COLNAME = \"" + "MST_AGENCY" + "\" ORDINAL = \"" + 1 + "\" DATATYPE = \"" + "Text" +
            //               "\" DISPLAY = \"" + "0" + "\" COUNT = \"" + "0" + "\" SORT = \"" + "0" + "\" BREAK = \"" + "0" + "\" EQULATO = \"" + "NULL" + "\" NOTEQULATO = \"" + "NULL" +
            //               "\" LESSTHAN = \"" + "NULL" + "\"  GREATERTHAN = \"" + "NULL" + "\" ACGYCODE = \"" + "NULL" + "\" DISPPOS = \"" + "0" + "\" CASTTO = \"" + "NULL" + "\" FORMATTYPE= \"" + "A" + "\" CUSTOMSELECT= \"" + "NULL" + "\" />");

            //}

            str.Append("</Rows>");

            XML_Strings[0] = str.ToString();

            XML_Strings[1] = null;
            if (Summary_Sw)
            {
                str_Summary.Append("</Rows>");
                XML_Strings[1] = str_Summary.ToString();
            }


            //convertStringToDataTable(XML_Strings[0]);
            return XML_Strings;
        }

        private string Replace_Agency_Code(string Str_To_Replace)
        {
            string Replaced_Str = "NULL";

            string[] Replace_Str_Arr = Regex.Split(Str_To_Replace.ToString().Trim(), ",");
            if (Replace_Str_Arr.Length > 0)
            {
                Replaced_Str = string.Empty;
                foreach (string str in Replace_Str_Arr)
                    Replaced_Str += " '" + str.Substring(3, (str.Length - 3)) + ",";

                Replaced_Str = Replaced_Str.Substring(0, (Replaced_Str.Length - 1));
            }

            return Replaced_Str;
        }

        private void Crit_SelCol_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Crit_SelCol_Grid.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = Crit_SelCol_Grid.CurrentCell.ColumnIndex;
                int RowIdx = Crit_SelCol_Grid.CurrentCell.RowIndex;

                if (!ColGrid_Header_Clicked)
                {
                    string Att_List = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString(),
                           Count_Sw = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Count_SW"].Value.ToString(),
                           Col_Name = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString();

                    if (Col_Name == "ENRL_DATE")
                        return;

                    if (Col_Name != "ENRL_DATE")
                    {
                        switch (e.ColumnIndex)
                        {
                            case 4: Check_Uncheck_Related_Attribute("Sel_Display", 0); break;
                            case 5:
                                if (Att_List.Substring(0, 1) == "Y" && (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Format_Type"].Value.ToString().Trim() == "D" ||
                                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Format_Type"].Value.ToString().Trim() == "L" || Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Format_Type"].Value.ToString().Trim() == "C"))
                                    Check_Uncheck_Related_Attribute("Sel_Desc", 1);
                                else
                                    AlertBox.Show("Description Selection Restricted for Selected Column.", MessageBoxIcon.Warning);
                                break;
                            case 6:
                                if (Count_Sw == "Y" || Count_Sw == "S")
                                    Check_Uncheck_Related_Attribute("Sel_Count", 2);
                                else
                                    AlertBox.Show("Summary Selection Restricted for Selected Column.", MessageBoxIcon.Warning);
                                break;
                            case 7:
                                if (Att_List.Substring(0, 1) == "Y")
                                    Check_Uncheck_Related_Attribute("Sel_Sort", 3);
                                //Prepare_Sot_Priority_Columns();
                                break;
                            case 8:
                                if (Att_List.Substring(0, 1) == "Y")
                                    Check_Uncheck_Related_Attribute("Sel_Break", 4); break;
                        }
                    }

                    if (Col_Name == "Name") Pb_Edit.Visible = false; else Pb_Edit.Visible = true;
                }
                ColGrid_Header_Clicked = false;
            }
        }


        private string Get_Custom_Select_Query(string AgyCode, string Table_Name, string Column_Name, char Description_SW)
        {
            string Custom_Query = "NULL";
            string Temp_Agy_Col, Temp_Dept_Col, Temp_Prog_Col;
            string Table_Subscript = Table_Name.Substring(4, Table_Name.Length - 4);
            int Comma_Interval = 1;

            switch (Column_Name)
            {
                case "MST_INCOME_TYPES": Comma_Interval = 2; break;
                case "STF_POSITIONS": Comma_Interval = 3; break;
            }

            //switch (Table_Name)
            //{
            //    case "CASEINCOME": Table_Subscript = "INCOME"; break;
            //    case "CASEACT": Table_Subscript = "CASEACT"; break;
            //    case "CASECONT": Table_Subscript = "CASECONT"; break;
            //}


            foreach (ADHOCFLSEntity Entity in Master_Table_List)
            {
                if (Entity.Table_Name == Table_Name)
                {
                    Table_Subscript = Entity.Column_Perfix;
                    break;
                }
            }

            Temp_Agy_Col = Table_Subscript + "_AGENCY";
            Temp_Dept_Col = Table_Subscript + "_DEPT";
            Temp_Prog_Col = "_PROGRAM";
            //Temp_Prog_Col = Table_Subscript + (Table_Name == "CHLDMEDI" ? "_PROG" : "_PROGRAM");
            if (Table_Name == "CHLDMEDI" || Table_Name == "CHLDEMER" || Table_Name == "CASEENRL") //|| Table_Name == "CHLDATTN"
                Temp_Prog_Col = "_PROG";

            string Serv_Inq = string.Empty;
            Serv_Inq = BaseForm.BaseAgencyControlDetails.ServinqCaseHie.Trim();

            switch (AgyCode)
            {

                //CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();


                case "WORKR":
                    if (CAseWorkerr_NameFormat == "1")
                        Custom_Query = "(SELECT DISTINCT TOP 1 LTRIM(ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  ' + ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')) FROM PASSWORD WHERE PWR_CASEWORKER = " + Column_Name + ") AS " + Column_Name;
                    else
                        Custom_Query = "(SELECT DISTINCT TOP 1 LTRIM(ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+',  ' + ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')) FROM PASSWORD WHERE PWR_CASEWORKER = " + Column_Name + ") AS " + Column_Name;
                    break;
                //case "HIEAG": Custom_Query = "(SELECT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' AND HIE_AGENCY = " + Column_Name + ") AS " + Column_Name + "_DESC"; break;
                case "HIEAG": Custom_Query = "(SELECT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' AND HIE_AGENCY = " + Column_Name + ") AS " + Column_Name; break;
                case "HIEDE": Custom_Query = "(SELECT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_PROGRAM = '  ' AND HIE_AGENCY = " + Temp_Agy_Col + " AND HIE_DEPT = " + Column_Name + ") AS " + Column_Name; break;
                case "HIEPR": Custom_Query = "(SELECT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_AGENCY = " + Temp_Agy_Col + " AND HIE_DEPT = " + Temp_Dept_Col + " AND HIE_PROGRAM = " + Column_Name + ") AS " + Column_Name; break;

                case "ALLPR": Custom_Query = "(SELECT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_AGENCY = SUBSTRING(" + Column_Name + ",1,2) AND HIE_DEPT = SUBSTRING(" + Column_Name + ",3,2) AND HIE_PROGRAM = SUBSTRING(" + Column_Name + ",5,2)) AS " + Column_Name; break;
                case "SPHIE": Custom_Query = "(SELECT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_AGENCY = SUBSTRING(" + Column_Name + ",1,2) AND HIE_DEPT = SUBSTRING(" + Column_Name + ",3,2) AND HIE_PROGRAM = SUBSTRING(" + Column_Name + ",5,2)) AS " + Column_Name; break;
                case "MATRX": Custom_Query = "(SELECT RTRIM(MATDEF_DESC) FROM MATDEF WHERE MATDEF_MAT_CODE = " + Column_Name + " AND MATDEF_SCL_CODE = 0) AS " + Column_Name; break;
                case "SCALS": Custom_Query = "(SELECT RTRIM(MATDEF_DESC) FROM MATDEF WHERE MATDEF_MAT_CODE = MATASMT_MAT_CODE AND MATDEF_SCL_CODE = " + Column_Name + ") AS " + Column_Name; break;

                case "PRREP": Custom_Query = "(SELECT RTRIM(AGYR_REP_FNAME)+' '+RTRIM(AGYR_REP_LNAME) FROM AGCYREP WHERE AGYR_PART_CODE=PREF_CODE AND AGYR_REP_CODE=" + Column_Name + " ) AS " + Column_Name; break;
                case "PRSER": Custom_Query = "(SELECT RTRIM(AGYS_SERVICE) FROM AGCYSERVICES WHERE AGYS_PART_CODE=PREF_CODE AND AGYS_CATEGORY=PREF_CATEGORY AND  AGYS_SER_CODE=" + Column_Name + " ) AS " + Column_Name; break;



                case "ZIPLS":
                    {
                        if (Column_Name == "MST_ZIP")
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(ISNULL(TEST.MST_ZIP, '0') AS VARCHAR(5)),5)+'-'+ RIGHT('0000'+ CAST(ISNULL(TEST.MST_ZIPPLUS, '0') AS VARCHAR(4)), 4)) AS " + Column_Name;
                        else
                            if (Column_Name == "TMSAPPT_ZIP1")
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(TMSAPPT_ZIP1 AS VARCHAR(5)),5)) AS " + Column_Name;
                        else if (Column_Name == "STF_ZIP")
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(STF_ZIP AS VARCHAR(5)),5)) AS " + Column_Name;
                        else if (Column_Name == "CASEREF_ZIP")
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(CASEREF_ZIP AS VARCHAR(5)),5)) AS " + Column_Name;
                        else if (Column_Name == "AGYP_Zip") //ADDED BY SUDHEER ON 02/18/2021 
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(AGYP_Zip AS VARCHAR(5)),5)) AS " + Column_Name;
                        else if (Column_Name == "AGYR_REP_Zip") //ADDED BY SUDHEER ON 02/18/2021 
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(AGYR_REP_Zip AS VARCHAR(5)),5)) AS " + Column_Name;
                        else if (Column_Name == "AGYS_ZIP") //ADDED BY SUDHEER ON 02/19/2021 
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(AGYS_ZIP AS VARCHAR(5)),5)) AS " + Column_Name;
                        else
                            Custom_Query = "(SELECT RIGHT('0000'+ CAST(ISNULL(TEST.MST_ZIP, '0') AS VARCHAR(5)),5)+'-'+ RIGHT('0000'+ CAST(ISNULL(TEST.MST_ZIPPLUS, '0') AS VARCHAR(4)), 4)) AS " + Column_Name;
                    }
                    break;

                //case "ZIPLS": Custom_Query = "(SELECT RIGHT('0000'+ CAST(MST_ZIP AS VARCHAR(5)),5)+'-'+ RIGHT('0000'+ CAST(MST_ZIPPLUS AS VARCHAR(4)), 4)) AS " + Column_Name; break;

                case "DLMTD": Custom_Query = "(SELECT dbo.[AppendCommainstring](" + Column_Name + " , " + Comma_Interval + ")) AS " + Column_Name; break;
                //case "MSTAD": Custom_Query = "(SELECT RTRIM(MST_HN)+' '+RTRIM(MST_STREET)+' '+RTRIM(MST_SUFFIX)+', '+RTRIM(MST_CITY)+' '+RTRIM(MST_STATE)+RTRIM(MST_ZIP)+ CASE WHEN (LEN(MST_ZIPPLUS) >0 AND MST_ZIPPLUS > 0) THEN '-' + CAST(MST_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEMST ADRS WHERE ADRS.MST_AGENCY+ADRS.MST_DEPT+ADRS.MST_PROGRAM+ADRS.MST_YEAR+ADRS.MST_APP_NO  = TEST.MST_AGENCY+TEST.MST_DEPT+TEST.MST_PROGRAM+TEST.MST_YEAR+TEST.MST_APP_NO) AS " + Column_Name; break;
                //case "CDIFF": Custom_Query = "(SELECT DIFF_" + Column_Name.Substring(4, Column_Name.Length - 4) + " FROM CASEDIFF WHERE DIFF_AGENCY+DIFF_DEPT+DIFF_PROGRAM+DIFF_YEAR+DIFF_APP_NO = MST_AGENCY+MST_DEPT+MST_PROGRAM+MST_YEAR+MST_APP_NO) AS " + Column_Name; break;

                case "STFMS": Custom_Query = "(SELECT RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END  FROM STAFFMST WHERE STF_AGENCY = ''+TEST.MST_AGENCY+'' AND STF_YEAR = ''+TEST.MST_YEAR+'' AND STF_CODE = ''+" + Column_Name + "+'') AS " + Column_Name; break;
                //case "STFMS": Custom_Query = "(SELECT RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END  FROM STAFFMST WHERE STF_AGENCY = " + Temp_Agy_Col + " AND STF_CODE = " + Column_Name + ") AS " + Column_Name; break;
                //SELECT RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END  FROM STAFFMST WHERE STF_AGENCY = 02 AND STF_CODE = 00011594

                case "STFTR": Custom_Query = "(SELECT STFTR_NAME FROM STFTRAIN WHERE STFTR_AGENCY = SPDB_AGENCY AND STFTR_DEPT = SPDB_DEPT AND STFTR_PROG = SPDB_PROGRAM AND STFTR_YEAR = SPDB_YEAR AND STFTR_ID = " + Column_Name + ") AS " + Column_Name; break;

                case "SERVS":
                    if (string.IsNullOrEmpty(SelServices_Inq.Trim()) || SelServices_Inq == "NULL") SelServices_Inq = "NULL"; else SelServices_Inq = "'" + SelServices_Inq + "'";
                    Custom_Query = "(SELECT [dbo].[Adhoc_Get_MSTSER](TEST.MST_AGENCY, TEST.MST_DEPT,TEST.MST_PROGRAM, TEST.MST_YEAR,TEST.MST_APP_NO , " + SelServices_Inq + " ," + Serv_Inq + " ," + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name; break;
                //case "SERVS": Custom_Query = "(SELECT [dbo].[Adhoc_Get_MSTSER](TEST.MST_AGENCY, TEST.MST_DEPT,TEST.MST_PROGRAM, TEST.MST_YEAR,TEST.MST_APP_NO , NULL) AS " + Column_Name; break;
                //case "SERVS": Custom_Query = "(SELECT [dbo].[Adhoc_Get_MSTSER](MST_AGENCY, MST_DEPT,MST_PROGRAM, MST_YEAR,MST_APP_NO , NULL )) AS " + Column_Name; break;

                //ADDED BY SUDHEER ON 06/02/2022
                case "CMBDC": Custom_Query = "(SELECT RTRIM(BDC_DESCRIPTION) FROM CMBDC WHERE BDC_ID = " + Column_Name + " ) AS " + Column_Name; break;

                case "REFRL": Custom_Query = "(SELECT RTRIM(CASEREF_NAME1) FROM CASEREF WHERE CASEREF_CODE = " + Column_Name + " ) AS " + Column_Name; break;

                case "PARTN": Custom_Query = "(SELECT RTRIM(AGYP_NAME) FROM AGCYPART WHERE AGYP_CODE = " + Column_Name + " ) AS " + Column_Name; break;

                case "SPLAN": Custom_Query = "(SELECT RTRIM(sp0_description) FROM CASESP0 WHERE sp0_servicecode = " + Column_Name + " ) AS " + Column_Name; break;

                case "CACTI": Custom_Query = "(SELECT RTRIM(CA_DESC) FROM CAMAST WHERE CA_CODE = " + Column_Name + " ) AS " + Column_Name; break;

                case "MSTON": Custom_Query = "(SELECT RTRIM(MS_DESC) FROM MSMAST WHERE RTRIM(MS_CODE) = " + Column_Name + " ) AS " + Column_Name; break;

                case "CST62": Custom_Query = "(SELECT RTRIM(CUST_DESC) FROM CUSTFLDS CST62 WHERE CST62.CUST_CODE = " + Column_Name + " ) AS " + Column_Name; break;

                //ADDED BY SUDHEER ON 05/25/2020
                case "SAL62": Custom_Query = "(SELECT RTRIM(SALQ_DESC) FROM SALQUES CST62 WHERE CST62.SALQ_ID = " + Column_Name + " ) AS " + Column_Name; break;

                case "CST61": Custom_Query = "(SELECT RTRIM(CUST_DESC) FROM CUSTFLDS CST61 WHERE CST61.CUST_CODE = " + Column_Name + " ) AS " + Column_Name; break;

                //case "RSP62": Custom_Query = "ISNULL((SELECT RTRIM(RSP_DESC) FROM CUSTRESP CST62 WHERE CST62.RSP_SCR_CODE = 'CASE0063' AND CST62.RSP_CUST_CODE = " + Column_Name + " ), " + Column_Name + ") AS " + Column_Name; break;

                //case "RSP61": Custom_Query = "ISNULL((SELECT RTRIM(RSP_DESC) FROM CUSTRESP CST61 WHERE CST61.RSP_SCR_CODE = 'CASE0061' AND CST61.RSP_CUST_CODE = " + Column_Name + " ), " + Column_Name + ") AS " + Column_Name; break;

                case "SITES": Custom_Query = "(SELECT TOP 1 RTRIM(SITE_NAME) FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' AND SITE_AGENCY = " + Temp_Agy_Col + " AND SITE_NUMBER = " + Column_Name + " ) AS " + Column_Name; break;

                case "TASKS":
                    if (Table_Name == "HLSMEDI")
                        Custom_Query = "(SELECT RTRIM(HLSTRCK_TASK_DESC) FROM HLSTRCK WHERE HLSTRCK_COMPONENT = '0000' AND HLSTRCK_TASK = " + Column_Name + " ) AS " + Column_Name;
                    else Custom_Query = "(SELECT RTRIM(TRCK_TASK_DESC) FROM CHLDTRCK WHERE TRCK_COMPONENT = '0000' AND TRCK_TASK = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "RSP62":
                case "RSP61":
                case "CSTRS":
                    string Code_Col = Column_Name.Replace("VALUE", "CODE");
                    Custom_Query = "(SELECT CASE When(SELECT CUST_RESP_TYPE FROM CUSTFLDS WHERE CUST_CODE = " + Code_Col + ") = 'D' Then(Select  RSP_DESC FROM CUSTRESP WHERE RSP_CUST_CODE = " + Code_Col + " AND RSP_RESP_CODE = " + Column_Name + ") Else " + Column_Name + " End ) AS " + Column_Name; break;
                
                    //ADDED BY SUDHEER ON 05/25/2020
                case "SAR62":
                    string Sal_Code_Col = Column_Name.Replace("VALUE", "CODE");
                    Custom_Query = "(SELECT CASE When(SELECT SALQ_TYPE FROM SALQUES WHERE SALQ_ID = " + Sal_Code_Col + ") = 'D' or (SELECT SALQ_TYPE FROM SALQUES WHERE SALQ_ID = " + Sal_Code_Col + ") = 'C' " +
                        "Then case when (Select  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") is null THEN " + Column_Name + " Else (Select  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") End "+ 
                        "Else " + Column_Name + " End ) AS " + Column_Name; break;

                //Custom_Query = "(SELECT CASE When(SELECT SALQ_TYPE FROM SALQUES WHERE SALQ_ID = " + Sal_Code_Col + ") = 'D' Then(Select  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") Else " + Column_Name + " End ) AS " + Column_Name; break;


                //string Sal_Code_Col = Column_Name.Replace("VALUE", "CODE");
                //Custom_Query = "(SELECT CASE When(SELECT SALQ_TYPE FROM SALQUES WHERE SALQ_ID = " + Sal_Code_Col + ") = 'D' Then(Select  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") " +
                //    "(When(SELECT SALQ_TYPE FROM SALQUES WHERE SALQ_ID = " + Sal_Code_Col + ") = 'C' Then  CASE WHEN (Select  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") IS NULL THEN " + Column_Name + " Else (SELECT  SALQR_DESC FROM SALQRESP WHERE SALQR_Q_ID = " + Sal_Code_Col + " AND SALQR_CODE = " + Column_Name + ") END" +
                //    "Else " + Column_Name + " End ) AS " + Column_Name; break;


                case "VENDR":
                    Custom_Query = "(SELECT CASEVDD_NAME FROM CASEVDD Where CASEVDD_CODE = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "SALNM":
                    Custom_Query = "(SELECT SALD_NAME FROM SALDEF Where SALD_ID = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "CALNM":
                    Custom_Query = "(SELECT SALD_NAME FROM SALDEF Where SALD_ID = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "SALQS":
                    Custom_Query = "(SELECT SALQ_DESC FROM SALQUES Where SALQ_ID = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "CALQS":
                    Custom_Query = "(SELECT SALQ_DESC FROM SALQUES Where SALQ_ID = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "MATQS":
                    Custom_Query = "(SELECT Top 1 MATQUEST_DESC FROM MATQUEST Where MATQUEST_MAT_CODE = MATASMT_MAT_CODE And MATQUEST_SCL_CODE = MATASMT_SCL_CODE And MATQUEST_CODE = " + Column_Name + " ) AS " + Column_Name;
                    break;

                //ADDED BY SUDHEER ON 11/12/2018
                //case "MAQSS":
                //    Custom_Query = "(SELECT Top 1 MATQUEST_SEQ FROM MATQUEST Where MATQUEST_MAT_CODE = MATASMT_MAT_CODE And MATQUEST_SCL_CODE = MATASMT_SCL_CODE And MATQUEST_CODE = MATASMT_QUES_CODE) AS " + Column_Name;
                //    break;


                case "PRGRP":
                    Custom_Query = "(SELECT Top 1 PREASSGRP_DESC FROM PREASSGRP Where RTRIM(PREASSGRP_SUBCODE) = '' AND PREASSGRP_CODE = " + Column_Name + " ) AS " + Column_Name;
                    break;

                case "PRCAT":
                    Custom_Query = "(SELECT Top 1 PREASSGRP_DESC FROM PREASSGRP Where PREASSGRP_CODE = MST_PRESS_GRP and PREASSGRP_SUBCODE = " + Column_Name + " ) AS " + Column_Name;
                    break;

                //SELECT CASEREF_NAME1, * FROM CASEREF WHERE CASEREF_CODE = '00001'
                //	SELECT @Result_Name = COALESCE(@Result_Name +', ', '') + RTRIM(MSTSER_SERVICE) FROM MSTSER WHERE MSTSER_AGENCY = '01' AND MSTSER_DEPT = '02' AND MSTSER_PROGRAM = '09' AND MSTSER_YEAR = '    ' AND MSTSER_APP_NO = '00000014'
                //AND MSTSER_SERVICE IN (@Selected_Services)

                //ACT_AGENCY = @Agy AND ACT_AGENCY = @Dept AND ACT_AGENCY = @Prog AND ACT_YEAR = @Year AND ACT_APP_NO = @App AND
                //ACT_SNP_FAMILY_SEQ = @Fam_Seq AND ACT_CODE = @Cust_Ques_Code 

                case "ADCST":
                    string Pass_Seq = ""; bool Hdr_Seq_Found = false, Date_Column = false;
                    foreach (CustfldsEntity Ent in Case2001_Cust_List)
                    {
                        if (Column_Name.Trim() == Ent.CustCode)
                        {
                            switch (Ent.MemAccess)
                            {
                                case "A": if (MSTTab_Sel_Sw) Pass_Seq = "9999999"; break;
                                case "H": Pass_Seq = "8888888"; break;
                            }

                            if (Ent.RespType == "T")
                                Date_Column = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(Pass_Seq.Trim()))
                        Hdr_Seq_Found = true;

                    if (MSTTab_Sel_Sw && SNPTab_Sel_Sw)
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO, " + (Hdr_Seq_Found ? Pass_Seq : " SNP_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (Date_Column)
                            Custom_Query = "(Select (Cast((SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO, " + (Hdr_Seq_Found ? Pass_Seq : " SNP_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1 , " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As Date))) AS " + Column_Name;
                    }
                    else
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (ACT_AGENCY, ACT_DEPT, ACT_PROGRAM, ACT_YEAR, ACT_APP_NO, ACT_SNP_FAMILY_SEQ, '" + Column_Name + "' , ACT_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (Date_Column)
                            Custom_Query = "Select (Cast((SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (ACT_AGENCY, ACT_DEPT, ACT_PROGRAM, ACT_YEAR, ACT_APP_NO, ACT_SNP_FAMILY_SEQ, '" + Column_Name + "' , ACT_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As DATE))) AS " + Column_Name;
                    }
                    break;

                case "PREAS":
                    string Pass_Seq1 = ""; bool Hdr_Seq_Found1 = false, Date_Column1 = false;
                    foreach (CustfldsEntity Ent in Preasses_Cust_List)
                    {
                        if (Column_Name.Trim() == Ent.CustCode)
                        {
                            Pass_Seq1 = "7777777";
                            //switch (Ent.MemAccess)
                            //{
                            //    case "A": if (MSTTab_Sel_Sw) Pass_Seq = "9999999"; break;
                            //    case "H": Pass_Seq = "8888888"; break;
                            //}

                            if (Ent.RespType == "T")
                                Date_Column1 = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(Pass_Seq1.Trim()))
                        Hdr_Seq_Found1 = true;

                    if (MSTTab_Sel_Sw && SNPTab_Sel_Sw)
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_PRESRESP_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO, " + (Hdr_Seq_Found1 ? Pass_Seq1 : " SNP_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (Date_Column1)
                            Custom_Query = "(Select (Cast((SELECT [dbo].[CASB4404_PRESRESP_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO, " + (Hdr_Seq_Found1 ? Pass_Seq1 : " SNP_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1 , " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As Date))) AS " + Column_Name;
                    }
                    else
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_PRESRESP_REP_Column] (PRES_AGENCY, PRES_DEPT, PRES_PROGRAM, PRES_YEAR, PRES_APP_NO, PRES_SNP_FAMILY_SEQ, '" + Column_Name + "' , PRES_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (Date_Column1)
                            Custom_Query = "Select (Cast((SELECT [dbo].[CASB4404_PRESRESP_REP_Column] (PRES_AGENCY, PRES_DEPT, PRES_PROGRAM, PRES_YEAR, PRES_APP_NO, PRES_SNP_FAMILY_SEQ, '" + Column_Name + "' , PRES_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As DATE))) AS " + Column_Name;
                    }
                    break;
                case "SRCST":
                    string SPass_Seq1 = null; bool SHdr_Seq_Found1 = false, SDate_Column1 = false;
                    foreach (CustfldsEntity Ent in SERCUST_Cust_List)
                    {
                        if (Column_Name.Trim() == Ent.CustCode)
                        {
                            //Pass_Seq1 = "7777777";
                            //switch (Ent.MemAccess)
                            //{
                            //    case "A": if (MSTTab_Sel_Sw) Pass_Seq = "9999999"; break;
                            //    case "H": Pass_Seq = "8888888"; break;
                            //}

                            if (Ent.RespType == "T")
                                SDate_Column1 = true;
                        }
                    }
                    //if (!string.IsNullOrEmpty(SPass_Seq1.Trim()))
                    //    Hdr_Seq_Found1 = true;

                    if (MSTTab_Sel_Sw && SNPTab_Sel_Sw)
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_SERCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO,EMSRES_FUND,  '" + Column_Name + "' , 1, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (SDate_Column1)
                            Custom_Query = "(Select (Cast((SELECT [dbo].[CASB4404_SERCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO,EMSRES_FUND, '" + Column_Name + "' , 1 , " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As Date))) AS " + Column_Name;
                    }
                    else
                    {
                        Custom_Query = "(SELECT [dbo].[CASB4404_SERCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO,EMSRES_FUND, '" + Column_Name + "' , SER_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name;

                        if (SDate_Column1)
                            Custom_Query = "Select (Cast((SELECT [dbo].[CASB4404_SERCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO,EMSRES_FUND, '" + Column_Name + "' , SER_RESP_SEQ, " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) As DATE))) AS " + Column_Name;
                    }
                    break;

                //if (MSTTab_Sel_Sw)
                //    Custom_Query = "(SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (MST_AGENCY, MST_DEPT, MST_PROGRAM, MST_YEAR, MST_APP_NO, " + (Hdr_Seq_Found ? Pass_Seq : " MST_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1) ) AS " + Column_Name;
                //else if (SNPTab_Sel_Sw)
                //    Custom_Query = "(SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (SNP_AGENCY, SNP_DEPT, SNP_PROGRAM, SNP_YEAR, SNP_APP, " + (Hdr_Seq_Found ? Pass_Seq : " SNP_FAMILY_SEQ ") + ", '" + Column_Name + "' , 1) ) AS " + Column_Name;
                //else
                //    Custom_Query = "(SELECT [dbo].[CASB4404_ADDCUST_REP_Column] (ACT_AGENCY, ACT_DEPT, ACT_PROGRAM, ACT_YEAR, ACT_APP_NO, ACT_SNP_FAMILY_SEQ, '" + Column_Name + "' , ACT_RESP_SEQ) ) AS " + Column_Name;
                //break;
                case "00107": Custom_Query = "(SELECT TOP 1 RTRIM(AGYS_DESC) FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "' AND ATTN_PA = 'A' AND AGYS_CODE = " + Column_Name + "  COLLATE SQL_Latin1_General_CP1_CS_AS) AS " + Column_Name; break; //COLLATE Latin1_General_CS_AS
                case "00109": Custom_Query = "(SELECT TOP 1 RTRIM(AGYS_DESC) FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "' AND ATTN_PA = 'P' AND AGYS_CODE = ATTN_REASON  COLLATE SQL_Latin1_General_CP1_CS_AS) AS " + Column_Name; break; //COLLATE Latin1_General_CS_AS

                default: Custom_Query = "(SELECT TOP 1 RTRIM(AGYS_DESC) FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "'AND AGYS_CODE = " + Column_Name + " COLLATE Latin1_General_CS_AS) AS " + Column_Name; break;
            }

            if (AgyCode.Substring(0, 2) == "DL" && (AgyCode.Substring(4, 1) == "," || AgyCode.Substring(4, 1) == "|"))
            {
                Comma_Interval = int.Parse(AgyCode.Substring(2, 2));
                string Agy_Type = "";
                switch (Column_Name)
                {
                    case "MST_ALERT_CODES": Agy_Type = "00524"; break;
                    case "MST_INCOME_TYPES": Agy_Type = "00004"; break;
                    case "STF_POSITIONS": Agy_Type = "00550"; break;
                    case "SNP_NCASHBEN": Agy_Type = "00037"; break;
                    case "SNP_HEALTH_CODES": Agy_Type = "00038"; break;
                    case "MST_NCASHBEN": Agy_Type = "00037"; break;
                    case "AGYP_Target": Agy_Type = "S0076";break;
                    case "AGYP_COUNTIES_SERVED": Agy_Type = "00525";break;
                    case "CASEACT_BILL_PERIOD": Agy_Type = "00202"; break;
                    case "AGYP_SERPLANS": Agy_Type = "SPLAN"; break;
                    case "AGYR_REP_REQLEAD": Agy_Type = "SPLAN";break;
                    case "AGYP_REF_ASOC": Agy_Type = "S0125"; break;
                }

                if(Column_Name== "AGYP_SERPLANS" || Column_Name== "AGYR_REP_REQLEAD")
                    Custom_Query = "(SELECT dbo.[AppendString_GetDesc](" + Column_Name + ", '" + AgyCode.Substring(4, 1) + "', '" + Column_Name + "' , '" + Agy_Type + "')) AS " + Column_Name;
                else
                    Custom_Query = "(SELECT dbo.[AppendCommainstring_GetDesc](" + Column_Name + ", '" + AgyCode.Substring(4, 1) + "', " + Comma_Interval + " , '" + Agy_Type + "')) AS " + Column_Name;
            }

            if (!string.IsNullOrEmpty(AgyCode))
            {
                switch (Column_Name)
                {
                    case "Address": //Custom_Query = //"(SELECT RTRIM(MST_HN)+' '+RTRIM(MST_STREET)+' '+RTRIM(MST_SUFFIX)+', '+RTRIM(MST_CITY)+' '+RTRIM(MST_STATE)+RTRIM(MST_ZIP)+ CASE WHEN (LEN(MST_ZIPPLUS) >0 AND MST_ZIPPLUS > 0) THEN '-' + CAST(MST_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEMST ADRS WHERE ADRS.MST_AGENCY = TEST.MST_AGENCY AND ADRS.MST_DEPT = TEST.MST_DEPT AND ADRS.MST_PROGRAM = TEST.MST_PROGRAM AND ADRS.MST_YEAR = TEST.MST_YEAR AND ADRS.MST_APP_NO = TEST.MST_APP_NO) AS " + Column_Name; break;
                        //case "Address": Custom_Query = "(SELECT RTRIM(MST_HN)+' '+RTRIM(MST_STREET)+' '+RTRIM(MST_SUFFIX)+', '+RTRIM(MST_CITY)+' '+RTRIM(MST_STATE)+RTRIM(MST_ZIP)+ CASE WHEN (LEN(MST_ZIPPLUS) >0 AND MST_ZIPPLUS > 0) THEN '-' + CAST(MST_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEMST ADRS WHERE ADRS.MST_AGENCY = MST_AGENCY AND ADRS.MST_DEPT = MST_DEPT AND ADRS.MST_PROGRAM = MST_PROGRAM AND ADRS.MST_YEAR = MST_YEAR AND ADRS.MST_APP_NO = MST_APP_NO) AS " + Column_Name; break;
                        if (Cb_Use_DIFF.Checked)
                            //Custom_Query = "(SELECT ISNULL(RTRIM(DIFF_HN), ' ')+' '+ISNULL(RTRIM(DIFF_STREET), ' ')+' '+ISNULL(RTRIM(DIFF_SUFFIX), ' ')+', '+ISNULL(RTRIM(DIFF_CITY), ' ')+' '+ISNULL(RTRIM(DIFF_STATE), ' ')+ISNULL(RTRIM(DIFF_ZIP), ' ')+ CASE WHEN (LEN(DIFF_ZIPPLUS) >0 AND DIFF_ZIPPLUS > 0) THEN '-' + CAST(DIFF_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEDIFF WHERE DIFF_AGENCY = MST_AGENCY AND DIFF_DEPT = MST_DEPT AND DIFF_PROGRAM = MST_PROGRAM AND DIFF_YEAR = MST_YEAR AND DIFF_APP_NO =MST_APP_NO) AS " + Column_Name; 
                            //Custom_Query = "(SELECT ISNULL(RTRIM(DIFF_HN), ISNULL(RTRIM(MST_HN), ' '))+' '+ISNULL(RTRIM(DIFF_STREET), ISNULL(RTRIM(MST_STREET), ' '))+' '+ISNULL(RTRIM(DIFF_SUFFIX), ISNULL(RTRIM(MST_SUFFIX), ' '))+', '+ISNULL(RTRIM(DIFF_CITY), ISNULL(RTRIM(MST_CITY), ' '))+' '+ISNULL(RTRIM(DIFF_STATE), ISNULL(RTRIM(MST_STATE), ' '))+ISNULL(RTRIM(DIFF_ZIP), ISNULL(RTRIM(MST_ZIP), ' '))+ CASE WHEN (LEN(DIFF_ZIPPLUS) >0 AND DIFF_ZIPPLUS > 0) THEN '-' + CAST(DIFF_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEDIFF WHERE DIFF_AGENCY = MST_AGENCY AND DIFF_DEPT = MST_DEPT AND DIFF_PROGRAM = MST_PROGRAM AND DIFF_YEAR = MST_YEAR AND DIFF_APP_NO =MST_APP_NO) AS " + Column_Name; 
                            Custom_Query = "ISNULL((SELECT ISNULL(RTRIM(DIFF_HN), ' ') + ' '+ISNULL(RTRIM(DIFF_STREET), ' ')+' '+ISNULL(RTRIM(DIFF_SUFFIX), ' ')+' '+CASE WHEN DIFF_APT IS NOT NULL THEN 'Apt:'+ ISNULL(RTRIM(DIFF_APT), '')ELSE '' END +' '+CASE WHEN DIFF_FLR IS NOT NULL THEN 'Flr:'+  ISNULL(RTRIM(DIFF_FLR), ' ')ELSE '' END+', '+ISNULL(RTRIM(DIFF_CITY), ' ')+' '+ISNULL(RTRIM(DIFF_STATE), ' ')+' '+CASE WHEN DIFF_ZIP IS NOT NULL THEN ISNULL(Right('00000' + Cast(RTRIM(DIFF_ZIP) As Varchar(5)), 5), ' ') ELSE '00000' END  + CASE WHEN (LEN(DIFF_ZIPPLUS) >0 AND DIFF_ZIPPLUS > 0) THEN '-' + Right('0000' + CAST(DIFF_ZIPPLUS AS VARCHAR(4)),4) ELSE ' ' END FROM CASEDIFF WHERE DIFF_AGENCY = MST_AGENCY AND DIFF_DEPT = MST_DEPT AND DIFF_PROGRAM = MST_PROGRAM AND DIFF_YEAR = MST_YEAR AND DIFF_APP_NO =MST_APP_NO), (ISNULL(RTRIM(MST_HN), ' ')+ ' '+ISNULL(RTRIM(MST_STREET), ' ')+' '+ISNULL(RTRIM(MST_SUFFIX), ' ')+', '+ISNULL(RTRIM(MST_CITY), ' ')+' '+ISNULL(RTRIM(MST_STATE), ' ')+ISNULL(RTRIM(MST_ZIP), ' '))) AS " + Column_Name;
                        else
                            Custom_Query = "(SELECT ISNULL(RTRIM(MST_HN), ' ')+' '+CASE WHEN MST_DIRECTION IS NOT NULL THEN ISNULL(RTRIM(MST_DIRECTION)+' ', '') ELSE '' END +ISNULL(RTRIM(MST_STREET), ' ')+' '+ISNULL(RTRIM(MST_SUFFIX), ' ')+' '+CASE WHEN MST_APT IS NOT NULL THEN 'Apt:'+ ISNULL(RTRIM(MST_APT), '')ELSE '' END +' '+CASE WHEN MST_FLR IS NOT NULL THEN 'Flr:'+  ISNULL(RTRIM(MST_FLR), ' ')ELSE '' END+', '+ISNULL(RTRIM(MST_CITY), ' ')+' '+ISNULL(RTRIM(MST_STATE), ' ')+' '+ISNULL(Right('00000' + Cast(RTRIM(MST_ZIP) As Varchar(5)), 5), ' ')+ CASE WHEN (LEN(MST_ZIPPLUS) >0 AND MST_ZIPPLUS > 0) THEN '-' + Right('0000' +CAST(MST_ZIPPLUS AS VARCHAR(4)),4) ELSE ' ' END FROM CASEMST ADRS WHERE ADRS.MST_AGENCY = TEST.MST_AGENCY AND ADRS.MST_DEPT = TEST.MST_DEPT AND ADRS.MST_PROGRAM = TEST.MST_PROGRAM AND ADRS.MST_YEAR = TEST.MST_YEAR AND ADRS.MST_APP_NO = TEST.MST_APP_NO) AS " + Column_Name;
                        break;

                    //case "Name": Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_LAST)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '. '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY+NAM.SNP_DEPT+NAM.SNP_PROGRAM+NAM.SNP_YEAR+NAM.SNP_APP = TST1.SNP_AGENCY+TST1.SNP_DEPT+TST1.SNP_PROGRAM+TST1.SNP_YEAR+TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; break;
                    case "Name":
                        if (Member_NameFormat == "1")
                            Custom_Query = "(RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI)+'. ' ELSE ' ' END + RTRIM(ISNULL(SNP_NAME_IX_LAST,''))) AS " + Column_Name;
                        //Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI)+'. ' ELSE ' ' END + RTRIM(SNP_NAME_IX_LAST) FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; 

                        //////Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_LAST)) > 0)  THEN '.  '+ RTRIM(SNP_NAME_IX_LAST) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; 
                        //////Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_LAST)) > 0)  THEN '.  '+ RTRIM(SNP_NAME_IX_LAST) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = SNP_AGENCY AND NAM.SNP_DEPT = SNP_DEPT AND NAM.SNP_PROGRAM = SNP_PROGRAM AND NAM.SNP_YEAR = SNP_YEAR AND NAM.SNP_APP = SNP_APP AND NAM.SNP_FAMILY_SEQ = SNP_FAMILY_SEQ) AS " + Column_Name; 
                        else
                            //////Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_LAST)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_FI)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_FI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = SNP_AGENCY AND NAM.SNP_DEPT = SNP_DEPT AND NAM.SNP_PROGRAM = SNP_PROGRAM AND NAM.SNP_YEAR = SNP_YEAR AND NAM.SNP_APP = SNP_APP AND NAM.SNP_FAMILY_SEQ = SNP_FAMILY_SEQ) AS " + Column_Name;
                            //Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_LAST)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_FI)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_FI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; 
                            Custom_Query = "(RTRIM(ISNULL(SNP_NAME_IX_LAST,''))+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_FI)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_FI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END) AS " + Column_Name;
                        break;


                    case "Current_Age": //Curr_Age_Asof_Date
                        //Custom_Query = "(SELECT CAST((DATEDIFF( mm, SNP_ALT_BDATE, GETDATE()) / 12) AS VARCHAR) + '.' + CAST(( DATEDIFF( mm, SNP_ALT_BDATE,GETDATE()) % 12) AS VARCHAR) FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name;
                        Curr_Age_Asof_Date = (Curr_Age_Asof_Date.Trim() == "NULL" ? Curr_Age_Asof_Date = " GETDATE() " : "'" + Curr_Age_Asof_Date + "'");
                        //Commented by sudheer on 02/09/2021 
                        //Custom_Query = "(SELECT CAST((DATEDIFF( mm, SNP_ALT_BDATE, " + Curr_Age_Asof_Date + ") / 12) AS VARCHAR) + '.' + CAST(( DATEDIFF( mm, SNP_ALT_BDATE," + Curr_Age_Asof_Date + ") % 12) AS VARCHAR) FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name;
                        //Added by Sudheer on 02/09/2021 
                        Custom_Query = "(SELECT CAST(((DATEDIFF(m, SNP_ALT_BDATE, " + Curr_Age_Asof_Date + ") - CASE WHEN DAY(SNP_ALT_BDATE) > DAY(" + Curr_Age_Asof_Date + ") THEN 1 ELSE 0 END)/12) AS varchar) +'.'+ CAST(((DATEDIFF(m, SNP_ALT_BDATE, " + Curr_Age_Asof_Date + ") - CASE WHEN DAY(SNP_ALT_BDATE) > DAY(" + Curr_Age_Asof_Date + ") THEN 1 ELSE 0 END) % 12) AS VARCHAR)) AS " + Column_Name;
                        break;

                    case "Birth_Date_Month":
                        Custom_Query = "(SELECT DATEPART(MM, SNP_ALT_BDATE) FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name;
                        break;

                    case "ATTN_Month":
                        Custom_Query = "(SELECT DATEPART(MM, ATTN_DATE) FROM CHLDATTN MON Where TST2.ATTN_AGENCY = MON.ATTN_AGENCY And TST2.ATTN_DEPT = MON.ATTN_DEPT And TST2.ATTN_PROGRAM = MON.ATTN_PROGRAM And TST2.ATTN_YEAR = MON.ATTN_YEAR And TST2.ATTN_APP_NO = MON.ATTN_APP_NO And TST2.ATTN_SITE = MON.ATTN_SITE And TST2.ATTN_ROOM = MON.ATTN_ROOM And TST2.ATTN_AMPM = MON.ATTN_AMPM And TST2.ATTN_FUNDING_SOURCE = MON.ATTN_FUNDING_SOURCE And TST2.ATTN_DATE = MON.ATTN_DATE ) AS " + Column_Name;
                        break;

                    //case "Name": Custom_Query = "(SELECT [dbo].[Adhoc_Get_Member_Name](SNP_AGENCY,SNP_DEPT,SNP_PROGRAM,SNP_YEAR,SNP_APP, SNP_FAMILY_SEQ, 1)) AS " + Column_Name; break;
                    //[dbo].[Adhoc_Get_Member_Name]

                    case "SNP_SSNO": Custom_Query= "(SELECT ISNULL(CASE WHEN LEN(SNP_SSNO)=9 THEN SUBSTRING(SNP_SSNO,1,3)+'-'+SUBSTRING(SNP_SSNO,4,2)+'-'+SUBSTRING(SNP_SSNO,6,4) ELSE SNP_SSNO END ,'') FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; break;

                    case "MST_PHONE": Custom_Query = "(SELECT CAST(RIGHT('000'+CASE WHEN (LEN(MST_AREA) >0) THEN MST_AREA ELSE '000' END, 3)+RIGHT('0000000'+CASE WHEN (LEN(MST_PHONE) >0) THEN MST_PHONE ELSE '000' END, 7) AS NUMERIC(10)) FROM CASEMST PHN WHERE PHN.MST_AGENCY = TEST.MST_AGENCY AND PHN.MST_DEPT = TEST.MST_DEPT AND PHN.MST_PROGRAM = TEST.MST_PROGRAM AND PHN.MST_YEAR = TEST.MST_YEAR AND PHN.MST_APP_NO = TEST.MST_APP_NO) AS " + Column_Name; break;
                    //case "MST_PHONE": Custom_Query = "(SELECT CAST(RIGHT('000'+CASE WHEN (LEN(MST_AREA) >0) THEN MST_AREA ELSE '000' END, 3)+RIGHT('0000000'+CASE WHEN (LEN(MST_PHONE) >0) THEN MST_PHONE ELSE '000' END, 7) AS NUMERIC(10)) FROM CASEMST PHN WHERE PHN.MST_AGENCY = MST_AGENCY AND PHN.MST_DEPT = MST_DEPT AND PHN.MST_PROGRAM = MST_PROGRAM AND PHN.MST_YEAR = MST_YEAR AND PHN.MST_APP_NO = MST_APP_NO) AS " + Column_Name; break;

                    case "MST_FAMILY_ID": Custom_Query = "(Right('000000000'+ CAST(" + Column_Name + " As Varchar(9)), 9)) AS " + Column_Name; break;

                    case "MST_STATE":
                    case "MST_CITY":
                    case "MST_STREET":
                    case "MST_SUFFIX":
                    case "MST_HN":
                    case "MST_APT":
                    case "MST_FLR":
                    case "MST_ZIP":
                    case "MST_DIRECTION": if (Cb_Use_DIFF.Checked) Custom_Query = "ISNULL((SELECT DIFF_" + Column_Name.Substring(4, Column_Name.Length - 4) + " FROM CASEDIFF WHERE DIFF_AGENCY = MST_AGENCY AND DIFF_DEPT = MST_DEPT AND DIFF_PROGRAM = MST_PROGRAM AND DIFF_YEAR = MST_YEAR AND DIFF_APP_NO =MST_APP_NO), ISNULL("+Column_Name+ ", CASE WHEN "+ Column_Name+ "= MST_ZIP THEN '0' ELSE ' ' END)) AS " + Column_Name; break;

                    case "TMSAPPT_TIME":
                        Custom_Query = "(Select [dbo].[Adhoc_Get_Time_Format_Form_Numeric](TMSAPPT_TIME)) AS " + Column_Name;
                        break;

                    case "EnrlLumpedSite":
                        Custom_Query = "(ENRL_SITE+'-'+RTRIM(ENRL_ROOM)+'-'+ENRL_AMPM) AS " + Column_Name;
                        break;

                    case "CASEMSOBO_Name":
                        Custom_Query = "(Select [dbo].Adhoc_Get_Member_Name(CASEMS_AGEncy, CASEMS_DEPt, CASEMS_PROGRAM, CASEMS_YEar, CASEMS_APP_NO, CASEMSOBO_FAM_SEQ, 0)) AS " + Column_Name;
                        break;

                    case "CASEACTOBO_Name":
                        Custom_Query = "(Select [dbo].Adhoc_Get_Member_Name(CASEACT_AGEncy, CASEACT_DEPt, CASEACT_PROGRAM, CASEACT_YEar, CASEACT_APP_NO, CAOBO_FAM_SEQ, 0)) AS " + Column_Name;
                        break;

                   // case "AGYB_REQ_DATE": Custom_Query= "((SELECT TOP 1 AGYB_REQ_DATE FROM AGCYBOUTIQUE WHERE AGYB_PART_CODE=AGYP_CODE) AS " + Column_Name; break;

                    case "TMSAPPT_TEMPLATE_TYPE":
                    case "CHLDMST_BIRTH_CERT":
                    case "CHLDMST_CHLD_REPEAT":
                    case "CHLDMST_NXTYEAR_PREP":
                    case "CHLDMST_PRE_CLIENT":
                    case "CHLDMST_MED_SECURITY":
                    case "CHLDMST_MED_COVERAGE":
                    case "CHLDMST_DENTAL_COVERAGE":
                    case "CHLDMST_DISABILITY":
                    case "CHLDMST_ALLERGIES":
                    case "CHLDMST_DIETARY_RES":
                    case "CHLDMST_MEDICATIONS":
                    case "CHLDMST_MEDICAL_COND":
                    case "CHLDMST_HH_CONCERNS":
                    case "CHLDMST_DEVLPMNTL_CONCERN":
                    case "CASESUM_NOT_INTERESTED":
                    //case "MATASMT_QUES_CODE":
                    case "STF_HS_PARENT":
                    case "STF_EHS_PARENT":
                    case "STF_DAYCARE_PARENT":
                    case "STF_POS_FILLED":
                    case "STF_REPLACE_SM":
                    case "CLC_S_OBO":
                        Custom_Query = "(SELECT TOP 1 RTRIM(AGYS_DESC) FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "'AND AGYS_CODE = " + Column_Name + ") AS " + Column_Name; break;

                    case "MEDI_ANSWER1":
                        Custom_Query = "(SELECT [dbo].[CASB4404_Task_REP_Column] (MEDI_AGENCY, MEDI_DEPT, MEDI_PROG, MEDI_YEAR, MEDI_APP_NO, MEDI_TASK, " + Column_Name + ", " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name; break;
                    case "HLSMEDI_ANSWER1":
                        Custom_Query = "(SELECT [dbo].[CASB4404_HLSTask_REP_Column] (HLSMEDI_AGENCY, HLSMEDI_DEPT, HLSMEDI_PROG, HLSMEDI_YEAR, HLSMEDI_APP_NO, HLSMEDI_TASK, " + Column_Name + ", " + (Description_SW == '1' ? "'Y'" : "'N'") + " )) AS " + Column_Name; break;

                }
            }
            return Custom_Query;
        }


        private string Get_Custom_SUmmary_Query(string AgyCode, string Table_Name, string Column_Name, char Desc_SW, string Summ_Type)
        {
            string Custom_Query = "NULL";
            string Temp_Agy_Col, Temp_Dept_Col, Temp_Prog_Col;
            string Table_Subscript = Table_Name.Substring(4, Table_Name.Length - 4);
            int Comma_Interval = 1;

            Temp_Agy_Col = Table_Subscript + "_AGENCY";
            Temp_Dept_Col = Table_Subscript + "_DEPT";
            //Temp_Prog_Col = Table_Subscript + "_PROGRAM";
            Temp_Prog_Col = Table_Subscript + (Table_Name != "CHLDMEDI" ? "_PROGRAM" : "_PROG");

            switch (AgyCode)
            {

                //CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                case "WORKR":
                    switch (Desc_SW)
                    {
                        case '0':
                            if (CAseWorkerr_NameFormat == "1")
                                Custom_Query = " PWR_CASEWORKER , '      ' + LTRIM(ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  ' + ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')), NULL,0, NULL FROM PASSWORD ";
                            else
                                Custom_Query = " PWR_CASEWORKER , '      ' +  LTRIM(ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+',  ' + ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')), NULL,0, NULL FROM PASSWORD ";
                            break;

                        case '1':
                            if (CAseWorkerr_NameFormat == "1")
                                Custom_Query = " LTRIM(ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  ' + ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')) , '      ' + LTRIM(ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  ' + ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')),NULL, 0, NULL FROM PASSWORD ";
                            else
                                Custom_Query = " LTRIM(ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+',  ' + ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')) , '      ' +  LTRIM(ISNULL(RTRIM(PWR_NAME_IX_LAST),' ')+',  ' + ISNULL(RTRIM(PWR_NAME_IX_FIRST),' ')+'  '+ ISNULL(RTRIM(PWR_NAME_IX_MI),' ')), NULL,0, NULL FROM PASSWORD ";
                            break;
                    }
                    break;

                //case "HIEAG": if (Desc_SW == '1')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                //                    Custom_Query = "(SELECT DISTINCT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' ) "; 
                //               else
                //                    Custom_Query = "(SELECT DISTINCT RTRIM(HIE_AGENCY) FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' ) "; 
                //                break;

                //case "HIEDE": if (Desc_SW == '1')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                //                    Custom_Query = "(SELECT DISTINCT RTRIM(HIE_NAME) FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' ) ";
                //                else
                //                    Custom_Query = "(SELECT DISTINCT RTRIM(HIE_AGENCY) FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  ' ) ";
                //                break;


                //case "SITES": Custom_Query = "(SELECT DISTINCT RTRIM(SITE_NAME) FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' AND SITE_AGENCY = TEST.MST_AGENCY AND SITE_NUMBER = " + Column_Name + ") AS " + Column_Name; break;
                case "ZIPLS": Custom_Query = " RIGHT('0000'+ CAST(ZCR_ZIP AS VARCHAR(5)),5)+'-'+ RIGHT('0000'+ CAST(ZCRPLUS_4 AS VARCHAR(4)), 4), '      ' + RIGHT('0000'+ CAST(ZCR_ZIP AS VARCHAR(5)),5)+'-'+ RIGHT('0000'+ CAST(ZCRPLUS_4 AS VARCHAR(4)), 4),NULL, 0, NULL FROM  ZIPCODE "; break;

                case "DLMTD": Custom_Query = "(SELECT dbo.[AppendCommainstring](" + Column_Name + " , " + Comma_Interval + ")) AS " + Column_Name; break;

                case "STFMS":

                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " STF_CODE, '      ' + RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END, 0, STF_AGENCY FROM STAFFMST WHERE STF_YEAR= "+ Program_Year; break;
                        case '1': Custom_Query = " RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END, '      ' + RTRIM(STF_NAME_FI)+ CASE WHEN (LEN(RTRIM(STF_NAME_LAST)) > 0)  THEN ', '+ RTRIM(STF_NAME_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(STF_NAME_MI)) > 0)  THEN '. '+ RTRIM(STF_NAME_MI) ELSE ' ' END, 0, STF_AGENCY FROM STAFFMST WHERE STF_YEAR= "+Program_Year; break;
                    }
                    break;

                case "STFTR":

                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " STFTR_ID, '      ' + STFTR_NAME, 0, STFTR_AGENCY FROM STFTRAIN WHERE STFTR_YEAR= " + Program_Year; break;
                        case '1': Custom_Query = " STFTR_NAME, 0, STFTR_AGENCY FROM STFTRAIN WHERE STFTR_YEAR= " + Program_Year; break;
                    }
                    break;

                case "SITES":
                    if(Current_Hierarchy_DB.Substring(0, 2)=="**")
                    {
                        switch (Desc_SW)
                        {
                            case '0': Custom_Query = " RTRIM(SITE_NUMBER), '      ' + RTRIM(SITE_NAME), 0,0, SITE_AGENCY FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' "; break;
                            case '1': Custom_Query = " RTRIM(SITE_NAME), '      ' + RTRIM(SITE_NAME), 0,0, SITE_AGENCY FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' "; break;
                        }
                    }
                    else
                    {
                        switch (Desc_SW)
                        {
                            case '0': Custom_Query = " RTRIM(SITE_NUMBER), '      ' + RTRIM(SITE_NAME), 0,0, SITE_AGENCY FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' AND SITE_AGENCY = '" + Current_Hierarchy_DB.Substring(0, 2) + "'"; break;
                            case '1': Custom_Query = " RTRIM(SITE_NAME), '      ' + RTRIM(SITE_NAME), 0,0, SITE_AGENCY FROM CASESITE WHERE SITE_ROOM = '0000' AND SITE_DEPT = '  ' AND SITE_PROG = '  ' AND SITE_AGENCY =  '" + Current_Hierarchy_DB.Substring(0, 2) + "'"; break;
                        }
                    }
                    
                    break;

                case "REFRL": //Custom_Query = "(SELECT RTRIM(CASEREF_NAME1) FROM CASEREF WHERE CASEREF_CODE = " + Column_Name + " ) AS " + Column_Name; break;
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " RTRIM(CASEREF_CODE), RTRIM(CASEREF_NAME1),NULL, 0, NULL FROM CASEREF "; break;
                        case '1': Custom_Query = " REPLACE(RTRIM(CASEREF_NAME1), '''', ''''''), RTRIM(CASEREF_NAME1),NULL, 0, NULL FROM CASEREF "; break;
                    }
                    break;

                case "PARTN": //Custom_Query = "(SELECT RTRIM(CASEREF_NAME1) FROM CASEREF WHERE CASEREF_CODE = " + Column_Name + " ) AS " + Column_Name; break;
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " RTRIM(AGYP_CODE), RTRIM(AGYP_NAME),NULL, 0, NULL FROM AGCYPART "; break;
                        case '1': Custom_Query = " REPLACE(RTRIM(AGYP_NAME), '''', ''''''), RTRIM(AGYP_NAME),NULL, 0, NULL FROM AGCYPART "; break;
                    }
                    break;

                case "SPLAN": //Custom_Query = "(SELECT RTRIM(CASEREF_NAME1) FROM CASEREF WHERE CASEREF_CODE = " + Column_Name + " ) AS " + Column_Name; break;
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " sp0_servicecode, RTRIM(sp0_description), NULL,0, NULL FROM CASESP0 "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(sp0_description), '''', ''''''), RTRIM(sp0_description),NULL, 0, NULL FROM CASESP0 "; break;
                    }
                    break;

                case "CACTI": //Custom_Query = "(SELECT RTRIM(CASEREF_NAME1) FROM CASEREF WHERE CASEREF_CODE = " + Column_Name + " ) AS " + Column_Name; break;
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(CA_CODE), RTRIM(CA_DESC),NULL, 0, NULL FROM CAMAST "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(CA_DESC), '''', ''''''), RTRIM(CA_DESC),NULL, 0, NULL FROM CAMAST  "; break;
                    }
                    break;

                case "MSTON":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(MS_CODE), RTRIM(MS_DESC), NULL,0, NULL FROM MSMAST "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(MS_DESC), '''', ''''''), RTRIM(MS_DESC),NULL, 0, NULL FROM MSMAST  "; break;
                    }
                    break;

                case "CST62":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(CUST_CODE), RTRIM(CUST_DESC),NULL, 0, NULL FROM CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0063' "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(CUST_DESC), '''', ''''''), RTRIM(CUST_DESC),NULL, 0, NULL FROM CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0063' "; break;
                    }
                    break;

                case "SAL62":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(SALQ_ID), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(SALQ_DESC), '''', ''''''), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES "; break;
                    }
                    break;

                case "CST61":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(CUST_CODE), RTRIM(CUST_DESC),NULL, 0, NULL FROM CUSTFLDS WHERE  CUST_SCR_CODE = 'CASE0061' "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(CUST_DESC), '''', ''''''), RTRIM(CUST_DESC),NULL, 0, NULL FROM CUSTFLDS WHERE  CUST_SCR_CODE = 'CASE0061' "; break;
                    }
                    break;


                case "PRGRP":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(PREASSGRP_CODE), RTRIM(PREASSGRP_DESC),NULL, 0, NULL FROM PREASSGRP Where RTRIM(PREASSGRP_SUBCODE) = ''"; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(PREASSGRP_DESC), '''', ''''''), RTRIM(PREASSGRP_DESC),NULL, 0, NULL FROM PREASSGRP Where RTRIM(PREASSGRP_SUBCODE) = ''"; break;
                    }
                    break;

                case "PRCAT":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(PREASSGRP_SUBCODE), RTRIM(PREASSGRP_DESC),NULL, 0, NULL FROM PREASSGRP Where RTRIM(PREASSGRP_SUBCODE) != ''"; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(PREASSGRP_DESC), '''', ''''''), RTRIM(PREASSGRP_DESC),NULL, 0, NULL FROM PREASSGRP Where RTRIM(PREASSGRP_SUBCODE) != ''"; break;
                    }

                    //Custom_Query = "(SELECT Top 1 PREASSGRP_DESC FROM PREASSGRP Where PREASSGRP_CODE = MST_PRESS_GRP and PREASSGRP_SUBCODE = " + Column_Name + " ) AS " + Column_Name;
                    break;

                //Added by Sudheer on 06/02/2022
                case "CMBDC": 
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = " RTRIM(BDC_ID), RTRIM(BDC_DESCRIPTION),NULL, 0, NULL FROM CMBDC "; break;
                        case '1': Custom_Query = " REPLACE(RTRIM(BDC_DESCRIPTION), '''', ''''''), RTRIM(BDC_DESCRIPTION),NULL, 0, NULL FROM CMBDC "; break;
                    }
                    break;

                //case "RSP62":
                //    switch (Desc_SW)
                //    {
                //        //case '0': Custom_Query = "  RTRIM(RSP_CUST_CODE), RTRIM(RESP_DESC), 0, NULL FROM  CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0063' "; break;
                //        //case '1': Custom_Query = "  REPLACE(RTRIM(RESP_DESC), '''', ''''''), RTRIM(RESP_DESC), 0, NULL FROM  CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0063' "; break;
                //        case '0': Custom_Query = "  RTRIM(RSP_RESP_CODE), RTRIM(RSP_DESC), 0, NULL FROM  CUSTRESP WHERE RSP_SCR_CODE = 'CASE0063' "; break;
                //        case '1': Custom_Query = "  REPLACE(RTRIM(RSP_DESC), '''', ''''''), RTRIM(RSP_DESC), 0, NULL FROM  CUSTRESP WHERE RSP_SCR_CODE = 'CASE0063' "; break;
                //    //    default:
                //    //Custom_Query = "(SELECT CASE When(SELECT CUST_RESP_TYPE FROM CUSTFLDS WHERE CUST_CODE = " + Column_Name + ") = 'D' Then(Select  RSP_DESC FROM CUSTRESP WHERE RSP_CUST_CODE = " + Column_Name + " AND RSP_RESP_CODE = " + Column_Name + ") Else " + Column_Name + " End ) AS " + Column_Name; break;

                //    }
                //    break;

                //case "RSP61":
                //    switch (Desc_SW)
                //    {
                //        //case '0': Custom_Query = "  RTRIM(RSP_CUST_CODE), RTRIM(RSP_DESC), 0, NULL FROM  CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0061' "; break;
                //        //case '1': Custom_Query = "  REPLACE(RTRIM(RSP_DESC), '''', ''''''), RTRIM(RSP_DESC), 0, NULL FROM  CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0061' "; break;
                //        case '0': Custom_Query = "  RTRIM(RSP_RESP_CODE), RTRIM(RSP_DESC), 0, NULL FROM  CUSTRESP WHERE RSP_SCR_CODE = 'CASE0061' "; break;
                //        case '1': Custom_Query = "  REPLACE(RTRIM(RSP_DESC), '''', ''''''), RTRIM(RSP_DESC), 0, NULL FROM  CUSTRESP WHERE RSP_SCR_CODE = 'CASE0061' "; break;
                //    }
                //    break;

                case "OPRTR":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(PWR_EMPLOYEE_NO), RTRIM(PWR_EMPLOYEE_NO),NULL, 0, NULL FROM  PASSWORD  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(PWR_EMPLOYEE_NO), '''', ''''''), RTRIM(PWR_EMPLOYEE_NO),NULL, 0, NULL FROM  PASSWORD "; break;
                    }
                    break;

                case "VENDR":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(CASEVDD_CODE), RTRIM(CASEVDD_NAME),NULL, 0, NULL FROM CASEVDD  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(CASEVDD_CODE), '''', ''''''), RTRIM(CASEVDD_NAME),NULL, 0, NULL FROM CASEVDD "; break;
                    }
                    break;

                case "SALNM":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(SALD_ID), RTRIM(SALD_NAME),NULL, 0, NULL FROM SALDEF  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(SALD_ID), '''', ''''''), RTRIM(SALD_NAME),NULL, 0, NULL FROM SALDEF "; break;
                    }
                    break;

                case "SALQS":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(SALQ_ID), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(SALQ_ID), '''', ''''''), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES "; break;
                    }
                    break;

                case "CALNM":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(SALD_ID), RTRIM(SALD_NAME),NULL, 0, NULL FROM SALDEF  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(SALD_ID), '''', ''''''), RTRIM(SALD_NAME),NULL, 0, NULL FROM SALDEF "; break;
                    }
                    break;

                case "CALQS":
                    switch (Desc_SW)
                    {
                        case '0': Custom_Query = "  RTRIM(SALQ_ID), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES  "; break;
                        case '1': Custom_Query = "  REPLACE(RTRIM(SALQ_ID), '''', ''''''), RTRIM(SALQ_DESC),NULL, 0, NULL FROM SALQUES "; break;
                    }
                    break;

                case "ALLPR":
                    if (Desc_SW == '0')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                        Custom_Query = "RTRIM(HIE_AGENCY+HIE_DEPT+HIE_PROGRAM), RTRIM(HIE_NAME),NULL, 0, NULL FROM  CASEHIE WHERE HIE_DEPT != '  ' AND HIE_PROGRAM != '  ' ";
                    else
                        Custom_Query = "REPLACE(RTRIM(HIE_NAME), '''', ''''''), RTRIM(HIE_NAME),NULL, 0, NULL FROM CASEHIE WHERE HIE_DEPT != '  ' AND HIE_PROGRAM != '  ' ";
                    break;

                case "SPHIE":
                    if (Desc_SW == '0')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                        Custom_Query = "RTRIM(HIE_AGENCY+HIE_DEPT+HIE_PROGRAM), RTRIM(HIE_NAME),NULL, 0, NULL FROM  CASEHIE WHERE HIE_DEPT != '  ' AND HIE_PROGRAM != '  ' ";
                    else
                        Custom_Query = "REPLACE(RTRIM(HIE_NAME), '''', ''''''), RTRIM(HIE_NAME),NULL, 0, NULL FROM CASEHIE WHERE HIE_DEPT != '  ' AND HIE_PROGRAM != '  ' ";
                    break;

                case "MATRX":
                    if (Desc_SW == '0')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                        Custom_Query = "RTRIM(MATDEF_MAT_CODE), RTRIM(MATDEF_DESC),NULL, 0, NULL FROM MATDEF Where MATDEF_SCL_CODE = 0 ";
                    else
                        Custom_Query = "REPLACE(RTRIM(MATDEF_DESC), '''', ''''''), RTRIM(MATDEF_DESC),NULL, 0, NULL FROM MATDEF WHERE MATDEF_SCL_CODE = 0 ";
                    break;

                case "SCALS":
                    if (Desc_SW == '0')  //SELECT DISTINCT HIE_NAME FROM CASEHIE WHERE HIE_DEPT = '  ' AND HIE_PROGRAM = '  '
                        Custom_Query = "RTRIM(MATDEF_SCL_CODE), RTRIM(MATDEF_DESC),NULL, 0, NULL FROM MATDEF Where MATDEF_SCL_CODE != 0 ";
                    else
                        Custom_Query = "REPLACE(RTRIM(MATDEF_DESC), '''', ''''''), RTRIM(MATDEF_DESC),NULL, 0, NULL FROM MATDEF WHERE MATDEF_SCL_CODE != 0 ";
                    break;
                //case "TASKS":
                //    switch (Desc_SW)
                //    {
                //        case '0': Custom_Query = "  RTRIM(TRCK_TASK), RTRIM(TRCK_TASK_DESC), NULL,0, NULL FROM CHLDTRCK WHERE TRCK_COMPONENT = '0000'"; break;
                //        case '1': Custom_Query = "  REPLACE(RTRIM(TRCK_TASK_DESC), '''', ''''''), RTRIM(TRCK_TASK_DESC),NULL, 0, NULL FROM CHLDTRCK WHERE TRCK_COMPONENT = '0000' "; break;
                //    }
                //    break;



                //case "CNTDT":
                //case "ADCST":
                case "RSP61":
                case "RSP62":
                case "SAR62":
                case "CSTRS":
                case "TASKS":
                case "NULL":
                    if (Summ_Type != "S")
                        Custom_Query = "  " + Column_Name + ", " + Column_Name + ", " + Column_Name + ", (SELECT COUNT(*) FROM #TmpResult_Table DT_CNT WHERE DT_CNT." + Column_Name + " = DT_CNT1." + Column_Name + ")  , NULL FROM  #TmpResult_Table  DT_CNT1 ";
                    else
                        Custom_Query = "  " + Column_Name + ", " + Column_Name + ", " + Column_Name + ", (SELECT COUNT(*) FROM #TmpResult_Table DT_CNT WHERE DT_CNT." + Column_Name + " = DT_CNT1." + Column_Name + ")  , 'S' FROM  #TmpResult_Table  DT_CNT1 ";
                    break;


                    //switch (Desc_SW)
                    //{
                    //    case '0': Custom_Query = "  RTRIM(Column_Name), RTRIM(Column_Name), 0, NULL FROM  #TmpResult_Table "; break;
                    //case '1': Custom_Query = "  REPLACE(RTRIM(RESP_DESC), '''', ''''''), RTRIM(RESP_DESC), 0, NULL FROM  CUSTFLDS WHERE CUST_SCR_CODE = 'CASE0061' "; break;
                    //}
                    //break;
                    //default: Custom_Query = "(SELECT RTRIM(AGYS_DESC) FROM AGYTABS WHERE AGYS_TYPE = '" + AgyCode + "'AND AGYS_CODE = " + Column_Name + ") AS " + Column_Name; break;
            }


            ///case "MSTON": Custom_Query = "SELECT RTRIM(MS_DESC) FROM MSMAST WHERE RTRIM(MS_CODE) = " + Column_Name + " ) AS " + Column_Name; break;


            //if (AgyCode.Substring(0, 2) == "DL" && (AgyCode.Substring(4, 1) == "," || AgyCode.Substring(4, 1) == "|"))
            //{
            //    Comma_Interval = int.Parse(AgyCode.Substring(2, 2));
            //    Custom_Query = "(SELECT dbo.[AppendCommainstring](" + Column_Name + ", '" + AgyCode.Substring(4, 1) + "', " + Comma_Interval + ")) AS " + Column_Name;
            //}

            //if (!string.IsNullOrEmpty(AgyCode))
            //{
            //    switch (Column_Name)
            //    {
            //        case "Address": Custom_Query = "(SELECT RTRIM(MST_HN)+' '+RTRIM(MST_STREET)+' '+RTRIM(MST_SUFFIX)+', '+RTRIM(MST_CITY)+' '+RTRIM(MST_STATE)+RTRIM(MST_ZIP)+ CASE WHEN (LEN(MST_ZIPPLUS) >0 AND MST_ZIPPLUS > 0) THEN '-' + CAST(MST_ZIPPLUS AS VARCHAR(4)) ELSE ' ' END FROM CASEMST ADRS WHERE ADRS.MST_AGENCY = TEST.MST_AGENCY AND ADRS.MST_DEPT = TEST.MST_DEPT AND ADRS.MST_PROGRAM = TEST.MST_PROGRAM AND ADRS.MST_YEAR = TEST.MST_YEAR AND ADRS.MST_APP_NO = TEST.MST_APP_NO) AS " + Column_Name; break;

            //        //case "Name": Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_LAST)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_LAST) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '. '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY+NAM.SNP_DEPT+NAM.SNP_PROGRAM+NAM.SNP_YEAR+NAM.SNP_APP = TST1.SNP_AGENCY+TST1.SNP_DEPT+TST1.SNP_PROGRAM+TST1.SNP_YEAR+TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name; break;
            //        case "Name":
            //            if (Member_NameFormat == "1")
            //                Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_FI)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_LAST)) > 0)  THEN '.  '+ RTRIM(SNP_NAME_IX_LAST) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name;
            //            else
            //                Custom_Query = "(SELECT RTRIM(SNP_NAME_IX_LAST)+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_FI)) > 0)  THEN ', '+ RTRIM(SNP_NAME_IX_FI) ELSE ' ' END+ CASE WHEN (LEN(RTRIM(SNP_NAME_IX_MI)) > 0)  THEN '  '+ RTRIM(SNP_NAME_IX_MI) ELSE ' ' END FROM CASESNP NAM WHERE NAM.SNP_AGENCY = TST1.SNP_AGENCY AND NAM.SNP_DEPT = TST1.SNP_DEPT AND NAM.SNP_PROGRAM = TST1.SNP_PROGRAM AND NAM.SNP_YEAR = TST1.SNP_YEAR AND NAM.SNP_APP = TST1.SNP_APP AND NAM.SNP_FAMILY_SEQ = TST1.SNP_FAMILY_SEQ) AS " + Column_Name;

            //            break;

            //        //case "Name": Custom_Query = "(SELECT [dbo].[Adhoc_Get_Member_Name](SNP_AGENCY,SNP_DEPT,SNP_PROGRAM,SNP_YEAR,SNP_APP, SNP_FAMILY_SEQ, 1)) AS " + Column_Name; break;
            //        //[dbo].[Adhoc_Get_Member_Name]

            //        case "MST_PHONE": Custom_Query = "(SELECT CAST(RIGHT('000'+CASE WHEN (LEN(MST_AREA) >0) THEN MST_AREA ELSE '000' END, 3)+RIGHT('0000000'+CASE WHEN (LEN(MST_PHONE) >0) THEN MST_PHONE ELSE '000' END, 7) AS NUMERIC(10)) FROM CASEMST PHN WHERE PHN.MST_AGENCY = TEST.MST_AGENCY AND PHN.MST_DEPT = TEST.MST_DEPT AND PHN.MST_PROGRAM = TEST.MST_PROGRAM AND PHN.MST_YEAR = TEST.MST_YEAR AND PHN.MST_APP_NO = TEST.MST_APP_NO) AS " + Column_Name; break;

            //        case "MST_STATE":
            //        case "MST_CITY":
            //        case "MST_STREET":
            //        case "MST_SUFFIX":
            //        case "MST_HN":
            //        case "MST_APT":
            //        case "MST_FLR":
            //        case "MST_ZIP":
            //        case "MST_DIRECTION": if (Cb_Use_DIFF.Checked) Custom_Query = "(SELECT DIFF_" + Column_Name.Substring(4, Column_Name.Length - 4) + " FROM CASEDIFF WHERE DIFF_AGENCY = MST_AGENCY AND DIFF_DEPT = MST_DEPT AND DIFF_PROGRAM = MST_PROGRAM AND DIFF_YEAR = MST_YEAR AND DIFF_APP_NO =MST_APP_NO) AS " + Column_Name; break;
            //    }
            //}
            return Custom_Query;
        }



        private void Prepare_Sot_Priority_Columns() // Method to Sort Entity
        {
            List<ListItem> Sort_list = new List<ListItem>();
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            {
                if (Entity.Sort_Order != "0" && Entity.Sort == "Y")
                    Sort_list.Add(new ListItem(Entity.Sort_Order, Entity.Column_Disp_Name));
            }

            Sort_list.Sort(delegate (ListItem p1, ListItem p2) { return p1.Text.CompareTo(p2.Text); });

            //if (Sort_list.Count > 0)
            //{
            //    Lbl_Sort_Cols.Text = " ";
            //    foreach (ListItem List in Sort_list)
            //        Lbl_Sort_Cols.Text += List.Value + ", ";

            //    Lbl_Sort_Cols.Text = Lbl_Sort_Cols.Text.Substring(0, Lbl_Sort_Cols.Text.Length - 2);

            //    label8.Visible = Lbl_Sort_Cols.Visible = true;
            //}
            //else
            //    label8.Visible = Lbl_Sort_Cols.Visible = false;
        }


        int Next_Sort_Order = 1;
        private void Check_Uncheck_Related_Attribute(string Col_Name, int Attribute_Position)
        {
            string Att_List = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
            char Selected_SW = 'N';
            if (Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString().Substring(Attribute_Position, 1) == "Y")
            {
                Crit_SelCol_Grid.CurrentRow.Cells[Col_Name].Value = Img_Blank;
                //Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value = Att_List.Substring(0, Attribute_Position) + "N" + Att_List.Substring(Attribute_Position + 1, 4 - (Attribute_Position + 1));
                Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value = Att_List.Substring(0, Attribute_Position) + "N" + Att_List.Substring(Attribute_Position + 1, 5 - (Attribute_Position + 1));
            }
            else
            {
                Selected_SW = 'Y';
                Crit_SelCol_Grid.CurrentRow.Cells[Col_Name].Value = Img_Tick;
                //Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value = Att_List.Substring(0, Attribute_Position) + "Y" + Att_List.Substring(Attribute_Position + 1, 4 - (Attribute_Position + 1));
                Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value = Att_List.Substring(0, Attribute_Position) + "Y" + Att_List.Substring(Attribute_Position + 1, 5 - (Attribute_Position + 1));
            }

            string MaxLength = "0";
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            {
                if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&   /// Sindhe
                    Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString())
                {
                    switch (Attribute_Position)
                    {
                        case 0:
                            Entity.Can_Add_Col = Selected_SW.ToString();
                            if (Entity.Can_Add_Col == "N")
                            {
                                Crit_SelCol_Grid.CurrentRow.Cells["Sel_Desc"].Value =
                                Crit_SelCol_Grid.CurrentRow.Cells["Sel_Sort"].Value =
                                Crit_SelCol_Grid.CurrentRow.Cells["Sel_Break"].Value = Img_Blank;
                                Entity.Description = Entity.Can_Add_Col;
                                Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value = "NN" + Att_List.Substring(2, 1) + "NN";

                                if (Entity.Description != "Y" && (!string.IsNullOrEmpty(Entity.AgyCode.Trim())))
                                    MaxLength = Entity.Disp_Code_Length;
                                else
                                    MaxLength = Entity.Disp_Desc_Length;

                                Entity.Max_Display_Width = Get_Column_Disp_Width(Entity.Data_Type, (string.IsNullOrEmpty(MaxLength.Trim()) ? 0 : int.Parse(MaxLength)), Entity.Col_Format_Type);
                            }
                            break;
                        case 1:
                            Entity.Description = Selected_SW.ToString();
                            if (Entity.Description != "Y" && (!string.IsNullOrEmpty(Entity.AgyCode.Trim())))
                                MaxLength = Entity.Disp_Code_Length;
                            else
                                MaxLength = Entity.Disp_Desc_Length;

                            Entity.Max_Display_Width = Get_Column_Disp_Width(Entity.Data_Type, (string.IsNullOrEmpty(MaxLength.Trim()) ? 0 : int.Parse(MaxLength)), Entity.Col_Format_Type);
                            break;

                        case 2:
                            Entity.Count = Selected_SW.ToString();
                            break;

                        case 3:
                            Entity.Sort = Selected_SW.ToString();
                            Crit_SelCol_Grid.CurrentRow.Cells["Col_Sort_Order"].Value = Entity.Sort_Order = Next_Sort_Order.ToString();
                            if (Selected_SW.ToString() == "Y")
                                Next_Sort_Order++;
                            else
                            {
                                Next_Sort_Order--;
                                Crit_SelCol_Grid.CurrentRow.Cells["Col_Sort_Order"].Value = Entity.Sort_Order = "0";
                                Decrement_Sort_Orders();
                            }
                            break;
                        case 4:
                            Entity.Break_Order = Selected_SW.ToString();
                            break;

                            //case 0: Entity.Can_Add_Col = Selected_SW.ToString(); break;
                    }
                    Entity.Attributes = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
                    Entity.Display = Entity.Attributes.Substring(0, 1);
                    Entity.Description = Entity.Attributes.Substring(1, 1);
                    Entity.Count = Entity.Attributes.Substring(2, 1);
                    Entity.Sort = Entity.Attributes.Substring(3, 1);
                    //Entity.Break = Entity.Attributes.Substring(4, 1);
                    Entity.Break_Order = Entity.Attributes.Substring(4, 1);
                }
            }

        }

        private void Decrement_Sort_Orders()
        {
            int Tmp_Sort_Order = 1;
            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                if (string.IsNullOrEmpty(dr.Cells["Col_Sort_Order"].Value.ToString().Trim()))
                    dr.Cells["Col_Sort_Order"].Value = "0";

                if (dr.Cells["Col_Sort_Order"].Value.ToString() != "0")
                {
                    Tmp_Sort_Order = int.Parse(dr.Cells["Col_Sort_Order"].Value.ToString());
                    if (Tmp_Sort_Order > Next_Sort_Order)
                        dr.Cells["Col_Sort_Order"].Value = (--Tmp_Sort_Order).ToString();


                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&   /// Sindhe
                            Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString())
                        {
                            Entity.Sort_Order = dr.Cells["Col_Sort_Order"].Value.ToString();
                        }
                    }
                }
            }
        }


        List<ADHOCFLSEntity> Master_Table_List = new List<ADHOCFLSEntity>();
        private void Get_Master_Table_Details()
        {
            Btn_CAMS_Work.Visible = false;
            ADHOCFLSEntity Search_ADHOCFLS = new ADHOCFLSEntity(true);
            if (Cmb_Category.SelectedItem != null)
            {
                Search_ADHOCFLS.Category = ((ListItem)Cmb_Category.SelectedItem).Value.ToString();
                if (Search_ADHOCFLS.Category == "01")
                {
                    //Search_ADHOCFLS.Category = "01";
                    Btn_CAMS_Work.Visible = true;
                }
            }


            Search_ADHOCFLS.Module = Privileges.ModuleCode;
            Master_Table_List = _model.AdhocData.Browse_ADHOCFLS(Search_ADHOCFLS);
            Fill_Tables_Grid();
        }


        private bool Check_ACTMS_Filters()
        {
            bool Can_Generate = false;

            string EqualTo = "", Greater = "", Less = "", NotEqualTo = "", Sel_Table_Name = "", Column_name = "";
            string ACT_Filter = "N", MS_Filter = "MS";
            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                Sel_Table_Name = dr.Cells["Sel_TableName"].Value.ToString();
                EqualTo = (dr.Cells["Sel_Equalto"].Value.ToString() != " " ? dr.Cells["Sel_Equalto"].Value.ToString() : "NULL");
                NotEqualTo = (dr.Cells["Sel_NotEqualto"].Value.ToString() != " " ? dr.Cells["Sel_NotEqualto"].Value.ToString() : "NULL");
                Greater = (dr.Cells["Sel_Greater"].Value.ToString() != " " ? dr.Cells["Sel_Greater"].Value.ToString() : "NULL");
                Less = (dr.Cells["Sel_Less"].Value.ToString() != " " ? dr.Cells["Sel_Less"].Value.ToString() : "NULL");
                Column_name = dr.Cells["Sel_Org_ColNmae"].Value.ToString();



                if (Sel_Table_Name.Trim() == "CASEACT" && (EqualTo != "NULL" || NotEqualTo != "NULL" || Greater != "NULL" || Less != "NULL"))
                    ACT_Filter = "Y";

                if (Sel_Table_Name.Trim() == "CASEMS" && (EqualTo != "NULL" || NotEqualTo != "NULL" || Greater != "NULL" || Less != "NULL"))
                    MS_Filter = "Y";

                if (ACT_Filter == "Y" && MS_Filter == "Y")
                {
                    Can_Generate = true;
                    break;
                }

            }

            return Can_Generate;
        }

        bool SerCustFund = true;
        private void button1_Click(object sender, EventArgs e)
        {

            if (Criteria_SelCol_List.Count > 0)
            {
                //AdhocSel_CriteriaEntity serfundList = Criteria_SelCol_List.Find(u => u.Table_name.Equals("SERCUST"));
                //if (serfundList != null && SerCustFund)
                //{
                //    Add_SelCol_To_List_SERFUND(); SerCustFund = false;
                //}
                //else
                //{
                //    List<AdhocSel_CriteriaEntity> serfundLists = Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SERCUST"));
                //    if (serfundLists.Count == 1)
                //    {
                //        serfundList = serfundLists.Find(u => u.Table_name.Equals("SERCUST") && u.Column_Name.Equals("SERFUND"));
                //        Criteria_SelCol_List.Remove(serfundLists[0]);
                //        SerCustFund = true;
                //    }
                //}

                bool Atleasst_One_Column_DISP_Sel = false;
                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                {
                    if (Entity.Can_Add_Col == "Y")
                    { Atleasst_One_Column_DISP_Sel = true; break; }
                }


                if (Check_ACTMS_Filters())
                {
                    AlertBox.Show("Queries with fields from 'CASEACT' and 'CASEMS' can only have filters on one table, not both.", MessageBoxIcon.Warning); return;
                }

                MSTTab_Sel_Sw = SNPTab_Sel_Sw = ADDCUSTTab_Sel_Sw = PRESRESPTab_Sel_Sw = SERCUSTTab_Sel_sw = false; EMSRESTabSel_Sw = false; EMSCLCPMCTabSel_Sw = false;SALACTTable_Sw = false;CALCONTTable_Sw = false;
                string SelSercustSw = "N";
                SPDBPost_Sw = false; STFMST_Sw = false; STFTRAIN_SW = false;

                //Added by Sudheer on 02/18/2021 for Agency Partner screen
                PartnerTab_SelSw = false; PartRepTab_SelSw = false; PartSerTab_SelSw = false; PartboutiqueTab_SelSw = false;

                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASEMST")).Count > 0) MSTTab_Sel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASESNP")).Count > 0) SNPTab_Sel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("ADDCUST")).Count > 0) ADDCUSTTab_Sel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("PRESRESP")).Count > 0) PRESRESPTab_Sel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("EMSRES")).Count > 0) EMSRESTabSel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("EMSCLCPMC")).Count > 0) EMSCLCPMCTabSel_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SERCUST")).Count > 0)
                {
                    SERCUSTTab_Sel_sw = true;
                    //if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("EMSRES")).Count > 0)
                    //    SelSercustSw = "Y";
                }
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SALACT")).Count > 0) SALACTTable_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CALCONT")).Count > 0) CALCONTTable_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASECONT")).Count > 0) CASECONTTab_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("CASEACT")).Count > 0) CASEACTTab_Sel_SW = true;

                //Added by Sudheer on 02/28/2021 for Agency Partner
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYPART")).Count > 0) PartnerTab_SelSw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYREP")).Count > 0) PartRepTab_SelSw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYSERVICES")).Count > 0) PartSerTab_SelSw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("AGCYBOUTIQUE")).Count > 0) PartboutiqueTab_SelSw = true;


                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("SPDBPOST")).Count > 0) SPDBPost_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("STAFFMST")).Count > 0) STFMST_Sw = true;
                if (Criteria_SelCol_List.FindAll(u => u.Table_name.Equals("STFTRAIN")).Count > 0) STFTRAIN_SW = true;

                //foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)
                //{
                //    switch (Sel_Col_Entity.Table_name)
                //    {
                //        case "CASEMST": MSTTab_Sel_Sw = true; break;
                //        case "CASESNP": SNPTab_Sel_Sw = true; break;
                //        case "ADDCUST": ADDCUSTTab_Sel_Sw = true; break;
                //        case "EMSRES": EMSRESTabSel_Sw = true; break;
                //        case "PRESRESP": PRESRESPTab_Sel_Sw = true; break;
                //        case "SERCUST": SERCUSTTab_Sel_sw = true; break;
                //    }


                //    if (Sel_Col_Entity.Table_name != "ADDCUST" || Sel_Col_Entity.Table_name != "PRESRESP" || Sel_Col_Entity.Table_name != "SERCUST")
                //        ADDCUSTTab_Alone_Sel_Sw = "N";

                //    if (MSTTab_Sel_Sw && SNPTab_Sel_Sw && ADDCUSTTab_Alone_Sel_Sw == "N" && SelSercustSw == "N")
                //        break;

                //    if (SERCUSTTab_Sel_sw && SelSercustSw == "N")
                //    {
                //        //if (!MSTTab_Sel_Sw || !SNPTab_Sel_Sw || !EMSRESTabSel_Sw)
                //        break;
                //    }


                //}




                if (ADDCUSTTab_Sel_Sw || PRESRESPTab_Sel_Sw || SERCUSTTab_Sel_sw )
                {
                    if (!MSTTab_Sel_Sw && !SNPTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from Both 'CASEMST' and 'CASESNP' Tables.", MessageBoxIcon.Warning); return; }
                    else if (!MSTTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from 'CASEMST' Table.", MessageBoxIcon.Warning); return; }
                    else if (!SNPTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from 'CASESNP' Table.", MessageBoxIcon.Warning); return; }
                    else if (!EMSRESTabSel_Sw && (SERCUSTTab_Sel_sw || EMSCLCPMCTabSel_Sw))
                    { AlertBox.Show("Please select at least One column from 'EMSRES' Table.", MessageBoxIcon.Warning); return; }
                }

                if(EMSCLCPMCTabSel_Sw && !EMSRESTabSel_Sw)
                { AlertBox.Show("Please select at least One column from 'EMSRES' Table.", MessageBoxIcon.Warning); return; }

                if (((ListItem)Cmb_Category.SelectedItem).Value.ToString() == "90" )
                {
                    if (CALCONTTable_Sw && (MSTTab_Sel_Sw || SNPTab_Sel_Sw) && !CASECONTTab_Sw)
                    {
                        { AlertBox.Show("Please select at least One column from 'CASECONT' Table.", MessageBoxIcon.Warning); return; }
                    }
                    if (SALACTTable_Sw && (MSTTab_Sel_Sw || SNPTab_Sel_Sw) && !CASEACTTab_Sel_SW)
                    {
                        { AlertBox.Show("Please select at least One column from 'CASEACT' Table.", MessageBoxIcon.Warning); return; }
                    }
                }

                if(PartboutiqueTab_SelSw || PartRepTab_SelSw || PartSerTab_SelSw)
                {
                    if (!PartnerTab_SelSw)
                    { AlertBox.Show("Please select at least One column from 'AGCYPART' Table.", MessageBoxIcon.Warning); return; }
                }

                if (!SPDBPost_Sw && STFMST_Sw && STFTRAIN_SW)
                {
                    AlertBox.Show("Please select at least One column from 'SPDBPOST' Table.", MessageBoxIcon.Warning); return;
                }


                    if (Atleasst_One_Column_DISP_Sel)
                {

                    GetSel_Columns_Width_To_Set_Page();

                    CASB2012_AdhocPageSetup PageSetup_Form = new CASB2012_AdhocPageSetup(total_Columns_Width, total_Columns_Selected_2Display, Privileges.Program);
                    PageSetup_Form.FormClosed += new FormClosedEventHandler(On_Pagesetup_Form_Closed);
                    PageSetup_Form.StartPosition = FormStartPosition.CenterScreen;
                    PageSetup_Form.ShowDialog();
                    // PageSetup_Form.Dispose();
                }
                else
                    AlertBox.Show("Please select at least One column to display in report.", MessageBoxIcon.Warning);
            }
            else
                AlertBox.Show("Please select columns to generate report.", MessageBoxIcon.Warning);
        }

        bool[] Pagesetup_results = new bool[5];
        bool Include_header = false, Include_Footer = false, Include_Header_Title = false, Include_Header_Image = false,
             Include_Footer_PageCnt = false, Save_This_Adhoc_Criteria = false;
        string Rep_Name = " ", Rep_Header_Title = string.Empty, Page_Orientation = "A4 Portrait", Pub_SubRep_Name = string.Empty;
        string Curr_Hie_To_Pass = string.Empty, Curr_Year_To_Pass = string.Empty;

        private void On_Pagesetup_Form_Closed(object sender, FormClosedEventArgs e)
        {
            Include_header = Include_Footer = Include_Header_Title = Include_Header_Image =
            Include_Footer_PageCnt = Save_This_Adhoc_Criteria = false;

            Rep_Name = " "; Rep_Header_Title = string.Empty; Page_Orientation = "A4 Portrait";

            CASB2012_AdhocPageSetup form = sender as CASB2012_AdhocPageSetup;
            if (form.DialogResult == DialogResult.OK)
            {

                try
                {
                    string strOutId = string.Empty;
                    _model.AdhocData.InsertUpdateDelAdhocHistory(BaseForm.UserID, BaseForm.BusinessModuleID, Current_Hierarchy + Program_Year, ((ListItem)Cmb_Category.SelectedItem).Value.ToString(), "ADD", "ADHOC", string.Empty, string.Empty, out strOutId);
                }
                catch (Exception ex)
                {


                }

                Pagesetup_results = form.Get_Checkbox_Status();
                DataTable dt = new DataTable();
                DataTable dt_Summary = new DataTable();
                Include_header = Pagesetup_results[0]; Include_Header_Title = Pagesetup_results[1]; Include_Header_Image = Pagesetup_results[2];
                Include_Footer = Pagesetup_results[3]; Include_Footer_PageCnt = Pagesetup_results[4];
                Save_This_Adhoc_Criteria = Pagesetup_results[5];

                if (Include_Header_Title)
                    Rep_Header_Title = form.Get_Header_Title();

                Pub_SubRep_Name = Rep_Name = "SYSTEM " + form.Get_Report_Name();
                Rep_Name += ".rdlc"; Pub_SubRep_Name += "SummaryReport";

                Page_Orientation = form.Get_Page_Orientation();

                switch (Page_Orientation)
                {
                    case "A4 Portrait": Rb_A4_Port.Checked = true; break;
                    default: Rb_A4_Land.Checked = true; break;
                }

                string Category_Code = ((ListItem)Cmb_Category.SelectedItem).Value.ToString();
                string Secret_SW = ((ListItem)Cmb_Applications.SelectedItem).Value.ToString();
                string Group_Sort_SW = ((ListItem)Cmb_Group_Sort.SelectedItem).Value.ToString();
                string Data_Filter = ((ListItem)Cmb_Dat_Filter.SelectedItem).Value.ToString();
                string Use_Casediff_SW = Cb_Use_DIFF.Checked ? "Y" : "N";
                string Include_Mambers = Cb_Inc_Menbers.Checked ? "Y" : "N";
                Data_Filter = (Data_Filter != "*" ? Data_Filter : string.Empty);

                string[] XML_String = new string[2];
                XML_String = Get_XML_Format_of_Selected_Rows("Generate_Rep");

                string Card_1 = Get_XML_Format_for_Report_Controls();

                if (Summary_Sw)
                    Generete_Dynamic_Summary_RDLC();

                Curr_Hie_To_Pass = Current_Hierarchy; Curr_Year_To_Pass = Program_Year;

                if (((ListItem)Cmb_Category.SelectedItem).Value.ToString() == "02")
                {
                    switch (Category_02_Validation())
                    {
                        case "STAFFPOST": Curr_Year_To_Pass = string.Empty; break;
                        case "STAFFMST": Curr_Hie_To_Pass = Current_Hierarchy.Substring(0, 2) + "    "; Curr_Year_To_Pass = string.Empty; break;
                    }
                }

                if (((ListItem)Cmb_Category.SelectedItem).Value.ToString() == "04")
                { Curr_Hie_To_Pass = Current_Hierarchy.Substring(0, 2) + "    "; Curr_Year_To_Pass = string.Empty; }


                DataSet ds = _model.AdhocData.Get_SelCol_Data(XML_String[0], Secret_SW, Group_Sort_SW, Include_Mambers, Curr_Hie_To_Pass + Curr_Year_To_Pass, XML_String[1], ADDCUSTTab_Alone_Sel_Sw, Card_1, BaseForm.BusinessModuleID, BaseForm.UserID, Data_Filter, Category_Code);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        dt = ds.Tables[3];

                        if (dt.Rows.Count > 0)
                        {
                            if (Summary_Sw && ds.Tables[7].Rows.Count > 0)
                            {
                                Get_SelCountCol_To_List();
                                //Generete_Dynamic_Summary_RDLC();
                                dt_Summary = ds.Tables[7];
                            }

                            Dynamic_RDLC();

                            CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(dt, dt_Summary, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Pub_SubRep_Name);
                            RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
                            RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
                            RDLC_Form.ShowDialog();
                            // RDLC_Form.Dispose();
                            ds = null;
                            dt = null;
                            dt_Summary = null;
                            //Card_List = null;      Can Delete this Line Made Card_List Local to that Method
                            //Table_List = null;
                            //Column_List = null;  
                            //Master_Columns_List = null;
                            //Adhoc_Categories_List = null;   Can Delete this Line Made Adhoc_Categories_List Local to that Method
                            //AgyTabs_List = null;            
                            //Master_Table_List = null;

                            // Entities that are used in Filter Criteria (Condition Sub Panel)
                            //Pass_AgyTabs_List = null;


                            //////Addcust_Cust_Columns = null;     // ADDCUST Table Columns Entity used to fill Columns Grid.                      
                            //////Pass_HS_Funds_List = null;
                            //////Pass_CM_Funds_List = null;
                            //////Users_Table = null;
                            //////Sites_Table = null;
                            //////zipcode_List = null;
                            //////STAFFMST_List = null;
                            //////CaseVddlist = null;
                            //////Activity_Cust_List = null;
                            //////Cont_Cust_List = null;
                            //////Activity_Resp_List = null;
                            //////Cont_Resp_List = null;
                            //////Addcust_Resp_List = null;
                            //////ChldTrck_Tasks_List = null;
                            //////Addcust_SelResp_List = null;
                            //////ServicePlan_List = null;
                            //////Activity_List = null;
                            //////MileStone_List = null;
                            //////CASEREF_List = null;
                            //////Agys_List = null;
                            //////Depts_List = null;
                            //////Progs_List = null;


                            //CASEHIE_Table = null;  Can Delete this Line, CASEHIE_Table Made Local to that Method
                            GC.Collect();
                            GC.SuppressFinalize(RDLC_Form);
                        }
                    }
                    else
                        AlertBox.Show("No Records exist with the selected criteria.", MessageBoxIcon.Warning);
                }
            }
        }

        private string Category_02_Validation()
        {
            string Driving_Table = string.Empty;
            foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)  //
            {
                if (Sel_Col_Entity.Table_name == "CASEMST")
                {
                    Driving_Table = "CASEMST";
                    break;
                }
            }

            if (string.IsNullOrEmpty(Driving_Table))
            {
                foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)  //
                {
                    if (Sel_Col_Entity.Table_name == "STAFFPOST")
                    {
                        Driving_Table = "STAFFPOST";
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(Driving_Table))
            {
                foreach (AdhocSel_CriteriaEntity Sel_Col_Entity in Criteria_SelCol_List)  //
                {
                    if (Sel_Col_Entity.Table_name == "STAFFMST")
                    {
                        Driving_Table = "STAFFMST";
                        break;
                    }
                }
            }

            return Driving_Table;
        }

        DirectoryInfo MyDir;
        private void Delete_Dynamic_RDLC_File(object sender, FormClosedEventArgs e)
        {
            // Test Comment to get Updates from Yeswanth-Laptop
            CASB2012_AdhocRDLCForm form = sender as CASB2012_AdhocRDLCForm;
            //MyDir = new DirectoryInfo(@"C:\Capreports\");
            //MyDir = new DirectoryInfo(Consts.Common.ReportFolderLocation + "\\"); // Run at Server
            //MyDir = new DirectoryInfo(Consts.Common.Tmp_ReportFolderLocation + "\\"); // Run at Server
            MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server

            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            bool MasterRep_Deleted = false, SubReport_Deleted = false;
            Pub_SubRep_Name += ".rdlc";
            if (!Summary_Sw)
                SubReport_Deleted = true;
            FileStream stream = null;
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    if (Rep_Name == MyFile.Name)
                    {
                        stream = MyFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                        if (stream != null)
                        {
                            stream.Close();
                            MyFile.Delete();
                            MasterRep_Deleted = true;

                        }
                        else
                        {
                            MyFile.Delete();
                            MasterRep_Deleted = true;
                        }


                    }

                    if (Summary_Sw)
                    {
                        if (Pub_SubRep_Name == MyFile.Name)
                        {
                            stream = MyFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                            if (stream != null)
                            {
                                stream.Close();
                                MyFile.Delete();
                                SubReport_Deleted = true;
                            }
                            else
                            {
                                MyFile.Delete();
                                SubReport_Deleted = true;
                            }
                        }
                    }

                    if (MasterRep_Deleted && SubReport_Deleted)
                        break;
                }
            }
            GC.Collect();
            GC.SuppressFinalize(form);
        }


        private void Delete_Existing_Main_Rdlc()
        {
            MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server
            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            bool MasterRep_Deleted = false;
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    if (Rep_Name == MyFile.Name)
                    {
                        MyFile.Delete();
                        MasterRep_Deleted = true;
                    }

                    if (MasterRep_Deleted)
                        break;
                }
            }
        }

        private void Delete_Existing_Summary_Rdlc()
        {
            MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server
            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            bool SubReport_Deleted = false;
            string Sub_Rep = Pub_SubRep_Name + ".rdlc";
            if (!Summary_Sw)
                SubReport_Deleted = true;
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    {
                        if (Sub_Rep == MyFile.Name)
                        {
                            MyFile.Delete();
                            SubReport_Deleted = true;
                        }
                    }

                    if (SubReport_Deleted)
                        break;
                }
            }
        }

        private void GetSel_Columns_Width_To_Set_Page()
        {
            total_Columns_Width = 0.2;
            total_Columns_Selected_2Display = 0;
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            {
                if (Entity.Can_Add_Col == "Y")
                {
                    total_Columns_Width = total_Columns_Width + double.Parse(Entity.Max_Display_Width.Substring(0, Entity.Max_Display_Width.Length - 2));
                    total_Columns_Selected_2Display++;
                }
            }

            //if (Rb_A4_Port.Checked)
            //{
            //    if (total_Columns_Width > 8.27)
            //    {
            //        if (total_Columns_Width <= 11)
            //        {
            //            MessageBox.Show("Total Width of selected columns exceeds 'Portrait' dimensions \n So report will be generated with 'Landscape' dimensions...", "CAP Systems");
            //            Rb_A4_Land.Checked = true;
            //        }
            //        else
            //            MessageBox.Show("Total Width of selected columns exceeds both 'Portrait and Landscape' dimensions...", "CAP Systems");
            //    }
            //}
            //else
            //{
            //    if (total_Columns_Width > 11)
            //    {
            //        MessageBox.Show("Total Width of selected columns exceeds 'Landscape' dimensions...", "CAP Systems");
            //    }
            //}
        }



        private void Pb_Edit_Click(object sender, EventArgs e)
        {
            if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Criteria_SW"].Value.ToString() == "Y")
            {
                string Field_Name = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_DispName"].Value.ToString();
                string Format_Type = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Format_Type"].Value.ToString();
                string Data_Type = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString();
                string Max_Length = Crit_SelCol_Grid.CurrentRow.Cells["Sel_MaxLen"].Value.ToString();
                string equal = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value.ToString();
                string Notequal = Crit_SelCol_Grid.CurrentRow.Cells["Sel_NotEqualto"].Value.ToString();
                string greater = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Greater"].Value.ToString();
                string lesser = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Less"].Value.ToString();
                string Agytab_Code = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_AgyTabCode"].Value.ToString();
                string Get_Null_Recs = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Null_Recs"].Value.ToString();
                string col_master_code = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString();
                string Table_Name = Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString();
                //Sel_TableName


                int Delimiter_Length = 0;
                if (!string.IsNullOrEmpty(Agytab_Code.Trim()))
                {
                    if (Agytab_Code.Substring(0, 2) == "DL")
                        Delimiter_Length = int.Parse(Agytab_Code.Substring(2, 2));
                }

                switch (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString())
                {
                    case "MST_ALERT_CODES": Agytab_Code = "00524"; break;
                    case "STF_POSITIONS": Agytab_Code = "00550"; break;


                    case "TMSAPPT_SOURCE_INCOME":
                    case "MST_INCOME_TYPES": Agytab_Code = "00004"; break;

                    case "SNP_NCASHBEN": Agytab_Code = "00037"; break;
                    case "SNP_HEALTH_CODES": Agytab_Code = "00038"; break;
                    case "MST_NCASHBEN": Agytab_Code = "00037"; break;

                    case "AGYP_COUNTIES_SERVED": Agytab_Code = "00525";break;
                    case "AGYP_Target": Agytab_Code = "S0076";break;
                    case "CASEACT_BILL_PERIOD": Agytab_Code = "00202"; break;
                    case "AGYP_SERPLANS": Agytab_Code = "SPLAN"; break;
                    case "AGYR_REP_REQLEAD": Agytab_Code = "SPLAN"; break;
                    case "AGYP_REF_ASOC": Agytab_Code = "S0125"; break;
                }
                if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() == "")


                    Pass_AgyTabs_List.Clear();

                CASB2012_ConditionsForm Conditions_Form;

                if (Format_Type == "X" || Format_Type == "T" || Format_Type == "N")
                {
                    string Additional_Crit = "";
                    if (Data_Type == "Date")
                        Additional_Crit = Get_Null_Recs;

                    if (Field_Name == "Current Age")
                        Additional_Crit = Notequal;



                    Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Format_Type, Data_Type, Max_Length, equal, greater, lesser, Additional_Crit);
                    Conditions_Form.FormClosed += new FormClosedEventHandler(On_ConditionsForm_Closed);
                }

                else if (!string.IsNullOrEmpty(Agytab_Code.Trim()))
                {
                    Get_Pass_Agytabs_List4_Conditions(Agytab_Code);

                    switch (Agytab_Code)
                    {

                        case "WORKR":
                            if (CaseWorker_Table.Rows.Count == 0)
                                Get_CaseWorker_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, CaseWorker_Table); break;


                        case "PRCAT":
                            if (Press_Cat.Rows.Count == 0)
                                Get_PressGrp_Table();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Press_Cat); break;

                        case "PRGRP":
                            if (Press_Grp.Rows.Count == 0)
                                Get_PressGrp_Table();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Press_Grp); break;

                        case "OPRTR":
                            if (Users_Table.Rows.Count == 0)
                                Get_Users_List();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Users_Table); break;

                        case "SITES":
                            if (Sites_Table.Rows.Count == 0)
                                Get_Sites_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Sites_Table); break;

                        case "ZIPLS":
                            if (zipcode_List.Count == 0)
                                Get_Zipcodes_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, zipcode_List); break;

                        case "STFMS":
                            if (STAFFMST_List.Count == 0)
                                Get_STAFFMST_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, STAFFMST_List); break;
                        case "STFTR":
                            if (STAFFTRAIN_List.Count == 0)
                                Get_STAFFTRAIN_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, STAFFTRAIN_List); break;

                        case "REFRL":
                            if (CASEREF_List.Count == 0)
                                Get_Referral_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, CASEREF_List); break;

                        case "PARTN":
                            if (PARTNER_List.Count == 0)
                                Get_Partner_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, PARTNER_List); break;

                        case "CACTI":
                            if (Activity_List.Count == 0)
                                Get_Activity_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Activity_List); break;


                        case "MSTON":
                            if (MileStone_List.Count == 0)
                                Get_MileStone_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, MileStone_List); break;

                        case "SPLAN":
                            if (ServicePlan_List.Rows.Count == 0)
                                Get_ServicePlan_list();

                            if(Table_Name== "AGCYPART" || Table_Name== "AGCYREP")
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, ServicePlan_List, Table_Name); 
                            else
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, ServicePlan_List); 
                            break;

                        case "VENDR":
                            if (CaseVddlist.Count == 0)
                                Get_Vendor_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, CaseVddlist); break;

                        case "SALNM":
                            if (SALDEFLIST.Count == 0)
                                Get_SALDEF_list("S");

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SALDEFLIST); break;

                        case "SALQS":
                            if (SALQUESEntity.Count == 0)
                                Get_SALQUES_list("S");

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SALQUESEntity); break;

                        case "CALNM":
                            if (SALDEFLIST.Count == 0)
                                Get_SALDEF_list("C");

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SALDEFLIST); break;

                        case "CALQS":
                            if (SALQUESEntity.Count == 0)
                                Get_SALQUES_list("C");

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SALQUESEntity); break;

                        case "CST62":
                            if (Activity_Cust_List.Count == 0)
                                Get_ActivityCustom_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Activity_Cust_List); break;

                        case "SAL62":
                            if (SALQUES_List.Count == 0)
                                Get_SAL_CustQuestionList();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SALQUES_List); break;

                        case "CST61":
                            if (Cont_Cust_List.Count == 0)
                                Get_ContactCustom_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Cont_Cust_List); break;

                        //Added by Sudheer on 06/02/2022
                        case "CMBDC":
                            if (CMBDC_List.Count == 0)
                                Get_CMBDC_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, CMBDC_List); break;

                        case "RSP61":
                            if (Cont_Resp_List.Count == 0)
                                Get_Cont_Resp_list();

                            string ques_code = Get_cont_question_code(col_master_code);
                            Cont_Resp_List_filtered.Clear();
                            if (Cont_Resp_List.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ques_code.Trim()))
                                {
                                    foreach (CustRespEntity cont in Cont_Resp_List)
                                    {
                                        if (cont.ResoCode == ques_code)
                                            Cont_Resp_List_filtered.Add(new CustRespEntity(cont));
                                    }
                                }
                            }

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, (!string.IsNullOrEmpty(ques_code.Trim()) ? Cont_Resp_List_filtered : Cont_Resp_List)); break;

                        case "RSP62":
                            if (Activity_Resp_List.Count == 0)
                                Get_Activity_Resp_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Activity_Resp_List); break;

                        case "SAR62":
                            if (SAL_Resp_List.Count == 0)
                                Get_SAl_QuesResp_List();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SAL_Resp_List); break;

                        case "CSTRS":
                            if (CustResr_List.Count == 0)
                                Get_CustResr_Resp_list();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, CustResr_List); break;


                        case "ADCST":
                            if (Addcust_Resp_List.Count == 0)
                                Get_Addcust_Resp_List();

                            if (Addcust_Resp_List.Count > 0)
                                Get_Addcust_SelResp_List();


                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Addcust_SelResp_List); break;

                        case "PREAS":
                            if (Presresp_Resp_List.Count == 0)
                                Get_PREASSES_Resp_List();

                            if (Presresp_Resp_List.Count > 0)
                                Get_PreResp_SelResp_List();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, PresResp_SelResp_List); break;
                        case "SRCST":
                            if (SERCust_Resp_List.Count == 0)
                                Get_SERCUST_Resp_List();

                            if (SERCust_Resp_List.Count > 0)
                                Get_Sercust_SelResp_List();

                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, SERCUST_SelResp_List); break;

                        case "TASKS":
                            if (Table_Name == "HLSMEDI")
                            {
                                if (HLSTrck_Tasks_List.Count == 0)
                                    Get_HTasks_List();
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, HLSTrck_Tasks_List);
                            }
                            else
                            {
                                if (ChldTrck_Tasks_List.Count == 0)
                                    Get_Tasks_List();
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, ChldTrck_Tasks_List);
                            }
                            break;
                        case "HIEAG": Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Agys_List, Current_Hierarchy_DB); break;  //Current_Hierarchy
                        case "HIEDE": Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Depts_List, Current_Hierarchy_DB); break;
                        case "SERVS":
                            if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "0")
                            {
                                if (Activity_List.Count == 0)
                                    Get_Activity_list();

                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Activity_List);
                            }
                            else
                            {
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Progs_List, Current_Hierarchy_DB);
                            }
                            break;

                        case "HIEPR": Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Progs_List, Current_Hierarchy_DB); break;
                        case "ALLPR":
                            if (ALLPR_List.Count == 0)
                                Get_ALLPR_List();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, ALLPR_List, Current_Hierarchy_DB); break;
                        case "SPHIE":
                            if (ALLSPR_List.Count == 0)
                                Get_ALLSPR_List();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, ALLSPR_List, Current_Hierarchy_DB); break;
                        case "MATRX":
                            if (Matrix_List.Count == 0)
                                Get_MATDEF_List();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Matrix_List, Current_Hierarchy_DB); break;

                        case "MATQS":
                            if (MatQues_List.Count == 0)
                                Get_MAT_Ques_List();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, MatQues_List, Current_Hierarchy_DB); break;


                        case "SCALS":
                            if (Scales_List.Count == 0)
                                Get_MATDEF_List();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, Scales_List, Current_Hierarchy_DB); break;

                        case "PRREP":
                            if (AgyRepList.Count == 0)
                                Get_Representatives();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, AgyRepList, Current_Hierarchy_DB); break;

                        case "PRSER":
                            if (AGYSERList.Count == 0)
                                Get_Services();
                            Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Agytab_Code, equal, Notequal, AGYSERList, Current_Hierarchy_DB); break;

                        case "03320":
                            if (Privileges.ModuleCode == "02")
                            {
                                if (Pass_HS_Funds_List.Count == 0)
                                    Get_HS_Funds_List4_Conditions();
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, equal, Notequal, Pass_HS_Funds_List, Delimiter_Length);
                            }
                            else
                            {
                                if (Pass_CM_Funds_List.Count == 0)
                                    Get_CM_Funds_List4_Conditions();
                                Conditions_Form = new CASB2012_ConditionsForm(Field_Name, equal, Notequal, Pass_CM_Funds_List, Delimiter_Length);
                            }
                            break;

                        default: Conditions_Form = new CASB2012_ConditionsForm(Field_Name, equal, Notequal, Pass_AgyTabs_List, Delimiter_Length); break;
                    }

                    Conditions_Form.FormClosed += new FormClosedEventHandler(On_Multiple_ConditionsForm_Closed);
                }
                else
                {
                    string Additional_Crit = "";
                    if (Data_Type == "Date")
                        Additional_Crit = Get_Null_Recs;

                    if (Field_Name == "Current Age")
                        Additional_Crit = Notequal;



                    Conditions_Form = new CASB2012_ConditionsForm(Field_Name, Format_Type, Data_Type, Max_Length, equal, greater, lesser, Additional_Crit);
                    Conditions_Form.FormClosed += new FormClosedEventHandler(On_ConditionsForm_Closed);
                }
                Conditions_Form.StartPosition = FormStartPosition.CenterScreen;
                Conditions_Form.ShowDialog();

                Field_Name = Format_Type =
                Data_Type = Max_Length =
                equal = Notequal =
                greater = lesser = Agytab_Code = null;
                GC.Collect();
            }
            else
                AlertBox.Show("Criteria definition Restricted for selected Column.", MessageBoxIcon.Warning);
        }

        private void On_CAMS_Conditions_Closed(object sender, FormClosedEventArgs e)
        {
            CASB2012_ConditionsForm form = sender as CASB2012_ConditionsForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string toolTipText = null; //Txt_Resp = null, 
                string[] Num_Date_Resp = new string[3];
                Num_Date_Resp = form.Get_CAMS_WorkFile_Date_Cindition();

                CAMS_Date_SW = Num_Date_Resp[0];
                CAMS_From_Date = Num_Date_Resp[1];
                CAMS_To_Date = Num_Date_Resp[2];

                //int Row_Index = Crit_SelCol_Grid.CurrentRow.Index;
                //foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                //{
                //    cell.ToolTipText = "";
                //    cell.ToolTipText = toolTipText;
                //}
                ////Crit_SelCol_Grid.Update();
                //Crit_SelCol_Grid.Rows[Row_Index].Update();
            }
        }


        private void On_ConditionsForm_Closed(object sender, FormClosedEventArgs e)
        {
            CASB2012_ConditionsForm form = sender as CASB2012_ConditionsForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string toolTipText = null; //Txt_Resp = null, 
                string[] Num_Date_Resp = new string[4];
                bool Criteria_Exists = false;

                switch (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString())
                {
                    case "Text":
                        Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value = form.Get_Text_Cindition();
                        toolTipText = "Equal to : " + Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value;
                        if (!string.IsNullOrEmpty(Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value.ToString().Trim()))
                            Criteria_Exists = true;
                        break;

                    case "Numeric":
                        Num_Date_Resp = form.Get_Numeric_Cindition();
                        break;
                    case "Date":
                        Num_Date_Resp = form.Get_Date_Cindition();
                        break;
                    case "Time":
                        Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value = form.Get_Time_Cindition();
                        toolTipText = "Equal to : " + Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value;
                        if (!string.IsNullOrEmpty(Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value.ToString().Trim()))
                            Criteria_Exists = true;
                        break;
                }

                if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Numeric" ||
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Date")
                {
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value = Num_Date_Resp[0];
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Greater"].Value = Num_Date_Resp[1];
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Less"].Value = Num_Date_Resp[2];
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Null_Recs"].Value = Num_Date_Resp[3];


                    if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_DispName"].Value.ToString().Trim() == "Current Age")
                        Crit_SelCol_Grid.CurrentRow.Cells["Sel_NotEqualto"].Value = Num_Date_Resp[3];

                    if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString().Trim() != "ENRL_DATE")
                    {
                        toolTipText = "Greater Than   : " + Num_Date_Resp[1] + "\n" +
                                      "Equal to            : " + Num_Date_Resp[0] + "\n" +
                                      "Less Than        : " + Num_Date_Resp[2] +
                                      (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_DispName"].Value.ToString().Trim() == "Current Age" ? "\n" + "Age as of          : " + Num_Date_Resp[3] : "");
                    }
                    else
                    {
                        toolTipText = "As Of/From Date   :" + Num_Date_Resp[0] + "\n" +
                                      "To Date      : " + Num_Date_Resp[1];
                    }

                    if (!string.IsNullOrEmpty(Num_Date_Resp[0].Trim()) ||
                        !string.IsNullOrEmpty(Num_Date_Resp[1].Trim()) ||
                        !string.IsNullOrEmpty(Num_Date_Resp[2].Trim()) ||
                        !string.IsNullOrEmpty(Num_Date_Resp[3].Trim()))
                        Criteria_Exists = true;
                }

                int Row_Index = Crit_SelCol_Grid.CurrentRow.Index;
                foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                {
                    cell.ToolTipText = "";
                    cell.ToolTipText = toolTipText;
                }
                //Crit_SelCol_Grid.Update();
                Crit_SelCol_Grid.Rows[Row_Index].Update();

                if (Criteria_Exists)
                {
                    Crit_SelCol_Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Blue;
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                            Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                            Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                        {

                            if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Numeric" ||
                                Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Date")
                            {
                                Entity.EqualTo = Num_Date_Resp[0];
                                Entity.GreaterThan = Num_Date_Resp[1];
                                Entity.LessThan = Num_Date_Resp[2];
                                Entity.Get_Nulls = Num_Date_Resp[3];

                                if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_DispName"].Value.ToString().Trim() == "Current Age")
                                    Entity.NotEqualTo = Num_Date_Resp[3];
                            }
                            else if (Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Text" ||
                                     Crit_SelCol_Grid.CurrentRow.Cells["Sel_Datatype"].Value.ToString() == "Time")
                                Entity.EqualTo = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value.ToString();

                            break;
                        }
                    }
                }
                else
                {
                    Crit_SelCol_Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                            Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                            Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                        {
                            Entity.EqualTo = Entity.GreaterThan = Entity.LessThan = Entity.NotEqualTo = " "; break;
                        }

                    }

                    Row_Index = Crit_SelCol_Grid.Rows.Add(" ", " ", " ", " ", Img_Blank, Img_Blank, Img_Blank, Img_Blank, " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ");
                    Crit_SelCol_Grid.Rows.RemoveAt(Row_Index);
                }
            }
            //if (Crit_SelCol_Grid.Rows.Count > 0)
            //    Crit_SelCol_Grid.Rows[0].Selected = true;
        }

        string SelServices_Inq = string.Empty;
        private void On_Multiple_ConditionsForm_Closed(object sender, FormClosedEventArgs e)
        {
            CASB2012_ConditionsForm form = sender as CASB2012_ConditionsForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string toolTipText = null; //Txt_Resp = null, 
                string[] AgyTab_Equal_Not = new string[2];
                bool Criteria_Exists = false;
                string Agytab_Code = Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_AgyTabCode"].Value.ToString();

                if(Agytab_Code== "CACTI" || Agytab_Code== "MSTON")
                    AgyTab_Equal_Not = form.Get_CAMS_Condition();
                else
                    AgyTab_Equal_Not = form.Get_AgyTab_Cindition();

                if (AgyTab_Equal_Not[0] != "" || AgyTab_Equal_Not[1] != " ")
                {
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_Equalto"].Value = AgyTab_Equal_Not[0];
                    Crit_SelCol_Grid.CurrentRow.Cells["Sel_NotEqualto"].Value = AgyTab_Equal_Not[1];

                    toolTipText = "Equal to           : " + AgyTab_Equal_Not[0] + "\n" +
                                  "Not Equal to     : " + AgyTab_Equal_Not[1];

                    if (!string.IsNullOrEmpty(AgyTab_Equal_Not[0].Trim()) ||
                        !string.IsNullOrEmpty(AgyTab_Equal_Not[1].Trim()))
                        Criteria_Exists = true;

                }
                SelServices_Inq = string.Empty;
                int Row_Index = Crit_SelCol_Grid.CurrentRow.Index;
                foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                    cell.ToolTipText = toolTipText;

                Crit_SelCol_Grid.Rows[Row_Index].Update();

                if (Criteria_Exists)
                    Crit_SelCol_Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Blue;
                else
                    Crit_SelCol_Grid.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;

                Row_Index = Crit_SelCol_Grid.Rows.Add(" ", " ", " ", " ", Img_Blank, Img_Blank, Img_Blank, Img_Blank, " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ");
                Crit_SelCol_Grid.Rows.RemoveAt(Row_Index);

                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                {
                    if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                        Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                        Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                    {
                        if (Entity.Column_Name == "MST_SER")
                        {
                            AgyTab_Equal_Not[0] = AgyTab_Equal_Not[0].Replace("'", "");
                            SelServices_Inq =  AgyTab_Equal_Not[0].Trim();
                        }
                        else
                        {
                            Entity.EqualTo = AgyTab_Equal_Not[0];
                            Entity.NotEqualTo = AgyTab_Equal_Not[1];
                        }
                        break;

                    }
                }
            }
            //if (Crit_SelCol_Grid.Rows.Count > 0)
            //    Crit_SelCol_Grid.Rows[0].Selected = true;
        }

        List<AGYTABSEntity> Pass_HS_Funds_List = new List<AGYTABSEntity>();
        List<AGYTABSEntity> Pass_CM_Funds_List = new List<AGYTABSEntity>();
        private void Get_HS_Funds_List4_Conditions()
        {
            Pass_HS_Funds_List.Clear();
            DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            if (dsFund != null && dsFund.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsFund.Tables[0].Rows)
                    Pass_HS_Funds_List.Add(new AGYTABSEntity(row, "AGYTABS"));
            }
        }

        private void Get_CM_Funds_List4_Conditions()
        {
            Pass_CM_Funds_List.Clear();

            string ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    Pass_CM_Funds_List = _model.SPAdminData.GetSP0Funds_adhoc(Current_Hierarchy.Substring(0,2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(4, 2), string.Empty, "Reports",Privileges.UserID);
                else
                    Pass_CM_Funds_List = _model.SPAdminData.GetSP0Funds_adhoc(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2).ToString(), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(4, 2), string.Empty, "Reports", Privileges.UserID);

                if (Pass_CM_Funds_List.Count > 0)
                    Pass_CM_Funds_List = Pass_CM_Funds_List.OrderByDescending(u => u.Active).ThenBy(u => u.Code_Desc.Trim()).ToList();
            }
            else
            {
                DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "C");
                if (dsFund != null && dsFund.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsFund.Tables[0].Rows)
                        Pass_CM_Funds_List.Add(new AGYTABSEntity(row, "AGYTABS"));
                }

                if (Pass_CM_Funds_List.Count > 0)
                    Pass_CM_Funds_List = Pass_CM_Funds_List.OrderByDescending(u => u.Active).ToList();
            }
        }

        List<FUELCNTLEntity> FuelCntlList = new List<FUELCNTLEntity>();
        private void FillFuelcntlList()
        {
            DataSet dsAwards = DatabaseLayer.FuelControlDB.Browse_FuelCntl(Program_Year);
            if (dsAwards != null)
            {
                DataTable dtAwards = dsAwards.Tables[0];
                FuelCntlList.Clear();
                foreach (DataRow Row in dtAwards.Rows)
                {
                    if (Row["FCNTL_YEAR"].ToString().Trim() == Program_Year)
                        FuelCntlList.Add(new FUELCNTLEntity(Row));
                }
            }
        }

        List<AGYTABSEntity> Pass_AgyTabs_List = new List<AGYTABSEntity>();
        
        private void Get_Pass_Agytabs_List4_Conditions(string Sel_Agy_Type)
        {
            Pass_AgyTabs_List.Clear();

            foreach (AGYTABSEntity Entity in AgyTabs_List)
            {
                if (Sel_Agy_Type == "S0048")
                {
                    if (FuelCntlList.Count > 0)
                    {
                        FUELCNTLEntity SelFList = FuelCntlList.Find(u => u.Benefit_Type == "B" && u.Award_Type.ToString() == Entity.Table_Code.ToString());
                        if (SelFList != null && Entity.Tabs_Type == Sel_Agy_Type)
                            Pass_AgyTabs_List.Add(Entity);
                    }
                    
                }
                else if (Entity.Tabs_Type == Sel_Agy_Type)
                    Pass_AgyTabs_List.Add(Entity);
            }
        }


        //            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";

        private void Btn_Clear_All_Click(object sender, EventArgs e)
        {
            _pastselParams = "";
            Next_Sort_Order = 1;
            Criteria_SelCol_List.Clear();
            Crit_SelCol_Grid.Rows.Clear();
            Pb_Edit.Visible = false;
            Table_Grid_SelectionChanged(Table_Grid, EventArgs.Empty);
            Btn_Clear_All.Visible= false;
            //AlertBox.Show("All Selections Cleared Successfully");
        }



        DataTable CaseWorker_Table = new DataTable();
        private void Get_CaseWorker_list()
        {
            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetCaseWorker("1", "**", "**", "**");
            string Agency = string.Empty;
            if(Current_Hierarchy.Substring(0,2).ToString()!="**")
                Agency= Current_Hierarchy.Substring(0,2);

            DataSet ds = Captain.DatabaseLayer.CaseMst.GetAllCaseWorkers(Agency);
            if (ds.Tables.Count > 0)
            {
                CaseWorker_Table = ds.Tables[0];

                
            }
        }

        DataTable Press_Grp = new DataTable();
        DataTable Press_Cat = new DataTable();
        private void Get_PressGrp_Table()
        {
            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetCaseWorker("1", "**", "**", "**");
            DataSet ds = Captain.DatabaseLayer.AdhocDB.Get_PressGrp_Table();
            if (ds.Tables.Count > 0)
            {
                Press_Grp = ds.Tables[0];
                Press_Cat = ds.Tables[1];
            }
        }


        DataTable Users_Table = new DataTable();
        private void Get_Users_List()
        {
            //DataSet ds = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            string Agency=string.Empty;
            if(Current_Hierarchy.Substring(0,2).ToString()!="**")
                Agency=Current_Hierarchy.Substring(0,2);

            DataSet ds = Captain.DatabaseLayer.ADMNB002DB.GetUserNameswithAgency(Agency, string.Empty);
            if (ds.Tables.Count > 0)
            {
                Users_Table = ds.Tables[0];

                if(Users_Table.Rows.Count>0)
                {
                    DataView dv = new DataView(Users_Table);
                    dv.Sort = "PWR_INACTIVE_FLAG";
                    Users_Table = dv.ToTable();
                }
            }


        }


        DataTable Sites_Table = new DataTable();
        private void Get_Sites_list()
        {
            string Agy = Current_Hierarchy.Substring(0, 2), Dept = Current_Hierarchy.Substring(2, 2), Prog = Current_Hierarchy.Substring(4, 2);

           DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(Agy != "**" ? Agy : string.Empty, string.Empty, string.Empty);
            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(Agy != "**" ? Agy : string.Empty, Dept != "**" ? Dept : string.Empty, Prog != "**" ? Prog : string.Empty);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    Sites_Table = ds.Tables[0];
            }
        }

        List<ZipCodeEntity> zipcode_List = new List<ZipCodeEntity>();
        private void Get_Zipcodes_list()
        {
            zipcode_List = _model.ZipCodeAndAgency.GetZipCodeSearch(null, null, null, null);

            zipcode_List = zipcode_List.OrderBy(u => u.InActive).ToList();

        }

        List<STAFFMSTEntity> STAFFMST_List = new List<STAFFMSTEntity>();
        private void Get_STAFFMST_list()
        {
            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            if (!string.IsNullOrEmpty(Program_Year.Trim())) Search_STAFFMST.Year = Program_Year; else Search_STAFFMST.Year = Program_Year;
            STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
        }

        List<STAFFTRAINEntity> STAFFTRAIN_List =new List<STAFFTRAINEntity>();
        private void Get_STAFFTRAIN_list()
        {
            string Agy = Current_Hierarchy.Substring(0, 2), Dept = Current_Hierarchy.Substring(2, 2), Prog = Current_Hierarchy.Substring(4, 2);

            STAFFTRAINEntity Search_STAFFMST = new STAFFTRAINEntity(true);
            if (Agy!="**") Search_STAFFMST.Agency = Agy; if (Dept != "**") Search_STAFFMST.Dept = Dept; if (Prog != "**") Search_STAFFMST.Prog = Prog;
            if (!string.IsNullOrEmpty(Program_Year.Trim())) Search_STAFFMST.Year = Program_Year; else Search_STAFFMST.Year = Program_Year;
            STAFFTRAIN_List = _model.STAFFData.GetSTFTRAIN(Search_STAFFMST.Agency, Search_STAFFMST.Dept, Search_STAFFMST.Prog, Search_STAFFMST.Year,"");
        }


        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_list()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);

            CaseVddlist = CaseVddlist.OrderBy(u => u.Active).ToList();
        }

        List<SaldefEntity> SALDEFLIST = new List<SaldefEntity>();
        private void Get_SALDEF_list(string type)
        {
            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);
            Search_saldef_Entity.SALD_TYPE = type;

            SALDEFLIST = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse", BaseForm.UserID, BaseForm.BaseAdminAgency);
        }

        List<SalquesEntity> SALQUESEntity = new List<SalquesEntity>();
        private void Get_SALQUES_list(string Type)
        {
            SalquesEntity Search_Salques_Entity = new SalquesEntity(true);
            SALQUESEntity = _model.SALDEFData.Browse_SALQUES(Search_Salques_Entity, "Browse");

            //if(SALQUESEntity.Count>0)
            //{
            //    SALQUESEntity=SALQUESEntity.FindAll(u=>u.SALQ_ID.Contains())
            //}
        }


        List<CustfldsEntity> Case2001_Cust_List = new List<CustfldsEntity>();
        private void Get_Case2001_Cust_List()
        {
            Case2001_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("CASE2001", "CUSTFLDS", string.Empty);
            Case2001_Cust_List = Case2001_Cust_List.OrderBy(u => u.Active).ToList();
        }

        List<CustfldsEntity> Case0061_Cust_List = new List<CustfldsEntity>();
        private void Get_Case0061_Cust_List()
        {
            Case0061_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("CASE0061", "CUSTFLDS", string.Empty);
            Case0061_Cust_List = Case0061_Cust_List.OrderBy(u => u.Active).ToList();
        }
        //Added by Sudheer on 01/08/2016
        List<CustfldsEntity> Preasses_Cust_List = new List<CustfldsEntity>();
        private void Get_Preasses_Cust_List()
        {
            Preasses_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("PREASSES", "CUSTFLDS", string.Empty);
            Preasses_Cust_List = Preasses_Cust_List.OrderBy(u => u.Active).ToList();
        }

        //Added by Sudheer on 05/24/2017
        List<CustfldsEntity> SERCUST_Cust_List = new List<CustfldsEntity>();
        private void Get_Sercust_Cust_List()
        {
            SERCUST_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("EMS00030", "CUSTFLDS", string.Empty);
            SERCUST_Cust_List = SERCUST_Cust_List.OrderBy(u => u.Active).ToList();
        }

        List<CustfldsEntity> Activity_Cust_List = new List<CustfldsEntity>();
        private void Get_ActivityCustom_list()
        {
            Activity_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("CASE0063", "CUSTFLDS", string.Empty);
            Activity_Cust_List = Activity_Cust_List.OrderBy(u => u.Active).ToList();
        }

        //added by sudheer on 05/25/2020
        List<SalquesEntity> SALQUES_List = new List<SalquesEntity>();
        private void Get_SAL_CustQuestionList()
        {
            SaldefEntity Search_saldef_Entity = new SaldefEntity(true);

            List<SaldefEntity> SALDEF = _model.SALDEFData.Browse_SALDEF(Search_saldef_Entity, "Browse",BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (SALDEF.Count > 0)
            {
                //SALDEF = SALDEF.FindAll(u => (u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.SALD_HIE.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.SALD_HIE.Contains(BaseForm.BaseAgency + "****") || u.SALD_HIE.Contains("******"))  && u.SALD_SERVICES.Equals(string.Empty) && u.SALD_TYPE.Equals("S") && u.SALD_5QUEST == "Y");


                SalquesEntity Search_Salques_Entity = new SalquesEntity(true);
                //Search_Salques_Entity.SALQ_SALD_ID = SALDEF[0].SALD_ID;
                SALQUESEntity = _model.SALDEFData.Browse_SALQUES(Search_Salques_Entity, "Browse");

                if (SALQUESEntity.Count > 0)
                {
                    foreach(SaldefEntity Entity in SALDEF)
                    {
                        List<SalquesEntity> SelSAlques = SALQUESEntity.FindAll(u => u.SALQ_SALD_ID.Equals(Entity.SALD_ID));

                        if(SelSAlques.Count>0)
                        {
                            SALQUES_List.AddRange(SelSAlques);
                        }
                        

                    }

                    if (SALQUES_List.Count > 0)
                        SALQUES_List = SALQUES_List.FindAll(u => u.SALQ_SEQ != "0");
                }
            }

        }

        List<CustfldsEntity> Cont_Cust_List = new List<CustfldsEntity>();
        private void Get_ContactCustom_list()
        {
            Cont_Cust_List = _model.FieldControls.GetCUSTFLDSByScrCode("CASE0061", "CUSTFLDS", string.Empty);
            Cont_Cust_List = Cont_Cust_List.OrderBy(u => u.Active).ToList();
        }

        List<CustRespEntity> Activity_Resp_List = new List<CustRespEntity>();
        private void Get_Activity_Resp_list()
        {
            Activity_Resp_List = _model.FieldControls.GetCustRespByScrCode("CASE0063");
        }

        //added by sudheer on 05/25/2020
        List<SalqrespEntity> SAL_Resp_List = new List<SalqrespEntity>();
        private void Get_SAl_QuesResp_List()
        {
            SalqrespEntity Search_Salqresp_Entity = new SalqrespEntity(true);
            List<SalqrespEntity> Resp_List = _model.SALDEFData.Browse_SALQRESP(Search_Salqresp_Entity, "Browse");

            if (Resp_List.Count > 0)
                SAL_Resp_List = Resp_List;
        }

        List<CustRespEntity> CustResr_List = new List<CustRespEntity>();
        private void Get_CustResr_Resp_list()
        {
            CustResr_List = _model.FieldControls.GetCustRespByScrCode("");
            
        }

        List<CustRespEntity> Cont_Resp_List = new List<CustRespEntity>();
        List<CustRespEntity> Cont_Resp_List_filtered = new List<CustRespEntity>();
        private void Get_Cont_Resp_list()
        {
            Cont_Resp_List = _model.FieldControls.GetCustRespByScrCode("CASE0061");
        }

        List<CustRespEntity> Addcust_Resp_List = new List<CustRespEntity>();
        private void Get_Addcust_Resp_List()
        {
            Addcust_Resp_List = _model.FieldControls.GetCustRespByScrCode("CASE2001");
        }

        //Added by Sudheer
        List<CustRespEntity> Presresp_Resp_List = new List<CustRespEntity>();
        private void Get_PREASSES_Resp_List()
        {
            Presresp_Resp_List = _model.FieldControls.GetCustRespByScrCode("PREASSES");
        }

        //Added by Sudheer on 24/05/2017
        List<CustRespEntity> SERCust_Resp_List = new List<CustRespEntity>();
        private void Get_SERCUST_Resp_List()
        {
            SERCust_Resp_List = _model.FieldControls.GetCustRespByScrCode("EMS00030");
        }

        List<HierarchyEntity> ALLPR_List = new List<HierarchyEntity>();
        private void Get_ALLPR_List()
        {
            ALLPR_List = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "I");

            if (ALLPR_List.Count > 0 && Current_Hierarchy.Substring(0,2)!="**")
                ALLPR_List = ALLPR_List.FindAll(u => u.Agency == Current_Hierarchy.Substring(0, 2));
            ////Added by sudheer on 04/08/2019
            //ALLPR_List = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");



        }

        List<HierarchyEntity> ALLSPR_List = new List<HierarchyEntity>();
        private void Get_ALLSPR_List()
        {
            //Added by sudheer on 10/08/2021
            if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                ALLSPR_List = _model.lookupDataAccess.GetserHierarchyByUserID(BaseForm.UserProfile.UserID, "S", "Reports", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
            else
                ALLSPR_List = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");
            ////Added by sudheer on 04/08/2019
            //ALLPR_List = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");

            if (ALLSPR_List.Count > 0)
            {
                if(BaseForm.BaseAgencyControlDetails.SerPlanAllow=="D")
                {
                    if (Current_Hierarchy.Substring(2, 2) != "**")
                        ALLSPR_List = ALLSPR_List.FindAll(u => u.Agency == Current_Hierarchy.Substring(0, 2) && u.Dept == Current_Hierarchy.Substring(2, 2));
                    else if (Current_Hierarchy.Substring(0, 2) != "**" && Current_Hierarchy.Substring(2, 2) == "**")
                        ALLSPR_List = ALLSPR_List.FindAll(u => u.Agency == Current_Hierarchy.Substring(0, 2));
                    
                }
                else if(BaseForm.BaseAgencyControlDetails.SerPlanAllow == "A")
                {
                    if (Current_Hierarchy.Substring(0, 2) != "**")
                        ALLSPR_List = ALLSPR_List.FindAll(u => u.Agency == Current_Hierarchy.Substring(0, 2));
                }
                
                ALLSPR_List = ALLSPR_List.FindAll(u => !u.Code.Contains("**"));
            }

        }

        List<MATDEFEntity> Scales_List = new List<MATDEFEntity>();
        List<MATDEFEntity> Matrix_List = new List<MATDEFEntity>();
        private void Get_MATDEF_List()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            //Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> MATDEF_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            foreach (MATDEFEntity Ent in MATDEF_List)
            {
                if (Ent.Scale_Code == "0")
                    Matrix_List.Add(new MATDEFEntity(Ent));
                else
                    Scales_List.Add(new MATDEFEntity(Ent));
            }

            if(Matrix_List.Count>0)
                Matrix_List= Matrix_List.OrderBy(u => u.Active).ToList();

        }

        List<MATQUESTEntity> MatQues_List = new List<MATQUESTEntity>();
        private void Get_MAT_Ques_List()
        {
            MATQUESTEntity Search_Entity = new MATQUESTEntity(true);
            //Search_Entity.Scale_Code = "0";
            MatQues_List = _model.MatrixScalesData.Browse_MATQUEST(Search_Entity, "Browse");
            //foreach (MATQUESTEntity Ent in MatQues)
            //{
            //    if (Ent.Scale_Code == "0")
            //        MatQues_List.Add(new MATDEFEntity(Ent));
            //    else
            //        MatQues_List.Add(new MATDEFEntity(Ent));
            //}

            MatQues_List = MatQues_List.OrderByDescending(u => u.QuesActive).ToList();
        }


        List<ChldTrckEntity> ChldTrck_Tasks_List = new List<ChldTrckEntity>();
        private void Get_Tasks_List()
        {
            ChldTrck_Tasks_List = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
        }

        List<HlsTrckEntity> HLSTrck_Tasks_List = new List<HlsTrckEntity>();
        private void Get_HTasks_List()
        {
            HLSTrck_Tasks_List = _model.HlsTrckData.GetHlstrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);
        }

        List<AGCYSEREntity> AGYSERList = new List<Model.Objects.AGCYSEREntity>();
        private void Get_Services()
        {
            AGCYSEREntity SearchSub_Entity = new AGCYSEREntity(true);
            AGYSERList = _model.SPAdminData.Browse_AGCYServices(SearchSub_Entity, "Browse");
        }

        List<AGCYREPEntity> AgyRepList = new List<Model.Objects.AGCYREPEntity>();
        private void Get_Representatives()
        {
            AGCYREPEntity SearchRep_Entity = new AGCYREPEntity(true);
            AgyRepList = _model.SPAdminData.Browse_AGCYREP(SearchRep_Entity, "Browse");
        }

        List<CustRespEntity> Addcust_SelResp_List = new List<CustRespEntity>();
        private void Get_Addcust_SelResp_List()
        {
            Addcust_SelResp_List.Clear();
            foreach (CustRespEntity Entity in Addcust_Resp_List)
            {
                if (Entity.ResoCode == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString())
                    Addcust_SelResp_List.Add(new CustRespEntity(Entity));
            }
        }

        //Added By Sudheer on 01/08/2016
        List<CustRespEntity> PresResp_SelResp_List = new List<CustRespEntity>();
        private void Get_PreResp_SelResp_List()
        {
            PresResp_SelResp_List.Clear();
            foreach (CustRespEntity Entity in Presresp_Resp_List)
            {
                if (Entity.ResoCode == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString())
                    PresResp_SelResp_List.Add(new CustRespEntity(Entity));
            }
        }

        //Added By Sudheer on 05/25/2017
        List<CustRespEntity> SERCUST_SelResp_List = new List<CustRespEntity>();
        private void Get_Sercust_SelResp_List()
        {
            SERCUST_SelResp_List.Clear();
            foreach (CustRespEntity Entity in SERCust_Resp_List)
            {
                if (Entity.ResoCode == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString())
                    SERCUST_SelResp_List.Add(new CustRespEntity(Entity));
            }
        }


        //List<CASESP0Entity> ServicePlan_List = new List<CASESP0Entity>();
        DataTable ServicePlan_List = new DataTable();
        private void Get_ServicePlan_list()
        {
            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP0(null, null, null, null, null, null, null, null, null);
            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();

            ///Added by Sudheer on 10/14/2021 based on service plan hie
            if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()=="Y")
            {
                
                if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, Current_Hierarchy.Substring(0,2) == "**" ? null : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2,2)=="**"?null:Current_Hierarchy.Substring(2,2), null, BaseForm.UserID);
                else
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, Current_Hierarchy.Substring(0, 2) == "**" ? null : Current_Hierarchy.Substring(0, 2), null, null, BaseForm.UserID);

                DataTable dtSP = new DataTable();
                //dtSP = ServicePlan_List;
                if (SP_Hierarchies.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                        ServicePlan_List = ds.Tables[0];

                    if (ServicePlan_List.Rows.Count > 0)
                    {
                        for (int i = ServicePlan_List.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = ServicePlan_List.Rows[i];
                            CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                            if (Entity == null)
                                dr.Delete();
                        }
                        ServicePlan_List.AcceptChanges();

                        //foreach(DataRow dr in ServicePlan_List.Rows)
                        //{
                        //    CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                        //    if (Entity == null) dtSP.Rows.Remove(dr);
                        //}
                    }
                }
                ///if (dtSP.Rows.Count > 0) ServicePlan_List = dtSP;

            }
            else
            {
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, Current_Hierarchy.Substring(0, 2) == "**" ? null : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? null : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? null : Current_Hierarchy.Substring(4, 2));

                DataTable dtSP = new DataTable();
                //dtSP = ServicePlan_List;
                if (SP_Hierarchies.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                        ServicePlan_List = ds.Tables[0];

                    if (ServicePlan_List.Rows.Count > 0)
                    {
                        for (int i = ServicePlan_List.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = ServicePlan_List.Rows[i];
                            CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                            if (Entity == null)
                                dr.Delete();
                        }
                        ServicePlan_List.AcceptChanges();

                        //foreach(DataRow dr in ServicePlan_List.Rows)
                        //{
                        //    CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                        //    if (Entity == null) dtSP.Rows.Remove(dr);
                        //}
                    }
                }

                //if (Current_Hierarchy.Substring(0, 2) != "**")
                //{
                //    List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();
                //    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, Current_Hierarchy.Substring(0, 2), null, null, BaseForm.UserID);
                //    DataTable dtSP = new DataTable();
                //    //dtSP = ServicePlan_List;
                //    if (SP_Hierarchies.Count > 0)
                //    {
                //        if (ds.Tables.Count > 0)
                //            ServicePlan_List = ds.Tables[0];

                //        if (ServicePlan_List.Rows.Count > 0)
                //        {
                //            for (int i = ServicePlan_List.Rows.Count - 1; i >= 0; i--)
                //            {
                //                DataRow dr = ServicePlan_List.Rows[i];
                //                CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                //                if (Entity == null)
                //                    dr.Delete();
                //            }
                //            ServicePlan_List.AcceptChanges();

                //            //foreach(DataRow dr in ServicePlan_List.Rows)
                //            //{
                //            //    CASESP1Entity Entity = SP_Hierarchies.Find(u => u.Code.Trim() == dr["sp0_servicecode"].ToString().Trim());
                //            //    if (Entity == null) dtSP.Rows.Remove(dr);
                //            //}
                //        }
                //    }
                //}
                //else
                //{
                //    if (ds.Tables.Count > 0)
                //        ServicePlan_List = ds.Tables[0];
                ////}
            }

        }

        List<CAMASTEntity> Activity_List = new List<CAMASTEntity>();
        private void Get_Activity_list()
        {
            string ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                {
                    ACR_SERV_Hies = "S";
                    Activity_List = _model.SPAdminData.CAP_SEROUTCOMES(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(4, 2), "CA", Privileges.UserID);
                }
                else
                    Activity_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
            }
            else
                Activity_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
            
            //if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            //{
            //    if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
            //        Activity_List = _model.SPAdminData.CAP_SEROUTCOMES(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), string.Empty, "CA");
            //    else
            //        Activity_List = _model.SPAdminData.CAP_SEROUTCOMES(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), string.Empty, string.Empty, "CA");
            //}
            //else


            //Activity_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
        }

        List<MSMASTEntity> MileStone_List = new List<MSMASTEntity>();
        private void Get_MileStone_list()
        {
            string ACR_SERV_Hies = string.Empty;
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                {
                    ACR_SERV_Hies = "S";

                    MileStone_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(4, 2), "MS", Privileges.UserID);
                }
                else
                    MileStone_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            }
            else
                MileStone_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);

            //if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            //{
            //    if (BaseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
            //        MileStone_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), string.Empty, "MS");
            //    else
            //        MileStone_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), string.Empty, string.Empty, "MS");
            //}
            //else
            //MileStone_List = _model.SPAdminData.CAP_SEROUTCOMES_MS(Current_Hierarchy.Substring(0, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2) == "**" ? string.Empty : Current_Hierarchy.Substring(4, 2), "MS",Privileges.UserID);

            //MileStone_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
        }


        List<CASEREFEntity> CASEREF_List = new List<CASEREFEntity>();
        private void Get_Referral_list()
        {
            CASEREFEntity Search_Entity = new CASEREFEntity(true);
            CASEREF_List = _model.SPAdminData.Browse_CASEREF(Search_Entity, "Browse");
        }

        List<AGCYPARTEntity> PARTNER_List = new List<AGCYPARTEntity>();
        private void Get_Partner_list()
        {
            AGCYPARTEntity Search_Entity = new AGCYPARTEntity(true);
            PARTNER_List = _model.SPAdminData.Browse_AgencyPartner(Search_Entity, "Browse");
        }

        //Added by Sudheer on 06/02/2022 for CMBDC
        List<CMBDCEntity> CMBDC_List = new List<CMBDCEntity>();
        private void Get_CMBDC_list()
        {
            CMBDCEntity Search_Entity = new CMBDCEntity();
            CMBDC_List = _model.SPAdminData.GetCMBdcAllData(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        List<CaseHierarchyEntity> Agys_List = new List<CaseHierarchyEntity>();
        List<CaseHierarchyEntity> Depts_List = new List<CaseHierarchyEntity>();
        List<CaseHierarchyEntity> Progs_List = new List<CaseHierarchyEntity>();
        private void Get_CASEHIE_list()
        {
            //CASEHIE_List = _model.AdhocData.Browse_CASEHIE("**", "**", "**");
            DataTable CASEHIE_Table = new DataTable();
            DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie("**-**-**", BaseForm.UserID, BaseForm.BaseAdminAgency,"");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CASEHIE_Table = ds.Tables[0];
                    foreach (DataRow dr in CASEHIE_Table.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString()) && string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim()) && string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim()))
                            Agys_List.Add(new CaseHierarchyEntity(dr));
                        else
                            if (!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString()) && !string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim()) && string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim()))
                            Depts_List.Add(new CaseHierarchyEntity(dr));
                        else
                                if (!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString()) && !string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim()) && !string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim()))
                            Progs_List.Add(new CaseHierarchyEntity(dr));
                    }
                }
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            ////HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", Current_Hierarchy_DB, string.Empty, "Reports");
            ////hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////hierarchieSelectionForm.ShowDialog();

            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();

        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";

        private void cmbSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbSP.Items.Count>0)
            {
                CategoryCode = ((ListItem)cmbSP.SelectedItem).Value.ToString(); ;


                Fill_Columns_Grid();
            }
        }

        private void CASB2012_AdhocForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);

                    ALLSPR_List.Clear();
                    ServicePlan_List.Clear();
                    Pass_CM_Funds_List.Clear();
                    Pass_HS_Funds_List.Clear();
                    Sites_Table.Rows.Clear();
                    MileStone_List.Clear();
                    Activity_List.Clear();
                    STAFFMST_List.Clear();
                }
            }
        }


        string Sel_AGY, Sel_DEPT, Sel_PROG = string.Empty;
        string Sel_Secret_App, Sel_Group_Sort, Sel_Use_CASEDIFF, Sel_Include_Members = string.Empty;
        private void Get_Report_Selection_Parameters()
        {
            Sel_AGY = Current_Hierarchy.Substring(0, 2);
            Sel_DEPT = Current_Hierarchy.Substring(2, 2);
            Sel_PROG = Current_Hierarchy.Substring(4, 2);

            switch (((ListItem)Cmb_Applications.SelectedItem).Value.ToString())
            {
                case "1": Sel_Secret_App = "Non-Secret Only"; break;
                case "2": Sel_Secret_App = "Secret Only"; break;
                case "3": Sel_Secret_App = "Both Secret & Non-Secret"; break;
            }

            switch (((ListItem)Cmb_Group_Sort.SelectedItem).Value.ToString())
            {
                case "ASC": Sel_Group_Sort = "Sort Fields : Ascending"; break;
                case "DESC": Sel_Group_Sort = "Sort Fields : Descending"; break;
            }

            Sel_Use_CASEDIFF = "Use CASEDIFF : No";
            if (Cb_Use_DIFF.Checked)
                Sel_Use_CASEDIFF = "Use CASEDIFF : Yes";


            Sel_Include_Members = "Include All Members : No";
            if (Cb_Inc_Menbers.Checked)
                Sel_Include_Members = "Include All Members : Yes";
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       Dynamic RDLC    >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        private void Dynamic_RDLC()
        {

            Get_Report_Selection_Parameters();

            XmlNode xmlnode;

            XmlDocument xml = new XmlDocument();
            xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xml.AppendChild(xmlnode);

            XmlElement Report = xml.CreateElement("Report");
            Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            xml.AppendChild(Report);

            XmlElement DataSources = xml.CreateElement("DataSources");
            XmlElement DataSource = xml.CreateElement("DataSource");
            DataSource.SetAttribute("Name", "CaptainDataSource");
            DataSources.AppendChild(DataSource);

            Report.AppendChild(DataSources);

            XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
            DataSource.AppendChild(ConnectionProperties);

            XmlElement DataProvider = xml.CreateElement("DataProvider");
            DataProvider.InnerText = "System.Data.DataSet";


            XmlElement ConnectString = xml.CreateElement("ConnectString");
            ConnectString.InnerText = "/* Local Connection */";
            ConnectionProperties.AppendChild(DataProvider);
            ConnectionProperties.AppendChild(ConnectString);

            //string SourceID = "rd:DataSourceID";
            //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
            //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
            //DataSource.AppendChild(DataSourceID);

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

            XmlElement DataSets = xml.CreateElement("DataSets");
            Report.AppendChild(DataSets);

            XmlElement DataSet = xml.CreateElement("DataSet");
            DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
            DataSets.AppendChild(DataSet);

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

            XmlElement Fields = xml.CreateElement("Fields");
            DataSet.AppendChild(Fields);

            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            {
                if (Entity.Can_Add_Col == "Y")   // 09062012
                {
                    XmlElement Field = xml.CreateElement("Field");

                   Field.SetAttribute("Name", Entity.Column_Name);

                    if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
                        Field.SetAttribute("Name", Entity.Column_Name);
                    //Field.SetAttribute("Name", Entity.Column_Name + "_DESC");

                    Fields.AppendChild(Field);

                    XmlElement DataField = xml.CreateElement("DataField");
                    DataField.InnerText = Entity.Column_Name;
                    if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
                        DataField.InnerText = Entity.Column_Name;
                    //DataField.InnerText = Entity.Column_Name + "_DESC";

                    Field.AppendChild(DataField);

                    //XmlElement DataField_RD = xml.CreateElement("rd");
                    //DataField_RD.InnerText = "System." + Entity.Disp_Data_Type;
                    //Field.AppendChild(DataField_RD);
                }
            }

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

            XmlElement Query = xml.CreateElement("Query");
            DataSet.AppendChild(Query);

            XmlElement DataSourceName = xml.CreateElement("DataSourceName");
            DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
            Query.AppendChild(DataSourceName);

            XmlElement CommandText = xml.CreateElement("CommandText");
            CommandText.InnerText = "/* Local Query */";
            Query.AppendChild(CommandText);


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
            //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            XmlElement Body = xml.CreateElement("Body");
            Report.AppendChild(Body);


            XmlElement ReportItems = xml.CreateElement("ReportItems");
            Body.AppendChild(ReportItems);

            XmlElement Height = xml.CreateElement("Height");
            //Height.InnerText = "4.15625in";       // Landscape
            Height.InnerText = "2in";           // Portrait
            Body.AppendChild(Height);


            XmlElement Style = xml.CreateElement("Style");
            Body.AppendChild(Style);

            XmlElement Border = xml.CreateElement("Border");
            Style.AppendChild(Border);

            XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
            BackgroundColor.InnerText = "White";
            Style.AppendChild(BackgroundColor);


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
            Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
            ReportItems.AppendChild(Sel_Rectangle);

            XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
            Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


            List<AdhocSel_CriteriaEntity> SelectionRecs = Criteria_SelCol_List;//Criteria_SelCol_List.FindAll(u => u.GreaterThan.Trim() != "" || u.LessThan.Trim() != "" || u.EqualTo.Trim() != "");

            int RecsCount = SelectionRecs.Count * 2;
            double Total_Sel_TextBox_Height = 0.16667;
            string Tmp_Sel_Text = string.Empty;
            Regex reg = new Regex("[*'\"_&#^@]");
            int Rect_Total_Rows = (Rb_A4_Port.Checked ? 50 : (20+RecsCount));
            int f = 0;
            for (int i = 0; i < Rect_Total_Rows; i++)
            {
                XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
                Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
                Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

                XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
                Textbox1_Cangrow.InnerText = "true";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

                XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
                Textbox1_Keep.InnerText = "true";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

                XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

                XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
                Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

                XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
                Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


                XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
                Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

                XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

                Tmp_Sel_Text = string.Empty;
                string Sel1 = string.Empty; string sel2 = string.Empty;
                if (i>19 && SelectionRecs.Count>0)
                {
                    if (f < SelectionRecs.Count) //&& f<10
                    {
                        Sel1 = "            "+SelectionRecs[f].Column_Disp_Name;
                        if (!string.IsNullOrEmpty(SelectionRecs[f].EqualTo.Trim())) sel2 = " : "+SelectionRecs[f].EqualTo.Trim();
                        if (!string.IsNullOrEmpty(SelectionRecs[f].GreaterThan.Trim())) sel2 = sel2 +  " >= " + SelectionRecs[f].GreaterThan.Trim();
                        if (!string.IsNullOrEmpty(SelectionRecs[f].LessThan.Trim())) sel2 = sel2 + " <= " + SelectionRecs[f].LessThan.Trim();

                        if (!string.IsNullOrEmpty(sel2.Trim())) { sel2 = reg.Replace(sel2, string.Empty); }
                        // sel2.Replace("'", ""); sel2 = sel2.Replace("", "");

                        //if (string.IsNullOrEmpty(sel2.Trim())) { Tmp_Sel_Text = Sel1; f = f + 1; }
                        //else
                        //{
                        //    if (i % 2 != 0) { f = f + 1; Tmp_Sel_Text = sel2.Trim(); } else { Tmp_Sel_Text = Sel1;}
                        //}


                        if (i % 2 != 0) { f = f + 1; Tmp_Sel_Text = sel2.Trim(); } else { Tmp_Sel_Text = Sel1; }

                        //Tmp_Sel_Text = Sel1+"       " + sel2; f = f + 1;
                    }
                    
                }

                switch (i)
                {
                    //ADDED BY SUDHEER ON 03/11/2019

                    case 1:Tmp_Sel_Text = "Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3") +"                                                                                             "+ "Date : " + DateTime.Now.ToString(); break;

                    case 2: Tmp_Sel_Text = "--------------------------------------------------------------------------------------------------------------------------------------------------------------" ; break;

                    case 3: Tmp_Sel_Text = "Selected Report Parameters"; break;

                    case 5: Tmp_Sel_Text = "            Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

                    case 8: Tmp_Sel_Text = "            Report Format"; break;
                    case 9: Tmp_Sel_Text = " : Detailed Report"; break;
                    case 10: Tmp_Sel_Text = "            Applications"; break;
                    case 11: Tmp_Sel_Text = " : " + Sel_Secret_App; break;
                    case 12: Tmp_Sel_Text = "            Sort Order"; break;
                    case 13: Tmp_Sel_Text = " : " + Sel_Group_Sort; break;
                    case 14: Tmp_Sel_Text = "            Use CASEDIFF"; break;
                    case 15: Tmp_Sel_Text = " : " + (Cb_Use_DIFF.Checked ? "Yes" : "No"); break;
                    case 16: Tmp_Sel_Text = "            Include All Members"; break;
                    case 17: Tmp_Sel_Text = " : " + (Cb_Inc_Menbers.Checked ? "Yes" : "No"); break;
                    default: if (string.IsNullOrEmpty(Tmp_Sel_Text.Trim())) Tmp_Sel_Text = "  "; else Tmp_Sel_Text= Tmp_Sel_Text; break;

                        //case 1: Tmp_Sel_Text = "Selected Report Parameters"; break;

                        //case 3: Tmp_Sel_Text = "            Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

                        //case 6: Tmp_Sel_Text = "            Report Format"; break;
                        //case 7: Tmp_Sel_Text = " : Detailed Report"; break;
                        //case 8: Tmp_Sel_Text = "            Applications"; break;
                        //case 9: Tmp_Sel_Text = " : " + Sel_Secret_App; break;
                        //case 10: Tmp_Sel_Text = "            Sort Order"; break;
                        //case 11: Tmp_Sel_Text = " : " + Sel_Group_Sort; break;
                        //case 12: Tmp_Sel_Text = "            Use CASEDIFF"; break;
                        //case 13: Tmp_Sel_Text = " : " + (Cb_Use_DIFF.Checked ? "Yes" : "No"); break;
                        //case 14: Tmp_Sel_Text = "            Include All Members"; break;
                        //case 15: Tmp_Sel_Text = " : " + (Cb_Inc_Menbers.Checked ? "Yes" : "No"); break;
                        //default: Tmp_Sel_Text = "  "; break;
                }

                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

                XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                if(i==1 || i==2)
                    Textbox1_TextRun_Style_Color.InnerText = "Black";
                else
                    Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
                Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


                XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


                XmlElement Textbox1_Top = xml.CreateElement("Top");
                Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

                //XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                //Total_Sel_TextBox_Height += 0.21875;

                //modified by sudheer on 03/11/2019
                XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                if (i > 19 && string.IsNullOrEmpty(sel2.Trim()) && !string.IsNullOrEmpty(Sel1.Trim()))
                    Textbox1_Left.InnerText = (i % 2 == 0 ? "0.07292in" : "2.27292in");
                else
                    Textbox1_Left.InnerText = ((i > 6 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                if (i > 6 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                {
                    if (i % 2 != 0)
                        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                }
                else
                    Total_Sel_TextBox_Height += 0.21855;

                //XmlElement Textbox1_Left = xml.CreateElement("Left");
                ////Textbox1_Left.InnerText = "0.07292in";
                //Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                //if (i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                //{
                //    if (i % 2 != 0)
                //        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                //}
                //else
                //    Total_Sel_TextBox_Height += 0.21855;


                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? (10 < total_Columns_Width ? "10in" : total_Columns_Width.ToString() + "in") : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "10in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);


                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                if (i > 19 && string.IsNullOrEmpty(sel2.Trim()) && !string.IsNullOrEmpty(Sel1.Trim()))
                    Textbox1_Width.InnerText = i % 2 == 0 ? "2.2in" : "4.48777in";
                else
                    Textbox1_Width.InnerText = ((i > 6 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Style = xml.CreateElement("Style");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

                XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
                Textbox1_Style.AppendChild(Textbox1_Style_Border);

                XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
                Textbox1_Style_Border_Style.InnerText = "None";
                Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

                XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
                Textbox1_Style_PaddingLeft.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

                XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
                Textbox1_Style_PaddingRight.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

                XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
                Textbox1_Style_PaddingTop.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

                XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
                Textbox1_Style_PaddingTop.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

                //if (i > 17 && string.IsNullOrEmpty(sel2.Trim())) i = i + 1;

            }

            XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

            XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
            Break_After_SelParamRectangle_Location.InnerText = "End";
            Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

            XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
            Sel_Rectangle_KeepTogether.InnerText = "true";
            Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

            XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
            Sel_Rectangle_Top.InnerText = "0.2408in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");  // RectWidth
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.48777 ? (10 < total_Columns_Width ? "10in" : total_Columns_Width.ToString() + "in") : "7.48777in"); // "6.35055in";

            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.48777 ? "10in" : "7.48777in"); // "6.35055in";
            Sel_Rectangle_Width.InnerText = (total_Columns_Width > 8 ? "10in" : "7.48777in"); // "6.35055in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

            XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
            Sel_Rectangle_ZIndex.InnerText = "1";
            Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

            XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
            Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

            XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
            Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

            XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
            Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
            Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)                    // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add_Col == "Y")
                //if (Entity.Can_Add_Col == "Y" && Entity.Column_Name != "SNP_APP")   // Rao 11182015 for Group Testing
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");

                    Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width

                    //Col_Width.InnerText = "1in";        // Dynamic based on Display Columns Width
                    TablixColumn.AppendChild(Col_Width);
                    //total_Columns_Width = total_Columns_Width + double.Parse(Entity.Max_Display_Width);
                }
            }




            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add_Col == "Y")
                //if (Entity.Can_Add_Col == "Y" && Entity.Column_Name != "SNP_APP")   // Rao 11182015 for Group Testing
                {
                    //Entity.Column_Name;
                    Tmp_Loop_Cnt++;

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells.AppendChild(TablixCell);


                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    //if (Entity.Col_Format_Type == "C")
                    //    Field_type = "Checkbox";

                    XmlElement Textbox = xml.CreateElement(Field_type);
                    Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
                    CellContents.AppendChild(Textbox);

                    XmlElement CanGrow = xml.CreateElement("CanGrow");
                    CanGrow.InnerText = "true";
                    Textbox.AppendChild(CanGrow);

                    XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                    KeepTogether.InnerText = "true";
                    Textbox.AppendChild(KeepTogether);



                    XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                    Textbox.AppendChild(Paragraphs);

                    XmlElement Paragraph = xml.CreateElement("Paragraph");
                    Paragraphs.AppendChild(Paragraph);



                    XmlElement TextRuns = xml.CreateElement("TextRuns");
                    Paragraph.AppendChild(TextRuns);

                    XmlElement TextRun = xml.CreateElement("TextRun");
                    TextRuns.AppendChild(TextRun);

                    XmlElement Return_Value = xml.CreateElement("Value");

                    Tmp_Disp_Column_Name = Entity.Column_Disp_Name;

                    switch (Entity.Col_Format_Type)
                    {
                        case "F":
                            if (Tmp_Disp_Column_Name != "Service Time" && Tmp_Disp_Column_Name != "Transport Time")
                            {
                                Tmp_Disp_Column_Name += "($)";
                                //Entity.Disp_Length = "10";
                                Entity.Disp_Desc_Length = "10";
                            }
                            break;
                    }


                    Disp_Col_Substring_Len = Tmp_Disp_Column_Name.Length;
                    //if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Length))
                    //    Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Length));

                    //if (Entity.Description != "Y" && (!string.IsNullOrEmpty(Entity.AgyCode.Trim())))
                    //{
                    //    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
                    //        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
                    //        //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
                    //}
                    //else
                    //{
                    //    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Desc_Length))
                    //        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));

                    //}

                    if (!string.IsNullOrEmpty(Entity.AgyCode.Trim()))
                    {
                        if (Entity.Description != "Y")
                        {
                            if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
                                Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
                            //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
                        }
                        else
                            Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));
                    }
                    else
                        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));


                    Return_Value.InnerText = Tmp_Disp_Column_Name;                                    // Dynamic Column Heading

                    // Commented on 05/28/2015 Because Lisa asked full length Header
                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading


                    //Return_Value.InnerText = Entity.Column_Name;                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);


                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Center";
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);


                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight);


                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                    Textbox1_TextRun_Style_Color.InnerText = "#2D17EB";
                    //Textbox1_TextRun_Style_Color.InnerText = "Blue";
                    Cell_style.AppendChild(Textbox1_TextRun_Style_Color);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");     // Table Header Border Colour
                    Border_Color.InnerText = "Black"; // "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);

                    XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                    PaddingLeft.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingLeft);

                    XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                    PaddingRight.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingRight);

                    XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                    PaddingTop.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingTop);

                    XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                    PaddingBottom.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingBottom);
                }
            }




            XmlElement TablixRow2 = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow2);

            XmlElement Row_Height2 = xml.CreateElement("Height");
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            char Tmp_Double_Codes = '"';
            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add_Col == "Y")
                //if (Entity.Can_Add_Col == "Y" && Entity.Column_Name != "SNP_APP")    // Rao 11182015 for Group Testing
                {
                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);


                    XmlElement Textbox = xml.CreateElement("Textbox");
                    // Dynamic Column Heading
                    if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
                        Textbox.SetAttribute("Name", Entity.Column_Name);
                    //Textbox.SetAttribute("Name", Entity.Column_Name + "_DESC");  
                    else
                        Textbox.SetAttribute("Name", Entity.Column_Name);
                    CellContents.AppendChild(Textbox);


                    XmlElement CanGrow = xml.CreateElement("CanGrow");
                    CanGrow.InnerText = "true";
                    Textbox.AppendChild(CanGrow);

                    XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                    KeepTogether.InnerText = "true";
                    Textbox.AppendChild(KeepTogether);



                    XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                    Textbox.AppendChild(Paragraphs);

                    XmlElement Paragraph = xml.CreateElement("Paragraph");
                    Paragraphs.AppendChild(Paragraph);

                    XmlElement TextRuns = xml.CreateElement("TextRuns");
                    Paragraph.AppendChild(TextRuns);

                    XmlElement TextRun = xml.CreateElement("TextRun");
                    TextRuns.AppendChild(TextRun);

                    XmlElement Return_Value = xml.CreateElement("Value");


                    Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    switch (Entity.Col_Format_Type)  // (Entity.Column_Disp_Name)
                    {
                        case "P":                           // Telephone
                            Text_Align = "Right"; break;
                        case "%":                           // "%"
                            Field_Value = "=Fields!" + Entity.Column_Name + ".Value  & " + Tmp_Double_Codes + "%" + Tmp_Double_Codes;
                            Text_Align = "Right";
                            break;
                        //case "S":                           // SSN#
                        //                                    //Format_Style_String = "000-00-0000"; break;
                        //                                    //Field_Value = "=Fields!" + Entity.Column_Name + ".Value ,  " + Tmp_Double_Codes + "000-00-0000" + Tmp_Double_Codes;
                        //                                    //Text_Align = "Right";
                        //                                    //break;
                        //    Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "000-00-0000" + Tmp_Double_Codes + ")";
                        //    Text_Align = "Right";
                        //    break;
                        case "Z":                           // ZipCode
                            //Format_Style_String = "00000-0000"; 
                            Text_Align = "Right";
                            break;
                        case "E":                           // Telephone
                            Format_Style_String = "(000)-000-0000"; break;
                        case "T":                           // Telephone
                            //Field_Value = "=FormatDateTime(Fields!" + Entity.Column_Name + ".Value, DateFormat.ShortDate)";
                            Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";
                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, "+ Tmp_Double_Codes+ "MM-dd-yyyy" +Tmp_Double_Codes +")";

                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, d)";
                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value).ToString(" + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";
                            //Format_Style_String = "d"; break;
                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, "+ Tmp_Double_Codes + "d" + Tmp_Double_Codes +")";
                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, "+ Tmp_Double_Codes + "MMM-dd-yyyy" + Tmp_Double_Codes +")";   // jan-01-2014
                            //Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "dd-MMM-yyyy" + Tmp_Double_Codes + ")";

                            Text_Align = "Right";
                            break;
                            //case "D":                           // Hierarchy Description
                            //    //Field_Value = "=FormatDateTime(Fields!" + Entity.Column_Name + ".Value, DateFormat.ShortDate)";
                            //    if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
                            //        Field_Value = "=Fields!" + Entity.Column_Name + "_DESC.Value";
                            //break;
                            //case "Z":                           // Hierarchy Description
                            //        Format_Style_String = "LL,LL,LL"; break;
                            //case "F":                           // Money
                            //    Field_Value = "= " + Tmp_Double_Codes + "$ " + Tmp_Double_Codes+ "& Fields!" + Entity.Column_Name + ".Value";
                            //    Text_Align = "Right";
                            //    break;

                    }

                    //if(Entity.Col_Format_Type == "T")
                    //    Return_Value.InnerText = "=CDate(Fields!" + Entity.Column_Name + ".Value)";        // Dynamic Column Heading
                    //else
                    //    Return_Value.InnerText = "=Fields!" + Entity.Column_Name + ".Value";        // Dynamic Column Heading

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    if (!string.IsNullOrEmpty(Format_Style_String))
                    {
                        XmlElement Return_Style = xml.CreateElement("Style");
                        TextRun.AppendChild(Return_Style);

                        XmlElement Return_Style_Format = xml.CreateElement("Format");

                        Return_Style_Format.InnerText = Format_Style_String;
                        Return_Style.AppendChild(Return_Style_Format);

                        //XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                        //Return_Style_FontWeight.InnerText = "Bold";
                        //Return_Style.AppendChild(Return_Style_FontWeight);
                    }


                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "None";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                    PaddingLeft.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingLeft);

                    XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                    PaddingRight.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingRight);

                    XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                    PaddingTop.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingTop);

                    XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                    PaddingBottom.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingBottom);
                }
            }



            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add_Col == "Y")
                //if (Entity.Can_Add_Col == "Y" && Entity.Column_Name != "SNP_APP")   //Rao 11182015 testing for Groups
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }


            XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
            Tablix.AppendChild(TablixRowHierarchy);

            XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
            TablixRowHierarchy.AppendChild(Tablix_Row_Members);

            XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member);


            XmlElement FixedData = xml.CreateElement("FixedData");
            FixedData.InnerText = "true";
            Tablix_Row_Member.AppendChild(FixedData);

            XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
            KeepWithGroup.InnerText = "After";
            Tablix_Row_Member.AppendChild(KeepWithGroup);

            XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
            RepeatOnNewPage.InnerText = "true";
            Tablix_Row_Member.AppendChild(RepeatOnNewPage);

            XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

            XmlElement Group = xml.CreateElement("Group"); // 5656565656
            Group.SetAttribute("Name", "Details1");
            Tablix_Row_Member1.AppendChild(Group);



            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            Tablix.AppendChild(SubReport_PageBreak);

            XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            SubReport_PageBreak_Location.InnerText = "End";
            SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            //XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            //Tablix.AppendChild(SortExpressions);

            //XmlElement SortExpression = xml.CreateElement("SortExpression");
            //SortExpressions.AppendChild(SortExpression);

            ////XmlElement SortExpression_Value = xml.CreateElement("Value");
            ////SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            ////SortExpression.AppendChild(SortExpression_Value);

            //XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            //SortExpression_Direction.InnerText = "Descending";
            //SortExpression.AppendChild(SortExpression_Direction);


            //XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            //SortExpressions.AppendChild(SortExpression1);

            //XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            ////SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            //SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            //SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            //Top.InnerText = (Total_Sel_TextBox_Height+1).ToString()+"in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            Top.InnerText = (Total_Sel_TextBox_Height + 0.29).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20417in";
            //Left.InnerText = "0.60417in";
            Tablix.AppendChild(Left);

            XmlElement Height1 = xml.CreateElement("Height");
            Height1.InnerText = "0.5in";
            Tablix.AppendChild(Height1);

            XmlElement Width1 = xml.CreateElement("Width");
            Width1.InnerText = "5.3229in";
            Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);


            if (Summary_Sw)
            {
                // Summary Sub Report 
                XmlElement Subreport = xml.CreateElement("Subreport");
                Subreport.SetAttribute("Name", "SummaryReport");  //99999999999999999
                ReportItems.AppendChild(Subreport);

                XmlElement SubRep_Name = xml.CreateElement("ReportName");
                SubRep_Name.InnerText = Pub_SubRep_Name;
                Subreport.AppendChild(SubRep_Name);

                XmlElement SubRep_Omit_Border = xml.CreateElement("OmitBorderOnPageBreak");
                SubRep_Omit_Border.InnerText = "true";
                Subreport.AppendChild(SubRep_Omit_Border);

                XmlElement SubRep_Top = xml.CreateElement("Top");
                SubRep_Top.InnerText = (Total_Sel_TextBox_Height + 1.5).ToString() + "in";
                //SubRep_Top.InnerText = "14.0in";
                Subreport.AppendChild(SubRep_Top);

                XmlElement SubRep_Left = xml.CreateElement("Left");
                SubRep_Left.InnerText = "0.009in";
                Subreport.AppendChild(SubRep_Left);

                XmlElement SubRep_Height = xml.CreateElement("Height");
                SubRep_Height.InnerText = ".5in";
                Subreport.AppendChild(SubRep_Height);

                XmlElement SubRep_Width = xml.CreateElement("Width");
                SubRep_Width.InnerText = "6.15624in";
                Subreport.AppendChild(SubRep_Width);

                XmlElement SubRep_ZIndex = xml.CreateElement("ZIndex");
                SubRep_ZIndex.InnerText = "2";
                Subreport.AppendChild(SubRep_ZIndex);

                XmlElement SubRep_Style = xml.CreateElement("Style");
                Subreport.AppendChild(SubRep_Style);

                XmlElement SubRep_Style_Border = xml.CreateElement("Border");
                SubRep_Style.AppendChild(SubRep_Style_Border);

                XmlElement SubRep_Style_Border_Style = xml.CreateElement("Style");
                SubRep_Style_Border_Style.InnerText = "None";
                SubRep_Style_Border.AppendChild(SubRep_Style_Border_Style);
            }


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

            XmlElement Width = xml.CreateElement("Width");               // Total Page Width
            Width.InnerText = "6.5in";      //Common
            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            if (Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                //PageHeader_Height.InnerText = "0.40558in";
                PageHeader_Height.InnerText = "0.39in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (Include_Header_Title)
                {
                    XmlElement Header_TextBox = xml.CreateElement("Textbox");
                    Header_TextBox.SetAttribute("Name", "HeaderTextBox");
                    Header_ReportItems.AppendChild(Header_TextBox);

                    XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
                    HeaderTextBox_CanGrow.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

                    XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
                    HeaderTextBox_Keep.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_Keep);

                    XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
                    Header_TextBox.AppendChild(Header_Paragraphs);

                    XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
                    Header_Paragraphs.AppendChild(Header_Paragraph);

                    XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
                    Header_Paragraph.AppendChild(Header_TextRuns);

                    XmlElement Header_TextRun = xml.CreateElement("TextRun");
                    Header_TextRuns.AppendChild(Header_TextRun);

                    //if (!string.IsNullOrEmpty(Rep_Header_Title.Trim()))
                    //{
                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);
                    //}


                    XmlElement Header_TextRun_Style = xml.CreateElement("Style");
                    Header_TextRun.AppendChild(Header_TextRun_Style);

                    XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
                    Header_Style_Font.InnerText = "Times New Roman";
                    Header_TextRun_Style.AppendChild(Header_Style_Font);

                    XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
                    Header_Style_FontSize.InnerText = "16pt";
                    Header_TextRun_Style.AppendChild(Header_Style_FontSize);

                    XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
                    Header_Style_TextDecoration.InnerText = "Underline";
                    Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

                    XmlElement Header_Style_Color = xml.CreateElement("Color");
                    Header_Style_Color.InnerText = "#104cda";
                    Header_TextRun_Style.AppendChild(Header_Style_Color);

                    XmlElement Header_TextBox_Top = xml.CreateElement("Top");
                    //Header_TextBox_Top.InnerText = "0.24792in";
                    //Header_TextBox_Top.InnerText = "0.14792in";
                    Header_TextBox_Top.InnerText = "0.07792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");   // 
                    Header_TextBox_Left.InnerText = (Rb_A4_Port.Checked ? "1.42361in" : "2.42361in");
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    //Header_TextBox_Height.InnerText = "0.30208in";
                    Header_TextBox_Height.InnerText = "0.3in";
                    Header_TextBox.AppendChild(Header_TextBox_Height);

                    XmlElement Header_TextBox_Width = xml.CreateElement("Width");
                    Header_TextBox_Width.InnerText = "5.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Width);

                    XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
                    Header_TextBox_ZIndex.InnerText = "1";
                    Header_TextBox.AppendChild(Header_TextBox_ZIndex);


                    XmlElement Header_TextBox_Style = xml.CreateElement("Style");
                    Header_TextBox.AppendChild(Header_TextBox_Style);

                    XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
                    Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

                    XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Header_TB_StyleBorderStyle.InnerText = "None";
                    Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

                    XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Header_TB_SBS_LeftPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

                    XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Header_TB_SBS_RightPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

                    XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Header_TB_SBS_TopPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

                    XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Header_TB_SBS_BotPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

                    XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
                    Header_Paragraph.AppendChild(Header_Text_Align_Style);

                    XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    Header_Text_Align.InnerText = "Center";
                    Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }

                //if (Include_Header_Image)
                //{
                //    // Add Image Heare
                //}

                XmlElement PageHeader_Style = xml.CreateElement("Style");
                PageHeader.AppendChild(PageHeader_Style);

                XmlElement PageHeader_Border = xml.CreateElement("Border");
                PageHeader_Style.AppendChild(PageHeader_Border);

                XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
                PageHeader_Border_Style.InnerText = "None";
                PageHeader_Border.AppendChild(PageHeader_Border_Style);


                XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
                PageHeader_BackgroundColor.InnerText = "White";
                PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            }


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            if (Include_Footer)
            {
                XmlElement PageFooter = xml.CreateElement("PageFooter");
                Page.AppendChild(PageFooter);

                XmlElement PageFooter_Height = xml.CreateElement("Height");
                PageFooter_Height.InnerText = "0.35083in";
                PageFooter.AppendChild(PageFooter_Height);

                XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                Footer_PrintOnFirstPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnFirstPage);

                XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                Footer_PrintOnLastPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnLastPage);

                XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
                PageFooter.AppendChild(Footer_ReportItems);

                if (Include_Footer_PageCnt)
                {
                    XmlElement Footer_TextBox = xml.CreateElement("Textbox");
                    Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
                    Footer_ReportItems.AppendChild(Footer_TextBox);

                    XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
                    FooterTextBox_CanGrow.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

                    XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
                    FooterTextBox_Keep.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_Keep);

                    XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
                    Footer_TextBox.AppendChild(Footer_Paragraphs);

                    XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
                    Footer_Paragraphs.AppendChild(Footer_Paragraph);

                    XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
                    Footer_Paragraph.AppendChild(Footer_TextRuns);

                    XmlElement Footer_TextRun = xml.CreateElement("TextRun");
                    Footer_TextRuns.AppendChild(Footer_TextRun);

                    XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
                    //Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    //Footer_TextRun_Value.InnerText = "=Globals!PageNumber &amp; " + Tmp_Double_Codes + '/' + Tmp_Double_Codes + "&amp; Globals!TotalPages";   // Dynamic Report Name
                    Footer_TextRun_Value.InnerText = "=Globals!PageNumber";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Left = xml.CreateElement("Left");
                    Footer_TextBox_Left.InnerText = (total_Columns_Width > 7.48777 ? "9.5in" : "7.3in");    // Rao
                    Footer_TextBox.AppendChild(Footer_TextBox_Left);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = ".3in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Width);


                    XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
                    Footer_TextBox.AppendChild(Footer_TextBox_Style);

                    XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
                    Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

                    XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Footer_TB_StyleBorderStyle.InnerText = "None";
                    Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

                    XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Footer_TB_SBS_LeftPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

                    XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Footer_TB_SBS_RightPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

                    XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Footer_TB_SBS_TopPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

                    XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Footer_TB_SBS_BotPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

                    XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
                    Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

                    //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    //Header_Text_Align.InnerText = "Center";
                    //Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }
            }


            //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


            XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
            XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

            //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
            //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            if (Rb_A4_Port.Checked)
            {
                Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
                Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
            }
            else
            {
                Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
                Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            }


            Page.AppendChild(Page_PageHeight);
            Page.AppendChild(Page_PageWidth);


            XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
            Page_LeftMargin.InnerText = "0.2in";
            Page.AppendChild(Page_LeftMargin);

            XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
            Page_RightMargin.InnerText = "0.2in";
            Page.AppendChild(Page_RightMargin);

            XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
            Page_TopMargin.InnerText = "0.2in";
            Page.AppendChild(Page_TopMargin);

            XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
            Page_BottomMargin.InnerText = "0.2in";
            Page.AppendChild(Page_BottomMargin);



            //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

            //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
            //EmbeddedImages.InnerText = "Image Attributes";
            //Report.AppendChild(EmbeddedImages);

            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


            string s = xml.OuterXml;

            try
            {


                Delete_Existing_Main_Rdlc();
                //System.IO.FileStream fs = new FileStream(ReportPath + Rep_Name, FileMode.Create);
                //xml.Save(@"C:\Capreports\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                //xml = null;

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                //xml.Save(@"C:\Capreports\MSTSNP.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //string strFolderPath = Consts.Common.ReportFolderLocation + "\\MSTSNP.rdlc";    // Run at Local System
                //xml.Save(strFolderPath); //I've chosen the c:\ for the resulting file pavel.xml
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // GC.ReRegisterForFinalize(xml);
            GC.SuppressFinalize(xml);
            //  Console.ReadLine();
        }




        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       Dynamic RDLC    >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>






        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       Dynamic Summary RDLC           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        private void Generete_Dynamic_Summary_RDLC()
        {
            XmlNode xmlnode;

            XmlDocument xml = new XmlDocument();
            xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xml.AppendChild(xmlnode);

            XmlElement Report = xml.CreateElement("Report");
            Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            xml.AppendChild(Report);

            XmlElement DataSources = xml.CreateElement("DataSources");
            XmlElement DataSource = xml.CreateElement("DataSource");
            DataSource.SetAttribute("Name", "CaptainDataSource");
            DataSources.AppendChild(DataSource);

            Report.AppendChild(DataSources);

            XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
            DataSource.AppendChild(ConnectionProperties);

            XmlElement DataProvider = xml.CreateElement("DataProvider");
            DataProvider.InnerText = "System.Data.DataSet";


            XmlElement ConnectString = xml.CreateElement("ConnectString");
            ConnectString.InnerText = "/* Local Connection */";
            ConnectionProperties.AppendChild(DataProvider);
            ConnectionProperties.AppendChild(ConnectString);

            //string SourceID = "rd:DataSourceID";
            //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
            //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
            //DataSource.AppendChild(DataSourceID);

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

            XmlElement DataSets = xml.CreateElement("DataSets");
            Report.AppendChild(DataSets);

            XmlElement DataSet = xml.CreateElement("DataSet");
            DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
            DataSets.AppendChild(DataSet);

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

            XmlElement Fields = xml.CreateElement("Fields");
            DataSet.AppendChild(Fields);


            string Tmp_Column_Name = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                XmlElement Field = xml.CreateElement("Field");


                XmlElement DataField = xml.CreateElement("DataField");
                switch (i)
                {
                    case 0: Tmp_Column_Name = "RO_Child_Desc1"; break;
                    case 1: Tmp_Column_Name = "RO_Child_Count1"; break;
                    case 2: Tmp_Column_Name = "RO_Spacing_Col1"; break;
                    case 3: Tmp_Column_Name = "RO_Child_Desc2"; break;
                    case 4: Tmp_Column_Name = "RO_Child_Count2"; break;
                    case 5: Tmp_Column_Name = "RO_IS_Header"; break;
                }
                Field.SetAttribute("Name", Tmp_Column_Name);
                Fields.AppendChild(Field);

                DataField.InnerText = Tmp_Column_Name;

                Field.AppendChild(DataField);
            }


            //foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
            //{
            //    if (Entity.Can_Add_Col == "Y")   // 09062012
            //    {
            //        XmlElement Field = xml.CreateElement("Field");

            //        Field.SetAttribute("Name", Entity.Column_Name);
            //        if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
            //            Field.SetAttribute("Name", Entity.Column_Name);
            //        //Field.SetAttribute("Name", Entity.Column_Name + "_DESC");

            //        Fields.AppendChild(Field);

            //        XmlElement DataField = xml.CreateElement("DataField");
            //        DataField.InnerText = Entity.Column_Name;
            //        if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
            //            DataField.InnerText = Entity.Column_Name;
            //        //DataField.InnerText = Entity.Column_Name + "_DESC";

            //        Field.AppendChild(DataField);

            //        //XmlElement DataField_RD = xml.CreateElement("rd");
            //        //DataField_RD.InnerText = "System." + Entity.Disp_Data_Type;
            //        //Field.AppendChild(DataField_RD);
            //    }
            //}

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

            XmlElement Query = xml.CreateElement("Query");
            DataSet.AppendChild(Query);

            XmlElement DataSourceName = xml.CreateElement("DataSourceName");
            DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
            Query.AppendChild(DataSourceName);

            XmlElement CommandText = xml.CreateElement("CommandText");
            CommandText.InnerText = "/* Local Query */";
            Query.AppendChild(CommandText);


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
            //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            XmlElement Body = xml.CreateElement("Body");
            Report.AppendChild(Body);


            XmlElement ReportItems = xml.CreateElement("ReportItems");
            Body.AppendChild(ReportItems);

            XmlElement Height = xml.CreateElement("Height");
            //Height.InnerText = "4.15625in";       // Landscape
            Height.InnerText = "2in";           // Portrait
            Body.AppendChild(Height);


            XmlElement Style = xml.CreateElement("Style");
            Body.AppendChild(Style);

            XmlElement Border = xml.CreateElement("Border");
            Style.AppendChild(Border);

            XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
            BackgroundColor.InnerText = "White";
            Style.AppendChild(BackgroundColor);


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
            Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
            ReportItems.AppendChild(Sel_Rectangle);

            XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
            Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


            double Total_Sel_TextBox_Height = 0.08;
            string Tmp_Sel_Text = string.Empty;
            for (int i = 0; i < 1; i++)
            {
                XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
                Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
                Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

                XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
                Textbox1_Cangrow.InnerText = "true";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

                XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
                Textbox1_Keep.InnerText = "true";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

                XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

                XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
                Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

                XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
                Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


                XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
                Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

                XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

                Tmp_Sel_Text = string.Empty;
                switch (i)
                {
                    case 0: Tmp_Sel_Text = "Report Summary"; break;
                    default: Tmp_Sel_Text = "  "; break;
                }


                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);

                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

                XmlElement Textbox1_TextRun_Style_Font = xml.CreateElement("FontWeight");
                Textbox1_TextRun_Style_Font.InnerText = "Bold";
                Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Font);


                XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);

                XmlElement Textbox1_Paragraph_Style_Txt_Align = xml.CreateElement("TextAlign");
                Textbox1_Paragraph_Style_Txt_Align.InnerText = "Center";
                Textbox1_Paragraph_Style.AppendChild(Textbox1_Paragraph_Style_Txt_Align);



                XmlElement Textbox1_Top = xml.CreateElement("Top");
                Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                Textbox1_Left.InnerText = "0.07292in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                Total_Sel_TextBox_Height += 0.21875;

                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? (11 > total_Columns_Width ? "10.5in" : total_Columns_Width.ToString() + "in") : "7.48777in"); // "6.35055in";
                Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "10in" : "7.48777in"); // "6.35055in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Style = xml.CreateElement("Style");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

                XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
                Textbox1_Style.AppendChild(Textbox1_Style_Border);

                XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
                Textbox1_Style_Border_Style.InnerText = "None";
                Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

                XmlElement Textbox1_Style_BackColor = xml.CreateElement("BackgroundColor");
                Textbox1_Style_BackColor.InnerText = "LightGrey";
                Textbox1_Style.AppendChild(Textbox1_Style_BackColor);

                XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
                Textbox1_Style_PaddingLeft.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

                XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
                Textbox1_Style_PaddingRight.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

                XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
                Textbox1_Style_PaddingTop.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

                XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
                Textbox1_Style_PaddingTop.InnerText = "2pt";
                Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

            }


            XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
            Sel_Rectangle_KeepTogether.InnerText = "true";
            Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

            XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
            Sel_Rectangle_Top.InnerText = "0.2408in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.48777 ? "10in" : "7.48777in"); // "6.35055in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

            XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
            Sel_Rectangle_ZIndex.InnerText = "1";
            Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

            XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
            Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

            XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
            Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

            XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
            Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
            Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

            XmlElement Sel_Rectangle_Style_BackColor = xml.CreateElement("BackgroundColor");
            Sel_Rectangle_Style_BackColor.InnerText = "LightGrey";//"None";
            Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_BackColor);


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            string Tmp_Width = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                TablixColumns.AppendChild(TablixColumn);

                XmlElement Col_Width = xml.CreateElement("Width");

                XmlElement DataField = xml.CreateElement("DataField");
                Tmp_Width = "1in";
                switch (i)
                {
                    case 0:
                    case 3: Tmp_Width = "2.6in"; break; //Tmp_Width = "2.8in"; 
                    case 2: Tmp_Width = "0.27in"; break;
                    case 1:
                    case 4: Tmp_Width = "1in"; break; //"0.8in";
                }

                Col_Width.InnerText = Tmp_Width;        // Dynamic based on Display Columns Width
                TablixColumn.AppendChild(Col_Width);
            }


            //10162012 Rao 
            //foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)                    // Dynamic based on Display Columns in Result Table
            //{
            //    if (Entity.Can_Add_Col == "Y")   // 09062012
            //    {
            //        XmlElement TablixColumn = xml.CreateElement("TablixColumn");
            //        TablixColumns.AppendChild(TablixColumn);

            //        XmlElement Col_Width = xml.CreateElement("Width");

            //        Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width

            //        //Col_Width.InnerText = "1in";        // Dynamic based on Display Columns Width
            //        TablixColumn.AppendChild(Col_Width);
            //        //total_Columns_Width = total_Columns_Width + double.Parse(Entity.Max_Display_Width);
            //    }
            //}


            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";


            //string Tmp_Width = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                //*********************************************************
                Tmp_Loop_Cnt++;

                XmlElement TablixCell = xml.CreateElement("TablixCell");
                Row_TablixCells.AppendChild(TablixCell);


                XmlElement CellContents = xml.CreateElement("CellContents");
                TablixCell.AppendChild(CellContents);

                //if (Entity.Col_Format_Type == "C")
                //    Field_type = "Checkbox";

                XmlElement Textbox = xml.CreateElement(Field_type);
                Textbox.SetAttribute("Name", "Textbox" + i.ToString());
                CellContents.AppendChild(Textbox);

                XmlElement CanGrow = xml.CreateElement("CanGrow");
                CanGrow.InnerText = "true";
                Textbox.AppendChild(CanGrow);

                XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                KeepTogether.InnerText = "true";
                Textbox.AppendChild(KeepTogether);



                XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                Textbox.AppendChild(Paragraphs);

                XmlElement Paragraph = xml.CreateElement("Paragraph");
                Paragraphs.AppendChild(Paragraph);



                XmlElement TextRuns = xml.CreateElement("TextRuns");
                Paragraph.AppendChild(TextRuns);

                XmlElement TextRun = xml.CreateElement("TextRun");
                TextRuns.AppendChild(TextRun);

                XmlElement Return_Value = xml.CreateElement("Value");


                //switch (i)
                //{
                //    case 0:
                //    case 2: Tmp_Width = "10in"; break;
                //    case 1:
                //    case 3: Tmp_Width = "2in"; break;
                //}



                Tmp_Disp_Column_Name = " ";

                //switch (i)
                //{
                //    case 0:
                //    case 2: Tmp_Width = "10in"; break;
                //    case 1:
                //    case 3: Tmp_Width = "2in"; break;
                //}                

                //switch (Entity.Col_Format_Type)
                //{
                //    case "F": Tmp_Disp_Column_Name += "($)";
                //        //Entity.Disp_Length = "10";
                //        Entity.Disp_Desc_Length = "10";
                //        break;
                //}


                //Disp_Col_Substring_Len = Tmp_Disp_Column_Name.Length;
                ////if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Length))
                ////    Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Length));

                ////if (Entity.Description != "Y" && (!string.IsNullOrEmpty(Entity.AgyCode.Trim())))
                ////{
                ////    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
                ////        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
                ////        //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
                ////}
                ////else
                ////{
                ////    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Desc_Length))
                ////        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));

                ////}

                //if (!string.IsNullOrEmpty(Entity.AgyCode.Trim()))
                //{
                //    if (Entity.Description != "Y")
                //    {
                //        if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
                //            Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
                //        //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
                //    }
                //    else
                //        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));
                //}
                //else
                //    Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));


                //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                Return_Value.InnerText = "  ";                           //10172012        // Header Text is Not Required
                TextRun.AppendChild(Return_Value);


                XmlElement Cell_Align = xml.CreateElement("Style");
                XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                Cell_TextAlign.InnerText = "Center";
                Cell_Align.AppendChild(Cell_TextAlign);
                Paragraph.AppendChild(Cell_Align);


                XmlElement Return_Style = xml.CreateElement("Style");
                TextRun.AppendChild(Return_Style);

                XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                Return_Style_FontWeight.InnerText = "Bold";
                Return_Style.AppendChild(Return_Style_FontWeight);


                //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                //Paragraph.AppendChild(Return_AlignStyle);

                //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                //DefaultName.InnerText = "Textbox" + i.ToString();
                //Textbox.AppendChild(DefaultName);


                XmlElement Cell_style = xml.CreateElement("Style");
                Textbox.AppendChild(Cell_style);


                XmlElement Cell_Border = xml.CreateElement("Border");
                Cell_style.AppendChild(Cell_Border);

                XmlElement Border_Color = xml.CreateElement("Color");
                Border_Color.InnerText = "LightGrey";
                Cell_Border.AppendChild(Border_Color);

                XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                Border_Style.InnerText = "None";       //10172012        // Header Border is Not Required
                Cell_Border.AppendChild(Border_Style);



                XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                PaddingLeft.InnerText = "2pt";
                Cell_style.AppendChild(PaddingLeft);

                XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                PaddingRight.InnerText = "2pt";
                Cell_style.AppendChild(PaddingRight);

                XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                PaddingTop.InnerText = "2pt";
                Cell_style.AppendChild(PaddingTop);

                XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                PaddingBottom.InnerText = "2pt";
                Cell_style.AppendChild(PaddingBottom);



            }


            //////foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)            // Dynamic based on Display Columns in Result Table
            //////{
            //////    if (Entity.Can_Add_Col == "Y")   // 09062012
            //////    {
            //////        //Entity.Column_Name;
            //////        Tmp_Loop_Cnt++;

            //////        XmlElement TablixCell = xml.CreateElement("TablixCell");
            //////        Row_TablixCells.AppendChild(TablixCell);


            //////        XmlElement CellContents = xml.CreateElement("CellContents");
            //////        TablixCell.AppendChild(CellContents);

            //////        //if (Entity.Col_Format_Type == "C")
            //////        //    Field_type = "Checkbox";

            //////        XmlElement Textbox = xml.CreateElement(Field_type);
            //////        Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
            //////        CellContents.AppendChild(Textbox);

            //////        XmlElement CanGrow = xml.CreateElement("CanGrow");
            //////        CanGrow.InnerText = "true";
            //////        Textbox.AppendChild(CanGrow);

            //////        XmlElement KeepTogether = xml.CreateElement("KeepTogether");
            //////        KeepTogether.InnerText = "true";
            //////        Textbox.AppendChild(KeepTogether);



            //////        XmlElement Paragraphs = xml.CreateElement("Paragraphs");
            //////        Textbox.AppendChild(Paragraphs);

            //////        XmlElement Paragraph = xml.CreateElement("Paragraph");
            //////        Paragraphs.AppendChild(Paragraph);



            //////        XmlElement TextRuns = xml.CreateElement("TextRuns");
            //////        Paragraph.AppendChild(TextRuns);

            //////        XmlElement TextRun = xml.CreateElement("TextRun");
            //////        TextRuns.AppendChild(TextRun);

            //////        XmlElement Return_Value = xml.CreateElement("Value");

            //////        Tmp_Disp_Column_Name = Entity.Column_Disp_Name;

            //////        switch (Entity.Col_Format_Type)
            //////        {
            //////            case "F": Tmp_Disp_Column_Name += "($)";
            //////                //Entity.Disp_Length = "10";
            //////                Entity.Disp_Desc_Length = "10";
            //////                break;
            //////        }


            //////        Disp_Col_Substring_Len = Tmp_Disp_Column_Name.Length;
            //////        //if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Length))
            //////        //    Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Length));

            //////        //if (Entity.Description != "Y" && (!string.IsNullOrEmpty(Entity.AgyCode.Trim())))
            //////        //{
            //////        //    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
            //////        //        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
            //////        //        //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
            //////        //}
            //////        //else
            //////        //{
            //////        //    if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Desc_Length))
            //////        //        Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));

            //////        //}

            //////        if (!string.IsNullOrEmpty(Entity.AgyCode.Trim()))
            //////        {
            //////            if (Entity.Description != "Y")
            //////            {
            //////                if (Disp_Col_Substring_Len >= int.Parse(Entity.Disp_Code_Length))
            //////                    Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length));
            //////                //Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Code_Length)) != 0 ? (int.Parse(Entity.Disp_Code_Length)) : (int.Parse(Entity.Disp_Desc_Length));
            //////            }
            //////            else
            //////                Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));
            //////        }
            //////        else
            //////            Disp_Col_Substring_Len = (int.Parse(Entity.Disp_Desc_Length));


            //////        Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
            //////        //Return_Value.InnerText = Entity.Column_Name;                                    // Dynamic Column Heading
            //////        TextRun.AppendChild(Return_Value);


            //////        XmlElement Cell_Align = xml.CreateElement("Style");
            //////        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
            //////        Cell_TextAlign.InnerText = "Center";
            //////        Cell_Align.AppendChild(Cell_TextAlign);
            //////        Paragraph.AppendChild(Cell_Align);


            //////        XmlElement Return_Style = xml.CreateElement("Style");
            //////        TextRun.AppendChild(Return_Style);

            //////        XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
            //////        Return_Style_FontWeight.InnerText = "Bold";
            //////        Return_Style.AppendChild(Return_Style_FontWeight);


            //////        //XmlElement Return_AlignStyle = xml.CreateElement("Style");
            //////        //Paragraph.AppendChild(Return_AlignStyle);

            //////        //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
            //////        //DefaultName.InnerText = "Textbox" + i.ToString();
            //////        //Textbox.AppendChild(DefaultName);


            //////        XmlElement Cell_style = xml.CreateElement("Style");
            //////        Textbox.AppendChild(Cell_style);


            //////        XmlElement Cell_Border = xml.CreateElement("Border");
            //////        Cell_style.AppendChild(Cell_Border);

            //////        XmlElement Border_Color = xml.CreateElement("Color");
            //////        Border_Color.InnerText = "LightGrey";
            //////        Cell_Border.AppendChild(Border_Color);

            //////        XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
            //////        Border_Style.InnerText = "Solid";
            //////        Cell_Border.AppendChild(Border_Style);



            //////        XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
            //////        PaddingLeft.InnerText = "2pt";
            //////        Cell_style.AppendChild(PaddingLeft);

            //////        XmlElement PaddingRight = xml.CreateElement("PaddingRight");
            //////        PaddingRight.InnerText = "2pt";
            //////        Cell_style.AppendChild(PaddingRight);

            //////        XmlElement PaddingTop = xml.CreateElement("PaddingTop");
            //////        PaddingTop.InnerText = "2pt";
            //////        Cell_style.AppendChild(PaddingTop);

            //////        XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
            //////        PaddingBottom.InnerText = "2pt";
            //////        Cell_style.AppendChild(PaddingBottom);
            //////    }
            //////}




            XmlElement TablixRow2 = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow2);

            XmlElement Row_Height2 = xml.CreateElement("Height");
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty,
                   Temporary_Field_Value = string.Empty, Vertical_Align = string.Empty;
            char Tmp_Double_Codes = '"';
            string Column_Name = string.Empty, Col_Format_Type = string.Empty;

            for (int i = 0; i < 5; i++)        // Dynamic based on Display Columns in Result Table
            {
                XmlElement TablixCell = xml.CreateElement("TablixCell");
                Row_TablixCells2.AppendChild(TablixCell);

                XmlElement CellContents = xml.CreateElement("CellContents");
                TablixCell.AppendChild(CellContents);


                XmlElement Textbox = xml.CreateElement("Textbox");

                Col_Format_Type = "D";
                Vertical_Align = string.Empty;
                switch (i)
                {
                    case 0: Column_Name = "RO_Child_Desc1"; Vertical_Align = "T"; break;
                    case 1: Column_Name = "RO_Child_Count1"; Col_Format_Type = "R"; Vertical_Align = "T"; break;
                    case 2: Column_Name = "RO_Spacing_Col1"; break;
                    case 3: Column_Name = "RO_Child_Desc2"; break;
                    case 4: Column_Name = "RO_Child_Count2"; Col_Format_Type = "R"; break;
                }


                Textbox.SetAttribute("Name", Column_Name);
                CellContents.AppendChild(Textbox);


                XmlElement CanGrow = xml.CreateElement("CanGrow");
                CanGrow.InnerText = "true";
                Textbox.AppendChild(CanGrow);

                XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                KeepTogether.InnerText = "true";
                Textbox.AppendChild(KeepTogether);



                XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                Textbox.AppendChild(Paragraphs);

                XmlElement Paragraph = xml.CreateElement("Paragraph");
                Paragraphs.AppendChild(Paragraph);

                XmlElement TextRuns = xml.CreateElement("TextRuns");
                Paragraph.AppendChild(TextRuns);

                XmlElement TextRun = xml.CreateElement("TextRun");
                TextRuns.AppendChild(TextRun);

                XmlElement Return_Value = xml.CreateElement("Value");


                Field_Value = "=Fields!" + Column_Name + ".Value";
                Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                switch (Col_Format_Type)  // (Entity.Column_Disp_Name)
                {
                    case "R":                           // Telephone
                        Text_Align = "Right"; break;
                    case "T":                           // Telephone
                        Text_Align = "Top"; break;
                }

                //if(Entity.Col_Format_Type == "T")
                //    Return_Value.InnerText = "=CDate(Fields!" + Entity.Column_Name + ".Value)";        // Dynamic Column Heading
                //else
                //    Return_Value.InnerText = "=Fields!" + Entity.Column_Name + ".Value";        // Dynamic Column Heading

                Return_Value.InnerText = Field_Value;
                TextRun.AppendChild(Return_Value);

                XmlElement Return_Style = xml.CreateElement("Style");
                TextRun.AppendChild(Return_Style);


                if (Column_Name == "RO_Child_Desc1" || Column_Name == "RO_Child_Count1")
                {
                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "=IIf(Fields!RO_IS_Header.Value=" + Tmp_Double_Codes + "True" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    Return_Style.AppendChild(Return_Style_FontWeight);
                }


                if (!string.IsNullOrEmpty(Format_Style_String))
                {
                    // XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Return_Style_Format = xml.CreateElement("Format");

                    Return_Style_Format.InnerText = Format_Style_String;
                    Return_Style.AppendChild(Return_Style_Format);

                    //XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //Return_Style_FontWeight.InnerText = "Bold";
                    //Return_Style.AppendChild(Return_Style_FontWeight);
                }


                //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                //DefaultName.InnerText = "Textbox" + i.ToString();
                //Textbox.AppendChild(DefaultName);

                if (!string.IsNullOrEmpty(Text_Align))
                {
                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                    Cell_TextAlign.InnerText = Text_Align;
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);
                }


                XmlElement Cell_style = xml.CreateElement("Style");
                Textbox.AppendChild(Cell_style);

                XmlElement Cell_Border = xml.CreateElement("Border");
                Cell_style.AppendChild(Cell_Border);

                XmlElement Border_Color = xml.CreateElement("Color");
                Border_Color.InnerText = "LightGrey";
                Cell_Border.AppendChild(Border_Color);

                XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                Border_Style.InnerText = "None";
                Cell_Border.AppendChild(Border_Style);

                switch (Vertical_Align)
                {
                    case "T": Vertical_Align = "Top"; break;
                }

                if (!string.IsNullOrEmpty(Vertical_Align))
                {
                    XmlElement Border_Style_VerticalAlign = xml.CreateElement("VerticalAlign");    // Repeating Cell Border Style
                    Border_Style_VerticalAlign.InnerText = Vertical_Align;
                    Cell_style.AppendChild(Border_Style_VerticalAlign);
                }

                XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                PaddingLeft.InnerText = "2pt";
                Cell_style.AppendChild(PaddingLeft);

                XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                PaddingRight.InnerText = "2pt";
                Cell_style.AppendChild(PaddingRight);

                XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                PaddingTop.InnerText = "2pt";
                Cell_style.AppendChild(PaddingTop);

                XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                PaddingBottom.InnerText = "2pt";
                Cell_style.AppendChild(PaddingBottom);

            }

            //foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)        // Dynamic based on Display Columns in Result Table
            //{
            //    if (Entity.Can_Add_Col == "Y")   // 09062012
            //    {
            //        XmlElement TablixCell = xml.CreateElement("TablixCell");
            //        Row_TablixCells2.AppendChild(TablixCell);

            //        XmlElement CellContents = xml.CreateElement("CellContents");
            //        TablixCell.AppendChild(CellContents);


            //        XmlElement Textbox = xml.CreateElement("Textbox");
            //        // Dynamic Column Heading
            //        if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
            //            Textbox.SetAttribute("Name", Entity.Column_Name);
            //        //Textbox.SetAttribute("Name", Entity.Column_Name + "_DESC");  
            //        else
            //            Textbox.SetAttribute("Name", Entity.Column_Name);
            //        CellContents.AppendChild(Textbox);


            //        XmlElement CanGrow = xml.CreateElement("CanGrow");
            //        CanGrow.InnerText = "true";
            //        Textbox.AppendChild(CanGrow);

            //        XmlElement KeepTogether = xml.CreateElement("KeepTogether");
            //        KeepTogether.InnerText = "true";
            //        Textbox.AppendChild(KeepTogether);



            //        XmlElement Paragraphs = xml.CreateElement("Paragraphs");
            //        Textbox.AppendChild(Paragraphs);

            //        XmlElement Paragraph = xml.CreateElement("Paragraph");
            //        Paragraphs.AppendChild(Paragraph);

            //        XmlElement TextRuns = xml.CreateElement("TextRuns");
            //        Paragraph.AppendChild(TextRuns);

            //        XmlElement TextRun = xml.CreateElement("TextRun");
            //        TextRuns.AppendChild(TextRun);

            //        XmlElement Return_Value = xml.CreateElement("Value");


            //        Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
            //        Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
            //        switch (Entity.Col_Format_Type)  // (Entity.Column_Disp_Name)
            //        {
            //            case "P":                           // Telephone
            //                Text_Align = "Right"; break;
            //            case "%":                           // "%"
            //                Field_Value = "=Fields!" + Entity.Column_Name + ".Value  & " + Tmp_Double_Codes + "%" + Tmp_Double_Codes;
            //                Text_Align = "Right";
            //                break;
            //            case "S":                           // SSN#
            //                Format_Style_String = "000-00-0000"; break;
            //            case "Z":                           // ZipCode
            //                //Format_Style_String = "00000-0000"; 
            //                Text_Align = "Right";
            //                break;
            //            case "E":                           // Telephone
            //                Format_Style_String = "(000)-000-0000"; break;
            //            case "T":                           // Telephone
            //                //Field_Value = "=FormatDateTime(Fields!" + Entity.Column_Name + ".Value, DateFormat.ShortDate)";
            //                Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";
            //                Text_Align = "Right";
            //                break;
            //            //case "D":                           // Hierarchy Description
            //            //    //Field_Value = "=FormatDateTime(Fields!" + Entity.Column_Name + ".Value, DateFormat.ShortDate)";
            //            //    if ((Entity.AgyCode == "HIEAG" || Entity.AgyCode == "HIEDE" || Entity.AgyCode == "HIEPR") && Entity.Description == "Y")
            //            //        Field_Value = "=Fields!" + Entity.Column_Name + "_DESC.Value";
            //            //break;
            //            //case "Z":                           // Hierarchy Description
            //            //        Format_Style_String = "LL,LL,LL"; break;
            //            //case "F":                           // Money
            //            //    Field_Value = "= " + Tmp_Double_Codes + "$ " + Tmp_Double_Codes+ "& Fields!" + Entity.Column_Name + ".Value";
            //            //    Text_Align = "Right";
            //            //    break;

            //        }

            //        //if(Entity.Col_Format_Type == "T")
            //        //    Return_Value.InnerText = "=CDate(Fields!" + Entity.Column_Name + ".Value)";        // Dynamic Column Heading
            //        //else
            //        //    Return_Value.InnerText = "=Fields!" + Entity.Column_Name + ".Value";        // Dynamic Column Heading

            //        Return_Value.InnerText = Field_Value;
            //        TextRun.AppendChild(Return_Value);

            //        if (!string.IsNullOrEmpty(Format_Style_String))
            //        {
            //            XmlElement Return_Style = xml.CreateElement("Style");
            //            TextRun.AppendChild(Return_Style);

            //            XmlElement Return_Style_Format = xml.CreateElement("Format");

            //            Return_Style_Format.InnerText = Format_Style_String;
            //            Return_Style.AppendChild(Return_Style_Format);

            //            //XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
            //            //Return_Style_FontWeight.InnerText = "Bold";
            //            //Return_Style.AppendChild(Return_Style_FontWeight);
            //        }


            //        //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
            //        //DefaultName.InnerText = "Textbox" + i.ToString();
            //        //Textbox.AppendChild(DefaultName);

            //        if (!string.IsNullOrEmpty(Text_Align))
            //        {
            //            XmlElement Cell_Align = xml.CreateElement("Style");
            //            XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
            //            Cell_TextAlign.InnerText = Text_Align;
            //            Cell_Align.AppendChild(Cell_TextAlign);
            //            Paragraph.AppendChild(Cell_Align);
            //        }


            //        XmlElement Cell_style = xml.CreateElement("Style");
            //        Textbox.AppendChild(Cell_style);

            //        XmlElement Cell_Border = xml.CreateElement("Border");
            //        Cell_style.AppendChild(Cell_Border);

            //        XmlElement Border_Color = xml.CreateElement("Color");
            //        Border_Color.InnerText = "LightGrey";
            //        Cell_Border.AppendChild(Border_Color);

            //        XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
            //        Border_Style.InnerText = "None";
            //        Cell_Border.AppendChild(Border_Style);


            //        XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
            //        PaddingLeft.InnerText = "2pt";
            //        Cell_style.AppendChild(PaddingLeft);

            //        XmlElement PaddingRight = xml.CreateElement("PaddingRight");
            //        PaddingRight.InnerText = "2pt";
            //        Cell_style.AppendChild(PaddingRight);

            //        XmlElement PaddingTop = xml.CreateElement("PaddingTop");
            //        PaddingTop.InnerText = "2pt";
            //        Cell_style.AppendChild(PaddingTop);

            //        XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
            //        PaddingBottom.InnerText = "2pt";
            //        Cell_style.AppendChild(PaddingBottom);
            //    }
            //}



            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            for (int i = 0; i < 5; i++)            // Dynamic based on Display Columns in Result Table
            {
                XmlElement TablixMember = xml.CreateElement("TablixMember");
                Tablix_Col_Members.AppendChild(TablixMember);
            }

            //foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)            // Dynamic based on Display Columns in Result Table
            //{
            //    if (Entity.Can_Add_Col == "Y")   // 09062012
            //    {
            //        XmlElement TablixMember = xml.CreateElement("TablixMember");
            //        Tablix_Col_Members.AppendChild(TablixMember);
            //    }
            //}

            XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
            Tablix.AppendChild(TablixRowHierarchy);

            XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
            TablixRowHierarchy.AppendChild(Tablix_Row_Members);

            XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member);

            XmlElement FixedData = xml.CreateElement("FixedData");
            FixedData.InnerText = "true";
            Tablix_Row_Member.AppendChild(FixedData);

            XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
            KeepWithGroup.InnerText = "After";
            Tablix_Row_Member.AppendChild(KeepWithGroup);

            XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
            RepeatOnNewPage.InnerText = "true";
            Tablix_Row_Member.AppendChild(RepeatOnNewPage);

            XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

            XmlElement Group = xml.CreateElement("Group");
            Group.SetAttribute("Name", "Details1");
            Tablix_Row_Member1.AppendChild(Group);


            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            //SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            Top.InnerText = "0.25in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20417in";
            //Left.InnerText = "0.60417in";
            Tablix.AppendChild(Left);

            XmlElement Height1 = xml.CreateElement("Height");
            Height1.InnerText = "0.5in";
            Tablix.AppendChild(Height1);

            XmlElement Width1 = xml.CreateElement("Width");
            Width1.InnerText = "5.3229in";
            Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

            XmlElement Width = xml.CreateElement("Width");               // Total Page Width
            Width.InnerText = "6.5in";      //Common
            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            //if (Include_header)
            //{
            //    XmlElement PageHeader = xml.CreateElement("PageHeader");
            //    Page.AppendChild(PageHeader);

            //    XmlElement PageHeader_Height = xml.CreateElement("Height");
            //    PageHeader_Height.InnerText = "0.51958in";
            //    PageHeader.AppendChild(PageHeader_Height);

            //    XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
            //    PrintOnFirstPage.InnerText = "true";
            //    PageHeader.AppendChild(PrintOnFirstPage);

            //    XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
            //    PrintOnLastPage.InnerText = "true";
            //    PageHeader.AppendChild(PrintOnLastPage);


            //    XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
            //    PageHeader.AppendChild(Header_ReportItems);

            //    if (Include_Header_Title)
            //    {
            //        XmlElement Header_TextBox = xml.CreateElement("Textbox");
            //        Header_TextBox.SetAttribute("Name", "HeaderTextBox");
            //        Header_ReportItems.AppendChild(Header_TextBox);

            //        XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
            //        HeaderTextBox_CanGrow.InnerText = "true";
            //        Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

            //        XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
            //        HeaderTextBox_Keep.InnerText = "true";
            //        Header_TextBox.AppendChild(HeaderTextBox_Keep);

            //        XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
            //        Header_TextBox.AppendChild(Header_Paragraphs);

            //        XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
            //        Header_Paragraphs.AppendChild(Header_Paragraph);

            //        XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
            //        Header_Paragraph.AppendChild(Header_TextRuns);

            //        XmlElement Header_TextRun = xml.CreateElement("TextRun");
            //        Header_TextRuns.AppendChild(Header_TextRun);

            //        XmlElement Header_TextRun_Value = xml.CreateElement("Value");
            //        Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
            //        Header_TextRun.AppendChild(Header_TextRun_Value);

            //        XmlElement Header_TextRun_Style = xml.CreateElement("Style");
            //        Header_TextRun.AppendChild(Header_TextRun_Style);

            //        XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
            //        Header_Style_Font.InnerText = "Times New Roman";
            //        Header_TextRun_Style.AppendChild(Header_Style_Font);

            //        XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
            //        Header_Style_FontSize.InnerText = "16pt";
            //        Header_TextRun_Style.AppendChild(Header_Style_FontSize);

            //        XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
            //        Header_Style_TextDecoration.InnerText = "Underline";
            //        Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

            //        XmlElement Header_Style_Color = xml.CreateElement("Color");
            //        Header_Style_Color.InnerText = "#104cda";
            //        Header_TextRun_Style.AppendChild(Header_Style_Color);

            //        XmlElement Header_TextBox_Top = xml.CreateElement("Top");
            //        Header_TextBox_Top.InnerText = "0.24792in";
            //        Header_TextBox.AppendChild(Header_TextBox_Top);

            //        XmlElement Header_TextBox_Left = xml.CreateElement("Left");
            //        Header_TextBox_Left.InnerText = "1.42361in";
            //        Header_TextBox.AppendChild(Header_TextBox_Left);

            //        XmlElement Header_TextBox_Height = xml.CreateElement("Height");
            //        Header_TextBox_Height.InnerText = "0.30208in";
            //        Header_TextBox.AppendChild(Header_TextBox_Height);

            //        XmlElement Header_TextBox_Width = xml.CreateElement("Width");
            //        Header_TextBox_Width.InnerText = "5.30208in";
            //        Header_TextBox.AppendChild(Header_TextBox_Width);

            //        XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
            //        Header_TextBox_ZIndex.InnerText = "1";
            //        Header_TextBox.AppendChild(Header_TextBox_ZIndex);


            //        XmlElement Header_TextBox_Style = xml.CreateElement("Style");
            //        Header_TextBox.AppendChild(Header_TextBox_Style);

            //        XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
            //        Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

            //        XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
            //        Header_TB_StyleBorderStyle.InnerText = "None";
            //        Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

            //        XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
            //        Header_TB_SBS_LeftPad.InnerText = "2pt";
            //        Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

            //        XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
            //        Header_TB_SBS_RightPad.InnerText = "2pt";
            //        Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

            //        XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
            //        Header_TB_SBS_TopPad.InnerText = "2pt";
            //        Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

            //        XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
            //        Header_TB_SBS_BotPad.InnerText = "2pt";
            //        Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

            //        XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
            //        Header_Paragraph.AppendChild(Header_Text_Align_Style);

            //        XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
            //        Header_Text_Align.InnerText = "Center";
            //        Header_Text_Align_Style.AppendChild(Header_Text_Align);
            //    }

            //    //if (Include_Header_Image)
            //    //{
            //    //    // Add Image Heare
            //    //}

            //    XmlElement PageHeader_Style = xml.CreateElement("Style");
            //    PageHeader.AppendChild(PageHeader_Style);

            //    XmlElement PageHeader_Border = xml.CreateElement("Border");
            //    PageHeader_Style.AppendChild(PageHeader_Border);

            //    XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
            //    PageHeader_Border_Style.InnerText = "None";
            //    PageHeader_Border.AppendChild(PageHeader_Border_Style);


            //    XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
            //    PageHeader_BackgroundColor.InnerText = "White";
            //    PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            //}


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            ////if (Include_Footer)
            ////{
            ////    XmlElement PageFooter = xml.CreateElement("PageFooter");
            ////    Page.AppendChild(PageFooter);

            ////    XmlElement PageFooter_Height = xml.CreateElement("Height");
            ////    PageFooter_Height.InnerText = "0.35083in";
            ////    PageFooter.AppendChild(PageFooter_Height);

            ////    XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
            ////    Footer_PrintOnFirstPage.InnerText = "true";
            ////    PageFooter.AppendChild(Footer_PrintOnFirstPage);

            ////    XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
            ////    Footer_PrintOnLastPage.InnerText = "true";
            ////    PageFooter.AppendChild(Footer_PrintOnLastPage);

            ////    XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
            ////    PageFooter.AppendChild(Footer_ReportItems);

            ////    if (Include_Footer_PageCnt)
            ////    {
            ////        XmlElement Footer_TextBox = xml.CreateElement("Textbox");
            ////        Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
            ////        Footer_ReportItems.AppendChild(Footer_TextBox);

            ////        XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
            ////        FooterTextBox_CanGrow.InnerText = "true";
            ////        Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

            ////        XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
            ////        FooterTextBox_Keep.InnerText = "true";
            ////        Footer_TextBox.AppendChild(FooterTextBox_Keep);

            ////        XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
            ////        Footer_TextBox.AppendChild(Footer_Paragraphs);

            ////        XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
            ////        Footer_Paragraphs.AppendChild(Footer_Paragraph);

            ////        XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
            ////        Footer_Paragraph.AppendChild(Footer_TextRuns);

            ////        XmlElement Footer_TextRun = xml.CreateElement("TextRun");
            ////        Footer_TextRuns.AppendChild(Footer_TextRun);

            ////        XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
            ////        Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
            ////        Footer_TextRun.AppendChild(Footer_TextRun_Value);

            ////        XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
            ////        Footer_TextRun.AppendChild(Footer_TextRun_Style);

            ////        XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
            ////        Footer_TextBox_Top.InnerText = "0.06944in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Top);

            ////        XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
            ////        Footer_TextBox_Height.InnerText = "0.25in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Height);

            ////        XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
            ////        Footer_TextBox_Width.InnerText = "1.65625in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Width);


            ////        XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Style);

            ////        XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
            ////        Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

            ////        XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
            ////        Footer_TB_StyleBorderStyle.InnerText = "None";
            ////        Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

            ////        XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
            ////        Footer_TB_SBS_LeftPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

            ////        XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
            ////        Footer_TB_SBS_RightPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

            ////        XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
            ////        Footer_TB_SBS_TopPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

            ////        XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
            ////        Footer_TB_SBS_BotPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

            ////        XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
            ////        Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

            ////        //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
            ////        //Header_Text_Align.InnerText = "Center";
            ////        //Header_Text_Align_Style.AppendChild(Header_Text_Align);
            ////    }
            ////}


            //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


            XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
            XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

            //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
            //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            if (Rb_A4_Port.Checked)
            {
                Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
                Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
            }
            else
            {
                Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
                Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            }
            Page.AppendChild(Page_PageHeight);
            Page.AppendChild(Page_PageWidth);


            XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
            Page_LeftMargin.InnerText = "0.2in";
            Page.AppendChild(Page_LeftMargin);

            XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
            Page_RightMargin.InnerText = "0.2in";
            Page.AppendChild(Page_RightMargin);

            XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
            Page_TopMargin.InnerText = "0.2in";
            Page.AppendChild(Page_TopMargin);

            XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
            Page_BottomMargin.InnerText = "0.2in";
            Page.AppendChild(Page_BottomMargin);



            //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

            //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
            //EmbeddedImages.InnerText = "Image Attributes";
            //Report.AppendChild(EmbeddedImages);

            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


            string s = xml.OuterXml;

            try
            {

                //xml.Save(@"C:\Capreports\" + Pub_SubRep_Name +".rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                Delete_Existing_Summary_Rdlc();
                xml.Save(ReportPath + Pub_SubRep_Name + ".rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                //xml = null;
                GC.Collect();

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                //xml.Save(@"C:\Capreports\MSTSNP.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //string strFolderPath = Consts.Common.ReportFolderLocation + "\\MSTSNP.rdlc";    // Run at Local System
                //xml.Save(strFolderPath); //I've chosen the c:\ for the resulting file pavel.xml
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Console.ReadLine();
            // GC.ReRegisterForFinalize(xml);
            GC.SuppressFinalize(xml);
        }

        private void Btn_Save_Param_Click(object sender, EventArgs e)
        {
            if (Criteria_SelCol_List.Count > 0)
            {
                string[] XML_String = new string[2];
                XML_String = Get_XML_Format_of_Selected_Rows("Save_Params");

                if (ADDCUSTTab_Sel_Sw)
                {
                    if (!MSTTab_Sel_Sw && !SNPTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from Both 'CASEMST' and 'CASESNP' Tables.", MessageBoxIcon.Warning); return; }
                    else if (!MSTTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from 'CASEMST' Table.", MessageBoxIcon.Warning); return; }
                    else if (!SNPTab_Sel_Sw)
                    { AlertBox.Show("Please select at least One column from 'CASESNP' Table.", MessageBoxIcon.Warning); return; }
                }


                if (Check_ACTMS_Filters())
                {
                    AlertBox.Show("Queries with fields from 'CASEACT' and 'CASEMS' can only have filters on one table, not both.", MessageBoxIcon.Warning); return;
                }

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = XML_String[0];
                Save_Entity.Card_3 = XML_String[1];
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save",BaseForm,Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
            else
                AlertBox.Show("Please select columns to Save.", MessageBoxIcon.Warning);
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string Secret_SW = ((ListItem)Cmb_Applications.SelectedItem).Value.ToString();
            string Group_Sort_SW = ((ListItem)Cmb_Group_Sort.SelectedItem).Value.ToString();
            string Use_Casediff_SW = Cb_Use_DIFF.Checked ? "Y" : "N";
            string Include_Mambers = Cb_Inc_Menbers.Checked ? "Y" : "N";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            //   PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) + "\" 
            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                            "\" YEAR = \"" + Program_Year + "\" APPLICATIONS = \"" + Secret_SW + "\" SORT = \"" + Group_Sort_SW + "\" CASEDIFF = \"" + Use_Casediff_SW + "\" INCMEM = \"" + Include_Mambers +
                            "\" CATEGORY = \"" + ((ListItem)Cmb_Category.SelectedItem).Value.ToString() + "\" Data_Filter = \"" + ((ListItem)Cmb_Dat_Filter.SelectedItem).Value.ToString() + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Btn_Get_Param_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();

        }

        public static string _pastselParams = "";
        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[1]);
                Set_Criteria_SelCol_List(RepCntl_Table);
            }
        }


        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                DataColumnCollection columns = Tmp_Table.Columns;


                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                SetComboBoxValue(Cmb_Applications, dr["APPLICATIONS"].ToString());
                SetComboBoxValue(Cmb_Group_Sort, dr["SORT"].ToString());
                SetComboBoxValue(Cmb_Category, dr["CATEGORY"].ToString());
                if (!string.IsNullOrEmpty(dr["YEAR"].ToString().Trim()))
                    SetComboBoxValue(CmbYear, dr["YEAR"].ToString());

                if (columns.Contains("Data_Filter"))
                    SetComboBoxValue(Cmb_Dat_Filter, dr["Data_Filter"].ToString());


                if (dr["CASEDIFF"].ToString() == "Y")
                    Cb_Use_DIFF.Checked = true;
                else
                    Cb_Use_DIFF.Checked = false;

                if (dr["INCMEM"].ToString() == "Y")
                    Cb_Inc_Menbers.Checked = true;
                else
                    Cb_Inc_Menbers.Checked = false;

            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    { comboBox.SelectedItem = li; break; }
                }
            }
        }

        private void Set_Criteria_SelCol_List(DataTable Tmp_Table)
        {
            Criteria_SelCol_List.Clear();
            Crit_SelCol_Grid.Rows.Clear();


            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                AdhocSel_CriteriaEntity Add_Col_entity = new AdhocSel_CriteriaEntity(true);
                string MaxLength = string.Empty, toolTipText = string.Empty, Attributes = string.Empty;
                int Row_Index = 0;
                bool Criteria_Exists = false, ADDCUST_Selected = false; ;

                //DataRow foundRow = Tmp_Table.Rows.Find("ADDCUST");///

                //foreach (DataRow dr in Tmp_Table.Rows)
                //{
                //    if (dr["TABLE"].ToString() == "ADDCUST")
                //    {   
                //        ADDCUST_Selected = true;
                //        Fill_AddCust_Cust_Ques(); 
                //        break;
                //    }
                //}

                Dsp_Position = 0;
                foreach (DataRow dr in Tmp_Table.Rows)
                {
                    foreach (ADHOCFLDEntity Master in Master_Columns_List)
                    {
                        MaxLength = string.Empty;
                        if (((Master.Act_Col_Name == dr["COLNAME"].ToString() && Master.Active == "A") || (dr["TABLE"].ToString() == "ADDCUST" || dr["TABLE"].ToString() == "PRESRESP" || dr["TABLE"].ToString() == "SERCUST"))) // && Master.Col_Code.Substring(0,2) == dr["TABLEID"].ToString()) Commented on 05312014 Not Needed Actual Column Check is Enough
                        {
                            Add_Col_entity = new AdhocSel_CriteriaEntity();
                            Criteria_Exists = false;

                            Dsp_Position++;
                            //Add_Col_entity.Disp_Position = Dsp_Position.ToString();
                            Add_Col_entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
                            Add_Col_entity.Table_ID = dr["TABLEID"].ToString();
                            Add_Col_entity.Table_name = dr["TABLE"].ToString();

                            Add_Col_entity.Col_Master_Code = Master.Col_Code;

                            if (dr["TABLE"].ToString() == "ADDCUST")
                            {
                                Add_Col_entity.Column_Name = dr["TABLE"].ToString() == "ADDCUST" ? dr["COLNAME"].ToString() : Master.Act_Col_Name;
                                Add_Col_entity.Col_Master_Code = dr["COLNAME"].ToString();
                            }
                            else if (dr["TABLE"].ToString() == "PRESRESP")
                            {
                                Add_Col_entity.Column_Name = dr["TABLE"].ToString() == "PRESRESP" ? dr["COLNAME"].ToString() : Master.Act_Col_Name;
                                Add_Col_entity.Col_Master_Code = dr["COLNAME"].ToString();
                            }
                            else if (dr["TABLE"].ToString() == "SERCUST")
                            {
                                Add_Col_entity.Column_Name = dr["TABLE"].ToString() == "SERCUST" ? dr["COLNAME"].ToString() : Master.Act_Col_Name;
                                Add_Col_entity.Col_Master_Code = dr["COLNAME"].ToString();
                            }
                            else
                                Add_Col_entity.Column_Name = Master.Act_Col_Name;

                            Add_Col_entity.Can_Add_Col = Add_Col_entity.Display = (dr["DISPLAY"].ToString() == "1" ? "Y" : "N");

                            if (dr["TABLE"].ToString() == "PRESRESP")
                            {
                                Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
                                if (Addcust_Cust_Columns.Count <= 0)
                                    Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_PREASSES_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

                                foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
                                {
                                    if (Entity.Act_Cust_Code == dr["COLNAME"].ToString())
                                    {
                                        Add_Col_entity.Column_Disp_Name = Entity.Cust_Desc;
                                        break;
                                    }
                                }

                                if (Presresp_Resp_List.Count == 0)
                                    Get_PREASSES_Resp_List();
                            }
                            else if (dr["TABLE"].ToString() == "SERCUST")
                            {
                                Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
                                if (Addcust_Cust_Columns.Count <= 0)
                                    Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_SERCUST_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

                                foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
                                {
                                    if (Entity.Act_Cust_Code == dr["COLNAME"].ToString())
                                    {
                                        Add_Col_entity.Column_Disp_Name = Entity.Cust_Desc;
                                        break;
                                    }
                                }

                                if (SERCust_Resp_List.Count == 0)
                                    Get_SERCUST_Resp_List();

                            }
                            else if (dr["TABLE"].ToString() != "ADDCUST")
                            {
                                Add_Col_entity.Column_Disp_Name = Master.Col_Disp_Name;
                                if (Master.Col_Code.Substring(0, 2) == "05")
                                {
                                    //string RanlColName = Master.Col_Disp_Name;
                                    if (Master.Col_Disp_Name.Contains("Rank"))
                                        Add_Col_entity.Column_Disp_Name = Get_Rank_Desc_4rm_RankCat_Table(Master.Col_Disp_Name.Substring(5, 1), Master.Col_Disp_Name);
                                }
                            }
                            else
                            {
                                Addcust_Cust_Columns = new List<Adhoc_ADDCUSTEntity>();
                                if (Addcust_Cust_Columns.Count <= 0)
                                    Addcust_Cust_Columns = _model.AdhocData.Adhoc_Get_ADDCUST_Ques_BYHie(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));

                                foreach (Adhoc_ADDCUSTEntity Entity in Addcust_Cust_Columns)
                                {
                                    if (Entity.Act_Cust_Code == dr["COLNAME"].ToString())
                                    {
                                        Add_Col_entity.Column_Disp_Name = Entity.Cust_Desc;
                                        break;
                                    }
                                }
                                if (Addcust_Resp_List.Count == 0)
                                    Get_Addcust_Resp_List();
                            }

                            Add_Col_entity.Data_Type = dr["DATATYPE"].ToString();
                            Add_Col_entity.Disp_Data_Type = Master.Col_Resp_Type;

                            Add_Col_entity.Disp_Code_Length = Master.Col_Code_Length;
                            Add_Col_entity.Disp_Desc_Length = MaxLength = Master.Col_Desc_Length;
                            Add_Col_entity.Col_Format_Type = dr["FORMATTYPE"].ToString();

                            if (dr["TABLE"].ToString() == "ADDCUST")
                            {
                                Add_Col_entity.Disp_Code_Length = (Add_Col_entity.Column_Disp_Name.Trim().Length > 30 ? "30" : Add_Col_entity.Column_Disp_Name.Trim().Length.ToString());
                                Add_Col_entity.Disp_Desc_Length = MaxLength = (Add_Col_entity.Column_Disp_Name.Trim().Length > 30 ? "30" : Add_Col_entity.Column_Disp_Name.Trim().Length.ToString());
                                //Add_Col_entity.Disp_Desc_Length = MaxLength = Master.Col_Desc_Length;
                            }

                            if (Add_Col_entity.Col_Format_Type == "D" || Add_Col_entity.Col_Format_Type == "L")
                            {
                                if (dr["CASTTO"].ToString() == "C")
                                    //MaxLength = Column_Grid.CurrentRow.Cells["Col_MinLength"].Value.ToString();
                                    MaxLength = Master.Col_Desc_Length;
                                else
                                    //MaxLength = Column_Grid.CurrentRow.Cells["Col_MinLength"].Value.ToString();
                                    MaxLength = Master.Col_Code_Length;
                            }

                            Add_Col_entity.Max_Display_Width = Get_Column_Disp_Width(Add_Col_entity.Data_Type, (string.IsNullOrEmpty(MaxLength.Trim()) ? 0 : int.Parse(MaxLength)), Add_Col_entity.Col_Format_Type);

                            if (dr["TABLE"].ToString() == "PRESRESP")
                                Add_Col_entity.AgyCode = !string.IsNullOrEmpty(dr["ACGYCODE"].ToString()) ? dr["ACGYCODE"].ToString() : " ";
                            else if (dr["TABLE"].ToString() == "SERCUST")
                                Add_Col_entity.AgyCode = !string.IsNullOrEmpty(dr["ACGYCODE"].ToString()) ? dr["ACGYCODE"].ToString() : " ";
                            else if (dr["TABLE"].ToString() != "ADDCUST")
                                Add_Col_entity.AgyCode = !string.IsNullOrEmpty(Master.Col_AgyCode) ? Master.Col_AgyCode : " ";
                            else
                                Add_Col_entity.AgyCode = !string.IsNullOrEmpty(dr["ACGYCODE"].ToString()) ? dr["ACGYCODE"].ToString() : " ";



                            Add_Col_entity.Description = "N";
                            if ((Add_Col_entity.Col_Format_Type == "D" || Add_Col_entity.Col_Format_Type == "L") &&
                                 dr["CASTTO"].ToString() == "C")
                                Add_Col_entity.Description = "Y";

                            Add_Col_entity.Count = (dr["COUNT"].ToString() == "1" ? "Y" : "N");

                            Add_Col_entity.Sort = "N";
                            if (!string.IsNullOrEmpty(dr["SORT"].ToString().Trim()))
                                Add_Col_entity.Sort = (int.Parse(dr["SORT"].ToString()) > 0 ? "Y" : "N");

                            Add_Col_entity.Sort_Order = "0";
                            if (Add_Col_entity.Sort == "Y")
                            {
                                Add_Col_entity.Sort_Order = Next_Sort_Order.ToString();
                                Next_Sort_Order++;
                            }

                            Add_Col_entity.Break_Order = (dr["BREAK"].ToString() == "1" ? "Y" : "N");

                            Attributes = "Y" + Add_Col_entity.Description + Add_Col_entity.Count + Add_Col_entity.Sort + Add_Col_entity.Break_Order;
                            if (Add_Col_entity.Column_Name == "ENRL_DATE")
                                Attributes = "N" + Add_Col_entity.Description + Add_Col_entity.Count + Add_Col_entity.Sort + Add_Col_entity.Break_Order;

                            Add_Col_entity.Attributes = Attributes;
                            Add_Col_entity.EqualTo = ((!string.IsNullOrEmpty(dr["EQULATO"].ToString()) && dr["EQULATO"].ToString() != "NULL") ? dr["EQULATO"].ToString() : " ");
                            Add_Col_entity.NotEqualTo = ((!string.IsNullOrEmpty(dr["NOTEQULATO"].ToString()) && dr["NOTEQULATO"].ToString() != "NULL") ? dr["NOTEQULATO"].ToString() : " ");
                            Add_Col_entity.LessThan = ((!string.IsNullOrEmpty(dr["LESSTHAN"].ToString()) && dr["LESSTHAN"].ToString() != "NULL") ? dr["LESSTHAN"].ToString() : " ");
                            Add_Col_entity.GreaterThan = ((!string.IsNullOrEmpty(dr["GREATERTHAN"].ToString()) && dr["GREATERTHAN"].ToString() != "NULL") ? dr["GREATERTHAN"].ToString() : " ");
                            if (dr.Table.Columns.Contains("IS_NULL")) //Added by Sudheer on 01/07/2016
                                Add_Col_entity.Get_Nulls = ((!string.IsNullOrEmpty(dr["IS_NULL"].ToString()) && dr["IS_NULL"].ToString() != "NULL") ? dr["IS_NULL"].ToString() : " ");



                            if (Add_Col_entity.Column_Name == "ENRL_DATE")
                            {
                                bool Range_Selected = false;
                                foreach (DataRow dr1 in Tmp_Table.Rows)
                                {
                                    if (dr1["COLNAME"].ToString() == "ENRL_TODATE")
                                    {
                                        Range_Selected = true;
                                        Add_Col_entity.GreaterThan = ((!string.IsNullOrEmpty(dr1["EQULATO"].ToString()) && dr1["EQULATO"].ToString() != "NULL") ? dr1["EQULATO"].ToString() : " ");
                                    }
                                }

                                if (!Range_Selected)
                                    Add_Col_entity.EqualTo = " ";
                            }


                            if (dt_cont_cust_ques.Rows.Count == 0)
                                get_cont_cust_questions();

                            if (dt_cont_cust_ques.Rows.Count > 0 && "080042,080046, 080050".Contains(Master.Col_Code))
                            {
                                int loop_cnt = 1; bool col_found = false;
                                foreach (DataRow cont_dr in dt_cont_cust_ques.Rows)
                                {
                                    if (cont_dr["FLDH_CODE"].ToString().Contains("C"))
                                    {
                                        if (Master.Col_Code == "080042" && loop_cnt == 1)
                                        {
                                            col_found = true;
                                        }
                                        else if (Master.Col_Code == "080046" && loop_cnt == 2)
                                        {
                                            col_found = true;
                                        }
                                        else if (Master.Col_Code == "080050" && loop_cnt == 3)
                                        {
                                            col_found = true;
                                        }

                                        if (col_found)
                                        {
                                            foreach (CustfldsEntity Ent12 in Case0061_Cust_List)
                                            {
                                                if (cont_dr["FLDH_CODE"].ToString() == Ent12.CustCode)
                                                {
                                                    Add_Col_entity.Column_Disp_Name = Ent12.CustDesc; break;
                                                }
                                            }
                                            break;
                                        }
                                        loop_cnt++;
                                    }
                                }
                            }
                            //if (Tmp_Table.Columns.Contains("hhhh"))  // Logic to Compare Whether table contains Required Column
                            //    Add_Col_entity.GreaterThan = ((!string.IsNullOrEmpty(dr["GREATERTHAN"].ToString()) && dr["GREATERTHAN"].ToString() != "NULL") ? dr["GREATERTHAN"].ToString() : " ");

                            // SINDHE
                            Add_Col_entity.Criteria_SW = Master.Have_Criteria;
                            Add_Col_entity.Countable_SW = Master.Have_Count;
                            ////Row_Index = Crit_SelCol_Grid.Rows.Add(Add_Col_entity.Column_Disp_Name, Add_Col_entity.Table_name, Add_Col_entity.Data_Type, Add_Col_entity.Disp_Desc_Length,
                            ////                                     (Add_Col_entity.Display == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Description == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Count == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Sort == "Y" ? Img_Tick : Img_Blank), (dr["BREAK"].ToString() == "1" ? Img_Tick : Img_Blank),
                            ////                                     Add_Col_entity.Column_Name, Attributes, dr["EQULATO"].ToString(), dr["NOTEQULATO"].ToString(), dr["GREATERTHAN"].ToString(), dr["LESSTHAN"].ToString(), Add_Col_entity.Table_ID, dr["SORT"].ToString(), Add_Col_entity.Col_Format_Type, Add_Col_entity.AgyCode, Add_Col_entity.Col_Master_Code);

                            Row_Index = Crit_SelCol_Grid.Rows.Add(Add_Col_entity.Column_Disp_Name, Add_Col_entity.Table_name, Add_Col_entity.Data_Type, Add_Col_entity.Disp_Desc_Length,
                                                                 (Add_Col_entity.Display == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Description == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Count == "Y" ? Img_Tick : Img_Blank), (Add_Col_entity.Sort == "Y" ? Img_Tick : Img_Blank), (dr["BREAK"].ToString() == "1" ? Img_Tick : Img_Blank),
                                                                 Add_Col_entity.Column_Name, Attributes, Add_Col_entity.EqualTo, Add_Col_entity.NotEqualTo, Add_Col_entity.GreaterThan, Add_Col_entity.LessThan, Add_Col_entity.Table_ID, Add_Col_entity.Sort_Order, Add_Col_entity.Col_Format_Type, Add_Col_entity.AgyCode, Add_Col_entity.Col_Master_Code,
                                                                 Add_Col_entity.Criteria_SW, Add_Col_entity.Countable_SW, Add_Col_entity.Disp_Position, Add_Col_entity.Get_Nulls); // Nedd to Fill 05082014

                            Criteria_SelCol_List.Add(new AdhocSel_CriteriaEntity(Add_Col_entity));

                            toolTipText = string.Empty;
                            switch (Add_Col_entity.Data_Type)
                            {
                                case "Text":
                                    if (Add_Col_entity.EqualTo != "NULL" && !string.IsNullOrEmpty(Add_Col_entity.EqualTo.Trim()))
                                    {
                                        toolTipText = "Equal to : " + Add_Col_entity.EqualTo;

                                        if (Add_Col_entity.Disp_Data_Type == "D" && dr["CASTTO"].ToString() == "C")
                                            toolTipText += "\n Not equal to : " + Add_Col_entity.NotEqualTo;
                                    }
                                    break;

                                case "Numeric":
                                case "Date":
                                case "Time":
                                    if (!string.IsNullOrEmpty(Add_Col_entity.GreaterThan.Trim()) && Add_Col_entity.GreaterThan != "NULL")
                                        toolTipText = "Greater Than   : " + Add_Col_entity.GreaterThan;

                                    if (!string.IsNullOrEmpty(Add_Col_entity.EqualTo.Trim()) && Add_Col_entity.EqualTo != "NULL")
                                        toolTipText += "\n Equal to            : " + Add_Col_entity.EqualTo;

                                    if (!string.IsNullOrEmpty(Add_Col_entity.LessThan.Trim()) && Add_Col_entity.LessThan != "NULL")
                                        toolTipText += "\n Less Than        : " + Add_Col_entity.LessThan;

                                    if (!string.IsNullOrEmpty(Add_Col_entity.NotEqualTo.Trim()) && Add_Col_entity.NotEqualTo != "NULL" && Add_Col_entity.Column_Disp_Name == "Current Age")
                                        toolTipText += "\n Age as of          : " + Add_Col_entity.NotEqualTo;

                                    break;
                            }

                            if (!string.IsNullOrEmpty(toolTipText.Trim()))
                            {
                                foreach (DataGridViewCell cell in Crit_SelCol_Grid.Rows[Row_Index].Cells)
                                    cell.ToolTipText = toolTipText;
                            }

                            if ((Add_Col_entity.EqualTo != "NULL" && !string.IsNullOrEmpty(Add_Col_entity.EqualTo.Trim())) ||
                                (Add_Col_entity.NotEqualTo != "NULL" && !string.IsNullOrEmpty(Add_Col_entity.NotEqualTo.Trim())) ||
                                (Add_Col_entity.LessThan != "NULL" && !string.IsNullOrEmpty(Add_Col_entity.LessThan.Trim())) ||
                                (Add_Col_entity.GreaterThan != "NULL" && !string.IsNullOrEmpty(Add_Col_entity.GreaterThan.Trim())) ||
                                (Add_Col_entity.Get_Nulls != "NULL" && Add_Col_entity.Get_Nulls.Trim() == "Y"))
                                Criteria_Exists = true;

                            //Crit_SelCol_Grid.CurrentRow.DefaultCellStyle.ForeColor = Criteria_Exists ? Color.Blue : Color.Black;

                            Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Criteria_Exists ? Color.Blue : Color.Black;



                            break;
                        }
                    }
                }
                Table_Grid_SelectionChanged(Table_Grid, EventArgs.Empty);
                Pb_Edit.Visible = true;
                Btn_Clear_All.Visible = true;
            }
            //if (Crit_SelCol_Grid.Rows.Count > 0)
            //    Crit_SelCol_Grid.Rows[0].Selected = true;
        }

        private void Btn_Associations_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;

            CASB2012_Adhoc_AssociationForm Save_Form = new CASB2012_Adhoc_AssociationForm(Save_Entity, BaseForm);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();

        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((ListItem)CmbYear.SelectedItem).Text.ToString();
                FillFuelcntlList();
            }

        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASB0012");
        }

        private void Cmb_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Category.SelectedItem != null)
            {
                Btn_Clear_All_Click(Btn_Clear_All, EventArgs.Empty);
                Get_Master_Table_Details();
            }
        }

        // Clear all the global Variables in this method.
        private void CASB2012_AdhocForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Table_List = null;
            Column_List = null;
            Master_Columns_List = null;
            AgyTabs_List = null;
            Addcust_Cust_Columns = null;         // ADDCUST Table Columns Entity used to fill Columns Grid.                      
            Master_Table_List = null;


            // Entities that are used in Filter Criteria (Condition Sub Panel)
            Pass_HS_Funds_List = null;
            Pass_CM_Funds_List = null;
            Pass_AgyTabs_List = null;
            zipcode_List = null;
            STAFFMST_List = null;
            CaseVddlist = null;
            Activity_Cust_List = null;
            Cont_Cust_List = null;
            Activity_Resp_List = null;
            Cont_Resp_List = null;
            Addcust_Resp_List = null;
            Presresp_Resp_List = null;

            ChldTrck_Tasks_List = null;
            ALLPR_List = null;
            ALLSPR_List = null;
            Addcust_SelResp_List = null;
            PresResp_SelResp_List = null;
            ServicePlan_List = null;
            Activity_List = null;
            MileStone_List = null;
            CASEREF_List = null;
            Agys_List = null;
            Depts_List = null;
            Progs_List = null;

            Users_Table = null;
            Sites_Table = null;
            Ranksgrid = null;

            GC.Collect();
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();

            if (Criteria_SelCol_List.Count > 1)
            {
                int Curr_Row = Crit_SelCol_Grid.CurrentRow.Index;
                if (Curr_Row != 0)
                {
                    MenuItem Menu1 = new MenuItem();
                    Menu1.Text = "Move Top";
                    Menu1.Tag = "T";
                    contextMenu1.MenuItems.Add(Menu1);
                    MenuItem Menu2 = new MenuItem();
                    Menu2.Text = "Move Up";
                    Menu2.Tag = "U";
                    contextMenu1.MenuItems.Add(Menu2);
                }
                if ((Curr_Row + 1) != Criteria_SelCol_List.Count)
                {
                    MenuItem Menu3 = new MenuItem();
                    Menu3.Text = "Move Down";
                    Menu3.Tag = "D";
                    contextMenu1.MenuItems.Add(Menu3);
                    MenuItem Menu4 = new MenuItem();
                    Menu4.Text = "Move Bottom";
                    Menu4.Tag = "B";
                    contextMenu1.MenuItems.Add(Menu4);
                }
            }

            if (Criteria_SelCol_List.Count > 0)
            {
                MenuItem Menu2 = new MenuItem();
                Menu2.Text = "Delete Field";
                Menu2.Tag = "Del";
                contextMenu1.MenuItems.Add(Menu2);
            }

        }

        private void Crit_SelCol_Grid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string[] Split_Array = new string[2];

            if (objArgs.MenuItem.Tag is string)
            {
                Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");

                int Curr_Disp_Pos = int.Parse(Crit_SelCol_Grid.CurrentRow.Cells["Sel_Disp_Pos"].Value.ToString().Trim()), Proc_Row_Disp_Pos = 0;
                switch (Split_Array[0])
                {
                    case "U":
                        foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                        {
                            Proc_Row_Disp_Pos = int.Parse(dr.Cells["Sel_Disp_Pos"].Value.ToString());

                            if ((Curr_Disp_Pos - 1) == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = Curr_Disp_Pos.ToString();
                                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                                {
                                    if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                        Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                                        Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                                    {
                                        //Entity.Disp_Position = Curr_Disp_Pos.ToString();
                                        Entity.Disp_Position = "000".Substring(0, (3 - Curr_Disp_Pos.ToString().Length)) + Curr_Disp_Pos.ToString();
                                        break;
                                    }
                                }
                            }

                            if (Curr_Disp_Pos == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = (Curr_Disp_Pos - 1).ToString();
                                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                                {
                                    if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                        Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                                        Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                                    {
                                        //Entity.Disp_Position = (Curr_Disp_Pos - 1).ToString();
                                        Entity.Disp_Position = "000".Substring(0, (3 - (Curr_Disp_Pos - 1).ToString().Length)) + (Curr_Disp_Pos - 1).ToString();
                                        break;
                                    }
                                }
                            }
                        }

                        break;

                    case "D":
                        foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                        {
                            Proc_Row_Disp_Pos = int.Parse(dr.Cells["Sel_Disp_Pos"].Value.ToString());

                            if ((Curr_Disp_Pos + 1) == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = Curr_Disp_Pos.ToString();
                                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                                {
                                    if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                        Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                                        Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                                    {
                                        //Entity.Disp_Position = Curr_Disp_Pos.ToString();
                                        Entity.Disp_Position = "000".Substring(0, (3 - Curr_Disp_Pos.ToString().Length)) + Curr_Disp_Pos.ToString();
                                        Entity.Attributes = dr.Cells["Atributes_List"].Value.ToString();
                                        break;
                                    }
                                }
                            }

                            if (Curr_Disp_Pos == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = (Curr_Disp_Pos + 1).ToString();
                                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                                {
                                    if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                        Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                                        Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                                    {
                                        //Entity.Disp_Position = (Curr_Disp_Pos + 1).ToString();
                                        Entity.Disp_Position = "000".Substring(0, (3 - (Curr_Disp_Pos + 1).ToString().Length)) + (Curr_Disp_Pos + 1).ToString();
                                        Entity.Attributes = dr.Cells["Atributes_List"].Value.ToString();
                                        break;
                                    }
                                }
                            }
                        }

                        break;

                    case "T":
                        Dsp_Position = 1;
                        foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                        {
                            Proc_Row_Disp_Pos = int.Parse(dr.Cells["Sel_Disp_Pos"].Value.ToString());

                            if (Curr_Disp_Pos == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = "1"; break;
                            }
                        }

                        foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                        {
                            if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                                Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                            {
                                //Entity.Disp_Position = "1";
                                Entity.Disp_Position = "001";
                                //Entity.Attributes = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
                            }
                            else
                            {
                                Dsp_Position++;
                                //Entity.Disp_Position = Dsp_Position.ToString();
                                Entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
                                //Entity.Attributes = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
                            }
                        }

                        break;

                    case "B":
                        Dsp_Position = 0;
                        foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                        {
                            Proc_Row_Disp_Pos = int.Parse(dr.Cells["Sel_Disp_Pos"].Value.ToString());

                            if (Curr_Disp_Pos == Proc_Row_Disp_Pos)
                            {
                                dr.Cells["Sel_Disp_Pos"].Value = Criteria_SelCol_List.Count.ToString(); break;
                            }
                        }

                        foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                        {
                            if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                                Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                                Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                            {
                                //Entity.Disp_Position = Criteria_SelCol_List.Count.ToString();
                                Entity.Disp_Position = "000".Substring(0, (3 - Criteria_SelCol_List.Count.ToString().Length)) + Criteria_SelCol_List.Count.ToString();
                                //Entity.Attributes = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
                            }
                            else
                            {
                                Dsp_Position++;
                                //Entity.Disp_Position = Dsp_Position.ToString();
                                Entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
                                //Entity.Attributes = Crit_SelCol_Grid.CurrentRow.Cells["Atributes_List"].Value.ToString();
                            }
                        }

                        break;

                    case "Del": Delete_SelCol_4rm_BottomGrid(); break;
                }

                {
                    int Row_Index = 0;
                    Crit_SelCol_Grid.Rows.Clear();

                    //List<ListItem> Sort_list = new List<ListItem>();
                    //foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    //        Sort_list.Add(new ListItem(Entity.Disp_Position, Entity.Column_Disp_Name));

                    //Sort_list.Sort(delegate(ListItem p1, ListItem p2) { return p1.Text.CompareTo(p2.Text); });

                    Criteria_SelCol_List = Criteria_SelCol_List.OrderBy(u => u.Disp_Position).ToList();

                    //foreach (ListItem List in Sort_list)
                    //{
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        //if (List.Text == Entity.Disp_Position)
                        {
                            Row_Index = Crit_SelCol_Grid.Rows.Add(Entity.Column_Disp_Name, Entity.Table_name, Entity.Data_Type, Entity.Disp_Desc_Length,
                                                                 (Entity.Display == "Y" ? Img_Tick : Img_Blank), (Entity.Description == "Y" ? Img_Tick : Img_Blank), (Entity.Count == "Y" ? Img_Tick : Img_Blank), (Entity.Sort == "Y" ? Img_Tick : Img_Blank), (Entity.Break_Order == "Y" ? Img_Tick : Img_Blank),
                                                                 Entity.Column_Name, Entity.Attributes, Entity.EqualTo, Entity.NotEqualTo, Entity.GreaterThan, Entity.LessThan, Entity.Table_ID, Entity.Sort_Order, Entity.Col_Format_Type, Entity.AgyCode, Entity.Col_Master_Code,
                                                                 Entity.Criteria_SW, Entity.Countable_SW, Entity.Disp_Position, Entity.Disp_Position, Entity.Get_Nulls); // Nedd to Fill 05082014

                            if (!string.IsNullOrEmpty(Entity.EqualTo.Trim()) || !string.IsNullOrEmpty(Entity.NotEqualTo.Trim()) || !string.IsNullOrEmpty(Entity.GreaterThan.Trim()) || !string.IsNullOrEmpty(Entity.LessThan.Trim()))
                                Crit_SelCol_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = Color.Blue;
                            //break;
                        }
                    }
                    //}
                }
                //Criteria_SelCol_List = Criteria_SelCol_List.OrderBy(u => u.Disp_Position).ThenBy(u => u.ADDRESSR1).ToList();
            }
            if (Crit_SelCol_Grid.Rows.Count > 0)
                Crit_SelCol_Grid.Rows[0].Selected = true;
        }

        private void Delete_SelCol_4rm_BottomGrid()
        {
            bool Rec_Deleted = false;
            foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
            {
                foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                {
                    if (Entity.Column_Name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                        Entity.Table_name == Crit_SelCol_Grid.CurrentRow.Cells["Sel_TableName"].Value.ToString() &&
                        Entity.Col_Master_Code == Crit_SelCol_Grid.CurrentRow.Cells["Sel_Col_Master_Code"].Value.ToString())
                    {
                        Criteria_SelCol_List.Remove(Entity);
                        Dsp_Position--;
                        Rec_Deleted = true;
                        break;
                    }
                }
                Crit_SelCol_Grid.Rows.RemoveAt(Crit_SelCol_Grid.CurrentRow.Index);

                if (!(Crit_SelCol_Grid.RowCount > 0))
                    Pb_Edit.Visible = false;
                break;
            }

            if (Rec_Deleted)
            {
                Table_Grid_SelectionChanged(Table_Grid, EventArgs.Empty);
                Dsp_Position = 0;
                foreach (DataGridViewRow dr in Crit_SelCol_Grid.Rows)
                {
                    foreach (AdhocSel_CriteriaEntity Entity in Criteria_SelCol_List)
                    {
                        if (Entity.Column_Name == dr.Cells["Sel_Org_ColNmae"].Value.ToString() &&
                            Entity.Table_name == dr.Cells["Sel_TableName"].Value.ToString() &&
                            Entity.Col_Master_Code == dr.Cells["Sel_Col_Master_Code"].Value.ToString())
                        {
                            Dsp_Position++;
                            //Entity.Disp_Position = Dsp_Position.ToString();
                            Entity.Disp_Position = "000".Substring(0, (3 - Dsp_Position.ToString().Length)) + Dsp_Position.ToString();
                        }
                    }
                }
            }
        }

        string CAMS_Date_SW = "", CAMS_From_Date = "", CAMS_To_Date = "";
        private void Btn_CAMS_Work_Click(object sender, EventArgs e)
        {
            //DataSet ds = _model.AdhocData.Generate_CAMS_Work_File(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), Curr_Year_To_Pass);
            //if(ds != null)
            //{
            //    if(ds.Tables.Count > 0)
            //    {
            //        if (int.Parse(ds.Tables[0].Rows[0]["Added_Recs"].ToString()) > 0)
            //            MessageBox.Show("ACTMS Table Regenerated for Selected Hierarchy", "CAP Systems");
            //    }
            //}

            CASB2012_ConditionsForm Conditions_Form = new CASB2012_ConditionsForm(CAMS_Date_SW, CAMS_From_Date, CAMS_To_Date, Current_Hierarchy, Program_Year, BaseForm.UserID);
            Conditions_Form.FormClosed += new FormClosedEventHandler(On_CAMS_Conditions_Closed);
            Conditions_Form.StartPosition = FormStartPosition.CenterScreen;
            //Conditions_Form.TopMost= true;
            //Conditions_Form.Focus(); 
            Conditions_Form.ShowDialog();

        }

        private void Btn_Priv_Rep_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, ReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        ////private DataTable convertStringToDataTable(string xmlString)
        ////{
        ////    DataSet dataSet = new DataSet();
        ////    StringReader stringReader = new StringReader(xmlString);
        ////    dataSet.ReadXml(stringReader);
        ////    DataTable dt = dataSet.Tables[0];
        ////    return dataSet.Tables[0];
        ////}

        DataTable dt_cont_cust_ques = new DataTable();
        private void get_cont_cust_questions()
        {
            DataSet ds = Captain.DatabaseLayer.FieldControlsDB.Browse_FLDCNTLHIE("CASE0061", Current_Hierarchy, null, null, null, null, null);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt_cont_cust_ques = ds.Tables[0];
        }


        private string Get_cont_question_code(string resp_code)
        {
            string rtn_ques_code = "";

            if (dt_cont_cust_ques.Rows.Count == 0)
                get_cont_cust_questions();

            if (dt_cont_cust_ques.Rows.Count > 0)
            {
                int loop_cnt = 1; bool col_found = false;
                foreach (DataRow cont_dr in dt_cont_cust_ques.Rows)
                {
                    if (cont_dr["FLDH_CODE"].ToString().Contains("C"))
                    {
                        if (resp_code == "080042" && loop_cnt == 1)
                        {
                            col_found = true;
                        }
                        else if (resp_code == "080046" && loop_cnt == 2)
                        {
                            col_found = true;
                        }
                        else if (resp_code == "080050" && loop_cnt == 3)
                        {
                            col_found = true;
                        }

                        if (col_found)
                        {
                            rtn_ques_code = cont_dr["FLDH_CODE"].ToString();
                            break;
                        }
                        loop_cnt++;
                    }

                }
            }
            return rtn_ques_code;
        }

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       Dynamic Summary RDLC           >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 





















    }
}



