/************************************************************************
 * Conversion On    :   12/14/2022      * Converted By     :   Kranthi
 * Modified On      :   12/14/2022      * Modified By      :   Kranthi
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
using Captain.DatabaseLayer;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0010_CASE_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        /************************************************
         * Transfer_Panel | Add_Fund_Panel block
         * **********************************************/
        public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, List<CaseEnrlEntity> pass_enroll_list, string sel_hie, string sel_prog_name, string site, string class_start_Date, string class_end_Date, string Attn_first_date, string Attn_last_date, string added_funds_List, string room, string ampm)
        {
            InitializeComponent();

            propTransferSiteSwitch = string.Empty;
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = mode;
            Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
            Sel_Prog_Name = sel_prog_name.Trim();
            Pass_Enroll_List = pass_enroll_list;
            Site = site;
            Class_Start_Date = class_start_Date;
            Class_End_Date = class_end_Date;
            Attn_Last_Date = Attn_last_date;
            Attn_First_Date = Attn_first_date;
            Added_Funds_List = added_funds_List;
            Room = room;
            AmPm = ampm;
            Pass_Enroll_Entity = new CaseEnrlEntity();

            this.Text = Privileges.PrivilegeName + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            if (Privileges.ModuleCode == "02")
                Get_HSS_Sites();
            else
                Get_CM_DepEnrl_Hierarchies();


            Txt_Parent_Rate.Validator = Txt_Fund_Rate.Validator = TextBoxValidation.FloatValidator;

            switch (Mode)
            {
                case "Add": Launch_NonEnroll_Panel(); break;
                case "Edit": Launch_Enroll_Panel(); ; break;

                case "Add_FUND":
                    this.Text = "Add Fund";//Privileges.Program + " - Add Fund";
                    Fill_Add_Fund_APP(); ; break;
                case "Edit_FUND":
                    this.Text = "Edit Fund Date/Details";//Privileges.Program + " - Edit Fund";
                    Fill_Edit_Fund_APP(); ; break;
                case "Transfer":
                    this.Text = "Transfer";//Privileges.Program + " - Transfer";
                    if (Pass_Enroll_List.Count == 1 && Pass_Enroll_List[0].Status == "L")
                        label33.Visible = Rb_Trans_All_Yes.Visible = Rb_Trans_All_No.Visible = false;
                    Fill_Transfer_Applicant(); ; break;
            }
        }

        /// <summary>
        /// Transfer switch Mst site update form logic
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="privileges"></param>
        /// <param name="mode"></param>
        /// <param name="pass_enroll_list"></param>
        /// <param name="sel_hie"></param>
        /// <param name="sel_prog_name"></param>
        /// <param name="site"></param>
        /// <param name="class_start_Date"></param>
        /// <param name="class_end_Date"></param>
        /// <param name="Attn_last_date"></param>
        /// <param name="added_funds_List"></param>
        /// <param name="room"></param>
        /// <param name="ampm"></param>
        public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, List<CaseEnrlEntity> pass_enroll_list, string sel_hie, string sel_prog_name, string site, string class_start_Date, string class_end_Date, string Attn_first_date, string Attn_last_date, string added_funds_List, string room, string ampm, string strTranferSwitch)
        {
            InitializeComponent();
            propTransferSiteSwitch = strTranferSwitch;
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = mode;
            Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
            Sel_Prog_Name = sel_prog_name.Trim();
            Pass_Enroll_List = pass_enroll_list;
            Site = site;
            Class_Start_Date = class_start_Date;
            Class_End_Date = class_end_Date;
            Attn_Last_Date = Attn_last_date;
            Attn_First_Date = Attn_first_date;
            Added_Funds_List = added_funds_List;
            Room = room;
            AmPm = ampm;
            Pass_Enroll_Entity = new CaseEnrlEntity();

            this.Text = Privileges.Program + " - " + Mode;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            if (Privileges.ModuleCode == "02")
                Get_HSS_Sites();
            else
                Get_CM_DepEnrl_Hierarchies();


            Txt_Parent_Rate.Validator = Txt_Fund_Rate.Validator = TextBoxValidation.FloatValidator;

            switch (Mode)
            {
                case "Add": Launch_NonEnroll_Panel(); break;
                case "Edit": Launch_Enroll_Panel(); ; break;

                case "Add_FUND":
                    this.Text = Privileges.Program + " - Add Fund";
                    Fill_Add_Fund_APP(); ; break;
                case "Edit_FUND":
                    this.Text = Privileges.Program + " - Edit Fund";
                    Fill_Edit_Fund_APP(); ; break;
                case "Transfer":
                    this.Text = Privileges.Program + " - Transfer";
                    if (Pass_Enroll_List.Count == 1 && Pass_Enroll_List[0].Status == "L")
                        label33.Visible = Rb_Trans_All_Yes.Visible = Rb_Trans_All_No.Visible = false;
                    Fill_Transfer_Applicant();
                    label27.Visible = false;
                    label28.Visible = false;
                    label29.Visible = false;
                    label30.Visible = false;
                    label31.Visible = false;
                    label32.Visible = false;
                    label33.Visible = false;
                    panel9.Visible = false;
                    Txt_Trans_Room.Visible = false;
                    Txt_Trans_AMPM.Visible = false;
                    Cmb_Trans_Reason.Visible = false;
                    Rb_Trans_All_No.Visible = false;
                    Rb_Trans_All_Yes.Visible = false;
                    Transfer_Date.Visible = false;
                    ; break;
            }
        }


        //public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, CaseEnrlEntity pass_Enroll_Entity, string sel_hie, string sel_prog_name, string site, string class_start_Date, string class_end_Date, string Attn_last_date, string added_funds_List)

        /*****************
         * Stat_DblHist_Panel block
         * **************************/
        public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string module_Code, CaseEnrlEntity pass_Enroll_Entity)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privileges;
            Mode = string.Empty;
            Module_Code = module_Code;
            //Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
            //Sel_Prog_Name = sel_prog_name.Trim();
            Pass_Enroll_Entity = pass_Enroll_Entity;
            //Site = site;
            //Class_Start_Date = class_start_Date;
            //Class_End_Date = class_end_Date;
            //Attn_Last_Date = Attn_last_date;
            //Added_Funds_List = added_funds_List;

            _model = new CaptainModel();
            label36.Text = Pass_Enroll_Entity.App;

            if (Module_Code == "02")
                label38.Text = Get_Fund_Description(Pass_Enroll_Entity.FundHie);
            else
            {
                //this.groupBox1.Location = new System.Drawing.Point(11, 35);
                //this.groupBox2.Location = new System.Drawing.Point(260, 35);

                label39.Visible = groupBox3.Visible = label38.Visible = false;
                SHistChg_Attn_From.HeaderText = SHistChg_Attn_To.HeaderText = "  ";
                SHistChg_Class.HeaderText = "Program";
                label54.Text = label53.Text = "Program hierarchy";
            }

            this.Text = "Edit Status History";
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;



            //this.Stat_DblHist_Panel.Location = new System.Drawing.Point(1, 1);
            //this.Stat_DblHist_Panel.Size = new System.Drawing.Size(528, 275);
            //this.SDbl_Status_Grid.Size = new System.Drawing.Size(519, 189);

            /**********************************************************************************************/
            Stat_DblHist_Panel.Visible = true;
            Stat_DblHist_Panel3.Visible = false;
            //this.Size = new System.Drawing.Size(531, 278);
            this.Size = new Size(this.Width - 200, this.Height - (PI_Panle.Height + Transfer_Panel.Height + Add_Fund_Panel.Height + Enroll_Panel.Height +
                Counts_Panel.Height + Stat_DblHist_Panel3.Height + Stat_DblHist_Edit_Panel.Height + Stat_DblHist_Save_Panel.Height)); //+ groupBox3.Height + pnlchangeGroup.Height

            /**********************************************************************************************/


            Get_HSS_Sites();
            Get_Applicant_Attn_Dates();
            Fill_Status_History();

            //switch (Mode)
            //{
            //    case "Add": Launch_NonEnroll_Panel(); break;
            //    case "Edit": Launch_Enroll_Panel(); ; break;

            //    case "Add_FUND": this.Text = Privileges.Program + " - Add Fund";
            //        Fill_Add_Fund_APP(); ; break;
            //    case "Edit_FUND": this.Text = Privileges.Program + " - Edit Fund";
            //        Fill_Edit_Fund_APP(); ; break;
            //    case "Transfer": this.Text = Privileges.Program + " - Transfer";
            //        Fill_Transfer_Applicant(); ; break;
            //}
        }


        //public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, CaseEnrlEntity pass_Enroll_Entity, string sel_hie, string sel_prog_name, string site, string class_start_Date, string class_end_Date, string Attn_last_date, string added_funds_List)
        /****************************************************
         * Counts_Panle Block
         * *******************************************************/
        public CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, List<RoomCount_Entity> RoomRace_list, List<RoomCount_Entity> RoomCounts_list, List<RoomCount_Entity> RoomEthnic_list, List<RoomCount_Entity> RoomFunding_list, List<RoomCount_Entity> RoomTeachers_list, List<RoomCount_Entity> RoomClassfication_list)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privileges;
            Mode = string.Empty;
            //Module_Code = module_Code;
            //Sel_Hie = (sel_hie.Length == 6 ? sel_hie + "    " : sel_hie);
            //Sel_Prog_Name = sel_prog_name.Trim();
            //Pass_Enroll_Entity = pass_Enroll_Entity;
            //Site = site;
            //Class_Start_Date = class_start_Date;
            //Class_End_Date = class_end_Date;
            //Attn_Last_Date = Attn_last_date;
            //Added_Funds_List = added_funds_List;

            RoomRace_List = RoomRace_list;
            RoomCounts_List = RoomCounts_list;
            RoomEthnic_List = RoomEthnic_list;
            RoomFunding_List = RoomFunding_list;
            RoomTeachers_List = RoomTeachers_list;
            RoomClassfy_List = RoomClassfication_list;

            _model = new CaptainModel();
            //label36.Text = Pass_Enroll_Entity.App;

            this.Text = "Site/Room Counts";
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            // this.Size = new System.Drawing.Size(435, 433);
            // this.Counts_Panel.Location = new System.Drawing.Point(2, 1);
            /**********************************************************************************************/
            Counts_Panel.Visible = true;
            this.Size = new Size(this.Width - 200, this.Height - (PI_Panle.Height + Stat_DblHist_Panel.Height + Add_Fund_Panel.Height + Enroll_Panel.Height + Add_Fund_Panel.Height));
            /**********************************************************************************************/


            Get_Fund_Desc_List();
            Fill_All_Counts_Grids();
        }

        List<DepEnrollHierachiesEntity> DepEnrollList = new List<DepEnrollHierachiesEntity>();
        private void Get_CM_DepEnrl_Hierarchies()
        {
            DepEnrollList = _model.HierarchyAndPrograms.GetDepEntollHierachies(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);
        }

        List<SPCommonEntity> RoomCnt_FundDesc_List = new List<SPCommonEntity>();
        private void Get_Fund_Desc_List()
        {
            RoomCnt_FundDesc_List = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
        }

        private void Fill_All_Counts_Grids()
        {
            //RoomRace_List = RoomRace_list;
            //RoomFunding_List = RoomFunding_list;
            //RoomTeachers_List = RoomTeachers_list;

            int Tmp_Loop_Cnt = 0;
            string Priv_Desc = string.Empty, Priv_Cnt = string.Empty;

            foreach (RoomCount_Entity Entity in RoomTeachers_List)
                Teachers_Grid.Rows.Add(Entity.Desc, Entity.Count);

            foreach (RoomCount_Entity Entity in RoomEthnic_List)
            {
                if (Tmp_Loop_Cnt % 2 != 0)
                    Ethnic_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", Entity.Desc, Entity.Count);
                else
                { Priv_Desc = Entity.Desc; Priv_Cnt = Entity.Count; }

                Tmp_Loop_Cnt++;
            }
            if (RoomEthnic_List.Count % 2 == 1)
                Ethnic_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", " ", " ");

            Tmp_Loop_Cnt = 0;
            foreach (RoomCount_Entity Entity in RoomFunding_List)
                Funding_Cnt_Grid.Rows.Add(Entity.Code, Get_Funding_Desc(Entity.Desc));

            Tmp_Loop_Cnt = 0;
            foreach (RoomCount_Entity Entity in RoomRace_List)
            {
                if (Tmp_Loop_Cnt % 2 != 0)
                    Race_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", Entity.Desc, Entity.Count);
                else
                { Priv_Desc = Entity.Desc; Priv_Cnt = Entity.Count; }

                Tmp_Loop_Cnt++;
            }
            if (RoomRace_List.Count % 2 == 1)
                Race_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", " ", " ");


            Tmp_Loop_Cnt = 0;
            foreach (RoomCount_Entity Entity in RoomCounts_List)
            {

                if (Tmp_Loop_Cnt % 2 != 0)
                    Room_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", Entity.Desc, Entity.Count);
                else
                { Priv_Desc = Entity.Desc; Priv_Cnt = Entity.Count; }

                Tmp_Loop_Cnt++;
            }

            foreach (RoomCount_Entity Entity in RoomClassfy_List)
            {
                if (Tmp_Loop_Cnt % 2 != 0)
                    Classification_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", Entity.Desc, Entity.Count);
                else
                { Priv_Desc = Entity.Desc; Priv_Cnt = Entity.Count; }

                Tmp_Loop_Cnt++;
            }
            if (RoomClassfy_List.Count % 2 == 1)
                Classification_Cnt_Grid.Rows.Add(Priv_Desc, Priv_Cnt, " ", " ", " ");

        }

        private string Get_Funding_Desc(string Fund_Code)
        {
            string Return_Desc = Fund_Code;
            foreach (SPCommonEntity Entity in RoomCnt_FundDesc_List)
            {
                if (Entity.Code == Fund_Code)
                {
                    Return_Desc = Entity.Desc;
                }
            }

            return Return_Desc;
        }

        private string Get_Fund_Description(string Fund_Code)
        {
            string Fund_Desc = string.Empty;
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
            foreach (SPCommonEntity Entity in FundingList)
            {
                if (Entity.Code == Fund_Code)
                {
                    Fund_Desc = Entity.Desc;
                    break;
                }
            }
            return Fund_Desc;
        }


        List<ChldAttnEntity> App_Attn_List = new List<ChldAttnEntity>();
        private void Get_Applicant_Attn_Dates()
        {
            App_Attn_List = _model.ChldAttnData.GetChldAttnDetails(Pass_Enroll_Entity.Agy, Pass_Enroll_Entity.Dept, Pass_Enroll_Entity.Prog, Pass_Enroll_Entity.Prog, Pass_Enroll_Entity.App, Pass_Enroll_Entity.Site, Pass_Enroll_Entity.Room, Pass_Enroll_Entity.AMPM, Pass_Enroll_Entity.FundHie, "Browse");
        }

        List<ENRLHIST_Entity> ENRLHIST_List = new List<ENRLHIST_Entity>();
        int Status_Grid_Cnt = 0;
        private void Fill_Status_History()
        {
            ENRLHIST_Entity Search_Entity = new ENRLHIST_Entity(true);
            Search_Entity.ID = Pass_Enroll_Entity.ID;
            Search_Entity.Asof_Date = "N";
            ENRLHIST_List = _model.EnrollData.Browse_ENRLHIST(Search_Entity, "Browse");

            SDbl_Status_Grid.Rows.Clear();
            string From_Date = " ", To_Date = " ", Add_Date = " ", Status_Desc = " ",
                Attn_From_Date = " ", Attn_To_Date = " ", Class_Prog = " ";
            int Row_Index = 0; Status_Grid_Cnt = 0;
            foreach (ENRLHIST_Entity Entity in ENRLHIST_List)
            {
                From_Date = To_Date = Add_Date = Status_Desc =
                Attn_From_Date = Attn_To_Date = Class_Prog = " ";

                if (Entity.ID == Pass_Enroll_Entity.ID)
                {
                    From_Date = LookupDataAccess.Getdate(Entity.From_Date);
                    To_Date = LookupDataAccess.Getdate(Entity.TO_Date);
                    Add_Date = LookupDataAccess.Getdate(Entity.Date_Add);

                    Attn_From_Date = LookupDataAccess.Getdate(Entity.Attn_First_Date);
                    Attn_To_Date = LookupDataAccess.Getdate(Entity.Attn_LAST_Date);

                    switch (Entity.Status)
                    {
                        case "L": Status_Desc = "Wait List"; break;
                        case "P": Status_Desc = "Pending"; break;
                        //case "R": Status_Desc = "Denied"; break;
                        case "E": Status_Desc = "Enrolled"; break;
                        case "W": Status_Desc = "Withdrawn"; break;
                        case "I": Status_Desc = "Post Intake"; break;
                        case "N": Status_Desc = "Inactive"; break;
                        case "C": Status_Desc = "Accepted"; break;
                    }

                    if (Module_Code == "02")
                        Class_Prog = (Entity.Site + (!string.IsNullOrEmpty(Entity.Room.Trim()) ? ("/" + Entity.Room) : "") + (!string.IsNullOrEmpty(Entity.AMPM.Trim()) ? ("/" + Entity.AMPM) : ""));
                    else
                        //Class_Prog = Pass_Enroll_Entity.FundHie;
                        Class_Prog = (Entity.Site + (!string.IsNullOrEmpty(Entity.Room.Trim()) ? ("-" + Entity.Room) : "") + (!string.IsNullOrEmpty(Entity.AMPM.Trim()) ? ("-" + Entity.AMPM) : ""));

                    Row_Index = SDbl_Status_Grid.Rows.Add(false, Status_Desc, From_Date, To_Date, Class_Prog, " ", Attn_From_Date, Attn_To_Date, Entity.Seq);
                    Status_Grid_Cnt++;
                }
            }


            if (Status_Grid_Cnt > 0)
            {
                SDbl_Status_Grid.CurrentCell = SDbl_Status_Grid.Rows[rowIndex].Cells[1];
                SDbl_Status_Grid.Rows[rowIndex].Selected = true;
                Pb_Edit.Visible = PbDelete.Visible = true;
            }
            else
                Pb_Edit.Visible = PbDelete.Visible = false;
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Module_Code { get; set; }

        public string Sel_Hie { get; set; }

        public string Sel_Prog_Name { get; set; }

        public string Attn_Last_Date { get; set; }

        public string Attn_First_Date { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<CaseEnrlEntity> Pass_Enroll_List { get; set; }

        public CaseEnrlEntity Pass_Enroll_Entity { get; set; }

        public string Site { get; set; }

        public string Class_Start_Date { get; set; }

        public string Class_End_Date { get; set; }

        public string Added_Funds_List { get; set; }

        public string Room { get; set; }

        public string AmPm { get; set; }

        public List<RoomCount_Entity> RoomRace_List { get; set; }

        public List<RoomCount_Entity> RoomCounts_List { get; set; }

        public List<RoomCount_Entity> RoomEthnic_List { get; set; }

        public List<RoomCount_Entity> RoomFunding_List { get; set; }

        public List<RoomCount_Entity> RoomTeachers_List { get; set; }

        public List<RoomCount_Entity> RoomClassfy_List { get; set; }

        public string propTransferSiteSwitch { get; set; }
        #endregion

        string Img_Blank = Consts.Icons.ico_Blank; // new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        string Img_Tick = Consts.Icons.ico_Tick; //new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");



        private void Launch_NonEnroll_Panel()
        {
            this.Size = new System.Drawing.Size(423, 457);

            switch (Privileges.ModuleCode)
            {
                case "02":
                    this.Text = Btn_PostIntake.Text = "Send To Wait List";
                    this.Fund_Panel.Size = new System.Drawing.Size(410, 30);
                    this.CaseMst_Panel.Size = new System.Drawing.Size(413, 357);
                    this.GD_PostIntake.Size = new System.Drawing.Size(408, 348);
                    Fund_Panel.Visible = true;
                    if (Room == "****" && string.IsNullOrEmpty(AmPm.Trim()))
                    {
                        Rb_HSS_NonEnrl.Visible = Rb_HSS_RefSum.Visible = false;
                        this.Cmb_Fund.Size = new System.Drawing.Size(365, 21);
                    }

                    Fill_Fund_Combo();
                    break;
                case "03":
                    this.Case_RB_Panel.Location = new System.Drawing.Point(3, 30);
                    this.Case_RB_Panel.Size = new System.Drawing.Size(409, 27);
                    //this.CaseMst_Panel.Location = new System.Drawing.Point(3, 30);
                    this.CaseMst_Panel.Size = new System.Drawing.Size(408, 357);
                    this.GD_PostIntake.Size = new System.Drawing.Size(408, 348);
                    //this.Lbl_Ref_SUM.Location = new System.Drawing.Point(2, 190);
                    //this.GD_PostIntake.Size = new System.Drawing.Size(408, 376);
                    //CaseSum_Panel.Visible = Lbl_Ref_SUM.Visible = true;
                    Case_RB_Panel.Visible = true;

                    ////this.CaseMst_Panel.Location = new System.Drawing.Point(3, 30);
                    //this.CaseMst_Panel.Size = new System.Drawing.Size(408, 230);
                    //this.GD_PostIntake.Size = new System.Drawing.Size(408, 185);
                    //this.Lbl_Ref_SUM.Location = new System.Drawing.Point(2, 190);
                    ////this.GD_PostIntake.Size = new System.Drawing.Size(408, 376);
                    //CaseSum_Panel.Visible = Lbl_Ref_SUM.Visible = true;
                    break;

            }


            PI_Panle.Visible = true;
            Fill_Non_Enroll_Grid();
        }

        private void Fill_Fund_Combo()
        {
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            //FundingList = _model.SPAdminData.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");

            Cmb_Add_Fund.Items.Clear();
            Cmb_Add_Fund.Items.Add(new ListItem(" ", "00", " ", (true ? Color.Green : Color.Red)));
            switch (Mode)
            {

                case "Add_FUND":
                    foreach (SPCommonEntity Entity in FundingList)
                    {
                        //if (Pass_Enroll_List[0].FundHie.Trim() != Entity.Code && !string.IsNullOrEmpty(Entity.Ext.Trim()))

                        //if ((((!Added_Funds_List.Contains(" " + Entity.Code)) && (Pass_Enroll_List[0].Status == "E" || Pass_Enroll_List[0].Status == "W"))) && !string.IsNullOrEmpty(Entity.Ext.Trim()))
                        if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                            Cmb_Add_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                    }

                    if (Cmb_Add_Fund.Items.Count > 0)
                        Cmb_Add_Fund.SelectedIndex = 0;

                    break;

                case "Edit_FUND":
                    foreach (SPCommonEntity Entity in FundingList)
                    {
                        if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                            Cmb_Add_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                    }

                    //if (Cmb_Add_Fund.Items.Count > 0)
                    //    Cmb_Add_Fund.SelectedIndex = 0;

                    break;

                case "Transfer":
                    Cmb_Trans_Fund.Items.Clear();
                    foreach (SPCommonEntity Entity in FundingList)
                    {
                        if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                            Cmb_Trans_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                    }

                    break;


                default:
                    Cmb_Fund.Items.Clear();
                    foreach (SPCommonEntity Entity in FundingList)
                    {
                        if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                            Cmb_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                    }

                    if (Cmb_Fund.Items.Count > 0)
                        Cmb_Fund.SelectedIndex = 0;

                    break;

            }
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

        private void Fill_Fund_Transfer_Category()
        {
            Cmb_Trf_FundCat.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));
            listItem.Add(new ListItem("Full", "F"));
            listItem.Add(new ListItem("3/4 ", "3"));
            listItem.Add(new ListItem("1/2 ", "2"));
            listItem.Add(new ListItem("1/4", "1"));
            Cmb_Trf_FundCat.Items.AddRange(listItem.ToArray());
            Cmb_Trf_FundCat.SelectedIndex = 0;
        }


        List<SPCommonEntity> ReasonDesc_List = new List<SPCommonEntity>();
        private void Fill_Fund_Reasons_Combo()
        {
            // Filling Reason Combo
            Cmb_Reason.Items.Clear();
            ReasonDesc_List = _model.SPAdminData.Get_AgyRecs(Consts.AgyTab.Enroll_Reasons);
            List<ListItem> listItem = new List<ListItem>();

            listItem.Add(new ListItem("  ", "0"));
            foreach (SPCommonEntity Entity in ReasonDesc_List)
                listItem.Add(new ListItem(Entity.Desc, Entity.Code));
            Cmb_Reason.Items.AddRange(listItem.ToArray());
            Cmb_Reason.SelectedIndex = 0;
        }


        List<SPCommonEntity> Edit_ReasonDesc_List = new List<SPCommonEntity>();
        private void Fill_Fund_Edit_Reason_Combo()
        {
            // Filling Reason Combo
            Edit_ReasonDesc_List = _model.SPAdminData.Get_AgyRecs(Consts.AgyTab.HSREASONWITHDRWL);
            Edit_ReasonDesc_List = Edit_ReasonDesc_List.FindAll(u => u.Active.Equals("Y"));
            List<ListItem> listItem = new List<ListItem>();

            listItem.Add(new ListItem("  ", "0"));
            switch (Mode)
            {
                case "Edit_FUND":
                    Cmb_Edit_Reason.Items.Clear();
                    foreach (SPCommonEntity Entity in Edit_ReasonDesc_List)
                        listItem.Add(new ListItem(Entity.Desc, Entity.Code));
                    Cmb_Edit_Reason.Items.AddRange(listItem.ToArray());
                    Cmb_Edit_Reason.SelectedIndex = 0;
                    if (Pass_Enroll_List[0].Status == "W")
                        Cmb_Edit_Reason.Visible = label20.Visible = Lbl_ReasonReq.Visible = true;
                    break;
                case "Transfer":
                    Cmb_Trans_Reason.Items.Clear();
                    foreach (SPCommonEntity Entity in Edit_ReasonDesc_List)
                        listItem.Add(new ListItem(Entity.Desc, Entity.Code));
                    Cmb_Trans_Reason.Items.AddRange(listItem.ToArray());
                    Cmb_Trans_Reason.SelectedIndex = 0;
                    break;
            }
        }

        List<CaseSumEntity> CaseSum_List = new List<CaseSumEntity>();
        List<MainMenuEntity> Base_MainMenu_List = new List<MainMenuEntity>();
        List<MainMenuEntity> Base99_MainMenu_List = new List<MainMenuEntity>();
        private void Fill_Non_Enroll_Grid()
        {
            //DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), null, BaseForm.UserID);
            Base_MainMenu_List = _model.MainMenuData.GetMainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), null, BaseForm.UserID, string.Empty);
            //DataSet ds_99 = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null, "999999    ", null, BaseForm.UserID);

            if (Privileges.ModuleCode == "02")
                Fill_NonEnrolls_On_FundChange();
            else
            {
                if (Rb_From_Mst.Checked)
                    Fill_NonEnrolls_From_CASEMST();
                else
                    Fill_NonEnrolls_From_CASESUM();
            }


            //string test = " ";
            //if (ds.Tables.Count > 0)
            //{
            //    DataTable dt = ds.Tables[0];

            //    DataTable dt99 = new DataTable();
            //    if (ds_99.Tables.Count > 0)
            //        dt99 = ds_99.Tables[0];

            //    if (dt.Rows.Count > 0)
            //    {
            //        DataRow dr_Mst = dt.Rows[0];
            //        GD_PostIntake.Rows.Clear();
            //        try
            //        {
            //            CaseEnrlEntity Insert_Entity = new CaseEnrlEntity(true);
            //            CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
            //            CaseEnrlEntity Tmp_EnrlEnt = new CaseEnrlEntity(true);
            //            StringBuilder Xml_To_Insert = new StringBuilder();
            //            StringBuilder Xml_HistTo_Pass = new StringBuilder();
            //            StringBuilder Xml_To_Update = new StringBuilder();
            //            Xml_To_Insert.Append("<Rows>");
            //            Xml_To_Update.Append("<Rows>");
            //            Xml_HistTo_Pass.Append("<Rows>");

            //            Insert_Entity.Row_Type = Insert_Entity.Status = "I";
            //            Insert_Entity.Agy = BaseForm.BaseAgency;
            //            Insert_Entity.Dept = BaseForm.BaseDept;
            //            Insert_Entity.Prog = BaseForm.BaseProg;
            //            Insert_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
            //            Insert_Entity.App = "00000001";
            //            Insert_Entity.FundHie = Sel_Hie;

            //            Insert_Entity.ID = "1";

            //            DateTime Tmp_date = DateTime.Today;
            //            TimeSpan Tmp_time = DateTime.Today.TimeOfDay;
            //            Insert_Entity.Status_Date = (Tmp_date + Tmp_time).ToString();


            //            int TmpRows = 0;
            //            bool App_Exists = false, Sum_App_Exists = false;
            //            int rowIndex = 0;
            //            string TmpName = null, Tmp_Curr_Status = " " ;
            //            string TmpAddress = null, TmpDOB = null, TmpUpdated = null, TmpSsn = null;
            //            int TmpLength = 0;
            //            char TmpSpace = ' ';

            //            bool Module_Filter = false;
            //            if (Privileges.ModuleCode == "02")
            //                Fill_NonEnrolls_On_FundChange();
            //            else
            //                Fill_NonEnrolls_On_FundChange();

            //            ////string FundHie_To_Compare = Module_Filter ? ((ListItem)Cmb_Fund.SelectedItem).Value.ToString() : Sel_Hie.Trim();

            //            ////foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            //            ////{
            //            ////    if (Base_Enty.Site == Site || (!Module_Filter))
            //            ////    {
            //            ////        Fill_NonEnrolls_On_FundChange();
            //            ////        //App_Exists = Sum_App_Exists = false; Tmp_EnrlEnt = new CaseEnrlEntity();
            //            ////        //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //            ////        //{
            //            ////        //    //if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.App == dr["Agency"].ToString() + dr["Agency"].ToString() + dr["Dept"].ToString() + dr["SnpYear"].ToString() + dr["AppNo"].ToString().Substring(0, 8))
            //            ////        //    if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
            //            ////        //        (Entity.FundHie.Trim() == FundHie_To_Compare))
            //            ////        //    {
            //            ////        //        App_Exists = true;
            //            ////        //        Tmp_EnrlEnt = Entity;
            //            ////        //        Tmp_Curr_Status = Entity.Status;
            //            ////        //        break;
            //            ////        //    }
            //            ////        //}

            //            ////        if (!Module_Filter && CaseSum_List.Count > 0)
            //            ////        {
            //            ////            if (!App_Exists || (App_Exists && (string.IsNullOrEmpty(Tmp_Curr_Status.Trim())) || Tmp_Curr_Status == "I"))
            //            ////            {
            //            ////                foreach (DataRow dr99 in dt99.Rows)
            //            ////                {
            //            ////                    if (Base_Enty.Ssn == dr99["Ssn"].ToString())
            //            ////                    {
            //            ////                        Sum_App_Exists = false;
            //            ////                        foreach (CaseSumEntity Ent in CaseSum_List)
            //            ////                        {
            //            ////                            if (Ent.CaseSumRefAgency + Ent.CaseSumRefDept + Ent.CaseSumRefProgram + Ent.CaseSumRefYear.Trim() == Sel_Hie.Trim() &&
            //            ////                                Ent.CaseSumNotInterested != "Y")
            //            ////                            {
            //            ////                                //if (!App_Exists)
            //            ////                                //{
            //            ////                                //    Xml_To_Insert.Append("<Row Enrl_APP_NO = \"" + dr["AppNo"].ToString().Substring(0, 8) + "\" Enrl_GROUP = \"" + BaseForm.BaseAgency + "\" Enrl_FUND_HIE = \"" + Sel_Hie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + null + "\" Enrl_ROOM = \"" + null + "\" Enrl_AMPM = \"" + null +
            //            ////                                //               "\" ENRL_ENRLD_DATE = \"" + null + "\" Enrl_WDRAW_CODE = \"" + null + "\" Enrl_WDRAW_DATE = \"" + null + "\" Enrl_WLIST_DATE = \"" + null + "\" Enrl_DENIED_CODE = \"" + null + "\" Enrl_DENIED_DATE = \"" + null +
            //            ////                                //               "\" Enrl_PENDING_CODE= \"" + null + "\" Enrl_PENDING_DATE= \"" + null + "\" Enrl_RANK= \"" + "0" + "\"  Enrl_RNKCHNG_CODE= \"" + null + "\" Enrl_TRAN_TYPE= \"" + null +
            //            ////                                //               "\" Enrl_TRANSFER_SITE= \"" + null + "\" Enrl_TRANSFER_ROOM= \"" + null + "\" Enrl_TRANSFER_AMPM= \"" + null + "\" Enrl_PARENT_RATE= \"" + null + "\"  Enrl_FUNDING_CODE= \"" + null + "\" Enrl_FUNDING_RATE= \"" + null +
            //            ////                                //               "\" Enrl_FUND_END_DATE= \"" + null + "\" Enrl_RATE_EFF_DATE= \"" + null + "\" Enrl_Enroll_DATE= \"" + Ent.CaseSumReferDate + "\"  Enrl_Curr_Status= \"" + "I" + "\" />");
            //            ////                                //}
            //            ////                                //else
            //            ////                                //{
            //            ////                                //    Xml_HistTo_Pass.Append("<Row ID = \"" + Tmp_EnrlEnt.ID + "\" Status = \"" + Tmp_EnrlEnt.Status + "\"  From_Date = \"" + Tmp_EnrlEnt.Status_Date + "\" SITE = \"" + Tmp_EnrlEnt.Site + "\" ROOM = \"" + Tmp_EnrlEnt.Room + "\" AMPM = \"" + Tmp_EnrlEnt.AMPM + "\" To_Date = \"" + Tmp_EnrlEnt.Status_Date + "\"/>");

            //            ////                                //    Xml_To_Update.Append("<Row Enrl_APP_NO = \"" + Tmp_EnrlEnt.App + "\" Enrl_GROUP = \"" + Tmp_EnrlEnt.Agy + "\" Enrl_FUND_HIE = \"" + Tmp_EnrlEnt.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Tmp_EnrlEnt.ID + "\" Enrl_SITE = \"" + Tmp_EnrlEnt.Site + "\" Enrl_ROOM = \"" + Tmp_EnrlEnt.Room + "\" Enrl_AMPM = \"" + Tmp_EnrlEnt.AMPM +
            //            ////                                //               "\" ENRL_ENRLD_DATE = \"" + Tmp_EnrlEnt.Enrl_Date + "\" Enrl_WDRAW_CODE = \"" + Tmp_EnrlEnt.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + Tmp_EnrlEnt.Withdraw_Date + "\" Enrl_WLIST_DATE = \"" + Tmp_EnrlEnt.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Tmp_EnrlEnt.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Tmp_EnrlEnt.Denied_Date +
            //            ////                                //               "\" Enrl_PENDING_CODE= \"" + Tmp_EnrlEnt.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Tmp_EnrlEnt.Pending_Date + "\" Enrl_RANK= \"" + Tmp_EnrlEnt.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Tmp_EnrlEnt.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + Tmp_EnrlEnt.Transc_Type +
            //            ////                                //               "\" Enrl_TRANSFER_SITE= \"" + Tmp_EnrlEnt.Tranfr_Site + "\" Enrl_TRANSFER_ROOM= \"" + Tmp_EnrlEnt.Tranfr_Room + "\" Enrl_TRANSFER_AMPM= \"" + Tmp_EnrlEnt.Tranfr_AMPM + "\" Enrl_PARENT_RATE= \"" + Tmp_EnrlEnt.Parent_Rate + "\"  Enrl_FUNDING_CODE= \"" + Tmp_EnrlEnt.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + Tmp_EnrlEnt.Funding_Rate +
            //            ////                                //               "\" Enrl_FUND_END_DATE= \"" + Tmp_EnrlEnt.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Tmp_EnrlEnt.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + Ent.CaseSumReferDate + "\" Enrl_Curr_Status= \"" + Update_Entity.Status + "\" />");

            //            ////                                //}
            //            ////                                Sum_App_Exists = true;
            //            ////                                break;
            //            ////                            }
            //            ////                        }
            //            ////                        if (Sum_App_Exists)
            //            ////                            break;
            //            ////                    }
            //            ////                }
            //            ////            }
            //            ////        }

            //            ////        TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());

            //            ////        if (!App_Exists || Sum_App_Exists)
            //            ////            rowIndex = GD_PostIntake.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo);

            //            ////        if(Sum_App_Exists)
            //            ////            GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

            //            ////    }
            //            ////}

            //            ////Xml_To_Insert.Append("</Rows>");
            //            ////Xml_HistTo_Pass.Append("</Rows>");
            //            ////Xml_To_Update.Append("</Rows>");

            //            //_model.EnrollData.UpdateCASEENRL(Insert_Entity, "Edit", Xml_To_Pass.ToString(), null, out Sql_SP_Result_Message);
            //            //_model.EnrollData.UpdateCASEENRL(Insert_Entity, "Edit", Xml_To_Update.ToString(), Xml_HistTo_Pass.ToString(), out Sql_SP_Result_Message);

            //        }
            //        catch (Exception ex) { }
            //    }
            //}
        }


        private void Fill_NonEnrolls_On_FundChange()
        {
            string FundHie_To_Compare = ((ListItem)Cmb_Fund.SelectedItem).Value.ToString(), TmpName = " ";
            int rowIndex = 0;
            Post_Intake_CNT = 0;
            bool App_Exists = false;

            GD_PostIntake.Rows.Clear();
            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = false;
            this.Text = Btn_PostIntake.Text = "Send to Wait List";
            this.PI_Prog.HeaderText = "App#";
            this.PI_Name.HeaderText = "Name";
            this.PI_Name.Width = 280;
            this.PI_Prog.Width = 80;
            this.Post_Img.Visible = true;

            Site_Det_Panel.Visible = true;
            Lbl_Site.Text = "Site : ";
            Lbl_Site_Code.Text = Site;

            Rb_HSS_NonEnrl.Checked = true;
            int Tmp_Loop_Cnt = 0;
            foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            {
                if (Base_Enty.Site == Site)
                {
                    App_Exists = false;
                    foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                    {
                        if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
                            (Entity.FundHie.Trim() == FundHie_To_Compare))
                        {
                            App_Exists = true;
                            break;
                        }
                    }

                    TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());

                    if (!App_Exists)
                    {
                        Tmp_Loop_Cnt++;
                        rowIndex = GD_PostIntake.Rows.Add(Img_Blank, " ", Base_Enty.AppNo, TmpName, Base_Enty.AppNo, "N", Base_Enty.AppNo, " ", " ", " ", " ", " ");
                    }
                }

                if (Tmp_Loop_Cnt > 0)
                    Btn_PostIntake.Visible = true;
                else
                    Btn_PostIntake.Visible = false;
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
            this.Text = Btn_PostIntake.Text = "Send to Wait List";
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

        private void Fill_Add_Fund_APP()
        {
            string TmpName = " ";
            int rowIndex = 0;

            //Fill_WithdrawEnroll_Fund_Combo();
            //bool App_Exists = false, App_Exists_In_SUM = false;
            //List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            //List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            //CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();
            //Base_MainMenu_List = _model.MainMenuData.GetMainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    "), null, BaseForm.UserID);

            List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("  ", "0"));
            listItem.Add(new ListItem("Accepted", "C"));
            listItem.Add(new ListItem("Enroll", "E"));
            listItem.Add(new ListItem("Wait List", "L"));

            Cmb_Post_Status.Items.AddRange(listItem.ToArray());
            SetComboBoxValue(Cmb_Post_Status, "E");

            Btn_Add_Fund.Text = "Save";
            //this.Size = new System.Drawing.Size(462, 276);//(464, 232);
            //this.Add_Fund_Panel1.Size = new System.Drawing.Size(458, 205);//(460, 195);
            //this.Add_Fund_Panel.Size = new System.Drawing.Size(458, 273);//(460, 228);
            //this.Btn_Add_Fund.Location = new System.Drawing.Point(354, 238);//(355, 193);
            //this.WithdrawEnroll_Site_Panel.Location = new System.Drawing.Point(-1, 235);
            //this.Add_Fund_Panel.Location = new System.Drawing.Point(2, 1);

            panel1.Visible = false;
            /******************************************************/
            Add_Fund_Panel.Visible = true;
            this.Size = new Size(this.Width - 200, this.Height - (PI_Panle.Height + Stat_DblHist_Panel.Height + Add_Fund_Panel.Height + Enroll_Panel.Height + Counts_Panel.Height) - 65);
            /******************************************************/

            Fill_Fund_Combo();
            Fill_Fund_Category();

            Fund_Enroll_Date.Value = DateTime.Today;
            Fund_Enroll_Date.Checked = false;
            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Status_Date.Trim()))
            {
                Fund_Enroll_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Status_Date.Trim());
                Fund_Enroll_Date.Checked = true;
            }

            Txt_DrawEnroll_Site.Text = Pass_Enroll_List[0].Site.Trim();
            Txt_DrawEnroll_Room.Text = Pass_Enroll_List[0].Room.Trim();
            Txt_DrawEnroll_AMPM.Text = Pass_Enroll_List[0].AMPM.Trim();
            if (string.IsNullOrEmpty(Pass_Enroll_List[0].AMPM.Trim()) && Pass_Enroll_List[0].Room.Trim() == "****")
                Txt_DrawEnroll_AMPM.Text = "*";

            WithdrawEnroll_Site_Panel.Visible = true;
            //if (Pass_Enroll_List[0].Status.Trim() != "W")
            //    WithdrawEnroll_Site_Panel.Visible = false;
            //else
            //    WithdrawEnroll_Site_Panel.Visible = true;


            //PI_Panle.Visible = true;

            //GD_PostIntake.Rows.Clear();
            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //{
            //    foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            //    {
            //        if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo )
            //        {
            //            TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
            //            Txt_Name.Text = TmpName;

            //            //rowIndex = GD_PostIntake.Rows.Add(Img_Tick, " ", Base_Enty.AppNo, TmpName, Sel_Prog_Name, "Y", Base_Enty.AppNo);
            //            break;
            //        }
            //    }
            //}

            TmpName = LookupDataAccess.GetMemberName(Pass_Enroll_List[0].Snp_F_Name, Pass_Enroll_List[0].Snp_M_Name, Pass_Enroll_List[0].Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
            Txt_Name.Text = TmpName;

        }

        private void Fill_Edit_Fund_APP()
        {
            string TmpName = " ";
            int rowIndex = 0;

            /**********************************************************************************************/
            //this.Size = new System.Drawing.Size(462, 301);
            Add_Fund_Panel.Visible = true;
            this.Size = new Size(this.Width - 200, this.Height - (PI_Panle.Height + Stat_DblHist_Panel.Height + Transfer_Panel.Height + Enroll_Panel.Height + Counts_Panel.Height));
            /**********************************************************************************************/
            Cmb_Add_Fund.Enabled = false;
            postApp.Text = Btn_PostIntake.Text = "Edit Fund Date/Details";
            Cmb_Edit_Reason.Enabled = true;
            Fund_Enroll_Date.Value = Fund_End_Date.Value = Rate_Eff_Date.Value = DateTime.Now;
            Fill_Fund_Combo();
            SetComboBoxValue(Cmb_Add_Fund, Pass_Enroll_List[0].FundHie.Trim());


            this.Add_Fund_Panel.Location = new System.Drawing.Point(2, 1);
            if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {

                //  this.Size = new System.Drawing.Size(464, 276);
                // this.Add_Fund_Panel1.Size = new System.Drawing.Size(458, 205);//(460, 195);
                //  this.Add_Fund_Panel.Size = new System.Drawing.Size(458, 273);//(460, 228);

                // this.Btn_Add_Fund.Location = new System.Drawing.Point(342, 239);
            }
            else
            {

                // this.Size = new System.Drawing.Size(464, 186);
                //  this.Add_Fund_Panel1.Size = new System.Drawing.Size(460, 152);
                //  this.Add_Fund_Panel.Size = new System.Drawing.Size(460, 183);
                // this.Btn_Add_Fund.Location = new System.Drawing.Point(338, 150);
            }
            panel1.Visible = false;


            Btn_Add_Fund.Text = "Save";
            Fill_Fund_Edit_Reason_Combo();
            Fill_Fund_Reasons_Combo();
            Fill_Fund_Statos_Combo();
            Fill_Fund_Category();

            SetComboBoxValue(Cmb_Edit_Reason, Pass_Enroll_List[0].Withdraw_Code.Trim());
            //PI_Panle.Visible = true;

            //GD_PostIntake.Rows.Clear();
            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //{
            //    foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            //    {
            //        if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo )
            //        {
            //            TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
            //            Txt_Name.Text = TmpName;

            //            //rowIndex = GD_PostIntake.Rows.Add(Img_Tick, " ", Base_Enty.AppNo, TmpName, Sel_Prog_Name, "Y", Base_Enty.AppNo);
            //            break;
            //        }
            //    }
            //}

            TmpName = LookupDataAccess.GetMemberName(Pass_Enroll_List[0].Snp_F_Name, Pass_Enroll_List[0].Snp_M_Name, Pass_Enroll_List[0].Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
            Txt_Name.Text = TmpName;

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Funding_Rate.Trim()))
                Txt_Fund_Rate.Text = Pass_Enroll_List[0].Funding_Rate;

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Parent_Rate.Trim()))
                Txt_Parent_Rate.Text = Pass_Enroll_List[0].Parent_Rate;

            Fund_Enroll_Date.Checked = Fund_End_Date.Checked = Rate_Eff_Date.Checked = false;
            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Status_Date.Trim()))
            {
                Fund_Enroll_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Status_Date.Trim());
                Fund_Enroll_Date.Checked = true;
            }

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Fund_End_date.Trim()))
            {
                Fund_End_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Fund_End_date.Trim());
                Fund_End_Date.Checked = true;
            }

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Rate_EFR_date.Trim()))
            {
                Rate_Eff_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Rate_EFR_date.Trim());
                Rate_Eff_Date.Checked = true;
            }

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Funding_Code.Trim()))
                SetComboBoxValue(Cmb_Fund_Category, Pass_Enroll_List[0].Funding_Code.Trim());

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Status.Trim()))
                SetComboBoxValue(Cmb_Status, Pass_Enroll_List[0].Status.Trim());

            Txt_Desc1.Text = Txt_Desc2.Text = string.Empty;
            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Desc_1.Trim()))
                Txt_Desc1.Text = Pass_Enroll_List[0].Desc_1.Trim();

            if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Desc_2.Trim()))
                Txt_Desc2.Text = Pass_Enroll_List[0].Desc_2.Trim();
        }

        private void Fill_Transfer_Applicant()
        {
            string TmpName = " ";
            int rowIndex = 0;

            /*****************************************************************/
            // this.Size = new System.Drawing.Size(453, 231);
            //this.Transfer_Panel.Location = new System.Drawing.Point(2, 1);
            Transfer_Panel.Visible = true;
            this.Size = new Size(this.Width - 200, this.Height - (PI_Panle.Height + Stat_DblHist_Panel.Height + Add_Fund_Panel.Height + Enroll_Panel.Height + Counts_Panel.Height));
            //this.Size = new Size(this.Width, 333);
            /**********************************************************************************************/
            DT_Trf_Fund_EndDate.Value = DateTime.Now;
            DT_Trf_Eff_Date.Value = DateTime.Now;
            Transfer_Date.Value = DateTime.Now;
            Fill_Fund_Combo();
            Fill_Fund_Edit_Reason_Combo();
            Fill_Fund_Transfer_Category();


            SetComboBoxValue(Cmb_Trans_Fund, Pass_Enroll_List[0].FundHie.Trim());

            if (((ListItem)Cmb_Trans_Fund.SelectedItem).ID.ToString() == "N")
            {
                SetComboBoxValue(Cmb_Trf_FundCat, Pass_Enroll_List[0].Funding_Code.Trim());

                if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Funding_Rate.Trim()))
                    Txt_Trf_Funding_Rate.Text = Pass_Enroll_List[0].Funding_Rate;

                if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Parent_Rate.Trim()))
                    Txt_Trf_Parent_Rate.Text = Pass_Enroll_List[0].Parent_Rate;

                DT_Trf_Fund_EndDate.Checked = DT_Trf_Eff_Date.Checked = false;
                //if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Status_Date.Trim()))
                //{
                //    Fund_Enroll_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Status_Date.Trim());
                //    Fund_Enroll_Date.Checked = true;
                //}

                if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Fund_End_date.Trim()))
                {
                    DT_Trf_Fund_EndDate.Value = Convert.ToDateTime(Pass_Enroll_List[0].Fund_End_date.Trim());
                    DT_Trf_Fund_EndDate.Checked = true;
                }

                if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Rate_EFR_date.Trim()))
                {
                    DT_Trf_Eff_Date.Value = Convert.ToDateTime(Pass_Enroll_List[0].Rate_EFR_date.Trim());
                    DT_Trf_Eff_Date.Checked = true;
                }
            }


            TmpName = LookupDataAccess.GetMemberName(Pass_Enroll_List[0].Snp_F_Name, Pass_Enroll_List[0].Snp_M_Name, Pass_Enroll_List[0].Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
            Txt_Trans_Name.Text = TmpName;

            if (Pass_Enroll_List[0].Transc_Type.Trim() == "T")
            {
                Txt_Trans_Site.Text = Pass_Enroll_List[0].Tranfr_Site.Trim();
                Txt_Trans_Room.Text = Pass_Enroll_List[0].Tranfr_Room.Trim();
                //Txt_Trans_AMPM.Text = Pass_Enroll_List[0].Tranfr_AMPM.Trim();

                switch (Pass_Enroll_List[0].Tranfr_AMPM.Trim())
                {
                    case "A": Txt_Trans_AMPM.Text = "A - AM Class"; break;
                    case "P": Txt_Trans_AMPM.Text = "P - PM Class"; break;
                    case "E": Txt_Trans_AMPM.Text = "E - Extended Day"; break;
                    case "F": Txt_Trans_AMPM.Text = "F - Full Day"; break;
                }

                SetComboBoxValue(Cmb_Trans_Reason, Pass_Enroll_List[0].Withdraw_Code.Trim());
            }
        }

        private void Fill_Fund_Statos_Combo()
        {
            Cmb_Status.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));
            listItem.Add(new ListItem("Denied", "R"));
            listItem.Add(new ListItem("Enrolled", "E"));
            listItem.Add(new ListItem("Pending", "P"));
            listItem.Add(new ListItem("Wait List", "L"));
            listItem.Add(new ListItem("Withdrawn", "W"));
            Cmb_Status.Items.AddRange(listItem.ToArray());
            Cmb_Status.SelectedIndex = 0;
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            bool Combo_Set = false;
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        Combo_Set = true;
                        break;
                    }
                }
            }

            if (!Combo_Set)
                comboBox.SelectedIndex = 0;
        }


        List<MainMenuEntity> App_Reff_From_CASESUM = new List<MainMenuEntity>();
        List<Sum_Referral_Entity> Tmp_Referral_List = new List<Sum_Referral_Entity>();
        List<CaseSumEntity> Tmp_Referral_NotInMST_List = new List<CaseSumEntity>();
        List<MainMenuEntity> Base_MainMenu_NotEnroll_List = new List<MainMenuEntity>();
        private void Fill_NonEnrolls_From_CASESUM()
        {
            string TmpName = " ";
            int rowIndex = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();

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


                List<SqlParameter> sqlParamList = _model.EnrollData.Prepare_MST_SUM_NonEnrollSqlParameters_List(Search_Entity, "Browse", BaseForm.BaseAgency
                                                                , BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
                DataSet CASEENRLData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_CaseSum_RefApps_On_MSTSNP]");

                if (CASEENRLData != null && CASEENRLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
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


                //Tmp_Referral_List = _model.EnrollData.Get_CaseSum_RefApps_On_MSTSNP(Search_Entity, "Browse");
                //CaseSum_List = Tmp_CaseSum_List.FindAll(u => (string.IsNullOrEmpty(u.CaseSumRefApplNo.Trim())));

                //Base99_MainMenu_List = _model.MainMenuData.GetMainMenuSearch("APP", "APP", null, null, null, null, null, null, null, null, null, null, null, null, null, "999999    ", null, BaseForm.UserID);
            }


            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = true;
            //this.LblHeader.Text = Btn_PostIntake.Text = "Send to Post Intake"; 
            GD_PostIntake.Rows.Clear();
            if (Rb_From_Sum.Checked)
            {
                this.Text = Btn_PostIntake.Text = "Send to Post Intake";
                this.PI_Prog.HeaderText = "Ref. to  Year  App #"; // "Exp Enroll App#";
                this.PI_Name.Width = 124;//132;
                this.PI_Prog.Width = 120;
                this.Post_Img.Visible = true;

                foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
                {
                    //rowIndex = From_Sum_Grid.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Ent.App);
                    //From_Sum_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                    Compare_SSN_List.Clear();
                    Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
                    if (Compare_SSN_List.Count > 0)
                    {
                        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                        rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName,
                                        BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, "N", Compare_SSN_List[0].AppNo,
                                        Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie, " ", " ", " ", " ");
                        GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                    //TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                    //rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName, Sel_Prog_Name, "N", Ent.App);
                    //GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
            else
            {
                if (Rb_From_Sum_NotInMst.Checked)
                {
                    TmpName = " "; string Ref_Date = " ";

                    this.Text = Btn_PostIntake.Text = "Drag Applicant";
                    this.PI_Name.HeaderText = "Client Name"; // "Referred By";
                    this.PI_Prog.HeaderText = "Referred Date";
                    this.PI_Name.Width = 183;//195;
                    this.PI_Prog.Width = 90;
                    this.Post_Img.Visible = false;
                    //foreach (CaseSumEntity Ent in Tmp_Referral_NotInMST_List)
                    //{
                    //    Compare_SSN_List.Clear();
                    //    Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
                    //    if (Compare_SSN_List.Count > 0)
                    //    {
                    //        TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                    //        Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.CaseSumReferDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    //        rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.CaseSumAgency + "-" + Ent.CaseSumDept + "-" + Ent.CaseSumProgram, Ent.CaseSumApplNo, Ent.CaseSumReferBy, Ref_Date,
                    //                            "N", Ent.CaseSumApplNo, Ent.CaseSumAgency + Ent.CaseSumDept + Ent.CaseSumProgram + Ent.CaseSumYear.Trim() + Ent.CaseSumApplNo + Ent.CaseSumRefAgency + Ent.CaseSumRefDept + Ent.CaseSumRefProgram);
                    //        GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    //    }
                    //}

                    foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
                    {
                        //rowIndex = From_Sum_Grid.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Ent.App);
                        //From_Sum_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

                        Compare_SSN_List.Clear();
                        Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));
                        if (Compare_SSN_List.Count == 0)
                        {
                            Ref_Date = " ";
                            TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                            if (!string.IsNullOrEmpty(Ent.Referred_Date.Trim()))
                                Ref_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.Referred_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, Ent.Referred_By, Ref_Date,
                                                "N", Ent.App, Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year + Ent.App + Ent.Ref_Hie, Ent.Ssn, Ent.Snp_F_Name, Ent.Snp_L_Name, Ent.Snp_DOB);

                            //rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName,
                            //                BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, "N", Compare_SSN_List[0].AppNo,
                            //                Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Agy + Ent.Ref_Dept + Ent.Ref_Prog);
                            GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        //TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                        //rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName, Sel_Prog_Name, "N", Ent.App);
                        //GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    }


                }
            }


            //if (App_Reff_From_CASESUM.Count == 0)
            //{
            //    App_Reff_From_CASESUM.Clear();

            //foreach (MainMenuEntity Base_Enty in Base_MainMenu_List)
            //{
            //    App_Exists = false;

            //    if (Base_Enty.AppNo == "00000111")
            //        App_Exists = false;

            //    foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //    {
            //        if (Entity.App == "00000111")
            //            App_Exists = false;

            //        if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year.Trim() + Entity.App == Base_Enty.Agy + Base_Enty.Dept + Base_Enty.Prog + Base_Enty.Year.Trim() + Base_Enty.AppNo &&
            //            (Entity.FundHie.Trim() == Sel_Hie.Trim()))
            //        {
            //            App_Exists = true;
            //            break;
            //        }
            //    }

            //    if (!App_Exists)
            //    {
            //        App_Exists_In_SUM = false;
            //        Compare_SSN_List = Base99_MainMenu_List.FindAll(u => u.Ssn.Equals(Base_Enty.Ssn));
            //        foreach (MainMenuEntity Tmp in Compare_SSN_List)
            //        {
            //            //if (Tmp.Ssn == "075704027")
            //            //    App_Exists_In_SUM = false;

            //            if (Base_Enty.Ssn == Tmp.Ssn)
            //            {

            //                Tmp_Compare_Sum_List = CaseSum_List.Find(u => u.CaseSumAgency + u.CaseSumDept + u.CaseSumProgram + u.CaseSumYear.Trim() + u.CaseSumApplNo == Tmp.Agy + Tmp.Dept + Tmp.Prog + Tmp.Year.Trim() + Tmp.AppNo &&
            //                                                             u.CaseSumRefAgency + u.CaseSumRefDept + u.CaseSumRefProgram == Sel_Hie.Substring(0, 6));
            //                if (Tmp_Compare_Sum_List != null)
            //                    App_Exists_In_SUM = true;
            //                //foreach (CaseSumEntity Ent in Compare_Sum_List)
            //                //{
            //                //    if (Ent.CaseSumAgency + Ent.CaseSumDept + Ent.CaseSumProgram + Ent.CaseSumYear.Trim() + Ent.CaseSumApplNo == Tmp.Agy + Tmp.Dept + Tmp.Prog + Tmp.Year.Trim() + Tmp.AppNo &&
            //                //        (Ent.CaseSumRefAgency + Ent.CaseSumRefDept + Ent.CaseSumRefProgram == Sel_Hie.Substring(0, 6)))
            //                //    {
            //                //        App_Exists_In_SUM = true;
            //                //        break;
            //                //    }
            //                //}
            //            }

            //            if (App_Exists_In_SUM)
            //                break;
            //        }

            //        if (App_Exists_In_SUM)
            //        {
            //            //rowIndex = From_Sum_Grid.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo);
            //            //From_Sum_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //            TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
            //            rowIndex = GD_PostIntake.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo);
            //            GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;

            //            App_Reff_From_CASESUM.Add(new MainMenuEntity(Base_Enty));
            //        }
            //    }
            //}
            //}
            //else
            //{
            //    foreach (MainMenuEntity Base_Enty in App_Reff_From_CASESUM)
            //    {
            //        TmpName = LookupDataAccess.GetMemberName(Base_Enty.Fname, Base_Enty.Mname, Base_Enty.Lname, BaseForm.BaseHierarchyCnFormat.ToString());
            //        rowIndex = GD_PostIntake.Rows.Add(Img_Blank, TmpName, Sel_Prog_Name, "N", Base_Enty.AppNo);
            //        GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //    }

            //}
        }

        private void Fill_HSS_NonEnrolls_From_CASESUM()
        {
            string TmpName = " ";
            int rowIndex = 0;
            Post_Intake_CNT = 0;
            bool App_Exists = false, App_Exists_In_SUM = false;
            List<MainMenuEntity> Compare_SSN_List = new List<MainMenuEntity>();
            List<CaseSumEntity> Compare_Sum_List = new List<CaseSumEntity>();
            CaseSumEntity Tmp_Compare_Sum_List = new CaseSumEntity();
            //List<CaseEnrlEntity> Compare_Enrl_List = new List<CaseEnrlEntity>();

            if (Tmp_Referral_List.Count == 0)
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
                    foreach (DataRow row in CASEENRLData.Tables[0].Rows)
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

            GD_PostIntake.Rows.Clear();
            this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = true;
            this.Text = Btn_PostIntake.Text = "Send to Post Intake";
            this.PI_Prog.HeaderText = "Ref. to  Year  App #"; // "Exp Enroll App#";
            this.PI_Name.Width = 124;//132;
            this.PI_Prog.Width = 120;
            this.Post_Img.Visible = true;

            Site_Det_Panel.Visible = true;
            Lbl_Site.Text = "Class : ";
            Lbl_Site_Code.Text = Site + "/" + Room + "/" + AmPm;

            int Tmp_Loop_Cnt = 0;
            foreach (Sum_Referral_Entity Ent in Tmp_Referral_List)
            {
                Compare_SSN_List.Clear(); //Compare_Enrl_List.Clear();
                Compare_SSN_List = Base_MainMenu_NotEnroll_List.FindAll(u => u.Ssn.Equals(Ent.Ssn) && u.Ssn.Equals(Ent.Ssn));

                //Compare_Enrl_List = Pass_Enroll_List.FindAll(u => u.Agy.Equals(Ent.Agy) && u.Dept.Equals(Ent.Dept) && u.Prog.Equals(Ent.Prog) && u.Year.Equals(Ent.Year) && u.App.Equals(Ent.App) && u.FundHie.Equals(((ListItem)Cmb_Fund.SelectedItem).Value.ToString()));

                if (Compare_SSN_List.Count > 0)//&& Compare_Enrl_List.Count == 0)
                {
                    TmpName = LookupDataAccess.GetMemberName(Ent.Snp_F_Name, Ent.Snp_M_Name, Ent.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                    rowIndex = GD_PostIntake.Rows.Add(Img_Blank, Ent.Agy + "-" + Ent.Dept + "-" + Ent.Prog, Ent.App, TmpName,
                                    BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + BaseForm.BaseYear + " - " + Compare_SSN_List[0].AppNo, "N", Compare_SSN_List[0].AppNo,
                                    Ent.Agy + Ent.Dept + Ent.Prog + Ent.Year.Trim() + Ent.App + Ent.Ref_Hie, " ", " ", " ", " ");
                    GD_PostIntake.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    Tmp_Loop_Cnt++;
                }
            }

            if (Tmp_Loop_Cnt > 0)
                Btn_PostIntake.Visible = true;
            else
                Btn_PostIntake.Visible = false;

        }

        private void Launch_Enroll_Panel()
        {
            this.Size = new System.Drawing.Size(673, 503);
            this.Enroll_Panel.Location = new System.Drawing.Point(1, 1);

            this.Enroll_Panel.Size = new System.Drawing.Size(669, 501);
            Enroll_Panel.Visible = true;
            //Fill_Enroll_Grid("fff");
            Rb_Wait_CheckedChanged(Rb_Wait, EventArgs.Empty);
            //Fill_Not_Enroll_Grid(Status_To_Compare);
            Rb_Enrolled_CheckedChanged(Rb_Enrolled, EventArgs.Empty);
        }


        private void Fill_Not_Enroll_Grid(string Status_Mode)
        {
            Top_NotEnrl_Grid.Rows.Clear();

            string TmpName = " ", Wait_Date = " ", Pending_Date = " ", Denied_Date = " ";
            int rowIndex = 0, Tmp_Loop_Cnt = 0;
            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            {
                if (Entity.Status == Status_Mode)
                {
                    TmpName = LookupDataAccess.GetMemberName(Entity.Snp_F_Name, Entity.Snp_M_Name, Entity.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());

                    Wait_Date = Pending_Date = Denied_Date = " ";
                    switch (Entity.Status)
                    {
                        case "L":
                            Wait_Date = Entity.Wiait_Date;
                            Lbl_NonEnroll.Text = "Waiting List Clients";
                            break;
                        case "P":
                            Pending_Date = Entity.Pending_Date;
                            Lbl_NonEnroll.Text = "Pending Clients";
                            break;
                        case "R":
                            Denied_Date = Entity.Denied_Date;
                            Lbl_NonEnroll.Text = "Rejected Clients";
                            break;
                        case "I":
                            Lbl_NonEnroll.Text = "Post Intake Clients";
                            break;
                    }

                    if (!string.IsNullOrEmpty(Wait_Date.Trim()))
                        Wait_Date = LookupDataAccess.Getdate(Wait_Date);
                    if (!string.IsNullOrEmpty(Pending_Date.Trim()))
                        Pending_Date = LookupDataAccess.Getdate(Pending_Date);
                    if (!string.IsNullOrEmpty(Denied_Date.Trim()))
                        Denied_Date = LookupDataAccess.Getdate(Denied_Date);

                    rowIndex = Top_NotEnrl_Grid.Rows.Add(Img_Blank, " ", " ", TmpName, Wait_Date, Pending_Date, " ", "N",
                                                         Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App);
                    Tmp_Loop_Cnt++;
                }
            }
        }

        string Top_Grid_Status_To_Compare = "L";
        private void Rb_Wait_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == Rb_Wait)
                Top_Grid_Status_To_Compare = "L";
            else if (sender == Rb_Pending)
                Top_Grid_Status_To_Compare = "P";
            else if (sender == Rb_Denied)
                Top_Grid_Status_To_Compare = "R";
            else if (sender == Rb_PostIntake)
                Top_Grid_Status_To_Compare = "I";

            Fill_Not_Enroll_Grid(Top_Grid_Status_To_Compare);
        }

        //private void Btn_Change_Status_Click(object sender, EventArgs e)
        //{
        //    if (Status_Change_CNT > 0)
        //    {
        //        List<CaseEnrlEntity> Change_Status_list = new List<CaseEnrlEntity>();
        //        Change_Status_list = Prepare_Pass_Status_List();
        //        CASE0010_StatusChange_Form Status_Form = new CASE0010_StatusChange_Form(BaseForm, Change_Status_list, Privileges.ModuleCode, Sel_Hie);
        //        Status_Form.FormClosed += new Form.FormClosedEventHandler(Status_Form_Close);
        //        Status_Form.ShowDialog();
        //    }
        //    else
        //        MessageBox.Show("You Must Select Atleast One Record ", "CAP Systems");
        //}


        // Kranthi: didn't find Status_Form_Close function enabled in the code so commenting this code - 12/19/2022
        private void Status_Form_Close(object sender, FormClosedEventArgs e)
        {

        }


        private List<CaseEnrlEntity> Prepare_Pass_Status_List()
        {
            List<CaseEnrlEntity> Tmp_Status_list = new List<CaseEnrlEntity>();
            if (Change_StatusOF_SW == "NotEnroll")
            {
                foreach (DataGridViewRow dr in Top_NotEnrl_Grid.Rows)
                {
                    if (dr.Cells["TGD_Status_Flg"].Value.ToString() == "Y")
                    {
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App ==
                                dr.Cells["TGD_Key"].Value.ToString())
                            {
                                Tmp_Status_list.Add(new CaseEnrlEntity(Entity));
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow dr in Bottom_Enrl_Grid.Rows)
                {
                    if (dr.Cells["BGD_Status_Flg"].Value.ToString() == "Y")
                    {
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App ==
                                dr.Cells["BGD_Key"].Value.ToString())
                            {
                                Tmp_Status_list.Add(new CaseEnrlEntity(Entity));
                            }
                        }
                    }
                }
            }

            return Tmp_Status_list;
        }

        private void Top_NotEnrl_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Top_NotEnrl_Grid.Rows.Count > 0 && (Change_StatusOF_SW == "NotEnroll" || string.IsNullOrEmpty(Change_StatusOF_SW.Trim())))
            {
                int ColIdx = Top_NotEnrl_Grid.CurrentCell.ColumnIndex;
                int RowIdx = Top_NotEnrl_Grid.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (Top_NotEnrl_Grid.CurrentRow.Cells["TGD_Status_Flg"].Value.ToString() == "Y")
                        {
                            Top_NotEnrl_Grid.CurrentRow.Cells["TGD_Image"].Value = Img_Blank;
                            Top_NotEnrl_Grid.CurrentRow.Cells["TGD_Status_Flg"].Value = "N";
                            Status_Change_CNT--;

                            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                            //{
                            //    if (Entity.Agy == Top_NotEnrl_Grid.CurrentRow.Cells["CACode"].Value.ToString())
                            //        Entity.Sel_SW = false;
                            //}
                        }
                        else
                        {
                            Top_NotEnrl_Grid.CurrentRow.Cells["TGD_Image"].Value = Img_Tick;
                            Top_NotEnrl_Grid.CurrentRow.Cells["TGD_Status_Flg"].Value = "Y";
                            Status_Change_CNT++;
                        }
                        Change_StatusOF_SW = "NotEnroll";

                        if (Status_Change_CNT == 0)
                            Change_StatusOF_SW = string.Empty;
                    }
                }
            }
        }

        string Bottom_Grid_Grid_Status_To_Compare = "E";
        private void Rb_Enrolled_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == Rb_Enrolled)
                Bottom_Grid_Grid_Status_To_Compare = "E";
            else if (sender == Rb_Withdraw)
                Bottom_Grid_Grid_Status_To_Compare = "W";

            Fill_Enroll_Grid(Bottom_Grid_Grid_Status_To_Compare);
        }



        private void Fill_Enroll_Grid(string Status_Mode)
        {
            Bottom_Enrl_Grid.Rows.Clear();

            string TmpName = " ", Enroll_Date = " ", Withdraw_Date = " ", Denied_Date = " ";
            int rowIndex = 0;
            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            {
                if (Entity.Status == Status_Mode)
                {
                    TmpName = LookupDataAccess.GetMemberName(Entity.Snp_F_Name, Entity.Snp_M_Name, Entity.Snp_L_Name, BaseForm.BaseHierarchyCnFormat.ToString());

                    Enroll_Date = Withdraw_Date = Denied_Date = " ";

                    //switch (Entity.Status)
                    //{
                    //    case "E": Enroll_Date = Entity.Enrl_Date;
                    //        break;
                    //    case "W": Withdraw_Date = Entity.Withdraw_Date;
                    //        break;
                    //}

                    if (!string.IsNullOrEmpty(Entity.Enrl_Date.Trim()))
                        Enroll_Date = LookupDataAccess.Getdate(Entity.Enrl_Date);
                    if (!string.IsNullOrEmpty(Entity.Withdraw_Date.Trim()))
                        Withdraw_Date = LookupDataAccess.Getdate(Entity.Withdraw_Date);

                    rowIndex = Bottom_Enrl_Grid.Rows.Add(Img_Blank, " ", " ", Enroll_Date, Withdraw_Date, TmpName, " ", "N",
                                                         Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App);
                }
            }

        }

        string Change_StatusOF_SW = string.Empty;
        int Status_Change_CNT = 0;
        private void Bottom_Enrl_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Bottom_Enrl_Grid.Rows.Count > 0 && (Change_StatusOF_SW == "Enroll" || string.IsNullOrEmpty(Change_StatusOF_SW.Trim())))
            {
                int ColIdx = Bottom_Enrl_Grid.CurrentCell.ColumnIndex;
                int RowIdx = Bottom_Enrl_Grid.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (Bottom_Enrl_Grid.CurrentRow.Cells["BGD_Status_Flg"].Value.ToString() == "Y")
                        {
                            Bottom_Enrl_Grid.CurrentRow.Cells["BGD_Image"].Value = Img_Blank;
                            Bottom_Enrl_Grid.CurrentRow.Cells["BGD_Status_Flg"].Value = "N";
                            Status_Change_CNT--;

                            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                            //{
                            //    if (Entity.Agy == Bottom_Enrl_Grid.CurrentRow.Cells["CACode"].Value.ToString())
                            //        Entity.Sel_SW = false;
                            //}
                        }
                        else
                        {
                            Bottom_Enrl_Grid.CurrentRow.Cells["BGD_Image"].Value = Img_Tick;
                            Bottom_Enrl_Grid.CurrentRow.Cells["BGD_Status_Flg"].Value = "Y";
                            Status_Change_CNT++;
                        }
                        Change_StatusOF_SW = "Enroll";

                        if (Status_Change_CNT == 0)
                            Change_StatusOF_SW = string.Empty;
                    }
                }
            }
        }

        public bool Validate_Status_Date()
        {
            bool IsValid = true;

            if (!Status_Date.Checked)
            {
                _errorProvider.SetError(Status_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label56.Text.Replace(Consts.Common.Colon, string.Empty)));
                IsValid = false;
            }
            else
            {
                if (Status_Date.Value > DateTime.Today)
                {
                    _errorProvider.SetError(Status_Date, label56.Text + " Should not be Future Date ".Replace(Consts.Common.Colon, string.Empty));
                    IsValid = false;
                }
                else
                    _errorProvider.SetError(Status_Date, null);
            }


            if (Privileges.ModuleCode == "02" && Rb_HSS_RefSum.Checked)
            {
                string Class_Start_Date = string.Empty, Class_End_Date = string.Empty;
                foreach (CaseSiteEntity Ent in HSS_Site_List)
                {
                    if (Ent.SiteNUMBER == Site && Ent.SiteROOM.Trim() == Room && Ent.SiteAM_PM.Trim() == AmPm)
                    {
                        Class_Start_Date = Ent.SiteCLASS_START;
                        Class_End_Date = Ent.SiteCLASS_END;
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
                        _errorProvider.SetError(Status_Date, label56.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                        Validate_Trf_Date = IsValid = false;
                    }
                    else
                        if (IsValid)
                        _errorProvider.SetError(Status_Date, null);
                }
            }

            if (Privileges.ModuleCode == "03" && !Rb_From_Sum_NotInMst.Checked)
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

        string Sql_SP_Result_Message = string.Empty;
        private void Btn_PostIntake_Click(object sender, EventArgs e)
        {
            if (Validate_Status_Date())
            {
                if (!Rb_From_Sum_NotInMst.Checked)
                {
                    if (Post_Intake_CNT > 0)
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
                        if (Privileges.ModuleCode == "03")
                        {
                            Find_Hie_ToPass = Sel_Hie;
                            RecordType_ToPass = "C";
                            Status_ToPass = Rb_From_Mst.Checked ? "L" : "I";
                        }
                        else
                        {
                            Status_ToPass = Rb_HSS_NonEnrl.Checked ? "L" : "I";
                            New_Post_Record.Site = Site;
                            Find_Hie_ToPass = ((ListItem)Cmb_Fund.SelectedItem).Value.ToString();
                        }

                        foreach (DataGridViewRow dr in GD_PostIntake.Rows)
                        {
                            if (dr.Cells["PI_Sel_SW"].Value.ToString() == "Y")
                            {
                                New_Post_Record.App = dr.Cells["PI_App"].Value.ToString();

                                New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.Group = New_Post_Record.AMPM = New_Post_Record.Enrl_Date = string.Empty;
                                New_Post_Record.Seq = "1";


                                New_Post_Record.FundHie = Find_Hie_ToPass;
                                New_Post_Record.Rec_Type = RecordType_ToPass;
                                New_Post_Record.Status = Status_ToPass;
                                New_Post_Record.Lstc_Oper = BaseForm.UserID;

                                Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + (Privileges.ModuleCode == "02" ? (Rb_HSS_NonEnrl.Checked ? null : Site) : null) +
                                           "\" Enrl_ROOM = \"" + (Privileges.ModuleCode == "02" ? (Rb_HSS_NonEnrl.Checked ? null : Room) : null) + "\" Enrl_AMPM = \"" + (Privileges.ModuleCode == "02" ? (Rb_HSS_NonEnrl.Checked ? null : AmPm) : null) +
                                           "\" ENRL_ENRLD_DATE = \"" + string.Empty + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                                           "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                                           "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                                           "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\" Sum_Key_To_Update= \"" + dr.Cells["PI_Sum_Key"].Value.ToString() + "\"/>");

                                //if (Privileges.ModuleCode == "03")
                                //{

                                //    Sum_Xml_To_Pass.Append("<Row Sum_Agy = \"" + New_Post_Record.App + "\" Sum_Dept = \"" + New_Post_Record.Agy + "\" Sum_Prog = \"" + Find_Hie_ToPass + "\" Sum_Year = \"" + "1" + "\" Sum_AppNo = \"" + "1" + "\" Sum_Ref_Agy = \"" + null + "\" Sum_Ref_Dept = \"" + string.Empty + "\" Sum_Ref_Prog = \"" + string.Empty +
                                //               "\" Sum_Ref_Year = \"" + string.Empty + "\" Sum_Ref_AppNo = \"" + string.Empty + "\" Sum_Refer_By = \"" + string.Empty + "\" Sum_Refer_Date = \"" + string.Empty + "\" Sum_Cont_Key = \"" + string.Empty + "\" Sum_Points = \"" + string.Empty +
                                //               "\" Sum_Status_CODE= \"" + string.Empty + "\" Sum_Status_DATE= \"" + string.Empty + "\" Sum_Not_Instd= \"" + string.Empty + "\"  Sum_Not_Instd_Date= \"" + string.Empty + "\" Sum_Not_Instd_By= \"" + string.Empty +
                                //               "\" Sum_Not_Instd_Resn= \"" + string.Empty + "\" Sum_Lstc_Date= \"" + string.Empty + "\" Sum_Lstc_Oper= \"" + "0" + "\"  Sum_Add_Date= \"" + string.Empty + "\" Sum_Add_Oper= \"" + "0" + "\"/>");

                                //}

                            }
                        }


                        //if (Privileges.ModuleCode == "03")
                        //{
                        //    Status_ToPass = "I";
                        //    foreach (DataGridViewRow dr in From_Sum_Grid.Rows)
                        //    {
                        //        if (dr.Cells["Sum_Sel_SW"].Value.ToString() == "Y")
                        //        {
                        //            New_Post_Record.App = dr.Cells["Sum_App"].Value.ToString();

                        //            New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.Group = New_Post_Record.AMPM = New_Post_Record.Enrl_Date = string.Empty;
                        //            New_Post_Record.Seq = "1";


                        //            New_Post_Record.FundHie = Find_Hie_ToPass;
                        //            New_Post_Record.Rec_Type = RecordType_ToPass;
                        //            New_Post_Record.Status = Status_ToPass;
                        //            New_Post_Record.Lstc_Oper = BaseForm.UserID;

                        //            Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + New_Post_Record.App + "\" Enrl_GROUP = \"" + New_Post_Record.Agy + "\" Enrl_FUND_HIE = \"" + Find_Hie_ToPass + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + "1" + "\" Enrl_SITE = \"" + null + "\" Enrl_ROOM = \"" + string.Empty + "\" Enrl_AMPM = \"" + string.Empty +
                        //                       "\" ENRL_ENRLD_DATE = \"" + string.Empty + "\" Enrl_WDRAW_CODE = \"" + string.Empty + "\" Enrl_WDRAW_DATE = \"" + string.Empty + "\" Enrl_WLIST_DATE = \"" + string.Empty + "\" Enrl_DENIED_CODE = \"" + string.Empty + "\" Enrl_DENIED_DATE = \"" + string.Empty +
                        //                       "\" Enrl_PENDING_CODE= \"" + string.Empty + "\" Enrl_PENDING_DATE= \"" + string.Empty + "\" Enrl_RANK= \"" + string.Empty + "\"  Enrl_RNKCHNG_CODE= \"" + string.Empty + "\" Enrl_TRAN_TYPE= \"" + string.Empty +
                        //                       "\" Enrl_TRANSFER_SITE= \"" + string.Empty + "\" Enrl_TRANSFER_AMPM= \"" + string.Empty + "\" Enrl_PARENT_RATE= \"" + "0" + "\"  Enrl_FUNDING_CODE= \"" + string.Empty + "\" Enrl_FUNDING_RATE= \"" + "0" +
                        //                       "\" Enrl_FUND_END_DATE= \"" + string.Empty + "\" Enrl_RATE_EFF_DATE= \"" + string.Empty + "\" Enrl_Enroll_DATE= \"" + New_Post_Record.Status_Date + "\"  Enrl_Curr_Status= \"" + Status_ToPass + "\"/>");
                        //        }
                        //    }
                        //}

                        Xml_To_Pass.Append("</Rows>");
                        Sum_Xml_To_Pass.Append("</Rows>");


                        bool save_result = false;
                        if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", Xml_To_Pass.ToString(), string.Empty, null, out Sql_SP_Result_Message))
                        {
                            save_result = true;
                            //if (Privileges.ModuleCode == "03" && Rb_From_Sum.Checked)
                            //{
                            //    save_result = false;
                            //    if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", Xml_To_Pass.ToString(), string.Empty, out Sql_SP_Result_Message))
                            //        Save_Result = true;
                            //    else
                            //        MessageBox.Show(Sql_SP_Result_Message, "CAP Systems");
                            //}
                            //if(Save_Result)
                            //{
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            //}
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                    else
                        AlertBox.Show("You must Select atleast One Record", MessageBoxIcon.Warning);
                }
                else
                {
                    //AdvancedMainMenuSearch advancedMainMenuSearch = new AdvancedMainMenuSearch(BaseForm, true);

                    //string Tmp_Hie_Key = GD_PostIntake.CurrentRow.Cells["PI_Ref_From"].Value.ToString();


                    string Tmp_Hie_Key = GD_PostIntake.CurrentRow.Cells["PI_Sum_Key"].Value.ToString();
                    string Tmp_Hie_App = GD_PostIntake.CurrentRow.Cells["PI_Ref_App"].Value.ToString();

                    string Fname = GD_PostIntake.CurrentRow.Cells["PI_FName"].Value.ToString();
                    string Lname = GD_PostIntake.CurrentRow.Cells["PI_LName"].Value.ToString();
                    string SSn = GD_PostIntake.CurrentRow.Cells["PI_SSN"].Value.ToString();
                    string DOB = GD_PostIntake.CurrentRow.Cells["PI_DOB"].Value.ToString();

                    //AdvancedMainMenuSearch advancedMainMenuSearch = new AdvancedMainMenuSearch(BaseForm, Tmp_Hie_Key.Substring(0, 2), Tmp_Hie_Key.Substring(3, 2), Tmp_Hie_Key.Substring(6, 2), "    " ,Tmp_Hie_App);
                    //advancedMainMenuSearch.FormClosed += new Form.FormClosedEventHandler(On_ADV_SerachFormClosed);
                    //advancedMainMenuSearch.ShowDialog();

                    MainMenuAddApplicantForm AddApplicant = new MainMenuAddApplicantForm(BaseForm, 'Y', DOB, Fname, Lname, SSn, Tmp_Hie_App, Tmp_Hie_Key.Substring(0, 10), Privileges.Program, true, string.Empty);
                    AddApplicant.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                    AddApplicant.StartPosition = FormStartPosition.CenterScreen;
                    AddApplicant.ShowDialog();
                }
            }
        }

        private void On_Applicant_Dragged(object sender, FormClosedEventArgs e)
        {
            MainMenuAddApplicantForm form = sender as MainMenuAddApplicantForm;
            if (form.DialogResult == DialogResult.OK)
            {
                this.Close();
            }
            //Adv_search = false;
        }

        private void On_ADV_SerachFormClosed(object sender, FormClosedEventArgs e)
        {
            AdvancedMainMenuSearch form = sender as AdvancedMainMenuSearch;
            if (form.DialogResult == DialogResult.OK)
            {
                //string Selected_App_key = null, BaseForm_Priv_Hierarchy = null;
                //BaseForm_Priv_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
                //Selected_App_key = form.GetSelectedApplicant();
                //if (!string.IsNullOrEmpty(Selected_App_key))
                //{
                //    MainPanel.Visible = true;

                //    if (BaseForm_Priv_Hierarchy != Selected_App_key.Substring(0, 6))
                //    {
                //        BaseForm.BaseAgency = Selected_App_key.Substring(0, 2);
                //        BaseForm.BaseDept = Selected_App_key.Substring(2, 2);
                //        BaseForm.BaseProg = Selected_App_key.Substring(4, 2);
                //        BaseForm.BaseYear = Selected_App_key.Substring(6, 4);
                //        BaseForm.RefreshNavigationTabs(Selected_App_key.Substring(0, 2) + Selected_App_key.Substring(2, 2) + Selected_App_key.Substring(4, 2));
                //    }

                //    BaseForm.BaseTopApplSelect = "N";
                //    TxtAppNo.Clear();
                //    GvwAppHou.Rows.Clear();

                //    if (Selected_App_key.Length > 10)
                //    {
                //        BaseForm.BaseTopApplSelect = "Y";
                //        ShowHierachyandApplNo(Selected_App_key.Substring(0, 2), Selected_App_key.Substring(2, 2), Selected_App_key.Substring(4, 2), Selected_App_key.Substring(6, 4), Selected_App_key.Substring(10, 8));

                //    }
                //    else
                //        ShowHierachyandApplNo(Selected_App_key.Substring(0, 2), Selected_App_key.Substring(2, 2), Selected_App_key.Substring(4, 2), Selected_App_key.Substring(6, 4), string.Empty);

                //    if (BaseForm.BaseTopApplSelect == "Y" || !string.IsNullOrEmpty(BaseForm.BaseApplicationNo.Trim()))
                //    {
                //        BtnAddApp.Visible = false;
                //        Btn_First.Visible = BtnP10.Visible = BtnPrev.Visible =
                //        BtnNxt.Visible = BtnN10.Visible = BtnLast.Visible = true;
                //    }
                //}
            }
            //Adv_search = false;
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

                            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                            //{
                            //    if (Entity.Agy == GD_PostIntake.CurrentRow.Cells["CACode"].Value.ToString())
                            //        Entity.Sel_SW = false;
                            //}
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

        int SUM_Intake_CNT = 0;
        private void From_Sum_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (From_Sum_Grid.Rows.Count > 0)
            {
                int ColIdx = From_Sum_Grid.CurrentCell.ColumnIndex;
                int RowIdx = From_Sum_Grid.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (From_Sum_Grid.CurrentRow.Cells["Sum_Sel_SW"].Value.ToString() == "Y")
                        {
                            From_Sum_Grid.CurrentRow.Cells["Sum_Img"].Value = Img_Blank;
                            From_Sum_Grid.CurrentRow.Cells["Sum_Sel_SW"].Value = "N";
                            SUM_Intake_CNT--;
                        }
                        else
                        {
                            From_Sum_Grid.CurrentRow.Cells["Sum_Img"].Value = Img_Tick;
                            From_Sum_Grid.CurrentRow.Cells["Sum_Sel_SW"].Value = "Y";
                            SUM_Intake_CNT++;
                        }
                    }
                }
            }
        }


        private void Cmb_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Mode != "Add_FUND")
                Fill_NonEnrolls_On_FundChange();
        }

        private void Rb_From_Mst_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_From_Mst.Checked)
                Fill_NonEnrolls_From_CASEMST();
        }

        private void Rb_From_Sum_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_From_Sum.Checked)
                Fill_NonEnrolls_From_CASESUM();
        }

        private void Rb_From_Sum_NotInMst_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_From_Sum_NotInMst.Checked)
                Fill_NonEnrolls_From_CASESUM();
        }


        private void contextMenu1_Popup(object sender, EventArgs e)
        {

        }

        private bool Validate_Eff_Date()
        {
            bool Can_Save = true;
            if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {
                if (!Rate_Eff_Date.Checked)
                {
                    _errorProvider.SetError(Rate_Eff_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label21.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                {
                    if (Convert.ToDateTime(Class_Start_Date) > Rate_Eff_Date.Value || Convert.ToDateTime(Class_End_Date) < Rate_Eff_Date.Value)
                    {
                        string Disp_Class_ST_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Class_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        string Disp_Class_END_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Class_End_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        string Error_Msg = label21.Text + " is not within class dates \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date;
                        //_errorProvider.SetError(Rate_Eff_Date, label21.Text + " is not within class dates \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                        Can_Save = false;
                        MessageBox.Show(Error_Msg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Question, onclose: Bypass_Edit_RateEff_Date);

                        ////DialogResult result1 = MessageBox.Show(Error_Msg, "CAP Systems", MessageBoxButtons.YesNo);
                        ////    if (result1 == DialogResult.No)
                        ////        Can_Save = false;
                        ////    else
                        ////        _errorProvider.SetError(Rate_Eff_Date, null);
                    }
                    else
                        _errorProvider.SetError(Rate_Eff_Date, null);
                }
            }

            return Can_Save;
        }


        bool Bypass_EFR_Date = false;
        private void Bypass_Edit_RateEff_Date(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            Bypass_EFR_Date = false;

            if (dialogResult == DialogResult.OK)
            {
                Bypass_EFR_Date = true;
                Btn_Add_Fund_Click(Fund_Enroll_Date, EventArgs.Empty);
            }
        }


        private void Btn_Add_Fund_Click(object sender, EventArgs e)
        {
            bool Can_Save = Validate_Fund_Edit_Controls();

            bool Can_Save_Eff_Date = true;

            if (!Bypass_EFR_Date)
            {
                Can_Save_Eff_Date = Validate_Eff_Date();

                if (!Can_Save_Eff_Date)
                    return;
            }


            if (App_Exists_in_Sel_Fund && Mode == "Add_FUND" && !Can_Save)
            {
                if (App_Exists_in_Sel_Fund_With_Same_Site)
                {
                    string Error_MSG = "App# Already Exists in Selected Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ") in Class (" + Txt_DrawEnroll_Site.Text.Trim() + "/" + Txt_DrawEnroll_Room.Text.Trim() + "/" + Txt_DrawEnroll_AMPM.Text.Trim() + ")";
                    _errorProvider.SetError(Pb_Withdraw_Enroll, Error_MSG.Replace(Consts.Common.Colon, string.Empty));
                    return;
                }
                else
                    _errorProvider.SetError(Pb_Withdraw_Enroll, null);

                MessageBox.Show("Are you sure? \n Do you want to Change Class in selected Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ")", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: AddSameFund_With_Diff_Site);
                return;
            }

            if ((Can_Save && Validate_Fund_Edit_Date()) || (Can_Save && sender != Btn_Add_Fund))
            {

                if (Mode == "Add_FUND")
                {
                    bool Save_Result = false;
                    CaseEnrlEntity New_Post_Record;

                    int NooF_Rows_Processed = 0;
                    New_Post_Record = new CaseEnrlEntity(true);

                    New_Post_Record.Row_Type = "I";
                    New_Post_Record.Agy = BaseForm.BaseAgency;
                    New_Post_Record.Dept = BaseForm.BaseDept;
                    New_Post_Record.Prog = BaseForm.BaseProg;
                    New_Post_Record.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");

                    New_Post_Record.ID = "1";
                    New_Post_Record.Status_Date = New_Post_Record.Enrl_Date = Fund_Enroll_Date.Value.ToShortDateString();

                    //New_Post_Record.Site = Pass_Enroll_List[0].Site;
                    //New_Post_Record.Room = Pass_Enroll_List[0].Room;
                    //New_Post_Record.AMPM = Pass_Enroll_List[0].AMPM;

                    //added by sudheer on 03/27/2018
                    if (Pass_Enroll_List[0].Status == "L")
                    {
                        New_Post_Record.Site = Txt_DrawEnroll_Site.Text;
                        New_Post_Record.Room = Txt_DrawEnroll_Room.Text;
                        New_Post_Record.AMPM = Txt_DrawEnroll_AMPM.Text;

                        //if (Txt_DrawEnroll_Room.Text == "****")
                        //    New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.AMPM = "";
                    }
                    //End by sudheer on 03/27/2018
                    else
                    {
                        New_Post_Record.Site = Txt_DrawEnroll_Site.Text;
                        New_Post_Record.Room = Txt_DrawEnroll_Room.Text;
                        New_Post_Record.AMPM = Txt_DrawEnroll_AMPM.Text;

                        if (Txt_DrawEnroll_Room.Text == "****")
                            New_Post_Record.Site = New_Post_Record.Room = New_Post_Record.AMPM = "";
                    }

                    New_Post_Record.App = Pass_Enroll_List[0].App;

                    New_Post_Record.Group = Pass_Enroll_List[0].Group;
                    //New_Post_Record.Enrl_Date = string.Empty;
                    New_Post_Record.Seq = "1";

                    New_Post_Record.FundHie = ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString();
                    New_Post_Record.Rec_Type = "H";
                    //New_Post_Record.Status = "E";
                    New_Post_Record.Status = ((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString();

                    New_Post_Record.Lstc_Oper = BaseForm.UserID;


                    New_Post_Record.Parent_Rate = New_Post_Record.Funding_Rate = New_Post_Record.Fund_End_date =
                        New_Post_Record.Funding_Code = New_Post_Record.Rate_EFR_date = null;
                    if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
                    {
                        New_Post_Record.Parent_Rate = !string.IsNullOrEmpty(Txt_Parent_Rate.Text) ? Txt_Parent_Rate.Text : null;
                        New_Post_Record.Funding_Rate = !string.IsNullOrEmpty(Txt_Fund_Rate.Text) ? Txt_Fund_Rate.Text : null;
                        New_Post_Record.Fund_End_date = Fund_End_Date.Checked ? Fund_End_Date.Value.ToShortDateString() : null;
                        New_Post_Record.Rate_EFR_date = (Rate_Eff_Date.Checked ? Rate_Eff_Date.Value.ToShortDateString() : null);
                        New_Post_Record.Funding_Code = ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString() == "0" ? null : ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString();
                    }


                    string Tmp_Site_Str = New_Post_Record.Site + "-" + New_Post_Record.Room + "-" + New_Post_Record.AMPM + " - " + New_Post_Record.FundHie;

                    StringBuilder Xml_FieldHistTo_Pass = new StringBuilder();
                    Xml_FieldHistTo_Pass.Append("<Rows>");
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "ADD FUND" + "\" Old_Value = \"" + " " + "\" New_Value = \"" + Tmp_Site_Str + "\"/>");
                    Xml_FieldHistTo_Pass.Append("</Rows>");

                    if (_model.EnrollData.UpdateCASEENRL(New_Post_Record, "Insert", null, string.Empty, (Pass_Enroll_List[0].Status != "W" ? Xml_FieldHistTo_Pass.ToString() : null), out Sql_SP_Result_Message))
                    {
                        Bypass_EFR_Date = Save_Result = false;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                        AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                }
                else
                {



                    bool Save_Result = false;

                    CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
                    StringBuilder Xml_To_Pass = new StringBuilder();
                    StringBuilder Xml_FieldHistTo_Pass = new StringBuilder();
                    Xml_To_Pass.Append("<Rows>");
                    Xml_FieldHistTo_Pass.Append("<Rows>");

                    //

                    //Update_Entity = Pass_Enroll_List[0];
                    Update_Entity = new CaseEnrlEntity(Pass_Enroll_List[0]);
                    Update_Entity.Row_Type = "U";

                    DateTime Tmp_date = Fund_Enroll_Date.Value;
                    TimeSpan Tmp_time = DateTime.Today.TimeOfDay;
                    Update_Entity.Status_Date = Fund_Enroll_Date.Value.ToShortDateString();

                    Update_Entity.Status = ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "0" ? null : ((ListItem)Cmb_Status.SelectedItem).Value.ToString();

                    Update_Entity.Lstc_Oper = BaseForm.UserID;
                    Update_Entity.Desc_1 = Txt_Desc1.Text.Trim();
                    Update_Entity.Desc_2 = Txt_Desc2.Text.Trim();

                    switch (((ListItem)Cmb_Status.SelectedItem).Value.ToString())
                    {
                        case "R": Update_Entity.Denied_Code = Update_Entity.Status_Reason = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString() == "0" ? ((ListItem)Cmb_Reason.SelectedItem).Value.ToString() : null; break;
                        case "P": Update_Entity.Pending_Code = Update_Entity.Status_Reason = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString() == "0" ? ((ListItem)Cmb_Reason.SelectedItem).Value.ToString() : null; break;
                        case "W": Update_Entity.Withdraw_Code = Update_Entity.Status_Reason = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString(); break;
                        default: Update_Entity.Withdraw_Code = Update_Entity.Status_Reason = string.Empty; break;
                    }

                    if (postApp.Text == "Edit Fund Date/Details")
                    {
                        if (Update_Entity.Status == "W")
                        {
                            if (Update_Entity.Withdraw_Date != string.Empty)
                                Update_Entity.Withdraw_Date = Fund_Enroll_Date.Value.ToShortDateString();
                        }
                    }


                    Update_Entity.Parent_Rate = !string.IsNullOrEmpty(Txt_Parent_Rate.Text) ? Txt_Parent_Rate.Text : "0";
                    Update_Entity.Funding_Rate = !string.IsNullOrEmpty(Txt_Fund_Rate.Text) ? Txt_Fund_Rate.Text : "0";
                    Update_Entity.Fund_End_date = Fund_End_Date.Checked ? Fund_End_Date.Value.ToShortDateString() : null;
                    Update_Entity.Rate_EFR_date = Rate_Eff_Date.Checked ? Rate_Eff_Date.Value.ToShortDateString() : null;
                    Update_Entity.Funding_Code = ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString() == "0" ? null : ((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString();

                    if (((ListItem)Cmb_Status.SelectedItem).Value.ToString() != "E")
                        Update_Entity.Withdraw_Code = ((ListItem)Cmb_Edit_Reason.SelectedItem).Value.ToString() == "0" ? null : ((ListItem)Cmb_Edit_Reason.SelectedItem).Value.ToString();

                    Update_Entity.Lstc_Oper = BaseForm.UserID;
                    Update_Entity.Desc_1 = Txt_Desc1.Text.Trim();
                    Update_Entity.Desc_2 = Txt_Desc2.Text.Trim();

                    //Xml_HistTo_Pass.Append("<Row ID = \"" + Pass_Enroll_List[0].ID + "\" Status = \"" + Pass_Enroll_List[0].Status + "\"  From_Date = \"" + Pass_Enroll_List[0].Status_Date + "\" SITE = \"" + Pass_Enroll_List[0].Site + "\" ROOM = \"" + Pass_Enroll_List[0].Room + "\" AMPM = \"" + Pass_Enroll_List[0].AMPM + "\" To_Date = \"" + Update_Entity.Status_Date + "\"/>");
                    //Xml_HistTo_Pass.Append("</Rows>");

                    Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Update_Entity.App + "\" Enrl_GROUP = \"" + Update_Entity.Agy + "\" Enrl_FUND_HIE = \"" + Update_Entity.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Update_Entity.ID + "\" Enrl_SITE = \"" + Update_Entity.Site + "\" Enrl_ROOM = \"" + Update_Entity.Room + "\" Enrl_AMPM = \"" + Update_Entity.AMPM +
                               "\" ENRL_ENRLD_DATE = \"" + Update_Entity.Enrl_Date + "\" Enrl_WDRAW_CODE = \"" + Update_Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + Update_Entity.Withdraw_Date + "\" Enrl_WLIST_DATE = \"" + Update_Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Update_Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Update_Entity.Denied_Date +
                               "\" Enrl_PENDING_CODE= \"" + Update_Entity.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Update_Entity.Pending_Date + "\" Enrl_RANK= \"" + Update_Entity.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Update_Entity.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + Update_Entity.Transc_Type +
                               "\" Enrl_TRANSFER_SITE= \"" + Update_Entity.Tranfr_Site + "\" Enrl_TRANSFER_ROOM= \"" + Update_Entity.Tranfr_Room + "\" Enrl_TRANSFER_AMPM= \"" + Update_Entity.Tranfr_AMPM + "\" Enrl_PARENT_RATE= \"" + Update_Entity.Parent_Rate + "\"  Enrl_FUNDING_CODE= \"" + Update_Entity.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + Update_Entity.Funding_Rate +
                               "\" Enrl_FUND_END_DATE= \"" + Update_Entity.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Update_Entity.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + Update_Entity.Status_Date + "\" Enrl_Curr_Status= \"" + Update_Entity.Status + "\" Sum_Key_To_Update= \"" + " " + "\" Enrl_Preferred_Class= \"" + " " + "\"/>"); //
                    Xml_To_Pass.Append("</Rows>");



                    string Tmp_Fund_Flag = ((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString();


                    Hist_Sw = false;
                    Xml_FieldHistTo_Pass = Prepare_Field_History(Pass_Enroll_List[0], Update_Entity);

                    //if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
                    //{
                    //Xml_FieldHistTo_Pass.Append("<Row Old_Start_Date = \"" + Pass_Enroll_List[0].Status_Date + "\" New_Start_Date = \"" + Update_Entity.Status_Date + "\" Old_Fund_Crit = \"" + Pass_Enroll_List[0].Funding_Code + "\" New_Fund_Crit = \"" + Update_Entity.Funding_Code + "\" Old_Fund_End_Date = \"" + Pass_Enroll_List[0].Fund_End_date + "\" New_Fund_End_Date = \"" + Update_Entity.Fund_End_date + "\" Old_Ref_Date = \"" + Pass_Enroll_List[0].Rate_EFR_date + "\" New_Ref_Date = \"" + Update_Entity.Rate_EFR_date +
                    //                "\" Old_Parent_Rate = \"" + Pass_Enroll_List[0].Parent_Rate + "\" New_Parent_Rate = \"" + Update_Entity.Parent_Rate + "\" Old_Fund_Rate = \"" + Pass_Enroll_List[0].Funding_Rate + "\" New_Fund_Rate = \"" + Update_Entity.Funding_Rate + "\" Old_Reasom = \"" + Pass_Enroll_List[0].Withdraw_Code + "\" New_Reasom = \"" + Update_Entity.Withdraw_Code + "\"/>");
                    //}
                    //else
                    //{
                    //    Xml_FieldHistTo_Pass.Append("<Row Old_Start_Date = \"" + Pass_Enroll_List[0].Status_Date + "\" New_Start_Date = \"" + Update_Entity.Status_Date + "\" Old_Fund_Crit = \"" + Pass_Enroll_List[0].Funding_Code + "\" New_Fund_Crit = \"" + Update_Entity.Funding_Code + "\" Old_Fund_End_Date = \"" + Pass_Enroll_List[0].Fund_End_date + "\" New_Fund_End_Date = \"" + Update_Entity.Fund_End_date + "\" Old_Ref_Date = \"" + Pass_Enroll_List[0].Rate_EFR_date + "\" New_Ref_Date = \"" + Update_Entity.Rate_EFR_date +
                    //                    "\" Old_Parent_Rate = \"" + Pass_Enroll_List[0].Parent_Rate + "\" New_Parent_Rate = \"" + Update_Entity.Parent_Rate + "\" Old_Fund_Rate = \"" + Pass_Enroll_List[0].Funding_Rate + "\" New_Fund_Rate = \"" + Update_Entity.Funding_Rate + "\" Old_Reasom = \"" + Pass_Enroll_List[0].Withdraw_Code + "\" New_Reasom = \"" + Update_Entity.Withdraw_Code + "\"/>");
                    //}


                    //if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), Xml_HistTo_Pass.ToString(), out Sql_SP_Result_Message))

                    bool Status_Date_Prior_To_Attn_LDate = false;
                    string strAttnDate = string.Empty;
                    if (!string.IsNullOrEmpty(Attn_Last_Date.Trim()))
                    {
                        //Commented by Sudheer on 10/21/2020
                        //if (Convert.ToDateTime(Attn_Last_Date) > Fund_Enroll_Date.Value)
                        //    Status_Date_Prior_To_Attn_LDate = true;

                        // Added by murali we have checked siteandroom wise attendance date  on 05/11/2021
                        if (postApp.Text == "Edit Fund Date/Details")
                        {
                            strAttnDate = ChldAttnDB.GetChldAttnDate(Update_Entity.Agy, Update_Entity.Dept, Update_Entity.Prog, Update_Entity.Year, Update_Entity.App, Update_Entity.Site, Update_Entity.Room, Update_Entity.AMPM, Update_Entity.FundHie, string.Empty);
                            if (!string.IsNullOrEmpty(strAttnDate))
                            {
                                if (Fund_Enroll_Date.Value > Convert.ToDateTime(strAttnDate))
                                    Status_Date_Prior_To_Attn_LDate = true;
                            }
                        }
                        else
                        {
                            //Added by Sudheer on 10/21/2020
                            if (Fund_Enroll_Date.Value > Convert.ToDateTime(Attn_First_Date)) //&& Fund_Enroll_Date.Value<Convert.ToDateTime(Attn_Last_Date)
                            {
                                strAttnDate = Attn_First_Date;
                                Status_Date_Prior_To_Attn_LDate = true;
                            }
                        }
                    }

                    if (!Status_Date_Prior_To_Attn_LDate)
                    {
                        if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), null, (Hist_Sw ? Xml_FieldHistTo_Pass.ToString() : null), out Sql_SP_Result_Message))
                        {
                            Bypass_EFR_Date = Save_Result = false;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        GblUpdate_Entity = Update_Entity;
                        GblXml_To_Pass = Xml_To_Pass;
                        GblXml_FieldHistTo_Pass = Xml_FieldHistTo_Pass;
                        // MessageBox.Show("Attendance posted after your From date do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, SoftEditfor_AttnLast_Date, true);

                        AlertBox.Show("Enroll Date should not be greater than First Attendance Date ( " + LookupDataAccess.Getdate(strAttnDate) + " )", MessageBoxIcon.Warning);
                    }

                }
            }
        }

        CaseEnrlEntity GblUpdate_Entity = new CaseEnrlEntity(true);
        StringBuilder GblXml_To_Pass = new StringBuilder();
        StringBuilder GblXml_FieldHistTo_Pass = new StringBuilder();

        // Kranthi: didn't find SoftEditfor_AttnLast_Date function enabled in the code so commenting this code - 12/19/2022
        //private void SoftEditfor_AttnLast_Date(object sender, EventArgs e)
        //{
        //    MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
        //    if (messageBoxWindow.DialogResult == DialogResult.Yes)
        //    {
        //        if (_model.EnrollData.UpdateCASEENRL(GblUpdate_Entity, "Edit", GblXml_To_Pass.ToString(), null, (Hist_Sw ? GblXml_FieldHistTo_Pass.ToString() : null), out Sql_SP_Result_Message))
        //        {
        //            //Bypass_EFR_Date = Save_Result = false;
        //            Bypass_EFR_Date = false;
        //            this.DialogResult = DialogResult.OK;
        //            this.Close();
        //        }
        //        else
        //            MessageBox.Show(Sql_SP_Result_Message, "CAP Systems");
        //    }
        //}

        private void AddSameFund_With_Diff_Site(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.No)
                return;

            Change_Class_WithExisting_Fund();
        }

        private void Change_Class_WithExisting_Fund()
        {
            Module_Code = "02";
            bool Save_Result = true;
            CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
            StringBuilder Xml_To_Pass = new StringBuilder();
            StringBuilder Xml_HistTo_Pass = new StringBuilder();
            Xml_To_Pass.Append("<Rows>");
            Xml_HistTo_Pass.Append("<Rows>");

            //

            Update_Entity.Row_Type = "U";
            Update_Entity.Agy = BaseForm.BaseAgency;
            Update_Entity.Dept = BaseForm.BaseDept;
            Update_Entity.Prog = BaseForm.BaseProg;
            Update_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
            Update_Entity.App = "00000001";
            Update_Entity.FundHie = Sel_Hie;

            Update_Entity.ID = "1";

            //DateTime Tmp_date = Enroll_Date.Value;
            //TimeSpan Tmp_time = DateTime.Today.TimeOfDay;
            //Update_Entity.Status_Date = (Tmp_date + Tmp_time).ToString();
            Update_Entity.Status_Date = Fund_Enroll_Date.Value.ToShortDateString();


            Update_Entity.Status = ((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString();

            Update_Entity.Seq = "1";
            Update_Entity.Rec_Type = "H";
            Update_Entity.Lstc_Oper = BaseForm.UserID;
            Update_Entity.Site = Update_Entity.Room = Update_Entity.Group = Update_Entity.AMPM = Update_Entity.Enrl_Date = string.Empty;

            Update_Entity.Desc_1 = Txt_Desc1.Text.Trim();
            Update_Entity.Desc_2 = Txt_Desc2.Text.Trim();

            Update_Entity.Status = ((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString();

            switch (((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString())
            {
                case "R":
                case "P":
                case "W":
                    Update_Entity.Withdraw_Code = Update_Entity.Status_Reason = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString(); ;
                    break;
            }


            string Tmp_Site = string.Empty, Tmp_Room = string.Empty, Tmp_AMPM = string.Empty; bool Replace_Site = false;

            if (!string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()))
                Tmp_Site = Txt_DrawEnroll_Site.Text.Trim();

            if (!string.IsNullOrEmpty(Txt_DrawEnroll_Room.Text.Trim()))
                Tmp_Room = Txt_DrawEnroll_Room.Text.Trim();

            if (!string.IsNullOrEmpty(Txt_DrawEnroll_AMPM.Text.Trim()))
                Tmp_AMPM = (Txt_DrawEnroll_AMPM.Text.Trim()).Substring(0, 1);

            int Sle_Rec_Count = 0; string Tmp_Reason_Code = string.Empty, Tmp_Prog_Hie = string.Empty;

            //foreach (DataGridViewRow dr in Grid_Applications.Rows)
            //{
            //    int NooF_Rows_Processed = 0;

            //    if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
            //    {
            Sle_Rec_Count++;
            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            {
                //if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App ==
                //    dr.Cells["GD_Key"].Value.ToString())
                {

                    //if (!string.IsNullOrEmpty(dr.Cells["GD_Date"].Value.ToString().Trim()))
                    {
                        //DateTime date = Convert.ToDateTime(dr.Cells["GD_Date"].Value);
                        //TimeSpan time = DateTime.Today.TimeOfDay;
                        Update_Entity.Status_Date = Fund_Enroll_Date.Value.ToShortDateString();
                        Update_Entity.FundHie = Entity.FundHie;

                        switch (((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString())
                        {
                            case "R":
                                Entity.Denied_Date = Update_Entity.Status_Date;
                                Entity.Denied_Code = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString();
                                break;
                            //case "E": Entity.Enrl_Date = Update_Entity.Status_Date; break;
                            case "P":
                                Entity.Pending_Date = Update_Entity.Status_Date;
                                Entity.Pending_Code = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString();
                                break;
                            case "L": Entity.Wiait_Date = Update_Entity.Status_Date; break;
                            case "W":
                                Entity.Withdraw_Date = Update_Entity.Status_Date;
                                Entity.Withdraw_Code = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString();
                                break;
                        }
                        Entity.Row_Type = "U";

                        Tmp_Reason_Code = null;
                        if (Entity.Status == "W")
                            Tmp_Reason_Code = Entity.Status_Reason;

                        Entity.Lstc_Oper = BaseForm.UserID;
                        Entity.Desc_1 = Txt_Desc1.Text.Trim();
                        Entity.Desc_2 = Txt_Desc2.Text.Trim();


                        bool From_Dept_WaitList = (!string.IsNullOrEmpty(Entity.Site.Trim()) ? false : true); // 02082014
                        if (Module_Code == "02")
                            Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Status = \"" + Entity.Status + "\"  From_Date = \"" + Entity.Status_Date + "\" SITE = \"" + (From_Dept_WaitList ? Entity.Mst_Site : Entity.Site) + "\" ROOM = \"" + (From_Dept_WaitList ? "****" : Entity.Room) + "\" AMPM = \"" + (From_Dept_WaitList ? "*" : Entity.AMPM) + "\" To_Date = \"" + Update_Entity.Status_Date + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");
                        else
                        {
                            Tmp_Prog_Hie = Entity.FundHie.Trim();
                            Tmp_Prog_Hie = Tmp_Prog_Hie + "      ".Substring(0, 6 - Tmp_Prog_Hie.Length);
                            Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Status = \"" + Entity.Status + "\"  From_Date = \"" + Entity.Status_Date + "\" SITE = \"" + Tmp_Prog_Hie.Substring(0, 2) + "\" ROOM = \"" + Tmp_Prog_Hie.Substring(2, 2) + "\" AMPM = \"" + Tmp_Prog_Hie.Substring(4, 2) + "\" To_Date = \"" + Update_Entity.Status_Date + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");
                        }

                        Replace_Site = false;
                        if (string.IsNullOrEmpty(Entity.Site.Trim()) && string.IsNullOrEmpty(Entity.Room.Trim()) && string.IsNullOrEmpty(Entity.AMPM.Trim()) && Module_Code == "02")
                        {
                            Replace_Site = true;
                            Entity.Site = Tmp_Site;
                            Entity.Room = Tmp_Room;
                            Entity.AMPM = Tmp_AMPM;
                        }

                        //if (Cb_Withdraw_Enroll.Checked)
                        Update_Entity.Enrl_Date = Entity.Enrl_Date;

                        //Update_Entity.Status_Date
                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Entity.App + "\" Enrl_GROUP = \"" + Entity.Agy + "\" Enrl_FUND_HIE = \"" + Entity.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Entity.ID + "\" Enrl_SITE = \"" + Entity.Site + "\" Enrl_ROOM = \"" + Entity.Room + "\" Enrl_AMPM = \"" + Entity.AMPM +
                            //"\" ENRL_ENRLD_DATE = \"" + (Update_Entity.Status != "E" ? Entity.Enrl_Date : (string.IsNullOrEmpty(Entity.Enrl_Date.Trim()) ? Update_Entity.Status_Date : Entity.Enrl_Date)) + "\" Enrl_WDRAW_CODE = \"" + Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + (Update_Entity.Status != "W" ? Entity.Withdraw_Date : Update_Entity.Status_Date) + "\" Enrl_WLIST_DATE = \"" + Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Entity.Denied_Date +
                            "\" ENRL_ENRLD_DATE = \"" + Entity.Enrl_Date + "\" Enrl_WDRAW_CODE = \"" + Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + (Update_Entity.Status != "W" ? Entity.Withdraw_Date : Update_Entity.Status_Date) + "\" Enrl_WLIST_DATE = \"" + Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Entity.Denied_Date +
                                   "\" Enrl_PENDING_CODE= \"" + Entity.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Entity.Pending_Date + "\" Enrl_RANK= \"" + Entity.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Entity.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + Entity.Transc_Type +
                                   "\" Enrl_TRANSFER_SITE= \"" + Entity.Tranfr_Site + "\" Enrl_TRANSFER_ROOM= \"" + Entity.Tranfr_Room + "\" Enrl_TRANSFER_AMPM= \"" + Entity.Tranfr_AMPM + "\" Enrl_PARENT_RATE= \"" + (!string.IsNullOrEmpty(Entity.Parent_Rate.Trim()) ? Entity.Parent_Rate : "0") + "\"  Enrl_FUNDING_CODE= \"" + Entity.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + (!string.IsNullOrEmpty(Entity.Funding_Rate.Trim()) ? Entity.Funding_Rate : "0") +
                                   "\" Enrl_FUND_END_DATE= \"" + Entity.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Entity.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + Update_Entity.Status_Date + "\" Enrl_Curr_Status= \"" + Update_Entity.Status + "\" Sum_Key_To_Update= \"" + " " + "\" Enrl_Preferred_Class= \"" + "N" +
                                   "\" Enrl_Withdraw_Enrl_Sw= \"" + "R" + "\" Enrl_Withdraw_Enrl_Site= \"" + ((Update_Entity.Status == "L" && Txt_DrawEnroll_Room.Text == "****") ? null : Txt_DrawEnroll_Site.Text) + "\" Enrl_Withdraw_Enrl_Room= \"" + ((Update_Entity.Status == "L" && Txt_DrawEnroll_Room.Text == "****") ? null : Txt_DrawEnroll_Room.Text) + "\" Enrl_Withdraw_Enrl_AMPM= \"" + ((Update_Entity.Status == "L" && Txt_DrawEnroll_AMPM.Text == "*") ? null : Txt_DrawEnroll_AMPM.Text) + "\" Enrl_Withdraw_Enrl_Fund= \"" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + "\"/>");
                    }
                    //else
                    //{
                    //    Save_Result = false;
                    //    break;
                    //}
                }
            }
            //    }
            //}
            Xml_To_Pass.Append("</Rows>");
            Xml_HistTo_Pass.Append("</Rows>");

            if (Sle_Rec_Count > 0)
            {
                if (Save_Result)
                {
                    if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), Xml_HistTo_Pass.ToString(), null, out Sql_SP_Result_Message))
                    {
                        Save_Result = false;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                        AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                }
                else
                    AlertBox.Show("Please Provide Date", MessageBoxIcon.Warning);
            }
            else
                AlertBox.Show("Please Select atleast One Record", MessageBoxIcon.Warning);
        }


        bool Hist_Sw = false;
        private StringBuilder Prepare_Field_History(CaseEnrlEntity Old_Entity, CaseEnrlEntity New_Entity)
        {
            Hist_Sw = false;
            StringBuilder Xml_FieldHistTo_Pass = new StringBuilder();
            Xml_FieldHistTo_Pass.Append("<Rows>");

            Old_Entity.Status_Date = Convert.ToDateTime(Old_Entity.Status_Date).ToShortDateString();
            New_Entity.Status_Date = Convert.ToDateTime(New_Entity.Status_Date).ToShortDateString();

            if (Old_Entity.Status_Date != New_Entity.Status_Date)
            {
                Hist_Sw = true;
                Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Start Date" + "\" Old_Value = \"" + Old_Entity.Status_Date + "\" New_Value = \"" + New_Entity.Status_Date + "\"/>");
            }

            if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {

                if (Old_Entity.Funding_Code != New_Entity.Funding_Code)
                {
                    Hist_Sw = true;
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Fund Category" + "\" Old_Value = \"" + Old_Entity.Funding_Code + "\" New_Value = \"" + New_Entity.Funding_Code + "\"/>");
                }

                if (!string.IsNullOrEmpty(Old_Entity.Fund_End_date))
                    Old_Entity.Fund_End_date = Convert.ToDateTime(Old_Entity.Fund_End_date).ToShortDateString();

                if (New_Entity.Fund_End_date == null)
                    New_Entity.Fund_End_date = "";

                if (!string.IsNullOrEmpty(New_Entity.Fund_End_date))
                    New_Entity.Fund_End_date = Convert.ToDateTime(New_Entity.Fund_End_date).ToShortDateString();
                if (Old_Entity.Fund_End_date != New_Entity.Fund_End_date)
                {
                    Hist_Sw = true;
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Fund End Date" + "\" Old_Value = \"" + Old_Entity.Fund_End_date + "\" New_Value = \"" + New_Entity.Fund_End_date + "\"/>");
                }


                if (!string.IsNullOrEmpty(Old_Entity.Rate_EFR_date))
                    Old_Entity.Rate_EFR_date = Convert.ToDateTime(Old_Entity.Rate_EFR_date).ToShortDateString();

                if (New_Entity.Fund_End_date == null)
                    New_Entity.Fund_End_date = "";

                if (!string.IsNullOrEmpty(New_Entity.Rate_EFR_date))
                    New_Entity.Rate_EFR_date = Convert.ToDateTime(New_Entity.Rate_EFR_date).ToShortDateString();

                if (Old_Entity.Rate_EFR_date != New_Entity.Rate_EFR_date)
                {
                    Hist_Sw = true;
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Rate Effective" + "\" Old_Value = \"" + Old_Entity.Rate_EFR_date + "\" New_Value = \"" + New_Entity.Rate_EFR_date + "\"/>");
                }
                if (Old_Entity.Parent_Rate != New_Entity.Parent_Rate)
                {
                    Hist_Sw = true;
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Parent Daily Rate" + "\" Old_Value = \"" + Old_Entity.Parent_Rate + "\" New_Value = \"" + New_Entity.Parent_Rate + "\"/>");
                }
                if (Old_Entity.Funding_Rate != New_Entity.Funding_Rate)
                {
                    Hist_Sw = true;
                    Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Funding Daily Rate" + "\" Old_Value = \"" + Old_Entity.Funding_Rate + "\" New_Value = \"" + New_Entity.Funding_Rate + "\"/>");
                }
            }

            if (Old_Entity.Withdraw_Code != New_Entity.Withdraw_Code)
            {
                Hist_Sw = true;
                if (New_Entity.Withdraw_Code == null)
                    New_Entity.Withdraw_Code = string.Empty;
                Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Reason" + "\" Old_Value = \"" + Get_Reason_Desc(Old_Entity.Withdraw_Code) + "\" New_Value = \"" + Get_Reason_Desc(New_Entity.Withdraw_Code) + "\"/>");
            }

            Xml_FieldHistTo_Pass.Append("</Rows>");

            return Xml_FieldHistTo_Pass;
        }

        private string Get_Reason_Desc(string Reason_Code)
        {
            string Reason_Desc = string.Empty;

            if (!string.IsNullOrEmpty(Reason_Code.Trim()))
            {
                foreach (SPCommonEntity Entity in Edit_ReasonDesc_List)
                {
                    if (Reason_Code == Entity.Code)
                    {
                        Reason_Desc = Entity.Desc;
                        break;
                    }
                }
            }

            return Reason_Desc;
        }

        private void Cmb_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Status.Items.Count > 0)
            {
                switch (((ListItem)Cmb_Status.SelectedItem).Value.ToString())
                {
                    case "P":
                    case "R":
                    case "W": Cmb_Reason.Enabled = true; break;
                    default: Cmb_Reason.Enabled = false; break;
                }
                SetComboBoxValue(Cmb_Reason, "0");
            }
        }


        private bool Validate_Fund_Edit_Date()
        {
            bool Can_Save = true;

            if (!Fund_Enroll_Date.Checked)
            {
                _errorProvider.SetError(Fund_Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label8.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
            {
                bool Validate_Trf_Date = true; string Error_Msg = "", Site = "", Room = "", ampm = "";
                if (Mode == "Add_FUND")
                {
                    Site = Txt_DrawEnroll_Site.Text.Trim(); Room = Txt_DrawEnroll_Room.Text.Trim(); ampm = Txt_DrawEnroll_AMPM.Text.Trim();
                }
                else
                {
                    Site = Pass_Enroll_List[0].Site.Trim(); Room = Pass_Enroll_List[0].Room.Trim(); ampm = Pass_Enroll_List[0].AMPM.Trim();
                }

                if (!string.IsNullOrEmpty(Site))
                {
                    string Class_start_date = string.Empty, Class_end_date = string.Empty;
                    foreach (CaseSiteEntity Ent in HSS_Site_List)
                    {
                        if (Ent.SiteNUMBER == Site && Ent.SiteROOM.Trim() == Room && Ent.SiteAM_PM.Trim() == ampm)
                        {
                            Class_start_date = Ent.SiteCLASS_START;
                            Class_end_date = Ent.SiteCLASS_END;
                            break;
                        }
                    }

                    string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_start_date);
                    string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_end_date);

                    if (!string.IsNullOrEmpty(Class_start_date.Trim()) && !string.IsNullOrEmpty(Class_end_date.Trim()))
                    {
                        if (Convert.ToDateTime(Class_start_date) > Fund_Enroll_Date.Value || Convert.ToDateTime(Class_end_date) < Fund_Enroll_Date.Value)
                        {
                            Error_Msg = label8.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date;
                            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                            Validate_Trf_Date = false;
                        }
                    }
                }

                if (Validate_Trf_Date)
                    _errorProvider.SetError(Fund_Enroll_Date, null);
                else
                {
                    MessageBox.Show(Error_Msg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Question, onclose: Bypass_Edit_Enroll_Date_Validations);
                    Can_Save = Validate_Trf_Date;
                }
            }
            return Can_Save;
        }

        private void Bypass_Edit_Enroll_Date_Validations(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            //if (messageBoxWindow.DialogResult == DialogResult.OK || messageBoxWindow.DialogResult == DialogResult.Cancel)
            if (dialogResult == DialogResult.OK)
                Btn_Add_Fund_Click(Fund_Enroll_Date, EventArgs.Empty);
        }


        bool App_Exists_in_Sel_Fund = false, App_Exists_in_Sel_Fund_With_Same_Site = false;
        private bool Validate_Fund_Edit_Controls()
        {
            bool Can_Save = true;
            App_Exists_in_Sel_Fund = App_Exists_in_Sel_Fund_With_Same_Site = false;
            string Disp_Class_ST_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Class_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
            string Disp_Class_END_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Class_End_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

            if (!Fund_Enroll_Date.Checked)
            {
                _errorProvider.SetError(Fund_Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label8.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
            {

                //if (Convert.ToDateTime(Class_Start_Date) > Fund_Enroll_Date.Value || Convert.ToDateTime(Class_End_Date) < Fund_Enroll_Date.Value)
                //{
                //    _errorProvider.SetError(Fund_Enroll_Date, label8.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                //    Can_Save = false;
                //}
                //else
                //    _errorProvider.SetError(Fund_Enroll_Date, null);


                //if (Mode == "Edit_FUND" && !string.IsNullOrEmpty(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate.Trim()))  // Rao 12052014
                //{
                //    if (Convert.ToDateTime(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate) > Fund_Enroll_Date.Value)
                //    {
                //        _errorProvider.SetError(Fund_Enroll_Date, label8.Text + " Should be Beyond Last Attendance Posting Date : " + Pass_Enroll_List[0].Enrl_Stat_Attn_LDate);
                //        Can_Save = false;
                //    }
                //    else
                //        if (Can_Save)
                //            _errorProvider.SetError(Fund_Enroll_Date, null);
                //}
                //else
                //    _errorProvider.SetError(Fund_Enroll_Date, null);

                if (Mode == "Edit_FUND" && Pass_Enroll_List[0].Status == "W")
                {
                    if (((ListItem)Cmb_Edit_Reason.SelectedItem).Value.ToString() == "0")
                    {
                        _errorProvider.SetError(Cmb_Edit_Reason, label20.Text);
                        Can_Save = false;
                    }
                    else
                        if (Can_Save)
                        _errorProvider.SetError(Cmb_Edit_Reason, null);
                }
                else
                    _errorProvider.SetError(Cmb_Edit_Reason, null);


                //if (Mode == "Edit_FUND")
                //{
                //    ENRLHIST_Entity Search_Entity = new ENRLHIST_Entity(true);
                //    Search_Entity.ID = Pass_Enroll_List[0].ID;
                //    Search_Entity.Asof_Date = "N";
                //    ENRLHIST_List = _model.EnrollData.Browse_ENRLHIST(Search_Entity, "Browse");

                //    string Error_Msg = string.Empty; bool Dates_Overlaped = false;
                //    foreach (ENRLHIST_Entity Entity in ENRLHIST_List)
                //    {
                //        if (Pass_Enroll_List[0].ID == Entity.ID)
                //        {
                //            if (Convert.ToDateTime(Entity.From_Date) > Fund_Enroll_Date.Value || Convert.ToDateTime(Entity.TO_Date) < Fund_Enroll_Date.Value)
                //            {
                //                Error_Msg = " Given Date Range Overlaps with " + "\n Status : " +Entity.Status + " From : " +  LookupDataAccess.Getdate(Entity.From_Date) + " To  : " +  LookupDataAccess.Getdate(Entity.TO_Date);
                //                Dates_Overlaped = true;
                //                Can_Save = false;
                //                break;
                //            }
                //        }
                //    }

                //    if (Dates_Overlaped)
                //    {
                //        _errorProvider.SetError(Fund_Enroll_Date, Error_Msg.Replace(Consts.Common.Colon, string.Empty));
                //        Can_Save = false;
                //    }
                //    else
                //        _errorProvider.SetError(Fund_Enroll_Date, null);

                //}
            }

            if (((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() == "00")// && Mode == "Add_FUND")
            {
                _errorProvider.SetError(Cmb_Add_Fund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label6.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_Add_Fund, null);


            if (((ListItem)Cmb_Fund_Category.SelectedItem).Value.ToString() == "0" && ((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {
                _errorProvider.SetError(Cmb_Fund_Category, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label16.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_Fund_Category, null);


            if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            {
                //_errorProvider.SetError(Fund_End_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label12.Text.Replace(Consts.Common.Colon, string.Empty)));
                //Can_Save = false;

                if (!Fund_End_Date.Checked)
                {
                    _errorProvider.SetError(Fund_End_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label12.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                {
                    //if (!Fund_End_Date.Checked)
                    //{
                    //    _errorProvider.SetError(Fund_End_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label12.Text.Replace(Consts.Common.Colon, string.Empty)));
                    //    Can_Save = false;
                    //}
                    //else
                    _errorProvider.SetError(Fund_End_Date, null);
                }

                //////if (!Rate_Eff_Date.Checked)
                //////{
                //////    _errorProvider.SetError(Rate_Eff_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label21.Text.Replace(Consts.Common.Colon, string.Empty)));
                //////    Can_Save = false;
                //////}
                //////else
                //////{
                //////    if (Convert.ToDateTime(Class_Start_Date) > Rate_Eff_Date.Value || Convert.ToDateTime(Class_End_Date) < Rate_Eff_Date.Value)
                //////    {
                //////        _errorProvider.SetError(Rate_Eff_Date, label21.Text + " is not within class dates \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                //////        //if (MessageBox.Show("Save Before Quit?", "Would you like to save your settings before you Quit?", MessageBoxButtons.YesNo) == DialogResult.No)
                //////        //    Can_Save = false;
                //////        //else
                //////        //    _errorProvider.SetError(Rate_Eff_Date, null);

                //////        DialogResult result1 = MessageBox.Show("Is Dot Net Perls awesome?", "Important Question",MessageBoxButtons.YesNo);
                //////        if (result1 == DialogResult.No)
                //////            Can_Save = false;
                //////        else
                //////            _errorProvider.SetError(Rate_Eff_Date, null);
                //////    }
                //////    else
                //////        _errorProvider.SetError(Rate_Eff_Date, null);
                //////}
            }
            ////else
            ////    _errorProvider.SetError(Fund_End_Date, null);

            //if (!Rate_Eff_Date.Checked && ((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() != "Y")
            //{
            //    _errorProvider.SetError(Rate_Eff_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label21.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    Can_Save = false;
            //}
            //else
            //    _errorProvider.SetError(Rate_Eff_Date, null);

            if (Mode == "Add_FUND")// && Pass_Enroll_List[0].Status == "W")
            {
                if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()) || string.IsNullOrEmpty(Txt_DrawEnroll_Room.Text.Trim()) || string.IsNullOrEmpty(Txt_DrawEnroll_AMPM.Text.Trim()))
                {
                    _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label63.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                {
                    bool App_Exists_in_Sel_Class = false, Is_Class_Spec = true;
                    string Error_MSG = string.Empty;
                    //if (Pass_Enroll_List[0].Status == "W")

                    if (((ListItem)Cmb_Post_Status.SelectedItem).Value.ToString() == "E")
                    {
                        if (Txt_DrawEnroll_Room.Text.Trim() == "****" || Txt_DrawEnroll_AMPM.Text.Trim() == "*")
                        {
                            _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format("Class Details Must be Specific"));
                            Is_Class_Spec = false;
                        }
                    }

                    if (Is_Class_Spec)
                    {
                        if (((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() != "00")
                        {
                            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
                            Search_Entity.Agy = BaseForm.BaseAgency;
                            Search_Entity.Dept = BaseForm.BaseDept;
                            Search_Entity.Prog = BaseForm.BaseProg;
                            Search_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
                            Search_Entity.ID = Pass_Enroll_List[0].ID;
                            Search_Entity.Rec_Type = "H";
                            Search_Entity.Enrl_Status_Not_Equalto = "T";
                            List<CaseEnrlEntity> App_Mult_Fund_List = new List<CaseEnrlEntity>();
                            App_Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");

                            if(App_Mult_Fund_List.Count>0)
                            {
                                if (Txt_DrawEnroll_Room.Text.Trim() != "****")
                                {
                                    CaseEnrlEntity entity = App_Mult_Fund_List.Find(u => u.FundHie == ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() && u.Site.Trim() == Txt_DrawEnroll_Site.Text.Trim() && u.Room.Trim() == Txt_DrawEnroll_Room.Text.Trim() && u.AMPM == Txt_DrawEnroll_AMPM.Text.Trim());
                                    if (entity != null)
                                    {
                                        App_Exists_in_Sel_Fund_With_Same_Site = true;
                                        App_Exists_in_Sel_Class = true;
                                    }
                                }
                                else
                                {
                                    CaseEnrlEntity entity = App_Mult_Fund_List.Find(u => u.FundHie == ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() && u.Site.Trim() == Txt_DrawEnroll_Site.Text.Trim() && u.Room.Trim() == Txt_DrawEnroll_Room.Text.Trim() && u.AMPM == Txt_DrawEnroll_AMPM.Text.Trim());
                                    if (entity != null)
                                    {
                                        App_Exists_in_Sel_Fund_With_Same_Site = true;
                                        App_Exists_in_Sel_Class = true;
                                    }
                                }
                            }

                            //foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                            //{
                            //    if (Txt_DrawEnroll_Room.Text.Trim() != "****")
                            //    {
                            //        if (Entity.FundHie == ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString())
                            //        //Entity.Site == Txt_DrawEnroll_Site.Text.Trim() && // Asked to Check Only Fund Because App# Can Exists in Any Fund Only Once 11/11/2014
                            //        //Entity.Room == Txt_DrawEnroll_Room.Text.Trim() &&
                            //        //Entity.AMPM == Txt_DrawEnroll_AMPM.Text.Trim())
                            //        {
                            //            //Error_MSG = "App# Already Exists in Selected Class (" + Entity.Site + "/" + Entity.Room + "/" + Entity.AMPM + ") - Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ")";
                            //            Error_MSG = "App# Already Exists in Selected Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ") in Class (" + Entity.Site + "/" + Entity.Room + "/" + Entity.AMPM + ")";

                            //            if (Entity.Site == Txt_DrawEnroll_Site.Text.Trim() &&
                            //               Entity.Room == Txt_DrawEnroll_Room.Text.Trim() &&
                            //               Entity.AMPM == Txt_DrawEnroll_AMPM.Text.Trim())
                            //                App_Exists_in_Sel_Fund_With_Same_Site = true;

                            //            App_Exists_in_Sel_Class = true; break;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (Entity.FundHie == ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString())
                            //        // string.IsNullOrEmpty(Entity.Site.Trim()) && // Asked to Check Only Fund Because App# Can Exists in Any Fund Only Once 11/11/2014
                            //        //string.IsNullOrEmpty(Entity.Room.Trim()) &&
                            //        //string.IsNullOrEmpty(Entity.AMPM.Trim()))
                            //        {
                            //            //Error_MSG = "App# Already Exists in Selected Class (" + Txt_DrawEnroll_Site.Text + "/" + Txt_DrawEnroll_Room.Text + "/" + Txt_DrawEnroll_AMPM.Text + ") - Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ")";
                            //            Error_MSG = "App# Already Exists in Selected Fund (" + ((ListItem)Cmb_Add_Fund.SelectedItem).Value.ToString() + ") In Class (" + Txt_DrawEnroll_Site.Text + "/" + Txt_DrawEnroll_Room.Text + "/" + Txt_DrawEnroll_AMPM.Text + ")";

                            //            if (Entity.Site == Txt_DrawEnroll_Site.Text.Trim() &&
                            //               Entity.Room == Txt_DrawEnroll_Room.Text.Trim() &&
                            //               Entity.AMPM == Txt_DrawEnroll_AMPM.Text.Trim())
                            //                App_Exists_in_Sel_Fund_With_Same_Site = true;

                            //            App_Exists_in_Sel_Class = true; break;
                            //        }
                            //    }
                            //}
                        }

                        if (App_Exists_in_Sel_Class)
                        {
                            //_errorProvider.SetError(Pb_Withdraw_Enroll, Error_MSG.Replace(Consts.Common.Colon, string.Empty));
                            Can_Save = false;
                            App_Exists_in_Sel_Fund = true;
                        }
                        else
                            _errorProvider.SetError(Pb_Withdraw_Enroll, null);
                    }
                }

                // Validate Yeswanth

                //if (((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString() == "00")
                //{
                //    _errorProvider.SetError(Cmb_DrawEnroll_Fund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label65.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Save = false;
                //}
                //else
                //    _errorProvider.SetError(Cmb_DrawEnroll_Fund, null);
            }



            return Can_Save;
        }

        private bool Validate_Transfer_Date()
        {
            bool Can_Save = true;
            if (propTransferSiteSwitch != "Y")
            {
                if (!Transfer_Date.Checked)
                {
                    _errorProvider.SetError(Transfer_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label29.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                {
                    //string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_Start_Date);
                    //string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_End_Date);  //07112014

                    bool Validate_Trf_Date = true; string Error_Msg = "";
                    if (!string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
                    {
                        string Class_start_date = string.Empty, Class_end_date = string.Empty;
                        foreach (CaseSiteEntity Ent in HSS_Site_List)
                        {
                            if (Ent.SiteNUMBER == Txt_Trans_Site.Text && Ent.SiteROOM.Trim() == Txt_Trans_Room.Text && Ent.SiteAM_PM.Trim() == Txt_Trans_AMPM.Text.Substring(0, 1))
                            {
                                Class_start_date = Ent.SiteCLASS_START;
                                Class_end_date = Ent.SiteCLASS_END;
                                break;
                            }
                        }

                        string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_start_date);
                        string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_end_date);

                        if (Convert.ToDateTime(Class_start_date) > Transfer_Date.Value || Convert.ToDateTime(Class_end_date) < Transfer_Date.Value)
                        {
                            Error_Msg = label29.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date;
                            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
                            Validate_Trf_Date = false;
                        }
                    }



                    if (Convert.ToDateTime(Pass_Enroll_List[0].Status_Date) > Transfer_Date.Value)
                    {
                        Error_Msg = label29.Text + " Should Not be Prior to Enroll Date";
                        //_errorProvider.SetError(Transfer_Date, label29.Text + " Should Not be Prior to Enroll Date".Replace(Consts.Common.Colon, string.Empty));
                        Validate_Trf_Date = false;
                    }

                    if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate.Trim()))
                    {
                        if (Convert.ToDateTime(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate) > Transfer_Date.Value)
                        {
                            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be Beyond Last Attendance Posting Date : " + Attn_Last_Date);
                            Error_Msg = label29.Text + " Should be Beyond Last Attendance Posting Date : " + LookupDataAccess.Getdate(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate);
                            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be Beyond Last Attendance Posting Date : " + LookupDataAccess.Getdate(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate));
                            Validate_Trf_Date = false;
                        }
                    }

                    if (Validate_Trf_Date)
                        _errorProvider.SetError(Transfer_Date, null);
                    else
                    {
                        if (!App_Exists_In_Transfer)
                            MessageBox.Show(Error_Msg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Question, onclose: Bypass_Enroll_Date_Validations);
                        Can_Save = Validate_Trf_Date;
                    }
                }
            }
            return Can_Save;
        }

        private void Bypass_Enroll_Date_Validations(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

            //if (messageBoxWindow.DialogResult == DialogResult.OK || messageBoxWindow.DialogResult == DialogResult.Cancel)
            if (dialogResult == DialogResult.OK)
                Btn_Transfer_Click(Transfer_Date, EventArgs.Empty);
        }

        private bool Validate_Transfer_Controls()
        {
            bool Can_Save = true;

            if (string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
            {
                _errorProvider.SetError(Pb_Site_Search, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label26.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
            {
                if (propTransferSiteSwitch != "Y")
                {
                    bool App_Exists_in_Sel_Class = false;
                    string Error_MSG = string.Empty;
                    if (Pass_Enroll_List[0].Status == "E")
                    {
                        CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
                        Search_Entity.Agy = BaseForm.BaseAgency;
                        Search_Entity.Dept = BaseForm.BaseDept;
                        Search_Entity.Prog = BaseForm.BaseProg;
                        Search_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
                        Search_Entity.ID = Pass_Enroll_List[0].ID;
                        Search_Entity.Rec_Type = "H";
                        Search_Entity.Enrl_Status_Not_Equalto = "T";
                        List<CaseEnrlEntity> App_Mult_Fund_List = new List<CaseEnrlEntity>();
                        App_Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");

                        foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                        {
                            if (Entity.Site == Txt_Trans_Site.Text.Trim() &&
                               Entity.Room == Txt_Trans_Room.Text.Trim() &&
                               Entity.AMPM == (Txt_Trans_AMPM.Text.Substring(0, 1)).Trim() &&
                               Entity.FundHie == ((ListItem)Cmb_Trans_Fund.SelectedItem).Value.ToString())
                            {
                                Error_MSG = "App# Already Exists in Selected Class (" + Entity.Site + "/" + Entity.Room + "/" + Entity.AMPM + ") - Fund (" + ((ListItem)Cmb_Trans_Fund.SelectedItem).Value.ToString() + ")";
                                App_Exists_in_Sel_Class = true; break;
                            }
                        }
                    }
                    if (App_Exists_in_Sel_Class)
                    {
                        _errorProvider.SetError(Pb_Site_Search, Error_MSG.Replace(Consts.Common.Colon, string.Empty));
                        Can_Save = false;
                    }
                    else
                        _errorProvider.SetError(Pb_Site_Search, null);


                    if (((ListItem)Cmb_Trans_Reason.SelectedItem).Value.ToString() == "0")
                    {
                        _errorProvider.SetError(Cmb_Trans_Reason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label31.Text.Replace(Consts.Common.Colon, string.Empty)));
                        Can_Save = false;
                    }
                    else
                        _errorProvider.SetError(Cmb_Trans_Reason, null);
                }
            }

            //if (!Transfer_Date.Checked)
            //{
            //    _errorProvider.SetError(Transfer_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label29.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    Can_Save = false;
            //}
            //else
            //{
            //    //string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_Start_Date);
            //    //string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_End_Date);  //07112014

            //    bool Validate_Trf_Date = true; string Error_Msg = "";
            //    if (!string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
            //    {
            //        string Class_start_date = string.Empty, Class_end_date = string.Empty;
            //        foreach (CaseSiteEntity Ent in HSS_Site_List)
            //        {
            //            if (Ent.SiteNUMBER == Txt_Trans_Site.Text && Ent.SiteROOM.Trim() == Txt_Trans_Room.Text && Ent.SiteAM_PM.Trim() == Txt_Trans_AMPM.Text.Substring(0, 1))
            //            {
            //                Class_start_date = Ent.SiteCLASS_START;
            //                Class_end_date = Ent.SiteCLASS_END;
            //                break;
            //            }
            //        }

            //        string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_start_date);
            //        string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_end_date);

            //        if (Convert.ToDateTime(Class_start_date) > Transfer_Date.Value || Convert.ToDateTime(Class_end_date) < Transfer_Date.Value)
            //        {
            //            Error_Msg = label29.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date;
            //            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be within Class Dates Range \n Form: " + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
            //            Validate_Trf_Date = false;
            //        }
            //    }



            //    if (Convert.ToDateTime(Pass_Enroll_List[0].Status_Date) > Transfer_Date.Value)
            //    {
            //        Error_Msg = label29.Text + " Should Not be Prior to Enroll Date";
            //        //_errorProvider.SetError(Transfer_Date, label29.Text + " Should Not be Prior to Enroll Date".Replace(Consts.Common.Colon, string.Empty));
            //        Validate_Trf_Date = false;
            //    }

            //    if (!string.IsNullOrEmpty(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate.Trim()))
            //    {
            //        if (Convert.ToDateTime(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate) > Transfer_Date.Value)
            //        {
            //            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be Beyond Last Attendance Posting Date : " + Attn_Last_Date);
            //            Error_Msg = label29.Text + " Should be Beyond Last Attendance Posting Date : " + LookupDataAccess.Getdate(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate);
            //            //_errorProvider.SetError(Transfer_Date, label29.Text + " Should be Beyond Last Attendance Posting Date : " + LookupDataAccess.Getdate(Pass_Enroll_List[0].Enrl_Stat_Attn_LDate));
            //            Validate_Trf_Date = false;
            //        }
            //    }

            //    if (Validate_Trf_Date)
            //        _errorProvider.SetError(Transfer_Date, null);
            //    else
            //    {
            //        MessageBox.Show(Error_Msg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Question, Bypass_Enroll_Date_Validations, true);
            //        //Can_Save = Validate_Trf_Date;
            //    }
            //}


            return Can_Save;
        }


        private void Cmb_Add_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Add_Fund.Items.Count > 0)
            {
                if (((ListItem)Cmb_Add_Fund.SelectedItem).ID.ToString() == "Y")
                {
                    panel8.Visible = false;
                    Clear_Pane8_Controls();
                    this.Size = new Size(this.Width, 324-(88));
                }
                else
                {
                    panel8.Visible = true;
                    this.Size = new Size(this.Width, 324);
                }
            }
        }

        private void Clear_Pane8_Controls()
        {
            Txt_Parent_Rate.Clear();
            Txt_Fund_Rate.Clear();

            Fund_Enroll_Date.Value = Fund_End_Date.Value = Rate_Eff_Date.Value = DateTime.Today;
            Fund_Enroll_Date.Checked = Fund_End_Date.Checked = Rate_Eff_Date.Checked = false;
        }

        private void Pb_Site_Search_Click(object sender, EventArgs e)
        {
            if (propTransferSiteSwitch == "Y")
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Site", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_Search_Form_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
            else
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_Search_Form_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        private void Site_Search_Form_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                App_Exists_In_Transfer = false;
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_Trans_Site.Text = From_Results[0];
                Txt_Trans_Room.Text = From_Results[1];

                switch (From_Results[2])
                {
                    case "A": Txt_Trans_AMPM.Text = "A - AM Class"; break;
                    case "P": Txt_Trans_AMPM.Text = "P - PM Class"; break;
                    case "E": Txt_Trans_AMPM.Text = "E - Extended Day"; break;
                    case "F": Txt_Trans_AMPM.Text = "F - Full Day"; break;
                }
            }
        }


        bool App_Exists_In_Transfer = false;
        private void Btn_Transfer_Click(object sender, EventArgs e)
        {

            bool Can_Save = Validate_Transfer_Controls();
            if (((Can_Save && Validate_Transfer_Date()) || (Can_Save && sender != Btn_Transfer)) && !App_Exists_In_Transfer)
            {
                if (Mode == "Transfer")
                {
                    if (propTransferSiteSwitch == "Y")
                    {
                        string strApplNo = string.Empty;
                        string strClientIdOut = string.Empty;
                        string strFamilyIdOut = string.Empty;
                        string strSSNNOOut = string.Empty;
                        string strErrorMsg = string.Empty;
                        CaseMstEntity casemstdata = BaseForm.BaseCaseMstListEntity[0];
                        string strOldSite = BaseForm.BaseCaseMstListEntity[0].Site;
                        casemstdata.Mode = "SITE";
                        casemstdata.Site = Txt_Trans_Site.Text;
                        CheckHistoryTableData(strOldSite.Trim(), Txt_Trans_Site.Text.Trim());
                        if (_model.CaseMstData.InsertUpdateCaseMst(casemstdata, out strApplNo, out strClientIdOut, out strFamilyIdOut, out strSSNNOOut, out strErrorMsg))
                        {
                            BaseForm.BaseCaseMstListEntity[0].Site = Txt_Trans_Site.Text;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        bool Save_Result = false;
                        string Tmp_Reason_Code = string.Empty;
                        if (Validate_App_Transfer())
                        {
                            CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
                            StringBuilder Xml_To_Pass = new StringBuilder();
                            StringBuilder Xml_HistTo_Pass = new StringBuilder();
                            Xml_To_Pass.Append("<Rows>");
                            Xml_HistTo_Pass.Append("<Rows>");

                            //

                            Update_Entity = new CaseEnrlEntity(Pass_Enroll_List[0]);
                            Update_Entity.Row_Type = "T";
                            //if (Pass_Enroll_List[0].Status == "L")
                            //    Update_Entity.Row_Type = "L";

                            DateTime Tmp_date = Fund_Enroll_Date.Value;
                            TimeSpan Tmp_time = DateTime.Today.TimeOfDay;
                            Update_Entity.Status_Date = Transfer_Date.Value.ToShortDateString();
                            Update_Entity.Lstc_Oper = BaseForm.UserID;
                            Update_Entity.Withdraw_Code = Tmp_Reason_Code = ((ListItem)Cmb_Trans_Reason.SelectedItem).Value.ToString();

                            Update_Entity.Parent_Rate = !string.IsNullOrEmpty(Pass_Enroll_List[0].Parent_Rate) ? Pass_Enroll_List[0].Parent_Rate : "0";
                            Update_Entity.Funding_Rate = !string.IsNullOrEmpty(Pass_Enroll_List[0].Funding_Rate) ? Pass_Enroll_List[0].Funding_Rate : "0";

                            Update_Entity.Lstc_Oper = BaseForm.UserID;
                            //Update_Entity.Site = Txt_Trans_Site.Text.Trim();
                            //Update_Entity.Room = Txt_Trans_Room.Text.Trim();
                            //Update_Entity.AMPM = Txt_Trans_AMPM.Text.Trim();

                            StringBuilder Xml_FieldHistTo_Pass = new StringBuilder();

                            string Tmp_New_Site_Details = Txt_Trans_Site.Text.Trim() + "/" + Txt_Trans_Room.Text.Trim() + "/" + (Txt_Trans_AMPM.Text.Trim()).Substring(0, 1);
                            string Tmp_Old_Site_Details = (!string.IsNullOrEmpty(Pass_Enroll_List[0].Site) ? Pass_Enroll_List[0].Site : Pass_Enroll_List[0].Mst_Site) + "/" +
                                                          (!string.IsNullOrEmpty(Pass_Enroll_List[0].Room) ? Pass_Enroll_List[0].Room : "****") + "/" +
                                                          (!string.IsNullOrEmpty(Pass_Enroll_List[0].AMPM) ? Pass_Enroll_List[0].AMPM : "*");
                            Xml_FieldHistTo_Pass.Append("<Rows>");

                            //Tmp_Reason_Code = string.Empty;
                            //if (Pass_Enroll_List[0].Status == "W")
                            //    Tmp_Reason_Code = Pass_Enroll_List[0].Status_Reason;

                            if (((ListItem)Cmb_Trans_Fund.SelectedItem).ID.ToString() == "N")
                            {
                                Update_Entity.Parent_Rate = !string.IsNullOrEmpty(Txt_Trf_Parent_Rate.Text) ? Txt_Trf_Parent_Rate.Text : "0";
                                Update_Entity.Funding_Rate = !string.IsNullOrEmpty(Txt_Trf_Funding_Rate.Text) ? Txt_Trf_Funding_Rate.Text : "0";
                                Update_Entity.Fund_End_date = DT_Trf_Fund_EndDate.Checked ? DT_Trf_Fund_EndDate.Value.ToShortDateString() : null;
                                Update_Entity.Rate_EFR_date = DT_Trf_Eff_Date.Checked ? DT_Trf_Eff_Date.Value.ToShortDateString() : null;
                                Update_Entity.Funding_Code = ((ListItem)Cmb_Trf_FundCat.SelectedItem).Value.ToString() == "0" ? null : ((ListItem)Cmb_Trf_FundCat.SelectedItem).Value.ToString();
                            }

                            if (Rb_Trans_All_No.Checked)
                            {
                                Xml_HistTo_Pass.Append("<Row ID = \"" + Pass_Enroll_List[0].ID + "\" Status = \"" + Pass_Enroll_List[0].Status + "\"  From_Date = \"" + Pass_Enroll_List[0].Status_Date + "\" SITE = \"" +
                                                                        (!string.IsNullOrEmpty(Pass_Enroll_List[0].Site) ? Pass_Enroll_List[0].Site : Pass_Enroll_List[0].Mst_Site) + "\" ROOM = \"" +
                                                                        (!string.IsNullOrEmpty(Pass_Enroll_List[0].Room) ? Pass_Enroll_List[0].Room : "****") + "\" AMPM = \"" +
                                                                        (!string.IsNullOrEmpty(Pass_Enroll_List[0].AMPM) ? Pass_Enroll_List[0].AMPM : "*") + "\" To_Date = \"" + Update_Entity.Status_Date + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");

                                Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Update_Entity.App + "\" Enrl_GROUP = \"" + Update_Entity.Agy + "\" Enrl_FUND_HIE = \"" + Update_Entity.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Update_Entity.ID + "\" Enrl_SITE = \"" + Update_Entity.Site + "\" Enrl_ROOM = \"" + Update_Entity.Room + "\" Enrl_AMPM = \"" + Update_Entity.AMPM +
                                           "\" ENRL_ENRLD_DATE = \"" + Update_Entity.Enrl_Date + "\" Enrl_WDRAW_CODE = \"" + Update_Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + Update_Entity.Withdraw_Date + "\" Enrl_WLIST_DATE = \"" + Update_Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Update_Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Update_Entity.Denied_Date +
                                           "\" Enrl_PENDING_CODE= \"" + Update_Entity.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Update_Entity.Pending_Date + "\" Enrl_RANK= \"" + Update_Entity.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Update_Entity.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + "T" +
                                           "\" Enrl_TRANSFER_SITE= \"" + Txt_Trans_Site.Text.Trim() + "\" Enrl_TRANSFER_ROOM= \"" + Txt_Trans_Room.Text.Trim() + "\" Enrl_TRANSFER_AMPM= \"" + Txt_Trans_AMPM.Text.Trim() + "\" Enrl_PARENT_RATE= \"" + Update_Entity.Parent_Rate + "\"  Enrl_FUNDING_CODE= \"" + Update_Entity.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + Update_Entity.Funding_Rate +
                                           "\" Enrl_FUND_END_DATE= \"" + Update_Entity.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Update_Entity.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + Update_Entity.Status_Date + "\" Enrl_Curr_Status= \"" + Update_Entity.Status + "\" Sum_Key_To_Update= \"" + " " + "\"  Enrl_TRANSFER_ID= \"" + Pass_Enroll_List[0].ID + "\"  />"); // 

                                Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Transfer" + "\" Old_Value = \"" + Tmp_Old_Site_Details + "\" New_Value = \"" + Tmp_New_Site_Details + "\" SDATE = \"" + Convert.ToDateTime(Pass_Enroll_List[0].Status_Date).ToShortDateString() + "\" TDATE = \"" + Update_Entity.Status_Date + "\"/>");
                            }
                            else
                            {
                                Get_Sel_App_Funds_List();

                                foreach (CaseEnrlEntity Entity in Enroll_List)
                                {
                                    if (Entity.Status == "E")
                                    {
                                        Tmp_Reason_Code = string.Empty;
                                        if (Entity.Status == "W")
                                            Tmp_Reason_Code = Entity.Status_Reason;

                                        Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Status = \"" + Entity.Status + "\"  From_Date = \"" + Entity.Status_Date + "\" SITE = \"" + Entity.Site + "\" ROOM = \"" + Entity.Room + "\" AMPM = \"" + Entity.AMPM + "\" To_Date = \"" + Update_Entity.Status_Date + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");

                                        Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Entity.App + "\" Enrl_GROUP = \"" + Entity.Agy + "\" Enrl_FUND_HIE = \"" + Entity.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Entity.ID + "\" Enrl_SITE = \"" + Update_Entity.Site + "\" Enrl_ROOM = \"" + Update_Entity.Room + "\" Enrl_AMPM = \"" + Update_Entity.AMPM +
                                                   "\" ENRL_ENRLD_DATE = \"" + Entity.Enrl_Date + "\" Enrl_WDRAW_CODE = \"" + Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + Entity.Withdraw_Date + "\" Enrl_WLIST_DATE = \"" + Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Entity.Denied_Date +
                                                   "\" Enrl_PENDING_CODE= \"" + Entity.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Entity.Pending_Date + "\" Enrl_RANK= \"" + Entity.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Entity.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + "T" +
                                                   "\" Enrl_TRANSFER_SITE= \"" + Txt_Trans_Site.Text.Trim() + "\" Enrl_TRANSFER_ROOM= \"" + Txt_Trans_Room.Text.Trim() + "\" Enrl_TRANSFER_AMPM= \"" + Txt_Trans_AMPM.Text.Trim() + "\" Enrl_PARENT_RATE= \"" + Update_Entity.Parent_Rate + "\"  Enrl_FUNDING_CODE= \"" + Update_Entity.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + Update_Entity.Funding_Rate +
                                                   "\" Enrl_FUND_END_DATE= \"" + Update_Entity.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Update_Entity.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + Update_Entity.Status_Date + "\" Enrl_Curr_Status= \"" + Entity.Status + "\" Sum_Key_To_Update= \"" + " " + "\" Enrl_Preferred_Class= \"" + " " + "\" Enrl_TRANSFER_ID= \"" + Entity.ID + "\"/>");


                                        //Tmp_New_Site_Details = Update_Entity.Site + "/" + Update_Entity.Room + "/" + Update_Entity.AMPM;
                                        Tmp_Old_Site_Details = Entity.Site + "/" + Entity.Room + "/" + Entity.AMPM;

                                        Xml_FieldHistTo_Pass.Append("<Row  Col_Name = \"" + "Transfer" + "\" Old_Value = \"" + Tmp_Old_Site_Details + "\" New_Value = \"" + Tmp_New_Site_Details + "\" SDATE = \"" + Convert.ToDateTime(Entity.Status_Date).ToShortDateString() + "\" TDATE = \"" + Update_Entity.Status_Date + "\"/>");
                                    }
                                }
                            }


                            Xml_To_Pass.Append("</Rows>");
                            Xml_HistTo_Pass.Append("</Rows>");
                            Xml_FieldHistTo_Pass.Append("</Rows>");

                            //if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), Xml_HistTo_Pass.ToString(), null, out Sql_SP_Result_Message))
                            if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), Xml_HistTo_Pass.ToString(), Xml_FieldHistTo_Pass.ToString(), out Sql_SP_Result_Message))
                            {
                                Save_Result = false;
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                                AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            AlertBox.Show("You can not Transfer Applicant to this Site. \nApplicant already exists in this Site", MessageBoxIcon.Warning);
                            App_Exists_In_Transfer = true;
                        }

                    }
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
                strHistoryDetails = strHistoryDetails + "<HistoryFields><FieldName>Site (Tranfer Enroll History) </FieldName><OldValue>" + strOldSite + "</OldValue><NewValue>" + strNewSite + "</NewValue></HistoryFields>";
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


        private bool Validate_App_Transfer()
        {
            bool Return_Value = true;

            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Join_Mst_Snp = "Y";
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            Search_Entity.Year = BaseForm.BaseYear;
            Search_Entity.App = Pass_Enroll_List[0].App;
            Search_Entity.FundHie = Pass_Enroll_List[0].FundHie;
            Search_Entity.Rec_Type = "H";
            Search_Entity.Enrl_Status_Not_Equalto = "T";

            Search_Entity.Site = Txt_Trans_Site.Text.Trim();
            Search_Entity.Room = Txt_Trans_Room.Text.Trim();
            Search_Entity.AMPM = Txt_Trans_AMPM.Text.Trim().Substring(0, 1);


            //Search_Entity.Site = Search_Entity.Room = Search_Entity.Group = Search_Entity.AMPM = Search_Entity.Enrl_Date = string.Empty;

            List<CaseEnrlEntity> Transfer_Enroll_List = new List<CaseEnrlEntity>();
            Transfer_Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");

            if (Transfer_Enroll_List.Count > 0)
                Return_Value = false;

            return Return_Value;
        }


        List<CaseEnrlEntity> Enroll_List = new List<CaseEnrlEntity>();
        private void Get_Sel_App_Funds_List()
        {
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Join_Mst_Snp = "Y";
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            Search_Entity.Year = BaseForm.BaseYear;
            Search_Entity.Rec_Type = "C";
            Search_Entity.App = Pass_Enroll_List[0].App;

            Search_Entity.Site = Pass_Enroll_List[0].Site;
            Search_Entity.Room = Pass_Enroll_List[0].Room;
            Search_Entity.AMPM = Pass_Enroll_List[0].AMPM;

            ////////Search_Entity.Site = Search_Entity.Room = Search_Entity.Group = Search_Entity.AMPM = Search_Entity.Enrl_Date = string.Empty;

            if (Privileges.ModuleCode == "02")
                Search_Entity.Rec_Type = "H";

            Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");

            //Search_Entity.ID = Pass_Enroll_List[0].ID;
            //Search_Entity.Enrl_Status_Not_Equalto = "T";
            //Enroll_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");
        }

        private void Txt_Parent_Rate_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Parent_Rate.Text.Trim()))
                Txt_Parent_Rate.Text = Set_Max_DecimalValue_For_TextBox(Txt_Parent_Rate.Text);
        }

        private void Txt_Fund_Rate_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Parent_Rate.Text.Trim()))
                Txt_Fund_Rate.Text = Set_Max_DecimalValue_For_TextBox(Txt_Fund_Rate.Text);
        }

        private string Set_Max_DecimalValue_For_TextBox(string Value)
        {
            string Return_Val = string.Empty;

            if (!string.IsNullOrEmpty(Value.Trim()))
            {
                decimal Tmp_Dec_Val = decimal.Parse(Value);
                int integral = (int)decimal.Truncate(Tmp_Dec_Val);
                if (integral > 999)
                    Return_Val = "999.99";
                else
                    Return_Val = ((decimal)(Math.Round(Tmp_Dec_Val, 2))).ToString();
            }

            return Return_Val;
        }

        private void Add_Fund_Panel_Click(object sender, EventArgs e)
        {

        }

        private void Lnk_View_Hist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Lnk_View_Hist.Text == "View History")
            {
                Lnk_View_Hist.Text = "Hide History";
                this.Size = new System.Drawing.Size(531, 463);
                this.Stat_DblHist_Panel.Size = new System.Drawing.Size(528, 460);
                //Stat_DblHist_Panel3.Visible = false;

                Fill_StatChg_History_Grid();
            }
            else
            {
                Lnk_View_Hist.Text = "View History";
                Stat_DblHist_Panel3.Visible = StatChg_Hist_Grid.Visible = false;
                this.Size = new System.Drawing.Size(531, 278);
                this.Stat_DblHist_Panel.Size = new System.Drawing.Size(528, 275);
            }
        }

        private void Fill_StatChg_History_Grid()
        {
            DataSet Fld_Hist_Data = _model.EnrollData.Browse_ENRLFLDHIST(Pass_Enroll_Entity.ID, null, null);

            int Tmp_loop_Cnt = 0;
            //bool Is_Header_Row = false;
            if (Fld_Hist_Data.Tables.Count > 0)
            {
                DataTable Fld_Hist_Table = Fld_Hist_Data.Tables[0];
                DataTable Fld_Hist_Table_Xml = new DataTable();
                string Add_Date = " ", Old_Date = " ", New_Date = " ", Col_Name = " ", Added_By = " ";
                foreach (DataRow dr in Fld_Hist_Table.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["EFHIST_SEQ"].ToString().Trim()))
                    {
                        //Is_Header_Row = true;
                        Fld_Hist_Table_Xml = CommonFunctions.Convert_XMLstring_To_Datatable(dr["EFHIST_XML"].ToString());
                        Add_Date = dr["EFHIST_ADD_DATE"].ToString();
                        if (!string.IsNullOrEmpty(Add_Date.Trim()))
                            Add_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Add_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        Added_By = dr["EFHIST_ADD_OPERATOR"].ToString();

                        foreach (DataRow dr_Xml in Fld_Hist_Table_Xml.Rows)
                        {
                            Old_Date = dr_Xml["Old_Value"].ToString();
                            New_Date = dr_Xml["New_Value"].ToString();
                            Col_Name = dr_Xml["Col_Name"].ToString();

                            if (Col_Name.Contains("Date"))
                                Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            StatChg_Hist_Grid.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date, Added_By);
                            Add_Date = Added_By = " ";

                            Tmp_loop_Cnt++;
                        }
                    }
                }
            }

            Stat_DblHist_Panel3.Visible = StatChg_Hist_Grid.Visible = true;
        }

        string Status_Opr_Mode = string.Empty; int rowIndex = 0;
        private void Pb_Edit_Click(object sender, EventArgs e)
        {
            if ((SDbl_Status_Grid.Rows.Count > 0 && Status_Grid_Cnt > 0 && sender == Pb_Edit) || (sender == Pb_Add))
            //(sender == Pb_Add && Status_Grid_Cnt == 0))       Commented on 10/27/2014 To add multiple history records manually
            {
                New_From_Date.Value = New_To_Date.Value = DateTime.Now;
                if (Lnk_View_Hist.Text == "Hide History")
                {
                    Lnk_View_Hist.Text = "View History";
                    Stat_DblHist_Panel3.Visible = StatChg_Hist_Grid.Visible = false;
                    //this.Size = new System.Drawing.Size(531, 278);
                    //this.Stat_DblHist_Panel.Size = new System.Drawing.Size(528, 275);
                }

                SDbl_Status_Grid.Visible = Stat_Menu_Panel.Visible = false;

                Status_Opr_Mode = (sender == Pb_Edit ? "Edit" : "Add");
                Set_Edit_PanelControls(sender == Pb_Edit ? "Edit" : "Add");

                //this.Stat_DblHist_Edit_Panel.Location = new System.Drawing.Point(-1, 2);
                //this.Stat_DblHist_Edit_Panel.Size = new System.Drawing.Size(524, 203);
                Stat_DblHist_Edit_Panel.Visible = true;

                Stat_DblHist_Edit_Panel.Visible = Stat_DblHist_Save_Panel.Visible = true;

                this.Stat_DblHist_Edit_Panel.Height = 317;//225;
                Stat_DblHist_Edit_Panel.Size = new System.Drawing.Size(this.Stat_DblHist_Edit_Panel.Width, (this.Stat_DblHist_Edit_Panel.Height) - groupBox3.Height);
                //this.Height = 50; this.groupBox3.Visible = false;
                this.Size = new Size(this.Width, 347);//260); //pnlchangeGroup.Height + (Stat_DblHist_Edit_Panel.Height) 
            }
            rowIndex = SDbl_Status_Grid.CurrentRow.Index;
        }

        private void Clear_New_Status_Errors()
        {
            _errorProvider.SetError(New_To_Date, null);
            _errorProvider.SetError(New_From_Date, null);
            _errorProvider.SetError(Cmb_New_Status, null);
            _errorProvider.SetError(Pb_Hist_Site, null);
        }

        private void Set_Edit_PanelControls(string Opr_Mode)
        {
            Clear_New_Status_Errors();
            if (Opr_Mode == "Edit")
            {
                Txt_Old_Status.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Status"].Value.ToString();
                Txt_Old_SDate.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Stat_From"].Value.ToString();
                Txt_Old_EDate.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Stat_To"].Value.ToString();
                Txt_Old_Site.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Class"].Value.ToString();

                Txt_Attn_SDate.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Attn_From"].Value.ToString();
                Txt_Attn_EDate.Text = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Attn_To"].Value.ToString();

                string Tmp_ID_Seq = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_ID_Seq"].Value.ToString();
                if (Module_Code == "02")
                {
                    foreach (ENRLHIST_Entity Entity in ENRLHIST_List)
                    {
                        if (Entity.Seq == Tmp_ID_Seq)
                        {
                            Txt_New_Site.Text = Entity.Site;
                            Txt_New_Room.Text = Entity.Room;
                            Txt_New_AMPM.Text = Entity.AMPM;
                            break;
                        }
                    }
                }

                groupBox2.Text = "Change Details To";
                if (Module_Code == "02")
                {
                    groupBox1.Visible = Attn_Dates_Panel.Visible = true;
                    this.Class_Dates_Panel.Location = new System.Drawing.Point(47, 18);
                    this.groupBox2.Location = new System.Drawing.Point(260, 86);
                }
                else
                {
                    groupBox1.Visible = true;
                    this.groupBox1.Location = new System.Drawing.Point(11, 35);
                    this.groupBox2.Location = new System.Drawing.Point(260, 35);
                }
            }
            else
            {
                groupBox1.Visible = Attn_Dates_Panel.Visible = false;
                groupBox2.Text = "New Status Details";
                Txt_New_Site.Clear(); Txt_New_Room.Clear(); Txt_New_AMPM.Clear();
                this.Class_Dates_Panel.Location = new System.Drawing.Point(90, 18);
                if (Module_Code == "02")
                    this.groupBox2.Location = new System.Drawing.Point(150, 86);
                else
                    this.groupBox2.Location = new System.Drawing.Point(150, 47);
            }

            foreach (CaseSiteEntity Ent in HSS_Site_List)
            {
                if (Ent.SiteNUMBER == Pass_Enroll_Entity.Site && Ent.SiteROOM.Trim() == Pass_Enroll_Entity.Room && Ent.SiteAM_PM.Trim() == Pass_Enroll_Entity.AMPM)
                {
                    Txt_Cls_SDate.Text = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.SiteCLASS_START).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    Txt_Cls_EDate.Text = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ent.SiteCLASS_END).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    break;
                }
            }
        }



        List<CaseSiteEntity> HSS_Site_List = new List<CaseSiteEntity>();
        private void Get_HSS_Sites()
        {
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = BaseForm.BaseAgency;
            Search_Entity.SiteDEPT = BaseForm.BaseDept;
            Search_Entity.SitePROG = BaseForm.BaseProg;
            Search_Entity.SiteYEAR = BaseForm.BaseYear;
            HSS_Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            // Filling Status Combo
            Cmb_Status.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));
            //listItem.Add(new ListItem("Denied", "R"));
            listItem.Add(new ListItem("Accepted", "C"));
            listItem.Add(new ListItem("Enrolled", "E"));
            listItem.Add(new ListItem("Inactive", "N"));
            listItem.Add(new ListItem("Pending", "P"));
            listItem.Add(new ListItem("Wait List", "L"));
            listItem.Add(new ListItem("Withdrawn", "W"));

            Cmb_New_Status.Items.Clear();
            foreach (ListItem Item in listItem)
            {
                if (Item.ID != Pass_Enroll_Entity.Status)
                    Cmb_New_Status.Items.Add(Item);
            }

            if (Cmb_New_Status.Items.Count > 0)
                Cmb_New_Status.SelectedIndex = 0;

            New_From_Date.Checked = New_To_Date.Checked = false;
        }

        private void Btn_Stat_Cancel_Click(object sender, EventArgs e)
        {
            Stat_DblHist_Edit_Panel.Visible = Stat_DblHist_Save_Panel.Visible = false;
            SDbl_Status_Grid.Visible = Stat_Menu_Panel.Visible = true;
            SDbl_Status_Grid.Dock = DockStyle.Fill;

        }

        int Delete_Status_Cnt = 0;
        private void SDbl_Status_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SDbl_Status_Grid.Rows.Count > 0)
            {
                int ColIdx = SDbl_Status_Grid.CurrentCell.ColumnIndex;
                int RowIdx = SDbl_Status_Grid.CurrentCell.RowIndex;
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Sel"].Value.ToString() == true.ToString())
                        {
                            //SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Sel"].Value = false;
                            //SDbl_Status_Grid.CurrentRow.Cells["PI_Sel_SW"].Value = "N";
                            Delete_Status_Cnt++;

                            //foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                            //{
                            //    if (Entity.Agy == SDbl_Status_Grid.CurrentRow.Cells["CACode"].Value.ToString())
                            //        Entity.Sel_SW = false;
                            //}
                        }
                        else
                        {
                            //SDbl_Status_Grid.CurrentRow.Cells["Post_Img"].Value = Img_Tick;
                            //SDbl_Status_Grid.CurrentRow.Cells["SHistChg_Sel"].Value = true;
                            Delete_Status_Cnt--;
                        }
                    }
                }
            }
        }

        private void PbDelete_Click(object sender, EventArgs e)
        {
            if (SDbl_Status_Grid.Rows.Count > 0 && Status_Grid_Cnt > 0)
            {
                if (Delete_Status_Cnt > 0)
                {
                    bool Attn_Exists = false;
                    if (Module_Code == "02")
                    {
                        foreach (DataGridViewRow dr in SDbl_Status_Grid.Rows)  // Rao 12162013
                        {
                            if (dr.Cells["SHistChg_Sel"].Value.ToString() == true.ToString())
                            {
                                if (!string.IsNullOrEmpty(dr.Cells["SHistChg_Attn_From"].Value.ToString().Trim()) || !string.IsNullOrEmpty(dr.Cells["SHistChg_Attn_To"].Value.ToString().Trim()))
                                    Attn_Exists = true;
                            }
                        }
                    }

                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\nSelected History Record(S) " + (Attn_Exists ? "\n\n Selected Record(S) include Attendance Span...!" : ""), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Delete_Sel_CustQues);
                }
                else
                    AlertBox.Show("Please Select atleast One Record to Delete", MessageBoxIcon.Warning);
            }
        }


        private void Delete_Sel_CustQues(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                StringBuilder Xml_HistTo_Pass = new StringBuilder();
                Xml_HistTo_Pass.Append("<Rows>");

                bool Attn_Exists = false;

                foreach (DataGridViewRow dr in SDbl_Status_Grid.Rows)  // Rao 12162013
                {
                    if (dr.Cells["SHistChg_Sel"].Value.ToString() == true.ToString())
                    {
                        Xml_HistTo_Pass.Append("<Row ID = \"" + Pass_Enroll_Entity.ID + "\" Sequence = \"" + dr.Cells["SHistChg_ID_Seq"].Value.ToString() + "\" Status = \"" + dr.Cells["SHistChg_Status"].Value.ToString() + "\"/>");

                        if (!string.IsNullOrEmpty(dr.Cells["SHistChg_Attn_From"].Value.ToString().Trim()) || !string.IsNullOrEmpty(dr.Cells["SHistChg_Attn_To"].Value.ToString().Trim()))
                            Attn_Exists = true;
                    }
                }
                Xml_HistTo_Pass.Append("</Rows>");

                if (!Attn_Exists)
                {
                    if (_model.EnrollData.UpdateENRLHIST("D", Pass_Enroll_Entity.ID, "D", Xml_HistTo_Pass.ToString(), null, BaseForm.UserID, out Sql_SP_Result_Message))
                    {
                        Fill_Status_History();
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                    AlertBox.Show("Selected Record(s) contains Attendance, \n so Status History cannot be Deleted", MessageBoxIcon.Warning);
            }
        }


        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
                Save_Admin_Stat_Edits();
        }

        private void Save_Admin_Stat_Edits()
        {
            CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
            StringBuilder Xml_HistTo_Pass = new StringBuilder();
            StringBuilder Xml_StatChag_HistTo_Pass = new StringBuilder();
            Xml_HistTo_Pass.Append("<Rows>");
            Xml_StatChag_HistTo_Pass.Append("<Rows>");

            string Tmp_New_Status = ((ListItem)Cmb_New_Status.SelectedItem).Value.ToString();

            string Tmp_ID_Seq = "1";
            bool Seq_Found = false;
            if (Status_Opr_Mode == "Edit") // Need to Add Reason for editing in Change History.
            {
                Tmp_ID_Seq = SDbl_Status_Grid.CurrentRow.Cells["SHistChg_ID_Seq"].Value.ToString();
                foreach (ENRLHIST_Entity Entity in ENRLHIST_List)
                {
                    if (Entity.Seq == Tmp_ID_Seq)
                    {
                        Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Sequence = \"" + Entity.Seq + "\" Status = \"" + Tmp_New_Status + "\"  From_Date = \"" + New_From_Date.Value.ToShortDateString() + "\" To_Date = \"" + New_To_Date.Value.ToShortDateString() +
                            "\" SITE  = \"" + Txt_New_Site.Text.Trim() + "\" ROOM  = \"" + Txt_New_Room.Text.Trim() + "\" AMPM  = \"" + Txt_New_AMPM.Text.Trim() + "\"/>");

                        Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "Status" + "\" Old_Value = \"" + Txt_Old_Status.Text + "\" New_Value = \"" + ((ListItem)Cmb_New_Status.SelectedItem).Text.ToString() + "\"/>");
                        Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "From Date" + "\" Old_Value = \"" + Entity.From_Date + "\" New_Value = \"" + New_From_Date.Value.ToShortDateString() + "\"/>");
                        Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "To Date" + "\" Old_Value = \"" + Entity.TO_Date + "\" New_Value = \"" + New_To_Date.Value.ToShortDateString() + "\"/>");
                        Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "Class" + "\" Old_Value = \"" + Txt_Old_Site.Text + "\" New_Value = \"" + Txt_New_Site.Text.Trim() + "/" + Txt_New_Room.Text.Trim() + "/" + Txt_New_AMPM.Text.Trim() + "\"/>");

                        Seq_Found = true;
                        break;
                    }
                }
            }
            else
            {
                Seq_Found = true;
                Xml_HistTo_Pass.Append("<Row ID = \"" + Pass_Enroll_Entity.ID + "\" Status = \"" + Tmp_New_Status + "\"  From_Date = \"" + New_From_Date.Value.ToShortDateString() + "\" To_Date = \"" + New_To_Date.Value.ToShortDateString() +
                    "\" SITE  = \"" + Txt_New_Site.Text.Trim() + "\" ROOM  = \"" + Txt_New_Room.Text.Trim() + "\" AMPM  = \"" + Txt_New_AMPM.Text.Trim() + "\"/>");

                Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "Status" + "\" Old_Value = \"" + " " + "\" New_Value = \"" + ((ListItem)Cmb_New_Status.SelectedItem).Text.ToString() + "\"/>");
                Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "From Date" + "\" Old_Value = \"" + " " + "\" New_Value = \"" + New_From_Date.Value.ToShortDateString() + "\"/>");
                Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "To Date" + "\" Old_Value = \"" + " " + "\" New_Value = \"" + New_To_Date.Value.ToShortDateString() + "\"/>");
                Xml_StatChag_HistTo_Pass.Append("<Row  Col_Name = \"" + "Class" + "\" Old_Value = \"" + " " + "\" New_Value = \"" + Txt_New_Site.Text.Trim() + "/" + Txt_New_Room.Text.Trim() + "/" + Txt_New_AMPM.Text.Trim() + "\"/>");
            }
            Xml_HistTo_Pass.Append("</Rows>");
            Xml_StatChag_HistTo_Pass.Append("</Rows>");

            if (Seq_Found)
            {
                if (_model.EnrollData.UpdateENRLHIST((Status_Opr_Mode == "Edit" ? "U" : "I"), Pass_Enroll_Entity.ID, Tmp_New_Status, Xml_HistTo_Pass.ToString(), Xml_StatChag_HistTo_Pass.ToString(), BaseForm.UserID, out Sql_SP_Result_Message))
                {
                    if (Lnk_View_Hist.Text == "Hide History")
                    {
                        Lnk_View_Hist.Text = "View History";
                        Stat_DblHist_Panel3.Visible = StatChg_Hist_Grid.Visible = false;
                        this.Size = new System.Drawing.Size(531, 278);
                        this.Stat_DblHist_Panel.Size = new System.Drawing.Size(528, 275);
                    }

                    if (Status_Opr_Mode == "Edit")
                        AlertBox.Show("Status History Changed Successfully");
                    else
                        AlertBox.Show("Status History Inserted Successfully");

                    Fill_Status_History();

                    Stat_DblHist_Edit_Panel.Visible = Stat_DblHist_Save_Panel.Visible = false;
                    SDbl_Status_Grid.Visible = Stat_Menu_Panel.Visible = true;
                    SDbl_Status_Grid.Dock = DockStyle.Fill;

                    this.DialogResult = DialogResult.OK;
                }
                else
                    AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
            }
        }

        private void Btn_Stat_Save_Click(object sender, EventArgs e)
        {
            if (Validate_StstChg_Hist_Controls())// && Status_Opr_Mode == "Edit")
            {
                switch (BaseForm.BusinessModuleID)
                {
                    case "02":
                        if (!string.IsNullOrEmpty(HS_Cls_Date_Validation.Trim()))
                            MessageBox.Show("Status Dates should be within Class Dates Range \n Form: " + Txt_Cls_SDate.Text + " To: " + Txt_Cls_EDate.Text, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked);
                        else
                            Save_Admin_Stat_Edits();
                        break;
                    case "03": Save_Admin_Stat_Edits(); break;
                }
            }
        }


        string HS_Cls_Date_Validation = "";
        private bool Validate_StstChg_Hist_Controls()
        {
            bool Can_Save = true;
            HS_Cls_Date_Validation = "";

            if (((ListItem)Cmb_New_Status.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(Cmb_New_Status, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label47.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_New_Status, null);

            if (!New_From_Date.Checked)
            {
                _errorProvider.SetError(New_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label49.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(New_From_Date, null);

            if (!New_To_Date.Checked)
            {
                _errorProvider.SetError(New_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label48.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(New_To_Date, null);

            bool Validate_Dates = true;
            if (New_From_Date.Checked && New_To_Date.Checked)
            {

                if (New_From_Date.Value > New_To_Date.Value)
                {
                    _errorProvider.SetError(New_To_Date, "To Date Should Not be Prior to From Date".Replace(Consts.Common.Colon, string.Empty));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(New_To_Date, null);

                if(Can_Save)
                {
                    if (Module_Code == "02")
                    {
                        if (Txt_Cls_SDate.Text.Trim() != string.Empty && Txt_Cls_EDate.Text.Trim() != string.Empty)
                        {
                            if (Convert.ToDateTime(Txt_Cls_SDate.Text) > New_From_Date.Value || Convert.ToDateTime(Txt_Cls_EDate.Text) < New_From_Date.Value)
                            {
                                _errorProvider.SetError(New_From_Date, (label49.Text + " Should be within Class Dates  Range \n Form: " + Txt_Cls_SDate.Text + " To: " + Txt_Cls_EDate.Text).Replace(Consts.Common.Colon, string.Empty));
                                HS_Cls_Date_Validation = "Should be within Class Dates";
                                //_errorProvider.SetError(New_From_Date, "From Date Should Not be Prior to Class Start Date".Replace(Consts.Common.Colon, string.Empty));
                                //Validate_Dates = false;
                            }
                            else
                                _errorProvider.SetError(New_From_Date, null);

                            if (Convert.ToDateTime(Txt_Cls_SDate.Text) > New_To_Date.Value || Convert.ToDateTime(Txt_Cls_EDate.Text) < New_To_Date.Value)
                            {
                                _errorProvider.SetError(New_To_Date, (label48.Text + " Should be within Class Dates  Range \n Form: " + Txt_Cls_SDate.Text + " To: " + Txt_Cls_EDate.Text).Replace(Consts.Common.Colon, string.Empty));
                                HS_Cls_Date_Validation = "Should be within Class Dates";
                                //_errorProvider.SetError(New_To_Date, "To Date Should Not be Beyond to Class End Date".Replace(Consts.Common.Colon, string.Empty));
                                //Validate_Dates = false;
                            }
                            else
                                _errorProvider.SetError(New_To_Date, null);

                            if (!string.IsNullOrEmpty(Txt_Attn_SDate.Text.Trim()))
                            {
                                if (Convert.ToDateTime(Txt_Attn_SDate.Text) < New_From_Date.Value)
                                {
                                    _errorProvider.SetError(New_From_Date, "From Date Should be Prior to First Attendance Date".Replace(Consts.Common.Colon, string.Empty));
                                    Validate_Dates = false;
                                }
                                else
                                {
                                    if (Validate_Dates)
                                        _errorProvider.SetError(New_From_Date, null);
                                }
                            }




                            if (!string.IsNullOrEmpty(Txt_Attn_EDate.Text.Trim()))
                            {
                                if (Convert.ToDateTime(Txt_Attn_EDate.Text) > New_To_Date.Value)
                                {
                                    _errorProvider.SetError(New_To_Date, "To Date Should be Beyond Last Attendance Date".Replace(Consts.Common.Colon, string.Empty));
                                    Validate_Dates = false;
                                }
                                else
                                {
                                    if (Validate_Dates)
                                        _errorProvider.SetError(New_To_Date, null);
                                }
                            }
                        }
                    }
                }

                

                if (!Validate_Dates)
                    Can_Save = Validate_Dates;
            }

            if (string.IsNullOrEmpty(Txt_New_Site.Text.Trim()))
            {
                _errorProvider.SetError(Pb_Hist_Site, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label54.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Pb_Hist_Site, null);


            return Can_Save;
        }

        private void Fund_Panel_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            if (Rb_HSS_RefSum.Checked)
            {
                this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = true;
                this.Text = Btn_PostIntake.Text = "Send to Post Intake";
                this.PI_Name.HeaderText = "Client Name"; // "Referred By";
                this.PI_Prog.HeaderText = "Referred Date";
                this.PI_Name.Width = 183;//195;
                this.PI_Prog.Width = 90;
                this.Post_Img.Visible = false;


                Fill_HSS_NonEnrolls_From_CASESUM();
            }
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            if (Rb_HSS_NonEnrl.Checked)
            {
                this.PI_Ref_From.Visible = this.PI_Ref_App.Visible = false;
                this.Text = Btn_PostIntake.Text = "Send to Wait List";
                this.PI_Prog.HeaderText = "App#";
                this.PI_Name.Width = 280;
                this.PI_Prog.Width = 80;
                this.Post_Img.Visible = true;

                Fill_NonEnrolls_On_FundChange();
            }
        }


        string Added_Edited_SiteCode = string.Empty; string Added_Edited_HieCode = string.Empty;
        private void Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_New_Site.Text = From_Results[0];
                Txt_New_Room.Text = From_Results[1];
                Txt_New_AMPM.Text = From_Results[2].Substring(0, 1);
            }
        }

        private void Pb_Hist_Site_Click(object sender, EventArgs e)
        {
            if (Module_Code == "02")
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
            else
            {
                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Txt_New_Site.Text + Txt_New_Room.Text + Txt_New_AMPM.Text, "Master", string.Empty, "A", "I");
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();
            }
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            string strPublicCode = string.Empty;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    strPublicCode = row.Code;
                    hierarchy += row.Agency + row.Dept + row.Prog;

                    Txt_New_Site.Text = row.Agency;
                    Txt_New_Room.Text = row.Dept;
                    Txt_New_AMPM.Text = row.Prog;
                }
            }
        }

        private void Fill_WithdrawEnroll_Fund_Combo()
        {
            //List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            //FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");

            //Cmb_DrawEnroll_Fund.Items.Clear();
            //Cmb_DrawEnroll_Fund.Items.Add(new ListItem(" ", "00", " ", (true ? Color.Green : Color.Red)));
            //foreach (SPCommonEntity Entity in FundingList)
            //{
            //    if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
            //        Cmb_DrawEnroll_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
            //}

            //if (Cmb_DrawEnroll_Fund.Items.Count > 0)
            //    Cmb_DrawEnroll_Fund.SelectedIndex = 0;
        }

        private void Pb_Withdraw_Enroll_Click(object sender, EventArgs e)
        {
            Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges, "****");
            SiteSelection.FormClosed += new FormClosedEventHandler(WithdrawEnroll_Site_AddForm_Closed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();
        }

        private void WithdrawEnroll_Site_AddForm_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] From_Results = new string[3];
                From_Results = form.GetSelected_Site_Row();
                Txt_DrawEnroll_Site.Text = From_Results[0];
                Txt_DrawEnroll_Room.Text = From_Results[1];
                Txt_DrawEnroll_AMPM.Text = From_Results[2];

                if (Txt_DrawEnroll_Room.Text.Trim() == "****")
                    Txt_DrawEnroll_Site.Text = Pass_Enroll_List[0].Mst_Site.Trim();
            }
        }

        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void Class_Dates_Panel_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void CASE0010_CASE_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            //CASE0010_StatusChange_Form form = sender as CASE0010_StatusChange_Form;
            //if (form.DialogResult == DialogResult.OK)
            //{
                this.DialogResult = DialogResult.OK;
                this.Close();
            //}
        }

        private void Cmb_Trans_Fund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Trans_Fund.Items.Count > 0)
            {
                if (((ListItem)Cmb_Trans_Fund.SelectedItem).ID.ToString() == "Y")
                {
                    panel9.Visible = false;
                    Btn_Transfer.Location = new System.Drawing.Point(352, 193);
                    panel11.Size = new System.Drawing.Size(451, 200);

                    Transfer_Panel.Size = new System.Drawing.Size(451, 227);
                    // this.Size = new System.Drawing.Size(453, 231);
                    this.Size = new System.Drawing.Size(500, 283);
                    //this.Transfer_Panel.Location = new System.Drawing.Point(2, 1);

                    ///Clear_Pane8_Controls();
                }
                else
                {

                    panel11.Size = new System.Drawing.Size(451, 200);
                    this.Size = new System.Drawing.Size(500, 390);
                    //this.Transfer_Panel.Location = new System.Drawing.Point(2, 1);
                    panel9.Size = new System.Drawing.Size(this.Width, 130);
                    panel9.Visible = true;
                }
            }
        }


        //private void Clear_Pane9_Controls()
        //{
        //    Txt_Parent_Rate.Clear();
        //    Txt_Fund_Rate.Clear();

        //    Fund_Enroll_Date.Value = Fund_End_Date.Value = Rate_Eff_Date.Value = DateTime.Today;
        //    Fund_Enroll_Date.Checked = Fund_End_Date.Checked = Rate_Eff_Date.Checked = false;
        //}






    }
}