/************************************************************************
 * Conversion On        :   11/28/2022
 * Converted By         :   Kranthi
 * Last Modification On :   11/28/2022
 * **********************************************************************/
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
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE3006_CAMSForm : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public CASE3006_CAMSForm(BaseForm baseform, string CAMS_flg, string CAMS_desc, string hierarchy, string year, CASEACTEntity pass_entity, PrivilegeEntity privileges, string MST_site, string MST_intakeWorker, List<FldcntlHieEntity> caFldcEntity, CASESP0Entity sp_header_rec, List<CASEACTEntity> CA_template_list, string sp_start_Date, string spm_site, string sp_End_Date)
        {
            InitializeComponent();
            BaseForm = baseform;
            CAMS_FLG = CAMS_flg;
            Hierarchy = hierarchy;
            Year = year;
            CAMS_Desc = CAMS_desc;
            Pass_CA_Entity = pass_entity;
            SP_Header_Rec = sp_header_rec;
            CA_Template_List = CA_template_list;
            Sp_Start_Date = sp_start_Date;
            Sp_End_Date = sp_End_Date;
            SPM_Site = spm_site;
            //if (Pass_CA_Entity.Rec_Type == "I" && CA_template_list.Count > 0)
            //    Pass_CA_Entity = CA_template_list[0];

            Privileges = privileges;
            MST_Site = MST_site;
            MST_Intakeworker = MST_intakeWorker;
            CntlCAEntity = caFldcEntity;

            if (CAMS_desc.Length > 60)
                Lbl_CA_Header.Text = CAMS_desc.Substring(0, 60);
            else
                Lbl_CA_Header.Text = CAMS_desc.Trim();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            BaseAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (BaseAgencyControlDetails.CAOBO.Trim() == "Y")
            {
                lblCABenefit.Visible = true;
                cmb_CA_Benefit.Visible = true;
                btnCAOBO.Visible = true;
                ACR_BenefitFrom = BaseAgencyControlDetails.BenefitFrom.Trim();
                FillApplicantData();
            }
            else
            {
                lblCABenefit.Visible = false;
                cmb_CA_Benefit.Visible = false;
                btnCAOBO.Visible = false;
            }


            Mode = "Add";
            if (Pass_CA_Entity.Rec_Type == "U")
            {
                this.Text = privileges.Program + " Activity Posting - Edit ";
                //Act_Date.Enabled = false;
                Mode = "Edit";
                Get_PROG_Notes_Status();
                Get_Vendor_List();
                GetAgencyDetails();
                //if (dtAgency.Rows.Count > 0)
                //{
                //    if(dtAgency.Rows[0]["ACR_CA_PVOUCHER"].ToString().Trim()=="Y")
                //        pbPdf.Visible = true;
                //}
            }
            else
            {
                this.Text = privileges.Program + " Activity Posting - Add " + (CA_Template_List.Count > 0 ? "(Template)" : "");
                Pb_CA_Notes.Visible = false; pbPdf.Visible = false;

                ACR_BenefitFrom = BaseAgencyControlDetails.BenefitFrom.Trim();
            }

            Fill_DropDowns();
            Get_ReferrTo_Data();

            Fill_Custom_Questions();
            //Fill_CA_Members_DropDown();
            Fill_CA_Benefiting_From();
            Fill_SP_CAMS_Details(SP_Header_Rec.Code.Trim(), pass_entity.Branch, null);

            switch (CAMS_FLG)
            {
                //case "MS": CAPanel.Visible = false;
                //    this.MSPanel.Location = new System.Drawing.Point(2, 2);
                //    this.Size = new System.Drawing.Size(507, 387);
                //    break;

                case "CA":
                    this.Size = new System.Drawing.Size(605, 599);
                    //this.Size = new System.Drawing.Size(605, 420); //405//602, 350
                    CAPanel.Visible = panel2.Visible = true;
                    //LblProgramReq.Visible = Cmb_Program.Enabled = true;
                    Fill_Program_Combo();
                    if (Pass_CA_Entity.Rec_Type == "U")
                        Fill_CA_Controls();
                    else if (CA_template_list.Count > 0)
                        Fill_CA_Controls_4rm_Template();
                    break;

            }
            EnableDisableControls();
            Txt_Cost.Validator = TextBoxValidation.FloatValidator;
            Txtx_ChkNo.Validator = TextBoxValidation.IntegerValidator;
            Txt_Units.Validator = TextBoxValidation.IntegerValidator;
            propReportPath = _model.lookupDataAccess.GetReportPath();
        }

        public CASE3006_CAMSForm(BaseForm baseform, string CAMS_flg, string CAMS_desc, string hierarchy, string year, CASEMSEntity pass_entity, PrivilegeEntity privileges, string MST_site, string MST_intakeWorker, List<FldcntlHieEntity> msFldcEntity, CASESP0Entity sp_header_rec, List<CASEMSEntity> MS_template_list, string sp_start_Date, string spm_site, string sp_End_Date)
        {
            InitializeComponent();
            BaseForm = baseform;
            CAMS_FLG = CAMS_flg;
            Hierarchy = hierarchy;
            Year = year;
            CAMS_Desc = CAMS_desc;
            Pass_MS_Entity = pass_entity;
            CntlMSEntity = msFldcEntity;
            Privileges = privileges;
            SP_Header_Rec = sp_header_rec;
            MS_Template_List = MS_template_list;
            Sp_Start_Date = sp_start_Date;
            Sp_End_Date = sp_End_Date;
            SPM_Site = spm_site;

            MST_Site = MST_site.Trim();
            MST_Intakeworker = MST_intakeWorker.Trim();


            if (CAMS_desc.Length > 55)
                Lbl_MS_Header.Text = CAMS_desc.Substring(0, 55);
            else
                Lbl_MS_Header.Text = CAMS_desc.Trim();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Mode = "Add";
            if (Pass_MS_Entity.Rec_Type == "U")
            {
                this.Text = privileges.Program + " Milestone Posting - Edit ";
                //MS_Date.Enabled = false;
                Mode = "Edit";
            }
            else
            {
                this.Text = privileges.Program + " Milestone Posting - Add " + (MS_Template_List.Count > 0 ? "(Template)" : "");
                Pb_MS_Notes.Visible = false;
                ACR_BenefitFrom = BaseAgencyControlDetails.BenefitFrom.Trim();
            }


            Fill_DropDowns();
            Fill_Members_DropDown();
            Fill_Benefiting_From();
            Get_PROG_Notes_Status();
            MS_Post_Dates_List.Clear();
            Fill_SP_CAMS_Details(pass_entity.Service_plan, pass_entity.Branch, null);

            switch (CAMS_FLG)
            {
                case "MS":
                    CAPanel.Visible = false;
                    this.panel4.Location = new System.Drawing.Point(1, 0);
                    this.MSPanel.Location = new System.Drawing.Point(2, 45);
                    //this.Size = new System.Drawing.Size(507, 376);
                    //this.Size = new System.Drawing.Size(507, 377); 
                    this.Size = new System.Drawing.Size(507, 530);
                    MSPanel.Visible = panel4.Visible = true;
                    Fill_MS_Program_Combo();
                    if (Pass_MS_Entity.Rec_Type == "U")
                        Fill_MS_Controls();
                    else if (MS_Template_List.Count > 0)
                        Fill_MS_Controls_4rm_Template(); // Template Controls

                    break;
            }
            EnableDisableControls();
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(MS_Help, "Help");
        }

        public CASE3006_CAMSForm(BaseForm baseform)
        {
            InitializeComponent();
            BaseForm = baseform;
            CAMS_FLG = "Bulk_Posting";
            this.Text = "Service Activity Bulk Posting";
            _model = new CaptainModel();

            Bulk_Posting_Panel.Location = new System.Drawing.Point(2, 2);
            this.Size = new System.Drawing.Size(620, 379);
            Bulk_Posting_Panel.Visible = true;

            Fill_Sites();
            Fill_Case_Type_Combo();
            Cmb_Bulk_SortBy.Items.Insert(0, new Captain.Common.Utilities.ListItem("App#", "0"));
            Cmb_Bulk_SortBy.Items.Insert(1, new Captain.Common.Utilities.ListItem("Client Name", "1"));
            Cmb_Bulk_SortBy.SelectedIndex = 0;
            Fill_Bulk_App_Grid_To_Post();
        }


        string OBF_Hie = "", OBF_App = "", OBF_Template_SW = "N"; int OBF_MS_Cnt = 0;
        List<Captain.Common.Utilities.ListItem> OBF_Type3_Sel_Members = new List<Captain.Common.Utilities.ListItem>();
        public CASE3006_CAMSForm(BaseForm baseform, string hie, string app, int MS_Cnt, List<Captain.Common.Utilities.ListItem> Sel_Members)//, string template_sw)
        {
            InitializeComponent();
            BaseForm = baseform;
            OBF_Hie = hie; OBF_App = app;
            OBF_MS_Cnt = MS_Cnt;
            OBF_Type3_Sel_Members = Sel_Members;
            //OBF_Template_SW = template_sw;

            this.Text = "Household Member(s) Selection";
            CAMS_FLG = "Bulk_Posting";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Sel_HH_Mem_Panel.Location = new System.Drawing.Point(2, 2);
            this.Size = new System.Drawing.Size(482, 266);
            Sel_HH_Mem_Panel.Visible = true;

            Fill_Members_For_BulkPost();

            //this.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedToolWindow;
        }

        string Exp_Post_Date = "", Calling_Form = "Posting";
        public CASE3006_CAMSForm(string date, string Exp_post_date, string calling_form)
        {
            InitializeComponent();

            this.Text = "Date Selection";
            CAMS_FLG = "Bulk_Posting";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            Exp_Post_Date = Exp_post_date;
            Calling_Form = "Posting";
            if (!string.IsNullOrEmpty(calling_form.Trim()))
                Calling_Form = calling_form;

            Post_Date_Panel.Location = new System.Drawing.Point(2, 2);
            this.Size = new System.Drawing.Size(150, 32);
            Post_Date_Panel.Visible = true;
            if (!string.IsNullOrEmpty(date.Trim()))
            {
                Post_Date.Value = Convert.ToDateTime(date);
                Post_Date.Checked = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(Exp_post_date.Trim()))
                {
                    Post_Date.Value = Convert.ToDateTime(Exp_post_date);
                    Post_Date.Checked = true;
                }
            }
            // if (Calling_Form != "Enrl")
            //   this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
        }

        string App_Ben_Type = "", App_Ben_level = "";
        public CASE3006_CAMSForm(bool age_sw, string lpb_type, string lpb_level, string source, string Check_date, string Check_Fdate, string Check_Tdate, string ben_type, string ben_level)
        {
            InitializeComponent();

            this.Text = "Fuel Triggers";
            CAMS_FLG = "Bulk_Posting";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            CT_Post_Triggers.Location = new System.Drawing.Point(2, 2);
            this.Size = new System.Drawing.Size(374, 188);
            CT_Post_Triggers.Visible = true;

            if (age_sw)
                Cb_Age.Checked = true;

            if (lpb_type.Contains("B1"))
                Cb_B1.Checked = true;
            if (lpb_type.Contains("U1"))
                Cb_U1.Checked = true;
            if (lpb_type.Contains("R1"))
                Cb_R1.Checked = true;

            if (lpb_level.Contains("1"))
                Cb_L1.Checked = true;
            if (lpb_level.Contains("2"))
                Cb_L2.Checked = true;
            if (lpb_level.Contains("3"))
                Cb_L3.Checked = true;
            if (lpb_level.Contains("4"))
                Cb_L4.Checked = true;
            if (lpb_level.Contains("5"))
                Cb_L5.Checked = true;

            CT_Ckeck_FDate.Value = CT_Ckeck_TDate.Value = DateTime.Today;
            if (!string.IsNullOrEmpty(Check_Fdate.Trim()))
            {
                CT_Ckeck_FDate.Value = Convert.ToDateTime(Check_Fdate.Trim());
                //Check_Fdate.Checked = true;
            }

            if (!string.IsNullOrEmpty(Check_Tdate.Trim()))
            {
                CT_Ckeck_TDate.Value = Convert.ToDateTime(Check_Tdate.Trim());
                //Check_Fdate.Checked = true;
            }

            if (!string.IsNullOrEmpty(Check_date.Trim()))
            {
                Cb_Chk_Date.Checked = true;
                Check_Dates_Panel.Visible = true;
            }

            App_Ben_Type = ben_type; App_Ben_level = ben_level;

            Fill_Primary_Source_Combo(source);

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Lbl_Ben_Type_Error, "Template Applicant Benefit type " + App_Ben_Type + " differs with selected");
            tooltip.SetToolTip(Lbl_Ben_level_Error, "Template Applicant Benefit Level " + App_Ben_level + " differs with selected");
            Check_Base_Ben_Type();
            Check_Base_Ben_Level();
            //this.FormBorderStyle = Wisej.Web.FormBorderStyle.None;
        }

        public CASE3006_CAMSForm(bool tmp, string Attn_SW, string from_Date, string to_Date)
        {
            InitializeComponent();

            this.Text = "HS Triggers";
            CAMS_FLG = "Bulk_Posting";
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            HS_Post_Triggers.Location = new System.Drawing.Point(2, 2);
            this.Size = new System.Drawing.Size(196, 129);
            HS_Post_Triggers.Visible = true;

            Rb_1Day.Checked = true;
            if (Attn_SW == "2")
                Rb_2Days.Checked = true;

            Attn_FDate.Checked = false;
            if (!string.IsNullOrEmpty(from_Date.Trim()))
            {
                Attn_FDate.Value = Convert.ToDateTime(from_Date.Trim());
                Attn_FDate.Checked = true;
            }

            Attn_TDate.Checked = false;
            if (!string.IsNullOrEmpty(to_Date.Trim()))
            {
                Attn_TDate.Value = Convert.ToDateTime(to_Date.Trim());
                Attn_TDate.Checked = true;
            }
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public List<FldcntlHieEntity> CntlCAEntity { get; set; }

        public List<FldcntlHieEntity> CntlMSEntity { get; set; }

        public AgencyControlEntity BaseAgencyControlDetails { get; set; }

        public string CAMS_FLG { get; set; }

        public DataSet dsAgency { get; set; }

        public DataTable dtAgency { get; set; }

        public string propReportPath { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string Year { get; set; }

        public string MST_Site { get; set; }

        public string Mode { get; set; }

        public string SPM_Site { get; set; }

        public string MST_Intakeworker { get; set; }

        public string Sp_Start_Date { get; set; }

        public string Sp_End_Date { get; set; }

        public string ACR_BenefitFrom { get; set; }

        public CASEACTEntity Pass_CA_Entity { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public CASESP0Entity SP_Header_Rec { get; set; }

        public List<CASEACTEntity> CA_Template_List { get; set; }

        public List<CASEMSEntity> MS_Template_List { get; set; }

        public List<CAOBOEntity> CA_OBO_List { get; set; }

        #endregion

        private void Get_App_CASEACT_List()
        {
            CASEACTEntity Search_Enty = new CASEACTEntity(true);
            Search_Enty.Agency = Hierarchy.Substring(0, 2);
            Search_Enty.Dept = Hierarchy.Substring(2, 2);
            Search_Enty.Program = Hierarchy.Substring(4, 2);
            Search_Enty.Year = Pass_CA_Entity.Year;                             // Year will be always Four-Spaces in CASEACT
            Search_Enty.App_no = BaseForm.BaseApplicationNo;
            Search_Enty.SPM_Seq = Pass_CA_Entity.SPM_Seq;
            Search_Enty.Service_plan = Pass_CA_Entity.Service_plan;

            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");

            SP_Activity_Details = SP_Activity_Details.OrderByDescending(u => Convert.ToDateTime(u.ACT_Date.Trim())).ToList();

        }

        private void Get_App_CASEMS_List()
        {
            CASEMSEntity Search_Enty = new CASEMSEntity(true);
            Search_Enty.Agency = Hierarchy.Substring(0, 2);
            Search_Enty.Dept = Hierarchy.Substring(2, 2);
            Search_Enty.Program = Hierarchy.Substring(4, 2);
            Search_Enty.Year = Pass_CA_Entity.Year; ;                              // Year will be always Four-Spaces in CASEMS
            Search_Enty.App_no = BaseForm.BaseApplicationNo;
            Search_Enty.SPM_Seq = Pass_CA_Entity.SPM_Seq;
            Search_Enty.Service_plan = Pass_CA_Entity.Service_plan;

            SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_Enty, "Browse");
            SP_MS_Details = SP_MS_Details.OrderByDescending(u => Convert.ToDateTime(u.Date.Trim())).ToList();
        }

        string Sql_SP_Result_Message = string.Empty;

        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        List<CASEACTEntity> SP_Activity_Details = new List<CASEACTEntity>();
        List<CASEMSEntity> SP_MS_Details = new List<CASEMSEntity>();
        private void Fill_SP_CAMS_Details(string sp_Code, string Branch_Code, string Sel_CAMS_Key)
        {
            if (Branch_Code != "9")
            {
                SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(sp_Code, Branch_Code, null, null, "CASE4006");
                SP_CAMS_Details = SP_CAMS_Details.OrderBy(u => u.Row).ToList();
                if (SP_CAMS_Details.Count > 0)
                {
                    int rowIndex = 0, Sel_CAMS_Index = 0;
                    string CAMS_DESC = null;
                    foreach (CASESP2Entity Entity in SP_CAMS_Details)
                    {
                        if (Entity.Type1 == "CA" && CAMS_FLG == "CA")
                        {
                            if (Pass_CA_Entity.ACT_Code.Trim() != Entity.CamCd.Trim())
                            {
                                if (Entity.SP2_CAMS_Active == "A" && Entity.CAMS_Active.Equals("True"))
                                    rowIndex = SP_CA_Grid.Rows.Add(false, Entity.Row, Entity.CamCd.Trim(), Entity.CAMS_Desc.Trim(), Entity.Type1, "C", Entity.Branch, Entity.Orig_Grp, "N", null, null, Entity.CAMS_Desc, Pass_CA_Entity.Year, Entity.CAMS_Active, null, Entity.Curr_Grp, Entity.Branch, null);
                            }
                            //foreach (CASEACTEntity ActEnt in SP_Activity_Details)
                            //{
                            //    if (ActEnt.Service_plan == Entity.ServPlan &&
                            //            ActEnt.Branch == Entity.Branch &&
                            //            ActEnt.Group == Entity.Orig_Grp.ToString() &&
                            //           ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim() &&
                            //           ActEnt.SPM_Seq.Trim() == Pass_CA_Entity.SPM_Seq)
                            //    {
                            //        rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, string.Empty, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, CAMS_Active_Status, ActEnt.ACT_ID, Entity.Curr_Grp, Entity.Branch, CA_Template_SW);
                            //    }
                            //}
                        }
                        else if (Entity.Type1 == "MS" && CAMS_FLG == "MS")
                        {
                            if (Pass_MS_Entity.MS_Code.Trim() != Entity.CamCd.Trim())
                                rowIndex = SP_MS_Grid.Rows.Add(false, Entity.CamCd.Trim(), Entity.CAMS_Desc, Entity.Type1, "C", Entity.Branch, Entity.Orig_Grp, "N", null, null, Entity.CAMS_Desc, Pass_MS_Entity.Year, Entity.CAMS_Active, null, Entity.Curr_Grp, Entity.Branch, null);
                        }
                    }
                }
            }
            else
            {
                Fill_Additional_CAMS_Details(sp_Code);

                if (ADD_CAMA_Details.Count > 0)
                {
                    int rowIndex = 0, Sel_CAMS_Index = 0;
                    string CAMS_DESC = null;
                    foreach (CASESPM2Entity Entity in ADD_CAMA_Details)
                    {
                        if (Entity.Type1 == "CA" && CAMS_FLG == "CA")
                        {
                            if (Pass_CA_Entity.ACT_Code.Trim() != Entity.CamCd.Trim())
                                rowIndex = SP_CA_Grid.Rows.Add(false, Entity.CamCd.Trim(), Entity.CAMS_Desc.Trim(), Entity.Type1, "C", Entity.Branch, Entity.Group, "N", null, null, Entity.CAMS_Desc, Pass_CA_Entity.Year, Entity.CAMS_Active, null, Entity.Curr_Group, Entity.Branch, null);
                            //foreach (CASEACTEntity ActEnt in SP_Activity_Details)
                            //{
                            //    if (ActEnt.Service_plan == Entity.ServPlan &&
                            //            ActEnt.Branch == Entity.Branch &&
                            //            ActEnt.Group == Entity.Orig_Grp.ToString() &&
                            //           ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim() &&
                            //           ActEnt.SPM_Seq.Trim() == Pass_CA_Entity.SPM_Seq)
                            //    {
                            //        rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, string.Empty, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, CAMS_Active_Status, ActEnt.ACT_ID, Entity.Curr_Grp, Entity.Branch, CA_Template_SW);
                            //    }
                            //}
                        }
                        else if (Entity.Type1 == "MS" && CAMS_FLG == "MS")
                        {
                            if (Pass_MS_Entity.MS_Code.Trim() != Entity.CamCd.Trim())
                                rowIndex = SP_MS_Grid.Rows.Add(false, Entity.CamCd.Trim(), Entity.CAMS_Desc, Entity.Type1, "C", Entity.Branch, Entity.Group, "N", null, null, Entity.CAMS_Desc, Pass_MS_Entity.Year, Entity.CAMS_Active, null, Entity.Curr_Group, Entity.Branch, null);
                        }
                    }
                }

            }
        }


        List<CASESPM2Entity> ADD_CAMA_Details = new List<CASESPM2Entity>();
        private void Fill_Additional_CAMS_Details(string Code)
        {
            CASESPM2Entity Search_Entity2 = new CASESPM2Entity();

            Search_Entity2.Agency = Hierarchy.Substring(0, 2);
            Search_Entity2.Dept = Hierarchy.Substring(2, 2);
            Search_Entity2.Prog = Hierarchy.Substring(4, 2);
            //Search_Entity2.Year = BaseYear;
            Search_Entity2.Year = Year;                         // Year will be always Four-Spaces in CASESPM2
            Search_Entity2.App = BaseForm.BaseApplicationNo;
            Search_Entity2.Spm_Seq = Pass_CA_Entity.SPM_Seq;

            Search_Entity2.ServPlan = Search_Entity2.Branch = Search_Entity2.Group = null;
            Search_Entity2.Type1 = Search_Entity2.CamCd = Search_Entity2.Curr_Group = null;
            Search_Entity2.SelOrdinal = Search_Entity2.DateLstc = Search_Entity2.lstcOperator = null;
            Search_Entity2.Dateadd = Search_Entity2.addoperator = null;

            Search_Entity2.ServPlan = Code;


            ADD_CAMA_Details = _model.SPAdminData.Browse_CASESPM2(Search_Entity2, "Browse");

            //if (ADD_CAMA_Details.Count > 0)
            //    Fill_Additional_CAMS_Details_Grid();
        }

        private void Fill_Primary_Source_Combo(string source)
        {
            //List<CommonEntity> Source_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HEATSOURCE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); //_model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
            List<CommonEntity> Source_List = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
            int Sel_Index = 0, Tmp_Cnt = 0;
            Cmb_Heat_Source.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "*"));
            foreach (CommonEntity List in Source_List)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(List.Desc, List.Code);
                Cmb_Heat_Source.Items.Add(li);

                if (source == List.Code.Trim())
                    Sel_Index = Tmp_Cnt;

                Tmp_Cnt++;
            }

            if (Tmp_Cnt > 0)
            {
                if (!string.IsNullOrEmpty(source.Trim()))
                    SetComboBoxValue(Cmb_Heat_Source, source);
                else
                    Cmb_Heat_Source.SelectedIndex = 0;
            }

            //Cmb_Heat_Source.SelectedIndex = Sel_Index;
        }

        private void Fill_DropDowns()
        {
            Fill_Sites();
            Fill_CaseWorker();

            if (CAMS_FLG == "CA")
            {
                Fill_Funding();
                Fill_UOM();
            }
            else
                Fill_Results();
        }

        string Tmp_SPM_Sequence = "1", Sel_CAMS_Program = "";
        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        private void Fill_Program_Combo()
        {
            //Cmb_Program.Items.Clear();
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", BaseForm.BaseAgency, BaseForm.BaseDept, " ");

            //List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
            //CASESPMEntity Search_Entity = new CASESPMEntity(true);

            //if (Pass_CA_Entity.Rec_Type == "I")
            //{
            //    Search_Entity.agency = BaseForm.BaseAgency;
            //    Search_Entity.dept = BaseForm.BaseDept;
            //    Search_Entity.program = BaseForm.BaseProg;
            //    Search_Entity.app_no = BaseForm.BaseApplicationNo;
            //    Search_Entity.service_plan = Pass_CA_Entity.Service_plan;

            //    CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            //}

            //List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("   ", "0"));
            //int TmpRows = 1;
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataTable dt = ds.Tables[0];

            //        string Tmp_Hierarchy = " ";
            //        int ProgIndex = 0; bool DefHieExist = false;
            //        try
            //        {
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                //Tmp_Hierarchy = dr["Agy"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();
            //                //Tmp_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + dr["Prog"].ToString();
            //                if (dt.Columns.Count >= 4)
            //                    Tmp_Hierarchy = dr["Agy"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();
            //                else
            //                    Tmp_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + dr["Prog"].ToString();
            //                listItem.Add(new ListItem(dr["Prog"] + " - " + dr["Name"], Tmp_Hierarchy));

            //                if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
            //                {
            //                    foreach (CASESPMEntity Entity in CASESPM_List)
            //                    {
            //                        if (Entity.Def_Program == Tmp_Hierarchy)
            //                        {
            //                            DefHieExist = true;
            //                            ProgIndex = TmpRows;
            //                            break;
            //                        }
            //                    }

            //                }
            //                //if (DefProg == dr["Prog"].ToString())
            //                //    ProgIndex = TmpRows;

            //                TmpRows++;
            //            }

            //            //if (Pass_CA_Entity.Rec_Type == "I")
            //            //{
            //            //    if (!string.IsNullOrEmpty(SP_Header_Rec.Default_Prog.Trim()) && !SP_Header_Rec.Default_Prog.Contains("**") && SP_Header_Rec.Default_Prog.Length == 6 )
            //            //        SetComboBoxValue(Cmb_Program, SP_Header_Rec.Default_Prog.Substring(4, 2));
            //            //}
            //        }
            //        catch (Exception ex) { }

            //        if (TmpRows > 0)
            //        {
            //            Cmb_Program.Items.AddRange(listItem.ToArray());
            //            //Cmb_Program.Enabled = true;
            //            if (DefHieExist)
            //                Cmb_Program.SelectedIndex = (ProgIndex);// Murali changed..
            //                //Cmb_Program.SelectedIndex = (ProgIndex + 1);
            //            else
            //            {
            //                //if (Cmb_Program.Items.Count == 1)
            //                    Cmb_Program.SelectedIndex = 0;
            //            }
            //        }

            //    }
            //    else
            //        MessageBox.Show("Programs Are Not Defined", "CAP Systems");

            //}

            List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
            CASESPMEntity Search_Entity = new CASESPMEntity(true);

            if (Pass_CA_Entity.Rec_Type == "I")
            {
                Search_Entity.agency = BaseForm.BaseAgency;
                Search_Entity.dept = BaseForm.BaseDept;
                Search_Entity.program = BaseForm.BaseProg;
                Search_Entity.app_no = BaseForm.BaseApplicationNo;
                Search_Entity.service_plan = Pass_CA_Entity.Service_plan;

                CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            }

            string ACR_SERV_Hies = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim();

            int TmpRows = 1;
            SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, Pass_CA_Entity.Service_plan, ACR_SERV_Hies);
            Txt_CA_Program.Clear();
            if (SP_Programs_List.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                {
                    string Tmp_Hierarchy = " ";
                    int ProgIndex = 0; bool DefHieExist = false;
                    try
                    {
                        if (CASESPM_List.Count > 0)
                        {
                            foreach (HierarchyEntity Ent in SP_Programs_List)
                            {
                                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                                if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
                                {
                                    foreach (CASESPMEntity Entity in CASESPM_List)
                                    {
                                        if (Entity.Def_Program == Tmp_Hierarchy)
                                        {
                                            Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                            DefHieExist = true;
                                            ProgIndex = TmpRows;
                                            break;
                                        }
                                    }

                                }
                                TmpRows++;
                            }
                        }
                        else
                        {
                            foreach (HierarchyEntity Ent in SP_Programs_List)
                            {
                                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                                if (Pass_CA_Entity.Rec_Type == "I" && ProgIndex == 0)
                                {
                                    //foreach (CASESPMEntity Entity in CASESPM_List)
                                    //{
                                    if (Pass_CA_Entity.Act_PROG == Tmp_Hierarchy)
                                    {
                                        Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                        DefHieExist = true;
                                        ProgIndex = TmpRows;
                                        break;
                                    }
                                    //}

                                }
                                TmpRows++;
                            }
                        }
                    }
                    catch (Exception ex) { }

                    if (TmpRows > 0)
                    {
                        Txt_CA_Program.Text = Sel_CAMS_Program;
                        //Cmb_Program.Items.AddRange(listItem.ToArray());
                        //if (DefHieExist)
                        //    Cmb_Program.SelectedIndex = (ProgIndex);// Murali changed..
                        //else
                        //    Cmb_Program.SelectedIndex = 0;
                    }
                }

            }
            else
                MessageBox.Show("Programs Are Not Defined", "CAP Systems");
        }

        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            Sel_CAMS_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_CAMS_Program;
        }

        private void Fill_MS_Program_Combo()
        {
            //Cmb_MS_Program.Items.Clear();
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", BaseForm.BaseAgency, BaseForm.BaseDept, " ");

            //List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
            //CASESPMEntity Search_Entity = new CASESPMEntity(true);

            //if (Pass_MS_Entity.Rec_Type == "I")
            //{
            //    Search_Entity.agency = BaseForm.BaseAgency;
            //    Search_Entity.dept = BaseForm.BaseDept;
            //    Search_Entity.program = BaseForm.BaseProg;
            //    Search_Entity.app_no = BaseForm.BaseApplicationNo;
            //    Search_Entity.service_plan = Pass_MS_Entity.Service_plan;

            //    CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            //}

            //int TmpRows = 1,  ProgIndex = 0;
            //bool DefHieExist = false;
            //List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("   ", "0"));
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataTable dt = ds.Tables[0];

            //        string Tmp_Hierarchy = " ";

            //        try
            //        {
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                if (dt.Columns.Count >= 4)
            //                    Tmp_Hierarchy = dr["Agy"].ToString() + dr["Dept"].ToString() + dr["Prog"].ToString();
            //                else
            //                    Tmp_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + dr["Prog"].ToString();
            //                listItem.Add(new ListItem(dr["Prog"] + " - " + dr["Name"], Tmp_Hierarchy));

            //                if (Pass_MS_Entity.Rec_Type == "I" && ProgIndex == 0)
            //                {
            //                    foreach (CASESPMEntity Entity in CASESPM_List)
            //                    {
            //                        if (Entity.Def_Program == Tmp_Hierarchy)
            //                        {
            //                            DefHieExist = true;
            //                            ProgIndex = TmpRows;
            //                            break;
            //                        }
            //                    }

            //                }
            //                TmpRows++;
            //            }


            //        }
            //        catch (Exception ex) { }
            //    }
            //    else
            //        MessageBox.Show("Programs Are Not Defined", "CAP Systems");
            //}
            //Cmb_MS_Program.Items.AddRange(listItem.ToArray());

            //if (TmpRows > 0)
            //{
            //    if (DefHieExist)
            //        Cmb_MS_Program.SelectedIndex = (ProgIndex);
            //    else
            //    {
            //        //if (Cmb_MS_Program.Items.Count == 1)
            //            Cmb_MS_Program.SelectedIndex = 0;
            //    }
            //}

            List<CASESPMEntity> CASESPM_List = new List<CASESPMEntity>();
            CASESPMEntity Search_Entity = new CASESPMEntity(true);

            if (Pass_MS_Entity.Rec_Type == "I")
            {
                Search_Entity.agency = BaseForm.BaseAgency;
                Search_Entity.dept = BaseForm.BaseDept;
                Search_Entity.program = BaseForm.BaseProg;
                Search_Entity.app_no = BaseForm.BaseApplicationNo;
                Search_Entity.service_plan = Pass_MS_Entity.Service_plan;

                CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");
            }

            string ACR_SERV_Hies = "N";
            if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                ACR_SERV_Hies = BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim();

            Sel_CAMS_Program = "";
            int TmpRows = 1;
            SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, Pass_MS_Entity.Service_plan, ACR_SERV_Hies);
            Txt_MS_Program.Clear();
            if (SP_Programs_List.Count > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                {
                    string Tmp_Hierarchy = " ";
                    int ProgIndex = 0; bool DefHieExist = false;
                    try
                    {
                        if (CASESPM_List.Count > 0)
                        {
                            foreach (HierarchyEntity Ent in SP_Programs_List)
                            {
                                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                                if (Pass_MS_Entity.Rec_Type == "I" && ProgIndex == 0)
                                {
                                    foreach (CASESPMEntity Entity in CASESPM_List)
                                    {
                                        if (Entity.Def_Program == Tmp_Hierarchy)
                                        {
                                            Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                            DefHieExist = true;
                                            ProgIndex = TmpRows;
                                            break;
                                        }
                                    }

                                }
                                TmpRows++;
                            }
                        }
                        else
                        {
                            foreach (HierarchyEntity Ent in SP_Programs_List)
                            {
                                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                                if (Pass_MS_Entity.Rec_Type == "I" && ProgIndex == 0)
                                {
                                    if (Pass_MS_Entity.Acty_PROG == Tmp_Hierarchy)
                                    {
                                        Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                                        TmpRows++;
                                        DefHieExist = true;
                                        ProgIndex = TmpRows;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }

                    if (TmpRows > 0)
                        Txt_MS_Program.Text = Sel_CAMS_Program;
                }

            }
            else
                MessageBox.Show("Programs Are Not Defined", "CAP Systems");

        }


        List<CASEMSOBOEntity> CASEMSOBO_List = new List<CASEMSOBOEntity>();
        CASEMSOBOEntity Search_OBO_Entity = new CASEMSOBOEntity();
        private void Get_OBO_Data()
        {

            Search_OBO_Entity.ID = Pass_MS_Entity.ID;
            if (MS_Template_List.Count > 0 && Pass_MS_Entity.Rec_Type == "I")
                Search_OBO_Entity.ID = MS_Template_List[0].ID;

            Search_OBO_Entity.Seq = Search_OBO_Entity.CLID = Search_OBO_Entity.Fam_Seq = null;

            CASEMSOBO_List = _model.SPAdminData.Browse_CASEMSOBO(Search_OBO_Entity, "Browse");
        }

        List<CAOBOEntity> CAOBO_List = new List<CAOBOEntity>();
        CAOBOEntity Search_CAOBO_Entity = new CAOBOEntity();
        private void Get_CAOBO_Data()
        {

            Search_CAOBO_Entity.ID = Pass_CA_Entity.ACT_ID;
            if (CA_Template_List.Count > 0 && Pass_CA_Entity.Rec_Type == "I")
                Search_CAOBO_Entity.ID = CA_Template_List[0].ACT_ID;

            Search_CAOBO_Entity.Seq = Search_CAOBO_Entity.CLID = Search_CAOBO_Entity.Fam_Seq = null;

            CAOBO_List = _model.SPAdminData.Browse_CAOBO(Search_CAOBO_Entity, "Browse");
        }


        List<CustfldsEntity> Cust;
        private void Fill_Custom_Questions()
        {
            Cust_Grid.Rows.Clear();
            Cust = _model.FieldControls.GetCUSTFLDSByScrCode("CASE0063", "FLDCNTLHIE", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions("CASE0063", "*", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "", "ALL", "P");

            CustRespEntity Search_Entity = new CustRespEntity(true);
            Search_Entity.ScrCode = "CASE0063";
            CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");

            //if (Pass_CA_Entity.Rec_Type == "I" && CA_Template_List.Count > 0)
            //{
            //    Pass_CA_Entity.Cust_Code1 = CA_Template_List[0].Cust_Code1;
            //    Pass_CA_Entity.Cust_Code2 = CA_Template_List[0].Cust_Code2;
            //    Pass_CA_Entity.Cust_Code3 = CA_Template_List[0].Cust_Code3;
            //}

            if (Cust.Count > 0)
            {
                int rowIndex = 0, Tmp_Row_Cnt = 0;
                bool DropDown_Exists = false, IS_Que_Req = false;
                string Tmp_Cust_Resp = null, Tmp_Cust_Resp_Code = null, Tmp_Cust_Resp_Desc = "";
                foreach (CustfldsEntity Entity in Cust)
                {
                    Tmp_Cust_Resp = " ";
                    if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code1))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value1;
                    else
                        if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code2))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value2;
                    else
                            if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code3))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value3;
                    else
                                if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code4))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value4;
                    else
                                    if (Entity.CustCode.Equals(Pass_CA_Entity.Cust_Code5))
                        Tmp_Cust_Resp = Pass_CA_Entity.Cust_Value5;

                    Tmp_Cust_Resp_Desc = Tmp_Cust_Resp;
                    if (Entity.RespType == "D" || Entity.RespType == "C")
                    {
                        foreach (CustRespEntity Ent in CustResp_List)
                        {
                            if (Entity.CustCode == Ent.ResoCode && Tmp_Cust_Resp == Ent.DescCode)
                            {
                                Tmp_Cust_Resp_Desc = Ent.RespDesc;
                                break;
                            }
                        }
                    }

                    IS_Que_Req = false;
                    foreach (CustomQuestionsEntity Cust_Fld in custQuestions)
                    {
                        if (Entity.CustCode == Cust_Fld.CUSTCODE && Cust_Fld.CUSTREQUIRED == "Y")
                        {
                            IS_Que_Req = true;
                            break;
                        }
                    }

                    rowIndex = Cust_Grid.Rows.Add(Entity.CustDesc, Tmp_Cust_Resp_Desc, (IS_Que_Req ? "*" : ""), Entity.RespType, Entity.CustCode, Tmp_Cust_Resp);
                    Tmp_Row_Cnt++;

                    set_Cust_Grid_Tooltip(rowIndex, Entity.RespType);

                    if (Entity.RespType == "C" || Entity.RespType == "D")
                    {
                        DropDown_Exists = true;
                        Cust_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        Cust_Grid.Rows[rowIndex].Cells["Resp"].ReadOnly = true;
                    }

                    if (Tmp_Row_Cnt == 5) //changed from 3 to 5 On 11/20/2014
                        break;
                }
                //set_CustGrid_Rows_On_Questions(DropDown_Exists);
            }
        }

        private void CA_Cust_Grid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            if (objArgs.MenuItem.Tag is string)
            {
                if (objArgs.MenuItem.Tag != "CLRRSP")
                {
                    Cust_Grid.CurrentRow.Cells["Resp"].Value = objArgs.MenuItem.Text;
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = objArgs.MenuItem.Tag;
                }
                else
                {
                    Cust_Grid.CurrentRow.Cells["Resp"].Value = " ";
                    Cust_Grid.CurrentRow.Cells["Resp_Code"].Value = " ";
                }
            }
        }


        List<CustRespEntity> CustResp_List;
        private void set_CustGrid_Rows_On_Questions(bool DropDown_Exists)
        {
            CustRespEntity Search_Entity = new CustRespEntity();

            Search_Entity.ScrCode = "CASE0063";
            Search_Entity.RecType = Search_Entity.ResoCode = Search_Entity.RespSeq = null;
            Search_Entity.RespDesc = Search_Entity.DescCode = Search_Entity.AddDate = Search_Entity.AddOpr = null;
            Search_Entity.ChgDate = Search_Entity.ChgOpr = Search_Entity.Changed = Search_Entity.ScrCode = null;

            if (DropDown_Exists)
                CustResp_List = _model.FieldControls.Browse_CUSTRESP(Search_Entity, "Browse");

        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                CaseVddlist = CaseVddlist.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }


        private void Fill_Members_DropDown()
        {
            Members_Grid.Rows.Clear();
            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, Pass_MS_Entity.App_no, null, null, null, null, null, null, null, null, null, null, Hierarchy + Year, null, BaseForm.UserID, string.Empty, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<CommonEntity> Relation;
                    Relation = _model.lookupDataAccess.GetRelationship();

                    int rowIndex = 0;
                    string Name = null, TmpSsn = null, Relation_Desc = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Name = TmpSsn = Relation_Desc = null;

                        Name = dr["Fname"].ToString().Trim() + " " + dr["MName"].ToString() + " " + dr["Lname"].ToString().Trim();
                        TmpSsn = dr["Ssn"].ToString();
                        if (!string.IsNullOrEmpty(TmpSsn))
                            TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);


                        foreach (CommonEntity Relationship in Relation)
                        {
                            if (Relationship.Code.Equals(dr["Mem_Code"].ToString().Trim()))
                            {
                                Relation_Desc = Relationship.Desc; break;
                            }
                        }



                        rowIndex = Members_Grid.Rows.Add(false, Name, Relation_Desc, TmpSsn, dr["RecFamSeq"].ToString(), dr["ClientID"].ToString(), dr["AppNo"].ToString().Substring(10, 1), "N", dr["AppStatus"].ToString(), dr["SNP_EXCLUDE"].ToString());

                        if (dr["AppStatus"].ToString() != "A")
                            Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        if (dr["SNP_EXCLUDE"].ToString() != "N")
                            Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


                        if (dr["AppNo"].ToString().Substring(10, 1) == "A")
                        {
                            if (dr["AppStatus"].ToString() != "A")
                                Members_Grid.Rows[rowIndex].Cells["Mem_Name"].Style.ForeColor = Color.Blue;
                            else
                                Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        //Members_Grid.Rows[rowIndex].Tag = dr;


                    }
                }
            }
            Get_OBO_Data();
        }

        //private void Fill_CA_Members_DropDown()
        //{
        //    CA_Members_Grid.Rows.Clear();
        //    DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, Pass_CA_Entity.App_no, null, null, null, null, null, null, null, null, null, null, Hierarchy + Year, null, BaseForm.UserID, string.Empty, string.Empty);
        //    if (ds.Tables.Count > 0)
        //    {
        //        DataTable dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            List<CommonEntity> Relation;
        //            Relation = _model.lookupDataAccess.GetRelationship();

        //            int rowIndex = 0;
        //            string Name = null, TmpSsn = null, Relation_Desc = null;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                Name = TmpSsn = Relation_Desc = null;

        //                Name = dr["Fname"].ToString().Trim() + " " + dr["MName"].ToString() + " " + dr["Lname"].ToString().Trim();
        //                TmpSsn = dr["Ssn"].ToString();
        //                if (!string.IsNullOrEmpty(TmpSsn))
        //                    TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);


        //                foreach (CommonEntity Relationship in Relation)
        //                {
        //                    if (Relationship.Code.Equals(dr["Mem_Code"].ToString().Trim()))
        //                    {
        //                        Relation_Desc = Relationship.Desc; break;
        //                    }
        //                }



        //                rowIndex = CA_Members_Grid.Rows.Add(false, Name, Relation_Desc, TmpSsn, dr["RecFamSeq"].ToString(), dr["ClientID"].ToString(), dr["AppNo"].ToString().Substring(10, 1), "N", dr["AppStatus"].ToString(), dr["SNP_EXCLUDE"].ToString());

        //                if (dr["AppStatus"].ToString() != "A")
        //                    CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

        //                if (dr["SNP_EXCLUDE"].ToString() != "N")
        //                    CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;


        //                if (dr["AppNo"].ToString().Substring(10, 1) == "A")
        //                {
        //                    if (dr["AppStatus"].ToString() != "A")
        //                        CA_Members_Grid.Rows[rowIndex].Cells["CA_Mem_Name"].Style.ForeColor = Color.Blue;
        //                    else
        //                        CA_Members_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
        //                }
        //                //Members_Grid.Rows[rowIndex].Tag = dr;


        //            }
        //        }
        //    }
        //    Get_CAOBO_Data();
        //}

        DataTable dtmembers = new DataTable();
        private void FillApplicantData()
        {
            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, Pass_CA_Entity.App_no, null, null, null, null, null, null, null, null, null, null, Hierarchy + Year, null, BaseForm.UserID, string.Empty, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                dtmembers = ds.Tables[0];
            }
        }

        private void Fill_Benefiting_From()
        {
            this.Cmb_MS_Benefit.SelectedIndexChanged -= new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);

            Cmb_MS_Benefit.Items.Clear();
            Cmb_MS_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
            Cmb_MS_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
            Cmb_MS_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));


            this.Cmb_MS_Benefit.SelectedIndexChanged += new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);

            if (Pass_MS_Entity.Rec_Type == "I")
            {
                if (!string.IsNullOrEmpty(ACR_BenefitFrom.Trim()))
                    Cmb_MS_Benefit.SelectedIndex = (int.Parse(ACR_BenefitFrom.Trim()) - 1);
                else
                    Cmb_MS_Benefit.SelectedIndex = 0;
            }
            //if (Pass_MS_Entity.Rec_Type == "I")
            //    Cmb_MS_Benefit.SelectedIndex = 0;
            //if (Pass_MS_Entity.Rec_Type == "I")

        }

        private void Fill_CA_Benefiting_From()
        {
            this.cmb_CA_Benefit.SelectedIndexChanged -= new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);

            cmb_CA_Benefit.Items.Clear();
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Applicant", "1"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("All Household Members", "2"));
            cmb_CA_Benefit.Items.Add(new Captain.Common.Utilities.ListItem("Selected Household Members", "3"));


            this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
            if (Pass_CA_Entity.Rec_Type == "I")
            {
                if (!string.IsNullOrEmpty(ACR_BenefitFrom.Trim()))
                    cmb_CA_Benefit.SelectedIndex = (int.Parse(ACR_BenefitFrom.Trim()) - 1);
                else
                    cmb_CA_Benefit.SelectedIndex = 0;
            }

            //if (Pass_CA_Entity.Rec_Type == "I")
            //    cmb_CA_Benefit.SelectedIndex = 0;

        }

        private void Fill_Results()
        {
            Cmb_MS_Results.Items.Clear(); Cmb_MS_Results.ColorMember = "FavoriteColor";

            Cmb_MS_Results.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            Cmb_MS_Results.SelectedIndex = 0;


            if (!string.IsNullOrEmpty(SP_Header_Rec.Status))
            {
                bool Ststus_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0, Tmp_Curr_Status_Len = 0;
                string Status_Str = SP_Header_Rec.Status;
                List<SPCommonEntity> ResultsList = new List<SPCommonEntity>();
                ResultsList = _model.SPAdminData.Get_AgyRecs("Results");

                foreach (SPCommonEntity Entity in ResultsList)
                {
                    Ststus_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Status.Length; i++)
                    {
                        Tmp_Curr_Status_Len = (Status_Str.Substring(Pos, Status_Str.Substring(Pos, (Status_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Status.Substring(Pos, (Tmp_Curr_Status_Len >= 4 ? 4 : Tmp_Curr_Status_Len)).Trim())
                        {
                            Ststus_Exists = true; break;
                        }
                        Pos += 4;
                    }

                    if (Ststus_Exists)
                    {
                        if (Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                        {
                            Cmb_MS_Results.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                            Tmp_Loop_Cnt++;
                        }
                    }
                }

                //if (Tmp_Loop_Cnt > 0)
                //{
                //    Cmb_MS_Results.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
                //    Cmb_MS_Results.SelectedIndex = 0;
                //}
            }
        }

        private void Fill_UOM()
        {
            List<CommonEntity> Gender = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.UOMTABLE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); // _model.lookupDataAccess.GetGender();
            // Gender = filterByHIE(Gender);
            Cmb_UOM.Items.Insert(0, new Captain.Common.Utilities.ListItem(" ", "0"));
            Cmb_UOM.ColorMember = "FavoriteColor";
            Cmb_UOM.SelectedIndex = 0;
            foreach (CommonEntity gender in Gender)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(gender.Desc, gender.Code, gender.Active, gender.Active.Equals("Y") ? Color.Green : Color.Red);
                Cmb_UOM.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && gender.Default.Equals("Y")) Cmb_UOM.SelectedItem = li;
            }
        }

        private void Fill_Funding()
        {
            CmbFunding1.Items.Clear(); CmbFunding1.ColorMember = "FavoriteColor";
            CmbFunding2.Items.Clear(); CmbFunding2.ColorMember = "FavoriteColor";
            CmbFunding3.Items.Clear(); CmbFunding3.ColorMember = "FavoriteColor";

            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
            if (!string.IsNullOrEmpty(SP_Header_Rec.Funds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;
                
                string Funds_Str = SP_Header_Rec.Funds;
                int Tmp_Curr_Fund_Len = 0;

                foreach (SPCommonEntity Entity in FundingList)
                {

                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Funds.Length; i++)
                    {
                        //if (Entity.Code == SP_Header_Rec.Funds.Substring(Pos, 4).Trim())
                        //{
                        //    Fund_Exists = true; break;
                        //}
                        //Pos += 4;

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Funds.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        if (Mode == "Edit" || (Mode == "Add" && Entity.Active.Equals("Y")))
                        {
                            CmbFunding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                            CmbFunding2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                            CmbFunding3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, (Entity.Active.Equals("Y") ? Color.Green : Color.Red)));
                            Tmp_Loop_Cnt++;
                        }
                    }
                }
                //if (Tmp_Loop_Cnt > 0)
                //{
                //CmbFunding1.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
                //CmbFunding2.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
                //CmbFunding3.Items.Insert(0, new ListItem("    ", "0", " ", Color.White));
                //CmbFunding1.SelectedIndex = 0;
                //CmbFunding2.SelectedIndex = 0;
                //CmbFunding3.SelectedIndex = 0;
                //}
            }
            if (Mode == "Edit" && !string.IsNullOrEmpty(SP_Header_Rec.UsedFunds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;
                string Funds_Str = SP_Header_Rec.UsedFunds;
                int Tmp_Curr_Fund_Len = 0;

                foreach (SPCommonEntity Entity in FundingList)
                {
                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.UsedFunds.Length; i++)
                    {

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.UsedFunds.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        if (Mode == "Edit")
                        {
                            CmbFunding1.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, Color.Red));
                            CmbFunding2.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, Color.Red));
                            CmbFunding3.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc, Entity.Code, Entity.Active, Color.Red));

                            Tmp_Loop_Cnt++;
                        }
                    }
                }
            }
            CmbFunding1.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            CmbFunding2.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            CmbFunding3.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            CmbFunding1.SelectedIndex = CmbFunding2.SelectedIndex = CmbFunding3.SelectedIndex = 0;
        }

        private void Fill_Sites()
        {
            CmbSite.Items.Clear();
            Cmb_MS_Site.Items.Clear();
            Cmb_Bulk_Site.Items.Clear();

            CmbSite.ColorMember = "FavoriteColor";
            Cmb_MS_Site.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));

            //DataSet ds = Captain.DatabaseLayer.Lookups.GetCaseSite();
            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable Sites_Table = ds.Tables[0];
                if (Sites_Table.Rows.Count > 0)
                {
                    if (Mode.Equals("Add"))
                    {
                        DataView dv = new DataView(Sites_Table);
                        dv.RowFilter = "SITE_ACTIVE='Y'";
                        Sites_Table = dv.ToTable();
                    }

                    foreach (DataRow dr in Sites_Table.Rows)
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString().Trim(), dr["SITE_ACTIVE"].ToString().Trim(), (dr["SITE_ACTIVE"].ToString().Trim().Equals("Y") ? Color.Green : Color.Red)));
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
                        listItemSite.Add(new Captain.Common.Utilities.ListItem("   ", "0", " ", Color.White));
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
            if (CAMS_FLG == "CA")
                CmbSite.Items.AddRange(listItem.ToArray());
            else if (CAMS_FLG == "MS")
                Cmb_MS_Site.Items.AddRange(listItem.ToArray());
            else
            {
                Cmb_Bulk_Site.Items.AddRange(listItem.ToArray());
                Cmb_Bulk_Site.Items.Insert(0, new Captain.Common.Utilities.ListItem("All Sites", "0"));
                Cmb_Bulk_Site.SelectedIndex = 0;
            }


            switch (CAMS_FLG)
            {
                case "CA":
                    if (Pass_CA_Entity.Rec_Type == "I") //!string.IsNullOrEmpty(MST_Site))
                    {
                        if (CA_Template_List.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(BaseForm.UserProfile.Site.Trim()) && BaseForm.UserProfile.Site.Trim() != "****")
                            {
                                SetComboBoxValue(CmbSite, BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.Trim().Length - 2));
                                //if (BaseForm.UserProfile.Site.Trim().Length == 6)
                                //    SetComboBoxValue(CmbSite, BaseForm.UserProfile.Site.Trim().Substring(2, 4));
                                //else
                                //    SetComboBoxValue(CmbSite, BaseForm.UserProfile.Site.Trim());
                            }
                            else if (!string.IsNullOrEmpty(MST_Site))
                                SetComboBoxValue(CmbSite, MST_Site);
                            else if (!string.IsNullOrEmpty(SPM_Site))
                                SetComboBoxValue(CmbSite, SPM_Site);
                            else CmbSite.SelectedIndex = 0;
                        }
                    }
                    else
                        CmbSite.SelectedIndex = 0;
                    break;
                case "MS":
                    if (Pass_MS_Entity.Rec_Type == "I")//&& !string.IsNullOrEmpty(MST_Site))
                    {
                        if (!string.IsNullOrEmpty(BaseForm.UserProfile.Site.Trim()) && BaseForm.UserProfile.Site.Trim() != "****")
                        {
                            SetComboBoxValue(Cmb_MS_Site, BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.Trim().Length - 2));
                        }
                        else if (!string.IsNullOrEmpty(MST_Site))
                            SetComboBoxValue(Cmb_MS_Site, MST_Site);
                        else if (!string.IsNullOrEmpty(SPM_Site))
                            SetComboBoxValue(Cmb_MS_Site, SPM_Site);
                        else Cmb_MS_Site.SelectedIndex = 0;
                    }
                    else
                        Cmb_MS_Site.SelectedIndex = 0;
                    break;
            }
        }

        private void Fill_CaseWorker()
        {
            //DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Hierarchy.Substring(0, 2), Hierarchy.Substring(2, 2), Hierarchy.Substring(4, 2));
            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Hierarchy.Substring(0, 2), "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            CmbWorker.Items.Clear(); CmbWorker.ColorMember = "FavoriteColor";
            Cmb_MS_Worker.Items.Clear(); Cmb_MS_Worker.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            //CmbWorker.Items.Insert(0, new ListItem("All", "**"));
            listItem.Add(new Captain.Common.Utilities.ListItem("    ", "0", " ", Color.White));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, Hierarchy.Substring(0, 2), Hierarchy.Substring(2, 2), Hierarchy.Substring(4, 2));
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if ((Mode == "Add" && dr["PWH_INACTIVE"].ToString().Trim() == "N") || (Mode == "Edit"))
                            listItem.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Green));
                    }

                    if (CAMS_FLG == "CA")
                        CmbWorker.Items.AddRange(listItem.ToArray());
                    else
                        Cmb_MS_Worker.Items.AddRange(listItem.ToArray());
                }
            }
            switch (CAMS_FLG)
            {
                case "CA":
                    if (Pass_CA_Entity.Rec_Type == "I") // !string.IsNullOrEmpty(MST_Intakeworker))
                    {

                        if (!string.IsNullOrEmpty(BaseForm.UserProfile.CaseWorker))
                            SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                        //else
                        //    !string.IsNullOrEmpty(MST_Intakeworker)
                        //        SetComboBoxValue(CmbWorker, MST_Intakeworker);
                    }
                    //SetComboBoxValue(CmbWorker, MST_Intakeworker);
                    else
                        CmbWorker.SelectedIndex = 0;
                    break;
                case "MS":
                    if (Pass_MS_Entity.Rec_Type == "I" && !string.IsNullOrEmpty(BaseForm.UserProfile.CaseWorker)) // !string.IsNullOrEmpty(MST_Intakeworker))
                        SetComboBoxValue(Cmb_MS_Worker, BaseForm.UserProfile.CaseWorker);
                    //SetComboBoxValue(Cmb_MS_Worker, MST_Intakeworker);
                    else
                        Cmb_MS_Worker.SelectedIndex = 0;
                    break;
            }
        }


        private void Fill_CA_Controls()
        {
            if (!string.IsNullOrEmpty(Pass_CA_Entity.ACT_Date))
            {
                Act_Date.Checked = true; Act_Date.Value = Convert.ToDateTime(Pass_CA_Entity.ACT_Date);
            }

            if (!string.IsNullOrEmpty(Pass_CA_Entity.ActSeek_Date))
            {
                dtActSeek_Date.Checked = true; dtActSeek_Date.Value = Convert.ToDateTime(Pass_CA_Entity.ActSeek_Date);
            }

            SetComboBoxValue(CmbWorker, Pass_CA_Entity.Caseworker);
            SetComboBoxValue(CmbSite, Pass_CA_Entity.Site);

            SetComboBoxValue(CmbFunding1, Pass_CA_Entity.Fund1);
            SetComboBoxValue(CmbFunding2, Pass_CA_Entity.Fund2);
            SetComboBoxValue(CmbFunding3, Pass_CA_Entity.Fund3);
            SetComboBoxValue(Cmb_UOM, Pass_CA_Entity.UOM);

            //if (!string.IsNullOrEmpty(Pass_CA_Entity.Act_PROG.Trim()) && !Pass_CA_Entity.Act_PROG.Contains("**"))
            //    SetComboBoxValue(Cmb_Program, Pass_CA_Entity.Act_PROG);

            if (!string.IsNullOrEmpty(Pass_CA_Entity.Act_PROG.Trim()) && !Pass_CA_Entity.Act_PROG.Contains("**"))
                Txt_CA_Program.Text = Set_SP_Program_Text(Pass_CA_Entity.Act_PROG.Trim());



            Txt_VendNo.Text = Pass_CA_Entity.Vendor_No;
            Text_VendName.Text = Get_Vendor_Name();

            Txtx_ChkNo.Text = Pass_CA_Entity.Check_No;
            if (Pass_CA_Entity.Units != null)
                Txt_Units.Text = Pass_CA_Entity.Units.Trim();

            if (!string.IsNullOrEmpty(Pass_CA_Entity.Check_Date))
            {
                Check_Date.Checked = true;
                Check_Date.Value = Convert.ToDateTime(Pass_CA_Entity.Check_Date);
            }

            Txt_Cost.Text = Pass_CA_Entity.Cost;

            Txt_RefTo.Text = Pass_CA_Entity.Refer_Data;
            if (!string.IsNullOrEmpty(Pass_CA_Entity.Followup_On))
            {
                UpOn_Date.Checked = true;
                UpOn_Date.Value = Convert.ToDateTime(Pass_CA_Entity.Followup_On);
            }

            if (!string.IsNullOrEmpty(Pass_CA_Entity.Followup_Comp))
            {
                Complete_Date.Checked = true;
                Complete_Date.Value = Convert.ToDateTime(Pass_CA_Entity.Followup_Comp);
            }

            Txt_TobeFollowUp.Text = Pass_CA_Entity.Followup_By;

            //this.cmb_CA_Benefit.SelectedIndexChanged -= new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
            //SetComboBoxValue(cmb_CA_Benefit, Pass_CA_Entity.CA_OBF);

            //if (CAMS_Desc == "Auto Post MS")
            //    Set_CAMembers_Grid_Auto_Post();
            //else
            //    Set_Members_CA_Grid_As_Benefit_Change(false, Pass_CA_Entity.CA_OBF);
            ////  Set_Members_Grid_From_MSOBO();

            //this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);


            CASEREFEntity Search_REF_Entity = new CASEREFEntity(true);
            Search_REFS_Entity.Service = Pass_CA_Entity.ACT_Code.Trim();
            Search_REFS_Entity.Service = Pass_CA_Entity.ACT_Code.Trim();
        }

        private void Fill_CA_Controls_4rm_Template()
        {
            if (!string.IsNullOrEmpty(CA_Template_List[0].ACT_Date))
            {
                Act_Date.Checked = true; Act_Date.Value = Convert.ToDateTime(CA_Template_List[0].ACT_Date);
            }
            if (!string.IsNullOrEmpty(CA_Template_List[0].ActSeek_Date))
            {
                dtActSeek_Date.Checked = true; dtActSeek_Date.Value = Convert.ToDateTime(CA_Template_List[0].ActSeek_Date);
            }
            SetComboBoxValue(CmbWorker, CA_Template_List[0].Caseworker);
            SetComboBoxValue(CmbSite, CA_Template_List[0].Site);

            SetComboBoxValue(CmbFunding1, CA_Template_List[0].Fund1);
            SetComboBoxValue(CmbFunding2, CA_Template_List[0].Fund2);
            SetComboBoxValue(CmbFunding3, CA_Template_List[0].Fund3);
            //if (!string.IsNullOrEmpty(CA_Template_List[0].Act_PROG.Trim()) && !CA_Template_List[0].Act_PROG.Contains("**"))
            //    SetComboBoxValue(Cmb_Program, CA_Template_List[0].Act_PROG);

            if (!string.IsNullOrEmpty(CA_Template_List[0].Act_PROG.Trim()) && !CA_Template_List[0].Act_PROG.Contains("**"))
                Txt_CA_Program.Text = Set_SP_Program_Text(CA_Template_List[0].Act_PROG.Trim());

            Txt_VendNo.Text = CA_Template_List[0].Vendor_No;
            Text_VendName.Text = Get_Vendor_Name();

            Txtx_ChkNo.Text = CA_Template_List[0].Check_No;

            if (!string.IsNullOrEmpty(CA_Template_List[0].Check_Date))
            {
                Check_Date.Checked = true;
                Check_Date.Value = Convert.ToDateTime(CA_Template_List[0].Check_Date);
            }

            Txt_Cost.Text = CA_Template_List[0].Cost;

            Txt_RefTo.Text = CA_Template_List[0].Refer_Data;
            if (!string.IsNullOrEmpty(CA_Template_List[0].Followup_On))
            {
                UpOn_Date.Checked = true;
                UpOn_Date.Value = Convert.ToDateTime(CA_Template_List[0].Followup_On);
            }

            if (!string.IsNullOrEmpty(CA_Template_List[0].Followup_Comp))
            {
                Complete_Date.Checked = true;
                Complete_Date.Value = Convert.ToDateTime(CA_Template_List[0].Followup_Comp);
            }

            Txt_TobeFollowUp.Text = CA_Template_List[0].Followup_By;

            //this.cmb_CA_Benefit.SelectedIndexChanged -= new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);
            //SetComboBoxValue(cmb_CA_Benefit, CA_Template_List[0].CA_OBF);  // Yeswanth

            //foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
            //{
            //    dr.Cells["CA_Sel"].Value = false;
            //    dr.Cells["Is_CAOBO_Rec"].Value = "N";
            //}

            ////if (Members_Grid.Rows.Count > 0)
            //Set_Members_Grid_As_Benefit_Change(false, CA_Template_List[0].CA_OBF);
            ////  Set_Members_Grid_From_MSOBO();

            //this.cmb_CA_Benefit.SelectedIndexChanged += new System.EventHandler(this.cmb_CA_Benefit_SelectedIndexChanged);


            //CASEREFEntity Search_REF_Entity = new CASEREFEntity(true);
            //Search_REFS_Entity.Service = CA_Template_List[0].ACT_Code.Trim();
            //Search_REFS_Entity.Service = CA_Template_List[0].ACT_Code.Trim();
        }

        private string Get_Vendor_Name()
        {
            string Vend_Name = string.Empty;
            foreach (CASEVDDEntity Entity in CaseVddlist)
            {
                if (Entity.Code == Pass_CA_Entity.Vendor_No)
                {
                    Vend_Name = Entity.Name.Trim(); break;
                }
            }

            return Vend_Name;
        }

        private void Fill_MS_Controls()
        {
            if (!string.IsNullOrEmpty(Pass_MS_Entity.Date))
            {
                MS_Date.Checked = true; MS_Date.Value = Convert.ToDateTime(Pass_MS_Entity.Date);
            }

            if (!string.IsNullOrEmpty(Pass_MS_Entity.Seek_Date))
            {
                dtMSSeek_Date.Checked = true; dtMSSeek_Date.Value = Convert.ToDateTime(Pass_MS_Entity.Seek_Date);
            }

            SetComboBoxValue(Cmb_MS_Site, Pass_MS_Entity.Site);
            SetComboBoxValue(Cmb_MS_Results, Pass_MS_Entity.Result);

            SetComboBoxValue(Cmb_MS_Worker, Pass_MS_Entity.CaseWorker);
            //if (!string.IsNullOrEmpty(Pass_MS_Entity.Acty_PROG.Trim()) && !Pass_MS_Entity.Acty_PROG.Contains("**"))
            //    SetComboBoxValue(Cmb_MS_Program, Pass_MS_Entity.Acty_PROG);

            if (!string.IsNullOrEmpty(Pass_MS_Entity.Acty_PROG.Trim()) && !Pass_MS_Entity.Acty_PROG.Contains("**"))
                Txt_MS_Program.Text = Set_SP_Program_Text(Pass_MS_Entity.Acty_PROG.Trim());

            this.Cmb_MS_Benefit.SelectedIndexChanged -= new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);
            SetComboBoxValue(Cmb_MS_Benefit, Pass_MS_Entity.OBF);  // Yeswanth

            //if (Members_Grid.Rows.Count > 0)
            if (CAMS_Desc == "Auto Post MS")
                Set_Members_Grid_Auto_Post();
            else
                Set_Members_Grid_As_Benefit_Change(false, Pass_MS_Entity.OBF);
            //  Set_Members_Grid_From_MSOBO();

            this.Cmb_MS_Benefit.SelectedIndexChanged += new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);
        }

        private void Fill_MS_Controls_4rm_Template()
        {
            if (!string.IsNullOrEmpty(MS_Template_List[0].Date))
            {
                MS_Date.Checked = true; MS_Date.Value = Convert.ToDateTime(MS_Template_List[0].Date);
            }
            if (!string.IsNullOrEmpty(MS_Template_List[0].Seek_Date))
            {
                dtMSSeek_Date.Checked = true; dtMSSeek_Date.Value = Convert.ToDateTime(MS_Template_List[0].Seek_Date);
            }

            SetComboBoxValue(Cmb_MS_Site, MS_Template_List[0].Site);
            SetComboBoxValue(Cmb_MS_Results, MS_Template_List[0].Result);

            SetComboBoxValue(Cmb_MS_Worker, MS_Template_List[0].CaseWorker);
            //if (!string.IsNullOrEmpty(MS_Template_List[0].Acty_PROG.Trim()) && !MS_Template_List[0].Acty_PROG.Contains("**"))
            //    SetComboBoxValue(Cmb_MS_Program, MS_Template_List[0].Acty_PROG);

            if (!string.IsNullOrEmpty(MS_Template_List[0].Acty_PROG.Trim()) && !MS_Template_List[0].Acty_PROG.Contains("**"))
                Txt_MS_Program.Text = Set_SP_Program_Text(MS_Template_List[0].Acty_PROG.Trim());

            this.Cmb_MS_Benefit.SelectedIndexChanged -= new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);
            SetComboBoxValue(Cmb_MS_Benefit, MS_Template_List[0].OBF);  // Yeswanth

            foreach (DataGridViewRow dr in Members_Grid.Rows)
            {
                dr.Cells["MS_Sel"].Value = false;
                dr.Cells["Is_OBO_Rec"].Value = "N";
            }

            //if (Members_Grid.Rows.Count > 0)
            Set_Members_Grid_As_Benefit_Change(false, MS_Template_List[0].OBF);
            //  Set_Members_Grid_From_MSOBO();

            this.Cmb_MS_Benefit.SelectedIndexChanged += new System.EventHandler(this.Cmb_MS_Benefit_SelectedIndexChanged);
        }

        private void Update_MSOBO_Benefitig_Members(int MS_ID)
        {
            foreach (DataGridViewRow dr in Members_Grid.Rows)
            {
                Search_OBO_Entity.ID = Pass_MS_Entity.ID;
                if (Pass_MS_Entity.Rec_Type == "I")
                    Search_OBO_Entity.ID = MS_ID.ToString();

                Search_OBO_Entity.CLID = dr.Cells["CLID"].Value.ToString();
                Search_OBO_Entity.Fam_Seq = dr.Cells["Mem_Seq"].Value.ToString();
                Search_OBO_Entity.Seq = "1";

                if (dr.Cells["MS_Sel"].Value.ToString() == true.ToString())
                {
                    Search_OBO_Entity.Rec_Type = "I";

                    _model.SPAdminData.UpdateCASEMSOBO(Search_OBO_Entity, "Insert", out Sql_SP_Result_Message);
                }
                else
                {
                    //if (dr.Cells["Is_OBO_Rec"].Value.ToString() == "Y")
                    //{
                    Search_OBO_Entity.Rec_Type = "D";

                    _model.SPAdminData.UpdateCASEMSOBO(Search_OBO_Entity, "Delete", out Sql_SP_Result_Message);
                    //}
                }
            }
        }

        //private void Update_CAOBO_Benefitig_Members(int CA_ID)
        //{
        //    foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //    {
        //        Search_CAOBO_Entity.ID = Pass_CA_Entity.ACT_ID;
        //        if (Pass_CA_Entity.Rec_Type == "I")
        //            Search_CAOBO_Entity.ID = CA_ID.ToString();

        //        Search_CAOBO_Entity.CLID = dr.Cells["CA_CLID"].Value.ToString();
        //        Search_CAOBO_Entity.Fam_Seq = dr.Cells["CA_Mem_Seq"].Value.ToString();
        //        Search_CAOBO_Entity.Seq = "1";

        //        if (dr.Cells["CA_Sel"].Value.ToString() == true.ToString())
        //        {
        //            Search_CAOBO_Entity.Rec_Type = "I";

        //            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
        //        }
        //        else
        //        {
        //            if (dr.Cells["Is_CAOBO_Rec"].Value.ToString() == "Y")
        //            {
        //                Search_CAOBO_Entity.Rec_Type = "D";

        //                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
        //            }
        //        }
        //    }
        //}



        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            bool Combo_Set = false;
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (Captain.Common.Utilities.ListItem li in comboBox.Items)
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


        string Current_CA_Seq = "1";
        private void Btn_CASave_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                Get_Latest_Activity_data();
                Pass_CA_Entity.CA_OBF = ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

                if (Pass_CA_Entity.CA_OBF == "3" && CAOBO_List.Count == 0)
                {
                    MessageBox.Show("Members should be selected", "CAP Systems");
                }
                else
                {
                    string Operatipn_Mode = "Insert";

                    if (Pass_CA_Entity.Rec_Type == "U")
                        Operatipn_Mode = "Update";

                    Pass_CA_Entity.Lsct_Operator = BaseForm.UserID;

                    int New_CAID = 1, New_CA_Seq = 1;
                    if (!string.IsNullOrEmpty(Pass_CA_Entity.ACT_ID) && Pass_CA_Entity.Rec_Type == "U")
                        New_CAID = int.Parse(Pass_CA_Entity.ACT_ID);
                    else
                        Pass_CA_Entity.ACT_ID = "1";



                    if (CAMS_Desc == "Auto Post CA")
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }

                    if (_model.SPAdminData.UpdateCASEACT(Pass_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message))
                    {

                        Pass_CA_Entity.ACT_ID = New_CAID.ToString();
                        Pass_CA_Entity.ACT_Seq = Current_CA_Seq = New_CA_Seq.ToString();
                        ////if (CA_Members_Grid.Rows.Count > 0)  // Yeswanth Sindhe
                        ////    Update_CAOBO_Benefitig_Members(New_CAID);

                        if (CAOBO_List.Count > 0)
                        {
                            Search_CAOBO_Entity.ID = New_CAID.ToString();
                            Search_CAOBO_Entity.Rec_Type = "S";
                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                            foreach (CAOBOEntity Entity in CAOBO_List)
                            {
                                Search_CAOBO_Entity.CLID = Entity.CLID.ToString();
                                Search_CAOBO_Entity.Fam_Seq = Entity.Fam_Seq.ToString();
                                Search_CAOBO_Entity.Seq = "1";
                                Search_CAOBO_Entity.Rec_Type = "I";

                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                            }
                        }
                        else
                        {
                            if (dtmembers.Rows.Count > 0)
                            {
                                if (Pass_CA_Entity.CA_OBF == "1")
                                {
                                    Search_CAOBO_Entity.ID = New_CAID.ToString();
                                    Search_CAOBO_Entity.Rec_Type = "S";
                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                    foreach (DataRow dr in dtmembers.Rows)
                                    {
                                        if (dr["AppNo"].ToString().Substring(10, 1) == "A" && dr["AppStatus"].ToString() == "A" && dr["SNP_EXCLUDE"].ToString() == "N")
                                        {
                                            Search_CAOBO_Entity.CLID = dr["ClientID"].ToString();
                                            Search_CAOBO_Entity.Fam_Seq = dr["RecFamSeq"].ToString();
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                            break;
                                        }
                                    }
                                }
                                else if (Pass_CA_Entity.CA_OBF == "2")
                                {
                                    Search_CAOBO_Entity.ID = New_CAID.ToString();
                                    Search_CAOBO_Entity.Rec_Type = "S";
                                    _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                    foreach (DataRow dr in dtmembers.Rows)
                                    {
                                        if (dr["AppStatus"].ToString() == "A" && dr["SNP_EXCLUDE"].ToString() == "N")
                                        {
                                            Search_CAOBO_Entity.CLID = dr["ClientID"].ToString();
                                            Search_CAOBO_Entity.Fam_Seq = dr["RecFamSeq"].ToString();
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);


                                        }
                                    }
                                }
                            }
                        }

                        ////if (Ref_Grid.Rows.Count > 0)
                        ////    Update_ReferrTo_in_CASEREFS();
                        CASEACTEntity New_CA_Entity = new CASEACTEntity(true);
                        if (SP_CA_Grid.Rows.Count > 0)
                        {
                            //New_CA_Entity = Pass_CA_Entity;
                            foreach (DataGridViewRow dr in SP_CA_Grid.Rows)
                            {
                                if (dr.Cells["Sel_1"].Value.ToString() == true.ToString())
                                {
                                    New_CA_Entity.Agency = Pass_CA_Entity.Agency; New_CA_Entity.Dept = Pass_CA_Entity.Dept; New_CA_Entity.Program = Pass_CA_Entity.Program;
                                    New_CA_Entity.Year = Pass_CA_Entity.Year; New_CA_Entity.App_no = Pass_CA_Entity.App_no; New_CA_Entity.Service_plan = Pass_CA_Entity.Service_plan;
                                    New_CA_Entity.SPM_Seq = Pass_CA_Entity.SPM_Seq; New_CA_Entity.ACT_Date = Pass_CA_Entity.ACT_Date; New_CA_Entity.Caseworker = Pass_CA_Entity.Caseworker;
                                    New_CA_Entity.Site = Pass_CA_Entity.Site; New_CA_Entity.Fund1 = Pass_CA_Entity.Fund1; New_CA_Entity.Fund2 = Pass_CA_Entity.Fund2; New_CA_Entity.Fund3 = Pass_CA_Entity.Fund3;
                                    New_CA_Entity.Vendor_No = Pass_CA_Entity.Vendor_No; New_CA_Entity.Check_Date = Pass_CA_Entity.Check_Date; New_CA_Entity.Check_No = Pass_CA_Entity.Check_No;
                                    New_CA_Entity.Cost = Pass_CA_Entity.Cost; New_CA_Entity.Followup_By = Pass_CA_Entity.Followup_By; New_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_Comp;
                                    New_CA_Entity.Followup_On = Pass_CA_Entity.Followup_On; New_CA_Entity.Refer_Data = Pass_CA_Entity.Refer_Data;
                                    New_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Code1; New_CA_Entity.Cust_Value1 = Pass_CA_Entity.Cust_Value1;
                                    New_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Code2; New_CA_Entity.Cust_Value2 = Pass_CA_Entity.Cust_Value2;
                                    New_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Code3; New_CA_Entity.Cust_Value3 = Pass_CA_Entity.Cust_Value3;
                                    New_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Code4; New_CA_Entity.Cust_Value4 = Pass_CA_Entity.Cust_Value4;
                                    New_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Code5; New_CA_Entity.Cust_Value5 = Pass_CA_Entity.Cust_Value5;
                                    New_CA_Entity.Bulk = "Q"; New_CA_Entity.Units = Pass_CA_Entity.Units; New_CA_Entity.VOUCHNO = Pass_CA_Entity.VOUCHNO;
                                    New_CA_Entity.Act_PROG = Pass_CA_Entity.Act_PROG; New_CA_Entity.Notes_Count = Pass_CA_Entity.Notes_Count;
                                    New_CA_Entity.Lsct_Operator = BaseForm.UserID;

                                    New_CA_Entity.ACT_Code = dr.Cells["SP2_CAMS_Code"].Value.ToString();
                                    New_CA_Entity.Branch = dr.Cells["SP2_Branch"].Value.ToString();
                                    New_CA_Entity.Group = dr.Cells["SP2_Group"].Value.ToString();
                                    New_CA_Entity.Curr_Grp = dr.Cells["SP2_Curr_Grp"].Value.ToString();
                                    New_CA_Entity.ACT_ID = "1";
                                    New_CA_Entity.ACT_Seq = "1";
                                    New_CA_Entity.Rec_Type = "I";
                                    New_CA_Entity.CA_OBF = Pass_CA_Entity.CA_OBF;
                                    Operatipn_Mode = "Insert";

                                    _model.SPAdminData.UpdateCASEACT(New_CA_Entity, Operatipn_Mode, out New_CAID, out New_CA_Seq, out Sql_SP_Result_Message);

                                    dr.Cells["SP2_CAMS_ID"].Value = New_CAID.ToString();
                                    dr.Cells["SP2_CA_Seq"].Value = New_CA_Seq.ToString();

                                    if (CAOBO_List.Count > 0)
                                    {
                                        Search_CAOBO_Entity.ID = New_CAID.ToString();
                                        Search_CAOBO_Entity.Rec_Type = "S";
                                        _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                        foreach (CAOBOEntity Entity in CAOBO_List)
                                        {
                                            Search_CAOBO_Entity.CLID = Entity.CLID.ToString();
                                            Search_CAOBO_Entity.Fam_Seq = Entity.Fam_Seq.ToString();
                                            Search_CAOBO_Entity.Seq = "1";
                                            Search_CAOBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);
                                        }
                                    }
                                    else
                                    {
                                        if (dtmembers.Rows.Count > 0)
                                        {
                                            if (Pass_CA_Entity.CA_OBF == "1")
                                            {
                                                Search_CAOBO_Entity.ID = New_CAID.ToString();
                                                Search_CAOBO_Entity.Rec_Type = "S";
                                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                                foreach (DataRow dr1 in dtmembers.Rows)
                                                {
                                                    if (dr1["AppNo"].ToString().Substring(10, 1) == "A" && dr1["AppStatus"].ToString() == "A" && dr1["SNP_EXCLUDE"].ToString() == "N")
                                                    {
                                                        Search_CAOBO_Entity.CLID = dr1["ClientID"].ToString();
                                                        Search_CAOBO_Entity.Fam_Seq = dr1["RecFamSeq"].ToString();
                                                        Search_CAOBO_Entity.Seq = "1";
                                                        Search_CAOBO_Entity.Rec_Type = "I";

                                                        _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                                        break;
                                                    }
                                                }
                                            }
                                            else if (Pass_CA_Entity.CA_OBF == "2")
                                            {
                                                Search_CAOBO_Entity.ID = New_CAID.ToString();
                                                Search_CAOBO_Entity.Rec_Type = "S";
                                                _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Delete", out Sql_SP_Result_Message);
                                                foreach (DataRow dr1 in dtmembers.Rows)
                                                {
                                                    if (dr1["AppStatus"].ToString() == "A" && dr1["SNP_EXCLUDE"].ToString() == "N")
                                                    {
                                                        Search_CAOBO_Entity.CLID = dr1["ClientID"].ToString();
                                                        Search_CAOBO_Entity.Fam_Seq = dr1["RecFamSeq"].ToString();
                                                        Search_CAOBO_Entity.Seq = "1";
                                                        Search_CAOBO_Entity.Rec_Type = "I";

                                                        _model.SPAdminData.UpdateCAOBO(Search_CAOBO_Entity, "Insert", out Sql_SP_Result_Message);

                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }

                        if (Pass_CA_Entity.Rec_Type == "I")
                        {
                            if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                                MessageBox.Show("Critical Activity Posting Successful \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_PROGNotes_For_CAMS);
                            else
                            {
                                MessageBox.Show("Critical Activity Posting Successful", "CAP Systems");

                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Critical Activity Posting Updated Successfully", "CAP Systems");


                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                //else
                //    MessageBox.Show("Members should be select", "CAP Systems");
            }
        }

        public string Get_CA_Sequence()
        {
            return Current_CA_Seq;
        }

        public CASEACTEntity Get_CA_AutoPost_Details()
        {
            return Pass_CA_Entity;
        }

        public CASEMSEntity Get_MS_AutoPost_Details()
        {
            return Pass_MS_Entity;
        }

        public List<CASEMSOBOEntity> Get_MSOBO_AutoPost_Details()
        {
            List<CASEMSOBOEntity> OBO_List = new List<CASEMSOBOEntity>();
            CASEMSOBOEntity Tmp = new CASEMSOBOEntity();
            foreach (DataGridViewRow dr in Members_Grid.Rows)
            {
                if (dr.Cells["MS_Sel"].Value.ToString() == true.ToString())
                {
                    Tmp.ID = Pass_MS_Entity.ID;
                    Tmp.CLID = dr.Cells["CLID"].Value.ToString();
                    Tmp.Fam_Seq = dr.Cells["Mem_Seq"].Value.ToString();
                    Tmp.Seq = "1";

                    OBO_List.Add(new CASEMSOBOEntity(Tmp));
                }
            }

            return OBO_List;
        }


        private void Allow_Post_Future_Date(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                Future_Date_Soft_Edit = true;
                if (CAMS_FLG == "CA")
                    Btn_CASave_Click(Btn_CASave, EventArgs.Empty);
                else
                    Btn_MS_Save_Click(Btn_MS_Save, EventArgs.Empty);
            }
        }



        private void Add_PROGNotes_For_CAMS(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                switch (CAMS_FLG)
                {
                    case "CA": Pb_Notes_Click(Pb_CA_Notes, EventArgs.Empty); break;
                    case "MS": Pb_Notes_Click(Pb_MS_Notes, EventArgs.Empty); break;
                }
                Get_PROG_Notes_Status();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        //CASEREFSEntity REFS_Search_Entity = new CASEREFSEntity();
        //private void Update_ReferrTo_in_CASEREFS()
        //{
        //    ACTREFSEntity Search_ACTREF_Entity = new ACTREFSEntity(true);
        //    foreach (DataGridViewRow dr in Ref_Grid.Rows)
        //    {
        //        //Search_REFS_Entity.Rec_Type = "I";
        //        //Search_REFS_Entity.Code = dr.Cells["Ref_Code"]. Value.ToString();
        //        //Search_REFS_Entity.Service = Pass_CA_Entity.ACT_Code;


        //        Search_ACTREF_Entity.Rec_Type = "I";
        //        if (!string.IsNullOrEmpty(Pass_CA_Entity.ACT_ID))
        //            Search_ACTREF_Entity.Act_ID = Pass_CA_Entity.ACT_ID;
        //        else
        //            Search_ACTREF_Entity.Act_ID = "1";

        //        Search_ACTREF_Entity.Code = dr.Cells["Ref_Code"].Value.ToString();
        //        Search_ACTREF_Entity.Seq = "1";

        //        //_model.SPAdminData.UpdateCASEREFS(Search_REFS_Entity, "Insert");
        //        _model.SPAdminData.UpdateACTREFS(Search_ACTREF_Entity, "Insert", out Sql_SP_Result_Message);
        //    }
        //}


        private void Get_Latest_Activity_data()
        {
            Pass_CA_Entity.Check_Date = Pass_CA_Entity.Followup_Comp = Pass_CA_Entity.Followup_On =
            Pass_CA_Entity.Caseworker = Pass_CA_Entity.Site = Pass_CA_Entity.Fund1 = Pass_CA_Entity.Fund2 =
            Pass_CA_Entity.Fund3 = Pass_CA_Entity.Vendor_No = Pass_CA_Entity.Check_No = Pass_CA_Entity.Cost = Pass_CA_Entity.Refer_Data =
            Pass_CA_Entity.Cust_Code1 = Pass_CA_Entity.Cust_Value1 =
            Pass_CA_Entity.Cust_Code2 = Pass_CA_Entity.Cust_Value2 =
            Pass_CA_Entity.Cust_Code3 = Pass_CA_Entity.Cust_Value3 =
            Pass_CA_Entity.Cust_Code4 = Pass_CA_Entity.Cust_Value4 =
            Pass_CA_Entity.Cust_Code5 = Pass_CA_Entity.Cust_Value5 = Pass_CA_Entity.CA_OBF =
            Pass_CA_Entity.Act_PROG = Pass_CA_Entity.UOM = Pass_CA_Entity.Units = null;

            if (string.IsNullOrEmpty(Pass_CA_Entity.Bulk.Trim()))
                Pass_CA_Entity.Bulk = "Q";

            if (Act_Date.Checked)
                Pass_CA_Entity.ACT_Date = Act_Date.Value.ToShortDateString();

            if (dtActSeek_Date.Checked)
                Pass_CA_Entity.ActSeek_Date = dtActSeek_Date.Value.ToShortDateString();

            if (CmbWorker.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)CmbWorker.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.Caseworker = ((Captain.Common.Utilities.ListItem)CmbWorker.SelectedItem).Value.ToString();
            }

            if (CmbSite.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)CmbSite.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.Site = ((Captain.Common.Utilities.ListItem)CmbSite.SelectedItem).Value.ToString();
            }

            if (CmbFunding1.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)CmbFunding1.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.Fund1 = ((Captain.Common.Utilities.ListItem)CmbFunding1.SelectedItem).Value.ToString();
            }

            if (CmbFunding2.Items.Count > 0)
            {

                if (((Captain.Common.Utilities.ListItem)CmbFunding2.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.Fund2 = ((Captain.Common.Utilities.ListItem)CmbFunding2.SelectedItem).Value.ToString();
            }

            if (CmbFunding3.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)CmbFunding3.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.Fund3 = ((Captain.Common.Utilities.ListItem)CmbFunding3.SelectedItem).Value.ToString();
            }

            if (Cmb_UOM.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)Cmb_UOM.SelectedItem).Value.ToString() != "0")
                    Pass_CA_Entity.UOM = ((Captain.Common.Utilities.ListItem)Cmb_UOM.SelectedItem).Value.ToString();
            }

            //if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString()))
            //    Pass_CA_Entity.CA_OBF = ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(Txt_Units.Text.Trim()))
            {
                Pass_CA_Entity.Units = Txt_Units.Text.Trim();
            }


            //if (Cmb_Program.Items.Count > 0)
            //{
            //    if (((ListItem)Cmb_Program.SelectedItem).Value.ToString() != "0")
            //        Pass_CA_Entity.Act_PROG = ((ListItem)Cmb_Program.SelectedItem).Value.ToString();
            //}

            if (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))
                Pass_CA_Entity.Act_PROG = Txt_CA_Program.Text.Substring(0, 6);

            if (!string.IsNullOrEmpty(Txt_VendNo.Text))
                Pass_CA_Entity.Vendor_No = Txt_VendNo.Text;

            //if (!string.IsNullOrEmpty(Text_VendName.Text))
            //    Pass_CA_Entity.Vendor_No = Text_VendName.Text;

            if (!string.IsNullOrEmpty(Txtx_ChkNo.Text))
                Pass_CA_Entity.Check_No = Txtx_ChkNo.Text;


            if (Check_Date.Checked)
                Pass_CA_Entity.Check_Date = Check_Date.Value.ToShortDateString();

            //if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString()))
            //    Pass_CA_Entity.CA_OBF = ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString();


            if (!string.IsNullOrEmpty(Txt_Cost.Text))
                Pass_CA_Entity.Cost = Txt_Cost.Text;

            if (!string.IsNullOrEmpty(Txt_RefTo.Text))
                Pass_CA_Entity.Refer_Data = Txt_RefTo.Text;

            if (UpOn_Date.Checked)
                Pass_CA_Entity.Followup_On = UpOn_Date.Value.ToShortDateString();

            if (Complete_Date.Checked)
                Pass_CA_Entity.Followup_Comp = Complete_Date.Value.ToShortDateString();

            if (!string.IsNullOrEmpty(Txt_TobeFollowUp.Text))
                Pass_CA_Entity.Followup_By = Txt_TobeFollowUp.Text;


            int Tmp_Cust_Cnt = 1;
            string Curr_Ques_Type = "";
            if (Cust_Grid.Rows.Count > 0)
            {
                this.Cust_Grid.Sort(this.Cust_Grid.Columns["Code"], ListSortDirection.Ascending);
            }
            foreach (DataGridViewRow dr in Cust_Grid.Rows)
            {
                Curr_Ques_Type = dr.Cells["Type"].Value.ToString();
                switch (Tmp_Cust_Cnt)
                {
                    case 1:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                        {
                            Pass_CA_Entity.Cust_Code1 = dr.Cells["Code"].Value.ToString();
                            if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                Pass_CA_Entity.Cust_Value1 = dr.Cells["Resp_Code"].Value.ToString();
                            else
                                Pass_CA_Entity.Cust_Value1 = dr.Cells["Resp"].Value.ToString();

                            if (string.IsNullOrEmpty(Pass_CA_Entity.Cust_Value1.Trim()))
                                Pass_CA_Entity.Cust_Value1 = null;
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                        {
                            Pass_CA_Entity.Cust_Code2 = dr.Cells["Code"].Value.ToString();
                            if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                Pass_CA_Entity.Cust_Value2 = dr.Cells["Resp_Code"].Value.ToString();
                            else
                                Pass_CA_Entity.Cust_Value2 = dr.Cells["Resp"].Value.ToString();

                            if (string.IsNullOrEmpty(Pass_CA_Entity.Cust_Value2.Trim()))
                                Pass_CA_Entity.Cust_Value2 = null;
                        }
                        break;
                    case 3:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                        {
                            Pass_CA_Entity.Cust_Code3 = dr.Cells["Code"].Value.ToString();
                            if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                Pass_CA_Entity.Cust_Value3 = dr.Cells["Resp_Code"].Value.ToString();
                            else
                                Pass_CA_Entity.Cust_Value3 = dr.Cells["Resp"].Value.ToString();

                            if (string.IsNullOrEmpty(Pass_CA_Entity.Cust_Value3.Trim()))
                                Pass_CA_Entity.Cust_Value3 = null;
                        }
                        break;
                    case 4:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                        {
                            Pass_CA_Entity.Cust_Code4 = dr.Cells["Code"].Value.ToString();
                            if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                Pass_CA_Entity.Cust_Value4 = dr.Cells["Resp_Code"].Value.ToString();
                            else
                                Pass_CA_Entity.Cust_Value4 = dr.Cells["Resp"].Value.ToString();

                            if (string.IsNullOrEmpty(Pass_CA_Entity.Cust_Value4.Trim()))
                                Pass_CA_Entity.Cust_Value4 = null;
                        }
                        break;
                    case 5:
                        if (!string.IsNullOrEmpty(dr.Cells["Resp"].EditedFormattedValue.ToString()))
                        {
                            Pass_CA_Entity.Cust_Code5 = dr.Cells["Code"].Value.ToString();
                            if (Curr_Ques_Type == "D" || Curr_Ques_Type == "C")
                                Pass_CA_Entity.Cust_Value5 = dr.Cells["Resp_Code"].Value.ToString();
                            else
                                Pass_CA_Entity.Cust_Value5 = dr.Cells["Resp"].Value.ToString();

                            if (string.IsNullOrEmpty(Pass_CA_Entity.Cust_Value5.Trim()))
                                Pass_CA_Entity.Cust_Value5 = null;
                        }
                        break;
                }
                Tmp_Cust_Cnt++;
            }


        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (!Act_Date.Checked)
            {
                _errorProvider.SetError(Act_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Act_Date, null);


            return isValid;

        }


        private bool Validate_MS_Form()
        {
            bool isValid = true;

            if (!MS_Date.Checked)
            {
                _errorProvider.SetError(MS_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(MS_Date, null);


            if (((Captain.Common.Utilities.ListItem)Cmb_MS_Results.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(Cmb_MS_Results, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResult.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Cmb_MS_Results, null);

            if (((Captain.Common.Utilities.ListItem)Cmb_MS_Worker.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(Cmb_MS_Worker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsCasewor.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Cmb_MS_Worker, null);

            if (((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(Cmb_MS_Benefit, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBenefit.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Cmb_MS_Benefit, null);


            return isValid;
        }

        List<CASEMSEntity> MS_Post_Dates_List = new List<CASEMSEntity>();
        private bool Validate_MS_Posting_Dtae()
        {
            bool Can_Save = true;


            {
                CASEMSEntity Search_Entity = new CASEMSEntity(true);

                Search_Entity.Agency = BaseForm.BaseAgency;
                Search_Entity.Dept = BaseForm.BaseDept;
                Search_Entity.Program = BaseForm.BaseProg;
                Search_Entity.Year = BaseForm.BaseYear;
                Search_Entity.App_no = BaseForm.BaseApplicationNo;
                Search_Entity.Service_plan = Pass_MS_Entity.Service_plan;
                Search_Entity.SPM_Seq = Pass_MS_Entity.SPM_Seq;
                Search_Entity.Branch = Pass_MS_Entity.Branch;
                Search_Entity.Group = Pass_MS_Entity.Group;
                Search_Entity.MS_Code = Pass_MS_Entity.MS_Code;
                if (MS_Post_Dates_List.Count == 0)
                    MS_Post_Dates_List = _model.SPAdminData.Browse_CASEMS(Search_Entity, "Browse");

                int Matched_Count = 0;
                foreach (CASEMSEntity Ent in MS_Post_Dates_List)
                {
                    if (Convert.ToDateTime(Ent.Date.Trim()).ToShortDateString() == MS_Date.Value.ToShortDateString() &&
                        Ent.ID != Pass_MS_Entity.ID)
                    {
                        Can_Save = false;
                        Matched_Count++;

                        //if (Pass_MS_Entity.Rec_Type == "I")
                        break;
                    }
                }

                //if ((Pass_MS_Entity.Rec_Type == "U" && Matched_Count > 1) || (Pass_MS_Entity.Rec_Type == "I" && Matched_Count > 0))


                if (!Can_Save)
                    MessageBox.Show("You Cannot have Multiple Postings for the Same Day", "CAP Systems");
            }

            return Can_Save;
        }

        private void Btn_MS_Save_Click(object sender, EventArgs e)
        {
            if (isValidate() && Validate_MS_Posting_Dtae())
            {
                Get_Latest_MS_data();
                //Pass_MS_Entity.Year = !string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) ? BaseForm.BaseYear : "    ";
                string Operatipn_Mode = "Insert";

                if (Pass_MS_Entity.Rec_Type == "U")
                    Operatipn_Mode = "Update";

                Pass_MS_Entity.Lsct_Operator = BaseForm.UserID;
                if (string.IsNullOrEmpty(Pass_MS_Entity.Bulk.Trim()))
                    Pass_MS_Entity.Bulk = "N";

                if (CAMS_Desc == "Auto Post MS")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }

                int New_MS_ID = 0;

                if (_model.SPAdminData.UpdateCASEMS(Pass_MS_Entity, Operatipn_Mode, out New_MS_ID, out Sql_SP_Result_Message))
                {
                    if (Members_Grid.Rows.Count > 0)  // Yeswanth Sindhe
                        Update_MSOBO_Benefitig_Members(New_MS_ID);

                    if (SP_MS_Grid.Rows.Count > 0)
                    {

                        CASEMSEntity NS_MS_Entity = new CASEMSEntity();
                        foreach (DataGridViewRow dr in SP_MS_Grid.Rows)
                        {
                            if (dr.Cells["MS_Sel_1"].Value.ToString() == true.ToString())
                            {
                                NS_MS_Entity.Agency = Pass_MS_Entity.Agency; NS_MS_Entity.Dept = Pass_MS_Entity.Dept; NS_MS_Entity.Program = Pass_MS_Entity.Program;
                                NS_MS_Entity.Year = Pass_MS_Entity.Year; NS_MS_Entity.App_no = Pass_MS_Entity.App_no; NS_MS_Entity.Service_plan = Pass_MS_Entity.Service_plan;
                                NS_MS_Entity.SPM_Seq = Pass_MS_Entity.SPM_Seq; NS_MS_Entity.Date = Pass_MS_Entity.Date; NS_MS_Entity.CaseWorker = Pass_MS_Entity.CaseWorker;
                                NS_MS_Entity.Site = Pass_MS_Entity.Site; NS_MS_Entity.Result = Pass_MS_Entity.Result; NS_MS_Entity.OBF = Pass_MS_Entity.OBF; NS_MS_Entity.Bulk = "Q";
                                NS_MS_Entity.Acty_PROG = Pass_MS_Entity.Acty_PROG; NS_MS_Entity.Notes_Count = Pass_MS_Entity.Notes_Count;

                                NS_MS_Entity.MS_Code = dr.Cells["SP2_MS_Code"].Value.ToString();
                                NS_MS_Entity.Branch = dr.Cells["MS_SP2_Branch"].Value.ToString();
                                NS_MS_Entity.Group = dr.Cells["MS_SP2_Group"].Value.ToString();
                                NS_MS_Entity.Curr_Grp = dr.Cells["MS_SP2_Curr_Grp"].Value.ToString();
                                NS_MS_Entity.ID = "1";
                                NS_MS_Entity.Rec_Type = "I";
                                NS_MS_Entity.Add_Operator = BaseForm.UserID; NS_MS_Entity.Lsct_Operator = BaseForm.UserID;

                                Operatipn_Mode = "Insert";

                                _model.SPAdminData.UpdateCASEMS(NS_MS_Entity, Operatipn_Mode, out New_MS_ID, out Sql_SP_Result_Message);

                                if (Members_Grid.Rows.Count > 0)
                                {
                                    CASEMSOBOEntity Search_OBO_Entity = new CASEMSOBOEntity();
                                    foreach (DataGridViewRow drmem in Members_Grid.Rows)
                                    {
                                        Search_OBO_Entity.ID = NS_MS_Entity.ID;
                                        if (NS_MS_Entity.Rec_Type == "I")
                                            Search_OBO_Entity.ID = New_MS_ID.ToString();

                                        Search_OBO_Entity.CLID = drmem.Cells["CLID"].Value.ToString();
                                        Search_OBO_Entity.Fam_Seq = drmem.Cells["Mem_Seq"].Value.ToString();
                                        Search_OBO_Entity.Seq = "1";

                                        if (drmem.Cells["MS_Sel"].Value.ToString() == true.ToString())
                                        {
                                            Search_OBO_Entity.Rec_Type = "I";

                                            _model.SPAdminData.UpdateCASEMSOBO(Search_OBO_Entity, "Insert", out Sql_SP_Result_Message);
                                        }
                                        else
                                        {
                                            //if (dr.Cells["Is_OBO_Rec"].Value.ToString() == "Y")
                                            //{
                                            Search_OBO_Entity.Rec_Type = "D";

                                            _model.SPAdminData.UpdateCASEMSOBO(Search_OBO_Entity, "Delete", out Sql_SP_Result_Message);
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (Pass_MS_Entity.Rec_Type == "I")
                    {
                        if (BaseForm.BaseAgencyControlDetails.ProgressNotesSwitch.ToUpper() == "Y")
                            MessageBox.Show("Milestone Posting Successful \n Do you want to add Progress Notes?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:Add_PROGNotes_For_CAMS);
                        else
                        {
                            MessageBox.Show("Milestone Posting Successful", "CAP Systems");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Milestone Posting Updated Successfully", "CAP Systems");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Milestone Posting Unsuccessful \n Reason : " + Sql_SP_Result_Message, "CAP Systems");
            }
        }


        private void Get_Latest_MS_data()
        {
            if (MS_Date.Checked)
                Pass_MS_Entity.Date = MS_Date.Value.ToShortDateString();

            if (dtMSSeek_Date.Checked)
                Pass_MS_Entity.Seek_Date = dtMSSeek_Date.Value.ToShortDateString();

            Pass_MS_Entity.Acty_PROG = Pass_MS_Entity.Site = Pass_MS_Entity.Result = Pass_MS_Entity.CaseWorker = Pass_MS_Entity.OBF = null;

            if (Cmb_MS_Site.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)Cmb_MS_Site.SelectedItem).Value.ToString() != "0")
                    Pass_MS_Entity.Site = ((Captain.Common.Utilities.ListItem)Cmb_MS_Site.SelectedItem).Value.ToString();
            }

            if (Cmb_MS_Results.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)Cmb_MS_Results.SelectedItem).Value.ToString() != "0")
                    Pass_MS_Entity.Result = ((Captain.Common.Utilities.ListItem)Cmb_MS_Results.SelectedItem).Value.ToString();
            }

            if (Cmb_MS_Worker.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)Cmb_MS_Worker.SelectedItem).Value.ToString() != "0")
                    Pass_MS_Entity.CaseWorker = ((Captain.Common.Utilities.ListItem)Cmb_MS_Worker.SelectedItem).Value.ToString();
            }

            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString()))
                Pass_MS_Entity.OBF = ((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString();

            //if (!string.IsNullOrEmpty(((ListItem)Cmb_MS_Program.SelectedItem).Value.ToString()))
            //{
            //    if (((ListItem)Cmb_MS_Program.SelectedItem).Value.ToString() != "0")
            //        Pass_MS_Entity.Acty_PROG = ((ListItem)Cmb_MS_Program.SelectedItem).Value.ToString();
            //}

            if (!string.IsNullOrEmpty(Txt_MS_Program.Text.Trim()))
                Pass_MS_Entity.Acty_PROG = Txt_MS_Program.Text.Trim().Substring(0, 6);
        }

        private void Cmb_MS_Benefit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set_Mem_From_OBO = false;
            //string Mem_To_Select = "M";
            //this.MS_Sel.ReadOnly = true;
            //switch (((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString())
            //{
            //    case "1": Mem_To_Select = "A"; break;
            //    case "2": Mem_To_Select = "M"; break;
            //    case "3": Mem_To_Select = "Y"; this.MS_Sel.ReadOnly = false; break;
            //}

            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Text.ToString()))
                Set_Members_Grid_As_Benefit_Change(true, ((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString());
        }

        //bool Set_Mem_From_OBO = false;
        private void Set_Members_Grid_As_Benefit_Change(bool Set_Mem_On_Combo, string OBF_Type)
        {
            if (Members_Grid.Rows.Count > 0)
            {
                string Mem_Status = "M";
                this.MS_Sel.ReadOnly = true;
                switch (((Captain.Common.Utilities.ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString())
                {
                    case "1": Mem_Status = "A"; break;
                    case "2": Mem_Status = "M"; break;
                    case "3": Mem_Status = "Y"; this.MS_Sel.ReadOnly = false; break;
                }


                if (Set_Mem_On_Combo)//(Pass_MS_Entity.Rec_Type == "I" && !Set_Mem_From_OBO)
                {
                    int Row_index = 0;
                    foreach (DataGridViewRow dr in Members_Grid.Rows)
                    {
                        switch (Mem_Status)
                        {
                            case "A":
                                if (dr.Cells["App_SW"].Value.ToString() == Mem_Status)
                                {
                                    if (dr.Cells["Active_Sw"].Value.ToString() == "A" && dr.Cells["Exclude_Sw"].Value.ToString() == "N")
                                        dr.Cells["MS_Sel"].Value = true;
                                    // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
                                }
                                else
                                    dr.Cells["MS_Sel"].Value = false;
                                break;
                            case "M":
                                if (dr.Cells["Active_Sw"].Value.ToString() == "A" && dr.Cells["Exclude_Sw"].Value.ToString() == "N")
                                    dr.Cells["MS_Sel"].Value = true;
                                break;
                            default:
                                //if (dr.Cells["Active_Sw"].Value.ToString() == "A" && dr.Cells["Exclude_Sw"].Value.ToString() == "N")
                                //    dr.Cells["MS_Sel"].Value = true;
                                break;
                        }
                        Row_index++;
                    }
                }
                else
                    Set_Members_FromCASEMSOBO();
            }

        }


        private void Set_Members_Grid_Auto_Post()
        {
            if (Members_Grid.Rows.Count > 0 && SPM_Site.Length > 0)
            {
                foreach (DataGridViewRow dr in Members_Grid.Rows)
                {
                    if (SPM_Site.Contains(dr.Cells["CLID"].Value.ToString().Trim() + ","))
                        dr.Cells["MS_Sel"].Value = true;
                    else
                        dr.Cells["MS_Sel"].Value = false;
                }

                switch (Pass_MS_Entity.OBF)
                {
                    case "3": this.MS_Sel.ReadOnly = false; break;
                }
            }
        }

        private void Set_Members_FromCASEMSOBO()
        {
            if (CASEMSOBO_List.Count > 0)
            {
                foreach (CASEMSOBOEntity Entity in CASEMSOBO_List)
                {
                    foreach (DataGridViewRow dr in Members_Grid.Rows)
                    {
                        if (Entity.CLID == dr.Cells["CLID"].Value.ToString() &&
                            Entity.Fam_Seq == dr.Cells["Mem_Seq"].Value.ToString())
                        {
                            dr.Cells["MS_Sel"].Value = true;
                            dr.Cells["Is_OBO_Rec"].Value = "Y";
                            break;
                        }
                        //else
                        //{
                        //    dr.Cells["MS_Sel"].Value = false;
                        //    dr.Cells["Is_OBO_Rec"].Value = "N";
                        //    break;
                        //}
                    }
                }
            }
        }

        private void Members_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Members_Grid.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (Members_Grid.CurrentRow.Cells["Active_Sw"].Value.ToString() != "A")
                    {
                        Members_Grid.CurrentRow.Cells["MS_Sel"].Value = false;
                        MessageBox.Show("Member is Inactive", "CAP Systems");
                        return;
                    }

                    if (Members_Grid.CurrentRow.Cells["Exclude_Sw"].Value.ToString() == "Y")
                    {
                        Members_Grid.CurrentRow.Cells["MS_Sel"].Value = false;
                        MessageBox.Show("Member is Excluded", "CAP Systems");
                    }
                }
            }
        }

        //private void Set_Members_CA_Grid_As_Benefit_Change(bool Set_Mem_On_Combo, string OBF_Type)
        //{
        //    if (CA_Members_Grid.Rows.Count > 0)
        //    {
        //        string Mem_Status = "M";
        //        this.CA_Sel.ReadOnly = true;
        //        switch (((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString())
        //        {
        //            case "1": Mem_Status = "A"; break;
        //            case "2": Mem_Status = "M"; break;
        //            case "3": Mem_Status = "Y"; this.CA_Sel.ReadOnly = false; break;
        //        }


        //        if (Set_Mem_On_Combo)//(Pass_MS_Entity.Rec_Type == "I" && !Set_Mem_From_OBO)
        //        {
        //            int Row_index = 0;
        //            foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //            {
        //                switch (Mem_Status)
        //                {
        //                    case "A":
        //                        if (dr.Cells["CA_AppSw"].Value.ToString() == Mem_Status)
        //                        {
        //                            if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                                dr.Cells["CA_Sel"].Value = true;
        //                            // Members_Grid.Rows[Row_index].DefaultCellStyle.ForeColor = Color.Blue;
        //                        }
        //                        else
        //                            dr.Cells["CA_Sel"].Value = false;
        //                        break;
        //                    default:
        //                        if (dr.Cells["CA_Active_Sw"].Value.ToString() == "A" && dr.Cells["CA_Exclude_Sw"].Value.ToString() == "N")
        //                            dr.Cells["CA_Sel"].Value = true;
        //                        break;
        //                }
        //                Row_index++;
        //            }
        //        }
        //        else
        //            Set_Members_FromCAOBO();
        //    }

        //}

        //private void Set_CAMembers_Grid_Auto_Post()
        //{
        //    if (CA_Members_Grid.Rows.Count > 0 && SPM_Site.Length > 0)
        //    {
        //        foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //        {
        //            if (SPM_Site.Contains(dr.Cells["CLID"].Value.ToString().Trim() + ","))
        //                dr.Cells["CA_Sel"].Value = true;
        //            else
        //                dr.Cells["CA_Sel"].Value = false;
        //        }

        //        switch (Pass_CA_Entity.CA_OBF)
        //        {
        //            case "3": this.CA_Sel.ReadOnly = false; break;
        //        }
        //    }
        //}

        //private void Set_Members_FromCAOBO()
        //{
        //    if (CAOBO_List.Count > 0)
        //    {
        //        foreach (CAOBOEntity Entity in CAOBO_List)
        //        {
        //            foreach (DataGridViewRow dr in CA_Members_Grid.Rows)
        //            {
        //                if (Entity.CLID == dr.Cells["CA_CLID"].Value.ToString() &&
        //                    Entity.Fam_Seq == dr.Cells["CA_Mem_Seq"].Value.ToString())
        //                {
        //                    dr.Cells["CA_Sel"].Value = true;
        //                    dr.Cells["Is_CAOBO_Rec"].Value = "Y";
        //                    break;
        //                }
        //                //else
        //                //{
        //                //    dr.Cells["MS_Sel"].Value = false;
        //                //    dr.Cells["Is_OBO_Rec"].Value = "N";
        //                //    break;
        //                //}
        //            }
        //        }
        //    }
        //}


        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();
            if (Cust_Grid.Rows.Count > 0)
            {
                if ((Cust_Grid.CurrentRow.Cells["Type"].Value.ToString() == "C" || Cust_Grid.CurrentRow.Cells["Type"].Value.ToString() == "D"))
                {
                    List<PopUp_Menu_L1_Entity> listItem = new List<PopUp_Menu_L1_Entity>();

                    foreach (CustRespEntity Entity in CustResp_List)
                    {
                        if (Cust_Grid.CurrentRow.Cells["Code"].Value.ToString() == Entity.ResoCode)
                        {
                            MenuItem Resp_Menu = new MenuItem();
                            Resp_Menu.Text = Entity.RespDesc;
                            Resp_Menu.Tag = Entity.DescCode;
                            contextMenu1.MenuItems.Add(Resp_Menu);

                            if (Cust_Grid.CurrentRow.Cells["Resp"].Value.ToString() == Entity.RespDesc)
                                Resp_Menu.Checked = true;
                        }
                    }
                    MenuItem Resp_Menu1 = new MenuItem();
                    Resp_Menu1.Text = "Clear Response";
                    Resp_Menu1.Tag = "CLRRSP";
                    contextMenu1.MenuItems.Add(Resp_Menu1);
                }
            }
        }


        private void set_Cust_Grid_Tooltip(int rowIndex, string Type)
        {
            string Type_Desc = "Not Defined";
            switch (Type)
            {
                case "C": Type_Desc = "Check Box"; break;
                case "D": Type_Desc = "DropDown"; break;
                case "T": Type_Desc = "Date"; break;
                case "N": Type_Desc = "Numeric"; break;
                case "X": Type_Desc = "Text"; break;

            }

            string toolTipText = "Question Type : " + Type_Desc;

            foreach (DataGridViewCell cell in Cust_Grid.Rows[rowIndex].Cells)
                cell.ToolTipText = toolTipText;
        }



        List<CASEREFEntity> CASEREFREF_List = new List<CASEREFEntity>();
        CASEREFSEntity Search_REFS_Entity = new CASEREFSEntity(true);
        private void Get_ReferrTo_Data()
        {
            CASEREFEntity Search_REF_Entity = new CASEREFEntity(true);
            CASEREFREF_List = _model.SPAdminData.Browse_CASEREF(Search_REF_Entity, "Browse");

            //Search_REFS_Entity.Service = Pass_CA_Entity.ACT_Code.Trim();
            //Sel_REFS_List = _model.SPAdminData.Browse_CASEREFS(Search_REFS_Entity, "Browse");
        }

        //private void Fill_ReferrTo_Data()
        //{
        //    bool Ref_Exists = false;
        //    string Active_Stat = "N";
        //    Ref_Grid.Rows.Clear();
        //    int rowIndex = 0;
        //    //foreach (CASEREFSEntity Entity in Sel_REFS_List)
        //    //{
        //    foreach (ACTREFSEntity Entity in ACTREFS_List)
        //    {
        //        Ref_Exists = false;
        //        foreach (CASEREFEntity Entity1 in CASEREFREF_List)
        //        {
        //            Active_Stat = Entity1.Active;
        //            if (Entity1.Code == Entity.Code)
        //            {
        //                Ref_Exists = true;
        //                rowIndex = Ref_Grid.Rows.Add(Entity1.Code, Entity1.Name1.Trim(), Entity1.City, Entity1.State, Entity1.Active);
        //                break;
        //            }
        //        }
        //        if (!Ref_Exists)
        //            rowIndex = Ref_Grid.Rows.Add(Entity.Code, "Not Defined in 'CASEREF'", " ", " ", " ");

        //        if (Active_Stat != "Y")
        //            Ref_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;
        //    }

        //    //}
        //}



        private void Btn_CACancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Pb_Notes_Click(object sender, EventArgs e)
        {
            string Notes_Field_Name = null;

            //if (sender == Pb_CA_Notes)
            //    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan + "CA" + Pass_CA_Entity.Branch + Pass_CA_Entity.Group + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
            //else
            //    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no+ Pass_MS_Entity.Service_plan + "MS" + Pass_MS_Entity.Branch + Pass_MS_Entity.Group + Pass_MS_Entity.MS_Code.Trim();

            //if (sender == Pb_CA_Notes)
            //    Notes_Field_Name = Hierarchy + "    " + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan + Pass_CA_Entity.Branch +"CA"+ Pass_CA_Entity.Group + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
            //else
            //    Notes_Field_Name = Hierarchy + "    " + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan + "MS" + Pass_MS_Entity.Branch + Pass_MS_Entity.Group + Pass_MS_Entity.MS_Code.Trim();

            //if (CAMS_FLG == "CA")
            //    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + ("000000".Substring(0, (6 - Pass_CA_Entity.Service_plan.Length)) + Pass_CA_Entity.Service_plan) + Pass_CA_Entity.SPM_Seq + "CA" + Pass_CA_Entity.Branch +
            //            ("000000".Substring(0, (6 - Pass_CA_Entity.Group.ToString().Length)) + Pass_CA_Entity.Group.ToString()) + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
            //else
            //    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + ("000000".Substring(0, (6 - Pass_MS_Entity.Service_plan.Length)) + Pass_MS_Entity.Service_plan) + Pass_MS_Entity.SPM_Seq + "MS" + Pass_MS_Entity.Branch +
            //            ("000000".Substring(0, (6 - Pass_MS_Entity.Group.ToString().Length)) + Pass_MS_Entity.Group.ToString()) + Pass_MS_Entity.MS_Code.Trim();

            if (CAMS_FLG == "CA")
                Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + Pass_CA_Entity.Branch.Trim() +
                        Pass_CA_Entity.Group.ToString() + "CA" + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq + Pass_CA_Entity.ACT_ID;
            else
                Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan.Trim() + Pass_MS_Entity.SPM_Seq + Pass_MS_Entity.Branch.Trim() +
                        Pass_MS_Entity.Group.ToString() + "MS" + Pass_MS_Entity.MS_Code.Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Pass_MS_Entity.Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

            List<string> list = new List<string>();
            List<CommonEntity> SelEntity = new List<CommonEntity>();
            if (CAMS_FLG == "CA")
            {
                SelEntity.Add(new CommonEntity(Pass_CA_Entity.ACT_Code.Trim(), Lbl_CA_Header.Text.Trim(), Notes_Field_Name, "CA"));
                if (SP_CA_Grid.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in SP_CA_Grid.Rows)
                    {
                        if (dr.Cells["Sel_1"].Value.ToString() == true.ToString())
                        {
                            string Notes = string.Empty;
                            Notes = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + dr.Cells["SP2_Branch"].Value.ToString() +
                                            dr.Cells["SP2_Group"].Value.ToString() + "CA" + dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() + dr.Cells["SP2_CA_Seq"].Value.ToString();

                            //dr.Cells["SP2_CAMS_ID"].Value = New_CAID.ToString();
                            //dr.Cells["SP2_CA_Seq"].Value = New_CA_Seq.ToString();

                            list.Add(Notes);

                            //SelEntity.Add(new CommonEntity(dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim(), dr.Cells["SP2_Desc"].Value.ToString().Trim(), Notes));
                            SelEntity.Add(new CommonEntity(dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim(), dr.Cells["SP2_Desc"].Value.ToString().Trim(), Notes, "CA"));

                        }
                    }
                }
            }
            else
            {
                if (SP_MS_Grid.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in SP_MS_Grid.Rows)
                    {
                        if (dr.Cells["MS_Sel_1"].Value.ToString() == true.ToString())
                        {
                            string Notes = string.Empty;
                            Notes = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan.Trim() + Pass_MS_Entity.SPM_Seq + dr.Cells["MS_SP2_Branch"].Value.ToString() +
                                    dr.Cells["MS_SP2_Group"].Value.ToString() + "MS" + dr.Cells["SP2_MS_Code"].Value.ToString().Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Pass_MS_Entity.Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            //dr.Cells["SP2_CAMS_ID"].Value = New_CAID.ToString();
                            //dr.Cells["SP2_CA_Seq"].Value = New_CA_Seq.ToString();

                            list.Add(Notes);
                        }
                    }
                }
            }

            //if (CAMS_FLG == "CA")
            //    Notes_Field_Name = Hierarchy + "    " + Pass_CA_Entity.App_no + ("000000".Substring(0, (6 - Pass_CA_Entity.Service_plan.Length)) + Pass_CA_Entity.Service_plan) + "CA" + Pass_CA_Entity.Branch +
            //            ("000000".Substring(0, (6 - Pass_CA_Entity.Group.ToString().Length)) + Pass_CA_Entity.Group.ToString()) + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
            //else
            //    Notes_Field_Name = Hierarchy + "    " + Pass_MS_Entity.App_no + ("000000".Substring(0, (6 - Pass_MS_Entity.Service_plan.Length)) + Pass_MS_Entity.Service_plan) + "MS" + Pass_MS_Entity.Branch +
            //            ("000000".Substring(0, (6 - Pass_MS_Entity.Group.ToString().Length)) + Pass_MS_Entity.Group.ToString()) + Pass_MS_Entity.MS_Code.Trim();

            if (SP_CA_Grid.Rows.Count > 0 && SelEntity.Count > 1)
            {
                ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, Mode, Privileges, Notes_Field_Name, list, "QuickPost", SelEntity);
                Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                Prog_Form.ShowDialog();
            }
            else
            {
                ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, Mode, Privileges, Notes_Field_Name);
                Prog_Form.FormClosed += new FormClosedEventHandler(On_PROGNOTES_Closed);
                Prog_Form.ShowDialog();
            }

            //ProgressNotes_Form Prog_Form = new ProgressNotes_Form(BaseForm, Mode, Privileges, Notes_Field_Name);
            //Prog_Form.FormClosed += new Form.FormClosedEventHandler(On_PROGNOTES_Closed);
            //Prog_Form.ShowDialog();
        }


        private void Get_PROG_Notes_Status()
        {
            if (Mode.Equals("Edit"))
            {
                List<CaseNotesEntity> caseNotesEntity = new List<CaseNotesEntity>();
                string Notes_Field_Name = null;

                //if (CAMS_FLG == "CA")
                //    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan + "CA" + Pass_CA_Entity.Branch + Pass_CA_Entity.Group + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
                //else
                //    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan + "MS" + Pass_MS_Entity.Branch + Pass_MS_Entity.Group + Pass_MS_Entity.MS_Code.Trim();


                //if (CAMS_FLG == "CA")
                //    Notes_Field_Name = Hierarchy + "    " + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan + "CA" + Pass_CA_Entity.Branch + Pass_CA_Entity.Group + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
                //else
                //    Notes_Field_Name = Hierarchy + "    " + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan + "MS" + Pass_MS_Entity.Branch + Pass_MS_Entity.Group + Pass_MS_Entity.MS_Code.Trim();

                //if (CAMS_FLG == "CA")
                //    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + ("000000".Substring(0, (6 - Pass_CA_Entity.Service_plan.Length)) + Pass_CA_Entity.Service_plan) + Pass_CA_Entity.SPM_Seq + "CA" + Pass_CA_Entity.Branch +
                //            ("000000".Substring(0, (6 - Pass_CA_Entity.Group.ToString().Length)) + Pass_CA_Entity.Group.ToString()) + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq;
                //else
                //    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + ("000000".Substring(0, (6 - Pass_MS_Entity.Service_plan.Length)) + Pass_MS_Entity.Service_plan) +  Pass_MS_Entity.SPM_Seq + "MS" + Pass_MS_Entity.Branch +
                //            ("000000".Substring(0, (6 - Pass_MS_Entity.Group.ToString().Length)) + Pass_MS_Entity.Group.ToString()) + Pass_MS_Entity.MS_Code.Trim();

                if (CAMS_FLG == "CA")
                    Notes_Field_Name = Hierarchy + Pass_CA_Entity.Year + Pass_CA_Entity.App_no + Pass_CA_Entity.Service_plan.Trim() + Pass_CA_Entity.SPM_Seq + Pass_CA_Entity.Branch.Trim() +
                            Pass_CA_Entity.Group.ToString() + "CA" + Pass_CA_Entity.ACT_Code.Trim() + Pass_CA_Entity.ACT_Seq + Pass_CA_Entity.ACT_ID;
                else
                    Notes_Field_Name = Hierarchy + Pass_MS_Entity.Year + Pass_MS_Entity.App_no + Pass_MS_Entity.Service_plan.Trim() + Pass_MS_Entity.SPM_Seq + Pass_MS_Entity.Branch.Trim() +
                            Pass_MS_Entity.Group.ToString() + "MS" + Pass_MS_Entity.MS_Code.Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Pass_MS_Entity.Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat); ;


                caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName((CAMS_FLG == "CA" ? "CASE00063" : "CASE00064"), Notes_Field_Name.Trim());
                Pb_CA_Notes.ImageSource = Consts.Icons.ico_CaseNotes_New;
                Pb_MS_Notes.ImageSource = Consts.Icons.ico_CaseNotes_New;

                if (caseNotesEntity.Count > 0)
                {

                    switch (CAMS_FLG)
                    {
                        case "CA":
                            if (Pass_CA_Entity.Rec_Type == "I")
                                Pb_CA_Notes.Visible = true;
                            break;

                        case "MS":
                            if (Pass_MS_Entity.Rec_Type == "I")
                                Pb_MS_Notes.Visible = true;
                            break;
                    }
                    Pb_CA_Notes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                    Pb_MS_Notes.ImageSource = Consts.Icons.ico_CaseNotes_View;
                }
            }
        }


        private void On_PROGNOTES_Closed(object sender, FormClosedEventArgs e)
        {
            string SelRef_Name = null;

            ProgressNotes_Form form = sender as ProgressNotes_Form;

            switch (CAMS_FLG)
            {
                case "CA":
                    if (Pass_CA_Entity.Rec_Type == "I")                   // in Add mode if user Clicks Cancel in P.Notes Sub form 
                        this.DialogResult = DialogResult.OK;            //             in that case we need to set the result    Yeswanth
                    break;

                case "MS":
                    if (Pass_MS_Entity.Rec_Type == "I")
                        this.DialogResult = DialogResult.OK;
                    break;
            }

            if (form.DialogResult == DialogResult.OK)
            {
                Get_PROG_Notes_Status();

                this.DialogResult = DialogResult.OK;

                switch (CAMS_FLG)
                {
                    case "CA":
                        if (Pass_CA_Entity.Rec_Type == "I")
                            this.Close(); break;

                    case "MS":
                        if (Pass_MS_Entity.Rec_Type == "I")
                            this.Close(); break;
                }
            }
        }

        private void Act_Date_LostFocus(object sender, EventArgs e)
        {
            if (Act_Date.Checked)
                _errorProvider.SetError(Act_Date, null);

        }

        private void Cmb_MS_Results_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)Cmb_MS_Results.SelectedItem).Value.ToString() != "0")
                _errorProvider.SetError(Cmb_MS_Results, null);
        }

        private void Cmb_MS_Worker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)Cmb_MS_Worker.SelectedItem).Value.ToString() != "0")
                _errorProvider.SetError(Cmb_MS_Worker, null);

        }

        private void MS_Date_LostFocus(object sender, EventArgs e)
        {
            if (MS_Date.Checked)
                _errorProvider.SetError(MS_Date, null);
        }

        public CASEACTEntity GetEdited_CA_Entity()
        {
            CASEACTEntity Edited_CA_Entity = new CASEACTEntity();
            Edited_CA_Entity = Pass_CA_Entity;

            return Edited_CA_Entity;
        }


        public CASEMSEntity GetEdited_MS_Entity()
        {
            CASEMSEntity Edited_MS_Entity = new CASEMSEntity();
            Edited_MS_Entity = Pass_MS_Entity;

            return Edited_MS_Entity;
        }

        private void Txt_Cost_LostFocus(object sender, EventArgs e)
        {

            if (Txt_Cost.Text.Length > 7)
            {
                if (Txt_Cost.Text.Substring(0, 7).Contains("."))
                    Txt_Cost.Text.Substring(0, 7).Replace(".", "");

                Txt_Cost.Text = Txt_Cost.Text.Substring(0, 7) + "." + Txt_Cost.Text.Substring(7, (Txt_Cost.Text.Length - 7));
            }
        }

        private void Cust_Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal number;
            DateTime Compare_Date = DateTime.Today;

            switch (Cust_Grid.CurrentRow.Cells["Type"].Value.ToString())
            {
                case "N":
                    if ((!(Decimal.TryParse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number))) &&
                    !(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())))
                    {
                        MessageBox.Show("Please Enter Decimal Response", "CAP Systems", MessageBoxButtons.OK);
                        Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    break;

                case "X":
                case "A":
                    if ((string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Trim())))
                    {
                        MessageBox.Show("Please Provide Valid Data", "CAP Systems", MessageBoxButtons.OK);
                        Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    break;

                case "T":
                    //if (!(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())) &&
                    //      (!(Convert.ToDateTime(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out DateTime.Today))))
                    //{
                    //    MessageBox.Show("Please Provide Valid Data", "CAP Systems", MessageBoxButtons.OK);
                    //    Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    //}
                    if (!(string.IsNullOrEmpty(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString())))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), Consts.StaticVars.DateFormatMMDDYYYY))
                        {
                            try
                            {
                                if (DateTime.Parse(Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < Convert.ToDateTime("01/01/1800"))
                                {
                                    MessageBox.Show("01/01/1800 below date not except", "CAP Systems");
                                    Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                                }
                                else
                                    MessageBox.Show(Consts.Messages.PleaseEntermmddyyyyDateFormat, "CAP Systems");
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Please Enter Valid Date Format MM/DD/YYYY", "CAP Systems");
                                Cust_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                            }
                        }
                    }
                    break;
            }
        }

        private void CASE3006_CAMSForm_Load(object sender, EventArgs e)
        {
            if (CAMS_FLG == "MS")
            {

                if (!CntlMSEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                    Btn_MS_Save.Enabled = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(SP_Header_Rec.Status))
                        MessageBox.Show("Please Contact Administrator!!! \n No Result is associated to this SP in ADMN0020", "CAP Systems");
                }

                //if (Pass_MS_Entity.Rec_Type == "U")
                //{
                //    //MS_Date.Enabled = false;
                //    Cmb_MS_Site.Focus();
                //}
                //else
                //{
                MS_Date.Focus();
                //}
            }
            else if (CAMS_FLG == "CA")
            {
                if (!CntlCAEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    CommonFunctions.MessageBoxDisplay("Field controls not defined for this program");
                    Btn_CASave.Enabled = false;
                }
                //else      // Brain asked to comment this on 12062013
                //{   if (string.IsNullOrEmpty(SP_Header_Rec.Funds))
                //    MessageBox.Show("Please Contact Administrator!!! \n No Fund Source is associated to this SP in ADMN0020", "CAP Systems");
                //}

                //if (Pass_CA_Entity.Rec_Type == "U")
                //{
                //    //Act_Date.Enabled = false;
                //    CmbWorker.Focus();
                //}
                //else
                //{
                Act_Date.Focus();
                //}
            }

            if (CAMS_FLG == "Bulk_Posting")
            {
                if (OBF_MS_Cnt > 1)
                    Cb_Use_for_All.Visible = true;

                //if (OBF_Template_SW == "Y")
                //    Cb_Use_for_All.Checked = true;

                foreach (Captain.Common.Utilities.ListItem list in OBF_Type3_Sel_Members)
                {
                    if (list.Text.ToString() == "Sel_Sw" && list.Value.ToString() == "Y")
                    {
                        Cb_Use_for_All.Checked = true;
                        break;
                    }
                }
            }
        }


        bool Future_Date_Soft_Edit = false;
        private bool isValidate()
        {
            bool isValid = true;
            if (CAMS_FLG == "CA")
            {
                if (lblActivityDateReq.Visible)
                {
                    if (Act_Date.Checked == false)
                    {
                        _errorProvider.SetError(Act_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActivityDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {

                        bool Future_Date_Flg = false;
                        if (Act_Date.Value > DateTime.Today && !Future_Date_Soft_Edit)
                        {
                            //MessageBox.Show("' " + lblActivityDate.Text + "' Should not be Future Date", "CAPSYSTEMS");

                            MessageBox.Show("You are about to post a future date. \n Do you want to proceed?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Allow_Post_Future_Date);

                            //_errorProvider.SetError(Act_Date, string.Format("' " + lblActivityDate.Text + "' Should not be Future Date".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                            Future_Date_Flg = true;
                        }
                        else
                        {
                            Future_Date_Soft_Edit = false;
                            _errorProvider.SetError(Act_Date, null);
                        }


                        if (!Future_Date_Flg)
                        {
                            if (!string.IsNullOrEmpty(Sp_Start_Date.Trim()))
                            {
                                if (string.IsNullOrEmpty(Sp_End_Date.Trim()))
                                {
                                    if (Act_Date.Value < Convert.ToDateTime(Sp_Start_Date))
                                    {
                                        _errorProvider.SetError(Act_Date, string.Format("' " + lblActivityDate.Text + "' Should not be Prior to the 'Service Plan Master Date'".Replace(Consts.Common.Colon, string.Empty)));
                                        isValid = false;
                                    }
                                    else
                                        _errorProvider.SetError(Act_Date, null);
                                }
                                else
                                {
                                    if (Act_Date.Value < Convert.ToDateTime(Sp_Start_Date) || Act_Date.Value > Convert.ToDateTime(Sp_End_Date))
                                    {
                                        _errorProvider.SetError(Act_Date, string.Format("' The " + lblActivityDate.Text + "' Should not be Prior to the 'Service Plan Start Date' OR after the 'Actual Completion Date'".Replace(Consts.Common.Colon, string.Empty)));
                                        isValid = false;
                                    }
                                    else
                                        _errorProvider.SetError(Act_Date, null);
                                }

                            }
                            else
                                _errorProvider.SetError(Act_Date, null);
                        }
                    }
                }
                else
                    _errorProvider.SetError(Act_Date, null);

                if (lblFunSour1Req.Visible && (CmbFunding1.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbFunding1.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(CmbFunding1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFunSour1.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbFunding1, null);

                if (lblFunSour2Req.Visible && (CmbFunding2.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbFunding2.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(CmbFunding2, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFunSour2.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbFunding2, null);

                if (lblFunSour3Req.Visible && (CmbFunding3.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbFunding3.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(CmbFunding3, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFunSour3.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbFunding3, null);

                if (lblCaseworcaReq.Visible && (CmbWorker.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbWorker.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(CmbWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseworca.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbWorker, null);

                if (lblSitecaReq.Visible && (CmbSite.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbSite.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(CmbSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSiteca.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(CmbSite, null);

                if (lblCheckDateReq.Visible && Check_Date.Checked == false)
                {
                    _errorProvider.SetError(Check_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCheckDate.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Check_Date, null);

                if (lblVendorReq.Visible && string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
                {
                    _errorProvider.SetError(Text_VendName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblVendor.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Text_VendName, null);

                if (lblCostReq.Visible && string.IsNullOrEmpty(Txt_Cost.Text.Trim()))
                {
                    _errorProvider.SetError(Txt_Cost, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCost.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Txt_Cost, null);

                if (lblFollowupComplReq.Visible && Complete_Date.Checked == false)
                {
                    _errorProvider.SetError(Complete_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowupCompl.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Complete_Date, null);

                if (lblFollowupReq.Visible && UpOn_Date.Checked == false)
                {
                    _errorProvider.SetError(UpOn_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFollowup.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(UpOn_Date, null);

                if (lblTobeFolledReq.Visible && string.IsNullOrEmpty(Txt_TobeFollowUp.Text.Trim()))
                {
                    _errorProvider.SetError(Txt_TobeFollowUp, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTobeFolledReq.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Txt_TobeFollowUp, null);

                if (lblCheckReq.Visible && string.IsNullOrEmpty(Txtx_ChkNo.Text.Trim()))
                {
                    _errorProvider.SetError(Txtx_ChkNo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCheck.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Txtx_ChkNo, null);




                //if (Txtx_ChkNo.Enabled)
                //{
                //    if (string.IsNullOrEmpty(Txtx_ChkNo.Text.Trim()))
                //    {
                //        _errorProvider.SetError(Txtx_ChkNo, lblCheck.Text + " May Not be zero.");
                //        isValid = false;
                //    }
                //    else if ((int.Parse(Txtx_ChkNo.Text) <= 0))
                //        {
                //            _errorProvider.SetError(Txtx_ChkNo, lblCheck.Text + " May Not be zero.");
                //            isValid = false;
                //        }
                //    else
                //        _errorProvider.SetError(Txtx_ChkNo, null);
                //}

                //if (LblProgramReq.Visible && (Cmb_Program.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)Cmb_Program.SelectedItem).Text.Trim()))))
                //{
                //    _errorProvider.SetError(Cmb_Program, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text));
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(Cmb_Program, null);


                if (LblProgramReq.Visible && ((string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()))))
                {
                    _errorProvider.SetError(panel8, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblProgram.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(panel8, null);


                if (LblUOM_Req.Visible && (Cmb_UOM.SelectedItem == null || (string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)Cmb_UOM.SelectedItem).Text.Trim()))))
                {
                    _errorProvider.SetError(Cmb_UOM, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), Lbl_UOM.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Cmb_UOM, null);

                if (LblUnits_Req.Visible && (string.IsNullOrEmpty(Txt_Units.Text.Trim())))
                {
                    _errorProvider.SetError(Txt_Units, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), Lbl_Units.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Txt_Units, null);

                if (lblActSeekDateReq.Visible && dtActSeek_Date.Checked == false)
                {
                    _errorProvider.SetError(dtActSeek_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblActSeekDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtActSeek_Date, null);

                if (dtActSeek_Date.Visible)
                {

                    if (dtActSeek_Date.Checked && Act_Date.Checked)
                    {
                        if (Act_Date.Value < dtActSeek_Date.Value)
                        {
                            _errorProvider.SetError(dtActSeek_Date, "Please The Activity/Service Requested Date never Greater than Activity/Service Date");
                            isValid = false;
                        }
                        else
                            _errorProvider.SetError(dtActSeek_Date, null);
                    }
                }

                foreach (DataGridViewRow dataGridViewRow in Cust_Grid.Rows)
                {

                    if (dataGridViewRow.Cells["CA_Cust_Req"].Value.ToString() == "*")
                    {
                        string inputValue = string.Empty;
                        inputValue = dataGridViewRow.Cells["Resp"].Value != null ? dataGridViewRow.Cells["Resp"].Value.ToString() : string.Empty;
                        if (inputValue.Trim() == string.Empty)
                        {
                            //CommonFunctions.MessageBoxDisplay("Please Provide Response for Required Custom Question(s)");
                            CommonFunctions.MessageBoxDisplay("Custom Question requires a response");
                            isValid = false;
                            break;
                        }
                    }
                }

            }
            else
            {
                //if (lblMsDateReq.Visible && (MS_Date.Checked == false))
                //{
                //    _errorProvider.SetError(MS_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(MS_Date, null);

                if (lblMsDateReq.Visible)
                {
                    if (MS_Date.Checked == false)
                    {
                        _errorProvider.SetError(MS_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {

                        bool Future_Date_Flg = false;
                        if (MS_Date.Value > DateTime.Today && !Future_Date_Soft_Edit)
                        {
                            MessageBox.Show("You are about to post a future date. \n Do you want to proceed?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Allow_Post_Future_Date);
                            //_errorProvider.SetError(MS_Date, string.Format("' " + lblMsDate.Text + "' Should not be Future Date".Replace(Consts.Common.Colon, string.Empty)));
                            isValid = false;
                            Future_Date_Flg = true;
                        }
                        else
                        {
                            Future_Date_Soft_Edit = false;
                            _errorProvider.SetError(Act_Date, null);
                        }


                        if (!Future_Date_Flg)
                        {
                            if (!string.IsNullOrEmpty(Sp_Start_Date.Trim()))
                            {
                                if (string.IsNullOrEmpty(Sp_End_Date.Trim()))
                                {
                                    if (MS_Date.Value < Convert.ToDateTime(Sp_Start_Date))
                                    {
                                        _errorProvider.SetError(MS_Date, string.Format("' " + lblMsDate.Text + "' Should not be Prior to the 'Service Plan Master Date'".Replace(Consts.Common.Colon, string.Empty)));
                                        isValid = false;
                                    }
                                    else
                                        _errorProvider.SetError(MS_Date, null);
                                }
                                else
                                {
                                    if (MS_Date.Value < Convert.ToDateTime(Sp_Start_Date) || MS_Date.Value > Convert.ToDateTime(Sp_End_Date))
                                    {
                                        _errorProvider.SetError(MS_Date, string.Format("' The " + lblMsDate.Text + "' Should not be Prior to the 'Service Plan Start Date' OR after the 'Actual Completion Date'".Replace(Consts.Common.Colon, string.Empty)));
                                        isValid = false;
                                    }
                                    else
                                        _errorProvider.SetError(MS_Date, null);
                                }
                            }
                            else
                                _errorProvider.SetError(MS_Date, null);
                        }
                    }
                }
                else
                    _errorProvider.SetError(MS_Date, null);


                if (lblResultReq.Visible && ((Captain.Common.Utilities.ListItem)Cmb_MS_Results.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(Cmb_MS_Results, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblResult.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Cmb_MS_Results, null);

                if (lblMsCaseworReq.Visible && ((Captain.Common.Utilities.ListItem)Cmb_MS_Worker.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(Cmb_MS_Worker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsCasewor.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Cmb_MS_Worker, null);

                //if (lblBenefitReq.Visible && ((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "0")
                //{
                //    _errorProvider.SetError(Cmb_MS_Benefit, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBenefit.Text.Replace(Consts.Common.Colon, string.Empty)));
                //    isValid = false;
                //}
                //else
                //{
                bool AtLeast_One_Mem_Selected = true;
                //if (lblBenefitReq.Visible && ((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "3")
                //if (((ListItem)Cmb_MS_Benefit.SelectedItem).Value.ToString() == "3")
                {
                    AtLeast_One_Mem_Selected = false;
                    foreach (DataGridViewRow dr in Members_Grid.Rows)
                    {
                        if (dr.Cells["MS_Sel"].Value.ToString() == true.ToString())
                        {
                            AtLeast_One_Mem_Selected = true;
                            break;
                        }
                    }
                }

                if (AtLeast_One_Mem_Selected)
                    _errorProvider.SetError(Cmb_MS_Benefit, null);
                else
                {
                    _errorProvider.SetError(Cmb_MS_Benefit, "Select Atleast One Household Member".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                //}

                if (lblSiteReq.Visible && ((Captain.Common.Utilities.ListItem)Cmb_MS_Site.SelectedItem).Value.ToString() == "0")
                {
                    _errorProvider.SetError(Cmb_MS_Site, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblBenefit.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(Cmb_MS_Site, null);

                //if (lblMSProgramReq.Visible && (Cmb_MS_Program.SelectedItem == null || (string.IsNullOrEmpty(((ListItem)Cmb_MS_Program.SelectedItem).Text.Trim())))) // 
                //{
                //    _errorProvider.SetError(Cmb_MS_Program, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMSProgram.Text));
                //    isValid = false;
                //}
                //else
                //    _errorProvider.SetError(Cmb_MS_Program, null);

                if (lblMSProgramReq.Visible && ((string.IsNullOrEmpty(Txt_MS_Program.Text.Trim()))))
                {
                    _errorProvider.SetError(panel9, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMSProgram.Text));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(panel9, null);
                if (lblMSSeekDateReq.Visible && dtMSSeek_Date.Checked == false)
                {
                    _errorProvider.SetError(dtMSSeek_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblMsSeekDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;

                }
                else
                    _errorProvider.SetError(dtMSSeek_Date, null);

                if (dtMSSeek_Date.Visible)
                {
                    if (dtMSSeek_Date.Checked && MS_Date.Checked)
                    {
                        if (MS_Date.Value < dtMSSeek_Date.Value)
                        {
                            _errorProvider.SetError(dtMSSeek_Date, "Please The Estimated MS/Outcome Date never Greater than MS/Outcome Date");
                            isValid = false;
                        }
                        else
                            _errorProvider.SetError(dtMSSeek_Date, null);
                    }
                }

            }
            return isValid;
        }

        //bool Vendor_Enable_Status = false;
        private void EnableDisableControls()
        {
            if (CAMS_FLG == "MS")
            {


                if (!CntlMSEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    //MessageBox.Show("Field controls not defined for this program");
                    Btn_MS_Save.Enabled = false;
                }

                foreach (FldcntlHieEntity entity in CntlMSEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE0006.Date:
                            if (enabled) { MS_Date.Enabled = lblMsDate.Enabled = true; if (required) lblMsDateReq.Visible = true; } else { MS_Date.Enabled = lblMsDate.Enabled = false; lblMsDateReq.Visible = false; }
                            break;
                        case Consts.CASE0006.MsSite:
                            if (enabled) { lblSite.Enabled = Cmb_MS_Site.Enabled = true; if (required) lblSiteReq.Visible = true; } else { lblSite.Enabled = lblSiteReq.Enabled = false; Cmb_MS_Site.Visible = false; }
                            break;
                        case Consts.CASE0006.Result:
                            if (enabled) { lblResult.Enabled = Cmb_MS_Results.Enabled = true; if (required) lblResultReq.Visible = true; } else { Cmb_MS_Results.Enabled = lblResult.Enabled = false; lblResultReq.Visible = false; }
                            break;

                        case Consts.CASE0006.MsCaseWorker:
                            if (enabled) { Cmb_MS_Worker.Enabled = lblMsCasewor.Enabled = true; if (required) lblMsCaseworReq.Visible = true; } else { Cmb_MS_Worker.Enabled = lblMsCasewor.Enabled = false; lblMsCaseworReq.Visible = false; }
                            break;
                        case Consts.CASE0006.MS_Acty_Program:
                            if (enabled) { Pb_MS_Prog.Visible = lblMSProgram.Enabled = true; if (required) lblMSProgramReq.Visible = true; } else { Pb_MS_Prog.Visible = Pb_MS_Prog.Visible = lblMSProgram.Enabled = false; lblMSProgramReq.Visible = false; }
                            break;
                        case Consts.CASE0006.BenefitingfromServiceActivity:
                            if (enabled) { Cmb_MS_Benefit.Enabled = lblBenefit.Enabled = true; if (required) lblBenefitReq.Visible = true; } else { Cmb_MS_Benefit.Enabled = lblBenefit.Enabled = false; lblBenefitReq.Visible = false; }
                            break;
                        //case Consts.CASE0006.BenefitingfromCAServiceActivity:
                        //    if (enabled) { cmb_CA_Benefit.Enabled = label18.Enabled = true; if (required) label17.Visible = true; } else { cmb_CA_Benefit.Enabled = label18.Enabled = false; label17.Visible = false; }
                        //    break;
                        case Consts.CASE0006.MS_Seek_Date:
                            if (enabled) { dtMSSeek_Date.Visible = lblMsSeekDate.Visible = true; if (required) lblMSSeekDateReq.Visible = true; } else { dtMSSeek_Date.Visible = lblMsSeekDate.Visible = false; lblMSSeekDateReq.Visible = false; }
                            break;
                    }
                }
            }
            else
            {
                if (!CntlCAEntity.Exists(u => u.Enab.Equals("Y")))
                {
                    //MessageBox.Show("Field controls not defined for this program");
                    Btn_CASave.Enabled = false;
                }

                foreach (FldcntlHieEntity entity in CntlCAEntity)
                {
                    bool required = entity.Req.Equals("Y") ? true : false;
                    bool enabled = entity.Enab.Equals("Y") ? true : false;

                    switch (entity.FldCode)
                    {
                        case Consts.CASE0006.FundingSource1:
                            if (enabled) { lblFunSour1.Enabled = CmbFunding1.Enabled = true; if (required) lblFunSour1Req.Visible = true; } else { lblFunSour1.Enabled = CmbFunding1.Enabled = false; lblFunSour1Req.Visible = false; }
                            break;
                        case Consts.CASE0006.FundingSource2:
                            if (enabled) { lblFunSour2.Enabled = CmbFunding2.Enabled = true; if (required) lblFunSour2Req.Visible = true; } else { lblFunSour2.Enabled = CmbFunding2.Enabled = false; lblFunSour2Req.Visible = false; }
                            break;
                        case Consts.CASE0006.FundingSource3:
                            if (enabled) { lblFunSour3.Enabled = CmbFunding3.Enabled = true; if (required) lblFunSour3Req.Visible = true; } else { lblFunSour3.Enabled = CmbFunding3.Enabled = false; lblFunSour3Req.Visible = false; }
                            break;
                        case Consts.CASE0006.ActCaseWorker:
                            if (enabled) { CmbWorker.Enabled = lblCaseworca.Enabled = true; if (required) lblCaseworcaReq.Visible = true; } else { CmbWorker.Enabled = lblCaseworca.Enabled = false; lblCaseworcaReq.Visible = false; }
                            break;
                        case Consts.CASE0006.Site:
                            if (enabled) { CmbSite.Enabled = lblSiteca.Enabled = true; if (required) lblSitecaReq.Visible = true; } else { CmbSite.Enabled = lblSiteca.Enabled = false; lblSitecaReq.Visible = false; }
                            break;
                        case Consts.CASE0006.VendorNo:
                            if (enabled) { Txt_VendNo.Enabled = Text_VendName.Enabled = lblVendor.Enabled = panel_Referral2.Visible = true; if (required) lblVendorReq.Visible = true; } else { Txt_VendNo.Enabled = Text_VendName.Enabled = lblVendor.Enabled = panel_Referral2.Visible = false; lblVendorReq.Visible = false; }
                            break;
                        case Consts.CASE0006.CheckNo:
                            if (enabled) { Txtx_ChkNo.Enabled = lblCheck.Enabled = true; if (required) lblCheckReq.Visible = true; } else { Txtx_ChkNo.Enabled = lblCheck.Enabled = false; lblCheckReq.Visible = false; }
                            break;
                        case Consts.CASE0006.CheckDate:
                            if (enabled) { Check_Date.Enabled = lblCheckDate.Enabled = true; if (required) lblCheckDateReq.Visible = true; } else { Check_Date.Enabled = lblCheckDate.Enabled = false; lblCheckDateReq.Visible = false; }
                            break;
                        case Consts.CASE0006.TobeFollowUpBy:
                            if (enabled) { Txt_TobeFollowUp.Enabled = lblTobeFolled.Enabled = true; if (required) lblTobeFolledReq.Visible = true; } else { Txt_TobeFollowUp.Enabled = lblTobeFolled.Enabled = false; lblTobeFolledReq.Visible = false; }
                            break;
                        case Consts.CASE0006.FollowUPOn:
                            if (enabled) { UpOn_Date.Enabled = lblFollowup.Enabled = true; if (required) lblFollowupReq.Visible = true; } else { UpOn_Date.Enabled = lblFollowup.Enabled = false; lblFollowupReq.Visible = false; }
                            break;
                        case Consts.CASE0006.FollowUpComplete:
                            if (enabled) { Complete_Date.Enabled = lblFollowupCompl.Enabled = true; if (required) lblFollowupComplReq.Visible = true; } else { Complete_Date.Enabled = lblFollowupCompl.Enabled = false; lblFollowupComplReq.Visible = false; }
                            break;
                        case Consts.CASE0006.Cost:
                            if (enabled) { Txt_Cost.Enabled = lblCost.Enabled = true; if (required) lblCostReq.Visible = true; } else { Txt_Cost.Enabled = lblCost.Enabled = false; lblCostReq.Visible = false; }
                            break;
                        //case Consts.CASE0006.Act_Acty_Program:
                        //    if (enabled) { Cmb_Program.Enabled = LblProgram.Enabled = true; if (required) LblProgramReq.Visible = true; } else { Cmb_Program.Enabled = LblProgram.Enabled = false; LblProgramReq.Visible = false; }
                        case Consts.CASE0006.Act_Acty_Program:
                            if (enabled) { Pb_CA_Prog.Visible = LblProgram.Enabled = true; if (required) LblProgramReq.Visible = true; } else { Pb_CA_Prog.Visible = Pb_CA_Prog.Visible = LblProgram.Enabled = false; LblProgramReq.Visible = false; }
                            break;
                        case Consts.CASE0006.Act_UOM:
                            //if (enabled) { Cmb_UOM.Enabled = Lbl_UOM.Enabled = true; if (required) LblUOM_Req.Visible = true; } else { Cmb_UOM.Enabled = Lbl_UOM.Enabled = false; LblUOM_Req.Visible = false; }
                            if (enabled) { Lbl_UOM.Visible = Cmb_UOM.Visible = Lbl_UOM.Visible = Cmb_UOM.Enabled = Lbl_UOM.Enabled = true; if (required) LblUOM_Req.Visible = true; } else { Lbl_UOM.Visible = Cmb_UOM.Visible = Lbl_UOM.Visible = Cmb_UOM.Enabled = Lbl_UOM.Enabled = false; LblUOM_Req.Visible = false; }
                            break;
                        case Consts.CASE0006.Act_Units:
                            //if (enabled) { Txt_Units.Enabled = Lbl_Units.Enabled = true; if (required) LblUnits_Req.Visible = true; } else { Txt_Units.Enabled = Lbl_Units.Enabled = false; LblUnits_Req.Visible = false; }
                            if (enabled) { Lbl_Units.Visible = Txt_Units.Visible = Lbl_Units.Visible = Txt_Units.Enabled = Lbl_Units.Enabled = true; if (required) LblUnits_Req.Visible = true; } else { Lbl_Units.Visible = Txt_Units.Visible = Lbl_Units.Visible = Txt_Units.Enabled = Lbl_Units.Enabled = false; LblUnits_Req.Visible = false; }
                            break;
                        case Consts.CASE0006.Act_Seek_Date:
                            if (enabled) { dtActSeek_Date.Visible = lblActSeekDate.Visible = true; if (required) lblActSeekDateReq.Visible = true; } else { dtActSeek_Date.Visible = lblActSeekDate.Visible = false; lblActSeekDateReq.Visible = false; }
                            break;
                            //case Consts.CASE2001.InitialDate:
                            //    if (enabled) { dtpInitialDate.Enabled = lblInitialDate.Enabled = true; if (required) lblInitialDateReq.Visible = true; } else { dtpInitialDate.Enabled = lblInitialDate.Enabled = false; lblInitialDateReq.Visible = false; }
                            //    break;

                    }
                }

            }
        }

        private void PbVendor_Click(object sender, EventArgs e)
        {
            VendBrowseForm Vendor_Browse = new VendBrowseForm(BaseForm, Privileges, "**");
            Vendor_Browse.FormClosed += new FormClosedEventHandler(On_Vendor_Browse_Closed);
            Vendor_Browse.ShowDialog();

        }

        private void On_Vendor_Browse_Closed(object sender, FormClosedEventArgs e)
        {
            VendBrowseForm form = sender as VendBrowseForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Vendor_Details = new string[2];
                Vendor_Details = form.Get_Selected_Vendor();

                Txt_VendNo.Text = Vendor_Details[0].Trim();
                Text_VendName.Text = Vendor_Details[1].Trim();
            }
        }

        private void Hepl_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0006_Add");
        }

        private void MS_Help_Click(object sender, EventArgs e)
        {
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "CASE0006_Add");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Fill_Bulk_App_Grid_To_Post()
        {
            int rowIndex = 0;
            Post_App_Grid.Rows.Clear();
            List<SP_Bulk_Post_Entity> CASESPM_List = new List<SP_Bulk_Post_Entity>();
            CASESPMEntity Search_Entity = new CASESPMEntity();

            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;

            //Search_Entity.year = MainMenuYear;        
            Search_Entity.year = BaseForm.BaseYear;                // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no =
            Search_Entity.service_plan = Search_Entity.caseworker = Search_Entity.site =
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate =
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc =
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator =
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = null;

            CASESPM_List = _model.SPAdminData.Browse_CASESPM_4Bulk_Posting(Search_Entity, "Browse", BaseForm.BaseApplicationNo, "CASESPM");

            string App_Name = string.Empty, App_Address = string.Empty, Start_Date = string.Empty;
            foreach (SP_Bulk_Post_Entity Entity in CASESPM_List)
            {
                App_Name = App_Address = Start_Date = " ";
                App_Name = LookupDataAccess.GetMemberName(Entity.SNP_First_Name, Entity.SNP_Middle_Name, Entity.SNP_Last_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                App_Address = Entity.Mst_Hno + ' ' + Entity.MST_Street + ' ' + Entity.MST_City + ' ' + Entity.MST_State + ' ' + Entity.MST_Zip;
                if (!string.IsNullOrEmpty(Entity.startdate.Trim()))
                    Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.startdate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                rowIndex = Post_App_Grid.Rows.Add(false, Entity.MST_app_no, App_Name, App_Address, Entity.site, Start_Date);
            }
        }

        private void Fill_Case_Type_Combo()
        {
            List<CommonEntity> CaseType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            foreach (CommonEntity casetype in CaseType)
            {
                Captain.Common.Utilities.ListItem li = new Captain.Common.Utilities.ListItem(casetype.Desc, casetype.Code);
                Cmb_Bulk_CaseType.Items.Add(li);
            }
            Cmb_Bulk_CaseType.Items.Insert(0, new Captain.Common.Utilities.ListItem("All Case Types", "0"));
            Cmb_Bulk_CaseType.SelectedIndex = 0;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Txtx_ChkNo_LostFocus(object sender, EventArgs e)
        {
            if (Txtx_ChkNo.Enabled)
            {
                if (string.IsNullOrEmpty(Txtx_ChkNo.Text.Trim()))
                    MessageBox.Show(lblCheck.Text + " May Not be Zero or Empty", "CAP Systems");
                else if ((int.Parse(Txtx_ChkNo.Text) <= 0))
                {
                    MessageBox.Show(lblCheck.Text + " May Not be zero or less", "CAP Systems");
                    Txtx_ChkNo.Text = "";
                }
            }
        }

        //CASE0006_Bulk_Posting



        private void Pb_Prog_Click(object sender, EventArgs e)
        {
            string Sel_Prog = (CAMS_FLG == "CA" ? (!string.IsNullOrEmpty(Txt_CA_Program.Text.Trim()) ? Txt_CA_Program.Text.Substring(0, 6) : "") : (!string.IsNullOrEmpty(Txt_MS_Program.Text.Trim()) ? Txt_MS_Program.Text.Substring(0, 6) : "")), Sel_SerPlan = (CAMS_FLG == "CA" ? Pass_CA_Entity.Service_plan : Pass_MS_Entity.Service_plan);
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Sel_Prog, Sel_SerPlan, string.Empty);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();

        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                Sel_CAMS_Program = form.Selected_SerPlan_Prog();

                if (CAMS_FLG == "CA")
                    Txt_CA_Program.Text = Sel_CAMS_Program;
                else
                    Txt_MS_Program.Text = Sel_CAMS_Program;


                //SetComboBoxValue(Cmb_Program, Sel_Prog);
            }
        }



        private void Fill_Members_For_BulkPost()
        {
            Blk_Post_Grid.Rows.Clear();
            DataSet ds = Captain.DatabaseLayer.MainMenu.MainMenuSearch("APP", "All", null, null, OBF_App, null, null, null, null, null, null, null, null, null, null, OBF_Hie, null, BaseForm.UserID, string.Empty, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<CommonEntity> Relation;
                    Relation = _model.lookupDataAccess.GetRelationship();

                    int rowIndex = 0; bool Mem_selected = false;
                    string Name = null, TmpSsn = null, Relation_Desc = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Name = TmpSsn = Relation_Desc = null;

                        Name = dr["Fname"].ToString().Trim() + " " + dr["MName"].ToString() + " " + dr["Lname"].ToString().Trim();
                        TmpSsn = dr["Ssn"].ToString();
                        if (!string.IsNullOrEmpty(TmpSsn))
                            TmpSsn = TmpSsn.Substring(0, 3) + '-' + TmpSsn.Substring(3, 2) + '-' + TmpSsn.Substring(5, 4);


                        foreach (CommonEntity Relationship in Relation)
                        {
                            if (Relationship.Code.Equals(dr["Mem_Code"].ToString().Trim()))
                            {
                                Relation_Desc = Relationship.Desc; break;
                            }
                        }

                        Mem_selected = false;
                        foreach (Captain.Common.Utilities.ListItem list in OBF_Type3_Sel_Members)
                        {
                            if (list.Text.ToString() == "Mem" && list.Value.ToString() == dr["ClientID"].ToString())
                            {
                                Mem_selected = true;
                                break;
                            }
                        }


                        rowIndex = Blk_Post_Grid.Rows.Add(Mem_selected, Name, Relation_Desc, TmpSsn, dr["RecFamSeq"].ToString(), dr["ClientID"].ToString(), dr["AppNo"].ToString().Substring(10, 1), "N", dr["AppStatus"].ToString(), dr["SNP_EXCLUDE"].ToString());

                        if (dr["AppStatus"].ToString() != "A")
                            Blk_Post_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        if (dr["SNP_EXCLUDE"].ToString() != "N")
                            Blk_Post_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        if (dr["AppNo"].ToString().Substring(10, 1) == "A")
                        {
                            if (dr["AppStatus"].ToString() != "A")
                                Blk_Post_Grid.Rows[rowIndex].Cells["Mem_Name"].Style.ForeColor = Color.Blue;
                            else
                                Blk_Post_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        //Members_Grid.Rows[rowIndex].Tag = dr;
                    }
                }
            }
            //Get_OBO_Data();
        }

        private void Btn_Sve_Mem_List_Click(object sender, EventArgs e)
        {
            bool AtLeast_One_Mem_Selected = true;
            if (Blk_Post_Grid.Rows.Count > 0)
            {
                AtLeast_One_Mem_Selected = false;
                foreach (DataGridViewRow dr in Blk_Post_Grid.Rows)
                {
                    if (dr.Cells["BP_Sel"].Value.ToString() == true.ToString())
                    {
                        AtLeast_One_Mem_Selected = true;
                        break;
                    }
                }

                if (AtLeast_One_Mem_Selected)
                    _errorProvider.SetError(label7, null);
                else
                {
                    _errorProvider.SetError(label7, "Select Atleast One Household Member".Replace(Consts.Common.Colon, string.Empty));
                    return;
                }
            }
            else
            {
                MessageBox.Show("No Members Exists for this APP# \n Please contact CAP Systems", "CAP Systems");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public List<Captain.Common.Utilities.ListItem> Get_Sel_Members_List()
        {
            List<Captain.Common.Utilities.ListItem> Sel_Members = new List<Captain.Common.Utilities.ListItem>();

            foreach (DataGridViewRow dr in Blk_Post_Grid.Rows)
            {
                if (dr.Cells["BP_Sel"].Value.ToString() == true.ToString())
                    Sel_Members.Add(new Captain.Common.Utilities.ListItem("Mem", dr.Cells["BP_CLID"].Value.ToString(), dr.Cells["BP_Mem_Seq"].Value.ToString(), ""));
            }

            if (Sel_Members.Count > 0 && OBF_MS_Cnt > 1 && Cb_Use_for_All.Checked)
                Sel_Members.Add(new Captain.Common.Utilities.ListItem("Sel_Sw", Cb_Use_for_All.Checked ? "Y" : "N", "", ""));

            return Sel_Members;
        }

        public string Get_Posting_Date()
        {
            string Date = "";

            if (Post_Date.Checked)
                Date = Post_Date.Value.ToShortDateString();

            return Date;
        }

        public string[] Get_CT_Triggers()
        {
            string[] Triggers = new string[7];
            Triggers[0] = "N";
            Triggers[1] = Triggers[2] = Triggers[3] = Triggers[4] = Triggers[5] = Triggers[6] = "";

            if (Cb_Age.Checked)
                Triggers[0] = "Y";

            if (Cb_B1.Checked)
                Triggers[1] += "B1,";
            if (Cb_U1.Checked)
                Triggers[1] += "U1,";
            if (Cb_R1.Checked)
                Triggers[1] += "R1,";

            if (Cb_L1.Checked)
                Triggers[2] += "1,";
            if (Cb_L2.Checked)
                Triggers[2] += "2,";
            if (Cb_L3.Checked)
                Triggers[2] += "3,";
            if (Cb_L4.Checked)
                Triggers[2] += "4,";
            if (Cb_L5.Checked)
                Triggers[2] += "5,";

            if (((Captain.Common.Utilities.ListItem)Cmb_Heat_Source.SelectedItem).Value.ToString() != "*")
                Triggers[3] = ((Captain.Common.Utilities.ListItem)Cmb_Heat_Source.SelectedItem).Value.ToString();

            if (!string.IsNullOrEmpty(Triggers[1].Trim()))
                Triggers[1] = Triggers[1].Substring(0, (Triggers[1].Length - 1));

            if (!string.IsNullOrEmpty(Triggers[2].Trim()))
                Triggers[2] = Triggers[2].Substring(0, (Triggers[2].Length - 1));

            //if (CT_Ckeck_Date.Checked)
            //    Triggers[4] = CT_Ckeck_Date.Value.ToShortDateString();

            if (Cb_Chk_Date.Checked)
            {
                Triggers[4] = "Y";
                Triggers[5] = CT_Ckeck_FDate.Value.ToShortDateString();
                Triggers[6] = CT_Ckeck_TDate.Value.ToShortDateString();
            }



            return Triggers;
        }

        public string[] Get_HS_Triggers()
        {
            string[] Triggers = new string[3];
            Triggers[0] = Triggers[1] = Triggers[2] = "";

            if (Rb_2Days.Checked)
            {
                Triggers[0] = "2";
                Triggers[1] = Attn_FDate.Value.ToShortDateString();
                Triggers[2] = Attn_TDate.Value.ToShortDateString();
            }
            else Triggers[0] = "1";

            return Triggers;
        }



        private void Blk_Post_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Blk_Post_Grid.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (Blk_Post_Grid.CurrentRow.Cells["BP_Active_Sw"].Value.ToString() != "A")
                    {
                        Blk_Post_Grid.CurrentRow.Cells["BP_Sel"].Value = false;
                    }

                    if (Blk_Post_Grid.CurrentRow.Cells["BP_Exclude_SW"].Value.ToString() == "Y")
                    {
                        Blk_Post_Grid.CurrentRow.Cells["BP_Sel"].Value = false;
                    }

                }
            }

        }


        private void pbPdf_Click(object sender, EventArgs e)
        {
            On_PaymentVoucher();
        }


        #region Payment Voucher

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        private void On_PaymentVoucher()
        {
            Random_Filename = null;

            PdfName = "PAYMENT VOUCHER";//form.GetFileName();
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
            }


            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            Document document = new Document(PageSize.LETTER, 10, 10, 20, 20);
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bf_timesBold = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 10, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font CalibriFont = new iTextSharp.text.Font(bf_times, 11);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 11, 2);
            iTextSharp.text.Font TblFont10 = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblBFont8 = new iTextSharp.text.Font(bf_times, 8, 1);

            //iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bf_times, 9);

            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 11, 4);
            //iTextSharp.text.Font TimesBoldline = new iTextSharp.text.Font(bf_times, 11, 5);
            //iTextSharp.text.Font TimesBoldlineHead = new iTextSharp.text.Font(bf_times, 13, 5);
            //iTextSharp.text.Font TimesBold = new iTextSharp.text.Font(bf_times, 15, 1);
            cb = writer.DirectContent;

            //List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("AGENCY", string.Empty, string.Empty, string.Empty);
            List<CAVoucherEntity> VoucherList = new List<CAVoucherEntity>();
            CAVoucherEntity Search_Entity = new CAVoucherEntity(true);

            VoucherList = _model.SPAdminData.Browse_CAVoucher(Search_Entity, "Browse");
            string FundV = string.Empty, ProgV = string.Empty, CAV = string.Empty;

            try
            {

                PdfPTable FTable = new PdfPTable(3);
                FTable.TotalWidth = 750f;
                FTable.WidthPercentage = 100;
                FTable.LockedWidth = true;
                float[] Headertablewidths = new float[] { 40f, 60f, 70f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                FTable.SetWidths(Headertablewidths);
                FTable.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPTable STable = new PdfPTable(6);
                STable.TotalWidth = 750f;
                STable.WidthPercentage = 100;
                STable.LockedWidth = true;
                float[] STablewidths = new float[] { 40f, 90f, 20f, 30f, 20f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                STable.SetWidths(STablewidths);
                STable.HorizontalAlignment = Element.ALIGN_CENTER;
                STable.SpacingBefore = 5f;


                #region For Office Use Only
                /* --------------------For Office Use Only----------------------*/
                PdfPTable F1Table = new PdfPTable(3);
                F1Table.WidthPercentage = 100;
                float[] F1widths = new float[] { 40f, 30f, 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                F1Table.SetWidths(F1widths);
                F1Table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell A1 = new PdfPCell(new Phrase("FOR " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " Office Use Only", CalibriFont));
                A1.Colspan = 3;
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A1);

                PdfPCell A2 = new PdfPCell(new Phrase("NSC Initials", TableFont));
                A2.HorizontalAlignment = Element.ALIGN_RIGHT;
                A2.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase("CO Initials", TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase("Accounting Initials", TableFont));
                A4.HorizontalAlignment = Element.ALIGN_CENTER;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A4);

                PdfPCell A5 = new PdfPCell(new Phrase("", TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A5);

                PdfPCell A6 = new PdfPCell(new Phrase("Date Rc'd CO", TableFont));
                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                A6.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A6);

                PdfPCell A7 = new PdfPCell(new Phrase("Voucher #", TableFont));
                A7.HorizontalAlignment = Element.ALIGN_LEFT;
                A7.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A7);

                PdfPCell A8 = new PdfPCell(new Phrase("Entered on CIS", TableFont));
                A8.HorizontalAlignment = Element.ALIGN_LEFT;
                A8.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A8);

                PdfPCell A9 = new PdfPCell(new Phrase("Verified", TableFont));
                A9.HorizontalAlignment = Element.ALIGN_LEFT;
                A9.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A9);

                PdfPCell A10 = new PdfPCell(new Phrase("Vendor #", TableFont));
                A10.HorizontalAlignment = Element.ALIGN_LEFT;
                A10.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A10);

                PdfPCell A11 = new PdfPCell(new Phrase("", TableFont));
                A11.HorizontalAlignment = Element.ALIGN_LEFT;
                A11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F1Table.AddCell(A11);

                PdfPCell A12 = new PdfPCell(new Phrase("DP in CO", TableFont));
                A12.HorizontalAlignment = Element.ALIGN_LEFT;
                A12.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A12);

                PdfPCell A13 = new PdfPCell(new Phrase("Check & Date", TableFont));
                A13.HorizontalAlignment = Element.ALIGN_LEFT;
                A13.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A13);

                PdfPCell A14 = new PdfPCell(new Phrase("", TableFont));
                A14.HorizontalAlignment = Element.ALIGN_LEFT;
                A14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F1Table.AddCell(A14);

                PdfPCell A15 = new PdfPCell(new Phrase("DC on MR", TableFont));
                A15.HorizontalAlignment = Element.ALIGN_LEFT;
                A15.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A15);

                PdfPCell A16 = new PdfPCell(new Phrase("Entered by", TableFont));
                A16.HorizontalAlignment = Element.ALIGN_LEFT;
                A16.Border = iTextSharp.text.Rectangle.BOX;
                F1Table.AddCell(A16);

                #endregion

                #region Address
                PdfPTable F2Table = new PdfPTable(1);
                F2Table.WidthPercentage = 100;
                float[] F2widths = new float[] { 70f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                F2Table.SetWidths(F2widths);
                F2Table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # -000000000", CalibriFont));
                B1.HorizontalAlignment = Element.ALIGN_CENTER;
                B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F2Table.AddCell(B1);

                string Address = string.Empty, City = string.Empty, telphn = string.Empty, Fax = string.Empty;
                string strAgencyName = string.Empty;
                MaskedTextBox mskPhn = new MaskedTextBox();
                MaskedTextBox mskFax = new MaskedTextBox();

                strAgencyName = _model.lookupDataAccess.GetHierachyDescription("1", BaseForm.BaseAgency, string.Empty, string.Empty);
                Address = dtAgency.Rows[0]["ACR_STREET"].ToString().Trim();
                City = dtAgency.Rows[0]["ACR_CITY"].ToString().Trim() + ", " + dtAgency.Rows[0]["ACR_STATE"].ToString() + "  " + dtAgency.Rows[0]["ACR_ZIP1"].ToString();
                telphn = dtAgency.Rows[0]["ACR_MAIN_PHONE"].ToString().Trim();

                if (!string.IsNullOrEmpty(telphn.Trim()))
                {
                    mskPhn.Mask = "(999) 000-0000";
                    mskPhn.Text = telphn.Trim();
                }
                Fax = dtAgency.Rows[0]["ACR_FAX_NUMBER"].ToString().Trim();

                if (!string.IsNullOrEmpty(Fax.Trim()))
                {
                    mskFax.Mask = "(999) 000-0000";
                    mskFax.Text = Fax.Trim();
                }

                PdfPCell B2 = new PdfPCell(new Phrase(strAgencyName, CalibriFont));
                B2.HorizontalAlignment = Element.ALIGN_LEFT;
                B2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F2Table.AddCell(B2);

                PdfPCell B3 = new PdfPCell(new Phrase(Address + ", " + City, CalibriFont));
                B3.HorizontalAlignment = Element.ALIGN_LEFT;
                B3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F2Table.AddCell(B3);

                PdfPCell B4 = new PdfPCell(new Phrase(mskPhn.Text, CalibriFont));
                B4.HorizontalAlignment = Element.ALIGN_LEFT;
                B4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F2Table.AddCell(B4);

                PdfPCell B5 = new PdfPCell(new Phrase("Tax Exemption # " + dtAgency.Rows[0]["ACR_CA_TAX_EXMNO"].ToString().Trim(), CalibriFont));
                B5.HorizontalAlignment = Element.ALIGN_CENTER;
                B5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                F2Table.AddCell(B5);

                #endregion

                #region Particular column image

                PdfPTable ImgTable = new PdfPTable(1);
                ImgTable.WidthPercentage = 100;
                float[] ImgTablewidths = new float[] { 20f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                ImgTable.SetWidths(ImgTablewidths);
                ImgTable.HorizontalAlignment = Element.ALIGN_CENTER;

                iTextSharp.text.Image _image = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_lg_Righ_Arrow);
                //iTextSharp.text.Image _image1 = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\arrow-left-icon.png"));
                //_image.SetAbsolutePosition(160, 310);

                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                Space1.HorizontalAlignment = Element.ALIGN_CENTER;
                //HF1.FixedHeight = 50f;
                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImgTable.AddCell(Space1);

                PdfPCell I1 = new PdfPCell(_image);
                I1.HorizontalAlignment = Element.ALIGN_CENTER;
                I1.FixedHeight = 30f;
                I1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImgTable.AddCell(I1);

                PdfPCell I2 = new PdfPCell(new Phrase("Deliver Articles to or Render Services For", TableFont));
                I2.HorizontalAlignment = Element.ALIGN_CENTER;
                I2.Rowspan = 5;
                I2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImgTable.AddCell(I2);

                PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
                Space5.HorizontalAlignment = Element.ALIGN_CENTER;
                //HF1.FixedHeight = 50f;
                Space5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImgTable.AddCell(Space5);

                PdfPCell I3 = new PdfPCell(_image);
                I3.HorizontalAlignment = Element.ALIGN_CENTER;
                I3.FixedHeight = 30f;
                I3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImgTable.AddCell(I3);


                #endregion

                #region details Table

                PdfPTable F3Table = new PdfPTable(3);
                F3Table.WidthPercentage = 100;
                float[] F3widths = new float[] { 40f, 15f, 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                F3Table.SetWidths(F3widths);
                F3Table.HorizontalAlignment = Element.ALIGN_LEFT;

                string VendName = string.Empty; string VendAddress = string.Empty; string Vendcity = string.Empty;// string VendPhone = string.Empty;
                MaskedTextBox mskvendphn = new MaskedTextBox();
                mskvendphn.Mask = "(000)000-0000";
                if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
                {
                    CASEVDDEntity selvend = CaseVddlist.Find(u => u.Code.Trim().Equals(Txt_VendNo.Text.Trim()));
                    if (selvend != null)
                    {
                        VendName = selvend.Name.Trim();
                        VendAddress = selvend.Addr1.Trim();
                        Vendcity = selvend.City.Trim() + ", " + selvend.State.Trim() + "  " + selvend.Zip.Trim();
                        if (!string.IsNullOrEmpty(selvend.Phone.Trim()))
                            mskvendphn.Text = selvend.Phone.Trim();
                    }

                }

                #region Vendor Name

                PdfPTable VendNametable = new PdfPTable(1);
                VendNametable.WidthPercentage = 100;
                float[] VendNametablewidths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                VendNametable.SetWidths(VendNametablewidths);
                VendNametable.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell K1 = new PdfPCell(new Phrase("Vendor/Service Provider", TableFont));
                K1.HorizontalAlignment = Element.ALIGN_LEFT;
                K1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable.AddCell(K1);

                PdfPCell K2 = new PdfPCell(new Phrase(VendName, TableFont));
                K2.HorizontalAlignment = Element.ALIGN_LEFT;
                K2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable.AddCell(K2);

                #endregion


                PdfPCell C1 = new PdfPCell(VendNametable);
                //C1.HorizontalAlignment = Element.ALIGN_LEFT;
                C1.FixedHeight = 32f;
                C1.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C1);

                PdfPCell C2 = new PdfPCell(ImgTable);
                C2.Padding = 0f;
                C2.Rowspan = 4;
                C2.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C2);

                #region Consumer Name

                PdfPTable Nametable = new PdfPTable(1);
                Nametable.WidthPercentage = 100;
                float[] Nametablewidths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Nametable.SetWidths(Nametablewidths);
                Nametable.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell J1 = new PdfPCell(new Phrase("Consumer Name", TableFont));
                J1.HorizontalAlignment = Element.ALIGN_LEFT;
                J1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable.AddCell(J1);

                PdfPCell J2 = new PdfPCell(new Phrase(BaseForm.BaseApplicationName.Trim(), TableFont));
                J2.HorizontalAlignment = Element.ALIGN_LEFT;
                J2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable.AddCell(J2);

                #endregion

                PdfPCell C3 = new PdfPCell(Nametable);//new PdfPCell(new Phrase("Consumer Name", TableFont));
                //C3.HorizontalAlignment = Element.ALIGN_LEFT;
                C3.FixedHeight = 32f;
                C3.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C3);

                #region Vendor Address

                PdfPTable VendNametable1 = new PdfPTable(1);
                VendNametable1.WidthPercentage = 100;
                float[] VendNametable1widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                VendNametable1.SetWidths(VendNametable1widths);
                VendNametable1.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell K3 = new PdfPCell(new Phrase("Mailing Address", TableFont));
                K3.HorizontalAlignment = Element.ALIGN_LEFT;
                K3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable1.AddCell(K3);

                //string Apt = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim())) Apt = ", Apt: " + BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim();
                //string Flr = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim())) Apt = ", Flr: " + BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim();

                PdfPCell K4 = new PdfPCell(new Phrase(VendAddress, TableFont));
                K4.HorizontalAlignment = Element.ALIGN_LEFT;
                K4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable1.AddCell(K4);

                #endregion


                PdfPCell C4 = new PdfPCell(VendNametable1);
                //C4.HorizontalAlignment = Element.ALIGN_LEFT;
                C4.FixedHeight = 30f;
                C4.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C4);

                #region Consumer Address

                PdfPTable Nametable1 = new PdfPTable(1);
                Nametable1.WidthPercentage = 100;
                float[] Nametable1widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Nametable1.SetWidths(Nametable1widths);
                Nametable1.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell J3 = new PdfPCell(new Phrase("Address", TableFont));
                J3.HorizontalAlignment = Element.ALIGN_LEFT;
                J3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable1.AddCell(J3);

                string Apt = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim())) Apt = ", Apt: " + BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim();
                string Flr = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim())) Apt = ", Flr: " + BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim();

                PdfPCell J4 = new PdfPCell(new Phrase(BaseForm.BaseCaseMstListEntity[0].Hn.Trim().Trim() + " " + BaseForm.BaseCaseMstListEntity[0].Street.Trim() + " " + BaseForm.BaseCaseMstListEntity[0].Suffix.Trim() + Apt + Flr, TableFont));
                J4.HorizontalAlignment = Element.ALIGN_LEFT;
                J4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable1.AddCell(J4);

                #endregion

                PdfPCell C5 = new PdfPCell(Nametable1);
                //C5.HorizontalAlignment = Element.ALIGN_LEFT;
                C5.FixedHeight = 32f;
                C5.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C5);

                #region Vendor City

                PdfPTable VendNametable2 = new PdfPTable(1);
                VendNametable2.WidthPercentage = 100;
                float[] VendNametable2widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                VendNametable2.SetWidths(VendNametable2widths);
                VendNametable2.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell K5 = new PdfPCell(new Phrase("City, State, Zip", TableFont));
                K5.HorizontalAlignment = Element.ALIGN_LEFT;
                K5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable2.AddCell(K5);

                PdfPCell K6 = new PdfPCell(new Phrase(Vendcity, TableFont));
                K6.HorizontalAlignment = Element.ALIGN_LEFT;
                K6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable2.AddCell(K6);

                #endregion

                PdfPCell C6 = new PdfPCell(VendNametable2);
                //C6.HorizontalAlignment = Element.ALIGN_LEFT;
                C6.FixedHeight = 32f;
                C6.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C6);

                #region Consumer City

                PdfPTable Nametable2 = new PdfPTable(1);
                Nametable2.WidthPercentage = 100;
                float[] Nametable2widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Nametable2.SetWidths(Nametable2widths);
                Nametable2.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell J5 = new PdfPCell(new Phrase("City, State, Zip", TableFont));
                J5.HorizontalAlignment = Element.ALIGN_LEFT;
                J5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable2.AddCell(J5);

                PdfPCell J6 = new PdfPCell(new Phrase(BaseForm.BaseCaseMstListEntity[0].City.Trim().Trim() + ", " + BaseForm.BaseCaseMstListEntity[0].State.Trim() + " " + "00000".Substring(0, 5 - BaseForm.BaseCaseMstListEntity[0].Zip.Trim().Length) + BaseForm.BaseCaseMstListEntity[0].Zip.Trim(), TableFont));
                J6.HorizontalAlignment = Element.ALIGN_LEFT;
                J6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable2.AddCell(J6);

                #endregion

                PdfPCell C7 = new PdfPCell(Nametable2);
                //C7.HorizontalAlignment = Element.ALIGN_LEFT;
                C7.FixedHeight = 32f;
                C7.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C7);

                #region Vendor Phone

                PdfPTable VendNametable3 = new PdfPTable(1);
                VendNametable3.WidthPercentage = 100;
                float[] VendNametable3widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                VendNametable3.SetWidths(VendNametable3widths);
                VendNametable3.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell K7 = new PdfPCell(new Phrase("Phone", TableFont));
                K7.HorizontalAlignment = Element.ALIGN_LEFT;
                K7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable3.AddCell(K7);

                PdfPCell K8 = new PdfPCell(new Phrase(mskvendphn.Text.Trim(), TableFont));
                K8.HorizontalAlignment = Element.ALIGN_LEFT;
                K8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                VendNametable3.AddCell(K8);

                #endregion

                PdfPCell C8 = new PdfPCell(VendNametable3);
                //C8.HorizontalAlignment = Element.ALIGN_LEFT;
                C8.FixedHeight = 32f;
                C8.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C8);

                #region Consumer Account && Phn

                PdfPTable Nametable3 = new PdfPTable(2);
                Nametable3.WidthPercentage = 100;
                float[] Nametable3widths = new float[] { 28f, 12f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Nametable3.SetWidths(Nametable3widths);
                Nametable3.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell J7 = new PdfPCell(new Phrase("Consumer Account Number", TableFont));
                J7.HorizontalAlignment = Element.ALIGN_LEFT;
                J7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable3.AddCell(J7);

                PdfPCell J8 = new PdfPCell(new Phrase("Phone", TableFont));
                J8.HorizontalAlignment = Element.ALIGN_LEFT;
                J8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable3.AddCell(J8);

                PdfPCell J9 = new PdfPCell(new Phrase("", TableFont));
                J9.HorizontalAlignment = Element.ALIGN_LEFT;
                J9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable3.AddCell(J9);

                MaskedTextBox mskphn = new MaskedTextBox();
                mskphn.Mask = "(000)000-0000";
                mskphn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim().Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim().Trim();

                PdfPCell J10 = new PdfPCell(new Phrase(mskphn.Text, TableFont));
                J10.HorizontalAlignment = Element.ALIGN_LEFT;
                J10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Nametable3.AddCell(J10);

                #endregion

                PdfPCell C9 = new PdfPCell(Nametable3);
                //C9.HorizontalAlignment = Element.ALIGN_LEFT;
                C9.FixedHeight = 32f;
                C9.Border = iTextSharp.text.Rectangle.BOX;
                F3Table.AddCell(C9);

                #endregion

                #region Paragraph Table

                PdfPTable PTable = new PdfPTable(1);
                PTable.WidthPercentage = 100;
                float[] PTablewidths = new float[] { 20f };
                PTable.SetWidths(PTablewidths);
                PTable.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell D1 = new PdfPCell(new Phrase("CERTIFICATION REGARDING", TableFont));
                D1.HorizontalAlignment = Element.ALIGN_CENTER;
                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PTable.AddCell(D1);

                PdfPCell D2 = new PdfPCell(new Phrase("(Debarment, Suspension, Ineligibility and Voluntary Exclusion)", TableFont));
                D2.HorizontalAlignment = Element.ALIGN_CENTER;
                D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PTable.AddCell(D2);

                PdfPCell D3 = new PdfPCell(new Phrase("This certification is required by the requisitions implementing executive order 13549, debarment and suspension, CFR 1036 Appendix B.  The prospective lower tier participant certifies by submission of this proposal, that neither it nor its principals is presently debarred, suspended, proposed for debarment, declared ineligible, or voluntarily excluded from participation in this transaction by any federal department or agency.  Where the prospective lower tier participant is unable to certify any of the statements in this certification, such prospective participant shall attach an explanation to this proposal.", TableFont));
                D3.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PTable.AddCell(D3);

                PdfPCell D4 = new PdfPCell(new Phrase("Signature _________________________________________ Date __________", TableFont));
                D4.HorizontalAlignment = Element.ALIGN_LEFT;
                D4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PTable.AddCell(D4);

                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                Space2.HorizontalAlignment = Element.ALIGN_CENTER;
                Space2.FixedHeight = 5f;
                Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PTable.AddCell(Space2);

                PdfPCell D5 = new PdfPCell(new Phrase("Vendor Federal Tax ID", TableFont));
                D5.HorizontalAlignment = Element.ALIGN_LEFT;
                D5.Border = iTextSharp.text.Rectangle.BOX;
                PTable.AddCell(D5);

                #endregion

                iTextSharp.text.Image _Logo = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\CommunityAction1.jpg"));
                _Logo.ScalePercent(40f);

                PdfPCell HF1 = new PdfPCell(_Logo);
                HF1.HorizontalAlignment = Element.ALIGN_LEFT;
                //HF1.FixedHeight = 50f;
                HF1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FTable.AddCell(HF1);

                PdfPCell HF2 = new PdfPCell(F2Table);
                //HF2.HorizontalAlignment = Element.ALIGN_LEFT;
                //HF2.FixedHeight = 50f;
                HF2.Padding = 0f;
                HF2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FTable.AddCell(HF2);

                PdfPCell HF3 = new PdfPCell(F1Table);
                HF3.Padding = 0f;
                //HF3.HorizontalAlignment = Element.ALIGN_LEFT;
                //HF3.FixedHeight = 50f;
                HF3.Border = iTextSharp.text.Rectangle.BOX;
                FTable.AddCell(HF3);

                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                Space.Colspan = 3;
                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FTable.AddCell(Space);

                PdfPCell HF4 = new PdfPCell(F3Table);
                HF4.Padding = 0f;
                HF4.Colspan = 2;
                HF4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FTable.AddCell(HF4);

                PdfPCell HF5 = new PdfPCell(PTable);
                HF5.Padding = 0f;
                HF5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FTable.AddCell(HF5);

                document.Add(FTable);

                PdfPCell E1 = new PdfPCell(new Phrase("The information on this form is for " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " purposes only and is strictly confidential.  This is a legal document.", TblFontItalic));
                E1.HorizontalAlignment = Element.ALIGN_LEFT;
                E1.Colspan = 6;
                E1.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E1);

                #region First Row

                PdfPCell R1 = new PdfPCell(new Phrase("Services", TblFontBold));
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(R1);

                PdfPCell E3 = new PdfPCell(new Phrase("By signing this voucher I " + Text_VendName.Text.Trim() + " guarantee that the above named consumer and those family members listed on the rental/mortgage agreement will be allowed to dwell in the above named property for one month (no less than 30 days) as a result of this assistance.  Further, I state that I am the authorized owner, manager (landlord) and/or mortgage holder of said property.  ", TableFont));
                E3.HorizontalAlignment = Element.ALIGN_LEFT;
                E3.Rowspan = 4;
                E3.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E3);

                PdfPCell E4 = new PdfPCell(new Phrase("FUND", TblFontBold));
                E4.HorizontalAlignment = Element.ALIGN_CENTER;
                E4.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E4);

                PdfPCell E5 = new PdfPCell(new Phrase("Program Code", TblFontBold));
                E5.HorizontalAlignment = Element.ALIGN_CENTER;
                E5.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E5);

                PdfPCell E6 = new PdfPCell(new Phrase("County", TblFontBold));
                E6.HorizontalAlignment = Element.ALIGN_CENTER;
                E6.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E6);

                PdfPCell E7 = new PdfPCell(new Phrase("Amt of Assistance", TblFont10));
                E7.HorizontalAlignment = Element.ALIGN_CENTER;
                E7.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E7);

                #endregion

                iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_checkbox);
                _image_UnChecked.ScalePercent(60f);

                #region temp code

                #region Past due rent

                PdfPTable S1Table = new PdfPTable(2);
                S1Table.WidthPercentage = 100;
                float[] S1Tablewidths = new float[] { 5f, 35f };
                S1Table.SetWidths(S1Tablewidths);
                S1Table.HorizontalAlignment = Element.ALIGN_LEFT;



                //PdfPCell G1 = new PdfPCell(_image_UnChecked);
                //G1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G1.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S1Table.AddCell(G1);

                //PdfPCell G2 = new PdfPCell(new Phrase("Past Due Rent", TableFont));
                //G2.HorizontalAlignment = Element.ALIGN_LEFT;
                //G2.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S1Table.AddCell(G2);

                ////PdfAppearance[] checkbox =new PdfAppearance[2];
                ////PdfFormField footerField = PdfFormField.CreateEmpty(writer);
                ////PdfPTable footerTbl = new PdfPTable(2);
                ////float[] footerWidths = new float[] { 1f, 4f };
                ////PdfPCell noDeleteCell = new PdfPCell();
                ////PdfPCell noDeleteText = new PdfPCell(new Paragraph("Past Due Rent", TableFont));
                ////RadioCheckField fCell = new RadioCheckField(writer, new iTextSharp.text.Rectangle(20, 20), "PastDueRent", "Yes");
                ////fCell.CheckType = RadioCheckField.TYPE_CROSS;

                ////var tf = new TextField(writer, new iTextSharp.text.Rectangle(20, 20), "PastDueRent");

                ////PdfFormField footerCheck = null;
                ////footerCheck = fCell.CheckField;
                //////footerCheck.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, "Off", checkbox[0]);
                //////footerCheck.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, "Yes", checkbox[1]);
                ////noDeleteCell.CellEvent = new ChildFieldEvent(footerField, tf.GetTextField(), 3);
                ////footerField.FieldName = "Past_DueRent";
                ////footerTbl.SetWidths(footerWidths);
                ////footerTbl.AddCell(noDeleteCell);
                ////footerTbl.AddCell(noDeleteText);


                #endregion

                #region First Months Payment

                PdfPTable S2Table = new PdfPTable(2);
                S2Table.WidthPercentage = 100;
                float[] S2Tablewidths = new float[] { 5f, 35f };
                S2Table.SetWidths(S2Tablewidths);
                S2Table.HorizontalAlignment = Element.ALIGN_LEFT;



                //PdfPCell G3 = new PdfPCell(_image_UnChecked);
                //G3.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G3.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S2Table.AddCell(G3);

                //PdfPCell G4 = new PdfPCell(new Phrase("First Months Payment", TableFont));
                //G4.HorizontalAlignment = Element.ALIGN_LEFT;
                //G4.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S2Table.AddCell(G4);

                #endregion

                #region Mortgage

                PdfPTable S3Table = new PdfPTable(2);
                S3Table.WidthPercentage = 100;
                float[] S3Tablewidths = new float[] { 5f, 35f };
                S3Table.SetWidths(S3Tablewidths);
                S3Table.HorizontalAlignment = Element.ALIGN_LEFT;



                //PdfPCell G5 = new PdfPCell(_image_UnChecked);
                //G5.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G5.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S3Table.AddCell(G5);

                //PdfPCell G6 = new PdfPCell(new Phrase("Mortgage", TableFont));
                //G6.HorizontalAlignment = Element.ALIGN_LEFT;
                //G6.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S3Table.AddCell(G6);

                #endregion

                #region Goods

                PdfPTable S4Table = new PdfPTable(2);
                S4Table.WidthPercentage = 100;
                float[] S4Tablewidths = new float[] { 5f, 35f };
                S4Table.SetWidths(S4Tablewidths);
                S4Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G7 = new PdfPCell(_image_UnChecked);
                //G7.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G7.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S4Table.AddCell(G7);

                //PdfPCell G8 = new PdfPCell(new Phrase("Goods", TableFont));
                //G8.HorizontalAlignment = Element.ALIGN_LEFT;
                //G8.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S4Table.AddCell(G8);

                #endregion

                #region Utility of Water

                PdfPTable S5Table = new PdfPTable(2);
                S5Table.WidthPercentage = 100;
                float[] S5Tablewidths = new float[] { 5f, 35f };
                S5Table.SetWidths(S5Tablewidths);
                S5Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G9 = new PdfPCell(_image_UnChecked);
                //G9.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G9.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S5Table.AddCell(G9);

                //PdfPCell G10 = new PdfPCell(new Phrase("Utlilities of Water,Power or Nat. Gas", TableFont));
                //G10.HorizontalAlignment = Element.ALIGN_LEFT;
                //G10.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S5Table.AddCell(G10);

                #endregion

                #region Non-Utility heating Assistance

                PdfPTable S6Table = new PdfPTable(2);
                S6Table.WidthPercentage = 100;
                float[] S6Tablewidths = new float[] { 5f, 35f };
                S6Table.SetWidths(S6Tablewidths);
                S6Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G11 = new PdfPCell(_image_UnChecked);
                //G11.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G11.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S6Table.AddCell(G11);

                //PdfPCell G12 = new PdfPCell(new Phrase("Non-Utility Heating Assistance", TableFont));
                //G12.HorizontalAlignment = Element.ALIGN_LEFT;
                //G12.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S6Table.AddCell(G12);

                #endregion

                #region Health

                PdfPTable S7Table = new PdfPTable(2);
                S7Table.WidthPercentage = 100;
                float[] S7Tablewidths = new float[] { 5f, 35f };
                S7Table.SetWidths(S7Tablewidths);
                S7Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G13 = new PdfPCell(_image_UnChecked);
                //G13.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G13.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G13.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S7Table.AddCell(G13);

                //PdfPCell G14 = new PdfPCell(new Phrase("Health", TableFont));
                //G14.HorizontalAlignment = Element.ALIGN_LEFT;
                //G14.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S7Table.AddCell(G14);

                #endregion

                #region Heating Equipment

                PdfPTable S8Table = new PdfPTable(2);
                S8Table.WidthPercentage = 100;
                float[] S8Tablewidths = new float[] { 5f, 35f };
                S8Table.SetWidths(S8Tablewidths);
                S8Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G15 = new PdfPCell(_image_UnChecked);
                //G15.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G15.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S8Table.AddCell(G15);

                //PdfPCell G16 = new PdfPCell(new Phrase("Heating Equipment", TableFont));
                //G16.HorizontalAlignment = Element.ALIGN_LEFT;
                //G16.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S8Table.AddCell(G16);

                #endregion

                #region Well/Spring Pump

                PdfPTable S9Table = new PdfPTable(2);
                S9Table.WidthPercentage = 100;
                float[] S9Tablewidths = new float[] { 5f, 35f };
                S9Table.SetWidths(S9Tablewidths);
                S9Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G17 = new PdfPCell(_image_UnChecked);
                //G17.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G17.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S9Table.AddCell(G17);

                //PdfPCell G18 = new PdfPCell(new Phrase("Well/Spring Pump", TableFont));
                //G18.HorizontalAlignment = Element.ALIGN_LEFT;
                //G18.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S9Table.AddCell(G18);

                #endregion

                #region Employment Related Items

                PdfPTable S10Table = new PdfPTable(2);
                S10Table.WidthPercentage = 100;
                float[] S10Tablewidths = new float[] { 5f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                S10Table.SetWidths(S10Tablewidths);
                S10Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G19 = new PdfPCell(_image_UnChecked);
                //G19.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G19.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S10Table.AddCell(G19);

                //PdfPCell G20 = new PdfPCell(new Phrase("Employment Related Items", TableFont));
                //G20.HorizontalAlignment = Element.ALIGN_LEFT;
                //G20.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S10Table.AddCell(G20);

                #endregion

                #region Housing Program

                PdfPTable S11Table = new PdfPTable(2);
                S11Table.WidthPercentage = 100;
                float[] S11Tablewidths = new float[] { 5f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                S11Table.SetWidths(S11Tablewidths);
                S11Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G21 = new PdfPCell(_image_UnChecked);
                //G21.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G21.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S11Table.AddCell(G21);

                //PdfPCell G22 = new PdfPCell(new Phrase("Housing Program", TableFont));
                //G22.HorizontalAlignment = Element.ALIGN_LEFT;
                //G22.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S11Table.AddCell(G22);

                #endregion

                #region Self-Sufficiency Items

                PdfPTable S12Table = new PdfPTable(2);
                S12Table.WidthPercentage = 100;
                float[] S12Tablewidths = new float[] { 5f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                S12Table.SetWidths(S11Tablewidths);
                S12Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G23 = new PdfPCell(_image_UnChecked);
                //G23.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G23.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S12Table.AddCell(G23);

                //PdfPCell G24 = new PdfPCell(new Phrase("Self-Sufficiency Items", TableFont));
                //G24.HorizontalAlignment = Element.ALIGN_LEFT;
                //G24.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G24.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S12Table.AddCell(G24);

                #endregion

                #region Deposits

                PdfPTable S13Table = new PdfPTable(2);
                S13Table.WidthPercentage = 100;
                float[] S13Tablewidths = new float[] { 5f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                S13Table.SetWidths(S13Tablewidths);
                S13Table.HorizontalAlignment = Element.ALIGN_LEFT;

                //PdfPCell G25 = new PdfPCell(_image_UnChecked);
                //G25.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G25.HorizontalAlignment = Element.ALIGN_RIGHT;
                ////W2UnCheked.FixedHeight = 15f;
                //G25.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                //S13Table.AddCell(G25);

                //PdfPCell G26 = new PdfPCell(new Phrase("Deposits", TableFont));
                //G26.HorizontalAlignment = Element.ALIGN_LEFT;
                //G26.VerticalAlignment = Element.ALIGN_MIDDLE;
                //G26.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                //S13Table.AddCell(G26);

                #endregion

                #endregion

                //PdfPCell E8 = new PdfPCell(S1Table);
                //E8.Padding = 0f;
                //E8.Border = iTextSharp.text.Rectangle.BOX;
                //STable.AddCell(E8);
                ////writer.AddAnnotation(footerField);

                #region Second Row

                PdfPCell R2 = new PdfPCell(new Phrase(CAMS_Desc.Trim(), TblBFont8));
                R2.HorizontalAlignment = Element.ALIGN_LEFT;
                //SS1.FixedHeight = 15f;
                R2.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(R2);

                //SpaceCells(STable, TableFont);

                //PdfPCell E9 = new PdfPCell(S2Table);
                //E9.Padding = 0f;
                //E9.Border = iTextSharp.text.Rectangle.BOX;
                //STable.AddCell(E9);

                if (!string.IsNullOrEmpty(Pass_CA_Entity.Fund1.Trim()))
                {
                    //string FundDesc = string.Empty;
                    //List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
                    //FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
                    //foreach (SPCommonEntity Entity in FundingList)
                    //{
                    //    if (Entity.Code.Trim() == Pass_CA_Entity.Fund1.Trim())
                    //    {
                    //        FundDesc = Entity.Desc.Trim();
                    //        break;
                    //    }
                    //}
                    PdfPCell SS2 = new PdfPCell(new Phrase(Pass_CA_Entity.Fund1.Trim(), TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                    //SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }
                else
                {
                    PdfPCell SS2 = new PdfPCell(new Phrase("", TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                    SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }

                //string FundV = string.Empty, ProgV = string.Empty, CAV = string.Empty;
                CAVoucherEntity FundVouch = VoucherList.Find(u => u.Code.Equals(Pass_CA_Entity.Fund1.Trim()));
                CAVoucherEntity ProgramVouch = VoucherList.Find(u => u.Code.Equals(Pass_CA_Entity.Act_PROG.Trim()));
                CAVoucherEntity CACode = VoucherList.Find(u => u.Code.Equals(Pass_CA_Entity.ACT_Code.Trim()));

                if (FundVouch != null) FundV = FundVouch.VCode.Trim() + "-";
                if (ProgramVouch != null) ProgV = ProgramVouch.VCode.Trim() + "-";
                if (CACode != null) CAV = CACode.VCode.Trim();

                if (string.IsNullOrEmpty(ProgV.Trim()) && string.IsNullOrEmpty(CAV.Trim())) FundV = FundV.Replace("-", "");
                if (string.IsNullOrEmpty(CAV.Trim())) ProgV = ProgV.Replace("-", "");

                string ProgramCode = FundV + ProgV + CAV;

                //if (!string.IsNullOrEmpty(Pass_CA_Entity.Act_PROG.Trim()))
                //{
                PdfPCell SS3 = new PdfPCell(new Phrase(ProgramCode, TblBFont8));
                SS3.HorizontalAlignment = Element.ALIGN_LEFT;
                //SS2.FixedHeight = 15f;
                SS3.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(SS3);
                //}
                //else
                //{
                //    PdfPCell SS2 = new PdfPCell(new Phrase("", TblBFont8));
                //    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                //    SS2.FixedHeight = 15f;
                //    SS2.Border = iTextSharp.text.Rectangle.BOX;
                //    STable.AddCell(SS2);
                //}

                if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].County.Trim()))
                {
                    string CountyDesc = string.Empty;
                    List<CommonEntity> Country = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetCountry();
                    foreach (CommonEntity country in Country)
                    {
                        if (BaseForm.BaseCaseMstListEntity[0].County.Trim() == country.Code.Trim())
                        {
                            CountyDesc = country.Desc.Trim();
                            break;
                        }
                    }

                    PdfPCell SS2 = new PdfPCell(new Phrase(CountyDesc, TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                    //SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }
                else
                {
                    PdfPCell SS2 = new PdfPCell(new Phrase("", TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                    SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }

                if (!string.IsNullOrEmpty(Pass_CA_Entity.Cost.Trim()))
                {
                    PdfPCell SS2 = new PdfPCell(new Phrase(Pass_CA_Entity.Cost.Trim(), TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }
                else
                {
                    PdfPCell SS2 = new PdfPCell(new Phrase("", TblBFont8));
                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                    SS2.FixedHeight = 15f;
                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                    STable.AddCell(SS2);
                }

                #endregion

                //SpaceCells(STable, TableFont);

                //PdfPCell E10 = new PdfPCell(S3Table);
                //E10.Padding = 0f;
                //E10.Border = iTextSharp.text.Rectangle.BOX;
                //STable.AddCell(E10);
                //if (!string.IsNullOrEmpty(Pass_CA_Entity.p.Trim()))

                #region Third Row

                PdfPCell R3 = new PdfPCell(new Phrase("", TblFontBold));
                R3.HorizontalAlignment = Element.ALIGN_CENTER;
                R3.FixedHeight = 15f;
                R3.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(R3);

                SpaceCells(STable, TableFont);

                #endregion

                #region Fourth Row

                PdfPCell R4 = new PdfPCell(new Phrase("", TblFontBold));
                R4.HorizontalAlignment = Element.ALIGN_CENTER;
                R4.FixedHeight = 15f;
                R4.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(R4);

                SpaceCells(STable, TableFont);

                #endregion


                PdfPCell E11 = new PdfPCell(S4Table);
                E11.Padding = 0f;
                E11.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E11);

                //PdfPCell E12 = new PdfPCell(new Phrase("Please Circle:        Hygiene        Clothing         Food        Household Goods", TableFont));
                PdfPCell E12 = new PdfPCell(new Phrase("", TableFont));
                E12.FixedHeight = 15f;
                E12.HorizontalAlignment = Element.ALIGN_LEFT;
                E12.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E12);

                SpaceCells(STable, TableFont);

                PdfPCell E13 = new PdfPCell(S5Table);
                E13.Padding = 0f;
                E13.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E13);

                //PdfPCell E14 = new PdfPCell(new Phrase("Please Circle:      Water Assistance        Electrical Assistne        Natural Gas", TableFont));
                PdfPCell E14 = new PdfPCell(new Phrase("", TableFont));
                E14.FixedHeight = 15f;
                E14.HorizontalAlignment = Element.ALIGN_LEFT;
                E14.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E14);

                SpaceCells(STable, TableFont);

                PdfPCell E15 = new PdfPCell(S6Table);
                E15.Padding = 0f;
                E15.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E15);

                //PdfPCell E16 = new PdfPCell(new Phrase("This assistance is for a one months supply of fuel", TableFont));
                PdfPCell E16 = new PdfPCell(new Phrase(" ", TableFont));
                E16.FixedHeight = 15f;
                E16.HorizontalAlignment = Element.ALIGN_LEFT;
                E16.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E16);

                SpaceCells(STable, TableFont);

                PdfPCell E17 = new PdfPCell(S7Table);
                E17.Padding = 0f;
                E17.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E17);

                //PdfPCell E18 = new PdfPCell(new Phrase("Medical Item: (description) ________________________ Prescription Dental Over the Counter This assistance is not to be used for purchase of narcotics or foe reimbursement of previously purchased prescriptions or for medication covered by insurance.", TableFont));
                PdfPCell E18 = new PdfPCell(new Phrase(" ", TableFont));
                E18.FixedHeight = 15f;
                E18.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
                E18.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E18);

                SpaceCells(STable, TableFont);

                PdfPCell E19 = new PdfPCell(S8Table);
                E19.Padding = 0f;
                E19.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E19);

                //PdfPCell E20 = new PdfPCell(new Phrase("Please Circle:        Repair        New        Temporary Heating Equipment", TableFont));
                PdfPCell E20 = new PdfPCell(new Phrase(" ", TableFont));
                E20.FixedHeight = 15f;
                E20.HorizontalAlignment = Element.ALIGN_LEFT;
                E20.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E20);

                SpaceCells(STable, TableFont);

                PdfPCell E21 = new PdfPCell(S9Table);
                E21.Padding = 0f;
                E21.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E21);

                //PdfPCell E22 = new PdfPCell(new Phrase("Please Circle:        Repair                New        ", TableFont));
                PdfPCell E22 = new PdfPCell(new Phrase(" ", TableFont));
                E22.FixedHeight = 15f;
                E22.HorizontalAlignment = Element.ALIGN_LEFT;
                E22.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E22);

                SpaceCells(STable, TableFont);

                PdfPCell E23 = new PdfPCell(S10Table);
                E23.Padding = 0f;
                E23.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E23);

                //PdfPCell E24 = new PdfPCell(new Phrase("Description of Items : ", TableFont));
                PdfPCell E24 = new PdfPCell(new Phrase(" ", TableFont));
                E24.FixedHeight = 15f;
                E24.HorizontalAlignment = Element.ALIGN_LEFT;
                E24.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E24);

                SpaceCells(STable, TableFont);

                PdfPCell E25 = new PdfPCell(S11Table);
                E25.Padding = 0f;
                E25.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E25);

                //PdfPCell E26 = new PdfPCell(new Phrase("Description : _______________________________________________________ ____________________________________________________________________ \n By signing this voucher I(vendor) understand that I will not be paid until work is completed and " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " has received a final inspection from USDA -Rural Development Office. I also understand that the payment will not be made until the original invoice has been received by " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + ". ", TableFont));
                PdfPCell E26 = new PdfPCell(new Phrase("", TableFont));
                E26.FixedHeight = 15f;
                E26.HorizontalAlignment = Element.ALIGN_LEFT;
                E26.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E26);

                SpaceCells(STable, TableFont);


                PdfPCell E27 = new PdfPCell(S12Table);
                E27.Padding = 0f;
                E27.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E27);

                //PdfPCell E28 = new PdfPCell(new Phrase("Please list the Items:  ", TableFont));
                PdfPCell E28 = new PdfPCell(new Phrase("", TableFont));
                E28.FixedHeight = 15f;
                E28.HorizontalAlignment = Element.ALIGN_LEFT;
                E28.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E28);

                SpaceCells(STable, TableFont);

                PdfPCell E29 = new PdfPCell(S13Table);
                E29.Padding = 0f;
                E29.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E29);

                //PdfPCell E30 = new PdfPCell(new Phrase("Please Circle:          Utility                  Housing        ", TableFont));
                PdfPCell E30 = new PdfPCell(new Phrase("", TableFont));
                E30.FixedHeight = 15f;
                E30.HorizontalAlignment = Element.ALIGN_LEFT;
                E30.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E30);

                SpaceCells(STable, TableFont);

                #region authorization

                PdfPTable LTable = new PdfPTable(3);
                LTable.WidthPercentage = 100;
                float[] LTablewidths = new float[] { 50f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                LTable.SetWidths(LTablewidths);
                LTable.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell H1 = new PdfPCell(new Phrase("Authorized by                         Date          ", TableFont));
                H1.HorizontalAlignment = Element.ALIGN_LEFT;
                H1.FixedHeight = 25f;
                H1.Border = iTextSharp.text.Rectangle.BOX;
                LTable.AddCell(H1);

                PdfPCell H2 = new PdfPCell(new Phrase("Consumer Signature                            Date          ", TableFont));
                H2.HorizontalAlignment = Element.ALIGN_LEFT;
                H2.FixedHeight = 25f;
                H2.Border = iTextSharp.text.Rectangle.BOX;
                LTable.AddCell(H2);

                PdfPCell H3 = new PdfPCell(new Phrase("Program Director                          Date          ", TableFont));
                H3.HorizontalAlignment = Element.ALIGN_LEFT;
                H3.FixedHeight = 25f;
                H3.Border = iTextSharp.text.Rectangle.BOX;
                LTable.AddCell(H3);

                #endregion

                PdfPCell E31 = new PdfPCell(LTable);
                E31.Padding = 0f;
                E31.Colspan = 6;
                E31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                STable.AddCell(E31);

                PdfPCell E32 = new PdfPCell(new Phrase("I understand that if the above mentioned services are not rendered or breached in any way, the monies paid by " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " are to be returned immediately to " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + ".  Failure to do so or adhere to the conditions of this agreement will likely result in legal action.  I(we) certify that the services listed above have been performed for the client or articles received as authorized on this order and that payment therefore is due. ", TableFont));
                E32.HorizontalAlignment = Element.ALIGN_LEFT;
                E32.Colspan = 6;
                E32.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E32);

                #region Vendor

                PdfPTable L1Table = new PdfPTable(3);
                L1Table.WidthPercentage = 100;
                float[] LTable1widths = new float[] { 50f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                L1Table.SetWidths(LTable1widths);
                L1Table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell H4 = new PdfPCell(new Phrase("Vendor/Service Provider                           Date          ", TableFont));
                H4.HorizontalAlignment = Element.ALIGN_LEFT;
                H4.FixedHeight = 25f;
                H4.Border = iTextSharp.text.Rectangle.BOX;
                L1Table.AddCell(H4);

                PdfPCell H5 = new PdfPCell(new Phrase("Vendor/Service Provider Address (if different from above)        ", TableFont));
                H5.HorizontalAlignment = Element.ALIGN_LEFT;
                H5.FixedHeight = 25f;
                H5.Border = iTextSharp.text.Rectangle.BOX;
                L1Table.AddCell(H5);

                PdfPCell H6 = new PdfPCell(new Phrase("Finance Director                          Date          ", TableFont));
                H6.HorizontalAlignment = Element.ALIGN_LEFT;
                H6.FixedHeight = 25f;
                H6.Border = iTextSharp.text.Rectangle.BOX;
                L1Table.AddCell(H6);

                #endregion

                PdfPCell E33 = new PdfPCell(L1Table);
                E33.Padding = 0f;
                E33.Colspan = 6;
                E33.Border = iTextSharp.text.Rectangle.NO_BORDER;
                STable.AddCell(E33);

                PdfPCell E34 = new PdfPCell(new Phrase("Submit voucher for payment within five (5) days to " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " at the address listed above.  An attached itemized receipt of services/items is required.", TableFont));
                E34.HorizontalAlignment = Element.ALIGN_CENTER;
                E34.Colspan = 6;
                E34.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E34);

                PdfPCell E35 = new PdfPCell(new Phrase("Service Provider should receive payment within twenty (20) days.", TableFont));
                E35.HorizontalAlignment = Element.ALIGN_CENTER;
                E35.Colspan = 6;
                E35.Border = iTextSharp.text.Rectangle.BOX;
                STable.AddCell(E35);

                document.Add(STable);
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            document.Close();
            fs.Close();
            fs.Dispose();

            if (LookupDataAccess.FriendlyName().Contains("2012"))
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
        }

        private void SpaceCells(PdfPTable STable, iTextSharp.text.Font TableFont)
        {
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void PrintSpaceCell(PdfPTable table, int Spacesnum, iTextSharp.text.Font TableFont)
        {
            if (Spacesnum == 1)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                table.AddCell(S2);
            }
            else if (Spacesnum == 2)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 2;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 3)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 3;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 4)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 4;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 6)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 6;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 7)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 7;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 10)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 10;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 15)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 15;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 12)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 12;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
        }

        private void GetAgencyDetails()
        {
            dsAgency = DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            dtAgency = dsAgency.Tables[0];
        }

        #endregion

        private void Btn_Save_Post_Date_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Exp_Post_Date.Trim()))
            {
                if (Post_Date.Checked && Post_Date.Value < Convert.ToDateTime(Exp_Post_Date))
                {
                    if (Calling_Form == "Posting")
                        MessageBox.Show("CA/MS Posting date should not be prior to Service plan posting date", "CAP Systems");
                    else
                        MessageBox.Show("Status Date should not be prior to History Date " + Convert.ToDateTime(Exp_Post_Date), "CAP Systems");
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
                if (Calling_Form == "Enrl")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void Btn_Save_Triggers_Click(object sender, EventArgs e)
        {
            if (Validate_Triggers())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private bool Validate_Triggers()
        {
            bool Can_Save = true;

            if (Cb_Chk_Date.Checked)
            {
                if (Cb_B1.Checked || Cb_U1.Checked || Cb_R1.Checked ||
                   Cb_L1.Checked || Cb_L2.Checked || Cb_L3.Checked || Cb_L4.Checked || Cb_L5.Checked)
                {
                    Can_Save = true;
                    _errorProvider.SetError(Cb_R1, null);

                }
                else
                {
                    _errorProvider.SetError(Cb_R1, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Any one Benefit Type or Level".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }

                if (CT_Ckeck_FDate.Value > CT_Ckeck_TDate.Value)
                {
                    _errorProvider.SetError(CT_Ckeck_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Check From date Should not be greater that to date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(CT_Ckeck_TDate, null);

            }

            return Can_Save;
        }


        private bool Validate_HS_Triggers()
        {
            bool Can_Save = true;

            if (Rb_2Days.Checked)
            {
                if (!Attn_FDate.Checked)
                {
                    _errorProvider.SetError(Attn_FDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label13.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(Attn_FDate, null);

                if (!Attn_TDate.Checked)
                {
                    _errorProvider.SetError(Attn_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label14.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Can_Save = false;
                }
                else
                    _errorProvider.SetError(Attn_TDate, null);

                if (Attn_TDate.Checked && Attn_FDate.Checked)
                {
                    if (Attn_FDate.Value > Attn_TDate.Value)
                    {
                        _errorProvider.SetError(Attn_TDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date should not be greater than to date".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Save = false;
                    }
                    else
                        _errorProvider.SetError(Attn_TDate, null);
                }
            }

            return Can_Save;
        }

        private void Btn_Hs_Triggers_Click(object sender, EventArgs e)
        {
            if (Validate_HS_Triggers())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void Rb_1Day_Click(object sender, EventArgs e)
        {
            Attn_FDate.Value = Attn_TDate.Value = DateTime.Today;
            Attn_FDate.Checked = Attn_TDate.Checked = false;
        }

        private void Rb_2Days_Click(object sender, EventArgs e)
        {
            Attn_FDate.Value = Attn_TDate.Value = DateTime.Today;
            Attn_FDate.Checked = Attn_TDate.Checked = true;
        }

        private void Cb_Chk_Date_Click(object sender, EventArgs e)
        {
            if (Cb_Chk_Date.Checked)
            {
                Check_Dates_Panel.Visible = true;

            }
            else
            {
                Check_Dates_Panel.Visible = false;
            }
        }

        private void Cb_B1_Click(object sender, EventArgs e)
        {
            Check_Base_Ben_Type();
        }

        private void btnCAOBO_Click(object sender, EventArgs e)
        {
            MembersGridForm PostCA_Form;
            PostCA_Form = new MembersGridForm(BaseForm, Hierarchy, Year, CAMS_Desc, Pass_CA_Entity, Privileges, CA_Template_List, ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString(), CA_OBO_List);   // 08022012
            PostCA_Form.FormClosed += new FormClosedEventHandler(Add_Edit_MembersForm_Closed);

            PostCA_Form.ShowDialog();
        }


        private void Add_Edit_MembersForm_Closed(object sender, FormClosedEventArgs e)
        {
            MembersGridForm form = sender as MembersGridForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CAOBO_List = new List<CAOBOEntity>();
                CAOBO_List = form.GetMemberRecords();
            }
        }

        private void Pb_SP2_Max_Click(object sender, EventArgs e)
        {
            pnlCustomeQues.Visible = Pb_SP2_Max.Visible = false;
            this.pnlService.Location = new System.Drawing.Point(0, 5);//(0, 340);
            this.pnlService.Size = new System.Drawing.Size(602, 522); //(602, 182);
            lblActSeekDate.Visible = false;
            lblActSeekDateReq.Visible = false;
            dtActSeek_Date.Visible = false;
            lblCABenefit.Visible = false;
            cmb_CA_Benefit.Visible = false;
            btnCAOBO.Visible = false;
            lblActivityDate.Visible = false;
            lblActivityDateReq.Visible = false;
            Act_Date.Visible = false;
            lblFunSour1.Visible = false;
            lblFunSour1Req.Visible = false;
            CmbFunding1.Visible = false;

            this.SP_CA_Grid.Size = new System.Drawing.Size(590, 488); // 590, 148

            ////this.panel5.Size = new System.Drawing.Size(761, 22);
            //this.Pb_SP2_Min.Location = new System.Drawing.Point(744, 0);

            // this.SP2_Desc.Width = 526;
            Pb_SP2_Min.Visible = true;




        }

        private void Pb_SP2_Min_Click(object sender, EventArgs e)
        {
            pnlCustomeQues.Visible = true; Pb_SP2_Min.Visible = false;
            this.pnlService.Location = new System.Drawing.Point(0, 340);//(0, 340);
            this.pnlService.Size = new System.Drawing.Size(602, 182); //(602, 182);


            lblActivityDate.Visible = true;
            lblActivityDateReq.Visible = true;
            lblFunSour1.Visible = true;
            CmbFunding1.Visible = true;
            Act_Date.Visible = true;
            foreach (FldcntlHieEntity entity in CntlCAEntity)
            {
                bool required = entity.Req.Equals("Y") ? true : false;
                bool enabled = entity.Enab.Equals("Y") ? true : false;

                switch (entity.FldCode)
                {
                    case Consts.CASE0006.FundingSource1:
                        if (enabled) { lblFunSour1.Enabled = CmbFunding1.Enabled = true; if (required) lblFunSour1Req.Visible = true; } else { lblFunSour1.Enabled = CmbFunding1.Enabled = false; lblFunSour1Req.Visible = false; }
                        break;
                    case Consts.CASE0006.Act_Seek_Date:
                        if (enabled) { dtActSeek_Date.Visible = lblActSeekDate.Visible = true; if (required) lblActSeekDateReq.Visible = true; } else { dtActSeek_Date.Visible = lblActSeekDate.Visible = false; lblActSeekDateReq.Visible = false; }
                        break;
                }
            }

            if (BaseAgencyControlDetails.CAOBO.Trim() == "Y")
            {
                lblCABenefit.Visible = true;
                cmb_CA_Benefit.Visible = true;
                btnCAOBO.Visible = true;
            }

            this.SP_CA_Grid.Size = new System.Drawing.Size(590, 148); // 590, 148


            Pb_SP2_Max.Visible = true;
        }

        private void Check_Base_Ben_Type()
        {
            Lbl_Ben_Type_Error.Visible = false;
            if ((Cb_B1.Checked && App_Ben_Type != "B1") ||
                (Cb_U1.Checked && App_Ben_Type != "U1") ||
                (Cb_R1.Checked && App_Ben_Type != "R1"))
                Lbl_Ben_Type_Error.Visible = true;

        }


        private void Check_Base_Ben_Level()
        {
            Lbl_Ben_level_Error.Visible = false;
            if ((Cb_L1.Checked && App_Ben_level != "1") ||
                (Cb_L2.Checked && App_Ben_level != "2") ||
                (Cb_L3.Checked && App_Ben_level != "3") ||
                (Cb_L4.Checked && App_Ben_level != "4") ||
                (Cb_L5.Checked && App_Ben_level != "5"))
                Lbl_Ben_level_Error.Visible = true;

        }



        private void Cb_L1_Click(object sender, EventArgs e)
        {
            Check_Base_Ben_Level();
        }

        private void dtActSeek_Date_LostFocus(object sender, EventArgs e)
        {
            if (dtActSeek_Date.Checked && Act_Date.Checked)
            {
                if (Act_Date.Value < dtActSeek_Date.Value)
                {
                    _errorProvider.SetError(dtActSeek_Date, "Please The Activity/Service Requested Date never Greater than Activity/Service Date");
                }
                else
                    _errorProvider.SetError(dtActSeek_Date, null);
            }
        }

        private void dtMSSeek_Date_LostFocus(object sender, EventArgs e)
        {
            if (dtMSSeek_Date.Checked && MS_Date.Checked)
            {
                if (MS_Date.Value < dtMSSeek_Date.Value)
                {
                    _errorProvider.SetError(dtMSSeek_Date, "Please The Estimated MS/Outcome Date never Greater than MS/Outcome Date");

                }
                else
                    _errorProvider.SetError(dtMSSeek_Date, null);
            }
        }

        private void cmb_CA_Benefit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Text.ToString()))
            //    Set_Members_CA_Grid_As_Benefit_Change(true, ((Captain.Common.Utilities.ListItem)cmb_CA_Benefit.SelectedItem).Value.ToString());
        }


    }
}


