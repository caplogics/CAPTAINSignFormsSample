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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;
using Org.BouncyCastle.Security;
using Captain.Common.Interfaces;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2111ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;

        private CaptainModel _model = null;

        #endregion
        public HSSB2111ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = /*privileges.Program + " - " +*/ privileges.PrivilegeName;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Prog = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;


            CustRespEntity SearchCustresp = new CustRespEntity(true);
            FillCaseSite();

            propRankscategory = _model.SPAdminData.Browse_RankCtg();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

            //  btnExcel.Visible = false;
            //if (BaseForm.UserID == "SYSTEM")
            //    btnExcel.Visible = true;







        }


        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<RankCatgEntity> propRankscategory { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public List<CaseMstEntity> propCaseMstList { get; set; }
        public List<CaseSnpEntity> propCaseSnpList { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CaseSiteEntity> propCaseAllSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<CaseSiteEntity> AllSitesEntity { get; set; }

        List<ChldAttnEntity> propChldAttnList { get; set; }
        public List<ChildATTMSEntity> propchldAttmsEntityList { get; set; }
        public List<CommonEntity> propAppsentReasons { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }

        #endregion

        private void FillCaseSite()
        {
            cmbSiteforHome.Items.Clear();
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Program_Year;
            propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            AllSitesEntity = propCaseSiteEntity;

            propCaseSiteEntity = propCaseSiteEntity.GroupBy(u => u.SiteNUMBER).Select(g => g.First()).ToList();
            propCaseAllSiteEntity = propCaseSiteEntity;
            //propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000").Distinct(propCaseSiteEntity.Find(u=>u.SiteNUMBER)));
            // propCaseAllSiteEntity = _model.CaseMstData.GetCaseSiteAll();
            if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
            {
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                HierarchyEntity hierarchyEntity = new HierarchyEntity(); List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
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
                                foreach (CaseSiteEntity casesite in propCaseSiteEntity) //Site_List)//ListcaseSiteEntity)
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
                        propCaseSiteEntity = selsites;
                    }
                }
            }


            foreach (CaseSiteEntity item in propCaseSiteEntity)
            {
                cmbSiteforHome.Items.Add(new Captain.Common.Utilities.ListItem(item.SiteNUMBER + "   " + item.SiteNAME, item.SiteNUMBER));
            }
            cmbSiteforHome.Items.Insert(0, new Captain.Common.Utilities.ListItem(" None ", "0"));
            cmbSiteforHome.SelectedIndex = 0;
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
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

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);
                    propReportPath = _model.lookupDataAccess.GetReportPath();
                    cmbSiteforHome.Items.Clear();
                    FillCaseSite();
                }
            }
        }

        string Program_Year;
        string strProgramName;
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
                    {
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        strProgramName = ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString();
                    }
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(790, 25);
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

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(710, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(790, 25);
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


        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (chkDetailReport.Checked)
                {
                    string SelFunds = "A";
                    if (rdoDetailBoth.Checked == true)
                    {
                        if (rdoSelected.Checked)
                        {
                            if (SelFundingList.Count > 0)
                            {
                                SelFunds = string.Empty;
                                foreach (CommonEntity Entity in SelFundingList)
                                {
                                    if (!SelFunds.Equals(string.Empty))
                                    {
                                        SelFunds += ",";
                                    }
                                    SelFunds += Entity.Code;
                                }
                            }
                        }
                    }

                    string strsiteRoomNames = string.Empty;

                    if (rdoMultipleSites.Checked == true)
                    {
                        foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                        {
                            if (!strsiteRoomNames.Equals(string.Empty))
                            {
                                strsiteRoomNames += ",";
                                //strOnlySites += ",";
                            }
                            strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                            //strOnlySites += siteRoom.SiteNUMBER;
                        }
                    }
                    else
                    {
                        strsiteRoomNames = "A";
                    }

                    propCaseEnrlList = _model.EnrollData.Get2111ExcelDetails(Agency, Dept, Prog, Program_Year, string.Empty, strsiteRoomNames, SelFunds, "HSSB2111EXCEL", dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString());
                    propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");

                    propChldAttnList = _model.ChldAttnData.GetChldAttnBetweenDatehssb2109(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), string.Empty, string.Empty, "ALL");


                }

                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                //pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed_DevExpress);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
        }

        #region PrintingReportDetails

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);

        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {

            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {

                string strsiteRoomNames = string.Empty;
                string strOnlySites = string.Empty;
                if (rdoMultipleSites.Checked == true)
                {
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty))
                        {
                            strsiteRoomNames += ",";
                            strOnlySites += ",";
                        }
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                        strOnlySites += siteRoom.SiteNUMBER;
                    }
                }
                else
                {
                    strsiteRoomNames = "A";
                    CaseSiteEntity searchEntity = new CaseSiteEntity(true);
                    searchEntity.SiteAGENCY = Agency; searchEntity.SiteDEPT = Dept; searchEntity.SitePROG = Prog;
                    searchEntity.SiteYEAR = Program_Year;
                    List<CaseSiteEntity> AllSites = _model.CaseMstData.Browse_CASESITE(searchEntity, "Browse");
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                                { hierarchyEntity = null; strsiteRoomNames = "A"; }
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
                                        foreach (CaseSiteEntity casesite in AllSites) //Site_List)//ListcaseSiteEntity)
                                        {
                                            if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                            {
                                                if (!strsiteRoomNames.Equals(string.Empty)) { strsiteRoomNames += ","; strOnlySites += ","; }
                                                strsiteRoomNames += casesite.SiteNUMBER + casesite.SiteROOM + casesite.SiteAM_PM;
                                                strOnlySites += casesite.SiteNUMBER;
                                                //break;
                                            }
                                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                        }
                                    }
                                }
                                //strsiteRoomNames = hierarchyEntity.Sites;
                            }
                        }


                    }
                    else
                        strsiteRoomNames = "A";
                }

                StringBuilder strMstApplUpdate = new StringBuilder();
                string PdfName = "Pdf File";
                string strExcelFileName = "ExcelFile";
                PdfName = form.GetFileName();
                strExcelFileName = form.GetFileName();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                if (chkDetailReport.Checked)
                {
                    ExcelreportData(strExcelFileName);
                }

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
                //document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
                BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                try
                {

                    PrintHeaderPage(document, writer);

                    List<CaseSiteEntity> caseSiteAllSiteList = propCaseAllSiteEntity;
                    if (cmbSiteforHome.Items.Count > 0)
                    {
                        if (((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Value.ToString() != "0")
                        {
                            caseSiteAllSiteList = propCaseAllSiteEntity.FindAll(u => u.SiteNUMBER != ((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Value.ToString());
                        }
                    }

                    if (rdoDetailBoth.Checked == true || rdoDetail.Checked == true)
                    {

                        document.NewPage();
                        Y_Pos = 795;

                        PdfPTable hssb2111_Table = new PdfPTable(10);
                        hssb2111_Table.TotalWidth = 550f;
                        hssb2111_Table.WidthPercentage = 100;
                        hssb2111_Table.LockedWidth = true;
                        float[] widths = new float[] { 35f, 25f, 25f, 25f, 25f, 25f, 25f, 25f, 20f, 20f };
                        hssb2111_Table.SetWidths(widths);
                        hssb2111_Table.HorizontalAlignment = Element.ALIGN_CENTER;

                        // List<CaseSiteEntity> caseGroupSite =  from c in propCaseSiteEntity group c by into myAccount SiteNUMBER.Distinct() ;
                        List<CaseEnrlEntity> chldAttnDetails = _model.EnrollData.Get2111Details(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strsiteRoomNames, "HSSB2111DETAILS");

                        int intFundEnrollmentTot = 0;
                        int intEndEnrollmentTot = 0;
                        int intClassDaysTot = 0;
                        int intAvailbleDaysTot = 0;
                        int intExcusedDaysTot = 0;
                        int intAttandanceDaysTot = 0;
                        int intAveAttendanceTot = 0;
                        decimal intAveAttendanceTot2 = 0;
                        int intTotalRows = 0;


                        int intprogFundEnrollmentTot = 0;
                        int intprogEndEnrollmentTot = 0;
                        int intprogClassDaysTot = 0;
                        int intprogAvailbleDaysTot = 0;
                        int intprogExcusedDaysTot = 0;
                        int intprogAttandanceDaysTot = 0;
                        int intprogAveAttendanceTot = 0;
                        decimal intprogAveAttendanceTot2 = 0;
                        int intprogTotalRows = 0;
                        bool boolprogprint = false;
                        foreach (CaseSiteEntity siterow in caseSiteAllSiteList)
                        {
                            intFundEnrollmentTot = 0;
                            intEndEnrollmentTot = 0;
                            intClassDaysTot = 0;
                            intAvailbleDaysTot = 0;
                            intExcusedDaysTot = 0;
                            intAttandanceDaysTot = 0;
                            intAveAttendanceTot = 0;
                            intAveAttendanceTot2 = 0;
                            intTotalRows = 0;
                            List<CaseEnrlEntity> caseEnrlDetails = chldAttnDetails.FindAll(u => u.Site.Trim().ToString() == siterow.SiteNUMBER.Trim().ToString());
                            if (caseEnrlDetails.Count > 0)
                            {
                                boolprogprint = true;
                                PdfPCell Header_1 = new PdfPCell(new Phrase("Class Period From " + dtForm.Value.ToShortDateString() + "  To  " + dtTodate.Value.ToShortDateString(), TableFont));
                                Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header_1.Colspan = 4;
                                Header_1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                hssb2111_Table.AddCell(Header_1);

                                PdfPCell Header = new PdfPCell(new Phrase("Site :" + siterow.SiteNUMBER + " " + siterow.SiteNAME, TableFont));
                                Header.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header.Colspan = 2;
                                Header.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                hssb2111_Table.AddCell(Header);

                                PdfPCell HeaderTop1 = new PdfPCell(new Phrase(siterow.SiteCOMMENT, TableFont));
                                HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderTop1.Colspan = 4;
                                HeaderTop1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                hssb2111_Table.AddCell(HeaderTop1);

                                PdfPCell Header1 = new PdfPCell(new Phrase("Class", TableFont));
                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                hssb2111_Table.AddCell(Header1);

                                PdfPCell Header2 = new PdfPCell(new Phrase("Funding Source", TableFont));
                                Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header2);

                                PdfPCell Header3 = new PdfPCell(new Phrase("Funded Enrollment", TableFont));
                                Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header3);

                                PdfPCell Header4 = new PdfPCell(new Phrase("End Period Enrollment", TableFont));
                                Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header4);

                                PdfPCell Header5 = new PdfPCell(new Phrase("Class Days", TableFont));
                                Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header5);

                                PdfPCell Header6 = new PdfPCell(new Phrase("Attendance Days", TableFont));
                                Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header6);

                                PdfPCell Header7 = new PdfPCell(new Phrase("Excused Days", TableFont));
                                Header7.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header7);


                                PdfPCell Header8 = new PdfPCell(new Phrase("Available Days", TableFont));
                                Header8.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header8);


                                PdfPCell Header9 = new PdfPCell(new Phrase("Ave Daily Attendance", TableFont));
                                Header9.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header9.Colspan = 2;
                                Header9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                hssb2111_Table.AddCell(Header9);


                                bool boolFundswtch = true;

                                foreach (CaseEnrlEntity item in caseEnrlDetails)
                                {
                                    if (rdoSelected.Checked)
                                    {
                                        boolFundswtch = false;
                                        if (SelFundingList.Count > 0)
                                        {
                                            if (SelFundingList.FindAll(u => u.Code.Trim() == item.FundHie.Trim()).Count > 0)
                                                boolFundswtch = true;
                                        }

                                    }
                                    if (boolFundswtch)
                                    {
                                        intTotalRows = intTotalRows + 1;
                                        decimal intAvg = 0;
                                        if (Convert.ToInt32(item.ClassDays) > 0)
                                            intAvg = (Convert.ToDecimal(item.AttandanceDays) / Convert.ToDecimal(item.ClassDays));

                                        decimal intAvgPer = 0;
                                        if (Convert.ToInt32(item.AvailbleDays) > 0)
                                            intAvgPer = (Convert.ToDecimal(item.AttandanceDays) * 100) / Convert.ToDecimal(item.AvailbleDays);
                                        intFundEnrollmentTot = intFundEnrollmentTot + Convert.ToInt32(item.FundEnrollment == string.Empty ? "0" : item.FundEnrollment);
                                        intEndEnrollmentTot = intEndEnrollmentTot + Convert.ToInt32(item.EndEnrollment == string.Empty ? "0" : item.EndEnrollment);
                                        intClassDaysTot = intClassDaysTot + Convert.ToInt32(item.ClassDays);
                                        intAvailbleDaysTot = intAvailbleDaysTot + Convert.ToInt32(item.AvailbleDays);
                                        intExcusedDaysTot = intExcusedDaysTot + Convert.ToInt32(item.ExcusedDays);
                                        intAttandanceDaysTot = intAttandanceDaysTot + Convert.ToInt32(item.AttandanceDays);
                                        intAveAttendanceTot = intAveAttendanceTot + Convert.ToInt32(Math.Round(intAvg));
                                        intAveAttendanceTot2 = intAveAttendanceTot2 + intAvgPer;

                                        PdfPCell pdfData1 = new PdfPCell(new Phrase(item.Site + " " + item.Room + " " + item.AMPM, TableFont));
                                        pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfData1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        hssb2111_Table.AddCell(pdfData1);

                                        PdfPCell pdfData2 = new PdfPCell(new Phrase(item.FundHie, TableFont));
                                        pdfData2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfData2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData2);

                                        PdfPCell pdfData3 = new PdfPCell(new Phrase(item.FundEnrollment, TableFont));
                                        pdfData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData3);


                                        PdfPCell pdfData4 = new PdfPCell(new Phrase(item.EndEnrollment, TableFont));
                                        pdfData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData4);

                                        PdfPCell pdfData5 = new PdfPCell(new Phrase(item.ClassDays, TableFont));
                                        pdfData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData5);

                                        PdfPCell pdfData6 = new PdfPCell(new Phrase(item.AttandanceDays, TableFont));
                                        pdfData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData6);

                                        PdfPCell pdfData7 = new PdfPCell(new Phrase(item.ExcusedDays, TableFont));
                                        pdfData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData7);


                                        PdfPCell pdfData8 = new PdfPCell(new Phrase(item.AvailbleDays, TableFont));
                                        pdfData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData8);
                                        string i = Math.Round(intAvg).ToString();
                                        PdfPCell pdfData9 = new PdfPCell(new Phrase(i, TableFont));
                                        pdfData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        hssb2111_Table.AddCell(pdfData9);

                                        PdfPCell pdfData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", intAvgPer) + "%", TableFont));
                                        pdfData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfData10.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        hssb2111_Table.AddCell(pdfData10);
                                    }
                                }

                                PdfPCell pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.Colspan = 10;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                hssb2111_Table.AddCell(pdfttlspace1);

                                PdfPCell pdfttlData1 = new PdfPCell(new Phrase("Total Site  " + siterow.SiteNAME, TableFont));
                                pdfttlData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlData1.Colspan = 2;
                                pdfttlData1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                hssb2111_Table.AddCell(pdfttlData1);


                                PdfPCell pdfttlData3 = new PdfPCell(new Phrase(intFundEnrollmentTot.ToString(), TableFont));
                                pdfttlData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData3);

                                PdfPCell pdfttlData4 = new PdfPCell(new Phrase(intEndEnrollmentTot.ToString(), TableFont));
                                pdfttlData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData4);

                                PdfPCell pdfttlData5 = new PdfPCell(new Phrase(intClassDaysTot.ToString(), TableFont));
                                pdfttlData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData5);

                                PdfPCell pdfttlData6 = new PdfPCell(new Phrase(intAttandanceDaysTot.ToString(), TableFont));
                                pdfttlData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData6);

                                PdfPCell pdfttlData7 = new PdfPCell(new Phrase(intExcusedDaysTot.ToString(), TableFont));
                                pdfttlData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData7);


                                PdfPCell pdfttlData8 = new PdfPCell(new Phrase(intAvailbleDaysTot.ToString(), TableFont));
                                pdfttlData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData8);


                                PdfPCell pdfttlData9 = new PdfPCell(new Phrase(intAveAttendanceTot.ToString(), TableFont));
                                pdfttlData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(pdfttlData9);

                                if (intTotalRows > 0)
                                {
                                    PdfPCell pdfttlData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", (intAveAttendanceTot2 / intTotalRows)) + "%", TableFont));
                                    pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER; ;
                                    hssb2111_Table.AddCell(pdfttlData10);
                                }
                                else
                                {
                                    PdfPCell pdfttlData10 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER; ;
                                    hssb2111_Table.AddCell(pdfttlData10);
                                }


                                intprogFundEnrollmentTot = intprogFundEnrollmentTot + intFundEnrollmentTot;
                                intprogEndEnrollmentTot = intprogEndEnrollmentTot + intEndEnrollmentTot;
                                intprogClassDaysTot = intprogClassDaysTot + intClassDaysTot;
                                intprogAvailbleDaysTot = intprogAvailbleDaysTot + intAvailbleDaysTot;
                                intprogExcusedDaysTot = intprogExcusedDaysTot + intExcusedDaysTot;
                                intprogAttandanceDaysTot = intprogAttandanceDaysTot + intAttandanceDaysTot;
                                intprogAveAttendanceTot = intprogAveAttendanceTot + intAveAttendanceTot;

                                if (hssb2111_Table.Rows.Count > 0)
                                {
                                    document.Add(hssb2111_Table);
                                    hssb2111_Table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }
                        }
                        if (boolprogprint)
                        {

                            decimal intprogAvg = 0;
                            if (Convert.ToInt32(intprogClassDaysTot) > 0)
                                intprogAvg = (Convert.ToDecimal(intprogAttandanceDaysTot) / Convert.ToDecimal(intprogClassDaysTot));

                            decimal intprogAvgPer = 0;
                            if (Convert.ToInt32(intprogAvailbleDaysTot) > 0)
                                intprogAvgPer = (Convert.ToDecimal(intprogAttandanceDaysTot) * 100) / Convert.ToDecimal(intprogAvailbleDaysTot);

                            PdfPCell Header_1 = new PdfPCell(new Phrase("Class Period From " + dtForm.Value.ToShortDateString() + "  To  " + dtTodate.Value.ToShortDateString(), TableFont));
                            Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header_1.Colspan = 4;
                            Header_1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_Table.AddCell(Header_1);

                            PdfPCell Header = new PdfPCell(new Phrase("Program wide totals", TableFont));
                            Header.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header.Colspan = 2;
                            Header.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_Table.AddCell(Header);

                            PdfPCell HeaderTop1 = new PdfPCell(new Phrase("", TableFont));
                            HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                            HeaderTop1.Colspan = 4;
                            HeaderTop1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_Table.AddCell(HeaderTop1);

                            PdfPCell Header1 = new PdfPCell(new Phrase(strProgramName, TableFont));
                            Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            hssb2111_Table.AddCell(Header1);

                            PdfPCell Header2 = new PdfPCell(new Phrase("", TableFont));
                            Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header2);

                            PdfPCell Header3 = new PdfPCell(new Phrase("Funded Enrollment", TableFont));
                            Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header3);

                            PdfPCell Header4 = new PdfPCell(new Phrase("End Period Enrollment", TableFont));
                            Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header4);

                            PdfPCell Header5 = new PdfPCell(new Phrase("Class Days", TableFont));
                            Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header5);

                            PdfPCell Header6 = new PdfPCell(new Phrase("Attendance Days", TableFont));
                            Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header6);

                            PdfPCell Header7 = new PdfPCell(new Phrase("Excused Days", TableFont));
                            Header7.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header7);


                            PdfPCell Header8 = new PdfPCell(new Phrase("Available Days", TableFont));
                            Header8.HorizontalAlignment = Element.ALIGN_CENTER;
                            // Header8.Colspan = 2;
                            Header8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(Header8);


                            PdfPCell Header9 = new PdfPCell(new Phrase("Ave Daily Attendance", TableFont));
                            Header9.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header9.Colspan = 2;
                            Header9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            hssb2111_Table.AddCell(Header9);

                            PdfPCell pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.Colspan = 10;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            hssb2111_Table.AddCell(pdfttlspace1);

                            PdfPCell pdfttlData1 = new PdfPCell(new Phrase("Program wide totals", TableFont));
                            pdfttlData1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlData1.Colspan = 2;
                            pdfttlData1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            hssb2111_Table.AddCell(pdfttlData1);


                            PdfPCell pdfttlData3 = new PdfPCell(new Phrase(intprogFundEnrollmentTot.ToString(), TableFont));
                            pdfttlData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData3);

                            PdfPCell pdfttlData4 = new PdfPCell(new Phrase(intprogEndEnrollmentTot.ToString(), TableFont));
                            pdfttlData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData4);

                            PdfPCell pdfttlData5 = new PdfPCell(new Phrase(intprogClassDaysTot.ToString(), TableFont));
                            pdfttlData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData5);

                            PdfPCell pdfttlData6 = new PdfPCell(new Phrase(intprogAttandanceDaysTot.ToString(), TableFont));
                            pdfttlData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData6);

                            PdfPCell pdfttlData7 = new PdfPCell(new Phrase(intprogExcusedDaysTot.ToString(), TableFont));
                            pdfttlData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData7);


                            PdfPCell pdfttlData8 = new PdfPCell(new Phrase(intprogAvailbleDaysTot.ToString(), TableFont));
                            pdfttlData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData8);

                            string i = Math.Round(intprogAvg).ToString();

                            PdfPCell pdfttlData9 = new PdfPCell(new Phrase(i, TableFont));
                            pdfttlData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_Table.AddCell(pdfttlData9);

                            PdfPCell pdfttlData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", (intprogAvgPer)) + "%", TableFont));
                            pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfttlData10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER; ;
                            hssb2111_Table.AddCell(pdfttlData10);

                            if (hssb2111_Table.Rows.Count > 0)
                            {
                                document.Add(hssb2111_Table);
                                hssb2111_Table.DeleteBodyRows();
                                document.NewPage();
                            }
                        }
                    }

                    // ******** Summary Report *************//

                    if (rdoDetailBoth.Checked == true || rdoSummaryO.Checked == true)
                    {

                        string SelFunds = "A";
                        if (rdoSelected.Checked)
                        {
                            if (SelFundingList.Count > 0)
                            {
                                SelFunds = string.Empty;
                                foreach (CommonEntity Entity in SelFundingList)
                                {
                                    if (!SelFunds.Equals(string.Empty))
                                    {
                                        SelFunds += ",";
                                    }
                                    SelFunds += Entity.Code;
                                }
                            }
                        }

                        List<CaseEnrlEntity> chldAttnSummary = _model.EnrollData.Get2111SummaryDetails(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strOnlySites, "HSSB2111SUMMARY", SelFunds);

                        int intAvgTot = 0;
                        int intFreeTot = 0;
                        int intReducedTot = 0;
                        int intOverIncomeTot = 0;
                        int intATot = 0;
                        int intBreakfastTot = 0;
                        int intLunchTot = 0;
                        int intSupperTot = 0;
                        int intSupplementTot = 0;
                        int intBAllTot = 0;
                        decimal intAverageCalcu = 0;
                        if (rdoSummaryO.Checked == true)
                            document.NewPage();
                        PdfPTable hssb2111_SummaryTable = new PdfPTable(11);
                        hssb2111_SummaryTable.TotalWidth = 550f;
                        hssb2111_SummaryTable.WidthPercentage = 100;
                        hssb2111_SummaryTable.LockedWidth = true;
                        float[] summarywidths = new float[] { 50f, 25f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 25f, 20f };
                        hssb2111_SummaryTable.SetWidths(summarywidths);
                        hssb2111_SummaryTable.HorizontalAlignment = Element.ALIGN_CENTER;


                        PdfPCell SumHeader = new PdfPCell(new Phrase("", TableFont));
                        SumHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                        SumHeader.Colspan = 2;
                        SumHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumHeader);

                        PdfPCell SumHeader1 = new PdfPCell(new Phrase("No. of Participants Enrolled by Category", TableFont));
                        SumHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                        SumHeader1.Colspan = 4;
                        SumHeader1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumHeader1);


                        PdfPCell SumHeader2 = new PdfPCell(new Phrase("No. of Meals Served to Enrolled Participants", TableFont));
                        SumHeader2.HorizontalAlignment = Element.ALIGN_LEFT;
                        SumHeader2.Colspan = 5;
                        SumHeader2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumHeader2);


                        PdfPCell SumSiteHeader = new PdfPCell(new Phrase("Site", TableFont));
                        SumSiteHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumSiteHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSiteHeader);

                        PdfPCell SumDailyAttHeader = new PdfPCell(new Phrase("Ave Daily Attendance", TableFont));
                        SumDailyAttHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumDailyAttHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumDailyAttHeader);

                        PdfPCell SumDailyAttFree = new PdfPCell(new Phrase("Free", TableFont));
                        SumDailyAttFree.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumDailyAttFree.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumDailyAttFree);

                        PdfPCell SumReduced = new PdfPCell(new Phrase("Reduced", TableFont));
                        SumReduced.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumReduced.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumReduced);

                        PdfPCell SumOverIncome = new PdfPCell(new Phrase("Over Income", TableFont));
                        SumOverIncome.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumOverIncome.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumOverIncome);

                        PdfPCell SumTotal = new PdfPCell(new Phrase("Total", TableFont));
                        SumTotal.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumTotal);

                        PdfPCell SumBreakfast = new PdfPCell(new Phrase("Breakfast", TableFont));
                        SumBreakfast.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumBreakfast.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumBreakfast);

                        PdfPCell SumLunch = new PdfPCell(new Phrase("Lunch", TableFont));
                        SumLunch.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumLunch.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumLunch);

                        PdfPCell SumSupper = new PdfPCell(new Phrase("Supper", TableFont));
                        SumSupper.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumSupper.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSupper);

                        PdfPCell SumSupleme = new PdfPCell(new Phrase("Supplements", TableFont));
                        SumSupleme.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumSupleme.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSupleme);


                        PdfPCell SumTotal2 = new PdfPCell(new Phrase("Total", TableFont));
                        SumTotal2.HorizontalAlignment = Element.ALIGN_CENTER;
                        SumTotal2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                        hssb2111_SummaryTable.AddCell(SumTotal2);

                        foreach (CaseEnrlEntity item in chldAttnSummary)
                        {
                            CaseSiteEntity caseSiteDetails = caseSiteAllSiteList.Find(u => u.SiteNUMBER.Trim() == item.Site.Trim());
                            if (caseSiteDetails != null)
                            {
                                intAverageCalcu = 0;
                                if (Convert.ToInt32(item.ClassDays) > 0)
                                    intAverageCalcu = Convert.ToDecimal(item.AttandanceDays) / Convert.ToDecimal(item.ClassDays);
                                PdfPCell SumSiteData = new PdfPCell(new Phrase(caseSiteDetails.SiteNAME, TableFont));
                                SumSiteData.HorizontalAlignment = Element.ALIGN_LEFT;
                                SumSiteData.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                hssb2111_SummaryTable.AddCell(SumSiteData);

                                PdfPCell SumDailyAttData = new PdfPCell(new Phrase(Math.Round(intAverageCalcu).ToString(), TableFont));
                                SumDailyAttData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumDailyAttData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumDailyAttData);

                                PdfPCell SumDailyAttFreeData = new PdfPCell(new Phrase(item.Free, TableFont));
                                SumDailyAttFreeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumDailyAttFreeData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumDailyAttFreeData);

                                PdfPCell SumReducedData = new PdfPCell(new Phrase(item.Reduced, TableFont));
                                SumReducedData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumReducedData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumReducedData);

                                PdfPCell SumOverIncomeData = new PdfPCell(new Phrase(item.OverIncome, TableFont));
                                SumOverIncomeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumOverIncomeData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumOverIncomeData);

                                PdfPCell SumTotalData = new PdfPCell(new Phrase((Convert.ToInt32(item.Free) + Convert.ToInt32(item.Reduced) + Convert.ToInt32(item.OverIncome)).ToString(), TableFont));
                                SumTotalData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumTotalData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumTotalData);

                                PdfPCell SumBreakfastData = new PdfPCell(new Phrase(item.Breakfast, TableFont));
                                SumBreakfastData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumBreakfastData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumBreakfastData);

                                PdfPCell SumLunchData = new PdfPCell(new Phrase(item.Lunch, TableFont));
                                SumLunchData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumLunchData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumLunchData);

                                PdfPCell SumSupperData = new PdfPCell(new Phrase(item.Supper, TableFont));
                                SumSupperData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumSupperData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumSupperData);

                                PdfPCell SumSuplemeData = new PdfPCell(new Phrase(item.Suppliment, TableFont));
                                SumSuplemeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumSuplemeData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_SummaryTable.AddCell(SumSuplemeData);

                                int intBreakfastTotal = Convert.ToInt32(item.Breakfast) + Convert.ToInt32(item.Lunch) + Convert.ToInt32(item.Supper) + Convert.ToInt32(item.Suppliment);
                                intBreakfastTot = Convert.ToInt32(item.Breakfast) + intBreakfastTot;
                                intLunchTot = Convert.ToInt32(item.Lunch) + intLunchTot;
                                intSupperTot = Convert.ToInt32(item.Supper) + intSupperTot;
                                intSupplementTot = Convert.ToInt32(item.Suppliment) + intSupplementTot;
                                intAvgTot = Convert.ToInt32(Math.Round(intAverageCalcu)) + intAvgTot;
                                intFreeTot = Convert.ToInt32(item.Free) + intFreeTot;
                                intReducedTot = Convert.ToInt32(item.Reduced) + intReducedTot;
                                intOverIncomeTot = Convert.ToInt32(item.OverIncome) + intOverIncomeTot;
                                intATot = (Convert.ToInt32(item.Free) + Convert.ToInt32(item.Reduced) + Convert.ToInt32(item.OverIncome)) + intATot;
                                intBAllTot = intBreakfastTotal + intBAllTot;

                                PdfPCell SumTotal2Data = new PdfPCell(new Phrase(intBreakfastTotal.ToString(), TableFont));
                                SumTotal2Data.HorizontalAlignment = Element.ALIGN_RIGHT;
                                SumTotal2Data.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                hssb2111_SummaryTable.AddCell(SumTotal2Data);

                            }
                        }


                        PdfPCell SumSiteTotal = new PdfPCell(new Phrase("Total", TableFont));
                        SumSiteTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                        SumSiteTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSiteTotal);

                        PdfPCell SumDailyAttTot = new PdfPCell(new Phrase(intAvgTot.ToString(), TableFont));
                        SumDailyAttTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumDailyAttTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumDailyAttTot);

                        PdfPCell SumDailyAttFreeTot = new PdfPCell(new Phrase(intFreeTot.ToString(), TableFont));
                        SumDailyAttFreeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumDailyAttFreeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumDailyAttFreeTot);

                        PdfPCell SumReducedTot = new PdfPCell(new Phrase(intReducedTot.ToString(), TableFont));
                        SumReducedTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumReducedTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumReducedTot);

                        PdfPCell SumOverIncomeTot = new PdfPCell(new Phrase(intOverIncomeTot.ToString(), TableFont));
                        SumOverIncomeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumOverIncomeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumOverIncomeTot);

                        PdfPCell SumTotalTot = new PdfPCell(new Phrase(intATot.ToString(), TableFont));
                        SumTotalTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumTotalTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumTotalTot);

                        PdfPCell SumBreakfastTot = new PdfPCell(new Phrase(intBreakfastTot.ToString(), TableFont));
                        SumBreakfastTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumBreakfastTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumBreakfastTot);

                        PdfPCell SumLunchTot = new PdfPCell(new Phrase(intLunchTot.ToString(), TableFont));
                        SumLunchTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumLunchTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumLunchTot);

                        PdfPCell SumSupperTot = new PdfPCell(new Phrase(intSupperTot.ToString(), TableFont));
                        SumSupperTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumSupperTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSupperTot);

                        PdfPCell SumSuplemeTot = new PdfPCell(new Phrase(intSupplementTot.ToString(), TableFont));
                        SumSuplemeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumSuplemeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        hssb2111_SummaryTable.AddCell(SumSuplemeTot);


                        PdfPCell SumTotal2Tot = new PdfPCell(new Phrase(intBAllTot.ToString(), TableFont));
                        SumTotal2Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                        SumTotal2Tot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                        hssb2111_SummaryTable.AddCell(SumTotal2Tot);

                        if (hssb2111_SummaryTable.Rows.Count > 0)
                        {
                            document.Add(hssb2111_SummaryTable);
                            hssb2111_SummaryTable.DeleteBodyRows();
                            document.NewPage();
                        }
                    }

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();

                AlertBox.Show("Report Generated Successfully");
            }
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
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);*/

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

           /* PdfPCell row2 = new PdfPCell(new Phrase("Run By :" + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
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

            /*string Agy = "Agency : All"; string Det = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = "Agency : " + Agency;
            if (Dept != "**") Det = "Dept : " + Dept;
            if (Prog != "**") Prg = "Program : " + Prog;
            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Dept != "**") Dept = /*"Dept : " + */Dept;
            if (Prog != "**") Prg = /*"Program : " +*/ Prog;
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

            string Site = string.Empty;
            if (rdoAllSite.Checked == true) Site = "For All Sites";
            else
            {
                string Selsites = string.Empty;
                if (Sel_REFS_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Sel_REFS_List)
                    {
                        Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + "  ";
                    }
                }
                Site = /*"Site : " + */rdoMultipleSites.Text + " ( " + Selsites + " ) ";
            }
            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Site", TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Site, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            string strReportType = string.Empty;
            if (rdoDetailBoth.Checked == true)
                strReportType = rdoDetailBoth.Text;
            else if (rdoDetail.Checked == true)
                strReportType = rdoDetail.Text;
            else if (rdoSummaryO.Checked == true)
                strReportType = rdoSummaryO.Text;
            //   cb.ShowTextAligned(100, "Report Type : " + strReportType, 120, 530, 0);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Report Type"/* + strReportType*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase(strReportType, TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            string strFundingSource = string.Empty;
            if (rdoFSourceAll.Checked == true) strFundingSource = "All Funds";
            else
            {
                string SelFunds = string.Empty;
                if (SelFundingList.Count > 0)
                {
                    foreach (CommonEntity Entity in SelFundingList)
                    {
                        SelFunds += Entity.Code + "  ";
                    }
                }
                strFundingSource = "Selected Funds: " + SelFunds;
            }

            PdfPCell R7 = new PdfPCell(new Phrase("  " + "Funding Sources", TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            Headertable.AddCell(R7);

            PdfPCell R77 = new PdfPCell(new Phrase(strFundingSource, TableFont));
            R77.HorizontalAlignment = Element.ALIGN_LEFT;
            R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R77.PaddingBottom = 5;
            Headertable.AddCell(R77);

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Report From Date" /*+ dtForm.Text.Trim() + "  To Date: " + dtTodate.Text.Trim()*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(dtForm.Text.Trim() + "   To Date: " + dtTodate.Text.Trim(), TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            //PdfPCell R3 = new PdfPCell(new Phrase("Site :" + (rdoAllSite.Checked == true ? rdoAllSite.Text : rdoMultipleSites.Text), TableFont));
            //R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //R3.Colspan = 2;
            //R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R3);

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Site for Home Based" /*+ ((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Text.ToString().Trim()*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Text.ToString().Trim(), TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "All Funded Days" /*+ (rdoAllFundYes.Checked == true ? "Yes" : "No")*/, TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            Headertable.AddCell(R6);

            PdfPCell R66 = new PdfPCell(new Phrase((rdoAllFundYes.Checked == true ? "Yes" : "No"), TableFont));
            R66.HorizontalAlignment = Element.ALIGN_LEFT;
            R66.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R66.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R66.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R66.PaddingBottom = 5;
            Headertable.AddCell(R66);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private string GetModuleDesc()
        {
            string ModuleDesc = null;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["APPL_CODE"].ToString() == Privileges.ModuleCode)
                {
                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                }
            }
            return ModuleDesc;
        }

        #endregion

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(rdoMultipleSites, null);
            _errorProvider.SetError(rdoSelected, null);
            _errorProvider.SetError(dtForm, null);
            _errorProvider.SetError(dtTodate, null);

            if (dtForm.Checked == false)
            {
                _errorProvider.SetError(dtForm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report From Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtForm, null);
            }
            if (dtTodate.Checked == false)
            {
                _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report To Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtTodate, null);
            }

            if (dtForm.Checked == true || dtTodate.Checked == true)
            {
                if (dtForm.Checked == false && dtTodate.Checked == true)
                {
                    _errorProvider.SetError(dtForm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtForm, null);
                }
            }

            if (dtForm.Checked == true || dtTodate.Checked == true)
            {
                if (dtTodate.Checked == false && dtForm.Checked == true)
                {
                    _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }
            }
            if (dtForm.Checked.Equals(true) && dtTodate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtForm.Text))
                {
                    _errorProvider.SetError(dtForm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtForm, null);
                }
                if (string.IsNullOrWhiteSpace(dtTodate.Text))
                {
                    _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }
            }

            if (dtForm.Checked && dtTodate.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtForm.Text) && !string.IsNullOrWhiteSpace(dtTodate.Text))
                {
                    if (dtForm.Value.Date > dtTodate.Value.Date)
                    {
                        _errorProvider.SetError(dtTodate, "Report 'To Date' should be equal to or greater than 'From Date'");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtTodate, null);
                    }
                }
            }

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



            if (chkDetailReport.Checked)
            {
                if (dtForm.Checked && dtTodate.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(dtForm.Text) && !string.IsNullOrWhiteSpace(dtTodate.Text))
                    {
                        if (dtForm.Value.Month != dtTodate.Value.Month)
                        {
                            _errorProvider.SetError(dtTodate, "From and To Dates should be of the same Month in a Year");  //_errorProvider.SetError(dtTodate, "Please select Same Month");
                            isValid = false;
                        }
                        if (dtForm.Value.Year != dtTodate.Value.Year)
                        {
                            _errorProvider.SetError(dtTodate, "From and To Dates should be of the same Month in a Year");  //_errorProvider.SetError(dtTodate, "Please select Same Month");
                            isValid = false;
                        }
                        //if (dtForm.Value.DayOfWeek == DayOfWeek.Sunday || dtForm.Value.DayOfWeek == DayOfWeek.Saturday)
                        //{
                        //    _errorProvider.SetError(dtTodate, "From Date should be a Weekday");
                        //    isValid = false;
                        //}
                    }
                }
            }

            return (isValid);
        }

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtTodate, null);
            _errorProvider.SetError(dtForm, null);
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
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            string Category = string.Empty;
            if (cmbSiteforHome.Items.Count > 0)
                Category = ((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Value.ToString();
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strReportType = string.Empty;
            if (rdoDetailBoth.Checked == true)
                strReportType = "A";
            else if (rdoDetail.Checked == true)
                strReportType = "D";
            else if (rdoSummaryO.Checked == true)
                strReportType = "S";

            string strAllFundedDays = rdoAllFundYes.Checked ? "Y" : "N";
            string strFundSource = rdoFSourceAll.Checked ? "Y" : "N";

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
            if (rdoSelected.Checked == true)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }



            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" ReportType = \"" + strReportType + "\" FundedDays = \"" + strAllFundedDays + "\" FundedSource = \"" + strFundSource + "\" SiteHomeBased = \"" + Category + "\"  FromDate = \"" + dtForm.Value.Date + "\" ToDate = \"" + dtTodate.Value.Date + "\" SiteNames = \"" + strsiteRoomNames + "\" FundingCode = \"" + strFundingCodes + "\" DetailsExcel = \"" + (chkDetailReport.Checked == true ? "Y" : "N") + "\" />");
            str.Append("</Rows>");

            return str.ToString();
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

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                DataColumnCollection columns = Tmp_Table.Columns;

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbSiteforHome, dr["SiteHomeBased"].ToString());

                if (dr["Site"].ToString() == "A")
                    rdoAllSite.Checked = true;
                else
                {
                    rdoMultipleSites.Checked = true;
                    Sel_REFS_List.Clear();
                    string strSites = dr["SiteNames"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = AllSitesEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                            Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }
                Sel_REFS_List = Sel_REFS_List;
                if (dr["ReportType"].ToString() == "A")
                    rdoDetailBoth.Checked = true;

                else if (dr["ReportType"].ToString() == "D")
                    rdoDetail.Checked = true;

                else if (dr["ReportType"].ToString() == "S")
                    rdoSummaryO.Checked = true;

                if (dr["FundedDays"].ToString() == "Y")
                    rdoAllFundYes.Checked = true;
                else
                    rdoAllFundNo.Checked = true;
                if (dr["FundedSource"].ToString() == "Y")
                    rdoFSourceAll.Checked = true;
                else
                {
                    rdoSelected.Checked = true;
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

                dtForm.Value = Convert.ToDateTime(dr["FromDate"].ToString());
                dtTodate.Value = Convert.ToDateTime(dr["ToDate"].ToString());
                if (columns.Contains("DetailsExcel"))
                {
                    if (dr["DetailsExcel"].ToString() == "Y")
                        chkDetailReport.Checked = true;
                    else
                        chkDetailReport.Checked = false;
                }

            }
        }


        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
            }
        }


        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
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
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Sel_REFS_List, "Report", Agency.Trim(), Dept.Trim(), Prog.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        private void rdoSelected_Click(object sender, EventArgs e)
        {
            if (rdoSelected.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        List<Headstart_Template> Template_List = new List<Headstart_Template>();
        private void GetHeadstartTemplate_Values()
        {
            Template_List = _model.CaseMstData.GetHeadstartTemplate(Consts.AgyTab.HSCALENDARDAYSTATS, "00000");
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

            if (ValidateForm())
            {
                propCaseEnrlList = _model.EnrollData.Get2111ExcelDetails(Agency, Dept, Prog, Program_Year, string.Empty, "A", "A", "HSSB2111EXCEL", dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString());

                propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");


                propChldAttnList = _model.ChldAttnData.GetChldAttnBetweenDatehssb2109(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), string.Empty, string.Empty, "ALL");

                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
                pdfListForm.FormClosed += new FormClosedEventHandler(On_ExcelSaveFormClosed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }

        }

        private void On_ExcelSaveFormClosed(object sender, EventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string ExcelName = "Pdf File";
                ExcelName = form.GetFileName();
                ExcelreportData(ExcelName);
            }
        }

        #region Excel Format

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Defaultt");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Calibri Light";
            s16.Font.Size = 16;
            s16.Interior.Color = "#DDEBF7";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s16.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.Bold = true;
            s17.Font.FontName = "Calibri Light";
            s17.Font.Size = 16;
            s17.Interior.Color = "#DDEBF7";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s17.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Interior.Color = "#DDEBF7";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s18.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Interior.Color = "#DDEBF7";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Interior.Color = "#DDEBF7";
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Calibri";
            s21.Font.Size = 11;
            s21.Font.Color = "#000000";
            s21.Interior.Color = "#FCE4D6";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s21.Alignment.Vertical = StyleVerticalAlignment.Center;
            s21.Alignment.WrapText = true;
            s21.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s21.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Font.Bold = true;
            s22.Font.FontName = "Calibri";
            s22.Font.Size = 11;
            s22.Font.Color = "#000000";
            s22.Interior.Color = "#FCE4D6";
            s22.Interior.Pattern = StyleInteriorPattern.Solid;
            s22.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s22.Alignment.Vertical = StyleVerticalAlignment.Center;
            s22.Alignment.WrapText = true;
            s22.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s22.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s24.Alignment.Vertical = StyleVerticalAlignment.Center;
            s24.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s25.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s25.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s27.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s27.NumberFormat = "General Date";
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Center;
            s28.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s28.NumberFormat = "General Date";
            // -----------------------------------------------
            //  s29
            // -----------------------------------------------
            WorksheetStyle s29 = styles.Add("s29");
            s29.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s29.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s29.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s30
            // -----------------------------------------------
            WorksheetStyle s30 = styles.Add("s30");
            s30.Font.FontName = "Arial";
            s30.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s30.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s30.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s31
            // -----------------------------------------------
            WorksheetStyle s31 = styles.Add("s31");
            s31.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s31.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s32
            // -----------------------------------------------
            WorksheetStyle s32 = styles.Add("s32");
            s32.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s32.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s33
            // -----------------------------------------------
            WorksheetStyle s33 = styles.Add("s33");
            s33.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s33.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s34
            // -----------------------------------------------
            WorksheetStyle s34 = styles.Add("s34");
            s34.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s34.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s35
            // -----------------------------------------------
            WorksheetStyle s35 = styles.Add("s35");
            // -----------------------------------------------
            //  s36
            // -----------------------------------------------
            WorksheetStyle s36 = styles.Add("s36");
            s36.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s36.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s37
            // -----------------------------------------------
            WorksheetStyle s37 = styles.Add("s37");
            s37.Font.FontName = "Arial";
            s37.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s37.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s38
            // -----------------------------------------------
            WorksheetStyle s38 = styles.Add("s38");
            s38.Font.FontName = "Arial";
            s38.Font.Color = "#000000";
            s38.Interior.Color = "#FFFFFF";
            s38.Interior.Pattern = StyleInteriorPattern.Solid;
            s38.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s38.Alignment.Vertical = StyleVerticalAlignment.Center;
            s38.Alignment.WrapText = true;
            s38.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s39
            // -----------------------------------------------
            WorksheetStyle s39 = styles.Add("s39");
            s39.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s39.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s40
            // -----------------------------------------------
            WorksheetStyle s40 = styles.Add("s40");
            s40.Font.FontName = "Arial";
            s40.Font.Color = "#000000";
            s40.Interior.Color = "#FFFFFF";
            s40.Interior.Pattern = StyleInteriorPattern.Solid;
            s40.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s40.Alignment.Vertical = StyleVerticalAlignment.Center;
            s40.Alignment.WrapText = true;
            s40.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s40.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s41
            // -----------------------------------------------
            WorksheetStyle s41 = styles.Add("s41");
            s41.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s41.Alignment.Vertical = StyleVerticalAlignment.Center;
            s41.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            // -----------------------------------------------
            //  m386123900
            // -----------------------------------------------
            WorksheetStyle m386123900 = styles.Add("m386123900");
            m386123900.Font.FontName = "Calibri";
            m386123900.Font.Size = 12;
            m386123900.Font.Color = "#000000";
            m386123900.Interior.Color = "#F2DCDB";
            m386123900.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123900.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m386123900.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123900.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  m386123920
            // -----------------------------------------------
            WorksheetStyle m386123920 = styles.Add("m386123920");
            m386123920.Font.Bold = true;
            m386123920.Font.FontName = "Calibri";
            m386123920.Font.Size = 11;
            m386123920.Font.Color = "#000000";
            m386123920.Interior.Color = "#DCE6F1";
            m386123920.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123920.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m386123920.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123920.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  m386123940
            // -----------------------------------------------
            WorksheetStyle m386123940 = styles.Add("m386123940");
            m386123940.Font.Bold = true;
            m386123940.Font.FontName = "Calibri";
            m386123940.Font.Size = 12;
            m386123940.Font.Color = "#000000";
            m386123940.Interior.Color = "#FCD5B4";
            m386123940.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123940.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m386123940.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123940.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123940.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123940.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s45
            // -----------------------------------------------
            WorksheetStyle s45 = styles.Add("s45");
            s45.Font.Bold = true;
            s45.Font.FontName = "Calibri";
            s45.Font.Size = 11;
            s45.Font.Color = "#000000";
            s45.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s45.Alignment.Vertical = StyleVerticalAlignment.Top;
            s45.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s45.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s45.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s46
            // -----------------------------------------------
            WorksheetStyle s46 = styles.Add("s46");
            s46.Font.Bold = true;
            s46.Font.FontName = "Calibri";
            s46.Font.Size = 11;
            s46.Font.Color = "#000000";
            s46.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s46.Alignment.Vertical = StyleVerticalAlignment.Center;
            s46.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s46.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s46.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s53
            // -----------------------------------------------
            WorksheetStyle s53 = styles.Add("s53");
            s53.Font.Bold = true;
            s53.Font.FontName = "Calibri";
            s53.Font.Size = 11;
            s53.Font.Color = "#000000";
            s53.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s53.Alignment.Vertical = StyleVerticalAlignment.Top;
            s53.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s53.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s53.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s53.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s54
            // -----------------------------------------------
            WorksheetStyle s54 = styles.Add("s54");
            s54.Font.Bold = true;
            s54.Font.FontName = "Calibri";
            s54.Font.Size = 11;
            s54.Font.Color = "#000000";
            s54.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s54.Alignment.Vertical = StyleVerticalAlignment.Top;
            s54.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s55
            // -----------------------------------------------
            WorksheetStyle s55 = styles.Add("s55");
            s55.Font.Bold = true;
            s55.Font.FontName = "Calibri";
            s55.Font.Size = 11;
            s55.Font.Color = "#000000";
            s55.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s55.Alignment.Vertical = StyleVerticalAlignment.Top;
            s55.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s55.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s55.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s55.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s56
            // -----------------------------------------------
            WorksheetStyle s56 = styles.Add("s56");
            s56.Alignment.Vertical = StyleVerticalAlignment.Center;
            s56.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s56.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s56.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s56.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s57
            // -----------------------------------------------
            WorksheetStyle s57 = styles.Add("s57");
            s57.Alignment.Vertical = StyleVerticalAlignment.Center;
            s57.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s57.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s57.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s57.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s58
            // -----------------------------------------------
            WorksheetStyle s58 = styles.Add("s58");
            s58.Alignment.Vertical = StyleVerticalAlignment.Center;
            s58.Alignment.WrapText = true;
            s58.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s58.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s59
            // -----------------------------------------------
            WorksheetStyle s59 = styles.Add("s59");
            s59.Alignment.Vertical = StyleVerticalAlignment.Center;
            s59.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s59.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s59.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s59.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s60
            // -----------------------------------------------
            WorksheetStyle s60 = styles.Add("s60");
            s60.Alignment.Vertical = StyleVerticalAlignment.Center;
            s60.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s60.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s60.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s60.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s61
            // -----------------------------------------------
            WorksheetStyle s61 = styles.Add("s61");
            s61.Font.Bold = true;
            s61.Font.FontName = "Calibri";
            s61.Font.Size = 11;
            s61.Font.Color = "#000000";
            s61.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s61.Alignment.Vertical = StyleVerticalAlignment.Center;
            s61.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s61.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s61.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Font.Bold = true;
            s62.Font.FontName = "Calibri";
            s62.Font.Size = 11;
            s62.Font.Color = "#000000";
            s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s62.Alignment.Vertical = StyleVerticalAlignment.Center;
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s63.Alignment.Vertical = StyleVerticalAlignment.Center;
            s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s64.Alignment.Vertical = StyleVerticalAlignment.Center;
            s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s65.Alignment.Vertical = StyleVerticalAlignment.Center;
            s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s66.Alignment.Vertical = StyleVerticalAlignment.Center;
            s66.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s66.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Font.Bold = true;
            s67.Font.FontName = "Calibri";
            s67.Font.Size = 16;
            s67.Font.Color = "#000000";
            s67.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s67.Alignment.Vertical = StyleVerticalAlignment.Bottom;
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
            //  s143
            // -----------------------------------------------
            WorksheetStyle s143 = styles.Add("s143");
            s143.Parent = "s137";
            s143.Font.FontName = "Calibri";
            s143.Font.Size = 11;
            s143.Interior.Color = "#FFFFFF";
            s143.Interior.Pattern = StyleInteriorPattern.Solid;
            s143.Alignment.Vertical = StyleVerticalAlignment.Top;
            s143.Alignment.WrapText = true;
            s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
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
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = styles.Add("s145");
            s145.Parent = "s137";
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Interior.Color = "#FFFFFF";
            s145.Interior.Pattern = StyleInteriorPattern.Solid;
            s145.Alignment.Vertical = StyleVerticalAlignment.Top;
            s145.Alignment.WrapText = true;
            s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
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
            s171.Parent = "s137";
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 11;
            s171.Interior.Color = "#FFFFFF";
            s171.Interior.Pattern = StyleInteriorPattern.Solid;
            s171.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172 = styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

            // -----------------------------------------------
            //  s169
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
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170H = styles.Add("s170H");
            s170H.Parent = "s137";
            s170H.Font.FontName = "Calibri";
            s170H.Font.Size = 11;
            s170H.Interior.Color = "#FFFFFF";
            s170H.Interior.Pattern = StyleInteriorPattern.Solid;
            s170H.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
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
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172H = styles.Add("s172H");
            s172H.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        }

        private void GenerateWorksheetParameters(Workbook book)
        {
            try
            {
                Worksheet Psheet = book.Worksheets.Add("Parameters");
                Psheet.Table.DefaultRowHeight = 14.4F;

                WorksheetColumn columnHead = Psheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                Psheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = Psheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172H";
                Psheet.Table.Columns.Add(59);
                // -----------------------------------------------
                WorksheetRow RowHead = Psheet.Table.Rows.Add();
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
                WorksheetRow Row1Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row2Head = Psheet.Table.Rows.Add();
                Row2Head.Height = 12;
                Row2Head.AutoFitHeight = false;
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "m2611536909264";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 14;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row101 = Psheet.Table.Rows.Add();
                Row101.Height = 14;
                Row101.AutoFitHeight = false;
                cell = Row101.Cells.Add();
                cell.StyleID = "s139";
                cell = Row101.Cells.Add();
                cell.StyleID = "s143";
                cell = Row101.Cells.Add();
                cell.StyleID = "m2611540549592";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row88Head = Psheet.Table.Rows.Add();
                Row88Head.Height = 14;
                Row88Head.AutoFitHeight = false;
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row3Head = Psheet.Table.Rows.Add();
                Row3Head.Height = 12;
                Row3Head.AutoFitHeight = false;
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "m2611536909284";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "Selected Report Parameters";
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row4Head = Psheet.Table.Rows.Add();
                Row4Head.Height = 12;
                Row4Head.AutoFitHeight = false;
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row5Head = Psheet.Table.Rows.Add();
                Row5Head.Height = 12;
                Row5Head.AutoFitHeight = false;
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "m2611536909304";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row6Head = Psheet.Table.Rows.Add();
                Row6Head.Height = 12;
                Row6Head.AutoFitHeight = false;
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string Header_year = string.Empty;
                if (CmbYear.Visible == true)
                    Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

                WorksheetRow Row78Head = Psheet.Table.Rows.Add();
                Row78Head.Height = 12;
                Row78Head.AutoFitHeight = false;
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = Txt_HieDesc.Text.Trim() + "     " + Header_year;
                cell.MergeAcross = 2;
                // -----------------------------------------------

                string Site = string.Empty;
                if (rdoAllSite.Checked == true) Site = "For All Sites";
                else
                {
                    string Selsites = string.Empty;
                    if (Sel_REFS_List.Count > 0)
                    {
                        foreach (CaseSiteEntity Entity in Sel_REFS_List)
                        {
                            Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + "  ";
                        }
                    }
                    Site = /*"Site : " + */rdoMultipleSites.Text + " ( " + Selsites + " ) ";
                }

                WorksheetRow Row8 = Psheet.Table.Rows.Add();
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
                // -----------------------------------------------
                Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 14;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "             Site: " + Site.Trim();
                //Row7Head.Cells.Add("            Site: ", DataType.String, "s144");
                //Row7Head.Cells.Add(Site, DataType.String, "s169");
                cell.MergeAcross = 2;

                // -----------------------------------------------
                Row8 = Psheet.Table.Rows.Add();
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
                // -----------------------------------------------
                string strReportType = string.Empty;
                if (rdoDetailBoth.Checked == true)
                    strReportType = rdoDetailBoth.Text;
                else if (rdoDetail.Checked == true)
                    strReportType = rdoDetail.Text;
                else if (rdoSummaryO.Checked == true)
                    strReportType = rdoSummaryO.Text;

                Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 14;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "             Report Type: " + strReportType;
                //Row7Head.Cells.Add("            Report Type: ", DataType.String, "s144");
                //Row7Head.Cells.Add(strReportType, DataType.String, "s169");
                cell.MergeAcross = 2;

                // -----------------------------------------------
                Row8 = Psheet.Table.Rows.Add();
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
                // -----------------------------------------------
                string strFundingSource = string.Empty;
                if (rdoFSourceAll.Checked == true) strFundingSource = "All Funds";
                else
                {
                    string SelFunds = string.Empty;
                    if (SelFundingList.Count > 0)
                    {
                        foreach (CommonEntity Entity in SelFundingList)
                        {
                            SelFunds += Entity.Code + "  ";
                        }
                    }
                    strFundingSource = "Selected Funds: " + SelFunds;
                }

                Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 14;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "             " + "Funding Sources" + ": " + strFundingSource;
                //Row7Head.Cells.Add("            Funding Sources: ", DataType.String, "s144");
                //Row7Head.Cells.Add(strFundingSource, DataType.String, "s169");
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row133 = Psheet.Table.Rows.Add();
                Row133.Height = 14;
                Row133.AutoFitHeight = false;
                cell = Row133.Cells.Add();
                cell.StyleID = "s139";
                cell = Row133.Cells.Add();
                cell.StyleID = "s143";
                cell = Row133.Cells.Add();
                cell.StyleID = "s139";
                cell = Row133.Cells.Add();
                cell.StyleID = "s170";
                cell = Row133.Cells.Add();
                cell.StyleID = "s145";

                // -----------------------------------------------
                if (dtForm.Checked == true && dtTodate.Checked == true)
                {
                    WorksheetRow IntDte = Psheet.Table.Rows.Add();
                    IntDte.Height = 14;
                    IntDte.AutoFitHeight = false;
                    cell = IntDte.Cells.Add();
                    cell.StyleID = "s139";
                    cell = IntDte.Cells.Add();
                    cell.StyleID = "s143";
                    cell.StyleID = "m2611536909324";
                    IntDte.Cells.Add("           Report From Date: ", DataType.String, "s144");
                    IntDte.Cells.Add(dtForm.Text + "   To: " + dtTodate.Text, DataType.String, "s169");
                    //IntDte.Cells.Add(CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtForm.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                    //    + "   To: " + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtTodate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat));
                    cell = IntDte.Cells.Add();
                    cell.StyleID = "s145";
                    Row8 = Psheet.Table.Rows.Add();
                }
                //// -----------------------------------------------
                //WorksheetRow Row011 = Psheet.Table.Rows.Add();
                //Row011.Height = 14;
                //Row011.AutoFitHeight = false;
                //cell = Row011.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row011.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row011.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row011.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row011.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                //WorksheetRow ReportType = Psheet.Table.Rows.Add();
                //ReportType.Height = 14;
                //ReportType.AutoFitHeight = false;
                //cell = ReportType.Cells.Add();
                //cell.StyleID = "s139";
                //cell = ReportType.Cells.Add();
                //cell.StyleID = "s143";
                //ReportType.Cells.Add("           Detail Excel Report: ", DataType.String, "s144");
                //ReportType.Cells.Add((chkDetailReport.Checked == true ? "Yes" : "No"), DataType.String, "s169");
                //cell = ReportType.Cells.Add();
                //cell.StyleID = "s145";
                //// -----------------------------------------------
                WorksheetRow Row13 = Psheet.Table.Rows.Add();
                Row13.Height = 14;
                Row13.AutoFitHeight = false;
                cell = Row13.Cells.Add();
                cell.StyleID = "s139";
                cell = Row13.Cells.Add();
                cell.StyleID = "s143";
                cell = Row13.Cells.Add();
                cell.StyleID = "s139";
                cell = Row13.Cells.Add();
                cell.StyleID = "s170";
                cell = Row13.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow AppType = Psheet.Table.Rows.Add();
                AppType.Height = 14;
                AppType.AutoFitHeight = false;
                cell = AppType.Cells.Add();
                cell.StyleID = "s139";
                cell = AppType.Cells.Add();
                cell.StyleID = "s143";
                AppType.Cells.Add("           Site For Home Based: ", DataType.String, "s144");
                AppType.Cells.Add(((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Text.Trim(), DataType.String, "s169");
                cell = AppType.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row11 = Psheet.Table.Rows.Add();
                Row11.Height = 14;
                Row11.AutoFitHeight = false;
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s143";
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s170";
                cell = Row11.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow ReportType = Psheet.Table.Rows.Add();
                ReportType.Height = 14;
                ReportType.AutoFitHeight = false;
                cell = ReportType.Cells.Add();
                cell.StyleID = "s139";
                cell = ReportType.Cells.Add();
                cell.StyleID = "s143";
                ReportType.Cells.Add("           All Funded Days: ", DataType.String, "s144");
                ReportType.Cells.Add((rdoAllFundYes.Checked == true ? "Yes" : "No"), DataType.String, "s169");
                cell = ReportType.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
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
                // -----------------------------------------------
                WorksheetRow Row24 = Psheet.Table.Rows.Add();
                Row24.Height = 12;
                Row24.AutoFitHeight = false;
                cell = Row24.Cells.Add();
                cell.StyleID = "s139";
                cell = Row24.Cells.Add();
                cell.StyleID = "s143";
                cell = Row24.Cells.Add();
                cell.StyleID = "m2611540549632";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row25 = Psheet.Table.Rows.Add();
                Row25.Height = 12;
                Row25.AutoFitHeight = false;
                cell = Row25.Cells.Add();
                cell.StyleID = "s139";
                cell = Row25.Cells.Add();
                cell.StyleID = "s143";
                cell = Row25.Cells.Add();
                cell.StyleID = "m2611540549652";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row26Head = Psheet.Table.Rows.Add();
                Row26Head.Height = 12;
                Row26Head.AutoFitHeight = false;
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row27Head = Psheet.Table.Rows.Add();
                Row27Head.Height = 12;
                Row27Head.AutoFitHeight = false;
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "m2611540549672";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row28 = Psheet.Table.Rows.Add();
                Row28.Height = 12;
                Row28.AutoFitHeight = false;
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s143";
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row28.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row29 = Psheet.Table.Rows.Add();
                Row29.Height = 12;
                Row29.AutoFitHeight = false;
                cell = Row29.Cells.Add();
                cell.StyleID = "s139";
                cell = Row29.Cells.Add();
                cell.StyleID = "s143";
                cell = Row29.Cells.Add();
                cell.StyleID = "m2611540549552";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row30 = Psheet.Table.Rows.Add();
                Row30.Height = 12;
                Row30.AutoFitHeight = false;
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s143";
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row30.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row31 = Psheet.Table.Rows.Add();
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
                Psheet.Options.Selected = true;
                Psheet.Options.ProtectObjects = false;
                Psheet.Options.ProtectScenarios = false;
                Psheet.Options.PageSetup.Header.Margin = 0.3F;
                Psheet.Options.PageSetup.Footer.Margin = 0.3F;
                Psheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
                Psheet.Options.PageSetup.PageMargins.Left = 0.7F;
                Psheet.Options.PageSetup.PageMargins.Right = 0.7F;
                Psheet.Options.PageSetup.PageMargins.Top = 0.75F;
            }
            catch (Exception e) { }
        }

        private void ExcelreportData(string ExcelName)
        {
            List<CaseSiteEntity> Site_list = new List<CaseSiteEntity>();
            if (rdoAllSite.Checked == true)
            {
                CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
                Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                Site_list = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                Site_list = Site_list.FindAll(u => u.SiteROOM.Trim() != "0000");

                List<CaseSiteEntity> SelSites = new List<CaseSiteEntity>();
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.InActiveFlag == "N")
                        {
                            if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
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
                                    foreach (CaseSiteEntity casesite in Site_list) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                        {
                                            SelSites.Add(casesite);
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            //strsiteRoomNames = hierarchyEntity.Sites;
                            Site_list = SelSites;
                        }
                    }


                }

            }
            else
            {
                Site_list = Sel_REFS_List;
            }

            StringBuilder strMstApplUpdate = new StringBuilder();

            string PdfName = "Pdf File";
            PdfName = ExcelName;
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
                Workbook book = new Workbook();

                GenerateStyles(book.Styles);
                GenerateWorksheetParameters(book);

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Calibri";
                styleb.Font.Size = 9;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                styleb.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Calibri";
                style.Font.Size = 13;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle HeaderStyleSmallblue = book.Styles.Add("HeaderStyleSmallblue");
                HeaderStyleSmallblue.Font.FontName = "Calibri";
                HeaderStyleSmallblue.Font.Size = 11;
                HeaderStyleSmallblue.Font.Bold = true;
                HeaderStyleSmallblue.Font.Color = "#0000FF";
                HeaderStyleSmallblue.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallblue.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallblue.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallblue.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle stylesmall = book.Styles.Add("HeaderStyleSmall");
                stylesmall.Font.FontName = "Calibri";
                stylesmall.Font.Size = 11;
                stylesmall.Font.Bold = true;
                stylesmall.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                stylesmall.Alignment.Vertical = StyleVerticalAlignment.Center;
                stylesmall.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                stylesmall.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle HeaderStyleSmallApcent = book.Styles.Add("HeaderStyleSmallApcent");
                HeaderStyleSmallApcent.Font.FontName = "Calibri";
                HeaderStyleSmallApcent.Font.Size = 11;
                HeaderStyleSmallApcent.Font.Bold = true;
                HeaderStyleSmallApcent.Interior.Color = "#FFCDD2";
                HeaderStyleSmallApcent.Interior.Pattern = StyleInteriorPattern.Solid;
                HeaderStyleSmallApcent.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallApcent.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallApcent.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle HeaderStyleSmallClosed = book.Styles.Add("HeaderStyleSmallClosed");
                HeaderStyleSmallClosed.Font.FontName = "Calibri";
                HeaderStyleSmallClosed.Font.Size = 11;
                HeaderStyleSmallClosed.Font.Bold = true;
                HeaderStyleSmallClosed.Interior.Color = "#E1DDDC";
                HeaderStyleSmallClosed.Interior.Pattern = StyleInteriorPattern.Solid;
                HeaderStyleSmallClosed.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleSmallClosed.Alignment.Vertical = StyleVerticalAlignment.Center;
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleSmallClosed.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Calibri";
                style1.Font.Size = 11;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style1.Alignment.WrapText = true;
                style1.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style1.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle HeaderStyleRotate = book.Styles.Add("HeaderStyleRotate");
                HeaderStyleRotate.Font.FontName = "Calibri";
                HeaderStyleRotate.Font.Size = 11;
                HeaderStyleRotate.Font.Bold = true;
                HeaderStyleRotate.Alignment.Rotate = 90;
                HeaderStyleRotate.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                HeaderStyleRotate.Alignment.WrapText = true;
                HeaderStyleRotate.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                HeaderStyleRotate.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Calibri";
                style2.Font.Size = 11;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle stylesunday = book.Styles.Add("CellStylesunday");
                style2.Font.FontName = "Calibri";
                style2.Font.Size = 11;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle ResponseStyle = book.Styles.Add("ResponseStyle");
                ResponseStyle.Font.FontName = "Calibri";
                ResponseStyle.Font.Size = 10;
                ResponseStyle.Font.Bold = true;
                ResponseStyle.Interior.Color = "#000000";
                ResponseStyle.Interior.Pattern = StyleInteriorPattern.Solid;
                ResponseStyle.Alignment.Vertical = StyleVerticalAlignment.Center;
                ResponseStyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                ResponseStyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                ResponseStyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle ResponseStylegreen = book.Styles.Add("ResponseStylegreen");
                ResponseStylegreen.Font.FontName = "Calibri";
                ResponseStylegreen.Font.Size = 11;
                ResponseStylegreen.Font.Bold = true;
                ResponseStylegreen.Interior.Color = "#00FF00";
                ResponseStylegreen.Interior.Pattern = StyleInteriorPattern.Solid;
                ResponseStylegreen.Alignment.Vertical = StyleVerticalAlignment.Center;
                ResponseStylegreen.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                ResponseStylegreen.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                ResponseStylegreen.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                style = book.Styles.Add("Default");
                style.Font.FontName = "Calibri";
                style.Font.Size = 9;
                style.Font.Bold = true;

                WorksheetStyle Default12 = book.Styles.Add("Default12");
                Default12.Font.FontName = "Calibri";
                Default12.Font.Size = 9;
                Default12.Font.Bold = true;
                Default12.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                Default12.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                SiteScheduleEntity site_Search = new SiteScheduleEntity(true);
                site_Search.ATTM_AGENCY = Agency.Trim(); site_Search.ATTM_DEPT = Dept;
                site_Search.ATTM_PROG = Prog; site_Search.ATTM_YEAR = Program_Year;
                site_Search.ATTM_MONTH = dtForm.Value.Month.ToString();
                List<SiteScheduleEntity> SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");

                foreach (CaseSiteEntity siteroomsitem in Site_list)
                {
                    List<CaseEnrlEntity> caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == siteroomsitem.SiteNUMBER.Trim() && u.Room == siteroomsitem.SiteROOM.Trim() && u.AMPM == siteroomsitem.SiteAM_PM.Trim());

                    List<CaseEnrlEntity> casewaitlistEnrlList = propCaseEnrlList.FindAll(u => u.Site.Trim() == string.Empty);
                    foreach (CaseEnrlEntity waillistitem in casewaitlistEnrlList)
                    {
                        caseEnrlList.Add(new CaseEnrlEntity(waillistitem));
                    }

                    caseEnrlList = caseEnrlList.OrderBy(u => u.Snp_L_Name).ToList();

                    Worksheet sheet = book.Worksheets.Add(siteroomsitem.SiteNUMBER + siteroomsitem.SiteROOM + siteroomsitem.SiteAM_PM);

                    List<ChldAttnEntity> propChldAttnListsitewise = propChldAttnList.FindAll(u => u.SITE == siteroomsitem.SiteNUMBER.Trim() && u.ROOM == siteroomsitem.SiteROOM.Trim() && u.AMPM == siteroomsitem.SiteAM_PM.Trim());


                    sheet.Table.Columns.Add(new WorksheetColumn(20));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(40));
                    sheet.Table.Columns.Add(new WorksheetColumn(20));
                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(20));

                    DateTime today = dtForm.Value;
                    int inttoday = today.Day;
                    inttoday = inttoday - 1;
                    int day = dtTodate.Value.Day;

                    DateTime now = dtForm.Value;
                    int count;
                    count = 0;
                    int intcolumns = 0;
                    for (int i = inttoday; i < day; ++i)
                    {
                        DateTime d = new DateTime(now.Year, now.Month, i + 1);
                        //Compare date with sunday

                        if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(20));
                            intcolumns = intcolumns + 1;
                        }
                        else
                        {

                            if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if ((i + 1) == day)
                                { }
                                else
                                {
                                    intcolumns = intcolumns + 1;
                                    sheet.Table.Columns.Add(new WorksheetColumn(20));
                                }
                            }
                        }

                    }

                    sheet.Table.Columns.Add(new WorksheetColumn(45));
                    sheet.Table.Columns.Add(new WorksheetColumn(45));
                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(35));

                    WorksheetRow row = sheet.Table.Rows.Add();

                    WorksheetCell cell = row.Cells.Add("CENTER " + siteroomsitem.SiteNUMBER + siteroomsitem.SiteROOM + siteroomsitem.SiteAM_PM, DataType.String, "HeaderStyle");
                    cell.MergeAcross = 1;
                    cell.Row.Height = 80;

                    row.Cells.Add(new WorksheetCell("App #", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Status", "HeaderStyleRotate"));
                    row.Cells.Add(new WorksheetCell("Birth Date", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Cat. Eligibility", "HeaderStyleRotate"));
                    cell = row.Cells.Add("DETAIL ATTENDANCE  " + dtForm.Value.ToShortDateString() + "  -  " + dtTodate.Value.ToShortDateString(), DataType.String, "HeaderStyle");
                    cell.MergeAcross = intcolumns - 1;
                    row.Cells.Add(new WorksheetCell("Present/Days Open", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Monthly ADA", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Enroll", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Drop", "HeaderStyle1"));

                    row = sheet.Table.Rows.Add();
                    cell = row.Cells.Add("Student's Name:", DataType.String, "HeaderStyle1");
                    cell.MergeAcross = 1;
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));


                    for (int i = inttoday; i < day; ++i)
                    {
                        DateTime d = new DateTime(now.Year, now.Month, i + 1);
                        //Compare date with sunday

                        if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                        {
                            row.Cells.Add(new WorksheetCell(d.Day.ToString(), "HeaderStyleSmallblue"));
                        }
                        else
                        {
                            if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if ((i + 1) == day)
                                {
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                                }
                            }
                        }
                    }

                    row.Cells.Add(new WorksheetCell("", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "Default12"));

                    int intstudent = 0;
                    int intOpendayscount = 0;
                    int intPresentDaysCount = 0;
                    int intAbsentDaysCount = 0;
                    bool boolApplicantdata = true;
                    foreach (CaseEnrlEntity enrldataitem in caseEnrlList)
                    {
                        boolApplicantdata = true;
                        if ((enrldataitem.Status != "E"))
                        {
                            if ((enrldataitem.Status == "T" && enrldataitem.Withdraw_Date != string.Empty))
                            {
                                if (Convert.ToDateTime(enrldataitem.Enrl_Date) < dtForm.Value)
                                {
                                    boolApplicantdata = false;
                                }
                                else
                                {
                                    List<ChldAttnEntity> enrlappwithdrawAttn = propChldAttnListsitewise.FindAll(u => u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);
                                    if (enrlappwithdrawAttn.Count == 0)
                                    {
                                        boolApplicantdata = false;
                                    }
                                }
                            }
                            else
                            {
                                List<ChldAttnEntity> enrlappwithdrawAttn = propChldAttnListsitewise.FindAll(u => u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);
                                if (enrlappwithdrawAttn.Count == 0)
                                {
                                    boolApplicantdata = false;
                                }
                            }
                        }
                        else
                        {
                            if (enrldataitem.Enrl_Date != string.Empty)
                                if (Convert.ToDateTime(enrldataitem.Enrl_Date) > dtTodate.Value)
                                {
                                    boolApplicantdata = false;
                                }
                        }

                        if (boolApplicantdata)
                        {
                            row = sheet.Table.Rows.Add();
                            intOpendayscount = 0;
                            intPresentDaysCount = 0;
                            intAbsentDaysCount = 0;
                            intstudent = intstudent + 1;
                            row.Cells.Add(new WorksheetCell(intstudent.ToString(), "Default12"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.GetMemberName(enrldataitem.Snp_F_Name, enrldataitem.Snp_M_Name, enrldataitem.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), "Default12"));
                            row.Cells.Add(new WorksheetCell(enrldataitem.App, "Default12"));
                            row.Cells.Add(new WorksheetCell(enrldataitem.Status, "Default12"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(enrldataitem.Snp_DOB), "Default12"));
                            row.Cells.Add(new WorksheetCell(enrldataitem.MST_MealElig, "Default12"));

                            for (int i = inttoday; i < day; ++i)
                            {
                                DateTime d = new DateTime(now.Year, now.Month, i + 1);
                                //Compare date with sunday

                                if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                                {

                                    ChldAttnEntity chldattnmember = propChldAttnListsitewise.Find(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);
                                    if (chldattnmember != null)
                                    {
                                        intOpendayscount = intOpendayscount + 1;
                                        if (chldattnmember.PA.ToString() == "A")
                                        {
                                            intAbsentDaysCount = intAbsentDaysCount + 1;
                                            row.Cells.Add(new WorksheetCell(chldattnmember.PA.ToString(), "HeaderStyleSmallApcent"));
                                        }
                                        else
                                        {
                                            intPresentDaysCount = intPresentDaysCount + 1;
                                            row.Cells.Add(new WorksheetCell(chldattnmember.PA.ToString(), "HeaderStyleSmall"));
                                        }
                                    }
                                    else
                                    {
                                        string strStatus = GetDateStatus(d.Date, siteroomsitem.SiteNUMBER, siteroomsitem.SiteROOM, siteroomsitem.SiteAM_PM, enrldataitem.FundHie, "");
                                        if (strStatus == "C")
                                            row.Cells.Add(new WorksheetCell(strStatus, "HeaderStyleSmallClosed"));
                                        else if (strStatus == "O")
                                        {
                                            if (enrldataitem.Enrl_Date != string.Empty)
                                            {
                                                DateTime dtenrolldate = Convert.ToDateTime(LookupDataAccess.Getdate(enrldataitem.Enrl_Date));
                                                if (d.Date >= dtenrolldate.Date && enrldataitem.Withdraw_Date == string.Empty)
                                                {
                                                    intOpendayscount = intOpendayscount + 1;
                                                }
                                                else
                                                {
                                                    if (enrldataitem.Withdraw_Date != string.Empty)
                                                    {
                                                        DateTime dtenrollwithdate = Convert.ToDateTime(LookupDataAccess.Getdate(enrldataitem.Withdraw_Date));
                                                        if (d.Date < dtenrollwithdate.Date)
                                                        {
                                                            intOpendayscount = intOpendayscount + 1;
                                                        }
                                                    }
                                                }
                                            }
                                            row.Cells.Add(new WorksheetCell("", "HeaderStyleSmall"));
                                        }
                                        else
                                            row.Cells.Add(new WorksheetCell(strStatus, "HeaderStyleSmall"));
                                    }
                                }
                                else
                                {
                                    if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        if ((i + 1) == day)
                                        {
                                        }
                                        else
                                        {
                                            row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                                        }
                                    }
                                }
                            }

                            if (intAbsentDaysCount == 0)
                            {
                                row.Cells.Add(new WorksheetCell(intPresentDaysCount.ToString() + "/" + intPresentDaysCount.ToString(), "Default12"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(intPresentDaysCount.ToString() + "/" + intOpendayscount.ToString(), "Default12"));
                            }
                            if (intOpendayscount > 0 && intPresentDaysCount > 0)
                            {
                                if (intAbsentDaysCount == 0)
                                {
                                    double decpresentper = ((Convert.ToDouble(intPresentDaysCount) / Convert.ToDouble(intPresentDaysCount)) * 100);
                                    row.Cells.Add(new WorksheetCell(Math.Round(decpresentper).ToString() + "%", "Default12"));
                                }
                                else
                                {
                                    double decpresentper = ((Convert.ToDouble(intPresentDaysCount) / Convert.ToDouble(intOpendayscount)) * 100);
                                    row.Cells.Add(new WorksheetCell(Math.Round(decpresentper).ToString() + "%", "Default12"));
                                }
                            }
                            else
                                row.Cells.Add(new WorksheetCell("0%", "Default12"));
                            if (enrldataitem.Status == "E")
                            {
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(enrldataitem.Enrl_Date), "Default12"));
                                if (enrldataitem.Withdraw_Date != string.Empty)
                                    row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(enrldataitem.Withdraw_Date), "Default12"));
                                else
                                    row.Cells.Add(new WorksheetCell("", "Default12"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(enrldataitem.Status_Date), "Default12"));
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(enrldataitem.Withdraw_Date), "Default12"));
                            }
                        }
                    }

                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("# Present", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));

                    int intTotalPresentCount = 0;
                    for (int i = inttoday; i < day; ++i)
                    {
                        DateTime d = new DateTime(now.Year, now.Month, i + 1);
                        //Compare date with sunday

                        if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                        {
                            List<ChldAttnEntity> chldattnpresent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "P");
                            intTotalPresentCount = intTotalPresentCount + chldattnpresent.Count;
                            row.Cells.Add(new WorksheetCell(chldattnpresent.Count.ToString(), "HeaderStyleSmall"));
                        }
                        else
                        {
                            if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if ((i + 1) == day)
                                {
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                                }
                            }
                        }

                    }
                    row.Cells.Add(new WorksheetCell(intTotalPresentCount.ToString(), "ResponseStylegreen"));


                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("# Absent", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                    int intTotalAbsentCount = 0;
                    for (int i = inttoday; i < day; ++i)
                    {
                        DateTime d = new DateTime(now.Year, now.Month, i + 1);
                        //Compare date with sunday

                        if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                        {
                            List<ChldAttnEntity> chldattnabsent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "A");
                            intTotalAbsentCount = intTotalAbsentCount + chldattnabsent.Count;
                            row.Cells.Add(new WorksheetCell(chldattnabsent.Count.ToString(), "HeaderStyleSmall"));
                        }
                        else
                        {
                            if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if ((i + 1) == day)
                                {
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                                }
                            }
                        }

                    }
                    row.Cells.Add(new WorksheetCell(intTotalAbsentCount.ToString(), "ResponseStylegreen"));


                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("% Present", "Default12"));
                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));

                    double decapplicanpercent = 0;
                    for (int i = inttoday; i < day; ++i)
                    {
                        DateTime d = new DateTime(now.Year, now.Month, i + 1);
                        //Compare date with sunday
                        decapplicanpercent = 0;
                        if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                        {
                            List<ChldAttnEntity> chldattnpresent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "P");
                            List<ChldAttnEntity> chldattnabsent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "A");
                            if (chldattnpresent.Count > 0 && intstudent > 0)
                                decapplicanpercent = (Convert.ToDouble(chldattnpresent.Count) / Convert.ToDouble(chldattnpresent.Count + chldattnabsent.Count)) * 100;
                            row.Cells.Add(new WorksheetCell(Math.Round(decapplicanpercent).ToString() + "%", "ResponseStylegreen"));
                        }
                        else
                        {
                            if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if ((i + 1) == day)
                                {
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "ResponseStyle"));
                                }
                            }
                        }

                    }
                    row.Cells.Add(new WorksheetCell((intTotalPresentCount + intTotalAbsentCount).ToString(), "ResponseStylegreen"));

                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();


                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    cell = row.Cells.Add("items", DataType.String, "HeaderStyleSmall");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("This Month", DataType.String, "HeaderStyleSmall");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("items", DataType.String, "HeaderStyleSmall");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("This Month", DataType.String, "HeaderStyleSmall");
                    cell.MergeAcross = 4;

                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    cell = row.Cells.Add("1. # of Class Days ", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add(intOpendayscount.ToString(), DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("4. # Present ", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add(intTotalPresentCount.ToString(), DataType.String, "ResponseStylegreen");
                    cell.MergeAcross = 4;

                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    cell = row.Cells.Add("2. # Enrolled	", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("5.  # Absent", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add(intTotalAbsentCount.ToString(), DataType.String, "ResponseStylegreen");
                    cell.MergeAcross = 4;

                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    cell = row.Cells.Add("3. # Dropped	", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("6. ADA (#4/#1)", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    if (intOpendayscount > 0)
                        cell = row.Cells.Add((Math.Round(Convert.ToDouble(intTotalPresentCount) / Convert.ToDouble(intOpendayscount))).ToString(), DataType.String, "ResponseStylegreen");
                    else
                        cell = row.Cells.Add("", DataType.String, "ResponseStylegreen");
                    cell.MergeAcross = 4;

                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    row.Cells.Add(new WorksheetCell("", "Default"));
                    cell = row.Cells.Add("", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("", DataType.String, "Default");
                    cell.MergeAcross = 4;
                    cell = row.Cells.Add("7. Classroom	", DataType.String, "Default12");
                    cell.MergeAcross = 4;
                    if ((intTotalPresentCount + intTotalAbsentCount) > 0)
                        cell = row.Cells.Add((Math.Round((Convert.ToDouble(intTotalPresentCount) / Convert.ToDouble((intTotalPresentCount + intTotalAbsentCount))) * 100).ToString()) + " %", DataType.String, "ResponseStylegreen");
                    else
                        cell = row.Cells.Add("", DataType.String, "ResponseStylegreen");
                    cell.MergeAcross = 4;

                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

            }
            catch (Exception ex)
            {

            }
        }

        #endregion
        private string GetDateStatus(DateTime dt, string Site, string Room, string Ampm, string Fund, string Dw)
        {
            int Day = dt.Day;
            int Month = dt.Month;
            string CalYear = dt.Year.ToString();
            string strStatus = string.Empty;
            ChildATTMSEntity childattmsitefundstatus = null;
            childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.Attm_Fund == Fund && u.AttM_Site == Site && u.AttM_Room == Room && u.AttM_AMPM == Ampm);
            if (childattmsitefundstatus == null)
            {
                childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.Attm_Fund == Fund && u.AttM_Site == Site && u.AttM_Room.Trim() == "");
                if (childattmsitefundstatus == null)
                {
                    childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.AttM_Site == Site && u.AttM_Room == Room && u.AttM_AMPM == Ampm);
                    if (childattmsitefundstatus == null)
                    {
                        childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.AttM_Site == Site && u.AttM_Room.Trim() == "");
                        if (childattmsitefundstatus == null)
                        {
                            childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.Attm_Fund == Fund && u.AttM_Site.Trim() == "");
                            if (childattmsitefundstatus == null)
                            {
                                childattmsitefundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.Attm_Fund.Trim() == "" && u.AttM_Site.Trim() == "");
                                if (childattmsitefundstatus == null)
                                {

                                }
                                else
                                {
                                    goto fundreport;
                                }
                            }
                            else
                            {
                                goto fundreport;
                            }
                        }
                        else
                        {
                            goto fundreport;
                        }
                    }
                    else
                    {
                        goto fundreport;
                    }
                }
                else
                {
                    goto fundreport;
                }

            }
            else
            {
                goto fundreport;
            }


            fundreport:
            if (childattmsitefundstatus != null)
            {
                if (childattmsitefundstatus.ATTMS_STATUS == "O")
                {
                    strStatus = childattmsitefundstatus.ATTMS_STATUS;

                }
                else
                    strStatus = childattmsitefundstatus.ATTMS_STATUS;
            }
            return strStatus;

        }

        private void HSSB2111ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
        }

        private void rdoFSourceAll_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdoSelected, null);
            SelFundingList.Clear();
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2111");
        }

        private void dtTodate_ValueChanged(object sender, EventArgs e)
        {
            if (dtForm.Checked == true && dtTodate.Checked == true)
            {
                int diffMonth = Math.Abs((dtForm.Value.Year - dtTodate.Value.Year) * 12 + dtForm.Value.Month - dtTodate.Value.Month);
                if (diffMonth > 12)
                {
                   AlertBox.Show(diffMonth.ToString(), MessageBoxIcon.Warning);
                }
            }
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllSite.Checked)
            {
                _errorProvider.SetError(rdoMultipleSites, null);
                Sel_REFS_List.Clear();
            }
        }

        #region Vikash added on 08/05/2024 for converting into DevExpress Format

        private void On_SaveFormClosed_DevExpress(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {

                string strsiteRoomNames = string.Empty;
                string strOnlySites = string.Empty;
                if (rdoMultipleSites.Checked == true)
                {
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty))
                        {
                            strsiteRoomNames += ",";
                            strOnlySites += ",";
                        }
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                        strOnlySites += siteRoom.SiteNUMBER;
                    }
                }
                else
                {
                    strsiteRoomNames = "A";
                    CaseSiteEntity searchEntity = new CaseSiteEntity(true);
                    searchEntity.SiteAGENCY = Agency;
                    searchEntity.SiteDEPT = Dept;
                    searchEntity.SitePROG = Prog;
                    searchEntity.SiteYEAR = Program_Year;
                    List<CaseSiteEntity> AllSites = _model.CaseMstData.Browse_CASESITE(searchEntity, "Browse");
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
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

                        if (hierarchyEntity != null)
                        {
                            if (hierarchyEntity.Sites.Length > 0)
                            {
                                string[] Sites = hierarchyEntity.Sites.Split(',');

                                for (int i = 0; i < Sites.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                    {
                                        foreach (CaseSiteEntity casesite in AllSites)
                                        {
                                            if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                            {
                                                if (!strsiteRoomNames.Equals(string.Empty))
                                                {
                                                    strsiteRoomNames += ",";
                                                    strOnlySites += ",";
                                                }
                                                strsiteRoomNames += casesite.SiteNUMBER + casesite.SiteROOM + casesite.SiteAM_PM;
                                                strOnlySites += casesite.SiteNUMBER;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        strsiteRoomNames = "A";
                }

                StringBuilder strMstApplUpdate = new StringBuilder();
                string PdfName = "Pdf File";
                string strExcelFileName = "ExcelFile";
                PdfName = form.GetFileName();
                strExcelFileName = form.GetFileName();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;

                if (chkDetailReport.Checked)
                {
                    //ExcelreportData(strExcelFileName);
                    ExcelreportData_DevExpress(strExcelFileName);
                }

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

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_calibri, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_calibri, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_calibri, 9, 4);

                iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 0, BaseColor.WHITE);
                iTextSharp.text.Font FooterTextfnt = new iTextSharp.text.Font(bf_calibri, 8, 0, BaseColor.WHITE);
                iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(26, 71, 119));

                #region Cell Color Define

                BaseColor DarkBlue = new BaseColor(26, 71, 119);
                BaseColor SecondBlue = new BaseColor(184, 217, 255);
                BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);

                #endregion

                try
                {
                    using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                    {
                        DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                        oDevExpress_Excel_Properties.sxlbook = wb;
                        oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                        oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                        oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                        PrintHeaderPage_New_Format(document, writer);

                        List<CaseSiteEntity> caseSiteAllSiteList = propCaseAllSiteEntity;
                        if (cmbSiteforHome.Items.Count > 0)
                        {
                            if (((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Value.ToString() != "0")
                            {
                                caseSiteAllSiteList = propCaseAllSiteEntity.FindAll(u => u.SiteNUMBER != ((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Value.ToString());
                            }
                        }

                        if (rdoDetailBoth.Checked == true || rdoDetail.Checked == true)
                        {

                            document.NewPage();
                            Y_Pos = 795;

                            PdfPTable hssb2111_Table = new PdfPTable(12);
                            hssb2111_Table.TotalWidth = 550f;
                            hssb2111_Table.WidthPercentage = 100;
                            hssb2111_Table.LockedWidth = true;
                            float[] widths = new float[] { 2f, 35f, 25f, 25f, 25f, 25f, 25f, 25f, 25f, 20f, 20f, 2f };
                            hssb2111_Table.SetWidths(widths);
                            hssb2111_Table.HorizontalAlignment = Element.ALIGN_CENTER;

                            List<CaseEnrlEntity> chldAttnDetails = _model.EnrollData.Get2111Details(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strsiteRoomNames, "HSSB2111DETAILS");

                            int intFundEnrollmentTot = 0;
                            int intEndEnrollmentTot = 0;
                            int intClassDaysTot = 0;
                            int intAvailbleDaysTot = 0;
                            int intExcusedDaysTot = 0;
                            int intAttandanceDaysTot = 0;
                            int intAveAttendanceTot = 0;
                            decimal intAveAttendanceTot2 = 0;
                            int intTotalRows = 0;

                            int intprogFundEnrollmentTot = 0;
                            int intprogEndEnrollmentTot = 0;
                            int intprogClassDaysTot = 0;
                            int intprogAvailbleDaysTot = 0;
                            int intprogExcusedDaysTot = 0;
                            int intprogAttandanceDaysTot = 0;
                            int intprogAveAttendanceTot = 0;
                            decimal intprogAveAttendanceTot2 = 0;
                            int intprogTotalRows = 0;
                            bool boolprogprint = false;

                            foreach (CaseSiteEntity siterow in caseSiteAllSiteList)
                            {
                                intFundEnrollmentTot = 0;
                                intEndEnrollmentTot = 0;
                                intClassDaysTot = 0;
                                intAvailbleDaysTot = 0;
                                intExcusedDaysTot = 0;
                                intAttandanceDaysTot = 0;
                                intAveAttendanceTot = 0;
                                intAveAttendanceTot2 = 0;
                                intTotalRows = 0;

                                List<CaseEnrlEntity> caseEnrlDetails = chldAttnDetails.FindAll(u => u.Site.Trim().ToString() == siterow.SiteNUMBER.Trim().ToString());

                                if (caseEnrlDetails.Count > 0)
                                {
                                    boolprogprint = true;

                                    #region Header of Each page

                                    PdfPCell Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.FixedHeight = 20f;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Colspan = 10;
                                    Cell.FixedHeight = 20f;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.FixedHeight = 20f;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    #endregion

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    PdfPCell Header_1 = new PdfPCell(new Phrase("Class Period From " + dtForm.Text.Trim() + "  To " + dtTodate.Text.Trim(), TableFont));
                                    Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Header_1.Colspan = 4;
                                    Header_1.BackgroundColor = TblHeaderBlue;
                                    Header_1.Border = 0;
                                    Header_1.BorderWidthBottom = 0.5f;
                                    Header_1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(Header_1);

                                    PdfPCell Header = new PdfPCell(new Phrase("Site:" + siterow.SiteNUMBER + " " + siterow.SiteNAME, TableFont));
                                    Header.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Header.Colspan = 2;
                                    Header.BackgroundColor = TblHeaderBlue;
                                    Header.Border = 0;
                                    Header.BorderWidthBottom = 0.5f;
                                    Header.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(Header);

                                    PdfPCell HeaderTop1 = new PdfPCell(new Phrase(siterow.SiteCOMMENT, TableFont));
                                    HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    HeaderTop1.Colspan = 4;
                                    HeaderTop1.BackgroundColor = TblHeaderBlue;
                                    HeaderTop1.Border = 0;
                                    HeaderTop1.BorderWidthBottom = 0.5f;
                                    HeaderTop1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(HeaderTop1);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    PdfPCell Header1 = new PdfPCell(new Phrase("Class", TblFontBold));
                                    Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Header1.BackgroundColor = SecondBlue;
                                    Header1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header1);

                                    PdfPCell Header2 = new PdfPCell(new Phrase("Funding Source", TblFontBold));
                                    Header2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header2.BackgroundColor = SecondBlue;
                                    Header2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header2);

                                    PdfPCell Header3 = new PdfPCell(new Phrase("Funded Enrollment", TblFontBold));
                                    Header3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header3.BackgroundColor = SecondBlue;
                                    Header3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header3);

                                    PdfPCell Header4 = new PdfPCell(new Phrase("End Period Enrollment", TblFontBold));
                                    Header4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header4.BackgroundColor = SecondBlue;
                                    Header4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header4);

                                    PdfPCell Header5 = new PdfPCell(new Phrase("Class Days", TblFontBold));
                                    Header5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header5.BackgroundColor = SecondBlue;
                                    Header5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header5);

                                    PdfPCell Header6 = new PdfPCell(new Phrase("Attendance Days", TblFontBold));
                                    Header6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header6.BackgroundColor = SecondBlue;
                                    Header6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header6);

                                    PdfPCell Header7 = new PdfPCell(new Phrase("Excused Days", TblFontBold));
                                    Header7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header7.BackgroundColor = SecondBlue;
                                    Header7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header7);

                                    PdfPCell Header8 = new PdfPCell(new Phrase("Available Days", TblFontBold));
                                    Header8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header8.BackgroundColor = SecondBlue;
                                    Header8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header8);

                                    PdfPCell Header9 = new PdfPCell(new Phrase("Ave Daily Attendance", TblFontBold));
                                    Header9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Header9.BackgroundColor = SecondBlue;
                                    Header9.Colspan = 2;
                                    Header9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    hssb2111_Table.AddCell(Header9);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    bool boolFundswtch = true;

                                    foreach (CaseEnrlEntity item in caseEnrlDetails)
                                    {
                                        if (rdoSelected.Checked)
                                        {
                                            boolFundswtch = false;
                                            if (SelFundingList.Count > 0)
                                            {
                                                if (SelFundingList.FindAll(u => u.Code.Trim() == item.FundHie.Trim()).Count > 0)
                                                    boolFundswtch = true;
                                            }

                                        }
                                        if (boolFundswtch)
                                        {
                                            intTotalRows = intTotalRows + 1;
                                            decimal intAvg = 0;
                                            if (Convert.ToInt32(item.ClassDays) > 0)
                                                intAvg = (Convert.ToDecimal(item.AttandanceDays) / Convert.ToDecimal(item.ClassDays));

                                            decimal intAvgPer = 0;
                                            if (Convert.ToInt32(item.AvailbleDays) > 0)
                                                intAvgPer = (Convert.ToDecimal(item.AttandanceDays) * 100) / Convert.ToDecimal(item.AvailbleDays);
                                            intFundEnrollmentTot = intFundEnrollmentTot + Convert.ToInt32(item.FundEnrollment == string.Empty ? "0" : item.FundEnrollment);
                                            intEndEnrollmentTot = intEndEnrollmentTot + Convert.ToInt32(item.EndEnrollment == string.Empty ? "0" : item.EndEnrollment);
                                            intClassDaysTot = intClassDaysTot + Convert.ToInt32(item.ClassDays);
                                            intAvailbleDaysTot = intAvailbleDaysTot + Convert.ToInt32(item.AvailbleDays);
                                            intExcusedDaysTot = intExcusedDaysTot + Convert.ToInt32(item.ExcusedDays);
                                            intAttandanceDaysTot = intAttandanceDaysTot + Convert.ToInt32(item.AttandanceDays);
                                            intAveAttendanceTot = intAveAttendanceTot + Convert.ToInt32(Math.Round(intAvg));
                                            intAveAttendanceTot2 = intAveAttendanceTot2 + intAvgPer;

                                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Cell.BackgroundColor = DarkBlue;
                                            hssb2111_Table.AddCell(Cell);

                                            PdfPCell pdfData1 = new PdfPCell(new Phrase(item.Site + " " + item.Room + " " + item.AMPM, TableFont));
                                            pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfData1.Border = 0;
                                            pdfData1.BorderWidthBottom = 0.6f;
                                            pdfData1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData1);

                                            PdfPCell pdfData2 = new PdfPCell(new Phrase(item.FundHie, TableFont));
                                            pdfData2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData2.Border = 0;
                                            pdfData2.BorderWidthLeft = 0.5f;
                                            pdfData2.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData2.BorderWidthBottom = 0.6f;
                                            pdfData2.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData2);

                                            PdfPCell pdfData3 = new PdfPCell(new Phrase(item.FundEnrollment, TableFont));
                                            pdfData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData3.Border = 0;
                                            pdfData3.BorderWidthLeft = 0.5f;
                                            pdfData3.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData3.BorderWidthBottom = 0.6f;
                                            pdfData3.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData3);


                                            PdfPCell pdfData4 = new PdfPCell(new Phrase(item.EndEnrollment, TableFont));
                                            pdfData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData4.Border = 0;
                                            pdfData4.BorderWidthLeft = 0.5f;
                                            pdfData4.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData4.BorderWidthBottom = 0.6f;
                                            pdfData4.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData4);

                                            PdfPCell pdfData5 = new PdfPCell(new Phrase(item.ClassDays, TableFont));
                                            pdfData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData5.Border = 0;
                                            pdfData5.BorderWidthLeft = 0.5f;
                                            pdfData5.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData5.BorderWidthBottom = 0.6f;
                                            pdfData5.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData5);

                                            PdfPCell pdfData6 = new PdfPCell(new Phrase(item.AttandanceDays, TableFont));
                                            pdfData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData6.Border = 0;
                                            pdfData6.BorderWidthLeft = 0.5f;
                                            pdfData6.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData6.BorderWidthBottom = 0.6f;
                                            pdfData6.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData6);

                                            PdfPCell pdfData7 = new PdfPCell(new Phrase(item.ExcusedDays, TableFont));
                                            pdfData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData7.Border = 0;
                                            pdfData7.BorderWidthLeft = 0.5f;
                                            pdfData7.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData7.BorderWidthBottom = 0.6f;
                                            pdfData7.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData7);


                                            PdfPCell pdfData8 = new PdfPCell(new Phrase(item.AvailbleDays, TableFont));
                                            pdfData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData8.Border = 0;
                                            pdfData8.BorderWidthLeft = 0.5f;
                                            pdfData8.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData8.BorderWidthBottom = 0.6f;
                                            pdfData8.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData8);

                                            string i = Math.Round(intAvg).ToString();

                                            PdfPCell pdfData9 = new PdfPCell(new Phrase(i, TableFont));
                                            pdfData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData9.Border = 0;
                                            pdfData9.BorderWidthLeft = 0.5f;
                                            pdfData9.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData9.BorderWidthBottom = 0.6f;
                                            pdfData9.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData9);

                                            PdfPCell pdfData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", intAvgPer) + "%", TableFont));
                                            pdfData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfData10.Border = 0;
                                            pdfData10.BorderWidthBottom = 0.6f;
                                            pdfData10.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            pdfData10.BorderWidthLeft = 0.5f;
                                            pdfData10.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                            hssb2111_Table.AddCell(pdfData10);

                                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Cell.BackgroundColor = DarkBlue;
                                            hssb2111_Table.AddCell(Cell);
                                        }
                                    }

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    PdfPCell pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.Colspan = 2;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlspace1.FixedHeight = 5f;
                                    pdfttlspace1.Border = 0;
                                    pdfttlspace1.BorderWidthLeft = 0.5f;
                                    pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    pdfttlspace1.BorderWidthTop = 0.5f;
                                    pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                    hssb2111_Table.AddCell(pdfttlspace1);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    PdfPCell pdfttlData1 = new PdfPCell(new Phrase("Total Site " + siterow.SiteNAME, TableFont));
                                    pdfttlData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlData1.Colspan = 2;
                                    pdfttlData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_Table.AddCell(pdfttlData1);

                                    PdfPCell pdfttlData3 = new PdfPCell(new Phrase(intFundEnrollmentTot.ToString(), TableFont));
                                    pdfttlData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData3.Border = 0;
                                    pdfttlData3.BorderWidthLeft = 0.5f;
                                    pdfttlData3.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData3);

                                    PdfPCell pdfttlData4 = new PdfPCell(new Phrase(intEndEnrollmentTot.ToString(), TableFont));
                                    pdfttlData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData4.Border = 0;
                                    pdfttlData4.BorderWidthLeft = 0.5f;
                                    pdfttlData4.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData4);

                                    PdfPCell pdfttlData5 = new PdfPCell(new Phrase(intClassDaysTot.ToString(), TableFont));
                                    pdfttlData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData5.Border = 0;
                                    pdfttlData5.BorderWidthLeft = 0.5f;
                                    pdfttlData5.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData5);

                                    PdfPCell pdfttlData6 = new PdfPCell(new Phrase(intAttandanceDaysTot.ToString(), TableFont));
                                    pdfttlData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData6.Border = 0;
                                    pdfttlData6.BorderWidthLeft = 0.5f;
                                    pdfttlData6.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData6);

                                    PdfPCell pdfttlData7 = new PdfPCell(new Phrase(intExcusedDaysTot.ToString(), TableFont));
                                    pdfttlData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData7.Border = 0;
                                    pdfttlData7.BorderWidthLeft = 0.5f;
                                    pdfttlData7.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData7);

                                    PdfPCell pdfttlData8 = new PdfPCell(new Phrase(intAvailbleDaysTot.ToString(), TableFont));
                                    pdfttlData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData8.Border = 0;
                                    pdfttlData8.BorderWidthLeft = 0.5f;
                                    pdfttlData8.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData8);

                                    PdfPCell pdfttlData9 = new PdfPCell(new Phrase(intAveAttendanceTot.ToString(), TableFont));
                                    pdfttlData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData9.Border = 0;
                                    pdfttlData9.BorderWidthLeft = 0.5f;
                                    pdfttlData9.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_Table.AddCell(pdfttlData9);

                                    if (intTotalRows > 0)
                                    {
                                        PdfPCell pdfttlData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", (intAveAttendanceTot2 / intTotalRows)) + "%", TableFont));
                                        pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfttlData10.Border = 0;
                                        pdfttlData10.BorderWidthLeft = 0.5f;
                                        pdfttlData10.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                        hssb2111_Table.AddCell(pdfttlData10);
                                    }
                                    else
                                    {
                                        PdfPCell pdfttlData10 = new PdfPCell(new Phrase("", TableFont));
                                        pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfttlData10.Border = 0;
                                        pdfttlData10.BorderWidthLeft = 0.5f;
                                        pdfttlData10.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                        hssb2111_Table.AddCell(pdfttlData10);
                                    }

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cell.FixedHeight = 12f;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("©2024 CAPSystems Inc", FooterTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cell.Colspan = 10;
                                    Cell.FixedHeight = 12f;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cell.FixedHeight = 12f;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_Table.AddCell(Cell);

                                    intprogFundEnrollmentTot = intprogFundEnrollmentTot + intFundEnrollmentTot;
                                    intprogEndEnrollmentTot = intprogEndEnrollmentTot + intEndEnrollmentTot;
                                    intprogClassDaysTot = intprogClassDaysTot + intClassDaysTot;
                                    intprogAvailbleDaysTot = intprogAvailbleDaysTot + intAvailbleDaysTot;
                                    intprogExcusedDaysTot = intprogExcusedDaysTot + intExcusedDaysTot;
                                    intprogAttandanceDaysTot = intprogAttandanceDaysTot + intAttandanceDaysTot;
                                    intprogAveAttendanceTot = intprogAveAttendanceTot + intAveAttendanceTot;

                                    if (hssb2111_Table.Rows.Count > 0)
                                    {
                                        document.Add(hssb2111_Table);
                                        hssb2111_Table.DeleteBodyRows();
                                        document.NewPage();
                                    }
                                }
                            }

                            if (boolprogprint)
                            {

                                decimal intprogAvg = 0;
                                if (Convert.ToInt32(intprogClassDaysTot) > 0)
                                    intprogAvg = (Convert.ToDecimal(intprogAttandanceDaysTot) / Convert.ToDecimal(intprogClassDaysTot));

                                decimal intprogAvgPer = 0;
                                if (Convert.ToInt32(intprogAvailbleDaysTot) > 0)
                                    intprogAvgPer = (Convert.ToDecimal(intprogAttandanceDaysTot) * 100) / Convert.ToDecimal(intprogAvailbleDaysTot);

                                #region Header of Each page

                                PdfPCell Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.FixedHeight = 20f;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Colspan = 10;
                                Cell.FixedHeight = 20f;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.FixedHeight = 20f;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                #endregion

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                PdfPCell Header_1 = new PdfPCell(new Phrase("Class Period From " + dtForm.Text.Trim() + "  To " + dtTodate.Text.Trim(), TableFont));
                                Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header_1.Colspan = 4;
                                Header_1.BackgroundColor = TblHeaderBlue;
                                Header_1.Border = 0;
                                Header_1.BorderWidthBottom = 0.5f;
                                Header_1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(Header_1);

                                PdfPCell Header = new PdfPCell(new Phrase("Program wide totals", TableFont));
                                Header.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header.Colspan = 2;
                                Header.BackgroundColor = TblHeaderBlue;
                                Header.Border = 0;
                                Header.BorderWidthBottom = 0.5f;
                                Header.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(Header);

                                PdfPCell HeaderTop1 = new PdfPCell(new Phrase("", TableFont));
                                HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                                HeaderTop1.Colspan = 4;
                                HeaderTop1.BackgroundColor = TblHeaderBlue;
                                HeaderTop1.Border = 0;
                                HeaderTop1.BorderWidthBottom = 0.5f;
                                HeaderTop1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(HeaderTop1);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                PdfPCell Header1 = new PdfPCell(new Phrase(strProgramName, TblFontBold));
                                Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header1.BackgroundColor = SecondBlue;
                                Header1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header1);

                                PdfPCell Header2 = new PdfPCell(new Phrase("", TblFontBold));
                                Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header2.BackgroundColor = SecondBlue;
                                Header2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header2);

                                PdfPCell Header3 = new PdfPCell(new Phrase("Funded Enrollment", TblFontBold));
                                Header3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header3.BackgroundColor = SecondBlue;
                                Header3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header3);

                                PdfPCell Header4 = new PdfPCell(new Phrase("End Period Enrollment", TblFontBold));
                                Header4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header4.BackgroundColor = SecondBlue;
                                Header4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header4);

                                PdfPCell Header5 = new PdfPCell(new Phrase("Class Days", TblFontBold));
                                Header5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header5.BackgroundColor = SecondBlue;
                                Header5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header5);

                                PdfPCell Header6 = new PdfPCell(new Phrase("Attendance Days", TblFontBold));
                                Header6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header6.BackgroundColor = SecondBlue;
                                Header6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header6);

                                PdfPCell Header7 = new PdfPCell(new Phrase("Excused Days", TblFontBold));
                                Header7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header7.BackgroundColor = SecondBlue;
                                Header7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header7);

                                PdfPCell Header8 = new PdfPCell(new Phrase("Available Days", TblFontBold));
                                Header8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header8.BackgroundColor = SecondBlue;
                                Header8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header8);

                                PdfPCell Header9 = new PdfPCell(new Phrase("Ave Daily Attendance", TblFontBold));
                                Header9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Header9.Colspan = 2;
                                Header9.BackgroundColor = SecondBlue;
                                Header9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2111_Table.AddCell(Header9);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                PdfPCell pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.Colspan = 2;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                                pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlspace1.FixedHeight = 5f;
                                pdfttlspace1.Border = 0;
                                pdfttlspace1.BorderWidthLeft = 0.5f;
                                pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                pdfttlspace1.BorderWidthTop = 0.5f;
                                pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                hssb2111_Table.AddCell(pdfttlspace1);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                PdfPCell pdfttlData1 = new PdfPCell(new Phrase("Program wide totals", TableFont));
                                pdfttlData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfttlData1.Colspan = 2;
                                pdfttlData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2111_Table.AddCell(pdfttlData1);

                                PdfPCell pdfttlData3 = new PdfPCell(new Phrase(intprogFundEnrollmentTot.ToString(), TableFont));
                                pdfttlData3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData3.Border = 0;
                                pdfttlData3.BorderWidthLeft = 0.5f;
                                pdfttlData3.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData3);

                                PdfPCell pdfttlData4 = new PdfPCell(new Phrase(intprogEndEnrollmentTot.ToString(), TableFont));
                                pdfttlData4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData4.Border = 0;
                                pdfttlData4.BorderWidthLeft = 0.5f;
                                pdfttlData4.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData4);

                                PdfPCell pdfttlData5 = new PdfPCell(new Phrase(intprogClassDaysTot.ToString(), TableFont));
                                pdfttlData5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData5.Border = 0;
                                pdfttlData5.BorderWidthLeft = 0.5f;
                                pdfttlData5.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData5);

                                PdfPCell pdfttlData6 = new PdfPCell(new Phrase(intprogAttandanceDaysTot.ToString(), TableFont));
                                pdfttlData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData6.Border = 0;
                                pdfttlData6.BorderWidthLeft = 0.5f;
                                pdfttlData6.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData6);

                                PdfPCell pdfttlData7 = new PdfPCell(new Phrase(intprogExcusedDaysTot.ToString(), TableFont));
                                pdfttlData7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData7.Border = 0;
                                pdfttlData7.BorderWidthLeft = 0.5f;
                                pdfttlData7.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData7);

                                PdfPCell pdfttlData8 = new PdfPCell(new Phrase(intprogAvailbleDaysTot.ToString(), TableFont));
                                pdfttlData8.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData8.Border = 0;
                                pdfttlData8.BorderWidthLeft = 0.5f;
                                pdfttlData8.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData8);

                                string i = Math.Round(intprogAvg).ToString();

                                PdfPCell pdfttlData9 = new PdfPCell(new Phrase(i, TableFont));
                                pdfttlData9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData9.Border = 0;
                                pdfttlData9.BorderWidthLeft = 0.5f;
                                pdfttlData9.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData9);

                                PdfPCell pdfttlData10 = new PdfPCell(new Phrase(String.Format("{0:0.0}", (intprogAvgPer)) + "%", TableFont));
                                pdfttlData10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                pdfttlData10.Border = 0;
                                pdfttlData10.BorderWidthLeft = 0.5f;
                                pdfttlData10.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                hssb2111_Table.AddCell(pdfttlData10);

                                Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell.FixedHeight = 12f;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("©2024 CAPSystems Inc", FooterTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell.Colspan = 10;
                                Cell.FixedHeight = 12f;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                                Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Cell.FixedHeight = 12f;
                                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Cell.BackgroundColor = DarkBlue;
                                hssb2111_Table.AddCell(Cell);

                                if (hssb2111_Table.Rows.Count > 0)
                                {
                                    document.Add(hssb2111_Table);
                                    hssb2111_Table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }
                        }

                        // ******** Summary Report *************//

                        if (rdoDetailBoth.Checked == true || rdoSummaryO.Checked == true)
                        {

                            string SelFunds = "A";
                            if (rdoSelected.Checked)
                            {
                                if (SelFundingList.Count > 0)
                                {
                                    SelFunds = string.Empty;
                                    foreach (CommonEntity Entity in SelFundingList)
                                    {
                                        if (!SelFunds.Equals(string.Empty))
                                        {
                                            SelFunds += ",";
                                        }
                                        SelFunds += Entity.Code;
                                    }
                                }
                            }

                            List<CaseEnrlEntity> chldAttnSummary = _model.EnrollData.Get2111SummaryDetails(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strOnlySites, "HSSB2111SUMMARY", SelFunds);

                            int intAvgTot = 0;
                            int intFreeTot = 0;
                            int intReducedTot = 0;
                            int intOverIncomeTot = 0;
                            int intATot = 0;
                            int intBreakfastTot = 0;
                            int intLunchTot = 0;
                            int intSupperTot = 0;
                            int intSupplementTot = 0;
                            int intBAllTot = 0;
                            decimal intAverageCalcu = 0;

                            if (rdoSummaryO.Checked == true)
                                document.NewPage();

                            PdfPTable hssb2111_SummaryTable = new PdfPTable(13);
                            hssb2111_SummaryTable.TotalWidth = 550f;
                            hssb2111_SummaryTable.WidthPercentage = 100;
                            hssb2111_SummaryTable.LockedWidth = true;
                            float[] summarywidths = new float[] { 2f, 50f, 25f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 25f, 20f, 2f };
                            hssb2111_SummaryTable.SetWidths(summarywidths);
                            hssb2111_SummaryTable.HorizontalAlignment = Element.ALIGN_CENTER;

                            #region Header of Each page

                            PdfPCell Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.FixedHeight = 20f;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase(Privileges.PrivilegeName.Trim(), HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Colspan = 11;
                            Cell.FixedHeight = 20f;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.FixedHeight = 20f;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            #endregion

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            PdfPCell SumHeader = new PdfPCell(new Phrase("", TableFont));
                            SumHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumHeader.Colspan = 2;
                            SumHeader.BackgroundColor = TblHeaderBlue;
                            SumHeader.Border = 0;
                            SumHeader.BorderWidthBottom = 0.5f;
                            SumHeader.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(SumHeader);

                            PdfPCell SumHeader1 = new PdfPCell(new Phrase("No. of Participants Enrolled by Category", TableFont));
                            SumHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumHeader1.Colspan = 4;
                            SumHeader1.BackgroundColor = TblHeaderBlue;
                            SumHeader1.Border = 0;
                            SumHeader1.BorderWidthBottom = 0.5f;
                            SumHeader1.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(SumHeader1);


                            PdfPCell SumHeader2 = new PdfPCell(new Phrase("No. of Meals Served to Enrolled Participants", TableFont));
                            SumHeader2.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumHeader2.Colspan = 5;
                            SumHeader2.BackgroundColor = TblHeaderBlue;
                            SumHeader2.Border = 0;
                            SumHeader2.BorderWidthBottom = 0.5f;
                            SumHeader2.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(SumHeader2);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            PdfPCell SumSiteHeader = new PdfPCell(new Phrase("Site", TblFontBold));
                            SumSiteHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumSiteHeader.BackgroundColor = SecondBlue;
                            SumSiteHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSiteHeader);

                            PdfPCell SumDailyAttHeader = new PdfPCell(new Phrase("Ave Daily Attendance", TblFontBold));
                            SumDailyAttHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttHeader.BackgroundColor = SecondBlue;
                            SumDailyAttHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttHeader);

                            PdfPCell SumDailyAttFree = new PdfPCell(new Phrase("Free", TblFontBold));
                            SumDailyAttFree.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttFree.BackgroundColor = SecondBlue;
                            SumDailyAttFree.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttFree);

                            PdfPCell SumReduced = new PdfPCell(new Phrase("Reduced", TblFontBold));
                            SumReduced.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumReduced.BackgroundColor = SecondBlue;
                            SumReduced.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumReduced);

                            PdfPCell SumOverIncome = new PdfPCell(new Phrase("Over Income", TblFontBold));
                            SumOverIncome.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumOverIncome.BackgroundColor = SecondBlue;
                            SumOverIncome.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumOverIncome);

                            PdfPCell SumTotal = new PdfPCell(new Phrase("Total", TblFontBold));
                            SumTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumTotal.BackgroundColor = SecondBlue;
                            SumTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumTotal);

                            PdfPCell SumBreakfast = new PdfPCell(new Phrase("Breakfast", TblFontBold));
                            SumBreakfast.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumBreakfast.BackgroundColor = SecondBlue;
                            SumBreakfast.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumBreakfast);

                            PdfPCell SumLunch = new PdfPCell(new Phrase("Lunch", TblFontBold));
                            SumLunch.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumLunch.BackgroundColor = SecondBlue;
                            SumLunch.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumLunch);

                            PdfPCell SumSupper = new PdfPCell(new Phrase("Supper", TblFontBold));
                            SumSupper.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSupper.BackgroundColor = SecondBlue;
                            SumSupper.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSupper);

                            PdfPCell SumSupleme = new PdfPCell(new Phrase("Supplements", TblFontBold));
                            SumSupleme.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSupleme.BackgroundColor = SecondBlue;
                            SumSupleme.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSupleme);

                            PdfPCell SumTotal2 = new PdfPCell(new Phrase("Total", TblFontBold));
                            SumTotal2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumTotal2.BackgroundColor = SecondBlue;
                            SumTotal2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2111_SummaryTable.AddCell(SumTotal2);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            foreach (CaseEnrlEntity item in chldAttnSummary)
                            {
                                CaseSiteEntity caseSiteDetails = caseSiteAllSiteList.Find(u => u.SiteNUMBER.Trim() == item.Site.Trim());
                                if (caseSiteDetails != null)
                                {
                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_SummaryTable.AddCell(Cell);

                                    intAverageCalcu = 0;
                                    if (Convert.ToInt32(item.ClassDays) > 0)
                                        intAverageCalcu = Convert.ToDecimal(item.AttandanceDays) / Convert.ToDecimal(item.ClassDays);
                                    PdfPCell SumSiteData = new PdfPCell(new Phrase(caseSiteDetails.SiteNAME, TableFont));
                                    SumSiteData.HorizontalAlignment = Element.ALIGN_LEFT;
                                    SumSiteData.Border = 0;
                                    SumSiteData.BorderWidthBottom = 0.6f;
                                    SumSiteData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumSiteData);

                                    PdfPCell SumDailyAttData = new PdfPCell(new Phrase(Math.Round(intAverageCalcu).ToString(), TableFont));
                                    SumDailyAttData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumDailyAttData.Border = 0;
                                    SumDailyAttData.BorderWidthLeft = 0.5f;
                                    SumDailyAttData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumDailyAttData.BorderWidthBottom = 0.6f;
                                    SumDailyAttData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumDailyAttData);

                                    PdfPCell SumDailyAttFreeData = new PdfPCell(new Phrase(item.Free, TableFont));
                                    SumDailyAttFreeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumDailyAttFreeData.Border = 0;
                                    SumDailyAttFreeData.BorderWidthLeft = 0.5f;
                                    SumDailyAttFreeData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumDailyAttFreeData.BorderWidthBottom = 0.6f;
                                    SumDailyAttFreeData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumDailyAttFreeData);

                                    PdfPCell SumReducedData = new PdfPCell(new Phrase(item.Reduced, TableFont));
                                    SumReducedData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumReducedData.Border = 0;
                                    SumReducedData.BorderWidthLeft = 0.5f;
                                    SumReducedData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumReducedData.BorderWidthBottom = 0.6f;
                                    SumReducedData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumReducedData);

                                    PdfPCell SumOverIncomeData = new PdfPCell(new Phrase(item.OverIncome, TableFont));
                                    SumOverIncomeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumOverIncomeData.Border = 0;
                                    SumOverIncomeData.BorderWidthLeft = 0.5f;
                                    SumOverIncomeData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumOverIncomeData.BorderWidthBottom = 0.6f;
                                    SumOverIncomeData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumOverIncomeData);

                                    PdfPCell SumTotalData = new PdfPCell(new Phrase((Convert.ToInt32(item.Free) + Convert.ToInt32(item.Reduced) + Convert.ToInt32(item.OverIncome)).ToString(), TableFont));
                                    SumTotalData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumTotalData.Border = 0;
                                    SumTotalData.BorderWidthLeft = 0.5f;
                                    SumTotalData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumTotalData.BorderWidthBottom = 0.6f;
                                    SumTotalData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumTotalData);

                                    PdfPCell SumBreakfastData = new PdfPCell(new Phrase(item.Breakfast, TableFont));
                                    SumBreakfastData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumBreakfastData.Border = 0;
                                    SumBreakfastData.BorderWidthLeft = 0.5f;
                                    SumBreakfastData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumBreakfastData.BorderWidthBottom = 0.6f;
                                    SumBreakfastData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumBreakfastData);

                                    PdfPCell SumLunchData = new PdfPCell(new Phrase(item.Lunch, TableFont));
                                    SumLunchData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumLunchData.Border = 0;
                                    SumLunchData.BorderWidthLeft = 0.5f;
                                    SumLunchData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumLunchData.BorderWidthBottom = 0.6f;
                                    SumLunchData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumLunchData);

                                    PdfPCell SumSupperData = new PdfPCell(new Phrase(item.Supper, TableFont));
                                    SumSupperData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumSupperData.Border = 0;
                                    SumSupperData.BorderWidthLeft = 0.5f;
                                    SumSupperData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumSupperData.BorderWidthBottom = 0.6f;
                                    SumSupperData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumSupperData);

                                    PdfPCell SumSuplemeData = new PdfPCell(new Phrase(item.Suppliment, TableFont));
                                    SumSuplemeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumSuplemeData.Border = 0;
                                    SumSuplemeData.BorderWidthLeft = 0.5f;
                                    SumSuplemeData.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumSuplemeData.BorderWidthBottom = 0.6f;
                                    SumSuplemeData.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumSuplemeData);

                                    int intBreakfastTotal = Convert.ToInt32(item.Breakfast) + Convert.ToInt32(item.Lunch) + Convert.ToInt32(item.Supper) + Convert.ToInt32(item.Suppliment);
                                    intBreakfastTot = Convert.ToInt32(item.Breakfast) + intBreakfastTot;
                                    intLunchTot = Convert.ToInt32(item.Lunch) + intLunchTot;
                                    intSupperTot = Convert.ToInt32(item.Supper) + intSupperTot;
                                    intSupplementTot = Convert.ToInt32(item.Suppliment) + intSupplementTot;
                                    intAvgTot = Convert.ToInt32(Math.Round(intAverageCalcu)) + intAvgTot;
                                    intFreeTot = Convert.ToInt32(item.Free) + intFreeTot;
                                    intReducedTot = Convert.ToInt32(item.Reduced) + intReducedTot;
                                    intOverIncomeTot = Convert.ToInt32(item.OverIncome) + intOverIncomeTot;
                                    intATot = (Convert.ToInt32(item.Free) + Convert.ToInt32(item.Reduced) + Convert.ToInt32(item.OverIncome)) + intATot;
                                    intBAllTot = intBreakfastTotal + intBAllTot;

                                    PdfPCell SumTotal2Data = new PdfPCell(new Phrase(intBreakfastTotal.ToString(), TableFont));
                                    SumTotal2Data.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumTotal2Data.Border = 0;
                                    SumTotal2Data.BorderWidthLeft = 0.5f;
                                    SumTotal2Data.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    SumTotal2Data.BorderWidthBottom = 0.6f;
                                    SumTotal2Data.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                                    hssb2111_SummaryTable.AddCell(SumTotal2Data);

                                    Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Cell.BackgroundColor = DarkBlue;
                                    hssb2111_SummaryTable.AddCell(Cell);

                                }
                            }

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            PdfPCell pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            pdfttlspace1 = new PdfPCell(new Phrase("", TableFont));
                            pdfttlspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfttlspace1.FixedHeight = 5f;
                            pdfttlspace1.Border = 0;
                            pdfttlspace1.BorderWidthLeft = 0.5f;
                            pdfttlspace1.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            pdfttlspace1.BorderWidthTop = 0.5f;
                            pdfttlspace1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            hssb2111_SummaryTable.AddCell(pdfttlspace1);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            PdfPCell SumSiteTotal = new PdfPCell(new Phrase("Total", TableFont));
                            SumSiteTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumSiteTotal.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSiteTotal);

                            PdfPCell SumDailyAttTot = new PdfPCell(new Phrase(intAvgTot.ToString(), TableFont));
                            SumDailyAttTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttTot.Border = 0;
                            SumDailyAttTot.BorderWidthLeft = 0.5f;
                            SumDailyAttTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumDailyAttTot);

                            PdfPCell SumDailyAttFreeTot = new PdfPCell(new Phrase(intFreeTot.ToString(), TableFont));
                            SumDailyAttFreeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttFreeTot.Border = 0;
                            SumDailyAttFreeTot.BorderWidthLeft = 0.5f;
                            SumDailyAttFreeTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumDailyAttFreeTot);

                            PdfPCell SumReducedTot = new PdfPCell(new Phrase(intReducedTot.ToString(), TableFont));
                            SumReducedTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumReducedTot.Border = 0;
                            SumReducedTot.BorderWidthLeft = 0.5f;
                            SumReducedTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumReducedTot);

                            PdfPCell SumOverIncomeTot = new PdfPCell(new Phrase(intOverIncomeTot.ToString(), TableFont));
                            SumOverIncomeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumOverIncomeTot.Border = 0;
                            SumOverIncomeTot.BorderWidthLeft = 0.5f;
                            SumOverIncomeTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumOverIncomeTot);

                            PdfPCell SumTotalTot = new PdfPCell(new Phrase(intATot.ToString(), TableFont));
                            SumTotalTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumTotalTot.Border = 0;
                            SumTotalTot.BorderWidthLeft = 0.5f;
                            SumTotalTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumTotalTot);

                            PdfPCell SumBreakfastTot = new PdfPCell(new Phrase(intBreakfastTot.ToString(), TableFont));
                            SumBreakfastTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumBreakfastTot.Border = 0;
                            SumBreakfastTot.BorderWidthLeft = 0.5f;
                            SumBreakfastTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumBreakfastTot);

                            PdfPCell SumLunchTot = new PdfPCell(new Phrase(intLunchTot.ToString(), TableFont));
                            SumLunchTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumLunchTot.Border = 0;
                            SumLunchTot.BorderWidthLeft = 0.5f;
                            SumLunchTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumLunchTot);

                            PdfPCell SumSupperTot = new PdfPCell(new Phrase(intSupperTot.ToString(), TableFont));
                            SumSupperTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSupperTot.Border = 0;
                            SumSupperTot.BorderWidthLeft = 0.5f;
                            SumSupperTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumSupperTot);

                            PdfPCell SumSuplemeTot = new PdfPCell(new Phrase(intSupplementTot.ToString(), TableFont));
                            SumSuplemeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSuplemeTot.Border = 0;
                            SumSuplemeTot.BorderWidthLeft = 0.5f;
                            SumSuplemeTot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumSuplemeTot);

                            PdfPCell SumTotal2Tot = new PdfPCell(new Phrase(intBAllTot.ToString(), TableFont));
                            SumTotal2Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumTotal2Tot.Border = 0;
                            SumTotal2Tot.BorderWidthLeft = 0.5f;
                            SumTotal2Tot.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
                            hssb2111_SummaryTable.AddCell(SumTotal2Tot);

                            Cell = new PdfPCell(new Phrase("", HeaderTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell.FixedHeight = 12f;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("©2024 CAPSystems Inc", FooterTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell.Colspan = 11;
                            Cell.FixedHeight = 12f;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            Cell = new PdfPCell(new Phrase("", FooterTextfnt));
                            Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Cell.FixedHeight = 12f;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            hssb2111_SummaryTable.AddCell(Cell);

                            if (hssb2111_SummaryTable.Rows.Count > 0)
                            {
                                document.Add(hssb2111_SummaryTable);
                                hssb2111_SummaryTable.DeleteBodyRows();
                                document.NewPage();
                            }
                        }
                    }

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception......................!!!")); }
                document.Close();
                fs.Close();
                fs.Dispose();

                AlertBox.Show("Report Generated Successfully");
            }
        }

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
                        Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + ", ";
                    }
                }
                Site = Selsites.Trim().TrimEnd(',');
            }

            //Row 1
            PdfPCell CH1 = new PdfPCell(new Phrase(rdoAllSite.Checked ? lblSite.Text.Trim() : "Selected Site(s)", paramsCellStyle));
            CH1.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1.Border = iTextSharp.text.Rectangle.BOX;
            CH1.PaddingBottom = 5;
            CH1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1);

            PdfPCell CB1 = new PdfPCell(new Phrase(Site, paramsCellStyle));
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

            PdfPCell CH1_2 = new PdfPCell(new Phrase(lblReportType.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            string strReportType = string.Empty;
            if (rdoDetailBoth.Checked == true)
                strReportType = rdoDetailBoth.Text;
            else if (rdoDetail.Checked == true)
                strReportType = rdoDetail.Text;
            else if (rdoSummaryO.Checked == true)
                strReportType = rdoSummaryO.Text;

            PdfPCell CB1_2 = new PdfPCell(new Phrase(strReportType, paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 2
            CH1_2 = new PdfPCell(new Phrase(rdoFSourceAll.Checked ? lblFundingSource.Text.Trim() : "Selected Funding Source(s)", paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            string strFundingSource = string.Empty;
            if (rdoFSourceAll.Checked == true)
                strFundingSource = "All Funds";
            else
            {
                string SelFunds = string.Empty;
                if (SelFundingList.Count > 0)
                {
                    foreach (CommonEntity Entity in SelFundingList)
                    {
                        SelFunds += Entity.Code + ", ";
                    }
                }
                strFundingSource = SelFunds;
            }

            CB1_2 = new PdfPCell(new Phrase(strFundingSource, paramsCellStyle));
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

            CH1_2 = new PdfPCell(new Phrase(lblReportFormdt.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(dtForm.Text.Trim() + "  To Date: " + dtTodate.Text.Trim(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 3
            CH1_2 = new PdfPCell(new Phrase(chkDetailReport.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(chkDetailReport.Checked ? "Yes" : "No", paramsCellStyle));
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

            CH1_2 = new PdfPCell(new Phrase(lblSitefor.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Text.ToString().Trim(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 4
            CH1_2 = new PdfPCell(new Phrase(lblAllFunded.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(rdoAllFundYes.Checked ? "Yes" : "No", paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By: ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On: ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

        }

        private void PrintHeaderPage_Excel(DevExpress.Spreadsheet.Workbook wb, DevExpress_Excel_Properties oDevExpress_Excel_Properties)
        {
            #region Parameters Page

            DevExpress.Spreadsheet.Worksheet paramSheet = wb.Worksheets[0];
            paramSheet.Name = "Parameters";
            paramSheet.ActiveView.TabColor = Color.ForestGreen;
            paramSheet.ActiveView.ShowGridlines = false;
            wb.Unit = DevExpress.Office.DocumentUnit.Point;

            paramSheet.Columns[1].Width = 80;
            paramSheet.Columns[2].Width = 200;
            paramSheet.Columns[3].Width = 50;
            paramSheet.Columns[4].Width = 80;
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
                        Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + ", ";
                    }
                }
                Site = Selsites.Trim().TrimEnd(',');
            }

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdoAllSite.Checked ? lblSite.Text.Trim() : "Selected Site(s)";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = Site;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblReportType.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string strReportType = string.Empty;
            if (rdoDetailBoth.Checked == true)
                strReportType = rdoDetailBoth.Text;
            else if (rdoDetail.Checked == true)
                strReportType = rdoDetail.Text;
            else if (rdoSummaryO.Checked == true)
                strReportType = rdoSummaryO.Text;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = strReportType;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdoFSourceAll.Checked ? lblFundingSource.Text.Trim() : "Selected Funding Source(s)";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string strFundingSource = string.Empty;
            if (rdoFSourceAll.Checked == true)
                strFundingSource = "All Funds";
            else
            {
                string SelFunds = string.Empty;
                if (SelFundingList.Count > 0)
                {
                    foreach (CommonEntity Entity in SelFundingList)
                    {
                        SelFunds += Entity.Code + ", ";
                    }
                }
                strFundingSource = SelFunds;
            }

            paramSheet.Rows[_Rowindex][_Columnindex].Value = strFundingSource;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblReportFormdt.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = dtForm.Text.Trim() + "  To Date: " + dtTodate.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = chkDetailReport.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = chkDetailReport.Checked ? "Yes" : "No";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblSitefor.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbSiteforHome.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;


            _Rowindex++;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblAllFunded.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdoAllFundYes.Checked ? "Yes" : "No";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

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

            #endregion
        }

        private void ExcelreportData_DevExpress(string ExcelName)
        {
            List<CaseSiteEntity> Site_list = new List<CaseSiteEntity>();
            if (rdoAllSite.Checked == true)
            {
                CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                Search_Entity.SiteAGENCY = Agency;
                Search_Entity.SiteDEPT = Dept;
                Search_Entity.SitePROG = Prog;
                Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                Site_list = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                Site_list = Site_list.FindAll(u => u.SiteROOM.Trim() != "0000");

                List<CaseSiteEntity> SelSites = new List<CaseSiteEntity>();
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
                    foreach (HierarchyEntity Entity in userHierarchy)
                    {
                        if (Entity.InActiveFlag == "N")
                        {
                            if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                hierarchyEntity = Entity;
                            else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                            {
                                hierarchyEntity = null;
                            }
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
                                    foreach (CaseSiteEntity casesite in Site_list)
                                    {
                                        if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                        {
                                            SelSites.Add(casesite);
                                        }
                                    }
                                }
                            }
                            Site_list = SelSites;
                        }
                    }
                }
            }
            else
            {
                Site_list = Sel_REFS_List;
            }

            StringBuilder strMstApplUpdate = new StringBuilder();

            #region FileNameBuild

            Random_Filename = null;
            string xlFileName = ExcelName;
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

            try
            {
                SiteScheduleEntity site_Search = new SiteScheduleEntity(true);
                site_Search.ATTM_AGENCY = Agency.Trim();
                site_Search.ATTM_DEPT = Dept;
                site_Search.ATTM_PROG = Prog;
                site_Search.ATTM_YEAR = Program_Year;
                site_Search.ATTM_MONTH = dtForm.Value.Month.ToString();
                List<SiteScheduleEntity> SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");

                int _Rowindex = 0;
                int _Columnindex = 0;

                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                    oDevExpress_Excel_Properties.sxlbook = wb;
                    oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                    oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                    oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                    PrintHeaderPage_Excel(wb, oDevExpress_Excel_Properties);

                    foreach (CaseSiteEntity siteroomsitem in Site_list)
                    {
                        List<CaseEnrlEntity> caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == siteroomsitem.SiteNUMBER.Trim() && u.Room == siteroomsitem.SiteROOM.Trim() && u.AMPM == siteroomsitem.SiteAM_PM.Trim());

                        List<CaseEnrlEntity> casewaitlistEnrlList = propCaseEnrlList.FindAll(u => u.Site.Trim() == string.Empty);
                        foreach (CaseEnrlEntity waillistitem in casewaitlistEnrlList)
                        {
                            caseEnrlList.Add(new CaseEnrlEntity(waillistitem));
                        }

                        caseEnrlList = caseEnrlList.OrderBy(u => u.Snp_L_Name).ToList();

                        DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets.Add(siteroomsitem.SiteNUMBER + siteroomsitem.SiteROOM + siteroomsitem.SiteAM_PM);

                        SheetDetails.ActiveView.TabColor = ColorTranslator.FromHtml("#ADD8E6");

                        SheetDetails.ActiveView.ShowGridlines = false;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        List<ChldAttnEntity> propChldAttnListsitewise = propChldAttnList.FindAll(u => u.SITE == siteroomsitem.SiteNUMBER.Trim() && u.ROOM == siteroomsitem.SiteROOM.Trim() && u.AMPM == siteroomsitem.SiteAM_PM.Trim());

                        #region Column Widths

                        SheetDetails.Columns[0].Width = 10;
                        SheetDetails.Columns[1].Width = 20;
                        SheetDetails.Columns[2].Width = 120;
                        SheetDetails.Columns[3].Width = 80;
                        SheetDetails.Columns[4].Width = 40;
                        SheetDetails.Columns[5].Width = 80;
                        SheetDetails.Columns[6].Width = 20;

                        #endregion

                        DateTime today = dtForm.Value;
                        int inttoday = today.Day;
                        inttoday = inttoday - 1;
                        int day = dtTodate.Value.Day;

                        DateTime now = dtForm.Value;
                        int count;
                        count = 0;
                        int intcolumns = 0;

                        for (int i = inttoday; i < day; ++i)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);

                            if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                            {
                                SheetDetails.Columns[7].Width = 25;//20;
                                intcolumns = intcolumns + 1;
                            }
                            else
                            {
                                if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if ((i + 1) == day)
                                    {
                                    }
                                    else
                                    {
                                        intcolumns = intcolumns + 1;
                                        SheetDetails.Columns[7].Width = 25;// 20;
                                    }
                                }
                            }
                        }

                        SheetDetails.Columns[8].Width = 45;
                        SheetDetails.Columns[9].Width = 45;
                        SheetDetails.Columns[10].Width = 80;
                        SheetDetails.Columns[11].Width = 35;
                        SheetDetails.Columns[12].Width = 10;

                        _Rowindex = 0;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName.Trim().ToUpper();
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (intcolumns - 1) + 10 + 1, oDevExpress_Excel_Properties.gxlFrameStyleL);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex = _Columnindex + (intcolumns - 1) + 10 + 1;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "CENTER " + siteroomsitem.SiteNUMBER + siteroomsitem.SiteROOM + siteroomsitem.SiteAM_PM;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlFrameBlockStyleC);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex = _Columnindex + 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Status";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Birth Date";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Cat. Eligibility";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Alignment.RotationAngle = 90;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 40;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "DETAIL ATTENDANCE  " + dtForm.Text + "  -  " + dtTodate.Text;
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (intcolumns - 1) + 1, oDevExpress_Excel_Properties.gxlFrameBlockStyleC);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex = _Columnindex + (intcolumns - 1) + 1;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Present/Days Open";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Monthly ADA";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Enroll";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 80;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Drop";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;
                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 80;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 60;

                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Student's Name";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex = _Columnindex + 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        for (int i = inttoday; i < day; ++i)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);

                            if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                            {
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = d.Day.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                _Columnindex++;
                            }
                            else
                            {
                                if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if ((i + 1) == day)
                                    {
                                    }
                                    else
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                        _Columnindex++;
                                    }
                                }
                            }
                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 10;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        int intstudent = 0;
                        int intOpendayscount = 0;
                        int intPresentDaysCount = 0;
                        int intAbsentDaysCount = 0;
                        bool boolApplicantdata = true;

                        foreach (CaseEnrlEntity enrldataitem in caseEnrlList)
                        {
                            boolApplicantdata = true;
                            if ((enrldataitem.Status != "E"))
                            {
                                if ((enrldataitem.Status == "T" && enrldataitem.Withdraw_Date != string.Empty))
                                {
                                    if (Convert.ToDateTime(enrldataitem.Enrl_Date) < dtForm.Value)
                                    {
                                        boolApplicantdata = false;
                                    }
                                    else
                                    {
                                        List<ChldAttnEntity> enrlappwithdrawAttn = propChldAttnListsitewise.FindAll(u => u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);
                                        if (enrlappwithdrawAttn.Count == 0)
                                        {
                                            boolApplicantdata = false;
                                        }
                                    }
                                }
                                else
                                {
                                    List<ChldAttnEntity> enrlappwithdrawAttn = propChldAttnListsitewise.FindAll(u => u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);
                                    if (enrlappwithdrawAttn.Count == 0)
                                    {
                                        boolApplicantdata = false;
                                    }
                                }
                            }
                            else
                            {
                                if (enrldataitem.Enrl_Date != string.Empty)
                                    if (Convert.ToDateTime(enrldataitem.Enrl_Date) > dtTodate.Value)
                                    {
                                        boolApplicantdata = false;
                                    }
                            }

                            if (boolApplicantdata)
                            {
                                _Rowindex++;
                                _Columnindex = 0;

                                intOpendayscount = 0;
                                intPresentDaysCount = 0;
                                intAbsentDaysCount = 0;
                                intstudent = intstudent + 1;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = intstudent;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.GetMemberName(enrldataitem.Snp_F_Name, enrldataitem.Snp_M_Name, enrldataitem.Snp_L_Name, BaseForm.BaseHierarchyCnFormat);
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = enrldataitem.App;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = enrldataitem.Status;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(enrldataitem.Snp_DOB);
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = enrldataitem.MST_MealElig;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                _Columnindex++;


                                for (int i = inttoday; i < day; ++i)
                                {
                                    DateTime d = new DateTime(now.Year, now.Month, i + 1);

                                    if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                                    {

                                        ChldAttnEntity chldattnmember = propChldAttnListsitewise.Find(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.APP_NO == enrldataitem.App && u.FUNDING_SOURCE == enrldataitem.FundHie);

                                        if (chldattnmember != null)
                                        {
                                            intOpendayscount = intOpendayscount + 1;
                                            if (chldattnmember.PA.ToString() == "A")
                                            {
                                                intAbsentDaysCount = intAbsentDaysCount + 1;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = chldattnmember.PA.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                                _Columnindex++;
                                            }
                                            else
                                            {
                                                intPresentDaysCount = intPresentDaysCount + 1;

                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = chldattnmember.PA.ToString();
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                                _Columnindex++;
                                            }
                                        }
                                        else
                                        {
                                            string strStatus = GetDateStatus(d.Date, siteroomsitem.SiteNUMBER, siteroomsitem.SiteROOM, siteroomsitem.SiteAM_PM, enrldataitem.FundHie, "");
                                            if (strStatus == "C")
                                            {
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = strStatus;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                                _Columnindex++;
                                            }
                                            else if (strStatus == "O")
                                            {
                                                if (enrldataitem.Enrl_Date != string.Empty)
                                                {
                                                    DateTime dtenrolldate = Convert.ToDateTime(LookupDataAccess.Getdate(enrldataitem.Enrl_Date));
                                                    if (d.Date >= dtenrolldate.Date && enrldataitem.Withdraw_Date == string.Empty)
                                                    {
                                                        intOpendayscount = intOpendayscount + 1;
                                                    }
                                                    else
                                                    {
                                                        if (enrldataitem.Withdraw_Date != string.Empty)
                                                        {
                                                            DateTime dtenrollwithdate = Convert.ToDateTime(LookupDataAccess.Getdate(enrldataitem.Withdraw_Date));
                                                            if (d.Date < dtenrollwithdate.Date)
                                                            {
                                                                intOpendayscount = intOpendayscount + 1;
                                                            }
                                                        }
                                                    }
                                                }
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                                _Columnindex++;
                                            }
                                            else
                                            {
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = strStatus;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                                _Columnindex++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                        {
                                            if ((i + 1) == day)
                                            {
                                            }
                                            else
                                            {
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 20;
                                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                                _Columnindex++;
                                            }
                                        }
                                    }
                                }

                                if (intAbsentDaysCount == 0)
                                {
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = intPresentDaysCount.ToString() + "/" + intPresentDaysCount.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;
                                }
                                else
                                {
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = intPresentDaysCount.ToString() + "/" + intOpendayscount.ToString();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;
                                }

                                if (intOpendayscount > 0 && intPresentDaysCount > 0)
                                {
                                    if (intAbsentDaysCount == 0)
                                    {
                                        double decpresentper = ((Convert.ToDouble(intPresentDaysCount) / Convert.ToDouble(intPresentDaysCount)) * 100);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Math.Round(decpresentper).ToString() + "%";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;
                                    }
                                    else
                                    {
                                        double decpresentper = ((Convert.ToDouble(intPresentDaysCount) / Convert.ToDouble(intOpendayscount)) * 100);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Math.Round(decpresentper).ToString() + "%";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;
                                    }
                                }
                                else
                                {
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = "0%";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;
                                }

                                if (enrldataitem.Status == "E")
                                {
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(enrldataitem.Enrl_Date);
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    if (enrldataitem.Withdraw_Date != string.Empty)
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(enrldataitem.Withdraw_Date);
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;
                                    }
                                    else
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        _Columnindex++;
                                    }
                                }
                                else
                                {
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(enrldataitem.Status_Date);
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(enrldataitem.Withdraw_Date);
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                    _Columnindex++;
                                }

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            }

                        }

                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "# Present";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        int intTotalPresentCount = 0;

                        for (int i = inttoday; i < day; ++i)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);

                            if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                            {
                                List<ChldAttnEntity> chldattnpresent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "P");
                                intTotalPresentCount = intTotalPresentCount + chldattnpresent.Count;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = chldattnpresent.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                //SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                _Columnindex++;
                            }
                            else
                            {
                                if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if ((i + 1) == day)
                                    {
                                    }
                                    else
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                        _Columnindex++;
                                    }
                                }
                            }
                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = intTotalPresentCount.ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

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
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "# Absent";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        int intTotalAbsentCount = 0;

                        for (int i = inttoday; i < day; ++i)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            //Compare date with sunday

                            if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                            {
                                List<ChldAttnEntity> chldattnabsent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "A");
                                intTotalAbsentCount = intTotalAbsentCount + chldattnabsent.Count;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = chldattnabsent.Count.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                //SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                _Columnindex++;
                            }
                            else
                            {
                                if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if ((i + 1) == day)
                                    {
                                    }
                                    else
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                        _Columnindex++;
                                    }
                                }
                            }
                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = intTotalAbsentCount.ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

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
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "% Present";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        double decapplicanpercent = 0;
                        for (int i = inttoday; i < day; ++i)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            //Compare date with sunday
                            decapplicanpercent = 0;
                            if (d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday)
                            {
                                List<ChldAttnEntity> chldattnpresent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "P");
                                List<ChldAttnEntity> chldattnabsent = propChldAttnListsitewise.FindAll(u => LookupDataAccess.Getdate(u.DATE) == LookupDataAccess.Getdate(d.Date.ToShortDateString()) && u.PA == "A");
                                if (chldattnpresent.Count > 0 && intstudent > 0)
                                    decapplicanpercent = (Convert.ToDouble(chldattnpresent.Count) / Convert.ToDouble(chldattnpresent.Count + chldattnabsent.Count)) * 100;
                                
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = Math.Round(decapplicanpercent).ToString() + "%";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                                _Columnindex++;
                            }
                            else
                            {
                                if (i > 1 && d.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if ((i + 1) == day)
                                    {
                                    }
                                    else
                                    {
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGray;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                                        SheetDetails.Rows[_Rowindex][_Columnindex].ColumnWidth = 25;
                                        _Columnindex++;
                                    }
                                }
                            }

                        }

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = (intTotalPresentCount + intTotalAbsentCount).ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        _Rowindex++;
                        _Columnindex = 0;

                        //Footer
                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "©2024 CAPSystems Inc";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, (intcolumns - 1) + 10 + 1, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex = _Columnindex + (intcolumns - 1) + 10 + 1;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                        _Rowindex = _Rowindex + 2;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Items";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "This Month";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Items";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails,_Rowindex,_Columnindex,5, oDevExpress_Excel_Properties.gxlFrameBlockStyleL);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex = _Columnindex + 5;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "This Month";
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlFrameBlockStyleC);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "1. # of Class Days";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = intOpendayscount.ToString();
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "4. # Present";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex = _Columnindex + 5;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = intTotalPresentCount.ToString();
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "2. # Enrolled";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "5.  # Absent";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex = _Columnindex + 5;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = intTotalAbsentCount.ToString();
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "3. # Dropped";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "6. ADA (#4/#1)";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex = _Columnindex + 5;

                        if (intOpendayscount > 0)
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = (Math.Round(Convert.ToDouble(intTotalPresentCount) / Convert.ToDouble(intOpendayscount))).ToString();
                        else
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                       SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;


                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "7. Classroom";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 5, oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex = _Columnindex + 5;

                        if ((intTotalPresentCount + intTotalAbsentCount) > 0)
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = (Math.Round((Convert.ToDouble(intTotalPresentCount) / Convert.ToDouble((intTotalPresentCount + intTotalAbsentCount))) * 100).ToString()) + " %";
                        else
                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        //SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders);
                        SheetDetails.Rows[_Rowindex][_Columnindex].FillColor = Color.LightGreen;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);
                    }

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

            }
        }

        #endregion

    }
}