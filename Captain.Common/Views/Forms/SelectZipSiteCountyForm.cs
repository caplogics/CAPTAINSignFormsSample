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
using Wisej.Web;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DevExpress.XtraPrinting.Design;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using DevExpress.XtraReports.UI;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class SelectZipSiteCountyForm : Form
    {
        private ErrorProvider _errorProvider = null;
        private List<CommonEntity> _selectedcounty = null;

        private List<CommonEntity> _selectedTarget = null;
        private List<CommonEntity> _selectedRefAsoc = null;
        private List<CommonEntity> _selectedEnrollstat = null;
        private List<CaseSiteEntity> _selectedcasesite = null;
        private List<ZipCodeEntity> _selectedzipcode = null;
        private List<Csb14GroupEntity> _selectedGRPCode = null;

        private List<RCsb14GroupEntity> _RselectedGRPCode = null;
        private List<SRCsb14GroupEntity> _SRselectedGRPCode = null;

        private List<RCsb14GroupEntity> _selectedRngGRPCode = null;

        private List<CASESP1Entity> _selectedServicePlans = null;

        private List<SPCommonEntity> _Sel_FundingSource = null;
        private List<MATDEFEntity> _Sel_Scales = null;
        private CaptainModel _model = null;

        public BaseForm BaseForm { get; set; }

        public string FormType { get; set; }

        public string Ref_Frm_Date { get; set; }

        public string Ref_To_Date { get; set; }
        public string str_Agy { get; set; }

        public string strSelectionType { get; set; }

        public DataGridView HierarchieGrid
        {
            get { return gvwZipSiteCounty; }
        }

        public List<ZipCodeEntity> ListOfSelectedZipcode
        {
            get;
            set;
        }

        public List<CaseSiteEntity> ListOfSelectedCaseSite
        {
            get;
            set;
        }

        public List<CommonEntity> ListOfSelectedCommonEntity
        {
            get;
            set;
        }

        public List<AGCYPARTEntity> ListOfSelectedRefAsoc
        {
            get;
            set;
        }

        public List<SPCommonEntity> ListOfSelectedEntity
        {
            get;
            set;
        }

        public List<Csb14GroupEntity> ListOfSelectedGroupcode
        {
            get;
            set;
        }
        public List<RCsb14GroupEntity> RListOfSelectedGroupcode
        {
            get;
            set;
        }
        public List<SRCsb14GroupEntity> SRListOfSelectedGroupcode
        {
            get;
            set;
        }
        public List<RCsb14GroupEntity> ListOfSelectedRngGroupcode
        {
            get;
            set;
        }

        public List<CASESP1Entity> ListOfServicePlans
        {
            get;
            set;
        }

        public SelectZipSiteCountyForm()
        {
            InitializeComponent();
        }
        string Row_Color = "";
        public SelectZipSiteCountyForm(BaseForm baseForm, List<ZipCodeEntity> ZipcodeEntityData)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select ZIP Codes";//"CASB0004 - Select ZIP Codes";
            FormType = "ZIPCODE";
            ListOfSelectedZipcode = ZipcodeEntityData;

            this.Code.Width = 80;
            this.Description.Width = 233;
            this.Active.Width = 85;//130;
            this.Size = new System.Drawing.Size(490, 358);
            this.Active.Visible = true; this.Active.ShowInVisibilityMenu = true;
            //this.Size = new System.Drawing.Size(425, 358);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            strSelectionType = string.Empty;

            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);

            if (ListOfSelectedZipcode != null && ListOfSelectedZipcode.Count > 0)
            {
                //**zipcodeEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedZipcode.Exists(u => u.Zcrzip.Equals(item.Zcrzip)) ? "true" : "false"));

                zipcodeEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedZipcode.Exists(u => (u.Zcrzip + "-" +u.Zcrplus4).Equals(item.Zcrzip + "-" +  item.Zcrplus4))) ? "true" : "false");
            }

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //if (zipcodeEntity.Count > 0)
            //{
            //    zipcodeEntity = zipcodeEntity.OrderBy(item => item.InActive.Trim()).ToList();

            //    List<ZipCodeEntity> _checkedlst = zipcodeEntity.Where(row => row.InActiveFlag == "true").ToList();
            //    List<ZipCodeEntity> _uncheckedlst = zipcodeEntity.Where(row => row.InActiveFlag == "false").ToList();

            //    foreach (ZipCodeEntity zipdetails in _checkedlst)
            //    {
            //        string zipPlus = zipdetails.Zcrplus4.ToString();
            //        zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            //        int rowIndex = gvwZipSiteCounty.Rows.Add(zipdetails.InActiveFlag, SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity.Trim() + ", " + zipdetails.Zcrstate.ToString().Trim());
            //        gvwZipSiteCounty.Rows[rowIndex].Tag = zipdetails;
            //        CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);

            //        if (zipdetails.InActive == "Y")
            //        {
            //            gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //        }
            //    }
            //    foreach (ZipCodeEntity zipdetails in _uncheckedlst)
            //    {
            //        string zipPlus = zipdetails.Zcrplus4.ToString();
            //        zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            //        int rowIndex = gvwZipSiteCounty.Rows.Add(zipdetails.InActiveFlag, SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity.Trim() + ", " + zipdetails.Zcrstate.ToString().Trim());
            //        gvwZipSiteCounty.Rows[rowIndex].Tag = zipdetails;
            //        CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);

            //        if (zipdetails.InActive == "Y")
            //        {
            //            gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //        }
            //    }
            //}

            #endregion


            if (zipcodeEntity.Count > 0)
            {
                //zipcodeEntity = zipcodeEntity.OrderBy(item => item.InActive.Trim()).ToList();

                zipcodeEntity = zipcodeEntity.OrderByDescending(u => u.InActiveFlag).ThenBy(u => u.InActive.Trim()).ToList();

                foreach (ZipCodeEntity zipdetails in zipcodeEntity)
                {
                    string zipPlus = zipdetails.Zcrplus4.ToString();
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    int rowIndex = gvwZipSiteCounty.Rows.Add(zipdetails.InActiveFlag, SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity.Trim() + ", " + zipdetails.Zcrstate.ToString().Trim(),zipdetails.InActive=="N"?"Active":"Inactive");
                    gvwZipSiteCounty.Rows[rowIndex].Tag = zipdetails;
                    CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);

                    if (zipdetails.InActive == "Y")
                    {
                        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<ZipCodeEntity> ZipcodeEntityData, PrivilegeEntity privilege, string strZipcodeType)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = /*privilege.Program +*/ "Select ZIP Code(s)";
            FormType = "ZIPCODE";
            ListOfSelectedZipcode = ZipcodeEntityData;

            this.Code.Width = 80;
            this.Description.Width = 250;
            this.Size = new System.Drawing.Size(425, 389);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            strSelectionType = string.Empty;

            List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);

            if (strZipcodeType == "SkipSelected")
            {
                if (ListOfSelectedZipcode != null && ListOfSelectedZipcode.Count > 0)
                {
                    zipcodeEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedZipcode.Exists(u => u.Zcrzip.Equals(item.Zcrzip))) ? "false" : "true");
                }
            }
            else
            {
                if (ListOfSelectedZipcode != null && ListOfSelectedZipcode.Count > 0)
                {
                    zipcodeEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedZipcode.Exists(u => u.Zcrzip.Equals(item.Zcrzip))) ? "true" : "false");
                }
            }
            if (zipcodeEntity.Count > 0)
            {
                zipcodeEntity = zipcodeEntity.OrderBy(item => item.InActive.Trim()).ToList();
                foreach (ZipCodeEntity zipdetails in zipcodeEntity)
                {
                    string zipPlus = zipdetails.Zcrplus4.ToString();
                    zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    int rowIndex = gvwZipSiteCounty.Rows.Add(zipdetails.InActiveFlag, SetLeadingZeros(zipdetails.Zcrzip.ToString()) + "-" + zipPlus, zipdetails.Zcrcity.Trim() + ", " + zipdetails.Zcrstate.ToString().Trim());
                    gvwZipSiteCounty.Rows[rowIndex].Tag = zipdetails;
                    CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAPTAIN", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }

            return (TmpCode);
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<CaseSiteEntity> CaseSiteEntityData, string strAgency, string strDept, string strProgram, string strselectionType)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select Sites";
            ListOfSelectedCaseSite = CaseSiteEntityData;
            FormType = "CASESITE";
            strSelectionType = strselectionType;
            this.Code.Width = 80;
            this.Description.Width = 228;
            this.Active.Width = 85;//130;
            this.Size = new System.Drawing.Size(490, 358);
            //this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            this.Active.Visible = true; this.Active.ShowInVisibilityMenu = true;

            List<CaseSiteEntity> casesiteEntity = new List<CaseSiteEntity>();
            //Added by Sudheer on 03/07/2023 as per the Service Plan hierarchy
            string ACR_SERV_Hies = string.Empty;

            if (!string.IsNullOrEmpty(baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (baseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    casesiteEntity = _model.CaseMstData.GetCaseSite(strAgency, strDept, null, "SiteHie");
                else
                    casesiteEntity = _model.CaseMstData.GetCaseSite(strAgency, null, null, "SiteHie");
            }
            else
                casesiteEntity = _model.CaseMstData.GetCaseSite(strAgency, strDept, strProgram, "SiteHie");

            if (ListOfSelectedCaseSite != null && ListOfSelectedCaseSite.Count > 0)
            {
                casesiteEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedCaseSite.Exists(u => u.SiteNUMBER.Equals(item.SiteNUMBER))) ? "true" : "false");
            }

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //List<CaseSiteEntity> _checkedlst = casesiteEntity.Where(row => row.InActiveFlag == "true").ToList();
            //List<CaseSiteEntity> _uncheckedlst = casesiteEntity.Where(row => row.InActiveFlag == "false").ToList();

            //foreach (CaseSiteEntity casesite in _checkedlst)
            //{
            //    int rowIndex = gvwZipSiteCounty.Rows.Add(casesite.InActiveFlag, casesite.SiteNUMBER, casesite.SiteNAME, casesite.SiteACTIVE == "Y" ? "Active" : "Inactive", Row_Color);
            //    gvwZipSiteCounty.Rows[rowIndex].Tag = casesite;

            //    if (casesite.SiteACTIVE != "Y")
            //    {
            //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}
            //foreach (CaseSiteEntity casesite in _uncheckedlst)
            //{
            //    int rowIndex = gvwZipSiteCounty.Rows.Add(casesite.InActiveFlag, casesite.SiteNUMBER, casesite.SiteNAME, casesite.SiteACTIVE == "Y" ? "Active" : "Inactive", Row_Color);
            //    gvwZipSiteCounty.Rows[rowIndex].Tag = casesite;

            //    if (casesite.SiteACTIVE != "Y")
            //    {
            //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}

            #endregion

            if (casesiteEntity.Count > 0)
                casesiteEntity = casesiteEntity.OrderByDescending(u => u.InActiveFlag).ThenByDescending(u => u.SiteACTIVE).ToList();

            foreach (CaseSiteEntity casesite in casesiteEntity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add(casesite.InActiveFlag, casesite.SiteNUMBER, casesite.SiteNAME, casesite.SiteACTIVE == "Y" ? "Active" : "Inactive", Row_Color);
                gvwZipSiteCounty.Rows[rowIndex].Tag = casesite;

                if (casesite.SiteACTIVE != "Y")
                {
                    gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }

            }

            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<CommonEntity> CountyEntityData)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select County";
            ListOfSelectedCommonEntity = CountyEntityData;
            //cmbSite.ColorMember = "FavoriteColor";
            FormType = "COUNTY";
            strSelectionType = string.Empty;
            this.Code.Width = 80;
            this.Description.Width = 233;
            this.Active.Width = 85;//130;
            this.Size = new System.Drawing.Size(490, 358);
            //this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            this.Active.Visible = true; this.Active.ShowInVisibilityMenu = true;
            
            List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();

            if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
            {
                CountryEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }
            //CountryEntity = CountryEntity.OrderByDescending(item => item.Hierarchy).ToList();

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //List<CommonEntity> _checkedlst = CountryEntity.Where(row => row.InActiveflag == true).ToList();
            //List<CommonEntity> _uncheckedlst = CountryEntity.Where(row => row.InActiveflag == false).ToList();

            //foreach (CommonEntity country in _checkedlst)
            //{
            //    int rowIndex = gvwZipSiteCounty.Rows.Add((country.InActiveflag == null ? false : Convert.ToBoolean(country.InActiveflag)), country.Code, country.Desc, country.Hierarchy);
            //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

            //    if (country.Hierarchy == "N")
            //    {
            //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}
            //foreach (CommonEntity country in _uncheckedlst)
            //{
            //    int rowIndex = gvwZipSiteCounty.Rows.Add((country.InActiveflag == null ? false : Convert.ToBoolean(country.InActiveflag)), country.Code, country.Desc, country.Hierarchy);
            //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

            //    if (country.Hierarchy == "N")
            //    {
            //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}

            #endregion

            CountryEntity= CountryEntity.OrderByDescending(u=>u.InActiveflag).ThenByDescending(u=>u.Hierarchy).ToList();

            foreach (CommonEntity country in CountryEntity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add((country.InActiveflag == null ? false : Convert.ToBoolean(country.InActiveflag)), country.Code, country.Desc, country.Hierarchy=="Y"?"Active":"Inactive");
                gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                if (country.Hierarchy == "N")
                {
                    gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }

                //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
            }

            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        //Added by Sudheer on 05/04/2021 for Cities
        public SelectZipSiteCountyForm(BaseForm baseForm, List<CommonEntity> CountyEntityData, string City, string Citites, string sample)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select City";
            ListOfSelectedCommonEntity = CountyEntityData;
            FormType = "City";
            strSelectionType = string.Empty;
            //this.Code.Width = 80;
            this.Code.Visible = false;
            //this.Description.Width = 230;
            this.Description.Width = 310;
            this.Size = new System.Drawing.Size(425, 358);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            List<CommonEntity> CityEntity = _model.ZipCodeAndAgency.GetCity(string.Empty, string.Empty, string.Empty, "CITY");
            if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
            {
                CityEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.ToUpper().Equals(item.Code.ToUpper()))) ? true : false);
            }

            foreach (CommonEntity country in CityEntity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
                gvwZipSiteCounty.Rows[rowIndex].Tag = country;
                //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<CommonEntity> EntityData, string Target)
        {
            InitializeComponent();
            _model = new CaptainModel();
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            if (Target == "TargetPop")
            {
                this.Text = "Select Target Population";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Target";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);
                //this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();
                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "S0076", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            }
            /*else if(Target=="DenyReason")
            {
                this.Text = "Select Denied Reason";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Reason";
                strSelectionType = string.Empty;
                this.Code.Width = 100;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);
               // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();
                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "S0085", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
                commonEntity = commonEntity.OrderBy(u => u.Code).ToList();
            }*/

            else if (Target.Contains("DenyReason"))
            {
                this.Text = "Select Denied Reason";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Reason";
                strSelectionType = string.Empty;
                this.Code.Width = 100;
                this.Description.Width = 260;
                this.Size = new System.Drawing.Size(450, 365);
                this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();
                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "S0085", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
                commonEntity = commonEntity.OrderBy(u => u.Code).ToList();
                if (Target.Length > 10)
                {
                    if (int.Parse(Target.Substring(10, 4)) > 2022)
                    {
                        commonEntity.Where(w => w.Code == "01").ToList().ForEach(s => s.Desc = "Denied – Day 11 Incompletes");
                        commonEntity.Where(w => w.Code == "02").ToList().ForEach(s => s.Desc = "Denied Pre-2023 Changes");
                    }
                    else
                    {
                        commonEntity.Where(w => w.Code == "01").ToList().ForEach(s => s.Desc = "Income Incomplete");
                        commonEntity.Where(w => w.Code == "02").ToList().ForEach(s => s.Desc = "Insufficient Documents");
                    }
                }

            }
            // Murali added Serviceplan on 10/25/2021
            else if (Target == "ServicePlan")
            {
                this.Text = "Select Service Plan";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "ServicePlan";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 350;
                this.Size = new System.Drawing.Size(525, 358);
                // this.gvwZipSiteCounty.Size = new System.Drawing.Size(512, 313);
                btnOk.Location = new Point(364, 325);
                btnCancel.Location = new Point(443, 325);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();
                DataSet dsSP_Services = DatabaseLayer.SPAdminDB.Browse_CASESP0(null, null, null, null, null, null, null, null, null);
                if (dsSP_Services.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dritem in dsSP_Services.Tables[0].Rows)
                    {
                        if (dritem["SP0_PPTYPE"].ToString().Trim().ToUpper() == "B" || dritem["SP0_PPTYPE"].ToString().Trim().ToUpper() == "G" || dritem["SP0_PPTYPE"].ToString().Trim().ToUpper() == "I")
                        {
                            commonEntity.Add(new CommonEntity(dritem["SP0_SERVICECODE"].ToString().Trim(), dritem["SP0_DESCRIPTION"].ToString().Trim(), string.Empty, dritem["SP0_PPTYPE"].ToString().Trim()));
                        }
                    }
                }

            }

            else if (Target == "ReferralAssoc")
            {
                this.Text = "Referral Associations";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "RefAsoc";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 380;
                this.Size = new System.Drawing.Size(525, 358);

                btnOk.Location = new Point(364, 325);
                btnCancel.Location = new Point(443, 325);

                //dsRefAsoc = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0125", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "S0125", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            }

            #region Languages and Services Provided Added by Vikash on 09/05/2024 for HUD

            else if (Target == "Lang")
            {
                this.Text = "Select Language(s)";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Lang";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);

                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "00353", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            }
            else if (Target == "Services")
            {
                this.Text = "Select Service(s) Provided";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Services";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);

                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "99025", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            }

            #endregion

            else if (Target == "Users")
            {
                this.Text = "Select User";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Users";
                strSelectionType = string.Empty;
                //this.Code.Width = 80;
                this.Code.Visible = false;
                this.Description.Width = 310;
                this.Size = new System.Drawing.Size(425, 358);
                // this.gvwZipSiteCounty.Size = new System.Drawing.Size(512, 313);
                btnOk.Location = new Point(364, 325);
                btnCancel.Location = new Point(443, 325);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();
                DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
                DataTable dt1 = ds1.Tables[0];


                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dritem in dt1.Rows)
                    {
                        if (dritem["PWR_EMPLOYEE_NO"].ToString() != "CAPLOGICS")
                            commonEntity.Add(new CommonEntity(dritem["PWR_EMPLOYEE_NO"].ToString(), dritem["PWR_EMPLOYEE_NO"].ToString(), dritem["PWR_INACTIVE_FLAG"].ToString()));
                    }
                }
            }

            else if (Target == "Results")
            {
                this.Text = "Select Outcome Counting Control(s)";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Results";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);

                commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "03335", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            }


            List<CommonEntity> CountryEntity = new List<CommonEntity>();
            if (commonEntity.Count > 0)
            {
                if (Target == "Users")
                {
                    foreach (CommonEntity Entity in commonEntity)
                    {
                        CountryEntity.Add(new CommonEntity(Entity.Code, Entity.Desc.Trim(),Entity.Hierarchy));
                    }
                }
                else
                {
                    foreach (CommonEntity Entity in commonEntity)
                    {
                        CountryEntity.Add(new CommonEntity(Entity.Code, Entity.Desc.Trim()));
                    }
                }
            }


            if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
            {
                CountryEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }

            foreach (CommonEntity country in CountryEntity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
                gvwZipSiteCounty.Rows[rowIndex].Tag = country;
                //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
                if (Target == "Users")
                {
                    if(country.Hierarchy=="Y")
                        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor= Color.Red;
                }




            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        ////Added by Sudheer on 07/28/2021 for billing Period
        //public SelectZipSiteCountyForm(BasePage baseForm, List<CommonEntity> EntityData, string Billing,string Bundle,string None,string Space)
        //{
        //    InitializeComponent();
        //    _model = new CaptainModel();
        //    List<CommonEntity> commonEntity = new List<CommonEntity>();
        //    if (Billing == "BillingPeriod")
        //    {
        //        this.Text = "Select Billing Period";
        //        ListOfSelectedCommonEntity = EntityData;
        //        FormType = "Billing";
        //        strSelectionType = string.Empty;
        //        this.Code.Width = 80;
        //        this.Description.Width = 230;
        //        this.Size = new System.Drawing.Size(355, 354);
        //        this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
        //        //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();

        //        List<Agy_Ext_Entity> PPC_List = new List<Agy_Ext_Entity>();
        //        PPC_List = _model.SPAdminData.Get_AgyRecs_With_Ext("00202", "7", null, null, null);


        //        commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "00202", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
        //        commonEntity = commonEntity.OrderBy(u => u.Code).ToList();

        //        if (Bundle == "Y") { btnOk.Visible = false; this.Select.ReadOnly = true; } else { btnOk.Visible = true; this.Select.ReadOnly = false; }
        //    }

        //    List<CommonEntity> CountryEntity = new List<CommonEntity>();
        //    if (commonEntity.Count > 0)
        //    {
        //        foreach (CommonEntity Entity in commonEntity)
        //        {
        //            CountryEntity.Add(new CommonEntity(Entity.Code, Entity.Desc.Trim()));
        //        }
        //    }


        //    if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
        //    {
        //        CountryEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
        //    }

        //    foreach (CommonEntity country in CountryEntity)
        //    {
        //        if (Bundle == "Y")
        //        {
        //            if (country.InActiveflag == "true")
        //            {
        //                int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
        //                gvwZipSiteCounty.Rows[rowIndex].Tag = country;

        //            }
        //        }
        //        else
        //        {
        //            int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
        //            gvwZipSiteCounty.Rows[rowIndex].Tag = country;
        //        }

        //        //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
        //    }
        //}




        //Added by Sudheer on 12/06/2021 for billing Period
        public SelectZipSiteCountyForm(BaseForm baseForm, List<CommonEntity> EntityData, string Billing, string Bundle, string None, string Space)
        {
            InitializeComponent();
            _model = new CaptainModel();
            //List<CommonEntity> commonEntity = new List<CommonEntity>();
            List<Agy_Ext_Entity> commonEntity = new List<Agy_Ext_Entity>();
            if (Billing == "BillingPeriod")
            {
                this.Text = "Select Billing Period";
                ListOfSelectedCommonEntity = EntityData;
                FormType = "Billing";
                strSelectionType = string.Empty;
                this.Code.Width = 80;
                this.Description.Width = 230;
                this.Size = new System.Drawing.Size(425, 358);
                //  this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
                //List<CommonEntity> CountryEntity = _model.ZipCodeAndAgency.GetCounty();


                commonEntity = _model.SPAdminData.Get_AgyRecs_With_Ext("00202", "7", null, null, null);
                commonEntity = commonEntity.OrderBy(u => u.Ext_1).ToList();

                //commonEntity = CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "00202", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
                //commonEntity = commonEntity.OrderBy(u => u.Code).ToList();

                if (Bundle == "Y") { btnOk.Visible = false; this.Select.ReadOnly = true; } else { btnOk.Visible = true; this.Select.ReadOnly = false; }
            }

            List<CommonEntity> CountryEntity = new List<CommonEntity>();
            if (commonEntity.Count > 0)
            {
                foreach (Agy_Ext_Entity Entity in commonEntity)
                {
                    CountryEntity.Add(new CommonEntity(Entity.Code, Entity.Desc.Trim()));
                }
            }


            if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
            {
                CountryEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }

            foreach (CommonEntity country in CountryEntity)
            {
                if (Bundle == "Y")
                {
                    if (country.InActiveflag == true)
                    {
                        int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
                        gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                    }
                }
                else
                {
                    int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
                    gvwZipSiteCounty.Rows[rowIndex].Tag = country;
                }

                //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }


        public SelectZipSiteCountyForm(BaseForm baseForm, List<CommonEntity> EntityData, string AgnecyName, string EnrollStatus)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select Enroll Statuses";
            ListOfSelectedCommonEntity = EntityData;
            BaseForm = baseForm;
            FormType = "EnrollStatus";
            strSelectionType = string.Empty;
            this.Code.Width = 80;
            this.Description.Width = 230;
            this.Size = new System.Drawing.Size(425, 358);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            //List<ListItem> listItem = new List<ListItem>();
            //listItem.Add(new ListItem("Enroll", "E"));
            //if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")
            //{//  this logic value modified 10/06/2018 ask to customer
            //    listItem.Add(new ListItem("Postintake", "I"));
            //}
            //else
            //{
            //    listItem.Add(new ListItem("Accepted", "C"));
            //}
            //listItem.Add(new ListItem("Parent declined", "A"));
            //listItem.Add(new ListItem("No Longer Interested", "B"));
            //listItem.Add(new ListItem("Wait List", "L"));
            //// Newly added jan 11 2019
            //listItem.Add(new ListItem("Deferred", "F"));


            ////listItem.Add(new ListItem("Denied", "R"));
            ////listItem.Add(new ListItem("Inactive", "N"));
            ////listItem.Add(new ListItem("Pending", "P"));

            List<CommonEntity> commonEntity = new List<CommonEntity>();
            commonEntity.Add(new CommonEntity("E", "Enroll"));
            if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() != "OCO")
            {//  this logic value modified 10/06/2018 ask to customer
                commonEntity.Add(new CommonEntity("I", "Postintake"));
            }
            else
            {
                commonEntity.Add(new CommonEntity("C", "Accepted"));
            }
            commonEntity.Add(new CommonEntity("A", "Parent declined"));
            commonEntity.Add(new CommonEntity("B", "No Longer Interested"));
            commonEntity.Add(new CommonEntity("L", "Wait List"));
            // Newly added jan 11 2019
            commonEntity.Add(new CommonEntity("F", "Deferred"));

            //CommonFunctions.AgyTabsFilterCode(baseForm.BaseAgyTabsEntity, "S0076", baseForm.BaseAgency, baseForm.BaseDept, baseForm.BaseProg, "Add");
            List<CommonEntity> CountryEntity = new List<CommonEntity>();
            if (commonEntity.Count > 0)
            {
                foreach (CommonEntity Entity in commonEntity)
                {
                    CountryEntity.Add(new CommonEntity(Entity.Code, Entity.Desc.Trim()));
                }
            }


            if (ListOfSelectedCommonEntity != null && ListOfSelectedCommonEntity.Count > 0)
            {
                CountryEntity.ForEach(item => item.InActiveflag = (ListOfSelectedCommonEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
            }

            foreach (CommonEntity country in CountryEntity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add(country.InActiveflag, country.Code, country.Desc);
                gvwZipSiteCounty.Rows[rowIndex].Tag = country;
                //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);                  
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<SPCommonEntity> Entity_List, string Scr_Code, string strAgency, string strDept, string strProgram, string servicePlan, string UserID)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select Funding Source";//CASB0004
            FormType = "FUND";
            ListOfSelectedEntity = Entity_List;
            strSelectionType = string.Empty;
            this.Code.Width = 70;
            this.Description.Width = 233;
            this.Active.Width = 85;//130;
            this.Size = new System.Drawing.Size(490, 358);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            List<SPCommonEntity> FundingList = new List<SPCommonEntity>();
            this.Active.Visible = true; this.Active.ShowInVisibilityMenu = true;

            if (Scr_Code == "RNGB0014" || Scr_Code == "RNGB0004" || Scr_Code == "RNGS0014")
            {
                string ACR_SERV_Hies = string.Empty;
                if (!string.IsNullOrEmpty(baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
                {
                    if (baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                        ACR_SERV_Hies = "S";
                }

                if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
                {
                    //if (baseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    FundingList = _model.SPAdminData.GetSP0Funds(strAgency == "**" ? string.Empty : strAgency, strDept == "**" ? string.Empty : strDept, strProgram == "**" ? string.Empty : strProgram, string.Empty, "Reports", UserID);
                    //else
                    //    FundingList = _model.SPAdminData.GetSP0Funds(strAgency == "**" ? string.Empty : strAgency, string.Empty, string.Empty, string.Empty, "Reports");
                }
                else
                    FundingList = _model.SPAdminData.GetSP0Funds(strAgency == "**" ? string.Empty : strAgency, strDept == "**" ? string.Empty : strDept, strProgram == "**" ? string.Empty : strProgram, string.Empty, "Reports", UserID);

                if (ListOfSelectedEntity != null && ListOfSelectedEntity.Count > 0)
                {
                    // FundingList.ForEach(item => item.Active = (ListOfSelectedEntity.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
                    FundingList.ForEach(item => item.Sel_WS = (ListOfSelectedEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
                }

                //FundingList = FundingList.OrderByDescending(u => u.Active.Trim()).ToList();   

                FundingList = FundingList.OrderByDescending(u=>u.Sel_WS).ThenByDescending(u => u.Active.Trim()).ToList();

                # region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

                //List<SPCommonEntity> _checkedlst = FundingList.Where(row => row.Sel_WS == true).ToList();
                //List<SPCommonEntity> _uncheckedlst = FundingList.Where(row => row.Sel_WS == false).ToList();

                //foreach (SPCommonEntity country in _checkedlst)
                //{
                //    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                //    if (country.Active != "Y")
                //    {
                //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }

                //}
                //foreach (SPCommonEntity country in _uncheckedlst)
                //{
                //    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                //    if (country.Active != "Y")
                //    {
                //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }

                //}

                #endregion

                foreach (SPCommonEntity country in FundingList)
                {
                    //int rowIndex = gvwZipSiteCounty.Rows.Add((Entity_List.Count == 0 ? false : (country.Active == "true" ? true : false)), country.Code, country.Desc,country.Active=="Y"?"Active":"Inactive");
                    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                    if (country.Active != "Y")
                    {
                        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }

                    //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }
            }
            else
            {
                FundingList = _model.SPAdminData.Get_AgyRecs("Funding");

                FundingList = FundingList.OrderByDescending(u => u.Active.Trim()).ToList();

                if (ListOfSelectedEntity != null && ListOfSelectedEntity.Count > 0)
                {
                    //FundingList.ForEach(item => item.Active = (Entity_List.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
                    FundingList.ForEach(item => item.Sel_WS = (ListOfSelectedEntity.Exists(u => u.Code.Equals(item.Code))) ? true : false);
                }

                //#region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

                //List<SPCommonEntity> _checkedlst = FundingList.Where(row => row.Sel_WS == true).ToList();
                //List<SPCommonEntity> _uncheckedlst = FundingList.Where(row => row.Sel_WS == false).ToList();

                //foreach (SPCommonEntity country in _checkedlst)
                //{
                //    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                //    if (country.Active != "Y")
                //    {
                //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }

                //}
                //foreach (SPCommonEntity country in _uncheckedlst)
                //{
                //    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                //    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                //    if (country.Active != "Y")
                //    {
                //        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //    }

                //}

                //#endregion

                FundingList = FundingList.OrderByDescending(u => u.Sel_WS).ThenByDescending(u => u.Active.Trim()).ToList();
                foreach (SPCommonEntity country in FundingList)
                {
                    //int rowIndex = gvwZipSiteCounty.Rows.Add((Entity_List.Count == 0 ? false : (country.Active == "true" ? true : false)), country.Code, country.Desc);
                    int rowIndex = gvwZipSiteCounty.Rows.Add(country.Sel_WS, country.Code, country.Desc, country.Active == "Y" ? "Active" : "Inactive");
                    gvwZipSiteCounty.Rows[rowIndex].Tag = country;

                    if (country.Active != "Y")
                    {
                        gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }

                

            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<Csb14GroupEntity> GrpEntityData, string RefFDate, string RefTdate)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = /*"CASB0014 - */"Select Groups";
            FormType = "MSCODE";
            ListOfSelectedGroupcode = GrpEntityData;
            this.Select.Width = 70;//55;// 20;
            this.Code.Width = 80;// 40;
            this.Description.Width = 300;//415;
            //this.Size = new System.Drawing.Size(385, 354);
            this.Size = new System.Drawing.Size(450, 240);
            //this.gvwZipSiteCounty.Size = new System.Drawing.Size(372, 313);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(437, 200);
            //this.btnOk.Location = new System.Drawing.Point(224, 325);
            this.btnOk.Location = new System.Drawing.Point(293, 210);
            //this.btnCancel.Location = new System.Drawing.Point(303, 325);
            this.btnCancel.Location = new System.Drawing.Point(367, 210);
            strSelectionType = string.Empty;
            Ref_Frm_Date = RefFDate;
            Ref_To_Date = RefTdate;
            List<Csb14GroupEntity> GroupEntity = _model.SPAdminData.Browse_CSB14Grp(Ref_Frm_Date, Ref_To_Date, null, null, null);

            if (ListOfSelectedGroupcode != null && ListOfSelectedGroupcode.Count > 0)
            {
                GroupEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedGroupcode.Exists(u => u.GrpCode.Equals(item.GrpCode))) ? "true" : "false");
            }

            if (GroupEntity.Count > 0)
            {
                foreach (Csb14GroupEntity GrpDetails in GroupEntity)
                {
                    int rowIndex = 0;
                    //string zipPlus = zipdetails.Zcrplus4.ToString();
                    //zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    if (string.IsNullOrEmpty(GrpDetails.TblCode.Trim()))
                    {
                        rowIndex = gvwZipSiteCounty.Rows.Add(GrpDetails.InActiveFlag, GrpDetails.GrpCode, GrpDetails.GrpDesc.Trim());
                        gvwZipSiteCounty.Rows[rowIndex].Tag = GrpDetails;
                    }


                    //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }



        public SelectZipSiteCountyForm(BaseForm baseForm, List<RCsb14GroupEntity> GrpEntityData, string RefFDate, string RefTdate, string strRngMainCode, string Agy)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = /*"RNGB0014 - */"Select Domains";
            FormType = "MSCODE";
            ListOfSelectedRngGroupcode = GrpEntityData;
            this.Select.Width = 70;//55;
            this.Code.Width = 80;
            this.Description.Width = 250;
            //this.Size = new System.Drawing.Size(385, 354);
            this.Size = new System.Drawing.Size(426, 400); //240
                                                           //this.gvwZipSiteCounty.Size = new System.Drawing.Size(372, 313);
                                                           // this.gvwZipSiteCounty.Size = new System.Drawing.Size(437, 200);
                                                           //this.btnOk.Location = new System.Drawing.Point(224, 325);
                                                           //this.btnOk.Location = new System.Drawing.Point(293, 210);
                                                           //this.btnCancel.Location = new System.Drawing.Point(303, 325);
                                                           // this.btnCancel.Location = new System.Drawing.Point(367, 210);
            strSelectionType = string.Empty;
            Ref_Frm_Date = RefFDate;
            Ref_To_Date = RefTdate;
            str_Agy = Agy;
            List<RCsb14GroupEntity> GroupEntity = _model.SPAdminData.Browse_RNGGrp(strRngMainCode, str_Agy, null, null, null, baseForm.UserID, str_Agy);


            if (ListOfSelectedRngGroupcode != null && ListOfSelectedRngGroupcode.Count > 0)
            {
                GroupEntity.ForEach(item => item.InActiveFlag = (ListOfSelectedRngGroupcode.Exists(u => u.GrpCode.Equals(item.GrpCode))) ? "true" : "false");
            }
            
            GroupEntity = GroupEntity.OrderByDescending(u => u.InActiveFlag).ToList();
            if (GroupEntity.Count > 0)
            {
                foreach (RCsb14GroupEntity GrpDetails in GroupEntity)
                {
                    int rowIndex = 0;
                    //string zipPlus = zipdetails.Zcrplus4.ToString();
                    //zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    if (string.IsNullOrEmpty(GrpDetails.TblCode.Trim()) && (!string.IsNullOrEmpty(GrpDetails.GrpCode.Trim())))
                    {
                        rowIndex = gvwZipSiteCounty.Rows.Add(GrpDetails.InActiveFlag, GrpDetails.GrpCode, GrpDetails.GrpDesc.Trim());
                        gvwZipSiteCounty.Rows[rowIndex].Tag = GrpDetails;
                    }


                    //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public SelectZipSiteCountyForm(BaseForm baseForm, List<SRCsb14GroupEntity> GrpEntityData, string RefFDate, string RefTdate, string strRngMainCode, string Agy)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = /*"RNGS0014 - */"Select Domains";
            FormType = "MSCODE";
            SRListOfSelectedGroupcode = GrpEntityData;
            this.Select.Width = 70;//55;
            this.Code.Width = 80;
            this.Description.Width = 280;//300;
            //this.Size = new System.Drawing.Size(385, 354);
            this.Size = new System.Drawing.Size(450, 400);
            //this.gvwZipSiteCounty.Size = new System.Drawing.Size(372, 313);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(437, 200);
            //this.btnOk.Location = new System.Drawing.Point(224, 325);
            this.btnOk.Location = new System.Drawing.Point(293, 210);
            //this.btnCancel.Location = new System.Drawing.Point(303, 325);
            this.btnCancel.Location = new System.Drawing.Point(367, 210);
            strSelectionType = string.Empty;
            Ref_Frm_Date = RefFDate;
            Ref_To_Date = RefTdate;
            str_Agy = Agy;
            List<SRCsb14GroupEntity> GroupEntity = _model.SPAdminData.Browse_RNGSRGrp(strRngMainCode, str_Agy, null, null, null, baseForm.UserID, str_Agy/*baseForm.BaseAgency*/); //Vikash 04/12/2023 - Replaced BaseAgency with str_Agy for group filtering

            if (SRListOfSelectedGroupcode != null && SRListOfSelectedGroupcode.Count > 0)
            {
                GroupEntity.ForEach(item => item.InActiveFlag = (SRListOfSelectedGroupcode.Exists(u => u.GrpCode.Equals(item.GrpCode))) ? "true" : "false");
            }

            #region Added logic for to sort checked columns on top of a grid as per May 2023 Enhancement doc on 05/202/2023

            //if (GroupEntity.Count > 0)
            //{
            //    List<SRCsb14GroupEntity> _checkedlst = GroupEntity.Where(row => row.InActiveFlag == "true").ToList();
            //    List<SRCsb14GroupEntity> _uncheckedlst = GroupEntity.Where(row => row.InActiveFlag == "false").ToList();

            //    foreach (SRCsb14GroupEntity GrpDetails in _checkedlst)
            //    {
            //        int rowIndex = 0;
            //        //string zipPlus = zipdetails.Zcrplus4.ToString();
            //        //zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            //        if (string.IsNullOrEmpty(GrpDetails.TblCode.Trim()) && (!string.IsNullOrEmpty(GrpDetails.GrpCode.Trim())))
            //        {
            //            rowIndex = gvwZipSiteCounty.Rows.Add(GrpDetails.InActiveFlag, GrpDetails.GrpCode, GrpDetails.GrpDesc.Trim());
            //            gvwZipSiteCounty.Rows[rowIndex].Tag = GrpDetails;
            //        }
            //    }
                
            //    foreach (SRCsb14GroupEntity GrpDetails in _uncheckedlst)
            //    {
            //        int rowIndex = 0;
            //        //string zipPlus = zipdetails.Zcrplus4.ToString();
            //        //zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
            //        if (string.IsNullOrEmpty(GrpDetails.TblCode.Trim()) && (!string.IsNullOrEmpty(GrpDetails.GrpCode.Trim())))
            //        {
            //            rowIndex = gvwZipSiteCounty.Rows.Add(GrpDetails.InActiveFlag, GrpDetails.GrpCode, GrpDetails.GrpDesc.Trim());
            //            gvwZipSiteCounty.Rows[rowIndex].Tag = GrpDetails;
            //        }
            //    }
            //}
            #endregion


            if (GroupEntity.Count > 0)
            {
                GroupEntity= GroupEntity.OrderByDescending(u=>u.InActiveFlag).ToList();

                foreach (SRCsb14GroupEntity GrpDetails in GroupEntity)
                {
                    int rowIndex = 0;
                    //string zipPlus = zipdetails.Zcrplus4.ToString();
                    //zipPlus = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    if (string.IsNullOrEmpty(GrpDetails.TblCode.Trim()) && (!string.IsNullOrEmpty(GrpDetails.GrpCode.Trim())))
                    {
                        rowIndex = gvwZipSiteCounty.Rows.Add(GrpDetails.InActiveFlag, GrpDetails.GrpCode, GrpDetails.GrpDesc.Trim());
                        gvwZipSiteCounty.Rows[rowIndex].Tag = GrpDetails;
                    }


                    //CommonFunctions.setTooltip(rowIndex, zipdetails.Zcraddoperator, zipdetails.Zcrdateadd, zipdetails.Zcrlstcoperator, zipdetails.Zcrdatelstc, gvwZipSiteCounty);
                }
            }
            else
            {
                AlertBox.Show(Consts.Messages.Recordsornotfound, MessageBoxIcon.Warning);
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }


        public SelectZipSiteCountyForm(BaseForm baseForm, string Scr_Code, string Matrix_Code, List<MATDEFEntity> Entity_List)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = /*Scr_Code + " - */"Select Scale(s)";
            FormType = "SCALE";
            strSelectionType = string.Empty;
            this.Code.Width = 80;
            this.Description.Width = 250;
            this.Size = new System.Drawing.Size(425, 356);
            // this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);

            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Mat_Code = Matrix_Code;
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");
            if (matdefEntity.Count > 0)
            {
                bool Scale_Selected = false;
                foreach (MATDEFEntity matdef in matdefEntity)
                {
                    if (matdef.Scale_Code != "0")
                    {
                        Scale_Selected = false;
                        foreach (MATDEFEntity Ent in Entity_List)
                        {
                            if (Ent.Scale_Code == matdef.Scale_Code)
                            {
                                Scale_Selected = true;
                                break;
                            }
                        }

                        int rowIndex = gvwZipSiteCounty.Rows.Add(Scale_Selected, matdef.Scale_Code, matdef.Desc);
                        gvwZipSiteCounty.Rows[rowIndex].Tag = matdef;
                    }
                }
            }
            if (gvwZipSiteCounty.Rows.Count > 0)
            { btnCancel.Visible = btnOk.Visible = true; gvwZipSiteCounty.Rows[0].Selected = true; }
            else
                btnCancel.Visible = btnOk.Visible = false;

        }

        //added by Sudheer to filter Service Plans
        public SelectZipSiteCountyForm(BaseForm baseForm, List<CASESP1Entity> CaseSPEntityData, string strAgency, string strDept, string strProgram)
        {
            InitializeComponent();
            _model = new CaptainModel();
            this.Text = "Select Service Plans";
            ListOfServicePlans = CaseSPEntityData;
            FormType = "CASESP";
            //strSelectionType = strselectionType;
            this.Code.Width = 80;
            this.Description.Width = 240;
            this.Active.Width = 85;//130;
            this.Size = new System.Drawing.Size(500, 358);
            //this.gvwZipSiteCounty.Size = new System.Drawing.Size(342, 313);
            this.Active.Visible = true; this.Active.ShowInVisibilityMenu = true;
            chkbActive.Visible = true; chkbInactive.Visible = true;

            List<CASESP1Entity> SP_Hierarchies = new List<CASESP1Entity>();

            string ACR_SERV_Hies = string.Empty;

            if (!string.IsNullOrEmpty(baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim()))
            {
                if (baseForm.BaseAgencyControlDetails.ServicePlanHiecontrol.Trim() == "Y")
                    ACR_SERV_Hies = "S";
            }

            if (ACR_SERV_Hies == "Y" || ACR_SERV_Hies == "S")
            {
                if (baseForm.BaseAgencyControlDetails.SerPlanAllow.Trim() == "D")
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, strAgency == "**" ? null : strAgency, strDept == "**" ? null : strDept, null, baseForm.UserID);
                else
                    SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, strAgency == "**" ? null : strAgency, null, null, baseForm.UserID);
            }
            else
            {
                SP_Hierarchies = _model.SPAdminData.Browse_CASESP1(null, strAgency == "**" ? null : strAgency, strDept == "**" ? null : strDept, strProgram == "**" ? null : strProgram);
                //DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP0(null, null, null, null, null, null, null, null, null);

                //if (ds.Tables[0].Rows.Count>0)
                //{
                //    foreach(DataRow dr in ds.Tables[0].Rows)
                //    {
                //        SP_Hierarchies.Add(new CASESP1Entity(dr["sp0_servicecode"].ToString(), dr["sp0_description"].ToString(), dr["SP0_ACTIVE"].ToString(),string.Empty));
                //    }    
                //}

                //SP_Hierarchies = _model.SPAdminData.CASESP1_SerPlans(null, strAgency == "**" ? null : strAgency, strDept == "**" ? null : strDept, strProgram == "**" ? null : strProgram, baseForm.UserID);
            }

            if (ListOfServicePlans != null && ListOfServicePlans.Count > 0)
            {
                SP_Hierarchies.ForEach(item => item.Sel_Type = (ListOfServicePlans.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
            }

            SP_Hierarchies = SP_Hierarchies.OrderByDescending(U => U.Sp0_Active).ToList();

            foreach (CASESP1Entity entity in SP_Hierarchies)
            {
                //if (string.IsNullOrEmpty(entity.Sel_Type.Trim())) entity.Sel_Type = "";

                int rowIndex = gvwZipSiteCounty.Rows.Add(entity.Sel_Type == "" ? "false" : entity.Sel_Type, entity.Code, entity.SP_Desc, entity.Sp0_Active == "Y" ? "Active" : "Inactive");
                gvwZipSiteCounty.Rows[rowIndex].Tag = entity;

                if (entity.Sp0_Active == "N")
                    gvwZipSiteCounty.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

            }
            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        #region Added by Vikash on 01/08/2025 for Service Type search form in In-Kind Report
        public string Agency
        {
            get;set;
        }
        public string Department
        {
            get; set;
        }
        public string Program
        {
            get; set;
        }
        public string ProgYear
        {
            get; set;
        }

        List<CommonEntity> servtypes = new List<CommonEntity>();
        public SelectZipSiteCountyForm(BaseForm baseForm, PrivilegeEntity privilegeEntity, string strAgency, string strDept, string strProgram, string Year, List<CommonEntity> ServTypes)
        {
            InitializeComponent();

            _model = new CaptainModel();
            this.Text = "Select Service Type";
            servtypes = ServTypes;
            this.Code.Width = 80;
            this.Description.Width = 300;
            this.Size = new System.Drawing.Size(490, 358);
            this.Active.Visible = false;
            this.Active.ShowInVisibilityMenu = false;
            BaseForm = baseForm;

            Agency = strAgency;
            Department = strDept;
            Program = strProgram;
            ProgYear = Year;

            List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "05558", Agency, Department, Program, string.Empty);

            foreach (CommonEntity servTypes in Ethnicity)
            {
                int rowIndex = gvwZipSiteCounty.Rows.Add(false, servTypes.Code, servTypes.Desc);

                CommonEntity checkSel = ServTypes.Find(x => x.Code == servTypes.Code);

                if (checkSel != null)
                    gvwZipSiteCounty.Rows[rowIndex].Cells["Select"].Value = true;

                gvwZipSiteCounty.Rows[rowIndex].Tag = servTypes;
            }

            if (gvwZipSiteCounty.Rows.Count > 0)
                btnCancel.Visible = btnOk.Visible = true;
            else
                btnCancel.Visible = btnOk.Visible = false;
        }

        public List<CommonEntity> GetSelectedServTypes()
        {
            List<CommonEntity> sel_ServTypes_List = new List<CommonEntity>();

            foreach (DataGridViewRow dr in gvwZipSiteCounty.Rows)
            {
                if (dr.Cells["Select"].Value.ToString() == "True")
                {
                    CommonEntity ServType_Entity = dr.Tag as CommonEntity;
                    sel_ServTypes_List.Add(ServType_Entity);
                }
            }
            return sel_ServTypes_List;
        }
        #endregion

        public List<UserEntity> SelectedUsers = new List<UserEntity>();
        


        /// <summary>
        /// Adds the event handlers to the grid
        /// </summary>
        private void AddGridEventHandles()
        {
            //gvwHierarchie.DataError += new Wisej.Web.DataGridViewDataErrorEventHandler(DataGridViewDataError);
            //gvwHierarchie.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }

        /// <summary>
        /// Removes the event handlers from the grid.
        /// </summary>
        private void RemoveGridEventHandles()
        {
            //gvwHierarchie.DataError -= new DataGridViewDataErrorEventHandler(DataGridViewDataError);
            //gvwHierarchie.CellValueChanged -= new DataGridViewCellEventHandler(DataGridViewCellValueChanged);
        }

        private void EnableDisableCheckBox()
        {
            //if (SelectedHierarchies.Count == 0) { return; }
            //foreach (HierarchyEntity hierarchyEntity in SelectedHierarchies)
            //{
            //    string selectedHIE = hierarchyEntity.Code;

            //    if (selectedHIE.IndexOf("**") > 0 || selectedHIE.Equals("**-**-**"))
            //    {
            //        string selectedHierarchy = selectedHIE.Replace("-**", string.Empty);
            //        foreach (DataGridViewRow dr in gvwHierarchie.Rows)
            //        {
            //            string rowCode = dr.Cells["Code"].Value.ToString();
            //            if (selectedHIE.Equals("**-**-**") && !rowCode.Equals(selectedHIE))
            //            {
            //                dr.Cells["Select"].ReadOnly = true;
            //                dr.DefaultCellStyle.ForeColor = Color.LightGray;
            //            }
            //            else if (rowCode.StartsWith(selectedHierarchy) && !rowCode.Equals(selectedHIE))
            //            {
            //                dr.Cells["Select"].ReadOnly = true;
            //                dr.DefaultCellStyle.ForeColor = Color.LightGray;
            //            }
            //        }
            //    }
            //    gvwHierarchie.Update();
            //    gvwHierarchie.ResumeLayout();
        }

        public List<CommonEntity> SelectedCountyEntity
        {
            get
            {
                return _selectedcounty = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedUserEntity
        {
            get
            {
                return _selectedcounty = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedCityEntity
        {
            get
            {
                return _selectedcounty = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedTargetEntity
        {
            get
            {
                return _selectedTarget = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedResultsEntity
        {
            get
            {
                return _selectedTarget = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedRefAscoEntity
        {
            get
            {
                return _selectedRefAsoc = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                          where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                          select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CommonEntity> SelectedEnrollStatusEntity
        {
            get
            {
                return _selectedEnrollstat = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                              where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                              select ((DataGridViewRow)c).Tag as CommonEntity).ToList();

            }
        }

        public List<CaseSiteEntity> SelectedCaseSiteEntity
        {
            get
            {
                return _selectedcasesite = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                            where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                            select ((DataGridViewRow)c).Tag as CaseSiteEntity).ToList();

            }
        }

        public List<ZipCodeEntity> SelectedZipcodeEntity
        {
            get
            {
                return _selectedzipcode = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                           where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                           select ((DataGridViewRow)c).Tag as ZipCodeEntity).ToList();

            }
        }

        public List<ZipCodeEntity> SelectedZipcodeSkipEntity
        {
            get
            {
                return _selectedzipcode = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                           where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.False, StringComparison.CurrentCultureIgnoreCase))
                                           select ((DataGridViewRow)c).Tag as ZipCodeEntity).ToList();

            }
        }

        public List<Csb14GroupEntity> SelectedGroupCodeEntity
        {
            get
            {
                return _selectedGRPCode = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                           where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                           select ((DataGridViewRow)c).Tag as Csb14GroupEntity).ToList();

            }
        }

        public List<RCsb14GroupEntity> SelectedRngGroupCodeEntity
        {
            get
            {
                return _selectedRngGRPCode = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                              where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                              select ((DataGridViewRow)c).Tag as RCsb14GroupEntity).ToList();

            }
        }
        public List<SRCsb14GroupEntity> SelectedSRGroupCodeEntity
        {
            get
            {
                return _SRselectedGRPCode = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                             where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                             select ((DataGridViewRow)c).Tag as SRCsb14GroupEntity).ToList();

            }
        }

        public List<CASESP1Entity> SelectedServicePlanEntity
        {
            get
            {
                return _selectedServicePlans = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                                where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                select ((DataGridViewRow)c).Tag as CASESP1Entity).ToList();

            }
        }



        public List<SPCommonEntity> Get_Sel_Fund_List
        {
            get
            {
                return _Sel_FundingSource = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                             where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                             select ((DataGridViewRow)c).Tag as SPCommonEntity).ToList();

            }
        }

        public List<MATDEFEntity> Get_Sel_Scales_List
        {
            get
            {
                return _Sel_Scales = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                      where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                      select ((DataGridViewRow)c).Tag as MATDEFEntity).ToList();

            }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (FormType == "ZIPCODE")
            {
                bool boolPrimary = true;
                List<DataGridViewRow> SelectedgvRows = (from c in gvwZipSiteCounty.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (c.Cells["Select"].Value.ToString().ToUpper().Equals("TRUE"))
                                                        select c).ToList();
                if (SelectedgvRows.Count <= 30)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    AlertBox.Show("You can Select a Maximum of 30 ZIP Codes", MessageBoxIcon.Warning);
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void gvwZipSiteCounty_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (strSelectionType == "1")
            {
                string selectedHIE = gvwZipSiteCounty.SelectedRows[0].Cells["Code"].Value.ToString();
                foreach (DataGridViewRow item in gvwZipSiteCounty.Rows)
                {
                    string rowCode = item.Cells["Code"].Value.ToString();
                    if (!rowCode.Equals(selectedHIE))
                    {
                        item.Cells["Select"].Value = "false";
                    }
                }
            }
        }

        private void chkbActive_CheckedChanged(object sender, EventArgs e)
        {
            if (gvwZipSiteCounty.Rows.Count > 0)
            {
                if (chkbActive.Checked)
                {
                    foreach (DataGridViewRow dr in gvwZipSiteCounty.Rows)
                    {
                        if (dr.Cells["Active"].Value.ToString().Trim() == "Active")
                        {
                            dr.Cells["Select"].Value = true;
                        }
                    }

                }
                else
                {
                    foreach (DataGridViewRow dr in gvwZipSiteCounty.Rows)
                    {
                        if (dr.Cells["Active"].Value.ToString().Trim() == "Active")
                        {
                            dr.Cells["Select"].Value = false;
                        }
                    }
                }
            }

        }

        private void chkbInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (gvwZipSiteCounty.Rows.Count > 0)
            {
                if (chkbInactive.Checked)
                {
                    foreach (DataGridViewRow dr in gvwZipSiteCounty.Rows)
                    {
                        if (dr.Cells["Active"].Value.ToString().Trim() == "Inactive")
                        {
                            dr.Cells["Select"].Value = true;
                        }
                    }

                }
                else
                {
                    foreach (DataGridViewRow dr in gvwZipSiteCounty.Rows)
                    {
                        if (dr.Cells["Active"].Value.ToString().Trim() == "Inactive")
                        {
                            dr.Cells["Select"].Value = false;
                        }
                    }
                }
            }
        }
        private void gvwZipSiteCounty_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
