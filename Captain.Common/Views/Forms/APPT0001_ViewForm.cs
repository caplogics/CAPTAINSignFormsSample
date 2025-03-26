#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class APPT0001_ViewForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        private string Mode = null;
        public string strKey = null;
        public string strLAgency = null;
        public string strLDept = null;
        public string strLProgram = null;
        public string strLYear;
        public string strAutoRefresh = null;
        private string strGridcount = "0";

        public APPT0001_ViewForm(BaseForm baseForm, string mode, List<APPTSCHEDULEEntity> SchAppList, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            Mode = mode;
            BaseForm = baseForm;
            //lblHeader.Text = Convert.ToString(privilegeEntity.PrivilegeName);
            this.Text = privilegeEntity.PrivilegeName + Consts.Common.Dash + " " + Consts.Common.View;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            privilege = privilegeEntity;

            APTSCHAppList = SchAppList;

            fillDatesGrid();

            //fillGvSchGrid();
        }

        #region Properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity privilege { get; set; }

        public string MainMenuAgency { get; set; }

        public string MainMenuDept { get; set; }

        public string MainMenuProgram { get; set; }

        public string MainMenuYear { get; set; }

        public string MainMenuCode { get; set; }

        public string MainFDate { get; set; }

        public string MainTDate { get; set; }

        public string MainType { get; set; }
        public List<CaseSiteEntity> CaseSiteEntityList { get; set; }

        public List<TmsApptEntity> tmsApptlist { get; set; }

        public List<APPTEMPLATESEntity> APPTEMPList { get; set; }

        public List<APPTSCHEDULEEntity> APTSCHAppList { get; set; }

        #endregion 

        private void fillDatesGrid()
        {
            if (APTSCHAppList.Count > 0)
            {
                //List<APPTSCHEDULEEntity> Dates=APTSCHAppList.FindAll()
                var Dates = APTSCHAppList.Select(u => new { u.Date, u.Time }).Distinct();

                foreach (var item in Dates)
                {
                    string Tmp_Time = null, Disp_Time = null;
                    Tmp_Time = item.Time;
                    Disp_Time = Format_time(Tmp_Time);
                    int rowIndex = gvDates.Rows.Add( LookupDataAccess.Getdate(item.Date.Trim()),Convert.ToDateTime(item.Date).ToString("ddd"), Disp_Time,item.Time);
                }
            }
        }

        private void fillGvSchGrid()
        {
            gvScheduled.Rows.Clear();

            if(APTSCHAppList.Count>0)
            {
                List<APPTSCHEDULEEntity> SchdList = new List<APPTSCHEDULEEntity>();
                if (chkbShowDates.Checked) SchdList = APTSCHAppList;
                else
                    SchdList = APTSCHAppList.FindAll(u => LookupDataAccess.Getdate(u.Date.Trim())==gvDates.CurrentRow.Cells["Dates"].Value.ToString() && u.Time.Equals(gvDates.CurrentRow.Cells["StrTime"].Value.ToString()));
                if (SchdList.Count > 0)
                {
                    int rowIndex = 0; string TemplateDesc = string.Empty;
                    foreach (APPTSCHEDULEEntity Entity in SchdList)
                    {
                        string FName = string.Empty; string Disp_Time = null, Tmp_Time = null; TemplateDesc = string.Empty;

                        if (!string.IsNullOrEmpty(Entity.FirstName.Trim()))
                            FName = Entity.FirstName.Trim() + " ";


                        Tmp_Time = Entity.Time;
                        Disp_Time = Format_time(Tmp_Time);

                        rowIndex = gvScheduled.Rows.Add(LookupDataAccess.Getdate(Entity.Date.Trim()), Disp_Time,   Entity.SlotNumber.Trim(), Entity.SchdType == "1" ? "Booked" : "Reserved", FName + Entity.LastName.Trim());
                    }
                }
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void APPT0001_ViewForm_Load(object sender, EventArgs e)
        {

        }

        private void gvDates_SelectionChanged(object sender, EventArgs e)
        {
            if(!chkbShowDates.Checked)
                fillGvSchGrid();
        }

        private void chkbShowDates_CheckedChanged(object sender, EventArgs e)
        {
            fillGvSchGrid();
        }
    }
}