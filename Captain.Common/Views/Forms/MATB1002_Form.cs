#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
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
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using DevExpress.XtraPrinting;
using NPOI.SS.Formula.Functions;
using DevExpress.PivotGrid.OLAP;
using DevExpress.Utils.Win.Hook;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MATB1002_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion


        public MATB1002_Form(BaseForm baseform, PrivilegeEntity previliges)
        {
            InitializeComponent();
            Selected_Scales_List = new List<MATDEFEntity>();

            Privileges = previliges;
            BaseForm = baseform;
            this.Text = /*Privileges.Program + " - " + */Privileges.PrivilegeName;
            _model = new CaptainModel();
            this.Size = new Size(760, 425);//389);
            //if (BaseForm.UserProfile.Security.Trim() == "P" || BaseForm.UserProfile.Security.Trim() == "B")
            //{
            //    btnChangeDates.Enabled = true;
            //    btnChangeDates.Visible = true;
            //}

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            //this.Site_Panel.Location = new System.Drawing.Point(58, 210);           

            Agency = BaseForm.BaseAgency;
            Depart = BaseForm.BaseDept;
            Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            ReportPath = _model.lookupDataAccess.GetReportPath();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            Fill_Matrix_Combo();
            Fill_CaseWorker_Combo();
            FillAssessment_Types();
            Fill_Sites();

            caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "R");

            if (BaseForm.BaseAgencyControlDetails != null)
            {
                if (BaseForm.BaseAgencyControlDetails.MatAssesment.ToUpper() == "Y")
                {
                    cmbAssessmentType.Visible = true;
                    lblAssType.Visible = true;
                    CommonFunctions.SetComboBoxValue(cmbAssessmentType, "H");
                }
            }
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<MATDEFEntity> Selected_Scales_List { get; set; }

        //public List<MATDEFEntity> Scales_List { get; set; }

        public string ReportPath { get; set; }

        public List<CommonEntity> propfundingSource { get; set; }

        public List<CaseSiteEntity> propCaseAllSiteEntity { get; set; }

        public List<HierarchyEntity> caseHierarchy { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }
        public string strAgency { get; set; }

        public string strDept { get; set; }

        public string strProg { get; set; }

        #endregion


        List<DG_ResTab_Entity> DG_BaseTable_List = new List<DG_ResTab_Entity>();
        List<DG_ResTab_Entity> DG_Table_List = new List<DG_ResTab_Entity>();
        List<DG_ResTab_Entity> DG_SubTable_List = new List<DG_ResTab_Entity>();

        private void FillAssessment_Types()
        {
            cmbAssessmentType.Items.Clear();
            List<Captain.Common.Utilities.ListItem> ListAssesmentType = new List<Captain.Common.Utilities.ListItem>();
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Households", "H"));
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Individual", "I"));
            ListAssesmentType.Add(new Captain.Common.Utilities.ListItem("Both", "B"));
            cmbAssessmentType.Items.AddRange(ListAssesmentType.ToArray());
            cmbAssessmentType.SelectedIndex = 0;
        }

        private void Get_DG_Result_Table_Structure()
        {
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Agy_Type", "AGY Type", "L", "3.8in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Col_Name", "Column Name To Compare", "L", "3.8in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Child_Code", "Child Code", "L", "2in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Desc", "Attribute", "L", "4.3in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_From_Age", "Age From", "R", "3.8in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_To_Age", "Age To", "R", "3.8in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Period_Count", "Rep Period", "R", "1.5in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Cum_Count", "Cumulative", "R", "1.5in"));
        }

        private void Fill_CaseWorker_Combo()
        {
            Cmb_Worker.Items.Clear();
            Cmb_Worker.ColorMember = "FavoriteColor";
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //CmbWorker.Items.Insert(0, new ListItem("All", "**"));
            //listItem.Add(new Captain.Common.Utilities.ListItem("All", "**", " ", Color.White));
            listItem.Add(new Captain.Common.Utilities.ListItem("All", "**"));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(BaseForm.BaseHierarchyCwFormat, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        //if ((Mode == "Add" && dr["PWH_INACTIVE"].ToString().Trim() == "N") || (Mode == "Edit"))
                        //listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Green));

                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), dr["PWH_INACTIVE"].ToString().Equals("Y") ? Color.Red : Color.Black));
                    }
                }
            }
            Cmb_Worker.Items.AddRange(listItem.ToArray());
            Cmb_Worker.SelectedIndex = 0;

        }

        private void FillEnrollStatus()
        {
            cmbEnrlStatus.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Enrolled", "E"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Withdrawn", "W"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Enrolled/Withdrawn", "B"));

            cmbEnrlStatus.Items.AddRange(listItem.ToArray());
            cmbEnrlStatus.SelectedIndex = 2;
        }

        private DataTable GenerateTransposedBaseTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            DG_BaseTable_List.Clear();
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());
            DG_BaseTable_List.Add(new DG_ResTab_Entity("Y", inputTable.Columns[0].ColumnName.ToString(), inputTable.Columns[0].ColumnName.ToString(), "L", "2.3in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("Y", inputTable.Columns[0].ColumnName.ToString(), "Benchmark Level", "L", "2.3in"));

            // Header row's second column onwards, 'inputTable's first column taken
            int Col_Pos = 1;
            foreach (DataRow inRow in inputTable.Rows)
            {
                //string newColName = inRow[0].ToString();
                Col_Pos++;
                string newColName = "Col_" + Col_Pos.ToString();

                DG_BaseTable_List.Add(new DG_ResTab_Entity("Y", newColName, inRow[1].ToString(), "R", "0.4in"));


                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            DG_Table_List.Clear();
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());
            DG_Table_List.Add(new DG_ResTab_Entity("Y", inputTable.Columns[0].ColumnName.ToString(), inputTable.Columns[0].ColumnName.ToString(), "L", "2.3in"));
            //DG_Table_List.Add(new DG_ResTab_Entity("Y", inputTable.Columns[0].ColumnName.ToString(), "Benchmark Level", "L", "2.3in"));

            // Header row's second column onwards, 'inputTable's first column taken
            int Col_Pos = 1;
            foreach (DataRow inRow in inputTable.Rows)
            {
                //string newColName = inRow[0].ToString();
                Col_Pos++;
                string newColName = "Col_" + Col_Pos.ToString();

                DG_Table_List.Add(new DG_ResTab_Entity("Y", newColName, inRow[1].ToString(), "R", "0.4in"));


                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }


        private DataTable GenerateTransposed_SubTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            DG_SubTable_List.Clear();
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());
            DG_SubTable_List.Add(new DG_ResTab_Entity("Y", inputTable.Columns[0].ColumnName.ToString(), " ", "L", "2.3in"));

            // Header row's second column onwards, 'inputTable's first column taken
            int Col_Pos = 1;
            foreach (DataRow inRow in inputTable.Rows)
            {
                //string newColName = inRow[0].ToString();
                Col_Pos++;
                string newColName = "Col_" + Col_Pos.ToString();

                //DG_SubTable_List.Add(new DG_ResTab_Entity("Y", newColName, inRow[1].ToString(), "L", "0.6in"));
                DG_SubTable_List.Add(new DG_ResTab_Entity("Y", newColName, " ", "R", "0.4in"));


                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /**HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();*/

            // Updated by Vikash on 03/29/2023 for Report Parameters filtering
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void Fill_Matrix_Combo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            Cmb_Matrix.Items.Clear();
            Cmb_Matrix.ColorMember = "FavoriteColor";
             matdefEntity = matdefEntity.OrderBy(x=> x.Active).ToList();
            if (matdefEntity.Count > 0)
            {
                foreach (MATDEFEntity matdef in matdefEntity)
                    Cmb_Matrix.Items.Add(new Captain.Common.Utilities.ListItem(matdef.Desc, matdef.Mat_Code, matdef.Interval, matdef.Show_BA, matdef.Date_option /*string.Empty*/,matdef.Active,matdef.Active.Equals("A") ? Color.Black : Color.Red));
            }
            else
                Cmb_Matrix.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0"));

            Cmb_Matrix.SelectedIndex = 0;

            


        }


        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //**HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
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
                    Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + hierarchy.Substring(2, 2) + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                }
            }
        }

       // string DeptName = string.Empty; string ProgName = string.Empty;
        string Program_Year, Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            //strAgency = Agy;
            //strDept = Dept;
            //strProg = Prog;

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
                    //{
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                        //DeptName = ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString().Trim() + "/";
                    //}
                }
            }
            else
            //{
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";
                //DeptName = "All Departments /";
            //}
            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                    //{
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        //ProgName = ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString().Trim();
                    //}
                }
            }
            else
            //{
                Txt_HieDesc.Text += "PROG : ** - All Programs ";
                //ProgName = "All Programs ";
            //}


            if (Agy != "**")
            {
                //Get_NameFormat_For_Agencirs(Agy);
                Member_NameFormat = BaseForm.BaseHierarchyCnFormat.ToString(); CAseWorkerr_NameFormat = BaseForm.BaseHierarchyCwFormat.ToString();
            }
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(695, 25);
                if(Agy=="**") Agy = string.Empty; if(Dept =="**")Dept = string.Empty; if(Prog=="**") Prog = string.Empty;
                Agency = Agy; Depart = Dept; Program = Prog; strYear = "    ";
                Fill_Sites();
            }

        }
        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e) //** Added by Vikash for Matrix Saved parmeters 02/15/2023
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.SelectedIndexChanged -= new EventHandler(CmbYear_SelectedIndexChanged);
            CmbYear.Items.Clear();
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
                            TempCompareYear = int.Parse(Year);
                        List<ListItem> listItem = new List<ListItem>();
                        listItem.Add(new ListItem("All", ""));
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = (i+1);
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }
            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;
            Fill_Sites();

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(627, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(695, 25);

            CmbYear.SelectedIndexChanged += new EventHandler(CmbYear_SelectedIndexChanged);
        }



        //

        string Sel_AGY, Sel_DEPT, Sel_PROG = string.Empty, Sel_MatriX = " ";
        string Asmt_From_Date, Sel_Group_Sort, Sel_Use_CASEDIFF, Sel_Include_Members = string.Empty;
        private void Get_Report_Selection_Parameters()
        {
            Sel_AGY = Current_Hierarchy.Substring(0, 2);
            Sel_DEPT = Current_Hierarchy.Substring(2, 2);
            Sel_PROG = Current_Hierarchy.Substring(4, 2);

            Sel_MatriX = "All";//((ListItem)Cmb_Matrix.SelectedItem).Text.ToString();

            Sel_Use_CASEDIFF = "Use CASEDIFF : No";
            //if (Cb_Use_DIFF.Checked)
            //    Sel_Use_CASEDIFF = "Use CASEDIFF : Yes";


            //Sel_Include_Members = "Include All Members : No";
            //if (Cb_Inc_Menbers.Checked)
            //    Sel_Include_Members = "Include All Members : Yes";
        }

        private string Get_Sel_Sites()
        {
            string Sel_Scales_Codes = string.Empty;

            foreach (MATDEFEntity Entity in Selected_Scales_List)
            {
                Sel_Scales_Codes += "'" + Entity.Scale_Code + "' ,";
            }

            if (Sel_Scales_Codes.Length > 0)
                Sel_Scales_Codes = Sel_Scales_Codes.Substring(0, (Sel_Scales_Codes.Length - 1));

            return Sel_Scales_Codes;
        }

        List<MATDEFBMEntity> BenchMark_List = new List<MATDEFBMEntity>();
        private void Get_Bench_Marks_List()
        {
            MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
            Search_Entity.MatCode = ((Captain.Common.Utilities.ListItem)Cmb_Matrix.SelectedItem).Value.ToString();
            BenchMark_List = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
        }

        List<MATDEFEntity> Scales_List = new List<MATDEFEntity>();
        private void GetScalesList()
        {
            Scales_List.Clear();
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Mat_Code = ((Captain.Common.Utilities.ListItem)Cmb_Matrix.SelectedItem).Value.ToString();
            Scales_List = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
        }


        string strFundingCodes = string.Empty;
        private List<SqlParameter> Get_Sql_Parametets_List()
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@Agency", Current_Hierarchy.Substring(0, 2)));
            sqlParamList.Add(new SqlParameter("@Dept", Current_Hierarchy.Substring(2, 2)));
            sqlParamList.Add(new SqlParameter("@Prog", Current_Hierarchy.Substring(4, 2)));
            if (CmbYear.Visible)
            {
                if (((ListItem)CmbYear.SelectedItem).Text.ToString() != "All")
                    sqlParamList.Add(new SqlParameter("@Year", (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    ")));
            }
            else
            {
                sqlParamList.Add(new SqlParameter("@Year", "    "));
            }
            //sqlParamList.Add(new SqlParameter("@App", Entity.Tabs_Type));
            //sqlParamList.Add(new SqlParameter("@Asmt_Type", (Rb_Asmt_FDate.Checked ? "S" : "L")));
            sqlParamList.Add(new SqlParameter("@Base_From_Date", Base_FDate.Value.ToShortDateString()));
            sqlParamList.Add(new SqlParameter("@Base_To_Date", Base_TDate.Value.ToShortDateString()));
            sqlParamList.Add(new SqlParameter("@Asmt_From_Date", Asmt_F_Date.Value.ToShortDateString()));
            sqlParamList.Add(new SqlParameter("@Asmt_To_Date", Asmt_T_Date.Value.ToShortDateString()));
            sqlParamList.Add(new SqlParameter("@Matrix_Code", ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString()));
            sqlParamList.Add(new SqlParameter("@BaseLine_Date_Sel_SW", (Rb_Asmt_TDate.Checked ? "L" : "F")));

            if (((ListItem)Cmb_Worker.SelectedItem).Value.ToString() != "**")
                sqlParamList.Add(new SqlParameter("@CaseWorker", ((ListItem)Cmb_Worker.SelectedItem).Value.ToString()));

            sqlParamList.Add(new SqlParameter("@Include_Enrl", (Cb_Enroll.Checked ? "Y" : "N")));
            if (Cb_Enroll.Checked)
            {
                sqlParamList.Add(new SqlParameter("@Enrl_Asof_Date", Asof_From_Date.Value.ToShortDateString()));
                sqlParamList.Add(new SqlParameter("@Enrl_Asof_Status", ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString()));
                //sqlParamList.Add(new SqlParameter("@Enrl_Prog", Txt_Program.Text.Substring(0, 6)));

                sqlParamList.Add(new SqlParameter("@Module_Code", Privileges.ModuleCode.Trim()));
                if (Privileges.ModuleCode == "03")
                    sqlParamList.Add(new SqlParameter("@Enrl_Prog", Txt_Program.Text.Trim()));
                else if (Privileges.ModuleCode == "02")
                {
                    sqlParamList.Add(new SqlParameter("@Asof_Site", Txt_DrawEnroll_Site.Text.Trim()));
                    if (!string.IsNullOrEmpty(Txt_DrawEnroll_Room.Text.Trim()))
                        sqlParamList.Add(new SqlParameter("@Asof_Romm", Txt_DrawEnroll_Room.Text.Trim()));
                    if (!string.IsNullOrEmpty(Txt_DrawEnroll_AMPM.Text.Trim()))
                        sqlParamList.Add(new SqlParameter("@Asof_AMPM", Txt_DrawEnroll_AMPM.Text.Trim()));
                }


                if (Asof_From_Date.Checked)
                    sqlParamList.Add(new SqlParameter("@Asof_From_Date", Asof_From_Date.Value.ToShortDateString()));

                if (Asof_To_Date.Checked)
                    sqlParamList.Add(new SqlParameter("@Asof_TO_Date", Asof_To_Date.Value.ToShortDateString()));

                if (rbFundSel.Checked == true)
                {
                    strFundingCodes = string.Empty;
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                else
                {
                    strFundingCodes = "A";
                }

                if (!string.IsNullOrEmpty(strFundingCodes.Trim()))
                    sqlParamList.Add(new SqlParameter("@FundHie", strFundingCodes.Trim()));
                
            }

            if (Rb_Sel_Scales.Checked)
                sqlParamList.Add(new SqlParameter("@Scales_List", Get_Sel_Sites()));

            if (Cb_Only_Asmt_Scale.Checked)
                sqlParamList.Add(new SqlParameter("@Only_Assed_Scales", "Y"));
            if (cmbAssessmentType.Visible == true)
            {
                sqlParamList.Add(new SqlParameter("@Assessment_Type", (((ListItem)cmbAssessmentType.SelectedItem).Value.ToString())));
            }

            if((((ListItem)cmbSite.SelectedItem).Value.ToString())!="0")
                sqlParamList.Add(new SqlParameter("@IntakeSite", (((ListItem)cmbSite.SelectedItem).Value.ToString())));

            if (chkDet.Checked== true)
            {
                sqlParamList.Add(new SqlParameter("@ChkDet", "Y"));
            }

            sqlParamList.Add(new SqlParameter("@UserName", BaseForm.UserID));

            if (SelectedAlertCodes == "")
                SelectedAlertCodes = "**";

            sqlParamList.Add(new SqlParameter("@AlertCodes", SelectedAlertCodes));

            return sqlParamList;
        }

        bool[] Pagesetup_results = new bool[5];
        bool Include_header = false, Include_Footer = false, Include_Header_Title = false, Include_Header_Image = false,
             Include_Footer_PageCnt = false, Save_This_Adhoc_Criteria = false, Page_Setup_Completed = false;
        string Main_Rep_Name = " ", Rep_Name = " ", Rep_Header_Title = " ", Page_Orientation = "A4 Portrait", Pub_SubRep_Name = string.Empty;
        private void On_Pagesetup_Form_Closed(object sender, FormClosedEventArgs e)
        {
            Include_header = Include_Footer = Include_Header_Title = Include_Header_Image =
            Include_Footer_PageCnt = Save_This_Adhoc_Criteria = false;

            Rep_Name = Rep_Header_Title = " "; Page_Orientation = "A4 Portrait";

            CASB2012_AdhocPageSetup form = sender as CASB2012_AdhocPageSetup;
            if (form.DialogResult == DialogResult.OK)
            {
                //form.Close();
                //System.Threading.Thread.Sleep(200);
                Page_Setup_Completed = true;
                Pagesetup_results = form.Get_Checkbox_Status();

                Include_header = Pagesetup_results[0]; Include_Header_Title = Pagesetup_results[1]; Include_Header_Image = Pagesetup_results[2];
                Include_Footer = Pagesetup_results[3]; Include_Footer_PageCnt = Pagesetup_results[4];
                Save_This_Adhoc_Criteria = Pagesetup_results[5];

                if (Include_Header_Title)
                    Rep_Header_Title = form.Get_Header_Title();

                Main_Rep_Name = Pub_SubRep_Name = Rep_Name = "SYSTEM " + form.Get_Report_Name();
                Rep_Name += ".rdlc"; Pub_SubRep_Name += "SummaryReport";

                //Get_Selection_Criteria();

                DataSet programData = _model.AdhocData.Rep_MAT1002_MatAssessments(Get_Sql_Parametets_List(), Privileges.ModuleCode);
                if (programData.Tables.Count > 0)
                {
                    DataTable Trans_BaseprogramData = Calculate_TableRow_Totals(programData.Tables[5]);
                    Trans_BaseprogramData = GenerateTransposedTable(Trans_BaseprogramData);
                    
                    DataTable Trans_programData = Calculate_TableRow_Totals(programData.Tables[6]);
                    Trans_programData = GenerateTransposedTable(Trans_programData);

                    //if (Privileges.ModuleCode == "02")
                    //{
                    //    foreach (DataColumn cl in Trans_programData.Columns)
                    //    {
                    //         foreach (MATDEFBMEntity ent in BenchMark_List)
                    //        {
                    //            if (cl.ColumnName.Trim().Contains(ent.Code.Trim()) && cl.ColumnName.Trim().Substring(0,1) != "R")
                    //            {
                    //        switch (cl.ColumnName.Trim().Substring(0,1))
                    //        {
                    //            case "P": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                    //            case "C": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                    //            case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                    //            case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                    //            case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                    //            case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                    //            case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                    //        }
                    //            }
                    //    }
                    //}

                    DataTable Trans_Sub_Columns = programData.Tables[3];
                    DataTable Trans_Sub_programData = GenerateTransposed_SubTable(programData.Tables[8]);
                    DataTable Counts_Table = programData.Tables[9];
                    DataTable Scales = programData.Tables[5];
                    DataTable Details_Table = new DataTable();
                    DataTable ScalesAssTable = new DataTable();
                    DataTable ScalesDetailTable = new DataTable();
                    if (chkDet.Checked)
                    {
                        Details_Table = programData.Tables[9];
                        ScalesAssTable = programData.Tables[10];
                        ScalesDetailTable = programData.Tables[11];
                    }


                    BaseLine_Clients_Cnt = Counts_Table.Rows[0]["BaseLine_App_Cnt"].ToString();
                    RepPeriod_Clients_Cnt = Counts_Table.Rows[0]["Rep_App_Cnt"].ToString();
                    FolloUp_Clients_Cnt = Counts_Table.Rows[0]["Followup_Cnt"].ToString();
                    Stable_Clients_Cnt = Counts_Table.Rows[0]["Progress_Cnt"].ToString();
                    Progress_Clients_Cnt = Counts_Table.Rows[0]["Stable_Cnt"].ToString();

                    //Dynamic_RDLC();
                    //Dynamic_Sub_RDLC();
                    //Dynamic_Sub_RDLC1();

                    foreach (DataRow dr in Trans_programData.Rows)
                    {
                        if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                        {
                            dr.Delete();
                            break;
                        }
                    }

                    string Tmp_BM_Code = string.Empty;
                    foreach (DataRow dr in Trans_programData.Rows)
                    {
                        Tmp_BM_Code = dr["BM_Scale"].ToString();

                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                        {
                            if(Tmp_BM_Code=="BM_S")
                            {
                                dr["BM_Scale"] = "Scale Bypassed"; break;
                            }
                            if (Tmp_BM_Code.Substring(3, (Tmp_BM_Code.Length - 3)) == Entity.Code)
                            {
                                dr["BM_Scale"] = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                break;
                            }
                        }
                    }

                    foreach (DataRow dr in Trans_BaseprogramData.Rows)
                    {
                        Tmp_BM_Code = dr["BM_Scale"].ToString();

                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                        {
                            if (Tmp_BM_Code == "BM_S")
                            {
                                dr["BM_Scale"] = "Scale Bypassed"; break;
                            }
                            if (Tmp_BM_Code.Substring(3, (Tmp_BM_Code.Length - 3)) == Entity.Code)
                            {
                                dr["BM_Scale"] = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                break;
                            }
                        }
                    }

                   

                    foreach (DataRow dr in Trans_Sub_programData.Rows)
                    {
                        if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                        {
                            dr.Delete();
                            break;
                        }
                    }

                    if (Privileges.ModuleCode == "03")
                    {
                        foreach (DataRow dr in Trans_Sub_programData.Rows)
                        {
                            if(Trans_Sub_Columns.Rows.Count>0)
                            {
                                foreach(DataRow dc in Trans_Sub_Columns.Rows)
                                {
                                    if (dr["BM_Scale"].ToString() == dc["BM_Ass_Code"].ToString())
                                        dr["BM_Scale"] = dc["BM_Desc"].ToString();break;
                                }
                            }
                            //switch (dr["BM_Scale"].ToString())
                            //{
                            //    case "a": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                            //    case "b": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                            //    case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            //    case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            //    case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            //    case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            //    case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            //}
                        }
                    }
                    else if (Privileges.ModuleCode == "02")
                    {
                        foreach (DataRow dr in Trans_Sub_programData.Rows)
                        {
                            if (Trans_Sub_Columns.Rows.Count > 0)
                            {
                                foreach (DataRow dc in Trans_Sub_Columns.Rows)
                                {
                                    if (dr["BM_Scale"].ToString() == dc["BM_Ass_Code"].ToString())
                                        dr["BM_Scale"] = dc["BM_Desc"].ToString(); break;
                                }
                            }
                            //foreach (MATDEFBMEntity ent in BenchMark_List)
                            //{
                            //    if (dr["BM_Scale"].ToString().Contains(ent.Code.Trim()) && dr["BM_Scale"].ToString().Substring(0, 1) != "R")
                            //    {
                            //        switch (dr["BM_Scale"].ToString().Substring(0, 1))
                            //        {
                            //            case "P": dr["BM_Scale"] = "# progressed to " + ent.Desc; break;
                            //            case "C": dr["BM_Scale"] = "# continued at " + ent.Desc; break;
                            //            //case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            //            //case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            //            //case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            //            //case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            //            //case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            //        }
                            //    }

                            //    if (dr["BM_Scale"].ToString().Contains("R"))
                            //    {
                            //        dr["BM_Scale"] = "Regressed ";
                            //    }
                            //}

                            ////switch (dr["BM_Scale"].ToString())
                            ////{
                            ////    case "a": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                            ////    case "b": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                            ////    case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            ////    case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            ////    case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            ////    case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            ////    case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            ////}
                        }
                    }
                    Trans_Sub_programData.AcceptChanges();

                    //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Trans_programData, Trans_Sub_programData, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, string.Empty); //8, 5
                    //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
                    //RDLC_Form.ShowDialog();

                    //OnExcel_Report(Details_Table, Scales, ScalesAssTable, ScalesDetailTable, Trans_BaseprogramData, Trans_programData, Trans_Sub_programData, programData);
                    //if (chkDet.Checked)
                    //{
                    //    if (Details_Table.Rows.Count > 0)
                    //        OnExcel_Report(Details_Table, Scales, ScalesAssTable, ScalesDetailTable, Trans_BaseprogramData, Trans_programData, Trans_Sub_programData, programData);
                    //}

                    //if (!Data_processed)
                    //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");
                }
            }
        }

        DirectoryInfo MyDir;
        private void Delete_Dynamic_RDLC_File(object sender, FormClosedEventArgs e)
        {
            CASB2012_AdhocRDLCForm form = sender as CASB2012_AdhocRDLCForm;
            //MyDir = new DirectoryInfo(@"C:\Capreports\");
            //MyDir = new DirectoryInfo(Consts.Common.ReportFolderLocation + "\\"); // Run at Server
            //MyDir = new DirectoryInfo(Consts.Common.Tmp_ReportFolderLocation + "\\"); // Run at Server
            MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server

            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            bool MasterRep_Deleted = false, SubReport_Deleted = false;
            foreach (FileInfo MyFile in MyFiles)
            {
                if (MyFile.Exists)
                {
                    if (Rep_Name == MyFile.Name)
                    {
                        MyFile.Delete();
                        MasterRep_Deleted = true;
                    }

                    if (Rep_Name + "SubReport.rdlc" == MyFile.Name) // RepDel
                    {
                        MyFile.Delete();
                        SubReport_Deleted = true;
                    }

                    if (MasterRep_Deleted && SubReport_Deleted)
                        break;
                }
            }
        }


        string BaseLine_Clients_Cnt = "0", RepPeriod_Clients_Cnt = "0", FolloUp_Clients_Cnt = "0",
               Stable_Clients_Cnt = "0", Progress_Clients_Cnt = "0";
        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(Asof_From_Date, null); _errorProvider.SetError(Asof_To_Date, null);
            _errorProvider.SetError(rbFundSel, null); _errorProvider.SetError(Rb_Sel_Scales, null);
            _errorProvider.SetError(Asmt_F_Date, null); _errorProvider.SetError(Asmt_T_Date, null);
            _errorProvider.SetError(Base_FDate, null); _errorProvider.SetError(Base_TDate, null);
            _errorProvider.SetError(Pb_Prog, null); _errorProvider.SetError(Pb_Withdraw_Enroll, null);
            _errorProvider.SetError(rdbSelAlertCds, null);
            if (Validate_Report())
            {
                //changed by Sudheer on 05/08/2020
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "XLS");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();

                ////Page_Setup_Completed = false;
                //CASB2012_AdhocPageSetup PageSetup_Form = new CASB2012_AdhocPageSetup(20, 10, Privileges.Program);
                //PageSetup_Form.FormClosed += new Form.FormClosedEventHandler(On_Pagesetup_Form_Closed);
                //PageSetup_Form.ShowDialog();
                ////if (Page_Setup_Completed)
                ////    Process_report();



                ////DataSet programData = _model.AdhocData.Rep_MAT0002_MatAssessments(Get_Sql_Parametets_List());
                ////DataTable Trans_programData = Calculate_TableRow_Totals(programData.Tables[5]);
                ////Trans_programData = GenerateTransposedTable(Trans_programData);
                ////DataTable Trans_Sub_programData = GenerateTransposed_SubTable(programData.Tables[7]);
                ////BaseLine_Clients_Cnt = programData.Tables[8].Rows.Count.ToString();
                ////RepPeriod_Clients_Cnt = programData.Tables[9].Rows.Count.ToString();
                ////Dynamic_RDLC();
                ////Dynamic_Sub_RDLC();

                ////foreach (DataRow dr in Trans_programData.Rows)
                ////{
                ////    if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                ////    {
                ////        dr.Delete();
                ////        break;
                ////    }
                ////}

                ////string Tmp_BM_Code = string.Empty;
                ////foreach (DataRow dr in Trans_programData.Rows)
                ////{
                ////    Tmp_BM_Code = dr["BM_Scale"].ToString();

                ////    foreach(MATDEFBMEntity Entity in BenchMark_List)
                ////    {
                ////        if (Tmp_BM_Code.Substring(3, (Tmp_BM_Code.Length - 3)) == Entity.Code)
                ////        {
                ////            dr["BM_Scale"] = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                ////            break;
                ////        }
                ////    }
                ////}

                ////foreach (DataRow dr in Trans_Sub_programData.Rows)
                ////{
                ////    if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                ////    {
                ////        dr.Delete();
                ////        break;
                ////    }
                ////}

                ////foreach (DataRow dr in Trans_Sub_programData.Rows)
                ////{
                ////    switch (dr["BM_Scale"].ToString())
                ////    {
                ////        case "a": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                ////        case "b": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                ////        case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                ////        case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                ////        case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                ////        case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                ////    }
                ////}
                ////Trans_Sub_programData.AcceptChanges();

                ////string Report_Name_1 = @"C:\Capreports\";

                //////CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Result_DataSet.Tables[5], Summary_table, Report_Name, "Result Table");
                ////CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(Trans_programData, Trans_Sub_programData, Rep_Name, "Result Table", Report_Name_1); //8, 5
                //////RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
                ////RDLC_Form.ShowDialog();
            }
        }

        private DataTable Calculate_TableRow_Totals(DataTable Trans_programData)
        {
            int Scale_Row_Total = 0, Test_Num = 2;
            foreach (DataRow dr in Trans_programData.Rows)
            {
                Scale_Row_Total = 0;
                foreach (DataColumn dc in Trans_programData.Columns)
                {

                    if (int.TryParse(dr[dc].ToString(), out Test_Num) && dc.ToString() != "BM_Scale")
                        Scale_Row_Total += int.Parse(dr[dc].ToString());
                }

                dr["Total"] = Scale_Row_Total;
            }
            return Trans_programData;
        }

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

            foreach (DG_ResTab_Entity Entity in DG_BaseTable_List)
            {
                XmlElement Field = xml.CreateElement("Field");
                Field.SetAttribute("Name", Entity.Column_Name);
                Fields.AppendChild(Field);

                XmlElement DataField = xml.CreateElement("DataField");
                DataField.InnerText = Entity.Column_Name;
                Field.AppendChild(DataField);
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


            double Total_Sel_TextBox_Height = 0.16667;
            string Tmp_Sel_Text = string.Empty;
            //for (int i = 0; i < 58; i++)
            for (int i = 0; i < 24; i++) //16
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
                    case 1: Tmp_Sel_Text = "Selected Report Parameters"; break;

                    case 3: Tmp_Sel_Text = "            Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

                    case 6: Tmp_Sel_Text = "            Matrix"; break;
                    case 7: Tmp_Sel_Text = " : " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString(); break; //((ListItem)Cmb_Matrix.SelectedItem).Text.ToString()
                    case 8: Tmp_Sel_Text = "            Scale"; break;
                    case 9: Tmp_Sel_Text = " : " + (Rb_All_Scales.Checked ? "All" : "Selected"); break;
                    case 10: Tmp_Sel_Text = "            Date Selection"; break;
                    case 11: Tmp_Sel_Text = " : " + (Rb_Asmt_FDate.Checked ? "First Assessment Date" : "Last Assessment Date"); break;

                    case 12: Tmp_Sel_Text = "            Baseline Range"; break;
                    case 13:
                        Tmp_Sel_Text = " : From " +
                                        CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_FDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                        + "    To " +
                                        CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_TDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        break;

                    case 14: Tmp_Sel_Text = "            Assessment Range"; break;
                    case 15:
                        Tmp_Sel_Text = " : From " +
                                        CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_F_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                        + "    To " +
                                        CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_T_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        break;


                    //case 10: Tmp_Sel_Text = "            Matrix"; break;
                    //case 11: Tmp_Sel_Text = " : " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString(); break; //((ListItem)Cmb_Matrix.SelectedItem).Text.ToString()
                    //case 12: Tmp_Sel_Text = "            Scale"; break;
                    //case 13: Tmp_Sel_Text = " : " + (Rb_All_Scales.Checked ? "All" : "Selected"); break;
                    //case 14: Tmp_Sel_Text = "            Date Selection"; break;
                    //case 15: Tmp_Sel_Text = " : " + (Rb_Asmt_FDate.Checked ? "First Assessment Date" : "Last Assessment Date"); break;

                    case 16: Tmp_Sel_Text = "            Caseworker"; break;
                    case 17: Tmp_Sel_Text = " : " + ((ListItem)Cmb_Worker.SelectedItem).Text.ToString(); break;
                    case 18: Tmp_Sel_Text = "            Site"; break;
                    case 19: Tmp_Sel_Text = " : " + ((ListItem)cmbSite.SelectedItem).Text.ToString(); break;
                    case 20: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            Enroll Status" : "  "); break;
                    case 21: Tmp_Sel_Text = (Cb_Enroll.Checked ? ": " + ((ListItem)cmbEnrlStatus.SelectedItem).Text.ToString() : "  "); break;
                    case 22: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            As of Date" : "  "); break;
                    case 23:
                        Tmp_Sel_Text = (Cb_Enroll.Checked ? ": From " +
                                       CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                       + "    To " +
                                       CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat) : "  "); break;
                    case 24: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            Program" : "  "); break;
                    case 25: Tmp_Sel_Text = (Cb_Enroll.Checked ? ": " + Txt_Program.Text.Trim() : "  "); break;

                    //case 16: Tmp_Sel_Text = "            Include All Members"; break;
                    //case 17: Tmp_Sel_Text = " : " + (Cb_Inc_Menbers.Checked ? "Yes" : "No"); break;
                    default: Tmp_Sel_Text = "  "; break;
                }


                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

                XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
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

                //Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                if (i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                {
                    if (i % 2 != 0)
                        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                }
                else
                    Total_Sel_TextBox_Height += 0.21855;


                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                Textbox1_Width.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
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
            Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            Sel_Rectangle_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
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


            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            //Yeswanth Rao Sindhe
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            XmlElement BtnTot_Rectangle = xml.CreateElement("Rectangle");
            BtnTot_Rectangle.SetAttribute("Name", "BtnTot_Rect");
            ReportItems.AppendChild(BtnTot_Rectangle);

            XmlElement BtnTot_REPItems = xml.CreateElement("ReportItems");
            BtnTot_Rectangle.AppendChild(BtnTot_REPItems);

            for (int i = 0; i < 5; i++)
            {
                XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
                Sel_Rect_Textbox1.SetAttribute("Name", "BtnTot_Textbox" + i.ToString());
                BtnTot_REPItems.AppendChild(Sel_Rect_Textbox1);

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
                    case 0: Tmp_Sel_Text = "A. Total number of persons receiving a Baseline Assessment "; break;
                    case 1: Tmp_Sel_Text = BaseLine_Clients_Cnt; break;
                    case 2: Tmp_Sel_Text = "B. Total number of persons assessed during reporting period "; break;
                    case 3: Tmp_Sel_Text = RepPeriod_Clients_Cnt; break;
                    case 4: Tmp_Sel_Text = "C. Total number of persons receiving a Follow-up Assessment "; break;
                    case 5: Tmp_Sel_Text = FolloUp_Clients_Cnt; break;
                    default: Tmp_Sel_Text = "  "; break;
                }

                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);


                //if (i == 4)
                //{
                //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                //    Return_Style_FontWeight.InnerText = "Bold";
                //    Textbox1_TextRun_Style.AppendChild(Return_Style_FontWeight);
                //}

                XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                //Textbox1_TextRun_Style_Color.InnerText = (i != 4 ? "DarkViolet" : "Black");
                Textbox1_TextRun_Style_Color.InnerText = ("DarkViolet");
                Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


                //XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                //Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);

                if (i == 1 || i == 3 || i==5)
                {
                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                    //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                    Cell_TextAlign.InnerText = Text_Align;
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Textbox1_Paragraph.AppendChild(Cell_Align);
                }


                //Total_Sel_TextBox_Height += 0.25;

                XmlElement Textbox1_Top = xml.CreateElement("Top");
                //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
                //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
                switch (i)
                {
                    case 0:
                    case 1: Textbox1_Top.InnerText = "0.09in"; break;
                    case 2:
                    case 3: Textbox1_Top.InnerText = "0.30855in"; break;
                    case 4: Textbox1_Top.InnerText = "0.5571in"; break;
                    case 5: Textbox1_Top.InnerText = "0.6571in"; break;
                }
                Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

                //XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                //Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                if (i == 0 || i == 2 || i == 4)
                    Textbox1_Left.InnerText = "0.07292in";
                else
                    Textbox1_Left.InnerText = "4.70611in";
                //Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                if (i == 0 || i == 2 || i == 4)
                    Textbox1_Width.InnerText = "4.62708in";
                else
                    Textbox1_Width.InnerText = "0.54389in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Width);


                XmlElement Textbox1_Style = xml.CreateElement("Style");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

                XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
                Textbox1_Style.AppendChild(Textbox1_Style_Border);

                XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
                //Textbox1_Style_Border_Style.InnerText = (i != 4 ? "None" : "Solid");
                Textbox1_Style_Border_Style.InnerText = ("None");
                Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

                //if (i == 4)
                //{
                //    XmlElement Textbox1_Style_BackColor = xml.CreateElement("BackgroundColor");
                //    Textbox1_Style_BackColor.InnerText = "LightGrey";
                //    Textbox1_Style.AppendChild(Textbox1_Style_BackColor);
                //}

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

            //XmlElement Break_After_BtnTot_Rect = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            //BtnTot_Rectangle.AppendChild(Break_After_BtnTot_Rect);

            //XmlElement Break_After_BtnTot_Rect_Location = xml.CreateElement("BreakLocation");
            //Break_After_BtnTot_Rect_Location.InnerText = "End";
            //Break_After_BtnTot_Rect.AppendChild(Break_After_BtnTot_Rect_Location);  // End Page break After Selectio Parameters

            XmlElement BtnTot_Rect_KeepTogether = xml.CreateElement("KeepTogether");
            BtnTot_Rect_KeepTogether.InnerText = "true";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_KeepTogether);

            XmlElement BtnTot_Rect_Top = xml.CreateElement("Top");
            BtnTot_Rect_Top.InnerText = (Total_Sel_TextBox_Height + 0.73).ToString() + "in"; //"0.2008in"; //"0.2408in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Top);

            XmlElement BtnTot_Rect_Left = xml.CreateElement("Left");
            BtnTot_Rect_Left.InnerText = "0.20417in"; //"0.277792in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Left);

            XmlElement BtnTot_Rect_Height = xml.CreateElement("Height");
            //BtnTot_Rect_Height.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//"10.33333in"; 11.4
            BtnTot_Rect_Height.InnerText = "0.5in";//"10.33333in"; 11.4
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Height);

            XmlElement BtnTot_Rect_Width = xml.CreateElement("Width");
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            BtnTot_Rect_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Width);

            XmlElement BtnTot_Rect_ZIndex = xml.CreateElement("ZIndex");
            BtnTot_Rect_ZIndex.InnerText = "1";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_ZIndex);

            XmlElement BtnTot_Rect_Style = xml.CreateElement("Style");
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Style);

            XmlElement BtnTot_Rect_Style_Border = xml.CreateElement("Border");
            BtnTot_Rect_Style.AppendChild(BtnTot_Rect_Style_Border);

            XmlElement BtnTot_Rect_Style_Border_Style = xml.CreateElement("Style");
            BtnTot_Rect_Style_Border_Style.InnerText = "None";//"Solid";
            BtnTot_Rect_Style_Border.AppendChild(BtnTot_Rect_Style_Border_Style);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            ///Creation Base table in RDLC through XML

            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            foreach (DG_ResTab_Entity Entity in DG_BaseTable_List)                      // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
                    //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
                }
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "2in";//"0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (DG_ResTab_Entity Entity in DG_BaseTable_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
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

                    Tmp_Disp_Column_Name = Entity.Disp_Name;


                    //Disp_Col_Substring_Len = 6;

                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                    Return_Value.InnerText = (Entity.Disp_Name == "BM_Scale" ? "Benchmark Level" : Entity.Disp_Name);                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);

                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Left";//"Center";
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
                    Border_Color.InnerText = "Black";//"LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

                    XmlElement Head_VerticalAlign = xml.CreateElement("VerticalAlign");
                    Head_VerticalAlign.InnerText = "Middle";
                    Cell_style.AppendChild(Head_VerticalAlign);

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

                    if (Entity.Disp_Name != "BM_Scale")
                    {
                        XmlElement Head_WritingMode = xml.CreateElement("WritingMode");
                        Head_WritingMode.InnerText = "Vertical";
                        Cell_style.AppendChild(Head_WritingMode);
                    }
                }
            }




            XmlElement TablixRow2 = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow2);

            XmlElement Row_Height2 = xml.CreateElement("Height");
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            
            char Tmp_Double_Codes = '"';
            foreach (DG_ResTab_Entity Entity in DG_BaseTable_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    XmlElement Textbox = xml.CreateElement("Textbox");
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
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black";// "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "Solid";  // Test Rao
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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

            for (int Loop = 0; Loop < DG_BaseTable_List.Count; Loop++)            // Dynamic based on Display Columns in 3/6 
            {
                XmlElement TablixMember = xml.CreateElement("TablixMember");
                Tablix_Col_Members.AppendChild(TablixMember);
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

            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            //Tablix.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "End";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);
            Total_Sel_TextBox_Height = (Total_Sel_TextBox_Height + 2);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20517in";//"0.20417in";
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

          



            //   Subreport
            ////////if (Summary_Sw)
            ////////{
            ////////    // Summary Sub Report 
            ////////}

            if (true) // Sindhe Rao
            {
                // Summary Sub Report 
                XmlElement Subreport = xml.CreateElement("Subreport");
                Subreport.SetAttribute("Name", "SummaryReport");  //99999999999999999
                ReportItems.AppendChild(Subreport);

                XmlElement SubRep_Name = xml.CreateElement("ReportName");
                SubRep_Name.InnerText = (Rep_Name + "SubReport");
                Subreport.AppendChild(SubRep_Name);

                XmlElement SubRep_Omit_Border = xml.CreateElement("OmitBorderOnPageBreak");
                SubRep_Omit_Border.InnerText = "true";
                Subreport.AppendChild(SubRep_Omit_Border);

                XmlElement SubRep_Top = xml.CreateElement("Top");
                //SubRep_Top.InnerText = (Total_Sel_TextBox_Height + 1.5).ToString() + "in";
                SubRep_Top.InnerText = (Total_Sel_TextBox_Height + 1.35).ToString() + "in";
                //SubRep_Top.InnerText = "14.0in";
                Subreport.AppendChild(SubRep_Top);
                Total_Sel_TextBox_Height += 1.35;

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

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>> aaaaaaaaaaa

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

            //Rep_Header_Title = " Test Report";
            if (true && !string.IsNullOrEmpty(Rep_Header_Title.Trim())) //Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                PageHeader_Height.InnerText = "0.51958in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (true) // 
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

                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);

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
                    Header_TextBox_Top.InnerText = "0.24792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
                    Header_TextBox_Left.InnerText = "2.29861in";// "1.42361in";
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    Header_TextBox_Height.InnerText = "0.30208in";
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

            if (false) //Include_Footer
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

                if (true) //Include_Footer_PageCnt
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
                    Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = "1.65625in";
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
            if (false) //(Rb_A4_Port.Checked)
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

            try  // Generate Rep Name
            {
                xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            Console.ReadLine();
        }

        private void Dynamic_RDLC1()
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

            foreach (DG_ResTab_Entity Entity in DG_Table_List)
            {
                XmlElement Field = xml.CreateElement("Field");
                Field.SetAttribute("Name", Entity.Column_Name);
                Fields.AppendChild(Field);

                XmlElement DataField = xml.CreateElement("DataField");
                DataField.InnerText = Entity.Column_Name;
                Field.AppendChild(DataField);
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


            double Total_Sel_TextBox_Height = 0.16667;
            string Tmp_Sel_Text = string.Empty;
            //for (int i = 0; i < 58; i++)
            for (int i = 0; i < 24; i++) //16
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
                    case 1: Tmp_Sel_Text = "Selected Report Parameters"; break;

                    case 3: Tmp_Sel_Text = "            Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

                    case 6: Tmp_Sel_Text = "            Baseline Range"; break;
                    case 7: Tmp_Sel_Text = " : From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_FDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_TDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        break;

                    case 8: Tmp_Sel_Text = "            Assessment Range"; break;
                    case 9: Tmp_Sel_Text = " : From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_F_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_T_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        break;


                    case 10: Tmp_Sel_Text = "            Matrix"; break;
                    case 11: Tmp_Sel_Text = " : " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString(); break; //((ListItem)Cmb_Matrix.SelectedItem).Text.ToString()
                    case 12: Tmp_Sel_Text = "            Scale"; break;
                    case 13: Tmp_Sel_Text = " : " + (Rb_All_Scales.Checked ? "All" : "Selected"); break;
                    case 14: Tmp_Sel_Text = "            Date Selection"; break;
                    case 15: Tmp_Sel_Text = " : " + (Rb_Asmt_FDate.Checked ? "First Assessment Date" : "Last Assessment Date"); break;

                    case 16: Tmp_Sel_Text = "            Caseworker"; break;
                    case 17: Tmp_Sel_Text = " : " + ((ListItem)Cmb_Worker.SelectedItem).Text.ToString(); break;
                    case 18: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            Enroll Status" : "  "); break;
                    case 19: Tmp_Sel_Text = (Cb_Enroll.Checked ? ": " + ((ListItem)cmbEnrlStatus.SelectedItem).Text.ToString() : "  "); break;
                    case 20: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            As of Date" : "  "); break;
                    case 21: Tmp_Sel_Text = (Cb_Enroll.Checked ? ": From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat) : "  "); break;
                    case 22: Tmp_Sel_Text = (Cb_Enroll.Checked ? "            Program" : "  "); break;
                    case 23: Tmp_Sel_Text = (Cb_Enroll.Checked ? ": " + Txt_Program.Text.Trim() : "  "); break;

                    //case 16: Tmp_Sel_Text = "            Include All Members"; break;
                    //case 17: Tmp_Sel_Text = " : " + (Cb_Inc_Menbers.Checked ? "Yes" : "No"); break;
                    default: Tmp_Sel_Text = "  "; break;
                }


                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

                XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
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

                //Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                if (i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                {
                    if (i % 2 != 0)
                        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                }
                else
                    Total_Sel_TextBox_Height += 0.21855;


                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                Textbox1_Width.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
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
            Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            Sel_Rectangle_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
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

            foreach (DG_ResTab_Entity Entity in DG_Table_List)                      // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
                    //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
                }
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "2in";//"0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (DG_ResTab_Entity Entity in DG_Table_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
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

                    Tmp_Disp_Column_Name = Entity.Disp_Name;


                    //Disp_Col_Substring_Len = 6;

                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                    Return_Value.InnerText = (Entity.Disp_Name == "BM_Scale" ? "Benchmark Level" : Entity.Disp_Name);                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);

                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Left";//"Center";
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
                    Border_Color.InnerText = "Black";//"LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

                    XmlElement Head_VerticalAlign = xml.CreateElement("VerticalAlign");
                    Head_VerticalAlign.InnerText = "Middle";
                    Cell_style.AppendChild(Head_VerticalAlign);

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

                    if (Entity.Disp_Name != "BM_Scale")
                    {
                        XmlElement Head_WritingMode = xml.CreateElement("WritingMode");
                        Head_WritingMode.InnerText = "Vertical";
                        Cell_style.AppendChild(Head_WritingMode);
                    }
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
            foreach (DG_ResTab_Entity Entity in DG_Table_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    XmlElement Textbox = xml.CreateElement("Textbox");
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
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black";// "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "Solid";  // Test Rao
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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

            for (int Loop = 0; Loop < DG_Table_List.Count; Loop++)            // Dynamic based on Display Columns in 3/6 
            {
                XmlElement TablixMember = xml.CreateElement("TablixMember");
                Tablix_Col_Members.AppendChild(TablixMember);
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

            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            //Tablix.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "End";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);
            Total_Sel_TextBox_Height = (Total_Sel_TextBox_Height + 2);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20517in";//"0.20417in";
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

            //Yeswanth Rao Sindhe
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            XmlElement BtnTot_Rectangle = xml.CreateElement("Rectangle");
            BtnTot_Rectangle.SetAttribute("Name", "BtnTot_Rect");
            ReportItems.AppendChild(BtnTot_Rectangle);

            XmlElement BtnTot_REPItems = xml.CreateElement("ReportItems");
            BtnTot_Rectangle.AppendChild(BtnTot_REPItems);

            for (int i = 0; i < 4; i++)
            {
                XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
                Sel_Rect_Textbox1.SetAttribute("Name", "BtnTot_Textbox" + i.ToString());
                BtnTot_REPItems.AppendChild(Sel_Rect_Textbox1);

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
                    case 0: Tmp_Sel_Text = "b. TOTAL # of persons receiving a baseline assessment "; break;
                    case 1: Tmp_Sel_Text = BaseLine_Clients_Cnt; break;
                    case 2: Tmp_Sel_Text = "c. TOTAL # of persons receiving a follow-up assessment "; break;
                    case 3: Tmp_Sel_Text = FolloUp_Clients_Cnt; break;
                    case 4: Tmp_Sel_Text = "2. Customer progress during reporting period "; break;
                    default: Tmp_Sel_Text = "  "; break;
                }

                Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


                XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);


                if (i == 4)
                {
                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "Bold";
                    Textbox1_TextRun_Style.AppendChild(Return_Style_FontWeight);
                }

                XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                Textbox1_TextRun_Style_Color.InnerText = (i != 4 ? "DarkViolet" : "Black");
                Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


                //XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                //Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);

                if (i == 1 || i == 3)
                {
                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                    //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                    Cell_TextAlign.InnerText = Text_Align;
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Textbox1_Paragraph.AppendChild(Cell_Align);
                }


                //Total_Sel_TextBox_Height += 0.25;

                XmlElement Textbox1_Top = xml.CreateElement("Top");
                //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
                //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
                switch (i)
                {
                    case 0:
                    case 1: Textbox1_Top.InnerText = "0.09in"; break;
                    case 2:
                    case 3: Textbox1_Top.InnerText = "0.30855in"; break;
                    case 4: Textbox1_Top.InnerText = "0.5571in"; break;
                }
                Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

                //XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                //Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                if (i == 0 || i == 2 || i == 4)
                    Textbox1_Left.InnerText = "0.07292in";
                else
                    Textbox1_Left.InnerText = "4.70611in";
                //Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                if (i == 0 || i == 2 || i == 4)
                    Textbox1_Width.InnerText = "4.62708in";
                else
                    Textbox1_Width.InnerText = "0.54389in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Width);


                XmlElement Textbox1_Style = xml.CreateElement("Style");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

                XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
                Textbox1_Style.AppendChild(Textbox1_Style_Border);

                XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
                Textbox1_Style_Border_Style.InnerText = (i != 4 ? "None" : "Solid");
                Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

                if (i == 4)
                {
                    XmlElement Textbox1_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Textbox1_Style_BackColor.InnerText = "LightGrey";
                    Textbox1_Style.AppendChild(Textbox1_Style_BackColor);
                }

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

            //XmlElement Break_After_BtnTot_Rect = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            //BtnTot_Rectangle.AppendChild(Break_After_BtnTot_Rect);

            //XmlElement Break_After_BtnTot_Rect_Location = xml.CreateElement("BreakLocation");
            //Break_After_BtnTot_Rect_Location.InnerText = "End";
            //Break_After_BtnTot_Rect.AppendChild(Break_After_BtnTot_Rect_Location);  // End Page break After Selectio Parameters

            XmlElement BtnTot_Rect_KeepTogether = xml.CreateElement("KeepTogether");
            BtnTot_Rect_KeepTogether.InnerText = "true";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_KeepTogether);

            XmlElement BtnTot_Rect_Top = xml.CreateElement("Top");
            BtnTot_Rect_Top.InnerText = (Total_Sel_TextBox_Height + 0.73).ToString() + "in"; //"0.2008in"; //"0.2408in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Top);

            XmlElement BtnTot_Rect_Left = xml.CreateElement("Left");
            BtnTot_Rect_Left.InnerText = "0.20417in"; //"0.277792in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Left);

            XmlElement BtnTot_Rect_Height = xml.CreateElement("Height");
            //BtnTot_Rect_Height.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//"10.33333in"; 11.4
            BtnTot_Rect_Height.InnerText = "0.5in";//"10.33333in"; 11.4
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Height);

            XmlElement BtnTot_Rect_Width = xml.CreateElement("Width");
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            BtnTot_Rect_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Width);

            XmlElement BtnTot_Rect_ZIndex = xml.CreateElement("ZIndex");
            BtnTot_Rect_ZIndex.InnerText = "1";
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_ZIndex);

            XmlElement BtnTot_Rect_Style = xml.CreateElement("Style");
            BtnTot_Rectangle.AppendChild(BtnTot_Rect_Style);

            XmlElement BtnTot_Rect_Style_Border = xml.CreateElement("Border");
            BtnTot_Rect_Style.AppendChild(BtnTot_Rect_Style_Border);

            XmlElement BtnTot_Rect_Style_Border_Style = xml.CreateElement("Style");
            BtnTot_Rect_Style_Border_Style.InnerText = "None";//"Solid";
            BtnTot_Rect_Style_Border.AppendChild(BtnTot_Rect_Style_Border_Style);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





            //   Subreport
            ////////if (Summary_Sw)
            ////////{
            ////////    // Summary Sub Report 
            ////////}

            if (true) // Sindhe Rao
            {
                // Summary Sub Report 
                XmlElement Subreport = xml.CreateElement("Subreport");
                Subreport.SetAttribute("Name", "SummaryReport");  //99999999999999999
                ReportItems.AppendChild(Subreport);

                XmlElement SubRep_Name = xml.CreateElement("ReportName");
                SubRep_Name.InnerText = (Rep_Name + "SubReport");
                Subreport.AppendChild(SubRep_Name);

                XmlElement SubRep_Omit_Border = xml.CreateElement("OmitBorderOnPageBreak");
                SubRep_Omit_Border.InnerText = "true";
                Subreport.AppendChild(SubRep_Omit_Border);

                XmlElement SubRep_Top = xml.CreateElement("Top");
                //SubRep_Top.InnerText = (Total_Sel_TextBox_Height + 1.5).ToString() + "in";
                SubRep_Top.InnerText = (Total_Sel_TextBox_Height + 1.35).ToString() + "in";
                //SubRep_Top.InnerText = "14.0in";
                Subreport.AppendChild(SubRep_Top);
                Total_Sel_TextBox_Height += 1.35;

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

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>> aaaaaaaaaaa

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

            //Rep_Header_Title = " Test Report";
            if (true && !string.IsNullOrEmpty(Rep_Header_Title.Trim())) //Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                PageHeader_Height.InnerText = "0.51958in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (true) // 
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

                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);

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
                    Header_TextBox_Top.InnerText = "0.24792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
                    Header_TextBox_Left.InnerText = "2.29861in";// "1.42361in";
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    Header_TextBox_Height.InnerText = "0.30208in";
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

            if (false) //Include_Footer
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

                if (true) //Include_Footer_PageCnt
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
                    Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = "1.65625in";
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
            if (false) //(Rb_A4_Port.Checked)
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

            try  // Generate Rep Name
            {
                xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            Console.ReadLine();
        }

        #region ExcelReportFormat


        string propReportPath = "";
        private void OnExcel_Report1(DataTable table, DataTable Scale_Table)
        {
            string propReportPath = _model.lookupDataAccess.GetReportPath();
            string PdfName = "Pdf File";
            PdfName = "MATB0002_Details.xls";
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            string data = null;

            ExcelDocument xlWorkSheet = new ExcelDocument();

            xlWorkSheet.ColumnWidth(0, 0);
            xlWorkSheet.ColumnWidth(1, 70);
            xlWorkSheet.ColumnWidth(2, 70);
            xlWorkSheet.ColumnWidth(3, 70);
            xlWorkSheet.ColumnWidth(4, 70);
            xlWorkSheet.ColumnWidth(5, 90);
            xlWorkSheet.ColumnWidth(6, 200);
            xlWorkSheet.ColumnWidth(7, 200);
            xlWorkSheet.ColumnWidth(8, 100);
            xlWorkSheet.ColumnWidth(9, 65);
            xlWorkSheet.ColumnWidth(10, 120);
            xlWorkSheet.ColumnWidth(11, 100);
            xlWorkSheet.ColumnWidth(12, 65);
            xlWorkSheet.ColumnWidth(13, 120);
            //xlWorkSheet.ColumnWidth(12, 250);
            int Row = 13;
            int excelcolumn = 0;

            try
            {
                if (table.Rows.Count > 0)
                {
                    excelcolumn = excelcolumn + 1;


                    xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Left;
                    xlWorkSheet.WriteCell(excelcolumn, 1, "Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim());

                    excelcolumn = excelcolumn + 1;
                    int HeaderColumn = excelcolumn;
                    xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Left;
                    xlWorkSheet.WriteCell(excelcolumn, 8, "Baseline   ");

                    xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Left;
                    xlWorkSheet.WriteCell(excelcolumn, 11, "Assessment 1   ");


                    excelcolumn = excelcolumn + 1;
                    string[] HeaderSeq4 = { "Agency", "Dept", "Program", "Year", "AppNo", "Client Name", "Scale", "Date", "Points", "Benchmark", "Date", "Points", "Benchmark" };
                    for (int i = 0; i < HeaderSeq4.Length; ++i)
                    {
                        xlWorkSheet[excelcolumn, (i + 1)].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        xlWorkSheet[excelcolumn, (i + 1)].Alignment = Alignment.Centered;
                        xlWorkSheet.WriteCell(excelcolumn, (i + 1), HeaderSeq4[i]);
                    }

                    bool first = true; int length = 0;
                    foreach (DataRow dr in table.Rows)
                    {
                        excelcolumn = excelcolumn + 1;

                        xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 1, dr["Det_Agy"].ToString().Trim());


                        xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 2, dr["Det_Dept"].ToString().Trim());

                        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 3, dr["Det_Prog"].ToString().Trim());

                        xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 4, dr["Det_Year"].ToString().Trim());

                        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Left;
                        xlWorkSheet.WriteCell(excelcolumn, 5, dr["Det_App"].ToString().Trim());

                        xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Left;
                        xlWorkSheet.WriteCell(excelcolumn, 6, dr["Client_Name"].ToString().Trim());

                        //xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Left;
                        //xlWorkSheet.WriteCell(excelcolumn, 7, ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim());




                        string Scale_Desc = dr["Det_Scale_Code"].ToString().Trim();
                        if (Scale_Table.Rows.Count > 0)
                        {
                            foreach (DataRow drscale in Scale_Table.Rows)
                            {
                                if (dr["Det_Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
                                {
                                    Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
                                }
                            }
                        }

                        xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Left;
                        xlWorkSheet.WriteCell(excelcolumn, 7, Scale_Desc);

                        //xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Right;
                        //xlWorkSheet.WriteCell(excelcolumn, 9, dr["Det_POINTS"].ToString().Trim());

                        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Left;
                        xlWorkSheet.WriteCell(excelcolumn, 8, LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()));

                        xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 9, dr["Det_POINTS"].ToString().Trim());

                        string BM_Desc = dr["Det_Asmt_BM_Code"].ToString().Trim();
                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                        {
                            if (dr["Det_Asmt_BM_Code"].ToString().Trim() == Entity.Code)
                            {
                                BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                break;
                            }
                        }

                        xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Left;
                        xlWorkSheet.WriteCell(excelcolumn, 10, BM_Desc);

                        string[] assdates = dr["Ass_DateList"].ToString().Trim().Split('?');
                        string[] datelist = new string[3];
                        if (assdates.Length > 0)
                        {
                            first = true; int k = 0;
                            for (int i = 0; i < assdates.Length; i++)
                            {
                                datelist = assdates[0].Split(',');

                                datelist[0] = datelist[0].Replace("'", string.Empty);
                                datelist[2] = datelist[2].Replace("'", string.Empty);

                                if (i > length)
                                {
                                    Row = Row + 1;
                                    xlWorkSheet.ColumnWidth(Row, 100);
                                    xlWorkSheet[HeaderColumn, Row].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[HeaderColumn, Row].Alignment = Alignment.Left;
                                    xlWorkSheet.WriteCell(HeaderColumn, Row, "Assessment " + (i + 1));

                                    xlWorkSheet[HeaderColumn + 1, Row].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[HeaderColumn + 1, Row].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(HeaderColumn + 1, Row, "Date   ");

                                    //xlWorkSheet[excelcolumn, Row].Alignment = Alignment.Left;
                                    //xlWorkSheet.WriteCell(excelcolumn, Row, datelist[0].ToString().Trim());

                                    Row = Row + 1;
                                    xlWorkSheet.ColumnWidth(Row, 65);
                                    xlWorkSheet[HeaderColumn + 1, Row].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[HeaderColumn + 1, Row].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(HeaderColumn + 1, Row, "Points   ");

                                    //xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Left;
                                    //xlWorkSheet.WriteCell(excelcolumn, 12, datelist[1].ToString().Trim());

                                    Row = Row + 1;
                                    xlWorkSheet.ColumnWidth(Row, 120);
                                    xlWorkSheet[HeaderColumn + 1, Row].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                    xlWorkSheet[HeaderColumn + 1, Row].Alignment = Alignment.Centered;
                                    xlWorkSheet.WriteCell(HeaderColumn + 1, Row, "Benchmark   ");

                                    //foreach (MATDEFBMEntity Entity in BenchMark_List)
                                    //{
                                    //    if (datelist[2].ToString().Trim() == Entity.Code)
                                    //    {
                                    //        BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                    //        break;
                                    //    }
                                    //}
                                    //xlWorkSheet[excelcolumn, Row].Alignment = Alignment.Left;
                                    //xlWorkSheet.WriteCell(excelcolumn, Row, datelist[2].ToString().Trim());

                                    length++;
                                }
                                if (first)
                                {
                                    xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Left;
                                    xlWorkSheet.WriteCell(excelcolumn, 11, LookupDataAccess.Getdate1(datelist[0].ToString().Trim()));

                                    xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(excelcolumn, 12, datelist[1].ToString().Trim());

                                    foreach (MATDEFBMEntity Entity in BenchMark_List)
                                    {
                                        if (datelist[2].ToString().Trim() == Entity.Code)
                                        {
                                            BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                            break;
                                        }
                                    }
                                    xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Left;
                                    xlWorkSheet.WriteCell(excelcolumn, 13, BM_Desc);

                                    first = false; k = 13;
                                }
                                else
                                {
                                    k++;
                                    xlWorkSheet[excelcolumn, k].Alignment = Alignment.Left;
                                    xlWorkSheet.WriteCell(excelcolumn, k, LookupDataAccess.Getdate1(datelist[0].ToString().Trim()));

                                    k++;
                                    xlWorkSheet[excelcolumn, k].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(excelcolumn, k, datelist[1].ToString().Trim());

                                    foreach (MATDEFBMEntity Entity in BenchMark_List)
                                    {
                                        if (datelist[2].ToString().Trim() == Entity.Code)
                                        {
                                            BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                            break;
                                        }
                                    }
                                    k++;
                                    xlWorkSheet[excelcolumn, k].Alignment = Alignment.Left;
                                    xlWorkSheet.WriteCell(excelcolumn, k, BM_Desc);
                                }
                            }
                        }
                        //if (first)
                        //{
                        //    xlWorkSheet.ColumnWidth(11, 250);
                        //    first = false;
                        //}
                        //xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Left;
                        //xlWorkSheet.WriteCell(excelcolumn, 11, dr["Ass_DateList"].ToString().Trim());
                    }
                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    xlWorkSheet.Save(stream);
                    stream.Close();

                }
            }
            catch (Exception ex) { }




        }

        string Random_Filename = null; string PdfName = "Pdf File";
        private void OnExcel_Report1(DataTable table, DataTable Scale_Table, DataTable AssTable, DataTable DetailTable)
        {
            string propReportPath = _model.lookupDataAccess.GetReportPath();
            string PdfName = "Pdf File";
            PdfName = "MATB0002_Details.xls";
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName;
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName = PdfName;

            List<CaseHierarchyEntity> propCaseHieEntity = _model.AdhocData.Browse_CASEHIE("**", "**", "**", BaseForm.UserID, BaseForm.BaseAdminAgency);


            string data = null;
            Workbook book = new Workbook();

            Worksheet sheet = book.Worksheets.Add("Sheet1");
            sheet.Table.Columns.Add(new WorksheetColumn(70));
            sheet.Table.Columns.Add(new WorksheetColumn(60));
            sheet.Table.Columns.Add(new WorksheetColumn(70));
            sheet.Table.Columns.Add(new WorksheetColumn(70));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(200));
            sheet.Table.Columns.Add(new WorksheetColumn(100));
            sheet.Table.Columns.Add(new WorksheetColumn(130));
            if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                sheet.Table.Columns.Add(new WorksheetColumn(130));
            //if (Cb_Enroll.Checked)
            //    sheet.Table.Columns.Add(new WorksheetColumn(200));
            if (Cb_Enroll.Checked && (!string.IsNullOrEmpty(Txt_Program.Text.Trim())))
            {
                string[] Programs = Txt_Program.Text.Split(',');
                if (Programs.Length > 0)
                {
                    for (int i = 0; i < Programs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Programs[i].Trim()))
                            sheet.Table.Columns.Add(new WorksheetColumn(200));
                    }
                }
            }

            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

            WorksheetStyle style2 = book.Styles.Add("CellStyle");
            style2.Font.FontName = "Tahoma";
            style2.Font.Size = 10;
            style2.Font.Color = "Blue";
            style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;


            //style = book.Styles.Add("Default");
            //style.Font.FontName = "Tahoma";
            //style.Font.Size = 10;

            try
            {
                if (table.Rows.Count > 0)
                {

                    WorksheetRow row = sheet.Table.Rows.Add();

                    WorksheetCell cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "HeaderStyle");
                    //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                    if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                        cell.MergeAcross = 10;
                    else if (Cb_Enroll.Checked)
                        cell.MergeAcross = 9;
                    else
                        cell.MergeAcross = 8;


                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("Agency", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Dept", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Year", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("AppNo", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Client Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Base Range", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Assessment Range", "HeaderStyle"));
                    if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                        row.Cells.Add(new WorksheetCell("Status", "HeaderStyle"));
                    //if (Cb_Enroll.Checked)
                    //    row.Cells.Add(new WorksheetCell("Program", "HeaderStyle"));
                    if (Cb_Enroll.Checked && (!string.IsNullOrEmpty(Txt_Program.Text.Trim())))
                    {
                        string[] Programs = Txt_Program.Text.Split(',');
                        if (Programs.Length > 0)
                        {
                            for (int i = 0; i < Programs.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Programs[i].Trim()))
                                {
                                    CaseHierarchyEntity HieProgram = propCaseHieEntity.Find(u => u.Code.Equals(Programs[i].Trim()));
                                    if (HieProgram != null)
                                        row.Cells.Add(new WorksheetCell(HieProgram.HierarchyName.Trim(), "HeaderStyle"));
                                }
                            }
                        }
                    }


                    //excelcolumn = excelcolumn + 1;
                    //string[] HeaderSeq4 = { "Agency", "Dept", "Program", "Year", "AppNo", "Client Name", "Base Range", "Assessment Range" };
                    //for (int i = 0; i < HeaderSeq4.Length; ++i)
                    //{
                    //    xlWorkSheet[excelcolumn, (i + 1)].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //    xlWorkSheet[excelcolumn, (i + 1)].Alignment = Alignment.Centered;
                    //    xlWorkSheet.WriteCell(excelcolumn, (i + 1), HeaderSeq4[i]);
                    //}

                    bool first = true; int length = 0;
                    foreach (DataRow dr in table.Rows)
                    {
                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(dr["Det_Agy"].ToString().Trim());
                        row.Cells.Add(dr["Det_Dept"].ToString().Trim());
                        row.Cells.Add(dr["Det_Prog"].ToString().Trim());
                        row.Cells.Add(dr["Det_Year"].ToString().Trim());
                        row.Cells.Add(dr["Det_App"].ToString().Trim());
                        row.Cells.Add(dr["Client_Name"].ToString().Trim());
                        row.Cells.Add(dr["Det_BaseRange"].ToString().Trim());
                        row.Cells.Add(dr["Det_AssmentRange"].ToString().Trim());
                        if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                        {
                            string Status = string.Empty;
                            if (dr["ENRL_STATUS"].ToString().Trim() == "E")
                                Status = "Enroll";
                            else if (dr["ENRL_STATUS"].ToString().Trim() == "W")
                                Status = "Withdrawn";

                            row.Cells.Add(Status.Trim());
                        }
                        if (Cb_Enroll.Checked && !string.IsNullOrEmpty(Txt_Program.Text.Trim()))
                        {
                            string EnrlProgs = dr["ENRL_FUND_HIE"].ToString().Trim();

                            string[] Programs = Txt_Program.Text.Split(',');
                            if (Programs.Length > 0)
                            {
                                for (int i = 0; i < Programs.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Programs[i].Trim()))
                                    {
                                        if (EnrlProgs.Contains(Programs[i].Trim()))
                                            row.Cells.Add("Y");
                                        else row.Cells.Add(" ");
                                    }
                                    //CaseHierarchyEntity HieProgram = propCaseHieEntity.Find(u => u.Code.Equals(Programs[i].Trim()));
                                    //if (HieProgram != null)
                                    //    row.Cells.Add(new WorksheetCell(HieProgram.HierarchyName.Trim(), "HeaderStyle"));
                                }
                            }
                        }

                    }
                }
                if (AssTable.Rows.Count > 0)
                {


                    Worksheet sheet1 = book.Worksheets.Add("Sheet2");

                    //WorksheetColumn Column = sheet1.Table.Columns.Add();

                    sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    sheet1.Table.Columns.Add(new WorksheetColumn(60));
                    sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    sheet1.Table.Columns.Add(new WorksheetColumn(80));
                    sheet1.Table.Columns.Add(new WorksheetColumn(220));
                    sheet1.Table.Columns.Add(new WorksheetColumn(200));
                    sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(120));
                    sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(120));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    sheet1.Table.Columns.Add(new WorksheetColumn(100));

                    //int LengthCol = 0; int k = 0;
                    //foreach (DataRow dr in AssTable.Rows)
                    //{
                    //    string[] assdates = dr["Ass_DateList"].ToString().Trim().Split('?');
                    //    string[] datelist = new string[3];
                    //    if (assdates.Length > 0)
                    //    {
                    //        for (int i = 0; i < assdates.Length; i++)
                    //        {
                    //            datelist = assdates[0].Split(',');

                    //            datelist[0] = datelist[0].Replace("'", string.Empty);
                    //            datelist[2] = datelist[2].Replace("'", string.Empty);

                    //            if (i > LengthCol)
                    //            {
                    //                sheet1.Table.Columns.Add(new WorksheetColumn(70));
                    //                sheet1.Table.Columns.Add(new WorksheetColumn(50));
                    //                sheet1.Table.Columns.Add(new WorksheetColumn(120));

                    //                k++;
                    //                LengthCol++;
                    //            }
                    //        }

                    //    }
                    //}

                    WorksheetRow row = sheet1.Table.Rows.Add();

                    WorksheetCell cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "HeaderStyle1");
                    cell.MergeAcross = 19;

                    row = sheet1.Table.Rows.Add();

                    for (int i = 1; i < 8; i++)
                    {
                        row.Cells.Add("");
                    }

                    WorksheetCell bas1e = row.Cells.Add("Baseline", DataType.String, "HeaderStyle");
                    bas1e.MergeAcross = 2;

                    WorksheetCell Assw1 = row.Cells.Add("Assessment", DataType.String, "HeaderStyle");
                    Assw1.MergeAcross = 2;

                    //if (k > 0)
                    //{
                    //    for (int i = 1; i <= k; i++)
                    //    {
                    //        WorksheetCell Assw2 = row.Cells.Add("Assessment " + (i + 1), DataType.String, "HeaderStyle");
                    //        Assw2.MergeAcross = 2;
                    //    }
                    //}

                    row = sheet1.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("Agency", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Dept", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Year", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("AppNo", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Client Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Scale", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Date", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Points", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Benchmark", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Date", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Points", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Benchmark", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("a", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("b", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("c", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("d", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("e", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("f", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Incrisis", "HeaderStyle"));
                    //if (k > 0)
                    //{
                    //    for (int i = 1; i <= k; i++)
                    //    {
                    //        row.Cells.Add(new WorksheetCell("Date", "HeaderStyle"));
                    //        row.Cells.Add(new WorksheetCell("Points", "HeaderStyle"));
                    //        row.Cells.Add(new WorksheetCell("Benchmark", "HeaderStyle"));
                    //    }
                    //}

                    bool first = true; int length = 0;
                    foreach (DataRow dr in AssTable.Rows)
                    {
                        row = sheet1.Table.Rows.Add();

                        row.Cells.Add(dr["Det_Agy"].ToString().Trim());
                        row.Cells.Add(dr["Det_Dept"].ToString().Trim());
                        row.Cells.Add(dr["Det_Prog"].ToString().Trim());
                        row.Cells.Add(dr["Det_Year"].ToString().Trim());
                        row.Cells.Add(dr["Det_App"].ToString().Trim());
                        row.Cells.Add(dr["Client_Name"].ToString().Trim());

                        string Scale_Desc = dr["Det_Scale_Code"].ToString().Trim();
                        if (Scale_Table.Rows.Count > 0)
                        {
                            foreach (DataRow drscale in Scale_Table.Rows)
                            {
                                if (dr["Det_Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
                                {
                                    Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
                                }
                            }
                        }

                        row.Cells.Add(Scale_Desc.Trim());


                        if (!string.IsNullOrEmpty(Asmt_T_Date.Text.Trim()))
                        {
                            if (Convert.ToDateTime(dr["BaseLine_Date"].ToString().Trim()) >= Convert.ToDateTime(Asmt_F_Date.Text.Trim()) && Convert.ToDateTime(dr["BaseLine_Date"].ToString().Trim()) <= Convert.ToDateTime(Asmt_T_Date.Text.Trim()))
                                row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "CellStyle");
                            else
                                row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()));
                        }
                        else
                            row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()));

                        string BM_Desc = dr["Det_Asmt_BM_Code"].ToString().Trim();
                        if (!string.IsNullOrEmpty(dr["BaseLine_Date"].ToString().Trim()))
                        {
                            if(dr["Det_BYPASS"].ToString().Trim()=="N")
                                row.Cells.Add(dr["Det_POINTS"].ToString().Trim());
                            else
                                row.Cells.Add("");

                            foreach (MATDEFBMEntity Entity in BenchMark_List)
                            {
                                if (dr["Det_Asmt_BM_Code"].ToString().Trim() == Entity.Code)
                                {
                                    BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                    break;
                                }
                            }
                            if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                row.Cells.Add(BM_Desc.Trim());
                            else row.Cells.Add("Scale Bypassed");
                        }
                        else
                        {
                            row.Cells.Add("");
                            row.Cells.Add("");
                        }

                        string[] assdates = dr["Ass_DateList"].ToString().Trim().Split(',');
                        string[] datelist = new string[3];
                        if (assdates.Length > 0)
                        {
                            if (assdates.Length > 1)
                            {
                                row.Cells.Add(LookupDataAccess.Getdate1(assdates[0].ToString().Trim()));
                                

                                if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(assdates[1].ToString().Trim());
                                else
                                    row.Cells.Add("");

                                foreach (MATDEFBMEntity Entity in BenchMark_List)
                                {
                                    if (assdates[2].ToString().Trim() == Entity.Code)
                                    {
                                        BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                        break;
                                    }
                                }
                                if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(BM_Desc.Trim());
                                else row.Cells.Add("Scale Bypassed");
                                //row.Cells.Add(BM_Desc.Trim());
                            }
                            else
                            {
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                            }

                            //first = true; //int k = 0;
                            //for (int i = 0; i < assdates.Length; i++)
                            //{
                            //    datelist = assdates[0].Split(',');

                            //    datelist[0] = datelist[0].Replace("'", string.Empty);
                            //    datelist[2] = datelist[2].Replace("'", string.Empty);

                            //    if (i > length)
                            //    {
                            //        //sheet1.Table.Columns.Add(new WorksheetColumn(65));
                            //        //sheet1.Table.Columns.Add(new WorksheetColumn(120));
                            //        //sheet1.Table.Columns.Add(new WorksheetColumn(250));

                            //        ////row.Index = 2; Column.Index = 14;
                            //        //WorksheetCell Assw2 = row.Cells.Add("Assessment " + (i + 1), DataType.String, "HeaderStyle");
                            //        //Assw2.MergeAcross = 2;

                            //        ////row.Index = 3; Column.Index = 14;
                            //        //row.Cells.Add(new WorksheetCell("Date", "HeaderStyle"));
                            //        //row.Cells.Add(new WorksheetCell("Points", "HeaderStyle"));
                            //        //row.Cells.Add(new WorksheetCell("Benchmark", "HeaderStyle"));

                            //        length++;
                            //    }
                            //    if (first)
                            //    {
                            //        row.Cells.Add(LookupDataAccess.Getdate1(datelist[0].ToString().Trim()));
                            //        row.Cells.Add(datelist[1].ToString().Trim());

                            //        foreach (MATDEFBMEntity Entity in BenchMark_List)
                            //        {
                            //            if (datelist[2].ToString().Trim() == Entity.Code)
                            //            {
                            //                BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                            //                break;
                            //            }
                            //        }
                            //        row.Cells.Add(BM_Desc.Trim());
                            //        first = false;
                            //    }
                            //    else
                            //    {
                            //        row.Cells.Add(LookupDataAccess.Getdate1(datelist[0].ToString().Trim()));
                            //        row.Cells.Add(datelist[1].ToString().Trim());

                            //        foreach (MATDEFBMEntity Entity in BenchMark_List)
                            //        {
                            //            if (datelist[2].ToString().Trim() == Entity.Code)
                            //            {
                            //                BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                            //                break;
                            //            }
                            //        }
                            //        row.Cells.Add(BM_Desc.Trim());
                            //    }
                            //}
                        }

                        switch (dr["Det_Progress_Code"].ToString().Trim())
                        {
                            case "a": row.Cells.Add("1");
                                break;
                            case "b": row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                            case "c": row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                            case "d": row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                            case "e": row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                            case "f": row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                            case "i": row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("");
                                row.Cells.Add("1");
                                break;
                        }


                    }

                }
                if (DetailTable.Rows.Count > 0)
                {
                    Worksheet sheet2 = book.Worksheets.Add("Sheet3");

                    sheet2.Table.Columns.Add(new WorksheetColumn(70));
                    sheet2.Table.Columns.Add(new WorksheetColumn(60));
                    sheet2.Table.Columns.Add(new WorksheetColumn(70));
                    sheet2.Table.Columns.Add(new WorksheetColumn(70));
                    sheet2.Table.Columns.Add(new WorksheetColumn(80));
                    sheet2.Table.Columns.Add(new WorksheetColumn(200));
                    sheet2.Table.Columns.Add(new WorksheetColumn(220));
                    sheet2.Table.Columns.Add(new WorksheetColumn(100));
                    sheet2.Table.Columns.Add(new WorksheetColumn(130));


                    WorksheetRow row = sheet2.Table.Rows.Add();
                    WorksheetCell cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "HeaderStyle");
                    cell.MergeAcross = 10;

                    row = sheet2.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("Agency", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Dept", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Year", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("AppNo", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Client Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Scale", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Base Range", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Assessment Range", "HeaderStyle"));


                    foreach (DataRow dr in DetailTable.Rows)
                    {
                        row = sheet2.Table.Rows.Add();

                        row.Cells.Add(dr["MATASMT_AGENCY"].ToString().Trim());
                        row.Cells.Add(dr["MATASMT_DEPT"].ToString().Trim());
                        row.Cells.Add(dr["MATASMT_PROGRAM"].ToString().Trim());
                        row.Cells.Add(dr["MATASMT_YEAR"].ToString().Trim());
                        row.Cells.Add(dr["MATASMT_APP"].ToString().Trim());
                        row.Cells.Add(dr["Name"].ToString().Trim());
                        row.Cells.Add(dr["MATDEF_DESC"].ToString().Trim());
                        row.Cells.Add(dr["BaseRange"].ToString().Trim());
                        row.Cells.Add(dr["AssmentRange"].ToString().Trim());

                    }


                }


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
            }
            catch (Exception ex) { }




        }

        private void On_SaveForm_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                //PdfName = strFolderPath + PdfName;
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }

                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";


                DataSet programData = _model.AdhocData.Rep_MAT1002_MatAssessments(Get_Sql_Parametets_List(), Privileges.ModuleCode);
                if (programData.Tables.Count > 0)
                {
                    DataTable Trans_BaseprogramData = Calculate_TableRow_Totals(programData.Tables[5]);
                    Trans_BaseprogramData = GenerateTransposedTable(Trans_BaseprogramData);

                    DataTable Trans_programData = Calculate_TableRow_Totals(programData.Tables[6]);
                    Trans_programData = GenerateTransposedTable(Trans_programData);


                    DataTable Trans_Sub_Columns = programData.Tables[3];
                    DataTable Trans_Sub_programData = GenerateTransposed_SubTable(programData.Tables[8]);
                    DataTable Counts_Table = programData.Tables[9];
                    DataTable Scales = programData.Tables[5];
                    DataTable Details_Table = new DataTable();
                    DataTable ScalesAssTable = new DataTable();
                    DataTable ScalesDetailTable = new DataTable();
                    if (chkDet.Checked)
                    {
                        Details_Table = programData.Tables[10];
                        ScalesAssTable = programData.Tables[11];
                        ScalesDetailTable = programData.Tables[12];
                    }


                    BaseLine_Clients_Cnt = Counts_Table.Rows[0]["BaseLine_App_Cnt"].ToString();
                    RepPeriod_Clients_Cnt = Counts_Table.Rows[0]["Rep_App_Cnt"].ToString();
                    FolloUp_Clients_Cnt = Counts_Table.Rows[0]["Followup_Cnt"].ToString();
                    Stable_Clients_Cnt = Counts_Table.Rows[0]["Progress_Cnt"].ToString();
                    Progress_Clients_Cnt = Counts_Table.Rows[0]["Stable_Cnt"].ToString();

                    //Dynamic_RDLC();
                    //Dynamic_Sub_RDLC();
                    //Dynamic_Sub_RDLC1();

                    foreach (DataRow dr in Trans_programData.Rows)
                    {
                        if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                        {
                            dr.Delete();
                            break;
                        }
                    }

                    string Tmp_BM_Code = string.Empty;
                    foreach (DataRow dr in Trans_programData.Rows)
                    {
                        Tmp_BM_Code = dr["BM_Scale"].ToString();

                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                        {
                            if (Tmp_BM_Code == "BM_S")
                            {
                                dr["BM_Scale"] = "Scale Bypassed"; break;
                            }
                            if (Tmp_BM_Code.Substring(3, (Tmp_BM_Code.Length - 3)) == Entity.Code)
                            {
                                dr["BM_Scale"] = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                break;
                            }
                        }
                    }

                    foreach (DataRow dr in Trans_BaseprogramData.Rows)
                    {
                        Tmp_BM_Code = dr["BM_Scale"].ToString();

                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                        {
                            if (Tmp_BM_Code == "BM_S")
                            {
                                dr["BM_Scale"] = "Scale Bypassed"; break;
                            }
                            if (Tmp_BM_Code.Substring(3, (Tmp_BM_Code.Length - 3)) == Entity.Code)
                            {
                                dr["BM_Scale"] = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                break;
                            }
                        }
                    }



                    foreach (DataRow dr in Trans_Sub_programData.Rows)
                    {
                        if (dr["BM_Scale"].ToString() == "BM_Scale_Desc")
                        {
                            dr.Delete();
                            break;
                        }
                    }

                    if (Privileges.ModuleCode == "03")
                    {
                        foreach (DataRow dr in Trans_Sub_programData.Rows)
                        {
                            if (Trans_Sub_Columns.Rows.Count > 0)
                            {
                                foreach (DataRow dc in Trans_Sub_Columns.Rows)
                                {
                                    if (dr["BM_Scale"].ToString() == dc["BM_Ass_Code"].ToString())
                                    {
                                        dr["BM_Scale"] = dc["BM_Desc"].ToString(); break;
                                    }
                                }
                            }
                            //switch (dr["BM_Scale"].ToString())
                            //{
                            //    case "a": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                            //    case "b": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                            //    case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            //    case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            //    case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            //    case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            //    case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            //}
                        }
                    }
                    else if (Privileges.ModuleCode == "02")
                    {
                        foreach (DataRow dr in Trans_Sub_programData.Rows)
                        {
                            foreach (DataRow dc in Trans_Sub_Columns.Rows)
                            {
                                if (dr["BM_Scale"].ToString() == dc["BM_Ass_Code"].ToString())
                                {
                                    dr["BM_Scale"] = dc["BM_Desc"].ToString(); break;
                                }
                            }
                            //foreach (MATDEFBMEntity ent in BenchMark_List)
                            //{
                            //    if (dr["BM_Scale"].ToString().Contains(ent.Code.Trim()) && dr["BM_Scale"].ToString().Substring(0, 1) != "R")
                            //    {
                            //        switch (dr["BM_Scale"].ToString().Substring(0, 1))
                            //        {
                            //            case "P": dr["BM_Scale"] = "# progressed to " + ent.Desc; break;
                            //            case "C": dr["BM_Scale"] = "# continued at " + ent.Desc; break;
                            //                //case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            //                //case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            //                //case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            //                //case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            //                //case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            //        }
                            //    }

                            //    if (dr["BM_Scale"].ToString().Contains("R"))
                            //    {
                            //        dr["BM_Scale"] = "Regressed ";
                            //    }
                            //}

                            ////switch (dr["BM_Scale"].ToString())
                            ////{
                            ////    case "a": dr["BM_Scale"] = "a. # progressed to Vulnerable"; break;
                            ////    case "b": dr["BM_Scale"] = "b. # progressed to Stable"; break;
                            ////    case "c": dr["BM_Scale"] = "c. # progressed to Safe/Thriving"; break;
                            ////    case "d": dr["BM_Scale"] = "d. # continued at Stable or >"; break;
                            ////    case "e": dr["BM_Scale"] = "e. # continued at Vulnerable"; break;
                            ////    case "f": dr["BM_Scale"] = "f. # Regressed"; break;
                            ////    case "g": dr["BM_Scale"] = "g. # continued at Crisis"; break;
                            ////}
                        }
                    }
                    Trans_Sub_programData.AcceptChanges();

                   

                    OnExcel_Report(PdfName, Details_Table, Scales, ScalesAssTable, ScalesDetailTable, Trans_BaseprogramData, Trans_programData, Trans_Sub_programData, programData, Trans_Sub_Columns);

                }
            }
        }



        private void OnExcel_Report(string Pdfname, DataTable table, DataTable Scale_Table, DataTable AssTable, DataTable DetailTable, DataTable BaseTable,DataTable AssesmentTable,DataTable Trans_Sub_programData, DataSet programData,DataTable Trans_Sub_Columns)
        {
            string propReportPath = _model.lookupDataAccess.GetReportPath();
            string PdfName = "Pdf File";
            PdfName = Pdfname;
            //PdfName = "MATB0002_Details.xls";
            ////PdfName = strFolderPath + PdfName;
            //PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            //try
            //{
            //    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
            //    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            //}
            //catch (Exception ex)
            //{
            //    CommonFunctions.MessageBoxDisplay("Error");
            //}

            //try
            //{
            //    string Tmpstr = PdfName;
            //    if (File.Exists(Tmpstr))
            //        File.Delete(Tmpstr);
            //}
            //catch (Exception ex)
            //{
            //    int length = 8;
            //    string newFileName = System.Guid.NewGuid().ToString();
            //    newFileName = newFileName.Replace("-", string.Empty);

            //    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            //}


            //if (!string.IsNullOrEmpty(Random_Filename))
            //    PdfName = Random_Filename;
            //else
            //    PdfName = PdfName;

            List<CaseHierarchyEntity> propCaseHieEntity = _model.AdhocData.Browse_CASEHIE("**", "**", "**", BaseForm.UserID, BaseForm.BaseAdminAgency);//**Vikash added userid and baseagy

            Sel_AGY = Current_Hierarchy.Substring(0, 2);
            Sel_DEPT = Current_Hierarchy.Substring(2, 2);
            Sel_PROG = Current_Hierarchy.Substring(4, 2);

            try
            {
                string data = null;
                Workbook book = new Workbook();

                Worksheet sheet; //WorksheetCell cell; WorksheetRow Row0;

                this.GenerateStyles(book.Styles);

                #region Header Page

                sheet = book.Worksheets.Add("Sheet1");

                WorksheetColumn columnHead = sheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                sheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = sheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172";
                sheet.Table.Columns.Add(59);
                // -----------------------------------------------
                WorksheetRow RowHead = sheet.Table.Rows.Add();
                WorksheetCell cell;
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s170";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                // -----------------------------------------------
                WorksheetRow Row1Head = sheet.Table.Rows.Add();
                Row1Head.Height = 12;
                Row1Head.AutoFitHeight = false;
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s140";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s141";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s171";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s142";
                // -----------------------------------------------
                WorksheetRow Row2Head = sheet.Table.Rows.Add();
                Row2Head.Height = 12;
                Row2Head.AutoFitHeight = false;
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "m2611536909264";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row3Head = sheet.Table.Rows.Add();
                Row3Head.Height = 12;
                Row3Head.AutoFitHeight = false;
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "m2611536909284";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "Selected Report Parameters";
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row4Head = sheet.Table.Rows.Add();
                Row4Head.Height = 12;
                Row4Head.AutoFitHeight = false;
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row5Head = sheet.Table.Rows.Add();
                Row5Head.Height = 12;
                Row5Head.AutoFitHeight = false;
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "m2611536909304";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row6Head = sheet.Table.Rows.Add();
                Row6Head.Height = 12;
                Row6Head.AutoFitHeight = false;
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row7Head = sheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row8 = sheet.Table.Rows.Add();
                Row8.Height = 12;
                Row8.AutoFitHeight = false;
                cell = Row8.Cells.Add();
                cell.StyleID = "s139";
                cell = Row8.Cells.Add();
                cell.StyleID = "s143";
                cell = Row8.Cells.Add();
                cell.StyleID = "m2611536909344";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row9 = sheet.Table.Rows.Add();
                Row9.Height = 12;
                Row9.AutoFitHeight = false;
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s143";
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s170";
                cell = Row9.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row10 = sheet.Table.Rows.Add();
                Row10.Height = 12;
                Row10.AutoFitHeight = false;
                cell = Row10.Cells.Add();
                cell.StyleID = "s139";
                cell = Row10.Cells.Add();
                cell.StyleID = "s143";
                cell = Row10.Cells.Add();
                cell.StyleID = "m2611540549592";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row11 = sheet.Table.Rows.Add();
                Row11.Height = 12;
                Row11.AutoFitHeight = false;
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s143";
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s170";
                cell = Row11.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row15Head = sheet.Table.Rows.Add();
                Row15Head.Height = 12;
                Row15Head.AutoFitHeight = false;
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s143";
                Row15Head.Cells.Add("            Matrix", DataType.String, "s144");
                Row15Head.Cells.Add(" : " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = Row15Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------

                string selectedAlertCodes = SelectedAlertCodes;
                selectedAlertCodes = selectedAlertCodes.Replace("'", "");
                selectedAlertCodes = selectedAlertCodes.Replace(",", ", ");

                WorksheetRow RowAlertCds = sheet.Table.Rows.Add();
                RowAlertCds.Height = 12;
                RowAlertCds.AutoFitHeight = false;
                cell = RowAlertCds.Cells.Add();
                cell.StyleID = "s139";
                cell = RowAlertCds.Cells.Add();
                cell.StyleID = "s143";
                RowAlertCds.Cells.Add("            Alert Codes", DataType.String, "s144");
                RowAlertCds.Cells.Add(": " + (rdbAlertAllCds.Checked ? "All" : selectedAlertCodes), DataType.String, "s169");
                cell = RowAlertCds.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row16Head = sheet.Table.Rows.Add();
                //Row16Head.Height = 12;
                //Row16Head.AutoFitHeight = false;
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row17 = sheet.Table.Rows.Add();
                Row17.Height = 12;
                Row17.AutoFitHeight = false;
                cell = Row17.Cells.Add();
                cell.StyleID = "s139";
                cell = Row17.Cells.Add();
                cell.StyleID = "s143";
                Row17.Cells.Add("            Scale", DataType.String, "s144");
                Row17.Cells.Add(": " + (Rb_All_Scales.Checked ? "All" : "Selected"), DataType.String, "s169");
                cell = Row17.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row18 = sheet.Table.Rows.Add();
                //Row18.Height = 12;
                //Row18.AutoFitHeight = false;
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row19 = sheet.Table.Rows.Add();
                Row19.Height = 12;
                Row19.AutoFitHeight = false;
                cell = Row19.Cells.Add();
                cell.StyleID = "s139";
                cell = Row19.Cells.Add();
                cell.StyleID = "s143";
                Row19.Cells.Add("            Date Selection", DataType.String, "s144");
                Row19.Cells.Add(": " + (Rb_Asmt_FDate.Checked ? "First Assessment Date" : "Last Assessment Date"), DataType.String, "s169");
                cell = Row19.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row12 = sheet.Table.Rows.Add();
                Row12.Height = 12;
                Row12.AutoFitHeight = false;
                cell = Row12.Cells.Add();
                cell.StyleID = "s139";
                cell = Row12.Cells.Add();
                cell.StyleID = "s143";
                Row12.Cells.Add("            Baseline Range From", DataType.String, "s144");
                Row12.Cells.Add(": " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_FDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To: " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Base_TDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), DataType.String, "s169");
                cell = Row12.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row13 = sheet.Table.Rows.Add();
                Row13.Height = 12;
                Row13.AutoFitHeight = false;
                cell = Row13.Cells.Add();
                cell.StyleID = "s139";
                cell = Row13.Cells.Add();
                cell.StyleID = "s143";
                Row13.Cells.Add("            Assessment Range From", DataType.String, "s144");
                Row13.Cells.Add(": " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_F_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To: " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asmt_T_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), DataType.String, "s169");
                cell = Row13.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row14 = sheet.Table.Rows.Add();
                //Row14.Height = 12;
                //Row14.AutoFitHeight = false;
                //cell = Row14.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row14.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row14.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row14.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row14.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row15Head = sheet.Table.Rows.Add();
                //Row15Head.Height = 12;
                //Row15Head.AutoFitHeight = false;
                //cell = Row15Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row15Head.Cells.Add();
                //cell.StyleID = "s143";
                //Row15Head.Cells.Add("            Matrix", DataType.String, "s144");
                //Row15Head.Cells.Add(" : " + ((ListItem)Cmb_Matrix.SelectedItem).Text.ToString(), DataType.String, "s169");
                //cell = Row15Head.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row16Head = sheet.Table.Rows.Add();
                //Row16Head.Height = 12;
                //Row16Head.AutoFitHeight = false;
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                //WorksheetRow Row17 = sheet.Table.Rows.Add();
                //Row17.Height = 12;
                //Row17.AutoFitHeight = false;
                //cell = Row17.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row17.Cells.Add();
                //cell.StyleID = "s143";
                //Row17.Cells.Add("            Scale", DataType.String, "s144");
                //Row17.Cells.Add(": " + (Rb_All_Scales.Checked ? "All" : "Selected"), DataType.String, "s169");
                //cell = Row17.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                //WorksheetRow Row18 = sheet.Table.Rows.Add();
                //Row18.Height = 12;
                //Row18.AutoFitHeight = false;
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                //WorksheetRow Row19 = sheet.Table.Rows.Add();
                //Row19.Height = 12;
                //Row19.AutoFitHeight = false;
                //cell = Row19.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row19.Cells.Add();
                //cell.StyleID = "s143";
                //Row19.Cells.Add("            Date Selection", DataType.String, "s144");
                //Row19.Cells.Add(": " + (Rb_Asmt_FDate.Checked ? "First Assessment Date" : "Last Assessment Date"), DataType.String, "s169");
                //cell = Row19.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                WorksheetRow Row20 = sheet.Table.Rows.Add();
                Row20.Height = 12;
                Row20.AutoFitHeight = false;
                cell = Row20.Cells.Add();
                cell.StyleID = "s139";
                cell = Row20.Cells.Add();
                cell.StyleID = "s143";
                Row20.Cells.Add("            Caseworker", DataType.String, "s144");
                Row20.Cells.Add(" : " + ((ListItem)Cmb_Worker.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = Row20.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row120 = sheet.Table.Rows.Add();
                Row120.Height = 12;
                Row120.AutoFitHeight = false;
                cell = Row120.Cells.Add();
                cell.StyleID = "s139";
                cell = Row120.Cells.Add();
                cell.StyleID = "s143";
                Row120.Cells.Add("            Site", DataType.String, "s144");
                Row120.Cells.Add(" : " + ((ListItem)cmbSite.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = Row120.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row21 = sheet.Table.Rows.Add();
                Row21.Height = 12;
                Row21.AutoFitHeight = false;
                cell = Row21.Cells.Add();
                cell.StyleID = "s139";
                cell = Row21.Cells.Add();
                cell.StyleID = "s143";
                Row21.Cells.Add((Cb_Enroll.Checked ? "            Enroll Status" : "  "), DataType.String, "s144");
                Row21.Cells.Add((Cb_Enroll.Checked ? ": " + ((ListItem)cmbEnrlStatus.SelectedItem).Text.ToString() : "  "), DataType.String, "s169");
                cell = Row21.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row22 = sheet.Table.Rows.Add();
                Row22.Height = 12;
                Row22.AutoFitHeight = false;
                cell = Row22.Cells.Add();
                cell.StyleID = "s139";
                cell = Row22.Cells.Add();
                cell.StyleID = "s143";
                Row22.Cells.Add((Cb_Enroll.Checked ? "            As of Date" : "  "), DataType.String, "s144");
                Row22.Cells.Add((Cb_Enroll.Checked ? ": From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Asof_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat) : "  "), DataType.String, "s169");
                cell = Row22.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row23 = sheet.Table.Rows.Add();
                Row23.Height = 12;
                Row23.AutoFitHeight = false;
                cell = Row23.Cells.Add();
                cell.StyleID = "s139";
                cell = Row23.Cells.Add();
                cell.StyleID = "s143";
                Row23.Cells.Add((Cb_Enroll.Checked ? "            Program" : "  "), DataType.String, "s144");
                Row23.Cells.Add((Cb_Enroll.Checked ? ": " + Txt_Program.Text.Trim() : "  "), DataType.String, "s169");
                cell = Row23.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                if (cmbAssessmentType.Visible == true)
                {
                    WorksheetRow Row122 = sheet.Table.Rows.Add();
                    Row122.Height = 12;
                    Row122.AutoFitHeight = false;
                    cell = Row122.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Row122.Cells.Add();
                    cell.StyleID = "s143";
                    Row122.Cells.Add("            Assessment Type ", DataType.String, "s144");
                    Row122.Cells.Add(" : " + ((ListItem)cmbAssessmentType.SelectedItem).Text.ToString(), DataType.String, "s169");
                    cell = Row122.Cells.Add();
                    cell.StyleID = "s145";
                }
                // -----------------------------------------------
                string PrintDet = string.Empty;
                if (chkDet.Checked)
                    PrintDet = "Yes";
                else
                    PrintDet = "No";
                WorksheetRow Row123 = sheet.Table.Rows.Add();
                Row123.Height = 12;
                Row123.AutoFitHeight = false;
                cell = Row123.Cells.Add();
                cell.StyleID = "s139";
                cell = Row123.Cells.Add();
                cell.StyleID = "s143";
                Row123.Cells.Add("            Print Details ", DataType.String, "s144");
                Row123.Cells.Add(" : " + PrintDet, DataType.String, "s169");
                cell = Row123.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row24 = sheet.Table.Rows.Add();
                Row24.Height = 12;
                Row24.AutoFitHeight = false;
                cell = Row24.Cells.Add();
                cell.StyleID = "s139";
                cell = Row24.Cells.Add();
                cell.StyleID = "s143";
                cell = Row24.Cells.Add();
                cell.StyleID = "m2611540549632";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row25 = sheet.Table.Rows.Add();
                Row25.Height = 12;
                Row25.AutoFitHeight = false;
                cell = Row25.Cells.Add();
                cell.StyleID = "s139";
                cell = Row25.Cells.Add();
                cell.StyleID = "s143";
                cell = Row25.Cells.Add();
                cell.StyleID = "m2611540549652";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row26Head = sheet.Table.Rows.Add();
                Row26Head.Height = 12;
                Row26Head.AutoFitHeight = false;
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row27Head = sheet.Table.Rows.Add();
                Row27Head.Height = 12;
                Row27Head.AutoFitHeight = false;
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "m2611540549672";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row28 = sheet.Table.Rows.Add();
                Row28.Height = 12;
                Row28.AutoFitHeight = false;
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s143";
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s170";
                cell = Row28.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row29 = sheet.Table.Rows.Add();
                Row29.Height = 12;
                Row29.AutoFitHeight = false;
                cell = Row29.Cells.Add();
                cell.StyleID = "s139";
                cell = Row29.Cells.Add();
                cell.StyleID = "s143";
                cell = Row29.Cells.Add();
                cell.StyleID = "m2611540549552";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row30 = sheet.Table.Rows.Add();
                Row30.Height = 12;
                Row30.AutoFitHeight = false;
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s143";
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s170";
                cell = Row30.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row31 = sheet.Table.Rows.Add();
                Row31.Height = 12;
                Row31.AutoFitHeight = false;
                cell = Row31.Cells.Add();
                cell.StyleID = "s139";
                cell = Row31.Cells.Add();
                cell.StyleID = "s146";
                cell = Row31.Cells.Add();
                cell.StyleID = "m2611540549572";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                //  Options
                // -----------------------------------------------
                sheet.Options.Selected = true;
                sheet.Options.ProtectObjects = false;
                sheet.Options.ProtectScenarios = false;
                sheet.Options.PageSetup.Header.Margin = 0.3F;
                sheet.Options.PageSetup.Footer.Margin = 0.3F;
                sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
                sheet.Options.PageSetup.PageMargins.Left = 0.7F;
                sheet.Options.PageSetup.PageMargins.Right = 0.7F;
                sheet.Options.PageSetup.PageMargins.Top = 0.75F;


                #endregion



                #region Summary Sheet

                sheet = book.Worksheets.Add("Matrix Summary");

                WorksheetColumn column0 = sheet.Table.Columns.Add();
                column0.Width = 36;
                column0.StyleID = "s63";
                WorksheetColumn column1 = sheet.Table.Columns.Add();
                column1.Width = 57;
                column1.StyleID = "s63";
                column1.Span = 4;
                WorksheetColumn column2 = sheet.Table.Columns.Add();
                column2.Index = 7;
                column2.Width = 40;
                column2.StyleID = "s63";
                WorksheetColumn column3 = sheet.Table.Columns.Add();
                column3.Width = 36;
                column3.StyleID = "s63";
                column3.Span = 7;
                WorksheetColumn column4 = sheet.Table.Columns.Add();
                column4.Index = 16;
                column4.Width = 28;
                column4.StyleID = "s63";
                WorksheetColumn column5 = sheet.Table.Columns.Add();
                column5.Width = 88;
                column5.StyleID = "s63";
                // -----------------------------------------------
                WorksheetRow Row0 = sheet.Table.Rows.Add();
                
                cell = Row0.Cells.Add();
                cell.StyleID = "s64";
                cell.Index = 17;
                //-----------------------------------------------
                WorksheetRow Row1 = sheet.Table.Rows.Add();
                Row1.Height = 15;
                Row1.AutoFitHeight = false;
                cell = Row1.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "A. Total number of persons assessed during Baseline Range";
                cell.Index = 2;
                cell.MergeAcross = 11;
                cell = Row1.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.Number;
                cell.Data.Text = programData.Tables[9].Rows[0]["BaseLine_App_Cnt"].ToString();
                cell.MergeAcross = 1;
                cell = Row1.Cells.Add();
                cell.StyleID = "s64";
                cell.Index = 17;
                // -----------------------------------------------
                WorksheetRow Row2 = sheet.Table.Rows.Add();
                cell = Row2.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "B. Total number of persons assessed during Assessment Range";
                cell.Index = 2;
                cell.MergeAcross = 11;
                cell = Row2.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.Number;
                cell.Data.Text = programData.Tables[9].Rows[0]["Rep_App_Cnt"].ToString();
                cell.MergeAcross = 1;
                // -----------------------------------------------
                WorksheetRow Row3 = sheet.Table.Rows.Add();
                cell = Row3.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "C. Total number of persons receiving a Follow-up Assessment";
                cell.Index = 2;
                cell.MergeAcross = 11;
                cell = Row3.Cells.Add();
                cell.StyleID = "s65";
                cell.Data.Type = DataType.Number;
                cell.Data.Text = programData.Tables[9].Rows[0]["Followup_Cnt"].ToString();
                cell.MergeAcross = 1;
                // -----------------------------------------------
                WorksheetRow Row4 = sheet.Table.Rows.Add();
                Row4.Height = 7;
                Row4.AutoFitHeight = false;
                cell = Row4.Cells.Add();
                cell.StyleID = "s66";
                cell.Index = 4;
                cell = Row4.Cells.Add();
                cell.StyleID = "s66";
                cell = Row4.Cells.Add();
                cell.StyleID = "s67";
                cell.MergeAcross = 4;
                cell = Row4.Cells.Add();
                cell.StyleID = "s68";
                cell = Row4.Cells.Add();
                cell.StyleID = "s68";
                cell = Row4.Cells.Add();
                cell.StyleID = "s68";
                cell = Row4.Cells.Add();
                cell.StyleID = "s69";
                cell.MergeAcross = 1;
                //Base Table counts
                WorksheetRow Row5 = sheet.Table.Rows.Add();
                Row5.Height = 18;
                Row5.AutoFitHeight = false;
                cell = Row5.Cells.Add();
                cell.StyleID = "s70";
                cell = Row5.Cells.Add();
                cell.StyleID = "s71";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "1. Baseline Benchmark Level";
                cell.MergeAcross = 13;
                cell = Row5.Cells.Add();
                cell.StyleID = "s70";
                cell = Row5.Cells.Add();
                cell.StyleID = "s70";
                if (BaseTable.Rows.Count > 0)
                {
                    //DataTable BaseTable = programData.Tables[5];

                    WorksheetRow Row6 = sheet.Table.Rows.Add();
                    Row6.Height = 144;
                    Row6.AutoFitHeight = false;
                    cell = Row6.Cells.Add();
                    cell.StyleID = "s70";

                    DataRow FirstRow = BaseTable.Rows[0];

                    bool ISfalse = true;
                    foreach (DataColumn dc in BaseTable.Columns)
                    {
                        if (ISfalse)
                        {
                            cell = Row6.Cells.Add(FirstRow[dc.ColumnName].ToString(), DataType.String, "s73");
                            cell.MergeAcross = 4;
                            ISfalse = false;
                        }
                        else
                            Row6.Cells.Add(FirstRow[dc.ColumnName].ToString(), DataType.String, "s74");
                    }

                    //WorksheetRow Row7 = sheet.Table.Rows.Add();
                    //Row7.AutoFitHeight = false;

                    int i = 0;
                    foreach (DataRow dr in BaseTable.Rows)
                    {
                        if (i > 0)
                        {
                            WorksheetRow Row7 = sheet.Table.Rows.Add();
                            Row7.AutoFitHeight = false;
                            cell = Row7.Cells.Add();
                            cell.StyleID = "s70";
                            bool IsRow = true;
                            foreach (DataColumn dc in BaseTable.Columns)
                            {
                                
                                if (IsRow)
                                {
                                    cell = Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.String, "s75");
                                    cell.MergeAcross = 4;
                                    IsRow = false;
                                }
                                else
                                    Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.Number, "s77");
                            }
                        }
                        i++;
                    }

                }

                // -----------------------------------------------
                WorksheetRow Row15 = sheet.Table.Rows.Add();
                Row15.Height = 14;
                Row15.AutoFitHeight = false;
                cell = Row15.Cells.Add();
                cell.StyleID = "s70";
                cell = Row15.Cells.Add();
                cell.StyleID = "s80";
                cell = Row15.Cells.Add();
                cell.StyleID = "s81";
                cell = Row15.Cells.Add();
                cell.StyleID = "s81";
                cell = Row15.Cells.Add();
                cell.StyleID = "s81";
                cell = Row15.Cells.Add();
                cell.StyleID = "s81";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s82";
                cell = Row15.Cells.Add();
                cell.StyleID = "s70";


                // -----------------------------------------------
                WorksheetRow Row16 = sheet.Table.Rows.Add();
                Row16.Height = 18;
                Row16.AutoFitHeight = false;
                cell = Row16.Cells.Add();
                cell.StyleID = "s70";
                cell = Row16.Cells.Add();
                cell.StyleID = "s83";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "2. Assessment Benchmark Level";
                cell.MergeAcross = 13;
                cell = Row16.Cells.Add();
                cell.StyleID = "s70";
                cell = Row16.Cells.Add();
                cell.StyleID = "s70";

                if (AssesmentTable.Rows.Count > 0)
                {
                    //DataTable BaseTable = programData.Tables[5];

                    WorksheetRow Row6 = sheet.Table.Rows.Add();
                    Row6.Height = 144;
                    Row6.AutoFitHeight = false;
                    cell = Row6.Cells.Add();
                    cell.StyleID = "s70";

                    DataRow FirstRow = BaseTable.Rows[0];

                    bool ISfalse = true;
                    foreach (DataColumn dc in AssesmentTable.Columns)
                    {
                        if (ISfalse)
                        {
                            cell = Row6.Cells.Add(FirstRow[dc.ColumnName].ToString(), DataType.String, "s73");
                            cell.MergeAcross = 4;
                            ISfalse = false;
                        }
                        else
                            Row6.Cells.Add(FirstRow[dc.ColumnName].ToString(), DataType.String, "s74");
                    }

                    //WorksheetRow Row7 = sheet.Table.Rows.Add();
                    //Row7.AutoFitHeight = false;

                    int i = 0;
                    foreach (DataRow dr in AssesmentTable.Rows)
                    {
                        //if (i > 0)
                        //{
                            WorksheetRow Row7 = sheet.Table.Rows.Add();
                            Row7.AutoFitHeight = false;
                            cell = Row7.Cells.Add();
                            cell.StyleID = "s70";
                            bool IsRow = true;
                            foreach (DataColumn dc in AssesmentTable.Columns)
                            {

                                if (IsRow)
                                {
                                    cell = Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.String, "s75");
                                    cell.MergeAcross = 4;
                                    IsRow = false;
                                }
                                else
                                    Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.Number, "s77");
                            }
                        //}
                        i++;
                    }

                }
            


                // -----------------------------------------------
                WorksheetRow Row26 = sheet.Table.Rows.Add();
                Row26.Height = 6;
                Row26.AutoFitHeight = false;
                cell = Row26.Cells.Add();
                cell.StyleID = "s70";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s81";
                cell = Row26.Cells.Add();
                cell.StyleID = "s70";
                cell = Row26.Cells.Add();
                cell.StyleID = "s70";
                // -----------------------------------------------
                WorksheetRow Row27 = sheet.Table.Rows.Add();
                Row27.Height = 18;
                Row27.AutoFitHeight = false;
                cell = Row27.Cells.Add();
                cell.StyleID = "s70";
                cell = Row27.Cells.Add();
                cell.StyleID = "m1698893804784";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "3. Follow-up Progress";
                cell.MergeAcross = 13;
                cell = Row27.Cells.Add();
                cell.StyleID = "s70";

                if (Trans_Sub_programData.Rows.Count > 0)
                {
                    foreach (DataRow dr in Trans_Sub_programData.Rows)
                    {
                        WorksheetRow Row7 = sheet.Table.Rows.Add();
                        Row7.AutoFitHeight = false;

                        bool ISrow = true;
                        cell = Row7.Cells.Add();
                        cell.StyleID = "s70";
                        foreach (DataColumn dc in Trans_Sub_programData.Columns)
                        {
                            if (ISrow)
                            {
                                cell = Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.String, "m1698893804804");
                                cell.MergeAcross = 4;
                                ISrow = false;
                            }
                            else
                                Row7.Cells.Add(dr[dc.ColumnName].ToString(), DataType.Number, "s77");
                        }
                    }
                }
                // -----------------------------------------------
                WorksheetRow Row36 = sheet.Table.Rows.Add();
                Row36.Height = 7;
                Row36.AutoFitHeight = false;
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                cell = Row36.Cells.Add();
                cell.StyleID = "s70";
                // -----------------------------------------------
                WorksheetRow Row37 = sheet.Table.Rows.Add();
                Row37.Height = 14;
                Row37.AutoFitHeight = false;
                cell = Row37.Cells.Add();
                cell.StyleID = "s70";
                cell = Row37.Cells.Add();
                cell.StyleID = "s95";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "4. Progress towards Self-Sufficiency ";
                cell.MergeAcross = 13;
                cell = Row37.Cells.Add();
                cell.StyleID = "s70";
                // -----------------------------------------------
                WorksheetRow Row38 = sheet.Table.Rows.Add();
                Row38.Height = 14;
                Row38.AutoFitHeight = false;
                cell = Row38.Cells.Add();
                cell.StyleID = "s70";
                cell = Row38.Cells.Add();
                cell.StyleID = "s85";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "a. Total number of persons who made progress towards Self-Sufficiency: ";
                cell.MergeAcross = 11;
                cell = Row38.Cells.Add();
                cell.StyleID = "s76";
                cell.Data.Type = DataType.Number;
                cell.Data.Text = programData.Tables[9].Rows[0]["Stable_Cnt"].ToString();
                cell.MergeAcross = 1;
                cell = Row38.Cells.Add();
                cell.StyleID = "s70";
                // -----------------------------------------------

                #endregion

                if (chkDet.Checked)
                {
                    #region Unduplicated Person Count
                    if (table.Rows.Count>0)
                    {
                        sheet = book.Worksheets.Add("Unduplicated Person Count (ABC)");
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(130));
                        sheet.Table.Columns.Add(new WorksheetColumn(130));

                        #region Added 2 columns by Vikash on 06/17/2024 as per 2024 Enhancements Document

                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(200));

                        #endregion

                        if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                            sheet.Table.Columns.Add(new WorksheetColumn(130));
                        //if (Cb_Enroll.Checked)
                        //    sheet.Table.Columns.Add(new WorksheetColumn(200));
                        if (Cb_Enroll.Checked && (!string.IsNullOrEmpty(Txt_Program.Text.Trim())))
                        {
                            string[] Programs = Txt_Program.Text.Split(',');
                            if (Programs.Length > 0)
                            {
                                for (int i = 0; i < Programs.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Programs[i].Trim()))
                                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                                }
                            }
                        }

                        //WorksheetStyle style = book.Styles.Add("HeaderStyle");
                        //style.Font.FontName = "Tahoma";
                        //style.Font.Size = 12;
                        //style.Font.Bold = true;
                        //style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                        //WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                        //style1.Font.FontName = "Tahoma";
                        //style1.Font.Size = 12;
                        //style1.Font.Bold = true;
                        //style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                        //WorksheetStyle style2 = book.Styles.Add("CellStyle");
                        //style2.Font.FontName = "Tahoma";
                        //style2.Font.Size = 10;
                        //style2.Font.Color = "Blue";
                        //style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                        //style = book.Styles.Add("Default");
                        //style.Font.FontName = "Tahoma";
                        //style.Font.Size = 10;

                        WorksheetRow row = sheet.Table.Rows.Add();

                        cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s133");
                        //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                        if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                            cell.MergeAcross = 10;
                        else if (Cb_Enroll.Checked)
                            cell.MergeAcross = 11;//9;
                        else
                            cell.MergeAcross = 10;//8;


                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("Agency", DataType.String,"s134"));
                        row.Cells.Add(new WorksheetCell("Dept", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Program", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Year", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("AppNo", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Client Name", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Base Range", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Assessment Range", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Follow-up", DataType.String, "s134"));

                        row.Cells.Add(new WorksheetCell("Alert Codes", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Assigned Worker", DataType.String, "s134"));

                        if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                            row.Cells.Add(new WorksheetCell("Status", "s134"));
                        //if (Cb_Enroll.Checked)
                        //    row.Cells.Add(new WorksheetCell("Program", "HeaderStyle"));
                        if (Cb_Enroll.Checked && (!string.IsNullOrEmpty(Txt_Program.Text.Trim())))
                        {
                            string[] Programs = Txt_Program.Text.Split(',');
                            if (Programs.Length > 0)
                            {
                                for (int i = 0; i < Programs.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Programs[i].Trim()))
                                    {
                                        CaseHierarchyEntity HieProgram = propCaseHieEntity.Find(u => u.Code.Equals(Programs[i].Trim()));
                                        if (HieProgram != null)
                                            row.Cells.Add(new WorksheetCell(HieProgram.HierarchyName.Trim(), "HeaderStyle"));
                                    }
                                }
                            }
                        }


                        //excelcolumn = excelcolumn + 1;
                        //string[] HeaderSeq4 = { "Agency", "Dept", "Program", "Year", "AppNo", "Client Name", "Base Range", "Assessment Range" };
                        //for (int i = 0; i < HeaderSeq4.Length; ++i)
                        //{
                        //    xlWorkSheet[excelcolumn, (i + 1)].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        //    xlWorkSheet[excelcolumn, (i + 1)].Alignment = Alignment.Centered;
                        //    xlWorkSheet.WriteCell(excelcolumn, (i + 1), HeaderSeq4[i]);
                        //}

                        bool first = true; int length = 0;
                        foreach (DataRow dr in table.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(dr["Det_Agy"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_Dept"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_Prog"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_Year"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_App"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Client_Name"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_BaseRange"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_AssmentRange"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_FollowUp"].ToString().Trim(), DataType.String, "s135");

                            row.Cells.Add(dr["MST_ALERT_CODES"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["MST_INTAKE_WORKER"].ToString().Trim(), DataType.String, "s135");

                            if (Cb_Enroll.Checked && ((ListItem)cmbEnrlStatus.SelectedItem).Value.ToString().Trim() == "B")
                            {
                                string Status = string.Empty;
                                if (dr["ENRL_STATUS"].ToString().Trim() == "E")
                                    Status = "Enroll";
                                else if (dr["ENRL_STATUS"].ToString().Trim() == "W")
                                    Status = "Withdrawn";

                                row.Cells.Add(Status.Trim(), DataType.String, "s135");
                            }
                            if (Cb_Enroll.Checked && !string.IsNullOrEmpty(Txt_Program.Text.Trim()))
                            {
                                string EnrlProgs = dr["ENRL_FUND_HIE"].ToString().Trim();

                                string[] Programs = Txt_Program.Text.Split(',');
                                if (Programs.Length > 0)
                                {
                                    for (int i = 0; i < Programs.Length; i++)
                                    {
                                        if (!string.IsNullOrEmpty(Programs[i].Trim()))
                                        {
                                            if (EnrlProgs.Contains(Programs[i].Trim()))
                                                row.Cells.Add("Y", DataType.String, "s135");
                                            else row.Cells.Add(" ", DataType.String, "s135");
                                        }
                                        //CaseHierarchyEntity HieProgram = propCaseHieEntity.Find(u => u.Code.Equals(Programs[i].Trim()));
                                        //if (HieProgram != null)
                                        //    row.Cells.Add(new WorksheetCell(HieProgram.HierarchyName.Trim(), "HeaderStyle"));
                                    }
                                }
                            }

                        }


                    }
                    #endregion

                    #region Base Range Details with Scales

                    if (programData.Tables[13].Rows.Count > 0)
                    {
                        DataTable dtBase = programData.Tables[13];

                        sheet = book.Worksheets.Add("Details for Base Range");
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                        sheet.Table.Columns.Add(new WorksheetColumn(180));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));

                        sheet.Table.Columns.Add(new WorksheetColumn(80)); // Added by Vikash on 03/22/2024 as part of 2024 Enhancement for Alert Codes


                        WorksheetRow row = sheet.Table.Rows.Add();

                        cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s133");
                        cell.MergeAcross = 10;//9;

                        for (int i = 1; i < 8; i++)
                        {
                            row.Cells.Add("");
                        }

                        WorksheetCell bas1e = row.Cells.Add("Baseline", DataType.String, "s134");
                        bas1e.MergeAcross = 2;


                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("Agency", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Dept", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Program", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Year", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("AppNo", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Client Name", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Scale", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Date", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Points", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Benchmark", DataType.String, "s134"));

                        row.Cells.Add(new WorksheetCell("Alert Codes", DataType.String, "s134")); // Added by Vikash on 03/22/2024 as part of 2024 Enhancement for Alert Codes

                        foreach (DataRow dr in dtBase.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(dr["Agy"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Dept"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Prog"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Base_Year"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["App"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Client_Name"].ToString().Trim(), DataType.String, "s135");

                            string Scale_Desc = dr["Scale_Code"].ToString().Trim();
                            if (Scale_Table.Rows.Count > 0)
                            {
                                foreach (DataRow drscale in Scale_Table.Rows)
                                {
                                    if (dr["Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
                                    {
                                        Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
                                    }
                                }
                            }

                            row.Cells.Add(Scale_Desc.Trim());

                            row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "s135");
                            //row.Cells.Add(dr["Points"].ToString().Trim(), DataType.String, "s135");

                            string BM_Desc = dr["BM_Code"].ToString().Trim();
                            //if (!string.IsNullOrEmpty(dr["BaseLine_Date"].ToString().Trim()))
                            //{
                                if (dr["BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(dr["Points"].ToString().Trim(),DataType.String, "s135");
                                else
                                    row.Cells.Add("");

                                foreach (MATDEFBMEntity Entity in BenchMark_List)
                                {
                                    if (dr["BM_Code"].ToString().Trim() == Entity.Code)
                                    {
                                        BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                        break;
                                    }
                                }
                                if (dr["BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(BM_Desc.Trim(), DataType.String, "s135");
                                else row.Cells.Add("Scale Bypassed", DataType.String, "s135");
                            //}
                            row.Cells.Add(dr["Tmp_ALERT_Codes"].ToString().Trim() == "" ? "" : dr["Tmp_ALERT_Codes"].ToString().Trim(), DataType.String, "s135"); //Added by Vikash on 03/22/2024 as part of 2024 Enhancement for Alert Codes

                        }

                    }
                    #endregion

                    #region Assessment Range details with Scales
                    if (programData.Tables[14].Rows.Count > 0)
                    {
                        DataTable dtAss = programData.Tables[14];

                        sheet = book.Worksheets.Add("Details for Assessment Range");
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                        sheet.Table.Columns.Add(new WorksheetColumn(180));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));

                        sheet.Table.Columns.Add(new WorksheetColumn(80)); //Added by Vikash on 03/22/2024 as part of 2024 Enhancement for Alert Codes

                        WorksheetRow row = sheet.Table.Rows.Add();

                        cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s133");
                        cell.MergeAcross = 10;//9;

                        for (int i = 1; i < 8; i++)
                        {
                            row.Cells.Add("");
                        }

                        WorksheetCell bas1e = row.Cells.Add("Assessment Range", DataType.String, "s134");
                        bas1e.MergeAcross = 2;


                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("Agency", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Dept", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Program", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Year", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("AppNo", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Client Name", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Scale", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Date", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Points", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Benchmark", DataType.String, "s134"));

                        row.Cells.Add(new WorksheetCell("Alert Codes", DataType.String, "s134")); //Added by Vikash on 03/22/2024 as part of 2024 Enhancement for Alert Codes

                        foreach (DataRow dr in dtAss.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(dr["Agy"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Dept"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Prog"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Ass_Year"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["App"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Client_Name"].ToString().Trim(), DataType.String, "s135");

                            string Scale_Desc = dr["Scale_Code"].ToString().Trim();
                            if (Scale_Table.Rows.Count > 0)
                            {
                                foreach (DataRow drscale in Scale_Table.Rows)
                                {
                                    if (dr["Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
                                    {
                                        Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
                                    }
                                }
                            }

                            row.Cells.Add(Scale_Desc.Trim());

                            row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "s135");
                            //row.Cells.Add(dr["Points"].ToString().Trim(), DataType.String, "s135");

                            string BM_Desc = dr["BM_Code"].ToString().Trim();
                            //if (!string.IsNullOrEmpty(dr["BaseLine_Date"].ToString().Trim()))
                            //{
                            if (dr["BYPASS"].ToString().Trim() == "N")
                                row.Cells.Add(dr["Points"].ToString().Trim(), DataType.String, "s135");
                            else
                                row.Cells.Add("");

                            foreach (MATDEFBMEntity Entity in BenchMark_List)
                            {
                                if (dr["BM_Code"].ToString().Trim() == Entity.Code)
                                {
                                    BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                    break;
                                }
                            }
                            if (dr["BYPASS"].ToString().Trim() == "N")
                                row.Cells.Add(BM_Desc.Trim(), DataType.String, "s135");
                            else row.Cells.Add("Scale Bypassed", DataType.String, "s135");
                            //}

                            row.Cells.Add(dr["Tmp_ALERT_Codes"].ToString().Trim() == "" ? "" : dr["Tmp_ALERT_Codes"].ToString().Trim(), DataType.String, "s135");//Added by Vikash on 03 / 22 / 2024 as part of 2024 Enhancement for Alert Codes
                        }

                    }
                    #endregion

                    #region Follow-up details

                    if (AssTable.Rows.Count > 0)
                    {


                        Worksheet sheet1 = book.Worksheets.Add("Followup Details");

                        //WorksheetColumn Column = sheet1.Table.Columns.Add();

                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(60));
                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(80));
                        sheet1.Table.Columns.Add(new WorksheetColumn(220));
                        sheet1.Table.Columns.Add(new WorksheetColumn(200));
                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        sheet1.Table.Columns.Add(new WorksheetColumn(120));
                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        sheet1.Table.Columns.Add(new WorksheetColumn(120));
                        sheet1.Table.Columns.Add(new WorksheetColumn(70));
                        sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        sheet1.Table.Columns.Add(new WorksheetColumn(120));
                        if(Trans_Sub_Columns.Rows.Count>0)
                        {
                            foreach(DataRow dr in Trans_Sub_Columns.Rows)
                            {
                                sheet1.Table.Columns.Add(new WorksheetColumn(50));
                            }
                        }
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(50));
                        //sheet1.Table.Columns.Add(new WorksheetColumn(100));

                        WorksheetRow row = sheet1.Table.Rows.Add();

                        cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s133");
                        cell.MergeAcross = 22;

                        row = sheet1.Table.Rows.Add();

                        for (int i = 1; i < 8; i++)
                        {
                            row.Cells.Add("");
                        }

                        WorksheetCell bas1e = row.Cells.Add("Baseline", DataType.String, "s134");
                        bas1e.MergeAcross = 2;

                        WorksheetCell Assw1 = row.Cells.Add("Assessment", DataType.String, "s134");
                        Assw1.MergeAcross = 2;

                        WorksheetCell Followe = row.Cells.Add("Follow-up", DataType.String, "s134");
                        Followe.MergeAcross = 2;
                        //if (k > 0)
                        //{
                        //    for (int i = 1; i <= k; i++)
                        //    {
                        //        WorksheetCell Assw2 = row.Cells.Add("Assessment " + (i + 1), DataType.String, "HeaderStyle");
                        //        Assw2.MergeAcross = 2;
                        //    }
                        //}

                        row = sheet1.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("Agency", "s134"));
                        row.Cells.Add(new WorksheetCell("Dept", "s134"));
                        row.Cells.Add(new WorksheetCell("Program", "s134"));
                        row.Cells.Add(new WorksheetCell("Year", "s134"));
                        row.Cells.Add(new WorksheetCell("AppNo", "s134"));
                        row.Cells.Add(new WorksheetCell("Client Name", "s134"));
                        row.Cells.Add(new WorksheetCell("Scale", "s134"));
                        row.Cells.Add(new WorksheetCell("Date", "s134"));
                        row.Cells.Add(new WorksheetCell("Points", "s134"));
                        row.Cells.Add(new WorksheetCell("Benchmark", "s134"));
                        row.Cells.Add(new WorksheetCell("Date", "s134"));
                        row.Cells.Add(new WorksheetCell("Points", "s134"));
                        row.Cells.Add(new WorksheetCell("Benchmark", "s134"));
                        row.Cells.Add(new WorksheetCell("Date", "s134"));
                        row.Cells.Add(new WorksheetCell("Points", "s134"));
                        row.Cells.Add(new WorksheetCell("Benchmark", "s134"));
                        if (Trans_Sub_Columns.Rows.Count > 0)
                        {
                            foreach (DataRow dr in Trans_Sub_Columns.Rows)
                            {
                                row.Cells.Add(new WorksheetCell(dr["BM_Ass_Code"].ToString(), "s134"));
                            }
                        }
                        //row.Cells.Add(new WorksheetCell("a", "s134"));
                        //row.Cells.Add(new WorksheetCell("b", "s134"));
                        //row.Cells.Add(new WorksheetCell("c", "s134"));
                        //row.Cells.Add(new WorksheetCell("d", "s134"));
                        //row.Cells.Add(new WorksheetCell("e", "s134"));
                        //row.Cells.Add(new WorksheetCell("f", "s134"));
                        //row.Cells.Add(new WorksheetCell("Incrisis", "s134"));


                        bool first = true; int length = 0;
                        foreach (DataRow dr in AssTable.Rows)
                        {
                            row = sheet1.Table.Rows.Add();

                            row.Cells.Add(dr["Det_Agy"].ToString().Trim(),DataType.String, "s135");
                            row.Cells.Add(dr["Det_Dept"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_Prog"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_Year"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Det_App"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Client_Name"].ToString().Trim(), DataType.String, "s135");

                            string Scale_Desc = dr["Det_Scale_Code"].ToString().Trim();
                            if (Scale_Table.Rows.Count > 0)
                            {
                                foreach (DataRow drscale in Scale_Table.Rows)
                                {
                                    if (dr["Det_Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
                                    {
                                        Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
                                    }
                                }
                            }

                            row.Cells.Add(Scale_Desc.Trim(), DataType.String, "s135");


                            if (!string.IsNullOrEmpty(Asmt_T_Date.Text.Trim()))
                            {
                                if (Convert.ToDateTime(dr["BaseLine_Date"].ToString().Trim()) >= Convert.ToDateTime(Asmt_F_Date.Text.Trim()) && Convert.ToDateTime(dr["BaseLine_Date"].ToString().Trim()) <= Convert.ToDateTime(Asmt_T_Date.Text.Trim()))
                                    row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "s135");
                                else
                                    row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "s135");
                            }
                            else
                                row.Cells.Add(LookupDataAccess.Getdate1(dr["BaseLine_Date"].ToString().Trim()), DataType.String, "s135");

                            string BM_Desc = dr["Det_Asmt_BM_Code"].ToString().Trim();
                            if (!string.IsNullOrEmpty(dr["BaseLine_Date"].ToString().Trim()))
                            {
                                if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(dr["Det_POINTS"].ToString().Trim(), DataType.String, "s135");
                                else
                                    row.Cells.Add("", DataType.String, "s135");

                                foreach (MATDEFBMEntity Entity in BenchMark_List)
                                {
                                    if (dr["Det_Asmt_BM_Code"].ToString().Trim() == Entity.Code)
                                    {
                                        BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                        break;
                                    }
                                }
                                if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                    row.Cells.Add(BM_Desc.Trim(), DataType.String, "s135");
                                else row.Cells.Add("Scale Bypassed", DataType.String, "s135");
                            }
                            else
                            {
                                row.Cells.Add("", DataType.String, "s135");
                                row.Cells.Add("", DataType.String, "s135");
                            }

                            string[] assdates = dr["Ass_DateList"].ToString().Trim().Split(',');
                            string[] datelist = new string[3];
                            if (assdates.Length > 0)
                            {
                                if (assdates.Length > 1)
                                {
                                    row.Cells.Add(LookupDataAccess.Getdate1(assdates[0].ToString().Trim()), DataType.String, "s135");


                                    if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                        row.Cells.Add(assdates[1].ToString().Trim(), DataType.String, "s135");
                                    else
                                        row.Cells.Add("", DataType.String, "s135");

                                    foreach (MATDEFBMEntity Entity in BenchMark_List)
                                    {
                                        if (assdates[2].ToString().Trim() == Entity.Code)
                                        {
                                            BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                            break;
                                        }
                                    }
                                    if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                        row.Cells.Add(BM_Desc.Trim(), DataType.String, "s135");
                                    else row.Cells.Add("Scale Bypassed", DataType.String, "s135");
                                    //row.Cells.Add(BM_Desc.Trim());
                                }
                                else
                                {
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                }

                               
                            }

                            if (assdates.Length > 0)
                            {
                                if (assdates.Length > 1)
                                {
                                    if (Convert.ToDateTime(assdates[0].ToString().Trim()) > Convert.ToDateTime(dr["BaseLine_Date"].ToString().Trim()))
                                    {
                                        row.Cells.Add(LookupDataAccess.Getdate1(assdates[0].ToString().Trim()), DataType.String, "s135");
                                        if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                            row.Cells.Add(assdates[1].ToString().Trim(), DataType.String, "s135");
                                        else
                                            row.Cells.Add("", DataType.String, "s135");

                                        foreach (MATDEFBMEntity Entity in BenchMark_List)
                                        {
                                            if (assdates[2].ToString().Trim() == Entity.Code)
                                            {
                                                BM_Desc = Entity.Desc.Trim() + " (" + Entity.Low + "-" + Entity.High + ")";
                                                break;
                                            }
                                        }
                                        if (dr["Det_BYPASS"].ToString().Trim() == "N")
                                            row.Cells.Add(BM_Desc.Trim(), DataType.String, "s135");
                                        else row.Cells.Add("Scale Bypassed", DataType.String, "s135");
                                    }
                                    else
                                    {
                                        row.Cells.Add("", DataType.String, "s135");
                                        row.Cells.Add("", DataType.String, "s135");
                                        row.Cells.Add("", DataType.String, "s135");
                                    }




                                    //row.Cells.Add(BM_Desc.Trim());
                                }
                                else
                                {
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                }

                            }
                            else
                            {
                                row.Cells.Add("", DataType.String, "s135");
                                row.Cells.Add("", DataType.String, "s135");
                                row.Cells.Add("", DataType.String, "s135");
                            }





                            switch (dr["Det_Progress_Code"].ToString().Trim())
                            {
                                case "a":
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "b":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "c":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "d":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "e":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "f":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "g":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "h":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "i":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "j":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "k":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "l":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "m":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "n":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "o":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "p":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "q":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "r":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "s":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "t":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "u":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "v":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "w":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "x":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "y":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                                case "z":
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("", DataType.String, "s135");
                                    row.Cells.Add("1", DataType.String, "s135");
                                    break;
                            }


                        }

                    }
                    #endregion

                    #region Unduplicated Persons for Progression

                    if (programData.Tables[15].Rows.Count > 0)
                    {
                        DataTable dtProgress = programData.Tables[15];

                        sheet = book.Worksheets.Add("Unduplicated Person Progr.");
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(60));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        sheet.Table.Columns.Add(new WorksheetColumn(80));
                        sheet.Table.Columns.Add(new WorksheetColumn(200));

                        WorksheetRow row = sheet.Table.Rows.Add();

                        cell = row.Cells.Add("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), DataType.String, "s133");
                        cell.MergeAcross = 5;

                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("Agency", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Dept", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Program", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Year", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("AppNo", DataType.String, "s134"));
                        row.Cells.Add(new WorksheetCell("Client Name", DataType.String, "s134"));

                        foreach (DataRow dr in dtProgress.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(dr["Tmp_Agy"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Tmp_Dept"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Tmp_Prog"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Tmp_Year"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Tmp_App"].ToString().Trim(), DataType.String, "s135");
                            row.Cells.Add(dr["Client_Name"].ToString().Trim(), DataType.String, "s135");
                        }

                    }
                    #endregion

                }

                








                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
                AlertBox.Show("Report Generated Successfully");
            }
            catch (Exception ex) { }

        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            WorksheetStyle style = styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Name = "Normal 2";
            s62.Font.FontName = "Calibri";
            s62.Font.Size = 11;
            s62.Font.Color = "#000000";
            s62.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m1698893805328
            // -----------------------------------------------
            WorksheetStyle m1698893805328 = styles.Add("m1698893805328");
            m1698893805328.Parent = "s62";
            m1698893805328.Font.FontName = "Arial";
            m1698893805328.Font.Color = "#000000";
            m1698893805328.Interior.Color = "#FFFFFF";
            m1698893805328.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893805328.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893805328.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893805328.Alignment.WrapText = true;
            m1698893805328.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893805328.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893805328.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804912
            // -----------------------------------------------
            WorksheetStyle m1698893804912 = styles.Add("m1698893804912");
            m1698893804912.Parent = "s62";
            m1698893804912.Font.FontName = "Arial";
            m1698893804912.Font.Color = "#000000";
            m1698893804912.Interior.Color = "#FFFFFF";
            m1698893804912.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804912.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804912.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804912.Alignment.WrapText = true;
            m1698893804912.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804912.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804912.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804932
            // -----------------------------------------------
            WorksheetStyle m1698893804932 = styles.Add("m1698893804932");
            m1698893804932.Parent = "s62";
            m1698893804932.Font.FontName = "Arial";
            m1698893804932.Font.Color = "#000000";
            m1698893804932.Interior.Color = "#FFFFFF";
            m1698893804932.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804932.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804932.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804932.Alignment.WrapText = true;
            m1698893804932.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804932.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804932.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804952
            // -----------------------------------------------
            WorksheetStyle m1698893804952 = styles.Add("m1698893804952");
            m1698893804952.Parent = "s62";
            m1698893804952.Font.FontName = "Arial";
            m1698893804952.Font.Color = "#000000";
            m1698893804952.Interior.Color = "#FFFFFF";
            m1698893804952.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804952.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804952.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804952.Alignment.WrapText = true;
            m1698893804952.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804952.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804952.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804972
            // -----------------------------------------------
            WorksheetStyle m1698893804972 = styles.Add("m1698893804972");
            m1698893804972.Parent = "s62";
            m1698893804972.Font.FontName = "Arial";
            m1698893804972.Font.Color = "#000000";
            m1698893804972.Interior.Color = "#FFFFFF";
            m1698893804972.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804972.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804972.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804972.Alignment.WrapText = true;
            m1698893804972.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804972.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804972.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804992
            // -----------------------------------------------
            WorksheetStyle m1698893804992 = styles.Add("m1698893804992");
            m1698893804992.Parent = "s62";
            m1698893804992.Font.FontName = "Arial";
            m1698893804992.Font.Color = "#000000";
            m1698893804992.Interior.Color = "#FFFFFF";
            m1698893804992.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804992.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804992.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804992.Alignment.WrapText = true;
            m1698893804992.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804992.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804992.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893805012
            // -----------------------------------------------
            WorksheetStyle m1698893805012 = styles.Add("m1698893805012");
            m1698893805012.Parent = "s62";
            m1698893805012.Font.FontName = "Arial";
            m1698893805012.Font.Color = "#000000";
            m1698893805012.Interior.Color = "#FFFFFF";
            m1698893805012.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893805012.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893805012.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893805012.Alignment.WrapText = true;
            m1698893805012.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893805012.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893805012.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804744
            // -----------------------------------------------
            WorksheetStyle m1698893804744 = styles.Add("m1698893804744");
            m1698893804744.Parent = "s62";
            m1698893804744.Font.FontName = "Arial";
            m1698893804744.Font.Color = "#000000";
            m1698893804744.Interior.Color = "#FFFFFF";
            m1698893804744.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804744.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804744.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804744.Alignment.WrapText = true;
            m1698893804744.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804744.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804744.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804784
            // -----------------------------------------------
            WorksheetStyle m1698893804784 = styles.Add("m1698893804784");
            m1698893804784.Parent = "s62";
            m1698893804784.Font.Bold = true;
            m1698893804784.Font.FontName = "Arial";
            m1698893804784.Font.Color = "#000000";
            m1698893804784.Interior.Color = "#D3D3D3";
            m1698893804784.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804784.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m1698893804784.Alignment.Vertical = StyleVerticalAlignment.Center;
            m1698893804784.Alignment.WrapText = true;
            m1698893804784.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804784.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804784.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m1698893804804
            // -----------------------------------------------
            WorksheetStyle m1698893804804 = styles.Add("m1698893804804");
            m1698893804804.Parent = "s62";
            m1698893804804.Font.FontName = "Arial";
            m1698893804804.Font.Color = "#000000";
            m1698893804804.Interior.Color = "#FFFFFF";
            m1698893804804.Interior.Pattern = StyleInteriorPattern.Solid;
            m1698893804804.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m1698893804804.Alignment.Vertical = StyleVerticalAlignment.Top;
            m1698893804804.Alignment.WrapText = true;
            m1698893804804.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m1698893804804.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m1698893804804.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s137
            // -----------------------------------------------
            WorksheetStyle s137 = styles.Add("s137");
            s137.Name = "Normal 3";
            s137.Font.FontName = "Calibri";
            s137.Font.Size = 11;
            s137.Font.Color = "#000000";
            s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m2611536909264
            // -----------------------------------------------
            WorksheetStyle m2611536909264 = styles.Add("m2611536909264");
            m2611536909264.Parent = "s137";
            m2611536909264.Font.FontName = "Arial";
            m2611536909264.Font.Color = "#9400D3";
            m2611536909264.Interior.Color = "#FFFFFF";
            m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909264.Alignment.WrapText = true;
            m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909284
            // -----------------------------------------------
            WorksheetStyle m2611536909284 = styles.Add("m2611536909284");
            m2611536909284.Parent = "s137";
            m2611536909284.Font.FontName = "Arial";
            m2611536909284.Font.Color = "#9400D3";
            m2611536909284.Interior.Color = "#FFFFFF";
            m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909284.Alignment.WrapText = true;
            m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909304
            // -----------------------------------------------
            WorksheetStyle m2611536909304 = styles.Add("m2611536909304");
            m2611536909304.Parent = "s137";
            m2611536909304.Font.FontName = "Arial";
            m2611536909304.Font.Color = "#9400D3";
            m2611536909304.Interior.Color = "#FFFFFF";
            m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909304.Alignment.WrapText = true;
            m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909324
            // -----------------------------------------------
            WorksheetStyle m2611536909324 = styles.Add("m2611536909324");
            m2611536909324.Parent = "s137";
            m2611536909324.Font.FontName = "Arial";
            m2611536909324.Font.Color = "#9400D3";
            m2611536909324.Interior.Color = "#FFFFFF";
            m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909324.Alignment.WrapText = true;
            m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909344
            // -----------------------------------------------
            WorksheetStyle m2611536909344 = styles.Add("m2611536909344");
            m2611536909344.Parent = "s137";
            m2611536909344.Font.FontName = "Arial";
            m2611536909344.Font.Color = "#9400D3";
            m2611536909344.Interior.Color = "#FFFFFF";
            m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909344.Alignment.WrapText = true;
            m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549552
            // -----------------------------------------------
            WorksheetStyle m2611540549552 = styles.Add("m2611540549552");
            m2611540549552.Parent = "s137";
            m2611540549552.Font.FontName = "Arial";
            m2611540549552.Font.Color = "#9400D3";
            m2611540549552.Interior.Color = "#FFFFFF";
            m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549552.Alignment.WrapText = true;
            m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549572
            // -----------------------------------------------
            WorksheetStyle m2611540549572 = styles.Add("m2611540549572");
            m2611540549572.Parent = "s137";
            m2611540549572.Font.FontName = "Arial";
            m2611540549572.Font.Color = "#9400D3";
            m2611540549572.Interior.Color = "#FFFFFF";
            m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549572.Alignment.WrapText = true;
            m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549592
            // -----------------------------------------------
            WorksheetStyle m2611540549592 = styles.Add("m2611540549592");
            m2611540549592.Parent = "s137";
            m2611540549592.Font.FontName = "Arial";
            m2611540549592.Font.Color = "#9400D3";
            m2611540549592.Interior.Color = "#FFFFFF";
            m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549592.Alignment.WrapText = true;
            m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549612
            // -----------------------------------------------
            WorksheetStyle m2611540549612 = styles.Add("m2611540549612");
            m2611540549612.Parent = "s137";
            m2611540549612.Font.FontName = "Arial";
            m2611540549612.Font.Color = "#9400D3";
            m2611540549612.Interior.Color = "#FFFFFF";
            m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549612.Alignment.WrapText = true;
            m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549632
            // -----------------------------------------------
            WorksheetStyle m2611540549632 = styles.Add("m2611540549632");
            m2611540549632.Parent = "s137";
            m2611540549632.Font.FontName = "Arial";
            m2611540549632.Font.Color = "#9400D3";
            m2611540549632.Interior.Color = "#FFFFFF";
            m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549632.Alignment.WrapText = true;
            m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549652
            // -----------------------------------------------
            WorksheetStyle m2611540549652 = styles.Add("m2611540549652");
            m2611540549652.Parent = "s137";
            m2611540549652.Font.FontName = "Arial";
            m2611540549652.Font.Color = "#9400D3";
            m2611540549652.Interior.Color = "#FFFFFF";
            m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549652.Alignment.WrapText = true;
            m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549672
            // -----------------------------------------------
            WorksheetStyle m2611540549672 = styles.Add("m2611540549672");
            m2611540549672.Parent = "s137";
            m2611540549672.Font.FontName = "Arial";
            m2611540549672.Font.Color = "#9400D3";
            m2611540549672.Interior.Color = "#FFFFFF";
            m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549672.Alignment.WrapText = true;
            m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Parent = "s62";
            s63.Font.FontName = "Calibri";
            s63.Font.Size = 11;
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Parent = "s62";
            s64.Font.Bold = true;
            s64.Font.FontName = "Calibri";
            s64.Font.Size = 11;
            s64.Font.Color = "#FF0000";
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Parent = "s62";
            s65.Font.Bold = true;
            s65.Font.FontName = "Arial";
            s65.Font.Color = "#7030A0";
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s65.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Parent = "s62";
            s66.Font.FontName = "Calibri";
            s66.Font.Size = 11;
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s66.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Parent = "s62";
            s67.Font.FontName = "Calibri";
            s67.Font.Size = 11;
            s67.Font.Color = "#7030A0";
            s67.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s67.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Parent = "s62";
            s68.Font.FontName = "Calibri";
            s68.Font.Size = 11;
            s68.Font.Color = "#7030A0";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Parent = "s62";
            s69.Font.FontName = "Calibri";
            s69.Font.Size = 11;
            s69.Font.Color = "#7030A0";
            s69.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s69.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Parent = "s62";
            s70.Font.FontName = "Calibri";
            s70.Font.Size = 11;
            s70.Interior.Color = "#FFFFFF";
            s70.Interior.Pattern = StyleInteriorPattern.Solid;
            s70.Alignment.Vertical = StyleVerticalAlignment.Top;
            s70.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Parent = "s62";
            s71.Font.Bold = true;
            s71.Font.FontName = "Arial";
            s71.Font.Color = "#000000";
            s71.Interior.Color = "#D3D3D3";
            s71.Interior.Pattern = StyleInteriorPattern.Solid;
            s71.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s71.Alignment.Vertical = StyleVerticalAlignment.Center;
            s71.Alignment.WrapText = true;
            s71.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s71.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s71.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.Parent = "s62";
            s73.Font.Bold = true;
            s73.Font.FontName = "Arial";
            s73.Font.Color = "#000000";
            s73.Interior.Color = "#B0C4DE";
            s73.Interior.Pattern = StyleInteriorPattern.Solid;
            s73.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s73.Alignment.Vertical = StyleVerticalAlignment.Center;
            s73.Alignment.WrapText = true;
            s73.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s73.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s73.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Parent = "s62";
            s74.Font.Bold = true;
            s74.Font.FontName = "Arial";
            s74.Font.Color = "#000000";
            s74.Interior.Color = "#B0C4DE";
            s74.Interior.Pattern = StyleInteriorPattern.Solid;
            s74.Alignment.Rotate = -90;
            s74.Alignment.Vertical = StyleVerticalAlignment.Top;
            s74.Alignment.WrapText = true;
            s74.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s74.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s74.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Parent = "s62";
            s75.Font.FontName = "Arial";
            s75.Font.Color = "#000000";
            s75.Interior.Color = "#FFFFFF";
            s75.Interior.Pattern = StyleInteriorPattern.Solid;
            s75.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s75.Alignment.Vertical = StyleVerticalAlignment.Top;
            s75.Alignment.WrapText = true;
            s75.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Parent = "s62";
            s76.Font.FontName = "Arial";
            s76.Font.Color = "#000000";
            s76.Interior.Color = "#FFFFFF";
            s76.Interior.Pattern = StyleInteriorPattern.Solid;
            s76.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s76.Alignment.Vertical = StyleVerticalAlignment.Top;
            s76.Alignment.WrapText = true;
            s76.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Parent = "s62";
            s77.Font.FontName = "Arial";
            s77.Font.Color = "#000000";
            s77.Interior.Color = "#FFFFFF";
            s77.Interior.Pattern = StyleInteriorPattern.Solid;
            s77.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s77.Alignment.Vertical = StyleVerticalAlignment.Top;
            s77.Alignment.WrapText = true;
            s77.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Parent = "s62";
            s79.Font.FontName = "Arial";
            s79.Font.Color = "#000000";
            s79.Interior.Color = "#FFFFFF";
            s79.Interior.Pattern = StyleInteriorPattern.Solid;
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s79.Alignment.Vertical = StyleVerticalAlignment.Top;
            s79.Alignment.WrapText = true;
            s79.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s79.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s80
            // -----------------------------------------------
            WorksheetStyle s80 = styles.Add("s80");
            s80.Parent = "s62";
            s80.Font.FontName = "Arial";
            s80.Font.Color = "#000000";
            s80.Interior.Color = "#FFFFFF";
            s80.Interior.Pattern = StyleInteriorPattern.Solid;
            s80.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s80.Alignment.Vertical = StyleVerticalAlignment.Top;
            s80.Alignment.WrapText = true;
            s80.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Parent = "s62";
            s81.Font.FontName = "Calibri";
            s81.Font.Size = 11;
            s81.Interior.Color = "#FFFFFF";
            s81.Interior.Pattern = StyleInteriorPattern.Solid;
            s81.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s81.Alignment.Vertical = StyleVerticalAlignment.Top;
            s81.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Parent = "s62";
            s82.Font.FontName = "Arial";
            s82.Font.Color = "#000000";
            s82.Interior.Color = "#FFFFFF";
            s82.Interior.Pattern = StyleInteriorPattern.Solid;
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s82.Alignment.Vertical = StyleVerticalAlignment.Top;
            s82.Alignment.WrapText = true;
            s82.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s83
            // -----------------------------------------------
            WorksheetStyle s83 = styles.Add("s83");
            s83.Parent = "s62";
            s83.Font.Bold = true;
            s83.Font.FontName = "Arial";
            s83.Font.Color = "#000000";
            s83.Interior.Color = "#D3D3D3";
            s83.Interior.Pattern = StyleInteriorPattern.Solid;
            s83.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s83.Alignment.Vertical = StyleVerticalAlignment.Center;
            s83.Alignment.WrapText = true;
            s83.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s83.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s83.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s83.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s85
            // -----------------------------------------------
            WorksheetStyle s85 = styles.Add("s85");
            s85.Parent = "s62";
            s85.Font.FontName = "Arial";
            s85.Font.Color = "#000000";
            s85.Interior.Color = "#FFFFFF";
            s85.Interior.Pattern = StyleInteriorPattern.Solid;
            s85.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s85.Alignment.Vertical = StyleVerticalAlignment.Top;
            s85.Alignment.WrapText = true;
            s85.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s85.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Parent = "s62";
            s93.Font.FontName = "Calibri";
            s93.Font.Size = 11;
            s93.Interior.Color = "#FFFFFF";
            s93.Interior.Pattern = StyleInteriorPattern.Solid;
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s93.Alignment.Vertical = StyleVerticalAlignment.Top;
            s93.Alignment.WrapText = true;
            s93.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Parent = "s62";
            s95.Font.Bold = true;
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#D3D3D3";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Parent = "s62";
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s133
            // -----------------------------------------------
            WorksheetStyle s133 = styles.Add("s133");
            s133.Font.Bold = true;
            s133.Font.FontName = "Tahoma";
            s133.Font.Size = 12;
            s133.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s133.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s134
            // -----------------------------------------------
            WorksheetStyle s134 = styles.Add("s134");
            s134.Font.Bold = true;
            s134.Font.FontName = "Tahoma";
            s134.Font.Size = 12;
            s134.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s134.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s135
            // -----------------------------------------------
            WorksheetStyle s135 = styles.Add("s135");
            s135.Font.FontName = "Tahoma";
            // -----------------------------------------------
            //  s136
            // -----------------------------------------------
            WorksheetStyle s136 = styles.Add("s136");
            s136.Font.FontName = "Tahoma";
            s136.Font.Color = "#0000FF";
            s136.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s136.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s139
            // -----------------------------------------------
            WorksheetStyle s139 = styles.Add("s139");
            s139.Parent = "s137";
            s139.Font.FontName = "Calibri";
            s139.Font.Size = 11;
            s139.Interior.Color = "#FFFFFF";
            s139.Interior.Pattern = StyleInteriorPattern.Solid;
            s139.Alignment.Vertical = StyleVerticalAlignment.Top;
            s139.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s140
            // -----------------------------------------------
            WorksheetStyle s140 = styles.Add("s140");
            s140.Parent = "s137";
            s140.Font.FontName = "Calibri";
            s140.Font.Size = 11;
            s140.Interior.Color = "#FFFFFF";
            s140.Interior.Pattern = StyleInteriorPattern.Solid;
            s140.Alignment.Vertical = StyleVerticalAlignment.Top;
            s140.Alignment.WrapText = true;
            s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s141
            // -----------------------------------------------
            WorksheetStyle s141 = styles.Add("s141");
            s141.Parent = "s137";
            s141.Font.FontName = "Calibri";
            s141.Font.Size = 11;
            s141.Interior.Color = "#FFFFFF";
            s141.Interior.Pattern = StyleInteriorPattern.Solid;
            s141.Alignment.Vertical = StyleVerticalAlignment.Top;
            s141.Alignment.WrapText = true;
            s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s142
            // -----------------------------------------------
            WorksheetStyle s142 = styles.Add("s142");
            s142.Parent = "s137";
            s142.Font.FontName = "Calibri";
            s142.Font.Size = 11;
            s142.Interior.Color = "#FFFFFF";
            s142.Interior.Pattern = StyleInteriorPattern.Solid;
            s142.Alignment.Vertical = StyleVerticalAlignment.Top;
            s142.Alignment.WrapText = true;
            s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s143
            // -----------------------------------------------
            WorksheetStyle s143 = styles.Add("s143");
            s143.Parent = "s137";
            s143.Font.FontName = "Calibri";
            s143.Font.Size = 11;
            s143.Interior.Color = "#FFFFFF";
            s143.Interior.Pattern = StyleInteriorPattern.Solid;
            s143.Alignment.Vertical = StyleVerticalAlignment.Top;
            s143.Alignment.WrapText = true;
            s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s144
            // -----------------------------------------------
            WorksheetStyle s144 = styles.Add("s144");
            s144.Parent = "s137";
            s144.Font.FontName = "Arial";
            s144.Font.Color = "#9400D3";
            s144.Interior.Color = "#FFFFFF";
            s144.Interior.Pattern = StyleInteriorPattern.Solid;
            s144.Alignment.Vertical = StyleVerticalAlignment.Top;
            s144.Alignment.WrapText = true;
            s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = styles.Add("s145");
            s145.Parent = "s137";
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Interior.Color = "#FFFFFF";
            s145.Interior.Pattern = StyleInteriorPattern.Solid;
            s145.Alignment.Vertical = StyleVerticalAlignment.Top;
            s145.Alignment.WrapText = true;
            s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s146
            // -----------------------------------------------
            WorksheetStyle s146 = styles.Add("s146");
            s146.Parent = "s137";
            s146.Font.FontName = "Calibri";
            s146.Font.Size = 11;
            s146.Interior.Color = "#FFFFFF";
            s146.Interior.Pattern = StyleInteriorPattern.Solid;
            s146.Alignment.Vertical = StyleVerticalAlignment.Top;
            s146.Alignment.WrapText = true;
            s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s169
            // -----------------------------------------------
            WorksheetStyle s169 = styles.Add("s169");
            s169.Parent = "s137";
            s169.Font.FontName = "Arial";
            s169.Font.Color = "#9400D3";
            s169.Interior.Color = "#FFFFFF";
            s169.Interior.Pattern = StyleInteriorPattern.Solid;
            s169.Alignment.Vertical = StyleVerticalAlignment.Top;
            s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170 = styles.Add("s170");
            s170.Parent = "s137";
            s170.Font.FontName = "Calibri";
            s170.Font.Size = 11;
            s170.Interior.Color = "#FFFFFF";
            s170.Interior.Pattern = StyleInteriorPattern.Solid;
            s170.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = styles.Add("s171");
            s171.Parent = "s137";
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 11;
            s171.Interior.Color = "#FFFFFF";
            s171.Interior.Pattern = StyleInteriorPattern.Solid;
            s171.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172 = styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

        }




        //private void OnExcel_Report(DataTable table, DataTable Scale_Table)
        //{
        //    string propReportPath = _model.lookupDataAccess.GetReportPath(BaseForm.BaseAgency);
        //    string PdfName = "Pdf File";
        //    PdfName = "MATB0002_Details.xls";
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

        //    string data = null;

        //    ExcelDocument xlWorkSheet = new ExcelDocument();

        //    xlWorkSheet.ColumnWidth(0, 0);
        //    xlWorkSheet.ColumnWidth(1, 70);
        //    xlWorkSheet.ColumnWidth(2, 70);
        //    xlWorkSheet.ColumnWidth(3, 70);
        //    xlWorkSheet.ColumnWidth(4, 70);
        //    xlWorkSheet.ColumnWidth(5, 90);
        //    xlWorkSheet.ColumnWidth(6, 200);
        //    xlWorkSheet.ColumnWidth(7, 100);
        //    xlWorkSheet.ColumnWidth(8, 130);
        //    //xlWorkSheet.ColumnWidth(9, 65);
        //    //xlWorkSheet.ColumnWidth(10, 120);
        //    //xlWorkSheet.ColumnWidth(11, 100);
        //    //xlWorkSheet.ColumnWidth(12, 65);
        //    //xlWorkSheet.ColumnWidth(13, 120);
        //    ////xlWorkSheet.ColumnWidth(12, 250);
        //    int Row = 13;
        //    int excelcolumn = 0;

        //    try
        //    {
        //        if (table.Rows.Count > 0)
        //        {
        //            excelcolumn = excelcolumn + 1;


        //            xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Left;
        //            xlWorkSheet.WriteCell(excelcolumn, 1, "Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim());

        //            //excelcolumn = excelcolumn + 1;
        //            //int HeaderColumn = excelcolumn;
        //            //xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            //xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Left;
        //            //xlWorkSheet.WriteCell(excelcolumn, 8, "Baseline   ");

        //            //xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            //xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Left;
        //            //xlWorkSheet.WriteCell(excelcolumn, 11, "Assessment 1   ");


        //            excelcolumn = excelcolumn + 1;
        //            string[] HeaderSeq4 = { "Agency", "Dept", "Program", "Year", "AppNo", "Client Name", "Base Range", "Assessment Range" };
        //            for (int i = 0; i < HeaderSeq4.Length; ++i)
        //            {
        //                xlWorkSheet[excelcolumn, (i + 1)].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //                xlWorkSheet[excelcolumn, (i + 1)].Alignment = Alignment.Centered;
        //                xlWorkSheet.WriteCell(excelcolumn, (i + 1), HeaderSeq4[i]);
        //            }

        //            bool first = true; int length = 0;
        //            foreach (DataRow dr in table.Rows)
        //            {
        //                excelcolumn = excelcolumn + 1;

        //                xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 1, dr["Det_Agy"].ToString().Trim());


        //                xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 2, dr["Det_Dept"].ToString().Trim());

        //                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 3, dr["Det_Prog"].ToString().Trim());

        //                xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 4, dr["Det_Year"].ToString().Trim());

        //                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Left;
        //                xlWorkSheet.WriteCell(excelcolumn, 5, dr["Det_App"].ToString().Trim());

        //                xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Left;
        //                xlWorkSheet.WriteCell(excelcolumn, 6, dr["Client_Name"].ToString().Trim());

        //                //xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Left;
        //                //xlWorkSheet.WriteCell(excelcolumn, 7, ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim());




        //                //string Scale_Desc = dr["Det_Scale_Code"].ToString().Trim();
        //                //if (Scale_Table.Rows.Count > 0)
        //                //{
        //                //    foreach (DataRow drscale in Scale_Table.Rows)
        //                //    {
        //                //        if (dr["Det_Scale_Code"].ToString().Trim() == drscale["BM_Scale"].ToString().Trim())
        //                //        {
        //                //            Scale_Desc = drscale["BM_Scale_Desc"].ToString().Trim(); break;
        //                //        }
        //                //    }
        //                //}

        //                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Left;
        //                xlWorkSheet.WriteCell(excelcolumn, 7, dr["Det_BaseRange"].ToString().Trim());

        //                //xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Right;
        //                //xlWorkSheet.WriteCell(excelcolumn, 9, dr["Det_POINTS"].ToString().Trim());

        //                xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Left;
        //                xlWorkSheet.WriteCell(excelcolumn, 8, dr["Det_AssmentRange"].ToString().Trim());

        //            }

        //            FileStream stream = new FileStream(PdfName, FileMode.Create);

        //            xlWorkSheet.Save(stream);
        //            stream.Close();

        //        }
        //    }
        //    catch (Exception ex) { }




        //}

        #endregion


        //--------------------------------------------------------------------------------------------------------------

        private void Dynamic_Sub_RDLC()
        {

            //Get_Report_Selection_Parameters();

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

            foreach (DG_ResTab_Entity Entity in DG_SubTable_List)
            {
                XmlElement Field = xml.CreateElement("Field");
                Field.SetAttribute("Name", Entity.Column_Name);
                Fields.AppendChild(Field);

                XmlElement DataField = xml.CreateElement("DataField");
                DataField.InnerText = Entity.Column_Name;
                Field.AppendChild(DataField);
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

            double Total_Sel_TextBox_Height = 0.016667;
            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
            Sel_Rect_Textbox1.SetAttribute("Name", "BtnTot_Textbox");
            ReportItems.AppendChild(Sel_Rect_Textbox1);

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
            Textbox1_TextRun_Value.InnerText = "2. Assessment Benchmark Level";
            Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


            XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
            Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

            XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
            Return_Style_FontWeight.InnerText = "Bold";
            Textbox1_TextRun_Style.AppendChild(Return_Style_FontWeight);

            XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            Textbox1_TextRun_Style_Color.InnerText = "Black";
            Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);

            XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
            Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);

            //Total_Sel_TextBox_Height += 0.25;

            XmlElement Textbox1_Top = xml.CreateElement("Top");
            //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
            Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Top);
            Total_Sel_TextBox_Height += 0.21855;

            XmlElement Textbox1_Left = xml.CreateElement("Left");
            Textbox1_Left.InnerText = "0.19792in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

            XmlElement Textbox1_Height = xml.CreateElement("Height");
            Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

            XmlElement Textbox1_Width = xml.CreateElement("Width");
            Textbox1_Width.InnerText = "4.62708in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

            XmlElement Textbox1_Style = xml.CreateElement("Style");
            Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

            XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
            Textbox1_Style.AppendChild(Textbox1_Style_Border);

            XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
            Textbox1_Style_Border_Style.InnerText = "Solid";
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

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>


            //double Total_Sel_TextBox_Height = 0.16667;

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            foreach (DG_ResTab_Entity Entity in DG_Table_List)                      // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
                    //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
                }
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height"); // Column Headers Height
            //Row_Height.InnerText = "2in";//"0.25in";
            //Row_Height.InnerText = "0.25in";
            Row_Height.InnerText = "0.025in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (DG_ResTab_Entity Entity in DG_Table_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
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

                    Tmp_Disp_Column_Name = Entity.Disp_Name;


                    //Disp_Col_Substring_Len = 6;

                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                    Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);


                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Center";
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);


                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Return_Style_FontWeight_1 = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight_1.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight_1);

                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    //XmlElement Border_Color = xml.CreateElement("Color");
                    //Border_Color.InnerText = "Black";//"LightGrey";
                    //Cell_Border.AppendChild(Border_Color);

                    ////XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    ////Border_Style.InnerText = "Solid";
                    ////Cell_Border.AppendChild(Border_Style);

                    //XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    //Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


                    //XmlElement Head_VerticalAlign = xml.CreateElement("VerticalAlign");
                    //Head_VerticalAlign.InnerText = "Middle";
                    //Cell_style.AppendChild(Head_VerticalAlign);

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

                    //XmlElement Head_WritingMode = xml.CreateElement("WritingMode");
                    //Head_WritingMode.InnerText = "Vertical";
                    //Cell_style.AppendChild(Head_WritingMode);
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
            foreach (DG_ResTab_Entity Entity in DG_Table_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    XmlElement Textbox = xml.CreateElement("Textbox");
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
                    if (Privileges.ModuleCode == "03")
                    {
                        if (Entity.Text_Align == "R")
                            Field_Value = "=CInt(Fields!" + Entity.Column_Name + ".Value)";
                        else
                            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    }
                    else if (Privileges.ModuleCode == "02")
                    {
                        //uncommented the three lines in the MATB1002
                        if (Entity.Text_Align == "R")
                            Field_Value = "=CInt(Fields!" + Entity.Column_Name + ".Value)";
                        else
                            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    }
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black"; // "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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

            for (int Loop = 0; Loop < DG_SubTable_List.Count; Loop++)            // Dynamic based on Display Columns in 3/6 
            {
                XmlElement TablixMember = xml.CreateElement("TablixMember");
                Tablix_Col_Members.AppendChild(TablixMember);
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

            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            //Tablix.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "End";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            Top.InnerText = (Total_Sel_TextBox_Height).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20517in";
            //Left.InnerText = "0.20417in";
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


            //   Subreport
            ////////if (Summary_Sw)
            ////////{
            ////////    // Summary Sub Report 
            ////////}

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            ////<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>
            //for (int i = 0; i < 2; i++)
            //{
            //    XmlElement RepTot_Textbox1 = xml.CreateElement("Textbox");
            //    RepTot_Textbox1.SetAttribute("Name", "BtnTot_Textbox_Rep" + i.ToString());
            //    ReportItems.AppendChild(RepTot_Textbox1);

            //    XmlElement RepTot_Cangrow = xml.CreateElement("CanGrow");
            //    RepTot_Cangrow.InnerText = "true";
            //    RepTot_Textbox1.AppendChild(RepTot_Cangrow);

            //    XmlElement RepTot_Keep = xml.CreateElement("KeepTogether");
            //    RepTot_Keep.InnerText = "true";
            //    RepTot_Textbox1.AppendChild(RepTot_Keep);

            //    XmlElement RepTot_Paragraphs = xml.CreateElement("Paragraphs");
            //    RepTot_Textbox1.AppendChild(RepTot_Paragraphs);

            //    XmlElement RepTot_Paragraph = xml.CreateElement("Paragraph");
            //    RepTot_Paragraphs.AppendChild(RepTot_Paragraph);

            //    XmlElement RepTot_TextRuns = xml.CreateElement("TextRuns");
            //    RepTot_Paragraph.AppendChild(RepTot_TextRuns);

            //    XmlElement RepTot_TextRun = xml.CreateElement("TextRun");
            //    RepTot_TextRuns.AppendChild(RepTot_TextRun);

            //    XmlElement RepTot_TextRun_Value = xml.CreateElement("Value"); //ppppppppppp
            //    RepTot_TextRun_Value.InnerText = (i == 0 ? "Total number of persons assessed during reporting period " : RepPeriod_Clients_Cnt);
            //    RepTot_TextRun.AppendChild(RepTot_TextRun_Value);

            //    //XmlElement Cell_Align = xml.CreateElement("Style");
            //    //XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");        
            //    //Cell_TextAlign.InnerText = "Right";//"Center";
            //    //Cell_Align.AppendChild(Cell_TextAlign);
            //    //RepTot_Paragraph.AppendChild(Cell_Align);

            //    XmlElement RepTot_TextRun_Style = xml.CreateElement("Style");
            //    RepTot_TextRun.AppendChild(RepTot_TextRun_Style);

            //    XmlElement RepTotReturn_Style_FontWeight = xml.CreateElement("FontWeight");
            //    RepTotReturn_Style_FontWeight.InnerText = "Bold";
            //    RepTot_TextRun_Style.AppendChild(RepTotReturn_Style_FontWeight);

            //    XmlElement RepTot_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            //    RepTot_TextRun_Style_Color.InnerText = "Black";
            //    RepTot_TextRun_Style.AppendChild(RepTot_TextRun_Style_Color);

            //    //XmlElement RepTot_Paragraph_Style = xml.CreateElement("Style");
            //    //RepTot_Paragraph.AppendChild(RepTot_Paragraph_Style);

            //    if (i == 1)
            //    {
            //        XmlElement Cell_Align = xml.CreateElement("Style");
            //        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
            //        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
            //        Cell_TextAlign.InnerText = Text_Align;
            //        Cell_Align.AppendChild(Cell_TextAlign);
            //        RepTot_Paragraph.AppendChild(Cell_Align);
            //    }


            //    //Total_Sel_TextBox_Height += 0.25;

            //    XmlElement RepTot_Top = xml.CreateElement("Top");
            //    RepTot_Top.InnerText = (Total_Sel_TextBox_Height + 0.3).ToString() + "in";
            //    RepTot_Textbox1.AppendChild(RepTot_Top);

            //    //Total_Sel_TextBox_Height += 1.35;

            //    XmlElement RepTot_Left = xml.CreateElement("Left");
            //    RepTot_Left.InnerText = (i == 0 ? "0.19792in" : "4.87654in");
            //    RepTot_Textbox1.AppendChild(RepTot_Left);

            //    XmlElement RepTot_Height = xml.CreateElement("Height");
            //    RepTot_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            //    RepTot_Textbox1.AppendChild(RepTot_Height);

            //    XmlElement RepTot_Width = xml.CreateElement("Width");
            //    RepTot_Width.InnerText = (i == 0 ? "4.62708in" : "0.42461in");
            //    RepTot_Textbox1.AppendChild(RepTot_Width);

            //    XmlElement RepTot_Style = xml.CreateElement("Style");
            //    RepTot_Textbox1.AppendChild(RepTot_Style);

            //    XmlElement RepTot_Style_Border = xml.CreateElement("Border");
            //    RepTot_Style.AppendChild(RepTot_Style_Border);

            //    XmlElement RepTot_Style_Border_Style = xml.CreateElement("Style");
            //    RepTot_Style_Border_Style.InnerText = "None";
            //    RepTot_Style_Border.AppendChild(RepTot_Style_Border_Style);

            //    //XmlElement RepTot_Style_BackColor = xml.CreateElement("BackgroundColor");
            //    //RepTot_Style_BackColor.InnerText = "LightGrey";
            //    //RepTot_Style.AppendChild(RepTot_Style_BackColor);

            //    XmlElement RepTot_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
            //    RepTot_Style_PaddingLeft.InnerText = "2pt";
            //    RepTot_Style.AppendChild(RepTot_Style_PaddingLeft);

            //    XmlElement RepTot_Style_PaddingRight = xml.CreateElement("PaddingRight");
            //    RepTot_Style_PaddingRight.InnerText = "2pt";
            //    RepTot_Style.AppendChild(RepTot_Style_PaddingRight);

            //    XmlElement RepTot_Style_PaddingTop = xml.CreateElement("PaddingTop");
            //    RepTot_Style_PaddingTop.InnerText = "2pt";
            //    RepTot_Style.AppendChild(RepTot_Style_PaddingTop);

            //    XmlElement RepTot_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
            //    RepTot_Style_PaddingTop.InnerText = "2pt";
            //    RepTot_Style.AppendChild(RepTot_Style_PaddingBottom);
            //}
            //Total_Sel_TextBox_Height += 0.8;

            ////
            //for (int i = 0; i < 5; i++)
            //{
            //    XmlElement Suff_Head_Textbox1 = xml.CreateElement("Textbox");
            //    Suff_Head_Textbox1.SetAttribute("Name", "Suff_Head_Textbox" + i.ToString());
            //    ReportItems.AppendChild(Suff_Head_Textbox1);

            //    XmlElement SuffHead_Textbox_Cangrow = xml.CreateElement("CanGrow");
            //    SuffHead_Textbox_Cangrow.InnerText = "true";
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Cangrow);

            //    XmlElement SuffHead_Textbox_Keep = xml.CreateElement("KeepTogether");
            //    SuffHead_Textbox_Keep.InnerText = "true";
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Keep);

            //    XmlElement SuffHead_Textbox_Paragraphs = xml.CreateElement("Paragraphs");
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Paragraphs);

            //    XmlElement SuffHead_Textbox_Paragraph = xml.CreateElement("Paragraph");
            //    SuffHead_Textbox_Paragraphs.AppendChild(SuffHead_Textbox_Paragraph);

            //    XmlElement SuffHead_Textbox_TextRuns = xml.CreateElement("TextRuns");
            //    SuffHead_Textbox_Paragraph.AppendChild(SuffHead_Textbox_TextRuns);

            //    XmlElement SuffHead_Textbox_TextRun = xml.CreateElement("TextRun");
            //    SuffHead_Textbox_TextRuns.AppendChild(SuffHead_Textbox_TextRun);

            //    XmlElement SuffHead_Textbox_TextRun_Value = xml.CreateElement("Value");

            //    switch (i)
            //    {
            //        case 0: SuffHead_Textbox_TextRun_Value.InnerText = "3. Customers making progress towards Self-Sufficiency "; break;
            //        case 1: SuffHead_Textbox_TextRun_Value.InnerText = "Total number of persons who made progress towards Self-Sufficiency: "; break;
            //        case 2: SuffHead_Textbox_TextRun_Value.InnerText = Progress_Clients_Cnt; break;
            //        case 3: SuffHead_Textbox_TextRun_Value.InnerText = "Total number of persons who achieved Self-Sufficiency: "; break;
            //        case 4: SuffHead_Textbox_TextRun_Value.InnerText = Stable_Clients_Cnt; break;
            //        default: SuffHead_Textbox_TextRun_Value.InnerText = " "; break;
            //    }
            //    SuffHead_Textbox_TextRun.AppendChild(SuffHead_Textbox_TextRun_Value);

            //    XmlElement SuffHead_Textbox_TextRun_Style = xml.CreateElement("Style");
            //    SuffHead_Textbox_TextRun.AppendChild(SuffHead_Textbox_TextRun_Style);

            //    if (i == 0)
            //    {
            //        XmlElement SuffHead_TextboxReturn_Style_FontWeight = xml.CreateElement("FontWeight");
            //        SuffHead_TextboxReturn_Style_FontWeight.InnerText = "Bold";
            //        SuffHead_Textbox_TextRun_Style.AppendChild(SuffHead_TextboxReturn_Style_FontWeight);
            //    }

            //    XmlElement SuffHead_Textbox_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            //    SuffHead_Textbox_TextRun_Style_Color.InnerText = "Black";
            //    SuffHead_Textbox_TextRun_Style.AppendChild(SuffHead_Textbox_TextRun_Style_Color);

            //    //XmlElement SuffHead_Textbox_Paragraph_Style = xml.CreateElement("Style");
            //    //SuffHead_Textbox_Paragraph.AppendChild(SuffHead_Textbox_Paragraph_Style);
            //    if (i == 2 || i == 4)
            //    {
            //        XmlElement Cell_Align = xml.CreateElement("Style");
            //        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
            //        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
            //        Cell_TextAlign.InnerText = Text_Align;
            //        Cell_Align.AppendChild(Cell_TextAlign);
            //        SuffHead_Textbox_Paragraph.AppendChild(Cell_Align);
            //    }


            //    //Total_Sel_TextBox_Height += 0.25;

            //    XmlElement SuffHead_Textbox_Top = xml.CreateElement("Top");
            //    //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //    //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
            //    SuffHead_Textbox_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Top);
            //    if (i == 0)
            //        Total_Sel_TextBox_Height += 0.21855;
            //    else
            //        if (i == 2)
            //        Total_Sel_TextBox_Height += 0.31855;

            //    XmlElement SuffHead_Textbox_Left = xml.CreateElement("Left");
            //    switch (i)
            //    {
            //        case 0: SuffHead_Textbox_Left.InnerText = "0.19792in"; break;
            //        case 1:
            //        case 3: SuffHead_Textbox_Left.InnerText = "0.29792in"; break;
            //        default: SuffHead_Textbox_Left.InnerText = "4.99792in"; break;
            //    }
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Left);

            //    XmlElement SuffHead_Textbox_Height = xml.CreateElement("Height");
            //    SuffHead_Textbox_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Height);

            //    XmlElement SuffHead_Textbox_Width = xml.CreateElement("Width");
            //    if (i == 0 || i == 1 || i == 3)
            //        SuffHead_Textbox_Width.InnerText = "4.52708in";
            //    else
            //        SuffHead_Textbox_Width.InnerText = "0.42461in";
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Width);

            //    XmlElement SuffHead_Textbox_Style = xml.CreateElement("Style");
            //    Suff_Head_Textbox1.AppendChild(SuffHead_Textbox_Style);

            //    XmlElement SuffHead_Textbox_Style_Border = xml.CreateElement("Border");
            //    SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_Border);

            //    if (i == 0)
            //    {
            //        XmlElement SuffHead_Textbox_Style_Border_Style = xml.CreateElement("Style");
            //        SuffHead_Textbox_Style_Border_Style.InnerText = "Solid";
            //        SuffHead_Textbox_Style_Border.AppendChild(SuffHead_Textbox_Style_Border_Style);

            //        XmlElement SuffHead_Textbox_Style_BackColor = xml.CreateElement("BackgroundColor");
            //        SuffHead_Textbox_Style_BackColor.InnerText = "LightGrey";
            //        SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_BackColor);
            //    }

            //    XmlElement SuffHead_Textbox_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
            //    SuffHead_Textbox_Style_PaddingLeft.InnerText = "2pt";
            //    SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_PaddingLeft);

            //    XmlElement SuffHead_Textbox_Style_PaddingRight = xml.CreateElement("PaddingRight");
            //    SuffHead_Textbox_Style_PaddingRight.InnerText = "2pt";
            //    SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_PaddingRight);

            //    XmlElement SuffHead_Textbox_Style_PaddingTop = xml.CreateElement("PaddingTop");
            //    SuffHead_Textbox_Style_PaddingTop.InnerText = "2pt";
            //    SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_PaddingTop);

            //    XmlElement SuffHead_Textbox_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
            //    SuffHead_Textbox_Style_PaddingTop.InnerText = "2pt";
            //    SuffHead_Textbox_Style.AppendChild(SuffHead_Textbox_Style_PaddingBottom);
            //}
            ////





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

            //Rep_Header_Title = " Test Report";
            //if (true && !string.IsNullOrEmpty(Rep_Header_Title.Trim())) //Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            //{
            //}

            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            //if (false) //Include_Footer
            //{
            //}

            //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


            XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
            XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

            //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
            //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            if (false) //(Rb_A4_Port.Checked)
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

                xml.Save(ReportPath + Rep_Name + "SubReport.rdlc");
                //xml.Save(@"C:\Capreports\" + Rep_Name + "SubReport.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            Console.ReadLine();
        }

        private void Dynamic_Sub_RDLC1()
        {

            //Get_Report_Selection_Parameters();

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

            foreach (DG_ResTab_Entity Entity in DG_SubTable_List)
            {
                XmlElement Field = xml.CreateElement("Field");
                Field.SetAttribute("Name", Entity.Column_Name);
                Fields.AppendChild(Field);

                XmlElement DataField = xml.CreateElement("DataField");
                DataField.InnerText = Entity.Column_Name;
                Field.AppendChild(DataField);
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

            double Total_Sel_TextBox_Height = 0.016667;
            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
            Sel_Rect_Textbox1.SetAttribute("Name", "BtnTot_Textbox");
            ReportItems.AppendChild(Sel_Rect_Textbox1);

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
            Textbox1_TextRun_Value.InnerText = "3. Follow-up Progress ";
            Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


            XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
            Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

            XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
            Return_Style_FontWeight.InnerText = "Bold";
            Textbox1_TextRun_Style.AppendChild(Return_Style_FontWeight);

            XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            Textbox1_TextRun_Style_Color.InnerText = "Black";
            Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);

            XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
            Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);

            //Total_Sel_TextBox_Height += 0.25;

            XmlElement Textbox1_Top = xml.CreateElement("Top");
            //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
            Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Top);
            Total_Sel_TextBox_Height += 0.21855;

            XmlElement Textbox1_Left = xml.CreateElement("Left");
            Textbox1_Left.InnerText = "0.19792in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

            XmlElement Textbox1_Height = xml.CreateElement("Height");
            Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

            XmlElement Textbox1_Width = xml.CreateElement("Width");
            Textbox1_Width.InnerText = "4.62708in";
            Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

            XmlElement Textbox1_Style = xml.CreateElement("Style");
            Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

            XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
            Textbox1_Style.AppendChild(Textbox1_Style_Border);

            XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
            Textbox1_Style_Border_Style.InnerText = "Solid";
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

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>


            //double Total_Sel_TextBox_Height = 0.16667;

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            foreach (DG_ResTab_Entity Entity in DG_SubTable_List)                      // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
                    //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
                }
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height"); // Column Headers Height
            //Row_Height.InnerText = "2in";//"0.25in";
            //Row_Height.InnerText = "0.25in";
            Row_Height.InnerText = "0.025in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (DG_ResTab_Entity Entity in DG_SubTable_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
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

                    Tmp_Disp_Column_Name = Entity.Disp_Name;


                    //Disp_Col_Substring_Len = 6;

                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                    Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);


                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Center";
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);


                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Return_Style_FontWeight_1 = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight_1.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight_1);

                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    //XmlElement Border_Color = xml.CreateElement("Color");
                    //Border_Color.InnerText = "Black";//"LightGrey";
                    //Cell_Border.AppendChild(Border_Color);

                    ////XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    ////Border_Style.InnerText = "Solid";
                    ////Cell_Border.AppendChild(Border_Style);

                    //XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    //Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


                    //XmlElement Head_VerticalAlign = xml.CreateElement("VerticalAlign");
                    //Head_VerticalAlign.InnerText = "Middle";
                    //Cell_style.AppendChild(Head_VerticalAlign);

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

                    //XmlElement Head_WritingMode = xml.CreateElement("WritingMode");
                    //Head_WritingMode.InnerText = "Vertical";
                    //Cell_style.AppendChild(Head_WritingMode);
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
            foreach (DG_ResTab_Entity Entity in DG_SubTable_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    XmlElement Textbox = xml.CreateElement("Textbox");
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
                    if (Privileges.ModuleCode == "03")
                    {
                        if (Entity.Text_Align == "R")
                            Field_Value = "=CInt(Fields!" + Entity.Column_Name + ".Value)";
                        else
                            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    }
                    else if (Privileges.ModuleCode == "02")
                    {
                        //Uncommented these 3 lines on 02/09/2021
                        if (Entity.Text_Align == "R")
                            Field_Value = "=CInt(Fields!" + Entity.Column_Name + ".Value)";
                        else
                            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    }
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black"; // "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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

            for (int Loop = 0; Loop < DG_SubTable_List.Count; Loop++)            // Dynamic based on Display Columns in 3/6 
            {
                XmlElement TablixMember = xml.CreateElement("TablixMember");
                Tablix_Col_Members.AppendChild(TablixMember);
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

            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            //Tablix.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "End";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            Top.InnerText = (Total_Sel_TextBox_Height).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20517in";
            //Left.InnerText = "0.20417in";
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

            //////////////////
            XmlElement Sel_Rect_Textbox2 = xml.CreateElement("Textbox");
            Sel_Rect_Textbox2.SetAttribute("Name", "BtnTot_Textbox");
            ReportItems.AppendChild(Sel_Rect_Textbox2);

            XmlElement Textbox1_Cangrow1 = xml.CreateElement("CanGrow");
            Textbox1_Cangrow1.InnerText = "true";
            Sel_Rect_Textbox2.AppendChild(Textbox1_Cangrow1);

            XmlElement Textbox2_Keep = xml.CreateElement("KeepTogether");
            Textbox2_Keep.InnerText = "true";
            Sel_Rect_Textbox2.AppendChild(Textbox2_Keep);

            XmlElement Textbox2_Paragraphs = xml.CreateElement("Paragraphs");
            Sel_Rect_Textbox2.AppendChild(Textbox2_Paragraphs);

            XmlElement Textbox2_Paragraph = xml.CreateElement("Paragraph");
            Textbox2_Paragraph.AppendChild(Textbox2_Paragraph);

            XmlElement Textbox2_TextRuns = xml.CreateElement("TextRuns");
            Textbox2_Paragraph.AppendChild(Textbox2_TextRuns);

            XmlElement Textbox2_TextRun = xml.CreateElement("TextRun");
            Textbox2_TextRuns.AppendChild(Textbox2_TextRun);

            XmlElement Textbox2_TextRun_Value = xml.CreateElement("Value");
            Textbox2_TextRun_Value.InnerText = "4. Progress towards Self-Sufficiency  ";
            Textbox2_TextRun.AppendChild(Textbox2_TextRun_Value);


            XmlElement Textbox2_TextRun_Style = xml.CreateElement("Style");
            Textbox2_TextRun.AppendChild(Textbox2_TextRun_Style);

            XmlElement Return_Style_FontWeight1 = xml.CreateElement("FontWeight");
            Return_Style_FontWeight1.InnerText = "Bold";
            Textbox2_TextRun_Style.AppendChild(Return_Style_FontWeight1);

            XmlElement Textbox2_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            Textbox2_TextRun_Style_Color.InnerText = "Black";
            Textbox2_TextRun_Style.AppendChild(Textbox2_TextRun_Style_Color);

            XmlElement Textbox2_Paragraph_Style = xml.CreateElement("Style");
            Textbox2_Paragraph.AppendChild(Textbox2_Paragraph_Style);

            //Total_Sel_TextBox_Height += 0.25;

            XmlElement Textbox2_Top = xml.CreateElement("Top");
            //Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //Textbox1_Top.InnerText = (i == 0 ? "0.09in" : "0.30855in");
            Textbox2_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";
            Sel_Rect_Textbox2.AppendChild(Textbox2_Top);
            Total_Sel_TextBox_Height += 0.21855;

            XmlElement Textbox2_Left = xml.CreateElement("Left");
            Textbox2_Left.InnerText = "0.19792in";
            Sel_Rect_Textbox2.AppendChild(Textbox2_Left);

            XmlElement Textbox2_Height = xml.CreateElement("Height");
            Textbox2_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            Sel_Rect_Textbox2.AppendChild(Textbox2_Height);

            XmlElement Textbox2_Width = xml.CreateElement("Width");
            Textbox2_Width.InnerText = "4.62708in";
            Sel_Rect_Textbox2.AppendChild(Textbox2_Width);

            XmlElement Textbox2_Style = xml.CreateElement("Style");
            Sel_Rect_Textbox2.AppendChild(Textbox2_Style);

            XmlElement Textbox2_Style_Border = xml.CreateElement("Border");
            Textbox2_Style.AppendChild(Textbox2_Style_Border);

            XmlElement Textbox2_Style_Border_Style = xml.CreateElement("Style");
            Textbox2_Style_Border_Style.InnerText = "Solid";
            Textbox2_Style_Border.AppendChild(Textbox2_Style_Border_Style);

            XmlElement Textbox2_Style_BackColor = xml.CreateElement("BackgroundColor");
            Textbox2_Style_BackColor.InnerText = "LightGrey";
            Textbox2_Style.AppendChild(Textbox2_Style_BackColor);

            XmlElement Textbox2_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
            Textbox2_Style_PaddingLeft.InnerText = "2pt";
            Textbox2_Style.AppendChild(Textbox2_Style_PaddingLeft);

            XmlElement Textbox2_Style_PaddingRight = xml.CreateElement("PaddingRight");
            Textbox2_Style_PaddingRight.InnerText = "2pt";
            Textbox2_Style.AppendChild(Textbox2_Style_PaddingRight);

            XmlElement Textbox2_Style_PaddingTop = xml.CreateElement("PaddingTop");
            Textbox2_Style_PaddingTop.InnerText = "2pt";
            Textbox2_Style.AppendChild(Textbox2_Style_PaddingTop);

            XmlElement Textbox2_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
            Textbox2_Style_PaddingTop.InnerText = "2pt";
            Textbox2_Style.AppendChild(Textbox2_Style_PaddingBottom);


            //   Subreport
            ////////if (Summary_Sw)
            ////////{
            ////////    // Summary Sub Report 
            ////////}

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>
            for (int i = 0; i < 2; i++)
            {
                XmlElement RepTot_Textbox1 = xml.CreateElement("Textbox");
                RepTot_Textbox1.SetAttribute("Name", "BtnTot_Textbox_Rep" + i.ToString());
                ReportItems.AppendChild(RepTot_Textbox1);

                XmlElement RepTot_Cangrow = xml.CreateElement("CanGrow");
                RepTot_Cangrow.InnerText = "true";
                RepTot_Textbox1.AppendChild(RepTot_Cangrow);

                XmlElement RepTot_Keep = xml.CreateElement("KeepTogether");
                RepTot_Keep.InnerText = "true";
                RepTot_Textbox1.AppendChild(RepTot_Keep);

                XmlElement RepTot_Paragraphs = xml.CreateElement("Paragraphs");
                RepTot_Textbox1.AppendChild(RepTot_Paragraphs);

                XmlElement RepTot_Paragraph = xml.CreateElement("Paragraph");
                RepTot_Paragraphs.AppendChild(RepTot_Paragraph);

                XmlElement RepTot_TextRuns = xml.CreateElement("TextRuns");
                RepTot_Paragraph.AppendChild(RepTot_TextRuns);

                XmlElement RepTot_TextRun = xml.CreateElement("TextRun");
                RepTot_TextRuns.AppendChild(RepTot_TextRun);

                XmlElement RepTot_TextRun_Value = xml.CreateElement("Value"); //ppppppppppp
                RepTot_TextRun_Value.InnerText = (i == 0 ? "a. Total number of persons who made progress towards Self-Sufficiency:  " : RepPeriod_Clients_Cnt);
                RepTot_TextRun.AppendChild(RepTot_TextRun_Value);

                //XmlElement Cell_Align = xml.CreateElement("Style");
                //XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");        
                //Cell_TextAlign.InnerText = "Right";//"Center";
                //Cell_Align.AppendChild(Cell_TextAlign);
                //RepTot_Paragraph.AppendChild(Cell_Align);

                XmlElement RepTot_TextRun_Style = xml.CreateElement("Style");
                RepTot_TextRun.AppendChild(RepTot_TextRun_Style);

                XmlElement RepTotReturn_Style_FontWeight = xml.CreateElement("FontWeight");
                RepTotReturn_Style_FontWeight.InnerText = "Bold";
                RepTot_TextRun_Style.AppendChild(RepTotReturn_Style_FontWeight);

                XmlElement RepTot_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                RepTot_TextRun_Style_Color.InnerText = "Black";
                RepTot_TextRun_Style.AppendChild(RepTot_TextRun_Style_Color);

                //XmlElement RepTot_Paragraph_Style = xml.CreateElement("Style");
                //RepTot_Paragraph.AppendChild(RepTot_Paragraph_Style);

                if (i == 1)
                {
                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                    //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                    Cell_TextAlign.InnerText = Text_Align;
                    Cell_Align.AppendChild(Cell_TextAlign);
                    RepTot_Paragraph.AppendChild(Cell_Align);
                }


                //Total_Sel_TextBox_Height += 0.25;

                XmlElement RepTot_Top = xml.CreateElement("Top");
                RepTot_Top.InnerText = (Total_Sel_TextBox_Height + 0.3).ToString() + "in";
                RepTot_Textbox1.AppendChild(RepTot_Top);

                //Total_Sel_TextBox_Height += 1.35;

                XmlElement RepTot_Left = xml.CreateElement("Left");
                RepTot_Left.InnerText = (i == 0 ? "0.19792in" : "4.87654in");
                RepTot_Textbox1.AppendChild(RepTot_Left);

                XmlElement RepTot_Height = xml.CreateElement("Height");
                RepTot_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
                RepTot_Textbox1.AppendChild(RepTot_Height);

                XmlElement RepTot_Width = xml.CreateElement("Width");
                RepTot_Width.InnerText = (i == 0 ? "4.62708in" : "0.42461in");
                RepTot_Textbox1.AppendChild(RepTot_Width);

                XmlElement RepTot_Style = xml.CreateElement("Style");
                RepTot_Textbox1.AppendChild(RepTot_Style);

                XmlElement RepTot_Style_Border = xml.CreateElement("Border");
                RepTot_Style.AppendChild(RepTot_Style_Border);

                XmlElement RepTot_Style_Border_Style = xml.CreateElement("Style");
                RepTot_Style_Border_Style.InnerText = "None";
                RepTot_Style_Border.AppendChild(RepTot_Style_Border_Style);

                //XmlElement RepTot_Style_BackColor = xml.CreateElement("BackgroundColor");
                //RepTot_Style_BackColor.InnerText = "LightGrey";
                //RepTot_Style.AppendChild(RepTot_Style_BackColor);

                XmlElement RepTot_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
                RepTot_Style_PaddingLeft.InnerText = "2pt";
                RepTot_Style.AppendChild(RepTot_Style_PaddingLeft);

                XmlElement RepTot_Style_PaddingRight = xml.CreateElement("PaddingRight");
                RepTot_Style_PaddingRight.InnerText = "2pt";
                RepTot_Style.AppendChild(RepTot_Style_PaddingRight);

                XmlElement RepTot_Style_PaddingTop = xml.CreateElement("PaddingTop");
                RepTot_Style_PaddingTop.InnerText = "2pt";
                RepTot_Style.AppendChild(RepTot_Style_PaddingTop);

                XmlElement RepTot_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
                RepTot_Style_PaddingTop.InnerText = "2pt";
                RepTot_Style.AppendChild(RepTot_Style_PaddingBottom);
            }
            Total_Sel_TextBox_Height += 0.8;

            //
           

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

            //Rep_Header_Title = " Test Report";
            //if (true && !string.IsNullOrEmpty(Rep_Header_Title.Trim())) //Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            //{
            //}

            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            //if (false) //Include_Footer
            //{
            //}

            //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


            XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
            XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

            //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
            //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            if (false) //(Rb_A4_Port.Checked)
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

                xml.Save(ReportPath + Rep_Name + "SubReport.rdlc");
                //xml.Save(@"C:\Capreports\" + Rep_Name + "SubReport.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            Console.ReadLine();
        }

        private void rbFundSel_Click(object sender, EventArgs e)
        {
            if (rbFundSel.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void Rb_Sel_Scales_Click(object sender, EventArgs e)
        {
            if (Rb_Sel_Scales.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, Privileges.Program, ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString(), Selected_Scales_List);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
                Selected_Scales_List = form.Get_Sel_Scales_List;
        }

        private void Rb_All_Scales_Click(object sender, EventArgs e)
        {
            Selected_Scales_List.Clear();
        }

        private void MATB1002_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private bool Validate_Report()
        {
            bool Can_Generate = true;

            if ((string.IsNullOrEmpty(((ListItem)Cmb_Matrix.SelectedItem).Text.Trim())))
            {
                _errorProvider.SetError(Cmb_Matrix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMatrix.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Cmb_Matrix, null);

            if (Rb_Sel_Scales.Checked)
            {
                if (Selected_Scales_List.Count == 0)
                {
                    _errorProvider.SetError(Rb_Sel_Scales, string.Format("Please Select at least one Scale".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_Sel_Scales, null);
            }
            else
            {
                if (Rb_All_Scales.Checked)
                    _errorProvider.SetError(Rb_Sel_Scales, null);
            }

            if (!Base_FDate.Checked)
            {
                _errorProvider.SetError(Base_FDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine From Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Base_FDate, null);


            if (!Base_TDate.Checked)
            {
                _errorProvider.SetError(Base_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine To Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Base_TDate, null);

            if (!Asmt_F_Date.Checked)
            {
                _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Asmt_F_Date, null);

            if (!Asmt_T_Date.Checked)
            {
                _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment To Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Asmt_T_Date, null);
            //**
            if (Base_FDate.Checked.Equals(true) && Base_TDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Base_FDate.Text))
                {
                    _errorProvider.SetError(Base_FDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Base_FDate, null);
                }
                if (string.IsNullOrWhiteSpace(Base_TDate.Text))
                {
                    _errorProvider.SetError(Base_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine To Date ".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Base_TDate, null);
                }
            }

            if (Asmt_F_Date.Checked.Equals(true) && Asmt_T_Date.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Asmt_F_Date.Text))
                {
                    _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_F_Date, null);
                }
                if (string.IsNullOrWhiteSpace(Asmt_T_Date.Text))
                {
                    _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment To Date ".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Asmt_T_Date, null);
                }
            }
            //**
            if (Base_FDate.Checked && Base_TDate.Checked)
            {
                if (!string.IsNullOrEmpty(Base_FDate.Text) && (!string.IsNullOrEmpty(Base_TDate.Text)))
                {
                    if (Convert.ToDateTime(Base_FDate.Text) > Convert.ToDateTime(Base_TDate.Text))
                    {
                        _errorProvider.SetError(Base_FDate, string.Format("BaseLine 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                }
            }
            else
            {
                if (Base_FDate.Checked && Base_TDate.Checked)
                    _errorProvider.SetError(Base_FDate, null);
            }

            if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
            {
                if (!string.IsNullOrEmpty(Asmt_F_Date.Text) && (!string.IsNullOrEmpty(Asmt_T_Date.Text)))
                {
                    if (Convert.ToDateTime(Asmt_F_Date.Text) > Convert.ToDateTime(Asmt_T_Date.Text))
                    {
                        _errorProvider.SetError(Asmt_F_Date, string.Format("Assessment 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                }
            }
            else
            {
                if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
                    _errorProvider.SetError(Asmt_F_Date, null);
            }

            if (Cb_Enroll.Checked)
            {

                if (Privileges.ModuleCode == "03")
                {
                    if (!Asof_From_Date.Checked)
                    {
                        _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of From Date".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Asof_From_Date, null);


                    if (!Asof_To_Date.Checked)
                    {
                        _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of To Date".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Asof_To_Date, null);
                    //**
                    if (Asof_From_Date.Checked.Equals(true) && Asof_To_Date.Checked.Equals(true))
                    {
                        if (string.IsNullOrWhiteSpace(Asof_From_Date.Text))
                        {
                            _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of From Date".Replace(Consts.Common.Colon, string.Empty)));
                            Can_Generate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(Asof_From_Date, null);
                        }
                        if (string.IsNullOrWhiteSpace(Asof_To_Date.Text))
                        {
                            _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of To Date ".Replace(Consts.Common.Colon, string.Empty)));
                            Can_Generate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(Asof_To_Date, null);
                        }
                    }
                    if (Asof_From_Date.Checked && Asof_To_Date.Checked)
                    {
                        if (!string.IsNullOrEmpty(Asof_From_Date.Text) && (!string.IsNullOrEmpty(Asof_To_Date.Text)))
                        {
                            if (Convert.ToDateTime(Asof_From_Date.Text) > Convert.ToDateTime(Asof_To_Date.Text))
                            {
                                _errorProvider.SetError(Asof_From_Date, string.Format("As of 'From Date' should not be greater than 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                                Can_Generate = false;
                            }
                        }
                    }
                    else
                    {
                        if (Asof_From_Date.Checked && Asof_To_Date.Checked)
                            _errorProvider.SetError(Asof_From_Date, null);
                    }

                    if (string.IsNullOrEmpty(Txt_Program.Text.Trim()))
                    {
                        _errorProvider.SetError(Pb_Prog, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text.Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Pb_Prog, null);
                }

                else if (Privileges.ModuleCode == "02")
                {
                    if (!Asof_From_Date.Checked)
                    {
                        _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of From Date".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Asof_From_Date, null);


                    if (!Asof_To_Date.Checked)
                    {
                        _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of To Date".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Asof_To_Date, null);

                    if (Asof_From_Date.Checked.Equals(true) && Asof_To_Date.Checked.Equals(true))
                    {
                        if (string.IsNullOrWhiteSpace(Asof_From_Date.Text))
                        {
                            _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of From Date".Replace(Consts.Common.Colon, string.Empty)));
                            Can_Generate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(Asof_From_Date, null);
                        }
                        if (string.IsNullOrWhiteSpace(Asof_To_Date.Text))
                        {
                            _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of To Date ".Replace(Consts.Common.Colon, string.Empty)));
                            Can_Generate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(Asof_To_Date, null);
                        }
                    }

                    if (Asof_From_Date.Checked && Asof_To_Date.Checked)
                    {
                        if (!string.IsNullOrEmpty(Asof_From_Date.Text) && (!string.IsNullOrEmpty(Asof_To_Date.Text)))
                        {
                            if (Convert.ToDateTime(Asof_From_Date.Text) > Convert.ToDateTime(Asof_To_Date.Text))
                            {
                                _errorProvider.SetError(Asof_From_Date, string.Format("As of From Date should not be greater than To Date".Replace(Consts.Common.Colon, string.Empty)));
                                Can_Generate = false;
                            }
                        }
                    }
                    else
                    {
                        if (Asof_From_Date.Checked && Asof_To_Date.Checked)
                            _errorProvider.SetError(Asof_From_Date, null);
                    }

                    if (rbFundSel.Checked == true)
                    {
                        if (SelFundingList.Count == 0)
                        {
                            _errorProvider.SetError(rbFundSel, "Select at least One Site");
                            Can_Generate = false;
                        }
                        else
                        {
                            _errorProvider.SetError(rbFundSel, null);
                        }
                    }
                    else
                    {
                        _errorProvider.SetError(rbFundSel, null);
                    }

                    if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()))
                    {
                        _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Pb_Withdraw_Enroll, null);
                }
            }

            if (rdbSelAlertCds.Checked)
            {
                if (SelectedAlertCodes == "")
                {
                    _errorProvider.SetError(rdbSelAlertCds, "Please Select atleast one Alert Code");
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(rdbSelAlertCds, null);
                }
            }

            return Can_Generate;
        }

        //private bool Validate_Report()
        //{
        //    bool Can_Generate = true;

        //    if (Cb_Enroll.Checked)
        //    {
        //        if (!Asof_From_Date.Checked)
        //        {
        //            _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of From Date".Replace(Consts.Common.Colon, string.Empty)));
        //            Can_Generate = false;
        //        }
        //        else
        //            _errorProvider.SetError(Asof_From_Date, null);


        //        if (!Asof_To_Date.Checked)
        //        {
        //            _errorProvider.SetError(Asof_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "As of To Date".Replace(Consts.Common.Colon, string.Empty)));
        //            Can_Generate = false;
        //        }
        //        else
        //            _errorProvider.SetError(Asof_To_Date, null);

        //        if (Asof_From_Date.Checked && Asof_To_Date.Checked)
        //        {
        //            if (!string.IsNullOrEmpty(Asof_From_Date.Text) && (!string.IsNullOrEmpty(Asof_To_Date.Text)))
        //            {
        //                if (Convert.ToDateTime(Asof_From_Date.Text) > Convert.ToDateTime(Asof_To_Date.Text))
        //                {
        //                    _errorProvider.SetError(Asof_From_Date, string.Format("As of From Date should not be greater than To Date".Replace(Consts.Common.Colon, string.Empty)));
        //                    Can_Generate = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (Asof_From_Date.Checked && Asof_To_Date.Checked)
        //                _errorProvider.SetError(Asof_From_Date, null);
        //        }

        //        if (rbFundSel.Checked == true)
        //        {
        //            if (SelFundingList.Count == 0)
        //            {
        //                _errorProvider.SetError(rbFundSel, "Select at least One Site");
        //                Can_Generate = false;
        //            }
        //            else
        //            {
        //                _errorProvider.SetError(rbFundSel, null);
        //            }
        //        }
        //        else
        //        {
        //            _errorProvider.SetError(rbFundSel, null);
        //        }

        //    }

        //    if ((string.IsNullOrEmpty(((ListItem)Cmb_Matrix.SelectedItem).Text.Trim())))
        //    {
        //        _errorProvider.SetError(Cmb_Matrix, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMatrix.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        Can_Generate = false;
        //    }
        //    else
        //        _errorProvider.SetError(Cmb_Matrix, null);

        //    if (Rb_Sel_Scales.Checked)
        //    {
        //        if (Selected_Scales_List.Count == 0)
        //        {
        //            _errorProvider.SetError(Rb_Sel_Scales, string.Format("Please Select at least one Scale".Replace(Consts.Common.Colon, string.Empty)));
        //            Can_Generate = false;
        //        }
        //        else
        //            _errorProvider.SetError(Rb_Sel_Scales, null);
        //    }
        //    else
        //    {
        //        if (Rb_All_Scales.Checked)
        //            _errorProvider.SetError(Rb_Sel_Scales, null);
        //    }

        //    if (!Base_FDate.Checked)
        //    {
        //        _errorProvider.SetError(Base_FDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine From Date".Replace(Consts.Common.Colon, string.Empty)));
        //        Can_Generate = false;
        //    }
        //    else
        //        _errorProvider.SetError(Base_FDate, null);


        //    if (!Base_TDate.Checked)
        //    {
        //        _errorProvider.SetError(Base_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "BaseLine To Date".Replace(Consts.Common.Colon, string.Empty)));
        //        Can_Generate = false;
        //    }
        //    else
        //        _errorProvider.SetError(Base_TDate, null);

        //    if (!Asmt_F_Date.Checked)
        //    {
        //        _errorProvider.SetError(Asmt_F_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date".Replace(Consts.Common.Colon, string.Empty)));
        //        Can_Generate = false;
        //    }
        //    else
        //        _errorProvider.SetError(Asmt_F_Date, null);

        //    if (!Asmt_T_Date.Checked)
        //    {
        //        _errorProvider.SetError(Asmt_T_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Assessment From Date".Replace(Consts.Common.Colon, string.Empty)));
        //        Can_Generate = false;
        //    }
        //    else
        //        _errorProvider.SetError(Asmt_T_Date, null);


        //    if (Base_FDate.Checked && Base_TDate.Checked)
        //    {
        //        if (!string.IsNullOrEmpty(Base_FDate.Text) && (!string.IsNullOrEmpty(Base_TDate.Text)))
        //        {
        //            if (Convert.ToDateTime(Base_FDate.Text) > Convert.ToDateTime(Base_TDate.Text))
        //            {
        //                _errorProvider.SetError(Base_FDate, string.Format("BaseLine 'From Date' should be prior or equal to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
        //                Can_Generate = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Base_FDate.Checked && Base_TDate.Checked)
        //            _errorProvider.SetError(Base_FDate, null);
        //    }

        //    if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
        //    {
        //        if (!string.IsNullOrEmpty(Asmt_F_Date.Text) && (!string.IsNullOrEmpty(Asmt_T_Date.Text)))
        //        {
        //            if (Convert.ToDateTime(Asmt_F_Date.Text) > Convert.ToDateTime(Asmt_T_Date.Text))
        //            {
        //                _errorProvider.SetError(Asmt_F_Date, string.Format("Assessment 'From Date' should be prior or equal to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
        //                Can_Generate = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Asmt_F_Date.Checked && Asmt_T_Date.Checked)
        //            _errorProvider.SetError(Asmt_F_Date, null);
        //    }

        //    _errorProvider.SetError(Asof_From_Date, null);
        //    _errorProvider.SetError(Pb_Prog, null);
        //    _errorProvider.SetError(Pb_Withdraw_Enroll, null);
        //    if (Cb_Enroll.Checked)
        //    {
        //        if (!Asof_From_Date.Checked)
        //        {
        //            _errorProvider.SetError(Asof_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), Lbl_Asof_Date.Text.Replace(Consts.Common.Colon, string.Empty)));
        //            Can_Generate = false;
        //        }
        //        else
        //            _errorProvider.SetError(Asof_From_Date, null);

        //        if (Privileges.ModuleCode == "03")
        //        {
        //            if (string.IsNullOrEmpty(Txt_Program.Text.Trim()))
        //            {
        //                _errorProvider.SetError(Pb_Prog, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text.Replace(Consts.Common.Colon, string.Empty)));
        //                Can_Generate = false;
        //            }
        //            else
        //                _errorProvider.SetError(Pb_Prog, null);
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()))
        //            {
        //                _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site".Replace(Consts.Common.Colon, string.Empty)));
        //                Can_Generate = false;
        //            }
        //            else
        //                _errorProvider.SetError(Pb_Withdraw_Enroll, null);
        //        }
        //    }


        //    return Can_Generate;
        //}

        private void Btn_Save_Params_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(Asof_From_Date, null); _errorProvider.SetError(Asof_To_Date, null);
            _errorProvider.SetError(rbFundSel, null); _errorProvider.SetError(Rb_Sel_Scales, null);
            _errorProvider.SetError(Asmt_F_Date, null); _errorProvider.SetError(Asmt_T_Date, null);
            _errorProvider.SetError(Base_FDate, null); _errorProvider.SetError(Base_TDate, null);
            _errorProvider.SetError(Pb_Prog, null); _errorProvider.SetError(Pb_Withdraw_Enroll, null);
            if (Validate_Report())
            {
                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);

                //Get_Selection_Criteria();
                //Save_Entity.Scr_Code = PrivilegeEntity.Program;
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = null;
                Save_Entity.Card_3 = null;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }

        }

        private string Get_XML_Format_for_Report_Controls()   // 12012012
        {
            StringBuilder str = new StringBuilder();
            if (Privileges.ModuleCode == "03")
            {
                string strAssessmenttype = string.Empty, strEnrl = string.Empty;
                if (cmbAssessmentType.Visible == true)
                {
                    strAssessmenttype = (((ListItem)cmbAssessmentType.SelectedItem).Value.ToString());
                }
                string strScaleSource = Rb_All_Scales.Checked ? "Y" : "N";

                    string strscalesCodes = string.Empty;
                    if (Rb_Sel_Scales.Checked == true)
                    {
                        foreach (MATDEFEntity ScaleCode in Selected_Scales_List)
                        {
                            if (!strscalesCodes.Equals(string.Empty)) strscalesCodes += ",";
                            strscalesCodes += ScaleCode.Scale_Code;
                        }
                    }

                if (Cb_Enroll.Checked)
                {
                    if (cmbEnrlStatus.Visible == true)
                    {
                        strEnrl = ((ListItem)cmbAssessmentType.SelectedItem).Value.ToString();
                    }
                }
                string Site = ((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Text.Trim();

                if (Cb_Enroll.Checked == true)
                {
                    str.Append("<Rows>");

                    str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                        "\" YEAR = \"" + Program_Year +
                        "\" MATRIX = \"" + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim() + "\" SCALE = \"" + strScaleSource + "\" SCALECODES = \"" + strscalesCodes + "\" PRINTONLY = \"" +
                        (Cb_Only_Asmt_Scale.Checked ? "Y" : "N") + "\" DATESEL = \"" + (Rb_Asmt_FDate.Checked ? "F" : "L") +
                        "\" BASE_FDATE = \"" + Base_FDate.Value.ToShortDateString() + "\" BASE_TDATE = \"" + Base_TDate.Value.ToShortDateString() +
                        "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() + "\" ASSESSMENT_TYPE = \"" + strAssessmenttype +
                        "\" CASEWORKER = \"" + (((ListItem)Cmb_Worker.SelectedItem).Text.Trim()) + "\" Site = \"" + Site +
                        "\" ENROLL = \"" + (Cb_Enroll.Checked ? "Y" : "N") + "\" ENROLLSTATUS = \"" + ((ListItem)cmbEnrlStatus.SelectedItem).Text.Trim() +
                        "\" ASOFFROMDATE = \"" + Asof_From_Date.Value.ToShortDateString() + "\" ASOFTODATE = \"" + Asof_To_Date.Value.ToShortDateString() +
                        "\" PROGRAM = \"" + Txt_Program.Text.Trim() + "\" PRINTDETAIL = \"" + (chkDet.Checked ? "Y" : "N") + "\" ALERTCODES = \"" + (rdbAlertAllCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />");
                    str.Append("</Rows>");
                }

                else
                {
                    str.Append("<Rows>");

                    str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                        "\" YEAR = \"" + Program_Year +
                        "\" MATRIX = \"" + (((ListItem)Cmb_Matrix.SelectedItem).Text.Trim()) + "\" SCALE = \"" + strScaleSource + "\" SCALECODES = \"" + strscalesCodes + "\" PRINTONLY = \"" +
                        (Cb_Only_Asmt_Scale.Checked ? "Y" : "N") + "\" DATESEL = \"" + (Rb_Asmt_FDate.Checked ? "F" : "L") +
                        "\" BASE_FDATE = \"" + Base_FDate.Value.ToShortDateString() + "\" BASE_TDATE = \"" + Base_TDate.Value.ToShortDateString() +
                        "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() + "\" ASSESSMENT_TYPE = \"" + strAssessmenttype +
                        "\" CASEWORKER = \"" + (((ListItem)Cmb_Worker.SelectedItem).Text.Trim()) + "\" Site = \"" + Site + 
                        "\" PRINTDETAIL = \"" + (chkDet.Checked ? "Y" : "N") + "\" ENROLL = \"" + "N" + "\" ALERTCODES = \"" + (rdbAlertAllCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />");
                    str.Append("</Rows>");
                }

            }
            else if(Privileges.ModuleCode == "02")
            {
                string strAssessmenttype = string.Empty;
                if (cmbAssessmentType.Visible == true)
                {
                    strAssessmenttype = (((ListItem)cmbAssessmentType.SelectedItem).Value.ToString());
                }
                string strScaleSource = Rb_All_Scales.Checked ? "Y" : "N";

                string strscalesCodes = string.Empty;
                if (Rb_Sel_Scales.Checked == true)
                {
                    foreach (MATDEFEntity ScaleCode in Selected_Scales_List)
                    {
                        if (!strscalesCodes.Equals(string.Empty)) strscalesCodes += ",";
                        strscalesCodes += ScaleCode.Scale_Code;
                    }
                }
                string strFundSource = rbFundAll.Checked ? "Y" : "N";

                string strFundingCodes = string.Empty;
                if (Cb_Enroll.Checked)
                {
                    if (rbFundSel.Checked == true)
                    {
                        foreach (CommonEntity FundingCode in SelFundingList)
                        {
                            if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                            strFundingCodes += FundingCode.Code;
                        }
                    }
                }
                string Site = ((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Text.Trim();

                if (Cb_Enroll.Checked == true)
                {
                    str.Append("<Rows>");

                    str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                        "\" YEAR = \"" + Program_Year + "\" SITESEL = \"" + Txt_DrawEnroll_Site.Text.Trim() + "\" ROOM = \"" + Txt_DrawEnroll_Room.Text.Trim() + "\" AMPM = \"" + Txt_DrawEnroll_AMPM.Text.Trim() +
                        "\" MATRIX = \"" + (((ListItem)Cmb_Matrix.SelectedItem).Text.Trim()) + "\" SCALE = \"" + strScaleSource + "\" SCALECODES = \"" + strscalesCodes + "\" PRINTONLY = \"" +
                        (Cb_Only_Asmt_Scale.Checked ? "Y" : "N") + "\" DATESEL = \"" + (Rb_Asmt_FDate.Checked ? "F" : "L") +
                        "\" BASE_FDATE = \"" + Base_FDate.Value.ToShortDateString() + "\" BASE_TDATE = \"" + Base_TDate.Value.ToShortDateString() +
                        "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() + "\" ASSESSMENT_TYPE = \"" + strAssessmenttype +
                        "\" CASEWORKER = \"" + (((ListItem)Cmb_Worker.SelectedItem).Text.Trim()) + "\" Site = \"" + Site +
                        "\" ENROLL = \"" + (Cb_Enroll.Checked ? "Y" : "N") + "\" ENROLLSTATUS = \"" + ((ListItem)cmbEnrlStatus.SelectedItem).Text.Trim()  + 
                        "\" ASOFFROMDATE = \"" + Asof_From_Date.Value.ToShortDateString() + "\" ASOFTODATE = \"" + Asof_To_Date.Value.ToShortDateString() 
                        + "\" PRINTDETAIL = \"" + (chkDet.Checked ? "Y" : "N") +
                        "\" FundedSource = \"" + strFundSource + "\" FundingCode = \"" + strFundingCodes + "\" ALERTCODES = \"" + (rdbAlertAllCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />");
                    str.Append("</Rows>");
                }
                else
                {
                    str.Append("<Rows>");

                    str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                        "\" YEAR = \"" + Program_Year +
                        "\" MATRIX = \"" + (((ListItem)Cmb_Matrix.SelectedItem).Text.Trim()) + "\" SCALE = \"" + strScaleSource + "\" SCALECODES = \"" + strscalesCodes + "\" PRINTONLY = \"" +
                        (Cb_Only_Asmt_Scale.Checked ? "Y" : "N") + "\" DATESEL = \"" + (Rb_Asmt_FDate.Checked ? "F" : "L") +
                        "\" BASE_FDATE = \"" + Base_FDate.Value.ToShortDateString() + "\" BASE_TDATE = \"" + Base_TDate.Value.ToShortDateString() +
                        "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() + "\" ASSESSMENT_TYPE = \"" + strAssessmenttype +
                        "\" CASEWORKER = \"" + (((ListItem)Cmb_Worker.SelectedItem).Text.Trim()) + "\" Site = \"" + Site +
                        "\" PRINTDETAIL = \"" + (chkDet.Checked ? "Y" : "N")  + "\" ENROLL = \"" + "N" + "\" ALERTCODES = \"" + (rdbAlertAllCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />");
                    str.Append("</Rows>");
                }
                //StringBuilder str = new StringBuilder();
                //str.Append("<Rows>");

                //str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                //    "\" MATRIX = \"" + (((ListItem)Cmb_Matrix.SelectedItem).Text.Trim()) + "\" SCALE = \"" + (Rb_All_Scales.Checked ? "*" : "Sel") + "\" PRINTONLY = \"" + (Cb_Only_Asmt_Scale.Checked ? "Y" : "N") +
                //    "\" DATESEL = \"" + (Rb_Asmt_FDate.Checked ? "F" : "L") + "\" BASE_FDATE = \"" + Base_FDate.Value.ToShortDateString() + "\" BASE_TDATE = \"" + Base_TDate.Value.ToShortDateString() + "\" Site = \"" + Site +
                //                          "\" ASMT_FDATE = \"" + Asmt_F_Date.Value.ToShortDateString() + "\" ASMT_TDATE = \"" + Asmt_T_Date.Value.ToShortDateString() + "\" ASSESSMENT_TYPE = \"" + strAssessmenttype + "\" FundedSource = \"" + strFundSource + "\" FundingCode = \"" + strFundingCodes + "\"/>");
                //str.Append("</Rows>");
            }
            return str.ToString();
        }

        private void Rb_Asmt_FDate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Btn_Get_Params_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            //Save_Entity.Scr_Code = PrivilegeEntity.Program;
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();

        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                Clear_Error_Providers();

                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);
            }
        }


        private void Clear_Error_Providers()
        {
            _errorProvider.SetError(Cmb_Matrix, null);
            _errorProvider.SetError(Rb_Sel_Scales, null);
            _errorProvider.SetError(Base_FDate, null);
            _errorProvider.SetError(Base_TDate, null);
            _errorProvider.SetError(Asmt_F_Date, null);
            _errorProvider.SetError(Asmt_T_Date, null);
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        { 
            if (Privileges.ModuleCode == "03")
            {
                if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
                {
                    DataRow dr = Tmp_Table.Rows[0];
                    DataColumnCollection columns = Tmp_Table.Columns;
                    Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());/*string.Empty*/

                    SetComboBoxValue(Cmb_Matrix, dr["MATRIX"].ToString());
                    CommonFunctions.SetComboBoxValue(cmbSite, dr["Site"].ToString());
                    SetComboBoxValue(Cmb_Worker, dr["CASEWORKER"].ToString());
                    //switch (dr["SCALE"].ToString())
                    //{
                    //    case "*": Rb_All_Scales.Checked = true; break;
                    //    default: Rb_Sel_Scales.Checked = true; break;
                    //}
                    if (dr["SCALE"].ToString() == "Y")
                        Rb_All_Scales.Checked = true;
                    else
                    {

                        Rb_Sel_Scales.Checked = true;
                        Selected_Scales_List.Clear();
                        string strScales = dr["SCALECODES"].ToString();
                        List<string> siteList1 = new List<string>();
                        if (strScales != null)
                        {
                            string[] ScaleCodes = strScales.Split(',');
                            for (int i = 0; i < ScaleCodes.Length; i++)
                            {
                                MATDEFEntity ScaleDetails = Scales_List.Find(u => u.Scale_Code == ScaleCodes.GetValue(i).ToString());
                                Selected_Scales_List.Add(ScaleDetails);
                            }
                        }
                        Selected_Scales_List = Selected_Scales_List;
                    }
                    Cb_Only_Asmt_Scale.Checked = (dr["PRINTONLY"].ToString() == "Y" ? true : false);
                    chkDet.Checked = (dr["PRINTDETAIL"].ToString() == "Y" ? true : false);

                    if (dr["DATESEL"].ToString() == "F")
                        Rb_Asmt_FDate.Checked = true;
                    else
                        Rb_Asmt_TDate.Checked = true;

                    Base_FDate.Value = Convert.ToDateTime(dr["BASE_FDATE"].ToString());
                    Base_TDate.Value = Convert.ToDateTime(dr["BASE_TDATE"].ToString());

                    Asmt_F_Date.Value = Convert.ToDateTime(dr["ASMT_FDATE"].ToString());
                    Asmt_T_Date.Value = Convert.ToDateTime(dr["ASMT_TDATE"].ToString());

                    Base_FDate.Checked = Base_TDate.Checked =
                    Asmt_F_Date.Checked = Asmt_T_Date.Checked = true;
                    
                    if (cmbAssessmentType.Visible == true)
                    {
                        if (Tmp_Table.Columns.Contains("ASSESSMENT_TYPE"))
                        {
                            SetComboBoxValue(cmbAssessmentType, dr["ASSESSMENT_TYPE"].ToString());
                        }
                    }

                    Cb_Enroll.Checked = (dr["ENROLL"].ToString() == "Y" ? true : false);
                    if(Cb_Enroll.Checked == true)
                    {
                        SetComboBoxValue(cmbEnrlStatus, dr["ENROLLSTATUS"].ToString());
                        Asof_From_Date.Value = Convert.ToDateTime(dr["ASOFFROMDATE"].ToString());
                        Asof_To_Date.Value = Convert.ToDateTime(dr["ASOFTODATE"].ToString());
                        //SelectedHierarchies = dr["PROGRAM"].ToString();
                        //Fill_Programs_Grid(SelectedHierarchies);
                        List<HierarchyEntity> SelHies = new List<HierarchyEntity>();
                        
                        Txt_Program.Clear();
                        string Program = dr["PROGRAM"].ToString();
                        List<string> siteList1 = new List<string>();
                        if (Program != null)
                        {
                            string[] Prog = Program.Split(',');
                            for (int i = 0; i < Prog.Length; i++)
                            {
                                HierarchyEntity ProgramDesc = caseHierarchy.Find(u => u.Agency+u.Dept+ u.Prog == Prog.GetValue(i).ToString().TrimStart());
                                SelHies.Add(ProgramDesc);
                            }

                            if(SelHies.Count > 0) { Fill_Programs_Grid(SelHies); }
                        }
                    }

                    if (columns.Contains("ALERTCODES"))
                    {
                        if (dr["ALERTCODES"].ToString() == "A")
                            rdbAlertAllCds.Checked = true;
                        else if (dr["ALERTCODES"].ToString() == "S")
                            rdbSelAlertCds.Checked = true;
                    }
                    else
                        rdbAlertAllCds.Checked = true;

                    if (columns.Contains("SELECTEDALERTCODES"))
                    {
                        if (dr["SELECTEDALERTCODES"].ToString() != "")
                        {
                            Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                        }
                    }

                    //if (dr["FundedSource"].ToString() == "Y")
                    //    rbFundAll.Checked = true;
                    //else
                    //{
                    //    rbFundSel.Checked = true;
                    //    SelFundingList.Clear();
                    //    string strFunds = dr["FundingCode"].ToString();
                    //    List<string> siteList1 = new List<string>();
                    //    if (strFunds != null)
                    //    {
                    //        string[] FundCodes = strFunds.Split(',');
                    //        for (int i = 0; i < FundCodes.Length; i++)
                    //        {
                    //            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                    //            SelFundingList.Add(fundDetails);
                    //        }
                    //    }
                    //    SelFundingList = SelFundingList;
                    //}
                }
            }
            else if (Privileges.ModuleCode == "02")
            {
                if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
                {
                    DataRow dr = Tmp_Table.Rows[0];
                    DataColumnCollection columns = Tmp_Table.Columns;
                    Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                    SetComboBoxValue(Cmb_Matrix, dr["MATRIX"].ToString());
                    CommonFunctions.SetComboBoxValue(cmbSite, dr["Site"].ToString());
                    SetComboBoxValue(Cmb_Matrix, dr["CASEWORKER"].ToString());
                    switch (dr["SCALE"].ToString())
                    {
                        case "*":
                            Rb_All_Scales.Checked = true;
                            break;
                        default:
                            Rb_Sel_Scales.Checked = true;
                            break;
                    }

                    Cb_Only_Asmt_Scale.Checked = (dr["PRINTONLY"].ToString() == "Y" ? true : false);
                    chkDet.Checked = (dr["PRINTDETAIL"].ToString() == "Y" ? true : false);


                    if (dr["DATESEL"].ToString() == "F")
                        Rb_Asmt_FDate.Checked = true;
                    else
                        Rb_Asmt_TDate.Checked = true;

                    Base_FDate.Value = Convert.ToDateTime(dr["BASE_FDATE"].ToString());
                    Base_TDate.Value = Convert.ToDateTime(dr["BASE_TDATE"].ToString());

                    Asmt_F_Date.Value = Convert.ToDateTime(dr["ASMT_FDATE"].ToString());
                    Asmt_T_Date.Value = Convert.ToDateTime(dr["ASMT_TDATE"].ToString());

                    Base_FDate.Checked = Base_TDate.Checked =
                    Asmt_F_Date.Checked = Asmt_T_Date.Checked = true;
                    if (cmbAssessmentType.Visible == true)
                    {
                        if (Tmp_Table.Columns.Contains("ASSESSMENT_TYPE"))
                        {
                            SetComboBoxValue(cmbAssessmentType, dr["ASSESSMENT_TYPE"].ToString());
                        }
                    }

                    Cb_Enroll.Checked = (dr["ENROLL"].ToString() == "Y" ? true : false);
                    if (Cb_Enroll.Checked == true)
                    {
                        SetComboBoxValue(cmbEnrlStatus, dr["ENROLLSTATUS"].ToString());
                        Asof_From_Date.Value = Convert.ToDateTime(dr["ASOFFROMDATE"].ToString());
                        Asof_To_Date.Value = Convert.ToDateTime(dr["ASOFTODATE"].ToString());

                        Txt_DrawEnroll_Site.Text = dr["SITESEL"].ToString();
                        Txt_DrawEnroll_Room.Text = dr["ROOM"].ToString();
                        Txt_DrawEnroll_AMPM.Text = dr["AMPM"].ToString();

                        if (dr["FundedSource"].ToString() == "Y")
                            rbFundAll.Checked = true;
                        else
                        {
                            rbFundSel.Checked = true;
                            SelFundingList.Clear();
                            string strFunds = dr["FundingCode"].ToString();
                            List<string> siteList1 = new List<string>();
                            if (strFunds != null)
                            {
                                string[] FundCodes = strFunds.Split(',');
                                for (int i = 0; i < FundCodes.Length; i++)
                                {
                                    CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                                    SelFundingList.Add(fundDetails);
                                }
                            }
                            SelFundingList = SelFundingList;
                        }
                    }

                    if (columns.Contains("ALERTCODES"))
                    {
                        if (dr["ALERTCODES"].ToString() == "A")
                            rdbAlertAllCds.Checked = true;
                        else if (dr["ALERTCODES"].ToString() == "S")
                            rdbSelAlertCds.Checked = true;
                    }
                    else
                        rdbAlertAllCds.Checked = true;

                    if (columns.Contains("SELECTEDALERTCODES"))
                    {
                        if (dr["SELECTEDALERTCODES"].ToString() != "")
                        {
                            Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                        }
                    }
                }
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

        private void btnRepMaintPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void Cmb_Matrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_Bench_Marks_List();
            GetScalesList();

        }

        private void MATB0002_Form_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void Pb_Prog_Click(object sender, EventArgs e)
        {
            string Sel_Prog = (!string.IsNullOrEmpty(Txt_Program.Text.Trim()) ? Txt_Program.Text.Substring(0, 6) : "");
            if (!string.IsNullOrEmpty(Sel_Prog.Trim())) Sel_Prog = Sel_Prog.Substring(0, 2) + "-" + Sel_Prog.Substring(2, 2) + "-" + Sel_Prog.Substring(4, 2).ToString();
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, "Master", "", "*", "R");
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, "Master", "", "A", "R");
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Service", "I", "A", "R", 8);

            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnProgramClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Sel_CAMS_Program = "";
        private void OnProgramClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_CAMS_Program = form.Selected_SerPlan_Prog();

                Txt_Program.Text = "";
                //Txt_Program.Text = Sel_CAMS_Program;

                //SetComboBoxValue(Cmb_Program, Sel_Prog);
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;

                Fill_Programs_Grid(selectedHierarchies);


            }
        }

        string hierarchy = string.Empty;
        private void Fill_Programs_Grid(List<HierarchyEntity> selectedHierarchies)
        {
           
            int Rows_Cnt = 0;
            // HierarchyGrid.Rows.Clear();
            HierarchyGrid.Rows.Clear();
            if (selectedHierarchies.Count > 0)
            {
                HierarchyGrid.Rows.Clear();
                string Agy = "**", Dept = "**", Prog = "**";
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    Agy = Dept = Prog = "**";
                    if (!string.IsNullOrEmpty(row.Agency.Trim()))
                        Agy = row.Agency.Trim();

                    if (!string.IsNullOrEmpty(row.Prog.Trim()))
                        Prog = row.Prog.Trim();

                    if (!string.IsNullOrEmpty(row.Dept.Trim()))
                        Dept = row.Dept.Trim();

                    int rowIndex = HierarchyGrid.Rows.Add(row.Code + "  " + row.HirarchyName.ToString(), Agy + Dept + Prog);
                    HierarchyGrid.Rows[rowIndex].Tag = row;
                    Rows_Cnt++;

                    //hierarchy += row.Agency + row.Dept + row.Prog;
                    hierarchy += row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2) + ", ";
                }

                if (Rows_Cnt > 0)
                    Txt_Program.Text = hierarchy.Substring(0, hierarchy.Length - 2);
            }
        }

        private void Cb_Enroll_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(Asof_From_Date, null); _errorProvider.SetError(Asof_To_Date, null);
            _errorProvider.SetError(rbFundSel, null); _errorProvider.SetError(Rb_Sel_Scales, null);
            _errorProvider.SetError(Asmt_F_Date, null); _errorProvider.SetError(Asmt_T_Date, null);
            _errorProvider.SetError(Base_FDate, null); _errorProvider.SetError(Base_TDate, null);
            _errorProvider.SetError(Pb_Prog, null); _errorProvider.SetError(Pb_Withdraw_Enroll, null);
            if (Cb_Enroll.Checked)
            {
                this.Size = new Size(760, 455);//422);
                //this.pnlFunds.Location = new System.Drawing.Point(331, 210);
                Program_Panel.Visible = Site_Panel.Visible = false;  pnlFunds.Visible = false;
                cmbEnrlStatus.Visible = Lbl_Prog_Req.Visible = Lbl_Asof_Req.Visible = lbl_AsofTo.Visible =
                Lbl_Asof_Date.Visible = Asof_From_Date.Visible = Asof_From_Date.Checked = Asof_To_Date.Checked =
                 Asof_To_Date.Visible = true;
                //Txt_Program.Visible = LblProgram.Visible = Pb_Prog.Visible =
                Asof_To_Date.Value = Asof_From_Date.Value = DateTime.Today;

                if (Privileges.ModuleCode == "03")
                    Program_Panel.Visible = true;
                else if (Privileges.ModuleCode == "02")
                { this.Size = new Size(760, 455);  Site_Panel.Visible = true; pnlFunds.Visible = true; }

                FillEnrollStatus();
            }
            else
            {
                this.Size = new Size(760, 425);//389);
                //this.pnlFunds.Location = new System.Drawing.Point(331, 238);
                cmbEnrlStatus.Visible = Lbl_Prog_Req.Visible = Lbl_Asof_Req.Visible = lbl_AsofTo.Visible =
                Lbl_Asof_Date.Visible = Asof_From_Date.Visible = Asof_From_Date.Checked = Asof_To_Date.Checked =
                Asof_To_Date.Visible =                
                //Txt_Program.Visible = LblProgram.Visible = Pb_Prog.Visible =
                Program_Panel.Visible = Site_Panel.Visible = false; pnlFunds.Visible = false;
            }
        }

        private void btnChangeDates_Click(object sender, EventArgs e)
        {
            string Scale_codes = "A";
            if (Rb_Sel_Scales.Checked) Scale_codes = Get_Sel_Sites();

            MAT00003AssessmentDate Mat_ass_date = new MAT00003AssessmentDate(BaseForm, Privileges, ((ListItem)Cmb_Matrix.SelectedItem).Value.ToString(), Scale_codes, "Matrix", "Report", Agency, Depart, Program, strYear);
            Mat_ass_date.StartPosition = FormStartPosition.CenterScreen;
            Mat_ass_date.ShowDialog();
        }

        private void Pb_Withdraw_Enroll_Click(object sender, EventArgs e)
        {
            Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    "), Privileges);
            SiteSelection.FormClosed += new FormClosedEventHandler(Get_Site_AddForm_Closed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();
        }

        private void Get_Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_DrawEnroll_Site.Text = From_Results[0];
                Txt_DrawEnroll_Room.Text = From_Results[1];
                Txt_DrawEnroll_AMPM.Text = From_Results[2];
            }
        }

        private void Btn_Site_Click(object sender, EventArgs e)
        {
            //Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", , Privileges);
            //string Site_Year = "    ";
            //if (CmbYear.Visible) { if (((ListItem)CmbYear.SelectedItem).Text.ToString() == "All") Site_Year = "    "; else Site_Year = ((ListItem)CmbYear.SelectedItem).Text.ToString(); }
            Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Site", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    "), Privileges);
            SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();
        }

        private void Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_DrawEnroll_Site.Text = From_Results[0];
                Txt_DrawEnroll_Room.Text = "";
                Txt_DrawEnroll_AMPM.Text = "";
                //if (!string.IsNullOrEmpty(txtSite.Text))
                //{
                //    cmbMonth.Enabled = true;
                //    chkbFund.Visible = true;
                //    label2.Visible = true;
                //}
                //else
                //    chkbFund.Visible = false;
            }
        }

        //

        private void sites()
        {
            cmbSite.Items.Clear();

            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency;
            Search_Entity.SiteROOM = "0000";

            propCaseAllSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            cmbSite.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Sites", "0", " ", Color.Black));

            string SiteAgency = Agency.Trim();
        }

        private void Fill_Sites()
        {
            cmbSite.Items.Clear();
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            /*if(Current_Hierarchy.Substring(0,2).ToString() == "**")
            {
                Agency = " ";
            }*/
            Search_Entity.SiteAGENCY = Agency;
            Search_Entity.SiteROOM = "0000";

            propCaseAllSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
            propCaseAllSiteEntity = propCaseAllSiteEntity.OrderByDescending(u => u.SiteACTIVE.Trim()).ToList();
            cmbSite.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Sites", "0", " ", Color.Black));

            string SiteAgency = Agency.Trim();

            //DataSet ds = Captain.DatabaseLayer.Lookups.GetCaseSite();
            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(SiteAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable Sites_Table = ds.Tables[0];
                if (Sites_Table.Rows.Count > 0)
                {
                    //if (Mode.Equals("Add"))
                    //{
                    DataView dv = new DataView(Sites_Table);
                    dv.Sort = "SITE_ACTIVE DESC";//"SITE_NAME";
                    Sites_Table = dv.ToTable();
                    //}

                    foreach (DataRow dr in Sites_Table.Rows)
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Black : Color.Red)));
                    //listItem.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString().Trim(), dr["SITE_NUMBER"].ToString().Trim()));
                }
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        { hierarchyEntity = null; }
                    }

                    if (hierarchyEntity != null)
                    {
                        List<Captain.Common.Utilities.ListItem> listItemSite = new List<Captain.Common.Utilities.ListItem>();
                        //listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');

                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (Captain.Common.Utilities.ListItem casesite in listItem) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.Value.ToString())
                                        {
                                            listItemSite.Add(casesite);
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            //strsiteRoomNames = hierarchyEntity.Sites;
                            listItem = listItemSite;
                        }

                    }
                }
            }
            cmbSite.Items.AddRange(listItem.ToArray());
            cmbSite.SelectedIndex = 0;
        }

        #region Vikash added Alerts Codes Parameter on 03/21/2024 as per 2024 Enhancements document 

        private void rdbSelAlertCds_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, SelectedAlertCodes);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            objform.ShowDialog();
        }

        private void rdbAlertAllCds_Click(object sender, EventArgs e)
        {
            SelectedAlertCodes = string.Empty;
            _errorProvider.SetError(rdbSelAlertCds, null);
        }

        string SelectedAlertCodes = string.Empty;
        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelectedAlertCodes = form.fillAlertCodesforReport();
            }
            else
                form.Close();

        }

        private string Get_Sel_AlertCodes(string alertCodes)
        {
            SelectedAlertCodes = alertCodes;

            return SelectedAlertCodes;
        }

        #endregion


    }
}



