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
using DevExpress.XtraReports.UI;
using NPOI.SS.Formula.Functions;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using log4net.Repository.Hierarchy;
using Aspose.Cells;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MainMenuAddApplicantForm : Form
    {

        #region private variables

        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        #endregion


        public MainMenuAddApplicantForm(BaseForm baseForm, char CASE2001_AddPriv, string dob, string fname, string lname, string ssn)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            propClientRulesSwitch = "N";
            BaseForm = baseForm;
            AddPriv = false;
            Fill_Only_Hie = Fill_Only_APP = string.Empty;
            if (CASE2001_AddPriv == 'Y')
            {
                AddPriv = true;

                //if (string.IsNullOrEmpty(dob.Trim()) &&
                //    string.IsNullOrEmpty(fname.Trim()) &&
                //    string.IsNullOrEmpty(lname.Trim()) &&
                //    string.IsNullOrEmpty(ssn.Trim()))
                //    BtnNewApp.Visible = true;
            }

            this.Text = "Main Menu Add Application";



            PassDOB = dob.Trim(); Fname = fname.Trim(); Lname = lname.Trim(); PassSSN = ssn.Trim();

            Fill_Default_Values_inControls();
            //GetSelectedProgram();
            Fill_Gender_MemberCode_Desc_List();

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }

            //if (!string.IsNullOrEmpty(PassDOB) ||
            //   !string.IsNullOrEmpty(Fname) ||
            //   !string.IsNullOrEmpty(Lname) ||
            //   !string.IsNullOrEmpty(PassSSN))
            //    BtnSearch_Click(BtnSearch, EventArgs.Empty);
        }

        /// <summary>
        ///  /// Murali added mainmenu serach logic changed select applicant color.
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="CASE2001_AddPriv"></param>
        /// <param name="dob"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="ssn"></param>
        /// <param name="strApp"></param>
        /// <param name="strhierarchy"></param>
        /// <param name="strYear"></param>

        public MainMenuAddApplicantForm(BaseForm baseForm, char CASE2001_AddPriv, string dob, string fname, string lname, string ssn, string strApp, string strhierarchy, string strYear, string strClientRules, string btnType, string type)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            propClientRulesSwitch = strClientRules;
            BaseForm = baseForm;
            RequireControlEnable();
            AddPriv = false;
            Fill_Only_Hie = Fill_Only_APP = string.Empty;
            propFormType = "ADSEARCH";
            propApp = strApp;
            propYear = strYear;
            propHiearchy = strhierarchy;

            BtnType = btnType;

            if (CASE2001_AddPriv == 'Y')
            {
                AddPriv = true;

                //if (string.IsNullOrEmpty(dob.Trim()) &&
                //    string.IsNullOrEmpty(fname.Trim()) &&
                //    string.IsNullOrEmpty(lname.Trim()) &&
                //    string.IsNullOrEmpty(ssn.Trim()))
                //    BtnNewApp.Visible = true;
            }

            //this.Text = "Main Menu Add Application";
            if (btnType == "Drag")
                this.Text = "Copy/Drag Client – Adding Client and Household Members";
            else
                this.Text = "Main Menu Add Application";

            PassDOB = dob.Trim(); Fname = fname.Trim(); Lname = lname.Trim(); PassSSN = ssn.Trim();

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", BaseForm.BaseAgency, BaseForm.BaseDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                }
            }

            Fill_Default_Values_inControls();
            //GetSelectedProgram();
            Fill_Gender_MemberCode_Desc_List();

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }
            if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
            {
                Name.Width = 100;
                gvtLastName.Visible = true;
                Name.HeaderText = "First Name";
                gvtKeyPoints.Visible = true;
                TopGrid.Columns[11].DisplayIndex = 0;
                TopGrid.Columns[4].DisplayIndex = 1;
                TopGrid.Columns[12].DisplayIndex = 2;
                TopGrid.Columns[5].DisplayIndex = 3;
                TopGrid.Columns[3].DisplayIndex = 4;
                TopGrid.Columns[0].DisplayIndex = 5;
                TopGrid.Columns[1].DisplayIndex = 6;
                TopGrid.Columns[2].DisplayIndex = 7;
                TopGrid.Columns[6].DisplayIndex = 8;
            }


            //if (!string.IsNullOrEmpty(PassDOB) ||
            //   !string.IsNullOrEmpty(Fname) ||
            //   !string.IsNullOrEmpty(Lname) ||
            //   !string.IsNullOrEmpty(PassSSN))
            //    BtnSearch_Click(BtnSearch, EventArgs.Empty);
        }


        string Sum_RefApp_Key = "";
        public MainMenuAddApplicantForm(BaseForm baseForm, char CASE2001_AddPriv, string dob, string fname, string lname, string ssn, string App, string hierarchy, string Calling_Scr, bool Disable_Search_Controls, string casesum_key)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            AddPriv = false;
            propClientRulesSwitch = "N"; ;
            Fill_Only_Hie = propHiearchy = hierarchy.Trim();
            Fill_Only_APP = App;
            if (CASE2001_AddPriv == 'Y')
            {
                AddPriv = true;

                //if (string.IsNullOrEmpty(dob.Trim()) &&
                //    string.IsNullOrEmpty(fname.Trim()) &&
                //    string.IsNullOrEmpty(lname.Trim()) &&
                //    string.IsNullOrEmpty(ssn.Trim()))
                //    BtnNewApp.Visible = true;
            }


            BtnSearch.Visible = BtnNewApp.Visible = false;
            this.Text = Calling_Scr + " - Drag Application";
            Sum_RefApp_Key = casesum_key;
            Sum_RefApp_Key = ""; // Brain Asked to Change it, And will think on it Later to Update Reffer App# in CaseSum while Dragging 


            PassDOB = dob.Trim(); Fname = fname.Trim(); Lname = lname.Trim(); PassSSN = ssn.Trim();

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", BaseForm.BaseAgency, BaseForm.BaseDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                }
            }

            Fill_Default_Values_inControls();
            //GetSelectedProgram();
            Fill_Gender_MemberCode_Desc_List();

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }

            if (Disable_Search_Controls)
                MtxtSsn.Enabled = TxtFName.Enabled = TxtLName.Enabled = DtDOB.Enabled = false;

            //if (!string.IsNullOrEmpty(PassDOB) ||
            //   !string.IsNullOrEmpty(Fname) ||
            //   !string.IsNullOrEmpty(Lname) ||
            //   !string.IsNullOrEmpty(PassSSN))
            //    BtnSearch_Click(BtnSearch, EventArgs.Empty);
        }


        /*** KRANTHI 07-21-2022:: For to view the Applicant details ***/
        public MainMenuAddApplicantForm(BaseForm baseForm, char CASE2001_AddPriv, string dob, string fname, string lname, string ssn, string strApp,
            string strhierarchy, string strYear, string strClientRules, string Mode)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            propClientRulesSwitch = strClientRules;
            BaseForm = baseForm;
            RequireControlEnable();
            AddPriv = false;
            Fill_Only_Hie = Fill_Only_APP = string.Empty;
            propFormType = "ADSEARCH";
            propApp = strApp;
            propYear = strYear;
            propHiearchy = strhierarchy;
            //Fill_Only_Hie= strhierarchy;
            BtnType = "View";
            Lbl_No_Access.Visible = false;
            Lbl_No_Access.Dock = DockStyle.Right;

            if (CASE2001_AddPriv == 'Y')
            {
                AddPriv = true;
            }
            SearchHie = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear;
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", BaseForm.BaseAgency, BaseForm.BaseDept, " "); // Verify it Once

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (ds.Tables.Count > 1)
                        User_Hie_Acc_List = ds.Tables[1];

                }
            }

            //this.Text = "View Applicant Details";
            this.Text = "Applicant/Member Details and Programs";

            PassDOB = dob.Trim(); Fname = fname.Trim(); Lname = lname.Trim(); PassSSN = ssn.Trim();

            Fill_Default_Values_inControls();
            //GetSelectedProgram();
            Fill_Gender_MemberCode_Desc_List();

            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }
            if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
            {
                Name.Width = 100;
                gvtLastName.Visible = true;
                Name.HeaderText = "First Name";
                gvtKeyPoints.Visible = true;
                TopGrid.Columns[11].DisplayIndex = 0;
                TopGrid.Columns[4].DisplayIndex = 1;
                TopGrid.Columns[12].DisplayIndex = 2;
                TopGrid.Columns[5].DisplayIndex = 3;
                TopGrid.Columns[3].DisplayIndex = 4;
                TopGrid.Columns[0].DisplayIndex = 5;
                TopGrid.Columns[1].DisplayIndex = 6;
                TopGrid.Columns[2].DisplayIndex = 7;
                TopGrid.Columns[6].DisplayIndex = 8;
            }

            BottomGrid.Columns["SSN1"].Visible = false;

            pnlHeader.Visible = false;
            SearchPanel.Visible = false;
            //pnlSaveControls.Visible = false;
            pnlSaveControls.Visible = true;
            BtnDragApp.Visible = false; BtnNewApp.Visible = false; btnRecentData.Visible = false; BtnCancel.Visible = false;
            //BtnSelApp.Visible = true;
            this.Size = new System.Drawing.Size(840, this.Height - (pnlHeader.Height + pnlSaveControls.Height + SearchPanel.Height));

           
        }

        string SearchHie = "";
        bool Called_From_FormLoad = true;
        private void MainMenuAddApplicantForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PassDOB) ||
               !string.IsNullOrEmpty(Fname) ||
               !string.IsNullOrEmpty(Lname) ||
               !string.IsNullOrEmpty(PassSSN))
                BtnSearch_Click(sender, EventArgs.Empty);


            //BottomGrid_CellClick(BottomGrid, new DataGridViewCellEventArgs(0, 0) { });
            if (TopGrid.Rows.Count > 0)
                GetDup_APP_MEM_Status_In_APP_Change();
        }


        string[,] Gender_Desc, Mem_Desc;
        private void Fill_Gender_MemberCode_Desc_List()
        {
            List<CommonEntity> Gender = _model.lookupDataAccess.GetGender();
            List<CommonEntity> Relation = _model.lookupDataAccess.GetRelationship();
            Gender_Desc = new string[Gender.Count, 2];
            Mem_Desc = new string[Relation.Count, 2];

            int i = 0;
            foreach (CommonEntity gender in Gender)
            {
                Gender_Desc[i, 0] = gender.Code;
                Gender_Desc[i, 1] = gender.Desc;
                i++;
            }
            i = 0;
            foreach (CommonEntity Rel in Relation)
            {
                Mem_Desc[i, 0] = Rel.Code;
                Mem_Desc[i, 1] = Rel.Desc;
                i++;
            }

        }

        public BaseForm BaseForm { get; set; }

        public string MainMenuAgency { get; set; }

        public string MainMenuDept { get; set; }

        public string MainMenuProgram { get; set; }

        public string MainMenuYear { get; set; }

        public string MainMenuCode { get; set; }

        public bool AddPriv { get; set; }

        public string Fill_Only_Hie { get; set; }

        public string Fill_Only_APP { get; set; }

        public string PassDOB { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string PassSSN { get; set; }
        public string propFormType { get; set; }
        string propApp { get; set; }
        string propYear { get; set; }
        string propHiearchy { get; set; }
        string propClientRulesSwitch { get; set; }

        string BtnType { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        PrivilegeEntity Privileges = new PrivilegeEntity();



        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void Fill_Default_Values_inControls()
        {
            if (!string.IsNullOrEmpty(PassDOB))
                DtDOB.Value = Convert.ToDateTime(PassDOB);

            if (!string.IsNullOrEmpty(Fname))
                TxtFName.Text = Fname;

            if (!string.IsNullOrEmpty(Lname))
                TxtLName.Text = Lname;

            if (!string.IsNullOrEmpty(PassSSN))
                MtxtSsn.Text = PassSSN;
        }

        bool Atleast_Once_Searched = false;
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (ValidationSearch())
            {
                if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()) ||
                   !string.IsNullOrEmpty(TxtFName.Text.Trim()) ||
                   !string.IsNullOrEmpty(TxtLName.Text.Trim()) ||
                   DtDOB.Checked)
                {

                    string DateofBirth = null;
                    BtnDragApp.Visible = false;
                    Error_1.Visible = false;
                    if (DtDOB.Checked)
                        DateofBirth = DtDOB.Value.ToShortDateString();
                    Atleast_Once_Searched = true;

                    DataSet ds = new DataSet();

                    string strClientSwitch = string.Empty;
                    if (propClientRulesSwitch == "Y")
                    {
                        if (BaseForm.BaseAgencyControlDetails != null)
                            strClientSwitch = BaseForm.BaseAgencyControlDetails.ClientRules;
                    }
                    if (string.IsNullOrEmpty(Fill_Only_Hie.Trim()))
                        ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "ALL", null, null, null, TxtLName.Text, TxtFName.Text, MtxtSsn.Text, null, null, null, null, null, null, null, null, DateofBirth, BaseForm.UserID, strClientSwitch, string.Empty, null);
                    else
                        ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "APP", null, null, Fill_Only_APP, TxtLName.Text, TxtFName.Text, MtxtSsn.Text, null, null, null, null, null, null, null, Fill_Only_Hie, DateofBirth, BaseForm.UserID, strClientSwitch, string.Empty, null);

                    TopGrid.SelectionChanged -= new EventHandler(TopGrid_SelectionChanged);
                    TopGrid.Rows.Clear();
                    BottomGrid.Rows.Clear();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        DataView dv = new DataView(dt);
                        dv.Sort = "SNP_DATE_LSTC desc";
                        dt = dv.ToTable();

                        /********************************************************************************************************/
                        /******************************** Show only Intake Agency Records ***************************************/
                        string _setAgy = propHiearchy.Substring(0, 2);
                        DataRow[] drfiltAgy = dt.Select("Agency='" + _setAgy + "'");
                        if (drfiltAgy.Length > 0)
                        {
                            dt = new DataTable();
                            dt = drfiltAgy.CopyToDataTable();
                        }

                        /******************************************************************************************************/
                        int TmpRows = 0;
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                string TmpHierarchy = null, DOB = null, Snp_Lstc_Date = null;
                                string Address = null;
                                string TmpAddress = null;
                                string TmpSsn = null;
                                int TmpLength = 0;
                                char TmpSpace = ' ';
                                string TmpYear = "    ";
                                string TmpName = "    ";
                                string Mst_Key = null;

                                if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
                                {
                                    List<CaseSnpEntity> snpalldata = new List<CaseSnpEntity>();
                                    int intsearchcount = 0;
                                    int SSNPoint = 0, DOBpoint = 0, FNamePoint = 0, LastNamePoint = 0, DOBLNamePoint = 0, SSNLNamePoint = 0, DOBFNamePoint = 0;
                                    if (BaseForm.BaseAgencyControlDetails.SSNPoint != string.Empty)
                                        SSNPoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SSNPoint);
                                    if (BaseForm.BaseAgencyControlDetails.DOBPoint != string.Empty)
                                        DOBpoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBPoint);
                                    if (BaseForm.BaseAgencyControlDetails.FirstNamePoint != string.Empty)
                                        FNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.FirstNamePoint);
                                    if (BaseForm.BaseAgencyControlDetails.LastNamePoint != string.Empty)
                                        LastNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.LastNamePoint);
                                    if (BaseForm.BaseAgencyControlDetails.DOBLastNamePoint != string.Empty)
                                        DOBLNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBLastNamePoint);
                                    //added by Sudheer on 02/21/2018
                                    if (BaseForm.BaseAgencyControlDetails.DOBFirstNamePoint != string.Empty)
                                        DOBFNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.DOBFirstNamePoint);

                                    if (BaseForm.BaseAgencyControlDetails.SSNLastNamePoint != string.Empty)
                                        SSNLNamePoint = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SSNLastNamePoint);
                                    int intSearchhit = 0;
                                    int intsearchrating = 0;
                                    if (BaseForm.BaseAgencyControlDetails.SearchHit == "3")
                                    {
                                        if (BaseForm.BaseAgencyControlDetails.SearchRating != string.Empty)
                                            intsearchrating = Convert.ToInt32(BaseForm.BaseAgencyControlDetails.SearchRating);
                                    }
                                    string SsnSwitch, NameSwitch, DobSwitch, LastNameSwitch;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        intsearchcount = 0;
                                        SsnSwitch = NameSwitch = LastNameSwitch = DobSwitch = "N";
                                        if (MtxtSsn.Text.Trim() == dr["SSN"].ToString())
                                        {
                                            SsnSwitch = "Y";
                                            intsearchcount = SSNPoint;
                                        }
                                        if (DtDOB.Checked && dr["DOB"].ToString() != string.Empty)
                                        {
                                            if (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString()))
                                            {
                                                intsearchcount = intsearchcount + DOBpoint;
                                                DobSwitch = "Y";
                                            }
                                            if ((dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper()) && (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString())))
                                            {
                                                intsearchcount = intsearchcount + DOBLNamePoint;
                                                if (BaseForm.BaseAgencyControlDetails.SearchHit == "2")
                                                    intSearchhit = intSearchhit + 1;
                                            }
                                            //added by sudheer on 02/21/2018
                                            if ((dr["FName"].ToString().ToUpper() == TxtFName.Text.ToUpper()) && (Convert.ToDateTime(DateofBirth) == Convert.ToDateTime(dr["DOB"].ToString())))
                                            {
                                                intsearchcount = intsearchcount + DOBFNamePoint;
                                                if (BaseForm.BaseAgencyControlDetails.SearchHit == "4")
                                                    intSearchhit = intSearchhit + 1;
                                            }

                                        }
                                        if (dr["FName"].ToString().ToUpper() == TxtFName.Text.ToUpper())
                                        {
                                            NameSwitch = "Y";
                                            intsearchcount = intsearchcount + FNamePoint;
                                        }
                                        if (dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper())
                                        {
                                            LastNameSwitch = "Y";
                                            intsearchcount = intsearchcount + LastNamePoint;
                                        }
                                        if ((dr["LName"].ToString().ToUpper() == TxtLName.Text.ToUpper()) && (MtxtSsn.Text.Trim() == dr["SSN"].ToString()))
                                        {
                                            intsearchcount = intsearchcount + SSNLNamePoint;
                                            if (BaseForm.BaseAgencyControlDetails.SearchHit == "1")
                                                intSearchhit = intSearchhit + 1;
                                        }
                                        if (BaseForm.BaseAgencyControlDetails.SearchHit == "3")
                                        {
                                            if (intsearchrating < intsearchcount)
                                                intSearchhit = intSearchhit + 1;
                                        }

                                        snpalldata.Add(new CaseSnpEntity(dr, "MAINMENUSEARCH", intsearchcount, SsnSwitch, NameSwitch, LastNameSwitch, DobSwitch));
                                    }
                                    bool Can_Add_Cur_Hie = true, All_Hie_Access = true; ;
                                    string Priv_Hie = null; SearchHie = null;
                                    if (string.IsNullOrEmpty(SearchHie))
                                        All_Hie_Access = Get_Hi_Access_Status(string.Empty);

                                    snpalldata = snpalldata.OrderByDescending(u => Convert.ToDateTime(u.DateLstc)).ThenByDescending(u => u.searchAppCount).ToList();
                                    foreach (CaseSnpEntity snpdata in snpalldata)
                                    {

                                        int rowIndex = 0;
                                        DOB = Snp_Lstc_Date = " ";

                                        TmpSsn = snpdata.Ssno.ToString();
                                        TmpLength = (9 - TmpSsn.Length);
                                        for (int i = 0; i < TmpLength; i++)
                                            TmpAddress += TmpSpace;
                                        TmpSsn += TmpAddress;
                                        TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", ProgramDefinition.SSNReasonFlag.Trim(), string.Empty);
                                        TmpHierarchy = snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString();    //RecKey
                                        TmpYear = "    ";
                                        TmpName = null;


                                        Mst_Key = null;
                                        Mst_Key = snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString() + (snpdata.Year.ToString() == string.Empty ? "    " : snpdata.Year.ToString()) + snpdata.Appdetails.ToString();

                                        if (!string.IsNullOrEmpty(snpdata.AltBdate.ToString()))
                                            //DOB = Convert.ToDateTime(snpdata.AltBdate.ToString()).ToShortDateString();
                                            DOB = Convert.ToDateTime(snpdata.AltBdate).ToString("MM/dd/yyyy");

                                        if (!string.IsNullOrEmpty(snpdata.DateLstc.ToString()))
                                            Snp_Lstc_Date = snpdata.DateLstc.ToString();
                                        else
                                            Snp_Lstc_Date = "01/01/1900";

                                        // TmpName = LookupDataAccess.GetMemberName(snpdata.NameixFi.ToString(), snpdata.NameixMi.ToString(), snpdata.NameixLast.ToString(), BaseForm.BaseHierarchyCnFormat.ToString());

                                        var lstcdatetime = Convert.ToDateTime(Snp_Lstc_Date).ToString("MM/dd/yyyy");

                                        if (!All_Hie_Access)
                                        {
                                            if (Priv_Hie != snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString())
                                                Can_Add_Cur_Hie = Get_Hi_Access_Status(snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString());
                                        }

                                        string _otherHIEAccess = "N";
                                        if (Can_Add_Cur_Hie)
                                            _otherHIEAccess = "Y";

                                        string TmpClientID = string.Empty;
                                        if (!string.IsNullOrEmpty(snpdata.ClientId.Trim()))
                                            TmpClientID = snpdata.ClientId;

                                        //rowIndex = TopGrid.Rows.Add(TmpHierarchy, snpdata.Year, snpdata.Appdetails, TmpSsn, snpdata.NameixFi.ToString(), DOB, lstcdatetime, " ", Mst_Key, snpdata.FamilySeq, snpdata.Ssno.ToString(), snpdata.searchAppCount, snpdata.NameixLast.ToString());
                                        if (BtnType == "View")
                                            rowIndex = TopGrid.Rows.Add(TmpHierarchy, snpdata.Year, Get_Program_Desc(TmpHierarchy), snpdata.Appdetails, TmpSsn, snpdata.NameixFi.ToString(), snpdata.NameixLast.ToString(), DOB, lstcdatetime, " ", Mst_Key, snpdata.FamilySeq, snpdata.Ssno.ToString(), snpdata.searchAppCount, _otherHIEAccess, TmpClientID);
                                        else
                                            rowIndex = TopGrid.Rows.Add(TmpHierarchy, snpdata.Year, Get_Program_Desc(TmpHierarchy), snpdata.Appdetails, TmpSsn, snpdata.NameixFi.ToString(), snpdata.NameixLast.ToString(), DOB, lstcdatetime, " ", Mst_Key, snpdata.FamilySeq, snpdata.Ssno.ToString(), snpdata.searchAppCount, _otherHIEAccess,TmpClientID);

                                        TopGrid.Rows[rowIndex].Tag = snpdata;
                                        if (snpdata.SnpAcitveStatus.ToString().Trim() != "A")// && (dr["AppNo"].ToString()).Substring(10, 1) == "A")  
                                            TopGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                                        if (snpdata.SsnSwitch == "Y")
                                            TopGrid.Rows[rowIndex].Cells[3].Style.ForeColor = Color.Green;
                                        if (snpdata.DobSwitch == "Y")
                                            TopGrid.Rows[rowIndex].Cells[5].Style.ForeColor = Color.Green;
                                        if (snpdata.NameSwitch == "Y")
                                            TopGrid.Rows[rowIndex].Cells[4].Style.ForeColor = Color.Green;
                                        if (snpdata.LastNameSwitch == "Y")
                                            TopGrid.Rows[rowIndex].Cells[12].Style.ForeColor = Color.Green;

                                        if (propFormType == "ADSEARCH")
                                        {
                                            string strAppno = snpdata.Appdetails.ToString();
                                            if (strAppno.Length > 8)
                                            {
                                                strAppno = strAppno.Substring(0, 8);
                                            }
                                            if (propHiearchy.Trim() == TmpHierarchy.Trim() && propYear.Trim() == snpdata.Year.ToString().Trim() && propApp.Trim() == strAppno.Trim())
                                            {
                                                TopGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                                            }
                                        }



                                        Priv_Hie = snpdata.Agency.ToString() + snpdata.Dept.ToString() + snpdata.Program.ToString();
                                        TmpRows++;
                                    }

                                    if (BtnType == "View")
                                    {
                                        TopGrid.Columns["Prog_Name"].Visible = true;
                                        TopGrid.Columns["SSN"].Visible = false;
                                        TopGrid.Columns["gvtLastName"].Visible = true;
                                        TopGrid.Columns["Name"].Name = "First Name";
                                        TopGrid.Columns["Name"].Width = 115;
                                        TopGrid.Columns["gvtLastName"].Width = 115;
                                    }
                                    else
                                    {
                                        TopGrid.Columns["Prog_Name"].Visible = false;
                                        TopGrid.Columns["SSN"].Visible = true;
                                        TopGrid.Columns["gvtLastName"].Visible = false;
                                        TopGrid.Columns["Name"].Name = "Name";
                                        TopGrid.Columns["Name"].Width = 230;
                                        //TopGrid.Columns["gvtLastName"].Width = 115;
                                    }

                                    if (TmpRows > 0)
                                    {
                                        Loading_Complete = true;
                                        if (TopGrid.Rows.Count > 0)
                                        {

                                            TopGrid.CurrentCell = TopGrid.Rows[0].Cells[0];
                                            TopGrid.Rows[0].Selected = true;
                                            //TopGrid.Columns[4].DisplayIndex = 0;
                                            //TopGrid.Columns[5].DisplayIndex = 1;
                                            //TopGrid.Columns[3].DisplayIndex = 2;
                                            //TopGrid.Columns[0].DisplayIndex = 3;
                                            //TopGrid.Columns[1].DisplayIndex = 4;
                                            //TopGrid.Columns[2].DisplayIndex = 5;
                                            //TopGrid.Columns[11].DisplayIndex = 6;
                                            //TopGrid.Columns[6].DisplayIndex = 7;
                                            GetHie_App_Details();
                                            if (intSearchhit == 0)
                                                BtnNewApp.Visible = true;
                                            else
                                                BtnNewApp.Visible = false;


                                        }
                                        LblHeader.Text = "Client is in the programs/hierarchies listed below ";

                                    }

                                }
                                else
                                {

                                    bool Can_Add_Cur_Hie = true, All_Hie_Access = true; ;
                                    string Priv_Hie = null; SearchHie = null;
                                    if (string.IsNullOrEmpty(SearchHie))
                                        All_Hie_Access = Get_Hi_Access_Status(string.Empty);

                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        int rowIndex = 0;
                                        DOB = Snp_Lstc_Date = " ";

                                        TmpSsn = dr["Ssn"].ToString();
                                        TmpLength = (9 - TmpSsn.Length);
                                        for (int i = 0; i < TmpLength; i++)
                                            TmpAddress += TmpSpace;
                                        TmpSsn += TmpAddress;
                                        TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", ProgramDefinition.SSNReasonFlag.Trim(), string.Empty);
                                        TmpHierarchy = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();    //RecKey
                                        TmpYear = "    ";
                                        TmpName = null;


                                      


                                        //if (dr["Fname"].ToString().Trim().Length > 0)
                                        //    TmpName = dr["Fname"].ToString().Trim();
                                        //if (dr["Lname"].ToString().Trim().Length > 0)
                                        //{
                                        //    if (!(string.IsNullOrEmpty(TmpName)))
                                        //        TmpName += ", ";
                                        //    TmpName += dr["Lname"].ToString().Trim();
                                        //}

                                        Mst_Key = null;
                                        Mst_Key = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString() + (dr["SnpYear"].ToString() == string.Empty ? "    " : dr["SnpYear"].ToString()) + dr["AppNo"].ToString();

                                        if (!string.IsNullOrEmpty(dr["DOB"].ToString()))
                                            //DOB = Convert.ToDateTime(dr["DOB"].ToString()).ToShortDateString();
                                            DOB = Convert.ToDateTime(dr["DOB"]).ToString("MM/dd/yyyy");

                                        if (!string.IsNullOrEmpty(dr["SNP_DATE_LSTC"].ToString()))
                                            Snp_Lstc_Date = dr["SNP_DATE_LSTC"].ToString();
                                        else
                                            Snp_Lstc_Date = "01/01/1900";

                                        TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString(), dr["MName"].ToString(), dr["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat.ToString());

                                        //commented by sudheer on 08/12/2020
                                        //var lstcdatetime = Convert.ToDateTime(Snp_Lstc_Date).Date;
                                        var lstcdatetime = Convert.ToDateTime(Snp_Lstc_Date).ToString("MM/dd/yyyy");


                                        if (!All_Hie_Access)
                                        {
                                            if (Priv_Hie != dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString())
                                                Can_Add_Cur_Hie = Get_Hi_Access_Status(dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString());
                                        }

                                        string _otherHIEAccess = "N";
                                        if (Can_Add_Cur_Hie)
                                            _otherHIEAccess = "Y";

                                        string TmpClientID = string.Empty;
                                        if (!string.IsNullOrEmpty(dr["ClientID"].ToString()))
                                            TmpClientID = dr["ClientID"].ToString();

                                        //rowIndex = TopGrid.Rows.Add(TmpHierarchy, dr["SnpYear"], dr["AppNo"], TmpSsn, TmpName, DOB, lstcdatetime, " ", Mst_Key, dr["RecFamSeq"], dr["Ssn"].ToString());

                                        if (BtnType == "View")
                                            rowIndex = TopGrid.Rows.Add(TmpHierarchy, dr["SnpYear"], Get_Program_Desc(TmpHierarchy), dr["AppNo"], TmpSsn, dr["Fname"].ToString(), dr["Lname"].ToString(), DOB, lstcdatetime, " ", Mst_Key, dr["RecFamSeq"], dr["Ssn"].ToString(),"",_otherHIEAccess, TmpClientID);
                                        else
                                            rowIndex = TopGrid.Rows.Add(TmpHierarchy, dr["SnpYear"], Get_Program_Desc(TmpHierarchy), dr["AppNo"], TmpSsn, TmpName, dr["Lname"].ToString(), DOB, lstcdatetime, " ", Mst_Key, dr["RecFamSeq"], dr["Ssn"].ToString(), "", _otherHIEAccess, TmpClientID);

                                        TopGrid.Rows[rowIndex].Tag = dr;
                                        if (dr["AppStatus"].ToString().Trim() != "A")// && (dr["AppNo"].ToString()).Substring(10, 1) == "A")  
                                            TopGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        if (propFormType == "ADSEARCH")
                                        {
                                            string strAppno = dr["AppNo"].ToString();
                                            if (strAppno.Length > 8)
                                            {
                                                strAppno = strAppno.Substring(0, 8);
                                            }
                                            if (propHiearchy.Trim() == TmpHierarchy.Trim() && propYear.Trim() == dr["SnpYear"].ToString().Trim() && propApp.Trim() == strAppno.Trim())
                                            {
                                                TopGrid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                                            }
                                        }

                                        Priv_Hie = dr["Agency"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();

                                        TmpRows++;
                                    }

                                    if (BtnType == "View")
                                    {
                                        TopGrid.Columns["Prog_Name"].Visible = true;
                                        TopGrid.Columns["SSN"].Visible = false;
                                        TopGrid.Columns["gvtLastName"].Visible = true;
                                        TopGrid.Columns["Name"].HeaderText = "First Name";
                                        TopGrid.Columns["Name"].Width = 115;
                                        TopGrid.Columns["gvtLastName"].Width = 115;
                                    }
                                    else
                                    {
                                        TopGrid.Columns["Prog_Name"].Visible = false;
                                        TopGrid.Columns["SSN"].Visible = true;
                                        TopGrid.Columns["gvtLastName"].Visible = false;
                                        TopGrid.Columns["Name"].HeaderText = "Name";
                                        TopGrid.Columns["Name"].Width = 230;
                                        //TopGrid.Columns["gvtLastName"].Width = 115;
                                    }

                                    if (TmpRows > 0)
                                    {
                                        //TopGrid.Sort(TopGrid.Columns["LstcDate"], ListSortDirection.Descending);
                                        Loading_Complete = true;
                                        if (TopGrid.Rows.Count > 0)
                                        {
                                            TopGrid.CurrentCell = TopGrid.Rows[0].Cells[0];
                                            TopGrid.Rows[0].Selected = true;

                                            //string strMemberData = TopGrid.Rows[0].Cells["SnpKey"].Value.ToString() + TopGrid.Rows[0].Cells["SnpFamSeq"].Value.ToString();
                                            GetHie_App_Details();

                                        }
                                        LblHeader.Text = "Client is in the programs/hierarchies listed below ";

                                        //if(AddPriv)
                                        //    BtnDragApp.Visible = true;
                                        ////////AppDetailsPanel.Visible = true;
                                        //////btnAppliciant.Visible = true;
                                    }
                                }
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        if (sender == BtnSearch)
                            MessageBox.Show("No Record(s) Exists with selected Criteria", "CAP Systems"); // Kathy Asked to Comment this on 09122013
                        Error_1.Visible = BtnDragApp.Visible = false;
                        BtnNewApp.Visible = true;
                    }
                    TopGrid.SelectionChanged += new EventHandler(TopGrid_SelectionChanged);
                }
                else
                    MessageBox.Show("Please Fill Search Criteria and Proceed", "CAP Systems");
            }

        }
        DataTable User_Hie_Acc_List = new DataTable();
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
        public void RequireControlEnable()
        {
            lblSSNReq.Visible = lblDobReq.Visible = lblFirstNameReq.Visible = lblLastNameReq.Visible = false;
            if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
            {
                if (BaseForm.BaseAgencyControlDetails.SSN == "Y")
                    lblSSNReq.Visible = true;
                if (BaseForm.BaseAgencyControlDetails.DOB == "Y")
                    lblDobReq.Visible = true;
                if (BaseForm.BaseAgencyControlDetails.FirstName == "Y")
                    lblFirstNameReq.Visible = true;
                if (BaseForm.BaseAgencyControlDetails.LastName == "Y")
                    lblLastNameReq.Visible = true;

            }
        }

        public bool ValidationSearch()
        {
            bool isvalid = true;
            if (BaseForm.BaseAgencyControlDetails.ClientRules == "Y" && propClientRulesSwitch == "Y")
            {
                if (lblSSNReq.Visible == true && string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
                {
                    _errorProvider.SetError(MtxtSsn, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSSNReq.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isvalid = false;
                }
                else
                {
                    _errorProvider.SetError(MtxtSsn, null);
                }
                if (lblDobReq.Visible == true && DtDOB.Checked == false)
                {
                    _errorProvider.SetError(DtDOB, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDOB.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isvalid = false;
                }
                else
                {
                    _errorProvider.SetError(DtDOB, null);
                }
                if (lblFirstNameReq.Visible == true && string.IsNullOrEmpty(TxtFName.Text.Trim()))
                {
                    _errorProvider.SetError(TxtFName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFirstName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isvalid = false;
                }
                else
                {
                    _errorProvider.SetError(TxtFName, null);
                }
                if (lblLastNameReq.Visible == true && string.IsNullOrEmpty(TxtLName.Text.Trim()))
                {
                    _errorProvider.SetError(TxtLName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLastName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isvalid = false;
                }
                else
                {
                    _errorProvider.SetError(TxtLName, null);
                }
            }
            return isvalid;

        }

        bool Loading_Complete = false;
        private void GetHie_App_Details()
        {
            Error_1.Visible = false;
            if (Loading_Complete)
            {
                if (TopGrid.SelectedRows[0].Selected == true)
                {
                    DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuGetHouseDetails(TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString() + TopGrid.SelectedRows[0].Cells["SnpFamSeq"].Value.ToString());
                    DataTable dt1 = ds1.Tables[0];

                    GetDup_APP_MEM_Status();

                    BottomGrid.Rows.Clear();
                    try
                    {
                        int TmpRows = 0;
                        string TmpName = null;
                        string TmpAddress = null;
                        string TmpDOB = null;
                        string TmpUpdated = null;
                        string TmpSsn = null;string TmpClientID = string.Empty;
                        int TmpLength = 0;
                        char TmpSpace = ' ';
                        string Tme_Gender = null, Tmp_Relation = null;
                        int selIndex = 0;
                        foreach (DataRow dr in dt1.Rows)
                        {
                            int rowIndex = 0;

                            //TmpName = dr["Fname"] + ", " + dr["Lname"] + "  " + dr["MName"];
                            TmpName = LookupDataAccess.GetMemberName(dr["Fname"].ToString(), dr["MName"].ToString(), dr["Lname"].ToString(), BaseForm.BaseHierarchyCnFormat.ToString());
                            TmpClientID = dr["SNP_CLIENT_ID"].ToString();
                            TmpSsn = dr["Ssn"].ToString();
                            TmpSsn = LookupDataAccess.GetCardNo(TmpSsn, "1", ProgramDefinition.SSNReasonFlag.Trim(), string.Empty);
                            //TmpLength = (9 - TmpSsn.Length);
                            //for (int i = 0; i < TmpLength; i++)
                            //    TmpAddress += TmpSpace;
                            //TmpSsn += TmpAddress;
                            //TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);
                            TmpDOB = " ";

                            if (!string.IsNullOrEmpty(dr["Dob"].ToString()))
                                TmpDOB = Convert.ToDateTime(dr["Dob"]).ToString("MM/dd/yyyy");//.ToShortDateString();

                            //TmpDOB = dr["Dob"].ToString();
                            //TmpDOB = TmpDOB.Substring(4,2) + '/' + TmpDOB.Substring(1,2) + '/' + TmpDOB.Substring(7,2);
                            string[] time = Regex.Split(TmpDOB.ToString(), " ");
                            TmpDOB = time[0];
                            TmpUpdated = null;
                            TmpUpdated = Convert.ToDateTime(dr["Updated"]).ToString("MM/dd/yyyy");//dr["Updated"].ToString();
                            time = null;
                            time = Regex.Split(TmpUpdated.ToString(), " ");
                            TmpUpdated = time[0];
                            //TmpUpdated = TmpUpdated.Substring(3, 2) + '/' + TmpUpdated.Substring(0, 2) + '/' + TmpUpdated.Substring(6, 2);

                            Tme_Gender = Tmp_Relation = null;


                            for (int i = 0; i < (Gender_Desc.Length / 2); i++)
                            {
                                if (Gender_Desc[i, 0] == dr["SNP_SEX"].ToString())
                                {
                                    Tme_Gender = Gender_Desc[i, 1];
                                    break;
                                }
                            }
                            for (int i = 0; i < (Mem_Desc.Length / 2); i++)
                            {
                                if (Mem_Desc[i, 0] == dr["SNP_MEMBER_CODE"].ToString())
                                {
                                    Tmp_Relation = Mem_Desc[i, 1];
                                    break;
                                }
                            }


                            if (TopGrid.SelectedRows[0].Cells["SnpFamSeq"].Value.ToString() == dr["FamSeq"].ToString())
                            {
                                rowIndex = BottomGrid.Rows.Add(true, TmpName, TmpSsn, TmpDOB, Tmp_Relation, Tme_Gender, dr["SNP_AGE"].ToString(), TmpUpdated, " ", dr["FamSeq"].ToString(), dr["Ssn"].ToString(), dr["App_Mem_SW"].ToString(), dr["Fname"].ToString(),TmpClientID);
                                selIndex = rowIndex;
                            }
                            else
                                rowIndex = BottomGrid.Rows.Add(false, TmpName, TmpSsn, TmpDOB, Tmp_Relation, Tme_Gender, dr["SNP_AGE"].ToString(), TmpUpdated, " ", dr["FamSeq"].ToString(), dr["Ssn"].ToString(), dr["App_Mem_SW"].ToString(), dr["Fname"].ToString(),TmpClientID);

                            BottomGrid.Rows[rowIndex].Tag = dr;

                            if (dr["App_Mem_SW"].ToString().Trim() == "A")
                                BottomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                            if (dr["SNP_STATUS"].ToString().Trim() != "A")//|| dr["SNP_STATUS"].ToString() != "i")
                                BottomGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


                            TmpRows++;
                        }

                        if (TmpRows > 0)
                        {
                            BottomGrid.CurrentCell = BottomGrid.Rows[selIndex].Cells[0];
                            BottomGrid.Rows[0].Tag = 0;
                        }
                    }
                    catch (Exception ex) { }


                    string _otherHIEAccess = TopGrid.SelectedRows[0].Cells["OtherHIEACCESS"].Value.ToString();
                    if (_otherHIEAccess == "N")
                    {
                        BtnSelApp.Visible = false;/*BtnDragApp.Visible = false;*/
                        Lbl_No_Access.Visible = true;
                        Lbl_No_Access.Dock = DockStyle.Fill;
                        Lbl_No_Access.TextAlign = ContentAlignment.MiddleRight;
                    }
                    else if (_otherHIEAccess == "Y")
                    {
                        //BtnSelApp.Visible = true;
                        //Lbl_No_Access.Visible = false;
                        //Lbl_No_Access.Dock = DockStyle.Right;
                        string _HIEarry = TopGrid.SelectedRows[0].Cells["Hie"].Value.ToString();
                        string _agy = _HIEarry.Substring(0, 2); string _dep = _HIEarry.Substring(2, 2); string _prog = _HIEarry.Substring(4, 2);
                        ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(_agy, _dep, _prog);
                        if (programEntity.DepIntakeProg == "Y")
                        {
                            BtnSelApp.Visible = true; /*BtnDragApp.Visible = true;*/
                            Lbl_No_Access.Visible = false;
                            Lbl_No_Access.Dock = DockStyle.Right;
                        }
                        else if (programEntity.DepIntakeProg == "N")
                        {
                            BtnSelApp.Visible = false; /*BtnDragApp.Visible = false;*/
                            Lbl_No_Access.Visible = true;
                            Lbl_No_Access.Dock = DockStyle.Fill;
                            Lbl_No_Access.TextAlign = ContentAlignment.MiddleRight;
                        }
                    }
                }
            }
        }


        private void GetDup_APP_MEM_Status()
        {
            bool Can_Drag = true; Error_1.Visible = false;
            if (TopGrid.SelectedRows[0].Selected == true)
            {
                string strSSN = TopGrid.SelectedRows[0].Cells["SsnKey"].Value.ToString();
                string strClientID = TopGrid.SelectedRows[0].Cells["gvClientID"].Value.ToString();
                if (strSSN == "000000000" && ProgramDefinition.SSNReasonFlag.ToUpper().Trim() == "Y")
                {
                    Can_Drag = true;
                }
                else
                {
                    DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(TopGrid.SelectedRows[0].Cells["SsnKey"].Value.ToString(),
                                                                                       BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, "AddApplicant", TopGrid.SelectedRows[0].Cells["gvtLastName"].Value.ToString(), null, TopGrid.SelectedRows[0].Cells["gvDOB"].Value.ToString(), strClientID);
                    DataTable dt2, dt3;
                    if (ds1.Tables.Count > 0)
                    {
                        dt2 = ds1.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            if (dt2.Rows[0]["APP_MEM"].ToString() == "A")
                                Error_1.Text = "Applicant already exists in  " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   Cannot Copy/Drag";
                            else
                                Error_1.Text = "Member already exists in " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   Cannot Copy/Drag";

                            this.Error_1.ForeColor = System.Drawing.Color.Black;
                            Error_1.Visible = true; Can_Drag = false;
                        }
                        else
                        {
                            if (AddPriv)
                                BtnDragApp.Visible = true;
                            Error_1.Visible = false;
                        }
                    }
                }
                if (Can_Drag)
                {
                    this.Error_1.ForeColor = System.Drawing.Color.Black;// GreenYellow;

                    Error_1.Text = "the drag button will copy the customer into the " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim();
                    Error_1.Visible = true;

                    if (BtnType == "Drag")
                    {
                        SearchPanel.Visible = false;
                        //panel1.Dock = DockStyle.Fill;
                        TopGridPanel.Dock = DockStyle.Fill;
                        //panel1.BringToFront();
                    }
                }


                //if (ds1.Tables.Count > 1 && Can_Drag)
                //{
                //    dt3 = ds1.Tables[1];
                //    dt2 = ds1.Tables[0];
                //    if (dt2.Rows.Count > 0)
                //    {
                //        Error_1.Text = "Member already exists in App#:  : " + dt2.Rows[0]["AppNo"].ToString() +"("+  MainMenuAgency + "-" + MainMenuDept + "-" + MainMenuProgram + "  " + MainMenuYear + ")";
                //         Error_1.Visible = true; Can_Drag = false;
                //    }
                //    else
                //    {
                //        if (AddPriv)
                //            BtnDragApp.Visible = true; Error_1.Visible = false;
                //    }
                //}

                if (Can_Drag && TopGrid.RowCount > 0)
                    BtnDragApp.Visible = true;
                else
                    BtnDragApp.Visible = false;

                if (BtnType == "View" && TopGrid.RowCount > 0)
                {
                    BtnDragApp.Visible = false;
                    string Sel_Agency = TopGrid.SelectedRows[0].Cells["Hie"].Value.ToString();
                    if (!string.IsNullOrEmpty(Sel_Agency.Trim()))
                    {
                        if (BaseForm.BaseAgency == Sel_Agency.Substring(0, 2))
                            BtnSelApp.Visible = true;
                        else
                            BtnSelApp.Visible = false;
                    }
                }
            }

        }

        private void GetDup_APP_MEM_Status_In_APP_Change()
        {
            bool Can_Drag = true; Error_1.Visible = false;
            string strSSN = BottomGrid.CurrentRow.Cells["Btm_SSN"].Value.ToString();
            string strClientID = BottomGrid.CurrentRow.Cells["gvBClientID"].Value.ToString(); 
            if (strSSN == "000000000" && ProgramDefinition.SSNReasonFlag.ToUpper().Trim() == "Y")
            {
                Can_Drag = true;
            }
            else
            {
                DataSet ds1 = Captain.DatabaseLayer.MainMenu.MainMenuOtherPrograms(BottomGrid.CurrentRow.Cells["Btm_SSN"].Value.ToString(),
                                                                                   BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, "AddApplicant", BottomGrid.CurrentRow.Cells["gvFname"].Value.ToString(), null, BottomGrid.CurrentRow.Cells["DOB1"].Value.ToString(), strClientID);
                DataTable dt2, dt3;
                if (ds1.Tables.Count > 0)
                {
                    dt2 = ds1.Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Rows[0]["APP_MEM"].ToString() == "A")
                            Error_1.Text = "Applicant already exists in  " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   Cannot Copy/Drag";
                        else
                            Error_1.Text = "Member already exists in " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim() + "  App# " + dt2.Rows[0]["AppNo"].ToString() + "   Cannot Copy/Drag";

                        this.Error_1.ForeColor = System.Drawing.Color.Black;
                        Error_1.Visible = true; Can_Drag = false;
                    }
                    else
                    {
                        if (AddPriv)
                            BtnDragApp.Visible = true;
                        Error_1.Visible = false;
                    }
                }
            }
            if (Can_Drag)
            {
                this.Error_1.ForeColor = System.Drawing.Color.Black;
                Error_1.Text = "the drag button will copy the customer into the " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear.Trim();
                Error_1.Visible = true;
            }


            //if (ds1.Tables.Count > 1 && Can_Drag)
            //{
            //    dt3 = ds1.Tables[1];
            //    dt2 = ds1.Tables[0];
            //    if (dt2.Rows.Count > 0)
            //    {
            //        Error_1.Text = "Member already exists in App#:  : " + dt2.Rows[0]["AppNo"].ToString() +"("+  MainMenuAgency + "-" + MainMenuDept + "-" + MainMenuProgram + "  " + MainMenuYear + ")";
            //         Error_1.Visible = true; Can_Drag = false;
            //    }
            //    else
            //    {
            //        if (AddPriv)
            //            BtnDragApp.Visible = true; Error_1.Visible = false;
            //    }
            //}

            if (Can_Drag && TopGrid.RowCount > 0)
            {
                //if(Lbl_No_Access.Visible.ToString()=="True") BtnDragApp.Visible = false;
                //else 
                    BtnDragApp.Visible = true;
            }
            else
            {
                BtnDragApp.Visible = false;
            }

        }

        private void TopGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (TopGrid.Rows.Count > 0)
            {
                GetHie_App_Details();
                GetDup_APP_MEM_Status_In_APP_Change();
            }
        }


        string New_Dragged_App_No = string.Empty;
        bool boolmemberswitch = false;
        private void BtnDragApp_Click(object sender, EventArgs e)
        {
            string App_Name = string.Empty;
            boolmemberswitch = false;
            foreach (DataGridViewRow dr in BottomGrid.Rows)
            {
                if (dr.Cells["Client_Sel"].Value.ToString() == true.ToString())
                {
                    App_Name = dr.Cells["Name1"].Value.ToString();
                    if (dr.Cells["AppSwitch"].Value.ToString().ToUpper().TrimStart().Trim() != "A")
                        boolmemberswitch = true;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(App_Name.Trim()))
            {
                MessageBox.Show("You are about to Drag :" + App_Name + " as the Applicant \n into : " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear +
                                 "  From : " + TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString().Substring(0, 2) + "-" + TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString().Substring(2, 2) + "-" +
                                 TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString().Substring(4, 2) + TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString().Substring(6, 12)
                                 , Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Drag_APP_Confirmation);

            }
            else
                MessageBox.Show("Please Select Applicant to Drag", "CAP Systems");

            //bool Save_Result = false;
            //New_Dragged_App_No = string.Empty;

            //if (TopGrid.Rows.Count > 0)
            //{
            //    string Member_seq = null,App_Mem_Sw = "APP";

            //    Member_seq = TopGrid.CurrentRow.Cells["SnpFamSeq"].Value.ToString();
            //    if ((TopGrid.CurrentRow.Cells["AppNo"].Value.ToString()).Substring(10,1) == "M" )
            //        App_Mem_Sw = "MEM";

            //    Save_Result = Captain.DatabaseLayer.MainMenu.DragApp_HouseHold(TopGrid.CurrentRow.Cells["SnpKey"].Value.ToString(), MainMenuAgency + MainMenuDept + MainMenuProgram + MainMenuYear, BaseForm.UserID, App_Mem_Sw, Member_seq, out New_Dragged_App_No);
            //    if (Save_Result)
            //    {
            //        this.DialogResult = DialogResult.OK;
            //        this.Close();
            //        MessageBox.Show("Applicant/Member Dragged Successfully", "CAP Systems", MessageBoxButtons.OK);
            //    }
            //    else
            //        MessageBox.Show("Applicant/Member Drag is not UnSuccessful", "CAP Systems", MessageBoxButtons.OK);
            //}
        }



        bool App_Selection_Changed = false;
        string Drag_Member_seq = "";
        private void Drag_APP_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                if (boolmemberswitch)
                {

                    //MessageBoxManager.Cancel = "Cancel";                   
                    //MessageBoxManager.Yes = "Create New Family ID";
                    //MessageBoxManager.No = "Keep Existing Family ID";
                    //MessageBoxManager.Register();

                    MessageBox.Show("You have selected a household member to be the primary applicant of a new intake. \n If this is the same household, with a new primary applicant, select  YES.\n If this is a new household, select NO. \n If you are uncertain, please reach out to your system administrator prior to continuing.", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, onclose: Drag_APPFamilyId_Confirmation);
                }
                else
                {
                    DragApplicantData();
                }
            }
        }


        private void DragApplicantData()
        {
            bool Save_Result = false;
            New_Dragged_App_No = string.Empty;

            if (TopGrid.Rows.Count > 0)
            {
                if (TopGrid.SelectedRows[0].Selected)
                {
                    string Member_seq = null, App_Mem_Sw = "APP", Old_Mem_Seq = "";

                    Old_Mem_Seq = Member_seq = TopGrid.SelectedRows[0].Cells["SnpFamSeq"].Value.ToString();

                    foreach (DataGridViewRow dr in BottomGrid.Rows)
                    {
                        if (dr.Cells["Client_Sel"].Value.ToString() == true.ToString())
                        {
                            Drag_Member_seq = Member_seq = dr.Cells["Mem_Seq"].Value.ToString();
                            break;
                        }
                    }

                    if (Member_seq != Old_Mem_Seq)
                        App_Selection_Changed = true;

                    //Mem_Seq
                    if ((TopGrid.SelectedRows[0].Cells["AppNo"].Value.ToString()).Substring(10, 1) == "M")
                        App_Mem_Sw = "MEM";

                    //string Agy_Short_Name = "";
                    //AgencyControlEntity Agcy_Data = new AgencyControlEntity();
                    //Agcy_Data = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
                    //if (Agcy_Data != null)
                    //{
                    //    if (string.IsNullOrEmpty(Agcy_Data.AgyShortName.Trim()))
                    //        Agy_Short_Name = Agcy_Data.AgyShortName.Trim();
                    //}

                    //Save_Result = Captain.DatabaseLayer.MainMenu.DragApp_HouseHold(TopGrid.CurrentRow.Cells["SnpKey"].Value.ToString(), BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, App_Mem_Sw, Member_seq, Sum_RefApp_Key, Agy_Short_Name, out New_Dragged_App_No);
                    Save_Result = Captain.DatabaseLayer.MainMenu.DragApp_HouseHold(TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString(), BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, App_Mem_Sw, Member_seq, Sum_RefApp_Key, BaseForm.BusinessModuleID, out New_Dragged_App_No);
                    if (Save_Result)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        MessageBox.Show("Applicant/Member Dragged Successfully \n With App# : ''" + New_Dragged_App_No.Substring(10, 8) + "''   into Hierarchy : " + New_Dragged_App_No.Substring(0, 10), "CAP Systems", MessageBoxButtons.OK);
                    }
                    else
                        MessageBox.Show("Applicant/Member Drag is not UnSuccessful", "CAP Systems", MessageBoxButtons.OK);
                }
            }
        }

        private void Drag_APPFamilyId_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                if (boolmemberswitch)
                {
                    bool Save_Result = false;
                    New_Dragged_App_No = string.Empty;

                    if (TopGrid.Rows.Count > 0)
                    {
                        if (TopGrid.SelectedRows[0].Selected)
                        {
                            string Member_seq = null, App_Mem_Sw = "APP", Old_Mem_Seq = "";

                            Old_Mem_Seq = Member_seq = TopGrid.SelectedRows[0].Cells["SnpFamSeq"].Value.ToString();

                            foreach (DataGridViewRow dr in BottomGrid.Rows)
                            {
                                if (dr.Cells["Client_Sel"].Value.ToString() == true.ToString())
                                {
                                    Drag_Member_seq = Member_seq = dr.Cells["Mem_Seq"].Value.ToString();
                                    break;
                                }
                            }

                            if (Member_seq != Old_Mem_Seq)
                                App_Selection_Changed = true;

                            //Mem_Seq
                            // if ((TopGrid.CurrentRow.Cells["AppNo"].Value.ToString()).Substring(10, 1) == "M")
                            App_Mem_Sw = "NEW";

                            //string Agy_Short_Name = "";
                            //AgencyControlEntity Agcy_Data = new AgencyControlEntity();
                            //Agcy_Data = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
                            //if (Agcy_Data != null)
                            //{
                            //    if (string.IsNullOrEmpty(Agcy_Data.AgyShortName.Trim()))
                            //        Agy_Short_Name = Agcy_Data.AgyShortName.Trim();
                            //}

                            //Save_Result = Captain.DatabaseLayer.MainMenu.DragApp_HouseHold(TopGrid.CurrentRow.Cells["SnpKey"].Value.ToString(), BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, App_Mem_Sw, Member_seq, Sum_RefApp_Key, Agy_Short_Name, out New_Dragged_App_No);
                            Save_Result = Captain.DatabaseLayer.MainMenu.DragApp_HouseHold(TopGrid.SelectedRows[0].Cells["SnpKey"].Value.ToString(), BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    "), BaseForm.UserID, App_Mem_Sw, Member_seq, Sum_RefApp_Key, BaseForm.BusinessModuleID, out New_Dragged_App_No);
                            if (Save_Result)
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                MessageBox.Show("Applicant/Member Dragged Successfully \n With App# : ''" + New_Dragged_App_No.Substring(10, 8) + "''   into Hierarchy : " + New_Dragged_App_No.Substring(0, 10), "CAP Systems", MessageBoxButtons.OK);
                            }
                            else
                                MessageBox.Show("Applicant/Member Drag is not UnSuccessful", "CAP Systems", MessageBoxButtons.OK);
                        }
                    }
                }

            }
            else if (dialogResult == DialogResult.No)
            {
                DragApplicantData();
            }
        }



        public string Get_Dragged_App_No()
        {
            string New_App_No = string.Empty;

            if (!(string.IsNullOrEmpty(New_Dragged_App_No.Trim())))
                New_App_No = New_Dragged_App_No;

            if (BtnType == "View")
            {
                if (string.IsNullOrEmpty(New_Dragged_App_No))
                {
                    New_App_No = TopGrid.CurrentRow.Cells["Hie"].Value.ToString()
                                       + TopGrid.CurrentRow.Cells["Year"].Value.ToString()
                                       + TopGrid.CurrentRow.Cells["AppNo"].Value.ToString();
                }
            }

            return New_App_No;
        }

        public string Get_Dragged_App_No_Fam_Seq()
        {
            return Drag_Member_seq;
        }


        public bool Get_App_Slection_Change()
        {
            return App_Selection_Changed;
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAddApp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are About to Add Customer in " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear,
                               Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_New_APP_Confirmation);

            //CaseMstEntity caseMSTEntity = new CaseMstEntity();
            //caseMSTEntity.ApplAgency = MainMenuAgency;
            //caseMSTEntity.ApplDept = MainMenuDept;
            //caseMSTEntity.ApplProgram = MainMenuProgram;
            //caseMSTEntity.ApplYr = MainMenuYear;

            //Privileges.Program = "Main Menu";
            //ClientSNPForm clientSNPForm = new ClientSNPForm(BaseForm, true, caseMSTEntity, null, Consts.Common.Add, Privileges,null,string.Empty);
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
                if (DtDOB.Checked)
                    caseSNPEntity.AltBdate = DtDOB.Value.ToShortDateString();

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
                    clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                    clientSNPForm.ShowDialog();
                }
                else
                {
                    Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                    clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                    clientSNPForm.ShowDialog();
                }
            }
        }

        private void On_Applicant_Dragged(object sender, FormClosedEventArgs e)
        {
            New_Dragged_App_No = string.Empty;
            //ClientSNPForm form = sender as ClientSNPForm;
            if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
            {
                Case4001Form form = sender as Case4001Form;
                if (form.DialogResult == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    New_Dragged_App_No = form.GetNew_App_For_Mainmenu();
                    this.Close();
                }
            }
            else
            {
                Case3001Form form = sender as Case3001Form;
                if (form.DialogResult == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    New_Dragged_App_No = form.GetNew_App_For_Mainmenu();
                    this.Close();
                }
            }

        }


        private void GetSelectedProgram()
        {
            if (BaseForm.ContentTabs.TabPages[0].Controls[0] is MainMenuControl)
            {
                MainMenuControl mainMenuControl = (BaseForm.ContentTabs.TabPages[0].Controls[0] as MainMenuControl);
                MainMenuAgency = mainMenuControl.Agency;
                MainMenuDept = mainMenuControl.Dept;
                MainMenuProgram = mainMenuControl.Program;

                MainMenuYear = "    ";
                if (!string.IsNullOrEmpty(mainMenuControl.ProgramYear.Trim()))
                    MainMenuYear = mainMenuControl.ProgramYear;
            }
        }

        private void MtxtSsn_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()))
            {
                string Tmp_SSn = MtxtSsn.Text + "000000000".Substring(0, (9 - MtxtSsn.Text.Length));
                MtxtSsn.Text = Tmp_SSn.Replace(' ', '0');
            }
        }

        private void lblSSNReq_Click(object sender, EventArgs e)
        {

        }

        private void btnRecentData_Click(object sender, EventArgs e)
        {
            //if (ValidationSearch())
            //{
            //    if (!string.IsNullOrEmpty(MtxtSsn.Text.Trim()) ||
            //       !string.IsNullOrEmpty(TxtFName.Text.Trim()) ||
            //       !string.IsNullOrEmpty(TxtLName.Text.Trim()) ||
            //       DtDOB.Checked)
            //    {
            //        if (TopGrid.Rows.Count > 0)
            //        {
            //            string DateofBirth = null;
            //            BtnDragApp.Visible = false;
            //            Error_1.Visible = false;
            //            if (DtDOB.Checked)
            //                DateofBirth = DtDOB.Value.ToShortDateString();
            //            Atleast_Once_Searched = true;

            //            DataSet ds = new DataSet();

            //            string strClientSwitch = string.Empty;
            //            if (propClientRulesSwitch == "Y")
            //            {
            //                if (BaseForm.BaseAgencyControlDetails != null)
            //                    strClientSwitch = BaseForm.BaseAgencyControlDetails.ClientRules;
            //            }

            //                ds = Captain.DatabaseLayer.MainMenu.MainMenuSearchEMS("APP", "ALL", null, null, Fill_Only_APP, TxtLName.Text, TxtFName.Text, MtxtSsn.Text, null, null, null, null, null, null, null, Fill_Only_Hie, DateofBirth, BaseForm.UserID,"Single",string.Empty,string.Empty);

            //            if(ds.Tables[0].Rows.Count>0)
            //            { 
            //                PIPUpdateApplicantForm pipupdateForm = new Forms.PIPUpdateApplicantForm(BaseForm, ds.Tables[0].Rows[0]["Agency"].ToString(), ds.Tables[0].Rows[0]["Dept"].ToString(), ds.Tables[0].Rows[0]["Prog"].ToString(), ds.Tables[0].Rows[0]["SnpYear"].ToString(), ds.Tables[0].Rows[0]["APPLICANTNO"].ToString());
            //                //pipupdateForm.FormClosed += new FormClosedEventHandler(PipupdateForm_FormClosed);
            //                pipupdateForm.ShowDialog();
            //            }

            //        }
            //    }
            //    else
            //        MessageBox.Show("Please Fill Search Criteria and Proceed", "CAP Systems");
            //}
        }

        bool Can_Set_App = false;
        private void BtnSelApp_Click(object sender, EventArgs e)
        {
            Can_Set_App = true;
            try //changed sudheer 12/13/2016
            {
                // if (sender == Btn_Curr_Hie)
                //    New_Dragged_App_No = "Set_Curr_Hie";
                // else
                // {
                if (TopGrid.Rows.Count > 0 && string.IsNullOrEmpty(New_Dragged_App_No.Trim()))
                {
                    if (TopGrid.CurrentRow.Cells["Hie"].Value.ToString() + TopGrid.CurrentRow.Cells["Year"].Value.ToString() != (BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ")))
                    {
                        MessageBox.Show("Selected client is not in the currently selected desktop program '" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + " " + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ") + "' \n Would you like to select the desktop program : "
                                        + TopGrid.CurrentRow.Cells["Hie"].Value.ToString() + " " + TopGrid.CurrentRow.Cells["Year"].Value.ToString(),
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

        private void Set_APP_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BottomGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (BottomGrid.Rows.Count > 0)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                ColIdx = BottomGrid.CurrentCell.ColumnIndex;
                RowIdx = BottomGrid.CurrentCell.RowIndex;

                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    foreach (DataGridViewRow dr in BottomGrid.Rows)
                    {
                        if (dr.Cells["Mem_Seq"].Value.ToString() != BottomGrid.CurrentRow.Cells["Mem_Seq"].Value.ToString())
                            dr.Cells["Client_Sel"].Value = false;
                    }

                    GetDup_APP_MEM_Status_In_APP_Change();
                }
            }
        }

        private void BottomGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void MainMenuAddApplicantForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps("COPYDRAG", 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
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