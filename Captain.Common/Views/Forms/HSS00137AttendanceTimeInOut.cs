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
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSS00137AttendanceTimeInOut : Form
    {
        private CaptainModel _model = null;
        public HSS00137AttendanceTimeInOut(BaseForm baseForm, PrivilegeEntity privileges, List<ChldAttnEntity> chldAttnlist, string SiteRoomAmpm, string Site, string Room, string Ampm, string strStatus, string strMonth, string strOrderBy, string strMode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            propSiteRoomAmpm = SiteRoomAmpm;
            propStatus = strStatus;
            propMonth = strMonth;
            propSite = Site;
            propRoom = Room;
            propAmpm = Ampm;
            propOrderBy = strOrderBy;
            propMode = strMode;
            if (Privileges.ChangePriv.Equals("false"))
            {
                PbEdit.Visible = false;

            }
            else
            {
                PbEdit.Visible = true;

            }
            GridFilling(strOrderBy, chldAttnlist);
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
        public string propMode { get; set; }

        #endregion

        void GridFilling(string strOrderBy, List<ChldAttnEntity> listAttnData)
        {

            gvwChildDetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwChildDetails_CellValueChanged);
            gvwChildDetails.Rows.Clear();
            List<ChldAttnEntity> chldAttnDataOrder;
            if (strOrderBy == "1")
            {

                chldAttnDataOrder = listAttnData.OrderBy(u => u.FirstName).ThenBy(u => u.MiddleName).ThenBy(u => u.LastName).ThenBy(u => Convert.ToDateTime(u.Date)).ToList();
                chldAttnDataOrder.ForEach(u => u.ChildName = LookupDataAccess.GetMemberName(u.FirstName, u.MiddleName, u.LastName, "1"));
            }
            else
            {
                chldAttnDataOrder = listAttnData.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ThenBy(u => u.MiddleName).ThenBy(u => Convert.ToDateTime(u.Date)).ToList();
                chldAttnDataOrder.ForEach(u => u.ChildName = LookupDataAccess.GetMemberName(u.FirstName, u.MiddleName, u.LastName, "2"));

            }
            foreach (ChldAttnEntity item in chldAttnDataOrder)
            {
                int intRowIndex = gvwChildDetails.Rows.Add(item.ChildName, item.Date, item.TimeStart1, item.TimeEnd1, item.TimeSum1, item.TimeStart2, item.TimeEnd2, item.TimeSum2, item.HoursSpent, item.Fund, item.Applicant);
                CommonFunctions.setTooltip(intRowIndex, item.ADD_OPERATOR, item.DATE_ADD, item.LSTC_OPERATOR, item.DATE_LSTC, gvwChildDetails, item.Applicant);
            }
           // gvwChildDetails.CellValueChanged += new DataGridViewCellEventHandler(gvwChildDetails_CellValueChanged);
        }
        public bool boolsaveclick = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool boolsucess = true;
            if (ValidateCalculation())
            {
                foreach (DataGridViewRow gvrows in gvwChildDetails.Rows)
                {

                    ChldAttnEntity chldattnentity = new ChldAttnEntity();
                    chldattnentity.AGENCY = BaseForm.BaseAgency;
                    chldattnentity.DEPT = BaseForm.BaseDept;
                    chldattnentity.PROG = BaseForm.BaseProg;
                    chldattnentity.YEAR = BaseForm.BaseYear;
                    chldattnentity.APP_NO = gvrows.Cells["gvtApplicant"].Value.ToString();
                    chldattnentity.SITE = propSite;
                    chldattnentity.ROOM = propRoom;
                    chldattnentity.AMPM = propAmpm;
                    chldattnentity.FUNDING_SOURCE = gvrows.Cells["gvtFund"].Value.ToString();
                    chldattnentity.DATE = gvrows.Cells["gvtDate"].Value.ToString();
                    chldattnentity.TimeStart1 = gvrows.Cells["gvtTime1Start"].Value== null ? string.Empty :gvrows.Cells["gvtTime1Start"].Value.ToString();
                    chldattnentity.TimeStart2 = gvrows.Cells["gvtTime2Start"].Value == null ? string.Empty : gvrows.Cells["gvtTime2Start"].Value.ToString();
                    chldattnentity.TimeEnd1 = gvrows.Cells["gvtTime1End"].Value== null ? string.Empty :gvrows.Cells["gvtTime1End"].Value.ToString();
                    chldattnentity.TimeEnd2 = gvrows.Cells["gvtTime2End"].Value== null ? string.Empty :gvrows.Cells["gvtTime2End"].Value.ToString();
                    chldattnentity.TimeSum1 = gvrows.Cells["gvtTime1Sum"].Value== null ? string.Empty :gvrows.Cells["gvtTime1Sum"].Value.ToString();
                    chldattnentity.TimeSum2 = gvrows.Cells["gvtTime2Sum"].Value == null ? string.Empty : gvrows.Cells["gvtTime2Sum"].Value.ToString();
                    chldattnentity.HOURS = gvrows.Cells["gvtTimeTotal"].Value.ToString();
                    chldattnentity.Mode = "EditTIME";
                    if (_model.ChldAttnData.InsertUpdateDelChldAttn(chldattnentity))
                    {
                        boolsaveclick = true;
                    }

                }
                PbEdit.Visible = true;
                btnSave.Visible = false;
                btnCalculated.Visible = false;
                gvwChildDetails.Enabled = false;
                btnCancel.Text = "&Close";
            }
        }
        private string SaveAttandanceDetails()
        {
            StringBuilder str = new StringBuilder();
            if (propMode != Consts.Common.Delete)
            {

                string strPresent = string.Empty;
                string strAppcent = string.Empty;
                string strLReason = string.Empty;
                string strPresentAppcent = string.Empty;
                string strB = string.Empty;
                string strA = string.Empty;
                string strL = string.Empty;
                string strP = string.Empty;
                string strS = string.Empty;
                string strMeals = string.Empty;
                bool boolsave = false;

                str.Append("<ChldAttn>");
                foreach (DataGridViewRow gvrows in gvwChildDetails.Rows)
                {
                    strMeals = gvrows.Cells["gvtBillMeals"].Value.ToString();
                    if ((strMeals != string.Empty && strMeals != "N") && (Convert.ToString(gvrows.Cells["gvtUpdatest"].Value) == "U"))
                    {
                        boolsave = true;
                        strPresent = Convert.ToBoolean(gvrows.Cells["gvCPresent"].Value) == true ? "Y" : "N";
                        strAppcent = Convert.ToBoolean(gvrows.Cells["gvCAppcent"].Value) == true ? "Y" : "N";
                        if (strPresent == "Y" && strAppcent == "N")
                            strPresentAppcent = "P";
                        if (strPresent == "N" && strAppcent == "Y")
                            strPresentAppcent = "A";
                        if (strPresent == "N" && strAppcent == "N")
                            strPresentAppcent = "";
                        strLReason = Convert.ToBoolean(gvrows.Cells["gvCLReason"].Value) == true ? "Y" : "N";
                        strB = Convert.ToBoolean(gvrows.Cells["gvcB"].Value) == true ? "Y" : "N";
                        strA = Convert.ToBoolean(gvrows.Cells["gvcN"].Value) == true ? "Y" : "N";
                        strL = Convert.ToBoolean(gvrows.Cells["gvCL"].Value) == true ? "Y" : "N";
                        strP = Convert.ToBoolean(gvrows.Cells["gvCP"].Value) == true ? "Y" : "N";
                        strS = Convert.ToBoolean(gvrows.Cells["gvCN1"].Value) == true ? "Y" : "N";
                        str.Append("<Details ATTN_APP_NO = \"" + gvrows.Cells["gvApplicant"].Value.ToString() + "\" ATTN_DATE = \"" + gvrows.Cells["gvDate"].Value.ToString() + "\" ATTN_FUNDING_SOURCE =\"" + gvrows.Cells["gvFund"].Value.ToString() + "\" ATTN_PA = \"" + strPresentAppcent + "\" ATTN_REASON = \"" + gvrows.Cells["gvtReasoncode"].Value.ToString() + "\" ATTN_B = \"" + strB + "\" ATTN_A = \"" + strA + "\" ATTN_L = \"" + strL + "\" ATTN_P = \"" + strP + "\" ATTN_S = \"" + strS + "\" ATTN_PARENT_RATE = \"" + (gvrows.Cells["gvParentFees"].Value.ToString() == string.Empty ? "0.00" : gvrows.Cells["gvParentFees"].Value.ToString()) + "\"  ATTN_FUNDING_RATE = \"" + (gvrows.Cells["gvFundFee"].Value.ToString() == string.Empty ? "0.00" : gvrows.Cells["gvFundFee"].Value.ToString()) + " \" ATTN_HOURS = \"" + (gvrows.Cells["gvHoursSpent"].Value.ToString() == string.Empty ? "0.00" : gvrows.Cells["gvHoursSpent"].Value.ToString()) + "\" ATTN_CATEGORY = \"" + gvrows.Cells["gvtcategory"].Value.ToString() + "\" ATTN_CHARGE_CODE = \"" + gvrows.Cells["gvtchargecode"].Value.ToString() + "\" ATTN_LEGAL = \"" + strLReason + "\" ATTN_MEAL = \"" + strMeals + "\" ATTN_PRES_DESC = \"" + gvrows.Cells["gvtOther"].Value.ToString() + "\" />");
                    }
                }
                str.Append("</ChldAttn>");
                if (!boolsave)
                    str = null;
            }
            else
                str = null;

            return Convert.ToString(str);
        }


        public void InsertUpdateChldAttendance()
        {
            if (CheckReasons())
            {
                string strdeletemsg = string.Empty;
                ChldAttnEntity chldattnentity = new ChldAttnEntity();
                chldattnentity.AGENCY = BaseForm.BaseAgency;
                chldattnentity.DEPT = BaseForm.BaseDept;
                chldattnentity.PROG = BaseForm.BaseProg;
                chldattnentity.YEAR = BaseForm.BaseYear;
                chldattnentity.SITE = propSite;
                chldattnentity.ROOM = propRoom;
                chldattnentity.AMPM = propAmpm;
                chldattnentity.AttnXml = SaveAttandanceDetails();
                chldattnentity.ADD_OPERATOR = BaseForm.UserID;
                chldattnentity.LSTC_OPERATOR = BaseForm.UserID;

                chldattnentity.Mode = Privileges.DelPriv.ToUpper().Trim();

                if (chldattnentity.AttnXml != string.Empty)
                {
                    if (_model.ChldAttnData.InsertUpdateDelChldAttnXml(chldattnentity, out strdeletemsg))
                    {
                        if (strdeletemsg == "NOTDELETE")
                        {
                           AlertBox.Show("User have not deleted privileges, so some records are not deleted",MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        if (strdeletemsg == "NOTDELETE")
                        {
                            //strMode = Consts.Common.View;
                            //gvwChildDetails.Rows.Clear();
                            //strMode = Consts.Common.View;
                            //ShowButtons();
                            //btnProcess_Click(btnProcess, new EventArgs());
                            //CommonFunctions.MessageBoxDisplay("User does n't have delete privileges. some records not deleted");
                        }

                    }
                }
                else
                {
                    // CommonFunctions.MessageBoxDisplay("Does not changed any attendance record.");
                    //strMode = Consts.Common.View;
                    //ShowButtons();
                    //btnProcess_Click(btnProcess, new EventArgs());
                    //pnlHeader.Enabled = true;
                }
            }
            else
            {
                AlertBox.Show("Client is marked as absent, but no absent reason has been selected",MessageBoxIcon.Warning);
            }

        }

        private bool CheckReasons()
        {
            bool boolReasons = true;
            foreach (DataGridViewRow item in gvwChildDetails.Rows)
            {
                if (Convert.ToBoolean(item.Cells["gvCAppcent"].Value) == true)
                {
                    if (item.Cells["gvtReason"].Value.ToString() == string.Empty || item.Cells["gvtReasoncode"].Value.ToString() == string.Empty)
                    {
                        boolReasons = false;
                        break;
                    }
                }
            }
            return boolReasons;
        }


        private void gvwChildDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           // gvwChildDetails.CellValueChanged -= new DataGridViewCellEventHandler(gvwChildDetails_CellValueChanged);

            //if (e.ColumnIndex == gvtTime1Start.Index || e.ColumnIndex == gvtTime1End.Index || e.ColumnIndex == gvtTime2Start.Index || e.ColumnIndex == gvtTime2End.Index)
            //{
            //    int introwindex = gvwChildDetails.CurrentCell.RowIndex;
            //    int intcolumnindex = gvwChildDetails.CurrentCell.ColumnIndex;
            //    string strCurrectValue = Convert.ToString(gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Value);
            //    //strCurrectValue = strCurrectValue.Replace("_", "").Trim();
            //    //strCurrectValue = strCurrectValue.Replace(" ", "").Trim();

            //    if (!string.IsNullOrEmpty(strCurrectValue)) // && strCurrectValue.Trim() != "  :  ")
            //    {
            //        try
            //        {

            //            if (IsTimeString(strCurrectValue))
            //            {
            //                //gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Style.BackColor = System.Drawing.Color.White;
            //                CalculationTime(introwindex);

            //            }
            //            else
            //            {
            //                // gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Style.BackColor = System.Drawing.Color.Red;
            //                gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
            //                // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterHHMMTimeFormat);

            //            }

            //        }
            //        catch (Exception)
            //        {
            //            //gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Style.BackColor = System.Drawing.Color.Red;
            //            gvwChildDetails.Rows[introwindex].Cells[intcolumnindex].Value = string.Empty;
            //            // CommonFunctions.MessageBoxDisplay(Consts.Messages.PleaseEnterHHMMTimeFormat);

            //        }
            //    }

            //}
           // gvwChildDetails.CellValueChanged += new DataGridViewCellEventHandler(gvwChildDetails_CellValueChanged);

        }

        public bool IsTimeString(string ts)
        {
            //if (ts.Length == 5 && ts.Contains(':'))
            //{
            //    int h;
            //    int m;

            //    return int.TryParse(ts.Substring(0, 2), out h) &&
            //           int.TryParse(ts.Substring(3, 2), out m) &&
            //           h >= 0 && h < 24 &&
            //           m >= 0 && m < 60;
            //}
            if (ts.Contains(':'))
            {
                string[] str = ts.Split(':');
                int h;
                int m;
                return int.TryParse(str[0], out h) &&
                       int.TryParse(str[1], out m) &&
                       h >= 0 && h < 24 &&
                       m >= 0 && m < 60;
            }
            else
                return false;
        }

        public string TimeCalculation(string strstart, string strEnd)
        {
            TimeSpan ts1 = TimeSpan.Parse(strstart); //"1:35"
            TimeSpan ts2 = TimeSpan.Parse(strEnd); //"3:30"

            if (ts1 > ts2)
            {
                ts2 = ts2.Add(TimeSpan.Parse("12:00"));
            }
            return (ts2 - ts1).ToString();
        }

        private void CalculationTime(int rowcalindex)
        {
            try
            {

                string strStart1 = string.Empty;
                string strStart2 = string.Empty;
                string strEnd1 = string.Empty;
                string strEnd2 = string.Empty;
                string strTime1Total = string.Empty;
                string strTime2Total = string.Empty;

                strStart1 = Convert.ToString(gvwChildDetails.Rows[rowcalindex].Cells["gvtTime1Start"].Value);
                //strStart1 = strStart1.Replace("_", "").Trim();
                //strStart1 = strStart1.Replace(" ", "").Trim();

                strEnd1 = Convert.ToString(gvwChildDetails.Rows[rowcalindex].Cells["gvtTime1End"].Value);
                //strEnd1 = strEnd1.Replace("_", "").Trim();
                //strEnd1 = strEnd1.Replace(" ", "").Trim();

                strStart2 = Convert.ToString(gvwChildDetails.Rows[rowcalindex].Cells["gvtTime2Start"].Value);
                //strStart2 = strStart2.Replace("_", "").Trim();
                //strStart2 = strStart2.Replace(" ", "").Trim();

                strEnd2 = Convert.ToString(gvwChildDetails.Rows[rowcalindex].Cells["gvtTime2End"].Value);
                //strEnd2 = strEnd2.Replace("_", "").Trim();
                //strEnd2 = strEnd2.Replace(" ", "").Trim();

                if (((!string.IsNullOrEmpty(strStart1))) && ((!string.IsNullOrEmpty(strEnd1))))
                {
                    strTime1Total = TimeCalculation(strStart1, strEnd1);
                    gvwChildDetails.Rows[rowcalindex].Cells["gvtTime1Sum"].Value = strTime1Total.Substring(0, 5);
                }
                if (((!string.IsNullOrEmpty(strStart2))) && ((!string.IsNullOrEmpty(strEnd2))))
                {
                    strTime2Total = TimeCalculation(strStart2, strEnd2);
                    gvwChildDetails.Rows[rowcalindex].Cells["gvtTime2Sum"].Value = strTime2Total.Substring(0, 5);

                }
                if (strTime1Total != string.Empty && strTime2Total != string.Empty)
                {
                    TimeSpan t1 = TimeSpan.Parse(strTime1Total);
                    TimeSpan t2 = TimeSpan.Parse(strTime2Total);
                    TimeSpan t3 = t1.Add(t2);
                    string strTotalTime = t3.ToString().Replace(':', '.').Substring(0, 5);
                    gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTotalTime.ToString();
                }
                else
                {
                    if (strTime1Total != string.Empty)
                    {
                        gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTime1Total.Replace(':', '.').Substring(0, 5); ;
                    }
                    if (strTime2Total != string.Empty)
                    {
                        gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTime2Total.Replace(':', '.').Substring(0, 5); ;
                    }

                }


                //if (((!string.IsNullOrEmpty(strStart1)) && strStart1.Trim() != "  :  ") && ((!string.IsNullOrEmpty(strEnd1)) && strEnd1.Trim() != "  :  "))
                //{
                //    strTime1Total = TimeCalculation(strStart1, strEnd1);
                //    gvwChildDetails.Rows[rowcalindex].Cells["gvtTime1Sum"].Value = strTime1Total.Substring(0, 5);
                //}
                //if (((!string.IsNullOrEmpty(strStart2)) && strStart2.Trim() != "  :  ") && ((!string.IsNullOrEmpty(strEnd2)) && strEnd2.Trim() != "  :  "))
                //{
                //    strTime2Total = TimeCalculation(strStart2, strEnd2);
                //    gvwChildDetails.Rows[rowcalindex].Cells["gvtTime2Sum"].Value = strTime2Total.Substring(0, 5);

                //}
                //if (strTime1Total != string.Empty && strTime2Total != string.Empty)
                //{
                //    TimeSpan t1 = TimeSpan.Parse(strTime1Total);
                //    TimeSpan t2 = TimeSpan.Parse(strTime2Total);
                //    TimeSpan t3 = t1.Add(t2);
                //    string strTotalTime = t3.ToString().Replace(':', '.').Substring(0, 5);
                //    gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTotalTime.ToString();
                //}
                //else
                //{
                //    if (strTime1Total != string.Empty)
                //    {
                //        gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTime1Total.Replace(':', '.').Substring(0, 5); ;
                //    }
                //    if (strTime2Total != string.Empty)
                //    {
                //        gvwChildDetails.Rows[rowcalindex].Cells["gvtTimeTotal"].Value = strTime2Total.Replace(':', '.').Substring(0, 5); ;
                //    }

                //}
            }
            catch (Exception ex)
            {


            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text.ToUpper() == "CLOSE")
            {
                this.Close();
            }
            else
            {
                PbEdit.Visible = true;
                btnSave.Visible = false;
                btnCalculated.Visible = false;
                gvwChildDetails.Enabled = false;
                btnCancel.Text = "&Close";
            }
        }

        private void PbEdit_Click(object sender, EventArgs e)
        {
            btnCalculated.Visible = true;
            btnSave.Visible = true;
            PbEdit.Visible = false;
            gvwChildDetails.Enabled = true;
            btnCancel.Text = "&Cancel";
        }

        private void btnCalculated_Click(object sender, EventArgs e)
        {
            ValidateCalculation();
        }

        bool ValidateCalculation()
        {
            

            string strTimeStart1 = string.Empty;
            string strTimeStart2 = string.Empty;
            string strTimeEnd1 = string.Empty;
            string strTimeEnd2 = string.Empty;
            string strTimeSum1 = string.Empty;
            string strTimeSum2 = string.Empty;
            string strHours = string.Empty;

            bool boolTimeStart1 = true;
            bool boolTimeStart2 = true;
            bool boolTimeEnd1 = true;
            bool boolTimeEnd2 = true;
            bool boolMainValidattion = true;
            foreach (DataGridViewRow gvrows in gvwChildDetails.Rows)
            {
                boolTimeStart1 = false;
                boolTimeStart2 = false;
                boolTimeEnd1 = false;
                boolTimeEnd2 = false;

                gvrows.Cells["gvtTime1Sum"].Value = string.Empty;
                gvrows.Cells["gvtTime2Sum"].Value = string.Empty;
                gvrows.Cells["gvtTimeTotal"].Value = string.Empty;

                var varTimeStart1 = gvrows.Cells["gvtTime1Start"].Value;
                strTimeStart1 = varTimeStart1 == null ? string.Empty : varTimeStart1.ToString();
                if (!string.IsNullOrEmpty(strTimeStart1.Trim())) // && strCurrectValue.Trim() != "  :  ")
                {
                    if (IsTimeString(strTimeStart1))
                    {
                        boolTimeStart1 = true;
                        gvrows.Cells["gvtTime1Start"].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        boolMainValidattion = false;
                        boolTimeStart1 = false;
                        gvrows.Cells["gvtTime1Start"].Style.BackColor = System.Drawing.Color.Red;
                    }
                }
                var varTimeStart2 = gvrows.Cells["gvtTime2Start"].Value;
                strTimeStart2 = varTimeStart2 == null ? string.Empty : varTimeStart2.ToString();
                if (!string.IsNullOrEmpty(strTimeStart2.Trim())) // && strCurrectValue.Trim() != "  :  ")
                {
                    if (IsTimeString(strTimeStart2))
                    {
                        boolTimeStart2 = true;
                        gvrows.Cells["gvtTime2Start"].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        boolMainValidattion = false;
                        boolTimeStart2 = false;
                        gvrows.Cells["gvtTime2Start"].Style.BackColor = System.Drawing.Color.Red;
                    }
                }

                var varTimeEnd1 = gvrows.Cells["gvtTime1End"].Value;
                strTimeEnd1 = varTimeEnd1 == null ? string.Empty : varTimeEnd1.ToString();
                if (!string.IsNullOrEmpty(strTimeEnd1.Trim())) // && strCurrectValue.Trim() != "  :  ")
                {
                    if (IsTimeString(strTimeEnd1))
                    {
                        boolTimeEnd1 = true;
                        gvrows.Cells["gvtTime1End"].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        boolMainValidattion = false;
                        boolTimeEnd1 = false;
                        gvrows.Cells["gvtTime1End"].Style.BackColor = System.Drawing.Color.Red;
                    }
                }
                var varTimeEnd2 = gvrows.Cells["gvtTime2End"].Value;
                strTimeEnd2 = varTimeEnd2 == null ? string.Empty : varTimeEnd2.ToString();
                if (!string.IsNullOrEmpty(strTimeEnd2.Trim()))
                {
                    if (IsTimeString(strTimeEnd2))
                    {
                        boolTimeEnd2 = true;
                        gvrows.Cells["gvtTime2End"].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        boolMainValidattion = false;
                        boolTimeEnd2 = false;
                        gvrows.Cells["gvtTime2End"].Style.BackColor = System.Drawing.Color.Red;
                    }
                }


                string strTime1Total = string.Empty;
                string strTime2Total = string.Empty;
                if ((boolTimeStart1) && (boolTimeEnd1))
                {
                    strTime1Total = TimeCalculation(strTimeStart1, strTimeEnd1);
                    gvrows.Cells["gvtTime1Sum"].Value = strTime1Total.Substring(0, 5);
                }
                if ((boolTimeStart2) && (boolTimeEnd2))
                {
                    strTime2Total = TimeCalculation(strTimeStart2, strTimeEnd2);
                    gvrows.Cells["gvtTime2Sum"].Value = strTime2Total.Substring(0, 5);

                }
                if (strTime1Total != string.Empty && strTime2Total != string.Empty)
                {
                    TimeSpan t1 = TimeSpan.Parse(strTime1Total);
                    TimeSpan t2 = TimeSpan.Parse(strTime2Total);
                    TimeSpan t3 = t1.Add(t2);
                    string strTotalTime = t3.ToString().Replace(':', '.').Substring(0, 5);
                    gvrows.Cells["gvtTimeTotal"].Value = strTotalTime.ToString();
                }
                else
                {
                    if (strTime1Total != string.Empty)
                    {
                        gvrows.Cells["gvtTimeTotal"].Value = strTime1Total.Replace(':', '.').Substring(0, 5); ;
                    }
                    if (strTime2Total != string.Empty)
                    {
                        gvrows.Cells["gvtTimeTotal"].Value = strTime2Total.Replace(':', '.').Substring(0, 5); ;
                    }

                }
            }
            gvwChildDetails.Update();
            return boolMainValidattion;
        }
    }
}