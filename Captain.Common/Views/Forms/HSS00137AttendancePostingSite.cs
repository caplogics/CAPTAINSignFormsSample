#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Model.Data;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00137AttendancePostingSite : Form
    {
        private CaptainModel _model = null;
        public HSS00137AttendancePostingSite(BaseForm baseForm, PrivilegeEntity privileges, List<CaseEnrlEntity> caseEnrlList, List<CaseSnpEntity> casesnpList, List<CaseMstEntity> CaseMstList,string SiteRoomAmpm, string Site,string Room,string Ampm,  string strStatus,string strMonth,string strOrderBy)
        {
            InitializeComponent();
            _model = new CaptainModel();          
            BaseForm = baseForm;
            Privileges = privileges;
            propCaseENrl = caseEnrlList;
            propCaseSnp = casesnpList;
            propCaseMst = CaseMstList;
            propSiteRoomAmpm = SiteRoomAmpm;
            propStatus = strStatus;
            propMonth = strMonth;
            propSite = Site;
            propRoom = Room;
            propAmpm = Ampm;
            propOrderBy = strOrderBy;
            cmbMonth.SelectedIndexChanged -=new EventHandler(cmbMonth_SelectedIndexChanged);
            FillCombo();
            this.Text = /*privileges.Program + " -*/ "Attendance Summary";
            if (propMonth.Length == 1)
            {
                propMonth = "0" + propMonth;
            }
            CommonFunctions.SetComboBoxValue(cmbMonth, propMonth);
            LoadGridData();
            cmbMonth.SelectedIndexChanged += new EventHandler(cmbMonth_SelectedIndexChanged);

        }

        #region Properties

        public BaseForm BaseForm { get; set; }
       

        public string Mode { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public List<CaseEnrlEntity> propCaseENrl { get; set; }
        public List<CaseSnpEntity> propCaseSnp { get; set; }
        public List<CaseMstEntity> propCaseMst { get; set; }
        public string propSiteRoomAmpm { get; set; }
        public string propStatus { get; set; }
        public string propMonth { get; set; }
        public string propSite { get; set; }
        public string propRoom { get; set; }
        public string propAmpm { get; set; }
        public string propOrderBy { get; set; }

        #endregion

        private void FillCombo()
        {
            List<CommonEntity> AgyTabs_List = CommonFunctions.AgyTabsFilterOrderbyCode(BaseForm.BaseAgyTabsEntity, "SMONT", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.AdhocData.Browse_AGYTABS(searchAgytabs);
            foreach (CommonEntity item in AgyTabs_List)
            {
                cmbMonth.Items.Add(new ListItem(item.Desc, item.Code));
            }
            cmbMonth.SelectedIndex = 0;
        }

        private void LoadGridData()
        {
            gvwAttendance.Rows.Clear();
            if (propCaseENrl.Count > 0)
            {
             List<ChldAttnEntity> chldAttnList =   _model.ChldAttnData.GetChldAttnCountMonth(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,BaseForm.BaseYear, string.Empty, propSite, propRoom, propAmpm, string.Empty, propMonth);
                List<CaseEnrlEntity> caseEnrlChildList = null;
                if (propStatus == "B")
                {
                    caseEnrlChildList = propCaseENrl.FindAll(u => (u.Site + u.Room + u.AMPM).Equals(propSiteRoomAmpm) && (u.Status == "W" || u.Status == "E" || u.Status == "T"));
                }
                else if (propStatus == "W")
                {
                    caseEnrlChildList = propCaseENrl.FindAll(u => (u.Site + u.Room + u.AMPM).Equals(propSiteRoomAmpm) && (u.Status == "W" || u.Status == "T"));
                }
                else
                {
                    caseEnrlChildList = propCaseENrl.FindAll(u => (u.Site + u.Room + u.AMPM).Equals(propSiteRoomAmpm) && (u.Status == "E"));
                }
                if (caseEnrlChildList.Count > 0)
                {
                    string  intPresent = "0";
                    string intAbsent =  "0";
                    string intLegal =  "0";
                        foreach (CaseEnrlEntity item in caseEnrlChildList)
                        {
                            intPresent =  "0";
                            intAbsent =  "0";
                            intLegal =  "0";
                            if (chldAttnList.Count > 0)
                            {
                                ChldAttnEntity chldattndata = chldAttnList.Find(u => u.APP_NO == item.App && u.FUNDING_SOURCE == item.FundHie);
                                if (chldattndata != null)
                                {
                                    intPresent = chldattndata.PresentDays;
                                    intAbsent = chldattndata.AbsentDays;
                                    intLegal = chldattndata.LegalDays;
                                }
                            }
                            int introwIndex = 0;
                            CaseSnpEntity caseSnp = propCaseSnp.Find(u => u.App.Equals(item.App) && u.FamilySeq.Equals(((propCaseMst.Find(mst => mst.ApplNo.Equals(item.App))).FamilySeq)));
                            introwIndex = gvwAttendance.Rows.Add(item.App,string.Empty, item.FundHie,LookupDataAccess.Getdate(item.Enrl_Date),LookupDataAccess.Getdate(item.Withdraw_Date), intPresent, intAbsent, intLegal,caseSnp.NameixFi,caseSnp.NameixLast,caseSnp.NameixMi);
                        }
                   
                }
            }
            if (gvwAttendance.Rows.Count > 0)
            {
                if (propOrderBy == "1")
                {
                    foreach (DataGridViewRow item in gvwAttendance.Rows)
                    {
                        item.Cells["gvtName"].Value = LookupDataAccess.GetMemberName(item.Cells["gvtFirstName"].Value.ToString(), item.Cells["gvtMiddleName"].Value.ToString(), item.Cells["gvtLastName"].Value.ToString(), "1");
                    }
                    gvwAttendance.Sort(gvwAttendance.Columns["gvtFirstName"], ListSortDirection.Ascending);
                }
                else
                {
                    foreach (DataGridViewRow item in gvwAttendance.Rows)
                    {
                        item.Cells["gvtName"].Value = LookupDataAccess.GetMemberName(item.Cells["gvtFirstName"].Value.ToString(), item.Cells["gvtMiddleName"].Value.ToString(), item.Cells["gvtLastName"].Value.ToString(), "2");
                    }
                    gvwAttendance.Sort(gvwAttendance.Columns["gvtLastName"], ListSortDirection.Ascending);

                }
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (propMonth.Length == 1)
            //{
                propMonth = ((ListItem)cmbMonth.SelectedItem).Value.ToString();
            //}
            LoadGridData();
        }
    }
}