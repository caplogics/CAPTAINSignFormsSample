#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using Wisej.Web;
using System.Globalization;


#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class UserListControl : BaseUserControl
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _servicePlanHierarchy = null;     //Kranthi 02/02/2023 :: Keep in mind.docx :: change service plan grid to new format
        private SerHierarchyControl _servicePlanHierarchy = null;

        private ClientInq_GridControl _serviceHierarchy = null;
        private GridControl _intakeHierarchy = null;
        private PrivilegesControl _screenPrivileges = null;
        private PrivilegesControl _reportPrivileges = null;
        private CaptainModel _model = null;
        #endregion

        public UserListControl(BaseForm baseForm, PrivilegeEntity privileges)
            : base(baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privileges;
            _model = new CaptainModel();

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N")
            {
                chkSearchPIP.Visible = true;
            }
            if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
            {

                tabPageServicePlanHie.Show();
            }
            else
            {
                tabPageServicePlanHie.Hide();
            }
            if (BaseForm.UserProfile.Security.Trim() == "P" || BaseForm.UserProfile.Security.Trim() == "B")
            {
                btnSetPassword.Enabled = true;
                btnSetPassword.Visible = true;
                btnPasswordswitch.Enabled = true;
                btnPasswordswitch.Visible = true;
            }
            DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dv.Sort = "PWR_INACTIVE_FLAG";
                dt = dv.ToTable();
            }

            int rowIndex = 0;
            int intActivecount = 0;
            int intinactivecount = 0;
            int int30dayscount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string status = "Active";
                if (dr["PWR_INACTIVE_FLAG"].ToString().Equals("Y"))
                {
                    status = "Inactive";
                    intinactivecount = intinactivecount + 1;
                }
                else
                {
                    if (BaseForm.UserID == "CAPLOGICS")
                    {
                        intActivecount = intActivecount + 1;
                    }
                    else
                    {
                        if (BaseForm.UserID == "JAKE")
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                            {
                                intActivecount = intActivecount + 1;
                            }
                        }
                        else
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                            {
                                intActivecount = intActivecount + 1;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(dr["PWR_LAST_SUCCESSFUL_DATE"].ToString()))
                {
                    if (DateTime.Now.Date.AddDays(-30).Date < Convert.ToDateTime(dr["PWR_LAST_SUCCESSFUL_DATE"]).Date)
                    {
                        int30dayscount = int30dayscount + 1;
                    }
                }
                string strDefaultHie = string.Empty;
                if (!(string.IsNullOrEmpty(dr["PWR_DEF_AGENCY"].ToString())))
                {
                    strDefaultHie = dr["PWR_DEF_AGENCY"] + "-" + dr["PWR_DEF_DEPT"] + "-" + dr["PWR_DEF_PROG"];
                }
                if (BaseForm.UserID == "CAPLOGICS")
                {
                    rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                    gvwCustomer.Rows[rowIndex].Tag = dr;
                    setTooltip(rowIndex, dr);

                }
                else
                {
                    if (BaseForm.UserID == "JAKE")
                    {
                        if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                        {
                            rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                            gvwCustomer.Rows[rowIndex].Tag = dr;
                            setTooltip(rowIndex, dr);
                        }
                    }
                    else
                    {
                        if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                        {
                            rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                            gvwCustomer.Rows[rowIndex].Tag = dr;
                            setTooltip(rowIndex, dr);
                        }
                    }
                }


                if (status.Equals("Inactive"))
                {
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }

            txtActiveCount.Text = intActivecount.ToString();
            txtInactivecount.Text = intinactivecount.ToString();
            txt30daysCount.Text = int30dayscount.ToString();

            setControlEnabled(false);
            fillDropdowns();
            TabControls();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Forcepasswordswitch();
            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
                gvwCustomer.Rows[0].Selected = true;
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }

            PopulateToolbar(oToolbarMnustrip);
        }

        public int strQuesIndex = 0;
        public int strGroupIndex = 0;
        public string SortColumn = string.Empty;
        public string SortOrder = string.Empty;
        public string strQuesID = string.Empty;
        public void RefreshGrid(string selectedUserID)
        {
            bool chkIsfirstRow = false;
            gvwCustomer.Rows.Clear();
            string empNo = txtUserID.Text.ToString();
            string firstName = txtFirstName.Text.ToString();
            string lastName = txtLastName.Text.ToString();
            string status = ((ListItem)cmbStatus.SelectedItem).Value.ToString();
            DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(empNo, firstName, lastName, status, BaseForm.UserID, BaseForm.BaseAdminAgency);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dv.Sort = "PWR_INACTIVE_FLAG";
                dt = dv.ToTable();
            }

            int rowIndex = 0;
            int intActivecount = 0;
            int intinactivecount = 0;
            int int30dayscount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                status = "Active";
                if (dr["PWR_INACTIVE_FLAG"].ToString().Equals("Y"))
                {
                    status = "Inactive";
                    intinactivecount = intinactivecount + 1;
                }
                else
                {
                    if (BaseForm.UserID == "CAPLOGICS")
                    {
                        intActivecount = intActivecount + 1;
                    }
                    else
                    {
                        if (BaseForm.UserID == "JAKE")
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                                intActivecount = intActivecount + 1;
                        }
                        else
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                            {
                                intActivecount = intActivecount + 1;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(dr["PWR_LAST_SUCCESSFUL_DATE"].ToString()))
                {
                    if (DateTime.Now.Date.AddDays(-30).Date < Convert.ToDateTime(dr["PWR_LAST_SUCCESSFUL_DATE"]).Date)
                    {
                        int30dayscount = int30dayscount + 1;
                    }
                }
                string strDefaultHie = string.Empty;
                if (!(string.IsNullOrEmpty(dr["PWR_DEF_AGENCY"].ToString())))
                {
                    strDefaultHie = dr["PWR_DEF_AGENCY"] + "-" + dr["PWR_DEF_DEPT"] + "-" + dr["PWR_DEF_PROG"];
                }
                string userID = dr["PWR_EMPLOYEE_NO"].ToString();
                if (BaseForm.UserID.ToUpper() == "CAPLOGICS")
                {
                    rowIndex = gvwCustomer.Rows.Add(userID, dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                    gvwCustomer.Rows[rowIndex].Tag = dr;
                    setTooltip(rowIndex, dr);
                }
                else
                {
                    if (BaseForm.UserID.ToUpper() == "JAKE")
                    {
                        if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                        {
                            rowIndex = gvwCustomer.Rows.Add(userID, dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                            gvwCustomer.Rows[rowIndex].Tag = dr;
                            setTooltip(rowIndex, dr);
                        }
                    }
                    else
                    {
                        if (userID.ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                        {
                            rowIndex = gvwCustomer.Rows.Add(userID, dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), status, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                            gvwCustomer.Rows[rowIndex].Tag = dr;
                            setTooltip(rowIndex, dr);
                        }
                    }
                }
                if (selectedUserID.Equals(userID))
                {
                    // gvwCustomer.FirstDisplayedScrollingRowIndex = rowIndex;
                    gvwCustomer.CurrentCell = gvwCustomer.Rows[rowIndex].Cells[0];
                    gvwCustomer.Rows[rowIndex].Selected = true;
                    chkIsfirstRow = true;
                    // gvwCustomer.CurrentPage = ((rowIndex + gvwCustomer.ItemsPerPage) / gvwCustomer.ItemsPerPage);
                }


                if (status.Equals("Inactive"))
                {
                    gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            txtActiveCount.Text = intActivecount.ToString();
            txtInactivecount.Text = intinactivecount.ToString();
            txt30daysCount.Text = int30dayscount.ToString();

            gvwCustomer.Update();
            gvwCustomer.ResumeLayout();
            DisabletoolBarButtons();
            if (gvwCustomer.Rows.Count > 0)
            {
                ToolBarEdit.Visible = true;

                if (chkIsfirstRow == false)
                {
                    gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
                    gvwCustomer.Rows[0].Selected = true;
                }
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }
            else
                ClearControls();



            if (!string.IsNullOrEmpty(SortOrder))
            {
                gvwCustomer.SortedColumn.Name = SortColumn;
                if (SortOrder == "Ascending")
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Ascending);
                else
                    this.gvwCustomer.Sort(this.gvwCustomer.Columns[SortColumn], ListSortDirection.Descending);
            }
        }

        public void Forcepasswordswitch()
        {
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            if (propAgencyControlDetails != null)
            {
                _errorProvider.SetError(btnSetPassword, null);
                if (propAgencyControlDetails.ForceAllPwdOn != string.Empty)
                {
                    DateTime dt1 = Convert.ToDateTime(propAgencyControlDetails.ForceAllPwdOn).Date;
                    DateTime dtnew = DateTime.Now.Date;
                    int intdif = (dtnew - dt1).Days;
                    if (intdif > 60)
                    {
                        _errorProvider.SetError(btnSetPassword, "Force All Password Date is already expired. Please set to Force All Passwords.");
                    }
                }
                ToolTip tooltipadd = new ToolTip();
                tooltipadd.SetToolTip(btnSetPassword, "Force All Password done By : " + propAgencyControlDetails.ForceAllPwdBy + " on " + LookupDataAccess.Getdate(propAgencyControlDetails.ForceAllPwdOn));
            }
        }

        private void setTooltip(int rowIndex, DataRow dr)
        {

            string toolTipText = "Added By    : " + dr["PWR_ADD_OPERATOR"].ToString().Trim() + " on " + dr["PWR_DATE_ADD"].ToString() + "\n";
            //toolTipText += "Modified By : " + dr["PWR_LSTC_OPERATOR"].ToString().Trim() + " on " + dr["PWR_DATE_LSTC"].ToString();
            string modifiedBy = string.Empty;
            if (!dr["PWR_LSTC_OPERATOR"].ToString().Trim().Equals(string.Empty))
                modifiedBy = dr["PWR_LSTC_OPERATOR"].ToString().Trim() + " on " + dr["PWR_DATE_LSTC"].ToString();
            toolTipText += "Modified By : " + modifiedBy;
            foreach (DataGridViewCell cell in gvwCustomer.Rows[rowIndex].Cells)
            {
                cell.ToolTipText = toolTipText;
            }
        }

        private void TabControls()
        {
            //_serviceHierarchy = new GridControl(BaseForm, "Service", null);
            _serviceHierarchy = new ClientInq_GridControl(BaseForm, "Service", null, "View");
            _serviceHierarchy.Dock = DockStyle.Fill;
            _serviceHierarchy.SetVisible = false;
            this.tabPageService.Controls.Add(_serviceHierarchy);

            _servicePlanHierarchy = new SerHierarchyControl(BaseForm, "Service", null, "View");
            _servicePlanHierarchy.Dock = DockStyle.Fill;
            _servicePlanHierarchy.SetVisible = false;
            _servicePlanHierarchy.Height = 250;
            _servicePlanHierarchy.GridViewControl.Height = 220;
            this.tabPageServicePlanHie.Controls.Add(_servicePlanHierarchy);

            _intakeHierarchy = new GridControl(BaseForm, "InTake", null, "View");
            _intakeHierarchy.GridViewControl.Columns["cellSites"].Visible = false;
            _intakeHierarchy.Dock = DockStyle.Top;
            _intakeHierarchy.SetVisible = false;
            _intakeHierarchy.Height = 250;
            _intakeHierarchy.Width = 580;
            _intakeHierarchy.GridViewControl.Width = 680;
            this.tabPageIntake.Controls.Add(_intakeHierarchy);
            _screenPrivileges = new PrivilegesControl(BaseForm, "Screen", null);
            _screenPrivileges.Dock = DockStyle.Top;
            _screenPrivileges.SetVisible = false;
            this.tabPageScreen.Controls.Add(_screenPrivileges);
            _reportPrivileges = new PrivilegesControl(BaseForm, "Reports", null);
            _reportPrivileges.Dock = DockStyle.Top;
            _reportPrivileges.SetVisible = false;
            this.tabPageReport.Controls.Add(_reportPrivileges);
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public ToolBarButton ToolBarEdit { get; set; }

        public ToolBarButton ToolBarNew { get; set; }

        public ToolBarButton ToolBarExcel { get; set; }

        public ToolBarButton ToolBarHelp { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }

        #endregion

        private void ClearControls()
        {

            cbAccessAll.Checked = false;

            cbforcePassChange.Checked = false;
            cbTemplateUser.Checked = false;
            chkSearchPIP.Checked = false;
            cmbEMS.SelectedIndex = 0;
            cmbSecurity.SelectedIndex = 0;
            cmbSite.SelectedIndex = 0;
            cmbStaff.SelectedIndex = 0;
            gvwComponents.Rows.Clear();
            _intakeHierarchy.GridViewControl.Rows.Clear();
            _servicePlanHierarchy.GridViewControl.Rows.Clear();
            _serviceHierarchy.GridViewControl.Rows.Clear();
            _reportPrivileges.GridViewControl.Rows.Clear();
            _screenPrivileges.GridViewControl.Rows.Clear();
            SetImageTypes(string.Empty);

            txtCaseWorker.Text = "";
            maskPhone.Text = "";
            dtpDob.Text = "";
            txtEmail.Text = "";
        }

        private void OnSearchClick(object sender, EventArgs e)
        {
            gvwCustomer.Rows.Clear();
            ClearControls();
            string empNo = txtUserID.Text.ToString();
            string firstName = txtFirstName.Text.ToString();
            string lastName = txtLastName.Text.ToString();
            string status = ((ListItem)cmbStatus.SelectedItem).Value.ToString();

            DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(empNo, firstName, lastName, status, BaseForm.UserID, BaseForm.BaseAdminAgency);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                dt = ds.Tables[0];

            if(dt.Rows.Count>0)
            {
                DataView dv = new DataView(dt);
                dv.Sort = "PWR_INACTIVE_FLAG";
                dt = dv.ToTable();
            }

            int intActivecount = 0;
            int intinactivecount = 0;
            int int30dayscount = 0;
            if (dt.Rows.Count > 0)
            {
                int rowIndex = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    string userStatus = "Active";
                    if (dr["PWR_INACTIVE_FLAG"].ToString().Equals("Y"))
                    {
                        userStatus = "Inactive";
                        intinactivecount = intinactivecount + 1;
                    }
                    else
                    {
                        if (BaseForm.UserID == "CAPLOGICS")
                        {
                            intActivecount = intActivecount + 1;
                        }
                        else
                        {
                            if (BaseForm.UserID == "JAKE")
                            {
                                if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                                    intActivecount = intActivecount + 1;
                            }
                            else
                            {
                                if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                                {
                                    intActivecount = intActivecount + 1;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["PWR_LAST_SUCCESSFUL_DATE"].ToString()))
                    {
                        if (DateTime.Now.Date.AddDays(-30).Date < Convert.ToDateTime(dr["PWR_LAST_SUCCESSFUL_DATE"]).Date)
                        {
                            int30dayscount = int30dayscount + 1;
                        }
                    }
                    string strDefaultHie = string.Empty;
                    if (!(string.IsNullOrEmpty(dr["PWR_DEF_AGENCY"].ToString())))
                    {
                        strDefaultHie = dr["PWR_DEF_AGENCY"] + "-" + dr["PWR_DEF_DEPT"] + "-" + dr["PWR_DEF_PROG"];
                    }
                    if (BaseForm.UserID.ToUpper() == "CAPLOGICS")
                    {
                        rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), userStatus, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                        gvwCustomer.Rows[rowIndex].Tag = dr;
                        setTooltip(rowIndex, dr);
                    }
                    else
                    {
                        if (BaseForm.UserID.ToUpper() == "JAKE")
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                            {
                                rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), userStatus, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                                gvwCustomer.Rows[rowIndex].Tag = dr;
                                setTooltip(rowIndex, dr);
                            }
                        }
                        else
                        {
                            if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                            {
                                rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_NAME_IX_FIRST"].ToString(), dr["PWR_NAME_IX_LAST"].ToString(), dr["PWR_CASEWORKER"].ToString(), userStatus, strDefaultHie, LookupDataAccess.Getdate(dr["PWR_CHANGE_DATE"].ToString()));
                                gvwCustomer.Rows[rowIndex].Tag = dr;
                                setTooltip(rowIndex, dr);
                            }
                        }
                    }
                    if (userStatus.Equals("Inactive"))
                    {
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }

                ToolBarEdit.Visible = true;
                ToolBarEdit.Enabled = true;
            }
            else
            {

                ToolBarEdit.Visible = false;
                AlertBox.Show("No Record found with this criteria", MessageBoxIcon.Warning, null, ContentAlignment.BottomRight);
                //CommonFunctions.MessageBoxDisplay(Consts.Messages.Recordsornotfound);
            }
            txtActiveCount.Text = intActivecount.ToString();
            txtInactivecount.Text = intinactivecount.ToString();
            txt30daysCount.Text = "0";
            if (gvwCustomer.Rows.Count > 0)
            {
                gvwCustomer.CurrentCell = gvwCustomer.Rows[0].Cells[1];
                txt30daysCount.Text = int30dayscount.ToString();
                gvwCustomer.Rows[0].Selected = true;
                gvwCustomer_SelectionChanged(gvwCustomer, new EventArgs());
            }

        }

        private void gvwCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwCustomer.SelectedRows.Count > 0)
            {
                DataRow row = gvwCustomer.SelectedRows[0].Tag as DataRow;
                if (row != null)
                {
                    ClearControls();

                    string strCaseworker = row["PWR_CASEWORKER"].ToString();
                    string strDOB = row["PWR_DOB"].ToString();
                    string strMobile = row["PWR_MOBILE"].ToString();
                    string strEmail = row["PWR_EMAIL"].ToString();

                    txtCaseWorker.Text = strCaseworker;
                    maskPhone.Text = strMobile;
                    dtpDob.Text = strDOB;
                    txtEmail.Text = strEmail;


                    string UserID = row["PWR_EMPLOYEE_NO"].ToString();

                    SetComboBoxValue(cmbEMS, row["PWR_EMS_ACCESS"].ToString());
                    SetComboBoxValue(cmbSecurity, row["PWR_SECURITY"].ToString());
                    SetComboBoxValue(cmbSite, row["PWR_SITE"].ToString());
                    SetComboBoxValue(cmbStaff, row["PWR_STAFF_CODE"].ToString());
                    fillComponents(row["PWR_COMPONENTS"].ToString());
                    SetImageTypes(row["PWR_IMAGE_TYPES"].ToString());

                    string userStatus = row["PWR_INACTIVE_FLAG"].ToString();

                    string userAccess = row["PWR_ACCESS_ALL"].ToString();
                    if (userAccess.Equals("Y"))
                    {
                        cbAccessAll.Checked = true;
                    }
                    else
                    {
                        cbAccessAll.Checked = false;
                    }

                    if (row["PWR_SEARCH_PIP"].ToString() == "Y")
                    { chkSearchPIP.Checked = true; }
                    else
                    {
                        chkSearchPIP.Checked = false;
                    }
                    string templateUser = row["PWR_TEMPLATE_USER"].ToString();
                    if (templateUser.Equals("Y"))
                    {
                        cbTemplateUser.Checked = true;
                    }
                    else
                    {
                        cbTemplateUser.Checked = false;
                    }
                    string forcePassword = row["PWR_SUCCESSFUL"].ToString();
                    if (forcePassword.Equals("0"))
                    {
                        cbforcePassChange.Checked = true;
                    }
                    else
                    {
                        cbforcePassChange.Checked = false;
                    }
                    CaptainModel model = new CaptainModel();
                    List<HierarchyEntity> userHierarchy = model.UserProfileAccess.GetUserHierarchyByID(UserID);
                    List<HierarchyEntity> userServiceHierarchy = _model.UserProfileAccess.GetUserServiceHierarchyByID(UserID);
                    List<CLINQHIEEntity> ClientHierarchy = _model.UserProfileAccess.GetClentInqByID(UserID);
                    _intakeHierarchy.GridViewControl.Rows.Clear();
                    _servicePlanHierarchy.GridViewControl.Rows.Clear();
                    _serviceHierarchy.GridViewControl.Rows.Clear();


                    List<HierarchyEntity> ointakeHeadHierarchy = userHierarchy.FindAll(x => x.HirarchyType == "I");
                    //List<HierarchyEntity> oSPHierarchieslst = userHierarchy.FindAll(x => x.HirarchyType == "S");

                    List<HierarchyEntity> oSPHierarchieslst = userServiceHierarchy;

                    if (ointakeHeadHierarchy.Count > 0)
                    {
                        foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                        {
                            string code = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                            string hierarchyName = hierarchy.HirarchyName.ToString();
                            if (hierarchy.HirarchyType.Equals("I"))
                            {
                                int rowIndex = _intakeHierarchy.GridViewControl.Rows.Add(code, hierarchyName, hierarchy.Sites);
                                _intakeHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                                if (hierarchy.UsedFlag.Equals("Y"))
                                {
                                    _intakeHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                //int rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add(code, hierarchyName);
                                //_servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                                //if (hierarchy.UsedFlag.Equals("Y"))
                                //{
                                //    _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                //}
                            }
                        }
                    }

                    if (ointakeHeadHierarchy.Count > 0)
                    {
                        ointakeHeadHierarchy = ointakeHeadHierarchy.OrderBy(u => u.UsedFlag).ThenBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();

                        _servicePlanHierarchy.GridViewControl.Rows.Clear();
                        List<HierarchyEntity> oSPHierachies = new List<HierarchyEntity>();
                        if (oSPHierarchieslst.Count > 0)
                            oSPHierachies = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();

                        List<string> lstServices = new List<string>();
                        int rowIndex = 0;

                        foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                        {
                            string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                            string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                            string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                            string SearchIntakCode = Agy + "-" + Dept + "-" + Prog;
                            string hierarchyName = hierarchy.HirarchyName.ToString();

                            rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((SearchIntakCode), hierarchyName, "", "");
                            _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                            lstServices.Add(SearchIntakCode);
                            if (hierarchy.UsedFlag.Equals("Y"))
                            {
                                _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }

                            List<HierarchyEntity> SPFilterHier = oSPHierachies.FindAll(x => x.SerAgency == hierarchy.Agency && x.SerDept == hierarchy.Dept && x.SerProg == hierarchy.Prog);
                            if (SPFilterHier.Count > 0)
                            {
                                rowIndex = Captain.Common.Utilities.CommonFunctions.BuildSerHIEGrid(SPFilterHier, _servicePlanHierarchy.GridViewControl);
                            }

                            ////CASE 1 :: **-**-**    || If the intake hierarchy is **-**-**
                            //if (Agy == "**" && Dept == "**" && Prog == "**")
                            //{

                            //    if (oSPHierachies.Count > 0)
                            //    {
                            //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(oSPHierachies, _servicePlanHierarchy.GridViewControl);
                            //    }
                            //}

                            ////CASE 2 :: AGY-DEPT-**    || If the intake hierarchy has Agency-Depratment-**
                            //if (Agy != "**" && Dept != "**" && Prog == "**")
                            //{
                            //    //Added by Sudheer on 04/14/23
                            //    List<HierarchyEntity> ofilterSerAgyDepHIE = new List<HierarchyEntity>();
                            //    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
                            //    {
                            //        if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "D")
                            //            ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);
                            //        else
                            //            ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                            //    }
                            //    else
                            //        ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);

                            //    //List<HierarchyEntity> ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);
                            //    if (ofilterSerAgyDepHIE.Count > 0)
                            //    {
                            //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerAgyDepHIE, _servicePlanHierarchy.GridViewControl);
                            //    }
                            //}

                            ////CASE 3 :: AGY-**-**    || If the intake hierarchy has only Agency-**-**
                            //if (Agy != "**" && Dept == "**" && Prog == "**")
                            //{
                            //    List<HierarchyEntity> ofilterSerAgyHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                            //    if (ofilterSerAgyHIE.Count > 0)
                            //    {
                            //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerAgyHIE, _servicePlanHierarchy.GridViewControl);
                            //    }
                            //}

                            ////CASE 4 :: AGY-DEPT-PROG    || If the intake hierarchy has Agency-Department-Program
                            //if (Agy != "**" && Dept != "**" && Prog != "**")
                            //{
                            //    //Added by Sudheer on 04/13/2023
                            //    List<HierarchyEntity> ofilterSerHIE = new List<HierarchyEntity>();
                            //    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
                            //    {
                            //        if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "D")
                            //            ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);
                            //        else
                            //            ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                            //    }
                            //    else
                            //        ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && x.Prog == hierarchy.Prog);

                            //    //List<HierarchyEntity> ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && x.Prog == hierarchy.Prog);
                            //    if (ofilterSerHIE.Count > 0)
                            //    {
                            //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerHIE, _servicePlanHierarchy.GridViewControl);
                            //    }
                            //}

                        }
                    }

                    #region Changed the code to display SPs based on Intake Hierarchies selection
                    /*
                    if (oSPHeadHierarchy.Count > 0) {

                        /*************** Kranthi :: NEW CONCEPT on 02/02/2023 :: Keep in mind document *********************
                        List<string> lstServices = new List<string>();
                        int rowIndex = 0;
                        foreach (HierarchyEntity hierarchy in oSPHeadHierarchy)
                        {
                            string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                            string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                            string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                            string code = Agy + "-" + Dept + "-" + Prog;
                            string hierarchyName = hierarchy.HirarchyName.ToString();

                            string searchHeadCode = hierarchy.Agency + "-**-**";
                            string searchCode = hierarchy.Agency + "-" + hierarchy.Dept + "-**";


                            HierarchyEntity ofilterintkHeadHIE = ointakeHeadHierarchy.Find(x => (x.Agency == hierarchy.Agency && x.Dept == "**" && x.Prog == "**"));
                            if (ofilterintkHeadHIE != null)
                            {
                                if (!lstServices.Contains(searchHeadCode))
                                {
                                    rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((ofilterintkHeadHIE.Agency + "-" + ofilterintkHeadHIE.Dept + "-" + ofilterintkHeadHIE.Prog), ofilterintkHeadHIE.HirarchyName, "", "");
                                    _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                                    lstServices.Add(searchHeadCode);
                                    if (ofilterintkHeadHIE.UsedFlag.Equals("Y"))
                                    {
                                        _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }

                            }

                            HierarchyEntity ofilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency == hierarchy.Agency && x.Dept== hierarchy.Dept && x.Prog=="**"));
                            if (ofilterintkHIE!=null)
                            {
                                if (!lstServices.Contains(searchCode))
                                {
                                    rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((ofilterintkHIE.Agency + "-" + ofilterintkHIE.Dept + "-" + ofilterintkHIE.Prog), ofilterintkHIE.HirarchyName, "", "");
                                    lstServices.Add(searchCode);
                                    if (ofilterintkHIE.UsedFlag.Equals("Y"))
                                    {
                                        _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }
                            }
                            else
                            {
                                List<HierarchyEntity> ofilterintkHIE2 = ointakeHeadHierarchy.FindAll(x => (x.Agency + "-" + x.Dept + "-" + x.Prog) == code);
                                if (ofilterintkHIE2.Count > 0)
                                {
                                    if (!lstServices.Contains(code))
                                    {
                                        rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((ofilterintkHIE2[0].Agency + "-" + ofilterintkHIE2[0].Dept + "-" + ofilterintkHIE2[0].Prog), ofilterintkHIE2[0].HirarchyName, "", "");
                                        lstServices.Add(code);
                                        if (ofilterintkHIE2[0].UsedFlag.Equals("Y"))
                                        {
                                            _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                        }
                                    }
                                }
                                else
                                {
                                    HierarchyEntity ostarfilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency == "**" && x.Dept == "**" && x.Prog == "**"));
                                    if (ostarfilterintkHIE != null)
                                    {
                                        string strSearchstarcCode = "**-**-**";
                                        if (!lstServices.Contains(strSearchstarcCode))
                                        {
                                            rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((strSearchstarcCode), ostarfilterintkHIE.HirarchyName, "", "");
                                            _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                                            lstServices.Add(strSearchstarcCode);
                                            if (ostarfilterintkHIE.UsedFlag.Equals("Y"))
                                            {
                                                _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                            }
                                        }
                                    }
                                }
                            }

                            rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add("", "", code, hierarchyName);
                            _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                            if (hierarchy.UsedFlag.Equals("Y"))
                            {
                                _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                        ************************************************************************
                    }

                    */
                    #endregion

                    if (ClientHierarchy.Count > 0)
                    {
                        foreach (CLINQHIEEntity hierarchy in ClientHierarchy)
                        {
                            string code = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                            string hierarchyName = hierarchy.HirarchyName.ToString();
                            int rowIndex = _serviceHierarchy.GridViewControl.Rows.Add(code, hierarchyName, hierarchy.CLINQPdf == "Y" ? true : false, hierarchy.CLINQCNotes == "Y" ? true : false);
                            _serviceHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                            if (hierarchy.UsedFlag.Equals("Y"))
                            {
                                _serviceHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }

                        }
                    }

                    List<PrivilegeEntity> userPrivileges = model.UserProfileAccess.GetUserPrivilegesByID(UserID, "Screen", string.Empty);
                    _screenPrivileges.GridViewControl.Rows.Clear();

                    userPrivileges = userPrivileges.OrderBy(u => u.ModuleCode).ThenBy(u => u.Hierarchy).ThenBy(u => u.PrivilegeName).ToList();
                    foreach (PrivilegeEntity privileges in userPrivileges)
                    {
                        string screenName = privileges.PrivilegeName;
                        int rowIndex = _screenPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.Hierarchy, privileges.ViewPriv, privileges.AddPriv, privileges.ChangePriv, privileges.DelPriv);
                        _screenPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;

                        /*Hiding the checkboxes for particular screens -- kranthi 07-07-2022 */
                        LookupDataAccess.Hide_ScreenPrivilages_checkboxes(privileges, _screenPrivileges.GridViewControl, rowIndex);
                    }

                    List<PrivilegeEntity> userReports = model.UserProfileAccess.GetUserPrivilegesByID(UserID, "Reports", string.Empty);
                    _reportPrivileges.GridViewControl.Rows.Clear();
                    foreach (PrivilegeEntity privileges in userReports)
                    {
                        string screenName = privileges.PrivilegeName;
                        int rowIndex = _reportPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.ViewPriv);
                        _reportPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;
                    }
                }
                setControlEnabled(false);
            }
        }

        private void fillComponents(string components)
        {
            List<string> incomeList = new List<string>();
            if (components != null)
            {
                string[] incomeTypes = components.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }

            DataSet ds = Captain.DatabaseLayer.Lookups.GetLookUpFromAGYTAB("00356");
            //DataSet ds = Captain.DatabaseLayer.Lookups.GetAdditionalPrivileges();
            DataTable dt = ds.Tables[0];
            List<ListItem> listItem = new List<ListItem>();

            listItem.Add(new ListItem("None", "None", "false", "false"));
            listItem.Add(new ListItem("All", "****", "false", "false"));
            foreach (DataRow dr in dt.Rows)
            {
                //listItem.Add(new ListItem(dr["AGY_7"].ToString(), dr["AGY_4"].ToString(), "false", "false"));
                listItem.Add(new ListItem(dr["LookupDesc"].ToString(), dr["Code"].ToString(), "false", "false"));
            }
            gvwComponents.Rows.Clear();
            foreach (ListItem listComponents in listItem)
            {
                if (components != null && incomeList.Contains(listComponents.Value.ToString()))
                {
                    int rowIndex = gvwComponents.Rows.Add(listComponents.Value, listComponents.Text);
                    gvwComponents.Rows[rowIndex].Tag = listComponents;
                }
            }
        }

        public override void PopulateToolbar(ToolBar toolBar)
        {
            base.PopulateToolbar(toolBar);

            bool toolbarButtonInitialized = ToolBarNew != null;
            ToolBarButton divider = new ToolBarButton();
            divider.Style = ToolBarButtonStyle.Separator;

            if (toolBar.Controls.Count == 0)
            {
                ToolBarNew = new ToolBarButton();
                ToolBarNew.Tag = "New";
                ToolBarNew.ToolTipText = "Add New User";
                ToolBarNew.Enabled = true;
                ToolBarNew.ImageSource = @"captain-add"; //new IconResourceHandle(Consts.Icons16x16.AddItem);
                ToolBarNew.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarNew.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.AddPriv.Equals("false"))
                {
                    ToolBarNew.Enabled = false;
                }

                ToolBarEdit = new ToolBarButton();
                ToolBarEdit.Tag = "Edit";
                ToolBarEdit.ToolTipText = "Edit User";
                ToolBarEdit.Enabled = true;
                //ToolBarEdit.Image = new IconResourceHandle(Consts.Icons16x16.EditIcon);
                // ToolBarEdit.ImageSource = "resource.wx/_16X16.EditIcon.gif";
                ToolBarEdit.ImageSource = @"captain-edit";
                ToolBarEdit.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarEdit.Click += new EventHandler(OnToolbarButtonClicked);
                if (Privileges.ChangePriv.Equals("false"))
                {
                    ToolBarEdit.Enabled = false;

                }

                ToolBarExcel = new ToolBarButton();
                ToolBarExcel.Tag = "Excel";
                ToolBarExcel.ToolTipText = "User Account & Privileges Report in Excel";
                ToolBarExcel.Enabled = true;
                //ToolBarExcel.Image = new IconResourceHandle(Consts.Icons16x16.MSExcel);
                // ToolBarExcel.ImageSource = "resource.wx/_16X16.MSExcel.gif";
                ToolBarExcel.ImageSource = @"captain-excel";
                ToolBarExcel.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarExcel.Click += new EventHandler(OnToolbarButtonClicked);

                ToolBarHelp = new ToolBarButton();
                ToolBarHelp.Tag = "Help";
                ToolBarHelp.ToolTipText = "User Help";
                ToolBarHelp.Enabled = true;
                // ToolBarHelp.Image = new IconResourceHandle(Consts.Icons16x16.Help);
                //ToolBarHelp.ImageSource = "resource.wx/_16X16.Help.gif";
                ToolBarHelp.ImageSource = @"icon-help";
                ToolBarHelp.Click -= new EventHandler(OnToolbarButtonClicked);
                ToolBarHelp.Click += new EventHandler(OnToolbarButtonClicked);
            }

            toolBar.Buttons.AddRange(new ToolBarButton[]
            {
                ToolBarNew,
                ToolBarEdit,
                 ToolBarExcel,
                ToolBarHelp
            });

            DisabletoolBarButtons();
        }

        private void DisabletoolBarButtons()
        {
            if (gvwCustomer.Rows.Count > 0)
            {
                if (Privileges.ChangePriv.Equals("false"))
                {
                    ToolBarEdit.Enabled = false;
                }
                else
                    ToolBarEdit.Enabled = true;

            }
            else
            {
                if (ToolBarEdit != null)
                    ToolBarEdit.Enabled = false;

            }
        }

        /// <summary>
        /// Handles the toolbar button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            ToolBarButton btn = (ToolBarButton)sender;
            StringBuilder executeCode = new StringBuilder();

            executeCode.Append(Consts.Javascript.BeginJavascriptCode);
            if (btn.Tag == null) { return; }
            try
            {
                switch (btn.Tag.ToString())
                {
                    case Consts.ToolbarActions.New:
                        if (gvwCustomer.SortOrder.ToString() != "None")
                        { SortColumn = gvwCustomer.SortedColumn.Name.ToString(); SortOrder = gvwCustomer.SortOrder.ToString(); }

                        AddUserForm addUserForm = new AddUserForm(BaseForm, "Add", null, Privileges);
                        addUserForm.FormClosed += AddUserForm_FormClosed;
                        addUserForm.StartPosition = FormStartPosition.CenterScreen;
                        addUserForm.ShowDialog();
                        break;
                    case Consts.ToolbarActions.Edit:
                        string selectedRowUser = GetSelectedRow();
                        if (gvwCustomer.Rows.Count > 0)
                            if (!selectedRowUser.Equals(string.Empty))
                            {
                                if (gvwCustomer.SortOrder.ToString() != "None")
                                { SortColumn = gvwCustomer.SortedColumn.Name.ToString(); SortOrder = gvwCustomer.SortOrder.ToString(); }

                                AddUserForm editUserForm = new AddUserForm(BaseForm, "Edit", selectedRowUser, Privileges);

                                editUserForm.FormClosed += AddUserForm_FormClosed;
                                editUserForm.StartPosition = FormStartPosition.CenterScreen;
                                editUserForm.ShowDialog();

                            }
                            else
                            {
                                MessageBox.Show("Please select the user to edit", Consts.Common.ApplicationCaption);
                            }
                        break;
                    case Consts.ToolbarActions.Excel:
                        AssignUserReportForm objForm = new AssignUserReportForm(BaseForm, Privileges);
                        objForm.StartPosition = FormStartPosition.CenterScreen;
                        objForm.ShowDialog();
                        break;

                    case Consts.ToolbarActions.Help:
                        Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
                        break;

                }
                executeCode.Append(Consts.Javascript.EndJavascriptCode);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogAndDisplayMessageToUser(new StackFrame(true), ex, QuantumFaults.None, ExceptionSeverityLevel.High);
            }
        }

        private void AddUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (BaseForm.isPrivSaveFlag == "Y")
            {
                // Kranthi :: 03/06/2023 :: PCS.docx :: Wants to close all the Menu Module items when user logged in. They want to open on their own.
                // If we refresh the screen in the above sutiation the menuu will close automatically.

                BaseForm.strcurrentModuleID = "01";
                BaseForm.BuildMenuTree();

            }

            BaseForm.isPrivSaveFlag = "N";
        }

        private void fillDropdowns()
        {
            fillEMS();
            fillSecurity();
            fillStatus();
            DataSet ds = Captain.DatabaseLayer.Lookups.GetStaff();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cmbStaff.Items.Add(new ListItem(dr["STF_NAME"].ToString(), dr["STF_CODE"].ToString()));
            }
            cmbStaff.Items.Insert(0, new ListItem("All", "********"));
            cmbStaff.Items.Insert(0, new ListItem("None", "0"));
            cmbStaff.SelectedIndex = 0;

            ds = Captain.DatabaseLayer.Lookups.GetCaseSite();
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cmbSite.Items.Add(new ListItem(dr["SITE_NAME"].ToString(), dr["SITE_KEY"].ToString().Trim()));
            }
            cmbSite.Items.Insert(0, new ListItem("    ", string.Empty));
            cmbSite.Items.Insert(1, new ListItem("All Sites", "****"));
            cmbSite.SelectedIndex = 0;
            //  fillAddlPrivileges();
        }

        private void SetImageTypes(string ImageTypes)
        {
            List<string> imageTypeList = new List<string>();
            if (ImageTypes != null)
            {
                string[] imageTypes = ImageTypes.Split(' ');
                for (int i = 0; i < imageTypes.Length; i++)
                {
                    imageTypeList.Add(imageTypes.GetValue(i).ToString());
                }
            }
            clstImageTypes.Items.Clear();
            List<CommonEntity> commonEntity = _model.lookupDataAccess.GetImageTypes();

            foreach (CommonEntity dr in commonEntity)
            {
                string code = dr.Code;
                bool flag = false;
                if (imageTypeList.Contains(code))
                {
                    flag = true;
                }
                clstImageTypes.Items.Add(new ListItem(dr.Desc, dr.Code), flag);
            }
        }

        private void fillEMS()
        {
            cmbEMS.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", ""));
            listItem.Add(new ListItem("Authorization (Level 0)", "A"));
            listItem.Add(new ListItem("Supervisory (Level 1)", "S"));
            listItem.Add(new ListItem("Departmental (Level 2)", "D"));
            cmbEMS.Items.AddRange(listItem.ToArray());
            cmbEMS.SelectedIndex = 0;
        }

        private void fillStatus()
        {
            cmbStatus.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Both Active and Inactive", "All"));
            listItem.Add(new ListItem("Active", "N"));
            listItem.Add(new ListItem("Inactive", "Y"));
            cmbStatus.Items.AddRange(listItem.ToArray());
            cmbStatus.SelectedIndex = 0;
        }

        private void fillSecurity()
        {
            cmbSecurity.Items.Clear();
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("Select One", ""));
            listItem.Add(new ListItem("Reception/Clerical", "R"));
            listItem.Add(new ListItem("Case Manager", "C"));
            listItem.Add(new ListItem("Program Administrator", "P"));
            listItem.Add(new ListItem("Both PA and CM", "B"));
            cmbSecurity.Items.AddRange(listItem.ToArray());
            cmbSecurity.SelectedIndex = 0;
        }

        private void fillAddlPrivileges()
        {
            DataSet ds = Captain.DatabaseLayer.Lookups.GetAdditionalPrivileges();
            DataTable dt = ds.Tables[0];
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("None", "0"));
            listItem.Add(new ListItem("All", "****"));
            foreach (DataRow dr in dt.Rows)
            {
                listItem.Add(new ListItem(dr["AGY_7"].ToString(), dr["AGY_4"].ToString()));
            }

        }

        /// <summary>
        /// Get Selected Rows Tag Clas.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRow()
        {
            string UserID = string.Empty;
            if (gvwCustomer != null && gvwCustomer.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvwCustomer.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        DataRow srow = dr.Tag as DataRow;
                        UserID = srow["PWR_EMPLOYEE_NO"].ToString().Trim();
                        break;
                    }
                }
            }
            return UserID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void setControlEnabled(bool flag)
        {

            cbAccessAll.Enabled = flag;
            cbforcePassChange.Enabled = flag;
            cbTemplateUser.Enabled = flag;
            chkSearchPIP.Enabled = flag;
            cmbEMS.Enabled = flag;
            clstImageTypes.Enabled = flag;
            cmbSecurity.Enabled = flag;
            cmbSite.Enabled = flag;
            cmbStaff.Enabled = flag;

            txtCaseWorker.Enabled = flag;
            maskPhone.Enabled = flag;
            dtpDob.Enabled = flag;
            txtEmail.Enabled = flag;
        }

        //private void saveUserInfo()
        //{
        //    if (ValidateForm())
        //    {
        //        //Add PASSWORD
        //        List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //        sqlParamList.Add(new SqlParameter("@PWR_EMPLOYEE_NO", txtUserID.Text));
        //        sqlParamList.Add(new SqlParameter("@PWR_CASEWORKER", txtCase.Text));
        //        sqlParamList.Add(new SqlParameter("@PWR_NAME_IX_LAST", txtFname.Text));
        //        sqlParamList.Add(new SqlParameter("@PWR_NAME_IX_FIRST", txtLname.Text));
        //        sqlParamList.Add(new SqlParameter("@PWR_SECURITY", ((ListItem)cmbSecurity.SelectedItem).Value.ToString()));
        //        if (cbAccessAll.Checked)
        //        {
        //            sqlParamList.Add(new SqlParameter("@PWR_ACCESS_ALL", "Y"));
        //        }
        //        else
        //        {
        //            sqlParamList.Add(new SqlParameter("@PWR_ACCESS_ALL", "N"));
        //        }
        //        sqlParamList.Add(new SqlParameter("@PWR_TEMPLATE_USER", "N"));
        //        if (!((ListItem)cmbSecurity.SelectedItem).Value.ToString().Equals("0"))
        //        {
        //            sqlParamList.Add(new SqlParameter("@PWR_STAFF_CODE", ((ListItem)cmbSecurity.SelectedItem).Value.ToString()));
        //        }

        //        if (!((ListItem)cmbSite.SelectedItem).Value.ToString().Equals("0"))
        //        {
        //            sqlParamList.Add(new SqlParameter("@PWR_SITE", ((ListItem)cmbSite.SelectedItem).Value.ToString()));
        //        }
        //        if (!((ListItem)cmbEMS.SelectedItem).Value.ToString().Equals("0"))
        //        {
        //            sqlParamList.Add(new SqlParameter("@PWR_EMS_ACCESS", ((ListItem)cmbEMS.SelectedItem).Value.ToString()));
        //        }
        //    }
        //}

        //private bool ValidateForm()
        //{
        //    bool isValid = true;

        //    if (String.IsNullOrEmpty(txtFname.Text))
        //    {
        //        _errorProvider.SetError(txtFname, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblfname.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        isValid = false;
        //    }
        //    else
        //    {
        //        _errorProvider.SetError(txtFname, null);
        //    }

        //    if (String.IsNullOrEmpty(txtLname.Text))
        //    {
        //        _errorProvider.SetError(txtLname, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLname.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        isValid = false;
        //    }
        //    else
        //    {
        //        _errorProvider.SetError(txtLname, null);
        //    }

        //    if (String.IsNullOrEmpty(txtCase.Text))
        //    {
        //        _errorProvider.SetError(txtCase, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCase.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        isValid = false;
        //    }
        //    else
        //    {
        //        _errorProvider.SetError(txtCase, null);
        //    }

        //    if (((ListItem)cmbSecurity.SelectedItem).Value.ToString() == "0")
        //    {
        //        _errorProvider.SetError(cmbSecurity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSecurity.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        isValid = false;
        //    }
        //    else
        //    {
        //        _errorProvider.SetError(cmbSecurity, null);
        //    }

        //    if (cmbSecurity.SelectedItem == null || ((ListItem)cmbSecurity.SelectedItem).Text == Consts.Common.SelectOne)
        //    {
        //        _errorProvider.SetError(cmbSecurity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSecurity.Text.Replace(Consts.Common.Colon, string.Empty)));
        //        isValid = false;
        //    }
        //    else
        //    {
        //        _errorProvider.SetError(cmbSecurity, null);
        //    }

        //    return (isValid);
        //}

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Are you sure you want reset all passwords? ", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);
        }

        private void MessageBoxHandler(object sender, EventArgs e)
        {
            // Get Wisej.Web.Form object that called MessageBox
            Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            if (senderForm != null)
            {
                // Set DialogResult value of the Form as a text for label
                if (senderForm.DialogResult.ToString() == "Yes")
                {
                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@PWRTYPE", "PasswordSwitch"));
                    sqlParamList.Add(new SqlParameter("@PWR_LSTC_OPERATOR", BaseForm.UserID));

                    if (Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORD(sqlParamList))
                    {
                        CommonFunctions.MessageBoxDisplay("Force All Password Successfully Changed");
                        Forcepasswordswitch();
                    }

                }
            }
        }

        private void btnPasswordswitch_Click(object sender, EventArgs e)
        {
            ChangePassword changepwd = new ChangePassword(BaseForm, "AGCYCNTL");
            changepwd.StartPosition = FormStartPosition.CenterScreen;
            changepwd.ShowDialog();
        }

        private void tabPageDetails_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Click(object sender, EventArgs e)
        {

        }

        private void tabPageDetails_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void gvwCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            CommonFunctions.KeyPress((DataGridView)sender, 0, e.KeyChar);

        }

        private void pbxHelpIcon_Click(object sender, EventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 2, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }
    }
}