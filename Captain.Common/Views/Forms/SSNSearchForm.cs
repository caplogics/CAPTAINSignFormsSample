#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


using Wisej.Web;
using Wisej.Design;
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
using DevExpress.CodeParser;
using System.Web.UI.WebControls.Expressions;
using Microsoft.IdentityModel.Tokens;
using Syncfusion.Drawing;
using DevExpress.Utils.Drawing.Helpers;
using Aspose.Cells.Drawing;

#endregion

namespace Captain.Common.Views.Forms
{


    public partial class SSNSearchForm : Form
    {
        #region private variables
        private CaptainModel _model = null;
        private string strNameFormat = string.Empty;
        private ErrorProvider _errorProvider = null;
        #endregion

        public SSNSearchForm()
        {
            _model = new CaptainModel();
            InitializeComponent();
            propFilterAllOrApp = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.IconPadding = 2;
            propScreenName = string.Empty;
            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;
        }

        public List<CaseSnpEntity> CaseSnpAllList { get; set; }
        public ProgramDefinitionEntity ProgramDefinition { get; set; }
        public CaseMstEntity CaseMST { get; set; }
        #region properties
        public PrivilegeEntity Privileges { get; set; }
        public BaseForm BaseForm { get; set; }
        public string propFilterType { get; set; }
        public string propFilterAllOrApp { get; set; }
        public string propScreenName { get; set; }
        #endregion


