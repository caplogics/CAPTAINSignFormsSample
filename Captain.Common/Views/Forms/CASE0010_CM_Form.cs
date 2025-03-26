/************************************************************************
 * Conversion On    :   12/15/2022      * Converted By     :   Kranthi
 * Modified On      :   12/15/2022      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
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
using System.Drawing;
using Wisej.Web;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0010_CM_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        /***************************************
         * This method is for **PI_Panle** panel
         ***************************************/
        public CASE0010_CM_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, List<CaseEnrlEntity> pass_enroll_list, string sel_hie, string sel_prog_name, string ClasS)
        {
            InitializeComponent();
            propComboType = string.Empty;
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = mode;
            Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
            Sel_Prog_Name = sel_prog_name.Trim();
            Pass_Enroll_List = pass_enroll_list;
            Pass_Enroll_Entity = new CaseEnrlEntity();
            Module_Code = BaseForm.BusinessModuleID;
            Sel_Site = Sel_Room = Sel_AMPM = string.Empty;

            string Hierarchy = (sel_hie.Substring(0, 2) + "-" + sel_hie.Substring(2, 2) + "-" + sel_hie.Substring(4, 2));
            SUM_Exp_App.HeaderText += Hierarchy;

            this.Text = Privileges.PrivilegeName + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;



            DepEnrollList = _model.HierarchyAndPrograms.GetDepEntollHierachies(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
            Get_ClientIntake_Priv();
            Fill_Sort_Combo();
            Fill_Status_Combo();
            Get_HIE_Program_Names();

            //kranthi//
            //CaseSum_Panel.Size = new System.Drawing.Size(677, 265);
            //CaseSumGrid.Size = new System.Drawing.Size(669, 260);
            //this.Size = new System.Drawing.Size(679, 455);

            /**********************************************************************/
            NoStatus_Panel.Visible = CM_NoStatus_Panel.Visible = Enrl_Notes_Panel.Visible = false;
            Case_RB_Panel.Visible = true; //= pnlHeader.Visible = true;
            this.Size = new Size(this.Width, this.Height - (NoStatus_Panel.Height + CM_NoStatus_Panel.Height + Enrl_Notes_Panel.Height + pnlHeader.Height + CaseMst_Panel.Height));
            PI_Panle.Dock = DockStyle.Fill;
            Tools["Pb_Add_Help"].Visible = true;
            /***********************************************************************/

            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));

            // **     Pending, WaitList   **   But asked to Add all om 06/19/2014
            if (!ClasS.Contains("****"))
            {
                listItem.Add(new ListItem("Denied", "R"));
                // Newly added jan 11 2019
                listItem.Add(new ListItem("Deferred", "F"));

                listItem.Add(new ListItem("Enrolled", "E"));
                listItem.Add(new ListItem("Inactive", "N"));
                listItem.Add(new ListItem("Pending", "P"));
                if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")//  this logic value modified 10/06/2018 ask to customer
                {
                    listItem.Add(new ListItem("Postintake", "I"));
                    // Added by Sudheer on 08/20/2020 as per customer request document MVCAA
                    // Added by Sudheer on 02/21/2025 as per customer request document ProAction WiseJ (ENROLLMENT/WITHDRAWAL MISSING STATUS OPTION 2/20/2025)
                    if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "MVCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PROACT")
                        listItem.Add(new ListItem("Accepted", "C"));
                }
                else
                    listItem.Add(new ListItem("Accepted", "C"));
            }

            listItem.Add(new ListItem("Wait List", "L"));
            if (!ClasS.Contains("****"))
            {
                listItem.Add(new ListItem("Withdrawn", "W"));
            }

            Cmb_Post_Status.Items.AddRange(listItem.ToArray());
            SetComboBoxValue(Cmb_Post_Status, "0");
            if(Privileges.ModuleCode == "02")
                this.Text = "Create Status Records from SIM Referrals";
            else
                this.Text = "Create Status Record(s)";
            Btn_PostIntake.Text = "&Create Status Record(s)";

            //if (!ClasS.Contains("****"))
            //{
            //    SetComboBoxValue(Cmb_Post_Status, "I");
            //    LblHeader.Text = Btn_PostIntake.Text = "Create PostIntake Record";
            //}
            //else
            //{
            //    SetComboBoxValue(Cmb_Post_Status, "L");
            //    LblHeader.Text = Btn_PostIntake.Text = "Create Wait List Record";
            //}

            if (Module_Code == "03")
            {
                Rb_From_Sum.Text += "'" + Sel_Prog_Name + "' " + Hierarchy; pnlHeader.Visible = false;
                CaseSum_Panel.Dock = DockStyle.Fill;
                CaseSum_Panel.BringToFront();
            }
            else
            {
                string[] Class = Regex.Split(ClasS.ToString(), "/");
                Sel_Site = Class[0]; Sel_Room = Class[1]; Sel_AMPM = Class[2];
                Lbl_Site_Code.Text = ClasS;

                Sel_Prog_Name = Get_Program_Desc(sel_hie.Substring(0, 2), sel_hie.Substring(2, 2), sel_hie.Substring(4, 2));
                pnlHeader.Visible = false; 
                Site_Det_Panel.Visible = Fund_Panel2.Visible = Fund_Panel.Visible = true;
                this.Size = new Size(this.Width, this.Height); //+ pnlHeader.Height);
                Fill_Fund_Combo(); PI_Panle.Dock = DockStyle.Fill;
                CaseSum_Panel.Dock = DockStyle.Fill;
                CaseSum_Panel.BringToFront();
                Rb_From_Sum.Text += "'" + Sel_Prog_Name + "' " + Hierarchy;

                //Fund_Panel2.Location = new System.Drawing.Point(0, 330);
                //Fund_Panel2.Size = new System.Drawing.Size(671, 24);
            }
            Fill_Non_Enroll_Grid();
        }

        /***********************************************************
        * This method is for ADD mode for 'NoStatus_Panel' panel
        ************************************************************/

        public CASE0010_CM_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, List<CaseEnrlEntity> pass_enroll_list, string sel_hie, string sel_prog_name, string ClasS, string strComboType)
        {
            InitializeComponent();

            try
            {
                propComboType = strComboType;
                BaseForm = baseForm;
                Privileges = privileges;
                Mode = mode;
                Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
                Sel_Prog_Name = sel_prog_name.Trim();
                Pass_Enroll_List = pass_enroll_list;
                Pass_Enroll_Entity = new CaseEnrlEntity();
                Module_Code = BaseForm.BusinessModuleID;
                Sel_Site = Sel_Room = Sel_AMPM = string.Empty;

                string Hierarchy = (sel_hie.Substring(0, 2) + "-" + sel_hie.Substring(2, 2) + "-" + sel_hie.Substring(4, 2));
                SUM_Exp_App.HeaderText += Hierarchy;

                PI_Panle.Visible = CM_NoStatus_Panel.Visible = false;
                this.Text = "Add Status Record(s)";//this.Text = Privileges.PrivilegeName + " - " + Mode;
                _model = new CaptainModel();
                _errorProvider = new ErrorProvider(this);
                _errorProvider.BlinkRate = 1;
                _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
                _errorProvider.Icon = null;

                //**NoStatus_Panel.Location = new System.Drawing.Point(1, 1);
                this.Size = new System.Drawing.Size(Width, 554);//(Width, 348);
                string[] Class = Regex.Split(ClasS.ToString(), "/");
                Txt_Site.Text = Sel_Site = Class[0]; Txt_Room.Text = Sel_Room = Class[1]; Txt_AMPM.Text = Sel_AMPM = Class[2];

                switch (Class[2])
                {
                    case "A": Txt_AMPM.Text = "A - AM Class"; break;
                    case "P": Txt_AMPM.Text = "P - PM Class"; break;
                    case "E": Txt_AMPM.Text = "E - Extended Day"; break;
                    case "F": Txt_AMPM.Text = "F - Full Day"; break;
                }

                Lbl_Site_Code.Text = ClasS;

                //  **   Enroll, Pending, Wait List **     Asked to Add All Status on 06/19/2014
                //  Asked to Have Enroll, Postintake, Wait List Only on 11/10/2014 in Enrollment Withdrawal Review Doc
                List<ListItem> listItem = new List<ListItem>();
                listItem.Add(new ListItem("  ", "0"));
                //listItem.Add(new ListItem("Denied", "R"));
                listItem.Add(new ListItem("Enroll", "E"));
                //listItem.Add(new ListItem("Inactive", "N"));
                //listItem.Add(new ListItem("Pending", "P"));
                if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")
                {//  this logic value modified 10/06/2018 ask to customer
                    listItem.Add(new ListItem("Postintake", "I"));
                    // Added by Sudheer on 08/20/2020 as per customer request document MVCAA
                    // Added by Sudheer on 02/21/2025 as per customer request document ProAction WiseJ (ENROLLMENT/WITHDRAWAL MISSING STATUS OPTION 2/20/2025)
                    if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "MVCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PROACT") 
                        listItem.Add(new ListItem("Accepted", "C"));
                }
                else
                {
                    listItem.Add(new ListItem("Accepted", "C"));
                }
                listItem.Add(new ListItem("Parent declined", "A"));
                listItem.Add(new ListItem("No Longer Interested", "B"));
                listItem.Add(new ListItem("Wait List", "L"));
                // Newly added jan 11 2019
                listItem.Add(new ListItem("Deferred", "F"));
                //listItem.Add(new ListItem("Withdrawn", "W"));

                Cmb_NoStat_Fund.Items.AddRange(listItem.ToArray());
                Cmb_NoStat_Fund.SelectedIndex = 0;

                Fill_Fund_Category();
                this.Cmb_Add_Fund.SelectedIndexChanged -= new System.EventHandler(this.Cmb_Add_Fund_SelectedIndexChanged);
                Fill_ADD_Fund_Combo();
                this.Cmb_Add_Fund.SelectedIndexChanged += new System.EventHandler(this.Cmb_Add_Fund_SelectedIndexChanged);

                if (Privileges.ModuleCode == "02")
                    Btn_NoStatus_Add.Visible = false;
                else
                    Btn_NoStatus_Add.Visible = true;
            }
            catch (Exception Exception)
            {
                AlertBox.Show(Exception.Message, MessageBoxIcon.Error);
            }
        }

        /***********************************************************
        * This method is for ADD mode for 'CM_NoStatus_Panel' panel
        ************************************************************/
        public CASE0010_CM_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, List<CaseEnrlEntity> pass_enroll_list, string sel_hie, string sel_prog_name)
        {
            InitializeComponent();
            propComboType = string.Empty;
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = mode;
            Sel_Hie = sel_hie;
            //Sel_Prog_Name = sel_prog_name.Trim();
            Pass_Enroll_List = pass_enroll_list;
            Pass_Enroll_Entity = new CaseEnrlEntity();
            Module_Code = BaseForm.BusinessModuleID;
            Sel_Site = Sel_Room = Sel_AMPM = string.Empty;

            this.Text = Privileges.PrivilegeName + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            /**************************************************************************************************************************************************/
            PI_Panle.Visible = NoStatus_Panel.Visible = Enrl_Notes_Panel.Visible = false;
            CM_NoStatus_Panel.Visible = true;
            // CM_NoStatus_Panel.Location = new System.Drawing.Point(1, 1);
            this.Size = new Size((this.Width - this.pnlTab2.Width), this.Height - (NoStatus_Panel.Height + PI_Panle.Height + Enrl_Notes_Panel.Height));
            Tools["pb_Add_help2"].Visible = true;
            /***********************************************************************************************************************************************************/

            Txt_Hie_DEsc.Text = sel_prog_name;

            //  Asked to Have Enroll, Postintake, Wait List Only on 11/10/2014 in Enrollment Withdrawal Review Doc
            //  Asked to Have All Statu(S) Options on 12/12/2014 in CM ( Dan Enhancement)
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Denied", "R"));
            // Newly added jan 11 2019
            listItem.Add(new ListItem("Deferred", "F"));

            listItem.Add(new ListItem("Enroll", "E"));
            listItem.Add(new ListItem("Inactive", "N"));
            listItem.Add(new ListItem("Pending", "P"));
            //listItem.Add(new ListItem("Parent declined", "A"));
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")//  this logic value modified 10/06/2018 ask to customer
            {
                listItem.Add(new ListItem("Postintake", "I"));
                // Added by Sudheer on 08/20/2020 as per customer request document MVCAA
                // Added by Sudheer on 02/21/2025 as per customer request document ProAction WiseJ (ENROLLMENT/WITHDRAWAL MISSING STATUS OPTION 2/20/2025)
                if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "MVCAA" || BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PROACT")
                    listItem.Add(new ListItem("Accepted", "C"));
            }
            else
            {
                listItem.Add(new ListItem("Accepted", "C"));
            }
            listItem.Add(new ListItem("Wait List", "L"));
            listItem.Add(new ListItem("Withdraw", "W"));
            Cmb_CM_NoStat_Status.Items.AddRange(listItem.ToArray());
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "OCO")// 09/Jan/2020 jen asked OCO customer changed default value Accepted  to Enroll
                Cmb_CM_NoStat_Status.SelectedIndex = 2;
            else
                Cmb_CM_NoStat_Status.SelectedIndex = 5;
        }

        public CASE0010_CM_Form(string Program, string Note1, string Note2)
        {
            InitializeComponent();
            propComboType = string.Empty;
            this.Text = "Enroll Notes";
            PI_Panle.Visible = NoStatus_Panel.Visible = CM_NoStatus_Panel.Visible = false;
            //Enrl_Notes_Panel.Location = new System.Drawing.Point(1, 1);
            /**************************************************************************************************************************************************/
            Enrl_Notes_Panel.Visible = true;
            this.Size = new Size((575), this.Height - (NoStatus_Panel.Height + PI_Panle.Height + CM_NoStatus_Panel.Height));
            /**************************************************************************************************************************************************/
            // this.Size = new System.Drawing.Size(521, 66);
            Txt_Enrl_Note1.Text = Note1;
            Txt_Enrl_Note2.Text = Note2;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Module_Code { get; set; }

        public string Class { get; set; }

        public string Sel_Hie { get; set; }

        public string Sel_Prog_Name { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<CaseEnrlEntity> Pass_Enroll_List { get; set; }

        public CaseEnrlEntity Pass_Enroll_Entity { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        public string propComboType { get; set; }

        #endregion

        string Img_Blank = Consts.Icons.ico_Blank;  // new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        string Img_Tick = Consts.Icons.ico_Tick;    //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");


        private void Fill_Sort_Combo()
        {
            List<HierarchyEntity> hierarchyClientName = _model.lookupDataAccess.GetClientNameFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyClientName)
            {
                Cmb_Sort.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));
            }
            CommonFunctions.SetComboBoxValue(Cmb_Sort, BaseForm.BaseHierarchyCnFormat.ToString());
        }

        List<CommonEntity> Status_List = new List<CommonEntity>();
        private void Fill_Status_Combo()
        {
            Status_List = _model.lookupDataAccess.GetREFERRALSTATUS();
            foreach (CommonEntity Entity in Status_List)
                Cmb_Status.Items.Add(new ListItem(Entity.Desc, Entity.Code));

            Cmb_Status.Items.Insert(0, new ListItem("No Referral Status", ""));
            Cmb_Status.Items.Insert(1, new ListItem("All Statuses", "0"));
            Cmb_Status.SelectedIndex = 0;
        }

        private string Get_Status_Desc(string Status_Code)
        {
            string Status_Desc = string.Empty;

            foreach (CommonEntity Entity in Status_List)
            {
                if (Entity.Code == Status_Code)
                {
                    Status_Desc = Entity.Desc;
                    break;
                }
            }
            return Status_Desc;
        }

        string Comm_Sel_Fund = string.Empty;
        private void Fill_Fund_Combo()
        {
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
            Cmb_Fund.Items.Clear();
            Cmb_Fund2.Items.Clear();
            foreach (SPCommonEntity Entity in FundingList)
            {
                if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                {
                    Cmb_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                    Cmb_Fund2.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                }
            }

            if (Cmb_Fund.Items.Count > 0)
            {
                Cmb_Fund.SelectedIndex = 0;
                Cmb_Fund2.SelectedIndex = 0;
                Comm_Sel_Fund = ((ListItem)Cmb_Fund2.SelectedItem).Value.ToString();
            }
        }

        private void Fill_ADD_Fund_Combo()
        {
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
            Cmb_Add_Fund.Items.Clear();

            Cmb_Add_Fund.Items.Add(new ListItem(" ", "00", "Y", Color.Green));
            foreach (SPCommonEntity Entity in FundingList)
            {
                if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                {
                    Cmb_Add_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                }
            }

            if (Cmb_Add_Fund.Items.Count > 0)
            {
                Cmb_Add_Fund.SelectedIndex = 0;
                Comm_Sel_Fund = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
            }

            fillApplicantsGrid();
        }
        bool EnableWithDrawn = false;
        private void fillApplicantsGrid()
        {
            this.Column0.Visible = false;
            if (Module_Code == "02")
            {
                this.GD_Program.HeaderText = "Site/Room/AMPM";
                this.GD_Status_Date.Visible = false;
                this.GD_Fund.Visible = true;
                this.GD_Name.Width = 300;//140;
                this.GD_Program.Width = 150;//112;

                this.Column0.Visible = true;
            }

            string TmpName = " ", Attn_Date = " ", Attn_Min_Date = " ", Status_Date = " ";
            bool From_Dept_WaitList = false;
            int Tmp_Rows_Cnt = 0;
            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            {
                TmpName = LookupDataAccess.GetMemberName(Entity.Snp_F_Name, Entity.Snp_M_Name, Entity.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());

                Attn_Date = Attn_Min_Date = Status_Date = " ";
                if (!string.IsNullOrEmpty(Entity.Enrl_Max_Attn_Date.Trim()))
                    Attn_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.Enrl_Max_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                if (!string.IsNullOrEmpty(Entity.Enrl_Min_Attn_Date.Trim()))
                    Attn_Min_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.Enrl_Min_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                if (!string.IsNullOrEmpty(Entity.Status_Date.Trim()))
                    Status_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.Status_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                string Status = string.Empty;
                if (Module_Code == "03")
                    Grid_Applications.Rows.Add(true, Entity.Agy + "-" + Entity.Dept + "-" + Entity.Prog, " ", Entity.App, TmpName, (!string.IsNullOrEmpty(Entity.Status_Date.Trim()) ? LookupDataAccess.Getdate(Entity.Status_Date) : " "), "", " ", " ", " ", Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App, Entity.ID);
                else
                {
                    this.GD_Date.Width = 150;
                    this.dataGridViewTextBoxColumn3.Width = 110;

                    switch (Entity.Status.Trim())
                    {
                        case "E":
                            Status = "Enrolled";
                            break;
                        case "R":
                            Status = "Denied";
                            break;
                        case "X":
                            Status = "Exited";
                            break;
                        case "N":
                            Status = "Inactive";
                            break;
                        case "P":
                            Status = "Pending";
                            break;
                        case "L":
                            Status = "Wait List";
                            break;
                        case "W":
                            Status = "Withdrawn";
                            break;
                        case "A":
                            Status = "Parent declined";
                            break;
                        case "B":
                            Status = "No Longer Interested";
                            break;
                        case "C":
                            Status = "Accepted";
                            break;
                    }
                    From_Dept_WaitList = (!string.IsNullOrEmpty(Entity.Site.Trim()) ? false : true);
                    string AMPM = string.Empty;
                    if (!string.IsNullOrEmpty(Txt_AMPM.Text.Trim())) AMPM = Txt_AMPM.Text.Trim().Substring(0, 1);

                    Grid_Applications.Rows.Add(true, (From_Dept_WaitList ? Txt_Site.Text.Trim()/*Entity.Mst_Site*/ : Entity.Site) + "/" + (From_Dept_WaitList ? Txt_Room.Text.Trim() : Entity.Room) + "/" + (From_Dept_WaitList ? AMPM : Entity.AMPM), Entity.FundHie, Entity.Mst_App, TmpName, Status_Date, Status, " ", Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App, Entity.ID);
                    if (Entity.Status == "W")
                        EnableWithDrawn = true;
                    else
                        EnableWithDrawn = false;
                }
                Tmp_Rows_Cnt++;
            }
        }

        List<DepEnrollHierachiesEntity> DepEnrollList = new List<DepEnrollHierachiesEntity>();
        List<CaseHierarchyEntity> CaseHie_List = new List<CaseHierarchyEntity>();
        private void Get_HIE_Program_Names()
        {
            CaseHie_List = _model.AdhocData.Browse_CASEHIE("**", "**", "**", BaseForm.UserID, BaseForm.BaseAdminAgency);
        }

        private string Get_Program_Desc(string Agy, string Dept, string Prog)
        {
            string Prog_Desc = string.Empty;
            foreach (CaseHierarchyEntity Entity in CaseHie_List)
            {
                if (Entity.Agency == Agy && Entity.Dept == Dept && Entity.Prog == Prog)
                {
                    Prog_Desc = Entity.HierarchyName.Trim();
                    break;
                }
            }

            return Prog_Desc;
        }


        List<CaseSumEntity> CaseSum_List = new List<CaseSumEntity>();
        List<MainMenuEntity> Base_MainMenu_List = new List<MainMenuEntity>();
        List<MainMenuEntity> Base99_MainMenu_List = new List<MainMenuEntity>();
        private void Fill_Non_Enroll_Grid()
        {
            Base_MainMenu_List = _model.MainMenuData.GetMainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null,
                BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), null, BaseForm.UserID, "");

            //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
            //    Rb_From_Mst.Visible = true;

            if (Module_Code == "03")
                Fill_NonEnrolls_From_CASESUM();
            else
                Fill_HSS_NonEnrolls_From_CASESUM();
        }


        List<CaseEnrlEntity> Enrl_BaseHie_App_List = new List<CaseEnrlEntity>();
        private void Get_ENRL_BaseHie_Apps()
        {
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Join_Mst_Snp = "N";
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            Search_Entity.Year = BaseForm.BaseYear;
            //Search_Entity.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
            Search_Entity.Enrl_Status_Not_Equalto = "T";
            Search_Entity.Rec_Type = "C";
            Search_Entity.FundHie = (Sel_Hie.Substring(0, 2) + Sel_Hie.Substring(2, 2) + Sel_Hie.Substring(4, 2));

            Enrl_BaseHie_App_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");
            ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (programEntity != null)
            {
                ProgramDefinition = programEntity;
            }

        }

        int CaseSum_Grid_Rows_Cnt = 0;
        List<MainMenuEntity> App_Reff_From_CASESUM = new List<MainMenuEntity>();
        List<Sum_Referral_Entity> Tmp_Referral_List = new List<Sum_Referral_Entity>();
        List<Sum_Referral_Entity> Referral_List_To_Fill = new List<Sum_Referral_Entity>();
        List<CaseSumEntity> Tmp_Referral_NotInMST_List = new List<CaseSumEntity>();
        List<MainMenuEntity> Base_MainMenu_NotEnroll_List = new List<MainMenuEntity>();
        private void Fill_NonEnrolls_From_CASESUM()
        {
            //CaseSum_Panel.Size = new System.Drawing.Size(677, 265);
            //CaseSumGrid.Size = new System.Drawing.Size(669, 260);

            string TmpName = " ";
            int rowIndex = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();
            Base_MainMenu_NotEnroll_List.Clear();

            if (Privileges.ModuleCode == "03" && Tmp_Referral_List.Count == 0)
            {
                List<CaseSumEntity> Tmp_CaseSum_List = new List<CaseSumEntity>();
                CaseSumEntity Search_Entity = new CaseSumEntity(true);


                //Search_Entity.CaseSumAgency = Search_Entity.CaseSumDept = Search_Entity.CaseSumProgram = "99";
                //Search_Entity.CaseSumYear = "    ";

                Search_Entity.CaseSumRefHierachy = Sel_Hie.Substring(0, 2) + Sel_Hie.Substring(2, 2) + Sel_Hie.Substring(4, 2);
                //Search_Entity.CaseSumRefDept = Sel_Hie.Substring(2, 2);
                //Search_Entity.CaseSumRefProgram = Sel_Hie.Substring(4, 2);

                //Search_Entity.CaseSumRefYear = Sel_Hie.Trim().Length > 6 ? Sel_Hie.Substring(6, 4) : "    ";

                //Tmp_CaseSum_List = _model.EnrollData.Browse_CASESUM(Search_Entity, "Browse");


                //Commented on 2/18/2016 Need to Implement New Logic
                Get_ENRL_BaseHie_Apps();
                List<SqlParameter> sqlParamList = _model.EnrollData.Prepare_MST_SUM_NonEnrollSqlParameters_List(Search_Entity, "Browse", BaseForm.BaseAgency
                                                                , BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_CaseSum_RefApps_On_MSTSNP]");




                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    //DataView dv = new DataView(CASEENRLData.Tables[0]);
                    //dv.Sort = "SNP_NAME_IX_FI , SNP_NAME_IX_MI, SNP_NAME_IX_LAST";

                    if (((ListItem)Cmb_Sort.SelectedItem).Value.ToString() == "1") //04072014
                        CASEENRLData.Tables[0].DefaultView.Sort = "SNP_NAME_IX_FI , SNP_NAME_IX_MI, SNP_NAME_IX_LAST";
                    else
                        CASEENRLData.Tables[0].DefaultView.Sort = "SNP_NAME_IX_LAST, SNP_NAME_IX_FI, SNP_NAME_IX_MI";

                    DataTable Referral_Table = CASEENRLData.Tables[0].DefaultView.ToTable();

                    //foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                    foreach (DataRow row in Referral_Table.Rows)
                        Tmp_Referral_List.Add(new Sum_Referral_Entity(row));
                }

                if (CASEENRLData.Tables[1].Rows.Count > 0)
                {
                    // Commented on 2/18/2016 Need to Implement New Logic
                    DataView dv = CASEENRLData.Tables[1].DefaultView;
                    dv.Sort = "MST_APP_NO desc";
                    DataTable sortedDT = dv.ToTable();

                    if (CASEENRLData != null && sortedDT.Rows.Count > 0)
                    {
                        foreach (DataRow row in sortedDT.Rows)
                            Base_MainMenu_NotEnroll_List.Add(new MainMenuEntity(row, "Original"));
                    }

                    //if (CASEENRLData != null && CASEENRLData.Tables[1].Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in CASEENRLData.Tables[1].Rows)
                    //        Base_MainMenu_NotEnroll_List.Add(new MainMenuEntity(row, "Original"));
                    //}
                }

                if (CASEENRLData != null && CASEENRLData.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[2].Rows)
                        Tmp_Referral_NotInMST_List.Add(new CaseSumEntity(row, string.Empty));
                }
            }


            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = true;
            //this.LblHeader.Text = Btn_PostIntake.Text = "Send to Post Intake"; 
            GD_PostIntake.Rows.Clear();
            if (Rb_From_Sum.Checked)
            {
                //this.LblHeader.Text = Btn_PostIntake.Text = "Create PostIntake Record";
                this.PI_Prog.HeaderText = "Exp Enroll App#";
                this.PI_Name.Width = 124;//132;
                this.PI_Prog.Width = 120;
                this.Post_Img.Visible = true;

                string Ref_Date = string.Empty;
                Referral_List_To_Fill.Clear();
                foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
                {
                    Ref_Date = " ";
                    if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
                        Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    Compare_SSN_List.Clear();

                    Ent.Ssn = ("000000000".Substring(0, (9 - Ent.Ssn.Length)) + Ent.Ssn);

                    if (Ent.Ssn.Substring(3, 2) == "00")
                        Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.DOB.Equals(Ent.Snp_DOB) && u.Fname.Equals(Ent.Snp_F_Name) && u.Lname.Equals(Ent.Snp_L_Name)); //Pseudo SSN # Logic
                    else
                        Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn));

                    if (Compare_SSN_List.Count > 0)
                    {
                        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                        CaseSum_Grid_Rows_Cnt++;
                        Ent.ID = CaseSum_Grid_Rows_Cnt.ToString();
                        Referral_List_To_Fill.Add(new Sum_Referral_Entity(Ent, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog)));
                    }
                    else
                    {
                        if (Compare_SSN_List.Count == 0)
                        {
                            TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());

                            CaseSum_Grid_Rows_Cnt++;
                            Ent.ID = CaseSum_Grid_Rows_Cnt.ToString();
                            Referral_List_To_Fill.Add(new Sum_Referral_Entity(Ent, string.Empty, Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog)));
                            //CaseSumGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }

            Fill_CaseSum_Grid();
            //else
            //{
            //    if (Rb_From_Sum_NotInMst.Checked)
            //    {
            //        TmpName = " "; string Ref_Date = " ";

            //        this.LblHeader.Text = Btn_PostIntake.Text = "Drag Applicant";
            //        this.PI_Name.HeaderText = "Referred By";
            //        this.PI_Prog.HeaderText = "Referred Date";
            //        this.PI_Name.Width = 183;//195;
            //        this.PI_Prog.Width = 90;
            //        this.Post_Img.Visible = false;

            //        foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
            //        {
            //            //rowIndex = From_Sum_Grid.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Ent.App);
            //            //From_Sum_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

            //            Compare_SSN_List.Clear();
            //            Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
            //            if (Compare_SSN_List.Count == 0)
            //            {
            //                Ref_Date = " ";
            //                TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
            //                if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
            //                    Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

            //                rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, Ent.Referred_By, Ref_Date,
            //                                    "N", Ent.App, Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year + Ent.App + Ent.Ref_Agy + Ent.Ref_Agy + Ent.Ref_Prog, Ent.Ssn, Ent.Snp_F_Name, Ent.Snp_L_Name, Ent.Snp_DOB);

            //                GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //            }
            //        }
            //    }
            //}
        }

        private void Fill_CaseSum_Grid()
        {
            SUM_Intake_CNT = 0;

            CaseMst_Panel.Visible = false;
            if (frmSearch != "Y")
               this.Size = new Size(this.Width, 382 + 105);

            frmSearch = "N";

            Sum_Search_Panel.Visible = CaseSum_Panel.Visible = true;

            int rowIndex = 0; bool From_Date_Flg = false, To_Date_Flg = false;
            string Ref_Date = string.Empty, TmpName = string.Empty, Status_To_Compare = ((ListItem)Cmb_Status.SelectedItem).Value.ToString(),
                   Snp_DOB = string.Empty;
            CaseSumGrid.Rows.Clear();
            List<CaseEnrlEntity> App_Curr_Enrl_Status_Records = new List<CaseEnrlEntity>();

            foreach (Sum_Referral_Entity Ent in Referral_List_To_Fill)
            {
                From_Date_Flg = To_Date_Flg = false;

                if (!From_Date.Checked)
                    From_Date_Flg = true;

                if (!To_Date.Checked)
                    To_Date_Flg = true;

                if ((string.IsNullOrEmpty(Txt_First_Name.Text.Trim()) || Ent.Snp_F_Name.Contains(Txt_First_Name.Text.Trim())) &&
                   (string.IsNullOrEmpty(Txt_Last_Name.Text.Trim()) || Ent.Snp_L_Name.Contains(Txt_Last_Name.Text.Trim())) &&
                    (Status_To_Compare == "0" || Status_To_Compare == Ent.Referred_Status))
                {
                    Ref_Date = Snp_DOB = " ";
                    if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
                    {
                        Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        if (From_Date.Checked)
                        {
                            if (From_Date.Value <= Convert.ToDateTime(Ent.Referred_Date))
                                From_Date_Flg = true;
                        }
                        if (To_Date.Checked)
                        {
                            if (To_Date.Value >= Convert.ToDateTime(Ent.Referred_Date))
                                To_Date_Flg = true;
                        }
                    }


                    if (From_Date_Flg && To_Date_Flg)
                    {
                        // Commented on 2/18/2016 Need to Implement New Logic
                        App_Curr_Enrl_Status_Records.Clear();
                        ////Enrl_BaseHie_App_List

                        if (ProgramDefinition != null)
                        {
                            if (ProgramDefinition.PRODUPSSN != "Y" && ProgramDefinition.ProDupMEM != "Y")
                            {
                                if (Ent.Expected_App_No != "No App" && Ent.Expected_App_No.Length > 10)
                                {
                                    //Ent.Expected_App_No.Substring(Ent.Expected_App_No.Length - 8, 8)
                                    App_Curr_Enrl_Status_Records = Enrl_BaseHie_App_List.FindAll(u => u.App.Equals(Ent.Expected_App_No.Substring(Ent.Expected_App_No.Length - 8, 8)));
                                }
                                if (App_Curr_Enrl_Status_Records.Count > 0)
                                {
                                    Ent.Expected_App_No = "No App";
                                }
                            }
                        }

                        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                        if (!string.IsNullOrEmpty(Ent.Snp_DOB.Trim()))
                            Snp_DOB = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Snp_DOB).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        if (Ent.Expected_App_No == "No App")
                        {
                            rowIndex = CaseSumGrid.Rows.Add(false, Img_Blank, Ref_Date, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog + " [" + Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog) + "]", Ent.App, TmpName, Get_Status_Desc(Ent.Referred_Status),
                            Ent.Expected_App_No, "N", ((Ent.Expected_App_No != "No App" && Ent.Expected_App_No.Length > 10) ? Ent.Expected_App_No.Substring(Ent.Expected_App_No.Length - 8, 8) : " "),
                            Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie, Ent.Snp_F_Name, Ent.Snp_L_Name, Ent.Snp_M_Name, "N", Ent.Ssn, Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year, Snp_DOB, Ent.Mst_Fam_Seq);

                            CaseSumGrid.Rows[rowIndex]["Column0"].Style.Padding = new Padding(CaseSumGrid.Rows[rowIndex]["Column0"].OwningColumn.Width, 0, 0, 0);
                        }
                        else
                        {
                            rowIndex = CaseSumGrid.Rows.Add(false, Img_Blank, Ref_Date, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog + " [" + Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog) + "]", Ent.App, TmpName, Get_Status_Desc(Ent.Referred_Status),
                            Ent.Expected_App_No, "N", ((Ent.Expected_App_No != "No App" && Ent.Expected_App_No.Length > 10) ? Ent.Expected_App_No.Substring(Ent.Expected_App_No.Length - 8, 8) : " "),
                            Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie, Ent.Snp_F_Name, Ent.Snp_L_Name, Ent.Snp_M_Name, "N", Ent.Ssn, Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year, Snp_DOB, Ent.Mst_Fam_Seq);

                            CaseSumGrid.Rows[rowIndex]["Column0"].Style.Padding = new Padding(5, 5, 5, 5);
                        }
                        CaseSumGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void Rb_From_Sum_NotInMst_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void From_Sum_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void radioButton4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_Click(object sender, EventArgs e)
        {

        }

        private void Fund_Panel_Click(object sender, EventArgs e)
        {

        }


        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Cmb_Sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CaseSumGrid.Rows.Count > 0 && CaseSum_Grid_Rows_Cnt > 0)
            {
                if (((ListItem)Cmb_Sort.SelectedItem).Value.ToString() == "1")
                {
                    foreach (DataGridViewRow item in CaseSumGrid.Rows)
                    {
                        item.Cells["Sum_CL_Name"].Value = LookupDataAccess.GetMemberName(item.Cells["SUM_FName"].Value.ToString(), item.Cells["SUM_MName"].Value.ToString(), item.Cells["SUM_LName"].Value.ToString(), "1");
                    }
                    CaseSumGrid.Sort(CaseSumGrid.Columns["SUM_FName"], ListSortDirection.Ascending);
                }
                else
                {
                    foreach (DataGridViewRow item in CaseSumGrid.Rows)
                    {
                        item.Cells["Sum_CL_Name"].Value = LookupDataAccess.GetMemberName(item.Cells["SUM_FName"].Value.ToString(), item.Cells["SUM_MName"].Value.ToString(), item.Cells["SUM_LName"].Value.ToString(), "2");
                    }
                    CaseSumGrid.Sort(CaseSumGrid.Columns["SUM_LName"], ListSortDirection.Ascending);

                }
            }
        }

        string frmSearch = "N";
        private void Btn_Search_Click(object sender, EventArgs e)
        {
            frmSearch = "Y";
            CaseSumGrid.Columns["Column0"].Visible = true;
            Fill_CaseSum_Grid();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Cmb_Status_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        int Priv_Group = int.MaxValue, Curr_Group;
        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (CaseSumGrid.Rows.Count > 0)
            {
                string Compare_Status_Desc = CaseSumGrid.CurrentRow.Cells["SUM_Status"].Value.ToString();
                contextMenu1.MenuItems.Clear();
                MenuItem Menu_L1;
                if (CaseSumGrid.Rows.Count > 0 && Status_List.Count > 0)
                {
                    foreach (CommonEntity Entity in Status_List)
                    {
                        Menu_L1 = new MenuItem();
                        Menu_L1.Text = Entity.Desc;
                        Menu_L1.Tag = Entity.Code;
                        contextMenu1.MenuItems.Add(Menu_L1);

                        if (Compare_Status_Desc == Entity.Desc)
                            Menu_L1.Checked = true;

                    }
                }
            }
        }

        string Sql_SP_Result_Message = string.Empty;
        private void CaseSumGrid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string[] Split_Array = new string[2];

            if (objArgs.MenuItem.Tag is string)
            {
                Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");
                string Key = CaseSumGrid.CurrentRow.Cells["SUM_Key"].Value.ToString();


                if (!string.IsNullOrEmpty(Split_Array[0].Trim()))
                {
                    foreach (Sum_Referral_Entity Ent in Referral_List_To_Fill)
                    {
                        if (Key == Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie)//Ent.Ref_Agy + Ent.Ref_Dept + Ent.Ref_Prog)
                        {
                            Ent.Snp_F_Name = BaseForm.UserID;
                            Ent.Referred_Status = Split_Array[0].Trim();
                            if (_model.LiheAllData.UpdateCASESUM_Status_From_Enroll(Ent, BaseForm.UserID, out Sql_SP_Result_Message))
                                CaseSumGrid.CurrentRow.Cells["SUM_Status"].Value = objArgs.MenuItem.Text.ToString();

                            break;
                        }
                    }
                }
            }
        }

        private void Fill_NonEnrolls_From_CASEMST()
        {
            string TmpName = " ";
            int rowIndex = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();

            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = false;
            this.PI_Name.Width = 300;//200;
            this.PI_Prog.Width = 60;// 157;
            //this.LblHeader.Text = Btn_PostIntake.Text = "Create Wait List Record";
            this.PI_Name.HeaderText = "Name";
            //this.PI_Prog.HeaderText = "Program";
            this.PI_Prog.HeaderText = "APP#";
            this.Post_Img.Visible = true;
            GD_PostIntake.Rows.Clear();
            foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            {
                App_Exists = false;

                foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                {
                    if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
                        (Entity.FundHie.Trim() == Sel_Hie.Trim()))
                    {
                        App_Exists = true;
                        break;
                    }
                }

                if (!App_Exists)
                {
                    TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
                    //rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                    rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Base_Enty.AppNo, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                }
            }
        }

        private void Rb_From_Mst_Click(object sender, EventArgs e)
        {
            SetComboBoxValue(Cmb_Post_Status, "L");
            SUM_Intake_CNT = 0;
            Btn_Drag_App.Visible = Sum_Search_Panel.Visible = CaseSum_Panel.Visible = false;
            //CaseMst_Panel.Location = new System.Drawing.Point(-1, 56);
            //CaseMst_Panel.Size = new System.Drawing.Size(677, 360);
            CaseMst_Panel.Visible = true;
            this.Size = new Size(this.Width, this.Height + (CaseMst_Panel.Height));

            if (Module_Code == "03")
            {
                GD_PostIntake.Size = new System.Drawing.Size(408, 354);
                Fill_NonEnrolls_From_CASEMST();
            }
            else
            {
                GD_PostIntake.Size = new System.Drawing.Size(408, 324);
                Fill_HSS_NonEnrolls_From_CASEMST();
            }
        }

        int SUM_Intake_CNT = 0;
        private void CaseSumGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CaseSumGrid.Rows.Count > 0)
            {
                int ColIdx = CaseSumGrid.CurrentCell.ColumnIndex;
                int RowIdx = CaseSumGrid.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && CaseSumGrid.CurrentRow.Cells["SUM_Exp_App"].Value.ToString() != "No App")
                    {
                        //if (CaseSumGrid.CurrentRow.Cells["SUM_Sel"].Value.ToString() == "Y")
                        //{
                        //    CaseSumGrid.CurrentRow.Cells["SUM_Img"].Value = Img_Blank;
                        //    CaseSumGrid.CurrentRow.Cells["SUM_Sel"].Value = "N";
                        //    SUM_Intake_CNT--;
                        //}
                        //else
                        //{
                        //    CaseSumGrid.CurrentRow.Cells["SUM_Img"].Value = Img_Tick;
                        //    CaseSumGrid.CurrentRow.Cells["SUM_Sel"].Value = "Y";
                        //    SUM_Intake_CNT++;
                        //}

                        if (CaseSumGrid.CurrentRow.Cells["Column0"].Value.ToString() == "True")
                        {
                            SUM_Intake_CNT++;
                        }
                        else
                        {
                            SUM_Intake_CNT--;
                        }
                    }
                }
            }
        }

        int Post_Intake_CNT = 0;
        private void GD_PostIntake_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GD_PostIntake.Rows.Count > 0)
            {
                int ColIdx = GD_PostIntake.CurrentCell.ColumnIndex;
                int RowIdx = GD_PostIntake.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (GD_PostIntake.CurrentRow.Cells["PI_Sel_SW"].Value.ToString() == "Y")
                        {
                            GD_PostIntake.CurrentRow.Cells["Post_Img"].Value = Img_Blank;
                            GD_PostIntake.CurrentRow.Cells["PI_Sel_SW"].Value = "N";
                            Post_Intake_CNT--;
                        }
                        else
                        {
                            GD_PostIntake.CurrentRow.Cells["Post_Img"].Value = Img_Tick;
                            GD_PostIntake.CurrentRow.Cells["PI_Sel_SW"].Value = "Y";
                            Post_Intake_CNT++;
                        }
                    }
                }
            }
        }


        private void Rb_From_Sum_Click(object sender, EventArgs e)
        {
            SetComboBoxValue(Cmb_Post_Status, "I");
            if (Module_Code == "02")
                SetComboBoxValue(Cmb_Fund, Comm_Sel_Fund);

            Fill_CaseSum_Grid();
        }


        public bool Validate_Status_Date()
        {
            bool IsValid = true;

            if (((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString() == "0")// && Mode == "Add_FUND")
            {
                _errorProvider.SetError(Cmb_Post_Status, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label8.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValid = false;
            }
            else
                _errorProvider.SetError(Cmb_Post_Status, null);


            if (!Status_Date.Checked)
            {
                _errorProvider.SetError(Status_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label56.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValid = false;
            }
            else
            {
                if (Convert.ToDateTime(Convert.ToDateTime(Status_Date.Value).ToString("MM/dd/yyyy")) > DateTime.Today)
                {
                    _errorProvider.SetError(Status_Date, label56.Text + " Should not be Future Date ".Replace(Consts.Common.Colon, string.Empty));
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(Status_Date, null);
            }


            //if (Privileges.ModuleCode == "03")
            {
                string Class_Start_Date = string.Empty, Class_End_Date = string.Empty;
                foreach (DepEnrollHierachiesEntity Entity in DepEnrollList)
                {
                    if (Entity.Hierachies == Sel_Hie.Trim())
                    {
                        Class_Start_Date = Entity.StartDate;
                        Class_End_Date = Entity.Enddate;
                        break;
                    }
                }

                string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_Start_Date);
                string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_End_Date);

                bool Validate_Trf_Date = true;
                if (!string.IsNullOrEmpty(Class_Start_Date) && !string.IsNullOrEmpty(Class_End_Date) && Status_Date.Checked)
                {
                    if (Convert.ToDateTime(Class_Start_Date) > Status_Date.Value || Convert.ToDateTime(Class_End_Date) < Status_Date.Value)
                    {
                        _errorProvider.SetError(Status_Date, label56.Text + " Should be within Program Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                        Validate_Trf_Date = IsValid = false;
                    }
                    else
                        if (IsValid)
                        _errorProvider.SetError(Status_Date, null);
                }
            }

            return IsValid;
        }

        private void Btn_PostIntake_Click(object sender, EventArgs e)
        {
            if (Validate_Status_Date())
            {
                // if (Rb_From_Sum.Checked)
                {
                    if ((SUM_Intake_CNT > 0 && Rb_From_Sum.Checked) ||
                        (Post_Intake_CNT > 0 && !Rb_From_Sum.Checked))
                    {
                        bool Save_Result = false;
                        CaseEnrlEntity New_Post_Record;

                        StringBuilder Xml_To_Pass = new StringBuilder();
                        StringBuilder Sum_Xml_To_Pass = new StringBuilder();
                        Xml_To_Pass.Append("<Rows>");
                        Sum_Xml_To_Pass.Append("<Rows>");

                        int NooF_Rows_Processed = 0;
                        New_Post_Record = new CaseEnrlEntity(true);

                        New_Post_Record.Row_Type = "I";
                        New_Post_Record.Agy = BaseForm.BaseAgency;
                        New_Post_Record.Dept = BaseForm.BaseDept;
                        New_Post_Record.Prog = BaseForm.BaseProg;
                        New_Post_Record.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");

                        New_Post_Record.ID = "1";
                        //New_Post_Record.Status_Date = DateTime.Today.ToShortDateString();
                        New_Post_Record.Status_Date = Status_Date.Value.ToShortDateString();

                        string Find_Hie_ToPass = null, RecordType_ToPass = "H", Status_ToPass = "L";
                        RecordType_ToPass = "C";
                        if (Privileges.ModuleCode == "03")
                        {
                            Find_Hie_ToPass = Sel_Hie;
                            //Status_ToPass = (Rb_From_Sum.Checked ? "I" : "L");
                        }
                        else
                        {
                            Find_Hie_ToPass = Comm_Sel_Fund;
                            RecordType_ToPass = "H"; // Replace it With Fund
                        }


                        Status_ToPass = ((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString();
                        if (Rb_From_Sum.Checked)
                        {
                            foreach (DataGridViewRow dr in CaseSumGrid.Rows)
                            {
                                if (dr.Cells["Column0"].Value.ToString() == "True")//(dr.Cells["SUM_Sel"].Value.ToString() == "Y")
                                {
                                    New_Post_Record.App = dr.Cells["dataGridViewTextBoxColumn7"].Value.ToString();

                                    New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.Group = New_Post_Record.AMPM = New_Post_Record.Enrl_Date = string.Empty;
                                    New_Post_Record.Seq = "1";

                                    New_Post_Record.FundHie = Find_Hie_ToPass;
                                    New_Post_Record.Rec_Type = RecordType_ToPass;
                                    New_Post_Record.Status = Status_ToPass;
                                    New_Post_Record.Lstc_Oper = BaseForm.UserID;

                                    if (Module_Code == "03")
                                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + null +
                                                   "\" Enrl_ROOM = \"" + null + "\" Enrl_AMPM = \"" + null +
                                                   "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                                   "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                                   "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                                   "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + dr.Cells["SUM_Key"].Value.ToString() +
                                                   "\" Mst_Fam_Seq= \"" + dr.Cells["MST_Seq"].Value.ToString() + "\"/>");
                                    else
                                        // Old Logic 07042014 With MST-Site
                                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + (Status_ToPass == "L" ? null : Sel_Site) +
                                                   "\" Enrl_ROOM = \"" + (Status_ToPass == "L" ? null : Sel_Room) + "\" Enrl_AMPM = \"" + (Status_ToPass == "L" ? null : Sel_AMPM) +
                                                   "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                                   "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                                   "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                                   "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + dr.Cells["SUM_Key"].Value.ToString() +
                                                    "\" Mst_Fam_Seq= \"" + dr.Cells["MST_Seq"].Value.ToString() + "\"/>");

                                    // New Logic 07042014 With MST-Site
                                    //Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + (Sel_Site) +
                                    //           "\" Enrl_ROOM = \"" + (Sel_Room) + "\" Enrl_AMPM = \"" + (Sel_AMPM) +
                                    //           "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                    //           "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                    //           "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                    //           "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + dr.Cells["SUM_Key"].Value.ToString() + "\"/>");


                                }
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow dr in GD_PostIntake.Rows)
                            {
                                if (dr.Cells["PI_Sel_SW"].Value.ToString() == "Y")
                                {
                                    New_Post_Record.App = dr.Cells["PI_Ref_App"].Value.ToString();

                                    New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.Group = New_Post_Record.AMPM = New_Post_Record.Enrl_Date = string.Empty;
                                    New_Post_Record.Seq = "1";

                                    New_Post_Record.FundHie = Find_Hie_ToPass;
                                    New_Post_Record.Rec_Type = RecordType_ToPass;
                                    New_Post_Record.Status = Status_ToPass;
                                    New_Post_Record.Lstc_Oper = BaseForm.UserID;

                                    if (Module_Code == "03")
                                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + null +
                                                   "\" Enrl_ROOM = \"" + null + "\" Enrl_AMPM = \"" + null +
                                                   "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                                   "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                                   "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                                   "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + " " +
                                                   "\" Mst_Fam_Seq= \"" + "" + "\"/>");

                                    else
                                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + Sel_Site +
                                                   "\" Enrl_ROOM = \"" + Sel_Room + "\" Enrl_AMPM = \"" + Sel_AMPM +
                                                   "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                                   "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                                   "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                                   "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + " " +
                                                   "\" Mst_Fam_Seq= \"" + "" + "\"/>");

                                }
                            }
                        }
                        Xml_To_Pass.Append("</Rows>");
                        Sum_Xml_To_Pass.Append("</Rows>");


                        bool save_result = false;
                        if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", Xml_To_Pass.ToString(), string.Empty, null, out Sql_SP_Result_Message))
                        {
                            AlertBox.Show("Record Created Successfully");
                            save_result = true;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                    else
                        AlertBox.Show("You must Select atleast One Record", MessageBoxIcon.Warning);
                }
            }
        }


        private void Btn_Drag_App_Click(object sender, EventArgs e)//03252015
        {
            string Tmp_Hie_Key = CaseSumGrid.CurrentRow.Cells["SUM_Ref_Hie"].Value.ToString();
            string Tmp_Hie_App = CaseSumGrid.CurrentRow.Cells["SUM_App"].Value.ToString();

            string Fname = CaseSumGrid.CurrentRow.Cells["SUM_FName"].Value.ToString();
            string Lname = CaseSumGrid.CurrentRow.Cells["SUM_LName"].Value.ToString();
            string SSn = CaseSumGrid.CurrentRow.Cells["SUM_SSN"].Value.ToString();
            string DOB = CaseSumGrid.CurrentRow.Cells["SUM_DOB"].Value.ToString(); ;// CaseSumGrid.CurrentRow.Cells["PI_DOB"].Value.ToString();
            string Sum_Key = CaseSumGrid.CurrentRow.Cells["SUM_Key"].Value.ToString();

            MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, 'Y', DOB, Fname, Lname, SSn, Tmp_Hie_App, Tmp_Hie_Key.Substring(0, 10), Privileges.Program, true, Sum_Key);
            AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
            AddApplicant.StartPosition = FormStartPosition.CenterScreen;
            AddApplicant.ShowDialog();
        }

        string New_Dragged_App_No = string.Empty;
        private void On_Applicant_Dragged(object sender, FormClosedEventArgs e)
        {
            New_Dragged_App_No = string.Empty;
            string New_Fam_Seq = string.Empty;
            bool App_Selecion_Changed = false;
            MainMenuAddApplicantForm form = sender as MainMenuAddApplicantForm;
            if (form.DialogResult == DialogResult.OK)
            {
                New_Dragged_App_No = form.Get_Dragged_App_No();
                New_Fam_Seq = form.Get_Dragged_App_No_Fam_Seq();
                App_Selecion_Changed = form.Get_App_Slection_Change();
                if (!App_Selecion_Changed)
                {
                    CaseSumGrid.CurrentRow.Cells["SUM_Exp_App"].Value = New_Dragged_App_No;
                    CaseSumGrid.CurrentRow.Cells["dataGridViewTextBoxColumn7"].Value = New_Dragged_App_No.Substring(New_Dragged_App_No.Length - 8, 8);
                    CaseSumGrid.CurrentRow.Cells["MST_Seq"].Value = New_Fam_Seq;
                    Btn_Drag_App.Visible = false;

                    string Key = CaseSumGrid.CurrentRow.Cells["SUM_Key"].Value.ToString();
                    foreach (Sum_Referral_Entity Ent in Referral_List_To_Fill)
                    {
                        if (Key == Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie)//Ent.Ref_Agy + Ent.Ref_Dept + Ent.Ref_Prog)
                        {
                            Ent.Expected_App_No = New_Dragged_App_No;
                            break;
                        }
                    }
                }

                Tmp_Referral_List.Clear();
                if (Module_Code == "03")
                    Fill_NonEnrolls_From_CASESUM();
                else
                    Fill_HSS_NonEnrolls_From_CASESUM();
            }
        }


        private void CaseSumGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (CaseSumGrid.Rows.Count > 0)
            {
                string Exp_App = CaseSumGrid.CurrentRow.Cells["SUM_Exp_App"].Value.ToString();
                Btn_Drag_App.Visible = false;
                if (Exp_App == "No App")
                {
                    if (Intake_Add_Priv)
                        Btn_Drag_App.Visible = true;
                }
            }
        }

        bool Intake_Add_Priv = false;
        private void Get_ClientIntake_Priv()
        {
            Intake_Add_Priv = false;
            char AddPriv = 'U';
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetPrivilizes_byScrCode(BaseForm.UserID, BaseForm.BusinessModuleID, "CASE2001");
            DataTable dt = ds.Tables[0];
            string TmpHie = null, Current_HieTo_Compare = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 1: Current_HieTo_Compare = BaseForm.BaseAgency + BaseForm.BaseDept + "**"; break;
                        case 2: Current_HieTo_Compare = BaseForm.BaseAgency + "****"; break;
                        case 3: Current_HieTo_Compare = "******"; break;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        TmpHie = dr["EFR_HIERARCHY"].ToString();

                        if (TmpHie.Substring(0, 2) == Current_HieTo_Compare.Substring(0, 2) &&
                            TmpHie.Substring(2, 2) == Current_HieTo_Compare.Substring(2, 2) &&
                            TmpHie.Substring(4, 2) == Current_HieTo_Compare.Substring(4, 2))
                        { AddPriv = char.Parse(dr["EFR_ADD_PRIV"].ToString()); break; }
                    }
                    if (AddPriv == 'Y' || AddPriv == 'N')
                        break;
                }
            }

            if (AddPriv == 'Y')
                Intake_Add_Priv = true;
        }

        private void CASE0010_CM_Form_Load(object sender, EventArgs e)
        {

        }

        private void Cmb_Post_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString())
            {
                case "P":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Pending Record(s)";             
                    Btn_PostIntake.Text = "&Create Pending Record(s)";
                    break;
                case "L":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Wait List Record(s)";
                    Btn_PostIntake.Text = "&Create Wait List Record(s)";
                    break;
                case "R":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Denied Record(s)";
                    Btn_PostIntake.Text = "&Create Denied Record(s)";
                    break;
                case "E":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Enroll Record(s)";
                    Btn_PostIntake.Text = "&Create Enroll Record(s)";
                    break;
                case "N":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Inactive Record(s)";
                    Btn_PostIntake.Text = "&Create Inactive Record(s)";
                    break;
                case "W":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Withdrawn Record(s)";
                    Btn_PostIntake.Text = "&Create Withdrawn Record(s)";
                    break;
                case "I":
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Postintake Record(s)";
                    Btn_PostIntake.Text = "&Create Postintake Record(s)";
                    break;
                default:
                    if (Privileges.ModuleCode == "03")
                        this.Text = "Create Status Record(s)";
                    Btn_PostIntake.Text = "&Create Status Record(s)";
                    break;
            }

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


        //List<MainMenuEntity> App_Reff_From_CASESUM = new List<MainMenuEntity>();
        //List<Sum_Referral_Entity> HSS_Referral_List = new List<Sum_Referral_Entity>();
        //List<CaseSumEntity> HSS_Tmp_Referral_NotInMST_List = new List<CaseSumEntity>();
        //List<MainMenuEntity> HSS_Base_MainMenu_NotEnroll_List = new List<MainMenuEntity>();
        private void Fill_HSS_NonEnrolls_From_CASESUM()
        {
            string TmpName = " ";
            int rowIndex = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();

            //if (Privileges.ModuleCode == "02" && Tmp_Referral_List.Count == 0)
            if (Privileges.ModuleCode == "02" && Tmp_Referral_List.Count == 0)
            {
                List<CaseSumEntity> Tmp_CaseSum_List = new List<CaseSumEntity>();
                CaseSumEntity Search_Entity = new CaseSumEntity(true);

                Search_Entity.CaseSumRefHierachy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
                //Search_Entity.CaseSumRefDept = BaseForm.BaseDept;
                //Search_Entity.CaseSumRefProgram = BaseForm.BaseProg;

                List<SqlParameter> sqlParamList = _model.EnrollData.Prepare_MST_SUM_NonEnrollSqlParameters_List(Search_Entity, "Browse", BaseForm.BaseAgency
                                                                , BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_CaseSum_RefApps_On_MSTSNP]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    if (((ListItem)Cmb_Sort.SelectedItem).Value.ToString() == "1")
                        CASEENRLData.Tables[0].DefaultView.Sort = "SNP_NAME_IX_FI , SNP_NAME_IX_MI, SNP_NAME_IX_LAST";
                    else
                        CASEENRLData.Tables[0].DefaultView.Sort = "SNP_NAME_IX_LAST, SNP_NAME_IX_FI, SNP_NAME_IX_MI"; // 04072014

                    DataTable Referral_Table = CASEENRLData.Tables[0].DefaultView.ToTable();

                    //foreach (DataRow row in CASEENRLData.Tables[0].Rows)
                    foreach (DataRow row in Referral_Table.Rows)
                        Tmp_Referral_List.Add(new Sum_Referral_Entity(row));
                }
                if (CASEENRLData != null && CASEENRLData.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[1].Rows)
                        Base_MainMenu_NotEnroll_List.Add(new MainMenuEntity(row, "Original"));
                }

                if (CASEENRLData != null && CASEENRLData.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[2].Rows)
                        Tmp_Referral_NotInMST_List.Add(new CaseSumEntity(row, string.Empty));
                }
            }


            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = true;
            //this.LblHeader.Text = Btn_PostIntake.Text = "Send to Post Intake"; 
            GD_PostIntake.Rows.Clear();
            if (Rb_From_Sum.Checked)
            {
                //this.LblHeader.Text = Btn_PostIntake.Text = "Create PostIntake Record";
                this.PI_Prog.HeaderText = "Exp Enroll App#";
                this.PI_Name.Width = 124;//132;
                this.PI_Prog.Width = 120;
                this.Post_Img.Visible = true;

                string Ref_Date = string.Empty;
                Referral_List_To_Fill.Clear();
                foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
                {
                    Ref_Date = " ";
                    if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
                        Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    Compare_SSN_List.Clear();
                    if (Ent.Ssn.Substring(3, 2) == "00")
                        Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.DOB.Equals(Ent.Snp_DOB) && u.Fname.Equals(Ent.Snp_F_Name) && u.Lname.Equals(Ent.Snp_L_Name)); //Pseudo SSN # Logic
                    else
                        Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn));

                    if (Compare_SSN_List.Count > 0)
                    {
                        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                        CaseSum_Grid_Rows_Cnt++;
                        Ent.ID = CaseSum_Grid_Rows_Cnt.ToString();
                        Ent.Mst_Fam_Seq = Compare_SSN_List[0].FamSeq;
                        Referral_List_To_Fill.Add(new Sum_Referral_Entity(Ent, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog)));
                    }
                    else
                    {
                        if (Compare_SSN_List.Count == 0)
                        {
                            TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());

                            CaseSum_Grid_Rows_Cnt++;
                            Ent.ID = CaseSum_Grid_Rows_Cnt.ToString();
                            Ent.Mst_Fam_Seq = "";
                            Referral_List_To_Fill.Add(new Sum_Referral_Entity(Ent, string.Empty, Get_Program_Desc(Ent.Agy, Ent.Dept, Ent.Prog)));
                            //CaseSumGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
                Fill_CaseSum_Grid();

                //foreach (Sum_Referral_Entity Ent in HSS_Referral_List)
                //{
                //    Compare_SSN_List.Clear();
                //    Compare_SSN_List = HSS_Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
                //    if (Compare_SSN_List.Count > 0)
                //    {
                //        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                //        rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName,
                //                        BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, "N", Compare_SSN_List[0].AppNo,
                //                        Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Agy + Ent.Ref_Dept + Ent.Ref_Prog, " ", " ", " ", " ");
                //        GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                //    }
                //}
            }
            //else
            //{
            //    if (Rb_From_Sum_NotInMst.Checked && Module_Code == "02")
            //    {
            //        TmpName = " "; string Ref_Date = " ";

            //        this.LblHeader.Text = Btn_PostIntake.Text = "Drag Applicant";
            //        this.PI_Name.HeaderText = "Client Name"; // "Referred By";
            //        this.PI_Prog.HeaderText = "Referred Date";
            //        this.PI_Name.Width = 183;//195;
            //        this.PI_Prog.Width = 90;
            //        this.Post_Img.Visible = false;

            //        foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
            //        {

            //            Compare_SSN_List.Clear();
            //            Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
            //            if (Compare_SSN_List.Count == 0)
            //            {
            //                Ref_Date = " ";
            //                TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
            //                if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
            //                    Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

            //                rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, Ent.Referred_By, Ref_Date,
            //                                    "N", Ent.App, Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year + Ent.App + Ent.Ref_Agy + Ent.Ref_Agy + Ent.Ref_Prog, Ent.Ssn, Ent.Snp_F_Name, Ent.Snp_L_Name, Ent.Snp_DOB);

            //                GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //            }
            //        }
            //    }
            //}
        }

        string Sel_Site = string.Empty, Sel_Room = string.Empty, Sel_AMPM = string.Empty;
        private void Fill_HSS_NonEnrolls_From_CASEMST()
        {
            SetComboBoxValue(Cmb_Fund2, Comm_Sel_Fund);
            string TmpName = " ";
            int rowIndex = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();

            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = false;
            this.PI_Name.Width = 300;//200;
            this.PI_Prog.Width = 60;// 157;
            //this.LblHeader.Text = Btn_PostIntake.Text = "Send to Wait List";
            this.PI_Name.HeaderText = "Name";
            //this.PI_Prog.HeaderText = "Program";
            this.PI_Prog.HeaderText = "APP#";
            this.Post_Img.Visible = true;
            GD_PostIntake.Rows.Clear();

            Sel_Hie = Comm_Sel_Fund;
            foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            {
                App_Exists = false;

                foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                {
                    if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
                        (Entity.FundHie.Trim() == Sel_Hie.Trim()))
                    {
                        App_Exists = true;
                        break;
                    }
                }


                if (!App_Exists && Base_Enty.Site == Sel_Site)
                {
                    TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
                    //rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                    rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Base_Enty.AppNo, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                }
            }
        }

        private void Cmb_Fund2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comm_Sel_Fund = ((ListItem)Cmb_Fund2.SelectedItem).Value.ToString();
            GD_PostIntake.Rows.Clear();

            bool App_Exists = false; int rowIndex = 0;
            string TmpName = "";
            Sel_Hie = Comm_Sel_Fund;
            foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            {
                App_Exists = false;

                foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                {
                    if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
                        (Entity.FundHie.Trim() == Sel_Hie.Trim()))
                    {
                        App_Exists = true;
                        break;
                    }
                }


                if (!App_Exists && Base_Enty.Site == Sel_Site)
                {
                    TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
                    rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Base_Enty.AppNo, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                }
            }
        }

        private void Cmb_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comm_Sel_Fund = ((ListItem)Cmb_Fund.SelectedItem).Value.ToString();
        }

        private void Pb_Site_Search_Click(object sender, EventArgs e)
        {

        }

        private void Txt_Fund_Rate_LostFocus(object sender, EventArgs e)
        {

        }

        private void Txt_Parent_Rate_LostFocus(object sender, EventArgs e)
        {

        }

        private void Cmb_Add_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Add_Fund.Items.Count > 0)
            {
                if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() == "Y")
                {
                    Fund_Controls_Panel.Visible = false;
                    this.Size = new System.Drawing.Size(Width, 464);//this.Size = new Size(this.Width, 254);
                    pnlEnrl2.Size = new Size(Width, 34);
                    Clear_Pane8_Controls();
                }
                else
                {
                    Fund_Controls_Panel.Visible = true;
                    pnlEnrl2.Size = new Size(Width, 350);
                    this.Size = new Size(this.Width, 31);
                }
            }

        }

        private void Clear_Pane8_Controls()
        {
            Txt_Parent_Rate.Clear();
            Txt_Fund_Rate.Clear();

            //Fund_Enroll_Date.Value = DateTime.Today; Fund_Enroll_Date.Checked = false;
            Fund_End_Date.Value = Rate_Eff_Date.Value = DateTime.Today;
            Fund_End_Date.Checked = Rate_Eff_Date.Checked = false;
        }

        private void Fill_Fund_Category()
        {
            Cmb_Fund_Category.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));
            listItem.Add(new ListItem("Full", "F"));
            listItem.Add(new ListItem("3/4 ", "3"));
            listItem.Add(new ListItem("1/2 ", "2"));
            listItem.Add(new ListItem("1/4", "1"));
            Cmb_Fund_Category.Items.AddRange(listItem.ToArray());
            Cmb_Fund_Category.SelectedIndex = 0;
        }

        private void Pb_Site_Search_Click_1(object sender, EventArgs e)
        {
            Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
            SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();
        }

        string Added_Edited_SiteCode = string.Empty; string Added_Edited_HieCode = string.Empty;
        private void Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_Site.Text = From_Results[0];
                Txt_Room.Text = From_Results[1];

                switch (From_Results[2])
                {
                    case "A": Txt_AMPM.Text = "A - AM Class"; break;
                    case "P": Txt_AMPM.Text = "P - PM Class"; break;
                    case "E": Txt_AMPM.Text = "E - Extended Day"; break;
                    case "F": Txt_AMPM.Text = "F - Full Day"; break;
                }
            }
        }

        private void fillStatusDate()
        {
            if (Fund_Enroll_Date.Checked)
            {
                _errorProvider.SetError(Fund_Enroll_Date, null);
                string Tmp_Date = string.Empty, Tmp_Attn_Date = string.Empty;
                foreach (DataGridViewRow dr in Grid_Applications.Rows)
                {
                    if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                    {
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.ID == dr.Cells["GD_ID"].Value.ToString())
                            {
                                dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Fund_Enroll_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            }
                        }
                    }
                }
            }
            else
                _errorProvider.SetError(Fund_Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Enroll Date".Replace(Consts.Common.Colon, string.Empty)));
        }

        string Error_MSG = "";
        private bool Validate_Fund_For_Add_Apps()
        {
            bool Can_Add = true;
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Join_Mst_Snp = "N";
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            Search_Entity.Year = BaseForm.BaseYear;
            Search_Entity.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
            Search_Entity.Enrl_Status_Not_Equalto = "T";
            Search_Entity.Rec_Type = "H";

            List<CaseEnrlEntity> Enroll_List = new List<CaseEnrlEntity>();
            Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");

            Error_MSG = "";
            foreach (CaseEnrlEntity Ent in Pass_Enroll_List)
            {
                foreach (CaseEnrlEntity ent in Enroll_List)
                {
                    if (Ent.App == ent.App)
                    {
                        Error_MSG += "App# " + ent.App + " Already Exists with this Fund in " + (Ent.Site + "/" + Ent.Room + "/" + Ent.AMPM) + "\n";
                        Can_Add = false;
                        break;
                    }

                }
            }

            return Can_Add;
        }

        private void Btn_NoStatus_Add_Click(object sender, EventArgs e)
        {
            if (Validate_Add_NoStatus_Controls())
            {
                if (!Validate_Fund_For_Add_Apps())
                {
                    AlertBox.Show(Error_MSG, MessageBoxIcon.Warning);
                    return;
                }

                bool Save_Result = false;
                CaseEnrlEntity New_Post_Record;

                int NooF_Rows_Processed = 0;
                string Tmp_Status = "";
                New_Post_Record = new CaseEnrlEntity(true);

                StringBuilder Xml_To_Pass = new StringBuilder();
                StringBuilder Sum_Xml_To_Pass = new StringBuilder();
                Xml_To_Pass.Append("<Rows>");
                Sum_Xml_To_Pass.Append("<Rows>");

                /* New_Post_Record.Row_Type = "I";
                 New_Post_Record.Agy = BaseForm.BaseAgency;
                 New_Post_Record.Dept = BaseForm.BaseDept;
                 New_Post_Record.Prog = BaseForm.BaseProg;
                 New_Post_Record.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");

                 New_Post_Record.ID = "1";
                 New_Post_Record.Status_Date = New_Post_Record.Enrl_Date = Fund_Enroll_Date.Value.ToShortDateString();

                 New_Post_Record.Site = Txt_Site.Text;
                 New_Post_Record.Room = Txt_Room.Text;
                 New_Post_Record.AMPM = Txt_AMPM.Text;

                 New_Post_Record.Desc_1 = Txt_Desc1.Text.Trim();
                 New_Post_Record.Desc_2 = Txt_Desc2.Text.Trim();

                 New_Post_Record.App = Pass_Enroll_List[0].App;

                 New_Post_Record.Group = Pass_Enroll_List[0].Group;
                 //New_Post_Record.Enrl_Date = string.Empty;
                 New_Post_Record.Seq = "1";

                 New_Post_Record.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
                 New_Post_Record.Rec_Type = "H";
                 Tmp_Status = New_Post_Record.Status = ((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString();
                 New_Post_Record.Lstc_Oper = BaseForm.UserID;

                 New_Post_Record.Funding_Code = New_Post_Record.Fund_End_date = New_Post_Record.Rate_EFR_date = "";
                 New_Post_Record.Parent_Rate = New_Post_Record.Funding_Rate = "0";

                 if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() == "N")
                 {
                     New_Post_Record.Funding_Code = ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString();
                     New_Post_Record.Fund_End_date = Fund_End_Date.Value.ToShortDateString();

                     New_Post_Record.Rate_EFR_date = Rate_Eff_Date.Value.ToShortDateString();

                     New_Post_Record.Parent_Rate = !string.IsNullOrEmpty(Txt_Parent_Rate.Text.Trim()) ? Txt_Parent_Rate.Text.Trim() : "0";
                     New_Post_Record.Funding_Rate = !string.IsNullOrEmpty(Txt_Fund_Rate.Text.Trim()) ? Txt_Fund_Rate.Text.Trim() : "0";

                 }*/

                string site = string.Empty; int x = 0;
                SelectedgvRows = (from c in Grid_Applications.Rows.Cast<DataGridViewRow>().ToList()
                              where (((DataGridViewCheckBoxCell)c.Cells["GD_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                              select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    foreach (DataGridViewRow dr in Grid_Applications.Rows)
                    {
                        if (dr.Cells["GD_Sel"].Value.ToString() == "True")
                        {
                            if (!string.IsNullOrEmpty(dr.Cells["GD_Date"].Value.ToString().Trim()))
                            {
                                New_Post_Record.Row_Type = "I";
                                New_Post_Record.Agy = BaseForm.BaseAgency;
                                New_Post_Record.Dept = BaseForm.BaseDept;
                                New_Post_Record.Prog = BaseForm.BaseProg;
                                New_Post_Record.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");

                                New_Post_Record.ID = "1";

                                New_Post_Record.Status_Date = New_Post_Record.Enrl_Date = dr.Cells["GD_Date"].Value.ToString();

                                //site = dr.Cells["GD_Program"].Value.ToString().Replace("/", "").Replace(" ", "");
                                //New_Post_Record.Site = site.Substring(0, 4);
                                //New_Post_Record.Room = site.Substring(4, 4);
                                //if(site.Length > 8)
                                //    New_Post_Record.AMPM = site.Substring(8, 1);

                                New_Post_Record.Site = Txt_Site.Text;
                                New_Post_Record.Room = Txt_Room.Text;
                                New_Post_Record.AMPM = Txt_AMPM.Text;

                                New_Post_Record.App = dr.Cells["GD_App"].Value.ToString();

                                New_Post_Record.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();//dr.Cells["GD_Fund"].Value.ToString();

                                New_Post_Record.Desc_1 = Txt_Desc1.Text.Trim();
                                New_Post_Record.Desc_2 = Txt_Desc2.Text.Trim();

                                New_Post_Record.Group = Pass_Enroll_List[0].Group;

                                New_Post_Record.Seq = "1";

                                New_Post_Record.Rec_Type = "H";
                                Tmp_Status = New_Post_Record.Status = ((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString();
                                New_Post_Record.Lstc_Oper = BaseForm.UserID;

                                New_Post_Record.Funding_Code = New_Post_Record.Fund_End_date = New_Post_Record.Rate_EFR_date = "";
                                New_Post_Record.Parent_Rate = New_Post_Record.Funding_Rate = "0";

                                if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() == "N")
                                {
                                    New_Post_Record.Funding_Code = ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString();
                                    New_Post_Record.Fund_End_date = Fund_End_Date.Value.ToShortDateString();

                                    New_Post_Record.Rate_EFR_date = Rate_Eff_Date.Value.ToShortDateString();

                                    New_Post_Record.Parent_Rate = !string.IsNullOrEmpty(Txt_Parent_Rate.Text.Trim()) ? Txt_Parent_Rate.Text.Trim() : "0";
                                    New_Post_Record.Funding_Rate = !string.IsNullOrEmpty(Txt_Fund_Rate.Text.Trim()) ? Txt_Fund_Rate.Text.Trim() : "0";

                                }

                                CaseEnrlEntity ent = Pass_Enroll_List.Find(u => u.Mst_App.ToString() == dr.Cells["GD_App"].Value.ToString() && u.FundHie.Trim() == dr.Cells["GD_Fund"].Value.ToString().Trim());

                                New_Post_Record.Seq = "1";

                                Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + "1" + "\" Enrl_FUND_HIE = \"" + New_Post_Record.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + ((Tmp_Status == "L" && Txt_Room.Text == "****") ? null : New_Post_Record.Site) + "\" Enrl_ROOM = \"" + ((Tmp_Status == "L" && Txt_Room.Text == "****") ? null : New_Post_Record.Room) + "\" Enrl_AMPM = \"" + ((Tmp_Status == "L" && Txt_Room.Text == "****") ? null : New_Post_Record.AMPM) +
                                               "\" ENRL_ENRLD_DATE = \"" + LookupDataAccess.Getdate(New_Post_Record.Status_Date) + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                               "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                               "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + New_Post_Record.Parent_Rate + "\"  Enrl_FUNDING_CODE= \"" + New_Post_Record.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + New_Post_Record.Funding_Rate +
                                               "\" Enrl_FUND_END_DATE= \"" + New_Post_Record.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + LookupDataAccess.Getdate(New_Post_Record.Rate_EFR_date) + "\" Enrl_Enroll_DATE= \"" + LookupDataAccess.Getdate(New_Post_Record.Status_Date) + "\"  Enrl_Curr_Status= \"" + New_Post_Record.Status + "\" Sum_Key_To_Update= \"" + " " +
                                               "\" Mst_Fam_Seq= \"" + "" + "\"/>");

                            }
                            else
                            {
                                AlertBox.Show("Status Date is required", MessageBoxIcon.Warning);
                                x = 1;
                            }
                        }
                    }

                    Xml_To_Pass.Append("</Rows>");
                    Sum_Xml_To_Pass.Append("</Rows>");

                    if (x == 0)
                    {
                        bool save_result = false;
                        if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", Xml_To_Pass.ToString(), string.Empty, null, out Sql_SP_Result_Message))
                        {
                            save_result = true;

                            if ((Tmp_Status == "L" && Txt_Room.Text == "****"))
                            {
                                if (propComboType == "2")
                                {
                                    foreach (CaseEnrlEntity Ent in Pass_Enroll_List)
                                    {
                                        string strApplNo = string.Empty;
                                        string strClientIdOut = string.Empty;
                                        string strFamilyIdOut = string.Empty;
                                        string strSSNNOOut = string.Empty;
                                        string strErrorMsg = string.Empty;
                                        CaseMstEntity casemstdata = new CaseMstEntity();
                                        casemstdata.ApplAgency = BaseForm.BaseAgency;
                                        casemstdata.ApplDept = BaseForm.BaseDept;
                                        casemstdata.ApplProgram = BaseForm.BaseProg;
                                        casemstdata.ApplYr = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
                                        casemstdata.ApplNo = Ent.Mst_App;
                                        casemstdata.Mode = "SITE";
                                        casemstdata.Site = Txt_Site.Text;
                                        CheckHistoryTableData(Ent.Mst_Site.Trim(), Txt_Site.Text.Trim());

                                        if (_model.CaseMstData.InsertUpdateCaseMst(casemstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg))
                                        {
                                        }
                                    }
                                }
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            AlertBox.Show("Saved Successfully");
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    AlertBox.Show("Please Select atleast One Record", MessageBoxIcon.Warning);
                }
            }
        }


        private void CheckHistoryTableData(string strOldSite, string strNewSite)
        {
            string strHistoryDetails = "<XmlHistory>";
            bool boolHistory = false;
            if (strOldSite != strNewSite)
            {
                boolHistory = true;
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Site (Enrollment/Withdrawals) </FieldName><OldValue>" + strOldSite + "</OldValue><NewValue>" + strNewSite + "</NewValue></HistoryFields>";
            }
            strHistoryDetails = strHistoryDetails + "</XmlHistory>";
            if (boolHistory)
            {
                CaseHistEntity caseHistEntity = new CaseHistEntity();
                caseHistEntity.HistTblName = "CASEMST";
                caseHistEntity.HistScreen = "CASE2001";
                caseHistEntity.HistSubScr = "Intake";
                caseHistEntity.HistTblKey = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + BaseForm.BaseApplicationNo + BaseForm.BaseCaseMstListEntity[0].FamilySeq;
                caseHistEntity.LstcOperator = BaseForm.UserID;
                caseHistEntity.HistChanges = strHistoryDetails;
                _model.CaseMstData.InsertCaseHist(caseHistEntity);
            }

        }
        private bool Validate_Add_NoStatus_Controls()
        {
            bool Can_Save = true;

            if (((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() == "0")// && Mode == "Add_FUND")
            {
                _errorProvider.SetError(Cmb_NoStat_Fund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label11.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_NoStat_Fund, null);


            if (!Fund_Enroll_Date.Checked)
            {
                _errorProvider.SetError(Fund_Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label24.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Fund_Enroll_Date, null);


            if (((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() == "00")// && Mode == "Add_FUND")
            {
                _errorProvider.SetError(Cmb_Add_Fund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label12.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_Add_Fund, null);

            if (((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString() == "0" && ((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {
                _errorProvider.SetError(Cmb_Fund_Category, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label19.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_Fund_Category, null);

            if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {
                if (!Fund_End_Date.Checked)
                {
                    _errorProvider.SetError(Fund_End_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label16.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(Fund_End_Date, null);

                if (!Rate_Eff_Date.Checked)
                {
                    _errorProvider.SetError(Rate_Eff_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label21.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                //else
                //{
                //    if (Convert.ToDateTime(Class_Start_Date) > Rate_Eff_Date.Value || Convert.ToDateTime(Class_End_Date) < Rate_Eff_Date.Value)
                //    {
                //        _errorProvider.SetError(Rate_Eff_Date, label21.Text + " is not within class dates \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                //        Can_Save = false;
                //    }
                //    else
                //        _errorProvider.SetError(Rate_Eff_Date, null);
                //}
            }

            if (((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() != "L" && ((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() != "A" && ((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() != "B" && ((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() != "I")
            {
                if (Txt_Room.Text.Trim() == "****" || string.IsNullOrEmpty(Txt_AMPM.Text.Trim()))
                {
                    _errorProvider.SetError(Pb_Site_Search, "Please Select Specific Class");
                    Can_Save = false;
                }
            }
            else
                _errorProvider.SetError(Pb_Site_Search, null);

            return Can_Save;
        }

        private void Cmb_NoStat_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)Cmb_NoStat_Fund.SelectedItem).Value.ToString() == "0")// && Mode == "Add_FUND")
            { label13.Text = "Create Status Record"; Btn_NoStatus_Add.Text = "&Create Status Record(s)"; }// "Create " + ((ListItem)Cmb_NoStat_Fund.SelectedItem).Text.ToString() + " Record";
            else
            { label13.Text = "Create " + ((ListItem)Cmb_NoStat_Fund.SelectedItem).Text.ToString() + " Record"; Btn_NoStatus_Add.Text = "&Create " + ((ListItem)Cmb_NoStat_Fund.SelectedItem).Text.ToString() + " Record(s)"; }
        }

        private void CASE0010_CM_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "Pb_Add_Help")
            {

            }
            else if (e.Tool.Name == "pb_Add_help2")
            {
            }
        }

        private void Grid_Applications_Click(object sender, EventArgs e)
        {
            //SelectedgvRows = (from c in Grid_Applications.Rows.Cast<DataGridViewRow>().ToList()
            //                  where (((DataGridViewCheckBoxCell)c.Cells["GD_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
            //                  select c).ToList();

            //if (Btn_Calu_Dates.Visible == true)
            //{
            //    if (SelectedgvRows.Count > 0)
            //    {
            //        Btn_Calu_Dates.Enabled = true;
            //    }
            //    else
            //    {
            //        Btn_Calu_Dates.Enabled = false;
            //    }
            //}

            //if (Btn_PostIntake.Visible == true)
            //{
            //    if (SelectedgvRows.Count > 0)
            //    {
            //        Btn_PostIntake.Enabled = true;
            //    }
            //    else
            //    {
            //        Btn_PostIntake.Enabled = false;
            //    }
            //}


        }
        List<DataGridViewRow> SelectedgvRows;
        private void Btn_Calu_Dates_Click(object sender, EventArgs e)
        {
            if (Validate_Add_NoStatus_Controls())
            {
                if (!Validate_Fund_For_Add_Apps())
                {
                    AlertBox.Show(Error_MSG, MessageBoxIcon.Warning);
                    return;
                }

                Btn_NoStatus_Add.Visible = true;
                Btn_Calu_Dates.Visible = false;
                foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                {
                    Entity.Site = Txt_Site.Text.Trim();
                    Entity.Room = Txt_Room.Text.Trim();
                    if(!string.IsNullOrEmpty(Txt_AMPM.Text.Trim()))
                        Entity.AMPM = Txt_AMPM.Text.Trim().Substring(0, 1);

                    Entity.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
                }

                SelectedgvRows = (from c in Grid_Applications.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["GD_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    foreach (DataGridViewRow dr in Grid_Applications.Rows)
                    {
                        string room = string.Empty;
                        if (dr["GD_Sel"].Value.ToString() == "True")
                        {
                            if (!string.IsNullOrEmpty(Txt_AMPM.Text.Trim()))
                                room = Txt_AMPM.Text.Trim().Substring(0, 1);

                            dr["GD_Program"].Value = Txt_Site.Text.Trim() + "/" + Txt_Room.Text.Trim() + "/" + room;
                            dr["GD_Fund"].Value = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
                            dr["GD_Date"].Value = Fund_Enroll_Date.Text.Trim();
                        }
                    }
                }
                else
                {
                    AlertBox.Show("Please select atleast One Record", MessageBoxIcon.Warning);
                    Btn_Calu_Dates.Visible = true;
                    Btn_NoStatus_Add.Visible = false;
                }
                fillStatusDate();
            }
        }

        private void Fund_Enroll_Date_ValueChanged(object sender, EventArgs e)
        {
            Btn_NoStatus_Add.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Txt_Site_TextChanged(object sender, EventArgs e)
        {
            Btn_NoStatus_Add.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Txt_Room_TextChanged(object sender, EventArgs e)
        {
            Btn_NoStatus_Add.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Txt_AMPM_TextChanged(object sender, EventArgs e)
        {
            Btn_NoStatus_Add.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Cmb_Add_Fund_SelectedValueChanged(object sender, EventArgs e)
        {
            Btn_NoStatus_Add.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Btn_CM_NoStatus_Add_Click(object sender, EventArgs e)
        {
            if (Validate_CM_Add_NoStatus_Controls())
            {
                bool Save_Result = false;
                CaseEnrlEntity New_Post_Record;

                int NooF_Rows_Processed = 0;
                string Tmp_Status = "";
                New_Post_Record = new CaseEnrlEntity(true);

                StringBuilder Xml_To_Pass = new StringBuilder();
                StringBuilder Sum_Xml_To_Pass = new StringBuilder();
                Xml_To_Pass.Append("<Rows>");
                Sum_Xml_To_Pass.Append("<Rows>");

                New_Post_Record.Row_Type = "I";
                New_Post_Record.Agy = BaseForm.BaseAgency;
                New_Post_Record.Dept = BaseForm.BaseDept;
                New_Post_Record.Prog = BaseForm.BaseProg;
                New_Post_Record.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");

                New_Post_Record.ID = "1";
                New_Post_Record.Status_Date = New_Post_Record.Enrl_Date = CM_NoStat_Date.Value.ToShortDateString();
                New_Post_Record.App = Pass_Enroll_List[0].App;

                New_Post_Record.Group = Pass_Enroll_List[0].Group;
                //New_Post_Record.Enrl_Date = string.Empty;
                New_Post_Record.Seq = "1";
                New_Post_Record.Desc_1 = txtNote_Desc1.Text;
                New_Post_Record.Desc_2 = txtNote_Desc2.Text;

                New_Post_Record.FundHie = Sel_Hie;
                New_Post_Record.Rec_Type = "C";
                Tmp_Status = New_Post_Record.Status = ((ListItem)Cmb_CM_NoStat_Status.SelectedItem).Value.ToString();
                New_Post_Record.Lstc_Oper = BaseForm.UserID;

                foreach (CaseEnrlEntity Ent in Pass_Enroll_List)
                {
                    {
                        New_Post_Record.App = Ent.Mst_App;
                        New_Post_Record.Seq = "1";

                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Ent.Mst_App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Sel_Hie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + null +
                                       "\" Enrl_ROOM = \"" + null + "\" Enrl_AMPM = \"" + null +
                                       "\" ENRL_ENRLD_DATE = \"" + New_Post_Record.Status_Date + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                       "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                       "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                       "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Tmp_Status + "\" Sum_Key_To_Update= \"" + " " +
                                       "\" Mst_Fam_Seq= \"" + "" + "\"/>");

                    }
                }

                Xml_To_Pass.Append("</Rows>");
                Sum_Xml_To_Pass.Append("</Rows>");


                bool save_result = false;
                if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", Xml_To_Pass.ToString(), string.Empty, null, out Sql_SP_Result_Message))
                {
                    save_result = true;
                    AlertBox.Show("PostIntake Created Successfully");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);

            }
        }

        private bool Validate_CM_Add_NoStatus_Controls()
        {
            bool Can_Save = true;

            if (!CM_NoStat_Date.Checked)
            {
                _errorProvider.SetError(CM_NoStat_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label27.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(CM_NoStat_Date, null);

            return Can_Save;
        }

        private void Cmb_CM_NoStat_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = Btn_CM_NoStatus_Add.Text = "Create " + ((ListItem)Cmb_CM_NoStat_Status.SelectedItem).Text.ToString() + " Record";
        }




    }
}