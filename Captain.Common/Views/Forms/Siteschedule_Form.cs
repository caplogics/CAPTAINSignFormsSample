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
    public partial class Siteschedule_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public Siteschedule_Form(BaseForm baseform, string mode, string agency, string dept, string prog, string year, string month, string site, string roomNo, string Ampm, string FundngSource, string hier_Desc, string hierar, string Sch_Type, string Calenderyear, PrivilegeEntity privileges, string ID)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            Agency = agency;
            Dept = dept; Prog = prog;
            Year = year; Month_No = month.Trim(); Site = site.Trim();
            strHiearchy = hierar; Room = roomNo; AMPM = Ampm;
            strHierarcy_Desc = hier_Desc; Fund_Source = FundngSource; Calander_Year = Calenderyear;
            Attm_ID = ID;
            Privileges = privileges; strSch_Type = Sch_Type;
            if (Privileges.ModuleCode == "02")
            {
                if (string.IsNullOrEmpty(strHiearchy.Trim()))
                    strHiearchy = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg;
            }

            hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            Form_Load();
            //FillMonths();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Month_No { get; set; }

        public string Attm_ID { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Prog { get; set; }

        public string Year { get; set; }

        public string Site { get; set; }

        public string Room { get; set; }

        public string AMPM { get; set; }

        public string Fund_Source { get; set; }

        public string Calander_Year { get; set; }

        public string strHierarcy_Desc { get; set; }

        public string HiearchyCode { get; set; }

        public string strHiearchy { get; set; }

        public string strSch_Type { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void Form_Load()
        {
            txtHierarchy.Text = strHiearchy;
            txtHierachydesc.Text = strHierarcy_Desc.Trim(); lblTxtYear.Text = Year;
            if (string.IsNullOrEmpty(txtHierarchy.Text.Trim()) && string.IsNullOrEmpty(txtHierachydesc.Text.Trim()))
                panel2.Enabled = false;
            else
                panel2.Visible = true;
            if (Mode == "Add")
            {
                this.Text = Privileges.PrivilegeName + " - Add";
                if (rbRoom.Checked.Equals(true))
                {
                    txtSite.Enabled = txtRoom.Enabled = txtAmPm.Enabled = btnSite.Enabled = true; label2.Visible = false;
                    chkbFund.Visible = false; cmbFund.Enabled = false; cmbMonth.Enabled = false; label1.Visible = false;
                }
                //Addschdule_List();
            }
            else if (Mode == "Edit")
            {
                this.Text = Privileges.PrivilegeName + " - Edit"; PbHierarchies.Visible = false;
                panel2.Enabled = panel3.Enabled = panel4.Enabled = false; chkbFund.Visible = false;
                lblMonthDis.Visible = false; label2.Visible = false;
                lblAMPMReq.Visible = lblSiteReq.Visible = lblRoomReq.Visible = false; cmbMonth.Visible = false;
                fillSiteSch_Controls();
            }
            else if (Mode == "View")
            {
                this.Text = Privileges.PrivilegeName + " - View"; PbHierarchies.Visible = false;
                panel2.Enabled = panel3.Enabled = panel4.Enabled = false; chkbFund.Visible = false;
                lblMonthDis.Visible = false; label2.Visible = false;
                lblAMPMReq.Visible = lblSiteReq.Visible = lblRoomReq.Visible = false; cmbMonth.Visible = false;
                fillSiteSch_Controls();
                EnableDisableControls();
            }


        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSS20135");
        }

        private void FillFundCombo()
        {
            this.cmbFund.SelectedIndexChanged -= new System.EventHandler(this.cmbFund_SelectedIndexChanged);
            cmbFund.Items.Clear();
            DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            DataTable dtFund = dsFund.Tables[0];
            if (Privileges.ModuleCode == "01")
            {
                dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                dtFund = dsFund.Tables[0];
            }

            cmbFund.Items.Insert(0, new ListItem("    ", "0"));
            if (dtFund.Rows.Count > 0)
            {
                foreach (DataRow drFund in dtFund.Rows)
                {
                    cmbFund.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));

                }
                cmbFund.SelectedIndex = 0;
            }
            this.cmbFund.SelectedIndexChanged += new System.EventHandler(this.cmbFund_SelectedIndexChanged);
        }
        List<SiteScheduleEntity> Site_scheduleList = new List<SiteScheduleEntity>();
        string list_schdeluedMonths = string.Empty;
        private void fillschdulemonth()
        {
            string Cal_Month = "01";
            SiteScheduleEntity Search_Schd = new SiteScheduleEntity(true);
            Search_Schd.ATTM_AGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
            Search_Schd.ATTM_DEPT = txtHierarchy.Text.Substring(3, 2).Trim(); Search_Schd.ATTM_PROG = txtHierarchy.Text.Substring(6, 2).Trim();
            Search_Schd.ATTM_YEAR = Year.Trim(); //Search_Schd.ATTM_MONTH = Month.Trim();
            Search_Schd.ATTM_SITE = txtSite.Text.Trim(); Search_Schd.ATTM_ROOM = txtRoom.Text.Trim();
            Search_Schd.ATTM_AMPM = txtAmPm.Text.Trim(); //Search_Schd.ATTM_FUND = Fund_Source.Trim();
            Site_scheduleList = _model.SPAdminData.Browse_CHILDATTM(Search_Schd, "Browse");
            list_schdeluedMonths = string.Empty;
            foreach (SiteScheduleEntity Entity in Site_scheduleList)
            {
                Cal_Month = Entity.ATTM_MONTH.Trim();
                Cal_Month = Cal_Month.Length == 1 ? "0" + Cal_Month : Cal_Month;
                if (rbSite.Checked)
                {
                    if (chkbFund.Checked)
                    {
                        if (string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()) && Entity.ATTM_FUND == ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim())
                            list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()))
                            list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                    }
                }
                else if (rbMaster.Checked)
                {
                    if (string.IsNullOrEmpty(Entity.ATTM_SITE.Trim()) && string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_FUND.Trim()))
                        list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                }
                else if (rbRoom.Checked)
                {
                    if (chkbFund.Checked)
                    {
                        if (Entity.ATTM_FUND == ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim())
                            list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                    }
                    else
                        list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                }
                else if (rbFund.Checked)
                {
                    if (Entity.ATTM_FUND != ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim())
                        list_schdeluedMonths += Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() + ",";
                }
            }
        }

        List<SiteScheduleEntity> Add_SchedulleList = new List<SiteScheduleEntity>(); string Added_SchID = string.Empty;
        private void Addschdule_List()
        {
            Added_SchID = string.Empty;
            SiteScheduleEntity Search_Add = new SiteScheduleEntity(true);
            Search_Add.ATTM_AGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
            Search_Add.ATTM_DEPT = txtHierarchy.Text.Substring(3, 2).Trim(); Search_Add.ATTM_PROG = txtHierarchy.Text.Substring(6, 2).Trim();
            Search_Add.ATTM_YEAR = Year.Trim(); //Search_Add.ATTM_MONTH = Month.Trim();
            Add_SchedulleList = _model.SPAdminData.Browse_CHILDATTM(Search_Add, "Browse");
            if (Add_SchedulleList.Count > 0)
            {
                foreach (SiteScheduleEntity Entity in Add_SchedulleList)
                {
                    if (string.IsNullOrEmpty(Entity.ATTM_SITE.Trim()) && string.IsNullOrEmpty(Entity.ATTM_ROOM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_AMPM.Trim()) && string.IsNullOrEmpty(Entity.ATTM_FUND.Trim()))
                    {
                        string Cal_Month = Entity.ATTM_MONTH.Trim();
                        //Cal_Month = Cal_Month.Length == 1 ? "0" + Cal_Month : Cal_Month;
                        if (Cal_Month.Trim() + Entity.ATTM_CALENDER_YEAR.Trim() == Month.Trim() + ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim())
                            Added_SchID = Entity.ATTM_ID.Trim();
                        //else
                        //    Added_SchID = string.Empty;
                    }
                }
            }
        }

        private void FillMonths()
        {
            this.cmbMonth.SelectedIndexChanged -= new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            List<ListItem> listItem = new List<ListItem>();
            listItem.Clear();
            //if (cmbMonth.Items.Count > 0)
            //    cmbMonth.DataSource = listItem;
            cmbMonth.Items.Clear();
            ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(txtHierarchy.Text.Substring(0, 2).Trim(), txtHierarchy.Text.Substring(3, 2).Trim(), txtHierarchy.Text.Substring(6, 2).Trim());
            StartMonth = int.Parse(programDefinitionList.StartMonth.Trim());
            int LMonth = int.Parse(programDefinitionList.EndMonth.Trim());
            List<AGYTABSEntity> Months = new List<AGYTABSEntity>();
            AGYTABSEntity Search_Months = new AGYTABSEntity(true);
            Search_Months.Tabs_Type = "SMONT"; string Cal_Month = "01";
            Months = _model.AdhocData.Browse_AGYTABS(Search_Months);
            int k = 0; k = StartMonth - 1;
            fillschdulemonth();
            if (Months.Count > 0)
            {
                int Cal_year = int.Parse(Year);
                listItem.Add(new ListItem("  ", "0", "0", string.Empty));
                for (int i = 0; i < 13; i++)
                {
                    Cal_Month = (k + 1).ToString();
                    Cal_Month = Cal_Month.Length == 1 ? "0" + Cal_Month : Cal_Month;
                    Cal_Month = Cal_Month + Cal_year;
                    if (rbFund.Checked)
                    {
                        if (list_schdeluedMonths.Contains(Cal_Month))
                            listItem.Add(new ListItem(Months[k].Code_Desc.Trim(), Months[k].Table_Code.Trim(), Cal_year.ToString(), string.Empty));
                    }
                    else
                    {
                        if (!list_schdeluedMonths.Contains(Cal_Month))
                        {
                            listItem.Add(new ListItem(Months[k].Code_Desc.Trim(), Months[k].Table_Code.Trim(), Cal_year.ToString(), string.Empty));
                            //if (i == (LMonth - 1))
                            //    break;
                        }
                    }
                    k++;
                    if (k >= 12)
                    {
                        k = k - 12;
                        Cal_year = Cal_year + 1;
                    }
                }
            }
            cmbMonth.Items.AddRange(listItem.ToArray());
            cmbMonth.SelectedIndex = 0;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
        }

        string Edit_Month = string.Empty; string Edit_ID = string.Empty;
        private void fillSiteSch_Controls()
        {
            string Temp_Cal_Month = "01";
            List<SiteScheduleEntity> scheduleList = new List<SiteScheduleEntity>();
            SiteScheduleEntity Search_Schd = new SiteScheduleEntity(true);
            Search_Schd.ATTM_AGENCY = Agency.Trim();
            Search_Schd.ATTM_DEPT = Dept.Trim(); Search_Schd.ATTM_PROG = Prog.Trim();
            Search_Schd.ATTM_YEAR = Year.Trim(); Search_Schd.ATTM_MONTH = Month_No.Trim();
            Search_Schd.ATTM_SITE = Site.Trim(); Search_Schd.ATTM_CALENDER_YEAR = Calander_Year.Trim();
            Search_Schd.ATTM_ID = Attm_ID.Trim();
            if (strSch_Type == "Room")
            {
                Search_Schd.ATTM_ROOM = Room.Trim();
                Search_Schd.ATTM_AMPM = AMPM.Trim();
            }
            else //if (strSch_Type == "Site")
            {
                Search_Schd.ATTM_ROOM = "    ";
                Search_Schd.ATTM_AMPM = " ";
            }
            if (!string.IsNullOrEmpty(Fund_Source.Trim()))
                Search_Schd.ATTM_FUND = Fund_Source.Trim();
            scheduleList = _model.SPAdminData.Browse_CHILDATTM(Search_Schd, "Browse");
            if (scheduleList.Count > 0)
            {

                txtSite.Text = scheduleList[0].ATTM_SITE.Trim();
                txtRoom.Text = scheduleList[0].ATTM_ROOM.Trim();
                txtAmPm.Text = scheduleList[0].ATTM_AMPM.Trim();
                this.rbFund.CheckedChanged -= new System.EventHandler(this.rbFund_CheckedChanged);
                this.rbMaster.CheckedChanged -= new System.EventHandler(this.rbMaster_CheckedChanged_1);
                this.rbSite.CheckedChanged -= new System.EventHandler(this.rbSite_CheckedChanged);
                this.rbRoom.CheckedChanged -= new System.EventHandler(this.rbRoom_CheckedChanged);
                if (strSch_Type == "Room") rbRoom.Checked = true;
                else if (strSch_Type == "Site") rbSite.Checked = true;
                else if (strSch_Type == "Master" && string.IsNullOrEmpty(scheduleList[0].ATTM_FUND.ToString().Trim())) rbMaster.Checked = true;
                else if (strSch_Type == "Master" && !string.IsNullOrEmpty(scheduleList[0].ATTM_FUND.ToString().Trim())) rbFund.Checked = true;
                this.rbFund.CheckedChanged += new System.EventHandler(this.rbFund_CheckedChanged);
                this.rbMaster.CheckedChanged += new System.EventHandler(this.rbMaster_CheckedChanged_1);
                this.rbSite.CheckedChanged += new System.EventHandler(this.rbSite_CheckedChanged);
                this.rbRoom.CheckedChanged += new System.EventHandler(this.rbRoom_CheckedChanged);

                if (!string.IsNullOrEmpty(scheduleList[0].ATTM_FUND.ToString().Trim()))
                {
                    FillFundCombo();
                    CommonFunctions.SetComboBoxValue(cmbFund, scheduleList[0].ATTM_FUND.ToString().Trim());
                }
                if (!string.IsNullOrEmpty(scheduleList[0].ATTM_MONTH.ToString().Trim()))
                {
                    Temp_Cal_Month = scheduleList[0].ATTM_MONTH.ToString();
                    //Temp_Cal_Month = Temp_Cal_Month.Length == 1 ? "0" + Temp_Cal_Month : Temp_Cal_Month;
                    //FillMonths();
                    CommonFunctions.SetComboBoxValue(cmbMonth, scheduleList[0].ATTM_MONTH.ToString());
                }
                Edit_Month = scheduleList[0].ATTM_MONTH.ToString().Trim();
                Edit_ID = scheduleList[0].ATTM_ID.ToString().Trim();
                lblCalenderYear.Text = scheduleList[0].ATTM_CALENDER_YEAR.Trim();

                lblSchedule.Visible = lblMonth.Visible = lblCalenderYear.Visible = lblFundSource.Visible = true;
                if (rbRoom.Checked)
                {
                    //lblSchedule.Visible = lblMonth.Visible = lblCalenderYear.Visible = lblFundSource.Visible = true;
                    lblSchedule.Text = "ROOM SCHEDULE";
                    lblMonth.Text = GetMonth(Edit_Month);
                    if (chkbFund.Checked)
                        lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                    else
                        lblFundSource.Text = " ";
                    //if (int.Parse(Edit_Month) > StartMonth)
                    //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                    //else
                    //{
                    //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                    //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                    //}
                }
                else if (rbSite.Checked)
                {

                    lblSchedule.Text = "SITE SCHEDULE";
                    lblMonth.Text = GetMonth(Edit_Month);
                    if (chkbFund.Checked)
                        lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                    else
                        lblFundSource.Text = " ";
                    //if (int.Parse(Edit_Month) > StartMonth)
                    //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                    //else
                    //{
                    //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                    //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                    //}
                }
                else if (rbFund.Checked)
                {
                    lblSchedule.Text = "MASTER SCHEDULE";
                    lblMonth.Text = GetMonth(Edit_Month);
                    if (((ListItem)cmbFund.SelectedItem).Value.ToString().Trim() != "0")
                        lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                    else
                        lblFundSource.Text = " ";
                    //if (int.Parse(Edit_Month) > StartMonth)
                    //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                    //else
                    //{
                    //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                    //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                    //}
                }
                else
                {
                    lblSchedule.Text = "MASTER SCHEDULE";
                    lblMonth.Text = GetMonth(Edit_Month);
                    lblFundSource.Text = " ";
                    //if (int.Parse(Edit_Month) > StartMonth)
                    //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                    //else
                    //{
                    //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                    //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                    //}
                }
            }

            gvDays.Rows.Clear();
            fillspacegrid();
            Month = Month_No.Trim();
            fillGvDays();
            //fillgvdaysData();
        }

        private void EnableDisableControls()
        {
            txtHierarchy.Enabled = false;
            txtHierachydesc.Enabled = false;
            rbFund.Enabled = false; rbMaster.Enabled = false; rbRoom.Enabled = false; rbSite.Enabled = false;
            txtSite.Enabled = false; btnSite.Enabled = false; txtRoom.Enabled = false; txtAmPm.Enabled = false;
            cmbFund.Enabled = false; cmbMonth.Enabled = false;
            gvDays.Enabled = false;
            btnSave.Visible = false; btnCancel.Visible = false;
        }

        private string GetMonth(String month)
        {
            string month_name = null;
            switch (month)
            {
                case "1": month_name = "January"; break;
                case "2": month_name = "February"; break;
                case "3": month_name = "March"; break;
                case "4": month_name = "April"; break;
                case "5": month_name = "May"; break;
                case "6": month_name = "June"; break;
                case "7": month_name = "July"; break;
                case "8": month_name = "August"; break;
                case "9": month_name = "September"; break;
                case "10": month_name = "October"; break;
                case "11": month_name = "November"; break;
                case "12": month_name = "December"; break;
            }
            return month_name;
        }



        string TempYear = string.Empty; string Month = string.Empty;
        private void fillspacegrid()
        {
            for (int i = 0; i < 6; i++)
            {
                gvDays.Rows.Add(" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ");
                TagFilling();
            }
        }

        private void TagFilling()
        {
            foreach (DataGridViewRow dr in gvDays.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    //if(dc.Value==" ")
                    //    gvDays.CurrentCell.Tag = " ";
                    dc.Tag = string.Empty;
                }
            }
        }


        //int Global_Index = 0;
        private void fillGvDays()
        {

            TempYear = lblTxtYear.Text;
            if (Month != "0")
            {
                DateTime firstday = new DateTime(Convert.ToInt32(lblCalenderYear.Text), Convert.ToInt32(Month), 1);
                string day = firstday.DayOfWeek.ToString();
                int rowIndex = 0; int k = 1; string Cal_Date = "01";
                if (day == DayOfWeek.Monday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }//rowIndex = 0;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 70)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
                else if (day == DayOfWeek.Tuesday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }//rowIndex = 1;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 70)
                        {
                            //int k = 1;
                            if (rowIndex == 0)
                            {
                                for (int i = 2; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;

                        }
                    }
                }
                else if (day == DayOfWeek.Wednesday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    } //rowIndex = 2;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 70)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 4; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
                else if (day == DayOfWeek.Thursday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }//rowIndex = 2;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 70)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 6; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
                else if (day == DayOfWeek.Friday.ToString())
                {
                    int j = 1;
                    if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }
                    //rowIndex = 2;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 76)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 8; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {

                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
                else if (day == DayOfWeek.Saturday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }//rowIndex = 2;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 76)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 10; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
                else if (day == DayOfWeek.Sunday.ToString())
                {
                    int j = 1; if (Mode == "Add")
                    {
                        Addschdule_List();
                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                            Child_List.Clear();
                    }//rowIndex = 2;
                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        if (j < 76)
                        {
                            if (rowIndex == 0)
                            {
                                for (int i = 12; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); 
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    i += 2; k++;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < 14;)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    gvDays.Rows[rowIndex].Cells[i].Value = Cal_Date.Trim();
                                    gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.AntiqueWhite;
                                    if (Mode == "Edit" || Mode == "View")
                                    {
                                        fillgvdaysData();
                                        if (controlList.Count > 0)
                                        {
                                            foreach (ChildATTMSEntity Entity in controlList)
                                            {
                                                if (Entity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                {
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(Entity.ATTMS_STATUS.Trim());
                                                    gvDays.Rows[rowIndex].Cells[i + 1].Tag = Entity.ATTMS_STATUS.Trim(); gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Mode == "Add")
                                    {
                                        if (!string.IsNullOrEmpty(Added_SchID.Trim()))
                                        {
                                            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
                                            searchId.ATTMS_ID = Added_SchID.Trim();
                                            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

                                            if (controlList.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in controlList)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //gvDays.Rows[rowIndex].Cells[i + 1].Tag = "Menu";
                                            if (Child_List.Count > 0)
                                            {
                                                foreach (ChildATTMSEntity ChEntity in Child_List)
                                                {
                                                    if (ChEntity.ATTMS_DAY.Trim() == Cal_Date.Trim())
                                                    {
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Value = GetStatus(ChEntity.ATTMS_STATUS.Trim());
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Tag = ChEntity.ATTMS_STATUS.Trim();
                                                        gvDays.Rows[rowIndex].Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
                                                        ChEntity.IsDatafill = "Y";
                                                        //Child_List.Add(new ChildATTMSEntity(ChEntity));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    k++;
                                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                                    {
                                        if (k > 32)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                                    {
                                        if (k > 31)
                                        {
                                            gvDays.Rows[rowIndex].Cells[i].Value = "";
                                            gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                            gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (IsLeapYear(int.Parse(lblCalenderYear.Text.Trim())))
                                        {
                                            if (k > 30)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            if (k > 29)
                                            {
                                                gvDays.Rows[rowIndex].Cells[i].Value = "";
                                                gvDays.Rows[rowIndex].Cells[i].Style.BackColor = System.Drawing.Color.White;
                                                gvDays.Rows[rowIndex].Cells[i + 1].Tag = string.Empty;
                                            }
                                        }
                                    }
                                    i += 2;
                                }
                            }
                            j += 14; rowIndex++;
                        }
                    }
                }
            }
        }


        public static bool IsLeapYear(int year)
        {
            if (year % 4 != 0)
            {
                return false;
            }
            if (year % 100 == 0)
            {
                return (year % 400 == 0);
            }
            return true;
        }

        List<ChildATTMSEntity> controlList = new List<ChildATTMSEntity>();
        private void fillgvdaysData()
        {
            List<SiteScheduleEntity> scheduleList = new List<SiteScheduleEntity>();
            SiteScheduleEntity Search_Schd = new SiteScheduleEntity(true);
            Search_Schd.ATTM_AGENCY = Agency.Trim();
            Search_Schd.ATTM_DEPT = Dept.Trim(); Search_Schd.ATTM_PROG = Prog.Trim();
            Search_Schd.ATTM_YEAR = Year.Trim(); Search_Schd.ATTM_MONTH = Month.Trim();
            Search_Schd.ATTM_SITE = Site.Trim(); Search_Schd.ATTM_ID = Attm_ID.Trim();
            if (strSch_Type == "Room")
            {
                Search_Schd.ATTM_ROOM = Room.Trim();
                Search_Schd.ATTM_AMPM = AMPM.Trim();
            }
            else //if (strSch_Type == "Site")
            {
                Search_Schd.ATTM_ROOM = "    ";
                Search_Schd.ATTM_AMPM = " ";
            }
            Search_Schd.ATTM_FUND = Fund_Source.Trim();
            Search_Schd.ATTM_CALENDER_YEAR = Calander_Year.Trim();
            scheduleList = _model.SPAdminData.Browse_CHILDATTM(Search_Schd, "Browse");
            string Attm_ID1 = scheduleList[0].ATTM_ID.Trim();

            ChildATTMSEntity searchId = new ChildATTMSEntity(true);
            searchId.ATTMS_ID = Attm_ID1.Trim();
            controlList = _model.SPAdminData.Browse_CHILDATTMS(searchId, "Browse");

        }

        private string GetStatus(string Status)
        {
            string Status_name = null;
            //switch (Status)
            //{
            //    case "O": Status_name = "OPEN"; break;
            //    case "C": Status_name = "CLOSED"; break;
            //    case "H": Status_name = "HOLIDAY"; break;
            //    case "P": Status_name = "PROFESSIONAL DAY"; break;
            //    case "W": Status_name = "WEATHER"; break;
            //    case "B": Status_name = "BUILDING MAINT."; break;

            //}
            GetHeadstartTemplate_Values();
            if (Template_List.Count > 0)
            {
                string Agy_2 = string.Empty;
                foreach (Headstart_Template Entity in Template_List)
                {
                    if (Entity.Code_Desc_Tag.Trim() == Status.Trim())
                    {
                        if (string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                            Agy_2 = string.Empty;
                        else
                            Agy_2 = " (" + Entity.Agy_2.Trim() + ")";
                        Status_name = Entity.Code_Desc + Agy_2;
                        break;
                    }
                }
            }

            return Status_name;
        }

        List<Headstart_Template> Template_List = new List<Headstart_Template>();
        private void GetHeadstartTemplate_Values()
        {
            Template_List = _model.CaseMstData.GetHeadstartTemplate(Consts.AgyTab.HSCALENDARDAYSTATS, "00000");
        }


        private void contextDayStrip_Popup(object sender, EventArgs e)
        {
            contextDayStrip.MenuItems.Clear();
            if (gvDays.Rows.Count > 0)
            {
                foreach (DataGridViewCell dr in gvDays.SelectedCells)
                {
                    if (dr.Selected)
                    {
                        if (!string.IsNullOrEmpty(dr.Tag.ToString().Trim()))
                        {
                            GetHeadstartTemplate_Values();
                            if (Template_List.Count > 0)
                            {
                                foreach (Headstart_Template Entity in Template_List)
                                {
                                    MenuItem menuItem = new MenuItem();
                                    menuItem.Text = Entity.Code_Desc;
                                    menuItem.Tag = Entity.Code_Desc_Tag;
                                    contextDayStrip.MenuItems.Add(menuItem);
                                }
                            }
                            //MenuItem menuItem = new MenuItem();
                            //menuItem.Text = "OPEN";
                            //menuItem.Tag = "O";
                            //contextDayStrip.MenuItems.Add(menuItem);
                            //MenuItem menuItem1 = new MenuItem();
                            //menuItem1.Text = "CLOSED";
                            //menuItem1.Tag = "C";
                            //contextDayStrip.MenuItems.Add(menuItem1);
                            //MenuItem menuItem2 = new MenuItem();
                            //menuItem2.Text = "HOLIDAY";
                            //menuItem2.Tag = "H";
                            //contextDayStrip.MenuItems.Add(menuItem2);
                            //MenuItem menuItem3 = new MenuItem();
                            //menuItem3.Text = "PROFESSIONAL DAY";
                            //menuItem3.Tag = "P";
                            //contextDayStrip.MenuItems.Add(menuItem3);
                            //MenuItem menuItem4 = new MenuItem();
                            //menuItem4.Text = "WEATHER";
                            //menuItem4.Tag = "W";
                            //contextDayStrip.MenuItems.Add(menuItem4);
                            //MenuItem menuItem5 = new MenuItem();
                            //menuItem5.Text = "BUILDING MAINT.";
                            //menuItem5.Tag = "B";
                            //contextDayStrip.MenuItems.Add(menuItem5);
                        }
                    }
                }
            }
        }

        private void gvDays_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            int columnIndex = gvDays.CurrentCell.ColumnIndex;
            int rowIndex = gvDays.CurrentCell.RowIndex;
            string AGY_2 = string.Empty;
            GetHeadstartTemplate_Values();
            if (Template_List.Count > 0)
            {
                foreach (Headstart_Template Entity in Template_List)
                {
                    if (Entity.Code_Desc_Tag.Trim() == objArgs.MenuItem.Tag.ToString().Trim())
                    {
                        if (string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                            AGY_2 = string.Empty;
                        else
                            AGY_2 = " (" + Entity.Agy_2.Trim() + ")";
                        break;
                    }
                }
            }
            //if (objArgs.MenuItem.Tag.Equals("C"))
            //{

            if (columnIndex == 1)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 3)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 5)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 7)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 9)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 11)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            else if (columnIndex == 13)
            {
                gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
                gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
                foreach (ChildATTMSEntity Entity in Child_List)
                {
                    if (gvDays.Rows[rowIndex].Cells[columnIndex - 1].Value.ToString().Trim() == Entity.ATTMS_DAY.Trim())
                    {
                        Entity.IsDatafill = "Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
                    }
                }
            }
            //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = objArgs.MenuItem.Tag;
            //}
            //if (objArgs.MenuItem.Tag.Equals("O"))
            //{
            //    if (columnIndex == 0)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 4)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 6)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 8)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 10)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 12)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    //gvDays.SelectedRows[0].Cells["Condition"].Value = objArgs.MenuItem.Text;
            //    //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = objArgs.MenuItem.Tag;
            //}
            //if (objArgs.MenuItem.Tag.Equals("H"))
            //{
            //    if (columnIndex == 0)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 4)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 6)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 8)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 10)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 12)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    //gvDays.SelectedRows[0].Cells["Condition"].Value = string.Empty;
            //    //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = string.Empty;
            //}
            //if (objArgs.MenuItem.Tag.Equals("P"))
            //{
            //    if (columnIndex == 0)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 4)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 6)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 8)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 10)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 12)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    //gvDays.SelectedRows[0].Cells["Condition"].Value = string.Empty;
            //    //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = string.Empty;
            //}
            //if (objArgs.MenuItem.Tag.Equals("W"))
            //{
            //    if (columnIndex == 0)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 4)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 6)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 8)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 10)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 12)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    //gvDays.SelectedRows[0].Cells["Condition"].Value = string.Empty;
            //    //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = string.Empty;
            //}
            //if (objArgs.MenuItem.Tag.Equals("B"))
            //{
            //    if (columnIndex == 0)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 4)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 6)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 8)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 10)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    else if (columnIndex == 12)
            //    {
            //        gvDays.CurrentCell.Value = objArgs.MenuItem.Text + AGY_2;
            //        gvDays.CurrentCell.Tag = objArgs.MenuItem.Tag;
            //        foreach(ChildATTMSEntity Entity in Child_List)
            //        {
            //            if(gvDays.Rows[rowIndex].Cells[columnIndex+1].Value.ToString().Trim()==Entity.ATTMS_DAY.Trim())
            //            {
            //                Entity.IsDatafill="Y";//Entity.ATTMS_STATUS=gvDays.CurrentCell.Tag;
            //            }
            //        }
            //    }
            //    //gvDays.SelectedRows[0].Cells["Condition"].Value = string.Empty;
            //    //gvDays.SelectedRows[0].Cells["ConditionValue"].Value = string.Empty;
            //}
        }

        private void txtAmPm_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRoom.Text.Trim()))
            {
                CaseSiteEntity Search_Site = new CaseSiteEntity(true);
                Search_Site.SiteAGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
                Search_Site.SiteDEPT = txtHierarchy.Text.Substring(3, 2).Trim();
                Search_Site.SitePROG = txtHierarchy.Text.Substring(6, 2).Trim();
                Search_Site.SiteYEAR = lblTxtYear.Text.Trim();
                Search_Site.SiteNUMBER = txtSite.Text.Trim(); Search_Site.SiteROOM = txtRoom.Text.Trim();
                Search_Site.SiteAM_PM = txtAmPm.Text.Trim();
                Check_Site = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");
                if (Check_Site.Count > 0)
                {
                    chkbFund.Focus();
                    _errorProvider.SetError(txtRoom, null);
                    chkbFund.Visible = true; cmbMonth.Enabled = true; label2.Visible = true;
                    FillMonths();
                }
                else
                {
                    this.txtAmPm.Leave -= new System.EventHandler(this.txtAmPm_Leave);
                    txtRoom.Clear(); txtAmPm.Clear();
                    txtRoom.Focus();
                    chkbFund.Visible = false; cmbMonth.Enabled = false; label2.Visible = false;
                    this.txtAmPm.Leave += new System.EventHandler(this.txtAmPm_Leave);
                    _errorProvider.SetError(txtRoom, string.Format("Not a Valid Room".Replace(Consts.Common.Colon, string.Empty)));
                    _errorProvider.SetError(txtAmPm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmpm.Text.Replace(Consts.Common.Colon, string.Empty)));
                }
            }
            else
            {
                txtAmPm.Clear();
                txtRoom.Focus();
                _errorProvider.SetError(txtRoom, string.Format("Not a Valid Room".Replace(Consts.Common.Colon, string.Empty)));
            }
        }

        private void chkbFund_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbFund.Checked)
            {
                cmbFund.Enabled = true;
                label1.Visible = true;
            }
            else
            {
                cmbFund.Enabled = false;
                label1.Visible = false;
                _errorProvider.SetError(cmbFund, null);
                lblMonth.Visible = false; lblCalenderYear.Visible = lblSchedule.Visible = false;
                //cmbMonth.Enabled = false;
                //label2.Visible = false;
            }
            FillFundCombo();
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionForm addForm = new HierarchieSelectionForm(BaseForm, "Master", string.Empty, string.Empty, "Casedep", string.Empty);
            //addForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            //addForm.ShowDialog();

            HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", string.Empty, "D", string.Empty);
            addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            addForm.StartPosition = FormStartPosition.CenterScreen;
            addForm.ShowDialog();
        }

        string SelAgency = string.Empty, SelDept = string.Empty, SelProg = string.Empty; int StartMonth = 0;
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    if (Year == "    ")
                    {
                        txtHierarchy.Text = row.Agency + "-" + "**" + "-" + "**";
                        HierarchyEntity hierachysubEntity = hierarchyEntity.Find(u => u.Code.Equals(txtHierarchy.Text.Trim()));
                        if (hierachysubEntity != null)
                        {
                            txtHierachydesc.Text = hierachysubEntity.HirarchyName;
                        }
                    }
                    else
                    {

                        txtHierarchy.Text = row.Code;
                        txtHierachydesc.Text = row.HirarchyName;

                    }
                    HiearchyCode = row.Agency + row.Dept + row.Prog;
                }
                ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(HiearchyCode.Substring(0, 2).Trim(), HiearchyCode.Substring(2, 2).Trim(), HiearchyCode.Substring(4, 2).Trim());
                if (!string.IsNullOrEmpty(programDefinitionList.DepYear.Trim()))
                {
                    lblTxtYear.Text = programDefinitionList.DepYear.Trim(); Year = programDefinitionList.DepYear.Trim();
                    if (string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) || string.IsNullOrEmpty(programDefinitionList.EndMonth.Trim()))
                    {
                        AlertBox.Show("Start Month and End Month should not be blank in Program Definition", MessageBoxIcon.Warning);
                        panel2.Enabled = panel3.Enabled = panel4.Enabled = panel5.Enabled = false; btnSave.Visible = false; btnCancel.Visible = true;
                        _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtRoom, null); _errorProvider.SetError(txtAmPm, null);
                        _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null); rbRoom.Checked = true;
                    }
                    else
                    {
                        panel2.Enabled = panel3.Enabled = panel4.Enabled = panel5.Enabled = true; rbRoom.Checked = true;
                        lblTxtYear.Visible = lblYear.Visible = true; btnSave.Visible = true; btnCancel.Visible = true;
                        _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtRoom, null); _errorProvider.SetError(txtAmPm, null);
                        _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
                        StartMonth = int.Parse(programDefinitionList.StartMonth.Trim());
                    }
                }
                else
                {
                    AlertBox.Show("Year should not be blank for this hierarchy in Program Definition", MessageBoxIcon.Warning);
                    panel2.Enabled = panel3.Enabled = panel4.Enabled = panel5.Enabled = false; rbRoom.Checked = true;
                    lblTxtYear.Visible = lblYear.Visible = false; btnSave.Visible = false; btnCancel.Visible = true;
                    _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtRoom, null); _errorProvider.SetError(txtAmPm, null);
                    _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
                }
            }

        }

        private void cmbFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMonth.Enabled = true;
            if (Mode == "Add")
                label2.Visible = true;
            lblFundSource.Visible = true;
            lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
            lblMonth.Visible = false; lblCalenderYear.Visible = lblSchedule.Visible = false;
            FillMonths();
            gvDays.Rows.Clear();
        }
        List<ChildATTMSEntity> Child_List = new List<ChildATTMSEntity>();
        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Month = ((ListItem)cmbMonth.SelectedItem).Value.ToString().Trim();

            int temp_month = int.Parse(((ListItem)cmbMonth.SelectedItem).Value.ToString().Trim());
            Month = temp_month.ToString(); bool IsExists = true;


            if (Mode == "Add")
            {
                if (rbFund.Checked.Equals(true))
                {
                    List<SiteScheduleEntity> searchlist = new List<SiteScheduleEntity>();
                    SiteScheduleEntity Search_Fund = new SiteScheduleEntity(true);
                    Search_Fund.ATTM_AGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
                    Search_Fund.ATTM_DEPT = txtHierarchy.Text.Substring(3, 2).Trim();
                    Search_Fund.ATTM_PROG = txtHierarchy.Text.Substring(6, 2).Trim();
                    Search_Fund.ATTM_YEAR = lblTxtYear.Text.Trim(); Search_Fund.ATTM_MONTH = Month.Trim();
                    Search_Fund.ATTM_FUND = ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim();
                    Search_Fund.ATTM_CALENDER_YEAR = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();
                    searchlist = _model.SPAdminData.Browse_CHILDATTM(Search_Fund, "Browse");
                    if (searchlist.Count > 0)
                    {
                        AlertBox.Show("Schedule already exists for Funding Source and Month", MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Child_List.Clear();
                        ChildATTMSEntity Entity = new ChildATTMSEntity(true);
                        string Cal_Date = "01";
                        if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                        {
                            for (int k = 1; k < 32; k++)
                            {
                                Cal_Date = k.ToString();
                                Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                Child_List.Add(new ChildATTMSEntity(Entity));
                            }

                        }
                        else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                        {
                            for (int k = 1; k < 31; k++)
                            {
                                Cal_Date = k.ToString();
                                Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                Child_List.Add(new ChildATTMSEntity(Entity));
                            }
                        }
                        else
                        {
                            if (IsLeapYear(int.Parse(((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim())))
                            {
                                for (int k = 1; k < 30; k++)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                    Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                    Child_List.Add(new ChildATTMSEntity(Entity));
                                }
                            }
                            else
                            {
                                for (int k = 1; k < 29; k++)
                                {
                                    Cal_Date = k.ToString();
                                    Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                    Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                    Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                    Child_List.Add(new ChildATTMSEntity(Entity));
                                }
                            }
                        }

                        lblSchedule.Visible = lblMonth.Visible = lblCalenderYear.Visible = lblFundSource.Visible = true;
                        lblSchedule.Text = "MASTER SCHEDULE";
                        lblMonth.Text = ((ListItem)cmbMonth.SelectedItem).Text.ToString().Trim();
                        //if (chkbFund.Checked)
                        lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                        //else
                        //lblFundSource.Text = " ";
                        //if (temp_month >= StartMonth)
                        //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                        //else
                        //{
                        //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                        //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                        //}
                        lblCalenderYear.Text = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();


                        gvDays.Rows.Clear();
                        fillspacegrid();
                        fillGvDays();
                    }
                }
                else
                {
                    if ((!rbMaster.Checked) && (!rbFund.Checked))
                    {
                        List<SiteScheduleEntity> Masterlist = new List<SiteScheduleEntity>();
                        SiteScheduleEntity Search_master = new SiteScheduleEntity(true);
                        Search_master.ATTM_AGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
                        Search_master.ATTM_DEPT = txtHierarchy.Text.Substring(3, 2).Trim();
                        Search_master.ATTM_PROG = txtHierarchy.Text.Substring(6, 2).Trim();
                        Search_master.ATTM_YEAR = lblTxtYear.Text.Trim(); Search_master.ATTM_MONTH = Month.Trim();
                        Search_master.ATTM_CALENDER_YEAR = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();
                        Masterlist = _model.SPAdminData.Browse_CHILDATTM(Search_master, "Browse");
                        if (Masterlist.Count > 0)
                        {
                            if (chkbFund.Checked)
                            {
                                foreach (SiteScheduleEntity SiteEntity in Masterlist)
                                {
                                    if (SiteEntity.ATTM_FUND.Trim() == ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim())
                                    {
                                        Month = temp_month.ToString();
                                        IsExists = true;
                                        break;
                                    }
                                    else
                                    {
                                        IsExists = false;
                                    }

                                }
                                if (!IsExists)
                                {
                                    if (Month != "0")
                                    {
                                        Month = "0";
                                        AlertBox.Show("Fund Master Schedule does not exist for this Month. You can't add a Schedule to the Funding Source", MessageBoxIcon.Warning);
                                        //btnSave.Visible = false; btnCancel.Visible = false;
                                    }
                                    else
                                    {
                                        //btnSave.Visible = true; btnCancel.Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Month != "0")
                            {
                                Month = "0";
                                AlertBox.Show("Master Schedule does not exist for this Month. You can't add a Schedule", MessageBoxIcon.Warning);
                            }
                        }
                    }
                    Child_List.Clear();
                    ChildATTMSEntity Entity = new ChildATTMSEntity(true);
                    string Cal_Date = "01";
                    if (Month == "1" || Month == "3" || Month == "5" || Month == "7" || Month == "8" || Month == "10" || Month == "12")
                    {
                        for (int k = 1; k < 32; k++)
                        {
                            Cal_Date = k.ToString();
                            Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                            Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                            //Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                            Entity.ATTMS_STATUS = "O"; Entity.IsDatafill = "Y";
                            Child_List.Add(new ChildATTMSEntity(Entity));
                        }

                    }
                    else if (Month == "4" || Month == "6" || Month == "9" || Month == "11")
                    {
                        for (int k = 1; k < 31; k++)
                        {
                            Cal_Date = k.ToString();
                            Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                            Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                            //Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                            Entity.ATTMS_STATUS = "O"; Entity.IsDatafill = "Y";
                            Child_List.Add(new ChildATTMSEntity(Entity));
                        }
                    }
                    else
                    {
                        if (IsLeapYear(int.Parse(((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim())))
                        {
                            for (int k = 1; k < 30; k++)
                            {
                                Cal_Date = k.ToString();
                                Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                //Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                Entity.ATTMS_STATUS = "O"; Entity.IsDatafill = "Y";
                                Child_List.Add(new ChildATTMSEntity(Entity));
                            }
                        }
                        else
                        {
                            for (int k = 1; k < 29; k++)
                            {
                                Cal_Date = k.ToString();
                                Cal_Date = Cal_Date.Length == 1 ? "0" + Cal_Date : Cal_Date;
                                Entity.ATTMS_ID = "0"; Entity.ATTMS_DAY = Cal_Date.Trim(); Entity.ATTMS_WEEK = string.Empty;
                                //Entity.ATTMS_STATUS = string.Empty; Entity.IsDatafill = "N";
                                Entity.ATTMS_STATUS = "O"; Entity.IsDatafill = "Y";
                                Child_List.Add(new ChildATTMSEntity(Entity));
                            }
                        }
                    }

                    lblSchedule.Visible = lblMonth.Visible = lblCalenderYear.Visible = lblFundSource.Visible = true;
                    if (rbRoom.Checked)
                    {
                        //lblSchedule.Visible = lblMonth.Visible = lblCalenderYear.Visible = lblFundSource.Visible = true;
                        lblSchedule.Text = "ROOM SCHEDULE";
                        lblMonth.Text = ((ListItem)cmbMonth.SelectedItem).Text.ToString().Trim();
                        if (chkbFund.Checked)
                            lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                        else
                            lblFundSource.Text = " ";
                        //if (temp_month >= StartMonth)
                        //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                        //else
                        //{
                        //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                        //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                        //}
                        lblCalenderYear.Text = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();
                    }
                    else if (rbSite.Checked)
                    {

                        lblSchedule.Text = "SITE SCHEDULE";
                        lblMonth.Text = ((ListItem)cmbMonth.SelectedItem).Text.ToString().Trim();
                        if (chkbFund.Checked)
                            lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                        else
                            lblFundSource.Text = " ";
                        //if (temp_month >= StartMonth)
                        //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                        //else
                        //{
                        //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                        //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                        //}
                        lblCalenderYear.Text = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();
                    }
                    //else if (rbFund.Checked)
                    //{
                    //    lblSchedule.Text = "MASTER SCHEDULE";
                    //    lblMonth.Text = ((ListItem)cmbMonth.SelectedItem).Text.ToString().Trim();
                    //    //if (chkbFund.Checked)
                    //    lblFundSource.Text = ((ListItem)cmbFund.SelectedItem).Text.ToString().Trim();
                    //    //else
                    //    //lblFundSource.Text = " ";
                    //    if (temp_month > StartMonth)
                    //        lblCalenderYear.Text = lblTxtYear.Text.Trim();
                    //    else
                    //    {
                    //        int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                    //        lblCalenderYear.Text = (Temp_Year + 1).ToString();
                    //    }
                    //}
                    else
                    {
                        lblSchedule.Text = "MASTER SCHEDULE";
                        lblMonth.Text = ((ListItem)cmbMonth.SelectedItem).Text.ToString().Trim();
                        lblFundSource.Text = " ";
                        //if (temp_month >= StartMonth)
                        //    lblCalenderYear.Text = lblTxtYear.Text.Trim();
                        //else
                        //{
                        //    int Temp_Year = int.Parse(lblTxtYear.Text.Trim());
                        //    lblCalenderYear.Text = (Temp_Year + 1).ToString();
                        //}
                        lblCalenderYear.Text = ((ListItem)cmbMonth.SelectedItem).ID.ToString().Trim();
                    }
                    gvDays.Rows.Clear();
                    fillspacegrid();
                    fillGvDays();
                }
            }
        }



        private void btnSite_Click(object sender, EventArgs e)
        {
            if (rbRoom.Checked)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", txtHierarchy.Text.Substring(0, 2).Trim(), txtHierarchy.Text.Substring(3, 2).Trim(), txtHierarchy.Text.Substring(6, 2).Trim(), lblTxtYear.Text, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(Site_AddForm_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
            else if (rbSite.Checked)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Site", txtHierarchy.Text.Substring(0, 2).Trim(), txtHierarchy.Text.Substring(3, 2).Trim(), txtHierarchy.Text.Substring(6, 2).Trim(), lblTxtYear.Text, Privileges);
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
                txtSite.Text = From_Results[0];
                txtRoom.Text = From_Results[1];
                txtAmPm.Text = From_Results[2];
                if (!string.IsNullOrEmpty(txtSite.Text))
                {
                    cmbMonth.Enabled = true;
                    chkbFund.Visible = true;
                    label2.Visible = true;
                }
                else
                    chkbFund.Visible = false;
                FillMonths();
            }
        }

        //object priv_sel_Option = string.Empty;
        private void rbMaster_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaptainModel Model = new CaptainModel();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;
                    string Attms_ID = string.Empty;
                    if (Mode == "Add")
                    {
                        SiteScheduleEntity Entity = new SiteScheduleEntity();

                        if (Mode == "Edit")
                            Entity.Row_Type = "U";
                        else
                            Entity.Row_Type = "I";
                        string Hierarchy_Code = txtHierarchy.Text.Trim();
                        Agency = Hierarchy_Code.Substring(0, 2).Trim();
                        Dept = Hierarchy_Code.Substring(3, 2).Trim();
                        Prog = Hierarchy_Code.Substring(6, 2).Trim();
                        Entity.ATTM_AGENCY = Agency;
                        Entity.ATTM_DEPT = Dept;
                        Entity.ATTM_PROG = Prog;
                        Entity.ATTM_YEAR = lblTxtYear.Text;
                        Entity.ATTM_MONTH = Month.Trim();
                        if (string.IsNullOrEmpty(txtSite.Text.Trim()))
                            txtSite.Text = "    ";
                        Entity.ATTM_SITE = txtSite.Text;
                        if (string.IsNullOrEmpty(txtRoom.Text.Trim()))
                            txtRoom.Text = "    ";
                        Entity.ATTM_ROOM = txtRoom.Text;
                        if (string.IsNullOrEmpty(txtAmPm.Text.Trim()))
                            txtAmPm.Text = " ";
                        Entity.ATTM_AMPM = txtAmPm.Text;
                        if (chkbFund.Checked)
                        {
                            if (((ListItem)cmbFund.SelectedItem).Value.ToString().Trim() == "0")
                                Entity.ATTM_FUND = " ";
                            else
                                Entity.ATTM_FUND = ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim();
                        }
                        else if (rbFund.Checked)
                            if (((ListItem)cmbFund.SelectedItem).Value.ToString().Trim() == "0")
                                Entity.ATTM_FUND = " ";
                            else
                                Entity.ATTM_FUND = ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim();
                        else
                            Entity.ATTM_FUND = " ";

                        Entity.ATTM_CALENDER_YEAR = lblCalenderYear.Text;
                        Entity.ATTM_ADD_OPERATOR = BaseForm.UserID;
                        Entity.ATTM_LSTC_OPERATOR = BaseForm.UserID;
                        int CuurentID = 0;
                        _model.SPAdminData.UpdateCHLDATTM(Entity, "Update", out msg, out SqlMsg, out CuurentID);
                        Attms_ID = CuurentID.ToString().Trim();
                    }
                    int rowIndex = 1; string Cal_Week = "01";

                    foreach (DataGridViewRow dr in gvDays.Rows)
                    {
                        Cal_Week = rowIndex.ToString();
                        Cal_Week = Cal_Week.Length == 1 ? "0" + Cal_Week : Cal_Week;
                        int i = 1; string first = string.Empty; string second = string.Empty;
                        foreach (DataGridViewCell dc in dr.Cells)
                        {
                            if (dc.Value != " ")
                            {
                                if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13)
                                    first = dc.Value.ToString().Trim();
                                else
                                    second = dc.Tag.ToString().Trim();
                                if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
                                {
                                    ChildATTMSEntity Child_Entity = new ChildATTMSEntity();
                                    if (Mode == "Edit")
                                    {
                                        Child_Entity.Row_Type = "U";
                                        Child_Entity.ATTMS_ID = Edit_ID.Trim();
                                    }
                                    else
                                    {
                                        Child_Entity.Row_Type = "I";
                                        Child_Entity.ATTMS_ID = Attms_ID.Trim();
                                    }
                                    Child_Entity.ATTMS_DAY = first.Trim();
                                    Child_Entity.ATTMS_STATUS = second.Trim();
                                    Child_Entity.ATTMS_WEEK = Cal_Week.Trim();
                                    _model.SPAdminData.UpdateCHLDATTMS(Child_Entity, "Update", out msg, out SqlMsg);
                                }
                            }
                            i++;
                        }
                        rowIndex++;
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (Mode == "Add")
            {
                if (string.IsNullOrEmpty(txtHierarchy.Text.Trim()))
                {
                    _errorProvider.SetError(PbHierarchies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierarchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtHierachydesc, null);
                }

                if (rbRoom.Checked.Equals(true) || rbSite.Checked.Equals(true))
                    if (string.IsNullOrEmpty(txtSite.Text) || string.IsNullOrWhiteSpace(txtSite.Text))
                    {
                        _errorProvider.SetError(btnSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(btnSite, null);

                if (rbRoom.Checked)
                {
                    if (string.IsNullOrEmpty(txtRoom.Text) || string.IsNullOrWhiteSpace(txtRoom.Text))
                    {
                        _errorProvider.SetError(txtRoom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRoom.Text.Replace(Consts.Common.Colon, string.Empty)));
                        txtAmPm.Clear();
                        //_errorProvider.SetError(txtAmPm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmpm.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtRoom, null);
                    if (string.IsNullOrEmpty(txtAmPm.Text) || string.IsNullOrWhiteSpace(txtAmPm.Text))
                    {
                        _errorProvider.SetError(txtAmPm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblAmpm.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtAmPm, null);
                    _errorProvider.SetError(cmbFund, null);
                    _errorProvider.SetError(cmbMonth, null);
                }
                if (chkbFund.Checked || rbFund.Checked.Equals(true))
                {
                    if (((ListItem)cmbFund.SelectedItem).Value.ToString() == "0")
                    {
                        _errorProvider.SetError(cmbFund, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFund.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(cmbFund, null);
                }
                else
                    _errorProvider.SetError(cmbFund, null);

                if (((ListItem)cmbMonth.SelectedItem).Value.ToString().Trim() == "0")
                {
                    if (rbRoom.Checked.Equals(true))
                    {
                        if (!string.IsNullOrEmpty(txtSite.Text.Trim()) && !string.IsNullOrEmpty(txtRoom.Text.Trim()) && !string.IsNullOrEmpty(txtAmPm.Text.Trim()))
                        {
                            _errorProvider.SetError(cmbMonth, "Month is Required".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                    }
                    else if (rbSite.Checked.Equals(true))
                    {
                        if (!string.IsNullOrEmpty(txtSite.Text.Trim()) && string.IsNullOrEmpty(txtRoom.Text.Trim()) && string.IsNullOrEmpty(txtAmPm.Text.Trim()))
                        {
                            _errorProvider.SetError(cmbMonth, "Month is Required".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                    }
                    else if (rbFund.Checked.Equals(true))
                    {
                        if (string.IsNullOrEmpty(txtSite.Text.Trim()) && string.IsNullOrEmpty(txtRoom.Text.Trim()) && string.IsNullOrEmpty(txtAmPm.Text.Trim()) && ((ListItem)cmbFund.SelectedItem).Value.ToString() != "0")
                        {
                            _errorProvider.SetError(cmbMonth, "Month is Required".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                    }
                    else if (rbMaster.Checked.Equals(true))
                    {
                        if (string.IsNullOrEmpty(txtSite.Text.Trim()) && string.IsNullOrEmpty(txtRoom.Text.Trim()) && string.IsNullOrEmpty(txtAmPm.Text.Trim()) && ((ListItem)cmbFund.SelectedItem).Value.ToString() == "0")
                        {
                            _errorProvider.SetError(cmbMonth, "Month is Required".Replace(Consts.Common.Colon, string.Empty));
                            isValid = false;
                        }
                    }

                }
                else
                    _errorProvider.SetError(cmbMonth, null);

                if (((ListItem)cmbMonth.SelectedItem).Value.ToString().Trim() != "0")
                {
                    if (Child_List.Count > 0)
                    {
                        foreach (ChildATTMSEntity Entity in Child_List)
                        {
                            if (Entity.IsDatafill == "N")
                            {
                                isValid = false;
                            }
                        }
                    }
                    if (gvDays.Rows.Count > 0)
                    {
                        if (!isValid)
                            AlertBox.Show("Days are Left Blank", MessageBoxIcon.Warning);
                    }
                    else
                        AlertBox.Show("All the Days are Left Blank", MessageBoxIcon.Warning);
                }



            }


            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_Schedule_Code()
        {
            string[] Added_Edited_ScheduleCode = new string[8];

            Added_Edited_ScheduleCode[0] = txtSite.Text;
            Added_Edited_ScheduleCode[1] = Mode;
            Added_Edited_ScheduleCode[2] = txtRoom.Text;
            Added_Edited_ScheduleCode[3] = txtAmPm.Text;
            Added_Edited_ScheduleCode[4] = txtHierarchy.Text;
            if (Month != null)
                Added_Edited_ScheduleCode[5] = Month;
            else
                Added_Edited_ScheduleCode[5] = "";
            if (cmbFund.Enabled == true)
            {
                if (((ListItem)cmbFund.SelectedItem).Value.ToString().Trim() != null)
                    Added_Edited_ScheduleCode[7] = ((ListItem)cmbFund.SelectedItem).Value.ToString().Trim();
                else
                    Added_Edited_ScheduleCode[7] = "";
            }
            else
                Added_Edited_ScheduleCode[7] = "";
            if (rbRoom.Checked)
                Added_Edited_ScheduleCode[6] = "Room";
            else if (rbSite.Checked)
                Added_Edited_ScheduleCode[6] = "Site";
            else if (rbMaster.Checked || rbFund.Checked)
                Added_Edited_ScheduleCode[6] = "Master";
            //Added_Edited_SiteCode[2] = ;

            return Added_Edited_ScheduleCode;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbRoom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRoom.Checked.Equals(true))
            {
                txtSite.Enabled = txtRoom.Enabled = txtAmPm.Enabled = btnSite.Enabled = true;
                chkbFund.Visible = false; cmbFund.Enabled = false; cmbMonth.Enabled = false; label1.Visible = false;
                lblAMPMReq.Visible = lblRoomReq.Visible = lblSiteReq.Visible = true; label2.Visible = false;
                txtSite.Text = txtRoom.Text = txtAmPm.Text = string.Empty; chkbFund.Checked = false;
                CommonFunctions.SetComboBoxValue(cmbFund, "0"); CommonFunctions.SetComboBoxValue(cmbMonth, "0");
                gvDays.Rows.Clear(); lblFundSource.Text = lblCalenderYear.Text = lblSchedule.Text = lblMonth.Text = string.Empty;
                _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtAmPm, null); _errorProvider.SetError(txtRoom, null);
                _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
            }
        }

        private void rbSite_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSite.Checked.Equals(true))
            {
                txtSite.Enabled = true; txtRoom.Enabled = false; txtAmPm.Enabled = false;
                lblAMPMReq.Visible = lblRoomReq.Visible = false; label1.Visible = false; label2.Visible = false;
                chkbFund.Visible = false; cmbFund.Enabled = false; btnSite.Enabled = true; chkbFund.Checked = false;
                cmbMonth.Enabled = false; txtSite.Text = txtRoom.Text = txtAmPm.Text = string.Empty;
                CommonFunctions.SetComboBoxValue(cmbFund, "0"); CommonFunctions.SetComboBoxValue(cmbMonth, "0");
                _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtAmPm, null); _errorProvider.SetError(txtRoom, null);
                _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
                gvDays.Rows.Clear(); lblFundSource.Text = lblCalenderYear.Text = lblSchedule.Text = lblMonth.Text = string.Empty;
            }
        }

        private void rbFund_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFund.Checked.Equals(true))
            {
                cmbFund.Enabled = true; label1.Visible = true; txtSite.Text = txtRoom.Text = txtAmPm.Text = string.Empty;
                lblAMPMReq.Visible = lblRoomReq.Visible = lblSiteReq.Visible = false; chkbFund.Visible = false;
                txtSite.Enabled = txtRoom.Enabled = txtAmPm.Enabled = btnSite.Enabled = false;
                FillFundCombo(); cmbMonth.Enabled = false; label2.Visible = false;
                CommonFunctions.SetComboBoxValue(cmbMonth, "0");
                gvDays.Rows.Clear(); lblFundSource.Text = lblCalenderYear.Text = lblSchedule.Text = lblMonth.Text = string.Empty;
                _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtAmPm, null); _errorProvider.SetError(txtRoom, null);
                _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
            }
        }

        private void rbMaster_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbMaster.Checked.Equals(true))
            {
                txtSite.Enabled = txtRoom.Enabled = txtAmPm.Enabled = btnSite.Enabled = false;
                chkbFund.Visible = false; cmbFund.Enabled = false; label1.Visible = false;
                gvDays.Rows.Clear(); lblFundSource.Text = lblCalenderYear.Text = lblSchedule.Text = lblMonth.Text = string.Empty;
                lblAMPMReq.Visible = lblRoomReq.Visible = lblSiteReq.Visible = false;
                cmbMonth.Enabled = true; txtSite.Text = txtRoom.Text = txtAmPm.Text = string.Empty; label2.Visible = true;
                _errorProvider.SetError(btnSite, null); _errorProvider.SetError(txtAmPm, null); _errorProvider.SetError(txtRoom, null);
                _errorProvider.SetError(cmbFund, null); _errorProvider.SetError(cmbMonth, null);
                CommonFunctions.SetComboBoxValue(cmbFund, "0");
                CommonFunctions.SetComboBoxValue(cmbMonth, "0");
                FillMonths();
            }
        }

        private void Siteschedule_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }

        List<CaseSiteEntity> Check_Site = new List<CaseSiteEntity>();
        private void txtSite_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSite.Text.Trim()))
            {
                CaseSiteEntity Search_Site = new CaseSiteEntity(true);
                Search_Site.SiteAGENCY = txtHierarchy.Text.Substring(0, 2).Trim();
                Search_Site.SiteDEPT = txtHierarchy.Text.Substring(3, 2).Trim();
                Search_Site.SitePROG = txtHierarchy.Text.Substring(6, 2).Trim();
                Search_Site.SiteYEAR = lblTxtYear.Text.Trim();
                Search_Site.SiteNUMBER = txtSite.Text.Trim();
                Check_Site = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");
                if (Check_Site.Count > 0)
                {
                    _errorProvider.SetError(btnSite, null);
                    txtRoom.Focus();
                    if (rbSite.Checked)
                        chkbFund.Visible = true;
                }
                else
                {
                    txtSite.Clear();
                    txtSite.Focus();
                    chkbFund.Visible = false;
                    _errorProvider.SetError(btnSite, string.Format("Not a Valid Site Number".Replace(Consts.Common.Colon, string.Empty)));
                }
            }
            else
                _errorProvider.SetError(btnSite, string.Format("Not a Valid Site Number".Replace(Consts.Common.Colon, string.Empty)));
        }







    }
}