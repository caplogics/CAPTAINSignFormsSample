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
    public partial class Sub_TMS00110_SsnNameSearch : Form
    {
        public Sub_TMS00110_SsnNameSearch(string hierarchy, string sel_site, string strScreenName,BaseForm baseForm)
        {
            InitializeComponent();
            Hierarchy = hierarchy;
            Sel_Site = string.Empty;
            BaseForm = baseForm;
            if (!string.IsNullOrEmpty(sel_site))
                Sel_Site = sel_site;

            PropStatus = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00125", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); 

            FillDropDown();

            propScreenName = strScreenName;
            this.Text = "Name/DOB Search";

            if(propScreenName == "APPT0002")
            {
                if (SearchGrid.Rows.Count > 0)
                {
                    lblMonths.Visible = true;
                    cmbMonth.Visible = true;
                }
                FillMonths();
            }

            Txt_Phone.Validator = TextBoxValidation.IntegerValidator;
            //this.Phone_Panel.Location = new System.Drawing.Point(194, 3);

           
        }

        public string Hierarchy { get; set; }
        public string Sel_Site { get; set; }
        public string propScreenName { get; set; }
        public BaseForm BaseForm { get; set; }
        public List<CommonEntity> PropStatus { get; set; }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            BtnSelect.Visible = false;
            string strDOB = string.Empty;
            if (string.IsNullOrEmpty(TxtLastName.Text))
                TxtLastName.Text = null;
            if (string.IsNullOrEmpty(TxtFirstName.Text))
                TxtFirstName.Text = null;
            if (dtBirth.Checked==true)
                strDOB = dtBirth.Value.ToShortDateString();

            if (string.IsNullOrEmpty(Txt_Phone.Text.Trim()))
                Txt_Phone.Text = string.Empty;

            //switch(((ListItem)CmbSearchBy.SelectedItem).Value.ToString())


            int rowIndex = 0, Row_Cnt = 0;
            string ApptKey = null;
            string AppDate = null;
            int Compare_Hours = 0;
            string SlotNo = null;
            string TmpSsn = null;
            string Time = null, Disp_Hours = null, Disp_Min = null, AM_PM = "AM", Location = "";
            if (propScreenName == "APPT0002")
            {
                this.Name.Width = 170;
                this.Phone.Width = 80;

                SearchGrid.Rows.Clear();
                CaptainModel _model = new CaptainModel();
                List<APPTSCHEDULEEntity> apptSchedulesearchlist = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty , string.Empty,TxtLastName.Text,TxtFirstName.Text,Txt_Phone.Text, strDOB, string.Empty);
                List<APPTSCHDHISTEntity> propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHDHISTBrowse(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, TxtLastName.Text, TxtFirstName.Text, Txt_Phone.Text, strDOB, string.Empty);
                if(propAPPtSchedulelist.Count>0)
                {
                    foreach(APPTSCHDHISTEntity Entity in propAPPtSchedulelist)
                    {
                        APPTSCHEDULEEntity SelEntity = new APPTSCHEDULEEntity(true);
                        SelEntity.Agency = Entity.Agency;SelEntity.Dept = Entity.Dept;SelEntity.Program = Entity.Program;SelEntity.Year = Entity.Year;SelEntity.Site = Entity.Site;
                        SelEntity.Date = Entity.Date;SelEntity.Time = Entity.Time;SelEntity.SlotNumber = Entity.SlotNumber;SelEntity.SsNumber = Entity.SsNumber;SelEntity.TemplateID = Entity.TemplateID;
                        SelEntity.SchdType = Entity.SchdType;SelEntity.SchdDay = Entity.SchdDay; SelEntity.LastName = Entity.LastName;SelEntity.FirstName = Entity.FirstName;SelEntity.DOB = Entity.DOB;
                        SelEntity.TelNumber = Entity.TelNumber;SelEntity.Status = Entity.Status; SelEntity.CellNumber = Entity.CellNumber;

                        apptSchedulesearchlist.Add(SelEntity);
                    }
                }
                if (apptSchedulesearchlist.Count > 0)
                {
                    apptSchedulesearchlist = apptSchedulesearchlist.OrderBy(u => u.Site).ThenBy(u => Convert.ToDateTime(u.Date)).ThenBy(u => u.Time).ToList();

                    foreach (APPTSCHEDULEEntity drschedulelist in apptSchedulesearchlist)
                    {
                        Location = drschedulelist.Site.ToString().Trim();
                        if (Cb_All_Sites.Checked || Location == Sel_Site)
                        {
                            rowIndex = 0;
                            AM_PM = "AM";
                            ApptKey = Hierarchy + Location + "    ".Substring(0, 4 - Location.Length) + drschedulelist.Date.ToString();

                            AppDate = null;
                            AppDate = drschedulelist.Date;
                            string[] time = Regex.Split(AppDate.ToString(), " ");
                            AppDate = time[0];

                            Time = drschedulelist.Time;
                            Time = "0000".Substring(0, 4 - Time.Length) + Time;
                            Disp_Hours = Time.Substring(0, 2);
                            Disp_Min = Time.Substring(2, 2);
                            Compare_Hours = int.Parse(Disp_Hours);
                            if (Compare_Hours > 11)
                            {
                                AM_PM = "PM";
                                if (Compare_Hours > 12)
                                    Compare_Hours -= 12;
                            }

                            Disp_Hours = Compare_Hours.ToString();
                            Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;
                            Time = Disp_Hours + ":" + Disp_Min + " " + AM_PM;

                            if (!string.IsNullOrEmpty(AppDate.Trim()))
                                AppDate = LookupDataAccess.Getdate(AppDate.Trim()
                                    );// CommonFunctions.ChangeDateFormat(Convert.ToDateTime(AppDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);


                            // TmpSsn = string.Empty;
                            //if (drschedulelist.SsNumber.Trim() != string.Empty)
                            //    TmpSsn = LookupDataAccess.GetCardNo(drschedulelist.SsNumber, "1", "N", string.Empty);

                            string strPhoneNumber = LookupDataAccess.GetPhoneFormat(drschedulelist.TelNumber);
                            if (drschedulelist.CellNumber != string.Empty)
                                strPhoneNumber = LookupDataAccess.GetPhoneFormat(drschedulelist.CellNumber);

                            string strStatus = string.Empty;
                            if (!string.IsNullOrEmpty(drschedulelist.Status.ToString().Trim()))
                            {
                                if (PropStatus.Count > 0)
                                {
                                    CommonEntity SelStatus = PropStatus.Find(u => u.Code.Trim().Equals(drschedulelist.Status.Trim()));
                                    if (SelStatus != null)
                                        strStatus = SelStatus.Desc.Trim();
                                }
                            }

                            rowIndex = SearchGrid.Rows.Add(drschedulelist.Site, AppDate, Time, drschedulelist.SlotNumber, LookupDataAccess.Getdate(drschedulelist.DOB), drschedulelist.LastName + "   " + drschedulelist.FirstName, strPhoneNumber, strStatus, ApptKey,drschedulelist.Status);
                            Row_Cnt++;
                            //if (propAPPtSchedulelist.Count > 0)
                            //{
                            //    List<APPTSCHDHISTEntity> selHistEntity = propAPPtSchedulelist.FindAll(u => u.Site.Equals(drschedulelist.Site) && u.Date.Equals(drschedulelist.Date) && u.Time.Equals(drschedulelist.Time));
                            //    if (selHistEntity.Count > 0)
                            //    {
                            //        foreach (APPTSCHDHISTEntity Entity in selHistEntity)
                            //        {
                            //            AM_PM = "AM";
                            //            ApptKey = Hierarchy + Location + "    ".Substring(0, 4 - Location.Length) + Entity.Date.ToString();

                            //            AppDate = null;
                            //            AppDate = Entity.Date;
                            //            string[] time1 = Regex.Split(AppDate.ToString(), " ");
                            //            AppDate = time1[0];

                            //            Time = Entity.Time;
                            //            Time = "0000".Substring(0, 4 - Time.Length) + Time;
                            //            Disp_Hours = Time.Substring(0, 2);
                            //            Disp_Min = Time.Substring(2, 2);
                            //            Compare_Hours = int.Parse(Disp_Hours);
                            //            if (Compare_Hours > 11)
                            //            {
                            //                AM_PM = "PM";
                            //                if (Compare_Hours > 12)
                            //                    Compare_Hours -= 12;
                            //            }

                            //            Disp_Hours = Compare_Hours.ToString();
                            //            Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;
                            //            Time = Disp_Hours + ":" + Disp_Min + " " + AM_PM;

                            //            if (!string.IsNullOrEmpty(AppDate.Trim()))
                            //                AppDate = LookupDataAccess.Getdate(AppDate.Trim());

                            //            string strPhoneNumber1 = LookupDataAccess.GetPhoneFormat(Entity.TelNumber);
                            //            if (Entity.CellNumber != string.Empty)
                            //                strPhoneNumber1 = LookupDataAccess.GetPhoneFormat(Entity.CellNumber);

                            //            string strStatus1 = string.Empty;
                            //            if (!string.IsNullOrEmpty(Entity.Status.ToString().Trim()))
                            //            {
                            //                if (PropStatus.Count > 0)
                            //                {
                            //                    CommonEntity SelStatus = PropStatus.Find(u => u.Code.Trim().Equals(Entity.Status.Trim()));
                            //                    if (SelStatus != null)
                            //                        strStatus1 = SelStatus.Desc.Trim();
                            //                }
                            //            }

                            //            rowIndex = SearchGrid.Rows.Add(Entity.Site, AppDate, Time, Entity.SlotNumber, LookupDataAccess.Getdate(Entity.DOB), Entity.LastName + "   " + Entity.FirstName, strPhoneNumber1, strStatus1, ApptKey,Entity.Status);
                            //            Row_Cnt++;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                //else if(propAPPtSchedulelist.Count>0)
                //{
                //    foreach (APPTSCHDHISTEntity Entity in propAPPtSchedulelist)
                //    {
                //        AM_PM = "AM";
                //        ApptKey = Hierarchy + Location + "    ".Substring(0, 4 - Location.Length) + Entity.Date.ToString();

                //        AppDate = null;
                //        AppDate = Entity.Date;
                //        string[] time1 = Regex.Split(AppDate.ToString(), " ");
                //        AppDate = time1[0];

                //        Time = Entity.Time;
                //        Time = "0000".Substring(0, 4 - Time.Length) + Time;
                //        Disp_Hours = Time.Substring(0, 2);
                //        Disp_Min = Time.Substring(2, 2);
                //        Compare_Hours = int.Parse(Disp_Hours);
                //        if (Compare_Hours > 11)
                //        {
                //            AM_PM = "PM";
                //            if (Compare_Hours > 12)
                //                Compare_Hours -= 12;
                //        }

                //        Disp_Hours = Compare_Hours.ToString();
                //        Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;
                //        Time = Disp_Hours + ":" + Disp_Min + " " + AM_PM;

                //        if (!string.IsNullOrEmpty(AppDate.Trim()))
                //            AppDate = LookupDataAccess.Getdate(AppDate.Trim());

                //        string strPhoneNumber1 = LookupDataAccess.GetPhoneFormat(Entity.TelNumber);
                //        if (Entity.CellNumber != string.Empty)
                //            strPhoneNumber1 = LookupDataAccess.GetPhoneFormat(Entity.CellNumber);

                //        string strStatus1 = string.Empty;
                //        if (!string.IsNullOrEmpty(Entity.Status.ToString().Trim()))
                //        {
                //            if (PropStatus.Count > 0)
                //            {
                //                CommonEntity SelStatus = PropStatus.Find(u => u.Code.Trim().Equals(Entity.Status.Trim()));
                //                if (SelStatus != null)
                //                    strStatus1 = SelStatus.Desc.Trim();
                //            }
                //        }

                //        rowIndex = SearchGrid.Rows.Add(Entity.Site, AppDate, Time, Entity.SlotNumber, LookupDataAccess.Getdate(Entity.DOB), Entity.LastName + "   " + Entity.FirstName, strPhoneNumber1, strStatus1, ApptKey,Entity.Status);
                //        Row_Cnt++;
                //    }
                //}
                
                
            }
            else
            {
                //DataSet dsAppt = Captain.DatabaseLayer.TMS00110DB.Browse_TMSAPPT(Hierarchy, null, null, null, null,  MtxtSsn.Text, null, null, TxtLastName.Text.Trim(), TxtFirstName.Text.Trim(),  null, null, null, null, null, null, null, null, null );
                DataSet dsAppt = Captain.DatabaseLayer.TMS00110DB.TMS00110_Browse_TMSAPPT_SSNSearch(Hierarchy, string.Empty, TxtLastName.Text.Trim(), TxtFirstName.Text.Trim(), null, Txt_Phone.Text);
                DataTable dtAppt = new DataTable();

                if (dsAppt.Tables.Count > 0)
                    dtAppt = dsAppt.Tables[0];

                this.Status.Visible = false;
                this.Name.Width = 320;

                SearchGrid.Rows.Clear();

                if (dtAppt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dtAppt.Rows)
                    {
                        Location = dr["TMSAPPT_LOCATION"].ToString().Trim();
                        if (Cb_All_Sites.Checked || Location == Sel_Site)
                        {
                            rowIndex = 0;
                            AM_PM = "AM";
                            ApptKey = Hierarchy + Location + "    ".Substring(0, 4 - Location.Length) + dr["TMSAPPT_DATE"].ToString();

                            AppDate = null;
                            AppDate = dr["TMSAPPT_DATE"].ToString();
                            string[] time = Regex.Split(AppDate.ToString(), " ");
                            AppDate = time[0];

                            Time = dr["TMSAPPT_TIME"].ToString();
                            Time = "0000".Substring(0, 4 - Time.Length) + Time;
                            Disp_Hours = Time.Substring(0, 2);
                            Disp_Min = Time.Substring(2, 2);
                            Compare_Hours = int.Parse(Disp_Hours);
                            if (Compare_Hours > 11)
                            {
                                AM_PM = "PM";
                                if (Compare_Hours > 12)
                                    Compare_Hours -= 12;
                            }


                            Disp_Hours = Compare_Hours.ToString();
                            Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;
                            Time = Disp_Hours + ":" + Disp_Min + " " + AM_PM;

                            if (!string.IsNullOrEmpty(AppDate.Trim()))
                                AppDate = CommonFunctions.ChangeDateFormat(Convert.ToDateTime(AppDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                            SlotNo = "00".Substring(0, 2 - dr["TMSAPPT_SLOT_NUMBER"].ToString().Length) + dr["TMSAPPT_SLOT_NUMBER"].ToString();

                            //if (!(string.IsNullOrEmpty(dr["TMSAPPT_SS_NUMBER"].ToString())) && dr["TMSAPPT_SS_NUMBER"].ToString().Length == 9)
                            //    TmpSsn = dr["TMSAPPT_SS_NUMBER"].ToString().Substring(0, 3) + "-" + dr["TMSAPPT_SS_NUMBER"].ToString().Substring(3, 2) + "-" + dr["TMSAPPT_SS_NUMBER"].ToString().Substring(5, 4);


                            //Pass_Min = "0000".Length - dr["TMSAPPT_TIME"].ToString().Length + Pass_Min;
                            //Disp_Hours = "00".Substring(0, 2 - Disp_Hours.Length) + Disp_Hours;

                            rowIndex = SearchGrid.Rows.Add(dr["TMSAPPT_LOCATION"], AppDate, Time, SlotNo, TmpSsn, dr["TMSAPPT_NAME"].ToString().Trim() + "   " + dr["TMSAPPT_FIRST_NAME"].ToString().Trim(), dr["TMSAPPT_TEL_NUMBER"].ToString().Trim(),string.Empty, ApptKey,string.Empty);
                            Row_Cnt++;
                        }
                    }
                }
                

            }
            if (Row_Cnt > 0)
            {
                SearchGrid.Rows[0].Tag = 0;
                BtnSelect.Visible = true;
                lblMonths.Visible = cmbMonth.Visible = true;
            }
        }

        private void FillDropDown()
        {
            CmbSearchBy.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("NAME", "1"));
            listItem.Add(new ListItem("PHONE", "3"));
            listItem.Add(new ListItem("DOB", "2"));
            CmbSearchBy.Items.AddRange(listItem.ToArray());
            CmbSearchBy.SelectedIndex = 0;
        }

        private void FillMonths()
        {
            cmbMonth.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("", "0"));
            listItem.Add(new ListItem("1", "1"));
            listItem.Add(new ListItem("2", "2"));
            listItem.Add(new ListItem("3", "3"));
            listItem.Add(new ListItem("4", "4"));
            listItem.Add(new ListItem("5", "5"));
            listItem.Add(new ListItem("6", "6"));
            listItem.Add(new ListItem("7", "7"));
            listItem.Add(new ListItem("8", "8"));
            listItem.Add(new ListItem("9", "9"));
            listItem.Add(new ListItem("10", "10"));
            listItem.Add(new ListItem("11", "11"));
            listItem.Add(new ListItem("12", "12"));
            cmbMonth.Items.AddRange(listItem.ToArray());
            cmbMonth.SelectedIndex = 0;
        }

        private void CmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
              TxtLastName.Text = TxtFirstName.Text = Txt_Phone.Text = string.Empty;
            dtBirth.Checked = false;
            switch (((ListItem)CmbSearchBy.SelectedItem).Value.ToString())
            {
                case "1":
                    Phone_Panel.Visible = SsnPanel.Visible = false; NamePanel.Visible = true;
                    dtBirth.Checked = false;
                    panel2.Height = 72;
                    break;
                case "2":
                    SsnPanel.Visible = true; Phone_Panel.Visible = NamePanel.Visible = false;
                    TxtLastName.Text = TxtFirstName.Text = null;
                   // SsnPanel.Location = new System.Drawing.Point(194, 3);
                    panel2.Height = 45;
                    break;
                case "3":
                    Phone_Panel.Visible = true; SsnPanel.Visible = NamePanel.Visible = false;
                    Txt_Phone.Text = string.Empty;
                    //Phone_Panel.Location = new System.Drawing.Point(194, 3);
                    panel2.Height = 45;
                    break;
            }
        }


        public string[] GetSelectedApplicant()
        {
            string[] SelAppKey = new string[5];

            if (!(string.IsNullOrEmpty(SearchGrid.CurrentRow.Cells["ApptKey"].Value.ToString())))
            {
                SelAppKey[0] = SearchGrid.CurrentRow.Cells["ApptKey"].Value.ToString();
                SelAppKey[1] = SearchGrid.CurrentRow.Cells["gvDate"].Value.ToString();
                SelAppKey[2] = SearchGrid.CurrentRow.Cells["Time"].Value.ToString();
                SelAppKey[3] = SearchGrid.CurrentRow.Cells["Slot"].Value.ToString();
                if (propScreenName == "APPT0002")
                    SelAppKey[4] = ((ListItem)cmbMonth.SelectedItem).Value.ToString();
                else SelAppKey[4] = string.Empty;
            }

            return SelAppKey;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SearchGrid_SelectionChanged(object sender, EventArgs e)
        {
            if(SearchGrid.Rows.Count>0)
            {
                if (SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "03" || SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "04" || SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "05")
                {
                    if (((ListItem)cmbMonth.SelectedItem).Value.ToString() != "0")
                        BtnSelect.Enabled = true;
                    else BtnSelect.Enabled = false;
                    
                }
                else
                    BtnSelect.Enabled = true;
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbMonth.Items.Count>0)
            {
                if(((ListItem)cmbMonth.SelectedItem).Value.ToString()!="0")
                {
                    if (SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "03" || SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "04" || SearchGrid.CurrentRow.Cells["StatusCode"].Value.ToString() == "05")
                    {
                        BtnSelect.Enabled = true;
                    }
                    else
                        BtnSelect.Enabled = true;
                }
            }
        }
    }
}