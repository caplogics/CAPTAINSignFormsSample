#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.UserControls;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class STFMST10Form : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public STFMST10Form(BaseForm baseForm, string mode, string strStaffCode, PrivilegeEntity privilegeEntity, STAFFMSTEntity staffEntity)
        {
            InitializeComponent();
            BaseForm = baseForm;
            Privileges = privilegeEntity;
            Mode = mode;
            SelectStaffCodeId = strStaffCode;
           // this.Text = privilegeEntity.PrivilegeName;
            propStaffEntity = staffEntity;
            this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Add;
            _model = new CaptainModel();

            btnAppSearch.Visible = true;
            dtAcquired.Value = DateTime.Now;
            dtHired.Value = DateTime.Now;
            dtTerminated.Value = DateTime.Now;
            dtTransfer.Value = DateTime.Now;

            dtAcquired.Checked = false;
            dtHired.Checked = false;
            dtTerminated.Checked = false;
            dtTransfer.Checked = false;

            RemoveEventHandler();
            Dropdownfills();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtEmployNumber.Validator = TextBoxValidation.IntegerValidator;
            txtZip.Validator = TextBoxValidation.IntegerValidator;
            txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
            txtStdHours.Validator = TextBoxValidation.FloatValidator;
            txtWeeksworked.Validator = TextBoxValidation.IntegerValidator;
            txtHourlyRate.Validator = TextBoxValidation.FloatValidator;// CustomDecimalValidation3dot5;
            txtYearsinPosition.Validator = TextBoxValidation.IntegerValidator;
            propstaffPostEntity = _model.STAFFData.GetStaffPostDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, strStaffCode, string.Empty, string.Empty);

            staffBulkPostEntity = _model.STAFFData.GetStaffBulkPost(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,BaseForm.BaseYear, SelectStaffCodeId);

            if (Mode.Equals("Edit"))
            {
                txtEmployNumber.Enabled = false;
                if (Privileges.Program == "HUD99001")
                    this.Text = "Staff Master Maintenance – Edit";
                else
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.Edit;
                FillAllControls();
                btnAppSearch.Visible = false;
            }
            else if (Mode.Equals("View"))
            {
                DisableControls();
                btnAppSearch.Visible = false;
                if (Privileges.Program == "HUD99001")
                    this.Text = "Staff Master Maintenance – View";
                else
                    this.Text = privilegeEntity.PrivilegeName + " - " + Consts.Common.View;
                FillAllControls();
            }
            if (staffBulkPostEntity.Count > 0)//(propstaffPostEntity.Count > 0)
            {
                btnCourseDetails.Visible = true;
            }
            else
            {
                btnCourseDetails.Visible = false;
            }
            chkActive_CheckedChanged(chkActive, new EventArgs());
            AddEventHandler();
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Mode { get; set; }

        public string SelectStaffCodeId { get; set; }

        public bool IsSaveValid { get; set; }

        public STAFFMSTEntity propStaffEntity { get; set; }

        public List<STAFFPostEntity> propstaffPostEntity { get; set; }

        public List<STAFFBULKPOSTEntity> staffBulkPostEntity
        {
            get; set;
        }

        private void PbPdf_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "STFMST10");
        }

        private void Dropdownfills()
        {
            cmbSite.Items.Clear();
            //cmbSite.Items.Insert(0, new ListItem("All", "****", "Y", Color.Green));
            //cmbSite.Items.Insert(0, new ListItem("None", "0", "Y", Color.White));
            cmbSite.ColorMember = "FavoriteColor";
            
            List<CaseSiteEntity> SiteList = new List<CaseSiteEntity>();
            CaseSiteEntity Search_Site = new CaseSiteEntity(true);
            Search_Site.SiteAGENCY = BaseForm.BaseAgency;
            if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()))
            {
                Search_Site.SiteDEPT = BaseForm.BaseDept;
                Search_Site.SitePROG = BaseForm.BaseProg;
                Search_Site.SiteYEAR = BaseForm.BaseYear;
            }
            Search_Site.SiteROOM = "0000";
            SiteList = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");

            if(SiteList.Count == 0) 
            {
                Search_Site = new CaseSiteEntity(true);
                Search_Site.SiteAGENCY = BaseForm.BaseAgency;
                SiteList = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");
            }

            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //DataTable dt = ds.Tables[0];
            //if (dt.Rows.Count > 0)
            //{

            //if (!string.IsNullOrEmpty(BaseForm.BaseYear.Trim()) && SiteList.Count>0)
            //{
            //    SiteList= SiteList.FindAll(u=>u.SiteAGENCY==BaseForm.BaseAgency && u.SiteDEPT==BaseForm.BaseDept && u.SitePROG==BaseForm.BaseProg && u.SiteYEAR==BaseForm.BaseYear);
            //}

            if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
            {
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                HierarchyEntity hierarchyEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        { hierarchyEntity = null; }
                    }
                }

                if (hierarchyEntity != null)
                {
                    if (hierarchyEntity.Sites.Length > 0)
                    {
                        string[] Sites = hierarchyEntity.Sites.Split(',');

                        for (int i = 0; i < Sites.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                            {
                                foreach (CaseSiteEntity casesite in SiteList) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (Sites[i].ToString() == casesite.SiteNUMBER)
                                    {
                                        selsites.Add(casesite);
                                        //break;
                                    }
                                    // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                }
                            }
                        }
                        //strsiteRoomNames = hierarchyEntity.Sites;
                        SiteList = selsites;
                    }
                    //SiteList = selsites;
                }
                else
                {
                    cmbSite.Items.Insert(0, new ListItem("All", "****", "Y", Color.Black));
                    cmbSite.Items.Insert(0, new ListItem("None", "0", "Y", Color.White));
                    cmbSite.SelectedIndex = 0;
                }
            }
            else
            {
                cmbSite.Items.Insert(0, new ListItem("All", "****", "Y", Color.Black));
                cmbSite.Items.Insert(0, new ListItem("None", "0", "Y", Color.White));
                cmbSite.SelectedIndex = 0;
            }

            if (SiteList.Count > 0)
                SiteList = SiteList.OrderBy(u => u.SiteNAME).ToList();
            foreach (CaseSiteEntity dr in SiteList)
            {
                ListItem li = new ListItem(dr.SiteNAME.ToString(), dr.SiteNUMBER.ToString(), dr.SiteACTIVE.ToString(), dr.SiteACTIVE.ToString().Equals("Y") ? Color.Black : Color.Red);
                cmbSite.Items.Add(li);
            }
            //DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //DataTable dt = ds.Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        ListItem li = new ListItem(dr["SITE_NAME"].ToString(), dr["SITE_NUMBER"].ToString(), dr["SITE_ACTIVE"].ToString(), dr["SITE_ACTIVE"].ToString().Equals("Y") ? Color.Green : Color.Red);
            //        cmbSite.Items.Add(li);
            //    }
            //}

            cmbPrimaryLang.Items.Clear();
            //List<CommonEntity> PrimaryLanguage = _model.lookupDataAccess.GetPrimaryLanguage();
            //List<CommonEntity> Languages = filterByHIE(PrimaryLanguage);
            List<CommonEntity> PrimaryLanguage = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0203", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            cmbPrimaryLang.Items.Insert(0, new ListItem("None", "0"));
            cmbPrimaryLang.ColorMember = "FavoriteColor";
            cmbPrimaryLang.SelectedIndex = 0;
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbPrimaryLang.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbPrimaryLang.SelectedItem = li;
            }
            cmbLanguage1.Items.Clear();
            cmbLanguage1.Items.Insert(0, new ListItem("None", "0"));
            cmbLanguage1.ColorMember = "FavoriteColor";
            cmbLanguage1.SelectedIndex = 0;
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbLanguage1.Items.Add(li);
                //  if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbLanguage1.SelectedItem = li;
            }
            cmbLanguage2.Items.Clear();
            cmbLanguage2.Items.Insert(0, new ListItem("None", "0"));
            cmbLanguage2.ColorMember = "FavoriteColor";
            cmbLanguage2.SelectedIndex = 0;
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbLanguage2.Items.Add(li);
                // if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbLanguage2.SelectedItem = li;
            }

            cmbLanguage3.Items.Clear();
            cmbLanguage3.Items.Insert(0, new ListItem("None", "0"));
            cmbLanguage3.ColorMember = "FavoriteColor";
            cmbLanguage3.SelectedIndex = 0;
            foreach (CommonEntity primarylanguage in PrimaryLanguage)
            {
                ListItem li = new ListItem(primarylanguage.Desc, primarylanguage.Code, primarylanguage.Active, primarylanguage.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbLanguage3.Items.Add(li);
                //if (Mode.Equals(Consts.Common.Add) && primarylanguage.Default.Equals("Y")) cmbLanguage3.SelectedItem = li;
            }

            cmbEthnicity.Items.Clear();
            //List<CommonEntity> Ethnicity = _model.lookupDataAccess.GetEthnicity();
            //Ethnicity = filterByHIE(Ethnicity);
            List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0201", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            cmbEthnicity.Items.Insert(0, new ListItem("None", "0"));
            cmbEthnicity.ColorMember = "FavoriteColor";
            cmbEthnicity.SelectedIndex = 0;
            foreach (CommonEntity Etncity in Ethnicity)
            {
                ListItem li = new ListItem(Etncity.Desc, Etncity.Code, Etncity.Active, Etncity.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbEthnicity.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Etncity.Default.Equals("Y")) cmbEthnicity.SelectedItem = li;
            }

            cmbRace.Items.Clear();
            //List<CommonEntity> Race = _model.lookupDataAccess.GetRace();
            //Race = filterByHIE(Race);
            List<CommonEntity> Race = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0202", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            cmbRace.Items.Insert(0, new ListItem("None", "0"));
            cmbRace.ColorMember = "FavoriteColor";
            cmbRace.SelectedIndex = 0;
            foreach (CommonEntity race in Race)
            {
                ListItem li = new ListItem(race.Desc, race.Code, race.Active, race.Active.Equals("Y") ? Color.Black : Color.Red);
                cmbRace.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && race.Default.Equals("Y")) cmbRace.SelectedItem = li;
            }


            CmbReasonfortermination.Items.Clear();
            //List<CommonEntity> Reason = _model.lookupDataAccess.GetReasonfor();
            List<CommonEntity> Reason = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0204", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            //Reason = filterByHIE(Reason);
            CmbReasonfortermination.Items.Insert(0, new ListItem("None", "0"));
            CmbReasonfortermination.ColorMember = "FavoriteColor";
            CmbReasonfortermination.SelectedIndex = 0;
            foreach (CommonEntity Reasonfor in Reason)
            {
                ListItem li = new ListItem(Reasonfor.Desc, Reasonfor.Code, Reasonfor.Active, Reasonfor.Active.Equals("Y") ? Color.Black : Color.Red);
                CmbReasonfortermination.Items.Add(li);
                if (Mode.Equals(Consts.Common.Add) && Reasonfor.Default.Equals("Y")) CmbReasonfortermination.SelectedItem = li;
            }

            cmbPositionalctg.Items.Clear();
            
            List<CommonEntity> commonPositionctg = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0031", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
            cmbPositionalctg.Items.Insert(0, new ListItem("Select One", "0"));
            foreach (CommonEntity item in commonPositionctg)
            {
                ListItem li = new ListItem(item.Desc, item.Code);
                cmbPositionalctg.Items.Add(li);
            }
           
            if (cmbPositionalctg.Items.Count > 0)
                cmbPositionalctg.SelectedIndex = 0;


            cmbEducationcompleted.Items.Clear();
            List<CommonEntity> commonEducationcomple = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0034", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);
           
             foreach (CommonEntity item in commonEducationcomple)
             {
                 ListItem li = new ListItem(item.Desc, item.Code);
                 cmbEducationcompleted.Items.Add(li);
             }
            if (cmbEducationcompleted.Items.Count > 0)
                cmbEducationcompleted.SelectedIndex = 0;

            cmbEducationProgress.Items.Clear();
            List<CommonEntity> commonEducationProgress = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0033", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            foreach (CommonEntity item in commonEducationProgress)
            {
                  ListItem li = new ListItem(item.Desc, item.Code);
                cmbEducationProgress.Items.Add(li);
            }          
            if (cmbEducationProgress.Items.Count > 0)
                cmbEducationProgress.SelectedIndex = 0;


            cmbEmploymentType.Items.Clear();
            List<CommonEntity> commonEmploymentType = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0032", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, Mode);

            foreach (CommonEntity item in commonEmploymentType)
            {
                ListItem li = new ListItem(item.Desc, item.Code);
                cmbEmploymentType.Items.Add(li);
            }          
            if (cmbEmploymentType.Items.Count > 0)
                cmbEmploymentType.SelectedIndex = 0;


        }

        private void FillAllControls()
         {
            if (propStaffEntity != null)
            {
                if (propStaffEntity.Active.Equals("A"))
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;

                txtEmployNumber.Text = propStaffEntity.Staff_Code;
                txtFirstName.Text = propStaffEntity.First_Name;
                txtMI.Text = propStaffEntity.Middle_Name;
                txtLastName.Text = propStaffEntity.Last_Name;
                CommonFunctions.SetComboBoxValue(cmbPrimaryLang, propStaffEntity.Language);
                CommonFunctions.SetComboBoxValue(cmbLanguage1, propStaffEntity.Language1);
                CommonFunctions.SetComboBoxValue(cmbLanguage2, propStaffEntity.Language2);
                CommonFunctions.SetComboBoxValue(cmbLanguage3, propStaffEntity.Language3);
                CommonFunctions.SetComboBoxValue(cmbPositionalctg, propStaffEntity.POS_CTG);
                CommonFunctions.SetComboBoxValue(cmbEducationcompleted, propStaffEntity.Education);
                CommonFunctions.SetComboBoxValue(cmbEducationProgress, propStaffEntity.Edu_Progress);
                CommonFunctions.SetComboBoxValue(CmbReasonfortermination, propStaffEntity.RES_Terminated);
                CommonFunctions.SetComboBoxValue(cmbRace, propStaffEntity.Race);
                CommonFunctions.SetComboBoxValue(cmbEthnicity, propStaffEntity.Ethnicity);
                CommonFunctions.SetComboBoxValue(cmbSite, propStaffEntity.Site);
                CommonFunctions.SetComboBoxValue(cmbEmploymentType, propStaffEntity.Employment_Type);
                if (propStaffEntity.Date_Hired != string.Empty)
                {
                    dtHired.Value = Convert.ToDateTime(propStaffEntity.Date_Hired);
                    dtHired.Checked = true;
                }
                if (propStaffEntity.Transerfer_Date != string.Empty)
                {
                    dtTransfer.Value = Convert.ToDateTime(propStaffEntity.Transerfer_Date);
                    dtTransfer.Checked = true;
                }
                if (propStaffEntity.Date_Acquired != string.Empty)
                {
                    dtAcquired.Value = Convert.ToDateTime(propStaffEntity.Date_Acquired);
                    dtAcquired.Checked = true;
                }
                if (propStaffEntity.Date_Terminated != string.Empty)
                {
                    dtTerminated.Value = Convert.ToDateTime(propStaffEntity.Date_Terminated);
                    dtTerminated.Checked = true;
                }
                txtYearsinPosition.Text = propStaffEntity.Years_in_POS;
                txtStreet.Text = propStaffEntity.Street;
                txtHN.Text = propStaffEntity.HNo;
                txtSuffix.Text = propStaffEntity.Suffix;
                txtApt.Text = propStaffEntity.Apt;
                txtFloor.Text = propStaffEntity.Floor;
                txtZip.Text = SetLeadingZeros(propStaffEntity.Zip);
                txtZipPlus.Text = "0000".Substring(0, 4 - propStaffEntity.Zip_Plus.Length) + propStaffEntity.Zip_Plus;
                txtState.Text = propStaffEntity.State;
                txtStdHours.Text = propStaffEntity.HRS_Worked_PW;
                txtWeeksworked.Text = propStaffEntity.Weeks_Worked;
                txtHourlyRate.Text = propStaffEntity.Base_Rate;
                txtSalary.Text = propStaffEntity.Salary;
                txtAnnualSalary.Text = propStaffEntity.Anual_Salary;
                chkContructed.Checked = propStaffEntity.Workfor_CONT.Equals("Y") ? true : false;
                chkHS.Checked = propStaffEntity.Workfor_HS.Equals("Y") ? true : false;
                ChkEhs.Checked = propStaffEntity.Workfor_EHS.Equals("Y") ? true : false;

                chkbHS2.Checked = propStaffEntity.Workfor_HS2.Equals("Y") ? true : false;
                chkbEHSCCP.Checked = propStaffEntity.Workfor_EHSCCP.Equals("Y") ? true : false;

                chkNonHs.Checked = propStaffEntity.Workfor_NONHS.Equals("Y") ? true : false;
                chkVolunteer.Checked = propStaffEntity.Workfor_VOL.Equals("Y") ? true : false;
                chkHeadsp.Checked = propStaffEntity.HS_Parent.Equals("1") ? true : false;
                chkDaycarePP.Checked = propStaffEntity.Daycare_Parent.Equals("1") ? true : false;
                chkEarlyHp.Checked = propStaffEntity.EHS_Parent.Equals("1") ? true : false;
                chkPositionFam.Checked = propStaffEntity.POS_Filled.Equals("1") ? true : false;
                chkthisinirasm.Checked = propStaffEntity.Replace_SM.Equals("1") ? true : false;
                txtPosition.Text = propStaffEntity.Position_Data;
                txtCityName.Text = propStaffEntity.City;
            }
        }



        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            //if (LookupValues.Exists(u => u.Hierarchy.Equals(HIE)))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(HIE)).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + CaseMST.ApplDept + "**")).ToList();
            //else if (LookupValues.Exists(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")))
            //    LookupValues = LookupValues.FindAll(u => u.Hierarchy.Equals(CaseMST.ApplAgency + "****")).ToList();
            //else
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******")).ToList();

            return LookupValues;
        }

        private void cmbRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbRace.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbRace.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show(Consts.AgyTab.RACEMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmbEthnicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbEthnicity.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbEthnicity.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show(Consts.AgyTab.ETHNICMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmbPrimaryLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbPrimaryLang.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show(Consts.AgyTab.PRIMARYLanguage, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void cmbLanguage1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbLanguage1.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbLanguage1.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show("Inactive Language 1", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void cmbLanguage2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbLanguage2.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbLanguage2.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show("Inactive Language 2", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void cmbLanguage3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbLanguage3.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbLanguage3.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show("Inactive Language 3", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbSite.SelectedItem).Value.ToString() != "0")
                if (((ListItem)cmbSite.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show("Inactive Site", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void AddEventHandler()
        {
            cmbEthnicity.SelectedIndexChanged += new EventHandler(cmbEthnicity_SelectedIndexChanged);
            cmbRace.SelectedIndexChanged += new EventHandler(cmbRace_SelectedIndexChanged);
            cmbSite.SelectedIndexChanged += new EventHandler(cmbSite_SelectedIndexChanged);
            cmbPrimaryLang.SelectedIndexChanged += new EventHandler(cmbPrimaryLang_SelectedIndexChanged);
            cmbLanguage1.SelectedIndexChanged += new EventHandler(cmbLanguage1_SelectedIndexChanged);
            cmbLanguage2.SelectedIndexChanged += new EventHandler(cmbLanguage2_SelectedIndexChanged);
            cmbLanguage3.SelectedIndexChanged += new EventHandler(cmbLanguage3_SelectedIndexChanged);
            CmbReasonfortermination.SelectedIndexChanged += new EventHandler(CmbReasonfortermination_SelectedIndexChanged);
        }

        private void RemoveEventHandler()
        {
            cmbEthnicity.SelectedIndexChanged -= new EventHandler(cmbEthnicity_SelectedIndexChanged);
            cmbRace.SelectedIndexChanged -= new EventHandler(cmbRace_SelectedIndexChanged);
            cmbSite.SelectedIndexChanged -= new EventHandler(cmbSite_SelectedIndexChanged);
            cmbPrimaryLang.SelectedIndexChanged -= new EventHandler(cmbPrimaryLang_SelectedIndexChanged);
            cmbLanguage1.SelectedIndexChanged -= new EventHandler(cmbLanguage1_SelectedIndexChanged);
            cmbLanguage2.SelectedIndexChanged -= new EventHandler(cmbLanguage2_SelectedIndexChanged);
            cmbLanguage3.SelectedIndexChanged -= new EventHandler(cmbLanguage3_SelectedIndexChanged);
            CmbReasonfortermination.SelectedIndexChanged -= new EventHandler(CmbReasonfortermination_SelectedIndexChanged);
        }

        private void CmbReasonfortermination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)CmbReasonfortermination.SelectedItem).Value.ToString() != "0")
                if (((ListItem)CmbReasonfortermination.SelectedItem).ID.ToString() != "Y")
                    MessageBox.Show(Consts.AgyTab.ReasonforMsg, Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            STAFFMSTEntity staffMstEntit = new STAFFMSTEntity();
            if (!isValidate()) return;

            if (chkActive.Checked == true)
                staffMstEntit.Active = "A";
            else
                staffMstEntit.Active = "I";

            staffMstEntit.Agency = BaseForm.BaseAgency;
            staffMstEntit.Year = BaseForm.BaseYear;

            staffMstEntit.Staff_Code = txtEmployNumber.Text;
            staffMstEntit.First_Name = txtFirstName.Text;
            staffMstEntit.Middle_Name = txtMI.Text;
            staffMstEntit.Last_Name = txtLastName.Text;

            if (!((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Language = ((ListItem)cmbPrimaryLang.SelectedItem).Value.ToString();
            }

            if (!((ListItem)cmbLanguage1.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Language1 = ((ListItem)cmbLanguage1.SelectedItem).Value.ToString();
            }

            if (!((ListItem)cmbLanguage2.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Language2 = ((ListItem)cmbLanguage2.SelectedItem).Value.ToString();
            }
            if (!((ListItem)cmbLanguage3.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Language3 = ((ListItem)cmbLanguage3.SelectedItem).Value.ToString();
            }
            if(cmbPositionalctg.Items.Count>0)
            staffMstEntit.POS_CTG = ((ListItem)cmbPositionalctg.SelectedItem).Value.ToString();
            //if (!((ListItem)cmbEducationcompleted.SelectedItem).Value.ToString().Equals("0"))
            //{
            if (cmbEducationcompleted.Items.Count > 0)
            staffMstEntit.Education = ((ListItem)cmbEducationcompleted.SelectedItem).Value.ToString();
            //}
            if (cmbEducationProgress.Items.Count > 0)
            staffMstEntit.Edu_Progress = ((ListItem)cmbEducationProgress.SelectedItem).Value.ToString();
            if (cmbEmploymentType.Items.Count > 0)
            staffMstEntit.Employment_Type = ((ListItem)cmbEmploymentType.SelectedItem).Value.ToString();
            if (chkActive.Checked == false)
                staffMstEntit.RES_Terminated = ((ListItem)CmbReasonfortermination.SelectedItem).Value.ToString();

            if (!((ListItem)cmbRace.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Race = ((ListItem)cmbRace.SelectedItem).Value.ToString();
            }

            if (!((ListItem)cmbEthnicity.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Ethnicity = ((ListItem)cmbEthnicity.SelectedItem).Value.ToString();
            }
            if (!((ListItem)cmbSite.SelectedItem).Value.ToString().Equals("0"))
            {
                staffMstEntit.Site = ((ListItem)cmbSite.SelectedItem).Value.ToString();
            }
            if (dtHired.Checked == true)
            {
                staffMstEntit.Date_Hired = dtHired.Value.ToString();
            }
            if (dtTransfer.Checked == true)
            {
                staffMstEntit.Transerfer_Date = dtTransfer.Value.ToString();
            }

            if (dtAcquired.Checked == true)
            {
                staffMstEntit.Date_Acquired = dtAcquired.Value.ToString();
            }

            if (dtTerminated.Checked == true)
            {
                staffMstEntit.Date_Terminated = dtTerminated.Value.ToString();
            }

            staffMstEntit.Years_in_POS = txtYearsinPosition.Text;
            staffMstEntit.Street = txtStreet.Text;
            staffMstEntit.HNo = txtHN.Text;
            staffMstEntit.Suffix = txtSuffix.Text;
            staffMstEntit.Apt = txtApt.Text;
            staffMstEntit.Floor = txtFloor.Text;
            staffMstEntit.Zip = txtZip.Text;
            staffMstEntit.Zip_Plus = txtZipPlus.Text;
            staffMstEntit.State = txtState.Text;
            staffMstEntit.HRS_Worked_PW = txtStdHours.Text;
            staffMstEntit.Weeks_Worked = txtWeeksworked.Text;
            staffMstEntit.City = txtCityName.Text;
            staffMstEntit.Position_Data = txtPosition.Text;
            staffMstEntit.Base_Rate = txtHourlyRate.Text;
            staffMstEntit.Salary = txtSalary.Text;
            staffMstEntit.Anual_Salary = txtAnnualSalary.Text;
            staffMstEntit.Workfor_CONT = chkContructed.Checked == true ? "Y" : "N";
            staffMstEntit.Workfor_HS = chkHS.Checked == true ? "Y" : "N";
            staffMstEntit.Workfor_EHS = ChkEhs.Checked == true ? "Y" : "N";

            staffMstEntit.Workfor_HS2 = chkbHS2.Checked == true ? "Y" : "N";
            staffMstEntit.Workfor_EHSCCP = chkbEHSCCP.Checked == true ? "Y" : "N";

            staffMstEntit.Workfor_NONHS = chkNonHs.Checked == true ? "Y" : "N";
            staffMstEntit.Workfor_VOL = chkVolunteer.Checked == true ? "Y" : "N";
            staffMstEntit.HS_Parent = chkHeadsp.Checked == true ? "1" : "0";
            staffMstEntit.Daycare_Parent = chkDaycarePP.Checked == true ? "1" : "0";
            staffMstEntit.EHS_Parent = chkEarlyHp.Checked == true ? "1" : "0";
            staffMstEntit.POS_Filled = chkPositionFam.Checked == true ? "1" : "0";
            staffMstEntit.Replace_SM = chkthisinirasm.Checked == true ? "1" : "0";
            staffMstEntit.Application = BaseForm.BusinessModuleID;
            staffMstEntit.LSTC_Operator = BaseForm.UserID;
            staffMstEntit.ADD_Operator = BaseForm.UserID;
            staffMstEntit.Mode = Mode;

            if (_model.STAFFData.InsertUpdateDelStaffMst(staffMstEntit))
            {
                if (Privileges.Program == "HUD99001")
                {
                    AlertBox.Show("Updated Successfully");
                }
                else
                {
                    STFMST10Control staffControl = BaseForm.GetBaseUserControl() as STFMST10Control;
                    if (staffControl != null)
                    {
                        if (Mode.Equals("Edit"))
                        {
                            AlertBox.Show("Updated successfully");
                            staffControl.RefreshGrid();
                        }
                        else
                        {
                            AlertBox.Show("Saved successfully");
                            staffControl.RefreshGrid(txtEmployNumber.Text);
                        }
                    }
                }
                
                this.Close();
            }
        }

        private bool isValidate()
        {
            bool isValid = true;


            if (String.IsNullOrEmpty(txtEmployNumber.Text))
            {
                _errorProvider.SetError(txtEmployNumber, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEmployeNumber.Text));
                isValid = false;
            }
            else
            {
                if (Convert.ToDouble(txtEmployNumber.Text) <= 0)
                {
                    _errorProvider.SetError(txtEmployNumber, Consts.Messages.Greaterthanzzero);
                    isValid = false;
                }
                else
                {

                    if (isStaffCodeExists(txtEmployNumber.Text))
                    {
                        _errorProvider.SetError(txtEmployNumber, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblEmployeNumber.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtEmployNumber, null);
                    }
                }

            }

            if (String.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                _errorProvider.SetError(txtFirstName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFirstName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFirstName, null);
            }

            if (String.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                _errorProvider.SetError(txtLastName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblLastName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtLastName, null);
            }

            if (String.IsNullOrEmpty(txtStreet.Text.Trim()))
            {
                _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtStreet, null);
            }


            if (String.IsNullOrEmpty(txtCityName.Text))
            {
                _errorProvider.SetError(txtCityName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCityName.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtCityName, null);
            }



            if (ChkEhs.Checked == false && chkNonHs.Checked == false && chkHS.Checked == false /*&& chkVolunteer.Checked == false && chkContructed.Checked == false */&& chkbEHSCCP.Checked == false && chkbHS2.Checked ==false)
            {
                _errorProvider.SetError(lblWorkFor, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblWorkFor.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(lblWorkFor, null);
            }

            if (string.IsNullOrEmpty(txtZip.Text))
            {
                _errorProvider.SetError(txtZip, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblZipCode.Text));
                isValid = false;
            }
            else if (Convert.ToDouble(txtZip.Text) <= 0)
            {
                _errorProvider.SetError(txtZip, Consts.Messages.Greaterthanzzero);
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtZip, null);
            }
            if (lblReasonReq.Visible && (((ListItem)CmbReasonfortermination.SelectedItem).Value.ToString().Equals("0")))
            {
                _errorProvider.SetError(CmbReasonfortermination, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReasonfortermination.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(CmbReasonfortermination, null);
            }

            if (lblTerminatedReq.Visible && dtTerminated.Checked == false)
            {
                _errorProvider.SetError(dtTerminated, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDateTerminated.Text));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtTerminated, null);
            }

            return isValid;
        }

        private bool isStaffCodeExists(string staffCode)
        {
            bool isExists = false;
            if (Mode.Equals("Add"))
            {
                STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
                Search_STAFFMST.Agency = BaseForm.BaseAgency;
                Search_STAFFMST.Year = BaseForm.BaseYear;
                Search_STAFFMST.Staff_Code = staffCode;
                List<STAFFMSTEntity> STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
                if (STAFFMST_List.Count > 0)
                {
                    isExists = true;
                }
            }
            return isExists;
        }



        private void btnPositions_Click(object sender, EventArgs e)
        {
            SelStaffPosCodesForm selStaffForm = new SelStaffPosCodesForm(BaseForm, Privileges, Mode, txtPosition.Text);
            selStaffForm.FormClosed += new FormClosedEventHandler(selStaffForm_FormClosed);
            selStaffForm.StartPosition = FormStartPosition.CenterScreen;
            selStaffForm.ShowDialog();
        }

        void selStaffForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SelStaffPosCodesForm form = sender as SelStaffPosCodesForm;
            if (form.DialogResult == DialogResult.OK)
            {
                txtPosition.Text = form.checkgvwApplicationData();
            }
        }

        private void btnZipSearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            btnZipSearch.Enabled = true;
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtState.Text = zipcodedetais.Zcrstate;
                    txtCityName.Text = zipcodedetais.Zcrcity;

                }
            }
            // btnCitySearch.Focus();
        }

        private void txtStdHours_TextChanged(object sender, EventArgs e)
        {
            decimal decstdHours = 0;
            decimal decWeekworked = 0;
            decimal decHourlyRate = 0;
            //decimal decAmount4 = 0;
            //decimal decAmount5 = 0;
            if (txtStdHours.Text != string.Empty)
                decstdHours = Convert.ToDecimal(txtStdHours.Text);
            if (txtWeeksworked.Text != string.Empty)
                decWeekworked = Convert.ToDecimal(txtWeeksworked.Text);
            if (txtHourlyRate.Text != string.Empty)
                decHourlyRate = Convert.ToDecimal(txtHourlyRate.Text);
            //if (txtAmount4.Text != string.Empty)
            //    decAmount4 = Convert.ToDecimal(txtAmount4.Text);
            //if (txtAmount5.Text != string.Empty)
            //    decAmount5 = Convert.ToDecimal(txtAmount5.Text);
            decimal decsalary = decstdHours * decWeekworked * decHourlyRate;
            decsalary = Math.Round(decsalary, 2);
            txtSalary.Text = decsalary.ToString();
            decimal decAnnualsalary = decstdHours * 52 * decHourlyRate;
            decAnnualsalary = Math.Round(decAnnualsalary, 2);
            txtAnnualSalary.Text = decAnnualsalary.ToString();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEmployNumber_Leave(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtEmployNumber, null);
            if (txtEmployNumber.Text != string.Empty)
            {
                txtEmployNumber.Text = "00000000".Substring(0, 8 - txtEmployNumber.Text.Length) + txtEmployNumber.Text;
                if (isStaffCodeExists(txtEmployNumber.Text))
                {
                    _errorProvider.SetError(txtEmployNumber, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblEmployeNumber.Text.Replace(Consts.Common.Colon, string.Empty)));
                    InvokeFocusCommand(txtEmployNumber);
                }
                else
                {
                    _errorProvider.SetError(txtEmployNumber, null);
                }
            }
            //chkActive.Focus();
        }

        private void STFMST10Form_Load(object sender, EventArgs e)
        {
            if (Mode.Equals("Add"))
            {
                txtEmployNumber.Focus();
            }
            else
                chkActive.Focus();
        }

        private void dtTransfer_ValueChanged(object sender, EventArgs e)
        {
            if (dtTransfer.Checked == true)
            {
                string strIntakeDate = string.Empty;
                strIntakeDate = DateTime.Now.Date.ToShortDateString();
                if (Convert.ToDateTime(dtTransfer.Text.Trim()).Date <= DateTime.Now.Date)
                {
                    txtYearsinPosition.Text = CommonFunctions.CalculationYear(Convert.ToDateTime(dtTransfer.Text.Trim()), Convert.ToDateTime(strIntakeDate));

                }
                else
                    txtYearsinPosition.Text = string.Empty;
            }
            else
            {
                txtYearsinPosition.Text = string.Empty;
            }
        }

        private void txtZipPlus_Leave(object sender, EventArgs e)
        {
            string zipPlus = txtZipPlus.Text;
            txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            btnZipSearch.Focus();
        }

        private void txtZip_Leave(object sender, EventArgs e)
        {
            string strZipCode = txtZip.Text;
            strZipCode = strZipCode.TrimStart('0');
            txtZip.Text = SetLeadingZeros(txtZip.Text);
            txtZipPlus.Text = "";
            txtZipPlus.Focus();
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seqlen = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seqlen)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }

            return (TmpCode);
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                lblReasonfortermination.Enabled = true;
                CmbReasonfortermination.Enabled = true;
                lblDateTerminated.Enabled = true;
                dtTerminated.Enabled = true;
                lblReasonReq.Visible = true;
                lblTerminatedReq.Visible = true;
                txtFirstName.Focus();
            }
            else
            {
                lblReasonfortermination.Enabled = false;
                CmbReasonfortermination.Enabled = false;
                lblDateTerminated.Enabled = false;
                dtTerminated.Enabled = false;
                dtTerminated.Checked = false;
                CmbReasonfortermination.SelectedIndex = 0;
                lblReasonReq.Visible = false;
                lblTerminatedReq.Visible = false;
                _errorProvider.SetError(dtTerminated, null);
                _errorProvider.SetError(CmbReasonfortermination, null);
                txtFirstName.Focus();
            }
        }

        private void InvokeFocusCommand(Control objControl)
        {
            //IApplicationContext objApplicationContext = this.Context as IApplicationContext;
            //if (objApplicationContext != null)
            //{
            //    objApplicationContext.SetFocused(objControl, true);
            //}
        }

        private void txtHourlyRate_Leave(object sender, EventArgs e)
        {
            if (txtHourlyRate.Text.Length > 3)
            {
                if (Convert.ToDecimal(txtHourlyRate.Text) > 999)
                {
                    txtHourlyRate.Text = "999.00000";
                }
            }
        }

        private void txtWeeksworked_Leave(object sender, EventArgs e)
        {
            btnCourseDetails.Focus();
        }


        private void DisableControls()
        {
            txtAnnualSalary.Enabled = false;
            txtApt.Enabled = false;
            txtCityName.Enabled = false;
            txtEmployNumber.Enabled = false;
            txtFirstName.Enabled = false;
            txtFloor.Enabled = false;
            txtHN.Enabled = false;
            txtHourlyRate.Enabled = false;
            txtLastName.Enabled = false;
            txtMI.Enabled = false;
            txtPosition.Enabled = false;
            txtSalary.Enabled = false;
            txtState.Enabled = false;
            txtStdHours.Enabled = false;
            txtStreet.Enabled = false;
            txtSuffix.Enabled = false;
            txtWeeksworked.Enabled = false;
            txtYearsinPosition.Enabled = false;
            txtZip.Enabled = false;
            txtZipPlus.Enabled = false;
            cmbEducationcompleted.Enabled = false;
            cmbEducationProgress.Enabled = false;
            cmbEmploymentType.Enabled = false;
            cmbEthnicity.Enabled = false;
            cmbLanguage1.Enabled = false;
            cmbLanguage2.Enabled = false;
            cmbLanguage3.Enabled = false;
            cmbPositionalctg.Enabled = false;
            cmbPrimaryLang.Enabled = false;
            cmbRace.Enabled = false;
            CmbReasonfortermination.Enabled = false;
            cmbSite.Enabled = false;
            chkActive.Enabled = false;
            chkContructed.Enabled = false;
            chkDaycarePP.Enabled = false;
            chkEarlyHp.Enabled = false;
            ChkEhs.Enabled = false;
            ChkEhs.Enabled = false;
            chkHeadsp.Enabled = false;
            chkHS.Enabled = false;
            chkNonHs.Enabled = false;
            chkPositionFam.Enabled = false;
            chkthisinirasm.Enabled = false;
            chkVolunteer.Enabled = false;
            dtAcquired.Enabled = false;
            dtHired.Enabled = false;
            dtTerminated.Enabled = false;
            dtTransfer.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnZipSearch.Visible = false;
        }

        private void btnCourseDetails_Click(object sender, EventArgs e)
        {
            if (!Mode.Equals("Add"))
            {
                STFMST10CourseDetails objCourseDetails = new STFMST10CourseDetails(BaseForm, Mode, txtEmployNumber.Text, Privileges, propstaffPostEntity);
                objCourseDetails.StartPosition = FormStartPosition.CenterScreen;
                objCourseDetails.FormClosed += new FormClosedEventHandler(On_CourseForm_Closed);
                objCourseDetails.ShowDialog();
            }
        }

        private void On_CourseForm_Closed(object sender, FormClosedEventArgs e)
        {
            staffBulkPostEntity = _model.STAFFData.GetStaffBulkPost(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, SelectStaffCodeId);

            if(staffBulkPostEntity.Count > 0)
                btnCourseDetails.Visible = true;
            else
                btnCourseDetails.Visible = false;
        }

        private void btnAppSearch_Click(object sender, EventArgs e)
        {
            SSNSearchForm SSNSearchForm = new SSNSearchForm(BaseForm, string.Empty, "H");
            SSNSearchForm.FormClosed += new FormClosedEventHandler(OnSearchFormClosed);
            SSNSearchForm.StartPosition = FormStartPosition.CenterScreen;
            SSNSearchForm.ShowDialog();
        }

        private void OnSearchFormClosed(object sender, FormClosedEventArgs e)
        {
            SSNSearchForm form = sender as SSNSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                CaseMstSnpEntity selectedSsn = form.GetSelectedRow();
                if (selectedSsn != null)
                {
                    List<CaseMstEntity> mstdetails = _model.CaseMstData.GetCaseMstadpyn(selectedSsn.Agency, selectedSsn.Dept, selectedSsn.Program, selectedSsn.Year, selectedSsn.ApplNo);
                    CaseSnpEntity snpdetails = _model.CaseMstData.GetCaseSnpDetails(selectedSsn.Agency, selectedSsn.Dept, selectedSsn.Program, selectedSsn.Year, selectedSsn.ApplNo, selectedSsn.FamilySeq);
                    txtFirstName.Text = snpdetails.NameixFi;
                    txtLastName.Text = snpdetails.NameixLast;
                    txtMI.Text = snpdetails.NameixMi;
                  
                 
                    CommonFunctions.SetComboBoxValue(cmbRace, snpdetails.Race.Trim());                 
                    CommonFunctions.SetComboBoxValue(cmbEthnicity, snpdetails.Ethnic.Trim());                 

                    if (mstdetails.Count > 0)
                    {
                        txtStreet.Text = mstdetails[0].Street;
                        txtHN.Text = mstdetails[0].Hn;
                        txtSuffix.Text = mstdetails[0].Suffix;
                        txtApt.Text = mstdetails[0].Apt;
                        txtFloor.Text = mstdetails[0].Flr;

                        txtZip.Text = mstdetails[0].Zip;
                        txtZipPlus.Text = mstdetails[0].Zipplus;
                      
                        txtCityName.Text = mstdetails[0].City;
                        txtState.Text =  mstdetails[0].State;
                        CommonFunctions.SetComboBoxValue(cmbPrimaryLang, mstdetails[0].Language.Trim());                     
                    }

                }
            }
        }

        private void STFMST10Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (e.Tool.Name == "tlHelp") { 
            }
        }

        private void lblDateTerminated_Click(object sender, EventArgs e)
        {

        }
    }


}