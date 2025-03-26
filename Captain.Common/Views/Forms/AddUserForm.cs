#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using NPOI.SS.Formula.Functions;
//using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
//using Gizmox.WebGUI.Common.Resources;

#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class AddUserForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _servicePlanHierarchy = null;
        private SerHierarchyControl _servicePlanHierarchy = null;   ////Kranthi 02/10/2023 :: Wisej misc 2-7-2023.docx :: change service plan grid to new format
        private ClientInq_GridControl _serviceHierarchy = null;
        private GridControl _intakeHierarchy = null;
        private PrivilegesControl _screenPrivileges = null;
        private PrivilegesControl _reportPrivileges = null;
        private List<HierarchyEntity> _userHierarchy = null;
        private List<ListItem> _selectedComponets = new List<ListItem>();
        private CaptainModel _model = null;

        #endregion

        public AddUserForm(BaseForm baseForm, string mode, string selectedUser, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            ListcaseSiteEntity = new List<CaseSiteEntity>();

            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            GlobalPrivileges = false;
            _model = new CaptainModel();
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N")
            {
                chkSearchPIP.Visible = false; // Kranthi 08/22/2024:: HCCAA.docx:: requested for want to hide in all systems
            }
            if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
            {

                tabPageServiceHierarchy.Show();
            }
            else
            {
                tabPageServiceHierarchy.Hide();
            }

            _servicePlanHierarchy = new SerHierarchyControl(BaseForm, "Service", null, Mode);
            //_servicePlanHierarchy = new SerHierarchyControl(BaseForm, "Service", null, Mode);
            _servicePlanHierarchy.Dock = DockStyle.Fill;
            this.tabPageServiceHierarchy.Controls.Add(_servicePlanHierarchy);
            _serviceHierarchy = new ClientInq_GridControl(BaseForm, "Service", null, Mode);
            _serviceHierarchy.Dock = DockStyle.Fill;
            this.tabPageService.Controls.Add(_serviceHierarchy);
            _intakeHierarchy = new GridControl(BaseForm, "InTake", null, Mode);
            _intakeHierarchy.Dock = DockStyle.Fill;
            this.tabPageIntake.Controls.Add(_intakeHierarchy);
            _screenPrivileges = new PrivilegesControl(BaseForm, "Screen", selectedUser);
            _screenPrivileges.Dock = DockStyle.Fill;
            _screenPrivileges.PrivelegeEntity = privilegeEntity;
            this.tabPageScreen.Controls.Add(_screenPrivileges);
            _reportPrivileges = new PrivilegesControl(BaseForm, "Reports", selectedUser);
            _reportPrivileges.Dock = DockStyle.Fill;
            _reportPrivileges.PrivelegeEntity = privilegeEntity;
            this.tabPageReport.Controls.Add(_reportPrivileges);
            fillDropdowns();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            if (Mode.Equals("Edit"))
            {

                SelectedUser = selectedUser;
                fillUserForm();
                setVisibleControls(false);
                _reportPrivileges.SetEditable = true;
                _screenPrivileges.SetEditable = true;
                _intakeHierarchy.SetEditable = true;
                _serviceHierarchy.SetEditable = true;
                _servicePlanHierarchy.SetEditable = true;
                btnCopy.Visible = false;
                btnResetPass.Visible = true;

                this.Text = "User Account and Privileges" + " - Edit";
                txtUserID.Focus();
                if (!(BaseForm.UserProfile.Security == "P" || BaseForm.UserProfile.Security == "B"))
                {
                    txtCaseWorker.Enabled = false;
                }
            }
            else
            {
                this.Text = "User Account and Privileges" + " - Add";

                btnResetPass.Visible = false;
                cbForcePassword.Checked = true;
                SetImageTypes(null);
                _reportPrivileges.SetEditable = false;
                _screenPrivileges.SetEditable = false;
                _intakeHierarchy.SetEditable = false;
                _serviceHierarchy.SetEditable = false;
                _servicePlanHierarchy.SetEditable = false;
                txtUserID.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;


            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;

                string hieType = form.HierarchyType;
                if (hieType.Equals("I"))
                {
                    _intakeHierarchy.GridViewControl.Rows.Clear();
                }
                else
                {
                    _servicePlanHierarchy.GridViewControl.Rows.Clear();
                }
                List<HierarchyEntity> intakeHierarchy = new List<Model.Objects.HierarchyEntity>();
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    if (hieType.Equals("I"))
                    {
                        int rowIndex = _intakeHierarchy.GridViewControl.Rows.Add(row.Code.ToString(), row.HirarchyName.ToString(), string.Empty);
                        _intakeHierarchy.GridViewControl.Rows[rowIndex].Tag = row;
                        intakeHierarchy.Add(row);
                    }
                    else
                    {
                        int rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add(row.Code.ToString(), row.HirarchyName.ToString());
                        _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = row;
                    }
                }
                _servicePlanHierarchy.proppUserIntakeHierarchy = intakeHierarchy;
                //RefreshGrid();
            }
        }

        private void OnSiteSelectionFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                //if (form.FormType == "CASESITE")
                //{
                ListcaseSiteEntity = form.SelectedCaseSiteEntity;
            }
        }

        private void fillUserForm()
        {
            if (SelectedUser != null)
            {
                UserEntity userProfile = _model.UserProfileAccess.GetUserProfileByID(SelectedUser);
                txtUserID.Text = userProfile.UserID;
                txtFirstName.Text = userProfile.FirstName;
                txtLastName.Text = userProfile.LastName;
                txtCaseWorker.Text = userProfile.CaseWorker;
                if (userProfile.Agency != string.Empty)
                    txtDefaultHierachy.Text = userProfile.Agency + "-" + userProfile.Dept + "-" + userProfile.Prog;
                SetComboBoxValue(cmbEMS, userProfile.EMS_Access);
                SetComboBoxValue(cmbSecurity, userProfile.Security);
                SetComboBoxValue(cmbSite, userProfile.Site);
                SetComboBoxValue(cmbStaff, userProfile.StaffCode);
                SetImageTypes(userProfile.ImageTypes);
                if (userProfile.InActiveFlag.Equals("Y"))
                {
                    chkbActive.Checked = false;
                }
                else
                {
                    chkbActive.Checked = true;
                }
                if (userProfile.AccessAll.Equals("Y"))
                {
                    cbAccessAll.Checked = true;
                }
                else
                {
                    cbAccessAll.Checked = false;
                }

                if (userProfile.TemplateUser.Equals("Y"))
                {
                    cbTemplateUser.Checked = true;
                }
                else
                {
                    cbTemplateUser.Checked = false;
                }
                if (userProfile.Successful.Equals("0"))
                {
                    cbForcePassword.Checked = true;
                }
                else
                {
                    cbForcePassword.Checked = false;
                }
                //if (userProfile.PWDSearchDatabase.Equals("Y"))
                //{ chkSearchDB.Checked = true; }
                //else
                //{
                //    chkSearchDB.Checked = false;
                //}
                if (userProfile.PWDSearchPIP.Equals("Y"))
                { chkSearchPIP.Checked = false; } // true; Kranthi 08/22/2024:: HCCAA.docx:: requested for want to hide in all systems
                else
                {
                    chkSearchPIP.Checked = false;
                }
                txtEmail.Text = userProfile.PWDEmail;

                if (!string.IsNullOrEmpty(userProfile.PWRDob.Trim()))
                {
                    dtpDob.Text = CommonFunctions.ChangeDateFormat(userProfile.PWRDob, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    dtpDob.Checked = true;
                }

                if (!string.IsNullOrEmpty(userProfile.PWRMobile.Trim()))
                    maskPhone.Text = userProfile.PWRMobile.Trim();


                fillComponents(userProfile.Components);
                _intakeHierarchy.UserProfile = userProfile;
                _servicePlanHierarchy.UserProfile = userProfile;
                _serviceHierarchy.UserProfile = userProfile;
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(SelectedUser);
                List<HierarchyEntity> userServiceHierarchy = _model.UserProfileAccess.GetUserServiceHierarchyByID(SelectedUser);
                List<CLINQHIEEntity> ClientHierarchy = _model.UserProfileAccess.GetClentInqByID(SelectedUser);
                _intakeHierarchy.GridViewControl.Rows.Clear();
                _serviceHierarchy.GridViewControl.Rows.Clear();
                _servicePlanHierarchy.GridViewControl.Rows.Clear();
                //_servicePlanHierarchy.SerGridViewControl.Rows.Clear();
                //UserHierarchy = userHierarchy.FindAll(u => u.HirarchyType.Equals("I")).ToList();
                List<HierarchyEntity> intakeHierarchy = new List<Model.Objects.HierarchyEntity>();


                List<HierarchyEntity> ointakeHeadHierarchy = userHierarchy.FindAll(x => x.HirarchyType == "I");
                //List<HierarchyEntity> oSPHierarchieslst = userHierarchy.FindAll(x => x.HirarchyType == "S");

                List<HierarchyEntity> oSPHierarchieslst = userServiceHierarchy;


                /*
                foreach (HierarchyEntity hierarchy in userHierarchy)
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

                        else
                        {
                            intakeHierarchy.Add(hierarchy);
                        }
                    }
                    else
                    {
                        int rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add(code, hierarchyName);
                        _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                        if (hierarchy.UsedFlag.Equals("Y"))
                        {
                            _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                }
                */

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
                            else
                            {
                                intakeHierarchy.Add(hierarchy);
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

                    List<HierarchyEntity> oSPHierachies = new List<HierarchyEntity>();
                    if (oSPHierarchieslst.Count > 0)
                        oSPHierachies = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();

                    _servicePlanHierarchy.GridViewControl.Rows.Clear();

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
                        //if (Agy == "**" && Dept == "**" && Prog == "**") {

                        //    if (oSPHierachies.Count > 0) {
                        //      rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(oSPHierachies, _servicePlanHierarchy.GridViewControl);
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

                #region Sel Hierarchies with Intake Hierarchies Kranthi Code 02162023
                /*
                if (oSPHeadHierarchy.Count > 0)
                {

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

                        //HierarchyEntity ofilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency + "-" + x.Dept + "-**") == searchCode);
                        HierarchyEntity ofilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && x.Prog == "**"));
                        if (ofilterintkHIE != null)
                        {
                            if (!lstServices.Contains(searchCode))
                            {
                                rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add((ofilterintkHIE.Agency + "-" + ofilterintkHIE.Dept + "-" + ofilterintkHIE.Prog), ofilterintkHIE.HirarchyName, "", "");
                                _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
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
                                    _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
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
                    /************************************************************************
            }
            */
                #endregion


                _servicePlanHierarchy.proppUserIntakeHierarchy = intakeHierarchy;

                ////Added by Sudheer on 07/14/2022
                //if(intakeHierarchy.Count>0)
                //{
                //    List<HierarchyEntity> SerHierarchy = new List<Model.Objects.HierarchyEntity>();
                //    SerHierarchy = userHierarchy.FindAll(u => u.HirarchyType.Equals("S"));
                //    _servicePlanHierarchy.SerGridViewControl.Rows.Clear();
                //    foreach (HierarchyEntity hierarchy in intakeHierarchy)
                //    {
                //        string code = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                //        string hierarchyName = hierarchy.HirarchyName.ToString();

                //        int rowIndex = _servicePlanHierarchy.SerGridViewControl.Rows.Add(code, hierarchyName, string.Empty,string.Empty);

                //        List<HierarchyEntity> Hierchyservicedata = new List<HierarchyEntity>();
                //        if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "A")
                //        {
                //            string strAgency = hierarchy.Agency == string.Empty ? "**" : hierarchy.Agency;

                //            if (strAgency == "**")
                //            {
                //                Hierchyservicedata = SerHierarchy;
                //            }
                //            else
                //            {
                //                Hierchyservicedata = SerHierarchy.FindAll(u => u.Agency == hierarchy.Agency);
                //            }

                //            foreach (HierarchyEntity hierarchyserviceEntity in Hierchyservicedata)
                //            {
                //                string scode = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                //                string shierarchyName = hierarchy.HirarchyName.ToString();

                //                rowIndex = _servicePlanHierarchy.SerGridViewControl.Rows.Add(string.Empty, string.Empty, scode, shierarchyName);
                //                if (hierarchyserviceEntity.UsedFlag.Equals("Y"))
                //                {
                //                    _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            string strAgency = hierarchy.Agency == string.Empty ? "**" : hierarchy.Agency;
                //            string strDept = hierarchy.Dept == string.Empty ? "**" : hierarchy.Dept;
                //            if (strAgency == "**")
                //            {
                //                Hierchyservicedata = SerHierarchy;
                //            }
                //            else if (strAgency != "**" && strDept == "**")
                //            {
                //                Hierchyservicedata = SerHierarchy.FindAll(u => u.Agency == hierarchy.Agency);
                //            }
                //            else
                //            {
                //                Hierchyservicedata = SerHierarchy.FindAll(u => u.Agency == hierarchy.Agency && u.Dept == hierarchy.Dept);
                //            }
                //            foreach (HierarchyEntity hierarchyserviceEntity in Hierchyservicedata)
                //            {
                //                string scode = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                //                string shierarchyName = hierarchy.HirarchyName.ToString();

                //                rowIndex = _servicePlanHierarchy.SerGridViewControl.Rows.Add(string.Empty, string.Empty, scode, shierarchyName);
                //                if (hierarchyserviceEntity.UsedFlag.Equals("Y"))
                //                {
                //                    _servicePlanHierarchy.GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                }
                //            }

                //        }
                //    }

                //}


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

                List<PrivilegeEntity> userPrivileges = _model.UserProfileAccess.GetUserPrivilegesByID(SelectedUser, "Screen", string.Empty);
                userPrivileges = userPrivileges.OrderBy(u => u.ModuleCode).ThenBy(u => u.Hierarchy).ThenBy(u => u.PrivilegeName).ToList();
                _screenPrivileges.GridViewControl.Rows.Clear();
                foreach (PrivilegeEntity privileges in userPrivileges)
                {
                    string screenName = privileges.PrivilegeName;
                    int rowIndex = _screenPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.Hierarchy, privileges.ViewPriv, privileges.AddPriv, privileges.ChangePriv, privileges.DelPriv);
                    _screenPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;

                    /*Hiding the checkboxes for particular screens -- kranthi 07-07-2022 */
                    LookupDataAccess.Hide_ScreenPrivilages_checkboxes(privileges, _screenPrivileges.GridViewControl, rowIndex);
                }

                List<PrivilegeEntity> userReports = _model.UserProfileAccess.GetUserPrivilegesByID(SelectedUser, "Reports", string.Empty);
                _reportPrivileges.GridViewControl.Rows.Clear();
                foreach (PrivilegeEntity privileges in userReports)
                {
                    string screenName = privileges.PrivilegeName;
                    int rowIndex = _reportPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.ViewPriv);
                    _reportPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;
                }
            }
        }

        List<CaseSiteEntity> Site_List = new List<CaseSiteEntity>();
        //private void FillSites()
        //{
        //    Site_List = _model.CaseMstData.GetCaseSite(_intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(0, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(3, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(6, 2), "SiteHie");
        //    ListcaseSiteEntity.Clear();
        //    foreach (CaseSiteEntity casesite in Site_List) //Site_List)//ListcaseSiteEntity)
        //    {
        //        //if (Txt_Sel_Site.Text.Contains(casesite.SiteNUMBER))
        //            ListcaseSiteEntity.Add(casesite);
        //        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
        //    }
        //}


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
            if (gvwComponents.Rows.Count == 0)
            {
                //picEdit.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
                picEdit.ImageSource = "captain-add";
            }
            else
            {
                //picEdit.Image = new IconResourceHandle(Consts.Icons16x16.EditIcon);
                picEdit.ImageSource = "captain-edit";
            }
        }

        private void setVisibleControls(bool flag)
        {
            txtUserID.Enabled = flag;
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string SelectedUser { get; set; }

        public bool IsSaveValid { get; set; }

        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        public List<HierarchyEntity> UserHierarchy
        {
            get
            {
                return _userHierarchy = (from c in _intakeHierarchy.GridViewControl.Rows.Cast<DataGridViewRow>().ToList()
                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }

        public bool GlobalPrivileges { get; set; }

        private void fillDropdowns()
        {
            fillEMS();
            fillSecurity();

            DataSet ds = Captain.DatabaseLayer.Lookups.GetStaff();
            DataTable dt = ds.Tables[0];
            DataView dv = dt.DefaultView;
            dv.Sort = "STF_Agency ASC";
            DataTable sortedDT = dv.ToTable();
            foreach (DataRow dr in sortedDT.Rows)
            {
                cmbStaff.Items.Add(new ListItem(dr["STF_Agency"].ToString() + " - " + dr["STF_NAME"].ToString(), dr["STF_CODE"].ToString()));
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
            cmbSite.Items.Insert(0, new ListItem("    ", "0"));
            cmbSite.Items.Insert(1, new ListItem("All Sites", "****"));
            cmbSite.SelectedIndex = 0;

            fillAddlPrivileges();
            cmbImageTypes.SetEditable(true);
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

            //cmbC1.Items.AddRange(listItem.ToArray());
            //cmbC2.Items.AddRange(listItem.ToArray());
            //cmbC3.Items.AddRange(listItem.ToArray());
            //cmbC4.Items.AddRange(listItem.ToArray());
            //cmbC5.Items.AddRange(listItem.ToArray());
            //cmbC6.Items.AddRange(listItem.ToArray());
            //cmbC7.Items.AddRange(listItem.ToArray());
            //cmbC8.Items.AddRange(listItem.ToArray());
            //cmbC9.Items.AddRange(listItem.ToArray());
            //cmbC1.SelectedIndex = 0;
            //cmbC2.SelectedIndex = 0;
            //cmbC3.SelectedIndex = 0;
            //cmbC4.SelectedIndex = 0;
            //cmbC5.SelectedIndex = 0;
            //cmbC6.SelectedIndex = 0;
            //cmbC7.SelectedIndex = 0;
            //cmbC8.SelectedIndex = 0;
            //cmbC9.SelectedIndex = 0;
        }

        private bool isUserExists(string userID)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                UserEntity userProfile = _model.UserProfileAccess.GetUserProfileByID(userID);
                if (userProfile != null)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        private bool isCaseworkerExists(string caseWorker)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                UserEntity userProfile = _model.UserProfileAccess.Checkcaseworker(caseWorker);
                if (userProfile != null)
                {
                    isExists = true;
                }
            }
            else
            {
                List<UserEntity> userProfile = _model.UserProfileAccess.CheckcaseworkerEdit(caseWorker);
                if (userProfile.Count > 0)
                {
                    userProfile = userProfile.FindAll(u => u.UserID != txtUserID.Text.Trim());
                    if (userProfile.Count > 0)
                    {
                        isExists = true;
                    }
                }

            }
            return isExists;
        }

        private bool ContainsWhitespace(string text)
        {
            return text.Contains(" ");
        }
        private bool ValidateForm()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(txtUserID.Text.Trim()))
            {
                _errorProvider.SetError(txtUserID, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblUserID.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (isUserExists(txtUserID.Text.Trim()))
                {
                    _errorProvider.SetError(txtUserID, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblUserID.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtUserID, null);
                }
            }

            //else if (ContainsWhitespace(txtUserID.Text.Trim()))
            //{
            //    _errorProvider.SetError(txtUserID, string.Format("Please avoid space in User ID", lblUserID.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}


            if (String.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                _errorProvider.SetError(txtFirstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblfname.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFirstName, null);
            }

            if (String.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                _errorProvider.SetError(txtLastName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lbllname.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLastName, null);
            }

            if (String.IsNullOrEmpty(txtCaseWorker.Text.Trim()))
            {
                _errorProvider.SetError(txtCaseWorker, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCaseWorker.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                if (isCaseworkerExists(txtCaseWorker.Text.Trim()))
                {
                    _errorProvider.SetError(txtCaseWorker, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblCaseWorker.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtCaseWorker, null);
                }


            }

            if (String.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                _errorProvider.SetError(txtEmail, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEmail.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtEmail, null);
            }

            if (((ListItem)cmbSecurity.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(cmbSecurity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSecurity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbSecurity, null);
            }

            if (cmbSecurity.SelectedItem == null || ((ListItem)cmbSecurity.SelectedItem).Text == Consts.Common.SelectOne)
            {
                _errorProvider.SetError(cmbSecurity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSecurity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(cmbSecurity, null);
            }
            if (String.IsNullOrEmpty(txtDefaultHierachy.Text.Trim()))
            {
                _errorProvider.SetError(PbHierarchies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDefaultHie.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {

                if (_intakeHierarchy.GridViewControl.Rows.Count > 0)
                {
                    bool intakeisValid = false;
                    // Hierachy
                    if (!intakeisValid)
                        foreach (DataGridViewRow row in _intakeHierarchy.GridViewControl.Rows)
                        {

                            HierarchyEntity hierarchyEntity = row.Tag as HierarchyEntity;
                            // if (!hierarchyEntity.UsedFlag.Equals("Y"))
                            if (hierarchyEntity != null)
                            {
                                string agency = hierarchyEntity.Agency.Equals(string.Empty) ? "**" : hierarchyEntity.Agency.ToString();
                                string dept = hierarchyEntity.Dept.Equals(string.Empty) ? "**" : hierarchyEntity.Dept.ToString();
                                string prog = hierarchyEntity.Prog.Equals(string.Empty) ? "**" : hierarchyEntity.Prog;
                                string strAgency = txtDefaultHierachy.Text.Substring(0, 2);
                                string strDetp = txtDefaultHierachy.Text.Substring(3, 2);
                                string strProgram = txtDefaultHierachy.Text.Substring(6, 2);
                                if ((agency + dept + prog == strAgency + strDetp + strProgram) && (hierarchyEntity.UsedFlag != "Y"))
                                {
                                    intakeisValid = true;
                                    break;
                                }
                                if ((agency + dept + prog == strAgency + strDetp + "**") && (hierarchyEntity.UsedFlag != "Y"))
                                {
                                    intakeisValid = true;
                                    break;
                                }
                                if ((agency + dept + prog == strAgency + "****") && (hierarchyEntity.UsedFlag != "Y"))
                                {
                                    intakeisValid = true;
                                    break;
                                }
                                if ((agency + dept + prog == "******") && (hierarchyEntity.UsedFlag != "Y"))
                                {
                                    intakeisValid = true;
                                    break;
                                }
                            }
                        }

                    if (intakeisValid)
                    {
                        _errorProvider.SetError(PbHierarchies, null);
                    }
                    else
                    {
                        _errorProvider.SetError(PbHierarchies, "Default Hierachy should be Intake hierachy");
                        isValid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(PbHierarchies, null);
                }
            }

            if (_intakeHierarchy.GridViewControl.Rows.Count > 0)
            {
                _errorProvider.SetError(_intakeHierarchy.olblRequired, null);
            }
            else
            {
                _errorProvider.SetError(_intakeHierarchy.olblRequired, "Intake Hierarchy Required");
                isValid = false;
            }



            if (txtEmail.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, Consts.StaticVars.EmailValidatingString))
                {
                    _errorProvider.SetError(txtEmail, Consts.Messages.PleaseEnterEmail);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtEmail, null);
                }
            }

            if (maskPhone.Text != "" && maskPhone.Text != "   -   -")
            {
                if (maskPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskPhone, "Please enter valid Mobile Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskPhone, null);
                }
            }

            IsSaveValid = isValid;
            return (isValid);
        }

        public void Refresh()
        {
            List<PrivilegeEntity> userPrivileges = _model.UserProfileAccess.GetUserPrivilegesByID(SelectedUser, "Screen", string.Empty);
            _screenPrivileges.GridViewControl.Rows.Clear();
            foreach (PrivilegeEntity privileges in userPrivileges)
            {
                string screenName = privileges.PrivilegeName;
                int rowIndex = _screenPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.Hierarchy, privileges.ViewPriv, privileges.AddPriv, privileges.ChangePriv, privileges.DelPriv);
                _screenPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;
            }
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

        private void SetImageTypes(string ImageTypes)
        {
            clstImageTypes.Items.Clear();
            List<string> imageTypeList = new List<string>();
            if (ImageTypes != null)
            {
                string[] imageTypes = ImageTypes.Split(' ');
                for (int i = 0; i < imageTypes.Length; i++)
                {
                    imageTypeList.Add(imageTypes.GetValue(i).ToString());
                }
            }
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

        /// <summary>
        /// Handles the TabControl tabs SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabUser.SelectedTab == null) return;
            string tabCode = tabUser.SelectedTab.Tag as string;
            if (tabUser.SelectedTab.Name.Equals("tabPageScreen"))
            {
                _screenPrivileges.SelectedHierarchy = UserHierarchy;
            }
            if (tabUser.SelectedTab.Name.Equals("tabPageAddlPrivileges"))
            {
                // cmbC1.Focus();
            }
            if (tabUser.SelectedTab.Name.Equals("tabPageServiceHierarchy"))
            {
                List<HierarchyEntity> intakeHierarchy = new List<Model.Objects.HierarchyEntity>();
                foreach (DataGridViewRow gvrowitem in _intakeHierarchy.GridViewControl.Rows)
                {
                    HierarchyEntity hieriekey = gvrowitem.Tag as HierarchyEntity;

                    //Kranthi:: 02/09/2023 :: Wisej misc 2-7-2023.docx:: wants to show inactive heirarchies also in intake grid
                    //if (hieriekey.UsedFlag != "Y")
                    //{

                    if (hieriekey.Dept.Trim() == "")
                        hieriekey.Dept = "**";
                    if (hieriekey.Prog.Trim() == "")
                        hieriekey.Prog = "**";

                    intakeHierarchy.Add(hieriekey);
                    //}
                }
                _servicePlanHierarchy.proppUserIntakeHierarchy = intakeHierarchy;
                if (Mode == "Add")
                {
                    _servicePlanHierarchy.RefreshGrid();
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanging event for the tab control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabControlSelectedIndexChanging(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == null) return;

            if (e.TabPage.Name.Equals("tabPageScreen") && (UserHierarchy == null || UserHierarchy.Count == 0))
            {
                MessageBox.Show("Please Select intake Hierarchy first, then set the privileges", "CAPTAIN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void rbtActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Click(object sender, EventArgs e)
        {

        }


        private void OnOkClick(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    //Add PASSWORD


                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@PWR_EMPLOYEE_NO", txtUserID.Text.ToString().TrimStart().Trim()));
                    sqlParamList.Add(new SqlParameter("@PWR_PASSWORD", txtUserID.Text));

                    sqlParamList.Add(new SqlParameter("@PWR_CASEWORKER", txtCaseWorker.Text));
                    sqlParamList.Add(new SqlParameter("@PWR_NAME_IX_FIRST", txtFirstName.Text));
                    sqlParamList.Add(new SqlParameter("@PWR_NAME_IX_LAST", txtLastName.Text));
                    sqlParamList.Add(new SqlParameter("@PWR_SECURITY", ((ListItem)cmbSecurity.SelectedItem).Value.ToString()));
                    if (cbAccessAll.Checked)
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_ACCESS_ALL", "Y"));
                    }
                    else
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_ACCESS_ALL", "N"));
                    }
                    if (cbForcePassword.Checked)
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_SUCCESSFUL", "0"));
                        sqlParamList.Add(new SqlParameter("@PWR_UNSUCCESSFUL", "1"));
                    }
                    else
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_SUCCESSFUL", "1"));
                        sqlParamList.Add(new SqlParameter("@PWR_UNSUCCESSFUL", "0"));
                    }
                    string imageTypes = string.Empty;
                    foreach (ListItem li in clstImageTypes.CheckedItems)
                    {
                        if (!imageTypes.Equals(string.Empty))
                        {
                            imageTypes += " ";
                        }
                        imageTypes += li.Value;
                    }
                    sqlParamList.Add(new SqlParameter("@PWR_IMAGE_TYPES", imageTypes));
                    if (cbTemplateUser.Checked)
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_TEMPLATE_USER", "Y"));
                    }
                    else
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_TEMPLATE_USER", "N"));
                    }

                    string status = chkbActive.Checked ? "N" : "Y";
                    sqlParamList.Add(new SqlParameter("@PWR_INACTIVE_FLAG", status));
                    if (!((ListItem)cmbStaff.SelectedItem).Value.ToString().Equals("0"))
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_STAFF_CODE", ((ListItem)cmbStaff.SelectedItem).Value.ToString()));
                    }

                    if (!((ListItem)cmbSite.SelectedItem).Value.ToString().Equals("0"))   // Yeswanth
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_SITE", ((ListItem)cmbSite.SelectedItem).Value.ToString()));
                    }
                    if (!((ListItem)cmbEMS.SelectedItem).Value.ToString().Equals("0"))
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_EMS_ACCESS", ((ListItem)cmbEMS.SelectedItem).Value.ToString()));
                    }
                    if (txtDefaultHierachy.Text != string.Empty)
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_DEF_AGENCY", txtDefaultHierachy.Text.Substring(0, 2)));
                        sqlParamList.Add(new SqlParameter("@PWR_DEF_DEPT", txtDefaultHierachy.Text.Substring(3, 2)));
                        sqlParamList.Add(new SqlParameter("@PWR_DEF_PROG", txtDefaultHierachy.Text.Substring(6, 2)));
                    }
                    string componentsTypes = string.Empty;
                    foreach (DataGridViewRow gvcomponents in gvwComponents.Rows)
                    {
                        if (!componentsTypes.Equals(string.Empty))
                        {
                            componentsTypes += " ";
                        }
                        componentsTypes += gvcomponents.Cells["cellCode"].Value;
                    }
                    if (componentsTypes != string.Empty)
                    {
                        sqlParamList.Add(new SqlParameter("@PWR_Components", componentsTypes));
                    }

                    sqlParamList.Add(new SqlParameter("@PWR_LSTC_OPERATOR", BaseForm.UserID));
                    sqlParamList.Add(new SqlParameter("@PWR_ADD_OPERATOR", BaseForm.UserID));
                    //sqlParamList.Add(new SqlParameter("@PWR_SEARCH_DATABASE", chkSearchDB.Checked == true ? "Y" : "N"));
                    sqlParamList.Add(new SqlParameter("@PWR_SEARCH_PIP", chkSearchPIP.Checked == true ? "N"/*Y*/ : "N")); // "Y" Kranthi 08/22/2024:: HCCAA.docx:: requested for want to hide in all systems
                    if (txtEmail.Text.Trim() != string.Empty)
                        sqlParamList.Add(new SqlParameter("@PWR_EMAIL", txtEmail.Text.Trim()));

                    if (dtpDob.Checked)
                        sqlParamList.Add(new SqlParameter("@PWR_DOB", dtpDob.Text.ToString()));

                    if (maskPhone.Text.Trim() != string.Empty)
                        sqlParamList.Add(new SqlParameter("@PWR_MOBILE", maskPhone.Text.Trim()));


                    if (Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORD(sqlParamList))
                    {
                        if (txtUserID.Text.ToString().TrimStart().Trim() == BaseForm.UserID.Trim())
                            BaseForm.BaseDefaultHIE = txtDefaultHierachy.Text.Substring(0, 2) + "-" + txtDefaultHierachy.Text.Substring(3, 2) + "-" + txtDefaultHierachy.Text.Substring(6, 2);

                        saveEMPLFUNC();
                        savePasswordHIE();
                        INSERTUPDATEDELTAKEACTIONSHLD(txtUserID.Text.ToString().Trim(), string.Empty, string.Empty, string.Empty, txtEmail.Text.Trim(), string.Empty, string.Empty, "UPDATE");

                        UserListControl userListControl = BaseForm.GetBaseUserControl() as UserListControl;
                        if (userListControl != null)
                        {
                            userListControl.RefreshGrid(txtUserID.Text.Trim());
                        }

                        BaseForm.isPrivSaveFlag = "Y";
                        AlertBox.Show("Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void savePasswordHIE()
        {
            //Add PASSWORDHIE
            string userID = txtUserID.Text.Trim();
            string type = string.Empty;
            string fname = txtFirstName.Text;
            string lname = txtLastName.Text;
            string caseWorker = txtCaseWorker.Text;
            string Security = ((ListItem)cmbSecurity.SelectedItem).Value.ToString();
            //if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
            //{
            if (_servicePlanHierarchy != null)
            {
                List<DataGridViewRow> SelGridRows = (from c in _servicePlanHierarchy.GridViewControl.Rows.Cast<DataGridViewRow>().ToList()
                                                     where (c.Cells[2].Value.ToString() != "" && c.Cells[3].Value.ToString() != "")
                                                     select c).ToList();

                List<string> lstServices = new List<string>();
                foreach (DataGridViewRow row in SelGridRows)
                {
                    HierarchyEntity hierarchyEntity = row.Tag as HierarchyEntity;
                    if (hierarchyEntity != null)
                    {
                        string agency = hierarchyEntity.Agency.Equals(string.Empty) ? "**" : hierarchyEntity.Agency.ToString();
                        string dept = hierarchyEntity.Dept.Equals(string.Empty) ? "**" : hierarchyEntity.Dept.ToString();
                        string prog = hierarchyEntity.Prog.Equals(string.Empty) ? "**" : hierarchyEntity.Prog;

                        string SearchCode = agency + "-" + dept + "-" + prog;

                        if (_intakeHierarchy != null)
                        {
                            foreach (DataGridViewRow Introw in _intakeHierarchy.GridViewControl.Rows)
                            {
                                HierarchyEntity inthierarchyEntity = Introw.Tag as HierarchyEntity;
                                {
                                    if (inthierarchyEntity != null)
                                    {
                                        if (inthierarchyEntity.Agency.ToString() == hierarchyEntity.SerAgency.ToString() && inthierarchyEntity.Dept.ToString() == hierarchyEntity.SerDept.ToString() && inthierarchyEntity.Prog.ToString() == hierarchyEntity.SerProg.ToString())
                                        {
                                            if (inthierarchyEntity.UsedFlag == "Y")
                                            {
                                                hierarchyEntity.UsedFlag = "Y";
                                                hierarchyEntity.InActiveFlag = "Y";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@PWSH_EMPLOYEE_NO", userID));
                        //sqlParamList.Add(new SqlParameter("@PWH_TYPE", "S"));
                        sqlParamList.Add(new SqlParameter("@PWSIH_AGENCY", hierarchyEntity.SerAgency));
                        sqlParamList.Add(new SqlParameter("@PWSIH_DEPT", hierarchyEntity.SerDept));
                        sqlParamList.Add(new SqlParameter("@PWSIH_PROG", hierarchyEntity.SerProg));
                        sqlParamList.Add(new SqlParameter("@PWSH_AGENCY", agency));
                        sqlParamList.Add(new SqlParameter("@PWSH_DEPT", dept));
                        sqlParamList.Add(new SqlParameter("@PWSH_PROG", prog));
                        sqlParamList.Add(new SqlParameter("@PWSH_LAST_NAME", lname));
                        sqlParamList.Add(new SqlParameter("@PWSH_FIRST_NAME", fname));
                        sqlParamList.Add(new SqlParameter("@PWSH_CASEWORKER", caseWorker));
                        sqlParamList.Add(new SqlParameter("@PWSH_SECURITY", Security));
                        if (hierarchyEntity.UsedFlag == "")
                        {
                            hierarchyEntity.UsedFlag = "N";
                        }
                        sqlParamList.Add(new SqlParameter("@PWSH_USED_FLAG", hierarchyEntity.UsedFlag));
                        if (hierarchyEntity.InActiveFlag == "false")
                        {
                            hierarchyEntity.InActiveFlag = "N";
                        }
                        else if (hierarchyEntity.InActiveFlag == "true")
                        {
                            hierarchyEntity.InActiveFlag = "Y";
                        }
                        //sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        sqlParamList.Add(new SqlParameter("@PWSH_INACTIVE", hierarchyEntity.InActiveFlag));
                        sqlParamList.Add(new SqlParameter("@PWSH_ADD_OPERATOR", BaseForm.UserID));
                        sqlParamList.Add(new SqlParameter("@PWSH_LSTC_OPERATOR", BaseForm.UserID));
                        Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORDSERHIE(sqlParamList);

                        //if (!lstServices.Contains(SearchCode))
                        //{
                        //    lstServices.Add(SearchCode);

                        //    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        //    sqlParamList.Add(new SqlParameter("@PWH_EMPLOYEE_NO", userID));
                        //    sqlParamList.Add(new SqlParameter("@PWH_TYPE", "S"));
                        //    sqlParamList.Add(new SqlParameter("@PWH_AGENCY", agency));
                        //    sqlParamList.Add(new SqlParameter("@PWH_DEPT", dept));
                        //    sqlParamList.Add(new SqlParameter("@PWH_PROG", prog));
                        //    sqlParamList.Add(new SqlParameter("@PWH_LAST_NAME", lname));
                        //    sqlParamList.Add(new SqlParameter("@PWH_FIRST_NAME", fname));
                        //    sqlParamList.Add(new SqlParameter("@PWH_CASEWORKER", caseWorker));
                        //    sqlParamList.Add(new SqlParameter("@PWH_SECURITY", Security));
                        //    //sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        //    // sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", "N"));
                        //    if (hierarchyEntity.UsedFlag == "")
                        //    {
                        //        hierarchyEntity.UsedFlag = "N";
                        //    }

                        //    //Added by Sudheer on 04/14/2023
                        //    if(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol=="Y")
                        //    {
                        //        if (_intakeHierarchy != null)
                        //        {
                        //            foreach (DataGridViewRow Introw in _intakeHierarchy.GridViewControl.Rows)
                        //            {
                        //                HierarchyEntity inthierarchyEntity = Introw.Tag as HierarchyEntity;
                        //                if(inthierarchyEntity!=null)
                        //                {
                        //                    if(inthierarchyEntity.UsedFlag=="N" || inthierarchyEntity.UsedFlag=="")
                        //                    {
                        //                        if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "D")
                        //                        {
                        //                            if (inthierarchyEntity.Agency != "**" && inthierarchyEntity.Dept != "**")
                        //                            {
                        //                                if (inthierarchyEntity.Agency.ToString() == hierarchyEntity.Agency.ToString() && inthierarchyEntity.Dept.ToString() == hierarchyEntity.Dept.ToString())
                        //                                { }
                        //                                else
                        //                                {
                        //                                    hierarchyEntity.UsedFlag = "Y";
                        //                                    hierarchyEntity.InActiveFlag = "Y";
                        //                                }
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            if (inthierarchyEntity.Agency != "**")
                        //                            {
                        //                                if (inthierarchyEntity.Agency.ToString() == hierarchyEntity.Agency.ToString())
                        //                                { }
                        //                                else
                        //                                {
                        //                                    hierarchyEntity.UsedFlag = "Y";
                        //                                    hierarchyEntity.InActiveFlag = "Y";
                        //                                }
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }



                        //    sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        //    if (hierarchyEntity.InActiveFlag == "false")
                        //    {
                        //        hierarchyEntity.InActiveFlag = "N";
                        //    }
                        //    else if (hierarchyEntity.InActiveFlag == "true")
                        //    {
                        //        hierarchyEntity.InActiveFlag = "Y";
                        //    }
                        //    //sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        //    sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", hierarchyEntity.InActiveFlag));
                        //    sqlParamList.Add(new SqlParameter("@PWH_ADD_OPERATOR", BaseForm.UserID));
                        //    sqlParamList.Add(new SqlParameter("@PWH_LSTC_OPERATOR", BaseForm.UserID));
                        //    Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORDHIE(sqlParamList);

                        //}
                    }
                }
            }
            //}
            if (_serviceHierarchy != null)
            {
                CLINQHIEEntity hierarchyEntity1 = new CLINQHIEEntity();
                List<SqlParameter> sqlParamList1 = new List<SqlParameter>();
                sqlParamList1.Add(new SqlParameter("@CLINQ_USER_ID", userID));
                sqlParamList1.Add(new SqlParameter("@CLINQ_AGENCY", string.Empty));
                sqlParamList1.Add(new SqlParameter("@CLINQ_DEPT", string.Empty));
                sqlParamList1.Add(new SqlParameter("@CLINQ_PROGRAM", string.Empty));
                sqlParamList1.Add(new SqlParameter("@CLINQ_PDF", string.Empty));
                sqlParamList1.Add(new SqlParameter("@CLINQ_CNOTES", string.Empty));
                sqlParamList1.Add(new SqlParameter("@CLINQ_USED_FLAG", "D"));
                sqlParamList1.Add(new SqlParameter("@CLINQ_ADD_OPERATOR", BaseForm.UserID));
                sqlParamList1.Add(new SqlParameter("@CLINQ_LSTC_OPERATOR", BaseForm.UserID));
                Captain.DatabaseLayer.UserAccess.InsertUpdateCLINQHIE(sqlParamList1);


                foreach (DataGridViewRow row in _serviceHierarchy.GridViewControl.Rows)
                {
                    CLINQHIEEntity hierarchyEntity = row.Tag as CLINQHIEEntity;
                    if (hierarchyEntity != null)
                    {
                        //string agency = hierarchyEntity.Agency.Equals(string.Empty) ? "**" : hierarchyEntity.Agency.ToString();
                        //string dept = hierarchyEntity.Dept.Equals(string.Empty) ? "**" : hierarchyEntity.Dept.ToString();
                        //string prog = hierarchyEntity.Prog.Equals(string.Empty) ? "**" : hierarchyEntity.Prog;
                        string agency = hierarchyEntity.Agency.ToString();
                        string dept = hierarchyEntity.Dept.ToString();
                        string prog = hierarchyEntity.Prog;

                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@CLINQ_USER_ID", userID));
                        //sqlParamList.Add(new SqlParameter("@PWH_TYPE", "S"));
                        sqlParamList.Add(new SqlParameter("@CLINQ_AGENCY", agency));
                        sqlParamList.Add(new SqlParameter("@CLINQ_DEPT", dept));
                        sqlParamList.Add(new SqlParameter("@CLINQ_PROGRAM", prog));
                        sqlParamList.Add(new SqlParameter("@CLINQ_PDF", hierarchyEntity.CLINQPdf));
                        sqlParamList.Add(new SqlParameter("@CLINQ_CNOTES", hierarchyEntity.CLINQCNotes));
                        //sqlParamList.Add(new SqlParameter("@PWH_CASEWORKER", caseWorker));
                        //sqlParamList.Add(new SqlParameter("@PWH_SECURITY", Security));
                        ////sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        //// sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", "N"));
                        if (hierarchyEntity.UsedFlag == "")
                        {
                            hierarchyEntity.UsedFlag = "N";
                        }
                        sqlParamList.Add(new SqlParameter("@CLINQ_USED_FLAG", hierarchyEntity.UsedFlag));
                        //if (hierarchyEntity.InActiveFlag == "false")
                        //{
                        //    hierarchyEntity.InActiveFlag = "N";
                        //}
                        //else if (hierarchyEntity.InActiveFlag == "true")
                        //{
                        //    hierarchyEntity.InActiveFlag = "Y";
                        //}
                        //sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        //sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", hierarchyEntity.InActiveFlag));
                        sqlParamList.Add(new SqlParameter("@CLINQ_ADD_OPERATOR", BaseForm.UserID));
                        sqlParamList.Add(new SqlParameter("@CLINQ_LSTC_OPERATOR", BaseForm.UserID));
                        Captain.DatabaseLayer.UserAccess.InsertUpdateCLINQHIE(sqlParamList);
                    }
                }
            }


            if (_intakeHierarchy != null)
            {
                foreach (DataGridViewRow row in _intakeHierarchy.GridViewControl.Rows)
                {
                    HierarchyEntity hierarchyEntity = row.Tag as HierarchyEntity;
                    // if (!hierarchyEntity.UsedFlag.Equals("Y"))
                    if (hierarchyEntity != null)
                    {
                        string agency = hierarchyEntity.Agency.Equals(string.Empty) ? "**" : hierarchyEntity.Agency.ToString();
                        string dept = hierarchyEntity.Dept.Equals(string.Empty) ? "**" : hierarchyEntity.Dept.ToString();
                        string prog = hierarchyEntity.Prog.Equals(string.Empty) ? "**" : hierarchyEntity.Prog;

                        string Sites = string.Empty;

                        if (row.Cells["cellSites"].Value != null)
                        {
                            if (!string.IsNullOrEmpty(row.Cells["cellSites"].Value.ToString().Trim()))
                                Sites = row.Cells["cellSites"].Value.ToString();
                        }
                        //if (ListcaseSiteEntity.Count > 0)
                        //{
                        //    foreach (CaseSiteEntity Entity in ListcaseSiteEntity)
                        //    {
                        //        if (!string.IsNullOrEmpty(Entity.SiteNUMBER.Trim()))
                        //            Sites = Entity.SiteNUMBER.Trim() + ",";
                        //    }
                        //}



                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@PWH_EMPLOYEE_NO", userID));
                        sqlParamList.Add(new SqlParameter("@PWH_TYPE", "I"));
                        sqlParamList.Add(new SqlParameter("@PWH_AGENCY", agency));
                        sqlParamList.Add(new SqlParameter("@PWH_DEPT", dept));
                        sqlParamList.Add(new SqlParameter("@PWH_PROG", prog));
                        sqlParamList.Add(new SqlParameter("@PWH_LAST_NAME", lname));
                        sqlParamList.Add(new SqlParameter("@PWH_FIRST_NAME", fname));
                        sqlParamList.Add(new SqlParameter("@PWH_CASEWORKER", caseWorker));
                        sqlParamList.Add(new SqlParameter("@PWH_SECURITY", Security));
                        if (hierarchyEntity.UsedFlag == "")
                        {
                            hierarchyEntity.UsedFlag = "N";
                        }
                        sqlParamList.Add(new SqlParameter("@PWH_USED_FLAG", hierarchyEntity.UsedFlag));
                        if (hierarchyEntity.InActiveFlag == "false")
                        {
                            hierarchyEntity.InActiveFlag = "N";
                        }
                        else if (hierarchyEntity.InActiveFlag == "true")
                        {
                            hierarchyEntity.InActiveFlag = "Y";
                        }
                        else if (hierarchyEntity.InActiveFlag == "")
                        {
                            hierarchyEntity.InActiveFlag = "N";
                        }

                        sqlParamList.Add(new SqlParameter("@PWH_INACTIVE", hierarchyEntity.InActiveFlag));
                        sqlParamList.Add(new SqlParameter("@PWH_ADD_OPERATOR", BaseForm.UserID));
                        sqlParamList.Add(new SqlParameter("@PWH_LSTC_OPERATOR", BaseForm.UserID));
                        sqlParamList.Add(new SqlParameter("@PWH_SITES", Sites));
                        Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORDHIE(sqlParamList);
                    }
                }
            }
        }

        private void saveEMPLFUNC()
        {
            //Add EMPLFUNC
            string userID = txtUserID.Text.Trim();
            string type = string.Empty;
            string fname = txtFirstName.Text;
            string lname = txtLastName.Text;
            string caseWorker = txtCaseWorker.Text;
            string Security = ((ListItem)cmbSecurity.SelectedItem).Value.ToString();

            if (_screenPrivileges != null)
            {
                foreach (DataGridViewRow row in _screenPrivileges.GridViewControl.Rows)
                {
                    PrivilegeEntity privilegeEntity = row.Tag as PrivilegeEntity;
                    string hierarchy = row.Cells["Hierarchy"].Value.ToString();
                    hierarchy = hierarchy.Replace("-", string.Empty);
                    string moduleCode = privilegeEntity.ModuleCode;
                    string progNo = privilegeEntity.Program;
                    string desc = privilegeEntity.PrivilegeName;
                    string viewPriv = "N";
                    string addPriv = "N";
                    string delPriv = "N";
                    string changePriv = "N";
                    if (row.Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        viewPriv = "Y";
                    }
                    if (row.Cells["Add"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        addPriv = "Y";
                    }
                    if (row.Cells["Delete"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        delPriv = "Y";
                    }
                    if (row.Cells["Change"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        changePriv = "Y";
                    }

                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@EFR_EMPLOYEE_NO", userID));
                    sqlParamList.Add(new SqlParameter("@EFR_MODULE_CODE", moduleCode));
                    //sqlParamList.Add(new SqlParameter("@EFR_ALT_EMPLOYEE_NO", userID));
                    //sqlParamList.Add(new SqlParameter("@EFR_ALT_MODULE_CODE", moduleCode));
                    sqlParamList.Add(new SqlParameter("@EFR_PROGNO", progNo));
                    sqlParamList.Add(new SqlParameter("@EFR_HIERARCHY", hierarchy));
                    sqlParamList.Add(new SqlParameter("@EFR_ADD_PRIV", addPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_CHG_PRIV", changePriv));
                    sqlParamList.Add(new SqlParameter("@EFR_DEL_PRIV", delPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_INQ_PRIV", viewPriv));
                    sqlParamList.Add(new SqlParameter("@EFR_DESCRIPTION", desc));
                    sqlParamList.Add(new SqlParameter("@EFR_ADD_OPERATOR", BaseForm.UserID));
                    sqlParamList.Add(new SqlParameter("@EFR_LSTC_OPERATOR", BaseForm.UserID));
                    Captain.DatabaseLayer.UserAccess.InsertUpdateEMPLFUNC(sqlParamList);
                }
            }

            if (_reportPrivileges != null)
            {
                foreach (DataGridViewRow row in _reportPrivileges.GridViewControl.Rows)
                {
                    PrivilegeEntity reportPrivileges = row.Tag as PrivilegeEntity;
                    //string hierarchy = row.Cells["Hierarchy"].Value.ToString();
                    string moduleCode = reportPrivileges.ModuleCode;
                    string progNo = reportPrivileges.Program;
                    string desc = reportPrivileges.PrivilegeName;
                    string viewPriv = "N";
                    if (row.Cells["View"].Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                    {
                        viewPriv = "Y";
                    }

                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@BAT_EMPLOYEE_NO", userID));
                    sqlParamList.Add(new SqlParameter("@BAT_MODULE_CODE", moduleCode));
                    sqlParamList.Add(new SqlParameter("@BAT_REPORT_CODE", progNo));
                    sqlParamList.Add(new SqlParameter("@BAT_REPORT_NAME", desc));
                    sqlParamList.Add(new SqlParameter("@BAT_VIEW_PRIV", viewPriv));
                    sqlParamList.Add(new SqlParameter("@BAT_ADD_OPERATOR", BaseForm.UserID));
                    sqlParamList.Add(new SqlParameter("@BAT_LSTC_OPERATOR", BaseForm.UserID));
                    Captain.DatabaseLayer.UserAccess.InsertUpdateBATCNTL(sqlParamList);
                }
            }
        }

        public bool INSERTUPDATEDELTAKEACTIONSHLD(string Userid, string Fname, string Lname, string Agency, string Email, string Levels, string stroperator, string Mode)
        {
            // UserEntity userProfile = null;
            string strquesid = string.Empty;
            bool strmsg = true;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (Userid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_EMPLOYEE_NO", Userid));
                if (Fname != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_FNAME", Fname));
                if (Lname != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_LNAME", Lname));
                if (Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_AGENCY", Agency));
                if (Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_EMAIL", Email));
                if (Levels != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_LEVELS", Levels));
                if (stroperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_ADD_OPERATOR", stroperator));
                if (stroperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@TAK_LSTC_OPERATOR", stroperator));
                if (Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", Mode));

                strmsg = Captain.DatabaseLayer.UserAccess.INSERTUPDATEDELTAKEACTIONSHLD(sqlParamList);
            }
            catch (Exception ex)
            {

                strmsg = false;
            }
            return strmsg;
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //  Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN00005_New");
        }

        private void OnCopyFromButtonClick(object sender, EventArgs e)
        {
            PrivilegeSelectionForm privilegeSelectionForm = new PrivilegeSelectionForm(BaseForm, Privileges);
            privilegeSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            privilegeSelectionForm.FormClosed += new FormClosedEventHandler(OnPrivilegeSelectionFormClosed);
            privilegeSelectionForm.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrivilegeSelectionFormClosed(object sender, FormClosedEventArgs e)
        {
            PrivilegeSelectionForm form = sender as PrivilegeSelectionForm;

            if (form.DialogResult == DialogResult.OK)
            {
                GlobalPrivileges = true;
                if (form.SelectedUser.Count > 0)
                {
                    UserEntity userEntity = form.SelectedUser[0];
                    if (userEntity == null) return;



                    UserEntity userProfile = _model.UserProfileAccess.GetUserProfileByID(userEntity.UserID);
                    SetComboBoxValue(cmbEMS, userProfile.EMS_Access);
                    SetComboBoxValue(cmbSecurity, userProfile.Security);
                    SetComboBoxValue(cmbSite, userProfile.Site);
                    SetComboBoxValue(cmbStaff, userProfile.StaffCode);
                    SetImageTypes(userProfile.ImageTypes);
                    chkbActive.Checked = true;

                    if (userProfile.AccessAll.Equals("Y"))
                    {
                        cbAccessAll.Checked = true;
                    }
                    else
                    {
                        cbAccessAll.Checked = false;
                    }
                    cbForcePassword.Checked = true;

                    //if (userProfile.PWDSearchDatabase.Equals("Y"))
                    //{ chkSearchDB.Checked = true; }
                    //else
                    //{
                    //    chkSearchDB.Checked = false;
                    //}

                    if (userProfile.PWDSearchPIP.Equals("Y"))
                    { chkSearchPIP.Checked = false; } // true; Kranthi 08/22/2024:: HCCAA.docx:: requested for want to hide in all systems
                    else
                    {
                        chkSearchPIP.Checked = false;
                    }


                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(userEntity.UserID);
                    List<HierarchyEntity> userServiceHierarchy = _model.UserProfileAccess.GetUserServiceHierarchyByID(userEntity.UserID);
                    List<CLINQHIEEntity> ClientHierarchy = _model.UserProfileAccess.GetClentInqByID(userEntity.UserID);
                    _intakeHierarchy.GridViewControl.Rows.Clear();
                    _serviceHierarchy.GridViewControl.Rows.Clear();
                    _servicePlanHierarchy.GridViewControl.Rows.Clear();
                    _screenPrivileges.GridViewControl.Rows.Clear();
                    userHierarchy = userHierarchy.FindAll(u => !u.UsedFlag.Equals("Y")).ToList();

                    fillComponents(userProfile.Components);
                    List<HierarchyEntity> intakeHierarchy = new List<Model.Objects.HierarchyEntity>();

                    foreach (HierarchyEntity hierarchy in userHierarchy)
                    {
                        string code = hierarchy.Agency + "-" + hierarchy.Dept + "-" + hierarchy.Prog;
                        string hierarchyName = hierarchy.HirarchyName.ToString();
                        if (hierarchy.HirarchyType.Equals("I"))
                        {
                            //int rowIndex = _intakeHierarchy.GridViewControl.Rows.Add(code, hierarchyName, hierarchy.Sites);
                            //_intakeHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                            //intakeHierarchy.Add(hierarchy);
                            List<PrivilegeEntity> userPrivileges = _model.UserProfileAccess.GetUserPrivilegesByID(userEntity.UserID, "Screen", hierarchy.Agency + hierarchy.Dept + hierarchy.Prog);
                            userPrivileges = userPrivileges.OrderBy(u => u.ModuleCode).ThenBy(u => u.Hierarchy).ThenBy(u => u.PrivilegeName).ToList();
                            foreach (PrivilegeEntity privileges in userPrivileges)
                            {
                                string screenName = privileges.PrivilegeName;
                                int rowIndex1 = _screenPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.Hierarchy, privileges.ViewPriv, privileges.AddPriv, privileges.ChangePriv, privileges.DelPriv);
                                _screenPrivileges.GridViewControl.Rows[rowIndex1].Tag = privileges;
                            }
                        }
                        else
                        {
                            //int rowIndex = _servicePlanHierarchy.GridViewControl.Rows.Add(code, hierarchyName);
                            // _servicePlanHierarchy.GridViewControl.Rows[rowIndex].Tag = hierarchy;
                        }
                    }

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
                                else
                                {
                                    intakeHierarchy.Add(hierarchy);
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

                        List<HierarchyEntity> oSPHierachies = new List<HierarchyEntity>();
                        if (oSPHierarchieslst.Count > 0)
                            oSPHierachies = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();

                        _servicePlanHierarchy.GridViewControl.Rows.Clear();

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
                            //    List<HierarchyEntity> ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);
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
                            //    List<HierarchyEntity> ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && x.Prog == hierarchy.Prog);
                            //    if (ofilterSerHIE.Count > 0)
                            //    {
                            //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerHIE, _servicePlanHierarchy.GridViewControl);
                            //    }
                            //}

                        }
                    }
                    _servicePlanHierarchy.proppUserIntakeHierarchy = intakeHierarchy;

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


                    List<PrivilegeEntity> userReports = _model.UserProfileAccess.GetUserPrivilegesByID(userEntity.UserID, "Reports", string.Empty);
                    _reportPrivileges.GridViewControl.Rows.Clear();
                    foreach (PrivilegeEntity privileges in userReports)
                    {
                        string screenName = privileges.PrivilegeName;
                        int rowIndex = _reportPrivileges.GridViewControl.Rows.Add(privileges.ModuleName, screenName, privileges.ViewPriv);
                        _reportPrivileges.GridViewControl.Rows[rowIndex].Tag = privileges;
                    }



                }
            }
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {

            if (Mode.Equals("Edit"))
                txtFirstName.Focus();
            else
                txtUserID.Focus();
        }

        private void txtCaseWorker_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
            //txtCaseWorker.Text = e.KeyChar
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            string strHierarchy = Convert.ToString(txtDefaultHierachy.Text);
            string UserID = BaseForm.UserID;
            string strFilter = "A";
            if (Mode.Equals("Edit"))
            {
                UserID = txtUserID.Text.Trim();
                strFilter = "D";
            }

            List<HierarchyEntity> SelIntHierarchies = new List<HierarchyEntity>();
            if (_intakeHierarchy != null)
            {
                foreach (DataGridViewRow Introw in _intakeHierarchy.GridViewControl.Rows)
                {
                    HierarchyEntity inthierarchyEntity = Introw.Tag as HierarchyEntity;
                    if (inthierarchyEntity.UsedFlag != "Y")
                    {
                        if (inthierarchyEntity.Agency != "**" && inthierarchyEntity.Dept != "**" && inthierarchyEntity.Prog != "**")
                            SelIntHierarchies.Add(inthierarchyEntity);
                    }

                }
            }

            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, strHierarchy, "Master", string.Empty, "A", "I");
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, strHierarchy, "Master", string.Empty, strFilter, "I", UserID, SelIntHierarchies);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnDefaultHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();

        }
        private void OnDefaultHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            string strPublicCode = string.Empty;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    strPublicCode = row.Code;
                    hierarchy += row.Agency + row.Dept + row.Prog;
                }
                txtDefaultHierachy.Text = strPublicCode;
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure want reset password", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: MessageBoxHandler);
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Wisej.Web.Form object that called MessageBox
            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            //Wisej.Web.Form senderForm = (Wisej.Web.Form)sender;

            // if (senderForm != null)
            // {
            // Set DialogResult value of the Form as a text for label
            if (dialogResult.ToString() == "Yes")
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PWR_EMPLOYEE_NO", txtUserID.Text.ToString().TrimStart().Trim()));
                sqlParamList.Add(new SqlParameter("@PWRTYPE", "RESETPASSWORD"));
                bool chkFlag = Captain.DatabaseLayer.UserAccess.InsertUpdatePASSWORD(sqlParamList);
                if (chkFlag)
                    AlertBox.Show("Password has been reset Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                else
                    AlertBox.Show("Failed to reset the password", MessageBoxIcon.Error, null, ContentAlignment.BottomRight);

            }
            // }
        }

        private void picEdit_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedListItem);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnComponentsFormClosed);
            hierarchieSelectionForm.ShowDialog();
        }

        public List<ListItem> SelectedListItem
        {
            get
            {
                _selectedComponets = new List<ListItem>();
                foreach (DataGridViewRow row in gvwComponents.Rows)
                {
                    ListItem listComponent = row.Tag as ListItem;
                    _selectedComponets.Add(listComponent);
                }
                return _selectedComponets;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnComponentsFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<ListItem> listCompontes = form.SelectedListItems;

                List<ListItem> usedHierarchies = (from c in gvwComponents.Rows.Cast<DataGridViewRow>().ToList()
                                                  select ((DataGridViewRow)c).Tag as ListItem).ToList();
                gvwComponents.Rows.Clear();
                foreach (ListItem row in listCompontes)
                {
                    int rowIndex = gvwComponents.Rows.Add(row.Value, row.Text.ToString());
                    gvwComponents.Rows[rowIndex].Tag = row;
                }
                if (gvwComponents.Rows.Count == 0)
                {
                    // picEdit.Image = new IconResourceHandle(Consts.Icons16x16.AddItem);
                    picEdit.ImageSource = "captain-add";
                }
                else
                {
                    //picEdit.Image = new IconResourceHandle(Consts.Icons16x16.EditIcon);
                    picEdit.ImageSource = "captain-edit";
                }
                //selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                //foreach (HierarchyEntity row in selectedHierarchies)
                //{
                //    HierarchyEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                //    if (hieEntity == null)
                //    {
                //        int rowIndex = gvwControl.Rows.Add(row.Code, row.HirarchyName.ToString());
                //        gvwControl.Rows[rowIndex].Tag = row;
                //    }
                //}
                //RefreshGrid();
            }
        }

        //private void GridViewControl_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseSiteEntity, _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(0, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(3, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(6, 2), string.Empty);
        //        siteform.FormClosed += new Form.FormClosedEventHandler(SelectZipSiteCountyFormClosed);
        //        siteform.ShowDialog();
        //    }
        //}

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {

            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                //if (form.FormType == "CASESITE")
                //{
                ListcaseSiteEntity = form.SelectedCaseSiteEntity;
                //if (Rb_Site_Sel.Checked == true && ListcaseSiteEntity.Count > 0)
                //    Txt_Sel_Site.Text = ListcaseSiteEntity[0].SiteNUMBER.ToString();
                //else
                //    Txt_Sel_Site.Clear();
                //}


                //ZipCodeEntity zipcodedetais = form.ListOfSelectedCaseSite;
                //if (zipcodedetais != null)
                //{
                //    string zipPlus = zipcodedetais.Zcrplus4;
                //    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                //    txtZipCode.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                //    txtState.Text = zipcodedetais.Zcrstate;
                //    txtCity.Text = zipcodedetais.Zcrcity;
                //    SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                //    SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                //}
            }
        }

        private void pnlSecurity_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void AddUserForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "TL_HELP")
            {
                Application.Navigate(CommonFunctions.CreateZenHelps(_screenPrivileges.PrivelegeEntity.Program, 1, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
            }
        }
    }
}