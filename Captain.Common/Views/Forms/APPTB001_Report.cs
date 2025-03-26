#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using Wisej.Web;
#endregion


namespace Captain.Common.Views.Forms
{
    public partial class APPTB001_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public APPTB001_Report(BaseForm baseForm, PrivilegeEntity privileges)
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
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = Privileges.PrivilegeName.Trim();

            FillSiteCombo();
        }
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }

        #endregion


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
                //rdoMultipleSites.Enabled = true;
            }
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(776, 25);
                //rdoMultipleSites.Enabled = false;
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
            FillSiteCombo();    //Added by vikash on 03/29/2023 for Paramter Report Cleaning
            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(689, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(776, 25);
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

        private void FillSiteCombo()
        {
            cmbSite.Items.Clear();
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency;
            Search_Entity.SiteROOM = "0000";
            cmbSite.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Sites", "****", " ", Color.Black));

            propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            propCaseSiteEntity = propCaseSiteEntity.OrderByDescending(u => u.SiteACTIVE.Trim()).ToList();
            if (propCaseSiteEntity.Count > 0)
            {
                propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteAGENCY.Equals(Agency) && u.SiteDEPT.Trim().Equals(string.Empty) && u.SitePROG.Trim().Equals(string.Empty) && u.SiteYEAR.Trim().Equals(string.Empty));



                foreach (CaseSiteEntity item in propCaseSiteEntity)
                {
                    //cmbSite.Items.Add(new Captain.Common.Utilities.ListItem(item.SiteNAME.Trim(), item.SiteNUMBER));
                    listItem.Add(new Captain.Common.Utilities.ListItem(item.SiteNAME.Trim(), item.SiteNUMBER, item.SiteACTIVE.Trim() , (item.SiteACTIVE.Trim().Equals("Y") ? Color.Black : Color.Red)));
                }               
            }
            //cmbSite.Items.Insert(0, new Captain.Common.Utilities.ListItem("All Sites", "****"));
            //cmbSite.SelectedIndex = 0;

            cmbSite.Items.AddRange(listItem.ToArray());
            cmbSite.SelectedIndex = 0;
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            //if (dtpStartDate.Checked == false)
            //{
            //    _errorProvider.SetError(dtpStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartDate.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtpStartDate, null);
            //}

            //if (dtpEndDate.Checked == false)
            //{
            //    _errorProvider.SetError(dtpEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndDate.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtpEndDate, null);
            //}
            if(rbDateRange.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtpStartDate.Text))
                {
                    _errorProvider.SetError(dtpStartDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Start Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpStartDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtpEndDate.Text))
                {
                    _errorProvider.SetError(dtpEndDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "End Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpEndDate, null);
                }
            }
            if (rbDateRange.Checked)
            {
                if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)
                {
                    _errorProvider.SetError(dtpEndDate, "End Date may not prior to Start Date".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                    //MessageBox.Show("End Date may not prior to Start Date", "TMSB0110", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    _errorProvider.SetError(dtpEndDate, null);
            }
            return (isValid);
        }


        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                //if (chkbExcel.Checked == true)
                //{
                //    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
                //    pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveFormClosed);
                //    pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveExcelClosed);
                //    pdfListForm.ShowDialog();
                //}
                //else
                //{
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
                //}
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
            if (ValidateForm())
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
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
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
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);
            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string Site = ((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString();

            string strShowTask = string.Empty;
            if (rbAll.Checked == true)
                strShowTask = "1";
            else if (rbDateRange.Checked == true)
                strShowTask = "2";
            else if (rbFuture.Checked == true)
                strShowTask = "3";

            string Summary = chkbSummary.Checked == true ? "Y" : "N";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program + "\" Site = \"" + Site +
                            "\" YEAR = \"" + Program_Year + "\" StartDate = \"" + dtpStartDate.Value.Date + "\" SUMMARY = \"" + Summary +
                                "\" EndDate = \"" + dtpEndDate.Value.Date + "\"  Sel_Type = \"" + strShowTask + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbSite, dr["Site"].ToString());

                chkbSummary.Checked = dr["SUMMARY"].ToString() == "Y" ? true : false;

                if (dr["Sel_Type"].ToString().Trim() == "1")
                    rbAll.Checked = true;
                else if (dr["Sel_Type"].ToString().Trim() == "2")
                {
                    rbDateRange.Checked = true;
                    dtpEndDate.Value = Convert.ToDateTime(dr["EndDate"]);
                    dtpEndDate.Enabled = true;
                    dtpStartDate.Value = Convert.ToDateTime(dr["StartDate"]);
                    dtpStartDate.Enabled = true;
                }
                else if (dr["Sel_Type"].ToString().Trim() == "3")
                    rbFuture.Checked = true;
                //CommonFunctions.SetComboBoxValue(cmbCounty, dr["county"].ToString());

            }
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();

        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";

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
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);

                }
            }
        }

        private void rbFuture_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDateRange.Checked)
            {
                dtpStartDate.Enabled = true; dtpEndDate.Enabled = true;
            }
            else
            {
                dtpStartDate.Enabled = false; dtpEndDate.Enabled = false;
            }
        }

        List<CaseSnpEntity> SnpData = new List<CaseSnpEntity>();
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";

        private void APPTB001_Report_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }

        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 795;
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

                //iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
                //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 9, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bf_Wingdings2 = BaseFont.CreateFont(Application.MapPath("~\\Fonts\\WINGDNG2.TTF"), BaseFont.WINANSI, BaseFont.EMBEDDED);

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font FontWingdings = new iTextSharp.text.Font(bf_Wingdings2, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#01a601")));

                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 8, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bf_Calibri, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.ITALIC);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                iTextSharp.text.Font TblFontWhite = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);





                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                //searchAgytabs.Tabs_Type = "08004";
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                List<APPTSCHEDULEEntity> ApptSelList = new List<APPTSCHEDULEEntity>();

                List<APPTSCHDHISTEntity> ApptSelHistList = new List<APPTSCHDHISTEntity>();

                //DataSet ds = DatabaseLayer.TMS00110DB.Browse_TMSAPPT(Agency + Depart + Program + Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty);

                List<APPTSCHEDULEEntity> TmsapptList = _model.TmsApcndata.GetAPPTSCHEDULEBrowse(Agency, Depart, Program, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                List<APPTSCHDHISTEntity> propAPPtSchedulelist = _model.TmsApcndata.GetAPPTSCHDHISTBrowse(Agency, Depart, Program, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                if (TmsapptList.Count > 0)
                {
                    if (((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString() != "****")
                        ApptSelList = TmsapptList.FindAll(u => u.Site.Equals(((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString()));
                    else
                        ApptSelList = TmsapptList;


                    if (rbDateRange.Checked)
                        ApptSelList = ApptSelList.FindAll(u => u.Agency.Equals(Agency) && u.Dept.Equals(Depart) && u.Program.Equals(Program) && u.Year.Trim().Equals(string.Empty) && Convert.ToDateTime(u.Date.Trim()) >= Convert.ToDateTime(dtpStartDate.Text.Trim()) && Convert.ToDateTime(u.Date.Trim()) <= Convert.ToDateTime(dtpEndDate.Text.Trim()));
                    else if (rbFuture.Checked)
                        ApptSelList = ApptSelList.FindAll(u => u.Agency.Equals(Agency) && u.Dept.Equals(Depart) && u.Program.Equals(Program) && u.Year.Trim().Equals(string.Empty) && Convert.ToDateTime(u.Date.Trim()) >= Convert.ToDateTime(DateTime.Today.ToShortDateString()));


                }

                if (propAPPtSchedulelist.Count > 0)
                {
                    if (((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString() != "****")
                        ApptSelHistList = propAPPtSchedulelist.FindAll(u => u.Site.Equals(((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString()));
                    else
                        ApptSelHistList = propAPPtSchedulelist;


                    if (rbDateRange.Checked)
                        ApptSelHistList = propAPPtSchedulelist.FindAll(u => u.Agency.Equals(Agency) && u.Dept.Equals(Depart) && u.Program.Equals(Program) && u.Year.Trim().Equals(string.Empty) && Convert.ToDateTime(u.Date.Trim()) >= Convert.ToDateTime(dtpStartDate.Text.Trim()) && Convert.ToDateTime(u.Date.Trim()) <= Convert.ToDateTime(dtpEndDate.Text.Trim()));
                    else if (rbFuture.Checked)
                        ApptSelHistList = propAPPtSchedulelist.FindAll(u => u.Agency.Equals(Agency) && u.Dept.Equals(Depart) && u.Program.Equals(Program) && u.Year.Trim().Equals(string.Empty) && Convert.ToDateTime(u.Date.Trim()) >= Convert.ToDateTime(DateTime.Today.ToShortDateString()));


                }

                try
                {
                    PrintHeaderPage(document, writer);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();
                    if (ApptSelList.Count > 0 || ApptSelHistList.Count > 0)
                    {

                        var SelTemplates = ApptSelList.GroupBy(n => new { n.TemplateID, n.Site }).Select(g => new { TemplateID = g.Key.TemplateID, g.Key.Site }).ToList();
                        //TMSB110SelList = TMSB110SelList.OrderBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Program).ThenBy(u => u.Year).ThenBy(u => u.Location).ThenBy(u => Convert.ToDateTime(u.Date)).ThenBy(u =>int.Parse(u.Time)).ThenBy(u => u.SlotNumber).ToList();

                        PdfPTable table = new PdfPTable(8);
                        table.TotalWidth = 750f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 18f, 45f, 45f, 20f, 10f, 20f, 20f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;

                        string PrivSite = string.Empty; string PrivDate = string.Empty;
                        bool First = true; string PrivTime = string.Empty; bool Time_Sw = false;
                        string PrivDateTime = string.Empty; bool Second = true; string PrivDtSite = string.Empty;
                        foreach (APPTSCHEDULEEntity Entity in ApptSelList)
                        {
                            if (Entity.Site.Trim() != PrivSite)
                            {
                                if (!First)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                    document.NewPage();
                                }
                                PrivSite = Entity.Site.Trim();
                                First = false; PrivDate = string.Empty;
                            }

                            if (Entity.Date.Trim() != PrivDate)
                            {
                                if (!Second)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }

                                string SiteName = string.Empty;
                                CaseSiteEntity SiteEntity = propCaseSiteEntity.Find(u => u.SiteAGENCY.Trim().Equals(Entity.Agency.Trim()) && u.SiteNUMBER.Trim().Equals(Entity.Site.Trim()));
                                if (SiteEntity != null)
                                    SiteName = SiteEntity.SiteNAME.Trim();

                                PdfPCell Head = new PdfPCell(new Phrase("Date: " + LookupDataAccess.Getdate(Entity.Date.Trim()), TblFontBold));
                                Head.HorizontalAlignment = Element.ALIGN_LEFT;
                                Head.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Head.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Head.Colspan = 2;
                                Head.FixedHeight = 15;
                                table.AddCell(Head);


                                PdfPCell Head1 = new PdfPCell(new Phrase("Site: " + Entity.Site.Trim() + " - " + SiteName.Trim(), TblFontBold));
                                Head1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Head1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                Head1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                // Head1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Head1.FixedHeight = 15;
                                Head1.Colspan = 6;
                                table.AddCell(Head1);


                                string[] Header = { "Time", "Name", "Address", "City", "St", "Telephone #", "Source", "Status" };
                                for (int i = 0; i < Header.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(Header[i], SubHeadFont));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = iTextSharp.text.Rectangle.BOX;
                                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));       // Column Sub-Headings Background
                                    cell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                    cell.FixedHeight = 15;
                                    table.AddCell(cell);
                                }
                                PrivDate = Entity.Date.Trim();
                                Second = false;
                            }
                            string Meridian = string.Empty;

                            int time_set = int.Parse(Entity.Time.Trim());
                            if (time_set > 1159)
                            {
                                Meridian = "PM";
                                if (time_set > 1259) time_set = time_set - 1200;
                            }
                            else Meridian = "AM";

                            string S_Time = string.Empty;
                            if (time_set >= 1000)
                                S_Time = time_set.ToString().Substring(0, 2) + ":" + time_set.ToString().Substring(2, 2);
                            else
                            {
                                if (time_set.ToString().Length > 2)
                                    S_Time = "0" + time_set.ToString().Substring(0, 1) + ":" + time_set.ToString().Substring(1, 2);
                                else {
                                    if (time_set.ToString() == "0")
                                        S_Time = "00:00";
                                    else
                                        S_Time = "00:" + ("00".Substring(0, 2 - time_set.ToString().Length) + time_set.ToString()); 
                                }
                            }

                            if (S_Time + Meridian == PrivTime)
                            {
                                if (Entity.Date.Trim() == PrivDateTime && Entity.Site.Trim() == PrivDtSite)
                                    Time_Sw = true;
                                else Time_Sw = false;
                            }
                            else Time_Sw = false;

                            if (!Time_Sw)
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase(S_Time + Meridian, TableFont));
                                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                                R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                R1.BorderColor = BaseColor.WHITE;
                                R1.FixedHeight = 15f;
                                table.AddCell(R1);
                            }
                            else
                            {
                                PdfPCell R1 = new PdfPCell(new Phrase("", TableFont));
                                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                // R1.BorderColor = BaseColor.WHITE;
                                R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                R1.FixedHeight = 15f;
                                table.AddCell(R1);
                            }

                            PdfPCell R2 = new PdfPCell(new Phrase(Entity.FirstName.Trim() + "  " + Entity.LastName.Trim(), TableFont));
                            R2.HorizontalAlignment = Element.ALIGN_LEFT;
                            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));       //Column Middle Rows Background
                            R2.BorderColor = BaseColor.WHITE;
                            R2.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            R2.FixedHeight = 15f;
                            table.AddCell(R2);

                            PdfPCell R3 = new PdfPCell(new Phrase(Entity.Street.Trim() + " " + Entity.Suffix.Trim(), TableFont));
                            R3.HorizontalAlignment = Element.ALIGN_LEFT;
                            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R3.BorderColor = BaseColor.WHITE;
                            R3.FixedHeight = 15f;
                            table.AddCell(R3);

                            PdfPCell R4 = new PdfPCell(new Phrase(Entity.City.Trim(), TableFont));
                            R4.HorizontalAlignment = Element.ALIGN_LEFT;
                            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R4.BorderColor = BaseColor.WHITE;
                            R4.FixedHeight = 15f;
                            table.AddCell(R4);

                            PdfPCell R5 = new PdfPCell(new Phrase(Entity.State.Trim(), TableFont));
                            R5.HorizontalAlignment = Element.ALIGN_LEFT;
                            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R5.BorderColor = BaseColor.WHITE;
                            R5.FixedHeight = 15f;
                            table.AddCell(R5);

                            if (!string.IsNullOrEmpty(Entity.TelNumber.Trim()))
                            {
                                MaskedTextBox mskPhn = new MaskedTextBox();
                                mskPhn.Mask = "(000)000-0000";
                                if (!string.IsNullOrEmpty(Entity.TelNumber.Trim()))
                                    mskPhn.Text = Entity.TelNumber.Trim();

                                PdfPCell R6 = new PdfPCell(new Phrase(mskPhn.Text, TableFont));
                                R6.HorizontalAlignment = Element.ALIGN_LEFT;
                                R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                                R6.BorderColor = BaseColor.WHITE;
                                R6.FixedHeight = 15f;
                                table.AddCell(R6);
                            }
                            else
                            {
                                PdfPCell R6 = new PdfPCell(new Phrase("", TableFont));
                                R6.HorizontalAlignment = Element.ALIGN_LEFT;
                                R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                                R6.BorderColor = BaseColor.WHITE;
                                R6.FixedHeight = 15f;
                                table.AddCell(R6);
                            }

                            string HeatSource = string.Empty;
                            if (!string.IsNullOrEmpty(Entity.HeatSource.Trim()))
                            {
                                if (AgyTabs_List.Count > 0)
                                {
                                    List<AGYTABSEntity> FuelTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("08004"));
                                    if (FuelTypes.Count > 0)
                                    {
                                        foreach (AGYTABSEntity dr in FuelTypes)
                                        {
                                            if (dr.Table_Code.Trim() == Entity.HeatSource.Trim())
                                            {
                                                HeatSource = dr.Code_Desc.Trim(); break;
                                            }
                                        }
                                    }
                                }
                            }

                            PdfPCell R7 = new PdfPCell(new Phrase(HeatSource, TableFont));
                            R7.HorizontalAlignment = Element.ALIGN_LEFT;
                            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R7.BorderColor = BaseColor.WHITE;
                            R7.FixedHeight = 15f;
                            table.AddCell(R7);

                            string Status = string.Empty;
                            if (!string.IsNullOrEmpty(Entity.Status.Trim()))
                            {
                                if (AgyTabs_List.Count > 0)
                                {
                                    List<AGYTABSEntity> FuelTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("00125"));
                                    if (FuelTypes.Count > 0)
                                    {
                                        foreach (AGYTABSEntity dr in FuelTypes)
                                        {
                                            if (dr.Table_Code.Trim() == Entity.Status.Trim())
                                            {
                                                Status = dr.Code_Desc.Trim(); break;
                                            }
                                        }
                                    }
                                }
                            }

                            PdfPCell R10 = new PdfPCell(new Phrase(Status, TableFont));
                            R10.HorizontalAlignment = Element.ALIGN_LEFT;
                            R10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R10.BorderColor = BaseColor.WHITE;
                            R10.FixedHeight = 15f;
                            table.AddCell(R10);

                            string IncomeSource = string.Empty;
                            if (!string.IsNullOrEmpty(Entity.SourceIncome.Trim()))
                            {
                                if (AgyTabs_List.Count > 0)
                                {
                                    List<AGYTABSEntity> IncomeTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("00004"));
                                    if (IncomeTypes.Count > 0)
                                    {
                                        foreach (AGYTABSEntity dr in IncomeTypes)
                                        {
                                            if (Entity.HeatSource.Trim().Contains(dr.Table_Code.Trim()))
                                                IncomeSource += dr.Code_Desc.Trim();
                                        }
                                    }
                                }
                            }



                            PrintSpaceCell(table, 1, TableFont);

                            PdfPCell R8 = new PdfPCell(new Phrase("Income source: " + IncomeSource, TblFontItalic));
                            R8.HorizontalAlignment = Element.ALIGN_LEFT;
                            R8.Colspan = 3;
                            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c3d4df"));
                            // R8.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            table.AddCell(R8);

                            //PdfPCell R9 = new PdfPCell(new Phrase("Contact: " + LookupDataAccess.Getdate(Entity.ContactDate.Trim()) + "                         Time arrived: __:__", TableFont));
                            //R9.HorizontalAlignment = Element.ALIGN_LEFT;
                            //R9.Colspan = 4;
                            //R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            //R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            //R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c3d4df"));
                            //table.AddCell(R9);


                            PdfPCell R9 = new PdfPCell(new Phrase("Contact Date: " + LookupDataAccess.Getdate(Entity.ContactDate.Trim()) + "", TblFontItalic));
                            R9.HorizontalAlignment = Element.ALIGN_LEFT;
                            R9.Colspan = 2;
                            R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c3d4df"));
                            table.AddCell(R9);

                            PdfPCell R11 = new PdfPCell(new Phrase("Time arrived: __:__", TblFontItalic));
                            R11.HorizontalAlignment = Element.ALIGN_LEFT;
                            R11.Colspan = 2;
                            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c3d4df"));
                            table.AddCell(R11);


                            //PrintSpaceCell(table, 8, TableFont);

                            PrivTime = S_Time.Trim() + Meridian; PrivDateTime = Entity.Date.Trim(); PrivDtSite = Entity.Site.Trim();
                        }

                        if (table.Rows.Count > 0)
                        {
                            document.Add(table);
                            table.DeleteBodyRows();
                        }

                        if (ApptSelHistList.Count > 0)
                        {
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            document.NewPage();

                            PdfPTable table1 = new PdfPTable(9);
                            table1.TotalWidth = 750f;
                            table1.WidthPercentage = 100;
                            table1.LockedWidth = true;
                            float[] widths1 = new float[] { 20f, 18f, 45f, 45f, 20f, 10f, 20f, 20f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table1.SetWidths(widths1);
                            table1.HorizontalAlignment = Element.ALIGN_LEFT;

                            PdfPCell Head1 = new PdfPCell(new Phrase("Cancelled and Re-Scheduled Information", TblFontBold));
                            Head1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Head1.BorderColor = BaseColor.WHITE;
                            Head1.FixedHeight = 15f;
                            Head1.Colspan = 9;
                            
                            table1.AddCell(Head1);

                            string[] Header = { "Date", "Time", "Name", "Address", "City", "St", "Telephone #", "Source", "Status" };
                            for (int i = 0; i < Header.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(Header[i], SubHeadFont));
                                cell.FixedHeight = 15f;
                                cell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                table1.AddCell(cell);
                            }

                            foreach (APPTSCHDHISTEntity Entity in ApptSelHistList)
                            {
                                string Meridian = string.Empty;
                                int time_set = int.Parse(Entity.Time.Trim());
                                if (time_set > 1159)
                                {
                                    Meridian = "PM";
                                    if (time_set > 1259) time_set = time_set - 1200;
                                }
                                else Meridian = "AM";

                                string S_Time = string.Empty;
                                if (time_set >= 1000)
                                    S_Time = time_set.ToString().Substring(0, 2) + ":" + time_set.ToString().Substring(2, 2);
                                else S_Time = "0" + time_set.ToString().Substring(0, 1) + ":" + time_set.ToString().Substring(1, 2);

                                if (S_Time + Meridian == PrivTime)
                                {
                                    if (Entity.Date.Trim() == PrivDateTime && Entity.Site.Trim() == PrivDtSite)
                                        Time_Sw = true;
                                    else Time_Sw = false;
                                }
                                else Time_Sw = false;

                                PdfPCell R0 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.Date.Trim()), TableFont));
                                R0.HorizontalAlignment = Element.ALIGN_LEFT;
                                R0.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R0.BorderColor = BaseColor.WHITE;
                                R0.FixedHeight = 15f;
                                table1.AddCell(R0);

                                //if (!Time_Sw)
                                //{
                                PdfPCell R1 = new PdfPCell(new Phrase(S_Time + Meridian, TableFont));
                                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                                R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R1.BorderColor = BaseColor.WHITE;
                                R1.FixedHeight = 15f;
                                table1.AddCell(R1);
                                //}
                                //else
                                //{
                                //    PdfPCell R1 = new PdfPCell(new Phrase("", TableFont));
                                //    R1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //    R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //    table.AddCell(R1);
                                //}

                                PdfPCell R2 = new PdfPCell(new Phrase(Entity.FirstName.Trim() + "  " + Entity.LastName.Trim(), TableFont));
                                R2.HorizontalAlignment = Element.ALIGN_LEFT;
                                R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R2.BorderColor = BaseColor.WHITE;
                                R2.FixedHeight = 15f;
                                table1.AddCell(R2);

                                PdfPCell R3 = new PdfPCell(new Phrase(Entity.Street.Trim() + " " + Entity.Suffix.Trim(), TableFont));
                                R3.HorizontalAlignment = Element.ALIGN_LEFT;
                                R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R3.BorderColor = BaseColor.WHITE;
                                R3.FixedHeight = 15f;
                                table1.AddCell(R3);

                                PdfPCell R4 = new PdfPCell(new Phrase(Entity.City.Trim(), TableFont));
                                R4.HorizontalAlignment = Element.ALIGN_LEFT;
                                R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R4.BorderColor = BaseColor.WHITE;
                                R4.FixedHeight = 15f;
                                table1.AddCell(R4);

                                PdfPCell R5 = new PdfPCell(new Phrase(Entity.State.Trim(), TableFont));
                                R5.HorizontalAlignment = Element.ALIGN_LEFT;
                                R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R5.BorderColor = BaseColor.WHITE;
                                R5.FixedHeight = 15f;
                                table1.AddCell(R5);

                                if (!string.IsNullOrEmpty(Entity.TelNumber.Trim()))
                                {
                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(000)000-0000";
                                    if (!string.IsNullOrEmpty(Entity.TelNumber.Trim()))
                                        mskPhn.Text = Entity.TelNumber.Trim();

                                    PdfPCell R6 = new PdfPCell(new Phrase(mskPhn.Text, TableFont));
                                    R6.HorizontalAlignment = Element.ALIGN_LEFT;
                                    R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    R6.BorderColor = BaseColor.WHITE;
                                    R6.FixedHeight = 15f;
                                    table1.AddCell(R6);
                                }
                                else
                                {
                                    PdfPCell R6 = new PdfPCell(new Phrase("", TableFont));
                                    R6.HorizontalAlignment = Element.ALIGN_LEFT;
                                    R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    R6.BorderColor = BaseColor.WHITE;
                                    R6.FixedHeight = 15f;
                                    table1.AddCell(R6);
                                }

                                string HeatSource = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.HeatSource.Trim()))
                                {
                                    if (AgyTabs_List.Count > 0)
                                    {
                                        List<AGYTABSEntity> FuelTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("08004"));
                                        if (FuelTypes.Count > 0)
                                        {
                                            foreach (AGYTABSEntity dr in FuelTypes)
                                            {
                                                if (dr.Table_Code.Trim() == Entity.HeatSource.Trim())
                                                {
                                                    HeatSource = dr.Code_Desc.Trim(); break;
                                                }
                                            }
                                        }
                                    }
                                }

                                PdfPCell R7 = new PdfPCell(new Phrase(HeatSource, TableFont));
                                R7.HorizontalAlignment = Element.ALIGN_LEFT;
                                R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R7.BorderColor = BaseColor.WHITE;
                                R7.FixedHeight = 15f;
                                table1.AddCell(R7);

                                string Status = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.Status.Trim()))
                                {
                                    if (AgyTabs_List.Count > 0)
                                    {
                                        List<AGYTABSEntity> FuelTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("00125"));
                                        if (FuelTypes.Count > 0)
                                        {
                                            foreach (AGYTABSEntity dr in FuelTypes)
                                            {
                                                if (dr.Table_Code.Trim() == Entity.Status.Trim())
                                                {
                                                    Status = dr.Code_Desc.Trim(); break;
                                                }
                                            }
                                        }
                                    }
                                }

                                PdfPCell R10 = new PdfPCell(new Phrase(Status, TableFont));
                                R10.HorizontalAlignment = Element.ALIGN_LEFT;
                                R10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                R10.BorderColor = BaseColor.WHITE;
                                R10.FixedHeight = 15f;
                                table1.AddCell(R10);

                                string IncomeSource = string.Empty;
                                if (!string.IsNullOrEmpty(Entity.SourceIncome.Trim()))
                                {
                                    if (AgyTabs_List.Count > 0)
                                    {
                                        List<AGYTABSEntity> IncomeTypes = AgyTabs_List.FindAll(u => u.Tabs_Type.Equals("00004"));
                                        if (IncomeTypes.Count > 0)
                                        {
                                            foreach (AGYTABSEntity dr in IncomeTypes)
                                            {
                                                if (Entity.HeatSource.Trim().Contains(dr.Table_Code.Trim()))
                                                    IncomeSource += dr.Code_Desc.Trim();
                                            }
                                        }
                                    }
                                }

                                PrintSpaceCell(table1, 2, TableFont);

                                PdfPCell R8 = new PdfPCell(new Phrase("Income source: " + IncomeSource, TblFontItalic));
                                R8.HorizontalAlignment = Element.ALIGN_LEFT;
                                R8.Colspan = 3;
                               // R8.Border = iTextSharp.text.Rectangle.BOX;
                                R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                                R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                table1.AddCell(R8);

                                PdfPCell R9 = new PdfPCell(new Phrase("Contact Date: " + LookupDataAccess.Getdate(Entity.ContactDate.Trim()) + "", TblFontItalic));
                                R9.HorizontalAlignment = Element.ALIGN_LEFT;
                                R9.Colspan = 2;
                               // R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                                 R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                table1.AddCell(R9);

                                PdfPCell R11 = new PdfPCell(new Phrase("Time arrived: __:__", TblFontItalic));
                                R11.HorizontalAlignment = Element.ALIGN_LEFT;
                                R11.Colspan = 2;
                                //R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                                R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                table1.AddCell(R11);

                                //PrintSpaceCell(table1, 9, TableFont);

                                PrivTime = S_Time.Trim() + Meridian; PrivDateTime = Entity.Date.Trim(); PrivDtSite = Entity.Site.Trim();
                            }

                            if (table1.Rows.Count > 0)
                            {
                                document.Add(table1);
                            }
                        }


                        if (chkbSummary.Checked)
                        {
                            if (SelTemplates.Count > 0)
                            {
                                document.NewPage();

                                PdfPTable head = new PdfPTable(1);
                                head.HorizontalAlignment = Element.ALIGN_CENTER;
                                head.TotalWidth = 50f;
                                PdfPCell headcell = new PdfPCell(new Phrase(""));
                                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                head.AddCell(headcell);
                                head.SpacingAfter = 50f;

                                PdfPTable Summarytable = new PdfPTable(4);
                                Summarytable.TotalWidth = 400f;
                                Summarytable.WidthPercentage = 100;
                                Summarytable.LockedWidth = true;
                                float[] Summarytablewidths = new float[] { 60, 40, 20, 20 };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Summarytable.SetWidths(Summarytablewidths);
                                Summarytable.HorizontalAlignment = Element.ALIGN_LEFT;
                                Summarytable.SpacingAfter = 150f;

                                string[] Header = { "Site", "Exception", "Booked", "Reserved" };
                                for (int i = 0; i < Header.Length; ++i)
                                {

                                    if (i == 0) {
                                        PdfPCell cellTitle = new PdfPCell(new Phrase("Summary", TblFontBold));
                                        cellTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cellTitle.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                        cellTitle.BorderColor = BaseColor.WHITE;
                                        cellTitle.FixedHeight = 15f;
                                        cellTitle.Colspan = 4;
                                        Summarytable.AddCell(cellTitle);
                                    }
                                    
                                    PdfPCell cell = new PdfPCell(new Phrase(Header[i], SubHeadFont));
                                    if (i == 0 || i == 1)
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    if (i > 1)
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                    cell.FixedHeight = 15f;
                                    cell.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    cell.Border = iTextSharp.text.Rectangle.BOX;
                                    Summarytable.AddCell(cell);
                                }

                                List<APPTEMPLATESEntity> tmsApcnEntity = _model.TmsApcndata.GetAPPTEMPLATESadpyldt(Agency, Depart, Program, string.Empty, string.Empty, string.Empty, string.Empty, "Dates");
                                if (rbAll.Checked)
                                {
                                    if (((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString() != "****")
                                        tmsApcnEntity = tmsApcnEntity.FindAll(u => u.Location.Equals(((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Value.ToString()));

                                    foreach (APPTEMPLATESEntity Ent in tmsApcnEntity)
                                    {
                                        APPTEMPLATESEntity TempEntity = tmsApcnEntity.Find(u => u.Type.Equals(Ent.Type.Trim()) && u.Location.Trim().Equals(Ent.Location.Trim()));
                                        string strType = LookupDataAccess.Getdate(TempEntity.FDate.ToString().Trim()) + " - " + LookupDataAccess.Getdate(TempEntity.TDate.ToString());
                                        string bookedSlots = string.Empty, ReservedSlots = string.Empty;

                                        TimeSpan Differnece = Convert.ToDateTime(TempEntity.TDate.ToString().Trim()) - Convert.ToDateTime(TempEntity.FDate.ToString().Trim());

                                        int TemplateDays = Differnece.Days + 1;
                                        List<APPTEMPLATESEntity> OtherTemplates = tmsApcnEntity.FindAll(u => Convert.ToDateTime(u.FDate.Trim()) >= Convert.ToDateTime(TempEntity.FDate.ToString().Trim()) && Convert.ToDateTime(u.TDate.Trim()) <= Convert.ToDateTime(TempEntity.TDate.ToString().Trim()) && u.Type != TempEntity.Type);
                                        if (OtherTemplates.Count > 0)
                                        {
                                            foreach (APPTEMPLATESEntity Entity in OtherTemplates)
                                            {

                                            }
                                        }

                                        bookedSlots = ApptSelList.FindAll(u => u.Site.Trim().Equals(TempEntity.Location.Trim()) && u.TemplateID.Equals(TempEntity.Type) && u.SchdType.Equals("1")).Count.ToString();
                                        if (bookedSlots == "0") bookedSlots = string.Empty;

                                        ReservedSlots = ApptSelList.FindAll(u => u.Site.Trim().Equals(TempEntity.Location.Trim()) && u.TemplateID.Equals(TempEntity.Type) && u.SchdType.Equals("2")).Count.ToString();
                                        if (ReservedSlots == "0") ReservedSlots = string.Empty;

                                        int inttotalOpendslots = Convert.ToInt32(TempEntity.OpenSlots == string.Empty ? "0" : TempEntity.OpenSlots);
                                        //int inttotalcount = inttotalOpendslots - Convert.ToInt32(apptschedulecount.TotalCount);

                                        PdfPCell S1 = new PdfPCell(new Phrase(TempEntity.Location + "- " + TempEntity.Description.Trim(), TableFont));
                                        S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        S1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S1.BorderColor = BaseColor.WHITE;
                                        S1.FixedHeight = 15f;
                                        Summarytable.AddCell(S1);

                                        PdfPCell S2 = new PdfPCell(new Phrase(strType, TableFont));
                                        S2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        S2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S2.BorderColor = BaseColor.WHITE;
                                        S2.FixedHeight = 15f;
                                        Summarytable.AddCell(S2);

                                        PdfPCell S3 = new PdfPCell(new Phrase(bookedSlots, TableFont));
                                        S3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        S3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S3.BorderColor = BaseColor.WHITE;
                                        S3.FixedHeight = 15f;
                                        Summarytable.AddCell(S3);

                                        PdfPCell S4 = new PdfPCell(new Phrase(ReservedSlots, TableFont));
                                        S4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        S4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S4.BorderColor = BaseColor.WHITE;
                                        S4.FixedHeight = 15f;
                                        Summarytable.AddCell(S4);

                                        //PdfPCell S5 = new PdfPCell(new Phrase("", TableFont));
                                        //S5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //S5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Summarytable.AddCell(S5);
                                    }
                                }
                                else
                                {
                                    foreach (var Ent in SelTemplates)
                                    {
                                        APPTEMPLATESEntity TempEntity = tmsApcnEntity.Find(u => u.Type.Equals(Ent.TemplateID.Trim()) && u.Location.Trim().Equals(Ent.Site.Trim()));
                                        string strType = LookupDataAccess.Getdate(TempEntity.FDate.ToString().Trim()) + " - " + LookupDataAccess.Getdate(TempEntity.TDate.ToString());
                                        string bookedSlots = string.Empty, ReservedSlots = string.Empty;

                                        TimeSpan Differnece = Convert.ToDateTime(TempEntity.TDate.ToString().Trim()) - Convert.ToDateTime(TempEntity.FDate.ToString().Trim());

                                        int TemplateDays = Differnece.Days;
                                        //List<APPTEMPLATESEntity> OtherTemplates = tmsApcnEntity.FindAll(u => Convert.ToDateTime(u.FDate.Trim()) >= Convert.ToDateTime(TempEntity.FDate.ToString().Trim()) && Convert.ToDateTime(u.TDate.Trim()) <= Convert.ToDateTime(TempEntity.TDate.ToString().Trim()) && u.Type!=TempEntity.Type);
                                        //if(OtherTemplates.Count>0)
                                        //{
                                        //    foreach(APPTEMPLATESEntity Entity in OtherTemplates)
                                        //    {
                                        //        if()
                                        //    }
                                        //}

                                        bookedSlots = ApptSelList.FindAll(u => u.Site.Trim().Equals(TempEntity.Location.Trim()) && u.TemplateID.Equals(TempEntity.Type) && u.SchdType.Equals("1")).Count.ToString();
                                        if (bookedSlots == "0") bookedSlots = string.Empty;

                                        ReservedSlots = ApptSelList.FindAll(u => u.Site.Trim().Equals(TempEntity.Location.Trim()) && u.TemplateID.Equals(TempEntity.Type) && u.SchdType.Equals("2")).Count.ToString();
                                        if (ReservedSlots == "0") ReservedSlots = string.Empty;

                                        int inttotalOpendslots = Convert.ToInt32(TempEntity.OpenSlots == string.Empty ? "0" : TempEntity.OpenSlots);
                                        //int inttotalcount = inttotalOpendslots - Convert.ToInt32(apptschedulecount.TotalCount);

                                        PdfPCell S1 = new PdfPCell(new Phrase(TempEntity.Location + "- " + TempEntity.Description.Trim(), TableFont));
                                        S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        S1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S1.BorderColor = BaseColor.WHITE;
                                        S1.FixedHeight = 15f;
                                        Summarytable.AddCell(S1);

                                        PdfPCell S2 = new PdfPCell(new Phrase(strType, TableFont));
                                        S2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        S2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S2.BorderColor = BaseColor.WHITE;
                                        S2.FixedHeight = 15f;
                                        Summarytable.AddCell(S2);

                                        PdfPCell S3 = new PdfPCell(new Phrase(bookedSlots, TableFont));
                                        S3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        S3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S3.BorderColor = BaseColor.WHITE;
                                        S3.FixedHeight = 15f;
                                        Summarytable.AddCell(S3);

                                        PdfPCell S4 = new PdfPCell(new Phrase(ReservedSlots, TableFont));
                                        S4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        S4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        S4.BorderColor = BaseColor.WHITE;
                                        S4.FixedHeight = 15f;
                                        Summarytable.AddCell(S4);

                                        //PdfPCell S5 = new PdfPCell(new Phrase("", TableFont));
                                        //S5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //S5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Summarytable.AddCell(S5);
                                    }
                                }
                                document.Add(head);
                                document.Add(Summarytable);
                            }
                        }

                    }
                    else
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bfTimes, 18);
                        cb.SetColorFill(BaseColor.RED);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "No Appointments found", 150, 500, 0);
                        cb.EndText();
                    }

                    AlertBox.Show("Report Generated Successfully");
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();
            }

        }

        private void PrintSpaceCell(PdfPTable table, int Spacesnum, iTextSharp.text.Font TableFont)
        {
            if (Spacesnum == 1)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                // S2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                //S2.BorderColor = BaseColor.WHITE;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                S2.FixedHeight = 15f;
                table.AddCell(S2);
            }
            else if (Spacesnum == 2)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 2;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 3)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 3;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 4)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 4;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 6)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 6;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 7)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 7;
                S2.FixedHeight = 12f;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 10)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 10;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 15)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 15;
                S2.FixedHeight = 15f;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 12)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 12;
                S2.FixedHeight = 15f;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
        }

        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
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

            PdfPTable Headertable = new PdfPTable(2);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 20f, 80f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.DARK_GRAY);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            //content.SetLineWidth(6);
            content.Stroke();



            PdfPCell cellspace = new PdfPCell(new Phrase(""));
            cellspace.HorizontalAlignment = Element.ALIGN_CENTER;
            cellspace.Colspan = 2;
            cellspace.PaddingBottom = 10;
            cellspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(cellspace);

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.PaddingBottom = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }


            PdfPCell cellRptTitle = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblHeaderTitleFont));
            cellRptTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptTitle.Colspan = 2;
            cellRptTitle.PaddingBottom = 15;
            cellRptTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(cellRptTitle);


            PdfPCell cellRptHeader = new PdfPCell(new Phrase("Report Parameters", TblParamsHeaderFont));
            cellRptHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptHeader.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cellRptHeader.PaddingBottom = 5;
            cellRptHeader.MinimumHeight = 6;
            cellRptHeader.Colspan = 2;
            cellRptHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellRptHeader.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(cellRptHeader);

            //PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            //row2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////row2.Colspan = 2;
            //row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row2);

            //PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            //row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////row2.Colspan = 2;
            //row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row21);

            //PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblFont));
            //row3.HorizontalAlignment = Element.ALIGN_CENTER;
            //row3.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            //row3.MinimumHeight = 8;
            //row3.Colspan = 2;
            //row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //row3.BackgroundColor = BaseColor.LIGHT_GRAY;
            //Headertable.AddCell(row3);

            string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = "Agency : " + Agency;
            if (Depart != "**") Dept = "Dept : " + Depart;
            if (Program != "**") Prg = "Program : " + Program;
            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Agy + "  " + Dept + "  " + Prg + "  " + Header_year, TableFont));Txt_HieDesc
            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            //Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            //Hierarchy.Colspan = 2;
            //Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(Hierarchy);




            PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title2);

            PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc2);



            PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  " + "Site", TableFont));
            cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title3);

            PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc3);



            //PdfPCell R1 = new PdfPCell(new Phrase("Site: " + ((Captain.Common.Utilities.ListItem)cmbSite.SelectedItem).Text.ToString(), TableFont));
            //R1.HorizontalAlignment = Element.ALIGN_LEFT;
            //R1.Colspan = 2;
            //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R1);

            string Sel_Type = "Date Range";
            if (rbAll.Checked) Sel_Type = rbAll.Text.Trim(); else if (rbFuture.Checked) Sel_Type = rbFuture.Text.Trim();

            //PdfPCell R2 = new PdfPCell(new Phrase(Sel_Type.Trim(), TableFont));
            //R2.HorizontalAlignment = Element.ALIGN_LEFT;
            //R2.Colspan = 2;
            //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R2);

            //if (rbDateRange.Checked)
            //{
            //    PdfPCell R3 = new PdfPCell(new Phrase(lblStartDate.Text + " : " + dtpStartDate.Text.Trim() + "       " + lblEndDate.Text.Trim() + " : " + dtpEndDate.Text.Trim(), TableFont));
            //    R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //    R3.Colspan = 2;
            //    R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Headertable.AddCell(R3);
            //}

            PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  " + "Selected Date Type", TableFont));
            cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title4);


            string strDateRange = Sel_Type;
            if (rbDateRange.Checked)
                strDateRange = Sel_Type + "From : " + dtpStartDate.Text.Trim() + "       " + lblEndDate.Text.Trim() + " : " + dtpEndDate.Text.Trim();


            PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase(strDateRange, TableFont));
            cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc4);

            document.Add(Headertable);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "TBSB0110");
        }

    }
}