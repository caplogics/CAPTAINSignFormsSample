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
using ListItem = Captain.Common.Utilities.ListItem;
using Captain.DatabaseLayer;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNGS0014 : _iForm//Form
    {
        public RNGS0014()
        {
            InitializeComponent();
        }
        #region private variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;


        #endregion


        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity PrivilegeEntity { get; set; }

        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        public List<SPCommonEntity> Sel_Funding_List { get; set; }

        public List<ZipCodeEntity> ListZipCode { get; set; }

        public List<SRCsb14GroupEntity> ListGroupCode { get; set; }

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

        public CaseMstEntity CaseMST
        {
            get; set;
        }

        #endregion
        List<ZipCodeEntity> zipcode_List = new List<ZipCodeEntity>();
        List<CommonEntity> County_List = new List<CommonEntity>();
        List<CaseSiteEntity> Site_List = new List<CaseSiteEntity>();
        List<SRCsb14GroupEntity> RngCodelist = new List<SRCsb14GroupEntity>();
        List<Csb16DTREntity> DateRange_List = new List<Csb16DTREntity>();
        List<Csb16DTREntity> Sys_DateRange_List = new List<Csb16DTREntity>();
        List<SRCsb14GroupEntity> OutCome_MasterList = new List<SRCsb14GroupEntity>();

        public RNGS0014(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            ListZipCode = new List<ZipCodeEntity>();
            ListGroupCode = new List<SRCsb14GroupEntity>();
            ListcaseSiteEntity = new List<CaseSiteEntity>();
            ListcaseMsSiteEntity = new List<CaseSiteEntity>();
            Sel_Funding_List = new List<SPCommonEntity>();
            ListcommonEntity = new List<CommonEntity>();

            InitializeComponent();
            BaseForm = baseForm;
            PrivilegeEntity = privilegeEntity;
            //this.Text = privilegeEntity.Program + " - " + privilegeEntity.PrivilegeName;
            this.Text =  privilegeEntity.PrivilegeName;
            _model = new CaptainModel();


            RngCodelist = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, string.Empty/*BaseForm.BaseAdminAgency*/);//*** Retriving RNGSR Data ***
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);//*** Retriving Hierarchy ***
            ReportPath = _model.lookupDataAccess.GetReportPath();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            Txt_Pov_High.Validator = TextBoxValidation.IntegerValidator;
            Txt_Pov_Low.Validator = TextBoxValidation.IntegerValidator;

            fill_CaseType_Combo();//***Filling Combobox ^Case Type ***
            propReportPath = _model.lookupDataAccess.GetReportPath();
            //Fill_Program_Combo();//***Filling ComboBox ^Program ****
            fillRngCode();//***Filling ComboBox ^Code with RNGServices***


            Get_MasterTable_DateRanges();
            Get_DG_Result_Table_Structure();
            Get_DG_Bypass_Table_Structure();
            Get_DG_SNP_Bypass_Table_Structure();
            Get_DG_MST_Bypass_Table_Structure();
            Fill_CAMS_Master_List();
            Fill_All_List_Arrays();
            Fill_Fund_Mast_List();

            if (Rb_SNP_Mem.Checked)
            {
                chkbUndupTable.Visible = false; chkbUndupTable.Checked = false;
                //**rdbSummaryDet.Visible = rbo_ProgramWise.Visible = true;  spacer6.Visible = false;
            }
            else
            {
                chkbUndupTable.Visible = true;
                //**rdbSummaryDet.Visible = false; rbo_ProgramWise.Visible = false; spacer6.Visible = true;
            }
            this.Size = new Size(this.Width, 685);//710)
            this.pnlParams.Size = new Size(Width, 535);
        }

        private void cmbRngCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRngCode.Items.Count > 0)
            {
                if (((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() != "**")
                {

                    SRCsb14GroupEntity rngcodedata = RngCodelist.Find(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString() && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                    if (rngcodedata != null)
                    {
                        if (rngcodedata.OFdate != string.Empty)
                        {
                            ListGroupCode.Clear();
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

        private void Txt_Pov_High_LostFocus(object sender, EventArgs e)
        {

        }

        private void Txt_Pov_Low_LostFocus(object sender, EventArgs e)
        {

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

        private void Rb_County_All_CheckedChanged(object sender, EventArgs e)
        {
            
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
                    switch (Scr_Oper_Mode)
                    {
                        //case "RNGS0014":
                        //    SelectZipSiteCountyForm zipcodeform = new SelectZipSiteCountyForm(BaseForm, ListZipCode);
                        //    zipcodeform.FormClosed += new Form.FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                        //    zipcodeform.ShowDialog();
                        //    break;
                        case "RNGS0014":   // Groups
                            SelectZipSiteCountyForm zipcodeform1 = new SelectZipSiteCountyForm(BaseForm, ListGroupCode, Ref_From_Date.Text.Trim(), Ref_To_Date.Text.Trim(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
                            zipcodeform1.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                            zipcodeform1.StartPosition = FormStartPosition.CenterScreen;
                            zipcodeform1.ShowDialog();
                            break;
                    }
                }
            }
        }

        private void Rb_Zip_All_CheckedChanged(object sender, EventArgs e)
        {
            ListGroupCode.Clear();
        }

        private void Rb_Site_No_CheckedChanged(object sender, EventArgs e)
        {

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
                    ListGroupCode = form.SelectedSRGroupCodeEntity;
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

        private void Rb_Site_All_Click(object sender, EventArgs e)
        {

        }

        private void Pb_Search_Hie_Click_1(object sender, EventArgs e)
        {

            //HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            // hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            // hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            // hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
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
                    HierarchyGrid.Rows.Clear(); SelectedHierarchies.Clear();
                    fillRngCode();
                }
            }
        }

        string Ser_Rep_Name = string.Empty;

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
                bool Data_processed = false;

                string strReportSwitch = "C";
                string strReportControlSwitch = "N";
                string strUndupTabSwitch = "N";
                if (rdoperiodBoth.Checked)
                {

                    strReportSwitch = "B";
                    strReportControlSwitch = chkRepControl.Checked == true ? "Y" : "N";

                    if (chkbUndupTable.Checked == true)
                        strUndupTabSwitch = "Y";
                    //strUndupTabSwitch = "N";
                }
                else
                {
                    if (rdoperiod.Checked == true)
                        strReportSwitch = "R";

                    if(chkbUndupTable.Checked==true) strUndupTabSwitch = "Y";
                }
                Ser_Rep_Name = string.Empty;

                DataSet ds = new DataSet();
                ds = _model.AdhocData.Get_SRRNGPM_Counts(Search_Entity, Rb_Details_Yes.Checked ? "Y" : "N", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, strReportControlSwitch, strUndupTabSwitch, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString());
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0)
                    {
                        Data_processed = true;
                        if (Rb_Details_Yes.Checked)
                        {
                            PerformanceMeasures_Details_Dynamic_RDLC();
                            Result_Table = ds.Tables[3];
                        }

                        DataTable dtBoth = new DataTable();
                        if (rdoperiodBoth.Checked)//&& Rb_SNP_Mem.Checked)
                        {
                            //Search_Entity.Rep_Period_FDate = Rep_From_Date.Value.ToShortDateString();
                            //Search_Entity.Rep_Period_TDate = Rep_To_Date.Value.ToShortDateString();
                            //Search_Entity.Rep_From_Date = Rep_From_Date.Value.ToShortDateString();
                            //Search_Entity.Rep_To_Date = Rep_To_Date.Value.ToShortDateString();

                            //DataSet dsboth = _model.AdhocData.Get_SRRNGPM_Counts(Search_Entity, "N", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString(), strReportSwitch, strReportControlSwitch);

                            //if (dsboth.Tables.Count > 1)
                            //{
                            //    if (chkRepControl.Checked == true)
                            //        dtBoth = dt;
                            //    else
                            //        dtBoth = dsboth.Tables[2];
                            //}
                        }



                        On_SaveForm_Closed(dt, dtBoth);
                        if (chkbExcel.Checked)
                            On_SaveExcel_Closed(dt, dtBoth);

                    }
                    else
                        AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);

                }

                if (!Data_processed)
                    AlertBox.Show("No Records exists with Selected Criteria", MessageBoxIcon.Warning);



            }
        }



        //*****************PerformanceMeasures_Details_Dynamic_RDLC***********************************************************************************


        List<DG_ResTab_Entity> PM_Detail_Table_List = new List<DG_ResTab_Entity>();
        private void Get_PM_Detail_Table_Structure()
        {
            //Declare @Detail_Cum_Table Table(SortUnDup_Group VarChar(10), SortUnDup_Table VarChar(10), SortUnDup_Agy Char(2), SortUnDup_Dept Char(2), SortUnDup_Prog Char(2), 
            //                                SortUnDup_Year VarChar(4), SortUnDup_App VarChar(8), SortUnDup_Fam_ID VarChar(9), SortUnDup_Client_ID Decimal(9),
            //                                SortUnDup_OutcomeCode Varchar(10), SortUnDup_OutCome_Date Date, SortUnDup_Count_Indicator Char(1), SortUnDup_Result Varchar(4), SortUnDup_Name Varchar(90), 
            //                                R1 Int Default 0, R2 Int Default 0, R3 Int Default 0, R4 Int Default 0, R5 Int Default 0)


            PM_Detail_Table_List.Clear();
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Group", "", "L", "2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Table", "", "L", "2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Agy", "", "L", "1.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Dept", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Prog", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Year", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_App", "", "L", "1.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Fam_ID", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Client_ID", "", "L", "3.2in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Name", "", "L", "2.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_UserName", "", "R", "2.1in"));
            if (rdoperiodBoth.Checked)
            {
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_OutCome_Date", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RefCount", "", "R", "1.5in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RefDate", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RepCount", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_RepDate", "", "R", ".95in"));

            }
            else
            {
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutCome_Date", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RefCount", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RefDate", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RepCount", "", "R", ".95in"));
                PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_RepDate", "", "R", ".95in"));
            }

            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_OutcomeCode", "", "R", "0.7in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Count_Indicator", "", "L", "3.2in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Result", "", "L", "3.2in"));

            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R1", "", "R", ".65in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R2", "", "R", ".65in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R3", "", "R", ".65in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R4", "", "R", ".65in"));
            //PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "R5", "", "R", ".65in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("N", "SortUnDup_Group_Desc", "", "R", ".85in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Goal_Desc", "", "L", "3.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Isite", "", "L", "1.5in"));
            PM_Detail_Table_List.Add(new DG_ResTab_Entity("Y", "SortUnDup_Psite", "", "L", "1.5in"));
        }

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



                    XmlElement Cell_style = xml.CreateElement("Style");
                    Textbox.AppendChild(Cell_style);




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

                        //case "SortUnDup_Agy":
                        //    Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                        //                   " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                        //                   " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes +
                        //                  ", Fields!SortUnDup_Group_Desc.Value, Fields!SortUnDup_Group.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
                        //                  " + Fields!SortUnDup_Table.Value + " + Tmp_Double_Codes + "   " + Tmp_Double_Codes +
                        //                  " + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                  " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                        //                  " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";






                        case "SortUnDup_Group":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                                                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                                                " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ",Fields!SortUnDup_Group_Desc.Value " +
                                                //"Fields!SortUnDup_Group.Value" + Tmp_Double_Codes + " " + Tmp_Double_Codes + "" +
                                                ", Fields!SortUnDup_Group.Value)";

                            // " + Fields!SortUnDup_App.Value  + " + Tmp_Double_Codes + "      " + Tmp_Double_Codes +
                            // " + Fields!SortUnDup_App.Value  + " + Tmp_Double_Codes + "      " + Tmp_Double_Codes +
                            //" + Fields!SortUnDup_Name.Value)";
                            break;

                        case "SortUnDup_Table":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value =" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Group " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", Fields!SortUnDup_Table.Value)))";
                                               
    
                            //"=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                            //                    " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                            //                    " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," +
                            //                Tmp_Double_Codes + "Group " + Tmp_Double_Codes + ", Fields!SortUnDup_Table.Value)";

                            //" + Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";
                            break;


                        case "SortUnDup_Agy":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +" , " + Tmp_Double_Codes + " " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Hiearachy " + Tmp_Double_Codes + ", " +
                                                "IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes+ " , " + Tmp_Double_Codes + " " + Tmp_Double_Codes 
                                                +
                                         ", Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                                         " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")))";

                            //"=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes +
                            //                    " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes +
                            //                    " OR Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "ZZZGrpDesc" + Tmp_Double_Codes + "," +
                            //                Tmp_Double_Codes + "Hiearachy " + Tmp_Double_Codes +
                            //             ", Fields!SortUnDup_Agy.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Dept.Value + " + Tmp_Double_Codes + "-" + Tmp_Double_Codes +
                            //             " + Fields!SortUnDup_Prog.Value + " + Tmp_Double_Codes + "  " + Tmp_Double_Codes + ")";
                            break;


                        case "SortUnDup_App":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                 Tmp_Double_Codes + "App# " + Tmp_Double_Codes +
                                ", Fields!SortUnDup_App.Value)"; break;
                        case "SortUnDup_Isite":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Intake Site" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_Isite.Value)"; break;
                        case "SortUnDup_Psite":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "Service Posting Site" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_Psite.Value)"; break;
                        case "SortUnDup_Name":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                Tmp_Double_Codes + "Name " + Tmp_Double_Codes +
                                ", Fields!SortUnDup_Name.Value)"; break;

                        case "SortUnDup_OutCome_Date":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                           Tmp_Double_Codes + " Activity Date" + Tmp_Double_Codes +
                                           ", Format(Fields!" + Entity.Column_Name + ".Value, " + Tmp_Double_Codes + "MM/dd/yyyy" + Tmp_Double_Codes + "))"; break;

                        case "SortUnDup_UserName":
                            Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                                                               Tmp_Double_Codes + "UPDATED BY USER ID" + Tmp_Double_Codes +
                                                               ", Fields!SortUnDup_UserName.Value)"; break;
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


                            //case "R1": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                            //                                              Tmp_Double_Codes + "R1" + Tmp_Double_Codes +
                            //                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
                            //case "R2": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                            //                                              Tmp_Double_Codes + "R2" + Tmp_Double_Codes +
                            //                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
                            //case "R3": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                            //                                              Tmp_Double_Codes + "R3" + Tmp_Double_Codes +
                            //                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
                            //case "R4": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                            //                                              Tmp_Double_Codes + "R4" + Tmp_Double_Codes +
                            //                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
                            //case "R5": Field_Value = "=IIf(Fields!SortUnDup_Table.Value= " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " +
                            //                                              Tmp_Double_Codes + "R5" + Tmp_Double_Codes +
                            //                                              ", Fields!" + Entity.Column_Name + ".Value)"; break;
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




                    if (!string.IsNullOrEmpty(Text_Align))
                    {
                        XmlElement Cell_Align = xml.CreateElement("Style");
                        XmlElement Cell_TextAlign = xml.CreateElement("TextAlign");         // Repeating Cell Border Style   09092012
                        //Cell_TextAlign.InnerText = "=IIf(Fields!Res_Row_Type.Value=" + Tmp_Double_Codes + "GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Left" + Tmp_Double_Codes + "," + Tmp_Double_Codes + Text_Align + Tmp_Double_Codes + ")";
                        Cell_TextAlign.InnerText = Text_Align;
                        Cell_Align.AppendChild(Cell_TextAlign);
                        Paragraph.AppendChild(Cell_Align);
                    }

                    //XmlElement Ret_Style = xml.CreateElement("Style");
                    //TextRun.AppendChild(Return_Style);

                    XmlElement Style_FontWeight = xml.CreateElement("FontWeight");
                    Style_FontWeight.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + "," +
                                                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "Bold" + Tmp_Double_Codes + ", "
                                                        + Tmp_Double_Codes + "Normal" + Tmp_Double_Codes + "))";
                    Return_Style.AppendChild(Style_FontWeight);

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
                    XmlElement Cell_Style_BackColor = xml.CreateElement("BackgroundColor");
                    //Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "White" + Tmp_Double_Codes + ")";
                    Cell_Style_BackColor.InnerText = "=IIf(Fields!SortUnDup_Table.Value=" + Tmp_Double_Codes + "000GrpDesc" + Tmp_Double_Codes + "," + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + "," +
                                                        "IIf(Fields!SortUnDup_Table.Value = " + Tmp_Double_Codes + "001GrpDesc" + Tmp_Double_Codes + ", " + Tmp_Double_Codes + "LightSteelBlue" + Tmp_Double_Codes + ", "
                                                        + Tmp_Double_Codes + "White" + Tmp_Double_Codes + "))";
                    //Cell_Style_BackColor.InnerText = "Blue";
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
                for (int Loop = 0; Loop < 13; Loop++)            // Dynamic based on Display Columns in 3/6
                {
                    XmlElement TablixMember = xml.CreateElement("TablixMember");
                    Tablix_Col_Members.AppendChild(TablixMember);
                }
            }
            else
            {
                for (int Loop = 0; Loop < 10; Loop++)            // Dynamic based on Display Columns in 3/6
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

                Ser_Rep_Name = "RNGS0014_Detail_RdlC_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmm") + ".rdlc";

                xml.Save(ReportPath + Ser_Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System

                //xml.Save(@"F:\CapreportsRDLC\" + Rep_Name); //I've chosen the c:\ for the resulting file pavel.xml   // Run at Local System
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            //Console.ReadLine();
        }

        private void HeaderPage(Document document, PdfWriter writer)
        {
            // BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 9);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);
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

            PdfPCell cellspace = new PdfPCell(new Phrase(""));
            cellspace.HorizontalAlignment = Element.ALIGN_CENTER;
            cellspace.Colspan = 2;
            cellspace.PaddingBottom = 10;
            cellspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            outer.AddCell(cellspace);

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

            PdfPCell cellRptTitle = new PdfPCell(new Phrase(PrivilegeEntity.Program + " - " + PrivilegeEntity.PrivilegeName, TblHeaderTitleFont));
            cellRptTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptTitle.Colspan = 2;
            cellRptTitle.PaddingBottom = 15;
            cellRptTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
            outer.AddCell(cellRptTitle);

            //PdfPCell row1 = new PdfPCell(new Phrase(PrivilegeEntity.Program + " - " + PrivilegeEntity.PrivilegeName, TblFontBold));
            //row1.HorizontalAlignment = Element.ALIGN_CENTER;
            //row1.Colspan = 2;
            //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(row1);

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

            PdfPCell cellRptHeader = new PdfPCell(new Phrase("Report Parameters", TblParamsHeaderFont));
            cellRptHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptHeader.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cellRptHeader.PaddingBottom = 5;
            cellRptHeader.MinimumHeight = 6;
            cellRptHeader.Colspan = 2;
            cellRptHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellRptHeader.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            outer.AddCell(cellRptHeader);

            //PdfPCell S1 = new PdfPCell(new Phrase("", TableFont));
            //S1.HorizontalAlignment = Element.ALIGN_LEFT;
            //S1.FixedHeight = 20f;
            //S1.Colspan = 2;
            //S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(S1);

            PdfPCell cell_Content_Title1 = new PdfPCell(new Phrase("  Hierarchy", TableFont));
            cell_Content_Title1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title1.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title1);

            PdfPCell cell_Content_Desc1 = new PdfPCell(new Phrase("Agency: " + Sel_AGY + ", Department: " + Sel_DEPT + ", Program: " + Sel_PROG, TableFont));
            cell_Content_Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc1.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc1);


            PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + "Case Type", TableFont));
            cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title2.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title2);

            PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc2.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc2);


            //PdfPCell R3 = new PdfPCell(new Phrase("           Agency: " + Sel_AGY + " , Department : " + Sel_DEPT + " , Program : " + Sel_PROG, TableFont));
            //R3.Colspan = 2;
            //R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R3);

            //PdfPCell R4 = new PdfPCell(new Phrase("            Case Type", TableFont));
            //R4.HorizontalAlignment = Element.ALIGN_LEFT;
            //R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R4);

            //PdfPCell R5 = new PdfPCell(new Phrase(" : " + ((Captain.Common.Utilities.ListItem)Cmb_CaseType.SelectedItem).Text.ToString(), TableFont));
            //R5.HorizontalAlignment = Element.ALIGN_LEFT;
            //R5.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R5);

            //PdfPCell R6 = new PdfPCell(new Phrase("            Program", TableFont));
            //R6.HorizontalAlignment = Element.ALIGN_LEFT;
            //R6.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R6);

            PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  " + "Program", TableFont));
            cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title3.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title3);


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
                        Sel_Programs = Sel_Programs.Substring(0, (Sel_Programs.Length - 2));// 1));
                }
            }


            //PdfPCell R7 = new PdfPCell(new Phrase(" : " + (rbAllPrograms.Checked == true ? "All" : Sel_Programs), TableFont));
            //R7.HorizontalAlignment = Element.ALIGN_LEFT;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R7);


            PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase((rbAllPrograms.Checked == true ? "All" : Sel_Programs), TableFont));
            cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc3.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc3);

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

            PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  " + "Funding Source", TableFont));
            cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title4.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title4);

            string strSelectFund = Search_Entity.CA_Fund_Filter;
            strSelectFund = strSelectFund.Replace("'", "");
            strSelectFund = strSelectFund.Trim().Replace(" ,", ", ");

            //PdfPCell RFundType = new PdfPCell(new Phrase(" : " + (Rb_Fund_All.Checked == true ? "All" : strSelectFund), TableFont));
            //RFundType.HorizontalAlignment = Element.ALIGN_LEFT;
            //RFundType.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(RFundType);

            PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase((Rb_Fund_All.Checked == true ? "All" : strSelectFund), TableFont));
            cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc4.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc4);

            //PdfPCell R8 = new PdfPCell(new Phrase("            Case Status", TableFont));
            //R8.HorizontalAlignment = Element.ALIGN_LEFT;
            //R8.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R8);

            //PdfPCell R9 = new PdfPCell(new Phrase(" : " + Sel_params_To_Print[1], TableFont));
            //R9.HorizontalAlignment = Element.ALIGN_LEFT;
            //R9.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R9);


            PdfPCell cell_Content_Title5 = new PdfPCell(new Phrase("  " + "Case Status", TableFont));
            cell_Content_Title5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title5.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title5);

            PdfPCell cell_Content_Desc5 = new PdfPCell(new Phrase(Sel_params_To_Print[1], TableFont));
            cell_Content_Desc5.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc5.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc5);


            //PdfPCell R10 = new PdfPCell(new Phrase("            Date Selection", TableFont));
            //R10.HorizontalAlignment = Element.ALIGN_LEFT;
            //R10.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R10);

            //PdfPCell R11 = new PdfPCell(new Phrase(" : " + (Rb_MS_Date.Checked ? "Service Date" : " ") + (Rb_MS_AddDate.Checked ? "Service ADD Date" : " "), TableFont));
            //R11.HorizontalAlignment = Element.ALIGN_LEFT;
            //R11.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R11);


            PdfPCell cell_Content_Title6 = new PdfPCell(new Phrase("  " + "Date Selection", TableFont));
            cell_Content_Title6.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title6.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title6);

            PdfPCell cell_Content_Desc6 = new PdfPCell(new Phrase((Rb_MS_Date.Checked ? "Service Date" : " ") + (Rb_MS_AddDate.Checked ? "Service ADD Date" : " "), TableFont));
            cell_Content_Desc6.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc6.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc6);


            //PdfPCell R12 = new PdfPCell(new Phrase("            Code", TableFont));
            //R12.HorizontalAlignment = Element.ALIGN_LEFT;
            //R12.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R12);

            //PdfPCell R13 = new PdfPCell(new Phrase(" : " + ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Text.ToString(), TableFont));
            //R13.HorizontalAlignment = Element.ALIGN_LEFT;
            //R13.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R13);

            //PdfPCell cell_Content_Title7 = new PdfPCell(new Phrase("  " + "Code", TableFont));
            PdfPCell cell_Content_Title7 = new PdfPCell(new Phrase("  " + "Report Description", TableFont));
            cell_Content_Title7.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title7.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title7);

            PdfPCell cell_Content_Desc7 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc7.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc7.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc7);


            //PdfPCell R34 = new PdfPCell(new Phrase("            Report For", TableFont));
            //R34.HorizontalAlignment = Element.ALIGN_LEFT;
            //R34.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R34);

            PdfPCell cell_Content_Title8 = new PdfPCell(new Phrase("  " + "Report For", TableFont));
            cell_Content_Title8.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title8.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title8);

            string strrptfor = "Report Period";
            if (rdoperiodCumulative.Checked)
                strrptfor = "Reference Period";
            if (rdoperiodBoth.Checked)
                strrptfor = "Report and Reference Period";


            //PdfPCell R35 = new PdfPCell(new Phrase(" : " + strrptfor, TableFont));
            //R35.HorizontalAlignment = Element.ALIGN_LEFT;
            //R35.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R35);

            PdfPCell cell_Content_Desc8 = new PdfPCell(new Phrase("" + strrptfor, TableFont));
            cell_Content_Desc8.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc8.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc8);

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
                //PdfPCell R14 = new PdfPCell(new Phrase("            Reference Period Date", TableFont));
                //R14.HorizontalAlignment = Element.ALIGN_LEFT;
                //R14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //outer.AddCell(R14);

                PdfPCell cell_Content_Title9 = new PdfPCell(new Phrase("  " + "Reference Period Date", TableFont));
                cell_Content_Title9.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Title9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Title9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Title9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                cell_Content_Title9.PaddingBottom = 5;
                outer.AddCell(cell_Content_Title9);



                //PdfPCell R15 = new PdfPCell(new Phrase(" : From " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                //                                + "    To " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));
                //R15.HorizontalAlignment = Element.ALIGN_LEFT;
                //R15.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //outer.AddCell(R15);

                PdfPCell cell_Content_Desc9 = new PdfPCell(new Phrase("From: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                                + "    To: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Ref_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));

                cell_Content_Desc9.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Desc9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Desc9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Desc9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                cell_Content_Desc9.PaddingBottom = 5;
                outer.AddCell(cell_Content_Desc9);

            }
            if (!rdoperiodCumulative.Checked)
            {
                //PdfPCell R16 = new PdfPCell(new Phrase("            Report Period Date", TableFont));
                //R16.HorizontalAlignment = Element.ALIGN_LEFT;
                //R16.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //outer.AddCell(R16);

                PdfPCell cell_Content_Title10 = new PdfPCell(new Phrase("  " + "Report Period Date", TableFont));
                cell_Content_Title10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Title10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Title10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Title10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                cell_Content_Title10.PaddingBottom = 5;
                outer.AddCell(cell_Content_Title10);



                //PdfPCell R17 = new PdfPCell(new Phrase(" : From " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                //                                + "    To " +
                //                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));
                //R17.HorizontalAlignment = Element.ALIGN_LEFT;
                //R17.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //outer.AddCell(R17);

                PdfPCell cell_Content_Desc10 = new PdfPCell(new Phrase("From: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_From_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                                + "    To: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(Rep_To_Date.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), TableFont));

                cell_Content_Desc10.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Desc10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Desc10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Desc10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                cell_Content_Desc10.PaddingBottom = 5;
                outer.AddCell(cell_Content_Desc10);

            }

            //PdfPCell R18 = new PdfPCell(new Phrase("           Intake Site", TableFont));
            //R18.HorizontalAlignment = Element.ALIGN_LEFT;
            //R18.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R18);

            PdfPCell cell_Content_Title11 = new PdfPCell(new Phrase("  " + "Intake Site", TableFont));
            cell_Content_Title11.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title11.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title11);

            string selectedSites = Search_Entity.Mst_Site;
            selectedSites = selectedSites.Replace("'", "");
            selectedSites = selectedSites.Replace(" ,", ", ");

            PdfPCell cell_Content_Desc11 = new PdfPCell(new Phrase((Rb_Site_All.Checked ? "All Sites" : " ") + (Rb_Site_Sel.Checked ? selectedSites : "") + (Rb_Site_No.Checked ? "No Sites" : ""), TableFont));
            cell_Content_Desc11.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc11.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc11);

            PdfPCell cell_Content_Title12 = new PdfPCell(new Phrase("  " + "Service Posting Site", TableFont));
            cell_Content_Title12.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title12.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title12);

            string selectedSites2 = Search_Entity.CaseMssite;
            selectedSites2 = selectedSites2.Replace("'","");
            selectedSites2 = selectedSites2.Replace(" ,", ", ");

            PdfPCell cell_Content_Desc12 = new PdfPCell(new Phrase((rdoMssiteall.Checked ? "All Sites" : " ") + (rdoMsselectsite.Checked ? selectedSites2 : "") + (rdomsNosite.Checked ? "No Sites" : ""), TableFont));
            cell_Content_Desc12.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc12.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc12);

            PdfPCell cell_Content_Title14 = new PdfPCell(new Phrase("  " + "Produce Details", TableFont));
            cell_Content_Title14.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title14.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title14);

            PdfPCell cell_Content_Desc14 = new PdfPCell(new Phrase((Rb_Details_Yes.Checked ? "Yes" : "No"), TableFont));
            cell_Content_Desc14.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc14.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc14);


            //PdfPCell R24 = new PdfPCell(new Phrase("            Poverty Levels", TableFont));
            //R24.HorizontalAlignment = Element.ALIGN_LEFT;
            //R24.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R24);

            PdfPCell cell_Content_Title15 = new PdfPCell(new Phrase("  " + "Poverty Level", TableFont));
            cell_Content_Title15.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title15.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title15);


            //PdfPCell R25 = new PdfPCell(new Phrase(" : From " + Txt_Pov_Low.Text + "    To " + Txt_Pov_High.Text, TableFont));
            //R25.HorizontalAlignment = Element.ALIGN_LEFT;
            //R25.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R25);

            PdfPCell cell_Content_Desc15 = new PdfPCell(new Phrase("From: " + Txt_Pov_Low.Text + "    To: " + Txt_Pov_High.Text, TableFont));
            cell_Content_Desc15.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc15.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc15);


            //PdfPCell R26 = new PdfPCell(new Phrase("            Groups", TableFont));
            //R26.HorizontalAlignment = Element.ALIGN_LEFT;
            //R26.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R26);
            
            PdfPCell cell_Content_Title16 = new PdfPCell(new Phrase("  " + "Domain", TableFont));
            cell_Content_Title16.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title16.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title16);

            string selectedDomain = Search_Entity.ZipCode;

            string[] values = selectedDomain.Split(',').Select(val => val.Trim('\''))
                       .ToArray();

            // Distinct the values and join them back into a string
            selectedDomain = string.Join(",", values.Distinct());

            selectedDomain = selectedDomain.Replace("'", "");
            selectedDomain = selectedDomain.Replace(",", ", ").TrimStart(',');

            PdfPCell cell_Content_Desc16 = new PdfPCell(new Phrase((Rb_Zip_All.Checked ? "All Domains" : selectedDomain), TableFont));
            cell_Content_Desc16.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc16.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc16);

            PdfPCell cell_Content_Title17 = new PdfPCell(new Phrase("  " + "County", TableFont));
            cell_Content_Title17.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title17.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title17);

            string selectedCounty = Search_Entity.County;
            selectedCounty = selectedCounty.Replace("'","");
            selectedCounty = selectedCounty.Replace(" ,", ", ");

            PdfPCell cell_Content_Desc17 = new PdfPCell(new Phrase((Rb_County_All.Checked ? "All" : selectedCounty), TableFont));
            cell_Content_Desc17.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc17.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc17);



            //PdfPCell R30 = new PdfPCell(new Phrase("            Secret Applications", TableFont));
            //R30.HorizontalAlignment = Element.ALIGN_LEFT;
            //R30.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R30);

            PdfPCell cell_Content_Title18 = new PdfPCell(new Phrase("  " + "Secret Applications", TableFont));
            cell_Content_Title18.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title18.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title18);


            //PdfPCell R31 = new PdfPCell(new Phrase(" : " + (Rb_Mst_NonSec.Checked ? "Non-Secret Only" : " ") + (Rb_Mst_Sec.Checked ? "Secret Only" : "") + (Rb_Mst_BothSec.Checked ? "Both Non-Secret and Secret" : ""), TableFont));
            //R31.HorizontalAlignment = Element.ALIGN_LEFT;
            //R31.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R31);

            PdfPCell cell_Content_Desc18 = new PdfPCell(new Phrase((Rb_Mst_NonSec.Checked ? "Non-Secret Only" : " ") + (Rb_Mst_Sec.Checked ? "Secret Only" : "") + (Rb_Mst_BothSec.Checked ? "Both Non-Secret and Secret" : ""), TableFont));
            cell_Content_Desc18.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc18.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc18);

            //PdfPCell R32 = new PdfPCell(new Phrase("            Report Format", TableFont));
            //R32.HorizontalAlignment = Element.ALIGN_LEFT;
            //R32.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R32);

            PdfPCell RepFormDet = new PdfPCell(new Phrase("  " + "Report Format", TableFont));
            RepFormDet.HorizontalAlignment = Element.ALIGN_LEFT;
            RepFormDet.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RepFormDet.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RepFormDet.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RepFormDet.PaddingBottom = 5;
            outer.AddCell(RepFormDet);



            string RepFormat = string.Empty;
            if (Rb_OBO_Mem.Checked) RepFormat = Rb_OBO_Mem.Text.Trim();
            //**else if (rbo_ProgramWise.Checked) RepFormat = rbo_ProgramWise.Text.Trim();
            else if (Rb_SNP_Mem.Checked) RepFormat = Rb_SNP_Mem.Text.Trim();

            //PdfPCell R33 = new PdfPCell(new Phrase(" : " + RepFormat, TableFont));
            //R33.HorizontalAlignment = Element.ALIGN_LEFT;
            //R33.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R33);

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

            RepFormDet = new PdfPCell(new Phrase("", TableFont));
            RepFormDet.HorizontalAlignment = Element.ALIGN_LEFT;
            RepFormDet.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RepFormDet.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RepFormDet.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RepFormDet.PaddingBottom = 5;
            outer.AddCell(RepFormDet);

            /*PdfPCell*/
            cell_Content_Desc19 = new PdfPCell(new Phrase("" +
    RepFormat + (Rb_OBO_Mem.Checked == true ? (chkbUndupTable.Checked == true ? " - Unduplicated at Group Level: Yes" : " - Unduplicated at Group Level: No") : ""), TableFont));
                cell_Content_Desc19.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Desc19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Desc19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Desc19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                cell_Content_Desc19.PaddingBottom = 5;
                outer.AddCell(cell_Content_Desc19);
            //}


            /*PdfPCell RepFormatDet = new PdfPCell(new Phrase("  " + "Report Format Details", TableFont));
            RepFormatDet.HorizontalAlignment = Element.ALIGN_LEFT;
            RepFormatDet.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RepFormatDet.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RepFormatDet.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RepFormatDet.PaddingBottom = 5;
            outer.AddCell(RepFormatDet);*/

            /*if(Rb_OBO_Mem.Checked)
            {
                if (chkbUndupTable.Checked)
                {
                    PdfPCell RepFormatDet1 = new PdfPCell(new Phrase("Unduplicated at Group Level: " + "Yes", TableFont));
                    RepFormatDet1.HorizontalAlignment = Element.ALIGN_LEFT;
                    RepFormatDet1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RepFormatDet1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RepFormatDet1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RepFormatDet1.PaddingBottom = 5;
                    outer.AddCell(RepFormatDet1);
                }
                else
                {
                    PdfPCell RepFormatDet1 = new PdfPCell(new Phrase("Unduplicated at Group Level: " + "No", TableFont));
                    RepFormatDet1.HorizontalAlignment = Element.ALIGN_LEFT;
                    RepFormatDet1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    RepFormatDet1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    RepFormatDet1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    RepFormatDet1.PaddingBottom = 5;
                    outer.AddCell(RepFormatDet1);
                }
            }
            else
            {
                string RepFormatDetails = string.Empty;
                if (rdbSummaryDet.Checked) RepFormatDetails = rdbSummaryDet.Text.Trim();
                else RepFormatDetails = rbo_ProgramWise.Text.Trim();


                PdfPCell RepFormatDet1 = new PdfPCell(new Phrase("" + RepFormatDetails, TableFont));
                RepFormatDet1.HorizontalAlignment = Element.ALIGN_LEFT;
                RepFormatDet1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RepFormatDet1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RepFormatDet1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RepFormatDet1.PaddingBottom = 5;
                outer.AddCell(RepFormatDet1);
            }*/



            //PdfPCell R34out = new PdfPCell(new Phrase("            Print Services", TableFont));
            //R34out.HorizontalAlignment = Element.ALIGN_LEFT;
            //R34out.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R34out);

            PdfPCell cell_Content_Title20 = new PdfPCell(new Phrase("  " + "Print Services", TableFont));
            cell_Content_Title20.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title20.PaddingBottom = 5;
            outer.AddCell(cell_Content_Title20);

            //PdfPCell R35out = new PdfPCell(new Phrase(" : " + (rdoOutcomesAll.Checked ? "Print All Services" : "Print Only Services with Counts"), TableFont));
            //R35out.HorizontalAlignment = Element.ALIGN_LEFT;
            //R35out.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //outer.AddCell(R35out);

            PdfPCell cell_Content_Desc20 = new PdfPCell(new Phrase("" + (rdoOutcomesAll.Checked ? "Print All Services" : "Print Only Services with Counts"), TableFont));
            cell_Content_Desc20.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc20.PaddingBottom = 5;
            outer.AddCell(cell_Content_Desc20);


            document.Add(outer);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

        }

        private void btnRepMaintPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, PrivilegeEntity, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
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
        #region CustomFunctions

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        string Scr_Oper_Mode = "RNGS0014";
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
                _errorProvider.SetError(Ref_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Ref_From_Date, null);

            if (!Ref_To_Date.Checked)
            {
                _errorProvider.SetError(Ref_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
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
                    _errorProvider.SetError(Ref_To_Date, string.Format("Reference Period 'From Date' should not be prior to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (Ref_To_Date.Checked && Ref_From_Date.Checked)
                    _errorProvider.SetError(Ref_To_Date, null);
            }

            //if (Scr_Oper_Mode == "RNGB0004")
            //{
            if (Rb_Fund_Sel.Checked && Sel_Funding_List.Count <= 0)
            {
                _errorProvider.SetError(Rb_Fund_Sel, string.Format("Please Select Atleast One 'Fund'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Fund_Sel, null);
            // }


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
                    _errorProvider.SetError(Rep_From_Date, string.Format("Report Period 'From Date' should be prior to 'TO Date'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
            }
            else
            {
                if (Rep_To_Date.Checked && Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_From_Date, null);
            }


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
            //else
            //{
            //    if (Ref_To_Date.Checked && Ref_From_Date.Checked && Rep_To_Date.Checked && Rep_From_Date.Checked)
            //        _errorProvider.SetError(Rep_From_Date, null);
            //}


            if ((Rb_Zip_Sel.Checked && ListZipCode.Count <= 0 && Scr_Oper_Mode == "RNGB0004") || (Rb_Zip_Sel.Checked && ListGroupCode.Count <= 0 && Scr_Oper_Mode == "RNGS0014"))
            {
                _errorProvider.SetError(Rb_Zip_Sel, string.Format("Please select atleast one " + (Scr_Oper_Mode == "RNGB0004" ? "'ZipCode'" : "'Group'").Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Zip_Sel, null);

            if (Scr_Oper_Mode == "RNGS0004" || Scr_Oper_Mode == "RNGS0014")
            {
                if (Rb_County_Sel.Checked && ListcommonEntity.Count <= 0)
                {
                    _errorProvider.SetError(Rb_County_Sel, string.Format("Please Select Atleast One 'County'".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                    _errorProvider.SetError(Rb_County_Sel, null);
            }

            if (Rb_Site_Sel.Checked && string.IsNullOrEmpty(Txt_Sel_Site.Text.Trim()))
            {
                _errorProvider.SetError(Rb_Site_Sel, string.Format("Please Select Atleast One 'Site'".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rb_Site_Sel, null);

            if (rdoMsselectsite.Checked && string.IsNullOrEmpty(txt_Msselect_site.Text.Trim()))
            {
                _errorProvider.SetError(rdoMsselectsite, string.Format("Please Select Atleast One 'ACT Posting Site'".Replace(Consts.Common.Colon, string.Empty)));
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
                    _errorProvider.SetError(Txt_Pov_High, string.Format("Poverty Level 'Low' Should not Exceed 'High'".Replace(Consts.Common.Colon, string.Empty)));
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
                this.txtHieDesc.Size = new System.Drawing.Size(670, 25);
            }

        }
        private void Get_MasterTable_DateRanges()
        {
            Csb16DTREntity Search_Entity = new Csb16DTREntity(true);
            DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");

            Search_Entity.SYS_Date_Range = DateTime.Today.ToShortDateString();
            Sys_DateRange_List = _model.SPAdminData.Browse_CSB16DTR(Search_Entity, "Browse");
        }
        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                DataColumnCollection columns = Tmp_Table.Columns;
                fillRngCode();
                SetComboBoxValue(cmbRngCode, dr["RNG_MAIN_CODE"].ToString().Trim());
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

                if (dr["DATE_SELECTION"].ToString() == "CADATE")
                    Rb_MS_Date.Checked = true;
                else
                    Rb_MS_AddDate.Checked = true;

                if ((dr["ATTRIBUTES"].ToString() == "SYSTEM" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["ATTRIBUTES"].ToString() == "All" && Scr_Oper_Mode == "RNGS0014"))
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
                    if (Scr_Oper_Mode == "RNGS0014")
                        Prepare_Selected_Group(dr["ZIPCODE"].ToString());
                }


                if ((dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "RNGB0004") ||
                    (dr["COUNTY"].ToString() == "**" && Scr_Oper_Mode == "RNGS0014"))
                    Rb_County_All.Checked = true;
                else
                {
                    Rb_County_Sel.Checked = true;
                    Fill_County_Selected_List(dr["COUNTY"].ToString().Trim());
                }

                if (columns.Contains("REPORTFOR"))
                {
                    rdoperiod.Checked = true;
                    //if (dr["REPORTFOR"].ToString() == "R")
                    //    rdoperiod.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "C")
                        rdoperiodCumulative.Checked = true;
                    if (dr["REPORTFOR"].ToString() == "B")
                        rdoperiodBoth.Checked = true;
                    rdoreportforselection();

                }

                if ((dr["COUNTY"].ToString() == "PM" && Scr_Oper_Mode == "RNGS0014"))
                    Rb_OBO_Mem.Checked = true;
                else if ((dr["COUNTY"].ToString() == "Both" && Scr_Oper_Mode == "RNGS0014"))
                {
                    Rb_SNP_Mem.Checked = true;
                }


                if (dr["DG_COUNT"].ToString() == "PM")
                    Rb_OBO_Mem.Checked = true;
                //**else if (dr["DG_COUNT"].ToString() == "Prog")
                //    rbo_ProgramWise.Checked = true;
                else
                    Rb_SNP_Mem.Checked = true;

                if (Rb_OBO_Mem.Checked)
                {
                    //if(rdoperiod.Checked)
                        chkbUndupTable.Visible = true;
                    spacer6.Visible = true;
                    rdbSummaryDet.Visible = false;
                    rbo_ProgramWise.Visible = false;

                    if (dr["UndupTable"].ToString() == "Y")
                        chkbUndupTable.Checked = true;
                    else
                        chkbUndupTable.Checked = false;
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


                if (columns.Contains("OUTCOME_SWITCH"))
                {
                    if (dr["OUTCOME_SWITCH"].ToString() == "S")
                        rdoOutcomesselect.Checked = true;
                    else
                        rdoOutcomesAll.Checked = true;
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

                if (columns.Contains("ALERTCODES"))
                {
                    if (dr["ALERTCODES"].ToString() == "A")
                        rdbAllAlertCds.Checked = true;
                    else if(dr["ALERTCODES"].ToString() == "S")
                        rdbSelAlertCds.Checked = true;
                }

                if (columns.Contains("SELECTEDALERTCODES"))
                {
                    if (dr["SELECTEDALERTCODES"].ToString() != "")
                    {
                        Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                    }
                }

                if (columns.Contains("REPORTCONTROL"))
                {
                    if (dr["REPORTCONTROL"].ToString() == "Y")
                        chkRepControl.Checked = true;
                    else
                        chkRepControl.Checked = false;
                }

                Get_Selection_Criteria();
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
        // List<SRCsb14GroupEntity> OutCome_MasterList = new List<SRCsb14GroupEntity>();
        private void Prepare_Selected_Group(string Sel_ZipCodes)
        {
            ListGroupCode.Clear();
            OutCome_MasterList = _model.SPAdminData.Browse_RNGSRGrp(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), null, null, null, null, BaseForm.UserID, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());
            foreach (SRCsb14GroupEntity Entity in OutCome_MasterList)
            {
                if (Sel_ZipCodes.Contains(Entity.GrpCode.Trim()))
                    ListGroupCode.Add(new SRCsb14GroupEntity(Entity));
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
        private void Fill_All_List_Arrays()
        {
            zipcode_List = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
            County_List = _model.ZipCodeAndAgency.GetCounty();
            Site_List = _model.CaseMstData.GetCaseSite(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), "SiteHie");
        }
        DG_Browse_Entity Search_Entity = new DG_Browse_Entity(true);
        PM_Browse_Entity PM_Search_Entity = new PM_Browse_Entity(true);
        string[] Sel_params_To_Print = new string[20] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
        private void Get_Selection_Criteria()
        {
            Sel_params_To_Print[4] = string.Empty;
            switch (Scr_Oper_Mode)
            {

                case "RNGS0014":
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
                    if (rbo_ProgramWise.Checked)
                        Search_Entity.PM_Rep_Format = "Prog";

                    Search_Entity.DG_Count_Sw = Search_Entity.PM_Rep_Format;

                    Search_Entity.CA_Fund_Filter = "**";
                    if (Rb_Fund_Sel.Checked)
                        Search_Entity.CA_Fund_Filter = Get_Sel_CA_Fund_List_To_Filter();

                    Search_Entity.alertCodes = "**";
                    if (rdbSelAlertCds.Checked)
                        Search_Entity.alertCodes = SelectedAlertCodes;

                    break;
            }

            Search_Entity.RngMainCode = ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString();
            Search_Entity.Ms_DriveColumn_Sw = "CADATE";
            if (Rb_MS_AddDate.Checked)
                Search_Entity.Ms_DriveColumn_Sw = "CAADDDATE";

            Search_Entity.CA_MS_Sw = "CA";

            if (rdoperiodCumulative.Checked)
            {
                Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); ;
                Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
            }
            else if (rdoperiod.Checked)
            {
                Search_Entity.Rep_From_Date = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_To_Date = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();
            }
            else
            {
                Search_Entity.Rep_From_Date = Convert.ToDateTime(Ref_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_To_Date = Convert.ToDateTime(Ref_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Ref_To_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_FDate = Convert.ToDateTime(Rep_From_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_From_Date.Value.ToShortDateString();
                Search_Entity.Rep_Period_TDate = Convert.ToDateTime(Rep_To_Date.Value.ToString()).ToString("MM/dd/yyyy"); //Rep_To_Date.Value.ToShortDateString();
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
            switch (Search_Entity.CaseMssite)
            {
                case "**": Sel_params_To_Print[3] = "ALL MS Sites"; break;
                case "NO": Sel_params_To_Print[3] = "No MS Sites"; break;
                default: Sel_params_To_Print[3] = "Selected MS Site"; break;
            }

            Search_Entity.OutComeSwitch = rdoOutcomesAll.Checked == true ? "A" : "S";
            Search_Entity.IncVerSwitch = "Y";//chkincverswitch.Checked == true ? "Y" : "N";

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
            foreach (SRCsb14GroupEntity Entity in ListGroupCode)
            {
                Sel_Groups_Codes += "'" + Entity.GrpCode + "',";
            }

            if (Sel_Groups_Codes.Length > 0)
                Sel_Groups_Codes = Sel_Groups_Codes.Substring(0, (Sel_Groups_Codes.Length - 1));

            return Sel_Groups_Codes;
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
            if (chkbUndupTable.Checked == true)
                UndupTable = "Y";

            //string SiteType = string.Empty;
            //if (Rb_Site_All.Checked == true)
            //    SiteType = Search_Entity.Mst_Site;
            //if (Rb_Site_Sel.Checked == true)
            //    SiteType = Search_Entity.Mst_Site;
            //else if (rdoMsselectsite.Checked == true)
            //    SiteType= Search_Entity.Mst_Site;

            string Year = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3, 2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) +
                                      "\" CASE_TYPE = \"" + Search_Entity.Mst_CaseType_Sw + "\" CASE_STATUS = \"" + Search_Entity.Mst_Acive_Sw + "\" DATE_SELECTION = \"" + Search_Entity.Ms_DriveColumn_Sw +
                                      "\" REFERENCE_FROM_DATE = \"" + Search_Entity.Rep_From_Date + "\" REFERENCE_TO_DATE = \"" + Search_Entity.Rep_To_Date + "\" REPORT_FROM_DATE = \"" + Search_Entity.Rep_Period_FDate +
                                      "\" REPORT_TO_DATE = \"" + Search_Entity.Rep_Period_TDate + "\" SITE = \"" + /*SiteType*/Search_Entity.Mst_Site + "\" ATTRIBUTES = \"" + Search_Entity.Attribute + "\" STAT_DETAILS = \"" + Search_Entity.Stat_Detail +
                                      "\" POVERTY_LOW = \"" + Search_Entity.Mst_Poverty_Low + "\" POVERTY_HIGH = \"" + Search_Entity.Mst_Poverty_High + "\" ZIPCODE = \"" + Search_Entity.ZipCode +
                                      "\" COUNTY = \"" + Search_Entity.County + "\" DG_COUNT = \"" + Search_Entity.DG_Count_Sw + "\" SECRET_APP = \"" + Search_Entity.Mst_Secret_Sw + "\" ACTY_PROGRAM = \"" + Search_Entity.Activity_Prog +
                                      "\" CAMS_SW = \"" + Search_Entity.CA_MS_Sw + "\" CAMS_Filter_List = \"" + Search_Entity.CAMS_Filter + "\" FUND_Filter_List = \"" + Search_Entity.CA_Fund_Filter + "\" RNG_MAIN_CODE = \"" + Search_Entity.RngMainCode + "\"  OUTCOME_SWITCH = \"" + (rdoOutcomesselect.Checked == true ? "S" : "A") + "\" CASEMSSITE = \"" + Search_Entity.CaseMssite + "\"  INCVERSWITCH = \"" + Search_Entity.IncVerSwitch + "\" REPORTFOR =\"" + strrptfor + "\" REPORTCONTROL =\"" + strRepControl + "\" UndupTable =\"" + UndupTable + "\" ALERTCODES = \"" + (rdbAllAlertCds.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes + "\" />"); //
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
                //this.txtHieDesc.Size = new System.Drawing.Size(532, 23);
                this.txtHieDesc.Size = new System.Drawing.Size(590, 25);//(714, 25);
            }
            else
                //this.txtHieDesc.Size = new System.Drawing.Size(580, 23);
                this.txtHieDesc.Size = new System.Drawing.Size(670,25);  //(787, 25);
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
        void fillRngCode()
        {
            cmbRngCode.Items.Clear();
            cmbRngCode.ColorMember = "FavoriteColor";
            List<SRCsb14GroupEntity> rngonlycodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == string.Empty && u.TblCode.Trim() == string.Empty);

            int rowIndex = 0; int cnt = 1;
            List<SRCsb14GroupEntity> rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals(strAgency));

            if (rngonlyAgencylist.Count == 0)
                rngonlyAgencylist = rngonlycodelist.FindAll(u => u.Agency.Equals("**"));

            rngonlyAgencylist = rngonlyAgencylist.OrderByDescending(u => u.Active).ToList();

            foreach (SRCsb14GroupEntity rngcodedata in rngonlyAgencylist)
            {
                ListItem li = new ListItem(rngcodedata.GrpDesc, rngcodedata.Code, rngcodedata.Agency, rngcodedata.Active.Equals("Y") ? Color.Black : Color.Red,rngcodedata.PPR_SW);

                if (DateTime.Now >= Convert.ToDateTime(rngcodedata.OFdate.Trim()) && DateTime.Now <= Convert.ToDateTime(rngcodedata.OTdate.Trim()))
                    rowIndex = cnt;

                cmbRngCode.Items.Add(li);

                cnt++;
                //cmbRngCode.Items.Add(new Captain.Common.Utilities.ListItem(rngcodedata.GrpDesc, rngcodedata.Code, rngcodedata.Agency, string.Empty));
            }
            cmbRngCode.Items.Insert(0, new Captain.Common.Utilities.ListItem("", "**"));
            cmbRngCode.SelectedIndex = rowIndex;
        }
        void fill_CaseType_Combo()
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

            OutCome_MasterList = _model.SPAdminData.Browse_RNGSRGrp(Ref_From_Date.Text.Trim(), Ref_To_Date.Text.Trim(), null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
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
        List<DG_Bypass_Entity> DG_Bypass_List = new List<DG_Bypass_Entity>();
        private void Get_DG_Bypass_Table_Structure()
        {
            //	(Byp_Row_ID INT, Byp_Agy VARCHAR(2), Byp_Dept VARCHAR(2), Byp_Prog VARCHAR(2), Byp_App VARCHAR(8), Byp_Fam_Seq Numeric(7), Byp_Client_Name VARCHAR(81), Byp_Site VARCHAR(4), Byp_Attribute VARCHAR(100), 
            //   Byp_Att_Resp VARCHAR(600), Byp_Exc_Reason VARCHAR(100), Byp_Relation VARCHAR(50), Byp_Gender VARCHAR(20), Byp_Ethnic VARCHAR(100), Byp_Race VARCHAR(100), Byp_Education VARCHAR(100), Byp_Health VARCHAR(50), Byp_Disabled VARCHAR(10))

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
            DG_Bypass_List.Add(new DG_Bypass_Entity("Y", "Byp_Attribute", "Attribute", "L", ".9in"));
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
        }
        List<DG_SNP_Bypass_Entity> DG_SNP_Bypass_List = new List<DG_SNP_Bypass_Entity>();
        private void Get_DG_SNP_Bypass_Table_Structure()
        {
            //	DECLARE @Individual_Details_Table TABLE(Ind_Row_ID INT, Ind_Agy VARCHAR(2), Ind_Dept VARCHAR(2), Ind_Prog VARCHAR(2), Ind_App VARCHAR(8), Ind_Fam_Seq Numeric(7), Ind_Client_Name VARCHAR(81), 
            //          Ind_Relation VARCHAR(50), Ind_Date DATE, Ind_Gender VARCHAR(20), Ind_Age VARCHAR(100), Ind_Ethnic VARCHAR(100), Ind_Race VARCHAR(100), Ind_Education VARCHAR(100),
            //          Ind_Health VARCHAR(50), Ind_Disabled VARCHAR(100), Ind_Vet VARCHAR(100), Ind_Food_Stamps VARCHAR(50), Ind_Farmer VARCHAR(100))


            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Row_ID", "Sno", "L", ".3in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Agy", "Ag", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Dept", "De", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Prog", "Pr", "C", ".25in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_App", "App#", "R", ".689in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Fam_Seq", "Seq", "R", ".3in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Client_Name", "Client Name", "L", "2in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Fam_ID", "Family ID", "R", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_CLID", "Client ID", "R", ".9in"));

            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Relation", "Relation", "L", ".95in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Date", "Date", "C", ".75in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Gender", "Gender", "L", ".8in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Age", "Age", "R", ".4in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Ethnic", "Ethnicity", "L", "1.15in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Race", "Race", "L", ".6in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Education", "Education", "L", "2in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Health", "Health Ins", "L", "1.35in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Disabled", "Disabled", "L", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Vet", "Veteran", "L", ".9in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Food_Stamps", "Food Stamps", "L", "1in"));
            DG_SNP_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Ind_Farmer", "Farmer", "L", ".9in"));
        }


        List<DG_SNP_Bypass_Entity> DG_MST_Bypass_List = new List<DG_SNP_Bypass_Entity>();
        private void Get_DG_MST_Bypass_Table_Structure()
        {
            //DECLARE @Family_Details_Table TABLE(Fam_Row_ID INT, Fam_Agy VARCHAR(2), Fam_Dept VARCHAR(2), Fam_Prog VARCHAR(2), Fam_Year VARCHAR(4), Fam_App VARCHAR(8), Ind_Fam_Seq Numeric(7), 
            //        Fam_Client_Name VARCHAR(81), Fam_Date DATE, Fam_Type VARCHAR(100), Fam_Size TINYINT, Fam_Hou_Type VARCHAR(100), Fam_Inc_Type VARCHAR(600), Fam_FPL VARCHAR(100), Fam_Ver_Date DATE)

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Fam_Row_ID", "Sno", "L", ".3in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Agy", "Ag", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Dept", "De", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Prog", "Pr", "C", ".25in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Fam_Year", "App#", "R", ".689in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_App", "App#", "R", ".689in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("N", "Ind_Fam_Seq", "Seq", "R", ".3in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Client_Name", "Client Name", "L", "2in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_FamilyID", "Family ID", "R", ".9in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_CLID", "Client ID", "R", ".9in"));

            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Date", "Date", "C", ".75in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Type", "Family Type", "L", "1.2in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Size", "Fam.Size", "R", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Hou_Type", "Housing Type", "L", "1in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Inc_Type", "Income Types", "L", "1.15in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_FPL", "FPL", "L", ".8in"));
            DG_MST_Bypass_List.Add(new DG_SNP_Bypass_Entity("Y", "Fam_Ver_Date", "Ver.Date", "C", ".75in"));
        }

        List<MSMASTEntity> MS_Mast_List = new List<MSMASTEntity>();
        List<CAMASTEntity> CA_Mast_List = new List<CAMASTEntity>();
        private void Fill_CAMS_Master_List()
        {
            MS_Mast_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);
            CA_Mast_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
        }
        #region PdfReportCode

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        string strReasonCodes = string.Empty;
        string PdfName;

        private void On_SaveForm_Closed(DataTable dtResult, DataTable dtResultBoth)
        {

            strReasonCodes = string.Empty;

            PdfName = "RNGS0014_MainReport";

            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                }
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
            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            cb = writer.DirectContent;

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(bfTimes, 12, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            HeaderPage(document, writer);
            try
            {

                document.NewPage();
                PdfPTable table = new PdfPTable(3);

                table.WidthPercentage = 100;
                table.LockedWidth = true;

                table.TotalWidth = 500f;

                float[] widths = new float[] { 300f, 75f, 75f };
                table.SetWidths(widths);


                // table.HeaderRows = 1;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
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

                CaptainModel _model = new CaptainModel();

                List<SRCsb14GroupEntity> RngGrouplist = new List<SRCsb14GroupEntity>();
                List<SRCsb14GroupEntity> RngtblCodelist = new List<SRCsb14GroupEntity>();
                List<SRCsb14GroupEntity> RngResultlist = new List<SRCsb14GroupEntity>();
                // List<SRCsb14GroupEntity> RngBothResultlist = new List<SRCsb14GroupEntity>();

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


                foreach (DataRow drResultitem in dtResult.Rows)
                {
                    RngResultlist.Add(new SRCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                }


                if (RngResultlist.Count > 0)
                {
                    RngGrouplist = RngResultlist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() == string.Empty && u.Code.Equals(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString()) && u.Row_Type.Equals("GrpDesc"));
                    RngtblCodelist = RngResultlist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() != string.Empty && u.Code.Equals(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString()) && u.Row_Type.Equals("TblDesc"));
                }


                //if (rdoperiodBoth.Checked)
                //{
                //    foreach (DataRow drResultitem in dtResultBoth.Rows)
                //    {
                //        RngBothResultlist.Add(new SRCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                //    }
                //}

                //  List<RNGGAEntity> GoalDetailsEntity = _model.SPAdminData.Browse_RNGGA(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty, string.Empty, string.Empty);

                foreach (SRCsb14GroupEntity codeitem in RngGrouplist)
                {

                    List<ENRL_Asof_Entity> ENRLDateAllList = new List<ENRL_Asof_Entity>();

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


                    PdfPCell pdfMainSubHeader = new PdfPCell(new Phrase(codeitem.GrpDesc.ToUpper() + " ", fcRed));
                    pdfMainSubHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfMainSubHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfMainSubHeader.Colspan = 3;
                    pdfMainSubHeader.FixedHeight = 20;
                    table.AddCell(pdfMainSubHeader);


                    //PdfPCell pdfMainSubHeader1 = new PdfPCell(new Phrase("Name of RNG Eligible Entity Reporting: _______________________", TableFont));
                    //pdfMainSubHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //pdfMainSubHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //pdfMainSubHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //pdfMainSubHeader1.FixedHeight = 20;
                    //pdfMainSubHeader1.Colspan = 3;
                    //table.AddCell(pdfMainSubHeader1);

                    //Added by sudheer on 06/30/2018 by FSCAA document
                    PdfPCell pdfMainSubHeader1 = new PdfPCell(new Phrase("Department/Program: " + DeptName + ProgName, TableFont));
                    pdfMainSubHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainSubHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    pdfMainSubHeader1.FixedHeight = 20;
                    pdfMainSubHeader1.Colspan = 3;
                    table.AddCell(pdfMainSubHeader1);

                    //Added by Sudheer on 06/29/2020
                    PdfPCell pdfMainSubHeader2 = new PdfPCell(new Phrase("Name of CSBG Eligible Entity Reporting: ", TblFontBold));
                    pdfMainSubHeader2.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainSubHeader2.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader2.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainSubHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    pdfMainSubHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //pdfMainSubHeader2.FixedHeight = 20;
                    //pdfMainSubHeader2.Colspan = 3;
                    table.AddCell(pdfMainSubHeader2);

                    PdfPCell pdfMainSubHeader3 = new PdfPCell(new Phrase(Agency, TableFont));
                    pdfMainSubHeader3.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainSubHeader3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader3.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainSubHeader3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    pdfMainSubHeader3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //pdfMainSubHeader3.FixedHeight = 20;
                    pdfMainSubHeader3.Colspan = 2;
                    table.AddCell(pdfMainSubHeader3);

                    PdfPCell pdfMainSubHeader4 = new PdfPCell(new Phrase("State: ", TblFontBold));
                    pdfMainSubHeader4.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainSubHeader4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader4.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainSubHeader4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    pdfMainSubHeader4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //pdfMainSubHeader4.FixedHeight = 20;
                    //pdfMainSubHeader4.Colspan = 3;
                    table.AddCell(pdfMainSubHeader4);

                    PdfPCell pdfMainSubHeader5 = new PdfPCell(new Phrase("DUNS: ", TblFontBold));
                    pdfMainSubHeader5.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainSubHeader5.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainSubHeader5.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainSubHeader5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    pdfMainSubHeader5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    //pdfMainSubHeader5.FixedHeight = 20;
                    pdfMainSubHeader5.Colspan = 2;
                    table.AddCell(pdfMainSubHeader5);

                    PdfPCell pdfMainTableSubHeader1 = new PdfPCell(new Phrase(codeitem.GrpDesc, TblFontBold));
                    pdfMainTableSubHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfMainTableSubHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainTableSubHeader1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                    pdfMainTableSubHeader1.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainTableSubHeader1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                    pdfMainTableSubHeader1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                    //pdfMainTableSubHeader1.FixedHeight = 20;
                    table.AddCell(pdfMainTableSubHeader1);

                    PdfPCell pdfMainTableHeader2 = new PdfPCell(new Phrase("Target ", TblFontBold));
                    pdfMainTableHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfMainTableHeader2.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfMainTableHeader2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                    //pdfMainTableHeader2.FixedHeight = 20;
                    pdfMainTableHeader2.Border = iTextSharp.text.Rectangle.BOX;
                    pdfMainTableHeader2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                    pdfMainTableHeader2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                    table.AddCell(pdfMainTableHeader2);


                    if (rdoperiodBoth.Checked)
                    {
                        PdfPTable NestedTable = new PdfPTable(2);
                        NestedTable.TotalWidth = 75f;
                        NestedTable.LockedWidth = true;
                        float[] N2Dwidths = new float[] { 37f, 38f };
                        NestedTable.SetWidths(N2Dwidths);

                        PdfPCell D1 = new PdfPCell(new Phrase("Number of Individuals Served", TblFontBold));
                        //D1.FixedHeight = 10f;
                        D1.Colspan = 2;
                        D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        D1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        NestedTable.AddCell(D1);

                        PdfPCell sD1 = new PdfPCell(new Phrase("Rept.", TblFontBold));
                        sD1.FixedHeight = 12f;//10f;
                        sD1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                        sD1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        NestedTable.AddCell(sD1);

                        PdfPCell sD2 = new PdfPCell(new Phrase("Ref.", TblFontBold));
                        sD2.FixedHeight = 12f;//10f;
                        sD2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        // sD2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        sD2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        NestedTable.AddCell(sD2);

                        PdfPCell R11 = new PdfPCell(NestedTable);
                        R11.Padding = 0f;
                        R11.Colspan = 1;
                        R11.Border = iTextSharp.text.Rectangle.BOX;
                        R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        table.AddCell(R11);
                    }
                    else
                    {

                        PdfPCell pdfMainTableHeader1 = new PdfPCell(new Phrase("Number of Individuals Served", TblFontBold));
                        pdfMainTableHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfMainTableHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //pdfMainTableHeader1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                        //pdfMainTableHeader1.FixedHeight = 20;
                        pdfMainTableHeader1.Border = iTextSharp.text.Rectangle.BOX;
                        pdfMainTableHeader1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                        pdfMainTableHeader1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        table.AddCell(pdfMainTableHeader1);
                    }

                    RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() != string.Empty && u.Agency == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).ID.ToString());


                    bool boolOutComesDetails = true;

                    string intcount1, intcount2, intcount3, intResultTotal1, intResulttotal2;
                    foreach (SRCsb14GroupEntity tblEnt in RngtblCodelist)
                    {

                        if (rdoOutcomesselect.Checked)
                        {
                            boolOutComesDetails = false;
                            if (rdoperiodBoth.Checked)
                            {
                                SRCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                if (rngcountdata != null)
                                    boolOutComesDetails = true;
                            }
                            else
                            {
                                SRCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                if (rngcountdata != null)
                                    boolOutComesDetails = true;
                            }
                        }

                        if (boolOutComesDetails)
                        {

                            //**PdfPCell pdfDetailsTable1 = new PdfPCell(new Phrase(tblEnt.TblCode.Trim() + "." + tblEnt.GrpDesc.ToString(), TblFontBold));
                            PdfPCell pdfDetailsTable1 = new PdfPCell(new Phrase(tblEnt.GrpDesc.ToString(), TblFontBold));
                            pdfDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfDetailsTable1.VerticalAlignment = Element.ALIGN_MIDDLE;
                            // pdfDetailsTable1.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                            pdfDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                            pdfDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            pdfDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            table.AddCell(pdfDetailsTable1);

                            intcount1 = intcount2 = intcount3 = intResultTotal1 = intResulttotal2 = "0";
                            SRCsb14GroupEntity rngcountdata = null;



                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == "ZZZZZTotal" && u.Row_Type.ToString() == "GrpTotal" && u.Count_type.ToString().Trim() == "TotCnt");
                                //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                            }
                            else
                            {
                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                            }
                            if (rngcountdata != null)
                            {
                                if (!string.IsNullOrEmpty(rngcountdata.unit_cnt.ToString().Trim()))
                                    intcount1 = rngcountdata.unit_cnt.ToString();
                                else
                                    intcount1 = "0";
                                intcount2 = rngcountdata.ExAchev.ToString();
                                if (!string.IsNullOrEmpty(rngcountdata.per_Achived.ToString().Trim()))
                                    intcount3 = rngcountdata.per_Achived.ToString();
                                else
                                    intcount3 = "0";
                                //intcout3 = rngcountdata.Hrd1.ToString();
                                if (rdoperiodBoth.Checked)
                                {
                                    SRCsb14GroupEntity rngBothcountdata = null;
                                    if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                                    {
                                        rngBothcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == "ZZZZZTotal" && u.Row_Type.ToString() == "GrpTotal" && u.Count_type.ToString().Trim() == "TotRefCnt");
                                        //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    }
                                    else
                                    {
                                        rngBothcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                    }
                                    if (rngBothcountdata != null)
                                    {
                                        if (!string.IsNullOrEmpty(rngBothcountdata.unit_cnt.ToString().Trim()))
                                            intcount1 = rngBothcountdata.unit_cnt.ToString();
                                        else
                                            intcount1 = "0";
                                    }
                                }

                            }

                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase("", TblFontBold));
                                pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                pdfDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfDetailsTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                table.AddCell(pdfDetailsTable2);
                            }
                            else
                            {
                                if (intcount2 == "0")
                                    intcount2 = "";

                                PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intcount2, TblFontBold));
                                pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                pdfDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                pdfDetailsTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                table.AddCell(pdfDetailsTable2);
                            }

                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                if (rdoperiodBoth.Checked)
                                {
                                    PdfPTable NestedTable = new PdfPTable(2);
                                    NestedTable.TotalWidth = 75f;
                                    NestedTable.LockedWidth = true;
                                    float[] N2Dwidths = new float[] { 37f, 38f };
                                    NestedTable.SetWidths(N2Dwidths);


                                    SRCsb14GroupEntity rngtotalcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    if (rngtotalcountdata != null)
                                    {
                                        intResultTotal1 = rngtotalcountdata.unit_cnt;
                                        //intResultTotal1 = rngtotalcountdata.per_Achived;
                                        //intResulttotal2 = rngtotalcountdata.per_Achived;
                                    }

                                    SRCsb14GroupEntity rngtotalBothcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblRefDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    if (rngtotalBothcountdata != null)
                                    {
                                        intResulttotal2 = rngtotalBothcountdata.per_Achived;
                                    }

                                    PdfPCell sD1 = new PdfPCell(new Phrase(intResultTotal1, TblFontBold));
                                    sD1.FixedHeight = 12f;//10f;
                                    sD1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    sD1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    sD1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    sD1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    NestedTable.AddCell(sD1);

                                    //SRCsb14GroupEntity rngtotalBothcountdata = RngBothResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    //if (rngtotalBothcountdata != null)


                                    PdfPCell sD2 = new PdfPCell(new Phrase(intResulttotal2, TblFontBold));
                                    sD2.FixedHeight = 12f;//10f;
                                    sD2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    sD2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    //sD2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                    NestedTable.AddCell(sD2);

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.Padding = 0f;
                                    R11.Colspan = 1;
                                    R11.Border = iTextSharp.text.Rectangle.BOX;
                                    R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    table.AddCell(R11);
                                }
                                else if (rdoperiodCumulative.Checked)
                                {
                                    SRCsb14GroupEntity rngtotalcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    if (rngtotalcountdata != null)
                                        intResultTotal1 = rngtotalcountdata.per_Achived;
                                    PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intResultTotal1, TblFontBold));
                                    pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    // pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                    pdfDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    pdfDetailsTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    table.AddCell(pdfDetailsTable2);
                                }
                                else
                                {
                                    SRCsb14GroupEntity rngtotalcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    if (rngtotalcountdata != null)
                                        intResultTotal1 = rngtotalcountdata.unit_cnt;
                                    PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intResultTotal1, TblFontBold));
                                    pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    // pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                    pdfDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    pdfDetailsTable2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    table.AddCell(pdfDetailsTable2);
                                }
                            }
                            else
                            {
                                if (rdoperiodBoth.Checked)
                                {
                                    if (intcount1 == "0")
                                        intcount1 = "";
                                    if (intcount3 == "0")
                                        intcount3 = "";
                                    PdfPTable NestedTable = new PdfPTable(2);
                                    NestedTable.TotalWidth = 75f;
                                    NestedTable.LockedWidth = true;
                                    float[] N2Dwidths = new float[] { 37f, 38f };
                                    NestedTable.SetWidths(N2Dwidths);

                                    PdfPCell sD1 = new PdfPCell(new Phrase(intcount3, TableFont));
                                    sD1.FixedHeight = 12f;//10f;
                                    sD1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    sD1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    NestedTable.AddCell(sD1);

                                    PdfPCell sD2 = new PdfPCell(new Phrase(intcount1, TableFont));
                                    sD2.FixedHeight = 12f;//10f;
                                    sD2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    sD2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    sD2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    //sD2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                    NestedTable.AddCell(sD2);

                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                    R11.Padding = 0f;
                                    R11.Colspan = 1;
                                    R11.Border = iTextSharp.text.Rectangle.BOX;
                                    R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    table.AddCell(R11);
                                }
                                else if (rdoperiodCumulative.Checked)
                                {
                                    if (intcount3 == "0")
                                        intcount3 = "";
                                    PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intcount3, TblFontBold));
                                    pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(pdfDetailsTable2);
                                }
                                else
                                {
                                    if (intcount1 == "0")
                                        intcount1 = "";
                                    PdfPCell pdfDetailsTable2 = new PdfPCell(new Phrase(intcount1, TblFontBold));
                                    pdfDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(pdfDetailsTable2);
                                }
                            }


                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                List<SRCsb14GroupEntity> rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                List<SRCsb14GroupEntity> itemBothDetails = new List<SRCsb14GroupEntity>();
                                if (rdoperiodBoth.Checked)
                                {
                                    itemBothDetails = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                }
                                //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                bool boolservicesubdetails = true;
                                int intBothcountdata = 0;
                                foreach (SRCsb14GroupEntity item in rngsubdetailsdata)
                                {
                                    if (rdoOutcomesselect.Checked)
                                    {

                                        boolservicesubdetails = false;
                                        if (rdoperiodBoth.Checked)
                                        {
                                            if (itemBothDetails[intBothcountdata].CalCost.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].CalCost != "0")
                                            {
                                                boolservicesubdetails = true;
                                            }
                                            else if (itemBothDetails[intBothcountdata].unit_cnt.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].unit_cnt != "0")
                                                boolservicesubdetails = true;
                                        }
                                        else
                                        {
                                            if (item.CalCost.ToString().Trim() != string.Empty && item.CalCost != "0")
                                            {
                                                boolservicesubdetails = true;
                                            }
                                            else if (item.unit_cnt.ToString().Trim() != string.Empty && item.unit_cnt != "0")
                                                boolservicesubdetails = true;
                                        }
                                    }
                                    //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt" && u.GrpDesc.ToString().Trim() == item.Desc.ToString().Trim());
                                    //intcount1 = intcount2 = intcout3 = "0";
                                    //if (rngcountdata != null)
                                    //{
                                    //    intcount1 = rngcountdata.unit_cnt.ToString();
                                    //    intcout3 = rngcountdata.Hrd1.ToString();
                                    //}
                                    if (boolservicesubdetails)
                                    {
                                        PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("      +" + item.GrpDesc.ToString(), TableFont));
                                        pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSubDetailsTable1.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafdff"));
                                        pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                        pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                        table.AddCell(pdfSubDetailsTable1);

                                        string xx = item.ExAchev.ToString() == "0" ? "" : item.ExAchev.ToString();
                                        PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase("  " + xx, TableFont));
                                        pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                        pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));

                                        table.AddCell(pdfSubDetailsTable3);

                                        if (rdoperiodBoth.Checked)
                                        {


                                            string x = "";
                                            SRCsb14GroupEntity itemBoth = itemBothDetails[intBothcountdata];
                                            if (itemBoth != null)
                                            {
                                                if (!string.IsNullOrEmpty(itemBoth.CalCost.Trim()))
                                                {
                                                    if (decimal.Parse(item.CalCost) > 0)
                                                        x = itemBoth.CalCost.ToString() == "0" ? "" : itemBoth.CalCost.ToString();
                                                    else
                                                        x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                }
                                                else
                                                    x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                            }

                                            //x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                            //string Y = item.per_Achived.ToString() == "0" ? "" : item.per_Achived.ToString();
                                            string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                          //if (itemBoth != null)
                                                          //{
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            //}
                                            PdfPTable NestedTable = new PdfPTable(2);
                                            NestedTable.TotalWidth = 75f;
                                            NestedTable.LockedWidth = true;
                                            float[] N2Dwidths = new float[] { 37f, 38f };
                                            NestedTable.SetWidths(N2Dwidths);


                                            PdfPCell sD1 = new PdfPCell(new Phrase(Y, TableFont));
                                            sD1.FixedHeight = 12f;//10f;
                                            sD1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            sD1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            sD1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            NestedTable.AddCell(sD1);

                                            PdfPCell sD2 = new PdfPCell(new Phrase(x, TableFont));
                                            sD2.FixedHeight = 12f;//10f;
                                            sD2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            sD2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //sD2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                            NestedTable.AddCell(sD2);

                                            PdfPCell R11 = new PdfPCell(NestedTable);
                                            R11.Padding = 0f;
                                            R11.Colspan = 1;
                                            R11.Border = iTextSharp.text.Rectangle.BOX;
                                            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            table.AddCell(R11);

                                        }
                                        else if (rdoperiodCumulative.Checked)
                                        {
                                            string Y = "";//item.per_Achived.ToString() == "0" ? "" : item.per_Achived.ToString();
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();

                                            PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase("  " + Y, TableFont));
                                            pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            table.AddCell(pdfSubDetailsTable2);
                                        }
                                        else
                                        {
                                            string x = "";// item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    x = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase("  " + x, TableFont));
                                            pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                            pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                            table.AddCell(pdfSubDetailsTable2);
                                        }
                                    }
                                    intBothcountdata = intBothcountdata + 1;

                                    if (rbo_ProgramWise.Checked)
                                    {
                                        List<SRCsb14GroupEntity> rngProgsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.OutcomeCode.ToString().Trim() == item.OutcomeCode.Trim() && u.Count_type.ToString().Trim() == "PrgCnt");
                                        List<SRCsb14GroupEntity> itemProgBothDetails = new List<SRCsb14GroupEntity>();
                                        if (rdoperiodBoth.Checked)
                                        {
                                            itemProgBothDetails = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.OutcomeCode.ToString().Trim() == item.OutcomeCode.Trim() && u.Count_type.ToString().Trim() == "RegCnt");
                                        }
                                        //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                        bool boolserviceProgsubdetails = true;
                                        int intProgBothcountdata = 0;
                                        foreach (SRCsb14GroupEntity itemProg in rngProgsubdetailsdata)
                                        {
                                            if (rdoOutcomesselect.Checked)
                                            {

                                                boolserviceProgsubdetails = false;
                                                if (rdoperiodBoth.Checked)
                                                {
                                                    if (itemBothDetails[intBothcountdata].CalCost.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].CalCost != "0")
                                                    {
                                                        boolservicesubdetails = true;
                                                    }
                                                    else if (itemProgBothDetails[intProgBothcountdata].unit_cnt.ToString().Trim() != string.Empty && itemProgBothDetails[intProgBothcountdata].unit_cnt != "0")
                                                        boolserviceProgsubdetails = true;
                                                }
                                                else
                                                {
                                                    if (item.CalCost.ToString().Trim() != string.Empty && item.CalCost != "0")
                                                    {
                                                        boolservicesubdetails = true;
                                                    }
                                                    else if (itemProg.unit_cnt.ToString().Trim() != string.Empty && itemProg.unit_cnt != "0")
                                                        boolserviceProgsubdetails = true;
                                                }
                                            }
                                            //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt" && u.GrpDesc.ToString().Trim() == item.Desc.ToString().Trim());
                                            //intcount1 = intcount2 = intcout3 = "0";
                                            //if (rngcountdata != null)
                                            //{
                                            //    intcount1 = rngcountdata.unit_cnt.ToString();
                                            //    intcout3 = rngcountdata.Hrd1.ToString();
                                            //}
                                            if (boolserviceProgsubdetails)
                                            {
                                                PdfPCell pdfSubDetailsTable1 = new PdfPCell(new Phrase("        " + itemProg.GrpDesc.ToString(), TableFont));
                                                pdfSubDetailsTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfSubDetailsTable1.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                pdfSubDetailsTable1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafdff"));
                                                pdfSubDetailsTable1.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                table.AddCell(pdfSubDetailsTable1);

                                                string xx = itemProg.ExAchev.ToString() == "0" ? "" : itemProg.ExAchev.ToString();
                                                PdfPCell pdfSubDetailsTable3 = new PdfPCell(new Phrase("  " + xx, TableFont));
                                                pdfSubDetailsTable3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                pdfSubDetailsTable3.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                pdfSubDetailsTable3.Border = iTextSharp.text.Rectangle.BOX;
                                                pdfSubDetailsTable3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                table.AddCell(pdfSubDetailsTable3);

                                                if (rdoperiodBoth.Checked)
                                                {


                                                    string x = "";
                                                    SRCsb14GroupEntity itemBoth = itemProgBothDetails[intProgBothcountdata];
                                                    if (itemBoth != null)
                                                    {
                                                        if (!string.IsNullOrEmpty(itemBoth.CalCost.Trim()) && itemBoth.CalCost != "0")
                                                            x = itemBoth.CalCost.ToString() == "0" ? "" : itemBoth.CalCost.ToString();
                                                        else
                                                            x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                    }
                                                    //x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                    ////string Y = item.per_Achived.ToString() == "0" ? "" : item.per_Achived.ToString();
                                                    string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                                  //if (itemBoth != null)
                                                                  //{
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()) && item.CalCost != "0")
                                                        Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                    else
                                                        Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    //}
                                                    PdfPTable NestedTable = new PdfPTable(2);
                                                    NestedTable.TotalWidth = 75f;
                                                    NestedTable.LockedWidth = true;
                                                    float[] N2Dwidths = new float[] { 37f, 38f };
                                                    NestedTable.SetWidths(N2Dwidths);


                                                    PdfPCell sD1 = new PdfPCell(new Phrase(Y, TableFont));
                                                    sD1.FixedHeight = 12f;//10f;
                                                    sD1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    sD1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    sD1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    NestedTable.AddCell(sD1);

                                                    PdfPCell sD2 = new PdfPCell(new Phrase(x, TableFont));
                                                    sD2.FixedHeight = 12f;//10f;
                                                    sD2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    sD2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    sD2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    //sD2.BackgroundColor = new iTextSharp.text.BaseColor(242, 243, 244);
                                                    NestedTable.AddCell(sD2);

                                                    PdfPCell R11 = new PdfPCell(NestedTable);
                                                    R11.Padding = 0f;
                                                    R11.Colspan = 1;
                                                    R11.Border = iTextSharp.text.Rectangle.BOX;
                                                    R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    table.AddCell(R11);

                                                }
                                                else if (rdoperiodCumulative.Checked)
                                                {
                                                    string Y = "";//item.per_Achived.ToString() == "0" ? "" : item.per_Achived.ToString();
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()) && item.CalCost != "0")
                                                        Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                    else
                                                        Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase("  " + Y, TableFont));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    table.AddCell(pdfSubDetailsTable2);
                                                }
                                                else
                                                {
                                                    string x = "";// item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()) && item.CalCost != "0")
                                                        x = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                    else
                                                        x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    PdfPCell pdfSubDetailsTable2 = new PdfPCell(new Phrase("  " + x, TableFont));
                                                    pdfSubDetailsTable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    pdfSubDetailsTable2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    pdfSubDetailsTable2.Border = iTextSharp.text.Rectangle.BOX;
                                                    pdfSubDetailsTable2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                                    table.AddCell(pdfSubDetailsTable2);
                                                }
                                            }
                                            intProgBothcountdata = intProgBothcountdata + 1;
                                        }

                                    }
                                }

                            }

                        }
                    }
                    if (table.Rows.Count > 0)
                    {
                        document.Add(table);
                        table.DeleteBodyRows();
                        document.NewPage();
                    }

                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            //document.Add(table);                
            document.Close();
            fs.Close();
            fs.Dispose();

            if (rdoperiodBoth.Checked)
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, dt, Result_Table, Ser_Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked, "RNGS0014", rdoperiodBoth.Checked, Rb_OBO_Mem.Checked, Search_Entity.Ms_DriveColumn_Sw, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString());
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName, dt, Result_Table, Ser_Rep_Name, "Result Table", ReportPath, BaseForm.UserID, Rb_Details_Yes.Checked,"RNGS0014", ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).DefaultValue.ToString(), string.Empty, Rb_OBO_Mem.Checked, Search_Entity.Ms_DriveColumn_Sw);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }

        }


        private void On_SaveExcel_Closed(DataTable dtResult, DataTable dtResultBoth)
        {

            strReasonCodes = string.Empty;

            PdfName = "RNGS0014_ExcelReport";

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


                List<CommonEntity> commonHeaderlist = new List<CommonEntity>();


                List<SRCsb14GroupEntity> RngGrouplist = new List<SRCsb14GroupEntity>();
                List<SRCsb14GroupEntity> RngtblCodelist = new List<SRCsb14GroupEntity>();
                List<SRCsb14GroupEntity> RngResultlist = new List<SRCsb14GroupEntity>();
                //List<SRCsb14GroupEntity> RngBothResultlist = new List<SRCsb14GroupEntity>();


                excelrow = sheet.Table.Rows.Add();

                //excelcolumn = excelcolumn + 1;
                //xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 1, "AGENCY");




                foreach (DataRow drResultitem in dtResult.Rows)
                {
                    RngResultlist.Add(new SRCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                }


                if (RngResultlist.Count > 0)
                {
                    RngGrouplist = RngResultlist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() == string.Empty && u.Code.Equals(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString()) && u.Row_Type.Equals("GrpDesc"));
                    RngtblCodelist = RngResultlist.FindAll(u => u.GrpCode.Trim() != string.Empty && u.TblCode.Trim() != string.Empty && u.Code.Equals(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString()) && u.Row_Type.Equals("TblDesc"));
                }


                if (rdoperiodBoth.Checked)
                {
                    //foreach (DataRow drResultitem in dtResultBoth.Rows)
                    //{
                    //    RngBothResultlist.Add(new SRCsb14GroupEntity(drResultitem, ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty));
                    //}
                }

                //  List<RNGGAEntity> GoalDetailsEntity = _model.SPAdminData.Browse_RNGGA(((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString(), string.Empty, string.Empty, string.Empty);

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

                foreach (SRCsb14GroupEntity codeitem in RngGrouplist)
                {

                    List<ENRL_Asof_Entity> ENRLDateAllList = new List<ENRL_Asof_Entity>();


                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add(codeitem.GrpDesc.ToUpper() + " ", DataType.String, "MainHeaderStyles");
                    cell.MergeAcross = 3;
                    excelrow.Height = 25;



                    //PdfPCell pdfMainSubHeader1 = new PdfPCell(new Phrase("Name of RNG Eligible Entity Reporting: _______________________", TableFont));
                    //pdfMainSubHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //pdfMainSubHeader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //pdfMainSubHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //pdfMainSubHeader1.FixedHeight = 20;
                    //pdfMainSubHeader1.Colspan = 3;
                    //table.AddCell(pdfMainSubHeader1);

                    //Added by sudheer on 06/30/2018 by FSCAA document

                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("Department/Program: " + DeptName + ProgName, DataType.String, "NormalLeft1");
                    cell.MergeAcross = 3;

                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("Name of CSBG Eligible Entity Reporting: " , DataType.String, "NormalLeft1");
                    cell = excelrow.Cells.Add(Agency, DataType.String, "NormalLeft1");
                    cell.MergeAcross = 2;

                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("State: ", DataType.String, "NormalLeft1");
                    cell = excelrow.Cells.Add("DUNS", DataType.String, "NormalLeft1");
                    cell.MergeAcross = 2;



                    excelrow = sheet.Table.Rows.Add();
                    excelrow.Height = 20;
                    cell = excelrow.Cells.Add(codeitem.GrpDesc, DataType.String, "Normalcenter");


                    cell = excelrow.Cells.Add("Target", DataType.String, "Normalcenter");



                    if (rdoperiodBoth.Checked)
                    {

                        cell = excelrow.Cells.Add("Number of Individuals Served", DataType.String, "Normalcenter");
                        cell.MergeAcross = 1;


                        excelrow = sheet.Table.Rows.Add();
                        cell = excelrow.Cells.Add("Rept.", DataType.String, "Normalcenter");
                        cell.MergeAcross = 2;

                        cell = excelrow.Cells.Add("Ref.", DataType.String, "Normalcenter");

                    }
                    else
                    {
                        cell = excelrow.Cells.Add("Number of Individuals Served", DataType.String, "Normalcenter");
                        cell.MergeAcross = 1;
                    }

                    RngtblCodelist = RngCodelist.FindAll(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() != string.Empty);


                    bool boolOutComesDetails = true;

                    string intcount1, intcount2, intcount3;
                    foreach (SRCsb14GroupEntity tblEnt in RngtblCodelist)
                    {

                        if (rdoOutcomesselect.Checked)
                        {
                            boolOutComesDetails = false;
                            if (rdoperiodBoth.Checked)
                            {
                                SRCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                if (rngcountdata != null)
                                    boolOutComesDetails = true;
                            }
                            else
                            {
                                SRCsb14GroupEntity rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == codeitem.GrpCode.Trim() && u.Code.Trim() == codeitem.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.unit_cnt.Trim() != "" && u.unit_cnt.Trim() != "0" && u.unit_cnt.Trim().ToUpper() != "UNIT COUNT");
                                if (rngcountdata != null)
                                    boolOutComesDetails = true;
                            }
                        }

                        if (boolOutComesDetails)
                        {

                            excelrow = sheet.Table.Rows.Add();
                            //**cell = excelrow.Cells.Add(tblEnt.TblCode.Trim() + "." + tblEnt.GrpDesc.ToString(), DataType.String, "Normal");
                            cell = excelrow.Cells.Add(tblEnt.GrpDesc.ToString(), DataType.String, "Normal");


                            intcount1 = intcount2 = intcount3 = "0";
                            SRCsb14GroupEntity rngcountdata = null;



                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == "ZZZZZTotal" && u.Row_Type.ToString() == "GrpTotal" && u.Count_type.ToString().Trim() == "TotCnt");
                                //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                            }
                            else
                            {
                                rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                            }
                            if (rngcountdata != null)
                            {
                                if (!string.IsNullOrEmpty(rngcountdata.unit_cnt.ToString().Trim())) intcount1 = rngcountdata.unit_cnt.ToString(); else intcount1 = "0";
                                intcount2 = rngcountdata.ExAchev.ToString();
                                if (!string.IsNullOrEmpty(rngcountdata.per_Achived.ToString().Trim())) intcount3 = rngcountdata.per_Achived.ToString(); else intcount3 = "0";
                                //intcout3 = rngcountdata.Hrd1.ToString();
                                if (rdoperiodBoth.Checked)
                                {
                                    SRCsb14GroupEntity rngBothcountdata = null;
                                    if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                                    {
                                        rngBothcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == "ZZZZZTotal" && u.Row_Type.ToString() == "GrpTotal" && u.Count_type.ToString().Trim() == "TotRefCnt");
                                        
                                        //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDesc" && u.Count_type.ToString().Trim() == string.Empty);
                                    }
                                    else
                                    {
                                        rngBothcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                    }
                                    if (rngBothcountdata != null)
                                    {
                                        if (!string.IsNullOrEmpty(rngBothcountdata.unit_cnt.ToString().Trim())) intcount1 = rngBothcountdata.unit_cnt.ToString(); else intcount1 = "0";
                                    }
                                }

                            }

                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {

                                cell = excelrow.Cells.Add("", DataType.String, "Normal");
                            }
                            else
                            {
                                if (intcount2 == "0") intcount2 = "";
                                cell = excelrow.Cells.Add(intcount2, DataType.String, "Normal");

                            }

                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                if (rdoperiodBoth.Checked)
                                {
                                    cell = excelrow.Cells.Add("", DataType.String, "Normal");

                                    cell = excelrow.Cells.Add("", DataType.String, "Normal");

                                }
                                else if (rdoperiodCumulative.Checked)
                                {
                                    cell = excelrow.Cells.Add("", DataType.String, "Normal");
                                    cell.MergeAcross = 1;
                                }
                                else
                                {
                                    cell = excelrow.Cells.Add("", DataType.String, "Normal");
                                    cell.MergeAcross = 1;
                                }
                            }
                            else
                            {
                                if (rdoperiodBoth.Checked)
                                {

                                    if (intcount1 == "0") intcount1 = "";
                                    if (intcount3 == "0") intcount3 = "";

                                    cell = excelrow.Cells.Add(intcount1, DataType.String, "Normal");

                                    cell = excelrow.Cells.Add(intcount3, DataType.String, "Normal");

                                }
                                else if (rdoperiodCumulative.Checked)
                                {
                                    if (intcount3 == "0") intcount3 = "";
                                    cell = excelrow.Cells.Add(intcount3, DataType.String, "Normal");
                                    cell.MergeAcross = 1;
                                }
                                else
                                {
                                    if (intcount1 == "0") intcount1 = "";
                                    cell = excelrow.Cells.Add(intcount1, DataType.String, "Normal");
                                    cell.MergeAcross = 1;
                                }
                            }


                            if (Rb_SNP_Mem.Checked && (rdbSummaryDet.Checked || rbo_ProgramWise.Checked))
                            {
                                List<SRCsb14GroupEntity> rngsubdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt");
                                List<SRCsb14GroupEntity> itemBothDetails = new List<SRCsb14GroupEntity>();
                                if (rdoperiodBoth.Checked)
                                {
                                    itemBothDetails = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                }
                                //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                bool boolservicesubdetails = true;
                                int intBothcountdata = 0;
                                foreach (SRCsb14GroupEntity item in rngsubdetailsdata)
                                {
                                    if (rdoOutcomesselect.Checked)
                                    {

                                        boolservicesubdetails = false;
                                        if (rdoperiodBoth.Checked)
                                        {
                                            if (itemBothDetails[intBothcountdata].CalCost.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].CalCost != "0")
                                            {
                                                boolservicesubdetails = true;
                                            }
                                            else if (itemBothDetails[intBothcountdata].unit_cnt.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].unit_cnt != "0")
                                                boolservicesubdetails = true;
                                        }
                                        else
                                        {
                                            if (item.CalCost.ToString().Trim() != string.Empty && item.CalCost != "0")
                                            { boolservicesubdetails = true; }
                                            else if (item.unit_cnt.ToString().Trim() != string.Empty && item.unit_cnt != "0")
                                                boolservicesubdetails = true;
                                        }
                                    }
                                    //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt" && u.GrpDesc.ToString().Trim() == item.Desc.ToString().Trim());
                                    //intcount1 = intcount2 = intcout3 = "0";
                                    //if (rngcountdata != null)
                                    //{
                                    //    intcount1 = rngcountdata.unit_cnt.ToString();
                                    //    intcout3 = rngcountdata.Hrd1.ToString();
                                    //}
                                    if (boolservicesubdetails)
                                    {
                                        excelrow = sheet.Table.Rows.Add();
                                        cell = excelrow.Cells.Add("      +" + item.GrpDesc.ToString(), DataType.String, "Normal");

                                        string xx = item.ExAchev.ToString() == "0" ? "" : item.ExAchev.ToString();

                                        cell = excelrow.Cells.Add("  " + xx, DataType.String, "Normal");

                                        if (rdoperiodBoth.Checked)
                                        {
                                            string x = "";
                                            SRCsb14GroupEntity itemBoth = itemBothDetails[intBothcountdata];
                                            if (itemBoth != null)
                                            {
                                                if (!string.IsNullOrEmpty(itemBoth.CalCost.Trim()))
                                                {
                                                    if (decimal.Parse(itemBoth.CalCost) > 0)
                                                        x = itemBoth.CalCost.ToString() == "0" ? "" : itemBoth.CalCost.ToString();
                                                    else
                                                        x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                }
                                                else
                                                    x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                            }
                                            //x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                            string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                          //if (itemBoth != null)
                                                          //{
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            cell = excelrow.Cells.Add(x, DataType.String, "Normal");
                                            cell = excelrow.Cells.Add(Y, DataType.String, "Normal");
                                        }
                                        else if (rdoperiodCumulative.Checked)
                                        {
                                            string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                          //if (itemBoth != null)
                                                          //{
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            cell = excelrow.Cells.Add(Y, DataType.String, "Normal");
                                            cell.MergeAcross = 1;
                                        }
                                        else
                                        {
                                            string x = "";// item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                            {
                                                if (decimal.Parse(item.CalCost) > 0)
                                                    x = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                else
                                                    x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            }
                                            else
                                                x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                            cell = excelrow.Cells.Add("  " + x, DataType.String, "Normal");
                                            cell.MergeAcross = 1;

                                        }
                                    }
                                    intBothcountdata = intBothcountdata + 1;
                                    if (rbo_ProgramWise.Checked)
                                    {
                                        List<SRCsb14GroupEntity> rngsubProgdetailsdata = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.OutcomeCode.ToString().Trim()==item.OutcomeCode.Trim() && u.Count_type.ToString().Trim() == "PrgCnt");
                                        List<SRCsb14GroupEntity> itemProgBothDetails = new List<SRCsb14GroupEntity>();
                                        if (rdoperiodBoth.Checked)
                                        {
                                            itemProgBothDetails = RngResultlist.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "RefCnt");
                                        }
                                        //List<RNGGAEntity> Goalsubdetails = GoalDetailsEntity.FindAll(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim());
                                        bool boolProgservicesubdetails = true;
                                        int intProgBothcountdata = 0;
                                        foreach (SRCsb14GroupEntity itemProg in rngsubProgdetailsdata)
                                        {
                                            if (rdoOutcomesselect.Checked)
                                            {

                                                boolProgservicesubdetails = false;
                                                if (rdoperiodBoth.Checked)
                                                {
                                                    if (itemBothDetails[intBothcountdata].CalCost.ToString().Trim() != string.Empty && itemBothDetails[intBothcountdata].CalCost != "0")
                                                    {
                                                        boolservicesubdetails = true;
                                                    }
                                                    else if (itemProgBothDetails[intProgBothcountdata].unit_cnt.ToString().Trim() != string.Empty && itemProgBothDetails[intProgBothcountdata].unit_cnt != "0")
                                                        boolProgservicesubdetails = true;
                                                }
                                                else
                                                {
                                                    if (item.CalCost.ToString().Trim() != string.Empty && item.CalCost != "0")
                                                    { boolservicesubdetails = true; }
                                                    else if (itemProg.unit_cnt.ToString().Trim() != string.Empty && itemProg.unit_cnt != "0")
                                                        boolProgservicesubdetails = true;
                                                }
                                            }
                                            //rngcountdata = RngResultlist.Find(u => u.GrpCode.Trim() == tblEnt.GrpCode.Trim() && u.Code.Trim() == tblEnt.Code.Trim() && u.TblCode.Trim() == tblEnt.TblCode.Trim() && u.Row_Type.ToString() == "TblDetails" && u.Count_type.ToString().Trim() == "PrdCnt" && u.GrpDesc.ToString().Trim() == item.Desc.ToString().Trim());
                                            //intcount1 = intcount2 = intcout3 = "0";
                                            //if (rngcountdata != null)
                                            //{
                                            //    intcount1 = rngcountdata.unit_cnt.ToString();
                                            //    intcout3 = rngcountdata.Hrd1.ToString();
                                            //}
                                            if (boolProgservicesubdetails)
                                            {
                                                excelrow = sheet.Table.Rows.Add();
                                                cell = excelrow.Cells.Add("         " + itemProg.GrpDesc.ToString(), DataType.String, "Normal");

                                                string xx = itemProg.ExAchev.ToString() == "0" ? "" : itemProg.ExAchev.ToString();

                                                cell = excelrow.Cells.Add("  " + xx, DataType.String, "Normal");

                                                if (rdoperiodBoth.Checked)
                                                {
                                                    string x = "";
                                                    SRCsb14GroupEntity itemBoth = itemProgBothDetails[intProgBothcountdata];
                                                    if (itemBoth != null)
                                                    {
                                                        if (!string.IsNullOrEmpty(itemBoth.CalCost.Trim()))
                                                        {
                                                            if (decimal.Parse(itemBoth.CalCost) > 0)
                                                                x = itemBoth.CalCost.ToString() == "0" ? "" : itemBoth.CalCost.ToString();
                                                            else
                                                                x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                        }
                                                        else
                                                            x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                    }
                                                    //x = itemBoth.unit_cnt.ToString() == "0" ? "" : itemBoth.unit_cnt.ToString();
                                                    string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                                  //if (itemBoth != null)
                                                                  //{
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                                    {
                                                        if (decimal.Parse(item.CalCost) > 0)
                                                            Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                        else
                                                            Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    }
                                                    else
                                                        Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    cell = excelrow.Cells.Add(x, DataType.String, "Normal");
                                                    cell = excelrow.Cells.Add(Y, DataType.String, "Normal");
                                                }
                                                else if (rdoperiodCumulative.Checked)
                                                {
                                                    string Y = "";//item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                                  //if (itemBoth != null)
                                                                  //{
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                                    {
                                                        if(decimal.Parse(item.CalCost)>0)
                                                            Y = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                        else
                                                            Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    }
                                                    else
                                                        Y = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    cell = excelrow.Cells.Add(Y, DataType.String, "Normal");
                                                    cell.MergeAcross = 1;
                                                }
                                                else
                                                {
                                                    string x = "";// item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    if (!string.IsNullOrEmpty(item.CalCost.Trim()))
                                                    {
                                                        if (decimal.Parse(item.CalCost) > 0)
                                                            x = item.CalCost.ToString() == "0" ? "" : item.CalCost.ToString();
                                                        else
                                                            x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    }
                                                    else
                                                        x = item.unit_cnt.ToString() == "0" ? "" : item.unit_cnt.ToString();
                                                    cell = excelrow.Cells.Add("  " + x, DataType.String, "Normal");
                                                    cell.MergeAcross = 1;

                                                }
                                            }
                                            intProgBothcountdata = intProgBothcountdata + 1;
                                        }

                                    }
                                }

                            }

                            
                        }
                    }
                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("", DataType.String, "Normal");
                    cell.MergeAcross = 3;
                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("", DataType.String, "Normal");
                    cell.MergeAcross = 3;
                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add("", DataType.String, "Normal");
                    cell.MergeAcross = 3;

                }

                FileStream stream1 = new FileStream(PdfName, FileMode.Create);
                book.Save(stream1);
                stream1.Close();

            }
            catch (Exception ex) { }
            //document.Add(table);                



        }


        #region properties

        public string Scr_Code { get; set; }

        public string Report_Name { get; set; }

        public string Report_To_Process { get; set; }

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


        public DataTable Result_Table { get; set; }

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
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
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
            //HierarchyGrid.Rows.Clear();
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

        private void rbAllPrograms_Click(object sender, EventArgs e)
        {
            if (rbAllPrograms.Checked == true)
            {
                HierarchyGrid.Rows.Clear();
                SelectedHierarchies.Clear();
            }
        }

        private void RNGS0014_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(PrivilegeEntity.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");

            Application.Navigate(CommonFunctions.CreateZenHelps(PrivilegeEntity.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void Rb_OBO_Mem_Click(object sender, EventArgs e)
        {
            if (rdoperiodBoth.Checked)
            {
                chkbUndupTable.Visible = true;

                //chkbUndupTable.Checked = false;
                //chkbUndupTable.Visible = false;
            }
            else if (rdoperiod.Checked)
                chkbUndupTable.Visible = true; 
            //**spacer6.Visible = true; rdbSummaryDet.Visible = false; rbo_ProgramWise.Visible = false; rdbSummaryDet.Checked = true; 12/29/2023
        }

        private void Rb_SNP_Mem_Click(object sender, EventArgs e)
        {
            chkbUndupTable.Checked = false; chkbUndupTable.Visible = false;
            //**rdbSummaryDet.Visible = true; rbo_ProgramWise.Visible = true; chkbUndupTable.Visible = false; spacer6.Visible = false; rdbSummaryDet.Checked = true;
        }

        private void OnExcel_FamilyForm_Report(DataTable dtFam)
        {
            string PdfName = "Pdf File";
            PdfName = "RNGS0014_Family_Details";
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

                //xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 9, "R1");

                //xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 10, "R2");

                //xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 11, "R3");

                //xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 12, "R4");

                //xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                //xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
                //xlWorkSheet.WriteCell(excelcolumn, 13, "R5");

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

                                strGroupCode = dr["SortUnDup_Table"].ToString().Trim();
                                SRCsb14GroupEntity rngcodedescdata = RngCodelist.Find(u => u.GrpCode.Trim() == strGroupCode && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
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
                                if (strGroupCode != dr["SortUnDup_Table"].ToString().Trim())
                                {
                                    strGroupCode = dr["SortUnDup_Table"].ToString().Trim();
                                    SRCsb14GroupEntity rngcodedescdata = RngCodelist.Find(u => u.GrpCode.Trim() == strGroupCode && u.TblCode.Trim() == string.Empty && u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbRngCode.SelectedItem).Value.ToString());
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
                            // xlWorkSheet.WriteCell(excelcolumn, 9, dr["R1"].ToString());

                            //xlWorkSheet.WriteCell(excelcolumn, 10, dr["R2"].ToString());

                            //xlWorkSheet.WriteCell(excelcolumn, 11, dr["R3"].ToString());
                            ////xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
                            ////xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Right;
                            //xlWorkSheet.WriteCell(excelcolumn, 12, dr["R4"].ToString());

                            //xlWorkSheet.WriteCell(excelcolumn, 13, dr["R5"].ToString());


                        }
                    }
                }


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                xlWorkSheet.Save(stream);
                stream.Close();
                //FileStream stream = new FileStream(PdfName, FileMode.Create);

                //book.Save(stream);
                //stream.Close();

            }
            catch (Exception ex) { }

            //Generate(PdfName);
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }


        #endregion

        private void rdoperiodBoth_Click(object sender, EventArgs e)
        {
            rdoreportforselection();
        }

        private void rdoreportforselection()
        {
            if (rdoperiod.Checked == true)
            {
                Ref_From_Date.Enabled = false;
                Ref_To_Date.Enabled = false;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                chkbUndupTable.Enabled = true;
                if(Rb_OBO_Mem.Checked)
                    chkbUndupTable.Visible = true;
                chkRepControl.Visible = false; chkRepControl.Checked = false;

                panel18.Visible = false;
                this.Size = new Size(this.Width, 685);
                this.pnlParams.Size = new Size(Width, 535);
            }
            else if (rdoperiodCumulative.Checked == true)
            {
                Ref_From_Date.Enabled = true;
                Ref_To_Date.Enabled = true;
                Rep_From_Date.Enabled = false;
                Rep_To_Date.Enabled = false;

                chkbUndupTable.Enabled = true;
            }
            else if (rdoperiodBoth.Checked == true)
            {
                Ref_From_Date.Enabled = true;
                Ref_To_Date.Enabled = true;
                Rep_From_Date.Enabled = true;
                Rep_To_Date.Enabled = true;

                if (Rb_OBO_Mem.Checked)
                    chkbUndupTable.Visible = true;

                //chkbUndupTable.Enabled = false;
                //chkbUndupTable.Checked = false;
                //chkbUndupTable.Visible = false;

                chkRepControl.Visible = true;

                panel18.Visible = true;
                this.Size = new Size(this.Width, 710);
                this.pnlParams.Size = new Size(Width, 570);
            }
        }
        #endregion

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
                    ListGroupCode = form.SelectedSRGroupCodeEntity;
                }
                else if (form.FormType == "FUND")
                {
                    Sel_Funding_List = form.Get_Sel_Fund_List;
                }


            }

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

        List<SPCommonEntity> Fund_Mast_List = new List<SPCommonEntity>();
        private void Fill_Fund_Mast_List()
        {
            Fund_Mast_List = _model.SPAdminData.Get_AgyRecs("Funding");
            //if (Entity_List != null && Entity_List.Count > 0)
            //{
            //    FundingList.ForEach(item => item.Active = (Entity_List.Exists(u => u.Code.Equals(item.Code))) ? "true" : "false");
            //}
        }

        #region Alert Codes - Added by Vikash as per "ROMA Services Report - Enhancements.docx" on 01/17/2024

        private void rdbSelAlertCds_Click(object sender, EventArgs e)
        {
            //string Year = string.Empty;
            //if (CmbYear.Visible)
            //{
            //    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Value.ToString();
            //    CaseMST = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProg, Year, string.Empty);
            //}
            //else
            //{
            //    CaseMST = _model.CaseMstData.GetCaseMST(strAgency, strDept, strProg, string.Empty, string.Empty);
            //}
            
            AlertCodeForm objform = new AlertCodeForm(BaseForm, PrivilegeEntity, SelectedAlertCodes/*, CaseMST*/);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            objform.ShowDialog();
        }
        private void rdbAllAlertCds_Click(object sender, EventArgs e)
        {
            SelectedAlertCodes = string.Empty;
            _errorProvider.SetError(rdbSelAlertCds, null);
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

        List<string> AlertcodesList = new List<string>();

        private string Get_Sel_AlertCodes(string alertCodes)
        {
            SelectedAlertCodes = alertCodes;

            return SelectedAlertCodes;
        }
        #endregion
    }
}