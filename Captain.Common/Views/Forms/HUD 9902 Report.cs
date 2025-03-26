using Aspose.Cells.Drawing;
using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.ExpressApp.Validation.AllContextsView;
using DevExpress.XtraRichEdit.Model;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HUD_9902_Report : Form
    {
        #region Private Variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public HUD_9902_Report(BaseForm baseform, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseform = baseform;
            _privilegeEntity = privilegeEntity;

            Agency = _baseform.BaseAgency;
            Depart = _baseform.BaseDept;
            Program = _baseform.BaseProg;
            Program_Year = _baseform.BaseYear;

            Set_Report_Hierarchy(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();

            hudCntlData = _model.HUDCNTLData.GetHUDCNTL(_baseform.BaseAgency);

            fillForm();
        }


        #region Properties

        public BaseForm _baseform
        {
            get; set;
        }

        public PrivilegeEntity _privilegeEntity
        {
            get; set;
        }
        public string propReportPath
        {
            get; set;
        }
        public string Agency
        {
            get; set;
        }
        public string Depart
        {
            get; set;
        }
        public string Program
        {
            get; set;
        }

        public List<HUDCNTLEntity> hudCntlData
        {
            get; set;
        }

        public List<HUDINDIVENTITY> hudIndvData
        {
            get; set;
        }

        #endregion

        #region Hierarchy Code

        string Program_Year;
        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(_baseform, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    Agency = hierarchy.Substring(0, 2);
                    Depart = hierarchy.Substring(2, 2);
                    Program = hierarchy.Substring(4, 2);

                }

            }
        }

        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(760, 25);
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }
            Agency = Agy;
            Depart = Dept;
            Program = Prog;
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(680, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(760, 25);
        }

        #endregion

        private void fillForm()
        {
            fillFiscalYear_Combo();

            this.cmbRepQuart.SelectedIndexChanged -= new System.EventHandler(this.cmbRepQuart_SelectedIndexChanged);
            fillReportQuart_Combo();
            

            if (!string.IsNullOrEmpty(hudCntlData[0].RPB_YEAR))
                CommonFunctions.SetComboBoxValue(cmbFiscYear, "1");

            if (DateTime.Now.Date >= Convert.ToDateTime("01/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()) && DateTime.Now.Date <= Convert.ToDateTime("03/31/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()))
            {
                CommonFunctions.SetComboBoxValue(cmbRepQuart, "1");

                //dtpFrmDte.Text = Convert.ToDateTime("01/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
                //dtpToDte.Text = Convert.ToDateTime("03/31/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("04/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()) && DateTime.Now.Date <= Convert.ToDateTime("06/30/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()))
            {
                CommonFunctions.SetComboBoxValue(cmbRepQuart, "2");

                //dtpFrmDte.Text = Convert.ToDateTime("04/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
                //dtpToDte.Text = Convert.ToDateTime("06/30/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("07/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()) && DateTime.Now.Date <= Convert.ToDateTime("09/30/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()))
            {
                CommonFunctions.SetComboBoxValue(cmbRepQuart, "3");

                //dtpFrmDte.Text = Convert.ToDateTime("07/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
                //dtpToDte.Text = Convert.ToDateTime("09/30/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
            }
            else if (DateTime.Now.Date >= Convert.ToDateTime("10/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()) && DateTime.Now.Date <= Convert.ToDateTime("12/31/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()))
            {
                CommonFunctions.SetComboBoxValue(cmbRepQuart, "4");

                //dtpFrmDte.Text = Convert.ToDateTime("10/01/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
                //dtpToDte.Text = Convert.ToDateTime("12/31/" + ((ListItem)cmbFiscYear.SelectedItem).Text.ToString()).ToString("MM/dd/yyyy");
            }

            this.cmbRepQuart.SelectedIndexChanged += new System.EventHandler(this.cmbRepQuart_SelectedIndexChanged);

            cmbRepQuart_SelectedIndexChanged(cmbRepQuart, EventArgs.Empty);

            fillHUDCounts_Grid();
        }

        private void fillFiscalYear_Combo()
        {
            cmbFiscYear.Items.Clear();

            cmbFiscYear.Items.Add(new ListItem("", "0"));
            cmbFiscYear.Items.Add(new ListItem(hudCntlData[0].RPB_YEAR, "1"));

            if (cmbFiscYear.Items.Count > 0)
                cmbFiscYear.SelectedIndex = 0;
        }

        List<CommonEntity> quarter = new List<CommonEntity>();
        private void fillReportQuart_Combo()
        {
            cmbRepQuart.Items.Clear();

            cmbRepQuart.Items.Add(new ListItem("Q1", 1));
            cmbRepQuart.Items.Add(new ListItem("Q2", 2));
            cmbRepQuart.Items.Add(new ListItem("Q3", 3));
            cmbRepQuart.Items.Add(new ListItem("Q4", 4));

            if (cmbRepQuart.Items.Count > 0)
                cmbRepQuart.SelectedIndex = 0;

            quarter.Add(new CommonEntity { Code = "1", Desc = "Q1" });
            quarter.Add(new CommonEntity { Code = "2", Desc = "Q2" });
            quarter.Add(new CommonEntity { Code = "3", Desc = "Q3" });
            quarter.Add(new CommonEntity { Code = "4", Desc = "Q4" });
        }

        List<CommonEntity> AppEthnicityList = new List<CommonEntity>();
        List<CommonEntity> AppRaceList = new List<CommonEntity>();
        List<CommonEntity> IncLevelList = new List<CommonEntity>();
        List<CommonEntity> RurAreaList = new List<CommonEntity>();
        List<CommonEntity> LimLangList = new List<CommonEntity>();

        List<CommonEntity> PurVisitList = new List<CommonEntity>();

        List<CommonEntity> ImpactsList = new List<CommonEntity>();
        public void FillImpacts()
        {

            ImpactsList.Add(new CommonEntity("1", "Households that received one-on-one counseling that also received education services."));
            ImpactsList.Add(new CommonEntity("2", "Households that received information fair housing, fair lending and/or accessibility rights."));
            ImpactsList.Add(new CommonEntity("3", "Households for whom counselor developed a budget customized to a client's current situation."));
            ImpactsList.Add(new CommonEntity("4", "Households that improved their financial capacity (e.g. increased discretionary income, decreased debt load, increased savings, increased credit score, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("5", "Households that gained access to resources to help improve their housing situation (e.g. down payment assistance, rental assistance, utility assistance, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("6", "Households that gained access to non-housing resources (e.g. social service programs, legal services, public benefits such as Social Security or Medicaid, etc.) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("7", "Homeless or potentially homeless households that obtained temporary or permanent housing after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("8", "Households that gained access to disaster recovery non-housing resources after receiving Housing Counseling Services (e.g. Red Cross/FEMA relief items, legal services, assistance)."));
            ImpactsList.Add(new CommonEntity("9", "Households obtained disaster recovery housing resources after receiving Housing Counseling Services (e.g. temporary shelter, homeowner rehab, relocation, etc."));
            ImpactsList.Add(new CommonEntity("10", "Households for whom counselor developed or updated an emergency preparedness plan."));
            ImpactsList.Add(new CommonEntity("11", "Household that received rental counseling and avoided eviction after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("12", "Household that received rental counseling and improved living conditions after receiving Housing Counseling Services"));
            ImpactsList.Add(new CommonEntity("13", "Household that received pre-purchase/homebuying counseling and purchased housing after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("14", "Household that received reverse mortgage counseling and obtained a Home Enquiry Conversion Mortgage (HECM) after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("15", "Household that received non-delinquency post-purchase counseling that were able to improve home conditions or home affordability after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("16", "Household that prevented or resolved a forward mortgage default after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("17", "Household that prevented or resolved a reverse mortgage default after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("18", "Household that received a forward mortgage modification and remained current in their modified mortgage after receiving Housing Counseling Services."));
            ImpactsList.Add(new CommonEntity("19", "Household that received a forward mortgage modification and improved their financial capacity after receiving Housing Counseling Services."));
        }

        private void dgvHUDCounts_CellPaint(object sender, DataGridViewCellPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
              
            }
        }

        private void fillHUDCounts_Grid()
        {
            hudIndvData = _model.HUDCNTLData.GetHUDINDIV(Agency, Depart, Program, Program_Year, "", "", "");

            dgvHUDCounts.Rows.Clear();

            int rowIndex = 0;

            #region Fill Hardcoded Data

            #region Ethnicity

            AppEthnicityList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, Consts.AgyTab.ETHNICODES, _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, "");

            rowIndex = dgvHUDCounts.Rows.Add("3. Ethnicity", "", "", "H", "", "00352","");

            foreach (CommonEntity entity in AppEthnicityList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode,"");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 3 Total", "", "", "T", "", "00352", "");

            #endregion

            #region Race

            AppRaceList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, Consts.AgyTab.RACE, _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, "");

            rowIndex = dgvHUDCounts.Rows.Add("4. Race", "", "", "H", "", "00003", "");

            foreach (CommonEntity entity in AppRaceList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode, "");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 4 Total", "", "", "T", "", "00003", "");

            #endregion

            #region Income Levels

            IncLevelList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0305", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            rowIndex = dgvHUDCounts.Rows.Add("5. Income Levels", "", "", "H", "", "S0305", "");

            foreach (CommonEntity entity in IncLevelList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode, "");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 5 Total", "", "", "T", "", "S0305", "");

            #endregion

            #region Rural Area Status

            RurAreaList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0309", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            rowIndex = dgvHUDCounts.Rows.Add("6. Rural Area Status", "", "", "H", "", "S0309", "");

            foreach (CommonEntity entity in RurAreaList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode, "");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 6 Total", "", "", "T", "", "S0309", "");

            #endregion

            #region Limited English Proficiency Status

            LimLangList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0310", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            rowIndex = dgvHUDCounts.Rows.Add("7. Limited English Proficiency Status", "", "", "H", "", "S0310", "");

            foreach (CommonEntity entity in LimLangList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode, "");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 7 Total", "", "", "T", "", "S0310", "");

            #endregion

            #region Households Receiving Eductaion Services (Including Online Education), by Purpose

            //RurAreaList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0309", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            rowIndex = dgvHUDCounts.Rows.Add("8. Households Receiving Eductaion Services (Including Online Education), by Purpose", "", "", "H", "", "","");

            rowIndex = dgvHUDCounts.Rows.Add("a. " + "Completed financial literacy workshop, including home affordability, budgeting and understanding use of credit", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("b. " + "Completed predatory lending, loan scam or other fraud prevention workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("c. " + "Completed fair housing workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("d. " + "Completed homelessness prevention workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("e. " + "Completed rental workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("f. " + "Completed pre-purcahse homebuyer education workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("g. " + "Completed non-delinquency post-purchase workshop, including home maintenance and/or financial managament for homeowners", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("h. " + "Completed resolving or preventing mortgage delinquency workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("i. " + "Completed disaster preparedness assistance workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("j. " + "Completed disaster recovery assistance workshop", "", "", "D", "", "", "");

            rowIndex = dgvHUDCounts.Rows.Add("Section 8 Total", "", "", "T", "", "", "");

            //foreach (CommonEntity entity in RurAreaList)
            //{
            //    dgvHUDCounts.Rows.Add("       " + entity.Desc, "", "");
            //}

            #endregion

            #region Households Receiving One-on-One Counseling by Purpose

            PurVisitList = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0301", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            rowIndex = dgvHUDCounts.Rows.Add("9. Households Receiving One-on-One Counseling by Purpose", "", "", "H", "", "S0301", "");

            foreach (CommonEntity entity in PurVisitList)
            {
                rowIndex = dgvHUDCounts.Rows.Add(entity.Desc, "", "", "D", entity.Code, entity.AgyCode, "");
            }

            rowIndex = dgvHUDCounts.Rows.Add("Section 9 Total", "", "", "T", "", "S0301", "");

            #endregion

            #region Outcome of One-on-One Counseling Services

            rowIndex = dgvHUDCounts.Rows.Add("10. Outcome of One-on-One Counseling Services", "", "", "H", "", "", "0");

            rowIndex = dgvHUDCounts.Rows.Add("a. " + "Households that received one-on-one counseling that also received education services.", "", "", "D", "", "", "1");

            rowIndex = dgvHUDCounts.Rows.Add("b. " + "Households that received information fair housing, fair lending and/or accessibility rights.", "", "", "D", "", "", "2");

            rowIndex = dgvHUDCounts.Rows.Add("c. " + "Households for whom counselor developed a budget customized to a client's current situation.", "", "", "D", "", "", "3");

            rowIndex = dgvHUDCounts.Rows.Add("d. " + "Households that improved their financial capacity (e.g. increased discretionary income, decreased debt load, increased savings, increased credit score, etc.) after receiving Housing Counseling Services.", "", "", "D", "", "", "4");

            rowIndex = dgvHUDCounts.Rows.Add("e. " + "Households that gained access to resources to help improve their housing situation (e.g. down payment assistance, rental assistance, utility assistance, etc.) after receiving Housing Counseling Services.", "", "", "D", "", "", "5");

            rowIndex = dgvHUDCounts.Rows.Add("f. " + "Households that gained access to non-housing resources (e.g. social service programs, legal services, public benefits such as Social Security or Medicaid, etc.) after receiving Housing Counseling Services.", "", "", "D", "", "", "6");

            rowIndex = dgvHUDCounts.Rows.Add("g. " + "Homeless or potentially homeless households that obtained temporary or permanent housing after receiving Housing Counseling Services.", "", "", "D", "", "", "7");

            rowIndex = dgvHUDCounts.Rows.Add("h. " + "Households that gained access to disaster recovery non-housing resources after receiving Housing Counseling Services (e.g. Red Cross/FEMA relief items, legal services, assistance).", "", "", "D", "", "", "8");

            rowIndex = dgvHUDCounts.Rows.Add("i. " + "Households obtained disaster recovery housing resources after receiving Housing Counseling Services (e.g. temporary shelter, homeowner rehab, relocation, etc.", "", "", "D", "", "", "9");

            rowIndex = dgvHUDCounts.Rows.Add("j. " + "Households for whom counselor developed or updated an emergency preparedness plan.", "", "", "D", "", "", "10");

            rowIndex = dgvHUDCounts.Rows.Add("k. " + "Household that received rental counseling and avoided eviction after receiving Housing Counseling Services.", "", "", "D", "", "", "11");

            rowIndex = dgvHUDCounts.Rows.Add("l. " + "Household that received rental counseling and improved living conditions after receiving Housing Counseling Services", "", "", "D", "", "", "12");

            rowIndex = dgvHUDCounts.Rows.Add("m. " + "Household that received pre-purchase/homebuying counseling and purchased housing after receiving Housing Counseling Services.", "", "", "D", "", "", "13");

            rowIndex = dgvHUDCounts.Rows.Add("n. " + "Household that received reverse mortgage counseling and obtained a Home Enquiry Conversion Mortgage (HECM) after receiving Housing Counseling Services.", "", "", "D", "", "", "14");

            rowIndex = dgvHUDCounts.Rows.Add("o. " + "Household that received non-delinquency post-purchase counseling that were able to improve home conditions or home affordability after receiving Housing Counseling Services.", "", "", "D", "", "", "15");

            rowIndex = dgvHUDCounts.Rows.Add("p. " + "Household that prevented or resolved a forward mortgage default after receiving Housing Counseling Services.", "", "", "D", "", "", "16");

            rowIndex = dgvHUDCounts.Rows.Add("q. " + "Household that prevented or resolved a reverse mortgage default after receiving Housing Counseling Services.", "", "", "D", "", "", "17");

            rowIndex = dgvHUDCounts.Rows.Add("r. " + "Household that received a forward mortgage modification and remained current in their modified mortgage after receiving Housing Counseling Services.", "", "", "D", "", "", "18");

            rowIndex = dgvHUDCounts.Rows.Add("s. " + "Household that received a forward mortgage modification and improved their financial capacity after receiving Housing Counseling Services.", "", "", "D", "", "", "19");

            rowIndex = dgvHUDCounts.Rows.Add("Section 10 Total", "", "", "T", "", "", "0");

            #endregion

            #endregion

            int impactCount = 0;
            int CatgTotals = 0,impactTotal = 0;
            string prevImpact = string.Empty;

            foreach (DataGridViewRow dr in dgvHUDCounts.Rows)
            {
                rowIndex = dr.Index;

                if (dr.Cells["gvRowType"].Value.ToString() == "D")
                {
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.Padding = new Padding(40, 0, 0, 0);
                }

                if (dr.Cells["gvRowType"].Value.ToString() == "H")
                {
                    dgvHUDCounts.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");

                    dgvHUDCounts.Rows[rowIndex].Cells["gvAllActivities"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");
                    dgvHUDCounts.Rows[rowIndex].Cells["gvFundedActivities"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");
                }

                if (dr.Cells["gvRowType"].Value.ToString() == "T")
                {
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvHUDCounts.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");
                    dgvHUDCounts.Rows[rowIndex].Cells["gvCategories"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");

                    dgvHUDCounts.Rows[rowIndex].Cells["gvAllActivities"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");
                    dgvHUDCounts.Rows[rowIndex].Cells["gvFundedActivities"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");
                }

                if (dtIndividual.Rows.Count > 0)
                {
                    foreach (DataRow drIndiv in dtIndividual.Rows)
                    {
                        if (dr.Cells["gvRowType"].Value.ToString() == "D" || dr.Cells["gvRowType"].Value.ToString() == "T")
                        {
                            if (dr.Cells["gvRowType"].Value.ToString() == "D")
                            {
                                dr.Cells["gvFundedActivities"].Value = "0";
                                if (!string.IsNullOrEmpty(dr.Cells["gvAgyCode"].Value.ToString().Trim()) && !string.IsNullOrEmpty(dr.Cells["gvAgyType"].Value.ToString().Trim()))
                                {
                                    if (dr.Cells["gvAgyCode"].Value.ToString().Trim() == drIndiv["AGYS_CODE"].ToString().Trim() && dr.Cells["gvAgyType"].Value.ToString().Trim() == drIndiv["AGYS_TYPE"].ToString().Trim())
                                    {
                                        dr.Cells["gvAllActivities"].Value = drIndiv["INDIV_COUNT"].ToString().Trim() == "" ? "0" : drIndiv["INDIV_COUNT"].ToString().Trim();

                                        CatgTotals = CatgTotals + Convert.ToInt32(drIndiv["INDIV_COUNT"].ToString().Trim());
                                        break;
                                    }
                                }
                                else
                                {
                                    dr.Cells["gvAllActivities"].Value = "0";
                                    break;
                                }

                            }
                        }
                        if (dr.Cells["gvRowType"].Value.ToString() == "T")
                        {
                            dr.Cells["gvAllActivities"].Value = CatgTotals.ToString();
                            dr.Cells["gvFundedActivities"].Value = "0";
                            CatgTotals = 0;
                            break;
                        }
                    }
                }

                if (dr.Cells["gvImpCode"].Value.ToString() != "")
                {
                    if (dtImpacts.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dtImpacts);

                        DataTable distinctImpacts = dv.ToTable(true, "HUDIMPACT_IMPACTS", "HUDIMPACT_APPNO");

                        if (distinctImpacts.Rows.Count > 0)
                        {
                            DataRow[] drImpact = distinctImpacts.Select("HUDIMPACT_IMPACTS='" + dr.Cells["gvImpCode"].Value.ToString() + "'");

                            if (drImpact.Length > 0)
                            {
                                impactCount = 0;
                                foreach (DataRow drimpact in drImpact)
                                {
                                    if (dr.Cells["gvRowType"].Value.ToString() == "D" || dr.Cells["gvRowType"].Value.ToString() == "T")
                                    {
                                        if (dr.Cells["gvRowType"].Value.ToString() == "D")
                                        {
                                            dr.Cells["gvFundedActivities"].Value = "0";

                                            if (drimpact["HUDIMPACT_IMPACTS"].ToString() == dr.Cells["gvImpCode"].Value.ToString())
                                            {
                                                impactCount++;

                                                dr.Cells["gvAllActivities"].Value = impactCount.ToString();
                                            }
                                            else
                                                dr.Cells["gvAllActivities"].Value = "0";
                                        }
                                    }
                                }
                            }
                            if (dr.Cells["gvRowType"].Value.ToString() == "T")
                            {
                                dr.Cells["gvAllActivities"].Value = distinctImpacts.Rows.Count;
                                dr.Cells["gvFundedActivities"].Value = "0";
                                //impactTotal = 0;
                            }
                        }
                    }
                }
            }
        }


        string isQ1 = string.Empty;
        string isQ2 = string.Empty;
        string isQ3 = string.Empty;
        string isQ4 = string.Empty;

        string isQ1T = string.Empty;
        string isQ2T = string.Empty;
        string isQ3T = string.Empty;
        string isQ4T = string.Empty;

        private void cmbRepQuart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "1")
            {
                dtpFrmDte.Text = "10/01/" + (Convert.ToInt32(((Captain.Common.Utilities.ListItem)cmbFiscYear.SelectedItem).Text.ToString()) - 1);

                DateTime Q1 = DateTime.Parse(dtpFrmDte.Text);

                isQ1 = Q1.ToString();

                dtpToDte.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ1T = dtpToDte.Text;
            }
            else if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "2")
            {
                dtpFrmDte.Text = "01/01/" + ((Captain.Common.Utilities.ListItem)cmbFiscYear.SelectedItem).Text.ToString();

                DateTime Q1 = DateTime.Parse(dtpFrmDte.Text);

                isQ2 = Q1.ToString();

                dtpToDte.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ2T = dtpToDte.Text;
            }
            else if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "3")
            {
                dtpFrmDte.Text = "04/01/" + ((Captain.Common.Utilities.ListItem)cmbFiscYear.SelectedItem).Text.ToString();

                DateTime Q1 = DateTime.Parse(dtpFrmDte.Text);

                isQ3 = Q1.ToString();

                dtpToDte.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ3T = dtpToDte.Text;
            }
            else
            {
                dtpFrmDte.Text = "07/01/" + ((Captain.Common.Utilities.ListItem)cmbFiscYear.SelectedItem).Text.ToString();

                DateTime Q1 = DateTime.Parse(dtpFrmDte.Text);

                isQ4 = Q1.ToString();

                dtpToDte.Text = Q1.AddMonths(3).AddDays(-1).ToString();

                isQ4T = dtpToDte.Text;
            }
        }

        DataTable dtIndividual = new DataTable();
        DataTable dtImpacts = new DataTable();
        private void btnGenHUDCnts_Click(object sender, EventArgs e)
        {
            if (FromDateQuarterCheck() && ToDateQuarterCheck())
            {
                if (isValid == true)
                {
                    if (ValidateDates())
                    {
                        DataSet dsHUDCounts = DatabaseLayer.HUDCNTLDB.Get_HUD_Counts(Agency,Depart,Program,Program_Year,dtpFrmDte.Text,dtpToDte.Text);

                        if (dsHUDCounts.Tables[0].Rows.Count > 0)
                            dtIndividual = dsHUDCounts.Tables[0];
                        else
                            dtIndividual.Clear();

                        if (dsHUDCounts.Tables[1].Rows.Count > 0)
                            dtImpacts = dsHUDCounts.Tables[1];
                        else
                            dtImpacts.Clear();

                        if (dtIndividual.Rows.Count > 0)
                        {
                            fillHUDCounts_Grid();
                        }
                    }
                }
            }
            isValid = true;
        }

        bool isValid = true;
        private bool ValidateDates()
        {
            isValid = true;

            if (string.IsNullOrWhiteSpace(dtpFrmDte.Text))
            {
                _errorProvider.SetError(dtpFrmDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFrmDte, null);
            }
            if (string.IsNullOrWhiteSpace(dtpToDte.Text))
            {
                _errorProvider.SetError(dtpToDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date ".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDte, null);
            }

            if (Convert.ToDateTime(dtpFrmDte.Text) > Convert.ToDateTime(dtpToDte.Text))
            {
                _errorProvider.SetError(dtpFrmDte, string.Format("'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }

            return isValid;
        }

        private bool FromDateQuarterCheck()
        {
            if (ValidateDates())
            {
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "1")
                {
                    if (Convert.ToDateTime(dtpFrmDte.Text) < Convert.ToDateTime(isQ1))
                    {
                        AlertBox.Show("The entered Date shouldn't be prior to " + Convert.ToDateTime(isQ1).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpFrmDte.Text = isQ1;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "2")
                {
                    if (Convert.ToDateTime(dtpFrmDte.Text) < Convert.ToDateTime(isQ2))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ2).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpFrmDte.Text = isQ2;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "3")
                {
                    if (Convert.ToDateTime(dtpFrmDte.Text) < Convert.ToDateTime(isQ3))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ3).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpFrmDte.Text = isQ3;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "4")
                {
                    if (Convert.ToDateTime(dtpFrmDte.Text) < Convert.ToDateTime(isQ4))
                    {
                        AlertBox.Show("The entered Date shouldn't prior to " + Convert.ToDateTime(isQ4).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpFrmDte.Text = isQ4;

                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private bool ToDateQuarterCheck()
        {
            if (ValidateDates())
            {
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "1")
                {
                    if (Convert.ToDateTime(dtpToDte.Text) > Convert.ToDateTime(isQ1T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ1T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpToDte.Text = isQ1T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "2")
                {
                    if (Convert.ToDateTime(dtpToDte.Text) > Convert.ToDateTime(isQ2T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ2T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpToDte.Text = isQ2T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "3")
                {
                    if (Convert.ToDateTime(dtpToDte.Text) > Convert.ToDateTime(isQ3T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ3T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpToDte.Text = isQ3T;

                        isValid = false;
                    }
                }
                if (((Captain.Common.Utilities.ListItem)cmbRepQuart.SelectedItem).Value.ToString() == "4")
                {
                    if (Convert.ToDateTime(dtpToDte.Text) > Convert.ToDateTime(isQ4T))
                    {
                        AlertBox.Show("The entered Date shouldn't be greater than " + Convert.ToDateTime(isQ4T).Date.ToString("MM/dd/yyyy"), MessageBoxIcon.Warning);
                        dtpToDte.Text = isQ4T;

                        isValid = false;
                    }
                }
            }

            return isValid;
        }
    }
}
