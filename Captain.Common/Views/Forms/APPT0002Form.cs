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
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{

    public partial class APPT0002Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public APPT0002Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, string sitecode, string strDate, string strMonth, string strYear)
        {
            InitializeComponent();
            BaseForm = baseForm;
            M_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //M_HieDesc = hieDesc;
            M_Year = string.Empty;
            Mode = mode; dtBirth.Focus();
            SchSite = sitecode;
            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CAPC")
            {
                lblWorkerReq.Visible = false;
                lblStatusReq.Visible = false;
                lblLangReq.Visible = false;
            }
            if (strDate != string.Empty)
            {
                SchDate = strMonth + "/" + strDate + "/" + strYear;
                AppDate.Value = Convert.ToDateTime(strMonth + "/" + strDate + "/" + strYear);
            }
            else
            {
                var first = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), 1);
                AppDate.Value = first;

            }

            SchDate = AppDate.Value.ToShortDateString();
            //SchType = type;
            Privileges = privileges;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            Sel_SSN_Rec[0] = Sel_SSN_Rec[1] = "";
            Next_Day_Avail_Slot_Key[0] = Next_Day_Avail_Slot_Key[1] = "";

            //TxtHie.Text = hieDesc;
            // TxtYear.Text = year;
            FillDropDowns();

            PropApptschedule = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, "1");
            PropApptschedule = PropApptschedule.FindAll(u => u.SchdType.Equals("1"));

            propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHDHISTBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, SchSite, AppDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            if (propAPPtSchedulelist.Count > 0)
                btnHist.Visible = true;

            this.Text = privileges.PrivilegeName;
            //this.Size = new System.Drawing.Size(768, 598);
            //this.Lbl_Now_Sch.Location = new System.Drawing.Point(4, 449);
           // this.Lbl_LPB_Info.Location = new System.Drawing.Point(4, 449);
           // Lbl_LPB_Info.Visible = false;

            TxtZip.Validator = TextBoxValidation.IntegerValidator;
            TxtZip1.Validator = TextBoxValidation.IntegerValidator;

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Pb_Add, "Add Appointment");
            tooltip.SetToolTip(Pb_Edit, "Edit Appointment");
            tooltip.SetToolTip(Pb_Delete, "Delete Appointment");
            PrivApp_date = DateTime.MinValue;
        }

        public APPT0002Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, string sitecode, string strDate, string strMonth, string strYear, string Rec0, string Rec1)
        {
            InitializeComponent();
            BaseForm = baseForm;
            M_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //M_HieDesc = hieDesc;
            M_Year = string.Empty;
            Mode = mode;
            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CAPC")
            {
                lblWorkerReq.Visible = false;
                lblStatusReq.Visible = false;

                lblLangReq.Visible = false;
            }
            SchSite = sitecode;
            if (strDate != string.Empty)
            {
                SchDate = strMonth + "/" + strDate + "/" + strYear;
                AppDate.Value = Convert.ToDateTime(strMonth + "/" + strDate + "/" + strYear);
            }
            else
            {
                var first = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), 1);
                AppDate.Value = first;

            }

            Rec_Sel_From_SSN_Search = true;

            Sel_SSN_Rec[0] = Rec0;
            Sel_SSN_Rec[1] = Rec1;

            SchDate = AppDate.Value.ToShortDateString();
            //SchType = type;
            Privileges = privileges;

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            //Sel_SSN_Rec[0] = Sel_SSN_Rec[1] = "";
            Next_Day_Avail_Slot_Key[0] = Next_Day_Avail_Slot_Key[1] = "";

            //TxtHie.Text = hieDesc;
            // TxtYear.Text = year;
            FillDropDowns();

            PropApptschedule = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, "1");
            PropApptschedule = PropApptschedule.FindAll(u => u.SchdType.Equals("1"));

            this.Text = privileges.PrivilegeName;
            //this.Size = new System.Drawing.Size(768, 598);
            //this.Lbl_Now_Sch.Location = new System.Drawing.Point(4, 449);
            //this.Lbl_LPB_Info.Location = new System.Drawing.Point(4, 449);
           // Lbl_LPB_Info.Visible = false;

            TxtZip.Validator = TextBoxValidation.IntegerValidator;
            TxtZip1.Validator = TextBoxValidation.IntegerValidator;

            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Pb_Add, "Add Appointment");
            tooltip.SetToolTip(Pb_Edit, "Edit Appointment");
            tooltip.SetToolTip(Pb_Delete, "Delete Appointment");
            PrivApp_date = DateTime.MinValue;
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string M_Hierarchy { get; set; }

        //public string M_HieDesc { get; set; }

        public string M_Year { get; set; }

        public string SchSite { get; set; }

        public string SchDate { get; set; }

        public string SchType { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<APPTSCHEDULEEntity> PropApptschedule { get; set; }

        public List<CommonEntity> PropStatus { get; set; }

        public List<CommonEntity> PropClientStatus { get; set; }

        public List<APPTSCHDHISTEntity> propAPPtSchedulelist { get; set; }

        #endregion

        EventArgs e = new EventArgs();
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges.Program, "S");
            SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,BaseForm.BaseYear, Privileges, "ClientIntake", BaseForm);
            Select_site.FormClosed += new FormClosedEventHandler(OnSerachFormClosed);
            Select_site.StartPosition = FormStartPosition.CenterScreen;
            Select_site.ShowDialog();
        }

        string Priv_Sel_Site = "";
        private void OnSerachFormClosed(object sender, FormClosedEventArgs e)
        {
            SiteSearchForm form = sender as SiteSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                TxtSiteCode.Text = form.GetSelectedRowDetails();
                if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                    BtnNextSlot.Visible = true;
                else
                    BtnNextSlot.Visible = false;

                if (Priv_Sel_Site != TxtSiteCode.Text.Trim())
                    AppDate_LostFocus(AppDate, EventArgs.Empty);

                Priv_Sel_Site = TxtSiteCode.Text.Trim();
            }
        }


        private void FillDropDowns()
        {
            CmbGen.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", "0"));
            listItem.Add(new ListItem("Female", "F"));
            listItem.Add(new ListItem("Male", "M"));
            CmbGen.Items.AddRange(listItem.ToArray());

            CmbHtSrc.Items.Clear(); 
            CmbHtSrc.ColorMember = "FavoriteColor";
            List<CommonEntity> HeatingSources = _model.lookupDataAccess.Get_HearingSources();
            List<ListItem> listItem2 = new List<ListItem>();
            foreach (CommonEntity Entity in HeatingSources)
            {
                //if ((Mode == "Add" && Entity.Active.Trim() != "N")) || (Mode == "Edit"))
                listItem2.Add(new ListItem(Entity.Desc, Entity.Code.Trim(), Entity.Active.Trim(), (Entity.Active.Trim() != "Y") ? Color.Red : Color.Black));

                //CmbHtSrc.Items.Add(new ListItem(Entity.Desc, Entity.Code));
            }
            CmbHtSrc.Items.AddRange(listItem2.ToArray());
            CmbHtSrc.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            CmbHtSrc.SelectedIndex = 0;

            cmbLang.Items.Clear(); cmbLang.ColorMember = "FavoriteColor";
            List<CommonEntity> Language = _model.lookupDataAccess.GetPrimaryLanguage();
            List<ListItem> listItem3 = new List<ListItem>();
            foreach (CommonEntity Entity in Language)
            {
                //if ((Mode == "Add" && Entity.Active.Trim() != "N")) || (Mode == "Edit"))
                listItem3.Add(new ListItem(Entity.Desc, Entity.Code.Trim(), Entity.Active.Trim(), (Entity.Active.Trim() != "Y") ? Color.Red : Color.Black));

                //CmbHtSrc.Items.Add(new ListItem(Entity.Desc, Entity.Code));
            }
            cmbLang.Items.AddRange(listItem3.ToArray());
            cmbLang.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            cmbLang.SelectedIndex = 0;

            listItem2.Clear();
            CmbCellProv.Items.Clear(); CmbCellProv.ColorMember = "FavoriteColor";
            List<CommonEntity> CellProvider = _model.lookupDataAccess.Get_CellProvider();
            foreach (CommonEntity Entity in CellProvider)
            {
                //if ((Mode == "Add" && Entity.Active.Trim() != "N")) || (Mode == "Edit"))
                listItem2.Add(new ListItem(Entity.Desc, Entity.Code.Trim(), Entity.Active.Trim(), (Entity.Active.Trim() != "Y") ? Color.Red : Color.Black));

                //CmbCellProv.Items.Add(new ListItem(Entity.Desc, Entity.Code));
                CmbCellProv.Items.AddRange(listItem2.ToArray());
            }
            CmbCellProv.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            CmbCellProv.SelectedIndex = 0;


            FillCaseWroker();

            //List<ListItem> listItem3 = new List<ListItem>();
            //listItem3.Add(new ListItem("Select One", "0"));
            //listItem3.Add(new ListItem("New Client ", "N"));
            //listItem3.Add(new ListItem("Returning Client", "R"));
            //cmbClientStatus.Items.AddRange(listItem3.ToArray());
            //cmbClientStatus.SelectedIndex = 0;

            PropClientStatus = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00126", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
            cmbClientStatus.Items.Insert(0, new ListItem("Select One", "0"));
            cmbClientStatus.ColorMember = "FavoriteColor";
            cmbClientStatus.SelectedIndex = 0;
            foreach (CommonEntity Entity in PropClientStatus)
            {
                ListItem li = new ListItem(Entity.Desc, Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbClientStatus.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Entity.Default.Equals("Y")) cmbClientStatus.SelectedItem = li;
            }


            PropStatus = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00125", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            cmbStatus.Items.Insert(0, new ListItem("Select One", "0"));
            cmbStatus.ColorMember = "FavoriteColor";
            cmbStatus.SelectedIndex = 0;
            foreach (CommonEntity casetype in PropStatus)
            {
                ListItem li = new ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbStatus.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && casetype.Default.Equals("Y")) cmbStatus.SelectedItem = li;
            }

        }


        private int Get_WeekDay_Name(string App_Week_Day)
        {
            int Week_day = 1;

            switch (App_Week_Day)
            {
                case "Monday": Week_day = 1; break;
                case "Tuesday": Week_day = 2; break;
                case "Wednesday": Week_day = 3; break;
                case "Thursday": Week_day = 4; break;
                case "Friday": Week_day = 5; break;
                case "Saturday": Week_day = 6; break;
                case "Sunday": Week_day = 7; break;
            }
            return Week_day;
        }

        string[,] Slots_Array;
        int Slots_Array_Colmns = 13;//12;
        string Process_Template_Type = "", Process_Template_Date = "";
        bool Next_Day_Slot_Exist = false, In_Search_Of_NextDay_Slot = false;
        int Tot_Sch_Slot_Cnt = 0;
        DateTime PrivApp_date = new DateTime();
        private void AppDate_LostFocus(object sender, EventArgs e)
        {
            Pb_Add.Visible = Pb_Edit.Visible = Pb_Delete.Visible = false;
            ClearBottomControls();
            if (!(string.IsNullOrEmpty(TxtSiteCode.Text)))
            {
                DateTime App_date = new DateTime();
                App_date = AppDate.Value;

                int Week_Day = Get_WeekDay_Name(App_date.DayOfWeek.ToString());

                Lbl_Now_Sch.Visible = Lbl_Reserved.Visible = false;
                this.SchAppGrid.SelectionChanged -= new System.EventHandler(this.SchAppGrid_SelectionChanged);
                SchAppGrid.Rows.Clear();

                int TmpCount = 0, Sel_SSN_Rec_Index = 0; Tot_Sch_Slot_Cnt = 0;
                bool Open_Day_With_Slots = true; Process_Template_Date = Process_Template_Type = "";
                //List<APPTEMPLATESEntity> apptTempplatelist = _model.TmsApcndata.GetAPPTEMPLATESadpysitedates(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty);
                //if (apptTempplatelist.Count > 0)
                //{              
                string Day_table, Period_Table = null, Template_Type = null;
                bool Template_Sw = false;

                int Template_slots = 0;
                int Template_Min = 5;
                List<APPTEMPLATESEntity> apptTempplatelist = _model.TmsApcndata.GetAPPTEMPLATESadpysitedates(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), string.Empty, "Dates", "Dates");
                if (apptTempplatelist.Count > 0)
                {


                    APPTEMPLATESEntity apptTemplatedata = apptTempplatelist[0];
                    if (apptTemplatedata != null)
                    {

                        List<APPTSCHEDULEEntity> propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                        //Added by Sudheer on 12/21/2020
                        propAPPtSchedulelist = propAPPtSchedulelist.FindAll(u => u.Status != "03" && u.Status != "04" && u.Status != "05" && u.Status != "02");

                        string Gbl_OpenClose = apptTemplatedata.TemplateAvailble;

                        Day_table = apptTemplatedata.DayTable;

                        if (Day_table.Substring(Week_Day - 1, 1) == "1")
                        {
                            Period_Table = apptTemplatedata.PeriodTable;
                            Template_slots = int.Parse(apptTemplatedata.SlotsPerPeriod);
                            // Template_Min = int.Parse(apptTemplatedata.Mins);
                            Template_Type = apptTemplatedata.Type;
                            Template_Sw = true;
                        }
                        //if (Gbl_OpenClose == "0")
                        //{
                        //    if (!In_Search_Of_NextDay_Slot)
                        //        MessageBox.Show("Template is Closed for this Day", "CAP Systems", MessageBoxButtons.OK);
                        //    Open_Day_With_Slots = false;
                        //    break;
                        //}

                        if (Process_Template_Type != "3")
                        {
                            if (Day_table.Substring(Week_Day - 1, 1) == "0")
                            {
                                if (!In_Search_Of_NextDay_Slot)
                                    AlertBox.Show(App_date.DayOfWeek.ToString() + " Is a Close Day, No Appointments Allowed",MessageBoxIcon.Warning);
                                Open_Day_With_Slots = Template_Sw = false;
                                //break;
                            }
                        }
                        if (!Period_Table.Contains("1"))
                        {
                            if (!In_Search_Of_NextDay_Slot)
                                AlertBox.Show(" No Slot is Opened for this Template", MessageBoxIcon.Warning);
                            Open_Day_With_Slots = Template_Sw = false;
                            // break;
                        }

                        string AM_PM = "AM";

                        if (Template_Sw && !string.IsNullOrEmpty(Period_Table) && Gbl_OpenClose == "1")
                        {
                            if (Period_Table.Contains("1"))
                            {
                                int Hours = 00, Mins = 00;
                                string Disp_Hours = "00", Disp_Miin = "00", Display_date = null;
                                string[] time;

                                string Pass_Hours = "00", Pass_Min = "00";

                                int Local_Time = 0;
                                string Local_Disp_Time = null;

                                int intSlotdetails = Period_Table.Length / Template_slots;

                                int intstartindex = 0;
                                List<CommonEntity> commonslots = new List<CommonEntity>();
                                for (int i = 0; i < intSlotdetails; i++)
                                {
                                    string strslots = Period_Table.Substring(intstartindex, Template_slots);
                                    commonslots.Add(new CommonEntity(i.ToString(), strslots));
                                    intstartindex = Template_slots + intstartindex;
                                }

                                for (int i = 0; i < intSlotdetails; i++)
                                {
                                    if ((Mins >= 60) && (Mins % 60) >= 0)
                                    {
                                        Hours++;
                                        Mins = 00;
                                    }

                                    if (Hours >= 12)
                                        AM_PM = "PM";

                                    Disp_Hours = Hours.ToString();
                                    Pass_Hours = Hours.ToString();
                                    if (Hours > 12)
                                    {
                                        Disp_Hours = (Hours - 12).ToString();
                                        Pass_Hours = "00".Substring(0, 2 - Pass_Hours.Length) + Pass_Hours;
                                    }

                                    Pass_Min = Mins.ToString();
                                    Disp_Miin = Pass_Min = "00".Substring(0, 2 - Pass_Min.Length) + Pass_Min;
                                    Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;

                                    if (Pass_Hours + Pass_Min == "000")
                                    {
                                        Pass_Hours = "0";
                                        Pass_Min = string.Empty;
                                    }

                                    string Appt_Rec_Type = "";
                                    CommonEntity slotsdetailis = commonslots.Find(u => u.Code.ToString() == i.ToString());
                                    if (slotsdetailis != null)
                                    {
                                        string strslotsdetails = slotsdetailis.Desc.ToString();
                                        int rowIndex = 0;
                                        for (int intslotd = 0; intslotd < strslotsdetails.Length; intslotd++)
                                        {
                                            if (strslotsdetails.Substring(intslotd, 1).ToString() == "1")
                                            {
                                                APPTSCHEDULEEntity apptexistedSlot = propAPPtSchedulelist.Find(u => (Convert.ToDateTime(u.Date) == Convert.ToDateTime(AppDate.Value)) && u.Time == Pass_Hours + Pass_Min && u.SlotNumber == (intslotd + 1).ToString());
                                                if (apptexistedSlot != null)
                                                {
                                                    string strPhoneNumber = LookupDataAccess.GetPhoneFormat(apptexistedSlot.TelNumber);
                                                    if (apptexistedSlot.CellNumber != string.Empty)
                                                        strPhoneNumber = LookupDataAccess.GetPhoneFormat(apptexistedSlot.CellNumber);
                                                    //if (apptexistedSlot.SsNumber.Trim() != string.Empty)
                                                    //    strSSN = LookupDataAccess.GetCardNo(apptexistedSlot.SsNumber, "1", "N", string.Empty);
                                                    //

                                                    string strCaseWorker = string.Empty;

                                                    if (!string.IsNullOrEmpty(apptexistedSlot.CaseWorker.ToString().Trim()))
                                                    {
                                                        if (propListCaseWorker.Count > 0)
                                                        {
                                                            ListItem listcaseworker = propListCaseWorker.Find(u => u.Value.ToString().Trim() == apptexistedSlot.CaseWorker.ToString().Trim());
                                                            if (listcaseworker != null)
                                                                strCaseWorker = listcaseworker.Text;

                                                        }
                                                    }

                                                    string strStatus = string.Empty;
                                                    if (!string.IsNullOrEmpty(apptexistedSlot.Status.ToString().Trim()))
                                                    {
                                                        if (PropStatus.Count > 0)
                                                        {
                                                            CommonEntity SelStatus = PropStatus.Find(u => u.Code.Trim().Equals(apptexistedSlot.Status.Trim()));
                                                            if (SelStatus != null)
                                                                strStatus = SelStatus.Desc.Trim();
                                                        }
                                                    }

                                                    //commented by Sudheer on 09/04/2020 from the document CAPc
                                                    //rowIndex = SchAppGrid.Rows.Add(false, Disp_Hours + ":" + Disp_Miin + " " + AM_PM, apptexistedSlot.LastName + " " + apptexistedSlot.FirstName, LookupDataAccess.Getdate(apptexistedSlot.DOB), strPhoneNumber, strCaseWorker, (intslotd + 1).ToString(), Pass_Hours + Pass_Min, "Y", apptexistedSlot.SsNumber, apptexistedSlot.SchdType, Template_Type);
                                                    rowIndex = SchAppGrid.Rows.Add(false, Disp_Hours + ":" + Disp_Miin + " " + AM_PM, apptexistedSlot.FirstName + " " + apptexistedSlot.LastName, LookupDataAccess.Getdate(apptexistedSlot.DOB), strPhoneNumber, strCaseWorker, strStatus, (intslotd + 1).ToString(), Pass_Hours + Pass_Min, "Y", apptexistedSlot.SsNumber, apptexistedSlot.SchdType, Template_Type, apptexistedSlot.SlotType);
                                                    SchAppGrid.Rows[rowIndex].Tag = apptexistedSlot;
                                                    if (chkExcludeSlots.Checked == true)
                                                    {
                                                        if (apptexistedSlot.SchdType == "2")
                                                        {

                                                            SchAppGrid.Rows[rowIndex].Visible = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Rec_Sel_From_SSN_Search)
                                                        {
                                                            if (Sel_SSN_Rec[0] == Disp_Hours + ":" + Disp_Miin + " " + AM_PM && Sel_SSN_Rec[1] == (intslotd + 1).ToString())
                                                            {
                                                                SchAppGrid.Rows[rowIndex].Selected = true;
                                                                Sel_SSN_Rec_Index = rowIndex;
                                                            }
                                                        }
                                                    }
                                                    CommonFunctions.setTooltip(rowIndex, apptexistedSlot.AddOperator, apptexistedSlot.DateAdd, apptexistedSlot.LstcOperation, apptexistedSlot.DateLstc, SchAppGrid);
                                                    TmpCount++;

                                                }
                                                else
                                                {
                                                    rowIndex = SchAppGrid.Rows.Add(false, Disp_Hours + ":" + Disp_Miin + " " + AM_PM, " ", " ", " ", " ", " ", (intslotd + 1).ToString(), Pass_Hours + Pass_Min, "N", "", "", Template_Type, "");
                                                    SchAppGrid.Rows[rowIndex].Tag = string.Empty;
                                                    SchAppGrid.Rows[rowIndex].Cells["Delete"].ReadOnly = true;
                                                }

                                            }

                                            if (strslotsdetails.Length == intslotd + 1)
                                            {
                                                List<APPTSCHEDULEEntity> Appschlist = propAPPtSchedulelist.FindAll(u => (Convert.ToDateTime(u.Date) == Convert.ToDateTime(AppDate.Value)) && u.Time == Pass_Hours + Pass_Min && u.SlotType == "A");

                                                if (Appschlist.Count > 0)
                                                {
                                                    foreach (APPTSCHEDULEEntity Entity in Appschlist)
                                                    {
                                                        string strPhoneNumber = LookupDataAccess.GetPhoneFormat(Entity.TelNumber);
                                                        if (Entity.CellNumber != string.Empty)
                                                            strPhoneNumber = LookupDataAccess.GetPhoneFormat(Entity.CellNumber);
                                                        //if (apptexistedSlot.SsNumber.Trim() != string.Empty)
                                                        //    strSSN = LookupDataAccess.GetCardNo(apptexistedSlot.SsNumber, "1", "N", string.Empty);
                                                        //

                                                        string strCaseWorker = string.Empty;

                                                        if (!string.IsNullOrEmpty(Entity.CaseWorker.ToString().Trim()))
                                                        {
                                                            if (propListCaseWorker.Count > 0)
                                                            {
                                                                ListItem listcaseworker = propListCaseWorker.Find(u => u.Value.ToString().Trim() == Entity.CaseWorker.ToString().Trim());
                                                                if (listcaseworker != null)
                                                                    strCaseWorker = listcaseworker.Text;

                                                            }
                                                        }

                                                        string strStatus = string.Empty;
                                                        if (!string.IsNullOrEmpty(Entity.Status.ToString().Trim()))
                                                        {
                                                            if (PropStatus.Count > 0)
                                                            {
                                                                CommonEntity SelStatus = PropStatus.Find(u => u.Code.Trim().Equals(Entity.Status.Trim()));
                                                                if (SelStatus != null)
                                                                    strStatus = SelStatus.Desc.Trim();
                                                            }
                                                        }

                                                        string Appstatus = "N";
                                                        if (!string.IsNullOrEmpty(Entity.LastName.Trim())) Appstatus = "Y";

                                                        rowIndex = SchAppGrid.Rows.Add(false, Disp_Hours + ":" + Disp_Miin + " " + AM_PM, Entity.FirstName + " " + Entity.LastName, LookupDataAccess.Getdate(Entity.DOB), strPhoneNumber, strCaseWorker, strStatus, Entity.SlotNumber, Pass_Hours + Pass_Min, Appstatus, Entity.SsNumber, Entity.SchdType, Template_Type, Entity.SlotType);
                                                        SchAppGrid.Rows[rowIndex].Tag = Entity;
                                                    }
                                                }
                                            }

                                        }

                                    }

                                    Mins += Template_Min;
                                }


                            }
                        }

                    }




                    this.SchAppGrid.SelectionChanged += new System.EventHandler(this.SchAppGrid_SelectionChanged);
                    if (SchAppGrid.Rows.Count > 0)
                    {
                        if (chkExcludeSlots.Checked)
                        {
                            foreach (DataGridViewRow item in SchAppGrid.Rows)
                            {
                                if (item.Visible)
                                {
                                    item.Selected = true;

                                    Sel_SSN_Rec_Index = item.Index;
                                    Priv_Selected_Row = item.Index;
                                    break;
                                }
                            }
                           // SchAppGrid.Rows[0].Selected = true;
                            //if (SchAppGrid.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
                            //{

                            //}

                        }

                        Tot_Sch_Slot_Cnt = SchAppGrid.Rows.Count;
                        Next_Day_Slot_Exist = true;
                        if (!Just_Saved)
                        {
                            if (Rec_Sel_From_SSN_Search)
                            {
                                //SchAppGrid.Rows[0].Tag = 0;
                                //BtnNextSlot.Visible = true;
                                //SchAppGrid.CurrentCell = SchAppGrid.Rows[0].Cells[1];
                                SchAppGrid.CurrentCell = SchAppGrid.Rows[Sel_SSN_Rec_Index].Cells[1];
                                scrollPosition = SchAppGrid.CurrentCell.RowIndex;

                               // CurrentPage = (scrollPosition / SchAppGrid.ItemsPerPage);
                              //  CurrentPage++;
                               // SchAppGrid.CurrentPage = CurrentPage;
                               // SchAppGrid.FirstDisplayedScrollingRowIndex = scrollPosition;
                            }
                            else
                            {
                                //SchAppGrid.Rows[0].Tag = 0;
                                //BtnNextSlot.Visible = true;
                                //SchAppGrid.CurrentCell = SchAppGrid.Rows[0].Cells[1];
                                SchAppGrid.CurrentCell = SchAppGrid.Rows[Priv_Selected_Row].Cells[1];
                                scrollPosition = SchAppGrid.CurrentCell.RowIndex;
                            }
                        }
                        else
                        {
                            //SchAppGrid.CurrentPage = CurrentPage;
                            SchAppGrid.CurrentCell = SchAppGrid.Rows[scrollPosition].Cells[1];
                           // SchAppGrid.FirstDisplayedScrollingRowIndex = scrollPosition;
                        }

                        SchAppGrid_SelectionChanged(SchAppGrid, e);
                        //BtnMemSearch.Visible = true;
                        //if (Privileges.ChangePriv == "Y")
                        //    BtnUpdate.Visible = true;

                        SchAppGrid.Rows[0].Selected = true;
                    }
                    else
                    {
                        Btn_Update.Visible = false; // BtnNextSlot.Visible = false;
                        //BtnMemSearch.Visible = false;
                        if (Open_Day_With_Slots && !In_Search_Of_NextDay_Slot)
                            AlertBox.Show("No Templates Defined!!!", MessageBoxIcon.Warning);
                    }

                    if (!Open_Day_With_Slots && !In_Search_Of_NextDay_Slot)
                        Enable_DisableBottomControls(false);
                }
                else
                {
                    AlertBox.Show("No Template is Defined for this Site", MessageBoxIcon.Warning);
                    Enable_DisableBottomControls(false);
                }
                //this.SchAppGrid.SelectionChanged += new System.EventHandler(this.SchAppGrid_SelectionChanged);
                PrivApp_date = AppDate.Value;
            }
            else
            {
                AlertBox.Show(" Please fill Site Code First", MessageBoxIcon.Warning);
                //TxtSiteCode.Text = Priv_Sel_Site;
            }

            Priv_Selected_Row = 0;
            Next_Day_Avail_Slot_Key[0] = Next_Day_Avail_Slot_Key[1] = "";
            Rec_Sel_From_SSN_Search = Just_Saved = false;
        }

        //private void Delete_Sel_App(object sender, EventArgs e)
        //{
        //    MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;

        //    if (messageBoxWindow.DialogResult == DialogResult.Yes)
        //    {
        //        DeleteSel_Appoint();
        //    }
        //}


        string Class_Error = "";
        private bool Check_MST_Classification()
        {
            bool Can_Save = true; Class_Error = "";

            return Can_Save;
        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            //if (Mode.Equals("Delete"))
            //{
            //    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Delete_Sel_App, true);
            //}
            //else
            //{
            if (ValidateForm())
            {
                APPTSCHEDULEEntity UpdateEntity = new Model.Objects.APPTSCHEDULEEntity();
                Get_MST_SNP_SSN_Status();

                char Rec_type = 'A';

                if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y")
                    Rec_type = 'U';

                if (SchAppGrid.CurrentRow.Cells["SlotType"].Value.ToString() == "A")
                    Rec_type = 'U';

                //if (Mode.Equals("Add"))
                //    UpdateEntity.SsNumber = MskSsn.Text;
                //else
                //    Rec_type = 'U';

                UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
                UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
                UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
                UpdateEntity.Year = string.Empty;

                UpdateEntity.Site = TxtSiteCode.Text;
                UpdateEntity.Date = AppDate.Value.ToShortDateString();
                UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
                UpdateEntity.SlotNumber = SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString();

                UpdateEntity.SsNumber = null;
                if (dtBirth.Checked)
                    UpdateEntity.DOB = dtBirth.Value.ToShortDateString();
                else
                    UpdateEntity.DOB = string.Empty;

                UpdateEntity.TemplateID = SchAppGrid.CurrentRow.Cells["gvt_TempId"].Value.ToString();
                UpdateEntity.SchdType = "1";

                UpdateEntity.LastName = TxtLstName.Text;
                UpdateEntity.FirstName = TxtFrstName.Text;

                UpdateEntity.TelNumber = UpdateEntity.HNo = UpdateEntity.Suffix = UpdateEntity.Apt =
                UpdateEntity.Floor = UpdateEntity.City = UpdateEntity.State = UpdateEntity.Zip1 =
                UpdateEntity.Zip2 = UpdateEntity.Sex = UpdateEntity.CaseWorker = UpdateEntity.HeatSource =
                UpdateEntity.SourceIncome = UpdateEntity.CellNumber = UpdateEntity.Street = UpdateEntity.CellProvider =
                UpdateEntity.EditTime = UpdateEntity.EditBy = UpdateEntity.Email = null;

                if (!string.IsNullOrEmpty(MskPhone.Text.Trim()))
                    UpdateEntity.TelNumber = MskPhone.Text.Trim();

                if (!string.IsNullOrEmpty(TxtHn.Text.Trim()))
                    UpdateEntity.HNo = TxtHn.Text;

                if (!string.IsNullOrEmpty(TxtStreet.Text.Trim()))
                    UpdateEntity.Street = TxtStreet.Text;

                if (!string.IsNullOrEmpty(TxtSf.Text.Trim()))
                    UpdateEntity.Suffix = TxtSf.Text;

                if (!string.IsNullOrEmpty(TxtApt.Text.Trim()))
                    UpdateEntity.Apt = TxtApt.Text;

                if (!string.IsNullOrEmpty(TxtFlr.Text.Trim()))
                    UpdateEntity.Floor = TxtFlr.Text;

                if (!string.IsNullOrEmpty(TxtCity.Text.Trim()))
                    UpdateEntity.City = TxtCity.Text;

                if (!string.IsNullOrEmpty(TxtState.Text.Trim()))
                    UpdateEntity.State = TxtState.Text;

                if (!(string.IsNullOrEmpty(TxtZip.Text)))
                    UpdateEntity.Zip1 = TxtZip.Text;
                if (!(string.IsNullOrEmpty(TxtZip1.Text)))
                    UpdateEntity.Zip2 = TxtZip1.Text;

                if (!(string.IsNullOrEmpty(TxtEMail.Text)))
                    UpdateEntity.Email = TxtEMail.Text;

                if (((ListItem)CmbGen.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.Sex = ((ListItem)CmbGen.SelectedItem).Value.ToString();

                if (((ListItem)CmbWorker.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();

                if (((ListItem)CmbHtSrc.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.HeatSource = ((ListItem)CmbHtSrc.SelectedItem).Value.ToString();

                if (((ListItem)CmbCellProv.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.CellProvider = ((ListItem)CmbCellProv.SelectedItem).Value.ToString();

                if (((ListItem)cmbClientStatus.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.Client = ((ListItem)cmbClientStatus.SelectedItem).Value.ToString();

                if (((ListItem)cmbStatus.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.Status = ((ListItem)cmbStatus.SelectedItem).Value.ToString();

                if (((ListItem)cmbLang.SelectedItem).Value.ToString() != "0")
                    UpdateEntity.Language = ((ListItem)cmbLang.SelectedItem).Value.ToString();

                if (!(string.IsNullOrEmpty(txtIncSrc.Text.Trim())))
                    UpdateEntity.SourceIncome = txtIncSrc.Text;

                if (!string.IsNullOrEmpty(MskCell.Text.Trim()))
                    UpdateEntity.CellNumber = MskCell.Text.Trim();

                UpdateEntity.ContactDate = DtpCntrctDt.Text;

                if (!string.IsNullOrEmpty(txtNotes.Text.Trim()))
                    UpdateEntity.Notes = txtNotes.Text.Trim();

                UpdateEntity.SlotType = SchAppGrid.CurrentRow.Cells["SlotType"].Value.ToString();

                UpdateEntity.LstcOperation = UpdateEntity.ContactPerson =
                UpdateEntity.AddOperator = BaseForm.UserID;
                if (Rec_type == 'U')
                    UpdateEntity.Mode = "Edit";
                else
                    UpdateEntity.Mode = "Add";

                if (UpdateEntity.Status == "03" || UpdateEntity.Status == "04" || UpdateEntity.Status == "05" || UpdateEntity.Status == "02")
                {
                    APPTSCHDHISTEntity HistEntity = new APPTSCHDHISTEntity();

                    HistEntity.Agency = UpdateEntity.Agency;
                    HistEntity.Dept = UpdateEntity.Dept;
                    HistEntity.Program = UpdateEntity.Program;
                    HistEntity.Year = UpdateEntity.Year;
                    HistEntity.Site = UpdateEntity.Site;
                    HistEntity.Date = UpdateEntity.Date;
                    HistEntity.Time = UpdateEntity.Time;
                    HistEntity.SlotNumber = UpdateEntity.SlotNumber;
                    HistEntity.Seq = "1";
                    HistEntity.SsNumber = UpdateEntity.SsNumber;
                    HistEntity.TemplateID = UpdateEntity.TemplateID;
                    HistEntity.SchdType = UpdateEntity.SchdType;
                    HistEntity.SchdDay = UpdateEntity.SchdDay;
                    HistEntity.LastName = UpdateEntity.LastName;
                    HistEntity.FirstName = UpdateEntity.FirstName;
                    HistEntity.TelNumber = UpdateEntity.TelNumber;
                    HistEntity.HNo = UpdateEntity.HNo;
                    HistEntity.Street = UpdateEntity.Street;
                    HistEntity.Suffix = UpdateEntity.Suffix;
                    HistEntity.Apt = UpdateEntity.Apt;
                    HistEntity.Floor = UpdateEntity.Floor;
                    HistEntity.City = UpdateEntity.City;
                    HistEntity.State = UpdateEntity.State;
                    HistEntity.Zip1 = UpdateEntity.Zip1;
                    HistEntity.Zip2 = UpdateEntity.Zip2;
                    HistEntity.HeatSource = UpdateEntity.HeatSource;
                    HistEntity.SourceIncome = UpdateEntity.SourceIncome;
                    HistEntity.ContactPerson = UpdateEntity.ContactPerson;
                    HistEntity.ContactDate = UpdateEntity.ContactDate;
                    HistEntity.Sex = UpdateEntity.Sex;
                    HistEntity.CellProvider = UpdateEntity.CellProvider;
                    HistEntity.CellNumber = UpdateEntity.CellNumber;
                    HistEntity.CaseWorker = UpdateEntity.CaseWorker;
                    HistEntity.DateLstc = UpdateEntity.DateLstc;
                    HistEntity.LstcOperation = UpdateEntity.LstcOperation;
                    HistEntity.DateAdd = UpdateEntity.DateAdd;
                    HistEntity.AddOperator = UpdateEntity.AddOperator;
                    HistEntity.EditTime = UpdateEntity.EditTime;
                    HistEntity.EditBy = UpdateEntity.EditBy;
                    HistEntity.Email = UpdateEntity.Email;
                    HistEntity.DOB = UpdateEntity.DOB;
                    HistEntity.Status = UpdateEntity.Status;
                    HistEntity.Client = UpdateEntity.Client;
                    HistEntity.Notes = UpdateEntity.Notes;

                    HistEntity.Mode = "Add";

                    UpdateEntity.Mode = "Delete";

                    if (_model.TMS00110Data.InsertUpdateDelAPPTSCHDHIST(HistEntity))
                    {
                        if (Rec_type == 'U')
                        {
                            _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);
                            AlertBox.Show("Appointment Cancelled Successfully");

                            Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
                            Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
                            SchAppGrid.Enabled = Refresh_Control = Just_Saved = true;
                            Enable_DisableBottomControls(false);
                            Btn_Update.Visible = BtnCancel.Visible = false;
                            //this.DialogResult = DialogResult.OK;
                            propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHDHISTBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, SchSite, AppDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            if (propAPPtSchedulelist.Count > 0)
                                btnHist.Visible = true;

                            AppDate_LostFocus(AppDate, e);
                        }
                    }


                }
                else
                {
                    if (_model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity))
                    {
                        if (Rec_type == 'U')
                            AlertBox.Show("Record Updated Successfully");
                        else
                        {
                            AlertBox.Show("Record Inserted Successfully");
                            NowSch_Slot_Flag = false;
                        }
                        Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
                        Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
                        SchAppGrid.Enabled = Refresh_Control = Just_Saved = true;
                        Enable_DisableBottomControls(false);
                        Btn_Update.Visible = BtnCancel.Visible = false;
                        //this.DialogResult = DialogResult.OK;

                        AppDate_LostFocus(AppDate, e);
                    }
                    else
                    {
                        //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
                        AlertBox.Show("Slot already booked by another user!!!", MessageBoxIcon.Warning);
                        AppDate_LostFocus(AppDate, e);
                    }
                }


            }
            //}

            if (Mode != "Add")
                TxtSiteCode.Enabled = BtnSearch.Enabled = AppDate.Enabled = false;
        }

        private void Set_Menu_Buttons_Visibility(string Status)
        {
            if (Status == "Add")
            {
                dtBirth.Focus();
                if (Privileges.AddPriv.Equals("false"))
                    Pb_Add.Visible = false;
                else
                    Pb_Add.Visible = true;

                Pb_Edit.Visible = Pb_Delete.Visible = false;

                if (SchAppGrid.CurrentRow.Cells["SlotType"].Value.ToString() == "A")
                {
                    if (Privileges.DelPriv.Equals("false"))
                        Pb_Delete.Visible = false;
                    else
                        Pb_Delete.Visible = true;
                }
            }

            if (Status == "Edit")
            {
                dtBirth.Focus();
                Pb_Add.Visible = false;

                if (Privileges.ChangePriv.Equals("false"))
                    Pb_Edit.Visible = false;
                else
                    Pb_Edit.Visible = true;

                if (Privileges.DelPriv.Equals("false"))
                    Pb_Delete.Visible = false;
                else
                    Pb_Delete.Visible = true;
            }
        }



        List<TmsApptEntity> Date_Appt_List = new List<TmsApptEntity>();
        //  APPTSCHEDULEEntity UpdateEntity;
        int scrollPosition, CurrentPage;
        bool Just_Saved = false;
        private void SchAppGrid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {


                Clear_RequiredIcons();
                if (SchAppGrid.Rows.Count > 0)
                {
                    Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = Lbl_Now_Sch.Visible = Lbl_Reserved.Visible = false;
                    if (!Next_Slot_Process)
                    {
                        // List<TmsApptEntity> ApptRecord = new List<TmsApptEntity>();
                        string strStatus = SchAppGrid.CurrentRow.Cells["AppStatus"].Value == null ? string.Empty : SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString();
                        if (strStatus.ToString() == "Y")
                        {

                            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);


                            // APPTSCHEDULEEntity ApptUpdateEntity = SchAppGrid.CurrentRow.Tag as APPTSCHEDULEEntity;

                            if (ApptRecord.Count > 0)
                            {
                                APPTSCHEDULEEntity ApptUpdateEntity = ApptRecord[0];
                                if (ApptRecord[0].SchdType != "3")
                                {
                                    //if (Mode.Equals("Add"))
                                    //    Enable_DisableBottomControls(false);
                                    //else
                                    //    Enable_DisableBottomControls(true);

                                    Set_Menu_Buttons_Visibility("Edit");

                                    if (ApptUpdateEntity.DOB != string.Empty)
                                    {
                                        dtBirth.Value = Convert.ToDateTime(ApptUpdateEntity.DOB);
                                        dtBirth.Checked = true;
                                    }
                                    else
                                    {
                                        dtBirth.Value = DateTime.Now.Date;
                                        dtBirth.Checked = false;

                                    }
                                    TxtLstName.Text = ApptUpdateEntity.LastName;
                                    TxtFrstName.Text = ApptUpdateEntity.FirstName;
                                    //MskPhone.Text = UpdateEntity.TelAreaCode + UpdateEntity.TelNumber;
                                    MskPhone.Text = ApptUpdateEntity.TelNumber;
                                    TxtHn.Text = ApptUpdateEntity.HNo;
                                    TxtStreet.Text = ApptUpdateEntity.Street;

                                    TxtSf.Text = ApptUpdateEntity.Suffix;
                                    TxtApt.Text = ApptUpdateEntity.Apt;
                                    TxtFlr.Text = ApptUpdateEntity.Floor;
                                    TxtCity.Text = ApptUpdateEntity.City;
                                    TxtState.Text = ApptUpdateEntity.State;
                                    TxtEMail.Text = ApptUpdateEntity.Email.Trim();
                                    TxtZip.Text = (!string.IsNullOrEmpty(ApptUpdateEntity.Zip1.Trim()) ? "00000".Substring(0, 5 - ApptUpdateEntity.Zip1.Trim().Length) + ApptUpdateEntity.Zip1 : "");
                                    TxtZip1.Text = (!string.IsNullOrEmpty(ApptUpdateEntity.Zip2.Trim()) ? "0000".Substring(0, 4 - ApptUpdateEntity.Zip2.Trim().Length) + ApptUpdateEntity.Zip2 : "");

                                    if (ApptUpdateEntity.Sex != string.Empty)
                                        CommonFunctions.SetComboBoxValue(CmbGen, ApptUpdateEntity.Sex);
                                    else
                                        CmbGen.SelectedIndex = 0;

                                    if (ApptUpdateEntity.CaseWorker != string.Empty)
                                        CommonFunctions.SetComboBoxValue(CmbWorker, ApptUpdateEntity.CaseWorker);
                                    else
                                        CmbWorker.SelectedIndex = 0;

                                    if (ApptUpdateEntity.HeatSource != string.Empty)
                                        CommonFunctions.SetComboBoxValue(CmbHtSrc, ApptUpdateEntity.HeatSource);
                                    else
                                        CmbHtSrc.SelectedIndex = 0;

                                    if (ApptUpdateEntity.CellProvider != string.Empty)
                                        CommonFunctions.SetComboBoxValue(CmbCellProv, ApptUpdateEntity.CellProvider);
                                    else
                                        CmbCellProv.SelectedIndex = 0;

                                    if (ApptUpdateEntity.Status != string.Empty)
                                        CommonFunctions.SetComboBoxValue(cmbStatus, ApptUpdateEntity.Status);
                                    else
                                        cmbStatus.SelectedIndex = 0;

                                    if (ApptUpdateEntity.Client != string.Empty)
                                        CommonFunctions.SetComboBoxValue(cmbClientStatus, ApptUpdateEntity.Client);
                                    else
                                        cmbClientStatus.SelectedIndex = 0;

                                    if (ApptUpdateEntity.Language != string.Empty)
                                        CommonFunctions.SetComboBoxValue(cmbLang, ApptUpdateEntity.Language);
                                    else
                                        cmbLang.SelectedIndex = 0;

                                    txtIncSrc.Text = ApptUpdateEntity.SourceIncome;
                                    //CmbCellProv.Text = UpdateEntity.CellProvider;
                                    //MskCell.Text = UpdateEntity.CellAreaCode + UpdateEntity.CellNumber;
                                    MskCell.Text = ApptUpdateEntity.CellNumber;
                                    DtpCntrctDt.Text = ApptUpdateEntity.ContactDate;

                                    if (!(string.IsNullOrEmpty(ApptUpdateEntity.ContactDate.Trim())))
                                    {
                                        DtpCntrctDt.Value = Convert.ToDateTime(ApptUpdateEntity.ContactDate);
                                        DtpCntrctDt.Checked = true;
                                    }
                                    else
                                    {
                                        DtpCntrctDt.Value = DateTime.Today;
                                        DtpCntrctDt.Checked = false;
                                    }

                                    txtNotes.Text = ApptUpdateEntity.Notes.Trim();

                                    SchAppGrid.CurrentRow.Cells["Rec_Type"].Value = ApptUpdateEntity.SchdType;

                                    //if (UpdateEntity.RecordType == "2")
                                    //    BtnUpdate.Visible = false;
                                    //else
                                    //    BtnUpdate.Visible = true;
                                }
                                else
                                {
                                    ClearBottomControls();
                                    Set_Menu_Buttons_Visibility("Add");
                                }
                            }
                            else
                            {
                                ClearBottomControls();
                                Set_Menu_Buttons_Visibility("Add");
                            }

                            //else
                            //{
                            //    ClearBottomControls();
                            //    Set_Menu_Buttons_Visibility("Add");
                            //    MessageBox.Show("Record Deleted by by another user!!!", "CAP Systems");
                            //   // AppDate_LostFocus(AppDate, e);
                            //}
                        }
                        else
                        {
                            ClearBottomControls();
                            Set_Menu_Buttons_Visibility("Add");
                        }
                    }
                    else
                    {
                        ClearBottomControls(); //LblAppMode.Text = "Add Appointment";
                    }

                    Lbl_Now_Sch.Visible = Lbl_Reserved.Visible = false;
                    if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y") //&& !Next_Slot_Process
                    {
                        //LblAppMode.Text = "Edit Appointment";
                        dtBirth.Enabled = false; BtnSsnSearch.Enabled = false;
                        if (SchAppGrid.CurrentRow.Cells["Rec_Type"].Value.ToString() == "2")
                        {
                            Btn_Update.Visible = false;
                            Pb_Add.Visible = Pb_Delete.Visible = Pb_Edit.Visible = false;
                            Enable_DisableBottomControls(false);
                            //BtnIncSource.Enabled = BtnZip.Enabled = false;
                            Lbl_Reserved.Visible = true;
                        }
                        else if (SchAppGrid.CurrentRow.Cells["Rec_Type"].Value.ToString() == "3")
                        {
                            Lbl_Now_Sch.Visible = true;
                            Btn_Update.Visible = false;
                            Pb_Add.Visible = Pb_Delete.Visible = Pb_Edit.Visible = false;
                            Enable_DisableBottomControls(false);
                            //BtnIncSource.Enabled = BtnZip.Enabled = false;
                        }
                        //else
                        //{
                        //    Btn_Update.Visible = true;
                        //    Enable_DisableBottomControls(true);
                        //    BtnIncSource.Enabled = BtnZip.Enabled = true;
                        //}
                    }
                    //else
                    //    {
                    //        LblAppMode.Text = "Add Appointment";
                    //        MskSsn.Enabled = true; BtnSsnSearch.Enabled = true;
                    //        DtpCntrctDt.Value = DateTime.Today;
                    //        DtpCntrctDt.Checked = false;
                    //        if (Privileges.AddPriv.Equals("true"))
                    //        {
                    //            Btn_Update.Visible = true;
                    //            Enable_DisableBottomControls(true);
                    //            BtnIncSource.Enabled = BtnZip.Enabled = true;
                    //        }
                    //        else
                    //        {
                    //            Btn_Update.Visible = false;
                    //            Enable_DisableBottomControls(false);
                    //            BtnIncSource.Enabled = BtnZip.Enabled = false;
                    //        }
                    //    }

                    if (!Just_Saved)
                    {
                        //CurrentPage = SchAppGrid.CurrentPage;
                        scrollPosition = SchAppGrid.CurrentCell.RowIndex;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void ClearBottomControls()
        {
            //if (Mode.Equals("Edit"))
            //    Enable_DisableBottomControls(false);
            //else
            //    Enable_DisableBottomControls(true); 
            TxtLstName.Clear();
            TxtFrstName.Clear(); MskPhone.Clear();
            TxtHn.Clear(); TxtStreet.Clear();
            TxtSf.Clear(); TxtApt.Clear();
            TxtFlr.Clear(); TxtCity.Clear();
            TxtState.Clear(); TxtZip.Clear();
            TxtZip1.Clear(); MskCell.Clear();
            txtIncSrc.Clear(); TxtEMail.Clear();

            txtNotes.Clear();

            CmbGen.SelectedIndex = 0;
            CmbWorker.SelectedIndex = 0;
            CmbHtSrc.SelectedIndex = 0;
            CmbCellProv.SelectedIndex = 0;
            cmbLang.SelectedIndex = 0;
            dtBirth.Value = DateTime.Today;
            dtBirth.Checked = false;
            DtpCntrctDt.Value = DateTime.Today;
            DtpCntrctDt.Checked = false;

            cmbClientStatus.SelectedIndex = 0; cmbStatus.SelectedIndex = 0;
        }


        private void Enable_DisableBottomControls(bool Enable)
        {
            //BtnSsnSearch.Enabled = false;
            //if (Mode.Equals("Add"))
            //{
            //    MskSsn.Enabled = true; BtnSsnSearch.Enabled = true;
            //}
            //else
            //    MskSsn.Enabled = false;

            dtBirth.Enabled = Enable;
            TxtLstName.Enabled = Enable;
            TxtFrstName.Enabled = Enable; MskPhone.Enabled = Enable;
            TxtHn.Enabled = Enable; TxtStreet.Enabled = Enable;
            TxtSf.Enabled = Enable; TxtApt.Enabled = Enable;
            TxtFlr.Enabled = Enable; TxtCity.Enabled = Enable;
            TxtState.Enabled = Enable; TxtZip.Enabled = Enable;
            TxtZip1.Enabled = Enable; TxtEMail.Enabled = Enable;

            txtNotes.Enabled = Enable;

            CmbHtSrc.Enabled = Enable; CmbHtSrc.Enabled = Enable;
            cmbStatus.Enabled = Enable; cmbClientStatus.Enabled = Enable;
            cmbLang.Enabled = Enable;
            //txtIncSrc.Enabled = Enable;            
            CmbWorker.Enabled = Enable;
            CmbCellProv.Enabled = Enable; CmbGen.Enabled = Enable;
            MskCell.Enabled = Enable;
            DtpCntrctDt.Enabled = Enable;
            BtnIncSource.Enabled = Enable;
            BtnZip.Enabled = Enable;
            BtnSsnSearch.Enabled = Enable;
            EnabDisable_Top_Cntls(!Enable);

            //if (!Enable && Cntls == "Btns")
            //    BtnSsnSearch.Enabled = BtnIncSource.Enabled = BtnZip.Enabled = false;
        }

        int NextSlot_Index = 0;
        bool Next_Slot_Process = false;
        private void BtnNextSlot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
            {
                return;
            }

            Lbl_Reserved.Visible = false;

            int i = 0; bool All_Slots_Filled = true;
            if (Tot_Sch_Slot_Cnt > 0)
                i = SchAppGrid.SelectedRows[0].Index;

            if (i < (SchAppGrid.Rows.Count - 1) && Tot_Sch_Slot_Cnt > 0)
            {

                int intNext = 0;
                foreach (DataGridViewRow item in SchAppGrid.Rows)
                {
                  
                    if (i < intNext)
                    {


                        if (Convert.ToString(item.Cells["AppStatus"].Value) == "N")
                        {
                            //int CurrentPage = (i / SchAppGrid.ItemsPerPage);
                            //CurrentPage++;
                            //SchAppGrid.CurrentPage = CurrentPage;
                            //
                            //SchAppGrid.FirstDisplayedScrollingRowIndex = i;
                            //

                            if (intNext < SchAppGrid.Rows.Count)
                            {
                                if (Convert.ToString(SchAppGrid.Rows[intNext].Cells["AppStatus"].Value) == "Y")
                                { }
                                else
                                {
                                    scrollPosition = intNext;
                                   // CurrentPage = (scrollPosition / SchAppGrid.ItemsPerPage);
                                    CurrentPage++;
                                   // SchAppGrid.CurrentPage = CurrentPage;
                                    SchAppGrid.CurrentCell = SchAppGrid.Rows[intNext].Cells[1];
                                    //SchAppGrid.FirstDisplayedScrollingRowIndex = scrollPosition;
                                    SchAppGrid.Rows[intNext].Selected = true;


                                    //LblAppMode.Text = "Add Appointment";
                                    //MskSsn.Enabled = true; BtnSsnSearch.Enabled = true;
                                    DtpCntrctDt.Value = DateTime.Today;
                                    DtpCntrctDt.Checked = false;

                                    SchAppGrid_SelectionChanged(SchAppGrid, EventArgs.Empty);
                                    break;
                                }
                            }
                            else
                            {
                                if (All_Slots_Filled)
                                {
                                    if (!(Mode.Equals("Edit")))
                                        get_Next_Day_Available_Slot();
                                    else
                                        AlertBox.Show("All slots are booked. After this \n no Slot is available for Scheduled Date '" + AppDate.Value.ToShortDateString() + "' ", MessageBoxIcon.Warning);
                                }

                            }
                        }
                        else
                        {
                           // intNext--;
                        }


                    }
                    intNext++;
                }

            }
            else
            {
                if (Mode.Equals("Edit"))
                {
                    AlertBox.Show("Next Slot is not available.\n This is the last slot for Scheduled Date '" + AppDate.Value.ToShortDateString() + "' ", MessageBoxIcon.Warning);
                    return;
                }

                //get_Next_Day_Available_Slot();

            }
        }

        private void get_Next_Day_Available_Slot()
        {
            //Next_Day_Slot_Exist = false;
            //In_Search_Of_NextDay_Slot = true;
            //DateTime Tmp_Disp = AppDate.Value;
            //DateTime Tmp = AppDate.Value;
            //int Nxt_Slot_lp_Cnt = 0;
            //for (; (!Next_Day_Slot_Exist && Nxt_Slot_lp_Cnt < 365);)
            //{
            //    Tmp = Tmp.AddDays(1);
            //    AppDate.Value = Tmp;
            //    //  AppDate_LostFocus_For_Next_Day_Available_Slot();

            //    //if (Set_Index_On_Next_Day_Free_Slot() && Next_Day_Slot_Exist)
            //    //    Next_Day_Slot_Exist = true;
            //    //else
            //    //    Next_Day_Slot_Exist = false;

            //    Nxt_Slot_lp_Cnt++;
            //}
            //if ((!Next_Day_Slot_Exist && Nxt_Slot_lp_Cnt == 365))
            //    MessageBox.Show("No Template is defined for this site form " + Tmp_Disp.ToShortDateString() + " to " + Tmp.ToShortDateString());

            //In_Search_Of_NextDay_Slot = false;

        }


        private bool Set_Index_On_Next_Day_Free_Slot()
        {

            int i = 0; bool All_Slots_Filled = true, Found_Free_Slot = false;
            if (Tot_Sch_Slot_Cnt > 0)
                i = SchAppGrid.CurrentRow.Index;

            if (i < (SchAppGrid.Rows.Count - 1) && Tot_Sch_Slot_Cnt > 0)
            {
                for (i++; i < (Slots_Array.Length / Slots_Array_Colmns); i++)   //Sindhe
                {
                    if (Slots_Array[i, 3] == "N")
                    {
                        All_Slots_Filled = false;
                        break;
                    }
                }
                if (i != SchAppGrid.CurrentRow.Index && i < (Slots_Array.Length / Slots_Array_Colmns))   //Sindhe
                {
                    NextSlot_Index = i;
                    Next_Slot_Process = true;
                    SchAppGrid.CurrentCell = SchAppGrid.Rows[i].Cells[1];
                    Next_Slot_Process = false;

                    scrollPosition = 0;
                    scrollPosition = SchAppGrid.CurrentCell.RowIndex;
                   // CurrentPage = (scrollPosition / SchAppGrid.ItemsPerPage);
                    CurrentPage++;
                   // SchAppGrid.CurrentPage = CurrentPage;
                    //SchAppGrid.FirstDisplayedScrollingRowIndex = scrollPosition;

                    //LblAppMode.Text = "Add Appointment";
                    //MskSsn.Enabled = true; BtnSsnSearch.Enabled = true;
                    DtpCntrctDt.Value = DateTime.Today;
                    DtpCntrctDt.Checked = false;
                    SchAppGrid_SelectionChanged(SchAppGrid, EventArgs.Empty);
                    //if (Privileges.AddPriv.Equals("true"))
                    //    Btn_Update.Visible = true;
                    //else
                    //    Btn_Update.Visible = false;

                }
            }

            return Found_Free_Slot;
        }


        private void BtnMemSearch_Click(object sender, EventArgs e)
        {
            Rec_Sel_From_SSN_Search = false;
            Sub_TMS00110_SsnNameSearch Select_App = new Sub_TMS00110_SsnNameSearch(M_Hierarchy + M_Year, TxtSiteCode.Text.Trim(), "APPT0002", BaseForm);
            Select_App.StartPosition = FormStartPosition.CenterScreen;
            Select_App.FormClosed += new FormClosedEventHandler(On_SsnNameSerachForm_Closed);
            Select_App.ShowDialog();
        }

        bool Rec_Sel_From_SSN_Search = false;
        string[] Sel_SSN_Rec = new string[2];
        private void On_SsnNameSerachForm_Closed(object sender, FormClosedEventArgs e)
        {
            Sub_TMS00110_SsnNameSearch form = sender as Sub_TMS00110_SsnNameSearch;
            if (form.DialogResult == DialogResult.OK)
            {
                Rec_Sel_From_SSN_Search = true;
                string[] SelApptKey = new string[4];
                SelApptKey = form.GetSelectedApplicant();


                //TxtSiteCode.Text = SelApptKey[0].Substring(10, 4).Trim();
                TxtSiteCode.Text = SelApptKey[0].Substring(6, 4).Trim();
                if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                    BtnNextSlot.Visible = true;
                else
                    BtnNextSlot.Visible = false;

                Sel_SSN_Rec[0] = SelApptKey[2];
                Sel_SSN_Rec[1] = SelApptKey[3];

                AppDate.Value = Convert.ToDateTime(SelApptKey[1]);
                AppDate_LostFocus(AppDate, e);
            }
        }



        //   Begining of Form Validation 

        private bool ValidateForm()
        {
            bool isValid = true;

            if (Mode.Equals("Add") && (String.IsNullOrEmpty(TxtSiteCode.Text.Trim())))
            {
                _errorProvider.SetError(TxtSiteCode, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtSiteCode, null);

            if (dtBirth.Checked == false)
            {
                _errorProvider.SetError(dtBirth, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDOB.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtBirth, null);
            }

            if ((String.IsNullOrEmpty(TxtLstName.Text.Trim())))
            {
                _errorProvider.SetError(TxtLstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblLstName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtLstName, null);

            if ((String.IsNullOrEmpty(TxtFrstName.Text.Trim())))
            {
                _errorProvider.SetError(TxtFrstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblFstName.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtFrstName, null);

            if ((String.IsNullOrEmpty(TxtCity.Text.Trim())))
            {
                _errorProvider.SetError(TxtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtCity, null);

            if (!DtpCntrctDt.Checked)
            {
                _errorProvider.SetError(DtpCntrctDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblCntctDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(DtpCntrctDt, null);

            if (((ListItem)cmbClientStatus.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbClientStatus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblClient.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbClientStatus, null);

            if ((lblLangReq.Visible) && ((ListItem)cmbLang.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbLang, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLang.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbLang, null);

            if ((string.IsNullOrEmpty(MskCell.Text.Trim())))
            {
                _errorProvider.SetError(MskCell, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblCell.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(MskCell, null);

            if ((lblStatusReq.Visible) && ((ListItem)cmbStatus.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbStatus, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStatus.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(cmbStatus, null);

            if ((lblWorkerReq.Visible) && ((ListItem)CmbWorker.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(CmbWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblWorker.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(CmbWorker, null);


            return (isValid);
        }


        private void CommonTextField_LostFocus(object sender, EventArgs e)
        {
            if (sender == TxtSiteCode)
            {
                //if (TxtSiteCode.Text.Length == 1)
                //    txtCode.Text = "0" + txtCode.Text;
                if (!(string.IsNullOrEmpty(TxtSiteCode.Text.Trim())) && TxtSiteCode.Text.Length == 4)
                    _errorProvider.SetError(TxtSiteCode, null);
            }

            if (sender == TxtState && !(string.IsNullOrEmpty(TxtState.Text.Trim())) && TxtState.Text.Length == 2)
                _errorProvider.SetError(TxtState, null);

            //if (sender == TxtLstName && !(string.IsNullOrEmpty(TxtLstName.Text.Trim())))
            //    _errorProvider.SetError(TxtLstName, null);

            //if (sender == TxtFrstName && !(string.IsNullOrEmpty(TxtFrstName.Text.Trim())))
            //    _errorProvider.SetError(TxtFrstName, null);

            //if (sender == TxtCity && !(string.IsNullOrEmpty(TxtCity.Text.Trim())))
            //    _errorProvider.SetError(TxtCity, null);

            //if (sender == DtpCntrctDt && DtpCntrctDt.Checked)
            //    _errorProvider.SetError(DtpCntrctDt, null);

            if (sender == TxtZip && !(string.IsNullOrEmpty(TxtZip.Text.Trim())))
                TxtZip.Text = "00000".Substring(0, 5 - TxtZip.Text.Trim().Length) + TxtZip.Text;

            if (sender == TxtZip1 && !(string.IsNullOrEmpty(TxtZip1.Text.Trim())))
                TxtZip1.Text = "0000".Substring(0, 4 - TxtZip1.Text.Trim().Length) + TxtZip1.Text;

            //if (sender == dtBirth)// && (!string.IsNullOrEmpty(MskSsn.Text.Trim())))
            // {
            //    Lpb_Error_Alter = "";
            //    MskSsn.Text = MskSsn.Text.Replace(" ", "");
            //    {
            //        this.MskSsn.LostFocus -= new System.EventHandler(this.CommonTextField_LostFocus);
            //        string Tmp_SSN_Str = "000000000".Substring(MskSsn.Text.Trim().Length, 9 - MskSsn.Text.Trim().Length) + MskSsn.Text.Trim();
            //        MskSsn.Text = Tmp_SSN_Str;
            //        this.MskSsn.LostFocus += new System.EventHandler(this.CommonTextField_LostFocus);

            //        if (Tmp_SSN_Str != "000000000")
            //        {
            //            if (Tmp_SSN_Str.Length != 9)
            //                return;

            //            List<APPTSCHEDULEEntity> propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, string.Empty, string.Empty, MskSsn.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            //            if (propAPPtSchedulelist.Count > 0)
            //            {
            //                string Tmp_Time = propAPPtSchedulelist[0].Time.Trim();
            //                switch (Tmp_Time.Length)
            //                {
            //                    case 1:
            //                        if (Tmp_Time == "0")
            //                            Tmp_Time = "0000" + Tmp_Time; break;
            //                    case 2: Tmp_Time = "00" + Tmp_Time; break;
            //                    case 3: Tmp_Time = "0" + Tmp_Time; break;
            //                }
            //                Tmp_Time = Tmp_Time.Substring(0, 2) + ":" + Tmp_Time.Substring(2, 2);

            //                if (int.Parse(Tmp_Time.Substring(0, 2)) > 12)
            //                    Tmp_Time = (int.Parse(Tmp_Time.Substring(0, 2)) - 12).ToString() + " PM";
            //                else
            //                    Tmp_Time += " AM";

            //                MessageBox.Show("Client already scheduled appointment On " + (Convert.ToDateTime(propAPPtSchedulelist[0].Date)).ToShortDateString() + "  " + Tmp_Time + "\n in the Site : " + propAPPtSchedulelist[0].Site, "CAP Systems", MessageBoxButtons.OK);
            //            }
            //            else
            //            {
            //                List<CaseMstSnpEntity> CaseSnpEntityList = _model.CaseMstData.GetSSNSearch("ALL", MskSsn.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "N", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, string.Empty);
            //                if (CaseSnpEntityList.Count > 0)
            //                {
            //                    TxtLstName.Text = CaseSnpEntityList[0].NameixLast;
            //                    TxtFrstName.Text = CaseSnpEntityList[0].NameixFi;
            //                    MskPhone.Text = CaseSnpEntityList[0].Area + CaseSnpEntityList[0].Phone;
            //                    TxtHn.Text = CaseSnpEntityList[0].Hn;

            //                    TxtStreet.Text = CaseSnpEntityList[0].Street;
            //                    TxtSf.Text = CaseSnpEntityList[0].Mst_Suffix;
            //                    TxtApt.Text = CaseSnpEntityList[0].Mst_Apt;
            //                    TxtFlr.Text = CaseSnpEntityList[0].MsT_Floor;
            //                    TxtCity.Text = CaseSnpEntityList[0].City;
            //                    TxtZip.Text = CaseSnpEntityList[0].MsT_Zip;
            //                    TxtZip1.Text = CaseSnpEntityList[0].Mst_Zip_Plus;
            //                    MskCell.Text = CaseSnpEntityList[0].Mst_Cell_Phone;

            //                    CommonFunctions.SetComboBoxValue(CmbGen, CaseSnpEntityList[0].Snp_Sex);
            //                    CommonFunctions.SetComboBoxValue(CmbHtSrc, CaseSnpEntityList[0].Mst_Heating_Source);
            //                    CommonFunctions.SetComboBoxValue(CmbWorker, CaseSnpEntityList[0].Mst_Intake_Worker);
            //                }
            //            }
            //        }

            //        Get_MST_SNP_SSN_Status();
            //        //MessageBox.Show(Lpb_Error_Alter, "CAP Systems");
            //    }
            //}
        }

        string Lpb_Error_Alter = "";
        private bool Get_MST_SNP_SSN_Status()
        {
            bool Can_Save = true;

            Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
            Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@Agy", BaseForm.BaseAgency));
            sqlParamList.Add(new SqlParameter("@Dept", BaseForm.BaseDept));
            sqlParamList.Add(new SqlParameter("@Prog", BaseForm.BaseProg));
            sqlParamList.Add(new SqlParameter("@Year", string.Empty));
            sqlParamList.Add(new SqlParameter("@Ssn", string.Empty));//MskSsn.Text.Trim()));
            DataSet Ds = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[TMSAPPT_SSN_Search_In_MSTSNP]");
            if (Ds.Tables.Count > 0)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["MST_INTAKE_DATE"].ToString().Trim()))
                    {
                        Lbl_Intake_Info.Text = Lpb_Error_Alter = "Application: " + Ds.Tables[0].Rows[0]["MST_APP_NO"].ToString() + "  SSN: " + Ds.Tables[0].Rows[0]["MST_SSN"].ToString() + " is in-house as of " + CommonFunctions.FormatDateString(Ds.Tables[0].Rows[0]["MST_INTAKE_DATE"].ToString());
                        if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["LPB_CERTIFIED_STATUS"].ToString().Trim()))
                        {
                            if (Ds.Tables[0].Rows[0]["LPB_CERTIFIED_STATUS"].ToString().Trim() == "98")
                                Lbl_LPB_Info.Text = Lpb_Error_Alter = "Already applied: DENIED---";
                            else if (Ds.Tables[0].Rows[0]["LPB_CERTIFIED_STATUS"].ToString().Trim() == "99")
                                Lbl_LPB_Info.Text = Lpb_Error_Alter = "Already applied: approved for $" + Ds.Tables[0].Rows[0]["LPB_AMOUNT"].ToString().Trim();
                        }
                        Can_Save = false;

                        if (!string.IsNullOrEmpty(Lbl_Intake_Info.Text.Trim()))
                            Lbl_Intake_Info.Visible = true;

                        if (!string.IsNullOrEmpty(Lbl_LPB_Info.Text.Trim()))
                            Lbl_LPB_Info.Visible = true;
                    }
                }
            }
            return Can_Save;
        }


        private void Clear_RequiredIcons()
        {
            _errorProvider.SetError(TxtSiteCode, null);
            _errorProvider.SetError(TxtState, null);
            _errorProvider.SetError(TxtLstName, null);
            _errorProvider.SetError(TxtFrstName, null);
            _errorProvider.SetError(TxtCity, null);
            _errorProvider.SetError(DtpCntrctDt, null);
            _errorProvider.SetError(dtBirth, null);
            _errorProvider.SetError(cmbClientStatus, null);
            _errorProvider.SetError(cmbLang, null);
            _errorProvider.SetError(cmbStatus, null);
            _errorProvider.SetError(MskCell, null);
            _errorProvider.SetError(CmbWorker, null);
            Lbl_Reserved.Visible = false;
        }

        //   Ending of Form Validation 


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //this.Close();
            if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "N" && NowSch_Slot_Flag)
            {
                List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity(true);
                if (ApptRecord.Count > 0)
                {
                    UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
                    UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
                    UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
                    UpdateEntity.Year = string.Empty;

                    UpdateEntity.Site = TxtSiteCode.Text;
                    UpdateEntity.Date = AppDate.Value.ToShortDateString();
                    UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
                    UpdateEntity.SlotNumber = SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString();
                    UpdateEntity.Mode = "Delete";

                    if ((_model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity)))
                    {

                        //if (_model.TMS00110Data.Delete_TMSAPPT(M_Hierarchy + M_Year, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), SchAppGrid.CurrentRow.Cells["KeySsn"].Value.ToString()))
                        NowSch_Slot_Flag = false;
                    }
                }
            }

            if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y")
            {
                APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity(true);
                UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
                UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
                UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
                UpdateEntity.Year = string.Empty;

                UpdateEntity.Site = TxtSiteCode.Text;
                UpdateEntity.Date = AppDate.Value.ToShortDateString();
                UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
                UpdateEntity.SlotNumber = SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString();

                UpdateEntity.EditBy =
                UpdateEntity.EditTime =
                UpdateEntity.Zip1 =
                UpdateEntity.Zip2 = null;

                UpdateEntity.LstcOperation = BaseForm.UserID;
                UpdateEntity.Mode = "EDITBY";
                _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);
            }

            Enable_DisableBottomControls(false);
            Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
            Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
            SchAppGrid_SelectionChanged(SchAppGrid, EventArgs.Empty);
            Btn_Update.Visible = BtnCancel.Visible = false;
            SchAppGrid.Enabled = true;

            if (Mode != "Add")
                TxtSiteCode.Enabled = BtnSearch.Enabled = AppDate.Enabled = false;
        }

        private void Hepl_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Scheduled Appointments");
        }

        private void BtnSsnSearch_Click(object sender, EventArgs e)
        {

            if (TxtLstName.Text == string.Empty && TxtFrstName.Text == string.Empty && dtBirth.Checked == false)
            {
                List<CaseMstSnpEntity> mstsnplist = new List<CaseMstSnpEntity>();
                SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, Privileges, mstsnplist, TxtFrstName.Text, TxtLstName.Text, string.Empty);
                SSNSearchForm.FormClosed += new FormClosedEventHandler(On_AsnSearchForm_Closed);
                SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
                SSNSearchForm.ShowDialog();
            }
            else
            {
                string strDOB = string.Empty;
                if (dtBirth.Checked)
                    strDOB = dtBirth.Value.ToShortDateString();
                List<CaseMstSnpEntity> CaseSnpEntityList = _model.CaseMstData.GetSSNSearch("ALL", string.Empty, TxtFrstName.Text, TxtLstName.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "N", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, strDOB);
                if (CaseSnpEntityList.Count > 0)
                {
                    if (CaseSnpEntityList.Count == 1)
                    {
                        TxtLstName.Text = CaseSnpEntityList[0].NameixLast;
                        TxtFrstName.Text = CaseSnpEntityList[0].NameixFi;
                        MskPhone.Text = CaseSnpEntityList[0].Area + CaseSnpEntityList[0].Phone;
                        TxtHn.Text = CaseSnpEntityList[0].Hn;

                        TxtStreet.Text = CaseSnpEntityList[0].Street;
                        TxtSf.Text = CaseSnpEntityList[0].Mst_Suffix;
                        TxtApt.Text = CaseSnpEntityList[0].Mst_Apt;
                        TxtFlr.Text = CaseSnpEntityList[0].MsT_Floor;
                        TxtCity.Text = CaseSnpEntityList[0].City;
                        TxtZip.Text = CaseSnpEntityList[0].MsT_Zip;
                        TxtZip.Text = "00000".Substring(0, 5 - TxtZip.Text.Trim().Length) + TxtZip.Text;

                        TxtZip1.Text = CaseSnpEntityList[0].Mst_Zip_Plus;
                        TxtZip1.Text = "0000".Substring(0, 4 - TxtZip1.Text.Trim().Length) + TxtZip1.Text;
                        MskCell.Text = CaseSnpEntityList[0].Mst_Cell_Phone;
                        TxtEMail.Text = CaseSnpEntityList[0].Mst_Email;
                        TxtState.Text = CaseSnpEntityList[0].State;
                        if (CaseSnpEntityList[0].AltBdate != string.Empty)
                        {
                            dtBirth.Value = Convert.ToDateTime(CaseSnpEntityList[0].AltBdate);
                            dtBirth.Checked = true;
                        }
                        CommonFunctions.SetComboBoxValue(CmbGen, CaseSnpEntityList[0].Snp_Sex);
                        CommonFunctions.SetComboBoxValue(CmbHtSrc, CaseSnpEntityList[0].Mst_Heating_Source);
                        //CommonFunctions.SetComboBoxValue(CmbWorker, CaseSnpEntityList[0].Mst_Intake_Worker);
                        CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                        CommonFunctions.SetComboBoxValue(cmbLang, CaseSnpEntityList[0].Mst_Language);

                        if (PropApptschedule.Count > 0)
                        {
                            List<APPTSCHEDULEEntity> SelRecs = new List<APPTSCHEDULEEntity>();
                            if (CaseSnpEntityList[0].AltBdate != string.Empty)
                                SelRecs = PropApptschedule.FindAll(u => u.LastName.Trim() == TxtLstName.Text.Trim() && u.FirstName.Trim() == TxtFrstName.Text.Trim() && u.DOB.Trim() == CaseSnpEntityList[0].AltBdate.Trim());

                            if (SelRecs.Count > 0)
                                CommonFunctions.SetComboBoxValue(cmbClientStatus, "R");
                        }
                    }
                    else
                    {
                        SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, Privileges, CaseSnpEntityList, TxtFrstName.Text, TxtLstName.Text, strDOB);
                        SSNSearchForm.FormClosed += new FormClosedEventHandler(On_AsnSearchForm_Closed);
                        SSNSearchForm.StartPosition=FormStartPosition.CenterScreen;
                        SSNSearchForm.ShowDialog();

                    }
                }
                else
                {
                   AlertBox.Show("Search results not found", MessageBoxIcon.Warning);
                }
            }
        }

        private void On_AsnSearchForm_Closed(object sender, FormClosedEventArgs e)
        {
            SSNSearchForm form = sender as SSNSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity caseMstSnpEntity = null;
                caseMstSnpEntity = form.GetSelectedRow();
                if (caseMstSnpEntity != null)
                {
                    if (caseMstSnpEntity.AltBdate != string.Empty)
                    {
                        dtBirth.Value = Convert.ToDateTime(caseMstSnpEntity.AltBdate);
                        dtBirth.Checked = true;
                    }

                    TxtLstName.Text = caseMstSnpEntity.NameixLast;
                    TxtFrstName.Text = caseMstSnpEntity.NameixFi;
                    MskPhone.Text = caseMstSnpEntity.Area + caseMstSnpEntity.Phone;
                    TxtHn.Text = caseMstSnpEntity.Hn;

                    TxtStreet.Text = caseMstSnpEntity.Street;
                    TxtSf.Text = caseMstSnpEntity.Mst_Suffix;
                    TxtApt.Text = caseMstSnpEntity.Mst_Apt;
                    TxtFlr.Text = caseMstSnpEntity.MsT_Floor;
                    TxtCity.Text = caseMstSnpEntity.City;
                    TxtZip.Text = caseMstSnpEntity.MsT_Zip;
                    TxtZip.Text = "00000".Substring(0, 5 - TxtZip.Text.Trim().Length) + TxtZip.Text;

                    TxtZip1.Text = caseMstSnpEntity.Mst_Zip_Plus;
                    TxtZip1.Text = "0000".Substring(0, 4 - TxtZip1.Text.Trim().Length) + TxtZip1.Text;

                    MskCell.Text = caseMstSnpEntity.Mst_Cell_Phone;
                    TxtEMail.Text = caseMstSnpEntity.Mst_Email;
                    TxtState.Text = caseMstSnpEntity.State;
                    CommonFunctions.SetComboBoxValue(CmbGen, caseMstSnpEntity.Snp_Sex);
                    CommonFunctions.SetComboBoxValue(CmbHtSrc, caseMstSnpEntity.Mst_Heating_Source);
                    //CommonFunctions.SetComboBoxValue(CmbWorker, caseMstSnpEntity.Mst_Intake_Worker);

                    CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                    CommonFunctions.SetComboBoxValue(cmbLang, caseMstSnpEntity.Mst_Language);

                    if (PropApptschedule.Count > 0)
                    {
                        List<APPTSCHEDULEEntity> SelRecs = new List<APPTSCHEDULEEntity>();
                        if (caseMstSnpEntity.AltBdate != string.Empty)
                            SelRecs = PropApptschedule.FindAll(u => u.LastName.Trim() == TxtLstName.Text.Trim() && u.FirstName.Trim() == TxtFrstName.Text.Trim() && u.DOB.Trim() == caseMstSnpEntity.AltBdate.Trim());

                        if (SelRecs.Count > 0)
                            CommonFunctions.SetComboBoxValue(cmbClientStatus, "R");
                        else
                            CommonFunctions.SetComboBoxValue(cmbClientStatus, "R");
                    }

                    Get_MST_SNP_SSN_Status();
                }
            }
        }


        string strNameFormat = null;
        string strCwFormat = null;
        List<ListItem> propListCaseWorker;
        private void FillCaseWroker()
        {
            //DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(M_Hierarchy.Substring(0, 2), M_Hierarchy.Substring(2, 2), M_Hierarchy.Substring(4, 2));
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(M_Hierarchy.Substring(0, 2), "**", "**");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            CmbWorker.Items.Clear(); CmbWorker.ColorMember = "FavoriteColor";
            propListCaseWorker = new List<ListItem>();
            CmbWorker.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            DataTable dt = ds1.Tables[0];
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    //if ((Mode == "Add" && dr["PWH_INACTIVE"].ToString().Trim() == "N")) && (Mode == "Edit"))
                    propListCaseWorker.Add(new ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Black));
                }
            }
            CmbWorker.Items.AddRange(propListCaseWorker.ToArray());
            CmbWorker.SelectedIndex = 0;
        }



        private void BtnIncSource_Click(object sender, EventArgs e)
        {
            Sub_IncomeSource_Selection IncSource = new Sub_IncomeSource_Selection("APPT0002", txtIncSrc.Text);
            IncSource.StartPosition = FormStartPosition.CenterScreen;
            IncSource.FormClosed += new FormClosedEventHandler(On_IncSource_Closed);
            IncSource.ShowDialog();
        }

        private void On_IncSource_Closed(object sender, FormClosedEventArgs e)
        {
            Sub_IncomeSource_Selection form = sender as Sub_IncomeSource_Selection;
            if (form.DialogResult == DialogResult.OK)
                txtIncSrc.Text = form.GetSelectedSources();
        }

        private void SchAppGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0 && Mode.Equals("Delete") && SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y")
            //{
            //    int ColIdx = 0;
            //    int RowIdx = 0;
            //    ColIdx = SchAppGrid.CurrentCell.ColumnIndex;
            //    RowIdx = SchAppGrid.CurrentCell.RowIndex;
            //}
        }

        private void DeleteSel_Appoint()
        {
            bool Tmptrue = true, Delete_Statue = true, Any_Rec_deleted = false;

            foreach (DataGridViewRow dr in SchAppGrid.Rows)
            {
                if (dr.Cells["Delete"].Value.ToString() == Tmptrue.ToString())
                {
                    APPTSCHEDULEEntity apptschddel = dr.Tag as APPTSCHEDULEEntity;
                    if (apptschddel != null)
                    {
                        apptschddel.Mode = "DELETE";
                        if (!(_model.TMS00110Data.InsertUpdateDelAPPTSCHED(apptschddel)))
                            Delete_Statue = false;

                        Any_Rec_deleted = true;
                    }
                }
            }
            if (Any_Rec_deleted)
            {
                if (Delete_Statue)
                {
                    AlertBox.Show("Appointment(s) Deleted Successfully");
                    Refresh_Control = true;
                    //this.DialogResult = DialogResult.OK;
                }
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
                AppDate_LostFocus(AppDate, e);
            }
            else
                AlertBox.Show("Please select at least One Appointment to Delete", MessageBoxIcon.Warning);
        }


        private void BtnZip_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, TxtZip.Text);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    TxtZip1.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    TxtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    TxtState.Text = zipcodedetais.Zcrstate;
                    TxtCity.Text = zipcodedetais.Zcrcity;
                    //SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                    //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                }
            }
        }

        private void TMS00110Form_Load(object sender, EventArgs e)
        {
            //this.Size = new System.Drawing.Size(768, 598);
            switch (Mode)
            {
                case "Edit":
                    this.Text =  Privileges.PrivilegeName;
                    TxtSiteCode.Text = SchSite;
                    if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                        BtnNextSlot.Visible = true;
                    else
                        BtnNextSlot.Visible = false;
                    AppDate.Value = Convert.ToDateTime(SchDate);
                    AppDate.Checked = true;
                    AppDate.Focus();
                    AppDate_LostFocus(AppDate, e);
                    this.Name.Width = 250;
                    break;
                case "Add":
                    this.Text =  Privileges.PrivilegeName;
                    TxtSiteCode.Enabled = true;
                    AppDate.Enabled = true;
                    BtnSearch.Enabled = true;
                    //LblAppMode.Text = "Add Appointment";
                    this.Name.Width = 250;
                    if (!string.IsNullOrEmpty(BaseForm.UserProfile.Site.Trim()))
                    {
                        if (BaseForm.UserProfile.Site.Trim() != "****")
                        {
                            Priv_Sel_Site = TxtSiteCode.Text = BaseForm.UserProfile.Site.Trim().Substring(2, BaseForm.UserProfile.Site.Trim().Length - 2);
                            if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                                BtnNextSlot.Visible = true;
                            else
                                BtnNextSlot.Visible = false;
                            AppDate_LostFocus(AppDate, EventArgs.Empty);
                        }
                    }
                    break;
                case "Delete":
                    this.Text = Privileges.PrivilegeName;
                    TxtSiteCode.Text = SchSite;
                    this.Delete.Visible = true;
                    //LblAppMode.Visible = false;
                    AppDate.Value = Convert.ToDateTime(SchDate);
                    AppDate.Checked = true;
                    AppDate.Focus();
                    AppDate_LostFocus(AppDate, e);
                    Btn_Update.Text = "Delete";
                    Enable_DisableBottomControls(false);
                    break;
            }
        }

        private void TxtSiteCode_LostFocus(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                BtnNextSlot.Visible = true;
            else
                BtnNextSlot.Visible = false;

            if (Priv_Sel_Site != TxtSiteCode.Text && PrivApp_date != AppDate.Value)
                AppDate_LostFocus(AppDate, EventArgs.Empty);

            Priv_Sel_Site = TxtSiteCode.Text;
        }

        int Priv_Selected_Row = 0;
        private void Pb_Add_Click(object sender, EventArgs e)
        {
            // Check Now Sch Status of the slot, If Yes Visible False Save and Cancel
            if (Get_Now_Sch_Status())
            {
                Set_Menu_Buttons_Visibility("Add");
                CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                CommonFunctions.SetComboBoxValue(cmbStatus, "06");
                DtpCntrctDt.Checked = true;
            }
            else
            {
                Pb_Add.Visible = Pb_Edit.Visible = Pb_Delete.Visible = false;
                SchAppGrid.Enabled = false;
                Enable_DisableBottomControls(true);
                BtnCancel.Visible = Btn_Update.Visible = true;
                Now_Sch_record();
                CommonFunctions.SetComboBoxValue(CmbWorker, BaseForm.UserProfile.CaseWorker);
                CommonFunctions.SetComboBoxValue(cmbStatus, "06");
                DtpCntrctDt.Checked = true;
            }
            //            BtnIncSource.Enabled = BtnZip.Enabled = true;

            // Save APPT Record with Now Sch data
        }



        private void Pb_Edit_Click(object sender, EventArgs e)
        {
            if (Update_Edit_Stamping())
            {
                Pb_Add.Visible = Pb_Edit.Visible = Pb_Delete.Visible = false;
                Enable_DisableBottomControls(true);
                BtnCancel.Visible = Btn_Update.Visible = true;
                SchAppGrid.Enabled = BtnSsnSearch.Enabled = false; // dtBirth.Enabled = false;
            }

            //            BtnIncSource.Enabled = BtnZip.Enabled = true;
        }

        private bool Update_Edit_Stamping()
        {
            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity();
            if (ApptRecord.Count > 0)
                UpdateEntity = ApptRecord[0];
            else
            {
                AlertBox.Show("Record already Deleted by another user!!!", MessageBoxIcon.Warning);
                //AppDate_LostFocus(AppDate, e);
                return false;
            }

            bool Can_Update_Edit_Stamping = true;
            if (!string.IsNullOrEmpty(ApptRecord[0].EditBy.Trim()))
            {
                if (ApptRecord[0].EditBy.Trim() != BaseForm.UserID)
                {
                    DateTime Current_Time = DateTime.Today;
                    DateTime Edit_Time = Convert.ToDateTime(ApptRecord[0].EditTime.Trim());

                    if ((Current_Time - Edit_Time).TotalHours < 1)
                    {
                        AlertBox.Show("Slot is being Edited by: " + UpdateEntity.EditBy, MessageBoxIcon.Warning);
                        Can_Update_Edit_Stamping = false;
                    }
                }
            }

            if (Can_Update_Edit_Stamping)
            {
                UpdateEntity.Zip1 =
                UpdateEntity.Zip2 = null;

                UpdateEntity.EditBy =
                UpdateEntity.LstcOperation = BaseForm.UserID;
                UpdateEntity.Mode = "EDITBY";
                _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);

            }

            return Can_Update_Edit_Stamping;
        }


        private void EnabDisable_Top_Cntls(bool Status)
        {
            BtnRefSch.Enabled = BtnNextSlot.Enabled = BtnMemSearch.Enabled = Status;
            if (!Status)
                TxtSiteCode.Enabled = BtnSearch.Enabled = AppDate.Enabled = Status;
            else
            {
                if (Mode == "Add")
                    TxtSiteCode.Enabled = BtnSearch.Enabled = AppDate.Enabled = Status;
            }
        }


        private bool Get_Now_Sch_Status()
        {
            bool NowSch_Status = false;
            Lbl_Now_Sch.Visible = false;
            Priv_Selected_Row = SchAppGrid.CurrentRow.Index;
            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            if (ApptRecord.Count > 0)
            {
                if (ApptRecord[0].SchdType == "3")
                    Lbl_Now_Sch.Visible = NowSch_Status = true;
                else if (ApptRecord[0].SchdType == "1")
                {
                    if (ApptRecord[0].SlotType == "A")
                    { }
                    else
                    {
                        NowSch_Status = true;
                        AlertBox.Show("Slot already booked by another user!!!", MessageBoxIcon.Warning);
                        AppDate_LostFocus(AppDate, e);
                    }
                }
            }

            return NowSch_Status;
        }

        bool NowSch_Slot_Flag = false;
        private void Now_Sch_record()
        {
            APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity();
            UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
            UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
            UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
            UpdateEntity.Year = string.Empty;

            UpdateEntity.Site = TxtSiteCode.Text;
            UpdateEntity.Date = AppDate.Value.ToShortDateString();
            UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
            UpdateEntity.SlotNumber = SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString();

            UpdateEntity.SchdType = "3";

            UpdateEntity.LastName = "Now scheduling";
            UpdateEntity.TemplateID = "0";
            UpdateEntity.FirstName = " ";
            //UpdateEntity.TelAreaCode =
            UpdateEntity.SsNumber =
            UpdateEntity.TelNumber =
            UpdateEntity.HNo =
            UpdateEntity.Street =

            UpdateEntity.Suffix =
            UpdateEntity.Apt =
            UpdateEntity.Floor =
            UpdateEntity.City =
            UpdateEntity.State =
            UpdateEntity.Zip1 =
            UpdateEntity.Zip2 =

            UpdateEntity.Sex =
            UpdateEntity.CaseWorker =
            UpdateEntity.HeatSource =
            UpdateEntity.SourceIncome =
            UpdateEntity.ContactPerson =


            //UpdateEntity.CellAreaCode =
            UpdateEntity.CellProvider =
            UpdateEntity.CellNumber =
            UpdateEntity.ContactDate =
            UpdateEntity.EditTime =
            UpdateEntity.EditBy =
            null;

            UpdateEntity.LstcOperation = BaseForm.UserID;
            UpdateEntity.AddOperator = BaseForm.UserID;
            UpdateEntity.Mode = "Add";

            if (_model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity))
                NowSch_Slot_Flag = true;
        }

        private void TMS00110Form_FormClosed(object sender, FormClosedEventArgs e)
        {

            APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity(true);
            UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
            UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
            UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
            UpdateEntity.Year = string.Empty;

            UpdateEntity.Site = TxtSiteCode.Text;
            UpdateEntity.Date = AppDate.Value.ToShortDateString();
            UpdateEntity.Time = "000";
            UpdateEntity.SlotNumber = "0";
            UpdateEntity.LstcOperation = BaseForm.UserID;
            UpdateEntity.AddOperator = BaseForm.UserID;

            UpdateEntity.Mode = "DELETEBY";
            _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);

            if (Refresh_Control)
                this.DialogResult = DialogResult.OK;
        }

        private void Pb_Delete_Click(object sender, EventArgs e)
        {
            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity();
            if (ApptRecord.Count > 0)
                UpdateEntity = ApptRecord[0];
            else
            {
                AlertBox.Show("Record already Deleted by another user!!!", MessageBoxIcon.Warning);
                //AppDate_LostFocus(AppDate, e);
                return;
            }

            bool Can_Update_Delete_Stamping = true;
            if (!string.IsNullOrEmpty(ApptRecord[0].EditBy.Trim()))
            {
                if (ApptRecord[0].EditBy.Trim() != BaseForm.UserID)
                {
                    DateTime Current_Time = DateTime.Today;
                    DateTime Edit_Time = Convert.ToDateTime(ApptRecord[0].EditTime.Trim());

                    if ((Current_Time - Edit_Time).TotalHours < 1)
                    {
                        AlertBox.Show("Slot is being Edited by: " + UpdateEntity.EditBy, MessageBoxIcon.Warning);
                        Can_Update_Delete_Stamping = false;
                    }
                }
            }

            if (Can_Update_Delete_Stamping)
                MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:Delete_Selcted_App_Row);
        }


        private void Delete_Selcted_App_Row(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                Delete_Selcted_Appoint();
            }
        }

        bool Refresh_Control = false;

        private void MskSsn_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnHist_Click(object sender, EventArgs e)
        {
            APPT0002Cancelled_Form APPT0002_Edit = new APPT0002Cancelled_Form(BaseForm, Privileges, "Edit", TxtSiteCode.Text, AppDate.Text);
            //APPT0002_Edit.FormClosed += new Form.FormClosedEventHandler(On_Edit_Delete_Closed);
            APPT0002_Edit.StartPosition = FormStartPosition.CenterScreen;
            APPT0002_Edit.ShowDialog();
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            contextMenu1.MenuItems.Clear();

            if (BaseForm.UserProfile.Security == "B" || BaseForm.UserProfile.Security == "P")
            {
                if (SchAppGrid.Rows.Count > 0)
                {
                    var MaxSlot = (from DataGridViewRow row in SchAppGrid.Rows
                                   where row.Cells["Time"].FormattedValue.ToString() == SchAppGrid.CurrentRow.Cells["Time"].Value.ToString()
                                   select Convert.ToInt32(row.Cells["SlotNo"].FormattedValue)).Max().ToString();

                    //var foundRows = this.SchAppGrid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["Time"].FormattedValue.ToString() == SchAppGrid.CurrentRow.Cells["Time"].Value.ToString() && row.Cells["Name"].Value.ToString().Trim() != "");

                    //var FoundCount = foundRows.Count();

                    var FilledSlots = (from DataGridViewRow row in SchAppGrid.Rows
                                       where row.Cells["Time"].FormattedValue.ToString() == SchAppGrid.CurrentRow.Cells["Time"].Value.ToString() && row.Cells["Name"].FormattedValue.ToString().Trim() != ""
                                       select Convert.ToInt32(row.Cells["SlotNo"].FormattedValue)).Count().ToString();

                    if (MaxSlot == SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString() && FilledSlots == MaxSlot)
                    {
                        MenuItem Menu_L1 = new MenuItem();
                        Menu_L1.Text = "Insert New Slot";
                        Menu_L1.Tag = "I";
                        contextMenu1.MenuItems.Add(Menu_L1);
                    }

                }
            }
        }

        private void SchAppGrid_MenuClick(object objSource, MenuItemEventArgs objArgs)
        {
            string[] Split_Array = new string[2];
            if (objArgs.MenuItem.Tag is string)
            {
                Split_Array = Regex.Split(objArgs.MenuItem.Tag.ToString(), " ");
                if (Split_Array[0].ToString() == "I")
                {
                    //DataGridViewRow row = (DataGridViewRow)SchAppGrid.CurrentRow.Clone();
                    //row.Cells["Name"].Value = null;
                    //row.Cells["DOB"].Value = null;
                    //row.Cells["Telephone"].Value = null;
                    //row.Cells["Person"].Value = null;
                    //row.Cells["SlotNo"].Value = Convert.ToInt32(SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString()) +1;
                    ////row.Cells["KeyTime"].Value = string.Empty;
                    //row.Cells["KeySsn"].Value = null;
                    //row.Cells["Rec_Type"].Value = "1";
                    //row.Cells["SlotType"].Value = "A";

                    SchAppGrid.Rows.Insert(SchAppGrid.CurrentRow.Index + 1, false, SchAppGrid.CurrentRow.Cells["Time"].Value.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Convert.ToInt32(SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString()) + 1, Convert.ToInt32(SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString()), "N", string.Empty, "1", SchAppGrid.CurrentRow.Cells["gvt_TempId"].Value.ToString(), "A");

                    //SchAppGrid.Rows.Add(row);

                    APPTSCHEDULEEntity UpdateEntity = new Model.Objects.APPTSCHEDULEEntity();
                    UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
                    UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
                    UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
                    UpdateEntity.Year = string.Empty;

                    UpdateEntity.Site = TxtSiteCode.Text;
                    UpdateEntity.Date = AppDate.Value.ToShortDateString();
                    UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
                    UpdateEntity.SlotNumber = (Convert.ToInt32(SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString()) + 1).ToString();
                    UpdateEntity.TemplateID = SchAppGrid.CurrentRow.Cells["gvt_TempId"].Value.ToString();
                    UpdateEntity.SchdType = "1";
                    UpdateEntity.SlotType = "A";

                    UpdateEntity.DOB = UpdateEntity.TelNumber = UpdateEntity.HNo = UpdateEntity.Suffix = UpdateEntity.Apt =
                    UpdateEntity.Floor = UpdateEntity.City = UpdateEntity.State = UpdateEntity.Zip1 =
                    UpdateEntity.Zip2 = UpdateEntity.Sex = UpdateEntity.CaseWorker = UpdateEntity.HeatSource =
                    UpdateEntity.SourceIncome = UpdateEntity.CellNumber = UpdateEntity.Street = UpdateEntity.CellProvider =
                    UpdateEntity.EditTime = UpdateEntity.EditBy = UpdateEntity.Email = null;

                    UpdateEntity.LstcOperation = UpdateEntity.ContactPerson =
                    UpdateEntity.AddOperator = BaseForm.UserID;
                    UpdateEntity.Mode = "Add";

                    _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);


                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void chkExcludeSlots_CheckedChanged(object sender, EventArgs e)
        {
            AppDate_LostFocus(sender, e);
        }

        private void Delete_Selcted_Appoint()
        {
            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            APPTSCHEDULEEntity UpdateEntity = new APPTSCHEDULEEntity();
            if (ApptRecord.Count > 0)
            {
                UpdateEntity = ApptRecord[0];
                UpdateEntity.Mode = "Delete";

                if ((_model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity)))
                {
                    Refresh_Control = false;
                    AppDate_LostFocus(AppDate, e);
                    AlertBox.Show("Record Deleted Successfully");
                }
                else
                    AlertBox.Show("Failed to Delete", MessageBoxIcon.Warning);
            }
        }

        private void MskPhone_LostFocus(object sender, EventArgs e)
        {
            if (sender == MskPhone)
            {
                if (!string.IsNullOrEmpty(MskPhone.Text.Trim()))
                {
                    string Tmp_Str = MskPhone.Text.Replace(" ", "");
                    MskPhone.Text = "0000000000".Substring(0, 10 - Tmp_Str.Trim().Length) + Tmp_Str;
                }
            }
            else if (sender == MskCell)
            {
                if (!string.IsNullOrEmpty(MskCell.Text.Trim()))
                {
                    string Tmp_Str = MskCell.Text.Replace(" ", "");
                    MskCell.Text = "0000000000".Substring(0, 10 - Tmp_Str.Trim().Length) + Tmp_Str;
                }
            }
        }

        string[] Next_Day_Avail_Slot_Key = new string[2];
        //private void AppDate_LostFocus_For_Next_Day_Available_Slot()
        //{
        //    Pb_Add.Visible = Pb_Edit.Visible = Pb_Delete.Visible = false;
        //    ClearBottomControls();
        //    Next_Day_Avail_Slot_Key[0] = Next_Day_Avail_Slot_Key[1] = "";
        //    if (!(string.IsNullOrEmpty(TxtSiteCode.Text)))
        //    {
        //        DateTime App_date = new DateTime();
        //        App_date = AppDate.Value;
        //        int Week_Day = Get_WeekDay_Name(App_date.DayOfWeek.ToString());

        //        DataSet ds = Captain.DatabaseLayer.TMS00110DB.GetTMSAPCN_SlotDetails(M_Hierarchy, M_Year, TxtSiteCode.Text.Trim(), AppDate.Value.ToShortDateString());
        //        DataTable dt = new DataTable();
        //        if (ds.Tables.Count > 0)
        //            dt = ds.Tables[0];
        //        int TmpCount = 0, Sel_SSN_Rec_Index = 0; Tot_Sch_Slot_Cnt = 0;
        //        bool Open_Day_With_Slots = true; Process_Template_Date = Process_Template_Type = "";
        //        if (dt.Rows.Count > 0)
        //        {
        //            //Date_Appt_List.Clear();
        //            DataSet dsAppt = Captain.DatabaseLayer.TMS00110DB.Browse_TMSAPPT(M_Hierarchy + M_Year, TxtSiteCode.Text.Trim(), AppDate.Value.ToShortDateString(), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, BaseForm.UserID);
        //            DataTable dtAppt = dsAppt.Tables[0];

        //            //foreach (DataRow Tmpdr in dtAppt.Rows)
        //            //    Date_Appt_List.Add(new TmsApptEntity(Tmpdr, ""));

        //            //DataRow dr1 = dt.Rows[0];
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string Period_table = dr["TotSlots"].ToString();
        //                int Noof_Open_Slots = int.Parse(dr["OpenSlots"].ToString());
        //                //int  Noof_Remaining_Slots = int.Parse(dr["RemSlots"].ToString());
        //                int Slots_Per_period = int.Parse(dr["SlotsPerPeriod"].ToString());
        //                string Day_table = dr["TMSAPCN_DAY_TABLE"].ToString();
        //                string Gbl_OpenClose = dr["Gbl_OpenClose"].ToString();
        //                Process_Template_Type = dr["TMSAPCN_TYPE"].ToString();
        //                Process_Template_Date = dr["TMSAPCN_DATE"].ToString();
        //                int Period = int.Parse(dr["Mins"].ToString());
        //                int Hours = 00, Mins = 00;
        //                string Disp_Hours = "00", Disp_Miin = "00", Display_date = null;
        //                string[] time;

        //                string Pass_Hours = "00", Pass_Min = "00";

        //                if (Gbl_OpenClose == "0")
        //                {
        //                    //if (!In_Search_Of_NextDay_Slot)
        //                    //    MessageBox.Show("Template is Closed for this Day", "CAP Systems", MessageBoxButtons.OK);
        //                    Open_Day_With_Slots = false;
        //                    break;
        //                }

        //                if (Day_table.Substring(Week_Day - 1, 1) == "0")
        //                {
        //                    //if (!In_Search_Of_NextDay_Slot)
        //                    //    MessageBox.Show(App_date.DayOfWeek.ToString() + " Is a Close Day, No Appointments Allowed", "CAP Systems", MessageBoxButtons.OK);
        //                    Open_Day_With_Slots = false;
        //                    break;
        //                }

        //                if (!Period_table.Contains("1"))
        //                {
        //                    //if (!In_Search_Of_NextDay_Slot)
        //                    //    MessageBox.Show(" No Slot is Opened for this Template", "CAP Systems", MessageBoxButtons.OK);
        //                    Open_Day_With_Slots = false;
        //                    break;
        //                }

        //                string AM_PM = "AM", Appt_Rec_Type = "";

        //                Slots_Array = new string[(Noof_Open_Slots * Slots_Per_period), Slots_Array_Colmns];   // Sindhe
        //                for (int i = 0; i < Period_table.Length; i++)
        //                {
        //                    if ((Mins >= 60) && (Mins % 60) >= 0)
        //                    {
        //                        Hours++;
        //                        Mins = 00;
        //                    }

        //                    if (Hours >= 12)
        //                        AM_PM = "PM";

        //                    Disp_Hours = Hours.ToString();
        //                    Pass_Hours = Hours.ToString();
        //                    if (Hours > 12)
        //                    {
        //                        Disp_Hours = (Hours - 12).ToString();
        //                        Pass_Hours = "00".Substring(0, 2 - Pass_Hours.Length) + Pass_Hours;
        //                    }

        //                    Pass_Min = Mins.ToString();
        //                    Disp_Miin = Pass_Min = "00".Substring(0, 2 - Pass_Min.Length) + Pass_Min;
        //                    Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;

        //                    if (Pass_Hours + Pass_Min == "000")
        //                    {
        //                        Pass_Hours = "0";
        //                        Pass_Min = string.Empty;
        //                    }

        //                    Appt_Rec_Type = "";
        //                    if (Period_table.Substring(i, 1) == "1")
        //                    {
        //                        for (int j = 1; j <= Slots_Per_period; j++)
        //                        {
        //                            Slots_Array[TmpCount, 0] = Disp_Hours + ":" + Disp_Miin + " " + AM_PM;
        //                            Slots_Array[TmpCount, 1] = j.ToString();
        //                            Slots_Array[TmpCount, 2] = Pass_Hours + Pass_Min; //Pass_Hours + Pass_Min + j.ToString();
        //                            Slots_Array[TmpCount, 3] = "N";
        //                            Slots_Array[TmpCount, 4] = Slots_Array[TmpCount, 5] = Slots_Array[TmpCount, 6] = null;
        //                            Slots_Array[TmpCount, 7] = Slots_Array[TmpCount, 8] = Slots_Array[TmpCount, 9] = null;
        //                            Slots_Array[TmpCount, 10] = Slots_Array[TmpCount, 11] = Slots_Array[TmpCount, 12] = null;

        //                            foreach (DataRow Tmpdr in dtAppt.Rows)
        //                            {
        //                                if (Slots_Array[TmpCount, 2] + Slots_Array[TmpCount, 1] == Tmpdr["TMSAPPT_TIME"].ToString() + Tmpdr["TMSAPPT_SLOT_NUMBER"].ToString())
        //                                {
        //                                    Slots_Array[TmpCount, 3] = "Y";
        //                                    break;
        //                                }
        //                            }
        //                            if (Slots_Array[TmpCount, 3] == "N")
        //                            {
        //                                Next_Day_Avail_Slot_Key[0] = Slots_Array[TmpCount, 0];
        //                                Next_Day_Avail_Slot_Key[1] = Slots_Array[TmpCount, 1];
        //                                goto Fill_Sel_Date_Slots;
        //                                //break;
        //                            }

        //                            TmpCount++;
        //                        }
        //                    }
        //                    //if (!string.IsNullOrEmpty(Next_Day_Avail_Slot_Key[0].Trim()))
        //                    //    break;

        //                    Mins += Period;
        //                }

        //                //if (!string.IsNullOrEmpty(Next_Day_Avail_Slot_Key[0].Trim()))
        //                //    break;
        //            }
        //            this.SchAppGrid.SelectionChanged += new System.EventHandler(this.SchAppGrid_SelectionChanged);

        //            PrivApp_date = AppDate.Value;
        //        }

        //    Fill_Sel_Date_Slots:
        //        if (!string.IsNullOrEmpty(Next_Day_Avail_Slot_Key[0].Trim()))
        //        {
        //            AppDate_LostFocus(AppDate, EventArgs.Empty);
        //            //BtnNextSlot_Click(BtnNextSlot, EventArgs.Empty);
        //        }

        //        Rec_Sel_From_SSN_Search = Just_Saved = false;
        //    }
        //}        

    }
}