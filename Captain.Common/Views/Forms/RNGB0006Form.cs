
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
using ListItem = Captain.Common.Utilities.ListItem;
using DevExpress.XtraPrinting.Design;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Web.Internal.XmlProcessor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNGB0006Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;


        #endregion

        public RNGB0006Form(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            ListZipCode = new List<ZipCodeEntity>();
            ListRngGroupCode = new List<RCsb14GroupEntity>();
            ListcaseSiteEntity = new List<CaseSiteEntity>();
            ListcaseMsSiteEntity = new List<CaseSiteEntity>();
            Sel_Funding_List = new List<SPCommonEntity>();
            ListcommonEntity = new List<CommonEntity>();
            ListServicePlans = new List<CASESP1Entity>();

            InitializeComponent();
            BaseForm = baseForm;
            PrivilegeEntity = privilegeEntity;
            this.Text = "PPR Report";
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


            //Initialize_All_Controls();
        }

        void fillRngCode()
        {
            cmbRngCode.Items.Clear();
            RngCodelist = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
            List<RCsb14GroupEntity> rngonlycodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty);

            List<RCsb14GroupEntity> rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals(strAgency));
            if (rngonlyAgencylist.Count == 0)
            {
                rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals("**"));
            }


            if (rngonlyAgencylist.Count > 0)
            {
                rngonlyAgencylist = rngonlyAgencylist.FindAll(u => u.PPR_SW == "Y");
                rngonlyAgencylist = rngonlyAgencylist.OrderByDescending(u => u.Code).ToList();
            }

            foreach (RCsb14GroupEntity rngcodedata in rngonlyAgencylist)
            {
                cmbRngCode.Items.Add(new Captain.Common.Utilities.ListItem(rngcodedata.GrpDesc, rngcodedata.Code, rngcodedata.Agency,string.Empty, rngcodedata.OFdate,rngcodedata.OTdate));
            }
            cmbRngCode.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "**"));
            cmbRngCode.SelectedIndex = 0;
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

           /** Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
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
            Rep_From_Date.Checked = Rep_To_Date.Checked = true;*/


        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegeEntity { get; set; }

        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        public List<SPCommonEntity> Sel_Funding_List { get; set; }

        public List<ZipCodeEntity> ListZipCode { get; set; }

        public List<RCsb14GroupEntity> ListRngGroupCode { get; set; }

        public List<CASESP1Entity> ListServicePlans { get; set; }

        public List<CommonEntity> ListcommonEntity { get; set; }

        public List<CaseSiteEntity> ListcaseMsSiteEntity { get; set; }

        public string strAgency { get; set; }

        public string strDept { get; set; }

        public string strProg { get; set; }

        public string ReportPath { get; set; }

        public string propReportPath { get; set; }

        /**public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                //return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                //                               where (((DataGridViewCheckBoxCell)c.Cells["Select"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                //                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();

                return _selectedHierarchies = (from c in HierarchyGrid.Rows.Cast<DataGridViewRow>().ToList()
                                               select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            }
        }*/

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

        /**private void rdoSelectedSites_Click(object sender, EventArgs e)
        {
            if (Rb_Site_Sel.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseSiteEntity, strAgency, strDept, strProg, string.Empty);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }*/


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
            if (Rb_SP_Sel.Checked == true)
            {
                //if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() == "**")
                //{
                //    AlertBox.Show("Please Select Code", MessageBoxIcon.Warning);
                //}
                //else
                //{

                    SelectZipSiteCountyForm zipcodeform1 = new SelectZipSiteCountyForm(BaseForm, ListServicePlans, Current_Hierarchy.Substring(0,2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
                    zipcodeform1.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                    zipcodeform1.StartPosition = FormStartPosition.CenterScreen;
                    zipcodeform1.ShowDialog();
                //}

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
                    /**if (Rb_Site_Sel.Checked == true && ListcaseSiteEntity.Count > 0)
                        Txt_Sel_Site.Text = ListcaseSiteEntity[0].SiteNUMBER.ToString();
                    else
                        Txt_Sel_Site.Clear();*/
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
                if (form.FormType == "CASESP")
                {
                    ListServicePlans = form.SelectedServicePlanEntity;
                    
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

       /** private void SelectZipSiteCountyMSFormClosed(object sender, FormClosedEventArgs e)
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
        }*/



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
                this.txtHieDesc.Size = new System.Drawing.Size(666/*810*/, 25);
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
                this.txtHieDesc.Size = new System.Drawing.Size(601/*753*/, 25);
            }
            else
                this.txtHieDesc.Size = new System.Drawing.Size(666/*810*/, 25);
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
                   //** rbAllPrograms.Checked = true;
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


        
        DataTable dt;
     

        #region PdfReportCode



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

        private void GenerateWorksheetParameters(WorksheetCollection sheets)
        {

            Worksheet sheet = sheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 14.4F;

            WorksheetColumn columnHead = sheet.Table.Columns.Add();
            columnHead.Index = 2;
            columnHead.Width = 5;
            sheet.Table.Columns.Add(163);
            WorksheetColumn column2Head = sheet.Table.Columns.Add();
            column2Head.Width = 332;
            column2Head.StyleID = "s172H";
            sheet.Table.Columns.Add(59);
            // -----------------------------------------------
            WorksheetRow RowHead = sheet.Table.Rows.Add();
            WorksheetCell cell;
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s170H";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            // -----------------------------------------------
            WorksheetRow Row1Head = sheet.Table.Rows.Add();
            Row1Head.Height = 12;
            Row1Head.AutoFitHeight = false;
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s140";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s141";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s171H";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s142";
            // -----------------------------------------------
            WorksheetRow Row2Head = sheet.Table.Rows.Add();
            Row2Head.Height = 12;
            Row2Head.AutoFitHeight = false;
            cell = Row2Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row2Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row2Head.Cells.Add();
            cell.StyleID = "m2611536909264";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row3Head = sheet.Table.Rows.Add();
            Row3Head.Height = 12;
            Row3Head.AutoFitHeight = false;
            cell = Row3Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row3Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row3Head.Cells.Add();
            cell.StyleID = "m2611536909284";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Selected Report Parameters";
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row4Head = sheet.Table.Rows.Add();
            Row4Head.Height = 12;
            Row4Head.AutoFitHeight = false;
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s145w";
            // -----------------------------------------------
            WorksheetRow Row5Head = sheet.Table.Rows.Add();
            Row5Head.Height = 12;
            Row5Head.AutoFitHeight = false;
            cell = Row5Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row5Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row5Head.Cells.Add();
            cell.StyleID = "m2611536909304";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row6Head = sheet.Table.Rows.Add();
            Row6Head.Height = 12;
            Row6Head.AutoFitHeight = false;
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s145w";
            // -----------------------------------------------


            //**string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            WorksheetRow Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + txtHieDesc.Text.Trim();//** + "     " + Header_year;
            cell.MergeAcross = 2;
            // -----------------------------------------------

            string caseType = string.Empty;
            caseType = ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString().Trim();


            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblCaseType.Text.Trim() + ": " + caseType;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblServiceplan.Text + ": " + (Rb_SP_All.Checked ? "All" : Rb_SP_Sel.Text);
            cell.MergeAcross = 2;


            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            string caseStatus = Rb_Stat_Both.Text;

            if (Rb_Stat_Act.Checked == true)
                caseStatus = Rb_Stat_Act.Text;
            else
                caseStatus = Rb_Stat_InAct.Text;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblCaseStatus.Text + ": " + caseStatus;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblDateSelection.Text.Trim() + ": " + (Rb_MS_Date.Checked == true ? Rb_MS_Date.Text : Rb_MS_AddDate.Text);
            cell.MergeAcross = 2;


            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblRepDesc.Text.Trim() + ": " + ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Text.ToString().Trim();
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblQ1.Text.Trim() + ": " + dtpQ1S.Text + " - " + dtpQ1E.Text;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblQ2.Text.Trim() + ": " + dtpQ2S.Text + " - " + dtpQ2E.Text;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblQ3.Text.Trim() + ": " + dtpQ3S.Text + " - " + dtpQ3E.Text;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblQ4.Text.Trim() + ": " + dtpQ4S.Text + " - " + dtpQ4E.Text;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;


            //Row7Head = sheet.Table.Rows.Add();
            //Row7Head.Height = 12;
            //Row7Head.AutoFitHeight = false;
            //cell = Row7Head.Cells.Add();
            //cell.StyleID = "s139";
            //cell = Row7Head.Cells.Add();
            //cell.StyleID = "s143";
            //cell = Row7Head.Cells.Add();
            //cell.StyleID = "m2611536909324";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "            " + lblAttributes.Text.Trim() + ": " + (Rb_Agy_Def.Checked == true ? Rb_Agy_Def.Text : Rb_User_Def.Text);
            //cell.MergeAcross = 2;


            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblProduceStatistical.Text.Trim() + ": " + (Rb_Details_No.Checked == true ? Rb_Details_No.Text : Rb_Details_Yes.Text);
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblPovertyLevel.Text.Trim() + ": " + lblLow.Text + ": " + Txt_Pov_Low.Text + "      " + lblHigh.Text + ": " + Txt_Pov_High.Text;
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

            if (chkincverswitch.Checked)
            { 
                Row7Head = sheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143w";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            " + chkincverswitch.Text;
                cell.MergeAcross = 2;

                Row8 = sheet.Table.Rows.Add();
                Row8.Height = 12;
                Row8.AutoFitHeight = false;
                cell = Row8.Cells.Add();
                cell.StyleID = "s139";
                cell = Row8.Cells.Add();
                cell.StyleID = "s143w";
                cell = Row8.Cells.Add();
                cell.StyleID = "m2611536909344";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2; 
            }

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblCounty.Text.Trim() + " : " + (Rb_County_All.Checked ? "All" : Rb_County_Sel.Text);
            cell.MergeAcross = 2;

            Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s139";
            cell = Row8.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row8.Cells.Add();
            cell.StyleID = "m2611536909344";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;

           /* if (chkbUndupTable.Checked)
            {
                Row7Head = sheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            " + chkbUndupTable.Text;
                cell.MergeAcross = 2;

                Row8 = sheet.Table.Rows.Add();
                Row8.Height = 12;
                Row8.AutoFitHeight = false;
                cell = Row8.Cells.Add();
                cell.StyleID = "s139";
                cell = Row8.Cells.Add();
                cell.StyleID = "s143";
                cell = Row8.Cells.Add();
                cell.StyleID = "m2611536909344";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
            }*/

            string secretApp = Rb_Mst_NonSec.Text;

            if (Rb_Mst_Sec.Checked == true)
                caseStatus = Rb_Mst_Sec.Text;
            else
                caseStatus = Rb_Mst_BothSec.Text;

            Row7Head = sheet.Table.Rows.Add();
            Row7Head.Height = 12;
            Row7Head.AutoFitHeight = false;
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row7Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            " + lblSecretApplications.Text.Trim() + " : " + secretApp;
            cell.MergeAcross = 2;

            // -----------------------------------------------
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            Row24.Height = 12;
            Row24.AutoFitHeight = false;
            cell = Row24.Cells.Add();
            cell.StyleID = "s139";
            cell = Row24.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row24.Cells.Add();
            cell.StyleID = "m2611540549632";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row25 = sheet.Table.Rows.Add();
            Row25.Height = 12;
            Row25.AutoFitHeight = false;
            cell = Row25.Cells.Add();
            cell.StyleID = "s139";
            cell = Row25.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row25.Cells.Add();
            cell.StyleID = "m2611540549652";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row26Head = sheet.Table.Rows.Add();
            Row26Head.Height = 12;
            Row26Head.AutoFitHeight = false;
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s145w";
            // -----------------------------------------------
            WorksheetRow Row27Head = sheet.Table.Rows.Add();
            Row27Head.Height = 12;
            Row27Head.AutoFitHeight = false;
            cell = Row27Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row27Head.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row27Head.Cells.Add();
            cell.StyleID = "m2611540549672";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row28 = sheet.Table.Rows.Add();
            Row28.Height = 12;
            Row28.AutoFitHeight = false;
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row28.Cells.Add();
            cell.StyleID = "s145w";
            // -----------------------------------------------
            WorksheetRow Row29 = sheet.Table.Rows.Add();
            Row29.Height = 12;
            Row29.AutoFitHeight = false;
            cell = Row29.Cells.Add();
            cell.StyleID = "s139";
            cell = Row29.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row29.Cells.Add();
            cell.StyleID = "m2611540549552";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row30 = sheet.Table.Rows.Add();
            Row30.Height = 12;
            Row30.AutoFitHeight = false;
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s143w";
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row30.Cells.Add();
            cell.StyleID = "s145w";
            // -----------------------------------------------
            WorksheetRow Row31 = sheet.Table.Rows.Add();
            Row31.Height = 12;
            Row31.AutoFitHeight = false;
            cell = Row31.Cells.Add();
            cell.StyleID = "s139";
            cell = Row31.Cells.Add();
            cell.StyleID = "s146";
            cell = Row31.Cells.Add();
            cell.StyleID = "m2611540549572";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.Selected = true;
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;
        }

       
        #endregion


       

        List<Csb16DTREntity> DateRange_List = new List<Csb16DTREntity>();
        List<Csb16DTREntity> Sys_DateRange_List = new List<Csb16DTREntity>();
        private void Get_MasterTable_DateRanges()
        {
            Csb16DTREntity Search_Entity = new Csb16DTREntity(true);
            DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");

            Search_Entity.SYS_Date_Range = DateTime.Today.ToShortDateString();
            Sys_DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");
        }

       
        private bool Validate_Report()
        {
            bool Can_Generate = true;
            if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() != "**")
            {
                if (dtpQ1S.Value > dtpQ1E.Value)
                {
                    _errorProvider.SetError(dtpQ1S, string.Format("First Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpQ1S, null);
                }

                DateTime FFdate = Convert.ToDateTime(rngcodedata.OFdate);
                DateTime FTdate = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(rngcodedata.OFdate).Month + 2, Convert.ToDateTime(rngcodedata.OFdate).Month + 2));

                if (dtpQ1S.Value < FFdate)
                {
                    _errorProvider.SetError(dtpQ1E, string.Format("First Quarter Date Range should be in between January and March months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ1E, null);

                if (dtpQ1E.Value > FTdate)
                {
                    _errorProvider.SetError(dtpQ1E, string.Format("First Quarter Date Range should be in between January and March months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ1E, null);

                if (dtpQ2S.Value > dtpQ2E.Value)
                {
                    _errorProvider.SetError(dtpQ2S, string.Format("Second Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpQ2S, null);
                }

                //DateTime SFdate = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 3, 1);//Convert.ToDateTime(rngcodedata.OFdate);
                //DateTime STdate = new DateTime(Convert.ToDateTime(dtpQ2S.Value).Year, Convert.ToDateTime(dtpQ2S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ2S.Value).Month + 2, Convert.ToDateTime(dtpQ2S.Value).Month + 2));

                //if (dtpQ2S.Value > STdate || dtpQ2S.Value < SFdate)
                //{
                //    _errorProvider.SetError(dtpQ2E, string.Format("Second Quarter Date Range should be in between April and June months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ2E, null);

                //if (dtpQ2E.Value > STdate || dtpQ2E.Value < SFdate)
                //{
                //    _errorProvider.SetError(dtpQ2E, string.Format("Second Quarter Date Range should be in between April and June months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ2E, null);

                //if (dtpQ3S.Value > dtpQ3E.Value)
                //{
                //    _errorProvider.SetError(dtpQ3S, string.Format("Third Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //{
                //    _errorProvider.SetError(dtpQ3S, null);
                //}

                //DateTime TFdate = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 6, 1);
                //DateTime TTdate = new DateTime(Convert.ToDateTime(dtpQ3S.Value).Year, Convert.ToDateTime(dtpQ3S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ3S.Value).Month + 2, Convert.ToDateTime(dtpQ3S.Value).Month + 2));

                //if (dtpQ3S.Value > TTdate || dtpQ3S.Value < TFdate)
                //{
                //    _errorProvider.SetError(dtpQ3E, string.Format("Third Quarter Date Range should be in between July and September months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ3E, null);

                //if (dtpQ3E.Value > TTdate || dtpQ3E.Value < TFdate)
                //{
                //    _errorProvider.SetError(dtpQ3E, string.Format("Third Quarter Date Range should be in between July and September months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ3E, null);

                //if (dtpQ4S.Value > dtpQ4E.Value)
                //{
                //    _errorProvider.SetError(dtpQ4S, string.Format("Fourth Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //{
                //    _errorProvider.SetError(dtpQ4S, null);
                //}

                //DateTime FoFdate = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 9, 1);
                //DateTime FoTdate = new DateTime(Convert.ToDateTime(dtpQ4S.Value).Year, Convert.ToDateTime(dtpQ4S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ4S.Value).Month + 2, Convert.ToDateTime(dtpQ4S.Value).Month + 2));

                //if (dtpQ4S.Value < FoFdate)
                //{
                //    _errorProvider.SetError(dtpQ4E, string.Format("Fourth Quarter Date Range should be in between October and December months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ4E, null);

                //if (dtpQ4E.Value > FoTdate)
                //{
                //    _errorProvider.SetError(dtpQ4E, string.Format("Fourth Quarter Date Range should be in between October and December months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                //    Can_Generate = false;
                //}
                //else
                //    _errorProvider.SetError(dtpQ4E, null);


                int MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 3;
                int YearValue = Convert.ToDateTime(rngcodedata.OFdate).Year;
                if (MonthValue > 12) { MonthValue = MonthValue - 12; YearValue = YearValue + 1; }

                DateTime SFdate = new DateTime(YearValue, MonthValue, 1);//Convert.ToDateTime(rngcodedata.OFdate);
                DateTime STdate = new DateTime(Convert.ToDateTime(dtpQ2S.Value).Year, Convert.ToDateTime(dtpQ2S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ2S.Value).Month + 2, Convert.ToDateTime(dtpQ2S.Value).Month + 2));

                if (dtpQ2S.Value > STdate || dtpQ2S.Value < SFdate)
                {
                    _errorProvider.SetError(dtpQ2E, string.Format("Second Quarter Date Range should be in between April and June months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ2E, null);

                if (dtpQ2E.Value > STdate || dtpQ2E.Value < SFdate)
                {
                    _errorProvider.SetError(dtpQ2E, string.Format("Second Quarter Date Range should be in between April and June months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ2E, null);

                if (dtpQ3S.Value > dtpQ3E.Value)
                {
                    _errorProvider.SetError(dtpQ3S, string.Format("Third Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpQ3S, null);
                }

                MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 6;
                if (MonthValue > 12) { MonthValue = MonthValue - 12; }
                DateTime TFdate = new DateTime(YearValue, MonthValue, 1);
                DateTime TTdate = new DateTime(Convert.ToDateTime(dtpQ3S.Value).Year, Convert.ToDateTime(dtpQ3S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ3S.Value).Month + 2, Convert.ToDateTime(dtpQ3S.Value).Month + 2));

                if (dtpQ3S.Value > TTdate || dtpQ3S.Value < TFdate)
                {
                    _errorProvider.SetError(dtpQ3E, string.Format("Third Quarter Date Range should be in between July and September months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ3E, null);

                if (dtpQ3E.Value > TTdate || dtpQ3E.Value < TFdate)
                {
                    _errorProvider.SetError(dtpQ3E, string.Format("Third Quarter Date Range should be in between July and September months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ3E, null);

                if (dtpQ4S.Value > dtpQ4E.Value)
                {
                    _errorProvider.SetError(dtpQ4S, string.Format("Fourth Quarter 'From Date' should be prior or equal to its 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(dtpQ4S, null);
                }
                MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 9;
                if (MonthValue > 12) { MonthValue = MonthValue - 12; }
                DateTime FoFdate = new DateTime(YearValue, MonthValue, 1);
                DateTime FoTdate = new DateTime(Convert.ToDateTime(dtpQ4S.Value).Year, Convert.ToDateTime(dtpQ4S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ4S.Value).Month + 2, Convert.ToDateTime(dtpQ4S.Value).Month + 2));

                if (dtpQ4S.Value < FoFdate)
                {
                    _errorProvider.SetError(dtpQ4E, string.Format("Fourth Quarter Date Range should be in between October and December months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ4E, null);

                if (dtpQ4E.Value > FoTdate)
                {
                    _errorProvider.SetError(dtpQ4E, string.Format("Fourth Quarter Date Range should be in between October and December months of selected year".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(dtpQ4E, null);

                _errorProvider.SetError(cmbRngCode, null);
            }
            else
            {
                _errorProvider.SetError(cmbRngCode, "Please Select Code");
                Can_Generate = false;
            }
            /**if (!Ref_From_Date.Checked)
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

            if (rdoperiodBoth.Checked || rdoperiod.Checked)
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
            else if (rdoperiodCumulative.Checked)
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
                _errorProvider.SetError(Rb_Fund_Sel, null);*/


            if ((Rb_SP_Sel.Checked && ListServicePlans.Count <= 0 && Scr_Oper_Mode == "RNGB0006"))
            {
                _errorProvider.SetError(Rb_SP_Sel, string.Format("Please Select at least One " + (Scr_Oper_Mode == "RNGB0006" ? "'Service Plan'" : "'ZIP Code'").Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_SP_Sel, null);

            if (Scr_Oper_Mode == "RNGB0006")
            {
                if (Rb_County_Sel.Checked && ListcommonEntity.Count <= 0)
                {
                    _errorProvider.SetError(Rb_County_Sel, string.Format("Please Select at least One 'County'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_County_Sel, null);
            }

           /** if (Rb_Site_Sel.Checked && string.IsNullOrEmpty(Txt_Sel_Site.Text.Trim()))
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
                _errorProvider.SetError(rdoMsselectsite, null);*/

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
            return Can_Generate;
        }






        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        SO_Browse_Entity Search_Entity = new SO_Browse_Entity(true);
        PM_Browse_Entity PM_Search_Entity = new PM_Browse_Entity(true);
        string[] Sel_params_To_Print = new string[20] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
        private void Get_Selection_Criteria()
        {
            Sel_params_To_Print[4] = string.Empty;
            switch (Scr_Oper_Mode)
            {
                case "RNGB0006":
                    if (Rb_SP_All.Checked)
                        Search_Entity.ZipCode = "**";
                    else
                    {
                        string strGrpcode = Get_Sel_SPCodes();
                        //if (strGrpcode.Trim() == "'7'")
                        //    Search_Entity.ZipCode = "**";
                        //else
                            Search_Entity.ZipCode = strGrpcode;
                    }

                    break;
                //case "RNGB0014":
                //    //Search_Entity.Attribute = "All";
                //    //if (Rb_User_Def.Checked)
                //    //    Search_Entity.Attribute = "Goal";

                //    if (Rb_SP_All.Checked)
                //        Search_Entity.ZipCode = "**";
                //    else
                //    {
                //        string strGrpcode = Get_Sel_GroupCodes();
                //        if (strGrpcode.Trim() == "'7'")
                //            Search_Entity.ZipCode = "**";
                //        else
                //            Search_Entity.ZipCode = strGrpcode;
                //    }

                //    Search_Entity.County = "**";
                //    if (Rb_County_Sel.Checked)
                //        Search_Entity.County = Get_Sel_County();

                //    /**Search_Entity.PM_Rep_Format = "PM";
                //    if (Rb_SNP_Mem.Checked)
                //        Search_Entity.PM_Rep_Format = "Both";
                //    else if (rbo_ProgramWise.Checked)
                //        Search_Entity.PM_Rep_Format = "Prog";*/

                //    Search_Entity.DG_Count_Sw = Search_Entity.PM_Rep_Format;

                //    break;
            }

            Search_Entity.RngMainCode = ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString();
            Search_Entity.Ms_DriveColumn_Sw = "CADATE";
            if (Rb_MS_AddDate.Checked)
                Search_Entity.Ms_DriveColumn_Sw = "CAADDDATE";

            Search_Entity.CA_MS_Sw = "MS";


            /** Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy");//Ref_From_Date.Value.ToShortDateString();
             Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy");//Ref_To_Date.Value.ToShortDateString();
             if (rdoperiod.Checked)
             {
                 Search_Entity.Rep_From_Date = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
                 Search_Entity.Rep_To_Date = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();
             }
             Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
             Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();*/

            /* if (rdoperiodCumulative.Checked)
             {
                 if (Rb_SNP_Mem.Checked || rbo_ProgramWise.Checked)
                 {
                     Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_From_Date.Value.ToShortDateString();
                     Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
                 }
             }*/

            Search_Entity.Q1_START_DATE= Convert.ToDateTime(dtpQ1S.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q1_END_DATE = Convert.ToDateTime(dtpQ1E.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q2_START_DATE = Convert.ToDateTime(dtpQ2S.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q2_END_DATE = Convert.ToDateTime(dtpQ2E.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q3_START_DATE = Convert.ToDateTime(dtpQ3S.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q3_END_DATE = Convert.ToDateTime(dtpQ3E.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q4_START_DATE = Convert.ToDateTime(dtpQ4S.Value.ToString()).ToString("MM/dd/yyyy");
            Search_Entity.Q4_END_DATE = Convert.ToDateTime(dtpQ4E.Value.ToString()).ToString("MM/dd/yyyy");

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

           /** Search_Entity.Mst_Site = "**";
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
                Search_Entity.CA_Fund_Filter = Get_Sel_CA_Fund_List_To_Filter();*/


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

            /**string Sel_Programs = string.Empty;
            if (rbSelProgram.Checked == true)
            {
                if (SelectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity Entity in SelectedHierarchies)
                    {
                        Sel_Programs += Entity.Agency + Entity.Dept + Entity.Prog + ",";
                    }

                    if (Sel_Programs.Length > 0)
                        Sel_Programs = Sel_Programs.Substring(0, (Sel_Programs.Length - 1));

                    if (Sel_Programs.Length > 0)
                        Sel_Permesures_Programs = Search_Entity.Activity_Prog = Sel_Programs;
                }
            }*/

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
            //switch (Search_Entity.Mst_Site)
            //{
            //    case "**": Sel_params_To_Print[2] = "All Sites"; break;
            //    case "NO": Sel_params_To_Print[2] = "No Sites"; break;
            //    default: Sel_params_To_Print[2] = "Selected Site"; break;
            //}
            //switch (Search_Entity.CaseMssite)
            //{
            //    case "**": Sel_params_To_Print[3] = "ALL MS Sites"; break;
            //    case "NO": Sel_params_To_Print[3] = "No MS Sites"; break;
            //    default: Sel_params_To_Print[3] = "Selected MS Site"; break;
            //}
           //** Search_Entity.OutComeSwitch = rdoOutcomesAll.Checked == true ? "A" : "S";
            Search_Entity.IncVerSwitch = chkincverswitch.Checked == true ? "Y" : "N";
        }

       
        private string Get_Sel_SPCodes()
        {
            string Sel_Groups_Codes = string.Empty;
            foreach (CASESP1Entity Entity in ListServicePlans)
            {
                Sel_Groups_Codes += "'" + Entity.Code + "' ,";
            }

            if (Sel_Groups_Codes.Length > 0)
                Sel_Groups_Codes = Sel_Groups_Codes.Substring(0, (Sel_Groups_Codes.Length - 1));

            return Sel_Groups_Codes;
        }

        List<RCsb14GroupEntity> OutCome_MasterList = new List<RCsb14GroupEntity>();
        private void Prepare_Selected_Group(string Sel_ZipCodes)
        {
            ListRngGroupCode.Clear();
            OutCome_MasterList = _model.SPAdminData.Browse_RNGGrp(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), null, null, null, null, BaseForm.UserID, BaseForm.BaseAgency);
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

            /**string strrptfor = "R";
            if (rdoperiodCumulative.Checked)
                strrptfor = "C";
            if (rdoperiodBoth.Checked)
                strrptfor = "B";

            string strRepControl = "N";
            if (chkRepControl.Checked == true)
                strRepControl = "Y";*/

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
                                      "\" CAMS_SW = \"" + Search_Entity.CA_MS_Sw + "\" CAMS_Filter_List = \"" + Search_Entity.CAMS_Filter + "\" FUND_Filter_List = \"" + Search_Entity.CA_Fund_Filter + "\" RNG_MAIN_CODE = \"" + Search_Entity.RngMainCode + "\" OUTCOME_SWITCH = \"" + "\" CASEMSSITE = \"" + Search_Entity.CaseMssite + "\" INCVERSWITCH = \"" + Search_Entity.IncVerSwitch + "\" REPORTFOR =\""  + "\" REPORTCONTROL =\"" + "\" />"); //
           /** //switch (Scr_Oper_Mode)
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
            //}*/
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

               /** Txt_Sel_Site.Clear();
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
                }*/

                if (columns.Contains("INCVERSWITCH"))
                {
                    chkincverswitch.Checked = dr["INCVERSWITCH"].ToString() == "Y" ? true : false;
                }
               /** if (columns.Contains("CASEMSSITE"))
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
                }*/


               /** Ref_From_Date.Value = Convert.ToDateTime(dr["REFERENCE_FROM_DATE"].ToString());
                Ref_To_Date.Value = Convert.ToDateTime(dr["REFERENCE_TO_DATE"].ToString());
                Rep_From_Date.Value = Convert.ToDateTime(dr["REPORT_FROM_DATE"].ToString());
                Rep_To_Date.Value = Convert.ToDateTime(dr["REPORT_TO_DATE"].ToString());*/

               /** if (columns.Contains("FUND_Filter_List"))
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

                }*/

               /** if (columns.Contains("REPORTFOR"))
                {
                    rdoperiod.Checked = true;
                    //if (dr["REPORTCONTROL"].ToString() == "R")
                    //    rdoperiod.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "C")
                        rdoperiodCumulative.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "B")
                        rdoperiodBoth.Checked = true;
                    reportforselection();

                }*/

                if (dr["DATE_SELECTION"].ToString() == "MSDATE")
                    Rb_MS_Date.Checked = true;
                else
                    Rb_MS_AddDate.Checked = true;

                //if ((dr["ATTRIBUTES"].ToString() == "SYSTEM" && Scr_Oper_Mode == "RNGB0004") ||
                //    (dr["ATTRIBUTES"].ToString() == "All" && Scr_Oper_Mode == "RNGB0014"))
                //    Rb_Agy_Def.Checked = true;
                //else
                //    Rb_User_Def.Checked = true;

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


               /** if (dr["DG_COUNT"].ToString() == "PM")
                    Rb_OBO_Mem.Checked = true;
                else if (dr["DG_COUNT"].ToString() == "Prog")
                    rbo_ProgramWise.Checked = true;
                else
                    Rb_SNP_Mem.Checked = true;*/


                switch (dr["SECRET_APP"].ToString())
                {
                    case "Y": Rb_Mst_Sec.Checked = true; break;
                    case "N": Rb_Mst_NonSec.Checked = true; break;
                    default: Rb_Mst_BothSec.Checked = true; break;
                }


                //Fill_Program_Combo();
                //SetComboBoxValue(Cmb_Program, dr["ACTY_PROGRAM"].ToString().Trim() == "******" ? "**" : dr["ACTY_PROGRAM"].ToString());

                /**if (dr["ACTY_PROGRAM"].ToString() != "A")
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
                }*/

                SetComboBoxValue(cmbRngCode, dr["RNG_MAIN_CODE"].ToString().Trim());

                /**Ref_From_Date.Value = Convert.ToDateTime(dr["REFERENCE_FROM_DATE"].ToString());
                Ref_To_Date.Value = Convert.ToDateTime(dr["REFERENCE_TO_DATE"].ToString());*/

                if (dr["ZIPCODE"].ToString() == "**")
                    Rb_SP_All.Checked = true;
                else
                {
                    Rb_SP_Sel.Checked = true;
                    if (Scr_Oper_Mode == "RNGB0014")
                        Prepare_Selected_Group(dr["ZIPCODE"].ToString());
                }
               /** if (columns.Contains("OUTCOME_SWITCH"))
                {
                    if (dr["OUTCOME_SWITCH"].ToString() == "S")
                        rdoOutcomesselect.Checked = true;
                    else
                        rdoOutcomesAll.Checked = true;
                }*/


               /** if (columns.Contains("REPORTCONTROL"))
                {
                    if (dr["REPORTCONTROL"].ToString() == "Y")
                        chkRepControl.Checked = true;
                    else
                        chkRepControl.Checked = false;
                }*/

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


      
        string Scr_Oper_Mode = "RNGB0006";


        private void Initialize_All_Controls()
        {


            Scr_Oper_Mode = "RNGB0014";
            //20160303

            this.Text = "RNGB0014 - Performance Measures";

            lblDateSelection.Text = "Process Report By";
            //lblAttributes.Text = "Categories";
            //Rb_Agy_Def.Text = "All";
            //Rb_User_Def.Text = "Only Goal Associated";

            lblProduceStatistical.Text = "Produce Details";

            Rb_Details_No.Text = "No";
            Rb_Details_Yes.Text = "Yes";

            lblServiceplan.Text = "Group";
            Rb_SP_All.Text = "All Groups";
            Rb_SP_Sel.Text = "Selected Groups";

            //lblCounty.Text = "Report Format";
            //Rb_County_All.Text = "Performance Measures Only";
            //Rb_County_Sel.Text = "Performance Measures + Goal Details";

            Rb_MS_Date.Text = "Outcome Date";//"Milestone Date";
            Rb_MS_AddDate.Text = "Outcome Add Date";//"Milestone Add Date";

          /**  lblDemographicsCount.Text = "Report Format";
            Rb_OBO_Mem.Text = "Performance Measures Only";
            Rb_SNP_Mem.Text = "Performance Measures + Goal Details";
            rbo_ProgramWise.Text = "Program Level Counts";
            Rb_SNP_Mem.Location = new System.Drawing.Point(154, 2);*/


            /**Lbl_Program.Visible = true; //Cmb_Program.Visible = true;
            Lbl_Program.Location = new System.Drawing.Point(5, 36);*/
            //Cmb_Program.Location = new System.Drawing.Point(143, 36);
            //this.Cmb_Program.Size = new System.Drawing.Size(249, 21);

            this.Date_Panel.Location = new System.Drawing.Point(0, -1);
            this.Service_Panel.Size = new System.Drawing.Size(450, 21);

            //this.panel8.Location = new System.Drawing.Point(0, 48);


            this.pnlfewrdb.Location = new System.Drawing.Point(0, 48);
            //this.panel8.Size = new System.Drawing.Size(607, 224); //20160303
            this.pnlfewrdb.Size = new System.Drawing.Size(607, 247);

            //this.panel4.Location = new System.Drawing.Point(-1, 33);
            //**this.pnlMultiRdb.Location = new System.Drawing.Point(-1, 55);

            //**Rb_SNP_Mem.Size = new System.Drawing.Size(230, 21);

            //**this.pnlMultiRdb.Size = new System.Drawing.Size(607, 296);
            //this.panel3.Location = new System.Drawing.Point(4, 385); //20160303
            //this.panel2.Size = new System.Drawing.Size(607, 352); //20160303

            //this.Size = new System.Drawing.Size(613, 395);

            //this.Size = new System.Drawing.Size(615, 422);
            //Fill_Program_Combo();


            Cmb_CaseType.SelectedIndex = 0;
            Rb_MS_Date.Checked = Rb_Stat_Both.Checked //**= Rb_Site_All.Checked = rdoMssiteall.Checked
            = Rb_Details_No.Checked = Rb_SP_All.Checked = Rb_County_All.Checked =
            Rb_Mst_NonSec.Checked = true;

            All_CAMS_Selected = true;
            Sel_MS_List.Clear();
            Sel_CA_List.Clear();

            switch (Scr_Oper_Mode)
            {
                case "RNGB0004":
                    //Rb_User_Def.Checked = true;//** Rb_OBO_Mem.Checked = true;
                   /** Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                    Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);*/
                    break;
                case "RNGB0014":
                    //Rb_Agy_Def.Checked = true; //**rbAllPrograms.Checked = true; //Fill_Program_Combo();
                    //**HierarchyGrid.Rows.Clear(); 
                    //**SelectedHierarchies.Clear();
                   /** if (Sys_DateRange_List.Count > 0)
                    {
                        Ref_From_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_FDATE);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                        Ref_To_Date.Value = Convert.ToDateTime(Sys_DateRange_List[0].REF_TDATE);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    }
                    else
                    {
                        Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                        Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    }*/
                    break;
            }

           /** Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));

            Ref_From_Date.Checked = Ref_To_Date.Checked = Rep_From_Date.Checked = Rep_To_Date.Checked = true;*/

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
            //**_errorProvider.SetError(Ref_To_Date, null);
            _errorProvider.SetError(Rb_SP_Sel, null);
            _errorProvider.SetError(Rb_County_Sel, null);
            //**_errorProvider.SetError(Txt_Sel_Site, null);
            //**_errorProvider.SetError(txt_Msselect_site, null);
            _errorProvider.SetError(Txt_Pov_Low, null);
            _errorProvider.SetError(Txt_Pov_High, null);
        }


        private void Rb_Zip_All_CheckedChanged(object sender, EventArgs e)
        {
            switch (Scr_Oper_Mode)
            {
                case "RNGB0004": ListZipCode.Clear(); break;
                case "RNGB0014": ListRngGroupCode.Clear(); break;
                case "RNGB0006": ListServicePlans.Clear();break;
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

        //DataTable dt = new DataTable();

        DataTable dtAudit = new DataTable();
        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            if (Validate_Report())
            {
                Get_Selection_Criteria();

                Search_Entity.Rep_Period_FDate = Convert.ToDateTime(((ListItem)cmbRngCode.SelectedItem).ScreenCode.ToString()).ToString("MM/dd/yyyy");
                Search_Entity.Rep_Period_TDate = Convert.ToDateTime(((ListItem)cmbRngCode.SelectedItem).ScreenType.ToString()).ToString("MM/dd/yyyy");

                DataSet ds = new DataSet();

                ds = _model.AdhocData.Get_RNGB0005_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "Y", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Q", string.Empty);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (Rb_Details_Yes.Checked)
                        dtAudit = ds.Tables[1];

                    if (dt.Rows.Count > 0)
                    {
                        PdfListForm pdfListForm = new PdfListForm(BaseForm, PrivilegeEntity, false, propReportPath, "xls");
                        pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveExcelForm_Closed);
                        pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                        pdfListForm.ShowDialog();
                    }
                    else
                        MessageBox.Show("No data for given parameters", "CAPTAIN");

                }
            }
        }

        List<CAMASTEntity> CA_Mast_List = new List<CAMASTEntity>();
        private void Fill_CAMS_Master_List()
        {
            MS_Mast_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            CA_Mast_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
        }

       /** private void rdoMssiteall_Click(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            ListcaseMsSiteEntity.Clear();
        }*/

        private void rdoMsselectsite_CheckedChanged(object sender, EventArgs e)
        {

        }

        /**private void rdoMsselectsite_Click(object sender, EventArgs e)
        {
            if (rdoMsselectsite.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseMsSiteEntity, strAgency, strDept, strProg, string.Empty);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyMSFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }*/

        /**private void rdomsNosite_CheckedChanged(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            ListcaseMsSiteEntity.Clear();
        }*/

        /**private void Btn_MS_Selection_Click(object sender, EventArgs e)
        //{
        //    CASE0010_HSS_Form MS_Selection_Form = new CASE0010_HSS_Form(PrivilegeEntity.Program, "MS", All_CAMS_Selected, Sel_MS_List, Sel_CA_List);
        //    MS_Selection_Form.FormClosed += new FormClosedEventHandler(Get_Sel_MS_List);
        //    MS_Selection_Form.StartPosition = FormStartPosition.CenterScreen;
        //    MS_Selection_Form.ShowDialog();
        //}*/

        /**private void Rb_Fund_Sel_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Fund_Sel.Checked == true)
            {
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, Sel_Funding_List, "Reports", strAgency, strDept, strProg, null);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }
        }*/

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


      

        RCsb14GroupEntity rngcodedata;
        private void cmbRngCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRngCode.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() != "**")
                {

                    rngcodedata = RngCodelist.Find(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                    if (rngcodedata != null)
                    {
                        ListRngGroupCode.Clear();
                        /**if (rngcodedata.OFdate != string.Empty)
                        {
                            Ref_From_Date.Value = Convert.ToDateTime(rngcodedata.OFdate);
                            Ref_From_Date.Checked = true;
                        }
                        if (rngcodedata.OTdate != string.Empty)
                        {
                            Ref_To_Date.Checked = true;
                            Ref_To_Date.Value = Convert.ToDateTime(rngcodedata.OTdate);
                        }*/

                        lblAnnualFDte.Text = Convert.ToDateTime(rngcodedata.OFdate).ToString("MM/dd//yyyy");   //Added by Vikash 03/20/2023
                        lblAnnualTDte.Text = Convert.ToDateTime(rngcodedata.OTdate).ToString("MM/dd/yyyy");

                        dtpQ1S.Value = Convert.ToDateTime(rngcodedata.OFdate);
                        dtpQ1E.Value = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month+2, DateTime.DaysInMonth(Convert.ToDateTime(rngcodedata.OFdate).Month+2, Convert.ToDateTime(rngcodedata.OFdate).Month+2));
                        //Convert.ToDateTime(rngcodedata.OFdate);

                        //new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));

                        //dtpQ2S.Value = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 3, 1);//Convert.ToDateTime(rngcodedata.OFdate);
                        //dtpQ2E.Value = new DateTime(Convert.ToDateTime(dtpQ2S.Value).Year, Convert.ToDateTime(dtpQ2S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ2S.Value).Month + 2, Convert.ToDateTime(dtpQ2S.Value).Month + 2));

                        //dtpQ3S.Value = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 6, 1);
                        //dtpQ3E.Value = new DateTime(Convert.ToDateTime(dtpQ3S.Value).Year, Convert.ToDateTime(dtpQ3S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ3S.Value).Month + 2, Convert.ToDateTime(dtpQ3S.Value).Month + 2));

                        //dtpQ4S.Value = new DateTime(Convert.ToDateTime(rngcodedata.OFdate).Year, Convert.ToDateTime(rngcodedata.OFdate).Month + 9, 1);
                        //dtpQ4E.Value = new DateTime(Convert.ToDateTime(dtpQ4S.Value).Year, Convert.ToDateTime(dtpQ4S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ4S.Value).Month + 2, Convert.ToDateTime(dtpQ4S.Value).Month + 2));

                        int YearValue = Convert.ToDateTime(rngcodedata.OFdate).Year;
                        int MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 3;
                        if (MonthValue > 12) { MonthValue = MonthValue - 12; YearValue = YearValue + 1; }

                        dtpQ2S.Value = new DateTime(YearValue, MonthValue, 1);//Convert.ToDateTime(rngcodedata.OFdate);
                        dtpQ2E.Value = new DateTime(Convert.ToDateTime(dtpQ2S.Value).Year, Convert.ToDateTime(dtpQ2S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ2S.Value).Month + 2, Convert.ToDateTime(dtpQ2S.Value).Month + 2));

                        MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 6;
                        if (MonthValue > 12) MonthValue = MonthValue - 12;

                        dtpQ3S.Value = new DateTime(YearValue, MonthValue, 1);
                        dtpQ3E.Value = new DateTime(Convert.ToDateTime(dtpQ3S.Value).Year, Convert.ToDateTime(dtpQ3S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ3S.Value).Month + 2, Convert.ToDateTime(dtpQ3S.Value).Month + 2));

                        MonthValue = Convert.ToDateTime(rngcodedata.OFdate).Month + 9;
                        if (MonthValue > 12) MonthValue = MonthValue - 12;
                        dtpQ4S.Value = new DateTime(YearValue, MonthValue, 1);
                        dtpQ4E.Value = new DateTime(Convert.ToDateTime(dtpQ4S.Value).Year, Convert.ToDateTime(dtpQ4S.Value).Month + 2, DateTime.DaysInMonth(Convert.ToDateTime(dtpQ4S.Value).Month + 2, Convert.ToDateTime(dtpQ4S.Value).Month + 2));

                    }
                    lblAnnualFDte.Visible = lblTo.Visible = lblAnnualTDte.Visible = true;
                }
                else
                {
                    lblAnnualFDte.Visible = lblTo.Visible = lblAnnualTDte.Visible = false;

                    dtpQ1S.Value = Convert.ToDateTime(DateTime.Now);
                    dtpQ1E.Value = Convert.ToDateTime(DateTime.Now);
                    //Convert.ToDateTime(rngcodedata.OFdate);

                    //new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));

                    dtpQ2S.Value = Convert.ToDateTime(DateTime.Now); //Convert.ToDateTime(rngcodedata.OFdate);
                    dtpQ2E.Value = Convert.ToDateTime(DateTime.Now);

                    dtpQ3S.Value = Convert.ToDateTime(DateTime.Now);
                    dtpQ3E.Value = Convert.ToDateTime(DateTime.Now);

                    dtpQ4S.Value = Convert.ToDateTime(DateTime.Now);
                    dtpQ4E.Value = Convert.ToDateTime(DateTime.Now);
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


        private void btnMergeExcelView_Click(object sender, EventArgs e)
        {
            PdfListForm pdfMergeListForm = new PdfListForm(BaseForm);
            pdfMergeListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfMergeListForm.ShowDialog();
        }

        string Random_Filename = string.Empty;
        private void On_SaveExcelForm_Closed(object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Random_Filename = null;
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                string PdfFileName = PdfName;
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay("Error");
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

                Workbook book = new Workbook();

                this.GenerateStyles1(book.Styles);



                bool First = true; string MS_Type = string.Empty;
                string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
                bool SPNum = true; Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;

                if(dt.Rows.Count>0)
                {
                    this.GenerateWorksheetParameters(book.Worksheets);

                    string PrivSP = string.Empty;
                    var distinctRows = (from DataRow dRow in dt.Rows select dRow["SP0_SERVICECODE"]).Distinct();

                    foreach (var dRow in distinctRows)
                    {
                        DataTable dataTable = new DataTable();
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "SP0_SERVICECODE='" + dRow + "'";
                        dataTable = dv.ToTable();

                        List<SPTargetEntity> EntityList = _model.SPAdminData.Browse_SPTarget(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), dRow.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());//Current_Hierarchy.Substring(0,2)

                        if (dataTable.Rows.Count > 0)
                        {
                            string ReportName = dataTable.Rows[0]["SP0_DESCRIPTION"].ToString();
                            ReportName = ReportName.Replace("/", "");

                            if (ReportName.Length >= 31)
                            {
                                ReportName = ReportName.Substring(0, 31);
                            }

                            this.GenerateWorksheetC2cWPPPRWICp8(book.Worksheets, ReportName, EntityList, dRow.ToString());
                        }

                    }



                    
                }


                //if(dt.Rows.Count>0)
                //{
                //    this.GenerateWorksheetParameters(book.Worksheets);

                //    string PrivSP = string.Empty;

                //    var distinctRows = (from DataRow dRow in dt.Rows
                //                        select dRow["SP0_SERVICECODE"]).Distinct();

                //    foreach(var dRow in distinctRows)
                //    {
                //        DataTable dataTable = new DataTable();
                //        DataView dv = new DataView(dt);
                //        dv.RowFilter = "SP0_SERVICECODE='" + dRow + "'";
                //        dataTable = dv.ToTable();

                //        if(dataTable.Rows.Count>0)
                //        {
                //            string ReportName = dataTable.Rows[0]["SP0_DESCRIPTION"].ToString();
                //            ReportName = ReportName.Replace("/", "");

                //            if (ReportName.Length >= 31)
                //            {
                //                ReportName = ReportName.Substring(0, 31);
                //            }
                //            sheet = book.Worksheets.Add(ReportName);
                //            sheet.Table.DefaultRowHeight = 14.25F;


                //            sheet.Table.Columns.Add(200);
                //            sheet.Table.Columns.Add(150);
                //            sheet.Table.Columns.Add(75);
                //            sheet.Table.Columns.Add(75);
                //            sheet.Table.Columns.Add(75);
                //            sheet.Table.Columns.Add(75);
                //            sheet.Table.Columns.Add(75);
                //            sheet.Table.Columns.Add(80);
                //            sheet.Table.Columns.Add(80);

                //            Row0 = sheet.Table.Rows.Add();
                //            Row0.AutoFitHeight = false;
                //            //WorksheetCell cell;
                //            cell = Row0.Cells.Add();
                //            cell.StyleID = "s83";
                //            cell.Data.Type = DataType.String;
                //            cell.Data.Text = dataTable.Rows[0]["SP0_DESCRIPTION"].ToString();
                //            cell.MergeAcross = 8;


                //            Row0 = sheet.Table.Rows.Add();

                //            cell = Row0.Cells.Add("Description", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Type", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Target", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Q1", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Q2", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Q3", DataType.String, "s94");
                //            cell = Row0.Cells.Add("Q4", DataType.String, "s94");
                //            cell = Row0.Cells.Add("YTD", DataType.String, "s94");
                //            cell = Row0.Cells.Add("YTD %", DataType.String, "s94");


                //            int i = 0;
                //            foreach (DataRow dr in dataTable.Rows)
                //            {
                //                Row0 = sheet.Table.Rows.Add();

                                
                //                string CAMSType = string.Empty;
                //                if (dr["SP2_TYPE"].ToString().Trim() == "CA") CAMSType = "Service"; else CAMSType = "Outcome";

                //                string Ytd = string.Empty;
                //                if(dr["TARGET_VAL"].ToString()!="0")
                //                {
                //                    decimal percent= (decimal.Parse(dr["YTD"].ToString().Trim()==string.Empty?"0": dr["YTD"].ToString().Trim()) / decimal.Parse(dr["TARGET_VAL"].ToString().Trim()==string.Empty?"0":dr["TARGET_VAL"].ToString().Trim())) *100;

                //                    Ytd = percent.ToString("0.00");
                //                }

                //                if(i%2==0)
                //                {
                //                    if (dr["SP0_ACTIVE"].ToString() == "Y")
                //                    {
                //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s105");
                //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s105");
                //                        cell = Row0.Cells.Add(dr["TARGET_VAL"].ToString(), DataType.String, "s106");
                //                        //if (dr["SP2_COUNT_TYPE"].ToString() == "A")
                //                        //{
                //                            cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A"?dr["Q1"].ToString()+"%": dr["Q1"].ToString(), DataType.String, "s106");
                //                            cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q2"].ToString() + "%": dr["Q2"].ToString(), DataType.String, "s106");
                //                            cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q3"].ToString() + "%": dr["Q3"].ToString(), DataType.String, "s106");
                //                            cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q4"].ToString() + "%":dr["Q4"].ToString(), DataType.String, "s106");
                //                            cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["YTD"].ToString() + "%":dr["YTD"].ToString(), DataType.String, "s106");
                //                            cell = Row0.Cells.Add(Ytd==string.Empty?"": Ytd + "%", DataType.String, "s106");
                //                        //}
                //                        //else
                //                        //{
                //                        //    cell = Row0.Cells.Add(dr["Q1"].ToString(), DataType.String, "s106");
                //                        //    cell = Row0.Cells.Add(dr["Q2"].ToString(), DataType.String, "s106");
                //                        //    cell = Row0.Cells.Add(dr["Q3"].ToString(), DataType.String, "s106");
                //                        //    cell = Row0.Cells.Add(dr["Q4"].ToString(), DataType.String, "s106");
                //                        //    cell = Row0.Cells.Add(dr["YTD"].ToString(), DataType.String, "s106");
                //                        //    cell = Row0.Cells.Add(Ytd, DataType.String, "s106");
                //                        //}
                //                    }
                //                    else
                //                    {
                //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s105R");
                //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s105R");
                //                        cell = Row0.Cells.Add(dr["TARGET_VAL"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q1"].ToString()+"%": dr["Q1"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q2"].ToString()+"%": dr["Q2"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q3"].ToString()+"%": dr["Q3"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q4"].ToString()+"%": dr["Q4"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["YTD"].ToString()+"%": dr["YTD"].ToString(), DataType.String, "s106R");
                //                        cell = Row0.Cells.Add(Ytd == string.Empty ? "" : Ytd + "%", DataType.String, "s106R");
                //                    }
                                    
                //                }
                //                else
                //                {
                //                    if (dr["SP0_ACTIVE"].ToString() == "Y")
                //                    {
                //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s95");
                //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s95");
                //                        cell = Row0.Cells.Add(dr["TARGET_VAL"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q1"].ToString() + "%" : dr["Q1"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q2"].ToString() + "%" : dr["Q2"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q3"].ToString() + "%" : dr["Q3"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q4"].ToString() + "%" : dr["Q4"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["YTD"].ToString() + "%" : dr["YTD"].ToString(), DataType.String, "s95RC");
                //                        cell = Row0.Cells.Add(Ytd == string.Empty ? "" : Ytd + "%", DataType.String, "s95RC");

                //                    }
                //                    else
                //                    {
                //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s95R");
                //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s95R");
                //                        cell = Row0.Cells.Add(dr["TARGET_VAL"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q1"].ToString() + "%" : dr["Q1"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q2"].ToString() + "%" : dr["Q2"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q3"].ToString() + "%" : dr["Q3"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["Q4"].ToString() + "%" : dr["Q4"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(dr["SP2_COUNT_TYPE"].ToString() == "A" ? dr["YTD"].ToString()+"%": dr["YTD"].ToString(), DataType.String, "s95RCR");
                //                        cell = Row0.Cells.Add(Ytd == string.Empty ? "" : Ytd + "%", DataType.String, "s95RCR");
                //                    }

                //                }

                                


                //                //if (dr["SP0_SERVICECODE"].ToString() != PrivSP)
                //                //{



                //                //    PrivSP = dr["SP0_SERVICECODE"].ToString();
                //                //}


                //                i++;
                //            }
                //        }
                        
                //        if(Rb_SP_Sel.Checked)
                //        {
                //            if(dtAudit.Rows.Count>0)
                //            {
                //                DataTable dataAudit = new DataTable();
                //                DataView dvAudit = new DataView(dtAudit);
                //                dvAudit.RowFilter = "SP='" + dRow + "'";
                //                dataAudit = dvAudit.ToTable();

                //                if (dataAudit.Rows.Count > 0)
                //                {
                //                    string ReportName = dRow + "_Audit";//dataTable.Rows[0]["SP_Desc"].ToString();
                //                    ReportName = ReportName.Replace("/", "");

                //                    if (ReportName.Length >= 31)
                //                    {
                //                        ReportName = ReportName.Substring(0, 31);
                //                    }
                //                    sheet = book.Worksheets.Add(ReportName);
                //                    sheet.Table.DefaultRowHeight = 14.25F;


                //                    sheet.Table.Columns.Add(75);
                //                    sheet.Table.Columns.Add(75);
                //                    sheet.Table.Columns.Add(75);
                //                    sheet.Table.Columns.Add(75);
                //                    sheet.Table.Columns.Add(80);
                //                    sheet.Table.Columns.Add(145);
                //                    sheet.Table.Columns.Add(75);
                //                    sheet.Table.Columns.Add(80);
                //                    sheet.Table.Columns.Add(180);
                //                    sheet.Table.Columns.Add(80);
                //                    sheet.Table.Columns.Add(80);

                //                    Row0 = sheet.Table.Rows.Add();
                //                    Row0.AutoFitHeight = false;
                //                    //WorksheetCell cell;
                //                    cell = Row0.Cells.Add();
                //                    cell.StyleID = "s83";
                //                    cell.Data.Type = DataType.String;
                //                    cell.Data.Text = dataAudit.Rows[0]["SP_Desc"].ToString();
                //                    cell.MergeAcross = 9;


                //                    Row0 = sheet.Table.Rows.Add();

                //                    cell = Row0.Cells.Add("Agency", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Dept", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Program", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Year", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("App", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Name", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Type", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Code", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Description", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Date", DataType.String, "s94");
                //                    cell = Row0.Cells.Add("Amount", DataType.String, "s94");


                //                    int i = 0;
                //                    foreach (DataRow dr in dataAudit.Rows)
                //                    {
                //                        Row0 = sheet.Table.Rows.Add();


                //                        string CAMSType = string.Empty;
                //                        if (dr["SP2_TYPE"].ToString().Trim() == "CA") CAMSType = "Service"; else CAMSType = "Outcome";



                //                        if (i % 2 == 0)
                //                        {
                //                            cell = Row0.Cells.Add(dr["Agency"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["Dept"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["Program"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["Prog_Year"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["AppNo"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["Name"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(CAMSType, DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["CAMS_CODE"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["CAMS_DATE"].ToString()), DataType.String, "s105");
                //                            cell = Row0.Cells.Add(dr["CASEMS_COST"].ToString(), DataType.String, "s106");



                //                        }
                //                        else
                //                        {

                //                            cell = Row0.Cells.Add(dr["Agency"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["Dept"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["Program"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["Prog_Year"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["AppNo"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["Name"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(CAMSType, DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["CAMS_CODE"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["CAMS_DATE"].ToString()), DataType.String, "s95");
                //                            cell = Row0.Cells.Add(dr["CASEMS_COST"].ToString(), DataType.String, "s95RC");


                //                        }
                //                        i++;
                //                    }
                //                }


                //            }
                //        }

                //    }

                    
                    
                //}


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "SPREPAPP_Report.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

                FileInfo fiDownload = new FileInfo(PdfName);
                /// Need to check for file exists, is local file, is allow to read, etc...
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                }

                //if(Rb_Details_Yes.Checked && Rb_SP_All.Checked)
                //{
                //    On_SaveExcelAudit_Closed(PdfFileName);
                //}

            }

        }

        //private void GenerateStyles(WorksheetStyleCollection styles)
        //{
        //    // -----------------------------------------------
        //    //  Default
        //    // -----------------------------------------------
        //    WorksheetStyle Default = styles.Add("Default");
        //    Default.Name = "Normal";
        //    Default.Font.FontName = "Arial";
        //    Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s16
        //    // -----------------------------------------------
        //    WorksheetStyle s16 = styles.Add("s16");
        //    // -----------------------------------------------
        //    //  s17
        //    // -----------------------------------------------
        //    WorksheetStyle s17 = styles.Add("s17");
        //    s17.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s18
        //    // -----------------------------------------------
        //    WorksheetStyle s18 = styles.Add("s18");
        //    // -----------------------------------------------
        //    //  s19
        //    // -----------------------------------------------
        //    WorksheetStyle s19 = styles.Add("s19");
        //    s19.Font.FontName = "Arial";
        //    // -----------------------------------------------
        //    //  s20
        //    // -----------------------------------------------
        //    WorksheetStyle s20 = styles.Add("s20");
        //    s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s21
        //    // -----------------------------------------------
        //    WorksheetStyle s21 = styles.Add("s21");
        //    s21.Font.Bold = true;
        //    s21.Font.FontName = "Arial";
        //    s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s21.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s23
        //    // -----------------------------------------------
        //    WorksheetStyle s23 = styles.Add("s23");
        //    s23.Font.Bold = true;
        //    s23.Font.FontName = "Calibri";
        //    s23.Font.Size = 11;
        //    s23.Font.Color = "#000000";
        //    // -----------------------------------------------
        //    //  s24
        //    // -----------------------------------------------
        //    WorksheetStyle s24 = styles.Add("s24");
        //    s24.Interior.Color = "#D8D8D8";
        //    s24.Interior.Pattern = StyleInteriorPattern.Solid;
        //    // -----------------------------------------------
        //    //  s25
        //    // -----------------------------------------------
        //    WorksheetStyle s25 = styles.Add("s25");
        //    s25.Font.FontName = "Arial";
        //    s25.Interior.Color = "#D8D8D8";
        //    s25.Interior.Pattern = StyleInteriorPattern.Solid;
        //    // -----------------------------------------------
        //    //  s26
        //    // -----------------------------------------------
        //    WorksheetStyle s26 = styles.Add("s26");
        //    s26.Interior.Color = "#D8D8D8";
        //    s26.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s26.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s27
        //    // -----------------------------------------------
        //    WorksheetStyle s27 = styles.Add("s27");
        //    s27.Interior.Color = "#D8D8D8";
        //    s27.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s28
        //    // -----------------------------------------------
        //    WorksheetStyle s28 = styles.Add("s28");
        //    s28.Font.Bold = true;
        //    s28.Font.FontName = "Arial";
        //    s28.Interior.Color = "#D8D8D8";
        //    s28.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s28.NumberFormat = "0%";
           
        //    // -----------------------------------------------
        //    //  s71
        //    // -----------------------------------------------
        //    WorksheetStyle s71 = styles.Add("s71");
        //    s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
           
        //    // -----------------------------------------------
        //    //  s73
        //    // -----------------------------------------------
        //    WorksheetStyle s73 = styles.Add("s73");
        //    s73.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s74
        //    // -----------------------------------------------
        //    WorksheetStyle s74 = styles.Add("s74");
        //    s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s74.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s75
        //    // -----------------------------------------------
        //    WorksheetStyle s75 = styles.Add("s75");
        //    s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s76
        //    // -----------------------------------------------
        //    WorksheetStyle s76 = styles.Add("s76");
        //    s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s76.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s77
        //    // -----------------------------------------------
        //    WorksheetStyle s77 = styles.Add("s77");
        //    s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s77.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s78
        //    // -----------------------------------------------
        //    WorksheetStyle s78 = styles.Add("s78");
        //    s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s78.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s79
        //    // -----------------------------------------------
        //    WorksheetStyle s79 = styles.Add("s79");
        //    s79.Font.Bold = true;
        //    s79.Font.FontName = "Arial";
        //    s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s81
        //    // -----------------------------------------------
        //    WorksheetStyle s81 = styles.Add("s81");
        //    s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s82
        //    // -----------------------------------------------
        //    WorksheetStyle s82 = styles.Add("s82");
        //    s82.Font.Bold = true;
        //    s82.Font.FontName = "Arial";
        //    s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s82.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s83
        //    // -----------------------------------------------
        //    WorksheetStyle s83 = styles.Add("s83");
        //    s83.Font.Bold = true;
        //    s83.Font.FontName = "Arial";
        //    s83.Font.Size = 11;
        //    s83.Font.Color = "#666699";
        //    s83.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s83.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s83.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s84
        //    // -----------------------------------------------
        //    WorksheetStyle s84 = styles.Add("s84");
        //    s84.Font.Bold = true;
        //    s84.Font.FontName = "Arial";
        //    s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s84.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s86
        //    // -----------------------------------------------
        //    WorksheetStyle s86 = styles.Add("s86");
        //    s86.Font.Bold = true;
        //    s86.Font.FontName = "Arial";
        //    s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s86.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s87
        //    // -----------------------------------------------
        //    WorksheetStyle s87 = styles.Add("s87");
        //    s87.Font.Bold = true;
        //    s87.Font.FontName = "Arial";
        //    s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s87.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s90
        //    // -----------------------------------------------
        //    WorksheetStyle s90 = styles.Add("s90");
        //    s90.Font.Bold = true;
        //    s90.Font.FontName = "Arial";
        //    s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s90.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s92
        //    // -----------------------------------------------
        //    WorksheetStyle s92 = styles.Add("s92");
        //    s92.Font.Bold = true;
        //    s92.Font.Italic = true;
        //    s92.Font.FontName = "Arial";
        //    s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s92.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s93
        //    // -----------------------------------------------
        //    WorksheetStyle s93 = styles.Add("s93");
        //    s93.Font.Bold = true;
        //    s93.Font.Italic = true;
        //    s93.Font.FontName = "Arial";
        //    s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s94
        //    // -----------------------------------------------
        //    WorksheetStyle s94 = styles.Add("s94");
        //    s94.Font.Bold = true;
        //    s94.Font.FontName = "Arial";
        //    s94.Font.Color = "#000000";
        //    s94.Interior.Color = "#B0C4DE";
        //    s94.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s94.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s94.Alignment.WrapText = true;
        //    s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
        //    s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s95
        //    // -----------------------------------------------
        //    WorksheetStyle s95 = styles.Add("s95");
        //    s95.Font.FontName = "Arial";
        //    s95.Font.Color = "#000000";
        //    s95.Interior.Color = "#FFFFFF";
        //    s95.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s95.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95.Alignment.WrapText = true;
        //    s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s95B
        //    // -----------------------------------------------
        //    WorksheetStyle s95B = styles.Add("s95B");
        //    s95B.Font.FontName = "Arial";
        //    s95B.Font.Bold = true;
        //    s95B.Font.Color = "#0000FF";
        //    s95B.Interior.Color = "#FFFFFF";
        //    s95B.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95B.Alignment.WrapText = true;
        //    s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //  s95R
        //    // -----------------------------------------------
        //    WorksheetStyle s95R = styles.Add("s95R");
        //    s95R.Font.FontName = "Arial";
        //    //s95R.Font.Bold = true;
        //    s95R.Font.Color = "#FF0000";
        //    s95R.Interior.Color = "#FFFFFF";
        //    s95R.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95R.Alignment.WrapText = true;
        //    s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s95RC
        //    // -----------------------------------------------
        //    WorksheetStyle s95RC = styles.Add("s95RC");
        //    s95RC.Font.FontName = "Arial";
        //    s95RC.Font.Color = "#000000";
        //    s95RC.Interior.Color = "#FFFFFF";
        //    s95RC.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s95RC.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s95RC.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95RC.Alignment.WrapText = true;
        //    s95RC.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s95RCR
        //    // -----------------------------------------------
        //    WorksheetStyle s95RCR = styles.Add("s95RCR");
        //    s95RCR.Font.FontName = "Arial";
        //    s95RCR.Font.Color = "#FF0000";
        //    s95RCR.Interior.Color = "#FFFFFF";
        //    s95RCR.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s95RCR.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s95RCR.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95RCR.Alignment.WrapText = true;
        //    s95RCR.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

        //    // -----------------------------------------------
        //    //  s96
        //    // -----------------------------------------------
        //    WorksheetStyle s96 = styles.Add("s96");
        //    s96.Font.FontName = "Arial";
        //    s96.Font.Color = "#000000";
        //    s96.Interior.Color = "#FFFFFF";
        //    s96.Font.Bold = true;
        //    s96.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s96.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s96.Alignment.WrapText = true;
        //    s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

        //    // -----------------------------------------------
        //    //  s97
        //    // -----------------------------------------------
        //    WorksheetStyle s97 = styles.Add("s97");
        //    s97.Font.Bold = true;
        //    s97.Font.FontName = "Arial";
        //    s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s97.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s97.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s98
        //    // -----------------------------------------------
        //    WorksheetStyle s98 = styles.Add("s98");
        //    s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    // -----------------------------------------------
        //    //  s99
        //    // -----------------------------------------------
        //    WorksheetStyle s99 = styles.Add("s99");
        //    s99.Font.Bold = true;
        //    s99.Font.FontName = "Arial";
        //    s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    // -----------------------------------------------
        //    //  s100
        //    // -----------------------------------------------
        //    WorksheetStyle s100 = styles.Add("s100");
        //    s100.Font.Bold = true;
        //    s100.Font.FontName = "Arial";
        //    s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s100.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s100.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s101
        //    // -----------------------------------------------
        //    WorksheetStyle s101 = styles.Add("s101");
        //    s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s101.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s102
        //    // -----------------------------------------------
        //    WorksheetStyle s102 = styles.Add("s102");
        //    s102.Font.Bold = true;
        //    s102.Font.FontName = "Arial";
        //    s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s102.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s102.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s103
        //    // -----------------------------------------------
        //    WorksheetStyle s103 = styles.Add("s103");
        //    s103.Font.Bold = true;
        //    s103.Font.FontName = "Arial";
        //    s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s103.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s104
        //    // -----------------------------------------------
        //    WorksheetStyle s104 = styles.Add("s104");
        //    s104.Font.FontName = "Arial";
        //    // -----------------------------------------------
        //    //  s105
        //    // -----------------------------------------------
        //    WorksheetStyle s105 = styles.Add("s105");
        //    s105.Font.FontName = "Arial";
        //    s105.Font.Color = "#000000";
        //    s105.Interior.Color = "#DCE6F1";
        //    s105.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s105.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s105.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s105.Alignment.WrapText = true;
        //    s105.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s106
        //    // -----------------------------------------------
        //    WorksheetStyle s106 = styles.Add("s106");
        //    s106.Font.FontName = "Arial";
        //    s106.Font.Color = "#000000";
        //    s106.Interior.Color = "#DCE6F1";
        //    s106.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s106.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s106.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s106.Alignment.WrapText = true;
        //    s106.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s105R
        //    // -----------------------------------------------
        //    WorksheetStyle s105R = styles.Add("s105R");
        //    s105R.Font.FontName = "Arial";
        //    s105R.Font.Color = "#FF0000";
        //    s105R.Interior.Color = "#DCE6F1";
        //    s105R.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s105R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s105R.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s105R.Alignment.WrapText = true;
        //    s105R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s106R
        //    // -----------------------------------------------
        //    WorksheetStyle s106R = styles.Add("s106R");
        //    s106R.Font.FontName = "Arial";
        //    s106R.Font.Color = "#FF0000";
        //    s106R.Interior.Color = "#DCE6F1";
        //    s106R.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s106R.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s106R.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s106R.Alignment.WrapText = true;
        //    s106R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

        //    // -----------------------------------------------
        //    //  s107
        //    // -----------------------------------------------
        //    WorksheetStyle s107 = styles.Add("s107");
        //    s107.Font.FontName = "Arial";
        //    // -----------------------------------------------
        //    //  s108
        //    // -----------------------------------------------
        //    WorksheetStyle s108 = styles.Add("s108");
        //    s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
        //    s108.NumberFormat = "0%";

        //    // -----------------------------------------------
        //    //  s31
        //    // -----------------------------------------------
        //    WorksheetStyle s31 = styles.Add("s31");
        //    s31.Font.Bold = true;
        //    s31.Font.Underline = UnderlineStyle.Single;
        //    s31.Font.FontName = "Times New Roman";
        //    s31.Font.Size = 12;
        //    s31.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s31.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    // -----------------------------------------------
        //    //  s32
        //    // -----------------------------------------------
        //    WorksheetStyle s32 = styles.Add("s32");
        //    s32.Font.Bold = true;
        //    s32.Font.Underline = UnderlineStyle.Single;
        //    s32.Font.FontName = "Times New Roman";
        //    s32.Font.Size = 12;
        //    s32.Font.Color = "#000000";
        //    s32.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s32.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    //s32.NumberFormat = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
        //    // -----------------------------------------------
        //    //  s33
        //    // -----------------------------------------------
        //    WorksheetStyle s33 = styles.Add("s33");
        //    s33.Font.Bold = true;
        //    s33.Font.Underline = UnderlineStyle.Single;
        //    s33.Font.FontName = "Times New Roman";
        //    s33.Font.Size = 12;
        //    s33.Font.Color = "#000000";
        //    s33.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s33.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    // -----------------------------------------------
        //    //  s34
        //    // -----------------------------------------------
        //    WorksheetStyle s34 = styles.Add("s34");
        //    s34.Font.Bold = true;
        //    s34.Font.Underline = UnderlineStyle.Single;
        //    s34.Font.FontName = "Calibri";
        //    s34.Font.Size = 11;
        //    s34.Font.Color = "#000000";
        //    s34.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s34.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    // -----------------------------------------------
        //    //  s35
        //    // -----------------------------------------------
        //    WorksheetStyle s35 = styles.Add("s35");
        //    s35.Font.FontName = "Calibri";
        //    s35.Font.Size = 11;
        //    s35.Font.Color = "#000000";
        //    s35.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s35.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s35.Alignment.WrapText = true;
        //    // -----------------------------------------------
        //    //  s36
        //    // -----------------------------------------------
        //    WorksheetStyle s36 = styles.Add("s36");
        //    s36.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s36.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  s37
        //    // -----------------------------------------------
        //    WorksheetStyle s37 = styles.Add("s37");
        //    s37.Font.FontName = "Calibri";
        //    s37.Font.Size = 11;
        //    s37.Font.Color = "#000000";
        //    s37.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s37.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s37.Alignment.WrapText = true;
        //    // -----------------------------------------------
        //    //  s38
        //    // -----------------------------------------------
        //    WorksheetStyle s38 = styles.Add("s38");
        //    //s38.Parent = "s16";
        //    s38.Font.FontName = "Calibri";
        //    s38.Font.Size = 11;
        //    s38.Font.Color = "#000000";
        //    s38.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s38.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s38.Alignment.WrapText = true;
        //    // -----------------------------------------------
        //    //  s39
        //    // -----------------------------------------------
        //    WorksheetStyle s39 = styles.Add("s39");
        //    s39.Font.FontName = "Calibri";
        //    s39.Font.Size = 11;
        //    s39.Font.Color = "#000000";
        //    s39.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s39.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s39.Alignment.WrapText = true;
        //    // -----------------------------------------------
        //    //  s40
        //    // -----------------------------------------------
        //    WorksheetStyle s40 = styles.Add("s40");
        //    s40.Font.Bold = true;
        //    s40.Font.FontName = "Calibri";
        //    s40.Font.Size = 11;
        //    s40.Font.Color = "#000000";
        //    s40.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s40.Alignment.Vertical = StyleVerticalAlignment.Center;
        //    s40.Alignment.WrapText = true;
        //    //// -----------------------------------------------
        //    ////  s41
        //    //// -----------------------------------------------
        //    //WorksheetStyle s41 = styles.Add("s41");
        //    //s41.Font.Bold = true;
        //    //s41.Font.FontName = "Calibri";
        //    //s41.Font.Size = 11;
        //    //s41.Font.Color = "#000000";
        //    ////s41.NumberFormat = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
        //    // -----------------------------------------------
        //    //  s60
        //    // -----------------------------------------------
        //    WorksheetStyle s60 = styles.Add("s60");
        //    s60.Font.Bold = true;
        //    s60.Font.FontName = "Arial";
        //    //s60.Font.Color = "#000000";
        //    //s60.Interior.Color = "#B0C4DE";
        //    //s60.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s60.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s60.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s60.Alignment.WrapText = true;
        //    //s60.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s60.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    //s60.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    //s60.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");



        //    // -----------------------------------------------
        //    //  s61
        //    // -----------------------------------------------
        //    WorksheetStyle s61 = styles.Add("s61");
        //    s61.Font.Bold = true;
        //    s61.Font.FontName = "Arial";
        //    //s61.Font.Color = "#000000";
        //    //s61.Interior.Color = "#B0C4DE";
        //    //s61.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s61.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s61.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s61.Alignment.WrapText = true;
        //    //s61.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s61.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    //s61.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    //s61.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");

        //    // -----------------------------------------------
        //    //  s62
        //    // -----------------------------------------------
        //    WorksheetStyle s62 = styles.Add("s62");
        //    s62.Font.Bold = true;
        //    s62.Font.FontName = "Arial";
        //    s62.Font.Size = 14;
        //    s62.Font.Color = "#000000";
        //    s62.Interior.Color = "#B0C4DE";
        //    s62.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    s62.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s62.Alignment.WrapText = true;
        //    //s62.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    //s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    //s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s63
        //    // -----------------------------------------------
        //    WorksheetStyle s63 = styles.Add("s63");
        //    //s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //    // -----------------------------------------------
        //    //  s64
        //    // -----------------------------------------------
        //    WorksheetStyle s64 = styles.Add("s64");
        //    s64.Font.FontName = "Arial";
        //    //s64.Font.Color = "#000000";
        //    //s64.Interior.Color = "#FFFFFF";
        //    //s64.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s64.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s64.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s64.Alignment.WrapText = true;
        //    //s64.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //    // -----------------------------------------------
        //    //  s65
        //    // -----------------------------------------------
        //    WorksheetStyle s65 = styles.Add("s65");
        //    s65.Font.FontName = "Arial";
        //    //s65.Font.Color = "#000000";
        //    //s65.Interior.Color = "#FFFFFF";
        //    //s65.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s65.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s65.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    s65.Alignment.WrapText = true;
        //    //s65.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //    // -----------------------------------------------
        //    //  s66
        //    // -----------------------------------------------
        //    WorksheetStyle s66 = styles.Add("s66");
        //    s66.Font.FontName = "Arial";
        //    s66.Font.Color = "#FF0000";
        //    s66.Interior.Color = "#FFFFFF";
        //    s66.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s66.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s66.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s66.Alignment.WrapText = true;
        //    s66.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s66.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s66.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s66.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //    // -----------------------------------------------
        //    //  s67
        //    // -----------------------------------------------
        //    WorksheetStyle s67 = styles.Add("s67");
        //    s67.Font.FontName = "Arial";
        //    s67.Font.Color = "#FF0000";
        //    s67.Interior.Color = "#FFFFFF";
        //    s67.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s67.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s67.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s67.Alignment.WrapText = true;
        //    s67.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s67.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //    // -----------------------------------------------
        //    //  s68
        //    // -----------------------------------------------
        //    WorksheetStyle s68 = styles.Add("s68");
        //    s68.Font.FontName = "Arial";
        //    s68.Font.Color = "#000000";
        //    s68.Interior.Color = "#ffe6e6";
        //    s68.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s68.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s68.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s68.Alignment.WrapText = true;
        //    s68.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    //s68.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    //s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    //s68.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    //s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //    // -----------------------------------------------
        //    //  s70
        //    // -----------------------------------------------
        //    WorksheetStyle s70 = styles.Add("s70");
        //    s70.Font.FontName = "Arial";
        //    s70.Font.Color = "#000000";
        //    s70.Interior.Color = "#d6f5d6";
        //    s70.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s70.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s70.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s70.Alignment.WrapText = true;
        //    s70.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    s70.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //    // -----------------------------------------------
        //    //  s72
        //    // -----------------------------------------------
        //    WorksheetStyle s72 = styles.Add("s72");
        //    s72.Font.FontName = "Arial";
        //    s72.Font.Color = "#000000";
        //    s72.Font.Bold = true;
        //    s72.Interior.Color = "#d6f5d6";
        //    s72.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s72.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //    s72.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s72.Alignment.WrapText = true;
        //    s72.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    s72.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //    s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //    s72.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //    s72.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //    // -----------------------------------------------
        //    //  s137
        //    // -----------------------------------------------
        //    WorksheetStyle s137 = styles.Add("s137");
        //    s137.Name = "Normal 3";
        //    s137.Font.FontName = "Calibri";
        //    s137.Font.Size = 11;
        //    s137.Font.Color = "#000000";
        //    s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        //    // -----------------------------------------------
        //    //  m2611536909264
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909264 = styles.Add("m2611536909264");
        //    m2611536909264.Parent = "s137";
        //    m2611536909264.Font.FontName = "Arial";
        //    m2611536909264.Font.Color = "#9400D3";
        //    m2611536909264.Interior.Color = "#FFFFFF";
        //    m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909264.Alignment.WrapText = true;
        //    m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611536909284
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909284 = styles.Add("m2611536909284");
        //    m2611536909284.Parent = "s137";
        //    m2611536909284.Font.FontName = "Arial";
        //    m2611536909284.Font.Color = "#9400D3";
        //    m2611536909284.Interior.Color = "#FFFFFF";
        //    m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909284.Alignment.WrapText = true;
        //    m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");

        //    // -----------------------------------------------
        //    //  m2611536909285
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909285 = styles.Add("m2611536909285");
        //    m2611536909285.Parent = "s137";
        //    m2611536909285.Font.FontName = "Arial";
        //    m2611536909285.Font.Size = 14;
        //    m2611536909285.Font.Bold = true;
        //    m2611536909285.Font.Color = "#9400D3";
        //    m2611536909285.Interior.Color = "#FFFFFF";
        //    m2611536909285.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909285.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909285.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //    m2611536909285.Alignment.WrapText = true;
        //    m2611536909285.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909285.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");

        //    // -----------------------------------------------
        //    //  m2611536909286
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909286 = styles.Add("m2611536909286");
        //    m2611536909286.Parent = "s137";
        //    m2611536909286.Font.FontName = "Arial";
        //    m2611536909286.Font.Color = "#9400D3";
        //    m2611536909286.Interior.Color = "#FFFFFF";
        //    m2611536909286.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909286.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909286.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    m2611536909286.Alignment.WrapText = true;
        //    m2611536909286.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909286.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611536909304
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909304 = styles.Add("m2611536909304");
        //    m2611536909304.Parent = "s137";
        //    m2611536909304.Font.FontName = "Arial";
        //    m2611536909304.Font.Color = "#9400D3";
        //    m2611536909304.Interior.Color = "#FFFFFF";
        //    m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909304.Alignment.WrapText = true;
        //    m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611536909324
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909324 = styles.Add("m2611536909324");
        //    m2611536909324.Parent = "s137";
        //    m2611536909324.Font.FontName = "Arial";
        //    m2611536909324.Font.Color = "#9400D3";
        //    m2611536909324.Interior.Color = "#FFFFFF";
        //    m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909324.Alignment.WrapText = true;
        //    m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611536909344
        //    // -----------------------------------------------
        //    WorksheetStyle m2611536909344 = styles.Add("m2611536909344");
        //    m2611536909344.Parent = "s137";
        //    m2611536909344.Font.FontName = "Arial";
        //    m2611536909344.Font.Color = "#9400D3";
        //    m2611536909344.Interior.Color = "#FFFFFF";
        //    m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611536909344.Alignment.WrapText = true;
        //    m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549552
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549552 = styles.Add("m2611540549552");
        //    m2611540549552.Parent = "s137";
        //    m2611540549552.Font.FontName = "Arial";
        //    m2611540549552.Font.Color = "#9400D3";
        //    m2611540549552.Interior.Color = "#FFFFFF";
        //    m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549552.Alignment.WrapText = true;
        //    m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549572
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549572 = styles.Add("m2611540549572");
        //    m2611540549572.Parent = "s137";
        //    m2611540549572.Font.FontName = "Arial";
        //    m2611540549572.Font.Color = "#9400D3";
        //    m2611540549572.Interior.Color = "#FFFFFF";
        //    m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549572.Alignment.WrapText = true;
        //    m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
        //    m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549592
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549592 = styles.Add("m2611540549592");
        //    m2611540549592.Parent = "s137";
        //    m2611540549592.Font.FontName = "Arial";
        //    m2611540549592.Font.Color = "#9400D3";
        //    m2611540549592.Interior.Color = "#FFFFFF";
        //    m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549592.Alignment.WrapText = true;
        //    m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549612
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549612 = styles.Add("m2611540549612");
        //    m2611540549612.Parent = "s137";
        //    m2611540549612.Font.FontName = "Arial";
        //    m2611540549612.Font.Color = "#9400D3";
        //    m2611540549612.Interior.Color = "#FFFFFF";
        //    m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549612.Alignment.WrapText = true;
        //    m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549632
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549632 = styles.Add("m2611540549632");
        //    m2611540549632.Parent = "s137";
        //    m2611540549632.Font.FontName = "Arial";
        //    m2611540549632.Font.Color = "#9400D3";
        //    m2611540549632.Interior.Color = "#FFFFFF";
        //    m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549632.Alignment.WrapText = true;
        //    m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549652
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549652 = styles.Add("m2611540549652");
        //    m2611540549652.Parent = "s137";
        //    m2611540549652.Font.FontName = "Arial";
        //    m2611540549652.Font.Color = "#9400D3";
        //    m2611540549652.Interior.Color = "#FFFFFF";
        //    m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549652.Alignment.WrapText = true;
        //    m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  m2611540549672
        //    // -----------------------------------------------
        //    WorksheetStyle m2611540549672 = styles.Add("m2611540549672");
        //    m2611540549672.Parent = "s137";
        //    m2611540549672.Font.FontName = "Arial";
        //    m2611540549672.Font.Color = "#9400D3";
        //    m2611540549672.Interior.Color = "#FFFFFF";
        //    m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
        //    m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    m2611540549672.Alignment.WrapText = true;
        //    m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");

        //    // -----------------------------------------------
        //    //  s139
        //    // -----------------------------------------------
        //    WorksheetStyle s139 = styles.Add("s139");
        //    s139.Parent = "s137";
        //    s139.Font.FontName = "Calibri";
        //    s139.Font.Size = 11;
        //    s139.Interior.Color = "#FFFFFF";
        //    s139.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s139.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s139.Alignment.WrapText = true;
        //    // -----------------------------------------------
        //    //  s140
        //    // -----------------------------------------------
        //    WorksheetStyle s140 = styles.Add("s140");
        //    s140.Parent = "s137";
        //    s140.Font.FontName = "Calibri";
        //    s140.Font.Size = 11;
        //    s140.Interior.Color = "#FFFFFF";
        //    s140.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s140.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s140.Alignment.WrapText = true;
        //    s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s141
        //    // -----------------------------------------------
        //    WorksheetStyle s141 = styles.Add("s141");
        //    s141.Parent = "s137";
        //    s141.Font.FontName = "Calibri";
        //    s141.Font.Size = 11;
        //    s141.Interior.Color = "#FFFFFF";
        //    s141.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s141.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s141.Alignment.WrapText = true;
        //    s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s142
        //    // -----------------------------------------------
        //    WorksheetStyle s142 = styles.Add("s142");
        //    s142.Parent = "s137";
        //    s142.Font.FontName = "Calibri";
        //    s142.Font.Size = 11;
        //    s142.Interior.Color = "#FFFFFF";
        //    s142.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s142.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s142.Alignment.WrapText = true;
        //    s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s143
        //    // -----------------------------------------------
        //    WorksheetStyle s143 = styles.Add("s143");
        //    s143.Parent = "s137";
        //    s143.Font.FontName = "Calibri";
        //    s143.Font.Size = 11;
        //    s143.Interior.Color = "#FFFFFF";
        //    s143.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s143.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s143.Alignment.WrapText = true;
        //    s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s144
        //    // -----------------------------------------------
        //    WorksheetStyle s144 = styles.Add("s144");
        //    s144.Parent = "s137";
        //    s144.Font.FontName = "Arial";
        //    s144.Font.Color = "#9400D3";
        //    s144.Interior.Color = "#FFFFFF";
        //    s144.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s144.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s144.Alignment.WrapText = true;
        //    s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s145
        //    // -----------------------------------------------
        //    WorksheetStyle s145 = styles.Add("s145");
        //    s145.Parent = "s137";
        //    s145.Font.FontName = "Calibri";
        //    s145.Font.Size = 11;
        //    s145.Interior.Color = "#FFFFFF";
        //    s145.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s145.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s145.Alignment.WrapText = true;
        //    s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s146
        //    // -----------------------------------------------
        //    WorksheetStyle s146 = styles.Add("s146");
        //    s146.Parent = "s137";
        //    s146.Font.FontName = "Calibri";
        //    s146.Font.Size = 11;
        //    s146.Interior.Color = "#FFFFFF";
        //    s146.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s146.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s146.Alignment.WrapText = true;
        //    s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
        //    s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s169
        //    // -----------------------------------------------
        //    WorksheetStyle s169 = styles.Add("s169");
        //    s169.Parent = "s137";
        //    s169.Font.FontName = "Arial";
        //    s169.Font.Color = "#9400D3";
        //    s169.Interior.Color = "#FFFFFF";
        //    s169.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s169.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s170
        //    // -----------------------------------------------
        //    WorksheetStyle s170 = styles.Add("s170");
        //    s170.Parent = "s137";
        //    s170.Font.FontName = "Calibri";
        //    s170.Font.Size = 11;
        //    s170.Interior.Color = "#FFFFFF";
        //    s170.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s170.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    // -----------------------------------------------
        //    //  s171
        //    // -----------------------------------------------
        //    WorksheetStyle s171 = styles.Add("s171");
        //    s171.Parent = "s137";
        //    s171.Font.FontName = "Calibri";
        //    s171.Font.Size = 11;
        //    s171.Interior.Color = "#FFFFFF";
        //    s171.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s171.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s172
        //    // -----------------------------------------------
        //    WorksheetStyle s172 = styles.Add("s172");
        //    s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

        //    // -----------------------------------------------
        //    //  s169
        //    // -----------------------------------------------
        //    WorksheetStyle s169H = styles.Add("s169H");
        //    s169H.Parent = "s137";
        //    s169H.Font.FontName = "Arial";
        //    s169H.Font.Color = "#9400D3";
        //    s169H.Interior.Color = "#FFFFFF";
        //    s169H.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s169H.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s169H.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s170
        //    // -----------------------------------------------
        //    WorksheetStyle s170H = styles.Add("s170H");
        //    s170H.Parent = "s137";
        //    s170H.Font.FontName = "Calibri";
        //    s170H.Font.Size = 11;
        //    s170H.Interior.Color = "#FFFFFF";
        //    s170H.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s170H.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    // -----------------------------------------------
        //    //  s171
        //    // -----------------------------------------------
        //    WorksheetStyle s171H = styles.Add("s171H");
        //    s171H.Parent = "s137";
        //    s171H.Font.FontName = "Calibri";
        //    s171H.Font.Size = 11;
        //    s171H.Interior.Color = "#FFFFFF";
        //    s171H.Interior.Pattern = StyleInteriorPattern.Solid;
        //    s171H.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s171H.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
        //    // -----------------------------------------------
        //    //  s172
        //    // -----------------------------------------------
        //    WorksheetStyle s172H = styles.Add("s172H");
        //    s172H.Alignment.Vertical = StyleVerticalAlignment.Bottom;

        //}


        //private void On_SaveExcelAudit_Closed(string PdfFileName)
        //{
        //    Random_Filename = null;
        //    string PdfName = "Pdf File";
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfFileName+"_Audit";
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".xls";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".xls";



        //        Workbook book = new Workbook();

        //        this.GenerateStyles(book.Styles);



        //        bool First = true; string MS_Type = string.Empty;
        //        string CAMSDesc = null; string Branch = null, Priv_Branch = null, SP_Plan_desc = null;
        //        bool SPNum = true; Worksheet sheet; WorksheetCell cell; WorksheetRow Row0; int Count = 1;


        //        if (dtAudit.Rows.Count > 0)
        //        {
        //            this.GenerateWorksheetParameters(book.Worksheets);

        //            string PrivSP = string.Empty;

        //            var distinctRows = (from DataRow dRow in dtAudit.Rows
        //                                select dRow["SP"]).Distinct();

        //            foreach (var dRow in distinctRows)
        //            {
        //                DataTable dataTable = new DataTable();
        //                DataView dv = new DataView(dtAudit);
        //                dv.RowFilter = "SP='" + dRow + "'";
        //                dataTable = dv.ToTable();

        //                if (dataTable.Rows.Count > 0)
        //                {
        //                    string ReportName = dataTable.Rows[0]["SP_Desc"].ToString();
        //                    ReportName = ReportName.Replace("/", "");

        //                    if (ReportName.Length >= 31)
        //                    {
        //                        ReportName = ReportName.Substring(0, 31);
        //                    }
        //                    sheet = book.Worksheets.Add(ReportName);
        //                    sheet.Table.DefaultRowHeight = 14.25F;


        //                    sheet.Table.Columns.Add(75);
        //                    sheet.Table.Columns.Add(75);
        //                    sheet.Table.Columns.Add(75);
        //                    sheet.Table.Columns.Add(75);
        //                    sheet.Table.Columns.Add(80);
        //                    sheet.Table.Columns.Add(145);
        //                    sheet.Table.Columns.Add(75);
        //                    sheet.Table.Columns.Add(80);
        //                    sheet.Table.Columns.Add(180);
        //                    sheet.Table.Columns.Add(80);
        //                    sheet.Table.Columns.Add(80);

        //                Row0 = sheet.Table.Rows.Add();
        //                    Row0.AutoFitHeight = false;
        //                    //WorksheetCell cell;
        //                    cell = Row0.Cells.Add();
        //                    cell.StyleID = "s83";
        //                    cell.Data.Type = DataType.String;
        //                    cell.Data.Text = dataTable.Rows[0]["SP_Desc"].ToString();
        //                    cell.MergeAcross = 9;


        //                    Row0 = sheet.Table.Rows.Add();

        //                    cell = Row0.Cells.Add("Agency", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Dept", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Program", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Year", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("App", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Name", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Type", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Code", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Description", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Date", DataType.String, "s94");
        //                    cell = Row0.Cells.Add("Amount", DataType.String, "s94");


        //                int i = 0;
        //                    foreach (DataRow dr in dataTable.Rows)
        //                    {
        //                        Row0 = sheet.Table.Rows.Add();


        //                        string CAMSType = string.Empty;
        //                        if (dr["SP2_TYPE"].ToString().Trim() == "CA") CAMSType = "Service"; else CAMSType = "Outcome";



        //                    if (i % 2 == 0)
        //                    {
        //                        cell = Row0.Cells.Add(dr["Agency"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["Dept"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["Program"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["Prog_Year"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["AppNo"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["Name"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["CAMS_CODE"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["CAMS_DATE"].ToString()), DataType.String, "s105");
        //                        cell = Row0.Cells.Add(dr["CASEMS_COST"].ToString(), DataType.String, "s106");



        //                    }
        //                    else
        //                    {

        //                        cell = Row0.Cells.Add(dr["Agency"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["Dept"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["Program"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["Prog_Year"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["AppNo"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["Name"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(CAMSType, DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["CAMS_CODE"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["CAMS_DESC"].ToString(), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(LookupDataAccess.Getdate(dr["CAMS_DATE"].ToString()), DataType.String, "s95");
        //                        cell = Row0.Cells.Add(dr["CASEMS_COST"].ToString(), DataType.String, "s95RC");


        //                    }




        //                        //if (dr["SP0_SERVICECODE"].ToString() != PrivSP)
        //                        //{



        //                        //    PrivSP = dr["SP0_SERVICECODE"].ToString();
        //                        //}


        //                        i++;
        //                    }
        //                }


        //            }



        //        }


        //        FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        book.Save(stream);
        //        stream.Close();

        //    AlertBox.Show(PdfFileName + "_Audit.xls file Generated Successfully");

        //        ////FileDownloadGateway downloadGateway = new FileDownloadGateway();
        //        ////downloadGateway.Filename = "SPREPAPP_Report.xls";

        //        ////// downloadGateway.Version = file.Version;

        //        ////downloadGateway.SetContentType(DownloadContentType.OctetStream);

        //        ////downloadGateway.StartFileDownload(new ContainerControl(), PdfName);

        //        //FileInfo fiDownload = new FileInfo(PdfName);
        //        ///// Need to check for file exists, is local file, is allow to read, etc...
        //        //string name = fiDownload.Name;
        //        //using (FileStream fileStream = fiDownload.OpenRead())
        //        //{
        //        //    Application.Download(fileStream, name);
        //        //}

        //    //}

        //}



        private void GenerateStyles1(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Name = "Percent";
            s18.NumberFormat = "0%";
            // -----------------------------------------------
            //  m137371072
            // -----------------------------------------------
            WorksheetStyle m137371072 = styles.Add("m137371072");
            m137371072.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371072.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371072.Alignment.WrapText = true;
            m137371072.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371072.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371072.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371112
            // -----------------------------------------------
            WorksheetStyle m137371112 = styles.Add("m137371112");
            m137371112.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371112.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371112.Alignment.WrapText = true;
            m137371112.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371112.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371112.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371192
            // -----------------------------------------------
            WorksheetStyle m137371192 = styles.Add("m137371192");
            m137371192.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371192.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371192.Alignment.WrapText = true;
            m137371192.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371192.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371192.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371232
            // -----------------------------------------------
            WorksheetStyle m137371232 = styles.Add("m137371232");
            m137371232.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371232.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371232.Alignment.WrapText = true;
            m137371232.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371232.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371232.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371272
            // -----------------------------------------------
            WorksheetStyle m137371272 = styles.Add("m137371272");
            m137371272.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371272.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371272.Alignment.WrapText = true;
            m137371272.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371272.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371272.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371292
            // -----------------------------------------------
            WorksheetStyle m137371292 = styles.Add("m137371292");
            m137371292.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371292.Alignment.Vertical = StyleVerticalAlignment.Top;
            m137371292.Alignment.WrapText = true;
            m137371292.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371292.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137371292.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371332
            // -----------------------------------------------
            WorksheetStyle m137371332 = styles.Add("m137371332");
            m137371332.Interior.Color = "#FFFFFF";
            m137371332.Interior.Pattern = StyleInteriorPattern.Solid;
            m137371332.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371332.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371332.Alignment.WrapText = true;
            m137371332.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371332.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371332.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137371372
            // -----------------------------------------------
            WorksheetStyle m137371372 = styles.Add("m137371372");
            m137371372.Interior.Color = "#FFFFFF";
            m137371372.Interior.Pattern = StyleInteriorPattern.Solid;
            m137371372.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137371372.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137371372.Alignment.WrapText = true;
            m137371372.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137371372.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137371372.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370752
            // -----------------------------------------------
            WorksheetStyle m137370752 = styles.Add("m137370752");
            m137370752.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370752.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370752.Alignment.WrapText = true;
            m137370752.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370752.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137370752.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370792
            // -----------------------------------------------
            WorksheetStyle m137370792 = styles.Add("m137370792");
            m137370792.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370792.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370792.Alignment.WrapText = true;
            m137370792.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370792.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137370792.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370832
            // -----------------------------------------------
            WorksheetStyle m137370832 = styles.Add("m137370832");
            m137370832.Interior.Color = "#FFFFFF";
            m137370832.Interior.Pattern = StyleInteriorPattern.Solid;
            m137370832.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370832.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370832.Alignment.WrapText = true;
            m137370832.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370832.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137370832.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370892
            // -----------------------------------------------
            WorksheetStyle m137370892 = styles.Add("m137370892");
            m137370892.Interior.Color = "#FFFFFF";
            m137370892.Interior.Pattern = StyleInteriorPattern.Solid;
            m137370892.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370892.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370892.Alignment.WrapText = true;
            m137370892.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370892.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137370892.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370432
            // -----------------------------------------------
            WorksheetStyle m137370432 = styles.Add("m137370432");
            m137370432.Font.Bold = true;
            m137370432.Font.FontName = "Calibri";
            m137370432.Font.Size = 11;
            m137370432.Interior.Color = "#C5D9F1";
            m137370432.Interior.Pattern = StyleInteriorPattern.Solid;
            m137370432.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370432.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370432.Alignment.WrapText = true;
            m137370432.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370432.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370432.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137370432.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370452
            // -----------------------------------------------
            WorksheetStyle m137370452 = styles.Add("m137370452");
            m137370452.Font.Bold = true;
            m137370452.Font.FontName = "Calibri";
            m137370452.Font.Size = 11;
            m137370452.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370452.Alignment.Vertical = StyleVerticalAlignment.Center;
            m137370452.Alignment.WrapText = true;
            m137370452.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370452.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137370452.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370492
            // -----------------------------------------------
            WorksheetStyle m137370492 = styles.Add("m137370492");
            m137370492.Font.Bold = true;
            m137370492.Font.FontName = "Calibri";
            m137370492.Font.Size = 11;
            m137370492.Font.Color = "#000000";
            m137370492.Interior.Color = "#C5D9F1";
            m137370492.Interior.Pattern = StyleInteriorPattern.Solid;
            m137370492.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m137370492.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370492.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370492.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370492.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137370492.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370512
            // -----------------------------------------------
            WorksheetStyle m137370512 = styles.Add("m137370512");
            m137370512.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m137370512.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370512.Alignment.WrapText = true;
            m137370512.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370512.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370512.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137370512.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370532
            // -----------------------------------------------
            WorksheetStyle m137370532 = styles.Add("m137370532");
            m137370532.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m137370532.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137370532.Alignment.WrapText = true;
            m137370532.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370532.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370532.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137370532.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137370192
            // -----------------------------------------------
            WorksheetStyle m137370192 = styles.Add("m137370192");
            m137370192.Font.FontName = "Calibri";
            m137370192.Font.Size = 11;
            m137370192.Interior.Color = "#C5D9F1";
            m137370192.Interior.Pattern = StyleInteriorPattern.Solid;
            m137370192.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m137370192.Alignment.Vertical = StyleVerticalAlignment.Center;
            m137370192.Alignment.WrapText = true;
            m137370192.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137370192.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137370192.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m137370192.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137369872
            // -----------------------------------------------
            WorksheetStyle m137369872 = styles.Add("m137369872");
            m137369872.Font.Bold = true;
            m137369872.Font.FontName = "Calibri";
            m137369872.Font.Size = 11;
            m137369872.Font.Color = "#000000";
            m137369872.Interior.Color = "#C5D9F1";
            m137369872.Interior.Pattern = StyleInteriorPattern.Solid;
            m137369872.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m137369872.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137369872.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137369872.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137369872.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137369872.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137369912
            // -----------------------------------------------
            WorksheetStyle m137369912 = styles.Add("m137369912");
            m137369912.Font.FontName = "Calibri";
            m137369912.Font.Size = 11;
            m137369912.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m137369912.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m137369912.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137369912.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137369912.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m263624672
            // -----------------------------------------------
            WorksheetStyle m263624672 = styles.Add("m263624672");
            m263624672.Font.Bold = true;
            m263624672.Font.FontName = "Calibri";
            m263624672.Font.Size = 11;
            m263624672.Interior.Color = "#C5D9F1";
            m263624672.Interior.Pattern = StyleInteriorPattern.Solid;
            m263624672.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m263624672.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m263624672.Alignment.WrapText = true;
            m263624672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m263624672.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m263624692
            // -----------------------------------------------
            WorksheetStyle m263624692 = styles.Add("m263624692");
            m263624692.Font.Bold = true;
            m263624692.Font.FontName = "Calibri";
            m263624692.Font.Size = 11;
            m263624692.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m263624692.Alignment.Vertical = StyleVerticalAlignment.Center;
            m263624692.Alignment.WrapText = true;
            m263624692.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m263624692.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m263624692.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m263624692.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.Underline = UnderlineStyle.Single;
            s21.Font.FontName = "Calibri";
            s21.Font.Size = 11;
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Font.FontName = "Calibri";
            s22.Font.Size = 11;
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            s23.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s23.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Font.Bold = true;
            s26.Font.FontName = "Calibri";
            s26.Font.Size = 11;
            // -----------------------------------------------
            //  s29
            // -----------------------------------------------
            WorksheetStyle s29 = styles.Add("s29");
            s29.Font.FontName = "Calibri";
            s29.Font.Size = 11;
            s29.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s29.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s35
            // -----------------------------------------------
            WorksheetStyle s35 = styles.Add("s35");
            s35.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s35.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s35.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s44
            // -----------------------------------------------
            WorksheetStyle s44 = styles.Add("s44");
            s44.Font.Bold = true;
            s44.Font.FontName = "Calibri";
            s44.Font.Size = 11;
            s44.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s44.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s46
            // -----------------------------------------------
            WorksheetStyle s46 = styles.Add("s46");
            s46.Font.FontName = "Calibri";
            s46.Font.Size = 11;
            s46.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s46.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s46.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s47
            // -----------------------------------------------
            WorksheetStyle s47 = styles.Add("s47");
            s47.Font.FontName = "Calibri";
            s47.Font.Size = 11;
            s47.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s47.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s48
            // -----------------------------------------------
            WorksheetStyle s48 = styles.Add("s48");
            // -----------------------------------------------
            //  s49
            // -----------------------------------------------
            WorksheetStyle s49 = styles.Add("s49");
            s49.Font.Bold = true;
            s49.Font.FontName = "Calibri";
            s49.Font.Size = 11;
            s49.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s49.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s50
            // -----------------------------------------------
            WorksheetStyle s50 = styles.Add("s50");
            s50.Font.FontName = "Calibri";
            s50.Font.Size = 11;
            s50.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s50.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s50.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s51
            // -----------------------------------------------
            WorksheetStyle s51 = styles.Add("s51");
            s51.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s117
            // -----------------------------------------------
            WorksheetStyle s117 = styles.Add("s117");
            // -----------------------------------------------
            //  s119
            // -----------------------------------------------
            WorksheetStyle s119 = styles.Add("s119");
            s119.Font.FontName = "Calibri";
            s119.Font.Size = 11;
            s119.Interior.Color = "#C5D9F1";
            s119.Interior.Pattern = StyleInteriorPattern.Solid;
            s119.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s119.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s119.Alignment.WrapText = true;
            s119.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s119.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s125
            // -----------------------------------------------
            WorksheetStyle s125 = styles.Add("s125");
            s125.Font.Bold = true;
            s125.Font.FontName = "Calibri";
            s125.Font.Size = 11;
            s125.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s125.Alignment.Vertical = StyleVerticalAlignment.Center;
            s125.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s137
            // -----------------------------------------------
            WorksheetStyle s137 = styles.Add("s137");
            s137.Name = "Normal 3";
            s137.Font.FontName = "Calibri";
            s137.Font.Size = 11;
            s137.Font.Color = "#000000";
            s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m2611536909264
            // -----------------------------------------------
            WorksheetStyle m2611536909264 = styles.Add("m2611536909264");
            m2611536909264.Parent = "s137";
            m2611536909264.Font.FontName = "Arial";
            m2611536909264.Font.Color = "#9400D3";
            m2611536909264.Interior.Color = "#FFFFFF";
            m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909264.Alignment.WrapText = true;
            m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909284
            // -----------------------------------------------
            WorksheetStyle m2611536909284 = styles.Add("m2611536909284");
            m2611536909284.Parent = "s137";
            m2611536909284.Font.FontName = "Arial";
            m2611536909284.Font.Color = "#9400D3";
            m2611536909284.Interior.Color = "#FFFFFF";
            m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909284.Alignment.WrapText = true;
            m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909304
            // -----------------------------------------------
            WorksheetStyle m2611536909304 = styles.Add("m2611536909304");
            m2611536909304.Parent = "s137";
            m2611536909304.Font.FontName = "Arial";
            m2611536909304.Font.Color = "#9400D3";
            m2611536909304.Interior.Color = "#FFFFFF";
            m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909304.Alignment.WrapText = true;
            m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909324
            // -----------------------------------------------
            WorksheetStyle m2611536909324 = styles.Add("m2611536909324");
            m2611536909324.Parent = "s137";
            m2611536909324.Font.FontName = "Arial";
            m2611536909324.Font.Color = "#9400D3";
            m2611536909324.Interior.Color = "#FFFFFF";
            m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909324.Alignment.WrapText = true;
            m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909344
            // -----------------------------------------------
            WorksheetStyle m2611536909344 = styles.Add("m2611536909344");
            m2611536909344.Parent = "s137";
            m2611536909344.Font.FontName = "Arial";
            m2611536909344.Font.Color = "#9400D3";
            m2611536909344.Interior.Color = "#FFFFFF";
            m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909344.Alignment.WrapText = true;
            m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549552
            // -----------------------------------------------
            WorksheetStyle m2611540549552 = styles.Add("m2611540549552");
            m2611540549552.Parent = "s137";
            m2611540549552.Font.FontName = "Arial";
            m2611540549552.Font.Color = "#9400D3";
            m2611540549552.Interior.Color = "#FFFFFF";
            m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549552.Alignment.WrapText = true;
            m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549572
            // -----------------------------------------------
            WorksheetStyle m2611540549572 = styles.Add("m2611540549572");
            m2611540549572.Parent = "s137";
            m2611540549572.Font.FontName = "Arial";
            m2611540549572.Font.Color = "#9400D3";
            m2611540549572.Interior.Color = "#FFFFFF";
            m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549572.Alignment.WrapText = true;
            m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549592
            // -----------------------------------------------
            WorksheetStyle m2611540549592 = styles.Add("m2611540549592");
            m2611540549592.Parent = "s137";
            m2611540549592.Font.FontName = "Arial";
            m2611540549592.Font.Color = "#9400D3";
            m2611540549592.Interior.Color = "#FFFFFF";
            m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549592.Alignment.WrapText = true;
            m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549612
            // -----------------------------------------------
            WorksheetStyle m2611540549612 = styles.Add("m2611540549612");
            m2611540549612.Parent = "s137";
            m2611540549612.Font.FontName = "Arial";
            m2611540549612.Font.Color = "#9400D3";
            m2611540549612.Interior.Color = "#FFFFFF";
            m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549612.Alignment.WrapText = true;
            m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549632
            // -----------------------------------------------
            WorksheetStyle m2611540549632 = styles.Add("m2611540549632");
            m2611540549632.Parent = "s137";
            m2611540549632.Font.FontName = "Arial";
            m2611540549632.Font.Color = "#9400D3";
            m2611540549632.Interior.Color = "#FFFFFF";
            m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549632.Alignment.WrapText = true;
            m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549652
            // -----------------------------------------------
            WorksheetStyle m2611540549652 = styles.Add("m2611540549652");
            m2611540549652.Parent = "s137";
            m2611540549652.Font.FontName = "Arial";
            m2611540549652.Font.Color = "#9400D3";
            m2611540549652.Interior.Color = "#FFFFFF";
            m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549652.Alignment.WrapText = true;
            m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549672
            // -----------------------------------------------
            WorksheetStyle m2611540549672 = styles.Add("m2611540549672");
            m2611540549672.Parent = "s137";
            m2611540549672.Font.FontName = "Arial";
            m2611540549672.Font.Color = "#9400D3";
            m2611540549672.Interior.Color = "#FFFFFF";
            m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549672.Alignment.WrapText = true;
            m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s139
            // -----------------------------------------------
            WorksheetStyle s139 = styles.Add("s139");
            s139.Parent = "s137";
            s139.Font.FontName = "Calibri";
            s139.Font.Size = 11;
            s139.Interior.Color = "#FFFFFF";
            s139.Interior.Pattern = StyleInteriorPattern.Solid;
            s139.Alignment.Vertical = StyleVerticalAlignment.Top;
            s139.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s140
            // -----------------------------------------------
            WorksheetStyle s140 = styles.Add("s140");
            s140.Parent = "s137";
            s140.Font.FontName = "Calibri";
            s140.Font.Size = 11;
            s140.Interior.Color = "#FFFFFF";
            s140.Interior.Pattern = StyleInteriorPattern.Solid;
            s140.Alignment.Vertical = StyleVerticalAlignment.Top;
            s140.Alignment.WrapText = true;
            s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s141
            // -----------------------------------------------
            WorksheetStyle s141 = styles.Add("s141");
            s141.Parent = "s137";
            s141.Font.FontName = "Calibri";
            s141.Font.Size = 11;
            s141.Interior.Color = "#FFFFFF";
            s141.Interior.Pattern = StyleInteriorPattern.Solid;
            s141.Alignment.Vertical = StyleVerticalAlignment.Top;
            s141.Alignment.WrapText = true;
            s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s142
            // -----------------------------------------------
            WorksheetStyle s142 = styles.Add("s142");
            s142.Parent = "s137";
            s142.Font.FontName = "Calibri";
            s142.Font.Size = 11;
            s142.Interior.Color = "#FFFFFF";
            s142.Interior.Pattern = StyleInteriorPattern.Solid;
            s142.Alignment.Vertical = StyleVerticalAlignment.Top;
            s142.Alignment.WrapText = true;
            s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s143w
            // -----------------------------------------------
            WorksheetStyle s143w = styles.Add("s143w");
            s143w.Parent = "s137";
            s143w.Font.FontName = "Calibri";
            s143w.Font.Size = 11;
            s143w.Interior.Color = "#FFFFFF";
            s143w.Interior.Pattern = StyleInteriorPattern.Solid;
            s143w.Alignment.Vertical = StyleVerticalAlignment.Top;
            s143w.Alignment.WrapText = true;
            s143w.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s144
            // -----------------------------------------------
            WorksheetStyle s144 = styles.Add("s144");
            s144.Parent = "s137";
            s144.Font.FontName = "Arial";
            s144.Font.Color = "#9400D3";
            s144.Interior.Color = "#FFFFFF";
            s144.Interior.Pattern = StyleInteriorPattern.Solid;
            s144.Alignment.Vertical = StyleVerticalAlignment.Top;
            s144.Alignment.WrapText = true;
            s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s145w
            // -----------------------------------------------
            WorksheetStyle s145w = styles.Add("s145w");
            s145w.Parent = "s137";
            s145w.Font.FontName = "Calibri";
            s145w.Font.Size = 11;
            s145w.Interior.Color = "#FFFFFF";
            s145w.Interior.Pattern = StyleInteriorPattern.Solid;
            s145w.Alignment.Vertical = StyleVerticalAlignment.Top;
            s145w.Alignment.WrapText = true;
            s145w.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s146
            // -----------------------------------------------
            WorksheetStyle s146 = styles.Add("s146");
            s146.Parent = "s137";
            s146.Font.FontName = "Calibri";
            s146.Font.Size = 11;
            s146.Interior.Color = "#FFFFFF";
            s146.Interior.Pattern = StyleInteriorPattern.Solid;
            s146.Alignment.Vertical = StyleVerticalAlignment.Top;
            s146.Alignment.WrapText = true;
            s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");

            // -----------------------------------------------
            //  s143
            // -----------------------------------------------
            WorksheetStyle s143 = styles.Add("s143");
            s143.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s143.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s143.Alignment.WrapText = true;
            s143.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s143.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s143.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = styles.Add("s145");
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s145.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s145.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s145.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s145.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s147
            // -----------------------------------------------
            WorksheetStyle s147 = styles.Add("s147");
            s147.Parent = "s18";
            s147.Font.Bold = true;
            s147.Font.FontName = "Calibri";
            s147.Font.Size = 11;
            s147.Font.Color = "#000000";
            s147.Interior.Color = "#C5D9F1";
            s147.Interior.Pattern = StyleInteriorPattern.Solid;
            s147.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s147.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s147.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s147.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s147.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s147.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s160
            // -----------------------------------------------
            WorksheetStyle s160 = styles.Add("s160");
            s160.Font.Bold = true;
            s160.Font.FontName = "Calibri";
            s160.Font.Size = 11;
            s160.Interior.Color = "#C5D9F1";
            s160.Interior.Pattern = StyleInteriorPattern.Solid;
            s160.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s160.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s160.Alignment.WrapText = true;
            s160.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s160.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s160.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s160.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s161
            // -----------------------------------------------
            WorksheetStyle s161 = styles.Add("s161");
            s161.Font.FontName = "Calibri";
            s161.Font.Size = 11;
            s161.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s161.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s161.Alignment.WrapText = true;
            s161.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s161.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s161.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s161.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s162
            // -----------------------------------------------
            WorksheetStyle s162 = styles.Add("s162");
            s162.Font.FontName = "Calibri";
            s162.Font.Size = 11;
            s162.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s162.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s162.Alignment.WrapText = true;
            s162.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s162.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s162.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s162.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s163
            // -----------------------------------------------
            WorksheetStyle s163 = styles.Add("s163");
            s163.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s163.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s163.Alignment.WrapText = true;
            s163.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s163.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s163.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s169
            // -----------------------------------------------
            WorksheetStyle s169 = styles.Add("s169");
            s169.Parent = "s137";
            s169.Font.FontName = "Arial";
            s169.Font.Color = "#9400D3";
            s169.Interior.Color = "#FFFFFF";
            s169.Interior.Pattern = StyleInteriorPattern.Solid;
            s169.Alignment.Vertical = StyleVerticalAlignment.Top;
            s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170 = styles.Add("s170");
            s170.Parent = "s137";
            s170.Font.FontName = "Calibri";
            s170.Font.Size = 11;
            s170.Interior.Color = "#FFFFFF";
            s170.Interior.Pattern = StyleInteriorPattern.Solid;
            s170.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = styles.Add("s171");
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 11;
            s171.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s171.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s171w
            // -----------------------------------------------
            WorksheetStyle s171w = styles.Add("s171w");
            s171w.Parent = "s137";
            s171w.Font.FontName = "Calibri";
            s171w.Font.Size = 11;
            s171w.Interior.Color = "#FFFFFF";
            s171w.Interior.Pattern = StyleInteriorPattern.Solid;
            s171w.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171w.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172 = styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

            // -----------------------------------------------
            //  s169H
            // -----------------------------------------------
            WorksheetStyle s169H = styles.Add("s169H");
            s169H.Parent = "s137";
            s169H.Font.FontName = "Arial";
            s169H.Font.Color = "#9400D3";
            s169H.Interior.Color = "#FFFFFF";
            s169H.Interior.Pattern = StyleInteriorPattern.Solid;
            s169H.Alignment.Vertical = StyleVerticalAlignment.Top;
            s169H.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s170H
            // -----------------------------------------------
            WorksheetStyle s170H = styles.Add("s170H");
            s170H.Parent = "s137";
            s170H.Font.FontName = "Calibri";
            s170H.Font.Size = 11;
            s170H.Interior.Color = "#FFFFFF";
            s170H.Interior.Pattern = StyleInteriorPattern.Solid;
            s170H.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171H
            // -----------------------------------------------
            WorksheetStyle s171H = styles.Add("s171H");
            s171H.Parent = "s137";
            s171H.Font.FontName = "Calibri";
            s171H.Font.Size = 11;
            s171H.Interior.Color = "#FFFFFF";
            s171H.Interior.Pattern = StyleInteriorPattern.Solid;
            s171H.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171H.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172H
            // -----------------------------------------------
            WorksheetStyle s172H = styles.Add("s172H");
            s172H.Alignment.Vertical = StyleVerticalAlignment.Bottom;

            // -----------------------------------------------
            //  s180
            // -----------------------------------------------
            WorksheetStyle s180 = styles.Add("s180");
            s180.Font.FontName = "Calibri";
            s180.Font.Size = 11;
            s180.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s180.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s180.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s192
            // -----------------------------------------------
            WorksheetStyle s192 = styles.Add("s192");
            s192.Font.Bold = true;
            s192.Font.FontName = "Calibri";
            s192.Font.Size = 11;
            s192.Font.Color = "#000000";
            s192.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s192.Alignment.WrapText = true;
            s192.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s192.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s193
            // -----------------------------------------------
            WorksheetStyle s193 = styles.Add("s193");
            s193.Font.Bold = true;
            s193.Font.FontName = "Calibri";
            s193.Font.Size = 11;
            s193.Font.Color = "#000000";
            s193.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s193.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s193.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s193.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s196
            // -----------------------------------------------
            WorksheetStyle s196 = styles.Add("s196");
            s196.Font.FontName = "Calibri";
            s196.Font.Size = 11;
            s196.Interior.Color = "#FFFFFF";
            s196.Interior.Pattern = StyleInteriorPattern.Solid;
            s196.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s196.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s196.Alignment.WrapText = true;
            s196.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s196.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s196.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s196.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s197
            // -----------------------------------------------
            WorksheetStyle s197 = styles.Add("s197");
            s197.Font.FontName = "Calibri";
            s197.Font.Size = 11;
            s197.Interior.Color = "#FFFFFF";
            s197.Interior.Pattern = StyleInteriorPattern.Solid;
            s197.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s197.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s197.Alignment.WrapText = true;
            s197.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s197.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s197.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s197.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s198
            // -----------------------------------------------
            WorksheetStyle s198 = styles.Add("s198");
            s198.Interior.Color = "#FFFFFF";
            s198.Interior.Pattern = StyleInteriorPattern.Solid;
            s198.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s198.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s198.Alignment.WrapText = true;
            s198.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s198.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s198.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s199
            // -----------------------------------------------
            WorksheetStyle s199 = styles.Add("s199");
            s199.Interior.Color = "#FFFFFF";
            s199.Interior.Pattern = StyleInteriorPattern.Solid;
            s199.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s199.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s199.Alignment.WrapText = true;
            s199.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s199.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s199.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s199.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s200
            // -----------------------------------------------
            WorksheetStyle s200 = styles.Add("s200");
            s200.Interior.Color = "#FFFFFF";
            s200.Interior.Pattern = StyleInteriorPattern.Solid;
            s200.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s200.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s200.Alignment.WrapText = true;
            s200.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s200.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s200.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s200.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s802
            // -----------------------------------------------
            WorksheetStyle s802 = styles.Add("s802");
            s802.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s802.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s897
            // -----------------------------------------------
            WorksheetStyle s897 = styles.Add("s897");
            s897.Font.Bold = true;
            s897.Font.FontName = "Calibri";
            s897.Font.Size = 12;
            s897.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s897.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s898
            // -----------------------------------------------
            WorksheetStyle s898 = styles.Add("s898");
            s898.Font.Bold = true;
            s898.Font.Italic = true;
            s898.Font.FontName = "Calibri";
            s898.Font.Color = "#FF0000";
            s898.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s898.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s899
            // -----------------------------------------------
            WorksheetStyle s899 = styles.Add("s899");
            s899.Font.Bold = true;
            s899.Font.Underline = UnderlineStyle.Single;
            s899.Font.FontName = "Calibri";
            s899.Font.Size = 11;
            s899.Font.Color = "#000000";
            s899.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s899.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s901
            // -----------------------------------------------
            WorksheetStyle s901 = styles.Add("s901");
            s901.Font.FontName = "Calibri";
            s901.Font.Size = 11;
            s901.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s901.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s901.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s902
            // -----------------------------------------------
            WorksheetStyle s902 = styles.Add("s902");
            s902.Font.FontName = "Calibri";
            s902.Font.Size = 11;
            s902.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s902.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s902.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s902.NumberFormat = "m/d/yy;@";
            // -----------------------------------------------
            //  s903
            // -----------------------------------------------
            WorksheetStyle s903 = styles.Add("s903");
            s903.Font.Bold = true;
            s903.Font.FontName = "Calibri";
            s903.Font.Size = 11;
            s903.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s903.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s907
            // -----------------------------------------------
            WorksheetStyle s907 = styles.Add("s907");
            s907.Font.FontName = "Calibri";
            s907.Font.Size = 11;
            s907.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s907.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s907.Alignment.WrapText = true;
            s907.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s907.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s907.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s907.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s930
            // -----------------------------------------------
            WorksheetStyle s930 = styles.Add("s930");
            s930.Font.Bold = true;
            s930.Font.FontName = "Calibri";
            s930.Font.Size = 11;
            s930.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s930.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s930.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s959
            // -----------------------------------------------
            WorksheetStyle s959 = styles.Add("s959");
            s959.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s959.Alignment.Vertical = StyleVerticalAlignment.Center;
            s959.Alignment.WrapText = true;
            s959.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s972
            // -----------------------------------------------
            WorksheetStyle s972 = styles.Add("s972");
            s972.Font.Bold = true;
            s972.Font.FontName = "Calibri";
            s972.Font.Size = 11;
            s972.Font.Color = "#000000";
            s972.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s972.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s972.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s995
            // -----------------------------------------------
            WorksheetStyle s995 = styles.Add("s995");
            s995.Font.FontName = "Calibri";
            s995.Font.Size = 11;
            s995.Interior.Color = "#C5D9F1";
            s995.Interior.Pattern = StyleInteriorPattern.Solid;
            s995.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s995.Alignment.Vertical = StyleVerticalAlignment.Center;
            s995.Alignment.WrapText = true;
            s995.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s995.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s995.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s995.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s997
            // -----------------------------------------------
            WorksheetStyle s997 = styles.Add("s997");
            s997.Font.FontName = "Calibri";
            s997.Font.Color = "#000000";
            s997.Interior.Color = "#C5D9F1";
            s997.Interior.Pattern = StyleInteriorPattern.Solid;
            s997.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s997.Alignment.Vertical = StyleVerticalAlignment.Center;
            s997.Alignment.WrapText = true;
            s997.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s997.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s997.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s997.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s999
            // -----------------------------------------------
            WorksheetStyle s999 = styles.Add("s999");
            s999.Font.Bold = true;
            s999.Font.FontName = "Calibri";
            s999.Font.Size = 11;
            s999.Interior.Color = "#C5D9F1";
            s999.Interior.Pattern = StyleInteriorPattern.Solid;
            s999.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s999.Alignment.Vertical = StyleVerticalAlignment.Center;
            s999.Alignment.WrapText = true;
            s999.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s999.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s999.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s999.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1001
            // -----------------------------------------------
            WorksheetStyle s1001 = styles.Add("s1001");
            s1001.Font.Bold = true;
            s1001.Font.FontName = "Calibri";
            s1001.Font.Size = 11;
            s1001.Interior.Color = "#C5D9F1";
            s1001.Interior.Pattern = StyleInteriorPattern.Solid;
            s1001.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1001.Alignment.Rotate = 90;
            s1001.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1001.Alignment.WrapText = true;
            s1001.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1001.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s1001.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s1001.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s1003
            // -----------------------------------------------
            WorksheetStyle s1003 = styles.Add("s1003");
            s1003.Font.Bold = true;
            s1003.Font.FontName = "Calibri";
            s1003.Font.Size = 11;
            s1003.Interior.Color = "#C5D9F1";
            s1003.Interior.Pattern = StyleInteriorPattern.Solid;
            s1003.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1003.Alignment.Rotate = 90;
            s1003.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1003.Alignment.WrapText = true;
            s1003.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1003.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1003.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1004
            // -----------------------------------------------
            WorksheetStyle s1004 = styles.Add("s1004");
            s1004.Font.Bold = true;
            s1004.Font.FontName = "Calibri";
            s1004.Font.Size = 11;
            s1004.Interior.Color = "#C5D9F1";
            s1004.Interior.Pattern = StyleInteriorPattern.Solid;
            s1004.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1004.Alignment.Rotate = 90;
            s1004.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1004.Alignment.WrapText = true;
            s1004.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1004.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s1004.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1004.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1005
            // -----------------------------------------------
            WorksheetStyle s1005 = styles.Add("s1005");
            s1005.Font.Bold = true;
            s1005.Font.FontName = "Calibri";
            s1005.Font.Size = 11;
            s1005.Interior.Color = "#C5D9F1";
            s1005.Interior.Pattern = StyleInteriorPattern.Solid;
            s1005.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1005.Alignment.Rotate = 90;
            s1005.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1005.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1005.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s1005.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1005.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1006
            // -----------------------------------------------
            WorksheetStyle s1006 = styles.Add("s1006");
            s1006.Font.Bold = true;
            s1006.Font.FontName = "Calibri";
            s1006.Font.Size = 11;
            s1006.Font.Color = "#000000";
            s1006.Interior.Color = "#C5D9F1";
            s1006.Interior.Pattern = StyleInteriorPattern.Solid;
            s1006.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1006.Alignment.Rotate = 90;
            s1006.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1006.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1006.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s1006.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1006.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1022
            // -----------------------------------------------
            WorksheetStyle s1022 = styles.Add("s1022");
            s1022.Font.Bold = true;
            s1022.Font.FontName = "Calibri";
            s1022.Font.Size = 11;
            s1022.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s1022.Alignment.Vertical = StyleVerticalAlignment.Center;
            s1022.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s1033
            // -----------------------------------------------
            WorksheetStyle s1033 = styles.Add("s1033");
            s1033.Font.FontName = "Calibri";
            s1033.Font.Size = 11;
            s1033.Interior.Color = "#FFFFFF";
            s1033.Interior.Pattern = StyleInteriorPattern.Solid;
            s1033.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s1033.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s1033.Alignment.WrapText = true;
            s1033.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1033.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s1033.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1033.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s1037
            // -----------------------------------------------
            WorksheetStyle s1037 = styles.Add("s1037");
            s1037.Interior.Color = "#FFFFFF";
            s1037.Interior.Pattern = StyleInteriorPattern.Solid;
            s1037.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s1037.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s1037.Alignment.WrapText = true;
            s1037.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s1037.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s1037.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s1037.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        }

        private void GenerateWorksheetC2cWPPPRWICp8(WorksheetCollection sheets,string ReportName,List<SPTargetEntity> TargetEntity,string SPCode)
        {
            Worksheet sheet = sheets.Add(ReportName);
            //sheet.Names.Add(new WorksheetNamedRange("Print_Area", "=\'C-2c WP & PPR WIC p.8\'!R1C1:R35C21", false));
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.DefaultColumnWidth = 46.5F;
            //sheet.Table.ExpandedColumnCount = 25;
            //sheet.Table.ExpandedRowCount = 36;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 85;
            column0.StyleID = "s22";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 14;
            column1.StyleID = "s22";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 72;
            column2.StyleID = "s29";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 14;
            column3.StyleID = "s22";
            column3.Span = 2;
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Index = 7;
            column4.Width = 56;
            column4.StyleID = "s22";
            column4.Span = 1;
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Index = 9;
            column5.Width = 73;
            column5.StyleID = "s22";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 92;
            column6.StyleID = "s22";
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 66;
            column7.StyleID = "s22";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 98;
            column8.StyleID = "s22";
            column8.Span = 1;
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Index = 14;
            column9.Width = 45;
            column9.StyleID = "s22";
            column9.Span = 5;
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Index = 20;
            column10.Width = 45;
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            WorksheetCell cell;
            cell = Row0.Cells.Add();
            cell.StyleID = "s802";
            cell.MergeAcross = 5;
            cell.MergeDown = 2;
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s899";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "ATTACHMENT C";
            cell.MergeAcross = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s802";
            cell.MergeAcross = 1;
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s47";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Page";
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s35";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "";
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s171";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "of";
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s180";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "";
            //cell.Formula = "";
            //cell.NamedCell.Add("Print_Area");
            cell = Row0.Cells.Add();
            cell.StyleID = "s48";
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            cell = Row1.Cells.Add();
            cell.StyleID = "s171";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "COMMUNITY SERVICES BLOCK GRANT";
            cell.Index = 7;
            cell.MergeAcross = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row1.Cells.Add();
            cell.StyleID = "s898";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "(Total Number of Pages should include C-2a, C-2b and C-2C)";
            cell.MergeAcross = 5;
            //cell.NamedCell.Add("Print_Area");
            cell = Row1.Cells.Add();
            cell.StyleID = "s21";
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 15;
            cell = Row2.Cells.Add();
            cell.StyleID = "s903";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "C-2c Work Plan and Program Progress Report (PPR)";
            cell.Index = 7;
            cell.MergeAcross = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row2.Cells.Add();
            cell.StyleID = "s897";
            cell.MergeAcross = 5;
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row3 = sheet.Table.Rows.Add();
            cell = Row3.Cells.Add();
            cell.StyleID = "s802";
            cell.MergeAcross = 19;
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------

            string AgyName= "Pro Action of Steuben and Yates, Inc.";
            string CntrctNum= "C1001486"; string PPRNum= "PPR #2";

            if(BaseForm.BaseAgencyControlDetails.AgyShortName.Trim()== "SUFFOLK")
            {
                AgyName = "Economic Opportunity Council of Suffolk, Inc.";
                CntrctNum = "C1001488";
                PPRNum = "PPR #1";
            }

            WorksheetRow Row4 = sheet.Table.Rows.Add();
            cell = Row4.Cells.Add();
            cell.StyleID = "s49";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Contractor";
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s901";
            cell.Data.Type = DataType.String;
            cell.Data.Text = AgyName;
            cell.MergeAcross = 9;
            //cell.Formula = "=#REF!";
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s23";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "FFY";
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s46";
            cell.Data.Type = DataType.String;
            cell.Data.Text = DateTime.Now.Year.ToString();
            //cell.Formula = "=#REF!";
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s171";
            cell.MergeAcross = 2;
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "m137369872";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Type (Work Plan, Amendment, PPR):";
            cell.MergeAcross = 3;
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s22";
            //cell.NamedCell.Add("Print_Area");
            cell = Row4.Cells.Add();
            cell.StyleID = "s26";
            // -----------------------------------------------
            WorksheetRow Row5 = sheet.Table.Rows.Add();
            cell = Row5.Cells.Add();
            cell.StyleID = "s171";
            cell.MergeAcross = 15;
            //cell.NamedCell.Add("Print_Area");
            cell = Row5.Cells.Add();
            cell.StyleID = "m137369912";
            cell.Data.Type = DataType.String;
            cell.Data.Text = PPRNum;
            cell.MergeAcross = 3;
            //cell.NamedCell.Add("Print_Area");
            cell = Row5.Cells.Add();
            cell.StyleID = "s22";
            //cell.NamedCell.Add("Print_Area");
            cell = Row5.Cells.Add();
            cell.StyleID = "s26";
            // -----------------------------------------------
            WorksheetRow Row6 = sheet.Table.Rows.Add();
            cell = Row6.Cells.Add();
            cell.StyleID = "s26";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Budget Period";
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s902";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Convert.ToDateTime(((ListItem)cmbRngCode.SelectedItem).ScreenCode.ToString()).ToString("MM/dd/yyyy"); 
            cell.MergeAcross = 1;
            //cell.Formula = "=#REF!";
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s903";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "to";
            cell.MergeAcross = 1;
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s902";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Convert.ToDateTime(((ListItem)cmbRngCode.SelectedItem).ScreenType.ToString()).ToString("MM/dd/yyyy");
            cell.MergeAcross = 1;
            //cell.Formula = "=#REF!";
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s903";
            cell.MergeAcross = 3;
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s44";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Contract #";
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s46";
            cell.Data.Type = DataType.String;
            cell.Data.Text = CntrctNum;
            //cell.Formula = "=#REF!";
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s802";
            cell.MergeAcross = 6;
            //cell.NamedCell.Add("Print_Area");
            cell = Row6.Cells.Add();
            cell.StyleID = "s22";
            cell.Index = 22;
            cell = Row6.Cells.Add();
            cell.StyleID = "s26";
            cell = Row6.Cells.Add();
            cell.StyleID = "s26";
            // -----------------------------------------------
            WorksheetRow Row7 = sheet.Table.Rows.Add();
            cell = Row7.Cells.Add();
            cell.StyleID = "s930";
            cell.MergeAcross = 19;
            //cell.NamedCell.Add("Print_Area");
            cell = Row7.Cells.Add();
            cell.StyleID = "s26";
            //cell.NamedCell.Add("Print_Area");
            cell = Row7.Cells.Add();
            cell.StyleID = "s49";
            cell = Row7.Cells.Add();
            cell.StyleID = "s26";
            cell.Index = 24;
            cell = Row7.Cells.Add();
            cell.StyleID = "s26";
            // -----------------------------------------------
            //WorksheetRow Row8 = sheet.Table.Rows.Add();
            //Row8.AutoFitHeight = true;
            //cell = Row8.Cells.Add();
            //cell.StyleID = "m137370432";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = @"Description of Need (Family, Community, Agency) \n Briefly identify the need that documents the reason for the programs/services/milestones and outcomes listed below.  Corresponds to the needs/strategic objectives identified in Attachment C-1b Demonstrated Needs and Attachment C-1c Strategic Plan.";
            //cell.MergeAcross = 10;
            ////cell.MergeDown = 3;
            //cell.NamedCell.Add("Print_Area");
            //cell = Row8.Cells.Add();
            //cell.StyleID = "m137370452";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Low-income individuals and families experience food insecurity. F";
            //cell.MergeAcross = 8;
            ////cell.MergeDown = 3;
            //cell.NamedCell.Add("Print_Area");
            //cell = Row8.Cells.Add();
            //cell.StyleID = "s51";
            //cell.NamedCell.Add("Print_Area");
            //// -----------------------------------------------
            //WorksheetRow Row9 = sheet.Table.Rows.Add();
            //Row9.Index = 13;
            //cell = Row9.Cells.Add();
            //cell.StyleID = "s1022";
            //cell.MergeAcross = 19;
            //cell.NamedCell.Add("Print_Area");

            string SPDetails = string.Empty;
            string SPInterventions = string.Empty;
            if(TargetEntity.Count>0)
            {
                SPTargetEntity TarEntity = TargetEntity.Find(u => u.SPT_TYPE == "SP");
                if (TarEntity != null)
                {
                    SPDetails = TarEntity.SPT_DESC.Trim();
                    SPInterventions = TarEntity.SPT_INTERVENTION.Trim();
                }
            }


            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "m263624672";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Description of Need (Family, Community, Agency)  ";
            cell.MergeAcross = 10;
            cell = Row8.Cells.Add();
            cell.StyleID = "m263624692";
            cell.Data.Type = DataType.String;
            cell.Data.Text = SPDetails.Trim(); //"Low-income individuals and families experience food insecurity. F";
            cell.MergeAcross = 8;
            cell.MergeDown = 1;
            cell = Row8.Cells.Add();
            cell.StyleID = "s117";
            // -----------------------------------------------
            WorksheetRow Row9 = sheet.Table.Rows.Add();
            Row9.Height = 48;
            Row9.AutoFitHeight = false;
            cell = Row9.Cells.Add();
            cell.StyleID = "s119";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Briefly identify the need that documents the reason for the programs/services/mil" +
                "estones and outcomes listed below.  Corresponds to the needs/strategic objective" +
                "s identified in Attachment C-1b Demonstrated Needs and Attachment C-1c Strategic" +
                " Plan.";
            cell.MergeAcross = 10;

            // -----------------------------------------------
            WorksheetRow wRow10 = sheet.Table.Rows.Add();
            //wRow10.Index = 13;
            wRow10.AutoFitHeight = false;
            cell = wRow10.Cells.Add();
            cell.StyleID = "s125";
            cell.MergeAcross = 19;
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            cell = Row10.Cells.Add();
            cell.StyleID = "m137370492";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Program that addresses the need described above:";
            cell.MergeAcross = 6;
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "m137370512";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "";
            cell.MergeAcross = 5;
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "s192";
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "s192";
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "s192";
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "s193";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Location (List Name of County):";
            //cell.NamedCell.Add("Print_Area");
            cell = Row10.Cells.Add();
            cell.StyleID = "m137370532";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "";
            cell.MergeAcross = 2;
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            Row11.Height = 15;
            cell = Row11.Cells.Add();
            cell.StyleID = "s959";
            cell.MergeAcross = 19;
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row12 = sheet.Table.Rows.Add();
            Row12.AutoFitHeight = false;
            cell = Row12.Cells.Add();
            cell.StyleID = "s995";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Interventions\nVery briefly describe the services, activities, and advocacy that w" +
                "ill address the need and achieve the outcome.\n(Use service and strategy  termino" +
                "logy from CSBG Annual Report Module 3 & Module 4).";
            cell.MergeAcross = 5;
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s997";
            cell.Data.Type = DataType.String;
            cell.Data.Text = @"Benchmarks or Milestones and Outcomes List the projected baseline number starting with the number seeking assistance followed by the number of customers to be enrolled. Then identify the expected benchmarks or milestones and outcomes to be achieved for the service or activity (Funnel).  When possible, describe the service or outcome using language from the Individual and Family National Performance Indicators (FNPIs)/ Individual and Family Services (SRV) or from the Strategies and Community National Performance Indicators (CNPIs) (STRs).";
            cell.MergeAcross = 3;
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s999";
            cell.Data.Type = DataType.String;
            cell.Data.Text = " NPI(s) or Service/ Capacity Codes";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "m137370192";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Method(s) of Measurement/Verification Identify the tool or process to be used to " +
                "verify progress on the outcome or milestone.";
            cell.MergeAcross = 1;
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1001";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Annual Target";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1003";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "PPR #1 Achieved";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1004";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "PPR #2 Achieved";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1004";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "PPR #3 Achieved";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1004";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "PPR #4 Achieved";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1005";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "YTD Total";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            cell = Row12.Cells.Add();
            cell.StyleID = "s1006";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "YTD %";
            cell.MergeDown = 7;
            //cell.NamedCell.Add("Print_Area");
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            Row13.Index = 20;
            Row13.Height = 19;
            Row13.AutoFitHeight = false;

            DataTable dataTable = new DataTable();
            DataView dv = new DataView(dt);
            dv.RowFilter = "SP0_SERVICECODE='" + SPCode + "'";
            dataTable = dv.ToTable();

            if (dataTable.Rows.Count > 0)
            {
                //// -----------------------------------------------
                //WorksheetRow Row14 = sheet.Table.Rows.Add();
                ////Row14.Index = 24;
                //Row14.Height = 45;
                //Row14.AutoFitHeight = false;
                //cell = Row14.Cells.Add();
                //cell.StyleID = "m137371292";
                //cell.Data.Type = DataType.String;
                //cell.Data.Text = SPInterventions.Trim();
                //cell.MergeAcross = 5;
                //cell.MergeDown = (dataTable.Rows.Count + 3);
                //cell.NamedCell.Add("Print_Area");

                int i = 0;
                foreach (DataRow dr in dataTable.Rows)
                {

                    SPTargetEntity Entity = new SPTargetEntity();
                    string CAMSDesc = string.Empty, CAMSIntervention = String.Empty;

                    if (dr["SP2_TYPE"].ToString().Trim() == "CA")
                        Entity = TargetEntity.Find(u => u.SPT_SP == dr["SP0_SERVICECODE"].ToString() && u.SPT_TYPE == "CA" && u.SPT_CAMS_CODE.Trim() == dr["SP2_CAMS_CODE"].ToString().Trim());
                    else if (dr["SP2_TYPE"].ToString().Trim() == "MS")
                        Entity = TargetEntity.Find(u => u.SPT_SP == dr["SP0_SERVICECODE"].ToString() && u.SPT_TYPE == "MS" && u.SPT_CAMS_CODE.Trim() == dr["SP2_CAMS_CODE"].ToString().Trim());

                    if (Entity != null) { CAMSDesc = Entity.SPT_DESC.Trim(); CAMSIntervention = Entity.SPT_INTERVENTION.Trim(); }
                    string CAMSCODE = dr["SP2_CAMS_CODE"].ToString();


                    string Ytd = string.Empty;
                    if (dr["TARGET_VAL"].ToString() != "0")
                    {
                        decimal percent = (decimal.Parse(dr["YTD"].ToString().Trim() == string.Empty ? "0" : dr["YTD"].ToString().Trim()) / decimal.Parse(dr["TARGET_VAL"].ToString().Trim() == string.Empty ? "0" : dr["TARGET_VAL"].ToString().Trim())) * 100;

                        Ytd = percent.ToString("0.00");
                    }

                    if (i == 0)
                    {
                        // -----------------------------------------------
                        WorksheetRow Row114 = sheet.Table.Rows.Add();
                        Row114.Index = 22;
                        Row114.Height = 45;
                        Row114.AutoFitHeight = false;
                        cell = Row114.Cells.Add();
                        cell.StyleID = "m137371292";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = SPInterventions.Trim();
                        cell.MergeAcross = 5;
                        cell.MergeDown = dataTable.Rows.Count-1;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s1037";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSDesc;
                        cell.MergeAcross = 3;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s196";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSCODE;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "m137371332";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSIntervention;
                        cell.MergeAcross = 1;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s200";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["TARGET_VAL"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s198";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q1"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q2"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q3"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q4"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s160";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["YTD"].ToString();
                        cell.Formula = "=SUM(RC[-4]:RC[-1])";
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row114.Cells.Add();
                        cell.StyleID = "s147";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = Ytd;
                        cell.Formula = "=RC[-1]/RC[-6]";
                        //cell.NamedCell.Add("Print_Area");
                    }
                    else
                    {


                        // -----------------------------------------------
                        WorksheetRow Row115 = sheet.Table.Rows.Add();
                        Row115.Height = 45;
                        Row115.AutoFitHeight = false;
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s1033";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSDesc;
                        cell.Index = 7;
                        cell.MergeAcross = 3;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s196";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSCODE;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "m137371372";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = CAMSIntervention;
                        cell.MergeAcross = 1;
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s200";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["TARGET_VAL"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s198";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q1"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q2"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q3"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s199";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["Q4"].ToString();
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s160";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = dr["YTD"].ToString();
                        cell.Formula = "=SUM(RC[-4]:RC[-1])";
                        //cell.NamedCell.Add("Print_Area");
                        cell = Row115.Cells.Add();
                        cell.StyleID = "s147";
                        cell.Data.Type = DataType.Number;
                        cell.Data.Text = Ytd;
                        cell.Formula = "=RC[-1]/RC[-6]";
                        //cell.NamedCell.Add("Print_Area");
                    }
                    i++;
                }
               
            }


            // -----------------------------------------------
            WorksheetRow Row25 = sheet.Table.Rows.Add();
            cell = Row25.Cells.Add();
            cell.StyleID = "s972";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "(continue on next page, if necessary)";
            cell.MergeAcross = 19;
            //cell.NamedCell.Add("Print_Area");
            //// -----------------------------------------------
            //WorksheetRow Row26 = sheet.Table.Rows.Add();
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell.Index = 14;
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            //cell = Row26.Cells.Add();
            //cell.StyleID = "s50";
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.Selected = true;
            sheet.Options.FitToPage = true;
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Layout.Orientation = CarlosAg.ExcelXmlWriter.Orientation.Landscape;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Data = "&L&10New York State Department of State&C&10Division of Community Services&R&10CS" + "BG Contract";
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.5F;
            sheet.Options.PageSetup.PageMargins.Left = 0.5F;
            sheet.Options.PageSetup.PageMargins.Right = 0.5F;
            sheet.Options.PageSetup.PageMargins.Top = 0.5F;
            sheet.Options.Print.Scale = 59;
            sheet.Options.Print.FitHeight = 0;
            sheet.Options.Print.ValidPrinterInfo = true;
        }




    }
}