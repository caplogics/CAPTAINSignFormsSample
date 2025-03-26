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
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class AdvancedAgencypartnerSearch : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public AdvancedAgencypartnerSearch(BaseForm baseForm, bool change_hierarchy, bool Add_Clients_Btn_vis)
        {
            InitializeComponent();
            BaseForm = baseForm;
           
            Change_Hierarchy = change_hierarchy;
            _model = new CaptainModel();
            Add_Clients_Btn_Vis = Add_Clients_Btn_vis;

            propAgencyControlList = _model.ZipCodeAndAgency.GetAgencyControlFileALL();
            
            Called_From = "Mainmenu";
          //  MtxtCode.Validated = TextBoxValidation.IntegerMaskValidator;

            //if (BaseForm.BaseAgencyControlDetails.ShowIntakeSwitch.ToUpper() != "Y")
            //{               
            Cb_MST_Prog.Visible = false;
            Cb_MST_Prog.Checked = false;
            //}


            Agency = DefAgy = Gbl_Agy = BaseForm.BaseAgency; Dept = DefDept = Gbl_Dept = BaseForm.BaseDept;
            Program = DefProg = Gbl_Prog = BaseForm.BaseProg; ProgramYear = DefYear = Gbl_Year = "    "; DefHieExist = true;



        }

        List<AgencyControlEntity> propAgencyControlList { get; set; }


        private void AdvancedAgencypartnerSearch_Load(object sender, EventArgs e)
        {
            Get_CASEDEP_List();
            FillAgencyCombo();
           

            if (!string.IsNullOrEmpty(BaseForm.BaseAgency) &&
                !string.IsNullOrEmpty(BaseForm.BaseDept) &&
                !string.IsNullOrEmpty(BaseForm.BaseProg))
            {
                Agency = BaseForm.BaseAgency;
                Dept = BaseForm.BaseDept;
                Program = BaseForm.BaseProg;               
            }
                       

        }

        public bool Change_Hierarchy { get; set; }

        public bool Add_Clients_Btn_Vis { get; set; }

        PrivilegeEntity Privileges = new PrivilegeEntity();


        bool Loading_Complete = false;

        string SearcgCategory = "7";  // Applicant No Default
        string SearchFor = "1";      // All as Sefault
        string SearchCaseType = "**";
      
        string PrvPanelCode = null;

        string SelAgency = null;
        string SelDept = null;
        string SelProg = null;
        string SelYear = null;

        string Hierarchy = null;
        string SearchHie = null;
       

        string Gbl_Agy = null;
        string Gbl_Dept = null;
        string Gbl_Prog = null;
        string Gbl_Year = null;
        string Gbl_AppNo = null;
      

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
            if (BaseForm.ContentTabs.TabPages[0].Controls[0] is AgencyPartnerControl)
            {
                Gbl_Agy = BaseForm.BaseAgency;
                Gbl_Dept = BaseForm.BaseDept;
                Gbl_Prog = BaseForm.BaseProg;
                Gbl_Year = !string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ";
                Gbl_AppNo = BaseForm.BaseApplicationNo;

                Agency = DefAgy = Gbl_Agy; Dept = DefDept = Gbl_Dept; Program = DefProg = Gbl_Prog; ProgramYear = DefYear = Gbl_Year; DefHieExist = true;
            }
        }


        private void FillAgencyCombo()
        {
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("1", " ", " ");
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once

            CmbAgency.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            int TmpRows = 0;
            int AgyIndex = 0;
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        listItem.Add(new ListItem(dr["Agy"] + " - " + dr["Name"], dr["Agy"]));
                        if (DefAgy == dr["Agy"].ToString())
                            AgyIndex = TmpRows;

                        TmpRows++;
                    }
                    if (TmpRows > 0)
                    {
                        CmbAgency.Items.AddRange(listItem.ToArray());
                        if (DefHieExist)
                            CmbAgency.SelectedIndex = AgyIndex;
                        else
                        {
                            if (CmbAgency.Items.Count == 1)
                                CmbAgency.SelectedIndex = 0;
                        }
                    }
                }
                //DefAgy = DefDept = DefProg = DefYear = null;
            }
            catch (Exception ex) { }
        }

        private void FillDeptCombo(string Agy)
        {
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("2", SelAgency, " ");
            CmbDept.Items.Clear();
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "2", SelAgency, " ", " "); // Verify it Once
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    CmbDept.Items.Clear();
                    List<ListItem> listItem = new List<ListItem>();
                    int TmpRows = 0;
                    int DeptIndex = 0;
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            listItem.Add(new ListItem(dr["Dept"] + " - " + dr["Name"], dr["Dept"]));
                            if (DefDept == dr["Dept"].ToString())
                                DeptIndex = TmpRows;

                            TmpRows++;
                        }
                        if (TmpRows > 0)
                        {
                            CmbDept.Items.AddRange(listItem.ToArray());
                            CmbDept.Enabled = true;
                            if (DefHieExist)
                                CmbDept.SelectedIndex = DeptIndex;
                            else
                            {
                                if (CmbDept.Items.Count == 1)
                                    CmbDept.SelectedIndex = 0;
                            }

                        }
                    }
                    catch (Exception ex) { }
                }
                else
                    MessageBox.Show("Departments and Programs are not defined", "CAPTAIN");
            }
        }

        private void FillProgCombo()
        {
            CmbProg.Items.Clear();

            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("3", SelAgency, SelDept);
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", SelAgency, SelDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                    List<ListItem> listItem = new List<ListItem>();
                    int TmpRows = 0;
                    int ProgIndex = 0;
                    try
                    {
                        bool IS_DepIntake_Hie = false;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Cb_MST_Prog.Checked)
                            {
                                IS_DepIntake_Hie = false;
                                foreach (ProgramDefinitionEntity Ent in CASEDEP_List)
                                {
                                    if (Ent.DepIntakeProg == "Y" && SelAgency + SelDept + dr["Prog"].ToString().Trim() == Ent.Agency + Ent.Dept + Ent.Prog)
                                    {
                                        IS_DepIntake_Hie = true;
                                        break;
                                    }
                                }
                            }
                            //if ((int.Parse(dr["Prog_Mst_Cnt"].ToString()) > 0 && Cb_MST_Prog.Checked) || !Cb_MST_Prog.Checked)
                            if ((IS_DepIntake_Hie && Cb_MST_Prog.Checked) || !Cb_MST_Prog.Checked)
                            {
                                listItem.Add(new ListItem(dr["Prog"] + " - " + dr["Name"], dr["Prog"]));
                                if (DefProg == dr["Prog"].ToString())
                                    ProgIndex = TmpRows;

                                TmpRows++;
                            }
                        }

                        if (TmpRows > 0)
                        {
                            CmbProg.Items.AddRange(listItem.ToArray());
                            CmbProg.Enabled = true;
                            if (DefHieExist)
                                CmbProg.SelectedIndex = ProgIndex;
                            else
                            {
                                if (CmbProg.Items.Count == 1)
                                    CmbProg.SelectedIndex = 0;
                            }

                        }
                    }
                    catch (Exception ex) { }
                }
                else
                    MessageBox.Show("Programs Are Not Defined", "CAPTAIN");
            }
        }


        DataTable User_Hie_Acc_List = new DataTable();
        string Dep_Program_Year = "    ";
        private void FillYearCombo()
        {
            CmbYear.Visible = BtnSelAgency.Visible = false;
            DefHieExist = false;
            //BtnAddApp.Text = "Add a Brand New Application";
            Dep_Program_Year = "    ";
            if (!string.IsNullOrEmpty(DefYear))
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
            CmbSearchBy.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Address", "3"));
            listItem.Add(new ListItem("Code", "7"));
            listItem.Add(new ListItem("Name", "1"));
            listItem.Add(new ListItem("Telephone", "4"));

            CmbSearchBy.Items.AddRange(listItem.ToArray());



            SetComboBoxValue(CmbSearchBy, SearcgCategory);

            //CmbSearchBy.SelectedIndex = 0;
            //CmbSearchFor.SelectedIndex = 0;
            SetSearchPanelsLocation();
            SetRelatedSearchControls(((ListItem)CmbSearchBy.SelectedItem).Value.ToString());
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


        private void CmbAgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeAllControls();
            CmbDept.Enabled = CmbProg.Enabled = BtnSelAgency.Visible = false;
            //BtnAddApp.Text = "Add a Brand New Application";
            SelDept = SelProg = null;

            SearcgCategory = "7";  // Applicant # Default
            SearchFor = "1";      // All as Default
            SearchCaseType = "**";

            SelAgency = ((ListItem)CmbAgency.SelectedItem).Value.ToString();
            if (SelAgency != DefAgy && DefHieExist == true)
                DefHieExist = false;

            Get_NameFormat_For_Agencies(SelAgency);
            FillDeptCombo(SelAgency);
            // Ram added the following 2 functions
            Fill_SearchControl_Default();
            FillAllDropdowns();


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

        private void CmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeAllControls();
            CmbProg.Enabled = BtnSelAgency.Visible = false;
            //BtnAddApp.Text = "Add a Brand New Application";
            SelProg = null;
            SelDept = ((ListItem)CmbDept.SelectedItem).Value.ToString();

            if (SelDept != DefDept && DefHieExist == true)
                DefHieExist = false;
            if (BaseForm.BaseAgencyControlDetails.ShowIntakeSwitch.ToUpper() != "Y")
            {
                Cb_MST_Prog.Checked = false;
            }
            else
            {
                Cb_MST_Prog.Checked = true;
            }
            FillProgCombo();
        }

        private void CmbProg_SelectedIndexChanged(object sender, EventArgs e)
        {


            BtnSelAgency.Visible = false;
            //BtnAddApp.Text = "Add a Brand New Application";
            GvwSearch.Rows.Clear();
            Clear_Search_Controls();
            //SetRelatedSearchControls(SearcgCategory);

            SelProg = ((ListItem)CmbProg.SelectedItem).Value.ToString();

            if (SelProg != DefProg && DefHieExist == true)
                DefHieExist = false;


            if (!(String.IsNullOrEmpty(SelDept)) && !(String.IsNullOrEmpty(SelProg)))
            {
                //if (Check_PROG_Existance_In_CASEDEP())
                //{

                //if (!(CmbYear.Visible))
                SelYear = "    ";

                SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;
                Agency = SelAgency;  // Yeswanth
                Dept = SelDept;
                Program = SelProg;
                //TreeViewControllerParameter treeViewControllerParameter = new TreeViewControllerParameter()
                //{
                //    TreeType = TreeType.CaseManagement,
                //    TreeView = BaseForm.NavigationTreeView,
                //    Hierarchy = Agency + Dept + Program
                //};
                //BaseForm.GetTreeView(treeViewControllerParameter);

                ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(SelAgency, SelDept, SelProg);
                if (programEntity != null)
                {
                    //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "CCA")
                    //{
                    //    BtnDefHier.Visible = false; ;
                    //    //Cb_MST_Prog.Visible = false;

                    //}
                    //else
                    //{
                    //    BtnDefHier.Visible = true;
                    //}

                    SeacrhPanel.Visible = Btn_Curr_Hie.Visible = true;

                    if (BaseForm.BaseAgencyControlDetails.SitesData == "1")
                        CbSearch.Visible = false;
                    else
                        CbSearch.Visible = true;

                    DepYear = null;
                    CmbYear.Items.Clear();
                    CmbYear.Visible = false;
                    FillYearCombo();

                    //Get_ClientIntake_Priv();
                }
                else
                {
                    MessageBox.Show("Control Record For Agency : " + SelAgency + " and Department :" + SelDept + " and Program : " + SelProg + " Has Not Been Created \n" +
                                    "Please Contact Your System Administrator. \n", "CAPTAIN");
                    SeacrhPanel.Visible = BtnDefHier.Visible = Btn_Curr_Hie.Visible = false;

                }


            }
        }

        private void Fill_SearchControl_Default()
        {
            if (propAgencyControlList.Count > 0)
            {
                AgencyControlEntity Agencydata = propAgencyControlList.Find(u => u.AgencyCode.ToString().Trim() == ((ListItem)CmbAgency.SelectedItem).Value.ToString());
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

        }



        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            GvwSearch.Rows.Clear();
            Clear_Search_Controls();
            

            SelYear = "    ";
            if (!(string.IsNullOrEmpty(((ListItem)CmbYear.SelectedItem).Text.ToString())))
                SelYear = ((ListItem)CmbYear.SelectedItem).Text.ToString();

            //if (!(CmbYear.Visible))
            //    SelYear = "    ";

            SearchHie = Hierarchy = SelAgency + SelDept + SelProg + SelYear;

            BtnSelAgency.Visible = false;
           
        }


        private void InitializeAllControls()
        {
            Loading_Complete = false;
            CmbProg.Items.Clear();
            CmbYear.Items.Clear();
            GvwSearch.Rows.Clear();

            CmbYear.Visible = SeacrhPanel.Visible = BtnDefHier.Visible = Btn_Curr_Hie.Visible = false;

            Clear_Search_Controls();

        }



        private void BtnDefHier_Click(object sender, EventArgs e)
        {
            //if (!(CmbYear.Visible))
            //    SelYear = null;
            //bool UpdateRes = Captain.DatabaseLayer.MainMenu.UpdateUserDefHierarchy(BaseForm.UserID, SelAgency, SelDept, SelProg, SelYear);
            //Agency = SelAgency;  // Yeswanth
            //Dept = SelDept;
            //Program = SelProg;
            //if (string.IsNullOrEmpty(SelYear))
            //    SelYear = "    ";
            //ProgramYear = SelYear;
            //if (UpdateRes)
            //{
            //    //MessageBox.Show("Default Hierarchy is Updated Successfully", "CAPTAIN", MessageBoxButtons.OK); // Lisa Asked to Comment on 01072013
            //    BaseForm.BaseTopApplSelect = "N";
            //    //if (SelAgency + SelDept + SelProg + SelYear != BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear)   // Yeswanth Base Form Hierarchy will Reset Only when  
            //    //{
            //    AgencyPartnerControl Agencypartner_Control = BaseForm.GetBaseUserControl() as AgencyPartnerControl;
            //    if (Agencypartner_Control != null)
            //    {
            //        Agencypartner_Control.Set_DefHie_as_BaseHie(SelAgency, SelDept, SelProg, SelYear);
            //    }
            //    Get_ClientIntake_Priv();
            //    ////}
            //}
            //else
            //    MessageBox.Show("Default Hierarchy is Not Updated", "CAPTAIN", MessageBoxButtons.OK);
        }

        private bool Check_Search_Criteria()
        {
            bool Can_Search = false;
            switch (SearcgCategory)
            {
                case "NAM":
                    if ((!string.IsNullOrEmpty(TxtAgencyName.Text.Trim())))
                        Can_Search = true; break;
                    ;
                case "ADD":
                    if ((!string.IsNullOrEmpty(TxtStreet.Text.Trim())) ||
                        (!string.IsNullOrEmpty(TxtCity.Text.Trim())) || (!string.IsNullOrEmpty(TxtState.Text.Trim())))
                        Can_Search = true; break;
                case "PHN":
                    if (!string.IsNullOrEmpty(MtxtPhone.Text.Trim()))
                        Can_Search = true; break;
                case "APP":
                    if (!string.IsNullOrEmpty(MtxtCode.Text.Trim()))
                        Can_Search = true; break;

            }


            return Can_Search;
        }

        private void BtnSearcRecs_Click(object sender, EventArgs e)
        {
            try
            {


                if (Check_Search_Criteria())
                {
                    BtnSelAgency.Visible = false;
                    BtnSearcRecs.Enabled = false;

                    Loading_Complete = false;
                    if (SearchCaseType.Equals("**"))
                        SearchCaseType = string.Empty;
                    //SetControlsVisibility('N');
                    GvwSearch.Rows.Clear();


                    string strFirstName = string.Empty;
                    if (TxtAgencyName.Text.Trim() != string.Empty)
                        strFirstName = TxtAgencyName.Text;



                    AGCYPARTEntity Search_Entity = new AGCYPARTEntity(true);

                    if (MtxtCode.Text != string.Empty)
                        Search_Entity.Code = MtxtCode.Text;
                    if (TxtState.Text != string.Empty)
                        Search_Entity.State = TxtState.Text;
                    if (TxtStreet.Text != string.Empty)
                        Search_Entity.Street = TxtStreet.Text;
                    if (TxtCity.Text != string.Empty)
                        Search_Entity.City = TxtCity.Text;
                    if (TxtAgencyName.Text != string.Empty)
                        Search_Entity.Name = TxtAgencyName.Text;
                    if (MtxtPhone.Text != string.Empty)
                        Search_Entity.Phone = MtxtPhone.Text;

                    List<AGCYPARTEntity> Agcypartentity = _model.SPAdminData.Browse_AgencyPartner(Search_Entity, "Browse");

                    if (Agcypartentity.Count > 0)
                    {
                        int TmpRows = 0;


                        try
                        {

                            foreach (AGCYPARTEntity dr in Agcypartentity)
                            {
                                int rowIndex = 0;
                                rowIndex = GvwSearch.Rows.Add(dr.Code, dr.Name, dr.Street, dr.City, dr.State);
                                GvwSearch.Rows[rowIndex].Tag = dr;

                            }
                            if (GvwSearch.Rows.Count > 0)
                            {
                                Loading_Complete = true;
                                BtnSelAgency.Visible = true;
                                GvwSearch_SelectionChanged(GvwSearch, EventArgs.Empty);

                            }
                        }
                        catch (Exception ex) { }
                        BtnSearcRecs.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No Record(s) With Selected Search Criteria", "CAPTAIN", MessageBoxButtons.OK);
                        BtnSelAgency.Visible = false;
                        BtnSearcRecs.Enabled = true;
                        //BtnAddApp.Text = "Add a Brand New Application";
                    }


                }
                else
                {
                    MessageBox.Show("Please Fill '" + ((ListItem)CmbSearchBy.SelectedItem).Text.ToString() + "' Search Criteria", "CAPTAIN", MessageBoxButtons.OK);

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
            if (!string.IsNullOrEmpty(MtxtCode.Text))
            {
                //MtxtAppNo.Text = SetLeadingZeros(MtxtAppNo.Text);
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
                    //default: MessageBox.Show("Table Code should not be blank", "CAPTAIN", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }




        private void CmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CmbSearchBy.Items.Count > 0)
                {
                    string strSerchBy = ((ListItem)CmbSearchBy.SelectedItem).Value == null ? string.Empty : ((ListItem)CmbSearchBy.SelectedItem).Value.ToString();
                    SetRelatedSearchControls(strSerchBy);
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void SetRelatedSearchControls(string SearchCatg)
        {

            switch (PrvPanelCode)
            {
                case "1":
                    NamePanel.Visible = false;
                    TxtAgencyName.Clear(); ;
                    break;
                case "3":
                    AddressPanel.Visible = false;
                    TxtStreet.Clear(); TxtCity.Clear(); TxtState.Clear();
                    break;
                case "4":
                    PhonePanel.Visible = false;
                    MtxtPhone.Clear();
                    break;

                case "7":
                    AppNoPanel.Visible = false;
                    MtxtCode.Clear();
                    break;

            }
            switch (SearchCatg)
            {
                case "1": NamePanel.Visible = true; SearcgCategory = "NAM"; break;
                case "3": AddressPanel.Visible = true; SearcgCategory = "ADD"; break;
                case "4": PhonePanel.Visible = true; SearcgCategory = "PHN"; break;
                case "7": AppNoPanel.Visible = true; SearcgCategory = "APP"; break;

            }
            PrvPanelCode = SearchCatg;

            Clear_Search_Controls();
        }

        private void Clear_Search_Controls()
        {
            TxtAgencyName.Clear();
            MtxtPhone.Clear(); MtxtCode.Clear();
            TxtStreet.Clear(); TxtCity.Clear(); TxtState.Clear();


            //SetRelatedSearchControls(SearcgCategory);
        }

        private void SetSearchPanelsLocation()
        {
            NamePanel.Size = new System.Drawing.Size(459, 24);
            NamePanel.Location = new System.Drawing.Point(168, 5);

            AddressPanel.Size = new System.Drawing.Size(459, 24);
            AddressPanel.Location = new System.Drawing.Point(168, 5);
            PhonePanel.Size = new System.Drawing.Size(459, 24);
            PhonePanel.Location = new System.Drawing.Point(168, 5);

            AppNoPanel.Size = new System.Drawing.Size(459, 24);
            AppNoPanel.Location = new System.Drawing.Point(168, 5);

        }


        private void BtnSelApp_Click(object sender, EventArgs e)
        {

            try
            {
                if (GvwSearch.Rows.Count > 0)
                {
                    AGCYPARTEntity AGCYPARTEntity = GvwSearch.SelectedRows[0].Tag as AGCYPARTEntity;
                    if (AGCYPARTEntity != null)
                    {
                        BaseForm.BaseAgencyNo = AGCYPARTEntity.Code;
                        this.DialogResult = DialogResult.OK;
                        this.Close();

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Server is busy, Try after sometime");
            }
        }

        private void Set_APP_Confirmation(object sender, EventArgs e)
        {
            MessageBox messageBoxWindow = sender as MessageBox;
          //  MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            //if (messageBoxWindow.DialogResult == DialogResult.Yes)
            //{
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }



        private void CbSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (CbSearch.Checked)
            {
                SearchHie = null;
            }
            else
            {
                SearchHie = Hierarchy;
            }
        }
               

        private void MtxtAppNo_EnterKeyDown(object objSender, KeyEventArgs objArgs)
        {
            MtxtAppNo_LostFocus(MtxtCode, EventArgs.Empty);
        }

        private void GvwSearch_SelectionChanged(object sender, EventArgs e)
        {
            if (GvwSearch.Rows.Count > 0)
            {
            }

        }

        private void Controls_Common_GotFocus(object sender, EventArgs e)
        {
            if (sender == MtxtCode)
                MtxtCode.SelectionLength = MtxtCode.Text.Length;
            else
                if (sender == MtxtPhone)
                MtxtPhone.SelectionLength = MtxtPhone.Text.Length;

        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            this.GvwSearch.SelectionChanged -= new System.EventHandler(this.GvwSearch_SelectionChanged);
            GvwSearch.Rows.Clear();
            SetRelatedSearchControls(((ListItem)CmbSearchBy.SelectedItem).Value.ToString());
            this.GvwSearch.SelectionChanged += new System.EventHandler(this.GvwSearch_SelectionChanged);
        }



        private void MtxtPhone_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MtxtPhone.Text.Trim()))
            {
                string Tmp_SSn = MtxtPhone.Text + "0000000000".Substring(0, (10 - MtxtPhone.Text.Length));
                MtxtPhone.Text = Tmp_SSn.Replace(' ', '0');
            }
        }


     

        private void Lbl_No_Access_Click(object sender, EventArgs e)
        {

        }

        private void Cb_MST_Prog_Click(object sender, EventArgs e)
        {
            InitializeAllControls();
            CmbProg.Enabled = BtnSelAgency.Visible = false;
            SelProg = null;
            SelDept = ((ListItem)CmbDept.SelectedItem).Value.ToString();

            if (SelDept != DefDept && DefHieExist == true)
                DefHieExist = false;

            FillProgCombo();
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
            //foreach (CaseHierarchyEntity Ent in Progs_List)
            //{
            //    if (Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim() == Prog_Code)
            //    {
            //        Prog_Desc = Ent.HierarchyName;
            //        break;
            //    }
            //}

            return Prog_Desc;
        }


    }
}