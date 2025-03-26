
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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using System.Xml;
using System.IO;
using Captain.Common.Utilities;
using System.Text.RegularExpressions;
using System.Threading;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using Captain.Common.Views.Controls.Compatibility;
using static NPOI.HSSF.Util.HSSFColor;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using ListItem = Captain.Common.Utilities.ListItem;
using DevExpress.XtraBars;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNGB0014Form : _iForm//Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;


        #endregion

        public RNGB0014Form(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            ListZipCode = new List<ZipCodeEntity>();
            ListRngGroupCode = new List<RCsb14GroupEntity>();
            ListcaseSiteEntity = new List<CaseSiteEntity>();
            ListcaseMsSiteEntity = new List<CaseSiteEntity>();
            Sel_Funding_List = new List<SPCommonEntity>();
            ListcommonEntity = new List<CommonEntity>();

            InitializeComponent();
            BaseForm = baseForm;
            PrivilegeEntity = privilegeEntity;
            this.Text = "ROMA Outcome Indicators";
            //this.Text = privilegeEntity.Program + " - " + privilegeEntity.PrivilegeName;
            _model = new CaptainModel();


            //RngCodelist = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null,BaseForm.UserID,BaseForm.BaseAgency);
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            ReportPath = _model.lookupDataAccess.GetReportPath();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            Txt_Pov_High.Validator = TextBoxValidation.IntegerValidator;
            Txt_Pov_Low.Validator = TextBoxValidation.IntegerValidator;
            fillDropDowns();
            propReportPath = _model.lookupDataAccess.GetReportPath();

            Get_MasterTable_DateRanges();
            Fill_CAMS_Master_List();
            Fill_All_List_Arrays();
            //Fill_Program_Combo();
            fillRngCode();
            Fill_Fund_Mast_List();

            if (Rb_SNP_Mem.Checked)
            {
                chkbUndupTable1.Visible = false; chkbUndupTable1.Checked = false;
            }
            else
            {
                chkbUndupTable1.Visible = true;
            }
            //Initialize_All_Controls();
            this.Size = new Size(this.Width, 657);//688);
            this.pnlbtns.Size = new Size(Width, 522);
        }

        void fillRngCode()
        {
            cmbRngCode.Items.Clear();
            cmbRngCode.ColorMember = "FavoriteColor";
            RngCodelist = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
            List<RCsb14GroupEntity> rngonlycodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty);

            int rowIndex = 0; int cnt = 1;
            List<RCsb14GroupEntity> rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals(strAgency));
            if (rngonlyAgencylist.Count == 0)
            {
                rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals("**"));
            }

            rngonlyAgencylist = rngonlyAgencylist.OrderByDescending(u => u.Active).ToList();
            foreach (RCsb14GroupEntity rngcodedata in rngonlyAgencylist)
            {
                ListItem li = new ListItem(rngcodedata.GrpDesc, rngcodedata.Code, rngcodedata.Agency, rngcodedata.Active.Equals("Y") ? Color.Black : Color.Red,rngcodedata.PPR_SW);

                if (DateTime.Now >= Convert.ToDateTime(rngcodedata.OFdate.Trim()) && DateTime.Now <= Convert.ToDateTime(rngcodedata.OTdate.Trim()))
                    rowIndex = cnt;

                cmbRngCode.Items.Add(li);

                cnt++;

               // cmbRngCode.Items.Add(new Captain.Common.Utilities.ListItem(rngcodedata.GrpDesc, rngcodedata.Code, rngcodedata.Agency, string.Empty));
            }
            cmbRngCode.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "**"));
            cmbRngCode.SelectedIndex = rowIndex;
        }

        void fillDropDowns()
        {
            Cmb_CaseType.Items.Clear();
            List<CommonEntity> CaseType = _model.lookupDataAccess.GetCaseType();
            CaseType = CaseType.OrderByDescending(u => u.Active.Trim()).ToList();
            Cmb_CaseType.ColorMember = "FavoriteColor";
            foreach (CommonEntity casetype in CaseType)
            {
                ListItem li = new ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Black : Color.Red);
                Cmb_CaseType.Items.Add(li);

                //Cmb_CaseType.Items.Add(new Captain.Common.Utilities.ListItem(casetype.Desc, casetype.Code));

                //if (casetype.Active != "Y")
                //    Cmb_CaseType.ForeColor = Color.Red;

            }
            Cmb_CaseType.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "**"));
            Cmb_CaseType.SelectedIndex = 0;

            Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
            Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
            if (Sys_DateRange_List.Count > 0)
            {
                Ref_From_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_FDATE);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                Ref_To_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_TDATE);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
            }

            //Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            //Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));
            Rep_From_Date.Checked = Rep_To_Date.Checked = true;


        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegeEntity { get; set; }

        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        public List<SPCommonEntity> Sel_Funding_List { get; set; }

        public List<ZipCodeEntity> ListZipCode { get; set; }

        public List<RCsb14GroupEntity> ListRngGroupCode { get; set; }

        public List<CommonEntity> ListcommonEntity { get; set; }

        public List<CaseSiteEntity> ListcaseMsSiteEntity { get; set; }

        public string strAgency { get; set; }

        public string strDept { get; set; }

        public string strProg { get; set; }

        public string ReportPath { get; set; }

        public string propReportPath { get; set; }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                //return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                //                               where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                //                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();

                return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }

        #endregion

        List<ZipCodeEntity> zipcode_List = new List<ZipCodeEntity>();
        List<CommonEntity> County_List = new List<CommonEntity>();
        List<CaseSiteEntity> Site_List = new List<CaseSiteEntity>();
        List<CaseSiteEntity> MSSite_List = new List<CaseSiteEntity>();
        List<RCsb14GroupEntity> RngCodelist = new List<RCsb14GroupEntity>();
        private void Fill_All_List_Arrays()
        {
            zipcode_List = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            County_List = _model.ZipCodeAndAgency.GetCounty();
            MSSite_List = Site_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
        }

        private void rdoSelectedSites_Click(object sender, EventArgs e)
        {
            if (Rb_Site_Sel.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseSiteEntity, strAgency, strDept, strProg, string.Empty);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }


        private void rdoCountySelected_Click(object sender, EventArgs e)
        {
            if (Rb_County_Sel.Checked == true)// && Scr_Oper_Mode == "CASB0004") // 20160303
            {
                SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(BaseForm, ListcommonEntity);
                countyform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                countyform.StartPosition = FormStartPosition.CenterScreen;
                countyform.ShowDialog();
            }
        }

        private void rdoZipcodeSelected_Click(object sender, EventArgs e)
        {
            if (Rb_Zip_Sel.Checked == true)
            {
                if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() == "**")
                {
                    AlertBox.Show("Please Select Code", MessageBoxIcon.Warning);
                }
                else
                {

                    SelectZipSiteCountyForm zipcodeform1 = new SelectZipSiteCountyForm(BaseForm, ListRngGroupCode, Ref_From_Date.Text.Trim(), Ref_To_Date.Text.Trim(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                    zipcodeform1.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                    zipcodeform1.StartPosition = FormStartPosition.CenterScreen;
                    zipcodeform1.ShowDialog();
                }

            }
        }

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {

            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "CASESITE")
                {
                    ListcaseSiteEntity = form.SelectedCaseSiteEntity;
                    if (Rb_Site_Sel.Checked == true && ListcaseSiteEntity.Count > 0)
                        Txt_Sel_Site.Text = ListcaseSiteEntity[0].SiteNUMBER.ToString();
                    else
                        Txt_Sel_Site.Clear();
                }
                else if (form.FormType == "COUNTY")
                {
                    ListcommonEntity = form.SelectedCountyEntity;
                }

                else if (form.FormType == "ZIPCODE")
                {
                    ListZipCode = form.SelectedZipcodeEntity;
                }
                else if (form.FormType == "MSCODE")
                {
                    ListRngGroupCode = form.SelectedRngGroupCodeEntity;
                }
                else if (form.FormType == "FUND")
                {
                    Sel_Funding_List = form.Get_Sel_Fund_List;
                }

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

        private void SelectZipSiteCountyMSFormClosed(object sender, FormClosedEventArgs e)
        {

            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "CASESITE")
                {
                    ListcaseMsSiteEntity = form.SelectedCaseSiteEntity;
                    if (rdoMsselectsite.Checked == true && ListcaseMsSiteEntity.Count > 0)
                        txt_Msselect_site.Text = ListcaseMsSiteEntity[0].SiteNUMBER.ToString();
                    else
                        txt_Msselect_site.Clear();
                }
                else if (form.FormType == "COUNTY")
                {
                    ListcommonEntity = form.SelectedCountyEntity;
                }

                else if (form.FormType == "ZIPCODE")
                {
                    ListZipCode = form.SelectedZipcodeEntity;
                }
                else if (form.FormType == "MSCODE")
                {
                    ListRngGroupCode = form.SelectedRngGroupCodeEntity;
                }
                else if (form.FormType == "FUND")
                {
                    Sel_Funding_List = form.Get_Sel_Fund_List;
                }

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



        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        string DeptName = string.Empty; string ProgName = string.Empty;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            txtHieDesc.Clear();
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;
            strAgency = Agy;
            strDept = Dept;
            strProg = Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        txtHieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                txtHieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                    {
                        txtHieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                        DeptName = ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString().Trim() + "/";
                    }
                }
            }
            else
            {
                txtHieDesc.Text += "DEPT : ** - All Departments      ";
                DeptName = "All Departments /";
            }

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                    {
                        txtHieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        ProgName = ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString().Trim();
                    }
                }
            }
            else
            {
                txtHieDesc.Text += "PROG : ** - All Programs ";
                ProgName = "All Programs ";
            }


            //if (Agy != "**")
            //    Get_NameFormat_For_Agencirs(Agy);
            //else
            //    Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                CmbYear.Visible = false;
                //this.txtHieDesc.Size = new System.Drawing.Size(580, 23);
                this.txtHieDesc.Size = new System.Drawing.Size(745, 25);//(810, 25);
            }

        }

        List<SPCommonEntity> Fund_Mast_List = new List<SPCommonEntity>();
        private void Fill_Fund_Mast_List()
        {
            Fund_Mast_List = _model.SPAdminData.Get_AgyRecs("Funding");
            //if (Entity_List != null && Entity_List.Count > 0)
            //{
            //    FundingList.ForEach(item => item.Active = (Entity_List.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
            //}
        }


        string DepYear, Program_Year;
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
                            //listItem.Add(new ListItem(TmpYearStr, i));
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, TmpYearStr));
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

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
            {
                //Current_Hierarchy = Current_Hierarchy + Program_Year;
                this.txtHieDesc.Size = new System.Drawing.Size(660, 25);//(753, 25);
            }
            else
                this.txtHieDesc.Size = new System.Drawing.Size(745,25);//(810, 25);
        }


        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {

            ////HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", Current_Hierarchy_DB, string.Empty, "Reports");
            ////hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            ////hierarchieSelectionForm.ShowDialog();

            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();

        }


        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            HierarchieSelection form = sender as HierarchieSelection;

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
                    Current_Hierarchy = hierarchy.Substring(0, 2) + hierarchy.Substring(2, 2) + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    //Initialize_All_Controls();
                    //Fill_Program_Combo();
                    rbAllPrograms.Checked = true;
                    fillRngCode();
                }
            }
        }


        private void btnRepMaintPreview_Click(object sender, EventArgs e)
        {
            //string myPath = Context.Server.MapPath("~\\Resources\\Excel\\Sample.xlsx"); //(@"C:\sample.xlsx");
            //if (File.Exists(myPath))
            //{
            //    //excelApp.Workbooks.Open(myPath);
            //    //excelApp.Visible = true;
            //    FrmViewer objfrm = new FrmViewer(myPath);
            //    objfrm.Size = new System.Drawing.Size(100,40);
            //    objfrm.FormBorderStyle = Wisej.Web.FormBorderStyle.FixedDialog;
            //    objfrm.ShowDialog();

            //}

            PdfListForm pdfListForm = new PdfListForm(BaseForm, PrivilegeEntity, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();

        }

        private void CASB0004Form_Load(object sender, EventArgs e)
        {
            Cmb_CaseType.Focus();
        }


        List<DG_ResTab_Entity> PM_Detail_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PM_Detail_Table_Structure()
        {
            //Declare @Detail_Cum_Table Table(SortUnDup_Group VarChar(10), SortUnDup_Table VarChar(10), SortUnDup_Agy Char(2), SortUnDup_Dept Char(2), SortUnDup_Prog Char(2), 
            //                                SortUnDup_Year VarChar(4), SortUnDup_App VarChar(8), SortUnDup_Fam_ID VarChar(9), SortUnDup_Client_ID Decimal(9),
            //                                SortUnDup_OutcomeCode Varchar(10), SortUnDup_OutCome_Date Date, SortUnDup_Count_Indicator Char(1), SortUnDup_Result Varchar(4), SortUnDup_Name Varchar(90), 
            //                                R1 Int Default 0, R2 Int Default 0, R3 Int Default 0, R4 Int Default 0, R5 Int Default 0)


            PM_Detail_Table_List.Clear();
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "Outcome_Name", "", "L", "5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Group", "", "L", "2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Table", "", "L", "2in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Agy", "", "L", "1.5in"));
            //Commented by Sudheer on 02/04/2021
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Agy", "", "L", "1.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Dept", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Prog", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Year", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_App", "", "L", ".95in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Fam_ID", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Client_ID", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Name", "", "L", "2.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Count_Indicator", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Result", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R1", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R2", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R3", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R4", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R5", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_UserId", "", "L", "2.5in"));
            if (rdoperiodBoth.Checked)
            {
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_OutCome_Date", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RefCount", "", "R", "1.5in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RefDate", "", "R", "1in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RepCount", "", "R", "1in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RepDate", "", "R", "1in"));              

            }
            else
            {
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutCome_Date", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RefCount", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RefDate", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RepCount", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RepDate", "", "R", ".95in"));
              
            }
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutcomeCode", "", "L", "0.8in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RePcount", "", "L", "1in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Refcount", "", "L", "1in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Group_Desc", "", "R", ".85in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Goal_Desc", "", "L", "3.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Isite", "", "L", "1.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Psite", "", "L", "1.5in"));

        }

        string Out_Rep_Name = string.Empty;
        DataTable dt;
        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            //string myPath = "";
            //if (Context.HttpContext.Request.IsSecureConnection)
            //    myPath = "\\\\cap-dev\\C-Drive\\CapReports\\CasDemo.pdf" ;
            //else
            //    myPath = @"C:\CapReports\CasDemo.pdf"; //Context.Server.MapPath("~\\Resources\\Excel\\Sample.xlsx"); //(@"C:\sample.xlsx");
            //FrmViewer objfrm = new FrmViewer(myPath);
            //objfrm.ShowDialog();

             if (Validate_Report())
            {
                Get_Selection_Criteria();

                DataSet ds = new DataSet();

                //ds = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                // Modified by 15/06/2018 Details value also pass to Y
                string strReportSwitch = "C";
                string strReportControlSwitch = "N";
                string strUndupTabSwitch = "N";
                if (rdoperiodBoth.Checked)
                {
                    strReportSwitch = "B";
                    strReportControlSwitch = chkRepControl.Checked == true ? "Y" : "N";

                    if (chkbUndupTable1.Checked == true)
                        strUndupTabSwitch = "Y";
                    //**strUndupTabSwitch = "N";
                }
                else
                {
                    if (rdoperiod.Checked == true)
                        strReportSwitch = "R";

                    if (chkbUndupTable1.Checked == true) strUndupTabSwitch = "Y";
                }
                string strRepCFromDate = string.Empty;
                string strRepCToDate = string.Empty;
                if (rdoperiodBoth.Checked)
                {
                    Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); // DateTime.ParseExact(Ref_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Ref_From_Date.Value.ToShortDateString();
                    Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy");  //DateTime.ParseExact(Ref_To_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Ref_To_Date.Value.ToShortDateString();

                }
                if (rdoperiodCumulative.Checked)
                {
                    //strRepCFromDate = Ref_From_Date.Value.ToShortDateString();
                    //strRepCToDate = Ref_To_Date.Value.ToShortDateString();

                    strRepCFromDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_From_Date.Value.ToShortDateString();
                    strRepCToDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_To_Date.Value.ToString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_To_Date.Value.ToShortDateString();

                }
                else
                {
                    //strRepCFromDate = Rep_From_Date.Value.ToShortDateString();
                    //strRepCToDate = Rep_To_Date.Value.ToShortDateString();

                    strRepCFromDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_From_Date.Value.ToShortDateString();
                    strRepCToDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_To_Date.Value.ToShortDateString();

                }

                if (rdoperiodBoth.Checked)
                {
                    //COMMENTED BY SUDHEER ON 05/20/2021 FOR THE COUNTS
                    //Search_Entity.Rep_From_Date = Rep_From_Date.Value.ToShortDateString();
                    //Search_Entity.Rep_To_Date = Rep_To_Date.Value.ToShortDateString();


                    Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_From_Date.Value.ToShortDateString();
                    Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Rep_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Rep_To_Date.Value.ToShortDateString();

                    //ADDED BY SUDHEER ON 05/20/2021 FOR THE COUNTS
                    Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Ref_From_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Ref_From_Date.Value.ToShortDateString();
                    Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //DateTime.ParseExact(Ref_To_Date.Value.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString();//Ref_To_Date.Value.ToShortDateString();

                }
                Out_Rep_Name = string.Empty;
                string RepType = string.Empty;
                if (rdoperiodBoth.Checked)
                    RepType = "Rep";


                ds = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, strRepCFromDate, strRepCToDate, strReportControlSwitch, strUndupTabSwitch,RepType, SelectedAlertCodes, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString());
                //ds = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, strRepCFromDate, strRepCToDate, strReportControlSwitch, strUndupTabSwitch, RepType);

                bool Data_processed = false;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            dt = ds.Tables[1];

                            if (dt.Rows.Count > 0)
                            {
                                Data_processed = true;
                                if (Rb_Details_Yes.Checked)
                                {
                                    PerformanceMeasures_Details_Dynamic_RDLC();
                                    Result_Table = ds.Tables[2];
                                    if (ds.Tables.Count > 2)
                                        IndSwitch_Table = ds.Tables[3];
                                }
                                else
                                {
                                    if (ds.Tables.Count > 2)
                                        IndSwitch_Table = ds.Tables[3];
                                }

                                DataTable dtBoth = new DataTable();
                                if (rdoperiodBoth.Checked)//&& Rb_SNP_Mem.Checked)
                                {
                                    Search_Entity.Rep_From_Date = Ref_From_Date.Value.ToShortDateString();
                                    Search_Entity.Rep_To_Date = Ref_To_Date.Value.ToShortDateString();
                                    Search_Entity.Rep_Period_FDate = Ref_From_Date.Value.ToShortDateString();
                                    Search_Entity.Rep_Period_TDate = Ref_To_Date.Value.ToShortDateString();

                                    //if(strReportControlSwitch=="Y")
                                    //{
                                    //    Search_Entity.Rep_Period_FDate = Rep_From_Date.Value.ToShortDateString();
                                    //    Search_Entity.Rep_Period_TDate = Rep_To_Date.Value.ToShortDateString();
                                    //}

                                    DataSet dsboth = new DataSet();

                                    if (Search_Entity.Rep_From_Date != Rep_From_Date.Value.ToShortDateString() || Search_Entity.Rep_To_Date != Rep_To_Date.Value.ToShortDateString())
                                    //dsboth = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, "Y", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, Rep_From_Date.Value.ToShortDateString(), Rep_To_Date.Value.ToShortDateString(), strReportControlSwitch, strUndupTabSwitch, string.Empty);
                                    dsboth = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, "Y", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, Rep_From_Date.Value.ToShortDateString(), Rep_To_Date.Value.ToShortDateString(), strReportControlSwitch, strUndupTabSwitch, string.Empty, SelectedAlertCodes, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString());
                                    else
                                        dsboth = ds;
                                    //ds = _model.AdhocData.Get_RNGPM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "Y", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), rdoperiod.Checked == true ? "R" : "C");
                                    if (dsboth.Tables.Count > 1)
                                    {
                                        Result_Table = dsboth.Tables[2];
                                        if (dsboth.Tables[1].Rows.Count > 0)
                                        {
                                            dtBoth = dsboth.Tables[1];
                                        }
                                        if (dsboth.Tables.Count > 2)
                                            IndSwitchReport_Table = dsboth.Tables[3];

                                    }
                                }

                                On_SaveForm_Closed(dt, dtBoth);
                                if (chkbExcel.Checked)
                                    On_SaveFormExcel_Closed(dt, dtBoth);

                            }
                        }
                        else
                            AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);

                    }
                    //else
                    //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");
                }


                if (!Data_processed)
                    AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);


            }
        }



        #region PdfReportCode




        //*****************PerformanceMeasures_Details_Dynamic_RDLC***********************************************************************************

        private void PerformanceMeasures_Details_Dynamic_RDLC()
        {

            //Get_Report_Selection_Parameters();
            Get_PM_Detail_Table_Structure();

            XmlNode xmlnode;

            XmlDocument xml = new XmlDocument();
            xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xml.AppendChild(xmlnode);

            XmlElement Report = xml.CreateElement("Report");
            Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            xml.AppendChild(Report);

            XmlElement DataSources = xml.CreateElement("DataSources");
            XmlElement DataSource = xml.CreateElement("DataSource");
            DataSource.SetAttribute("Name", "CaptainDataSource");
            DataSources.AppendChild(DataSource);

            Report.AppendChild(DataSources);

            XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
            DataSource.AppendChild(ConnectionProperties);

            XmlElement DataProvider = xml.CreateElement("DataProvider");
            DataProvider.InnerText = "System.Data.DataSet";


            XmlElement ConnectString = xml.CreateElement("ConnectString");
            ConnectString.InnerText = "/* Local Connection */";
            ConnectionProperties.AppendChild(DataProvider);
            ConnectionProperties.AppendChild(ConnectString);

            //string SourceID = "rd:DataSourceID";
            //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
            //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
            //DataSource.AppendChild(DataSourceID);

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

            XmlElement DataSets = xml.CreateElement("DataSets");
            Report.AppendChild(DataSets);

            XmlElement DataSet = xml.CreateElement("DataSet");
            DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
            DataSets.AppendChild(DataSet);

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

            XmlElement Fields = xml.CreateElement("Fields");
            DataSet.AppendChild(Fields);

            foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)
            {
                XmlElement Field = xml.CreateElement("Field");
                Field.SetAttribute("Name", Entity.Column_Name);
                Fields.AppendChild(Field);

                XmlElement DataField = xml.CreateElement("DataField");
                DataField.InnerText = Entity.Column_Name;
                Field.AppendChild(DataField);
            }

            //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

            XmlElement Query = xml.CreateElement("Query");
            DataSet.AppendChild(Query);

            XmlElement DataSourceName = xml.CreateElement("DataSourceName");
            DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
            Query.AppendChild(DataSourceName);

            XmlElement CommandText = xml.CreateElement("CommandText");
            CommandText.InnerText = "/* Local Query */";
            Query.AppendChild(CommandText);


            //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
            //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

            //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            XmlElement Body = xml.CreateElement("Body");
            Report.AppendChild(Body);


            XmlElement ReportItems = xml.CreateElement("ReportItems");
            Body.AppendChild(ReportItems);

            XmlElement Height = xml.CreateElement("Height");
            //Height.InnerText = "4.15625in";       // Landscape
            Height.InnerText = "2in";           // Portrait
            Body.AppendChild(Height);


            XmlElement Style = xml.CreateElement("Style");
            Body.AppendChild(Style);

            XmlElement Border = xml.CreateElement("Border");
            Style.AppendChild(Border);

            XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
            BackgroundColor.InnerText = "White";
            Style.AppendChild(BackgroundColor);


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

            //////XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
            //////Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
            //////ReportItems.AppendChild(Sel_Rectangle);

            //////XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
            //////Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


            //////double Total_Sel_TextBox_Height = 0.16667;
            //////string Tmp_Sel_Text = string.Empty;
            //////for (int i = 0; i < 22; i++)
            //////{
            //////    XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
            //////    Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
            //////    Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

            //////    XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
            //////    Textbox1_Cangrow.InnerText = "true";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

            //////    XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
            //////    Textbox1_Keep.InnerText = "true";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

            //////    XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

            //////    XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
            //////    Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

            //////    XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
            //////    Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


            //////    XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
            //////    Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

            //////    XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

            //////    Tmp_Sel_Text = string.Empty;
            //////    switch (i)
            //////    {
            //////        case 0: Tmp_Sel_Text = "Selected Report Parameters"; break;

            //////        case 3: Tmp_Sel_Text = "      Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

            //////        case 5: Tmp_Sel_Text = "            Attributes                         :   " + (Rb_Agy_Def.Checked ? "Agency Defined" : "User Defined Associations"); break;
            //////        case 6: Tmp_Sel_Text = "            Case Type                       :   " + ((ListItem)Cmb_CaseType.SelectedItem).Text.ToString(); break;
            //////        case 7: Tmp_Sel_Text = "            Case Status                     :   " + Sel_params_To_Print[1]; break;

            //////        case 9: Tmp_Sel_Text = "            Date Selection                 :   " + (Rb_MS_AddDate.Checked ? "MS AddDate" : "MS Date"); break;
            //////        case 10: Tmp_Sel_Text = "            Reference Period Date    :   From " + Search_Entity.Rep_From_Date + "    To " + Search_Entity.Rep_To_Date; break;
            //////        case 11: Tmp_Sel_Text = "            Report Period Date          :   From " + Search_Entity.Rep_Period_FDate + "    To " + Search_Entity.Rep_Period_TDate; break;
            //////        case 12: Tmp_Sel_Text = "            Poverty Levels                 :   From " + Txt_Pov_Low.Text + "    To " + Txt_Pov_High.Text; break;

            //////        case 14: Tmp_Sel_Text = "            Site                                   :   " + Sel_params_To_Print[2]; break;
            //////        case 15: Tmp_Sel_Text = "            Groups                          :   " + (Rb_Zip_All.Checked ? "All Groups" : "Selected Groups"); break;
            //////        case 16: Tmp_Sel_Text = "            Report Format                          :   " + (Rb_County_All.Checked ? "Performance Measures Only" : "Performance Measures + Goal Details"); break;

            //////        case 18: Tmp_Sel_Text = "            Secret Applications          :   " + Sel_params_To_Print[0]; break;
            //////        case 19: Tmp_Sel_Text = "            Produce Stastical Details :   " + (Rb_Details_Yes.Checked ? "Yes" : "No"); break;

            //////        default: Tmp_Sel_Text = "  "; break;
            //////    }


            //////    Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
            //////    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


            //////    XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
            //////    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

            //////    XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            //////    Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
            //////    Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


            //////    XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
            //////    Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


            //////    XmlElement Textbox1_Top = xml.CreateElement("Top");
            //////    Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

            //////    XmlElement Textbox1_Left = xml.CreateElement("Left");
            //////    Textbox1_Left.InnerText = "0.07292in";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

            //////    Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

            //////    XmlElement Textbox1_Height = xml.CreateElement("Height");
            //////    Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

            //////    XmlElement Textbox1_Width = xml.CreateElement("Width");
            //////    //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
            //////    Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

            //////    XmlElement Textbox1_Style = xml.CreateElement("Style");
            //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

            //////    XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
            //////    Textbox1_Style.AppendChild(Textbox1_Style_Border);

            //////    XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
            //////    Textbox1_Style_Border_Style.InnerText = "None";
            //////    Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

            //////    XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
            //////    Textbox1_Style_PaddingLeft.InnerText = "2pt";
            //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

            //////    XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
            //////    Textbox1_Style_PaddingRight.InnerText = "2pt";
            //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

            //////    XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
            //////    Textbox1_Style_PaddingTop.InnerText = "2pt";
            //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

            //////    XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
            //////    Textbox1_Style_PaddingTop.InnerText = "2pt";
            //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

            //////}

            //////XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            //////Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

            //////XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
            //////Break_After_SelParamRectangle_Location.InnerText = "End";
            //////Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

            //////XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
            //////Sel_Rectangle_KeepTogether.InnerText = "true";
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

            //////XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
            //////Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            //////XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            //////Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            //////XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            //////Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            //////XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
            ////////Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            //////Sel_Rectangle_Width.InnerText = (true ? "10" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

            //////XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
            //////Sel_Rectangle_ZIndex.InnerText = "1";
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

            //////XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
            //////Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

            //////XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
            //////Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

            //////XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
            //////Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
            //////Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)                      // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")
                {
                    XmlElement TablixColumn = xml.CreateElement("TablixColumn");
                    TablixColumns.AppendChild(TablixColumn);

                    XmlElement Col_Width = xml.CreateElement("Width");
                    //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
                    //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
                    Col_Width.InnerText = Entity.Disp_Width;
                    TablixColumn.AppendChild(Col_Width);
                }
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            //Row_Height.InnerText = "0.25in";
            Row_Height.InnerText = "0.0000001in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
            foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)            // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    //Entity.Column_Name;
                    Tmp_Loop_Cnt++;

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells.AppendChild(TablixCell);


                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    //if (Entity.Col_Format_Type == "C")
                    //    Field_type = "Checkbox";

                    XmlElement Textbox = xml.CreateElement(Field_type);
                    Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
                    CellContents.AppendChild(Textbox);

                    XmlElement CanGrow = xml.CreateElement("CanGrow");
                    CanGrow.InnerText = "true";
                    Textbox.AppendChild(CanGrow);

                    XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                    KeepTogether.InnerText = "true";
                    Textbox.AppendChild(KeepTogether);



                    XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                    Textbox.AppendChild(Paragraphs);

                    XmlElement Paragraph = xml.CreateElement("Paragraph");
                    Paragraphs.AppendChild(Paragraph);



                    XmlElement TextRuns = xml.CreateElement("TextRuns");
                    Paragraph.AppendChild(TextRuns);

                    XmlElement TextRun = xml.CreateElement("TextRun");
                    TextRuns.AppendChild(TextRun);

                    XmlElement Return_Value = xml.CreateElement("Value");

                    Tmp_Disp_Column_Name = Entity.Disp_Name;


                    //Disp_Col_Substring_Len = 6;

                    //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
                    Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);


                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Center";
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);


                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    ////XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    ////Return_Style_FontWeight.InnerText = "Bold";
                    ////Return_Style.AppendChild(Return_Style_FontWeight);


                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    //////XmlElement Cell_Border = xml.CreateElement("Border");
                    //////Cell_style.AppendChild(Cell_Border);

                    //////XmlElement Border_Color = xml.CreateElement("Color");
                    //////Border_Color.InnerText = "Black";//"LightGrey";
                    //////Cell_Border.AppendChild(Border_Color);

                    //////XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    //////Border_Style.InnerText = "Solid";
                    //////Cell_Border.AppendChild(Border_Style);

                    //////XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //////Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    //////Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

                    XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                    PaddingLeft.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingLeft);

                    XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                    PaddingRight.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingRight);

                    XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                    PaddingTop.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingTop);

                    XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                    PaddingBottom.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingBottom);
                }
            }




            XmlElement TablixRow2 = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow2);

            XmlElement Row_Height2 = xml.CreateElement("Height");
            Row_Height2.InnerText = "0.175in";
            //Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            char Tmp_Double_Codes = '"';
            foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)        // Dynamic based on Display Columns in Result Table
            {
                if (Entity.Can_Add == "Y")   // 09062012
                {

                    XmlElement TablixCell = xml.CreateElement("TablixCell");
                    Row_TablixCells2.AppendChild(TablixCell);

                    XmlElement CellContents = xml.CreateElement("CellContents");
                    TablixCell.AppendChild(CellContents);

                    XmlElement Textbox = xml.CreateElement("Textbox");
                    Textbox.SetAttribute("Name", Entity.Column_Name);
                    CellContents.AppendChild(Textbox);

                    XmlElement CanGrow = xml.CreateElement("CanGrow");
                    CanGrow.InnerText = "true";
                    Textbox.AppendChild(CanGrow);

                    XmlElement KeepTogether = xml.CreateElement("KeepTogether");
                    KeepTogether.InnerText = "true";
                    Textbox.AppendChild(KeepTogether);

                    XmlElement Paragraphs = xml.CreateElement("Paragraphs");
                    Textbox.AppendChild(Paragraphs);

                    XmlElement Paragraph = xml.CreateElement("Paragraph");
                    Paragraphs.AppendChild(Paragraph);

                    XmlElement TextRuns = xml.CreateElement("TextRuns");
                    Paragraph.AppendChild(TextRuns);

                    XmlElement TextRun = xml.CreateElement("TextRun");
                    TextRuns.AppendChild(TextRun);

                    XmlElement Return_Value = xml.CreateElement("Value");


                    Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    switch (Entity.Column_Name)                         //
                    {

                        //case "SortUnDup_Group": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //                                    ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + " , Fields!SortUnDup_Group.Value)";
                        //    break;

                        //case "SortUnDup_Table": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //                                    ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + "  , Fields!SortUnDup_Table.Value)";
                        //    break;

                        //case "SortUnDup_Agy": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //                                    ", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                                    " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                                    " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes +
                        //                                    " + Fields!SortUnDup_App.Value  + " + Tmp_Double_Codes + "      " + Tmp_Double_Codes +
                        //                                    " + Fields!SortUnDup_Name.Value)";

                        //Added by Sudheer on 02/03/2021
                        //case "SortUnDup_Group":
                        //    //    Field_Value = "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //    //                           ", " +Tmp_Double_Codes + " " + Tmp_Double_Codes +
                        //    //                                        ", Fields!SortUnDup_Group_Desc.Value)";
                        //    //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //    //                          ", " +Tmp_Double_Codes + " " + Tmp_Double_Codes +
                        //    //                                        ", Fields!SortUnDup_Group_Desc.Value)"; 
                        //    //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //    //                           ", " +Tmp_Double_Codes + "Group " + Tmp_Double_Codes +
                        //    //                                        ", Fields!SortUnDup_Group.Value)"; break;
                        //    //Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //    //                  " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //    //                   " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //    //                                Tmp_Double_Codes + "Group " + Tmp_Double_Codes +
                        //    //                                ", Fields!SortUnDup_Group.Value)"; break;

                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + ", " +" "+","+
                        //                          " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +", "+" "+ "," +
                        //                           " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //                                        Tmp_Double_Codes + "Group " + Tmp_Double_Codes +
                        //                                        ", Fields!SortUnDup_Group.Value)))"; break;
                        //case "SortUnDup_Table":
                        //    //Field_Value = "=IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //    //                      " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //    //                       " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //    //                                    Tmp_Double_Codes + "Table " + Tmp_Double_Codes +
                        //    //                                    ", Fields!SortUnDup_Table.Value)"; break;
                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + ", Fields!SortUnDup_Group_Desc.Value" + "," +
                        //                         " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ", " + "," +
                        //                          " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //                                       Tmp_Double_Codes + "Table " + Tmp_Double_Codes +
                        //                                       ", Fields!SortUnDup_Table.Value)))"; break;


                        //case "SortUnDup_Agy":
                        //    //Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //    //                      " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //    //                       " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //    //                                    Tmp_Double_Codes + "Hierarchy  " + Tmp_Double_Codes +
                        //    //                                    ",  Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //    //                                     " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //    //                                     " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")"; break;
                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + ", " +" "+ "," +
                        //                         " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ", " + "," +
                        //                          " IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                        //                                       Tmp_Double_Codes + "Fields!SortUnDup_Group_Desc.Value  " + Tmp_Double_Codes +
                        //                                        ",  Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                                         " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                                         " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")))"; break;

                        //Commented by Sudheer on 02/04/2021

                        //case "Outcome_Name":
                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value =" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                        //                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                        //                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "00GrpDesc" + Tmp_Double_Codes + ", Fields!SortUnDup_Group_Desc.Value )))";
                        //    break;
                        case "SortUnDup_Group":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                                                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                                                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ",Fields!SortUnDup_Group_Desc.Value " +
                                                //"Fields!SortUnDup_Group.Value" + Tmp_Double_Codes + " " + Tmp_Double_Codes + "" +
                                                ", Fields!SortUnDup_Group.Value)";
                                         // " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                                         //" OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                                        //", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Group.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes + ")";
                                         //" + Fields!SortUnDup_Table.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
                                         //" + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         //" + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         //" + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";
                            break;
                        case "SortUnDup_Table":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value =" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Group " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", Fields!SortUnDup_Table.Value)))";




                            //"=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                            //                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                            //                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," +
                            //            Tmp_Double_Codes + "Group " + Tmp_Double_Codes + ", Fields!SortUnDup_Table.Value)";

                            //" + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";
                            break;


                        case "SortUnDup_Agy":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Hiearachy " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes
                                                +
                                         ", Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")))";

                            //"=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                            //                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                            //                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," +
                            //            Tmp_Double_Codes + "Hiearachy " + Tmp_Double_Codes +
                            //         ", Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //         " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //         " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";

                            break;

                        //case "SortUnDup_Agy":
                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //                  " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //                  " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //                 ", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Group.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
                        //                 " + Fields!SortUnDup_Table.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
                        //                 " + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                 " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                 " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";
                        //    break;

                        case "SortUnDup_App":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                                Tmp_Double_Codes + "App# " + Tmp_Double_Codes +
                                                                ", Fields!SortUnDup_App.Value)"; break;
                        case "SortUnDup_Name":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Name" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_Name.Value)"; break;
                        case "SortUnDup_Isite":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Intake Site" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_Isite.Value)"; break;
                        case "SortUnDup_Psite":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Outcome Posting Site" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_Psite.Value)"; break;

                        case "SortUnDup_OutCome_Date":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                           Tmp_Double_Codes + " Activity Date" + Tmp_Double_Codes +
                                           ", Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + "))"; break;

                        case "R1":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "R1" + Tmp_Double_Codes +
                                                               ", Fields!" + Entity.Column_Name + ".Value)"; break;
                        case "R2":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "R2" + Tmp_Double_Codes +
                                                               ", Fields!" + Entity.Column_Name + ".Value)"; break;
                        case "R3":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "R3" + Tmp_Double_Codes +
                                                               ", Fields!" + Entity.Column_Name + ".Value)"; break;
                        case "R4":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "R4" + Tmp_Double_Codes +
                                                               ", Fields!" + Entity.Column_Name + ".Value)"; break;
                        case "R5":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "R5" + Tmp_Double_Codes +
                                                               ", Fields!" + Entity.Column_Name + ".Value)"; break;
                        case "SortUnDup_UserId":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "UPDATED BY USER ID" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_UserId.Value)"; break;
                        case "SortUnDup_RepCount":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Report Period" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_RepCount.Value)"; break;
                        case "SortUnDup_RepDate":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                           Tmp_Double_Codes + " Activity Date" + Tmp_Double_Codes +
                                           ", Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + "))"; break;


                        case "SortUnDup_RefCount":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Reference Period" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_RefCount.Value)"; break;

                        case "SortUnDup_RefDate":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                           Tmp_Double_Codes + " Activity Date" + Tmp_Double_Codes +
                                           ", Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + "))"; break;

                        case "SortUnDup_Goal_Desc":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                                Tmp_Double_Codes + "Description " + Tmp_Double_Codes +
                                                                ", Fields!SortUnDup_Goal_Desc.Value)"; break;
                    }

                    //Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Style_FontWeight = xml.CreateElement("FontWeight");
                    Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," +
                                                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + ", "
                                                        + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + "))";
                    Return_Style.AppendChild(Style_FontWeight);

                    // New
                    // Commented on 04022015 to Stop Color
                    //XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
                    //Textbox1_TextRun_Style_Color.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "#2D17EB" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + ")";
                    ////Textbox1_TextRun_Style_Color.InnerText = "Blue";
                    //Return_Style.AppendChild(Textbox1_TextRun_Style_Color);


                    ////////XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
                    ////////Paragraph.AppendChild(Textbox1_Paragraph_Style);

                    //////// New


                    ////////if (Entity.Column_Name == "Res_Table_Desc"  ) // 11292012
                    ////////{
                    //XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //// Commented on 04022015 to Stop Bold
                    ////Return_Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + " OR Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                    ////                                                                        " OR Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //Return_Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //Return_Style.AppendChild(Return_Style_FontWeight);
                    ////////}



                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Left" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    //Border_Color.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + ")";
                    Border_Color.InnerText = "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "None";
                    Cell_Border.AppendChild(Border_Style);


                    // Commented on 04022015 to Stop Background Color
                    //XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    ////Cell_Style_BackColor.InnerText = "Blue";
                    //Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + "," +
                                                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + ", "
                                                        + Tmp_Double_Codes + "White" + Tmp_Double_Codes + "))";
                    
                    Cell_style.AppendChild(Cell_Style_BackColor);

                    XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
                    PaddingLeft.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingLeft);

                    XmlElement PaddingRight = xml.CreateElement("PaddingRight");
                    PaddingRight.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingRight);

                    XmlElement PaddingTop = xml.CreateElement("PaddingTop");
                    PaddingTop.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingTop);

                    XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
                    PaddingBottom.InnerText = "2pt";
                    Cell_style.AppendChild(PaddingBottom);

                    //XmlElement ColSpan = xml.CreateElement("ColSpan");
                    ////ColSpan.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "1" + "," + "9" + ")";
                    //ColSpan.InnerText = "0";
                    //CellContents.AppendChild(ColSpan);

                    ////if (Entity.Column_Name == "Res_Table_Desc") // 11292012
                    ////{

                    ////    XmlElement Break_before_Group = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
                    ////    Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

                    ////    XmlElement Break_before_Group = xml.CreateElement("BreakLocation");
                    ////    Break_After_SelParamRectangle_Location.InnerText = "End";
                    ////    Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters
                    ////}






                }
            }



            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
            //TablixBody.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "End" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "None" + Tmp_Double_Codes + ")";
            ////SubReport_PageBreak_Location.InnerText = "End";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);



            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            if (rdoperiodBoth.Checked)
            {
                for (int Loop = 0; Loop < 19/*20*/; Loop++)     //17       // Dynamic based on Display Columns in 3/6
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }
            else
            {
                for (int Loop = 0; Loop < 16/*17*/; Loop++) //14            // Dynamic based on Display Columns in 3/6
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }

            XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
            Tablix.AppendChild(TablixRowHierarchy);

            XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
            TablixRowHierarchy.AppendChild(Tablix_Row_Members);

            XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member);

            XmlElement FixedData = xml.CreateElement("FixedData");
            FixedData.InnerText = "true";
            Tablix_Row_Member.AppendChild(FixedData);

            XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
            KeepWithGroup.InnerText = "After";
            Tablix_Row_Member.AppendChild(KeepWithGroup);

            XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
            RepeatOnNewPage.InnerText = "true";
            Tablix_Row_Member.AppendChild(RepeatOnNewPage);

            XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
            Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

            XmlElement Group = xml.CreateElement("Group"); // 5656565656
            Group.SetAttribute("Name", "Details1");
            Tablix_Row_Member1.AppendChild(Group);

            //XmlElement Group_Exps = xml.CreateElement("GroupExpressions"); // 5656565656
            //Group.AppendChild(Group_Exps);

            //XmlElement Group_Exp = xml.CreateElement("GroupExpression"); // 5656565656
            //Group_Exp.InnerText = "=Fields!Res_Group.Value+Fields!Res_Table_Desc.Value";
            //Group_Exps.AppendChild(Group_Exp);

            //XmlElement Group_Exp_Break = xml.CreateElement("PageBreak"); // 5656565656
            ////Group_Exp.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + ")";
            //Group.AppendChild(Group_Exp_Break);

            //XmlElement Group_Exp_Break_Loc = xml.CreateElement("BreakLocation"); // 5656565656
            ////Group_Exp_Break_Loc.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "Between" + "," + "None" + ")";
            //Group_Exp_Break_Loc.InnerText = "Between";
            //Group_Exp_Break.AppendChild(Group_Exp_Break_Loc);

            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
            //Tablix.AppendChild(SubReport_PageBreak);

            //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            //SubReport_PageBreak_Location.InnerText = "StartAndEnd";
            //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

            XmlElement SortExpressions = xml.CreateElement("SortExpressions");
            Tablix.AppendChild(SortExpressions);

            XmlElement SortExpression = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression);

            XmlElement SortExpression_Value = xml.CreateElement("Value");
            //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
            SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

            SortExpression.AppendChild(SortExpression_Value);

            XmlElement SortExpression_Direction = xml.CreateElement("Direction");
            SortExpression_Direction.InnerText = "Descending";
            SortExpression.AppendChild(SortExpression_Direction);


            XmlElement SortExpression1 = xml.CreateElement("SortExpression");
            SortExpressions.AppendChild(SortExpression1);

            XmlElement SortExpression_Value1 = xml.CreateElement("Value");
            //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
            SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
            SortExpression1.AppendChild(SortExpression_Value1);


            XmlElement Top = xml.CreateElement("Top");
            //Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            Top.InnerText = (0.01).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            //Top.InnerText = "0.60417in";
            Tablix.AppendChild(Top);

            XmlElement Left = xml.CreateElement("Left");
            Left.InnerText = "0.20417in";
            //Left.InnerText = "0.60417in";
            Tablix.AppendChild(Left);

            XmlElement Height1 = xml.CreateElement("Height");
            Height1.InnerText = "0.5in";
            Tablix.AppendChild(Height1);

            XmlElement Width1 = xml.CreateElement("Width");
            Width1.InnerText = "5.3229in";
            Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);


            //XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            //Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

            //XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
            //Break_After_SelParamRectangle_Location.InnerText = "End";
            //Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters


            //   Subreport
            ////////if (Summary_Sw)
            ////////{
            ////////    // Summary Sub Report 
            ////////}

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

            //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

            XmlElement Width = xml.CreateElement("Width");               // Total Page Width
            Width.InnerText = "6.5in";      //Common
            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            ////if (Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
            ////{
            ////    XmlElement PageHeader = xml.CreateElement("PageHeader");
            ////    Page.AppendChild(PageHeader);

            ////    XmlElement PageHeader_Height = xml.CreateElement("Height");
            ////    PageHeader_Height.InnerText = "0.51958in";
            ////    PageHeader.AppendChild(PageHeader_Height);

            ////    XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
            ////    PrintOnFirstPage.InnerText = "true";
            ////    PageHeader.AppendChild(PrintOnFirstPage);

            ////    XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
            ////    PrintOnLastPage.InnerText = "true";
            ////    PageHeader.AppendChild(PrintOnLastPage);


            ////    XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
            ////    PageHeader.AppendChild(Header_ReportItems);

            ////    ////if (true)
            ////    ////{
            ////    ////    XmlElement Header_TextBox = xml.CreateElement("Textbox");
            ////    ////    Header_TextBox.SetAttribute("Name", "HeaderTextBox");
            ////    ////    Header_ReportItems.AppendChild(Header_TextBox);

            ////    ////    XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
            ////    ////    HeaderTextBox_CanGrow.InnerText = "true";
            ////    ////    Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

            ////    ////    XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
            ////    ////    HeaderTextBox_Keep.InnerText = "true";
            ////    ////    Header_TextBox.AppendChild(HeaderTextBox_Keep);

            ////    ////    XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
            ////    ////    Header_TextBox.AppendChild(Header_Paragraphs);

            ////    ////    XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
            ////    ////    Header_Paragraphs.AppendChild(Header_Paragraph);

            ////    ////    XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
            ////    ////    Header_Paragraph.AppendChild(Header_TextRuns);

            ////    ////    XmlElement Header_TextRun = xml.CreateElement("TextRun");
            ////    ////    Header_TextRuns.AppendChild(Header_TextRun);

            ////    ////    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
            ////    ////    Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
            ////    ////    Header_TextRun.AppendChild(Header_TextRun_Value);

            ////    ////    XmlElement Header_TextRun_Style = xml.CreateElement("Style");
            ////    ////    Header_TextRun.AppendChild(Header_TextRun_Style);

            ////    ////    XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
            ////    ////    Header_Style_Font.InnerText = "Times New Roman";
            ////    ////    Header_TextRun_Style.AppendChild(Header_Style_Font);

            ////    ////    XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
            ////    ////    Header_Style_FontSize.InnerText = "16pt";
            ////    ////    Header_TextRun_Style.AppendChild(Header_Style_FontSize);

            ////    ////    XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
            ////    ////    Header_Style_TextDecoration.InnerText = "Underline";
            ////    ////    Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

            ////    ////    XmlElement Header_Style_Color = xml.CreateElement("Color");
            ////    ////    Header_Style_Color.InnerText = "#104cda";
            ////    ////    Header_TextRun_Style.AppendChild(Header_Style_Color);

            ////    ////    XmlElement Header_TextBox_Top = xml.CreateElement("Top");
            ////    ////    Header_TextBox_Top.InnerText = "0.24792in";
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_Top);

            ////    ////    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
            ////    ////    Header_TextBox_Left.InnerText = "0.42361in";
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_Left);

            ////    ////    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
            ////    ////    Header_TextBox_Height.InnerText = "0.30208in";
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_Height);

            ////    ////    XmlElement Header_TextBox_Width = xml.CreateElement("Width");
            ////    ////    //Header_TextBox_Width.InnerText = "10.30208in";
            ////    ////    Header_TextBox_Width.InnerText = "10in";
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_Width);

            ////    ////    XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
            ////    ////    Header_TextBox_ZIndex.InnerText = "1";
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_ZIndex);


            ////    ////    XmlElement Header_TextBox_Style = xml.CreateElement("Style");
            ////    ////    Header_TextBox.AppendChild(Header_TextBox_Style);

            ////    ////    XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
            ////    ////    Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

            ////    ////    XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
            ////    ////    Header_TB_StyleBorderStyle.InnerText = "None";
            ////    ////    Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

            ////    ////    XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
            ////    ////    Header_TB_SBS_LeftPad.InnerText = "2pt";
            ////    ////    Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

            ////    ////    XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
            ////    ////    Header_TB_SBS_RightPad.InnerText = "2pt";
            ////    ////    Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

            ////    ////    XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
            ////    ////    Header_TB_SBS_TopPad.InnerText = "2pt";
            ////    ////    Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

            ////    ////    XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
            ////    ////    Header_TB_SBS_BotPad.InnerText = "2pt";
            ////    ////    Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

            ////    ////    XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
            ////    ////    Header_Paragraph.AppendChild(Header_Text_Align_Style);

            ////    ////    XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
            ////    ////    Header_Text_Align.InnerText = "Center";
            ////    ////    Header_Text_Align_Style.AppendChild(Header_Text_Align);
            ////    ////}

            ////    //if (Include_Header_Image)
            ////    //{
            ////    //    // Add Image Heare
            ////    //}

            ////    XmlElement PageHeader_Style = xml.CreateElement("Style");
            ////    PageHeader.AppendChild(PageHeader_Style);

            ////    XmlElement PageHeader_Border = xml.CreateElement("Border");
            ////    PageHeader_Style.AppendChild(PageHeader_Border);

            ////    XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
            ////    PageHeader_Border_Style.InnerText = "None";
            ////    PageHeader_Border.AppendChild(PageHeader_Border_Style);


            ////    XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
            ////    PageHeader_BackgroundColor.InnerText = "White";
            ////    PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            ////}


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            ////if (true)
            ////{
            ////    XmlElement PageFooter = xml.CreateElement("PageFooter");
            ////    Page.AppendChild(PageFooter);

            ////    XmlElement PageFooter_Height = xml.CreateElement("Height");
            ////    PageFooter_Height.InnerText = "0.35083in";
            ////    PageFooter.AppendChild(PageFooter_Height);

            ////    XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
            ////    Footer_PrintOnFirstPage.InnerText = "true";
            ////    PageFooter.AppendChild(Footer_PrintOnFirstPage);

            ////    XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
            ////    Footer_PrintOnLastPage.InnerText = "true";
            ////    PageFooter.AppendChild(Footer_PrintOnLastPage);

            ////    XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
            ////    PageFooter.AppendChild(Footer_ReportItems);

            ////    if (true)
            ////    {
            ////        XmlElement Footer_TextBox = xml.CreateElement("Textbox");
            ////        Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
            ////        Footer_ReportItems.AppendChild(Footer_TextBox);

            ////        XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
            ////        FooterTextBox_CanGrow.InnerText = "true";
            ////        Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

            ////        XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
            ////        FooterTextBox_Keep.InnerText = "true";
            ////        Footer_TextBox.AppendChild(FooterTextBox_Keep);

            ////        XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
            ////        Footer_TextBox.AppendChild(Footer_Paragraphs);

            ////        XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
            ////        Footer_Paragraphs.AppendChild(Footer_Paragraph);

            ////        XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
            ////        Footer_Paragraph.AppendChild(Footer_TextRuns);

            ////        XmlElement Footer_TextRun = xml.CreateElement("TextRun");
            ////        Footer_TextRuns.AppendChild(Footer_TextRun);

            ////        XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
            ////        Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
            ////        Footer_TextRun.AppendChild(Footer_TextRun_Value);

            ////        XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
            ////        Footer_TextRun.AppendChild(Footer_TextRun_Style);

            ////        XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
            ////        Footer_TextBox_Top.InnerText = "0.06944in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Top);

            ////        XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
            ////        Footer_TextBox_Height.InnerText = "0.25in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Height);

            ////        XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
            ////        Footer_TextBox_Width.InnerText = "1.65625in";
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Width);


            ////        XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
            ////        Footer_TextBox.AppendChild(Footer_TextBox_Style);

            ////        XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
            ////        Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

            ////        XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
            ////        Footer_TB_StyleBorderStyle.InnerText = "None";
            ////        Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

            ////        XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
            ////        Footer_TB_SBS_LeftPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

            ////        XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
            ////        Footer_TB_SBS_RightPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

            ////        XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
            ////        Footer_TB_SBS_TopPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

            ////        XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
            ////        Footer_TB_SBS_BotPad.InnerText = "2pt";
            ////        Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

            ////        XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
            ////        Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

            ////        //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
            ////        //Header_Text_Align.InnerText = "Center";
            ////        //Header_Text_Align_Style.AppendChild(Header_Text_Align);
            ////    }
            ////}


            //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


            XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
            XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

            //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
            //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            if (false) //(Rb_A4_Port.Checked)
            {
                Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
                Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
            }
            else
            {
                Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
                Page_PageWidth.InnerText = "11in";            // Landscape "A4"
            }
            Page.AppendChild(Page_PageHeight);
            Page.AppendChild(Page_PageWidth);


            XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
            Page_LeftMargin.InnerText = "0.2in";
            Page.AppendChild(Page_LeftMargin);

            XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
            Page_RightMargin.InnerText = "0.2in";
            Page.AppendChild(Page_RightMargin);

            XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
            Page_TopMargin.InnerText = "0.2in";
            Page.AppendChild(Page_TopMargin);

            XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
            Page_BottomMargin.InnerText = "0.2in";
            Page.AppendChild(Page_BottomMargin);



            //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

            //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
            //EmbeddedImages.InnerText = "Image Attributes";
            //Report.AppendChild(EmbeddedImages);

            //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


            string s = xml.OuterXml;

            try
            {
                //xml.Save(@"C:\Capreports\" + Main_Rep_Name + "Detail_RdlC.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                Out_Rep_Name = "RNGB0014_Detail_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";

                xml.Save(ReportPath + Out_Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            
            //Console.ReadLine();   //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        }

        private void HeaderPage(Document document, PdfWriter writer)
        {
            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            ////BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            //BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            //iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            //iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            //iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblParamsHeaderFont = new iTextSharp.text.Font(bf_Calibri, 11, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#2e5f71")));
            iTextSharp.text.Font TblHeaderTitleFont = new iTextSharp.text.Font(bf_Calibri, 14, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font fnttimesRoman_Italic = new iTextSharp.text.Font(bf_TimesRomanI, 9, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")));

            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy("AGENCY", BaseForm.BaseAdminAgency, string.Empty, string.Empty, string.Empty, string.Empty);
            string _strImageFolderPath = "";
            if (hierarchyDetails != null)
            {
                string LogoName = hierarchyDetails.Logo.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";
                FileInfo info = new FileInfo(_strImageFolderPath + LogoName);
                if (info.Exists)
                    _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\" + LogoName;
                else
                    _strImageFolderPath = "";

            }

            Sel_AGY = Current_Hierarchy.Substring(0, 2);
            Sel_DEPT = Current_Hierarchy.Substring(2, 2);
            Sel_PROG = Current_Hierarchy.Substring(4, 2);

            PdfPTable outer = new PdfPTable(2);
            outer.TotalWidth = 510f;
            outer.LockedWidth = true;
            float[] widths2 = new float[] { 25f, 80f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
            outer.SetWidths(widths2);
            outer.WidthPercentage = 100;
            float[] Headerwidths = new float[] { 12f, 88f };
            outer.HorizontalAlignment = Element.ALIGN_CENTER;
            //outer.SpacingBefore = 40f;

            //border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.Padding = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(cellLogo);
            }

            PdfPCell row1 = new PdfPCell(new Phrase(PrivilegeEntity.Program + " - " + PrivilegeEntity.PrivilegeName, TblFontBold));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            outer.AddCell(row1);

            //PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            //row2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////row2.Colspan = 2;
            //row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(row2);

            //PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            //row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////row2.Colspan = 2;
            //row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(row21);

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.MinimumHeight = 6;
            row3.PaddingBottom = 5;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            outer.AddCell(row3);

            PdfPCell hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            hierarchy.PaddingBottom = 5;
            outer.AddCell(hierarchy);


            PdfPCell R3 = new PdfPCell(new Phrase("Agency: " + Sel_AGY + ", Department: " + Sel_DEPT + ", Program: " + Sel_PROG, TableFont));
            R3.Colspan = 2;
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R3.PaddingBottom = 5;
            outer.AddCell(R3);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Case Type", TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            outer.AddCell(R4);

            string Text1 = ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString().Trim();
            PdfPCell R5 = new PdfPCell(new Phrase(Text1, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R5.PaddingBottom = 5;
            outer.AddCell(R5);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Program", TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            outer.AddCell(R6);

            //PdfPCell R7 = new PdfPCell(new Phrase(" : " + ((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Text.ToString(), TableFont));
            //R7.HorizontalAlignment = Element.ALIGN_LEFT;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R7);

            string Sel_Programs = string.Empty;
            if (rbSelProgram.Checked == true)
            {
                if (SelectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity Entity in SelectedHierarchies)
                    {
                        Sel_Programs += Entity.Agency + Entity.Dept + Entity.Prog + ", ";
                    }

                    if (Sel_Programs.Length > 0)
                        Sel_Programs = Sel_Programs.Substring(0, (Sel_Programs.Length - 2));//1));
                }
            }

            Text1 = /*" : " + */(rbAllPrograms.Checked == true ? "All" : Sel_Programs);
            PdfPCell R7 = new PdfPCell(new Phrase(Text1, TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R7.PaddingBottom = 5;
            outer.AddCell(R7);

            string selectedAlertCodes = SelectedAlertCodes;
            selectedAlertCodes = selectedAlertCodes.Replace("'", "");
            selectedAlertCodes = selectedAlertCodes.Replace(",", ", ");

            PdfPCell alertCodes = new PdfPCell(new Phrase("  " + "Alert Codes", TableFont));
            alertCodes.HorizontalAlignment = Element.ALIGN_LEFT;
            alertCodes.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            alertCodes.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            alertCodes.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            alertCodes.PaddingBottom = 5;
            outer.AddCell(alertCodes);

            PdfPCell alertCodes1 = new PdfPCell(new Phrase((rdbAllAlertCds.Checked == true ? "All" : selectedAlertCodes), TableFont));
            alertCodes1.HorizontalAlignment = Element.ALIGN_LEFT;
            alertCodes1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            alertCodes1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            alertCodes1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            alertCodes1.PaddingBottom = 5;
            outer.AddCell(alertCodes1);

            PdfPCell RFund = new PdfPCell(new Phrase("  " + "Funding Source", TableFont));
            RFund.HorizontalAlignment = Element.ALIGN_LEFT;
            RFund.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RFund.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RFund.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RFund.PaddingBottom = 5;
            outer.AddCell(RFund);

            string strSelectFund = Search_Entity.CA_Fund_Filter;
            strSelectFund = strSelectFund.Replace("'", "");
            strSelectFund = strSelectFund.Trim().Replace(" ,", ", ");

            Text1 = /*" : " + */(Rb_Fund_All.Checked == true ? "All" : strSelectFund);
            PdfPCell RFundType = new PdfPCell(new Phrase(Text1, TableFont));
            RFundType.HorizontalAlignment = Element.ALIGN_LEFT;
            RFundType.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RFundType.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RFundType.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RFundType.PaddingBottom = 5;
            outer.AddCell(RFundType);

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Case Status", TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            outer.AddCell(R8);

            PdfPCell R9 = new PdfPCell(new Phrase(/*" : " + */Sel_params_To_Print[1], TableFont));
            R9.HorizontalAlignment = Element.ALIGN_LEFT;
            R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R9.PaddingBottom = 5;
            outer.AddCell(R9);

            PdfPCell R10 = new PdfPCell(new Phrase("  " + "Date Selection", TableFont));
            R10.HorizontalAlignment = Element.ALIGN_LEFT;
            R10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R10.PaddingBottom = 5;
            outer.AddCell(R10);

            Text1 = /*" : " + */(Rb_MS_Date.Checked ? "Outcome Date" : " ") + (Rb_MS_AddDate.Checked ? "Outcome Add Date" : " ");
            PdfPCell R11 = new PdfPCell(new Phrase(Text1, TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            outer.AddCell(R11);


            //PdfPCell R12 = new PdfPCell(new Phrase("  " + "Code", TableFont));
            PdfPCell R12 = new PdfPCell(new Phrase("  " + "Report Description", TableFont));
            R12.HorizontalAlignment = Element.ALIGN_LEFT;
            R12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R12.PaddingBottom = 5;
            outer.AddCell(R12);

            //Text1 = /*" : " + */((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Text.ToString(), TableFont)
            PdfPCell R13 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Text.ToString(), TableFont));
            R13.HorizontalAlignment = Element.ALIGN_LEFT;
            R13.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R13.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R13.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R13.PaddingBottom = 5;
            outer.AddCell(R13);

            PdfPCell R34 = new PdfPCell(new Phrase("  " + "Report For", TableFont));
            R34.HorizontalAlignment = Element.ALIGN_LEFT;
            R34.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R34.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R34.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R34.PaddingBottom = 5;
            outer.AddCell(R34);

            string strrptfor = "Report Period";
            //**if (rdoperiodCumulative.Checked)
            //**    strrptfor = "Reference Period";
            if (rdoperiodBoth.Checked)
                strrptfor = "Report and Reference Period";


            PdfPCell R35 = new PdfPCell(new Phrase(/*" : " + */strrptfor, TableFont));
            R35.HorizontalAlignment = Element.ALIGN_LEFT;
            R35.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R35.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R35.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R35.PaddingBottom = 5;
            outer.AddCell(R35);

            if (rdoperiodBoth.Checked)
            {
                PdfPCell RepPerControl = new PdfPCell(new Phrase("  " + "Report Period Control", TableFont));
                RepPerControl.HorizontalAlignment = Element.ALIGN_LEFT;
                RepPerControl.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RepPerControl.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RepPerControl.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RepPerControl.PaddingBottom = 5;
                outer.AddCell(RepPerControl);

                PdfPCell RepPerControldesc;
                if (chkRepControl.Checked)
                {
                    RepPerControldesc = new PdfPCell(new Phrase("Yes", TableFont));
                }
                else
                {
                    RepPerControldesc = new PdfPCell(new Phrase("No", TableFont));
                }
                RepPerControldesc.HorizontalAlignment = Element.ALIGN_LEFT;
                RepPerControldesc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RepPerControldesc.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RepPerControldesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RepPerControldesc.PaddingBottom = 5;
                outer.AddCell(RepPerControldesc);

            }

            if (!rdoperiod.Checked)
            {
                PdfPCell R14 = new PdfPCell(new Phrase("  " + "Reference Period Date", TableFont));
                R14.HorizontalAlignment = Element.ALIGN_LEFT;
                R14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R14.PaddingBottom = 5;
                outer.AddCell(R14);

                string Date = "From: " + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                + "     To: " + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                PdfPCell R15 = new PdfPCell(new Phrase(Date, TableFont));
                R15.HorizontalAlignment = Element.ALIGN_LEFT;
                R15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R15.PaddingBottom = 5;
                outer.AddCell(R15);

            }

            if (!rdoperiodCumulative.Checked)
            {
                PdfPCell R16 = new PdfPCell(new Phrase("  " + "Report Period Date", TableFont));
                R16.HorizontalAlignment = Element.ALIGN_LEFT;
                R16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R16.PaddingBottom = 5;
                outer.AddCell(R16);

                string Date = "From: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                                + "     To: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                PdfPCell R17 = new PdfPCell(new Phrase(Date, TableFont));
                R17.HorizontalAlignment = Element.ALIGN_LEFT;
                R17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R17.PaddingBottom = 5;
                outer.AddCell(R17);
            }

            string selectedSites = Search_Entity.Mst_Site;
            selectedSites = selectedSites.Replace("'", "");
            selectedSites = selectedSites.Replace(" ,", ", ");

            PdfPCell R18 = new PdfPCell(new Phrase("  " + "Intake Site", TableFont));
            R18.HorizontalAlignment = Element.ALIGN_LEFT;
            R18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R18.PaddingBottom = 5;
            outer.AddCell(R18);

            Text1 = /*" : " + */(Rb_Site_All.Checked ? "All Sites" : " ") + (Rb_Site_Sel.Checked ? selectedSites : "") + (Rb_Site_No.Checked ? "No Sites" : "");
            PdfPCell R19 = new PdfPCell(new Phrase(Text1, TableFont));
            R19.HorizontalAlignment = Element.ALIGN_LEFT;
            R19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R19.PaddingBottom = 5;
            outer.AddCell(R19);

            string selectedSites2 = Search_Entity.CaseMssite;
            selectedSites2 = selectedSites2.Replace("'", "");
            selectedSites2 = selectedSites2.Replace(" ,", ", ");

            PdfPCell R18mssite = new PdfPCell(new Phrase("  " + "Outcome Posting Site", TableFont));
            R18mssite.HorizontalAlignment = Element.ALIGN_LEFT;
            R18mssite.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R18mssite.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R18mssite.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R18mssite.PaddingBottom = 5;
            outer.AddCell(R18mssite);

            Text1 = /*" : " + */(rdoMssiteall.Checked ? "All Sites" : " ") + (rdoMsselectsite.Checked ? selectedSites2 : "") + (rdomsNosite.Checked ? "No Sites" : "");
            PdfPCell R19mssite = new PdfPCell(new Phrase(Text1, TableFont));
            R19mssite.HorizontalAlignment = Element.ALIGN_LEFT;
            R19mssite.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R19mssite.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R19mssite.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R19mssite.PaddingBottom = 5;
            outer.AddCell(R19mssite);

            //PdfPCell R20 = new PdfPCell(new Phrase("  " + "Categories", TableFont));
            //R20.HorizontalAlignment = Element.ALIGN_LEFT;
            //R20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //R20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            //R20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            //R20.PaddingBottom = 5;
            //outer.AddCell(R20);

            //Text1 = /*" : " + */(Rb_Agy_Def.Checked ? "All" : "Only Goal Associated");
            //PdfPCell R21 = new PdfPCell(new Phrase(Text1, TableFont));
            //R21.HorizontalAlignment = Element.ALIGN_LEFT;
            //R21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //R21.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            //R21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            //R21.PaddingBottom = 5;
            //outer.AddCell(R21);


            PdfPCell R22 = new PdfPCell(new Phrase("  " + "Produce Details", TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R22.PaddingBottom = 5;
            outer.AddCell(R22);

            Text1 = /*" : " + */(Rb_Details_Yes.Checked ? "Yes" : "No");
            PdfPCell R23 = new PdfPCell(new Phrase(Text1, TableFont));
            R23.HorizontalAlignment = Element.ALIGN_LEFT;
            R23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R23.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R23.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R23.PaddingBottom = 5;
            outer.AddCell(R23);


            PdfPCell R24 = new PdfPCell(new Phrase("  " + "Poverty Level", TableFont));
            R24.HorizontalAlignment = Element.ALIGN_LEFT;
            R24.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R24.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R24.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R24.PaddingBottom = 5;
            outer.AddCell(R24);

            Text1 = "From: " + Txt_Pov_Low.Text + "     To: " + Txt_Pov_High.Text;
            PdfPCell R25 = new PdfPCell(new Phrase(Text1, TableFont));
            R25.HorizontalAlignment = Element.ALIGN_LEFT;
            R25.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R25.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R25.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R25.PaddingBottom = 5;
            outer.AddCell(R25);

            string selectedDomain = Search_Entity.ZipCode;
            selectedDomain = selectedDomain.Replace("'", "");
            selectedDomain = selectedDomain.Replace(" ,", ", ");

            PdfPCell R26 = new PdfPCell(new Phrase("  " + "Domain", TableFont));
            R26.HorizontalAlignment = Element.ALIGN_LEFT;
            R26.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R26.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R26.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R26.PaddingBottom = 5;
            outer.AddCell(R26);

            Text1 = /*" : " + */(Rb_Zip_All.Checked ? "All Domains" : selectedDomain);
            PdfPCell R27 = new PdfPCell(new Phrase(Text1, TableFont));
            R27.HorizontalAlignment = Element.ALIGN_LEFT;
            R27.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R27.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R27.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R27.PaddingBottom = 5;
            outer.AddCell(R27);

            string selectedCounty = Search_Entity.County;
            selectedCounty = selectedCounty.Replace("'", "");
            selectedCounty = selectedCounty.Replace(" ,", ", ");

            PdfPCell R28 = new PdfPCell(new Phrase("  " + "County", TableFont));
            R28.HorizontalAlignment = Element.ALIGN_LEFT;
            R28.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R28.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R28.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R28.PaddingBottom = 5;
            outer.AddCell(R28);

            Text1 = /*" : " + */(Rb_County_All.Checked ? "All" : selectedCounty);
            PdfPCell R29 = new PdfPCell(new Phrase(Text1, TableFont));
            R29.HorizontalAlignment = Element.ALIGN_LEFT;
            R29.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R29.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R29.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R29.PaddingBottom = 5;
            outer.AddCell(R29);

            PdfPCell R30 = new PdfPCell(new Phrase("  " + "Secret Applications", TableFont));
            R30.HorizontalAlignment = Element.ALIGN_LEFT;
            R30.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R30.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R30.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R30.PaddingBottom = 5;
            outer.AddCell(R30);

            Text1 = (Rb_Mst_NonSec.Checked ? "Non-Secret Only" : " ") + (Rb_Mst_Sec.Checked ? "Secret Only" : "") + (Rb_Mst_BothSec.Checked ? "Both Non-Secret and Secret" : "");
            PdfPCell R31 = new PdfPCell(new Phrase(Text1, TableFont));
            R31.HorizontalAlignment = Element.ALIGN_LEFT;
            R31.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R31.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R31.PaddingBottom = 5;
            outer.AddCell(R31);


            PdfPCell R32 = new PdfPCell(new Phrase("  " + "Report Format", TableFont));
            R32.HorizontalAlignment = Element.ALIGN_LEFT;
            R32.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R32.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R32.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R32.PaddingBottom = 5;
            outer.AddCell(R32);

            string RepFormat = string.Empty;
            if (Rb_OBO_Mem.Checked)
                RepFormat = Rb_OBO_Mem.Text.Trim();
            else if (Rb_SNP_Mem.Checked)
                RepFormat = Rb_SNP_Mem.Text.Trim();

            //if (rdoperiodBoth.Checked)
            //{
            PdfPCell cell_Content_Desc19 = new PdfPCell(new Phrase(RepFormat, TableFont));
            cell_Content_Desc19.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc19.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc19);
            //}
            //else
            //{
            PdfPCell RepFormDet = new PdfPCell(new Phrase("", TableFont));
            RepFormDet.HorizontalAlignment = Element.ALIGN_LEFT;
            RepFormDet.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RepFormDet.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RepFormDet.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RepFormDet.PaddingBottom = 5;
            outer.AddCell(RepFormDet);

            /*PdfPCell*/
            cell_Content_Desc19 = new PdfPCell(new Phrase("" +
RepFormat + (Rb_OBO_Mem.Checked == true ? (chkbUndupTable1.Checked == true ? " - Unduplicated at Group Level: Yes" : " - Unduplicated at Group Level: No") : ""), TableFont));
            cell_Content_Desc19.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc19.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc19);
            //}


            PdfPCell R34out = new PdfPCell(new Phrase("  " + "Print Outcomes", TableFont));
            R34out.HorizontalAlignment = Element.ALIGN_LEFT;
            R34out.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R34out.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R34out.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R34out.PaddingBottom = 5;
            outer.AddCell(R34out);

            Text1 = (rdoOutcomesAll.Checked ? "Print All Outcomes" : "Print Only Outcomes with Counts");
            PdfPCell R35out = new PdfPCell(new Phrase(Text1, TableFont));
            R35out.HorizontalAlignment = Element.ALIGN_LEFT;
            R35out.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R35out.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R35out.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R35out.PaddingBottom = 5;
            outer.AddCell(R35out);

            document.Add(outer);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }



        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        string strReasonCodes = string.Empty;
        string PdfName;
        private void On_SaveForm_Closed(DataTable dtResult, DataTable dtResultBoth)
        {

            strReasonCodes = string.Empty;

            PdfName = "RNGB0014_MainReport";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);// 15, 15, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
           
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            cb = writer.DirectContent;

            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 10, 1, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_calibri, 8, 1);//(1, 7, 1);
            iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(bf_calibri, 12, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            //Added by Vikash on 01/30/2024 as a part of design enahncement following ROMA Outcome Report - Enhancements document.
            //**BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font fcRedNew = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font TableFontNew = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TblFontBoldCenter = new iTextSharp.text.Font(bf_calibri, 8, 1);
            iTextSharp.text.Font TableFontNewCenter = new iTextSharp.text.Font(bf_calibri, 8);


            HeaderPage(document,writer);
            try
            {
                document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());

                document.NewPage();

                //PdfPTable table = new PdfPTable(6);

               
                // table.FooterRows = 3;

                List<CommonEntity> commonHeaderlist = new List<CommonEntity>();

                //cb.BeginText();
                ////cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 12);

                ////List<CSB4AsocEntity> CSb4Entity;
                ////CSb4Entity = _model.SPAdminData.Browse_CSB4Assoc(string.Empty, string.Empty);
                //DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

                //cb.SetFontAndSize(bfTimesBold, 10);
                //cb.SetRGBColorFill(4, 4, 15);
                //X_Pos = 300; Y_Pos = 785;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section A: Individual and Family National Performance Indicators (NPIs) - Data Entry Form", X_Pos, Y_Pos, 0);
                //Y_Pos -= 15;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low incomes are stable and achieve economic security. ", X_Pos, Y_Pos, 0);
                //Y_Pos -= 15;
                //cb.EndText();

                // CaptainModel _model = new CaptainModel();

                List<RCsb14GroupEntity> RngGrouplist = new List<RCsb14GroupEntity>();
                List<RCsb14GroupEntity> RngtblCodelist = new List<RCsb14GroupEntity>();
                List<RCsb14GroupEntity> RngResultlist = new List<RCsb14GroupEntity>();

                List<RCsb14GroupEntity> RngBothResultlist = new List<RCsb14GroupEntity>();

                if (rdoperiodBoth.Checked) //&& Rb_SNP_Mem.Checked)
                {
                    foreach (DataRow drResultBothitem in dtResultBoth.Rows)
                    {
                        RngBothResultlist.Add(new RCsb14GroupEntity(drResultBothitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                    }
                }

                foreach (DataRow drResultitem in dtResult.Rows)
                {
                    RngResultlist.Add(new RCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                }

                List<RNGGAEntity> GoalDetailsEntity = _model.SPAdminData.Browse_RNGGA(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), string.Empty, string.Empty, string.Empty);
                if (RngCodelist.Count > 0)
                {
                    RngGrouplist = RngCodelist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() == string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString() && u.Code == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                    RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() != string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString() && u.Code == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                }
                if (rdoOutcomesselect.Checked)
                {

                    List<RCsb14GroupEntity> rngindswithclist = RngtblCodelist.FindAll(u => u.IndSwitch == "Y");
                    foreach (RCsb14GroupEntity itemindswitch in rngindswithclist)
                    {
                        List<RCsb14GroupEntity> rngtblcodelist = RngResultlist.FindAll(u => u.TblCode == itemindswitch.TblCode);
                        if (rngtblcodelist.Count == 0)
                        {
                            List<RNGGAEntity> GoalDetailsIndswitchEntity = GoalDetailsEntity.FindAll(u => u.TblCode.Trim() == itemindswitch.TblCode.Trim() && u.GrpCode.Trim() == itemindswitch.GrpCode.Trim() && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString().Trim());
                            foreach (RNGGAEntity itemGoalind in GoalDetailsIndswitchEntity)
                            {
                                RngResultlist.Add(new RCsb14GroupEntity(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), itemindswitch.GrpCode, itemindswitch.TblCode, itemGoalind.Desc, "0", "0", "0", "0", "0", itemindswitch.unit_cnt, itemindswitch.ExAchev, itemindswitch.per_Achived, itemindswitch.CalCost, itemindswitch.Count_type, "INDSWITCH"));
                            }

                        }
                        if (rdoperiodBoth.Checked) //&& Rb_SNP_Mem.Checked)
                        {
                            List<RCsb14GroupEntity> rngtblbothcodelist = RngBothResultlist.FindAll(u => u.TblCode == itemindswitch.TblCode);
                            if (rngtblcodelist.Count == 0)
                            {
                                List<RNGGAEntity> GoalDetailsIndswitchEntity = GoalDetailsEntity.FindAll(u => u.TblCode.Trim() == itemindswitch.TblCode.Trim() && u.GrpCode.Trim() == itemindswitch.GrpCode.Trim() && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString().Trim());
                                foreach (RNGGAEntity itemGoalind in GoalDetailsIndswitchEntity)
                                {
                                    RngBothResultlist.Add(new RCsb14GroupEntity(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), itemindswitch.GrpCode, itemindswitch.TblCode, itemGoalind.Desc, "0", "0", "0", "0", "0", itemindswitch.unit_cnt, itemindswitch.ExAchev, itemindswitch.per_Achived, itemindswitch.CalCost, itemindswitch.Count_type, "INDSWITCH"));
                                }
                            }
                        }
                    }
                }



                bool boolGroupFilter = true;
                bool boolGoalAssoFilter = true;

                foreach (RCsb14GroupEntity codeitem in RngGrouplist)
                {
                    //Commented by Sudheer on 07/10/23
                    //if (Rb_User_Def.Checked)
                    //{
                    //    boolGoalAssoFilter = false;
                    //    if (GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim()).Count > 0)
                    //        boolGoalAssoFilter = true;
                    //}
                    if (Rb_Zip_Sel.Checked)
                    {
                        boolGroupFilter = false;
                        if (ListRngGroupCode.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim()).Count > 0)
                            boolGroupFilter = true;
                    }
                    string Agy = string.Empty;
                    if (Current_Hierarchy.Substring(0, 2).ToString() != "**")
                        Agy = Current_Hierarchy.Substring(0, 2);
                    string Agency = string.Empty;
                    DataSet dsAgency = new DataSet();
                    if (!string.IsNullOrEmpty(Agy.Trim()))
                    {
                        dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(Agy, null, null, null, null, null, null);
                        if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
                            Agency = dsAgency.Tables[0].Rows[0]["ACR_NAME"].ToString().Trim();
                    }

                    if ((boolGroupFilter) && (boolGoalAssoFilter))
                    {
                        string strResultColumn = codeitem.ExAchev.ToString();
                        //PdfPCell pdfMainHeader = new PdfPCell(new Phrase("Module 4, Section A: Individual and Family National Performance Indicators (NPIs) - Data Entry Form", TblFontHeadBold));
                        //pdfMainHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        //pdfMainHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //pdfMainHeader.Colspan = 6;
                        //table.AddCell(pdfMainHeader);

                        //PdfPCell pdfMainHeader2 = new PdfPCell(new Phrase("Goal 1: Individuals and Families with low incomes are stable and achieve economic security. ", TblFontHeadBold));
                        //pdfMainHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                        //pdfMainHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //pdfMainHeader2.Colspan = 6;
                        //table.AddCell(pdfMainHeader2);

                        PdfPTable table = new PdfPTable(6);
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;

                        table.TotalWidth = 750f;

                        float[] widths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f };
                        table.SetWidths(widths);


                        table.HeaderRows = 2;
                        table.HorizontalAlignment = Element.ALIGN_CENTER;



                        PdfPCell pdfMainSubHeader = new PdfPCell(new Phrase(codeitem.GrpDesc, fcRedNew));//fcRed));//+ " Indicators"
                        pdfMainSubHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainSubHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfMainSubHeader.Colspan = 6;
                        table.AddCell(pdfMainSubHeader);


                        //PdfPCell pdfMainSubHeader1 = new PdfPCell(new Phrase("Name of CSBG Eligible Entity Reporting: __________________________________", TableFont));
                        //pdfMainSubHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //pdfMainSubHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //pdfMainSubHeader1.Colspan = 6;
                        //table.AddCell(pdfMainSubHeader1);

                        //Added by Sudheer on 06/30/2018
                        PdfPCell pdfMainSubHeader1 = new PdfPCell(new Phrase("Department/Program: " + DeptName + ProgName, TableFontNew));
                        pdfMainSubHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainSubHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainSubHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfMainSubHeader1.Colspan = 6;
                        pdfMainSubHeader1.FixedHeight = 20;
                        table.AddCell(pdfMainSubHeader1);



                        int intcolumns = 6;
                        int intheadercount = 0;
                        if (codeitem.Incld1.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld2.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld3.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld4.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld5.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (strResultColumn == "0")
                            intheadercount = intheadercount + 1;
                        if (intheadercount > 1)
                            intcolumns = intcolumns + (intheadercount - 1);

                        PdfPTable subtable = new PdfPTable(intcolumns);

                        subtable.WidthPercentage = 100;
                        subtable.LockedWidth = true;

                        subtable.TotalWidth = 750f;

                        if (rdoperiodBoth.Checked)
                            subtable.HeaderRows = 6;
                        else
                            subtable.HeaderRows = 5;

                        float[] subwidths = null;
                        if (intcolumns == 6)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f };
                        if (intcolumns == 7)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f };
                        if (intcolumns == 8)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        if (intcolumns == 9)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        if (intcolumns == 10)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        if (intcolumns == 11)
                            subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };

                        subtable.SetWidths(subwidths);

                        PdfPCell pdfMainSubHeader2 = new PdfPCell(new Phrase("Name of CSBG Eligible Entity Reporting:  ", TblFontBold));
                        pdfMainSubHeader2.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainSubHeader2.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainSubHeader2.Border = iTextSharp.text.Rectangle.BOX;
                        //pdfMainSubHeader2.Colspan = 6;
                        pdfMainSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        pdfMainSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        subtable.AddCell(pdfMainSubHeader2);

                        PdfPCell pdfMainSubHeader3 = new PdfPCell(new Phrase(Agency, TableFontNew));
                        pdfMainSubHeader3.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainSubHeader3.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainSubHeader3.Border = iTextSharp.text.Rectangle.BOX;
                        pdfMainSubHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        pdfMainSubHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        pdfMainSubHeader3.Colspan = intcolumns - 1;
                        subtable.AddCell(pdfMainSubHeader3);

                        PdfPCell pdfMainSubHeader4 = new PdfPCell(new Phrase("State:  ", TblFontBold));
                        pdfMainSubHeader4.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainSubHeader4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainSubHeader4.Border = iTextSharp.text.Rectangle.BOX;
                        pdfMainSubHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        pdfMainSubHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        //pdfMainSubHeader2.Colspan = 6;
                        subtable.AddCell(pdfMainSubHeader4);

                        PdfPCell pdfMainSubHeader5 = new PdfPCell(new Phrase("DUNS: ", TblFontBold));
                        pdfMainSubHeader5.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainSubHeader5.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfMainSubHeader5.Border = iTextSharp.text.Rectangle.BOX;
                        pdfMainSubHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        pdfMainSubHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        pdfMainSubHeader5.Colspan = intcolumns - 1;
                        subtable.AddCell(pdfMainSubHeader5);

                        PdfPCell pdfMainSubHeaderSpace = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainSubHeaderSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfMainSubHeaderSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfMainSubHeaderSpace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainSubHeaderSpace.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));// ("#ebf4fa"));
                        pdfMainSubHeaderSpace.Colspan = intcolumns;
                        subtable.AddCell(pdfMainSubHeaderSpace);

                        PdfPCell pdfMainTableHeader1 = new PdfPCell(new Phrase("", TblFontBold));
                        pdfMainTableHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfMainTableHeader1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableHeader1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader1);

                        PdfPCell pdfMainTableHeader2 = new PdfPCell(new Phrase("I.) Number of Individuals Served ", TblFontBold));
                        //PdfPCell pdfMainTableHeader2 = new PdfPCell(new Phrase("Target", TblFontBold));
                        pdfMainTableHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableHeader2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader2);


                        PdfPCell pdfMainTableHeader3 = new PdfPCell(new Phrase("II.) Target (#)", TblFontBold));
                        //PdfPCell pdfMainTableHeader3 = new PdfPCell(new Phrase("# of Individuals Served", TblFontBold));
                        pdfMainTableHeader3.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainTableHeader3.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader3.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader3);

                        PdfPCell pdfMainTableHeader4 = new PdfPCell(new Phrase("III.) Actual Results (#) ", TblFontBold));
                        //PdfPCell pdfMainTableHeader4 = new PdfPCell(new Phrase("Actual Results", TblFontBold));
                        pdfMainTableHeader4.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainTableHeader4.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader4.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader4);

                        int intcolumnscount = 3;
                        if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + codeitem.Hrd1, TblFontBold));
                            //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(codeitem.Hrd1, TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }

                        if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + codeitem.Hrd2, TblFontBold));
                            //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(codeitem.Hrd2, TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + codeitem.Hrd3, TblFontBold));
                            //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(codeitem.Hrd3, TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + codeitem.Hrd4, TblFontBold));
                            //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(codeitem.Hrd4, TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + codeitem.Hrd5, TblFontBold));
                            //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(codeitem.Hrd5, TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }

                        intcolumnscount = intcolumnscount + 1;
                        PdfPCell pdfMainTableHeader5 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + "Percentage Achieving Outcome", TblFontBold));
                        //PdfPCell pdfMainTableHeader5 = new PdfPCell(new Phrase("Percentage Achieving Outcome", TblFontBold));
                        pdfMainTableHeader5.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainTableHeader5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader5.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                        pdfMainTableHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader5);

                        intcolumnscount = intcolumnscount + 1;
                        PdfPCell pdfMainTableHeader6 = new PdfPCell(new Phrase(HeadersValue(intcolumnscount) + "Performance Target Accuracy", TblFontBold));
                        //PdfPCell pdfMainTableHeader6 = new PdfPCell(new Phrase("Performance Target Accuracy", TblFontBold));
                        pdfMainTableHeader6.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfMainTableHeader6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableHeader6.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableHeader6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableHeader6);

                        PdfPCell pdfMainTableSubHeader1 = new PdfPCell(new Phrase(codeitem.GrpDesc, TblFontBold));
                        pdfMainTableSubHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableSubHeader1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader1);

                        PdfPCell pdfMainTableSubHeader2 = new PdfPCell(new Phrase("in program(s) (#)", TableFontNew));
                        //PdfPCell pdfMainTableSubHeader2 = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainTableSubHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader2);

                        PdfPCell pdfMainTableSubHeader3 = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainTableSubHeader3.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader3.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader3.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableSubHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader3);

                        PdfPCell pdfMainTableSubHeader4 = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainTableSubHeader4.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader4.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableSubHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader4);

                        if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                        {
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                        {
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                        {
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                        {
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }
                        if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                        {
                            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTableIncld2);
                        }


                        PdfPCell pdfMainTableSubHeader5 = new PdfPCell(new Phrase("[III/ I = " + HeadersValue1(intcolumnscount - 1) + " ] (% auto calculated) ", TableFontNew));
                        //PdfPCell pdfMainTableSubHeader5 = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainTableSubHeader5.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        pdfMainTableSubHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader5);

                        PdfPCell pdfMainTableSubHeader6 = new PdfPCell(new Phrase("(III/II = " + HeadersValue1(intcolumnscount) + " ] (% auto calculated) ", TableFontNew));
                        //PdfPCell pdfMainTableSubHeader6 = new PdfPCell(new Phrase("", TableFontNew));
                        pdfMainTableSubHeader6.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableSubHeader6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        pdfMainTableSubHeader6.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                        pdfMainTableSubHeader6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableSubHeader6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        subtable.AddCell(pdfMainTableSubHeader6);



                        if (rdoperiodBoth.Checked)
                        {
                            PdfPTable NestedTable1 = new PdfPTable(2);
                            NestedTable1.TotalWidth = 60f;
                            NestedTable1.LockedWidth = true;
                            float[] widths2 = new float[] { 30f, 30f };
                            NestedTable1.SetWidths(widths2);
                            NestedTable1.WidthPercentage = 100;
                            float[] Headerwidths = new float[] { 12f, 88f };
                            NestedTable1.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPCell pdfMainTablebothSubHeader1 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTablebothSubHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTablebothSubHeader1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader1);

                            //PdfPCell pdfMainTablebothSubHeader2 = new PdfPCell(new Phrase("", TblFontBold));
                            //pdfMainTablebothSubHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //pdfMainTablebothSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader2);

                            PdfPCell Cell3 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                            Cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell3);

                            PdfPCell Cell33 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                            Cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell33.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell33);

                            PdfPCell pdfMainTablebothSubHeader3 = new PdfPCell(NestedTable1);
                            pdfMainTablebothSubHeader3.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader3.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTablebothSubHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader3);
                            NestedTable1.DeleteBodyRows();

                            PdfPCell pdfMainTablebothSubHeader2 = new PdfPCell(new Phrase("", TblFontBold));
                            pdfMainTablebothSubHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTablebothSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader2);

                            PdfPCell Cell4 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                            Cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell4);

                            PdfPCell Cell44 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                            Cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell44.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell44);

                            PdfPCell pdfMainTablebothSubHeader4 = new PdfPCell(NestedTable1);
                            pdfMainTablebothSubHeader4.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTablebothSubHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader4);
                            NestedTable1.DeleteBodyRows();

                            if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                            {
                                PdfPCell Cell5 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell5);

                                PdfPCell Cell55 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell55);

                                PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable1);
                                pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                subtable.AddCell(pdfMainTableIncld2);
                                NestedTable1.DeleteBodyRows();
                            }
                            if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                            {
                                PdfPCell Cell5 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell5);

                                PdfPCell Cell55 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell55);

                                PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable1);
                                pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                subtable.AddCell(pdfMainTableIncld2);
                                NestedTable1.DeleteBodyRows();
                            }
                            if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                            {
                                PdfPCell Cell5 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell5);

                                PdfPCell Cell55 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell55);

                                PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable1);
                                pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                subtable.AddCell(pdfMainTableIncld2);
                                NestedTable1.DeleteBodyRows();
                            }
                            if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                            {
                                PdfPCell Cell5 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell5);

                                PdfPCell Cell55 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell55);

                                PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable1);
                                pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                subtable.AddCell(pdfMainTableIncld2);
                                NestedTable1.DeleteBodyRows();
                            }
                            if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                            {
                                PdfPCell Cell5 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell5);

                                PdfPCell Cell55 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(Cell55);

                                PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable1);
                                pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                subtable.AddCell(pdfMainTableIncld2);
                                NestedTable1.DeleteBodyRows();
                            }
                            PdfPCell Cell6 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                            Cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell6);

                            PdfPCell Cell66 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                            Cell66.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell66.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell66);

                            PdfPCell pdfMainTablebothSubHeader5 = new PdfPCell(NestedTable1);
                            pdfMainTablebothSubHeader5.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            pdfMainTablebothSubHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader5);
                            NestedTable1.DeleteBodyRows();

                            PdfPCell Cell7 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                            Cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell7);

                            PdfPCell Cell77 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                            Cell77.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell77.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            NestedTable1.AddCell(Cell77);

                            PdfPCell pdfMainTablebothSubHeader6 = new PdfPCell(NestedTable1);
                            pdfMainTablebothSubHeader6.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfMainTablebothSubHeader6.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            pdfMainTablebothSubHeader6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfMainTablebothSubHeader6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            subtable.AddCell(pdfMainTablebothSubHeader6);

                            ////**PdfPCell pdfMainTablebothSubHeader2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //PdfPCell pdfMainTablebothSubHeader2 = new PdfPCell(new Phrase("", TblFontBold));
                            //pdfMainTablebothSubHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //pdfMainTablebothSubHeader2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //pdfMainTablebothSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader2);

                            ////**PdfPCell pdfMainTablebothSubHeader3 = new PdfPCell(new Phrase("", TblFontBold));
                            //PdfPCell pdfMainTablebothSubHeader3 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //pdfMainTablebothSubHeader3.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader3.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //pdfMainTablebothSubHeader3.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //pdfMainTablebothSubHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader3);

                            //PdfPCell pdfMainTablebothSubHeader4 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //pdfMainTablebothSubHeader4.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader4.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //pdfMainTablebothSubHeader4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //pdfMainTablebothSubHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader4);

                            //if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                            //{
                            //    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //    subtable.AddCell(pdfMainTableIncld2);
                            //}
                            //if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                            //{
                            //    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //    subtable.AddCell(pdfMainTableIncld2);
                            //}
                            //if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                            //{
                            //    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //    subtable.AddCell(pdfMainTableIncld2);
                            //}
                            //if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                            //{
                            //    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //    subtable.AddCell(pdfMainTableIncld2);
                            //}
                            //if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                            //{
                            //    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                            //    pdfMainTableIncld2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //    subtable.AddCell(pdfMainTableIncld2);
                            //}


                            //PdfPCell pdfMainTablebothSubHeader5 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //pdfMainTablebothSubHeader5.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //pdfMainTablebothSubHeader5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            //pdfMainTablebothSubHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader5);

                            //PdfPCell pdfMainTablebothSubHeader6 = new PdfPCell(new Phrase("Rept.      Ref.", TblFontBold));
                            //pdfMainTablebothSubHeader6.HorizontalAlignment = Element.ALIGN_CENTER;
                            //pdfMainTablebothSubHeader6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            //pdfMainTablebothSubHeader6.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            //pdfMainTablebothSubHeader6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            //pdfMainTablebothSubHeader6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            //subtable.AddCell(pdfMainTablebothSubHeader6);



                        }



                        RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() != string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());

                        bool boolOutComesDetails = true;


                        string intcount1, intcount2, intcout3, intcount4, intcount5;
                        string intcummulativecount1, intcummulativecount2, intcummulativecout3, intcummulativecount4, intcummulativecount5;
                        string strIndSwitch = string.Empty;
                        foreach (RCsb14GroupEntity tblEnt in RngtblCodelist)
                        {
                            strIndSwitch = tblEnt.IndSwitch.ToString();

                            if (rdoOutcomesselect.Checked)
                            {
                                boolOutComesDetails = false;

                                if (strIndSwitch == "Y")
                                {
                                    boolOutComesDetails = true;

                                }
                                else
                                {
                                    if (rdoperiodBoth.Checked)
                                    {
                                        RCsb14GroupEntity rngcountdata = RngBothResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                        if (rngcountdata != null)
                                        {
                                            boolOutComesDetails = true;
                                        }
                                    }
                                    else
                                    {
                                        RCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                        if (rngcountdata != null)
                                        {
                                            boolOutComesDetails = true;
                                        }
                                    }
                                }

                            }

                            if (boolOutComesDetails)
                            {

                                if (Rb_User_Def.Checked)
                                {
                                    boolGoalAssoFilter = false;
                                    if (strIndSwitch == "Y")
                                    {
                                        boolGoalAssoFilter = true;
                                    }
                                    else
                                    {
                                        if (GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim()).Count > 0)
                                            boolGoalAssoFilter = true;
                                    }
                                }
                                if (boolGoalAssoFilter)
                                {



                                    intcount1 = intcount2 = intcout3 = "0";
                                    intcount4 = intcount5 = string.Empty;
                                    RCsb14GroupEntity rngcountdata = null;
                                    if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                    {
                                        if (Rb_SNP_Mem.Checked || rbo_ProgramWise1.Checked)
                                        {
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                        }
                                        else
                                        {
                                            if (rdoperiodCumulative.Checked)
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                            else
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                        }
                                        if (rngcountdata == null)
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                    }
                                    else
                                    {
                                        if (Rb_SNP_Mem.Checked || rbo_ProgramWise1.Checked)
                                        {
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                        }
                                        else
                                        {
                                            if (rdoperiodCumulative.Checked)
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                            else
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                        }
                                    }
                                    if (rngcountdata != null)
                                    {
                                        PdfPCell pdfDetailsTable1 = new PdfPCell(new Phrase(tblEnt.TblCode.Trim() + "." + tblEnt.GrpDesc.ToString(), TblFontBold));
                                        pdfDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                        pdfDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                        pdfDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                        pdfDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        subtable.AddCell(pdfDetailsTable1);

                                        if (strIndSwitch == "Y" && strResultColumn=="0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                        {
                                            //DataTable dtappcount = Result_Table.Select("SORTUNDUP_APP <>''").CopyToDataTable();
                                            //// DataRow[] dr = dtappcount.Select("GROUP BY SortUnDup_Agy,SortUnDup_Dept,SortUnDup_Prog,SortUnDup_Year,SortUnDup_App,SortUnDup_Fam_ID,SortUnDup_Client_ID");

                                            //List<DataTable> dtAssess = dtappcount.AsEnumerable().GroupBy(row => new { SortUnDup_Agy = row.Field<string>("SortUnDup_Agy"), SortUnDup_Dept = row.Field<string>("SortUnDup_Dept"), SortUnDup_Prog = row.Field<string>("SortUnDup_Prog"), SortUnDup_Year = row.Field<string>("SortUnDup_Year"), SortUnDup_App = row.Field<string>("SortUnDup_App"), SortUnDup_Fam_ID = row.Field<string>("SortUnDup_Fam_ID"), SortUnDup_Client_ID = row.Field<decimal>("SortUnDup_Client_ID") }).Select(g => g.CopyToDataTable()).ToList();

                                            ////DataTable dtappcount1 =  dtappcount.AsEnumerable()
                                            ////    .GroupBy(r => new { SortUnDup_Agy = r["SortUnDup_Agy"], SortUnDup_Dept = r["SortUnDup_Dept"], SortUnDup_Prog = r["SortUnDup_Prog"], SortUnDup_Year = r["SortUnDup_Year"], SortUnDup_App = r["SortUnDup_App"], SortUnDup_Fam_ID = r["SortUnDup_Fam_ID"], SortUnDup_Client_ID = r["SortUnDup_Client_ID"] })
                                            ////    .Select(g =>
                                            ////    {
                                            ////        var row = dt.NewRow();

                                            ////        row["SortUnDup_Agy"] = g.Key.SortUnDup_Agy;
                                            ////        row["SortUnDup_Dept"] = g.Key.SortUnDup_Dept;
                                            ////        row["SortUnDup_Prog"] = g.Key.SortUnDup_Prog;
                                            ////        row["SortUnDup_Year"] = g.Key.SortUnDup_Year;
                                            ////        row["SortUnDup_App"] = g.Key.SortUnDup_App;
                                            ////        row["SortUnDup_Fam_ID"] = g.Key.SortUnDup_Fam_ID;
                                            ////        row["SortUnDup_Client_ID"] = g.Key.SortUnDup_Client_ID;

                                            ////        return row;

                                            ////    })
                                            ////    .CopyToDataTable();

                                            ////DataRow[] dr= dtappcount.AsEnumerable().GroupBy(r => new { Col1 = r["SortUnDup_Agy"], Col2 = r["SortUnDup_Dept"], Col3 = r["SortUnDup_Prog"], Col4 = r["SortUnDup_Year"], Col5 = r["SortUnDup_App"], Col6 = r["SortUnDup_Fam_ID"], Col7 = r["SortUnDup_Client_ID"] });
                                            ////DataView dvResult = ;
                                            ////dvResult.fi
                                            //if (rdoperiodBoth.Checked)
                                            //{
                                            //    intcount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                            //}
                                            //else
                                            intcount1 = IndSwitch_Table.Rows.Count.ToString();

                                            intcount2 = rngcountdata.ExAchev.ToString();
                                            intcout3 = "0";

                                        }
                                        else
                                        {
                                            if (strIndSwitch == "Y")
                                                intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                            else
                                                intcount1 = rngcountdata.unit_cnt.ToString();
                                            intcount2 = rngcountdata.ExAchev.ToString();
                                            switch (strResultColumn)
                                            {
                                                case "0":
                                                     intcout3 = rngcountdata.unit_cnt.ToString();
                                                    break;
                                                case "1":
                                                    intcout3 = rngcountdata.Hrd1.ToString();
                                                    break;
                                                case "2":
                                                    intcout3 = rngcountdata.Hrd2.ToString();
                                                    break;
                                                case "3":
                                                    intcout3 = rngcountdata.Hrd3.ToString();
                                                    break;
                                                case "4":
                                                    intcout3 = rngcountdata.Hrd4.ToString();
                                                    break;
                                                case "5":
                                                    intcout3 = rngcountdata.Hrd5.ToString();
                                                    break;
                                                default:
                                                    intcout3 = rngcountdata.Hrd1.ToString();
                                                    break;
                                            }
                                            if (strIndSwitch == "Y")
                                                intcout3 = IndSwitch_Table.Rows.Count.ToString();

                                        }
                                        if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                        {
                                            if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                            {
                                                intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                if (intcount4.Length > 4)
                                                    intcount4 = intcount4.Substring(0, 4);
                                            }
                                            if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                            {
                                                intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                if (intcount5.Length > 4)
                                                    intcount5 = intcount5.Substring(0, 4);
                                            }

                                        }


                                        if (rdoperiodBoth.Checked)
                                        {
                                            //PdfPCell pdfDetailstotalTable1 = new PdfPCell(new Phrase("", TableFont));
                                            //pdfDetailstotalTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //pdfDetailstotalTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                            //pdfDetailstotalTable1.Border = iTextSharp.text.Rectangle.BOX;
                                            //subtable.AddCell(pdfDetailstotalTable1);


                                            intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                            intcummulativecount4 = intcummulativecount5 = string.Empty;
                                            RCsb14GroupEntity rngcummulativecounttotaldata = null;
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                if (Rb_SNP_Mem.Checked || rbo_ProgramWise1.Checked)//if (!rdbSumDetail.Checked)
                                                    rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                                else
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                    {
                                                        rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                                    }
                                                    else
                                                        rngcummulativecounttotaldata = rngcountdata;
                                                }
                                                if (rngcummulativecounttotaldata == null)
                                                    rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            }
                                            else
                                            {
                                                if (Rb_SNP_Mem.Checked || rbo_ProgramWise1.Checked)//if (!rdbSumDetail.Checked)
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                    {
                                                        rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                                    }
                                                    else
                                                        rngcummulativecounttotaldata = rngcountdata;

                                                    // rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                                }
                                                else
                                                {
                                                    rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");

                                                    //if (rdoperiodBoth.Checked)
                                                    //{
                                                    //    rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                                    //}
                                                    //else
                                                    //    rngcummulativecounttotaldata = rngcountdata;
                                                }
                                            }
                                            if (rngcummulativecounttotaldata != null)
                                            {
                                                if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                        intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    else
                                                        intcummulativecount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = rngcummulativecounttotaldata.ExAchev.ToString();
                                                    intcummulativecout3 = "0";
                                                }
                                                else
                                                {
                                                    if(strIndSwitch == "Y")
                                                        intcummulativecount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    else
                                                        intcummulativecount1 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                                    intcummulativecount2 = rngcummulativecounttotaldata.ExAchev.ToString();
                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                                intcummulativecout3 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                                            break;
                                                    }
                                                    if (strIndSwitch == "Y")
                                                        intcummulativecout3 = IndSwitch_Table.Rows.Count.ToString();
                                                }
                                                if (intcummulativecount2 == string.Empty)
                                                    intcummulativecount2 = intcount2;

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }

                                                PdfPTable NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths1 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths1);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell3 = new PdfPCell(new Phrase(intcount1, TblFontBold));
                                                Cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell3);

                                                PdfPCell Cell33 = new PdfPCell(new Phrase(intcummulativecount1, TblFontBold));
                                                Cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell33.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell33);

                                                PdfPCell pdfDetailstotalTable3 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable3);
                                                //NestedTable2.DeleteBodyRows();

                                                PdfPCell pdfDetailstotalTable2 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                                pdfDetailstotalTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable2);

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths2 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths2);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell4 = new PdfPCell(new Phrase(intcout3, TblFontBold));
                                                Cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell4);

                                                PdfPCell Cell44 = new PdfPCell(new Phrase(intcummulativecout3, TblFontBold));
                                                Cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell44.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell44);

                                                PdfPCell pdfDetailstotalTable4 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable4);
                                                //NestedTable2.DeleteBodyRows();

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths7 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths7);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd1, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd1, TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths8 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths8);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //**NestedTable2.DeleteBodyRows();

                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd2, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd2, TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //**NestedTable2.DeleteBodyRows();

                                                    }
                                                }

                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths6 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths6);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd3, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd3, TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                }

                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths9 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths9);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd4, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd4, TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                }

                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths5 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths5);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd5, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd5, TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                        //NestedTable2.DeleteBodyRows();

                                                    }
                                                }

                                                //PdfPCell pdfDetailstotalTable2 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                                //pdfDetailstotalTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable2);

                                                //PdfPCell pdfDetailstotalTable3 = new PdfPCell(new Phrase(intcount1 + "     " + intcummulativecount1, TblFontBold));
                                                //pdfDetailstotalTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable3);

                                                //PdfPCell pdfDetailstotalTable4 = new PdfPCell(new Phrase(intcout3 + "     " + intcummulativecout3, TblFontBold));
                                                //pdfDetailstotalTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable4);

                                                //if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                //{
                                                //    if (strIndSwitch == "Y")
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //    else
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd1 + "     " + rngcummulativecounttotaldata.Hrd1, TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //}

                                                //if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                //{
                                                //    if (strIndSwitch == "Y")
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //    else
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd2 + "     " + rngcummulativecounttotaldata.Hrd2, TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //}
                                                //if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                //{
                                                //    if (strIndSwitch == "Y")
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //    else
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd3 + "     " + rngcummulativecounttotaldata.Hrd3, TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //}
                                                //if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                //{
                                                //    if (strIndSwitch == "Y")
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //    else
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd4 + "     " + rngcummulativecounttotaldata.Hrd4, TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //}
                                                //if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                //{
                                                //    if (strIndSwitch == "Y")
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //    else
                                                //    {
                                                //        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd5 + "     " + rngcummulativecounttotaldata.Hrd5, TblFontBold));
                                                //        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                //        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //        subtable.AddCell(pdfMainTableIncld2);
                                                //    }
                                                //}

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                //string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcount5 = intcount5.ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths3 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths3);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell6 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%"), TblFontBold));
                                                Cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell6);

                                                PdfPCell Cell66 = new PdfPCell(new Phrase((strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TblFontBold));
                                                Cell66.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell66.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell66);

                                                PdfPCell pdfDetailstotalTable5 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable5);
                                                //NestedTable2.DeleteBodyRows();

                                                //PdfPCell pdfDetailstotalTable5 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%") + "       " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TblFontBold));
                                                //pdfDetailstotalTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfDetailstotalTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable5);
                                                // string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcummulativecount5 = intcummulativecount5.ToString();
                                                //if (strcummulativecountArray5.Length > 1)
                                                //{
                                                //    if (strcummulativecountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcummulativecount5 = strcummulativecountArray5[0];
                                                //    }
                                                //}

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths4 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths4);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell7 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%"), TblFontBold));
                                                Cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell7);

                                                PdfPCell Cell77 = new PdfPCell(new Phrase((strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TblFontBold));
                                                Cell77.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell77.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell77);

                                                PdfPCell pdfDetailstotalTable6 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable6);
                                                //NestedTable2.DeleteBodyRows();

                                                //PdfPCell pdfDetailstotalTable6 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%") + "       " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TblFontBold));
                                                //pdfDetailstotalTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfDetailstotalTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable6);
                                            }

                                            else
                                            {

                                                PdfPTable NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths1 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths1);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell3 = new PdfPCell(new Phrase(intcount1, TblFontBold));
                                                Cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell3);

                                                PdfPCell Cell33 = new PdfPCell(new Phrase(intcummulativecount1, TblFontBold));
                                                Cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell33.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell33);

                                                PdfPCell pdfDetailstotalTable3 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable3);

                                                PdfPCell pdfDetailstotalTable2 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                                pdfDetailstotalTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable2);

                                                //PdfPCell pdfDetailstotalTable3 = new PdfPCell(new Phrase(intcount1 + "     " + intcummulativecount1, TblFontBold));
                                                //pdfDetailstotalTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable3);

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths2 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths2);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell4 = new PdfPCell(new Phrase(intcout3, TblFontBold));
                                                Cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell4);

                                                PdfPCell Cell44 = new PdfPCell(new Phrase(intcummulativecout3, TblFontBold));
                                                Cell44.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell44.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell44);

                                                PdfPCell pdfDetailstotalTable4 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable4);

                                                //PdfPCell pdfDetailstotalTable4 = new PdfPCell(new Phrase( intcout3 + "     " + intcummulativecout3, TblFontBold));
                                                //pdfDetailstotalTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable4);

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths7 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths7);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {

                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd1, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd1 + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths8 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths8);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {

                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd2, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd2 + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths6 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths6);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd3, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd3 + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths9 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths9);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd4, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd4 + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    NestedTable2 = new PdfPTable(2);
                                                    NestedTable2.TotalWidth = 60f;
                                                    NestedTable2.LockedWidth = true;
                                                    float[] widths5 = new float[] { 30f, 30f };
                                                    NestedTable2.SetWidths(widths5);
                                                    NestedTable2.WidthPercentage = 100;
                                                    NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell5 = new PdfPCell(new Phrase(rngcountdata.Hrd5, TblFontBold));
                                                        Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell5);

                                                        PdfPCell Cell55 = new PdfPCell(new Phrase("0", TblFontBold));
                                                        Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable2.AddCell(Cell55);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable2);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd5 + "     " + "0", TblFontBold));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                //string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcount5 = intcount5.ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths3 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths3);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell6 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%"), TblFontBold));
                                                Cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell6);

                                                PdfPCell Cell66 = new PdfPCell(new Phrase((strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TblFontBold));
                                                Cell66.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell66.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell66);

                                                PdfPCell pdfDetailstotalTable5 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable5);

                                                //PdfPCell pdfDetailstotalTable5 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%") + "       " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TblFontBold));
                                                //pdfDetailstotalTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfDetailstotalTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable5);


                                                // string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcummulativecount5 = intcummulativecount5.ToString();
                                                //if (strcummulativecountArray5.Length > 1)
                                                //{
                                                //    if (strcummulativecountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcummulativecount5 = strcummulativecountArray5[0];
                                                //    }
                                                //}

                                                NestedTable2 = new PdfPTable(2);
                                                NestedTable2.TotalWidth = 60f;
                                                NestedTable2.LockedWidth = true;
                                                float[] widths4 = new float[] { 30f, 30f };
                                                NestedTable2.SetWidths(widths4);
                                                NestedTable2.WidthPercentage = 100;
                                                NestedTable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell7 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%"), TblFontBold));
                                                Cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell7);

                                                PdfPCell Cell77 = new PdfPCell(new Phrase((strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TblFontBold));
                                                Cell77.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell77.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable2.AddCell(Cell77);

                                                PdfPCell pdfDetailstotalTable6 = new PdfPCell(NestedTable2);
                                                pdfDetailstotalTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfDetailstotalTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfDetailstotalTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfDetailstotalTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                subtable.AddCell(pdfDetailstotalTable6);

                                                //PdfPCell pdfDetailstotalTable6 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%") + "       " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TblFontBold));
                                                //pdfDetailstotalTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfDetailstotalTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfDetailstotalTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfDetailstotalTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfDetailstotalTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //subtable.AddCell(pdfDetailstotalTable6);
                                            }
                                        }
                                        else
                                        {


                                            PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intcount1, TblFontBold));
                                            //PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                            pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            pdfDetailsTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            subtable.AddCell(pdfDetailsTable2);

                                            PdfPCell pdfDetailsTable3 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                            //PdfPCell pdfDetailsTable3 = new PdfPCell(new Phrase(intcount1, TblFontBold));
                                            pdfDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            pdfDetailsTable3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            subtable.AddCell(pdfDetailsTable3);

                                            PdfPCell pdfDetailsTable4 = new PdfPCell(new Phrase(intcout3, TblFontBold));
                                            pdfDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            pdfDetailsTable4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            subtable.AddCell(pdfDetailsTable4);

                                            if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd1, TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                            }

                                            if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd2, TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                            }
                                            if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd3, TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                            }
                                            if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd4, TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                            }
                                            if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcountdata.Hrd5, TblFontBold));
                                                    pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    pdfMainTableIncld2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    subtable.AddCell(pdfMainTableIncld2);
                                                }
                                            }

                                            string[] strcountArray4 = intcount4.Split('.');
                                            string strcount4 = intcount4.ToString();
                                            if (strcountArray4.Length > 1)
                                                strcount4 = strcountArray4[0];

                                            PdfPCell pdfDetailsTable5 = new PdfPCell(new Phrase(strcount4 == string.Empty ? "" : strcount4 + "%", TblFontBold));
                                            pdfDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                            pdfDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            pdfDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            subtable.AddCell(pdfDetailsTable5);


                                            // string[] strcountArray5 = intcount5.Split('.');
                                            string strcount5 = string.Empty;
                                            if (intcount5 != string.Empty)
                                                strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                            //if (strcountArray5.Length > 1)
                                            //{
                                            //    if (strcountArray5[1].StartsWith("0"))
                                            //    {
                                            //        strcount5 = strcountArray5[0];
                                            //    }
                                            //}

                                            PdfPCell pdfDetailsTable6 = new PdfPCell(new Phrase(strcount5 == string.Empty ? "" : strcount5 + "%", TblFontBold));
                                            pdfDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                            pdfDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            pdfDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            subtable.AddCell(pdfDetailsTable6);
                                        }
                                    }
                                    if (Rb_SNP_Mem.Checked)  //&& (rdbSumDetail.Checked || rbo_ProgramWise1.Checked)
                                    {
                                        if (!rdoperiodBoth.Checked)
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;
                                                if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                                {
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcount2 = item.ExAchev.ToString();
                                                    intcout3 = "0";
                                                }
                                                else
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                        intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    else
                                                    {
                                                        if(strIndSwitch=="Y")
                                                            intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                        else
                                                            intcount1 = item.unit_cnt.ToString();
                                                    }
                                                    intcount2 = item.ExAchev.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                                intcout3 = item.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                    }
                                                    if (strIndSwitch == "Y")
                                                        intcout3 = IndSwitch_Table.Rows.Count.ToString();
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }



                                                PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("     " + item.GrpDesc.ToString(), TableFontNew));
                                                pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfSubDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable1);

                                                //PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                //pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable3);

                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(intcount1.ToString(), TableFontNew));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(item.unit_cnt.ToString(), TableFontNew));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);
                                                }

                                                PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable3);

                                                PdfPCell pdfSubDetailsTable4 = new PdfPCell(new Phrase(intcout3, TableFontNew));
                                                pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable4);

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd1, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd2, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd3, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd4, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd5, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                PdfPCell pdfSubDetailsTable5 = new PdfPCell(new Phrase(strcount4 == string.Empty ? "" : strcount4 + "%", TableFontNew));
                                                pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable5);

                                                // string[] strcountArray5 = intcount5.Split('.');
                                                // string strcount5 = intcount5.ToString();
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                PdfPCell pdfSubDetailsTable6 = new PdfPCell(new Phrase(strcount5 == string.Empty ? "" : strcount5 + "%", TableFontNew));
                                                pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable6);
                                            }
                                        }
                                        else
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            List<RCsb14GroupEntity> rngsubBothdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");

                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");

                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                if (rngsubBothdetailsdata.Count == 0)
                                                    rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH" && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            int intbothcount = 0;

                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;

                                                intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                                intcummulativecount4 = intcummulativecount5 = string.Empty;

                                                if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                                {
                                                    intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();
                                                    intcout3 = intcummulativecout3 = "0";
                                                }
                                                else
                                                {
                                                    if (strIndSwitch == "Y")
                                                        intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    else
                                                        intcount1 = item.unit_cnt.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();

                                                    intcummulativecount1 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                           intcout3 = item.unit_cnt.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                    }
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        intcout3 = IndSwitch_Table.Rows.Count.ToString();
                                                        intcummulativecout3 = IndSwitch_Table.Rows.Count.ToString();
                                                    }
                                                        
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }

                                                PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("     " + item.GrpDesc.ToString(), TableFontNew));
                                                pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfSubDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable1);

                                                //PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                //pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable3);

                                                PdfPTable NestedTable3 = new PdfPTable(2);
                                                NestedTable3.TotalWidth = 60f;
                                                NestedTable3.LockedWidth = true;
                                                float[] widths1 = new float[] { 30f, 30f };
                                                NestedTable3.SetWidths(widths1);
                                                NestedTable3.WidthPercentage = 100;
                                                NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                if (strIndSwitch == "Y")
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] widt = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(widt);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    PdfPCell Cell50 = new PdfPCell(new Phrase(intcount1.ToString(), TableFontNew));
                                                    Cell50.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell50.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable3.AddCell(Cell50);

                                                    PdfPCell Cell550 = new PdfPCell(new Phrase(intcummulativecount1, TableFontNew));
                                                    Cell550.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell550.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable3.AddCell(Cell550);

                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(NestedTable3);
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);

                                                    //PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(intcount1.ToString() + "     " + intcummulativecount1, TableFontNew));
                                                    //pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    //pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    //subtable.AddCell(pdfSubDetailsTable2);
                                                }
                                                else
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] width7 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(width7);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    PdfPCell Cell57 = new PdfPCell(new Phrase(item.unit_cnt.ToString(), TableFontNew));
                                                    Cell57.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell57.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable3.AddCell(Cell57);

                                                    PdfPCell Cell557 = new PdfPCell(new Phrase(intcummulativecount1, TableFontNew));
                                                    Cell557.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell557.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable3.AddCell(Cell557);

                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(NestedTable3);
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);

                                                    //PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(item.unit_cnt.ToString() + "     " + intcummulativecount1, TableFontNew));
                                                    //pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    //pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    //subtable.AddCell(pdfSubDetailsTable2);

                                                }

                                                PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable3);

                                                NestedTable3 = new PdfPTable(2);
                                                NestedTable3.TotalWidth = 60f;
                                                NestedTable3.LockedWidth = true;
                                                float[] widths7 = new float[] { 30f, 30f };
                                                NestedTable3.SetWidths(widths7);
                                                NestedTable3.WidthPercentage = 100;
                                                NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell5 = new PdfPCell(new Phrase(intcout3, TableFontNew));
                                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell5);

                                                PdfPCell Cell55 = new PdfPCell(new Phrase(intcummulativecout3, TableFontNew));
                                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell55);

                                                PdfPCell pdfSubDetailsTable4 = new PdfPCell(NestedTable3);
                                                pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable4);

                                                //PdfPCell pdfSubDetailsTable4 = new PdfPCell(new Phrase(intcout3 + "     " + intcummulativecout3, TableFontNew));
                                                //pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable4);

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] withs10 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(withs10);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd1, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd1, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd1 + "     " + rngsubBothdetailsdata[intbothcount].Hrd1, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] widths110 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(widths110);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd2, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd2, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd2 + "     " + rngsubBothdetailsdata[intbothcount].Hrd2, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] widths101 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(widths101);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd3, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd3, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd3 + "     " + rngsubBothdetailsdata[intbothcount].Hrd3, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] widths109 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(widths109);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd4, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd4, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd4 + "     " + rngsubBothdetailsdata[intbothcount].Hrd4, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    NestedTable3 = new PdfPTable(2);
                                                    NestedTable3.TotalWidth = 60f;
                                                    NestedTable3.LockedWidth = true;
                                                    float[] widths105 = new float[] { 30f, 30f };
                                                    NestedTable3.SetWidths(widths105);
                                                    NestedTable3.WidthPercentage = 100;
                                                    NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd5, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd5, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable3.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable3);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd5 + "     " + rngsubBothdetailsdata[intbothcount].Hrd5, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                NestedTable3 = new PdfPTable(2);
                                                NestedTable3.TotalWidth = 60f;
                                                NestedTable3.LockedWidth = true;
                                                float[] widths10 = new float[] { 30f, 30f };
                                                NestedTable3.SetWidths(widths10);
                                                NestedTable3.WidthPercentage = 100;
                                                NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell6 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%"), TableFontNew));
                                                Cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell6);

                                                PdfPCell Cell66 = new PdfPCell(new Phrase((strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TableFontNew));
                                                Cell66.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell66.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell66);

                                                PdfPCell pdfSubDetailsTable5 = new PdfPCell(NestedTable3);
                                                pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable5);

                                                //PdfPCell pdfSubDetailsTable5 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%") + "       " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TableFontNew));
                                                //pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                //subtable.AddCell(pdfSubDetailsTable5);

                                                string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcount5 = intcount5.ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                //string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcummulativecount5 = intcummulativecount5.ToString();
                                                //if (strcummulativecountArray5.Length > 1)
                                                //{
                                                //    if (strcummulativecountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcummulativecount5 = strcummulativecountArray5[0];
                                                //    }
                                                //}

                                                NestedTable3 = new PdfPTable(2);
                                                NestedTable3.TotalWidth = 60f;
                                                NestedTable3.LockedWidth = true;
                                                float[] widths11 = new float[] { 30f, 30f };
                                                NestedTable3.SetWidths(widths11);
                                                NestedTable3.WidthPercentage = 100;
                                                NestedTable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell7 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%"), TableFontNew));
                                                Cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell7);

                                                PdfPCell Cell77 = new PdfPCell(new Phrase((strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TableFontNew));
                                                Cell77.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell77.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable3.AddCell(Cell77);

                                                PdfPCell pdfSubDetailsTable6 = new PdfPCell(NestedTable3);
                                                pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable6);
                                                intbothcount = intbothcount + 1;

                                                //PdfPCell pdfSubDetailsTable6 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%") + "       " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TableFontNew));
                                                //pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                //subtable.AddCell(pdfSubDetailsTable6);
                                                //intbothcount = intbothcount + 1;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        // if (rdoperiodBoth.Checked)
                                        //{
                                        //    PdfPCell pdfDetailstotalTable1 = new PdfPCell(new Phrase("", TableFont));
                                        //    pdfDetailstotalTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //    pdfDetailstotalTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                        //    pdfDetailstotalTable1.Border = iTextSharp.text.Rectangle.BOX;
                                        //    subtable.AddCell(pdfDetailstotalTable1);

                                        //    string intcummulativecount1, intcummulativecount2, intcummulativecout3, intcummulativecount4, intcummulativecount5;
                                        //    intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                        //    intcummulativecount4 = intcummulativecount5 = string.Empty;
                                        //    RCsb14GroupEntity rngcummulativecounttotaldata = null;

                                        //    rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                        //    if (rngcummulativecounttotaldata != null)
                                        //    {
                                        //        intcummulativecount1 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                        //        intcummulativecount2 = rngcummulativecounttotaldata.ExAchev.ToString();
                                        //        switch (strResultColumn)
                                        //        {
                                        //            case "0":
                                        //                intcout3 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                        //                break;
                                        //            case "1":
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                        //                break;
                                        //            case "2":
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd2.ToString();
                                        //                break;
                                        //            case "3":
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd3.ToString();
                                        //                break;
                                        //            case "4":
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd4.ToString();
                                        //                break;
                                        //            case "5":
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd5.ToString();
                                        //                break;
                                        //            default:
                                        //                intcout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                        //                break;
                                        //        }

                                        //        if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                        //        {
                                        //            if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                        //            {
                                        //                intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                        //                if (intcummulativecount4.Length > 4)
                                        //                    intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                        //            }
                                        //            if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                        //            {
                                        //                intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                        //                if (intcummulativecount5.Length > 4)
                                        //                    intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                        //            }

                                        //        }



                                        //        PdfPCell pdfDetailstotalTable2 = new PdfPCell(new Phrase(intcummulativecount1, TableFont));
                                        //        pdfDetailstotalTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //        pdfDetailstotalTable2.Border = iTextSharp.text.Rectangle.BOX;
                                        //        subtable.AddCell(pdfDetailstotalTable2);

                                        //        PdfPCell pdfDetailstotalTable3 = new PdfPCell(new Phrase(intcummulativecount2, TableFont));
                                        //        pdfDetailstotalTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //        pdfDetailstotalTable3.Border = iTextSharp.text.Rectangle.BOX;
                                        //        subtable.AddCell(pdfDetailstotalTable3);

                                        //        PdfPCell pdfDetailstotalTable4 = new PdfPCell(new Phrase(intcummulativecout3, TableFont));
                                        //        pdfDetailstotalTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //        pdfDetailstotalTable4.Border = iTextSharp.text.Rectangle.BOX;
                                        //        subtable.AddCell(pdfDetailstotalTable4);

                                        //        if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                        //        {
                                        //            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd1, TableFont));
                                        //            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                        //            subtable.AddCell(pdfMainTableIncld2);
                                        //        }

                                        //        if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                        //        {
                                        //            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd2, TableFont));
                                        //            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                        //            subtable.AddCell(pdfMainTableIncld2);
                                        //        }
                                        //        if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                        //        {
                                        //            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd3, TableFont));
                                        //            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                        //            subtable.AddCell(pdfMainTableIncld2);
                                        //        }
                                        //        if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                        //        {
                                        //            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd4, TableFont));
                                        //            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                        //            subtable.AddCell(pdfMainTableIncld2);
                                        //        }
                                        //        if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                        //        {
                                        //            PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(rngcummulativecounttotaldata.Hrd5, TableFont));
                                        //            pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //            pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                        //            subtable.AddCell(pdfMainTableIncld2);
                                        //        }

                                        //        string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                        //        string strcummulativecount4 = intcummulativecount4.ToString();
                                        //        if (strcummulativecountArray4.Length > 1)
                                        //            strcummulativecount4 = strcummulativecountArray4[0];

                                        //        PdfPCell pdfDetailstotalTable5 = new PdfPCell(new Phrase(strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%", TableFont));
                                        //        pdfDetailstotalTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //        pdfDetailstotalTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                        //        pdfDetailstotalTable5.Border = iTextSharp.text.Rectangle.BOX;
                                        //        subtable.AddCell(pdfDetailstotalTable5);


                                        //        string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                        //        string strcummulativecount5 = intcummulativecount5.ToString();
                                        //        if (strcummulativecountArray5.Length > 1)
                                        //        {
                                        //            if (strcummulativecountArray5[1].StartsWith("0"))
                                        //            {
                                        //                strcummulativecount5 = strcummulativecountArray5[0];
                                        //            }
                                        //        }

                                        //        PdfPCell pdfDetailstotalTable6 = new PdfPCell(new Phrase(strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%", TableFont));
                                        //        pdfDetailstotalTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //        pdfDetailstotalTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                        //        pdfDetailstotalTable6.Border = iTextSharp.text.Rectangle.BOX;
                                        //        subtable.AddCell(pdfDetailstotalTable6);
                                        //    }
                                        //}
                                    }

                                    if (rbo_ProgramWise1.Checked)
                                    {
                                        if (!rdoperiodBoth.Checked)
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;
                                                if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                                {
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcount2 = item.ExAchev.ToString();
                                                    intcout3 = "0";
                                                }
                                                else
                                                {
                                                    if (strIndSwitch == "Y")
                                                        intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    else
                                                        intcount1 = item.unit_cnt.ToString();
                                                    intcount2 = item.ExAchev.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                    }
                                                    if (strIndSwitch == "Y")
                                                        intcout3 = IndSwitch_Table.Rows.Count.ToString();
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }



                                                PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("     " + item.GrpDesc.ToString(), TableFontNew));
                                                pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfSubDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable1);

                                                //PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                //pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable3);

                                                if (strIndSwitch == "Y")
                                                {
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(intcount1.ToString(), TableFontNew));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);
                                                }
                                                else
                                                {
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(item.unit_cnt.ToString(), TableFontNew));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);
                                                }
                                                PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable3);

                                                PdfPCell pdfSubDetailsTable4 = new PdfPCell(new Phrase(intcout3, TableFontNew));
                                                pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable4);

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd1, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd2, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd3, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd4, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd5, TableFontNew));
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                PdfPCell pdfSubDetailsTable5 = new PdfPCell(new Phrase(strcount4 == string.Empty ? "" : strcount4 + "%", TableFontNew));
                                                pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable5);

                                                // string[] strcountArray5 = intcount5.Split('.');
                                                // string strcount5 = intcount5.ToString();
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                PdfPCell pdfSubDetailsTable6 = new PdfPCell(new Phrase(strcount5 == string.Empty ? "" : strcount5 + "%", TableFontNew));
                                                pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable6);
                                            }
                                        }
                                        else
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            List<RCsb14GroupEntity> rngsubBothdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");

                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");

                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                if (rngsubBothdetailsdata.Count == 0)
                                                    rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH" && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            int intbothcount = 0;

                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;

                                                intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                                intcummulativecount4 = intcummulativecount5 = string.Empty;

                                                if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                                {
                                                    intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();
                                                    intcout3 = intcummulativecout3 = "0";
                                                }
                                                else
                                                {
                                                    if (strIndSwitch == "Y")
                                                        intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    else
                                                        intcount1 = item.unit_cnt.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();

                                                    intcummulativecount1 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                    }
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        intcout3 = IndSwitch_Table.Rows.Count.ToString();
                                                        intcummulativecout3 = IndSwitch_Table.Rows.Count.ToString();
                                                    }
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }

                                                PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("     " + item.GrpDesc.ToString(), TableFontNew));
                                                pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfSubDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable1);

                                                //PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                //pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable3);

                                                PdfPTable NestedTable4 = new PdfPTable(2);
                                                NestedTable4.TotalWidth = 60f;
                                                NestedTable4.LockedWidth = true;
                                                float[] widths1 = new float[] { 30f, 30f };
                                                NestedTable4.SetWidths(widths1);
                                                NestedTable4.WidthPercentage = 100;
                                                NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                if (strIndSwitch == "Y")
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths07 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths07);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    PdfPCell Cell50 = new PdfPCell(new Phrase(intcount1.ToString(), TableFontNew));
                                                    Cell50.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell50.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable4.AddCell(Cell50);

                                                    PdfPCell Cell550 = new PdfPCell(new Phrase(intcummulativecount1, TableFontNew));
                                                    Cell550.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell550.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable4.AddCell(Cell550);

                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(NestedTable4);
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);

                                                    //PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(intcount1.ToString() + "     " + intcummulativecount1, TableFontNew));
                                                    //pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    //pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    //subtable.AddCell(pdfSubDetailsTable2);
                                                }
                                                else
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths709 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths709);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    PdfPCell Cell57 = new PdfPCell(new Phrase(item.unit_cnt.ToString(), TableFontNew));
                                                    Cell57.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell57.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable4.AddCell(Cell57);

                                                    PdfPCell Cell557 = new PdfPCell(new Phrase(intcummulativecount1, TableFontNew));
                                                    Cell557.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    Cell557.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    NestedTable4.AddCell(Cell557);


                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(NestedTable4);
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    subtable.AddCell(pdfSubDetailsTable2);

                                                    //PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase(item.unit_cnt.ToString() + "     " + intcummulativecount1, TableFontNew));
                                                    //pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_CENTER;
                                                    //pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    //pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    //subtable.AddCell(pdfSubDetailsTable2);

                                                }

                                                PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase(intcount2.ToString(), TableFontNew));
                                                pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable3);

                                                NestedTable4 = new PdfPTable(2);
                                                NestedTable4.TotalWidth = 60f;
                                                NestedTable4.LockedWidth = true;
                                                float[] widths7 = new float[] { 30f, 30f };
                                                NestedTable4.SetWidths(widths7);
                                                NestedTable4.WidthPercentage = 100;
                                                NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell5 = new PdfPCell(new Phrase(intcout3, TableFontNew));
                                                Cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell5);

                                                PdfPCell Cell55 = new PdfPCell(new Phrase(intcummulativecout3, TableFontNew));
                                                Cell55.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell55.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell55);

                                                PdfPCell pdfSubDetailsTable4 = new PdfPCell(NestedTable4);
                                                pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                subtable.AddCell(pdfSubDetailsTable4);

                                                //PdfPCell pdfSubDetailsTable4 = new PdfPCell(new Phrase(intcout3 + "     " + intcummulativecout3, TableFontNew));
                                                //pdfSubDetailsTable4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable4.VerticalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable4.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //subtable.AddCell(pdfSubDetailsTable4);

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths708 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths708);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd1, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd1, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd1 + "     " + rngsubBothdetailsdata[intbothcount].Hrd1, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths708 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths708);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);


                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd2, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd2, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd2 + "     " + rngsubBothdetailsdata[intbothcount].Hrd2, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths708 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths708);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd3, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd3, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd3 + "     " + rngsubBothdetailsdata[intbothcount].Hrd3, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths708 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths708);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd4, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd4, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd4 + "     " + rngsubBothdetailsdata[intbothcount].Hrd4, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    NestedTable4 = new PdfPTable(2);
                                                    NestedTable4.TotalWidth = 60f;
                                                    NestedTable4.LockedWidth = true;
                                                    float[] widths708 = new float[] { 30f, 30f };
                                                    NestedTable4.SetWidths(widths708);
                                                    NestedTable4.WidthPercentage = 100;
                                                    NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                    if (strIndSwitch == "Y")
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase("0", TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase("0" + "     " + "0", TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell56 = new PdfPCell(new Phrase(item.Hrd5, TableFontNew));
                                                        Cell56.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell56.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell56);

                                                        PdfPCell Cell556 = new PdfPCell(new Phrase(rngsubBothdetailsdata[intbothcount].Hrd5, TableFontNew));
                                                        Cell556.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        Cell556.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        NestedTable4.AddCell(Cell556);

                                                        PdfPCell pdfMainTableIncld2 = new PdfPCell(NestedTable4);
                                                        pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        subtable.AddCell(pdfMainTableIncld2);

                                                        //PdfPCell pdfMainTableIncld2 = new PdfPCell(new Phrase(item.Hrd5 + "     " + rngsubBothdetailsdata[intbothcount].Hrd5, TableFontNew));
                                                        //pdfMainTableIncld2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.VerticalAlignment = Element.ALIGN_CENTER;
                                                        //pdfMainTableIncld2.Border = iTextSharp.text.Rectangle.BOX;
                                                        //pdfMainTableIncld2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                        //subtable.AddCell(pdfMainTableIncld2);
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                NestedTable4 = new PdfPTable(2);
                                                NestedTable4.TotalWidth = 60f;
                                                NestedTable4.LockedWidth = true;
                                                float[] widths78 = new float[] { 30f, 30f };
                                                NestedTable4.SetWidths(widths78);
                                                NestedTable4.WidthPercentage = 100;
                                                NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell561 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%"), TableFontNew));
                                                Cell561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell561.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell561);

                                                PdfPCell Cell5561 = new PdfPCell(new Phrase((strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TableFontNew));
                                                Cell5561.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell5561.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell5561);

                                                PdfPCell pdfSubDetailsTable5 = new PdfPCell(NestedTable4);
                                                pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable5);

                                                //PdfPCell pdfSubDetailsTable5 = new PdfPCell(new Phrase((strcount4 == string.Empty ? "" : strcount4 + "%") + "       " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), TableFontNew));
                                                //pdfSubDetailsTable5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable5.VerticalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable5.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfSubDetailsTable5.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfSubDetailsTable5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                //subtable.AddCell(pdfSubDetailsTable5);

                                                string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcount5 = intcount5.ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                //string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcummulativecount5 = intcummulativecount5.ToString();
                                                //if (strcummulativecountArray5.Length > 1)
                                                //{
                                                //    if (strcummulativecountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcummulativecount5 = strcummulativecountArray5[0];
                                                //    }
                                                //}

                                                NestedTable4 = new PdfPTable(2);
                                                NestedTable4.TotalWidth = 60f;
                                                NestedTable4.LockedWidth = true;
                                                float[] widths787 = new float[] { 30f, 30f };
                                                NestedTable4.SetWidths(widths787);
                                                NestedTable4.WidthPercentage = 100;
                                                NestedTable4.HorizontalAlignment = Element.ALIGN_CENTER;

                                                PdfPCell Cell5610 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%"), TableFontNew));
                                                Cell5610.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell5610.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell5610);

                                                PdfPCell Cell551 = new PdfPCell(new Phrase((strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TableFontNew));
                                                Cell551.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Cell551.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                NestedTable4.AddCell(Cell551);

                                                PdfPCell pdfSubDetailsTable6 = new PdfPCell(NestedTable4);
                                                pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.VerticalAlignment = Element.ALIGN_CENTER;
                                                pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                subtable.AddCell(pdfSubDetailsTable6);
                                                intbothcount = intbothcount + 1;

                                                //PdfPCell pdfSubDetailsTable6 = new PdfPCell(new Phrase((strcount5 == string.Empty ? "" : strcount5 + "%") + "       " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), TableFontNew));
                                                //pdfSubDetailsTable6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable6.VerticalAlignment = Element.ALIGN_CENTER;
                                                //pdfSubDetailsTable6.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                //pdfSubDetailsTable6.Border = iTextSharp.text.Rectangle.BOX;
                                                //pdfSubDetailsTable6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                //pdfSubDetailsTable6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                                                //subtable.AddCell(pdfSubDetailsTable6);
                                                //intbothcount = intbothcount + 1;
                                            }

                                        }

                                    }

                                }
                            }
                        }



                        //PdfPCell pdfBottomRow1 = new PdfPCell(new Phrase("FRN #2 – CSBG Annual Report ", TableFont));
                        //pdfBottomRow1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //pdfBottomRow1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //pdfBottomRow1.Colspan = 6;
                        ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //table.AddCell(pdfBottomRow1);

                        //PdfPCell pdfBottomRow2 = new PdfPCell(new Phrase(DateTime.Now.Month.ToString() + DateTime.Now.Year + "       Module 4, Section A: Individual and Family NPIs - Employment", TableFont));
                        //pdfBottomRow2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //pdfBottomRow2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //pdfBottomRow2.Colspan = 6;
                        ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //table.AddCell(pdfBottomRow2);


                        //table.FooterRows = 2;


                        if (subtable.Rows.Count > 0)
                        {
                            PdfPCell R12 = new PdfPCell(subtable);
                            R12.Padding = 0f;
                            R12.Colspan = 6;
                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(R12);

                            if (table.Rows.Count > 0)
                            {
                                document.Add(table);
                                subtable.DeleteBodyRows();
                                table.DeleteBodyRows();
                                document.NewPage();
                            }
                        }

                    }

                }
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            //document.Add(table);                
            document.Close();
            fs.Close();
            fs.Dispose();

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{
           /*PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, null, Result_Table, Out_Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0014");
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();*/

            if (rdoperiodBoth.Checked)
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, dt, Result_Table, Out_Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0014", rdoperiodBoth.Checked, Rb_OBO_Mem.Checked, Search_Entity.Ms_DriveColumn_Sw, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString());
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, dt, Result_Table, Out_Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0014", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString(), string.Empty, Rb_OBO_Mem.Checked, Search_Entity.Ms_DriveColumn_Sw);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName,, Result_Table, null, "RNGB0014_Detail_RdlC.rdlc", "Result Table", ReportPath, string.Empty,Rb_Details_Yes.Checked,"RNGB0014");               
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}

        }

        private void On_SaveFormExcel_Closed(DataTable dtResult, DataTable dtResultBoth)
        {

            strReasonCodes = string.Empty;

            PdfName = "RNGB0014_ExcelMainReport";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xls";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";

            try
            {

                WorksheetRow excelrow;
                WorksheetCell cell;

                Workbook book = new Workbook();

                WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
                mainstyle.Font.FontName = "Tahoma";
                mainstyle.Font.Size = 10;
                mainstyle.Font.Bold = true;
                mainstyle.Font.Color = "#FFFFFF";
                mainstyle.Interior.Color = "#0070c0";
                mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;


                WorksheetStyle style1 = book.Styles.Add("Normal");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 10;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style1.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
                stylecenter.Font.FontName = "Tahoma";
                stylecenter.Font.Bold = true;
                stylecenter.Font.Size = 10;
                stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                stylecenter.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style3 = book.Styles.Add("NormalLeft");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 10;
                style3.Interior.Color = "#f2f2f2";
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style3.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style31 = book.Styles.Add("NormalLeft1");
                style31.Font.FontName = "Tahoma";
                style31.Font.Size = 8;
                style31.Interior.Color = "#f2f2f2";
                style31.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style31.Alignment.Vertical = StyleVerticalAlignment.Center;


                Worksheet sheet = book.Worksheets.Add("Data");
                //sheet.Names.Add(new WorksheetNamedRange("_FilterDatabase", "=Data!R1C1:R"+(MatRepList.Count+1).ToString()+"C11", true));
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(300);
                sheet.Table.Columns.Add(90);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);

                List<CommonEntity> commonHeaderlist = new List<CommonEntity>();



                // CaptainModel _model = new CaptainModel();

                excelrow = sheet.Table.Rows.Add();

                List<RCsb14GroupEntity> RngGrouplist = new List<RCsb14GroupEntity>();
                List<RCsb14GroupEntity> RngtblCodelist = new List<RCsb14GroupEntity>();
                List<RCsb14GroupEntity> RngResultlist = new List<RCsb14GroupEntity>();

                List<RCsb14GroupEntity> RngBothResultlist = new List<RCsb14GroupEntity>();

                if (rdoperiodBoth.Checked) //&& Rb_SNP_Mem.Checked)
                {
                    foreach (DataRow drResultBothitem in dtResultBoth.Rows)
                    {
                        RngBothResultlist.Add(new RCsb14GroupEntity(drResultBothitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                    }
                }

                foreach (DataRow drResultitem in dtResult.Rows)
                {
                    RngResultlist.Add(new RCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                }

                List<RNGGAEntity> GoalDetailsEntity = _model.SPAdminData.Browse_RNGGA(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), string.Empty, string.Empty, string.Empty);
                if (RngCodelist.Count > 0)
                {
                    RngGrouplist = RngCodelist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() == string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString() && u.Code == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                    RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() != string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString() && u.Code == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                }
                if (rdoOutcomesselect.Checked)
                {

                    List<RCsb14GroupEntity> rngindswithclist = RngtblCodelist.FindAll(u => u.IndSwitch == "Y");
                    foreach (RCsb14GroupEntity itemindswitch in rngindswithclist)
                    {
                        List<RCsb14GroupEntity> rngtblcodelist = RngResultlist.FindAll(u => u.TblCode == itemindswitch.TblCode);
                        if (rngtblcodelist.Count == 0)
                        {
                            List<RNGGAEntity> GoalDetailsIndswitchEntity = GoalDetailsEntity.FindAll(u => u.TblCode.Trim() == itemindswitch.TblCode.Trim() && u.GrpCode.Trim() == itemindswitch.GrpCode.Trim() && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString().Trim());
                            foreach (RNGGAEntity itemGoalind in GoalDetailsIndswitchEntity)
                            {
                                RngResultlist.Add(new RCsb14GroupEntity(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), itemindswitch.GrpCode, itemindswitch.TblCode, itemGoalind.Desc, "0", "0", "0", "0", "0", itemindswitch.unit_cnt, itemindswitch.ExAchev, itemindswitch.per_Achived, itemindswitch.CalCost, itemindswitch.Count_type, "INDSWITCH"));
                            }

                        }
                        if (rdoperiodBoth.Checked) //&& Rb_SNP_Mem.Checked)
                        {
                            List<RCsb14GroupEntity> rngtblbothcodelist = RngBothResultlist.FindAll(u => u.TblCode == itemindswitch.TblCode);
                            if (rngtblcodelist.Count == 0)
                            {
                                List<RNGGAEntity> GoalDetailsIndswitchEntity = GoalDetailsEntity.FindAll(u => u.TblCode.Trim() == itemindswitch.TblCode.Trim() && u.GrpCode.Trim() == itemindswitch.GrpCode.Trim() && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString().Trim());
                                foreach (RNGGAEntity itemGoalind in GoalDetailsIndswitchEntity)
                                {
                                    RngBothResultlist.Add(new RCsb14GroupEntity(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), itemindswitch.GrpCode, itemindswitch.TblCode, itemGoalind.Desc, "0", "0", "0", "0", "0", itemindswitch.unit_cnt, itemindswitch.ExAchev, itemindswitch.per_Achived, itemindswitch.CalCost, itemindswitch.Count_type, "INDSWITCH"));
                                }
                            }
                        }
                    }
                }



                bool boolGroupFilter = true;
                bool boolGoalAssoFilter = true;

                string Agy = string.Empty;
                if (Current_Hierarchy.Substring(0, 2).ToString() != "**")
                    Agy = Current_Hierarchy.Substring(0, 2);
                string Agency = string.Empty;
                DataSet dsAgency = new DataSet();
                if (!string.IsNullOrEmpty(Agy.Trim()))
                {
                    dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(Agy, null, null, null, null, null, null);
                    if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
                        Agency = dsAgency.Tables[0].Rows[0]["ACR_NAME"].ToString().Trim();
                }

                foreach (RCsb14GroupEntity codeitem in RngGrouplist)
                {


                    if (Rb_User_Def.Checked)
                    {
                        boolGoalAssoFilter = false;
                        if (GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim()).Count > 0)
                            boolGoalAssoFilter = true;
                    }
                    if (Rb_Zip_Sel.Checked)
                    {
                        boolGroupFilter = false;
                        if (ListRngGroupCode.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim()).Count > 0)
                            boolGroupFilter = true;
                    }

                    if ((boolGroupFilter) && (boolGoalAssoFilter))
                    {
                        string strResultColumn = codeitem.ExAchev.ToString();


                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add(codeitem.GrpDesc , DataType.String, "MainHeaderStyles");
                        cell.MergeAcross = 11;
                        excelrow.Height = 25;


                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add("Department/Program: " + DeptName + ProgName, DataType.String, "NormalLeft1");
                        cell.MergeAcross = 11;

                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add("Name of CSBG Eligible Entity Reporting: " , DataType.String, "NormalLeft1");
                        //cell.MergeAcross = 11;

                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add(Agency , DataType.String, "NormalLeft1");
                        cell.MergeAcross = 10;

                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add("State: " , DataType.String, "NormalLeft1");
                        //cell.MergeAcross = 11;

                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add("DUNS: " , DataType.String, "NormalLeft1");
                        cell.MergeAcross = 10;

                        int intcolumns = 6;
                        int intheadercount = 0;
                        if (codeitem.Incld1.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld2.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld3.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld4.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (codeitem.Incld5.ToString().ToUpper() == "TRUE")
                            intheadercount = intheadercount + 1;
                        if (strResultColumn == "0")
                            intheadercount = intheadercount + 1;
                        if (intheadercount > 1)
                            intcolumns = intcolumns + (intheadercount - 1);

                        //PdfPTable subtable = new PdfPTable(intcolumns);



                        //float[] subwidths = null;
                        //if (intcolumns == 6)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f };
                        //if (intcolumns == 7)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f };
                        //if (intcolumns == 8)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        //if (intcolumns == 9)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        //if (intcolumns == 10)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };
                        //if (intcolumns == 11)
                        //    subwidths = new float[] { 250f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f };

                        //subtable.SetWidths(subwidths);

                        excelrow = sheet.Table.Rows.Add();     

                        cell = excelrow.Cells.Add("", DataType.String, "Normal");

                        cell = excelrow.Cells.Add("I.) Number of Individuals Served ", DataType.String, "Normal");

                        cell = excelrow.Cells.Add("II.) Target (#)", DataType.String, "Normal");

                        cell = excelrow.Cells.Add("III.) Actual Results (#) ", DataType.String, "Normal");

                        int intcolumnscount = 3;
                        if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;

                            cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + codeitem.Hrd1, DataType.String, "Normal");

                        }

                        if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + codeitem.Hrd2, DataType.String, "Normal");

                        }
                        if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + codeitem.Hrd3, DataType.String, "Normal");

                        }
                        if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + codeitem.Hrd4, DataType.String, "Normal");

                        }
                        if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                        {
                            intcolumnscount = intcolumnscount + 1;
                            cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + codeitem.Hrd5, DataType.String, "Normal");

                        }

                        intcolumnscount = intcolumnscount + 1;
                        cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + "Percentage Achieving Outcome ", DataType.String, "Normal");


                        intcolumnscount = intcolumnscount + 1;
                        cell = excelrow.Cells.Add(HeadersValue(intcolumnscount) + "Performance Target Accuracy ", DataType.String, "Normal");


                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add(codeitem.GrpDesc, DataType.String, "Normal");


                        cell = excelrow.Cells.Add("in program(s) (#)", DataType.String, "Normal");


                        cell = excelrow.Cells.Add("", DataType.String, "Normal");

                        cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        

                        if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                        {
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        }
                        if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                        {
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        }
                        if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                        {
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        }
                        if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                        {
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        }
                        if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                        {
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        }

                        cell = excelrow.Cells.Add("[III/ I = " + HeadersValue1(intcolumnscount - 1) + " ] (% auto calculated) ", DataType.String, "Normal");



                        cell = excelrow.Cells.Add("(III/II = " + HeadersValue1(intcolumnscount) + " ] (% auto calculated) ", DataType.String, "Normal");



                        if (rdoperiodBoth.Checked)
                        {

                            excelrow = sheet.Table.Rows.Add();
                            cell = excelrow.Cells.Add("", DataType.String, "Normal");

                            cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");

                            cell = excelrow.Cells.Add("", DataType.String, "Normal");

                            cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");

                            if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                            {
                                cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");
                            }
                            if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                            {
                                cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");
                            }
                            if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                            {
                                cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");
                            }
                            if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                            {
                                cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");
                            }
                            if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                            {
                                cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");
                            }


                            cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");

                            cell = excelrow.Cells.Add("Rept. | Ref.", DataType.String, "Normal");


                        }



                        RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() != string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());

                        bool boolOutComesDetails = true;


                        string intcount1, intcount2, intcout3, intcount4, intcount5;
                        string intcummulativecount1, intcummulativecount2, intcummulativecout3, intcummulativecount4, intcummulativecount5;
                        string strIndSwitch = string.Empty;
                        foreach (RCsb14GroupEntity tblEnt in RngtblCodelist)
                        {
                            strIndSwitch = tblEnt.IndSwitch.ToString();

                            excelrow = sheet.Table.Rows.Add();

                            if (rdoOutcomesselect.Checked)
                            {
                                boolOutComesDetails = false;

                                if (strIndSwitch == "Y")
                                {
                                    boolOutComesDetails = true;

                                }
                                else
                                {
                                    RCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                    if (rngcountdata != null)
                                    {
                                        boolOutComesDetails = true;
                                    }
                                }

                            }

                            if (boolOutComesDetails)
                            {

                                if (Rb_User_Def.Checked)
                                {
                                    boolGoalAssoFilter = false;
                                    if (strIndSwitch == "Y")
                                    {
                                        boolGoalAssoFilter = true;
                                    }
                                    else
                                    {
                                        if (GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim()).Count > 0)
                                            boolGoalAssoFilter = true;
                                    }
                                }
                                if (boolGoalAssoFilter)
                                {



                                    intcount1 = intcount2 = intcout3 = "0";
                                    intcount4 = intcount5 = string.Empty;
                                    RCsb14GroupEntity rngcountdata = null;
                                    if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                    {
                                        if (Rb_SNP_Mem.Checked && (rdbSumDetail.Checked || rbo_ProgramWise1.Checked))
                                        {
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                        }
                                        else
                                        {
                                            if (rdoperiodCumulative.Checked)
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                            else
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                        }
                                        if (rngcountdata == null)
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                    }
                                    else
                                    {
                                        if (Rb_SNP_Mem.Checked && (rdbSumDetail.Checked || rbo_ProgramWise1.Checked))
                                        {
                                            rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                        }
                                        else
                                        {
                                            if (rdoperiodCumulative.Checked)
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                            else
                                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                        }
                                    }
                                    if (rngcountdata != null)
                                    {
                                        
                                        cell = excelrow.Cells.Add(tblEnt.TblCode.Trim() + "." + tblEnt.GrpDesc.ToString(), DataType.String, "NormalLeft");

                                        if (strIndSwitch == "Y" && strResultColumn == "0")  //Added strResultColumn condition by Sudheer on 02/13/25 based on the 'Jen: Outcomes Report Enhancement for FNPI 7a 2/3/2025' in 2025 Enhancements.docx
                                        {

                                            intcount1 = IndSwitch_Table.Rows.Count.ToString();

                                            intcount2 = rngcountdata.ExAchev.ToString();
                                            intcout3 = "0";

                                        }
                                        else
                                        {
                                            if (strIndSwitch == "Y")
                                                intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                            else
                                                intcount1 = rngcountdata.unit_cnt.ToString();
                                            intcount2 = rngcountdata.ExAchev.ToString();
                                            switch (strResultColumn)
                                            {
                                                case "0":
                                                    intcout3 = rngcountdata.unit_cnt.ToString();
                                                    break;
                                                case "1":
                                                    intcout3 = rngcountdata.Hrd1.ToString();
                                                    break;
                                                case "2":
                                                    intcout3 = rngcountdata.Hrd2.ToString();
                                                    break;
                                                case "3":
                                                    intcout3 = rngcountdata.Hrd3.ToString();
                                                    break;
                                                case "4":
                                                    intcout3 = rngcountdata.Hrd4.ToString();
                                                    break;
                                                case "5":
                                                    intcout3 = rngcountdata.Hrd5.ToString();
                                                    break;
                                                default:
                                                    intcout3 = rngcountdata.Hrd1.ToString();
                                                    break;
                                            }
                                            if (strIndSwitch == "Y")
                                                intcout3 = IndSwitch_Table.Rows.Count.ToString();
                                        }
                                        if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                        {
                                            if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                            {
                                                intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                if (intcount4.Length > 4)
                                                    intcount4 = intcount4.Substring(0, 4);
                                            }
                                            if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                            {
                                                intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                if (intcount5.Length > 4)
                                                    intcount5 = intcount5.Substring(0, 4);
                                            }

                                        }


                                        if (rdoperiodBoth.Checked)
                                        {
                                            //PdfPCell pdfDetailstotalTable1 = new PdfPCell(new Phrase("", TableFont));
                                            //pdfDetailstotalTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //pdfDetailstotalTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                            //pdfDetailstotalTable1.Border = iTextSharp.text.Rectangle.BOX;
                                            //subtable.AddCell(pdfDetailstotalTable1);


                                            intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                            intcummulativecount4 = intcummulativecount5 = string.Empty;
                                            RCsb14GroupEntity rngcummulativecounttotaldata = null;
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                if (!rdbSumDetail.Checked)
                                                    rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                                else
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                    {
                                                        rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                                    }
                                                    else
                                                        rngcummulativecounttotaldata = rngcountdata;
                                                }
                                                if (rngcummulativecounttotaldata == null)
                                                    rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            }
                                            else
                                            {
                                                if (!rdbSumDetail.Checked)
                                                {
                                                    rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                                    // rngcummulativecounttotaldata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                                }
                                                else
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                    {
                                                        rngcummulativecounttotaldata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                                    }
                                                    else
                                                        rngcummulativecounttotaldata = rngcountdata;
                                                }
                                            }
                                            if (rngcummulativecounttotaldata != null)
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    if (rdoperiodBoth.Checked)
                                                        intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    else
                                                        intcummulativecount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = rngcummulativecounttotaldata.ExAchev.ToString();
                                                    intcummulativecout3 = "0";
                                                }
                                                else
                                                {
                                                    intcummulativecount1 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                                    intcummulativecount2 = rngcummulativecounttotaldata.ExAchev.ToString();
                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcummulativecout3 = rngcummulativecounttotaldata.Hrd1.ToString();
                                                            break;
                                                    }
                                                }
                                                if (intcummulativecount2 == string.Empty)
                                                    intcummulativecount2 = intcount2;

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }


                                                cell = excelrow.Cells.Add(intcount1 + "  |  " + intcummulativecount1, DataType.String, "Normal");



                                                cell = excelrow.Cells.Add(intcount2, DataType.String, "Normal");


                                                cell = excelrow.Cells.Add(intcout3 + "  |  " + intcummulativecout3, DataType.String, "Normal");

                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + "  |  " + "0", DataType.String, "Normal");

                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(rngcountdata.Hrd1 + "  |  " + rngcummulativecounttotaldata.Hrd1, DataType.String, "Normal");

                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + "  |  " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(rngcountdata.Hrd2 + "  |  " + rngcummulativecounttotaldata.Hrd2, DataType.String, "Normal");

                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + "  |  " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(rngcountdata.Hrd3 + "  |  " + rngcummulativecounttotaldata.Hrd3, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + "  |  " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(rngcountdata.Hrd4 + "  |  " + rngcummulativecounttotaldata.Hrd4, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + "  |  " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(rngcountdata.Hrd5 + "  |  " + rngcummulativecounttotaldata.Hrd5, DataType.String, "Normal");
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                //string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcount5 = intcount5.ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                cell = excelrow.Cells.Add((strcount4 == string.Empty ? "" : strcount4 + "%") + " | " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), DataType.String, "Normal");



                                                // string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();
                                                //string strcummulativecount5 = intcummulativecount5.ToString();
                                                //if (strcummulativecountArray5.Length > 1)
                                                //{
                                                //    if (strcummulativecountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcummulativecount5 = strcummulativecountArray5[0];
                                                //    }
                                                //}

                                                cell = excelrow.Cells.Add((strcount5 == string.Empty ? "" : strcount5 + "%") + " | " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), DataType.String, "Normal");

                                            }
                                        }
                                        else
                                        {
                                            //excelrow = sheet.Table.Rows.Add();

                                            cell = excelrow.Cells.Add(intcount1, DataType.String, "Normal");

                                            cell = excelrow.Cells.Add(intcount2, DataType.String, "Normal");

                                            cell = excelrow.Cells.Add(intcout3, DataType.String, "Normal");

                                            if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(rngcountdata.Hrd1, DataType.String, "Normal");
                                                }
                                            }

                                            if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(rngcountdata.Hrd2, DataType.String, "Normal");
                                                }
                                            }
                                            if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(rngcountdata.Hrd3, DataType.String, "Normal");
                                                }
                                            }
                                            if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(rngcountdata.Hrd4, DataType.String, "Normal");
                                                }
                                            }
                                            if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                            {
                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(rngcountdata.Hrd5, DataType.String, "Normal");
                                                }
                                            }

                                            string[] strcountArray4 = intcount4.Split('.');
                                            string strcount4 = intcount4.ToString();
                                            if (strcountArray4.Length > 1)
                                                strcount4 = strcountArray4[0];

                                            cell = excelrow.Cells.Add(strcount4 == string.Empty ? "" : strcount4 + "%", DataType.String, "Normal");


                                            // string[] strcountArray5 = intcount5.Split('.');
                                            string strcount5 = string.Empty;
                                            if (intcount5 != string.Empty)
                                                strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                            //if (strcountArray5.Length > 1)
                                            //{
                                            //    if (strcountArray5[1].StartsWith("0"))
                                            //    {
                                            //        strcount5 = strcountArray5[0];
                                            //    }
                                            //}

                                            cell = excelrow.Cells.Add(strcount5 == string.Empty ? "" : strcount5 + "%", DataType.String, "Normal");

                                        }
                                    }
                                    if (Rb_SNP_Mem.Checked && (rdbSumDetail.Checked || rbo_ProgramWise1.Checked))
                                    {
                                        if (!rdoperiodBoth.Checked)
                                        {
                                           

                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                excelrow = sheet.Table.Rows.Add();
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;
                                                if (strIndSwitch == "Y")
                                                {
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcount2 = item.ExAchev.ToString();
                                                    intcout3 = "0";
                                                }
                                                else
                                                {
                                                    intcount1 = item.unit_cnt.ToString();
                                                    intcount2 = item.ExAchev.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                    }
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }


                                                cell = excelrow.Cells.Add("     " + item.GrpDesc.ToString(), DataType.String, "Normal");


                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add(intcount1.ToString(), DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(item.unit_cnt.ToString(), DataType.String, "Normal");
                                                }

                                                cell = excelrow.Cells.Add(intcount2.ToString(), DataType.String, "Normal");


                                                cell = excelrow.Cells.Add(intcout3.ToString(), DataType.String, "Normal");


                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");

                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd1, DataType.String, "Normal");
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd2, DataType.String, "Normal");

                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd3, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd4, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd5, DataType.String, "Normal");
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                cell = excelrow.Cells.Add(strcount4 == string.Empty ? "" : strcount4 + "%", DataType.String, "Normal");


                                                // string[] strcountArray5 = intcount5.Split('.');
                                                // string strcount5 = intcount5.ToString();
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                cell = excelrow.Cells.Add(strcount5 == string.Empty ? "" : strcount5 + "%", DataType.String, "Normal");

                                            }
                                        }
                                        else
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            List<RCsb14GroupEntity> rngsubBothdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");

                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");

                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                if (rngsubBothdetailsdata.Count == 0)
                                                    rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH" && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            int intbothcount = 0;

                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;

                                                intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                                intcummulativecount4 = intcummulativecount5 = string.Empty;

                                                if (strIndSwitch == "Y")
                                                {
                                                    intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();
                                                    intcout3 = intcummulativecout3 = "0";
                                                }
                                                else
                                                {

                                                    intcount1 = item.unit_cnt.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();

                                                    intcummulativecount1 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                    }
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }


                                                cell = excelrow.Cells.Add("     " + item.GrpDesc.ToString(), DataType.String, "Normal");

                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add(intcount1.ToString() + " | " + intcummulativecount1, DataType.String, "Normal");

                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(item.unit_cnt.ToString() + " | " + intcummulativecount1, DataType.String, "Normal");

                                                }


                                                cell = excelrow.Cells.Add(intcount2.ToString(), DataType.String, "Normal");



                                                cell = excelrow.Cells.Add(intcout3 + " | " + intcummulativecout3, DataType.String, "Normal");



                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd1 + " | " + rngsubBothdetailsdata[intbothcount].Hrd1, DataType.String, "Normal");
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd2 + " | " + rngsubBothdetailsdata[intbothcount].Hrd2, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd3 + " | " + rngsubBothdetailsdata[intbothcount].Hrd3, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd4 + " | " + rngsubBothdetailsdata[intbothcount].Hrd4, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd5 + " | " + rngsubBothdetailsdata[intbothcount].Hrd5, DataType.String, "Normal");
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                cell = excelrow.Cells.Add((strcount4 == string.Empty ? "" : strcount4 + "%") + " | " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), DataType.String, "Normal");

                                                string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();


                                                //string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();

                                                cell = excelrow.Cells.Add((strcount5 == string.Empty ? "" : strcount5 + "%") + " | " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), DataType.String, "Normal");
                                                intbothcount = intbothcount + 1;
                                            }

                                        }

                                    }
                                    else
                                    {

                                    }

                                    //Added by Sudheer on 10/10/2022 for Program Wise Counts
                                    if (rbo_ProgramWise1.Checked)
                                    {
                                        if (!rdoperiodBoth.Checked)
                                        {


                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                excelrow = sheet.Table.Rows.Add();
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;
                                                if (strIndSwitch == "Y")
                                                {
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcount2 = item.ExAchev.ToString();
                                                    intcout3 = "0";
                                                }
                                                else
                                                {
                                                    intcount1 = item.unit_cnt.ToString();
                                                    intcount2 = item.ExAchev.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            break;
                                                    }
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }


                                                cell = excelrow.Cells.Add("     " + item.GrpDesc.ToString(), DataType.String, "Normal");


                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add(intcount1.ToString(), DataType.String, "Normal");
                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(item.unit_cnt.ToString(), DataType.String, "Normal");
                                                }

                                                cell = excelrow.Cells.Add(intcount2.ToString(), DataType.String, "Normal");


                                                cell = excelrow.Cells.Add(intcout3.ToString(), DataType.String, "Normal");


                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");

                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd1, DataType.String, "Normal");
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd2, DataType.String, "Normal");

                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd3, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd4, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd5, DataType.String, "Normal");
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                cell = excelrow.Cells.Add(strcount4 == string.Empty ? "" : strcount4 + "%", DataType.String, "Normal");


                                                // string[] strcountArray5 = intcount5.Split('.');
                                                // string strcount5 = intcount5.ToString();
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();
                                                //if (strcountArray5.Length > 1)
                                                //{
                                                //    if (strcountArray5[1].StartsWith("0"))
                                                //    {
                                                //        strcount5 = strcountArray5[0];
                                                //    }
                                                //}

                                                cell = excelrow.Cells.Add(strcount5 == string.Empty ? "" : strcount5 + "%", DataType.String, "Normal");

                                            }
                                        }
                                        else
                                        {
                                            List<RCsb14GroupEntity> rngsubdetailsdata = new List<RCsb14GroupEntity>();
                                            List<RCsb14GroupEntity> rngsubBothdetailsdata = new List<RCsb14GroupEntity>();
                                            if (strIndSwitch == "Y" && rdoOutcomesselect.Checked)
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");

                                                if (rngsubdetailsdata.Count == 0)
                                                    rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH");

                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                if (rngsubBothdetailsdata.Count == 0)
                                                    rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "INDSWITCH" && u.Row_Type.ToString() == "INDSWITCH");
                                            }
                                            else
                                            {
                                                rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                                rngsubBothdetailsdata = RngBothResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrgCnt");
                                            }
                                            //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                            int intbothcount = 0;

                                            foreach (RCsb14GroupEntity item in rngsubdetailsdata)
                                            {
                                                intcount1 = intcount2 = intcout3 = "0";
                                                intcount4 = intcount5 = string.Empty;

                                                intcummulativecount1 = intcummulativecount2 = intcummulativecout3 = "0";
                                                intcummulativecount4 = intcummulativecount5 = string.Empty;

                                                if (strIndSwitch == "Y")
                                                {
                                                    intcummulativecount1 = IndSwitchReport_Table.Rows.Count.ToString();
                                                    intcount1 = IndSwitch_Table.Rows.Count.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();
                                                    intcout3 = intcummulativecout3 = "0";
                                                }
                                                else
                                                {

                                                    intcount1 = item.unit_cnt.ToString();
                                                    intcummulativecount2 = intcount2 = item.ExAchev.ToString();

                                                    intcummulativecount1 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();

                                                    switch (strResultColumn)
                                                    {
                                                        case "0":
                                                            intcout3 = item.unit_cnt.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].unit_cnt.ToString();
                                                            break;
                                                        case "1":
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                        case "2":
                                                            intcout3 = item.Hrd2.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd2.ToString();
                                                            break;
                                                        case "3":
                                                            intcout3 = item.Hrd3.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd3.ToString();
                                                            break;
                                                        case "4":
                                                            intcout3 = item.Hrd4.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd4.ToString();
                                                            break;
                                                        case "5":
                                                            intcout3 = item.Hrd5.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd5.ToString();
                                                            break;
                                                        default:
                                                            intcout3 = item.Hrd1.ToString();
                                                            intcummulativecout3 = rngsubBothdetailsdata[intbothcount].Hrd1.ToString();
                                                            break;
                                                    }
                                                }
                                                if (intcout3.Trim() != string.Empty && intcout3 != "0")
                                                {
                                                    if (intcount1.Trim() != string.Empty && intcount1 != "0")
                                                    {
                                                        intcount4 = ((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount1)) * 100).ToString();
                                                        if (intcount4.Length > 4)
                                                            intcount4 = intcount4.Substring(0, 4);
                                                    }
                                                    if (intcount2.Trim() != string.Empty && intcount2 != "0")
                                                    {
                                                        intcount5 = (((Convert.ToDecimal(intcout3) / Convert.ToDecimal(intcount2))) * 100).ToString();
                                                        if (intcount5.Length > 4)
                                                            intcount5 = intcount5.Substring(0, 4);
                                                    }

                                                }

                                                if (intcummulativecout3.Trim() != string.Empty && intcummulativecout3 != "0")
                                                {
                                                    if (intcummulativecount1.Trim() != string.Empty && intcummulativecount1 != "0")
                                                    {
                                                        intcummulativecount4 = ((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount1)) * 100).ToString();
                                                        if (intcummulativecount4.Length > 4)
                                                            intcummulativecount4 = intcummulativecount4.Substring(0, 4);
                                                    }
                                                    if (intcummulativecount2.Trim() != string.Empty && intcummulativecount2 != "0")
                                                    {
                                                        intcummulativecount5 = (((Convert.ToDecimal(intcummulativecout3) / Convert.ToDecimal(intcummulativecount2))) * 100).ToString();
                                                        if (intcummulativecount5.Length > 4)
                                                            intcummulativecount5 = intcummulativecount5.Substring(0, 4);
                                                    }

                                                }


                                                cell = excelrow.Cells.Add("     " + item.GrpDesc.ToString(), DataType.String, "Normal");

                                                if (strIndSwitch == "Y")
                                                {
                                                    cell = excelrow.Cells.Add(intcount1.ToString() + " | " + intcummulativecount1, DataType.String, "Normal");

                                                }
                                                else
                                                {
                                                    cell = excelrow.Cells.Add(item.unit_cnt.ToString() + " | " + intcummulativecount1, DataType.String, "Normal");

                                                }


                                                cell = excelrow.Cells.Add(intcount2.ToString(), DataType.String, "Normal");



                                                cell = excelrow.Cells.Add(intcout3 + " | " + intcummulativecout3, DataType.String, "Normal");



                                                if ((codeitem.Incld1.ToString().ToUpper() == "TRUE") && (strResultColumn != "1" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd1 + " | " + rngsubBothdetailsdata[intbothcount].Hrd1, DataType.String, "Normal");
                                                    }
                                                }

                                                if ((codeitem.Incld2.ToString().ToUpper() == "TRUE") && (strResultColumn != "2" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd2 + " | " + rngsubBothdetailsdata[intbothcount].Hrd2, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld3.ToString().ToUpper() == "TRUE") && (strResultColumn != "3" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd3 + " | " + rngsubBothdetailsdata[intbothcount].Hrd3, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld4.ToString().ToUpper() == "TRUE") && (strResultColumn != "4" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd4 + " | " + rngsubBothdetailsdata[intbothcount].Hrd4, DataType.String, "Normal");
                                                    }
                                                }
                                                if ((codeitem.Incld5.ToString().ToUpper() == "TRUE") && (strResultColumn != "5" || strResultColumn == "0"))
                                                {
                                                    if (strIndSwitch == "Y")
                                                    {
                                                        cell = excelrow.Cells.Add("0" + " | " + "0", DataType.String, "Normal");
                                                    }
                                                    else
                                                    {
                                                        cell = excelrow.Cells.Add(item.Hrd5 + " | " + rngsubBothdetailsdata[intbothcount].Hrd5, DataType.String, "Normal");
                                                    }
                                                }

                                                string[] strcountArray4 = intcount4.Split('.');
                                                string strcount4 = intcount4.ToString();
                                                if (strcountArray4.Length > 1)
                                                    strcount4 = strcountArray4[0];


                                                string[] strcummulativecountArray4 = intcummulativecount4.Split('.');
                                                string strcummulativecount4 = intcummulativecount4.ToString();
                                                if (strcummulativecountArray4.Length > 1)
                                                    strcummulativecount4 = strcummulativecountArray4[0];

                                                cell = excelrow.Cells.Add((strcount4 == string.Empty ? "" : strcount4 + "%") + " | " + (strcummulativecount4 == string.Empty ? "" : strcummulativecount4 + "%"), DataType.String, "Normal");

                                                string[] strcountArray5 = intcount5.Split('.');
                                                string strcount5 = string.Empty;
                                                if (intcount5 != string.Empty)
                                                    strcount5 = (Math.Round(Convert.ToDouble(intcount5), MidpointRounding.AwayFromZero)).ToString();


                                                //string[] strcummulativecountArray5 = intcummulativecount5.Split('.');
                                                string strcummulativecount5 = string.Empty;
                                                if (intcummulativecount5 != string.Empty)
                                                    strcummulativecount5 = (Math.Round(Convert.ToDouble(intcummulativecount5), MidpointRounding.AwayFromZero)).ToString();

                                                cell = excelrow.Cells.Add((strcount5 == string.Empty ? "" : strcount5 + "%") + " | " + (strcummulativecount5 == string.Empty ? "" : strcummulativecount5 + "%"), DataType.String, "Normal");
                                                intbothcount = intbothcount + 1;
                                            }

                                        }

                                    }

                                }
                            }
                        }

                        excelrow = sheet.Table.Rows.Add();
                        excelrow.Height = 100;
                        cell = excelrow.Cells.Add("", DataType.String, "Normal");
                        cell.MergeAcross = 11;

                    }

                }

                FileStream stream1 = new FileStream(PdfName, FileMode.Create);
                book.Save(stream1);
                stream1.Close();
            }
            catch (Exception ex) { }


        }


        #region properties

        public string Scr_Code { get; set; }

        public string Report_Name { get; set; }

        public string Report_To_Process { get; set; }

        public DataTable Result_Table { get; set; }
        public DataTable IndSwitch_Table { get; set; }
        public DataTable IndSwitchReport_Table { get; set; }

        public DataTable Summary_table { get; set; }

        public DataSet Result_DataSet { get; set; }

        public bool Detail_Rep_Required { get; set; }

        public string Main_Rep_Name { get; set; }


        #endregion

        private void On_DetailsReport(DataTable dtDetails)
        {

            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "RNGB0014_" + "Details";
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            //PdfName = strFolderPath + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".pdf";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 9, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 1, BaseColor.BLUE);
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            cb = writer.DirectContent;

            PdfPTable Snp_Table = new PdfPTable(14);
            Snp_Table.TotalWidth = 760f;
            Snp_Table.WidthPercentage = 100;
            Snp_Table.LockedWidth = true;
            float[] widths = new float[] { 20f, 25f, 15f, 15f, 15f, 25f, 55f, 40f, 30f, 25f, 25f, 25f, 25f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
            Snp_Table.SetWidths(widths);
            Snp_Table.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPCell Header = new PdfPCell(new Phrase("Detail Family Report", fc1));
            Header.Colspan = 14;
            Header.FixedHeight = 15f;
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            //Header.BackgroundColor = BaseColor.LIGHT_GRAY;
            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Snp_Table.AddCell(Header);

            PdfPCell row2 = new PdfPCell(new Phrase(""));
            row2.Colspan = 14;
            row2.FixedHeight = 15f;
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Snp_Table.AddCell(row2);

            string[] col = { "Group", "Table", "Agy", "Dept", "Prog", "App", "Client Name", "Activity Date", "Goal", "R1", "R2", "R3", "R4", "R5" };
            for (int i = 0; i < col.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col[i], TableFontBoldItalic));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                Snp_Table.AddCell(cell);
            }

            foreach (DataRow dr in dtDetails.Rows)
            {
                if (!dr["SortUnDup_Table"].ToString().Contains("Desc"))
                {
                    PdfPCell C14 = new PdfPCell(new Phrase(dr["SortUnDup_Group"].ToString(), TableFont));
                    C14.HorizontalAlignment = Element.ALIGN_LEFT;
                    C14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C14);

                    PdfPCell C15 = new PdfPCell(new Phrase(dr["SortUnDup_Table"].ToString(), TableFont));
                    C15.HorizontalAlignment = Element.ALIGN_LEFT;
                    C15.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C15);

                    PdfPCell C1 = new PdfPCell(new Phrase(dr["SortUnDup_Agy"].ToString(), TableFont));
                    C1.HorizontalAlignment = Element.ALIGN_LEFT;
                    C1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C1);

                    PdfPCell C2 = new PdfPCell(new Phrase(dr["SortUnDup_Dept"].ToString(), TableFont));
                    C2.HorizontalAlignment = Element.ALIGN_LEFT;
                    C2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C2);

                    PdfPCell C3 = new PdfPCell(new Phrase(dr["SortUnDup_Prog"].ToString(), TableFont));
                    C3.HorizontalAlignment = Element.ALIGN_LEFT;
                    C3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C3);

                    PdfPCell C4 = new PdfPCell(new Phrase(dr["SortUnDup_App"].ToString(), TableFont));
                    C4.HorizontalAlignment = Element.ALIGN_LEFT;
                    C4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C4);

                    PdfPCell C5 = new PdfPCell(new Phrase(dr["SortUnDup_Name"].ToString(), TableFont));
                    C5.HorizontalAlignment = Element.ALIGN_LEFT;
                    C5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C5);

                    PdfPCell C6 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["SortUnDup_OutCome_Date"].ToString()), TableFont));
                    C6.HorizontalAlignment = Element.ALIGN_LEFT;
                    C6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C6);

                    PdfPCell C7 = new PdfPCell(new Phrase(dr["SortUnDup_OutcomeCode"].ToString(), TableFont));
                    C7.HorizontalAlignment = Element.ALIGN_RIGHT;
                    C7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C7);



                    PdfPCell C9 = new PdfPCell(new Phrase(dr["R1"].ToString(), TableFont));
                    C9.HorizontalAlignment = Element.ALIGN_LEFT;
                    C9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C9);

                    PdfPCell C10 = new PdfPCell(new Phrase(dr["R2"].ToString(), TableFont));
                    C10.HorizontalAlignment = Element.ALIGN_LEFT;
                    C10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C10);

                    PdfPCell C11 = new PdfPCell(new Phrase(dr["R3"].ToString(), TableFont));
                    C11.HorizontalAlignment = Element.ALIGN_RIGHT;
                    C11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C11);

                    PdfPCell C12 = new PdfPCell(new Phrase(dr["R4"].ToString(), TableFont));
                    C12.HorizontalAlignment = Element.ALIGN_LEFT;
                    C12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C12);

                    PdfPCell C13 = new PdfPCell(new Phrase(dr["R5"].ToString(), TableFont));
                    C13.HorizontalAlignment = Element.ALIGN_LEFT;
                    C13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Snp_Table.AddCell(C13);
                }

            }

            document.Add(Snp_Table);
            document.NewPage();

            document.Close();
            fs.Close();
            fs.Dispose();

        }


        private void OnExcel_FamilyForm_Report(DataTable dtFam)
        {
            string PdfName = "Pdf File";
            PdfName = "RNGB0014_Family_Details";
            //string AuditName = PdfName;
            //PdfName = strFolderPath + PdfName;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xls";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xls";

            //Workbook book = new Workbook();

            //this.GenerateStyles(book.Styles);

            ExcelDocument xlWorkSheet = new ExcelDocument();


            xlWorkSheet.ColumnWidth(0, 60);
            xlWorkSheet.ColumnWidth(1, 60);
            xlWorkSheet.ColumnWidth(2, 60);
            xlWorkSheet.ColumnWidth(3, 80);
            xlWorkSheet.ColumnWidth(4, 60);
            xlWorkSheet.ColumnWidth(5, 80);
            xlWorkSheet.ColumnWidth(6, 250);
            xlWorkSheet.ColumnWidth(7, 150);
            xlWorkSheet.ColumnWidth(8, 70);
            xlWorkSheet.ColumnWidth(9, 60);
            xlWorkSheet.ColumnWidth(10, 60);
            xlWorkSheet.ColumnWidth(11, 60);
            xlWorkSheet.ColumnWidth(12, 60);
            xlWorkSheet.ColumnWidth(13, 60);


            int excelcolumn = 0;
            try
            {
                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
                xlWorkSheet.WriteCell(excelcolumn, 5, "Detail Family Report");

                excelcolumn = excelcolumn + 2;


                xlWorkSheet[excelcolumn, 0].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 0].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 0, "Group");

                xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 1, "Table");

                xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 2, "Agency");

                xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 3, "Dept");

                xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 4, "Prog");

                xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 5, "App");

                xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 6, "Client Name");

                xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 7, "Activity Date");

                xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 8, "Goal");

                xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 9, "R1");

                xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 10, "R2");

                xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 11, "R3");

                xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 12, "R4");

                xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
                xlWorkSheet.WriteCell(excelcolumn, 13, "R5");

                string strGroupCode = string.Empty;
                bool boolfirst = true;

                if (dtFam.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFam.Rows)
                    {
                        if (!dr["SortUnDup_Table"].ToString().Contains("Desc"))
                        {
                            if (boolfirst)
                            {

                                strGroupCode = dr["SortUnDup_Group"].ToString().Trim();
                                RCsb14GroupEntity rngcodedescdata = RngCodelist.Find(u => u.GrpCode.Trim() == strGroupCode && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                                if (rngcodedescdata != null)
                                {
                                    excelcolumn = excelcolumn + 2;
                                    xlWorkSheet.WriteCell(excelcolumn, 6, rngcodedescdata.GrpDesc.ToString());
                                    excelcolumn = excelcolumn + 2;
                                }
                                boolfirst = false;
                            }
                            else
                            {
                                if (strGroupCode != dr["SortUnDup_Group"].ToString().Trim())
                                {
                                    strGroupCode = dr["SortUnDup_Group"].ToString().Trim();
                                    RCsb14GroupEntity rngcodedescdata = RngCodelist.Find(u => u.GrpCode.Trim() == strGroupCode && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
                                    if (rngcodedescdata != null)
                                    {
                                        excelcolumn = excelcolumn + 2;
                                        xlWorkSheet.WriteCell(excelcolumn, 6, rngcodedescdata.GrpDesc.ToString());
                                        excelcolumn = excelcolumn + 2;
                                    }
                                }
                            }


                            excelcolumn = excelcolumn + 1;
                            xlWorkSheet.WriteCell(excelcolumn, 0, dr["SortUnDup_Group"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 1, dr["SortUnDup_Table"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 2, dr["SortUnDup_Agy"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 3, dr["SortUnDup_Dept"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 4, dr["SortUnDup_Prog"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 5, dr["SortUnDup_App"].ToString());
                            //xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
                            //xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(excelcolumn, 6, dr["SortUnDup_Name"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 7, LookupDataAccess.Getdate(dr["SortUnDup_OutCome_Date"].ToString()));
                            xlWorkSheet.WriteCell(excelcolumn, 8, dr["SortUnDup_OutcomeCode"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 9, dr["R1"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 10, dr["R2"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 11, dr["R3"].ToString());
                            //xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
                            //xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(excelcolumn, 12, dr["R4"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 13, dr["R5"].ToString());


                        }
                    }
                }

                if (!boolfirst)
                {
                    FileStream stream = new FileStream(PdfName, FileMode.Create);
                    xlWorkSheet.Save(stream);
                    stream.Close();
                }


            }
            catch (Exception ex) { }

            //Generate(PdfName);
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }


        #endregion


        private void SecondPage(DataSet dscategories, Document document)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(60, 15, 112));
            iTextSharp.text.Font fc2 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            cb.BeginText();

            cb.SetFontAndSize(bfTimesBold, 10);
            cb.SetRGBColorFill(4, 4, 15);
            X_Pos = 300; Y_Pos = 785;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            Y_Pos -= 15;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            Y_Pos -= 20;

            if (dscategories.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dscategories.Tables[0];
                DataView dv = new DataView(dt);
                dv.Sort = "RNG4CATG_SEQ";
                dt = dv.ToTable();

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; //Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting:", X_Pos, Y_Pos, 0);
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(210, Y_Pos - 2);
                cb.LineTo(580, Y_Pos - 2);
                cb.LineTo(580, Y_Pos + 8);
                cb.LineTo(210, Y_Pos + 8);
                cb.ClosePathStroke();

                //Y_Pos -= 15;
                //cb.SetRGBColorFill(45, 45, 153);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos);
                //cb.LineTo(580, Y_Pos);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(540, Y_Pos + 10);
                //cb.ClosePathFillStroke();

                //Y_Pos -= 15;
                //cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos);
                //cb.LineTo(580, Y_Pos);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(540, Y_Pos + 10);
                //cb.ClosePathFillStroke();


                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                PdfPCell headcell = new PdfPCell(new Phrase(""));
                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                head.AddCell(headcell);

                PdfPTable outer = new PdfPTable(2);
                outer.TotalWidth = 500f;
                outer.LockedWidth = true;
                float[] widths2 = new float[] { 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                outer.SetWidths(widths2);
                outer.HorizontalAlignment = Element.ALIGN_LEFT;
                outer.SpacingBefore = 70f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table.SetWidths(widths);
                //table.SpacingBefore = 270f;

                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 30f, 30f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Rtable.SetWidths(widths1);

                PdfPCell Header = new PdfPCell(new Phrase("D. HOUSEHOLD LEVEL CHARACTERISTICS", fc1));
                Header.Colspan = 3;
                Header.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.FixedHeight = 15f;
                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(Header);

                PdfPCell RHeader = new PdfPCell(new Phrase(" ", fc1));
                RHeader.Colspan = 3;
                RHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                RHeader.FixedHeight = 15f;
                RHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Rtable.AddCell(RHeader);

                bool First = true; int Count = 9; bool RFirst = true; bool EduCol = true; bool Rtab = true;
                string PrivType = string.Empty; bool Disable = true; bool Health = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) > 70)
                    {
                        if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) >= 107)
                        {
                            if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
                            {
                                if (Rtab)
                                {
                                    PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R0.FixedHeight = 10f;
                                    R0.Colspan = 2;
                                    R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R0);

                                    PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
                                    R02.FixedHeight = 10f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R02);

                                    RFirst = false; First = true; Rtab = false;
                                }

                                if (!First)
                                {
                                    PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R0.FixedHeight = 10f;
                                    R0.Colspan = 2;
                                    R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R0);

                                    PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
                                    R02.FixedHeight = 10f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Rtable.AddCell(R02);

                                    if (dr["RNG4CATG_CODE"].ToString() == "J")
                                    {
                                        PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
                                        P0.Colspan = 3;
                                        P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Rtable.AddCell(P0);
                                    }
                                }

                                if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "E")
                                {
                                    PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
                                    R3.FixedHeight = 10f;
                                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                    Rtable.AddCell(R3);

                                    PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R4.FixedHeight = 10f;
                                    R4.Colspan = 2;
                                    R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R4.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                    Rtable.AddCell(R4);

                                    PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
                                    R1.FixedHeight = 10f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);
                                }
                                else if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
                                    R1.FixedHeight = 10f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);
                                }
                                else if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "R")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase("I.Race", TblFontBold));
                                    R1.FixedHeight = 10f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 10f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 10f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                    Rtable.AddCell(R2);
                                }

                                PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
                                //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

                                First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && dr["RNG4CATG_CODE"].ToString().Trim() != "E") Count++;
                            }
                            else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
                                R1.FixedHeight = 10f;
                                R1.Colspan = 2;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                Rtable.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("", Times));
                                R2.FixedHeight = 10f;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.BOX;
                                //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                Rtable.AddCell(R2);
                            }
                        }
                        else if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
                        {
                            if (!First)
                            {
                                if (PrivType == "U")
                                {
                                    PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R0.FixedHeight = 10f;
                                    R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R0);

                                    PdfPCell R01 = new PdfPCell(new Phrase("0", TblFontBold));
                                    R01.FixedHeight = 10f;
                                    R01.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R01.Border = iTextSharp.text.Rectangle.BOX;
                                    R01.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
                                    R02.FixedHeight = 10f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R02);
                                }
                                else
                                {
                                    if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && PrivType != "N")
                                    {
                                        PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R0.FixedHeight = 10f;
                                        R0.Colspan = 2;
                                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R0);

                                        PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
                                        R02.FixedHeight = 10f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);

                                        if (dr["RNG4CATG_CODE"].ToString() == "J")
                                        {
                                            PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
                                            P0.Colspan = 3;
                                            P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Rtable.AddCell(P0);
                                        }
                                    }
                                }
                            }

                            if (Count == 5 && dr["RNG4CATG_CODE"].ToString().Trim() == "D")
                            {
                                PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Health", TblFontBold));
                                R3.FixedHeight = 10f;
                                R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R3);

                                PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                R4.FixedHeight = 10f;
                                R4.Colspan = 2;
                                R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R4.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R4);
                            }

                            if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 10f;
                                R1.Colspan = 3;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                table.AddCell(R1);
                            }
                            else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 10f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                R2.FixedHeight = 10f;
                                R2.Colspan = 2;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R2);
                            }

                            PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
                            //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

                            First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N") Count++;
                        }
                        else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
                        {
                            if (dr["RNG4CATG_CODE"].ToString().Trim() == "D" && Disable)
                            {
                                dv = new DataView(dt);
                                dv.RowFilter = "RNG4CATG_CODE='D' AND RNG4CATG_DEM<>''";
                                DataTable dtDis = dv.ToTable();

                                if (dtDis.Rows.Count > 0)
                                {
                                    PdfPTable NestedTable = new PdfPTable(4);
                                    NestedTable.TotalWidth = 240f;
                                    NestedTable.LockedWidth = true;
                                    float[] Dwidths = new float[] { 100f, 30f, 30f, 30f };
                                    NestedTable.SetWidths(Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                    D1.FixedHeight = 10f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D1);

                                    foreach (DataRow drDis in dtDis.Rows)
                                    {
                                        PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
                                        D2.FixedHeight = 10f;
                                        D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        NestedTable.AddCell(D2);
                                    }

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.FixedHeight = 10f;
                                    R11.Colspan = 3;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R11);

                                    if (NestedTable.Rows.Count > 0)
                                        NestedTable.DeleteBodyRows();

                                    PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", Times));
                                    D3.FixedHeight = 10f;
                                    D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D3);
                                    for (int i = 0; i < dtDis.Rows.Count; i++)
                                    {
                                        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
                                        R2.FixedHeight = 10f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R2);
                                    }

                                    PdfPCell R12 = new PdfPCell(NestedTable);
                                    R12.FixedHeight = 10f;
                                    R12.Colspan = 3;
                                    R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R12);
                                }
                                Disable = false;

                            }

                            if (dr["RNG4CATG_CODE"].ToString().Trim() == "N" && Health)
                            {
                                dv = new DataView(dt);
                                dv.RowFilter = "RNG4CATG_CODE='N' AND RNG4CATG_DEM<>''";
                                DataTable dtDis = dv.ToTable();

                                if (dtDis.Rows.Count > 0)
                                {
                                    PdfPTable NestedTable = new PdfPTable(4);
                                    NestedTable.TotalWidth = 250f;
                                    NestedTable.LockedWidth = true;
                                    float[] Dwidths = new float[] { 100f, 30f, 30f, 30f };
                                    NestedTable.SetWidths(Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                    D1.FixedHeight = 10f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D1);

                                    foreach (DataRow drDis in dtDis.Rows)
                                    {
                                        PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
                                        D2.FixedHeight = 10f;
                                        D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        NestedTable.AddCell(D2);
                                    }

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.FixedHeight = 10f;
                                    R11.Colspan = 3;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R11);

                                    if (NestedTable.Rows.Count > 0)
                                        NestedTable.DeleteBodyRows();

                                    PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance*", Times));
                                    D3.FixedHeight = 10f;
                                    D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D3);
                                    for (int i = 0; i < dtDis.Rows.Count; i++)
                                    {
                                        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
                                        R2.FixedHeight = 10f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R2);
                                    }

                                    PdfPCell R12 = new PdfPCell(NestedTable);
                                    R12.FixedHeight = 10f;
                                    R12.Colspan = 3;
                                    R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R12);
                                }
                                Health = false;
                            }


                            if (dr["RNG4CATG_CODE"].ToString().Trim() == "U")
                            {
                                if (EduCol)
                                {
                                    PdfPCell R11 = new PdfPCell(new Phrase("", Times));
                                    R11.FixedHeight = 10f;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R11);

                                    PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
                                    R12.FixedHeight = 10f;
                                    R12.HorizontalAlignment = Element.ALIGN_CENTER;
                                    R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R12);

                                    PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
                                    R13.FixedHeight = 10f;
                                    R13.HorizontalAlignment = Element.ALIGN_CENTER;
                                    R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R13);
                                    EduCol = false;
                                }

                                PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
                                R1.FixedHeight = 10f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("", Times));
                                R2.FixedHeight = 10f;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(R2);

                                PdfPCell R3 = new PdfPCell(new Phrase("", Times));
                                R3.FixedHeight = 10f;
                                R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R3.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(R3);
                            }
                            else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
                                R1.FixedHeight = 10f;
                                R1.Colspan = 2;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("", Times));
                                R2.FixedHeight = 10f;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(R2);
                            }
                        }
                    }
                }
                //if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) > 133)
                //{
                PdfPCell L0 = new PdfPCell(new Phrase("", TblFontBold));
                L0.FixedHeight = 10f;
                L0.Colspan = 3;
                L0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(L0);

                PdfPCell L01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                L01.FixedHeight = 10f;
                L01.Colspan = 2;
                L01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Rtable.AddCell(L01);

                PdfPCell L02 = new PdfPCell(new Phrase("0", TblFontBold));
                L02.FixedHeight = 10f;
                L02.HorizontalAlignment = Element.ALIGN_RIGHT;
                L02.Border = iTextSharp.text.Rectangle.BOX;
                L02.BackgroundColor = BaseColor.LIGHT_GRAY;
                Rtable.AddCell(L02);

                PdfPCell L03 = new PdfPCell(new Phrase("", TblFontBold));
                L03.FixedHeight = 10f;
                L03.Colspan = 3;
                L03.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Rtable.AddCell(L03);


                PdfPCell O1 = new PdfPCell(table);
                O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O1);

                PdfPCell O2 = new PdfPCell(Rtable);
                O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O2);

                PdfPTable LastOuter = new PdfPTable(1);
                LastOuter.TotalWidth = 520f;
                LastOuter.LockedWidth = true;
                //float[] Lastwidths2 = new float[] { 150f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                //Last.SetWidths(Lastwidths2);
                LastOuter.HorizontalAlignment = Element.ALIGN_LEFT;
                LastOuter.SpacingBefore = 30f;

                PdfPTable Last = new PdfPTable(3);
                Last.TotalWidth = 500f;
                Last.LockedWidth = true;
                float[] Lastwidths2 = new float[] { 150f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Last.SetWidths(Lastwidths2);
                Last.HorizontalAlignment = Element.ALIGN_LEFT;
                Last.SpacingBefore = 30f;

                //iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, new iTextSharp.text.BaseColor(204, 169, 217));

                PdfPCell S0 = new PdfPCell(new Phrase("E. Number of Individuals Not Included in the Totals Above (due to data collection system integration barriers)", fc2));
                S0.FixedHeight = 10f;
                S0.Colspan = 3;
                S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S0);

                PdfPCell S1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of INDIVIDUALS served in each program*:", Times));
                S1.FixedHeight = 10f;
                //S1.Colspan = 3;
                S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S1);

                PdfPCell S2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                S2.FixedHeight = 10f;
                S2.HorizontalAlignment = Element.ALIGN_CENTER;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                S2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S2);

                PdfPCell S3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                S3.FixedHeight = 10f;
                S3.HorizontalAlignment = Element.ALIGN_CENTER;
                S3.Border = iTextSharp.text.Rectangle.BOX;
                S3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 10f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 10f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 10f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M0 = new PdfPCell(new Phrase("F. Number of Households Not Included in the Totals Above (due to data collection system integration barriers)", fc1));
                M0.FixedHeight = 10f;
                M0.Colspan = 3;
                M0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M0);

                PdfPCell M1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of HOUSEHOLDS served in each program*:", Times));
                M1.FixedHeight = 10f;
                //S1.Colspan = 3;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M1);

                PdfPCell M2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                M2.FixedHeight = 10f;
                M2.HorizontalAlignment = Element.ALIGN_CENTER;
                M2.Border = iTextSharp.text.Rectangle.BOX;
                M2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M2);

                PdfPCell M3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                M3.FixedHeight = 10f;
                M3.HorizontalAlignment = Element.ALIGN_CENTER;
                M3.Border = iTextSharp.text.Rectangle.BOX;
                M3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 10f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 10f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 10f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M5 = new PdfPCell(new Phrase("  *The system will add rows to allow reporting on multiple programs.", TblFontSmall));
                M5.FixedHeight = 10f;
                M5.Colspan = 3;
                M5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M5);

                PdfPCell M6 = new PdfPCell(new Phrase(" ", TblFontSmall));
                M6.FixedHeight = 5f;
                M6.Colspan = 3;
                M6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M6);

                PdfPCell LO1 = new PdfPCell(Last);
                //LO1.Border = iTextSharp.text.Rectangle.BOX;
                LO1.BorderColor = BaseColor.RED;
                LO1.BorderWidth = 2f;
                LastOuter.AddCell(LO1);

                document.Add(head);
                document.Add(outer);

                document.Add(LastOuter);
                //break;
                //}
            }
        }


        List<Csb16DTREntity> DateRange_List = new List<Csb16DTREntity>();
        List<Csb16DTREntity> Sys_DateRange_List = new List<Csb16DTREntity>();
        private void Get_MasterTable_DateRanges()
        {
            Csb16DTREntity Search_Entity = new Csb16DTREntity(true);
            DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");

            Search_Entity.SYS_Date_Range = DateTime.Today.ToShortDateString();
            Sys_DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");
        }

        private bool Validate_PM_DateRanges()
        {
            bool Valid_Dates = false;

            //foreach (Csb16DTREntity Entity in DateRange_List)
            //{
            //    if (Ref_From_Date.Value.ToShortDateString() == Convert.ToDateTime(Entity.REF_FDATE).ToShortDateString() &&
            //        Ref_To_Date.Value.ToShortDateString() == Convert.ToDateTime(Entity.REF_TDATE).ToShortDateString())
            //    {
            //        Valid_Dates = true; break;
            //    }
            //}

            if (!Valid_Dates)
            {
                string Disp_Date_Ranges = "Available Date Ranges are as Below \n \n" +
                                          "               From                TO ";
                int i = 1;
                foreach (Csb16DTREntity Entity in DateRange_List)
                {
                    Disp_Date_Ranges += "\n " + (i <= 9 ? "  " : "") + i.ToString() + ").  " + LookupDataAccess.Getdate(Entity.REF_FDATE) + "   -   " + LookupDataAccess.Getdate(Entity.REF_TDATE);
                    i++;
                }
                AlertBox.Show(Disp_Date_Ranges, MessageBoxIcon.Warning);
            }

            return Valid_Dates;
        }

        private bool Validate_Report()
        {
            bool Can_Generate = true;

            if (!Ref_From_Date.Checked)
            {
                _errorProvider.SetError(Ref_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'From Date'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Ref_From_Date, null);

            if (!Ref_To_Date.Checked)
            {
                _errorProvider.SetError(Ref_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                if (Ref_From_Date.Checked)
                    _errorProvider.SetError(Ref_To_Date, null);
            }

            if (Ref_To_Date.Checked && Ref_From_Date.Checked)
            {
                if (Ref_From_Date.Value > Ref_To_Date.Value)
                {
                    _errorProvider.SetError(Ref_To_Date, string.Format("Reference Period 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (Ref_To_Date.Checked && Ref_From_Date.Checked)
                    _errorProvider.SetError(Ref_To_Date, null);
            }


            if (!Rep_From_Date.Checked)
            {
                _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rep_From_Date, null);

            if (!Rep_To_Date.Checked)
            {
                _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                if (Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_To_Date, null);
            }

            if (Rep_To_Date.Checked && Rep_From_Date.Checked)
            {
                if (Rep_From_Date.Value > Rep_To_Date.Value)
                {
                    _errorProvider.SetError(Rep_From_Date, string.Format("Report Period 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (Rep_To_Date.Checked && Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_From_Date, null);
            }

            if(rdoperiodBoth.Checked|| rdoperiod.Checked)
            {
                if (Ref_To_Date.Checked && Ref_From_Date.Checked && Rep_To_Date.Checked && Rep_From_Date.Checked)
                {
                    if (Ref_From_Date.Value > Rep_From_Date.Value || Ref_To_Date.Value < Rep_To_Date.Value)
                    {
                        _errorProvider.SetError(Rep_To_Date, string.Format("Report Period Should be in 'Reference Period Date Range'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Rep_To_Date, null);
                }
            }
            else if(rdoperiodCumulative.Checked)
            {
                Rep_From_Date.Value = Ref_From_Date.Value;
                Rep_To_Date.Value = Ref_To_Date.Value;
            }
            
            //else
            //{
            //    if (Ref_To_Date.Checked && Ref_From_Date.Checked && Rep_To_Date.Checked && Rep_From_Date.Checked)
            //        _errorProvider.SetError(Rep_From_Date, null);
            //}

            if (Rb_Fund_Sel.Checked && Sel_Funding_List.Count <= 0)
            {
                _errorProvider.SetError(Rb_Fund_Sel, string.Format("Please Select at least One 'Fund'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Fund_Sel, null);


            if ((Rb_Zip_Sel.Checked && ListZipCode.Count <= 0 && Scr_Oper_Mode == "RNGB0004") || (Rb_Zip_Sel.Checked && ListRngGroupCode.Count <= 0 && Scr_Oper_Mode == "RNGB0014"))
            {
                _errorProvider.SetError(Rb_Zip_Sel, string.Format("Please Select at least One " + (Scr_Oper_Mode == "RNGB0004" ? "'ZIP Code'" : "'Group'").Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Zip_Sel, null);

            if (Scr_Oper_Mode == "RNGB0004" || Scr_Oper_Mode == "RNGB0014")
            {
                if (Rb_County_Sel.Checked && ListcommonEntity.Count <= 0)
                {
                    _errorProvider.SetError(Rb_County_Sel, string.Format("Please Select at least One 'County'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_County_Sel, null);
            }

            if (Rb_Site_Sel.Checked && string.IsNullOrEmpty(Txt_Sel_Site.Text.Trim()))
            {
                _errorProvider.SetError(Rb_Site_Sel, string.Format("Please Select at least One 'Site'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Site_Sel, null);

            if (rdoMsselectsite.Checked && string.IsNullOrEmpty(txt_Msselect_site.Text.Trim()))
            {
                _errorProvider.SetError(rdoMsselectsite, string.Format("Please Select at least One 'Outcome Posting Site'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(rdoMsselectsite, null);

            if (string.IsNullOrEmpty(Txt_Pov_Low.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Pov_Low, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Poverty Level 'Low'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Txt_Pov_Low, null);

            if (string.IsNullOrEmpty(Txt_Pov_High.Text.Trim()))
            {
                _errorProvider.SetError(Txt_Pov_High, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Poverty Level 'High'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Txt_Pov_High, null);

            if (!string.IsNullOrEmpty(Txt_Pov_Low.Text.Trim()) && !string.IsNullOrEmpty(Txt_Pov_High.Text.Trim()))
            {
                if (int.Parse(Txt_Pov_Low.Text) > int.Parse(Txt_Pov_High.Text))
                {
                    _errorProvider.SetError(Txt_Pov_High, string.Format("Poverty Level 'Low' should not Exceed 'High'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Txt_Pov_High, null);
            }
            if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() == "**")
            {
                _errorProvider.SetError(cmbRngCode, "Please Select Code");
                Can_Generate = false;
            }
            else
            {
                _errorProvider.SetError(cmbRngCode, null);
            }

            if (rdbSelAlertCds.Checked)
            {
                if (SelectedAlertCodes == "")
                {
                    _errorProvider.SetError(rdbSelAlertCds, "Please Select atleast one Alert Code");
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(rdbSelAlertCds, null);
                }
            }


            return Can_Generate;
        }






        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        DG_Browse_Entity Search_Entity = new DG_Browse_Entity(true);
        PM_Browse_Entity PM_Search_Entity = new PM_Browse_Entity(true);
        string[] Sel_params_To_Print = new string[20] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
        private void Get_Selection_Criteria()
        {
            Sel_params_To_Print[4] = string.Empty;
            switch (Scr_Oper_Mode)
            {

                case "RNGB0014":
                    Search_Entity.Attribute = "All";
                    if (Rb_User_Def.Checked)
                        Search_Entity.Attribute = "Goal";

                    if (Rb_Zip_All.Checked)
                        Search_Entity.ZipCode = "**";
                    else
                    {
                        string strGrpcode = Get_Sel_GroupCodes();
                        if (strGrpcode.Trim() == "'7'")
                            Search_Entity.ZipCode = "**";
                        else
                            Search_Entity.ZipCode = strGrpcode;
                    }

                    Search_Entity.County = "**";
                    if (Rb_County_Sel.Checked)
                        Search_Entity.County = Get_Sel_County();

                    Search_Entity.PM_Rep_Format = "PM";
                    if (rbo_ProgramWise1.Checked)
                        Search_Entity.PM_Rep_Format = "Prog";
                    else if (Rb_SNP_Mem.Checked)
                        Search_Entity.PM_Rep_Format = "Both";
                    else if(rbo_ProgramWise1.Checked)
                        Search_Entity.PM_Rep_Format = "Prog";

                    Search_Entity.DG_Count_Sw = Search_Entity.PM_Rep_Format;

                    break;
            }

            Search_Entity.RngMainCode = ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString();
            Search_Entity.Ms_DriveColumn_Sw = "MSDATE";
            if (Rb_MS_AddDate.Checked)
                Search_Entity.Ms_DriveColumn_Sw = "MSADDDATE";

            Search_Entity.CA_MS_Sw = "MS";


            Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy");//Ref_From_Date.Value.ToShortDateString();
            Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy");//Ref_To_Date.Value.ToShortDateString();
            if (rdoperiod.Checked)
            {
                Search_Entity.Rep_From_Date = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_To_Date = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();
            }
            Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
            Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();

            if (rdoperiodCumulative.Checked)
            {
                if (Rb_SNP_Mem.Checked && (rdbSumDetail.Checked || rbo_ProgramWise1.Checked))
                {
                    Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_From_Date.Value.ToShortDateString();
                    Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
                }
            }

            Search_Entity.Mst_CaseType_Sw = ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Value.ToString();  //"**"; 

            Search_Entity.Stat_Detail = "N";
            if (Rb_Details_Yes.Checked)
                Search_Entity.Stat_Detail = "Y";

            Search_Entity.Mst_Secret_Sw = "B";
            if (Rb_Mst_Sec.Checked)
                Search_Entity.Mst_Secret_Sw = "Y";
            else
                if (Rb_Mst_NonSec.Checked)
                Search_Entity.Mst_Secret_Sw = "N";

            Search_Entity.Mst_Acive_Sw = "Y";
            if (Rb_Stat_InAct.Checked)
                Search_Entity.Mst_Acive_Sw = "N";
            else
                if (Rb_Stat_Both.Checked)
                Search_Entity.Mst_Acive_Sw = "B";

            Search_Entity.Mst_Site = "**";
            if (Rb_Site_Sel.Checked)
                Search_Entity.Mst_Site = Get_Sel_Sites();
            else if (Rb_Site_No.Checked)
                Search_Entity.Mst_Site = "NO";


            Search_Entity.CaseMssite = "**";
            if (rdoMsselectsite.Checked)
                Search_Entity.CaseMssite = Get_Sel_CasemsSites();
            else if (rdomsNosite.Checked)
                Search_Entity.CaseMssite = "NO";

            Search_Entity.CA_Fund_Filter = "**";
            if (Rb_Fund_Sel.Checked)
                Search_Entity.CA_Fund_Filter = Get_Sel_CA_Fund_List_To_Filter();


            Search_Entity.Mst_Poverty_Low = Txt_Pov_Low.Text;
            Search_Entity.Mst_Poverty_High = Txt_Pov_High.Text;

            Search_Entity.Hierarchy = Current_Hierarchy + (CmbYear.Visible ? ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Value.ToString() : string.Empty);
            Search_Entity.Activity_Prog = "A";
            Sel_Permesures_Programs = "";
            //if (Cmb_Program.SelectedItem != null)
            //{
            //    string Tmp_Hie_On_Porgram = ((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Value.ToString();
            //    if (((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Value.ToString() == "**")
            //    {

            //        Search_Entity.Activity_Prog = Tmp_Hie_On_Porgram = "******";
            //    }

            //    Sel_Permesures_Programs = Search_Entity.Activity_Prog = Tmp_Hie_On_Porgram;
            //}

            string Sel_Programs = string.Empty;
            if (rbSelProgram.Checked == true)
            {
                if (SelectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity Entity in SelectedHierarchies)
                    {
                        Sel_Programs +=  Entity.Agency + Entity.Dept + Entity.Prog + ",";
                    }

                    if (Sel_Programs.Length > 0)
                        Sel_Programs = Sel_Programs.Substring(0, (Sel_Programs.Length - 1));

                    if (Sel_Programs.Length > 0)
                        Sel_Permesures_Programs = Search_Entity.Activity_Prog = Sel_Programs;
                }
            }

            Search_Entity.UserName = BaseForm.UserID;

            switch (Search_Entity.Mst_Secret_Sw)
            {
                case "Y": Sel_params_To_Print[0] = "Secret Only"; break;
                case "N": Sel_params_To_Print[0] = "Non Secret Only"; break;
                default: Sel_params_To_Print[0] = "Both Secret and Non Secret "; break;
            }
            switch (Search_Entity.Mst_Acive_Sw)
            {
                case "Y": Sel_params_To_Print[1] = "Active"; break;
                case "N": Sel_params_To_Print[1] = "Inactive"; break;
                default: Sel_params_To_Print[1] = "Both Active and Inactive"; break;
            }
            switch (Search_Entity.Mst_Site)
            {
                case "**": Sel_params_To_Print[2] = "All Sites"; break;
                case "NO": Sel_params_To_Print[2] = "No Sites"; break;
                default: Sel_params_To_Print[2] = "Selected Site"; break;
            }
            switch (Search_Entity.CaseMssite)
            {
                case "**": Sel_params_To_Print[3] = "ALL MS Sites"; break;
                case "NO": Sel_params_To_Print[3] = "No MS Sites"; break;
                default: Sel_params_To_Print[3] = "Selected MS Site"; break;
            }
            Search_Entity.OutComeSwitch = rdoOutcomesAll.Checked == true ? "A" : "S";
            Search_Entity.IncVerSwitch = "Y";//chkincverswitch.Checked == true ? "Y" : "N";
        }

        private string Get_Sel_CAMS_List_To_Filter()
        {
            string Sel_Codes = null;


            foreach (CAMASTEntity Entity in Sel_CA_List)
            {
                Sel_Codes += "'" + Entity.Code + "' ,";
            }

            if (Sel_Codes.Length > 0)
                Sel_Codes = Sel_Codes.Substring(0, (Sel_Codes.Length - 1));


            return Sel_Codes;
        }


        private string Get_Sel_ZipCodes()
        {
            string Sel_Zip_Codes = string.Empty;
            foreach (ZipCodeEntity Entity in ListZipCode) //ListZipCode)
            {
                Sel_Zip_Codes += "'" + Entity.Zcrzip + (!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty) + "' ,";
            }

            if (Sel_Zip_Codes.Length > 0)
                Sel_Zip_Codes = Sel_Zip_Codes.Substring(0, (Sel_Zip_Codes.Length - 1));

            return Sel_Zip_Codes;
        }

        private string Get_Sel_County()
        {
            string Sel_County_Codes = null;
            foreach (CommonEntity Entity in ListcommonEntity)
            {
                Sel_County_Codes += "'" + Entity.Code + "' ,";
            }

            if (Sel_County_Codes.Length > 0)
                Sel_County_Codes = Sel_County_Codes.Substring(0, (Sel_County_Codes.Length - 1));

            return Sel_County_Codes;
        }

        private string Get_Sel_Sites()
        {
            string Sel_Site_Codes = string.Empty;

            foreach (CaseSiteEntity casesite in ListcaseSiteEntity) //Site_List)//ListcaseSiteEntity)
            {
                Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
            }

            if (Sel_Site_Codes.Length > 0)
                Sel_Site_Codes = Sel_Site_Codes.Substring(0, (Sel_Site_Codes.Length - 1));

            return Sel_Site_Codes;
        }

        private string Get_Sel_CasemsSites()
        {
            string Sel_Site_Codes = string.Empty;

            foreach (CaseSiteEntity casesite in ListcaseMsSiteEntity) //Site_List)//ListcaseSiteEntity)
            {
                Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
            }

            if (Sel_Site_Codes.Length > 0)
                Sel_Site_Codes = Sel_Site_Codes.Substring(0, (Sel_Site_Codes.Length - 1));

            return Sel_Site_Codes;
        }

        private string Get_Sel_GroupCodes()
        {
            string Sel_Groups_Codes = string.Empty;
            foreach (RCsb14GroupEntity Entity in ListRngGroupCode)
            {
                Sel_Groups_Codes += "'" + Entity.GrpCode + "' ,";
            }

            if (Sel_Groups_Codes.Length > 0)
                Sel_Groups_Codes = Sel_Groups_Codes.Substring(0, (Sel_Groups_Codes.Length - 1));

            return Sel_Groups_Codes;
        }

        List<RCsb14GroupEntity> OutCome_MasterList = new List<RCsb14GroupEntity>();
        private void Prepare_Selected_Group(string Sel_ZipCodes)
        {
            ListRngGroupCode.Clear();
            OutCome_MasterList = _model.SPAdminData.Browse_RNGGrp(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), null, null, null, null,BaseForm.UserID, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
            foreach (RCsb14GroupEntity Entity in OutCome_MasterList)
            {
                if (Sel_ZipCodes.Contains(Entity.GrpCode.Trim()))
                    ListRngGroupCode.Add(new RCsb14GroupEntity(Entity));
            }
        }


        string Sel_AGY, Sel_DEPT, Sel_PROG = string.Empty;
        string Sel_Secret_App, Sel_Group_Sort, Sel_Use_CASEDIFF, Sel_Include_Members = string.Empty, Sel_Permesures_Programs = "";
        private void Get_Report_Selection_Parameters()
        {
            Sel_AGY = Current_Hierarchy.Substring(0, 2);
            Sel_DEPT = Current_Hierarchy.Substring(2, 2);
            Sel_PROG = Current_Hierarchy.Substring(4, 2);

            //switch (((ListItem)Cmb_Applications.SelectedItem).Value.ToString())
            //{
            //    case "1": Sel_Secret_App = "Non-Secret Only"; break;
            //    case "2": Sel_Secret_App = "Secret Only"; break;
            //    case "3": Sel_Secret_App = "Both Secret & Non-Secret"; break;
            //}

            //switch (((ListItem)Cmb_Group_Sort.SelectedItem).Value.ToString())
            //{
            //    case "ASC": Sel_Group_Sort = "Sort Fields : Ascending"; break;
            //    case "DESC": Sel_Group_Sort = "Sort Fields : Descending"; break;
            //}

            //Sel_Use_CASEDIFF = "Use CASEDIFF : No";
            //if (Cb_Use_DIFF.Checked)
            //    Sel_Use_CASEDIFF = "Use CASEDIFF : Yes";


            //Sel_Include_Members = "Include All Members : No";
            //if (Cb_Inc_Menbers.Checked)
            //    Sel_Include_Members = "Include All Members : Yes";
        }

        private void Btn_Save_Params_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);

                Get_Selection_Criteria();
                //Save_Entity.Scr_Code = PrivilegeEntity.Program;
                Save_Entity.Scr_Code = Scr_Oper_Mode;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = Save_Entity.Card_3 = null;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, PrivilegeEntity);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }

        }

        private string Get_XML_Format_for_Report_Controls()   // 12012012
        {

            string strrptfor = "R";
            if (rdoperiodCumulative.Checked)
                strrptfor = "C";
            if (rdoperiodBoth.Checked)
                strrptfor = "B";

            string strRepControl = "N";
            if (chkRepControl.Checked == true)
                strRepControl = "Y";

            string UndupTable = "N";
            if (chkbUndupTable1.Checked == true)
                UndupTable = "Y";

            //string Year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            /**** string SiteType = string.Empty;
             if (Rb_Site_Sel.Checked == true)
                 SiteType = Search_Entity.Mst_Site;
             else if (rdoMsselectsite.Checked == true)
                 SiteType = Search_Entity.Mst_Site;*/

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                                      "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
                                      "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
                                      "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + /*SiteType*/ Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
                                      "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
                                      "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" ACTY_PROGRAM = \"" + Search_Entity.Activity_Prog +
                                      "\" CAMS_SW = \"" + Search_Entity.CA_MS_Sw + "\" CAMS_Filter_List = \"" + Search_Entity.CAMS_Filter + "\" FUND_Filter_List = \"" + Search_Entity.CA_Fund_Filter + "\" RNG_MAIN_CODE = \"" + Search_Entity.RngMainCode + "\" OUTCOME_SWITCH = \"" + (rdoOutcomesselect.Checked == true ? "S" : "A") + "\" CASEMSSITE = \"" + Search_Entity.CaseMssite + "\" INCVERSWITCH = \"" + Search_Entity.IncVerSwitch + "\" REPORTFOR =\"" + strrptfor + "\" REPORTCONTROL =\"" + strRepControl + "\" UndupTable =\"" + UndupTable + "\" ALERTCODES = \"" + (rdbAllAlertCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />"); //
            //switch (Scr_Oper_Mode)
            //{
            //    case "CASB0004":
            //        str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
            //                                  "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
            //                                  "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
            //                                  "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
            //                                  "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
            //                                  "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" />");
            //        break;

            //    case "RNGB0014":
            //        str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
            //                                  "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
            //                                  "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
            //                                  "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
            //                                  "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
            //                                  "\" COUNTY = \"" + Search_Entity.County + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" />");
            //        break;
            //}
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Btn_Get_Params_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            //Save_Entity.Scr_Code = PrivilegeEntity.Program;
            Save_Entity.Scr_Code = Scr_Oper_Mode;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();

        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                Clear_Error_Providers();

                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);
            }
        }


        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                DataColumnCollection columns = Tmp_Table.Columns;

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), string.Empty);
                fillRngCode();
                SetComboBoxValue(Cmb_CaseType, dr["CASE_TYPE"].ToString());

                switch (dr["CASE_STATUS"].ToString())
                {
                    case "Y": Rb_Stat_Act.Checked = true; break;
                    case "N": Rb_Stat_InAct.Checked = true; break;
                    case "B": Rb_Stat_Both.Checked = true; break;
                    default: Rb_Stat_Act.Checked = true; break;
                }

                Txt_Sel_Site.Clear();
                switch (dr["SITE"].ToString())
                {
                    case "**": Rb_Site_All.Checked = true; break;
                    case "NO": Rb_Site_No.Checked = true; break;
                    default:
                        Rb_Site_Sel.Checked = true; Txt_Sel_Site.Text = dr["SITE"].ToString();
                        Site_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
                        ListcaseSiteEntity.Clear();
                        foreach (CaseSiteEntity casesite in Site_List) //Site_List)//ListcaseSiteEntity)
                        {
                            if (Txt_Sel_Site.Text.Contains(casesite.SiteNUMBER))
                                ListcaseSiteEntity.Add(casesite);
                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                        }


                        break;
                }
                //if (columns.Contains("INCVERSWITCH"))
                //{
                //    chkincverswitch.Checked = true;//dr["INCVERSWITCH"].ToString() == "Y" ? true : false;
                //}
                if (columns.Contains("CASEMSSITE"))
                {
                    txt_Msselect_site.Clear();
                    switch (dr["CASEMSSITE"].ToString())
                    {
                        case "**": rdoMssiteall.Checked = true; break;
                        case "NO": rdomsNosite.Checked = true; break;
                        default:
                            rdoMsselectsite.Checked = true; txt_Msselect_site.Text = dr["CASEMSSITE"].ToString();
                            // MSSite_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
                            ListcaseMsSiteEntity.Clear();
                            foreach (CaseSiteEntity casesite in Site_List) //Site_List)//ListcaseSiteEntity)
                            {
                                if (txt_Msselect_site.Text.Contains(casesite.SiteNUMBER))
                                    ListcaseMsSiteEntity.Add(casesite);
                                // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                            }


                            break;
                    }
                }


                Ref_From_Date.Value = Convert.ToDateTime(dr["REFERENCE_FROM_DATE"].ToString());
                Ref_To_Date.Value = Convert.ToDateTime(dr["REFERENCE_TO_DATE"].ToString());
                Rep_From_Date.Value = Convert.ToDateTime(dr["REPORT_FROM_DATE"].ToString());
                Rep_To_Date.Value = Convert.ToDateTime(dr["REPORT_TO_DATE"].ToString());

                if (columns.Contains("FUND_Filter_List"))
                {
                    Sel_Funding_List.Clear();
                    if (dr["FUND_Filter_List"].ToString().Trim() == "**")
                        Rb_Fund_All.Checked = true;
                    else
                    {
                        GetParam_Fund_Filter_List(dr["FUND_Filter_List"].ToString());
                        this.Rb_Fund_Sel.Click -= new System.EventHandler(this.Rb_Fund_Sel_CheckedChanged);
                        Rb_Fund_Sel.Checked = true;
                        this.Rb_Fund_Sel.Click += new System.EventHandler(this.Rb_Fund_Sel_CheckedChanged);
                    }

                }

                if (columns.Contains("REPORTFOR"))
                {
                    rdoperiod.Checked = true;
                    //if (dr["REPORTCONTROL"].ToString() == "R")
                    //    rdoperiod.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "C")
                        rdoperiodCumulative.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "B")
                        rdoperiodBoth.Checked = true;
                    reportforselection();

                }

                if (dr["DATE_SELECTION"].ToString() == "MSDATE")
                    Rb_MS_Date.Checked = true;
                else
                    Rb_MS_AddDate.Checked = true;

                if ((dr["ATTRIBUTES"].ToString() == "SYSTEM" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["ATTRIBUTES"].ToString() == "All" && Scr_Oper_Mode == "RNGB0014"))
                    Rb_Agy_Def.Checked = true;
                else
                    Rb_User_Def.Checked = true;

                if (dr["STAT_DETAILS"].ToString() == "Y")
                    Rb_Details_Yes.Checked = true;
                else
                    Rb_Details_No.Checked = true;

                Txt_Pov_Low.Text = dr["POVERTY_LOW"].ToString();
                Txt_Pov_High.Text = dr["POVERTY_HIGH"].ToString();


                if ((dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "RNGB0014"))
                    Rb_County_All.Checked = true;
                else
                {
                    Rb_County_Sel.Checked = true;
                    Fill_County_Selected_List(dr["COUNTY"].ToString().Trim());
                }


                //if ((dr["COUNTY"].ToString() == "PM" && Scr_Oper_Mode == "RNGB0014"))
                //    Rb_OBO_Mem.Checked = true;
                //else if ((dr["COUNTY"].ToString() == "Both" && Scr_Oper_Mode == "RNGB0014"))
                //{
                //    Rb_SNP_Mem.Checked = true;
                //}
                //else if ((dr["COUNTY"].ToString() == "Prog" && Scr_Oper_Mode == "RNGB0014"))
                //{
                //    rbo_ProgramWise.Checked = true;
                //}


                if (dr["DG_COUNT"].ToString() == "PM")
                    Rb_OBO_Mem.Checked = true;
                else if (dr["DG_COUNT"].ToString() == "Prog")
                    rbo_ProgramWise1.Checked = true;
                else
                    Rb_SNP_Mem.Checked = true;

                if (Rb_OBO_Mem.Checked)
                {
                    //**if(rdoperiod.Checked)
                        chkbUndupTable1.Visible = true;
                    spacerr.Visible = true;
                    rdbSumDetail.Visible = false;
                    rbo_ProgramWise1.Visible = false;

                    if (dr["UndupTable"].ToString() == "Y")
                        chkbUndupTable1.Checked = true;
                    else
                        chkbUndupTable1.Checked = false;
                }


                switch (dr["SECRET_APP"].ToString())
                {
                    case "Y": Rb_Mst_Sec.Checked = true; break;
                    case "N": Rb_Mst_NonSec.Checked = true; break;
                    default: Rb_Mst_BothSec.Checked = true; break;
                }


                //Fill_Program_Combo();
                //SetComboBoxValue(Cmb_Program, dr["ACTY_PROGRAM"].ToString().Trim() == "******" ? "**" : dr["ACTY_PROGRAM"].ToString());

                if (dr["ACTY_PROGRAM"].ToString() != "A")
                {

                    rbAllPrograms.Checked = true;
                    //Btn_MS_Selection.Visible = false;
                    GetSelectedHierarchies(dr["ACTY_PROGRAM"].ToString());
                    if (SelectedHierarchies.Count > 0)
                    {
                        rbSelProgram.Checked = true;
                        //All_CAMS_Selected = false;
                        //Btn_CA_Selection.Text = "Sel";
                    }
                }

                SetComboBoxValue(cmbRngCode, dr["RNG_MAIN_CODE"].ToString().Trim());

                Ref_From_Date.Value = Convert.ToDateTime(dr["REFERENCE_FROM_DATE"].ToString());
                Ref_To_Date.Value = Convert.ToDateTime(dr["REFERENCE_TO_DATE"].ToString());

                if (dr["ZIPCODE"].ToString() == "**")
                    Rb_Zip_All.Checked = true;
                else
                {
                    Rb_Zip_Sel.Checked = true;
                    if (Scr_Oper_Mode == "RNGB0014")
                        Prepare_Selected_Group(dr["ZIPCODE"].ToString());
                }
                if (columns.Contains("OUTCOME_SWITCH"))
                {
                    if (dr["OUTCOME_SWITCH"].ToString() == "S")
                        rdoOutcomesselect.Checked = true;
                    else
                        rdoOutcomesAll.Checked = true;
                }

               
                if (columns.Contains("REPORTCONTROL"))
                {
                    if (dr["REPORTCONTROL"].ToString() == "Y")
                        chkRepControl.Checked = true;
                    else
                        chkRepControl.Checked = false;
                }

                if (columns.Contains("ALERTCODES"))
                {
                    if (dr["ALERTCODES"].ToString() == "A")
                        rdbAllAlertCds.Checked = true;
                    else if (dr["ALERTCODES"].ToString() == "S")
                        rdbSelAlertCds.Checked = true;
                }
                else
                    rdbAllAlertCds.Checked = true;

                if (columns.Contains("SELECTEDALERTCODES"))
                {
                    if (dr["SELECTEDALERTCODES"].ToString() != "")
                    {
                        Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                    }
                }

                Get_Selection_Criteria();
            }
        }

        private void Fill_County_Selected_List(string County_Str)
        {
            foreach (CommonEntity Ent in County_List)
            {
                if (County_Str.Contains("'" + Ent.Code + "'"))
                    ListcommonEntity.Add(new CommonEntity(Ent));
            }
        }



        private void GetParam_Fund_Filter_List(string Sel_List)
        {
            if (!string.IsNullOrEmpty(Sel_List.Trim()))
            {
                Sel_List = Sel_List + ",";
                Sel_Funding_List.Clear();
                foreach (SPCommonEntity Entity in Fund_Mast_List)
                {
                    if (Sel_List.Contains("'" + Entity.Code.Trim() + "' ,"))
                        Sel_Funding_List.Add(new SPCommonEntity(Entity));
                }
            }
        }



        private void GetParam_MS_Filter_List(string Sel_List)
        {
            if (!string.IsNullOrEmpty(Sel_List.Trim()))
            {
                Sel_List = Sel_List + ",";
                Sel_MS_List.Clear();
                foreach (MSMASTEntity Entity in MS_Mast_List)
                {
                    if (Sel_List.Contains("'" + Entity.Code.Trim() + "' ,"))
                        Sel_MS_List.Add(new MSMASTEntity(Entity));
                }
            }
        }

        private void GetParam_CA_Filter_List(string Sel_List)
        {
            if (!string.IsNullOrEmpty(Sel_List.Trim()))
            {
                Sel_List = Sel_List + ",";
                Sel_CA_List.Clear();
                foreach (CAMASTEntity Entity in CA_Mast_List)
                {
                    if (Sel_List.Contains("'" + Entity.Code.Trim() + "' ,"))
                        Sel_CA_List.Add(new CAMASTEntity(Entity));
                }
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value) || value == " ")
                value = "0";
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (Captain.Common.Utilities.ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    { comboBox.SelectedItem = li; break; }
                }
            }
        }

        private void Txt_Pov_High_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_Pov_High.Text.Trim()))
                Txt_Pov_High.Text = "000";
        }

        private void Txt_Pov_Low_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_Pov_Low.Text.Trim()))
                Txt_Pov_Low.Text = "000";
        }


        private void Rb_Site_All_Click(object sender, EventArgs e)
        {
            Txt_Sel_Site.Clear();
            ListcaseSiteEntity.Clear();
        }

        private void Rb_Site_No_CheckedChanged(object sender, EventArgs e)
        {
            Txt_Sel_Site.Clear();
            ListcaseSiteEntity.Clear();
        }

        private void Fill_Program_Combo()
        {
            Cmb_Program.Items.Clear();


            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "3", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), " "); // Verify it Once
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");

            bool All_Agy = false, All_Dept = false;

            if (Current_Hierarchy.Substring(0, 2) == "**")
                All_Agy = true;

            if (Current_Hierarchy.Substring(2, 2) == "**")
                All_Dept = true;

            string Tmp_Hierarchy = string.Empty;
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            int TmpRows = 0, ProgIndex = 0;
            listItem.Add(new Captain.Common.Utilities.ListItem("All Programs", "**"));
            foreach (HierarchyEntity Ent in caseHierarchy)
            {
                if ((All_Agy || (!All_Agy && Ent.Agency == Current_Hierarchy.Substring(0, 2))) &&
                    (All_Dept || (!All_Dept && Ent.Dept == Current_Hierarchy.Substring(2, 2))))
                {
                    Tmp_Hierarchy = Ent.Agency + Ent.Dept + Ent.Prog;

                    if (!Ent.Code.Contains("**"))
                    {
                        listItem.Add(new Captain.Common.Utilities.ListItem(Ent.Prog + " - " + Ent.HirarchyName, Tmp_Hierarchy));
                        if (Current_Hierarchy == Tmp_Hierarchy)
                        {
                            ProgIndex = TmpRows; DefHieExist = true;
                        }
                        TmpRows++;
                    }
                }
            }

            if (TmpRows > 0)
            {
                Cmb_Program.Items.AddRange(listItem.ToArray());
                Cmb_Program.Enabled = true;
                if (DefHieExist)
                    Cmb_Program.SelectedIndex = (ProgIndex + 1);
                else
                {
                    if (Cmb_Program.Items.Count > 1)
                        Cmb_Program.SelectedIndex = 0;
                }
            }
            else
                AlertBox.Show("Programs are not Defined", MessageBoxIcon.Warning);
        }


        string Scr_Oper_Mode = "RNGB0014";




        private void Initialize_All_Controls()
        {


            Scr_Oper_Mode = "RNGB0014";
            //20160303

            this.Text = "RNGB0014 - Performance Measures";

            lblDateSelection.Text = "Process Report By";
            lblAttributes.Text = "Categories";
            Rb_Agy_Def.Text = "All";
            Rb_User_Def.Text = "Only Goal Associated";

            lblProduceStatistical.Text = "Produce Details";

            Rb_Details_No.Text = "No";
            Rb_Details_Yes.Text = "Yes";

            lblZipCodes.Text = "Group";
            Rb_Zip_All.Text = "All Groups";
            Rb_Zip_Sel.Text = "Selected Groups";

            //lblCounty.Text = "Report Format";
            //Rb_County_All.Text = "Performance Measures Only";
            //Rb_County_Sel.Text = "Performance Measures + Goal Details";

            Rb_MS_Date.Text = "Outcome Date";//"Milestone Date";
            Rb_MS_AddDate.Text = "Outcome Add Date";//"Milestone Add Date";

            lblDemographicsCount.Text = "Report Format";
            Rb_OBO_Mem.Text = "Performance Measures Only";
            Rb_SNP_Mem.Text = "Performance Measures + Goal Details";
            rbo_ProgramWise1.Text = "Program Level Counts";
            Rb_SNP_Mem.Location = new System.Drawing.Point(154, 2);


            Lbl_Program.Visible = true; //Cmb_Program.Visible = true;
            Lbl_Program.Location = new System.Drawing.Point(5, 36);
            //Cmb_Program.Location = new System.Drawing.Point(143, 36);
            //this.Cmb_Program.Size = new System.Drawing.Size(249, 21);

            this.Date_Panel.Location = new System.Drawing.Point(0, -1);
            this.Service_Panel.Size = new System.Drawing.Size(450, 21);

            //this.panel8.Location = new System.Drawing.Point(0, 48);


            this.pnlfewrdb.Location = new System.Drawing.Point(0, 48);
            //this.panel8.Size = new System.Drawing.Size(607, 224); //20160303
            this.pnlfewrdb.Size = new System.Drawing.Size(607, 247);

            //this.panel4.Location = new System.Drawing.Point(-1, 33);
            this.pnlMultiRdb.Location = new System.Drawing.Point(-1, 55);

            Rb_SNP_Mem.Size = new System.Drawing.Size(230, 21);

            this.pnlMultiRdb.Size = new System.Drawing.Size(607, 296);
            //this.panel3.Location = new System.Drawing.Point(4, 385); //20160303
            //this.panel2.Size = new System.Drawing.Size(607, 352); //20160303

            //this.Size = new System.Drawing.Size(613, 395);

            //this.Size = new System.Drawing.Size(615, 422);
            //Fill_Program_Combo();


            Cmb_CaseType.SelectedIndex = 0;
            Rb_MS_Date.Checked = Rb_Stat_Both.Checked = Rb_Site_All.Checked = rdoMssiteall.Checked =
            Rb_Details_No.Checked = Rb_Zip_All.Checked = Rb_County_All.Checked =
            Rb_Mst_NonSec.Checked = true;

            All_CAMS_Selected = true;
            Sel_MS_List.Clear();
            Sel_CA_List.Clear();

            switch (Scr_Oper_Mode)
            {
                case "RNGB0004":
                    Rb_User_Def.Checked = Rb_OBO_Mem.Checked = true;
                    Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                    Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    break;
                case "RNGB0014":
                    Rb_Agy_Def.Checked = true; rbAllPrograms.Checked = true; //Fill_Program_Combo();
                    HierarchyGrid.Rows.Clear(); SelectedHierarchies.Clear();
                    if (Sys_DateRange_List.Count > 0)
                    {
                        Ref_From_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_FDATE);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                        Ref_To_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_TDATE);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    }
                    else
                    {
                        Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                        Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    }
                    break;
            }

            Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));

            Ref_From_Date.Checked = Ref_To_Date.Checked = Rep_From_Date.Checked = Rep_To_Date.Checked = true;

            Txt_Pov_Low.Text = "0"; Txt_Pov_High.Text = "999";


            ListcaseSiteEntity.Clear();
            ListcaseMsSiteEntity.Clear();
            ListcommonEntity.Clear();
            ListZipCode.Clear();
            ListRngGroupCode.Clear();
            Clear_Error_Providers();
        }

        private void Clear_Error_Providers()
        {
            _errorProvider.SetError(Ref_To_Date, null);
            _errorProvider.SetError(Rb_Zip_Sel, null);
            _errorProvider.SetError(Rb_County_Sel, null);
            _errorProvider.SetError(Txt_Sel_Site, null);
            _errorProvider.SetError(txt_Msselect_site, null);
            _errorProvider.SetError(Txt_Pov_Low, null);
            _errorProvider.SetError(Txt_Pov_High, null);
        }


        private void Rb_Zip_All_CheckedChanged(object sender, EventArgs e)
        {
            switch (Scr_Oper_Mode)
            {
                case "RNGB0004": ListZipCode.Clear(); break;
                case "RNGB0014": ListRngGroupCode.Clear(); break;
            }
        }

        private void Rb_County_All_CheckedChanged(object sender, EventArgs e)
        {
            ListcommonEntity.Clear();
        }



        bool All_CAMS_Selected = true;
        List<MSMASTEntity> Sel_MS_List = new List<MSMASTEntity>();
        List<CAMASTEntity> Sel_CA_List = new List<CAMASTEntity>();


        private void Rb_Site_Sel_CheckedChanged(object sender, EventArgs e)
        {

        }


        List<MSMASTEntity> MS_Mast_List = new List<MSMASTEntity>();
        List<CAMASTEntity> CA_Mast_List = new List<CAMASTEntity>();
        private void Fill_CAMS_Master_List()
        {
            MS_Mast_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            CA_Mast_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
        }

        private void rdoMssiteall_Click(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            ListcaseMsSiteEntity.Clear();
        }

        private void rdoMsselectsite_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoMsselectsite_Click(object sender, EventArgs e)
        {
            if (rdoMsselectsite.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseMsSiteEntity, strAgency, strDept, strProg, string.Empty);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyMSFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }

        private void rdomsNosite_CheckedChanged(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            ListcaseMsSiteEntity.Clear();
        }

        //private void Btn_MS_Selection_Click(object sender, EventArgs e)
        //{
        //    CASE0010_HSS_Form MS_Selection_Form = new CASE0010_HSS_Form(PrivilegeEntity.Program, "MS", All_CAMS_Selected, Sel_MS_List, Sel_CA_List);
        //    MS_Selection_Form.FormClosed += new FormClosedEventHandler(Get_Sel_MS_List);
        //    MS_Selection_Form.StartPosition = FormStartPosition.CenterScreen;
        //    MS_Selection_Form.ShowDialog();
        //}

        private void Rb_Fund_Sel_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Fund_Sel.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, Sel_Funding_List, PrivilegeEntity.Program, strAgency,strDept,strProg,null, PrivilegeEntity.UserID);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }

        private void Get_Sel_MS_List(object sender, FormClosedEventArgs e)
        {
            CASE0010_HSS_Form form = sender as CASE0010_HSS_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                List<MSMASTEntity> MS_List = new List<MSMASTEntity>();
                MS_List = form.Get_Sel_MS_List();
                Sel_MS_List.Clear();
                All_CAMS_Selected = true;
                if (MS_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_MS_List = MS_List;
                }
            }
        }

        private void rbSelProgram_Click(object sender, EventArgs e)
        {
            if (rbSelProgram.Checked == true)
            {
                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Service", "I", "A", "R", PrivilegeEntity, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnProgramClosed);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();

                //HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, SelectedHierarchies, "Service", "I", "A", "R", PrivilegeEntity, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2));
                //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnProgramClosed);
                //hierarchieSelectionForm.ShowDialog();
            }
        }

        private void OnProgramClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {

                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;

                Fill_Programs_Grid(selectedHierarchies);


            }
        }

        private void Fill_Programs_Grid(List<HierarchyEntity> selectedHierarchies)
        {
            string hierarchy = string.Empty;
            int Rows_Cnt = 0;
            // HierarchyGrid.Rows.Clear();
            HierarchyGrid.Rows.Clear();
            if (selectedHierarchies.Count > 0)
            {
                HierarchyGrid.Rows.Clear();
                string Agy = "**", Dept = "**", Prog = "**";
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    Agy = Dept = Prog = "**";
                    if (!string.IsNullOrEmpty(row.Agency.Trim()))
                        Agy = row.Agency.Trim();

                    if (!string.IsNullOrEmpty(row.Prog.Trim()))
                        Prog = row.Prog.Trim();

                    if (!string.IsNullOrEmpty(row.Dept.Trim()))
                        Dept = row.Dept.Trim();

                    int rowIndex = HierarchyGrid.Rows.Add(row.Code + "  " + row.HirarchyName.ToString(), Agy + Dept + Prog);
                    HierarchyGrid.Rows[rowIndex].Tag = row;
                    Rows_Cnt++;

                    //hierarchy += row.Agency + row.Dept + row.Prog;
                    hierarchy += row.Code.Substring(0, 2) + row.Code.Substring(3, 2) + row.Code.Substring(6, 2) + ", ";
                }

                //if (Rows_Cnt > 0)
                //    Txt_Program.Text = hierarchy.Substring(0, hierarchy.Length - 2);
            }
        }

        //Added by Sudheer on 07/22/2022
        private void GetSelectedHierarchies(string Sel_List)
        {
            List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetHierarchyByUserID(BaseForm.UserProfile.UserID, "I", "Reports");
            List<HierarchyEntity> ListofSelHiers = new List<HierarchyEntity>();
            if (caseHierarchy.Count > 0)
            {
                //caseHierarchy= caseHierarchy.FindAll(u=>u.Agency)

                if (!string.IsNullOrEmpty(Sel_List.Trim()))
                {
                    Sel_List = Sel_List + ",";

                    HierarchyGrid.Rows.Clear();
                    foreach (HierarchyEntity Entity in caseHierarchy)
                    {
                        if (!Entity.Code.Contains('*'))
                        {
                            if (Sel_List.Contains(Entity.Agency + Entity.Dept + Entity.Prog.Trim()))
                            {
                                int rowIndex = HierarchyGrid.Rows.Add(Entity.Code + "  " + Entity.HirarchyName.ToString(), Entity.Agency + Entity.Dept + Entity.Prog);
                                HierarchyGrid.Rows[rowIndex].Tag = Entity;

                            }
                        }
                    }
                }
            }
        }

        private void RNGB0014Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void Rb_SNP_Mem_Click(object sender, EventArgs e)
        {
            chkbUndupTable1.Checked = false;
            chkbUndupTable1.Visible = false;
        }

        private void Rb_OBO_Mem_Click(object sender, EventArgs e)
        {
            if (rdoperiodBoth.Checked)
            {
                chkbUndupTable1.Visible = true;

                //chkbUndupTable1.Checked = false;
                //chkbUndupTable1.Visible = false;
            }
            else if (rdoperiod.Checked)
                chkbUndupTable1.Visible = true;
        }

        private void rbAllPrograms_Click(object sender, EventArgs e)
        {
            if (rbAllPrograms.Checked == true)
            {
                HierarchyGrid.Rows.Clear();
                SelectedHierarchies.Clear();
            }
        }

        private void Get_Sel_CA_List(object sender, FormClosedEventArgs e)
        {
            CASE0010_HSS_Form form = sender as CASE0010_HSS_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                List<CAMASTEntity> CA_List = new List<CAMASTEntity>();
                CA_List = form.Get_Sel_CA_List();
                Sel_CA_List.Clear();
                All_CAMS_Selected = true;
                if (CA_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_CA_List = CA_List;
                }
            }
        }


        //private void Btn_CA_Selection_Click(object sender, EventArgs e)
        //{
        //    CASE0010_HSS_Form CA_Selection_Form = new CASE0010_HSS_Form(PrivilegeEntity.Program, "CA", All_CAMS_Selected, Sel_MS_List, Sel_CA_List);
        //    CA_Selection_Form.FormClosed += new FormClosedEventHandler(Get_Sel_CA_List);
        //    CA_Selection_Form.StartPosition = FormStartPosition.CenterScreen;
        //    CA_Selection_Form.ShowDialog();
        //}


        private void cmbRngCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRngCode.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() != "**")
                {

                    RCsb14GroupEntity rngcodedata = RngCodelist.Find(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                    if (rngcodedata != null)
                    {
                        ListRngGroupCode.Clear();
                        if (rngcodedata.OFdate != string.Empty)
                        {
                            Ref_From_Date.Value = Convert.ToDateTime(rngcodedata.OFdate);
                            Ref_From_Date.Checked = true;
                        }
                        if (rngcodedata.OTdate != string.Empty)
                        {
                            Ref_To_Date.Checked = true;
                            Ref_To_Date.Value = Convert.ToDateTime(rngcodedata.OTdate);
                        }
                    }
                }
            }
        }


        public string HeadersValue(int intcolumnscount)
        {
            string strValue = string.Empty;
            switch (intcolumnscount)
            {
                case 4:
                    strValue = "IV.) ";
                    break;
                case 5:
                    strValue = "V.) ";
                    break;
                case 6:
                    strValue = "VI.) ";
                    break;
                case 7:
                    strValue = "VII.) ";
                    break;
                case 8:
                    strValue = "VIII.) ";
                    break;
                case 9:
                    strValue = "IX.) ";
                    break;
                case 10:
                    strValue = "X.) ";
                    break;
                default:
                    break;
            }
            return strValue;
        }

        public string HeadersValue1(int intcolumnscount)
        {
            string strValue = string.Empty;
            switch (intcolumnscount)
            {
                case 4:
                    strValue = "IV";
                    break;
                case 5:
                    strValue = "V";
                    break;
                case 6:
                    strValue = "VI";
                    break;
                case 7:
                    strValue = "VII";
                    break;
                case 8:
                    strValue = "VIII";
                    break;
                case 9:
                    strValue = "IX";
                    break;
                case 10:
                    strValue = "X";
                    break;
                default:
                    break;
            }
            return strValue;
        }

        private void rdoperiodBoth_Click(object sender, EventArgs e)
        {
            reportforselection();

            //if (rdoperiod.Checked)
            //{
            //    Rb_SNP_Mem.Enabled = true;
            //}
            //else
            //{
            //    Rb_SNP_Mem.Enabled = false;
            //    Rb_SNP_Mem.Checked = false;
            //    Rb_OBO_Mem.Checked = true;
            //}

        }

        private void reportforselection()
        {
            //if (rdoperiodBoth.Checked)
            //{
            //    // Rb_SNP_Mem.Enabled = false;
            //    Rb_SNP_Mem.Checked = false;
            //    Rb_OBO_Mem.Checked = true;
            //    //**Rb_OBO_Mem_Click(this, new EventArgs() { });    // Added by Vikash on 07/10/2023
            //}
            //else
            //{
            //    Rb_SNP_Mem.Enabled = true;
            //    //**Rb_SNP_Mem_Click(this, new EventArgs() { });
            //}
            if (rdoperiod.Checked == true)
            {
                Ref_From_Date.Enabled = false;
                Ref_To_Date.Enabled = false;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                chkbUndupTable1.Enabled = true;
                if (Rb_OBO_Mem.Checked)
                    chkbUndupTable1.Visible = true;

                chkRepControl.Visible = false; chkRepControl.Checked = false;

                pnlRefPeriod.Visible = false;
                this.Size = new Size(this.Width, 657);
                this.pnlbtns.Size = new Size(Width, 522);
            }
            else if (rdoperiodCumulative.Checked == true)
            {
                Ref_From_Date.Enabled = true;
                Ref_To_Date.Enabled = true;
                Rep_From_Date.Enabled = false;
                Rep_To_Date.Enabled = false;

                chkbUndupTable1.Enabled = true;
            }
            else if (rdoperiodBoth.Checked == true)
            {
                Ref_From_Date.Enabled = true;
                Ref_To_Date.Enabled = true;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                if (Rb_OBO_Mem.Checked)
                    chkbUndupTable1.Visible = true;

                //chkbUndupTable1.Enabled = false;
                //chkbUndupTable1.Checked = false;
                //chkbUndupTable1.Visible = false;

                chkRepControl.Visible = true;

                pnlRefPeriod.Visible = true;
                this.Size = new Size(this.Width, 682);
                this.pnlbtns.Size = new Size(Width, 547);
            }

        }

        private void btnMergeExcelView_Click(object sender, EventArgs e)
        {
            PdfListForm pdfMergeListForm = new PdfListForm(BaseForm);
            pdfMergeListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfMergeListForm.ShowDialog();
        }

        private string Get_Sel_CA_Fund_List_To_Filter()
        {
            string Sel_Codes = null;

            if (Rb_Fund_Sel.Checked)
            {
                foreach (SPCommonEntity Entity in Sel_Funding_List)
                {
                    Sel_Codes += "'" + Entity.Code + "' ,";
                }

                if (Sel_Codes.Length > 0)
                    Sel_Codes = Sel_Codes.Substring(0, (Sel_Codes.Length - 1));
            }

            return Sel_Codes;
        }

        private void rdbAllAlertCds_Click(object sender, EventArgs e)
        {
            SelectedAlertCodes = string.Empty;
            _errorProvider.SetError(rdbSelAlertCds, null);
        }

        private void rdbSelAlertCds_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(BaseForm, PrivilegeEntity, SelectedAlertCodes);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            objform.ShowDialog();
        }
        string SelectedAlertCodes = string.Empty;
        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelectedAlertCodes = form.fillAlertCodesforReport();
            }
            else
                form.Close();

        }

        private string Get_Sel_AlertCodes(string alertCodes)
        {
            SelectedAlertCodes = alertCodes;

            return SelectedAlertCodes;
        }
    }
}