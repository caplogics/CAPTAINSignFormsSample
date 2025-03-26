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
using DevExpress.XtraRichEdit.Model;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE0010_StatusChange_Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;

        #endregion

        public CASE0010_StatusChange_Form(BaseForm baseForm, List<CaseEnrlEntity> pass_enroll_list, string module_code, string sel_hie, string class_sDate, string class_eDate, PrivilegeEntity previliges)
        {
            InitializeComponent();

            this.Text = "Edit Status Record(s)";//"Status Change Form";
            //this.Size = new System.Drawing.Size(575, 373);
            // this.Size = new System.Drawing.Size(687, 371);
            /***********************************************************************************************************************************/
            Status_Panel.Visible = true;
            this.Size = new Size(this.Width, this.Height - (IncDoc_Inner_Panel.Height + Fld_History_Panel.Height + History_Panel.Height));
            Tools["Pb_StatChange_Help"].Visible = true;
            /***********************************************************************************************************************************/
            Module_Code = module_code;
            Class_Start_Date = class_sDate;
            Class_End_Date = class_eDate;
            Privileges = previliges;


            BaseForm = baseForm;
            Pass_Enroll_List = pass_enroll_list;
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            //this.Cb_Other_Room.Location = new System.Drawing.Point(318, 39);
            //kranthi// this.Site_Panel.Location = new System.Drawing.Point(169, 32);
            //kranthi// this.Btn_Calu_Dates.Location = new System.Drawing.Point(505, 1);
            //this.Cb_Withdraw_Enroll.Location = new System.Drawing.Point(310, 34);
            //kranthi//this.Cb_Withdraw_Enroll.Location = new System.Drawing.Point(169, 34);
            //this.Cb_Withdraw_Enroll.Size = new System.Drawing.Size(283, 15);            

            Sel_Hie = sel_hie;
            Fill_WithdrawEnroll_Fund_Combo();
            Fill_Combos();
            if (Module_Code == "02")
            {
                Lbl_Rank_Desc.Visible = Lbl_HS_Rank.Visible = Stf_Grid.Visible = Teacher_Panel.Visible = true;
            }


        }

        bool Year_Priv_SW = true;
        public CASE0010_StatusChange_Form(BaseForm baseForm, PrivilegeEntity previliges, CaseEnrlEntity pass_enroll_entity, string module_code) // Enroll Hostory Constructor
        {
            InitializeComponent();

            this.Text = "Enroll History";

            //Lbl_App_NO.Text = pass_enroll_entity.App;
            Privileges = previliges;
            BaseForm = baseForm;
            Pass_Enroll_Entity = pass_enroll_entity;
            Module_Code = module_code;
            FillYearCombo();

            Lbl_Teacher.Text = "   ";
            Lbl_App_NO.Text = pass_enroll_entity.App + ((!string.IsNullOrEmpty(pass_enroll_entity.Snp_F_Name.Trim())) ? " - " + pass_enroll_entity.Snp_F_Name.Trim() : "");


            if (Module_Code == "03")
            {
                //*********Kranthi:: Need to look into this settings later working on History screen******************//
                //Act_Status_Grid.Size = new System.Drawing.Size(512, 104);

                //TimeLine_Panel.Location = new System.Drawing.Point(-1, 154);
                //TimeLine_Panel.Size = new System.Drawing.Size(557, 153);

                //History_Grid.Size = new System.Drawing.Size(550, 125);

                //FldHist_Panel.Location = new System.Drawing.Point(-1, 306);
                //FldHist_Panel.Size = new System.Drawing.Size(557, 177);

                //Fld_Hist_Grid_New.Size = new System.Drawing.Size(550, 152);

            }
            //this.Size = new System.Drawing.Size(559, 486);
            // this.History_Panel.Location = new System.Drawing.Point(1, 1);
            //History_Panel.Visible = true;

            /***********************************************************************************************************************************/
            History_Panel.Visible = true;
            this.Size = new Size(this.Width, this.Height - (IncDoc_Inner_Panel.Height + Fld_History_Panel.Height + Status_Panel.Height)); //+ Stf_Grid.Height
            /***********************************************************************************************************************************/


            Enrl_ID = pass_enroll_entity.ID;
            Pass_Enroll_List = new List<CaseEnrlEntity>();
            _model = new CaptainModel();

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            string Program_Year = "";
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                    Program_Year = dt.Rows[0]["DEP_YEAR"].ToString();
            }
            if (Program_Year.Trim() != BaseForm.BaseYear.Trim())
                Year_Priv_SW = false;

            caseMstList = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Pass_Enroll_Entity.App);

            if (module_code == "02")
            {
                Get_App_in_All_Funds(pass_enroll_entity.ID, string.Empty);
                Fill_HS_Ranks(caseMstList);
                if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                    Btn_Edit_Stat_Hist.Visible = true;

                //Get_Field_History(pass_enroll_entity.ID);
            }
            else
            {
                // this.Size = new System.Drawing.Size(513, 300);
                Lbl_Current_Status.Text = "Current Status";
                this.Hist_Fund_Hie.Visible = true; Hist_Fund_Hie.ShowInVisibilityMenu = true;
                this.Hist_Operator.Width = 95;
                Get_App_in_All_Funds(pass_enroll_entity.ID, string.Empty);
                Rb_Fund_Edit.Visible = Rb_Status_Edit.Visible = Rb_Hist_All.Visible = label11.Visible = false;
                //FldHist_Lable.Visible = Fld_Hist_Grid_New.Visible = false;

                Btn_Bus.Visible = Cb_Show_Entire_Status.Visible = false;
                ActHist_Site.Visible = ActHist_Attn_SDate.Visible = ActHist_Attn_LDate.Visible = false; 
                ActHist_Site.ShowInVisibilityMenu = ActHist_Attn_SDate.ShowInVisibilityMenu = ActHist_Attn_LDate.ShowInVisibilityMenu = false;
                ActHist_Fund.HeaderText = "Hierarchy";
                ActHist_Fund.Width = 250;

                //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                //{
                //    Btn_Edit_Curr_Status_Date.Location = new System.Drawing.Point(495, 49);
                //    Btn_Edit_Curr_Status_Date.Size = new System.Drawing.Size(56, 59); 
                //    Btn_Edit_Curr_Status_Date.Visible = true;
                //}

                //Act_Status_Grid.Visible = label10.Visible = Cb_Show_Entire_Status.Visible = false;
                //this.StatusHist_Lable.Location = new System.Drawing.Point(1, 35);
                //this.History_Grid.Location = new System.Drawing.Point(4, 54);
            }

            Get_History();
            if (Module_Code == "02")
            {
                Lbl_Rank_Desc.Visible = Lbl_HS_Rank.Visible = Stf_Grid.Visible = Teacher_Panel.Visible = true;
                //this.dataGridViewTextBoxColumn5.Visible = this.dataGridViewTextBoxColumn5.ShowInVisibilityMenu = false;
            }
            //else
            //    this.dataGridViewTextBoxColumn5.Visible = this.dataGridViewTextBoxColumn5.ShowInVisibilityMenu = true;

            //Get_Field_History(Pass_Enroll_Entity.ID);
        }



        public CASE0010_StatusChange_Form(BaseForm baseForm, PrivilegeEntity previliges) // Called form Main Form
        {
            InitializeComponent();

            this.Text = "Enroll History";

            //Lbl_App_NO.Text = pass_enroll_entity.App;
            Privileges = previliges;
            BaseForm = baseForm;
            Pass_Enroll_Entity = new CaseEnrlEntity();
            Pass_Enroll_Entity.App = BaseForm.BaseApplicationNo;

            Lbl_App_NO.Text = BaseForm.BaseApplicationNo + ((string.IsNullOrEmpty(BaseForm.BaseApplicationName.Trim())) ? "" : " - " + BaseForm.BaseApplicationName.Trim());
            // this.Size = new System.Drawing.Size(559, 486);
            //this.History_Panel.Location = new System.Drawing.Point(1, 1);
            // History_Panel.Visible = true;

            /***********************************************************************************************************************************/
            History_Panel.Visible = true;
            this.Size = new Size(this.Width, this.Height - (IncDoc_Inner_Panel.Height + Fld_History_Panel.Height + Status_Panel.Height));
            /***********************************************************************************************************************************/


            Module_Code = Privileges.ModuleCode;

            CmbYear.Visible = true;
            FillYearCombo();
            CmbYear.Enabled = true;

            Enrl_ID = string.Empty;
            Pass_Enroll_List = new List<CaseEnrlEntity>();
            _model = new CaptainModel();

            caseMstList = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Pass_Enroll_Entity.App);
            if (Module_Code != "02" && Module_Code != "03")
            {
                List<CaseEnrlEntity> Mult_Fund_List = Ge_Related_Module_0n_App_ID(string.Empty, string.Empty);
                if (Mult_Fund_List.Count > 0)
                {
                    if (Mult_Fund_List[0].Rec_Type == "H")
                        Module_Code = "02";
                    else
                        Module_Code = "03";
                }
            }


            Lbl_Teacher.Text = "   ";
            if (Module_Code == "02")
            {
                Get_App_in_All_Funds(string.Empty, string.Empty);
                Fill_HS_Ranks(caseMstList);

                //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                //    Btn_Edit_Stat_Hist.Visible = true;
            }
            else
            {
                Lbl_Current_Status.Text = "Current Status";
                this.Hist_Fund_Hie.Visible = true; this.Hist_Fund_Hie.ShowInVisibilityMenu = true;
                this.Hist_Operator.Width = 95;
                Get_App_in_All_Funds(string.Empty, string.Empty);
                Rb_Fund_Edit.Visible = Rb_Status_Edit.Visible = Rb_Hist_All.Visible = label11.Visible = false;

                Btn_Bus.Visible = Cb_Show_Entire_Status.Visible = false;
                ActHist_Site.Visible = ActHist_Attn_SDate.Visible = ActHist_Attn_LDate.Visible = false;
                ActHist_Fund.HeaderText = "Hierarchy";
                ActHist_Fund.Width = 250;
            }

            Get_History();
        }


        public CASE0010_StatusChange_Form(BaseForm baseForm, string appNo, string module_code, string enrl_ID)
        {
            InitializeComponent();

            this.Text = "Enroll History";


            label8.Text = appNo;
            //this.Size = new System.Drawing.Size(460, 336);
            //this.Fld_History_Panel.Location = new System.Drawing.Point(-1, -1);
            //Fld_History_Panel.Visible = true;

            /***********************************************************************************************************************************/
            Fld_History_Panel.Visible = true;
            this.Size = new Size(this.Width, this.Height - (IncDoc_Inner_Panel.Height + History_Panel.Height + Status_Panel.Height));
            /***********************************************************************************************************************************/


            Module_Code = module_code;
            Pass_Enroll_List = new List<CaseEnrlEntity>();
            Enrl_ID = enrl_ID;

            BaseForm = baseForm;
            //Pass_Enroll_Entity = pass_enroll_entity;
            _model = new CaptainModel();

            //Get_History();
            Get_Field_History(enrl_ID);
        }

        public CASE0010_StatusChange_Form(string[] Inc_doc_values)
        {
            InitializeComponent();
            Inc_Doc_Values = Inc_doc_values;
            this.FormBorderStyle = FormBorderStyle.None;
            //this.Size = new System.Drawing.Size(193, 180);
            //IncDoc_Inner_Panel.Location = new System.Drawing.Point(2, 2);
            //IncDoc_Inner_Panel.Visible = true;

            /***********************************************************************************************************************************/
            IncDoc_Inner_Panel.Visible = true;
            this.Size = new Size(this.Width - panel16.Width, this.Height - (Fld_History_Panel.Height + History_Panel.Height + Status_Panel.Height));
            /***********************************************************************************************************************************/

            if (Inc_doc_values[0] == "Y")
                Cb_Assests.Checked = true;

            if (Inc_doc_values[1] == "Y")
                Cb_Social.Checked = true;

            if (Inc_doc_values[2] == "Y")
                Cb_Rent.Checked = true;

            if (Inc_doc_values[3] == "Y")
                Cb_Utility.Checked = true;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Sel_Hie { get; set; }

        public string Sel_Prog_Name { get; set; }

        public string Module_Code { get; set; }

        public string Class_Start_Date { get; set; }

        public string Class_End_Date { get; set; }

        public string Enrl_ID { get; set; }

        public string[] Inc_Doc_Values { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<CaseEnrlEntity> Pass_Enroll_List { get; set; }

        public CaseEnrlEntity Pass_Enroll_Entity { get; set; }

        #endregion

        bool EnableWithDrawn = false;
        //List<SPCommonEntity> ReasonDesc_List = new List<SPCommonEntity>();
        List<Agy_Ext_Entity> ReasonDesc_List = new List<Agy_Ext_Entity>();
        private void Fill_Combos()
        {
            // Filling Status Combo
            Cmb_Status.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));

            if (Pass_Enroll_List.Count == 1)
            {
                if (Pass_Enroll_List[0].Status == "W")
                    listItem.Add(new ListItem("Accepted", "C"));

                if (Pass_Enroll_List[0].Status == "L")
                    listItem.Add(new ListItem("Accepted", "C"));

                if (Pass_Enroll_List[0].Status != "R")
                    listItem.Add(new ListItem("Denied", "R"));
                if (Pass_Enroll_List[0].Status != "E")
                    listItem.Add(new ListItem("Enrolled", "E"));


                if (Pass_Enroll_List[0].Status != "X")
                    listItem.Add(new ListItem("Exited", "X"));

                if (Pass_Enroll_List[0].Status != "N")
                    listItem.Add(new ListItem("Inactive", "N"));
                if (Pass_Enroll_List[0].Status == "L")
                {
                    listItem.Add(new ListItem("Parent declined", "A"));
                    listItem.Add(new ListItem("No Longer Interested", "B"));

                }
                if (Pass_Enroll_List[0].Status != "P")
                    listItem.Add(new ListItem("Pending", "P"));

                // if (Pass_Enroll_List[0].Status != "L" && Pass_Enroll_List[0].Status != "E")
                if (Pass_Enroll_List[0].Status != "L") // murali modified 05/sep/2019
                    listItem.Add(new ListItem("Wait List", "L"));
                if (Pass_Enroll_List[0].Status != "W")
                    listItem.Add(new ListItem("Withdrawn", "W"));


            }
            else
            {
                listItem.Add(new ListItem("Denied", "R"));
                listItem.Add(new ListItem("Enrolled", "E"));
                listItem.Add(new ListItem("Exited", "X")); // Murali added on 05/06/2022 OCO_1 document brain is asking
                listItem.Add(new ListItem("Inactive", "N"));
                listItem.Add(new ListItem("Pending", "P"));
                listItem.Add(new ListItem("Wait List", "L"));
                listItem.Add(new ListItem("Withdrawn", "W"));
            }
            Cmb_Status.Items.AddRange(listItem.ToArray());
            Cmb_Status.SelectedIndex = 0;

            // Filling Reason Combo
            Cmb_Reason.Items.Clear();
            //ReasonDesc_List = _model.SPAdminData.Get_AgyRecs(Consts.AgyTab.Enroll_Reasons);
            if (Module_Code == "02")
            {
                ReasonDesc_List = _model.SPAdminData.Get_AgyRecs_With_Ext(Consts.AgyTab.HSREASONWITHDRWL, "1", "2", null, null);
                if (ReasonDesc_List.Count > 0)
                    ReasonDesc_List = ReasonDesc_List.FindAll(u => u.Active.Equals("Y"));
            }
            else
            {
                ReasonDesc_List = _model.SPAdminData.Get_AgyRecs_With_Ext(Consts.AgyTab.Enroll_Reasons, "1", "2", null, null);
            }

            listItem = new List<ListItem>();

            //foreach (SPCommonEntity Entity in ReasonDesc_List)
            //    listItem.Add(new ListItem(Entity.Desc, Entity.Code));

            foreach (Agy_Ext_Entity Entity in ReasonDesc_List)
                listItem.Add(new ListItem(Entity.Desc, Entity.Code));

            Cmb_Reason.Items.AddRange(listItem.ToArray());

            this.Column0.Visible= false;
            if (Module_Code == "02")
            {
                this.GD_Program.HeaderText = "Site / Room / AMPM";
                this.GD_Status_Date.Visible = this.GD_Fund.Visible = this.GD_Attn_FDate.Visible = this.GD_Attn_Date.Visible = true;
                this.GD_Name.Width = 140;
                this.GD_Program.Width = 112;

                this.Column0.Visible = true;
            }

           

            string TmpName = " ", Attn_Date = " ", Attn_Min_Date = " ", Status_Date = " "; Enable_Site_Panel = false;
            bool From_Dept_WaitList = false; int Tmp_Rows_Cnt = 0;
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

                string Status=string.Empty;
                if (Module_Code == "03")
                    Grid_Applications.Rows.Add(true, Entity.Agy + "-" + Entity.Dept + "-" + Entity.Prog, " ", Entity.App, TmpName, (!string.IsNullOrEmpty(Entity.Status_Date.Trim()) ? LookupDataAccess.Getdate(Entity.Status_Date) : " "),"", " ", " ", " ", Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App, Entity.ID);
                else
                { //Try to Lo
                    switch (Entity.Status.Trim())
                    {
                        case "E": Status = "Enrolled"; break;
                        case "R": Status = "Denied"; break;
                        case "X": Status = "Exited"; break;
                        case "N": Status = "Inactive"; break;
                        case "P": Status = "Pending"; break;
                        case "L": Status = "Wait List"; break;
                        case "W": Status = "Withdrawn"; break;
                        case "A": Status = "Parent declined"; break;
                        case "B": Status = "No Longer Interested"; break;
                        case "C": Status = "Accepted"; break;
                    }
                    From_Dept_WaitList = (!string.IsNullOrEmpty(Entity.Site.Trim()) ? false : true);
                    Grid_Applications.Rows.Add(true, (From_Dept_WaitList ? Entity.Mst_Site : Entity.Site) + " / " + (From_Dept_WaitList ? "****" : Entity.Room) + "  /  " + (From_Dept_WaitList ? "*" : Entity.AMPM), Entity.FundHie, Entity.App, TmpName, Status_Date,Status, Attn_Min_Date, Attn_Date, " ", Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App, Entity.ID);
                    if (Entity.Status == "W") EnableWithDrawn = true; else EnableWithDrawn = false;

                   

                }

                if ((string.IsNullOrEmpty(Entity.Site.Trim()) && string.IsNullOrEmpty(Entity.Room.Trim()) && string.IsNullOrEmpty(Entity.AMPM.Trim())) || Entity.Status == "L")
                    Enable_Site_Panel = true;

                Tmp_Rows_Cnt++;
            }

            if (Module_Code == "02")
            {
                if (Enable_Site_Panel)
                {
                    Site_Panel.Visible = true;
                    Cb_Other_Room.Visible = false;
                }
                else
                {
                    Site_Panel.Visible = false;
                    //Cb_Other_Room.Visible = true;
                }
            }

            if (Tmp_Rows_Cnt > 8)
                this.GD_Name.Width = 145;
        }

        private List<CaseEnrlEntity> Ge_Related_Module_0n_App_ID(string ID, string strYear)
        {
            List<CaseEnrlEntity> Mult_Fund_List = new List<CaseEnrlEntity>();
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            if (strYear != string.Empty)
            {
                Search_Entity.Year = strYear;
            }
            else
                Search_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
            if (!string.IsNullOrEmpty(ID.Trim()))
                Search_Entity.ID = ID;
            else
                Search_Entity.App = Pass_Enroll_Entity.App;//BaseForm.BaseApplicationNo;

            switch (Module_Code)
            {
                case "02": Search_Entity.Rec_Type = "H"; break;
                case "03": Search_Entity.Rec_Type = "C"; break;
                default: Search_Entity.Rec_Type = "U"; break;
            }
            //Search_Entity.Rec_Type = (Module_Code == "03" ? "C" : "H");
            Search_Entity.Enrl_Status_Not_Equalto = "T";
            Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");

            return Mult_Fund_List;
        }

        bool Enable_Site_Panel = true;
        string Sql_SP_Result_Message = string.Empty;
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (Validate_Status_Change_Controls())
            {
                if (Cb_Withdraw_Enroll.Checked)
                {
                    if (!Validate_Fund_For_Add_Apps())
                    {
                        AlertBox.Show(Error_MSG, MessageBoxIcon.Warning);
                        return;
                    }
                }

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

                DateTime Tmp_date = Enroll_Date.Value;
                TimeSpan Tmp_time = DateTime.Today.TimeOfDay;
                Update_Entity.Status_Date = (Tmp_date + Tmp_time).ToString();

                Update_Entity.Status = ((ListItem)Cmb_Status.SelectedItem).Value.ToString();

                Update_Entity.Seq = "1";
                Update_Entity.Rec_Type = "C";
                Update_Entity.Lstc_Oper = BaseForm.UserID;
                Update_Entity.Site = Update_Entity.Room = Update_Entity.Group = Update_Entity.AMPM = Update_Entity.Enrl_Date = string.Empty;

                Update_Entity.Desc_1 = Txt_Desc1.Text.Trim();
                Update_Entity.Desc_2 = Txt_Desc2.Text.Trim();

                Update_Entity.Status = ((ListItem)Cmb_Status.SelectedItem).Value.ToString();

                switch (((ListItem)Cmb_Status.SelectedItem).Value.ToString())
                {
                    case "R":
                    case "P":
                    case "W":
                        Update_Entity.Withdraw_Code = Update_Entity.Status_Reason = ((ListItem)Cmb_Reason.SelectedItem).Value.ToString(); ;
                        break;
                }


                string Tmp_Site = string.Empty, Tmp_Room = string.Empty, Tmp_AMPM = string.Empty; bool Replace_Site = false;

                if (!string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
                    Tmp_Site = Txt_Trans_Site.Text.Trim();

                if (!string.IsNullOrEmpty(Txt_Trans_Room.Text.Trim()))
                    Tmp_Room = Txt_Trans_Room.Text.Trim();

                if (!string.IsNullOrEmpty(Txt_Trans_AMPM.Text.Trim()))
                    Tmp_AMPM = (Txt_Trans_AMPM.Text.Trim()).Substring(0, 1);

                int Sle_Rec_Count = 0; string Tmp_Reason_Code = string.Empty, Tmp_Prog_Hie = string.Empty;
                foreach (DataGridViewRow dr in Grid_Applications.Rows)
                {
                    int NooF_Rows_Processed = 0;

                    if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                    {
                        Sle_Rec_Count++;
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.App ==
                                dr.Cells["GD_Key"].Value.ToString())
                            {

                                if (!string.IsNullOrEmpty(dr.Cells["GD_Date"].Value.ToString().Trim()))
                                {
                                    DateTime date = Convert.ToDateTime(dr.Cells["GD_Date"].Value);
                                    //TimeSpan time = DateTime.Today.TimeOfDay;
                                    Update_Entity.Status_Date = date.ToShortDateString();

                                    switch (((ListItem)Cmb_Status.SelectedItem).Value.ToString())
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
                                        Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Status = \"" + Entity.Status + "\"  From_Date = \"" + LookupDataAccess.Getdate(Entity.Status_Date) + "\" SITE = \"" + (From_Dept_WaitList ? Entity.Mst_Site : Entity.Site) + "\" ROOM = \"" + (From_Dept_WaitList ? "****" : Entity.Room) + "\" AMPM = \"" + (From_Dept_WaitList ? "*" : Entity.AMPM) + "\" To_Date = \"" + LookupDataAccess.Getdate(Update_Entity.Status_Date) + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");
                                    else
                                    {
                                        Tmp_Prog_Hie = Entity.FundHie.Trim();
                                        Tmp_Prog_Hie = Tmp_Prog_Hie + "      ".Substring(0, 6 - Tmp_Prog_Hie.Length);
                                        Xml_HistTo_Pass.Append("<Row ID = \"" + Entity.ID + "\" Status = \"" + Entity.Status + "\"  From_Date = \"" + LookupDataAccess.Getdate(Entity.Status_Date) + "\" SITE = \"" + Tmp_Prog_Hie.Substring(0, 2) + "\" ROOM = \"" + Tmp_Prog_Hie.Substring(2, 2) + "\" AMPM = \"" + Tmp_Prog_Hie.Substring(4, 2) + "\" To_Date = \"" + LookupDataAccess.Getdate(Update_Entity.Status_Date) + "\" REASON = \"" + Tmp_Reason_Code + "\"/>");
                                    }

                                    Replace_Site = false;
                                    if (((string.IsNullOrEmpty(Entity.Site.Trim()) && string.IsNullOrEmpty(Entity.Room.Trim()) && string.IsNullOrEmpty(Entity.AMPM.Trim())) || ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "L") && Module_Code == "02")
                                    {
                                        Replace_Site = true;
                                        Entity.Site = Tmp_Site;
                                        Entity.Room = Tmp_Room;
                                        Entity.AMPM = Tmp_AMPM;
                                        //if (((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "L")
                                        //{
                                        //    if (string.IsNullOrEmpty(Entity.Room.Trim()))
                                        //        Entity.Room = "****";
                                        //    if (string.IsNullOrEmpty(Entity.AMPM.Trim()))
                                        //        Entity.AMPM = "*";
                                        //}
                                    }

                                    if (Cb_Withdraw_Enroll.Checked)
                                        Update_Entity.Enrl_Date = Entity.Enrl_Date;

                                    // insert pirwithdraw table
                                    if (((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "W")
                                    {
                                        if (Entity.Rec_Type == "H")
                                        {
                                            PirWorkEntity pirworkEntity = new Model.Objects.PirWorkEntity();
                                            if (Pass_Enroll_List.Count > 0)
                                            {
                                                pirworkEntity.PIRWORK_AGENCY = Entity.Agy;
                                                pirworkEntity.PIRWORK_DEPT = Entity.Dept;
                                                pirworkEntity.PIRWORK_PROG = Entity.Prog;
                                                pirworkEntity.PIRWORK_YEAR = Entity.Year;
                                                pirworkEntity.PIRWORK_APP_NO = Entity.App;
                                                pirworkEntity.PIRWORK_FUND = Entity.FundHie;
                                                pirworkEntity.PIRWORK_WDRAW_DATE = Entity.Withdraw_Date;
                                                pirworkEntity.PIRWORK_FAMILY_ID = Entity.MST_FAMILY_ID; ;
                                                pirworkEntity.PIRWORK_INCOME_TYPES = Entity.MST_INCOME_TYPE;
                                                pirworkEntity.PIRWORK_POVERTY = Entity.MST_POVERTY;
                                                pirworkEntity.PIRWORK_FAMILY_TYPE = Entity.MST_FAMILY_TYPE;
                                                pirworkEntity.PIRWORK_LANGUAGE = Entity.MST_Language;
                                            }

                                            pirworkEntity.Mode = string.Empty;

                                            _model.EnrollData.INSERTUPDATEPIRWITHDRAW(pirworkEntity);
                                        }

                                    }

                                    //Update_Entity.Status_Date
                                    Xml_To_Pass.Append("<Row Enrl_APP_NO = \"" + Entity.App + "\" Enrl_GROUP = \"" + Entity.Agy + "\" Enrl_FUND_HIE = \"" + Entity.FundHie + "\" Enrl_SEQ = \"" + "1" + "\" Enrl_ID = \"" + Entity.ID + "\" Enrl_SITE = \"" + Entity.Site + "\" Enrl_ROOM = \"" + Entity.Room + "\" Enrl_AMPM = \"" + Entity.AMPM +
                                        //"\" ENRL_ENRLD_DATE = \"" + (Update_Entity.Status != "E" ? Entity.Enrl_Date : (string.IsNullOrEmpty(Entity.Enrl_Date.Trim()) ? Update_Entity.Status_Date : Entity.Enrl_Date)) + "\" Enrl_WDRAW_CODE = \"" + Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + (Update_Entity.Status != "W" ? Entity.Withdraw_Date : Update_Entity.Status_Date) + "\" Enrl_WLIST_DATE = \"" + Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Entity.Denied_Date +
                                        "\" ENRL_ENRLD_DATE = \"" + LookupDataAccess.Getdate(Entity.Enrl_Date) + "\" Enrl_WDRAW_CODE = \"" + Entity.Withdraw_Code + "\" Enrl_WDRAW_DATE = \"" + (Update_Entity.Status != "W" ? Entity.Withdraw_Date : Update_Entity.Status_Date) + "\" Enrl_WLIST_DATE = \"" + Entity.Wiait_Date + "\" Enrl_DENIED_CODE = \"" + Entity.Denied_Code + "\" Enrl_DENIED_DATE = \"" + Entity.Denied_Date +
                                               "\" Enrl_PENDING_CODE= \"" + Entity.Pending_Code + "\" Enrl_PENDING_DATE= \"" + Entity.Pending_Date + "\" Enrl_RANK= \"" + Entity.Rank + "\"  Enrl_RNKCHNG_CODE= \"" + Entity.Rank_Chg_Code + "\" Enrl_TRAN_TYPE= \"" + Entity.Transc_Type +
                                               "\" Enrl_TRANSFER_SITE= \"" + Entity.Tranfr_Site + "\" Enrl_TRANSFER_ROOM= \"" + Entity.Tranfr_Room + "\" Enrl_TRANSFER_AMPM= \"" + Entity.Tranfr_AMPM + "\" Enrl_PARENT_RATE= \"" + (!string.IsNullOrEmpty(Entity.Parent_Rate.Trim()) ? Entity.Parent_Rate : "0") + "\"  Enrl_FUNDING_CODE= \"" + Entity.Funding_Code + "\" Enrl_FUNDING_RATE= \"" + (!string.IsNullOrEmpty(Entity.Funding_Rate.Trim()) ? Entity.Funding_Rate : "0") +
                                               "\" Enrl_FUND_END_DATE= \"" + Entity.Fund_End_date + "\" Enrl_RATE_EFF_DATE= \"" + Entity.Rate_EFR_date + "\" Enrl_Enroll_DATE= \"" + LookupDataAccess.Getdate(Update_Entity.Status_Date) + "\" Enrl_Curr_Status= \"" + Update_Entity.Status + "\" Sum_Key_To_Update= \"" + " " + "\" Enrl_Preferred_Class= \"" + (Cb_Other_Room.Checked ? "Y" : "N") +
                                               "\" Enrl_Withdraw_Enrl_Sw= \"" + (Cb_Withdraw_Enroll.Checked ? "Y" : "N") + "\" Enrl_Withdraw_Enrl_Site= \"" + Txt_DrawEnroll_Site.Text + "\" Enrl_Withdraw_Enrl_Room= \"" + Txt_DrawEnroll_Room.Text + "\" Enrl_Withdraw_Enrl_AMPM= \"" + Txt_DrawEnroll_AMPM.Text + "\" Enrl_Withdraw_Enrl_Fund= \"" + ((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString() + "\"/>");
                                }
                                else
                                {
                                    Save_Result = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                Xml_To_Pass.Append("</Rows>");
                Xml_HistTo_Pass.Append("</Rows>");

                if (Sle_Rec_Count > 0)
                {
                    if (Save_Result)
                    {


                        if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Xml_To_Pass.ToString(), Xml_HistTo_Pass.ToString(), null, out Sql_SP_Result_Message))
                        {

                            AlertBox.Show("Saved Successfully");
                            Save_Result = false;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                            AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        AlertBox.Show("Please Provide Date", MessageBoxIcon.Warning);
                        if (Privileges.ModuleCode == "02")
                        {
                            Btn_Calu_Dates.Visible = true;
                            Btn_Save.Visible = false;
                        }
                    }
                }
                else
                    AlertBox.Show("Please Select atleast One Record", MessageBoxIcon.Warning);
            }
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
            if (Pass_Enroll_List.Count > 0)
                Search_Entity.App = Pass_Enroll_List[0].App;
            Search_Entity.FundHie = ((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString();
            Search_Entity.Enrl_Status_Not_Equalto = "T";
            Search_Entity.Rec_Type = "H";

            List<CaseEnrlEntity> Enroll_List = new List<CaseEnrlEntity>();
            Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");

            Error_MSG = "";
            foreach (CaseEnrlEntity Ent in Pass_Enroll_List)
            {
                foreach (CaseEnrlEntity ent in Enroll_List)
                {
                    if (Ent.App == ent.App &&
                        Ent.Site == Txt_DrawEnroll_Site.Text.Trim() &&
                        Ent.Room == Txt_DrawEnroll_Room.Text.Trim() &&
                        Ent.AMPM == Txt_DrawEnroll_AMPM.Text.Trim())
                    {
                        Error_MSG += "App# " + ent.App + " Already Exists with this Fund in " + (Ent.Site + "/" + Ent.Room + "/" + Ent.AMPM) + "\n";
                        Can_Add = false;
                        break;
                    }
                }
            }

            return Can_Add;
        }

        bool Status_Dates_Validated = false;
        private void Set_Status_Dates_Confirmation(object sender, EventArgs e)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            //if (messageBoxWindow.DialogResult == DialogResult.Yes)
            //{
            //    Status_Dates_Validated = true;
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }


        private bool Validate_Status_Change_Controls()
        {
            bool Can_Save = true;

            if (!Enroll_Date.Checked)
            {
                _errorProvider.SetError(Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Enroll Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Enroll_Date, null);

            string Tmp_Status = ((ListItem)Cmb_Status.SelectedItem).Value.ToString();
            if (Tmp_Status == "0")
            {
                _errorProvider.SetError(Cmb_Status, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                Can_Save = false;
            }
            else
                _errorProvider.SetError(Cmb_Status, null);

            //if (Tmp_Status == "E" && Enroll_Date.Checked && Module_Code == "02")
            //{
            //    if (HSS_Site_List.Count == 0)
            //        Get_HSS_Sites();

            //    //string Disp_Class_ST_Date = LookupDataAccess.Getdate(Class_Start_Date);
            //    //string Disp_Class_END_Date = LookupDataAccess.Getdate(Class_End_Date);

            //    string Disp_Class_ST_Date = string.Empty, Disp_Class_END_Date = string.Empty;
            //    DateTime Start_Date = DateTime.Today;
            //    DateTime End_Date = DateTime.Today;
            //    DateTime Compare_Date;

            //    bool Validate_Status_Date = true, Replace_Site = false; ;
            //    string Error_Msg = string.Empty, Tmp_Site = " ", Tmp_Room = " ", Tmp_AMPM = " ";
            //    string Tmp_NewSite = " ", Tmp_NewRoom = " ", Tmp_NewAMPM = " ";

            //    if (!string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
            //        Tmp_NewSite = Txt_Trans_Site.Text.Trim();

            //    if (!string.IsNullOrEmpty(Txt_Trans_Room.Text.Trim()))
            //        Tmp_NewRoom = Txt_Trans_Room.Text.Trim();

            //    if (!string.IsNullOrEmpty(Txt_Trans_AMPM.Text.Trim()))
            //        Tmp_NewAMPM = (Txt_Trans_AMPM.Text.Trim()).Substring(0, 1);

            //    foreach (DataGridViewRow dr in Grid_Applications.Rows)
            //    {
            //        if (!string.IsNullOrEmpty(dr.Cells["GD_Date"].Value.ToString().Trim()))
            //        {
            //            Compare_Date = Convert.ToDateTime(dr.Cells["GD_Date"].Value.ToString());

            //            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //            {
            //                Replace_Site = false;
            //                if (Entity.ID == dr.Cells["GD_ID"].Value.ToString().Trim())
            //                {
            //                    Tmp_Site = Entity.Site; Tmp_Room = Entity.Room; Tmp_AMPM = Entity.AMPM;
            //                    if (string.IsNullOrEmpty(Entity.Site.Trim()) && string.IsNullOrEmpty(Entity.Room.Trim()) && string.IsNullOrEmpty(Entity.AMPM.Trim()))
            //                        Replace_Site = true;

            //                    break;
            //                }
            //            }


            //            foreach (CaseSiteEntity Ent in HSS_Site_List)
            //            {
            //                if (Ent.SiteNUMBER == (Replace_Site ? Tmp_NewSite : Tmp_Site) && Ent.SiteROOM.Trim() == (Replace_Site ? Tmp_NewRoom : Tmp_Room) && Ent.SiteAM_PM.Trim() == (Replace_Site ? Tmp_NewAMPM : Tmp_AMPM))
            //                {
            //                    Disp_Class_ST_Date = LookupDataAccess.Getdate(Ent.SiteCLASS_START);
            //                    Disp_Class_END_Date = LookupDataAccess.Getdate(Ent.SiteCLASS_END);

            //                    Start_Date = Convert.ToDateTime(Ent.SiteCLASS_START);
            //                    End_Date = Convert.ToDateTime(Ent.SiteCLASS_END);
            //                    break;
            //                }
            //            }


            //            if (Start_Date > Compare_Date || End_Date < Compare_Date)
            //            {
            //                Validate_Status_Date = Can_Save = false;
            //                Error_Msg += "\n" + dr.Cells["GD_App"].Value.ToString() + " Enroll Date Should be Between " + Disp_Class_ST_Date + " And " + Disp_Class_END_Date;
            //            }
            //        }
            //    }

            //    if (Validate_Status_Date)
            //        _errorProvider.SetError(Enroll_Date, null);
            //    else
            //        _errorProvider.SetError(Enroll_Date, "Enroll Date(S) for the App# : " + Error_Msg.Replace(Consts.Common.Colon, string.Empty));
            //    //_errorProvider.SetError(Enroll_Date, "Enroll Date(S) for the App# : " + Error_Msg + "\n Should be Within the Class Date Range \n" + Disp_Class_ST_Date + " To: " + Disp_Class_END_Date.Replace(Consts.Common.Colon, string.Empty));
            //}

            if (Tmp_Status == "P" || Tmp_Status == "R" || Tmp_Status == "W")
            {
                if (((ListItem)Cmb_Reason.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(Cmb_Reason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(Cmb_Reason, null);
            }

            if (Module_Code == "02")
            {
                if (Enable_Site_Panel)
                {
                    if (string.IsNullOrEmpty(Txt_Trans_Site.Text.Trim()))
                    {
                        _errorProvider.SetError(Site_Panel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label26.Text.Replace(Consts.Common.Colon, string.Empty)));
                        Can_Save = false;
                    }
                    else
                        _errorProvider.SetError(Site_Panel, null);

                    if (Tmp_Status != "A" && Tmp_Status != "B" && Tmp_Status != "L")
                    {
                        if (string.IsNullOrEmpty(Txt_Trans_Room.Text.Trim()) || Txt_Trans_Room.Text.Trim() == "****")
                        {
                            _errorProvider.SetError(Site_Panel, "Room can't be ****".Replace(Consts.Common.Colon, string.Empty));//string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Room can't be ****" ));
                            Can_Save = false;
                        }
                        else
                            _errorProvider.SetError(Site_Panel, null);
                    }

                }
                else
                {
                    if (Cb_Withdraw_Enroll.Checked)
                    {
                        if (string.IsNullOrEmpty(Txt_DrawEnroll_Site.Text.Trim()) || string.IsNullOrEmpty(Txt_DrawEnroll_Room.Text.Trim()) || string.IsNullOrEmpty(Txt_DrawEnroll_AMPM.Text.Trim()))
                        {
                            _errorProvider.SetError(Pb_Withdraw_Enroll, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label12.Text.Replace(Consts.Common.Colon, string.Empty)));
                            Can_Save = false;
                        }
                        else
                        {
                            bool App_Exists_in_Sel_Class = false;
                            string Error_MSG = string.Empty;
                            //if (Pass_Enroll_List[0].Status == "E")
                            //{
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
                                if (Entity.Site == Txt_DrawEnroll_Site.Text.Trim() &&
                                   Entity.Room == Txt_DrawEnroll_Room.Text.Trim() &&
                                   Entity.AMPM == (Txt_DrawEnroll_AMPM.Text.Substring(0, 1)).Trim() &&
                                   Entity.FundHie == ((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString())
                                {
                                    Error_MSG = "App# Already Exists in Selected Class (" + Entity.Site + "/" + Entity.Room + "/" + Entity.AMPM + ") - Fund (" + ((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString() + ")";
                                    App_Exists_in_Sel_Class = true; break;
                                }
                            }
                            //}
                            if (App_Exists_in_Sel_Class)
                            {
                                _errorProvider.SetError(Pb_Withdraw_Enroll, Error_MSG.Replace(Consts.Common.Colon, string.Empty));
                                Can_Save = false;
                            }
                            else
                                _errorProvider.SetError(Pb_Withdraw_Enroll, null);
                        }

                        if (((ListItem)Cmb_DrawEnroll_Fund.SelectedItem).Value.ToString() == "00")
                        {
                            _errorProvider.SetError(Cmb_DrawEnroll_Fund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label14.Text.Replace(Consts.Common.Colon, string.Empty)));
                            Can_Save = false;
                        }
                        else
                            _errorProvider.SetError(Cmb_DrawEnroll_Fund, null);
                    }
                }


            }


            return Can_Save;
        }


        List<ENRLHIST_Entity> ENRLHIST_List = new List<ENRLHIST_Entity>();
        string CM_Last_Hist_FDate = "";
        private void Get_History()
        {
            History_Grid.Rows.Clear();
            CM_Last_Hist_FDate = "";
            Lbl_Hist_Gap.Visible = false; Btn_Edit_Stat_Hist.Visible = false;
            if (ENRLHIST_List.Count == 0 && !string.IsNullOrEmpty(Pass_Enroll_Entity.ID.Trim()))
            {
                ENRLHIST_Entity Search_Entity = new ENRLHIST_Entity(true);
                Search_Entity.ID = Pass_Enroll_Entity.ID;
                Search_Entity.Asof_Date = "N";
                ENRLHIST_List = _model.EnrollData.Browse_ENRLHIST(Search_Entity, "Browse");
            }

            if (Module_Code == "03")
            {
                this.Hist_Site.Visible = this.Hist_Fund_Hie.Visible = false;
                this.Hist_Status.Width = 70;
                this.Hist_TDate.Width = 80;
                this.Hist_Operator.Width = 150;

                this.Hist_Operator.Visible = this.Hist_Operator.ShowInVisibilityMenu = true;
                this.Hist_Add_Date.Visible = this.Hist_Add_Date.ShowInVisibilityMenu = true;
            }
            else
            {
                Lbl_Rank_Desc.Visible = Lbl_HS_Rank.Visible = Stf_Grid.Visible = this.Hist_Fund_Hie.Visible = Teacher_Panel.Visible = true;
                this.Hist_Operator.Width = 105;

                this.Hist_Operator.Visible = this.Hist_Operator.ShowInVisibilityMenu = false;
                this.Hist_Add_Date.Visible = this.Hist_Add_Date.ShowInVisibilityMenu = false;
            }

            int Tmp_loop_Cnt = 0, Row_Index = 0, Tmp_Sel_ID_Hist_Cnt = 0;
            if (!string.IsNullOrEmpty(Enrl_ID.Trim()) && (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                Btn_Edit_Stat_Hist.Visible = true;

            //if (!string.IsNullOrEmpty(Enrl_ID.Trim()) && (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B") && Module_Code == "03")
            //{
            //    Btn_Edit_Curr_Status_Date.Location = new System.Drawing.Point(495, 49);
            //    Btn_Edit_Curr_Status_Date.Size = new System.Drawing.Size(56, 59);
            //    Btn_Edit_Curr_Status_Date.Visible = true;
            //}


            if (ENRLHIST_List.Count > 0)
            {
                string TmP_Sel_ID = Enrl_ID; string Priv_Stat_Todate = ""; bool Hist_Gap_SW = false; Lbl_Hist_Gap.Visible = false;
                //if (Module_Code == "02")
                if(Act_Status_Grid.Rows.Count>0)
                    TmP_Sel_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString().Trim();
                CM_Last_Hist_FDate = LookupDataAccess.Getdate(ENRLHIST_List[0].From_Date);

                string Status_Desc = " ", From_Date = " ", To_Date = " ", Add_Date = " ";
                foreach (ENRLHIST_Entity Entity in ENRLHIST_List)
                {
                    if (TmP_Sel_ID == Entity.ID || Cb_Show_Entire_Status.Checked)
                    {
                        if (!string.IsNullOrEmpty(Entity.From_Date.Trim()))
                            From_Date = LookupDataAccess.Getdate(Entity.From_Date);
                        if (!string.IsNullOrEmpty(Entity.TO_Date.Trim()))
                            To_Date = LookupDataAccess.Getdate(Entity.TO_Date);
                        if (!string.IsNullOrEmpty(Entity.Date_Add.Trim()))
                            Add_Date = LookupDataAccess.Getdate(Entity.Date_Add);
                        switch (Entity.Status)
                        {
                            case "L": Status_Desc = "Wait List"; break;
                            case "P": Status_Desc = "Pending"; break;
                            case "R": Status_Desc = "Denied"; break;
                            case "E": Status_Desc = "Enrolled"; break;
                            case "W": Status_Desc = "Withdrawn"; break;
                            case "I": Status_Desc = "Post Intake"; break;
                            case "N": Status_Desc = "Inactive"; break;
                            case "A": Status_Desc = "Parent declined"; break;
                            case "B": Status_Desc = "No Longer Interested"; break;
                            case "C": Status_Desc = "Accepted"; break;
                            case "X": Status_Desc = "Exited"; break;

                        }


                        Row_Index = History_Grid.Rows.Add(Status_Desc, From_Date, To_Date, Entity.Site + (!string.IsNullOrEmpty(Entity.Room.Trim()) ? ("/" + Entity.Room) : "") + (!string.IsNullOrEmpty(Entity.AMPM.Trim()) ? ("/" + Entity.AMPM) : ""), Entity.Fund_Hie, Entity.Add_Opr, Add_Date);

                        if (TmP_Sel_ID == Entity.ID)
                        {
                            if (!string.IsNullOrEmpty(Priv_Stat_Todate.Trim()) && Priv_Stat_Todate != To_Date)
                                Hist_Gap_SW = true;

                            Tmp_Sel_ID_Hist_Cnt++;
                            History_Grid.Rows[Row_Index].DefaultCellStyle.ForeColor = System.Drawing.Color.BlueViolet;
                            Priv_Stat_Todate = From_Date;
                            //History_Grid.Rows[Row_Index].DefaultCellStyle.BackColor = System.Drawing.Color.PaleTurquoise; // SlateGray 
                        }


                        Tmp_loop_Cnt++;
                    }

                    if (History_Grid.Rows.Count > 0)
                    {
                        string toolTipText = "Added By      : " + Entity.Add_Opr.Trim() + " on " + Entity.Date_Add + "\n" +
                         "Modified By  : " + Entity.Lstc_Opr.Trim() + " on " + Entity.Lstc_Dtae;

                        foreach (DataGridViewCell cell in History_Grid.Rows[Row_Index].Cells)
                        {
                            cell.ToolTipText = toolTipText;
                        }
                    }
                }

                if (Tmp_loop_Cnt > 0)
                {
                    //if (Module_Code == "02")
                    //if (Tmp_Sel_ID_Hist_Cnt > 0 && !string.IsNullOrEmpty(Enrl_ID.Trim()))
                    //    Btn_Edit_Stat_Hist.Visible = true;
                    //if (!string.IsNullOrEmpty(Enrl_ID.Trim()) && (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                    //    Btn_Edit_Stat_Hist.Visible = true;

                    History_Grid.CurrentCell = History_Grid.Rows[0].Cells[1];

                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@ENRL_ID", TmP_Sel_ID));
                    DataSet Hist_Gaps = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_Any_Gaps_Sel_Curr_Enrl]");

                    if (Hist_Gaps.Tables.Count > 0)
                    {
                        if (Hist_Gaps.Tables[0].Rows.Count > 0)
                            Lbl_Hist_Gap.Visible = true;
                    }

                    //if (Hist_Gap_SW)
                    //    Lbl_Hist_Gap.Visible = true;
                }
            }
        }

        private void Fill_HeadTeacher_Data()
        {
            Lbl_Teacher.Text = "";
            List<CaseSiteEntity> Site_Entity = new List<CaseSiteEntity>();
            List<STAFFMSTEntity> Staff_List = new List<STAFFMSTEntity>();
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = BaseForm.BaseAgency;

            Search_Entity.SiteAGENCY = Search_STAFFMST.Agency = BaseForm.BaseAgency;
            Search_Entity.SiteDEPT = BaseForm.BaseDept;
            Search_Entity.SitePROG = BaseForm.BaseProg;
            Search_Entity.SiteYEAR = BaseForm.BaseYear;

            string[] Site = Regex.Split(Act_Status_Grid.CurrentRow.Cells["ActHist_Site"].Value.ToString().Trim(), "/");


            if (Site.Length >= 1)
            {
                if (!string.IsNullOrEmpty(Site[0].Trim()))
                    Search_Entity.SiteNUMBER = Site[0];
            }

            if (Site.Length >= 2)
            {
                if (!Site[1].Trim().Contains("**"))
                    Search_Entity.SiteROOM = Site[1];
            }

            if (Site.Length == 3)
            {
                if (!Site[2].Trim().Contains("**"))
                    Search_Entity.SiteAM_PM = Site[2];
            }

            Site_Entity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
            if (Site_Entity.Count > 0)
            {
                Search_STAFFMST.Staff_Code = Site_Entity[0].SiteHEDTEACHER;
                Staff_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

                if (Staff_List.Count > 0)
                    Lbl_Teacher.Text = Staff_List[0].First_Name + "  " + Staff_List[0].Last_Name;
            }

            Fill_MST_StaffPositions();
        }

        private void Fill_HS_Ranks(List<CaseMstEntity> caseMstList)
        {
            int Rank_Total = 0, Rank_Cnt = 0;
            string Rank_String = "", Agey_2_Compare = BaseForm.BaseAgency;
            List<RankCatgEntity> Ranksgrid = new List<RankCatgEntity>();
            Ranksgrid = _model.SPAdminData.Browse_RankCtg();

        Get_Rank_Totals:
            foreach (RankCatgEntity Ent in Ranksgrid)
            {
                if (Agey_2_Compare == Ent.Agency && Ent.HeadStrt != "N" && !string.IsNullOrEmpty(Ent.HeadStrt.Trim()) && string.IsNullOrEmpty(Ent.SubCode.Trim()))
                {
                    if (Rank_Cnt > 3)
                        break;

                    if (Ent.Code == "01" || Ent.Code == "1")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank1.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank1.Trim()) ? int.Parse(caseMstList[0].Rank1) : 0);
                        Rank_Cnt++;
                    }
                    if (Ent.Code == "02" || Ent.Code == "2")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank2.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank2.Trim()) ? int.Parse(caseMstList[0].Rank2) : 0);
                        Rank_Cnt++;
                    }
                    if (Ent.Code == "03" || Ent.Code == "3")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank3.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank3.Trim()) ? int.Parse(caseMstList[0].Rank3) : 0);
                        Rank_Cnt++;
                    }
                    if (Ent.Code == "04" || Ent.Code == "4")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank4.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank4.Trim()) ? int.Parse(caseMstList[0].Rank4) : 0);
                        Rank_Cnt++;
                    }
                    if (Ent.Code == "05" || Ent.Code == "5")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank5.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank5.Trim()) ? int.Parse(caseMstList[0].Rank5) : 0);
                        Rank_Cnt++;
                    }
                    if (Ent.Code == "06" || Ent.Code == "6")
                    {
                        Rank_String += Ent.Desc + ": " + caseMstList[0].Rank6.Trim() + ", ";
                        Rank_Total += (!string.IsNullOrEmpty(caseMstList[0].Rank6.Trim()) ? int.Parse(caseMstList[0].Rank6) : 0);
                        Rank_Cnt++;
                    }
                }
            }

            if (Rank_Cnt == 0 && Agey_2_Compare != "**")
            {
                Agey_2_Compare = "**";
                goto Get_Rank_Totals;
            }

            Lbl_Rank_Desc.Text = Rank_String + "Total: " + Rank_Total.ToString();
        }

        List<CaseMstEntity> caseMstList = new List<CaseMstEntity>();
        private void Fill_MST_StaffPositions()
        {
            Stf_Grid.Rows.Clear();
            string[,] Stf_Pos = new string[3, 3];
            Stf_Pos[0, 0] = Stf_Pos[0, 1] = Stf_Pos[0, 2] =
            Stf_Pos[1, 0] = Stf_Pos[1, 1] = Stf_Pos[1, 2] =
            Stf_Pos[2, 0] = Stf_Pos[2, 1] = Stf_Pos[2, 2] = "";
            List<CommonEntity> Positions_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.POSITIONCODS, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, "Edit"); // _model.lookupDataAccess.GetGender();

            string Stf_Code_To_Compare = "", Stf_Position = "", Pos_Code = "";
            int POS = 0;
            if (!string.IsNullOrEmpty(caseMstList[0].Position1) ||
                !string.IsNullOrEmpty(caseMstList[0].Position2) ||
                !string.IsNullOrEmpty(caseMstList[0].Position3))
            {
                List<STAFFMSTEntity> Staff_List = new List<STAFFMSTEntity>();
                STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
                Search_STAFFMST.Agency = BaseForm.BaseAgency;
                Staff_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        case 0: Stf_Code_To_Compare = caseMstList[0].Position1; break;
                        case 1: Stf_Code_To_Compare = caseMstList[0].Position2; break;
                        case 2: Stf_Code_To_Compare = caseMstList[0].Position3; break;
                    }

                    Stf_Position = "";
                    if (!string.IsNullOrEmpty(Stf_Code_To_Compare))
                    {
                        foreach (STAFFMSTEntity Ent in Staff_List)
                        {
                            if (Ent.Staff_Code == Stf_Code_To_Compare)
                            {
                                POS = 0; Stf_Position = "";
                                POS = (Ent.Position_Data).IndexOf('P');
                                if (POS >= 2)
                                    Stf_Position = Ent.Position_Data.Substring(POS - 2, 2);

                                if (!string.IsNullOrEmpty(Stf_Position))
                                {
                                    foreach (CommonEntity Lst in Positions_List)
                                    {
                                        if (Lst.Code == Stf_Position)
                                        {
                                            Stf_Position = Lst.Desc; break;
                                        }
                                    }
                                }
                                Stf_Grid.Rows.Add(Ent.First_Name + "  " + Ent.Last_Name, Stf_Position);
                                break;
                            }
                        }
                    }
                }

            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void Enroll_Date_LostFocus(object sender, EventArgs e)
        {
            //if (Enroll_Date.Checked)
            //{
            //    Enroll_Date_ValueChanged(sender, e);
            //    string Tmp_Date = string.Empty, Tmp_Attn_Date = string.Empty;
            //    foreach (DataGridViewRow dr in Grid_Applications.Rows)
            //    {
            //        if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
            //        {
            //            foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
            //            {
            //                if (Entity.ID == dr.Cells["GD_ID"].Value.ToString())
            //                {
            //                    Tmp_Date = Entity.Status_Date.Trim();
            //                    if (string.IsNullOrEmpty(Tmp_Date))
            //                        Tmp_Date = "01/01/1900";

            //                    Tmp_Attn_Date = dr.Cells["GD_Attn_Date"].Value.ToString();
            //                    if (string.IsNullOrEmpty(Tmp_Attn_Date.Trim()))
            //                        Tmp_Attn_Date = "01/01/1900";

            //                    if (Convert.ToDateTime(Convert.ToDateTime(Tmp_Date).ToShortDateString()) > Convert.ToDateTime(Enroll_Date.Value.ToShortDateString()))
            //                    {
            //                        if (Convert.ToDateTime(Convert.ToDateTime(Tmp_Date).ToShortDateString()) > Convert.ToDateTime(Tmp_Attn_Date))
            //                            dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Date).ToShortDateString();
            //                        else
            //                            dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Attn_Date);
            //                        //dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Date).ToShortDateString();
            //                    }
            //                    else
            //                    {
            //                        if (Convert.ToDateTime(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString()) > Convert.ToDateTime(Tmp_Attn_Date))
            //                            dr.Cells["GD_Date"].Value = Convert.ToDateTime(Enroll_Date.Value).ToShortDateString();
            //                        else
            //                            dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Attn_Date);                                 
            //                       // dr.Cells["GD_Date"].Value = Enroll_Date.Value.ToShortDateString();
            //                    }
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void Cmb_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Cmb_Status.Items.Count > 0)
                {
                    string strcmbstatus = ((ListItem)Cmb_Status.SelectedItem).Value == null ? string.Empty : ((ListItem)Cmb_Status.SelectedItem).Value.ToString();

                    if (!string.IsNullOrEmpty(strcmbstatus))
                    {
                        List<ListItem> listItem = new List<ListItem>();
                        string Tmp_Status = ((ListItem)Cmb_Status.SelectedItem).Value.ToString(), Chg_Denied_Code = "J";
                        if (EnableWithDrawn && Tmp_Status != "L") Enable_Site_Panel = false;
                        Cmb_Reason.Items.Clear();
                        listItem.Add(new ListItem("  ", "0"));
                        Withdraw_Site_Panel.Visible = Cb_Withdraw_Enroll.Visible = Cb_Withdraw_Enroll.Checked = false;
                        if (Module_Code == "02")
                        {
                            if (Pass_Enroll_List.Count == 1 && Pass_Enroll_List[0].Status == Tmp_Status)
                                AlertBox.Show("Both Current and New Statu are same \n for the selected Statusrecord", MessageBoxIcon.Warning);

                            foreach (Agy_Ext_Entity Entity in ReasonDesc_List)
                            {
                                listItem.Add(new ListItem(Entity.Desc, Entity.Code));
                            }
                            switch (Tmp_Status)
                            {
                                case "P":
                                case "R":
                                case "W":
                                    Reason_Panel.Visible = Cmb_Reason.Enabled = Lbl_Reason_Req.Visible = true;
                                    if (Pass_Enroll_List.Count == 1 && !Enable_Site_Panel && Tmp_Status == "W")
                                        Cb_Withdraw_Enroll.Visible = true;
                                    break;
                                case "L":
                                    Reason_Panel.Visible = Cmb_Reason.Enabled = Lbl_Reason_Req.Visible = false;
                                    if (Pass_Enroll_List.Count == 1 && Tmp_Status == "L" && !Enable_Site_Panel && EnableWithDrawn)
                                        Enable_Site_Panel = true;
                                    break;
                                default: Reason_Panel.Visible = Cmb_Reason.Enabled = Lbl_Reason_Req.Visible = false; break;
                            }
                        }
                        else
                        {
                            switch (Tmp_Status)
                            {
                                case "P":
                                case "R":
                                case "W":
                                    Reason_Panel.Visible = Cmb_Reason.Enabled = Lbl_Reason_Req.Visible = true;
                                    foreach (Agy_Ext_Entity Entity in ReasonDesc_List)
                                    {
                                        if ((Entity.Ext_1 == Tmp_Status) || (Tmp_Status == "R" && Entity.Ext_1 == Chg_Denied_Code))
                                            listItem.Add(new ListItem(Entity.Desc, Entity.Code));
                                    }

                                    if (Pass_Enroll_List.Count == 1 && !Enable_Site_Panel && Tmp_Status == "W")
                                        Cb_Withdraw_Enroll.Visible = true;
                                    break;

                                default: Reason_Panel.Visible = Cmb_Reason.Enabled = Lbl_Reason_Req.Visible = false; break;
                            }
                        }
                        Cmb_Reason.Items.AddRange(listItem.ToArray());
                        Cmb_Reason.SelectedIndex = 0;

                        if (Module_Code == "02")
                        {
                            if (Enable_Site_Panel)
                            {
                                Site_Panel.Visible = true;
                                Cb_Other_Room.Visible = false;
                            }
                            else
                            {
                                Site_Panel.Visible = false;
                                //Cb_Other_Room.Visible = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Pb_Site_Search_Click(object sender, EventArgs e)
        {
            if (((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "A" || ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "B" || ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "L")
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Site", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
            else
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
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
                Txt_Trans_Site.Text = From_Results[0];
                Txt_Trans_Room.Text = From_Results[1];

                if ((((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "A" || ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "B" || ((ListItem)Cmb_Status.SelectedItem).Value.ToString() == "L") && string.IsNullOrEmpty(From_Results[1].Trim()))
                { Txt_Trans_Room.Text = "****"; Txt_Trans_AMPM.Text = "*"; }

                switch (From_Results[2])
                {
                    case "A": Txt_Trans_AMPM.Text = "A - AM Class"; break;
                    case "P": Txt_Trans_AMPM.Text = "P - PM Class"; break;
                    case "E": Txt_Trans_AMPM.Text = "E - Extended Day"; break;
                    case "F": Txt_Trans_AMPM.Text = "F - Full Day"; break;
                }
            }
        }


        //List<ENRLHIST_Entity> ENRLHIST_List = new List<ENRLHIST_Entity>();
        DataTable Fld_Hist_Data = new DataTable();
        private void Get_Field_History(string ID)
        {
            //Fld_History_Panel.Visible = true;
            Fld_Hist_Grid.Rows.Clear();
            Fld_Hist_Grid_New.Rows.Clear();
            //ENRLHIST_Entity Search_Entity = new ENRLHIST_Entity(true);

            //Search_Entity.ID = Pass_Enroll_Entity.ID;
            //Search_Entity.Asof_Date = "N";
            //DataSet Fld_Hist_Data = _model.EnrollData.Browse_ENRLFLDHIST(Pass_Enroll_Entity.ID, null, null);
            Fld_Hist_Data.Rows.Clear();
            DataSet Fld_Hist_DataSet = _model.EnrollData.Browse_ENRLFLDHIST(ID, null, null);



            int Tmp_loop_Cnt = 0;
            //bool Is_Header_Row = false;



            if (Fld_Hist_DataSet.Tables.Count > 0)
            {
                Fld_Hist_Data = Fld_Hist_DataSet.Tables[0];
                DataTable Fld_Hist_Table_Xml = new DataTable();

                Tmp_loop_Cnt = Fill_History_Grid();

                //string Add_Date = " ", Old_Date = " ", New_Date = " ", Col_Name = " ", Added_By = " ";
                //foreach (DataRow dr in Fld_Hist_Data.Rows)
                //{
                //    if (string.IsNullOrEmpty(dr["EFHIST_SEQ"].ToString().Trim()))
                //    {
                //        //Is_Header_Row = true;
                //        Tmp_loop_Cnt = Fill_FundEdit_History(dr);

                //Fld_Hist_Table_Xml = CommonFunctions.Convert_XMLstring_To_Datatable(dr["EFHIST_XML"].ToString());
                //Add_Date = dr["EFHIST_ADD_DATE"].ToString();
                //if(!string.IsNullOrEmpty(Add_Date.Trim()))
                //    Add_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Add_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //Added_By = dr["EFHIST_ADD_OPERATOR"].ToString();

                //foreach (DataRow dr_Xml in Fld_Hist_Table_Xml.Rows)
                //{

                //    Old_Date = dr_Xml["Old_Value"].ToString();
                //    New_Date = dr_Xml["New_Value"].ToString();
                //    Col_Name = dr_Xml["Col_Name"].ToString();

                //    switch (Col_Name)
                //    {
                //        case "Start Date":
                //        case "Fund End Date":
                //        case "Rate Effective":

                //                if (!string.IsNullOrEmpty(Old_Date.Trim()))
                //                    Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //                if (!string.IsNullOrEmpty(New_Date.Trim()))
                //                    New_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //                break;

                //        case "Fund Category":
                //            switch (Old_Date)
                //            {
                //                case "F": Old_Date = "Full"; break;
                //                case "3": Old_Date = "3/4"; break;
                //                case "2": Old_Date = "1/2"; break;
                //                case "1": Old_Date = "1/4"; break;
                //                default: Old_Date = " "; break;
                //            }
                //            switch (New_Date)
                //            {
                //                case "F": New_Date = "Full"; break;
                //                case "3": New_Date = "3/4"; break;
                //                case "2": New_Date = "1/2"; break;
                //                case "1": New_Date = "1/4"; break;
                //                default: Old_Date = " "; break;
                //            }
                //            break;
                //    }


                //    //Fld_Hist_Grid.Rows.Add(dr["EFHIST_ADD_DATE"].ToString(), "Start Date", dr_Xml["Old_Start_Date"].ToString(), dr_Xml["New_Start_Date"].ToString());
                //    Fld_Hist_Grid.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date);
                //    Fld_Hist_Grid_New.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date, Added_By);
                //    Add_Date = Added_By = " ";


                //    //Old_Date = dr_Xml["Old_Fund_Crit"].ToString();

                //    ////Fld_Hist_Grid.Rows.Add(" ", "Fund Criteria", dr_Xml["Old_Fund_Crit"].ToString(), dr_Xml["New_Fund_Crit"].ToString());
                //    //Fld_Hist_Grid.Rows.Add(" ", "Fund Criteria", Old_Date, New_Date);

                //    //Old_Date = dr_Xml["Old_Fund_End_Date"].ToString();
                //    //if (!string.IsNullOrEmpty(Old_Date.Trim()))
                //    //    Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //    //New_Date = dr_Xml["New_Fund_End_Date"].ToString();
                //    //if (!string.IsNullOrEmpty(New_Date.Trim()))
                //    //    New_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //    ////Fld_Hist_Grid.Rows.Add(" ", "Fund End Date", dr_Xml["Old_Fund_End_Date"].ToString(), dr_Xml["New_Fund_End_Date"].ToString());
                //    //Fld_Hist_Grid.Rows.Add(" ", "Fund End Date", Old_Date, New_Date);

                //    //Old_Date = dr_Xml["Old_Ref_Date"].ToString();
                //    //if (!string.IsNullOrEmpty(Old_Date.Trim()))
                //    //    Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //    //New_Date = dr_Xml["New_Ref_Date"].ToString();
                //    //if (!string.IsNullOrEmpty(New_Date.Trim()))
                //    //    New_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //    ////Fld_Hist_Grid.Rows.Add(" ", "Re-Effective Date", dr_Xml["Old_Ref_Date"].ToString(), dr_Xml["New_Ref_Date"].ToString());
                //    //Fld_Hist_Grid.Rows.Add(" ", "Re-Effective Date", Old_Date, New_Date);
                //    //Fld_Hist_Grid.Rows.Add(" ", "Parent Rate", dr_Xml["Old_Parent_Rate"].ToString(), dr_Xml["New_Parent_Rate"].ToString());
                //    //Fld_Hist_Grid.Rows.Add(" ", "Fund Rate", dr_Xml["Old_Fund_Rate"].ToString(), dr_Xml["New_Fund_Rate"].ToString());

                //    Tmp_loop_Cnt++;
                //}
                //    }
                //}

                if (Tmp_loop_Cnt > 0)
                {
                    // Fld_Hist_Grid.CurrentCell = Fld_Hist_Grid.Rows[0].Cells[1];
                    Fld_Hist_Grid_New.CurrentCell = Fld_Hist_Grid_New.Rows[0].Cells[1];
                }
            }
        }

        private int Fill_History_Grid()
        {
            int Grid_Rows_Cnt = 0;
            Fld_Hist_Grid_New.Rows.Clear();
            foreach (DataRow dr in Fld_Hist_Data.Rows)
            {
                if (Module_Code == "02")
                {
                    if (Rb_Hist_All.Checked)
                        Grid_Rows_Cnt += Fill_FundEdit_History(dr);
                    else
                        if ((string.IsNullOrEmpty(dr["EFHIST_SEQ"].ToString().Trim()) && Rb_Fund_Edit.Checked))
                        Grid_Rows_Cnt += Fill_FundEdit_History(dr);
                    else
                            if ((!string.IsNullOrEmpty(dr["EFHIST_SEQ"].ToString().Trim()) && Rb_Status_Edit.Checked) || Rb_Hist_All.Checked)
                        Grid_Rows_Cnt += Fill_StatusEdit_History(dr);

                    //this.dataGridViewTextBoxColumn5.Visible = this.dataGridViewTextBoxColumn5.ShowInVisibilityMenu = false;
                }
                else
                {
                    if ((!string.IsNullOrEmpty(dr["EFHIST_SEQ"].ToString().Trim())))// && Rb_Status_Edit.Checked))
                        Grid_Rows_Cnt += Fill_StatusEdit_History(dr);

                    //this.dataGridViewTextBoxColumn5.Visible = this.dataGridViewTextBoxColumn5.ShowInVisibilityMenu = true;
                }
            }
            return Grid_Rows_Cnt;
        }

        private int Fill_FundEdit_History(DataRow dr)
        {
            int Rows_Cnt = 0;
            string Add_Date = " ", Old_Date = " ", New_Date = " ", Col_Name = " ", Added_By = " ";
            DataTable Fld_Hist_Table_Xml = new DataTable();
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

                switch (Col_Name)
                {
                    case "From Date":
                    case "To Date":
                    case "Start Date":
                    case "Status Date":
                    case "Fund End Date":
                    case "Rate Effective":

                        if (!string.IsNullOrEmpty(Old_Date.Trim()))
                            Old_Date = LookupDataAccess.Getdate(Old_Date);
                        //Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        if (!string.IsNullOrEmpty(New_Date.Trim()))
                            New_Date = LookupDataAccess.Getdate(New_Date);
                        //New_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);


                        break;

                    case "Fund Category":
                        switch (Old_Date)
                        {
                            case "F": Old_Date = "Full"; break;
                            case "3": Old_Date = "3/4"; break;
                            case "2": Old_Date = "1/2"; break;
                            case "1": Old_Date = "1/4"; break;
                            default: Old_Date = " "; break;
                        }
                        switch (New_Date)
                        {
                            case "F": New_Date = "Full"; break;
                            case "3": New_Date = "3/4"; break;
                            case "2": New_Date = "1/2"; break;
                            case "1": New_Date = "1/4"; break;
                            default: Old_Date = " "; break;
                        }
                        break;
                }

                if (Col_Name == "TRANSFERED" || Col_Name == "TRANSFERRED" || Col_Name == "Transfer")
                    Col_Name = "Transfer";

                if (Col_Name == "Class") Col_Name = "Site – Room – AMPM";

                //Fld_Hist_Grid.Rows.Add(dr["EFHIST_ADD_DATE"].ToString(), "Start Date", dr_Xml["Old_Start_Date"].ToString(), dr_Xml["New_Start_Date"].ToString());
                Fld_Hist_Grid.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date);
                Fld_Hist_Grid_New.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date, Added_By);
                Add_Date = Added_By = " ";

                Rows_Cnt++;

                //string toolTipText = "Added By      : " + Entity.Add_Oper.Trim() + " on " + Entity.Add_Date + "\n" +
                //     "Modified By  : " + Entity.Lstc_Oper.Trim() + " on " + Entity.Lstc_Date;

                //foreach (DataGridViewCell cell in Act_Status_Grid.Rows[rowIndex].Cells)
                //{
                //    cell.ToolTipText = toolTipText;
                //}
            }

            return Rows_Cnt;
        }

        private int Fill_StatusEdit_History(DataRow dr)
        {
            //DataSet Fld_Hist_Data = _model.EnrollData.Browse_ENRLFLDHIST(Pass_Enroll_Entity.ID, null, null);

            int Rows_Cnt = 0;
            //bool Is_Header_Row = false;
            //if (Fld_Hist_Data.Tables.Count > 0)
            //{
            //    DataTable Fld_Hist_Table = Fld_Hist_Data.Tables[0];
            DataTable Fld_Hist_Table_Xml = new DataTable();
            string Add_Date = " ", Old_Date = " ", New_Date = " ", Col_Name = " ", Added_By = " ";
            //    foreach (DataRow dr in Fld_Hist_Table.Rows)
            //    {
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

                    if (Col_Name.Contains("Date") && !string.IsNullOrEmpty(Old_Date.Trim()))
                    {
                        //Old_Date = LookupDataAccess.Getdate(Old_Date);
                        Old_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Old_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                        New_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    }

                    Fld_Hist_Grid_New.Rows.Add(Add_Date, Col_Name, Old_Date, New_Date, Added_By);
                    Add_Date = Added_By = " ";

                    Rows_Cnt++;
                }
            }
            ////    }
            ////}

            //Fld_Hist_Grid_New.Visible = Fld_Hist_Grid_New.Visible = true;

            return Rows_Cnt;
        }

        string Sel_ID_Site = string.Empty, Sel_ID_Room = string.Empty, Sel_ID_AMPM = string.Empty;
        List<CaseEnrlEntity> App_Mult_Fund_List = new List<CaseEnrlEntity>();
        private void Get_App_in_All_Funds(string ID, string strYear)
        {
            App_Mult_Fund_List.Clear();
            lblpreviousMsg.Visible = false;
            Sel_ID_Site = Sel_ID_Room = Sel_ID_AMPM = string.Empty;
            string Tmp_Site_Details = "";
            CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            Search_Entity.Agy = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            if (strYear != string.Empty)
            {
                Search_Entity.Year = strYear;
            }
            else
                Search_Entity.Year = (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ");
            if (!string.IsNullOrEmpty(ID.Trim()))
                Search_Entity.ID = ID;
            else
                Search_Entity.App = Pass_Enroll_Entity.App;//BaseForm.BaseApplicationNo;

            switch (Module_Code)
            {
                case "02": Search_Entity.Rec_Type = "H"; break;
                case "03": Search_Entity.Rec_Type = "C"; break;
                default: Search_Entity.Rec_Type = "U"; break;
            }
            //Search_Entity.Rec_Type = (Module_Code == "03" ? "C" : "H");
            Search_Entity.Enrl_Status_Not_Equalto = "T";
            App_Mult_Fund_List = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity, "Browse");
            Act_Status_Grid.SelectionChanged -= new EventHandler(Act_Status_Grid_SelectionChanged);
            Act_Status_Grid.Rows.Clear();
            History_Grid.Rows.Clear();
            string Add_Date = " ", Status_Date = " ", Status_Desc = " ", Attn_Fdate = " ", Attn_Ldate = " ", Hie_Desc = "";

            int Tmp_loop_Cnt = 0, Row_Index = 0, TmP_Sel_Index = 0;
            //bool Is_Header_Row = false;
            if (App_Mult_Fund_List.Count > 0)
            {
                if (string.IsNullOrEmpty(ID.Trim()))
                    Pass_Enroll_Entity.ID = App_Mult_Fund_List[0].ID;

                foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                {
                    //if (Entity.Status != "T")
                    //{
                    Status_Date = Add_Date = Attn_Fdate = Attn_Ldate = " ";
                    if (!string.IsNullOrEmpty(Entity.Status_Date.Trim()))
                        Status_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.Status_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    if (!string.IsNullOrEmpty(Entity.Add_Date.Trim()))
                        Add_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.Add_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    //// Murali un commented below logic COI attendance Site wise wrong dates displayed Date on 09/20/2022
                    //if (!string.IsNullOrEmpty(Entity.Enrl_Stat_Attn_FDate.Trim()))
                    //    Attn_Fdate = LookupDataAccess.Getdate(Entity.Enrl_Stat_Attn_FDate);
                    //if (!string.IsNullOrEmpty(Entity.Enrl_Stat_Attn_LDate.Trim()))
                    //    Attn_Ldate = LookupDataAccess.Getdate(Entity.Enrl_Stat_Attn_LDate);

                    // Murali uncommented below logic CSEOP and we have changed procedure Browse_CASEENRL_ForApp_InMultFunds un commented Enrl_site values. Date on 10/25/2022
                    // Murali commented below logic COI attendance Site wise wrong dates displayed Date on 09/20/2022
                    if (!string.IsNullOrEmpty(Entity.Enrl_Min_Attn_Date))
                        Attn_Fdate = LookupDataAccess.Getdate(Entity.Enrl_Min_Attn_Date);
                    if (!string.IsNullOrEmpty(Entity.Enrl_Max_Attn_Date.Trim()))
                        Attn_Ldate = LookupDataAccess.Getdate(Entity.Enrl_Max_Attn_Date);

                    if (Entity.ID == Pass_Enroll_Entity.ID)
                    {
                        Sel_ID_Site = Entity.Site; Sel_ID_Room = Entity.Room; Sel_ID_AMPM = Entity.AMPM;
                    }

                    Tmp_Site_Details = Entity.Site + (!string.IsNullOrEmpty(Entity.Room.Trim()) ? ("/" + Entity.Room) : "") + (!string.IsNullOrEmpty(Entity.AMPM.Trim()) ? ("/" + Entity.AMPM) : "");
                    if (string.IsNullOrEmpty(Tmp_Site_Details.Trim()))
                        Tmp_Site_Details = Entity.Mst_Site + "/****/*";

                    Status_Desc = " ";
                    switch (Entity.Status)
                    {
                        case "L": Status_Desc = "Wait List"; break;
                        case "P": Status_Desc = "Pending"; break;
                        case "R": Status_Desc = "Denied"; break;
                        case "E": Status_Desc = "Enrolled"; break;
                        case "W": Status_Desc = "Withdrawn"; break;
                        case "I": Status_Desc = "Post Intake"; break;
                        case "N": Status_Desc = "Inactive"; break;
                        case "A": Status_Desc = "Parent declined"; break;
                        case "B": Status_Desc = "No Longer Interested"; break;
                        case "C": Status_Desc = "Accepted"; break;
                        case "X": Status_Desc = "Exited"; break;


                    }

                    Status_Desc += (Entity.Enrl_Denied == "Y" ? " - Denied" : "");

                    Hie_Desc = "";
                    if (Module_Code == "03")
                    {
                        if (Entity.FundHie.Trim().Length >= 6)
                        {
                            DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Entity.FundHie.Substring(0, 2), Entity.FundHie.Substring(2, 2), Entity.FundHie.Substring(4, 2));
                            if (ds_PROG.Tables.Count > 0)
                            {
                                if (ds_PROG.Tables[0].Rows.Count > 0)
                                    Hie_Desc += " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                            }
                        }
                    }

                    //Row_Index = Act_Status_Grid.Rows.Add(Status_Desc, Status_Date, Entity.Site + (!string.IsNullOrEmpty(Entity.Room.Trim()) ? ("/" + Entity.Room) : "") + (!string.IsNullOrEmpty(Entity.AMPM.Trim()) ? ("/" + Entity.AMPM) : ""), Entity.FundHie + Hie_Desc, Attn_Fdate, Attn_Ldate, " ", " ", Entity.ID, Entity.Enrl_Denied);
                    Row_Index = Act_Status_Grid.Rows.Add(Status_Desc, Status_Date, Tmp_Site_Details, Entity.FundHie + Hie_Desc, Attn_Fdate, Attn_Ldate, " ", " ", Entity.ID, Entity.Enrl_Denied, Entity.Desc_1, Entity.Desc_2);
                    Add_Date = " ";
                    set_SPGridTooltip(Row_Index, Entity);

                    if (ID == Entity.ID)
                        TmP_Sel_Index = Row_Index;

                    Tmp_loop_Cnt++;
                    //}
                }
            }
            else
            {
                btnRMEditstatus.Visible = false;    //  Edit Status Record
                btnRMAddFund.Visible = false;       //  Add Fund
                btnRMEditfundDt.Visible = false;    //  Edit Fund Date/Details
                btnRMTransfer.Visible = false;      //  Transfer
                btnRMDelstatus.Visible = false;     //  Delete Status Record
            }

            if (Tmp_loop_Cnt > 0)
            {
                Act_Status_Grid.CurrentCell = Act_Status_Grid.Rows[TmP_Sel_Index].Cells[1];
                Get_Field_History(Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_ID"].Value.ToString());
            }
            if (Act_Status_Grid.Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
                {
                    if (this.Text == "Enroll History")
                    {
                        if (Convert.ToInt32(BaseForm.BaseYear) == Convert.ToInt32(((ListItem)CmbYear.SelectedItem).Text.ToString()))
                        {
                            string strPreviousYear = (Convert.ToInt32(BaseForm.BaseYear) - 1).ToString();
                            CaseEnrlEntity Search_Entity1 = new CaseEnrlEntity(true);
                            Search_Entity1.Agy = BaseForm.BaseAgency;
                            Search_Entity1.Dept = BaseForm.BaseDept;
                            Search_Entity1.Prog = BaseForm.BaseProg;
                            Search_Entity1.Year = strPreviousYear;

                            Search_Entity1.App = Pass_Enroll_Entity.App;//BaseForm.BaseApplicationNo;

                            switch (Module_Code)
                            {
                                case "02": Search_Entity1.Rec_Type = "H"; break;
                                case "03": Search_Entity1.Rec_Type = "C"; break;
                                default: Search_Entity1.Rec_Type = "U"; break;
                            }
                            //Search_Entity.Rec_Type = (Module_Code == "03" ? "C" : "H");
                            Search_Entity1.Enrl_Status_Not_Equalto = "T";
                            List<CaseEnrlEntity> App_Mult_Fund_List1 = _model.EnrollData.Browse_CASEENRL_ForApp_InMultFunds(Search_Entity1, "Browse");
                            if (App_Mult_Fund_List1.Count > 0)
                                lblpreviousMsg.Visible = true;
                        }
                    }
                }
            }
            Act_Status_Grid.SelectionChanged += new EventHandler(Act_Status_Grid_SelectionChanged);
            Act_Status_Grid_SelectionChanged(Act_Status_Grid, new EventArgs());
        }


        private void set_SPGridTooltip(int rowIndex, CaseEnrlEntity Entity)
        {
            string toolTipText = "Added By      : " + Entity.Add_Oper.Trim() + " on " + Entity.Add_Date + "\n" +
                                 "Modified By  : " + Entity.Lstc_Oper.Trim() + " on " + Entity.Lstc_Date;

            foreach (DataGridViewCell cell in Act_Status_Grid.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }


        private void Act_Status_Grid_Click(object sender, EventArgs e)
        {

        }

        private void Act_Status_Grid_SelectionChanged(object sender, EventArgs e)
        {
            Lbl_Hist_Gap.Visible = Cb_Show_Entire_Status.Checked = false;
            if (Act_Status_Grid.Rows.Count > 0)
            {
                Get_History();
                Get_Field_History(Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_ID"].Value.ToString());

                Lbl_Teacher.Text = "";
                if (Module_Code == "02")
                    Fill_HeadTeacher_Data();

                EnbDisRMbtns();
            }
        }

        private void Cb_Show_Entire_Status_CheckedChanged(object sender, EventArgs e)
        {
            if (Act_Status_Grid.Rows.Count > 0)
                Get_History();
        }


        int Priv_Group = int.MaxValue, Curr_Group;
        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            MenuItem Menu_Edit = new MenuItem();
            if (Year_Priv_SW && Act_Status_Grid.Rows.Count > 0)
            {
                Menu_Edit.Text = "Edit Status";
                Menu_Edit.Tag = "Edit_STAT";

                if (!Privileges.ChangePriv.Equals("false"))
                    contextMenu1.MenuItems.Add(Menu_Edit);
            }



            //if (Act_Status_Grid.Rows.Count > 0 && ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") || (Privileges.Program == "ENRLHIST" && Module_Code == "02")))
            if (Act_Status_Grid.Rows.Count > 0)
            {
                if ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") ||
                   (Privileges.Program == "ENRLHIST" && Module_Code == "02"))
                {
                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Enrolled")
                    {
                        if (Year_Priv_SW)
                        {
                            MenuItem Menu_L1 = new MenuItem();
                            Menu_L1.Text = "Add Fund";
                            Menu_L1.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L1);

                            MenuItem Menu_L2 = new MenuItem();
                            Menu_L2.Text = "Edit Fund Date/Details";
                            Menu_L2.Tag = "Edit_FUND";

                            if (!Privileges.ChangePriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L2);

                            MenuItem Menu_L3 = new MenuItem();
                            Menu_L3.Text = "Transfer";
                            Menu_L3.Tag = "Transfer";

                            if (!Privileges.ChangePriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L3);
                        }
                    }

                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Withdrawn")
                    {
                        MenuItem Menu_L2 = new MenuItem();
                        Menu_L2.Text = "Edit Fund Date/Details";
                        Menu_L2.Tag = "Edit_FUND";

                        if (!Privileges.ChangePriv.Equals("false"))
                            contextMenu1.MenuItems.Add(Menu_L2);
                    }

                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Wait List")
                    {
                        if (Year_Priv_SW)
                        {
                            // added by sudheer on 03/27/2018 as per COI document
                            MenuItem Menu_L1 = new MenuItem();
                            Menu_L1.Text = "Add Fund";
                            Menu_L1.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L1);

                            //end sudheer

                            MenuItem Menu_L3 = new MenuItem();
                            Menu_L3.Text = "Transfer";
                            Menu_L3.Tag = "Transfer";

                            if (!Privileges.ChangePriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L3);
                        }
                    }


                    //if (string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString().Trim()) &&
                    //    string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString().Trim()) &&
                    //    (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))

                    //if ((BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")) // Brain asked to Remove Attendance Check here and Give an Alert in Delete
                    //{
                    MenuItem Menu_Del = new MenuItem();
                    Menu_Del.Text = "Delete Status Record";//"Delete this Enrollment";
                    Menu_Del.Tag = "DELETE";

                    if (!Privileges.DelPriv.Equals("false"))
                        contextMenu1.MenuItems.Add(Menu_Del);
                    // }
                    //else
                    //{
                    //    MenuItem Menu_Del = new MenuItem();
                    //    Menu_Del.Text = "cannot delete, attendance record(s) exist";//"Delete this Enrollment";
                    //    Menu_Del.Tag = "DELETE";
                    //    contextMenu1.MenuItems.Add(Menu_Del);
                    //}
                }
                else
                {
                    //if ((BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                    //{
                    MenuItem Menu_Del = new MenuItem();
                    Menu_Del.Text = "Delete Status Record";//"Delete this Enrollment";
                    Menu_Del.Tag = "DELETE";

                    if (!Privileges.DelPriv.Equals("false"))
                        contextMenu1.MenuItems.Add(Menu_Del);
                    // }

                    //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                    //{
                    if (!Privileges.ChangePriv.Equals("false"))
                    {
                        MenuItem Menu_Edit_Date = new MenuItem();
                        Menu_Edit_Date.Text = "Edit Status Date";//"Delete this Enrollment";
                        Menu_Edit_Date.Tag = "EDITSTATDATE";
                        contextMenu1.MenuItems.Add(Menu_Edit_Date);
                    }

                    //Btn_Edit_Curr_Status_Date.Location = new System.Drawing.Point(495, 49);
                    //Btn_Edit_Curr_Status_Date.Size = new System.Drawing.Size(56, 59);
                    //Btn_Edit_Curr_Status_Date.Visible = true;
                    // }

                }
            }
            MenuItem Menu_L4 = new MenuItem();
            MenuItem Menu_L5 = new MenuItem();

            if (Act_Status_Grid.Rows.Count > 0)
            {
                if (Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value.ToString() == "Y")
                {
                    Menu_L4.Text = "Remove Denied";
                    Menu_L4.Tag = "Denied";
                }
                else
                {
                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Withdrawn"
                        && ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") || (Privileges.Program == "ENRLHIST" && Module_Code == "02")))
                    {
                        if (Year_Priv_SW)
                        {
                            Menu_L5.Text = "Add Fund";
                            Menu_L5.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                                contextMenu1.MenuItems.Add(Menu_L5);
                        }
                    }

                    Menu_L4.Text = "Denied App#";
                    Menu_L4.Tag = "Denied";
                }
                if (BaseForm.BusinessModuleID == "03")  // This Option is ment for CM Only
                    contextMenu1.MenuItems.Add(Menu_L4);
            }

        }


        private void EnbDisRMbtns()
        {
            btnRMEditstatus.Visible = false;    //  Edit Status Record
            btnRMAddFund.Visible = false;       //  Add Fund
            btnRMEditfundDt.Visible = false;    //  Edit Fund Date/Details
            btnRMTransfer.Visible = false;      //  Transfer
            btnRMDelstatus.Visible = false;     //  Delete Status Record

            contextMenu1.MenuItems.Clear();
            //MenuItem Menu_Edit = new MenuItem();
            if (Year_Priv_SW && Act_Status_Grid.Rows.Count > 0)
            {
                //Menu_Edit.Text = "Edit Status";
                //Menu_Edit.Tag = "Edit_STAT";
                if (!Privileges.ChangePriv.Equals("false"))
                {
                    //  contextMenu1.MenuItems.Add(Menu_Edit);
                    btnRMEditstatus.Visible = true;
                    btnRMEditstatus.Tag= "Edit_STAT";
                }
            }


            //if (Act_Status_Grid.Rows.Count > 0 && ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") || (Privileges.Program == "ENRLHIST" && Module_Code == "02")))
            if (Act_Status_Grid.Rows.Count > 0)
            {
                if ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") ||
                   (Privileges.Program == "ENRLHIST" && Module_Code == "02"))
                {
                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Enrolled")
                    {
                        if (Year_Priv_SW)
                        {
                            //MenuItem Menu_L1 = new MenuItem();
                            //Menu_L1.Text = "Add Fund";
                            //Menu_L1.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                            {
                                //   contextMenu1.MenuItems.Add(Menu_L1);
                                btnRMAddFund.Visible = true;
                                btnRMAddFund.Tag = "Add_FUND";
                            }

                            //MenuItem Menu_L2 = new MenuItem();
                            //Menu_L2.Text = "Edit Fund Date/Details";
                            //Menu_L2.Tag = "Edit_FUND";

                            if (!Privileges.ChangePriv.Equals("false"))
                            {
                                //contextMenu1.MenuItems.Add(Menu_L2);
                                btnRMEditfundDt.Visible= true;
                                btnRMEditfundDt.Tag = "Edit_FUND";
                            }

                            //MenuItem Menu_L3 = new MenuItem();
                            //Menu_L3.Text = "Transfer";
                            //Menu_L3.Tag = "Transfer";

                            if (!Privileges.ChangePriv.Equals("false"))
                            {
                                //contextMenu1.MenuItems.Add(Menu_L3);
                                btnRMTransfer.Visible = true;
                                btnRMTransfer.Tag = "Transfer";
                            }
                        }
                    }

                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Withdrawn")
                    {
                        //MenuItem Menu_L2 = new MenuItem();
                        //Menu_L2.Text = "Edit Fund Date/Details";
                        //Menu_L2.Tag = "Edit_FUND";

                        if (!Privileges.ChangePriv.Equals("false"))
                        {
                            //contextMenu1.MenuItems.Add(Menu_L2);
                            btnRMEditfundDt.Visible = true;
                            btnRMEditfundDt.Tag = "Edit_FUND";
                        }
                    }

                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Wait List")
                    {
                        if (Year_Priv_SW)
                        {
                            // added by sudheer on 03/27/2018 as per COI document
                            //MenuItem Menu_L1 = new MenuItem();
                            //Menu_L1.Text = "Add Fund";
                            //Menu_L1.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                            {
                                //contextMenu1.MenuItems.Add(Menu_L1);
                                btnRMAddFund.Visible= true;
                                btnRMAddFund.Tag = "Add_FUND";
                            }

                            //end sudheer

                            //MenuItem Menu_L3 = new MenuItem();
                            //Menu_L3.Text = "Transfer";
                            //Menu_L3.Tag = "Transfer";

                            if (!Privileges.ChangePriv.Equals("false"))
                            {
                                //contextMenu1.MenuItems.Add(Menu_L3);
                                btnRMTransfer.Visible= true;
                                btnRMTransfer.Tag = "Transfer";
                            }
                        }
                    }


                    //if (string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString().Trim()) &&
                    //    string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString().Trim()) &&
                    //    (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))

                    //if ((BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")) // Brain asked to Remove Attendance Check here and Give an Alert in Delete
                    //{
                    //MenuItem Menu_Del = new MenuItem();
                    //Menu_Del.Text = "Delete Status Record";//"Delete this Enrollment";
                    //Menu_Del.Tag = "DELETE";

                    if (!Privileges.DelPriv.Equals("false"))
                    {
                        //contextMenu1.MenuItems.Add(Menu_Del);
                        btnRMDelstatus.Visible = true;
                        btnRMDelstatus.Tag = "DELETE";
                    }
                    // }
                    //else
                    //{
                    //    MenuItem Menu_Del = new MenuItem();
                    //    Menu_Del.Text = "cannot delete, attendance record(s) exist";//"Delete this Enrollment";
                    //    Menu_Del.Tag = "DELETE";
                    //    contextMenu1.MenuItems.Add(Menu_Del);
                    //}
                }
                else
                {
                    //if ((BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                    //{
                    //MenuItem Menu_Del = new MenuItem();
                    //Menu_Del.Text = "Delete Status Record";//"Delete this Enrollment";
                    //Menu_Del.Tag = "DELETE";

                    if (!Privileges.DelPriv.Equals("false"))
                    {
                        //contextMenu1.MenuItems.Add(Menu_Del);
                        btnRMDelstatus.Visible = true;
                        btnRMDelstatus.Tag = "DELETE";
                    }
                    // }

                    //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                    //{
                    if (!Privileges.ChangePriv.Equals("false"))
                    {
                        //MenuItem Menu_Edit_Date = new MenuItem();
                        //Menu_Edit_Date.Text = "Edit Status Date";//"Delete this Enrollment";
                        //Menu_Edit_Date.Tag = "EDITSTATDATE";
                        //contextMenu1.MenuItems.Add(Menu_Edit_Date);

                        btnRMEditstatus.Visible = true; 
                        btnRMEditstatus.Tag = "EDITSTATDATE";

                    }

                    //Btn_Edit_Curr_Status_Date.Location = new System.Drawing.Point(495, 49);
                    //Btn_Edit_Curr_Status_Date.Size = new System.Drawing.Size(56, 59);
                    //Btn_Edit_Curr_Status_Date.Visible = true;
                    // }

                }
            }
            //MenuItem Menu_L4 = new MenuItem();
            //MenuItem Menu_L5 = new MenuItem();

            if (Act_Status_Grid.Rows.Count > 0)
            {
                if (Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value.ToString() == "Y")
                {
                    //Kranthi 05/24/2024:: We don't when this is appear so as of now we are commenting this
                    //Menu_L4.Text = "Remove Denied";
                    //Menu_L4.Tag = "Denied";
                }
                else
                {
                    if (Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString().Trim() == "Withdrawn"
                        && ((Privileges.Program != "ENRLHIST" && Privileges.ModuleCode == "02") || (Privileges.Program == "ENRLHIST" && Module_Code == "02")))
                    {
                        if (Year_Priv_SW)
                        {
                            //Menu_L5.Text = "Add Fund";
                            //Menu_L5.Tag = "Add_FUND";

                            if (!Privileges.AddPriv.Equals("false"))
                            {
                                //contextMenu1.MenuItems.Add(Menu_L5);
                                btnRMAddFund.Visible = true;
                                btnRMAddFund.Tag = "Add_FUND";
                            }
                        }
                    }

                    //Kranthi 05/24/2024:: We don't when this is appear so as of now we are commenting this
                    //Menu_L4.Text = "Denied App#";
                    //Menu_L4.Tag = "Denied";
                }
                if (BaseForm.BusinessModuleID == "03")  // This Option is ment for CM Only
                {
                   // contextMenu1.MenuItems.Add(Menu_L4);
                }
            }

        }

        string Oper_Performed = string.Empty;
        private void Act_Status_Grid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string Sql_SP_Result_Message = string.Empty;
            string[] Split_Array = new string[2];

            if (objArgs.MenuItem.Tag is string)
            {
                Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");
                string Sel_Tmp_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString();
                string Sel_Tmp_Status = Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value.ToString();
                string Tmp_Desc = Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString();
                string Tmp_Site = Act_Status_Grid.CurrentRow.Cells["ActHist_Site"].Value.ToString();

                if (!Split_Array[0].Contains("Denied"))
                {
                    string Class_Start_Date = string.Empty,
                           Class_End_Date = string.Empty,
                           Attn_Last_Date = string.Empty,
                           Attn_First_Date = string.Empty;

                    Attn_Last_Date = Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString();
                    Attn_First_Date = Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString();


                    //string Class_Start_Date = Top_Grid.CurrentRow.Cells["TGD_Start_Date"].Value.ToString(),
                    //       Class_End_Date = Top_Grid.CurrentRow.Cells["TGD_End_Date"].Value.ToString(),
                    //       Attn_Last_Date = Bottom_Grid.CurrentRow.Cells["BGD_Attn_Lstc_Date"].Value.ToString();

                    //if (Top_Grid.CurrentRow.Cells["TGD_Desc"].Value.ToString().Contains("All Rooms in Site - ") &&
                    //    Top_Grid.CurrentRow.Cells["TGD_Room_Date"].Value.ToString().Equals("****"))
                    //{
                    //    string Tmp_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString();
                    //    foreach (CaseEnrlEntity Entity in Enroll_List)
                    //    {
                    //        if (Tmp_ID == Entity.ID)
                    //        {
                    //            foreach (CaseSiteEntity Ent in HSS_Site_List)
                    //            {
                    //                if (Ent.SiteNUMBER == Entity.Site && Ent.SiteROOM.Trim() == Entity.Room && Ent.SiteAM_PM.Trim() == Entity.AMPM)
                    //                {
                    //                    Class_Start_Date = Ent.SiteCLASS_START;
                    //                    Class_End_Date = Ent.SiteCLASS_END;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    string[] Class_Date = Get_Sel_Prog_App_To_Add_Fund();
                    Oper_Performed = Split_Array[0];

                    if (Oper_Performed != "DELETE" && Oper_Performed != "Edit_STAT" && Oper_Performed != "EDITSTATDATE")
                    {
                        string Defined_Funds_List = string.Empty;
                        if (Oper_Performed == "Add_FUND")
                        {
                            foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                            {
                                if (Entity.Site == Sel_ID_Site && Entity.Room == Sel_ID_Room &&
                                    Entity.AMPM == Sel_ID_AMPM)
                                    Defined_Funds_List = Defined_Funds_List + " " + Entity.FundHie + ", ";
                            }
                        }
                        if (Oper_Performed == "Transfer" && Tmp_Desc.ToUpper() == "WAIT LIST" && Tmp_Site.Contains("****/*"))
                        {
                            CASE0010_CASE_Form Add_Edit_Trns_Form = new CASE0010_CASE_Form(BaseForm, Privileges, Split_Array[0], Pass_Enroll_List, string.Empty, " ", string.Empty,
                                                                                                   Class_Date[0], Class_Date[1], Attn_First_Date, Attn_Last_Date, Defined_Funds_List, string.Empty, string.Empty, "Y");
                            Add_Edit_Trns_Form.FormClosed += new FormClosedEventHandler(Add_Edit_Trns_Form_Close);
                            Add_Edit_Trns_Form.StartPosition = FormStartPosition.CenterScreen;
                            Add_Edit_Trns_Form.ShowDialog();
                        }
                        else
                        {
                            CASE0010_CASE_Form Add_Edit_Trns_Form = new CASE0010_CASE_Form(BaseForm, Privileges, Split_Array[0], Pass_Enroll_List, string.Empty, " ", string.Empty,
                                                                                                Class_Date[0], Class_Date[1], Attn_First_Date, Attn_Last_Date, Defined_Funds_List, string.Empty, string.Empty);
                            //CASE0010_CASE_Form Post_Intake = new CASE0010_CASE_Form(BaseForm, Privileges, "Add", Get_Sel_Prog_App_To_Pass(), "010299    ", "Test Program");
                            Add_Edit_Trns_Form.FormClosed += new FormClosedEventHandler(Add_Edit_Trns_Form_Close);
                            Add_Edit_Trns_Form.StartPosition = FormStartPosition.CenterScreen;
                            Add_Edit_Trns_Form.ShowDialog();
                        }
                    }
                    else
                    {
                        switch (Oper_Performed)
                        {
                            case "DELETE":
                                if (!string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString().Trim()) &&
                                    !string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString().Trim()))
                                    AlertBox.Show("You Cannot Delete Selected Status Record \n Attendance Exist for this status record!!!", MessageBoxIcon.Warning);
                                else
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected Status Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked); break;
                            case "Edit_STAT": Edit_Sel_Enrollment_Status(Sel_Tmp_ID); break;

                            case "EDITSTATDATE":
                                CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(Act_Status_Grid.CurrentRow.Cells["ActHist_From"].Value.ToString().Trim(), CM_Last_Hist_FDate, "Enrl");
                                Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_New_Status_Date);
                                Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
                                Mem_Sel_Form.ShowDialog();
                                break;
                        }
                    }
                }
                else
                {
                    Sel_Tmp_Status = (Sel_Tmp_Status == "Y" ? "N" : "Y");

                    if (_model.EnrollData.UpdateCASEENRL_Denied_Status(Sel_Tmp_ID, Sel_Tmp_Status, BaseForm.UserID, out Sql_SP_Result_Message))
                    {
                        AlertBox.Show((Sel_Tmp_Status == "Y" ? "Applicant's Denied Flag is Set" : "Applicant's Denied Flag is Reset"), MessageBoxIcon.Warning);
                        //if (Tmp_Status == "Y")
                        //{
                        //string Tmp_Desc = string.Empty;
                        // Tmp_Desc = Act_Status_Grid.CurrentRow.Cells["Tmp_Status"].Value.ToString();
                        Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value = (Sel_Tmp_Status == "Y" ? Tmp_Desc + " - Denied" : Tmp_Desc.Replace(" - Denied", " "));

                        this.DialogResult = DialogResult.OK;

                        //}
                        Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value = Sel_Tmp_Status;

                        //int Tmp_Cnt = int.Parse(Top_Grid.CurrentRow.Cells["TGD_Stat1"].Value.ToString()), Tmp_Old_Cnt = 0;
                        //Tmp_Old_Cnt = Tmp_Cnt;
                        //int Tmp_Tot_Cnt = int.Parse(Top_Grid.Rows[(Top_Grid.Rows.Count - 1)].Cells["TGD_Stat1"].Value.ToString());
                        //Tmp_Cnt = (Tmp_Status == "Y" ? (Tmp_Cnt + 1) : (Tmp_Cnt > 0 ? (Tmp_Cnt - 1) : 0));
                        //Top_Grid.CurrentRow.Cells["TGD_Stat1"].Value = Tmp_Cnt;
                        //Top_Grid.Rows[(Top_Grid.Rows.Count - 1)].Cells["TGD_Stat1"].Value = (Tmp_Tot_Cnt - Tmp_Old_Cnt + Tmp_Cnt);

                        //foreach (CaseEnrlEntity Entity in Enroll_List)
                        //{
                        //    if (Tmp_ID == Entity.ID)
                        //    {
                        //        Entity.Enrl_Denied = Tmp_Status;
                        //        break;
                        //    }
                        //}
                    }
                    else
                        AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);

                }

            }
        }

        private void RM_ButtonsClick(object sender, EventArgs e)
        {
            Button btnRm = ((sender)) as Button;
            string Sql_SP_Result_Message = string.Empty;
            string[] Split_Array = new string[2];

            if (btnRm.Tag is string)
            {
                Split_Array = Regex.Split(btnRm.Tag.ToString(), " ");
                string Sel_Tmp_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString();
                string Sel_Tmp_Status = Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value.ToString();
                string Tmp_Desc = Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value.ToString();
                string Tmp_Site = Act_Status_Grid.CurrentRow.Cells["ActHist_Site"].Value.ToString();

                if (!Split_Array[0].Contains("Denied"))
                {
                    string Class_Start_Date = string.Empty,
                           Class_End_Date = string.Empty,
                           Attn_Last_Date = string.Empty,
                           Attn_First_Date = string.Empty;

                    Attn_Last_Date = Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString();
                    Attn_First_Date = Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString();


                    //string Class_Start_Date = Top_Grid.CurrentRow.Cells["TGD_Start_Date"].Value.ToString(),
                    //       Class_End_Date = Top_Grid.CurrentRow.Cells["TGD_End_Date"].Value.ToString(),
                    //       Attn_Last_Date = Bottom_Grid.CurrentRow.Cells["BGD_Attn_Lstc_Date"].Value.ToString();

                    //if (Top_Grid.CurrentRow.Cells["TGD_Desc"].Value.ToString().Contains("All Rooms in Site - ") &&
                    //    Top_Grid.CurrentRow.Cells["TGD_Room_Date"].Value.ToString().Equals("****"))
                    //{
                    //    string Tmp_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString();
                    //    foreach (CaseEnrlEntity Entity in Enroll_List)
                    //    {
                    //        if (Tmp_ID == Entity.ID)
                    //        {
                    //            foreach (CaseSiteEntity Ent in HSS_Site_List)
                    //            {
                    //                if (Ent.SiteNUMBER == Entity.Site && Ent.SiteROOM.Trim() == Entity.Room && Ent.SiteAM_PM.Trim() == Entity.AMPM)
                    //                {
                    //                    Class_Start_Date = Ent.SiteCLASS_START;
                    //                    Class_End_Date = Ent.SiteCLASS_END;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    string[] Class_Date = Get_Sel_Prog_App_To_Add_Fund();
                    Oper_Performed = Split_Array[0];

                    if (Oper_Performed != "DELETE" && Oper_Performed != "Edit_STAT" && Oper_Performed != "EDITSTATDATE")
                    {
                        string Defined_Funds_List = string.Empty;
                        if (Oper_Performed == "Add_FUND")
                        {
                            foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                            {
                                if (Entity.Site == Sel_ID_Site && Entity.Room == Sel_ID_Room &&
                                    Entity.AMPM == Sel_ID_AMPM)
                                    Defined_Funds_List = Defined_Funds_List + " " + Entity.FundHie + ", ";
                            }
                        }
                        if (Oper_Performed == "Transfer" && Tmp_Desc.ToUpper() == "WAIT LIST" && Tmp_Site.Contains("****/*"))
                        {
                            CASE0010_CASE_Form Add_Edit_Trns_Form = new CASE0010_CASE_Form(BaseForm, Privileges, Split_Array[0], Pass_Enroll_List, string.Empty, " ", string.Empty,
                                                                                                   Class_Date[0], Class_Date[1], Attn_First_Date, Attn_Last_Date, Defined_Funds_List, string.Empty, string.Empty, "Y");
                            Add_Edit_Trns_Form.FormClosed += new FormClosedEventHandler(Add_Edit_Trns_Form_Close);
                            Add_Edit_Trns_Form.StartPosition = FormStartPosition.CenterScreen;
                            Add_Edit_Trns_Form.ShowDialog();
                        }
                        else
                        {
                            CASE0010_CASE_Form Add_Edit_Trns_Form = new CASE0010_CASE_Form(BaseForm, Privileges, Split_Array[0], Pass_Enroll_List, string.Empty, " ", string.Empty,
                                                                                                Class_Date[0], Class_Date[1], Attn_First_Date, Attn_Last_Date, Defined_Funds_List, string.Empty, string.Empty);
                            //CASE0010_CASE_Form Post_Intake = new CASE0010_CASE_Form(BaseForm, Privileges, "Add", Get_Sel_Prog_App_To_Pass(), "010299    ", "Test Program");
                            Add_Edit_Trns_Form.FormClosed += new FormClosedEventHandler(Add_Edit_Trns_Form_Close);
                            Add_Edit_Trns_Form.StartPosition = FormStartPosition.CenterScreen;
                            Add_Edit_Trns_Form.ShowDialog();
                        }
                    }
                    else
                    {
                        switch (Oper_Performed)
                        {
                            case "DELETE":
                                if (!string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_SDate"].Value.ToString().Trim()) &&
                                    !string.IsNullOrEmpty(Act_Status_Grid.CurrentRow.Cells["ActHist_Attn_LDate"].Value.ToString().Trim()))
                                    AlertBox.Show("You Cannot Delete Selected Status Record \n Attendance Exist for this status record!!!", MessageBoxIcon.Warning);
                                else
                                    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage() + "\n Selected Status Record", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: OnDeleteMessageBoxClicked); break;
                            case "Edit_STAT": Edit_Sel_Enrollment_Status(Sel_Tmp_ID); break;

                            case "EDITSTATDATE":
                                CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(Act_Status_Grid.CurrentRow.Cells["ActHist_From"].Value.ToString().Trim(), CM_Last_Hist_FDate, "Enrl");
                                Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_New_Status_Date);
                                Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
                                Mem_Sel_Form.ShowDialog();
                                break;
                        }
                    }
                }
                else
                {
                    Sel_Tmp_Status = (Sel_Tmp_Status == "Y" ? "N" : "Y");

                    if (_model.EnrollData.UpdateCASEENRL_Denied_Status(Sel_Tmp_ID, Sel_Tmp_Status, BaseForm.UserID, out Sql_SP_Result_Message))
                    {
                        AlertBox.Show((Sel_Tmp_Status == "Y" ? "Applicant's Denied Flag is Set" : "Applicant's Denied Flag is Reset"), MessageBoxIcon.Warning);
                        //if (Tmp_Status == "Y")
                        //{
                        //string Tmp_Desc = string.Empty;
                        // Tmp_Desc = Act_Status_Grid.CurrentRow.Cells["Tmp_Status"].Value.ToString();
                        Act_Status_Grid.CurrentRow.Cells["ActHist_Status"].Value = (Sel_Tmp_Status == "Y" ? Tmp_Desc + " - Denied" : Tmp_Desc.Replace(" - Denied", " "));

                        this.DialogResult = DialogResult.OK;

                        //}
                        Act_Status_Grid.CurrentRow.Cells["ActHist_Denied"].Value = Sel_Tmp_Status;

                        //int Tmp_Cnt = int.Parse(Top_Grid.CurrentRow.Cells["TGD_Stat1"].Value.ToString()), Tmp_Old_Cnt = 0;
                        //Tmp_Old_Cnt = Tmp_Cnt;
                        //int Tmp_Tot_Cnt = int.Parse(Top_Grid.Rows[(Top_Grid.Rows.Count - 1)].Cells["TGD_Stat1"].Value.ToString());
                        //Tmp_Cnt = (Tmp_Status == "Y" ? (Tmp_Cnt + 1) : (Tmp_Cnt > 0 ? (Tmp_Cnt - 1) : 0));
                        //Top_Grid.CurrentRow.Cells["TGD_Stat1"].Value = Tmp_Cnt;
                        //Top_Grid.Rows[(Top_Grid.Rows.Count - 1)].Cells["TGD_Stat1"].Value = (Tmp_Tot_Cnt - Tmp_Old_Cnt + Tmp_Cnt);

                        //foreach (CaseEnrlEntity Entity in Enroll_List)
                        //{
                        //    if (Tmp_ID == Entity.ID)
                        //    {
                        //        Entity.Enrl_Denied = Tmp_Status;
                        //        break;
                        //    }
                        //}
                    }
                    else
                        AlertBox.Show(Sql_SP_Result_Message, MessageBoxIcon.Warning);

                }

            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
                Delete_Sel_Enrollment(Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString());
        }


        private void Delete_Sel_Enrollment(string sel_Tmp_ID)
        {
            StringBuilder Delete_IDs_List = new StringBuilder();
            Delete_IDs_List.Append("<Rows>");

            Delete_IDs_List.Append("<Row Enrl_ID = \"" + sel_Tmp_ID + "\"/>");

            Delete_IDs_List.Append("</Rows>");
            CaseEnrlEntity Update_Entity = new CaseEnrlEntity(true);
            Sql_SP_Result_Message = string.Empty;
            Update_Entity.Row_Type = "D";
            Update_Entity.Agy = Update_Entity.Dept = Update_Entity.Prog =
            Update_Entity.Year = Update_Entity.App = Update_Entity.Group =
            Update_Entity.FundHie = Update_Entity.ID = Update_Entity.Seq =
            Update_Entity.Lstc_Oper = "1";
            Update_Entity.Status_Date = DateTime.Today.ToShortDateString();
            Update_Entity.FundHie = Act_Status_Grid.CurrentRow.Cells["ActHist_Fund"].Value.ToString();

            if (_model.EnrollData.UpdateCASEENRL(Update_Entity, "Edit", Delete_IDs_List.ToString(), null, null, out Sql_SP_Result_Message))
            {
                this.DialogResult = DialogResult.OK;
                Get_App_in_All_Funds(string.Empty, string.Empty);
                if (App_Mult_Fund_List.Count == 0)
                    this.Close();
            }
        }

        private void Edit_Sel_Enrollment_Status(string sel_Tmp_ID)
        {
            List<CaseEnrlEntity> Change_Status_list = new List<CaseEnrlEntity>();

            foreach (CaseEnrlEntity ent in App_Mult_Fund_List)
            {
                if (sel_Tmp_ID == ent.ID)
                {
                    Change_Status_list.Add(new CaseEnrlEntity(ent));
                    break;
                }
            }
            CASE0010_StatusChange_Form Status_Form;
            //CASE0010_StatusChange_Form Status_Form = new CASE0010_StatusChange_Form(BaseForm, Change_Status_list, Privileges.ModuleCode, string.Empty, Class_Start_Date, Class_End_Date);
            if (Privileges.Program == "ENRLHIST")
                Status_Form = new CASE0010_StatusChange_Form(BaseForm, Change_Status_list, Module_Code, string.Empty, Class_Start_Date, Class_End_Date, Privileges);
            else
                Status_Form = new CASE0010_StatusChange_Form(BaseForm, Change_Status_list, Privileges.ModuleCode, string.Empty, Class_Start_Date, Class_End_Date, Privileges);
            Status_Form.FormClosed += new FormClosedEventHandler(Change_Statue_Close);
            Status_Form.StartPosition = FormStartPosition.CenterScreen;
            Status_Form.ShowDialog();
        }

        private void Change_Statue_Close(object sender, FormClosedEventArgs e)
        {
            CASE0010_StatusChange_Form form = sender as CASE0010_StatusChange_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                Get_App_in_All_Funds(Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString(), string.Empty);
                ENRLHIST_List.Clear();
                Get_History();
                Get_Field_History(Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_ID"].Value.ToString());
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Add_Edit_Trns_Form_Close(object sender, FormClosedEventArgs e)
        {
            CASE0010_CASE_Form form = sender as CASE0010_CASE_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                switch (Oper_Performed)
                {
                    //case "Add_FUND": Get_App_in_All_Funds(pass_enroll_entity.ID); break;

                    case "Denied": break;

                    case "Add_FUND":
                    case "Edit_FUND":
                    case "Transfer":
                        Get_App_in_All_Funds(Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString().Trim(), string.Empty);
                        this.DialogResult = DialogResult.OK;

                        break;
                        //Get_Field_History(Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString().Trim()); break;
                }
            }
        }


        private string[] Get_Sel_Prog_App_To_Add_Fund()
        {
            string[] Class_Dates = new string[2];
            Pass_Enroll_List.Clear();
            if (HSS_Site_List.Count == 0)
                Get_HSS_Sites();

            if (Module_Code == "02")
            {
                foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
                {
                    if (Entity.ID.Trim() == Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString())
                    {
                        Pass_Enroll_List.Add(new CaseEnrlEntity(Entity));

                        foreach (CaseSiteEntity Ent in HSS_Site_List)
                        {
                            if (Ent.SiteNUMBER == Entity.Site && Ent.SiteROOM.Trim() == Entity.Room && Ent.SiteAM_PM.Trim() == Entity.AMPM)
                            {
                                Class_Dates[0] = Ent.SiteCLASS_START;
                                Class_Dates[1] = Ent.SiteCLASS_END;
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            return Class_Dates;
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

            //foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
            //{
            //    if (Entity.ID == Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString())
            //    {
            //        foreach (CaseSiteEntity Ent in HSS_Site_List)
            //        {
            //            if (Ent.SiteNUMBER == Entity.Site && Ent.SiteROOM.Trim() == Entity.Room && Ent.SiteAM_PM.Trim() == Entity.AMPM)
            //            {
            //                Class_Start_Date = Ent.SiteCLASS_START;
            //                Class_End_Date = Ent.SiteCLASS_END;
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        private void Btn_Edit_Stat_Hist_Click(object sender, EventArgs e)
        {
            //CASE0010_CASE_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, CaseEnrlEntity pass_Enroll_Entity)
            CaseEnrlEntity test = new CaseEnrlEntity();

            foreach (CaseEnrlEntity Entity in App_Mult_Fund_List)
            {
                if (Entity.ID == Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString())
                {
                    test = Entity;
                    break;
                }
            }

            CASE0010_CASE_Form Edit_Status_Hist_Form = new CASE0010_CASE_Form(BaseForm, Privileges, Module_Code, test);
            Edit_Status_Hist_Form.FormClosed += new FormClosedEventHandler(Edit_Status_Hist_Closed);
            Edit_Status_Hist_Form.StartPosition = FormStartPosition.CenterScreen;
            Edit_Status_Hist_Form.ShowDialog();
        }


        private void Edit_Status_Hist_Closed(object sender, FormClosedEventArgs e)
        {
            CASE0010_CASE_Form form = sender as CASE0010_CASE_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                ENRLHIST_List.Clear();
                Get_History();
                Get_Field_History(Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_ID"].Value.ToString());
            }
        }



        private void Rb_Fund_Edit_Click(object sender, EventArgs e)
        {
            Fill_History_Grid();
        }

        private void History_Panel_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void Rb_Fund_Edit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Fld_Hist_Grid_New_Click(object sender, EventArgs e)
        {

        }

        private void Site_Panel_Click(object sender, EventArgs e)
        {

        }

        private void Cb_Other_Room_CheckedChanged(object sender, EventArgs e)
        {

        }

        string StatusChange_Date = string.Empty;
        private void Enroll_Date_ValueChanged(object sender, EventArgs e)
        {
            StatusChange_Date = Enroll_Date.Text;
            Status_Dates_Calculated = Btn_Save.Visible = false;
            Btn_Calu_Dates.Visible = true;
        }

        private void Calculate_Status_Dates_For_Status_Change()
        {
            if (Enroll_Date.Checked)
            {
                string Tmp_Date = string.Empty, Tmp_Attn_Date = string.Empty;
                foreach (DataGridViewRow dr in Grid_Applications.Rows)
                {
                    if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                    {
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.ID == dr.Cells["GD_ID"].Value.ToString())
                            {
                                Tmp_Date = Entity.Status_Date.Trim();
                                if (string.IsNullOrEmpty(Tmp_Date))
                                    Tmp_Date = "01/01/1900";

                                Tmp_Attn_Date = dr.Cells["GD_Attn_Date"].Value.ToString();
                                if (string.IsNullOrEmpty(Tmp_Attn_Date.Trim()))
                                    Tmp_Attn_Date = "01/01/1900";

                                if (Convert.ToDateTime(Convert.ToDateTime(Tmp_Date).ToShortDateString()) > Convert.ToDateTime(Enroll_Date.Value.ToShortDateString()))
                                {
                                    if (Convert.ToDateTime(Convert.ToDateTime(Tmp_Date).ToShortDateString()) > Convert.ToDateTime(Tmp_Attn_Date))
                                        dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Date).ToShortDateString();
                                    else
                                        dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Attn_Date);
                                    //dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Date).ToShortDateString();
                                }
                                else
                                {
                                    if (Convert.ToDateTime(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString()) > Convert.ToDateTime(Tmp_Attn_Date))
                                        dr.Cells["GD_Date"].Value = Convert.ToDateTime(Enroll_Date.Value).ToShortDateString();
                                    else
                                        dr.Cells["GD_Date"].Value = Convert.ToDateTime(Tmp_Attn_Date);
                                    // dr.Cells["GD_Date"].Value = Enroll_Date.Value.ToShortDateString();
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        bool Status_Dates_Calculated = false;
        private void Btn_Calu_Dates_Click(object sender, EventArgs e)
        {
            bool Soft_Edit_For_Last_Attn_Date = false;
            if (Enroll_Date.Checked)
            {
                _errorProvider.SetError(Enroll_Date, null);
                string Tmp_Date = string.Empty, Tmp_Attn_Date = string.Empty;
                foreach (DataGridViewRow dr in Grid_Applications.Rows)
                {
                    if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                    {
                        foreach (CaseEnrlEntity Entity in Pass_Enroll_List)
                        {
                            if (Entity.ID == dr.Cells["GD_ID"].Value.ToString())
                            {
                                Tmp_Date = Entity.Status_Date.Trim();
                                if (string.IsNullOrEmpty(Tmp_Date))
                                    Tmp_Date = "01/01/1900";

                                Tmp_Attn_Date = dr.Cells["GD_Attn_Date"].Value.ToString();
                                if (string.IsNullOrEmpty(Tmp_Attn_Date.Trim()))
                                    Tmp_Attn_Date = "01/01/1900";

                                if (Tmp_Attn_Date != "01/01/1900")
                                {
                                    if (Convert.ToDateTime(Tmp_Attn_Date) >= Convert.ToDateTime(Enroll_Date.Value))
                                    {
                                        dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                        dr.Cells["GD_Date"].Style.ForeColor = Color.Red;
                                        if (Pass_Enroll_List.Count == 1)
                                        {
                                            Soft_Edit_For_Last_Attn_Date = true;
                                            //if (Soft_Edit_For_Last_Attn_Date)
                                            //{
                                            //    MessageBox.Show("Attendance posted after your From date do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, SoftEditfor_AttnLast_Date, true);
                                            //}

                                        }
                                    }
                                    else if (Convert.ToDateTime(Tmp_Date) > Convert.ToDateTime(Enroll_Date.Value.ToShortDateString()))
                                    {
                                        if (Convert.ToDateTime(Tmp_Date) > Convert.ToDateTime(Tmp_Attn_Date))
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                        else
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                        dr.Cells["GD_Date"].Style.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(Enroll_Date.Value) > Convert.ToDateTime(Tmp_Attn_Date))
                                        {
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                            dr.Cells["GD_Date"].Style.ForeColor = Color.Green;
                                        }
                                    }
                                }
                                else
                                {


                                    if (Convert.ToDateTime(Tmp_Date) > Convert.ToDateTime(Enroll_Date.Value.ToShortDateString()))
                                    {
                                        if (Convert.ToDateTime(Tmp_Date) > Convert.ToDateTime(Tmp_Attn_Date))
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                        else
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                        dr.Cells["GD_Date"].Style.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(Enroll_Date.Value) > Convert.ToDateTime(Tmp_Attn_Date))
                                        {
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                            dr.Cells["GD_Date"].Style.ForeColor = Color.Green;
                                        }
                                        else
                                        {
                                            dr.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_Attn_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                            dr.Cells["GD_Date"].Style.ForeColor = Color.Red;
                                            if (Pass_Enroll_List.Count == 1)
                                            {
                                                Soft_Edit_For_Last_Attn_Date = true;
                                                //if (Soft_Edit_For_Last_Attn_Date)
                                                //{
                                                //    MessageBox.Show("Attendance posted after your From date do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, SoftEditfor_AttnLast_Date, true);
                                                //}

                                            }
                                        }
                                        // dr.Cells["GD_Date"].Value = Enroll_Date.Value.ToShortDateString();
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                Btn_Calu_Dates.Visible = false;
                Status_Dates_Calculated = Btn_Save.Visible = true;

                if (Soft_Edit_For_Last_Attn_Date)
                {
                    MessageBox.Show("Attendance posted after your From date, do you want to continue?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: SoftEditfor_AttnLast_Date);
                }

            }
            else
                _errorProvider.SetError(Enroll_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Enroll Date".Replace(Consts.Common.Colon, string.Empty)));

            //if (Soft_Edit_For_Last_Attn_Date)
            //{
            //    MessageBox.Show("Attendance posted after your From date do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, SoftEditfor_AttnLast_Date, true);
            //}

        }

        private void SoftEditfor_AttnLast_Date(DialogResult dialogResult)
        {
            // MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                //Grid_Applications.CurrentRow.Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                Grid_Applications.Rows[0].Cells["GD_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Enroll_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                Btn_Calu_Dates.Visible = false;
                Status_Dates_Calculated = Btn_Save.Visible = true;
            }
        }


        private void Cmb_Reason_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void CASE0010_StatusChange_Form_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Click_1(object sender, EventArgs e)
        {

        }

        private void Grid_Applications_Click(object sender, EventArgs e)
        {

        }

        private void Txt_Desc1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Enroll_Date_Click(object sender, EventArgs e)
        {

        }

        private void Txt_Desc2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Reason_Panel_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblCostReq_Click(object sender, EventArgs e)
        {

        }

        private void Cb_Enroll_Withdraw_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_Withdraw_Enroll.Checked)
            {
                Withdraw_Site_Panel.Visible = true;
                if (Pass_Enroll_List.Count > 0)
                {
                    Txt_DrawEnroll_Site.Text = Pass_Enroll_List[0].Site;
                    Txt_DrawEnroll_Room.Text = Pass_Enroll_List[0].Room;
                    Txt_DrawEnroll_AMPM.Text = Pass_Enroll_List[0].AMPM;
                }
            }
            else
            {
                Txt_DrawEnroll_Site.Clear();
                Txt_DrawEnroll_Room.Clear();
                Txt_DrawEnroll_AMPM.Clear();
                Withdraw_Site_Panel.Visible = false;
            }
        }

        private void Pb_Withdraw_Enroll_Click(object sender, EventArgs e)
        {
            Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges);
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
            }
        }

        private void Fill_WithdrawEnroll_Fund_Combo()
        {
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            FundingList = _model.SPAdminData.GetLookUpFromAGYTAB_EXT(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");


            Cmb_DrawEnroll_Fund.Items.Clear();
            Cmb_DrawEnroll_Fund.Items.Add(new ListItem(" ", "00", " ", (true ? Color.Green : Color.Red)));
            foreach (SPCommonEntity Entity in FundingList)
            {
                if (!string.IsNullOrEmpty(Entity.Ext.Trim()))
                    Cmb_DrawEnroll_Fund.Items.Add(new ListItem(Entity.Desc, Entity.Code, Entity.Ext, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
            }

            if (Cmb_DrawEnroll_Fund.Items.Count > 0)
                Cmb_DrawEnroll_Fund.SelectedIndex = 0;
        }

        private void panel4_Click(object sender, EventArgs e)
        {

        }

        private void Pb_StatChange_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Enrollment");
        }

        private void Btn_Save_Inc_Docs_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public string[] Get_Inc_Doc_Vales()
        {
            string[] Inc_Values = new string[4];
            Inc_Values[0] = Inc_Values[1] = Inc_Values[2] = Inc_Values[3] = "N";

            if (Cb_Assests.Checked)
                Inc_Values[0] = "Y";

            if (Cb_Social.Checked)
                Inc_Values[1] = "Y";

            if (Cb_Rent.Checked)
                Inc_Values[2] = "Y";

            if (Cb_Utility.Checked)
                Inc_Values[3] = "Y";

            return Inc_Values;
        }

        private void Pb_Hist_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Enroll History");
        }

        private void Act_Status_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 12) && e.RowIndex != -1)
            {
                CASE0010_CM_Form addForm = new CASE0010_CM_Form(Privileges.Program, Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["Enrl_Note1"].Value.ToString(),
                                                                Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["Enrl_Note2"].Value.ToString());
                addForm.StartPosition = FormStartPosition.CenterScreen;
                addForm.ShowDialog();
            }
        }

        private void Btn_Edit_Curr_Status_Date_Click(object sender, EventArgs e)
        {
            CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(Act_Status_Grid.CurrentRow.Cells["ActHist_From"].Value.ToString().Trim(), CM_Last_Hist_FDate, "Enrl");
            Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_New_Status_Date);
            Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
            Mem_Sel_Form.ShowDialog();
        }

        private void get_New_Status_Date(object sender, FormClosedEventArgs e)
        {
            string SelRef_Name = null;

            CASE0006_CAMSForm form = sender as CASE0006_CAMSForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string New_Post_Date = "";
                New_Post_Date = form.Get_Posting_Date();

                if (!string.IsNullOrEmpty(New_Post_Date.Trim()))
                {
                    New_Post_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(New_Post_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    Update_New_StatusDate_in_Enrl(New_Post_Date);
                }
                else
                {
                }

            }
        }

        private void Update_New_StatusDate_in_Enrl(string New_Date)
        {
            bool Save_Result = true;
            {

                {//09092015
                    string Sel_Enrl_ID = Act_Status_Grid.CurrentRow.Cells["ActHist_ID"].Value.ToString().Trim();
                    if (_model.EnrollData.UpdateCASEENRL_Status_Date(Sel_Enrl_ID, New_Date, BaseForm.UserID, out Sql_SP_Result_Message))
                    {
                        Get_App_in_All_Funds(Sel_Enrl_ID, string.Empty);
                        ENRLHIST_List.Clear();
                        Get_History();
                        Get_Field_History(Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_ID"].Value.ToString());
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                        MessageBox.Show(Sql_SP_Result_Message, "CAP Systems");
                }

            }
        }

        private void Btn_Bus_Click(object sender, EventArgs e)
        {
            HSS00140_App_Bus_Form Client_Form_Add = new HSS00140_App_Bus_Form(BaseForm, Pass_Enroll_Entity.App);
            Client_Form_Add.StartPosition = FormStartPosition.CenterScreen;
            Client_Form_Add.ShowDialog();
        }

        void EditStatusTimelinebutton()
        {
            if (Act_Status_Grid.Rows.Count > 0)
            {
                if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                {
                    if (Act_Status_Grid.Rows[Act_Status_Grid.CurrentRow.Index].Cells["ActHist_Site"].Value.ToString().Contains("****"))
                    {
                        if (History_Grid.Rows.Count == 0)
                            Btn_Edit_Stat_Hist.Visible = false;
                    }
                    else
                    {
                        Btn_Edit_Stat_Hist.Visible = true;
                    }

                }
            }

        }

        private void CASE0010_StatusChange_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "Pb_StatChange_Help")
            {
            }
            if (e.Tool.Name == "Pb_Hist_Help")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
            
        }

        string Dep_Program_Year = string.Empty;
        private void FillYearCombo()
        {
            CmbYear.Visible = false;

            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                CmbYear.SelectedIndexChanged -= CmbYear_SelectedIndexChanged;

                DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string DepYear = string.Empty;
                    DataTable dt = ds.Tables[0];
                    int YearIndex = 0;

                    if (dt.Rows.Count > 0)
                    {
                        Dep_Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                        if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                        {
                            int TmpYear = int.Parse(DepYear);
                            string TmpYearStr = null;

                            List<ListItem> listItem = new List<ListItem>();
                            for (int i = 0; i < 10; i++)
                            {
                                TmpYearStr = (TmpYear - i).ToString();
                                listItem.Add(new ListItem(TmpYearStr, i));
                                if (TmpYearStr == BaseForm.BaseYear.ToString())
                                    YearIndex = i;
                            }

                            CmbYear.Items.AddRange(listItem.ToArray());

                            CmbYear.Visible = true;

                            if (Dep_Program_Year == BaseForm.BaseYear.ToString())
                                CmbYear.SelectedIndex = 0;
                            else
                                CmbYear.SelectedIndex = YearIndex;
                        }
                    }
                }


                CmbYear.SelectedIndexChanged += CmbYear_SelectedIndexChanged;
            }
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CmbYear.Items.Count > 0)
                {

                    Enrl_ID = string.Empty;
                    Pass_Enroll_List = new List<CaseEnrlEntity>();
                    _model = new CaptainModel();

                    Fld_Hist_Grid_New.Rows.Clear();
                    Stf_Grid.Rows.Clear();
                    lblpreviousMsg.Visible = false;


                    caseMstList = _model.CaseMstData.GetCaseMstadpyn(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, ((ListItem)CmbYear.SelectedItem).Text.ToString(), Pass_Enroll_Entity.App);
                    if (Module_Code != "02" && Module_Code != "03")
                    {
                        List<CaseEnrlEntity> Mult_Fund_List = Ge_Related_Module_0n_App_ID(string.Empty, ((ListItem)CmbYear.SelectedItem).Text.ToString());
                        if (Mult_Fund_List.Count > 0)
                        {
                            if (Mult_Fund_List[0].Rec_Type == "H")
                                Module_Code = "02";
                            else
                                Module_Code = "03";
                        }
                    }


                    Lbl_Teacher.Text = "   ";
                    if (Module_Code == "02")
                    {
                        Get_App_in_All_Funds(string.Empty, ((ListItem)CmbYear.SelectedItem).Text.ToString());
                        Fill_HS_Ranks(caseMstList);

                        //if (BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B")
                        //    Btn_Edit_Stat_Hist.Visible = true;
                    }
                    else
                    {
                        Lbl_Current_Status.Text = "Current Status";
                        this.Hist_Fund_Hie.Visible = true; this.Hist_Fund_Hie.ShowInVisibilityMenu = true;
                        this.Hist_Operator.Width = 95;
                        Get_App_in_All_Funds(string.Empty, ((ListItem)CmbYear.SelectedItem).Text.ToString());
                        Rb_Fund_Edit.Visible = Rb_Status_Edit.Visible = Rb_Hist_All.Visible = label11.Visible = false;

                        Btn_Bus.Visible = Cb_Show_Entire_Status.Visible = false;
                        ActHist_Site.Visible = ActHist_Attn_SDate.Visible = ActHist_Attn_LDate.Visible = false;
                        ActHist_Fund.HeaderText = "Hierarchy";
                        ActHist_Fund.Width = 250;
                    }

                    Get_History();
                    if (((ListItem)CmbYear.SelectedItem).Text.ToString() == Dep_Program_Year.Trim())
                    {
                        Act_Status_Grid.Enabled = true;
                        //History_Grid.Enabled = true;
                        Btn_Bus.Enabled = true;
                        Cb_Show_Entire_Status.Enabled = true;
                        Btn_Edit_Stat_Hist.Enabled = true;
                    }
                    else
                    {
                        Act_Status_Grid.Enabled = false;
                        Btn_Bus.Enabled = false;
                        Cb_Show_Entire_Status.Enabled = false;
                        Btn_Edit_Stat_Hist.Enabled = false;
                    }

                }

            }
            catch (Exception ex)
            {


            }
        }
    }
}
