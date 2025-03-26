#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class APPT0001REASONSForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        public APPT0001REASONSForm(BaseForm baseForm, PrivilegeEntity privilieges, string sunday, string Monday, string Tuesday, string wedday, string Thrusday, string friday, string saturday, string fromdt, string Todt, string Site,string Currendate)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            strSunday = sunday;
            strMonday = Monday;
            strTuesDay = Tuesday;
            strWed = wedday;
            strThrus = Thrusday;
            strFriday = friday;
            strSatday = saturday;
            strFromdt = fromdt;
            strTodt = Todt;
            strSite = Site;
            strCurrentdate = Currendate;
            if(strCurrentdate!=string.Empty)
            {
                dtDate.Text = strCurrentdate;
                dtDate.Checked = true;
                dtDate.Enabled = false;
            }
            FillGridData();
        }

        #region properties
        public BaseForm BaseForm { get; set; }
        public string strSunday { get; set; }
        public string strMonday { get; set; }
        public string strTuesDay { get; set; }
        public string strWed { get; set; }
        public string strThrus { get; set; }
        public string strFriday { get; set; }
        public string strSatday { get; set; }
        public string strFromdt { get; set; }
        public string strTodt { get; set; }
        public string strCurrentdate { get; set; }
        public string propMode { get; set; }
        public string strSite { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        #endregion
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (ValidateForm())
            {
                APPTREASNEntity ApptReasonEntity = new APPTREASNEntity();

                ApptReasonEntity.APTRSN_AGY = BaseForm.BaseAgency;
                ApptReasonEntity.APTRSN_DEPT = BaseForm.BaseDept;
                ApptReasonEntity.APTRSN_PROG = BaseForm.BaseProg;
                ApptReasonEntity.APTRSN_YEAR = string.Empty;
                ApptReasonEntity.APTRSN_SITE = strSite;
                ApptReasonEntity.APTRSN_DATE = dtDate.Text.Trim();
                ApptReasonEntity.APTRSN_REASON = txtReasons.Text.Trim();



                ApptReasonEntity.APTRSN_LSTC_OPERATOR = BaseForm.UserID;
                ApptReasonEntity.APTRSN_ADD_OPERATOR = BaseForm.UserID;
                ApptReasonEntity.Mode = propMode;

                if (_model.TmsApcndata.InsertUpdateDelAPPTREASN(ApptReasonEntity))
                {
                    FillGridData();
                }
                AlertBox.Show("Reason Saved Successfully");
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (dtDate.Checked == false)
            {
                _errorProvider.SetError(dtDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtDate, null);
                if (propMode == "Add")
                {
                    if (strCurrentdate == string.Empty)
                    {
                                            
                        if (!(Convert.ToDateTime(strFromdt).Date <= dtDate.Value && Convert.ToDateTime(strTodt).Date >= dtDate.Value))
                        {
                            _errorProvider.SetError(dtDate, "Please select 'From Date'" + Convert.ToDateTime(strFromdt).Date.ToShortDateString() + " and 'To date'" + Convert.ToDateTime(strTodt).Date.ToShortDateString() + " Between Date ");
                            isValid = false;
                        }
                    }
                    else
                    {
                       if(Convert.ToDateTime(strCurrentdate).Date!=dtDate.Value )
                        {
                            _errorProvider.SetError(dtDate, "Please select this Date " + Convert.ToDateTime(strCurrentdate).Date.ToShortDateString() );
                            isValid = false;
                        }

                    }
                }
            }
            if (propMode == "Add")
            {
                foreach (DataGridViewRow item in gvwReasons.Rows)
                {
                    string strdata = item.Cells["gvtDate"].Value == null ? string.Empty : item.Cells["gvtDate"].Value.ToString();
                    if (strdata != string.Empty)
                    {
                        if (dtDate.Value == Convert.ToDateTime(strdata).Date)
                        {
                            AlertBox.Show("Date already existed", MessageBoxIcon.Warning);
                            isValid = false;
                        }
                    }
                }
            }

            if (strCurrentdate == string.Empty)
            {
                string strday = dtDate.Value.DayOfWeek.ToString();
                if (isValid)
                {
                    switch (strday)
                    {
                        case "Monday":
                            if (strMonday == "Y")
                            {
                                isValid = false;
                                AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Tuesday":
                            if (strTuesDay == "Y")
                            {
                                isValid = false;
                                AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Wednesday":
                            if (strWed == "Y")
                            {
                                isValid = false;
                               AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Thursday":
                            if (strThrus == "Y")
                            {
                                isValid = false;
                               AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Friday":
                            if (strFriday == "Y")
                            {
                                isValid = false;
                               AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Saturday":
                            if (strSatday == "Y")
                            {
                                isValid = false;
                               AlertBox.Show("Day is open, so reasons are not allowed", MessageBoxIcon.Warning);
                            }
                            break;
                        case "Sunday":
                            if (strSunday == "Y")
                            {
                                isValid = false;
                               AlertBox.Show("Closed day", MessageBoxIcon.Warning);
                            }
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(txtReasons.Text))
            {
                _errorProvider.SetError(txtReasons, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReason.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtReasons, null);
            }


            return (isValid);
        }


        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvwReasons_Click(object sender, EventArgs e)
        {

        }
        int strIndex;
        private void gvwReasons_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwReasons.Rows.Count > 0)
            {

                propMode = "Add";
                txtReasons.Text = string.Empty;
                _errorProvider.SetError(txtReasons, null);
                _errorProvider.SetError(dtDate, null);
                dtDate.Checked = false;
                if (gvwReasons.SelectedRows.Count > 0)
                {
                    if (gvwReasons.SelectedRows[0].Tag is APPTREASNEntity)
                    {

                        APPTREASNEntity row = gvwReasons.SelectedRows[0].Tag as APPTREASNEntity;
                        strIndex = gvwReasons.SelectedRows[0].Index;
                        if (row != null)
                        {
                            dtDate.Checked = true;
                            dtDate.Text = row.APTRSN_DATE;

                            txtReasons.Text = row.APTRSN_REASON;
                            propMode = "Edit";

                            //strIncomeIndex = dataGridCaseIncome.SelectedRows[0].Index;
                            if (Privileges.ChangePriv.Equals("false"))
                            {
                                EnableAllcontrols(false);
                            }
                            else
                            {
                                EnableAllcontrols(true);
                                dtDate.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        propMode = "Add";
                        dtDate.Checked = false;
                        txtReasons.Text = string.Empty;
                        if (Privileges.AddPriv.Equals("false"))
                        {
                            EnableAllcontrols(false);
                        }
                        else
                        {
                            EnableAllcontrols(true);
                        }
                    }
                }
                else
                {
                    propMode = "Add";
                    dtDate.Checked = false;
                    txtReasons.Text = string.Empty;
                    if (Privileges.AddPriv.Equals("false"))
                    {
                        EnableAllcontrols(false);
                    }
                    else
                    {
                        EnableAllcontrols(true);
                    }
                }

                if(strCurrentdate!=string.Empty)
                {
                    dtDate.Checked = true;
                    dtDate.Enabled = false;

                }
            }
            else
            {
                propMode = "Add";
                dtDate.Checked = false;
                txtReasons.Text = string.Empty;
                if (Privileges.AddPriv.Equals("false"))
                {
                    EnableAllcontrols(false);
                }
                else
                {
                    EnableAllcontrols(true);
                }
                if (strCurrentdate != string.Empty)
                {
                    dtDate.Checked = true;
                    dtDate.Enabled = false;
                }
            }

        }
        private void EnableAllcontrols(bool booltrue)
        {
            txtReasons.Enabled = booltrue;
            dtDate.Enabled = booltrue;
            btnSave.Visible = booltrue;
            btnCancel.Text = "&Close";
        }

        private void gvwReasons_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex ==gviDel.Index && e.RowIndex != -1)
            {
                if (gvwReasons.SelectedRows.Count > 0)
                {

                    if (gvwReasons.SelectedRows[0].Tag is APPTREASNEntity)
                    {
                        APPTREASNEntity row = gvwReasons.SelectedRows[0].Tag as APPTREASNEntity;
                        if (row != null)
                        {
                            MessageBox.Show(Consts.Messages.AreYouSureYouWantToDelete.GetMessage(), Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose:OnDeleteMessageBoxClicked);

                        }
                    }
                }
            }
        }

        private void OnDeleteMessageBoxClicked(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {

                APPTREASNEntity ApptReasonEntity = new APPTREASNEntity();

                ApptReasonEntity.APTRSN_AGY = BaseForm.BaseAgency;
                ApptReasonEntity.APTRSN_DEPT = BaseForm.BaseDept;
                ApptReasonEntity.APTRSN_PROG = BaseForm.BaseProg;
                ApptReasonEntity.APTRSN_YEAR = string.Empty;//BaseForm.BaseYear;
                ApptReasonEntity.APTRSN_SITE = strSite;
                ApptReasonEntity.APTRSN_DATE = dtDate.Text.Trim();
                ApptReasonEntity.APTRSN_REASON = txtReasons.Text.Trim();



                ApptReasonEntity.APTRSN_LSTC_OPERATOR = BaseForm.UserID;
                ApptReasonEntity.APTRSN_ADD_OPERATOR = BaseForm.UserID;
                ApptReasonEntity.Mode = "Delete";

                if (_model.TmsApcndata.InsertUpdateDelAPPTREASN(ApptReasonEntity))
                {


                    if (strIndex != 0)
                        strIndex = strIndex - 1;
                    FillGridData();
                    if (gvwReasons.Rows.Count != 0)
                    {
                        gvwReasons.Rows[strIndex].Selected = true;
                        gvwReasons.CurrentCell = gvwReasons.Rows[strIndex].Cells[1];

                    }
                    //gvwReasons_SelectionChanged(sender, e);

                }
                AlertBox.Show("Reason Deleted Successfully");
            }
        }

        private void FillGridData()
        {
            List<APPTREASNEntity> apptreasonlist = _model.TmsApcndata.GETAPPTREASONS(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty, strSite, strCurrentdate);

            gvwReasons.Rows.Clear();

            foreach (APPTREASNEntity apptlist in apptreasonlist)
            {
                string strAltDate = LookupDataAccess.Getdate(apptlist.APTRSN_DATE);
                int rowIndex = gvwReasons.Rows.Add(strAltDate, apptlist.APTRSN_REASON);

                gvwReasons.Rows[rowIndex].Tag = apptlist;
                //gvwReasons.ItemsPerPage = 100;
                CommonFunctions.setTooltip(rowIndex, apptlist.APTRSN_ADD_OPERATOR, apptlist.APTRSN_DATE_ADD, apptlist.APTRSN_LSTC_OPERATOR, apptlist.APTRSN_DATE_LSTC, gvwReasons);
            }
            if (gvwReasons.Rows.Count > 0)
                gvwReasons.Rows[0].Selected = true;
            gvwReasons_SelectionChanged(gvwReasons, new EventArgs());
        }

    }
}