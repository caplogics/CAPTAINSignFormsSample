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
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASE1006_Bulk_Posting_Latest : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public CASE1006_Bulk_Posting_Latest(BaseForm baseform, string SP_code, string SP_sequence, string SP_start_date, CASESP0Entity sp_header_rec, string branchCode, PrivilegeEntity privileges)
        {
            InitializeComponent();
            BaseForm = baseform;
            Privileges = privileges;
            SP_Code = SP_code;
            SP_Sequence = SP_sequence;
            //Selected_CAMS_List = selected_CAMS_list;
            Selected_CAMS_List = new List<ListItem>();
            SP_Start_Date = SP_start_date;
            SP_Header_Rec = sp_header_rec;
            BranchCode = branchCode;
            CAMS_FLG = "Bulk_Posting";
            this.Text = "Service Activity Bulk Posting" + "     "+SP_Header_Rec.Desc.Trim();
            _model = new CaptainModel();
            if (!string.IsNullOrEmpty(SP_start_date.Trim()))
            {
                From_Date.Value = To_Date.Value = Convert.ToDateTime(SP_start_date.Trim());
                From_Date.Checked = To_Date.Checked = true;
            }

           // Bulk_Posting_Panel.Location = new System.Drawing.Point(2, 2);
            //this.Size = new System.Drawing.Size(624, 469);
            Bulk_Posting_Panel.Visible = true;
            //Cmb_Enrl_Stat.Size = new System.Drawing.Size(187, 25);

            SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);

            SP_CAMS_Details = SP_CAMS_Details.FindAll(u => u.Branch == BranchCode);

            Fill_Sites();
            Fill_Case_Type_Combo();
            Fill_SPM_Filter();
            Fill_EnrollStat_Combo();
            //Cmb_Bulk_SortBy.Items.Insert(0, new ListItem("App#", "0"));
            //Cmb_Bulk_SortBy.Items.Insert(1, new ListItem("Client Name", "1"));

            List<PrivilegeEntity> userPrivilege = _model.UserProfileAccess.GetScreensByUserID(BaseForm.BusinessModuleID, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg);
            CaseSerPrivileges = Privileges;
            CaseSerPrivileges = userPrivilege.Find(u => u.Program == "CASE9006");


            this.Cmb_Bulk_SortBy.SelectedIndexChanged -= new System.EventHandler(this.Cmb_Bulk_SortBy_SelectedIndexChanged);
            Cmb_Bulk_SortBy.Items.Clear();
            List<HierarchyEntity> hierarchyClientName = _model.lookupDataAccess.GetClientNameFormat();
            foreach (HierarchyEntity hierarchyEntity in hierarchyClientName)
                Cmb_Bulk_SortBy.Items.Add(new ListItem(hierarchyEntity.ShortName, hierarchyEntity.Code));

            CommonFunctions.SetComboBoxValue(Cmb_Bulk_SortBy, BaseForm.BaseHierarchyCnFormat.ToString());
            this.Cmb_Bulk_SortBy.SelectedIndexChanged += new System.EventHandler(this.Cmb_Bulk_SortBy_SelectedIndexChanged);

            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                strCAOBO = dsAgency.Tables[0].Rows[0]["ACR_CAOBO"].ToString().Trim();
            }

            Cmb_Bulk_SortBy.SelectedIndex = 0;
            Get_Source_ACT_For_Posting();
            Get_Source_MS_For_Posting();

            //Fill_Bulk_App_Grid_To_Post(string.Empty);
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            ToolTip tooltip = new ToolTip();
            //tooltip.SetToolTip(Pb_Edit_OBF_Mem, "Edit Household Member(s) Benefiting from");
            if (BaseForm.BusinessModuleID == "08")
                Get_App_Base_LPB_Details();

            if (CaseSerPrivileges != null)
            {
                if (CaseSerPrivileges.AddPriv.Equals("true"))
                {
                    Btn_Bulk_Post.Visible = true;
                    Btn_PostAll.Visible = true;
                    Btn_Sel_All.Visible = true;
                    Btn_UnSel_All.Visible = true;
                }
                else
                {
                    Btn_Bulk_Post.Visible = false;
                    Btn_PostAll.Visible = false;
                    Btn_Sel_All.Visible = false;
                    Btn_UnSel_All.Visible = false;
                }
            }

            Rescheduled();
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public List<FldcntlHieEntity> CntlCAEntity { get; set; }

        public List<FldcntlHieEntity> CntlMSEntity { get; set; }



        public PrivilegeEntity CaseSerPrivileges { get; set; }

        public string CAMS_FLG { get; set; }

        public string BranchCode { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string SP_Code { get; set; }

        public string Year { get; set; }

        public string MST_Site { get; set; }

        public string Mode { get; set; }

        public string SP_Sequence { get; set; }

        public string MST_Intakeworker { get; set; }

        public string SP_Start_Date { get; set; }

        public string strCAOBO { get; set; }

        public CASEACTEntity Pass_CA_Entity { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public CASESP0Entity SP_Header_Rec { get; set; }

        #endregion

        string Sql_SP_Result_Message = string.Empty;
        string Img_Saved = Consts.Icons.ico_Save;
        string Img_Blank = Consts.Icons.ico_Blank;
        string Img_Tick = Consts.Icons.ico_Tick;
        string Img_Add = Consts.Icons.ico_Add;
        string Img_Edit = Consts.Icons.ico_Edit;


        private void Fill_Sites()
        {
            Cmb_Bulk_Site.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("   ", "0"));

            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable Sites_Table = ds.Tables[0];
                if (Sites_Table.Rows.Count > 0)
                {
                    foreach (DataRow dr in Sites_Table.Rows)
                    {
                        //if (dr["SITE_NAME"].ToString().Contains(BaseForm.BaseAgency))
                        listItem.Add(new ListItem(dr["SITE_NAME"].ToString().Trim(), dr["SITE_NUMBER"].ToString().Trim()));
                    }

                    {
                        Cmb_Bulk_Site.Items.AddRange(listItem.ToArray());
                    }
                }
            }
            Cmb_Bulk_Site.Items.Insert(0, new ListItem("All Sites", "0"));
            Cmb_Bulk_Site.SelectedIndex = 0;

        }

        private void Fill_SPM_Filter()
        {
            Cmb_SPM_Filter.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("No SPM", "1"));
            listItem.Add(new ListItem("With SPM", "2"));
            listItem.Add(new ListItem("Both", "3"));
            Cmb_SPM_Filter.Items.AddRange(listItem.ToArray());
            Cmb_SPM_Filter.SelectedIndex = 2;
        }

        private void Fill_Case_Type_Combo()
        {
            List<CommonEntity> CaseType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CASETYPES, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            foreach (CommonEntity casetype in CaseType)
            {
                ListItem li = new ListItem(casetype.Desc, casetype.Code);
                Cmb_Bulk_CaseType.Items.Add(li);
            }
            Cmb_Bulk_CaseType.Items.Insert(0, new ListItem("All Case Types", "0"));
            Cmb_Bulk_CaseType.SelectedIndex = 0;
        }

        private void Fill_EnrollStat_Combo()
        {
            Cmb_Enrl_Stat.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("  ", "0"));
            listItem.Add(new ListItem("Denied", "R"));
            listItem.Add(new ListItem("Enroll", "E"));
            listItem.Add(new ListItem("Inactive", "N"));
            listItem.Add(new ListItem("Pending", "P"));
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")
            {//  this logic value modified 10/06/2018 ask to customer
                listItem.Add(new ListItem("Postintake", "I"));
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
            listItem.Add(new ListItem("Withdrawn", "W"));

            Cmb_Enrl_Stat.Items.AddRange(listItem.ToArray());
            Cmb_Enrl_Stat.SelectedIndex = 0;
        }

        private void Fill_CutomQuestions()
        {
            Cmb_Enrl_Stat.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("   ", "0"));

            List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions("CASE2001", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "Active", "P");
            if (custQuestions.Count > 0)
            {
                custQuestions = custQuestions.FindAll(u => u.CUSTRESPTYPE.Equals("D") || u.CUSTRESPTYPE.Equals("C"));

                foreach (CustomQuestionsEntity Entity in custQuestions)
                {
                    //if (dr["SITE_NAME"].ToString().Contains(BaseForm.BaseAgency))
                    listItem.Add(new ListItem(Entity.CUSTDESC.Trim(), Entity.CUSTCODE.Trim(), Entity.CUSTMEMACCESS.Trim(), string.Empty));
                }


            }
            //Cmb_Bulk_Site.Items.Insert(0, new ListItem("All Sites", "0"));
            Cmb_Enrl_Stat.Items.AddRange(listItem.ToArray());
            Cmb_Enrl_Stat.SelectedIndex = 0;
        }

        private void Fill_CustResponses()
        {
            cmbResp.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            if (Cmb_Enrl_Stat.Items.Count > 0)
            {
                if (((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString() != "0")
                {
                    List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString());

                    if (custReponseEntity.Count > 0)
                    {
                        foreach (CustRespEntity Entity in custReponseEntity)
                        {
                            listItem.Add(new ListItem(Entity.RespDesc.Trim(), Entity.DescCode.Trim()));
                        }

                        cmbResp.Items.AddRange(listItem.ToArray());
                    }

                    cmbResp.SelectedIndex = 0;
                }
            }
        }

        List<CASESP2Entity> CA_Details = new List<CASESP2Entity>();
        private void FillServicescombo()
        {
            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvService"];

            if (SP_CAMS_Details.Count > 0)
            {
                CA_Details = new List<CASESP2Entity>();
                //if (CAMA_Post_Mode == "Add")
                {
                    CA_Details = SP_CAMS_Details.FindAll(u => u.Type1.Equals("CA"));
                    CA_Details = CA_Details.FindAll(u => u.CAMS_Active.Equals("True") && u.SP2_CAMS_Active.Equals("A"));
                }
                //else
                //{
                //    CA_Details = SP_CAMS_Details.FindAll(u => u.Type1.Equals("CA"));
                //}


                // CA_Details = SP_CAMS_Details.FindAll(u => u.Type1.Equals("CA"));
                if (CA_Details.Count > 0)
                {
                    //CASESP2Entity Entity = new CASESP2Entity();
                    //Entity.CAMS_Desc = ""; Entity.CamCd = "0";
                    //CA_Details.Add(new CASESP2Entity(Entity));

                    cb.DataSource = CA_Details;
                    cb.DisplayMember = "CAMS_Desc";
                    cb.ValueMember = "CamCd";
                    cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvService"];
                }
            }
        }

        private void FillUOM()
        {
            List<SPCommonEntity> UOMList = new List<SPCommonEntity>();
            UOMList = _model.SPAdminData.Get_AgyRecs_WithFilter("UOM", "A");

            UOMList = UOMList.FindAll(u => (u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
            UOMList = UOMList.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();

            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvUOM"];

            if (UOMList.Count > 0)
            {
                cb.DataSource = UOMList;
                cb.DisplayMember = "Desc";
                cb.ValueMember = "Code";
                cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvUOM"];
            }

        }

        private void Fill_Funding()
        {
            if (!string.IsNullOrEmpty(SP_Header_Rec.Funds))
            {
                bool Fund_Exists = false; int Pos = 0, Tmp_Loop_Cnt = 0;
                List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
                FundingList = _model.SPAdminData.Get_AgyRecs_WithFilter("Funding", "A");
                string Funds_Str = SP_Header_Rec.Funds;
                int Tmp_Curr_Fund_Len = 0;

                List<SPCommonEntity> SelFundingList = new List<SPCommonEntity>();

                foreach (SPCommonEntity Entity in FundingList)
                {

                    Fund_Exists = false; Pos = 0;
                    for (int i = 0; Pos < SP_Header_Rec.Funds.Length; i++)
                    {

                        Tmp_Curr_Fund_Len = (Funds_Str.Substring(Pos, Funds_Str.Substring(Pos, (Funds_Str.Length - Pos)).Length)).Length;

                        if (Entity.Code == SP_Header_Rec.Funds.Substring(Pos, (Tmp_Curr_Fund_Len >= 10 ? 10 : Tmp_Curr_Fund_Len)).Trim())
                        {
                            Fund_Exists = true; break;
                        }
                        Pos += 10;

                    }

                    if (Fund_Exists)
                    {
                        SelFundingList.Add(new SPCommonEntity(Entity));

                    }
                }

                DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvFund"];

                if (SelFundingList.Count > 0)
                {
                    cb.DataSource = SelFundingList;
                    cb.DisplayMember = "Desc";
                    cb.ValueMember = "Code";
                    cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvFund"];
                }


            }
        }

        private void BenefitFrom()
        {
            List<SPCommonEntity> Benefit = new List<SPCommonEntity>();

            Benefit.Add(new SPCommonEntity("1", "Applicant"));
            Benefit.Add(new SPCommonEntity("2", "All Household Members"));
            //Benefit.Add(new SPCommonEntity("3", "Applicant"));


            DataGridViewComboBoxColumn cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvBenefit"];
            if (Benefit.Count > 0)
            {
                cb.DataSource = Benefit;
                cb.DisplayMember = "Desc";
                cb.ValueMember = "Code";
                cb = (DataGridViewComboBoxColumn)this.SP_CAMS_Grid.Columns["gvBenefit"];
            }

        }


        private void Get_App_Base_LPB_Details()
        {
            APP_Ben_Type = APP_Ben_Level = "";

            LIHEAPBEntity Search_Entity = new LIHEAPBEntity(true);
            List<LIHEAPBEntity> LPB_List = new List<LIHEAPBEntity>();
            Search_Entity.Agency = BaseForm.BaseAgency;
            Search_Entity.Dept = BaseForm.BaseDept;
            Search_Entity.Prog = BaseForm.BaseProg;
            Search_Entity.Year = BaseForm.BaseYear;
            Search_Entity.AppNo = BaseForm.BaseApplicationNo;
            LPB_List = _model.LiheAllData.Browse_LIHEAPB(Search_Entity, "Browse");
            if (LPB_List.Count > 0)
            {
                APP_Ben_Type = LPB_List[0].Award_Type;
                APP_Ben_Level = LPB_List[0].Benefit_Level;
            }
        }

        int Listed_App_Cnt = 0; int List_Post_Cnt = 0; decimal TotPostAmount = 0;
        string Priv_Scr_Mode = string.Empty;
        List<SP_Bulk_Post_Entity> CASESPM_List = new List<SP_Bulk_Post_Entity>();
        List<Bulk_Post_Entity> Bulk_list = new List<Bulk_Post_Entity>();
        private void Fill_Bulk_App_Grid_To_Post(string Sel_App)
        {
            int rowIndex = 0, Selected_Index = 0; bool Can_Post_Atlest_One_APP = false;
            OBF_Type3_MS_Cnt = Listed_App_Cnt = List_Post_Cnt = 0; TotPostAmount = 0;
            Lbl_Tot_Apps.Text = ""; lblPost.Text = ""; lblPostAmt.Text = "";
            Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = Lbl_Tot_Apps.Visible = false; lblPost.Visible = false; lblPostAmt.Visible = false;
            btnShowAll.Visible = btnShowSel.Visible = lblTotSel.Visible = false; lblAmount.Visible = false;
            //CASESPM_List.Clear();
            this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            //this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            Post_App_Grid.Rows.Clear();
            SP_CAMS_Grid.Rows.Clear();
            //this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);

            Bulk_list = new List<Bulk_Post_Entity>();

            CASESPMEntity Search_Entity = new CASESPMEntity(true);

            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;

            //Search_Entity.year = MainMenuYear;        
            Search_Entity.year = BaseForm.BaseYear;                // Year will be always Four-Spaces in CASESPM
            Search_Entity.service_plan = SP_Code;
            Search_Entity.Seq = SP_Sequence;
            Search_Entity.startdate = From_Date.Value.ToShortDateString(); //SP_Start_Date; modified by murali 27/FEB/2019
            //Search_Entity.app_no =
            //Search_Entity.caseworker = Search_Entity.site =
            //Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate =
            //Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc =
            //Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator =
            //Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = null;

            //if (Priv_Scr_Mode != (Rb_SPM_Date.Checked ? "CASESPM" : "CASEMST"))
            //    CASESPM_List = _model.SPAdminData.Browse_CASESPM_4Bulk_Posting(Search_Entity, "Browse", BaseForm.BaseApplicationNo, (Rb_SPM_Date.Checked ? "CASESPM" : "CASEMST"));
            string Curr_Scr_Mode = "", Row_Color = "";
            if (Rb_SPM_Date.Checked)
                Curr_Scr_Mode = "CASESPM";
            else if (Rb_Intake_Date.Checked)
                Curr_Scr_Mode = "CASEMST";
            else Curr_Scr_Mode = "TRIGGER";

            string CustCode = string.Empty; string QuestAccess = string.Empty;
            if (rbQuestion.Checked) { CustCode = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString().Trim(); QuestAccess = ((ListItem)Cmb_Enrl_Stat.SelectedItem).ID.ToString().Trim(); }

            string Sel_Site = null;
            if (((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString() != "0")
                Sel_Site = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();

            string Source_For_Posting = Get_Source_ActMS_For_Posting_Identity();
            //if (Priv_Scr_Mode != Curr_Scr_Mode)
            CASESPM_List = _model.SPAdminData.Browse_CASESPM_4Bulk_Posting_Latest(Search_Entity, "Browse", BaseForm.BaseApplicationNo, "BULKMST", Source_For_Posting, BaseForm.BusinessModuleID, Sel_Site,
                                                                                    (CT_Age_SW ? "Y" : null), (!string.IsNullOrEmpty(CT_LPB_Benfit.Trim()) ? CT_LPB_Benfit : null), (!string.IsNullOrEmpty(CT_LPB_Benfit_Level.Trim()) ? CT_LPB_Benfit_Level : null),
                                                                                                              (!string.IsNullOrEmpty(CT_LPB_Source.Trim()) ? CT_LPB_Source : null), (!string.IsNullOrEmpty(CT_LPB_Chk_Date.Trim()) ? CT_LPB_Chk_Date : null), Cb_Triggeer.Checked,
                                                                                                              (!string.IsNullOrEmpty(HS_Attn_SW.Trim()) ? HS_Attn_SW : null), (!string.IsNullOrEmpty(HS_Attn_From_Date.Trim()) ? HS_Attn_From_Date : null), (!string.IsNullOrEmpty(HS_Attn_To_Date.Trim()) ? HS_Attn_To_Date : null),
                                                                                                              (CT_LPB_Chk_Date.Trim() == "Y" ? CT_LPB_Chk_FDate : ""), (CT_LPB_Chk_Date.Trim() == "Y" ? CT_LPB_Chk_TDate : ""), CustCode, QuestAccess, string.Empty);

            if (Rb_SPM_Date.Checked)
                Priv_Scr_Mode = "CASESPM";
            else if (Rb_Intake_Date.Checked)
                Priv_Scr_Mode = "CASEMST";
            else Priv_Scr_Mode = "TRIGGER";

            //Priv_Scr_Mode = (Rb_SPM_Date.Checked ? "CASESPM" : "CASEMST");
            string App_Name = string.Empty, App_Address = string.Empty, Start_Date = string.Empty, Intake_Date = string.Empty;

            Btn_Bulk_Post.Visible = false;
            bool From_Date_Flg = false, To_Date_Flg = false;
            string Ref_Date = string.Empty, TmpName = string.Empty, CaseType_To_Compare = ((ListItem)Cmb_Bulk_CaseType.SelectedItem).Value.ToString(),
                   Site_To_Compare = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();

            string Status_enrl = "0";
            if (rbEnrlStatus.Checked)
                Status_enrl = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();

            string CustResp = "0";
            if (rbQuestion.Checked)
                CustResp = ((ListItem)cmbResp.SelectedItem).Value.ToString().Trim();


            if (CASESPM_List.Count > 0)
            {
                if (Status_enrl != "0")
                {
                    CASESPM_List = CASESPM_List.FindAll(u => u.Enrl_Status.Equals(Status_enrl.Trim()));
                }
                else if (CustResp != "0")
                {
                    CASESPM_List = CASESPM_List.FindAll(u => u.Cust_Resp.Equals(CustResp.Trim()));
                }

                if (chkbShowPost.Checked == true)
                {
                    CASESPM_List = CASESPM_List.FindAll(u => u.Post_SW.Substring(0, 1) == "Y");
                }
            }



            if (Rb_Intake_Date.Checked)
            {
                this.dataGridViewTextBoxColumn1.Visible = true;
                this.dataGridViewTextBoxColumn3.Width = 340; //220;
            }
            else
            {
                this.dataGridViewTextBoxColumn3.Width = 360;// 265;
                this.dataGridViewTextBoxColumn1.Visible = false;
            }

            //App_Name = LookupDataAccess.GetMemberName(Entity.SNP_First_Name, Entity.SNP_Middle_Name, Entity.SNP_Last_Name, BaseForm.BaseHierarchyCnFormat.ToString());

            List<SP_Bulk_Post_Entity> List_Ord_Name = new List<SP_Bulk_Post_Entity>();
            //List<SP_Bulk_Post_Entity> List_Ord_Name_Format = new List<SP_Bulk_Post_Entity>();
            //List_Ord_Name = CASESPM_List.OrderBy(u => u.SNP_First_Name).ThenBy(u => u.SNP_Middle_Name).ThenBy(u => u.SNP_Last_Name).ThenBy(u => Convert.ToDateTime(u.date_lstc)).ToList();

            if (((ListItem)Cmb_Bulk_SortBy.SelectedItem).Value.ToString() == "1")
            {
                List_Ord_Name = CASESPM_List.OrderBy(u => u.SNP_First_Name).ThenBy(u => u.SNP_Middle_Name).ThenBy(u => u.SNP_Last_Name).ToList();
                List_Ord_Name.ForEach(u => u.Disp_Name = LookupDataAccess.GetMemberName(u.SNP_First_Name, u.SNP_Middle_Name, u.SNP_Last_Name, "1"));
            }
            else
            {
                List_Ord_Name = CASESPM_List.OrderBy(u => u.SNP_Last_Name).ThenBy(u => u.SNP_First_Name).ThenBy(u => u.SNP_Middle_Name).ToList();
                List_Ord_Name.ForEach(u => u.Disp_Name = LookupDataAccess.GetMemberName(u.SNP_First_Name, u.SNP_Middle_Name, u.SNP_Last_Name, "2"));
            }

            CASESPM_List.Clear();
            CASESPM_List = List_Ord_Name;

            bool SPM_Filter = false; int SPM_Cnt = 0;
            string Cmb_SMP_Filter_Val = ((ListItem)Cmb_SPM_Filter.SelectedItem).Value.ToString();
            foreach (SP_Bulk_Post_Entity Entity in CASESPM_List)
            {
                //if (((Rb_Intake_Date.Checked && ((Entity.SPM_Count == "0") || (Entity.SPM_Count != "0" && !string.IsNullOrEmpty(Entity.SPM_app_no.Trim())))) || 
                //    (Entity.SPM_Count != "0" && Rb_SPM_Date.Checked)) ||
                //    (BaseForm.BusinessModuleID == "02" && Entity.Attn_1Day_SW == "Y" && Rb_Trigger.Checked && Entity.SPM_Count == "0") || // Filter For HS )
                //    (BaseForm.BusinessModuleID == "08" && Entity.CT_Trigger_SW == "Y" && Rb_Trigger.Checked && Entity.SPM_Count == "0")  // Filter For HS )
                //    )

                //if (((Rb_Intake_Date.Checked && ((Entity.SPM_Count == "0") || (Entity.SPM_Count != "0" && !string.IsNullOrEmpty(Entity.SPM_app_no.Trim())))) ||
                //    (Entity.SPM_Count != "0" && Rb_SPM_Date.Checked)) ||
                //    (BaseForm.BusinessModuleID == "02" && Entity.Attn_1Day_SW == "Y" && Cb_Triggeer.Checked && Entity.SPM_Count == "0") || // Filter For HS )
                //    (BaseForm.BusinessModuleID == "08" && Entity.CT_Trigger_SW == "Y" && Cb_Triggeer.Checked && Entity.SPM_Count == "0")  // Filter For HS )
                //    )                

                //if (Entity.MST_app_no == "00000476")
                //    SPM_Filter = false; 

                SPM_Filter = false; SPM_Cnt = int.Parse(Entity.SPM_Count.Trim());
                if (Rb_Intake_Date.Checked)
                {
                    if ((Cmb_SMP_Filter_Val == "1" && SPM_Cnt == 0) || (Cmb_SMP_Filter_Val == "2" && SPM_Cnt > 0) || Cmb_SMP_Filter_Val == "3")
                        SPM_Filter = true;
                }

                if ((Rb_SPM_Date.Checked && SPM_Cnt > 0) ||
                    (Rb_Intake_Date.Checked && SPM_Filter && ((Cb_Triggeer.Checked && ((Entity.CT_Trigger_SW == "Y" && BaseForm.BusinessModuleID == "08") || (Entity.Attn_1Day_SW == "Y" && BaseForm.BusinessModuleID == "02"))) || !Cb_Triggeer.Checked)))
                {
                    From_Date_Flg = To_Date_Flg = false;

                    if (!From_Date.Checked)
                        From_Date_Flg = true;

                    if (!To_Date.Checked)
                        To_Date_Flg = true;

                    if ((CaseType_To_Compare == "0" || CaseType_To_Compare == Entity.MST_Case_Type) &&
                        (Site_To_Compare == "0" || Site_To_Compare == Entity.Mst_Site))
                    {
                        if (From_Date.Checked)
                        {
                            //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked) //Cb_Triggeer
                            if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked)
                            {
                                if (!string.IsNullOrEmpty((Entity.MST_Intake_Date.Trim())))
                                {
                                    if (From_Date.Value <= Convert.ToDateTime(Entity.MST_Intake_Date))
                                        From_Date_Flg = true;
                                }
                            }

                            if (Rb_SPM_Date.Checked)
                            {
                                if (!string.IsNullOrEmpty((Entity.startdate.Trim())))
                                {
                                    if (From_Date.Value <= Convert.ToDateTime(Entity.startdate))
                                        From_Date_Flg = true;
                                }
                            }

                            //if ((From_Date.Value <= Convert.ToDateTime(Entity.startdate) && Rb_SPM_Date.Checked) ||
                            //    (From_Date.Value <= Convert.ToDateTime(Entity.MST_Intake_Date) && Rb_Intake_Date.Checked))
                            //    From_Date_Flg = true;
                        }
                        if (To_Date.Checked)
                        {
                            //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked) //Cb_Triggeer
                            if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked)
                            {
                                if (!string.IsNullOrEmpty((Entity.MST_Intake_Date.Trim())))
                                {
                                    if (To_Date.Value >= Convert.ToDateTime(Entity.MST_Intake_Date))
                                        To_Date_Flg = true;
                                }
                            }

                            if (Rb_SPM_Date.Checked)
                            {
                                if (!string.IsNullOrEmpty((Entity.startdate.Trim())))
                                {
                                    if (To_Date.Value >= Convert.ToDateTime(Entity.startdate))
                                        To_Date_Flg = true;
                                }
                            }

                            //if ((To_Date.Value >= Convert.ToDateTime(Entity.startdate) && Rb_SPM_Date.Checked) ||
                            //    (To_Date.Value >= Convert.ToDateTime(Entity.MST_Intake_Date) && Rb_Intake_Date.Checked))
                            //    To_Date_Flg = true;
                        }

                        if (From_Date_Flg && To_Date_Flg)
                        {
                            App_Name = App_Address = Start_Date = Intake_Date = " ";
                            //App_Name = LookupDataAccess.GetMemberName(Entity.SNP_First_Name, Entity.SNP_Middle_Name, Entity.SNP_Last_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                            App_Name = Entity.Disp_Name;
                            App_Address = Entity.Mst_Hno + ' ' + Entity.MST_Street + ' ' + Entity.MST_City + ' ' + Entity.MST_State + ' ' + Entity.MST_Zip;
                            if (!string.IsNullOrEmpty(Entity.startdate.Trim()))
                                Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.startdate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            if (!string.IsNullOrEmpty(Entity.MST_Intake_Date.Trim()))
                                Intake_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.MST_Intake_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            Row_Color = "";
                            if (!string.IsNullOrEmpty(Entity.Post_SW.Trim()))
                            {
                                if (Entity.Post_SW.Substring(2, 1) == "Y")
                                    Row_Color = "Red";
                                else if (Entity.Post_SW.Substring(1, 1) == "Y")
                                    Row_Color = "Green";
                                else if (Entity.Post_SW.Substring(0, 1) == "Y")
                                    Row_Color = "Blak";
                            }
                            //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked) //Cb_Triggeer
                            //if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked) //Cb_Triggeer
                            //    Row_Color = "Green";

                            rowIndex = Post_App_Grid.Rows.Add(false, Entity.MST_app_no, Entity.MST_Year, App_Name, ((Entity.Mst_Active == "Y" && Entity.Snp_Active == "A") ? "A" : "I"), Intake_Date, App_Address, Entity.Mst_Site, Start_Date, Entity.Seq, Entity.Post_SW, Row_Color);

                            Bulk_list.Add(new Bulk_Post_Entity("False", Entity.MST_app_no, Entity.MST_Year, App_Name, ((Entity.Mst_Active == "Y" && Entity.Snp_Active == "A") ? "A" : "I"), Intake_Date, App_Address, Entity.Mst_Site, Start_Date, Entity.Seq, Entity.Post_SW, Row_Color, string.Empty, string.Empty, string.Empty, Entity.Post_Amounts,Entity.ENRL_DATE));

                            if (Sel_App == Entity.MST_app_no)
                                Selected_Index = rowIndex;

                            if (!string.IsNullOrEmpty(Entity.Post_SW.Trim()))
                            {
                                if (Entity.Post_SW.Substring(2, 1) == "Y")
                                    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                else if (Entity.Post_SW.Substring(1, 1) == "Y")
                                    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                                else if (Entity.Post_SW.Substring(0, 1) == "Y")
                                {
                                    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                    List_Post_Cnt++;
                                    TotPostAmount = TotPostAmount + decimal.Parse(Entity.Post_Amounts.Trim());
                                }
                            }
                            else
                                Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;

                            //if (Entity.Post_SW == "N")
                            //    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                            //else
                            //{
                            //    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green; Can_Post_Atlest_One_APP = true;
                            //}
                            Can_Post_Atlest_One_APP = true;

                            Listed_App_Cnt++;
                        }
                    }
                }
            }

            if (Listed_App_Cnt > 0)
            {
                Lbl_Tot_Apps.Text = "Total Applications: " + Listed_App_Cnt.ToString();
                Lbl_Tot_Apps.Visible = true; //Btn_Bulk_Post.Visible = 
                if (List_Post_Cnt > 0)
                {
                    lblPost.Text = "Total Posted: " + List_Post_Cnt.ToString();
                    lblPost.Visible = true;
                    lblPostAmt.Text = "Total Posted Amount: " + TotPostAmount.ToString();
                    lblPostAmt.Visible = true;
                }

                Post_App_Grid.CurrentCell = Post_App_Grid.Rows[Selected_Index].Cells[1];
                if (Can_Post_Atlest_One_APP)
                {
                    if (CaseSerPrivileges != null)
                    {
                        if (CaseSerPrivileges.AddPriv.Equals("true"))
                        { Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = true; }
                        else
                        { Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = false; }

                    }

                    // Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = true;
                }
            }
            this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            Post_App_Grid_SelectionChanged(Post_App_Grid, EventArgs.Empty);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //CASESP0Entity SP_Header_Rec;
        string Tmp_SPM_Sequence = "1";
        List<CASESPMEntity> CASESPM_SP_List;
        //CASESPMEntity Search_Entity = new CASESPMEntity();
        private void Fill_SP_Controls()
        {
            CASESPMEntity Search_Entity = new CASESPMEntity(true);
            List<CASESPMEntity> Tmp_CASESPM_List = new List<CASESPMEntity>();

            Search_Entity.agency = BaseForm.BaseAgency;
            Search_Entity.dept = BaseForm.BaseDept;
            Search_Entity.program = BaseForm.BaseProg;
            Search_Entity.year = null;                                // Year will be always Four-Spaces in CASESPM
            Search_Entity.app_no = Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString();

            Search_Entity.caseworker = Search_Entity.site = null;
            Search_Entity.startdate = Search_Entity.estdate = Search_Entity.compdate = null;
            Search_Entity.sel_branches = Search_Entity.have_addlbr = Search_Entity.date_lstc = null;
            Search_Entity.lstc_operator = Search_Entity.date_add = Search_Entity.add_operator = null;
            Search_Entity.Sp0_Desc = Search_Entity.Sp0_Validatetd = Search_Entity.Def_Program =
            Search_Entity.Bulk_Post = null;

            Search_Entity.service_plan = SP_Code;

            Tmp_CASESPM_List = _model.SPAdminData.Browse_CASESPM(Search_Entity, "Browse");

            //if (CASESPM_List.Count > 0)
            {
                if (Tmp_CASESPM_List.Count > 0)
                    Search_Entity = Tmp_CASESPM_List[0];


                //SP_CAMS_Details = _model.SPAdminData.Browse_CASESP2(SP_Code, null, null, null);
                Fill_SP_CAMS_Details(Search_Entity.service_plan, "P", null);

                //Get_Duplicate_MS_Posting_Dates();
            }
        }

        private void Get_Duplicate_MS_Posting_Dates()
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                int Row_Index = 0;
                foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                {
                    Row_Index = dr.Index;
                    if (dr.Cells["SP2_Type"].Value.ToString().Trim() == "MS" && dr.Cells["Dup_Ms_Date"].Value.ToString() == "N")
                    {
                        foreach (DataGridViewRow dr1 in SP_CAMS_Grid.Rows)
                        {
                            if (dr1.Cells["SP2_Type"].Value.ToString().Trim() == "MS")
                            {
                                if ((dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() == dr1.Cells["SP2_CAMS_Code"].Value.ToString().Trim() &&
                                     dr.Cells["SP2_Branch"].Value.ToString().Trim() == dr1.Cells["SP2_Branch"].Value.ToString().Trim() &&
                                     dr.Cells["SP2_Group"].Value.ToString().Trim() == dr1.Cells["SP2_Group"].Value.ToString().Trim()) &&
                                     //dr.Cells["SP2_Comp_Date"].Value.ToString().Trim() == dr1.Cells["SP2_Comp_Date"].Value.ToString().Trim() &&
                                     ((dr.Cells["Can_Post"].Value.ToString().Trim() == "Y" &&
                                       dr.Cells["SP2_Follow_Date"].Value.ToString().Trim() == dr1.Cells["SP2_Follow_Date"].Value.ToString().Trim()) ||
                                      (dr.Cells["Can_Post"].Value.ToString().Trim() == "N" &&
                                       dr.Cells["SP2_Comp_Date"].Value.ToString().Trim() == dr1.Cells["SP2_Comp_Date"].Value.ToString().Trim())) &&
                                     Row_Index != dr1.Index)
                                {
                                    dr1.Cells["Dup_Ms_Date"].Value = "Y";
                                    dr1.Cells["Post_Type"].Value = "4";
                                    dr1.Cells["Can_Post"].Value = "N";
                                    dr1.Cells["Remark"].Value = "Duplicate Psting Dates";
                                    SP_CAMS_Grid.Rows[dr1.Index].Cells["Remark"].Style.ForeColor = Color.Purple;
                                    SP_CAMS_Grid.Rows[dr1.Index].Cells["SP2_Comp_Date"].Style.ForeColor = Color.Purple;
                                }
                            }
                        }
                    }
                }
            }

            //            SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
            //SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;

        }

        private void Get_App_CASEACT_List()
        {
            CA_Pass_Entity.Agency = BaseForm.BaseAgency;
            CA_Pass_Entity.Dept = BaseForm.BaseDept;
            CA_Pass_Entity.Program = BaseForm.BaseProg;


            //CA_Pass_Entity.Year = Year;                        
            CA_Pass_Entity.Year = null;                             // Year will be always Four-Spaces in CASEACT
            CA_Pass_Entity.App_no = Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString();
            CA_Pass_Entity.Service_plan = SP_Code;
            CA_Pass_Entity.SPM_Seq = Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString();  //SP_Sequence;

            CA_Pass_Entity.Branch = CA_Pass_Entity.Group = CA_Pass_Entity.ACT_Code =
            CA_Pass_Entity.ACT_Date = CA_Pass_Entity.ACT_Seq = CA_Pass_Entity.Site = CA_Pass_Entity.Fund1 =
            CA_Pass_Entity.Fund2 = CA_Pass_Entity.Fund3 = CA_Pass_Entity.Caseworker = CA_Pass_Entity.Vendor_No =
            CA_Pass_Entity.Check_Date = CA_Pass_Entity.Check_No = CA_Pass_Entity.Cost = CA_Pass_Entity.Followup_On =
            CA_Pass_Entity.Followup_Comp = CA_Pass_Entity.Followup_By = CA_Pass_Entity.Refer_Data = CA_Pass_Entity.Cust_Code1 =
            CA_Pass_Entity.Cust_Value1 = CA_Pass_Entity.Cust_Code2 = CA_Pass_Entity.Cust_Value2 = CA_Pass_Entity.Cust_Code3 =
            CA_Pass_Entity.Cust_Value3 = CA_Pass_Entity.Lstc_Date = CA_Pass_Entity.Lsct_Operator = CA_Pass_Entity.Add_Date = null;
            CA_Pass_Entity.Add_Operator = CA_Pass_Entity.ACT_ID = CA_Pass_Entity.Bulk = CA_Pass_Entity.Act_PROG =
            CA_Pass_Entity.Cust_Code4 = CA_Pass_Entity.Cust_Value4 = CA_Pass_Entity.Cust_Code5 = CA_Pass_Entity.Cust_Value5 =
            CA_Pass_Entity.Units = CA_Pass_Entity.UOM = null;

            Tmp_SP_Activity_Details.Clear();
            Tmp_SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(CA_Pass_Entity, "Browse");
            SP_Activity_Details.Clear();
            bool Activity_Matched = false;
            string Comp_date = "";
            foreach (CASEACTEntity ActEnt in Tmp_SP_Activity_Details)
            {
                Activity_Matched = false;
                foreach (TemplateEntity Entity in TemplateList)
                {
                    Comp_date = "";
                    if (!string.IsNullOrEmpty(ActEnt.ACT_Date.Trim()))
                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                    if (Entity.BTPL_CACODE.Trim() + Entity.BTPL_GROUP.Trim() + Entity.BTPL_BRANCH.Trim() == ActEnt.ACT_Code.Trim() + ActEnt.Group.Trim() + ActEnt.Branch.Trim())
                    {
                        Activity_Matched = true;
                        SP_Activity_Details.Add(new CASEACTEntity(ActEnt));
                        break;
                    }

                }
                //foreach(ListItem List in Selected_CAMS_List)
                //{
                //    Comp_date = "";
                //    if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
                //        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                //    if (List.Text.ToString() == "CA" + ActEnt.ACT_Code.Trim() + ActEnt.Group + ActEnt.Branch )// + ActEnt.ACT_Seq)
                //    {
                //        Activity_Matched = true;
                //        SP_Activity_Details.Add(new CASEACTEntity(ActEnt));
                //        break;
                //    }
                //}
                ////if (!Activity_Matched)
                ////    SP_Activity_Details.Add(new CASEACTEntity(ActEnt));
            }
            //Selected_CAMS_List
        }

        CASEACTEntity CA_Pass_Entity = new CASEACTEntity();
        CASEMSEntity MS_Pass_Entity = new CASEMSEntity();
        private void Get_App_CASEMS_List()
        {
            Search_MS_Details.Agency = BaseForm.BaseAgency;
            Search_MS_Details.Dept = BaseForm.BaseDept;
            Search_MS_Details.Program = BaseForm.BaseProg;
            //Search_MS_Details.Year = Year; 
            Search_MS_Details.Year = null;                              // Year will be always Four-Spaces in CASEMS
            Search_MS_Details.App_no = Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString(); ;
            Search_MS_Details.Service_plan = SP_Code;
            Search_MS_Details.SPM_Seq = Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString();  //SP_Sequence;SP_Sequence;

            Search_MS_Details.Branch = Search_MS_Details.Group = Search_MS_Details.MS_Code =
            Search_MS_Details.ID = Search_MS_Details.Date = Search_MS_Details.CaseWorker = Search_MS_Details.Site =
            Search_MS_Details.Result = Search_MS_Details.OBF = Search_MS_Details.Add_Operator =
            Search_MS_Details.Lstc_Date = Search_MS_Details.Lsct_Operator = Search_MS_Details.Add_Date = Search_MS_Details.Bulk =
            Search_MS_Details.Acty_PROG = Search_MS_Details.Curr_Grp = null;

            Tmp_SP_MS_Details.Clear();
            Tmp_SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");
            SP_MS_Details.Clear();
            bool Activity_Matched = false;
            foreach (CASEMSEntity ActEnt in Tmp_SP_MS_Details)
            {
                Activity_Matched = false;
                foreach (ListItem List in Selected_CAMS_List)
                {
                    if (List.Text.ToString() == "MS" + ActEnt.MS_Code.Trim() + ActEnt.Group + ActEnt.Branch)
                    {
                        Activity_Matched = true;
                        SP_MS_Details.Add(new CASEMSEntity(ActEnt));
                        break;
                    }
                }
                //if (!Activity_Matched)
                //    SP_MS_Details.Add(new CASEMSEntity(ActEnt));
            }

        }

        int OBF_Type3_MS_Cnt = 0;
        string Sel_CAMS_Notes_Key = string.Empty;
        List<CASESP2Entity> SP_CAMS_Details = new List<CASESP2Entity>();
        List<CASEACTEntity> Tmp_SP_Activity_Details = new List<CASEACTEntity>();
        List<CASEACTEntity> SP_Activity_Details = new List<CASEACTEntity>();
        CASEACTEntity Search_Activity_Details = new CASEACTEntity();
        List<CASEMSEntity> Tmp_SP_MS_Details = new List<CASEMSEntity>();
        List<CASEMSEntity> SP_MS_Details = new List<CASEMSEntity>();
        CASEMSEntity Search_MS_Details = new CASEMSEntity();
        private void Fill_SP_CAMS_Details(string sp_Code, string Branch_Code, string Sel_CAMS_Key)
        {
            bool Atleast_One_Rec_To_Post = false;
            SP_CAMS_Grid.Rows.Clear();
            int Tmp_Bottom_Grid_Cnt = 0;
            OBF_Type3_MS_Cnt = 0;
            if (SP_CAMS_Details.Count > 0)
            {
                int rowIndex = 0, Sel_CAMS_Index = 0;
                bool CASEACT_Exists = false, CASEMS_Exists = false, MAp_CA_Posting_Date = false;
                string Comp_date = null, Followup = " ", Notes_Exists = "N", Notes_Key = null, CAMS_DESC = null, Tmp_MS_ID = string.Empty;
                string Add_Date = null, Add_Opr = null, Lstc_Date = null, Lstc_Opr = null, Posted_Year = null, OBF_Desc = "", Post_Type = "", Tmp_MS_Date = "";

                foreach (CASESP2Entity Entity in SP_CAMS_Details)
                {
                    CASEACT_Exists = CASEMS_Exists = false;
                    Add_Date = Add_Opr = Lstc_Date = Lstc_Opr = null;
                    Comp_date = Followup = Notes_Key = " "; Posted_Year = "    ";
                    Notes_Exists = "N"; OBF_Desc = "";

                    Notes_Key = ("000000".Substring(0, (6 - Entity.ServPlan.Length)) + Entity.ServPlan) + Entity.Type1 + Entity.Branch +
                                ("000000".Substring(0, (6 - Entity.Orig_Grp.ToString().Length)) + Entity.Orig_Grp.ToString()) + Entity.CamCd.Trim();
                    CASEACT_Exists = false;
                    if (Entity.Type1 == "CA")
                    {
                        foreach (TemplateEntity List in TemplateList)
                        {
                            if (List.BTPL_CACODE.ToString().Trim() + List.BTPL_GROUP.Trim() + List.BTPL_BRANCH == Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
                            {
                                CASEACT_Exists = false;
                                foreach (CASEACTEntity ActEnt in SP_Activity_Details)
                                {
                                    Comp_date = "";
                                    if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                    if (LookupDataAccess.Getdate(List.BTPL_DATE.Trim()) == Comp_date && (List.BTPL_CACODE.ToString().Trim() + List.BTPL_GROUP.Trim() + List.BTPL_BRANCH.Trim() == Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
                                            ActEnt.Service_plan == Entity.ServPlan && ActEnt.Branch == Entity.Branch && ActEnt.Group == Entity.Orig_Grp.ToString() &&
                                           ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim())
                                    {
                                        MAp_CA_Posting_Date = true;

                                        Comp_date = ActEnt.ACT_Date; Followup = ActEnt.Followup_On;
                                        Add_Date = ActEnt.Add_Date; Add_Opr = ActEnt.Add_Operator;
                                        Lstc_Date = ActEnt.Lstc_Date; Lstc_Opr = ActEnt.Lsct_Operator;
                                        Posted_Year = ActEnt.Year;

                                        Notes_Exists = "N";
                                        if (int.Parse(ActEnt.Notes_Count) > 0)
                                            Notes_Exists = "Y";

                                        if (!string.IsNullOrEmpty(Comp_date))
                                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                        if (!string.IsNullOrEmpty(Followup))
                                            Followup = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Followup.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                        CAMS_DESC = Entity.CAMS_Desc;
                                        if (CASEACT_Exists)
                                            CAMS_DESC = " ";

                                        CASEACT_Exists = true;

                                        switch (List.BTPL_OBF)
                                        {
                                            case "1": OBF_Desc = "App Only"; break;
                                            case "2": OBF_Desc = "All Mem"; break;
                                            case "3": OBF_Desc = "Sel HH Mem"; break;
                                        }
                                        //this.SP2_Add_Edit.Visible = false;
                                        //this.SP2_Save.Visible = false;
                                        if (Post_App_Grid.CurrentRow.Cells["SPM_Post_SW"].Value.ToString().Substring(0, 1) == "N")
                                            this.pb_Edit_CA.Visible = true;
                                        else if (Post_App_Grid.CurrentRow.Cells["SPM_Post_SW"].Value.ToString().Substring(0, 1) == "Y" && chkbShowPost.Checked == true)
                                            this.pb_Edit_CA.Visible = true;
                                        else this.pb_Edit_CA.Visible = false;
                                        Readonlytrue();

                                        string Remarks = "Service Already Posted";
                                        if (chkbShowPost.Checked)
                                        {
                                            TemplateEntity TempEntity = OriginalTemplateList.Find(u => u.BTPL_ID == List.BTPL_ID);
                                            if (TempEntity != null)
                                            {
                                                if (ActEnt.Units != TempEntity.BTPL_UNITS || ActEnt.Rate != TempEntity.BTPL_RATE || ActEnt.Cost != TempEntity.BTPL_TOTAL)
                                                    Remarks = "Service Posted (Template Exception)";
                                            }
                                            else
                                            {
                                                if (ActEnt.Units != List.BTPL_UNITS || ActEnt.Rate != List.BTPL_RATE || ActEnt.Cost != List.BTPL_TOTAL)
                                                    Remarks = "Service Posted (Template Exception)";
                                            }
                                        }
                                        else
                                        {
                                            if (ActEnt.Units != List.BTPL_UNITS || ActEnt.Rate != List.BTPL_RATE || ActEnt.Cost != List.BTPL_TOTAL)
                                                Remarks = "Service Posted (Template Exception)";
                                        }

                                        rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, OBF_Desc, Comp_date, " ", ActEnt.Units, ActEnt.Rate, ActEnt.Cost, Remarks, Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, Entity.CAMS_Active, ActEnt.ACT_ID, " ", "N", "N", Comp_date, "N", "2", List.BTPL_ID, Img_Saved);
                                        Tmp_Bottom_Grid_Cnt++;
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;

                                        if (!string.IsNullOrEmpty(Sel_CAMS_Key))
                                        {
                                            if (Sel_CAMS_Key == Entity.Orig_Grp.ToString() + Entity.Type1 + Entity.CamCd.Trim() + ActEnt.ACT_Seq.Trim())
                                                Sel_CAMS_Index = rowIndex;
                                        }
                                        break;

                                    }

                                }
                                if (!CASEACT_Exists)
                                {
                                    string[] Exp_Posting_Dates = { "", "", "Copy from Template" };
                                    switch (List.BTPL_OBF)
                                    {
                                        case "1": OBF_Desc = "App Only"; break;
                                        case "2": OBF_Desc = "All Mem"; break;
                                        case "3": OBF_Desc = "Sel HH Mem"; break;
                                    }

                                    Exp_Posting_Dates[0] = LookupDataAccess.Getdate(List.BTPL_DATE.Trim()); Exp_Posting_Dates[2] = "Copy from Template";

                                    if (!string.IsNullOrEmpty(Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim()))
                                    {
                                        string Complete_Date = string.Empty, FollowUp_Date = string.Empty, APP_SP_Start_Date = string.Empty;
                                        Complete_Date = List.BTPL_DATE.Trim();
                                        APP_SP_Start_Date = Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim();
                                        if (!string.IsNullOrEmpty(Complete_Date))
                                        {
                                            Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                            Exp_Posting_Dates[0] = Complete_Date;

                                            if (!string.IsNullOrEmpty(APP_SP_Start_Date))
                                            {
                                                if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
                                                {
                                                    //Exp_Posting_Dates[0] = APP_SP_Start_Date;
                                                    Exp_Posting_Dates[2] = "Prior to SPM Date"; // Less than SPM Date // "Dates As per Start Date";
                                                }
                                            }
                                        }
                                    }

                                    switch (Exp_Posting_Dates[2])
                                    {
                                        case "Prior to SPM Date": Post_Type = "3"; break;
                                        case "Copy from Template": Post_Type = "1"; break;
                                    }

                                    //if (Exp_Posting_Dates[2].ToString() == "Copy from Template")
                                    //{
                                    //this.SP2_Add_Edit.Visible = true; this.SP2_Save.Visible = true;
                                    if (Post_App_Grid.CurrentRow.Cells["SPM_Post_SW"].Value.ToString().Substring(0, 1) == "N")
                                        this.pb_Edit_CA.Visible = true;
                                    else if (Post_App_Grid.CurrentRow.Cells["SPM_Post_SW"].Value.ToString().Substring(0, 1) == "Y" && chkbShowPost.Checked == true)
                                        this.pb_Edit_CA.Visible = true;
                                    else this.pb_Edit_CA.Visible = false;
                                    Readonlytrue();
                                    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), List.BTPL_UNITS, List.BTPL_RATE, List.BTPL_TOTAL, Exp_Posting_Dates[2], Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type, List.BTPL_ID, Img_Saved);
                                    //}
                                    //else
                                    //{
                                    //    //this.SP2_Add_Edit.Visible = false; this.SP2_Save.Visible = false;
                                    //    this.pb_Edit_CA.Visible = false;
                                    //    Readonlytrue();
                                    //    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), List.BTPL_UNITS, List.BTPL_RATE, List.BTPL_AMOUNT, Exp_Posting_Dates[2], Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type, List.BTPL_ID, Img_Saved);
                                    //}

                                    Tmp_Bottom_Grid_Cnt++;
                                    if (Post_Type == "1")
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
                                    else
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;

                                    //SP_CAMS_Grid.Rows[rowIndex].Cells["Del_1"].ReadOnly = true;
                                    Atleast_One_Rec_To_Post = true;

                                }
                            }

                        }
                    }
                }
            }

            if (Tmp_Bottom_Grid_Cnt > 0 && Atleast_One_Rec_To_Post)
            {
                if (CaseSerPrivileges != null)
                {
                    if (CaseSerPrivileges.AddPriv.Equals("true"))
                        Btn_Bulk_Post.Visible = true;
                    else Btn_Bulk_Post.Visible = false;
                }
                if (TemplateList.Count > 0)
                { lblTempName.Visible = true; lblTempName.Text = "Template: " + TemplateList[0].BTPL_DESC.Trim(); }
            }
            else
            {
                Btn_Bulk_Post.Visible = false;
                if (TemplateList.Count > 0)
                { lblTempName.Visible = true; lblTempName.Text = "Template: " + TemplateList[0].BTPL_DESC.Trim(); }
            }
        }

        //private void Fill_SP_CAMS_Details1(string sp_Code, string Branch_Code, string Sel_CAMS_Key)
        //{
        //    bool Atleast_One_Rec_To_Post = false;
        //    SP_CAMS_Grid.Rows.Clear();
        //    int Tmp_Bottom_Grid_Cnt = 0;
        //    OBF_Type3_MS_Cnt = 0;
        //    if (SP_CAMS_Details.Count > 0)
        //    {
        //        int rowIndex = 0, Sel_CAMS_Index = 0;
        //        bool CASEACT_Exists = false, CASEMS_Exists = false, MAp_CA_Posting_Date = false;
        //        string Comp_date = null, Followup = " ", Notes_Exists = "N", Notes_Key = null, CAMS_DESC = null, Tmp_MS_ID = string.Empty;
        //        string Add_Date = null, Add_Opr = null, Lstc_Date = null, Lstc_Opr = null, Posted_Year = null, OBF_Desc = "", Post_Type = "", Tmp_MS_Date = ""; 

        //        foreach (CASESP2Entity Entity in SP_CAMS_Details)
        //        {
        //            //if (Entity.Branch == Branch_Code)
        //            {
        //                CASEACT_Exists = CASEMS_Exists = false;
        //                Add_Date = Add_Opr = Lstc_Date = Lstc_Opr = null;
        //                Comp_date = Followup = Notes_Key = " "; Posted_Year = "    ";
        //                Notes_Exists = "N"; OBF_Desc = "";

        //                Notes_Key = ("000000".Substring(0, (6 - Entity.ServPlan.Length)) + Entity.ServPlan) + Entity.Type1 + Entity.Branch +
        //                            ("000000".Substring(0, (6 - Entity.Orig_Grp.ToString().Length)) + Entity.Orig_Grp.ToString()) + Entity.CamCd.Trim();
        //                CASEACT_Exists = false;
        //                if (Entity.Type1 == "CA")
        //                {
        //                    //if (SP_Activity_Details.Count > 0)
        //                    //{
        //                    //CASEACT_Exists = false; 
        //                    //CATemplateEntity TempEnt = new CATemplateEntity();
        //                    TemplateEntity TempEnt = new TemplateEntity();
        //                    foreach (ListItem List in Selected_CAMS_List)
        //                    {



        //                            if (List.Text.ToString() == "CA" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
        //                        {
        //                            CASEACT_Exists = false;
        //                            foreach (CASEACTEntity ActEnt in SP_Activity_Details)
        //                            {
        //                                Comp_date = "";
        //                                if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
        //                                    Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                if (List.ID == Comp_date && (List.Text.ToString() == "CA" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
        //                                    ActEnt.Service_plan == Entity.ServPlan && ActEnt.Branch == Entity.Branch && ActEnt.Group == Entity.Orig_Grp.ToString() &&
        //                                   ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim())
        //                                {
        //                                    MAp_CA_Posting_Date = true;

        //                                    Comp_date = ActEnt.ACT_Date; Followup = ActEnt.Followup_On;
        //                                    Add_Date = ActEnt.Add_Date; Add_Opr = ActEnt.Add_Operator;
        //                                    Lstc_Date = ActEnt.Lstc_Date; Lstc_Opr = ActEnt.Lsct_Operator;
        //                                    Posted_Year = ActEnt.Year;

        //                                    Notes_Exists = "N";
        //                                    if (int.Parse(ActEnt.Notes_Count) > 0)
        //                                        Notes_Exists = "Y";

        //                                    if (!string.IsNullOrEmpty(Comp_date))
        //                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                                    if (!string.IsNullOrEmpty(Followup))
        //                                        Followup = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Followup.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                    CAMS_DESC = Entity.CAMS_Desc;
        //                                    if (CASEACT_Exists)
        //                                        CAMS_DESC = " ";

        //                                    CASEACT_Exists = true; //break;

        //                                    if (strCAOBO == "Y")
        //                                    {
        //                                        foreach (CASEACTEntity Ms_Ent in Source_SP_Activity_Details)
        //                                        {
        //                                            Tmp_MS_Date = Ms_Ent.ACT_Date;
        //                                            if (!string.IsNullOrEmpty(Comp_date.Trim()))
        //                                                Tmp_MS_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_MS_Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                            if (TemplateList.Count > 0)
        //                                            {
        //                                                TempEnt = TemplateList.Find(u => u.BTPL_CACODE.Trim() == Entity.CamCd.Trim() && Convert.ToDateTime(u.BTPL_DATE.ToString()) == Convert.ToDateTime(Ms_Ent.ACT_Date));
        //                                            }

        //                                            if (TempEnt != null)
        //                                            {
        //                                                switch (TempEnt.BTPL_OBF)
        //                                                {
        //                                                    case "1": OBF_Desc = "App Only"; break;
        //                                                    case "2": OBF_Desc = "All Mem"; break;
        //                                                    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                }
        //                                            }



        //                                            if (Ms_Ent.ACT_Code.Trim() == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && Tmp_MS_Date == Comp_date)
        //                                            {
        //                                                //switch (Ms_Ent.CA_OBF)
        //                                                //{
        //                                                //    case "1": OBF_Desc = "App Only"; break;
        //                                                //    case "2": OBF_Desc = "All Mem"; break;
        //                                                //    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                //}


        //                                            }
        //                                        }

        //                                        if (OBF_Desc == "Sel HH Mem")
        //                                            OBF_Type3_MS_Cnt++;
        //                                    }

        //                                    rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, OBF_Desc, Comp_date, " ", "CA Already Posted", Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, Entity.CAMS_Active, ActEnt.ACT_ID, " ", "N", "N", Comp_date, "N", "2");
        //                                    Tmp_Bottom_Grid_Cnt++;
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;

        //                                    if (!string.IsNullOrEmpty(Sel_CAMS_Key))
        //                                    {
        //                                        if (Sel_CAMS_Key == Entity.Orig_Grp.ToString() + Entity.Type1 + Entity.CamCd.Trim() + ActEnt.ACT_Seq.Trim())
        //                                            Sel_CAMS_Index = rowIndex;
        //                                    }
        //                                    break;
        //                                }
        //                            }

        //                            if (!CASEACT_Exists)
        //                            {
        //                              string[] Exp_Posting_Dates = { "", "", "Copy from Template" };
        //                                if (strCAOBO == "Y")
        //                                {
        //                                    if (TemplateList.Count > 0)
        //                                    {
        //                                        foreach (TemplateEntity TempEntity in TemplateList)
        //                                        {
        //                                            if (List.Text.ToString() == "CA" + TempEntity.BTPL_CACODE.Trim() + TempEntity.BTPL_GROUP + TempEntity.BTPL_BRANCH && Convert.ToDateTime(List.ID.Trim()) == Convert.ToDateTime(TempEntity.BTPL_DATE.Trim()))
        //                                            {
        //                                                switch (TempEntity.BTPL_OBF)
        //                                                {
        //                                                    case "1": OBF_Desc = "App Only"; break;
        //                                                    case "2": OBF_Desc = "All Mem"; break;
        //                                                    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                }

        //                                                Exp_Posting_Dates[0] = TempEntity.BTPL_DATE.Trim(); Exp_Posting_Dates[2]= "Copy from Template";

        //                                                if(!string.IsNullOrEmpty(Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim()))
        //                                                {
        //                                                    string Complete_Date = string.Empty, FollowUp_Date = string.Empty, APP_SP_Start_Date=string.Empty;
        //                                                    Complete_Date = TempEntity.BTPL_DATE.Trim();
        //                                                    APP_SP_Start_Date = Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim();
        //                                                    if (!string.IsNullOrEmpty(Complete_Date))
        //                                                    {
        //                                                        Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                                                        Exp_Posting_Dates[0] = Complete_Date;

        //                                                        if (!string.IsNullOrEmpty(APP_SP_Start_Date))
        //                                                        {
        //                                                            if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
        //                                                            {
        //                                                                //Exp_Posting_Dates[0] = APP_SP_Start_Date;
        //                                                                Exp_Posting_Dates[2] = "Prior to SPM Date"; // Less than SPM Date // "Dates As per Start Date";
        //                                                            }
        //                                                        }
        //                                                    }
        //                                                }


        //                                                switch (Exp_Posting_Dates[2])
        //                                                {
        //                                                    case "Prior to SPM Date": Post_Type = "3"; break;
        //                                                    case "Copy from Template": Post_Type = "1"; break;
        //                                                }

        //                                                break;
        //                                            }
        //                                        }

        //                                    }



        //                                }


        //                                //string[] Exp_Posting_Dates = Get_Expected_posting_Dates("CA", List.Value.ToString(), Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim());
        //                                ////Added by Sudheer on 03/10/2021
        //                                //if (!string.IsNullOrEmpty(Exp_Posting_Dates[0].Trim()))
        //                                //{
        //                                //    Exp_Posting_Dates[0] = List.ID.ToString();
        //                                //    Exp_Posting_Dates[2] = "Copy from Template";
        //                                //}

        //                                //switch (Exp_Posting_Dates[2])
        //                                //{
        //                                //    case "Prior to SPM Date": Post_Type = "3"; break;
        //                                //    case "Copy from Template": Post_Type = "1"; break;
        //                                //}



        //                                //rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, " ", (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[1], Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Comp_date, "Y", Post_Type);
        //                                rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, Exp_Posting_Dates[0], (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type);
        //                                //}
        //                                //else
        //                                //    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, Exp_Posting_Dates[0], Exp_Posting_Dates[1], Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", "N", "N");

        //                                Tmp_Bottom_Grid_Cnt++;
        //                                if (Post_Type == "1")
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
        //                                else
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;

        //                                //SP_CAMS_Grid.Rows[rowIndex].Cells["Del_1"].ReadOnly = true;
        //                                Atleast_One_Rec_To_Post = true;
        //                                //break;
        //                                //    }
        //                                //}
        //                            }
        //                        }
        //                    }


        //                }
        //                else
        //                {

        //                }
        //            }
        //        }
        //    }

        //    if (Tmp_Bottom_Grid_Cnt > 0 && Atleast_One_Rec_To_Post)
        //    {
        //        if (CaseSerPrivileges != null)
        //        {
        //            if (CaseSerPrivileges.AddPriv.Equals("true"))
        //                Btn_Bulk_Post.Visible = true;
        //            else Btn_Bulk_Post.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        Btn_Bulk_Post.Visible = false;
        //    }
        //}

        private void Fill_ADDSP_CAMS_Details(string sp_Code, string Branch_Code, string Sel_CAMS_Key)
        {
            bool Atleast_One_Rec_To_Post = false;
            SP_CAMS_Grid.Rows.Clear();
            int Tmp_Bottom_Grid_Cnt = 0;
            OBF_Type3_MS_Cnt = 0;
            if (SP_CAMS_Details.Count > 0)
            {
                int rowIndex = 0, Sel_CAMS_Index = 0;
                bool CASEACT_Exists = false, CASEMS_Exists = false, MAp_CA_Posting_Date = false;
                string Comp_date = null, Followup = " ", Notes_Exists = "N", Notes_Key = null, CAMS_DESC = null, Tmp_MS_ID = string.Empty;
                string Add_Date = null, Add_Opr = null, Lstc_Date = null, Lstc_Opr = null, Posted_Year = null, OBF_Desc = "", Post_Type = "", Tmp_MS_Date = "";

                foreach (CASESP2Entity Entity in SP_CAMS_Details)
                {
                    CASEACT_Exists = CASEMS_Exists = false;
                    Add_Date = Add_Opr = Lstc_Date = Lstc_Opr = null;
                    Comp_date = Followup = Notes_Key = " "; Posted_Year = "    ";
                    Notes_Exists = "N"; OBF_Desc = "";

                    Notes_Key = ("000000".Substring(0, (6 - Entity.ServPlan.Length)) + Entity.ServPlan) + Entity.Type1 + Entity.Branch +
                                ("000000".Substring(0, (6 - Entity.Orig_Grp.ToString().Length)) + Entity.Orig_Grp.ToString()) + Entity.CamCd.Trim();
                    CASEACT_Exists = false;

                    if (Entity.Type1 == "CA")
                    {
                        foreach (TemplateEntity List in TemplateList)
                        {
                            if (List.BTPL_CACODE.ToString().Trim() + List.BTPL_GROUP.Trim() + List.BTPL_BRANCH.Trim() == Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
                            {
                                CASEACT_Exists = false;
                                foreach (CASEACTEntity ActEnt in SP_Activity_Details)
                                {
                                    Comp_date = "";
                                    if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                    if (List.BTPL_DATE == Comp_date && (List.BTPL_CACODE.ToString().Trim() + List.BTPL_GROUP.Trim() + List.BTPL_BRANCH.Trim() == Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
                                            ActEnt.Service_plan == Entity.ServPlan && ActEnt.Branch == Entity.Branch && ActEnt.Group == Entity.Orig_Grp.ToString() &&
                                           ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim())
                                    {
                                        MAp_CA_Posting_Date = true;

                                        Comp_date = ActEnt.ACT_Date; Followup = ActEnt.Followup_On;
                                        Add_Date = ActEnt.Add_Date; Add_Opr = ActEnt.Add_Operator;
                                        Lstc_Date = ActEnt.Lstc_Date; Lstc_Opr = ActEnt.Lsct_Operator;
                                        Posted_Year = ActEnt.Year;

                                        Notes_Exists = "N";
                                        if (int.Parse(ActEnt.Notes_Count) > 0)
                                            Notes_Exists = "Y";

                                        if (!string.IsNullOrEmpty(Comp_date))
                                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                                        if (!string.IsNullOrEmpty(Followup))
                                            Followup = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Followup.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                        CAMS_DESC = Entity.CAMS_Desc;
                                        if (CASEACT_Exists)
                                            CAMS_DESC = " ";

                                        CASEACT_Exists = true; //break;

                                        switch (List.BTPL_OBF)
                                        {
                                            case "1": OBF_Desc = "App Only"; break;
                                            case "2": OBF_Desc = "All Mem"; break;
                                            case "3": OBF_Desc = "Sel HH Mem"; break;
                                        }
                                        //this.SP2_Add_Edit.Visible = false;
                                        //this.SP2_Save.Visible = false;
                                        this.pb_Edit_CA.Visible = false;
                                        Readonlytrue();
                                        string Remarks = "CA Already Posted";
                                        if (ActEnt.Units != List.BTPL_UNITS || ActEnt.Rate != List.BTPL_RATE || ActEnt.Cost != List.BTPL_TOTAL)
                                            Remarks = "CA Posted (Template Exception)";

                                        rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, OBF_Desc, Comp_date, " ", ActEnt.Units, ActEnt.Rate, ActEnt.Cost, Remarks, Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, Entity.CAMS_Active, ActEnt.ACT_ID, " ", "N", "N", Comp_date, "N", List.BTPL_OBF, List.BTPL_ID, Img_Saved);
                                        Tmp_Bottom_Grid_Cnt++;
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;
                                        if (!string.IsNullOrEmpty(Sel_CAMS_Key))
                                        {
                                            if (Sel_CAMS_Key == Entity.Orig_Grp.ToString() + Entity.Type1 + Entity.CamCd.Trim() + ActEnt.ACT_Seq.Trim())
                                                Sel_CAMS_Index = rowIndex;
                                        }
                                        break;

                                    }
                                }
                                if (!CASEACT_Exists)
                                {
                                    Comp_date = "";
                                    if (!string.IsNullOrEmpty(List.BTPL_DATE.Trim()))
                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(List.BTPL_DATE.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                    switch (List.BTPL_OBF)
                                    {
                                        case "1": OBF_Desc = "App Only"; break;
                                        case "2": OBF_Desc = "All Mem"; break;
                                        case "3": OBF_Desc = "Sel HH Mem"; break;
                                    }
                                    string[] Exp_Posting_Dates = { LookupDataAccess.Getdate(List.BTPL_DATE.ToString().Trim()), "", "Copy from Template" };
                                    switch (Exp_Posting_Dates[2])
                                    {
                                        case "Prior to SPM Date": Post_Type = "3"; break;
                                        case "Copy from Template": Post_Type = "1"; break;
                                    }

                                    if (Exp_Posting_Dates[2].ToString() == "Copy from Template")
                                    {
                                        //this.SP2_Add_Edit.Visible = true; this.SP2_Save.Visible = true;
                                        this.pb_Edit_CA.Visible = true;
                                        Readonlytrue();
                                        rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), List.BTPL_UNITS, List.BTPL_RATE, List.BTPL_TOTAL, Exp_Posting_Dates[2], Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type, List.BTPL_ID, Img_Saved);
                                    }
                                    else
                                    {
                                        //this.SP2_Add_Edit.Visible = false; this.SP2_Save.Visible = false;
                                        this.pb_Edit_CA.Visible = true;
                                        Readonlytrue();
                                        rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), List.BTPL_UNITS, List.BTPL_RATE, List.BTPL_TOTAL, Exp_Posting_Dates[2], Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type, List.BTPL_ID, Img_Saved);
                                    }

                                    Tmp_Bottom_Grid_Cnt++;
                                    if (Post_Type == "1")
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
                                    else
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;

                                    //SP_CAMS_Grid.Rows[rowIndex].Cells["Del_1"].ReadOnly = true;
                                    Atleast_One_Rec_To_Post = true;
                                }
                            }
                        }
                    }
                }
            }

            if (Tmp_Bottom_Grid_Cnt > 0 && Atleast_One_Rec_To_Post)
            {
                if (CaseSerPrivileges != null)
                {
                    if (CaseSerPrivileges.AddPriv.Equals("true"))
                        Btn_Bulk_Post.Visible = true;
                    else Btn_Bulk_Post.Visible = false;
                }
                if (TemplateList.Count > 0)
                { lblTempName.Visible = true; lblTempName.Text = "Template: " + TemplateList[0].BTPL_DESC.Trim(); }
            }
            else
            {
                Btn_Bulk_Post.Visible = false;
                lblTempName.Visible = false; lblTempName.Text = "";
            }
        }

        //private void Fill_ADDSP_CAMS_Details1(string sp_Code, string Branch_Code, string Sel_CAMS_Key)
        //{
        //    bool Atleast_One_Rec_To_Post = false;
        //    SP_CAMS_Grid.Rows.Clear();
        //    int Tmp_Bottom_Grid_Cnt = 0;
        //    OBF_Type3_MS_Cnt = 0;
        //    if (SP_CAMS_Details.Count > 0)
        //    {
        //        int rowIndex = 0, Sel_CAMS_Index = 0;
        //        bool CASEACT_Exists = false, CASEMS_Exists = false, MAp_CA_Posting_Date = false;
        //        string Comp_date = null, Followup = " ", Notes_Exists = "N", Notes_Key = null, CAMS_DESC = null, Tmp_MS_ID = string.Empty;
        //        string Add_Date = null, Add_Opr = null, Lstc_Date = null, Lstc_Opr = null, Posted_Year = null, OBF_Desc = "", Post_Type = "", Tmp_MS_Date = "";

        //        foreach (CASESP2Entity Entity in SP_CAMS_Details)
        //        {
        //            //if (Entity.Branch == Branch_Code)
        //            {
        //                CASEACT_Exists = CASEMS_Exists = false;
        //                Add_Date = Add_Opr = Lstc_Date = Lstc_Opr = null;
        //                Comp_date = Followup = Notes_Key = " "; Posted_Year = "    ";
        //                Notes_Exists = "N"; OBF_Desc = "";

        //                Notes_Key = ("000000".Substring(0, (6 - Entity.ServPlan.Length)) + Entity.ServPlan) + Entity.Type1 + Entity.Branch +
        //                            ("000000".Substring(0, (6 - Entity.Orig_Grp.ToString().Length)) + Entity.Orig_Grp.ToString()) + Entity.CamCd.Trim();
        //                CASEACT_Exists = false;
        //                if (Entity.Type1 == "CA")
        //                {
        //                    //if (SP_Activity_Details.Count > 0)
        //                    //{
        //                    //CASEACT_Exists = false; 
        //                    //CATemplateEntity TempEnt = new CATemplateEntity();
        //                    TemplateEntity TempEnt = new TemplateEntity();
        //                    foreach (ListItem List in Selected_CAMS_List)
        //                    {



        //                        if (List.Text.ToString() == "CA" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
        //                        {
        //                            CASEACT_Exists = false;
        //                            foreach (CASEACTEntity ActEnt in SP_Activity_Details)
        //                            {
        //                                Comp_date = "";
        //                                if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
        //                                    Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                if (List.ID == Comp_date && (List.Text.ToString() == "CA" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
        //                                    ActEnt.Service_plan == Entity.ServPlan && ActEnt.Branch == Entity.Branch && ActEnt.Group == Entity.Orig_Grp.ToString() &&
        //                                   ActEnt.ACT_Code.Trim() == Entity.CamCd.Trim())
        //                                {
        //                                    MAp_CA_Posting_Date = true;

        //                                    Comp_date = ActEnt.ACT_Date; Followup = ActEnt.Followup_On;
        //                                    Add_Date = ActEnt.Add_Date; Add_Opr = ActEnt.Add_Operator;
        //                                    Lstc_Date = ActEnt.Lstc_Date; Lstc_Opr = ActEnt.Lsct_Operator;
        //                                    Posted_Year = ActEnt.Year;

        //                                    Notes_Exists = "N";
        //                                    if (int.Parse(ActEnt.Notes_Count) > 0)
        //                                        Notes_Exists = "Y";

        //                                    if (!string.IsNullOrEmpty(Comp_date))
        //                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                                    if (!string.IsNullOrEmpty(Followup))
        //                                        Followup = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Followup.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                    CAMS_DESC = Entity.CAMS_Desc;
        //                                    if (CASEACT_Exists)
        //                                        CAMS_DESC = " ";

        //                                    CASEACT_Exists = true; //break;

        //                                    if (strCAOBO == "Y")
        //                                    {
        //                                        foreach (CASEACTEntity Ms_Ent in Source_SP_Activity_Details)
        //                                        {
        //                                            Tmp_MS_Date = Ms_Ent.ACT_Date;
        //                                            if (!string.IsNullOrEmpty(Comp_date.Trim()))
        //                                                Tmp_MS_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_MS_Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                            if (TemplateList.Count > 0)
        //                                            {
        //                                                TempEnt = TemplateList.Find(u => u.BTPL_CACODE.Trim() == Entity.CamCd.Trim() && Convert.ToDateTime(u.BTPL_DATE.ToString()) == Convert.ToDateTime(Ms_Ent.ACT_Date));
        //                                            }

        //                                            if (TempEnt != null)
        //                                            {
        //                                                switch (TempEnt.BTPL_OBF)
        //                                                {
        //                                                    case "1": OBF_Desc = "App Only"; break;
        //                                                    case "2": OBF_Desc = "All Mem"; break;
        //                                                    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                }
        //                                            }



        //                                            if (Ms_Ent.ACT_Code.Trim() == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && Tmp_MS_Date == Comp_date)
        //                                            {
        //                                                //switch (Ms_Ent.CA_OBF)
        //                                                //{
        //                                                //    case "1": OBF_Desc = "App Only"; break;
        //                                                //    case "2": OBF_Desc = "All Mem"; break;
        //                                                //    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                //}


        //                                            }
        //                                        }

        //                                        if (OBF_Desc == "Sel HH Mem")
        //                                            OBF_Type3_MS_Cnt++;
        //                                    }

        //                                    rowIndex = SP_CAMS_Grid.Rows.Add(false, CAMS_DESC, OBF_Desc, Comp_date, " ", "CA Already Posted", Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key + ActEnt.ACT_Seq, ActEnt.ACT_Seq, Entity.CAMS_Desc, ActEnt.Year, Entity.CAMS_Active, ActEnt.ACT_ID, " ", "N", "N", Comp_date, "N", "2");
        //                                    Tmp_Bottom_Grid_Cnt++;
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;

        //                                    if (!string.IsNullOrEmpty(Sel_CAMS_Key))
        //                                    {
        //                                        if (Sel_CAMS_Key == Entity.Orig_Grp.ToString() + Entity.Type1 + Entity.CamCd.Trim() + ActEnt.ACT_Seq.Trim())
        //                                            Sel_CAMS_Index = rowIndex;
        //                                    }
        //                                    break;
        //                                }
        //                            }

        //                            if (!CASEACT_Exists)
        //                            {
        //                                if (strCAOBO == "Y")
        //                                {
        //                                    foreach (CASEACTEntity Ms_Ent in Source_SP_Activity_Details)
        //                                    {
        //                                        Comp_date = "";
        //                                        if (!string.IsNullOrEmpty(Ms_Ent.ACT_Date.Trim()))
        //                                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ms_Ent.ACT_Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                        if (TemplateList.Count > 0)
        //                                        {
        //                                            TempEnt = TemplateList.Find(u => u.BTPL_CACODE.Trim() == Entity.CamCd.Trim()); //
        //                                        }

        //                                        if (TempEnt != null)
        //                                        {
        //                                            switch (TempEnt.BTPL_OBF)
        //                                            {
        //                                                case "1": OBF_Desc = "App Only"; break;
        //                                                case "2": OBF_Desc = "All Mem"; break;
        //                                                case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                            }
        //                                        }

        //                                        //OBF_Desc = " ";
        //                                        if (Ms_Ent.ACT_Code.Trim() == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && List.ID.ToString() == Comp_date)
        //                                        {
        //                                            List.Value = Ms_Ent.ACT_ID;
        //                                            //switch (Ms_Ent.CA_OBF)
        //                                            //{
        //                                            //    case "1": OBF_Desc = "App Only"; break;
        //                                            //    case "2": OBF_Desc = "All Mem"; break;
        //                                            //    case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                            //}
        //                                        }
        //                                    }

        //                                    if (OBF_Desc == "Sel HH Mem")
        //                                        OBF_Type3_MS_Cnt++;
        //                                }

        //                                //string[] Exp_Posting_Dates = Get_Expected_posting_Dates("CA", List.Value.ToString(), Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim());
        //                                ////Added by Sudheer on 03/10/2021
        //                                //if (!string.IsNullOrEmpty(Exp_Posting_Dates[0].Trim()))
        //                                //{
        //                                //    Exp_Posting_Dates[0] = List.ID.ToString();
        //                                //    Exp_Posting_Dates[2] = "Copy from Template";
        //                                //}
        //                                string[] Exp_Posting_Dates = { List.ID.ToString(), "", "Copy from Template" };
        //                                switch (Exp_Posting_Dates[2])
        //                                {
        //                                    case "Prior to SPM Date": Post_Type = "3"; break;
        //                                    case "Copy from Template": Post_Type = "1"; break;
        //                                }


        //                                //rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, " ", (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[1], Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Comp_date, "Y", Post_Type);
        //                                rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type);
        //                                //}
        //                                //else
        //                                //    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, Exp_Posting_Dates[0], Exp_Posting_Dates[1], Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", "N", "N");

        //                                Tmp_Bottom_Grid_Cnt++;
        //                                if (Post_Type == "1")
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
        //                                else
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;

        //                                //SP_CAMS_Grid.Rows[rowIndex].Cells["Del_1"].ReadOnly = true;
        //                                Atleast_One_Rec_To_Post = true;
        //                                //break;
        //                                //    }
        //                                //}
        //                            }
        //                        }
        //                    }


        //                }
        //                else
        //                {

        //                    foreach (ListItem List in Selected_CAMS_List)
        //                    {
        //                        if (List.Text.ToString() == "MS" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
        //                        {
        //                            CASEACT_Exists = false;

        //                            foreach (CASEMSEntity MSEnt in SP_MS_Details)
        //                            {
        //                                Comp_date = "";
        //                                if (!string.IsNullOrEmpty(MSEnt.Date.Trim()))
        //                                    Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(MSEnt.Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                if (List.ID.ToString() == Comp_date && (List.Text.ToString() == "MS" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
        //                                    MSEnt.Service_plan == Entity.ServPlan && MSEnt.Branch == Entity.Branch && MSEnt.Group == Entity.Orig_Grp.ToString() &&
        //                                    MSEnt.MS_Code.Trim() == Entity.CamCd.Trim())
        //                                {
        //                                    CASEACT_Exists = MAp_CA_Posting_Date = true;


        //                                    Comp_date = MSEnt.Date; //Followup = MSEnt.Followup_On;
        //                                    Add_Date = MSEnt.Add_Date; Add_Opr = MSEnt.Add_Operator;
        //                                    Lstc_Date = MSEnt.Lstc_Date; Lstc_Opr = MSEnt.Lsct_Operator;
        //                                    Posted_Year = MSEnt.Year;

        //                                    if (!string.IsNullOrEmpty(Comp_date.Trim()))
        //                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                    //OBF_Desc = " ";
        //                                    foreach (CASEMSEntity Ms_Ent in SourceSP_CAMS_Details)
        //                                    {
        //                                        Tmp_MS_Date = Ms_Ent.Date;
        //                                        if (!string.IsNullOrEmpty(Comp_date.Trim()))
        //                                            Tmp_MS_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Tmp_MS_Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                        if (Ms_Ent.MS_Code == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && Tmp_MS_Date == Comp_date)
        //                                        {
        //                                            switch (Ms_Ent.OBF)
        //                                            {
        //                                                case "1": OBF_Desc = "App Only"; break;
        //                                                case "2": OBF_Desc = "All Mem"; break;
        //                                                case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                            }
        //                                        }
        //                                    }

        //                                    if (OBF_Desc == "Sel HH Mem")
        //                                        OBF_Type3_MS_Cnt++;

        //                                    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, Comp_date, " ", "MS Already Posted", Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, Tmp_MS_ID, " ", "N", "N", Comp_date, "N", "2");
        //                                    Tmp_Bottom_Grid_Cnt++;
        //                                    SP_CAMS_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue; //Color.Peru; //Color.DarkTurquoise;
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;


        //                                    break;
        //                                }
        //                            }

        //                            if (!CASEACT_Exists)
        //                            {
        //                                //foreach (ListItem List in Selected_CAMS_List)
        //                                //{
        //                                //if (List.Text.ToString() == "MS" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch)
        //                                //{
        //                                foreach (CASEMSEntity Ms_Ent in SourceSP_CAMS_Details)
        //                                {
        //                                    Comp_date = "";
        //                                    if (!string.IsNullOrEmpty(Ms_Ent.Date.Trim()))
        //                                        Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ms_Ent.Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                    //OBF_Desc = " ";
        //                                    if (Ms_Ent.MS_Code == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && List.ID.ToString() == Comp_date)
        //                                    {
        //                                        switch (Ms_Ent.OBF)
        //                                        {
        //                                            case "1": OBF_Desc = "App Only"; break;
        //                                            case "2": OBF_Desc = "All Mem"; break;
        //                                            case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                        }
        //                                    }
        //                                }

        //                                if (OBF_Desc == "Sel HH Mem")
        //                                    OBF_Type3_MS_Cnt++;

        //                                string[] Exp_Posting_Dates = Get_Expected_posting_Dates("MS", List.Value.ToString(), Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim());
        //                                switch (Exp_Posting_Dates[2])
        //                                {
        //                                    case "Prior to SPM Date": Post_Type = "3"; break;
        //                                    case "Copy from Template": Post_Type = "1"; break;
        //                                }

        //                                if (Exp_Posting_Dates[2] == "Prior to SPM Date")
        //                                {
        //                                    foreach (CASEMSEntity MSEnt in SP_MS_Details)
        //                                    {
        //                                        Comp_date = "";
        //                                        if (!string.IsNullOrEmpty(MSEnt.Date.Trim()))
        //                                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(MSEnt.Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                        if (Exp_Posting_Dates[0] == Comp_date && (List.Text.ToString() == "MS" + Entity.CamCd.Trim() + Entity.Orig_Grp + Entity.Branch) &&
        //                                            MSEnt.Service_plan == Entity.ServPlan && MSEnt.Branch == Entity.Branch && MSEnt.Group == Entity.Orig_Grp.ToString() &&
        //                                            MSEnt.MS_Code.Trim() == Entity.CamCd.Trim())
        //                                        {
        //                                            CASEACT_Exists = MAp_CA_Posting_Date = true;
        //                                            Comp_date = MSEnt.Date; //Followup = MSEnt.Followup_On;
        //                                            Add_Date = MSEnt.Add_Date; Add_Opr = MSEnt.Add_Operator;
        //                                            Lstc_Date = MSEnt.Lstc_Date; Lstc_Opr = MSEnt.Lsct_Operator;
        //                                            Posted_Year = MSEnt.Year;

        //                                            //OBF_Desc = " ";
        //                                            foreach (CASEMSEntity Ms_Ent in SourceSP_CAMS_Details)
        //                                            {
        //                                                if (Ms_Ent.MS_Code == Entity.CamCd.Trim() && Ms_Ent.Group == Entity.Orig_Grp.ToString() && Ms_Ent.Branch == Entity.Branch && Ms_Ent.Date == Comp_date)
        //                                                {
        //                                                    switch (Ms_Ent.OBF)
        //                                                    {
        //                                                        case "1": OBF_Desc = "App Only"; break;
        //                                                        case "2": OBF_Desc = "All Mem"; break;
        //                                                        case "3": OBF_Desc = "Sel HH Mem"; break;
        //                                                    }
        //                                                }
        //                                            }

        //                                            if (OBF_Desc == "Sel HH Mem")
        //                                                OBF_Type3_MS_Cnt++;

        //                                            if (!string.IsNullOrEmpty(Comp_date.Trim()))
        //                                                Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Comp_date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

        //                                            Post_Type = "2";
        //                                            rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, Comp_date, " ", "MS Already Posted", Img_Edit, Entity.Type1, Entity.CamCd.Trim(), "C", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, Tmp_MS_ID, " ", "N", "N", Comp_date, "N", Post_Type);
        //                                            Tmp_Bottom_Grid_Cnt++;

        //                                            SP_CAMS_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue; //Color.Peru; //Color.DarkTurquoise;
        //                                            SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                            SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;
        //                                            break;
        //                                        }
        //                                    }
        //                                }

        //                                if (!CASEACT_Exists)
        //                                {
        //                                    //rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[1], Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Comp_date, "Y", Post_Type);
        //                                    rowIndex = SP_CAMS_Grid.Rows.Add(false, Entity.CAMS_Desc, OBF_Desc, (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), (Post_Type == "3" ? " " : Exp_Posting_Dates[0]), Exp_Posting_Dates[2], Img_Add, Entity.Type1, Entity.CamCd.Trim(), "A", Entity.Branch, Entity.Orig_Grp, Notes_Exists, Notes_Key, " ", Entity.CAMS_Desc, Posted_Year, Entity.CAMS_Active, " ", " ", "N", "N", Exp_Posting_Dates[0], (Post_Type == "3" ? "N" : "Y"), Post_Type);
        //                                    Tmp_Bottom_Grid_Cnt++;
        //                                    SP_CAMS_Grid.Rows[rowIndex].Cells["Del_1"].ReadOnly = true;
        //                                    SP_CAMS_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue; //Color.Peru; //Color.DarkTurquoise;
        //                                    if (Post_Type == "1")
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
        //                                    else
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["Remark"].Style.ForeColor =
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Comp_Date"].Style.ForeColor =
        //                                        SP_CAMS_Grid.Rows[rowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Red;
        //                                }
        //                                Atleast_One_Rec_To_Post = true;
        //                                //break;
        //                                //}
        //                                //}
        //                            }
        //                        }
        //                    }




        //                }
        //            }
        //        }
        //    }

        //    if (Tmp_Bottom_Grid_Cnt > 0 && Atleast_One_Rec_To_Post)
        //    {
        //        if (CaseSerPrivileges != null)
        //        {
        //            if (CaseSerPrivileges.AddPriv.Equals("true"))
        //                Btn_Bulk_Post.Visible = true;
        //            else Btn_Bulk_Post.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        Btn_Bulk_Post.Visible = false;
        //    }
        //}

        private string[] Get_Expected_posting_Dates(string CAMS_Type, string CAMS_ID, string APP_SP_Start_Date)
        {
            string[] Exp_Posting_Dates = new string[3];
            Exp_Posting_Dates[0] = Exp_Posting_Dates[1] = APP_SP_Start_Date; Exp_Posting_Dates[2] = " ";
            string Complete_Date = string.Empty, FollowUp_Date = string.Empty;
            if (CAMS_Type == "CA")
            {
                foreach (CASEACTEntity Entity in Source_SP_Activity_Details)
                {
                    if (CAMS_ID == Entity.ACT_ID)
                    {
                        Complete_Date = Entity.ACT_Date;
                        Exp_Posting_Dates[0] = " ";
                        Exp_Posting_Dates[2] = "Copy from Template"; //Copied form Source
                        if (!string.IsNullOrEmpty(Complete_Date))
                        {
                            Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            Exp_Posting_Dates[0] = Complete_Date;

                            if (!string.IsNullOrEmpty(APP_SP_Start_Date))
                            {
                                if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
                                {
                                    Exp_Posting_Dates[0] = APP_SP_Start_Date;
                                    Exp_Posting_Dates[2] = "Prior to SPM Date"; // Less than SPM Date // "Dates As per Start Date";
                                }
                            }
                        }

                        FollowUp_Date = Entity.Followup_On;
                        Exp_Posting_Dates[1] = " ";
                        if (!string.IsNullOrEmpty(FollowUp_Date))
                        {
                            FollowUp_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(FollowUp_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            Exp_Posting_Dates[1] = FollowUp_Date;
                            if (!string.IsNullOrEmpty(APP_SP_Start_Date))
                            {
                                if (Convert.ToDateTime(FollowUp_Date) < Convert.ToDateTime(APP_SP_Start_Date))
                                {
                                    Exp_Posting_Dates[1] = APP_SP_Start_Date;
                                    Exp_Posting_Dates[2] = "Prior to SPM Date";
                                }
                            }
                        }
                        else if (Exp_Posting_Dates[2] == "Prior to SPM Date")
                            Exp_Posting_Dates[1] = APP_SP_Start_Date;


                        break;
                    }
                }
            }
            else
            {
                foreach (CASEMSEntity Entity in SourceSP_CAMS_Details)
                {
                    if (CAMS_ID == Entity.ID)
                    {
                        Complete_Date = Entity.Date;
                        Exp_Posting_Dates[0] = APP_SP_Start_Date;
                        Exp_Posting_Dates[1] = " ";
                        Exp_Posting_Dates[2] = "Copy from Template";
                        if (!string.IsNullOrEmpty(Complete_Date))
                        {
                            Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            Exp_Posting_Dates[0] = Complete_Date;

                            if (!string.IsNullOrEmpty(APP_SP_Start_Date))
                            {
                                if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
                                {
                                    Exp_Posting_Dates[0] = APP_SP_Start_Date;
                                    Exp_Posting_Dates[2] = "Prior to SPM Date";
                                }
                            }
                        }

                        break;
                    }
                }
            }

            Exp_Posting_Dates[1] = " ";
            return Exp_Posting_Dates;
        }

        //private string[] Get_Expected_Intake_posting_Dates(string CAMS_Type, string CAMS_ID, string APP_SP_Start_Date)
        //{
        //    string[] Exp_Posting_Dates = new string[3];
        //    Exp_Posting_Dates[0] = Exp_Posting_Dates[1] = APP_SP_Start_Date; Exp_Posting_Dates[2] = " ";
        //    string Complete_Date = string.Empty, FollowUp_Date = string.Empty;
        //    if (CAMS_Type == "CA")
        //    {
        //        foreach (CASEACTEntity Entity in Source_SP_Activity_Details)
        //        {
        //            if (CAMS_ID == Entity.ACT_ID)
        //            {
        //                Complete_Date = Entity.ACT_Date;
        //                Exp_Posting_Dates[0] = " ";
        //                Exp_Posting_Dates[2] = "Copy from Template";
        //                if (!string.IsNullOrEmpty(Complete_Date))
        //                {
        //                    Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                    Exp_Posting_Dates[0] = Complete_Date;
        //                    //if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
        //                    //{
        //                    //    Exp_Posting_Dates[0] = APP_SP_Start_Date;
        //                    //    Exp_Posting_Dates[2] = "Prior to SPM Date";
        //                    //}
        //                }

        //                FollowUp_Date = Entity.Followup_On;
        //                Exp_Posting_Dates[1] = " ";
        //                if (!string.IsNullOrEmpty(FollowUp_Date))
        //                {
        //                    FollowUp_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(FollowUp_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                    Exp_Posting_Dates[1] = FollowUp_Date;
        //                    //if (Convert.ToDateTime(FollowUp_Date) < Convert.ToDateTime(APP_SP_Start_Date))
        //                    //{
        //                    //    Exp_Posting_Dates[1] = APP_SP_Start_Date;
        //                    //    Exp_Posting_Dates[2] = "Prior to SPM Date";
        //                    //}
        //                }
        //                //else if (Exp_Posting_Dates[2] == "Prior to SPM Date")
        //                //    Exp_Posting_Dates[1] = APP_SP_Start_Date;


        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (CASEMSEntity Entity in SourceSP_CAMS_Details)
        //        {
        //            if (CAMS_ID == Entity.ID)
        //            {
        //                Complete_Date = Entity.Date;
        //                Exp_Posting_Dates[0] = APP_SP_Start_Date;
        //                Exp_Posting_Dates[1] = " ";
        //                Exp_Posting_Dates[2] = "Copy from Template";
        //                if (!string.IsNullOrEmpty(Complete_Date))
        //                {
        //                    Complete_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Complete_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                    Exp_Posting_Dates[0] = Complete_Date;
        //                    if (Convert.ToDateTime(Complete_Date) < Convert.ToDateTime(APP_SP_Start_Date))
        //                    {
        //                        Exp_Posting_Dates[0] = APP_SP_Start_Date;
        //                        Exp_Posting_Dates[2] = "Prior to SPM Date";
        //                    }
        //                }

        //                break;
        //            }
        //        }
        //    }

        //    return Exp_Posting_Dates;
        //}


        private void Post_App_Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (Post_App_Grid.Rows.Count > 0 && CASESPM_List.Count > 0)
            {
                //Get_App_CASEMS_List();
                OriginalTemplateList = _model.SPAdminData.Browse_Templates(string.Empty, TemplateCode, SP_Code, string.Empty, BranchCode, string.Empty);
                Get_App_CASEACT_List();
                Fill_SP_Controls();
            }
            else
                Fill_ADDSP_CAMS_Details(SP_Code, "P", null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Sel_Apps_To_Post = Get_Selected_ApplicantsFor_Posting("Selected");
            string Source_For_Posting = Get_Source_ActMS_For_Posting("Selected");

            if (_model.SPAdminData.Case0006_Act_Bulk_Posting_Latest(Search_MS_Details, Sel_Apps_To_Post, Source_For_Posting, (Rb_Intake_Date.Checked ? true : false), "N", string.Empty, out Sql_SP_Result_Message))
                MessageBox.Show("Posted Successfully", "CAP Systems");
        }


        private string Get_Selected_ApplicantsFor_Posting(string Process_Mode)
        {
            string App_xml = string.Empty;

            if (Post_App_Grid.Rows.Count > 0 && Listed_App_Cnt > 0)
            {
                StringBuilder App_Xml_To_Pass = new StringBuilder();
                App_Xml_To_Pass.Append("<Rows>");

                bool AtLeast_1_App_Selcted = false;
                if (Process_Mode == "All")
                {
                    foreach (DataGridViewRow dr in Post_App_Grid.Rows) // Use ths code for Multiple Applicants
                    {
                        if (dr.Cells["App_Sel"].Value.ToString() == true.ToString())
                        {
                            if (chkbShowPost.Checked == false)
                                App_Xml_To_Pass.Append("<Row App_No = \"" + dr.Cells["Post_App"].Value.ToString() + "\"/>");
                            else if (chkbShowPost.Checked == true)
                                App_Xml_To_Pass.Append("<Row App_No=\"" + dr.Cells["Post_App"].Value.ToString() + "\" spm_seq = \"" + dr.Cells["SPM_Seq"].Value.ToString() + "\"/>");

                            AtLeast_1_App_Selcted = true;
                        }
                    }
                }
                else
                {
                    if (chkbShowPost.Checked == false)
                        App_Xml_To_Pass.Append("<Row App_No = \"" + Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString() + "\"/>");
                    else if (chkbShowPost.Checked == true)
                        App_Xml_To_Pass.Append("<Row App_No=\"" + Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString() + "\" spm_seq = \"" + Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString() + "\"/>");
                    AtLeast_1_App_Selcted = true;
                }

                App_Xml_To_Pass.Append("</Rows>");

                if (AtLeast_1_App_Selcted)
                    App_xml = App_Xml_To_Pass.ToString();
            }

            return App_xml;
        }

        private void Get_Source_ACT_For_Posting()
        {
            CA_Pass_Entity.Agency = BaseForm.BaseAgency;
            CA_Pass_Entity.Dept = BaseForm.BaseDept;
            CA_Pass_Entity.Program = BaseForm.BaseProg;

            //CA_Pass_Entity.Year = Year;                        
            CA_Pass_Entity.Year = null;                             // Year will be always Four-Spaces in CASEACT
            CA_Pass_Entity.App_no = BaseForm.BaseApplicationNo;
            CA_Pass_Entity.Service_plan = SP_Code;
            CA_Pass_Entity.SPM_Seq = SP_Sequence;

            CA_Pass_Entity.Branch = CA_Pass_Entity.Group = CA_Pass_Entity.ACT_Code =
            CA_Pass_Entity.ACT_Date = CA_Pass_Entity.ACT_Seq = CA_Pass_Entity.Site = CA_Pass_Entity.Fund1 =
            CA_Pass_Entity.Fund2 = CA_Pass_Entity.Fund3 = CA_Pass_Entity.Caseworker = CA_Pass_Entity.Vendor_No =
            CA_Pass_Entity.Check_Date = CA_Pass_Entity.Check_No = CA_Pass_Entity.Cost = CA_Pass_Entity.Followup_On =
            CA_Pass_Entity.Followup_Comp = CA_Pass_Entity.Followup_By = CA_Pass_Entity.Refer_Data = CA_Pass_Entity.Cust_Code1 =
            CA_Pass_Entity.Cust_Value1 = CA_Pass_Entity.Cust_Code2 = CA_Pass_Entity.Cust_Value2 = CA_Pass_Entity.Cust_Code3 =
            CA_Pass_Entity.Cust_Value3 = CA_Pass_Entity.Lstc_Date = CA_Pass_Entity.Lsct_Operator = CA_Pass_Entity.Add_Date = null;
            CA_Pass_Entity.Add_Operator = CA_Pass_Entity.ACT_ID = CA_Pass_Entity.Bulk = CA_Pass_Entity.Act_PROG =
            CA_Pass_Entity.Cust_Code4 = CA_Pass_Entity.Cust_Value4 = CA_Pass_Entity.Cust_Code5 = CA_Pass_Entity.Cust_Value5 =
            CA_Pass_Entity.Units = CA_Pass_Entity.UOM = CA_Pass_Entity.Curr_Grp = null;

            Source_SP_Activity_Details.Clear();
            Source_SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(CA_Pass_Entity, "Browse");
        }


        private void Get_Source_MS_For_Posting()
        {
            Search_MS_Details.Agency = BaseForm.BaseAgency;
            Search_MS_Details.Dept = BaseForm.BaseDept;
            Search_MS_Details.Program = BaseForm.BaseProg;
            //Search_MS_Details.Year = Year; 
            Search_MS_Details.Year = null;                              // Year will be always Four-Spaces in CASEMS
            Search_MS_Details.App_no = BaseForm.BaseApplicationNo;
            Search_MS_Details.Service_plan = SP_Code;
            Search_MS_Details.SPM_Seq = SP_Sequence;

            Search_MS_Details.Branch = Search_MS_Details.Group = Search_MS_Details.MS_Code =
            Search_MS_Details.ID = Search_MS_Details.Date = Search_MS_Details.CaseWorker = Search_MS_Details.Site =
            Search_MS_Details.Result = Search_MS_Details.OBF = Search_MS_Details.Add_Operator =
            Search_MS_Details.Lstc_Date = Search_MS_Details.Lsct_Operator = Search_MS_Details.Add_Date = Search_MS_Details.Bulk =
            Search_MS_Details.Acty_PROG = Search_MS_Details.Curr_Grp = null;

            SourceSP_CAMS_Details.Clear();
            SourceSP_CAMS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");
        }


        List<CASEMSEntity> SourceSP_CAMS_Details = new List<CASEMSEntity>();
        List<CASEACTEntity> Source_SP_Activity_Details = new List<CASEACTEntity>();
        private string Get_Source_ActMS_For_Posting(string Process_Mode)
        {
            string App_xml = string.Empty;

            StringBuilder App_Xml_To_Pass = new StringBuilder();
            App_Xml_To_Pass.Append("<Rows>");

            bool AtLeast_1_App_Selcted = false, CA_Matched = false;

            if (TemplateList.Count > 0)
            {
                foreach (TemplateEntity Entity in TemplateList)
                {
                    foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                    {
                        if ((dr.Cells["SP2_Branch"].Value.ToString().Trim() == Entity.BTPL_BRANCH.Trim() &&
                                 dr.Cells["SP2_Group"].Value.ToString().Trim() == Entity.BTPL_GROUP.Trim() &&
                                 dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() == Entity.BTPL_CACODE.Trim()) && dr.Cells["Added_To_Xml"].Value.ToString() == "N" &&
                                 ((dr.Cells["Can_Post"].Value.ToString().Trim() == "Y" && chkbShowPost.Checked == false && string.IsNullOrEmpty(dr.Cells["SP2_ID"].Value.ToString().Trim())) || chkbShowPost.Checked == true))
                        {
                            dr.Cells["Added_To_Xml"].Value = "Y";

                            string ActSeq = Entity.BTPL_CASEQ.Trim() == "" ? "1" : Entity.BTPL_CASEQ.Trim();
                            string Units = string.Empty, Cost = string.Empty, Amount = string.Empty, Amount2 = string.Empty, Amount3 = string.Empty, Rate = string.Empty, Arrears = string.Empty;
                            Units = Entity.BTPL_UNITS.Trim() == "" ? "0.00" : Entity.BTPL_UNITS.Trim();
                            Rate = Entity.BTPL_RATE.Trim() == "" ? "0.00" : Entity.BTPL_RATE.Trim();
                            Amount = Entity.BTPL_AMOUNT.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT.Trim();
                            Amount2 = Entity.BTPL_AMOUNT3.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT3.Trim();
                            Amount3 = Entity.BTPL_AMOUNT3.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT3.Trim();
                            Cost = Entity.BTPL_TOTAL.Trim() == "" ? "0.00" : Entity.BTPL_TOTAL.Trim();
                            Arrears = Entity.BTPL_ARREARS.Trim() == "" ? "0.00" : Entity.BTPL_ARREARS.Trim();

                            if (chkbShowPost.Checked == false)
                            {
                                App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + Entity.BTPL_BRANCH + "\" ActMs_Group = \"" + Entity.BTPL_GROUP + "\" ActMs_Code = \"" + Entity.BTPL_CACODE + "\" ActMs_Seq = \"" + ActSeq + "\" ActMs_ID = \"" + "0" +
                                                             "\" Comp_Date = \"" + Entity.BTPL_DATE + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() +
                                                              "\" ActMs_Vendor = \"" + Entity.BTPL_VENDOR + "\" ActMs_Fund = \"" + Entity.BTPL_FUND + "\" ActMs_Fund2 = \"" + Entity.BTPL_FUND2 + "\" ActMs_Fund3 = \"" + Entity.BTPL_FUND3 +
                                                              "\" ActMs_UOM = \"" + Entity.BTPL_UOM + "\" ActMs_UOM2 = \"" + Entity.BTPL_UOM2 + "\" ActMs_UOM3 = \"" + Entity.BTPL_UOM3 + "\" ActMs_units= \"" + Units + "\" ActMs_Cost = \"" + Cost +
                                                              "\" ActMs_Amount = \"" + Amount + "\" ActMs_Amount2 = \"" + Amount2 + "\" ActMs_Amount3 = \"" + Amount3 + "\" ActMs_Rate = \"" + Rate + "\" ActMs_OBF= \"" + Entity.BTPL_OBF +
                                                              "\" ActMs_BillPeriod = \"" + Entity.BTPL_BILL_PERIOD + "\" ActMs_Arrears = \"" + Arrears + "\" ActMs_Vend_Account = \"" + Entity.BTPL_VEND_ACCT +
                                                              "\" ActMs_TempCode= \"" + Entity.BTPL_CODE + "\" ActMs_Site = \"" + Entity.BTPL_SITE + "\" ActMs_Worker = \"" + Entity.BTPL_WORKER + "\" ActMs_prog = \"" + Entity.BTPL_PROG + "\"/>");
                            }
                            else
                            {
                                App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + Entity.BTPL_BRANCH + "\" ActMs_Group = \"" + Entity.BTPL_GROUP + "\" ActMs_Code = \"" + Entity.BTPL_CACODE + "\" ActMs_Seq = \"" + ActSeq + "\" ActMs_ID = \"" + "0" +
                                                             "\" Comp_Date = \"" + Entity.BTPL_DATE + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() + ///dr.Cells["SP2_Follow_Date"].Value.ToString().Trim()
                                                              "\" ActMs_Vendor = \"" + Entity.BTPL_VENDOR + "\" ActMs_Fund = \"" + Entity.BTPL_FUND + "\" ActMs_Fund2 = \"" + Entity.BTPL_FUND2 + "\" ActMs_Fund3 = \"" + Entity.BTPL_FUND3 +
                                                              "\" ActMs_UOM = \"" + Entity.BTPL_UOM + "\" ActMs_UOM2 = \"" + Entity.BTPL_UOM2 + "\" ActMs_UOM3 = \"" + Entity.BTPL_UOM3 + "\" ActMs_units= \"" + Units + "\" ActMs_Cost = \"" + Cost +
                                                              "\" ActMs_Amount = \"" + Amount + "\" ActMs_Amount2 = \"" + Amount2 + "\" ActMs_Amount3 = \"" + Amount3 + "\" ActMs_Rate = \"" + Rate + "\" ActMs_OBF= \"" + Entity.BTPL_OBF +
                                                              "\" ActMs_BillPeriod = \"" + Entity.BTPL_BILL_PERIOD + "\" ActMs_Arrears = \"" + Arrears + "\" ActMs_Vend_Account = \"" + Entity.BTPL_VEND_ACCT +
                                                              "\" ActMs_TempCode= \"" + Entity.BTPL_CODE + "\" ActMs_Site = \"" + Entity.BTPL_SITE + "\" ActMs_Worker = \"" + Entity.BTPL_WORKER + "\" ActMs_prog = \"" + Entity.BTPL_PROG + "\"/>");
                            }

                            CA_Matched = AtLeast_1_App_Selcted = true;
                            //break;
                        }
                    }
                }
            }



            //foreach (CASEACTEntity ActEnt in Source_SP_Activity_Details)
            //{
            //    CA_Matched = false;
            //    foreach (ListItem List in Selected_CAMS_List)
            //    {
            //        if (List.Value.ToString() == ActEnt.ACT_ID && List.Text.ToString().Contains("CA"))// + ActEnt.ACT_Seq)
            //        {
            //            //App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID + "\"/>");
            //            //CA_Matched = AtLeast_1_App_Selcted = true;
            //            //break;

            //            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            //            {
            //                if (string.IsNullOrEmpty(dr.Cells["SP2_ID"].Value.ToString().Trim()) &&
            //                    (dr.Cells["SP2_Branch"].Value.ToString().Trim() == ActEnt.Branch.Trim() &&
            //                     dr.Cells["SP2_Group"].Value.ToString().Trim() == ActEnt.Group.Trim() &&
            //                     dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() == ActEnt.ACT_Code.Trim()) && dr.Cells["Added_To_Xml"].Value.ToString() == "N" &&
            //                     dr.Cells["Can_Post"].Value.ToString().Trim() == "Y")
            //                {
            //                    dr.Cells["Added_To_Xml"].Value = "Y";
            //                    //App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                    //                         "\" Comp_Date = \"" + dr.Cells["SP2_Comp_Date"].Value.ToString().Trim() + "\" FollowUpDate = \"" + dr.Cells["SP2_Follow_Date"].Value.ToString().Trim().Replace("'", "") + "\"/>");

            //                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                                             "\" Comp_Date = \"" + dr.Cells["SP2_Follow_Date"].Value.ToString().Trim() + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() + "\"/>");

            //                    CA_Matched = AtLeast_1_App_Selcted = true;
            //                    break;

            //                }
            //            }
            //            if (CA_Matched)
            //                break;

            //        }
            //    }
            //}


            App_Xml_To_Pass.Append("</Rows>");

            if (AtLeast_1_App_Selcted)
                App_xml = App_Xml_To_Pass.ToString();

            return App_xml;
        }

        private string Get_Source_ActMS_For_Posting_All()
        {
            string App_xml = string.Empty;

            StringBuilder App_Xml_To_Pass = new StringBuilder();
            App_Xml_To_Pass.Append("<Rows>");

            bool AtLeast_1_App_Selcted = false, CA_Matched = false;

            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                if (TemplateList.Count > 0)
                {
                    foreach (TemplateEntity Entity in TemplateList)
                    {
                        foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                        {
                            if ((dr.Cells["SP2_Branch"].Value.ToString().Trim() == Entity.BTPL_BRANCH.Trim() &&
                                 dr.Cells["SP2_Group"].Value.ToString().Trim() == Entity.BTPL_GROUP.Trim() &&
                                 dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() == Entity.BTPL_CACODE.Trim()) && dr.Cells["Added_To_Xml"].Value.ToString() == "N" &&
                                 ((dr.Cells["Can_Post"].Value.ToString().Trim() == "Y" && chkbShowPost.Checked == false && string.IsNullOrEmpty(dr.Cells["SP2_ID"].Value.ToString().Trim())) || chkbShowPost.Checked == true))
                            {
                                dr.Cells["Added_To_Xml"].Value = "Y";

                                string ActSeq = Entity.BTPL_CASEQ.Trim() == "" ? "1" : Entity.BTPL_CASEQ.Trim();
                                string Units = string.Empty, Cost = string.Empty, Amount = string.Empty, Amount2 = string.Empty, Amount3 = string.Empty, Rate = string.Empty, Arrears = string.Empty;
                                Units = Entity.BTPL_UNITS.Trim() == "" ? "0.00" : Entity.BTPL_UNITS.Trim();
                                Rate = Entity.BTPL_RATE.Trim() == "" ? "0.00" : Entity.BTPL_RATE.Trim();
                                Amount = Entity.BTPL_AMOUNT.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT.Trim();
                                Amount2 = Entity.BTPL_AMOUNT3.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT3.Trim();
                                Amount3 = Entity.BTPL_AMOUNT3.Trim() == "" ? "0.00" : Entity.BTPL_AMOUNT3.Trim();
                                Cost = Entity.BTPL_TOTAL.Trim() == "" ? "0.00" : Entity.BTPL_TOTAL.Trim();
                                Arrears = Entity.BTPL_ARREARS.Trim() == "" ? "0.00" : Entity.BTPL_ARREARS.Trim();

                                if (chkbShowPost.Checked == false)
                                {
                                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + Entity.BTPL_BRANCH + "\" ActMs_Group = \"" + Entity.BTPL_GROUP + "\" ActMs_Code = \"" + Entity.BTPL_CACODE + "\" ActMs_Seq = \"" + ActSeq + "\" ActMs_ID = \"" + "0" +
                                                             "\" Comp_Date = \"" + Entity.BTPL_DATE + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() + ///dr.Cells["SP2_Follow_Date"].Value.ToString().Trim() 
                                                              "\" ActMs_Vendor = \"" + Entity.BTPL_VENDOR + "\" ActMs_Fund = \"" + Entity.BTPL_FUND + "\" ActMs_Fund2 = \"" + Entity.BTPL_FUND2 + "\" ActMs_Fund3 = \"" + Entity.BTPL_FUND3 +
                                                              "\" ActMs_UOM = \"" + Entity.BTPL_UOM + "\" ActMs_UOM2 = \"" + Entity.BTPL_UOM2 + "\" ActMs_UOM3 = \"" + Entity.BTPL_UOM3 + "\" ActMs_units= \"" + Units + "\" ActMs_Cost = \"" + Cost +
                                                              "\" ActMs_Amount = \"" + Amount + "\" ActMs_Amount2 = \"" + Amount2 + "\" ActMs_Amount3 = \"" + Amount3 + "\" ActMs_Rate = \"" + Rate + "\" ActMs_OBF= \"" + Entity.BTPL_OBF +
                                                              "\" ActMs_BillPeriod = \"" + Entity.BTPL_BILL_PERIOD + "\" ActMs_Arrears = \"" + Arrears + "\" ActMs_Vend_Account = \"" + Entity.BTPL_VEND_ACCT +
                                                              "\" ActMs_TempCode= \"" + Entity.BTPL_CODE + "\" ActMs_Site = \"" + Entity.BTPL_SITE + "\" ActMs_Worker = \"" + Entity.BTPL_WORKER + "\" ActMs_prog = \"" + Entity.BTPL_PROG + "\"/>");
                                }
                                else
                                {
                                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + Entity.BTPL_BRANCH + "\" ActMs_Group = \"" + Entity.BTPL_GROUP + "\" ActMs_Code = \"" + Entity.BTPL_CACODE + "\" ActMs_Seq = \"" + ActSeq + "\" ActMs_ID = \"" + dr.Cells["SP2_ID"].Value.ToString().Trim() +
                                                             "\" Comp_Date = \"" + Entity.BTPL_DATE.Trim() + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() +
                                                              "\" ActMs_Vendor = \"" + Entity.BTPL_VENDOR + "\" ActMs_Fund = \"" + Entity.BTPL_FUND + "\" ActMs_Fund2 = \"" + Entity.BTPL_FUND2 + "\" ActMs_Fund3 = \"" + Entity.BTPL_FUND3 +
                                                              "\" ActMs_UOM = \"" + Entity.BTPL_UOM + "\" ActMs_UOM2 = \"" + Entity.BTPL_UOM2 + "\" ActMs_UOM3 = \"" + Entity.BTPL_UOM3 + "\" ActMs_units= \"" + Units + "\" ActMs_Cost = \"" + Cost +
                                                              "\" ActMs_Amount = \"" + Amount + "\" ActMs_Amount2 = \"" + Amount2 + "\" ActMs_Amount3 = \"" + Amount3 + "\" ActMs_Rate = \"" + Rate + "\" ActMs_OBF= \"" + Entity.BTPL_OBF +
                                                              "\" ActMs_BillPeriod = \"" + Entity.BTPL_BILL_PERIOD + "\" ActMs_Arrears = \"" + Arrears + "\" ActMs_Vend_Account = \"" + Entity.BTPL_VEND_ACCT +
                                                              "\" ActMs_TempCode= \"" + Entity.BTPL_CODE + "\" ActMs_Site = \"" + Entity.BTPL_SITE + "\" ActMs_Worker = \"" + Entity.BTPL_WORKER + "\" ActMs_prog = \"" + Entity.BTPL_PROG + "\"/>");
                                }
                                CA_Matched = AtLeast_1_App_Selcted = true;
                                //break;
                            }
                        }
                    }
                }

                //var Services = SP_CAMS_Grid.Rows.Cast<DataGridViewRow>()
                //           .Where(x => !x.IsNewRow)                   // either..
                //                                                      //.Where(x => x.Cells["gvcService"].Value != null) //..or or both
                //           .Select(x => x.Cells["gvService"].Value.ToString().Trim())
                //           .Distinct()
                //           .ToList();


                //if (Services.Count > 0)
                //{
                //    foreach (var entity in Services)
                //    {
                //        foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                //        {
                //            string Service = dr.Cells["gvService"].Value == null ? string.Empty : dr.Cells["gvService"].Value.ToString().Trim();
                //            if (entity.Trim() == Service.Trim())
                //            {
                //                CASESP2Entity SelRec = CA_Details.Find(u => u.CamCd.Trim() == Service);

                //                if(SelRec!=null)
                //                {
                //                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + SelRec.Branch + "\" ActMs_Group = \"" + SelRec.Orig_Grp + "\" ActMs_Code = \"" + SelRec.CamCd.Trim()+ "\" ActMs_Seq = \"" + 1+ "\" ActMs_ID = \"" + 1 +
                //                                  "\" Comp_Date = \"" + dr.Cells["gvCADate"].Value.ToString().Trim() + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + "" + "\"/>");
                //                }



                //                CA_Matched = AtLeast_1_App_Selcted = true;
                //            }
                //        }
                //    }
                //}

            }



            App_Xml_To_Pass.Append("</Rows>");

            if (AtLeast_1_App_Selcted)
                App_xml = App_Xml_To_Pass.ToString();

            return App_xml;
        }

        private string Get_Source_ActMS_For_Posting_Identity()
        {
            string App_xml = string.Empty;

            StringBuilder App_Xml_To_Pass = new StringBuilder();
            App_Xml_To_Pass.Append("<Rows>");

            bool AtLeast_1_App_Selcted = false, CA_Matched = false;

            CA_Matched = false;
            if (TemplateList.Count > 0)
            {
                foreach (TemplateEntity ActEnt in TemplateList)
                {
                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.BTPL_BRANCH + "\" ActMs_Group = \"" + ActEnt.BTPL_GROUP + "\" ActMs_Code = \"" + ActEnt.BTPL_CACODE + "\" ActMs_Seq = \"" + ActEnt.BTPL_CASEQ + "\" ActMs_ID = \"" + string.Empty +
                                             "\" Comp_Date = \"" + LookupDataAccess.Getdate(ActEnt.BTPL_DATE.Trim()) + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_OBF_Type = \"" + ActEnt.BTPL_OBF.Trim() + "\" ActMs_TempCode = \"" + ActEnt.BTPL_CODE.Trim() + "\"/>");


                    CA_Matched = AtLeast_1_App_Selcted = true;
                }
            }

            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            {
                dr.Cells["Added_To_Xml"].Value = "N";
            }

            //foreach (ListItem List in Selected_CAMS_List)
            //{
            //    if (List.ID.ToString() == ActEnt.ACT_Date && (List.Text.ToString() == "CA" + ActEnt.ACT_Code.Trim() + ActEnt.Group + ActEnt.Branch))// + ActEnt.ACT_Seq)List.Value.ToString() == ActEnt.ACT_ID &&
            //    {
            //        App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                                 "\" Comp_Date = \"" + LookupDataAccess.Getdate(ActEnt.ACT_Date) + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_OBF_Type = \"" + string.Empty + "\"/>");


            //        CA_Matched = AtLeast_1_App_Selcted = true;
            //    }
            //}

            //foreach (CASEACTEntity ActEnt in Source_SP_Activity_Details)
            //{


            //    foreach (ListItem List in Selected_CAMS_List)
            //    {
            //        if (List.Text.ToString().Contains("CA"))// + ActEnt.ACT_Seq)List.Value.ToString() == ActEnt.ACT_ID &&
            //        {
            //            App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                                     "\" Comp_Date = \"" + LookupDataAccess.Getdate(ActEnt.ACT_Date) + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_OBF_Type = \"" + string.Empty + "\"/>");


            //            CA_Matched = AtLeast_1_App_Selcted = true;
            //        }
            //    }

            //}

            //foreach (CASEACTEntity ActEnt in Source_SP_Activity_Details)
            //{
            //    CA_Matched = false;
            //    foreach (ListItem List in Selected_CAMS_List)
            //    {
            //        if ( List.Text.ToString().Contains("CA"))// + ActEnt.ACT_Seq)List.Value.ToString() == ActEnt.ACT_ID &&
            //        {
            //           foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            //            {
            //                if (string.IsNullOrEmpty(dr.Cells["SP2_ID"].Value.ToString().Trim()) &&
            //                    (dr.Cells["SP2_Branch"].Value.ToString().Trim() == ActEnt.Branch.Trim() &&
            //                     dr.Cells["SP2_Group"].Value.ToString().Trim() == ActEnt.Group.Trim() &&
            //                     dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim() == ActEnt.ACT_Code.Trim()) && dr.Cells["Added_To_Xml"].Value.ToString() == "N" &&
            //                     dr.Cells["Can_Post"].Value.ToString().Trim() == "Y")
            //                {
            //                    dr.Cells["Added_To_Xml"].Value = "Y";
            //                    //App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                    //                         "\" Comp_Date = \"" + dr.Cells["SP2_Comp_Date"].Value.ToString().Trim() + "\" FollowUpDate = \"" + dr.Cells["SP2_Follow_Date"].Value.ToString().Trim().Replace("'", "") + "\"/>");

            //                    App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                                             "\" Comp_Date = \"" + dr.Cells["SP2_Follow_Date"].Value.ToString().Trim() + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + dr.Cells["SP2_Mem_List"].Value.ToString().Trim() + "\"/>");

            //                    CA_Matched = AtLeast_1_App_Selcted = true;
            //                    break;

            //                }
            //            }
            //            if (CA_Matched)
            //                break;

            //        }
            //    }
            //}


            //if (SP_CAMS_Grid.Rows.Count > 0)
            //{
            //    var Services = SP_CAMS_Grid.Rows.Cast<DataGridViewRow>()
            //               .Where(x => !x.IsNewRow)                   // either..
            //                                                          //.Where(x => x.Cells["gvcService"].Value != null) //..or or both
            //               .Select(x => x.Cells["gvService"].Value.ToString().Trim())
            //               .Distinct()
            //               .ToList();


            //    if (Services.Count > 0)
            //    {
            //        foreach (var entity in Services)
            //        {
            //            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            //            {
            //                string Service = dr.Cells["gvService"].Value == null ? string.Empty : dr.Cells["gvService"].Value.ToString().Trim();
            //                if (entity.Trim() == Service.Trim())
            //                {
            //                    CASESP2Entity SelRec = CA_Details.Find(u => u.CamCd.Trim() == Service);

            //                    if (SelRec != null)
            //                    {
            //                        App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + SelRec.Branch + "\" ActMs_Group = \"" + SelRec.Orig_Grp + "\" ActMs_Code = \"" + SelRec.CamCd.Trim() + "\" ActMs_Seq = \"" + 1 + "\" ActMs_ID = \"" + 1 +
            //                                      "\" Comp_Date = \"" + dr.Cells["gvCADate"].Value.ToString().Trim() + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_CLIDs = \"" + "" + "\"/>");
            //                    }



            //                    CA_Matched = AtLeast_1_App_Selcted = true;
            //                }
            //            }
            //        }
            //    }

            //}

            //foreach (CASEACTEntity ActEnt in Source_SP_Activity_Details)
            //{
            //    CA_Matched = false;
            //    foreach (ListItem List in Selected_CAMS_List)
            //    {
            //        if (List.Value.ToString() == ActEnt.ACT_ID && List.Text.ToString().Contains("CA"))// + ActEnt.ACT_Seq)
            //        {
            //            App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "CA" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.ACT_Code + "\" ActMs_Seq = \"" + ActEnt.ACT_Seq + "\" ActMs_ID = \"" + ActEnt.ACT_ID +
            //                                     "\" Comp_Date = \"" + LookupDataAccess.Getdate(ActEnt.ACT_Date) + "\" FollowUpDate = \"" + string.Empty + "\" ActMs_OBF_Type = \"" + string.Empty + "\"/>");


            //            CA_Matched = AtLeast_1_App_Selcted = true;
            //        }
            //    }
            //}

            //bool MS_Matched = false;
            //foreach (CASEMSEntity ActEnt in SourceSP_CAMS_Details)
            //{
            //    MS_Matched = false;
            //    foreach (ListItem List in Selected_CAMS_List)
            //    {
            //        if (List.Value.ToString() == ActEnt.ID && List.Text.ToString().Contains("MS"))// + ActEnt.ACT_Seq)
            //        {

            //            App_Xml_To_Pass.Append("<Row ActMs_Type = \"" + "MS" + "\" ActMs_Branch = \"" + ActEnt.Branch + "\" ActMs_Group = \"" + ActEnt.Group + "\" ActMs_Code = \"" + ActEnt.MS_Code + "\" ActMs_Seq = \"" + " " + "\" ActMs_ID = \"" + ActEnt.ID +
            //                                    "\" Comp_Date = \"" + LookupDataAccess.Getdate(ActEnt.Date) + "\" FollowUpDate = \"" + "" + "\" ActMs_OBF_Type = \"" + ActEnt.OBF + "\"/>");
            //            MS_Matched = AtLeast_1_App_Selcted = true;
            //        }
            //    }
            //}

            App_Xml_To_Pass.Append("</Rows>");

            if (AtLeast_1_App_Selcted)
                App_xml = App_Xml_To_Pass.ToString();

            return App_xml;
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            bool Can_Fill_Grid = true;
            OBF_Type3_MS_Cnt = 0; lblTotSel.Visible = false; lblAmount.Visible = false;
            if (From_Date.Checked && To_Date.Checked)
            {
                if (From_Date.Value > To_Date.Value)
                {
                    _errorProvider.SetError(To_Date, string.Format("'From Date' should not be prior to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Fill_Grid = false;
                }
            }
            else
            {
                if (To_Date.Checked && To_Date.Checked)
                    _errorProvider.SetError(To_Date, null);
            }

            if (SP_CAMS_Grid.Rows.Count == 0)
            {
                Can_Fill_Grid = false;
                MessageBox.Show("Please Select Services through 'Set Posting Template' button");
            }

            if (Can_Fill_Grid)
                Fill_Bulk_App_Grid_To_Post(string.Empty);
        }

        private bool Validate_All_OBF_Type3_MS()
        {
            bool Can_Save = true;

            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            {
                if (dr.Cells["SP2_OBF"].Value.ToString() == "Sel HH Mem" && string.IsNullOrEmpty(dr.Cells["SP2_Mem_List"].Value.ToString().Trim()) &&
                    string.IsNullOrEmpty(dr.Cells["SP2_ID"].Value.ToString().Trim()) && dr.Cells["Dup_Ms_Date"].Value.ToString().Trim() == "N" &&
                    dr.Cells["Can_Post"].Value.ToString().Trim() == "Y")
                {
                    Can_Save = false;
                    //MS_With_No_Members += "      " + dr.Cells["SP2_Desc"].Value.ToString() + " \n";
                    //added by Sudheer on 07/25/2018

                    MS_With_No_Members += "User must select HH Members for the " + dr.Cells["SP2_Type"].Value.ToString() + "(s) listed below: \n \n" + "      " + dr.Cells["SP2_Desc"].Value.ToString() + " \n";

                }
            }

            return Can_Save;
        }

        private bool Exists_Valid_Postings()
        {
            bool Can_Save = false;

            foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
            {
                if (dr.Cells["Can_Post"].Value.ToString().Trim() == "Y")
                {
                    Can_Save = true;
                    break;
                }
            }

            return Can_Save;
        }


        string MS_With_No_Members = "";
        private void Btn_Bulk_Post_Click(object sender, EventArgs e)
        {
            if (Btn_Bulk_Post.Text == "Cancel")
            {
                this.gvRate.ReadOnly = true;
                this.gvUnits.ReadOnly = true;
                this.gvAmount.ReadOnly = true;
                panel7.Enabled = true;
                this.pb_Save_CA.Visible = false;
                Btn_Bulk_Post.Text = "Post for Selected App#";
            }
            else
            {
                MS_With_No_Members = "";

                if (!Exists_Valid_Postings() && chkbShowPost.Checked == false)
                {
                    MessageBox.Show("No Valid Postings exists!!!", "CAP Systems");
                    return;
                }

                if (!Validate_All_OBF_Type3_MS())
                {
                    //MessageBox.Show("User must select HH Members for the MS(s) listed below: \n \n" + MS_With_No_Members, "CAP Systems");
                    //Added by Sudheer on 07/25/2018

                    MessageBox.Show(MS_With_No_Members, "CAP Systems");

                    return;
                }

                string Sel_Apps_To_Post = Get_Selected_ApplicantsFor_Posting("Selected");
                string Source_For_Posting = Get_Source_ActMS_For_Posting("Selected");

                if (!string.IsNullOrEmpty(Source_For_Posting.Trim()))
                {


                    string FDate = From_Date.Value.ToShortDateString();
                    string TDate = To_Date.Value.ToShortDateString();
                    string BulkSite = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();
                    string BulkSort = ((ListItem)Cmb_Bulk_SortBy.SelectedItem).Value.ToString();
                    string BulkCaseType = ((ListItem)Cmb_Bulk_CaseType.SelectedItem).Value.ToString();
                    string Qtype = rbEnrlStatus.Checked == true ? "E" : "C";
                    string EnrlStatus = string.Empty; string Question = string.Empty; string Response = string.Empty;
                    if (Qtype == "E")
                        EnrlStatus = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();
                    else if (Qtype == "C")
                    {
                        Question = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();
                        Response = ((ListItem)cmbResp.SelectedItem).Value.ToString();
                    }

                    CA_Pass_Entity.Lsct_Operator = BaseForm.UserID;
                    CA_Pass_Entity.App_no = BaseForm.BaseApplicationNo;
                    CA_Pass_Entity.Year = BaseForm.BaseYear;
                    string Tmp_Sel_Start_Date = Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim();
                    string Tmp_Sel_Seq = Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString().Trim();
                    //Search_MS_Details.SPM_Seq = (Rb_Intake_Date.Checked ? (string.IsNullOrEmpty(Tmp_Sel_Start_Date) ? SP_Sequence.ToString() : Tmp_Sel_Seq) : Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString());
                    CA_Pass_Entity.SPM_Seq = (string.IsNullOrEmpty(Tmp_Sel_Start_Date) ? SP_Sequence.ToString() : Tmp_Sel_Seq);
                    string Add_SPM = "N";
                    if (TemplateList.Count > 0) Add_SPM = TemplateList[0].BTPL_SPM.Trim();
                    string ShowPosted = "N";
                    if (chkbShowPost.Checked == true) ShowPosted = "Y"; else ShowPosted = "N";


                    if (_model.SPAdminData.Case1006_Act_Bulk_Posting_Latest(CA_Pass_Entity, Sel_Apps_To_Post, Source_For_Posting, ShowPosted, "N",
                        FDate, TDate, BulkSite, BulkSort, BulkCaseType, Qtype, EnrlStatus, Question, Response, Add_SPM, out Sql_SP_Result_Message))
                    {
                        MessageBox.Show("Posted Successfully", "CAP Systems");
                        if (Rb_Intake_Date.Checked)
                        {
                            if (string.IsNullOrEmpty(Tmp_Sel_Start_Date))
                            {
                                Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value = "1";

                                if (!string.IsNullOrEmpty(SP_Start_Date))
                                    Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(SP_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat); ;

                                string Sel_App = Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString();
                                foreach (SP_Bulk_Post_Entity Ent in CASESPM_List)
                                {
                                    if (Ent.MST_app_no == Sel_App)
                                    {
                                        Ent.startdate = SP_Start_Date;
                                        Ent.Seq = "1";
                                        break;
                                    }
                                }
                            }
                        }
                        //Post_App_Grid_SelectionChanged(Post_App_Grid, EventArgs.Empty);
                        Priv_Scr_Mode = "";

                        //Added by Sudheer on 08/26/2021
                        TemplateList = _model.SPAdminData.Browse_Templates(string.Empty, TemplateCode, SP_Code, string.Empty, BranchCode, string.Empty);
                        if (TemplateList.Count > 0)
                        {
                            //OriginalTemplateList = TemplateList;
                            List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                            if (TemplateBalances.Count > 0)
                                TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));
                        }

                        Fill_Bulk_App_Grid_To_Post(Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString());
                    }
                }
                else
                    MessageBox.Show("Posted Successfully", "CAP Systems");
            }

        }

        private void Rb_Intake_Date_Click(object sender, EventArgs e)
        {
            Clear_All_Stuff();
            //if (BaseForm.BusinessModuleID == "08") //BaseForm.BusinessModuleID == "02" || 
            //{
            //    //Rb_Trigger.Visible = true;
            //    Cb_Triggeer.Visible = true;
            //}

            if (Rb_Intake_Date.Checked)
            {
                //Cmb_SPM_Filter.Visible = Lbl_SPM_Filter.Visible = true;
                //Cmb_SPM_Filter.SelectedIndex = 0;
            }
            else
                Cmb_SPM_Filter.Visible = Lbl_SPM_Filter.Visible = false;

            From_Date.Enabled = To_Date.Enabled = true;
            From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));
        }

        private void Btn_PostAll_Click(object sender, EventArgs e)
        {
            string Sel_Apps_To_Post = Get_Selected_ApplicantsFor_Posting("All");
            if (string.IsNullOrEmpty(Sel_Apps_To_Post.Trim()))
            {
                MessageBox.Show("Please Select Atleast one Applicant to Post", "CAP Systems");
                return;
            }

            MessageBox.Show("Are you sure Do you want post All?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Post_All_Aplicants);
        }

        private void Post_All_Aplicants(DialogResult dialogResult)
        {

            if (dialogResult == DialogResult.Yes)
            {
                string Sel_Apps_To_Post = Get_Selected_ApplicantsFor_Posting("All");
                if (string.IsNullOrEmpty(Sel_Apps_To_Post.Trim()))
                {
                    MessageBox.Show("Please Select Atleast one Applicant to Post", "CAP Systems");
                    return;
                }

                string Source_For_Posting = Get_Source_ActMS_For_Posting_All();

                if (!string.IsNullOrEmpty(Source_For_Posting.Trim()))
                {
                    string FDate = From_Date.Value.ToShortDateString();
                    string TDate = To_Date.Value.ToShortDateString();
                    string BulkSite = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();
                    string BulkSort = ((ListItem)Cmb_Bulk_SortBy.SelectedItem).Value.ToString();
                    string BulkCaseType = ((ListItem)Cmb_Bulk_CaseType.SelectedItem).Value.ToString();
                    string Qtype = rbEnrlStatus.Checked == true ? "E" : "C";
                    string EnrlStatus = string.Empty; string Question = string.Empty; string Response = string.Empty;
                    if (Qtype == "E")
                        EnrlStatus = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();
                    else if (Qtype == "C")
                    {
                        Question = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();
                        Response = ((ListItem)cmbResp.SelectedItem).Value.ToString();
                    }

                    CA_Pass_Entity.Lsct_Operator = BaseForm.UserID;
                    CA_Pass_Entity.App_no = BaseForm.BaseApplicationNo;
                    CA_Pass_Entity.Year = BaseForm.BaseYear;
                    string Tmp_Sel_Start_Date = Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value.ToString().Trim();
                    string Tmp_Sel_Seq = Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString().Trim();
                    string Add_SPM = "N";
                    if (TemplateList.Count > 0) Add_SPM = TemplateList[0].BTPL_SPM.Trim();
                    //Search_MS_Details.SPM_Seq = (Rb_Intake_Date.Checked ? (string.IsNullOrEmpty(Tmp_Sel_Start_Date) ? SP_Sequence.ToString() : Tmp_Sel_Seq) : Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value.ToString());
                    CA_Pass_Entity.SPM_Seq = (string.IsNullOrEmpty(Tmp_Sel_Start_Date) ? SP_Sequence.ToString() : Tmp_Sel_Seq);

                    string ShowPosted = "N";
                    if (chkbShowPost.Checked == true) ShowPosted = "Y"; else ShowPosted = "N";


                    if (_model.SPAdminData.Case1006_Act_Bulk_Posting_Latest(CA_Pass_Entity, Sel_Apps_To_Post, Source_For_Posting, ShowPosted, "Y",
                        FDate, TDate, BulkSite, BulkSort, BulkCaseType, Qtype, EnrlStatus, Question, Response, Add_SPM, out Sql_SP_Result_Message))
                    {
                        MessageBox.Show("Posted Successfully", "CAP Systems");
                        if (Rb_Intake_Date.Checked)
                        {
                            if (string.IsNullOrEmpty(Tmp_Sel_Start_Date))
                            {
                                Post_App_Grid.CurrentRow.Cells["SPM_Seq"].Value = "1";

                                if (!string.IsNullOrEmpty(SP_Start_Date))
                                    Post_App_Grid.CurrentRow.Cells["App_SP_Start_Date"].Value = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(SP_Start_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat); ;

                                string Sel_App = Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString();
                                foreach (SP_Bulk_Post_Entity Ent in CASESPM_List)
                                {
                                    if (Ent.MST_app_no == Sel_App)
                                    {
                                        Ent.startdate = SP_Start_Date;
                                        Ent.Seq = "1";
                                        break;
                                    }
                                }
                            }
                        }
                        //Post_App_Grid_SelectionChanged(Post_App_Grid, EventArgs.Empty);
                        Priv_Scr_Mode = "";

                        //Added by Sudheer on 08/26/2021
                        TemplateList = _model.SPAdminData.Browse_Templates(string.Empty, TemplateCode, SP_Code, string.Empty, BranchCode, string.Empty);
                        if (TemplateList.Count > 0)
                        {
                            //OriginalTemplateList = TemplateList;
                            List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                            if (TemplateBalances.Count > 0)
                                TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));
                        }

                        Fill_Bulk_App_Grid_To_Post(Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString());
                    }
                }
                else
                    MessageBox.Show("Posted Successfully", "CAP Systems");
            }
        }



        List<ListItem> OBF_Type3_Template_List = new List<ListItem>();
        private void Pb_Edit_OBF_Mem_Click(object sender, EventArgs e)
        {
            List<ListItem> OBF_Type3_Sel_Members = new List<ListItem>();
            string Hie = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + Post_App_Grid.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
            string Sel_Mem_Str = SP_CAMS_Grid.CurrentRow.Cells["SP2_Mem_List"].Value.ToString();

            if (!string.IsNullOrEmpty(Sel_Mem_Str.Trim()))
            {
                string Tmp_Str = "";
                string[] Clids_Str = Regex.Split(Sel_Mem_Str.ToString(), ", ");
                foreach (string str in Clids_Str)
                {
                    Tmp_Str = str.Replace("'", "");
                    string[] Clids = Regex.Split(Tmp_Str.ToString(), " - ");
                    {
                        if (Clids != null)
                        {
                            if (Clids.Length == 2)
                            {
                                OBF_Type3_Sel_Members.Add(new ListItem("Mem", Clids[0], Clids[1], ""));
                            }
                        }
                    }
                }
            }


            CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(BaseForm, Hie, Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString(), OBF_Type3_MS_Cnt, OBF_Type3_Sel_Members);//, (OBF_Type3_Template_List.Count > 0 ? "Y" : "N"));   // 08022012
            Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_Sel_Members_List);
            Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
            Mem_Sel_Form.ShowDialog();
        }



        private void get_Sel_Members_List(object sender, FormClosedEventArgs e)
        {
            string SelRef_Name = null;

            CASE0006_CAMSForm form = sender as CASE0006_CAMSForm;
            if (form.DialogResult == DialogResult.OK)
            {
                List<ListItem> Sel_Members = new List<ListItem>();
                Sel_Members.Clear();
                Sel_Members = form.Get_Sel_Members_List();

                string Sel_Mem_Str = " "; bool Template_set = false;
                foreach (ListItem list in Sel_Members)
                {
                    if (list.Text.ToString() == "Sel_Sw" && list.Value.ToString() == "Y")
                        Template_set = true;

                    if (list.Text.ToString() == "Mem")
                        Sel_Mem_Str += "'" + list.Value.ToString() + " - " + list.ID.ToString() + "', ";
                }


                if (Sel_Mem_Str.Length > 8)
                    Sel_Mem_Str = Sel_Mem_Str.Substring(0, Sel_Mem_Str.Length - 2);

                SP_CAMS_Grid.CurrentRow.Cells["SP2_Mem_List"].Value = Sel_Mem_Str;

                if (Template_set)
                {
                    OBF_Type3_Template_List.Clear();
                    OBF_Type3_Template_List = Sel_Members;

                    //foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                    //{
                    //    dr.Cells["SP2_Mem_List"].Value = Sel_Mem_Str;
                    //}
                }
            }
        }


        private void SP_CAMS_Grid_SelectionChanged(object sender, EventArgs e)
        {
            Pb_Edit_Date.Visible = Pb_Edit_OBF_Mem.Visible = false;
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                if (SP_CAMS_Grid.CurrentRow.Cells["SP2_OBF"].Value.ToString() == "Sel HH Mem" &&
                    string.IsNullOrEmpty(SP_CAMS_Grid.CurrentRow.Cells["SP2_ID"].Value.ToString().Trim()))
                    Pb_Edit_OBF_Mem.Visible = true;

                if (strCAOBO == "Y")
                {
                    if (SP_CAMS_Grid.CurrentRow.Cells["SP2_OBF"].Value.ToString() == "Sel HH Mem" && SP_CAMS_Grid.CurrentRow.Cells["SP2_TYPE"].Value.ToString() == "CA")
                        Pb_Edit_OBF_Mem.Visible = true;
                }

                //if (SP_CAMS_Grid.CurrentRow.Cells["Post_Type"].Value.ToString() == "3")
                //    Pb_Edit_Date.Visible = true;
            }
        }

        private void Pb_Edit_Date_Click(object sender, EventArgs e)
        {

        }

        private void get_Posting_Date(object sender, FormClosedEventArgs e)
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

                    //SP_CAMS_Grid.CurrentRow.Cells["SP2_Comp_Date"].Value = New_Post_Date;
                    SP_CAMS_Grid.CurrentRow.Cells["SP2_Follow_Date"].Value = New_Post_Date;
                    SP_CAMS_Grid.Rows[SP_CAMS_Grid.CurrentCell.RowIndex].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Green;
                    SP_CAMS_Grid.CurrentRow.Cells["Can_Post"].Value = "Y";
                }
                else
                {
                    SP_CAMS_Grid.CurrentRow.Cells["SP2_Comp_Date"].Value =
                    SP_CAMS_Grid.CurrentRow.Cells["SP2_Follow_Date"].Value = " ";
                    SP_CAMS_Grid.CurrentRow.Cells["Can_Post"].Value = "N";
                }

                Get_Duplicate_MS_Posting_Dates();
                Get_Posting_Exist_For_Edited_Date();
            }
        }


        private void Get_Posting_Exist_For_Edited_Date()
        {
            int Tmp_Bottom_Grid_Cnt = 0;
            if (SP_CAMS_Details.Count > 0)
            {
                int rowIndex = 0, Sel_CAMS_Index = 0;
                bool CASEACT_Exists = false, CASEMS_Exists = false, MAp_CA_Posting_Date = false;
                string Comp_date = null, Followup = " ", Notes_Exists = "N", Notes_Key = null, CAMS_DESC = null, Tmp_MS_ID = string.Empty;
                string Add_Date = null, Add_Opr = null, Lstc_Date = null, Lstc_Opr = null, Posted_Year = null, OBF_Desc = "", Post_Type = "";

                string Cur_Type = SP_CAMS_Grid.CurrentRow.Cells["SP2_Type"].Value.ToString();
                string Cur_CAMS_Code = SP_CAMS_Grid.CurrentRow.Cells["SP2_CAMS_Code"].Value.ToString();
                string Cur_Branch = SP_CAMS_Grid.CurrentRow.Cells["SP2_Branch"].Value.ToString();
                string Cur_Group = SP_CAMS_Grid.CurrentRow.Cells["SP2_Group"].Value.ToString();
                string Edited_Post_Date = SP_CAMS_Grid.CurrentRow.Cells["SP2_Follow_Date"].Value.ToString();

                if (Cur_Type == "CA")
                {
                    foreach (CASEACTEntity ActEnt in SP_Activity_Details)
                    {
                        Comp_date = "";
                        if (!string.IsNullOrEmpty(ActEnt.ACT_Date))
                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(ActEnt.ACT_Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        if (Edited_Post_Date == Comp_date && (ActEnt.ACT_Code.Trim() == Cur_CAMS_Code && ActEnt.Group == Cur_Group && ActEnt.Branch == Cur_Branch))
                        {
                            SP_CAMS_Grid.CurrentRow.Cells["Can_Post"].Value = "N";
                            SP_CAMS_Grid.Rows[SP_CAMS_Grid.CurrentRow.Index].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (CASEMSEntity MSEnt in SP_MS_Details)
                    {
                        Comp_date = "";
                        if (!string.IsNullOrEmpty(MSEnt.Date.Trim()))
                            Comp_date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(MSEnt.Date.Trim()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                        if (Edited_Post_Date == Comp_date && (MSEnt.MS_Code.Trim() == Cur_CAMS_Code && MSEnt.Group == Cur_Group && MSEnt.Branch == Cur_Branch))
                        {
                            SP_CAMS_Grid.CurrentRow.Cells["Can_Post"].Value = "N";
                            SP_CAMS_Grid.Rows[SP_CAMS_Grid.CurrentRow.Index].Cells["SP2_Follow_Date"].Style.ForeColor = Color.Violet;
                            break;
                        }

                    }
                }



            }
        }

        private void Rb_SPM_Date_CheckedChanged(object sender, EventArgs e)
        {
            From_Date.Enabled = To_Date.Enabled = true;
            Clear_All_Stuff();
            Lbl_SPM_Filter.Visible = Cmb_SPM_Filter.Visible = false;

            if (Rb_SPM_Date.Checked)
                Cb_Triggeer.Checked = false;
        }

        private void Btn_Sel_All_Click(object sender, EventArgs e)
        {
            if (Listed_App_Cnt > 0 && Post_App_Grid.Rows.Count > 0)
            {
                SelectedCount = 0;
                foreach (DataGridViewRow dr in Post_App_Grid.Rows)
                {
                    if (dr.Cells["SPM_Row_Color"].Value.ToString().Trim() == "Green")
                    {
                        dr.Cells["App_Sel"].Value = true;
                        SelectedCount++;
                    }
                }
                if (SelectedCount > 0) { lblTotSel.Text = "Total Selected: " + SelectedCount.ToString(); lblTotSel.Visible = true; lblAmount.Text = "Total Selected Amount: " + (TempAmount * SelectedCount).ToString("0.00"); lblAmount.Visible = true; }
                else { lblTotSel.Visible = false; lblAmount.Visible = false; }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Listed_App_Cnt > 0 && Post_App_Grid.Rows.Count > 0)
            {
                SelectedCount = 0;
                foreach (DataGridViewRow dr in Post_App_Grid.Rows)
                {
                    if (dr.Cells["SPM_Row_Color"].Value.ToString().Trim() == "Green")
                        dr.Cells["App_Sel"].Value = false;
                }
                if (SelectedCount > 0) { lblTotSel.Text = "Total Selected: " + SelectedCount.ToString(); lblTotSel.Visible = true; lblAmount.Text = "Total Selected Amount: " + (TempAmount * SelectedCount).ToString("0.00"); lblAmount.Visible = true; }
                else { lblTotSel.Visible = false; lblAmount.Visible = false; }
            }
        }

        private void Post_App_Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void CASE0006_Bulk_Posting_Latest_Load(object sender, EventArgs e)
        {
            if (BaseForm.BusinessModuleID == "08" || BaseForm.BusinessModuleID == "02")
            {
                //Rb_Trigger.Visible = true;
                Cb_Triggeer.Visible = true;
            }
        }

        bool CT_Age_SW = false;
        string CT_LPB_Benfit = "", CT_LPB_Benfit_Level = "", CT_LPB_Source = "", CT_LPB_Chk_Date = "", CT_LPB_Chk_FDate = "", CT_LPB_Chk_TDate = "", APP_Ben_Type = "", APP_Ben_Level = "";

        private void Pb_SP2_Min_Click(object sender, EventArgs e)
        {
            SP_CAMS_Grid.Visible = false; Pb_SP2_Min.Visible = false; Pb_SP2_Max.Visible = true;
            //this.pnlTotal.Location = new System.Drawing.Point(2, 428);
            //this.panel7.Size = new System.Drawing.Size(746, 464);
            //this.pnlTotal.Location = new System.Drawing.Point(2, 455);
            //this.panel7.Size = new System.Drawing.Size(1013, 490);
            // Post_App_Grid.Size = new System.Drawing.Size(1007, 340);
            pnlCAMS.Visible = false;
        }

        private void Pb_SP2_Max_Click(object sender, EventArgs e)
        {

            SP_CAMS_Grid.Visible = true; Pb_SP2_Min.Visible = true; Pb_SP2_Max.Visible = false;
            pnlCAMS.Visible = true;
            //this.pnlTotal.Location = new System.Drawing.Point(2, 272);
            //this.panel7.Size = new System.Drawing.Size(746, 309);
            //his.pnlTotal.Location = new System.Drawing.Point(2, 299);
            // this.panel7.Size = new System.Drawing.Size(1013, 335);
            //Post_App_Grid.Size = new System.Drawing.Size(1007, 181);
        }

        string HS_Attn_SW = "", HS_Attn_From_Date = "", HS_Attn_To_Date = "";

        List<DataGridView> SelPostAppGrid = null;

        private void btnShowAll_Click(object sender, EventArgs e)
        {

            if (Bulk_list.Count > 0)
            {
                Bulk_list = Bulk_list.OrderBy(u => u.AppName).ToList();
                Post_App_Grid.Rows.Clear();
                int rowIndex = 0, Selected_Index = 0; Listed_App_Cnt = 0; SelectedCount = 0; bool Can_Post_Atlest_One_APP = false; List_Post_Cnt = 0; TotPostAmount = 0;
                foreach (Bulk_Post_Entity Entity in Bulk_list)
                {
                    //added by sudheer on 02/27/2021
                    bool IsFalse = false;
                    if (SelectedgvRows.Count > 0)
                    {
                        List<DataGridViewRow> selected = (from c in SelectedgvRows.Cast<DataGridViewRow>().ToList()
                                                          where (((DataGridViewTextBoxCell)c.Cells[1]).Value.ToString().Equals(Entity.MST_app_no))
                                                          select c).ToList();
                        //foreach(DataGridViewRow dr in SelectedgvRows)
                        //{
                        //    if(dr.Cells["Post_App"].Value.ToString()==Entity.MST_app_no)
                        //    {
                        //        IsFalse = true; break;
                        //    }
                        //}

                        if (selected.Count > 0) IsFalse = true;
                    }

                    if (IsFalse)
                        rowIndex = Post_App_Grid.Rows.Add(true, Entity.MST_app_no, Entity.year, Entity.AppName, Entity.Active, Entity.MST_Intake_Date, Entity.App_Address, Entity.site, Entity.Start_date, Entity.Seq, Entity.PostSw, Entity.Rowcolor);
                    else
                        rowIndex = Post_App_Grid.Rows.Add(false, Entity.MST_app_no, Entity.year, Entity.AppName, Entity.Active, Entity.MST_Intake_Date, Entity.App_Address, Entity.site, Entity.Start_date, Entity.Seq, Entity.PostSw, Entity.Rowcolor);

                    //rowIndex = Post_App_Grid.Rows.Add(Entity.Selected == "True" ? true : false, Entity.MST_app_no, Entity.year, Entity.AppName, Entity.Active, Entity.MST_Intake_Date, Entity.App_Address, Entity.site, Entity.Start_date, Entity.Seq, Entity.PostSw, Entity.Rowcolor);

                    if (!string.IsNullOrEmpty(Entity.PostSw.Trim()))
                    {
                        if (Entity.PostSw.Substring(2, 1) == "Y")
                            Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        else if (Entity.PostSw.Substring(1, 1) == "Y")
                            Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                        else if (Entity.PostSw.Substring(0, 1) == "Y")
                        {
                            Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            List_Post_Cnt++;
                            TotPostAmount = TotPostAmount + decimal.Parse(Entity.Post_Amounts.Trim());
                        }
                    }
                    else
                        Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;

                    //if (Entity.Selected == "True") SelectedCount++;
                    if (IsFalse) SelectedCount++;
                    Listed_App_Cnt++;
                    Can_Post_Atlest_One_APP = true;
                }
                if (Listed_App_Cnt > 0)
                {
                    Lbl_Tot_Apps.Text = "Total Applications: " + Listed_App_Cnt.ToString();
                    Lbl_Tot_Apps.Visible = true; //Btn_Bulk_Post.Visible = 
                    if (List_Post_Cnt > 0)
                    {
                        lblPost.Text = "Total Posted: " + List_Post_Cnt.ToString();
                        lblPost.Visible = true;
                        lblPostAmt.Text = "Total Posted Amount: " + TotPostAmount.ToString();
                        lblPostAmt.Visible = true;
                    }
                    if (SelectedCount > 0) { lblTotSel.Text = "Total Selected: " + SelectedCount.ToString(); lblTotSel.Visible = true; lblAmount.Text = "Total Selected Amount: " + (TempAmount * SelectedCount).ToString("0.00"); lblAmount.Visible = true; }
                    if (SelectedCount == 1) Btn_Bulk_Post.Visible = true; else Btn_Bulk_Post.Visible = false;
                    if (SelectedCount <= 0) { lblTotSel.Text = ""; btnShowSel.Visible = false; btnShowAll.Visible = false; lblAmount.Text = ""; }
                    Post_App_Grid.CurrentCell = Post_App_Grid.Rows[Selected_Index].Cells[1];
                    if (Can_Post_Atlest_One_APP)
                        Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = true;
                }
            }
        }

        private void rbEnrlStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEnrlStatus.Checked)
            {
                Cmb_Enrl_Stat.Size = new System.Drawing.Size(187, 25);
                lblEnrlStat.Text = "Status";
                lblResp.Visible = false;
                cmbResp.Visible = false;

                Fill_EnrollStat_Combo();
            }
        }

        private void rbQuestion_CheckedChanged(object sender, EventArgs e)
        {
            if (rbQuestion.Checked)
            {
               // Cmb_Enrl_Stat.Size = new System.Drawing.Size(425, 25);
                lblEnrlStat.Text = "Question";
                lblResp.Visible = true;
                cmbResp.Visible = true;

                Fill_CutomQuestions();
            }
        }

        private void Cmb_Enrl_Stat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbQuestion.Checked)
            {
                if (Cmb_Enrl_Stat.Items.Count > 0)
                {
                    Fill_CustResponses();
                }
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void btnShowSel_Click(object sender, EventArgs e)
        {
            if (Post_App_Grid.Rows.Count > 0)
            {

                foreach (DataGridViewRow dr in Post_App_Grid.Rows)
                {

                    if (dr.Cells["App_Sel"].Value.ToString() == "False")
                    {
                        dr.Visible = false;
                    }
                    else if (dr.Cells["App_Sel"].Value.ToString() == "True")
                    {
                        dr.Visible = true;
                    }
                }
            }
        }

        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
        private void btnTemplate_Click(object sender, EventArgs e)
        {

            //CASE0006_TemplateForm Bulk_Posting_Form = new CASE0006_TemplateForm(BaseForm, SP_Code, SP_Sequence, SP_Start_Date, Selected_CAMS_List,SP_Header_Rec,BranchCode, TemplateList);
            //Bulk_Posting_Form.FormClosed += new Form.FormClosedEventHandler(Get_TemplateData);

            //Bulk_Posting_Form.ShowDialog();

            CommonEntity Entity = new CommonEntity();
            Entity.Code = From_Date.Value.ToShortDateString();
            Entity.Desc = To_Date.Value.ToShortDateString();
            Entity.Hierarchy = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();
            Entity.Extension = ((ListItem)Cmb_Bulk_SortBy.SelectedItem).Value.ToString();
            Entity.Pyear = ((ListItem)Cmb_Bulk_CaseType.SelectedItem).Value.ToString();
            Entity.PAgency = rbEnrlStatus.Checked == true ? "E" : "C";
            if (Entity.PAgency.Trim() == "E") Entity.PDept = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString(); else Entity.PAgency = string.Empty;
            if (Entity.PAgency.Trim() == "C")
            {
                Entity.Pprog = ((ListItem)Cmb_Enrl_Stat.SelectedItem).Value.ToString();
                Entity.Active = ((ListItem)cmbResp.SelectedItem).Value.ToString();
            }
            else
            {
                Entity.Pprog = string.Empty;
                Entity.Active = string.Empty;
            }


            CASE1006_TemplateForm Bulk_Posting_Form = new CASE1006_TemplateForm(BaseForm, SP_Code, SP_Sequence, SP_Start_Date, Selected_CAMS_List, SP_Header_Rec, BranchCode, TemplateList, TemplateCode, Entity, Privileges);
            Bulk_Posting_Form.FormClosed += new FormClosedEventHandler(Get_TemplateData);
            Bulk_Posting_Form.StartPosition = FormStartPosition.CenterScreen;
            Bulk_Posting_Form.ShowDialog();
        }

        //List<CATemplateEntity> TemplateList = new List<CATemplateEntity>();
        List<TemplateEntity> TemplateList = new List<TemplateEntity>();
        List<TemplateEntity> OriginalTemplateList = new List<TemplateEntity>();
        List<ListItem> Selected_CAMS_List = new List<Captain.Common.Utilities.ListItem>();
        string TemplateCode = string.Empty; decimal TempAmount = 0;
        private void Get_TemplateData(object sender, FormClosedEventArgs e)
        {
            CASE1006_TemplateForm form = sender as CASE1006_TemplateForm;
            Selected_CAMS_List = new List<Captain.Common.Utilities.ListItem>();
            //TemplateList = new List<CATemplateEntity>();
            if (form.DialogResult == DialogResult.OK)
            {
                Selected_CAMS_List = form.Getdata();
                TemplateCode = form.GetTemplateCode();

                TemplateList = _model.SPAdminData.Browse_Templates(string.Empty, TemplateCode, SP_Code, string.Empty, BranchCode, string.Empty);

                if (TemplateList.Count > 0)
                {
                    //OriginalTemplateList = TemplateList;
                    List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                    if (TemplateBalances.Count > 0)
                        TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));
                }


                if (listItem.Count > 0)
                    Selected_CAMS_List = listItem;

                //TemplateList = form.GetTotaldata();

                //Get_Source_ACT_For_Posting();
                //Get_Source_MS_For_Posting();

                //Fill_SP_Controls();
                Fill_ADDSP_CAMS_Details(SP_Code, "P", null);
                Btn_Search_Click(sender, e);
            }
        }


        private void SP_CAMS_Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SP_CAMS_Grid.CellValueChanged -= new DataGridViewCellEventHandler(SP_CAMS_Grid_CellValueChanged);
                //if (e.ColumnIndex == gvCADate.Index)
                //{
                //    int introwindex = SP_CAMS_Grid.CurrentCell.RowIndex;
                //    string strIntervalValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvCADate"].Value);
                //    int intcolumnindex = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                //    string strCurrectValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value);
                //    strCurrectValue = strCurrectValue.Replace("_", "").Trim();
                //    strCurrectValue = strCurrectValue.Replace(" ", "").Trim();
                //    SP_CAMS_Grid.Rows[introwindex].Cells["gvCADate"].Selected = true;

                //    if ((!string.IsNullOrEmpty(strCurrectValue)) && strCurrectValue.Trim() != "/  /")
                //    {
                //        if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.DateFormatMMDDYYYY))
                //        {
                //            try
                //            {

                //                if (DateTime.Parse(strCurrectValue) < Convert.ToDateTime("01/01/1800"))
                //                {
                //                    CommonFunctions.MessageBoxDisplay("01/01/1800 below date not except");
                //                    SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;

                //                }
                //                else
                //                {
                //                    SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                //                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);

                //                }

                //            }
                //            catch (Exception)
                //            {
                //                SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                //                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);

                //            }


                //        }
                //        else
                //        {
                //            bool booldatevalid = true;
                //            if ((strCurrectValue.ToString().Substring(0, 2) == "02") && (strCurrectValue.ToString().Substring(3, 2) == "29" || strCurrectValue.ToString().Substring(3, 2) == "30" || strCurrectValue.ToString().Substring(3, 2) == "31"))
                //            {
                //                SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                //                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEntermmddyyDateFormat);
                //                //booldatevalid = false;
                //            }

                //        }
                //    }

                //}
                if (e.ColumnIndex == gvAmount.Index)
                {
                    int introwindex = SP_CAMS_Grid.CurrentCell.RowIndex;
                    string strIntervalValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvAmount"].Value);
                    int intcolumnindex = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                    string strAmtValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value);
                    SP_CAMS_Grid.Rows[introwindex].Cells["gvAmount"].Selected = true;
                    if (!string.IsNullOrEmpty(strAmtValue))
                    {
                        if (CommonFunctions.IsNumeric(strAmtValue.Trim()))
                        {
                            if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                            {
                            }
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                {
                                    SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                                }
                                else
                                {
                                    if (strAmtValue.Length > 6)
                                    {
                                        if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange6String))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                            // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                                        }
                                    }
                                    else
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                            {
                                SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                            }
                        }
                    }

                }
                else if (e.ColumnIndex == gvUnits.Index)
                {
                    int introwindex = SP_CAMS_Grid.CurrentCell.RowIndex;
                    string strIntervalValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvUnits"].Value);
                    int intcolumnindex = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                    string strAmtValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value);
                    string strRate = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvRate"].Value);
                    SP_CAMS_Grid.Rows[introwindex].Cells["gvUnits"].Selected = true;
                    if (!string.IsNullOrEmpty(strAmtValue))
                    {
                        if (CommonFunctions.IsNumeric(strAmtValue.Trim()))
                        {
                            if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                            {
                            }
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                {
                                    SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                                }
                                else
                                {
                                    if (strAmtValue.Length > 6)
                                    {
                                        if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange6String))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                            // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                                        }
                                    }
                                    else
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                            if (!string.IsNullOrEmpty(strRate.Trim()))
                                            {
                                                decimal Amount = 0;
                                                Amount = decimal.Parse(strRate.Trim()) * decimal.Parse(strAmtValue.Trim());
                                                SP_CAMS_Grid.Rows[introwindex].Cells["gvAmount"].Value = Amount.ToString("0.00");
                                                this.gvAmount.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                            {
                                SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                            }
                        }
                    }
                }
                else if (e.ColumnIndex == gvRate.Index)
                {
                    int introwindex = SP_CAMS_Grid.CurrentCell.RowIndex;
                    string strIntervalValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvRate"].Value);
                    int intcolumnindex = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                    string strAmtValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value);
                    SP_CAMS_Grid.Rows[introwindex].Cells["gvRate"].Selected = true;
                    string strUnits = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvUnits"].Value);
                    if (!string.IsNullOrEmpty(strAmtValue))
                    {
                        if (CommonFunctions.IsNumeric(strAmtValue.Trim()))
                        {
                            if (Convert.ToDecimal(strAmtValue) < 1 && Convert.ToDecimal(strAmtValue) > 0)
                            {
                            }
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                                {
                                    SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                    CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                                }
                                else
                                {
                                    if (strAmtValue.Length > 6)
                                    {
                                        if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalRange6String))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = "999999.99";
                                            // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals6Range);
                                        }
                                    }
                                    else
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                                        {
                                            SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = strAmtValue + ".00";
                                            if (!string.IsNullOrEmpty(strUnits.Trim()))
                                            {
                                                decimal Amount = 0;
                                                Amount = decimal.Parse(strUnits.Trim()) * decimal.Parse(strAmtValue.Trim());
                                                SP_CAMS_Grid.Rows[introwindex].Cells["gvAmount"].Value = Amount.ToString("0.00");
                                                this.gvAmount.ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.TwoDecimalString))
                            {
                                SP_CAMS_Grid.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
                                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterDecimals);
                            }
                        }
                    }
                }
                //else if (e.ColumnIndex == gvVendor.Index)
                //{
                //    int intcolindex = SP_CAMS_Grid.CurrentCell.ColumnIndex;
                //    int introwindex = SP_CAMS_Grid.CurrentCell.RowIndex;

                //    string strCurrectValue = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells[intcolindex].Value);
                //    string Points = Convert.ToString(SP_CAMS_Grid.Rows[introwindex].Cells["gvVendor"].Value);
                //    if (Points != "")
                //    {
                //        if (SP_CAMS_Grid.Columns[intcolindex].Name.Equals("gvVendor"))
                //        {
                //            if (!System.Text.RegularExpressions.Regex.IsMatch(strCurrectValue, Consts.StaticVars.PNNumericString) && strCurrectValue != string.Empty)
                //            {
                //                //MessageBox.Show(Consts.Messages.NumericOnly, "CAP Systems");

                //                SP_CAMS_Grid.Rows[introwindex].Cells["gvVendor"].Value = string.Empty;
                //                CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterNumbers);
                //            }
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {

            }


        }

        private void SP_CAMS_Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                SP_CAMS_Grid.CellValueChanged += new DataGridViewCellEventHandler(SP_CAMS_Grid_CellValueChanged);
                SP_CAMS_Grid_CellValueChanged(sender, e);
            }
        }

        private void pnlTotal_Click(object sender, EventArgs e)
        {

        }

        private void SP_CAMS_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0 && e.RowIndex != -1)
            {
                if (e.ColumnIndex == SP2_Add_Edit.Index)
                {
                    this.gvRate.ReadOnly = false;
                    this.gvUnits.ReadOnly = false;
                    this.gvAmount.ReadOnly = false;
                    panel7.Enabled = false;
                    ///Btn_Bulk_Post.Text = "Cancel";
                    Btn_Bulk_Post.Visible = false;
                    btnCancel.Visible = true;
                   // this.btnCancel.Location = new System.Drawing.Point(843, 516);
                }
                else if (e.ColumnIndex == SP2_Save.Index)
                {
                    this.gvRate.ReadOnly = true;
                    this.gvUnits.ReadOnly = true;
                    this.gvAmount.ReadOnly = true;
                    panel7.Enabled = true;
                    Btn_Bulk_Post.Text = "Post for Selected App#";
                    if (TemplateList.Count > 0)
                    {

                        TemplateEntity TempEntity = TemplateList.Find(u => u.BTPL_ID == SP_CAMS_Grid.CurrentRow.Cells["SP2_Temp_ID"].Value.ToString());
                        if (TempEntity != null)
                        {
                            TemplateList.Remove(TempEntity);

                            TempEntity.BTPL_UNITS = SP_CAMS_Grid.CurrentRow.Cells["gvUnits"].Value.ToString();
                            TempEntity.BTPL_RATE = SP_CAMS_Grid.CurrentRow.Cells["gvRate"].Value.ToString();
                            TempEntity.BTPL_TOTAL = SP_CAMS_Grid.CurrentRow.Cells["gvAmount"].Value.ToString();

                            TemplateList.Add(TempEntity);
                        }

                        if (TemplateList.Count > 0)
                        {
                            List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                            if (TemplateBalances.Count > 0)
                                TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));
                        }
                    }

                }
            }
        }

        private void Rb_Trigger_Click(object sender, EventArgs e)
        {
            if ((BaseForm.BusinessModuleID == "08" || BaseForm.BusinessModuleID == "02") && Cb_Triggeer.Checked)
            {
                Clear_All_Stuff();
                Rb_Intake_Date.Checked = true;

                //Cmb_SPM_Filter.Visible = Lbl_SPM_Filter.Visible = true;
                //if (((ListItem)Cmb_SPM_Filter.SelectedItem).Value == null)
                //    Cmb_SPM_Filter.SelectedIndex = 0;

                if (BaseForm.BusinessModuleID == "08")
                {
                    CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(CT_Age_SW, CT_LPB_Benfit, CT_LPB_Benfit_Level, CT_LPB_Source, CT_LPB_Chk_Date, CT_LPB_Chk_FDate, CT_LPB_Chk_TDate, APP_Ben_Type, APP_Ben_Level);
                    Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_CT_Triggers);
                    Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
                    Mem_Sel_Form.ShowDialog();
                }
                else if (BaseForm.BusinessModuleID == "02")
                {
                    From_Date.Checked = To_Date.Checked = false;
                    CASE0006_CAMSForm Mem_Sel_Form = new CASE0006_CAMSForm(true, HS_Attn_SW, HS_Attn_From_Date, HS_Attn_To_Date);
                    Mem_Sel_Form.FormClosed += new FormClosedEventHandler(get_HS_Triggers);
                    Mem_Sel_Form.StartPosition = FormStartPosition.CenterScreen;
                    Mem_Sel_Form.ShowDialog();
                }

            }
        }

        private void get_CT_Triggers(object sender, FormClosedEventArgs e)
        {
            CASE0006_CAMSForm form = sender as CASE0006_CAMSForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Triggers = new string[7];
                Triggers = form.Get_CT_Triggers();

                CT_Age_SW = false;
                CT_LPB_Benfit = CT_LPB_Benfit_Level = "";

                if (Triggers[0] == "Y")
                    CT_Age_SW = true;

                CT_LPB_Benfit = Triggers[1];
                CT_LPB_Benfit_Level = Triggers[2];
                CT_LPB_Source = Triggers[3];
                CT_LPB_Chk_Date = Triggers[4];
                CT_LPB_Chk_FDate = Triggers[5];
                CT_LPB_Chk_TDate = Triggers[6];
            }
        }

        private void pb_Edit_CA_Click(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                SelectedgvRows = (from c in Post_App_Grid.Rows.Cast<DataGridViewRow>().ToList()
                                  where (((DataGridViewCheckBoxCell)c.Cells["App_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                  select c).ToList();

                if (Post_App_Grid.Rows.Count > 0 && SelectedgvRows.Count > 0)
                {
                    TemplateEntity TempEntity = TemplateList.Find(u => u.BTPL_ID == SP_CAMS_Grid.CurrentRow.Cells["SP2_Temp_ID"].Value.ToString());
                    if (TempEntity != null)
                    {

                    }

                    CASE1006_TemplateChange TemplateChange = new CASE1006_TemplateChange(BaseForm, SP_Code, TemplateCode, lblTempName.Text.Trim(), SP_CAMS_Grid.CurrentRow.Cells["SP2_CAMS_Code"].Value.ToString(), SP_CAMS_Grid.CurrentRow.Cells["SP2_Desc"].Value.ToString().Trim(), SP_CAMS_Grid.CurrentRow.Cells["SP2_Comp_Date"].Value.ToString().Trim(), TempEntity, SP_Header_Rec, Privileges);
                    TemplateChange.FormClosed += new FormClosedEventHandler(Get_TemplateChangeData);
                    TemplateChange.StartPosition = FormStartPosition.CenterScreen;
                    TemplateChange.ShowDialog();

                    //this.gvRate.ReadOnly = false;
                    //this.gvUnits.ReadOnly = false;
                    //this.gvAmount.ReadOnly = false;
                    //panel7.Enabled = false;
                    ////Btn_Bulk_Post.Text = "Cancel";
                    //this.pb_Save_CA.Visible = true;
                    //Btn_Bulk_Post.Visible = false; btnCancel.Visible = true;
                    //pb_Edit_CA.Visible = false;
                    //this.btnCancel.Location = new System.Drawing.Point(843, 516);

                }
                else
                {
                    MessageBox.Show("Please Select Atleast one Applicant to Change and Post", "CAP Systems");
                    return;
                }
            }
        }

        private void Get_TemplateChangeData(object sender, FormClosedEventArgs e)
        {
            CASE1006_TemplateChange form = sender as CASE1006_TemplateChange;
            Selected_CAMS_List = new List<Captain.Common.Utilities.ListItem>();
            //TemplateList = new List<CATemplateEntity>();
            if (form.DialogResult == DialogResult.OK)
            {
                TemplateEntity ChangedEntity = new TemplateEntity();
                ChangedEntity = form.GetTemplatedata();

                TemplateEntity TempEntity = TemplateList.Find(u => u.BTPL_ID == SP_CAMS_Grid.CurrentRow.Cells["SP2_Temp_ID"].Value.ToString());
                if (TempEntity != null)
                {
                    TemplateList.Remove(TempEntity);

                    //TempEntity.BTPL_UNITS = dr.Cells["gvUnits"].Value.ToString();
                    //TempEntity.BTPL_RATE = dr.Cells["gvRate"].Value.ToString();
                    //TempEntity.BTPL_AMOUNT = dr.Cells["gvAmount"].Value.ToString();
                    if (ChangedEntity != null)
                    {
                        SP_CAMS_Grid.CurrentRow.Cells["gvUnits"].Value = ChangedEntity.BTPL_UNITS;
                        SP_CAMS_Grid.CurrentRow.Cells["gvRate"].Value = ChangedEntity.BTPL_RATE;
                        SP_CAMS_Grid.CurrentRow.Cells["gvAmount"].Value = ChangedEntity.BTPL_TOTAL;

                        TemplateList.Add(ChangedEntity);
                    }
                }

            }
        }

        private void pb_Save_CA_Click(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                this.gvRate.ReadOnly = true;
                this.gvUnits.ReadOnly = true;
                this.gvAmount.ReadOnly = true;
                panel7.Enabled = true;
                this.pb_Save_CA.Visible = false;
                Btn_Bulk_Post.Text = "Post for Selected App#";
                Btn_Bulk_Post.Visible = true; btnCancel.Visible = false;
                //if(TemplateList.Count>0) OriginalTemplateList = TemplateList;
                pb_Edit_CA.Visible = true;
                foreach (DataGridViewRow dr in SP_CAMS_Grid.Rows)
                {
                    if (TemplateList.Count > 0)
                    {
                        TemplateEntity TempEntity = TemplateList.Find(u => u.BTPL_ID == dr.Cells["SP2_Temp_ID"].Value.ToString());
                        if (TempEntity != null)
                        {
                            TemplateList.Remove(TempEntity);

                            TempEntity.BTPL_UNITS = dr.Cells["gvUnits"].Value.ToString();
                            TempEntity.BTPL_RATE = dr.Cells["gvRate"].Value.ToString();
                            TempEntity.BTPL_TOTAL = dr.Cells["gvAmount"].Value.ToString();

                            TemplateList.Add(TempEntity);
                        }


                    }
                }
                if (TemplateList.Count > 0)
                {
                    List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                    if (TemplateBalances.Count > 0)
                        TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));
                }

                SelectedgvRows = (from c in Post_App_Grid.Rows.Cast<DataGridViewRow>().ToList()
                                  where (((DataGridViewCheckBoxCell)c.Cells["App_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                  select c).ToList();

                if (Post_App_Grid.Rows.Count > 0 && SelectedgvRows.Count > 1)
                    Btn_PostAll_Click(sender, e);
                else if (Post_App_Grid.Rows.Count > 0 && SelectedgvRows.Count == 1)
                    Btn_Bulk_Post_Click(sender, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid.Rows.Count > 0)
            {
                this.gvRate.ReadOnly = true;
                this.gvUnits.ReadOnly = true;
                this.gvAmount.ReadOnly = true;
                panel7.Enabled = true;
                this.pb_Save_CA.Visible = false;
                pb_Edit_CA.Visible = true;
                btnCancel.Visible = false; Btn_Bulk_Post.Visible = true;
                //Btn_Bulk_Post.Text = "Post for Selected App#";
            }
        }

        private void chkbShowPost_CheckedChanged(object sender, EventArgs e)
        {
            Btn_Search_Click(sender, e);
        }

        private void Post_App_Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void get_HS_Triggers(object sender, FormClosedEventArgs e)
        {
            CASE0006_CAMSForm form = sender as CASE0006_CAMSForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Triggers = new string[3];
                Triggers = form.Get_HS_Triggers();

                HS_Attn_SW = HS_Attn_From_Date = HS_Attn_To_Date = "";
                HS_Attn_SW = Triggers[0];

                HS_Attn_From_Date = Triggers[1];
                HS_Attn_To_Date = Triggers[2];
            }
        }

        int SelectedCount = 0; List<DataGridViewRow> SelectedgvRows = new List<DataGridViewRow>();
        private void Post_App_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Post_App_Grid.Rows.Count > 0 && e.RowIndex != -1)
            {
                int ColIdx = Post_App_Grid.CurrentCell.ColumnIndex;
                int RowIdx = Post_App_Grid.CurrentCell.RowIndex;

                switch (e.ColumnIndex)
                {
                    case 0:
                        if (Post_App_Grid.CurrentRow.Cells["SPM_Row_Color"].Value.ToString() != "Green" && chkbShowPost.Checked == false)
                            Post_App_Grid.CurrentRow.Cells["App_Sel"].Value = false;

                        SelectedgvRows = (from c in Post_App_Grid.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["App_Sel"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select c).ToList();

                        if (SelectedgvRows.Count > 0)
                        {
                            if (SelectedgvRows.Count > 1 && (TemplateList[0].BTPL_ALLOW_POST == "N" || TemplateList[0].BTPL_ALLOW_POST == ""))
                                Post_App_Grid.CurrentRow.Cells["App_Sel"].Value = false;
                            else
                            {
                                lblTotSel.Text = "Total Selected: " + SelectedgvRows.Count;
                                lblTotSel.Visible = true; //Btn_Bulk_Post.Visible = 
                                btnShowSel.Visible = true; btnShowAll.Visible = true;

                                decimal TotAmt = 0;
                                TotAmt = TempAmount * SelectedgvRows.Count;

                                lblAmount.Text = "Total Selected Amount: " + TotAmt.ToString("0.00");
                                lblAmount.Visible = true;

                                if (SelectedgvRows.Count == 1) Btn_Bulk_Post.Visible = true; else Btn_Bulk_Post.Visible = false;
                            }

                        }
                        else
                        {
                            if (SelectedCount <= 0)
                            { lblTotSel.Text = ""; lblAmount.Text = ""; btnShowSel.Visible = false; btnShowAll.Visible = false; }
                        }


                        //if (Post_App_Grid.CurrentRow.Cells["App_Sel"].Value.ToString() == "True")
                        //{
                        //    SelectedCount++;
                        //    if (SelectedCount > 0)
                        //    {
                        //        lblTotSel.Text = "Total Selected: " + SelectedCount.ToString();
                        //        lblTotSel.Visible = true; //Btn_Bulk_Post.Visible = 
                        //        btnShowSel.Visible = true; btnShowAll.Visible = true;

                        //        if (SelectedCount == 1) Btn_Bulk_Post.Visible = true; else Btn_Bulk_Post.Visible = false;
                        //    }
                        //    if (Bulk_list.Count > 0)
                        //    {
                        //        Bulk_Post_Entity Entity = Bulk_list.Find(u => u.MST_app_no.Equals(Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString()));
                        //        if (Entity != null)
                        //        {
                        //            Bulk_list.Remove(Entity);
                        //            Entity.Selected = "True";

                        //            Bulk_list.Add(Entity);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    SelectedCount--;
                        //    if (SelectedCount > 0) { lblTotSel.Text = "Total Selected: " + SelectedCount.ToString(); lblTotSel.Visible = true; }
                        //    if (SelectedCount == 1) Btn_Bulk_Post.Visible = true; else Btn_Bulk_Post.Visible = false;
                        //    if (SelectedCount <= 0)
                        //    { lblTotSel.Text = ""; btnShowSel.Visible = false; btnShowAll.Visible = false; }
                        //    if (Bulk_list.Count > 0)
                        //    {
                        //        Bulk_Post_Entity Entity = Bulk_list.Find(u => u.MST_app_no.Equals(Post_App_Grid.CurrentRow.Cells["Post_App"].Value.ToString()));
                        //        if (Entity != null)
                        //        {
                        //            Bulk_list.Remove(Entity);
                        //            Entity.Selected = "False";
                        //            Bulk_list.Add(Entity);
                        //        }
                        //    }
                        //}

                        break;
                }
            }
        }

        private void Cmb_Bulk_SortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CASESPM_List.Count > 0)
            {
                int rowIndex = 0, Selected_Index = 0; bool Can_Post_Atlest_One_APP = false;
                OBF_Type3_MS_Cnt = Listed_App_Cnt = 0; List_Post_Cnt = 0; TotPostAmount = 0;
                Lbl_Tot_Apps.Text = ""; lblTotSel.Text = ""; lblAmount.Text = ""; lblPost.Text = ""; lblPost.Visible = false; lblPostAmt.Visible = false; lblPostAmt.Text = "";
                Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = Lbl_Tot_Apps.Visible = false; lblTotSel.Visible = false; lblAmount.Visible = false;
                btnShowAll.Visible = btnShowSel.Visible = false;
                //CASESPM_List.Clear();
                this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
                //this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
                Post_App_Grid.Rows.Clear();
                SP_CAMS_Grid.Rows.Clear();
                //this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);
                this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);

                //CASESPMEntity Search_Entity = new CASESPMEntity(true);

                //Search_Entity.agency = BaseForm.BaseAgency;
                //Search_Entity.dept = BaseForm.BaseDept;
                //Search_Entity.program = BaseForm.BaseProg;

                ////Search_Entity.year = MainMenuYear;        
                //Search_Entity.year = BaseForm.BaseYear;                // Year will be always Four-Spaces in CASESPM
                //Search_Entity.service_plan = SP_Code;

                string Curr_Scr_Mode = "", Row_Color = "";
                if (Rb_SPM_Date.Checked)
                    Curr_Scr_Mode = "CASESPM";
                else if (Rb_Intake_Date.Checked)
                    Curr_Scr_Mode = "CASEMST";
                else Curr_Scr_Mode = "TRIGGER";

                string Sel_Site = null;
                if (((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString() != "0")
                    Sel_Site = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();

                string Source_For_Posting = Get_Source_ActMS_For_Posting_Identity();

                //if (Rb_SPM_Date.Checked)
                //    Priv_Scr_Mode = "CASESPM";
                //else if (Rb_Intake_Date.Checked)
                //    Priv_Scr_Mode = "CASEMST";
                //else Priv_Scr_Mode = "TRIGGER";

                string App_Name = string.Empty, App_Address = string.Empty, Start_Date = string.Empty, Intake_Date = string.Empty;

                Btn_Bulk_Post.Visible = false;
                bool From_Date_Flg = false, To_Date_Flg = false;
                string Ref_Date = string.Empty, TmpName = string.Empty, CaseType_To_Compare = ((ListItem)Cmb_Bulk_CaseType.SelectedItem).Value.ToString(),
                       Site_To_Compare = ((ListItem)Cmb_Bulk_Site.SelectedItem).Value.ToString();

                if (Rb_Intake_Date.Checked)
                {
                    this.dataGridViewTextBoxColumn1.Visible = true;
                    this.dataGridViewTextBoxColumn3.Width = 207; //220;
                }
                else
                {
                    this.dataGridViewTextBoxColumn3.Width = 253;// 265;
                    this.dataGridViewTextBoxColumn1.Visible = false;
                }

                //App_Name = LookupDataAccess.GetMemberName(Entity.SNP_First_Name, Entity.SNP_Middle_Name, Entity.SNP_Last_Name, BaseForm.BaseHierarchyCnFormat.ToString());


                List<SP_Bulk_Post_Entity> List_Ord_Name = new List<SP_Bulk_Post_Entity>();
                //List<SP_Bulk_Post_Entity> List_Ord_Name_Format = new List<SP_Bulk_Post_Entity>();
                if (((ListItem)Cmb_Bulk_SortBy.SelectedItem).Value.ToString() == "1")
                {
                    List_Ord_Name = CASESPM_List.OrderBy(u => u.SNP_First_Name).ThenBy(u => u.SNP_Middle_Name).ThenBy(u => u.SNP_Last_Name).ToList();
                    List_Ord_Name.ForEach(u => u.Disp_Name = LookupDataAccess.GetMemberName(u.SNP_First_Name, u.SNP_Middle_Name, u.SNP_Last_Name, "1"));
                }
                else
                {
                    List_Ord_Name = CASESPM_List.OrderBy(u => u.SNP_Last_Name).ThenBy(u => u.SNP_First_Name).ThenBy(u => u.SNP_Middle_Name).ToList();
                    List_Ord_Name.ForEach(u => u.Disp_Name = LookupDataAccess.GetMemberName(u.SNP_First_Name, u.SNP_Middle_Name, u.SNP_Last_Name, "2"));
                }

                CASESPM_List.Clear();
                CASESPM_List = List_Ord_Name;

                bool SPM_Filter = false; int SPM_Cnt = 0;
                string Cmb_SMP_Filter_Val = ((ListItem)Cmb_SPM_Filter.SelectedItem).Value.ToString();
                foreach (SP_Bulk_Post_Entity Entity in CASESPM_List)
                {
                    //if (((Rb_Intake_Date.Checked && ((Entity.SPM_Count == "0") || (Entity.SPM_Count != "0" && !string.IsNullOrEmpty(Entity.SPM_app_no.Trim())))) ||
                    //    (Entity.SPM_Count != "0" && Rb_SPM_Date.Checked)) ||
                    //    (BaseForm.BusinessModuleID == "02" && Entity.Attn_1Day_SW == "Y" && Rb_Trigger.Checked && Entity.SPM_Count == "0") || // Filter For HS )
                    //    (BaseForm.BusinessModuleID == "08" && Entity.CT_Trigger_SW == "Y" && Rb_Trigger.Checked && Entity.SPM_Count == "0")  // Filter For HS )
                    //    )
                    //if (((Rb_Intake_Date.Checked && ((Entity.SPM_Count == "0") || (Entity.SPM_Count != "0" && !string.IsNullOrEmpty(Entity.SPM_app_no.Trim())))) ||
                    //    (Entity.SPM_Count != "0" && Rb_SPM_Date.Checked)) ||
                    //    (BaseForm.BusinessModuleID == "02" && Entity.Attn_1Day_SW == "Y" && Cb_Triggeer.Checked && Entity.SPM_Count == "0") || // Filter For HS )
                    //    (BaseForm.BusinessModuleID == "08" && Entity.CT_Trigger_SW == "Y" && Cb_Triggeer.Checked && Entity.SPM_Count == "0")  // Filter For HS )
                    //    )

                    SPM_Filter = false; SPM_Cnt = int.Parse(Entity.SPM_Count.Trim());
                    if (Rb_Intake_Date.Checked)
                    {
                        if ((Cmb_SMP_Filter_Val == "1" && SPM_Cnt == 0) || (Cmb_SMP_Filter_Val == "2" && SPM_Cnt > 0) || Cmb_SMP_Filter_Val == "3")
                            SPM_Filter = true;
                    }

                    if ((Rb_SPM_Date.Checked && SPM_Cnt > 0) || (Rb_Intake_Date.Checked && SPM_Filter && ((Cb_Triggeer.Checked && Entity.CT_Trigger_SW == "Y") || !Cb_Triggeer.Checked)))
                    {
                        From_Date_Flg = To_Date_Flg = false;

                        if (!From_Date.Checked)
                            From_Date_Flg = true;

                        if (!To_Date.Checked)
                            To_Date_Flg = true;

                        if ((CaseType_To_Compare == "0" || CaseType_To_Compare == Entity.MST_Case_Type) &&
                            (Site_To_Compare == "0" || Site_To_Compare == Entity.Mst_Site))
                        {
                            if (From_Date.Checked)
                            {
                                //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked)//Cb_Triggeer
                                if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked)//Cb_Triggeer
                                {
                                    if (!string.IsNullOrEmpty((Entity.MST_Intake_Date.Trim())))
                                    {
                                        if (From_Date.Value <= Convert.ToDateTime(Entity.MST_Intake_Date))
                                            From_Date_Flg = true;
                                    }
                                }

                                if (Rb_SPM_Date.Checked)
                                {
                                    if (!string.IsNullOrEmpty((Entity.startdate.Trim())))
                                    {
                                        if (From_Date.Value <= Convert.ToDateTime(Entity.startdate))
                                            From_Date_Flg = true;
                                    }
                                }

                                //if ((From_Date.Value <= Convert.ToDateTime(Entity.startdate) && Rb_SPM_Date.Checked) ||
                                //    (From_Date.Value <= Convert.ToDateTime(Entity.MST_Intake_Date) && Rb_Intake_Date.Checked))
                                //    From_Date_Flg = true;
                            }
                            if (To_Date.Checked)
                            {
                                //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked) //Cb_Triggeer
                                if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked) //Cb_Triggeer
                                {
                                    if (!string.IsNullOrEmpty((Entity.MST_Intake_Date.Trim())))
                                    {
                                        if (To_Date.Value >= Convert.ToDateTime(Entity.MST_Intake_Date))
                                            To_Date_Flg = true;
                                    }
                                }

                                if (Rb_SPM_Date.Checked)
                                {
                                    if (!string.IsNullOrEmpty((Entity.startdate.Trim())))
                                    {
                                        if (To_Date.Value >= Convert.ToDateTime(Entity.startdate))
                                            To_Date_Flg = true;
                                    }
                                }

                                //if ((To_Date.Value >= Convert.ToDateTime(Entity.startdate) && Rb_SPM_Date.Checked) ||
                                //    (To_Date.Value >= Convert.ToDateTime(Entity.MST_Intake_Date) && Rb_Intake_Date.Checked))
                                //    To_Date_Flg = true;
                            }

                            if (From_Date_Flg && To_Date_Flg)
                            {
                                App_Name = App_Address = Start_Date = Intake_Date = " ";
                                //App_Name = LookupDataAccess.GetMemberName(Entity.SNP_First_Name, Entity.SNP_Middle_Name, Entity.SNP_Last_Name, BaseForm.BaseHierarchyCnFormat.ToString());
                                App_Name = Entity.Disp_Name;
                                App_Address = Entity.Mst_Hno + ' ' + Entity.MST_Street + ' ' + Entity.MST_City + ' ' + Entity.MST_State + ' ' + Entity.MST_Zip;
                                if (!string.IsNullOrEmpty(Entity.startdate.Trim()))
                                    Start_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.startdate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                if (!string.IsNullOrEmpty(Entity.MST_Intake_Date.Trim()))
                                    Intake_Date = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Entity.MST_Intake_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                                Row_Color = "";
                                if (!string.IsNullOrEmpty(Entity.Post_SW.Trim()))
                                {
                                    if (Entity.Post_SW.Substring(2, 1) == "Y")
                                        Row_Color = "Red";
                                    else if (Entity.Post_SW.Substring(1, 1) == "Y")
                                        Row_Color = "Green";
                                    else if (Entity.Post_SW.Substring(0, 1) == "Y")
                                        Row_Color = "Blak";
                                }
                                //if (Rb_Intake_Date.Checked || Rb_Trigger.Checked) //Cb_Triggeer
                                //if (Rb_Intake_Date.Checked || Cb_Triggeer.Checked) //Cb_Triggeer
                                //    Row_Color = "Green";

                                rowIndex = Post_App_Grid.Rows.Add(false, Entity.MST_app_no, Entity.MST_Year, App_Name, ((Entity.Mst_Active == "Y" && Entity.Snp_Active == "A") ? "A" : "I"), Intake_Date, App_Address, Entity.Mst_Site, Start_Date, Entity.Seq, Entity.Post_SW, Row_Color);

                                //if (Sel_App == Entity.MST_app_no)
                                //    Selected_Index = rowIndex;

                                if (!string.IsNullOrEmpty(Entity.Post_SW.Trim()))
                                {
                                    if (Entity.Post_SW.Substring(2, 1) == "Y")
                                        Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                    else if (Entity.Post_SW.Substring(1, 1) == "Y")
                                        Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                                    else if (Entity.Post_SW.Substring(0, 1) == "Y")
                                    {
                                        Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                        List_Post_Cnt++;
                                        TotPostAmount = TotPostAmount + decimal.Parse(Entity.Post_Amounts.Trim());
                                    }
                                }
                                else
                                    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;

                                //if (Entity.Post_SW == "N")
                                //    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                                //else
                                //{
                                //    Post_App_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Green; Can_Post_Atlest_One_APP = true;
                                //}
                                Can_Post_Atlest_One_APP = true;

                                Listed_App_Cnt++;
                            }
                        }
                    }
                }

                if (Listed_App_Cnt > 0)
                {
                    Lbl_Tot_Apps.Text = "Total Applications: " + Listed_App_Cnt.ToString();
                    Lbl_Tot_Apps.Visible = true; //Btn_Bulk_Post.Visible = 
                    if (List_Post_Cnt > 0)
                    {
                        lblPost.Text = "Total Posted: " + List_Post_Cnt.ToString();
                        lblPost.Visible = true;
                        lblPostAmt.Text = "Total Posted Amount: " + TotPostAmount.ToString();
                        lblPostAmt.Visible = true;
                    }
                    Post_App_Grid.CurrentCell = Post_App_Grid.Rows[Selected_Index].Cells[1];
                    if (Can_Post_Atlest_One_APP)
                        Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = true;
                }
            }
        }

        private void Clear_All_Stuff()
        {
            OBF_Type3_MS_Cnt = Listed_App_Cnt = 0;
            Lbl_Tot_Apps.Text = ""; lblPost.Text = ""; lblPost.Visible = false; lblPostAmt.Text = ""; lblPostAmt.Visible = false;
            Btn_Bulk_Post.Visible = Btn_Sel_All.Visible = Btn_UnSel_All.Visible = Btn_PostAll.Visible = Lbl_Tot_Apps.Visible = false;
            btnShowSel.Visible = btnShowAll.Visible = false;
            CASESPM_List.Clear();
            this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            //this.Post_App_Grid.SelectionChanged -= new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            Post_App_Grid.Rows.Clear();
            //SP_CAMS_Grid.Rows.Clear();
            //this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);
            this.Post_App_Grid.SelectionChanged += new System.EventHandler(this.Post_App_Grid_SelectionChanged);
        }


        private void Rescheduled()
        {
            List<TemplateEntity> TempList = _model.SPAdminData.Browse_Templates(string.Empty, string.Empty, SP_Code, string.Empty, BranchCode, string.Empty);
            TempList = TempList.FindAll(u => u.BTPL_ADD_OPERATOR.Trim() == BaseForm.UserID.Trim());
            if (TempList.Count > 0)
            {
                TempList = TempList.OrderByDescending(u => u.BTPL_DATE_LSTC).ToList();
                var DistTemp = TempList.Select(u => u.BTPL_CODE).Distinct().ToList();
                if (DistTemp.Count > 0)
                {
                    TemplateCode = DistTemp[0];

                    TemplateList = TempList.FindAll(u => u.BTPL_CODE == TemplateCode);//_model.SPAdminData.Browse_Templates(string.Empty, TemplateCode, SP_Code, SP_Sequence, BranchCode, string.Empty);

                    if (TemplateList.Count > 0)
                    {
                        //OriginalTemplateList = TemplateList;

                        List<TemplateEntity> TemplateBalances = TemplateList.FindAll(u => u.BTPL_TOTAL != "");
                        if (TemplateBalances.Count > 0)
                            TempAmount = TemplateBalances.Sum(u => Convert.ToDecimal(u.BTPL_TOTAL.Trim()));

                        Fill_ADDSP_CAMS_Details(SP_Code, "P", null);

                        From_Date.Text = TemplateList[0].BTPL_SER_FDATE;
                        To_Date.Text = TemplateList[0].BTPL_SER_TDATE;
                        CommonFunctions.SetComboBoxValue(Cmb_Bulk_Site, TemplateList[0].BTPL_SER_SITE);
                        CommonFunctions.SetComboBoxValue(Cmb_Bulk_SortBy, TemplateList[0].BTPL_SER_SORT);
                        CommonFunctions.SetComboBoxValue(Cmb_Bulk_CaseType, TemplateList[0].BTPL_SER_CASETYPE);

                        if (TemplateList[0].BTPL_SER_TYPE.Trim() == "E")
                        {
                            rbEnrlStatus.Checked = true;
                            CommonFunctions.SetComboBoxValue(Cmb_Enrl_Stat, TemplateList[0].BTPL_SER_ESTATUS);
                        }
                        else if (TemplateList[0].BTPL_SER_TYPE.Trim() == "C")
                        {
                            rbQuestion.Checked = true;
                            CommonFunctions.SetComboBoxValue(Cmb_Enrl_Stat, TemplateList[0].BTPL_SER_QUESTION);
                            CommonFunctions.SetComboBoxValue(cmbResp, TemplateList[0].BTPL_SER_RESPONSE);
                        }

                        Btn_Search_Click(Btn_Search, EventArgs.Empty);

                    }
                }
            }

        }

        private void Readonlytrue()
        {
            this.gvAmount.ReadOnly = true;
            this.gvRate.ReadOnly = true;
            this.gvUnits.ReadOnly = true;
        }

        private void Readonlyfalse()
        {
            this.gvAmount.ReadOnly = false;
            this.gvRate.ReadOnly = false;
            this.gvUnits.ReadOnly = false;
        }


    }
}


