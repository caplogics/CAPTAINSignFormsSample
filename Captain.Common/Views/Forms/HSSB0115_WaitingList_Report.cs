#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using System.Xml;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using XLSExportFile;
using DevExpress.Pdf.Xmp;
using Captain.Common.Interfaces;



#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0115_WaitingList_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public HSSB0115_WaitingList_Report(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

            ShortName = string.Empty;
           
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " + */Privileges.PrivilegeName.Trim();
            //if (ShortName == "OFO" )
            //{
            //    //rbFundSel.Text = "Primary"; rbDayCare.Visible = true;
            //    lblIncludeClients.Visible = true; panel5.Visible = true;
            //}
            //else { rbFundSel.Text = "Selected Funding"; rbDayCare.Visible = false; }
            this.Size = new Size(850, 425);
            FillRankCatgCombo();

            if (rbNoLbl.Checked)
            {
                rbChild.Enabled = false;
                rbParent.Enabled = false;
                lblPrintlbl.Enabled = false;
                pnlPrint.Visible = false;
            }
            else if (rbMailLbl.Checked)
            {
                pnlPrint.Visible = true;
                lblPrintlbl.Enabled = true;
                rbChild.Enabled = true;
                rbParent.Enabled = true;
            }
        }
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }
        public string ShortName { get; set; }
        
        public List<RankCatgEntity> Ranklist { get; set; }

        public string propReportPath { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<ChldTrckEntity> propTasks { get; set; }
        public List<ChldTrckEntity> TrackList { get; set; }
        public List<ChldMediEntity> propMediTasks { get; set; }

        public List<HSSB2115Entity> SelRecords { get; set; }
        List<HSSB2115Entity> SelRecords_Labels = new List<HSSB2115Entity>();
        public List<RankCatgEntity> Sel_Ranklist { get; set; }
        public List<CommonEntity> lookUpCategrcalEligiblity { get; set; }

        #endregion


        private void FillRankCatgCombo()
        {
            cmbRankCtg.Items.Clear();
            Ranklist = _model.SPAdminData.Browse_RankCtg();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            if (Ranklist.Count > 0)
            {
                cmbRankCtg.Items.Add(new Captain.Common.Utilities.ListItem("All Rankings", "0"));
                foreach (RankCatgEntity Entity in Ranklist)
                {
                    if (Entity.Agency.Trim() == Agency && Entity.HeadStrt != "N" && string.IsNullOrEmpty(Entity.SubCode.Trim()))
                        cmbRankCtg.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc.Trim(), Entity.Code.Trim()));
                }
            }

            cmbRankCtg.Items.AddRange(listItem.ToArray());
            cmbRankCtg.SelectedIndex = 0;

            cmbCategorical.Items.Clear();
            lookUpCategrcalEligiblity = _model.lookupDataAccess.GetCategrcalEligiblity();
            foreach (CommonEntity agyEntity in lookUpCategrcalEligiblity)
            {
                cmbCategorical.Items.Add(new Captain.Common.Utilities.ListItem(agyEntity.Desc, agyEntity.Code));
            }
            cmbCategorical.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "0"));
            cmbCategorical.SelectedIndex = 0;
        }


        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }
        string Program_Year;

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
            {
                FillYearCombo(Agy, Dept, Prog, Year);
                rdoMultipleSites.Enabled = true;
            }
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(765, 25);
                rdoMultipleSites.Enabled = false;
            }
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
                System.Data.DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear)) && DepYear != null && DepYear != "    ")
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

            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;

            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(685, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(765, 25);
        }

        private void rbFundSel_Click(object sender, EventArgs e)
        {
            //if (ShortName == "OFO")
            //{
                //rbFundSel.Text = "Primary"; rbDayCare.Visible = true;
            lblIncludeClients.Visible = true; pnlIncClients.Visible = true;pnlIncClients.Visible = true;
            this.Size = new Size(850, 425);
            _errorProvider.SetError(rbDayCare, null);
            //SelFundingList.Clear();
            //}
            if (rbFundSel.Checked == true && rbFundSel.Text == "Selected Funding")
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
            else if (rbFundSel.Checked == true && rbFundSel.Text == "Primary")
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges, "Headstart");
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
            
        }

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rdoMultipleSites_Click(object sender, EventArgs e)
        {
            if (rdoMultipleSites.Checked == true)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "SITE", Sel_REFS_List, "Report", Agency.Trim(), Depart.Trim(), Program.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
                //if (Sel_REFS_List.Count > 0)
                //{
                //    CommonFunctions.SetComboBoxValue(cmbEnrlStatus, "E");
                //    cmbEnrlStatus.Enabled = false;
                //}
            }
        }


        private bool ValidateForm()
        {
            bool isValid = true;
            
            if (rdoMultipleSites.Checked == true)
            {
                if (Sel_REFS_List.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleSites, "Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleSites, null);
                }
            }
            else
                _errorProvider.SetError(rdoMultipleSites, null);

            if (rbFundSel.Checked == true )
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rbFundSel, "Select at least One Fund");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbFundSel, null);
                }
            }
            else
                _errorProvider.SetError(rbFundSel, null);

            if (rbDayCare.Checked)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rbDayCare, "Select at least One Fund");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rbDayCare, null);
                }
            }
            else
                _errorProvider.SetError(rbDayCare, null);

            return (isValid);
        }



        string strsiteRoomNames = string.Empty; string strFundingCodes = string.Empty; 
        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (rdoMultipleSites.Checked == true)
                {
                    strsiteRoomNames = string.Empty;
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                        strsiteRoomNames += siteRoom.SiteNUMBER;// +siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                    }
                }
                else
                {
                    strsiteRoomNames = "A";
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity(); 
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == Program)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                                {
                                    hierarchyEntity = null;
                                    strsiteRoomNames = "A";
                                }
                            }
                        }

                        if (hierarchyEntity != null) { if (!string.IsNullOrEmpty(hierarchyEntity.Sites.Trim())) strsiteRoomNames = hierarchyEntity.Sites; }
                    }
                    else
                        strsiteRoomNames = "A";
                }


                //string strFundingCodes = string.Empty;
                if (rbFundSel.Checked == true || rbDayCare.Checked)
                {
                    strFundingCodes = string.Empty;
                    //if (rbFundSel.Text == "Headstart")
                    //    strFundingCodes = "H";
                    //else
                    //{
                        foreach (CommonEntity FundingCode in SelFundingList)
                        {
                            if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                            strFundingCodes += FundingCode.Code;
                        }
                    //}
                }
                else if(rbFundAll.Checked)
                {
                    strFundingCodes = "A";
                }
                else if (rbDayCare.Checked)
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }

                string Fund_Sw = "A";
                if (rbFundAll.Checked) Fund_Sw = "A";
                else if (rbDayCare.Checked) Fund_Sw = "S";
                else if (rbFundSel.Text == "Primary") Fund_Sw = "P"; else Fund_Sw = "S";

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string BaseAge = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                string Active_stat = string.Empty; string Clients = string.Empty;
                if (rdoTodayDate.Checked == true) BaseAge = "T"; else BaseAge = "K";
                if (rbDayCare.Checked==false)
                {
                    if (rbBoth.Checked == true) Clients = "D";
                    else if (rbIntakeCmpltd.Checked == true) Clients = "E";
                    else if (rbWaitlist.Checked == true) Clients = "W";
                }

                SelRecords = new List<HSSB2115Entity>();
                Sel_Ranklist = Ranklist.FindAll(u => u.SubCode.Equals("") && u.Agency.Equals(Agency));
                DataSet ds = Captain.DatabaseLayer.ChldTrckDB.GetHSSB0115_Report(Agency, Depart, Program, Program_Year, BaseAge, strsiteRoomNames, strFundingCodes, txtFrom.Text.Trim(), txtTo.Text.Trim(), Income_Stat, ((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Value.ToString().Trim(), Clients, Fund_Sw);
                System.Data.DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    System.Data.DataTable dtMod = dt.Clone();
                    dtMod.Columns["TOT_RANK"].DataType = Type.GetType("System.Int32");

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtMod.ImportRow(dr);
                    }

                    DataView dv = new DataView(dtMod);
                    if (rdoRank.Checked == true)
                        dv.Sort = "TOT_RANK desc";
                    else if (rdoSiteName.Checked == true && Clients!="E")
                        dv.Sort = "ENRL_SITE,Lname";
                    else if (rdoSiteName.Checked == true)
                        dv.Sort = "MST_SITE,Lname";
                    dtMod = dv.ToTable();
                    //if (rdoRank.Checked == true)
                    //    dt.DefaultView.Sort = "TOT_RANK desc";
                    //else
                    //    dt.DefaultView.Sort = "MST_SITE,TOT_RANK";

                    foreach (DataRow row in dtMod.Rows)
                    {
                        SelRecords.Add(new HSSB2115Entity(row));
                    }

                }
                if (chkbExcel.Checked == true)
                {
                    //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "XLS");
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "XLSX");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    //pdfListForm.FormClosed += new FormClosedEventHandler(ExcelFormats);
                    pdfListForm.FormClosed += new FormClosedEventHandler(ExcelFormats_DevExpress);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
                else
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
            }
        }

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdoMultipleSites, null);
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
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
                System.Data.DataTable RepCntl_Table = new System.Data.DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string RankCatg = ((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Value.ToString();
            string catg = ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString();

            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strSeq = rdoSiteName.Checked == true ? "S" : "R";
            string strStatus = string.Empty;

            string strLabel = rbNoLbl.Checked ? "N" : "M";

            string strPrint = string.Empty;
            if (rbMailLbl.Checked)
            {
                strPrint = rbChild.Checked ? "C" : "P";
            }

            string Excel = "N";
            if (chkbExcel.Checked)
                Excel = "Y";

            if (rbIncAll.Checked == true)
                strStatus = "IA";
            else if (rbOverInc.Checked == true)
                strStatus = "IO";
            else if (rdoIncCerti.Checked == true)
                strStatus = "IC";
            else if (rdoIncElig.Checked == true)
                strStatus = "IE";
            else if (rb101.Checked == true)
                strStatus = "101";
            else if (rbCategorical.Checked == true)
                strStatus = "CA";

            string IncludeClients = string.Empty;
            if (rbBoth.Checked) IncludeClients = "D"; else if (rbIntakeCmpltd.Checked) IncludeClients = "E"; else if (rbWaitlist.Checked) IncludeClients = "W";

            string strBaseAge = rdoTodayDate.Checked ? "T" : "K";
            string strFundSource = string.Empty;
            if (rbFundAll.Checked) strFundSource = "A"; else if (rbFundSel.Checked) strFundSource = "P"; else if (rbDayCare.Checked) strFundSource = "S";
            string strsiteRoomNames = string.Empty;
            if (rdoMultipleSites.Checked == true)
            {
                foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                {
                    if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                    strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                }
            }

            
            string strFundingCodes = string.Empty;
            if (rbFundSel.Checked == true || rbDayCare.Checked)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }

            string Casenote = chkbCasenotes.Checked ? "Y" : "N";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" Sequnce = \"" + strSeq + "\" Category = \"" + RankCatg + 
                            "\" AgeFrom = \"" + txtFrom.Text + "\" AgeTo = \"" + txtTo.Text + "\" BaseAge = \"" + strBaseAge + "\" FundedSource = \"" + strFundSource  +"\" Categorical = \"" + catg + "\" Print = \"" + strPrint +
                            "\" SiteNames = \"" + strsiteRoomNames + "\" Status = \"" + strStatus + "\" FundingCode = \"" + strFundingCodes + "\" CaseNotes = \"" + Casenote +
                            "\" Clients = \"" + IncludeClients + "\" EXCEL = \"" + Excel  +"\" Label = \"" + strLabel + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(System.Data.DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbRankCtg, dr["Category"].ToString());

                if (dr["Sequnce"].ToString().Trim() == "S")
                    rdoSiteName.Checked = true;
                else 
                    rdoRank.Checked = true;

                if (dr["CaseNotes"].ToString().Trim() == "Y") chkbCasenotes.Checked = true; else chkbCasenotes.Checked = false;

                if (dr["Site"].ToString() == "A")
                    rdoAllSite.Checked = true;
                else
                {
                    rdoMultipleSites.Checked = true;
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                    Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                    propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() == "0000");
                    Sel_REFS_List.Clear();
                    string strSites = dr["SiteNames"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM  == sitesRooms.GetValue(i).ToString());
                            Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }
                Sel_REFS_List = Sel_REFS_List;

                CommonFunctions.SetComboBoxValue(cmbCategorical, dr["Categorical"].ToString());

                if (dr["Label"].ToString().Trim() == "N")
                {
                    rbNoLbl.Checked = true;
                }
                else
                {
                    rbMailLbl.Checked = true;
                }
                rbMailLbl_Click(this, new EventArgs { });

                if (dr["Print"].ToString().Trim() == "C")
                    rbChild.Checked = true;
                else
                    rbParent.Checked = true;

                if (dr["EXCEL"].ToString() == "Y")
                    chkbExcel.Checked = true;
                else
                    chkbExcel.Checked = false;

                if (dr["Status"].ToString().Trim() == "IA")
                    rbIncAll.Checked = true;
                else if (dr["Status"].ToString().Trim() == "IO")
                    rbOverInc.Checked = true;

                else if (dr["Status"].ToString().Trim() == "IC")
                    rdoIncCerti.Checked = true;
                else if (dr["Status"].ToString().Trim() == "IE")
                    rdoIncElig.Checked = true;
                else if (dr["Status"].ToString().Trim() == "101")
                    rb101.Checked = true;
                else if (dr["Status"].ToString().Trim() == "CA")
                    rbCategorical.Checked = true;


                //if (dr["FundedDays"].ToString() == "Y")
                //    rdoAllFundYes.Checked = true;
                //else
                //    rdoAllFundNo.Checked = true;
                if (dr["FundedSource"].ToString() == "A")
                    rbFundAll.Checked = true;
                else
                {
                    if (dr["FundedSource"].ToString() == "P")
                        rbFundSel.Checked = true;
                    else if (dr["FundedSource"].ToString() == "S")
                        rbDayCare.Checked = true;

                    SelFundingList.Clear();
                    string strFunds = dr["FundingCode"].ToString();
                    List<string> siteList = new List<string>();
                    if (strFunds != null)
                    {
                        string[] FundCodes = strFunds.Split(',');
                        for (int i = 0; i < FundCodes.Length; i++)
                        {
                            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                //List<ChldTrckEntity> chldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);

                

                txtFrom.Text = dr["AgeFrom"].ToString();
                txtTo.Text = dr["AgeTo"].ToString();

                if (dr["BaseAge"].ToString().Trim() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["FundedSource"].ToString() != "S")
                {
                    pnlIncClients.Visible = true; this.Size = new Size(850, 425); lblIncludeClients.Visible = true;
                    if (dr["Clients"].ToString().Trim() == "D") rbBoth.Checked = true;
                    else if (dr["Clients"].ToString().Trim() == "E") rbIntakeCmpltd.Checked = true;
                    else if (dr["Clients"].ToString().Trim() == "W") rbWaitlist.Checked = true;
                }
                else
                {
                    pnlIncClients.Visible = false; this.Size = new Size(850, 395); lblIncludeClients.Visible = false;
                }
                

            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
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
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    propReportPath = _model.lookupDataAccess.GetReportPath();
                }
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

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

                //Document document = new Document();
                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;

                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                iTextSharp.text.Font TableFontRed = new iTextSharp.text.Font(bf_times, 8, 1, BaseColor.RED);

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
                iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));

                #region Cell Color Define
                BaseColor DarkBlue = new BaseColor(26, 71, 119);
                BaseColor SecondBlue = new BaseColor(184, 217, 255);
                BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);
                #endregion

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                CaseSiteEntity SearchEntity = new CaseSiteEntity(true);
                SearchEntity.SiteROOM = "0000"; SearchEntity.SiteAGENCY = Agency; SearchEntity.SiteDEPT = Depart; SearchEntity.SitePROG = Program;
                SearchEntity.SiteYEAR = Year;
                propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(SearchEntity, "Browse");
                //int count = 0;
                try
                {
                    //PrintHeaderPage(document, writer);
                    PrintHeaderPage_New_Format(document, writer);
                    document.NewPage();

                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 550f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 12f, 20f, 50f, 35f, 65f, 55f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.HeaderRows = 2;

                    PdfPTable Counttable = new PdfPTable(6);
                    Counttable.TotalWidth = 500f;
                    Counttable.WidthPercentage = 100;
                    Counttable.LockedWidth = true;
                    float[] Counttablewidths = new float[] { 20f, 20f, 20f, 20f, 20f, 20f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    Counttable.SetWidths(Counttablewidths);
                    Counttable.HorizontalAlignment = Element.ALIGN_LEFT;

                    if (SelRecords.Count > 0)
                    {
                        SelRecords = SelRecords.FindAll(u => Convert.ToInt32(u.Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Age) <= Convert.ToInt32(txtTo.Text));

                        if (((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Trim() != "0")
                            SelRecords = SelRecords.FindAll(u => u.MSTCatElig.Equals(((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Trim()));

                        CaseSiteEntity Sel_Site = new CaseSiteEntity();
                        if (SelRecords.Count > 0)
                        {

                            if (rdoRank.Checked == true)
                            {
                                PdfPCell Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Colspan = 6;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                table.AddCell(Cell);

                                PrintMainHeader(table);
                            }
                            int count = 0;int EnrlCnt = 0;
                            bool first = true; string Priv_Site = string.Empty; string Priv_Desc = string.Empty; string Site_Desc = string.Empty;
                            foreach (HSSB2115Entity Entity in SelRecords)
                            {
                                if(count>=13)
                                {
                                    if(table.Rows.Count>0)
                                    {
                                        count = 0;EnrlCnt = 0;

                                        document.Add(table);
                                        table.DeleteBodyRows();
                                        document.NewPage();


                                        string SiteName = string.Empty;
                                        if (rbWaitlist.Checked || rbBoth.Checked)
                                            SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                        else
                                            SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

                                        PdfPCell Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                        Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Cell.Colspan = 6;
                                        Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Cell.BackgroundColor = DarkBlue;
                                        table.AddCell(Cell);

                                        if (rdoSiteName.Checked == true)
                                        {

                                            PdfPCell Header2 = new PdfPCell(new Phrase(SiteName, HeaderTextfnt));
                                            Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header2.Colspan = 6;
                                            //Header2.FixedHeight = 15f;
                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Header2.BackgroundColor = DarkBlue;
                                            table.AddCell(Header2);
                                        }

                                        PrintMainHeader(table);


                                    }
                                }


                                if (rdoSiteName.Checked == true)
                                {
                                    if (rbIntakeCmpltd.Checked == false ? Entity.EnrlSite.Trim() != Priv_Site : Entity.Site.Trim() != Priv_Site)

                                    //if ((rbWaitlist.Checked && Entity.EnrlSite.Trim() != Priv_Site) || (Entity.Site.Trim() != Priv_Site))
                                    {
                                        if (propCaseSiteEntity.Count > 0)
                                        {
                                            if (rbWaitlist.Checked || rbBoth.Checked)
                                                Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(Entity.EnrlSite.Trim()) && u.SiteYEAR.Equals(Year));
                                            else
                                                Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(Entity.Site.Trim()) && u.SiteYEAR.Equals(Year));

                                            if (Sel_Site != null)
                                                Site_Desc = Sel_Site.SiteNAME.Trim();
                                            else Site_Desc = "";
                                        }

                                        if (!first)
                                        {
                                            if (table.Rows.Count > 0)
                                            {
                                                count = 0; EnrlCnt = 0;

                                                document.Add(table);
                                                table.DeleteBodyRows();
                                                document.NewPage();
                                            }

                                            PrintCountHeader(Counttable, Priv_Site, Priv_Desc);


                                            PrintCountTable(Counttable, Priv_Site);

                                            if (Counttable.Rows.Count > 0)
                                            {
                                                document.Add(Counttable);
                                                Counttable.DeleteBodyRows();
                                                document.NewPage();

                                                string SiteName = string.Empty;
                                                if (rbWaitlist.Checked || rbBoth.Checked)
                                                    SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                                else
                                                    SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

                                                PdfPCell Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Cell.Colspan = 6;
                                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                Cell.BackgroundColor = DarkBlue;
                                                table.AddCell(Cell);

                                                PdfPCell Header2 = new PdfPCell(new Phrase(SiteName, HeaderTextfnt));
                                                Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header2.Colspan = 6;
                                                //Header2.FixedHeight = 15f;
                                                Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                Header2.BackgroundColor= DarkBlue;
                                                table.AddCell(Header2);

                                                
                                                PrintMainHeader(table);
                                            }
                                        }
                                        else
                                        {
                                            string SiteName = string.Empty;
                                            if (rbWaitlist.Checked || rbBoth.Checked)
                                                SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                            else
                                                SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

                                            PdfPCell Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Cell.Colspan = 6;
                                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Cell.BackgroundColor = DarkBlue;
                                            table.AddCell(Cell);

                                            PdfPCell Header = new PdfPCell(new Phrase(SiteName, HeaderTextfnt));
                                            Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header.Colspan = 6;
                                            //Header.FixedHeight = 15f;
                                            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Header.BackgroundColor = DarkBlue;
                                            table.AddCell(Header);

                                            PrintMainHeader(table);
                                            first = false;
                                        }
                                    }
                                    else
                                    {
                                        if ((first && Entity.EnrlSite.Trim() == Priv_Site && (rbWaitlist.Checked || rbBoth.Checked)) || (first && Entity.Site.Trim() == Priv_Site))
                                        {
                                            string SiteName = string.Empty;
                                            if (rbWaitlist.Checked || rbBoth.Checked)
                                                SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                            else
                                                SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

                                            PdfPCell Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Cell.Colspan = 6;
                                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Cell.BackgroundColor = DarkBlue;
                                            table.AddCell(Cell);

                                            PdfPCell Header = new PdfPCell(new Phrase(SiteName, HeaderTextfnt));
                                            Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header.Colspan = 6;
                                            //Header.FixedHeight = 15f;
                                            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Header.BackgroundColor = DarkBlue;  
                                            table.AddCell(Header);

                                            
                                            PrintMainHeader(table);
                                            first = false;
                                        }
                                    }
                                }

                                if (Entity.AppNo.Trim() == "00162300")
                                {
                                    //MessageBox.Show
                                }

                                SelRecords_Labels.Add(new HSSB2115Entity(Entity));

                                PdfPCell Rnk = new PdfPCell(new Phrase(Entity.Rank.Trim(), TableFont));
                                Rnk.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //Rnk.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                Rnk.Border = 0;
                                Rnk.BorderWidthLeft = 0.5f;Rnk.BorderColorLeft = BaseColor.BLACK;
                                Rnk.BorderWidthTop = 0.5f; Rnk.BorderColorTop = BaseColor.BLACK;
                                table.AddCell(Rnk);

                                PdfPCell App = new PdfPCell(new Phrase(" " + Entity.AppNo.Trim(), TableFont));
                                App.HorizontalAlignment = Element.ALIGN_LEFT;
                                //App.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                App.Border = 0;
                                App.BorderWidthLeft = 0.5f; App.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                App.BorderWidthTop = 0.5f; App.BorderColorTop = BaseColor.BLACK;
                                table.AddCell(App);

                                if (Entity.Disable.Trim() == "Y")
                                {
                                    var fontred = FontFactory.GetFont("Times", 9, BaseColor.RED.Darker());
                                    //var disabtext = new Chunk("*", fontred);
                                    //var Name = new Chunk(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont);
                                    var phrase = new Phrase();
                                    phrase.Add(new Chunk("*", fontred));
                                    phrase.Add(new Chunk(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont));

                                    PdfPCell AppName = new PdfPCell(phrase);
                                    AppName.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppName.Border = 0;
                                    AppName.BorderWidthLeft = 0.5f; AppName.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppName.BorderWidthTop = 0.5f; AppName.BorderColorTop = BaseColor.BLACK;
                                    table.AddCell(AppName);
                                }
                                else
                                {
                                    PdfPCell AppName = new PdfPCell(new Phrase(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont));
                                    AppName.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppName.Border = 0;
                                    AppName.BorderWidthLeft = 0.5f; AppName.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppName.BorderWidthTop = 0.5f; AppName.BorderColorTop = BaseColor.BLACK;
                                    table.AddCell(AppName);
                                }

                                PdfPCell AppAge = new PdfPCell(new Phrase("Age  " + Entity.Age.Trim() + " yrs  " + Entity.Months.Trim() + " mos", TableFont));
                                AppAge.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppAge.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                AppAge.Border = 0;
                                AppAge.BorderWidthLeft = 0.5f; AppAge.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppAge.BorderWidthTop = 0.5f; AppAge.BorderColorTop = BaseColor.BLACK;
                                table.AddCell(AppAge);

                                string Apt = string.Empty; string Floor = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.Apt.Trim()))
                                    Apt = "  Apt  " + Entity.Apt.Trim();
                                if (!string.IsNullOrEmpty(Entity.Flr.Trim()))
                                    Floor = "  Flr  " + Entity.Flr.Trim();
                                PdfPCell AppStreet = new PdfPCell(new Phrase(Entity.Hno.Trim() + "   " + Entity.Street.Trim() + " " + Entity.Suffix.Trim() + Apt + Floor, TableFont));
                                AppStreet.HorizontalAlignment = Element.ALIGN_LEFT;
                                AppStreet.Border = 0;
                                AppStreet.BorderWidthLeft = 0.5f; AppStreet.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppStreet.BorderWidthTop = 0.5f; AppStreet.BorderColorTop = BaseColor.BLACK;
                                //AppStreet.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AppStreet);

                                PdfPCell AppGuardian = new PdfPCell(new Phrase(Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim(), TableFont));
                                AppGuardian.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppGuardian.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                AppGuardian.Border = 0;
                                AppGuardian.BorderWidthLeft = 0.5f; AppGuardian.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppGuardian.BorderWidthTop = 0.5f; AppGuardian.BorderColorTop = BaseColor.BLACK;
                                AppGuardian.BorderWidthRight = 0.5f; AppGuardian.BorderColorRight = BaseColor.BLACK;
                                table.AddCell(AppGuardian);

                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Space.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                Space.Border= 0;
                                Space.BorderWidthLeft = 0.5f;Space.BorderColorLeft = BaseColor.BLACK;
                                Space.BorderWidthTop = 0.5f; Space.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(Space);

                                string Sex = string.Empty; string Class = string.Empty;
                                if (Entity.Sex.Trim() == "M") Sex = "MALE"; else Sex = "FEMALE";
                                if (Entity.ClassPrefer.Trim() == "A") Class = "AM";
                                else if (Entity.ClassPrefer.Trim() == "P") Class = "PM";
                                else if (Entity.Classfication.Trim() == "E") Class = "EXTD"; else if (Entity.ClassPrefer.Trim() == "F") Class = "FULL";

                                string CatgDesc = string.Empty;
                                if (lookUpCategrcalEligiblity.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(Entity.MSTCatElig.Trim()))
                                    {
                                        foreach (CommonEntity looks in lookUpCategrcalEligiblity)
                                        {
                                            if (looks.Code == Entity.MSTCatElig.Trim())
                                            {
                                                CatgDesc = "       Categorical: " + looks.Desc.Trim();
                                                break;
                                            }
                                        }
                                    }
                                    //CatgDesc = "       Categorical: " + lookUpCategrcalEligiblity.Find(u => u.Code.Equals(Entity.MSTCatElig.Trim())).Desc.Trim();
                                }


                                if (Entity.ClassPrefer.Trim() == "N")
                                {
                                    PdfPCell AppClass = new PdfPCell(new Phrase("PREFER" + "       " + " SLOT    " + Sex.Trim() + CatgDesc, TableFont));
                                    AppClass.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AppClass.Colspan = 3;
                                    //AppClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppClass.Border = 0;
                                    AppClass.BorderWidthLeft = 0.5f; AppClass.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppClass.BorderWidthTop = 0.5f; AppClass.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppClass);
                                }
                                else
                                {
                                    PdfPCell AppClass = new PdfPCell(new Phrase("PREFER    " + Class + " SLOT  " + Sex.Trim() + CatgDesc, TableFont));
                                    AppClass.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AppClass.Colspan = 3;
                                    //AppClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppClass.Border = 0;
                                    AppClass.BorderWidthLeft = 0.5f; AppClass.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppClass.BorderWidthTop = 0.5f; AppClass.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppClass);
                                }


                                PdfPCell AppAddress = new PdfPCell(new Phrase(Entity.City.Trim() + " " + Entity.State.Trim() + "  " + Entity.Zip.Trim(), TableFont));
                                AppAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppAddress.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                AppAddress.Border = 0;
                                AppAddress.BorderWidthLeft = 0.5f; AppAddress.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppAddress.BorderWidthTop = 0.5f; AppAddress.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(AppAddress);

                                MaskedTextBox mskWPhn = new MaskedTextBox();
                                mskWPhn.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(Entity.Empl_Phone.Trim()))
                                    mskWPhn.Text = Entity.Phone.Trim();
                                PdfPCell AppWrkPhn = new PdfPCell(new Phrase("Work :  " + mskWPhn.Text.Trim(), TableFont));
                                AppWrkPhn.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppWrkPhn.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                AppWrkPhn.Border = 0;
                                AppWrkPhn.BorderWidthLeft = 0.5f; AppWrkPhn.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppWrkPhn.BorderWidthTop = 0.5f; AppWrkPhn.BorderColorTop = BaseColor.LIGHT_GRAY;
                                AppWrkPhn.BorderWidthRight = 0.5f; AppWrkPhn.BorderColorRight = BaseColor.BLACK;
                                table.AddCell(AppWrkPhn);

                                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Space1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                Space1.Border = 0;
                                Space1.BorderWidthLeft = 0.5f;Space1.BorderColorLeft = BaseColor.BLACK;
                                Space1.BorderWidthTop = 0.5f;Space1.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(Space1);

                                MaskedTextBox mskPhn = new MaskedTextBox();
                                mskPhn.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(Entity.Phone.Trim()))
                                    mskPhn.Text = Entity.Phone.Trim();

                                PdfPCell AppDob = new PdfPCell(new Phrase("DOB: " + LookupDataAccess.Getdate(Entity.DOB.Trim()) + "    Phone: " + mskPhn.Text.Trim(), TableFont));
                                AppDob.HorizontalAlignment = Element.ALIGN_LEFT;
                                AppDob.Colspan = 2;
                                //AppDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                AppDob.Border = 0;
                                AppDob.BorderWidthLeft = 0.5f; AppDob.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppDob.BorderWidthTop = 0.5f; AppDob.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(AppDob);

                                string Income_Desc = string.Empty; string Meals = string.Empty;
                                if (Entity.Classfication.Trim() == "98" || Entity.Classfication.Trim() == "95") Income_Desc = "Over Inc.";
                                else if (Entity.Classfication.Trim() == "99") Income_Desc = "Eligible";
                                else if (Entity.Classfication.Trim() == "96") Income_Desc = "101% - 130%";
                                else if (Entity.Classfication.Trim() == "97") Income_Desc = "Categorical";
                                else Income_Desc = "In Certify";

                                string Meals_Type = string.Empty;
                                if (Entity.MealElig == "1") Meals_Type = "Free Meals";
                                else if (Entity.MealElig == "2") Meals_Type = "Reduced Meals";
                                else if (Entity.MealElig == "3") Meals_Type = "Paid Meals";

                                if (rdoSiteName.Checked == true)
                                {
                                    PdfPCell AppDet = new PdfPCell(new Phrase("      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type, TableFont));
                                    AppDet.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AppDet.Colspan = 2;
                                    //AppDet.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppDet.Border = 0;
                                    AppDet.BorderWidthLeft = 0.5f; AppDet.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppDet.BorderWidthTop = 0.5f; AppDet.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppDet);
                                }
                                else
                                {
                                    PdfPCell AppDet = new PdfPCell(new Phrase(Entity.Site.Trim() + "      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type, TableFont));
                                    AppDet.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AppDet.Colspan = 2;
                                    //AppDet.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppDet.Border = 0;
                                    AppDet.BorderWidthLeft = 0.5f; AppDet.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppDet.BorderWidthTop = 0.5f; AppDet.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppDet);
                                }

                                string Fund = string.Empty;
                                
                                if (!string.IsNullOrEmpty(Entity.CHLDMST_FUND.Trim()))
                                {
                                    foreach (CommonEntity drFund in propfundingSource)
                                    {
                                        if (Entity.CHLDMST_FUND.Trim() == drFund.Code.ToString().Trim())
                                        {
                                            Fund = drFund.Desc.ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                PdfPCell AppFund = new PdfPCell(new Phrase("Expected Fund :  " + Fund, TableFont));
                                AppFund.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppFund.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                AppFund.Border = 0;
                                AppFund.BorderWidthLeft = 0.5f; AppFund.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppFund.BorderWidthTop = 0.5f; AppFund.BorderColorTop = BaseColor.LIGHT_GRAY;
                                AppFund.BorderWidthRight = 0.5f; AppFund.BorderColorRight = BaseColor.BLACK;
                                table.AddCell(AppFund);

                                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Space2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                Space2.Border = 0;
                                Space2.BorderWidthLeft = 0.5f; Space2.BorderColorLeft = BaseColor.BLACK;
                                Space2.BorderWidthTop = 0.5f; Space2.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(Space2);

                                PdfPCell AppInc = new PdfPCell(new Phrase("Household Income  " + LookupDataAccess.GetAmountSep(Entity.Fam_Income.Trim()) + "      Household Size:  " + Entity.Family_count.Trim(), TableFont));
                                AppInc.HorizontalAlignment = Element.ALIGN_LEFT;
                                AppInc.Colspan = 3;
                                //AppInc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                AppInc.Border = 0;
                                AppInc.BorderWidthLeft = 0.5f; AppInc.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppInc.BorderWidthTop = 0.5f; AppInc.BorderColorTop = BaseColor.LIGHT_GRAY;
                                table.AddCell(AppInc);

                                if (Entity.Transport.Trim() == "None")
                                {
                                    PdfPCell AppTrans = new PdfPCell(new Phrase("No Transportation", TableFont));
                                    AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppTrans.Border = 0;
                                    AppTrans.BorderWidthLeft = 0.5f; AppTrans.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppTrans.BorderWidthTop = 0.5f; AppTrans.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppTrans);
                                }
                                else if (Entity.Transport.Trim() == "Not Defined in AGYTAB")
                                {
                                    PdfPCell AppTrans = new PdfPCell(new Phrase("", TableFont));
                                    AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppTrans.Border = 0;
                                    AppTrans.BorderWidthLeft = 0.5f; AppTrans.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppTrans.BorderWidthTop = 0.5f; AppTrans.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppTrans);
                                }
                                else
                                {
                                    PdfPCell AppTrans = new PdfPCell(new Phrase(Entity.Transport.Trim() + "  Trans", TableFont));
                                    AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AppTrans.Border = 0;
                                    AppTrans.BorderWidthLeft = 0.5f; AppTrans.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    AppTrans.BorderWidthTop = 0.5f; AppTrans.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(AppTrans);
                                }

                                string AltFund = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.Alt_FUND.Trim()))
                                {
                                    foreach (CommonEntity drFund in propfundingSource)
                                    {
                                        if (Entity.Alt_FUND.Trim() == drFund.Code.ToString().Trim())
                                        {
                                            AltFund = drFund.Desc.ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                PdfPCell AppAltFund = new PdfPCell(new Phrase("Alternate Fund:  " + AltFund, TableFont));
                                AppAltFund.HorizontalAlignment = Element.ALIGN_LEFT;
                                //AppAltFund.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                AppAltFund.Border = 0;
                                AppAltFund.BorderWidthLeft = 0.5f; AppAltFund.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                AppAltFund.BorderWidthTop = 0.5f; AppAltFund.BorderColorTop = BaseColor.LIGHT_GRAY;
                                AppAltFund.BorderWidthRight = 0.5f; AppAltFund.BorderColorRight = BaseColor.BLACK;
                                table.AddCell(AppAltFund);

                                if (rdoRank.Checked == true)
                                {
                                    if (Sel_Ranklist.Count > 0)
                                    {
                                        int TempR_Cnt = Sel_Ranklist.Count;
                                        string Rank1_Desc = string.Empty, Rank2_Desc = string.Empty, Rank3_Desc = string.Empty, Rank4_Desc = string.Empty, Rank5_Desc = string.Empty, Rank6_Desc = string.Empty;
                                        string Rank1_Det = "0", Rank2_Det = "0", Rank3_Det = "0", Rank4_Det = "0", Rank5_Det = "0", Rank6_Det = "0";
                                        for (int i = 0; i < Sel_Ranklist.Count; i++)
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    if (!string.IsNullOrEmpty(Entity.Rank1.Trim()) && Entity.Rank1.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[0].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank1_Desc = Sel_Ranklist[0].Desc.Trim();
                                                            Rank1_Det = Entity.Rank1.Trim();
                                                        }
                                                    }
                                                    break;
                                                case 1:
                                                    if (!string.IsNullOrEmpty(Entity.Rank2.Trim()) && Entity.Rank2.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[1].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank2_Desc = Sel_Ranklist[1].Desc.Trim();
                                                            Rank2_Det = Entity.Rank2.Trim();
                                                        }
                                                    }
                                                    break;
                                                case 2:
                                                    if (!string.IsNullOrEmpty(Entity.Rank3.Trim()) && Entity.Rank3.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[2].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank3_Desc = Sel_Ranklist[2].Desc.Trim();
                                                            Rank3_Det = Entity.Rank3.Trim();
                                                        }
                                                    }
                                                    break;
                                                case 3:
                                                    if (!string.IsNullOrEmpty(Entity.Rank4.Trim()) && Entity.Rank4.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[3].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank4_Desc = Sel_Ranklist[3].Desc.Trim();
                                                            Rank4_Det = Entity.Rank4.Trim();
                                                        }
                                                    }
                                                    break;
                                                case 4:
                                                    if (!string.IsNullOrEmpty(Entity.Rank5.Trim()) && Entity.Rank5.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[4].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank5_Desc = Sel_Ranklist[4].Desc.Trim();
                                                            Rank5_Det = Entity.Rank5.Trim();
                                                        }
                                                    }
                                                    break;
                                                case 6:
                                                    if (!string.IsNullOrEmpty(Entity.Rank6.Trim()) && Entity.Rank6.Trim() != "0")
                                                    {
                                                        if (Sel_Ranklist[5].HeadStrt.Trim() != "N")
                                                        {
                                                            Rank6_Desc = Sel_Ranklist[5].Desc.Trim();
                                                            Rank6_Det = Entity.Rank6.Trim();
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        int TotR = int.Parse(Rank1_Det) + int.Parse(Rank2_Det) + int.Parse(Rank3_Det) + int.Parse(Rank4_Det) + int.Parse(Rank5_Det) + int.Parse(Rank6_Det);

                                        //if (Rank1_Det == "0") Rank1_Det = ""; if (Rank2_Det == "0") Rank2_Det = ""; if (Rank3_Det == "0") Rank3_Det = "";
                                        //if (Rank4_Det == "0") Rank4_Det = ""; if (Rank5_Det == "0") Rank5_Det = ""; if (Rank6_Det == "0") Rank6_Det = "";

                                        PdfPCell Space4 = new PdfPCell(new Phrase("", TableFont));
                                        Space4.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        Space4.Border = 0;
                                        Space4.BorderWidthLeft = 0.5f;Space4.BorderColorLeft = BaseColor.BLACK;
                                        Space4.BorderWidthTop = 0.5f; Space4.BorderColorTop = BaseColor.LIGHT_GRAY;
                                        table.AddCell(Space4);

                                        if (string.IsNullOrEmpty(Rank1_Desc) && Rank1_Det == "0") Rank1_Det = "";
                                        if (string.IsNullOrEmpty(Rank2_Desc) && Rank2_Det == "0") Rank2_Det = "";
                                        if (string.IsNullOrEmpty(Rank3_Desc) && Rank3_Det == "0") Rank3_Det = "";
                                        if (string.IsNullOrEmpty(Rank4_Desc) && Rank4_Det == "0") Rank4_Det = "";
                                        if (string.IsNullOrEmpty(Rank5_Desc) && Rank5_Det == "0") Rank5_Det = "";
                                        if (string.IsNullOrEmpty(Rank6_Desc) && Rank6_Det == "0") Rank6_Det = "";

                                        string Total_Desc = string.Empty;
                                        if (TotR == 0)
                                            Total_Desc = "Ranking Total : " + TotR;
                                        else
                                        {
                                            Total_Desc = Rank1_Desc + "   " + Rank1_Det.ToString() + "    " + Rank2_Desc + "    " + Rank2_Det.ToString() + "    " + Rank3_Desc + " " + Rank3_Det.ToString()
                                            + "    " + Rank4_Desc + "    " + Rank4_Det.ToString() + "    " + Rank5_Desc + "    " + Rank5_Det.ToString() + "    " + Rank6_Desc + "    " + Rank6_Det.ToString() + " Total:  " + TotR;
                                        }


                                        PdfPCell Rank = new PdfPCell(new Phrase(Total_Desc, TableFont));
                                        Rank.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rank.Colspan = 5;
                                        //Rank.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        Rank.Border = 0;
                                        Rank.BorderWidthLeft = 0.5f; Rank.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                        Rank.BorderWidthTop = 0.5f; Rank.BorderColorTop = BaseColor.LIGHT_GRAY;
                                        Rank.BorderWidthRight = 0.5f; Rank.BorderColorRight = BaseColor.BLACK;
                                        table.AddCell(Rank);
                                    }
                                    else
                                    {
                                        PdfPCell Space4 = new PdfPCell(new Phrase("", TableFont));
                                        Space4.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        Space4.Border = 0;
                                        Space4.BorderWidthLeft = 0.5f; Space4.BorderColorLeft = BaseColor.BLACK;
                                        Space4.BorderWidthTop = 0.5f; Space4.BorderColorTop = BaseColor.LIGHT_GRAY;
                                        table.AddCell(Space4);

                                        PdfPCell Rank = new PdfPCell(new Phrase(" ", TableFont));
                                        Rank.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rank.Colspan = 5;
                                        //Rank.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        Rank.Border = 0;
                                        Rank.BorderWidthLeft = 0.5f; Rank.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                        Rank.BorderWidthTop = 0.5f; Rank.BorderColorTop = BaseColor.LIGHT_GRAY;
                                        Rank.BorderWidthRight = 0.5f; Rank.BorderColorRight = BaseColor.BLACK;
                                        table.AddCell(Rank);
                                    }
                                }


                                string Spl_Desc = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.CasecondApp.Trim()))
                                {
                                    if (!string.IsNullOrEmpty(Entity.Allergy.Trim())) Spl_Desc = Entity.Allergy.Trim();
                                    else if (!string.IsNullOrEmpty(Entity.DietRestrct.Trim())) Spl_Desc = Entity.DietRestrct.Trim();
                                    else if (!string.IsNullOrEmpty(Entity.Medications.Trim())) Spl_Desc = Entity.Medications.Trim();
                                    else if (!string.IsNullOrEmpty(Entity.MedConds.Trim())) Spl_Desc = Entity.MedConds.Trim();
                                    else if (!string.IsNullOrEmpty(Entity.HHConcerns.Trim())) Spl_Desc = Entity.HHConcerns.Trim();
                                    else if (!string.IsNullOrEmpty(Entity.DevlConcerns.Trim())) Spl_Desc = Entity.DevlConcerns.Trim();

                                    PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
                                    Space5.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Space5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    Space5.Border = 0;
                                    Space5.BorderWidthLeft = 0.5f; Space5.BorderColorLeft = BaseColor.BLACK;
                                    Space5.BorderWidthTop = 0.5f; Space5.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(Space5);

                                    string Disability = string.Empty;
                                    if (!string.IsNullOrEmpty(Entity.Disability_type.Trim())) Disability = Entity.Disability_type.Trim() + "\r\n";


                                    PdfPCell APPSpl = new PdfPCell(new Phrase(Disability + Spl_Desc, TableFont));
                                    APPSpl.HorizontalAlignment = Element.ALIGN_LEFT;
                                    APPSpl.Colspan = 5;
                                    //APPSpl.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    APPSpl.Border = 0;
                                    APPSpl.BorderWidthLeft = 0.5f; APPSpl.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    APPSpl.BorderWidthTop = 0.5f; APPSpl.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    APPSpl.BorderWidthRight = 0.5f; APPSpl.BorderColorRight = BaseColor.BLACK;
                                    table.AddCell(APPSpl);
                                }

                                if (string.IsNullOrEmpty(Entity.EnrlStatus.Trim()) && string.IsNullOrEmpty(Entity.EnrlAppNo.Trim()))
                                {
                                    PdfPCell Space15 = new PdfPCell(new Phrase("", TableFont));
                                    Space15.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Space15.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    Space15.Border = 0;
                                    Space15.BorderWidthLeft = 0.5f; Space15.BorderColorLeft = BaseColor.BLACK;
                                    Space15.BorderWidthTop = 0.5f; Space15.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    table.AddCell(Space15);

                                    PdfPCell Stat = new PdfPCell(new Phrase("INTAKE COMPLETED ....missing status record!", TableFont));
                                    Stat.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Stat.Colspan = 5;
                                    //Stat.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    Stat.Border = 0;
                                    Stat.BorderWidthLeft = 0.5f; Stat.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                    Stat.BorderWidthTop = 0.5f; Stat.BorderColorTop = BaseColor.LIGHT_GRAY;
                                    Stat.BorderWidthRight = 0.5f; Stat.BorderColorRight = BaseColor.BLACK;
                                    table.AddCell(Stat);

                                    EnrlCnt++;
                                }

                                if (chkbCasenotes.Checked == true)
                                {
                                    List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesWaitList(Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.AppNo);
                                    if (caseNotesEntity.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(caseNotesEntity[0].Data.Trim()))
                                        {
                                            PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
                                            Space5.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //Space5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            Space5.Border = 0;
                                            Space5.BorderWidthLeft = 0.5f; Space5.BorderColorLeft = BaseColor.BLACK;
                                            Space5.BorderWidthTop = 0.5f; Space5.BorderColorTop = BaseColor.LIGHT_GRAY;
                                            table.AddCell(Space5);

                                            PdfPCell APPSpl = new PdfPCell(new Phrase(caseNotesEntity[0].Data.Trim(), TableFont));
                                            APPSpl.HorizontalAlignment = Element.ALIGN_LEFT;
                                            APPSpl.Colspan = 5;
                                            //APPSpl.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            APPSpl.Border = 0;
                                            APPSpl.BorderWidthLeft = 0.5f; APPSpl.BorderColorLeft = BaseColor.LIGHT_GRAY;
                                            APPSpl.BorderWidthTop = 0.5f; APPSpl.BorderColorTop = BaseColor.LIGHT_GRAY;
                                            APPSpl.BorderWidthRight = 0.5f; APPSpl.BorderColorRight = BaseColor.BLACK;
                                            table.AddCell(APPSpl);
                                        }
                                    }
                                }

                                PdfPCell Space7 = new PdfPCell(new Phrase("", TableFont));
                                Space7.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space7.Colspan = 6;
                                Space7.Border = iTextSharp.text.Rectangle.TOP_BORDER; //iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Space7);

                                //PdfPCell Space8 = new PdfPCell(new Phrase("", TableFont));
                                //Space8.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Space8.Colspan = 6;
                                //Space8.Border = iTextSharp.text.Rectangle.NO_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //table.AddCell(Space8);
                                if (rbWaitlist.Checked || rbBoth.Checked) Priv_Site = Entity.EnrlSite.Trim(); else Priv_Site = Entity.Site.Trim();
                                Priv_Desc = Site_Desc.Trim();

                                if ((EnrlCnt > 5 && count >= 11) || (EnrlCnt >= 8 && count >= 10)) count = 13;

                                count++;

                            }
                            if (rdoSiteName.Checked == true)
                            {
                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }

                                
                                PrintCountHeader(Counttable, Priv_Site, Priv_Desc);

                                

                                PrintCountTable(Counttable, Priv_Site);

                                if (Counttable.Rows.Count > 0)
                                {
                                    document.Add(Counttable);
                                    Counttable.DeleteBodyRows();
                                    document.NewPage();
                                }

                                

                                string[] Footer123 = { "Report Totals:", "Income Eligible", "Over Income", "In Certify", "101% - 130%", "Categorical" };
                                for (int i = 0; i < Footer123.Length; ++i)
                                {
                                    PdfPCell Cnt = new PdfPCell(new Phrase(Footer123[i], TblFontBold));
                                    Cnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Cnt.FixedHeight = 15f;
                                    Cnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cnt.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Counttable.AddCell(Cnt);
                                }

                               

                                if (propfundingSource.Count > 0)
                                {
                                    if (rbDayCare.Checked || (rbFundSel.Checked))
                                    {
                                        
                                        string[] Funds = strFundingCodes.Split(',');
                                        if (Funds.Length > 0)
                                        {
                                            for (int i = 0; i < Funds.Length; i++)
                                            {
                                                CommonEntity FundEntity = propfundingSource.Find(u => u.Code.Equals(Funds[i].ToString()));
                                                if (FundEntity != null)
                                                {
                                                    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                                    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                                    List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                                    PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                                                    Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Desc.FixedHeight = 15f;
                                                    Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(Desc);

                                                    
                                                    //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                                                    IEentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
                                                    OIentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                                    ICentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
                                                    Pentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
                                                    CAentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));

                                                    PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                                                    IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    IECnt.FixedHeight = 15f;
                                                    IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(IECnt);

                                                    PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                                                    OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    OICnt.FixedHeight = 15f;
                                                    OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(OICnt);

                                                    PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                                                    ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    ICCnt.FixedHeight = 15f;
                                                    ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(ICCnt);

                                                    PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
                                                    PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    PCnt.FixedHeight = 15f;
                                                    PCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(PCnt);

                                                    PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
                                                    CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    CACnt.FixedHeight = 15f;
                                                    CACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Counttable.AddCell(CACnt);
                                                }
                                            }
                                        }
                                        

                                    }
                                    else if (rbFundAll.Checked)
                                    {
                                        foreach (CommonEntity FundEntity in propfundingSource)
                                        {
                                            List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                            PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Desc.FixedHeight = 15f;
                                            Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(Desc);

                                            //IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                            //OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98")|| u.Classfication.Trim().Equals("95")));
                                            //ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                            //Pentity = SelRecords.FindAll(u =>  u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                            //CAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

                                            //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                                            IEentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
                                            OIentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                            ICentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
                                            Pentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
                                            CAentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));

                                            PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                                            IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                            IECnt.FixedHeight = 15f;
                                            IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(IECnt);

                                            PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                                            OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                            OICnt.FixedHeight = 15f;
                                            OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(OICnt);

                                            PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                                            ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                            ICCnt.FixedHeight = 15f;
                                            ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(ICCnt);

                                            PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
                                            PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                            PCnt.FixedHeight = 15f;
                                            PCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(PCnt);

                                            PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
                                            CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                            CACnt.FixedHeight = 15f;
                                            CACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Counttable.AddCell(CACnt);

                                        }
                                    }

                                    PdfPCell Desc1 = new PdfPCell(new Phrase("No Fund Specified", TableFont));
                                    Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Desc1.FixedHeight = 15f;
                                    Desc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Counttable.AddCell(Desc1);

                                   
                                    //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                                    List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("99"));
                                    List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                    List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("00"));
                                    List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("96"));
                                    List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("97"));


                                    PdfPCell NICnt = new PdfPCell(new Phrase(SEentity.Count.ToString(), TableFont));
                                    NICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    NICnt.FixedHeight = 15f;
                                    NICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Counttable.AddCell(NICnt);

                                    PdfPCell NOCnt = new PdfPCell(new Phrase(SOentity.Count.ToString(), TableFont));
                                    NOCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    NOCnt.FixedHeight = 15f;
                                    NOCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Counttable.AddCell(NOCnt);

                                    PdfPCell NCCnt = new PdfPCell(new Phrase(SCentity.Count.ToString(), TableFont));
                                    NCCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    NCCnt.FixedHeight = 15f;
                                    NCCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Counttable.AddCell(NCCnt);

                                    PdfPCell NPCnt = new PdfPCell(new Phrase(SPentity.Count.ToString(), TableFont));
                                    NPCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    NPCnt.FixedHeight = 15f;
                                    NPCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(NPCnt);

                                    PdfPCell NCACnt = new PdfPCell(new Phrase(SCAentity.Count.ToString(), TableFont));
                                    NCACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    NCACnt.FixedHeight = 15f;
                                    NCACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(NCACnt);

                                }

                                if (Counttable.Rows.Count > 0)
                                {
                                    document.Add(Counttable);
                                    Counttable.DeleteBodyRows();

                                }
                            }
                            else
                            {
                                document.Add(table);
                            }

                        }
                    }

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                document.Close();
                fs.Close();
                fs.Dispose();

                AlertBox.Show("Report Generated Successfully");

                if (rbMailLbl.Checked && SelRecords_Labels.Count > 0)
                {
                    On_SaveLabelsReport(PdfName);
                }

            }
        }



        //private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        //{
        //    PdfListForm form = sender as PdfListForm;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        string PdfName = "Pdf File";
        //        PdfName = form.GetFileName();

        //        PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //        try
        //        {
        //            if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //            { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //        }
        //        catch (Exception ex)
        //        {
        //           AlertBox.Show("Error", MessageBoxIcon.Error);
        //        }


        //        try
        //        {
        //            string Tmpstr = PdfName + ".pdf";
        //            if (File.Exists(Tmpstr))
        //                File.Delete(Tmpstr);
        //        }
        //        catch (Exception ex)
        //        {
        //            int length = 8;
        //            string newFileName = System.Guid.NewGuid().ToString();
        //            newFileName = newFileName.Replace("-", string.Empty);

        //            Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //        }


        //        if (!string.IsNullOrEmpty(Random_Filename))
        //            PdfName = Random_Filename;
        //        else
        //            PdfName += ".pdf";

        //        FileStream fs = new FileStream(PdfName, FileMode.Create);

        //        //Document document = new Document();
        //        Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
        //        PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //        document.Open();
        //        cb = writer.DirectContent;

        //        //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //        iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //        iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
        //        iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
        //        iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
        //        iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
        //        iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
        //        iTextSharp.text.Font TableFontRed = new iTextSharp.text.Font(bf_times, 8,1,BaseColor.RED);

        //        if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
        //        string Year = string.Empty;
        //        if (CmbYear.Visible == true)
        //            Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

        //        CaseSiteEntity SearchEntity =new CaseSiteEntity(true);
        //        SearchEntity.SiteROOM = "0000"; SearchEntity.SiteAGENCY = Agency; SearchEntity.SiteDEPT = Depart; SearchEntity.SitePROG = Program;
        //        SearchEntity.SiteYEAR = Year;
        //        propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(SearchEntity, "Browse");

        //        try
        //        {
        //            PrintHeaderPage(document, writer);
        //            document.NewPage();

        //            PdfPTable table = new PdfPTable(6);
        //            table.TotalWidth = 550f;
        //            table.WidthPercentage = 100;
        //            table.LockedWidth = true;
        //            float[] widths = new float[] { 12f, 20f, 50f, 35f, 65f, 55f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //            table.SetWidths(widths);
        //            table.HorizontalAlignment = Element.ALIGN_CENTER;

        //            PdfPTable Counttable = new PdfPTable(6);
        //            Counttable.TotalWidth = 500f;
        //            Counttable.WidthPercentage = 100;
        //            Counttable.LockedWidth = true;
        //            float[] Counttablewidths = new float[] { 20f, 20f, 20f, 20f,20f,20f};// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //            Counttable.SetWidths(Counttablewidths);
        //            Counttable.HorizontalAlignment = Element.ALIGN_LEFT;

        //            if (SelRecords.Count > 0)
        //            {
        //                SelRecords = SelRecords.FindAll(u => Convert.ToInt32(u.Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Age) <= Convert.ToInt32(txtTo.Text));

        //                if (((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Trim() != "0")
        //                    SelRecords = SelRecords.FindAll(u => u.MSTCatElig.Equals(((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Value.ToString().Trim()));

        //                CaseSiteEntity Sel_Site = new CaseSiteEntity();
        //                if (SelRecords.Count > 0)
        //                {

        //                    if (rdoRank.Checked == true)
        //                    {   
        //                        PrintMainHeader(table);
        //                    }

        //                    bool first = true; string Priv_Site = string.Empty; string Priv_Desc = string.Empty; string Site_Desc = string.Empty;
        //                    foreach (HSSB2115Entity Entity in SelRecords)
        //                    {
        //                        if (rdoSiteName.Checked == true)
        //                        {
        //                            if(rbIntakeCmpltd.Checked==false? Entity.EnrlSite.Trim() != Priv_Site : Entity.Site.Trim() != Priv_Site)

        //                            //if ((rbWaitlist.Checked && Entity.EnrlSite.Trim() != Priv_Site) || (Entity.Site.Trim() != Priv_Site))
        //                            {
        //                                if (propCaseSiteEntity.Count > 0)
        //                                {
        //                                    if(rbWaitlist.Checked || rbBoth.Checked)
        //                                        Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(Entity.EnrlSite.Trim()) && u.SiteYEAR.Equals(Year));
        //                                    else 
        //                                        Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(Entity.Site.Trim()) && u.SiteYEAR.Equals(Year));

        //                                    if (Sel_Site != null)
        //                                        Site_Desc = Sel_Site.SiteNAME.Trim();
        //                                    else Site_Desc = "";
        //                                }

        //                                if (!first)
        //                                {
        //                                    if (table.Rows.Count > 0)
        //                                    {
        //                                        document.Add(table);
        //                                        table.DeleteBodyRows();
        //                                        document.NewPage();
        //                                    }

        //                                    PrintCountHeader(Counttable, Priv_Site, Priv_Desc);


        //                                    PrintCountTable(Counttable, Priv_Site);

        //                                    if (Counttable.Rows.Count > 0)
        //                                    {
        //                                        document.Add(Counttable);
        //                                        Counttable.DeleteBodyRows();
        //                                        document.NewPage();

        //                                        string SiteName = string.Empty;
        //                                        if (rbWaitlist.Checked || rbBoth.Checked)
        //                                            SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
        //                                        else
        //                                            SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

        //                                        PdfPCell Header2 = new PdfPCell(new Phrase(SiteName, TblFontBold));
        //                                        Header2.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                        Header2.Colspan = 6;
        //                                        Header2.FixedHeight = 15f;
        //                                        Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                        table.AddCell(Header2);

        //                                        //string[] HeaderSeq4 = { "Rank", "App.", "Name", "", "Address", "Parent / Guardian" };
        //                                        //for (int i = 0; i < HeaderSeq4.Length; ++i)
        //                                        //{
        //                                        //    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
        //                                        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                        //    cell.FixedHeight = 15f;
        //                                        //    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                        //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                                        //    table.AddCell(cell);
        //                                        //}
        //                                        PrintMainHeader(table);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    string SiteName = string.Empty;
        //                                    if (rbWaitlist.Checked || rbBoth.Checked)
        //                                        SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
        //                                    else
        //                                        SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

        //                                    PdfPCell Header = new PdfPCell(new Phrase(SiteName, TblFontBold));
        //                                    Header.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    Header.Colspan = 6;
        //                                    Header.FixedHeight = 15f;
        //                                    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(Header);

        //                                    //string[] HeaderSeq4 = { "Rank", "App.", "Name", "", "Address", "Parent / Guardian" };
        //                                    //for (int i = 0; i < HeaderSeq4.Length; ++i)
        //                                    //{
        //                                    //    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
        //                                    //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    //    cell.FixedHeight = 15f;
        //                                    //    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                    //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                                    //    table.AddCell(cell);
        //                                    //}
        //                                    PrintMainHeader(table);
        //                                    first = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if ((first && Entity.EnrlSite.Trim() == Priv_Site && (rbWaitlist.Checked || rbBoth.Checked)) || (first && Entity.Site.Trim() == Priv_Site))
        //                                {
        //                                    string SiteName = string.Empty;
        //                                    if (rbWaitlist.Checked || rbBoth.Checked)
        //                                        SiteName = "All applicants for site " + Entity.EnrlSite.Trim() + "  " + Site_Desc.Trim();
        //                                    else
        //                                        SiteName = "All applicants for site " + Entity.Site.Trim() + "  " + Site_Desc.Trim();

        //                                    PdfPCell Header = new PdfPCell(new Phrase(SiteName, TblFontBold));
        //                                    Header.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    Header.Colspan = 6;
        //                                    Header.FixedHeight = 15f;
        //                                    Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(Header);

        //                                    //string[] HeaderSeq4 = { "Rank", "App.", "Name", "", "Address", "Parent / Guardian" };
        //                                    //for (int i = 0; i < HeaderSeq4.Length; ++i)
        //                                    //{
        //                                    //    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
        //                                    //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    //    cell.FixedHeight = 15f;
        //                                    //    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                    //    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                                    //    table.AddCell(cell);
        //                                    //}
        //                                    PrintMainHeader(table);
        //                                    first = false;
        //                                }
        //                            }
        //                        }

        //                        if (Entity.AppNo.Trim() == "00162300")
        //                        {
        //                            //MessageBox.Show
        //                        }

        //                        SelRecords_Labels.Add(new HSSB2115Entity(Entity));

        //                        PdfPCell Rnk = new PdfPCell(new Phrase(Entity.Rank.Trim(), TableFont));
        //                        Rnk.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                        Rnk.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                        table.AddCell(Rnk);

        //                        PdfPCell App = new PdfPCell(new Phrase(" " + Entity.AppNo.Trim(), TableFont));
        //                        App.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        App.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(App);

        //                        if (Entity.Disable.Trim() == "Y")
        //                        {
        //                            var fontred = FontFactory.GetFont("Times", 9,BaseColor.RED.Darker());
        //                            //var disabtext = new Chunk("*", fontred);
        //                            //var Name = new Chunk(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont);
        //                            var phrase = new Phrase();
        //                            phrase.Add(new Chunk("*", fontred));
        //                            phrase.Add(new Chunk(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont));

        //                            PdfPCell AppName = new PdfPCell(phrase);
        //                            AppName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppName);
        //                        }
        //                        else
        //                        {
        //                            PdfPCell AppName = new PdfPCell(new Phrase(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont));
        //                            AppName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppName);
        //                        }

        //                        PdfPCell AppAge = new PdfPCell(new Phrase("Age  " + Entity.Age.Trim() + " yrs  " + Entity.Months.Trim() + " mos", TableFont));
        //                        AppAge.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppAge.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(AppAge);

        //                        string Apt = string.Empty; string Floor = string.Empty;
        //                        if (!string.IsNullOrEmpty(Entity.Apt.Trim()))
        //                            Apt = "  Apt  " + Entity.Apt.Trim();
        //                        if (!string.IsNullOrEmpty(Entity.Flr.Trim()))
        //                            Floor = "  Flr  " + Entity.Flr.Trim();
        //                        PdfPCell AppStreet = new PdfPCell(new Phrase(Entity.Hno.Trim() + "   " + Entity.Street.Trim() + " " + Entity.Suffix.Trim() + Apt + Floor, TableFont));
        //                        AppStreet.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppStreet.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(AppStreet);

        //                        PdfPCell AppGuardian = new PdfPCell(new Phrase(Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim(), TableFont));
        //                        AppGuardian.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppGuardian.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        table.AddCell(AppGuardian);

        //                        PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
        //                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        Space.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                        table.AddCell(Space);

        //                        string Sex = string.Empty; string Class = string.Empty;
        //                        if (Entity.Sex.Trim() == "M") Sex = "MALE"; else Sex = "FEMALE";
        //                        if (Entity.ClassPrefer.Trim() == "A") Class = "AM";
        //                        else if (Entity.ClassPrefer.Trim() == "P") Class = "PM";
        //                        else if (Entity.Classfication.Trim() == "E") Class = "EXTD"; else if (Entity.ClassPrefer.Trim() == "F") Class = "FULL";

        //                        string CatgDesc = string.Empty;
        //                        if (lookUpCategrcalEligiblity.Count > 0)
        //                        {
        //                            if (!string.IsNullOrEmpty(Entity.MSTCatElig.Trim()))
        //                            {
        //                                foreach (CommonEntity looks in lookUpCategrcalEligiblity)
        //                                {
        //                                    if (looks.Code == Entity.MSTCatElig.Trim())
        //                                    {
        //                                        CatgDesc = "       Categorical: " + looks.Desc.Trim();
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                                //CatgDesc = "       Categorical: " + lookUpCategrcalEligiblity.Find(u => u.Code.Equals(Entity.MSTCatElig.Trim())).Desc.Trim();
        //                        }


        //                        if (Entity.ClassPrefer.Trim() == "N")
        //                        {
        //                            PdfPCell AppClass = new PdfPCell(new Phrase("PREFER" + "       " + " SLOT    " + Sex.Trim() + CatgDesc, TableFont));
        //                            AppClass.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppClass.Colspan = 3;
        //                            AppClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppClass);
        //                        }
        //                        else
        //                        {
        //                            PdfPCell AppClass = new PdfPCell(new Phrase("PREFER    " + Class + " SLOT  " + Sex.Trim() + CatgDesc, TableFont));
        //                            AppClass.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppClass.Colspan = 3;
        //                            AppClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppClass);
        //                        }


        //                        PdfPCell AppAddress = new PdfPCell(new Phrase(Entity.City.Trim() + " " + Entity.State.Trim() + "  " + Entity.Zip.Trim(), TableFont));
        //                        AppAddress.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppAddress.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(AppAddress);

        //                        MaskedTextBox mskWPhn = new MaskedTextBox();
        //                        mskWPhn.Mask = "(999) 000-0000";
        //                        if (!string.IsNullOrEmpty(Entity.Empl_Phone.Trim()))
        //                            mskWPhn.Text = Entity.Phone.Trim();
        //                        PdfPCell AppWrkPhn = new PdfPCell(new Phrase("Work :  " + mskWPhn.Text.Trim(), TableFont));
        //                        AppWrkPhn.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppWrkPhn.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        table.AddCell(AppWrkPhn);

        //                        PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
        //                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        Space1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                        table.AddCell(Space1);

        //                        MaskedTextBox mskPhn = new MaskedTextBox();
        //                        mskPhn.Mask = "(999) 000-0000";
        //                        if (!string.IsNullOrEmpty(Entity.Phone.Trim()))
        //                            mskPhn.Text = Entity.Phone.Trim();

        //                        PdfPCell AppDob = new PdfPCell(new Phrase("DOB: " + LookupDataAccess.Getdate(Entity.DOB.Trim()) + "    Phone: " + mskPhn.Text.Trim(), TableFont));
        //                        AppDob.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppDob.Colspan = 2;
        //                        AppDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(AppDob);

        //                        string Income_Desc = string.Empty; string Meals = string.Empty;
        //                        if (Entity.Classfication.Trim() == "98" || Entity.Classfication.Trim() == "95") Income_Desc = "Over Inc.";
        //                        else if (Entity.Classfication.Trim() == "99") Income_Desc = "Eligible";
        //                        else if (Entity.Classfication.Trim() == "96") Income_Desc = "101% - 130%";
        //                        else if (Entity.Classfication.Trim() == "97") Income_Desc = "Categorical";
        //                        else Income_Desc = "In Certify";

        //                        string Meals_Type = string.Empty;
        //                        if (Entity.MealElig == "1") Meals_Type = "Free Meals";
        //                        else if (Entity.MealElig == "2") Meals_Type = "Reduced Meals";
        //                        else if (Entity.MealElig == "3") Meals_Type = "Paid Meals";

        //                        if (rdoSiteName.Checked == true)
        //                        {
        //                            PdfPCell AppDet = new PdfPCell(new Phrase("      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type, TableFont));
        //                            AppDet.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppDet.Colspan = 2;
        //                            AppDet.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppDet);
        //                        }
        //                        else
        //                        {
        //                            PdfPCell AppDet = new PdfPCell(new Phrase(Entity.Site.Trim() + "      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type, TableFont));
        //                            AppDet.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppDet.Colspan = 2;
        //                            AppDet.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppDet);
        //                        }

        //                        string Fund = string.Empty;
        //                        //if (!string.IsNullOrEmpty(Entity.EnrlFundHie.Trim()))
        //                        //{
        //                        //    foreach (CommonEntity drFund in propfundingSource)
        //                        //    {
        //                        //        if (Entity.EnrlFundHie.Trim() == drFund.Code.ToString().Trim())
        //                        //        {
        //                        //            Fund = drFund.Desc.ToString().Trim();
        //                        //            break;
        //                        //        }
        //                        //    }
        //                        //}
        //                        if (!string.IsNullOrEmpty(Entity.CHLDMST_FUND.Trim()))
        //                        {
        //                            foreach (CommonEntity drFund in propfundingSource)
        //                            {
        //                                if (Entity.CHLDMST_FUND.Trim() == drFund.Code.ToString().Trim())
        //                                {
        //                                    Fund = drFund.Desc.ToString().Trim();
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                        PdfPCell AppFund = new PdfPCell(new Phrase("Expected Fund :  " + Fund, TableFont));
        //                        AppFund.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppFund.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        table.AddCell(AppFund);

        //                        PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
        //                        Space2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        Space2.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                        table.AddCell(Space2);

        //                        PdfPCell AppInc = new PdfPCell(new Phrase("Household Income  " + LookupDataAccess.GetAmountSep(Entity.Fam_Income.Trim()) + "      Household Size:  " + Entity.Family_count.Trim(), TableFont));
        //                        AppInc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppInc.Colspan = 3;
        //                        AppInc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        table.AddCell(AppInc);

        //                        if (Entity.Transport.Trim() == "None")
        //                        {
        //                            PdfPCell AppTrans = new PdfPCell(new Phrase("No Transportation", TableFont));
        //                            AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppTrans);
        //                        }
        //                        else if (Entity.Transport.Trim() == "Not Defined in AGYTAB")
        //                        {
        //                            PdfPCell AppTrans = new PdfPCell(new Phrase("", TableFont));
        //                            AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppTrans);
        //                        }
        //                        else
        //                        {
        //                            PdfPCell AppTrans = new PdfPCell(new Phrase(Entity.Transport.Trim() + "  Trans", TableFont));
        //                            AppTrans.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            AppTrans.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(AppTrans);
        //                        }

        //                        string AltFund = string.Empty;
        //                        if (!string.IsNullOrEmpty(Entity.Alt_FUND.Trim()))
        //                        {
        //                            foreach (CommonEntity drFund in propfundingSource)
        //                            {
        //                                if (Entity.Alt_FUND.Trim() == drFund.Code.ToString().Trim())
        //                                {
        //                                    AltFund = drFund.Desc.ToString().Trim();
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                        PdfPCell AppAltFund = new PdfPCell(new Phrase("Alternate Fund:  " + AltFund, TableFont));
        //                        AppAltFund.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        AppAltFund.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        table.AddCell(AppAltFund);

        //                        if (rdoRank.Checked == true)
        //                        {
        //                            if (Sel_Ranklist.Count > 0)
        //                            {
        //                                int TempR_Cnt = Sel_Ranklist.Count;
        //                                string Rank1_Desc = string.Empty, Rank2_Desc = string.Empty, Rank3_Desc = string.Empty, Rank4_Desc = string.Empty, Rank5_Desc = string.Empty, Rank6_Desc = string.Empty;
        //                                string Rank1_Det = "0", Rank2_Det = "0", Rank3_Det = "0", Rank4_Det = "0", Rank5_Det = "0", Rank6_Det = "0";
        //                                for (int i = 0; i < Sel_Ranklist.Count; i++)
        //                                {
        //                                    switch (i)
        //                                    {
        //                                        case 0: if (!string.IsNullOrEmpty(Entity.Rank1.Trim()) && Entity.Rank1.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[0].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank1_Desc = Sel_Ranklist[0].Desc.Trim();
        //                                                    Rank1_Det = Entity.Rank1.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                        case 1: if (!string.IsNullOrEmpty(Entity.Rank2.Trim()) && Entity.Rank2.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[1].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank2_Desc = Sel_Ranklist[1].Desc.Trim();
        //                                                    Rank2_Det = Entity.Rank2.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                        case 2: if (!string.IsNullOrEmpty(Entity.Rank3.Trim()) && Entity.Rank3.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[2].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank3_Desc = Sel_Ranklist[2].Desc.Trim();
        //                                                    Rank3_Det = Entity.Rank3.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                        case 3: if (!string.IsNullOrEmpty(Entity.Rank4.Trim()) && Entity.Rank4.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[3].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank4_Desc = Sel_Ranklist[3].Desc.Trim();
        //                                                    Rank4_Det = Entity.Rank4.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                        case 4:
        //                                            if (!string.IsNullOrEmpty(Entity.Rank5.Trim()) && Entity.Rank5.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[4].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank5_Desc = Sel_Ranklist[4].Desc.Trim();
        //                                                    Rank5_Det = Entity.Rank5.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                        case 6:
        //                                            if (!string.IsNullOrEmpty(Entity.Rank6.Trim()) && Entity.Rank6.Trim() != "0")
        //                                            {
        //                                                if (Sel_Ranklist[5].HeadStrt.Trim() != "N")
        //                                                {
        //                                                    Rank6_Desc = Sel_Ranklist[5].Desc.Trim();
        //                                                    Rank6_Det = Entity.Rank6.Trim();
        //                                                }
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                int TotR = int.Parse(Rank1_Det) + int.Parse(Rank2_Det) + int.Parse(Rank3_Det) + int.Parse(Rank4_Det) + int.Parse(Rank5_Det) + int.Parse(Rank6_Det);

        //                                //if (Rank1_Det == "0") Rank1_Det = ""; if (Rank2_Det == "0") Rank2_Det = ""; if (Rank3_Det == "0") Rank3_Det = "";
        //                                //if (Rank4_Det == "0") Rank4_Det = ""; if (Rank5_Det == "0") Rank5_Det = ""; if (Rank6_Det == "0") Rank6_Det = "";

        //                                PdfPCell Space4 = new PdfPCell(new Phrase("", TableFont));
        //                                Space4.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Space4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                                table.AddCell(Space4);

        //                                if (string.IsNullOrEmpty(Rank1_Desc) && Rank1_Det == "0") Rank1_Det = "";
        //                                if (string.IsNullOrEmpty(Rank2_Desc) && Rank2_Det == "0") Rank2_Det = "";
        //                                if (string.IsNullOrEmpty(Rank3_Desc) && Rank3_Det == "0") Rank3_Det = "";
        //                                if (string.IsNullOrEmpty(Rank4_Desc) && Rank4_Det == "0") Rank4_Det = "";
        //                                if (string.IsNullOrEmpty(Rank5_Desc) && Rank5_Det == "0") Rank5_Det = "";
        //                                if (string.IsNullOrEmpty(Rank6_Desc) && Rank6_Det == "0") Rank6_Det = "";

        //                                string Total_Desc = string.Empty;
        //                                if (TotR == 0)
        //                                    Total_Desc = "Ranking Total : " + TotR;
        //                                else
        //                                {
        //                                    Total_Desc = Rank1_Desc + "   " + Rank1_Det.ToString() + "    " + Rank2_Desc + "    " + Rank2_Det.ToString() + "    " + Rank3_Desc + " " + Rank3_Det.ToString()
        //                                    + "    " + Rank4_Desc + "    " + Rank4_Det.ToString() + "    " + Rank5_Desc + "    " + Rank5_Det.ToString() + "    " + Rank6_Desc + "    " + Rank6_Det.ToString() + " Total:  " + TotR;
        //                                }


        //                                PdfPCell Rank = new PdfPCell(new Phrase(Total_Desc, TableFont));
        //                                Rank.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Rank.Colspan = 5;
        //                                Rank.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                                table.AddCell(Rank);
        //                            }
        //                            else
        //                            {
        //                                PdfPCell Space4 = new PdfPCell(new Phrase("", TableFont));
        //                                Space4.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Space4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                                table.AddCell(Space4);

        //                                PdfPCell Rank = new PdfPCell(new Phrase(" ", TableFont));
        //                                Rank.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Rank.Colspan = 5;
        //                                Rank.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                                table.AddCell(Rank);
        //                            }
        //                        }


        //                        string Spl_Desc = string.Empty;
        //                        if (!string.IsNullOrEmpty(Entity.CasecondApp.Trim()))
        //                        {
        //                            if (!string.IsNullOrEmpty(Entity.Allergy.Trim())) Spl_Desc = Entity.Allergy.Trim();
        //                            else if (!string.IsNullOrEmpty(Entity.DietRestrct.Trim())) Spl_Desc = Entity.DietRestrct.Trim();
        //                            else if (!string.IsNullOrEmpty(Entity.Medications.Trim())) Spl_Desc = Entity.Medications.Trim();
        //                            else if (!string.IsNullOrEmpty(Entity.MedConds.Trim())) Spl_Desc = Entity.MedConds.Trim();
        //                            else if (!string.IsNullOrEmpty(Entity.HHConcerns.Trim())) Spl_Desc = Entity.HHConcerns.Trim();
        //                            else if (!string.IsNullOrEmpty(Entity.DevlConcerns.Trim())) Spl_Desc = Entity.DevlConcerns.Trim();

        //                            PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
        //                            Space5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            Space5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                            table.AddCell(Space5);

        //                            string Disability = string.Empty;
        //                            if (!string.IsNullOrEmpty(Entity.Disability_type.Trim())) Disability = Entity.Disability_type.Trim() + "\r\n";


        //                            PdfPCell APPSpl = new PdfPCell(new Phrase(Disability + Spl_Desc, TableFont));
        //                            APPSpl.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            APPSpl.Colspan = 5;
        //                            APPSpl.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                            table.AddCell(APPSpl);
        //                        }

        //                        if (string.IsNullOrEmpty(Entity.EnrlStatus.Trim()) && string.IsNullOrEmpty(Entity.EnrlAppNo.Trim()))
        //                        {
        //                            PdfPCell Space15 = new PdfPCell(new Phrase("", TableFont));
        //                            Space15.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            Space15.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                            table.AddCell(Space15);

        //                            PdfPCell Stat = new PdfPCell(new Phrase("INTAKE COMPLETED ....missing status record!", TableFont));
        //                            Stat.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            Stat.Colspan = 5;
        //                            Stat.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                            table.AddCell(Stat);
        //                        }

        //                        if (chkbCasenotes.Checked == true)
        //                        {
        //                            List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesWaitList(Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.AppNo);
        //                            if (caseNotesEntity.Count > 0)
        //                            {
        //                                if (!string.IsNullOrEmpty(caseNotesEntity[0].Data.Trim()))
        //                                {
        //                                    PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
        //                                    Space5.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    Space5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
        //                                    table.AddCell(Space5);

        //                                    PdfPCell APPSpl = new PdfPCell(new Phrase(caseNotesEntity[0].Data.Trim(), TableFont));
        //                                    APPSpl.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    APPSpl.Colspan = 5;
        //                                    APPSpl.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                                    table.AddCell(APPSpl);
        //                                }
        //                            }
        //                        }

        //                        PdfPCell Space7 = new PdfPCell(new Phrase("", TableFont));
        //                        Space7.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        Space7.Colspan = 6;
        //                        Space7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        table.AddCell(Space7);

        //                        //PdfPCell Space8 = new PdfPCell(new Phrase("", TableFont));
        //                        //Space8.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        //Space8.Colspan = 6;
        //                        //Space8.Border = iTextSharp.text.Rectangle.NO_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
        //                        //table.AddCell(Space8);
        //                        if(rbWaitlist.Checked || rbBoth.Checked) Priv_Site = Entity.EnrlSite.Trim(); else Priv_Site = Entity.Site.Trim();
        //                        Priv_Desc = Site_Desc.Trim();

        //                    }
        //                    if (rdoSiteName.Checked == true)
        //                    {
        //                        if (table.Rows.Count > 0)
        //                        {
        //                            document.Add(table);
        //                            table.DeleteBodyRows();
        //                            document.NewPage();
        //                        }

        //                        //PdfPCell Header21 = new PdfPCell(new Phrase("All applicants for site " + Priv_Site + "  " + Priv_Desc, TblFontBold));
        //                        //Header21.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //Header21.Colspan = 4;
        //                        //Header21.FixedHeight = 15f;
        //                        //Header21.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //Counttable.AddCell(Header21);

        //                        //string[] Header123 = { "Site Totals:", "Income Eligible", "Over Income", "In Certify" };
        //                        //for (int i = 0; i < Header123.Length; ++i)
        //                        //{
        //                        //    PdfPCell Cnt = new PdfPCell(new Phrase(Header123[i], TblFontBold));
        //                        //    Cnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    Cnt.FixedHeight = 15f;
        //                        //    Cnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Cnt.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                        //    Counttable.AddCell(Cnt);
        //                        //}
        //                        PrintCountHeader(Counttable, Priv_Site, Priv_Desc);

        //                        //if (propfundingSource.Count > 0)
        //                        //{
        //                        //    foreach (CommonEntity FundEntity in propfundingSource)
        //                        //    {
        //                        //        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
        //                        //        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
        //                        //        PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
        //                        //        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        //        Desc.FixedHeight = 15f;
        //                        //        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(Desc);

        //                        //        IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
        //                        //        OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("98"));
        //                        //        ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));

        //                        //        PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
        //                        //        IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        IECnt.FixedHeight = 15f;
        //                        //        IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(IECnt);

        //                        //        PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
        //                        //        OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        OICnt.FixedHeight = 15f;
        //                        //        OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(OICnt);

        //                        //        PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
        //                        //        ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        ICCnt.FixedHeight = 15f;
        //                        //        ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(ICCnt);
        //                        //    }

        //                        //    PdfPCell Desc1 = new PdfPCell(new Phrase("No Fund Specified", TableFont));
        //                        //    Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        //    Desc1.FixedHeight = 15f;
        //                        //    Desc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(Desc1);

        //                        //    List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
        //                        //    List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("98"));
        //                        //    List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));

        //                        //    PdfPCell NICnt = new PdfPCell(new Phrase(SEentity.Count.ToString(), TableFont));
        //                        //    NICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NICnt.FixedHeight = 15f;
        //                        //    NICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NICnt);

        //                        //    PdfPCell NOCnt = new PdfPCell(new Phrase(SOentity.Count.ToString(), TableFont));
        //                        //    NOCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NOCnt.FixedHeight = 15f;
        //                        //    NOCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NOCnt);

        //                        //    PdfPCell NCCnt = new PdfPCell(new Phrase(SCentity.Count.ToString(), TableFont));
        //                        //    NCCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NCCnt.FixedHeight = 15f;
        //                        //    NCCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NCCnt);
        //                        //}

        //                        PrintCountTable(Counttable, Priv_Site);

        //                        if (Counttable.Rows.Count > 0)
        //                        {
        //                            document.Add(Counttable);
        //                            Counttable.DeleteBodyRows();
        //                            document.NewPage();
        //                        }

        //                        //PdfPCell Footer21 = new PdfPCell(new Phrase("All applicants for site " + Priv_Site + "  " + Priv_Desc, TblFontBold));
        //                        //Footer21.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //Footer21.Colspan = 4;
        //                        //Footer21.FixedHeight = 15f;
        //                        //Footer21.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //Counttable.AddCell(Footer21);

        //                        string[] Footer123 = { "Report Totals:", "Income Eligible", "Over Income", "In Certify", "101% - 130%", "Categorical" };
        //                        for (int i = 0; i < Footer123.Length; ++i)
        //                        {
        //                            PdfPCell Cnt = new PdfPCell(new Phrase(Footer123[i], TblFontBold));
        //                            Cnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            Cnt.FixedHeight = 15f;
        //                            Cnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Cnt.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            Counttable.AddCell(Cnt);
        //                        }

        //                        //if (propfundingSource.Count > 0)
        //                        //{
        //                        //    foreach (CommonEntity FundEntity in propfundingSource)
        //                        //    {
        //                        //        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
        //                        //        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
        //                        //        PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
        //                        //        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        //        Desc.FixedHeight = 15f;
        //                        //        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(Desc);

        //                        //        IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
        //                        //        OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("98"));
        //                        //        ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));

        //                        //        PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
        //                        //        IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        IECnt.FixedHeight = 15f;
        //                        //        IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(IECnt);

        //                        //        PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
        //                        //        OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        OICnt.FixedHeight = 15f;
        //                        //        OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(OICnt);

        //                        //        PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
        //                        //        ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //        ICCnt.FixedHeight = 15f;
        //                        //        ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //        Counttable.AddCell(ICCnt);
        //                        //    }

        //                        //    PdfPCell Desc1 = new PdfPCell(new Phrase("No Fund Specified", TableFont));
        //                        //    Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                        //    Desc1.FixedHeight = 15f;
        //                        //    Desc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(Desc1);

        //                        //    List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
        //                        //    List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("98"));
        //                        //    List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));

        //                        //    PdfPCell NICnt = new PdfPCell(new Phrase(SEentity.Count.ToString(), TableFont));
        //                        //    NICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NICnt.FixedHeight = 15f;
        //                        //    NICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NICnt);

        //                        //    PdfPCell NOCnt = new PdfPCell(new Phrase(SOentity.Count.ToString(), TableFont));
        //                        //    NOCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NOCnt.FixedHeight = 15f;
        //                        //    NOCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NOCnt);

        //                        //    PdfPCell NCCnt = new PdfPCell(new Phrase(SCentity.Count.ToString(), TableFont));
        //                        //    NCCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        //    NCCnt.FixedHeight = 15f;
        //                        //    NCCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                        //    Counttable.AddCell(NCCnt);
        //                        //}

        //                        if (propfundingSource.Count > 0)
        //                        {
        //                            if (rbDayCare.Checked || (rbFundSel.Checked))
        //                            {
        //                                //if (rbFundSel.Text == "Headstart" && rbFundSel.Checked)
        //                                //{
        //                                //    CommonEntity FundEntity = propfundingSource.Find(u => u.Code.Equals("HS"));
        //                                //    if (FundEntity != null)
        //                                //    {
        //                                //        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
        //                                //        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
        //                                //        PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
        //                                //        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                //        Desc.FixedHeight = 15f;
        //                                //        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                //        Counttable.AddCell(Desc);

        //                                //        IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
        //                                //        OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("98"));
        //                                //        ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));

        //                                //        PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
        //                                //        IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                //        IECnt.FixedHeight = 15f;
        //                                //        IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                //        Counttable.AddCell(IECnt);

        //                                //        PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
        //                                //        OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                //        OICnt.FixedHeight = 15f;
        //                                //        OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                //        Counttable.AddCell(OICnt);

        //                                //        PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
        //                                //        ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                //        ICCnt.FixedHeight = 15f;
        //                                //        ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                //        Counttable.AddCell(ICCnt);
        //                                //    }
        //                                //}
        //                                //else
        //                                //{
        //                                    string[] Funds = strFundingCodes.Split(',');
        //                                    if (Funds.Length > 0)
        //                                    {
        //                                        for (int i = 0; i < Funds.Length; i++)
        //                                        {
        //                                            CommonEntity FundEntity = propfundingSource.Find(u => u.Code.Equals(Funds[i].ToString()));
        //                                            if (FundEntity != null)
        //                                            {
        //                                                List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
        //                                                List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
        //                                                List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

        //                                                PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
        //                                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                Desc.FixedHeight = 15f;
        //                                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(Desc);

        //                                            //IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
        //                                            //OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
        //                                            //ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
        //                                            //Pentity = SelRecords.FindAll(u =>  u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
        //                                            //CAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

        //                                            //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
        //                                            IEentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim()==FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim()==string.Empty)) && u.Classfication.Trim().Equals("99"));
        //                                            OIentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
        //                                            ICentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
        //                                            Pentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
        //                                            CAentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));

        //                                            PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
        //                                                IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                                IECnt.FixedHeight = 15f;
        //                                                IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(IECnt);

        //                                                PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
        //                                                OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                                OICnt.FixedHeight = 15f;
        //                                                OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(OICnt);

        //                                                PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
        //                                                ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                                ICCnt.FixedHeight = 15f;
        //                                                ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(ICCnt);

        //                                                PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
        //                                                PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                                PCnt.FixedHeight = 15f;
        //                                                PCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(PCnt);

        //                                                PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
        //                                                CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                                CACnt.FixedHeight = 15f;
        //                                                CACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                Counttable.AddCell(CACnt);
        //                                            }
        //                                        }
        //                                    }
        //                                //}

        //                            }
        //                            else if (rbFundAll.Checked)
        //                            {
        //                                foreach (CommonEntity FundEntity in propfundingSource)
        //                                {
        //                                    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
        //                                    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
        //                                    List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

        //                                    PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
        //                                    Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    Desc.FixedHeight = 15f;
        //                                    Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(Desc);

        //                                    //IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
        //                                    //OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98")|| u.Classfication.Trim().Equals("95")));
        //                                    //ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
        //                                    //Pentity = SelRecords.FindAll(u =>  u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
        //                                    //CAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

        //                                    //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
        //                                    IEentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
        //                                    OIentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
        //                                    ICentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
        //                                    Pentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
        //                                    CAentity = SelRecords.FindAll(u => (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));

        //                                    PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
        //                                    IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    IECnt.FixedHeight = 15f;
        //                                    IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(IECnt);

        //                                    PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
        //                                    OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    OICnt.FixedHeight = 15f;
        //                                    OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(OICnt);

        //                                    PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
        //                                    ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    ICCnt.FixedHeight = 15f;
        //                                    ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(ICCnt);

        //                                    PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
        //                                    PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    PCnt.FixedHeight = 15f;
        //                                    PCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(PCnt);

        //                                    PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
        //                                    CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                    CACnt.FixedHeight = 15f;
        //                                    CACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    Counttable.AddCell(CACnt);

        //                                }
        //                            }

        //                            PdfPCell Desc1 = new PdfPCell(new Phrase("No Fund Specified", TableFont));
        //                            Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                            Desc1.FixedHeight = 15f;
        //                            Desc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Counttable.AddCell(Desc1);

        //                            //List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
        //                            //List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
        //                            //List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
        //                            //List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
        //                            //List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));

        //                            //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
        //                            List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("99"));
        //                            List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
        //                            List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("00"));
        //                            List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("96"));
        //                            List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("97"));


        //                            PdfPCell NICnt = new PdfPCell(new Phrase(SEentity.Count.ToString(), TableFont));
        //                            NICnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            NICnt.FixedHeight = 15f;
        //                            NICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Counttable.AddCell(NICnt);

        //                            PdfPCell NOCnt = new PdfPCell(new Phrase(SOentity.Count.ToString(), TableFont));
        //                            NOCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            NOCnt.FixedHeight = 15f;
        //                            NOCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Counttable.AddCell(NOCnt);

        //                            PdfPCell NCCnt = new PdfPCell(new Phrase(SCentity.Count.ToString(), TableFont));
        //                            NCCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            NCCnt.FixedHeight = 15f;
        //                            NCCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            Counttable.AddCell(NCCnt);

        //                            PdfPCell NPCnt = new PdfPCell(new Phrase(SPentity.Count.ToString(), TableFont));
        //                            NPCnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            NPCnt.FixedHeight = 15f;
        //                            NPCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(NPCnt);

        //                            PdfPCell NCACnt = new PdfPCell(new Phrase(SCAentity.Count.ToString(), TableFont));
        //                            NCACnt.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            NCACnt.FixedHeight = 15f;
        //                            NCACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                            table.AddCell(NCACnt);

        //                        }

        //                        if (Counttable.Rows.Count > 0)
        //                        {
        //                            document.Add(Counttable);
        //                            Counttable.DeleteBodyRows();

        //                        }
        //                    }
        //                    else
        //                    {
        //                        document.Add(table);
        //                    }

        //                }
        //            }

        //        }
        //        catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        //        document.Close();
        //        fs.Close();
        //        fs.Dispose();

        //        AlertBox.Show("Report Generated Successfully");

        //        if (rbMailLbl.Checked && SelRecords_Labels.Count > 0)
        //        {
        //            On_SaveLabelsReport(PdfName);
        //        }

        //    }
        //}

        private void On_SaveLabelsReport(string PdfName)
        {
            PdfName = PdfName.Replace(".pdf", "");
            PdfName = PdfName + "_label.pdf";

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            cb = writer.DirectContent;

            if (SelRecords_Labels.Count > 0)
            {
                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 560f;
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                float[] widths = new float[] { 80f, 82f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPTable table1 = new PdfPTable(1);
                table1.TotalWidth = 195f;//170
                table1.WidthPercentage = 100;
                table1.LockedWidth = true;
                float[] widths1 = new float[] { 100f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table1.SetWidths(widths1);
                table1.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPTable table2 = new PdfPTable(1);
                table2.TotalWidth = 200f;//170
                table2.WidthPercentage = 100;
                table2.LockedWidth = true;
                float[] widths2 = new float[] { 100f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table2.SetWidths(widths2);
                table2.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPTable table3 = new PdfPTable(1);
                table3.TotalWidth = 170f;
                table3.WidthPercentage = 100;
                table3.LockedWidth = true;
                float[] widths3 = new float[] { 100f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                table2.SetWidths(widths3);
                table3.HorizontalAlignment = Element.ALIGN_LEFT;

                int i = 1; int Count = 1;
                foreach (HSSB2115Entity Entity in SelRecords_Labels)
                {
                    if (Entity.AppNo == "00161999")
                    {
                    }

                    PdfPCell L1 = new PdfPCell(new Phrase("App " + Entity.AppNo.Trim(), TableFont));
                    L1.HorizontalAlignment = Element.ALIGN_LEFT;
                    L1.FixedHeight = 10f;
                    L1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    switch (i)
                    {
                        case 1: table1.AddCell(L1); break;
                        case 2: table2.AddCell(L1); break;
                        case 3: table3.AddCell(L1); break;
                    }

                    if (rbParent.Checked)
                    {
                        if (Entity.G1_LName.Trim() == "No Guardian")
                            Entity.G1_LName = "";

                        if (!string.IsNullOrEmpty(Entity.G1_FName.Trim()) || !string.IsNullOrEmpty(Entity.G1_LName.Trim()))
                        {
                            string ParentName = string.Empty;
                            PdfPCell L2 = new PdfPCell(new Phrase(Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim(), TableFont));
                            L2.HorizontalAlignment = Element.ALIGN_LEFT;
                            L2.FixedHeight = 10f;
                            L2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            switch (i)
                            {
                                case 1: table1.AddCell(L2); break;
                                case 2: table2.AddCell(L2); break;
                                case 3: table3.AddCell(L2); break;
                            }
                        }

                        //if (!string.IsNullOrEmpty(Entity.G2_FName.Trim()) || !string.IsNullOrEmpty(Entity.G2_LName.Trim()))
                        //{
                        //    string ParentName = string.Empty;
                        //    PdfPCell L2 = new PdfPCell(new Phrase(Entity.G2_FName.Trim() + " " + Entity.G2_LName.Trim(), TableFont));
                        //    L2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //    L2.FixedHeight = 10f;
                        //    L2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //    switch (i)
                        //    {
                        //        case 1: table1.AddCell(L2); break;
                        //        case 2: table2.AddCell(L2); break;
                        //        case 3: table3.AddCell(L2); break;
                        //    }
                        //}

                    }
                    else
                    {
                        PdfPCell L2 = new PdfPCell(new Phrase("To the parents of:", TableFont));
                        L2.HorizontalAlignment = Element.ALIGN_LEFT;
                        L2.FixedHeight = 10f;
                        L2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        switch (i)
                        {
                            case 1: table1.AddCell(L2); break;
                            case 2: table2.AddCell(L2); break;
                            case 3: table3.AddCell(L2); break;
                        }

                        PdfPCell L3 = new PdfPCell(new Phrase(Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim(), TableFont));
                        L3.HorizontalAlignment = Element.ALIGN_LEFT;
                        L3.FixedHeight = 10f;
                        L3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        switch (i)
                        {
                            case 1: table1.AddCell(L3); break;
                            case 2: table2.AddCell(L3); break;
                            case 3: table3.AddCell(L3); break;
                        }

                    }

                    string HN = string.Empty; string street = string.Empty; string Suffix = string.Empty; string apt = string.Empty; string flr = string.Empty;
                    if (!string.IsNullOrEmpty(Entity.Hno.Trim())) HN = Entity.Hno.Trim() + " ";
                    if (!string.IsNullOrEmpty(Entity.Street.Trim())) street = Entity.Street.Trim() + " ";
                    if (!string.IsNullOrEmpty(Entity.Suffix.Trim())) Suffix = Entity.Suffix.Trim() + " ";
                    if (!string.IsNullOrEmpty(Entity.Apt.Trim())) apt = " APT " + Entity.Apt.Trim() + " ";
                    if (!string.IsNullOrEmpty(Entity.Flr.Trim())) flr = " FLR " + Entity.Flr.Trim() + " ";

                    string Address = HN + street + Suffix + apt + flr;

                    PdfPCell L4 = new PdfPCell(new Phrase(Address, TableFont));
                    L4.HorizontalAlignment = Element.ALIGN_LEFT;
                    L4.FixedHeight = 10f;
                    L4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    switch (i)
                    {
                        case 1: table1.AddCell(L4); break;
                        case 2: table2.AddCell(L4); break;
                        case 3: table3.AddCell(L4); break;
                    }

                    PdfPCell L5 = new PdfPCell(new Phrase(Entity.City.Trim() + " " + Entity.State.Trim() + " " + "00000".Substring(0, 5 - Entity.Zip.Length) + Entity.Zip.Trim(), TableFont));
                    L5.HorizontalAlignment = Element.ALIGN_LEFT;
                    L5.FixedHeight = 10f;
                    L5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    switch (i)
                    {
                        case 1: table1.AddCell(L5); break;
                        case 2: table2.AddCell(L5); break;
                        case 3: table3.AddCell(L5); break;
                    }

                    if (rbParent.Checked)
                    {
                        if ((string.IsNullOrEmpty(Entity.G1_FName.Trim()) || string.IsNullOrEmpty(Entity.G1_LName.Trim())))
                        {
                            PdfPCell L6 = new PdfPCell(new Phrase("", TableFont));
                            L6.HorizontalAlignment = Element.ALIGN_LEFT;
                            L6.FixedHeight = 10f;
                            //if (rbChild.Checked) L6.FixedHeight = 26f;
                            //else L6.FixedHeight = 23f;
                            L6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            switch (i)
                            {
                                case 1: table1.AddCell(L6); break;
                                case 2: table2.AddCell(L6); break;
                                case 3: table3.AddCell(L6); break;
                            }

                            PdfPCell L7 = new PdfPCell(new Phrase("", TableFont));
                            L7.HorizontalAlignment = Element.ALIGN_LEFT;
                            L7.FixedHeight = 10f;
                            //if (rbChild.Checked) L6.FixedHeight = 26f;
                            //else L6.FixedHeight = 23f;
                            L7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            switch (i)
                            {
                                case 1: table1.AddCell(L7); break;
                                case 2: table2.AddCell(L7); break;
                                case 3: table3.AddCell(L7); break;
                            }
                        }

                        //if ((string.IsNullOrEmpty(Entity.G2_FName.Trim()) || string.IsNullOrEmpty(Entity.G2_LName.Trim())))
                        //{
                        //    PdfPCell L6 = new PdfPCell(new Phrase("", TableFont));
                        //    L6.HorizontalAlignment = Element.ALIGN_LEFT;
                        //    L6.FixedHeight = 10f;
                        //    //if (rbChild.Checked) L6.FixedHeight = 26f;
                        //    L6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //    switch (i)
                        //    {
                        //        case 1: table1.AddCell(L6); break;
                        //        case 2: table2.AddCell(L6); break;
                        //        case 3: table3.AddCell(L6); break;
                        //    }
                        //}
                    }

                    //if (rbChild.Checked)
                    //{
                    //    for (int j = 0; j < 7; j++)
                    //    {
                    //        PdfPCell L6 = new PdfPCell(new Phrase("", TableFont));
                    //        L6.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        //if (rbChild.Checked) L6.FixedHeight = 26f;
                    //        //else L6.FixedHeight = 23f;
                    //        L6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //        switch (i)
                    //        {
                    //            case 1: table1.AddCell(L6); break;
                    //            case 2: table2.AddCell(L6); break;
                    //            case 3: table3.AddCell(L6); break;
                    //        }
                    //    }
                    //}
                    if (rbParent.Checked || rbChild.Checked)
                    {
                        if (Count < 28)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                PdfPCell L6 = new PdfPCell(new Phrase("", TableFont));
                                L6.HorizontalAlignment = Element.ALIGN_LEFT;
                                //if (rbChild.Checked) L6.FixedHeight = 26f;
                                //else L6.FixedHeight = 23f;
                                L6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                switch (i)
                                {
                                    case 1: table1.AddCell(L6); break;
                                    case 2: table2.AddCell(L6); break;
                                    case 3: table3.AddCell(L6); break;
                                }
                            }
                        }
                    }



                    if (i == 3) i = 0;

                    if ((rbChild.Checked && Count == 30) || rbParent.Checked && Count == 30)
                    {
                        PdfPCell T1 = new PdfPCell(table1);
                        T1.HorizontalAlignment = Element.ALIGN_LEFT;
                        T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(T1);

                        PdfPCell T2 = new PdfPCell(table2);
                        T2.HorizontalAlignment = Element.ALIGN_LEFT;
                        T2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(T2);

                        PdfPCell T3 = new PdfPCell(table3);
                        T3.HorizontalAlignment = Element.ALIGN_LEFT;
                        T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(T3);

                        document.Add(table);
                        table1.DeleteBodyRows(); table2.DeleteBodyRows(); table3.DeleteBodyRows();
                        table.DeleteBodyRows();
                        document.NewPage();

                        Count = 0;

                    }

                    i++; Count++;

                }

                if (table1.Rows.Count > 0 && table2.Rows.Count > 0 && table3.Rows.Count > 0)
                {
                    PdfPCell T1 = new PdfPCell(table1);
                    T1.HorizontalAlignment = Element.ALIGN_LEFT;
                    T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(T1);

                    PdfPCell T2 = new PdfPCell(table2);
                    T2.HorizontalAlignment = Element.ALIGN_LEFT;
                    T2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(T2);

                    PdfPCell T3 = new PdfPCell(table3);
                    T3.HorizontalAlignment = Element.ALIGN_LEFT;
                    T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(T3);

                    //table.AddCell(table2);
                    //table.AddCell(table3);
                }

                document.Add(table);

            }
            document.Close();
            fs.Close();
            fs.Dispose();
        }

        private void PrintMainHeader(PdfPTable table)
        {
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);

            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
            iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));

            #region Cell Color Define
            BaseColor DarkBlue = new BaseColor(26, 71, 119);
            BaseColor SecondBlue = new BaseColor(184, 217, 255);
            BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);
            #endregion

            string[] HeaderSeq4 = { "Rank", "App.", "Name", "", "Address", "Parent / Guardian" };
            for (int i = 0; i < HeaderSeq4.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cell.BackgroundColor = SecondBlue;
                table.AddCell(cell);
            }
        }

        private void PrintCountHeader(PdfPTable Table, string sitenum,string sitename)
        {
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);

            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
            iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));

            #region Cell Color Define
            BaseColor DarkBlue = new BaseColor(26, 71, 119);
            BaseColor SecondBlue = new BaseColor(184, 217, 255);
            BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);
            #endregion

            PdfPCell Header = new PdfPCell(new Phrase("All applicants for site " + sitenum + "  " + sitename, HeaderTextfnt));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 6;
            //Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Header.BackgroundColor = DarkBlue;
            Table.AddCell(Header);

            string[] Header12 = { "Site Totals:", "Income Eligible", "Over Income", "In Certify", "101% - 130%", "Categorical" };
            for (int i = 0; i < Header12.Length; ++i)
            {
                PdfPCell Cnt = new PdfPCell(new Phrase(Header12[i], TblFontBold));
                Cnt.HorizontalAlignment = Element.ALIGN_CENTER;
                Cnt.FixedHeight = 15f;
                Cnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cnt.BackgroundColor = SecondBlue;
                Table.AddCell(Cnt);
            }
        }

        private void PrintCountTable(PdfPTable table, string sitenum)
        {
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);

            if (propfundingSource.Count > 0)
            {
                if (rbDayCare.Checked || (rbFundSel.Checked))
                {
                    //if (rbFundSel.Text == "Headstart" && rbFundSel.Checked)
                    //{
                    //    CommonEntity FundEntity = propfundingSource.Find(u => u.Code.Equals("HS"));
                    //    if (FundEntity != null)
                    //    {
                    //        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                    //        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
                    //        PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                    //        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        Desc.FixedHeight = 15f;
                    //        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //        table.AddCell(Desc);

                    //        IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                    //        OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("98"));
                    //        ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));

                    //        PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                    //        IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        IECnt.FixedHeight = 15f;
                    //        IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //        table.AddCell(IECnt);

                    //        PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                    //        OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        OICnt.FixedHeight = 15f;
                    //        OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //        table.AddCell(OICnt);

                    //        PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                    //        ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        ICCnt.FixedHeight = 15f;
                    //        ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //        table.AddCell(ICCnt);
                    //    }
                    //}
                    //else
                    //{
                        string[] Funds = strFundingCodes.Split(',');
                        if (Funds.Length > 0)
                        {
                            for (int i = 0; i < Funds.Length; i++)
                            {
                                CommonEntity FundEntity = propfundingSource.Find(u => u.Code.Equals(Funds[i].ToString()));
                                if (FundEntity != null)
                                {
                                    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();
                                    PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                                    Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Desc.FixedHeight = 15f;
                                    Desc.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(Desc);

                                //IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                //OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                //ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                //Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                //CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

                                //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                                IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
                                OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
                                Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
                                CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));


                                PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                                    IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    IECnt.FixedHeight = 15f;
                                    IECnt.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(IECnt);

                                    PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                                    OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    OICnt.FixedHeight = 15f;
                                    OICnt.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(OICnt);

                                    PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                                    ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    ICCnt.FixedHeight = 15f;
                                    ICCnt.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(ICCnt);

                                    PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
                                    PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PCnt.FixedHeight = 15f;
                                    PCnt.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(PCnt);

                                    PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
                                    CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                                    CACnt.FixedHeight = 15f;
                                    CACnt.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(CACnt);
                                }
                            }
                        //}
                    }
                    
                    //foreach (CommonEntity FundEntity in propfundingSource)
                    //{
                        
                    //    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                    //    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
                    //    PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                    //    Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    Desc.FixedHeight = 15f;
                    //    Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    table.AddCell(Desc);

                    //    IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                    //    OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("98"));
                    //    ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));

                    //    PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                    //    IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    IECnt.FixedHeight = 15f;
                    //    IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    table.AddCell(IECnt);

                    //    PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                    //    OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    OICnt.FixedHeight = 15f;
                    //    OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    table.AddCell(OICnt);

                    //    PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                    //    ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    ICCnt.FixedHeight = 15f;
                    //    ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    table.AddCell(ICCnt);
                    //}
                }
                else if(rbFundAll.Checked)
                {
                    foreach (CommonEntity FundEntity in propfundingSource)
                    {
                        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                        List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();
                        PdfPCell Desc = new PdfPCell(new Phrase(FundEntity.Desc.Trim(), TableFont));
                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        Desc.FixedHeight = 15f;
                        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Desc);

                        //IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                        //OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                        //ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                        //Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                        //CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

                        //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                        if (rbWaitlist.Checked || rbBoth.Checked)
                        {
                            IEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
                            OIentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                            ICentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
                            Pentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
                            CAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));

                        }
                        else
                        {
                            IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("99"));
                            OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                            ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("00"));
                            Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("96"));
                            CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.EnrlFundHie.Trim() == FundEntity.Code.Trim() || (u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.EnrlFundHie.Trim() == string.Empty)) && u.Classfication.Trim().Equals("97"));
                        }

                        PdfPCell IECnt = new PdfPCell(new Phrase(IEentity.Count.ToString(), TableFont));
                        IECnt.HorizontalAlignment = Element.ALIGN_CENTER;
                        IECnt.FixedHeight = 15f;
                        IECnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(IECnt);

                        PdfPCell OICnt = new PdfPCell(new Phrase(OIentity.Count.ToString(), TableFont));
                        OICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                        OICnt.FixedHeight = 15f;
                        OICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(OICnt);

                        PdfPCell ICCnt = new PdfPCell(new Phrase(ICentity.Count.ToString(), TableFont));
                        ICCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                        ICCnt.FixedHeight = 15f;
                        ICCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(ICCnt);

                        PdfPCell PCnt = new PdfPCell(new Phrase(Pentity.Count.ToString(), TableFont));
                        PCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                        PCnt.FixedHeight = 15f;
                        PCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(PCnt);

                        PdfPCell CACnt = new PdfPCell(new Phrase(CAentity.Count.ToString(), TableFont));
                        CACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                        CACnt.FixedHeight = 15f;
                        CACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(CACnt);
                    }
                }

                PdfPCell Desc1 = new PdfPCell(new Phrase("No Fund Specified", TableFont));
                Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
                Desc1.FixedHeight = 15f;
                Desc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(Desc1);

                //List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                //List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                //List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                //List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                //List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));

                List<HSSB2115Entity> SEentity = new List<HSSB2115Entity>();
                List<HSSB2115Entity> SOentity = new List<HSSB2115Entity>();
                List<HSSB2115Entity> SCentity = new List<HSSB2115Entity>();
                List<HSSB2115Entity> SPentity = new List<HSSB2115Entity>();
                List<HSSB2115Entity> SCAentity = new List<HSSB2115Entity>();

                //We includued the Enroll Fund in this logic as per MVCAA document on 06/18/2021
                if (rbWaitlist.Checked || rbBoth.Checked)
                {
                    SEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("99"));
                    SOentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                    SCentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("00"));
                    SPentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("96"));
                    SCAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("97"));

                }
                else
                {
                    SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("99"));
                    SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                    SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("00"));
                    SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("96"));
                    SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(sitenum.Trim()) && (u.CHLDMST_FUND.Trim().Equals("") && u.EnrlFundHie.Trim().Equals("")) && u.Classfication.Trim().Equals("97"));

                }


                PdfPCell NICnt = new PdfPCell(new Phrase(SEentity.Count.ToString(), TableFont));
                NICnt.HorizontalAlignment = Element.ALIGN_CENTER;
                NICnt.FixedHeight = 15f;
                NICnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(NICnt);

                PdfPCell NOCnt = new PdfPCell(new Phrase(SOentity.Count.ToString(), TableFont));
                NOCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                NOCnt.FixedHeight = 15f;
                NOCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(NOCnt);

                PdfPCell NCCnt = new PdfPCell(new Phrase(SCentity.Count.ToString(), TableFont));
                NCCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                NCCnt.FixedHeight = 15f;
                NCCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(NCCnt);

                PdfPCell NPCnt = new PdfPCell(new Phrase(SPentity.Count.ToString(), TableFont));
                NPCnt.HorizontalAlignment = Element.ALIGN_CENTER;
                NPCnt.FixedHeight = 15f;
                NPCnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(NPCnt);

                PdfPCell NCACnt = new PdfPCell(new Phrase(SCAentity.Count.ToString(), TableFont));
                NCACnt.HorizontalAlignment = Element.ALIGN_CENTER;
                NCACnt.FixedHeight = 15f;
                NCACnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(NCACnt);
            }
        }
        
        string Income_Stat = "A";
        private void rbIncAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOverInc.Checked == true)
                Income_Stat = "O";
            else if (rbIncAll.Checked == true)
                Income_Stat = "A";
            else if (rdoIncElig.Checked == true)
                Income_Stat = "I";
            else if (rdoIncCerti.Checked == true)
                Income_Stat = "C";
            else if (rb101.Checked == true)
                Income_Stat = "1";
            else if (rbCategorical.Checked == true)
                Income_Stat = "B";
        }


        private void PrintHeaderPage(Document document, PdfWriter writer)
        {

            /*BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);*/

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

            PdfPTable Headertable = new PdfPTable(2);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 25f, 80f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

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
                Headertable.AddCell(cellLogo);
            }

            PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblParamsHeaderFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            /*PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            row2.HorizontalAlignment = Element.ALIGN_LEFT;
            //row2.Colspan = 2;
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row2);

            PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            //row2.Colspan = 2;
            row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row21);*/

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblHeaderTitleFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.PaddingBottom = 5;
            row3.MinimumHeight = 6;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(row3);

            /*string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = "Agency : " + Agency;
            if (Depart != "**") Dept = "Dept : " + Depart;
            if (Program != "**") Prg = "Program : " + Program;
            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Agy + "  " + Dept + "  " + Prg + "  " + Header_year, TableFont));Txt_HieDesc
            PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim()+ "     " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Depart != "**") Dept = /*"Dept : " + */Depart;
            if (Program != "**") Prg = /*"Program : " +*/ Program;
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Hierarchy.PaddingBottom = 5;
            Headertable.AddCell(Hierarchy);

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            PdfPCell R1 = new PdfPCell(new Phrase("  " + "Report Sequence" /*+ (rdoSiteName.Checked == true ? rdoSiteName.Text : rdoRank.Text)*/, TableFont));
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase((rdoSiteName.Checked == true ? rdoSiteName.Text : rdoRank.Text), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Ranking Category" /*+ ((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Text.ToString()*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Text.ToString(), TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            string Casenote = string.Empty;
            if (chkbCasenotes.Checked == true) Casenote = "Yes";
            else Casenote = "No";

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Include Case Notes", TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase(Casenote, TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            string Fund = string.Empty;
            if (rbFundAll.Checked == true)
                Fund = /*lblFundSource.Text.Trim() + " : " +*/ rbFundAll.Text.Trim() + " Funds";
            else if (rbFundSel.Text == "Selected Funding" && rbFundSel.Checked)
                Fund = "Selected Funds: " + strFundingCodes;
            else if (rbDayCare.Checked)
                Fund = "Secondary Funds: " + strFundingCodes;
            else
                Fund = "Primary Funds: " + strFundingCodes;

            PdfPCell R3 = new PdfPCell(new Phrase("  " + lblFundSource.Text.Trim(), TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Fund, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Age" /*From " + txtFrom.Text + " to " + txtTo.Text + ",  Base on " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text)*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase("From: " + txtFrom.Text + "   To: " + txtTo.Text + ", Base on " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            string Site = string.Empty;
            if (rdoAllSite.Checked == true) Site = "For All Sites";
            else
            {
                string Selsites = string.Empty;
                if (Sel_REFS_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Sel_REFS_List)
                    {
                        Selsites += Entity.SiteNUMBER+"   ";// +"/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + "  ";
                    }
                }
                Site = rdoMultipleSites.Text.Trim() + " ( " + Selsites + " )";
            }
            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Site", TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            Headertable.AddCell(R6);

            PdfPCell R66 = new PdfPCell(new Phrase(Site, TableFont));
            R66.HorizontalAlignment = Element.ALIGN_LEFT;
            R66.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R66.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R66.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R66.PaddingBottom = 5;
            Headertable.AddCell(R66);

            string Income = rdoIncElig.Text;
            if (rdoIncCerti.Checked == true) Income = rdoIncCerti.Text;
            else if (rbOverInc.Checked == true) Income = rbOverInc.Text;
            else if (rbIncAll.Checked == true) Income = rbIncAll.Text;
            else if (rb101.Checked == true) Income = rb101.Text;
            else if (rbCategorical.Checked == true) Income = rbCategorical.Text;

            PdfPCell R7 = new PdfPCell(new Phrase("  " + "Income Status" /*+ Income*/, TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            Headertable.AddCell(R7);

            PdfPCell R77 = new PdfPCell(new Phrase(Income, TableFont));
            R77.HorizontalAlignment = Element.ALIGN_LEFT;
            R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R77.PaddingBottom = 5;
            Headertable.AddCell(R77);

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Categorical" /*+ ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Text.ToString()*/, TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            Headertable.AddCell(R8);

            PdfPCell R88 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Text.ToString(), TableFont));
            R88.HorizontalAlignment = Element.ALIGN_LEFT;
            R88.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R88.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R88.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R88.PaddingBottom = 5;
            Headertable.AddCell(R88);

            if (rbDayCare.Checked == false)
            {
                string Include_Clients = rdoIncElig.Text;
                if (rbBoth.Checked == true) Include_Clients = rbBoth.Text;
                else if (rbIntakeCmpltd.Checked == true) Include_Clients = rbIntakeCmpltd.Text;
                else if (rbWaitlist.Checked == true) Include_Clients = rbWaitlist.Text;

                PdfPCell R12 = new PdfPCell(new Phrase("  " + "Include Clients" /*+ Include_Clients*/, TableFont));
                R12.HorizontalAlignment = Element.ALIGN_LEFT;
                R12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R12.PaddingBottom = 5;
                Headertable.AddCell(R12);

                PdfPCell R121 = new PdfPCell(new Phrase(Include_Clients, TableFont));
                R121.HorizontalAlignment = Element.ALIGN_LEFT;
                R121.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R121.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R121.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R121.PaddingBottom = 5;
                Headertable.AddCell(R121);
            }

                PdfPCell R9 = new PdfPCell(new Phrase("  " + "Labels" /*+ (rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text)*/, TableFont));
                R9.HorizontalAlignment = Element.ALIGN_LEFT;
                R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R9.PaddingBottom = 5;
                Headertable.AddCell(R9);

                PdfPCell R99 = new PdfPCell(new Phrase((rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text), TableFont));
                R99.HorizontalAlignment = Element.ALIGN_LEFT;
                R99.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R99.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R99.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R99.PaddingBottom = 5;
                Headertable.AddCell(R99);

                if (rbMailLbl.Checked)
                {
                    PdfPCell R08 = new PdfPCell(new Phrase("  " + lblPrintlbl.Text.Trim() /*+ " : " + (rbChild.Checked == true ? rbChild.Text : rbParent.Text)*/, TableFont));
                    R08.HorizontalAlignment = Element.ALIGN_LEFT;
                    R08.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R08.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R08.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    R08.PaddingBottom = 5;
                    Headertable.AddCell(R08);

                    PdfPCell R088 = new PdfPCell(new Phrase((rbChild.Checked == true ? rbChild.Text : rbParent.Text), TableFont));
                    R088.HorizontalAlignment = Element.ALIGN_LEFT;
                    R088.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R088.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R088.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R088.PaddingBottom = 5;
                    Headertable.AddCell(R088);
                }

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

            //cb.BeginText();

            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            ////cb.SetRGBColorFill(00, 00, 00);      //ss
            //cb.ShowTextAligned(100, "Program Name: ", 30, 725, 0);
            //cb.ShowTextAligned(100, Privileges.Program + " - " + Privileges.PrivilegeName, 110, 725, 0);
            //cb.ShowTextAligned(100, "Run By : " + Privileges.UserID, 30, 705, 0);
            //cb.ShowTextAligned(100, "Module Desc :" + GetModuleDesc(), 30, 685, 0);
            //cb.ShowTextAligned(100, "Started : " + DateTime.Now.ToString(), 30, 665, 0);
            //cb.ShowTextAligned(100, "Report  Selection Criterion", 40, 635, 0);
            //string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            //cb.ShowTextAligned(100, "Hierarchy : " + Agency + "-" + Depart + "-" + Program + "   " + Header_year, 120, 610, 0);
            //string Report = "Site and Name";
            //if (rdoSiteName.Checked.Equals(true)) Report = "Site and Name"; else Report = "Rank";
            //cb.ShowTextAligned(100, "Report Sequence :" + Report, 120, 590, 0);
            ////cb.ShowTextAligned(100, "Report Type :" + ((Captain.Common.Utilities.ListItem)cmbReport.SelectedItem).Text.ToString(), 120, 590, 0);
            //cb.ShowTextAligned(100, "Ranking Category : " + ((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Text.ToString(), 120, 570, 0);
            //string Fund = "All Funds";
            //if (rbFundAll.Checked == false) Fund = "Selected Funds"; else Fund = "All Funds";
            //cb.ShowTextAligned(100, "Funding Source : " + Fund, 120, 550, 0);
            //cb.ShowTextAligned(100, "Age   From : "+txtFrom.Text.ToString()+"       To :  "+ txtTo.Text.Trim(), 120, 530, 0);
            //cb.ShowTextAligned(100, "Age   From : " + txtFrom.Text.ToString() + "       To :  " + txtTo.Text.Trim(), 120, 530, 0);
            //string Site = "All Sites";
            //if (rdoAllSite.Checked == false) Site = "Multiple Sites"; else Site = "All Sites";
            //cb.ShowTextAligned(100, "Site : " + Site, 120, 510, 0);
            //if(chkbCasenotes.Checked==true)
            //    cb.ShowTextAligned(100, chkbCasenotes.Text.Trim(), 350, 510, 0);
            //string Income = rdoIncElig.Text;
            //if (rdoIncCerti.Checked == true) Income = rdoIncCerti.Text; 
            //else if(rbOverInc.Checked==true) Income = rbOverInc.Text;
            //else if (rbIncAll.Checked == true) Income = rbIncAll.Text;
            //cb.ShowTextAligned(100, "Income Status : " + Income, 120, 490, 0);

            //cb.SetFontAndSize(bfTimes, 12);
            //Y_Pos = 480;    // Y =  350

            //cb.EndText();

            //cb.SetLineWidth(0.7f);
            //cb.MoveTo(30f, 638f);
            //cb.LineTo(40f, 638f);

            //cb.LineTo(30f, 638f);
            //cb.LineTo(30f, 470f);

            //cb.LineTo(30f, 470f);
            //cb.LineTo(470f, 470f);

            //cb.LineTo(470f, 638f);
            //cb.LineTo(470f, 470f);

            //cb.MoveTo(170f, 638f);
            //cb.LineTo(470f, 638f);

            //cb.Stroke();
        }


        private string GetModuleDesc()
        {
            string ModuleDesc = null;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            System.Data.DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["APPL_CODE"].ToString() == Privileges.ModuleCode)
                {
                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                }
            }
            return ModuleDesc;
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            Sel_REFS_List.Clear();
            _errorProvider.SetError(rdoMultipleSites, null);
        }

        private void HSSB0115_WaitingList_Report_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }


        //private void MSWordFormats(object sender, FormClosedEventArgs e)
        //{

        //    PdfListForm form = sender as PdfListForm;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        string PdfName = "Pdf File";
        //        PdfName = form.GetFileName();
        //        //PdfName = strFolderPath + PdfName;
        //        PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //        try
        //        {
        //            if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //            { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonFunctions.MessageBoxDisplay("Error");
        //        }


        //        try
        //        {
        //            string Tmpstr = PdfName + ".doc";
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
        //            PdfName += ".doc";

        //        if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
        //        string Year = string.Empty;
        //        if (CmbYear.Visible == true)
        //            Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

        //        CaseSiteEntity SearchEntity = new CaseSiteEntity(true);
        //        SearchEntity.SiteROOM = "0000"; SearchEntity.SiteAGENCY = Agency;
        //        propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(SearchEntity, "Browse");

        //        try
        //        {

        //            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
        //            winword.Visible = false;

        //            object missing = System.Reflection.Missing.Value;

        //            //Create a new document
        //            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

        //            //Add header into the document
        //            foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
        //            {
        //                //Get the header range and add the header details.
        //                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
        //                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
        //                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
        //                headerRange.Font.Size = 10;
        //                headerRange.Text = "Waiting List Report";

        //            }

        //            //Add the footers into the document
        //            foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
        //            {
        //                //Get the footer range and add the footer details.
        //                Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
        //                footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
        //                footerRange.Font.Size = 10;
        //                footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                footerRange.Text = "Waiting List Report";

        //            }

        //            //Add paragraph with Heading 1 style
        //            //Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
        //            //object styleHeading1 = "Heading 1";
        //            //para1.Range.set_Style(ref styleHeading1);
        //            //para1.Range.Text = "Para 1 text";
        //            ////para1.Range.InsertParagraphAfter();

        //            object oEndOfDoc = "\\endofdoc";

        //            Microsoft.Office.Interop.Word.Range wrdRng = document.Bookmarks.get_Item(ref oEndOfDoc).Range;

        //            Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(wrdRng, 1, 6, ref missing, ref missing);

        //            int i = 1;

        //            document.PageSetup.LeftMargin = 20.0f;
        //            document.PageSetup.RightMargin = 20.0f;
        //            //firstTable.Borders.Enable = 1;

        //            //firstTable.Cell(i, 5).Width = 30;

        //            //int j = 0;
        //            //Microsoft.Office.Interop.Word.Range wrdRng = document.Bookmarks.get_Item(ref oEndOfDoc).Range;


        //            bool First = true;
        //            foreach (HSSB2115Entity Entity in SelRecords)
        //            {
        //                //if (i <= 65)
        //                //{
        //                    if (First)
        //                    {
        //                        firstTable.Rows[i].Range.Font.Name = "Times New Roman";
        //                        firstTable.Rows[i].Range.Font.Bold = 1;
        //                        firstTable.Rows[i].Range.Font.Italic = 1;
        //                        firstTable.Rows[i].Range.Font.Size = 14;

        //                        //firstTable.Columns[1].Cells.PreferredWidth = 50;
        //                        //firstTable.Columns[2].Cells.PreferredWidth = 70;
        //                        //firstTable.Columns[3].Cells.PreferredWidth = 110;
        //                        //firstTable.Columns[4].Cells.PreferredWidth = 110;
        //                        //firstTable.Columns[5].Cells.PreferredWidth = 120;
        //                        //firstTable.Columns[6].Cells.PreferredWidth = 120;

        //                        firstTable.Rows[1].Cells[1].PreferredWidth = 50;
        //                        firstTable.Rows[1].Cells[2].PreferredWidth = 70;
        //                        firstTable.Rows[1].Cells[3].PreferredWidth = 110;
        //                        firstTable.Rows[1].Cells[4].PreferredWidth = 110;
        //                        firstTable.Rows[1].Cells[5].PreferredWidth = 120;
        //                        firstTable.Rows[1].Cells[6].PreferredWidth = 120;

        //                        firstTable.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                        firstTable.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                        firstTable.Cell(1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                        firstTable.Cell(1, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                        firstTable.Cell(1, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                        firstTable.Cell(1, 6).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

        //                        firstTable.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        //                        firstTable.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        //                        firstTable.Cell(1, 3).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        //                        firstTable.Cell(1, 4).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        //                        firstTable.Cell(1, 5).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        //                        firstTable.Cell(1, 6).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

        //                        firstTable.Cell(i, 1).Range.Text = "Rank";
        //                        firstTable.Cell(i, 2).Range.Text = "App";
        //                        firstTable.Cell(i, 3).Range.Text = "Name";
        //                        firstTable.Cell(i, 4).Range.Text = string.Empty;
        //                        firstTable.Cell(i, 5).Range.Text = "Address";
        //                        firstTable.Cell(i, 6).Range.Text = "Parent/Guardian";

        //                        First = false;
        //                    }
        //                    i++;
        //                    firstTable.Rows.Add(missing);

        //                    firstTable.Rows[i].Range.Font.Name = "Times New Roman";
        //                    firstTable.Rows[i].Range.Font.Bold = 0;
        //                    firstTable.Rows[i].Range.Font.Italic = 0;
        //                    firstTable.Rows[i].Range.Font.Size = 10;

        //                    firstTable.Cell(i, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //                    firstTable.Cell(i, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //                    firstTable.Cell(i, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //                    firstTable.Cell(i, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //                    firstTable.Cell(i, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //                    firstTable.Cell(i, 6).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

        //                    firstTable.Cell(i, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    firstTable.Cell(i, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    firstTable.Cell(i, 3).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    firstTable.Cell(i, 4).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    firstTable.Cell(i, 5).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    firstTable.Cell(i, 6).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 3).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 3).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 3).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 3).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 4).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 4).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 4).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 4).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 5).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 5).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 5).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 5).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    //firstTable.Cell(i, 6).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 6).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 6).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleNone;
        //                    //firstTable.Cell(i, 6).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone;

        //                    firstTable.Cell(i, 1).Range.Text = Entity.Rank.Trim();
        //                    firstTable.Cell(i, 2).Range.Text = Entity.AppNo.Trim();
        //                    firstTable.Cell(i, 3).Range.Text = Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim();
        //                    firstTable.Cell(i, 4).Range.Text = "Age  " + Entity.Age.Trim() + " yrs  " + Entity.Months.Trim() + " mos";

        //                    string Apt = string.Empty; string Floor = string.Empty;
        //                    if (!string.IsNullOrEmpty(Entity.Apt.Trim()))
        //                        Apt = "  Apt  " + Entity.Apt.Trim();
        //                    if (!string.IsNullOrEmpty(Entity.Flr.Trim()))
        //                        Apt = "  Flr  " + Entity.Flr.Trim();

        //                    firstTable.Cell(i, 5).Range.Text = Entity.Hno.Trim() + "   " + Entity.Street.Trim() + " " + Entity.Suffix.Trim() + Apt + Floor;
        //                    firstTable.Cell(i, 6).Range.Text = Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim();

        //                    i++;
        //                    firstTable.Rows.Add(missing);

        //                    firstTable.Cell(i, 1).Range.Text = string.Empty;

        //                    string Sex = string.Empty; string Class = string.Empty;
        //                    if (Entity.Sex.Trim() == "M") Sex = "MALE"; else Sex = "FEMALE";
        //                    if (Entity.ClassPrefer.Trim() == "A") Class = "AM SLOT";
        //                    else if (Entity.ClassPrefer.Trim() == "P") Class = "PM SLOT";
        //                    else if (Entity.Classfication.Trim() == "E") Class = "EXTD SLOT"; else if (Entity.ClassPrefer.Trim() == "F") Class = "FULL SLOT";

        //                    if (Entity.ClassPrefer.Trim() == "N")
        //                    {


        //                        //firstTable.Rows[i].Cells[2].Merge(firstTable.Rows[i].Cells[3]);
        //                        //firstTable.Cell(i, 2).Merge(firstTable.Cell(i, 4));
        //                        //firstTable.Rows[i].Cells[2].PreferredWidth = 290;
        //                        //firstTable.Rows[i].Cells[3].PreferredWidth = 0;
        //                        //firstTable.Rows[i].Cells[4].PreferredWidth = 0;
        //                        firstTable.Cell(i, 2).Range.Text = "PREFER" + "       " + Sex.Trim();


        //                    }
        //                    else
        //                    {
        //                        //firstTable.Cell(i, 2).Merge(firstTable.Cell(i, 4));
        //                        //firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 3;
        //                        //firstTable.Rows[i].Cells[2].Merge(firstTable.Rows[i].Cells[3]);
        //                        firstTable.Cell(i, 2).Range.Text = "PREFER    " + Class + " SLOT  " + Sex.Trim();
        //                    }
        //                    //firstTable.Cell(i, 2).Range.Text = Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim();
        //                    firstTable.Cell(i, 4).Range.Text = "";
        //                    firstTable.Cell(i, 5).Range.Text = Entity.City.Trim() + " " + Entity.State.Trim() + "  " + Entity.Zip.Trim();

        //                    MaskedTextBox mskWPhn = new MaskedTextBox();
        //                    mskWPhn.Mask = "(999) 000-0000";
        //                    if (!string.IsNullOrEmpty(Entity.Empl_Phone.Trim()))
        //                        mskWPhn.Text = Entity.Phone.Trim();

        //                    firstTable.Cell(i, 6).Range.Text = "Work :  " + mskWPhn.Text.Trim();
        //                    //firstTable.Cell(i, 5).Range.Text = Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim();

        //                    i++;
        //                    firstTable.Rows.Add(missing);

        //                    firstTable.Cell(i, 1).Range.Text = string.Empty;

        //                    MaskedTextBox mskPhn = new MaskedTextBox();
        //                    mskPhn.Mask = "(999) 000-0000";
        //                    if (!string.IsNullOrEmpty(Entity.Phone.Trim()))
        //                        mskPhn.Text = Entity.Phone.Trim();

        //                    firstTable.Cell(i, 2).Range.Text = "DOB: " + LookupDataAccess.Getdate(Entity.DOB.Trim()) + "    Phone: " + mskPhn.Text.Trim();
        //                    firstTable.Cell(i, 2).Range.ParagraphFormat.SpaceAfter = 2;

        //                    string Income_Desc = string.Empty; string Meals = string.Empty;
        //                    if (Entity.Classfication.Trim() == "98" || Entity.Classfication.Trim() == "95") Income_Desc = "Over Inc.";
        //                    else if (Entity.Classfication.Trim() == "99") Income_Desc = "Eligible";
        //                    else if (Entity.Classfication.Trim() == "96") Income_Desc = "101% - 130%";
        //                    else if (Entity.Classfication.Trim() == "97") Income_Desc = "Categorical";
        //                    else Income_Desc = "In Certify";
        //                    string Meals_Type = string.Empty;
        //                    if (Entity.MealElig == "1") Meals_Type = "Free Meals";
        //                    else if (Entity.MealElig == "2") Meals_Type = "Reduced Meals";
        //                    else if (Entity.MealElig == "3") Meals_Type = "Paid Meals";

        //                    if (rdoSiteName.Checked == true)
        //                    {
        //                        firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 2;
        //                        firstTable.Cell(i, 4).Range.Text = "      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type;
        //                    }
        //                    else
        //                    {
        //                        firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 2;
        //                        firstTable.Cell(i, 4).Range.Text = Entity.Site.Trim() + "      " + Income_Desc + "  " + LookupDataAccess.Getdate(Entity.Elig_Date.Trim()) + "      " + Meals_Type;
        //                    }

        //                    string Fund = string.Empty;
        //                    if (!string.IsNullOrEmpty(Entity.CHLDMST_FUND.Trim()))
        //                    {
        //                        foreach (CommonEntity drFund in propfundingSource)
        //                        {
        //                            if (Entity.CHLDMST_FUND.Trim() == drFund.Code.ToString().Trim())
        //                            {
        //                                Fund = drFund.Desc.ToString().Trim();
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    firstTable.Cell(i, 6).Range.Text = "Expected Fund :  " + Fund;

        //                    i++;
        //                    firstTable.Rows.Add(missing);

        //                    firstTable.Cell(i, 1).Range.Text = string.Empty;
        //                    firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 3;
        //                    firstTable.Cell(i, 2).Range.Text = "Household Income  " + LookupDataAccess.GetAmountSep(Entity.Fam_Income.Trim()) + "      Household Size:  " + Entity.Family_count.Trim();

        //                    if (Entity.Transport.Trim() == "None")
        //                        firstTable.Cell(i, 5).Range.Text = "No Transportation";
        //                    else
        //                        firstTable.Cell(i, 5).Range.Text = Entity.Transport.Trim() + "  Trans";

        //                    string AltFund = string.Empty;
        //                    if (!string.IsNullOrEmpty(Entity.Alt_FUND.Trim()))
        //                    {
        //                        foreach (CommonEntity drFund in propfundingSource)
        //                        {
        //                            if (Entity.Alt_FUND.Trim() == drFund.Code.ToString().Trim())
        //                            {
        //                                AltFund = drFund.Desc.ToString().Trim();
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    firstTable.Cell(i, 6).Range.Text = "Alternate Fund:  " + AltFund;

        //                    i++;
        //                    firstTable.Rows.Add(missing);

        //                    if (rdoRank.Checked == true)
        //                    {
        //                        if (Sel_Ranklist.Count > 0)
        //                        {
        //                            int TempR_Cnt = Sel_Ranklist.Count;
        //                            string Rank1_Desc = string.Empty, Rank2_Desc = string.Empty, Rank3_Desc = string.Empty, Rank4_Desc = string.Empty, Rank5_Desc = string.Empty, Rank6_Desc = string.Empty;
        //                            string Rank1_Det = "0", Rank2_Det = "0", Rank3_Det = "0", Rank4_Det = "0", Rank5_Det = "0", Rank6_Det = "0";
        //                            for (int K = 0; i < Sel_Ranklist.Count; K++)
        //                            {
        //                                switch (i)
        //                                {
        //                                    case 0: if (!string.IsNullOrEmpty(Entity.Rank1.Trim()) && Entity.Rank1.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[0].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank1_Desc = Sel_Ranklist[0].Desc.Trim();
        //                                                Rank1_Det = Entity.Rank1.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                    case 1: if (!string.IsNullOrEmpty(Entity.Rank2.Trim()) && Entity.Rank2.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[1].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank2_Desc = Sel_Ranklist[1].Desc.Trim();
        //                                                Rank2_Det = Entity.Rank2.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                    case 2: if (!string.IsNullOrEmpty(Entity.Rank3.Trim()) && Entity.Rank3.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[2].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank3_Desc = Sel_Ranklist[2].Desc.Trim();
        //                                                Rank3_Det = Entity.Rank1.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                    case 3: if (!string.IsNullOrEmpty(Entity.Rank4.Trim()) && Entity.Rank4.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[3].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank4_Desc = Sel_Ranklist[3].Desc.Trim();
        //                                                Rank4_Det = Entity.Rank4.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                    case 4:
        //                                        if (!string.IsNullOrEmpty(Entity.Rank5.Trim()) && Entity.Rank5.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[4].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank5_Desc = Sel_Ranklist[4].Desc.Trim();
        //                                                Rank5_Det = Entity.Rank5.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                    case 6:
        //                                        if (!string.IsNullOrEmpty(Entity.Rank6.Trim()) && Entity.Rank6.Trim() != "0")
        //                                        {
        //                                            if (Sel_Ranklist[5].HeadStrt.Trim() != "N")
        //                                            {
        //                                                Rank6_Desc = Sel_Ranklist[5].Desc.Trim();
        //                                                Rank6_Det = Entity.Rank6.Trim();
        //                                            }
        //                                        }
        //                                        break;
        //                                }
        //                            }
        //                            int TotR = int.Parse(Rank1_Det) + int.Parse(Rank2_Det) + int.Parse(Rank3_Det) + int.Parse(Rank4_Det) + int.Parse(Rank5_Det) + int.Parse(Rank6_Det);

        //                            //if (Rank1_Det == "0") Rank1_Det = ""; if (Rank2_Det == "0") Rank2_Det = ""; if (Rank3_Det == "0") Rank3_Det = "";
        //                            //if (Rank4_Det == "0") Rank4_Det = ""; if (Rank5_Det == "0") Rank5_Det = ""; if (Rank6_Det == "0") Rank6_Det = "";

        //                            firstTable.Cell(i, 1).Range.Text = string.Empty;

        //                            if (string.IsNullOrEmpty(Rank1_Desc) && Rank1_Det == "0") Rank1_Det = "";
        //                            if (string.IsNullOrEmpty(Rank2_Desc) && Rank2_Det == "0") Rank2_Det = "";
        //                            if (string.IsNullOrEmpty(Rank3_Desc) && Rank3_Det == "0") Rank3_Det = "";
        //                            if (string.IsNullOrEmpty(Rank4_Desc) && Rank4_Det == "0") Rank4_Det = "";
        //                            if (string.IsNullOrEmpty(Rank5_Desc) && Rank5_Det == "0") Rank5_Det = "";
        //                            if (string.IsNullOrEmpty(Rank6_Desc) && Rank6_Det == "0") Rank6_Det = "";

        //                            firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 5;
        //                            firstTable.Cell(i, 2).Range.Text = Rank1_Desc + "   " + Rank1_Det.ToString() + "    " + Rank2_Desc + "    " + Rank2_Det.ToString() + "    " + Rank3_Desc + " " + Rank3_Det.ToString()
        //                                + "    " + Rank4_Desc + "    " + Rank4_Det.ToString() + "    " + Rank5_Desc + "    " + Rank5_Det.ToString() + "    " + Rank6_Desc + "    " + Rank6_Det.ToString() + " Total:  " + TotR;

        //                        }
        //                        else
        //                        {
        //                            firstTable.Cell(i, 1).Range.Text = string.Empty;
        //                            firstTable.Cell(i, 2).Range.ParagraphFormat.SpaceAfter = 5;
        //                            firstTable.Cell(i, 2).Range.Text = string.Empty;

        //                        }
        //                    }

        //                    string Spl_Desc = string.Empty;
        //                    if (!string.IsNullOrEmpty(Entity.CasecondApp.Trim()))
        //                    {
        //                        i++;
        //                        firstTable.Rows.Add(missing);
        //                        if (!string.IsNullOrEmpty(Entity.Allergy.Trim())) Spl_Desc = Entity.Allergy.Trim();
        //                        else if (!string.IsNullOrEmpty(Entity.DietRestrct.Trim())) Spl_Desc = Entity.DietRestrct.Trim();
        //                        else if (!string.IsNullOrEmpty(Entity.Medications.Trim())) Spl_Desc = Entity.Medications.Trim();
        //                        else if (!string.IsNullOrEmpty(Entity.MedConds.Trim())) Spl_Desc = Entity.MedConds.Trim();
        //                        else if (!string.IsNullOrEmpty(Entity.HHConcerns.Trim())) Spl_Desc = Entity.HHConcerns.Trim();
        //                        else if (!string.IsNullOrEmpty(Entity.DevlConcerns.Trim())) Spl_Desc = Entity.DevlConcerns.Trim();

        //                        firstTable.Cell(i, 1).Range.Text = string.Empty;


        //                        string Disability = string.Empty;
        //                        if (!string.IsNullOrEmpty(Entity.Disability_type.Trim())) Disability = Entity.Disability_type.Trim() + "\r\n";

        //                        firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 5;
        //                        firstTable.Cell(i, 2).Range.Text = Disability + Spl_Desc;
        //                    }

        //                    if (chkbCasenotes.Checked == true)
        //                    {
        //                        List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesWaitList(Entity.Agy + Entity.Dept + Entity.Prog + Entity.Year + Entity.AppNo);
        //                        if (caseNotesEntity.Count > 0)
        //                        {
        //                            if (!string.IsNullOrEmpty(caseNotesEntity[0].Data.Trim()))
        //                            {
        //                                i++;
        //                                firstTable.Rows.Add(missing);

        //                                firstTable.Cell(i, 1).Range.Text = string.Empty;
        //                                firstTable.Rows[i].Range.ParagraphFormat.SpaceAfter = 5;
        //                                firstTable.Cell(i, 2).Range.Text = caseNotesEntity[0].Data.Trim();

        //                            }
        //                        }
        //                    }

        //                    //i++;
        //                //}
        //            }

        //            //Save the document
        //            object filename = PdfName;
        //            document.SaveAs(ref filename);
        //            document.Close(ref missing, ref missing, ref missing);
        //            document = null;
        //            winword.Quit(ref missing, ref missing, ref missing);
        //            winword = null;
        //            MessageBox.Show("Document created successfully !");

        //        }

        //        //try
        //        //{
        //        //    Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
        //        //    winword.Visible = false;

        //        //    object missing = System.Reflection.Missing.Value;

        //        //    //Create a new document
        //        //    Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

        //        //    //Add header into the document
        //        //    foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
        //        //    {
        //        //        //Get the header range and add the header details.
        //        //        Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
        //        //        headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
        //        //        headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //        //        headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
        //        //        headerRange.Font.Size = 10;
        //        //        headerRange.Text = "Waiting List Report";

        //        //    }

        //        //    //Add paragraph with Heading 1 style
        //        //    //Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
        //        //    //object styleHeading1 = "Heading 1";
        //        //    //para1.Range.set_Style(ref styleHeading1);
        //        //    //para1.Range.Text = "Para 1 text";
        //        //    ////para1.Range.InsertParagraphAfter();

        //        //    object oEndOfDoc = "\\endofdoc";

        //        //    Microsoft.Office.Interop.Word.Range wrdRng = document.Bookmarks.get_Item(ref oEndOfDoc).Range;

        //        //    //Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(wrdRng, 1, 6, ref missing, ref missing);

        //        //    document.PageSetup.LeftMargin = 20.0f;
        //        //    document.PageSetup.RightMargin = 20.0f;

        //        //    string Data = String.Format("{0,5}", "Rank") + String.Format("{0,10}", "App.") + String.Format("{0,30}", "Name") + String.Format("{0,30}", "") + String.Format("{0,30}", "Address") + String.Format("{0,30}", "Parent/Guardian");

        //        //    Word.Paragraph p1 = document.Content.Paragraphs.Add(ref missing);
        //        //    p1.Range.Font.Bold = 1; p1.Range.Font.Size = 14;
        //        //    p1.Range.Font.Italic = 1; p1.Range.Text = Data;
        //        //    p1.Range.InsertParagraphAfter();

        //        //    bool First = true;

        //        //    Word.Paragraph p2 = document.Content.Paragraphs.Add(ref missing);
        //        //    p2.Range.Font.Size = 12;

        //        //    string FLine = string.Empty; string Sline = string.Empty; string TLine = string.Empty;


        //        //    foreach (HSSB2115Entity Entity in SelRecords)
        //        //    {
        //        //        string Apt = string.Empty; string Floor = string.Empty;
        //        //        if (!string.IsNullOrEmpty(Entity.Apt.Trim()))
        //        //            Apt = "  Apt  " + Entity.Apt.Trim();
        //        //        if (!string.IsNullOrEmpty(Entity.Flr.Trim()))
        //        //            Apt = "  Flr  " + Entity.Flr.Trim();

        //        //        FLine = String.Format("{0,5}", Entity.Rank.Trim()) + String.Format("{0,10}", Entity.AppNo.Trim()) + String.Format("{0,30}", Entity.Fname.Trim() + " " + Entity.Mname.Trim() + " " + Entity.Lname.Trim()) + String.Format("{0,30}", "Age  " + Entity.Age.Trim() + " yrs  " + Entity.Months.Trim() + " mos") + String.Format("{0,30}", Entity.Hno.Trim() + "   " + Entity.Street.Trim() + Apt + Floor) + String.Format("{0,30}", Entity.G1_FName.Trim() + " " + Entity.G1_LName.Trim());

        //        //        p2.Range.Text = FLine;

        //        //        p2.Range.InsertParagraphAfter();

        //        //    }


        //        //    //Save the document
        //        //    object filename = PdfName;
        //        //    document.SaveAs(ref filename);
        //        //    document.Close(ref missing, ref missing, ref missing);
        //        //    document = null;
        //        //    winword.Quit(ref missing, ref missing, ref missing);
        //        //    winword = null;
        //        //    MessageBox.Show("Document created successfully !");

        //        //}
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }


        //    }
        //}


        private void ExcelFormats(object sender, FormClosedEventArgs e)
        {

            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
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

                string data = null;
                int i = 0;
                int j = 0;

                ExcelDocument xlWorkSheet = new ExcelDocument();
                
               // Excel.Application xlApp;
               // Excel.Workbook xlWorkBook;
               //// Excel.Worksheet xlWorkSheet;
               // Excel.Range chartRange;
               // object misValue = System.Reflection.Missing.Value;

               // xlApp = new Excel.Application();
                //xlWorkBook = xlApp.Workbooks.Add(misValue);
               // xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                CaseSiteEntity SearchEntity = new CaseSiteEntity(true);
                SearchEntity.SiteROOM = "0000"; SearchEntity.SiteAGENCY = Agency; SearchEntity.SiteDEPT = Depart; SearchEntity.SitePROG = Program;
                SearchEntity.SiteYEAR = Year;
                propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(SearchEntity, "Browse");


                //xlWorkSheet.Cells[2, 2] = "Rank";
                //xlWorkSheet.Cells[2, 3] = "App.";
                //xlWorkSheet.Cells[2, 4] = "Name";
                //xlWorkSheet.Cells[2, 5] = "";
                //xlWorkSheet.Cells[2, 6] = "Address";
                //xlWorkSheet.Cells[2, 7] = "Parent/Guardian";
                xlWorkSheet.ColumnWidth(0, 0);
                xlWorkSheet.ColumnWidth(1, 0);
                xlWorkSheet.ColumnWidth(2,100);
                xlWorkSheet.ColumnWidth(3,100);
                xlWorkSheet.ColumnWidth(4,120);
                xlWorkSheet.ColumnWidth(5,200);
                xlWorkSheet.ColumnWidth(6,200);
                xlWorkSheet.ColumnWidth(7,200);
                xlWorkSheet.ColumnWidth(8,200);
             

                bool first = true; string Priv_Site = string.Empty; string Priv_Desc = string.Empty; string Site_Desc = string.Empty;
                CaseSiteEntity Sel_Site = new CaseSiteEntity();
                int k = 0;
                for (i = 0; i <= SelRecords.Count - 1; i++)
                {

                    //int number;
                    if (rdoSiteName.Checked == true)
                    {
                        if (rbIntakeCmpltd.Checked == false ? SelRecords[i].EnrlSite.Trim() != Priv_Site : SelRecords[i].Site.Trim() != Priv_Site)
                        //if (SelRecords[i].Site.Trim() != Priv_Site)
                        {
                            if (propCaseSiteEntity.Count > 0)
                            {
                                if (rbWaitlist.Checked || rbBoth.Checked)
                                    Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(SelRecords[i].EnrlSite.Trim()) && u.SiteYEAR.Equals(Year));
                                else
                                    Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(SelRecords[i].Site.Trim()) && u.SiteYEAR.Equals(Year));

                                //Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(SelRecords[i].Site.Trim()) && u.SiteYEAR.Equals(Year));
                                if (Sel_Site != null)
                                    Site_Desc = Sel_Site.SiteNAME.Trim();
                                else Site_Desc = "";
                            }

                            if (!first)
                            {
                                k = k + 2;

                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Merge();
                                ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true; 
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 2,"All applicants for site " + Priv_Site + "  " + Priv_Desc);
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);                                
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;

                                k++;

                              //  xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                               // xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                               // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                              //  xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 2, "Site Totals  ");
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 4, "Income Eligible");
                                xlWorkSheet[k + 3, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 4].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 5, "Over Income");
                                xlWorkSheet[k + 3, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3,5].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 6, "In Certify");
                                xlWorkSheet[k + 3, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 6].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 7, "101% - 130%");
                                xlWorkSheet[k + 3, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 7].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 8, "Categorical");
                                xlWorkSheet[k + 3, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 8].Alignment = Alignment.Centered;

                                if (propfundingSource.Count > 0)
                                {
                                    foreach (CommonEntity FundEntity in propfundingSource)
                                    {
                                        k++;
                                        List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                        List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                        List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                                         xlWorkSheet.WriteCell(k + 3, 2, FundEntity.Desc.Trim());

                                        if (rbWaitlist.Checked || rbBoth.Checked)
                                        {
                                            IEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                            OIentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                            ICentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                            Pentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                            CAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                        }
                                        else
                                        {
                                            IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                            OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                            ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                            Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                            CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                        }

                                        xlWorkSheet.WriteCell(k + 3, 4, IEentity.Count.ToString());
                                        xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                                        xlWorkSheet.WriteCell(k + 3, 5, OIentity.Count.ToString());
                                        xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                                        xlWorkSheet.WriteCell(k + 3, 6, ICentity.Count.ToString());
                                        xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                                        xlWorkSheet.WriteCell(k + 3, 7, Pentity.Count.ToString());
                                        xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                                        xlWorkSheet.WriteCell(k + 3, 8, CAentity.Count.ToString());
                                        xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;
                                        
                                    }
                                    k++;
                                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;

                                    List<HSSB2115Entity> SEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> SOentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> SCentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> SPentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> SCAentity = new List<HSSB2115Entity>();

                                    if (rbBoth.Checked || rbWaitlist.Checked)
                                    {
                                        SEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                        SOentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                        SCentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                        SPentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                        SCAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                    }
                                    else
                                    {
                                        SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                        SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                        SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                        SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                        SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                    }

                                    xlWorkSheet.WriteCell(k + 3, 2, "No Fund Specified");
                                    xlWorkSheet.WriteCell(k + 3, 4, SEentity.Count.ToString());
                                    xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(k + 3, 5, SOentity.Count.ToString());
                                    xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(k + 3, 6, SCentity.Count.ToString());
                                    xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(k + 3, 7, SPentity.Count.ToString());
                                    xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                                    xlWorkSheet.WriteCell(k + 3, 8, SCAentity.Count.ToString());
                                    xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;

                                    k++;
                                }

                                k = k + 2;
                                //    xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Merge();
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;

                                string SiteName = string.Empty;
                                if (rbWaitlist.Checked || rbBoth.Checked)
                                    SiteName = "All applicants for site " + SelRecords[i].EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                else
                                    SiteName = "All applicants for site " + SelRecords[i].Site.Trim() + "  " + Site_Desc.Trim();

                                xlWorkSheet.WriteCell(k + 3, 2, SiteName);
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                k++;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                                xlWorkSheet.WriteCell(k + 3, 2, "Rank");
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 3, "App.");
                                xlWorkSheet[k + 3,3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 3].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 4, "Name");
                                xlWorkSheet[k + 3, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 4].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 5,"");
                                xlWorkSheet[k + 3, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 5].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 6,"Address");
                                xlWorkSheet[k + 3, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 6].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 7, "Parent/Guardian");
                                xlWorkSheet[k + 3, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 7].Alignment = Alignment.Centered;
                            }
                            else
                            {
                                //k = k + 2;
                                // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Merge();
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;

                                string SiteName = string.Empty;
                                if (rbWaitlist.Checked || rbBoth.Checked)
                                    SiteName = "All applicants for site " + SelRecords[i].EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                else
                                    SiteName = "All applicants for site " + SelRecords[i].Site.Trim() + "  " + Site_Desc.Trim();

                                xlWorkSheet.WriteCell(k + 3, 2, SiteName);
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                k++;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                                //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                                xlWorkSheet.WriteCell(k + 3, 2, "Rank");
                                xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 3, "App.");
                                xlWorkSheet[k + 3, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 3].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 4, "Name");
                                xlWorkSheet[k + 3, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 4].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 5, "");
                                xlWorkSheet[k + 3, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 5].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 6, "Address");
                                xlWorkSheet[k + 3, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 6].Alignment = Alignment.Centered;
                                xlWorkSheet.WriteCell(k + 3, 7, "Parent/Guardian");
                                xlWorkSheet[k + 3, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                                xlWorkSheet[k + 3, 7].Alignment = Alignment.Centered;
                                first = false;

                            }
                        }
                    }

                    k++;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                    data = SelRecords[i].Rank.Trim();
                     xlWorkSheet.WriteCell(k + 3, 2, data);
                     xlWorkSheet[k + 3, 2].Alignment = Alignment.Right;
                     xlWorkSheet.WriteCell(k + 3, 3, SelRecords[i].AppNo.Trim());
                     xlWorkSheet.WriteCell(k + 3, 4, (SelRecords[i].Disable.Trim() == "Y" ? "*" : "") + SelRecords[i].Fname.Trim() + "  " + SelRecords[i].Mname.Trim() + "  " + SelRecords[i].Lname.Trim());
                     xlWorkSheet.WriteCell(k + 3, 5,"Age  " + SelRecords[i].Age.Trim() + " yrs  " + SelRecords[i].Months.Trim() + " mos");

                    string Apt = string.Empty; string Floor = string.Empty;
                    if (!string.IsNullOrEmpty(SelRecords[i].Apt.Trim()))
                        Apt = "  Apt  " + SelRecords[i].Apt.Trim();
                    if (!string.IsNullOrEmpty(SelRecords[i].Flr.Trim()))
                        Apt = "  Flr  " + SelRecords[i].Flr.Trim();

                     xlWorkSheet.WriteCell(k + 3, 6, SelRecords[i].Hno.Trim() + "   " + SelRecords[i].Street.Trim() + " " + SelRecords[i].Suffix.Trim() + Apt + Floor);
                     xlWorkSheet.WriteCell(k + 3, 7, SelRecords[i].G1_FName.Trim() + " " + SelRecords[i].G1_LName.Trim());

                    k++;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                     xlWorkSheet.WriteCell(k + 3, 2, " ");
                    string Sex = string.Empty; string Class = string.Empty;
                    if (SelRecords[i].Sex.Trim() == "M") Sex = "MALE"; else Sex = "FEMALE";
                    if (SelRecords[i].ClassPrefer.Trim() == "A") Class = "AM";
                    else if (SelRecords[i].ClassPrefer.Trim() == "P") Class = "PM";
                    else if (SelRecords[i].Classfication.Trim() == "E") Class = "EXTD"; else if (SelRecords[i].ClassPrefer.Trim() == "F") Class = "FULL";

                    //xlWorkSheet.get_Range("c"+k, "e"+k).Merge(false);
                   // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 5]].Merge();

                    if(SelRecords[i].ClassPrefer=="N")
                        xlWorkSheet.WriteCell(k + 3, 3, "PREFER" + "       " + " SLOT    " + Sex.Trim());
                    else
                         xlWorkSheet.WriteCell(k + 3, 3, "PREFER    " + Class + " SLOT  " + Sex.Trim());

                     xlWorkSheet.WriteCell(k + 3, 6, SelRecords[i].City.Trim() + " " + SelRecords[i].State.Trim() + "  " + SelRecords[i].Zip.Trim());

                    MaskedTextBox mskWPhn = new MaskedTextBox();
                    mskWPhn.Mask = "(999) 000-0000";
                    if (!string.IsNullOrEmpty(SelRecords[i].Empl_Phone.Trim()))
                        mskWPhn.Text = SelRecords[i].Phone.Trim();

                     xlWorkSheet.WriteCell(k + 3, 7, "Work :  " + mskWPhn.Text.Trim());

                    k++;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                     xlWorkSheet.WriteCell(k + 3, 2, " ");
                    MaskedTextBox mskPhn = new MaskedTextBox();
                    mskPhn.Mask = "(999) 000-0000";
                    if (!string.IsNullOrEmpty(SelRecords[i].Phone.Trim()))
                        mskPhn.Text = SelRecords[i].Phone.Trim();

                   // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 4]].Merge();
                    //xlWorkSheet.get_Range("c" + k, "d" + k).Merge(false);
                     xlWorkSheet.WriteCell(k + 3, 3, "DOB: " + LookupDataAccess.Getdate(SelRecords[i].DOB.Trim()) + "    Phone: " + mskPhn.Text.Trim());

                    string Income_Desc = string.Empty; string Meals = string.Empty;
                    if (SelRecords[i].Classfication.Trim() == "98" || SelRecords[i].Classfication.Trim()=="95") Income_Desc = "Over Inc.";
                    else if (SelRecords[i].Classfication.Trim() == "99") Income_Desc = "Eligible";
                    else Income_Desc = "In Certify";
                    string Meals_Type = string.Empty;
                    if (SelRecords[i].MealElig == "1") Meals_Type = "Free Meals";
                    else if (SelRecords[i].MealElig == "2") Meals_Type = "Reduced Meals";
                    else if (SelRecords[i].MealElig == "3") Meals_Type = "Paid Meals";

                    //xlWorkSheet.get_Range("e" + k, "f" + k).Merge(false);
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 5], xlWorkSheet.Cells[k + 3, 6]].Merge();
                    if(rdoSiteName.Checked==true)
                         xlWorkSheet.WriteCell(k + 3, 5, "      " + Income_Desc + "  " + LookupDataAccess.Getdate(SelRecords[i].Elig_Date.Trim()) + "      " + Meals_Type);
                    else
                        xlWorkSheet.WriteCell(k + 3, 5, SelRecords[i].Site.Trim() + "      " + Income_Desc + "  " + LookupDataAccess.Getdate(SelRecords[i].Elig_Date.Trim()) + "      " + Meals_Type);

                    string Fund = string.Empty;
                    if (!string.IsNullOrEmpty(SelRecords[i].CHLDMST_FUND.Trim()))
                    {
                        foreach (CommonEntity drFund in propfundingSource)
                        {
                            if (SelRecords[i].CHLDMST_FUND.Trim() == drFund.Code.ToString().Trim())
                            {
                                Fund = drFund.Desc.ToString().Trim();
                                break;
                            }
                        }
                    }

                    xlWorkSheet.WriteCell(k + 3, 7, "Expected Fund :  " + Fund);

                    k++;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                    xlWorkSheet.WriteCell(k + 3, 2, " ");
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 5]].Merge();
                    //xlWorkSheet.get_Range("c" + k, "e" + k).Merge(false);
                    xlWorkSheet.WriteCell(k + 3, 3, "Household Income  " + LookupDataAccess.GetAmountSep(SelRecords[i].Fam_Income.Trim()) + "      Household Size:  " + SelRecords[i].Family_count.Trim());

                    if (SelRecords[i].Transport.Trim() == "None")
                        xlWorkSheet.WriteCell(k + 3,6, "No Transportation");
                    else
                        xlWorkSheet.WriteCell(k + 3, 6, SelRecords[i].Transport.Trim() + "  Trans");

                    string AltFund = string.Empty;
                    if (!string.IsNullOrEmpty(SelRecords[i].Alt_FUND.Trim()))
                    {
                        foreach (CommonEntity drFund in propfundingSource)
                        {
                            if (SelRecords[i].Alt_FUND.Trim() == drFund.Code.ToString().Trim())
                            {
                                AltFund = drFund.Desc.ToString().Trim();
                                break;
                            }
                        }
                    }
                    xlWorkSheet.WriteCell(k + 3,7, "Alternate Fund:  " + AltFund);

                    k++;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                    if (rdoRank.Checked == true)
                    {
                        if (Sel_Ranklist.Count > 0)
                        {
                            int TempR_Cnt = Sel_Ranklist.Count;
                            string Rank1_Desc = string.Empty, Rank2_Desc = string.Empty, Rank3_Desc = string.Empty, Rank4_Desc = string.Empty, Rank5_Desc = string.Empty, Rank6_Desc = string.Empty;
                            string Rank1_Det = "0", Rank2_Det = "0", Rank3_Det = "0", Rank4_Det = "0", Rank5_Det = "0", Rank6_Det = "0";
                            for (int m = 0; m < Sel_Ranklist.Count; m++)
                            {
                                switch (i)
                                {
                                    case 0: if (!string.IsNullOrEmpty(SelRecords[i].Rank1.Trim()) && SelRecords[i].Rank1.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[0].HeadStrt.Trim() != "N")
                                            {
                                                Rank1_Desc = Sel_Ranklist[0].Desc.Trim();
                                                Rank1_Det = SelRecords[i].Rank1.Trim();
                                            }
                                        }
                                        break;
                                    case 1: if (!string.IsNullOrEmpty(SelRecords[i].Rank2.Trim()) && SelRecords[i].Rank2.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[1].HeadStrt.Trim() != "N")
                                            {
                                                Rank2_Desc = Sel_Ranklist[1].Desc.Trim();
                                                Rank2_Det = SelRecords[i].Rank2.Trim();
                                            }
                                        }
                                        break;
                                    case 2: if (!string.IsNullOrEmpty(SelRecords[i].Rank3.Trim()) && SelRecords[i].Rank3.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[2].HeadStrt.Trim() != "N")
                                            {
                                                Rank3_Desc = Sel_Ranklist[2].Desc.Trim();
                                                Rank3_Det = SelRecords[i].Rank1.Trim();
                                            }
                                        }
                                        break;
                                    case 3: if (!string.IsNullOrEmpty(SelRecords[i].Rank4.Trim()) && SelRecords[i].Rank4.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[3].HeadStrt.Trim() != "N")
                                            {
                                                Rank4_Desc = Sel_Ranklist[3].Desc.Trim();
                                                Rank4_Det = SelRecords[i].Rank4.Trim();
                                            }
                                        }
                                        break;
                                    case 4:
                                        if (!string.IsNullOrEmpty(SelRecords[i].Rank5.Trim()) && SelRecords[i].Rank5.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[4].HeadStrt.Trim() != "N")
                                            {
                                                Rank5_Desc = Sel_Ranklist[4].Desc.Trim();
                                                Rank5_Det = SelRecords[i].Rank5.Trim();
                                            }
                                        }
                                        break;
                                    case 6:
                                        if (!string.IsNullOrEmpty(SelRecords[i].Rank6.Trim()) && SelRecords[i].Rank6.Trim() != "0")
                                        {
                                            if (Sel_Ranklist[5].HeadStrt.Trim() != "N")
                                            {
                                                Rank6_Desc = Sel_Ranklist[5].Desc.Trim();
                                                Rank6_Det = SelRecords[i].Rank6.Trim();
                                            }
                                        }
                                        break;
                                }
                            }
                            int TotR = int.Parse(Rank1_Det) + int.Parse(Rank2_Det) + int.Parse(Rank3_Det) + int.Parse(Rank4_Det) + int.Parse(Rank5_Det) + int.Parse(Rank6_Det);


                            if (string.IsNullOrEmpty(Rank1_Desc) && Rank1_Det == "0") Rank1_Det = "";
                            if (string.IsNullOrEmpty(Rank2_Desc) && Rank2_Det == "0") Rank2_Det = "";
                            if (string.IsNullOrEmpty(Rank3_Desc) && Rank3_Det == "0") Rank3_Det = "";
                            if (string.IsNullOrEmpty(Rank4_Desc) && Rank4_Det == "0") Rank4_Det = "";
                            if (string.IsNullOrEmpty(Rank5_Desc) && Rank5_Det == "0") Rank5_Det = "";
                            if (string.IsNullOrEmpty(Rank6_Desc) && Rank6_Det == "0") Rank6_Det = "";

                            string Total_Desc = string.Empty;
                            if (TotR == 0)
                                Total_Desc = "Ranking Total : " + TotR;
                            else
                            {
                                Total_Desc = Rank1_Desc + "   " + Rank1_Det.ToString() + "    " + Rank2_Desc + "    " + Rank2_Det.ToString() + "    " + Rank3_Desc + " " + Rank3_Det.ToString()
                                + "    " + Rank4_Desc + "    " + Rank4_Det.ToString() + "    " + Rank5_Desc + "    " + Rank5_Det.ToString() + "    " + Rank6_Desc + "    " + Rank6_Det.ToString() + " Total:  " + TotR;
                            }
                            xlWorkSheet.WriteCell(k + 3, 2, " ");
                            //xlWorkSheet.get_Range("c" + k, "g" + k).Merge(false);
                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 7]].Merge();
                            xlWorkSheet.WriteCell(k + 3, 3, Total_Desc);
                        }
                    }

                    string Spl_Desc = string.Empty;
                    if (!string.IsNullOrEmpty(SelRecords[i].CasecondApp.Trim()))
                    {
                        if (!string.IsNullOrEmpty(SelRecords[i].Allergy.Trim())) Spl_Desc = SelRecords[i].Allergy.Trim();
                        else if (!string.IsNullOrEmpty(SelRecords[i].DietRestrct.Trim())) Spl_Desc = SelRecords[i].DietRestrct.Trim();
                        else if (!string.IsNullOrEmpty(SelRecords[i].Medications.Trim())) Spl_Desc = SelRecords[i].Medications.Trim();
                        else if (!string.IsNullOrEmpty(SelRecords[i].MedConds.Trim())) Spl_Desc = SelRecords[i].MedConds.Trim();
                        else if (!string.IsNullOrEmpty(SelRecords[i].HHConcerns.Trim())) Spl_Desc = SelRecords[i].HHConcerns.Trim();
                        else if (!string.IsNullOrEmpty(SelRecords[i].DevlConcerns.Trim())) Spl_Desc = SelRecords[i].DevlConcerns.Trim();

                        string Disability = string.Empty;
                        if (!string.IsNullOrEmpty(SelRecords[i].Disability_type.Trim())) Disability = SelRecords[i].Disability_type.Trim() + "\r\n";
                        k++;
                        xlWorkSheet.WriteCell(k + 3, 2, " ");
                       // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 7]].Merge();
                        //xlWorkSheet.get_Range("c" + k, "g" + k).Merge(false);
                        xlWorkSheet.WriteCell(k + 3, 3, Disability + Spl_Desc);
                    }

                    if (string.IsNullOrEmpty(SelRecords[i].EnrlStatus.Trim()))
                    {
                        k++;

                        xlWorkSheet.WriteCell(k + 3, 2, " ");
                        xlWorkSheet.WriteCell(k + 3, 3, "INTAKE COMPLETED ....missing status record!");
                    }


                    if (chkbCasenotes.Checked == true)
                    {
                        List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesWaitList(SelRecords[i].Agy + SelRecords[i].Dept + SelRecords[i].Prog + SelRecords[i].Year + SelRecords[i].AppNo);
                        if (caseNotesEntity.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(caseNotesEntity[0].Data.Trim()))
                            {
                                k++;
                                xlWorkSheet.WriteCell(k + 3, 2, " ");
                                //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 3], xlWorkSheet.Cells[k + 3, 7]].Merge();
                                //xlWorkSheet.get_Range("c" + k, "g" + k).Merge(false);
                                xlWorkSheet.WriteCell(k + 3, 3, caseNotesEntity[0].Data.Trim());
                            }
                        }
                    }
                    Priv_Site = SelRecords[i].Site.Trim(); Priv_Desc = Site_Desc.Trim();
                    //k++;
                }

                if (rdoSiteName.Checked == true)
                {
                    k = k + 2;
                   // xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Merge();
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                    //xlWorkSheet.Cells[k + 3, 2].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //xlWorkSheet.Cells[k + 3, 2].Style.Font.Size = 13;
                    xlWorkSheet.WriteCell(k + 3, 2, "All applicants for site " + Priv_Site + "  " + Priv_Desc);
                    xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;

                    k++;

                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                    xlWorkSheet.WriteCell(k + 3, 2, "Site Totals  ");
                    xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 4, "Income Eligible");
                    xlWorkSheet[k + 3, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 4].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 5, "Over Income");
                    xlWorkSheet[k + 3, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 5].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 6, "In Certify");
                    xlWorkSheet[k + 3, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 6].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 7, "101% - 130%");
                    xlWorkSheet[k + 3, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 7].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 8, "Categorical");
                    xlWorkSheet[k + 3, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 8].Alignment = Alignment.Centered;

                    if (propfundingSource.Count > 0)
                    {

                        foreach (CommonEntity FundEntity in propfundingSource)
                        {
                            k++;
                            List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                            List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                            List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();


                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                            ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.IsBold = false;
                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                            xlWorkSheet.WriteCell(k + 3, 2, FundEntity.Desc.Trim());

                            if(rbWaitlist.Checked || rbBoth.Checked)
                            {
                                IEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                OIentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                ICentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                Pentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                CAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                            }
                            else
                            {
                                IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                            }


                            xlWorkSheet.WriteCell(k + 3, 4, IEentity.Count.ToString());
                            xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 5, OIentity.Count.ToString());
                            xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 6, ICentity.Count.ToString());
                            xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 7, Pentity.Count.ToString());
                            xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 8, CAentity.Count.ToString());
                            xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;
                        }
                        k++;

                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                        ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.IsBold = false;
                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;

                        List<HSSB2115Entity> SEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> SOentity = new List<HSSB2115Entity>();
                        List<HSSB2115Entity> SCentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> SPentity = new List<HSSB2115Entity>();
                        List<HSSB2115Entity> SCAentity = new List<HSSB2115Entity>();

                        if (rbBoth.Checked || rbWaitlist.Checked)
                        {
                            SEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                            SOentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                            SCentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                            SPentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                            SCAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                        }
                        else
                        {
                            SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                            SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                            SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                            SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                            SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                        }


                        xlWorkSheet.WriteCell(k + 3, 2, "No Fund Specified");
                        xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                        xlWorkSheet.WriteCell(k + 3, 4, SEentity.Count.ToString());
                        xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 5, SOentity.Count.ToString());
                        xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 6, SCentity.Count.ToString());
                        xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 7, SPentity.Count.ToString());
                        xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 8, SCAentity.Count.ToString());
                        xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;
                    }

                    k = k + 2;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Merge();
                    //xlWorkSheet.Cells[k + 3, 2] = "All applicants for site " + Priv_Site + "  " + Priv_Desc;

                    //k++;

                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                    ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Style.HorizontalAlignment = 7;
                    //xlWorkSheet.Cells[k + 3, 1].EntireRow.Font.Bold = true;
                    //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 13;
                    xlWorkSheet.WriteCell(k + 3,  2, "Report Totals ");
                    xlWorkSheet[k + 3, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 2].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3,  4, "Income Eligible");
                    xlWorkSheet[k + 3, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 4].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3,  5, "Over Income");
                    xlWorkSheet[k + 3, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 5].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3,  6, "In Certify");
                    xlWorkSheet[k + 3, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 6].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 7, "101% - 130%");
                    xlWorkSheet[k + 3, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 7].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(k + 3, 8, "Categorical");
                    xlWorkSheet[k + 3, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[k + 3, 8].Alignment = Alignment.Centered;

                    if (propfundingSource.Count > 0)
                    {
                        foreach (CommonEntity FundEntity in propfundingSource)
                        {
                            k++;
                            List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                            List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>(); List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                            List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();


                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 3]].Merge();
                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                            ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.IsBold = false;
                            //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;
                            xlWorkSheet.WriteCell(k + 3,  2, FundEntity.Desc.Trim());




                            IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                            OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                            ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                            Pentity = SelRecords.FindAll(u =>  u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                            CAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));


                            xlWorkSheet.WriteCell(k + 3, 4, IEentity.Count.ToString());
                            xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 5, OIentity.Count.ToString());
                            xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 6, ICentity.Count.ToString());
                            xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 7, Pentity.Count.ToString());
                            xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                            xlWorkSheet.WriteCell(k + 3, 8, CAentity.Count.ToString());
                            xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;
                        }

                        List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                        List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                        List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                        List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                        List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));

                        k++;

                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.HorizontalAlignment = 2;
                        ////xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.IsBold = false;
                        //xlWorkSheet.Range[xlWorkSheet.Cells[k + 3, 2], xlWorkSheet.Cells[k + 3, 7]].Style.Font.Size = 11;

                        xlWorkSheet.WriteCell(k + 3, 2, "No Fund Specified");
                        xlWorkSheet.WriteCell(k + 3, 4, SEentity.Count.ToString());
                        xlWorkSheet[k + 3, 4].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 5, SOentity.Count.ToString());
                        xlWorkSheet[k + 3, 5].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 6, SCentity.Count.ToString());
                        xlWorkSheet[k + 3, 6].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 7, SPentity.Count.ToString());
                        xlWorkSheet[k + 3, 7].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(k + 3, 8, SCAentity.Count.ToString());
                        xlWorkSheet[k + 3, 8].Alignment = Alignment.Right;
                    }

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                xlWorkSheet.Save(stream);
                stream.Close();

                //xlWorkSheet.get_Range("a1", "h1").Merge(false);

                //chartRange = xlWorkSheet.get_Range("a1", "h1");
                //chartRange.FormulaR1C1 = "Waiting List Report";
                //chartRange.Font.Bold = true;
                //chartRange.Font.Color = System.Drawing.Color.Blue;
                //chartRange.Font.Size = 15;
                //chartRange.HorizontalAlignment = 7;
                ////chartRange.VerticalAlignment = 1;

                ////chartRange = xlWorkSheet.get_Range("d2", "c2");


                //chartRange = xlWorkSheet.get_Range("b2", "g2");
                //chartRange.Font.Bold = true;
                //chartRange.Font.Size = 13;
                //chartRange = xlWorkSheet.get_Range("b3", "g3");
                //chartRange.Font.Bold = true;
                //chartRange.Font.Size = 13;
                //chartRange.HorizontalAlignment = 7;
                //string PdfName = "Pdf File";
                //PdfName = PdfName + ".xls";

                //xlWorkBook.SaveAs(PdfName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();

                //releaseObject(xlWorkSheet);
                //releaseObject(xlWorkBook);
                //releaseObject(xlApp);

                //MessageBox.Show("Waiting_List_Reoirt.xls file created , you can find the file " + PdfName);
                
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                AlertBox.Show("Exception occured while releasing Object " + ex.ToString(), MessageBoxIcon.Warning);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Wait List");
        }

        private void rbDayCare_Click(object sender, EventArgs e)
        {
            //if (ShortName == "OFO")
            //{
            //    //rbFundSel.Text = "Primary"; rbDayCare.Visible = true;
            lblIncludeClients.Visible = false; pnlIncClients.Visible = false; pnlIncClients.Visible = false;
            this.Size = new Size(850, 395);
            //}
            if (rbDayCare.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges, "DayCare");
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        private void rbFundAll_Click(object sender, EventArgs e)
        {
            //if (ShortName == "OFO")
            //{
                //rbFundSel.Text = "Primary"; rbDayCare.Visible = true;
            lblIncludeClients.Visible = true; pnlIncClients.Visible = true; pnlIncClients.Visible = true;
            this.Size = new Size(850,425);
            SelFundingList.Clear();
            _errorProvider.SetError(rbFundSel, null);
            _errorProvider.SetError(rbDayCare, null);

            //}
        }

        private void rbMailLbl_Click(object sender, EventArgs e)
        {
            if (rbNoLbl.Checked)
            {
                rbChild.Enabled = false;
                rbParent.Enabled = false; pnlPrint.Enabled = false;
                lblPrintlbl.Enabled = false;

                pnlPrint.Visible = false;
            }
            else if (rbMailLbl.Checked)
            {
                lblPrintlbl.Enabled = true;
                rbChild.Enabled = true; pnlPrint.Enabled = true;
                rbParent.Enabled = true;

                pnlPrint.Visible = true;
            }
        }

        #region Vikash added on 07/30/2024 for converting Excel report to DevExpress format

        #region DevExpress Parameter Page

        private void PrintHeaderPage_New_Format(Document document, PdfWriter writer)
        {
            #region Font styles

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


            iTextSharp.text.Font reportNameStyle = new iTextSharp.text.Font(bf_Calibri, 12, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#002060")));
            iTextSharp.text.Font xTitleCellstyle2 = new iTextSharp.text.Font(bf_Calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0070C0")));
            iTextSharp.text.Font xsubTitleintakeCellstyle = new iTextSharp.text.Font(bf_Calibri, 9, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0070C0")));
            iTextSharp.text.Font paramsCellStyle = new iTextSharp.text.Font(bf_Calibri, 8);

            #endregion

            PdfPTable Headertable = new PdfPTable(5);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 30f, 70f, 10f, 30f, 70f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //***************** border trails *******************************//
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            string strAgy = Current_Hierarchy_DB.Split('-')[0];
            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);
            string ImgName = "";
            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = BaseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;

            if (imagesPath != "")
            {
                try
                {
                    iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imagesPath);
                    imgLogo.ScaleAbsolute(120f, 50f);
                    PdfPCell cellLogo = new PdfPCell(imgLogo);
                    cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellLogo.Rowspan = 2;
                    cellLogo.Padding = 3;
                    cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(cellLogo);


                    AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                    _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                        _agycntrldets = BAgyControlDetails;
                    else
                        _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
                    string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
                    string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
                    string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');
                    string strAddress = street + city + state + zip1;

                    PdfPCell rowH = new PdfPCell(new Phrase(BAgyControlDetails.AgyName, TblParamsHeaderFont));
                    rowH.HorizontalAlignment = Element.ALIGN_CENTER;
                    rowH.Colspan = 5;
                    rowH.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(rowH);

                    PdfPCell row1 = new PdfPCell(new Phrase(strAddress, TblParamsHeaderFont));
                    row1.HorizontalAlignment = Element.ALIGN_CENTER;
                    row1.Colspan = 4;
                    row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(row1);
                }
                catch (Exception ex)
                {
                    AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                    _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                        _agycntrldets = BAgyControlDetails;
                    else
                        _agycntrldets = BaseForm.BaseAgencyControlDetails;

                    string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
                    string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
                    string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
                    string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');
                    string strAddress = street + city + state + zip1;

                    PdfPCell rowH = new PdfPCell(new Phrase(BAgyControlDetails.AgyName, TblParamsHeaderFont));
                    rowH.HorizontalAlignment = Element.ALIGN_CENTER;
                    rowH.Colspan = 5;
                    rowH.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(rowH);

                    PdfPCell row1 = new PdfPCell(new Phrase(strAddress, TblParamsHeaderFont));
                    row1.HorizontalAlignment = Element.ALIGN_CENTER;
                    row1.Colspan = 5;
                    row1.FixedHeight = 25f;
                    row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(row1);
                }

            }

            PdfPCell row2 = new PdfPCell(new Phrase(Privileges.PrivilegeName, reportNameStyle));
            row2.HorizontalAlignment = Element.ALIGN_CENTER;
            row2.Colspan = 5;
            row2.PaddingBottom = 8;
            row2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#CFE6F9"));
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row2);

            PdfPCell row3 = new PdfPCell(new Phrase("Report Parameters", xTitleCellstyle2));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.Colspan = 5;
            row3.PaddingBottom = 8;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row3);

            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string program_year = string.Empty;
            if (Current_Hierarchy.Substring(0, 2) != "**")
                Agy = Current_Hierarchy.Substring(0, 2);
            if (Current_Hierarchy.Substring(2, 2) != "**")
                Dept = Current_Hierarchy.Substring(2, 2);
            if (Current_Hierarchy.Substring(4, 2) != "**")
                Prg = Current_Hierarchy.Substring(4, 2);
            if (CmbYear.Visible == true)
                program_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell row4 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + program_year, xsubTitleintakeCellstyle));
                row4.HorizontalAlignment = Element.ALIGN_CENTER;
                row4.Colspan = 5;
                row4.PaddingBottom = 8;
                row4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F8FBFE"));
                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(row4);
            }
            else
            {
                PdfPCell row4 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), xsubTitleintakeCellstyle));
                row4.HorizontalAlignment = Element.ALIGN_CENTER;
                row4.Colspan = 5;
                row4.PaddingBottom = 8;
                row4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F8FBFE"));
                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(row4);
            }

            PdfPCell rSpace = new PdfPCell(new Phrase(" ", TblHeaderTitleFont));
            rSpace.HorizontalAlignment = Element.ALIGN_CENTER;
            rSpace.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            rSpace.MinimumHeight = 6;
            rSpace.Colspan = 5;
            rSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(rSpace);

            //Row 1
            PdfPCell CH1 = new PdfPCell(new Phrase(lblRepSeq.Text.Trim(), paramsCellStyle));
            CH1.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1.Border = iTextSharp.text.Rectangle.BOX;
            CH1.PaddingBottom = 5;
            CH1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1);

            PdfPCell CB1 = new PdfPCell(new Phrase(rdoSiteName.Checked ? rdoSiteName.Text.Trim() : rdoRank.Text.Trim(), paramsCellStyle));
            CB1.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1.Border = iTextSharp.text.Rectangle.BOX;
            CB1.PaddingBottom = 5;
            CB1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1);

            PdfPCell CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            PdfPCell CH1_2 = new PdfPCell(new Phrase(lblActive.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            PdfPCell CB1_2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Text.ToString().Trim(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 2
            CH1_2 = new PdfPCell(new Phrase(chkbCasenotes.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(chkbCasenotes.Checked ? "Yes" : "No", paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            string Fund_Label = string.Empty;
            if (rbFundAll.Checked == true)
                Fund_Label = lblFundSource.Text.Trim();
            else if (rbFundSel.Text == "Selected Funding" && rbFundSel.Checked)
                Fund_Label = "Selected Funding Sources";
            else if (rbDayCare.Checked)
                Fund_Label = rbDayCare.Text.Trim();
            else
                Fund_Label = rbFundSel.Text.Trim();

            CH1_2 = new PdfPCell(new Phrase(Fund_Label, paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            string Fund = string.Empty;
            if (rbFundAll.Checked == true)
                Fund = rbFundAll.Text.Trim() + " Funds";
            else if (rbFundSel.Text == "Selected Funding" && rbFundSel.Checked)
                Fund = strFundingCodes;
            else if (rbDayCare.Checked)
                Fund = strFundingCodes;
            else
                Fund = strFundingCodes;

            CB1_2 = new PdfPCell(new Phrase(Fund, paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 3
            CH1_2 = new PdfPCell(new Phrase(lblChildAge.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase("From: " + txtFrom.Text.Trim() + "  To: " + txtTo.Text.Trim(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            CH1_2 = new PdfPCell(new Phrase(lblBaseOn.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase((rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 4

            string Site_Name = string.Empty;
            if (rdoAllSite.Checked)
                Site_Name = lblSite.Text.Trim();
            else
                Site_Name = "Selected Site(s)";

            CH1_2 = new PdfPCell(new Phrase(Site_Name, paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            string Site = string.Empty;
            if (rdoAllSite.Checked == true)
                Site = "All Sites";
            else
            {
                string Selsites = string.Empty;
                if (Sel_REFS_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Sel_REFS_List)
                    {
                        Selsites += Entity.SiteNUMBER + ", ";
                    }
                }
                Site = Selsites;
            }

            CB1_2 = new PdfPCell(new Phrase(Site, paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            CH1_2 = new PdfPCell(new Phrase(lblIncomeStatus.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            string Income = rdoIncElig.Text;
            if (rdoIncCerti.Checked == true)
                Income = rdoIncCerti.Text;
            else if (rbOverInc.Checked == true)
                Income = rbOverInc.Text;
            else if (rbIncAll.Checked == true)
                Income = rbIncAll.Text;
            else if (rb101.Checked == true)
                Income = rb101.Text;
            else if (rbCategorical.Checked == true)
                Income = rbCategorical.Text;

            CB1_2 = new PdfPCell(new Phrase(Income, paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            CH1_2 = new PdfPCell(new Phrase(lblCategorical.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Text.ToString().Trim(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            if (rbDayCare.Checked == false)
            {
                CH1_2 = new PdfPCell(new Phrase(lblIncludeClients.Text.Trim(), paramsCellStyle));
                CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                Headertable.AddCell(CH1_2);

                string Include_Clients = rdoIncElig.Text;
                if (rbBoth.Checked == true)
                    Include_Clients = "Both " + rbIntakeCmpltd.Text.Trim() + " and " + rbWaitlist.Text.Trim();
                else if (rbIntakeCmpltd.Checked == true)
                    Include_Clients = rbIntakeCmpltd.Text;
                else if (rbWaitlist.Checked == true)
                    Include_Clients = rbWaitlist.Text;

                CB1_2 = new PdfPCell(new Phrase(Include_Clients, paramsCellStyle));
                CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Headertable.AddCell(CB1_2);

                CH1_2 = new PdfPCell(new Phrase(lblLabels.Text.Trim(), paramsCellStyle));
                CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                Headertable.AddCell(CH1_2);

                CB1_2 = new PdfPCell(new Phrase((rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text), paramsCellStyle));
                CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Headertable.AddCell(CB1_2);

                if (rbMailLbl.Checked)
                {
                    CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CS1.HorizontalAlignment = Element.ALIGN_LEFT;
                    CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(CS1);

                    CH1_2 = new PdfPCell(new Phrase(lblPrintlbl.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((rbChild.Checked == true ? rbChild.Text : rbParent.Text), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);

                    CH1_2 = new PdfPCell(new Phrase(chkbExcel.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((chkbExcel.Checked == true ? "Yes" : "No"), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);
                }
                else
                {
                    CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CS1.HorizontalAlignment = Element.ALIGN_LEFT;
                    CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(CS1);

                    CH1_2 = new PdfPCell(new Phrase(chkbExcel.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((chkbExcel.Checked == true ? "Yes" : "No"), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);
                }
            }
            else
            {
                CH1_2 = new PdfPCell(new Phrase(lblLabels.Text.Trim(), paramsCellStyle));
                CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                Headertable.AddCell(CH1_2);

                CB1_2 = new PdfPCell(new Phrase((rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text), paramsCellStyle));
                CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Headertable.AddCell(CB1_2);

                if (rbMailLbl.Checked)
                {
                    CH1_2 = new PdfPCell(new Phrase(chkbExcel.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((chkbExcel.Checked == true ? "Yes" : "No"), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);

                    CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CS1.HorizontalAlignment = Element.ALIGN_LEFT;
                    CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(CS1);

                    CH1_2 = new PdfPCell(new Phrase(lblPrintlbl.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((rbChild.Checked == true ? rbChild.Text : rbParent.Text), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);
                }
                else
                {
                    CH1_2 = new PdfPCell(new Phrase(chkbExcel.Text.Trim(), paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase((chkbExcel.Checked == true ? "Yes" : "No"), paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);

                    CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CS1.HorizontalAlignment = Element.ALIGN_LEFT;
                    CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(CS1);

                    CH1_2 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CH1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
                    Headertable.AddCell(CH1_2);

                    CB1_2 = new PdfPCell(new Phrase("", paramsCellStyle));
                    CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    CB1_2.Border = iTextSharp.text.Rectangle.BOX;
                    CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    Headertable.AddCell(CB1_2);
                }
            }
           

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By: ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On: ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private void ParametersPage(DevExpress.Spreadsheet.Workbook wb, DevExpress_Excel_Properties oDevExpress_Excel_Properties)
        {

            DevExpress.Spreadsheet.Worksheet paramSheet = wb.Worksheets[0];
            paramSheet.Name = "Parameters";
            paramSheet.ActiveView.TabColor = Color.ForestGreen;
            paramSheet.ActiveView.ShowGridlines = false;
            wb.Unit = DevExpress.Office.DocumentUnit.Point;

            paramSheet.Columns[1].Width = 100;
            paramSheet.Columns[2].Width = 200;
            paramSheet.Columns[3].Width = 50;
            paramSheet.Columns[4].Width = 100;
            paramSheet.Columns[5].Width = 200;

            int _Rowindex = 0;
            int _Columnindex = 0;

            string strAgy = Current_Hierarchy_DB.Split('-')[0];

            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

            string ImgName = "";
            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = BaseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            _Rowindex = 1;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = BAgyControlDetails.AgyName;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xTitleCellstyle;
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            _Rowindex++;

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;
            DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, wb);
            //DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 200, 80, 630, 280);
            DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 50, 0, 120, 70);


            AgencyControlEntity _agycntrldets = new AgencyControlEntity();
            _agycntrldets = BaseForm.BaseAgencyControlDetails;

            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                _agycntrldets = BAgyControlDetails;
            else
                _agycntrldets = BaseForm.BaseAgencyControlDetails;

            string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
            string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
            string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
            string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');

            string strAddress = street + city + state + zip1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = strAddress;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.reportNameStyle;
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Report Parameters";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xTitleCellstyle2;
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string program_year = string.Empty;
            if (Current_Hierarchy.Substring(0, 2) != "**")
                Agy = Current_Hierarchy.Substring(0, 2);
            if (Current_Hierarchy.Substring(2, 2) != "**")
                Dept = Current_Hierarchy.Substring(2, 2);
            if (Current_Hierarchy.Substring(4, 2) != "**")
                Prg = Current_Hierarchy.Substring(4, 2);
            if (CmbYear.Visible == true)
                program_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                _Rowindex++;
                paramSheet.Rows[_Rowindex][_Columnindex].Value = Txt_HieDesc.Text.Trim() + "   " + program_year;
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xsubTitleintakeCellstyle;
                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            }
            else
            {
                _Rowindex++;
                paramSheet.Rows[_Rowindex][_Columnindex].Value = Txt_HieDesc.Text.Trim();
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xsubTitleintakeCellstyle;
                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            }

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblRepSeq.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdoSiteName.Checked ? rdoSiteName.Text.Trim() : rdoRank.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblActive.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbRankCtg.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbCasenotes.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbCasenotes.Checked ? "Yes" : "No";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            string Fund_Label = string.Empty;
            if (rbFundAll.Checked == true)
                Fund_Label = lblFundSource.Text.Trim();
            else if (rbFundSel.Text == "Selected Funding" && rbFundSel.Checked)
                Fund_Label = "Selected Funding Source(s)";
            else if (rbDayCare.Checked)
                Fund_Label = rbDayCare.Text.Trim();
            else
                Fund_Label = rbFundSel.Text.Trim();

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Fund_Label;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string Fund = string.Empty;
            if (rbFundAll.Checked == true)
                Fund = rbFundAll.Text.Trim() + " Funds";
            else if (rbFundSel.Text == "Selected Funding" && rbFundSel.Checked)
                Fund = strFundingCodes;
            else if (rbDayCare.Checked)
                Fund = strFundingCodes;
            else
                Fund = strFundingCodes;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Fund;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblChildAge.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "From: " + txtFrom.Text.Trim() + "  To: " + txtTo.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblBaseOn.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text);
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            string Site_Name = string.Empty;
            if (rdoAllSite.Checked)
                Site_Name = lblSite.Text.Trim();
            else
                Site_Name = "Selected Site(s)";

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Site_Name;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string Site = string.Empty;
            if (rdoAllSite.Checked == true)
                Site = "All Sites";
            else
            {
                string Selsites = string.Empty;
                if (Sel_REFS_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Sel_REFS_List)
                    {
                        Selsites += Entity.SiteNUMBER + ", ";
                    }
                }
                Site = Selsites;
            }

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Site;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblIncomeStatus.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string Income = rdoIncElig.Text;
            if (rdoIncCerti.Checked == true)
                Income = rdoIncCerti.Text;
            else if (rbOverInc.Checked == true)
                Income = rbOverInc.Text;
            else if (rbIncAll.Checked == true)
                Income = rbIncAll.Text;
            else if (rb101.Checked == true)
                Income = rb101.Text;
            else if (rbCategorical.Checked == true)
                Income = rbCategorical.Text;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Income;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblCategorical.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbCategorical.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            if (rbDayCare.Checked == false)
            {
                paramSheet.Rows[_Rowindex][_Columnindex].Value = lblIncludeClients.Text.Trim();
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                _Columnindex++;

                string Include_Clients = rdoIncElig.Text;
                if (rbBoth.Checked == true)
                    Include_Clients = "Both " + rbIntakeCmpltd.Text.Trim() + " and " + rbWaitlist.Text.Trim();
                else if (rbIntakeCmpltd.Checked == true)
                    Include_Clients = rbIntakeCmpltd.Text;
                else if (rbWaitlist.Checked == true)
                    Include_Clients = rbWaitlist.Text;

                paramSheet.Rows[_Rowindex][_Columnindex].Value = Include_Clients;
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                _Columnindex++;

                _Rowindex++;
                _Columnindex = 1;

                paramSheet.Rows[_Rowindex][_Columnindex].Value = lblLabels.Text.Trim();
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                _Columnindex++;

                paramSheet.Rows[_Rowindex][_Columnindex].Value = (rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text);
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                _Columnindex++;

                if (rbMailLbl.Checked)
                {
                    paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = lblPrintlbl.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (rbChild.Checked == true ? rbChild.Text : rbParent.Text);
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;

                    _Rowindex++;
                    _Columnindex = 1;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbExcel.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (chkbExcel.Checked == true ? "Yes" : "No");
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;
                }
                else
                {
                    paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbExcel.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (chkbExcel.Checked == true ? "Yes" : "No");
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;
                }
            }
            else
            {
                paramSheet.Rows[_Rowindex][_Columnindex].Value = lblLabels.Text.Trim();
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                _Columnindex++;

                paramSheet.Rows[_Rowindex][_Columnindex].Value = (rbNoLbl.Checked == true ? rbNoLbl.Text : rbMailLbl.Text);
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                _Columnindex++;

                if (rbMailLbl.Checked)
                {
                    _Rowindex++;
                    _Columnindex = 1;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbExcel.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (chkbExcel.Checked == true ? "Yes" : "No");
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = lblPrintlbl.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (rbChild.Checked == true ? rbChild.Text : rbParent.Text);
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;
                }
                else
                {
                    _Rowindex++;
                    _Columnindex = 1;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = chkbExcel.Text.Trim();
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = (chkbExcel.Checked == true ? "Yes" : "No");
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;

                    paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                    paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                    _Columnindex++;
                }
            }

            _Rowindex++;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            _Columnindex = _Columnindex + 5;

            _Rowindex = _Rowindex + 3;
            _Columnindex = 5;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated By: " + BaseForm.UserID;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

        }

        #endregion

        private void ExcelFormats_DevExpress(object sender, FormClosedEventArgs e)
        {

            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild

                Random_Filename = null;
                string xlFileName = form.GetFileName();
                xlFileName = propReportPath + BaseForm.UserID + "\\" + xlFileName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay("Error");
                }

                try
                {
                    string Tmpstr = xlFileName + ".xlsx";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = xlFileName + newFileName.Substring(0, length) + ".xlsx";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    xlFileName = Random_Filename;
                else
                    xlFileName += ".xlsx";

                string _excelPath = xlFileName;

                #endregion

                string data = null;
                int i = 0;
                int j = 0;

                if (Agency == "**")
                    Agency = null;
                if (Depart == "**")
                    Depart = null;
                if (Program == "**")
                    Program = null;
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                CaseSiteEntity SearchEntity = new CaseSiteEntity(true);
                SearchEntity.SiteROOM = "0000";
                SearchEntity.SiteAGENCY = Agency;
                SearchEntity.SiteDEPT = Depart;
                SearchEntity.SitePROG = Program;
                SearchEntity.SiteYEAR = Year;
                propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(SearchEntity, "Browse");

                int _Rowindex = 0;
                int _Columnindex = 0;

                try
                {
                    using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                    {
                        DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                        oDevExpress_Excel_Properties.sxlbook = wb;
                        oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                        oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                        oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                        ParametersPage(wb, oDevExpress_Excel_Properties);

                        DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets.Add("Fixed_Margin_Report");
                        SheetDetails.ActiveView.TabColor = ColorTranslator.FromHtml("#ADD8E6");

                        SheetDetails.ActiveView.ShowGridlines = false;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        #region Column Widths

                        SheetDetails.Columns[0].Width = 10;//Rank
                        SheetDetails.Columns[1].Width = 60;//Rank
                        SheetDetails.Columns[2].Width = 80;//App No
                        SheetDetails.Columns[3].Width = 200;//Name
                        SheetDetails.Columns[4].Width = 90;//Gender
                        SheetDetails.Columns[5].Width = 90;//DOB
                        SheetDetails.Columns[6].Width = 110;//Age
                        SheetDetails.Columns[7].Width = 200;//Address 1
                        SheetDetails.Columns[8].Width = 150;//Address 2
                        SheetDetails.Columns[9].Width = 80;//Phone No

                        SheetDetails.Columns[10].Width = 90;//Class Preferred
                        SheetDetails.Columns[11].Width = 80;//Site
                        SheetDetails.Columns[12].Width = 120;//Income Desc
                        SheetDetails.Columns[13].Width = 100;//Eligibility Date
                        SheetDetails.Columns[14].Width = 90;//Eligible Meals

                        SheetDetails.Columns[15].Width = 100;//Household Income
                        SheetDetails.Columns[16].Width = 100;//Household Size
                        SheetDetails.Columns[17].Width = 120;//Trans
                        SheetDetails.Columns[18].Width = 90;//Expected Fund
                        SheetDetails.Columns[19].Width = 90;//Alternate Fund

                        SheetDetails.Columns[20].Width = 150;//Parent/Guardian
                        SheetDetails.Columns[21].Width = 80;//Work Phone
                        SheetDetails.Columns[22].Width = 10;

                        #endregion

                        #region Excel Data Printing

                        bool first = true;
                        string Priv_Site = string.Empty;
                        string Priv_Desc = string.Empty;
                        string Site_Desc = string.Empty;
                        CaseSiteEntity Sel_Site = new CaseSiteEntity();
                        int k = 0;

                        _Rowindex = 0;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName.Trim().ToUpper();
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameStyleL);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex = _Columnindex + 21;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                        for (i = 0; i <= SelRecords.Count - 1; i++)
                        {
                            #region Column Headers

                            if (rdoSiteName.Checked == true)
                            {
                                if (rbIntakeCmpltd.Checked == false ? SelRecords[i].EnrlSite.Trim() != Priv_Site : SelRecords[i].Site.Trim() != Priv_Site)
                                {
                                    if (propCaseSiteEntity.Count > 0)
                                    {
                                        if (rbWaitlist.Checked || rbBoth.Checked)
                                            Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(SelRecords[i].EnrlSite.Trim()) && u.SiteYEAR.Equals(Year));
                                        else
                                            Sel_Site = propCaseSiteEntity.Find(u => u.SiteNUMBER.Equals(SelRecords[i].Site.Trim()) && u.SiteYEAR.Equals(Year));

                                        if (Sel_Site != null)
                                            Site_Desc = Sel_Site.SiteNAME.Trim();
                                        else
                                            Site_Desc = "";
                                    }

                                    if (!first)
                                    {
                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                        _Rowindex = _Rowindex+ + 2;
                                        _Columnindex = 1;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                        _Columnindex = _Columnindex + 5;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                        _Rowindex++;
                                        _Columnindex = 1;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "All applicants for site " + Priv_Site + "  " + Priv_Desc;
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameBlockStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex = _Columnindex + 5;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                        _Rowindex++;
                                        _Columnindex = 1;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                                        _Columnindex = _Columnindex + 5;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;

                                        _Rowindex++;
                                        _Columnindex = 1;

                                        #region Column Headers 1

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site Totals";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Eligible";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Over Income";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "In Certify";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "101% - 130%";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Categorical";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                                        #endregion

                                        if (propfundingSource.Count > 0)
                                        {
                                            foreach (CommonEntity FundEntity in propfundingSource)
                                            {
                                                List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>();
                                                List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                                List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
                                                List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                                List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                                if (rbWaitlist.Checked || rbBoth.Checked)
                                                {
                                                    IEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                                    OIentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                                    ICentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                                    Pentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                                    CAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                                }
                                                else
                                                {
                                                    IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                                    OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                                    ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                                    Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                                    CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                                }

                                                _Rowindex++;
                                                _Columnindex = 1;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = FundEntity.Desc.Trim();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = IEentity.Count.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = OIentity.Count.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = ICentity.Count.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = Pentity.Count.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAentity.Count.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                //_Columnindex++;

                                            }

                                            List<HSSB2115Entity> SEentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> SOentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> SCentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> SPentity = new List<HSSB2115Entity>();
                                            List<HSSB2115Entity> SCAentity = new List<HSSB2115Entity>();

                                            if (rbBoth.Checked || rbWaitlist.Checked)
                                            {
                                                SEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                                SOentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                                SCentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                                SPentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                                SCAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                            }
                                            else
                                            {
                                                SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                                SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                                SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                                SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                                SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                            }

                                            _Rowindex++;
                                            _Columnindex = 1;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "No Fund Specified";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Bold = true;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SEentity.Count.ToString();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SOentity.Count.ToString();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCentity.Count.ToString();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SPentity.Count.ToString();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                            _Columnindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCAentity.Count.ToString();
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                            _Rowindex++;
                                            _Columnindex = 1;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                            _Columnindex = _Columnindex + 5;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                        }

                                        _Rowindex = _Rowindex + 2;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;

                                        string SiteName = string.Empty;
                                        if (rbWaitlist.Checked || rbBoth.Checked)
                                            SiteName = "All applicants for site " + SelRecords[i].EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                        else
                                            SiteName = "All applicants for site " + SelRecords[i].Site.Trim() + "  " + Site_Desc.Trim();

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = SiteName;
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameBlockStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;

                                        #region Column Headers 2

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Rank";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App No";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Applicant Name";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Gender";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "DOB";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Age";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 1";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 2";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Phone No";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Class Preferred";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Description";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligibility Date";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligible Meals";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Income";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Size";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Trans";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Expected Fund";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Alternate Fund";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Parent/Guardian";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Work Phone";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                                        #endregion
                                    }
                                    else
                                    {
                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;

                                        string SiteName = string.Empty;
                                        if (rbWaitlist.Checked || rbBoth.Checked)
                                            SiteName = "All applicants for site " + SelRecords[i].EnrlSite.Trim() + "  " + Site_Desc.Trim();
                                        else
                                            SiteName = "All applicants for site " + SelRecords[i].Site.Trim() + "  " + Site_Desc.Trim();

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = SiteName;
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameBlockStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;

                                        #region Column Headers 2

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Rank";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App No";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Applicant Name";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Gender";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "DOB";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Age";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 1";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 2";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Phone No";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Class Preferred";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Description";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligibility Date";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligible Meals";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Income";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Size";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Trans";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Expected Fund";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Alternate Fund";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Parent/Guardian";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Work Phone";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                                        #endregion

                                        first = false;
                                    }
                                }
                            }

                            #endregion

                            if (rdoRank.Checked == true)
                            {
                                #region Column Headers 2

                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Rank";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App No";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Applicant Name";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Gender";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "DOB";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Age";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 1";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Address 2";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Phone No";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Class Preferred";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Description";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligibility Date";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Eligible Meals";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Income";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Household Size";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Trans";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Expected Fund";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Alternate Fund";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Parent/Guardian";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Work Phone";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 25;

                                #endregion
                            }

                            #region Printing Data

                            _Rowindex++;
                            _Columnindex = 0;

                            data = SelRecords[i].Rank.Trim();

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = data;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].AppNo.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = (SelRecords[i].Disable.Trim() == "Y" ? "*" : "") + SelRecords[i].Fname.Trim() + "  " + SelRecords[i].Mname.Trim() + "  " + SelRecords[i].Lname.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string Sex = string.Empty;
                            string Class = string.Empty;
                            if (SelRecords[i].Sex.Trim() == "M")
                                Sex = "MALE";
                            else
                                Sex = "FEMALE";

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = Sex;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(SelRecords[i].DOB.Trim());
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].Age.Trim() + " Years  " + SelRecords[i].Months.Trim() + " Months";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string Apt = string.Empty;
                            string Floor = string.Empty;
                            if (!string.IsNullOrEmpty(SelRecords[i].Apt.Trim()))
                                Apt = "  Apt  " + SelRecords[i].Apt.Trim();
                            if (!string.IsNullOrEmpty(SelRecords[i].Flr.Trim()))
                                Apt = "  Flr  " + SelRecords[i].Flr.Trim();

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].Hno.Trim() + "   " + SelRecords[i].Street.Trim() + " " + SelRecords[i].Suffix.Trim() + Apt + Floor;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].City.Trim() + " " + SelRecords[i].State.Trim() + "  " + SelRecords[i].Zip.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            MaskedTextBox mskPhn = new MaskedTextBox();
                            mskPhn.Mask = "(999) 000-0000";
                            if (!string.IsNullOrEmpty(SelRecords[i].Phone.Trim()))
                                mskPhn.Text = SelRecords[i].Phone.Trim();
                            else
                                mskPhn.Mask = "";

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = mskPhn.Text.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            if (SelRecords[i].ClassPrefer.Trim() == "A")
                                Class = "AM";
                            else if (SelRecords[i].ClassPrefer.Trim() == "P")
                                Class = "PM";
                            else if (SelRecords[i].Classfication.Trim() == "E")
                                Class = "EXTD";
                            else if (SelRecords[i].ClassPrefer.Trim() == "F")
                                Class = "FULL";

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = Class;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string Income_Desc = string.Empty;
                            if (SelRecords[i].Classfication.Trim() == "98" || SelRecords[i].Classfication.Trim() == "95")
                                Income_Desc = "Over Inc.";
                            else if (SelRecords[i].Classfication.Trim() == "99")
                                Income_Desc = "Eligible";
                            else
                                Income_Desc = "In Certify";

                            if (rdoSiteName.Checked == true)
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            else
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].Site.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = Income_Desc;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(SelRecords[i].Elig_Date.Trim());
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string Meals_Type = string.Empty;
                            if (SelRecords[i].MealElig == "1")
                                Meals_Type = "Free Meals";
                            else if (SelRecords[i].MealElig == "2")
                                Meals_Type = "Reduced Meals";
                            else if (SelRecords[i].MealElig == "3")
                                Meals_Type = "Paid Meals";

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = Meals_Type;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.GetAmountSep(SelRecords[i].Fam_Income.Trim());
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].Family_count.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            if (SelRecords[i].Transport.Trim() == "None")
                            {
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "No Transportation";
                            }
                            else
                            {
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].Transport.Trim();
                            }
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string Fund = string.Empty;
                            if (!string.IsNullOrEmpty(SelRecords[i].CHLDMST_FUND.Trim()))
                            {
                                foreach (CommonEntity drFund in propfundingSource)
                                {
                                    if (SelRecords[i].CHLDMST_FUND.Trim() == drFund.Code.ToString().Trim())
                                    {
                                        Fund = drFund.Desc.ToString().Trim();
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Fund;
                                        break;
                                    }
                                }
                            }
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            string AltFund = string.Empty;
                            if (!string.IsNullOrEmpty(SelRecords[i].Alt_FUND.Trim()))
                            {
                                foreach (CommonEntity drFund in propfundingSource)
                                {
                                    if (SelRecords[i].Alt_FUND.Trim() == drFund.Code.ToString().Trim())
                                    {
                                        AltFund = drFund.Desc.ToString().Trim();
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = AltFund;
                                        break;
                                    }
                                }
                            }
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = SelRecords[i].G1_FName.Trim() + " " + SelRecords[i].G1_LName.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            MaskedTextBox mskWPhn = new MaskedTextBox();
                            mskWPhn.Mask = "(999) 000-0000";
                            if (!string.IsNullOrEmpty(SelRecords[i].Empl_Phone.Trim()))
                                mskWPhn.Text = SelRecords[i].Phone.Trim();
                            else
                                mskWPhn.Mask = "";

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = mskWPhn.Text.Trim();
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            #endregion

                            if (rdoRank.Checked == true)
                            {
                                if (Sel_Ranklist.Count > 0)
                                {
                                    int TempR_Cnt = Sel_Ranklist.Count;
                                    string Rank1_Desc = string.Empty, Rank2_Desc = string.Empty, Rank3_Desc = string.Empty, Rank4_Desc = string.Empty, Rank5_Desc = string.Empty, Rank6_Desc = string.Empty;
                                    string Rank1_Det = "0", Rank2_Det = "0", Rank3_Det = "0", Rank4_Det = "0", Rank5_Det = "0", Rank6_Det = "0";
                                    
                                    for (int m = 0; m < Sel_Ranklist.Count; m++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank1.Trim()) && SelRecords[i].Rank1.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[0].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank1_Desc = Sel_Ranklist[0].Desc.Trim();
                                                        Rank1_Det = SelRecords[i].Rank1.Trim();
                                                    }
                                                }
                                                break;
                                            case 1:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank2.Trim()) && SelRecords[i].Rank2.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[1].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank2_Desc = Sel_Ranklist[1].Desc.Trim();
                                                        Rank2_Det = SelRecords[i].Rank2.Trim();
                                                    }
                                                }
                                                break;
                                            case 2:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank3.Trim()) && SelRecords[i].Rank3.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[2].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank3_Desc = Sel_Ranklist[2].Desc.Trim();
                                                        Rank3_Det = SelRecords[i].Rank1.Trim();
                                                    }
                                                }
                                                break;
                                            case 3:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank4.Trim()) && SelRecords[i].Rank4.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[3].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank4_Desc = Sel_Ranklist[3].Desc.Trim();
                                                        Rank4_Det = SelRecords[i].Rank4.Trim();
                                                    }
                                                }
                                                break;
                                            case 4:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank5.Trim()) && SelRecords[i].Rank5.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[4].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank5_Desc = Sel_Ranklist[4].Desc.Trim();
                                                        Rank5_Det = SelRecords[i].Rank5.Trim();
                                                    }
                                                }
                                                break;
                                            case 6:
                                                if (!string.IsNullOrEmpty(SelRecords[i].Rank6.Trim()) && SelRecords[i].Rank6.Trim() != "0")
                                                {
                                                    if (Sel_Ranklist[5].HeadStrt.Trim() != "N")
                                                    {
                                                        Rank6_Desc = Sel_Ranklist[5].Desc.Trim();
                                                        Rank6_Det = SelRecords[i].Rank6.Trim();
                                                    }
                                                }
                                                break;
                                        }
                                    }

                                    int TotR = int.Parse(Rank1_Det) + int.Parse(Rank2_Det) + int.Parse(Rank3_Det) + int.Parse(Rank4_Det) + int.Parse(Rank5_Det) + int.Parse(Rank6_Det);


                                    if (string.IsNullOrEmpty(Rank1_Desc) && Rank1_Det == "0")
                                        Rank1_Det = "";
                                    if (string.IsNullOrEmpty(Rank2_Desc) && Rank2_Det == "0")
                                        Rank2_Det = "";
                                    if (string.IsNullOrEmpty(Rank3_Desc) && Rank3_Det == "0")
                                        Rank3_Det = "";
                                    if (string.IsNullOrEmpty(Rank4_Desc) && Rank4_Det == "0")
                                        Rank4_Det = "";
                                    if (string.IsNullOrEmpty(Rank5_Desc) && Rank5_Det == "0")
                                        Rank5_Det = "";
                                    if (string.IsNullOrEmpty(Rank6_Desc) && Rank6_Det == "0")
                                        Rank6_Det = "";

                                    string Total_Desc = string.Empty;
                                    if (TotR == 0)
                                        Total_Desc = "Ranking Total: " + TotR;
                                    else
                                    {
                                        Total_Desc = Rank1_Desc + "   " + Rank1_Det.ToString() + "    " + Rank2_Desc + "    " + Rank2_Det.ToString() + "    " + Rank3_Desc + " " + Rank3_Det.ToString()
                                        + "    " + Rank4_Desc + "    " + Rank4_Det.ToString() + "    " + Rank5_Desc + "    " + Rank5_Det.ToString() + "    " + Rank6_Desc + "    " + Rank6_Det.ToString() + " Total:  " + TotR;
                                    }

                                    _Rowindex++;
                                    _Columnindex = 0;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = Total_Desc;
                                    oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex = _Columnindex + 21;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                }
                            }

                            string Spl_Desc = string.Empty;

                            if (!string.IsNullOrEmpty(SelRecords[i].CasecondApp.Trim()))
                            {
                                if (!string.IsNullOrEmpty(SelRecords[i].Allergy.Trim()))
                                    Spl_Desc = SelRecords[i].Allergy.Trim();
                                else if (!string.IsNullOrEmpty(SelRecords[i].DietRestrct.Trim()))
                                    Spl_Desc = SelRecords[i].DietRestrct.Trim();
                                else if (!string.IsNullOrEmpty(SelRecords[i].Medications.Trim()))
                                    Spl_Desc = SelRecords[i].Medications.Trim();
                                else if (!string.IsNullOrEmpty(SelRecords[i].MedConds.Trim()))
                                    Spl_Desc = SelRecords[i].MedConds.Trim();
                                else if (!string.IsNullOrEmpty(SelRecords[i].HHConcerns.Trim()))
                                    Spl_Desc = SelRecords[i].HHConcerns.Trim();
                                else if (!string.IsNullOrEmpty(SelRecords[i].DevlConcerns.Trim()))
                                    Spl_Desc = SelRecords[i].DevlConcerns.Trim();

                                string Disability = string.Empty;
                                if (!string.IsNullOrEmpty(SelRecords[i].Disability_type.Trim()))
                                    Disability = SelRecords[i].Disability_type.Trim() + "\r\n";

                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = Disability + Spl_Desc;
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex = _Columnindex + 21;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex = _Columnindex + 21;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            }

                            if (string.IsNullOrEmpty(SelRecords[i].EnrlStatus.Trim()))
                            {
                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "INTAKE COMPLETED ....missing status record!";
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex = _Columnindex + 21;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex = _Columnindex + 21;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            }

                            if (chkbCasenotes.Checked == true)
                            {
                                List<CaseNotesEntity> caseNotesEntity = _model.TmsApcndata.GetCaseNotesWaitList(SelRecords[i].Agy + SelRecords[i].Dept + SelRecords[i].Prog + SelRecords[i].Year + SelRecords[i].AppNo);

                                if (caseNotesEntity.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(caseNotesEntity[0].Data.Trim()))
                                    {
                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = caseNotesEntity[0].Data.Trim();
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                        _Rowindex++;
                                        _Columnindex = 0;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex = _Columnindex + 21;

                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    }
                                }
                            }

                            Priv_Site = SelRecords[i].Site.Trim();
                            Priv_Desc = Site_Desc.Trim();
                        }
                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "©2024 CAPSystems Inc";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 21, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex = _Columnindex + 21;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                        if (rdoSiteName.Checked == true)
                        {
                            _Rowindex = _Rowindex + 2;
                            _Columnindex = 1;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                            _Columnindex = _Columnindex + 5;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                            _Rowindex++;
                            _Columnindex = 1;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "All applicants for site " + Priv_Site + "  " + Priv_Desc;
                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameBlockStyleL);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex = _Columnindex + 5;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                            _Rowindex++;
                            _Columnindex = 1;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;
                            _Columnindex = _Columnindex + 5;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 3;

                            _Rowindex++;
                            _Columnindex = 1;

                            #region Column Headers 1

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site Totals";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Eligible";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Over Income";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "In Certify";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "101% - 130%";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Categorical";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                            #endregion

                            if (propfundingSource.Count > 0)
                            {
                                foreach (CommonEntity FundEntity in propfundingSource)
                                {
                                    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                    _Rowindex++;
                                    _Columnindex = 1;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = FundEntity.Desc.Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    if (rbWaitlist.Checked || rbBoth.Checked)
                                    {
                                        IEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                        OIentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                        ICentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                        Pentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                        CAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                    }
                                    else
                                    {
                                        IEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                        OIentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                        ICentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                        Pentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                        CAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = IEentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = OIentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = ICentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = Pentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                }

                                List<HSSB2115Entity> SEentity = new List<HSSB2115Entity>();
                                List<HSSB2115Entity> SOentity = new List<HSSB2115Entity>();
                                List<HSSB2115Entity> SCentity = new List<HSSB2115Entity>();
                                List<HSSB2115Entity> SPentity = new List<HSSB2115Entity>();
                                List<HSSB2115Entity> SCAentity = new List<HSSB2115Entity>();

                                if (rbBoth.Checked || rbWaitlist.Checked)
                                {
                                    SEentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                    SOentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                    SCentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                    SPentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                    SCAentity = SelRecords.FindAll(u => u.EnrlSite.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                }
                                else
                                {
                                    SEentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                    SOentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                    SCentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                    SPentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                    SCAentity = SelRecords.FindAll(u => u.Site.Trim().Equals(Priv_Site.Trim()) && u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));
                                }

                                _Rowindex++;
                                _Columnindex = 1;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "No Fund Specified";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Bold = true;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SEentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SOentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SPentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCAentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                _Rowindex++;
                                _Columnindex = 1;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                _Columnindex = _Columnindex + 5;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            }

                            _Rowindex = _Rowindex + 2;
                            _Columnindex = 1;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                            _Columnindex = _Columnindex + 5;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                            _Rowindex++;
                            _Columnindex = 1;

                            #region Column Headers 1

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Report Totals";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Income Eligible";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Over Income";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "In Certify";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "101% - 130%";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Categorical";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                            #endregion

                            if (propfundingSource.Count > 0)
                            {
                                foreach (CommonEntity FundEntity in propfundingSource)
                                {
                                    List<HSSB2115Entity> IEentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> OIentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> ICentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> Pentity = new List<HSSB2115Entity>();
                                    List<HSSB2115Entity> CAentity = new List<HSSB2115Entity>();

                                    _Rowindex++;
                                    _Columnindex = 1;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = FundEntity.Desc.Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    IEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("99"));
                                    OIentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                    ICentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("00"));
                                    Pentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("96"));
                                    CAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals(FundEntity.Code.Trim()) && u.Classfication.Trim().Equals("97"));

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = IEentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = OIentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = ICentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = Pentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = CAentity.Count.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                }

                                List<HSSB2115Entity> SEentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("99"));
                                List<HSSB2115Entity> SOentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && (u.Classfication.Trim().Equals("98") || u.Classfication.Trim().Equals("95")));
                                List<HSSB2115Entity> SCentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("00"));
                                List<HSSB2115Entity> SPentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("96"));
                                List<HSSB2115Entity> SCAentity = SelRecords.FindAll(u => u.CHLDMST_FUND.Trim().Equals("") && u.Classfication.Trim().Equals("97"));

                                _Rowindex++;
                                _Columnindex = 1;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "No Fund Specified";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Bold = true;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SEentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SOentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SPentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = SCAentity.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                                _Rowindex++;
                                _Columnindex = 1;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.LeftBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                                _Columnindex = _Columnindex + 5;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.LineStyle = DevExpress.Spreadsheet.BorderLineStyle.Thick;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Borders.RightBorder.Color = System.Drawing.ColorTranslator.FromHtml("#1A4777");
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                            }
                        }

                        SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                        #endregion

                        #region File Saving and Downloading

                        wb.Sheets.ActiveSheet = wb.Worksheets[0];
                        wb.SaveDocument(xlFileName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                        //try
                        //{
                        //    string localFilePath = xlFileName;

                        //    FileInfo fiDownload = new FileInfo(localFilePath);

                        //    string name = fiDownload.Name;
                        //    using (FileStream fileStream = fiDownload.OpenRead())
                        //    {
                        //        Application.Download(fileStream, name);
                        //    }
                        //}
                        //catch (Exception ex)
                        //{

                        //}

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                }


            }
        }

        #endregion
    }
}