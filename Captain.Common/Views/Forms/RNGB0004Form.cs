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
using Captain.Common.Views.Controls.Compatibility;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CarlosAg.ExcelXmlWriter;
using XLSExportFile;
using NPOI.SS.Formula.Functions;
using log4net.Repository.Hierarchy;
using DevExpress.XtraRichEdit.Model;
using System.Security.Cryptography;
using ListItem = Captain.Common.Utilities.ListItem;
//using DevExpress.CodeParser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNGB0004Form : _iForm
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public RNGB0004Form(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            ListZipCode = new List<ZipCodeEntity>();
            ListGroupCode = new List<Csb14GroupEntity>();
            ListcaseSiteEntity = new List<CaseSiteEntity>();
            Sel_Funding_List = new List<SPCommonEntity>();
            ListcommonEntity = new List<CommonEntity>();
            
            InitializeComponent();
            BaseForm = baseForm;
            PrivilegeEntity = privilegeEntity;
            this.Text = "ROMA Individual/Household Characteristics";
           // this.Text = privilegeEntity.Program + " - " + privilegeEntity.PrivilegeName;
            _model = new CaptainModel();

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
            Fill_Program_Combo();

            if (Rb_Process_Both.Checked)
            {
                All_CAMS_Selected = Btn_CA_Selection.Visible = false;
                lblDemographicsCount.Visible = pnlDemoCount.Visible = true;
                Btn_MS_Selection.Visible = false;
                Btn_CA_Selection.Text = "&All";
                Sel_CA_List.Clear(); Sel_MS_List.Clear();
                All_CAMS_Selected = true;

                Rb_Fund_All.Checked = true;
                //Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = false;
            }

            Get_MasterTable_DateRanges();
            Get_DG_Result_Table_Structure();
            //Get_DG_Bypass_Table_Structure();
            //Get_DG_SNP_Bypass_Table_Structure();
            //Get_DG_MST_Bypass_Table_Structure();
            Fill_CAMS_Master_List();
            Fill_All_List_Arrays();
            Fill_Fund_Mast_List();

            pnlRefPeriod.Visible = true;
            Size = new Size(this.Width, 695);
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
            Ref_From_Date.Checked = Ref_To_Date.Checked = true;

            OutCome_MasterList = _model.SPAdminData.Browse_CSB14Grp(Ref_From_Date.Text.Trim(), Ref_To_Date.Text.Trim(), null, null, null);
        }


        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegeEntity { get; set; }
        
        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        //public List<CaseSiteEntity> ListcaseMsSiteEntity { get; set; }

        //public List<CaseSiteEntity> ListcaseActSiteEntity { get; set; }

        public List<SPCommonEntity> Sel_Funding_List { get; set; }

        public List<ZipCodeEntity> ListZipCode { get; set; }

        public List<Csb14GroupEntity> ListGroupCode { get; set; }
        
        public List<CommonEntity> ListcommonEntity { get; set; }
        
        public string strAgency { get; set; }
        
        public string strDept { get; set; }
        
        public string strProg { get; set; }

        public string ReportPath { get; set; }
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

        public string propReportPath { get; set; }

        #endregion

        List<CaseSiteEntity> ListcaseMsSiteEntity = new List<CaseSiteEntity>();

        List<ZipCodeEntity> zipcode_List = new List<ZipCodeEntity>();
        List<CommonEntity> County_List = new List<CommonEntity>();
        List<CaseSiteEntity> Site_List = new List<CaseSiteEntity>();
        List<CaseSiteEntity> MSSite_List = new List<CaseSiteEntity>();
        List<CaseSiteEntity> CASite_List = new List<CaseSiteEntity>();
        private void Fill_All_List_Arrays()
        {
            zipcode_List = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            County_List = _model.ZipCodeAndAgency.GetCounty();
            CASite_List=MSSite_List = Site_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
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
                switch (Scr_Oper_Mode)
                {
                    case "RNGB0004":
                        SelectZipSiteCountyForm zipcodeform = new SelectZipSiteCountyForm(BaseForm, ListZipCode);
                        zipcodeform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                        zipcodeform.StartPosition = FormStartPosition.CenterScreen;
                        zipcodeform.ShowDialog();
                        break;
                    case "CASB0014":   // Groups
                        SelectZipSiteCountyForm zipcodeform1 = new SelectZipSiteCountyForm(BaseForm, ListGroupCode,Ref_From_Date.Text.Trim(),Ref_To_Date.Text.Trim());
                        zipcodeform1.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                        zipcodeform1.StartPosition = FormStartPosition.CenterScreen;
                        zipcodeform1.ShowDialog();
                        break;
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
                    ListGroupCode = form.SelectedGroupCodeEntity;
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
                    //if (Rb_Process_MS.Checked)
                    //{
                        ListcaseMsSiteEntity = form.SelectedCaseSiteEntity;
                        if (rdoMsselectsite.Checked == true && ListcaseMsSiteEntity.Count > 0)
                            txt_Msselect_site.Text = ListcaseMsSiteEntity[0].SiteNUMBER.ToString();
                        else
                            txt_Msselect_site.Clear();
                    //}
                    //else if(Rb_Process_CA.Checked)
                    //{
                    //    ListcaseActSiteEntity = form.SelectedCaseSiteEntity;
                    //    if (rdoMsselectsite.Checked == true && ListcaseActSiteEntity.Count > 0)
                    //        Txt_Sel_Site.Text = ListcaseActSiteEntity[0].SiteNUMBER.ToString();
                    //    else
                    //        Txt_Sel_Site.Clear();
                    //}
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
                    ListGroupCode = form.SelectedGroupCodeEntity;
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
        string DeptName = string.Empty;string ProgName = string.Empty;
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
                this.txtHieDesc.Size = new System.Drawing.Size(807,25);//(580, 23);
                //**pnlHie.Size = new Size(780,35);
                //**this.Size = new Size(825,652);
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
                this.txtHieDesc.Size = new Size(730, 25);//(532, 23);
                //**pnlHie.Size = new Size(840, 35);
                //**this.Size = new Size(877, 652);
            }
            else
            {
                this.txtHieDesc.Size = new Size(807, 25);//(580, 23);
                //**pnlHie.Size = new Size(780, 35);
                //**this.Size = new Size(825, 652);
            }
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
                    Initialize_All_Controls();
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


        List<DG_ResTab_Entity> DG_Table_List = new List<DG_ResTab_Entity>();
        private void Get_DG_Result_Table_Structure()
        {
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Agy_Type", "AGY Type", "L", "3.8in"));
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Col_Name", "Column Name To Compare", "L", "3.8in"));
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_Child_Code", "Child Code", "L", "2in"));
            DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Desc", "Attribute", "L", "4.3in"));
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_From_Age", "Age From", "R", "3.8in"));
            DG_Table_List.Add(new DG_ResTab_Entity("N", "Sum_To_Age", "Age To", "R", "3.8in"));
            DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Period_Count", "Rep Period", "R", "1.5in"));
            DG_Table_List.Add(new DG_ResTab_Entity("Y", "Sum_Child_Cum_Count", "Cumulative", "R", "1.5in"));
        }

        List<DG_ResTab_Entity> PM_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PM_Result_Table_Structure()
        {
            PM_Table_List.Clear();
            PM_Table_List.Add(new DG_ResTab_Entity("N", "Res_Group", "AGY Type", "L", "3.8in"));
            PM_Table_List.Add(new DG_ResTab_Entity("N", "Res_Table", "Column Name To Compare", "L", "3.5in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Table_Desc", "", "L", "3.2in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Unit_Cnt", "", "R", ".75in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Exp_To_Achive", "", "R", "1.15in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Hed1_Cnt", "", "R", ".85in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Hed2_Cnt", "", "R", ".85in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Hed3_Cnt", "", "R", ".85in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Hed4_Cnt", "", "R", ".85in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Hed5_Cnt", "", "R", ".85in"));
            PM_Table_List.Add(new DG_ResTab_Entity("Y", "Res_Per_Achived", "", "R", "1in"));
            PM_Table_List.Add(new DG_ResTab_Entity("N", "Res_Cost", "", "R", "1.5in"));
            PM_Table_List.Add(new DG_ResTab_Entity("N", "Res_Count_Type", "", "R", "1.5in"));
            PM_Table_List.Add(new DG_ResTab_Entity("N", "Res_Row_Type", "", "R", "1.5in"));
        }


        List<DG_ResTab_Entity> PM_Detail_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PM_Detail_Table_Structure()
        {
    //Declare @Detail_Cum_Table Table(SortUnDup_Group VarChar(10), SortUnDup_Table VarChar(10), SortUnDup_Agy Char(2), SortUnDup_Dept Char(2), SortUnDup_Prog Char(2), 
    //                                SortUnDup_Year VarChar(4), SortUnDup_App VarChar(8), SortUnDup_Fam_ID VarChar(9), SortUnDup_Client_ID Decimal(9),
    //                                SortUnDup_OutcomeCode Varchar(10), SortUnDup_OutCome_Date Date, SortUnDup_Count_Indicator Char(1), SortUnDup_Result Varchar(4), SortUnDup_Name Varchar(90), 
    //                                R1 Int Default 0, R2 Int Default 0, R3 Int Default 0, R4 Int Default 0, R5 Int Default 0)


            PM_Detail_Table_List.Clear();
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Group", "", "L", "1in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Table", "", "L", "1in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Agy", "", "L", "5.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Dept", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Prog", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Year", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_App", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Fam_ID", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Client_ID", "", "L", "3.2in"));

            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutCome_Date", "", "R", ".95in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutcomeCode", "", "R", "0.7in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Count_Indicator", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Result", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Name", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R1", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R2", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R3", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R4", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R5", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Group_Desc", "", "R", ".85in"));
        }


        List<DG_SNP_Bypass_Entity> DG_SNP_Bypass_List = new List<DG_SNP_Bypass_Entity>();
        private void Get_DG_SNP_Bypass_Table_Structure()
        {
            //	DECLARE @Individual_Details_Table TABLE(Ind_Row_ID INT, Ind_Agy VARCHAR(2), Ind_Dept VARCHAR(2), Ind_Prog VARCHAR(2), Ind_App VARCHAR(8), Ind_Fam_Seq Numeric(7), Ind_Client_Name VARCHAR(81), 
            //          Ind_Relation VARCHAR(50), Ind_Date DATE, Ind_Gender VARCHAR(20), Ind_Age VARCHAR(100), Ind_Ethnic VARCHAR(100), Ind_Race VARCHAR(100), Ind_Education VARCHAR(100),
            //          Ind_Health VARCHAR(50), Ind_Disabled VARCHAR(100), Ind_Vet VARCHAR(100), Ind_Food_Stamps VARCHAR(50), Ind_Farmer VARCHAR(100))

            //Comment by Sudheer on 11/22/2018
            DG_SNP_Bypass_List.Clear();

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Row_ID", "Sno", "L", ".3in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Agy", "Ag", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Dept", "De", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Prog", "Pr", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Year", "Year", "R", ".689in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_App", "App#", "R", ".689in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Fam_Seq", "Seq", "R", ".3in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Client_Name", "Client Name", "L", "2in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Fam_ID", "Family ID", "R", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_CLID", "Client ID", "R", ".9in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Relation", "Relation", "L", ".95in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Date", "Date", "C", ".75in"));

            if (rbBoth.Checked)
            {
                DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Reference_Period", "Reference Period", "C", "1.2in"));
                DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Report_Period", "Report Period", "C", "1.2in"));
            }

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Gender", "Gender", "L", ".8in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Age", "Age", "R", ".4in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Ethnic", "Ethnicity", "L", "1.15in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Race", "Race", "L", ".6in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Education", "Education", "L", "2in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Health", "Health Ins", "L", "1.35in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Health_Codes", "Health Codes", "L", "1.35in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Disabled", "Disabled", "L", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Vet", "Miltary Status", "L", "1.35in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Food_Stamps", "Food Stamps", "L", "1in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Farmer", "Work Status", "L", "1.35in"));
            
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Youth", "Disc Youth", "L", "1.15in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Byp_updated_date", "Updated Date", "L", "1.1in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Byp_updated_by", "Updated by", "L", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_County", "County", "L", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Zip", "Zip", "L", ".8in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_site", "Site", "L", "1in"));
            //if(rbBoth.Checked)
            //    DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Rep_Range", "Rep Range", "L", "1in"));
            
        }

        List<DG_Bypass_Entity> DG_Bypass_List = new List<DG_Bypass_Entity>();
        private void Get_DG_Bypass_Table_Structure()
        {
            //	(Byp_Row_ID INT, Byp_Agy VARCHAR(2), Byp_Dept VARCHAR(2), Byp_Prog VARCHAR(2), Byp_App VARCHAR(8), Byp_Fam_Seq Numeric(7), Byp_Client_Name VARCHAR(81), Byp_Site VARCHAR(4), Byp_Attribute VARCHAR(100), 
            //   Byp_Att_Resp VARCHAR(600), Byp_Exc_Reason VARCHAR(100), Byp_Relation VARCHAR(50), Byp_Gender VARCHAR(20), Byp_Ethnic VARCHAR(100), Byp_Race VARCHAR(100), Byp_Education VARCHAR(100), Byp_Health VARCHAR(50), Byp_Disabled VARCHAR(10))

            //Comment by Sudheer on 11/22/2018
            DG_Bypass_List.Clear();

            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Row_ID", "Sno", "L", ".3in")); 

            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Agy", "Ag", "L", ".25in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Dept", "De", "L", ".25in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Prog", "Pr", "L", ".25in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Year", "Year", "L", ".5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_App", "App#", "R", ".689in"));

            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Fam_Seq", "Seq", "R", ".3in"));

            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Client_Name", "Client Name", "L", "2.25in"));

            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Fam_ID", "Family ID", "R", ".9in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_CLID", "Client ID", "R", ".9in"));
            
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Site", "Site", "L", ".5in"));  
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Attribute", "Data Field", "L", ".9in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Att_Resp", "Response", "L", "1.7in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Exc_Reason", "Exclusion Reason", "L", "2.3in"));

            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Relation", "Rel", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Gender", "Gen", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Ethnic", "Eth", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Race", "Rac", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Education", "Edu", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Health", "Hel", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("N", "Byp_Disabled", "Dis", "R", "1.5in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_updated_date", "Updated Date", "L", "1.1in"));
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_updated_by", "Updated by", "L", ".9in"));
            //if (rbBoth.Checked)
            //    DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Rep_Range", "Rep Range", "L", ".9in"));
        }


        List<DG_SNP_Bypass_Entity> DG_MST_Bypass_List = new List<DG_SNP_Bypass_Entity>();
        private void Get_DG_MST_Bypass_Table_Structure()
        {
            //DECLARE @Family_Details_Table TABLE(Fam_Row_ID INT, Fam_Agy VARCHAR(2), Fam_Dept VARCHAR(2), Fam_Prog VARCHAR(2), Fam_Year VARCHAR(4), Fam_App VARCHAR(8), Ind_Fam_Seq Numeric(7), 
            //        Fam_Client_Name VARCHAR(81), Fam_Date DATE, Fam_Type VARCHAR(100), Fam_Size TINYINT, Fam_Hou_Type VARCHAR(100), Fam_Inc_Type VARCHAR(600), Fam_FPL VARCHAR(100), Fam_Ver_Date DATE)

            //Comment by Sudheer on 11/22/2018
            DG_MST_Bypass_List.Clear();

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Fam_Row_ID", "Sno", "L", ".3in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Agy", "Ag", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Dept", "De", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Prog", "Pr", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Year", "Year", "R", ".689in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_App", "App#", "R", ".689in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Fam_Seq", "Seq", "R", ".3in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Client_Name", "Client Name", "L", "2in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_FamilyID", "Family ID", "R", ".9in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_CLID", "Client ID", "R", ".9in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Date", "Date", "C", ".75in"));

            if (rbBoth.Checked)
            {
                DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Reference_Period", "Reference Perod", "C", "1.2in"));
                DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Report_Period", "Report Period", "C", "1.2in"));
            }
            
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Type", "Family Type", "L", "1.2in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Size", "Fam.Size", "R", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Hou_Type", "Housing Type", "L", "1in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Inc_Type", "Income Types", "L", "1.15in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_FPL", "FPL", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Ver_Date", "Ver.Date", "C", ".75in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Cash_Ben", "Non-Cash Ben", "L", "1.35in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13a", "13a", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13b", "13b", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13c", "13c", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13d", "13d", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13e", "13e", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13f", "13f", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13g", "13g", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13h", "13h", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_13i", "13i", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Byp_updated_date", "Updated Date", "L", "1.1in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Byp_updated_by", "Updated by", "L", ".9in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_County", "County", "L", ".9in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Zip", "Zip", "L", ".9in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_site", "Site", "L", "1in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Acty_Prog", "Act Program", "L", "1in"));
            //if (rbBoth.Checked)
            //    DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Rep_Range", "Rep Range", "L", "1in"));
        }

        DataSet ds = new DataSet();
        DataTable dtCounty = new DataTable();
        DataTable dtLanguage = new DataTable();
        private void btnGenerateFile_Click(object sender, EventArgs e)
         {
            //string myPath = "";
            //if (Context.HttpContext.Request.IsSecureConnection)
            //    myPath = "\\\\cap-dev\\C-Drive\\CapReports\\CasDemo.pdf" ;
            //else
            //    myPath = @"C:\CapReports\CasDemo.pdf"; //Context.Server.MapPath("~\\Resources\\Excel\\Sample.xlsx"); //(@"C:\sample.xlsx");
            //FrmViewer objfrm = new FrmViewer(myPath);
            //objfrm.ShowDialog();

            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            searchAgytabs.Tabs_Type = "00004";
            List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);


            AGYTABSEntity searchNCB = new AGYTABSEntity(true);
            searchNCB.Tabs_Type = "00037";
            List<AGYTABSEntity> AgyTabs_NCB_List = _model.AdhocData.Browse_AGYTABS(searchNCB);

            List<AGYTABSEntity> Income = new List<AGYTABSEntity>();
            List<AGYTABSEntity> NoIncome = new List<AGYTABSEntity>();
            List<AGYTABSEntity> Non_Cash = new List<AGYTABSEntity>();
            if (AgyTabs_List.Count > 0 && AgyTabs_NCB_List.Count > 0)
            {
                Income = AgyTabs_List.FindAll(u => u.Code_Desc.ToUpper().Trim().Contains("NO INCOME") || u.Code_Desc.ToUpper().Trim().Contains("NONE"));
                Non_Cash = AgyTabs_NCB_List.FindAll(u => u.Code_Desc.ToUpper().Trim().Contains("NONE"));
                //if(Income.Count==0)
                //    NoIncome = AgyTabs_List.FindAll(u => u.Code_Desc.ToUpper().Trim().Contains("No Income"));
            }

            string Msg = string.Empty;
            if (Income.Count > 1 || NoIncome.Count > 1 || Non_Cash.Count > 1)
            {
                if (Income.Count > 1 && Non_Cash.Count > 1)
                    Msg = "Multiple 'None/No Income’s' defined in Income Types \n Multiple 'None’s' defined in Non-Cash Benefits";
                else if (Income.Count > 1)
                    Msg = "Multiple 'None/No Income’s' defined in Income Types";
                else if (Non_Cash.Count > 1)
                    Msg = "Multiple 'None’s' defined in Non-Cash Benefits";
            }
            else if (Income.Count == 0 || NoIncome.Count == 0 || Non_Cash.Count == 0)
            {
                if (Income.Count == 0 && Non_Cash.Count == 0)
                    Msg = "'None/No Income' is not defined in Income Types \n 'None' is not defined in Non-Cash Benefits";
                else if (Income.Count == 0 && NoIncome.Count == 0)
                    Msg = "'None/No Income' is not defined in Income Types";
                else if (Non_Cash.Count == 0)
                    Msg = "'None' is not defined in Non-Cash Benefits";
            }


            if (!string.IsNullOrEmpty(Msg.Trim()))
            {
                MessageBox.Show(Msg, Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: GetReport);
            }

            else
            {
                if (Validate_Report())
                {
                    if ((Scr_Oper_Mode == "RNGB0004"))
                    {
                        Get_Selection_Criteria();

                        ds = new DataSet();
                        if (Scr_Oper_Mode == "RNGB0004")
                            ds = _model.AdhocData.Get_RNGDG_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", SelectedAlertCodes);
                        //else
                        //    ds = _model.AdhocData.Get_PM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");

                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                dt = ds.Tables[1];

                                if (Rb_Details_Yes.Checked)
                                {
                                    Bypass_Dynamic_RDLC();
                                    MST_Dynamic_RDLC();
                                    SNP_Dynamic_RDLC();

                                    if (Rb_Process_Both.Checked && Rb_Details_Yes.Checked)
                                    {
                                        if (chkbMontCounty.Checked && ds.Tables[5].Rows.Count > 0)
                                            dtCounty = ds.Tables[5];

                                        if (chkbLangCounts.Checked && ds.Tables[6].Rows.Count > 0)
                                            dtLanguage = ds.Tables[6];
                                    }

                                }
                                else
                                {
                                    if (chkbMontCounty.Checked)
                                        dtCounty = ds.Tables[2];

                                    if (chkbLangCounts.Checked)
                                        dtLanguage = ds.Tables[3];
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    if (rbBoth.Checked)
                                        On_SaveForm_Both_Counts_Pdf();
                                    if (rbRepPeriod.Checked)
                                        On_SaveForm_Counts_Pdf();
                                    if (RbCummilative.Checked)
                                        On_SaveForm_Cummilative_Counts_Pdf();
                                }

                                //if (Rb_Details_Yes.Checked)
                                //{
                                //    DataTable dtSNP = ds.Tables[5];
                                //    DataTable dtMST = ds.Tables[7];
                                //    DataTable dtBypass = ds.Tables[3];

                                //    if (dtSNP.Rows.Count > 0)
                                //    {
                                //        //On_IndividualsForm_Closed_Pdf(dtSNP);
                                //        //if(chkbExcel.Checked)
                                //            OnExcel_IndividualsForm_Report(dtSNP);
                                //    }

                                //    if (dtMST.Rows.Count > 0)
                                //    {
                                //        //On_FamilyForm_Closed_Pdf(dtMST);
                                //        //if (chkbExcel.Checked)
                                //            OnExcel_FamilyForm_Report(dtMST);
                                //    }

                                //    if (dtBypass.Rows.Count > 0)
                                //    {
                                //        //On_ByPassForm_Closed_Pdf(dtBypass);
                                //        //if (chkbExcel.Checked)
                                //            OnExcel_ByPassForm_Report(dtBypass);
                                //    }

                                //}
                            }
                        }
                    }


                    //if ((Scr_Oper_Mode == "CASB0014" && Validate_PM_DateRanges()) || (Scr_Oper_Mode == "CASB0004"))
                    //{
                    //    Page_Setup_Completed = false;
                    //    CASB2012_AdhocPageSetup PageSetup_Form = new CASB2012_AdhocPageSetup(20, 10, PrivilegeEntity.Program);
                    //    PageSetup_Form.FormClosed += new Form.FormClosedEventHandler(On_Pagesetup_Form_Closed);
                    //    PageSetup_Form.ShowDialog();
                    //    //if (Page_Setup_Completed)
                    //    //    Process_report();
                    //}
                }
            }
        }

        private void GetReport(DialogResult dialogResult)//(object sender, EventArgs e)
        {
            //Wisej.Web.Form senderform = (Wisej.Web.Form)sender;

            //if (senderform != null)
            //{
                if (dialogResult == DialogResult.Yes)
                {

                    if (Validate_Report())
                    {
                        if ((Scr_Oper_Mode == "RNGB0004"))
                        {
                            Get_Selection_Criteria();

                            ds = new DataSet();
                            if (Scr_Oper_Mode == "RNGB0004")
                                ds = _model.AdhocData.Get_RNGDG_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", SelectedAlertCodes);
                            //else
                            //    ds = _model.AdhocData.Get_PM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    dt = ds.Tables[1];

                                    if (Rb_Details_Yes.Checked)
                                    {
                                        Bypass_Dynamic_RDLC();
                                        MST_Dynamic_RDLC();
                                        SNP_Dynamic_RDLC();
                                    }

                                    if (dt.Rows.Count > 0)
                                    {
                                        if (rbBoth.Checked)
                                            On_SaveForm_Both_Counts_Pdf();
                                        if (rbRepPeriod.Checked)
                                            On_SaveForm_Counts_Pdf();
                                        if (RbCummilative.Checked)
                                            On_SaveForm_Cummilative_Counts_Pdf();
                                    }

                                }
                            }
                        }

                    }
                }
            //}
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        //private void On_SaveForm_Closed_Pdf()//object sender, FormClosedEventArgs e)
        //{

        //    Random_Filename = null;
        //    PdfName = "Pdf File";
        //    PdfName = "SIMLTR_" + BaseForm.BaseApplicationNo;
        //    PdfName = propReportPath + PdfName;
        //    //PdfName = strFolderPath + PdfName;
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
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }


        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();

        //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        //    BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
        //    iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
        //    iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
        //    iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

        //    cb = writer.DirectContent;


        //    //PdfShading axial = PdfShading.SimpleAxial(writer, 36, 716, 396,788, BaseColor.ORANGE, BaseColor.BLUE);
        //    ////cb.PaintShading(axial);
        //    //PdfShadingPattern shading = new PdfShadingPattern(axial);
        //    //cb.SetShadingFill(shading);

        //    cb.BeginText();
        //    //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
        //    cb.SetFontAndSize(bfTimes, 12);

        //    //List<CSB4AsocEntity> CSb4Entity;
        //    //CSb4Entity = _model.SPAdminData.Browse_CSB4Assoc(string.Empty, string.Empty);
        //    DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

        //    cb.SetFontAndSize(bfTimesBold, 10);
        //    cb.SetRGBColorFill(4, 4, 15);
        //    X_Pos = 300; Y_Pos = 785;
        //    //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
        //    //Y_Pos -= 15;
        //    //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
        //    //Y_Pos -= 15;

        //    if (dscategories.Tables[0].Rows.Count > 0)
        //    {
        //        DataTable dt = dscategories.Tables[0];
        //        DataView dv = new DataView(dt);
        //        dv.Sort = "RNG4CATG_SEQ";
        //        dt = dv.ToTable();

        //        cb.SetFontAndSize(bfTimesBold, 10);
        //        cb.SetRGBColorFill(4, 4, 15);
        //        //cb.SetColorFill(BaseColor.MAGENTA.Darker());
        //        X_Pos = 30; //Y_Pos = 780;
        //        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of Local Agency Reporting:", X_Pos, Y_Pos, 0);
        //        cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        cb.MoveTo(210, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos+8);
        //        cb.LineTo(210, Y_Pos + 8);
        //        cb.ClosePathStroke();

        //        Y_Pos -= 20;
        //        cb.SetRGBColorFill(45, 45, 153);
        //        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
        //        cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        cb.SetRGBColorFill(160, 160, 160);
        //        cb.MoveTo(540, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos + 8);
        //        cb.LineTo(540, Y_Pos + 8);
        //        cb.ClosePathFillStroke();

        //        Y_Pos -= 15;
        //        cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
        //        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
        //        cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        cb.SetRGBColorFill(160, 160, 160);
        //        cb.MoveTo(540, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos + 8);
        //        cb.LineTo(540, Y_Pos + 8);
        //        cb.ClosePathFillStroke();

        //        //Y_Pos -= 18;
        //        //cb.SetRGBColorFill(45, 45, 153);
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Estimated total number of NEW Individuals not included in this report: ", X_Pos, Y_Pos, 0);

        //        ///************************************CheckBoxes****************************/
        //        //iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
        //        //PdfAppearance[] onOff = new PdfAppearance[2];
        //        //onOff[0] = cb.CreateAppearance(20, 20);
        //        //onOff[0].Rectangle(1, 20, 1, 20);
        //        //onOff[0].Rectangle(18, 18, 1, 1);
        //        //onOff[0].Stroke();
        //        //onOff[1] = cb.CreateAppearance(20, 20);
        //        //onOff[1].SetRGBColorFill(255, 128, 128);
        //        //onOff[1].Rectangle(18, 18, 1, 1);
        //        //onOff[1].FillStroke();
        //        //onOff[1].MoveTo(1, 1);
        //        //onOff[1].LineTo(19, 19);
        //        //onOff[1].MoveTo(1, 19);
        //        //onOff[1].LineTo(19, 1);

        //        //RadioCheckField checkbox;
        //        //PdfFormField SField;

        //        //rect = new iTextSharp.text.Rectangle(350, Y_Pos + 8, 358, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "(0-200)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(0-200)", Times), 360, Y_Pos, 0);
                
        //        //rect = new iTextSharp.text.Rectangle(400, Y_Pos + 8, 408, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "(201-400)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(201-400)", Times), 410, Y_Pos, 0);
                
        //        //rect = new iTextSharp.text.Rectangle(450, Y_Pos + 8, 458, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "(401-600)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(401-600)", Times), 460, Y_Pos, 0);

        //        //rect = new iTextSharp.text.Rectangle(500, Y_Pos + 8, 508, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "(600+)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(600+)", Times), 510, Y_Pos, 0);

        //        //Y_Pos -= 15;
        //        //cb.SetRGBColorFill(60, 15, 112);
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Estimated total number of NEW Households not included in this report: ", X_Pos, Y_Pos, 0);

        //        //rect = new iTextSharp.text.Rectangle(350, Y_Pos + 8, 358, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "S(0-200)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(0-200)", Times), 360, Y_Pos, 0);

        //        //rect = new iTextSharp.text.Rectangle(400, Y_Pos + 8, 408, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "S(201-400)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(201-400)", Times), 410, Y_Pos, 0);

        //        //rect = new iTextSharp.text.Rectangle(450, Y_Pos + 8, 458, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "S(401-600)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(401-600)", Times), 460, Y_Pos, 0);

        //        //rect = new iTextSharp.text.Rectangle(500, Y_Pos + 8, 508, Y_Pos);
        //        //checkbox = new RadioCheckField(writer, rect, "S(600+)", "On");
        //        //checkbox.BorderColor = new GrayColor(0.3f);
        //        ////checkbox.Rotation = 90;
        //        //SField = checkbox.CheckField;
        //        //writer.AddAnnotation(SField);
        //        //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("(600+)", Times), 510, Y_Pos, 0);

        //        //cb.SetFontAndSize(bfTimes, 10);

        //        cb.EndText();

        //        //Temp table not displayed on the screen
        //        PdfPTable head = new PdfPTable(1);
        //        head.HorizontalAlignment = Element.ALIGN_CENTER;
        //        head.TotalWidth = 50f;
        //        PdfPCell headcell = new PdfPCell(new Phrase(""));
        //        headcell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        head.AddCell(headcell);

        //        PdfPTable outer = new PdfPTable(2);
        //        outer.TotalWidth = 500f;
        //        outer.LockedWidth = true;
        //        outer.HorizontalAlignment = Element.ALIGN_LEFT;
        //        float[] widths2 = new float[] { 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        outer.SetWidths(widths2);
        //        outer.SpacingBefore = 70f;

        //        PdfPTable table = new PdfPTable(3);
        //        table.TotalWidth = 240f;
        //        table.LockedWidth = true;
        //        float[] widths = new float[] { 120f, 35f,35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        table.SetWidths(widths);
        //        //table.SpacingBefore = 270f;

        //        PdfPTable Rtable = new PdfPTable(3);
        //        Rtable.TotalWidth = 240f;
        //        Rtable.LockedWidth = true;
        //        float[] widths1 = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        Rtable.SetWidths(widths1);

        //        PdfPCell Header = new PdfPCell(new Phrase("C. INDIVIDUAL LEVEL CHARACTERISTICS", fc1));
        //        Header.Colspan = 3;
        //        Header.HorizontalAlignment = Element.ALIGN_LEFT;
        //        Header.FixedHeight = 15f;
        //        Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        table.AddCell(Header);

        //        PdfPCell RHeader = new PdfPCell(new Phrase(" ", fc1));
        //        RHeader.Colspan = 3;
        //        RHeader.HorizontalAlignment = Element.ALIGN_LEFT;
        //        RHeader.FixedHeight = 15f;
        //        RHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Rtable.AddCell(RHeader);

        //        bool First = true; int Count = 1; bool RFirst = true; bool EduCol = true; bool Rtab = true;
        //        string PrivType = string.Empty; bool Disable = true; bool Health = true;
        //        foreach (DataRow dr in dt.Rows)
        //        {

        //            if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) == 71)
        //            {
        //                PdfPCell R0 = new PdfPCell(new Phrase("", TblFontBold));
        //                R0.FixedHeight = 12f;
        //                R0.Colspan = 3;
        //                R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                table.AddCell(R0);

        //                PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                R01.FixedHeight = 12f;
        //                R01.Colspan = 2;
        //                R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                Rtable.AddCell(R01);

        //                PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                R02.FixedHeight = 12f;
        //                R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                R02.Border = iTextSharp.text.Rectangle.BOX;
        //                R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                Rtable.AddCell(R02);

        //                PdfPCell R03 = new PdfPCell(new Phrase("", TblFontBold));
        //                R03.FixedHeight = 12f;
        //                R03.Colspan = 3;
        //                R03.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                Rtable.AddCell(R03);


        //                break;
        //            }


        //            if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) >= 45 && int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) < 71)
        //            {
        //                if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                {
        //                    if (Rtab)
        //                    {
        //                        PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                        R0.FixedHeight = 12f;
        //                        R0.Colspan = 2;
        //                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R0);

        //                        PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                        R02.FixedHeight = 12f;
        //                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R02.Border = iTextSharp.text.Rectangle.BOX;
        //                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                        table.AddCell(R02);

        //                        RFirst = false; First = true; Rtab = false;
        //                    }

        //                    if (!First)
        //                    {
        //                        PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                        R0.FixedHeight = 12f;
        //                        R0.Colspan = 2;
        //                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        Rtable.AddCell(R0);

        //                        PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                        R02.FixedHeight = 12f;
        //                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R02.Border = iTextSharp.text.Rectangle.BOX;
        //                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                        Rtable.AddCell(R02);
        //                    }

        //                    if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "E")
        //                    {
        //                        PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
        //                        R3.FixedHeight = 12f;
        //                        R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R3);

        //                        PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                        R4.FixedHeight = 12f;
        //                        R4.Colspan = 2;
        //                        R4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R4.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R4);

        //                        PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 3;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R1);
        //                    }
        //                    else if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 3;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R1);
        //                    }
        //                    else if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "R")
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase("I.Race", TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 3;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R1);
        //                    }
        //                    else 
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R1);

        //                        PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                        R2.FixedHeight = 12f;
        //                        R2.Colspan = 2;
        //                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        Rtable.AddCell(R2);
        //                    }

        //                    PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
        //                    //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

        //                    First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && dr["RNG4CATG_CODE"].ToString().Trim() != "E") Count++;
        //                }
        //                else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                {
        //                    PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                    R1.FixedHeight = 12f;
        //                    R1.Colspan = 2;
        //                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
        //                    Rtable.AddCell(R1);

        //                    PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                    R2.FixedHeight = 12f;
        //                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R2.Border = iTextSharp.text.Rectangle.BOX;
        //                    //R2.BackgroundColor = BaseColor.BLUE.Brighter();
        //                    Rtable.AddCell(R2);
        //                }
        //            }
        //            else if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            {
        //                if (!First)
        //                {
        //                    if (PrivType == "U")
        //                    {
        //                        PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                        R0.FixedHeight = 12f;
        //                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R0);

        //                        PdfPCell R01 = new PdfPCell(new Phrase("0", TblFontBold));
        //                        R01.FixedHeight = 12f;
        //                        R01.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R01.Border = iTextSharp.text.Rectangle.BOX;
        //                        R01.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                        table.AddCell(R01);

        //                        PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                        R02.FixedHeight = 12f;
        //                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R02.Border = iTextSharp.text.Rectangle.BOX;
        //                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                        table.AddCell(R02);
        //                    }
        //                    else
        //                    {
        //                        if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && PrivType!="N")
        //                        {
        //                            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                            R0.FixedHeight = 12f;
        //                            R0.Colspan = 2;
        //                            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R0);

        //                            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                            R02.FixedHeight = 12f;
        //                            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R02.Border = iTextSharp.text.Rectangle.BOX;
        //                            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(R02);
        //                        }
        //                    }
        //                }

        //                if (Count == 5 && dr["RNG4CATG_CODE"].ToString().Trim() == "D")
        //                {
        //                    PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Health", TblFontBold));
        //                    R3.FixedHeight = 12f;
        //                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                    table.AddCell(R3);

        //                    PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                    R4.FixedHeight = 12f;
        //                    R4.Colspan = 2;
        //                    R4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    R4.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                    table.AddCell(R4);
        //                }

        //                if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
        //                {
        //                    PdfPCell R1 = new PdfPCell(new Phrase( dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                    R1.FixedHeight = 12f;
        //                    R1.Colspan = 3;
        //                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                    table.AddCell(R1);
        //                }
        //                else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
        //                {
        //                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                    R1.FixedHeight = 12f;
        //                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                    table.AddCell(R1);

        //                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                    R2.FixedHeight = 12f;
        //                    R2.Colspan = 2;
        //                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                    table.AddCell(R2);
        //                }

        //                PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
        //                //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

        //                First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim()!="N") Count++;
        //            }
        //            else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            {
        //                if (dr["RNG4CATG_CODE"].ToString().Trim() == "D" && Disable)
        //                {
        //                    dv = new DataView(dt);
        //                    dv.RowFilter = "RNG4CATG_CODE='D' AND RNG4CATG_DEM<>''";
        //                    DataTable dtDis = dv.ToTable();

        //                    if (dtDis.Rows.Count > 0)
        //                    {
        //                        PdfPTable NestedTable = new PdfPTable(4);
        //                        NestedTable.TotalWidth = 240f;
        //                        NestedTable.LockedWidth = true;
        //                        float[] Dwidths = new float[] { 100f,30f, 30f, 30f };
        //                        NestedTable.SetWidths(Dwidths);

        //                        PdfPTable NestedTable1 = new PdfPTable(4);
        //                        NestedTable1.TotalWidth = 240f;
        //                        NestedTable1.LockedWidth = true;
        //                        float[] NDwidths = new float[] { 100f, 30f, 30f, 30f };
        //                        NestedTable1.SetWidths(NDwidths);

        //                        PdfPCell D1 = new PdfPCell(new Phrase("", Times));
        //                        D1.FixedHeight = 12f;
        //                        D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        NestedTable.AddCell(D1);

        //                        foreach (DataRow drDis in dtDis.Rows)
        //                        {
        //                            PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                            D2.FixedHeight = 12f;
        //                            D2.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D2);
        //                        }

        //                        PdfPCell R11 = new PdfPCell(NestedTable);
        //                        R11.Padding = 0f;
        //                        R11.Colspan = 3;
        //                        R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R11);

        //                        //if (NestedTable.Rows.Count > 0)
        //                        //    NestedTable.DeleteBodyRows();

        //                        PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", Times));
        //                        D3.FixedHeight = 12f;
        //                        D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        NestedTable1.AddCell(D3);
        //                        for (int i = 0; i < dtDis.Rows.Count; i++)
        //                        {
        //                            PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                            R2.FixedHeight = 12f;
        //                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R2.Border = iTextSharp.text.Rectangle.BOX;
        //                            NestedTable1.AddCell(R2);
        //                        }

        //                        PdfPCell R12 = new PdfPCell(NestedTable1);
        //                        R12.Padding = 0f;
        //                        R12.Colspan = 3;
        //                        R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R12);
        //                    }
        //                    Disable = false;

        //                }

        //                if (dr["RNG4CATG_CODE"].ToString().Trim() == "N" && Health)
        //                {
        //                    dv = new DataView(dt);
        //                    dv.RowFilter = "RNG4CATG_CODE='N' AND RNG4CATG_DEM<>''";
        //                    DataTable dtDis = dv.ToTable();

        //                    if (dtDis.Rows.Count > 0)
        //                    {
        //                        PdfPTable NestedTable = new PdfPTable(4);
        //                        NestedTable.TotalWidth = 250f;
        //                        NestedTable.LockedWidth = true;
        //                        float[] Dwidths = new float[] { 100f, 30f, 30f, 30f };
        //                        NestedTable.SetWidths(Dwidths);

        //                        PdfPTable NestedTable1 = new PdfPTable(4);
        //                        NestedTable1.TotalWidth = 240f;
        //                        NestedTable1.LockedWidth = true;
        //                        float[] NDwidths = new float[] { 100f, 30f, 30f, 30f };
        //                        NestedTable1.SetWidths(NDwidths);

        //                        PdfPCell D1 = new PdfPCell(new Phrase("", Times));
        //                        D1.FixedHeight = 12f;
        //                        D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        NestedTable.AddCell(D1);

        //                        foreach (DataRow drDis in dtDis.Rows)
        //                        {
        //                            PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                            D2.FixedHeight = 12f;
        //                            D2.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D2);
        //                        }

        //                        PdfPCell R11 = new PdfPCell(NestedTable);
        //                        R11.Padding = 0f;
        //                        R11.Colspan = 3;
        //                        R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R11);

        //                        //if (NestedTable.Rows.Count > 0)
        //                        //    NestedTable.DeleteBodyRows();

        //                        PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance*", Times));
        //                        D3.FixedHeight = 12f;
        //                        D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        NestedTable1.AddCell(D3);
        //                        for (int i = 0; i < dtDis.Rows.Count; i++)
        //                        {
        //                            PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                            R2.FixedHeight = 12f;
        //                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R2.Border = iTextSharp.text.Rectangle.BOX;
        //                            NestedTable1.AddCell(R2);
        //                        }

        //                        PdfPCell R12 = new PdfPCell(NestedTable1);
        //                        R12.Padding = 0f;
        //                        R12.Colspan = 3;
        //                        R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R12);
        //                    }
        //                    Health = false;
        //                }


        //                if (dr["RNG4CATG_CODE"].ToString().Trim() == "U")
        //                {
        //                    if (EduCol)
        //                    {
        //                        PdfPCell R11 = new PdfPCell(new Phrase("", Times));
        //                        R11.FixedHeight = 12f;
        //                        R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R11);

        //                        PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
        //                        R12.FixedHeight = 12f;
        //                        R12.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R12);

        //                        PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
        //                        R13.FixedHeight = 12f;
        //                        R13.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R13);
        //                        EduCol = false;
        //                    }

        //                    PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                    R1.FixedHeight = 12f;
        //                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    table.AddCell(R1);

        //                    PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                    R2.FixedHeight = 12f;
        //                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R2.Border = iTextSharp.text.Rectangle.BOX;
        //                    table.AddCell(R2);

        //                    PdfPCell R3 = new PdfPCell(new Phrase("", Times));
        //                    R3.FixedHeight = 12f;
        //                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R3.Border = iTextSharp.text.Rectangle.BOX;
        //                    table.AddCell(R3);
        //                }
        //                else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
        //                {
        //                    PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                    R1.FixedHeight = 12f;
        //                    R1.Colspan = 2;
        //                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                    table.AddCell(R1);

        //                    PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                    R2.FixedHeight = 12f;
        //                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    R2.Border = iTextSharp.text.Rectangle.BOX;
        //                    table.AddCell(R2);
        //                }
        //            }




        //            //if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) == 87)
        //            //{
        //            //    PdfPCell R0 = new PdfPCell(new Phrase("", TblFontBold));
        //            //    R0.FixedHeight = 12f;
        //            //    R0.Colspan = 3;
        //            //    R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //    table.AddCell(R0);

        //            //    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //            //    R01.FixedHeight = 12f;
        //            //    R01.Colspan = 2;
        //            //    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //    Rtable.AddCell(R01);

        //            //    PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //            //    R02.FixedHeight = 12f;
        //            //    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //    R02.Border = iTextSharp.text.Rectangle.BOX;
        //            //    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            //    Rtable.AddCell(R02);


        //            //    break;
        //            //}

        //            //if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim())>40)
        //            //{
        //            //    if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            //    {
        //            //        if (Rtab)
        //            //        {
        //            //            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //            //            R0.FixedHeight = 12f;
        //            //            R0.Colspan = 2;
        //            //            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //            table.AddCell(R0);

        //            //            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //            //            R02.FixedHeight = 12f;
        //            //            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //            R02.Border = iTextSharp.text.Rectangle.BOX;
        //            //            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            //            table.AddCell(R02);

        //            //            RFirst = false; First = true; Rtab = false;
        //            //        }

        //            //        if (!First)
        //            //        {
        //            //            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //            //            R0.FixedHeight = 12f;
        //            //            R0.Colspan = 2;
        //            //            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //            Rtable.AddCell(R0);

        //            //            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //            //            R02.FixedHeight = 12f;
        //            //            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //            R02.Border = iTextSharp.text.Rectangle.BOX;
        //            //            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            //            Rtable.AddCell(R02);
        //            //        }

        //            //        PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //            //        R1.FixedHeight = 12f;
        //            //        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //            //        Rtable.AddCell(R1);

        //            //        PdfPCell R2 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
        //            //        R2.FixedHeight = 12f;
        //            //        R2.Colspan = 2;
        //            //        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //            //        Rtable.AddCell(R2);

        //            //        First = false; Count++;
        //            //    }
        //            //    else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            //    {
        //            //        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //            //        R1.FixedHeight = 12f;
        //            //        R1.Colspan = 2;
        //            //        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
        //            //        Rtable.AddCell(R1);

        //            //        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //            //        R2.FixedHeight = 12f;
        //            //        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R2.Border = iTextSharp.text.Rectangle.BOX;
        //            //        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
        //            //        Rtable.AddCell(R2);
        //            //    }
        //            //}
        //            //else if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            //{
        //            //    if (!First)
        //            //    {
        //            //        PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)" , TblFontBold));
        //            //        R0.FixedHeight = 12f;
        //            //        R0.Colspan = 2;
        //            //        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        table.AddCell(R0);

        //            //        PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //            //        R02.FixedHeight = 12f;
        //            //        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R02.Border = iTextSharp.text.Rectangle.BOX;
        //            //        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            //        table.AddCell(R02);
        //            //    }

        //            //    PdfPCell R1 = new PdfPCell(new Phrase(Count+". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //            //    R1.FixedHeight = 12f;
        //            //    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //            //    table.AddCell(R1);

        //            //    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //            //    R2.FixedHeight = 12f;
        //            //    R2.Colspan = 2;
        //            //    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //            //    table.AddCell(R2);

        //            //    if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

        //            //    First = false; Count++;
        //            //}
        //            //else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //            //{
        //            //    if (!RFirst)
        //            //    {
        //            //        if (EduCol)
        //            //        {
        //            //            PdfPCell R11 = new PdfPCell(new Phrase("", Times));
        //            //            R11.FixedHeight = 12f;
        //            //            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //            table.AddCell(R11);

        //            //            PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
        //            //            R12.FixedHeight = 12f;
        //            //            R12.HorizontalAlignment = Element.ALIGN_CENTER;
        //            //            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //            table.AddCell(R12);

        //            //            PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
        //            //            R13.FixedHeight = 12f;
        //            //            R13.HorizontalAlignment = Element.ALIGN_CENTER;
        //            //            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //            table.AddCell(R13);
        //            //            EduCol = false;
        //            //        }

        //            //        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //            //        R1.FixedHeight = 12f;
        //            //        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        table.AddCell(R1);

        //            //        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //            //        R2.FixedHeight = 12f;
        //            //        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R2.Border = iTextSharp.text.Rectangle.BOX;
        //            //        table.AddCell(R2);

        //            //        PdfPCell R3 = new PdfPCell(new Phrase("", Times));
        //            //        R3.FixedHeight = 12f;
        //            //        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R3.Border = iTextSharp.text.Rectangle.BOX;
        //            //        table.AddCell(R3);
        //            //    }
        //            //    else
        //            //    {
        //            //        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //            //        R1.FixedHeight = 12f;
        //            //        R1.Colspan = 2;
        //            //        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            //        table.AddCell(R1);

        //            //        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //            //        R2.FixedHeight = 12f;
        //            //        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            //        R2.Border = iTextSharp.text.Rectangle.BOX;
        //            //        table.AddCell(R2);
        //            //    }
        //            //}
        //        }

                
        //        PdfPCell O1 = new PdfPCell(table);
        //        O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        outer.AddCell(O1);

        //        PdfPCell O2 = new PdfPCell(Rtable);
        //        O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        outer.AddCell(O2);

        //        document.Add(head);
        //        document.Add(outer);

        //        document.NewPage();
        //        SecondPage(dscategories,document);
                
        //    }

        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //    if (LookupDataAccess.FriendlyName().Contains("2012"))
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }

        //}

        //private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        //{
        //    System.IO.File.Delete(PdfName);
        //}


        //private void SecondPage(DataSet dscategories, Document document)
        //{
        //     BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        //    BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
        //    iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
        //    iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(60, 15, 112));
        //    iTextSharp.text.Font fc2 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(45, 45, 153));
        //    iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
        //    iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

        //    cb.BeginText();

        //    cb.SetFontAndSize(bfTimesBold, 10);
        //    cb.SetRGBColorFill(4, 4, 15);
        //    X_Pos = 300; Y_Pos = 785;
        //    //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
        //    //Y_Pos -= 15;
        //    //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
        //    //Y_Pos -= 20;

        //    if (dscategories.Tables[0].Rows.Count > 0)
        //    {
        //        DataTable dt = dscategories.Tables[0];
        //        DataView dv = new DataView(dt);
        //        dv.Sort = "RNG4CATG_SEQ";
        //        dt = dv.ToTable();

        //        cb.SetFontAndSize(bfTimesBold, 10);
        //        cb.SetRGBColorFill(4, 4, 15);
        //        //cb.SetColorFill(BaseColor.MAGENTA.Darker());
        //        X_Pos = 30; //Y_Pos = 780;
        //        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting:", X_Pos, Y_Pos, 0);
        //        cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        cb.MoveTo(210, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos-2);
        //        cb.LineTo(580, Y_Pos + 8);
        //        cb.LineTo(210, Y_Pos + 8);
        //        cb.ClosePathStroke();

        //        //Y_Pos -= 15;
        //        //cb.SetRGBColorFill(45, 45, 153);
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
        //        //cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        //cb.SetRGBColorFill(160, 160, 160);
        //        //cb.MoveTo(540, Y_Pos);
        //        //cb.LineTo(580, Y_Pos);
        //        //cb.LineTo(580, Y_Pos + 10);
        //        //cb.LineTo(540, Y_Pos + 10);
        //        //cb.ClosePathFillStroke();

        //        //Y_Pos -= 15;
        //        //cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
        //        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
        //        //cb.SetLineWidth(0.5f);
        //        //cb.SetLineCap(5);
        //        //cb.SetRGBColorFill(160, 160, 160);
        //        //cb.MoveTo(540, Y_Pos);
        //        //cb.LineTo(580, Y_Pos);
        //        //cb.LineTo(580, Y_Pos + 10);
        //        //cb.LineTo(540, Y_Pos + 10);
        //        //cb.ClosePathFillStroke();


        //        cb.EndText();

        //        //Temp table not displayed on the screen
        //        PdfPTable head = new PdfPTable(1);
        //        head.HorizontalAlignment = Element.ALIGN_CENTER;
        //        head.TotalWidth = 50f;
        //        PdfPCell headcell = new PdfPCell(new Phrase(""));
        //        headcell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        head.AddCell(headcell);

        //        PdfPTable outer = new PdfPTable(2);
        //        outer.TotalWidth = 500f;
        //        outer.LockedWidth = true;
        //        float[] widths2 = new float[] { 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        outer.SetWidths(widths2);
        //        outer.HorizontalAlignment = Element.ALIGN_LEFT;
        //        outer.SpacingBefore = 50f;

        //        PdfPTable table = new PdfPTable(3);
        //        table.TotalWidth = 240f;
        //        table.LockedWidth = true;
        //        float[] widths = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        table.SetWidths(widths);
        //        //table.SpacingBefore = 270f;

        //        PdfPTable Rtable = new PdfPTable(3);
        //        Rtable.TotalWidth = 240f;
        //        Rtable.LockedWidth = true;
        //        float[] widths1 = new float[] { 120f, 30f, 30f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //        Rtable.SetWidths(widths1);

        //        PdfPCell Header = new PdfPCell(new Phrase("D. HOUSEHOLD LEVEL CHARACTERISTICS", fc1));
        //        Header.Colspan = 3;
        //        Header.HorizontalAlignment = Element.ALIGN_LEFT;
        //        Header.FixedHeight = 15f;
        //        Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        table.AddCell(Header);

        //        PdfPCell RHeader = new PdfPCell(new Phrase(" ", fc1));
        //        RHeader.Colspan = 3;
        //        RHeader.HorizontalAlignment = Element.ALIGN_LEFT;
        //        RHeader.FixedHeight = 15f;
        //        RHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Rtable.AddCell(RHeader);

        //        bool First = true; int Count = 9; bool RFirst = true; bool EduCol = true; bool Rtab = true;
        //        string PrivType = string.Empty; bool Disable = true; bool Health = true;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) > 70)
        //            {
        //                if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) >= 107)
        //                {
        //                    if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                    {
        //                        if (Rtab)
        //                        {
        //                            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                            R0.FixedHeight = 12f;
        //                            R0.Colspan = 2;
        //                            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R0);

        //                            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                            R02.FixedHeight = 12f;
        //                            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R02.Border = iTextSharp.text.Rectangle.BOX;
        //                            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(R02);

        //                            RFirst = false; First = true; Rtab = false;
        //                        }

        //                        if (!First)
        //                        {
        //                            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                            R0.FixedHeight = 12f;
        //                            R0.Colspan = 2;
        //                            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Rtable.AddCell(R0);

        //                            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                            R02.FixedHeight = 12f;
        //                            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R02.Border = iTextSharp.text.Rectangle.BOX;
        //                            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            Rtable.AddCell(R02);

        //                            if (dr["RNG4CATG_CODE"].ToString() == "J")
        //                            {
        //                                PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
        //                                P0.Colspan = 3;
        //                                P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                Rtable.AddCell(P0);
        //                            }
        //                        }

        //                        if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "E")
        //                        {
        //                            PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
        //                            R3.FixedHeight = 12f;
        //                            R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                            Rtable.AddCell(R3);

        //                            PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                            R4.FixedHeight = 12f;
        //                            R4.Colspan = 2;
        //                            R4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            R4.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                            Rtable.AddCell(R4);

        //                            PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
        //                            R1.FixedHeight = 12f;
        //                            R1.Colspan = 3;
        //                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                            Rtable.AddCell(R1);
        //                        }
        //                        else if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
        //                        {
        //                            PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
        //                            R1.FixedHeight = 12f;
        //                            R1.Colspan = 3;
        //                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                            Rtable.AddCell(R1);
        //                        }
        //                        else if (Count == 6 && dr["RNG4CATG_CODE"].ToString().Trim() == "R")
        //                        {
        //                            PdfPCell R1 = new PdfPCell(new Phrase("I.Race", TblFontBold));
        //                            R1.FixedHeight = 12f;
        //                            R1.Colspan = 3;
        //                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                            Rtable.AddCell(R1);
        //                        }
        //                        else
        //                        {
        //                            PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                            R1.FixedHeight = 12f;
        //                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                            Rtable.AddCell(R1);

        //                            PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                            R2.FixedHeight = 12f;
        //                            R2.Colspan = 2;
        //                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                            Rtable.AddCell(R2);
        //                        }

        //                        PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
        //                        //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

        //                        First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && dr["RNG4CATG_CODE"].ToString().Trim() != "E") Count++;
        //                    }
        //                    else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 2;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
        //                        Rtable.AddCell(R1);

        //                        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                        R2.FixedHeight = 12f;
        //                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R2.Border = iTextSharp.text.Rectangle.BOX;
        //                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
        //                        Rtable.AddCell(R2);
        //                    }
        //                }
        //                else if (string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                {
        //                    if (!First)
        //                    {
        //                        if (PrivType == "U")
        //                        {
        //                            PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                            R0.FixedHeight = 12f;
        //                            R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R0);

        //                            PdfPCell R01 = new PdfPCell(new Phrase("0", TblFontBold));
        //                            R01.FixedHeight = 12f;
        //                            R01.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R01.Border = iTextSharp.text.Rectangle.BOX;
        //                            R01.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(R01);

        //                            PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                            R02.FixedHeight = 12f;
        //                            R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                            R02.Border = iTextSharp.text.Rectangle.BOX;
        //                            R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(R02);
        //                        }
        //                        else
        //                        {
        //                            if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N" && PrivType != "N")
        //                            {
        //                                PdfPCell R0 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //                                R0.FixedHeight = 12f;
        //                                R0.Colspan = 2;
        //                                R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(R0);

        //                                PdfPCell R02 = new PdfPCell(new Phrase("0", TblFontBold));
        //                                R02.FixedHeight = 12f;
        //                                R02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                                R02.Border = iTextSharp.text.Rectangle.BOX;
        //                                R02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                                table.AddCell(R02);

        //                                if (dr["RNG4CATG_CODE"].ToString() == "J")
        //                                {
        //                                    PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
        //                                    P0.Colspan = 3;
        //                                    P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Rtable.AddCell(P0);
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (Count == 5 && dr["RNG4CATG_CODE"].ToString().Trim() == "D")
        //                    {
        //                        PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Health", TblFontBold));
        //                        R3.FixedHeight = 12f;
        //                        R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                        table.AddCell(R3);

        //                        PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                        R4.FixedHeight = 12f;
        //                        R4.Colspan = 2;
        //                        R4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R4.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                        table.AddCell(R4);
        //                    }

        //                    if (dr["RNG4CATG_CODE"].ToString().Trim() == "S")
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 3;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
        //                        table.AddCell(R1);
        //                    }
        //                    else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["RNG4CATG_DESC"].ToString().Trim(), TblFontBold));
        //                        R1.FixedHeight = 12f;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                        table.AddCell(R1);

        //                        PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //                        R2.FixedHeight = 12f;
        //                        R2.Colspan = 2;
        //                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
        //                        table.AddCell(R2);
        //                    }

        //                    PrivType = dr["RNG4CATG_CODE"].ToString().Trim();
        //                    //if (dr["RNG4CATG_CODE"].ToString().Trim() == "U") RFirst = false; else RFirst = true;

        //                    First = false; if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N") Count++;
        //                }
        //                else if (!string.IsNullOrEmpty(dr["RNG4CATG_DEM"].ToString().Trim()))
        //                {
        //                    if (dr["RNG4CATG_CODE"].ToString().Trim() == "D" && Disable)
        //                    {
        //                        dv = new DataView(dt);
        //                        dv.RowFilter = "RNG4CATG_CODE='D' AND RNG4CATG_DEM<>''";
        //                        DataTable dtDis = dv.ToTable();

        //                        if (dtDis.Rows.Count > 0)
        //                        {
        //                            PdfPTable NestedTable = new PdfPTable(4);
        //                            NestedTable.TotalWidth = 240f;
        //                            NestedTable.LockedWidth = true;
        //                            float[] Dwidths = new float[] { 100f, 30f, 30f, 30f };
        //                            NestedTable.SetWidths(Dwidths);

        //                            PdfPCell D1 = new PdfPCell(new Phrase("", Times));
        //                            D1.FixedHeight = 12f;
        //                            D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D1);

        //                            foreach (DataRow drDis in dtDis.Rows)
        //                            {
        //                                PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                                D2.FixedHeight = 12f;
        //                                D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                NestedTable.AddCell(D2);
        //                            }

        //                            PdfPCell R11 = new PdfPCell(NestedTable);
        //                            R11.FixedHeight = 12f;
        //                            R11.Colspan = 3;
        //                            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R11);

        //                            if (NestedTable.Rows.Count > 0)
        //                                NestedTable.DeleteBodyRows();

        //                            PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", Times));
        //                            D3.FixedHeight = 12f;
        //                            D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D3);
        //                            for (int i = 0; i < dtDis.Rows.Count; i++)
        //                            {
        //                                PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                                R2.FixedHeight = 12f;
        //                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                                R2.Border = iTextSharp.text.Rectangle.BOX;
        //                                NestedTable.AddCell(R2);
        //                            }

        //                            PdfPCell R12 = new PdfPCell(NestedTable);
        //                            R12.FixedHeight = 12f;
        //                            R12.Colspan = 3;
        //                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R12);
        //                        }
        //                        Disable = false;

        //                    }

        //                    if (dr["RNG4CATG_CODE"].ToString().Trim() == "N" && Health)
        //                    {
        //                        dv = new DataView(dt);
        //                        dv.RowFilter = "RNG4CATG_CODE='N' AND RNG4CATG_DEM<>''";
        //                        DataTable dtDis = dv.ToTable();

        //                        if (dtDis.Rows.Count > 0)
        //                        {
        //                            PdfPTable NestedTable = new PdfPTable(4);
        //                            NestedTable.TotalWidth = 250f;
        //                            NestedTable.LockedWidth = true;
        //                            float[] Dwidths = new float[] { 100f, 30f, 30f, 30f };
        //                            NestedTable.SetWidths(Dwidths);

        //                            PdfPCell D1 = new PdfPCell(new Phrase("", Times));
        //                            D1.FixedHeight = 12f;
        //                            D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D1);

        //                            foreach (DataRow drDis in dtDis.Rows)
        //                            {
        //                                PdfPCell D2 = new PdfPCell(new Phrase(drDis["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                                D2.FixedHeight = 12f;
        //                                D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                NestedTable.AddCell(D2);
        //                            }

        //                            PdfPCell R11 = new PdfPCell(NestedTable);
        //                            R11.FixedHeight = 12f;
        //                            R11.Colspan = 3;
        //                            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R11);

        //                            if (NestedTable.Rows.Count > 0)
        //                                NestedTable.DeleteBodyRows();

        //                            PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance*", Times));
        //                            D3.FixedHeight = 12f;
        //                            D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            NestedTable.AddCell(D3);
        //                            for (int i = 0; i < dtDis.Rows.Count; i++)
        //                            {
        //                                PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                                R2.FixedHeight = 12f;
        //                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                                R2.Border = iTextSharp.text.Rectangle.BOX;
        //                                NestedTable.AddCell(R2);
        //                            }

        //                            PdfPCell R12 = new PdfPCell(NestedTable);
        //                            R12.FixedHeight = 12f;
        //                            R12.Colspan = 3;
        //                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R12);
        //                        }
        //                        Health = false;
        //                    }


        //                    if (dr["RNG4CATG_CODE"].ToString().Trim() == "U")
        //                    {
        //                        if (EduCol)
        //                        {
        //                            PdfPCell R11 = new PdfPCell(new Phrase("", Times));
        //                            R11.FixedHeight = 12f;
        //                            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R11);

        //                            PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
        //                            R12.FixedHeight = 12f;
        //                            R12.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R12);

        //                            PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
        //                            R13.FixedHeight = 12f;
        //                            R13.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(R13);
        //                            EduCol = false;
        //                        }

        //                        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                        R1.FixedHeight = 12f;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R1);

        //                        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                        R2.FixedHeight = 12f;
        //                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R2.Border = iTextSharp.text.Rectangle.BOX;
        //                        table.AddCell(R2);

        //                        PdfPCell R3 = new PdfPCell(new Phrase("", Times));
        //                        R3.FixedHeight = 12f;
        //                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R3.Border = iTextSharp.text.Rectangle.BOX;
        //                        table.AddCell(R3);
        //                    }
        //                    else if (dr["RNG4CATG_CODE"].ToString().Trim() != "D" && dr["RNG4CATG_CODE"].ToString().Trim() != "N")
        //                    {
        //                        PdfPCell R1 = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), Times));
        //                        R1.FixedHeight = 12f;
        //                        R1.Colspan = 2;
        //                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(R1);

        //                        PdfPCell R2 = new PdfPCell(new Phrase("", Times));
        //                        R2.FixedHeight = 12f;
        //                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        R2.Border = iTextSharp.text.Rectangle.BOX;
        //                        table.AddCell(R2);
        //                    }
        //                }
        //            }
        //        }
        //        //if (int.Parse(dr["RNG4CATG_SEQ"].ToString().Trim()) > 133)
        //        //{
        //            PdfPCell L0 = new PdfPCell(new Phrase("", TblFontBold));
        //            L0.FixedHeight = 12f;
        //            L0.Colspan = 3;
        //            L0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            table.AddCell(L0);

        //            PdfPCell L01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
        //            L01.FixedHeight = 12f;
        //            L01.Colspan = 2;
        //            L01.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Rtable.AddCell(L01);

        //            PdfPCell L02 = new PdfPCell(new Phrase("0", TblFontBold));
        //            L02.FixedHeight = 12f;
        //            L02.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            L02.Border = iTextSharp.text.Rectangle.BOX;
        //            L02.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            Rtable.AddCell(L02);

        //            PdfPCell L03 = new PdfPCell(new Phrase("", TblFontBold));
        //            L03.FixedHeight = 12f;
        //            L03.Colspan = 3;
        //            L03.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Rtable.AddCell(L03);


        //            PdfPCell O1 = new PdfPCell(table);
        //            O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            outer.AddCell(O1);

        //            PdfPCell O2 = new PdfPCell(Rtable);
        //            O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            outer.AddCell(O2);

        //            PdfPTable LastOuter = new PdfPTable(1);
        //            LastOuter.TotalWidth = 520f;
        //            LastOuter.LockedWidth = true;
        //            //float[] Lastwidths2 = new float[] { 150f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //            //Last.SetWidths(Lastwidths2);
        //            LastOuter.HorizontalAlignment = Element.ALIGN_LEFT;
        //            LastOuter.SpacingBefore = 30f;

        //            PdfPTable Last = new PdfPTable(3);
        //            Last.TotalWidth = 500f;
        //            Last.LockedWidth = true;
        //            float[] Lastwidths2 = new float[] { 150f, 50f,50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //            Last.SetWidths(Lastwidths2);
        //            Last.HorizontalAlignment = Element.ALIGN_LEFT;
        //            Last.SpacingBefore = 30f;

        //            //iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, new iTextSharp.text.BaseColor(204, 169, 217));

        //            PdfPCell S0 = new PdfPCell(new Phrase("E. Number of Individuals Not Included in the Totals Above (due to data collection system integration barriers)", fc2));
        //            S0.FixedHeight = 12f;
        //            S0.Colspan = 3;
        //            S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(S0);

        //            PdfPCell S1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of INDIVIDUALS served in each program*:", Times));
        //            S1.FixedHeight = 12f;
        //            //S1.Colspan = 3;
        //            S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(S1);

        //            PdfPCell S2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
        //            S2.FixedHeight = 12f;
        //            S2.HorizontalAlignment = Element.ALIGN_CENTER;
        //            S2.Border = iTextSharp.text.Rectangle.BOX;
        //            S2.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            Last.AddCell(S2);

        //            PdfPCell S3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //            S3.FixedHeight = 12f;
        //            S3.HorizontalAlignment = Element.ALIGN_CENTER;
        //            S3.Border = iTextSharp.text.Rectangle.BOX;
        //            S3.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            Last.AddCell(S3);

        //            for (int i = 0; i < 2; i++)
        //            {
        //                PdfPCell T1 = new PdfPCell(new Phrase("", Times));
        //                T1.FixedHeight = 12f;
        //                //S1.Colspan = 3;
        //                T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                Last.AddCell(T1);

        //                PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
        //                T2.FixedHeight = 12f;
        //                T2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                T2.Border = iTextSharp.text.Rectangle.BOX;
        //                T2.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                Last.AddCell(T2);

        //                PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
        //                T3.FixedHeight = 12f;
        //                T3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                T3.Border = iTextSharp.text.Rectangle.BOX;
        //                T3.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                Last.AddCell(T3);
        //            }

        //            PdfPCell M0 = new PdfPCell(new Phrase("F. Number of Households Not Included in the Totals Above (due to data collection system integration barriers)", fc1));
        //            M0.FixedHeight = 12f;
        //            M0.Colspan = 3;
        //            M0.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(M0);

        //            PdfPCell M1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of HOUSEHOLDS served in each program*:", Times));
        //            M1.FixedHeight = 12f;
        //            //S1.Colspan = 3;
        //            M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(M1);

        //            PdfPCell M2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
        //            M2.FixedHeight = 12f;
        //            M2.HorizontalAlignment = Element.ALIGN_CENTER;
        //            M2.Border = iTextSharp.text.Rectangle.BOX;
        //            M2.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            Last.AddCell(M2);

        //            PdfPCell M3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
        //            M3.FixedHeight = 12f;
        //            M3.HorizontalAlignment = Element.ALIGN_CENTER;
        //            M3.Border = iTextSharp.text.Rectangle.BOX;
        //            M3.BackgroundColor = BaseColor.LIGHT_GRAY;
        //            Last.AddCell(M3);

        //            for (int i = 0; i < 2; i++)
        //            {
        //                PdfPCell T1 = new PdfPCell(new Phrase("", Times));
        //                T1.FixedHeight = 12f;
        //                //S1.Colspan = 3;
        //                T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                Last.AddCell(T1);

        //                PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
        //                T2.FixedHeight = 12f;
        //                T2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                T2.Border = iTextSharp.text.Rectangle.BOX;
        //                T2.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                Last.AddCell(T2);

        //                PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
        //                T3.FixedHeight = 12f;
        //                T3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                T3.Border = iTextSharp.text.Rectangle.BOX;
        //                T3.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                Last.AddCell(T3);
        //            }

        //            PdfPCell M5 = new PdfPCell(new Phrase("  *The system will add rows to allow reporting on multiple programs.", TblFontSmall));
        //            M5.FixedHeight = 12f;
        //            M5.Colspan = 3;
        //            M5.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(M5);

        //            PdfPCell M6 = new PdfPCell(new Phrase(" ", TblFontSmall));
        //            M6.FixedHeight = 5f;
        //            M6.Colspan = 3;
        //            M6.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            Last.AddCell(M6);

        //            PdfPCell LO1 = new PdfPCell(Last);
        //            //LO1.Border = iTextSharp.text.Rectangle.BOX;
        //            LO1.BorderColor = BaseColor.RED;
        //            LO1.BorderWidth = 2f;
        //            LastOuter.AddCell(LO1);

        //            document.Add(head);
        //            document.Add(outer);

        //            document.Add(LastOuter);
        //            //break;
        //        //}
        //    }
        //}

        #region Report Period Counts

        private void On_SaveForm_Counts_Pdf()
        {

            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "RNGB0004_Report";
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

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            cb = writer.DirectContent;

            HeaderPage(document,writer);

            document.NewPage();

            cb.BeginText();
            cb.SetFontAndSize(bfTimes, 12);

            DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

            //Agency Control Table
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


            cb.SetFontAndSize(bfTimesBold, 10);
            cb.SetRGBColorFill(4, 4, 15);
            X_Pos = 300; Y_Pos = 785;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;

            if (dt.Rows.Count > 0)
            {
                DataTable dtHead = new DataTable();
                DataView dvHead = new DataView(dt);
                dvHead.RowFilter = "Sum_Catg_Code='4'";
                dtHead = dvHead.ToTable();

                string Indv = string.Empty, HH = string.Empty;
                if (dtHead.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtHead.Rows[1]["Sum_Child_Period_Count"].ToString().Trim()))
                        Indv = dtHead.Rows[1]["Sum_Child_Period_Count"].ToString();
                    if (!string.IsNullOrEmpty(dtHead.Rows[2]["Sum_Child_Period_Count"].ToString().Trim()))
                        HH = dtHead.Rows[2]["Sum_Child_Period_Count"].ToString();
                }

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);  
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();


                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of Local Agency Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName+ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                Y_Pos -= 20;
                //cb.SetRGBColorFill(45, 45, 153);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos + 8);
                //cb.LineTo(540, Y_Pos + 8);
                //cb.ClosePathFillStroke();

                //Y_Pos -= 15;
                //cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos + 8);
                //cb.LineTo(540, Y_Pos + 8);
                //cb.ClosePathFillStroke();

                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 75f;
                PdfPCell headcell = new PdfPCell(new Phrase(""));
                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                head.AddCell(headcell);

                PdfPTable THead = new PdfPTable(2);
                THead.TotalWidth = 500f;
                THead.LockedWidth = true;
                float[] twidths2 = new float[] { 80f, 14f };
                THead.HorizontalAlignment = Element.ALIGN_LEFT;
                THead.SetWidths(twidths2);
                //THead.SpacingBefore = 55f;
                //THead.SpacingBefore = 75f;

                iTextSharp.text.Font TblFntColor = new iTextSharp.text.Font(bfTimes, 9, 2);
                iTextSharp.text.Font hhdcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
                PdfPCell T1 = new PdfPCell(new Phrase("A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", hhdcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T1);

                PdfPCell T2 = new PdfPCell(new Phrase(Indv.ToString(), TblFntColor));
                T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                T2.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T2);

                PdfPCell T5 = new PdfPCell(new Phrase(Indv.ToString(), TblFntColor));
                T5.Colspan = 2;
                T5.FixedHeight = 12f;
                //T5.BackgroundColor = BaseColor.LIGHT_GRAY;
                T5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T5);


                iTextSharp.text.Font Indcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(60, 15, 112));
                PdfPCell T3 = new PdfPCell(new Phrase("B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", Indcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T3);

                PdfPCell T4 = new PdfPCell(new Phrase(HH.ToString(), TblFntColor));
                T4.HorizontalAlignment = Element.ALIGN_RIGHT;
                T4.BackgroundColor = BaseColor.LIGHT_GRAY;
                T4.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T4);

                PdfPTable outer = new PdfPTable(2);
                outer.TotalWidth = 500f;
                outer.LockedWidth = true;
                float[] widths2 = new float[] { 50f, 50f };
                outer.HorizontalAlignment = Element.ALIGN_LEFT;
                outer.SetWidths(widths2);
                outer.SpacingBefore = 20f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f, 35f };
                table.SetWidths(widths);
                

                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 35f, 35f };
                Rtable.SetWidths(widths1);

                PdfPCell Header = new PdfPCell(new Phrase("C. INDIVIDUAL LEVEL CHARACTERISTICS", fc1));
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

                bool First = true; int Count = 1; bool RFirst = true; bool EduCol = true; bool Rtab = true;
                string PrivType = string.Empty; bool Disable = true; bool Health = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        PdfPCell R0 = new PdfPCell(new Phrase("", TblFontBold));
                        R0.FixedHeight = 12f;
                        R0.Colspan = 3;
                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(R0);

                        PdfPCell R03 = new PdfPCell(new Phrase("", TblFontBold));
                        R03.FixedHeight = 12f;
                        R03.Colspan = 3;
                        R03.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Rtable.AddCell(R03);
                        break;
                    }

                    if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                    {
                        if (dr["Sum_Catg_Code"].ToString() == "E")
                        {
                            PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
                            R3.FixedHeight = 12f;
                            R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R3);

                            PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                            R4.FixedHeight = 12f;
                            R4.Colspan = 2;
                            R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R4.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R4);

                            PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 3;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R1);

                            Count++;
                        }
                        else if (dr["Sum_Catg_Code"].ToString() == "R")
                        {
                            Count--;

                            PdfPCell SR1 = new PdfPCell(new Phrase("", TblFontBold));
                            SR1.FixedHeight = 12f;
                            SR1.Colspan = 3;
                            SR1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(SR1);

                            PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 3;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(R1);

                            Count++;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "D")
                        {
                            PdfPCell R1 = new PdfPCell(new Phrase(Count + ". Health" , TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R1);

                            PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                            R2.FixedHeight = 12f;
                            R2.Colspan = 2;
                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R2);

                            Disable = true;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (Count > 6)
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "W" || dr["Sum_Catg_Code"].ToString().Trim() == "V")
                                {
                                    PdfPTable NestedTable = new PdfPTable(2);
                                    NestedTable.TotalWidth = 150f;
                                    NestedTable.LockedWidth = true;
                                    float[] N2Dwidths = new float[] { 50f, 60f };
                                    NestedTable.SetWidths(N2Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    D1.FixedHeight = 12f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    D1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(D1);

                                    PdfPCell sD1 = new PdfPCell(new Phrase("(Individuals 18+)", fcRed));
                                    sD1.FixedHeight = 12f;
                                    sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(sD1);

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.Padding = 0f;
                                    R11.Colspan = 1;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R11);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);
                                }
                            }
                            else
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "S")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase("  "+dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    //PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    //R2.FixedHeight = 12f;
                                    //R2.Colspan = 2;
                                    //R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    //table.AddCell(R2);
                                    Count--;
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R2);
                                }
                            }

                            Count++;
                        }
                    }
                    else
                    {
                        if (dr["Sum_Catg_Code"].ToString().Trim() == "D" && Disable)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='D' AND Sum_Child_Code<>'STATICHEAD' ";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(5);
                                NestedTable.TotalWidth = 240f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 80f, 30f, 30f, 30f,30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(5);
                                NestedTable1.TotalWidth = 240f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 3;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", TableFont));
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);
                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 3;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);
                            }
                            Disable = false;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "N" && Health)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='N' AND Sum_Child_Code<>'STATICHEAD' ";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(5);
                                NestedTable.TotalWidth = 240f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(5);
                                NestedTable1.TotalWidth = 240f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 3;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance", TableFont));
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);
                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 3;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);
                            }

                            PdfPCell R13 = new PdfPCell(new Phrase("*If an individual reported that they had Health Insurance please identify the source of health insurance below.", TblFontSmall));
                            R13.FixedHeight = 12f;
                            R13.Colspan = 3;
                            R13.HorizontalAlignment = Element.ALIGN_LEFT;
                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(R13);

                            Health = false; 
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count > 6)
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    R01.Colspan = 2;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), TblFontBold));
                                    R02.FixedHeight = 12f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Rtable.AddCell(R02);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        //R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Count"].ToString().Trim(), TblFontBold));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);

                                        PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), TblFontBold));
                                        R03.FixedHeight = 12f;
                                        R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R03.Border = iTextSharp.text.Rectangle.BOX;
                                        R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R03);
                                    }
                                    else
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), TblFontBold));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);
                                    }

                                    if (dr["Sum_Catg_Code"].ToString().Trim() == "S") Count++;
                                }
                            }
                            else 
                            {
                                if (Count > 6)
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "S")
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase("  " + dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Colspan = 2;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);
                                    }
                                    else if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        if (EduCol)
                                        {
                                            PdfPCell R11 = new PdfPCell(new Phrase("", Times));
                                            R11.FixedHeight = 12f;
                                            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R11);

                                            PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
                                            R12.FixedHeight = 12f;
                                            R12.HorizontalAlignment = Element.ALIGN_CENTER;
                                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R12);

                                            PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
                                            R13.FixedHeight = 12f;
                                            R13.HorizontalAlignment = Element.ALIGN_CENTER;
                                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R13);
                                            EduCol = false;
                                        }

                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Count"].ToString(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(R2);

                                        PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R3.FixedHeight = 12f;
                                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R3.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(R3);
                                    }
                                    else
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Colspan = 2;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);
                                    }
                                }
                            }
                        }
                    }
                }

                PdfPCell O1 = new PdfPCell(table);
                O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O1);

                PdfPCell O2 = new PdfPCell(Rtable);
                O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O2);

                document.Add(head);
                document.Add(THead);
                document.Add(outer);

                document.NewPage();
                SecondPageCounts(dt, document);

                ////ADDED BY SUDHEER ON 11/16/23
                if (chkbMontCounty.Checked && dtCounty.Rows.Count > 0)
                {
                    document.NewPage();
                    CountyWiseReport(dtCounty, document);
                }

                if (chkbLangCounts.Checked && dtLanguage.Rows.Count > 0)
                {
                    document.NewPage();
                    LanguageCountsReport(dtLanguage, document);
                }
            }

            document.Close();
            fs.Close();
            fs.Dispose();

            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, ds, dt, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0004",Bypass_Rep_Name,MST_Rep_Name,Ind_Rep_Name, BaseForm);
            //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{
            //    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName);
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}

        }

        private void SecondPageCounts(DataTable dtCounts, Document document)
        {

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(60, 15, 112));
            iTextSharp.text.Font fc2 = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
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
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 20;

            //Agency Control Table
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

            if (dtCounts.Rows.Count > 0)
            {
                //DataTable dt = dscategories.Tables[0];
                //DataView dv = new DataView(dt);
                //dv.Sort = "RNG4CATG_SEQ";
                //dt = dv.ToTable();

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();


                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 75f;
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
                //outer.SpacingBefore = 75f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table.SetWidths(widths);
                //table.SpacingBefore = 270f;

                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
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
                foreach (DataRow dr in dtCounts.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                        {
                            if (Count >= 13)
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.Colspan = 2;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R2);
                            }
                            else
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.Colspan = 2;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R2);
                            }

                            Count++;
                        }
                        else
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count > 13)
                                {
                                    if (dr["Sum_Catg_Code"].ToString() != "J" && dr["Sum_Catg_Code"].ToString() != "K")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Rtable.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), TblFontBold));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        Rtable.AddCell(R02);

                                        if (dr["Sum_Catg_Code"].ToString() == "I")
                                        {
                                            PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
                                            P0.Colspan = 3;
                                            P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Rtable.AddCell(P0);
                                        }
                                    }
                                }
                                else
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    R01.Colspan = 2;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), TblFontBold));
                                    R02.FixedHeight = 12f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R02);
                                }
                            }
                            else
                            {
                                if (Count > 13)
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R2);
                                }
                            }
                        }
                    }
                }
                PdfPCell L0 = new PdfPCell(new Phrase("", TblFontBold));
                L0.FixedHeight = 12f;
                L0.Colspan = 3;
                L0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(L0);

                PdfPCell L03 = new PdfPCell(new Phrase("", TblFontBold));
                L03.FixedHeight = 12f;
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
                S0.FixedHeight = 12f;
                S0.Colspan = 3;
                S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S0);

                PdfPCell S1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of INDIVIDUALS served in each program*:", Times));
                S1.FixedHeight = 12f;
                //S1.Colspan = 3;
                S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S1);

                PdfPCell S2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                S2.FixedHeight = 12f;
                S2.HorizontalAlignment = Element.ALIGN_CENTER;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                S2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S2);

                PdfPCell S3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                S3.FixedHeight = 12f;
                S3.HorizontalAlignment = Element.ALIGN_CENTER;
                S3.Border = iTextSharp.text.Rectangle.BOX;
                S3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TableFont));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M0 = new PdfPCell(new Phrase("F. Number of Households Not Included in the Totals Above (due to data collection system integration barriers)", fc1));
                M0.FixedHeight = 12f;
                M0.Colspan = 3;
                M0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M0);

                PdfPCell M1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of HOUSEHOLDS served in each program*:", Times));
                M1.FixedHeight = 12f;
                //S1.Colspan = 3;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M1);

                PdfPCell M2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                M2.FixedHeight = 12f;
                M2.HorizontalAlignment = Element.ALIGN_CENTER;
                M2.Border = iTextSharp.text.Rectangle.BOX;
                M2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M2);

                PdfPCell M3 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                M3.FixedHeight = 12f;
                M3.HorizontalAlignment = Element.ALIGN_CENTER;
                M3.Border = iTextSharp.text.Rectangle.BOX;
                M3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                    T3.BorderColor = BaseColor.WHITE;
                    //T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M5 = new PdfPCell(new Phrase("  *The system will add rows to allow reporting on multiple programs.", TblFontSmall));
                M5.FixedHeight = 12f;
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

        #endregion

        #region Cummilative Counts


        private void On_SaveForm_Cummilative_Counts_Pdf()
        {

            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "RNGB0004_Report";
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

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            cb = writer.DirectContent;

            HeaderPage(document,writer);

            document.NewPage();

            cb.BeginText();
            cb.SetFontAndSize(bfTimes, 12);

            DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

            //Agency Control Table
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


            cb.SetFontAndSize(bfTimesBold, 10);
            cb.SetRGBColorFill(4, 4, 15);
            X_Pos = 300; Y_Pos = 785;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;

            if (dt.Rows.Count > 0)
            {
                DataTable dtHead = new DataTable();
                DataView dvHead = new DataView(dt);
                dvHead.RowFilter = "Sum_Catg_Code='4'";
                dtHead = dvHead.ToTable();

                string Indv = string.Empty, HH = string.Empty;
                if (dtHead.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtHead.Rows[1]["Sum_Child_Cum_Count"].ToString().Trim()))
                        Indv = dtHead.Rows[1]["Sum_Child_Cum_Count"].ToString();
                    if (!string.IsNullOrEmpty(dtHead.Rows[2]["Sum_Child_Cum_Count"].ToString().Trim()))
                        HH = dtHead.Rows[2]["Sum_Child_Cum_Count"].ToString();
                }



                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());

                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();
                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of Local Agency Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.SetRGBColorFill(45, 45, 153);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos + 8);
                //cb.LineTo(540, Y_Pos + 8);
                //cb.ClosePathFillStroke();

                //Y_Pos -= 15;
                //cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos + 8);
                //cb.LineTo(540, Y_Pos + 8);
                //cb.ClosePathFillStroke();

                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 75f;
                PdfPCell headcell = new PdfPCell(new Phrase(""));
                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                head.AddCell(headcell);

                PdfPTable THead = new PdfPTable(2);
                THead.TotalWidth = 500f;
                THead.LockedWidth = true;
                float[] twidths2 = new float[] { 80f, 14f };
                THead.HorizontalAlignment = Element.ALIGN_LEFT;
                THead.SetWidths(twidths2);
                //THead.SpacingBefore = 75f;

                iTextSharp.text.Font TblFntColor = new iTextSharp.text.Font(bfTimes, 9, 2);
                iTextSharp.text.Font hhdcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
                PdfPCell T1 = new PdfPCell(new Phrase("A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", hhdcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T1);

                PdfPCell T2 = new PdfPCell(new Phrase(Indv.ToString(), TblFntColor));
                T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                T2.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T2);

                PdfPCell T5 = new PdfPCell(new Phrase(Indv.ToString(), TblFntColor));
                T5.Colspan = 2;
                T5.FixedHeight = 12f;
                //T5.BackgroundColor = BaseColor.LIGHT_GRAY;
                T5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T5);


                iTextSharp.text.Font Indcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(60, 15, 112));
                PdfPCell T3 = new PdfPCell(new Phrase("B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", Indcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T3);

                PdfPCell T4 = new PdfPCell(new Phrase(HH.ToString(), TblFntColor));
                T4.HorizontalAlignment = Element.ALIGN_RIGHT;
                T4.BackgroundColor = BaseColor.LIGHT_GRAY;
                T4.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T4);

                PdfPTable outer = new PdfPTable(2);
                outer.TotalWidth = 500f;
                outer.LockedWidth = true;
                float[] widths2 = new float[] { 50f, 50f };
                outer.HorizontalAlignment = Element.ALIGN_LEFT;
                outer.SetWidths(widths2);
                outer.SpacingBefore = 20f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f, 35f };
                table.SetWidths(widths);


                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 35f, 35f };
                Rtable.SetWidths(widths1);

                PdfPCell Header = new PdfPCell(new Phrase("C. INDIVIDUAL LEVEL CHARACTERISTICS", fc1));
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

                bool First = true; int Count = 1; bool RFirst = true; bool EduCol = true; bool Rtab = true;
                string PrivType = string.Empty; bool Disable = true; bool Health = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        PdfPCell R0 = new PdfPCell(new Phrase("", TblFontBold));
                        R0.FixedHeight = 12f;
                        R0.Colspan = 3;
                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(R0);

                        PdfPCell R03 = new PdfPCell(new Phrase("", TblFontBold));
                        R03.FixedHeight = 12f;
                        R03.Colspan = 3;
                        R03.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Rtable.AddCell(R03);
                        break;
                    }

                    if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                    {
                        if (dr["Sum_Catg_Code"].ToString() == "E")
                        {
                            PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
                            R3.FixedHeight = 12f;
                            R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R3);

                            PdfPCell R4 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                            R4.FixedHeight = 12f;
                            R4.Colspan = 2;
                            R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R4.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R4);

                            PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 3;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R1);

                            Count++;
                        }
                        else if (dr["Sum_Catg_Code"].ToString() == "R")
                        {
                            Count--;

                            PdfPCell SR1 = new PdfPCell(new Phrase("", TblFontBold));
                            SR1.FixedHeight = 12f;
                            SR1.Colspan = 3;
                            SR1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(SR1);

                            PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 3;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(R1);

                            Count++;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "D")
                        {
                            PdfPCell R1 = new PdfPCell(new Phrase(Count + ". Health", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R1);

                            PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                            R2.FixedHeight = 12f;
                            R2.Colspan = 2;
                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R2);

                            Disable = true;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (Count > 6)
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "W" || dr["Sum_Catg_Code"].ToString().Trim() == "V")
                                {
                                    PdfPTable NestedTable = new PdfPTable(2);
                                    NestedTable.TotalWidth = 150f;
                                    NestedTable.LockedWidth = true;
                                    float[] N2Dwidths = new float[] { 50f, 60f };
                                    NestedTable.SetWidths(N2Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    D1.FixedHeight = 12f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    D1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(D1);

                                    PdfPCell sD1 = new PdfPCell(new Phrase("(Individuals 18+)", fcRed));
                                    sD1.FixedHeight = 12f;
                                    sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(sD1);

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.Padding = 0f;
                                    R11.Colspan = 1;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R11);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);
                                }
                            }
                            else
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "S")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase("  " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    //PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    //R2.FixedHeight = 12f;
                                    //R2.Colspan = 2;
                                    //R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    //table.AddCell(R2);
                                    Count--;
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R2);
                                }
                            }

                            Count++;
                        }
                    }
                    else
                    {
                        if (dr["Sum_Catg_Code"].ToString().Trim() == "D" && Disable)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='D' AND Sum_Child_Code<>'STATICHEAD' ";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(5);
                                NestedTable.TotalWidth = 240f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(5);
                                NestedTable1.TotalWidth = 240f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 3;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", Times));
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);
                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 3;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);
                            }
                            Disable = false;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "N" && Health)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='N' AND Sum_Child_Code<>'STATICHEAD' AND Sum_Child_Code<>'STATICTOTL'";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(5);
                                NestedTable.TotalWidth = 240f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(5);
                                NestedTable1.TotalWidth = 240f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 80f, 30f, 30f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 3;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance", Times));
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);
                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 3;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);
                            }

                            PdfPCell R13 = new PdfPCell(new Phrase("*If an individual reported that they had Health Insurance please identify the source of health insurance below.", TblFontSmall));
                            R13.FixedHeight = 12f;
                            R13.Colspan = 3;
                            R13.HorizontalAlignment = Element.ALIGN_LEFT;
                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(R13);

                            Health = false;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count > 6)
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    R01.Colspan = 2;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R02.FixedHeight = 12f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Rtable.AddCell(R02);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        //R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Cum_Count"].ToString().Trim(), Times));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);

                                        PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R03.FixedHeight = 12f;
                                        R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R03.Border = iTextSharp.text.Rectangle.BOX;
                                        R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R03);
                                    }
                                    else
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);
                                    }

                                    if (dr["Sum_Catg_Code"].ToString().Trim() == "S") Count++;
                                }
                            }
                            else
                            {
                                if (Count > 6)
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "S")
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase("  " + dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Colspan = 2;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);
                                    }
                                    else if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        if (EduCol)
                                        {
                                            PdfPCell R11 = new PdfPCell(new Phrase("", Times));
                                            R11.FixedHeight = 12f;
                                            R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R11);

                                            PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
                                            R12.FixedHeight = 12f;
                                            R12.HorizontalAlignment = Element.ALIGN_CENTER;
                                            R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R12);

                                            PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
                                            R13.FixedHeight = 12f;
                                            R13.HorizontalAlignment = Element.ALIGN_CENTER;
                                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(R13);
                                            EduCol = false;
                                        }

                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Cum_Count"].ToString(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(R2);

                                        PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R3.FixedHeight = 12f;
                                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R3.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(R3);
                                    }
                                    else
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Colspan = 2;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);
                                    }
                                }
                            }
                        }
                    }
                }

                PdfPCell O1 = new PdfPCell(table);
                O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O1);

                PdfPCell O2 = new PdfPCell(Rtable);
                O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O2);

                document.Add(head);
                document.Add(THead);
                document.Add(outer);

                document.NewPage();
                SecondPage_Cummilative_Counts(dt, document);

                ////ADDED BY SUDHEER ON 11/16/23
                if (chkbMontCounty.Checked && dtCounty.Rows.Count > 0)
                {
                    document.NewPage();
                    CountyWiseReport(dtCounty, document);
                }

                if (chkbLangCounts.Checked && dtLanguage.Rows.Count > 0)
                {
                    document.NewPage();
                    LanguageCountsReport(dtLanguage, document);
                }
            }

            document.Close();
            fs.Close();
            fs.Dispose();


            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, ds, dt, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0004", Bypass_Rep_Name, MST_Rep_Name, Ind_Rep_Name, BaseForm);
            //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{
            //    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName);
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}

        }

        private void SecondPage_Cummilative_Counts(DataTable dtCounts, Document document)
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
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 20;

            //Agency Control Table
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

            if (dtCounts.Rows.Count > 0)
            {
                //DataTable dt = dscategories.Tables[0];
                //DataView dv = new DataView(dt);
                //dv.Sort = "RNG4CATG_SEQ";
                //dt = dv.ToTable();

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 75f;
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
                //outer.SpacingBefore = 75f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table.SetWidths(widths);
                //table.SpacingBefore = 270f;

                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 35f, 31f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
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
                foreach (DataRow dr in dtCounts.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                        {
                            if (Count >= 13)
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.Colspan = 2;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R2);
                            }
                            else
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.Colspan = 2;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R2);
                            }

                            Count++;
                        }
                        else
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count > 13)
                                {
                                    if (dr["Sum_Catg_Code"].ToString() != "J" && dr["Sum_Catg_Code"].ToString() != "K")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Rtable.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), TblFontBold));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        Rtable.AddCell(R02);

                                        if (dr["Sum_Catg_Code"].ToString() == "I")
                                        {
                                            PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
                                            P0.Colspan = 3;
                                            P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Rtable.AddCell(P0);
                                        }
                                    }
                                }
                                else
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    R01.Colspan = 2;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R02.FixedHeight = 12f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R02);
                                }
                            }
                            else
                            {
                                if (Count > 13)
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 2;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R2);
                                }
                            }
                        }
                    }
                }
                PdfPCell L0 = new PdfPCell(new Phrase("", TblFontBold));
                L0.FixedHeight = 12f;
                L0.Colspan = 3;
                L0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(L0);

                PdfPCell L03 = new PdfPCell(new Phrase("", TblFontBold));
                L03.FixedHeight = 12f;
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
                S0.FixedHeight = 12f;
                S0.Colspan = 3;
                S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S0);

                PdfPCell S1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of INDIVIDUALS served in each program*:", Times));
                S1.FixedHeight = 12f;
                //S1.Colspan = 3;
                S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S1);

                PdfPCell S2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                S2.FixedHeight = 12f;
                S2.HorizontalAlignment = Element.ALIGN_CENTER;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                S2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S2);

                PdfPCell S3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                S3.FixedHeight = 12f;
                S3.HorizontalAlignment = Element.ALIGN_CENTER;
                S3.Border = iTextSharp.text.Rectangle.BOX;
                S3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M0 = new PdfPCell(new Phrase("F. Number of Households Not Included in the Totals Above (due to data collection system integration barriers)", fc1));
                M0.FixedHeight = 12f;
                M0.Colspan = 3;
                M0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M0);

                PdfPCell M1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of HOUSEHOLDS served in each program*:", Times));
                M1.FixedHeight = 12f;
                //S1.Colspan = 3;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M1);

                PdfPCell M2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                M2.FixedHeight = 12f;
                M2.HorizontalAlignment = Element.ALIGN_CENTER;
                M2.Border = iTextSharp.text.Rectangle.BOX;
                M2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M2);

                PdfPCell M3 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                M3.FixedHeight = 12f;
                M3.HorizontalAlignment = Element.ALIGN_CENTER;
                M3.Border = iTextSharp.text.Rectangle.BOX;
                M3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M5 = new PdfPCell(new Phrase("  *The system will add rows to allow reporting on multiple programs.", TblFontSmall));
                M5.FixedHeight = 12f;
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


        #endregion

        #region Both Counts


        private void On_SaveForm_Both_Counts_Pdf()
        {

            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "RNGB0004_Report";
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

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            cb = writer.DirectContent;

            HeaderPage(document,writer);

            document.NewPage();

            cb.BeginText();
            cb.SetFontAndSize(bfTimes, 12);

            DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

            //Agency Control Table
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

            cb.SetFontAndSize(bfTimesBold, 10);
            cb.SetRGBColorFill(4, 4, 15);
            X_Pos = 300; Y_Pos = 785;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;

            if (dt.Rows.Count > 0)
            {
                DataTable dtHead = new DataTable();
                DataView dvHead = new DataView(dt);
                dvHead.RowFilter = "Sum_Catg_Code='4'";
                dtHead = dvHead.ToTable();

                string Indv = string.Empty, HH = string.Empty, CCIndv = string.Empty, CCHH = string.Empty;
                if (dtHead.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtHead.Rows[1]["Sum_Child_Period_Count"].ToString().Trim()))
                        Indv = dtHead.Rows[1]["Sum_Child_Period_Count"].ToString();
                    if (!string.IsNullOrEmpty(dtHead.Rows[2]["Sum_Child_Period_Count"].ToString().Trim()))
                        HH = dtHead.Rows[2]["Sum_Child_Period_Count"].ToString();

                    if (!string.IsNullOrEmpty(dtHead.Rows[1]["Sum_Child_Cum_Count"].ToString().Trim()))
                        CCIndv = dtHead.Rows[1]["Sum_Child_Cum_Count"].ToString();
                    if (!string.IsNullOrEmpty(dtHead.Rows[2]["Sum_Child_Cum_Count"].ToString().Trim()))
                        CCHH = dtHead.Rows[2]["Sum_Child_Cum_Count"].ToString();
                }

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of Local Agency Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.SetRGBColorFill(45, 45, 153);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);

                //////////cb.Rectangle(580, Y_Pos + 8, 540, Y_Pos - 2);
                //////////cb.ClosePath();
                //////////iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(540, Y_Pos + 8, 580, Y_Pos-2);
                //////////ColumnText ct = new ColumnText(cb);
                ////////////ct.SetSimpleColumn(540, Y_Pos + 8, 580, Y_Pos);
                ////////////ct.SetSimpleColumn(rect);
                //////////ct.AddElement(new Paragraph(Indv));
                //////////ct.Go();

                ////cb.SetLineWidth(0.5f);
                //////cb.SetLineCap(5);
                ////cb.SetRGBColorFill(160, 160, 160);
                ////cb.MoveTo(540, Y_Pos - 2);
                ////cb.LineTo(580, Y_Pos - 2);
                ////cb.LineTo(580, Y_Pos + 8);
                ////cb.LineTo(540, Y_Pos + 8);
                ////cb.ClosePathFillStroke();
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Indv, 550, Y_Pos + 5, 0);

                //Y_Pos -= 15;
                //cb.SetRGBColorFill(60, 15, 112);//cb.SetRGBColorFill(4, 4, 15);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", X_Pos, Y_Pos, 0);

                ////ct.SetSimpleColumn(580, Y_Pos + 8, 540, Y_Pos - 2);
                //////ct.SetSimpleColumn(rect);
                ////ct.AddElement(new Paragraph(HH));
                ////ct.Go();

                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.SetRGBColorFill(160, 160, 160);
                //cb.MoveTo(540, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos - 2);
                //cb.LineTo(580, Y_Pos + 8);
                //cb.LineTo(540, Y_Pos + 8);
                //cb.ClosePathFillStroke();
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, HH, 550, Y_Pos + 5, 0);

                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 70f;
                PdfPCell headcell = new PdfPCell(new Phrase(""));
                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                head.AddCell(headcell);

                PdfPTable THead = new PdfPTable(3);
                THead.TotalWidth = 520f;
                THead.LockedWidth = true;
                float[] twidths2 = new float[] { 85f, 11f,11f };
                THead.HorizontalAlignment = Element.ALIGN_LEFT;
                THead.SetWidths(twidths2);
                //THead.SpacingBefore = 75f;

                PdfPCell ST1 = new PdfPCell(new Phrase("", TblFontBold));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                ST1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(ST1);

                PdfPCell ST2 = new PdfPCell(new Phrase("Report", TblFontBold));
                ST2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ST2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(ST2);

                PdfPCell ST3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                ST3.HorizontalAlignment = Element.ALIGN_RIGHT;
                ST3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(ST3);


                iTextSharp.text.Font TblFntBold = new iTextSharp.text.Font(bfTimes, 9, 2);
                iTextSharp.text.Font hhdcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(45, 45, 153));
                PdfPCell T1 = new PdfPCell(new Phrase("A. Total unduplicated number of all INDIVIDUALS about whom one or more characteristics were obtained:", hhdcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T1);

                PdfPCell T2 = new PdfPCell(new Phrase(Indv.ToString(), TblFntBold));
                T2.HorizontalAlignment= Element.ALIGN_RIGHT;
                T2.BackgroundColor = BaseColor.LIGHT_GRAY; 
                T2.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T2);

                PdfPCell AT2 = new PdfPCell(new Phrase(CCIndv.ToString(), TblFntBold));
                AT2.HorizontalAlignment = Element.ALIGN_RIGHT;
                AT2.BackgroundColor = BaseColor.LIGHT_GRAY;
                AT2.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(AT2);

                PdfPCell T5 = new PdfPCell(new Phrase("", hhdcolor));
                T5.Colspan = 3;
                T5.FixedHeight = 10f;
                //T5.BackgroundColor = BaseColor.LIGHT_GRAY;
                T5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T5);


                iTextSharp.text.Font Indcolor = new iTextSharp.text.Font(bfTimes, 9, 2, new iTextSharp.text.BaseColor(60, 15, 112));
                PdfPCell T3 = new PdfPCell(new Phrase("B. Total unduplicated number of all HOUSEHOLDS about whom one or more characteristics were obtained:", Indcolor));
                //T1.FixedHeight = 12f;
                //T1.Colspan = 5;
                T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                THead.AddCell(T3);

                PdfPCell T4 = new PdfPCell(new Phrase(HH.ToString(), TblFntBold));
                T4.HorizontalAlignment = Element.ALIGN_RIGHT;
                T4.BackgroundColor = BaseColor.LIGHT_GRAY;
                T4.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(T4);

                PdfPCell AT4 = new PdfPCell(new Phrase(CCHH.ToString(), TblFntBold));
                AT4.HorizontalAlignment = Element.ALIGN_RIGHT;
                AT4.BackgroundColor = BaseColor.LIGHT_GRAY;
                AT4.Border = iTextSharp.text.Rectangle.BOX;
                THead.AddCell(AT4);

                PdfPTable outer = new PdfPTable(2);
                outer.TotalWidth = 550f;
                outer.LockedWidth = true;
                float[] widths2 = new float[] { 50f, 50f };
                outer.HorizontalAlignment = Element.ALIGN_LEFT;
                outer.SetWidths(widths2);
                outer.SpacingBefore = 10f;

                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 260f;
                table.LockedWidth = true;
                float[] widths = new float[] { 110f, 22f, 22f, 22f, 22f };
                table.SetWidths(widths);


                PdfPTable Rtable = new PdfPTable(5);
                Rtable.TotalWidth = 260f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 110f, 22f, 22f, 22f, 22f };
                Rtable.SetWidths(widths1);

                PdfPCell Header = new PdfPCell(new Phrase("C. INDIVIDUAL LEVEL CHARACTERISTICS", fc1));
                Header.Colspan = 5;
                Header.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.FixedHeight = 15f;
                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(Header);

                PdfPCell RHeader = new PdfPCell(new Phrase(" ", fc1));
                RHeader.Colspan = 5;
                RHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                RHeader.FixedHeight = 10f;
                RHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Rtable.AddCell(RHeader);

                bool First = true; int Count = 1; bool RFirst = true; bool EduCol = true; bool Rtab = true;
                string PrivType = string.Empty; bool Disable = true; bool Health = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        PdfPCell R0 = new PdfPCell(new Phrase("", TblFontBold));
                        R0.FixedHeight = 12f;
                        R0.Colspan = 5;
                        R0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(R0);

                        PdfPCell R03 = new PdfPCell(new Phrase("", TblFontBold));
                        R03.FixedHeight = 12f;
                        R03.Colspan = 5;
                        R03.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Rtable.AddCell(R03);
                        break;
                    }

                    if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                    {
                        if (dr["Sum_Catg_Code"].ToString() == "E")
                        {
                            PdfPCell R3 = new PdfPCell(new Phrase(Count + ". " + "Ethnicity/Race", TblFontBold));
                            R3.FixedHeight = 12f;
                            R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R3);

                            PdfPCell R4 = new PdfPCell(new Phrase("Report", TblFontBold));
                            R4.FixedHeight = 12f;
                            R4.Colspan = 2;
                            R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R4.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R4);

                            PdfPCell R5 = new PdfPCell(new Phrase("Reference", TblFontBold));
                            R5.FixedHeight = 12f;
                            R5.Colspan = 2;
                            R5.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R5.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R5);

                            PdfPCell R1 = new PdfPCell(new Phrase("I.Ethnicity", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 5;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            Rtable.AddCell(R1);
                        }
                        else if (dr["Sum_Catg_Code"].ToString() == "R")
                        {
                            PdfPCell SR1 = new PdfPCell(new Phrase("", TblFontBold));
                            SR1.FixedHeight = 12f;
                            SR1.Colspan = 5;
                            SR1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(SR1);

                            PdfPCell R1 = new PdfPCell(new Phrase("II.Race", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Colspan = 5;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Rtable.AddCell(R1);

                            Count++;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "D")
                        {
                            PdfPCell R1 = new PdfPCell(new Phrase(Count + ". Health", TblFontBold));
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R1);

                            PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                            R2.FixedHeight = 12f;
                            R2.Colspan = 4;
                            R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                            table.AddCell(R2);

                            Disable = true;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (Count > 6)
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "W" || dr["Sum_Catg_Code"].ToString().Trim() == "V")
                                {
                                    PdfPTable NestedTable = new PdfPTable(2);
                                    NestedTable.TotalWidth = 150f;
                                    NestedTable.LockedWidth = true;
                                    float[] N2Dwidths = new float[] { 50f,60f};
                                    NestedTable.SetWidths(N2Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    D1.FixedHeight = 12f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    D1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(D1);

                                    PdfPCell sD1 = new PdfPCell(new Phrase("(Individuals 18+)", fcRed));
                                    sD1.FixedHeight = 12f;
                                    sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    NestedTable.AddCell(sD1);

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.Padding = 0f;
                                    R11.Colspan = 1;
                                    R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R11);

                                    //PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    //R1.FixedHeight = 12f;
                                    //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    //Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                    R3.FixedHeight = 12f;
                                    R3.Colspan = 2;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R3);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                    R3.FixedHeight = 12f;
                                    R3.Colspan = 2;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    Rtable.AddCell(R3);
                                }
                            }
                            else
                            {
                                if (dr["Sum_Catg_Code"].ToString().Trim() == "S")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase("  " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    //R1.Colspan = 5;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                    R3.FixedHeight = 12f;
                                    R3.Colspan = 2;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R3);

                                    //PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    //R2.FixedHeight = 12f;
                                    //R2.Colspan = 2;
                                    //R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    //table.AddCell(R2);
                                }
                                else if (dr["Sum_Catg_Code"].ToString().Trim() == "Y")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    iTextSharp.text.Font fcRed1 = new iTextSharp.text.Font(bfTimes, 6, 1);

                                    //PdfPTable NestedTable = new PdfPTable(2);
                                    //NestedTable.TotalWidth = 150f;
                                    //NestedTable.LockedWidth = true;
                                    //float[] Dwidths = new float[] { 35f, 35f };
                                    //NestedTable.SetWidths(Dwidths);

                                    PdfPCell D1 = new PdfPCell(new Phrase("Report", fcRed1));
                                    D1.FixedHeight = 12f;
                                    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    D1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    D1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(D1);

                                    PdfPCell sD1 = new PdfPCell(new Phrase("Reference", fcRed1));
                                    sD1.FixedHeight = 12f;
                                    sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    sD1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(sD1);

                                    //PdfPCell R11 = new PdfPCell(NestedTable);
                                    //R11.Padding = 0f;
                                    //R11.Colspan = 3;
                                    //R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R11.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    //table.AddCell(R11);

                                }
                                else if (dr["Sum_Catg_Code"].ToString().Trim() == "U")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 4;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R2);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                    R1.FixedHeight = 12f;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R2.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                    R3.FixedHeight = 12f;
                                    R3.Colspan = 2;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    R3.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                                    table.AddCell(R3);
                                }
                            }

                            Count++;
                        }
                    }
                    else
                    {
                        if (dr["Sum_Catg_Code"].ToString().Trim() == "D" && Disable)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='D' AND Sum_Child_Desc<>'Disability' ";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(6);
                                NestedTable.TotalWidth = 260f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 60f,30f, 25f, 25f, 30f,30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(6);
                                NestedTable1.TotalWidth = 260f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 60f, 30f, 25f, 25f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPTable NestedTable2 = new PdfPTable(6);
                                NestedTable2.TotalWidth = 260f;
                                NestedTable2.LockedWidth = true;
                                float[] N2Dwidths = new float[] { 60f, 30f, 25f, 25f, 30f, 30f };
                                NestedTable2.SetWidths(N2Dwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                PdfPCell sD1 = new PdfPCell(new Phrase("", Times));
                                sD1.FixedHeight = 12f;
                                sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(sD1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 5;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("a. Disabling Condition", Times));
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);

                                PdfPCell SD3 = new PdfPCell(new Phrase("Report", Times));
                                SD3.FixedHeight = 12f;
                                SD3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //SD3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                NestedTable1.AddCell(SD3);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 5;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);

                                PdfPCell sD4 = new PdfPCell(new Phrase("", Times));
                                sD4.FixedHeight = 12f;
                                sD4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable2.AddCell(sD4);

                                PdfPCell D4 = new PdfPCell(new Phrase("Reference", Times));
                                D4.FixedHeight = 12f;
                                D4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //D4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                NestedTable2.AddCell(D4);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable2.AddCell(R2);
                                }

                                PdfPCell R23 = new PdfPCell(NestedTable2);
                                R23.Padding = 0f;
                                R23.Colspan = 5;
                                R23.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R23);

                            }
                            Disable = false;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "N" && Health)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "Sum_Catg_Code='N' AND Sum_Child_Desc<>'Health Insurance'";  //AND Sum_Child_Code<>'STATICTOTL'
                            DataTable dtDis = dv.ToTable();

                            if (dtDis.Rows.Count > 0)
                            {
                                PdfPTable NestedTable = new PdfPTable(6);
                                NestedTable.TotalWidth = 260f;
                                NestedTable.LockedWidth = true;
                                float[] Dwidths = new float[] { 60f, 30f, 25f, 25f, 30f, 30f };
                                NestedTable.SetWidths(Dwidths);

                                PdfPTable NestedTable1 = new PdfPTable(6);
                                NestedTable1.TotalWidth = 260f;
                                NestedTable1.LockedWidth = true;
                                float[] NDwidths = new float[] { 60f, 30f, 25f, 25f, 30f, 30f };
                                NestedTable1.SetWidths(NDwidths);

                                PdfPTable NestedTable2 = new PdfPTable(6);
                                NestedTable2.TotalWidth = 260f;
                                NestedTable2.LockedWidth = true;
                                float[] N2Dwidths = new float[] { 60f, 30f, 25f, 25f, 30f, 30f };
                                NestedTable2.SetWidths(N2Dwidths);

                                PdfPCell D1 = new PdfPCell(new Phrase("", Times));
                                D1.FixedHeight = 12f;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(D1);

                                PdfPCell sD1 = new PdfPCell(new Phrase("", Times));
                                sD1.FixedHeight = 12f;
                                sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable.AddCell(sD1);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell D2 = new PdfPCell(new Phrase(drDis["Sum_Child_Desc"].ToString().Trim(), Times));
                                    D2.FixedHeight = 12f;
                                    D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    NestedTable.AddCell(D2);
                                }

                                PdfPCell R11 = new PdfPCell(NestedTable);
                                R11.Padding = 0f;
                                R11.Colspan = 5;
                                R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R11);

                                //if (NestedTable.Rows.Count > 0)
                                //    NestedTable.DeleteBodyRows();

                                PdfPCell D3 = new PdfPCell(new Phrase("b. Health Insurance", Times));//               Rep Period
                                D3.FixedHeight = 12f;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable1.AddCell(D3);

                                PdfPCell SD3 = new PdfPCell(new Phrase("Report", Times));//               Rep Period
                                SD3.FixedHeight = 12f;
                                SD3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //SD3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                NestedTable1.AddCell(SD3);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable1.AddCell(R2);
                                }

                                PdfPCell R12 = new PdfPCell(NestedTable1);
                                R12.Padding = 0f;
                                R12.Colspan = 5;
                                R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R12);

                                PdfPCell sD4 = new PdfPCell(new Phrase("", Times));
                                sD4.FixedHeight = 12f;
                                sD4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                NestedTable2.AddCell(sD4);

                                PdfPCell D4 = new PdfPCell(new Phrase("Reference", Times));
                                D4.FixedHeight = 12f;
                                D4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //D4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                NestedTable2.AddCell(D4);

                                foreach (DataRow drDis in dtDis.Rows)
                                {
                                    PdfPCell R2 = new PdfPCell(new Phrase(drDis["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    NestedTable2.AddCell(R2);
                                }

                                PdfPCell R23 = new PdfPCell(NestedTable2);
                                R23.Padding = 0f;
                                R23.Colspan = 5;
                                R23.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(R23);

                            }

                            PdfPCell R13 = new PdfPCell(new Phrase("*If an individual reported that they had Health Insurance please identify the source of health insurance below.", TblFontSmall));
                            R13.FixedHeight = 8f;
                            R13.Colspan = 5;
                            R13.HorizontalAlignment = Element.ALIGN_LEFT;
                            R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(R13);

                            Health = false;
                        }
                        else if (dr["Sum_Catg_Code"].ToString().Trim() != "D" && dr["Sum_Catg_Code"].ToString().Trim() != "N")
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count >= 6 && dr["Sum_Catg_Code"].ToString() != "S")
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    //R01.Colspan = 3;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Rtable.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R02.FixedHeight = 12f;
                                    R02.Colspan = 2;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Rtable.AddCell(R02);

                                    PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R03.FixedHeight = 12f;
                                    R03.Colspan = 2;
                                    R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R03.Border = iTextSharp.text.Rectangle.BOX;
                                    R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Rtable.AddCell(R03);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        //R01.Colspan = 2;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPTable NestedTable = new PdfPTable(5);
                                        NestedTable.TotalWidth = 125f;
                                        NestedTable.LockedWidth = true;
                                        float[] Dwidths = new float[] { 21f, 24f, 3f, 21f, 24f };
                                        NestedTable.SetWidths(Dwidths);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Count"].ToString().Trim(), Times));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        NestedTable.AddCell(R02);

                                        PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Cum_Count"].ToString().Trim(), Times));
                                        R03.FixedHeight = 12f;
                                        R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R03.Border = iTextSharp.text.Rectangle.BOX;
                                        R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        NestedTable.AddCell(R03);

                                        PdfPCell R06 = new PdfPCell(new Phrase("", Times));
                                        R06.FixedHeight = 12f;
                                        R06.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R06.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        //R06.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        NestedTable.AddCell(R06);

                                        PdfPCell R04 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R04.FixedHeight = 12f;
                                        R04.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R04.Border = iTextSharp.text.Rectangle.BOX;
                                        R04.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        NestedTable.AddCell(R04);

                                        PdfPCell R05 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R05.FixedHeight = 12f;
                                        R05.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R05.Border = iTextSharp.text.Rectangle.BOX;
                                        R05.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        NestedTable.AddCell(R05);

                                        PdfPCell R12 = new PdfPCell(NestedTable);
                                        R12.Padding = 0f;
                                        R12.Colspan = 4;
                                        R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R12);
                                    }
                                    //else if (dr["Sum_Catg_Code"].ToString().Trim() == "Y")
                                    //{
                                    //    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    //    R01.FixedHeight = 12f;
                                    //    R01.Colspan = 2;
                                    //    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(R01);

                                    //    PdfPTable NestedTable = new PdfPTable(2);
                                    //    NestedTable.TotalWidth = 150f;
                                    //    NestedTable.LockedWidth = true;
                                    //    float[] Dwidths = new float[] { 33f, 33f };
                                    //    NestedTable.SetWidths(Dwidths);

                                    //    PdfPCell D1 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    //    D1.FixedHeight = 12f;
                                    //    D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    D1.HorizontalAlignment=Element.ALIGN_RIGHT;
                                    //    NestedTable.AddCell(D1);

                                    //    PdfPCell sD1 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    //    sD1.FixedHeight = 12f;
                                    //    sD1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    sD1.HorizontalAlignment=Element.ALIGN_RIGHT;
                                    //    NestedTable.AddCell(sD1);

                                    //    PdfPCell R11 = new PdfPCell(NestedTable);
                                    //    R11.Padding = 0f;
                                    //    R11.Colspan = 3;
                                    //    //R11.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //    R11.Border = iTextSharp.text.Rectangle.BOX;
                                    //    R11.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //    table.AddCell(R11);
                                    //}
                                    else
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        //R01.Colspan = 3;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R02.FixedHeight = 12f;
                                        R02.Colspan = 2;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R02);

                                        PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R03.FixedHeight = 12f;
                                        R03.Colspan = 2;
                                        R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R03.Border = iTextSharp.text.Rectangle.BOX;
                                        R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(R03);
                                    }

                                    //if (dr["Sum_Catg_Code"].ToString().Trim() == "S") Count++;
                                }
                            }
                            else
                            {
                                if (Count >= 6 && dr["Sum_Catg_Code"].ToString() != "S")
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    //R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.Colspan = 2;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R3.FixedHeight = 12f;
                                    R3.Colspan = 2;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R3);
                                }
                                else
                                {
                                    if (dr["Sum_Catg_Code"].ToString() == "S")
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase("  " + dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        //R1.Colspan = 3;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.Colspan = 2;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);

                                        PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R3.FixedHeight = 12f;
                                        R3.Colspan = 2;
                                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R3.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(R3);
                                    }
                                    else if (dr["Sum_Catg_Code"].ToString() == "U")
                                    {
                                        if (EduCol)
                                        {
                                            PdfPTable NestedTableHead = new PdfPTable(5);
                                            NestedTableHead.TotalWidth = 125f;
                                            NestedTableHead.LockedWidth = true;
                                            float[] sDwidths = new float[] { 21f, 24f, 3f, 21f, 24f };
                                            NestedTableHead.SetWidths(sDwidths);

                                            iTextSharp.text.Font fcRed1 = new iTextSharp.text.Font(bfTimes, 5, 1);

                                            PdfPCell SR1 = new PdfPCell(new Phrase("", Times));
                                            SR1.FixedHeight = 12f;
                                            SR1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(SR1);

                                            PdfPCell sR2 = new PdfPCell(new Phrase("Report", fcRed1));
                                            sR2.FixedHeight = 12f;
                                            sR2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR2.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead.AddCell(sR2);



                                            PdfPCell sR3 = new PdfPCell(new Phrase("Reference", fcRed1));
                                            sR3.FixedHeight = 12f;
                                            sR3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR3.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead.AddCell(sR3);

                                            PdfPCell sR8 = new PdfPCell(new Phrase("", Times));
                                            sR8.FixedHeight = 12f;
                                            sR8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR8.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                            NestedTableHead.AddCell(sR8);

                                            PdfPCell sR4 = new PdfPCell(new Phrase("Report", fcRed1));
                                            sR4.FixedHeight = 12f;
                                            sR4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR4.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead.AddCell(sR4);

                                            PdfPCell sR5 = new PdfPCell(new Phrase("Reference", fcRed1));
                                            sR5.FixedHeight = 12f;
                                            sR5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR5.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead.AddCell(sR5);

                                            PdfPCell sR22 = new PdfPCell(NestedTableHead);
                                            sR22.Padding = 0f;
                                            sR22.Colspan = 4;
                                            sR22.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(sR22);


                                            PdfPTable NestedTableHead1 = new PdfPTable(5);
                                            NestedTableHead1.TotalWidth = 125f;
                                            NestedTableHead1.LockedWidth = true;
                                            float[] s1Dwidths = new float[] { 21f, 24f, 3f, 21f, 24f };
                                            NestedTableHead1.SetWidths(s1Dwidths);

                                            PdfPCell SR11 = new PdfPCell(new Phrase("", Times));
                                            SR11.FixedHeight = 12f;
                                            SR11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(SR11);

                                            PdfPCell sR21 = new PdfPCell(new Phrase("[ages 14-24]", fcRed));
                                            sR21.FixedHeight = 12f;
                                            sR21.Colspan = 2;
                                            sR21.HorizontalAlignment = Element.ALIGN_CENTER;
                                            sR21.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead1.AddCell(sR21);

                                            //PdfPCell sR31 = new PdfPCell(new Phrase("Reference", fcRed));
                                            //sR31.FixedHeight = 12f;
                                            //sR31.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //sR31.Border = iTextSharp.text.Rectangle.BOX;
                                            //NestedTableHead1.AddCell(sR31);

                                            PdfPCell sR81 = new PdfPCell(new Phrase("", Times));
                                            sR81.FixedHeight = 12f;
                                            sR81.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sR81.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                            NestedTableHead1.AddCell(sR81);

                                            PdfPCell sR41 = new PdfPCell(new Phrase("[ages 25+]", fcRed));
                                            sR41.FixedHeight = 12f;
                                            sR41.Colspan = 2;
                                            sR41.HorizontalAlignment = Element.ALIGN_CENTER;
                                            sR41.Border = iTextSharp.text.Rectangle.BOX;
                                            NestedTableHead1.AddCell(sR41);

                                            //PdfPCell sR51 = new PdfPCell(new Phrase("Reference", fcRed));
                                            //sR51.FixedHeight = 12f;
                                            //sR51.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //sR51.Border = iTextSharp.text.Rectangle.BOX;
                                            //NestedTableHead1.AddCell(sR51);

                                            PdfPCell sR221 = new PdfPCell(NestedTableHead1);
                                            sR221.Padding = 0f;
                                            sR221.Colspan = 4;
                                            sR221.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(sR221);

                                            //PdfPCell R11 = new PdfPCell(new Phrase("", Times));
                                            //R11.FixedHeight = 12f;
                                            //R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //table.AddCell(R11);

                                            //PdfPCell R12 = new PdfPCell(new Phrase("[ages 14-24] ", fcRed));
                                            //R12.FixedHeight = 12f;
                                            //R12.Colspan = 2;
                                            //R12.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //table.AddCell(R12);

                                            //PdfPCell R13 = new PdfPCell(new Phrase("[ages 25+] ", fcRed));
                                            //R13.FixedHeight = 12f;
                                            //R13.Colspan = 2;
                                            //R13.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            //table.AddCell(R13);
                                            EduCol = false;
                                        }

                                        PdfPTable NestedTable = new PdfPTable(5);
                                        NestedTable.TotalWidth = 125f;
                                        NestedTable.LockedWidth = true;
                                        float[] Dwidths = new float[] { 21f, 24f, 3f, 21f, 24f };
                                        NestedTable.SetWidths(Dwidths);

                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Count"].ToString(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R2);

                                        PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Edu_Cum_Count"].ToString().Trim(), Times));
                                        R3.FixedHeight = 12f;
                                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R3.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R3);

                                        PdfPCell R8 = new PdfPCell(new Phrase("", Times));
                                        R8.FixedHeight = 12f;
                                        R8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R8.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                        NestedTable.AddCell(R8);

                                        PdfPCell R4 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString(), Times));
                                        R4.FixedHeight = 12f;
                                        R4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R4.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R4);

                                        PdfPCell R5 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R5.FixedHeight = 12f;
                                        R5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R5.Border = iTextSharp.text.Rectangle.BOX;
                                        NestedTable.AddCell(R5);

                                        PdfPCell R22 = new PdfPCell(NestedTable);
                                        R22.Padding = 0f;
                                        R22.Colspan = 4;
                                        R22.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R22);
                                    }
                                        else if (dr["Sum_Catg_Code"].ToString().Trim() == "Y")
                                    {
                                        PdfPCell R01 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R01.FixedHeight = 12f;
                                        R01.Colspan = 3;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(R01);

                                        //iTextSharp.text.Font fcRed1 = new iTextSharp.text.Font(1, 6, 1);

                                        //PdfPTable NestedTable = new PdfPTable(2);
                                        //NestedTable.TotalWidth = 150f;
                                        //NestedTable.LockedWidth = true;
                                        //float[] Dwidths = new float[] { 35f, 35f };
                                        //NestedTable.SetWidths(Dwidths);

                                        PdfPCell D1 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        D1.FixedHeight = 12f;
                                        D1.Border = iTextSharp.text.Rectangle.BOX;
                                        D1.HorizontalAlignment=Element.ALIGN_RIGHT;
                                        table.AddCell(D1);

                                        PdfPCell sD1 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        sD1.FixedHeight = 12f;
                                        sD1.Border = iTextSharp.text.Rectangle.BOX;
                                        sD1.HorizontalAlignment=Element.ALIGN_RIGHT;
                                        table.AddCell(sD1);

                                        //PdfPCell R11 = new PdfPCell(NestedTable);
                                        //R11.Padding = 0f;
                                        //R11.Colspan = 3;
                                        ////R11.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        ////R11.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        //table.AddCell(R11);
                                    }
                                    else
                                    {
                                        PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                        R1.FixedHeight = 12f;
                                        //R1.Colspan = 3;
                                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R1);

                                        PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R2.FixedHeight = 12f;
                                        R2.Colspan = 2;
                                        R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R2.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R2);

                                        PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R3.FixedHeight = 12f;
                                        R3.Colspan = 2;
                                        R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R3.Border = iTextSharp.text.Rectangle.BOX;
                                        //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                        table.AddCell(R3);
                                    }
                                }
                            }
                        }
                    }
                }

                PdfPCell O1 = new PdfPCell(table);
                O1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O1);

                PdfPCell O2 = new PdfPCell(Rtable);
                O2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                outer.AddCell(O2);

                document.Add(head);
                document.Add(THead);
                document.Add(outer);

                document.NewPage();
                SecondPage_Both_Counts(dt, document);

                ////ADDED BY SUDHEER ON 11/16/23
                if (chkbMontCounty.Checked && dtCounty.Rows.Count > 0)
                {
                    document.NewPage();
                    CountyWiseReport(dtCounty, document);
                }

                if (chkbLangCounts.Checked && dtLanguage.Rows.Count > 0)
                {
                    document.NewPage();
                    LanguageCountsReport(dtLanguage, document);
                }
            }

            document.Close();
            fs.Close();
            fs.Dispose();

            //if(chkbMontCounty.Checked && dtCounty.Rows.Count>0)
            //{
            //    Generate("RNGB0004_CountyWise_Report" );
            //    AlertBox.Show("RNGB0004_CountyWise_Report.xls file Generated", MessageBoxIcon.Warning);
            //}

            //if (LookupDataAccess.FriendlyName().Contains("2012"))
            //{

            string FamTypeSw=string.Empty;
            FamTypeSw = BaseForm.BaseAgencyControlDetails.FTypeSwitch;

                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, ds, dt, Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGB0004", Bypass_Rep_Name, MST_Rep_Name, Ind_Rep_Name, BaseForm);
            //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();
            //}
            //else
            //{
            //    FrmViewer objfrm = new FrmViewer(PdfName);
            //    //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //    objfrm.ShowDialog();
            //}

        }

        private void SecondPage_Both_Counts(DataTable dtCounts, Document document)
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
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Module 4, Section C: All Characteristics Report - Data Entry Form", X_Pos, Y_Pos, 0);
            //Y_Pos -= 15;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Goal 1: Individuals and Families with low-incomes are stable and achieve economic security.", X_Pos, Y_Pos, 0);
            //Y_Pos -= 20;

            //Agency Control Table
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

            if (dtCounts.Rows.Count > 0)
            {
                //DataTable dt = dscategories.Tables[0];
                //DataView dv = new DataView(dt);
                //dv.Sort = "RNG4CATG_SEQ";
                //dt = dv.ToTable();

                cb.SetFontAndSize(bfTimesBold, 10);
                cb.SetRGBColorFill(4, 4, 15);
                //cb.SetColorFill(BaseColor.MAGENTA.Darker());
                X_Pos = 30; Y_Pos = 780;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting: ", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 10);
                cb.LineTo(580, Y_Pos - 10);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 20;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 130, Y_Pos, 0);//215
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                Y_Pos -= 15;
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "State:", X_Pos, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(29, Y_Pos - 5);
                cb.LineTo(350, Y_Pos - 5);
                cb.LineTo(350, Y_Pos + 10);
                cb.LineTo(29, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "DUNS:", 351, Y_Pos, 0);
                cb.EndText();
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(350, Y_Pos - 5);
                cb.LineTo(580, Y_Pos - 5);
                cb.LineTo(580, Y_Pos + 10);
                cb.LineTo(350, Y_Pos + 10);
                cb.ClosePathStroke();
                cb.BeginText();

                //X_Pos = 30; //Y_Pos = 780;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name of CSBG Eligible Entity Reporting:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Agency, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                //Y_Pos -= 20;
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Department/Program:", X_Pos, Y_Pos, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DeptName + ProgName, 215, Y_Pos, 0);
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(210, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos - 5);
                //cb.LineTo(580, Y_Pos + 10);
                //cb.LineTo(210, Y_Pos + 10);
                //cb.ClosePathStroke();

                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 75f;
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
                //outer.SpacingBefore = 75f;

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 240f;
                table.LockedWidth = true;
                float[] widths = new float[] { 120f, 35f,35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table.SetWidths(widths);
                //table.SpacingBefore = 270f;

                PdfPTable Rtable = new PdfPTable(3);
                Rtable.TotalWidth = 240f;
                Rtable.LockedWidth = true;
                float[] widths1 = new float[] { 120f, 35f,35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
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
                foreach (DataRow dr in dtCounts.Rows)
                {
                    if (dr["Sum_Col_Name"].ToString().Contains("MST"))
                    {
                        if (dr["Sum_Child_Code"].ToString().Trim() == "STATICHEAD")
                        {
                            if (Count >= 13)
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R2);

                                PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                R3.FixedHeight = 12f;
                                R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                Rtable.AddCell(R3);
                            }
                            else
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(Count + ". " + dr["Sum_Child_Desc"].ToString().Trim(), TblFontBold));
                                R1.FixedHeight = 12f;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R1);

                                PdfPCell R2 = new PdfPCell(new Phrase("Report", TblFontBold));
                                R2.FixedHeight = 12f;
                                R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R2.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R2);

                                PdfPCell R3 = new PdfPCell(new Phrase("Reference", TblFontBold));
                                R3.FixedHeight = 12f;
                                R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R3.BackgroundColor = new iTextSharp.text.BaseColor(204, 169, 217);
                                table.AddCell(R3);
                            }

                            Count++;
                        }
                        else
                        {
                            if (dr["Sum_Child_Code"].ToString().Trim() == "STATICTOTL")
                            {
                                if (Count > 13)
                                {
                                    if (dr["Sum_Catg_Code"].ToString() != "J" && dr["Sum_Catg_Code"].ToString() != "K") //Brains asks to put logic to off the display of totals for 14&15 on 08/19/2017
                                    {

                                        PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                        R01.FixedHeight = 12f;
                                        R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Rtable.AddCell(R01);

                                        PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                        R02.FixedHeight = 12f;
                                        R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R02.Border = iTextSharp.text.Rectangle.BOX;
                                        R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        Rtable.AddCell(R02);

                                        PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                        R03.FixedHeight = 12f;
                                        R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        R03.Border = iTextSharp.text.Rectangle.BOX;
                                        R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        Rtable.AddCell(R03);
                                    }

                                    if (dr["Sum_Catg_Code"].ToString() == "I")
                                    {
                                        PdfPCell P0 = new PdfPCell(new Phrase("Below, please report the types of Other income and/or non-cash benefits received by the households who reported sources other than employment", TblFontSmall));
                                        P0.Colspan = 3;
                                        P0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Rtable.AddCell(P0);
                                    }
                                }
                                else
                                {
                                    PdfPCell R01 = new PdfPCell(new Phrase("  TOTAL (auto calculated)", TblFontBold));
                                    R01.FixedHeight = 12f;
                                    R01.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(R01);

                                    PdfPCell R02 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R02.FixedHeight = 12f;
                                    R02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R02.Border = iTextSharp.text.Rectangle.BOX;
                                    R02.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R02);

                                    PdfPCell R03 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R03.FixedHeight = 12f;
                                    R03.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R03.Border = iTextSharp.text.Rectangle.BOX;
                                    R03.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(R03);

                                }
                            }
                            else
                            {
                                if (Count > 13)
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    //R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    Rtable.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R3.FixedHeight = 12f;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.BOX;
                                    Rtable.AddCell(R3);
                                }
                                else
                                {
                                    PdfPCell R1 = new PdfPCell(new Phrase(dr["Sum_Child_Desc"].ToString().Trim(), Times));
                                    R1.FixedHeight = 12f;
                                    //R1.Colspan = 3;
                                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //R1.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R1);

                                    PdfPCell R2 = new PdfPCell(new Phrase(dr["Sum_Child_Period_Count"].ToString().Trim(), Times));
                                    R2.FixedHeight = 12f;
                                    R2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                    //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R2);

                                    PdfPCell R3 = new PdfPCell(new Phrase(dr["Sum_Child_Cum_Count"].ToString().Trim(), Times));
                                    R3.FixedHeight = 12f;
                                    R3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    R3.Border = iTextSharp.text.Rectangle.BOX;
                                    //R2.BackgroundColor = BaseColor.BLUE.Brighter();
                                    table.AddCell(R3);
                                }
                            }
                        }
                    }
                }
                PdfPCell L0 = new PdfPCell(new Phrase("", TblFontBold));
                L0.FixedHeight = 12f;
                L0.Colspan = 3;
                L0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(L0);

                PdfPCell L03 = new PdfPCell(new Phrase("", TblFontBold));
                L03.FixedHeight = 12f;
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

                PdfPCell S0 = new PdfPCell(new Phrase("E.  Number of Individuals Who May or May Not be Included in the Totals Above (due to data collection system integration barriers)", fc2));
                S0.FixedHeight = 12f;
                S0.Colspan = 3;
                S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S0);

                PdfPCell S1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of INDIVIDUALS served in each program*:", Times));
                S1.FixedHeight = 12f;
                //S1.Colspan = 3;
                S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(S1);

                PdfPCell S2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                S2.FixedHeight = 12f;
                S2.HorizontalAlignment = Element.ALIGN_CENTER;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                S2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S2);

                PdfPCell S3 = new PdfPCell(new Phrase("Number of Individuals", TblFontBold));
                S3.FixedHeight = 12f;
                S3.HorizontalAlignment = Element.ALIGN_CENTER;
                S3.Border = iTextSharp.text.Rectangle.BOX;
                S3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(S3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M0 = new PdfPCell(new Phrase("F. Number of Households Who May or May Not be Included in the Totals Above (due to data collection system integration barriers)", fc1));
                M0.FixedHeight = 12f;
                M0.Colspan = 3;
                M0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M0);

                PdfPCell M1 = new PdfPCell(new Phrase("  1. Please list the unduplicated number of HOUSEHOLDS served in each program*:", Times));
                M1.FixedHeight = 12f;
                //S1.Colspan = 3;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Last.AddCell(M1);

                PdfPCell M2 = new PdfPCell(new Phrase("Program Name", TblFontBold));
                M2.FixedHeight = 12f;
                M2.HorizontalAlignment = Element.ALIGN_CENTER;
                M2.Border = iTextSharp.text.Rectangle.BOX;
                M2.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M2);

                PdfPCell M3 = new PdfPCell(new Phrase("Number of Households", TblFontBold));
                M3.FixedHeight = 12f;
                M3.HorizontalAlignment = Element.ALIGN_CENTER;
                M3.Border = iTextSharp.text.Rectangle.BOX;
                M3.BackgroundColor = BaseColor.LIGHT_GRAY;
                Last.AddCell(M3);

                for (int i = 0; i < 2; i++)
                {
                    PdfPCell T1 = new PdfPCell(new Phrase("", Times));
                    T1.FixedHeight = 12f;
                    //S1.Colspan = 3;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Last.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(new Phrase("", TblFontBold));
                    T2.FixedHeight = 12f;
                    T2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T2.Border = iTextSharp.text.Rectangle.BOX;
                    T2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(new Phrase("", TblFontBold));
                    T3.FixedHeight = 12f;
                    T3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    T3.Border = iTextSharp.text.Rectangle.BOX;
                    T3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Last.AddCell(T3);
                }

                PdfPCell M5 = new PdfPCell(new Phrase("  *The system will add rows to allow reporting on multiple programs.", TblFontSmall));
                M5.FixedHeight = 12f;
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

        private void CountyWiseReport(DataTable dtCounty,Document document)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(60, 15, 112));
            iTextSharp.text.Font fc2 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 10, 1);
            iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            PdfPTable Table;
            if (rbRepPeriod.Checked)
            {
                Table = new PdfPTable(3);
                Table.TotalWidth = 500f;
                Table.LockedWidth = true;
                float[] widths2 = new float[] { 30f, 25f, 25f};// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Table.SetWidths(widths2);
                Table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell Header = new PdfPCell(new Phrase("County Counts", TblFontBold));
                Header.Colspan = 3;
                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.FixedHeight = 17f;
                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table.AddCell(Header);
            }
            else
            {
                Table = new PdfPTable(7);
                Table.TotalWidth = 500f;
                Table.LockedWidth = true;
                float[] widths2 = new float[] { 30f, 25f, 25f, 50f, 30f, 25f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                Table.SetWidths(widths2);
                Table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell Header = new PdfPCell(new Phrase("County Counts", TblFontBold));
                Header.Colspan = 7;
                Header.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.FixedHeight = 17f;//15f;
                Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table.AddCell(Header);
            }
            //Table.SpacingBefore = 75f;

            

            PdfPCell R1 = new PdfPCell(new Phrase("County Counts - Report Period", TblFontBold1));
            R1.Colspan = 3;
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.FixedHeight = 12f;
            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
            Table.AddCell(R1);

            if(rbBoth.Checked)
            {
                R1 = new PdfPCell(new Phrase("", TableFont));
                //R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table.AddCell(R1);

                R1 = new PdfPCell(new Phrase("County Counts - Refrence Period", TblFontBold1));
                R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                Table.AddCell(R1);
            }

            R1 = new PdfPCell(new Phrase("County", TblFontBold1));
            //R1.Colspan = 3;
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.FixedHeight = 12f;
            R1.Border = iTextSharp.text.Rectangle.BOX;
            Table.AddCell(R1);

            R1 = new PdfPCell(new Phrase("# of Individuals", TblFontBold1));
            //R1.Colspan = 3;
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.FixedHeight = 12f;
            R1.Border = iTextSharp.text.Rectangle.BOX;
            Table.AddCell(R1);

            R1 = new PdfPCell(new Phrase("# of Households", TblFontBold1));
            //R1.Colspan = 3;
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.FixedHeight = 12f;
            R1.Border = iTextSharp.text.Rectangle.BOX;
            Table.AddCell(R1);

            if (rbBoth.Checked)
            {
                R1 = new PdfPCell(new Phrase("", TableFont));
                //R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table.AddCell(R1);

                R1 = new PdfPCell(new Phrase("County", TblFontBold1));
                //R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                Table.AddCell(R1);

                R1 = new PdfPCell(new Phrase("# of Individuals", TblFontBold1));
                //R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                Table.AddCell(R1);

                R1 = new PdfPCell(new Phrase("# of Households", TblFontBold1));
                //R1.Colspan = 3;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                Table.AddCell(R1);


            }

            foreach(DataRow dr in dtCounty.Rows)
            {
                if (dr["Ind_County"].ToString() == "Total")
                {
                    R1 = new PdfPCell(new Phrase(dr["Ind_County"].ToString(), TblFontBold1));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase(dr["REPINDV"].ToString(), TblFontBold1));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase(dr["RepAppCNT"].ToString(), TblFontBold1));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);
                }
                else
                {
                    R1 = new PdfPCell(new Phrase(dr["Ind_County"].ToString(), TableFont));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase(dr["REPINDV"].ToString(), TableFont));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase(dr["RepAppCNT"].ToString(), TableFont));
                    //R1.Colspan = 3;
                    R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);
                }

                if (rbBoth.Checked)
                {
                    if (dr["Ind_County"].ToString() == "Total")
                    {
                        R1 = new PdfPCell(new Phrase("", TblFontBold1));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["Ind_County"].ToString(), TblFontBold1));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RefIndvCNT"].ToString(), TblFontBold1));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RefAppCNT"].ToString(), TblFontBold1));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);
                    }
                    else
                    {
                        R1 = new PdfPCell(new Phrase("", TableFont));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["Ind_County"].ToString(), TableFont));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RefIndvCNT"].ToString(), TableFont));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RefAppCNT"].ToString(), TableFont));
                        //R1.Colspan = 3;
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);
                    }


                }

            }

            if(Table.Rows.Count>0)
                document.Add(Table);



        }




        #endregion

        //private void On_IndividualsForm_Closed_Pdf(DataTable dtInd)
        //{

        //    Random_Filename = null;
        //    PdfName = "Pdf File";
        //    PdfName = "RNGB0004_" + "Individual_Details";
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    //PdfName = strFolderPath + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }


        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();

        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        //    BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
        //    iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 9, 2);
        //    iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 1, BaseColor.BLUE);
        //    iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

        //    cb = writer.DirectContent;

        //    PdfPTable Snp_Table = new PdfPTable(19);
        //    Snp_Table.TotalWidth = 770f;
        //    Snp_Table.WidthPercentage = 100;
        //    Snp_Table.LockedWidth = true;
        //    float[] widths = new float[] { 15f, 15f, 15f,25f, 55f, 28f, 25f, 30f, 22f, 18f, 15f,30f,25f, 40f, 35f, 25f, 25f, 25f, 23f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //    Snp_Table.SetWidths(widths);
        //    Snp_Table.HorizontalAlignment = Element.ALIGN_CENTER;


        //    PdfPCell Header = new PdfPCell(new Phrase("Detail Individual Report", fc1));
        //    Header.Colspan = 19;
        //    Header.FixedHeight = 15f;
        //    Header.HorizontalAlignment = Element.ALIGN_CENTER;
        //    //Header.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(Header);

        //    PdfPCell row2 = new PdfPCell(new Phrase(""));
        //    row2.Colspan = 19;
        //    row2.FixedHeight = 15f;
        //    row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(row2);

        //    string[] col = { "Agy", "Dept", "Prog","App", "Client Name", "Family ID", "Client ID", "Relation", "Date", "Gender", "Age", "Ethnicity", "Race", "Education", "Health Ins", "Disabled", "Veteran", "Food Stamps", "Farmer" };
        //    for (int i = 0; i < col.Length; ++i)
        //    {
        //        PdfPCell cell = new PdfPCell(new Phrase(col[i], TableFontBoldItalic));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        //cell.FixedHeight = 15f;
        //        cell.Border = iTextSharp.text.Rectangle.BOX;
        //        Snp_Table.AddCell(cell);
        //    }

        //    foreach (DataRow dr in dtInd.Rows)
        //    {
        //        PdfPCell C1 = new PdfPCell(new Phrase(dr["Ind_Agy"].ToString(), TableFont));
        //        C1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C1);

        //        PdfPCell C2 = new PdfPCell(new Phrase(dr["Ind_Dept"].ToString(), TableFont));
        //        C2.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C2);

        //        PdfPCell C3 = new PdfPCell(new Phrase(dr["Ind_Prog"].ToString(), TableFont));
        //        C3.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C3);

        //        PdfPCell C4 = new PdfPCell(new Phrase(dr["Ind_App"].ToString(), TableFont));
        //        C4.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C4);

        //        PdfPCell C5 = new PdfPCell(new Phrase(dr["Ind_Client_Name"].ToString(), TableFont));
        //        C5.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C5.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C5);

        //        PdfPCell C6 = new PdfPCell(new Phrase(dr["Ind_Fam_ID"].ToString(), TableFont));
        //        C6.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C6.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C6);

        //        PdfPCell C7 = new PdfPCell(new Phrase(dr["Ind_CLID"].ToString(), TableFont));
        //        C7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        C7.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C7);

        //        PdfPCell C8 = new PdfPCell(new Phrase(dr["Ind_Relation"].ToString(), TableFont));
        //        C8.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C8.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C8);

        //        PdfPCell C9 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["Ind_Date"].ToString()), TableFont));
        //        C9.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C9.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C9);

        //        PdfPCell C10 = new PdfPCell(new Phrase(dr["Ind_Gender"].ToString(), TableFont));
        //        C10.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C10.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C10);

        //        PdfPCell C11 = new PdfPCell(new Phrase(dr["Ind_Age"].ToString(), TableFont));
        //        C11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        C11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C11);

        //        PdfPCell C12 = new PdfPCell(new Phrase(dr["Ind_Ethnic"].ToString(), TableFont));
        //        C12.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C12);

        //        PdfPCell C13 = new PdfPCell(new Phrase(dr["Ind_Race"].ToString(), TableFont));
        //        C13.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C13);

        //        PdfPCell C14 = new PdfPCell(new Phrase(dr["Ind_Education"].ToString(), TableFont));
        //        C14.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C14.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C14);

        //        PdfPCell C15 = new PdfPCell(new Phrase(dr["Ind_Health"].ToString(), TableFont));
        //        C15.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C15.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C15);

        //        PdfPCell C16 = new PdfPCell(new Phrase(dr["Ind_Disabled"].ToString(), TableFont));
        //        C16.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C16.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C16);

        //        PdfPCell C17 = new PdfPCell(new Phrase(dr["Ind_Vet"].ToString(), TableFont));
        //        C17.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C17.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C17);

        //        PdfPCell C18 = new PdfPCell(new Phrase(dr["Ind_Food_Stamps"].ToString(), TableFont));
        //        C18.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C18.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C18);

        //        PdfPCell C19 = new PdfPCell(new Phrase(dr["Ind_Farmer"].ToString(), TableFont));
        //        C19.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C19.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C19);

        //    }

        //    document.Add(Snp_Table);
        //    document.NewPage();

        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //}

        //private void On_FamilyForm_Closed_Pdf(DataTable dtFam)
        //{

        //    Random_Filename = null;
        //    PdfName = "Pdf File";
        //    PdfName = "RNGB0004_" + "Family_Details";
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    //PdfName = strFolderPath + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }


        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();

        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        //    BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
        //    iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 9, 2);
        //    iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 1, BaseColor.BLUE);
        //    iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

        //    cb = writer.DirectContent;

        //    PdfPTable Snp_Table = new PdfPTable(14);
        //    Snp_Table.TotalWidth = 760f;
        //    Snp_Table.WidthPercentage = 100;
        //    Snp_Table.LockedWidth = true;
        //    float[] widths = new float[] { 15f, 15f, 15f, 25f, 55f, 28f, 25f,  22f,50f, 20f, 30f,  40f, 20f, 25f};// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //    Snp_Table.SetWidths(widths);
        //    Snp_Table.HorizontalAlignment = Element.ALIGN_CENTER;


        //    PdfPCell Header = new PdfPCell(new Phrase("Detail Family Report", fc1));
        //    Header.Colspan = 14;
        //    Header.FixedHeight = 15f;
        //    Header.HorizontalAlignment = Element.ALIGN_CENTER;
        //    //Header.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(Header);

        //    PdfPCell row2 = new PdfPCell(new Phrase(""));
        //    row2.Colspan = 14;
        //    row2.FixedHeight = 15f;
        //    row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(row2);

        //    string[] col = { "Agy", "Dept", "Prog", "App", "Client Name", "Family ID", "Client ID", "Date", "Family Type", "Fam.Size", "Housing Type", "Income Types", "FPL", "Ver.Date" };
        //    for (int i = 0; i < col.Length; ++i)
        //    {
        //        PdfPCell cell = new PdfPCell(new Phrase(col[i], TableFontBoldItalic));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        //cell.FixedHeight = 15f;
        //        cell.Border = iTextSharp.text.Rectangle.BOX;
        //        Snp_Table.AddCell(cell);
        //    }

        //    foreach (DataRow dr in dtFam.Rows)
        //    {
        //        PdfPCell C1 = new PdfPCell(new Phrase(dr["Fam_Agy"].ToString(), TableFont));
        //        C1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C1);

        //        PdfPCell C2 = new PdfPCell(new Phrase(dr["Fam_Dept"].ToString(), TableFont));
        //        C2.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C2);

        //        PdfPCell C3 = new PdfPCell(new Phrase(dr["Fam_Prog"].ToString(), TableFont));
        //        C3.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C3);

        //        PdfPCell C4 = new PdfPCell(new Phrase(dr["Fam_App"].ToString(), TableFont));
        //        C4.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C4);

        //        PdfPCell C5 = new PdfPCell(new Phrase(dr["Fam_Client_Name"].ToString(), TableFont));
        //        C5.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C5.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C5);

        //        PdfPCell C6 = new PdfPCell(new Phrase(dr["Fam_FamilyID"].ToString(), TableFont));
        //        C6.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C6.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C6);

        //        PdfPCell C7 = new PdfPCell(new Phrase(dr["Fam_CLID"].ToString(), TableFont));
        //        C7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        C7.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C7);

        //        //PdfPCell C8 = new PdfPCell(new Phrase(dr["Ind_Relation"].ToString(), TableFont));
        //        //C8.HorizontalAlignment = Element.ALIGN_LEFT;
        //        //C8.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        //Snp_Table.AddCell(C8);

        //        PdfPCell C9 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["Fam_Date"].ToString()), TableFont));
        //        C9.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C9.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C9);

        //        PdfPCell C10 = new PdfPCell(new Phrase(dr["Fam_Type"].ToString(), TableFont));
        //        C10.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C10.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C10);

        //        PdfPCell C11 = new PdfPCell(new Phrase(dr["Fam_Size"].ToString(), TableFont));
        //        C11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        C11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C11);

        //        PdfPCell C12 = new PdfPCell(new Phrase(dr["Fam_Hou_Type"].ToString(), TableFont));
        //        C12.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C12);

        //        PdfPCell C13 = new PdfPCell(new Phrase(dr["Fam_Inc_Type"].ToString(), TableFont));
        //        C13.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C13);

        //        PdfPCell C14 = new PdfPCell(new Phrase(dr["Fam_FPL"].ToString(), TableFont));
        //        C14.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C14.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C14);

        //        PdfPCell C15 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["Fam_Ver_Date"].ToString()), TableFont));
        //        C15.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C15.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C15);
                
        //    }

        //    document.Add(Snp_Table);
        //    document.NewPage();

        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //}

        //private void On_ByPassForm_Closed_Pdf(DataTable dtBypass)
        //{

        //    Random_Filename = null;
        //    PdfName = "Pdf File";
        //    PdfName = "RNGB0004_" + "Bypass_Report";
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    //PdfName = strFolderPath + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }


        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();

        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
        //    BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
        //    iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 9, 2);
        //    iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 1, BaseColor.BLUE);
        //    iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

        //    cb = writer.DirectContent;

        //    PdfPTable Snp_Table = new PdfPTable(13);
        //    Snp_Table.TotalWidth = 760f;
        //    Snp_Table.WidthPercentage = 100;
        //    Snp_Table.LockedWidth = true;
        //    float[] widths = new float[] { 15f, 15f, 15f, 25f, 55f, 28f, 25f, 22f, 30f, 40f, 55f, 25f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //    Snp_Table.SetWidths(widths);
        //    Snp_Table.HorizontalAlignment = Element.ALIGN_CENTER;


        //    PdfPCell Header = new PdfPCell(new Phrase("Bypass Report", fc1));
        //    Header.Colspan = 13;
        //    Header.FixedHeight = 15f;
        //    Header.HorizontalAlignment = Element.ALIGN_CENTER;
        //    //Header.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(Header);

        //    PdfPCell row2 = new PdfPCell(new Phrase(""));
        //    row2.Colspan = 13;
        //    row2.FixedHeight = 15f;
        //    row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    Snp_Table.AddCell(row2);

        //    string[] col = { "Agy", "Dept", "Prog", "App", "Client Name", "Family ID", "Client ID", "Site", "Attribute", "Response", "Exclusion Reason", "Updated Date", "Updated By" };
        //    for (int i = 0; i < col.Length; ++i)
        //    {
        //        PdfPCell cell = new PdfPCell(new Phrase(col[i], TableFontBoldItalic));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        //cell.FixedHeight = 15f;
        //        cell.Border = iTextSharp.text.Rectangle.BOX;
        //        Snp_Table.AddCell(cell);
        //    }

        //    foreach (DataRow dr in dtBypass.Rows)
        //    {
        //        PdfPCell C1 = new PdfPCell(new Phrase(dr["Byp_Agy"].ToString(), TableFont));
        //        C1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C1);

        //        PdfPCell C2 = new PdfPCell(new Phrase(dr["Byp_Dept"].ToString(), TableFont));
        //        C2.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C2);

        //        PdfPCell C3 = new PdfPCell(new Phrase(dr["Byp_Prog"].ToString(), TableFont));
        //        C3.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C3);

        //        PdfPCell C4 = new PdfPCell(new Phrase(dr["Byp_App"].ToString(), TableFont));
        //        C4.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C4);

        //        PdfPCell C5 = new PdfPCell(new Phrase(dr["Byp_Client_Name"].ToString(), TableFont));
        //        C5.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C5.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C5);

        //        PdfPCell C6 = new PdfPCell(new Phrase(dr["Byp_Fam_ID"].ToString(), TableFont));
        //        C6.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C6.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C6);

        //        PdfPCell C7 = new PdfPCell(new Phrase(dr["Byp_CLID"].ToString(), TableFont));
        //        C7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        C7.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C7);

        //        PdfPCell C8 = new PdfPCell(new Phrase(dr["Byp_Site"].ToString(), TableFont));
        //        C8.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C8.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C8);

        //        //PdfPCell C9 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["Byp_Site"].ToString()), TableFont));
        //        //C9.HorizontalAlignment = Element.ALIGN_LEFT;
        //        //C9.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        //Snp_Table.AddCell(C9);

        //        PdfPCell C10 = new PdfPCell(new Phrase(dr["Byp_Attribute"].ToString(), TableFont));
        //        C10.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C10.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C10);

        //        //PdfPCell C11 = new PdfPCell(new Phrase(dr["Fam_Size"].ToString(), TableFont));
        //        //C11.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        //C11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        //Snp_Table.AddCell(C11);

        //        PdfPCell C12 = new PdfPCell(new Phrase(dr["Byp_Att_Resp"].ToString(), TableFont));
        //        C12.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C12.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C12);

        //        PdfPCell C13 = new PdfPCell(new Phrase(dr["Byp_Exc_Reason"].ToString(), TableFont));
        //        C13.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C13.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C13);

        //        PdfPCell C14 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["Byp_updated_date"].ToString()), TableFont));
        //        C14.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C14.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C14);

        //        PdfPCell C15 = new PdfPCell(new Phrase(dr["Byp_updated_by"].ToString(), TableFont));
        //        C15.HorizontalAlignment = Element.ALIGN_LEFT;
        //        C15.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        Snp_Table.AddCell(C15);

        //    }

        //    document.Add(Snp_Table);
        //    document.NewPage();

        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //}

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
            float[] widths2 = new float[] { 35f, 80f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
            outer.SetWidths(widths2);
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

            PdfPCell row1 = new PdfPCell(new Phrase(PrivilegeEntity.Program + " - " + PrivilegeEntity.PrivilegeName, TblHeaderTitleFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            outer.AddCell(row1);

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.PaddingBottom = 5;
            row3.MinimumHeight = 6;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            outer.AddCell(row3);

            PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Hierarchy.PaddingBottom = 5;
            outer.AddCell(Hierarchy);

            PdfPCell R3 = new PdfPCell(new Phrase("Agency: " + Sel_AGY + ", Department: " + Sel_DEPT + ", Program: " + Sel_PROG, TableFont));
            R3.Colspan = 2;
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R3.PaddingBottom = 5;
            outer.AddCell(R3);

            //1 CaseType
            PdfPCell R6 = new PdfPCell(new Phrase("  Case Type", TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            outer.AddCell(R6);

            PdfPCell R7 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString(), TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R7.PaddingBottom = 5;
            outer.AddCell(R7);

            //2 Program
            PdfPCell R26 = new PdfPCell(new Phrase("  "+ Lbl_Program.Text, TableFont));
            R26.HorizontalAlignment = Element.ALIGN_LEFT;
            R26.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R26.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R26.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R26.PaddingBottom = 5;
            outer.AddCell(R26);

            /*PdfPCell R27 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Text.ToString(), TableFont));
            R27.HorizontalAlignment = Element.ALIGN_LEFT;
            R27.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R27.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R27.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R27.PaddingBottom = 5;
            outer.AddCell(R27);*/

            string Sel_Programs = string.Empty;
            if (rbSelProgram.Checked == true)
            {
                if (SelectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity Entity in SelectedHierarchies)
                    {
                        Sel_Programs += Entity.Agency + Entity.Dept + Entity.Prog + " ,";
                    }

                    if (Sel_Programs.Length > 0)
                        Sel_Programs = Sel_Programs.Substring(0, (Sel_Programs.Length - 1));
                }
            }

            PdfPCell R27 = new PdfPCell(new Phrase((rbAllPrograms.Checked == true ? "All" : Sel_Programs), TableFont));
            R27.HorizontalAlignment = Element.ALIGN_LEFT;
            R27.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R27.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R27.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R27.PaddingBottom = 5;
            outer.AddCell(R27);

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

            PdfPCell alertCodes1 = new PdfPCell(new Phrase((rdbAlertCodesAll.Checked == true ? "All" : selectedAlertCodes), TableFont));
            alertCodes1.HorizontalAlignment = Element.ALIGN_LEFT;
            alertCodes1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            alertCodes1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            alertCodes1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            alertCodes1.PaddingBottom = 5;
            outer.AddCell(alertCodes1);

            //3 FundSource
            if (!Rb_Process_Both.Checked)
            {
                PdfPCell RFund = new PdfPCell(new Phrase("  " + "Funding Source", TableFont));
                RFund.HorizontalAlignment = Element.ALIGN_LEFT;
                RFund.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RFund.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RFund.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RFund.PaddingBottom = 5;
                outer.AddCell(RFund);

                string strSelectFund = Search_Entity.CA_Fund_Filter;
                strSelectFund = strSelectFund.Replace("'", "");

                PdfPCell RFundType = new PdfPCell(new Phrase((Rb_Fund_All.Checked == true ? "All" : strSelectFund), TableFont));
                RFundType.HorizontalAlignment = Element.ALIGN_LEFT;
                RFundType.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RFundType.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RFundType.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RFundType.PaddingBottom = 5;
                outer.AddCell(RFundType);
            }

            //4 Case Status
            PdfPCell R8 = new PdfPCell(new Phrase("  "+lblCaseStatus.Text.Trim(), TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            outer.AddCell(R8);

            PdfPCell R9 = new PdfPCell(new Phrase(Sel_params_To_Print[1], TableFont));
            R9.HorizontalAlignment = Element.ALIGN_LEFT;
            R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R9.PaddingBottom = 5;
            outer.AddCell(R9);

            //5 Milestones/Services
            PdfPCell R51 = new PdfPCell(new Phrase("  " + lblMilestones.Text.Trim(), TableFont));
            R51.HorizontalAlignment = Element.ALIGN_LEFT;
            R51.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R51.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R51.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R51.PaddingBottom = 5;
            outer.AddCell(R51);

            string CAMSRep = string.Empty, btnCAText = string.Empty, btnMSText = string.Empty; ;
            if (Rb_Process_CA.Checked)
            {
                string SelCA = string.Empty;
                if (Btn_CA_Selection.Text == "Se&l")
                    SelCA = Search_Entity.CAMS_Filter;

                if (!string.IsNullOrEmpty(SelCA.Trim())) { SelCA = SelCA.Replace("'", ""); SelCA = " ( " + SelCA + " )"; }

                if (Btn_CA_Selection.Text == "&All") 
                    btnCAText = "All"; 
                else 
                    btnCAText = "Sel";

                CAMSRep = Rb_Process_CA.Text + "  " + btnCAText/*Btn_CA_Selection.Text*/ + SelCA;
                
            }
            else if (Rb_Process_MS.Checked)
            {
                string SelMS = string.Empty;
                if (Btn_MS_Selection.Text == "Se&l")
                    SelMS = Search_Entity.CAMS_Filter;

                if (!string.IsNullOrEmpty(SelMS.Trim())) { SelMS = SelMS.Replace("'", ""); SelMS = " ( " + SelMS + " )"; }

                if (Btn_MS_Selection.Text == "&All")
                    btnMSText = "All";
                else
                    btnMSText = "Sel";

                CAMSRep = Rb_Process_MS.Text + "  " + btnMSText/*Btn_MS_Selection.Text*/ + SelMS;

            }
            else CAMSRep = "Both";

            PdfPCell R52 = new PdfPCell(new Phrase(CAMSRep, TableFont));
            R52.HorizontalAlignment = Element.ALIGN_LEFT;
            R52.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R52.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R52.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R52.PaddingBottom = 5;
            outer.AddCell(R52);

            //6 Date Selection
            PdfPCell R10 = new PdfPCell(new Phrase("  "+lblDateSelection.Text, TableFont));
            R10.HorizontalAlignment = Element.ALIGN_LEFT;
            R10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R10.PaddingBottom = 5;
            outer.AddCell(R10);

            //string Reportser = string.Empty;
            //if (Rb_Process_MS.Checked) Reportser = "Milestones"; else if (Rb_Process_CA.Checked) Reportser = "Services"; else Reportser = "Both";

            PdfPCell R11 = new PdfPCell(new Phrase((Rb_MS_AddDate.Checked ? "Add Date" : "Posting Date"), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            outer.AddCell(R11);

            ////6 Date Type Selection
            //PdfPCell R210 = new PdfPCell(new Phrase("            " + lblDateType.Text, TableFont));
            //R210.HorizontalAlignment = Element.ALIGN_LEFT;
            //R210.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R210);

            ////string Reportser = string.Empty;
            ////if (Rb_Process_MS.Checked) Reportser = "Milestones"; else if (Rb_Process_CA.Checked) Reportser = "Services"; else Reportser = "Both";

            //PdfPCell R211 = new PdfPCell(new Phrase(" : " + (rbRecent.Checked ? "Most Recent" : "Oldest"), TableFont));
            //R211.HorizontalAlignment = Element.ALIGN_LEFT;
            //R211.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R211);

            //7 Report Range
            PdfPCell R30 = new PdfPCell(new Phrase("  "+ lblRepFormat.Text, TableFont));
            R30.HorizontalAlignment = Element.ALIGN_LEFT;
            R30.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R30.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R30.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R30.PaddingBottom = 5;
            outer.AddCell(R30);

            string Repfor = string.Empty;
            if (rbRepPeriod.Checked) Repfor = "Report Period"; else Repfor = rbBoth.Text.Trim(); //else if (RbCummilative.Checked) Repfor = "Cummilative";

            PdfPCell R31 = new PdfPCell(new Phrase(Repfor, TableFont));
            R31.HorizontalAlignment = Element.ALIGN_LEFT;
            R31.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R31.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R31.PaddingBottom = 5;
            outer.AddCell(R31);

            //8 Reference Period Date Range
            if (rbBoth.Checked)
            {
                PdfPCell R12 = new PdfPCell(new Phrase("  Reference Period Date", TableFont));
                R12.HorizontalAlignment = Element.ALIGN_LEFT;
                R12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R12.PaddingBottom = 5;
                outer.AddCell(R12);

                string Date = "From: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                                + "      To: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);


                //PdfPCell R13 = new PdfPCell(new Phrase(" : From " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_From_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                //                                + "    To " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_To_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));


                PdfPCell R13 = new PdfPCell(new Phrase(Date, TableFont));
                R13.HorizontalAlignment = Element.ALIGN_LEFT;
                R13.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R13.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R13.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R13.PaddingBottom = 5;
                outer.AddCell(R13);
            }


            //9 Report Period Date Range
            if (!RbCummilative.Checked)
            {
                PdfPCell R14 = new PdfPCell(new Phrase("  Report Period Date", TableFont));
                R14.HorizontalAlignment = Element.ALIGN_LEFT;
                R14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R14.PaddingBottom = 5;
                outer.AddCell(R14);

                string Date = "From: " +
                                               CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                               + "      To: " +
                                               CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);


                //PdfPCell R15 = new PdfPCell(new Phrase(" : From " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_FDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                //                                + "    To " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_TDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));
                PdfPCell R15 = new PdfPCell(new Phrase(Date, TableFont));
                R15.HorizontalAlignment = Element.ALIGN_LEFT;
                R15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R15.PaddingBottom = 5;
                outer.AddCell(R15);
            }

            //10 Intake Site
            PdfPCell R18 = new PdfPCell(new Phrase("  "+lblSite.Text, TableFont));
            R18.HorizontalAlignment = Element.ALIGN_LEFT;
            R18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R18.PaddingBottom = 5;
            outer.AddCell(R18);

            if (Rb_Site_Sel.Checked)
            {
                string SiteDesc = string.Empty;
                SiteDesc = Get_Sel_Sites_desc();

                PdfPCell R19 = new PdfPCell(new Phrase(SiteDesc, TableFont));
                R19.HorizontalAlignment = Element.ALIGN_LEFT;
                R19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R19.PaddingBottom = 5;
                outer.AddCell(R19);

            }
            else
            {
                PdfPCell R19 = new PdfPCell(new Phrase(Sel_params_To_Print[2], TableFont));
                R19.HorizontalAlignment = Element.ALIGN_LEFT;
                R19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R19.PaddingBottom = 5;
                outer.AddCell(R19);
            }

            //11 MS Site
            if (!Rb_Process_Both.Checked)
            {
                PdfPCell R118 = new PdfPCell(new Phrase("  " + lblMssite.Text, TableFont));
                R118.HorizontalAlignment = Element.ALIGN_LEFT;
                R118.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R118.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R118.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R118.PaddingBottom = 5;
                outer.AddCell(R118);

                if (rdoMsselectsite.Checked)
                {
                    string SiteDesc = string.Empty;
                    SiteDesc = Get_Sel_MSSites_desc();

                    PdfPCell R119 = new PdfPCell(new Phrase(SiteDesc, TableFont));
                    R119.HorizontalAlignment = Element.ALIGN_LEFT;
                    R119.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R119.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R119.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R119.PaddingBottom = 5;
                    outer.AddCell(R119);

                }
                else
                {
                    string MSSiteSel = string.Empty;

                    if (rdoMssiteall.Checked) MSSiteSel = rdoMssiteall.Text.Trim(); else if (rdomsNosite.Checked) MSSiteSel = rdomsNosite.Text; else if (rdoMsselectsite.Checked) MSSiteSel = rdoMsselectsite.Text;

                    PdfPCell R119 = new PdfPCell(new Phrase(MSSiteSel, TableFont));
                    R119.HorizontalAlignment = Element.ALIGN_LEFT;
                    R119.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R119.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R119.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R119.PaddingBottom = 5;
                    outer.AddCell(R119);
                }
            }

            //12 Attributes
            PdfPCell R4 = new PdfPCell(new Phrase("  "+lblAttributes.Text, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            outer.AddCell(R4);

            PdfPCell R5 = new PdfPCell(new Phrase((Rb_Agy_Def.Checked ? "Agency Defined" : "User Defined Associations"), TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R5.PaddingBottom = 5;
            outer.AddCell(R5);

            //13 Produce detail report
            PdfPCell R28 = new PdfPCell(new Phrase("  "+ lblProduceStatistical.Text, TableFont));
            R28.HorizontalAlignment = Element.ALIGN_LEFT;
            R28.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R28.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R28.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R28.PaddingBottom = 5;
            outer.AddCell(R28);

            PdfPCell R29 = new PdfPCell(new Phrase((Rb_Details_Yes.Checked ? "Yes" : "No"), TableFont));
            R29.HorizontalAlignment = Element.ALIGN_LEFT;
            R29.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R29.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R29.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R29.PaddingBottom = 5;
            outer.AddCell(R29);


            //14 Poverty Levels
            PdfPCell R16 = new PdfPCell(new Phrase("  "+ lblPovertyLevel.Text, TableFont));
            R16.HorizontalAlignment = Element.ALIGN_LEFT;
            R16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R16.PaddingBottom = 5;
            outer.AddCell(R16);

            PdfPCell R17 = new PdfPCell(new Phrase("From: " + Txt_Pov_Low.Text + "      To: " + Txt_Pov_High.Text, TableFont));
            R17.HorizontalAlignment = Element.ALIGN_LEFT;
            R17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R17.PaddingBottom = 5;
            outer.AddCell(R17);

            
            //15 Zipcodes
            PdfPCell R20 = new PdfPCell(new Phrase("  "+lblZipCodes.Text, TableFont));
            R20.HorizontalAlignment = Element.ALIGN_LEFT;
            R20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R20.PaddingBottom = 5;
            outer.AddCell(R20);

            PdfPCell R21 = new PdfPCell(new Phrase((Rb_Zip_All.Checked ? Rb_Zip_All.Text : Rb_Zip_Sel.Text), TableFont));
            R21.HorizontalAlignment = Element.ALIGN_LEFT;
            R21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R21.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R21.PaddingBottom = 5;
            outer.AddCell(R21);

            if (chkbLangCounts.Checked)
            {
                PdfPCell R15 = new PdfPCell(new Phrase("  " + chkbLangCounts.Text.Trim(), TableFont));
                R15.HorizontalAlignment = Element.ALIGN_LEFT;
                R15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R15.PaddingBottom = 5;
                outer.AddCell(R15);

                if (rbRepPeriod.Checked)
                {
                    PdfPCell R1516 = new PdfPCell(new Phrase(chkbLangCounts.Text.Trim() + " - Report Period", TableFont));
                    R1516.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1516.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R1516.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R1516.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R1516.PaddingBottom = 5;
                    outer.AddCell(R1516);
                }
                else
                {
                    PdfPCell R1516 = new PdfPCell(new Phrase(chkbLangCounts.Text.Trim() + " - Report & Reference Period", TableFont));
                    R1516.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1516.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R1516.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R1516.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R1516.PaddingBottom = 5;
                    outer.AddCell(R1516);
                }
            }

            //16 County
            PdfPCell R22 = new PdfPCell(new Phrase("  "+lblCounty.Text, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R22.PaddingBottom = 5;
            outer.AddCell(R22);

            string CountyDesc = string.Empty;
            if (Rb_County_Sel.Checked)
            {
                CountyDesc = Get_Sel_County_Desc();

                PdfPCell R23 = new PdfPCell(new Phrase(CountyDesc, TableFont));
                R23.HorizontalAlignment = Element.ALIGN_LEFT;
                R23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R23.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R23.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R23.PaddingBottom = 5;
                outer.AddCell(R23);
            }
            else
            {
                PdfPCell R23 = new PdfPCell(new Phrase((Rb_County_All.Checked ? "All" : "Selected"), TableFont));
                R23.HorizontalAlignment = Element.ALIGN_LEFT;
                R23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R23.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R23.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R23.PaddingBottom = 5;
                outer.AddCell(R23);
            }

            //17 County Wise Count
            //**if (Rb_Details_Yes.Checked == true)
            //**{
                if (chkbMontCounty.Checked)
                {
                    PdfPCell R15 = new PdfPCell(new Phrase("  " + "County Counts", TableFont));
                    R15.HorizontalAlignment = Element.ALIGN_LEFT;
                    R15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    R15.PaddingBottom = 5;
                    outer.AddCell(R15);

                    if (rbRepPeriod.Checked)
                    {
                        PdfPCell R1516 = new PdfPCell(new Phrase("County Counts - Report Period", TableFont));
                        R1516.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1516.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        R1516.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        R1516.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                        R1516.PaddingBottom = 5;
                        outer.AddCell(R1516);
                    }
                    else
                    {
                        PdfPCell R1516 = new PdfPCell(new Phrase("County Counts - Report & Reference Period", TableFont));
                        R1516.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1516.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        R1516.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        R1516.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                        R1516.PaddingBottom = 5;
                        outer.AddCell(R1516);
                    }
                }
            //**}


            //18 Secret Applications
            PdfPCell R176 = new PdfPCell(new Phrase("  "+lblSecretApplications.Text, TableFont));
            R176.HorizontalAlignment = Element.ALIGN_LEFT;
            R176.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R176.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R176.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R176.PaddingBottom = 5;
            outer.AddCell(R176);

            PdfPCell R177 = new PdfPCell(new Phrase(Sel_params_To_Print[0], TableFont));
            R177.HorizontalAlignment = Element.ALIGN_LEFT;
            R177.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R177.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R177.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R177.PaddingBottom = 5;
            outer.AddCell(R177);

            //19 Demographic Count
            PdfPCell R24 = new PdfPCell(new Phrase("  "+lblDemographicsCount.Text , TableFont));
            R24.HorizontalAlignment = Element.ALIGN_LEFT;
            R24.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R24.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R24.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R24.PaddingBottom = 5;
            outer.AddCell(R24);

            PdfPCell R25 = new PdfPCell(new Phrase(Sel_params_To_Print[3] , TableFont));
            R25.HorizontalAlignment = Element.ALIGN_LEFT;
            R25.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R25.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R25.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R25.PaddingBottom = 5;
            outer.AddCell(R25);

            

           document.Add(outer);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }


        #region ExcelReportFormat

        public void Generate(string filename)
        {
            string pdfName = "Pdf File";
            pdfName = filename;
            //string AuditName = PdfName;
            //PdfName = strFolderPath + PdfName;
            pdfName = propReportPath + BaseForm.UserID + "\\" + pdfName;
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
                string Tmpstr = pdfName + ".xls";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = pdfName + newFileName.Substring(0, length) + ".xls";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                pdfName = Random_Filename;
            else
                pdfName += ".xls";

            Workbook book = new Workbook();
            // -----------------------------------------------
            //  Properties
            // -----------------------------------------------
            book.Properties.Author = "G SUDHEER KUMAR";
            book.Properties.LastAuthor = "G SUDHEER KUMAR";
            book.Properties.Created = new System.DateTime(2023, 10, 19, 12, 7, 48, 0);
            book.Properties.Version = "16.00";
            book.ExcelWorkbook.WindowHeight = 12105;
            book.ExcelWorkbook.WindowWidth = 28800;
            book.ExcelWorkbook.WindowTopX = 32767;
            book.ExcelWorkbook.WindowTopY = 32767;
            book.ExcelWorkbook.ProtectWindows = false;
            book.ExcelWorkbook.ProtectStructure = false;
            // -----------------------------------------------
            //  Generate Styles
            // -----------------------------------------------
            this.GenerateStyles(book.Styles);
            // -----------------------------------------------
            //  Generate Sheet1 Worksheet
            // -----------------------------------------------
            this.GenerateWorksheetSheet1(book.Worksheets);
            //book.Save(filename);

            FileStream stream = new FileStream(pdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();
            
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
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
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.NumberFormat = "@";
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Font.Bold = true;
            s63.Font.FontName = "Calibri";
            s63.Font.Bold = true;
            s63.Font.Size = 12;
            s63.Font.Color = "#666699";
            s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s63.NumberFormat = "@";
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s65.NumberFormat = "@";
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Font.Bold = true;
            s66.Font.FontName = "Calibri";
            s66.Font.Bold = true;
            s66.Font.Size = 13;
            s66.Font.Color = "#666699";
            s66.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s66.NumberFormat = "@";

        }

        private void GenerateWorksheetSheet1(WorksheetCollection sheets)
        {
            Worksheet sheet = sheets.Add("Sheet1");
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.ExpandedColumnCount = 3;
            sheet.Table.ExpandedRowCount = 27;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s62";
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Index = 2;
            column0.Width = 105;
            column0.StyleID = "s62";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 90;
            column1.StyleID = "s62";
            //-----------------------------------------------
            WorksheetRow Row00 = sheet.Table.Rows.Add();

            WorksheetCell cell;
            cell = Row00.Cells.Add();
            cell.MergeAcross = 2;
            cell.Data.Type = DataType.String;
            cell.Data.Text= "ROMA Individual/Household Characteristics";
            cell.StyleID = "s66";


            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Cells.Add("S.No", DataType.String, "s63");
            Row0.Cells.Add("County", DataType.String, "s63");
            Row0.Cells.Add("No.of Individuals", DataType.String, "s63");

            if(dtCounty.Rows.Count>0)
            {
                int i = 1;
                foreach(DataRow dr in dtCounty.Rows)
                {
                    // -----------------------------------------------
                    WorksheetRow Row1 = sheet.Table.Rows.Add();
                    Row1.Cells.Add(i.ToString(), DataType.Number, "s64");
                    Row1.Cells.Add(dr["Ind_County"].ToString(), DataType.String, "s65");
                    //WorksheetCell cell;
                    //cell = Row1.Cells.Add();
                    //cell.Data.Type = DataType.String;
                    //cell.Data.Text = dr["Ind_County"].ToString();
                    Row1.Cells.Add(dr["APPS"].ToString(), DataType.Number, "s64");

                    i++;
                }
            }


            //// -----------------------------------------------
            //WorksheetRow Row1 = sheet.Table.Rows.Add();
            //Row1.Cells.Add("1", DataType.Number, "s65");
            //WorksheetCell cell;
            //cell = Row1.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Armstrong";
            //Row1.Cells.Add("15", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row2 = sheet.Table.Rows.Add();
            //Row2.Cells.Add("2", DataType.Number, "s65");
            //cell = Row2.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Briscoe";
            //Row2.Cells.Add("31", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row3 = sheet.Table.Rows.Add();
            //Row3.Cells.Add("3", DataType.Number, "s65");
            //cell = Row3.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Carson";
            //Row3.Cells.Add("48", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row4 = sheet.Table.Rows.Add();
            //Row4.Cells.Add("4", DataType.Number, "s65");
            //cell = Row4.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Castro";
            //Row4.Cells.Add("291", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row5 = sheet.Table.Rows.Add();
            //Row5.Cells.Add("5", DataType.Number, "s65");
            //cell = Row5.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Childress";
            //Row5.Cells.Add("247", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row6 = sheet.Table.Rows.Add();
            //Row6.Cells.Add("6", DataType.Number, "s65");
            //cell = Row6.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Collingsworth";
            //Row6.Cells.Add("132", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row7 = sheet.Table.Rows.Add();
            //Row7.Cells.Add("7", DataType.Number, "s65");
            //cell = Row7.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Dallam";
            //Row7.Cells.Add("74", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row8 = sheet.Table.Rows.Add();
            //Row8.Cells.Add("8", DataType.Number, "s65");
            //cell = Row8.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Deaf Smith";
            //Row8.Cells.Add("512", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row9 = sheet.Table.Rows.Add();
            //Row9.Cells.Add("9", DataType.Number, "s65");
            //cell = Row9.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Donley";
            //Row9.Cells.Add("97", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row10 = sheet.Table.Rows.Add();
            //Row10.Cells.Add("10", DataType.Number, "s65");
            //cell = Row10.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Gray";
            //Row10.Cells.Add("746", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row11 = sheet.Table.Rows.Add();
            //Row11.Cells.Add("11", DataType.Number, "s65");
            //cell = Row11.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Hall";
            //Row11.Cells.Add("163", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row12 = sheet.Table.Rows.Add();
            //Row12.Cells.Add("12", DataType.Number, "s65");
            //cell = Row12.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Hansford";
            //Row12.Cells.Add("20", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row13 = sheet.Table.Rows.Add();
            //Row13.Cells.Add("13", DataType.Number, "s65");
            //cell = Row13.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Hartley";
            //Row13.Cells.Add("5", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row14 = sheet.Table.Rows.Add();
            //Row14.Cells.Add("14", DataType.Number, "s65");
            //cell = Row14.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Hemphill";
            //Row14.Cells.Add("46", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row15 = sheet.Table.Rows.Add();
            //Row15.Cells.Add("15", DataType.Number, "s65");
            //cell = Row15.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Hutchinson";
            //Row15.Cells.Add("428", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row16 = sheet.Table.Rows.Add();
            //Row16.Cells.Add("16", DataType.Number, "s65");
            //cell = Row16.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Lipscomb";
            //Row16.Cells.Add("33", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row17 = sheet.Table.Rows.Add();
            //Row17.Cells.Add("17", DataType.Number, "s65");
            //cell = Row17.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Moore";
            //Row17.Cells.Add("394", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row18 = sheet.Table.Rows.Add();
            //Row18.Cells.Add("18", DataType.Number, "s65");
            //cell = Row18.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Ochiltree";
            //Row18.Cells.Add("234", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row19 = sheet.Table.Rows.Add();
            //Row19.Cells.Add("19", DataType.Number, "s65");
            //cell = Row19.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Oldham";
            //Row19.Cells.Add("1", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row20 = sheet.Table.Rows.Add();
            //Row20.Cells.Add("20", DataType.Number, "s65");
            //cell = Row20.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Parmer";
            //Row20.Cells.Add("169", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row21 = sheet.Table.Rows.Add();
            //Row21.Cells.Add("21", DataType.Number, "s65");
            //cell = Row21.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Potter";
            //Row21.Cells.Add("3288", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row22 = sheet.Table.Rows.Add();
            //Row22.Cells.Add("22", DataType.Number, "s65");
            //cell = Row22.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Randall";
            //Row22.Cells.Add("1104", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row23 = sheet.Table.Rows.Add();
            //Row23.Cells.Add("23", DataType.Number, "s65");
            //cell = Row23.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Roberts";
            //Row23.Cells.Add("42", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row24 = sheet.Table.Rows.Add();
            //Row24.Cells.Add("24", DataType.Number, "s65");
            //cell = Row24.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Sherman";
            //Row24.Cells.Add("29", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row25 = sheet.Table.Rows.Add();
            //Row25.Cells.Add("25", DataType.Number, "s65");
            //cell = Row25.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Swisher";
            //Row25.Cells.Add("658", DataType.Number, "s65");
            //// -----------------------------------------------
            //WorksheetRow Row26 = sheet.Table.Rows.Add();
            //Row26.Cells.Add("26", DataType.Number, "s65");
            //cell = Row26.Cells.Add();
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Wheeler";
            //Row26.Cells.Add("123", DataType.Number, "s65");
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



        //private void OnExcel_IndividualsForm_Report(DataTable dtInd)
        //{
        //        string PdfName = "Pdf File";
        //        PdfName = "RNGB0004_Individual_Details";
        //        //string AuditName = PdfName;
        //        //PdfName = strFolderPath + PdfName;
        //        PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //        try
        //        {
        //            if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //            { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //        }
        //        catch (Exception ex)
        //        {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
        //    }

        //        try
        //        {
        //            string Tmpstr = PdfName + ".xls";
        //            if (File.Exists(Tmpstr))
        //                File.Delete(Tmpstr);
        //        }
        //        catch (Exception ex)
        //        {
        //            int length = 8;
        //            string newFileName = System.Guid.NewGuid().ToString();
        //            newFileName = newFileName.Replace("-", string.Empty);

        //            Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
        //        }

        //        if (!string.IsNullOrEmpty(Random_Filename))
        //            PdfName = Random_Filename;
        //        else
        //            PdfName += ".xls";

        //        //Workbook book = new Workbook();

        //        //this.GenerateStyles(book.Styles);

        //        ExcelDocument xlWorkSheet = new ExcelDocument();

        //        xlWorkSheet.ColumnWidth(0, 60);
        //        xlWorkSheet.ColumnWidth(1, 50);
        //        xlWorkSheet.ColumnWidth(2, 50);
        //        xlWorkSheet.ColumnWidth(3, 80);
        //        xlWorkSheet.ColumnWidth(4, 200);
        //        xlWorkSheet.ColumnWidth(5, 80);
        //        xlWorkSheet.ColumnWidth(6, 80);
        //        xlWorkSheet.ColumnWidth(7, 150);
        //        xlWorkSheet.ColumnWidth(8, 80);
        //        xlWorkSheet.ColumnWidth(9, 90);
        //        xlWorkSheet.ColumnWidth(10, 50);
        //        xlWorkSheet.ColumnWidth(11, 150);
        //        xlWorkSheet.ColumnWidth(12, 220);
        //        xlWorkSheet.ColumnWidth(13, 250);
        //        xlWorkSheet.ColumnWidth(14, 80);
        //        xlWorkSheet.ColumnWidth(15, 80);
        //        xlWorkSheet.ColumnWidth(16, 60);
        //        xlWorkSheet.ColumnWidth(17, 100);
        //        xlWorkSheet.ColumnWidth(18, 120);

        //        //Worksheet sheet = book.Worksheets.Add("Data");
        //        //sheet.Table.DefaultRowHeight = 14.25F;

        //        ////sheet.Table.DefaultColumnWidth = 220.5F;
        //        //sheet.Table.Columns.Add(50);
        //        //sheet.Table.Columns.Add(50);
        //        //sheet.Table.Columns.Add(50);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(150);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(50);
        //        //sheet.Table.Columns.Add(50);
        //        //sheet.Table.Columns.Add(70);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(80);
        //        //sheet.Table.Columns.Add(80);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);
        //        //sheet.Table.Columns.Add(60);

        //        int excelcolumn = 0;
        //        try
        //        {
        //            xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //            xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
        //            xlWorkSheet.WriteCell(excelcolumn, 5, "Detail Individual Report");

        //            excelcolumn = excelcolumn + 2;


        //            xlWorkSheet[excelcolumn, 0].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 0].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 0, "Agency");

        //            xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 1, "Dept");

        //            xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 2, "Prog");

        //            xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 3, "App");

        //            xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 4, "Client Name");

        //            xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 5, "Family ID");

        //            xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 6, "Client ID");

        //            xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 7, "Relation");

        //            xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 8, "Date");

        //            xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 9, "Gender");

        //            xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 10, "Age");

        //            xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 11, "Ethnicity");

        //            xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 12, "Race");

        //            xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 13, "Education");

        //            xlWorkSheet[excelcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 14].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 14, "Health Ins");

        //            xlWorkSheet[excelcolumn, 15].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 15].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 15, "Disabled");

        //            xlWorkSheet[excelcolumn, 16].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 16].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 16, "Veteran");

        //            xlWorkSheet[excelcolumn, 17].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 17].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 17, "Food Stamps");

        //            xlWorkSheet[excelcolumn, 18].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //            xlWorkSheet[excelcolumn, 18].Alignment = Alignment.Centered;
        //            xlWorkSheet.WriteCell(excelcolumn, 18, "Farmer");

        //            //WorksheetRow Row0 = sheet.Table.Rows.Add();

        //            //WorksheetCell cell;
        //            //cell = Row0.Cells.Add("Agency", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Dept", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Prog", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("App#", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Client Name", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Family ID", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Client ID", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Relation", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Date", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Gender", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Age", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Ethnicity", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Race", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Education", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Health Ins", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Disabled", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Veteran", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Food Stamps", DataType.String, "s94");
        //            //cell = Row0.Cells.Add("Farmer", DataType.String, "s94");

        //            if (dtInd.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dtInd.Rows)
        //                {
        //                    excelcolumn = excelcolumn + 1;
        //                    xlWorkSheet.WriteCell(excelcolumn, 0, dr["Ind_Agy"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 1, dr["Ind_Dept"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 2, dr["Ind_Prog"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 3, dr["Ind_App"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 4, dr["Ind_Client_Name"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 5, dr["Ind_Fam_ID"].ToString());

        //                    xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                    xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Right;
        //                    xlWorkSheet.WriteCell(excelcolumn, 6, dr["Ind_CLID"].ToString());

        //                    //xlWorkSheet.WriteCell(excelcolumn, 7, dr["Ind_CLID"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 7, dr["Ind_Relation"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 8, LookupDataAccess.Getdate(dr["Ind_Date"].ToString()));
        //                    xlWorkSheet.WriteCell(excelcolumn, 9, dr["Ind_Gender"].ToString());

        //                    xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                    xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Right;
        //                    xlWorkSheet.WriteCell(excelcolumn, 10, dr["Ind_Age"].ToString());

        //                    //xlWorkSheet.WriteCell(excelcolumn, 11, dr["Ind_Age"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 11, dr["Ind_Ethnic"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 12, dr["Ind_Race"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 13, dr["Ind_Education"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 14, dr["Ind_Health"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 15, dr["Ind_Disabled"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 16, dr["Ind_Vet"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 17, dr["Ind_Food_Stamps"].ToString());
        //                    xlWorkSheet.WriteCell(excelcolumn, 18, dr["Ind_Farmer"].ToString());


        //                    //WorksheetRow Row1 = sheet.Table.Rows.Add();
        //                    //cell = Row1.Cells.Add(dr["Ind_Agy"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Dept"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Prog"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_App"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Client_Name"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Fam_ID"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_CLID"].ToString(), DataType.Number, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Relation"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(LookupDataAccess.Getdate(dr["Ind_Date"].ToString()), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Gender"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Age"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Ethnic"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Race"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Education"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Health"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Disabled"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Vet"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Food_Stamps"].ToString(), DataType.String, "s96");
        //                    //cell = Row1.Cells.Add(dr["Ind_Farmer"].ToString(), DataType.String, "s96");
        //                }
        //            }


        //            FileStream stream = new FileStream(PdfName, FileMode.Create);

        //            xlWorkSheet.Save(stream);
        //            stream.Close();
        //            //FileStream stream = new FileStream(PdfName, FileMode.Create);

        //            //book.Save(stream);
        //            //stream.Close();

        //        }
        //        catch (Exception ex) { }

        //        //Generate(PdfName);
        //    }

        //private void OnExcel_FamilyForm_Report(DataTable dtFam)
        //{
        //    string PdfName = "Pdf File";
        //    PdfName = "RNGB0004_Family_Details";
        //    //string AuditName = PdfName;
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
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

        //    //Workbook book = new Workbook();

        //    //this.GenerateStyles(book.Styles);

        //    ExcelDocument xlWorkSheet = new ExcelDocument();

        //    xlWorkSheet.ColumnWidth(0, 60);
        //    xlWorkSheet.ColumnWidth(1, 50);
        //    xlWorkSheet.ColumnWidth(2, 50);
        //    xlWorkSheet.ColumnWidth(3, 80);
        //    xlWorkSheet.ColumnWidth(4, 200);
        //    xlWorkSheet.ColumnWidth(5, 80);
        //    xlWorkSheet.ColumnWidth(6, 80);
        //    xlWorkSheet.ColumnWidth(7, 80);
        //    xlWorkSheet.ColumnWidth(8, 200);
        //    xlWorkSheet.ColumnWidth(9, 90);
        //    xlWorkSheet.ColumnWidth(10, 150);
        //    xlWorkSheet.ColumnWidth(11, 200);
        //    xlWorkSheet.ColumnWidth(12, 80);
        //    xlWorkSheet.ColumnWidth(13, 80);
        //    xlWorkSheet.ColumnWidth(13, 100);
        //    //xlWorkSheet.ColumnWidth(14, 80);
        //    //xlWorkSheet.ColumnWidth(15, 80);
        //    //xlWorkSheet.ColumnWidth(16, 60);
        //    //xlWorkSheet.ColumnWidth(17, 100);
        //    //xlWorkSheet.ColumnWidth(18, 120);

        //    int excelcolumn = 0;
        //    try
        //    {
        //        xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //        xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
        //        xlWorkSheet.WriteCell(excelcolumn, 5, "Detail Family Report");

        //        excelcolumn = excelcolumn + 2;


        //        xlWorkSheet[excelcolumn, 0].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 0].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 0, "Agency");

        //        xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 1, "Dept");

        //        xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 2, "Prog");

        //        xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 3, "App");

        //        xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 4, "Client Name");

        //        xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 5, "Family ID");

        //        xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 6, "Client ID");

        //        xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 7, "Date");

        //        xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 8, "Family Type");

        //        xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 9, "Fam.Size");

        //        xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 10, "Housing Type");

        //        xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 11, "Income Types");

        //        xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 12, "FPL");

        //        xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 13, "Ver.Date");

        //        xlWorkSheet[excelcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 14].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 14, "Non-Cash Benifits");

        //        //xlWorkSheet[excelcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 14].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 14, "Health Ins");

        //        //xlWorkSheet[excelcolumn, 15].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 15].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 15, "Disabled");

        //        //xlWorkSheet[excelcolumn, 16].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 16].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 16, "Veteran");

        //        //xlWorkSheet[excelcolumn, 17].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 17].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 17, "Food Stamps");

        //        //xlWorkSheet[excelcolumn, 18].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 18].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 18, "Farmer");

        //        if (dtFam.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtFam.Rows)
        //            {
        //                excelcolumn = excelcolumn + 1;
        //                xlWorkSheet.WriteCell(excelcolumn, 0, dr["Fam_Agy"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 1, dr["Fam_Dept"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 2, dr["Fam_Prog"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 3, dr["Fam_App"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 4, dr["Fam_Client_Name"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 5, dr["Fam_FamilyID"].ToString());

        //                xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 6, dr["Fam_CLID"].ToString());

        //                //xlWorkSheet.WriteCell(excelcolumn, 7, dr["Ind_CLID"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 7, LookupDataAccess.Getdate(dr["Fam_Date"].ToString()));
        //                xlWorkSheet.WriteCell(excelcolumn, 8, dr["Fam_Type"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 9, dr["Fam_Size"].ToString());

        //                //xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                //xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 10, dr["Fam_Hou_Type"].ToString());

        //                //xlWorkSheet.WriteCell(excelcolumn, 11, dr["Ind_Age"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 11, dr["Fam_Inc_Type"].ToString());

        //                xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 12, dr["Fam_FPL"].ToString());

        //                xlWorkSheet.WriteCell(excelcolumn, 13, LookupDataAccess.Getdate(dr["Fam_Ver_Date"].ToString()));
        //                xlWorkSheet.WriteCell(excelcolumn, 14, dr["Fam_Cash_Ben"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 14, dr["Ind_Health"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 15, dr["Ind_Disabled"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 16, dr["Ind_Vet"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 17, dr["Ind_Food_Stamps"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 18, dr["Ind_Farmer"].ToString());

        //            }
        //        }


        //        FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        xlWorkSheet.Save(stream);
        //        stream.Close();
        //        //FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        //book.Save(stream);
        //        //stream.Close();

        //    }
        //    catch (Exception ex) { }

        //    //Generate(PdfName);
        //}

        //private void OnExcel_ByPassForm_Report(DataTable dtBypass)
        //{
        //    string PdfName = "Pdf File";
        //    PdfName = "RNGB0004_Bypass_Report";
        //    //string AuditName = PdfName;
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        AlertBox.Show("Error", MessageBoxIcon.Error);
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

        //    //Workbook book = new Workbook();

        //    //this.GenerateStyles(book.Styles);

        //    ExcelDocument xlWorkSheet = new ExcelDocument();

        //    xlWorkSheet.ColumnWidth(0, 60);
        //    xlWorkSheet.ColumnWidth(1, 50);
        //    xlWorkSheet.ColumnWidth(2, 50);
        //    xlWorkSheet.ColumnWidth(3, 80);
        //    xlWorkSheet.ColumnWidth(4, 200);
        //    xlWorkSheet.ColumnWidth(5, 80);
        //    xlWorkSheet.ColumnWidth(6, 80);
        //    xlWorkSheet.ColumnWidth(7, 80);
        //    xlWorkSheet.ColumnWidth(8, 200);
        //    xlWorkSheet.ColumnWidth(9, 150);
        //    xlWorkSheet.ColumnWidth(10, 250);
        //    xlWorkSheet.ColumnWidth(11, 120);
        //    xlWorkSheet.ColumnWidth(12, 120);
        //    //xlWorkSheet.ColumnWidth(13, 80);
        //    //xlWorkSheet.ColumnWidth(14, 80);
        //    //xlWorkSheet.ColumnWidth(15, 80);
        //    //xlWorkSheet.ColumnWidth(16, 60);
        //    //xlWorkSheet.ColumnWidth(17, 100);
        //    //xlWorkSheet.ColumnWidth(18, 120);

        //    int excelcolumn = 0;
        //    try
        //    {

        //        xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //        xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
        //        xlWorkSheet.WriteCell(excelcolumn, 5, "Bypass Report");

        //        excelcolumn = excelcolumn + 2;

        //        xlWorkSheet[excelcolumn, 0].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 0].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 0, "Agency");

        //        xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 1, "Dept");

        //        xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 2, "Prog");

        //        xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 3, "App");

        //        xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 4, "Client Name");

        //        xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 5, "Family ID");

        //        xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 6, "Client ID");

        //        xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 7, "Site");

        //        xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 8, "Attribute");

        //        xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 9, "Response");

        //        xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 10, "Exclusion Reason");

        //        xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 11, "Updated Date");

        //        xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 12, "Updated by");

        //        //xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 13, "Ver.Date");

        //        //xlWorkSheet[excelcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 14].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 14, "Health Ins");

        //        //xlWorkSheet[excelcolumn, 15].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 15].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 15, "Disabled");

        //        //xlWorkSheet[excelcolumn, 16].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 16].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 16, "Veteran");

        //        //xlWorkSheet[excelcolumn, 17].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 17].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 17, "Food Stamps");

        //        //xlWorkSheet[excelcolumn, 18].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //xlWorkSheet[excelcolumn, 18].Alignment = Alignment.Centered;
        //        //xlWorkSheet.WriteCell(excelcolumn, 18, "Farmer");

        //        if (dtBypass.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtBypass.Rows)
        //            {
        //                excelcolumn = excelcolumn + 1;
        //                xlWorkSheet.WriteCell(excelcolumn, 0, dr["Byp_Agy"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 1, dr["Byp_Dept"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 2, dr["Byp_Prog"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 3, dr["Byp_App"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 4, dr["Byp_Client_Name"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 5, dr["Byp_Fam_ID"].ToString());

        //                xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
        //                xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Right;
        //                xlWorkSheet.WriteCell(excelcolumn, 6, dr["Byp_CLID"].ToString());

        //                xlWorkSheet.WriteCell(excelcolumn, 7, dr["Byp_Site"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 8, dr["Byp_Attribute"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 9, dr["Byp_Att_Resp"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 10, dr["Byp_Exc_Reason"].ToString());
        //                xlWorkSheet.WriteCell(excelcolumn, 11, LookupDataAccess.Getdate(dr["Byp_updated_date"].ToString()));
        //                xlWorkSheet.WriteCell(excelcolumn, 12, dr["Byp_updated_by"].ToString());

        //                //xlWorkSheet.WriteCell(excelcolumn, 13, LookupDataAccess.Getdate(dr["Fam_Ver_Date"].ToString()));
        //                //xlWorkSheet.WriteCell(excelcolumn, 14, dr["Ind_Health"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 15, dr["Ind_Disabled"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 16, dr["Ind_Vet"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 17, dr["Ind_Food_Stamps"].ToString());
        //                //xlWorkSheet.WriteCell(excelcolumn, 18, dr["Ind_Farmer"].ToString());

        //            }
        //        }


        //        FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        xlWorkSheet.Save(stream);
        //        stream.Close();
        //        //FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        //book.Save(stream);
        //        //stream.Close();

        //    }
        //    catch (Exception ex) { }

        //    //Generate(PdfName);
        //}

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
        //    //  s62
        //    // -----------------------------------------------
        //    WorksheetStyle s62 = styles.Add("s62");
        //    s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s63
        //    // -----------------------------------------------
        //    WorksheetStyle s63 = styles.Add("s63");
        //    s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s64
        //    // -----------------------------------------------
        //    WorksheetStyle s64 = styles.Add("s64");
        //    s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
        //    s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s65
        //    // -----------------------------------------------
        //    WorksheetStyle s65 = styles.Add("s65");
        //    s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
        //    s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s66
        //    // -----------------------------------------------
        //    WorksheetStyle s66 = styles.Add("s66");
        //    s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s67
        //    // -----------------------------------------------
        //    WorksheetStyle s67 = styles.Add("s67");
        //    s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s68
        //    // -----------------------------------------------
        //    WorksheetStyle s68 = styles.Add("s68");
        //    s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s68.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s69
        //    // -----------------------------------------------
        //    WorksheetStyle s69 = styles.Add("s69");
        //    s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s69.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s70
        //    // -----------------------------------------------
        //    WorksheetStyle s70 = styles.Add("s70");
        //    s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s70.NumberFormat = "0%";
        //    // -----------------------------------------------
        //    //  s71
        //    // -----------------------------------------------
        //    WorksheetStyle s71 = styles.Add("s71");
        //    s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    // -----------------------------------------------
        //    //  s72
        //    // -----------------------------------------------
        //    WorksheetStyle s72 = styles.Add("s72");
        //    s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
        //    s72.NumberFormat = "0%";
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
        //    s95.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //    s95.Alignment.Vertical = StyleVerticalAlignment.Top;
        //    s95.Alignment.WrapText = true;
        //    s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
        //    // -----------------------------------------------
        //    //  s96
        //    // -----------------------------------------------
        //    WorksheetStyle s96 = styles.Add("s96");
        //    s96.Font.FontName = "Arial";
        //    s96.Font.Color = "#000000";
        //    s96.Interior.Color = "#FFFFFF";
        //    s96.Interior.Pattern = StyleInteriorPattern.Solid;
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
        //    // -----------------------------------------------
        //    //  s106
        //    // -----------------------------------------------
        //    WorksheetStyle s106 = styles.Add("s106");
        //    s106.NumberFormat = "0%";
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
        //}



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

        //private bool Validate_PM_DateRanges()
        //{
        //    bool Valid_Dates = false;

        //    foreach (Csb16DTREntity Entity in DateRange_List)
        //    {
        //        if (Ref_From_Date.Value.ToShortDateString() == Convert.ToDateTime(Entity.REF_FDATE).ToShortDateString() &&
        //            Ref_To_Date.Value.ToShortDateString() == Convert.ToDateTime(Entity.REF_TDATE).ToShortDateString())
        //        {
        //            Valid_Dates = true; break;
        //        }
        //    }

        //    if (!Valid_Dates)
        //    {
        //        string Disp_Date_Ranges = "Available Date Ranges are as Below \n \n" +
        //                                  "               From                TO ";
        //        int i = 1;
        //        foreach (Csb16DTREntity Entity in DateRange_List)
        //        {
        //            Disp_Date_Ranges += "\n " + (i <= 9 ? "  " : "" ) + i.ToString() + ").  " + LookupDataAccess.Getdate(Entity.REF_FDATE) + "   -   " + LookupDataAccess.Getdate(Entity.REF_TDATE);
        //            i++;
        //        }
        //        AlertBox.Show(Disp_Date_Ranges, MessageBoxIcon.Warning);
        //    }

        //    return Valid_Dates;
        //}

        private bool Validate_Report()
        {
            bool Can_Generate = true;

            if (!rbRepPeriod.Checked)
            {
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
            }

            if (!RbCummilative.Checked)
            {
                if (!Rep_From_Date.Checked)
                {
                    _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'From Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rep_From_Date, null);

                if (!Rep_To_Date.Checked)
                {
                    _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
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
            }

            if (rbBoth.Checked)
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
            //else
            //{
            //    if (Ref_To_Date.Checked && Ref_From_Date.Checked && Rep_To_Date.Checked && Rep_From_Date.Checked)
            //        _errorProvider.SetError(Rep_From_Date, null);
            //}


            if ((Rb_Zip_Sel.Checked && ListZipCode.Count <= 0 && Scr_Oper_Mode == "RNGB0004") || (Rb_Zip_Sel.Checked && ListGroupCode.Count <= 0 && Scr_Oper_Mode == "CASB0014"))
            {
                _errorProvider.SetError(Rb_Zip_Sel, string.Format("Please Select at least One " + (Scr_Oper_Mode == "RNGB0004" ? "ZIP Code" : "Group").Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Zip_Sel, null);

            if (Scr_Oper_Mode == "RNGB0004" || Scr_Oper_Mode == "CASB0014")
            {
                if (Rb_County_Sel.Checked && ListcommonEntity.Count <= 0)
                {
                    _errorProvider.SetError(Rb_County_Sel, string.Format("Please Select at least One County".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_County_Sel, null);
            }

            if (Rb_Site_Sel.Checked && string.IsNullOrEmpty(Txt_Sel_Site.Text.Trim()))
            {
                _errorProvider.SetError(Rb_Site_Sel, string.Format("Please Select at least One Site".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Site_Sel, null);

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
                    _errorProvider.SetError(Txt_Pov_High, string.Format("Poverty Level 'Low' Should not exceed 'High'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Txt_Pov_High, null);            
            }

            if (Scr_Oper_Mode == "RNGB0004")
            {
                if (Rb_Fund_Sel.Checked && Sel_Funding_List.Count <= 0)
                {
                    _errorProvider.SetError(Rb_Fund_Sel, string.Format("Please Select at least One Fund".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_Fund_Sel, null);
            }

            if (rdbSelectedAlertCodes.Checked)
            {
                if (SelectedAlertCodes == "")
                {
                    _errorProvider.SetError(rdbSelectedAlertCodes, "Please Select atleast one Alert Code");
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(rdbSelectedAlertCodes, null);
                }
            }


            return Can_Generate;
        }



        DataTable dt = new DataTable();
        DataTable dt_Summary = new DataTable();
        //bool[] Pagesetup_results = new bool[5];
        bool Include_header = false, Include_Footer = false, Include_Header_Title = false, Include_Header_Image = false,
             Include_Footer_PageCnt = false, Save_This_Adhoc_Criteria = false, Page_Setup_Completed = false;
        string Main_Rep_Name = " ", Rep_Name = " ", Rep_Header_Title = " ", Page_Orientation = "A4 Portrait", Pub_SubRep_Name = string.Empty;
        string Bypass_Rep_Name = string.Empty, Ind_Rep_Name = string.Empty, MST_Rep_Name = string.Empty;
        //private void On_Pagesetup_Form_Closed(object sender, FormClosedEventArgs e)
        //{
        //    Include_header = Include_Footer = Include_Header_Title = Include_Header_Image =
        //    Include_Footer_PageCnt = Save_This_Adhoc_Criteria = false;

        //    Rep_Name = Rep_Header_Title = " "; Page_Orientation = "A4 Portrait";

        //    CASB2012_AdhocPageSetup form = sender as CASB2012_AdhocPageSetup;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        //form.Close();
        //        //System.Threading.Thread.Sleep(200);
        //        Page_Setup_Completed = true;
        //        Pagesetup_results = form.Get_Checkbox_Status();

        //        Include_header = Pagesetup_results[0]; Include_Header_Title = Pagesetup_results[1]; Include_Header_Image = Pagesetup_results[2];
        //        Include_Footer = Pagesetup_results[3]; Include_Footer_PageCnt = Pagesetup_results[4];
        //        Save_This_Adhoc_Criteria = Pagesetup_results[5];

        //        if (Include_Header_Title)
        //            Rep_Header_Title = form.Get_Header_Title();

        //        Main_Rep_Name = Pub_SubRep_Name = Rep_Name = "SYSTEM" + (Scr_Oper_Mode == "RNGB0004" ? "_DG_" : "_PM_")+ form.Get_Report_Name();
        //        Rep_Name += ".rdlc"; Pub_SubRep_Name += "SummaryReport";

        //        Bypass_Rep_Name = string.Empty; Ind_Rep_Name = string.Empty; MST_Rep_Name = string.Empty;

        //        Page_Orientation = form.Get_Page_Orientation();

        //        //switch (Page_Orientation)
        //        //{
        //        //    case "A4 Portrait": Rb_A4_Port.Checked = true; break;
        //        //    default: Rb_A4_Land.Checked = true; break;
        //        //}


        //        //string Secret_SW = ((ListItem)Cmb_Applications.SelectedItem).Value.ToString();
        //        //string Group_Sort_SW = ((ListItem)Cmb_Group_Sort.SelectedItem).Value.ToString();
        //        //string Use_Casediff_SW = Cb_Use_DIFF.Checked ? "Y" : "N";
        //        //string Include_Mambers = Cb_Inc_Menbers.Checked ? "Y" : "N";

        //        //string[] XML_String = new string[2];
        //        //XML_String = Get_XML_Format_of_Selected_Rows();

        //        //if (Summary_Sw)
        //        //    Generete_Dynamic_Summary_RDLC();


        //        Get_Selection_Criteria();

        //        DataSet ds = new DataSet();
        //        if (Scr_Oper_Mode == "RNGB0004")
        //            ds = _model.AdhocData.Get_DG_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");
        //        else
        //            ds = _model.AdhocData.Get_PM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");

        //        bool Data_processed = false;
        //        if (ds.Tables.Count > 0)
        //        {
        //            Delete_RDLC_Brfore_Creation();

        //            if (Scr_Oper_Mode == "RNGB0004")
        //            {
        //                if (ds.Tables[1].Rows.Count > 0)
        //                {
        //                    dt = ds.Tables[2];

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        Dynamic_RDLC();
        //                        Bypass_Dynamic_RDLC();
        //                        SNP_Dynamic_RDLC();
        //                        MST_Dynamic_RDLC();

        //                        Data_processed = true;
        //                        CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(Scr_Oper_Mode, ds, dt, dt_Summary, Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked, ReportPath, BaseForm.UserID);
        //                        RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                        RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
        //                        RDLC_Form.ShowDialog();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (ds.Tables.Count > 10)
        //                {
        //                    if (ds.Tables[11].Rows.Count > 0)
        //                    {
        //                        dt = ds.Tables[11];

        //                        if (dt.Rows.Count > 0)
        //                        {
        //                            Data_processed = true;
        //                            PerformanceMeasures_Dynamic_RDLC();

        //                            if (Rb_Details_Yes.Checked)
        //                                PerformanceMeasures_Details_Dynamic_RDLC();

        //                            //CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(ds, dt, dt_Summary, Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked);
        //                            //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                            //RDLC_Form.ShowDialog();

        //                            //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(dt, dt_Summary, Rep_Name, "Result Table");
        //                            //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                            //RDLC_Form.ShowDialog();

        //                            CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(Scr_Oper_Mode, ds, dt, ds.Tables[12], Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked, ReportPath,BaseForm.UserID);
        //                            RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                            RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
        //                            RDLC_Form.ShowDialog();
        //                        }
        //                    }
        //                    //else
        //                    //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");

        //                }
        //                //else
        //                //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");
        //            }
        //        }
        //        //else
        //        //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");

        //        if (!Data_processed)
        //            AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);
        //    }
        //}

        //private void Process_report()
        //{
        //    Get_Selection_Criteria();

        //    DataSet ds = new DataSet();
        //    if (Scr_Oper_Mode == "RNGB0004")
        //        ds = _model.AdhocData.Get_DG_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");
        //    else
        //        ds = _model.AdhocData.Get_PM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N");

        //    bool Data_processed = false;
        //    if (ds.Tables.Count > 0)
        //    {
        //        if (Scr_Oper_Mode == "RNGB0004")
        //        {
        //            if (ds.Tables[1].Rows.Count > 0)
        //            {
        //                dt = ds.Tables[2];

        //                if (dt.Rows.Count > 0)
        //                {
        //                    Dynamic_RDLC();
        //                    Bypass_Dynamic_RDLC();
        //                    SNP_Dynamic_RDLC();
        //                    MST_Dynamic_RDLC();

        //                    Data_processed = true;
        //                    CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(Scr_Oper_Mode, ds, dt, dt_Summary, Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked, ReportPath, BaseForm.UserID);
        //                    RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                    RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
        //                    RDLC_Form.ShowDialog();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (ds.Tables.Count > 10)
        //            {
        //                if (ds.Tables[11].Rows.Count > 0)
        //                {
        //                    dt = ds.Tables[11];

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        Data_processed = true;
        //                        PerformanceMeasures_Dynamic_RDLC();

        //                        if (Rb_Details_Yes.Checked)
        //                            PerformanceMeasures_Details_Dynamic_RDLC();

        //                        //CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(ds, dt, dt_Summary, Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked);
        //                        //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                        //RDLC_Form.ShowDialog();

        //                        //CASB2012_AdhocRDLCForm RDLC_Form = new CASB2012_AdhocRDLCForm(dt, dt_Summary, Rep_Name, "Result Table");
        //                        //RDLC_Form.FormClosed += new Form.FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                        //RDLC_Form.ShowDialog();

        //                        CASB0004_DG_RDLCForm RDLC_Form = new CASB0004_DG_RDLCForm(Scr_Oper_Mode, ds, dt, ds.Tables[12], Main_Rep_Name, Rep_Name, "Result Table", Rb_Details_Yes.Checked, ReportPath, BaseForm.UserID);
        //                        RDLC_Form.FormClosed += new FormClosedEventHandler(Delete_Dynamic_RDLC_File);
        //                        RDLC_Form.StartPosition = FormStartPosition.CenterScreen;
        //                        RDLC_Form.ShowDialog();
        //                    }
        //                }
        //                //else
        //                //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");

        //            }
        //            //else
        //            //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");
        //        }
        //    }
        //    //else
        //    //    MessageBox.Show("No Records exists with selected Criteria", "CAP Systems");

        //    if (!Data_processed)
        //        AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);
        //}


        //DirectoryInfo MyDir;
        //private void Delete_Dynamic_RDLC_File(object sender, FormClosedEventArgs e)
        //{
        //    CASB2012_AdhocRDLCForm form = sender as CASB2012_AdhocRDLCForm;
        //    //MyDir = new DirectoryInfo(@"C:\Capreports\");
        //    //MyDir = new DirectoryInfo(Consts.Common.ReportFolderLocation + "\\"); // Run at Server
        //    //MyDir = new DirectoryInfo(Consts.Common.Tmp_ReportFolderLocation + "\\"); // Run at Server
        //    MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server
            
        //    FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
        //    bool MasterRep_Deleted = false, Bypass_Rep_Deleted = false, IND_Rep_Deleted = false, FAM_Rep_Deleted = false, PM_Detailed_Rep_Deleted = false;
        //    string Bypass_Rep_Name, IND_Rep_Name, FAM_Rep_Name, PM_Details_Rep_Name;

        //    //Bypass_Rep_Name = IND_Rep_Name = FAM_Rep_Name = RemoveBetween(Rep_Name, '.', 'c');

        //    Bypass_Rep_Name = IND_Rep_Name = FAM_Rep_Name = PM_Details_Rep_Name= Main_Rep_Name;
        //    Bypass_Rep_Name += "Bypass_RdlC.rdlc";
        //    IND_Rep_Name += "SNP_IND_RdlC.rdlc";
        //    FAM_Rep_Name += "MST_FAM_RdlC.rdlc";
        //    PM_Details_Rep_Name += "Detail_RdlC.rdlc";
        //    if(Scr_Oper_Mode == "RNGB0004")
        //        PM_Detailed_Rep_Deleted = true;

        //    //if (!Summary_Sw)
        //    //    SubReport_Deleted = true;
        //    foreach (FileInfo MyFile in MyFiles)
        //    {
        //        if (MyFile.Exists)
        //        {

        //            if (Rep_Name == MyFile.Name && !MasterRep_Deleted)
        //                {   MyFile.Delete(); MasterRep_Deleted = true; }

        //            if (Bypass_Rep_Name == MyFile.Name && !Bypass_Rep_Deleted)
        //                { MyFile.Delete(); Bypass_Rep_Deleted = true; }

        //            if (IND_Rep_Name == MyFile.Name && !IND_Rep_Deleted)
        //                { MyFile.Delete(); IND_Rep_Deleted = true; }

        //            if (FAM_Rep_Name == MyFile.Name && !FAM_Rep_Deleted)
        //                { MyFile.Delete(); FAM_Rep_Deleted = true; }

        //            if(Scr_Oper_Mode == "CASB0014")
        //            {
        //                if (PM_Details_Rep_Name == MyFile.Name && !PM_Detailed_Rep_Deleted)
        //                { MyFile.Delete(); PM_Detailed_Rep_Deleted = true; }
        //            }


        //            if (MasterRep_Deleted && Bypass_Rep_Deleted 
        //                && IND_Rep_Deleted && FAM_Rep_Deleted && PM_Detailed_Rep_Deleted)
        //                break;
        //        }
        //    }
        //}

        //string RemoveBetween(string s, char begin, char end)
        //{
        //    Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
        //    return regex.Replace(s, string.Empty);
        //}

        DG_Browse_Entity Search_Entity = new DG_Browse_Entity(true);
        PM_Browse_Entity PM_Search_Entity = new PM_Browse_Entity(true);
        string[] Sel_params_To_Print = new string[20] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
        private void Get_Selection_Criteria()
        {
            Sel_params_To_Print[3] = string.Empty;
            switch (Scr_Oper_Mode)
            {
                case "RNGB0004":
                        Search_Entity.Attribute = "SYSTEM";
                        if (Rb_User_Def.Checked)
                            Search_Entity.Attribute = "USER";

                        if (Rb_Zip_All.Checked)
                            Search_Entity.ZipCode = "**";
                        else
                            Search_Entity.ZipCode = Get_Sel_ZipCodes();

                        if (Rb_County_All.Checked)
                            Search_Entity.County = "**";
                        else
                            Search_Entity.County = Get_Sel_County();

                        Search_Entity.DG_Count_Sw = "SNP";
                        if (Rb_OBO_Mem.Checked)
                            Search_Entity.DG_Count_Sw = "OBO";

                    Search_Entity.DateType = "O";
                    //if (rbOldest.Checked)
                    //    Search_Entity.DateType = "O";
                    //else if(rbRecent.Checked)
                    //    Search_Entity.DateType = "R";

                    switch (Search_Entity.DG_Count_Sw)
                        {
                            case "OBO": Sel_params_To_Print[3] = "Service/Outcome Recipient"; break;
                            default: Sel_params_To_Print[3] = "All Household Members"; break;
                        }

                        Search_Entity.CAMS_Filter = "**";
                        if (!All_CAMS_Selected)
                            Search_Entity.CAMS_Filter = Get_Sel_CAMS_List_To_Filter();

                        Search_Entity.CA_Fund_Filter = "**";
                        if (Rb_Fund_Sel.Checked)
                            Search_Entity.CA_Fund_Filter = Get_Sel_CA_Fund_List_To_Filter();

                        break;

                case "CASB0014":
                        Search_Entity.Attribute = "All";
                        if (Rb_User_Def.Checked)
                            Search_Entity.Attribute = "Goal";

                        if (Rb_Zip_All.Checked)
                            Search_Entity.ZipCode = "**";
                        else
                            Search_Entity.ZipCode = Get_Sel_GroupCodes();

                        Search_Entity.County = "**";
                        if (Rb_County_Sel.Checked)
                            Search_Entity.County = Get_Sel_County();

                        Search_Entity.PM_Rep_Format = "PM";
                        if (Rb_SNP_Mem.Checked)
                            Search_Entity.PM_Rep_Format = "Both";

                        Search_Entity.DG_Count_Sw = Search_Entity.PM_Rep_Format;

                        break;
            }

            
            Search_Entity.Ms_DriveColumn_Sw = "MSDATE";
            if (Rb_MS_AddDate.Checked)
                Search_Entity.Ms_DriveColumn_Sw = "MSADDDATE";

            Search_Entity.CA_MS_Sw = "MS";
            if (Rb_Process_CA.Checked)
            {
                Search_Entity.CA_MS_Sw = "CA";
                //Search_Entity.Ms_DriveColumn_Sw = "CADATE";
                //if (Rb_MS_AddDate.Checked)
                //    Search_Entity.Ms_DriveColumn_Sw = "CAADDDATE";
            }
            else if(Rb_Process_Both.Checked) Search_Entity.CA_MS_Sw="Both";

            if (rbRepPeriod.Checked) Search_Entity.RepRange = "Rep";  else if (rbBoth.Checked) Search_Entity.RepRange = "Both"; //else if (RbCummilative.Checked) Search_Entity.RepRange = "Ref";

            if (Ref_From_Date.Enabled) Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_From_Date.Value.ToShortDateString(); 
            else Search_Entity.Rep_From_Date = string.Empty;
            if (Ref_To_Date.Enabled) Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
            else Search_Entity.Rep_To_Date = string.Empty;
            if (Rep_From_Date.Enabled) Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString(); 
            else Search_Entity.Rep_Period_FDate = string.Empty;
            if (Rep_To_Date.Enabled) Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString(); 
            else Search_Entity.Rep_Period_TDate = string.Empty;

            Search_Entity.Mst_CaseType_Sw = ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Value.ToString();  //"**"; 

            Search_Entity.Stat_Detail = "N";
            if (Rb_Details_Yes.Checked)
                Search_Entity.Stat_Detail = "Y";

            Search_Entity.Mst_Secret_Sw = "B";
            if(Rb_Mst_Sec.Checked)
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

            if(Search_Entity.CA_MS_Sw=="MS" || Search_Entity.CA_MS_Sw == "CA")
            {
                Search_Entity.CaseMssite = "**";
                if (rdoMsselectsite.Checked)
                    Search_Entity.CaseMssite = Get_Sel_CasemsSites();
                else if (rdomsNosite.Checked)
                    Search_Entity.CaseMssite = "NO";
            }
            //else if (Search_Entity.CA_MS_Sw == "CA")
            //{
            //    Search_Entity.CaseMssite = "**";
            //    if (rdoMsselectsite.Checked)
            //        Search_Entity.CaseMssite = Get_Sel_CaseActSites();
            //    else if (rdomsNosite.Checked)
            //        Search_Entity.CaseMssite = "NO";
            //}

            Search_Entity.Mst_Poverty_Low = Txt_Pov_Low.Text;
            Search_Entity.Mst_Poverty_High = Txt_Pov_High.Text;

            Search_Entity.Hierarchy = Current_Hierarchy + (CmbYear.Visible ? ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Value.ToString() : string.Empty);
            Search_Entity.Activity_Prog = null;
            Sel_Permesures_Programs = "";
            /** if (Cmb_Program.SelectedItem != null)  
             {
                 string Tmp_Hie_On_Porgram = ((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Value.ToString();
                 if (((Captain.Common.Utilities.ListItem)Cmb_Program.SelectedItem).Value.ToString() == "**")
                 {
                     //////Tmp_Hie_On_Porgram = Current_Hierarchy.Substring(0, 4) + "**";

                     //////if(Current_Hierarchy.Substring(2, 2) == "**")
                     //////    Tmp_Hie_On_Porgram = Current_Hierarchy.Substring(0, 2) + "****";

                     //////if(Current_Hierarchy.Substring(0, 2) == "**")
                     //////    Tmp_Hie_On_Porgram = "******";
                     //////Search_Entity.Activity_Prog = Tmp_Hie_On_Porgram;

                     Search_Entity.Activity_Prog = Tmp_Hie_On_Porgram = "******";
                 }

                 Sel_Permesures_Programs = Search_Entity.Activity_Prog = Tmp_Hie_On_Porgram;
             }*/

            string Sel_Programs = string.Empty;
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

        }

        private string Get_Sel_CAMS_List_To_Filter()
        {
            string Sel_Codes = null;

            if (Rb_Process_MS.Checked)
            {
                foreach (MSMASTEntity Entity in Sel_MS_List)
                {
                    Sel_Codes += "'" + Entity.Code + "' ,";
                }

                if (Sel_Codes.Length > 0)
                    Sel_Codes = Sel_Codes.Substring(0, (Sel_Codes.Length - 1));
            }
            else
            {
                foreach (CAMASTEntity Entity in Sel_CA_List)
                {
                    Sel_Codes += "'" + Entity.Code + "' ,";
                }

                if (Sel_Codes.Length > 0)
                    Sel_Codes = Sel_Codes.Substring(0, (Sel_Codes.Length - 1));
            }

            return Sel_Codes;
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

        private string Get_Sel_ZipCodes()
        {
            string Sel_Zip_Codes = string.Empty;
            foreach (ZipCodeEntity Entity in ListZipCode) //ListZipCode)
            {
                Sel_Zip_Codes += "'" + Entity.Zcrzip + (!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty) + "' ,";
            }

            if(Sel_Zip_Codes.Length > 0)
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

        private string Get_Sel_County_Desc()
        {
            string Sel_County_Desc = null;
            foreach (CommonEntity Entity in ListcommonEntity)
            {
                Sel_County_Desc += Entity.Desc + ",";
            }

            if (Sel_County_Desc.Length > 0)
                Sel_County_Desc = Sel_County_Desc.Substring(0, (Sel_County_Desc.Length - 1));

            return Sel_County_Desc;
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

            //if (Sel_Site_Codes.Length > 0)
            //    Sel_Site_Codes = Sel_Site_Codes.Replace("'", "");

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

        /*private string Get_Sel_CaseActSites()
        {
            string Sel_Site_Codes = string.Empty;

            foreach (CaseSiteEntity casesite in ListcaseActSiteEntity) //Site_List)//ListcaseSiteEntity)
            {
                Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
            }

            if (Sel_Site_Codes.Length > 0)
                Sel_Site_Codes = Sel_Site_Codes.Substring(0, (Sel_Site_Codes.Length - 1));

            return Sel_Site_Codes;
        }*/

        private string Get_Sel_Sites_desc()
        {
            string Sel_Site_Desc = string.Empty;

            foreach (CaseSiteEntity casesite in ListcaseSiteEntity) //Site_List)//ListcaseSiteEntity)ListcaseMsSiteEntity
            {
                Sel_Site_Desc += casesite.SiteNUMBER + ",";
            }

            if (Sel_Site_Desc.Length > 0)
                Sel_Site_Desc = Sel_Site_Desc.Substring(0, (Sel_Site_Desc.Length - 1));

            return Sel_Site_Desc;
        }

        private string Get_Sel_MSSites_desc()
        {
            string Sel_Site_Desc = string.Empty;

            foreach (CaseSiteEntity casesite in ListcaseMsSiteEntity) //Site_List)//ListcaseSiteEntity)
            {
                Sel_Site_Desc += casesite.SiteNUMBER + ",";
            }

            if (Sel_Site_Desc.Length > 0)
                Sel_Site_Desc = Sel_Site_Desc.Substring(0, (Sel_Site_Desc.Length - 1));

            return Sel_Site_Desc;
        }

        private string Get_Sel_GroupCodes()
        {
            string Sel_Groups_Codes = string.Empty;
            foreach (Csb14GroupEntity Entity in ListGroupCode)
            {
                Sel_Groups_Codes += "'" + Entity.GrpCode + "' ,";
            }

            if (Sel_Groups_Codes.Length > 0)
                Sel_Groups_Codes = Sel_Groups_Codes.Substring(0, (Sel_Groups_Codes.Length - 1));

            return Sel_Groups_Codes;
        }

        List<Csb14GroupEntity> OutCome_MasterList = new List<Csb14GroupEntity>();
        //private void Prepare_Selected_Group(string Sel_ZipCodes)
        //{
        //    ListGroupCode.Clear();
        //    OutCome_MasterList = _model.SPAdminData.Browse_CSB14Grp(Ref_From_Date.Text.Trim(), Ref_To_Date.Text.Trim(), null, null, null);
        //    foreach (Csb14GroupEntity Entity in OutCome_MasterList)
        //    {
        //        if (Sel_ZipCodes.Contains(Entity.GrpCode.Trim()))
        //            ListGroupCode.Add(new Csb14GroupEntity(Entity));
        //    }
        //}


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
            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            string RefFromDt = string.Empty;
            string RefToDt = string.Empty;
            string RepFromDt = string.Empty;
            string RepToDt = string.Empty;

            //if (RbCummilative.Checked)
            //{
            //    RefFromDt = Ref_From_Date.Checked == true ? "Y" : "N";
            //    RefToDt = Ref_To_Date.Checked == true ? "Y" : "N";
            //}
            //else 
            if (rbRepPeriod.Checked)
            {
                //RefFromDt = Ref_From_Date.Checked == true ? "Y" : "N";
                //RefToDt = Ref_To_Date.Checked == true ? "Y" : "N";
                RepFromDt = Rep_From_Date.Checked == true ? "Y" : "N";
                RepToDt = Rep_To_Date.Checked == true ? "Y" : "N";
            }
            else
            {
                RefFromDt = Ref_From_Date.Checked == true ? "Y" : "N";
                RefToDt = Ref_To_Date.Checked == true ? "Y" : "N";
                RepFromDt = Rep_From_Date.Checked == true ? "Y" : "N";
                RepToDt = Rep_To_Date.Checked == true ? "Y" : "N";
            }

            string Countycounts = "N";
            if (chkbMontCounty.Checked == true)
                Countycounts = "Y";

            string LangCounts = "N";
            if (chkbLangCounts.Checked == true)
                LangCounts = "Y";

            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                                      "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" ACTY_PROGRAM = \"" + Search_Entity.Activity_Prog + "\" FUND_Filter_List = \"" + Search_Entity.CA_Fund_Filter +
                                      "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" CAMS_SW = \"" + Search_Entity.CA_MS_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw + "\" DATE_TYPE = \"" + Search_Entity.DateType +
                                      "\" REP_RANGE = \"" + Search_Entity.RepRange +
                                      "\" REFERENCE_FROM_DATETick = \"" + RefFromDt + "\" REFERENCE_TO_DATETick = \"" + RefToDt +
                                      "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date +
                                      "\" REPORT_FROM_DATETick = \"" + RepFromDt + "\" REPORT_TO_DATETick = \"" + RepToDt +
                                      "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate + "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" MSSITE = \"" + Search_Entity.CaseMssite + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
                                      "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
                                      "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + 
                                      "\" CAMS_Filter_List = \"" + Search_Entity.CAMS_Filter + "\" Languagecounts =\"" + LangCounts + "\" Countycounts =\"" + Countycounts + "\" ALERTCODES = \"" + (rdbAlertCodesAll.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />"); //


            //str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
            //                          "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
            //                          "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
            //                          "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" MSSITE = \"" + Search_Entity.CaseMssite + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
            //                          "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
            //                          "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" ACTY_PROGRAM = \"" + Search_Entity.Activity_Prog +
            //                          "\" CAMS_SW = \"" + Search_Entity.CA_MS_Sw + "\" CAMS_Filter_List = \"" + Search_Entity.CAMS_Filter + "\" FUND_Filter_List = \"" + Search_Entity.CA_Fund_Filter + "\" />"); //
            ////switch (Scr_Oper_Mode)
            ////{
            ////    case "CASB0004":
            ////        str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
            ////                                  "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
            ////                                  "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
            ////                                  "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
            ////                                  "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
            ////                                  "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" />");
            ////        break;

            ////    case "CASB0014":
            ////        str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
            ////                                  "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
            ////                                  "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
            ////                                  "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
            ////                                  "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
            ////                                  "\" COUNTY = \"" + Search_Entity.County + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" />");
            ////        break;
            ////}
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
                    default: Rb_Site_Sel.Checked = true; Txt_Sel_Site.Text = dr["SITE"].ToString();
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

                if(!string.IsNullOrEmpty(dr["REP_RANGE"].ToString().Trim()))
                {
                    if (dr["REP_RANGE"].ToString().Trim() == "Both") rbBoth.Checked = true;
                    else if (dr["REP_RANGE"].ToString().Trim() == "Rep") rbRepPeriod.Checked = true;
                    //else if (dr["REP_RANGE"].ToString().Trim() == "Ref") RbCummilative.Checked = true;
                }


                //Ref_From_Date.Checked = dr["REFERENCE_FROM_DATETick"].ToString() == "Y" ? true : false;
                //Ref_To_Date.Checked = dr["REFERENCE_TO_DATETick"].ToString() == "Y" ? true : false;

                //Rep_From_Date.Checked = dr["REPORT_FROM_DATETick"].ToString() == "Y" ? true : false;
                //Rep_To_Date.Checked = dr["REPORT_TO_DATETick"].ToString() == "Y" ? true : false;


               
               
                if (!string.IsNullOrEmpty(dr["REFERENCE_FROM_DATE"].ToString().Trim()))
                {
                    Ref_From_Date.Value = Convert.ToDateTime(dr["REFERENCE_FROM_DATE"].ToString());
                    Ref_From_Date.Checked = true;
                }
                else
                    Ref_From_Date.Checked = false;

                if (!string.IsNullOrEmpty(dr["REFERENCE_TO_DATE"].ToString().Trim()))
                {
                    Ref_To_Date.Value = Convert.ToDateTime(dr["REFERENCE_TO_DATE"].ToString());
                    Ref_To_Date.Checked = true;
                }
                else
                    Ref_To_Date.Checked = false;

                if (!string.IsNullOrEmpty(dr["REPORT_FROM_DATE"].ToString().Trim()))
                {
                    Rep_From_Date.Value = Convert.ToDateTime(dr["REPORT_FROM_DATE"].ToString());
                    Rep_From_Date.Checked = true;
                }
                else Rep_From_Date.Checked = false;
                if (!string.IsNullOrEmpty(dr["REPORT_TO_DATE"].ToString().Trim()))
                {
                    Rep_To_Date.Value = Convert.ToDateTime(dr["REPORT_TO_DATE"].ToString());
                    Rep_To_Date.Checked = true;
                }
                else Rep_To_Date.Checked = false;

                if (dr["DATE_SELECTION"].ToString() == "MSDATE")
                    Rb_MS_Date.Checked = true;
                else
                    Rb_MS_AddDate.Checked = true;

                if (Tmp_Table.Columns.Contains("DATE_TYPE"))
                {
                    if (dr["DATE_TYPE"].ToString() == "R")
                        rbRecent.Checked = true;
                    else
                        rbOldest.Checked = true;
                }

                if ((dr["ATTRIBUTES"].ToString() == "SYSTEM" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["ATTRIBUTES"].ToString() == "All" && Scr_Oper_Mode == "CASB0014"))
                    Rb_Agy_Def.Checked = true;
                else
                    Rb_User_Def.Checked = true;

                if (dr["STAT_DETAILS"].ToString() == "Y")
                    Rb_Details_Yes.Checked = true;
                else
                    Rb_Details_No.Checked = true;

                Txt_Pov_Low.Text = dr["POVERTY_LOW"].ToString();
                Txt_Pov_High.Text = dr["POVERTY_HIGH"].ToString();

                if (dr["ZIPCODE"].ToString() == "**")
                    Rb_Zip_All.Checked = true;
                else
                {
                    Rb_Zip_Sel.Checked = true;
                    Fill_Zipcode_Selected_List(dr["ZIPCODE"].ToString());
                    //if(Scr_Oper_Mode == "CASB0014")
                    //    Prepare_Selected_Group(dr["ZIPCODE"].ToString() );
                }


                if ((dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "CASB0014"))
                    Rb_County_All.Checked = true;
                else
                {
                    Rb_County_Sel.Checked = true;
                    Fill_County_Selected_List(dr["COUNTY"].ToString().Trim());
                }


                if ((dr["COUNTY"].ToString() == "PM" && Scr_Oper_Mode == "CASB0014"))
                    Rb_OBO_Mem.Checked = true;
                else if ((dr["COUNTY"].ToString() == "Both" && Scr_Oper_Mode == "CASB0014"))
                {
                    Rb_SNP_Mem.Checked = true;
                }

                //if ((dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "CASB0004") ||
                //    (dr["COUNTY"].ToString() == "PM" && Scr_Oper_Mode == "CASB0014"))
                //    Rb_County_All.Checked = true;
                //else
                //{
                //    Rb_County_Sel.Checked = true;
                //    Fill_County_Selected_List(dr["COUNTY"].ToString().Trim());
                //}

                if (Scr_Oper_Mode == "RNGB0004")
                {
                    if (dr["DG_COUNT"].ToString() == "SNP")
                        Rb_SNP_Mem.Checked = true;
                    else
                        Rb_OBO_Mem.Checked = true;
                }
                else
                {
                    if (dr["DG_COUNT"].ToString() == "PM")
                        Rb_OBO_Mem.Checked = true;
                    else
                        Rb_SNP_Mem.Checked = true;
                }

                switch (dr["SECRET_APP"].ToString())
                {
                    case "Y": Rb_Mst_Sec.Checked = true; break;
                    case "N": Rb_Mst_NonSec.Checked = true; break;
                    default: Rb_Mst_BothSec.Checked = true; break;
                }

                //if (Scr_Oper_Mode == "CASB0014")
                //{
                //**    Fill_Program_Combo();
                 //**   SetComboBoxValue(Cmb_Program, dr["ACTY_PROGRAM"].ToString().Trim() == "******" ? "**" : dr["ACTY_PROGRAM"].ToString());
                //}

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

                if (Tmp_Table.Columns.Contains("Languagecounts"))
                {
                    if (dr["Languagecounts"].ToString() == "Y")
                        chkbLangCounts.Checked = true;
                    else
                        chkbLangCounts.Checked = false;
                }

                if (Tmp_Table.Columns.Contains("Countycounts"))
                {
                    if (dr["Countycounts"].ToString() == "Y")
                        chkbMontCounty.Checked = true;
                    else
                        chkbMontCounty.Checked = false;
                }

                if (columns.Contains("ALERTCODES"))
                {
                    if (dr["ALERTCODES"].ToString() == "A")
                        rdbAlertCodesAll.Checked = true;
                    else if (dr["ALERTCODES"].ToString() == "S")
                        rdbSelectedAlertCodes.Checked = true;
                }
                else
                    rdbAlertCodesAll.Checked = true;

                if (columns.Contains("SELECTEDALERTCODES"))
                {
                    if (dr["SELECTEDALERTCODES"].ToString() != "")
                    {
                        Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                    }
                }


                if (Scr_Oper_Mode == "RNGB0004")
                {
                    if(dr["CAMS_SW"].ToString() == "CA")
                    {
                        Rb_Process_CA.Checked = true;
                        Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;
                        if (dr["CAMS_Filter_List"].ToString() != "**")
                        {
                            Btn_CA_Selection.Text = "&All";
                            Btn_CA_Selection.Visible = true;
                            Btn_MS_Selection.Visible = false;
                            GetParam_CA_Filter_List(dr["CAMS_Filter_List"].ToString());
                            if (Sel_CA_List.Count > 0)
                            {
                                All_CAMS_Selected = false;
                                Btn_CA_Selection.Text = "Se&l";
                            }
                        }

                        //lblMssite.Enabled = panel7.Enabled = true;
                        rdoMssiteall.Enabled = true; rdoMsselectsite.Enabled = true; rdomsNosite.Enabled = true;
                        txt_Msselect_site.Clear();
                        switch (dr["MSSITE"].ToString())
                        {
                            case "**": rdoMssiteall.Checked = true; break;
                            case "NO": rdomsNosite.Checked = true; break;
                            default:
                                rdoMsselectsite.Checked = true; txt_Msselect_site.Text = dr["MSSITE"].ToString();
                                CASite_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
                                ListcaseMsSiteEntity.Clear();
                                foreach (CaseSiteEntity casesite in CASite_List) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (txt_Msselect_site.Text.Contains(casesite.SiteNUMBER))
                                        ListcaseMsSiteEntity.Add(casesite);
                                    // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                }


                                break;
                        }

                    }
                    else if (dr["CAMS_SW"].ToString() == "MS")
                    {
                        Rb_Process_MS.Checked = true;
                        Btn_MS_Selection.Text = "&All";
                        Btn_MS_Selection.Visible = true;
                        Btn_CA_Selection.Visible = false;
                        Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;

                        if (dr["CAMS_Filter_List"].ToString() != "**")
                        {
                            GetParam_MS_Filter_List(dr["CAMS_Filter_List"].ToString());
                            if (Sel_MS_List.Count > 0)
                            {
                                All_CAMS_Selected = false;
                                Btn_MS_Selection.Text = "Se&l";
                            }
                        }
                        //lblMssite.Enabled = panel7.Enabled = true;
                        rdoMssiteall.Enabled = true; rdoMsselectsite.Enabled = true; rdomsNosite.Enabled = true;
                        txt_Msselect_site.Clear();
                        switch (dr["MSSITE"].ToString())
                        {
                            case "**": rdoMssiteall.Checked = true; break;
                            case "NO": rdomsNosite.Checked = true; break;
                            default:
                                rdoMsselectsite.Checked = true; txt_Msselect_site.Text = dr["MSSITE"].ToString();
                                MSSite_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
                                ListcaseMsSiteEntity.Clear();
                                foreach (CaseSiteEntity casesite in MSSite_List) //Site_List)//ListcaseSiteEntity)
                                {
                                    if (txt_Msselect_site.Text.Contains(casesite.SiteNUMBER))
                                        ListcaseMsSiteEntity.Add(casesite);
                                    // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                }


                                break;
                        }
                    }
                    else
                    {
                        All_CAMS_Selected = Btn_CA_Selection.Visible = false;
                        Btn_MS_Selection.Visible = false;
                        Btn_CA_Selection.Text = "&All";
                        All_CAMS_Selected = true;
                        Rb_Process_Both.Checked = true;
                        //Rb_Fund_All.Checked = true;
                        Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;

                        //lblMssite.Enabled = panel7.Enabled = false;
                        rdoMssiteall.Enabled = false; rdoMsselectsite.Enabled = false; rdomsNosite.Enabled = false;
                        rdoMssiteall.Checked = true;

                    }

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

        private void Fill_Zipcode_Selected_List(string Zip_Str)
        {
            foreach (ZipCodeEntity Ent in zipcode_List)
            {
                string Zip = Ent.Zcrzip + Ent.Zcrplus4;
                if (Zip_Str.Contains("'" + Zip + "'"))
                    ListZipCode.Add(Ent); 
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


//****************************************************************************************************

        //private void Dynamic_RDLC()
        //{

        //    Get_Report_Selection_Parameters();

        //    XmlNode xmlnode;

        //    XmlDocument xml = new XmlDocument();
        //    xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
        //    xml.AppendChild(xmlnode);

        //    XmlElement Report = xml.CreateElement("Report");
        //    Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
        //    Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
        //    xml.AppendChild(Report);

        //    XmlElement DataSources = xml.CreateElement("DataSources");
        //    XmlElement DataSource = xml.CreateElement("DataSource");
        //    DataSource.SetAttribute("Name", "CaptainDataSource");
        //    DataSources.AppendChild(DataSource);

        //    Report.AppendChild(DataSources);

        //    XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
        //    DataSource.AppendChild(ConnectionProperties);

        //    XmlElement DataProvider = xml.CreateElement("DataProvider");
        //    DataProvider.InnerText = "System.Data.DataSet";


        //    XmlElement ConnectString = xml.CreateElement("ConnectString");
        //    ConnectString.InnerText = "/* Local Connection */";
        //    ConnectionProperties.AppendChild(DataProvider);
        //    ConnectionProperties.AppendChild(ConnectString);

        //    //string SourceID = "rd:DataSourceID";
        //    //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
        //    //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
        //    //DataSource.AppendChild(DataSourceID);

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

        //    XmlElement DataSets = xml.CreateElement("DataSets");
        //    Report.AppendChild(DataSets);

        //    XmlElement DataSet = xml.CreateElement("DataSet");
        //    DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
        //    DataSets.AppendChild(DataSet);

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

        //    XmlElement Fields = xml.CreateElement("Fields");
        //    DataSet.AppendChild(Fields);

        //    foreach (DG_ResTab_Entity Entity in DG_Table_List)
        //    {
        //        XmlElement Field = xml.CreateElement("Field");
        //        Field.SetAttribute("Name", Entity.Column_Name);
        //        Fields.AppendChild(Field);

        //        XmlElement DataField = xml.CreateElement("DataField");
        //        DataField.InnerText = Entity.Column_Name;
        //        Field.AppendChild(DataField);
        //    }

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

        //    XmlElement Query = xml.CreateElement("Query");
        //    DataSet.AppendChild(Query);

        //    XmlElement DataSourceName = xml.CreateElement("DataSourceName");
        //    DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
        //    Query.AppendChild(DataSourceName);

        //    XmlElement CommandText = xml.CreateElement("CommandText");
        //    CommandText.InnerText = "/* Local Query */";
        //    Query.AppendChild(CommandText);


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
        //    //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    XmlElement Body = xml.CreateElement("Body");
        //    Report.AppendChild(Body);


        //    XmlElement ReportItems = xml.CreateElement("ReportItems");
        //    Body.AppendChild(ReportItems);

        //    XmlElement Height = xml.CreateElement("Height");
        //    //Height.InnerText = "4.15625in";       // Landscape
        //    Height.InnerText = "2in";           // Portrait
        //    Body.AppendChild(Height);


        //    XmlElement Style = xml.CreateElement("Style");
        //    Body.AppendChild(Style);

        //    XmlElement Border = xml.CreateElement("Border");
        //    Style.AppendChild(Border);

        //    XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
        //    BackgroundColor.InnerText = "White";
        //    Style.AppendChild(BackgroundColor);


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

        //    XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
        //    Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
        //    ReportItems.AppendChild(Sel_Rectangle);

        //    XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
        //    Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


        //    double Total_Sel_TextBox_Height = 0.16667;
        //    string Tmp_Sel_Text = string.Empty;
        //    for (int i = 0; i < 58; i++) 
        //    {
        //        XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
        //        Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
        //        Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

        //        XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
        //        Textbox1_Cangrow.InnerText = "true";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

        //        XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
        //        Textbox1_Keep.InnerText = "true";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

        //        XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

        //        XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
        //        Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

        //        XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
        //        Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


        //        XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
        //        Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

        //        XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

        //        Tmp_Sel_Text = string.Empty;
        //        switch (i)
        //        {
        //            case 1: Tmp_Sel_Text = "Selected Report Parameters"; break;

        //            case 4: Tmp_Sel_Text =  "           Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

        //            case 6: Tmp_Sel_Text = "            Attributes"; break;
        //            case 7: Tmp_Sel_Text = " : " + (Rb_Agy_Def.Checked ? "Agency Defined" : "User Defined Associations"); break;
        //            case 8: Tmp_Sel_Text =  "            Case Type"; break;
        //            case 9: Tmp_Sel_Text = " : " + ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString(); break;
        //            case 10: Tmp_Sel_Text = "            Case Status" ; break;
        //            case 11: Tmp_Sel_Text = " : " + Sel_params_To_Print[1]; break;
        //            case 12: Tmp_Sel_Text = "            Date Selection" ; break;
        //            case 13: Tmp_Sel_Text = " : " + (Rb_Process_MS.Checked ? "Outcomes" : "Services") + "  -  " + (Rb_MS_AddDate.Checked ? "ADD Date" : "Posting Date"); break;

        //            case 14: Tmp_Sel_Text = "            Reference Period Date"; break;
        //            case 15: Tmp_Sel_Text = " : From: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_From_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
        //                                    + "    To: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_To_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                break;
        //            case 16: Tmp_Sel_Text = "            Report Period Date"; break;
        //            case 17: Tmp_Sel_Text = " : From: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_FDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
        //                                    + "    To: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_TDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                break;


        //            case 18: Tmp_Sel_Text = "            Poverty Levels"; break;
        //            case 19: Tmp_Sel_Text = " : From: " + Txt_Pov_Low.Text + "    To: " + Txt_Pov_High.Text; break;

        //            case 20: Tmp_Sel_Text = "            Site"; break;
        //            case 21: Tmp_Sel_Text = " : " + Sel_params_To_Print[2]; break;
        //            case 22: Tmp_Sel_Text = "            Zip Code"; break;
        //            case 23: Tmp_Sel_Text = " : " + (Rb_Zip_All.Checked ? "All Zip Codes" : "Selected Zip Code"); break;
        //            case 24: Tmp_Sel_Text = "            County"; break;
        //            case 25: Tmp_Sel_Text = " : " + (Rb_County_All.Checked ? "All Counties" : "Selected County"); break;

        //            case 26: Tmp_Sel_Text = (Rb_Process_MS.Checked ? "            Demographic Count" : " "); break;
        //            case 27: Tmp_Sel_Text = (Rb_Process_MS.Checked ? " : " + Sel_params_To_Print[3] : " "); break;

        //            case 28: Tmp_Sel_Text = "            Secret Applications"; break;
        //            case 29: Tmp_Sel_Text = " : " + Sel_params_To_Print[0]; break;
        //            case 30: Tmp_Sel_Text = "            Produce Stastical Details"; break;
        //            case 31: Tmp_Sel_Text = " : " + (Rb_Details_Yes.Checked ? "Yes" : "No"); break;
                    
        //            default: Tmp_Sel_Text = "  "; break;
        //        }


        //        Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
        //        Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


        //        XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
        //        Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

        //        XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
        //        Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
        //        Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


        //        XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
        //        Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


        //        XmlElement Textbox1_Top = xml.CreateElement("Top");
        //        Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

        //        //XmlElement Textbox1_Left = xml.CreateElement("Left");
        //        //Textbox1_Left.InnerText = "0.07292in";
        //        //Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

        //        //Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

        //        XmlElement Textbox1_Left = xml.CreateElement("Left");
        //        //Textbox1_Left.InnerText = "0.07292in";
        //        Textbox1_Left.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

        //        if (i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
        //        {
        //            if (i % 2 != 0)
        //                Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
        //        }
        //        else
        //            Total_Sel_TextBox_Height += 0.21855;


        //        XmlElement Textbox1_Height = xml.CreateElement("Height");
        //        Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

        //        //XmlElement Textbox1_Width = xml.CreateElement("Width");
        //        ////Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
        //        //Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
        //        //Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

        //        XmlElement Textbox1_Width = xml.CreateElement("Width");
        //        //Textbox1_Width.InnerText = "7.48777";
        //        Textbox1_Width.InnerText = ((i > 4 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Width);


        //        XmlElement Textbox1_Style = xml.CreateElement("Style");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

        //        XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
        //        Textbox1_Style.AppendChild(Textbox1_Style_Border);

        //        XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
        //        Textbox1_Style_Border_Style.InnerText = "None";
        //        Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

        //        XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
        //        Textbox1_Style_PaddingLeft.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

        //        XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
        //        Textbox1_Style_PaddingRight.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

        //        XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
        //        Textbox1_Style_PaddingTop.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

        //        XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
        //        Textbox1_Style_PaddingTop.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

        //    }

        //    XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //    Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //    XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
        //    Break_After_SelParamRectangle_Location.InnerText = "End";
        //    Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

        //    XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
        //    Sel_Rectangle_KeepTogether.InnerText = "true";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

        //    XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
        //    Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

        //    XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
        //    Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

        //    XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
        //    Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

        //    XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
        //    //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    Sel_Rectangle_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

        //    XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
        //    Sel_Rectangle_ZIndex.InnerText = "1";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

        //    XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

        //    XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
        //    Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

        //    XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
        //    Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
        //    Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



        //    XmlElement Tablix = xml.CreateElement("Tablix");
        //    Tablix.SetAttribute("Name", "Tablix3");
        //    ReportItems.AppendChild(Tablix);

        //    XmlElement TablixBody = xml.CreateElement("TablixBody");
        //    Tablix.AppendChild(TablixBody);


        //    XmlElement TablixColumns = xml.CreateElement("TablixColumns");
        //    TablixBody.AppendChild(TablixColumns);

        //    foreach (DG_ResTab_Entity Entity in DG_Table_List)                      // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   
        //        {
        //            XmlElement TablixColumn = xml.CreateElement("TablixColumn");
        //            TablixColumns.AppendChild(TablixColumn);

        //            XmlElement Col_Width = xml.CreateElement("Width");
        //            //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
        //            //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
        //            Col_Width.InnerText = Entity.Disp_Width;
        //            TablixColumn.AppendChild(Col_Width);
        //        }
        //    }

        //    XmlElement TablixRows = xml.CreateElement("TablixRows");
        //    TablixBody.AppendChild(TablixRows);

        //    XmlElement TablixRow = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow);

        //    XmlElement Row_Height = xml.CreateElement("Height");
        //    Row_Height.InnerText = "0.25in";
        //    TablixRow.AppendChild(Row_Height);

        //    XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
        //    TablixRow.AppendChild(Row_TablixCells);


        //    int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
        //    string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
        //    foreach (DG_ResTab_Entity Entity in DG_Table_List)            // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            //Entity.Column_Name;
        //            Tmp_Loop_Cnt++;

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells.AppendChild(TablixCell);


        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            //if (Entity.Col_Format_Type == "C")
        //            //    Field_type = "Checkbox";

        //            XmlElement Textbox = xml.CreateElement(Field_type);
        //            Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);



        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);



        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");

        //            Tmp_Disp_Column_Name = Entity.Disp_Name;


        //            //Disp_Col_Substring_Len = 6;

        //            //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
        //            Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
        //            TextRun.AppendChild(Return_Value);


        //            XmlElement Cell_Align = xml.CreateElement("Style");
        //            XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
        //            Cell_TextAlign.InnerText = "Center";
        //            Cell_Align.AppendChild(Cell_TextAlign);
        //            Paragraph.AppendChild(Cell_Align);


        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);

        //            XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //            Return_Style_FontWeight.InnerText = "Bold";
        //            Return_Style.AppendChild(Return_Style_FontWeight);


        //            //XmlElement Return_AlignStyle = xml.CreateElement("Style");
        //            //Paragraph.AppendChild(Return_AlignStyle);

        //            //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
        //            //DefaultName.InnerText = "Textbox" + i.ToString();
        //            //Textbox.AppendChild(DefaultName);


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);


        //            XmlElement Cell_Border = xml.CreateElement("Border");
        //            Cell_style.AppendChild(Cell_Border);

        //            XmlElement Border_Color = xml.CreateElement("Color");
        //            Border_Color.InnerText = "Black";//"LightGrey";
        //            Cell_Border.AppendChild(Border_Color);

        //            XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
        //            Border_Style.InnerText = "Solid";
        //            Cell_Border.AppendChild(Border_Style);

        //            XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            Cell_Style_BackColor.InnerText = "LightSteelBlue";
        //            Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);
        //        }
        //    }




        //    XmlElement TablixRow2 = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow2);

        //    XmlElement Row_Height2 = xml.CreateElement("Height");
        //    Row_Height2.InnerText = "0.2in";
        //    TablixRow2.AppendChild(Row_Height2);

        //    XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
        //    TablixRow2.AppendChild(Row_TablixCells2);

        //    string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
        //    char Tmp_Double_Codes = '"';
        //    foreach (DG_ResTab_Entity Entity in DG_Table_List)        // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells2.AppendChild(TablixCell);

        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            XmlElement Textbox = xml.CreateElement("Textbox");
        //            Textbox.SetAttribute("Name", Entity.Column_Name);
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);

        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);

        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");


        //            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
        //            Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
        //            Text_Align = "Left";
        //            switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
        //            {
        //                case "R":
        //                    Text_Align = "Right"; break;
        //            }

        //            Return_Value.InnerText = Field_Value;
        //            TextRun.AppendChild(Return_Value);

        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);


        //            if (Entity.Column_Name == "Sum_Child_Desc" || 
        //                Entity.Column_Name == "Sum_Child_Period_Count" ||
        //                Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
        //            {
        //                XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //                Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes +   "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
        //                Return_Style.AppendChild(Return_Style_FontWeight);
        //            }

        //            if (!string.IsNullOrEmpty(Text_Align))
        //            {
        //                XmlElement Cell_Align = xml.CreateElement("Style");
        //                XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
        //                Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes +  "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
        //                //Cell_TextAlign.InnerText = Text_Align;
        //                Cell_Align.AppendChild(Cell_TextAlign);
        //                Paragraph.AppendChild(Cell_Align);
        //            }


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);

        //            XmlElement Cell_Border = xml.CreateElement("Border");
        //            Cell_style.AppendChild(Cell_Border);

        //            XmlElement Border_Color = xml.CreateElement("Color");
        //            Border_Color.InnerText = "LightGrey";
        //            Cell_Border.AppendChild(Border_Color);

        //            XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
        //            Border_Style.InnerText = "None";
        //            Cell_Border.AppendChild(Border_Style);


        //            XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
        //            //Cell_Style_BackColor.InnerText = "Blue";
        //            Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);
        //        }
        //    }



        //    XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
        //    Tablix.AppendChild(TablixColumnHierarchy);

        //    XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
        //    TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

        //    for (int Loop = 0; Loop < 3; Loop++)            // Dynamic based on Display Columns in 3/6 
        //    {
        //        XmlElement TablixMember = xml.CreateElement("TablixMember");
        //        Tablix_Col_Members.AppendChild(TablixMember);
        //    }


        //    XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
        //    Tablix.AppendChild(TablixRowHierarchy);

        //    XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
        //    TablixRowHierarchy.AppendChild(Tablix_Row_Members);

        //    XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member);

        //    XmlElement FixedData = xml.CreateElement("FixedData");
        //    FixedData.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(FixedData);

        //    XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
        //    KeepWithGroup.InnerText = "After";
        //    Tablix_Row_Member.AppendChild(KeepWithGroup);

        //    XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
        //    RepeatOnNewPage.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(RepeatOnNewPage);

        //    XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

        //    XmlElement Group = xml.CreateElement("Group"); // 5656565656
        //    Group.SetAttribute("Name", "Details1");
        //    Tablix_Row_Member1.AppendChild(Group);


        //    XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
        //    RepeatRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(RepeatRowHeaders);

        //    XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
        //    FixedRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(FixedRowHeaders);

        //    XmlElement DataSetName1 = xml.CreateElement("DataSetName");
        //    DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
        //    Tablix.AppendChild(DataSetName1);

        //    XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
        //    Tablix.AppendChild(SubReport_PageBreak);

        //    XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
        //    SubReport_PageBreak_Location.InnerText = "End";
        //    SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

        //    XmlElement SortExpressions = xml.CreateElement("SortExpressions");
        //    Tablix.AppendChild(SortExpressions);

        //    XmlElement SortExpression = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression);

        //    XmlElement SortExpression_Value = xml.CreateElement("Value");
        //    //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
        //    SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

        //    SortExpression.AppendChild(SortExpression_Value);

        //    XmlElement SortExpression_Direction = xml.CreateElement("Direction");
        //    SortExpression_Direction.InnerText = "Descending";
        //    SortExpression.AppendChild(SortExpression_Direction);


        //    XmlElement SortExpression1 = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression1);

        //    XmlElement SortExpression_Value1 = xml.CreateElement("Value");
        //    //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
        //    SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
        //    SortExpression1.AppendChild(SortExpression_Value1);


        //    XmlElement Top = xml.CreateElement("Top");
        //    Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
        //    //Top.InnerText = "0.60417in";
        //    Tablix.AppendChild(Top);

        //    XmlElement Left = xml.CreateElement("Left");
        //    Left.InnerText = "0.20417in";
        //    //Left.InnerText = "0.60417in";
        //    Tablix.AppendChild(Left);

        //    XmlElement Height1 = xml.CreateElement("Height");
        //    Height1.InnerText = "0.5in";
        //    Tablix.AppendChild(Height1);

        //    XmlElement Width1 = xml.CreateElement("Width");
        //    Width1.InnerText = "5.3229in";
        //    Tablix.AppendChild(Width1);


        //    XmlElement Style10 = xml.CreateElement("Style");
        //    Tablix.AppendChild(Style10);

        //    XmlElement Style10_Border = xml.CreateElement("Border");
        //    Style10.AppendChild(Style10_Border);

        //    XmlElement Style10_Border_Style = xml.CreateElement("Style");
        //    Style10_Border_Style.InnerText = "None";
        //    Style10_Border.AppendChild(Style10_Border_Style);


        //    //   Subreport
        //    ////////if (Summary_Sw)
        //    ////////{
        //    ////////    // Summary Sub Report 
        //    ////////}

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

        //    XmlElement Width = xml.CreateElement("Width");               // Total Page Width
        //    Width.InnerText = "6.5in";      //Common
        //    //if(Rb_A4_Port.Checked)
        //    //    Width.InnerText = "8.27in";      //Portrait "A4"
        //    //else
        //    //    Width.InnerText = "11in";      //Landscape "A4"
        //    Report.AppendChild(Width);


        //    XmlElement Page = xml.CreateElement("Page");
        //    Report.AppendChild(Page);

        //    //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

        //    if (Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
        //    {
        //        XmlElement PageHeader = xml.CreateElement("PageHeader");
        //        Page.AppendChild(PageHeader);

        //        XmlElement PageHeader_Height = xml.CreateElement("Height");
        //        PageHeader_Height.InnerText = "0.51958in";
        //        PageHeader.AppendChild(PageHeader_Height);

        //        XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        PrintOnFirstPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnFirstPage);

        //        XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        PrintOnLastPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnLastPage);


        //        XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
        //        PageHeader.AppendChild(Header_ReportItems);

        //        if (Include_Header_Title)
        //        {
        //            XmlElement Header_TextBox = xml.CreateElement("Textbox");
        //            Header_TextBox.SetAttribute("Name", "HeaderTextBox");
        //            Header_ReportItems.AppendChild(Header_TextBox);

        //            XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            HeaderTextBox_CanGrow.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

        //            XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
        //            HeaderTextBox_Keep.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_Keep);

        //            XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
        //            Header_TextBox.AppendChild(Header_Paragraphs);

        //            XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
        //            Header_Paragraphs.AppendChild(Header_Paragraph);

        //            XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
        //            Header_Paragraph.AppendChild(Header_TextRuns);

        //            XmlElement Header_TextRun = xml.CreateElement("TextRun");
        //            Header_TextRuns.AppendChild(Header_TextRun);

        //            XmlElement Header_TextRun_Value = xml.CreateElement("Value");
        //            Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
        //            Header_TextRun.AppendChild(Header_TextRun_Value);

        //            XmlElement Header_TextRun_Style = xml.CreateElement("Style");
        //            Header_TextRun.AppendChild(Header_TextRun_Style);

        //            XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
        //            Header_Style_Font.InnerText = "Times New Roman";
        //            Header_TextRun_Style.AppendChild(Header_Style_Font);

        //            XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
        //            Header_Style_FontSize.InnerText = "16pt";
        //            Header_TextRun_Style.AppendChild(Header_Style_FontSize);

        //            XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
        //            Header_Style_TextDecoration.InnerText = "Underline";
        //            Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

        //            XmlElement Header_Style_Color = xml.CreateElement("Color");
        //            Header_Style_Color.InnerText = "#104cda";
        //            Header_TextRun_Style.AppendChild(Header_Style_Color);

        //            XmlElement Header_TextBox_Top = xml.CreateElement("Top");
        //            Header_TextBox_Top.InnerText = "0.24792in";
        //            Header_TextBox.AppendChild(Header_TextBox_Top);

        //            XmlElement Header_TextBox_Left = xml.CreateElement("Left");
        //            Header_TextBox_Left.InnerText = "1.42361in";
        //            Header_TextBox.AppendChild(Header_TextBox_Left);

        //            XmlElement Header_TextBox_Height = xml.CreateElement("Height");
        //            Header_TextBox_Height.InnerText = "0.30208in";
        //            Header_TextBox.AppendChild(Header_TextBox_Height);

        //            XmlElement Header_TextBox_Width = xml.CreateElement("Width");
        //            Header_TextBox_Width.InnerText = "5.30208in";
        //            Header_TextBox.AppendChild(Header_TextBox_Width);

        //            XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
        //            Header_TextBox_ZIndex.InnerText = "1";
        //            Header_TextBox.AppendChild(Header_TextBox_ZIndex);


        //            XmlElement Header_TextBox_Style = xml.CreateElement("Style");
        //            Header_TextBox.AppendChild(Header_TextBox_Style);

        //            XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

        //            XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Header_TB_StyleBorderStyle.InnerText = "None";
        //            Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

        //            XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Header_TB_SBS_LeftPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

        //            XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Header_TB_SBS_RightPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

        //            XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Header_TB_SBS_TopPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

        //            XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Header_TB_SBS_BotPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

        //            XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
        //            Header_Paragraph.AppendChild(Header_Text_Align_Style);

        //            XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            Header_Text_Align.InnerText = "Center";
        //            Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }

        //        //if (Include_Header_Image)
        //        //{
        //        //    // Add Image Heare
        //        //}

        //        XmlElement PageHeader_Style = xml.CreateElement("Style");
        //        PageHeader.AppendChild(PageHeader_Style);

        //        XmlElement PageHeader_Border = xml.CreateElement("Border");
        //        PageHeader_Style.AppendChild(PageHeader_Border);

        //        XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
        //        PageHeader_Border_Style.InnerText = "None";
        //        PageHeader_Border.AppendChild(PageHeader_Border_Style);


        //        XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
        //        PageHeader_BackgroundColor.InnerText = "White";
        //        PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



        //    //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    if (Include_Footer)
        //    {
        //        XmlElement PageFooter = xml.CreateElement("PageFooter");
        //        Page.AppendChild(PageFooter);

        //        XmlElement PageFooter_Height = xml.CreateElement("Height");
        //        PageFooter_Height.InnerText = "0.35083in";
        //        PageFooter.AppendChild(PageFooter_Height);

        //        XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        Footer_PrintOnFirstPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnFirstPage);

        //        XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        Footer_PrintOnLastPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnLastPage);

        //        XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
        //        PageFooter.AppendChild(Footer_ReportItems);

        //        if (Include_Footer_PageCnt)
        //        {
        //            XmlElement Footer_TextBox = xml.CreateElement("Textbox");
        //            Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
        //            Footer_ReportItems.AppendChild(Footer_TextBox);

        //            XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            FooterTextBox_CanGrow.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

        //            XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
        //            FooterTextBox_Keep.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_Keep);

        //            XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
        //            Footer_TextBox.AppendChild(Footer_Paragraphs);

        //            XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
        //            Footer_Paragraphs.AppendChild(Footer_Paragraph);

        //            XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
        //            Footer_Paragraph.AppendChild(Footer_TextRuns);

        //            XmlElement Footer_TextRun = xml.CreateElement("TextRun");
        //            Footer_TextRuns.AppendChild(Footer_TextRun);

        //            XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
        //            Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
        //            Footer_TextRun.AppendChild(Footer_TextRun_Value);

        //            XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
        //            Footer_TextRun.AppendChild(Footer_TextRun_Style);

        //            XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
        //            Footer_TextBox_Top.InnerText = "0.06944in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Top);

        //            XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
        //            Footer_TextBox_Height.InnerText = "0.25in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Height);

        //            XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
        //            Footer_TextBox_Width.InnerText = "1.65625in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Width);


        //            XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
        //            Footer_TextBox.AppendChild(Footer_TextBox_Style);

        //            XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

        //            XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Footer_TB_StyleBorderStyle.InnerText = "None";
        //            Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

        //            XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Footer_TB_SBS_LeftPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

        //            XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Footer_TB_SBS_RightPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

        //            XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Footer_TB_SBS_TopPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

        //            XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Footer_TB_SBS_BotPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

        //            XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
        //            Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

        //            //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            //Header_Text_Align.InnerText = "Center";
        //            //Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
        //    XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

        //    //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //    //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    if (true) //(Rb_A4_Port.Checked)
        //    {
        //        Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
        //        Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
        //    }
        //    else
        //    {
        //        Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //        Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    }
        //    Page.AppendChild(Page_PageHeight);
        //    Page.AppendChild(Page_PageWidth);


        //    XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
        //    Page_LeftMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_LeftMargin);

        //    XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
        //    Page_RightMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_RightMargin);

        //    XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
        //    Page_TopMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_TopMargin);

        //    XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
        //    Page_BottomMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_BottomMargin);



        //    //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

        //    //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
        //    //EmbeddedImages.InnerText = "Image Attributes";
        //    //Report.AppendChild(EmbeddedImages);

        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


        //    string s = xml.OuterXml;

        //    try //Generate RDLC
        //    {
        //        //xml.Save(@"C:\Capreports\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //        xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
        //        //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //    }
        //    catch (Exception ex)
        //    {   Console.WriteLine(ex.Message);  }

        //    //Console.ReadLine();   //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        //}

        //****************************************************************************************************




        //***************    Bypass RDLC     ********************************

        private void Bypass_Dynamic_RDLC()
        {
            string RDLC_For = "BYPS";
            Get_Report_Selection_Parameters();
            Get_DG_Bypass_Table_Structure();
            


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

            switch (RDLC_For)
            {
                case "BYPS":
                    foreach (DG_Bypass_Entity Entity in DG_Bypass_List)
                    {
                        XmlElement Field = xml.CreateElement("Field");
                        Field.SetAttribute("Name", Entity.Column_Name);
                        Fields.AppendChild(Field);

                        XmlElement DataField = xml.CreateElement("DataField");
                        DataField.InnerText = Entity.Column_Name;
                        Field.AppendChild(DataField);
                    }
                    break;
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

            double Total_Sel_TextBox_Height = 0.16667;
            //string Tmp_Sel_Text = string.Empty;
            //for (int i = 0; i < 22; i++)
            //{
            //    XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
            //    Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
            //    Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

            //    XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
            //    Textbox1_Cangrow.InnerText = "true";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

            //    XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
            //    Textbox1_Keep.InnerText = "true";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

            //    XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

            //    XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
            //    Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

            //    XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
            //    Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


            //    XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
            //    Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

            //    XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

            //    Tmp_Sel_Text = string.Empty;
            //    switch (i)
            //    {
            //        case 0: Tmp_Sel_Text = "Selected Report Parameters"; break;

            //        case 3: Tmp_Sel_Text = "      Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

            //        case 5: Tmp_Sel_Text = "            Attributes                         :   " + (Rb_Agy_Def.Checked ? "Agency Defined" : "User Defined Associations"); break;
            //        case 6: Tmp_Sel_Text = "            Case Type                       :   " + ((ListItem)Cmb_CaseType.SelectedItem).Text.ToString(); break;
            //        case 7: Tmp_Sel_Text = "            Case Status                     :   " + Sel_params_To_Print[1]; break;

            //        case 9: Tmp_Sel_Text = "            Date Selection                 :   " + (Rb_MS_AddDate.Checked ? "MS AddDate" : "MS Date"); break;
            //        case 10: Tmp_Sel_Text = "            Reference Period Date    :   From " + Search_Entity.Rep_From_Date + "    To " + Search_Entity.Rep_To_Date; break;
            //        case 11: Tmp_Sel_Text = "            Report Period Date          :   From " + Search_Entity.Rep_Period_FDate + "    To " + Search_Entity.Rep_Period_TDate; break;
            //        case 12: Tmp_Sel_Text = "            Poverty Levels                 :   From " + Txt_Pov_Low.Text + "    To " + Txt_Pov_High.Text; break;

            //        case 14: Tmp_Sel_Text = "            Site                                   :   " + Sel_params_To_Print[2]; break;
            //        case 15: Tmp_Sel_Text = "            Zip Code                          :   " + (Rb_Zip_All.Checked ? "All Zip Codes" : "Selected Zip Code"); break;
            //        case 16: Tmp_Sel_Text = "            County                             :   " + (Rb_County_All.Checked ? "All Counties" : "Selected County"); break;

            //        case 18: Tmp_Sel_Text = "            Demographic Count         :   " + Sel_params_To_Print[3]; break;
            //        case 19: Tmp_Sel_Text = "            Secret Applications          :   " + Sel_params_To_Print[0]; break;
            //        case 20: Tmp_Sel_Text = "            Produce Stastical Details :   " + (Rb_Details_Yes.Checked ? "Yes" : "No"); break;

            //        default: Tmp_Sel_Text = "  "; break;
            //    }


            //    Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
            //    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


            //    XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
            //    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

            //    XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
            //    Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
            //    Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


            //    XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
            //    Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


            //    XmlElement Textbox1_Top = xml.CreateElement("Top");
            //    Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

            //    XmlElement Textbox1_Left = xml.CreateElement("Left");
            //    Textbox1_Left.InnerText = "0.07292in";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

            //    Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

            //    XmlElement Textbox1_Height = xml.CreateElement("Height");
            //    Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

            //    XmlElement Textbox1_Width = xml.CreateElement("Width");
            //    //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
            //    Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

            //    XmlElement Textbox1_Style = xml.CreateElement("Style");
            //    Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

            //    XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
            //    Textbox1_Style.AppendChild(Textbox1_Style_Border);

            //    XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
            //    Textbox1_Style_Border_Style.InnerText = "None";
            //    Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

            //    XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
            //    Textbox1_Style_PaddingLeft.InnerText = "2pt";
            //    Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

            //    XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
            //    Textbox1_Style_PaddingRight.InnerText = "2pt";
            //    Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

            //    XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
            //    Textbox1_Style_PaddingTop.InnerText = "2pt";
            //    Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

            //    XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
            //    Textbox1_Style_PaddingTop.InnerText = "2pt";
            //    Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

            //}

            //XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
            //Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

            //XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
            //Break_After_SelParamRectangle_Location.InnerText = "End";
            //Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

            ////XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
            ////Sel_Rectangle_KeepTogether.InnerText = "true";
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

            ////XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
            ////Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

            ////XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
            ////Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

            ////XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
            ////Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

            ////XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
            //////Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            ////Sel_Rectangle_Width.InnerText = (true ? "7.5" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

            ////XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
            ////Sel_Rectangle_ZIndex.InnerText = "1";
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

            ////XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
            ////Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

            //XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
            //Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

            //XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
            //Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
            //Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            switch (RDLC_For)
            {
                case "BYPS":
                    foreach (DG_Bypass_Entity Entity in DG_Bypass_List)                     // Dynamic based on Display Columns in Result Table
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
                    break;
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";

            //switch (RDLC_For)
            //{
            //    case "BYPS":


            foreach (DG_Bypass_Entity Entity in DG_Bypass_List)           // Dynamic based on Display Columns in Result Table
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

                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight);


                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black";//"LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

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
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            char Tmp_Double_Codes = '"';
            foreach (DG_Bypass_Entity Entity in DG_Bypass_List)        // Dynamic based on Display Columns in Result Table
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
                    if (Entity.Column_Name == "Byp_updated_date" || Entity.Column_Name == "Byp_updated_date")
                        Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";

                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R":
                            Text_Align = "Right"; break;
                    }


                    //    Return_Value.InnerText = "=CDate(Fields!" + Entity.Column_Name + ".Value)";        // Dynamic Column Heading

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "None";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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



            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            //if (rbBoth.Checked)
            //{
            //    for (int Loop = 0; Loop < 15; Loop++)            // Dynamic based on Display Columns in 3/6 10212014
            //    {
            //        XmlElement TablixMember = xml.CreateElement("TablixMember");
            //        Tablix_Col_Members.AppendChild(TablixMember);
            //    }
            //}
            //else
            //{
                for (int Loop = 0; Loop < 14; Loop++)            // Dynamic based on Display Columns in 3/6 10212014
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            //}


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


            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            Tablix.AppendChild(SubReport_PageBreak);

            XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            SubReport_PageBreak_Location.InnerText = "End";
            SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

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


            //XmlElement Top = xml.CreateElement("Top");
            //Top.InnerText = (Total_Sel_TextBox_Height).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            ////Top.InnerText = "0.60417in";
            //Tablix.AppendChild(Top);

            //XmlElement Left = xml.CreateElement("Left");
            //Left.InnerText = "0.20417in";
            ////Left.InnerText = "0.60417in";
            //Tablix.AppendChild(Left);

            //XmlElement Height1 = xml.CreateElement("Height");
            //Height1.InnerText = "0.5in";
            //Tablix.AppendChild(Height1);

            //XmlElement Width1 = xml.CreateElement("Width");
            //Width1.InnerText = "5.3229in";
            //Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);


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
            //Width.InnerText = "11in";      //Landscape "A4"

            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            if (Include_header)
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                PageHeader_Height.InnerText = "0.51958in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (Include_Header_Title)
                {
                    XmlElement Header_TextBox = xml.CreateElement("Textbox");
                    Header_TextBox.SetAttribute("Name", "HeaderTextBox");
                    Header_ReportItems.AppendChild(Header_TextBox);

                    XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
                    HeaderTextBox_CanGrow.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

                    XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
                    HeaderTextBox_Keep.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_Keep);

                    XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
                    Header_TextBox.AppendChild(Header_Paragraphs);

                    XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
                    Header_Paragraphs.AppendChild(Header_Paragraph);

                    XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
                    Header_Paragraph.AppendChild(Header_TextRuns);

                    XmlElement Header_TextRun = xml.CreateElement("TextRun");
                    Header_TextRuns.AppendChild(Header_TextRun);

                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = "Bypass Report";//Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);

                    XmlElement Header_TextRun_Style = xml.CreateElement("Style");
                    Header_TextRun.AppendChild(Header_TextRun_Style);

                    XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
                    Header_Style_Font.InnerText = "Times New Roman";
                    Header_TextRun_Style.AppendChild(Header_Style_Font);

                    XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
                    Header_Style_FontSize.InnerText = "16pt";
                    Header_TextRun_Style.AppendChild(Header_Style_FontSize);

                    XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
                    Header_Style_TextDecoration.InnerText = "Underline";
                    Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

                    XmlElement Header_Style_Color = xml.CreateElement("Color");
                    Header_Style_Color.InnerText = "#104cda";
                    Header_TextRun_Style.AppendChild(Header_Style_Color);

                    XmlElement Header_TextBox_Top = xml.CreateElement("Top");
                    Header_TextBox_Top.InnerText = "0.24792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
                    Header_TextBox_Left.InnerText = "1.42361in";
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    Header_TextBox_Height.InnerText = "0.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Height);

                    XmlElement Header_TextBox_Width = xml.CreateElement("Width");
                    Header_TextBox_Width.InnerText = "5.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Width);

                    XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
                    Header_TextBox_ZIndex.InnerText = "1";
                    Header_TextBox.AppendChild(Header_TextBox_ZIndex);


                    XmlElement Header_TextBox_Style = xml.CreateElement("Style");
                    Header_TextBox.AppendChild(Header_TextBox_Style);

                    XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
                    Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

                    XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Header_TB_StyleBorderStyle.InnerText = "None";
                    Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

                    XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Header_TB_SBS_LeftPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

                    XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Header_TB_SBS_RightPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

                    XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Header_TB_SBS_TopPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

                    XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Header_TB_SBS_BotPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

                    XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
                    Header_Paragraph.AppendChild(Header_Text_Align_Style);

                    XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    Header_Text_Align.InnerText = "Center";
                    Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }

                //if (Include_Header_Image)
                //{
                //    // Add Image Heare
                //}

                XmlElement PageHeader_Style = xml.CreateElement("Style");
                PageHeader.AppendChild(PageHeader_Style);

                XmlElement PageHeader_Border = xml.CreateElement("Border");
                PageHeader_Style.AppendChild(PageHeader_Border);

                XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
                PageHeader_Border_Style.InnerText = "None";
                PageHeader_Border.AppendChild(PageHeader_Border_Style);


                XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
                PageHeader_BackgroundColor.InnerText = "White";
                PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            }


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            if (Include_Footer)
            {
                XmlElement PageFooter = xml.CreateElement("PageFooter");
                Page.AppendChild(PageFooter);

                XmlElement PageFooter_Height = xml.CreateElement("Height");
                PageFooter_Height.InnerText = "0.35083in";
                PageFooter.AppendChild(PageFooter_Height);

                XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                Footer_PrintOnFirstPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnFirstPage);

                XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                Footer_PrintOnLastPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnLastPage);

                XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
                PageFooter.AppendChild(Footer_ReportItems);

                if (Include_Footer_PageCnt)
                {
                    XmlElement Footer_TextBox = xml.CreateElement("Textbox");
                    Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
                    Footer_ReportItems.AppendChild(Footer_TextBox);

                    XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
                    FooterTextBox_CanGrow.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

                    XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
                    FooterTextBox_Keep.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_Keep);

                    XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
                    Footer_TextBox.AppendChild(Footer_Paragraphs);

                    XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
                    Footer_Paragraphs.AppendChild(Footer_Paragraph);

                    XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
                    Footer_Paragraph.AppendChild(Footer_TextRuns);

                    XmlElement Footer_TextRun = xml.CreateElement("TextRun");
                    Footer_TextRuns.AppendChild(Footer_TextRun);

                    XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
                    Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = "1.65625in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Width);


                    XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
                    Footer_TextBox.AppendChild(Footer_TextBox_Style);

                    XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
                    Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

                    XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Footer_TB_StyleBorderStyle.InnerText = "None";
                    Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

                    XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Footer_TB_SBS_LeftPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

                    XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Footer_TB_SBS_RightPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

                    XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Footer_TB_SBS_TopPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

                    XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Footer_TB_SBS_BotPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

                    XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
                    Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

                    //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    //Header_Text_Align.InnerText = "Center";
                    //Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }
            }


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
                //xml.Save(@"C:\Capreports\" + Main_Rep_Name + "Bypass_RdlC.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                Bypass_Rep_Name =  "RNGB0004_Bypass_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";

                xml.Save(ReportPath + Bypass_Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            //  Console.ReadLine();        //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        }

        //****************************************************************************************************

        //***************    SNP Bypass RDLC     ********************************

        private void SNP_Dynamic_RDLC()
        {

            string RDLC_For = "SNP";

            Get_DG_SNP_Bypass_Table_Structure();
            

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

            switch (RDLC_For)
            {
                case "SNP":
                    foreach (DG_SNP_Bypass_Entity Entity in DG_SNP_Bypass_List)
                    {
                        XmlElement Field = xml.CreateElement("Field");
                        Field.SetAttribute("Name", Entity.Column_Name);
                        Fields.AppendChild(Field);

                        XmlElement DataField = xml.CreateElement("DataField");
                        DataField.InnerText = Entity.Column_Name;
                        Field.AppendChild(DataField);
                    }
                    break;
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
            double Total_Sel_TextBox_Height = 0.16667;
            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            switch (RDLC_For)
            {
                case "SNP":
                    foreach (DG_SNP_Bypass_Entity Entity in DG_SNP_Bypass_List)                    // Dynamic based on Display Columns in Result Table
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
                    break;
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";

            //switch (RDLC_For)
            //{
            //    case "BYPS":


            char Tmp_Double_Codes = '"';
            foreach (DG_SNP_Bypass_Entity Entity in DG_SNP_Bypass_List)           // Dynamic based on Display Columns in Result Table
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

                    //if (Entity.Column_Name == "Ind_Date")
                    //    Return_Value.InnerText = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";
                    //else
                    Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
                    TextRun.AppendChild(Return_Value);


                    XmlElement Cell_Align = xml.CreateElement("Style");
                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
                    Cell_TextAlign.InnerText = "Center";
                    Cell_Align.AppendChild(Cell_TextAlign);
                    Paragraph.AppendChild(Cell_Align);


                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);

                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight);


                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black";//"LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

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
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            foreach (DG_SNP_Bypass_Entity Entity in DG_SNP_Bypass_List)        // Dynamic based on Display Columns in Result Table
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
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R": Text_Align = "Right"; break;
                        case "C": Text_Align = "Center"; break;
                    }

                    if (Entity.Column_Name == "Ind_Date")
                        Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "None";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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



            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            if (rbBoth.Checked)
            {
                for (int Loop = 0; Loop < 28; Loop++)            // Dynamic based on Display Columns in 3/6 
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }
            else
            {
                for (int Loop = 0; Loop < 26; Loop++)            // Dynamic based on Display Columns in 3/6 
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


            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            Tablix.AppendChild(SubReport_PageBreak);

            XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            SubReport_PageBreak_Location.InnerText = "End";
            SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

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


            //XmlElement Top = xml.CreateElement("Top");
            //Top.InnerText = (Total_Sel_TextBox_Height).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            ////Top.InnerText = "0.60417in";
            //Tablix.AppendChild(Top);

            //XmlElement Left = xml.CreateElement("Left");
            //Left.InnerText = "0.20417in";
            ////Left.InnerText = "0.60417in";
            //Tablix.AppendChild(Left);

            //XmlElement Height1 = xml.CreateElement("Height");
            //Height1.InnerText = "0.5in";
            //Tablix.AppendChild(Height1);

            //XmlElement Width1 = xml.CreateElement("Width");
            //Width1.InnerText = "5.3229in";
            //Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);


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
            //Width.InnerText = "11in";      //Landscape "A4"

            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            if (Include_header)
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                PageHeader_Height.InnerText = "0.51958in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (Include_Header_Title)
                {
                    XmlElement Header_TextBox = xml.CreateElement("Textbox");
                    Header_TextBox.SetAttribute("Name", "HeaderTextBox");
                    Header_ReportItems.AppendChild(Header_TextBox);

                    XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
                    HeaderTextBox_CanGrow.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

                    XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
                    HeaderTextBox_Keep.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_Keep);

                    XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
                    Header_TextBox.AppendChild(Header_Paragraphs);

                    XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
                    Header_Paragraphs.AppendChild(Header_Paragraph);

                    XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
                    Header_Paragraph.AppendChild(Header_TextRuns);

                    XmlElement Header_TextRun = xml.CreateElement("TextRun");
                    Header_TextRuns.AppendChild(Header_TextRun);

                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = "Detail Individual Report";//Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);

                    XmlElement Header_TextRun_Style = xml.CreateElement("Style");
                    Header_TextRun.AppendChild(Header_TextRun_Style);

                    XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
                    Header_Style_Font.InnerText = "Times New Roman";
                    Header_TextRun_Style.AppendChild(Header_Style_Font);

                    XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
                    Header_Style_FontSize.InnerText = "16pt";
                    Header_TextRun_Style.AppendChild(Header_Style_FontSize);

                    XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
                    Header_Style_TextDecoration.InnerText = "Underline";
                    Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

                    XmlElement Header_Style_Color = xml.CreateElement("Color");
                    Header_Style_Color.InnerText = "#104cda";
                    Header_TextRun_Style.AppendChild(Header_Style_Color);

                    XmlElement Header_TextBox_Top = xml.CreateElement("Top");
                    Header_TextBox_Top.InnerText = "0.24792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
                    Header_TextBox_Left.InnerText = "2.72361in";
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    Header_TextBox_Height.InnerText = "0.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Height);

                    XmlElement Header_TextBox_Width = xml.CreateElement("Width");
                    Header_TextBox_Width.InnerText = "5.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Width);

                    XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
                    Header_TextBox_ZIndex.InnerText = "1";
                    Header_TextBox.AppendChild(Header_TextBox_ZIndex);


                    XmlElement Header_TextBox_Style = xml.CreateElement("Style");
                    Header_TextBox.AppendChild(Header_TextBox_Style);

                    XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
                    Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

                    XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Header_TB_StyleBorderStyle.InnerText = "None";
                    Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

                    XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Header_TB_SBS_LeftPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

                    XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Header_TB_SBS_RightPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

                    XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Header_TB_SBS_TopPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

                    XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Header_TB_SBS_BotPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

                    XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
                    Header_Paragraph.AppendChild(Header_Text_Align_Style);

                    XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    Header_Text_Align.InnerText = "Center";
                    Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }

                //if (Include_Header_Image)
                //{
                //    // Add Image Heare
                //}

                XmlElement PageHeader_Style = xml.CreateElement("Style");
                PageHeader.AppendChild(PageHeader_Style);

                XmlElement PageHeader_Border = xml.CreateElement("Border");
                PageHeader_Style.AppendChild(PageHeader_Border);

                XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
                PageHeader_Border_Style.InnerText = "None";
                PageHeader_Border.AppendChild(PageHeader_Border_Style);


                XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
                PageHeader_BackgroundColor.InnerText = "White";
                PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            }


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            if (Include_Footer)
            {
                XmlElement PageFooter = xml.CreateElement("PageFooter");
                Page.AppendChild(PageFooter);

                XmlElement PageFooter_Height = xml.CreateElement("Height");
                PageFooter_Height.InnerText = "0.35083in";
                PageFooter.AppendChild(PageFooter_Height);

                XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                Footer_PrintOnFirstPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnFirstPage);

                XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                Footer_PrintOnLastPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnLastPage);

                XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
                PageFooter.AppendChild(Footer_ReportItems);

                if (Include_Footer_PageCnt)
                {
                    XmlElement Footer_TextBox = xml.CreateElement("Textbox");
                    Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
                    Footer_ReportItems.AppendChild(Footer_TextBox);

                    XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
                    FooterTextBox_CanGrow.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

                    XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
                    FooterTextBox_Keep.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_Keep);

                    XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
                    Footer_TextBox.AppendChild(Footer_Paragraphs);

                    XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
                    Footer_Paragraphs.AppendChild(Footer_Paragraph);

                    XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
                    Footer_Paragraph.AppendChild(Footer_TextRuns);

                    XmlElement Footer_TextRun = xml.CreateElement("TextRun");
                    Footer_TextRuns.AppendChild(Footer_TextRun);

                    XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
                    Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = "1.65625in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Width);


                    XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
                    Footer_TextBox.AppendChild(Footer_TextBox_Style);

                    XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
                    Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

                    XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Footer_TB_StyleBorderStyle.InnerText = "None";
                    Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

                    XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Footer_TB_SBS_LeftPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

                    XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Footer_TB_SBS_RightPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

                    XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Footer_TB_SBS_TopPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

                    XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Footer_TB_SBS_BotPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

                    XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
                    Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

                    //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    //Header_Text_Align.InnerText = "Center";
                    //Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }
            }


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
                //xml.Save(@"C:\Capreports\" + Main_Rep_Name+ "SNP_IND_RdlC.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                Ind_Rep_Name =  "RNGB0004_SNP_IND_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";


                xml.Save(ReportPath + Ind_Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            //  Console.ReadLine();        //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        }

        //****************************************************************************************************


        //***************    MST Bypass RDLC     ********************************

        private void MST_Dynamic_RDLC()
        {

            string RDLC_For = "MST";

            Get_DG_MST_Bypass_Table_Structure();

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

            switch (RDLC_For)
            {
                case "MST":
                    foreach (DG_SNP_Bypass_Entity Entity in DG_MST_Bypass_List)
                    {
                        XmlElement Field = xml.CreateElement("Field");
                        Field.SetAttribute("Name", Entity.Column_Name);
                        Fields.AppendChild(Field);

                        XmlElement DataField = xml.CreateElement("DataField");
                        DataField.InnerText = Entity.Column_Name;
                        Field.AppendChild(DataField);
                    }
                    break;
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
            double Total_Sel_TextBox_Height = 0.16667;
            //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

            XmlElement Tablix = xml.CreateElement("Tablix");
            Tablix.SetAttribute("Name", "Tablix3");
            ReportItems.AppendChild(Tablix);

            XmlElement TablixBody = xml.CreateElement("TablixBody");
            Tablix.AppendChild(TablixBody);


            XmlElement TablixColumns = xml.CreateElement("TablixColumns");
            TablixBody.AppendChild(TablixColumns);

            switch (RDLC_For)
            {
                case "MST":
                    foreach (DG_SNP_Bypass_Entity Entity in DG_MST_Bypass_List)                    // Dynamic based on Display Columns in Result Table
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
                    break;
            }

            XmlElement TablixRows = xml.CreateElement("TablixRows");
            TablixBody.AppendChild(TablixRows);

            XmlElement TablixRow = xml.CreateElement("TablixRow");
            TablixRows.AppendChild(TablixRow);

            XmlElement Row_Height = xml.CreateElement("Height");
            Row_Height.InnerText = "0.25in";
            TablixRow.AppendChild(Row_Height);

            XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
            TablixRow.AppendChild(Row_TablixCells);


            int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
            string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";

            //switch (RDLC_For)
            //{
            //    case "BYPS":


            char Tmp_Double_Codes = '"';
            foreach (DG_SNP_Bypass_Entity Entity in DG_MST_Bypass_List)           // Dynamic based on Display Columns in Result Table
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

                    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    Return_Style_FontWeight.InnerText = "Bold";
                    Return_Style.AppendChild(Return_Style_FontWeight);


                    //XmlElement Return_AlignStyle = xml.CreateElement("Style");
                    //Paragraph.AppendChild(Return_AlignStyle);

                    //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
                    //DefaultName.InnerText = "Textbox" + i.ToString();
                    //Textbox.AppendChild(DefaultName);


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);


                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "Black";//"LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
                    Border_Style.InnerText = "Solid";
                    Cell_Border.AppendChild(Border_Style);

                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    Cell_Style_BackColor.InnerText = "LightSteelBlue";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

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
            Row_Height2.InnerText = "0.2in";
            TablixRow2.AppendChild(Row_Height2);

            XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
            TablixRow2.AppendChild(Row_TablixCells2);

            string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
            foreach (DG_SNP_Bypass_Entity Entity in DG_MST_Bypass_List)        // Dynamic based on Display Columns in Result Table
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
                    Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
                    Text_Align = "Left";
                    switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
                    {
                        case "R": Text_Align = "Right"; break;
                        case "C": Text_Align = "Center"; break;
                    }

                    if (Entity.Column_Name == "Fam_Date" || Entity.Column_Name == "Fam_Ver_Date")
                        Field_Value = "=Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + ")";

                    Return_Value.InnerText = Field_Value;
                    TextRun.AppendChild(Return_Value);

                    XmlElement Return_Style = xml.CreateElement("Style");
                    TextRun.AppendChild(Return_Style);


                    //if (Entity.Column_Name == "Sum_Child_Desc" ||
                    //    Entity.Column_Name == "Sum_Child_Period_Count" ||
                    //    Entity.Column_Name == "Sum_Child_Cum_Count") // 11292012
                    //{
                    //    XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
                    //    Return_Style_FontWeight.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + " OR Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICTOTL" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
                    //    Return_Style.AppendChild(Return_Style_FontWeight);
                    //}

                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Right" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }


                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);

                    XmlElement Cell_Border = xml.CreateElement("Border");
                    Cell_style.AppendChild(Cell_Border);

                    XmlElement Border_Color = xml.CreateElement("Color");
                    Border_Color.InnerText = "LightGrey";
                    Cell_Border.AppendChild(Border_Color);

                    XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
                    Border_Style.InnerText = "None";
                    Cell_Border.AppendChild(Border_Style);


                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!Sum_Child_Code.Value=" + Tmp_Double_Codes + "STATICHEAD" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "White";
                    Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth


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





            XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
            Tablix.AppendChild(TablixColumnHierarchy);

            XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
            TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

            if (rbBoth.Checked)
            {
                for (int Loop = 0; Loop < 33; Loop++)            // Dynamic based on Display Columns in 3/6 
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }
            else
            {
                for (int Loop = 0; Loop < 31; Loop++)            // Dynamic based on Display Columns in 3/6 
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


            XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
            RepeatRowHeaders.InnerText = "true";
            Tablix.AppendChild(RepeatRowHeaders);

            XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
            FixedRowHeaders.InnerText = "true";
            Tablix.AppendChild(FixedRowHeaders);

            XmlElement DataSetName1 = xml.CreateElement("DataSetName");
            DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
            Tablix.AppendChild(DataSetName1);

            XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");
            Tablix.AppendChild(SubReport_PageBreak);

            XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
            SubReport_PageBreak_Location.InnerText = "End";
            SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

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


            //XmlElement Top = xml.CreateElement("Top");
            //Top.InnerText = (Total_Sel_TextBox_Height).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
            ////Top.InnerText = "0.60417in";
            //Tablix.AppendChild(Top);

            //XmlElement Left = xml.CreateElement("Left");
            //Left.InnerText = "0.20417in";
            ////Left.InnerText = "0.60417in";
            //Tablix.AppendChild(Left);

            //XmlElement Height1 = xml.CreateElement("Height");
            //Height1.InnerText = "0.5in";
            //Tablix.AppendChild(Height1);

            //XmlElement Width1 = xml.CreateElement("Width");
            //Width1.InnerText = "5.3229in";
            //Tablix.AppendChild(Width1);


            XmlElement Style10 = xml.CreateElement("Style");
            Tablix.AppendChild(Style10);

            XmlElement Style10_Border = xml.CreateElement("Border");
            Style10.AppendChild(Style10_Border);

            XmlElement Style10_Border_Style = xml.CreateElement("Style");
            Style10_Border_Style.InnerText = "None";
            Style10_Border.AppendChild(Style10_Border_Style);


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
            //Width.InnerText = "11in";      //Landscape "A4"

            //if(Rb_A4_Port.Checked)
            //    Width.InnerText = "8.27in";      //Portrait "A4"
            //else
            //    Width.InnerText = "11in";      //Landscape "A4"
            Report.AppendChild(Width);


            XmlElement Page = xml.CreateElement("Page");
            Report.AppendChild(Page);

            //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

            if (Include_header)
            {
                XmlElement PageHeader = xml.CreateElement("PageHeader");
                Page.AppendChild(PageHeader);

                XmlElement PageHeader_Height = xml.CreateElement("Height");
                PageHeader_Height.InnerText = "0.51958in";
                PageHeader.AppendChild(PageHeader_Height);

                XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                PrintOnFirstPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnFirstPage);

                XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                PrintOnLastPage.InnerText = "true";
                PageHeader.AppendChild(PrintOnLastPage);


                XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
                PageHeader.AppendChild(Header_ReportItems);

                if (Include_Header_Title)
                {
                    XmlElement Header_TextBox = xml.CreateElement("Textbox");
                    Header_TextBox.SetAttribute("Name", "HeaderTextBox");
                    Header_ReportItems.AppendChild(Header_TextBox);

                    XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
                    HeaderTextBox_CanGrow.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

                    XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
                    HeaderTextBox_Keep.InnerText = "true";
                    Header_TextBox.AppendChild(HeaderTextBox_Keep);

                    XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
                    Header_TextBox.AppendChild(Header_Paragraphs);

                    XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
                    Header_Paragraphs.AppendChild(Header_Paragraph);

                    XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
                    Header_Paragraph.AppendChild(Header_TextRuns);

                    XmlElement Header_TextRun = xml.CreateElement("TextRun");
                    Header_TextRuns.AppendChild(Header_TextRun);

                    XmlElement Header_TextRun_Value = xml.CreateElement("Value");
                    Header_TextRun_Value.InnerText = "Detail Family Report";//Rep_Header_Title;   // Dynamic Report Name
                    Header_TextRun.AppendChild(Header_TextRun_Value);

                    XmlElement Header_TextRun_Style = xml.CreateElement("Style");
                    Header_TextRun.AppendChild(Header_TextRun_Style);

                    XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
                    Header_Style_Font.InnerText = "Times New Roman";
                    Header_TextRun_Style.AppendChild(Header_Style_Font);

                    XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
                    Header_Style_FontSize.InnerText = "16pt";
                    Header_TextRun_Style.AppendChild(Header_Style_FontSize);

                    XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
                    Header_Style_TextDecoration.InnerText = "Underline";
                    Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

                    XmlElement Header_Style_Color = xml.CreateElement("Color");
                    Header_Style_Color.InnerText = "#104cda";
                    Header_TextRun_Style.AppendChild(Header_Style_Color);

                    XmlElement Header_TextBox_Top = xml.CreateElement("Top");
                    Header_TextBox_Top.InnerText = "0.24792in";
                    Header_TextBox.AppendChild(Header_TextBox_Top);

                    XmlElement Header_TextBox_Left = xml.CreateElement("Left");
                    Header_TextBox_Left.InnerText = "2.72361in";
                    Header_TextBox.AppendChild(Header_TextBox_Left);

                    XmlElement Header_TextBox_Height = xml.CreateElement("Height");
                    Header_TextBox_Height.InnerText = "0.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Height);

                    XmlElement Header_TextBox_Width = xml.CreateElement("Width");
                    Header_TextBox_Width.InnerText = "5.30208in";
                    Header_TextBox.AppendChild(Header_TextBox_Width);

                    XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
                    Header_TextBox_ZIndex.InnerText = "1";
                    Header_TextBox.AppendChild(Header_TextBox_ZIndex);


                    XmlElement Header_TextBox_Style = xml.CreateElement("Style");
                    Header_TextBox.AppendChild(Header_TextBox_Style);

                    XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
                    Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

                    XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Header_TB_StyleBorderStyle.InnerText = "None";
                    Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

                    XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Header_TB_SBS_LeftPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

                    XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Header_TB_SBS_RightPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

                    XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Header_TB_SBS_TopPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

                    XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Header_TB_SBS_BotPad.InnerText = "2pt";
                    Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

                    XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
                    Header_Paragraph.AppendChild(Header_Text_Align_Style);

                    XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    Header_Text_Align.InnerText = "Center";
                    Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }

                //if (Include_Header_Image)
                //{
                //    // Add Image Heare
                //}

                XmlElement PageHeader_Style = xml.CreateElement("Style");
                PageHeader.AppendChild(PageHeader_Style);

                XmlElement PageHeader_Border = xml.CreateElement("Border");
                PageHeader_Style.AppendChild(PageHeader_Border);

                XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
                PageHeader_Border_Style.InnerText = "None";
                PageHeader_Border.AppendChild(PageHeader_Border_Style);


                XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
                PageHeader_BackgroundColor.InnerText = "White";
                PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
            }


            //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



            //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

            if (Include_Footer)
            {
                XmlElement PageFooter = xml.CreateElement("PageFooter");
                Page.AppendChild(PageFooter);

                XmlElement PageFooter_Height = xml.CreateElement("Height");
                PageFooter_Height.InnerText = "0.35083in";
                PageFooter.AppendChild(PageFooter_Height);

                XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
                Footer_PrintOnFirstPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnFirstPage);

                XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
                Footer_PrintOnLastPage.InnerText = "true";
                PageFooter.AppendChild(Footer_PrintOnLastPage);

                XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
                PageFooter.AppendChild(Footer_ReportItems);

                if (Include_Footer_PageCnt)
                {
                    XmlElement Footer_TextBox = xml.CreateElement("Textbox");
                    Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
                    Footer_ReportItems.AppendChild(Footer_TextBox);

                    XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
                    FooterTextBox_CanGrow.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

                    XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
                    FooterTextBox_Keep.InnerText = "true";
                    Footer_TextBox.AppendChild(FooterTextBox_Keep);

                    XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
                    Footer_TextBox.AppendChild(Footer_Paragraphs);

                    XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
                    Footer_Paragraphs.AppendChild(Footer_Paragraph);

                    XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
                    Footer_Paragraph.AppendChild(Footer_TextRuns);

                    XmlElement Footer_TextRun = xml.CreateElement("TextRun");
                    Footer_TextRuns.AppendChild(Footer_TextRun);

                    XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
                    Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
                    Footer_TextRun.AppendChild(Footer_TextRun_Value);

                    XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
                    Footer_TextRun.AppendChild(Footer_TextRun_Style);

                    XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
                    Footer_TextBox_Top.InnerText = "0.06944in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Top);

                    XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
                    Footer_TextBox_Height.InnerText = "0.25in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Height);

                    XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
                    Footer_TextBox_Width.InnerText = "1.65625in";
                    Footer_TextBox.AppendChild(Footer_TextBox_Width);


                    XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
                    Footer_TextBox.AppendChild(Footer_TextBox_Style);

                    XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
                    Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

                    XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
                    Footer_TB_StyleBorderStyle.InnerText = "None";
                    Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

                    XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
                    Footer_TB_SBS_LeftPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

                    XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
                    Footer_TB_SBS_RightPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

                    XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
                    Footer_TB_SBS_TopPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

                    XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
                    Footer_TB_SBS_BotPad.InnerText = "2pt";
                    Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

                    XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
                    Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

                    //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
                    //Header_Text_Align.InnerText = "Center";
                    //Header_Text_Align_Style.AppendChild(Header_Text_Align);
                }
            }


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
                //xml.Save(@"C:\Capreports\" + Main_Rep_Name + "MST_FAM_RdlC.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                MST_Rep_Name = "RNGB0004_MST_FAM_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";
                xml.Save(ReportPath + MST_Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            // Console.ReadLine();            //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
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


        string Scr_Oper_Mode = "RNGB0004";
        private void Lnk_SwitchTo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (Lnk_SwitchTo.Text)
            {
                case "Switch To Performance Measures":
                    Lnk_SwitchTo.Text = "Switch To Case DemoGraphics";

                    Scr_Oper_Mode = "CASB0014";
                    //20160303
                    Fund_Panel.Visible = false; //panel5.Visible = false;
                    //Fund_Panel.Visible = lblDemographicsCount.Visible = panel15.Visible = false; //panel5.Visible = false;
                    this.Text = "CASB0014 - Performance Measures";

                    lblDateSelection.Text = "Process Report By";
                    lblAttributes.Text = "Categories";
                    Rb_Agy_Def.Text = "All";
                    Rb_User_Def.Text = "Only Goal Associated";

                    lblProduceStatistical.Text = "Produce Details";

                    Rb_Details_No.Text = "No";
                    Rb_Details_Yes.Text = "Yes";

                    lblZipCodes.Text = "Domain";
                    Rb_Zip_All.Text = "All Domain";
                    Rb_Zip_Sel.Text = "Select Domains";

                    //lblCounty.Text = "Report Format";
                    //Rb_County_All.Text = "Performance Measures Only";
                    //Rb_County_Sel.Text = "Performance Measures + Goal Details";
                    lblMilestones.Text = "Date Selection";
                    Rb_MS_Date.Text = "Outcome Date";//**"Milestone Date";
                    Rb_MS_AddDate.Text = "Outcome Add Date";//**"Milestone Add Date";

                    lblDemographicsCount.Text = "Report Format";
                    Rb_OBO_Mem.Text = "Performance Measures Only";
                    Rb_SNP_Mem.Text = "Performance Measures + Goal Details";
                    Rb_SNP_Mem.Location = new System.Drawing.Point(154, 2);


                    CAMS_Panel.Visible = lblDateSelection.Visible = false; //panel15.Visible = false; 20160303


                    Lbl_Program.Visible = Cmb_Program.Visible = true;
                    Lbl_Program.Location = new System.Drawing.Point(5, 36);
                    Cmb_Program.Location = new System.Drawing.Point(143, 36);
                    this.Cmb_Program.Size = new System.Drawing.Size(249, 21);

                    this.Date_Panel.Location = new System.Drawing.Point(0, -1);
                    this.Service_Panel.Size = new System.Drawing.Size(450, 21);

                    //this.panel8.Location = new System.Drawing.Point(0, 48);
                    
                    
                    this.pnlBelowRdb.Location = new System.Drawing.Point(0, 48);
                    //this.panel8.Size = new System.Drawing.Size(607, 224); //20160303
                    this.pnlBelowRdb.Size = new System.Drawing.Size(607, 247);

                    //this.panel4.Location = new System.Drawing.Point(-1, 33);
                    this.pnlExcCPF.Location = new System.Drawing.Point(-1, 55);

                    Rb_SNP_Mem.Size = new System.Drawing.Size(230, 21);

                    this.pnlExcCPF.Size = new System.Drawing.Size(607, 296);
                    //this.panel3.Location = new System.Drawing.Point(4, 385); //20160303
                    //this.panel2.Size = new System.Drawing.Size(607, 352); //20160303
                    
                    //this.Size = new System.Drawing.Size(613, 395);
                    
                    //this.Size = new System.Drawing.Size(615, 422);
                    Fill_Program_Combo();
                    break;

                case "Switch To Case DemoGraphics":
                    Lnk_SwitchTo.Text = "Switch To Performance Measures";
                    Lbl_Program.Visible = Cmb_Program.Visible = false;

                    Scr_Oper_Mode = "RNGB0004";
                    this.Text = "RNGB0004 -  Case DemoGraphics";
                    lblDateSelection.Text = "Date Selection";
                    lblAttributes.Text = "Attributes";
                    Rb_Agy_Def.Text = "Agency Defined";
                    Rb_User_Def.Text = "User Defined Associations";

                    lblProduceStatistical.Text = "Produce Statistical Report";

                    Rb_Details_No.Text = "No";
                    Rb_Details_Yes.Text = "Yes";

                    lblZipCodes.Text = "ZIP Codes";
                    Rb_Zip_All.Text = "All";
                    Rb_Zip_Sel.Text = "Selected";

                    lblCounty.Text = "County";
                    Rb_County_All.Text = "All";
                    Rb_County_Sel.Text = "Selected";

                    lblMilestones.Text = "Outcomes/Services";//"Milestones/Services";
                    Rb_MS_Date.Text = "Posting Date";
                    Rb_MS_AddDate.Text = "Add Date";


                    lblDemographicsCount.Text = "Demographics Count";
                    Rb_OBO_Mem.Text = "Only Members Receiving Service"; //**"Only Members Receiving Service/Activity";
                    Rb_SNP_Mem.Text = "All Household Members";
                    Rb_SNP_Mem.Location = new System.Drawing.Point(276, 2);
                    Rb_SNP_Mem.Size = new System.Drawing.Size(148, 21);



                    this.pnlExcCPF.Location = new System.Drawing.Point(-1, 55);
                    this.pnlExcCPF.Size = new System.Drawing.Size(607, 319);
                    this.pnlReportFields.Size = new System.Drawing.Size(607, 377);
                    

                    this.pnlButtons.Location = new System.Drawing.Point(4, 410);
                    this.Date_Panel.Location = new System.Drawing.Point(0, 21);


                    this.Service_Panel.Size = new System.Drawing.Size(450, 46);

                    this.pnlBelowRdb.Location = new System.Drawing.Point(0, 72);
                    this.pnlBelowRdb.Size = new System.Drawing.Size(607, 247);
                    
                    Fund_Panel.Visible = lblDemographicsCount.Visible = pnlCounty.Visible =
                    CAMS_Panel.Visible = lblDateSelection.Visible = pnlDemoCount.Visible =
                    Rb_Process_MS.Checked = true;


                    this.lblDemographicsCount.Location = new System.Drawing.Point(5, 229);
                    this.pnlDemoCount.Location = new System.Drawing.Point(141, 224);
                    lblDemographicsCount.Visible = pnlDemoCount.Visible = true;

                    //this.Size = new System.Drawing.Size(613, 449);
                    this.Size = new System.Drawing.Size(615, 447);

                    break;
            }
            Initialize_All_Controls();
        }



        private void Initialize_All_Controls()
        {
            Cmb_CaseType.SelectedIndex = 0;
            Rb_MS_Date.Checked = Rb_Stat_Both.Checked = Rb_Site_All.Checked =
            Rb_Details_Yes.Checked = Rb_Zip_All.Checked = Rb_County_All.Checked =
            Rb_Mst_NonSec.Checked = Rb_Process_Both.Checked = true;

            All_CAMS_Selected =  true;
            Btn_CA_Selection.Visible = false; Btn_MS_Selection.Visible = false;
            Btn_MS_Selection.Text = Btn_CA_Selection.Text = "&All";
            Sel_MS_List.Clear();
            Sel_CA_List.Clear();

            Lbl_Program.Visible = true; //= Cmb_Program.Visible

            switch (Scr_Oper_Mode)
            {
                case "RNGB0004": Rb_User_Def.Checked = Rb_OBO_Mem.Checked = true; //**Fill_Program_Combo();
                    rbAllPrograms.Checked = true; HierarchyGrid.Rows.Clear(); SelectedHierarchies.Clear();
                    Ref_From_Date.Value = new DateTime(DateTime.Now.Year, 1, 1);// Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                    Ref_To_Date.Value = new DateTime(DateTime.Now.Year, 12, 31);// Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                    break;
               /** case "CASB0014": Rb_Agy_Def.Checked = true; Fill_Program_Combo();
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
                    break;*/
            }

            Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));

            Ref_From_Date.Checked = Ref_To_Date.Checked = Rep_From_Date.Checked = Rep_To_Date.Checked = true;

            Txt_Pov_Low.Text = "0"; Txt_Pov_High.Text = "999";


            ListcaseSiteEntity.Clear();
            ListcommonEntity.Clear();
            ListZipCode.Clear(); 
            ListGroupCode.Clear(); 
            Clear_Error_Providers();
        }

        private void Clear_Error_Providers()
        {
            _errorProvider.SetError(Ref_To_Date, null);
            _errorProvider.SetError(Rb_Zip_Sel, null);
            _errorProvider.SetError(Rb_County_Sel, null);
            _errorProvider.SetError(Txt_Sel_Site, null);
            _errorProvider.SetError(Txt_Pov_Low, null);
            _errorProvider.SetError(Txt_Pov_High, null);
            _errorProvider.SetError(Rb_Fund_Sel, null);
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



        //****************************************************************************************************




        //*****************PerformanceMeasures_Dynamic_RDLC***********************************************************************************

        //private void PerformanceMeasures_Dynamic_RDLC()
        //{

        //    Get_Report_Selection_Parameters();
        //    Get_PM_Result_Table_Structure();

        //    XmlNode xmlnode;

        //    XmlDocument xml = new XmlDocument();
        //    xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
        //    xml.AppendChild(xmlnode);

        //    XmlElement Report = xml.CreateElement("Report");
        //    Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
        //    Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
        //    xml.AppendChild(Report);

        //    XmlElement DataSources = xml.CreateElement("DataSources");
        //    XmlElement DataSource = xml.CreateElement("DataSource");
        //    DataSource.SetAttribute("Name", "CaptainDataSource");
        //    DataSources.AppendChild(DataSource);

        //    Report.AppendChild(DataSources);

        //    XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
        //    DataSource.AppendChild(ConnectionProperties);

        //    XmlElement DataProvider = xml.CreateElement("DataProvider");
        //    DataProvider.InnerText = "System.Data.DataSet";


        //    XmlElement ConnectString = xml.CreateElement("ConnectString");
        //    ConnectString.InnerText = "/* Local Connection */";
        //    ConnectionProperties.AppendChild(DataProvider);
        //    ConnectionProperties.AppendChild(ConnectString);

        //    //string SourceID = "rd:DataSourceID";
        //    //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
        //    //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
        //    //DataSource.AppendChild(DataSourceID);

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

        //    XmlElement DataSets = xml.CreateElement("DataSets");
        //    Report.AppendChild(DataSets);

        //    XmlElement DataSet = xml.CreateElement("DataSet");
        //    DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
        //    DataSets.AppendChild(DataSet);

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

        //    XmlElement Fields = xml.CreateElement("Fields");
        //    DataSet.AppendChild(Fields);

        //    foreach (DG_ResTab_Entity Entity in PM_Table_List)
        //    {
        //        XmlElement Field = xml.CreateElement("Field");
        //        Field.SetAttribute("Name", Entity.Column_Name);
        //        Fields.AppendChild(Field);

        //        XmlElement DataField = xml.CreateElement("DataField");
        //        DataField.InnerText = Entity.Column_Name;
        //        Field.AppendChild(DataField);
        //    }

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

        //    XmlElement Query = xml.CreateElement("Query");
        //    DataSet.AppendChild(Query);

        //    XmlElement DataSourceName = xml.CreateElement("DataSourceName");
        //    DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
        //    Query.AppendChild(DataSourceName);

        //    XmlElement CommandText = xml.CreateElement("CommandText");
        //    CommandText.InnerText = "/* Local Query */";
        //    Query.AppendChild(CommandText);


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
        //    //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    XmlElement Body = xml.CreateElement("Body");
        //    Report.AppendChild(Body);


        //    XmlElement ReportItems = xml.CreateElement("ReportItems");
        //    Body.AppendChild(ReportItems);

        //    XmlElement Height = xml.CreateElement("Height");
        //    //Height.InnerText = "4.15625in";       // Landscape
        //    Height.InnerText = "2in";           // Portrait
        //    Body.AppendChild(Height);


        //    XmlElement Style = xml.CreateElement("Style");
        //    Body.AppendChild(Style);

        //    XmlElement Border = xml.CreateElement("Border");
        //    Style.AppendChild(Border);

        //    XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
        //    BackgroundColor.InnerText = "White";
        //    Style.AppendChild(BackgroundColor);


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

        //    XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
        //    Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
        //    ReportItems.AppendChild(Sel_Rectangle);

        //    XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
        //    Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


        //    double Total_Sel_TextBox_Height = 0.16667;
        //    string Tmp_Sel_Text = string.Empty;
        //    for (int i = 0; i < 40; i++)
        //    {
        //        XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
        //        Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
        //        Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

        //        XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
        //        Textbox1_Cangrow.InnerText = "true";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

        //        XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
        //        Textbox1_Keep.InnerText = "true";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

        //        XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

        //        XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
        //        Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

        //        XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
        //        Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


        //        XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
        //        Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

        //        XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

        //        Tmp_Sel_Text = string.Empty;
        //        switch (i)
        //        {
        //            case 0: Tmp_Sel_Text = "Selected Report Parameters"; break;

        //            case 2: Tmp_Sel_Text = "      Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

        //            case 4: Tmp_Sel_Text = "            Categories"; break;
        //            case 5: Tmp_Sel_Text = " : " + (Rb_Agy_Def.Checked ? "All" : "Only Goal Associated"); break;
        //            case 6: Tmp_Sel_Text = "            Case Type"; break;
        //            case 7: Tmp_Sel_Text = " : " + ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString(); break;

                    
        //            case 8: Tmp_Sel_Text = "            Case Status"; break;
        //            case 9: Tmp_Sel_Text = " : " + Sel_params_To_Print[1]; break;

        //            case 10: Tmp_Sel_Text = "            Date Selection" ; break;
        //            case 11: Tmp_Sel_Text = " : " + (Rb_MS_AddDate.Checked ? "MS AddDate" : "MS Date"); break;
                        
        //            case 12: Tmp_Sel_Text = "            Reference Period Date" ; break;
        //            case 13: Tmp_Sel_Text = " : From: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_From_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
        //                                    + "    To: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_To_Date).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                                    break;

                    
        //            case 14: Tmp_Sel_Text = "            Report Period Date" ; break;
        //            case 15: Tmp_Sel_Text = " : From: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_FDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
        //                                    + "    To: " +
        //                                    CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Search_Entity.Rep_Period_TDate).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
        //                                    break;

                    
        //            case 18: Tmp_Sel_Text = "            Poverty Levels" ; break;
        //            case 19: Tmp_Sel_Text = " : From: " + Txt_Pov_Low.Text + "    To: " + Txt_Pov_High.Text; break;

        //            case 20: Tmp_Sel_Text = "            Site"; break;
        //            case 21: Tmp_Sel_Text = " : " + Sel_params_To_Print[2]; break;

        //            case 22: Tmp_Sel_Text = "            Groups" ; break;
        //            case 23: Tmp_Sel_Text = " : " + (Rb_Zip_All.Checked ? "All Groups" : "Selected Groups"); break;

        //            case 24: Tmp_Sel_Text = "            County"; break;
        //            case 25: Tmp_Sel_Text = " : " + (Rb_County_All.Checked ? "All Counties" : "Selected County"); break;

        //            case 26: Tmp_Sel_Text = "            Secret Applications" ; break;
        //            case 27: Tmp_Sel_Text = " : " + Sel_params_To_Print[0]; break;
        //            case 28: Tmp_Sel_Text = "            Produce Stastical Details"; break;
        //            case 29: Tmp_Sel_Text = " : " + (Rb_Details_Yes.Checked ? "Yes" : "No"); break;
        //            case 30: Tmp_Sel_Text = "            Program"; break;
        //            case 31: Tmp_Sel_Text = " : " + (Sel_Permesures_Programs); break;

        //            case 32: Tmp_Sel_Text = "            Report Format" ; break;
        //            case 33: Tmp_Sel_Text = " : " + (Rb_OBO_Mem.Checked ? "Performance Measures Only" : "Performance Measures + Goal Details"); break;

                        

        //            default: Tmp_Sel_Text = "  "; break;
        //        }

                

        //        Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
        //        Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


        //        XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
        //        Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

        //        XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
        //        Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
        //        Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


        //        XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
        //        Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


        //        XmlElement Textbox1_Top = xml.CreateElement("Top");
        //        Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

        //        XmlElement Textbox1_Left = xml.CreateElement("Left");
        //        //Textbox1_Left.InnerText = "0.07292in";
        //        Textbox1_Left.InnerText = ((i > 3 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "0.07292in" : "2.27292in") : "0.07292in");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

        //        if (i > 3 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim())))
        //        {
        //            if (i % 2 != 0)
        //                Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);
        //        }
        //        else
        //            Total_Sel_TextBox_Height += 0.21855;

        //        XmlElement Textbox1_Height = xml.CreateElement("Height");
        //        Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

        //        XmlElement Textbox1_Width = xml.CreateElement("Width");
        //        //Textbox1_Width.InnerText = "7.48777";
        //        Textbox1_Width.InnerText = ((i > 3 && (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()))) ? (i % 2 == 0 ? "2.2in" : "4.48777in") : "7.48777in");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

        //        XmlElement Textbox1_Style = xml.CreateElement("Style");
        //        Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

        //        XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
        //        Textbox1_Style.AppendChild(Textbox1_Style_Border);

        //        XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
        //        Textbox1_Style_Border_Style.InnerText = "None";
        //        Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

        //        XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
        //        Textbox1_Style_PaddingLeft.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

        //        XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
        //        Textbox1_Style_PaddingRight.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

        //        XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
        //        Textbox1_Style_PaddingTop.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

        //        XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
        //        Textbox1_Style_PaddingTop.InnerText = "2pt";
        //        Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

        //    }
            
        //    XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //    Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //    XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
        //    Break_After_SelParamRectangle_Location.InnerText = "End";
        //    Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

        //    XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
        //    Sel_Rectangle_KeepTogether.InnerText = "true";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

        //    XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
        //    Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

        //    XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
        //    Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

        //    XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
        //    Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

        //    XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
        //    //Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    Sel_Rectangle_Width.InnerText = (true ? "10" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

        //    XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
        //    Sel_Rectangle_ZIndex.InnerText = "1";
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

        //    XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
        //    Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

        //    XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
        //    Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

        //    XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
        //    Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
        //    Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



        //    XmlElement Tablix = xml.CreateElement("Tablix");
        //    Tablix.SetAttribute("Name", "Tablix3");
        //    ReportItems.AppendChild(Tablix);

        //    XmlElement TablixBody = xml.CreateElement("TablixBody");
        //    Tablix.AppendChild(TablixBody);


        //    XmlElement TablixColumns = xml.CreateElement("TablixColumns");
        //    TablixBody.AppendChild(TablixColumns);

        //    foreach (DG_ResTab_Entity Entity in PM_Table_List)                      // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")
        //        {
        //            XmlElement TablixColumn = xml.CreateElement("TablixColumn");
        //            TablixColumns.AppendChild(TablixColumn);

        //            XmlElement Col_Width = xml.CreateElement("Width");
        //            //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
        //            //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
        //            Col_Width.InnerText = Entity.Disp_Width;
        //            TablixColumn.AppendChild(Col_Width);
        //        }
        //    }

        //    XmlElement TablixRows = xml.CreateElement("TablixRows");
        //    TablixBody.AppendChild(TablixRows);

        //    XmlElement TablixRow = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow);

        //    XmlElement Row_Height = xml.CreateElement("Height");
        //    //Row_Height.InnerText = "0.25in";
        //    Row_Height.InnerText = "0.0000001in";
        //    TablixRow.AppendChild(Row_Height);

        //    XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
        //    TablixRow.AppendChild(Row_TablixCells);


        //    int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
        //    string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
        //    foreach (DG_ResTab_Entity Entity in PM_Table_List)            // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            //Entity.Column_Name;
        //            Tmp_Loop_Cnt++;

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells.AppendChild(TablixCell);


        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            //if (Entity.Col_Format_Type == "C")
        //            //    Field_type = "Checkbox";

        //            XmlElement Textbox = xml.CreateElement(Field_type);
        //            Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);



        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);



        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");

        //            Tmp_Disp_Column_Name = Entity.Disp_Name;


        //            //Disp_Col_Substring_Len = 6;

        //            //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
        //            Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
        //            TextRun.AppendChild(Return_Value);


        //            XmlElement Cell_Align = xml.CreateElement("Style");
        //            XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
        //            Cell_TextAlign.InnerText = "Center";
        //            Cell_Align.AppendChild(Cell_TextAlign);
        //            Paragraph.AppendChild(Cell_Align);


        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);

        //            ////XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //            ////Return_Style_FontWeight.InnerText = "Bold";
        //            ////Return_Style.AppendChild(Return_Style_FontWeight);


        //            //XmlElement Return_AlignStyle = xml.CreateElement("Style");
        //            //Paragraph.AppendChild(Return_AlignStyle);

        //            //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
        //            //DefaultName.InnerText = "Textbox" + i.ToString();
        //            //Textbox.AppendChild(DefaultName);


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);


        //            //////XmlElement Cell_Border = xml.CreateElement("Border");
        //            //////Cell_style.AppendChild(Cell_Border);

        //            //////XmlElement Border_Color = xml.CreateElement("Color");
        //            //////Border_Color.InnerText = "Black";//"LightGrey";
        //            //////Cell_Border.AppendChild(Border_Color);

        //            //////XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
        //            //////Border_Style.InnerText = "Solid";
        //            //////Cell_Border.AppendChild(Border_Style);

        //            //////XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            //////Cell_Style_BackColor.InnerText = "LightSteelBlue";
        //            //////Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);
        //        }
        //    }




        //    XmlElement TablixRow2 = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow2);

        //    XmlElement Row_Height2 = xml.CreateElement("Height");
        //    Row_Height2.InnerText = "0.175in";
        //    //Row_Height2.InnerText = "0.2in";
        //    TablixRow2.AppendChild(Row_Height2);

        //    XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
        //    TablixRow2.AppendChild(Row_TablixCells2);

        //    string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
        //    char Tmp_Double_Codes = '"';
        //    foreach (DG_ResTab_Entity Entity in PM_Table_List)        // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells2.AppendChild(TablixCell);

        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            XmlElement Textbox = xml.CreateElement("Textbox");
        //            Textbox.SetAttribute("Name", Entity.Column_Name);
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);

        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);

        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");


        //            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
        //            Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
        //            Text_Align = "Left";
        //            switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
        //            {
        //                case "R":
        //                    Text_Align = "Right"; break;
        //            }

        //            Return_Value.InnerText = Field_Value;
        //            TextRun.AppendChild(Return_Value);

        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);

        //            // New
        //            XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
        //            Textbox1_TextRun_Style_Color.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "#2D17EB" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + ")";
        //            //Textbox1_TextRun_Style_Color.InnerText = "Blue";
        //            Return_Style.AppendChild(Textbox1_TextRun_Style_Color);


        //            //XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
        //            //Paragraph.AppendChild(Textbox1_Paragraph_Style);

        //            // New


        //            //if (Entity.Column_Name == "Res_Table_Desc"  ) // 11292012
        //            //{
        //                XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //                Return_Style_FontWeight.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + " OR Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpTotal" + Tmp_Double_Codes +
        //                                                                                        " OR Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpHead" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
        //                Return_Style.AppendChild(Return_Style_FontWeight);
        //            //}



        //                if (!string.IsNullOrEmpty(Text_Align))
        //                {
        //                    XmlElement Cell_Align = xml.CreateElement("Style");
        //                    XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
        //                    Cell_TextAlign.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Left" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
        //                    //Cell_TextAlign.InnerText = Text_Align;
        //                    Cell_Align.AppendChild(Cell_TextAlign);
        //                    Paragraph.AppendChild(Cell_Align);
        //                }


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);

        //            XmlElement Cell_Border = xml.CreateElement("Border");
        //            Cell_style.AppendChild(Cell_Border);

        //            XmlElement Border_Color = xml.CreateElement("Color");
        //            Border_Color.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpHead" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + ")";
        //            //Border_Color.InnerText = "LightGrey";
        //            Cell_Border.AppendChild(Border_Color);

        //            XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
        //            Border_Style.InnerText = "None";
        //            Cell_Border.AppendChild(Border_Style);


        //            XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            Cell_Style_BackColor.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpHead" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
        //            //Cell_Style_BackColor.InnerText = "Blue";
        //            Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);

        //            //XmlElement ColSpan = xml.CreateElement("ColSpan");
        //            ////ColSpan.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "1" + "," + "9" + ")";
        //            //ColSpan.InnerText = "0";
        //            //CellContents.AppendChild(ColSpan);

        //            ////if (Entity.Column_Name == "Res_Table_Desc") // 11292012
        //            ////{

        //            ////    XmlElement Break_before_Group = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //            ////    Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //            ////    XmlElement Break_before_Group = xml.CreateElement("BreakLocation");
        //            ////    Break_After_SelParamRectangle_Location.InnerText = "End";
        //            ////    Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters
        //            ////}






        //        }
        //    }



        //    //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
        //    //TablixBody.AppendChild(SubReport_PageBreak);

        //    //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
        //    //SubReport_PageBreak_Location.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "End" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "None" + Tmp_Double_Codes + ")";
        //    ////SubReport_PageBreak_Location.InnerText = "End";
        //    //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);



        //    XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
        //    Tablix.AppendChild(TablixColumnHierarchy);

        //    XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
        //    TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

        //    for (int Loop = 0; Loop < 9; Loop++)            // Dynamic based on Display Columns in 3/6 
        //    {
        //        XmlElement TablixMember = xml.CreateElement("TablixMember");
        //        Tablix_Col_Members.AppendChild(TablixMember);
        //    }


        //    XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
        //    Tablix.AppendChild(TablixRowHierarchy);

        //    XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
        //    TablixRowHierarchy.AppendChild(Tablix_Row_Members);

        //    XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member);

        //    XmlElement FixedData = xml.CreateElement("FixedData");
        //    FixedData.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(FixedData);

        //    XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
        //    KeepWithGroup.InnerText = "After";
        //    Tablix_Row_Member.AppendChild(KeepWithGroup); 

        //    XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
        //    RepeatOnNewPage.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(RepeatOnNewPage);

        //    XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

        //    XmlElement Group = xml.CreateElement("Group"); // 5656565656
        //    Group.SetAttribute("Name", "Details1");
        //    Tablix_Row_Member1.AppendChild(Group);

        //            //XmlElement Group_Exps = xml.CreateElement("GroupExpressions"); // 5656565656
        //            //Group.AppendChild(Group_Exps);

        //            //XmlElement Group_Exp = xml.CreateElement("GroupExpression"); // 5656565656
        //            //Group_Exp.InnerText = "=Fields!Res_Group.Value+Fields!Res_Table_Desc.Value";
        //            //Group_Exps.AppendChild(Group_Exp);

        //            //XmlElement Group_Exp_Break = xml.CreateElement("PageBreak"); // 5656565656
        //            ////Group_Exp.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + ")";
        //            //Group.AppendChild(Group_Exp_Break);

        //            //XmlElement Group_Exp_Break_Loc = xml.CreateElement("BreakLocation"); // 5656565656
        //            ////Group_Exp_Break_Loc.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "Between" + "," + "None" + ")";
        //            //Group_Exp_Break_Loc.InnerText = "Between";
        //            //Group_Exp_Break.AppendChild(Group_Exp_Break_Loc);

        //    XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
        //    RepeatRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(RepeatRowHeaders);

        //    XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
        //    FixedRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(FixedRowHeaders);

        //    XmlElement DataSetName1 = xml.CreateElement("DataSetName");
        //    DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
        //    Tablix.AppendChild(DataSetName1);

        //    //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
        //    //Tablix.AppendChild(SubReport_PageBreak);

        //    //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
        //    //SubReport_PageBreak_Location.InnerText = "StartAndEnd";
        //    //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

        //    XmlElement SortExpressions = xml.CreateElement("SortExpressions");
        //    Tablix.AppendChild(SortExpressions);

        //    XmlElement SortExpression = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression);

        //    XmlElement SortExpression_Value = xml.CreateElement("Value");
        //    //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
        //    SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

        //    SortExpression.AppendChild(SortExpression_Value);

        //    XmlElement SortExpression_Direction = xml.CreateElement("Direction");
        //    SortExpression_Direction.InnerText = "Descending";
        //    SortExpression.AppendChild(SortExpression_Direction);


        //    XmlElement SortExpression1 = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression1);

        //    XmlElement SortExpression_Value1 = xml.CreateElement("Value");
        //    //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
        //    SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
        //    SortExpression1.AppendChild(SortExpression_Value1);


        //    XmlElement Top = xml.CreateElement("Top");
        //    //Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
        //    Top.InnerText = (Total_Sel_TextBox_Height + .205).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
        //    //Top.InnerText = "0.60417in";
        //    Tablix.AppendChild(Top);

        //    XmlElement Left = xml.CreateElement("Left");
        //    Left.InnerText = "0.20417in";
        //    //Left.InnerText = "0.60417in";
        //    Tablix.AppendChild(Left);

        //    XmlElement Height1 = xml.CreateElement("Height");
        //    Height1.InnerText = "0.5in";
        //    Tablix.AppendChild(Height1);

        //    XmlElement Width1 = xml.CreateElement("Width");
        //    Width1.InnerText = "5.3229in";
        //    Tablix.AppendChild(Width1);


        //    XmlElement Style10 = xml.CreateElement("Style");
        //    Tablix.AppendChild(Style10);

        //    XmlElement Style10_Border = xml.CreateElement("Border");
        //    Style10.AppendChild(Style10_Border);

        //    XmlElement Style10_Border_Style = xml.CreateElement("Style");
        //    Style10_Border_Style.InnerText = "None";
        //    Style10_Border.AppendChild(Style10_Border_Style);


        //    //XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //    //Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //    //XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
        //    //Break_After_SelParamRectangle_Location.InnerText = "End";
        //    //Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters


        //    //   Subreport
        //    ////////if (Summary_Sw)
        //    ////////{
        //    ////////    // Summary Sub Report 
        //    ////////}

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

        //    XmlElement Width = xml.CreateElement("Width");               // Total Page Width
        //    Width.InnerText = "6.5in";      //Common
        //    //if(Rb_A4_Port.Checked)
        //    //    Width.InnerText = "8.27in";      //Portrait "A4"
        //    //else
        //    //    Width.InnerText = "11in";      //Landscape "A4"
        //    Report.AppendChild(Width);


        //    XmlElement Page = xml.CreateElement("Page");
        //    Report.AppendChild(Page);

        //    //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

        //    if (Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
        //    {
        //        XmlElement PageHeader = xml.CreateElement("PageHeader");
        //        Page.AppendChild(PageHeader);

        //        XmlElement PageHeader_Height = xml.CreateElement("Height");
        //        PageHeader_Height.InnerText = "0.51958in";
        //        PageHeader.AppendChild(PageHeader_Height);

        //        XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        PrintOnFirstPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnFirstPage);

        //        XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        PrintOnLastPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnLastPage);


        //        XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
        //        PageHeader.AppendChild(Header_ReportItems);

        //        if (Include_Header_Title)
        //        {
        //            XmlElement Header_TextBox = xml.CreateElement("Textbox");
        //            Header_TextBox.SetAttribute("Name", "HeaderTextBox");
        //            Header_ReportItems.AppendChild(Header_TextBox);

        //            XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            HeaderTextBox_CanGrow.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

        //            XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
        //            HeaderTextBox_Keep.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_Keep);

        //            XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
        //            Header_TextBox.AppendChild(Header_Paragraphs);

        //            XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
        //            Header_Paragraphs.AppendChild(Header_Paragraph);

        //            XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
        //            Header_Paragraph.AppendChild(Header_TextRuns);

        //            XmlElement Header_TextRun = xml.CreateElement("TextRun");
        //            Header_TextRuns.AppendChild(Header_TextRun);

        //            XmlElement Header_TextRun_Value = xml.CreateElement("Value");
        //            Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
        //            Header_TextRun.AppendChild(Header_TextRun_Value);

        //            XmlElement Header_TextRun_Style = xml.CreateElement("Style");
        //            Header_TextRun.AppendChild(Header_TextRun_Style);

        //            XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
        //            Header_Style_Font.InnerText = "Times New Roman";
        //            Header_TextRun_Style.AppendChild(Header_Style_Font);

        //            XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
        //            Header_Style_FontSize.InnerText = "16pt";
        //            Header_TextRun_Style.AppendChild(Header_Style_FontSize);

        //            XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
        //            Header_Style_TextDecoration.InnerText = "Underline";
        //            Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

        //            XmlElement Header_Style_Color = xml.CreateElement("Color");
        //            Header_Style_Color.InnerText = "#104cda";
        //            Header_TextRun_Style.AppendChild(Header_Style_Color);

        //            XmlElement Header_TextBox_Top = xml.CreateElement("Top");
        //            Header_TextBox_Top.InnerText = "0.24792in";
        //            Header_TextBox.AppendChild(Header_TextBox_Top);

        //            XmlElement Header_TextBox_Left = xml.CreateElement("Left");
        //            Header_TextBox_Left.InnerText = "0.42361in";
        //            Header_TextBox.AppendChild(Header_TextBox_Left);

        //            XmlElement Header_TextBox_Height = xml.CreateElement("Height");
        //            Header_TextBox_Height.InnerText = "0.30208in";
        //            Header_TextBox.AppendChild(Header_TextBox_Height);

        //            XmlElement Header_TextBox_Width = xml.CreateElement("Width");
        //            //Header_TextBox_Width.InnerText = "10.30208in";
        //            Header_TextBox_Width.InnerText = "10in";
        //            Header_TextBox.AppendChild(Header_TextBox_Width);

        //            XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
        //            Header_TextBox_ZIndex.InnerText = "1";
        //            Header_TextBox.AppendChild(Header_TextBox_ZIndex);


        //            XmlElement Header_TextBox_Style = xml.CreateElement("Style");
        //            Header_TextBox.AppendChild(Header_TextBox_Style);

        //            XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

        //            XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Header_TB_StyleBorderStyle.InnerText = "None";
        //            Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

        //            XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Header_TB_SBS_LeftPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

        //            XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Header_TB_SBS_RightPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

        //            XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Header_TB_SBS_TopPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

        //            XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Header_TB_SBS_BotPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

        //            XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
        //            Header_Paragraph.AppendChild(Header_Text_Align_Style);

        //            XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            Header_Text_Align.InnerText = "Center";
        //            Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }

        //        //if (Include_Header_Image)
        //        //{
        //        //    // Add Image Heare
        //        //}

        //        XmlElement PageHeader_Style = xml.CreateElement("Style");
        //        PageHeader.AppendChild(PageHeader_Style);

        //        XmlElement PageHeader_Border = xml.CreateElement("Border");
        //        PageHeader_Style.AppendChild(PageHeader_Border);

        //        XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
        //        PageHeader_Border_Style.InnerText = "None";
        //        PageHeader_Border.AppendChild(PageHeader_Border_Style);


        //        XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
        //        PageHeader_BackgroundColor.InnerText = "White";
        //        PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



        //    //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    if (Include_Footer)
        //    {
        //        XmlElement PageFooter = xml.CreateElement("PageFooter");
        //        Page.AppendChild(PageFooter);

        //        XmlElement PageFooter_Height = xml.CreateElement("Height");
        //        PageFooter_Height.InnerText = "0.35083in";
        //        PageFooter.AppendChild(PageFooter_Height);

        //        XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        Footer_PrintOnFirstPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnFirstPage);

        //        XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        Footer_PrintOnLastPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnLastPage);

        //        XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
        //        PageFooter.AppendChild(Footer_ReportItems);

        //        if (Include_Footer_PageCnt)
        //        {
        //            XmlElement Footer_TextBox = xml.CreateElement("Textbox");
        //            Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
        //            Footer_ReportItems.AppendChild(Footer_TextBox);

        //            XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            FooterTextBox_CanGrow.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

        //            XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
        //            FooterTextBox_Keep.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_Keep);

        //            XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
        //            Footer_TextBox.AppendChild(Footer_Paragraphs);

        //            XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
        //            Footer_Paragraphs.AppendChild(Footer_Paragraph);

        //            XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
        //            Footer_Paragraph.AppendChild(Footer_TextRuns);

        //            XmlElement Footer_TextRun = xml.CreateElement("TextRun");
        //            Footer_TextRuns.AppendChild(Footer_TextRun);

        //            XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
        //            Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
        //            Footer_TextRun.AppendChild(Footer_TextRun_Value);

        //            XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
        //            Footer_TextRun.AppendChild(Footer_TextRun_Style);

        //            XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
        //            Footer_TextBox_Top.InnerText = "0.06944in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Top);

        //            XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
        //            Footer_TextBox_Height.InnerText = "0.25in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Height);

        //            XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
        //            Footer_TextBox_Width.InnerText = "1.65625in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Width);


        //            XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
        //            Footer_TextBox.AppendChild(Footer_TextBox_Style);

        //            XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

        //            XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Footer_TB_StyleBorderStyle.InnerText = "None";
        //            Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

        //            XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Footer_TB_SBS_LeftPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

        //            XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Footer_TB_SBS_RightPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

        //            XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Footer_TB_SBS_TopPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

        //            XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Footer_TB_SBS_BotPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

        //            XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
        //            Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

        //            //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            //Header_Text_Align.InnerText = "Center";
        //            //Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
        //    XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

        //    //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //    //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    if (false) //(Rb_A4_Port.Checked)
        //    {
        //        Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
        //        Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
        //    }
        //    else
        //    {
        //        Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //        Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    }
        //    Page.AppendChild(Page_PageHeight);
        //    Page.AppendChild(Page_PageWidth);


        //    XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
        //    Page_LeftMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_LeftMargin);

        //    XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
        //    Page_RightMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_RightMargin);

        //    XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
        //    Page_TopMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_TopMargin);

        //    XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
        //    Page_BottomMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_BottomMargin);



        //    //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

        //    //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
        //    //EmbeddedImages.InnerText = "Image Attributes";
        //    //Report.AppendChild(EmbeddedImages);

        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


        //    string s = xml.OuterXml;

        //    try
        //    {
        //        //xml.Save(@"C:\Capreports\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //        xml.Save(ReportPath + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
        //        //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //    }
        //    catch (Exception ex)
        //    { Console.WriteLine(ex.Message); }

        //    // Console.ReadLine();  //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        //}

        //****************************************************************************************************



        //*****************PerformanceMeasures_Details_Dynamic_RDLC***********************************************************************************

        //private void PerformanceMeasures_Details_Dynamic_RDLC()
        //{

        //    //Get_Report_Selection_Parameters();
        //    Get_PM_Detail_Table_Structure();

        //    XmlNode xmlnode;

        //    XmlDocument xml = new XmlDocument();
        //    xmlnode = xml.CreateNode(XmlNodeType.XmlDeclaration, "", "");
        //    xml.AppendChild(xmlnode);

        //    XmlElement Report = xml.CreateElement("Report");
        //    Report.SetAttribute("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
        //    Report.SetAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
        //    xml.AppendChild(Report);

        //    XmlElement DataSources = xml.CreateElement("DataSources");
        //    XmlElement DataSource = xml.CreateElement("DataSource");
        //    DataSource.SetAttribute("Name", "CaptainDataSource");
        //    DataSources.AppendChild(DataSource);

        //    Report.AppendChild(DataSources);

        //    XmlElement ConnectionProperties = xml.CreateElement("ConnectionProperties");
        //    DataSource.AppendChild(ConnectionProperties);

        //    XmlElement DataProvider = xml.CreateElement("DataProvider");
        //    DataProvider.InnerText = "System.Data.DataSet";


        //    XmlElement ConnectString = xml.CreateElement("ConnectString");
        //    ConnectString.InnerText = "/* Local Connection */";
        //    ConnectionProperties.AppendChild(DataProvider);
        //    ConnectionProperties.AppendChild(ConnectString);

        //    //string SourceID = "rd:DataSourceID";
        //    //XmlElement DataSourceID = xml.CreateElement(SourceID);     // Missing rd:
        //    //DataSourceID.InnerText = "d961c1ea-69f0-47db-b28e-cf07e54e65e6";
        //    //DataSource.AppendChild(DataSourceID);

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>

        //    XmlElement DataSets = xml.CreateElement("DataSets");
        //    Report.AppendChild(DataSets);

        //    XmlElement DataSet = xml.CreateElement("DataSet");
        //    DataSet.SetAttribute("Name", "ZipCodeDataset");                                             // Dynamic
        //    DataSets.AppendChild(DataSet);

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>

        //    XmlElement Fields = xml.CreateElement("Fields");
        //    DataSet.AppendChild(Fields);

        //    foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)
        //    {
        //        XmlElement Field = xml.CreateElement("Field");
        //        Field.SetAttribute("Name", Entity.Column_Name);
        //        Fields.AppendChild(Field);

        //        XmlElement DataField = xml.CreateElement("DataField");
        //        DataField.InnerText = Entity.Column_Name;
        //        Field.AppendChild(DataField);
        //    }

        //    //<<<<<<<<<<<<<<<<<<<   Fields Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>             Mandatory in DataSets Tag

        //    XmlElement Query = xml.CreateElement("Query");
        //    DataSet.AppendChild(Query);

        //    XmlElement DataSourceName = xml.CreateElement("DataSourceName");
        //    DataSourceName.InnerText = "CaptainDataSource";                                                 //Dynamic
        //    Query.AppendChild(DataSourceName);

        //    XmlElement CommandText = xml.CreateElement("CommandText");
        //    CommandText.InnerText = "/* Local Query */";
        //    Query.AppendChild(CommandText);


        //    //<<<<<<<<<<<<<<<<<<<   Query Tag     >>>>>>>>>
        //    //<<<<<<<<<<<<<<<<<<<   DataSetInfo Tag     >>>>>>>>>  Optional in DataSets Tag

        //    //<<<<<<<<<<<<<<<<<<<   DataSets Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    XmlElement Body = xml.CreateElement("Body");
        //    Report.AppendChild(Body);


        //    XmlElement ReportItems = xml.CreateElement("ReportItems");
        //    Body.AppendChild(ReportItems);

        //    XmlElement Height = xml.CreateElement("Height");
        //    //Height.InnerText = "4.15625in";       // Landscape
        //    Height.InnerText = "2in";           // Portrait
        //    Body.AppendChild(Height);


        //    XmlElement Style = xml.CreateElement("Style");
        //    Body.AppendChild(Style);

        //    XmlElement Border = xml.CreateElement("Border");
        //    Style.AppendChild(Border);

        //    XmlElement BackgroundColor = xml.CreateElement("BackgroundColor");
        //    BackgroundColor.InnerText = "White";
        //    Style.AppendChild(BackgroundColor);


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>

        //    //////XmlElement Sel_Rectangle = xml.CreateElement("Rectangle");
        //    //////Sel_Rectangle.SetAttribute("Name", "Sel_Param_Rect");
        //    //////ReportItems.AppendChild(Sel_Rectangle);

        //    //////XmlElement Sel_Rect_REPItems = xml.CreateElement("ReportItems");
        //    //////Sel_Rectangle.AppendChild(Sel_Rect_REPItems);


        //    //////double Total_Sel_TextBox_Height = 0.16667;
        //    //////string Tmp_Sel_Text = string.Empty;
        //    //////for (int i = 0; i < 22; i++)
        //    //////{
        //    //////    XmlElement Sel_Rect_Textbox1 = xml.CreateElement("Textbox");
        //    //////    Sel_Rect_Textbox1.SetAttribute("Name", "SeL_Prm_Textbox" + i.ToString());
        //    //////    Sel_Rect_REPItems.AppendChild(Sel_Rect_Textbox1);

        //    //////    XmlElement Textbox1_Cangrow = xml.CreateElement("CanGrow");
        //    //////    Textbox1_Cangrow.InnerText = "true";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Cangrow);

        //    //////    XmlElement Textbox1_Keep = xml.CreateElement("KeepTogether");
        //    //////    Textbox1_Keep.InnerText = "true";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Keep);

        //    //////    XmlElement Textbox1_Paragraphs = xml.CreateElement("Paragraphs");
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Paragraphs);

        //    //////    XmlElement Textbox1_Paragraph = xml.CreateElement("Paragraph");
        //    //////    Textbox1_Paragraphs.AppendChild(Textbox1_Paragraph);

        //    //////    XmlElement Textbox1_TextRuns = xml.CreateElement("TextRuns");
        //    //////    Textbox1_Paragraph.AppendChild(Textbox1_TextRuns);


        //    //////    XmlElement Textbox1_TextRun = xml.CreateElement("TextRun");
        //    //////    Textbox1_TextRuns.AppendChild(Textbox1_TextRun);

        //    //////    XmlElement Textbox1_TextRun_Value = xml.CreateElement("Value");

        //    //////    Tmp_Sel_Text = string.Empty;
        //    //////    switch (i)
        //    //////    {
        //    //////        case 0: Tmp_Sel_Text = "Selected Report Parameters"; break;

        //    //////        case 3: Tmp_Sel_Text = "      Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG; break;

        //    //////        case 5: Tmp_Sel_Text = "            Attributes                         :   " + (Rb_Agy_Def.Checked ? "Agency Defined" : "User Defined Associations"); break;
        //    //////        case 6: Tmp_Sel_Text = "            Case Type                       :   " + ((ListItem)Cmb_CaseType.SelectedItem).Text.ToString(); break;
        //    //////        case 7: Tmp_Sel_Text = "            Case Status                     :   " + Sel_params_To_Print[1]; break;

        //    //////        case 9: Tmp_Sel_Text = "            Date Selection                 :   " + (Rb_MS_AddDate.Checked ? "MS AddDate" : "MS Date"); break;
        //    //////        case 10: Tmp_Sel_Text = "            Reference Period Date    :   From " + Search_Entity.Rep_From_Date + "    To " + Search_Entity.Rep_To_Date; break;
        //    //////        case 11: Tmp_Sel_Text = "            Report Period Date          :   From " + Search_Entity.Rep_Period_FDate + "    To " + Search_Entity.Rep_Period_TDate; break;
        //    //////        case 12: Tmp_Sel_Text = "            Poverty Levels                 :   From " + Txt_Pov_Low.Text + "    To " + Txt_Pov_High.Text; break;

        //    //////        case 14: Tmp_Sel_Text = "            Site                                   :   " + Sel_params_To_Print[2]; break;
        //    //////        case 15: Tmp_Sel_Text = "            Groups                          :   " + (Rb_Zip_All.Checked ? "All Groups" : "Selected Groups"); break;
        //    //////        case 16: Tmp_Sel_Text = "            Report Format                          :   " + (Rb_County_All.Checked ? "Performance Measures Only" : "Performance Measures + Goal Details"); break;

        //    //////        case 18: Tmp_Sel_Text = "            Secret Applications          :   " + Sel_params_To_Print[0]; break;
        //    //////        case 19: Tmp_Sel_Text = "            Produce Stastical Details :   " + (Rb_Details_Yes.Checked ? "Yes" : "No"); break;

        //    //////        default: Tmp_Sel_Text = "  "; break;
        //    //////    }


        //    //////    Textbox1_TextRun_Value.InnerText = Tmp_Sel_Text;
        //    //////    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Value);


        //    //////    XmlElement Textbox1_TextRun_Style = xml.CreateElement("Style");
        //    //////    Textbox1_TextRun.AppendChild(Textbox1_TextRun_Style);

        //    //////    XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
        //    //////    Textbox1_TextRun_Style_Color.InnerText = "DarkViolet";
        //    //////    Textbox1_TextRun_Style.AppendChild(Textbox1_TextRun_Style_Color);


        //    //////    XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
        //    //////    Textbox1_Paragraph.AppendChild(Textbox1_Paragraph_Style);


        //    //////    XmlElement Textbox1_Top = xml.CreateElement("Top");
        //    //////    Textbox1_Top.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"0.16667in";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Top);

        //    //////    XmlElement Textbox1_Left = xml.CreateElement("Left");
        //    //////    Textbox1_Left.InnerText = "0.07292in";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Left);

        //    //////    Total_Sel_TextBox_Height += 0.21855;// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? 0.21855 : 0.01855);

        //    //////    XmlElement Textbox1_Height = xml.CreateElement("Height");
        //    //////    Textbox1_Height.InnerText = "0.21855in";// (!string.IsNullOrEmpty(Tmp_Sel_Text.Trim()) ? "0.21855in" : "0.01855in"); //"0.21875in";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Height);

        //    //////    XmlElement Textbox1_Width = xml.CreateElement("Width");
        //    //////    //Textbox1_Width.InnerText = (total_Columns_Width > 7.48777 ? "7.48777in" + "in" : "7.48777in"); // "6.35055in";
        //    //////    Textbox1_Width.InnerText = (true ? "7.48777" + "in" : "7.48777in"); // "6.35055in";
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Width);

        //    //////    XmlElement Textbox1_Style = xml.CreateElement("Style");
        //    //////    Sel_Rect_Textbox1.AppendChild(Textbox1_Style);

        //    //////    XmlElement Textbox1_Style_Border = xml.CreateElement("Border");
        //    //////    Textbox1_Style.AppendChild(Textbox1_Style_Border);

        //    //////    XmlElement Textbox1_Style_Border_Style = xml.CreateElement("Style");
        //    //////    Textbox1_Style_Border_Style.InnerText = "None";
        //    //////    Textbox1_Style_Border.AppendChild(Textbox1_Style_Border_Style);

        //    //////    XmlElement Textbox1_Style_PaddingLeft = xml.CreateElement("PaddingLeft");
        //    //////    Textbox1_Style_PaddingLeft.InnerText = "2pt";
        //    //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingLeft);

        //    //////    XmlElement Textbox1_Style_PaddingRight = xml.CreateElement("PaddingRight");
        //    //////    Textbox1_Style_PaddingRight.InnerText = "2pt";
        //    //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingRight);

        //    //////    XmlElement Textbox1_Style_PaddingTop = xml.CreateElement("PaddingTop");
        //    //////    Textbox1_Style_PaddingTop.InnerText = "2pt";
        //    //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingTop);

        //    //////    XmlElement Textbox1_Style_PaddingBottom = xml.CreateElement("PaddingBottom");
        //    //////    Textbox1_Style_PaddingTop.InnerText = "2pt";
        //    //////    Textbox1_Style.AppendChild(Textbox1_Style_PaddingBottom);

        //    //////}

        //    //////XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //    //////Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //    //////XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
        //    //////Break_After_SelParamRectangle_Location.InnerText = "End";
        //    //////Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters

        //    //////XmlElement Sel_Rectangle_KeepTogether = xml.CreateElement("KeepTogether");
        //    //////Sel_Rectangle_KeepTogether.InnerText = "true";
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_KeepTogether);

        //    //////XmlElement Sel_Rectangle_Top = xml.CreateElement("Top");
        //    //////Sel_Rectangle_Top.InnerText = "0.2008in"; //"0.2408in";
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_Top);

        //    //////XmlElement Sel_Rectangle_Left = xml.CreateElement("Left");
        //    //////Sel_Rectangle_Left.InnerText = "0.20417in"; //"0.277792in";
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_Left);

        //    //////XmlElement Sel_Rectangle_Height = xml.CreateElement("Height");
        //    //////Sel_Rectangle_Height.InnerText = Total_Sel_TextBox_Height.ToString() + "in";//"10.33333in"; 11.4
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_Height);

        //    //////XmlElement Sel_Rectangle_Width = xml.CreateElement("Width");
        //    ////////Sel_Rectangle_Width.InnerText = (total_Columns_Width > 7.5 ? total_Columns_Width.ToString() + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    //////Sel_Rectangle_Width.InnerText = (true ? "10" + "in" : "7.5in");//total_Columns_Width.ToString() + "in";//"6.72555in";
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_Width);

        //    //////XmlElement Sel_Rectangle_ZIndex = xml.CreateElement("ZIndex");
        //    //////Sel_Rectangle_ZIndex.InnerText = "1";
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_ZIndex);

        //    //////XmlElement Sel_Rectangle_Style = xml.CreateElement("Style");
        //    //////Sel_Rectangle.AppendChild(Sel_Rectangle_Style);

        //    //////XmlElement Sel_Rectangle_Style_Border = xml.CreateElement("Border");
        //    //////Sel_Rectangle_Style.AppendChild(Sel_Rectangle_Style_Border);

        //    //////XmlElement Sel_Rectangle_Style_Border_Style = xml.CreateElement("Style");
        //    //////Sel_Rectangle_Style_Border_Style.InnerText = "Solid";//"None";
        //    //////Sel_Rectangle_Style_Border.AppendChild(Sel_Rectangle_Style_Border_Style);

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems Childs   Selection Parameters">>>>>>>>>>>>>>>>>>>>>>>>>>



        //    XmlElement Tablix = xml.CreateElement("Tablix");
        //    Tablix.SetAttribute("Name", "Tablix3");
        //    ReportItems.AppendChild(Tablix);

        //    XmlElement TablixBody = xml.CreateElement("TablixBody");
        //    Tablix.AppendChild(TablixBody);


        //    XmlElement TablixColumns = xml.CreateElement("TablixColumns");
        //    TablixBody.AppendChild(TablixColumns);

        //    foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)                      // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")
        //        {
        //            XmlElement TablixColumn = xml.CreateElement("TablixColumn");
        //            TablixColumns.AppendChild(TablixColumn);

        //            XmlElement Col_Width = xml.CreateElement("Width");
        //            //Col_Width.InnerText = Entity.Max_Display_Width.Trim();        // Dynamic based on Display Columns Width
        //            //Col_Width.InnerText = "4in";        // Dynamic based on Display Columns Width
        //            Col_Width.InnerText = Entity.Disp_Width;
        //            TablixColumn.AppendChild(Col_Width);
        //        }
        //    }

        //    XmlElement TablixRows = xml.CreateElement("TablixRows");
        //    TablixBody.AppendChild(TablixRows);

        //    XmlElement TablixRow = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow);

        //    XmlElement Row_Height = xml.CreateElement("Height");
        //    //Row_Height.InnerText = "0.25in";
        //    Row_Height.InnerText = "0.0000001in";
        //    TablixRow.AppendChild(Row_Height);

        //    XmlElement Row_TablixCells = xml.CreateElement("TablixCells");
        //    TablixRow.AppendChild(Row_TablixCells);


        //    int Tmp_Loop_Cnt = 0, Disp_Col_Substring_Len = 0;
        //    string Tmp_Disp_Column_Name = " ", Field_type = "Textbox";
        //    foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)            // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            //Entity.Column_Name;
        //            Tmp_Loop_Cnt++;

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells.AppendChild(TablixCell);


        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            //if (Entity.Col_Format_Type == "C")
        //            //    Field_type = "Checkbox";

        //            XmlElement Textbox = xml.CreateElement(Field_type);
        //            Textbox.SetAttribute("Name", "Textbox" + Tmp_Loop_Cnt.ToString());
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);



        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);



        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");

        //            Tmp_Disp_Column_Name = Entity.Disp_Name;


        //            //Disp_Col_Substring_Len = 6;

        //            //Return_Value.InnerText = Tmp_Disp_Column_Name.Substring(0, (Tmp_Disp_Column_Name.Length < Disp_Col_Substring_Len ? Tmp_Disp_Column_Name.Length : Disp_Col_Substring_Len));                                    // Dynamic Column Heading
        //            Return_Value.InnerText = Entity.Disp_Name;                                    // Dynamic Column Heading
        //            TextRun.AppendChild(Return_Value);


        //            XmlElement Cell_Align = xml.CreateElement("Style");
        //            XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Header Cell Text Align
        //            Cell_TextAlign.InnerText = "Center";
        //            Cell_Align.AppendChild(Cell_TextAlign);
        //            Paragraph.AppendChild(Cell_Align);


        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);

        //            ////XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //            ////Return_Style_FontWeight.InnerText = "Bold";
        //            ////Return_Style.AppendChild(Return_Style_FontWeight);


        //            //XmlElement Return_AlignStyle = xml.CreateElement("Style");
        //            //Paragraph.AppendChild(Return_AlignStyle);

        //            //XmlElement DefaultName = xml.CreateElement("rd:DefaultName");     // rd:DefaultName is Optional
        //            //DefaultName.InnerText = "Textbox" + i.ToString();
        //            //Textbox.AppendChild(DefaultName);


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);


        //            //////XmlElement Cell_Border = xml.CreateElement("Border");
        //            //////Cell_style.AppendChild(Cell_Border);

        //            //////XmlElement Border_Color = xml.CreateElement("Color");
        //            //////Border_Color.InnerText = "Black";//"LightGrey";
        //            //////Cell_Border.AppendChild(Border_Color);

        //            //////XmlElement Border_Style = xml.CreateElement("Style");       // Header Border Style
        //            //////Border_Style.InnerText = "Solid";
        //            //////Cell_Border.AppendChild(Border_Style);

        //            //////XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            //////Cell_Style_BackColor.InnerText = "LightSteelBlue";
        //            //////Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);
        //        }
        //    }




        //    XmlElement TablixRow2 = xml.CreateElement("TablixRow");
        //    TablixRows.AppendChild(TablixRow2);

        //    XmlElement Row_Height2 = xml.CreateElement("Height");
        //    Row_Height2.InnerText = "0.175in";
        //    //Row_Height2.InnerText = "0.2in";
        //    TablixRow2.AppendChild(Row_Height2);

        //    XmlElement Row_TablixCells2 = xml.CreateElement("TablixCells");
        //    TablixRow2.AppendChild(Row_TablixCells2);

        //    string Format_Style_String = string.Empty, Field_Value = string.Empty, Text_Align = string.Empty, Temporary_Field_Value = string.Empty;
        //    char Tmp_Double_Codes = '"';
        //    foreach (DG_ResTab_Entity Entity in PM_Detail_Table_List)        // Dynamic based on Display Columns in Result Table
        //    {
        //        if (Entity.Can_Add == "Y")   // 09062012
        //        {

        //            XmlElement TablixCell = xml.CreateElement("TablixCell");
        //            Row_TablixCells2.AppendChild(TablixCell);

        //            XmlElement CellContents = xml.CreateElement("CellContents");
        //            TablixCell.AppendChild(CellContents);

        //            XmlElement Textbox = xml.CreateElement("Textbox");
        //            Textbox.SetAttribute("Name", Entity.Column_Name);
        //            CellContents.AppendChild(Textbox);

        //            XmlElement CanGrow = xml.CreateElement("CanGrow");
        //            CanGrow.InnerText = "true";
        //            Textbox.AppendChild(CanGrow);

        //            XmlElement KeepTogether = xml.CreateElement("KeepTogether");
        //            KeepTogether.InnerText = "true";
        //            Textbox.AppendChild(KeepTogether);

        //            XmlElement Paragraphs = xml.CreateElement("Paragraphs");
        //            Textbox.AppendChild(Paragraphs);

        //            XmlElement Paragraph = xml.CreateElement("Paragraph");
        //            Paragraphs.AppendChild(Paragraph);

        //            XmlElement TextRuns = xml.CreateElement("TextRuns");
        //            Paragraph.AppendChild(TextRuns);

        //            XmlElement TextRun = xml.CreateElement("TextRun");
        //            TextRuns.AppendChild(TextRun);

        //            XmlElement Return_Value = xml.CreateElement("Value");


        //            Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
        //            switch (Entity.Column_Name)                         //
        //            {

        //                //case "SortUnDup_Group": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
        //                //                                    ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + " , Fields!SortUnDup_Group.Value)";
        //                //    break;

        //                //case "SortUnDup_Table": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
        //                //                                    ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + "  , Fields!SortUnDup_Table.Value)";
        //                //    break;

        //                //case "SortUnDup_Agy": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
        //                //                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
        //                //                                    ", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
        //                //                                    " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
        //                //                                    " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes +
        //                //                                    " + Fields!SortUnDup_App.Value  + " + Tmp_Double_Codes + "      " + Tmp_Double_Codes +
        //                //                                    " + Fields!SortUnDup_Name.Value)";

        //                case "SortUnDup_Agy": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
        //                                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
        //                                                     " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
        //                                                    ", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Group.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_Table.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_App.Value  + " + Tmp_Double_Codes + "      " + Tmp_Double_Codes +
        //                                                    " + Fields!SortUnDup_Name.Value)";
        //                    break;

        //                case "SortUnDup_OutCome_Date": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + " Activity Date" + Tmp_Double_Codes +
        //                                                              ", Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + "))"; break;

        //                case "R1": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + "R1" + Tmp_Double_Codes +
        //                                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
        //                case "R2": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + "R2" + Tmp_Double_Codes +
        //                                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
        //                case "R3": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + "R3" + Tmp_Double_Codes +
        //                                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
        //                case "R4": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + "R4" + Tmp_Double_Codes +
        //                                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
        //                case "R5": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
        //                                                              Tmp_Double_Codes + "R5" + Tmp_Double_Codes +
        //                                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
        //            }

        //            //Field_Value = "=Fields!" + Entity.Column_Name + ".Value";
        //            Format_Style_String = Text_Align = Temporary_Field_Value = string.Empty;
        //            Text_Align = "Left";
        //            switch (Entity.Text_Align)  // (Entity.Column_Disp_Name)
        //            {
        //                case "R":
        //                    Text_Align = "Right"; break;
        //            }

        //            Return_Value.InnerText = Field_Value;
        //            TextRun.AppendChild(Return_Value);

        //            XmlElement Return_Style = xml.CreateElement("Style");
        //            TextRun.AppendChild(Return_Style);

        //            // New
        //            // Commented on 04022015 to Stop Color
        //            //XmlElement Textbox1_TextRun_Style_Color = xml.CreateElement("Color");   // Text Color
        //            //Textbox1_TextRun_Style_Color.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "#2D17EB" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + ")";
        //            ////Textbox1_TextRun_Style_Color.InnerText = "Blue";
        //            //Return_Style.AppendChild(Textbox1_TextRun_Style_Color);


        //            ////////XmlElement Textbox1_Paragraph_Style = xml.CreateElement("Style");
        //            ////////Paragraph.AppendChild(Textbox1_Paragraph_Style);

        //            //////// New


        //            ////////if (Entity.Column_Name == "Res_Table_Desc"  ) // 11292012
        //            ////////{
        //            //XmlElement Return_Style_FontWeight = xml.CreateElement("FontWeight");
        //            //// Commented on 04022015 to Stop Bold
        //            ////Return_Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + " OR Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
        //            ////                                                                        " OR Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
        //            //Return_Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + ")";
        //            //Return_Style.AppendChild(Return_Style_FontWeight);
        //            ////////}



        //            if (!string.IsNullOrEmpty(Text_Align))
        //            {
        //                XmlElement Cell_Align = xml.CreateElement("Style");
        //                XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
        //                //Cell_TextAlign.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Left" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
        //                Cell_TextAlign.InnerText = Text_Align;
        //                Cell_Align.AppendChild(Cell_TextAlign);
        //                Paragraph.AppendChild(Cell_Align);
        //            }


        //            XmlElement Cell_style = xml.CreateElement("Style");
        //            Textbox.AppendChild(Cell_style);

        //            XmlElement Cell_Border = xml.CreateElement("Border");
        //            Cell_style.AppendChild(Cell_Border);

        //            XmlElement Border_Color = xml.CreateElement("Color");
        //            //Border_Color.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Black" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + ")";
        //            Border_Color.InnerText = "LightGrey";
        //            Cell_Border.AppendChild(Border_Color);

        //            XmlElement Border_Style = xml.CreateElement("Style");    // Repeating Cell Border Style
        //            Border_Style.InnerText = "None";
        //            Cell_Border.AppendChild(Border_Style);


        //            // Commented on 04022015 to Stop Background Color
        //            //XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
        //            //Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightGrey" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
        //            ////Cell_Style_BackColor.InnerText = "Blue";
        //            //Cell_style.AppendChild(Cell_Style_BackColor);  // Yeswanth

        //            XmlElement PaddingLeft = xml.CreateElement("PaddingLeft");
        //            PaddingLeft.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingLeft);

        //            XmlElement PaddingRight = xml.CreateElement("PaddingRight");
        //            PaddingRight.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingRight);

        //            XmlElement PaddingTop = xml.CreateElement("PaddingTop");
        //            PaddingTop.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingTop);

        //            XmlElement PaddingBottom = xml.CreateElement("PaddingBottom");
        //            PaddingBottom.InnerText = "2pt";
        //            Cell_style.AppendChild(PaddingBottom);

        //            //XmlElement ColSpan = xml.CreateElement("ColSpan");
        //            ////ColSpan.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "1" + "," + "9" + ")";
        //            //ColSpan.InnerText = "0";
        //            //CellContents.AppendChild(ColSpan);

        //            ////if (Entity.Column_Name == "Res_Table_Desc") // 11292012
        //            ////{

        //            ////    XmlElement Break_before_Group = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //            ////    Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //            ////    XmlElement Break_before_Group = xml.CreateElement("BreakLocation");
        //            ////    Break_After_SelParamRectangle_Location.InnerText = "End";
        //            ////    Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters
        //            ////}






        //        }
        //    }



        //    //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
        //    //TablixBody.AppendChild(SubReport_PageBreak);

        //    //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
        //    //SubReport_PageBreak_Location.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "End" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "None" + Tmp_Double_Codes + ")";
        //    ////SubReport_PageBreak_Location.InnerText = "End";
        //    //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);



        //    XmlElement TablixColumnHierarchy = xml.CreateElement("TablixColumnHierarchy");
        //    Tablix.AppendChild(TablixColumnHierarchy);

        //    XmlElement Tablix_Col_Members = xml.CreateElement("TablixMembers");
        //    TablixColumnHierarchy.AppendChild(Tablix_Col_Members);

        //    for (int Loop = 0; Loop < 8; Loop++)            // Dynamic based on Display Columns in 3/6
        //    {
        //        XmlElement TablixMember = xml.CreateElement("TablixMember");
        //        Tablix_Col_Members.AppendChild(TablixMember);
        //    }


        //    XmlElement TablixRowHierarchy = xml.CreateElement("TablixRowHierarchy");
        //    Tablix.AppendChild(TablixRowHierarchy);

        //    XmlElement Tablix_Row_Members = xml.CreateElement("TablixMembers");
        //    TablixRowHierarchy.AppendChild(Tablix_Row_Members);

        //    XmlElement Tablix_Row_Member = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member);

        //    XmlElement FixedData = xml.CreateElement("FixedData");
        //    FixedData.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(FixedData);

        //    XmlElement KeepWithGroup = xml.CreateElement("KeepWithGroup");
        //    KeepWithGroup.InnerText = "After";
        //    Tablix_Row_Member.AppendChild(KeepWithGroup);

        //    XmlElement RepeatOnNewPage = xml.CreateElement("RepeatOnNewPage");
        //    RepeatOnNewPage.InnerText = "true";
        //    Tablix_Row_Member.AppendChild(RepeatOnNewPage);

        //    XmlElement Tablix_Row_Member1 = xml.CreateElement("TablixMember");
        //    Tablix_Row_Members.AppendChild(Tablix_Row_Member1);

        //    XmlElement Group = xml.CreateElement("Group"); // 5656565656
        //    Group.SetAttribute("Name", "Details1");
        //    Tablix_Row_Member1.AppendChild(Group);

        //    //XmlElement Group_Exps = xml.CreateElement("GroupExpressions"); // 5656565656
        //    //Group.AppendChild(Group_Exps);

        //    //XmlElement Group_Exp = xml.CreateElement("GroupExpression"); // 5656565656
        //    //Group_Exp.InnerText = "=Fields!Res_Group.Value+Fields!Res_Table_Desc.Value";
        //    //Group_Exps.AppendChild(Group_Exp);

        //    //XmlElement Group_Exp_Break = xml.CreateElement("PageBreak"); // 5656565656
        //    ////Group_Exp.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + ")";
        //    //Group.AppendChild(Group_Exp_Break);

        //    //XmlElement Group_Exp_Break_Loc = xml.CreateElement("BreakLocation"); // 5656565656
        //    ////Group_Exp_Break_Loc.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + "Between" + "," + "None" + ")";
        //    //Group_Exp_Break_Loc.InnerText = "Between";
        //    //Group_Exp_Break.AppendChild(Group_Exp_Break_Loc);

        //    XmlElement RepeatRowHeaders = xml.CreateElement("RepeatRowHeaders");
        //    RepeatRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(RepeatRowHeaders);

        //    XmlElement FixedRowHeaders = xml.CreateElement("FixedRowHeaders");
        //    FixedRowHeaders.InnerText = "true";
        //    Tablix.AppendChild(FixedRowHeaders);

        //    XmlElement DataSetName1 = xml.CreateElement("DataSetName");
        //    DataSetName1.InnerText = "ZipCodeDataset";          //Dynamic
        //    Tablix.AppendChild(DataSetName1);

        //    //XmlElement SubReport_PageBreak = xml.CreateElement("PageBreak");   // Commented By Yeswanth on 01182013 
        //    //Tablix.AppendChild(SubReport_PageBreak);

        //    //XmlElement SubReport_PageBreak_Location = xml.CreateElement("BreakLocation");
        //    //SubReport_PageBreak_Location.InnerText = "StartAndEnd";
        //    //SubReport_PageBreak.AppendChild(SubReport_PageBreak_Location);

        //    XmlElement SortExpressions = xml.CreateElement("SortExpressions");
        //    Tablix.AppendChild(SortExpressions);

        //    XmlElement SortExpression = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression);

        //    XmlElement SortExpression_Value = xml.CreateElement("Value");
        //    //SortExpression_Value.InnerText = "Fields!ZCR_STATE.Value";
        //    SortExpression_Value.InnerText = "Fields!MST_AGENCY.Value";

        //    SortExpression.AppendChild(SortExpression_Value);

        //    XmlElement SortExpression_Direction = xml.CreateElement("Direction");
        //    SortExpression_Direction.InnerText = "Descending";
        //    SortExpression.AppendChild(SortExpression_Direction);


        //    XmlElement SortExpression1 = xml.CreateElement("SortExpression");
        //    SortExpressions.AppendChild(SortExpression1);

        //    XmlElement SortExpression_Value1 = xml.CreateElement("Value");
        //    //SortExpression_Value1.InnerText = "Fields!ZCR_CITY.Value";
        //    SortExpression_Value1.InnerText = "Fields!MST_DEPT.Value";
        //    SortExpression1.AppendChild(SortExpression_Value1);


        //    XmlElement Top = xml.CreateElement("Top");
        //    //Top.InnerText = (Total_Sel_TextBox_Height + .5).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
        //    Top.InnerText = ( 0.01).ToString() + "in";//10.99999in";  //"0.20417in";   10092012 Adjusted for Selected Parameters
        //    //Top.InnerText = "0.60417in";
        //    Tablix.AppendChild(Top);

        //    XmlElement Left = xml.CreateElement("Left");
        //    Left.InnerText = "0.20417in";
        //    //Left.InnerText = "0.60417in";
        //    Tablix.AppendChild(Left);

        //    XmlElement Height1 = xml.CreateElement("Height");
        //    Height1.InnerText = "0.5in";
        //    Tablix.AppendChild(Height1);

        //    XmlElement Width1 = xml.CreateElement("Width");
        //    Width1.InnerText = "5.3229in";
        //    Tablix.AppendChild(Width1);


        //    XmlElement Style10 = xml.CreateElement("Style");
        //    Tablix.AppendChild(Style10);

        //    XmlElement Style10_Border = xml.CreateElement("Border");
        //    Style10.AppendChild(Style10_Border);

        //    XmlElement Style10_Border_Style = xml.CreateElement("Style");
        //    Style10_Border_Style.InnerText = "None";
        //    Style10_Border.AppendChild(Style10_Border_Style);


        //    //XmlElement Break_After_SelParamRectangle = xml.CreateElement("PageBreak");    // Start Page break After Selectio Parameters
        //    //Sel_Rectangle.AppendChild(Break_After_SelParamRectangle);

        //    //XmlElement Break_After_SelParamRectangle_Location = xml.CreateElement("BreakLocation");
        //    //Break_After_SelParamRectangle_Location.InnerText = "End";
        //    //Break_After_SelParamRectangle.AppendChild(Break_After_SelParamRectangle_Location);  // End Page break After Selectio Parameters


        //    //   Subreport
        //    ////////if (Summary_Sw)
        //    ////////{
        //    ////////    // Summary Sub Report 
        //    ////////}

        //    //<<<<<<<<<<<<<<<<<<<<< "ReportItems" Childs   >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    //<<<<<<<<<<<<<<<<<<<   Body Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   Width Tag     >>>>>>>>>

        //    XmlElement Width = xml.CreateElement("Width");               // Total Page Width
        //    Width.InnerText = "6.5in";      //Common
        //    //if(Rb_A4_Port.Checked)
        //    //    Width.InnerText = "8.27in";      //Portrait "A4"
        //    //else
        //    //    Width.InnerText = "11in";      //Landscape "A4"
        //    Report.AppendChild(Width);


        //    XmlElement Page = xml.CreateElement("Page");
        //    Report.AppendChild(Page);

        //    //<<<<<<<<<<<<<<<<<  Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>   09162012

        //    if (Include_header && !string.IsNullOrEmpty(Rep_Header_Title.Trim()))
        //    {
        //        XmlElement PageHeader = xml.CreateElement("PageHeader");
        //        Page.AppendChild(PageHeader);

        //        XmlElement PageHeader_Height = xml.CreateElement("Height");
        //        PageHeader_Height.InnerText = "0.51958in";
        //        PageHeader.AppendChild(PageHeader_Height);

        //        XmlElement PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        PrintOnFirstPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnFirstPage);

        //        XmlElement PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        PrintOnLastPage.InnerText = "true";
        //        PageHeader.AppendChild(PrintOnLastPage);


        //        XmlElement Header_ReportItems = xml.CreateElement("ReportItems");
        //        PageHeader.AppendChild(Header_ReportItems);

        //        if (Include_Header_Title )
        //        {
        //            XmlElement Header_TextBox = xml.CreateElement("Textbox");
        //            Header_TextBox.SetAttribute("Name", "HeaderTextBox");
        //            Header_ReportItems.AppendChild(Header_TextBox);

        //            XmlElement HeaderTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            HeaderTextBox_CanGrow.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_CanGrow);

        //            XmlElement HeaderTextBox_Keep = xml.CreateElement("KeepTogether");
        //            HeaderTextBox_Keep.InnerText = "true";
        //            Header_TextBox.AppendChild(HeaderTextBox_Keep);

        //            XmlElement Header_Paragraphs = xml.CreateElement("Paragraphs");
        //            Header_TextBox.AppendChild(Header_Paragraphs);

        //            XmlElement Header_Paragraph = xml.CreateElement("Paragraph");
        //            Header_Paragraphs.AppendChild(Header_Paragraph);

        //            XmlElement Header_TextRuns = xml.CreateElement("TextRuns");
        //            Header_Paragraph.AppendChild(Header_TextRuns);

        //            XmlElement Header_TextRun = xml.CreateElement("TextRun");
        //            Header_TextRuns.AppendChild(Header_TextRun);

        //            XmlElement Header_TextRun_Value = xml.CreateElement("Value");
        //            Header_TextRun_Value.InnerText = Rep_Header_Title;   // Dynamic Report Name
        //            Header_TextRun.AppendChild(Header_TextRun_Value);

        //            XmlElement Header_TextRun_Style = xml.CreateElement("Style");
        //            Header_TextRun.AppendChild(Header_TextRun_Style);

        //            XmlElement Header_Style_Font = xml.CreateElement("FontFamily");
        //            Header_Style_Font.InnerText = "Times New Roman";
        //            Header_TextRun_Style.AppendChild(Header_Style_Font);

        //            XmlElement Header_Style_FontSize = xml.CreateElement("FontSize");
        //            Header_Style_FontSize.InnerText = "16pt";
        //            Header_TextRun_Style.AppendChild(Header_Style_FontSize);

        //            XmlElement Header_Style_TextDecoration = xml.CreateElement("TextDecoration");
        //            Header_Style_TextDecoration.InnerText = "Underline";
        //            Header_TextRun_Style.AppendChild(Header_Style_TextDecoration);

        //            XmlElement Header_Style_Color = xml.CreateElement("Color");
        //            Header_Style_Color.InnerText = "#104cda";
        //            Header_TextRun_Style.AppendChild(Header_Style_Color);

        //            XmlElement Header_TextBox_Top = xml.CreateElement("Top");
        //            Header_TextBox_Top.InnerText = "0.24792in";
        //            Header_TextBox.AppendChild(Header_TextBox_Top);

        //            XmlElement Header_TextBox_Left = xml.CreateElement("Left");
        //            Header_TextBox_Left.InnerText = "0.42361in";
        //            Header_TextBox.AppendChild(Header_TextBox_Left);

        //            XmlElement Header_TextBox_Height = xml.CreateElement("Height");
        //            Header_TextBox_Height.InnerText = "0.30208in";
        //            Header_TextBox.AppendChild(Header_TextBox_Height);

        //            XmlElement Header_TextBox_Width = xml.CreateElement("Width");
        //            //Header_TextBox_Width.InnerText = "10.30208in";
        //            Header_TextBox_Width.InnerText = "10in";
        //            Header_TextBox.AppendChild(Header_TextBox_Width);

        //            XmlElement Header_TextBox_ZIndex = xml.CreateElement("ZIndex");
        //            Header_TextBox_ZIndex.InnerText = "1";
        //            Header_TextBox.AppendChild(Header_TextBox_ZIndex);


        //            XmlElement Header_TextBox_Style = xml.CreateElement("Style");
        //            Header_TextBox.AppendChild(Header_TextBox_Style);

        //            XmlElement Header_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Header_TextBox_Style.AppendChild(Header_TextBox_StyleBorder);

        //            XmlElement Header_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Header_TB_StyleBorderStyle.InnerText = "None";
        //            Header_TextBox_StyleBorder.AppendChild(Header_TB_StyleBorderStyle);

        //            XmlElement Header_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Header_TB_SBS_LeftPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_LeftPad);

        //            XmlElement Header_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Header_TB_SBS_RightPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_RightPad);

        //            XmlElement Header_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Header_TB_SBS_TopPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_TopPad);

        //            XmlElement Header_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Header_TB_SBS_BotPad.InnerText = "2pt";
        //            Header_TextBox_Style.AppendChild(Header_TB_SBS_BotPad);

        //            XmlElement Header_Text_Align_Style = xml.CreateElement("Style");
        //            Header_Paragraph.AppendChild(Header_Text_Align_Style);

        //            XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            Header_Text_Align.InnerText = "Center";
        //            Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }

        //        //if (Include_Header_Image)
        //        //{
        //        //    // Add Image Heare
        //        //}

        //        XmlElement PageHeader_Style = xml.CreateElement("Style");
        //        PageHeader.AppendChild(PageHeader_Style);

        //        XmlElement PageHeader_Border = xml.CreateElement("Border");
        //        PageHeader_Style.AppendChild(PageHeader_Border);

        //        XmlElement PageHeader_Border_Style = xml.CreateElement("Style");
        //        PageHeader_Border_Style.InnerText = "None";
        //        PageHeader_Border.AppendChild(PageHeader_Border_Style);


        //        XmlElement PageHeader_BackgroundColor = xml.CreateElement("BackgroundColor");
        //        PageHeader_BackgroundColor.InnerText = "White";
        //        PageHeader_Style.AppendChild(PageHeader_BackgroundColor);
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Heading Text                >>>>>>>>>>>>>>>>>>>>>>>>>>



        //    //<<<<<<<<<<<<<<<<<  Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>

        //    if (Include_Footer)
        //    {
        //        XmlElement PageFooter = xml.CreateElement("PageFooter");
        //        Page.AppendChild(PageFooter);

        //        XmlElement PageFooter_Height = xml.CreateElement("Height");
        //        PageFooter_Height.InnerText = "0.35083in";
        //        PageFooter.AppendChild(PageFooter_Height);

        //        XmlElement Footer_PrintOnFirstPage = xml.CreateElement("PrintOnFirstPage");
        //        Footer_PrintOnFirstPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnFirstPage);

        //        XmlElement Footer_PrintOnLastPage = xml.CreateElement("PrintOnLastPage");
        //        Footer_PrintOnLastPage.InnerText = "true";
        //        PageFooter.AppendChild(Footer_PrintOnLastPage);

        //        XmlElement Footer_ReportItems = xml.CreateElement("ReportItems");
        //        PageFooter.AppendChild(Footer_ReportItems);

        //        if (Include_Footer_PageCnt)
        //        {
        //            XmlElement Footer_TextBox = xml.CreateElement("Textbox");
        //            Footer_TextBox.SetAttribute("Name", "FooterTextBox1");
        //            Footer_ReportItems.AppendChild(Footer_TextBox);

        //            XmlElement FooterTextBox_CanGrow = xml.CreateElement("CanGrow");
        //            FooterTextBox_CanGrow.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_CanGrow);

        //            XmlElement FooterTextBox_Keep = xml.CreateElement("KeepTogether");
        //            FooterTextBox_Keep.InnerText = "true";
        //            Footer_TextBox.AppendChild(FooterTextBox_Keep);

        //            XmlElement Footer_Paragraphs = xml.CreateElement("Paragraphs");
        //            Footer_TextBox.AppendChild(Footer_Paragraphs);

        //            XmlElement Footer_Paragraph = xml.CreateElement("Paragraph");
        //            Footer_Paragraphs.AppendChild(Footer_Paragraph);

        //            XmlElement Footer_TextRuns = xml.CreateElement("TextRuns");
        //            Footer_Paragraph.AppendChild(Footer_TextRuns);

        //            XmlElement Footer_TextRun = xml.CreateElement("TextRun");
        //            Footer_TextRuns.AppendChild(Footer_TextRun);

        //            XmlElement Footer_TextRun_Value = xml.CreateElement("Value");
        //            Footer_TextRun_Value.InnerText = "=Globals!ExecutionTime";   // Dynamic Report Name
        //            Footer_TextRun.AppendChild(Footer_TextRun_Value);

        //            XmlElement Footer_TextRun_Style = xml.CreateElement("Style");
        //            Footer_TextRun.AppendChild(Footer_TextRun_Style);

        //            XmlElement Footer_TextBox_Top = xml.CreateElement("Top");
        //            Footer_TextBox_Top.InnerText = "0.06944in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Top);

        //            XmlElement Footer_TextBox_Height = xml.CreateElement("Height");
        //            Footer_TextBox_Height.InnerText = "0.25in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Height);

        //            XmlElement Footer_TextBox_Width = xml.CreateElement("Width");
        //            Footer_TextBox_Width.InnerText = "1.65625in";
        //            Footer_TextBox.AppendChild(Footer_TextBox_Width);


        //            XmlElement Footer_TextBox_Style = xml.CreateElement("Style");
        //            Footer_TextBox.AppendChild(Footer_TextBox_Style);

        //            XmlElement Footer_TextBox_StyleBorder = xml.CreateElement("Border");
        //            Footer_TextBox_Style.AppendChild(Footer_TextBox_StyleBorder);

        //            XmlElement Footer_TB_StyleBorderStyle = xml.CreateElement("Style");
        //            Footer_TB_StyleBorderStyle.InnerText = "None";
        //            Footer_TextBox_StyleBorder.AppendChild(Footer_TB_StyleBorderStyle);

        //            XmlElement Footer_TB_SBS_LeftPad = xml.CreateElement("PaddingLeft");
        //            Footer_TB_SBS_LeftPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_LeftPad);

        //            XmlElement Footer_TB_SBS_RightPad = xml.CreateElement("PaddingRight");
        //            Footer_TB_SBS_RightPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_RightPad);

        //            XmlElement Footer_TB_SBS_TopPad = xml.CreateElement("PaddingTop");
        //            Footer_TB_SBS_TopPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_TopPad);

        //            XmlElement Footer_TB_SBS_BotPad = xml.CreateElement("PaddingBottom");
        //            Footer_TB_SBS_BotPad.InnerText = "2pt";
        //            Footer_TextBox_Style.AppendChild(Footer_TB_SBS_BotPad);

        //            XmlElement Footer_Text_Align_Style = xml.CreateElement("Style");
        //            Footer_Paragraph.AppendChild(Footer_Text_Align_Style);

        //            //XmlElement Header_Text_Align = xml.CreateElement("TextAlign");
        //            //Header_Text_Align.InnerText = "Center";
        //            //Header_Text_Align_Style.AppendChild(Header_Text_Align);
        //        }
        //    }


        //    //<<<<<<<<<<<<<<<<<  End of Footer Text                >>>>>>>>>>>>>>>>>>>>>>>>>>


        //    XmlElement Page_PageHeight = xml.CreateElement("PageHeight");
        //    XmlElement Page_PageWidth = xml.CreateElement("PageWidth");

        //    //Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //    //Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    if (false) //(Rb_A4_Port.Checked)
        //    {
        //        Page_PageHeight.InnerText = "11.69in";            // Portrait  "A4"
        //        Page_PageWidth.InnerText = "8.27in";              // Portrait "A4"
        //    }
        //    else
        //    {
        //        Page_PageHeight.InnerText = "8.5in";            // Landscape  "A4"
        //        Page_PageWidth.InnerText = "11in";            // Landscape "A4"
        //    }
        //    Page.AppendChild(Page_PageHeight);
        //    Page.AppendChild(Page_PageWidth);


        //    XmlElement Page_LeftMargin = xml.CreateElement("LeftMargin");
        //    Page_LeftMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_LeftMargin);

        //    XmlElement Page_RightMargin = xml.CreateElement("RightMargin");
        //    Page_RightMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_RightMargin);

        //    XmlElement Page_TopMargin = xml.CreateElement("TopMargin");
        //    Page_TopMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_TopMargin);

        //    XmlElement Page_BottomMargin = xml.CreateElement("BottomMargin");
        //    Page_BottomMargin.InnerText = "0.2in";
        //    Page.AppendChild(Page_BottomMargin);



        //    //<<<<<<<<<<<<<<<<<<<   Page Tag     >>>>>>>>>


        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>

        //    //XmlElement EmbeddedImages = xml.CreateElement("EmbeddedImages");
        //    //EmbeddedImages.InnerText = "Image Attributes";
        //    //Report.AppendChild(EmbeddedImages);

        //    //<<<<<<<<<<<<<<<<<<<   EmbeddedImages Tag     >>>>>>>>>


        //    string s = xml.OuterXml;

        //    try
        //    {
        //        //xml.Save(@"C:\Capreports\" + Main_Rep_Name + "Detail_RdlC.rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //        xml.Save(ReportPath + Main_Rep_Name + "Detail_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc"); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
                
        //        //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
        //    }
        //    catch (Exception ex)
        //    { Console.WriteLine(ex.Message); }

        //    // Console.ReadLine();  //Kranthi 02/15/2023: This line is taking too much time to read unknow line to read. 
        //}

        private void Rb_Zip_All_CheckedChanged(object sender, EventArgs e)
        {
            switch (Scr_Oper_Mode)
            {
                case "RNGB0004": ListZipCode.Clear(); break;
                case "CASB0014": ListGroupCode.Clear(); break;
            }
        }

        private void rdomsNosite_CheckedChanged(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            if (Rb_Process_CA.Checked || Rb_Process_MS.Checked)
            {
                if(ListcaseMsSiteEntity.Count>0)
                    ListcaseMsSiteEntity.Clear();
            }
            //    ListcaseActSiteEntity.Clear();
            //else if(Rb_Process_MS.Checked)
                
        }

        private void rdoMsselectsite_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoMsselectsite_Click(object sender, EventArgs e)
        {
            if (rdoMsselectsite.Checked == true)
            {
                if (Rb_Process_MS.Checked || Rb_Process_CA.Checked)
                {
                    SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseMsSiteEntity, strAgency, strDept, strProg, string.Empty);
                    siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyMSFormClosed);
                    siteform.StartPosition = FormStartPosition.CenterScreen;
                    siteform.ShowDialog();
                }
                //else if (Rb_Process_CA.Checked)
                //{
                //    SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseActSiteEntity, strAgency, strDept, strProg, string.Empty);
                //    siteform.FormClosed += new Form.FormClosedEventHandler(SelectZipSiteCountyMSFormClosed);
                //    siteform.ShowDialog();
                //}
            }
        }

        private void rdoMssiteall_Click(object sender, EventArgs e)
        {
            txt_Msselect_site.Clear();
            if (Rb_Process_CA.Checked || Rb_Process_MS.Checked)
            //    ListcaseActSiteEntity.Clear();
            //else if (Rb_Process_MS.Checked)
                ListcaseMsSiteEntity.Clear();
        }

        private void rbSelProgram_Click(object sender, EventArgs e)
        {
            if (rbSelProgram.Checked == true)
            {
                HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Service", "I", "A", "R", PrivilegeEntity, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2));
                hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnProgramClosed);
                hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                hierarchieSelectionForm.ShowDialog();
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

        private void Ref_From_Date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblReferenceTo_Click(object sender, EventArgs e)
        {

        }

        private void Ref_To_Date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel8_PanelCollapsed(object sender, EventArgs e)
        {

        }

        private void lblReportfromDate_Click(object sender, EventArgs e)
        {

        }

        private void Rep_From_Date_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblReportPeriodDate_Click(object sender, EventArgs e)
        {

        }

        private void lblReportTo_Click(object sender, EventArgs e)
        {

        }

        private void Rb_Details_Yes_CheckedChanged(object sender, EventArgs e)
        {
            //if(Rb_Details_Yes.Checked)
            //{
            //    if (Rb_Process_Both.Checked) chkbMontCounty.Visible = true;else chkbMontCounty.Visible = false;
            //}
            //else
            //{
            //    chkbMontCounty.Visible = false;
            //}
        }

        private void rbAllPrograms_Click(object sender, EventArgs e)
        {
            if (rbAllPrograms.Checked == true)
            {
                HierarchyGrid.Rows.Clear();
                SelectedHierarchies.Clear();
            }
        }

        private void RNGB0004Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void Rb_County_All_CheckedChanged(object sender, EventArgs e)
        {
            ListcommonEntity.Clear();
        }


        private void Rb_Fund_Sel_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Fund_Sel.Checked == true)
            {
                //****** Commented by Vikash
                SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, Sel_Funding_List, PrivilegeEntity.Program, strAgency,strDept,strProg,null,PrivilegeEntity.UserID);// (BaseForm, Sel_Funding_List);
                siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                siteform.StartPosition = FormStartPosition.CenterScreen;
                siteform.ShowDialog();
            }

        }

        private void Rb_Process_CA_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Process_CA.Checked)
            {
                All_CAMS_Selected = Btn_CA_Selection.Visible = true;
                lblDemographicsCount.Visible = pnlDemoCount.Visible = true;
                Btn_MS_Selection.Visible = false;
                Btn_CA_Selection.Text = "&All";
                Sel_CA_List.Clear();
                All_CAMS_Selected = true;

                //Rb_Fund_All.Checked = true;
                Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;

                //lblMssite.Enabled = panel7.Enabled = true;
                rdoMssiteall.Enabled = true; rdoMsselectsite.Enabled = true; rdomsNosite.Enabled = true;
                lblMssite.Text = "Service Posting Site";
                //chkbMontCounty.Visible = false;
            }
        }

        bool All_CAMS_Selected = true;
        List<MSMASTEntity> Sel_MS_List = new List<MSMASTEntity>();
        List<CAMASTEntity> Sel_CA_List = new List<CAMASTEntity>();
        private void Rb_Process_MS_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Process_MS.Checked)
            {
                All_CAMS_Selected = 
                Btn_MS_Selection.Visible = lblDemographicsCount.Visible = pnlDemoCount.Visible = true;
                Btn_CA_Selection.Visible = false;
                Btn_MS_Selection.Text = "&All";
                Sel_MS_List.Clear();
                All_CAMS_Selected = true;
                Rb_Fund_All.Checked = true;
                Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;

                //lblMssite.Enabled = panel7.Enabled = true;
                rdoMssiteall.Enabled = true; rdoMsselectsite.Enabled = true; rdomsNosite.Enabled = true;
                lblMssite.Text = "Outcome Posting Site";
                //chkbMontCounty.Visible = false;
            }
        }

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

        private void Btn_MS_Selection_Click(object sender, EventArgs e)
        {
            CASE0010_HSS_Form MS_Selection_Form = new CASE0010_HSS_Form(BaseForm, PrivilegeEntity.Program, "MS", All_CAMS_Selected, Sel_MS_List, Sel_CA_List, strAgency, strDept, strProg, PrivilegeEntity.UserID);
            MS_Selection_Form.FormClosed += new FormClosedEventHandler(Get_Sel_MS_List);
            MS_Selection_Form.StartPosition = FormStartPosition.CenterScreen;
            MS_Selection_Form.ShowDialog();
        }

        private void Get_Sel_MS_List(object sender, FormClosedEventArgs e)
        {
            CASE0010_HSS_Form form = sender as CASE0010_HSS_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                List<MSMASTEntity> MS_List = new List<MSMASTEntity>();
                //Sel_MS_List.Clear();
                MS_List = form.Get_Sel_MS_List();
                
                Btn_MS_Selection.Text = "&All";
                All_CAMS_Selected = true;
                if (MS_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_MS_List = MS_List;
                    Btn_MS_Selection.Text = "Se&l";
                }
            }
            else
            {
                List<MSMASTEntity> MS_List = new List<MSMASTEntity>();
                //Sel_MS_List.Clear();
                MS_List = form.Get_Sel_MS_List();
                Btn_MS_Selection.Text = "&All";
                All_CAMS_Selected = true;
                if (MS_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_MS_List = MS_List;
                    Btn_MS_Selection.Text = "Se&l";
                }
            }
        }

        private void Get_Sel_CA_List(object sender, FormClosedEventArgs e)
        {
            CASE0010_HSS_Form form = sender as CASE0010_HSS_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                List<CAMASTEntity> CA_List = new List<CAMASTEntity>();
                //Sel_CA_List.Clear();
                CA_List = form.Get_Sel_CA_List();
                
                Btn_CA_Selection.Text = "&All";
                All_CAMS_Selected = true;
                if (CA_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_CA_List = CA_List;
                    Btn_CA_Selection.Text = "Se&l";
                }
            }
            else
            {
                List<CAMASTEntity> CA_List = new List<CAMASTEntity>();
                //Sel_CA_List.Clear();
                CA_List = form.Get_Sel_CA_List();

                Btn_CA_Selection.Text = "&All";
                All_CAMS_Selected = true;
                if (CA_List.Count > 0)
                {
                    All_CAMS_Selected = false;
                    Sel_CA_List = CA_List;
                    Btn_CA_Selection.Text = "Se&l";
                }
            }
        }


        private void Btn_CA_Selection_Click(object sender, EventArgs e)
        {
            CASE0010_HSS_Form CA_Selection_Form = new CASE0010_HSS_Form(BaseForm, PrivilegeEntity.Program, "CA", All_CAMS_Selected, Sel_MS_List, Sel_CA_List,strAgency,strDept,strProg,PrivilegeEntity.UserID);
            CA_Selection_Form.FormClosed += new FormClosedEventHandler(Get_Sel_CA_List);
            CA_Selection_Form.StartPosition = FormStartPosition.CenterScreen;
            CA_Selection_Form.ShowDialog();
        }

        //****************************************************************************************************

        //private void Delete_RDLC_Brfore_Creation()
        //{
        //    MyDir = new DirectoryInfo(ReportPath + "\\"); // Run at Server

        //    FileInfo[] MyFiles = MyDir.GetFiles("*.rdlc");
        //    bool MasterRep_Deleted = false, Bypass_Rep_Deleted = false, IND_Rep_Deleted = false, FAM_Rep_Deleted = false, PM_Detailed_Rep_Deleted = false;
        //    string Bypass_Rep_Name, IND_Rep_Name, FAM_Rep_Name, PM_Details_Rep_Name, TmpMainRep;

        //    //Bypass_Rep_Name = IND_Rep_Name = FAM_Rep_Name = RemoveBetween(Rep_Name, '.', 'c');

        //    Bypass_Rep_Name = IND_Rep_Name = FAM_Rep_Name = PM_Details_Rep_Name = TmpMainRep = Main_Rep_Name;
        //    TmpMainRep += ".rdlc";
        //    Bypass_Rep_Name += "Bypass_RdlC.rdlc";
        //    IND_Rep_Name += "SNP_IND_RdlC.rdlc";
        //    FAM_Rep_Name += "MST_FAM_RdlC.rdlc";
        //    PM_Details_Rep_Name += "Detail_RdlC.rdlc";
        //    if (Scr_Oper_Mode == "RNGB0004")
        //        PM_Detailed_Rep_Deleted = true;

        //    //if (!Summary_Sw)
        //    //    SubReport_Deleted = true;
        //    foreach (FileInfo MyFile in MyFiles)
        //    {
        //        if (MyFile.Exists)
        //        {

        //            if (TmpMainRep == MyFile.Name && !MasterRep_Deleted)
        //            { MyFile.Delete(); MasterRep_Deleted = true; }

        //            if (Bypass_Rep_Name == MyFile.Name && !Bypass_Rep_Deleted)
        //            { MyFile.Delete(); Bypass_Rep_Deleted = true; }

        //            if (IND_Rep_Name == MyFile.Name && !IND_Rep_Deleted)
        //            { MyFile.Delete(); IND_Rep_Deleted = true; }

        //            if (FAM_Rep_Name == MyFile.Name && !FAM_Rep_Deleted)
        //            { MyFile.Delete(); FAM_Rep_Deleted = true; }

        //            if (Scr_Oper_Mode == "CASB0014")
        //            {
        //                if (PM_Details_Rep_Name == MyFile.Name && !PM_Detailed_Rep_Deleted)
        //                { MyFile.Delete(); PM_Detailed_Rep_Deleted = true; }
        //            }


        //            if (MasterRep_Deleted && Bypass_Rep_Deleted
        //                && IND_Rep_Deleted && FAM_Rep_Deleted && PM_Detailed_Rep_Deleted)
        //                break;
        //        }
        //    }
        //}

        private void rbRepPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRepPeriod.Checked == true)
            {
                Ref_From_Date.Enabled = false;
                Ref_To_Date.Enabled = false;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                pnlRefPeriod.Visible = false;
                this.Size = new Size(Width, 665);
            }
            //else if (RbCummilative.Checked == true)
            //{
            //    Ref_From_Date.Enabled = true;
            //    Ref_To_Date.Enabled = true;
            //    Rep_From_Date.Enabled = false;
            //    Rep_To_Date.Enabled = false;
            //}
            else if (rbBoth.Checked == true)
            {
                Ref_From_Date.Enabled = true;
                Ref_To_Date.Enabled = true;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                pnlRefPeriod.Visible = true;
                Size = new Size(this.Width, 695);
            }
        }

        private void btnMergeExcelView_Click(object sender, EventArgs e)
        {
            PdfListForm pdfMergeListForm = new PdfListForm(BaseForm);
            pdfMergeListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfMergeListForm.ShowDialog();
        }

        private void Rb_Process_Both_CheckedChanged(object sender, EventArgs e)
        {
            if (Rb_Process_Both.Checked)
            {
                All_CAMS_Selected = Btn_CA_Selection.Visible = false;
                lblDemographicsCount.Visible = pnlDemoCount.Visible = true;
                Btn_MS_Selection.Visible = false;
                Btn_CA_Selection.Text = "&All";
                Sel_CA_List.Clear(); Sel_MS_List.Clear();
                All_CAMS_Selected = true;

                Rb_Fund_All.Checked = true;
                Sel_Funding_List.Clear();
                Rb_Fund_All.Enabled = Rb_Fund_Sel.Enabled = true;

                // lblMssite.Enabled = panel7.Enabled = false;
                rdoMssiteall.Enabled = false; rdoMsselectsite.Enabled = false; rdomsNosite.Enabled = false;
                lblMssite.Text = "Posting Site";
                rdoMssiteall.Checked = true;

                ListcaseMsSiteEntity.Clear();

                //if (Rb_Details_Yes.Checked) chkbMontCounty.Visible = true; else chkbMontCounty.Visible = false;
            }
            else
            {
                //chkbMontCounty.Visible = false; chkbMontCounty.Checked = false;
            }
        }

        //Added by Vikash on 02/16/2024 as per "Detail Audit Adjustments" document
        private void rdbAlertCodesAll_Click(object sender, EventArgs e)
        {
            SelectedAlertCodes = string.Empty;
            _errorProvider.SetError(rdbSelectedAlertCodes, null);
        }
        private void rdbSelectedAlertCodes_Click(object sender, EventArgs e)
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

        private void LanguageCountsReport(DataTable dtCounty, Document document)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(60, 15, 112));
            iTextSharp.text.Font fc2 = new iTextSharp.text.Font(bfTimes, 7, 1, new iTextSharp.text.BaseColor(45, 45, 153));
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bfTimes, 7, 2, BaseColor.RED);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bfTimes, 6);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 10, 1);
            iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            PdfPTable Table;
            try
            {
                if (rbRepPeriod.Checked)
                {
                    Table = new PdfPTable(2);
                    Table.TotalWidth = 500f;
                    Table.LockedWidth = true;
                    float[] widths2 = new float[] { 30f, 25f };
                    Table.SetWidths(widths2);
                    Table.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPCell Header = new PdfPCell(new Phrase("Language Counts", TblFontBold));
                    Header.Colspan = 2;
                    Header.HorizontalAlignment = Element.ALIGN_CENTER;
                    Header.FixedHeight = 17f;
                    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table.AddCell(Header);
                }
                else
                {
                    Table = new PdfPTable(5);
                    Table.TotalWidth = 500f;
                    Table.LockedWidth = true;
                    float[] widths2 = new float[] { 30f, 25f, 50f, 30f, 25f };
                    Table.SetWidths(widths2);
                    Table.HorizontalAlignment = Element.ALIGN_LEFT;

                    PdfPCell Header = new PdfPCell(new Phrase("Language Counts", TblFontBold));
                    Header.Colspan = 5;
                    Header.HorizontalAlignment = Element.ALIGN_CENTER;
                    Header.FixedHeight = 17f;//15f;
                    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table.AddCell(Header);
                }

                PdfPCell R1 = new PdfPCell(new Phrase("Language Counts - Report Period", TblFontBold1));
                R1.Colspan = 2;
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                Table.AddCell(R1);

                if (rbBoth.Checked)
                {
                    R1 = new PdfPCell(new Phrase("", TableFont));
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase("Language Counts - Refrence Period", TblFontBold1));
                    R1.Colspan = 2;
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    R1.BackgroundColor = new iTextSharp.text.BaseColor(210, 228, 247);
                    Table.AddCell(R1);
                }

                R1 = new PdfPCell(new Phrase("Language", TblFontBold1));
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                Table.AddCell(R1);

                R1 = new PdfPCell(new Phrase("# of Households", TblFontBold1));
                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                R1.FixedHeight = 12f;
                R1.Border = iTextSharp.text.Rectangle.BOX;
                Table.AddCell(R1);

                if (rbBoth.Checked)
                {
                    R1 = new PdfPCell(new Phrase("", TableFont));
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase("Language", TblFontBold1));
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);

                    R1 = new PdfPCell(new Phrase("# of Households", TblFontBold1));
                    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                    R1.FixedHeight = 12f;
                    R1.Border = iTextSharp.text.Rectangle.BOX;
                    Table.AddCell(R1);
                }

                foreach (DataRow dr in dtLanguage.Rows)
                {
                    if (dr["Fam_Lang"].ToString() == "Total")
                    {
                        R1 = new PdfPCell(new Phrase(dr["Fam_Lang"].ToString(), TblFontBold1));
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RepAppCNT"].ToString(), TblFontBold1));
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);
                    }
                    else if (dr["Fam_Lang"].ToString().Trim() != "")
                    {
                        R1 = new PdfPCell(new Phrase(dr["Fam_Lang"].ToString(), TableFont));
                        R1.HorizontalAlignment = Element.ALIGN_LEFT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);

                        R1 = new PdfPCell(new Phrase(dr["RepAppCNT"].ToString(), TableFont));
                        R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        R1.FixedHeight = 12f;
                        R1.Border = iTextSharp.text.Rectangle.BOX;
                        Table.AddCell(R1);
                    }

                    if (rbBoth.Checked)
                    {
                        if (dr["Fam_Lang"].ToString() == "Total")
                        {
                            R1 = new PdfPCell(new Phrase("", TblFontBold1));
                            R1.HorizontalAlignment = Element.ALIGN_LEFT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Table.AddCell(R1);

                            R1 = new PdfPCell(new Phrase(dr["Fam_Lang"].ToString(), TblFontBold1));
                            R1.HorizontalAlignment = Element.ALIGN_LEFT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.BOX;
                            Table.AddCell(R1);

                            R1 = new PdfPCell(new Phrase(dr["RefAppCNT"].ToString(), TblFontBold1));
                            R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.BOX;
                            Table.AddCell(R1);
                        }
                        else if (dr["Fam_Lang"].ToString().Trim() != "")
                        {
                            R1 = new PdfPCell(new Phrase("", TableFont));
                            R1.HorizontalAlignment = Element.ALIGN_LEFT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Table.AddCell(R1);

                            R1 = new PdfPCell(new Phrase(dr["Fam_Lang"].ToString(), TableFont));
                            R1.HorizontalAlignment = Element.ALIGN_LEFT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.BOX;
                            Table.AddCell(R1);

                            R1 = new PdfPCell(new Phrase(dr["RefAppCNT"].ToString(), TableFont));
                            R1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            R1.FixedHeight = 12f;
                            R1.Border = iTextSharp.text.Rectangle.BOX;
                            Table.AddCell(R1);
                        }


                    }

                }

                if (Table.Rows.Count > 0)
                    document.Add(Table);
            }
            catch (Exception e) { }
        }
    }
}