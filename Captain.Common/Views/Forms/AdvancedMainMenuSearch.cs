#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;
using DevExpress.CodeParser.CodeStyle.Formatting;
using DevExpress.XtraReports.UI;
using DevExpress.DashboardWeb.Native;
using System.Web.UI;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AdvancedMainMenuSearch : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion


        public AdvancedMainMenuSearch(BaseForm baseForm, bool change_hierarchy, bool Add_Clients_Btn_vis)
        {
            InitializeComponent();
            BaseForm = baseForm;
            //Privileges = privileges;
            Change_Hierarchy = change_hierarchy;
            _model = new CaptainModel();
            Add_Clients_Btn_Vis = Add_Clients_Btn_vis;
            SelAgency = baseForm.BaseAgency; SelDept = baseForm.BaseDept; SelProg = baseForm.BaseProg;SelYear = BaseForm.BaseYear;
            SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;

            applicationNameControl = new ApplicationNameControl(BaseForm); //Vikash added on 08/01/2023 as a part of "Advanced Search Push Button 7/28/2023 Point" in "July 2023 Enhancements and Issues" document


            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", SelAgency, SelDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                }
            }

            propAgencyControlList = _model.ZipCodeAndAgency.GetAgencyControlFileALL();
            //GetDefaultHierarchy();
            Called_From = "Mainmenu";
            MtxtAppNo.Validator = TextBoxValidation.IntegerValidator;

            if (Change_Hierarchy)
            {
                GetManimenu_Global_Hierarchy();
                //this.BorderStyle = Wisej.Web.BorderStyle.Fixed3D;
            }
            else
            {
                Agency = DefAgy = Gbl_Agy = BaseForm.BaseAgency; Dept = DefDept = Gbl_Dept = BaseForm.BaseDept;
                Program = DefProg = Gbl_Prog = BaseForm.BaseProg; ProgramYear = DefYear = Gbl_Year = "    "; DefHieExist = true;

                BtnAddApp.Visible = false;

                //BtnCancel.Visible = false;
                //    this.BtnSelApp.Location = new System.Drawing.Point(606, 349);
            }

            Get_CaseWorker_List();
            if (BaseForm.BusinessModuleID == "05")
            {
                this.pnl2.Location = new System.Drawing.Point(2, 146);
                this.GvwSearch.Size = new System.Drawing.Size(760, 275);
                this.GvwSearch.Location = new System.Drawing.Point(3, 50);
                this.Size = new System.Drawing.Size(773, 526);
                this.SeacrhPanel.Size = new System.Drawing.Size(767, 86);
                GvwSearch.ColumnHeadersVisible = false;
                pnlgvwHeaders.Visible = true;
                //lblFDate.Visible = true;
                PnlRow4.Visible = true;
                lblToDate.Visible = true;
                dtFDate.Visible = true;
                dtTDate.Visible = true;
                dtFDate.Value = DateTime.Now.AddYears(-2);
                PnlRow4.Visible = true;
            }
            else
            {
                if (BaseForm.BaseAgencyControlDetails.SsnDobMMenu == "D")
                {
                    this.SSN.HeaderText = "DOB";
                }

               // SeacrhPanel.Size = new System.Drawing.Size(972, (SeacrhPanel.Height - (PnlRow4.Height)));
                //this.Size = new System.Drawing.Size(972, (this.Height - (pnlgvwHeaders.Height + PnlRow4.Height)));
                this.Size = new System.Drawing.Size(972, (this.Height - (pnlgvwHeaders.Height)));
            }

            Fill_SearchControl_Default();
            FillAllDropdowns();

            //Get_ClientIntake_Priv(); //111720112 use it in Program selection change
            //Get_CASEDEP_List();
            //
            MtxtAppNo.Focus();
            MtxtAppNo.TabIndex = 1;

            if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
                CbSearch.Visible = false;
            else
                CbSearch.Visible = true;


            //if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
            //{
            //    CbSearch.Checked = false;
            //    CbSearch.Enabled = false;
            //    CbSearch.Visible = false;
            //}
            //else
            //{
            //    CbSearch.Enabled = true;
            //    CbSearch.Visible = true;
            //    CbSearch.Checked = true;
            //    //if (Agencydata.SearchDataBase.ToString() == "1")
            //    //    CbSearch.Checked = true;
            //    //else
            //    //    CbSearch.Checked = false;
            //}

        }

        List<AgencyControlEntity> propAgencyControlList { get; set; }

        public AdvancedMainMenuSearch(BaseForm baseForm, string Sel_Agency, string Sel_Dept, string Sel_Prog, string Sel_Year, string Sel_App)
        {
            InitializeComponent();
            BaseForm = baseForm;
            //Privileges = privileges;
            Add_Clients_Btn_Vis = Change_Hierarchy = true;
            SelAgency = baseForm.BaseAgency; SelDept = baseForm.BaseDept; SelProg = baseForm.BaseProg;
            SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;
            _model = new CaptainModel();
            propAgencyControlList = _model.ZipCodeAndAgency.GetAgencyControlFileALL();

            //GetDefaultHierarchy();

            MtxtAppNo.Validator = TextBoxValidation.IntegerMaskValidator;


            Called_From = "Enrollement";
            Gbl_Agy = Sel_Agency;
            Gbl_Dept = Sel_Dept;
            Gbl_Prog = Sel_Prog;
            Gbl_Year = !string.IsNullOrEmpty(Sel_Year) ? Sel_Year : "    ";
            Gbl_AppNo = Sel_App;

            Agency = DefAgy = Gbl_Agy; Dept = DefDept = Gbl_Dept; Program = DefProg = Gbl_Prog; ProgramYear = DefYear = Gbl_Year; DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", SelAgency, SelDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                }
            }


            if (BaseForm.BusinessModuleID == "05")
            {
                this.pnl2.Location = new System.Drawing.Point(2, 146);
                this.GvwSearch.Size = new System.Drawing.Size(760, 275);
                this.GvwSearch.Location = new System.Drawing.Point(3, 50);
                this.Size = new System.Drawing.Size(773, 526);
                this.SeacrhPanel.Size = new System.Drawing.Size(767, 86);
                GvwSearch.ColumnHeadersVisible = false;
                pnlgvwHeaders.Visible = true;
                //lblFDate.Visible = true;
                PnlRow4.Visible = true;
                lblToDate.Visible = true;
                dtFDate.Visible = true;
                dtTDate.Visible = true;
                dtFDate.Value = DateTime.Now.AddYears(-2);
                PnlRow4.Visible = true;
            }
            else
            {
                if (BaseForm.BaseAgencyControlDetails.SsnDobMMenu == "D")
                {
                    this.SSN.HeaderText = "DOB";
                }
            }
            FillAllDropdowns();
            Fill_SearchControl_Default();
            //if (Change_Hierarchy)
            //{
            //    GetManimenu_Global_Hierarchy();
            //    //this.BorderStyle = Wisej.Web.BorderStyle.Fixed3D;
            //}
            //else
            //{
            //    BtnCancel.Visible = false;
            //    this.BtnSelApp.Location = new System.Drawing.Point(606, 349);
            //}

            //Get_ClientIntake_Priv(); //111720112 use it in Program selection change
            //Get_CASEDEP_List();      


            MtxtAppNo.Focus();
            MtxtAppNo.TabIndex = 1;

            if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
                CbSearch.Visible = false;
            else
                CbSearch.Visible = true;
        }



        private void AdvancedMainMenuSearch_Load(object sender, EventArgs e)
        {
            Get_CASEDEP_List();
            //Get_CASEHIE_list();

            if (!string.IsNullOrEmpty(BaseForm.BaseAgency) &&
                !string.IsNullOrEmpty(BaseForm.BaseDept) &&
                !string.IsNullOrEmpty(BaseForm.BaseProg))
            {
                Agency = BaseForm.BaseAgency;
                Dept = BaseForm.BaseDept;
                Program = BaseForm.BaseProg;
                Get_ClientIntake_Priv(); //111720112 use it in Program selection change
            }

            if (Called_From != "Mainmenu")
            {
                MtxtAppNo.Text = Gbl_AppNo;
                MtxtAppNo_LostFocus(MtxtAppNo, EventArgs.Empty);
            }


            if (!Add_Clients_Btn_Vis)
            {
                BtnAddApp.Visible = Btn_Drag_App.Visible = false;
            }

            MtxtAppNo.Focus();
            MtxtAppNo.TabIndex = 1;
        }

        public bool Change_Hierarchy { get; set; }

        public bool Add_Clients_Btn_Vis { get; set; }

        PrivilegeEntity Privileges = new PrivilegeEntity();


        bool Loading_Complete = false;

        string SearcgCategory = "7";  // Applicant No Default
        string SearchFor = "1";      // All as Sefault
        string SearchCaseType = "**";
        string SearchCaseWRK = null;
        string PrvPanelCode = null;

        string SelAgency = null;
        string SelDept = null;
        string SelProg = null;
        string SelYear = null;

        string Hierarchy = null;
        string SearchHie = null;
        string strNameFormat = null, strCwFormat = null;

        string Gbl_Agy = null;
        string Gbl_Dept = null;
        string Gbl_Prog = null;
        string Gbl_Year = null;
        string Gbl_AppNo = null;
        bool Gbl_HieExist = false;

        string DefAgy = null;
        string DefDept = null;
        string DefProg = null;
        string DefYear = null;
        string DepYear = null;
        bool DefHieExist = false;

        string Called_From = "Mainmenu";

        public BaseForm BaseForm { get; set; }

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string ProgramYear { get; set; }
        public char AddPriv { get; set; }

        public string propSetDefaultSwitch { get; set; }

        private void GetDefaultHierarchy()
        {
            DefAgy = DefDept = DefProg = DefYear = null;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetUserDefHierarchy(BaseForm.UserID);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    DefAgy = dr["Agy"].ToString();
                    DefDept = dr["Dept"].ToString();
                    DefProg = dr["Prog"].ToString();
                    DefYear = dr["DefYear"].ToString();
                    Agency = DefAgy;
                    Dept = DefDept;
                    Program = DefProg;
                    if (!(String.IsNullOrEmpty(DefAgy)) && !(String.IsNullOrEmpty(DefDept)) && !(String.IsNullOrEmpty(DefProg)))
                        DefHieExist = true;
                }
            }
        }


        private void GetManimenu_Global_Hierarchy()
        {
            if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            {
                MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
                //Gbl_Agy = mainMenuControl.Agency;
                //Gbl_Dept = mainMenuControl.Dept;
                //Gbl_Prog = mainMenuControl.Program;
                //Gbl_Year = mainMenuControl.ProgramYear;
                //Gbl_AppNo = mainMenuControl.ApplicationNo;

                Gbl_Agy = BaseForm.BaseAgency;
                Gbl_Dept = BaseForm.BaseDept;
                Gbl_Prog = BaseForm.BaseProg;
                Gbl_Year = !string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ";
                Gbl_AppNo = BaseForm.BaseApplicationNo;

                Agency = DefAgy = Gbl_Agy; Dept = DefDept = Gbl_Dept; Program = DefProg = Gbl_Prog; ProgramYear = DefYear = Gbl_Year; DefHieExist = true;
            }
        }









        DataTable User_Hie_Acc_List = new DataTable();
        string Dep_Program_Year = "    ";
        private void FillYearCombo()
        {
            //BtnSelApp.Visible = Btn_Drag_App.Visible = false;
            //DefHieExist = false;
            //BtnAddApp.Text = "Add a Brand New Application";
            Dep_Program_Year = "    ";
            if (!string.IsNullOrEmpty(DefYear.Trim()))
                DefHieExist = true;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(SelAgency, SelDept, SelProg);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Dep_Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(DefYear)) && DefYear != null && DefYear != " " && DefHieExist)
                        {
                            if (!String.IsNullOrEmpty(DefYear.Trim()))
                                TempCompareYear = int.Parse(DefYear);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Dep_Program_Year.Trim()))
                            {
                                TempCompareYear = int.Parse(Dep_Program_Year);
                                DefHieExist = true;
                            }

                        }
                        //List<ListItem> listItem = new List<ListItem>();
                        //listItem.Add(new ListItem("", "10"));
                        //for (int i = 0; i < 10; i++)
                        //{
                        //    TmpYearStr = (TmpYear - i).ToString();
                        //    listItem.Add(new ListItem(TmpYearStr, i));
                        //    if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                        //        YearIndex = i;
                        //}

                        //CmbYear.Items.AddRange(listItem.ToArray());

                        //CmbYear.Visible = true;

                        //if (DefHieExist)
                        //    CmbYear.SelectedIndex = YearIndex + 1;
                        //else
                        //    CmbYear.SelectedIndex = 0;
                    }
                }
            }
        }

        List<ProgramDefinitionEntity> CASEDEP_List = new List<ProgramDefinitionEntity>();
        private void Get_CASEDEP_List()
        {
            //CASEDEP_List = _model.HierarchyAndPrograms.GetCaseGdl();
            CASEDEP_List = _model.HierarchyAndPrograms.GetPrograms(string.Empty, string.Empty, BaseForm.UserID, BaseForm.BaseAdminAgency);

        }


        private bool Check_PROG_Existance_In_CASEDEP()
        {
            bool PROG_Defined = false;
            if (CASEDEP_List.Count > 0)
            {
                foreach (ProgramDefinitionEntity Entity in CASEDEP_List)
                {
                    if (Entity.Agency == SelAgency &&
                       Entity.Dept == SelDept &&
                       Entity.Prog == SelProg)
                    { PROG_Defined = true; break; }
                }
            }

            return PROG_Defined;
        }

        private void FillAllDropdowns()
        {
            List<ListItem> listItem = new List<ListItem>();

            CmbSearchFor.Items.Clear();
            listItem = new List<ListItem>();
            listItem.Add(new ListItem("All", "1"));
            listItem.Add(new ListItem("Applicants", "2"));
            listItem.Add(new ListItem("Members", "3"));
            CmbSearchFor.Items.AddRange(listItem.ToArray());

            FillCmbCaseType();
            FillCaseWroker();
            SetComboBoxValue(CmbSearchFor, SearchFor);

            //FillYearCombo();

            //pnlSearchBy.Size = new System.Drawing.Size(893, 35);
            //SeacrhPanel.Size = new System.Drawing.Size(893, 35 + panel5.Height);
            //this.Size = new System.Drawing.Size(883, 538);

        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        public void FillCmbCaseType()
        {

            CmbCaseType.Items.Clear();

            List<ListItem> listItem = new List<ListItem>();
            listItem = new List<ListItem>();
            //listItem.Add(new ListItem("All", "0"));
            DataSet ds = Captain.DatabaseLayer.ZipCodePlusAgency.GetCaseType();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                    listItem.Add(new ListItem(dr["Agy_8"].ToString(), dr["Agy_2"].ToString()));

                CmbCaseType.Items.AddRange(listItem.ToArray());
                if (string.IsNullOrEmpty(SearchCaseType))
                    SearchCaseType = "**";
                //if (SearchCaseType.Equals("**"))
                //    SearchCaseType = "0";
                SetComboBoxValue(CmbCaseType, SearchCaseType);
                //CmbCaseType.SelectedIndex = 0;

            }
        }



        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencies(string Agency)
        {
            Member_NameFormat = CAseWorkerr_NameFormat = "1";
            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
            //        CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            //    }
            //}
            HierarchyEntity hiername = BaseForm.BaseCaseHierachyListEntity.Find(u => u.Agency.Trim() == Agency && u.Dept.Trim() == string.Empty && u.Prog.Trim() == string.Empty);
            if (hiername != null)
            {
                Member_NameFormat = hiername.CNFormat.ToString();
                CAseWorkerr_NameFormat = hiername.CWFormat.ToString();
            }
        }





        private void Fill_SearchControl_Default()
        {
            if (propAgencyControlList.Count > 0)
            {
                AgencyControlEntity Agencydata = propAgencyControlList.Find(u => u.AgencyCode.ToString().Trim() == SelAgency); // Kranthi Commented
                if (Agencydata != null)
                {
                    SearcgCategory = "7";  // Applicant No Default
                    SearchFor = "1";      // All as Sefault

                    if (!string.IsNullOrEmpty(Agencydata.SearchBy.ToString()))
                        SearcgCategory = Agencydata.SearchBy.ToString();
                    if (!string.IsNullOrEmpty(Agencydata.SearchFor.ToString()))
                        SearchFor = Agencydata.SearchFor.ToString();
                    if (!string.IsNullOrEmpty(Agencydata.SearchCaseType.ToString()))
                        SearchCaseType = Agencydata.SearchCaseType.ToString();

                    if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
                    {
                        CbSearch.Checked = false;
                        CbSearch.Enabled = false;
                        CbSearch.Visible = false;
                    }
                    else
                    {
                        CbSearch.Enabled = true;
                        CbSearch.Visible = true;
                        if (Agencydata.SearchDataBase.ToString() == "1")
                            CbSearch.Checked = true;
                        else
                            CbSearch.Checked = false;
                    }

                }
            }
            //DataSet ds = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(((ListItem)CmbAgency.SelectedItem).Value.ToString(), null, null, null, null, null, null);
            //DataRow dr;
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        dr = ds.Tables[0].Rows[0];

            //        SearcgCategory = "7";  // Applicant No Default
            //        SearchFor = "1";      // All as Sefault

            //        if (!string.IsNullOrEmpty(dr["ACR_SEARCH_BY"].ToString()))
            //            SearcgCategory = dr["ACR_SEARCH_BY"].ToString();
            //        if (!string.IsNullOrEmpty(dr["ACR_SEARCH_FOR"].ToString()))
            //            SearchFor = dr["ACR_SEARCH_FOR"].ToString();
            //        if (!string.IsNullOrEmpty(dr["ACR_SEARCH_CASETYPE"].ToString()))
            //            SearchCaseType = dr["ACR_SEARCH_CASETYPE"].ToString();

            //        if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
            //        {
            //            CbSearch.Checked = false;
            //            CbSearch.Enabled = false;
            //            CbSearch.Visible = false;
            //        }
            //        else
            //        {
            //            CbSearch.Enabled = true;
            //            CbSearch.Visible = true;
            //            if (dr["ACR_SEARCH_DATABASE"].ToString() == "1")
            //                CbSearch.Checked = true;
            //            else
            //                CbSearch.Checked = false;
            //        }
            //    }
            //}
        }



        private void Set_Add_App_Visibility()
        {
            object test = "BtnAddApp";
            BtnAddApp.Visible = false;
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                switch (BaseForm.UserProfile.Security)
                {
                    case "R":
                    case "C":
                        if (Dep_Program_Year != BaseForm.BaseYear.Trim())
                        {
                            //MessageBox.Show("Cannot add to prior program year", "CAP Systems");   //Kranthi 03/01/2023 :: NCCAA.docx :: we are not using hierarchy for advanced search so we are commenting the line
                            BtnAddApp.Visible = false;
                        }
                        //else
                        //{
                        //    if (AddPriv == 'Y' && Change_Hierarchy && Add_Clients_Btn_Vis)
                        //        BtnAddApp.Visible = true;
                        //}
                        break;

                        //case "P":
                        //case "B":
                        //    if (AddPriv == 'Y' && Change_Hierarchy && Add_Clients_Btn_Vis)
                        //        BtnAddApp.Visible = true;
                        //    break;
                }
            }
            else
            {
                //if (AddPriv == 'Y' && Change_Hierarchy && Add_Clients_Btn_Vis)
                //    BtnAddApp.Visible = true;
            }
        }

        private void InitializeAllControls()
        {
            Loading_Complete = false;

            GvwSearch.Rows.Clear();
            //GvwAppHou.Rows.Clear();

            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible =
           SeacrhPanel.Visible = false;
            //CbSearch.Visible = false;
            //AppDetailsPanel.Visible = false;
            Clear_Search_Controls();
            //SetRelatedSearchControls(SearcgCategory);
        }

        private void FillCaseWroker()
        {
            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(SelAgency, SelDept, SelProg);
            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(SelAgency, "**", "**");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    strNameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
            //    strCwFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            //}
            HierarchyEntity hiername = BaseForm.BaseCaseHierachyListEntity.Find(u => u.Agency.Trim() == Agency && u.Dept.Trim() == string.Empty && u.Prog.Trim() == string.Empty);
            if (hiername != null)
            {
                strNameFormat = hiername.CNFormat.ToString();
                strCwFormat = hiername.CWFormat.ToString();
            }
            CmbCaseWrk.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            CmbCaseWrk.Items.Insert(0, new ListItem("All", "**"));
            List<HierarchyEntity> hierarchyEntity = _model.CaseMstData.GetCaseWorker(strCwFormat, SelAgency, SelDept, SelProg);
            foreach (HierarchyEntity item in hierarchyEntity)
            {
                CmbCaseWrk.Items.Add(new ListItem(item.HirarchyName.ToString(), item.CaseWorker.ToString()));
            }

            //DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, SelAgency, SelDept, SelProg);
            //if (ds1.Tables.Count > 0)
            //{
            //    DataTable dt = ds1.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //            listItem.Add(new ListItem(dr["NAME"].ToString(), dr["PWH_CASEWORKER"].ToString()));
            //    }
            //}
            //CmbCaseWrk.Items.AddRange(listItem.ToArray());
            CmbCaseWrk.SelectedIndex = 0;
        }



        private bool Check_Search_Criteria()
        {
            bool Can_Search = false;
            //switch (SearcgCategory)
            //{
            //    case "NAM":
            //        if ((!string.IsNullOrEmpty(TxtFName.Text.Trim())) || (!string.IsNullOrEmpty(TxtLName.Text.Trim())))
            //            Can_Search = true; break;
            //    case "SSN":
            //        if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
            //            Can_Search = true; break;
            //    case "ADD":
            //        if ((!string.IsNullOrEmpty(TxtHNo.Text.Trim())) || (!string.IsNullOrEmpty(TxtState.Text.Trim())) ||
            //            (!string.IsNullOrEmpty(TxtCity.Text.Trim())) || (!string.IsNullOrEmpty(TxtState.Text.Trim())))
            //            Can_Search = true; break;
            //    case "PHN":
            //        if (!string.IsNullOrEmpty(MtxtPhone.Text.Trim()))
            //            Can_Search = true; break;
            //    case "ALS":
            //        if (!string.IsNullOrEmpty(TxtAlias.Text.Trim()))
            //            Can_Search = true; break;
            //    case "SCN":
            //        if (!string.IsNullOrEmpty(TxtScanApp.Text.Trim()))
            //            Can_Search = true; break;
            //    case "APP":
            //        if (!string.IsNullOrEmpty(MtxtAppNo.Text.Trim()))
            //            Can_Search = true; break;
            //    case "DOB":
            //        if ((dtBirth.Checked == true))       // if ((!string.IsNullOrEmpty(txtdtFirstName.Text.Trim())) && (dtBirth.Checked == true)) //Kranthi Commented
            //            Can_Search = true; break;
            //}

            if ((!string.IsNullOrEmpty(TxtFName.Text.Trim())) || (!string.IsNullOrEmpty(TxtLName.Text.Trim())))
                Can_Search = true;
            if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
                Can_Search = true;
            if ((!string.IsNullOrEmpty(TxtHNo.Text.Trim())) ||
                (!string.IsNullOrEmpty(TxtCity.Text.Trim())))
                Can_Search = true;
            if (!string.IsNullOrEmpty(MtxtPhone.Text.Trim()))
                Can_Search = true;
            if (!string.IsNullOrEmpty(TxtAlias.Text.Trim()))
                Can_Search = true;
            if (!string.IsNullOrEmpty(TxtScanApp.Text.Trim()))
                Can_Search = true;
            if (!string.IsNullOrEmpty(MtxtAppNo.Text.Trim()))
                Can_Search = true;
            if (!string.IsNullOrEmpty(TxtStreet.Text.Trim()))
                Can_Search = true;
            if (!string.IsNullOrEmpty(MtxtHomeMobNumber.Text.Trim()))
                Can_Search = true;

            if ((dtBirth.Checked == true))       // if ((!string.IsNullOrEmpty(txtdtFirstName.Text.Trim())) && (dtBirth.Checked == true)) //Kranthi Commented
                Can_Search = true;

            if (((ListItem)CmbCaseWrk.SelectedItem).Value.ToString() != "**" ||
               ((ListItem)CmbCaseType.SelectedItem).Value.ToString() != "**")
                Can_Search = true;
            if (Can_Search)
            {
                if (BaseForm.BusinessModuleID == "05")
                {
                    if (dtFDate.Value > dtTDate.Value)
                    {
                        Can_Search = false;
                    }
                }
            }

            return Can_Search;
        }

        private void BtnSearcRecs_Click(object sender, EventArgs e)
        {
            //SearchCaseWRK = 
            //SearchCaseWRK = ((ListItem)CmbCaseWrk.SelectedItem).Text.ToString();
            SearchFor = ((ListItem)CmbSearchFor.SelectedItem).Text.ToString();
            try
            {

                if (Check_Search_Criteria())
                {
                    //int intcount = 3;//Captain.DatabaseLayer.MainMenu.GETADHOCHISTORY("MAINMENU", DateTime.Now.Date.ToShortDateString(), string.Empty);
                    //if (intcount < 5)
                    //{

                    if (AddPriv == 'Y' && Change_Hierarchy && Add_Clients_Btn_Vis)
                        BtnAddApp.Visible = true;


                    string strParamerterData = string.Empty;
                    string strOutputId = string.Empty;
                    BtnSelApp.Visible = Btn_Drag_App.Visible = false;
                    BtnSearcRecs.Enabled = false;
                    //BtnAddApp.Text = "Add a Brand New Application";
                    Loading_Complete = false;
                    if (SearchCaseType.Equals("**"))
                        SearchCaseType = string.Empty;
                    //SetControlsVisibility('N');
                    GvwSearch.Rows.Clear();
                    Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
                    Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;

                    DataSet ds = new DataSet();
                    DataTable dtEMSCLCPMC = new DataTable();
                    List<CommonEntity> Decisionentity = new List<CommonEntity>();

                    strParamerterData = "Agency  " + SelAgency;                         //Kranthi Commented
                    strParamerterData = strParamerterData + ", Dept  " + SelDept;       //Kranthi Commented
                    strParamerterData = strParamerterData + ", Program  " + SelProg;   //Kranthi Commented

                    strParamerterData = strParamerterData + ", Search For  " + ((ListItem)CmbSearchFor.SelectedItem).Text.ToString();

                    strParamerterData = strParamerterData + ", SearchCaseType  " + ((ListItem)CmbCaseType.SelectedItem).Text.ToString();
                    strParamerterData = strParamerterData + ", SearchCaseWRK  " + ((ListItem)CmbCaseWrk.SelectedItem).Text.ToString();

                    if (MtxtAppNo.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Applicant  " + MtxtAppNo.Text.Trim();
                    if (TxtLName.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", LastName  " + TxtLName.Text.Trim();
                    if (TxtFName.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", FirstName  " + TxtFName.Text.Trim();
                    if (MtxtSsn.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", SSN Number  " + MtxtSsn.Text.Trim();
                    if (TxtHNo.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", House Number  " + TxtHNo.Text.Trim();
                    if (TxtStreet.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Street " + TxtStreet.Text.Trim();
                    if (TxtCity.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", City " + TxtCity.Text.Trim();
                    //if (TxtState.Text.Trim() != string.Empty)
                    //    strParamerterData = strParamerterData + ", State " + TxtState.Text.Trim();
                    if (MtxtPhone.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Phone  " + MtxtPhone.Text.Trim();
                    if (TxtAlias.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Alias  " + TxtAlias.Text.Trim();
                    if (TxtScanApp.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Applicant  " + TxtScanApp.Text.Trim();
                    if (MtxtHomeMobNumber.Text.Trim() != string.Empty)
                        strParamerterData = strParamerterData + ", Mobile  " + MtxtHomeMobNumber.Text.Trim();

                    // if (txtdtFirstName.Text.Trim() != string.Empty)
                    // strParamerterData = strParamerterData + ", FirstName  " + txtdtFirstName.Text.Trim();
                    if (dtBirth.Checked)
                        strParamerterData = strParamerterData + ", DOB  " + dtBirth.Text.Trim();
                    if (CbSearch.Checked)
                        strParamerterData = strParamerterData + ", Searcg Hierachy  " + SearchHie;
                    else
                        strParamerterData = strParamerterData + ", Searcg Hierachy  All";

                    if (BaseForm.BusinessModuleID == "05")
                    {
                        strParamerterData = strParamerterData + ", FDate  " + dtFDate.Value.ToShortDateString() + " TDate " + dtTDate.Value.ToShortDateString();
                    }
                    try
                    {

                        _model.AdhocData.InsertUpdateDelAdhocHistory(BaseForm.UserID, BaseForm.BusinessModuleID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear, string.Empty, "ADD", "MAINMENU", strParamerterData, string.Empty, out strOutputId);
                    }
                    catch (Exception ex)
                    {
                    }
                    if (BaseForm.BusinessModuleID == "05")
                    {

                        Decisionentity = _model.lookupDataAccess.GetAgyDecision();
                        Decisionentity = CommonFunctions.AgyTabsDecisionCodeFilters(Decisionentity, Consts.AgyTab.DecisionType, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

                        string strFirstName = string.Empty;
                        if (TxtFName.Text.Trim() != string.Empty)
                            strFirstName = TxtFName.Text;
                        // if (txtdtFirstName.Text.Trim() != string.Empty)
                        //    strFirstName = txtdtFirstName.Text;

                        string strDob = string.Empty;
                        if (dtBirth.Checked)
                            strDob = dtBirth.Value.ToShortDateString();

                        ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchEMS(SearcgCategory, SearchFor, SearchCaseType, SearchCaseWRK, MtxtAppNo.Text, TxtLName.Text, strFirstName.Trim(), MtxtSsn.Text,
                           TxtHNo.Text, TxtStreet.Text, TxtCity.Text, "", MtxtPhone.Text, TxtAlias.Text, TxtScanApp.Text, CbSearch.Checked ? null : SearchHie, strDob, BaseForm.UserID, "EMSCLCPMC", dtFDate.Value.ToShortDateString(), dtTDate.Value.ToShortDateString());
                        if (ds.Tables.Count > 1)
                        {
                            dtEMSCLCPMC = ds.Tables[1];
                        }
                    }
                    else
                    {
                        string strFirstName = string.Empty;
                        if (TxtFName.Text.Trim() != string.Empty)
                            strFirstName = TxtFName.Text;
                        // if (txtdtFirstName.Text.Trim() != string.Empty)
                        //   strFirstName = txtdtFirstName.Text;

                        string strDob = string.Empty;
                        if (dtBirth.Checked)
                            strDob = dtBirth.Value.ToShortDateString();

                        //Added by Sudheer on 12/26/2022
                        if (BaseForm.BaseAgencyControlDetails.DeepSearchSwitch == "Y")
                        {
                            string propClientRulesSwitch = "Y";
                            string strClientSwitch = string.Empty;
                            if (propClientRulesSwitch == "Y")
                            {
                                if (BaseForm.BaseAgencyControlDetails != null)
                                    strClientSwitch = BaseForm.BaseAgencyControlDetails.ClientRules;
                            }

                            ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch(SearcgCategory, SearchFor, SearchCaseType, SearchCaseWRK, MtxtAppNo.Text, TxtLName.Text, strFirstName.Trim(), MtxtSsn.Text,
                                TxtHNo.Text, TxtStreet.Text, TxtCity.Text, "", MtxtHomeMobNumber.Text, TxtAlias.Text, TxtScanApp.Text, string.Empty, strDob, BaseForm.UserID, strClientSwitch, string.Empty, MtxtPhone.Text);
                        }
                        else
                        {
                            ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch(SearcgCategory, SearchFor, SearchCaseType, SearchCaseWRK, MtxtAppNo.Text, TxtLName.Text, strFirstName.Trim(), MtxtSsn.Text,
                               TxtHNo.Text, TxtStreet.Text, TxtCity.Text, "", MtxtHomeMobNumber.Text, TxtAlias.Text, TxtScanApp.Text, CbSearch.Checked ? null : SearchHie, strDob, BaseForm.UserID, string.Empty, SelAgency, MtxtPhone.Text); /*Kranthi Commented*///((ListItem)CmbAgency.SelectedItem).Value.ToString() 
                        }

                    }

                    //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch(SearcgCategory, SearchFor, SearchCaseType, SearchCaseWRK, MtxtAppNo.Text, TxtLName.Text, TxtFName.Text.Trim(), MtxtSsn.Text,
                    //           TxtHNo.Text, TxtStreet.Text, TxtCity.Text, TxtState.Text, MtxtPhone.Text, TxtAlias.Text, TxtScanApp.Text, CbSearch.Checked ? null : SearchHie, null, BaseForm.UserID);

                    if(CbSearch.Checked==false)
                    {

                    }


                    int TmpRows = 0;
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        try
                        {
                            string TmpHierarchy = null, DOB = null;
                            string Address = null;
                            string TmpStatus = null, Mst_Active = "";
                            string TmpAddress = null;
                            string TmpSsn = null;
                            int TmpLength = 0;
                            char TmpSpace = ' ';
                            string TmpYear = "    ";
                            string TmpName = "    ";
                            string Priv_Hie = null;
                            bool Can_Add_Cur_Hie = true, All_Hie_Access = true;
                            string TmpClientID = string.Empty;

                            if (string.IsNullOrEmpty(SearchHie))
                                All_Hie_Access = Get_Hi_Access_Status(string.Empty);

                            foreach (DataRow dr in dt.Rows)
                            {
                                int rowIndex = 0; DOB = string.Empty;
                                //Can_Add_Cur_Hie = true;

                                Address = dr["Hno"].ToString() + ' ' + dr["Street"].ToString() + ' ' + dr["City"].ToString() + ' ' + dr["State1"].ToString() + ' ' + dr["Zip"].ToString();
                                TmpSsn = dr["Ssn"].ToString();
                                TmpClientID = dr["ClientID"].ToString();
                                TmpLength = (9 - TmpSsn.Length);
                                for (int i = 0; i < TmpLength; i++)
                                    TmpAddress += TmpSpace;
                                TmpSsn += TmpAddress;
                                if (MtxtSsn.Text != string.Empty)
                                    TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);
                                else
                                    TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", string.Empty, string.Empty);

                                TmpHierarchy = dr["Agency"].ToString() + '-' + dr["Dept"].ToString() + '-' + dr["Prog"].ToString();    //RecKey
                                TmpStatus = null;
                                if (dr["AppStatus"].ToString().Trim() != "A")
                                    TmpStatus = "Inactive";

                                Mst_Active = dr["MST_ACTIVE_STATUS"].ToString().Trim();

                                TmpYear = "    ";
                                if (!string.IsNullOrEmpty(dr["SnpYear"].ToString().Trim()))
                                    TmpYear = dr["SnpYear"].ToString();

                                TmpName = null;     //dr["Fname"] + ", " + dr["Lname"]
                                TmpName = "";

                                //if (dr["Fname"].ToString().Trim().Length > 0)
                                //    TmpName = dr["Fname"].ToString().Trim();
                                //if (dr["Lname"].ToString().Trim().Length > 0)
                                //{
                                //    if (!(string.IsNullOrEmpty(TmpName)))
                                //        TmpName += ", ";
                                //    TmpName += dr["Lname"].ToString().Trim();
                                //}

                                string strIntakeDate = string.Empty;
                                if (dt.Columns.Contains("MST_INTAKE_DATE"))
                                {
                                    strIntakeDate = LookupDataAccess.Getdate(dr["MST_INTAKE_DATE"].ToString());
                                }

                                TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString().Trim(), dr["Mname"].ToString().Trim(), dr["Lname"].ToString().Trim(), Member_NameFormat);

                                if (!string.IsNullOrEmpty(dr["DOB"].ToString()))
                                    DOB = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dr["DOB"].ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                if (BaseForm.BaseAgencyControlDetails.SsnDobMMenu == "D")
                                {
                                    TmpSsn = DOB;
                                }

                                if (!All_Hie_Access)
                                {
                                    if (Priv_Hie != dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString())
                                        Can_Add_Cur_Hie = Get_Hi_Access_Status(dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString());
                                }

                                if (Can_Add_Cur_Hie)
                                {
                                    rowIndex = GvwSearch.Rows.Add(
                                        TmpHierarchy, TmpYear, dr["AppNo"].ToString(), DOB, dr["Fname"].ToString(), dr["Lname"].ToString(), dr["Mobile"].ToString(), dr["Phone"].ToString(), Address,
                                        TmpSsn, TmpName, TmpStatus, dr["CaseType"].ToString(), dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString(),
                                        dr["AppNo"].ToString(), dr["RecFamSeq"].ToString(), "Y", dr["Ssn"].ToString(),
                                        dr["MST_INTAKE_WORKER"].ToString(), strIntakeDate, dr["MST_SITE"].ToString().Trim(), dr["Street"].ToString(), dr["SNP_ALIAS"].ToString(), dr["ClientID"].ToString());

                                    //rowIndex = GvwSearch.Rows.Add(dr["Hierarchy"], dr["SnpYear"], dr["AppNo"], TmpSsn, dr["Lname"] + ", " + dr["Fname"], dr["Phone"], Address, " ", dr["CaseType"], " ", " ", " ");

                                    //GvwSearch.Rows[rowIndex].Tag = dr;
                                    //if (TmpStatus == "Inactive")
                                    //    GvwSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    //TmpRows++;
                                }
                                else
                                {
                                    rowIndex = GvwSearch.Rows.Add(TmpHierarchy, TmpYear, dr["AppNo"].ToString(), DOB, dr["Fname"].ToString(), dr["Lname"].ToString(), dr["Mobile"].ToString(), dr["Phone"].ToString(), Address,
                                        TmpSsn, TmpName, TmpStatus, dr["CaseType"].ToString(), dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString(),
                                        dr["AppNo"].ToString(), dr["RecFamSeq"].ToString(), "N", dr["Ssn"].ToString(),
                                        dr["MST_INTAKE_WORKER"].ToString(), strIntakeDate, dr["MST_SITE"].ToString().Trim(), dr["Street"].ToString(), dr["SNP_ALIAS"].ToString(), dr["ClientID"].ToString());
                                }
                                GvwSearch.Rows[rowIndex].Tag = dr;
                                //if (TmpStatus == "Inactive" || Mst_Active != "Y")
                                //    GvwSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                if (dr["AppNo"].ToString().Substring(10, 1) == "A" && Mst_Active != "Y")
                                    GvwSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                if (dr["AppNo"].ToString().Substring(10, 1) == "M" && Mst_Active != "Y")
                                    GvwSearch.Rows[rowIndex].Cells["gvtAppNo"].Style.ForeColor = Color.Red;

                                if (TmpStatus == "Inactive")
                                    GvwSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                TmpRows++;

                                Priv_Hie = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();

                                if (BaseForm.BusinessModuleID == "05")
                                {
                                    DataRow[] drclcpmc = dtEMSCLCPMC.Select("CLC_AGENCY = '" + dr["Agency"].ToString() + "' AND CLC_DEPT = '" + dr["Dept"].ToString() + "' AND CLC_PROGRAM = '" + dr["Prog"].ToString() + "' AND CLC_YEAR = '" + TmpYear.Trim() + "' AND CLC_APP = '" + dr["APPLICANTNO"].ToString() + "'");
                                    foreach (DataRow drclcitem in drclcpmc)
                                    {
                                        CommonEntity comdecision = Decisionentity.Find(u => u.Code.Trim() == drclcitem["CLC_S_DECISION"].ToString().Trim());
                                        string strdeciondesc = string.Empty;
                                        if (comdecision != null)
                                            strdeciondesc = comdecision.Desc.Trim();
                                        rowIndex = GvwSearch.Rows.Add(string.Empty, string.Empty, string.Empty, LookupDataAccess.Getdate(drclcitem["CLC_S_DECSN_DATE"].ToString()), drclcitem["CLC_S_SERVICE_CODE"].ToString() + "           " + drclcitem["CLC_RES_FUND"].ToString(), drclcitem["PMC_AMOUNT"].ToString(), "Wkr: " + drclcitem["CLC_S_CASEWORKER"].ToString() + "       Dec: " + strdeciondesc, TmpStatus, dr["CaseType"].ToString(), dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString(), dr["AppNo"].ToString(), dr["RecFamSeq"].ToString(), DOB, dr["Fname"].ToString(), dr["Lname"].ToString(), "N", dr["Ssn"].ToString(), dr["MST_INTAKE_WORKER"].ToString(), strIntakeDate, string.Empty, dr["Street"].ToString(), dr["SNP_ALIAS"].ToString(), dr["ClientID"].ToString());
                                    }
                                }

                            }
                            if (TmpRows > 0)
                            {
                                GvwSearch.Rows[0].Selected = true;
                                Loading_Complete = true;
                                //GetHie_App_Details();
                                //AppDetailsPanel.Visible = true;
                                Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = BtnSelApp.Visible = true;
                                if (BtnAddApp.Visible && Add_Clients_Btn_Vis)
                                    Btn_Drag_App.Visible = true;
                                //BtnAddApp.Text = "Copy/Drag this Application";
                                GvwSearch_SelectionChanged(GvwSearch, EventArgs.Empty);
                                //PbMax.Visible = true;
                                //SetControlsVisibility('Y');
                            }

                        }
                        catch (Exception ex) { }
                        BtnSearcRecs.Enabled = true;
                    }
                    else
                    {
                        AlertBox.Show("No Record(s) With Selected Search Criteria", MessageBoxIcon.Warning, null, ContentAlignment.MiddleCenter, autoCloseDelay:700);
                        BtnSelApp.Visible = Btn_Drag_App.Visible = false;
                        BtnSearcRecs.Enabled = true;
                        //BtnAddApp.Text = "Add a Brand New Application";
                    }

                    try
                    {
                        _model.AdhocData.InsertUpdateDelAdhocHistory(BaseForm.UserID, BaseForm.BusinessModuleID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear, string.Empty, "Edit", "MAINMENU", strParamerterData, strOutputId, out strOutputId);
                    }
                    catch (Exception ex)
                    {
                    }
                    //}
                    //else
                    //{
                    //    CommonFunctions.MessageBoxDisplay("Server is busy, try little later");
                    //}
                }
                else
                {
                    bool boolmsg = true;
                    if (BaseForm.BusinessModuleID == "05")
                    {
                        if (dtFDate.Value > dtTDate.Value)
                        {
                            boolmsg = false;
                            CommonFunctions.MessageBoxDisplay("To Date may not prior to from Date");
                        }
                    }

                    if (boolmsg)
                        AlertBox.Show("Please give Search Criteria inputs", MessageBoxIcon.Question, null, ContentAlignment.MiddleCenter, 3000, null, null, true);
                }
            }
            catch (Exception ex)
            {


            }
        }


        private bool Get_Hi_Access_Status(string Curr_Hie)
        {
            bool Can_Add_Hie = false;

            if (string.IsNullOrEmpty(Curr_Hie))
            {
                foreach (DataRow dr in User_Hie_Acc_List.Rows)
                {
                    if (dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString() + dr["PWH_PROG"].ToString() == "******")
                    {
                        Can_Add_Hie = true; break;
                    }
                }
            }
            else
            {
                if (User_Hie_Acc_List.Rows.Count > 0)
                {
                    string All_Dept_Prog = "    ";
                    foreach (DataRow dr in User_Hie_Acc_List.Rows)
                    {
                        All_Dept_Prog = "CHECK";

                        if (dr["PWH_DEPT"].ToString() == "**")
                            All_Dept_Prog = "DEPT";

                        if (dr["PWH_DEPT"].ToString() != "**" && dr["PWH_PROG"].ToString() == "**")
                            All_Dept_Prog = "PROG";


                        if (All_Dept_Prog != "CHECK")
                        {
                            switch (All_Dept_Prog)
                            {
                                case "DEPT":
                                    if (Curr_Hie.Substring(0, 2) == dr["PWH_AGENCY"].ToString())
                                        Can_Add_Hie = true;
                                    break;
                                case "PROG":
                                    if (Curr_Hie.Substring(0, 4) == dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString())
                                        Can_Add_Hie = true;
                                    break;
                            }

                            if (Can_Add_Hie)
                                break;
                        }
                        else
                        {
                            if (Curr_Hie == dr["PWH_AGENCY"].ToString() + dr["PWH_DEPT"].ToString() + dr["PWH_PROG"].ToString())
                            {
                                Can_Add_Hie = true; break;
                            }
                        }
                    }
                }
            }

            return Can_Add_Hie;
        }


        private void MtxtAppNo_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MtxtAppNo.Text))
            {
                MtxtAppNo.Text = SetLeadingZeros(MtxtAppNo.Text);
                BtnSearcRecs_Click(BtnSearcRecs, EventArgs.Empty);
            }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }

        private void CmbSearchFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (CmbSearchFor.Items.Count > 0)
                {
                    switch (((ListItem)CmbSearchFor.SelectedItem).Value == null ? string.Empty : ((ListItem)CmbSearchFor.SelectedItem).Value.ToString())
                    {
                        default: SearchFor = "ALL"; break;
                        case "2": SearchFor = "APP"; break;
                        case "3": SearchFor = "MEM"; break;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void CmbCaseWrk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SearchCaseWRK = null;
                if (((ListItem)CmbCaseWrk.SelectedItem).Value != null)
                    if (((ListItem)CmbCaseWrk.SelectedItem).Value.ToString() != "**")
                        SearchCaseWRK = ((ListItem)CmbCaseWrk.SelectedItem).Value.ToString();
            }
            catch (Exception ex)
            {


            }
        }


        private void CmbCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SearchCaseType = ((ListItem)CmbCaseType.SelectedItem).Value.ToString();
                //if (SearchCaseType == '0'.ToString())
                //    SearchCaseType = null;
            }
            catch (Exception ex)
            {


            }
        }




        private void Clear_Search_Controls()
        {
            TxtFName.Clear(); TxtLName.Clear(); TxtAlias.Clear();
            MtxtSsn.Clear(); MtxtPhone.Clear(); MtxtAppNo.Clear();
            TxtHNo.Clear();
            //TxtState.Clear(); TxtCity.Clear(); TxtState.Clear();
            // txtdtFirstName.Clear(); dtBirth.Checked = false;

            //SetRelatedSearchControls(SearcgCategory);
        }

        //private void SetSearchPanelsLocation()
        //{
        //    NamePanel.Size = new System.Drawing.Size(459, 24);
        //    NamePanel.Location = new System.Drawing.Point(168, 5);
        //    SsnPanel.Size = new System.Drawing.Size(459, 24);
        //    SsnPanel.Location = new System.Drawing.Point(168, 5);
        //    AddressPanel.Size = new System.Drawing.Size(459, 24);
        //    AddressPanel.Location = new System.Drawing.Point(168, 5);
        //    PhonePanel.Size = new System.Drawing.Size(459, 24);
        //    PhonePanel.Location = new System.Drawing.Point(168, 5);
        //    AliasPanel.Size = new System.Drawing.Size(459, 24);
        //    AliasPanel.Location = new System.Drawing.Point(168, 5);
        //    ScanAppPanel.Size = new System.Drawing.Size(459, 24);
        //    ScanAppPanel.Location = new System.Drawing.Point(168, 5);
        //    AppNoPanel.Size = new System.Drawing.Size(459, 24);
        //    AppNoPanel.Location = new System.Drawing.Point(168, 5);
        //    pnlDobfirstName.Size = new System.Drawing.Size(459, 24);
        //    pnlDobfirstName.Location = new System.Drawing.Point(168, 5);
        //}

        bool App_Dragged_Sw = false, Can_Set_App = false;


        private void Set_APP_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        public string GetSelectedApplicant()
        {
            string Selected_applicant = null;

            if (string.IsNullOrEmpty(New_Dragged_App_No))
            {
                Selected_applicant = GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString()
                                   + GvwSearch.CurrentRow.Cells["Year"].Value.ToString()
                                   + GvwSearch.CurrentRow.Cells["AppNo"].Value.ToString();
            }
            else
            {
                if (New_Dragged_App_No == "Set_Curr_Hie")
                {
                    Selected_applicant = Hierarchy; // SearchHie; // 
                }
                else
                {
                    if (New_Dragged_App_No.Length == 8)
                        New_Dragged_App_No = (BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ") + New_Dragged_App_No);


                    Selected_applicant = New_Dragged_App_No;
                }
            }

            return Selected_applicant;
        }

        private void CbSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (CbSearch.Checked)
            {
                SearchHie = null;
            }
            else
            {
                Hierarchy = SelAgency + SelDept + SelProg + SelYear;
                SearchHie = Hierarchy;
            }
        }

        private void BtnAddApp_Click(object sender, EventArgs e)
        {
            string Tmp_Add_App_MSG = string.Empty;// "You are about to Add Client into : ";
            string strClientSwitch = "N";
            if (sender == BtnAddApp)
            {
                strClientSwitch = "Y";
            }
            if (sender == Btn_Drag_App)//(GvwSearch.Rows.Count > 0)
            {
                if (GetDup_APP_MEM_Status())
                {
                    //Tmp_Add_App_MSG = "You are about to Drag Client into : ";
                    //MessageBox.Show(Tmp_Add_App_MSG + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Drag_APP_Confirmation, true);
                    Drag_APP_Confirmation();
                }
            }
            else
            {
                string Fname = string.Empty, Lname = string.Empty, SSn = string.Empty;

                //Kranthi Commented

                //if (((ListItem)CmbSearchBy.SelectedItem).Value.ToString() != null)
                //{
                //    switch (((ListItem)CmbSearchBy.SelectedItem).Value.ToString())
                //    {
                //        case "1":
                //            if (!string.IsNullOrEmpty(TxtFName.Text.Trim()))
                //                Fname = TxtFName.Text.Trim();
                //            if (!string.IsNullOrEmpty(TxtLName.Text.Trim()))
                //                Lname = TxtLName.Text.Trim();
                //            break;

                //        case "2":
                //            if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
                //                SSn = MtxtSsn.Text.Trim();
                //            break;
                //        case "8":
                //            if (!string.IsNullOrEmpty(txtdtFirstName.Text.Trim()))
                //                Fname = txtdtFirstName.Text.Trim();
                //            break;
                //    }
                //}

                if (!string.IsNullOrEmpty(TxtFName.Text.Trim()))
                    Fname = TxtFName.Text.Trim();
                if (!string.IsNullOrEmpty(TxtLName.Text.Trim()))
                    Lname = TxtLName.Text.Trim();
                if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
                    SSn = MtxtSsn.Text.Trim();

                if (BaseForm.BaseAgencyControlDetails.DeepSearchSwitch.ToUpper() == "Y")
                {

                    New_Dragged_App_No = string.Empty;
                    string strApp = string.Empty;
                    string strHierchy = string.Empty;
                    string strYear = string.Empty;
                    if (GvwSearch.Rows.Count > 0)
                    {
                        strYear = GvwSearch.CurrentRow.Cells["Year"].Value.ToString();
                        strHierchy = GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString();
                        strApp = GvwSearch.CurrentRow.Cells["AppNo"].Value.ToString();
                        if (strApp.Length > 8)
                        {
                            strApp = strApp.Substring(0, 8);
                        }
                    }
                    MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, AddPriv, string.Empty, Fname, Lname, SSn, strApp, strHierchy, strYear, strClientSwitch, "Add", string.Empty);
                    AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    AddApplicant.StartPosition = FormStartPosition.CenterScreen;
                    AddApplicant.ShowDialog();
                }
                else
                {
                    if (GvwSearch.Rows.Count > 0)
                    {
                        CaseMstEntity caseMSTEntity = new CaseMstEntity();
                        CaseSnpEntity caseSNPEntity = new CaseSnpEntity();
                        caseMSTEntity.ApplAgency = caseSNPEntity.Agency = BaseForm.BaseAgency;
                        caseMSTEntity.ApplDept = caseSNPEntity.Dept = BaseForm.BaseDept;
                        caseMSTEntity.ApplProgram = caseSNPEntity.Program = BaseForm.BaseProg;
                        caseMSTEntity.ApplYr = caseSNPEntity.Year = BaseForm.BaseYear;
                        //caseMSTEntity.Ssn = (!string.IsNullOrEmpty(SSn.Trim()) ? SSn : "");
                        //caseSNPEntity.NameixLast = Lname;
                        //caseSNPEntity.NameixFi = Fname;

                        caseSNPEntity.NameixFi = TxtFName.Text.Trim();
                        caseSNPEntity.NameixLast = TxtLName.Text.Trim();
                        caseSNPEntity.Ssno = caseMSTEntity.Ssn = MtxtSsn.Text.Trim();
                        if (dtBirth.Checked)
                            caseSNPEntity.AltBdate = dtBirth.Value.ToShortDateString();

                        // Kranthi Commented 

                        //if (((ListItem)CmbSearchBy.SelectedItem).Value.ToString() == "8")
                        //{
                        //    caseSNPEntity.NameixFi = txtdtFirstName.Text.Trim();
                        //    if (dtBirth.Checked)
                        //        caseSNPEntity.AltBdate = dtBirth.Value.ToShortDateString();

                        //}

                        Privileges.Program = "Main Menu";
                        //ClientSNPForm clientSNPForm = new ClientSNPForm(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                        //clientSNPForm.FormClosed += new Form.FormClosedEventHandler(On_NewApplicantAdded);
                        //clientSNPForm.ShowDialog();

                        if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                        {
                            Privileges.AddPriv = "true";
                            Case4001Form clientSNPForm = new Case4001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                            clientSNPForm.FormClosed += new FormClosedEventHandler(On_NewApplicantAdded);
                            clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                            clientSNPForm.ShowDialog();
                        }
                        else
                        {
                            Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                            clientSNPForm.FormClosed += new FormClosedEventHandler(On_NewApplicantAdded);
                            clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                            clientSNPForm.ShowDialog();
                        }

                    }
                    else
                    {
                        New_Dragged_App_No = string.Empty;
                        string strApp = string.Empty;
                        string strHierchy = string.Empty;
                        string strYear = string.Empty;
                        if (GvwSearch.Rows.Count > 0)
                        {
                            strYear = GvwSearch.CurrentRow.Cells["Year"].Value.ToString();
                            strHierchy = GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString();
                            strApp = GvwSearch.CurrentRow.Cells["AppNo"].Value.ToString();
                            if (strApp.Length > 8)
                            {
                                strApp = strApp.Substring(0, 8);
                            }
                        }

                        //Kranthi :: 02022023 :: Keep in mind document :: requested to show confirmation message to add new application instead of this form

                        //MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, AddPriv, string.Empty, Fname, Lname, SSn, strApp, strHierchy, strYear, strClientSwitch,"Add",string.Empty);
                        //AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                        //AddApplicant.StartPosition = FormStartPosition.CenterScreen;
                        //AddApplicant.ShowDialog();


                        MessageBox.Show("You are about to add a customer into " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear,
                               Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_New_APP_Confirmation);


                    }
                }
            }
            //string DOB =  null, Fname = null, Lname = null, SSn = null ;
            //New_Dragged_App_No = string.Empty;
            //if (GvwSearch.RowCount > 0)
            //{
            //    if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["DOB"].Value.ToString()))
            //        DOB = GvwSearch.CurrentRow.Cells["DOB"].Value.ToString();
            //    if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Fname"].Value.ToString()))
            //        Fname = GvwSearch.CurrentRow.Cells["Fname"].Value.ToString();
            //    if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Lname"].Value.ToString()))
            //        Lname = GvwSearch.CurrentRow.Cells["Lname"].Value.ToString();
            //    if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["SSN"].Value.ToString()))
            //    {
            //        SSn = GvwSearch.CurrentRow.Cells["SSN"].Value.ToString();
            //        //SSn = SSn.Substring()
            //    }
            //}

            // MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, AddPriv, DOB, Fname, Lname, SSn);
            // AddApplicant.FormClosed += new Form.FormClosedEventHandler(On_Applicant_Dragged);
            // AddApplicant.ShowDialog();

            //App_Dragged_Sw




            //CaseMstEntity caseMSTEntity = new CaseMstEntity();
            //caseMSTEntity.ApplAgency = ((ListItem)CmbAgency.SelectedItem).Value.ToString();
            //caseMSTEntity.ApplDept = ((ListItem)CmbDept.SelectedItem).Value.ToString();
            //caseMSTEntity.ApplProgram = ((ListItem)CmbProg.SelectedItem).Value.ToString();
            //caseMSTEntity.ApplYr = string.Empty;
            //if (!string.IsNullOrEmpty(((ListItem)CmbYear.SelectedItem).Value.ToString()))
            //    caseMSTEntity.ApplYr = ((ListItem)CmbYear.SelectedItem).Value.ToString();

            //Privileges.Program = "Main Menu";
            //ClientSNPForm clientSNPForm = new ClientSNPForm(BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges);
            //clientSNPForm.ShowDialog();
        }


        private void Add_New_APP_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                CaseMstEntity caseMSTEntity = new CaseMstEntity();
                CaseSnpEntity caseSNPEntity = new CaseSnpEntity();
                caseMSTEntity.ApplAgency = BaseForm.BaseAgency;
                caseMSTEntity.ApplDept = BaseForm.BaseDept;
                caseMSTEntity.ApplProgram = BaseForm.BaseProg;
                caseMSTEntity.ApplYr = (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    ");


                caseSNPEntity.NameixFi = TxtFName.Text.Trim();
                caseSNPEntity.NameixLast = TxtLName.Text.Trim();
                caseSNPEntity.Ssno = caseMSTEntity.Ssn = MtxtSsn.Text.Trim();
                if (dtBirth.Checked)
                    caseSNPEntity.AltBdate = dtBirth.Value.ToShortDateString();

                string strRelationDefaultValue = string.Empty;
                List<CommonEntity> Relation = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                if (Relation.Count > 0)
                {
                    CommonEntity commRelation = Relation.Find(u => u.Default == "Y");
                    if (commRelation != null)
                        strRelationDefaultValue = commRelation.Code.ToString();
                }

                caseSNPEntity.MemberCode = strRelationDefaultValue;

                //if (!string.IsNullOrEmpty(PassDOB) ||
                //   !string.IsNullOrEmpty(Fname) ||
                //   !string.IsNullOrEmpty(Lname) ||
                //   !string.IsNullOrEmpty(PassSSN))


                Privileges.Program = "Main Menu";
                //ClientSNPForm clientSNPForm = new ClientSNPForm(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                //clientSNPForm.FormClosed += new Form.FormClosedEventHandler(On_Applicant_Dragged);
                //clientSNPForm.ShowDialog();
                if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                {
                    Privileges.AddPriv = "true";
                    Case4001Form clientSNPForm = new Case4001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                    //clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                    clientSNPForm.ShowDialog();


                    //this.Close();

                    New_Dragged_App_No = BaseForm.BaseApplicationNo;
                    BtnSelApp_Click(BtnSelApp, EventArgs.Empty);


                }
                else
                {
                    Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                    //clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                    clientSNPForm.ShowDialog();
                }
            }
        }

        public string Get_Dragged_App_No()
        {
            string New_App_No = string.Empty;

            if (!(string.IsNullOrEmpty(New_Dragged_App_No.Trim())))
                New_App_No = New_Dragged_App_No;

            // if (BtnType == "View")
            // {
            //if (string.IsNullOrEmpty(New_Dragged_App_No))
            //{
            //    New_App_No = TopGrid.CurrentRow.Cells["Hie"].Value.ToString()
            //                       + TopGrid.CurrentRow.Cells["Year"].Value.ToString()
            //                       + TopGrid.CurrentRow.Cells["AppNo"].Value.ToString();
            //}
            //}

            return New_App_No;
        }


        //private void AdvancedMainMenuSearch_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //}

        private bool GetDup_APP_MEM_Status()
        {
            bool Can_Drag = true;
            string SSn = string.Empty, Error = string.Empty, strReasonflag = "N";
            string ClientID = string.Empty;

            if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString()))
            {
                SSn = GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString();
                ProgramDefinitionEntity programEntity = null;    //_model.HierarchyAndPrograms.GetCaseDepadp(((ListItem)CmbAgency.SelectedItem).Value.ToString(), ((ListItem)CmbDept.SelectedItem).Value.ToString(), ((ListItem)CmbProg.SelectedItem).Value.ToString());    //Kranthi Commented
                if (programEntity != null)
                    strReasonflag = programEntity.SSNReasonFlag.ToString();

                if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["gvClientId"].Value.ToString()))
                    ClientID = GvwSearch.CurrentRow.Cells["gvClientId"].Value.ToString();

                    if (SSn == "000000000" && strReasonflag == "Y")
                {
                    Can_Drag = true;
                }
                else
                {
                    DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(SSn, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, "AddApplicant", GvwSearch.CurrentRow.Cells["Fname"].Value.ToString(), null, GvwSearch.CurrentRow.Cells["DOB"].Value.ToString(), ClientID);
                    DataTable dt2;
                    if (ds1.Tables.Count > 0)
                    {
                        dt2 = ds1.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {

                            if (dt2.Rows[0]["APP_MEM"].ToString() == "A")
                                Error = "Applicant already exists in  " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   cannot Copy/Drag";
                            else
                                Error = "Member already exists in " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   cannot Copy/Drag";

                            MessageBox.Show(Error, "CAP Systems");
                            Can_Drag = false;
                        }
                    }
                }
            }

            return Can_Drag;
        }


        private void Drag_APP_Confirmation() //Drag_APP_Confirmation(object sender, EventArgs e)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            //if (messageBoxWindow.DialogResult == DialogResult.Yes)
            //{
            string DOB = string.Empty, Fname = string.Empty, Lname = string.Empty, SSn = string.Empty;
            New_Dragged_App_No = string.Empty;
            string strApp = string.Empty;
            string strHierchy = string.Empty;
            string strYear = string.Empty;
            if (GvwSearch.RowCount > 0)
            {
                if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["DOB"].Value.ToString()))
                    DOB = GvwSearch.CurrentRow.Cells["DOB"].Value.ToString();
                if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Fname"].Value.ToString()))
                    Fname = GvwSearch.CurrentRow.Cells["Fname"].Value.ToString();
                if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Lname"].Value.ToString()))
                    Lname = GvwSearch.CurrentRow.Cells["Lname"].Value.ToString();
                if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString()))
                {
                    SSn = GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString();
                    //SSn = SSn.Substring()
                }

                strYear = GvwSearch.CurrentRow.Cells["Year"].Value.ToString();
                strHierchy = GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString();
                strApp = GvwSearch.CurrentRow.Cells["AppNo"].Value.ToString();
                if (strApp.Length > 8)
                {
                    strApp = strApp.Substring(0, 8);
                }
            }



            MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, AddPriv, DOB, Fname, Lname, SSn, strApp, strHierchy, strYear, "N", "Drag", string.Empty);
            AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
            AddApplicant.StartPosition = FormStartPosition.CenterScreen;
            AddApplicant.ShowDialog();

            //}
        }



        string New_Dragged_App_No = string.Empty;
        private void On_Applicant_Dragged(object sender, FormClosedEventArgs e)
        {
            setdbleclick = "Y";
            New_Dragged_App_No = string.Empty;
            MainMenuAddApplicantForm form = sender as MainMenuAddApplicantForm;
            if (form.DialogResult == DialogResult.OK)
            {
                New_Dragged_App_No = form.Get_Dragged_App_No();
                BtnSelApp_Click(BtnSelApp, EventArgs.Empty);

                //if (Mode=="View" && !string.IsNullOrEmpty(New_Dragged_App_No.Trim()))
                //{
                //    this.DialogResult= DialogResult.OK;
                //    this.Close();
                //}
            }
        }

        private void On_NewApplicantAdded(object sender, FormClosedEventArgs e)
        {
            New_Dragged_App_No = string.Empty;
            // ClientSNPForm form = sender as ClientSNPForm;
            if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
            {
                Case4001Form form = sender as Case4001Form;
                //this.Close();
                if (form.DialogResult == DialogResult.OK)
                {
                    New_Dragged_App_No = form.GetNew_App_For_Mainmenu();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                Case3001Form form = sender as Case3001Form;
                //this.Close();
                if (form.DialogResult == DialogResult.OK)
                {
                    New_Dragged_App_No = form.GetNew_App_For_Mainmenu();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }



        //public string Get_Dragged_App_No()
        //{
        //    return New_Dragged_App_No;
        //}


        private void Get_ClientIntake_Priv()
        {
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetPrivilizes_byScrCode(BaseForm.UserID, BaseForm.BusinessModuleID, "CASE2001");
            DataTable dt = ds.Tables[0];
            string TmpHie = null, Current_HieTo_Compare = Agency + Dept + Program;
            //bool All_Hie_Exists = false;
            //char All_Hie_Add_Priv = 'N';
            if (dt.Rows.Count > 0)
            {
                DataView dv=new DataView(dt);
                dv.RowFilter = "Hie_Used_Flag='N'";
                dt = dv.ToTable();

                AddPriv = 'U';

                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 1: Current_HieTo_Compare = Agency + Dept + "**"; break;
                        case 2: Current_HieTo_Compare = Agency + "****"; break;
                        case 3: Current_HieTo_Compare = "******"; break;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        TmpHie = dr["EFR_HIERARCHY"].ToString();

                        if (TmpHie.Substring(0, 2) == Current_HieTo_Compare.Substring(0, 2) &&
                            TmpHie.Substring(2, 2) == Current_HieTo_Compare.Substring(2, 2) &&
                            TmpHie.Substring(4, 2) == Current_HieTo_Compare.Substring(4, 2))
                        //&& dr["Hie_Used_Flag"].ToString() == "N")
                        { AddPriv = char.Parse(dr["EFR_ADD_PRIV"].ToString()); break; }
                    }
                    if (AddPriv == 'Y' || AddPriv == 'N')
                        break;
                }

                //foreach (DataRow dr in dt.Rows)
                //{
                //    TmpHie = dr["EFR_HIERARCHY"].ToString();

                //    if (TmpHie == "******")
                //    {
                //        All_Hie_Exists = true;
                //        All_Hie_Add_Priv = char.Parse(dr["EFR_ADD_PRIV"].ToString());
                //    }

                //    if (TmpHie.Substring(0, 2) == Agency && TmpHie.Substring(2, 2) == Dept && TmpHie.Substring(4, 2) == Program)
                //    { AddPriv = char.Parse(dr["EFR_ADD_PRIV"].ToString()); break; }
                //}

                //if (All_Hie_Exists && AddPriv == 'U')
                //    AddPriv = All_Hie_Add_Priv;

                // Set_Add_App_Visibility();  //Kranthi 03/01/2023 :: NCCAA.docx :: we are not using hierarchy for advanced search so we are commenting the line
            }


        }

        private void GvwSearch_SelectionChanged(object sender, EventArgs e)
        {
            if (GvwSearch.Rows.Count > 0)
            {
                if (BaseForm.BaseAgencyControlDetails.SsnDobMMenu == "D")
                {
                    string tmpssn1 = GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString();
                    if (tmpssn1.Length > 8)
                        Lbl_DOB.Text = "SS# - " + LookupDataAccess.GetCardNo(tmpssn1, "1", string.Empty, string.Empty);
                }
                else
                {
                    Lbl_DOB.Text = "DOB - " + GvwSearch.CurrentRow.Cells["DOB"].Value.ToString();
                }
                Lbl_Worker.Text = "Case Worker - " + Get_CaseWorket_DESC(GvwSearch.CurrentRow.Cells["Case_Worker"].Value.ToString());
                Lbl_program.Text = "Program - " + Get_Program_Desc(GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString());
                if (BaseForm.BusinessModuleID == "05")
                {
                    lblAppDate2.Visible = true;
                    lblAppDate2.Text = "Intake Date - " + GvwSearch.CurrentRow.Cells["gvtIntakeDate"].Value.ToString();
                }

                

                if (GvwSearch.CurrentRow.Cells["Other_Hie_Access"].Value.ToString() == "N")
                {
                    BtnSelApp.Visible = false; //Btn_Drag_App.Visible = false;
                    Lbl_No_Access.Visible = true;
                    if (BaseForm.BusinessModuleID == "05")
                    {
                        Lbl_No_Access.Visible = false;
                    }
                    //BtnAddApp.Text = "Add a Brand New Application";
                }
                else
                {
                    string[] _HIEarry = GvwSearch.CurrentRow.Cells["gvtHierachy"].Value.ToString().Split('-');
                    ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(_HIEarry[0].ToString(), _HIEarry[1].ToString(), _HIEarry[2].ToString());
                    if (programEntity.DepIntakeProg == "Y")
                    {
                        Lbl_No_Access.Visible = false; BtnSelApp.Visible = true; //Btn_Drag_App.Visible = true;
                    }
                    else if (programEntity.DepIntakeProg == "N")
                    {
                        BtnSelApp.Visible = false; //Btn_Drag_App.Visible = false;
                        Lbl_No_Access.Visible = true;
                    }

                    //if (BtnAddApp.Visible)
                    //  Btn_Drag_App.Visible = true;
                    //BtnAddApp.Text = "Copy/Drag this Application";
                }

                string Sel_Agency = GvwSearch.CurrentRow.Cells["gvtHierachy"].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(Sel_Agency.Trim()))
                {
                    if (BaseForm.BaseAgency == Sel_Agency.Substring(0, 2))
                        BtnSelApp.Visible = true;
                    else
                        BtnSelApp.Visible = false;
                }
            }
            else
            {
                Lbl_DOB.Text=string.Empty;lblAppDate2.Text=string.Empty;Lbl_Worker.Text=string.Empty;Lbl_program.Text=string.Empty;
            }

            if (Lbl_No_Access.Visible == true)
            { BtnSelApp.Visible = false; /*Btn_Drag_App.Visible = false;*/ }
            else
            {
                BtnSelApp.Visible = true;/* Btn_Drag_App.Visible = true;*/
            }



        }

        private void Controls_Common_GotFocus(object sender, EventArgs e)
        {
            if (sender == MtxtAppNo)
                MtxtAppNo.SelectionLength = MtxtAppNo.Text.Length;
            else
                if (sender == MtxtPhone)
                MtxtPhone.SelectionLength = MtxtPhone.Text.Length;
            else
                    if (sender == MtxtSsn)
                MtxtSsn.SelectionLength = MtxtSsn.Text.Length;
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            this.GvwSearch.SelectionChanged -= new System.EventHandler(this.GvwSearch_SelectionChanged);
            Lbl_DOB.Text = ""; Lbl_Worker.Text = ""; Lbl_program.Text = ""; lblAppDate2.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible =
            BtnSelApp.Visible = Btn_Drag_App.Visible = false;

            MtxtAppNo.Text = ""; TxtScanApp.Text = "";
            TxtFName.Text = ""; TxtLName.Text = ""; TxtAlias.Text = ""; dtBirth.Checked = false; dtBirth.Value = DateTime.Now; MtxtSsn.Text = "";
            TxtHNo.Text = ""; TxtStreet.Text = ""; TxtCity.Text = ""; MtxtPhone.Text = ""; MtxtHomeMobNumber.Text = "";
            CmbSearchFor.SelectedIndex = 0; CmbCaseType.SelectedIndex = 0; CmbCaseWrk.SelectedIndex = 0; CbSearch.Checked = false;

            Lbl_No_Access.Visible = false;
            MtxtAppNo.Focus();
            MtxtAppNo.TabIndex = 1;
            dtBirth.Value = DateTime.Now;
            GvwSearch.Rows.Clear();

            this.GvwSearch.SelectionChanged += new System.EventHandler(this.GvwSearch_SelectionChanged);
        }

        private void MtxtSsn_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
            {
                string strssn = MtxtSsn.Text.Replace(" ", string.Empty);
                string Tmp_SSn = MtxtSsn.Text + "000000000".Substring(0, (9 - MtxtSsn.Text.Length));
                MtxtSsn.Text = Tmp_SSn.Replace(' ', '0');
            }
        }

        private void MtxtPhone_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MtxtPhone.Text.Trim()))
            {
                string Tmp_SSn = MtxtPhone.Text + "0000000000".Substring(0, (10 - MtxtPhone.Text.Length));
                MtxtPhone.Text = Tmp_SSn.Replace(' ', '0');
            }
        }



        List<ListItem> Worker_List { get; set; }
        private void Get_CaseWorker_List()
        {

            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
            ////DataSet cwDataSet = Captain.DatabaseLayer.CaseMst.GetCaseWorker(BaseForm.BaseHierarchyCwFormat, "**", "**", "**");
            //DataSet cwDataSet = Captain.DatabaseLayer.CaseMst.GetAllCaseWorkers(string.Empty);
            //if (cwDataSet.Tables.Count > 0)
            //{
            //    DataTable dt = cwDataSet.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //            Worker_List.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim()));
            //    }
            //}
        }

        private string Get_CaseWorket_DESC(string Worker_Code)
        {
            string Worker_Desc = "";
            foreach (ListItem List in Worker_List)
            {
                if (List.Value.ToString().Trim() == Worker_Code.Trim())
                {
                    Worker_Desc = List.Text.ToString().Trim();
                    break;
                }
            }

            return Worker_Desc;
        }




        private void AdvancedMainMenuSearch_KeyDown(object objSender, KeyEventArgs e)
        {
            //if (TxtLName.Text != string.Empty)
            //{
            //    if (e.Alt && e.KeyCode == Keys.S)
            //    {
            //        BtnSearcRecs_Click(BtnSearcRecs, new EventArgs());
            //    }
            //}
        }

        private void MtxtAppNo_Leave(object sender, EventArgs e)
        {
            MtxtAppNo_LostFocus(MtxtAppNo, EventArgs.Empty);
        }

        // bool App_Dragged_Sw = false, Can_Set_App = false;
        private void BtnSelApp_Click(object sender, EventArgs e)
        {
            Can_Set_App = true;
            try //changed sudheer 12/13/2016
            {
                // if (sender == Btn_Curr_Hie)
                //    New_Dragged_App_No = "Set_Curr_Hie";
                // else
                // {
                if (GvwSearch.Rows.Count > 0 && string.IsNullOrEmpty(New_Dragged_App_No.Trim()))
                {
                    if (GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString() + GvwSearch.CurrentRow.Cells["Year"].Value.ToString() != (BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ")))
                    {
                        MessageBox.Show("Selected client is not in the currently selected desktop program '" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + " " + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ") + "' \n Would you like to select the desktop program : "
                                        + GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString() + " " + GvwSearch.CurrentRow.Cells["Year"].Value.ToString(),
                                        Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Set_APP_Confirmation);
                        Can_Set_App = false;
                    }
                }
                //}

                if (Can_Set_App)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is busy, Try after sometime");
            }
        }

        string Mode = string.Empty;
        string setdbleclick = "Y";
        private void GvwSearch_DoubleClick(object sender, EventArgs e)
        {
            if(GvwSearch.Rows.Count>0)
            {
                if (setdbleclick == "Y")
                {
                    string DOB = string.Empty, Fname = string.Empty, Lname = string.Empty, SSn = string.Empty;
                    New_Dragged_App_No = string.Empty;
                    string strApp = string.Empty;
                    string strHierchy = string.Empty;
                    string strYear = string.Empty;
                    if (GvwSearch.RowCount > 0)
                    {
                        if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["DOB"].Value.ToString()))
                            DOB = GvwSearch.CurrentRow.Cells["DOB"].Value.ToString();
                        if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Fname"].Value.ToString()))
                            Fname = GvwSearch.CurrentRow.Cells["Fname"].Value.ToString();
                        if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Lname"].Value.ToString()))
                            Lname = GvwSearch.CurrentRow.Cells["Lname"].Value.ToString();
                        if (!string.IsNullOrEmpty(GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString()))
                        {
                            SSn = GvwSearch.CurrentRow.Cells["Org_SSN"].Value.ToString();
                            //SSn = SSn.Substring()
                        }

                        strYear = GvwSearch.CurrentRow.Cells["Year"].Value.ToString();
                        strHierchy = GvwSearch.CurrentRow.Cells["RecHie"].Value.ToString();
                        strApp = GvwSearch.CurrentRow.Cells["AppNo"].Value.ToString();
                        if (strApp.Length > 8)
                        {
                            strApp = strApp.Substring(0, 8);
                        }
                    }

                    Mode = "View";

                    setdbleclick = "N";

                    string _otherHIEAccess = GvwSearch.CurrentRow.Cells["Other_Hie_Access"].Value.ToString();
                    MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, AddPriv, DOB, Fname, Lname, SSn, strApp, strHierchy, strYear, "N", "View");
                    AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    AddApplicant.StartPosition = FormStartPosition.CenterScreen;
                    AddApplicant.ShowDialog();

                }
            }
            
        }
        #region Vikash added on 08/01/2023 as a part of "Advanced Search Push Button 7/28/2023 Point" in "July 2023 Enhancements and Issues" document
        public string BusinessModuleID { get; set; }
        public string BaseAgency { get; set; }
        public string BaseDept { get; set; }
        public string BaseProg { get; set; }
        public string BaseYear { get; set; }

        public ApplicationNameControl applicationNameControl;

       // public string strcurrentModuleID = "";
        private void AdvancedMainMenuSearch_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tl_Help")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps("ADVMMSEARCH", 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
            }
            if (e.Tool.Name == "tlHierarchy")
            {
                strcurrentHIE = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg; strYear = BaseForm.BaseYear; //BaseAgency + "-" + BaseDept + "-" + BaseProg; strYear = BaseYear;
                HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, strcurrentHIE, "Master", string.Empty, "A", "I", BaseForm.UserID, "Master");
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnDefaultHierarchieFormClosed);
                hierarchieSelectionForm.ShowDialog();

            }
        }

        string strcurrentHIE = ""; string strYear = ""; string frmHiEScren = "N";
        
        private void OnDefaultHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            string strPublicCode = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchiesWithYear;
                string StrYear = form.SelectedHierarchyYear();
                if (selectedHierarchies.Count > 0)
                {
                    string hierarchy = string.Empty;
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        strPublicCode = row.Code;
                        hierarchy += row.Agency + row.Dept + row.Prog;
                    }

                    BaseForm.BaseAgency = selectedHierarchies[0].Agency;
                    BaseForm.BaseDept = selectedHierarchies[0].Dept;
                    BaseForm.BaseProg = selectedHierarchies[0].Prog;

                    if (!string.IsNullOrEmpty(StrYear.Trim()))
                    {
                        BaseForm.BaseYear = StrYear;
                    }
                    else
                    {
                        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                        if (programEntity != null)
                        {
                            BaseForm.BaseYear = programEntity.DepYear.Trim() == string.Empty ? "    " : programEntity.DepYear;
                        }
                    }

                    BaseForm.BaseAdminAgency = selectedHierarchies[0].Agency;

                    MainMenuControl MainMenu_Control = BaseForm.GetBaseUserControlMainMenu() as MainMenuControl;
                    if (MainMenu_Control != null)
                    {
                        applicationNameControl.Btn_First.Visible = applicationNameControl.BtnP10.Visible = applicationNameControl.BtnPrev.Visible =
                               applicationNameControl.BtnNxt.Visible = applicationNameControl.BtnN10.Visible = applicationNameControl.BtnLast.Visible = true;

                        MainMenu_Control.Set_DefHie_as_BaseHie(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

                        ProgramDefinitionEntity SprogramEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                        if (SprogramEntity != null)
                        {
                            if (BaseForm.BaseYear.Trim() != SprogramEntity.DepYear.Trim())
                            {
                                applicationNameControl.lblMsg.Text = "** Current Program Year is " + SprogramEntity.DepYear.Trim() + "... you are not in the Current Program Year **";
                                applicationNameControl.lblMsg.Visible = true;
                            }
                            else
                            {
                                applicationNameControl.lblMsg.Visible = false;
                            }
                        }

                    }

                    
                    if (strcurrentHIE + strYear != BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + BaseForm.BaseYear)
                    {
                        BaseForm.ContentTabs.TabPages.Clear();
                        BaseForm.strcurrentModuleID = BaseForm.BusinessModuleID;
                        /*** Refresh the Menu Tree ***/
                        BaseForm.frmHiEScren = "Y";
                        BaseForm.BuildMenuTree();

                        //**applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;

                        //MasterPage.btncloseAll_Click(sender, e);
                        SelAgency = BaseForm.BaseAgency; SelDept = BaseForm.BaseDept; SelProg = BaseForm.BaseProg; SelYear = BaseForm.BaseYear;
                        SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;

                        GvwSearch.Rows.Clear();
                        Lbl_DOB.Text = string.Empty; lblAppDate2.Text = string.Empty; Lbl_Worker.Text = string.Empty; Lbl_program.Text = string.Empty;
                        BtnSearcRecs_Click(sender, e);
                    }
                    else {
                        if (strcurrentHIE + strYear == BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + BaseForm.BaseYear) {
                            if (BaseForm.UserProfile.Agency + "-" + BaseForm.UserProfile.Dept + "-" + BaseForm.UserProfile.Prog != strcurrentHIE) {
                                BaseForm.ContentTabs.TabPages.Clear();
                                BaseForm.strcurrentModuleID = BaseForm.BusinessModuleID;
                                /*** Refresh the Menu Tree ***/
                                BaseForm.frmHiEScren = "Y";
                                BaseForm.BuildMenuTree();

                                //**applicationNameControl.Controls[0].Controls["panelNavButtons"].Visible = true;

                                //MasterPage.btncloseAll_Click(sender, e);
                                SelAgency = BaseForm.BaseAgency; SelDept = BaseForm.BaseDept; SelProg = BaseForm.BaseProg; SelYear = BaseForm.BaseYear;
                                SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;

                                GvwSearch.Rows.Clear();
                                Lbl_DOB.Text = string.Empty; lblAppDate2.Text = string.Empty; Lbl_Worker.Text = string.Empty; Lbl_program.Text = string.Empty;
                                BtnSearcRecs_Click(sender, e);

                            }
                        }



                    }

                    ProgramDefinitionEntity SrprogramEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                    if (SrprogramEntity != null)
                    {
                        if (BaseForm.BaseYear.Trim() != SrprogramEntity.DepYear.Trim())
                        {
                            applicationNameControl.lblMsg.Text = "** Current Program Year is " + SrprogramEntity.DepYear.Trim() + "... you are not in the Current Program Year **";
                            applicationNameControl.lblMsg.Visible = true;
                        }
                        else
                        {
                            applicationNameControl.lblMsg.Visible = false;
                        }
                    }
                }
            }
        }
        #endregion

        
        private void TxtLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if (TxtLName.Text != string.Empty)

            //if (e.KeyChar == 262144 )
            //{
            // MessageBox.Show(e.KeyChar.GetHashCode().ToString());//--83\u0012 18//5439571


            //if (e.KeyChar == (Char)Wisej.Web.Keys.Alt || (e.KeyChar == '\t' || e.KeyChar == (char)13))
            //{
            //    BtnSearcRecs_Click(BtnSearcRecs, new EventArgs());
            //}

            // }
        }

        private void Lbl_No_Access_Click(object sender, EventArgs e)
        {

        }


        private string Get_Program_Desc(string Prog_Code)
        {
            string Prog_Desc = "";
            if (BaseForm.BaseCaseHierachyListEntity.Count > 0)
            {
                HierarchyEntity hierchyName = BaseForm.BaseCaseHierachyListEntity.Find(u => u.Agency.Trim() + u.Dept.Trim() + u.Prog.Trim() == Prog_Code);
                if (hierchyName != null)
                {
                    Prog_Desc = hierchyName.HirarchyName;
                }
            }
            return Prog_Desc;
        }

    }
}