        public string strPageType = string.Empty;
        public SSNSearchForm(BaseForm baseForm, PrivilegeEntity Privileges, List<CaseMstSnpEntity> snplist, string strFirstName, string strLastName, string strDOB)
        {
            _model = new CaptainModel();
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            propScreenName = string.Empty;
            BaseForm = baseForm;
            cbduplicates.Visible = false;
            propFilterType = "H";
            propFilterAllOrApp = "ALL";
            strNameFormat = baseForm.BaseHierarchyCnFormat;
            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false; btnSSNSelect.Visible = false;
            if (Privileges.Program == "APPT0002")
            {
                propScreenName = Privileges.Program;
                //lblDOB.Location = new Point(6, 5);
                //lblSSN.Location = new Point(344, 56);
                //mskSSNNO.Location = new Point(399, 52);
                //dtBirth.Location = new Point(55, 5);
                //DOBReq.Location = new Point(1, 5);
                //lblssnreq.Location = new Point(332, 56);
                this.Text = Privileges.Program + " - Search";
                this.Text = "Search";
                txtLastName.Text = strLastName;
                txtFirstName.Text = strFirstName;
                SsnNo.HeaderText = "DOB";
                if (strDOB != string.Empty)
                {
                    dtBirth.Value = Convert.ToDateTime(strDOB);
                    dtBirth.Checked = true;
                }

                if (snplist != null)
                {
                    if (snplist.Count > 0)
                    {
                        gvwSSNSearch.SelectionChanged -= gvwSSNSearch_SelectionChanged;
                        gvwSSNSearch.Rows.Clear();

                        string TmpHierarchy = ""; string TmpYear = "";
                        foreach (CaseMstSnpEntity CaseSnp in snplist)
                        {
                            TmpHierarchy = CaseSnp.Agency.ToString() + '-' + CaseSnp.Dept.ToString() + '-' + CaseSnp.Program.ToString();
                            TmpYear = "    ";
                            if (!string.IsNullOrEmpty(CaseSnp.Year.ToString().Trim()))
                                TmpYear = CaseSnp.Year.ToString();
                            string ApplicantName = LookupDataAccess.GetMemberName(CaseSnp.NameixFi, CaseSnp.NameixMi, CaseSnp.NameixLast, strNameFormat);

                            //if (IsDup.Equals("N"))
                            //{
                            //    if (listSSN.Exists(u => u.Equals(CaseSnp.Ssno.ToString() + ApplicantName))) continue;
                            //}


                            // listSSN.Add(CaseSnp.Ssno.ToString() + ApplicantName);
                            string Address = CaseSnp.Hn.Trim();
                            if (!CaseSnp.Street.Equals(string.Empty))
                            {
                                if (!Address.Equals(string.Empty)) { Address += " "; }
                                Address += CaseSnp.Street.Trim();
                            }
                            if (!CaseSnp.City.Equals(string.Empty))
                            {
                                if (!Address.Equals(string.Empty)) { Address += ", "; }
                                Address += CaseSnp.City.Trim();
                            }
                            if (!CaseSnp.State.Equals(string.Empty))
                            {
                                if (!Address.Equals(string.Empty)) { Address += ", "; }
                                Address += CaseSnp.State.Trim();
                            }
                            // string phone = "   ".Substring(0, 3 - CaseSnp.Area.Length) + CaseSnp.Area + CaseSnp.Phone + "       ".Substring(0, 7 - CaseSnp.Phone.Length);

                            string Homephone = "";
                            if (CaseSnp.Phone != "")
                            {
                                if (CaseSnp.Phone.Length == 7)
                                {
                                    try
                                    {
                                        Homephone = CaseSnp.Area + "-" + CaseSnp.Phone.Substring(0, 3) + "-" + CaseSnp.Phone.Substring(3, (CaseSnp.Phone.Length - 3));//"   ".Substring(0, 3 - CaseSnp.Area.Length) + CaseSnp.Area + CaseSnp.Phone + "       ".Substring(0, 7 - CaseSnp.Phone.Length);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            string MobileNo = "";
                            if (CaseSnp.Mst_Cell_Phone != "")
                                if (CaseSnp.Mst_Cell_Phone.Length >= 10)
                                    MobileNo = CaseSnp.Mst_Cell_Phone.Substring(0, 3) + "-" + CaseSnp.Mst_Cell_Phone.Substring(2, 3) + "-" + CaseSnp.Mst_Cell_Phone.Substring(6, 4);

                            //int rowIndex = gvwSSNSearch.Rows.Add(LookupDataAccess.Getdate(CaseSnp.AltBdate), ApplicantName, phone, Address);
                            int rowIndex = gvwSSNSearch.Rows.Add(TmpHierarchy, TmpYear, CaseSnp.ApplNo, LookupDataAccess.Getdate(CaseSnp.AltBdate), CaseSnp.NameixFi, CaseSnp.NameixLast,
                           MobileNo, Homephone, Address, CaseSnp.Agency.ToString() + CaseSnp.Dept.ToString() + CaseSnp.Program.ToString(), CaseSnp.Mst_Intake_Worker);

                            gvwSSNSearch.Rows[rowIndex].Tag = CaseSnp;
                            if (CaseSnp.SnpActive.Equals("I"))
                                gvwSSNSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;



                        }
                        gvwSSNSearch.SelectionChanged += gvwSSNSearch_SelectionChanged;
                        if (gvwSSNSearch.Rows.Count > 0)
                        {
                            gvwSSNSearch_SelectionChanged(gvwSSNSearch, new EventArgs());
                            btnSSNSelect.Visible = true;
                        }

                    }
                }
            }
            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg);
            //if (HierarchyEntity != null)
            //{
            //    strNameFormat = HierarchyEntity.CNFormat.ToString();
            //}
            // strNameFormat = baseForm.BaseHierarchyCnFormat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseForm"></param>
        /// <param name="strPagetype">Pass the Programme Name</param>
        /// <param name="caseSnpEntitylist"></param>
        /// <param name="ProgramEntity"></param>
        /// <param name="casemst"></param>
        /// <param name="strFilterType">Must Pass H or A </param>
        /// where H is hierachy based filter or A is All hierachy based filter        
        /// <param name="FilterAllORApp">Must Pass APP or ALL or MEM </param>
        public SSNSearchForm(BaseForm baseForm, string strPagetype, List<CaseSnpEntity> caseSnpEntitylist, ProgramDefinitionEntity ProgramEntity, CaseMstEntity casemst, string strFilterType, string strFilterAllORApp)
        {
            _model = new CaptainModel();
            InitializeComponent();
            strPageType = strPagetype;
            CaseSnpAllList = caseSnpEntitylist;
            ProgramDefinition = ProgramEntity;
            CaseMST = casemst;
            propScreenName = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            propFilterType = strFilterType;
            propFilterAllOrApp = strFilterAllORApp;
            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;
            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg);
            //if (HierarchyEntity != null)
            //{
            //    strNameFormat = HierarchyEntity.CNFormat.ToString();
            //}
            strNameFormat = baseForm.BaseHierarchyCnFormat;
            cbduplicates.Visible = false;
            //jen is asked Murali is Hided cbduplicates checkbox on 03/08/2021
            //if (propFilterType == "H")
            //    cbduplicates.Visible = false;
            //else
            //    cbduplicates.Visible = true;

            mskSSNNO.Focus();
        }

        public SSNSearchForm(BaseForm baseForm, string strPagetype, List<CaseSnpEntity> caseSnpEntitylist, ProgramDefinitionEntity ProgramEntity, CaseMstEntity casemst, string strFilterType, string strFilterAllORApp, PrivilegeEntity privilege)
        {
            _model = new CaptainModel();
            InitializeComponent();
            strPageType = strPagetype;
            CaseSnpAllList = caseSnpEntitylist;
            ProgramDefinition = ProgramEntity;
            CaseMST = casemst;
            propScreenName = string.Empty;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm; Privileges = privilege;
            propFilterType = strFilterType;
            propFilterAllOrApp = strFilterAllORApp;
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;
            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg);
            //if (HierarchyEntity != null)
            //{
            //    strNameFormat = HierarchyEntity.CNFormat.ToString();
            //}
            strNameFormat = baseForm.BaseHierarchyCnFormat;
            cbduplicates.Visible = false;
            //jen is asked Murali is Hided cbduplicates checkbox on 03/08/2021
            //if (propFilterType == "H")
            //    cbduplicates.Visible = false;
            //else
            //    cbduplicates.Visible = true;
            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
            mskSSNNO.Focus();

            //if(strPageType== "Case2001")
            //{
            //    this.CloseBox = false;
            //}
        }

        string _CallFromForm = "";
        public SSNSearchForm(BaseForm baseForm, string strSsnSearch, string strType)
        {
            _model = new CaptainModel();
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            propScreenName = string.Empty;

            _CallFromForm = strSsnSearch;

            propFilterType = strType;
            propFilterAllOrApp = "App";
            //HierarchyEntity HierarchyEntity = CommonFunctions.GetHierachyNameFormat(baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg);
            //if (HierarchyEntity != null)
            //{
            strNameFormat = baseForm.BaseHierarchyCnFormat; //HierarchyEntity.CNFormat.ToString();
                                                            // }
                                                            // Jen is asked Murali is Hided cbduplicates checkbox on 03/08/2021
            cbduplicates.Visible = false;
            //if (propFilterType == "H")
            //    cbduplicates.Visible = false;
            //else
            //    cbduplicates.Visible = true;
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;
            Worker_List = _model.lookupDataAccess.GetAllCaseworker();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblFooter.Text = string.Empty;
            Lbl_DOB.Text = lblAppDate2.Text = Lbl_Worker.Text = Lbl_program.Text = "";
            Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = false;
            if (ValidateForm())
            {

                gvwSSNSearch.SelectionChanged -= gvwSSNSearch_SelectionChanged;
                gvwSSNSearch.Rows.Clear();
                string SSNNO = mskSSNNO.Text.ToString();
                string FirstName = txtFirstName.Text.ToString();
                string LastName = txtLastName.Text.ToString();
                string Alias = txtAlias.Text.ToString();
                string TelePhone = mskTelePhone.Text.ToString();
                string HouseNo = txtHouseNo.Text.ToString();
                string Street = txtStreet.Text.ToString();
                string City = txtCity.Text.ToString();
                string Phone = mskTelePhone.Text.ToString();
                string State = txtState.Text.ToString();
                string IsDup = cbduplicates.Checked ? "Y" : "N";
                string strDob = string.Empty;
                if (dtBirth.Checked)
                    strDob = dtBirth.Value.ToShortDateString();
                string Agency = string.Empty;
                string Dept = string.Empty;
                string Prog = string.Empty;
                string Year = string.Empty;
                if (propFilterType == "H")
                {
                    Agency = BaseForm.BaseAgency;
                    Dept = BaseForm.BaseDept;
                    Prog = BaseForm.BaseProg;
                    Year = BaseForm.BaseYear;
                }

                List<CaseMstSnpEntity> CaseSnpEntityList = _model.CaseMstData.GetSSNSearch(propFilterAllOrApp, SSNNO, FirstName, LastName, HouseNo, Street, City, State, Phone, Alias, IsDup, Agency, Dept, Prog, Year, BaseForm.UserID, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, strDob);

                //Added by SUDHEER ON 06/16/2023 based on the ADDING MEMBER SEARCH 6/15/2023 in OK document as per Jen's request
                if (CaseSnpEntityList.Count > 0)
                {
                    CaseSnpEntityList = CaseSnpEntityList.FindAll(u => u.Agency == BaseForm.BaseAgency);
                }

                if (CaseSnpEntityList.Count > 0)
                {
                    int rowIndex;
                    string strSSN = string.Empty; string TmpHierarchy = ""; string TmpYear = "";
                    // List<string> listSSN = new List<string>(); //int rowIndex = 0;
                    foreach (CaseMstSnpEntity CaseSnp in CaseSnpEntityList)
                    {
                        TmpHierarchy = CaseSnp.Agency.ToString() + '-' + CaseSnp.Dept.ToString() + '-' + CaseSnp.Program.ToString();
                        TmpYear = "    ";
                        if (!string.IsNullOrEmpty(CaseSnp.Year.ToString().Trim()))
                            TmpYear = CaseSnp.Year.ToString();

                        string ApplicantName = LookupDataAccess.GetMemberName(CaseSnp.NameixFi, CaseSnp.NameixMi, CaseSnp.NameixLast, strNameFormat);

                        //if (IsDup.Equals("N"))
                        //{
                        //    if (listSSN.Exists(u => u.Equals(CaseSnp.Ssno.ToString() + ApplicantName))) continue;
                        //}


                        // listSSN.Add(CaseSnp.Ssno.ToString() + ApplicantName);
                        string Address = CaseSnp.Hn.Trim();
                        if (!CaseSnp.Street.Equals(string.Empty))
                        {
                            if (!Address.Equals(string.Empty)) { Address += " "; }
                            Address += CaseSnp.Street.Trim();
                        }
                        if (!CaseSnp.City.Equals(string.Empty))
                        {
                            if (!Address.Equals(string.Empty)) { Address += ", "; }
                            Address += CaseSnp.City.Trim();
                        }
                        if (!CaseSnp.State.Equals(string.Empty))
                        {
                            if (!Address.Equals(string.Empty)) { Address += ", "; }
                            Address += CaseSnp.State.Trim();
                        }
                        //string Homephone = "   ".Substring(0, 3 - CaseSnp.Area.Length) + CaseSnp.Area + CaseSnp.Phone + "       ".Substring(0, 7 - CaseSnp.Phone.Length);

                        string Homephone = "";
                        if (CaseSnp.Phone != "")
                        {
                            if (CaseSnp.Phone.Length == 7)
                            {
                                try
                                {
                                    Homephone = CaseSnp.Area + "-" + CaseSnp.Phone.Substring(0, 3) + "-" + CaseSnp.Phone.Substring(3, (CaseSnp.Phone.Length - 3));//"   ".Substring(0, 3 - CaseSnp.Area.Length) + CaseSnp.Area + CaseSnp.Phone + "       ".Substring(0, 7 - CaseSnp.Phone.Length);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        string MobileNo = "";
                        if (CaseSnp.Mst_Cell_Phone != "")
                            if (CaseSnp.Mst_Cell_Phone.Length >= 10)
                                MobileNo = CaseSnp.Mst_Cell_Phone.Substring(0, 3) + "-" + CaseSnp.Mst_Cell_Phone.Substring(2, 3) + "-" + CaseSnp.Mst_Cell_Phone.Substring(6, 4);
                        //(SUBSTRING(MST.MST_CELL_PHONE, 1, 3) + '-' + SUBSTRING(MST.MST_CELL_PHONE, 4, 3) + '-' + SUBSTRING(MST.MST_CELL_PHONE, 7, 4))

                        if (propScreenName == "APPT0002")
                        {
                            //rowIndex = gvwSSNSearch.Rows.Add(LookupDataAccess.Getdate(CaseSnp.AltBdate), ApplicantName, phone, Address);

                            rowIndex = gvwSSNSearch.Rows.Add(TmpHierarchy, TmpYear, CaseSnp.ApplNo, LookupDataAccess.Getdate(CaseSnp.AltBdate), CaseSnp.NameixFi, CaseSnp.NameixLast,
                                MobileNo, Homephone, Address, CaseSnp.Agency.ToString() + CaseSnp.Dept.ToString() + CaseSnp.Program.ToString(), CaseSnp.Mst_Intake_Worker);
                        }
                        else
                        {
                            if (CaseSnp.Ssno.Trim() != string.Empty)
                                strSSN = LookupDataAccess.GetPhoneSsnNoFormat(CaseSnp.Ssno);

                            //rowIndex = gvwSSNSearch.Rows.Add(strSSN, ApplicantName, phone, Address); // Kranthi 02-23-2023:: NCCAA.docx :: want to change the SSN number to DOB column
                            //rowIndex = gvwSSNSearch.Rows.Add(LookupDataAccess.Getdate(CaseSnp.AltBdate), ApplicantName, phone, Address);

                            rowIndex = gvwSSNSearch.Rows.Add(TmpHierarchy, TmpYear, CaseSnp.ApplNo, LookupDataAccess.Getdate(CaseSnp.AltBdate), CaseSnp.NameixFi,
                                CaseSnp.NameixLast, MobileNo, Homephone, Address, CaseSnp.Agency.ToString() + CaseSnp.Dept.ToString() + CaseSnp.Program.ToString(), CaseSnp.Mst_Intake_Worker);
                        }

                        gvwSSNSearch.Rows[rowIndex].Tag = CaseSnp;
                        if (CaseSnp.SnpActive.Equals("I"))
                            gvwSSNSearch.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;



                    }
                }
                else
                {
                    AlertBox.Show("No Record(s) with Selected Search Criteria", MessageBoxIcon.Warning, null, ContentAlignment.MiddleCenter, autoCloseDelay: 500);
                }
                gvwSSNSearch.SelectionChanged += gvwSSNSearch_SelectionChanged;
                if (gvwSSNSearch.Rows.Count > 0)
                {
                    btnMember.Visible = true; // btnMember.Visible = false; //05/27/2024: 2024 Enhancements.docx :: Brian wants to see the ADD member button if there are search records in the grid or not
                    btnSSNSelect.Visible = true;
                    gvwSSNSearch.Rows[0].Selected = true;
                    Lbl_DOB.Visible = Lbl_Worker.Visible = Lbl_program.Visible = true;
                    gvwSSNSearch_SelectionChanged(gvwSSNSearch, new EventArgs());
                }
                else
                {
                    btnMember.Visible = true; btnSSNSelect.Visible = false;
                }

                this.CloseBox = true;


                if (_CallFromForm == "CASE0012")
                {
                    btnMember.Visible = false;
                }
            }
        }

        public CaseMstSnpEntity GetSelectedRow()
        {
            CaseMstSnpEntity entity = new CaseMstSnpEntity();
            if (gvwSSNSearch != null)
            {
                //entity = null;
                if (NewMem == "Y")
                {
                    entity.NameixFi = txtFirstName.Text.Trim();
                    entity.NameixLast = txtLastName.Text.Trim();
                    entity.Ssno = mskSSNNO.Text.Trim();
                    if (dtBirth.Checked)
                        entity.AltBdate = dtBirth.Value.ToShortDateString();
                }
                else
                {
                    foreach (DataGridViewRow dr in gvwSSNSearch.SelectedRows)
                    {
                        if (dr.Selected)
                        {
                            entity = dr.Tag as CaseMstSnpEntity;
                            string DOB = string.Empty;
                            if (!entity.AltBdate.Equals(string.Empty))
                            {
                                DOB = CommonFunctions.ChangeDateFormat(entity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                            }
                            string yr = entity.Year.Equals(string.Empty) ? "    " : entity.Year;
                            lblFooter.Text = entity.Agency + "  " + entity.Dept + "  " + entity.Program + " " + yr + "   App #" + entity.ApplNo + "       DOB : " + DOB;
                            break;
                        }
                    }
                }
            }

            if (gvwSSNSearch.Rows.Count == 0)
            {
                if (NewMem == "Y")
                {
                    entity.NameixFi = txtFirstName.Text.Trim();
                    entity.NameixLast = txtLastName.Text.Trim();
                    entity.Ssno = mskSSNNO.Text.Trim();
                    if (dtBirth.Checked)
                        entity.AltBdate = dtBirth.Value.ToShortDateString();
                }
            }
            return entity;
        }

        private void btnSSNSelect_Click(object sender, EventArgs e)
        {
            if (strPageType == "Case2001")
            {

                CaseMstSnpEntity CaseMSTSnp = GetSelectedRow();
                if (CaseMSTSnp != null)
                {
                    if (CaseMSTSnp.Ssno != "000000000" && CaseMSTSnp.Ssno.ToUpper() != "NEWSSNNUM")
                    {
                        List<CaseSnpEntity> CaseSnpMeberList = DupMembersChecking(CaseMSTSnp);
                        //CaseSnpAllList = _model.CaseMstData.GetCaseSnpSSno(CaseMSTSnp.Ssno, string.Empty, string.Empty, string.Empty);
                        if (ProgramDefinition.ProDupMEM.Equals("Y"))
                        {
                            if (ProgramDefinition.PRODUPMEM_BY == "1")
                            {
                                CaseSnpEntity casesnpssnoEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(CaseMSTSnp.Ssno));
                                if (casesnpssnoEntity != null)
                                {
                                    AlertBox.Show("SSN Number already exist in this family", MessageBoxIcon.Warning);

                                }
                                else
                                {
                                    CaseSnpEntity casesnpAlllistEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.Ssno.Equals(CaseMSTSnp.Ssno));
                                    if (casesnpAlllistEntity != null)
                                    {
                                        if (!string.IsNullOrEmpty(casesnpAlllistEntity.Ssno.Trim()))
                                        {

                                            if (casesnpAlllistEntity.Ssno != "000000000")
                                            {
                                                MessageBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App, "CAPTAIN");
                                                //_errorProvider.SetError(mskSSN, "Member already exists in App#: " + casesnpAlllistEntity.App);

                                            }
                                            else
                                            {
                                                this.DialogResult = DialogResult.OK;
                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            this.DialogResult = DialogResult.OK;
                                            this.Close();
                                        }
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(ProgramDefinition.PRODUPMEM_BY.Trim()))
                            {
                                CaseSnpEntity casesnpAlllistEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.NameixFi.Trim().ToUpper().Equals(CaseMSTSnp.NameixFi.Trim().ToUpper()) && Convert.ToDateTime(u.AltBdate).ToShortDateString() == Convert.ToDateTime(CaseMSTSnp.AltBdate).ToShortDateString());
                                if (casesnpAlllistEntity != null)
                                {
                                    if (ProgramDefinition.PRODUPMEM_BY == "2")
                                        AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App + " With " + casesnpAlllistEntity.NameixFi.Trim() + " and " + Convert.ToDateTime(casesnpAlllistEntity.AltBdate.ToString()).ToShortDateString(), MessageBoxIcon.Warning);
                                    else if (ProgramDefinition.PRODUPMEM_BY == "3")
                                        AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App + " With " + casesnpAlllistEntity.NameixFi.Trim().Substring(0, 1) + " and " + Convert.ToDateTime(casesnpAlllistEntity.AltBdate.ToString()).ToShortDateString(), MessageBoxIcon.Warning);
                                    //AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App + " With " + casesnpAlllistEntity.NameixFi.Trim().Substring(0, 1) + " and " + Convert.ToDateTime(casesnpAlllistEntity.AltBdate.ToString()).ToShortDateString(), MessageBoxIcon.Warning);

                                }
                                else
                                {
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                    //List<CaseSnpEntity> casesnpssnosearchEntity = CaseSnpAllList.FindAll(u => u.Ssno.Equals(mskSSN.Text));
                                    //if (casesnpssnosearchEntity != null)
                                    //{
                                    //    if (casesnpssnosearchEntity.Count != 0)
                                    //    {
                                    //        if (casesnpssnosearchEntity.Count == 1)
                                    //        {
                                    //            // mskSSN.Text = selectedSsn.Ssno;
                                    //            AltAgency = casesnpssnosearchEntity[0].Agency;
                                    //            AltDept = casesnpssnosearchEntity[0].Dept;
                                    //            AltProgram = casesnpssnosearchEntity[0].Program;
                                    //            AltYear = casesnpssnosearchEntity[0].Year;
                                    //            AltApp = casesnpssnosearchEntity[0].App.Substring(0, 8);
                                    //            AltFamilySeq = casesnpssnosearchEntity[0].FamilySeq;
                                    //            SearchSnpType = "SSNSEARCH";
                                    //            fillCaseSnpDetails(AltAgency.ToString(), AltDept.ToString(), AltProgram.ToString(), AltYear.ToString(), AltApp.ToString(), AltFamilySeq.ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            SsnScanForm SsnScanForm = new SsnScanForm(BaseForm, mskSSN.Text);
                                    //            SsnScanForm.FormClosed += new FormClosedEventHandler(OnSsnScanSearchFormClosed);
                                    //            SsnScanForm.ShowDialog();
                                    //        }
                                    //    }
                                    //}

                                }
                            }
                            else
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }


                        }
                        else
                        {
                            if (ProgramDefinition.ProDupMEM.Equals("Y"))
                            {
                                if (ProgramDefinition.PRODUPMEM_BY == "1")
                                {
                                    CaseSnpEntity casesnpssnoEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(CaseMSTSnp.Ssno));
                                    if (casesnpssnoEntity != null)
                                    {
                                        AlertBox.Show("SSN Number already exist in this family", MessageBoxIcon.Warning);

                                    }
                                }
                                else
                                {
                                    CaseSnpEntity casesnpAlllistEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.NameixFi.Trim().Equals(CaseMSTSnp.NameixFi.Trim()) && u.AltBdate == CaseMSTSnp.AltBdate);
                                    if (casesnpAlllistEntity != null)
                                    {
                                        if (ProgramDefinition.PRODUPMEM_BY == "2")
                                            AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App + " With " + casesnpAlllistEntity.NameixFi.Trim() + " and " + Convert.ToDateTime(casesnpAlllistEntity.AltBdate.ToString()).ToShortDateString(), MessageBoxIcon.Warning);
                                        else if (ProgramDefinition.PRODUPMEM_BY == "3")
                                            AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App + " With " + casesnpAlllistEntity.NameixFi.Trim().Substring(0, 1) + " and " + Convert.ToDateTime(casesnpAlllistEntity.AltBdate.ToString()).ToShortDateString(), MessageBoxIcon.Warning);

                                        //AlertBox.Show("Member already exists in App#: " + casesnpAlllistEntity.App, MessageBoxIcon.Warning);

                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();


                                    }
                                }



                            }
                            else
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            //CaseSnpEntity casesnpssnoEntity = CaseSnpMeberList.Find(u => u.Agency.Equals(CaseMST.ApplAgency) && u.Dept.Equals(CaseMST.ApplDept) && u.Program.Equals(CaseMST.ApplProgram) && u.Year.Trim().Equals(CaseMST.ApplYr.Trim()) && u.App.Equals(CaseMST.ApplNo) && u.Ssno.Equals(CaseMSTSnp.Ssno));
                            //if (casesnpssnoEntity != null)
                            //{
                            //    AlertBox.Show("SSN Number already exist in this family", MessageBoxIcon.Warning);

                            //}
                            //else
                            //{
                            //    this.DialogResult = DialogResult.OK;
                            //    this.Close();
                            //}

                        }

                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void Add_New_APP_Confirmation(DialogResult dialogResult)
        {
            //MessageBoxWindow messageBoxWindow = sender as MessageBoxWindow;
            if (dialogResult == DialogResult.Yes)
            {
                NewMem = "Y";
                this.DialogResult = DialogResult.Yes;
                this.Close();

                //CaseMstEntity caseMSTEntity = new CaseMstEntity();
                //CaseSnpEntity caseSNPEntity = new CaseSnpEntity();
                //caseMSTEntity.ApplAgency = BaseForm.BaseAgency;
                //caseMSTEntity.ApplDept = BaseForm.BaseDept;
                //caseMSTEntity.ApplProgram = BaseForm.BaseProg;
                //caseMSTEntity.ApplYr = (!(string.IsNullOrEmpty(BaseForm.BaseYear)) ? BaseForm.BaseYear : "    ");


                //caseSNPEntity.NameixFi = txtFirstName.Text.Trim();
                //caseSNPEntity.NameixLast = txtLastName.Text.Trim();
                //caseSNPEntity.Ssno = caseMSTEntity.Ssn = mskSSNNO.Text.Trim();
                //if (dtBirth.Checked)
                //    caseSNPEntity.AltBdate = dtBirth.Value.ToShortDateString();

                //string strRelationDefaultValue = string.Empty;
                //List<CommonEntity> Relation = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                //if (Relation.Count > 0)
                //{
                //    CommonEntity commRelation = Relation.Find(u => u.Default == "Y");
                //    if (commRelation != null)
                //        strRelationDefaultValue = commRelation.Code.ToString();
                //}

                //caseSNPEntity.MemberCode = strRelationDefaultValue;

                ////if (!string.IsNullOrEmpty(PassDOB) ||
                ////   !string.IsNullOrEmpty(Fname) ||
                ////   !string.IsNullOrEmpty(Lname) ||
                ////   !string.IsNullOrEmpty(PassSSN))


                ////Privileges.Program = "SSN";
                ////ClientSNPForm clientSNPForm = new ClientSNPForm(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                ////clientSNPForm.FormClosed += new Form.FormClosedEventHandler(On_Applicant_Dragged);
                ////clientSNPForm.ShowDialog();
                //if (BaseForm.BaseAgencyControlDetails.RomaSwitch.ToUpper() == "Y")
                //{
                //    this.Close();

                //    Privileges.AddPriv = "true";
                //    Case4001Form clientSNPForm = new Case4001Form(BaseForm, false, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                //    //clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                //    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                //    clientSNPForm.ShowDialog();




                //    //New_Dragged_App_No = BaseForm.BaseApplicationNo;
                //    //BtnSelApp_Click(BtnSelApp, EventArgs.Empty);


                //}
                ////else
                ////{
                ////    Case3001Form clientSNPForm = new Case3001Form(BaseForm, true, caseMSTEntity, caseSNPEntity, Consts.Common.Add, Privileges, null, string.Empty);
                ////    //clientSNPForm.FormClosed += new FormClosedEventHandler(On_Applicant_Dragged);
                ////    clientSNPForm.StartPosition = FormStartPosition.CenterScreen;
                ////    clientSNPForm.ShowDialog();
                ////}
            }
        }


        private List<CaseSnpEntity> DupMembersChecking(CaseMstSnpEntity MSTSNP)
        {
            List<CaseSnpEntity> SelMems = new List<CaseSnpEntity>();

            string DOB = string.Empty;
            //if (!MSTSNP && dtBirth.Checked)
            DOB = MSTSNP.AltBdate;

            //if (Mode == "Add")
            //{
            if (ProgramDefinition.PRODUPMEM_BY == "2" || ProgramDefinition.PRODUPMEM_BY == "3")
            {
                if (!string.IsNullOrEmpty(DOB.Trim()) && !string.IsNullOrEmpty(MSTSNP.NameixFi.Trim()))
                    SelMems = _model.CaseMstData.GetCaseSnpSSno(MSTSNP.Ssno, DOB, MSTSNP.NameixFi, ProgramDefinition.PRODUPMEM_BY.ToString().Trim());
            }
            else //if (ProgramDefinition.PRODUPMEM_BY == "1")
                SelMems = _model.CaseMstData.GetCaseSnpSSno(MSTSNP.Ssno, DOB, MSTSNP.NameixFi, ProgramDefinition.PRODUPMEM_BY.ToString().Trim());

            //}
            //else
            //{
            //    if (SearchType == "FNDOB")
            //    {
            //        if (ProgramDefinition.PRODUPMEM_BY == "2" || ProgramDefinition.PRODUPMEM_BY == "3")
            //        {
            //            if (!string.IsNullOrEmpty(DOB.Trim()) && !string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            //                SelMems = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, DOB, txtFirstName.Text.Trim(), ProgramDefinition.PRODUPMEM_BY.ToString().Trim());
            //        }
            //        else if (ProgramDefinition.PRODUPMEM_BY == "1" && strSsnNumber != mskSSN.Text)
            //            SelMems = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, DOB, txtFirstName.Text.Trim(), ProgramDefinition.PRODUPMEM_BY.ToString().Trim());
            //    }
            //    else if (SearchType == "SSN" && ProgramDefinition.PRODUPMEM_BY == "1")
            //        SelMems = _model.CaseMstData.GetCaseSnpSSno(mskSSN.Text, DOB, txtFirstName.Text.Trim(), ProgramDefinition.PRODUPMEM_BY.ToString().Trim());
            //}
            return SelMems;
        }
        private void gvwSSNSearch_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwSSNSearch.Rows.Count > 0 && gvwSSNSearch.SelectedRows.Count > 0)
            {
                CaseMstSnpEntity entity = gvwSSNSearch.SelectedRows[0].Tag as CaseMstSnpEntity;
                string DOB = string.Empty;
                if (!entity.AltBdate.Equals(string.Empty))
                    DOB = CommonFunctions.ChangeDateFormat(entity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                string yr = entity.Year.Equals(string.Empty) ? "    " : entity.Year;

                Lbl_DOB.Text = "DOB - " + CommonFunctions.ChangeDateFormat(entity.AltBdate, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                Lbl_Worker.Text = "Case Worker - " + Get_CaseWorket_DESC(gvwSSNSearch.CurrentRow.Cells[10].Value.ToString());
                Lbl_program.Text = "Program - " + Get_Program_Desc(gvwSSNSearch.CurrentRow.Cells[9].Value.ToString());

                //lblFooter.Text = entity.Agency + "  " + entity.Dept + "  " + entity.Program + "  " + yr + "  App #" + entity.ApplNo + "       DOB : " + DOB;
                btnSSNSelect.Visible = true;
            }
            else
            {
                Lbl_DOB.Text = string.Empty; lblAppDate2.Text = string.Empty; Lbl_Worker.Text = string.Empty; Lbl_program.Text = string.Empty;
                btnSSNSelect.Visible = false;
            }
        }
        private string Get_Program_Desc(string Prog_Code)
        {
            string Prog_Desc = "";
            if (BaseForm.BaseCaseHierachyListEntity.Count > 0)
            {
                HierarchyEntity hierchyName = BaseForm.BaseCaseHierachyListEntity.Find(u => u.Agency.Trim() + u.Dept.Trim() + u.Prog.Trim() == Prog_Code);
                if (hierchyName != null)
                {
                    Prog_Desc = hierchyName.HirarchyName;
                }
            }
            return Prog_Desc;
        }
        List<ListItem> Worker_List { get; set; }
        private string Get_CaseWorket_DESC(string Worker_Code)
        {
            string Worker_Desc = "";
            foreach (ListItem List in Worker_List)
            {
                if (List.Value.ToString().Trim() == Worker_Code.Trim())
                {
                    Worker_Desc = List.Text.ToString().Trim();
                    break;
                }
            }

            return Worker_Desc;
        }

        private bool ValidateForm()
        {
            bool isValid = true;


            if (String.IsNullOrEmpty(mskSSNNO.Text.Trim()) && String.IsNullOrEmpty(txtAlias.Text.Trim()) && String.IsNullOrEmpty(txtCity.Text.Trim()) && String.IsNullOrEmpty(txtFirstName.Text.Trim()) && String.IsNullOrEmpty(txtHouseNo.Text.Trim()) && String.IsNullOrEmpty(txtLastName.Text.Trim()) && String.IsNullOrEmpty(txtState.Text.Trim()) && String.IsNullOrEmpty(txtStreet.Text.Trim()) && string.IsNullOrEmpty(mskTelePhone.Text) && (dtBirth.Checked == false))
            {
                AlertBox.Show("Please type at least in One Field to Search for", MessageBoxIcon.Warning);
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(mskSSNNO, null);
            }
            //if (dtBirth.Checked == false)
            //{
            //    _errorProvider.SetError(dtBirth, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDOB.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtBirth, null);
            //}
            return (isValid);
        }

        string NewMem = "N";
        private void btnMember_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are about to add a Member into " + BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " " + BaseForm.BaseYear,
                               Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Add_New_APP_Confirmation);
        }
    }
}