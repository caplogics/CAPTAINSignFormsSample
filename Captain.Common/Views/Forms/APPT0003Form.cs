#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using Wisej.Web;

#endregion

namespace Captain.Common.Views.Forms
{

    public partial class APPT0003Form : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;
        #endregion

        public APPT0003Form(BaseForm baseform, PrivilegeEntity privileges, string mode, string sitecode, string strDate, string strMonth, string strYear)
        {
            InitializeComponent();
            BaseForm = baseform;
            Mode = mode;
            Privileges = privileges;
            SiteCode = sitecode;
            _model = new CaptainModel();


            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            MainMemu_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + "    "; //BaseForm.BaseYear.Trim();
            FillDropDowns();
            if (strDate != string.Empty)
            {
                FromDate.Value = Todate.Value = Convert.ToDateTime(strMonth + "/" + strDate + "/" + strYear);
            }
            else
            {
                var first = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), 1);
                FromDate.Value = first;

                var last = first.AddMonths(1).AddDays(-1);
                Todate.Value = last;
            }
            if (!string.IsNullOrEmpty(sitecode))
            {
                TxtSite.Text = SiteCode;
                CommonTextField_LostFocus(TxtSite, EventArgs.Empty);
            }
            BtnSave.Visible = false;
            switch (Mode)
            {
                case "Add":
                    this.Text = "Time/Slot Reserve" + " - Add";
                    BtnGetSlots.Visible = true;
                    BtnClearControls.Visible = true;
                    BtnSiteSearch.Visible = true;
                    SetDefaultTimeAddMode();
                    break;
                case "Edit":
                    this.Text = "Time/Slot Reserve" + " - Edit";

                    if (strDate != string.Empty)
                    {
                        Fill_ResrSlots_Edit_Delete();
                    }
                    break;
                case "View":
                    this.Text = "Time/Slot Reserve" + " - View";
                    panel3.Visible = false;
                    NewSoltsGrid.ReadOnly = true;
                    panel1.Enabled = false;
                    if (strDate != string.Empty)
                    {
                        Fill_ResrSlots_Edit_Delete();
                    }
                    break;
                case "Delete":
                    this.Text = "Time/Slot Reserve" + " - Delete";
                    BtnSave.Text = "&Delete Selected";
                    BtnSave.Size = new System.Drawing.Size(110,this.Height);
                    label12.Visible = false;
                    label8.Visible = false;
                    TxtReason.Visible = false;
                    if (strDate != string.Empty)
                    {
                        Fill_ResrSlots_Edit_Delete();
                    }
                    break;
            }
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string MainMemu_Hierarchy { get; set; }

        public string Mode { get; set; }

        public string SiteCode { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "TMS00120");

        }

        public List<APPTSCHEDULEEntity> propAPPtSchedulelist = new List<APPTSCHEDULEEntity>();
        private void Fill_Reserved_Slots()
        {
            propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSite.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        }


        private void FillDropDowns()
        {
            FromTime.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();

            int hour = 0, min = 0;
            string DispHours = null, Disp_Mins = null, AM_PM = "AM";
            listItem.Add(new ListItem("12:00 AM", "0"));
            for (int i = 0; i < 24; i++)
            {
                if (hour > 11)
                    AM_PM = "PM";
                DispHours = hour.ToString();

                if (hour > 12)
                    DispHours = (hour - 12).ToString();

                for (min = 5; min < 60;)
                {
                    Disp_Mins = min.ToString();
                    if (min == 0 || min == 5)
                        Disp_Mins = "0" + min.ToString();
                    if (hour == 0)
                        DispHours = "12";

                    listItem.Add(new ListItem(DispHours + ":" + Disp_Mins + " " + AM_PM, (int.Parse(hour.ToString() + Disp_Mins)).ToString()));
                    min += 5;
                }
                min = 0;
                hour++;
                if (hour > 11)
                    AM_PM = "PM";
                DispHours = (hour).ToString();
                if (hour > 12)
                    DispHours = (hour - 12).ToString();
                listItem.Add(new ListItem(DispHours + ":00 " + AM_PM, (int.Parse(hour.ToString() + "00")).ToString()));
            }
            FromTime.Items.AddRange(listItem.ToArray());
            ToTime.Items.AddRange(listItem.ToArray());


            FromSlots.Items.Clear();
            ToSlots.Items.Clear();
            listItem = new List<ListItem>();
            for (int i = 1; i < 10; i++)
                listItem.Add(new ListItem(i.ToString(), i.ToString()));

            FromSlots.Items.AddRange(listItem.ToArray());
            ToSlots.Items.AddRange(listItem.ToArray());

            FromTime.SelectedIndex = 96;
            ToTime.SelectedIndex = 240;
            FromSlots.SelectedIndex = 0;
            ToSlots.SelectedIndex = 2;


            cmbDayofweek.Items.Clear();
            List<ListItem> listItem2 = new List<ListItem>();
            listItem2.Add(new ListItem("All", "ALL"));
            listItem2.Add(new ListItem("Sunday", "SUNDAY"));
            listItem2.Add(new ListItem("Monday", "MONDAY"));
            listItem2.Add(new ListItem("Tuesday", "TUESDAY"));
            listItem2.Add(new ListItem("Wednesday", "WEDNESDAY"));
            listItem2.Add(new ListItem("Thursday", "THURSDAY"));
            listItem2.Add(new ListItem("Friday", "FRIDAY"));
            listItem2.Add(new ListItem("Saturday", "SATURDAY"));
            cmbDayofweek.Items.AddRange(listItem2.ToArray());
            cmbDayofweek.SelectedIndex = 0;

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


        private void Fill_ResrSlots_Edit_Delete()
        {

            NewSoltsGrid.Rows.Clear();

            int Sel_From_Time = 000;
            int Sel_To_Time = 2400;
            int Sel_From_Slot1 = 1; int Sel_To_Slot1 = 9;
            if (chkTime.Checked)
            {
                Sel_From_Time = int.Parse(((ListItem)FromTime.SelectedItem).Value.ToString());
                Sel_To_Time = int.Parse(((ListItem)ToTime.SelectedItem).Value.ToString());
            }
            if (chkSlots.Checked)
            {
                Sel_From_Slot1 = int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString());
                Sel_To_Slot1 = int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString());
            }
            List<APPTSCHEDULEEntity> AppscheduleReserveList = propAPPtSchedulelist.FindAll(u => u.SchdType == "2");
            // int Sel_From_Slot1 = int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString()), Sel_To_Slot1 = int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString());
            // int Sel_From_Time = int.Parse(((ListItem)FromTime.SelectedItem).Value.ToString()), Sel_To_Time = int.Parse(((ListItem)ToTime.SelectedItem).Value.ToString());
            int result_From, result_To;
            int rowIndex = 0; string Disp_Time = "";
            foreach (APPTSCHEDULEEntity Entity in AppscheduleReserveList)  // Sindhe
            {
                result_From = DateTime.Compare((Convert.ToDateTime(Entity.Date.Trim())), FromDate.Value);
                result_To = DateTime.Compare((Convert.ToDateTime(Entity.Date.Trim())), Todate.Value);

                if (result_From >= 0 && result_To <= 0 &&
                   (int.Parse(Entity.SlotNumber) >= Sel_From_Slot1) &&
                   (int.Parse(Entity.SlotNumber) <= Sel_To_Slot1) &&
                   (int.Parse(Entity.Time) >= Sel_From_Time) &&
                   (int.Parse(Entity.Time) <= Sel_To_Time))
                {
                    Disp_Time = "";
                    Disp_Time = Format_time(Entity.Time);


                    bool boolDayweek = true;
                    if (chkDayofweek.Checked)
                    {
                        if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != "ALL")
                        {
                            if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().ToUpper())
                            {
                                boolDayweek = false;
                            }
                        }
                    }

                    if (boolDayweek)
                    {

                        rowIndex = NewSoltsGrid.Rows.Add(true, Convert.ToDateTime(Entity.Date).ToString("MM/dd/yyyy"), Disp_Time, Entity.SlotNumber, Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().Substring(0, 3), Entity.SchdType == "1" ? "S" : "R", Entity.LastName, Entity.TemplateID, Entity.Time);

                        All_Rows_Selected = true;
                        CommonFunctions.setTooltip(rowIndex, Entity.AddOperator, Entity.DateAdd, Entity.LstcOperation, Entity.DateLstc, NewSoltsGrid);
                    }

                }
            }
            if (!(Mode.Equals("View")))
            {
                if (NewSoltsGrid.Rows.Count > 0)
                    BtnSave.Visible = true;
                else
                    BtnSave.Visible = false;
            }
        }

        public string strDefaultToslot { get; set; }
        public string strDefaultstartTime { get; set; }
        public string strDefaultEndTime { get; set; }
        private void BtnGetSlots_Click(object sender, EventArgs e)
        {
            try
            {
                strDefaultToslot = "2";
                strDefaultstartTime = string.Empty;
                strDefaultEndTime = string.Empty;
                All_Rows_Selected = false;
                if ((Mode.Equals("View") || Mode.Equals("Edit") || Mode.Equals("Delete")) && ValidateForm("Get Slots"))
                {
                    Fill_ResrSlots_Edit_Delete();
                    if (Mode.Equals("Edit"))
                    {
                        NewReason.ReadOnly = false;
                        if (NewSoltsGrid.Rows.Count == 0)
                            AlertBox.Show("No Slots Exist with Selected Criteria", MessageBoxIcon.Warning);

                    }
                    if (Mode.Equals("Delete"))
                    {
                        if (NewSoltsGrid.Rows.Count == 0)
                            AlertBox.Show("No Slots Exist with Selected Criteria", MessageBoxIcon.Warning);
                    }

                }

                if (ValidateForm("Get Slots") && Mode.Equals("Add"))
                {
                    string Sel_From_Time = "000";
                    string Sel_To_Time = "2400";
                    int Sel_From_Slot = 1; int Sel_To_Slot = 9;
                    if (chkTime.Checked)
                    {
                        Sel_From_Time = ((ListItem)FromTime.SelectedItem).Value.ToString();
                        Sel_To_Time = ((ListItem)ToTime.SelectedItem).Value.ToString();
                    }
                    if (chkSlots.Checked)
                    {
                        Sel_From_Slot = int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString());
                        Sel_To_Slot = int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString());
                    }
                    string Gbl_OpenClose = "";


                    NewReason.ReadOnly = false;
                    Get_Data_By_SiteDate();

                    string Day_table, Period_Table = null, Template_Type = null, Date_To_Compare = null, Month_Value = "";
                    bool Template_Sw = false, Can_Add_slot = false;

                    List<APPTSCHEDULEEntity> Add_Slots = new List<APPTSCHEDULEEntity>();
                    NewSoltsGrid.Rows.Clear();
                    //if (TmsApcnList.Count > 0)
                    //{
                    int Week_Day, Template_slots = 0, Template_Min = 5;

                    bool Date_Template_Exists = false;
                    for (DateTime date = FromDate.Value; date <= Todate.Value; date = date.AddDays(1))
                    {
                        Template_Sw = Date_Template_Exists = false;

                        List<APPTEMPLATESEntity> apptTempplatelist = _model.TmsApcndata.GetAPPTEMPLATESadpysitedates(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSite.Text, date.ToShortDateString(), string.Empty, "Dates", "Dates");
                        if (apptTempplatelist.Count > 0)
                        {
                            APPTEMPLATESEntity apptTemplatedata = apptTempplatelist[0];
                            if (apptTemplatedata != null)
                            {

                                Gbl_OpenClose = apptTemplatedata.TemplateAvailble;

                                Week_Day = Get_WeekDay_Name(date.DayOfWeek.ToString());
                                Day_table = apptTemplatedata.DayTable;

                                if (Day_table.Substring(Week_Day - 1, 1) == "1")
                                {
                                    Period_Table = apptTemplatedata.PeriodTable;
                                    Template_slots = int.Parse(apptTemplatedata.SlotsPerPeriod);
                                    // Template_Min = int.Parse(apptTemplatedata.Mins);
                                    Template_Type = apptTemplatedata.Type;
                                    Template_Sw = true;
                                }

                                if (FromDate.Value == Todate.Value)
                                {
                                    if (Template_slots > 1)
                                        strDefaultToslot = (Template_slots).ToString();
                                }

                            }
                        }


                        if (Template_Sw && !string.IsNullOrEmpty(Period_Table) && Gbl_OpenClose == "1")
                        {
                            if (Period_Table.Contains("1"))
                            {
                                int Hours = 00;
                                int Mins = 00;

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

                                    Can_Add_slot = true;

                                    if ((Local_Time >= int.Parse(Sel_From_Time)) && (Local_Time <= int.Parse(Sel_To_Time)))
                                    {

                                        Can_Add_slot = true;

                                        CommonEntity slotsdetailis = commonslots.Find(u => u.Code.ToString() == i.ToString());
                                        if (slotsdetailis != null)
                                        {
                                            string strslotsdetails = slotsdetailis.Desc.ToString();
                                            for (int intslotd = 0; intslotd < strslotsdetails.Length; intslotd++)
                                            {
                                                if (strslotsdetails.Substring(intslotd, 1).ToString() == "1")
                                                {

                                                    if (Can_Add_slot)
                                                    {
                                                        Local_Disp_Time = null;
                                                        Local_Disp_Time = Local_Time.ToString();

                                                        Local_Disp_Time = Format_time(Local_Disp_Time);
                                                        Add_Slots.Add(new APPTSCHEDULEEntity(date.ToShortDateString(), Local_Disp_Time, (intslotd + 1).ToString(), TxtReason.Text.Trim(), Template_Type, Local_Time.ToString()));
                                                        if (FromDate.Value == Todate.Value)
                                                        {
                                                            if (strDefaultstartTime == string.Empty)
                                                            {
                                                                strDefaultstartTime = Local_Time.ToString();

                                                            }
                                                            strDefaultEndTime = Local_Time.ToString();
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                    }


                                    Mins += Template_Min;
                                    if ((Mins >= 60) && (Mins % 60) >= 0)
                                    { Hours++; Mins = 00; }

                                    switch (Mins)
                                    {
                                        case 0: Local_Time = int.Parse(Hours.ToString() + "00"); break;
                                        case 5: Local_Time = int.Parse(Hours.ToString() + "05"); break;// murali added this condition 02/06/2021 
                                        default: Local_Time = int.Parse(Hours.ToString() + Mins.ToString()); break;
                                    }
                                }
                            }
                        }
                    }


                    NewSoltsGrid.Rows.Clear();
                    int index = 0;
                    bool boolstatusdata = true;
                    foreach (APPTSCHEDULEEntity Entity in Add_Slots)
                    {
                        boolstatusdata = true;
                        if (chkSlots.Checked)
                        {
                            boolstatusdata = false;

                            if (Sel_From_Slot <= int.Parse(Entity.SlotNumber) && Sel_To_Slot >= int.Parse(Entity.SlotNumber))
                                boolstatusdata = true;


                        }
                        if (boolstatusdata)
                        {
                            bool boolDayweek = true;
                            if (chkDayofweek.Checked)
                            {
                                if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != "ALL")
                                {
                                    if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().ToUpper())
                                    {
                                        boolDayweek = false;
                                    }
                                }
                            }

                            if (boolDayweek)
                            {
                                APPTSCHEDULEEntity ApptScheduleExistslot = propAPPtSchedulelist.Find(u => (Convert.ToDateTime(u.Date) == Convert.ToDateTime(Entity.Date)) && u.Time == Entity.CaseWorker && u.SlotNumber == Entity.SlotNumber);
                                if (ApptScheduleExistslot != null)
                                {
                                    index = NewSoltsGrid.Rows.Add(false, Convert.ToDateTime(ApptScheduleExistslot.Date).ToString("MM/dd/yyyy"), Format_time(ApptScheduleExistslot.Time), ApptScheduleExistslot.SlotNumber, Convert.ToDateTime(ApptScheduleExistslot.Date).DayOfWeek.ToString().Substring(0, 3), ApptScheduleExistslot.SchdType == "1" ? "S" : "R", ApptScheduleExistslot.LastName + " " + ApptScheduleExistslot.FirstName, ApptScheduleExistslot.TemplateID, Entity.CaseWorker);
                                    NewSoltsGrid.Rows[index].ReadOnly = true;
                                }
                                else
                                {
                                    index = NewSoltsGrid.Rows.Add(true, Convert.ToDateTime(Entity.Date).ToString("MM/dd/yyyy"), Entity.Time, Entity.SlotNumber, Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().Substring(0, 3), "O", Entity.LastName, Entity.SchdType, Entity.CaseWorker);
                                }
                            }
                        }
                    }
                    // NewSoltsGrid.Rows.Add(true, Entity.Date, Entity.FirstName, Entity.SlotNumber, Entity.Name, Entity.RecordType, Entity.CaseWorker);
                    if (Add_Slots.Count == 0 || NewSoltsGrid.Rows.Count == 0)
                        AlertBox.Show("No Slots Exist with Selected Criteria", MessageBoxIcon.Warning);
                    else
                        All_Rows_Selected = true;
                    //}
                    //else
                    //{
                    //    CommonFunctions.MessageBoxDisplay("No templates defined");
                    //}
                }

                if (NewSoltsGrid.Rows.Count > 0)
                {
                    NewSoltsGrid.Visible = true;
                    BtnSave.Visible = true;

                }
                else
                {
                    BtnSave.Visible = false;

                }

            }
            catch (Exception ex) { }
        }

        public static List<string> GetChunkss(string value, int chunkSize)
        {
            List<string> triplets = new List<string>();
            for (int i = 0; i < value.Length; i += chunkSize)
                if (i + chunkSize > value.Length)
                    triplets.Add(value.Substring(i));
                else
                    triplets.Add(value.Substring(i, chunkSize));

            return triplets;
        }

        private bool ValidateForm(string Operation)
        {
            bool isValid = true;
            if (Mode.Equals("Add") && (String.IsNullOrEmpty(TxtSite.Text)))
            {
                _errorProvider.SetError(BtnSiteSearch, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label1.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(BtnSiteSearch, null);

            if (!FromDate.Checked)
            {
                _errorProvider.SetError(FromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(FromDate, null);

            if (!Todate.Checked)
            {
                _errorProvider.SetError(Todate, string.Format("Date To is Required", label3.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Todate, null);



            if (FromDate.Checked && Todate.Checked && (FromDate.Value > Todate.Value))
            {
                _errorProvider.SetError(FromDate, string.Format("'Date From' Should not be greater than 'Date To'", label2.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            //if(FromDate.Checked && Todate.Checked)
            //{
            //    if(FromDate.Value.Month != Todate.Value.Month )
            //    {
            //        _errorProvider.SetError(FromDate, string.Format("'Date From' and 'Date To' Should be Same", label2.Text.Replace(Consts.Common.Colon, string.Empty)));
            //        isValid = false;
            //    }
            //}
            //else
            //    _errorProvider.SetError(FromDate, null);


            if (int.Parse(((ListItem)FromTime.SelectedItem).Value.ToString()) > int.Parse(((ListItem)ToTime.SelectedItem).Value.ToString()))
            {
                _errorProvider.SetError(FromTime, string.Format("'Time From' Should not be greater than 'Time To'", label4.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(FromTime, null);

            if (int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString()) > int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString()))
            {
                _errorProvider.SetError(FromSlots, string.Format("'Slots From' Should not be greater than 'Slots To'", label6.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(FromSlots, null);

            //if (Mode.Equals("Add"))
            //{
            if (string.IsNullOrEmpty(TxtReason.Text.Trim()) && ((Operation == "Save") && Mode != "Delete"))
            {
                _errorProvider.SetError(TxtReason, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), label8.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(TxtReason, null);
            //}

            IsSaveValid = isValid;
            return (isValid);
        }


        private void CommonTextField_LostFocus(object sender, EventArgs e)
        {
            if (sender == TxtSite)
            {
                if (!(string.IsNullOrEmpty(TxtSite.Text.Trim())))// && TxtSite.Text.Length == 4)
                {
                    _errorProvider.SetError(BtnSiteSearch, null);
                    Fill_Reserved_Slots();

                }
            }

            if (sender == FromDate && FromDate.Checked)
                _errorProvider.SetError(FromDate, null);

            if (sender == Todate && Todate.Checked)
                _errorProvider.SetError(Todate, null);

            if ((sender == FromDate || sender == Todate) && FromDate.Checked && Todate.Checked)
            {
                if (FromDate.Value < Todate.Value)
                    _errorProvider.SetError(FromDate, null);
            }


            if ((sender == FromTime || sender == ToTime) && (int.Parse(((ListItem)FromTime.SelectedItem).Value.ToString()) > int.Parse(((ListItem)ToTime.SelectedItem).Value.ToString())))
                _errorProvider.SetError(FromTime, string.Format("'Time From' Should not be greater than 'Time To'", label4.Text.Replace(Consts.Common.Colon, string.Empty)));
            else
                _errorProvider.SetError(FromTime, null);


            if ((sender == FromSlots || sender == ToSlots) && (int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString()) > int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString())))
                _errorProvider.SetError(FromSlots, null);
            else
                _errorProvider.SetError(FromSlots, null);


            if (sender == TxtReason && !(string.IsNullOrEmpty(TxtReason.Text.Trim())))
                _errorProvider.SetError(TxtReason, null);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Get_Data_By_SiteDate()
        {
            //DataSet ds = Captain.DatabaseLayer.TMS00110DB.Get_Template_APPT_ByDate(MainMemu_Hierarchy + TxtSite.Text, FromDate.Value.ToShortDateString(), Todate.Value.ToShortDateString());

            //if (ds.Tables.Count > 0)
            //    dt_Apcn = ds.Tables[0];

            //if (dt_Apcn != null && dt_Apcn.Rows.Count > 0)
            //{
            //    TmsApcnList.Clear();
            //    foreach (DataRow row in dt_Apcn.Rows)
            //    {
            //        TmsApcnList.Add(new TmsApcnEntity(row, " "));
            //    }
            //}

            //if (ds.Tables.Count > 1)
            //    dt_Appt = ds.Tables[1];
        }

        private string Format_time(string Tmp_Time)
        {
            string Disp_Time = null, Disp_Hours = null;
            switch (Tmp_Time.Length)
            {
                case 1: Disp_Time = "12:" + "0" + Tmp_Time + "  AM"; break;
                case 2: Disp_Time = "12:" + Tmp_Time + "  AM"; break;
                case 3: Disp_Time = "0" + Tmp_Time.Substring(0, 1) + ":" + Tmp_Time.Substring(1, 2) + "  AM"; break;
                case 4:
                    if (int.Parse(Tmp_Time.Substring(0, 2)) >= 12)
                    {
                        if (int.Parse(Tmp_Time.Substring(0, 2)) == 12)
                            Disp_Time = Tmp_Time.Substring(0, 2) + ":" + Tmp_Time.Substring(2, 2) + "  PM";
                        else
                        {
                            Disp_Hours = (int.Parse(Tmp_Time.Substring(0, 2)) - 12).ToString();
                            switch (Disp_Hours.Length)
                            {
                                case 1: Disp_Time = "0" + Disp_Hours + ":" + Tmp_Time.Substring(2, 2) + "  PM"; break;
                                case 2: Disp_Time = Disp_Hours + Tmp_Time.Substring(2, 2) + ":" + "  PM"; break;
                            }
                        }
                    }
                    else
                        Disp_Time = Tmp_Time.Substring(0, 2) + ":" + Tmp_Time.Substring(2, 2) + "  AM";
                    break;
            }

            return Disp_Time;
        }


        private void BtnClearControls_Click(object sender, EventArgs e)
        {
            //Form_Mode = "Auto";
            NewSoltsGrid.Rows.Clear();
            FromDate.Checked = true;
            Todate.Checked = true;
            // FromDate.Value = Todate.Value = DateTime.Today;
            var first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            FromDate.Value = first;

            var last = first.AddMonths(1).AddDays(-1);
            Todate.Value = last;

            FromTime.SelectedIndex = 96;
            ToTime.SelectedIndex = 240;
            FromSlots.SelectedIndex = 0;
            ToSlots.SelectedIndex = 2;
            TxtReason.Clear();
            BtnSave.Visible = false;
            cmbDayofweek.SelectedIndex = 0;
        }

        private void BtnSiteSearch_Click(object sender, EventArgs e)
        {
            //BtnClearControls_Click(BtnClearControls, EventArgs.Empty);
            //SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, "TMS00120", "S");
            SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges, "ClientIntake", BaseForm);
            Select_site.FormClosed += new FormClosedEventHandler(OnSerachFormClosed);
            Select_site.ShowDialog();
        }

        private void OnSerachFormClosed(object sender, FormClosedEventArgs e)
        {
            SiteSearchForm form = sender as SiteSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                TxtSite.Text = form.GetSelectedRowDetails();
                CommonTextField_LostFocus(TxtSite, EventArgs.Empty);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm("Save"))
            {


                APPTSCHEDULEEntity Sel_Entity = new APPTSCHEDULEEntity();

                Sel_Entity.Mode = Mode;
                bool Just_Saved = false;


                bool Atleast_One_Row_Selected = false;
                foreach (DataGridViewRow dr in NewSoltsGrid.Rows)
                {
                    if (dr.Cells["CB_Add_SW"].Value.ToString() == true.ToString())
                    {
                        Atleast_One_Row_Selected = true;
                        break;
                    }
                }

                if (!Atleast_One_Row_Selected)
                {
                    AlertBox.Show("Please Select at least One Record", MessageBoxIcon.Warning);
                    return;
                }

                if (Mode.ToUpper() != "DELETE")
                {
                    Sel_Entity.Agency = BaseForm.BaseAgency;//MainMemu_Hierarchy.Substring(0, 2);
                    Sel_Entity.Dept = BaseForm.BaseDept; // MainMemu_Hierarchy.Substring(2, 2);
                    Sel_Entity.Program = BaseForm.BaseProg;//MainMemu_Hierarchy.Substring(4, 2);
                    Sel_Entity.Year = string.Empty;// BaseForm.BaseYear;// MainMemu_Hierarchy.Substring(6, 4);
                    Sel_Entity.Site = TxtSite.Text;

                    Sel_Entity.SchdType = "2";

                    Sel_Entity.LastName = TxtReason.Text.Trim();
                    Sel_Entity.SsNumber = null;
                    Sel_Entity.FirstName = string.Empty;
                    Sel_Entity.TelNumber = Sel_Entity.HNo = Sel_Entity.Street = null;
                    Sel_Entity.Suffix = Sel_Entity.Apt = Sel_Entity.Floor = Sel_Entity.City = Sel_Entity.State = null;
                    Sel_Entity.Zip1 = Sel_Entity.Zip2 = "0";

                    Sel_Entity.Sex = Sel_Entity.CaseWorker = Sel_Entity.HeatSource = Sel_Entity.SourceIncome = null;
                    Sel_Entity.CellNumber = Sel_Entity.ContactDate = null;
                    Sel_Entity.CellProvider = Sel_Entity.ContactPerson = null;
                    Sel_Entity.EditBy = Sel_Entity.EditTime = null;

                    Sel_Entity.LstcOperation = Sel_Entity.AddOperator = BaseForm.UserID;


                    string strnewReason = string.Empty;
                    foreach (DataGridViewRow dr in NewSoltsGrid.Rows)
                    {

                        if (dr.Cells["CB_Add_SW"].Value.ToString() == true.ToString())
                        {
                            Just_Saved = false;
                            Sel_Entity.Date = (Convert.ToDateTime(dr.Cells["NewDate"].Value.ToString().Trim())).ToShortDateString();

                           // Sel_Entity.Time = "000".Substring(0, 3 - dr.Cells["NewTime_key"].Value.ToString().Length) + dr.Cells["NewTime_key"].Value.ToString();
                            Sel_Entity.Time = dr.Cells["NewTime_key"].Value.ToString();
                           
                            
                            Sel_Entity.SlotNumber = dr.Cells["NewSlot"].Value.ToString();
                            Sel_Entity.TemplateID = dr.Cells["NewTemp_Type"].Value.ToString();
                            strnewReason = dr.Cells["NewReason"].Value == null ? string.Empty : dr.Cells["NewReason"].Value.ToString();
                            if (strnewReason != string.Empty)
                                Sel_Entity.LastName = strnewReason;
                            else
                                Sel_Entity.LastName = TxtReason.Text.Trim();

                            if (Mode.ToUpper() == "EDIT")
                            {
                                Sel_Entity.LastName = TxtReason.Text.Trim();
                            }

                            //switch (Rec_Type)
                            //{
                            //    case 'D':
                            //        if ((_model.TMS00110Data.Delete_TMSAPPT(MainMemu_Hierarchy, TxtSite.Text, Sel_Entity.Date, Sel_Entity.Time, Sel_Entity.SlotNumber, "")))
                            //            Just_Saved = true;
                            //        break;

                            //    default:
                            if (_model.TMS00110Data.InsertUpdateDelAPPTSCHED(Sel_Entity))
                                Just_Saved = true;
                            //        break;
                            //}
                        }
                    }



                    switch (Just_Saved)
                    {
                        case true:
                            switch (Mode.ToUpper())
                            {

                                case "EDIT": AlertBox.Show("Record(s) Updated Successfully"); BtnSave.Visible = false; break;
                                case "ADD": AlertBox.Show("Record(s) Inserted Successfully"); NewReason.ReadOnly = false; BtnSave.Visible = false; break;
                                case "DELETE": AlertBox.Show("Record(s) Deleted Successfully"); BtnSave.Visible = false; break;
                            }
                            //BtnClearControls_Click(BtnClearControls, EventArgs.Empty);

                            TxtReason.Text = "";
                            NewSoltsGrid.Rows.Clear();
                            Fill_Reserved_Slots();
                            //Fill_ResrSlots_Edit_Delete();
                            break;
                        case false:
                            switch (Mode.ToUpper())
                            {
                                case "EDIT": AlertBox.Show("Record(s) Update UnSuccessful"); BtnSave.Visible = false; break;
                                case "ADD": AlertBox.Show("Record(s) Insert UnSuccessful"); BtnSave.Visible = false; break;
                                case "DELETE": AlertBox.Show("Record(s) Delete UnSuccessful"); BtnSave.Visible = false; break;
                            }
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Are you sure want to Delete selected Reserved Slots?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);

                }
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult.ToString() == "Yes")
            {
                APPTSCHEDULEEntity Sel_Entity = new APPTSCHEDULEEntity();

                Sel_Entity.Mode = "DELETE";
                bool Just_Saved = false;

                Sel_Entity.Agency = BaseForm.BaseAgency;
                Sel_Entity.Dept = BaseForm.BaseDept;
                Sel_Entity.Program = BaseForm.BaseProg;
                Sel_Entity.Year = string.Empty;//BaseForm.BaseYear;
                Sel_Entity.Site = TxtSite.Text;

                Sel_Entity.SchdType = "2";
                Sel_Entity.LastName = null;
                Sel_Entity.SsNumber = null;
                Sel_Entity.FirstName = string.Empty;
                Sel_Entity.TelNumber = Sel_Entity.HNo = Sel_Entity.Street = null;
                Sel_Entity.Suffix = Sel_Entity.Apt = Sel_Entity.Floor = Sel_Entity.City = Sel_Entity.State = null;
                Sel_Entity.Zip1 = Sel_Entity.Zip2 = "0";

                Sel_Entity.Sex = Sel_Entity.CaseWorker = Sel_Entity.HeatSource = Sel_Entity.SourceIncome = null;
                Sel_Entity.CellNumber = Sel_Entity.ContactDate = null;
                Sel_Entity.CellProvider = Sel_Entity.ContactPerson = null;
                Sel_Entity.EditBy = Sel_Entity.EditTime = null;

                Sel_Entity.LstcOperation = Sel_Entity.AddOperator = BaseForm.UserID;


                foreach (DataGridViewRow dr in NewSoltsGrid.Rows)
                {

                    if (dr.Cells["CB_Add_SW"].Value.ToString() == true.ToString())
                    {
                        Just_Saved = false;
                        Sel_Entity.Date = (Convert.ToDateTime(dr.Cells["NewDate"].Value.ToString().Trim())).ToShortDateString();
                        Sel_Entity.Time = dr.Cells["NewTime_key"].Value.ToString();
                        Sel_Entity.SlotNumber = dr.Cells["NewSlot"].Value.ToString();
                        Sel_Entity.TemplateID = dr.Cells["NewTemp_Type"].Value.ToString();
                        if (_model.TMS00110Data.InsertUpdateDelAPPTSCHED(Sel_Entity))
                            Just_Saved = true;
                    }
                }



                switch (Just_Saved)
                {
                    case true:
                        switch (Mode.ToUpper())
                        {
                            case "DELETE": AlertBox.Show("Record(s) Deleted Successfully"); break;
                        }
                        //BtnClearControls_Click(BtnClearControls, EventArgs.Empty);

                        TxtReason.Text = "";
                        NewSoltsGrid.Rows.Clear();
                        Fill_Reserved_Slots();
                        //Fill_ResrSlots_Edit_Delete();
                        break;
                    case false:
                        switch (Mode.ToUpper())
                        {
                            case "DELETE": AlertBox.Show("Failed to Delete selected Record(s)", MessageBoxIcon.Warning); break;
                        }
                        break;
                }


            }

        }



        bool All_Rows_Selected = false;
        private void NewSoltsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (All_Rows_Selected)
            //{
            //    foreach (DataGridViewRow dr in NewSoltsGrid.Rows)
            //        dr.Cells["CB_Add_SW"].Value = false;

            //    All_Rows_Selected = false;
            //}
            //else
            //{
            //    foreach (DataGridViewRow dr in NewSoltsGrid.Rows)
            //        dr.Cells["CB_Add_SW"].Value = true;

            //    All_Rows_Selected = true;
            //}
        }

        private void chkTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTime.Checked)
                label5.Visible = FromTime.Visible = ToTime.Visible = true;
            else
                label5.Visible = FromTime.Visible = ToTime.Visible = false;
            if (Mode.ToUpper() == "ADD")
            {
                FromTime.SelectedIndex = 96;
                ToTime.SelectedIndex = 240;
                if (FromDate.Value == Todate.Value)
                {
                    if (strDefaultstartTime != string.Empty)
                    {
                        CommonFunctions.SetComboBoxValue(FromTime, strDefaultstartTime);
                        CommonFunctions.SetComboBoxValue(ToTime, strDefaultEndTime);
                    }

                }

            }
        }

        private void chkSlots_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSlots.Checked)
                label7.Visible = FromSlots.Visible = ToSlots.Visible = true;
            else
                label7.Visible = FromSlots.Visible = ToSlots.Visible = false;
            if (Mode.ToUpper() == "ADD")
            {
                if (FromDate.Value == Todate.Value)
                {
                    CommonFunctions.SetComboBoxValue(FromSlots, "1");
                    CommonFunctions.SetComboBoxValue(ToSlots, strDefaultToslot);
                }
                else
                {
                    CommonFunctions.SetComboBoxValue(FromSlots, "1");
                    CommonFunctions.SetComboBoxValue(ToSlots, "3");
                }
            }

        }

        private void chkDayofweek_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDayofweek.Checked)
            {
                cmbDayofweek.Visible = true;
                cmbDayofweek.SelectedIndex = 0;
            }
            else
            {
                cmbDayofweek.Visible = false;
                cmbDayofweek.SelectedIndex = 0;
            }

        }

        public void SetDefaultTimeAddMode()
        {
            if (Mode.Equals("Add"))
            {
                if (FromDate.Value == Todate.Value)
                {
                    strDefaultToslot = "2";
                    strDefaultstartTime = string.Empty;
                    strDefaultEndTime = string.Empty;
                    string Sel_From_Time = "000";
                    string Sel_To_Time = "2400";
                    int Sel_From_Slot = 1; int Sel_To_Slot = 9;
                    if (chkTime.Checked)
                    {
                        Sel_From_Time = ((ListItem)FromTime.SelectedItem).Value.ToString();
                        Sel_To_Time = ((ListItem)ToTime.SelectedItem).Value.ToString();
                    }
                    if (chkSlots.Checked)
                    {
                        Sel_From_Slot = int.Parse(((ListItem)FromSlots.SelectedItem).Value.ToString());
                        Sel_To_Slot = int.Parse(((ListItem)ToSlots.SelectedItem).Value.ToString());
                    }
                    string Gbl_OpenClose = "";


                    NewReason.ReadOnly = false;
                    Get_Data_By_SiteDate();

                    string Day_table, Period_Table = null, Template_Type = null, Date_To_Compare = null, Month_Value = "";
                    bool Template_Sw = false, Can_Add_slot = false;

                    List<APPTSCHEDULEEntity> Add_Slots = new List<APPTSCHEDULEEntity>();
                    NewSoltsGrid.Rows.Clear();
                    //if (TmsApcnList.Count > 0)
                    //{
                    int Week_Day, Template_slots = 0, Template_Min = 5;

                    bool Date_Template_Exists = false;
                    for (DateTime date = FromDate.Value; date <= Todate.Value; date = date.AddDays(1))
                    {
                        Template_Sw = Date_Template_Exists = false;

                        List<APPTEMPLATESEntity> apptTempplatelist = _model.TmsApcndata.GetAPPTEMPLATESadpysitedates(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSite.Text, date.ToShortDateString(), string.Empty, "Dates", "Dates");
                        if (apptTempplatelist.Count > 0)
                        {
                            APPTEMPLATESEntity apptTemplatedata = apptTempplatelist[0];
                            if (apptTemplatedata != null)
                            {

                                Gbl_OpenClose = apptTemplatedata.TemplateAvailble;

                                Week_Day = Get_WeekDay_Name(date.DayOfWeek.ToString());
                                Day_table = apptTemplatedata.DayTable;

                                if (Day_table.Substring(Week_Day - 1, 1) == "1")
                                {
                                    Period_Table = apptTemplatedata.PeriodTable;
                                    Template_slots = int.Parse(apptTemplatedata.SlotsPerPeriod);
                                    // Template_Min = int.Parse(apptTemplatedata.Mins);
                                    Template_Type = apptTemplatedata.Type;
                                    Template_Sw = true;
                                }

                                if (FromDate.Value == Todate.Value)
                                {
                                    if (Template_slots > 1)
                                        strDefaultToslot = (Template_slots).ToString();
                                }

                            }
                        }


                        if (Template_Sw && !string.IsNullOrEmpty(Period_Table) && Gbl_OpenClose == "1")
                        {
                            if (Period_Table.Contains("1"))
                            {
                                int Hours = 00;
                                int Mins = 00;

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

                                    Can_Add_slot = true;

                                    if ((Local_Time >= int.Parse(Sel_From_Time)) && (Local_Time <= int.Parse(Sel_To_Time)))
                                    {

                                        Can_Add_slot = true;

                                        CommonEntity slotsdetailis = commonslots.Find(u => u.Code.ToString() == i.ToString());
                                        if (slotsdetailis != null)
                                        {
                                            string strslotsdetails = slotsdetailis.Desc.ToString();
                                            for (int intslotd = 0; intslotd < strslotsdetails.Length; intslotd++)
                                            {
                                                if (strslotsdetails.Substring(intslotd, 1).ToString() == "1")
                                                {

                                                    if (Can_Add_slot)
                                                    {
                                                        Local_Disp_Time = null;
                                                        Local_Disp_Time = Local_Time.ToString();

                                                        Local_Disp_Time = Format_time(Local_Disp_Time);
                                                        Add_Slots.Add(new APPTSCHEDULEEntity(date.ToShortDateString(), Local_Disp_Time, (intslotd + 1).ToString(), TxtReason.Text.Trim(), Template_Type, Local_Time.ToString()));
                                                        if (FromDate.Value == Todate.Value)
                                                        {
                                                            if (strDefaultstartTime == string.Empty)
                                                            {
                                                                strDefaultstartTime = Local_Time.ToString();

                                                            }
                                                            strDefaultEndTime = Local_Time.ToString();
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                    }


                                    Mins += Template_Min;
                                    if ((Mins >= 60) && (Mins % 60) >= 0)
                                    { Hours++; Mins = 00; }

                                    switch (Mins)
                                    {
                                        case 0: Local_Time = int.Parse(Hours.ToString() + "00"); break;
                                        case 5: Local_Time = int.Parse(Hours.ToString() + "05"); break; // murali added this condition 02/06/2021 
                                        default: Local_Time = int.Parse(Hours.ToString() + Mins.ToString()); break;
                                    }
                                }
                            }
                        }
                    }


                    NewSoltsGrid.Rows.Clear();
                    int index = 0;
                    bool boolstatusdata = true;
                    foreach (APPTSCHEDULEEntity Entity in Add_Slots)
                    {
                        boolstatusdata = true;
                        if (chkSlots.Checked)
                        {
                            boolstatusdata = false;

                            if (Sel_From_Slot <= int.Parse(Entity.SlotNumber) && Sel_To_Slot >= int.Parse(Entity.SlotNumber))
                                boolstatusdata = true;


                        }
                        if (boolstatusdata)
                        {
                            bool boolDayweek = true;
                            if (chkDayofweek.Checked)
                            {
                                if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != "ALL")
                                {
                                    if (((ListItem)cmbDayofweek.SelectedItem).Value.ToString().ToUpper() != Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().ToUpper())
                                    {
                                        boolDayweek = false;
                                    }
                                }
                            }

                            if (boolDayweek)
                            {
                                APPTSCHEDULEEntity ApptScheduleExistslot = propAPPtSchedulelist.Find(u => (Convert.ToDateTime(u.Date) == Convert.ToDateTime(Entity.Date)) && u.Time == Entity.CaseWorker && u.SlotNumber == Entity.SlotNumber);
                                if (ApptScheduleExistslot != null)
                                {
                                    index = NewSoltsGrid.Rows.Add(false, Convert.ToDateTime(ApptScheduleExistslot.Date).ToString("MM/dd/yyyy"), Format_time(ApptScheduleExistslot.Time), ApptScheduleExistslot.SlotNumber, Convert.ToDateTime(ApptScheduleExistslot.Date).DayOfWeek.ToString().Substring(0, 3), ApptScheduleExistslot.SchdType == "1" ? "S" : "R", ApptScheduleExistslot.LastName + " " + ApptScheduleExistslot.FirstName, ApptScheduleExistslot.TemplateID, Entity.CaseWorker);
                                    NewSoltsGrid.Rows[index].ReadOnly = true;
                                }
                                else
                                {
                                    index = NewSoltsGrid.Rows.Add(true, Convert.ToDateTime(Entity.Date).ToString("MM/dd/yyyy"), Entity.Time, Entity.SlotNumber, Convert.ToDateTime(Entity.Date).DayOfWeek.ToString().Substring(0, 3), "O", Entity.LastName, Entity.SchdType, Entity.CaseWorker);
                                }
                            }
                        }
                    }
                    // NewSoltsGrid.Rows.Add(true, Entity.Date, Entity.FirstName, Entity.SlotNumber, Entity.Name, Entity.RecordType, Entity.CaseWorker);
                    if (Add_Slots.Count == 0 || NewSoltsGrid.Rows.Count == 0)
                        AlertBox.Show("No Slots Exist with Selected Criteria", MessageBoxIcon.Warning);
                    else
                        All_Rows_Selected = true;

                    chkTime.Checked = true;
                    chkTime_CheckedChanged(chkTime, new EventArgs());
                    chkSlots_CheckedChanged(chkSlots, new EventArgs());
                    chkSlots.Checked = true;
                    if (NewSoltsGrid.Rows.Count > 0)
                    {
                        NewSoltsGrid.Visible = true;
                        BtnSave.Visible = true;
                    }
                    else
                    {
                        BtnSave.Visible = false;
                    }

                }
            }
        }
    }
}