#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
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
using DevExpress.XtraRichEdit.Forms;
using CarlosAg.ExcelXmlWriter;
using DevExpress.Utils.Win.Hook;
using iTextSharp.text.pdf;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using static NPOI.HSSF.Util.HSSFColor;
using DevExpress.Utils.Extensions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0026_PIRCounting_From : Form
    {

        #region private variables

        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;

        #endregion

        public HSSB0026_PIRCounting_From(BaseForm baseForm, PrivilegeEntity privileges)
        {

            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency;
            Depart = BaseForm.BaseDept;
            Program = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;
            _model = new CaptainModel();
            //** Add_Controls_To_Report();
            this.Text = /*privileges.Program + " - " + */privileges.PrivilegeName;
            Fill_Section_Combo();
            Fill_Sites_Combo();
        }

        private void HSSB0026_PIRCounting_From_Load(object sender, EventArgs e)
        {
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

            Controls_Loading_Completed = true;
            Fill_Questions_Grid();
            ReportPath = _model.lookupDataAccess.GetReportPath();
            Get_PIR_Report_Table_Structure();
            Get_PIR_Audit_Table_Structure();
        }


        #region properties

        public BaseForm BaseForm
        {
            get; set;
        }

        public PrivilegeEntity Privileges
        {
            get; set;
        }

        public string Calling_ID
        {
            get; set;
        }

        public string ReportPath
        {
            get; set;
        }

        public string Calling_UserID
        {
            get; set;
        }


        #endregion

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Controls_Loading_Completed)
                Fill_Questions_Grid();
        }


        List<DG_ResTab_Entity> PIR_Audit_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PIR_Audit_Table_Structure()
        {
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Hit_Num", "Hit", "R", "0.3in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Fam_ID", "Family ID", "L", "0.9in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Client_ID", "Client ID", "L", "0.7in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_App", "App#", "L", "0.7in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Namee", "Applicant Name", "L", "2.4in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Attn_Date", "1st Attn", "L", "0.8in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Class", "Site/Room", "L", "0.85in"));
            PIR_Audit_Table_List.Add(new DG_ResTab_Entity("Y", "App_Logic_Results", "Counting Logic results", "L", "2.5in"));
        }

        List<DG_ResTab_Entity> PIR_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PIR_Report_Table_Structure()
        {
            PIR_Table_List.Add(new DG_ResTab_Entity("N", "PIRQUEST_UNIQUE_ID", "Ques ID", "L", "3.8in"));
            PIR_Table_List.Add(new DG_ResTab_Entity("N", "PIRQUEST_SECTION", "Ques Sec", "L", "3.8in"));
            PIR_Table_List.Add(new DG_ResTab_Entity("N", "PIRQUEST_QUE_TYPE", "Ques Type", "L", "3.8in"));
            PIR_Table_List.Add(new DG_ResTab_Entity("Y", "PIRQUEST_QUE_DESC", "Question Description Type", "L", "6in"));
            PIR_Table_List.Add(new DG_ResTab_Entity("Y", "PIRCOUNT_SYS_COUNT", "System", "R", "0.7in"));
            PIR_Table_List.Add(new DG_ResTab_Entity("Y", "PIRCOUNT_USER_COUNT", "User", "R", "0.7in"));
        }


        bool Controls_Loading_Completed = false;
        private void Fill_Section_Combo()
        {
            Cmb_Section.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Section-A", "A"));
            //listItem.Add(new ListItem("Section-B", "B"));
            listItem.Add(new ListItem("Section-C", "C"));
            Cmb_Section.Items.AddRange(listItem.ToArray());
            Cmb_Section.SelectedIndex = 1;

            cmbFunds.Items.Insert(0, new ListItem("HS", "H"));
            cmbFunds.Items.Insert(1, new ListItem("HS 2", "2"));
            cmbFunds.Items.Insert(2, new ListItem("EHS", "E"));
            cmbFunds.Items.Insert(3, new ListItem("EHSCCP", "S"));
            cmbFunds.SelectedIndex = 0;
        }

        private void Fill_Sites_Combo()
        {
            Cmb_Site.Items.Clear();
            Cmb_Site.ColorMember = "FavoriteColor";

            List<ListItem> listItem = new List<ListItem>();

            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            List<CaseSiteEntity> HSS_Site_List = new List<CaseSiteEntity>();
            List<ListItem> HSS_Site_Headers_List = new List<ListItem>();
            HSS_Site_Headers_List.Add(new ListItem("All Sites", "****"));

            Search_Entity.SiteAGENCY = Agency;
            Search_Entity.SiteDEPT = Depart;
            Search_Entity.SitePROG = Program;
            Search_Entity.SiteYEAR = Program_Year;
            HSS_Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            if (HSS_Site_List.Count > 0 && BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
            {
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                HierarchyEntity hierarchyEntity = new HierarchyEntity();
                List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == Program)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        {
                            hierarchyEntity = null;
                        }
                    }
                }

                if (hierarchyEntity != null)
                {
                    List<CaseSiteEntity> SelSite_List = new List<CaseSiteEntity>();

                    if (hierarchyEntity.Sites.Length > 0)
                    {
                        string[] Sites = hierarchyEntity.Sites.Split(',');

                        for (int i = 0; i < Sites.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                            {
                                foreach (CaseSiteEntity casesite in HSS_Site_List) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (Sites[i].ToString() == casesite.SiteNUMBER)
                                    {
                                        SelSite_List.Add(casesite);
                                        //break;
                                    }
                                    // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                }
                            }
                        }
                        //strsiteRoomNames = hierarchyEntity.Sites;
                        HSS_Site_List = SelSite_List;
                    }

                }
            }
            HSS_Site_List = HSS_Site_List.OrderByDescending(u => u.SiteACTIVE).ToList();
            if (HSS_Site_List.Count > 0)
            {
                foreach (CaseSiteEntity Entity in HSS_Site_List)
                {
                    if (Entity.SiteROOM == "0000" && string.IsNullOrEmpty(Entity.SiteAM_PM.Trim()))
                        HSS_Site_Headers_List.Add(new ListItem(Entity.SiteNAME, Entity.SiteNUMBER, Entity.SiteACTIVE, Entity.SiteACTIVE.Equals("Y") ? Color.Black : Color.Red));
                }
            }
            Cmb_Site.Items.AddRange(HSS_Site_Headers_List.ToArray());
            Cmb_Site.SelectedIndex = 0;
        }

        string Program_Year, Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(800, 25);
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
                            TempCompareYear = int.Parse(Year);
                        List<ListItem> listItem = new List<ListItem>();
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
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(720, 25);
            else
            {
                AlertBox.Show("Year should not be Blank for this Hierarchy in Program Definition", MessageBoxIcon.Warning);
                this.Txt_HieDesc.Size = new System.Drawing.Size(800, 25);
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
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
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    //Rb_HSFund.Checked = true;
                    Agency = hierarchy.Substring(0, 2);
                    Depart = hierarchy.Substring(2, 2);
                    Program = hierarchy.Substring(4, 2);
                    this.Cmb_Site.SelectedIndexChanged -= new System.EventHandler(this.Reload_Questions_Grid);
                    Fill_Sites_Combo();
                    this.Cmb_Site.SelectedIndexChanged += new System.EventHandler(this.Reload_Questions_Grid);
                    if (!string.IsNullOrEmpty(Program_Year.Trim()))
                        Fill_Questions_Grid();

                    //Btn_Save_Counts.Enabled = false;
                }
            }
        }


        List<PIRQUESTEntity> Questions_List = new List<PIRQUESTEntity>();
        DataTable PIR_Report_Table = new DataTable();
        public void Fill_Questions_Grid()
        {
            Grid_Questions.Rows.Clear();
            PIR_Report_Table.Clear();
            Questions_List.Clear();
            Btn_Save_Counts.Enabled = AudiT_Question_Selected = false;
            PIRQUESTEntity Search_Entity = new PIRQUESTEntity(true);

            Search_Entity.Cnt_Agency = Current_Hierarchy.Substring(0, 2);
            Search_Entity.Cnt_Dept = Current_Hierarchy.Substring(2, 2);
            Search_Entity.Cnt_Prog = Current_Hierarchy.Substring(4, 2);
            Search_Entity.Cnt_Year = (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    ");
            //Search_Entity.Cnt_Year = BaseForm.BaseYear;

            string strFund = string.Empty;
            //if (Rb_HSFund.Checked == true)
            //    strFund = "H";
            //if (Rb_EHSFund.Checked == true)
            //    strFund = "E";
            //if (rdoEHSCCPFund.Checked == true)
            //    strFund = "S";
            strFund = ((ListItem)cmbFunds.SelectedItem).Value.ToString();

            Search_Entity.Cnt_Site = ((ListItem)Cmb_Site.SelectedItem).Value.ToString();

            //Search_Entity.Ques_Unique_ID = BaseForm.BaseAgency;
            //Search_Entity.Ques_Seq = BaseForm.BaseDept;
            Search_Entity.Ques_section = ((ListItem)Cmb_Section.SelectedItem).Value.ToString();
            Search_Entity.Fund_Type = strFund;
            Search_Entity.Active_Status = "A";

            //Questions_List = _model.PIRData.Browse_PIRQUEST(Search_Entity, "Y");

            List<SqlParameter> sqlParamList = Prepare_PIRQUEST_SqlParameters_List(Search_Entity, "Y");
            DataSet PIRQUESTData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIRQUEST]");

            if (PIRQUESTData != null && PIRQUESTData.Tables[0].Rows.Count > 0)
            {
                PIR_Report_Table = PIRQUESTData.Tables[0];

                foreach (DataRow row in PIRQUESTData.Tables[0].Rows)
                    Questions_List.Add(new PIRQUESTEntity(row, "Y"));
            }

            //if (Questions_List.Count > 0)
            //{
            //    if (strFund == "H")
            //        Questions_List = Questions_List.FindAll(u => u.Fund_Type.Equals("H") || u.Fund_Type.Equals("9") || u.Fund_Type.Equals("1") || u.Fund_Type.Equals("2"));
            //    else if (strFund == "E")
            //        Questions_List = Questions_List.FindAll(u => u.Fund_Type.Equals("E") || u.Fund_Type.Equals("9") || u.Fund_Type.Equals("1") || u.Fund_Type.Equals("3"));
            //    else if (strFund == "S")
            //        Questions_List = Questions_List.FindAll(u => u.Fund_Type.Equals("S") || u.Fund_Type.Equals("9") || u.Fund_Type.Equals("2") || u.Fund_Type.Equals("3"));
            //}


            int rowIndex = 0, Tmp_loop_Cnt = 0;
            string Attn_1Day = string.Empty, User_Cnt = " ", Sys_Cnt = " ",
                   Attn_1Day_Ques_List = "1002,1003,1007,1008,1005,1006,'3053,3054,3055,'3056,3057,3058,3059,3060,3061,3062,3063,3065,3066,3067,3068,3069,3070,3071,3072,3074,3075,1016,2003";



            foreach (PIRQUESTEntity Entity in Questions_List)
            {
                User_Cnt = " ";
                if (!string.IsNullOrEmpty(Entity.Cnt_User_Cnt.Trim()))
                {
                    User_Cnt = Entity.Cnt_User_Cnt.Trim();
                    if (User_Cnt == "0")
                        User_Cnt = " ";
                }


                Attn_1Day = " ";
                //if (Entity.Ques_Seq == "0")
                if (Entity.Ques_Bold == "Y")
                {
                    if (Attn_1Day_Ques_List.Contains(Entity.Ques_Unique_ID))
                        Attn_1Day = "1";

                    Sys_Cnt = Entity.Cnt_System_Cnt;
                    if (Sys_Cnt == "0")
                        Sys_Cnt = " ";

                    rowIndex = Grid_Questions.Rows.Add(false, Entity.Ques_Type, Entity.Ques_Desc, Attn_1Day, Sys_Cnt, User_Cnt, Entity.Ques_Unique_ID, Entity.Ques_Seq, Entity.Ques_SCode, Entity.Ques_Code);
                    Grid_Questions.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.AntiqueWhite;
                    Grid_Questions.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                }
                else
                    rowIndex = Grid_Questions.Rows.Add(false, Entity.Ques_Type, Entity.Ques_Desc, Attn_1Day, Entity.Cnt_System_Cnt, User_Cnt, Entity.Ques_Unique_ID, Entity.Ques_Seq, Entity.Ques_SCode, Entity.Ques_Code);

                if (Entity.Ques_Seq.Trim() != "0")
                    Grid_Questions.Rows[rowIndex].Cells["GD_User_Cnt"].ReadOnly = false;

                Tmp_loop_Cnt++;
            }

            if (Tmp_loop_Cnt > 0)
                Btn_Save_Counts.Enabled = true;

            if (Grid_Questions.Rows.Count > 0)
                Grid_Questions.Rows[0].Selected = true;
        }

        public List<SqlParameter> Prepare_PIRQUEST_SqlParameters_List(PIRQUESTEntity Entity, string Count_Join_SW)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@Join_Counts", Count_Join_SW));
                sqlParamList.Add(new SqlParameter("@Agy", Entity.Cnt_Agency));
                sqlParamList.Add(new SqlParameter("@Dept", Entity.Cnt_Dept));
                sqlParamList.Add(new SqlParameter("@Prog", Entity.Cnt_Prog));
                sqlParamList.Add(new SqlParameter("@Year", Entity.Cnt_Year));
                sqlParamList.Add(new SqlParameter("@Site", Entity.Cnt_Site));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_UNIQUE_ID", Entity.Ques_Unique_ID));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_POS", Entity.Ques_Position));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_ACTIVE", Entity.Active_Status));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SECTION", Entity.Ques_section));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_FUND", Entity.Fund_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_TYPE", Entity.Ques_Type));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_CODE", Entity.Ques_Code));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_SQUE_CODE", Entity.Ques_SCode));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_SECTION", Entity.Ques_Seq));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_DESC", Entity.Ques_Desc));
                sqlParamList.Add(new SqlParameter("@PIRQUEST_QUE_BOLD", Entity.Ques_Bold));

            }
            catch (Exception ex)
            {
                return sqlParamList;
            }

            return sqlParamList;
        }

        DataTable PIR_Audit_Table = new DataTable();
        DataTable PIR_Audit_Conditions_Table = new DataTable();
        string Audit_Ques_Desc = string.Empty; string Audit_HeadQues_desc = string.Empty;
        public void Get_Question_Audit_Table()
        {
            string Audit_Ques_ID = string.Empty, Audit_Ques_Seq = string.Empty, Audit_Ques_Typr = null, Audit_Ques_Scode = null, Audit_Ques_code = null;
            ;
            PIR_Audit_Table.Clear();
            PIR_Audit_Conditions_Table.Clear();
            Audit_Ques_Desc = string.Empty;
            Audit_HeadQues_desc = string.Empty;
            foreach (DataGridViewRow dr in Grid_Questions.Rows)
            {
                if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                {
                    Audit_Ques_Desc = dr.Cells["GD_Ques_Desc"].Value.ToString();
                    Audit_Ques_ID = dr.Cells["GD_Ques_ID"].Value.ToString();
                    Audit_Ques_Seq = dr.Cells["GD_Ques_Seq"].Value.ToString();
                    Audit_Ques_Typr = dr.Cells["GD_Ques_Type"].Value.ToString();
                    Audit_Ques_Scode = dr.Cells["GD_Ques_SCode"].Value.ToString();
                    Audit_Ques_code = dr.Cells["GD_Ques_Code"].Value.ToString();

                    foreach (DataGridViewRow drHead in Grid_Questions.Rows)
                    {
                        if (dr.Cells["GD_Ques_ID"].Value.ToString() == drHead.Cells["GD_Ques_ID"].Value.ToString() && drHead.Cells["GD_Ques_Seq"].Value.ToString() == "0" && drHead.Cells["GD_Ques_Code"].Value.ToString() == Audit_Ques_code)
                        {
                            Audit_HeadQues_desc = drHead.Cells["GD_Ques_Desc"].Value.ToString();
                            break;
                        }
                    }

                    break;
                }
            }

            if (Audit_Ques_ID == "3070")
            {
                Audit_Ques_code = "043";
                Audit_Ques_Scode = "000";
            }
            else if (Audit_Ques_ID == "1005")
            {
                Audit_Ques_code = "021";
                Audit_Ques_Scode = "000";
            }
            else if (Audit_Ques_ID == "1006")
            {
                Audit_Ques_code = "022";
                Audit_Ques_Scode = "000";
            }

            List<SqlParameter> sqlParamList = Prepare_AuditRep_SqlParameters_List(Audit_Ques_ID, Audit_Ques_Seq, Audit_Ques_Typr, Audit_Ques_Scode, Audit_Ques_code);
            DataSet PIRQUESTData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIR_Audit_Report_Testing]");

            if (PIRQUESTData != null && PIRQUESTData.Tables.Count > 0)
            {
                if (PIRQUESTData.Tables.Count >= 2)
                {
                    PIR_Audit_Conditions_Table = PIRQUESTData.Tables[1];
                    PIR_Audit_Table = PIRQUESTData.Tables[2];
                }
            }
        }

        #region Vikash added this function on 04/02/2024 to print multiple questions at one time as per PIR Counting Tool - Enhancements document

        DataSet Multi_Result_Table = new DataSet();
        DataSet Multi_Condition_Table = new DataSet();
        List<string> Audit_HeadQues_desc_List = new List<string>();
        List<string> Audit_Ques_desc_List = new List<string>();
        private DataSet Get_Multiple_Question_Audit_Table()
        {
            string Audit_Ques_ID = string.Empty, Audit_Ques_Seq = string.Empty, Audit_Ques_Typr = null, Audit_Ques_Scode = null, Audit_Ques_code = null;
            
            PIR_Audit_Table.Clear();
            PIR_Audit_Conditions_Table.Clear();
            Audit_Ques_Desc = string.Empty;
            Audit_HeadQues_desc = string.Empty;
            Audit_HeadQues_desc_List.Clear();
            Audit_Ques_desc_List.Clear();
            int Tablecount = 0;
            foreach (DataGridViewRow dr in Grid_Questions.Rows)
            {
                if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                {
                    Audit_Ques_Desc = dr.Cells["GD_Ques_Desc"].Value.ToString();
                    Audit_Ques_ID = dr.Cells["GD_Ques_ID"].Value.ToString();
                    Audit_Ques_Seq = dr.Cells["GD_Ques_Seq"].Value.ToString();
                    Audit_Ques_Typr = dr.Cells["GD_Ques_Type"].Value.ToString();
                    Audit_Ques_Scode = dr.Cells["GD_Ques_SCode"].Value.ToString();
                    Audit_Ques_code = dr.Cells["GD_Ques_Code"].Value.ToString();

                    Audit_Ques_desc_List.Add(Audit_Ques_Desc);

                    foreach (DataGridViewRow drHead in Grid_Questions.Rows)
                    {
                        if (dr.Cells["GD_Ques_ID"].Value.ToString() == drHead.Cells["GD_Ques_ID"].Value.ToString() && drHead.Cells["GD_Ques_Seq"].Value.ToString() == "0" && drHead.Cells["GD_Ques_Code"].Value.ToString() == Audit_Ques_code)
                        {
                            Audit_HeadQues_desc = drHead.Cells["GD_Ques_Desc"].Value.ToString();
                            Audit_HeadQues_desc_List.Add(Audit_HeadQues_desc);
                            //** break;
                        }
                    }

                    //**break;

                    if (Audit_Ques_ID == "3070")
                    {
                        Audit_Ques_code = "043";
                        Audit_Ques_Scode = "000";
                    }
                    else if (Audit_Ques_ID == "1005")
                    {
                        Audit_Ques_code = "021";
                        Audit_Ques_Scode = "000";
                    }
                    else if (Audit_Ques_ID == "1006")
                    {
                        Audit_Ques_code = "022";
                        Audit_Ques_Scode = "000";
                    }

                    List<SqlParameter> sqlParamList = Prepare_AuditRep_SqlParameters_List(Audit_Ques_ID, Audit_Ques_Seq, Audit_Ques_Typr, Audit_Ques_Scode, Audit_Ques_code);
                    DataSet PIRQUESTData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIR_Audit_Report_Testing]");

                    if (PIRQUESTData != null && PIRQUESTData.Tables.Count > 0)
                    {
                        if (PIRQUESTData.Tables.Count >= 2)
                        {
                            PIR_Audit_Conditions_Table = PIRQUESTData.Tables[1];
                            PIR_Audit_Conditions_Table.TableName = "Condition_Table" + Tablecount.ToString();
                            Multi_Condition_Table.Tables.Add(PIR_Audit_Conditions_Table.Copy());

                            PIR_Audit_Table = PIRQUESTData.Tables[2];
                            PIR_Audit_Table.TableName = "Table_" + Tablecount.ToString();
                            Multi_Result_Table.Tables.Add(PIR_Audit_Table.Copy());
                            Tablecount++;
                        }
                    }
                }
            }
            return Multi_Result_Table;
        }
        #endregion

        List<PIR_New_Count_Entity> New_PIR_Counts_List = new List<PIR_New_Count_Entity>();
        private void Btn_Generate_Count_Click(object sender, EventArgs e)
        {
            New_PIR_Counts_List.Clear();
            List<SqlParameter> sqlParamList = Prepare_AuditRep_SqlParameters_List(null, null, null, null, null);
            DataSet PIRQUESTData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_PIR_Audit_Report_Testing]");

            if (PIRQUESTData != null && PIRQUESTData.Tables.Count > 0)
            {
                if (PIRQUESTData.Tables.Count >= 2)
                {
                    foreach (DataRow Row in PIRQUESTData.Tables[3].Rows)
                        New_PIR_Counts_List.Add(new PIR_New_Count_Entity(Row));

                }
            }

            int rowIndex = 0, Tmp_loop_Cnt = 0;
            if (New_PIR_Counts_List.Count > 0)
            {
                Grid_Questions.Rows.Clear();
                string New_Sys_Cnt = string.Empty, New_User_Cnt = string.Empty, Attn_1Day = "", User_Cnt = " ",
                        Attn_1Day_Ques_List = "1002,1003,1007,1008,1005,1006,'3053,3054,3055,'3056,3057,3058,3059,3060,3061,3062,3063,3065,3066,3067,3068,3069,3070,3071,3072,3074,3075,1016";

                foreach (PIRQUESTEntity Entity in Questions_List)
                {
                    New_Sys_Cnt = New_User_Cnt = " ";

                    if (Entity.Ques_Unique_ID == "208")
                    {

                    }

                    if (Entity.Ques_Seq != "0")
                    {
                        foreach (PIR_New_Count_Entity Test in New_PIR_Counts_List)
                        {
                            if (Test.Ques_ID == Entity.Ques_Unique_ID && Test.Ques_Seq == Entity.Ques_Seq &&
                                Test.Ques_Type == Entity.Ques_Type && Test.Ques_Scode.Trim() == Entity.Ques_SCode && Test.Ques_code.Trim() == Entity.Ques_Code)
                            {
                                New_Sys_Cnt = Test.System_Cnt;
                                New_User_Cnt = Test.User_Cnt;
                                Entity.Cnt_System_Cnt = (string.IsNullOrEmpty(New_Sys_Cnt.Trim()) ? "0" : New_Sys_Cnt);
                                //Entity.Cnt_User_Cnt = (string.IsNullOrEmpty(New_User_Cnt.Trim()) ? "0" : New_User_Cnt);
                                break;
                            }
                        }
                    }

                    User_Cnt = " ";
                    if (!string.IsNullOrEmpty(Entity.Cnt_User_Cnt.Trim()))
                    {
                        User_Cnt = Entity.Cnt_User_Cnt.Trim();
                        if (User_Cnt == "0")
                            User_Cnt = " ";
                    }

                    if (Entity.Ques_Unique_ID == "3090")
                        Attn_1Day = " ";

                    Attn_1Day = " ";
                    //if (Entity.Ques_Seq == "0")
                    if (Entity.Ques_Bold == "Y")
                    {
                        if (Attn_1Day_Ques_List.Contains(Entity.Ques_Unique_ID))
                            Attn_1Day = "1";

                        rowIndex = Grid_Questions.Rows.Add(false, Entity.Ques_Type, Entity.Ques_Desc, Attn_1Day, New_Sys_Cnt, User_Cnt, Entity.Ques_Unique_ID, Entity.Ques_Seq, Entity.Ques_SCode, Entity.Ques_Code);
                        Grid_Questions.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.AntiqueWhite;
                        Grid_Questions.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                    }
                    else
                        rowIndex = Grid_Questions.Rows.Add(false, Entity.Ques_Type, Entity.Ques_Desc, Attn_1Day, New_Sys_Cnt, User_Cnt, Entity.Ques_Unique_ID, Entity.Ques_Seq, Entity.Ques_SCode, Entity.Ques_Code);

                    if (Entity.Ques_Seq.Trim() != "0")
                        Grid_Questions.Rows[rowIndex].Cells["GD_User_Cnt"].ReadOnly = false;

                    Tmp_loop_Cnt++;
                }

                Btn_Save_Counts.Enabled = false;
                if (Tmp_loop_Cnt > 0)
                    Btn_Save_Counts.Enabled = true;

                if (Grid_Questions.Rows.Count > 0)
                    Grid_Questions.Rows[0].Selected = true;
            }
        }

        public List<SqlParameter> Prepare_AuditRep_SqlParameters_List(string Ques_ID, string Ques_Seq, string Ques_Type, string Ques_SCode, string Ques_Code)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@BaseAgy", Current_Hierarchy.Substring(0, 2)));
                sqlParamList.Add(new SqlParameter("@BaseDept", Current_Hierarchy.Substring(2, 2)));
                sqlParamList.Add(new SqlParameter("@BaseProg", Current_Hierarchy.Substring(4, 2)));
                sqlParamList.Add(new SqlParameter("@BaseYear", (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    ")));
                sqlParamList.Add(new SqlParameter("@Sel_Section", ((ListItem)Cmb_Section.SelectedItem).Value.ToString()));
                string strFund = string.Empty;
                strFund = ((ListItem)cmbFunds.SelectedItem).Value.ToString();
                //if (Rb_HSFund.Checked == true)
                //    strFund = "H";
                //if (Rb_EHSFund.Checked == true)
                //    strFund = "E";
                //if (rdoEHSCCPFund.Checked == true)
                //    strFund = "S";
                sqlParamList.Add(new SqlParameter("@Sel_Fund", strFund));
                if ((((ListItem)Cmb_Site.SelectedItem).Value.ToString()) != "****")
                    sqlParamList.Add(new SqlParameter("@Sel_Site", ((ListItem)Cmb_Site.SelectedItem).Value.ToString()));
                sqlParamList.Add(new SqlParameter("@Sel_Ques_ID", Ques_ID));
                sqlParamList.Add(new SqlParameter("@Sel_Ques_Sec", Ques_Seq));
                sqlParamList.Add(new SqlParameter("@Sel_Ques_SCode", Ques_SCode));
                sqlParamList.Add(new SqlParameter("@Sel_Ques_Code", Ques_Code));

                //sqlParamList.Add(new SqlParameter("@Sel_Ques_Fund", Entity.Ques_Type));
                if (Generate_Report_Type != "Normal")
                    sqlParamList.Add(new SqlParameter("@Sel_Ques_Type", Ques_Type));
                sqlParamList.Add(new SqlParameter("@Userid", BaseForm.UserID));
            }
            catch (Exception ex)
            {
                return sqlParamList;
            }

            return sqlParamList;
        }


        private void Reload_Questions_Grid(object sender, EventArgs e)
        {
            if (Controls_Loading_Completed)
                Fill_Questions_Grid();
        }

        string Generate_Report_Type = "Normal";
        private void Btn_Generate_Report_Click(object sender, EventArgs e)
        {
            Multi_Result_Table = new DataSet();
            Multi_Condition_Table = new DataSet();
            if (Grid_Questions.Rows.Count > 0)
            {
                bool Can_Generete_Report = true;

                if (sender == Btn_Generate_Report)
                    Generate_Report_Type = "Normal";
                else
                {
                    Generate_Report_Type = "Audit";
                    //if (!AudiT_Question_Selected)
                    //    Can_Generete_Report = false;
                }

                if (Can_Generete_Report)
                {
                    // Added Excel by Vikash on 04/01/2024 as per PIR Counting Tool - Enhancements document 3rd point
                    if (Generate_Report_Type == "Audit")
                    {
                        Get_Question_Audit_Table();
                        AuditReport_Excel(PIR_Audit_Table, PIR_Audit_Table, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, string.Empty);
                    }
                    if (Generate_Report_Type == "Normal")
                    {
                        CASB2012_AdhocPageSetup PageSetup_Form = new CASB2012_AdhocPageSetup(11, 5, Privileges.Program);
                        PageSetup_Form.FormClosed += new FormClosedEventHandler(On_Pagesetup_Form_Closed);
                        PageSetup_Form.StartPosition = FormStartPosition.CenterScreen;
                        PageSetup_Form.ShowDialog();
                    }
                }
                else
                    AlertBox.Show("Please Select a Question to Generate Audit Report", MessageBoxIcon.Warning);
            }
        }

        PdfContentByte cb;
        string strFolderPath = string.Empty;
        string Random_Filename = null;

        private void AuditReport_Excel(DataTable Result_table, DataTable summary_table, string report_name, string report_to_process, string reportpath, string strUserId, string strScreenName)
        {
            string PdfName = "PIR Audit" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

            bool isFirstSheet = true;
            bool isMultiple = true;

            PdfName = ReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(ReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(ReportPath + BaseForm.UserID.Trim());
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

            WorksheetCell cell;

            Workbook book;

            Worksheet sheet;
            try
            {
                if (Grid_Questions.Rows.Count > 0)
                {
                    book = new Workbook();
                    cell = new WorksheetCell();
                    foreach (DataGridViewRow dr in Grid_Questions.Rows)
                    {
                        if (dr["GD_Sel"].Value.ToString() == "True")
                        {
                            Get_Multiple_Question_Audit_Table();
                            
                            if (Multi_Result_Table.Tables.Count > 0)
                            {
                                sheet = book.Worksheets.Add("Params");

                                PrintHeaderPage(sheet, cell);

                                this.GenerateStyles(book.Styles);

                                #region Styles

                                WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
                                mainstyle.Font.FontName = "Calibri";
                                mainstyle.Font.Size = 11;//12;
                                mainstyle.Font.Bold = true;
                                mainstyle.Font.Color = "#2c2c2c";
                                mainstyle.Interior.Color = "#d2e1ef";//"#0070c0";
                                mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
                                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                                mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;
                                mainstyle.Alignment.WrapText = true;

                                WorksheetStyle mainstyle2 = book.Styles.Add("MainHeaderStyles2");
                                mainstyle2.Font.FontName = "Calibri";
                                mainstyle2.Font.Size = 12;
                                mainstyle2.Font.Bold = true;
                                mainstyle2.Font.Color = "#FFFFFF";
                                mainstyle2.Interior.Color = "#0070c0";
                                mainstyle2.Interior.Pattern = StyleInteriorPattern.Solid;
                                mainstyle2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                                mainstyle2.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style1 = book.Styles.Add("Normal");
                                style1.Font.FontName = "Calibri";
                                style1.Font.Size = 10;
                                style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                                style1.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
                                stylecenter.Font.FontName = "Calibri";
                                stylecenter.Font.Bold = true;
                                stylecenter.Font.Size = 10;
                                stylecenter.Font.Bold = true;
                                stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                                stylecenter.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style3 = book.Styles.Add("NormalLeft");
                                style3.Font.FontName = "Calibri";
                                style3.Font.Size = 10;
                                style3.Interior.Color = "#f2f2f2";
                                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                                style3.Alignment.Vertical = StyleVerticalAlignment.Top;
                                style3.Alignment.WrapText = true;

                                WorksheetStyle style4 = book.Styles.Add("NormalLeftRed");
                                style4.Font.FontName = "Calibri";
                                style4.Font.Size = 10;
                                style4.Interior.Color = "#f2f2f2";
                                style4.Font.Color = "#f00808";
                                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                                style4.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style5 = book.Styles.Add("NormalRight");
                                style5.Font.FontName = "Calibri";
                                style5.Font.Size = 10;
                                style5.Interior.Color = "#f2f2f2";
                                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                                style5.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style55 = book.Styles.Add("NormalRightServTot");
                                style55.Font.FontName = "Calibri";
                                style55.Font.Size = 10;
                                style55.Font.Bold = true;
                                style55.Interior.Color = "#ffe8bd";
                                style55.Interior.Pattern = StyleInteriorPattern.Solid;
                                style55.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                                style55.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style555 = book.Styles.Add("NormalRightGrpTot");
                                style555.Font.FontName = "Calibri";
                                style555.Font.Size = 10;
                                style555.Font.Bold = true;
                                style555.Interior.Color = "#e0ebd8";
                                style555.Interior.Pattern = StyleInteriorPattern.Solid;
                                style555.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                                style555.Alignment.Vertical = StyleVerticalAlignment.Top;

                                WorksheetStyle style6 = book.Styles.Add("NormalRightRed");
                                style6.Font.FontName = "Calibri";
                                style6.Font.Size = 10;
                                style6.Interior.Color = "#f2f2f2";
                                style6.Font.Color = "#f00808";
                                style6.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                                style6.Alignment.Vertical = StyleVerticalAlignment.Top;

                                #endregion

                                GenerateMultipleAuditSheets(PdfName, book, Multi_Result_Table);
                                break;
                            }

                            //if (isFirstSheet)
                            //{
                            //    sheet = book.Worksheets.Add("Sheet" + " Audit_0");

                            //    GenerateSingleAuditSheets(PdfName, book, sheet, Result_table);
                            //    isFirstSheet = false;
                            //}
                            //else if(isMultiple)
                            //{
                            //    //Get_Multiple_Question_Audit_Table();
                            //    GenerateMultipleAuditSheets(PdfName, book, Multi_Result_Table);
                            //    isMultiple = false;
                            //}
                        }
                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();

                    FileInfo fiDownload = new FileInfo(PdfName);
                    string name = fiDownload.Name;
                    using (FileStream fileStream = fiDownload.OpenRead())
                    {
                        Application.Download(fileStream, name);
                        AlertBox.Show("PIR Audit Report Downloaded Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        List<string> QuesDesc = new List<string>();
        List<string> ConditionDesc = new List<string>();
        private void PrintHeaderPage(Worksheet sheet, WorksheetCell cell)
        {
            #region Header Page

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Index = 2;
            column0.Width = 5;
            sheet.Table.Columns.Add(162);
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 332;
            column2.StyleID = "s123";
            sheet.Table.Columns.Add(59);
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s108";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 14;
            Row1.AutoFitHeight = false;
            cell = Row1.Cells.Add();
            cell.StyleID = "s107";
            cell = Row1.Cells.Add();
            cell.StyleID = "s109";
            cell = Row1.Cells.Add();
            cell.StyleID = "s110";
            cell = Row1.Cells.Add();
            cell.StyleID = "s111";
            cell = Row1.Cells.Add();
            cell.StyleID = "s112";
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 14;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s107";
            cell = Row2.Cells.Add();
            cell.StyleID = "s113";
            cell = Row2.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row3 = sheet.Table.Rows.Add();
            Row3.Height = 18;
            Row3.AutoFitHeight = false;
            cell = Row3.Cells.Add();
            cell.StyleID = "s107";
            cell = Row3.Cells.Add();
            cell.StyleID = "s113";
            cell = Row3.Cells.Add();
            cell.StyleID = "s115";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row4 = sheet.Table.Rows.Add();
            Row4.Height = 14;
            Row4.AutoFitHeight = false;
            cell = Row4.Cells.Add();
            cell.StyleID = "s107";
            cell = Row4.Cells.Add();
            cell.StyleID = "s113";
            cell = Row4.Cells.Add();
            cell.StyleID = "s107";
            cell = Row4.Cells.Add();
            cell.StyleID = "s108";
            cell = Row4.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row6 = sheet.Table.Rows.Add();
            Row6.Height = 14;
            Row6.AutoFitHeight = false;
            cell = Row6.Cells.Add();
            cell.StyleID = "s107";
            cell = Row6.Cells.Add();
            cell.StyleID = "s113";
            cell = Row6.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Selected Report Parameters";
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row7 = sheet.Table.Rows.Add();
            Row7.Height = 14;
            Row7.AutoFitHeight = false;
            cell = Row7.Cells.Add();
            cell.StyleID = "s107";
            cell = Row7.Cells.Add();
            cell.StyleID = "s113";
            cell = Row7.Cells.Add();
            cell.StyleID = "s107";
            cell = Row7.Cells.Add();
            cell.StyleID = "s108";
            cell = Row7.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.Height = 14;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s107";
            cell = Row8.Cells.Add();
            cell.StyleID = "s113";
            cell = Row8.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            Row10.Height = 14;
            Row10.AutoFitHeight = false;
            cell = Row10.Cells.Add();
            cell.StyleID = "s107";
            cell = Row10.Cells.Add();
            cell.StyleID = "s113";
            cell = Row10.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            Agency: " + Agency + " , Department : " + Depart + " , Program : " + Program;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            Row11.Height = 14;
            Row11.AutoFitHeight = false;
            cell = Row11.Cells.Add();
            cell.StyleID = "s107";
            cell = Row11.Cells.Add();
            cell.StyleID = "s113";
            cell = Row11.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row12 = sheet.Table.Rows.Add();
            Row12.Height = 14;
            Row12.AutoFitHeight = false;
            cell = Row12.Cells.Add();
            cell.StyleID = "s107";
            cell = Row12.Cells.Add();
            cell.StyleID = "s113";
            cell = Row12.Cells.Add();
            cell.StyleID = "s107";
            cell = Row12.Cells.Add();
            cell.StyleID = "s108";
            cell = Row12.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            Row13.Height = 14;
            Row13.AutoFitHeight = false;
            cell = Row13.Cells.Add();
            cell.StyleID = "s107";
            cell = Row13.Cells.Add();
            cell.StyleID = "s113";
            cell = Row13.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row15 = sheet.Table.Rows.Add();
            Row15.Height = 14;
            Row15.AutoFitHeight = false;
            cell = Row15.Cells.Add();
            cell.StyleID = "s107";
            cell = Row15.Cells.Add();
            cell.StyleID = "s113";
            Row15.Cells.Add("            " + lblSection.Text.Trim(), DataType.String, "s117");
            Row15.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row15.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------

            WorksheetRow Row17 = sheet.Table.Rows.Add();
            Row17.Height = 14;
            Row17.AutoFitHeight = false;
            cell = Row17.Cells.Add();
            cell.StyleID = "s107";
            cell = Row17.Cells.Add();
            cell.StyleID = "s113";
            Row17.Cells.Add("            " + lblFund.Text.Trim(), DataType.String, "s117");
            Row17.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row17.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------

            WorksheetRow Row19 = sheet.Table.Rows.Add();
            Row19.Height = 14;
            Row19.AutoFitHeight = false;
            cell = Row19.Cells.Add();
            cell.StyleID = "s107";
            cell = Row19.Cells.Add();
            cell.StyleID = "s113";
            Row19.Cells.Add("            " + lblSite.Text.Trim(), DataType.String, "s117");
            Row19.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row19.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row21 = sheet.Table.Rows.Add();
            Row21.Height = 12;
            Row21.AutoFitHeight = false;
            cell = Row21.Cells.Add();
            cell.StyleID = "s107";
            cell = Row21.Cells.Add();
            cell.StyleID = "s113";
            cell = Row21.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row22 = sheet.Table.Rows.Add();
            Row22.Height = 12;
            Row22.AutoFitHeight = false;
            cell = Row22.Cells.Add();
            cell.StyleID = "s107";
            cell = Row22.Cells.Add();
            cell.StyleID = "s113";
            cell = Row22.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            string Task_Desc = string.Empty, Task_Condition = string.Empty, Task_Conjunction = string.Empty;
            string Tmp_Sel_TextQ = string.Empty;
            string Tmp_Sel_Text = string.Empty;
            string Text_to_Print = string.Empty;
            DataTable tempTable = new DataTable();

            int Rect_Total_Rows = 3;
            QuesDesc.Clear();
            ConditionDesc.Clear();

            /*for (int HeadquesCnt = 0; HeadquesCnt <= Audit_HeadQues_desc_List.Count; HeadquesCnt++)
            {
                WorksheetRow RowQ = sheet.Table.Rows.Add();
                RowQ.Height = 14;
                RowQ.AutoFitHeight = false;
                cell = RowQ.Cells.Add();
                cell.StyleID = "s109C";
                cell = RowQ.Cells.Add();
                cell.StyleID = "s113C";
                cell = RowQ.Cells.Add();
                cell.StyleID = "s114";
                cell.Data.Type = DataType.String;
                cell.Data.Text = Audit_HeadQues_desc_List[HeadquesCnt].Trim();
                cell.StyleID = "s112C";
                cell.MergeAcross = 2;

                // -----------------------------------------------
                //WorksheetRow Row233 = sheet.Table.Rows.Add();
                //Row233.Height = 12;
                //Row233.AutoFitHeight = false;
                //cell = Row233.Cells.Add();
                //cell.StyleID = "s107";
                //cell = Row233.Cells.Add();
                //cell.StyleID = "s113";
                //cell = Row233.Cells.Add();
                //cell.StyleID = "s107";
                //cell = Row233.Cells.Add();
                //cell.StyleID = "s108";
                //cell = Row233.Cells.Add();
                //cell.StyleID = "s116";

                for (int quesCnt = 0; quesCnt < Audit_Ques_desc_List.Count; quesCnt++)
                {
                    RowQ = sheet.Table.Rows.Add();
                    RowQ.Height = 14;
                    RowQ.AutoFitHeight = false;
                    cell = RowQ.Cells.Add();
                    cell.StyleID = "s109C";
                    cell = RowQ.Cells.Add();
                    cell.StyleID = "s113C";
                    cell = RowQ.Cells.Add();
                    cell.StyleID = "s114";
                    cell.Data.Type = DataType.String;
                    cell.Data.Text = Audit_Ques_desc_List[quesCnt].Trim();
                    cell.StyleID = "s112C";
                    cell.MergeAcross = 2;

                    WorksheetRow Row233 = sheet.Table.Rows.Add();
                    Row233.Height = 12;
                    Row233.AutoFitHeight = false;
                    cell = Row233.Cells.Add();
                    cell.StyleID = "s107";
                    cell = Row233.Cells.Add();
                    cell.StyleID = "s113";
                    cell = Row233.Cells.Add();
                    cell.StyleID = "s107";
                    cell = Row233.Cells.Add();
                    cell.StyleID = "s108";
                    cell = Row233.Cells.Add();
                    cell.StyleID = "s116";

                    for (int CondCnt = 0; CondCnt < Multi_Condition_Table.Tables[HeadquesCnt].Rows.Count; CondCnt++)
                    {
                        tempTable = Multi_Condition_Table.Tables[HeadquesCnt];

                        for (int tCondCnt = 0; tCondCnt < tempTable.Rows.Count; tCondCnt++)
                        {
                            RowQ = sheet.Table.Rows.Add();
                            RowQ.Height = 14;
                            RowQ.AutoFitHeight = false;
                            cell = RowQ.Cells.Add();
                            cell.StyleID = "s109C";
                            cell = RowQ.Cells.Add();
                            cell.StyleID = "s113C";
                            cell = RowQ.Cells.Add();
                            cell.StyleID = "s114";
                            cell.Data.Type = DataType.String;
                            cell.Data.Text = tempTable.Rows[tCondCnt]["Condition"].ToString();
                            cell.StyleID = "s112C";
                            cell.MergeAcross = 2;

                            Row233 = sheet.Table.Rows.Add();
                            Row233.Height = 12;
                            Row233.AutoFitHeight = false;
                            cell = Row233.Cells.Add();
                            cell.StyleID = "s107";
                            cell = Row233.Cells.Add();
                            cell.StyleID = "s113";
                            cell = Row233.Cells.Add();
                            cell.StyleID = "s107";
                            cell = Row233.Cells.Add();
                            cell.StyleID = "s108";
                            cell = Row233.Cells.Add();
                            cell.StyleID = "s116";
                        }
                    }
                }
            }*/

            string isPrevHead = string.Empty; string isPrevQues = string.Empty;

            /*for (int v = 0; v < Audit_HeadQues_desc_List.Count; v++)
            {
                if (isPrevHead != Audit_HeadQues_desc_List[v])
                {
                    for (int s = 0; s < Audit_Ques_desc_List.Count; s++)
                    {
                        if (isPrevQues != Audit_Ques_desc_List[s])
                        {
                            for (int i = 0; i < Rect_Total_Rows; i++)
                            {
                                switch (i)
                                {
                                    case 1:
                                        if (Generate_Report_Type != "Normal")
                                        {
                                            Tmp_Sel_TextQ = Audit_HeadQues_desc_List[v];
                                        }
                                        else
                                            Tmp_Sel_TextQ = "";
                                        break;
                                    case 2:
                                        if (Generate_Report_Type != "Normal")
                                        {
                                            Tmp_Sel_TextQ = "Question: " + Audit_Ques_desc_List[s];
                                        }
                                        else
                                            Tmp_Sel_TextQ = "";
                                        break;
                                    default:
                                        Tmp_Sel_TextQ = "  ";
                                        break;
                                }
                                //Text_to_Print = Text_to_Print + "    " + Tmp_Sel_TextQ;
                                //QuesDesc.Add(Text_to_Print);
                                QuesDesc.Add(Tmp_Sel_TextQ);
                            }
                        }
                        isPrevQues = Audit_Ques_desc_List[s];
                    }
                }

                isPrevHead = Audit_HeadQues_desc_List[v];
            }*/
            //QuesDesc.Add(Tmp_Sel_TextQ);

           /* for (int i = 0; i < Rect_Total_Rows; i++)
            {
                switch (i)
                {
                    case 1:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_TextQ = Audit_HeadQues_desc;
                        }
                        else
                            Tmp_Sel_TextQ = "";
                        break;
                    case 2:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_TextQ = "Question: " + Audit_Ques_Desc;
                        }
                        else
                            Tmp_Sel_TextQ = "";
                        break;
                    default:
                        Tmp_Sel_TextQ = "  ";
                        break;
                }
                Text_to_Print = Text_to_Print + "    " + Tmp_Sel_TextQ;

                QuesDesc.Add(Text_to_Print);
            }*/

            foreach (DataGridViewRow dr in Grid_Questions.Rows)
            {
                if (dr["GD_Sel"].Value.ToString() == true.ToString())
                {
                    QuesDesc.Add(dr["GD_Ques_Desc"].ToString().Trim());
                }
            }

            //WorksheetRow RowQ = sheet.Table.Rows.Add();
            //RowQ.Height = 14;
            //RowQ.AutoFitHeight = false;
            //cell = RowQ.Cells.Add();
            //cell.StyleID = "s109C";
            //cell = RowQ.Cells.Add();
            //cell.StyleID = "s113C";
            //cell = RowQ.Cells.Add();
            //cell.StyleID = "s114";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = Text_to_Print.Trim();
            //cell.StyleID = "s112C";
            //cell.MergeAcross = 2;

            //// -----------------------------------------------
            //WorksheetRow Row233 = sheet.Table.Rows.Add();
            //Row233.Height = 12;
            //Row233.AutoFitHeight = false;
            //cell = Row233.Cells.Add();
            //cell.StyleID = "s107";
            //cell = Row233.Cells.Add();
            //cell.StyleID = "s113";
            //cell = Row233.Cells.Add();
            //cell.StyleID = "s107";
            //cell = Row233.Cells.Add();
            //cell.StyleID = "s108";
            //cell = Row233.Cells.Add();
            //cell.StyleID = "s116";

            /*for (int t = 0; t < Multi_Condition_Table.Tables.Count; t++)
            {
                tempTable = Multi_Condition_Table.Tables[t];

                for (int i = 0; i < (tempTable.Rows.Count + 1); i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Task_Desc = Task_Condition = Task_Conjunction = Tmp_Sel_Text = string.Empty;
                        if (i < (tempTable.Rows.Count))
                        {
                            Task_Desc = tempTable.Rows[i]["Task"].ToString();
                            Task_Condition = tempTable.Rows[i]["Condition"].ToString();
                            if (i < (tempTable.Rows.Count - 1))
                                Task_Conjunction = tempTable.Rows[i]["Conj"].ToString();

                            switch (j)
                            {
                                case 0:
                                    Tmp_Sel_Text = "Task = " + Task_Condition;
                                    break;
                                case 1:
                                    Tmp_Sel_Text = "          " + Task_Conjunction;
                                    break;
                                default:
                                    Tmp_Sel_Text = "  ";
                                    break;
                            }
                            ConditionDesc.Add(Tmp_Sel_Text);
                        }

                        //ConditionDesc.Add(Tmp_Sel_Text);

                        //if (Tmp_Sel_Text.Contains("Task = "))
                        //{
                        //    WorksheetRow RowP = sheet.Table.Rows.Add();
                        //    RowP.Height = 16;
                        //    RowP.AutoFitHeight = false;
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s111P";
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s113";
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s114P";
                        //    cell.Data.Type = DataType.String;
                        //    cell.Data.Text = Tmp_Sel_Text;
                        //    cell.MergeAcross = 2;
                        //}
                        //else
                        //{
                        //    WorksheetRow RowP = sheet.Table.Rows.Add();
                        //    RowP.Height = 14;
                        //    RowP.AutoFitHeight = false;
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s107";
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s113";
                        //    cell = RowP.Cells.Add();
                        //    cell.StyleID = "s114";
                        //    cell.Data.Type = DataType.String;
                        //    cell.Data.Text = Tmp_Sel_Text;
                        //    cell.MergeAcross = 2;
                        //}
                    }
                }

            }*/


            for (int i = 0; i < Rect_Total_Rows; i++)
            {
                switch (i)
                {
                    case 1:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_TextQ = Audit_HeadQues_desc;
                        }
                        else
                            Tmp_Sel_TextQ = "";
                        break;
                    case 2:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_TextQ = "Question: " + Audit_Ques_Desc;
                        }
                        else
                            Tmp_Sel_TextQ = "";
                        break;
                    default:
                        Tmp_Sel_TextQ = "  ";
                        break;
                }
                Text_to_Print = Text_to_Print + "    " + Tmp_Sel_TextQ;

                //QuesDesc.Add(Text_to_Print);
            }

            WorksheetRow RowQ = sheet.Table.Rows.Add();
            RowQ.Height = 14;
            RowQ.AutoFitHeight = false;
            cell = RowQ.Cells.Add();
            cell.StyleID = "s109C";
            cell = RowQ.Cells.Add();
            cell.StyleID = "s113C";
            cell = RowQ.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Text_to_Print.Trim();
            cell.StyleID = "s112C";
            cell.MergeAcross = 2;

            // -----------------------------------------------
            WorksheetRow Row233 = sheet.Table.Rows.Add();
            Row233.Height = 12;
            Row233.AutoFitHeight = false;
            cell = Row233.Cells.Add();
            cell.StyleID = "s107";
            cell = Row233.Cells.Add();
            cell.StyleID = "s113";
            cell = Row233.Cells.Add();
            cell.StyleID = "s107";
            cell = Row233.Cells.Add();
            cell.StyleID = "s108";
            cell = Row233.Cells.Add();
            cell.StyleID = "s116";

            for (int i = 0; i < (PIR_Audit_Conditions_Table.Rows.Count + 1); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Task_Desc = Task_Condition = Task_Conjunction = Tmp_Sel_Text = string.Empty;
                    if (i < (PIR_Audit_Conditions_Table.Rows.Count))
                    {
                        Task_Desc = PIR_Audit_Conditions_Table.Rows[i]["Task"].ToString();
                        Task_Condition = PIR_Audit_Conditions_Table.Rows[i]["Condition"].ToString();
                        if (i < (PIR_Audit_Conditions_Table.Rows.Count - 1))
                            Task_Conjunction = PIR_Audit_Conditions_Table.Rows[i]["Conj"].ToString();

                        switch (j)
                        {
                            case 0:
                                Tmp_Sel_Text = "Task = " + Task_Condition;
                                break;
                            case 1:
                                Tmp_Sel_Text = "          " + Task_Conjunction;
                                break;
                            default:
                                Tmp_Sel_Text = "  ";
                                break;
                        }
                        //ConditionDesc.Add(Tmp_Sel_Text);
                    }

                    //ConditionDesc.Add(Tmp_Sel_Text);

                    if (Tmp_Sel_Text.Contains("Task = "))
                    {
                        WorksheetRow RowP = sheet.Table.Rows.Add();
                        RowP.Height = 16;
                        RowP.AutoFitHeight = false;
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s111P";
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s113";
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s114P";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = Tmp_Sel_Text;
                        cell.MergeAcross = 2;
                    }
                    else
                    {
                        WorksheetRow RowP = sheet.Table.Rows.Add();
                        RowP.Height = 14;
                        RowP.AutoFitHeight = false;
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s107";
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s113";
                        cell = RowP.Cells.Add();
                        cell.StyleID = "s114";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = Tmp_Sel_Text;
                        cell.MergeAcross = 2;
                    }
                }
            }

            // -----------------------------------------------

            // -----------------------------------------------
            WorksheetRow Row23 = sheet.Table.Rows.Add();
            Row23.Height = 12;
            Row23.AutoFitHeight = false;
            cell = Row23.Cells.Add();
            cell.StyleID = "s107";
            cell = Row23.Cells.Add();
            cell.StyleID = "s113";
            cell = Row23.Cells.Add();
            cell.StyleID = "s107";
            cell = Row23.Cells.Add();
            cell.StyleID = "s108";
            cell = Row23.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            Row24.Height = 12;
            Row24.AutoFitHeight = false;
            cell = Row24.Cells.Add();
            cell.StyleID = "s107";
            cell = Row24.Cells.Add();
            cell.StyleID = "s113";
            cell = Row24.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row25 = sheet.Table.Rows.Add();
            Row25.Height = 12;
            Row25.AutoFitHeight = false;
            cell = Row25.Cells.Add();
            cell.StyleID = "s107";
            cell = Row25.Cells.Add();
            cell.StyleID = "s113";
            cell = Row25.Cells.Add();
            cell.StyleID = "s107";
            cell = Row25.Cells.Add();
            cell.StyleID = "s108";
            cell = Row25.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row26 = sheet.Table.Rows.Add();
            Row26.Height = 12;
            Row26.AutoFitHeight = false;
            cell = Row26.Cells.Add();
            cell.StyleID = "s107";
            cell = Row26.Cells.Add();
            cell.StyleID = "s113";
            cell = Row26.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row27 = sheet.Table.Rows.Add();
            cell = Row27.Cells.Add();
            cell.StyleID = "s107";
            cell = Row27.Cells.Add();
            cell.StyleID = "s113";
            cell = Row27.Cells.Add();
            cell.StyleID = "s107";
            cell = Row27.Cells.Add();
            cell.StyleID = "s108";
            cell = Row27.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row28 = sheet.Table.Rows.Add();
            cell = Row28.Cells.Add();
            cell.StyleID = "s107";
            cell = Row28.Cells.Add();
            cell.StyleID = "s120";
            cell = Row28.Cells.Add();
            cell.StyleID = "m169114912";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;

            #endregion
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
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
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.Name = "Normal 3";
            s106.Font.FontName = "Calibri";
            s106.Font.Size = 11;
            s106.Font.Color = "#000000";
            s106.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m169114912
            // -----------------------------------------------
            WorksheetStyle m169114912 = styles.Add("m169114912");
            m169114912.Parent = "s106";
            m169114912.Font.FontName = "Arial";
            m169114912.Font.Color = "#800080";
            m169114912.Interior.Color = "#FFFFFF";
            m169114912.Interior.Pattern = StyleInteriorPattern.Solid;
            m169114912.Alignment.Vertical = StyleVerticalAlignment.Top;
            m169114912.Alignment.WrapText = true;
            m169114912.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m169114912.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m169114912.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m191119360
            // -----------------------------------------------
            WorksheetStyle m191119360 = styles.Add("m191119360");
            m191119360.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119360.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119360.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m191119460
            // -----------------------------------------------
            WorksheetStyle m191119460 = styles.Add("m191119460");
            m191119460.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119460.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119460.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m191119480
            // -----------------------------------------------
            WorksheetStyle m191119480 = styles.Add("m191119480");
            m191119480.Font.FontName = "Calibri";
            m191119480.Font.Size = 11;
            m191119480.Font.Color = "#000000";
            m191119480.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119480.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119480.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315584
            // -----------------------------------------------
            WorksheetStyle m137315584 = styles.Add("m137315584");
            m137315584.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315764
            // -----------------------------------------------
            WorksheetStyle m137315764 = styles.Add("m137315764");
            m137315764.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315784
            // -----------------------------------------------
            WorksheetStyle m137315784 = styles.Add("m137315784");
            m137315784.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315804
            // -----------------------------------------------
            WorksheetStyle m137315804 = styles.Add("m137315804");
            m137315804.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315824
            // -----------------------------------------------
            WorksheetStyle m137315824 = styles.Add("m137315824");
            m137315824.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s16.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.Bold = true;
            s17.Font.FontName = "Calibri";
            s17.Font.Size = 11;
            s17.Font.Color = "#000000";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s20.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            // -----------------------------------------------
            WorksheetStyle s2012 = styles.Add("s2012");
            s2012.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s2012.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s2012.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Interior.Color = "#F4B084";
            s22.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s24.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Calibri";
            s25.Font.Size = 11;
            s25.Font.Color = "#203764";
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s26.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            // -----------------------------------------------
            //  s29
            // -----------------------------------------------
            WorksheetStyle s29 = styles.Add("s29");
            s29.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s29.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s30
            // -----------------------------------------------
            WorksheetStyle s30 = styles.Add("s30");
            s30.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s30.Alignment.Vertical = StyleVerticalAlignment.Center;
            s30.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s31
            // -----------------------------------------------
            WorksheetStyle s31 = styles.Add("s31");
            s31.Font.FontName = "Calibri";
            s31.Font.Size = 8;
            s31.Font.Color = "#000000";
            // -----------------------------------------------
            //  s32
            // -----------------------------------------------
            WorksheetStyle s32 = styles.Add("s32");
            s32.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s33
            // -----------------------------------------------
            WorksheetStyle s33 = styles.Add("s33");
            s33.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s33.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s47
            // -----------------------------------------------
            WorksheetStyle s47 = styles.Add("s47");
            s47.Font.Bold = true;
            s47.Font.FontName = "Calibri";
            s47.Font.Size = 12;
            s47.Font.Color = "#000000";
            s47.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s47.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s48
            // -----------------------------------------------
            WorksheetStyle s48 = styles.Add("s48");
            s48.Font.Bold = true;
            s48.Font.FontName = "Calibri";
            s48.Font.Size = 11;
            s48.Font.Color = "#000000";
            s48.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s48.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s50
            // -----------------------------------------------
            WorksheetStyle s50 = styles.Add("s50");
            s50.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s50.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s50.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s501
            // -----------------------------------------------
            WorksheetStyle s501 = styles.Add("s501");
            s501.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s501.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s501.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s51
            // -----------------------------------------------
            WorksheetStyle s51 = styles.Add("s51");
            s51.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s51.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s55
            // -----------------------------------------------
            WorksheetStyle s55 = styles.Add("s55");
            s55.Font.FontName = "Calibri";
            s55.Font.Size = 11;
            s55.Font.Color = "#203764";
            s55.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s55.Alignment.Vertical = StyleVerticalAlignment.Center;
            s55.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Parent = "s106";
            s107.Font.FontName = "Calibri";
            s107.Font.Size = 11;
            s107.Interior.Color = "#FFFFFF";
            s107.Interior.Pattern = StyleInteriorPattern.Solid;
            s107.Alignment.Vertical = StyleVerticalAlignment.Top;
            s107.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Parent = "s106";
            s108.Font.FontName = "Calibri";
            s108.Font.Size = 11;
            s108.Interior.Color = "#FFFFFF";
            s108.Interior.Pattern = StyleInteriorPattern.Solid;
            s108.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s109
            // -----------------------------------------------
            WorksheetStyle s109 = styles.Add("s109");
            s109.Parent = "s106";
            s109.Font.FontName = "Calibri";
            s109.Font.Size = 11;
            s109.Interior.Color = "#FFFFFF";
            s109.Interior.Pattern = StyleInteriorPattern.Solid;
            s109.Alignment.Vertical = StyleVerticalAlignment.Top;
            s109.Alignment.WrapText = true;
            s109.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s109.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s109C
            // -----------------------------------------------
            WorksheetStyle s109C = styles.Add("s109C");
            s109C.Parent = "s106";
            s109C.Font.FontName = "Calibri";
            s109C.Font.Size = 11;
            s109C.Interior.Color = "#FFFFFF";
            s109C.Interior.Pattern = StyleInteriorPattern.Solid;
            s109C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s109C.Alignment.WrapText = true;
            s109C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s110
            // -----------------------------------------------
            WorksheetStyle s110 = styles.Add("s110");
            s110.Parent = "s106";
            s110.Font.FontName = "Calibri";
            s110.Font.Size = 11;
            s110.Interior.Color = "#FFFFFF";
            s110.Interior.Pattern = StyleInteriorPattern.Solid;
            s110.Alignment.Vertical = StyleVerticalAlignment.Top;
            s110.Alignment.WrapText = true;
            s110.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s111
            // -----------------------------------------------
            WorksheetStyle s111 = styles.Add("s111");
            s111.Parent = "s106";
            s111.Font.FontName = "Calibri";
            s111.Font.Size = 11;
            s111.Interior.Color = "#FFFFFF";
            s111.Interior.Pattern = StyleInteriorPattern.Solid;
            s111.Alignment.Vertical = StyleVerticalAlignment.Top;
            s111.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s111P
            // -----------------------------------------------
            WorksheetStyle s111P = styles.Add("s111P");
            s111P.Parent = "s106";
            s111P.Font.FontName = "Calibri";
            s111P.Font.Size = 11;
            s111P.Interior.Color = "#FFFFFF";
            s111P.Font.Color = "#000000";
            s111P.Interior.Pattern = StyleInteriorPattern.Solid;
            s111P.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s112
            // -----------------------------------------------
            WorksheetStyle s112 = styles.Add("s112");
            s112.Parent = "s106";
            s112.Font.FontName = "Calibri";
            s112.Font.Size = 11;
            s112.Interior.Color = "#FFFFFF";
            s112.Interior.Pattern = StyleInteriorPattern.Solid;
            s112.Alignment.Vertical = StyleVerticalAlignment.Top;
            s112.Alignment.WrapText = true;
            s112.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s112.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s112C
            // -----------------------------------------------
            WorksheetStyle s112C = styles.Add("s112C");
            s112C.Parent = "s106";
            s112C.Font.FontName = "Calibri";
            s112C.Font.Size = 11;
            s112C.Font.Color = "#800080";
            s112C.Interior.Color = "#FFFFFF";
            s112C.Interior.Pattern = StyleInteriorPattern.Solid;
            s112C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s112C.Alignment.WrapText = true;
            s112C.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s112C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s113
            // -----------------------------------------------
            WorksheetStyle s113 = styles.Add("s113");
            s113.Parent = "s106";
            s113.Font.FontName = "Calibri";
            s113.Font.Size = 11;
            s113.Interior.Color = "#FFFFFF";
            s113.Interior.Pattern = StyleInteriorPattern.Solid;
            s113.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113.Alignment.WrapText = true;
            s113.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s113C
            // -----------------------------------------------
            WorksheetStyle s113C = styles.Add("s113C");
            s113C.Parent = "s106";
            s113C.Font.FontName = "Calibri";
            s113C.Font.Size = 11;
            s113C.Interior.Color = "#FFFFFF";
            s113C.Interior.Pattern = StyleInteriorPattern.Solid;
            s113C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113C.Alignment.WrapText = true;
            s113C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s113C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s114
            // -----------------------------------------------
            WorksheetStyle s114 = styles.Add("s114");
            s114.Parent = "s106";
            s114.Font.FontName = "Arial";
            s114.Font.Color = "#800080";
            s114.Interior.Color = "#FFFFFF";
            s114.Interior.Pattern = StyleInteriorPattern.Solid;
            s114.Alignment.Vertical = StyleVerticalAlignment.Top;
            s114.Alignment.WrapText = true;
            s114.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s114.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s114P
            // -----------------------------------------------
            WorksheetStyle s114P = styles.Add("s114P");
            s114P.Parent = "s106";
            s114P.Font.FontName = "Arial";
            s114P.Font.Color = "#000000";
            s114P.Interior.Color = "#FFFFFF";
            s114P.Interior.Pattern = StyleInteriorPattern.Solid;
            s114P.Alignment.Vertical = StyleVerticalAlignment.Top;
            s114P.Alignment.WrapText = true;
            s114P.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s114P.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s115
            // -----------------------------------------------
            WorksheetStyle s115 = styles.Add("s115");
            s115.Parent = "s106";
            s115.Font.Bold = true;
            s115.Font.FontName = "Arial";
            s115.Font.Size = 14;
            s115.Font.Color = "#800080";
            s115.Interior.Color = "#FFFFFF";
            s115.Interior.Pattern = StyleInteriorPattern.Solid;
            s115.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s115.Alignment.Vertical = StyleVerticalAlignment.Top;
            s115.Alignment.WrapText = true;
            s115.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s115.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s116
            // -----------------------------------------------
            WorksheetStyle s116 = styles.Add("s116");
            s116.Parent = "s106";
            s116.Font.FontName = "Calibri";
            s116.Font.Size = 11;
            s116.Interior.Color = "#FFFFFF";
            s116.Interior.Pattern = StyleInteriorPattern.Solid;
            s116.Alignment.Vertical = StyleVerticalAlignment.Top;
            s116.Alignment.WrapText = true;
            s116.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s117
            // -----------------------------------------------
            WorksheetStyle s117 = styles.Add("s117");
            s117.Parent = "s106";
            s117.Font.FontName = "Arial";
            s117.Font.Color = "#800080";
            s117.Interior.Color = "#FFFFFF";
            s117.Interior.Pattern = StyleInteriorPattern.Solid;
            s117.Alignment.Vertical = StyleVerticalAlignment.Top;
            s117.Alignment.WrapText = true;
            s117.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s118
            // -----------------------------------------------
            WorksheetStyle s118 = styles.Add("s118");
            s118.Parent = "s106";
            s118.Font.FontName = "Arial";
            s118.Font.Color = "#800080";
            s118.Interior.Color = "#FFFFFF";
            s118.Interior.Pattern = StyleInteriorPattern.Solid;
            s118.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s118.Alignment.Vertical = StyleVerticalAlignment.Top;
            s118.Alignment.WrapText = true;
            s118.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s118.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s119
            // -----------------------------------------------
            WorksheetStyle s119 = styles.Add("s119");
            s119.Parent = "s106";
            s119.Font.FontName = "Arial";
            s119.Font.Color = "#800080";
            s119.Interior.Color = "#FFFFFF";
            s119.Interior.Pattern = StyleInteriorPattern.Solid;
            s119.Alignment.Vertical = StyleVerticalAlignment.Top;
            s119.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s120
            // -----------------------------------------------
            WorksheetStyle s120 = styles.Add("s120");
            s120.Parent = "s106";
            s120.Font.FontName = "Calibri";
            s120.Font.Size = 11;
            s120.Interior.Color = "#FFFFFF";
            s120.Interior.Pattern = StyleInteriorPattern.Solid;
            s120.Alignment.Vertical = StyleVerticalAlignment.Top;
            s120.Alignment.WrapText = true;
            s120.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s120.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s123
            // -----------------------------------------------
            WorksheetStyle s123 = styles.Add("s123");
            s123.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        }

        bool[] Pagesetup_results = new bool[5];
        bool Include_header = false, Include_Footer = false, Include_Header_Title = false, Include_Header_Image = false,
             Include_Footer_PageCnt = false, Save_This_Adhoc_Criteria = false;
        string Rep_Name = " ", Rep_Header_Title = string.Empty, Page_Orientation = "A4 Portrait", Pub_SubRep_Name = string.Empty;

        private void HSSB0026_PIRCounting_From_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }

        private void On_Pagesetup_Form_Closed(object sender, FormClosedEventArgs e)
        {
            Include_header = Include_Footer = Include_Header_Title = Include_Header_Image =
            Include_Footer_PageCnt = Save_This_Adhoc_Criteria = false;

            Rep_Name = " ";
            Rep_Header_Title = string.Empty;
            Page_Orientation = "A4 Portrait";

            CASB2012_AdhocPageSetup form = sender as CASB2012_AdhocPageSetup;
            if (form.DialogResult == DialogResult.OK)
            {
                Pagesetup_results = form.Get_Checkbox_Status();

                Include_header = Pagesetup_results[0];
                Include_Header_Title = Pagesetup_results[1];
                Include_Header_Image = Pagesetup_results[2];
                Include_Footer = Pagesetup_results[3];
                Include_Footer_PageCnt = Pagesetup_results[4];
                Save_This_Adhoc_Criteria = Pagesetup_results[5];

                if (Include_Header_Title)
                    Rep_Header_Title = form.Get_Header_Title();

                Pub_SubRep_Name = Rep_Name = "SYSTEM " + form.Get_Report_Name();
                Rep_Name += ".rdlc";
                Pub_SubRep_Name += "SummaryReport";

                Page_Orientation = form.Get_Page_Orientation();

                if (Generate_Report_Type != "Normal")
                    Get_Question_Audit_Table();

                Dynamic_RDLC();

                //

                CASB2012_AdhocRDLCForm RDLC_Form;
                if (Generate_Report_Type == "Normal")
                    RDLC_Form = new CASB2012_AdhocRDLCForm(PIR_Report_Table, PIR_Report_Table, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, string.Empty);
                else
                    RDLC_Form = new CASB2012_AdhocRDLCForm(PIR_Audit_Table, PIR_Audit_Table, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, string.Empty);

                RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
                RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
                RDLC_Form.ShowDialog();

                //AlertBox.Show("Report Generated Successfully");
            }
        }

        DirectoryInfo MyDir;
        private void Delete_Dynamic_RDLC_File(object sender, FormClosedEventArgs e)
        {
            CASB2012_AdhocRDLCForm form = sender as CASB2012_AdhocRDLCForm;
            MyDir = new DirectoryInfo(ReportPath + "\\");

            FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
            Pub_SubRep_Name += ".rdlc";
            foreach (FileInfo MyFile in MyFiles)
            {
                if (Rep_Name == MyFile.Name && MyFile.Exists)
                {
                    MyFile.Delete();
                    break;
                }
            }
        }

        bool AudiT_Question_Selected = false;
        private void Grid_Questions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Grid_Questions.Rows.Count > 0)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                ColIdx = Grid_Questions.CurrentCell.ColumnIndex;
                RowIdx = Grid_Questions.CurrentCell.RowIndex;

                AudiT_Question_Selected = false;
                if (e.ColumnIndex == 0 && e.RowIndex != -1 && Grid_Questions.CurrentRow.Cells["GD_Ques_Seq"].Value.ToString() != "0")
                {
                    if (Grid_Questions.CurrentRow.Cells["GD_Sel"].Value.ToString() == true.ToString())
                        AudiT_Question_Selected = true;
                }

                if (Grid_Questions.CurrentRow.Cells["GD_Ques_Seq"].Value.ToString() == "0")
                    Grid_Questions.CurrentRow.Cells["GD_Sel"].Value = AudiT_Question_Selected = false;

                /* foreach (DataGridViewRow dr in Grid_Questions.Rows)
                 {
                     if (dr.Cells["GD_Sel"].Value.ToString() == true.ToString())
                     {
                         if (dr.Cells["GD_Ques_ID"].Value.ToString() != Grid_Questions.CurrentRow.Cells["GD_Ques_ID"].Value.ToString() ||
                            dr.Cells["GD_Ques_Seq"].Value.ToString() != Grid_Questions.CurrentRow.Cells["GD_Ques_Seq"].Value.ToString() ||
                            dr.Cells["GD_Ques_Type"].Value.ToString() != Grid_Questions.CurrentRow.Cells["GD_Ques_Type"].Value.ToString())
                             dr.Cells["GD_Sel"].Value = false;
                     }

                 }*/
            }
        }

        string Sql_SP_Result_Message = string.Empty;
        private void Btn_Save_Counts_Click(object sender, EventArgs e)
        {
            StringBuilder Xml_To_Pass = new StringBuilder();
            Xml_To_Pass.Append("<Rows>");

            PIRCOUNTEntity Update_Entity = new PIRCOUNTEntity();
            Update_Entity.Agency = Current_Hierarchy.Substring(0, 2);
            Update_Entity.Dept = Current_Hierarchy.Substring(2, 2);
            Update_Entity.Prog = Current_Hierarchy.Substring(4, 2);
            Update_Entity.Year = (CmbYear.Visible ? ((ListItem)CmbYear.SelectedItem).Text.ToString() : "    ");
            Update_Entity.Site = ((ListItem)Cmb_Site.SelectedItem).Value.ToString();
            string strFund = string.Empty;
            strFund = ((ListItem)cmbFunds.SelectedItem).Value.ToString();
            //if (Rb_HSFund.Checked == true)
            //    strFund = "H";
            //if (Rb_EHSFund.Checked == true)
            //    strFund = "E";
            //if (rdoEHSCCPFund.Checked == true)
            //    strFund = "S";
            Update_Entity.Ques_Fund = strFund;
            Update_Entity.Lstc_Operator = BaseForm.UserID;

            foreach (PIRQUESTEntity Entity in Questions_List)
            {
                if (Entity.Ques_Unique_ID == "3090")
                    Update_Entity.Ques_Fund = strFund;

                foreach (DataGridViewRow dr in Grid_Questions.Rows)
                {
                    if (dr.Cells["GD_Ques_ID"].Value.ToString() == Entity.Ques_Unique_ID &&
                       dr.Cells["GD_Ques_Seq"].Value.ToString() == Entity.Ques_Seq &&
                       dr.Cells["GD_Ques_Type"].Value.ToString() == Entity.Ques_Type &&
                       dr.Cells["GD_Ques_SCode"].Value.ToString() == Entity.Ques_SCode &&
                       dr.Cells["GD_Ques_Code"].Value.ToString() == Entity.Ques_Code)
                    {
                        if (dr.Cells["GD_User_Cnt"].Value != null)
                            Entity.Cnt_User_Cnt = dr.Cells["GD_User_Cnt"].Value.ToString();
                        else
                            Entity.Cnt_User_Cnt = "0";

                        if (string.IsNullOrEmpty(Entity.Cnt_User_Cnt.Trim()))
                            Entity.Cnt_User_Cnt = "0";
                        break;
                    }
                }

                Xml_To_Pass.Append("<Row Ques_ID = \"" + Entity.Ques_Unique_ID + "\" Ques_Sec = \"" + Entity.Ques_Seq + "\" Ques_Sys_Cnt = \"" + Entity.Cnt_System_Cnt + "\" Ques_User_Cnt = \"" + Entity.Cnt_User_Cnt +
                           "\" Ques_Agy_Flag = \"" + " " + "\" Ques_Agy_Code = \"" + " " + "\" Ques_SQue_Code = \"" + Entity.Ques_SCode.Trim() + "\" Ques_Que_Code = \"" + Entity.Ques_Code.Trim() + "\"/>");
            }
            Xml_To_Pass.Append("</Rows>");


            bool save_result = false;
            if (_model.PIRData.UpdatePIRCOUNT(Update_Entity, "Update", Xml_To_Pass.ToString(), out Sql_SP_Result_Message))
            {
                Fill_Questions_Grid();
                AlertBox.Show("PIR Counts Updated Successfully");
                //Btn_Save_Counts.Enabled = false;
            }
            else
                AlertBox.Show("Failed to Update", MessageBoxIcon.Warning);
        }





        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       Dynamic RDLC    >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        private void Dynamic_RDLC()
        {

            //Get_Report_Selection_Parameters();

            List<DG_ResTab_Entity> Table_ListTo_Print = new List<DG_ResTab_Entity>();
            if (Generate_Report_Type == "Normal")
                Table_ListTo_Print = PIR_Table_List;
            else
                Table_ListTo_Print = PIR_Audit_Table_List;

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

            //foreach (DG_ResTab_Entity Entity in PIR_Table_List)
            foreach (DG_ResTab_Entity Entity in Table_ListTo_Print)
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

            int Rect_Total_Rows = 15;

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
                string strFund = string.Empty;
                strFund = ((ListItem)cmbFunds.SelectedItem).Text.ToString();
                //if (Rb_HSFund.Checked == true)
                //    strFund = "HS";
                //if (Rb_EHSFund.Checked == true)
                //    strFund = "EHS";
                //if (rdoEHSCCPFund.Checked == true)
                //    strFund = "EHSCCEP";
                switch (i)
                {
                    case 1:
                        Tmp_Sel_Text = "Selected Report Parameters";
                        break;

                    case 3:
                        Tmp_Sel_Text = "            Agency: " + Current_Hierarchy.Substring(0, 2) + " , Department : " + Current_Hierarchy.Substring(2, 2) + " , Program : " + Current_Hierarchy.Substring(4, 2);
                        break;

                    case 6:
                        Tmp_Sel_Text = "            Section";
                        break;
                    case 7:
                        Tmp_Sel_Text = " : " + ((ListItem)Cmb_Section.SelectedItem).Text.ToString();
                        break;
                    case 8:
                        Tmp_Sel_Text = "            Funding Source";
                        break;
                    case 9:
                        Tmp_Sel_Text = " : " + strFund;
                        break;
                    case 10:
                        Tmp_Sel_Text = "            Site";
                        break;
                    case 11:
                        Tmp_Sel_Text = " : " + ((ListItem)Cmb_Site.SelectedItem).Text.ToString();
                        break;

                    case 13:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_Text = Audit_HeadQues_desc;
                        }
                        else
                            Tmp_Sel_Text = "";
                        break;
                    case 14:
                        if (Generate_Report_Type != "Normal")
                        {
                            Tmp_Sel_Text = "Question: " + Audit_Ques_Desc;
                        }
                        else
                            Tmp_Sel_Text = "";
                        break;
                    default:
                        Tmp_Sel_Text = "  ";
                        break;
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

                //Total_Sel_TextBox_Height += 0.21875;

                XmlElement Textbox1_Left = xml.CreateElement("Left");
                //Textbox1_Left.InnerText = "0.07292in";
                Textbox1_Left.InnerText = ((i > 3 && i < 12 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                if (i > 3 && i < 12 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                {
                    if (i % 2 != 0)
                        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                }
                else
                    Total_Sel_TextBox_Height += 0.21855;


                XmlElement Textbox1_Height = xml.CreateElement("Height");
                Textbox1_Height.InnerText = "0.21875in";
                Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                //XmlElement Textbox1_Width = xml.CreateElement("Width");
                ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? (10 < total_Columns_Width ? "10in" : total_Columns_Width.ToString() + "in") : "7.48777in"); // "6.35055in";
                //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "10in" : "7.48777in"); // "6.35055in";
                //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);


                XmlElement Textbox1_Width = xml.CreateElement("Width");
                //Textbox1_Width.InnerText = "7.48777";
                Textbox1_Width.InnerText = ((i > 4 && i < 12 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
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


            string Task_Desc = string.Empty, Task_Condition = string.Empty, Task_Conjunction = string.Empty;
            for (int i = 0; i < (PIR_Audit_Conditions_Table.Rows.Count + 1); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
                    Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Cond_Textbox" + i.ToString() + j.ToString());
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

                    Task_Desc = Task_Condition = Task_Conjunction = Tmp_Sel_Text = string.Empty;
                    if (i < (PIR_Audit_Conditions_Table.Rows.Count))
                    {
                        Task_Desc = PIR_Audit_Conditions_Table.Rows[i]["Task"].ToString();
                        Task_Condition = PIR_Audit_Conditions_Table.Rows[i]["Condition"].ToString();
                        if (i < (PIR_Audit_Conditions_Table.Rows.Count - 1))
                            Task_Conjunction = PIR_Audit_Conditions_Table.Rows[i]["Conj"].ToString();

                        switch (j)
                        {
                            case 0:
                                Tmp_Sel_Text = "Task = " + Task_Condition;
                                break;
                            case 1:
                                Tmp_Sel_Text = "          " + Task_Conjunction;
                                break;
                            default:
                                Tmp_Sel_Text = "  ";
                                break;
                        }
                    }

                    Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
                    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);

                    XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
                    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

                    XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                    Textbox1_TextRun_Style_Color.InnerText = (j == 1 ? "DarkViolet" : "Black");// "DarkViolet";
                    Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


                    XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                    Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


                    XmlElement Textbox1_Top = xml.CreateElement("Top");
                    Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
                    Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

                    XmlElement Textbox1_Left = xml.CreateElement("Left");
                    //Textbox1_Left.InnerText = ((i > 3 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
                    Textbox1_Left.InnerText = "0.07292in";
                    Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

                    //if (i > 3 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
                    //{
                    //    if (i % 2 != 0)
                    //        Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
                    //}
                    //else
                    //    Total_Sel_TextBox_Height += 0.21855;

                    Total_Sel_TextBox_Height += 0.21855;

                    XmlElement Textbox1_Height = xml.CreateElement("Height");
                    Textbox1_Height.InnerText = "0.21875in";
                    Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

                    XmlElement Textbox1_Width = xml.CreateElement("Width");
                    //Textbox1_Width.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
                    Textbox1_Width.InnerText = "7.48777in";
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
            Sel_Rectangle_Width.InnerText = "7.48777in";
            //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 8 ? "10in" : "7.48777in"); // "6.35055in";
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

            //foreach (DG_ResTab_Entity Entity in PIR_Table_List)                      // Dynamic based on Display Columns in Result Table
            foreach (DG_ResTab_Entity Entity in Table_ListTo_Print)
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
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
            //foreach (DG_ResTab_Entity Entity in PIR_Table_List)            // Dynamic based on Display Columns in Result Table
            foreach (DG_ResTab_Entity Entity in Table_ListTo_Print)
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

                    //if (Entity.Disp_Name != "BM_Scale")
                    //{
                    //    XmlElement Head_WritingMode = xml.CreateElement("WritingMode");
                    //    Head_WritingMode.InnerText = "Vertical";
                    //    Cell_style.AppendChild(Head_WritingMode);
                    //}
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

            //foreach (DG_ResTab_Entity Entity in PIR_Table_List)        // Dynamic based on Display Columns in Result Table
            foreach (DG_ResTab_Entity Entity in Table_ListTo_Print)
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
                            Text_Align = "Right";
                            break;
                    }

                    if (Entity.Column_Name == "App_Attn_Date")
                        Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";

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

            ///int Tablix_Members = (Generate_Report_Type == "Normal" ? 3 : 6);
            int Tablix_Members = (Generate_Report_Type == "Normal" ? 3 : 8);
            for (int Loop = 0; Loop < Tablix_Members; Loop++)            // Dynamic based on Display Columns in 3/6 
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

            XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            Tablix.AppendChild(SubReport_PageBreak);

            XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            SubReport_PageBreak_Location.InnerText = "End";
            SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

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

            //if (Summary_Sw)
            //{ // Summary Sub Report //}

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
                    Header_TextBox_Top.InnerText = "0.07792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");   // 
                    Header_TextBox_Left.InnerText = (true ? "1.42361in" : "2.42361in");
                    //Header_TextBox_Left.InnerText = (Rb_A4_Port.Checked ? "1.42361in" : "2.42361in");
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
                    Footer_TextBox_Left.InnerText = "7.3in";    // Rao
                    //Footer_TextBox_Left.InnerText = (total_Columns_Width > 7.48777 ? "9.5in" : "7.3in");    // Rao
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
            if (true)
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
                xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Console.ReadLine();
        }


        private void Add_Controls_To_Report()
        {
            //this.panel1.Controls.Add(this.Rb_EHSFund);
            this.pnlParams.Controls.Add(this.Grid_Questions);
            this.pnlParams.Controls.Add(this.lblSite);
            this.pnlParams.Controls.Add(this.Cmb_Site);
            this.pnlParams.Controls.Add(this.Cmb_Section);
            //this.panel1.Controls.Add(this.Rb_HSFund);
            this.pnlParams.Controls.Add(this.lblSection);
            this.pnlHie.Controls.Add(this.CmbYear);
            this.pnlHie.Controls.Add(this.Txt_HieDesc);
            this.pnlGenerate.Controls.Add(this.Btn_Save_Counts);
            this.pnlGenerate.Controls.Add(this.Btn_Generate_Count);
            this.pnlGenerate.Controls.Add(this.Btn_Audit);
            this.pnlGenerate.Controls.Add(this.Btn_Generate_Report);
            this.Controls.Add(this.Pb_Search_Hie);
            this.Controls.Add(this.pnlGenerate);
            this.Controls.Add(this.pnlHie);
            this.Controls.Add(this.pnlParams);
        }

        private void Grid_Questions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex != -1 && Grid_Questions.CurrentRow.Cells["GD_Ques_Seq"].Value.ToString() != "0")
            {
                if (Grid_Questions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    if (!string.IsNullOrEmpty(Grid_Questions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim()))
                    {
                        int number;
                        if ((!(int.TryParse(Grid_Questions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out number))) &&
                        !(string.IsNullOrEmpty(Grid_Questions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())))
                        {
                            AlertBox.Show("Please enter Integer Count", MessageBoxIcon.Warning);
                            Grid_Questions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        }
                    }
                }
            }
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB0026");
        }

        #region Excel Functions added by Vikash on 04/02/2024 as per PIR Counting Enhancements document to print Single and Multiple Audits
        private void GenerateSingleAuditSheets(string PdfName, Workbook book, Worksheet sheet, DataTable Result_table)
        {
            try
            {
                WorksheetRow excelrowSpace;
                WorksheetRow excelrow1;
                WorksheetRow excelrow2;
                WorksheetCell cell1;

                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(20);
                sheet.Table.Columns.Add(30);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(200);

                excelrowSpace = sheet.Table.Rows.Add();
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");

                excelrow1 = sheet.Table.Rows.Add();
                cell1 = excelrow1.Cells.Add("", DataType.String, "NormalLeft");
                cell1 = excelrow1.Cells.Add("Hit", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("Family ID", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("Client ID", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("App#", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("Applicant Name", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("1st Attn", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("Site/Room", DataType.String, "MainHeaderStyles");
                cell1 = excelrow1.Cells.Add("Counting Logic results", DataType.String, "MainHeaderStyles");

                foreach (DataRow dr in Result_table.Rows)
                {
                    excelrow2 = sheet.Table.Rows.Add();
                    cell1 = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Hit_Num"].ToString().Trim() == "" ? "" : dr["App_Hit_Num"].ToString().Trim(), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Fam_ID"].ToString().Trim() == "" ? "" : dr["App_Fam_ID"].ToString().Trim(), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Client_ID"].ToString().Trim() == "" ? "" : dr["App_Client_ID"].ToString().Trim(), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_App"].ToString().Trim() == "" ? "" : dr["App_App"].ToString().Trim(), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Namee"].ToString().Trim() == "" ? "" : dr["App_Namee"].ToString().Trim(), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Attn_Date"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["App_Attn_Date"]).ToString("MM/dd/yyyy"), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add((dr["App_Site"].ToString().Trim() == "" ? "" : dr["App_Site"].ToString().Trim()) + (dr["App_Room"].ToString().Trim() == "" ? "" : dr["App_Room"].ToString().Trim()) + (dr["App_AMPM"].ToString().Trim() == "" ? "" : dr["App_AMPM"].ToString().Trim()), DataType.String, "NormalLeft");
                    cell1 = excelrow2.Cells.Add(dr["App_Logic_Results"].ToString().Trim() == "" ? "" : dr["App_Logic_Results"].ToString().Trim(), DataType.String, "NormalLeft");
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }
        private void GenerateMultipleAuditSheets(string PdfName, Workbook book, DataSet Result_table)
        {
            int sheetcount = 1;
            try
            {
                if (Result_table.Tables.Count > 0)
                {
                    foreach (DataTable dt in Result_table.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            WorksheetRow excelrowSpace;
                            WorksheetRow excelrow1;
                            WorksheetRow excelrow2;
                            WorksheetCell cell1;
                            Worksheet sheet = book.Worksheets.Add("Sheet " + "Audit_" + sheetcount);
                            sheetcount++;
                            sheet.Table.DefaultRowHeight = 14.25F;

                            sheet.Table.DefaultColumnWidth = 220.5F;
                            sheet.Table.Columns.Add(20);
                            sheet.Table.Columns.Add(30);
                            sheet.Table.Columns.Add(100);
                            sheet.Table.Columns.Add(80);
                            sheet.Table.Columns.Add(100);
                            sheet.Table.Columns.Add(150);
                            sheet.Table.Columns.Add(80);
                            sheet.Table.Columns.Add(100);
                            sheet.Table.Columns.Add(200);

                            excelrowSpace = sheet.Table.Rows.Add();
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");

                            excelrow1 = sheet.Table.Rows.Add();
                            cell1 = excelrow1.Cells.Add("", DataType.String, "NormalLeft");
                            cell1 = excelrow1.Cells.Add("Hit", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("Family ID", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("Client ID", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("App#", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("Applicant Name", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("1st Attn", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("Site/Room", DataType.String, "MainHeaderStyles");
                            cell1 = excelrow1.Cells.Add("Counting Logic results", DataType.String, "MainHeaderStyles");

                            foreach (DataRow dr in dt.Rows)
                            {
                                excelrow2 = sheet.Table.Rows.Add();
                                cell1 = excelrow2.Cells.Add("", DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Hit_Num"].ToString().Trim() == "" ? "" : dr["App_Hit_Num"].ToString().Trim(), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Fam_ID"].ToString().Trim() == "" ? "" : dr["App_Fam_ID"].ToString().Trim(), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Client_ID"].ToString().Trim() == "" ? "" : dr["App_Client_ID"].ToString().Trim(), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_App"].ToString().Trim() == "" ? "" : dr["App_App"].ToString().Trim(), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Namee"].ToString().Trim() == "" ? "" : dr["App_Namee"].ToString().Trim(), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Attn_Date"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["App_Attn_Date"]).ToString("MM/dd/yyyy"), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add((dr["App_Site"].ToString().Trim() == "" ? "" : dr["App_Site"].ToString().Trim()) + (dr["App_Room"].ToString().Trim() == "" ? "" : dr["App_Room"].ToString().Trim()) + (dr["App_AMPM"].ToString().Trim() == "" ? "" : dr["App_AMPM"].ToString().Trim()), DataType.String, "NormalLeft");
                                cell1 = excelrow2.Cells.Add(dr["App_Logic_Results"].ToString().Trim() == "" ? "" : dr["App_Logic_Results"].ToString().Trim(), DataType.String, "NormalLeft");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }
        #endregion
    }
}