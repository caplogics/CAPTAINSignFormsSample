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
using System.Globalization;
using Wisej.Web;

#endregion

namespace Captain.Common.Views.Forms
{

    public partial class APPT0002Cancelled_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public APPT0002Cancelled_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, string sitecode, string strDate)
        {
            InitializeComponent();
            BaseForm = baseForm;
            M_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //M_HieDesc = hieDesc;
            M_Year = string.Empty;
            Mode = mode;
            SchSite = sitecode;
            if (strDate != string.Empty)
            {
                SchDate = strDate;
                AppDate.Value = Convert.ToDateTime(strDate);
            }
            else
            {
                //var first = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), 1);
                //AppDate.Value = first;

            }

            SchDate = AppDate.Value.ToShortDateString();
            TxtSiteCode.Text = sitecode;
            //SchType = type;
            Privileges = privileges;
            this.Text = "Cancelled Appointments";//"TMS0011";

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

            PropApptschedule = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, string.Empty, string.Empty, string.Empty, string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty, string.Empty,"1");
            PropApptschedule = PropApptschedule.FindAll(u => u.SchdType.Equals("1"));

            //LblHeader.Text = privileges.PrivilegeName;
            //this.Size = new System.Drawing.Size(768, 598);
            PrivApp_date = DateTime.MinValue;
        }

        public APPT0002Cancelled_Form(BaseForm baseForm, PrivilegeEntity privileges, string mode, string sitecode, string strDate, string strMonth, string strYear,string Rec0,string Rec1)
        {
            InitializeComponent();
            BaseForm = baseForm;
            M_Hierarchy = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //M_HieDesc = hieDesc;
            M_Year = string.Empty;
            Mode = mode;
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


        #endregion

        EventArgs e = new EventArgs();
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges.Program, "S");
            SiteSearchForm Select_site = new SiteSearchForm(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, Privileges, "ClientIntake", BaseForm);
            Select_site.FormClosed += new FormClosedEventHandler(OnSerachFormClosed);
            Select_site.ShowDialog();
        }

        string Priv_Sel_Site = "";
        private void OnSerachFormClosed(object sender, FormClosedEventArgs e)
        {
            SiteSearchForm form = sender as SiteSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                TxtSiteCode.Text = form.GetSelectedRowDetails();
                //if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                //    BtnNextSlot.Visible = true;
                //else
                //    BtnNextSlot.Visible = false;

                if (Priv_Sel_Site != TxtSiteCode.Text.Trim())
                    FillGrid();

                Priv_Sel_Site = TxtSiteCode.Text.Trim();
            }
        }

        private void FillGrid()
        {
            if (!(string.IsNullOrEmpty(TxtSiteCode.Text)))
            {
                //this.SchAppGrid.SelectionChanged -= new System.EventHandler(this.SchAppGrid_SelectionChanged);
                SchAppGrid.Rows.Clear();

                int TmpCount = 0, Sel_SSN_Rec_Index = 0; Tot_Sch_Slot_Cnt = 0;
                bool Open_Day_With_Slots = true; Process_Template_Date = Process_Template_Type = "";

                List<APPTSCHDHISTEntity> propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHDHISTBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                int rowIndex = 0;
                if (propAPPtSchedulelist.Count>0)
                {
                    foreach(APPTSCHDHISTEntity apptexistedSlot in propAPPtSchedulelist)
                    {
                        string strPhoneNumber = LookupDataAccess.GetPhoneFormat(apptexistedSlot.TelNumber);
                        if (apptexistedSlot.CellNumber != string.Empty)
                            strPhoneNumber = LookupDataAccess.GetPhoneFormat(apptexistedSlot.CellNumber);
                        //if (apptexistedSlot.SsNumber.Trim() != string.Empty)
                        //    strSSN = LookupDataAccess.GetCardNo(apptexistedSlot.SsNumber, "1", "N", string.Empty);
                        //

                        
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

                        string Time = string.Empty;
                        if (int.Parse(apptexistedSlot.Time) > 1200)
                        {
                            Time = (int.Parse(apptexistedSlot.Time) - 1200).ToString();
                            if (Time.Length > 3) Time = Time.Substring(0, 2) + ":" + Time.Substring(2, Time.Length - 2) + " PM";
                            else Time = Time.Substring(0, 1) + ":" + Time.Substring(1, Time.Length - 1) + " PM";
                        }
                        else
                        {
                            Time = apptexistedSlot.Time;
                            if (Time.Length > 3) Time = Time.Substring(0, 2) + ":" + Time.Substring(2, Time.Length - 2) + " AM";
                            else Time = Time.Substring(0, 1) + ":" + Time.Substring(1, Time.Length - 1) + " AM";
                        }
                        //Time = result.ToString("hh:mm tt", CultureInfo.CurrentCulture);

                        rowIndex = SchAppGrid.Rows.Add(false, Time, apptexistedSlot.FirstName + " " + apptexistedSlot.LastName, strPhoneNumber, strStatus, apptexistedSlot.AddOperator, LookupDataAccess.Getdate(apptexistedSlot.DateAdd),  apptexistedSlot.SlotNumber, apptexistedSlot.Time, "Y", apptexistedSlot.SsNumber, apptexistedSlot.SchdType, apptexistedSlot.TemplateID);
                        SchAppGrid.Rows[rowIndex].Tag = apptexistedSlot;
                        if (Rec_Sel_From_SSN_Search)
                        {
                            //if (Sel_SSN_Rec[0] == Disp_Hours + ":" + Disp_Miin + " " + AM_PM && Sel_SSN_Rec[1] == (intslotd + 1).ToString())
                            //{
                            //    SchAppGrid.Rows[rowIndex].Selected = true;
                            //    Sel_SSN_Rec_Index = rowIndex;
                            //}
                        }
                        CommonFunctions.setTooltip(rowIndex, apptexistedSlot.AddOperator, apptexistedSlot.DateAdd, apptexistedSlot.LstcOperation, apptexistedSlot.DateLstc, SchAppGrid);
                        TmpCount++;
                    }
                }

            }
        }

        private void FillDropDowns()
        {
            //CmbGen.Items.Clear();
            //List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("Select One", "0"));
            //listItem.Add(new ListItem("Female", "F"));
            //listItem.Add(new ListItem("Male", "M"));
            //CmbGen.Items.AddRange(listItem.ToArray());

            //CmbHtSrc.Items.Clear(); CmbHtSrc.ColorMember = "FavoriteColor";
            //List<CommonEntity> HeatingSources = _model.lookupDataAccess.Get_HearingSources();
            //List<ListItem> listItem2 = new List<ListItem>();
            //foreach (CommonEntity Entity in HeatingSources)
            //{
            //    //if ((Mode == "Add" && Entity.Active.Trim() != "N")) || (Mode == "Edit"))
            //    listItem2.Add(new ListItem(Entity.Desc, Entity.Code.Trim(), Entity.Active.Trim(), (Entity.Active.Trim() != "Y") ? Color.Red : Color.Green));

            //    //CmbHtSrc.Items.Add(new ListItem(Entity.Desc, Entity.Code));
            //}
            //CmbHtSrc.Items.AddRange(listItem2.ToArray());
            //CmbHtSrc.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            //CmbHtSrc.SelectedIndex = 0;

            //listItem2.Clear();
            //CmbCellProv.Items.Clear(); CmbCellProv.ColorMember = "FavoriteColor";
            //List<CommonEntity> CellProvider = _model.lookupDataAccess.Get_CellProvider();
            //foreach (CommonEntity Entity in CellProvider)
            //{
            //    //if ((Mode == "Add" && Entity.Active.Trim() != "N")) || (Mode == "Edit"))
            //    listItem2.Add(new ListItem(Entity.Desc, Entity.Code.Trim(), Entity.Active.Trim(), (Entity.Active.Trim() != "Y") ? Color.Red : Color.Green));

            //    //CmbCellProv.Items.Add(new ListItem(Entity.Desc, Entity.Code));
            //    CmbCellProv.Items.AddRange(listItem2.ToArray());
            //}
            //CmbCellProv.Items.Insert(0, new ListItem("Select One", "0", "Y", Color.White));
            //CmbCellProv.SelectedIndex = 0;


            FillCaseWroker();

            List<ListItem> listItem3 = new List<ListItem>();
            listItem3.Add(new ListItem("Select One", "0"));
            listItem3.Add(new ListItem("New Client ", "N"));
            listItem3.Add(new ListItem("Returning Client", "R"));
            //cmbClientStatus.Items.AddRange(listItem3.ToArray());
            //cmbClientStatus.SelectedIndex = 0;

            PropStatus = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00125", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode); ////_model.lookupDataAccess.GetCaseType();
            // CaseType = filterByHIE(CaseType);
            //cmbStatus.Items.Insert(0, new ListItem("Select One", "0"));
            //cmbStatus.ColorMember = "FavoriteColor";
            //cmbStatus.SelectedIndex = 0;
            //foreach (CommonEntity casetype in PropStatus)
            //{
            //    ListItem li = new ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Green : Color.Red);
            //    cmbStatus.Items.Add(li);
            //    if (Mode.Equals(Consts.Common.Add) && casetype.Default.Equals("Y")) cmbStatus.SelectedItem = li;
            //}

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


        //private void BtnUpdate_Click(object sender, EventArgs e)
        //{
        //    //if (Mode.Equals("Delete"))
        //    //{
        //    //    MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, Delete_Sel_App, true);
        //    //}
        //    //else
        //    //{
        //    if (ValidateForm())
        //    {
        //        APPTSCHEDULEEntity UpdateEntity = new Model.Objects.APPTSCHEDULEEntity();
        //        Get_MST_SNP_SSN_Status();

        //        char Rec_type = 'A';

        //        if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y")
        //            Rec_type = 'U';

        //        //if (Mode.Equals("Add"))
        //        //    UpdateEntity.SsNumber = MskSsn.Text;
        //        //else
        //        //    Rec_type = 'U';

        //        UpdateEntity.Agency = M_Hierarchy.Substring(0, 2);
        //        UpdateEntity.Dept = M_Hierarchy.Substring(2, 2);
        //        UpdateEntity.Program = M_Hierarchy.Substring(4, 2);
        //        UpdateEntity.Year = string.Empty;

        //        UpdateEntity.Site = TxtSiteCode.Text;
        //        UpdateEntity.Date = AppDate.Value.ToShortDateString();
        //        UpdateEntity.Time = SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString();
        //        UpdateEntity.SlotNumber = SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString();

        //         UpdateEntity.SsNumber = null;
        //        if (dtBirth.Checked)
        //            UpdateEntity.DOB = dtBirth.Value.ToShortDateString();
        //        else
        //            UpdateEntity.DOB = string.Empty;

        //        UpdateEntity.TemplateID = SchAppGrid.CurrentRow.Cells["gvt_TempId"].Value.ToString();
        //        UpdateEntity.SchdType = "1";

        //        UpdateEntity.LastName = TxtLstName.Text;
        //        UpdateEntity.FirstName = TxtFrstName.Text;

        //        UpdateEntity.TelNumber = UpdateEntity.HNo = UpdateEntity.Suffix = UpdateEntity.Apt =
        //        UpdateEntity.Floor = UpdateEntity.City = UpdateEntity.State = UpdateEntity.Zip1 =
        //        UpdateEntity.Zip2 = UpdateEntity.Sex = UpdateEntity.CaseWorker = UpdateEntity.HeatSource =
        //        UpdateEntity.SourceIncome = UpdateEntity.CellNumber = UpdateEntity.Street = UpdateEntity.CellProvider =
        //        UpdateEntity.EditTime = UpdateEntity.EditBy = UpdateEntity.Email = null;

        //        if (!string.IsNullOrEmpty(MskPhone.Text.Trim()))
        //            UpdateEntity.TelNumber = MskPhone.Text.Trim();

        //        if (!string.IsNullOrEmpty(TxtHn.Text.Trim()))
        //            UpdateEntity.HNo = TxtHn.Text;

        //        if (!string.IsNullOrEmpty(TxtStreet.Text.Trim()))
        //            UpdateEntity.Street = TxtStreet.Text;

        //        if (!string.IsNullOrEmpty(TxtSf.Text.Trim()))
        //            UpdateEntity.Suffix = TxtSf.Text;

        //        if (!string.IsNullOrEmpty(TxtApt.Text.Trim()))
        //            UpdateEntity.Apt = TxtApt.Text;

        //        if (!string.IsNullOrEmpty(TxtFlr.Text.Trim()))
        //            UpdateEntity.Floor = TxtFlr.Text;

        //        if (!string.IsNullOrEmpty(TxtCity.Text.Trim()))
        //            UpdateEntity.City = TxtCity.Text;

        //        if (!string.IsNullOrEmpty(TxtState.Text.Trim()))
        //            UpdateEntity.State = TxtState.Text;

        //        if (!(string.IsNullOrEmpty(TxtZip.Text)))
        //            UpdateEntity.Zip1 = TxtZip.Text;
        //        if (!(string.IsNullOrEmpty(TxtZip1.Text)))
        //            UpdateEntity.Zip2 = TxtZip1.Text;

        //        if (!(string.IsNullOrEmpty(TxtEMail.Text)))
        //            UpdateEntity.Email = TxtEMail.Text;

        //        if (((ListItem)CmbGen.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.Sex = ((ListItem)CmbGen.SelectedItem).Value.ToString();

        //        if (((ListItem)CmbWorker.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.CaseWorker = ((ListItem)CmbWorker.SelectedItem).Value.ToString();

        //        if (((ListItem)CmbHtSrc.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.HeatSource = ((ListItem)CmbHtSrc.SelectedItem).Value.ToString();

        //        if (((ListItem)CmbCellProv.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.CellProvider = ((ListItem)CmbCellProv.SelectedItem).Value.ToString();

        //        if (((ListItem)cmbClientStatus.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.Client = ((ListItem)cmbClientStatus.SelectedItem).Value.ToString();

        //        if (((ListItem)cmbStatus.SelectedItem).Value.ToString() != "0")
        //            UpdateEntity.Status = ((ListItem)cmbStatus.SelectedItem).Value.ToString();

                
        //        if (!(string.IsNullOrEmpty(txtIncSrc.Text.Trim())))
        //            UpdateEntity.SourceIncome = txtIncSrc.Text;

        //        if (!string.IsNullOrEmpty(MskCell.Text.Trim()))
        //            UpdateEntity.CellNumber = MskCell.Text.Trim();

        //        UpdateEntity.ContactDate = DtpCntrctDt.Text;

        //        if (!string.IsNullOrEmpty(txtNotes.Text.Trim()))
        //            UpdateEntity.Notes = txtNotes.Text.Trim();

        //        UpdateEntity.LstcOperation = UpdateEntity.ContactPerson =
        //        UpdateEntity.AddOperator = BaseForm.UserID;
        //        if (Rec_type == 'U')
        //            UpdateEntity.Mode = "Edit";
        //        else
        //            UpdateEntity.Mode = "Add";

        //        if(UpdateEntity.Status=="03" || UpdateEntity.Status=="04"|| UpdateEntity.Status=="05")
        //        {
        //            APPTSCHDHISTEntity HistEntity = new APPTSCHDHISTEntity();

        //            HistEntity.Agency = UpdateEntity.Agency;
        //            HistEntity.Dept = UpdateEntity.Dept;
        //            HistEntity.Program = UpdateEntity.Program;
        //            HistEntity.Year = UpdateEntity.Year;
        //            HistEntity.Site = UpdateEntity.Site;
        //            HistEntity.Date = UpdateEntity.Date;
        //            HistEntity.Time = UpdateEntity.Time;
        //            HistEntity.SlotNumber = UpdateEntity.SlotNumber;
        //            HistEntity.Seq = "1";
        //            HistEntity.SsNumber = UpdateEntity.SsNumber;
        //            HistEntity.TemplateID = UpdateEntity.TemplateID;
        //            HistEntity.SchdType = UpdateEntity.SchdType;
        //            HistEntity.SchdDay = UpdateEntity.SchdDay;
        //            HistEntity.LastName = UpdateEntity.LastName;
        //            HistEntity.FirstName = UpdateEntity.FirstName;
        //            HistEntity.TelNumber = UpdateEntity.TelNumber;
        //            HistEntity.HNo = UpdateEntity.HNo;
        //            HistEntity.Street = UpdateEntity.Street;
        //            HistEntity.Suffix = UpdateEntity.Suffix;
        //            HistEntity.Apt = UpdateEntity.Apt;
        //            HistEntity.Floor = UpdateEntity.Floor;
        //            HistEntity.City = UpdateEntity.City;
        //            HistEntity.State = UpdateEntity.State;
        //            HistEntity.Zip1 = UpdateEntity.Zip1;
        //            HistEntity.Zip2 = UpdateEntity.Zip2;
        //            HistEntity.HeatSource = UpdateEntity.HeatSource;
        //            HistEntity.SourceIncome = UpdateEntity.SourceIncome;
        //            HistEntity.ContactPerson = UpdateEntity.ContactPerson;
        //            HistEntity.ContactDate = UpdateEntity.ContactDate;
        //            HistEntity.Sex = UpdateEntity.Sex;
        //            HistEntity.CellProvider = UpdateEntity.CellProvider;
        //            HistEntity.CellNumber = UpdateEntity.CellNumber;
        //            HistEntity.CaseWorker = UpdateEntity.CaseWorker;
        //            HistEntity.DateLstc = UpdateEntity.DateLstc;
        //            HistEntity.LstcOperation = UpdateEntity.LstcOperation;
        //            HistEntity.DateAdd = UpdateEntity.DateAdd;
        //            HistEntity.AddOperator = UpdateEntity.AddOperator;
        //            HistEntity.EditTime = UpdateEntity.EditTime;
        //            HistEntity.EditBy = UpdateEntity.EditBy;
        //            HistEntity.Email = UpdateEntity.Email;
        //            HistEntity.DOB = UpdateEntity.DOB;
        //            HistEntity.Status = UpdateEntity.Status;
        //            HistEntity.Client = UpdateEntity.Client;
        //            HistEntity.Notes = UpdateEntity.Notes;

        //            HistEntity.Mode = "Add";

        //            UpdateEntity.Mode = "Delete";

        //            if (_model.TMS00110Data.InsertUpdateDelAPPTSCHDHIST(HistEntity))
        //            {
        //                if (Rec_type == 'U')
        //                {
        //                    _model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity);
        //                    MessageBox.Show("Appointment Cancelled Successfully", "CAP Systems", MessageBoxButtons.OK);

        //                    Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
        //                    Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
        //                    SchAppGrid.Enabled = Refresh_Control = Just_Saved = true;
        //                    Enable_DisableBottomControls(false);
        //                    Btn_Update.Visible = BtnCancel.Visible = false;
        //                    //this.DialogResult = DialogResult.OK;
        //                    AppDate_LostFocus(AppDate, e);
        //                }
        //            }


        //        }
        //        else
        //        {
        //            if (_model.TMS00110Data.InsertUpdateDelAPPTSCHED(UpdateEntity))
        //            {
        //                if (Rec_type == 'U')
        //                    MessageBox.Show("Record Updated Successfully", "CAP Systems", MessageBoxButtons.OK);
        //                else
        //                {
        //                    MessageBox.Show("Record Inserted Successfully", "CAP Systems", MessageBoxButtons.OK);
        //                    NowSch_Slot_Flag = false;
        //                }
        //                Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = false;
        //                Lbl_Intake_Info.Text = Lbl_LPB_Info.Text = "";
        //                SchAppGrid.Enabled = Refresh_Control = Just_Saved = true;
        //                Enable_DisableBottomControls(false);
        //                Btn_Update.Visible = BtnCancel.Visible = false;
        //                //this.DialogResult = DialogResult.OK;
        //                AppDate_LostFocus(AppDate, e);
        //            }
        //            else
        //            {
        //                //Consts.Messages.UserCreatedSuccesssfully.DisplayFirendlyMessage(Captain.Common.Exceptions.ExceptionSeverityLevel.Information);
        //                MessageBox.Show("Slot already booked by another user!!!", "CAP Systems");
        //                AppDate_LostFocus(AppDate, e);
        //            }
        //        }

                
        //    }
        //    //}

        //    if (Mode != "Add")
        //        TxtSiteCode.Enabled = BtnSearch.Enabled = AppDate.Enabled = false;
        //}

     

        List<TmsApptEntity> Date_Appt_List = new List<TmsApptEntity>();
        //  APPTSCHEDULEEntity UpdateEntity;
        int scrollPosition, CurrentPage;
        bool Just_Saved = false;
        //private void SchAppGrid_SelectionChanged(object sender, EventArgs e)
        //{
        //    Clear_RequiredIcons();
        //    Lbl_Intake_Info.Visible = Lbl_LPB_Info.Visible = Lbl_Now_Sch.Visible = Lbl_Reserved.Visible = false;
        //    if (!Next_Slot_Process)
        //    {
        //        // List<TmsApptEntity> ApptRecord = new List<TmsApptEntity>();
        //        if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y")
        //        {

        //            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty,string.Empty, string.Empty, string.Empty);


        //            // APPTSCHEDULEEntity ApptUpdateEntity = SchAppGrid.CurrentRow.Tag as APPTSCHEDULEEntity;

        //            if (ApptRecord.Count > 0)
        //            {
        //                APPTSCHEDULEEntity ApptUpdateEntity = ApptRecord[0];
        //                if (ApptRecord[0].SchdType != "3")
        //                {
        //                    //if (Mode.Equals("Add"))
        //                    //    Enable_DisableBottomControls(false);
        //                    //else
        //                    //    Enable_DisableBottomControls(true);

        //                    Set_Menu_Buttons_Visibility("Edit");

        //                    if (ApptUpdateEntity.DOB != string.Empty)
        //                    {
        //                        dtBirth.Value = Convert.ToDateTime(ApptUpdateEntity.DOB);
        //                        dtBirth.Checked = true;
        //                    }
        //                    else
        //                    {
        //                        dtBirth.Checked = false;
                                
        //                    }
        //                    TxtLstName.Text = ApptUpdateEntity.LastName;
        //                    TxtFrstName.Text = ApptUpdateEntity.FirstName;
        //                    //MskPhone.Text = UpdateEntity.TelAreaCode + UpdateEntity.TelNumber;
        //                    MskPhone.Text = ApptUpdateEntity.TelNumber;
        //                    TxtHn.Text = ApptUpdateEntity.HNo;
        //                    TxtStreet.Text = ApptUpdateEntity.Street;

        //                    TxtSf.Text = ApptUpdateEntity.Suffix;
        //                    TxtApt.Text = ApptUpdateEntity.Apt;
        //                    TxtFlr.Text = ApptUpdateEntity.Floor;
        //                    TxtCity.Text = ApptUpdateEntity.City;
        //                    TxtState.Text = ApptUpdateEntity.State;
        //                    TxtEMail.Text = ApptUpdateEntity.Email.Trim();
        //                    TxtZip.Text = (!string.IsNullOrEmpty(ApptUpdateEntity.Zip1.Trim()) ? "00000".Substring(0, 5 - ApptUpdateEntity.Zip1.Trim().Length) + ApptUpdateEntity.Zip1 : "");
        //                    TxtZip1.Text = (!string.IsNullOrEmpty(ApptUpdateEntity.Zip2.Trim()) ? "0000".Substring(0, 4 - ApptUpdateEntity.Zip2.Trim().Length) + ApptUpdateEntity.Zip2 : "");

        //                    if (ApptUpdateEntity.Sex != string.Empty)
        //                        CommonFunctions.SetComboBoxValue(CmbGen, ApptUpdateEntity.Sex);
        //                    else
        //                        CmbGen.SelectedIndex = 0;

        //                    if (ApptUpdateEntity.CaseWorker != string.Empty)
        //                        CommonFunctions.SetComboBoxValue(CmbWorker, ApptUpdateEntity.CaseWorker);
        //                    else
        //                        CmbWorker.SelectedIndex = 0;

        //                    if (ApptUpdateEntity.HeatSource != string.Empty)
        //                        CommonFunctions.SetComboBoxValue(CmbHtSrc, ApptUpdateEntity.HeatSource);
        //                    else
        //                        CmbHtSrc.SelectedIndex = 0;

        //                    if (ApptUpdateEntity.CellProvider != string.Empty)
        //                        CommonFunctions.SetComboBoxValue(CmbCellProv, ApptUpdateEntity.CellProvider);
        //                    else
        //                        CmbCellProv.SelectedIndex = 0;

        //                    if (ApptUpdateEntity.Status != string.Empty)
        //                        CommonFunctions.SetComboBoxValue(cmbStatus, ApptUpdateEntity.Status);
        //                    else
        //                        cmbStatus.SelectedIndex = 0;

        //                    if (ApptUpdateEntity.Client!= string.Empty)
        //                        CommonFunctions.SetComboBoxValue(cmbClientStatus, ApptUpdateEntity.Client);
        //                    else
        //                        cmbClientStatus.SelectedIndex = 0;

        //                    txtIncSrc.Text = ApptUpdateEntity.SourceIncome;
        //                    //CmbCellProv.Text = UpdateEntity.CellProvider;
        //                    //MskCell.Text = UpdateEntity.CellAreaCode + UpdateEntity.CellNumber;
        //                    MskCell.Text = ApptUpdateEntity.CellNumber;
        //                    DtpCntrctDt.Text = ApptUpdateEntity.ContactDate;

        //                    if (!(string.IsNullOrEmpty(ApptUpdateEntity.ContactDate.Trim())))
        //                    {
        //                        DtpCntrctDt.Value = Convert.ToDateTime(ApptUpdateEntity.ContactDate);
        //                        DtpCntrctDt.Checked = true;
        //                    }
        //                    else
        //                    {
        //                        DtpCntrctDt.Value = DateTime.Today;
        //                        DtpCntrctDt.Checked = false;
        //                    }

        //                    txtNotes.Text = ApptUpdateEntity.Notes.Trim();

        //                    SchAppGrid.CurrentRow.Cells["Rec_Type"].Value = ApptUpdateEntity.SchdType;

        //                    //if (UpdateEntity.RecordType == "2")
        //                    //    BtnUpdate.Visible = false;
        //                    //else
        //                    //    BtnUpdate.Visible = true;
        //                }
        //                else
        //                {
        //                    ClearBottomControls();
        //                    Set_Menu_Buttons_Visibility("Add");
        //                }
        //            }
        //            else
        //            {
        //                ClearBottomControls();
        //                Set_Menu_Buttons_Visibility("Add");
        //            }

        //            //else
        //            //{
        //            //    ClearBottomControls();
        //            //    Set_Menu_Buttons_Visibility("Add");
        //            //    MessageBox.Show("Record Deleted by by another user!!!", "CAP Systems");
        //            //   // AppDate_LostFocus(AppDate, e);
        //            //}
        //        }
        //        else
        //        {
        //            ClearBottomControls();
        //            Set_Menu_Buttons_Visibility("Add");
        //        }
        //    }
        //    else
        //    {
        //        ClearBottomControls(); //LblAppMode.Text = "Add Appointment";
        //    }

        //    Lbl_Now_Sch.Visible = Lbl_Reserved.Visible = false;
        //    if (SchAppGrid.CurrentRow.Cells["AppStatus"].Value.ToString() == "Y") //&& !Next_Slot_Process
        //    {
        //        //LblAppMode.Text = "Edit Appointment";
        //        dtBirth.Enabled = false; BtnSsnSearch.Enabled = false;
        //        if (SchAppGrid.CurrentRow.Cells["Rec_Type"].Value.ToString() == "2")
        //        {
        //            Btn_Update.Visible = false;
        //            Pb_Add.Visible = Pb_Delete.Visible = Pb_Edit.Visible = false;
        //            Enable_DisableBottomControls(false);
        //            //BtnIncSource.Enabled = BtnZip.Enabled = false;
        //            Lbl_Reserved.Visible = true;
        //        }
        //        else if (SchAppGrid.CurrentRow.Cells["Rec_Type"].Value.ToString() == "3")
        //        {
        //            Lbl_Now_Sch.Visible = true;
        //            Btn_Update.Visible = false;
        //            Pb_Add.Visible = Pb_Delete.Visible = Pb_Edit.Visible = false;
        //            Enable_DisableBottomControls(false);
        //            //BtnIncSource.Enabled = BtnZip.Enabled = false;
        //        }
        //        //else
        //        //{
        //        //    Btn_Update.Visible = true;
        //        //    Enable_DisableBottomControls(true);
        //        //    BtnIncSource.Enabled = BtnZip.Enabled = true;
        //        //}
        //    }
        //    //else
        //    //    {
        //    //        LblAppMode.Text = "Add Appointment";
        //    //        MskSsn.Enabled = true; BtnSsnSearch.Enabled = true;
        //    //        DtpCntrctDt.Value = DateTime.Today;
        //    //        DtpCntrctDt.Checked = false;
        //    //        if (Privileges.AddPriv.Equals("true"))
        //    //        {
        //    //            Btn_Update.Visible = true;
        //    //            Enable_DisableBottomControls(true);
        //    //            BtnIncSource.Enabled = BtnZip.Enabled = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            Btn_Update.Visible = false;
        //    //            Enable_DisableBottomControls(false);
        //    //            BtnIncSource.Enabled = BtnZip.Enabled = false;
        //    //        }
        //    //    }

        //    if (!Just_Saved)
        //    {
        //        CurrentPage = SchAppGrid.CurrentPage;
        //        scrollPosition = SchAppGrid.CurrentCell.RowIndex;
        //    }
        //}

        //private void ClearBottomControls()
        //{
        //    //if (Mode.Equals("Edit"))
        //    //    Enable_DisableBottomControls(false);
        //    //else
        //    //    Enable_DisableBottomControls(true); 
        //     TxtLstName.Clear();
        //    TxtFrstName.Clear(); MskPhone.Clear();
        //    TxtHn.Clear(); TxtStreet.Clear();
        //    TxtSf.Clear(); TxtApt.Clear();
        //    TxtFlr.Clear(); TxtCity.Clear();
        //    TxtState.Clear(); TxtZip.Clear();
        //    TxtZip1.Clear(); MskCell.Clear();
        //    txtIncSrc.Clear(); TxtEMail.Clear();

        //    txtNotes.Clear();

        //    CmbGen.SelectedIndex = 0;
        //    CmbWorker.SelectedIndex = 0;
        //    CmbHtSrc.SelectedIndex = 0;
        //    CmbCellProv.SelectedIndex = 0;
        //    dtBirth.Value = DateTime.Today;
        //    dtBirth.Checked = false;
        //    DtpCntrctDt.Value = DateTime.Today;
        //    DtpCntrctDt.Checked = false;

        //    cmbClientStatus.SelectedIndex = 0;cmbStatus.SelectedIndex = 0;
        //}


        
      

        bool Rec_Sel_From_SSN_Search = false;
        string[] Sel_SSN_Rec = new string[2];
        
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void Hepl_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Scheduled Appointments");
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

            
            propListCaseWorker = new List<ListItem>();
            
            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            DataTable dt = ds1.Tables[0];
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    //if ((Mode == "Add" && dr["PWH_INACTIVE"].ToString().Trim() == "N")) && (Mode == "Edit"))
                    propListCaseWorker.Add(new ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim(), dr["PWH_INACTIVE"].ToString(), (dr["PWH_INACTIVE"].ToString().Equals("Y")) ? Color.Red : Color.Green));
                }
            }
            
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
                FillGrid();
            }
            else
                AlertBox.Show("Please Select at least One Appointment to Delete", MessageBoxIcon.Warning);
        }

        
       
        private void TMS00110Form_Load(object sender, EventArgs e)
        {
            //this.Size = new System.Drawing.Size(768, 598);
            switch (Mode)
            {
                case "Edit":
                    //this.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
                    TxtSiteCode.Text = SchSite;
                    //if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                    //    BtnNextSlot.Visible = true;
                    //else
                    //    BtnNextSlot.Visible = false;
                    AppDate.Value = Convert.ToDateTime(SchDate);
                    AppDate.Checked = true;
                    AppDate.Focus();
                    FillGrid();
                    this.Name.Width = 250;
                    break;
                case "Add":
                    this.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
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
                            //if (!string.IsNullOrEmpty(TxtSiteCode.Text.Trim()))
                            //    BtnNextSlot.Visible = true;
                            //else
                            //    BtnNextSlot.Visible = false;
                            FillGrid();
                        }
                    }
                    break;
                case "Delete":
                    this.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
                    TxtSiteCode.Text = SchSite;
                    this.Delete.Visible = true;
                    //LblAppMode.Visible = false;
                    AppDate.Value = Convert.ToDateTime(SchDate);
                    AppDate.Checked = true;
                    AppDate.Focus();
                    FillGrid();
                    Btn_Update.Text = "Delete";
                    
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
                FillGrid();

            Priv_Sel_Site = TxtSiteCode.Text;
        }

        
        
        private bool Update_Edit_Stamping()
        {
            List<APPTSCHEDULEEntity> ApptRecord = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, TxtSiteCode.Text, AppDate.Value.ToShortDateString(), SchAppGrid.CurrentRow.Cells["KeyTime"].Value.ToString(), SchAppGrid.CurrentRow.Cells["SlotNo"].Value.ToString(), string.Empty, BaseForm.UserID, string.Empty, string.Empty, string.Empty,string.Empty, string.Empty);
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
        
        
        bool Refresh_Control = false;

        private void MskSsn_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnHist_Click(object sender, EventArgs e)
        {

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