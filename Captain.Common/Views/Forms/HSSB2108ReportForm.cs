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
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2108ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public HSSB2108ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
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
            txtFrom.Validator = TextBoxValidation.IntegerValidator;
            txtTo.Validator = TextBoxValidation.IntegerValidator;
            List<CommonEntity> listSequence = _model.lookupDataAccess.GetHssb2108FormActiveDetails();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propMealTypes = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
            propAppsentReasons = _model.lookupDataAccess.GetAgyTabRecordsByCode("00107");
            if (propAppsentReasons.Count > 0)
                propAppsentReasons = propAppsentReasons.FindAll(u => u.Active.ToString() == "Y");
            Member_NameFormat = baseForm.BaseHierarchyCnFormat.ToString();
            CAseWorkerr_NameFormat = baseForm.BaseHierarchyCwFormat.ToString();

            foreach (CommonEntity item in listSequence)
            {
                cmbActive.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbActive.SelectedIndex = 2;
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
        public List<ChildATTMSEntity> propchldAttmsEntityList { get; set; }
        public List<CommonEntity> propAppsentReasons { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }
        public string propMealTypes { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        
        #endregion

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
           /* HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports", BaseForm.UserID);
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
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);

                    // propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");
                }

            }
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
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(650, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(570, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(650, 25);
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
            dtSecond.LostFocus -= new EventHandler(dtSecond_LostFocus);
            dtthird.LostFocus -= new EventHandler(dtthird_LostFocus);
            dtFourth.LostFocus -= new EventHandler(dtFourth_LostFocus);
            dtFifth.LostFocus -= new EventHandler(dtFifth_LostFocus);

            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                _errorProvider.SetError(dtstartDate, null);
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
                dtstartDate.Checked = false;
                dtSecond.Checked = false;
                dtthird.Checked = false;
                dtFourth.Checked = false;
                dtFifth.Checked = false;
            }
            dtSecond.LostFocus += new EventHandler(dtSecond_LostFocus);
            dtthird.LostFocus += new EventHandler(dtthird_LostFocus);
            dtFourth.LostFocus += new EventHandler(dtFourth_LostFocus);
            dtFifth.LostFocus += new EventHandler(dtFifth_LostFocus);
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                propCaseEnrlList = _model.EnrollData.GetEnrollDetails2108(Agency, Dept, Prog, Program_Year, rdoTodayDate.Checked == true ? "T" : "K");
                propCaseEnrlList = propCaseEnrlList.FindAll(u => u.Snp_Age != string.Empty);
                propchldAttmsEntityList = _model.SPAdminData.GetChldAttMsDetails(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "ALL");
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }

        }

        #region PdfReportCode

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null; string PdfName = null;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        string strReasonCodes = string.Empty;
        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            strReasonCodes = string.Empty;
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                List<CommonEntity> commonDate = new List<CommonEntity>();
                if (dtstartDate.Checked == true)
                    commonDate.Add(new CommonEntity(dtstartDate.Value.ToShortDateString(), string.Empty));
                if (dtSecond.Checked == true)
                    commonDate.Add(new CommonEntity(dtSecond.Value.ToShortDateString(), string.Empty));
                if (dtthird.Checked == true)
                    commonDate.Add(new CommonEntity(dtthird.Value.ToShortDateString(), string.Empty));
                if (dtFourth.Checked == true)
                    commonDate.Add(new CommonEntity(dtFourth.Value.ToShortDateString(), string.Empty));
                if (dtFifth.Checked == true)
                    commonDate.Add(new CommonEntity(dtFifth.Value.ToShortDateString(), string.Empty));



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
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 8);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 8);
                cb = writer.DirectContent;
                try
                {



                    PrintHeaderPage(document, writer);

                    if (rdoLandscape.Checked)
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                    foreach (CommonEntity dr in propAppsentReasons)
                    {
                        strReasonCodes = strReasonCodes + " (" + dr.Code.Trim() + ") " + dr.Desc.Trim();
                    }



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
                    document.NewPage();
                    PdfPTable table = new PdfPTable(9);

                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    if (rdoLandscape.Checked)
                        table.TotalWidth = 750f;
                    else
                        table.TotalWidth = 550f;
                    float[] widths = new float[] { 60f, 20f, 15f, 42f, 42f, 42f, 42f, 42f, 35f };
                    table.SetWidths(widths);



                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.FooterRows = 3;

                    DateTime dtMonday, dtTues, dtWed, dtThurs, dtFri;
                    if (dtstartDate.Value.Month != 9)
                    {

                        foreach (CaseSiteEntity item in Site_list)
                        {

                            propMealTypes = item.SiteMEAL_AREA.ToString();
                            // document.NewPage();
                            List<CaseEnrlEntity> caseEnrlList;
                            if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                            {
                                caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == item.SiteNUMBER.Trim() && u.Room == item.SiteROOM.Trim() && u.AMPM == item.SiteAM_PM.Trim() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                            }
                            else
                            {
                                caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == item.SiteNUMBER.Trim() && u.Room == item.SiteROOM.Trim() && u.AMPM == item.SiteAM_PM.Trim() && u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                            }
                            if (caseEnrlList.Count > 0)
                            {



                                foreach (CommonEntity weekDates in commonDate)
                                {

                                    List<ENRL_Asof_Entity> ENRLDateAllList = new List<ENRL_Asof_Entity>();

                                    PdfPCell pdfClassHeader = new PdfPCell(new Phrase("Class Room:" + item.SiteNUMBER.Trim() + " / " + item.SiteROOM.Trim() + " / " + item.SiteAM_PM.Trim(), TableFont));
                                    pdfClassHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfClassHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassHeader.Colspan = 2;
                                    table.AddCell(pdfClassHeader);

                                    PdfPCell pdfClassDetails = new PdfPCell(new Phrase(item.SiteNAME.Trim(), TableFont));
                                    pdfClassDetails.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfClassDetails.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassDetails.Colspan = 3;
                                    table.AddCell(pdfClassDetails);

                                    PdfPCell pdfClassDate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(weekDates.Code), TableFont));
                                    pdfClassDate.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfClassDate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassDate.Colspan = 1;
                                    table.AddCell(pdfClassDate);

                                    PdfPCell pdfSiteComment = new PdfPCell(new Phrase(item.SiteCOMMENT.Trim(), TableFont));
                                    pdfSiteComment.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSiteComment.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfSiteComment.Colspan = 3;
                                    table.AddCell(pdfSiteComment);

                                    PdfPCell pdfCompBy = new PdfPCell(new Phrase("Comp By:_________________________________   ", TableFont));
                                    pdfCompBy.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfCompBy.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfCompBy.Colspan = 4;
                                    table.AddCell(pdfCompBy);

                                    PdfPCell pdfattPosting = new PdfPCell(new Phrase("     Att Posting Initials:____________", TableFont));
                                    pdfattPosting.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfattPosting.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfattPosting.Colspan = 5;
                                    table.AddCell(pdfattPosting);

                                    PdfPCell pdfspaceh = new PdfPCell(new Phrase("", TableFont));
                                    pdfspaceh.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfspaceh.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfspaceh.Colspan = 9;
                                    table.AddCell(pdfspaceh);






                                    PdfPCell Headercell1 = new PdfPCell(new Phrase("Child Name", TableFont));
                                    Headercell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headercell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell1);


                                    PdfPCell Headercell2 = new PdfPCell(new Phrase("Funding", TableFont));
                                    Headercell2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headercell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell2);

                                    PdfPCell Headercell3 = new PdfPCell(new Phrase("Meal", TableFont));
                                    Headercell3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell3);

                                    PdfPCell Headercell4 = new PdfPCell(new Phrase("Monday", TableFont));
                                    Headercell4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell4);

                                    PdfPCell Headercell5 = new PdfPCell(new Phrase("Tuesday", TableFont));
                                    Headercell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell5);

                                    PdfPCell Headercell6 = new PdfPCell(new Phrase("Wednesday", TableFont));
                                    Headercell6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell6);

                                    PdfPCell Headercell7 = new PdfPCell(new Phrase("Thursday", TableFont));
                                    Headercell7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell7.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell7);

                                    PdfPCell Headercell8 = new PdfPCell(new Phrase("Friday", TableFont));
                                    Headercell8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell8.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell8);

                                    PdfPCell Headercell9 = new PdfPCell(new Phrase("Total Days Absent", TableFont));
                                    Headercell9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell9);


                                    //------ Data------


                                    PdfPCell Headerdata1 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headerdata1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata1);


                                    PdfPCell Headerdata2 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headerdata2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata2);

                                    PdfPCell Headerdata3 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata3);
                                    dtMonday = Convert.ToDateTime(weekDates.Code);
                                    dtTues = Convert.ToDateTime(weekDates.Code).AddDays(1);
                                    dtWed = Convert.ToDateTime(weekDates.Code).AddDays(2);
                                    dtThurs = Convert.ToDateTime(weekDates.Code).AddDays(3);
                                    dtFri = Convert.ToDateTime(weekDates.Code).AddDays(4);

                                    List<ENRL_Asof_Entity> ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtMonday.ToShortDateString());

                                    if (ENRLDateList.Count > 0)
                                    {
                                        foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                        {
                                            ENRLDateAllList.Add(Enrlitem);
                                        }
                                    }

                                    ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtTues.ToShortDateString());

                                    if (ENRLDateList.Count > 0)
                                    {
                                        foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                        {
                                            ENRLDateAllList.Add(Enrlitem);
                                        }
                                    }

                                    ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtWed.ToShortDateString());

                                    if (ENRLDateList.Count > 0)
                                    {
                                        foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                        {
                                            ENRLDateAllList.Add(Enrlitem);
                                        }
                                    }

                                    ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtThurs.ToShortDateString());

                                    if (ENRLDateList.Count > 0)
                                    {
                                        foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                        {
                                            ENRLDateAllList.Add(Enrlitem);
                                        }
                                    }

                                    ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtFri.ToShortDateString());

                                    if (ENRLDateList.Count > 0)
                                    {
                                        foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                        {
                                            ENRLDateAllList.Add(Enrlitem);
                                        }
                                    }

                                    PdfPCell Headerdata4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(weekDates.Code), TableFont));
                                    Headerdata4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata4);

                                    PdfPCell Headerdata5 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtTues.ToShortDateString()), TableFont));
                                    Headerdata5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata5);

                                    PdfPCell Headerdata6 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtWed.ToShortDateString()), TableFont));
                                    Headerdata6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata6);

                                    PdfPCell Headerdata7 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtThurs.ToShortDateString()), TableFont));
                                    Headerdata7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata7.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata7);

                                    PdfPCell Headerdata8 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtFri.ToShortDateString()), TableFont));
                                    Headerdata8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata8.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata8);

                                    PdfPCell Headerdata9 = new PdfPCell(new Phrase("For Week", TableFont));
                                    Headerdata9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata9);

                                    PdfPCell headerSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    headerSpace1.Colspan = 9;
                                    headerSpace1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    headerSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(headerSpace1);

                                    if (rdoYes.Checked == true)
                                    {

                                        PdfPCell headerSpace2 = new PdfPCell(new Phrase("", TableFont));
                                        headerSpace2.Colspan = 9;
                                        headerSpace2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        headerSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(headerSpace2);

                                        PdfPCell pdfNonMeals = new PdfPCell(new Phrase("NON-CACFP ADULT MEALS", TableFont));
                                        pdfNonMeals.Colspan = 3;
                                        pdfNonMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfNonMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfNonMeals);



                                        MealsDefaultSettings("MO");
                                        PdfPCell pdfNonMealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                        pdfNonMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealMonday);

                                        MealsDefaultSettings("TU");
                                        PdfPCell pdfNonMealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                        pdfNonMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealTuesday);

                                        MealsDefaultSettings("WE");
                                        PdfPCell pdfNonMealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                        pdfNonMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealWed);

                                        MealsDefaultSettings("TH");
                                        PdfPCell pdfNonMealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                        pdfNonMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealThus);

                                        MealsDefaultSettings("FR");
                                        PdfPCell pdfNonMealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                        pdfNonMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealFri);

                                        PdfPCell pdfNonMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfNonMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealTotal);

                                        PdfPCell headerSpace3 = new PdfPCell(new Phrase("", TableFont));
                                        headerSpace3.Colspan = 9;
                                        headerSpace3.HorizontalAlignment = Element.ALIGN_CENTER;
                                        headerSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(headerSpace3);


                                        PdfPCell pdfMeals = new PdfPCell(new Phrase("CACFP ADULT MEAL CNTS", TableFont));
                                        pdfMeals.Colspan = 3;
                                        pdfMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfMeals);




                                        PdfPCell pdfMealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                        pdfMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealMonday);

                                        PdfPCell pdfMealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                        pdfMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealTuesday);

                                        PdfPCell pdfMealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                        pdfMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealWed);

                                        PdfPCell pdfMealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                        pdfMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealThus);

                                        PdfPCell pdfMealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                        pdfMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealFri);

                                        PdfPCell pdfMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealTotal);



                                        PdfPCell pdfHrLine1 = new PdfPCell(new Phrase("", TableFont));
                                        pdfHrLine1.Colspan = 9;
                                        pdfHrLine1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfHrLine1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(pdfHrLine1);



                                        PdfPCell pdfSpaMeals = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpaMeals.Colspan = 3;
                                        pdfSpaMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpaMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpaMeals);




                                        PdfPCell pdfSpaMealMonday = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                        pdfSpaMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealMonday);

                                        PdfPCell pdfSpaMealTuesday = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                        pdfSpaMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealTuesday);

                                        PdfPCell pdfSpaMealWed = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                        pdfSpaMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealWed);

                                        PdfPCell pdfSpaMealThus = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                        pdfSpaMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealThus);

                                        PdfPCell pdfSpaMealFri = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                        pdfSpaMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealFri);

                                        PdfPCell pdfSpaMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpaMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealTotal);


                                        PdfPCell pdfSpace1 = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpace1.Colspan = 9;
                                        pdfSpace1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpace1);



                                        PdfPCell pdfSpa1Meals = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa1Meals.Colspan = 3;
                                        pdfSpa1Meals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpa1Meals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpa1Meals);




                                        PdfPCell pdfSpa1MealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                        pdfSpa1MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealMonday);

                                        PdfPCell pdfSpa1MealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                        pdfSpa1MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealTuesday);

                                        PdfPCell pdfSpa1MealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                        pdfSpa1MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealWed);

                                        PdfPCell pdfSpa1MealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                        pdfSpa1MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealThus);

                                        PdfPCell pdfSpa1MealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                        pdfSpa1MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealFri);

                                        PdfPCell pdfSpa1MealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa1MealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealTotal);
                                    }
                                    else
                                    {
                                        MealsDefaultSettings("MO");
                                        MealsDefaultSettings("TU");
                                        MealsDefaultSettings("WE");
                                        MealsDefaultSettings("TH");
                                        MealsDefaultSettings("FR");
                                    }
                                    PdfPCell pdfHrLine2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfHrLine2.Colspan = 9;
                                    pdfHrLine2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfHrLine2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table.AddCell(pdfHrLine2);


                                    PdfPCell pdfSpa2Meals = new PdfPCell(new Phrase("", TableFont));
                                    pdfSpa2Meals.Colspan = 3;
                                    pdfSpa2Meals.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfSpa2Meals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(pdfSpa2Meals);



                                    strMealDetails = strMondayMeal.Replace('_', ' ');
                                    PdfPCell pdfSpa2MealMonday = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                    pdfSpa2MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa2MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa2MealMonday);

                                    strMealDetails = strTuesMeal.Replace('_', ' ');
                                    PdfPCell pdfSpa2MealTuesday = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                    pdfSpa2MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa2MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa2MealTuesday);

                                    strMealDetails = strWedMeal.Replace('_', ' ');
                                    PdfPCell pdfSpa2MealWed = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                    pdfSpa2MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa2MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa2MealWed);

                                    strMealDetails = strThusMeal.Replace('_', ' ');
                                    PdfPCell pdfSpa3MealThus = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                    pdfSpa3MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa3MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa3MealThus);

                                    strMealDetails = strFriMeal.Replace('_', ' ');
                                    PdfPCell pdfSpa4MealFri = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                    pdfSpa4MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa4MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa4MealFri);

                                    PdfPCell pdfSpa5MealTotal = new PdfPCell(new Phrase("", TableFont));
                                    pdfSpa5MealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa5MealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa5MealTotal);

                                    int intcaseEnrlcountData = 0;
                                    foreach (CaseEnrlEntity EnrlDetails in caseEnrlList)
                                    {

                                        ENRL_Asof_Entity enrlasofapp = ENRLDateAllList.Find(u => u.App == EnrlDetails.App && u.Status == "E" && u.FundHie.Trim() == EnrlDetails.FundHie.Trim());

                                        if (enrlasofapp != null)
                                        {
                                            intcaseEnrlcountData = intcaseEnrlcountData + 1;
                                            PdfPCell pdfchildDetails = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(EnrlDetails.Snp_F_Name, EnrlDetails.Snp_M_Name, EnrlDetails.Snp_L_Name, Member_NameFormat), TableFont));
                                            pdfchildDetails.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfchildDetails.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfchildDetails);


                                            PdfPCell pdfFundingDesc = new PdfPCell(new Phrase(EnrlDetails.FundHie, TableFont));
                                            pdfFundingDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfFundingDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfFundingDesc);

                                            PdfPCell pdfMealDesc = new PdfPCell(new Phrase(DisplayMealElig(EnrlDetails.MST_MealElig.Trim()), TableFont));
                                            pdfMealDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfMealDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfMealDesc);




                                            PdfPCell pdfMondayDesc = new PdfPCell(new Phrase(GetDateStatus(dtMonday.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "MO"), TableFont));
                                            pdfMondayDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMondayDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMondayDesc);

                                            PdfPCell pdfTuesDesc = new PdfPCell(new Phrase(GetDateStatus(dtTues.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "TU"), TableFont));
                                            pdfTuesDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfTuesDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfTuesDesc);

                                            PdfPCell pdfWedDesc = new PdfPCell(new Phrase(GetDateStatus(dtWed.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "WE"), TableFont));
                                            pdfWedDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfWedDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfWedDesc);

                                            PdfPCell pdfThurDesc = new PdfPCell(new Phrase(GetDateStatus(dtThurs.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "TH"), TableFont));
                                            pdfThurDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfThurDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfThurDesc);

                                            PdfPCell pdfFridesc = new PdfPCell(new Phrase(GetDateStatus(dtFri.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "FR"), TableFont));
                                            pdfFridesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfFridesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfFridesc);

                                            PdfPCell pdfTotalDesc = new PdfPCell(new Phrase("______", TableFont));
                                            pdfTotalDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfTotalDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfTotalDesc);

                                            if (chkPrint.Checked == true)
                                            {
                                                PdfPCell pdfChildNewLine = new PdfPCell(new Phrase("", TableFont));
                                                pdfChildNewLine.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfChildNewLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                pdfChildNewLine.Colspan = 9;
                                                pdfChildNewLine.FixedHeight = 10f;
                                                table.AddCell(pdfChildNewLine);
                                            }

                                        }
                                    }

                                    for (int i = 0; i < 3; i++)
                                    {


                                        PdfPCell pdfChildNameSpace = new PdfPCell(new Phrase("____________________", TableFont));
                                        pdfChildNameSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildNameSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        pdfChildNameSpace.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildNameSpace);

                                        PdfPCell pdfFundingSpace = new PdfPCell(new Phrase("_______", TableFont));
                                        pdfFundingSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfFundingSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        pdfFundingSpace.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfFundingSpace);

                                        PdfPCell pdfFreeSpace = new PdfPCell(new Phrase("", TableFont));
                                        pdfFreeSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfFreeSpace.FixedHeight = 10f;
                                        pdfFreeSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfFreeSpace);


                                        PdfPCell pdfChildSpaceMonday = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceMonday.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceMonday);

                                        PdfPCell pdfChildSpaceTuesday = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceTuesday.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceTuesday);

                                        PdfPCell pdfChildSpaceWed = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceWed.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceWed);

                                        PdfPCell pdfChildSpaceThus = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceThus.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceThus);

                                        PdfPCell pdfChildSpaceFri = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceFri.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceFri);

                                        PdfPCell pdfChildSpaceTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceTotal.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceTotal);

                                    }

                                    PdfPCell pdfTotalChildSpace = new PdfPCell(new Phrase("Total Children Attending Daily:", TableFont));
                                    pdfTotalChildSpace.Colspan = 2;
                                    pdfTotalChildSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfTotalChildSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildSpace);



                                    PdfPCell pdfTotalFreeSpace = new PdfPCell(new Phrase("", TableFont));
                                    pdfTotalFreeSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfTotalFreeSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalFreeSpace);


                                    PdfPCell pdfTotalChildMonday = new PdfPCell(new Phrase("Mon _____", TableFont));
                                    pdfTotalChildMonday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfTotalChildMonday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildMonday);

                                    PdfPCell pdfTotalChildTuesday = new PdfPCell(new Phrase("Tue _____", TableFont));
                                    pdfTotalChildTuesday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfTotalChildTuesday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildTuesday);

                                    PdfPCell pdfTotalChildWed = new PdfPCell(new Phrase("Wed _____", TableFont));
                                    pdfTotalChildWed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfTotalChildWed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildWed);

                                    PdfPCell pdfTotalChildThus = new PdfPCell(new Phrase("Thu _____", TableFont));
                                    pdfTotalChildThus.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfTotalChildThus.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildThus);

                                    PdfPCell pdfTotalChildFri = new PdfPCell(new Phrase("Fri _____", TableFont));
                                    pdfTotalChildFri.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfTotalChildFri.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildFri);

                                    PdfPCell pdfTotalChildTotal = new PdfPCell(new Phrase("", TableFont));
                                    pdfTotalChildTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfTotalChildTotal.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildTotal);


                                    PdfPCell pdfBottomSpace = new PdfPCell(new Phrase("", TableFont));
                                    pdfBottomSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfBottomSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfBottomSpace.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfBottomSpace);

                                    PdfPCell pdfBottomSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfBottomSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfBottomSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfBottomSpace1.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfBottomSpace1);


                                    PdfPCell pdfReasons = new PdfPCell(new Phrase("Reason For Absence Codes: ", TableFont));
                                    pdfReasons.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfReasons.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfReasons.Colspan = 5;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfReasons);

                                    PdfPCell pdfchildcount = new PdfPCell(new Phrase("Number children enrolled: " + intcaseEnrlcountData, TableFont));//caseEnrlList.Count
                                    pdfchildcount.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfchildcount.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfchildcount.Colspan = 4;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfchildcount);

                                    PdfPCell pdfReasonsdesc = new PdfPCell(new Phrase(strReasonCodes, TableFont));
                                    pdfReasonsdesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfReasonsdesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfReasonsdesc.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfReasonsdesc);

                                    table.FooterRows = 3;

                                    //PdfPCell pdfBottomSpace2 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfBottomSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfBottomSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //pdfBottomSpace2.Colspan = 9;
                                    ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //table.AddCell(pdfBottomSpace2);

                                    //PdfPCell pdfBottomSpace3 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfBottomSpace3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfBottomSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //pdfBottomSpace3.Colspan = 9;
                                    ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //table.AddCell(pdfBottomSpace3);

                                    if (table.Rows.Count > 0)
                                    {
                                        document.Add(table);
                                        table.DeleteBodyRows();
                                        document.NewPage();
                                    }

                                }
                            }

                        }
                    }
                    else
                    {

                        // New logic Septerber



                        foreach (CaseSiteEntity item in Site_list)
                        {

                            propMealTypes = item.SiteMEAL_AREA.ToString();
                            // document.NewPage();
                            List<CaseEnrlEntity> caseEnrlList;
                            if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                            {
                                caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == item.SiteNUMBER.Trim() && u.Room == item.SiteROOM.Trim() && u.AMPM == item.SiteAM_PM.Trim() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                            }
                            else
                            {
                                caseEnrlList = propCaseEnrlList.FindAll(u => u.Site == item.SiteNUMBER.Trim() && u.Room == item.SiteROOM.Trim() && u.AMPM == item.SiteAM_PM.Trim() && u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                            }
                            if (caseEnrlList.Count > 0)
                            {

                                foreach (CommonEntity weekDates in commonDate)
                                {

                                    int intstartdays = 0;
                                    bool boolSepdata = false;

                                    DateTime datestartvalue = Convert.ToDateTime(weekDates.Code);

                                    if (dtstartDate.Value.Month == 09 || dtstartDate.Value.Month == 9)
                                    {
                                        if (datestartvalue.DayOfWeek != DayOfWeek.Monday)
                                        {
                                            boolSepdata = true;
                                            switch (datestartvalue.DayOfWeek)
                                            {
                                                case DayOfWeek.Friday:
                                                    intstartdays = 1;
                                                    break;
                                                //case DayOfWeek.Monday:
                                                //    intstartdays = 5;
                                                //    break;
                                                case DayOfWeek.Saturday:
                                                    break;
                                                case DayOfWeek.Sunday:
                                                    break;
                                                case DayOfWeek.Thursday:
                                                    intstartdays = 2;
                                                    break;
                                                case DayOfWeek.Tuesday:
                                                    intstartdays = 4;
                                                    break;
                                                case DayOfWeek.Wednesday:
                                                    intstartdays = 3;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    List<ENRL_Asof_Entity> ENRLDateAllList = new List<ENRL_Asof_Entity>();

                                    PdfPCell pdfClassHeader = new PdfPCell(new Phrase("Class Room:" + item.SiteNUMBER.Trim() + " / " + item.SiteROOM.Trim() + " / " + item.SiteAM_PM.Trim(), TableFont));
                                    pdfClassHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfClassHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassHeader.Colspan = 2;
                                    table.AddCell(pdfClassHeader);

                                    PdfPCell pdfClassDetails = new PdfPCell(new Phrase(item.SiteNAME.Trim(), TableFont));
                                    pdfClassDetails.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfClassDetails.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassDetails.Colspan = 3;
                                    table.AddCell(pdfClassDetails);

                                    PdfPCell pdfClassDate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(weekDates.Code), TableFont));
                                    pdfClassDate.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfClassDate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfClassDate.Colspan = 1;
                                    table.AddCell(pdfClassDate);

                                    PdfPCell pdfSiteComment = new PdfPCell(new Phrase(item.SiteCOMMENT.Trim(), TableFont));
                                    pdfSiteComment.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSiteComment.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfSiteComment.Colspan = 3;
                                    table.AddCell(pdfSiteComment);

                                    PdfPCell pdfCompBy = new PdfPCell(new Phrase("Comp By:_________________________________   ", TableFont));
                                    pdfCompBy.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfCompBy.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfCompBy.Colspan = 4;
                                    table.AddCell(pdfCompBy);

                                    PdfPCell pdfattPosting = new PdfPCell(new Phrase("     Att Posting Initials:____________", TableFont));
                                    pdfattPosting.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfattPosting.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfattPosting.Colspan = 5;
                                    table.AddCell(pdfattPosting);

                                    PdfPCell pdfspaceh = new PdfPCell(new Phrase("", TableFont));
                                    pdfspaceh.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfspaceh.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfspaceh.Colspan = 9;
                                    table.AddCell(pdfspaceh);






                                    PdfPCell Headercell1 = new PdfPCell(new Phrase("Child Name", TableFont));
                                    Headercell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headercell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell1);


                                    PdfPCell Headercell2 = new PdfPCell(new Phrase("Funding", TableFont));
                                    Headercell2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headercell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell2);

                                    PdfPCell Headercell3 = new PdfPCell(new Phrase("Meal", TableFont));
                                    Headercell3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headercell3);

                                    if (boolSepdata)
                                    {
                                        dtMonday = dtTues = dtWed = dtThurs = dtFri = Convert.ToDateTime("01/01/1990");
                                        switch (intstartdays)
                                        {
                                            case 1:
                                                dtFri = Convert.ToDateTime(weekDates.Code);
                                                break;
                                            case 2:
                                                dtThurs = Convert.ToDateTime(weekDates.Code);
                                                dtFri = Convert.ToDateTime(weekDates.Code).AddDays(1);
                                                break;
                                            case 3:
                                                dtWed = Convert.ToDateTime(weekDates.Code);
                                                dtThurs = Convert.ToDateTime(weekDates.Code).AddDays(1);
                                                dtFri = Convert.ToDateTime(weekDates.Code).AddDays(2);
                                                break;
                                            case 4:
                                                dtTues = Convert.ToDateTime(weekDates.Code);
                                                dtWed = Convert.ToDateTime(weekDates.Code).AddDays(1);
                                                dtThurs = Convert.ToDateTime(weekDates.Code).AddDays(2);
                                                dtFri = Convert.ToDateTime(weekDates.Code).AddDays(3);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        dtMonday = Convert.ToDateTime(weekDates.Code);
                                        dtTues = Convert.ToDateTime(weekDates.Code).AddDays(1);
                                        dtWed = Convert.ToDateTime(weekDates.Code).AddDays(2);
                                        dtThurs = Convert.ToDateTime(weekDates.Code).AddDays(3);
                                        dtFri = Convert.ToDateTime(weekDates.Code).AddDays(4);
                                    }


                                    PdfPCell Headercell4 = new PdfPCell(new Phrase("Monday", TableFont));
                                    Headercell4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell4);

                                    PdfPCell Headercell5 = new PdfPCell(new Phrase("Tuesday", TableFont));
                                    Headercell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell5);

                                    PdfPCell Headercell6 = new PdfPCell(new Phrase("Wednesday", TableFont));
                                    Headercell6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell6);

                                    PdfPCell Headercell7 = new PdfPCell(new Phrase("Thursday", TableFont));
                                    Headercell7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell7.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell7);

                                    PdfPCell Headercell8 = new PdfPCell(new Phrase("Friday", TableFont));
                                    Headercell8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell8.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell8);

                                    PdfPCell Headercell9 = new PdfPCell(new Phrase("Total Days Absent", TableFont));
                                    Headercell9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headercell9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headercell9);


                                    //------ Data------


                                    PdfPCell Headerdata1 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headerdata1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata1);


                                    PdfPCell Headerdata2 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Headerdata2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata2);

                                    PdfPCell Headerdata3 = new PdfPCell(new Phrase("", TableFont));
                                    Headerdata3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Headerdata3);



                                    List<ENRL_Asof_Entity> ENRLDateList;
                                    if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtMonday.ToShortDateString());

                                        if (ENRLDateList.Count > 0)
                                        {
                                            foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                            {
                                                ENRLDateAllList.Add(Enrlitem);
                                            }
                                        }
                                    }
                                    if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtTues.ToShortDateString());

                                        if (ENRLDateList.Count > 0)
                                        {
                                            foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                            {
                                                ENRLDateAllList.Add(Enrlitem);
                                            }
                                        }
                                    }
                                    if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtWed.ToShortDateString());

                                        if (ENRLDateList.Count > 0)
                                        {
                                            foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                            {
                                                ENRLDateAllList.Add(Enrlitem);
                                            }
                                        }
                                    }
                                    if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtThurs.ToShortDateString());

                                        if (ENRLDateList.Count > 0)
                                        {
                                            foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                            {
                                                ENRLDateAllList.Add(Enrlitem);
                                            }
                                        }
                                    }
                                    if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        ENRLDateList = _model.EnrollData.Browse_CASEENRL_Asof_Date_Attendance(Agency, Dept, Prog, Program_Year, string.Empty, string.Empty, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, dtFri.ToShortDateString());

                                        if (ENRLDateList.Count > 0)
                                        {
                                            foreach (ENRL_Asof_Entity Enrlitem in ENRLDateList)
                                            {
                                                ENRLDateAllList.Add(Enrlitem);
                                            }
                                        }
                                    }

                                    if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell Headerdata4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(weekDates.Code), TableFont));
                                        Headerdata4.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata4);
                                    }
                                    else
                                    {
                                        PdfPCell Headerdata4 = new PdfPCell(new Phrase("", TableFont));
                                        Headerdata4.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata4.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata4);
                                    }
                                    if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell Headerdata5 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtTues.ToShortDateString()), TableFont));
                                        Headerdata5.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata5);
                                    }
                                    else
                                    {
                                        PdfPCell Headerdata5 = new PdfPCell(new Phrase("", TableFont));
                                        Headerdata5.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata5);

                                    }
                                    if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell Headerdata6 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtWed.ToShortDateString()), TableFont));
                                        Headerdata6.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata6);
                                    }
                                    else
                                    {
                                        PdfPCell Headerdata6 = new PdfPCell(new Phrase("", TableFont));
                                        Headerdata6.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata6);
                                    }
                                    if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell Headerdata7 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtThurs.ToShortDateString()), TableFont));
                                        Headerdata7.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata7.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata7);
                                    }
                                    else
                                    {
                                        PdfPCell Headerdata7 = new PdfPCell(new Phrase("", TableFont));
                                        Headerdata7.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata7.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata7);
                                    }
                                    if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell Headerdata8 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtFri.ToShortDateString()), TableFont));
                                        Headerdata8.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata8.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata8);
                                    }
                                    else
                                    {
                                        PdfPCell Headerdata8 = new PdfPCell(new Phrase("", TableFont));
                                        Headerdata8.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Headerdata8.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Headerdata8);
                                    }

                                    PdfPCell Headerdata9 = new PdfPCell(new Phrase("For Week", TableFont));
                                    Headerdata9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Headerdata9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Headerdata9);

                                    PdfPCell headerSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    headerSpace1.Colspan = 9;
                                    headerSpace1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    headerSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(headerSpace1);

                                    if (rdoYes.Checked == true)
                                    {

                                        PdfPCell headerSpace2 = new PdfPCell(new Phrase("", TableFont));
                                        headerSpace2.Colspan = 9;
                                        headerSpace2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        headerSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(headerSpace2);



                                        PdfPCell pdfNonMeals = new PdfPCell(new Phrase("NON-CACFP ADULT MEALS", TableFont));
                                        pdfNonMeals.Colspan = 3;
                                        pdfNonMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfNonMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfNonMeals);

                                        if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            MealsDefaultSettings("MO");
                                            PdfPCell pdfNonMealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                            pdfNonMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealMonday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfNonMealMonday = new PdfPCell(new Phrase("", TableFont));
                                            pdfNonMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealMonday);

                                        }
                                        if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            MealsDefaultSettings("TU");
                                            PdfPCell pdfNonMealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                            pdfNonMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealTuesday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfNonMealTuesday = new PdfPCell(new Phrase("", TableFont));
                                            pdfNonMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealTuesday);
                                        }
                                        if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            MealsDefaultSettings("WE");
                                            PdfPCell pdfNonMealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                            pdfNonMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealWed);
                                        }
                                        else
                                        {
                                            PdfPCell pdfNonMealWed = new PdfPCell(new Phrase("", TableFont));
                                            pdfNonMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealWed);
                                        }
                                        if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            MealsDefaultSettings("TH");
                                            PdfPCell pdfNonMealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                            pdfNonMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealThus);
                                        }
                                        else
                                        {
                                            PdfPCell pdfNonMealThus = new PdfPCell(new Phrase("", TableFont));
                                            pdfNonMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealThus);
                                        }
                                        if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            MealsDefaultSettings("FR");
                                            PdfPCell pdfNonMealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                            pdfNonMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealFri);
                                        }
                                        else
                                        {

                                            PdfPCell pdfNonMealFri = new PdfPCell(new Phrase("", TableFont));
                                            pdfNonMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfNonMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfNonMealFri);
                                        }

                                        PdfPCell pdfNonMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfNonMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfNonMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfNonMealTotal);

                                        PdfPCell headerSpace3 = new PdfPCell(new Phrase("", TableFont));
                                        headerSpace3.Colspan = 9;
                                        headerSpace3.HorizontalAlignment = Element.ALIGN_CENTER;
                                        headerSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(headerSpace3);


                                        PdfPCell pdfMeals = new PdfPCell(new Phrase("CACFP ADULT MEAL CNTS", TableFont));
                                        pdfMeals.Colspan = 3;
                                        pdfMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfMeals);



                                        if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfMealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                            pdfMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealMonday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfMealMonday = new PdfPCell(new Phrase("", TableFont));
                                            pdfMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealMonday);
                                        }
                                        if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfMealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                            pdfMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealTuesday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfMealTuesday = new PdfPCell(new Phrase("", TableFont));
                                            pdfMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealTuesday);
                                        }
                                        if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfMealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                            pdfMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealWed);
                                        }
                                        else
                                        {
                                            PdfPCell pdfMealWed = new PdfPCell(new Phrase("", TableFont));
                                            pdfMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealWed);
                                        }
                                        if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfMealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                            pdfMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealThus);
                                        }
                                        else
                                        {
                                            PdfPCell pdfMealThus = new PdfPCell(new Phrase("", TableFont));
                                            pdfMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealThus);
                                        }
                                        if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfMealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                            pdfMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealFri);
                                        }
                                        else
                                        {
                                            PdfPCell pdfMealFri = new PdfPCell(new Phrase("", TableFont));
                                            pdfMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfMealFri);
                                        }


                                        PdfPCell pdfMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfMealTotal);



                                        PdfPCell pdfHrLine1 = new PdfPCell(new Phrase("", TableFont));
                                        pdfHrLine1.Colspan = 9;
                                        pdfHrLine1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfHrLine1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(pdfHrLine1);



                                        PdfPCell pdfSpaMeals = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpaMeals.Colspan = 3;
                                        pdfSpaMeals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpaMeals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpaMeals);




                                        if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfSpaMealMonday = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                            pdfSpaMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealMonday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpaMealMonday = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpaMealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealMonday);
                                        }
                                        if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfSpaMealTuesday = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                            pdfSpaMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealTuesday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpaMealTuesday = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpaMealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealTuesday);
                                        }
                                        if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpaMealWed = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                            pdfSpaMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealWed);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpaMealWed = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpaMealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealWed);
                                        }
                                        if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpaMealThus = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                            pdfSpaMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealThus);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpaMealThus = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpaMealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealThus);
                                        }
                                        if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpaMealFri = new PdfPCell(new Phrase("P__E__U__N__", TableFont));
                                            pdfSpaMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealFri);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpaMealFri = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpaMealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpaMealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpaMealFri);
                                        }


                                        PdfPCell pdfSpaMealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpaMealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpaMealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpaMealTotal);


                                        PdfPCell pdfSpace1 = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpace1.Colspan = 9;
                                        pdfSpace1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpace1);



                                        PdfPCell pdfSpa1Meals = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa1Meals.Colspan = 3;
                                        pdfSpa1Meals.HorizontalAlignment = Element.ALIGN_CENTER;
                                        pdfSpa1Meals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(pdfSpa1Meals);


                                        if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfSpa1MealMonday = new PdfPCell(new Phrase(strMondayMeal, TableFont));
                                            pdfSpa1MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealMonday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpa1MealMonday = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpa1MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealMonday);
                                        }
                                        if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {
                                            PdfPCell pdfSpa1MealTuesday = new PdfPCell(new Phrase(strTuesMeal, TableFont));
                                            pdfSpa1MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealTuesday);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpa1MealTuesday = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpa1MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealTuesday);
                                        }
                                        if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpa1MealWed = new PdfPCell(new Phrase(strWedMeal, TableFont));
                                            pdfSpa1MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealWed);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpa1MealWed = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpa1MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealWed);
                                        }
                                        if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpa1MealThus = new PdfPCell(new Phrase(strThusMeal, TableFont));
                                            pdfSpa1MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealThus);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpa1MealThus = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpa1MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealThus);
                                        }
                                        if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                        {

                                            PdfPCell pdfSpa1MealFri = new PdfPCell(new Phrase(strFriMeal, TableFont));
                                            pdfSpa1MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealFri);
                                        }
                                        else
                                        {
                                            PdfPCell pdfSpa1MealFri = new PdfPCell(new Phrase("", TableFont));
                                            pdfSpa1MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfSpa1MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfSpa1MealFri);
                                        }


                                        PdfPCell pdfSpa1MealTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa1MealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa1MealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa1MealTotal);
                                    }
                                    else
                                    {
                                        MealsDefaultSettings("MO");
                                        MealsDefaultSettings("TU");
                                        MealsDefaultSettings("WE");
                                        MealsDefaultSettings("TH");
                                        MealsDefaultSettings("FR");
                                    }
                                    PdfPCell pdfHrLine2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfHrLine2.Colspan = 9;
                                    pdfHrLine2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfHrLine2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table.AddCell(pdfHrLine2);


                                    PdfPCell pdfSpa2Meals = new PdfPCell(new Phrase("", TableFont));
                                    pdfSpa2Meals.Colspan = 3;
                                    pdfSpa2Meals.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfSpa2Meals.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(pdfSpa2Meals);


                                    if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        strMealDetails = strMondayMeal.Replace('_', ' ');
                                        PdfPCell pdfSpa2MealMonday = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                        pdfSpa2MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealMonday);
                                    }
                                    else
                                    {
                                        PdfPCell pdfSpa2MealMonday = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa2MealMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealMonday);
                                    }
                                    if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        strMealDetails = strTuesMeal.Replace('_', ' ');
                                        PdfPCell pdfSpa2MealTuesday = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                        pdfSpa2MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealTuesday);
                                    }
                                    else
                                    {
                                        PdfPCell pdfSpa2MealTuesday = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa2MealTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealTuesday);
                                    }
                                    if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        strMealDetails = strWedMeal.Replace('_', ' ');
                                        PdfPCell pdfSpa2MealWed = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                        pdfSpa2MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealWed);
                                    }
                                    else
                                    {
                                        PdfPCell pdfSpa2MealWed = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa2MealWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa2MealWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa2MealWed);
                                    }
                                    if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {

                                        strMealDetails = strThusMeal.Replace('_', ' ');
                                        PdfPCell pdfSpa3MealThus = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                        pdfSpa3MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa3MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa3MealThus);
                                    }
                                    else
                                    {
                                        PdfPCell pdfSpa3MealThus = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa3MealThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa3MealThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa3MealThus);
                                    }
                                    if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {

                                        strMealDetails = strFriMeal.Replace('_', ' ');
                                        PdfPCell pdfSpa4MealFri = new PdfPCell(new Phrase(strMealDetails + "ATT", TableFont));
                                        pdfSpa4MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa4MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa4MealFri);
                                    }
                                    else
                                    {
                                        PdfPCell pdfSpa4MealFri = new PdfPCell(new Phrase("", TableFont));
                                        pdfSpa4MealFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfSpa4MealFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(pdfSpa4MealFri);
                                    }


                                    PdfPCell pdfSpa5MealTotal = new PdfPCell(new Phrase("", TableFont));
                                    pdfSpa5MealTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpa5MealTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(pdfSpa5MealTotal);

                                    int intNumberenrlcount = 0;
                                    foreach (CaseEnrlEntity EnrlDetails in caseEnrlList)
                                    {

                                        ENRL_Asof_Entity enrlasofapp = ENRLDateAllList.Find(u => u.App == EnrlDetails.App && u.Status == "E" && u.FundHie.Trim() == EnrlDetails.FundHie.Trim());

                                        if (enrlasofapp != null)
                                        {
                                            intNumberenrlcount = intNumberenrlcount + 1;
                                            PdfPCell pdfchildDetails = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(EnrlDetails.Snp_F_Name, EnrlDetails.Snp_M_Name, EnrlDetails.Snp_L_Name, Member_NameFormat), TableFont));
                                            pdfchildDetails.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfchildDetails.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfchildDetails);


                                            PdfPCell pdfFundingDesc = new PdfPCell(new Phrase(EnrlDetails.FundHie, TableFont));
                                            pdfFundingDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfFundingDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfFundingDesc);

                                            PdfPCell pdfMealDesc = new PdfPCell(new Phrase(DisplayMealElig(EnrlDetails.MST_MealElig.Trim()), TableFont));
                                            pdfMealDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfMealDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(pdfMealDesc);



                                            if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                            {
                                                PdfPCell pdfMondayDesc = new PdfPCell(new Phrase(GetDateStatus(dtMonday.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "MO"), TableFont));
                                                pdfMondayDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfMondayDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfMondayDesc);
                                            }
                                            else
                                            {
                                                PdfPCell pdfMondayDesc = new PdfPCell(new Phrase("", TableFont));
                                                pdfMondayDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfMondayDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfMondayDesc);
                                            }
                                            if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                            {

                                                PdfPCell pdfTuesDesc = new PdfPCell(new Phrase(GetDateStatus(dtTues.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "TU"), TableFont));
                                                pdfTuesDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfTuesDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfTuesDesc);
                                            }
                                            else
                                            {
                                                PdfPCell pdfTuesDesc = new PdfPCell(new Phrase("", TableFont));
                                                pdfTuesDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfTuesDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfTuesDesc);
                                            }
                                            if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                            {

                                                PdfPCell pdfWedDesc = new PdfPCell(new Phrase(GetDateStatus(dtWed.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "WE"), TableFont));
                                                pdfWedDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfWedDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfWedDesc);

                                            }
                                            else
                                            {
                                                PdfPCell pdfWedDesc = new PdfPCell(new Phrase("", TableFont));
                                                pdfWedDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfWedDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfWedDesc);
                                            }
                                            if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                            {
                                                PdfPCell pdfThurDesc = new PdfPCell(new Phrase(GetDateStatus(dtThurs.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "TH"), TableFont));
                                                pdfThurDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfThurDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfThurDesc);
                                            }
                                            else
                                            {
                                                PdfPCell pdfThurDesc = new PdfPCell(new Phrase("", TableFont));
                                                pdfThurDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfThurDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfThurDesc);
                                            }
                                            if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                            {
                                                PdfPCell pdfFridesc = new PdfPCell(new Phrase(GetDateStatus(dtFri.Date, item.SiteNUMBER, item.SiteROOM, item.SiteAM_PM, EnrlDetails.FundHie, "FR"), TableFont));
                                                pdfFridesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfFridesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfFridesc);
                                            }
                                            else
                                            {
                                                PdfPCell pdfFridesc = new PdfPCell(new Phrase("", TableFont));
                                                pdfFridesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfFridesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                table.AddCell(pdfFridesc);
                                            }


                                            PdfPCell pdfTotalDesc = new PdfPCell(new Phrase("______", TableFont));
                                            pdfTotalDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                                            pdfTotalDesc.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(pdfTotalDesc);

                                            if (chkPrint.Checked == true)
                                            {
                                                PdfPCell pdfChildNewLine = new PdfPCell(new Phrase("", TableFont));
                                                pdfChildNewLine.HorizontalAlignment = Element.ALIGN_CENTER;
                                                pdfChildNewLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                pdfChildNewLine.Colspan = 9;
                                                pdfChildNewLine.FixedHeight = 10f;
                                                table.AddCell(pdfChildNewLine);
                                            }

                                        }
                                    }

                                    for (int i = 0; i < 3; i++)
                                    {


                                        PdfPCell pdfChildNameSpace = new PdfPCell(new Phrase("____________________", TableFont));
                                        pdfChildNameSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildNameSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        pdfChildNameSpace.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildNameSpace);

                                        PdfPCell pdfFundingSpace = new PdfPCell(new Phrase("_______", TableFont));
                                        pdfFundingSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfFundingSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        pdfFundingSpace.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfFundingSpace);

                                        PdfPCell pdfFreeSpace = new PdfPCell(new Phrase("", TableFont));
                                        pdfFreeSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfFreeSpace.FixedHeight = 10f;
                                        pdfFreeSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfFreeSpace);


                                        PdfPCell pdfChildSpaceMonday = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceMonday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceMonday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceMonday.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceMonday);

                                        PdfPCell pdfChildSpaceTuesday = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceTuesday.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceTuesday.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceTuesday.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceTuesday);

                                        PdfPCell pdfChildSpaceWed = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceWed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceWed.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceWed.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceWed);

                                        PdfPCell pdfChildSpaceThus = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceThus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceThus.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceThus.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceThus);

                                        PdfPCell pdfChildSpaceFri = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceFri.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceFri.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceFri.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceFri);

                                        PdfPCell pdfChildSpaceTotal = new PdfPCell(new Phrase("", TableFont));
                                        pdfChildSpaceTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                        pdfChildSpaceTotal.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                        pdfChildSpaceTotal.FixedHeight = 10f;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfChildSpaceTotal);

                                    }

                                    PdfPCell pdfTotalChildSpace = new PdfPCell(new Phrase("Total Children Attending Daily:", TableFont));
                                    pdfTotalChildSpace.Colspan = 2;
                                    pdfTotalChildSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfTotalChildSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildSpace);



                                    PdfPCell pdfTotalFreeSpace = new PdfPCell(new Phrase("", TableFont));
                                    pdfTotalFreeSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfTotalFreeSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalFreeSpace);


                                    if (dtMonday.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell pdfTotalChildMonday = new PdfPCell(new Phrase("Mon _____", TableFont));
                                        pdfTotalChildMonday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildMonday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildMonday);
                                    }
                                    else
                                    {
                                        PdfPCell pdfTotalChildMonday = new PdfPCell(new Phrase("", TableFont));
                                        pdfTotalChildMonday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildMonday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildMonday);
                                    }
                                    if (dtTues.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell pdfTotalChildTuesday = new PdfPCell(new Phrase("Tue _____", TableFont));
                                        pdfTotalChildTuesday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildTuesday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildTuesday);
                                    }
                                    else
                                    {
                                        PdfPCell pdfTotalChildTuesday = new PdfPCell(new Phrase("", TableFont));
                                        pdfTotalChildTuesday.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildTuesday.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildTuesday);
                                    }
                                    if (dtWed.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell pdfTotalChildWed = new PdfPCell(new Phrase("Wed _____", TableFont));
                                        pdfTotalChildWed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildWed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildWed);
                                    }
                                    else
                                    {
                                        PdfPCell pdfTotalChildWed = new PdfPCell(new Phrase("", TableFont));
                                        pdfTotalChildWed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildWed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildWed);
                                    }
                                    if (dtThurs.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell pdfTotalChildThus = new PdfPCell(new Phrase("Thu _____", TableFont));
                                        pdfTotalChildThus.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildThus.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildThus);
                                    }
                                    else
                                    {
                                        PdfPCell pdfTotalChildThus = new PdfPCell(new Phrase("", TableFont));
                                        pdfTotalChildThus.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildThus.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildThus);
                                    }
                                    if (dtFri.Date != Convert.ToDateTime("01/01/1990").Date)
                                    {
                                        PdfPCell pdfTotalChildFri = new PdfPCell(new Phrase("Fri _____", TableFont));
                                        pdfTotalChildFri.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildFri.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildFri);
                                    }
                                    else
                                    {
                                        PdfPCell pdfTotalChildFri = new PdfPCell(new Phrase("", TableFont));
                                        pdfTotalChildFri.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        pdfTotalChildFri.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                        table.AddCell(pdfTotalChildFri);
                                    }

                                    PdfPCell pdfTotalChildTotal = new PdfPCell(new Phrase("", TableFont));
                                    pdfTotalChildTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfTotalChildTotal.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfTotalChildTotal);


                                    PdfPCell pdfBottomSpace = new PdfPCell(new Phrase("", TableFont));
                                    pdfBottomSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfBottomSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfBottomSpace.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfBottomSpace);

                                    PdfPCell pdfBottomSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfBottomSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfBottomSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfBottomSpace1.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfBottomSpace1);


                                    PdfPCell pdfReasons = new PdfPCell(new Phrase("Reason For Absence Codes: ", TableFont));
                                    pdfReasons.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfReasons.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfReasons.Colspan = 5;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfReasons);

                                    PdfPCell pdfchildcount = new PdfPCell(new Phrase("Number children enrolled: " + intNumberenrlcount, TableFont));// caseEnrlList.Count
                                    pdfchildcount.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfchildcount.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfchildcount.Colspan = 4;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfchildcount);

                                    PdfPCell pdfReasonsdesc = new PdfPCell(new Phrase(strReasonCodes, TableFont));
                                    pdfReasonsdesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfReasonsdesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    pdfReasonsdesc.Colspan = 9;
                                    //Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    table.AddCell(pdfReasonsdesc);

                                    table.FooterRows = 3;

                                    //PdfPCell pdfBottomSpace2 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfBottomSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfBottomSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //pdfBottomSpace2.Colspan = 9;
                                    ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //table.AddCell(pdfBottomSpace2);

                                    //PdfPCell pdfBottomSpace3 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfBottomSpace3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfBottomSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //pdfBottomSpace3.Colspan = 9;
                                    ////Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    //table.AddCell(pdfBottomSpace3);

                                    if (table.Rows.Count > 0)
                                    {
                                        document.Add(table);
                                        table.DeleteBodyRows();
                                        document.NewPage();
                                    }

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

            /* string Agy = "Agency : All"; string Det = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
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

            ////iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\capTAIN-systems-header-5-25-12.gif"));
            ////logo.BackgroundColor = BaseColor.WHITE;
            ////logo.ScalePercent(20f);

            //////iTextSharp.text.Image logo1 = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\CapSystems_Title.bmp"));
            //////logo1.ScalePercent(50F);
            //////logo1.SetAbsolutePosition(70, 779);
            ////document.Add(logo);
            ////document.Add(logo1);

            //cb.BeginText();


            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            ////cb.SetRGBColorFill(00, 00, 00);      //ss
            //cb.ShowTextAligned(100, "Program Name:", 30, 725, 0);
            //cb.ShowTextAligned(100, Privileges.Program + " - " + Privileges.PrivilegeName, 110, 725, 0);
            //cb.ShowTextAligned(100, "Run By :" + Privileges.UserID, 30, 705, 0);
            //cb.ShowTextAligned(100, "Module Desc :" + GetModuleDesc(), 30, 685, 0);
            //cb.ShowTextAligned(100, "Started : " + DateTime.Now.ToString(), 30, 665, 0);
            //cb.ShowTextAligned(100, "Report  Selection Criterion", 40, 635, 0);
            //string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            //cb.ShowTextAligned(100, "Hierarchy : " + Agency + "-" + Dept + "-" + Prog + "   " + Header_year, 120, 610, 0);


            string Report = Privileges.PrivilegeName;

            //PdfPCell R1 = new PdfPCell(new Phrase("Report Type : " + Report, TableFont));
            //R1.HorizontalAlignment = Element.ALIGN_LEFT;
            //R1.Colspan = 2;
            //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R1);

            //cb.ShowTextAligned(100, "Report Type : " + Report, 120, 590, 0);

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Children Age" /*+ txtFrom.Text + "  To  " + txtTo.Text*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase("From: " + txtFrom.Text + "   To: " + txtTo.Text, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            //cb.ShowTextAligned(100, "Children Age :  From " + txtFrom.Text + "  To  " + txtTo.Text, 120, 570, 0);



            //cb.ShowTextAligned(100, "Active/Inactive : " + ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString().Trim(), 120, 550, 0);


            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Active/Inactive"/* + ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString().Trim()*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString().Trim(), TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            string strStartDate = string.Empty;
            if (dtSecond.Checked)
                strStartDate = ", " + LookupDataAccess.Getdate(dtSecond.Value.ToShortDateString());
            if (dtthird.Checked)
                strStartDate = strStartDate + ", " + LookupDataAccess.Getdate(dtthird.Value.ToShortDateString());
            if (dtFourth.Checked)
                strStartDate = strStartDate + ", " + LookupDataAccess.Getdate(dtFourth.Value.ToShortDateString());
            if (dtFifth.Checked)
                strStartDate = strStartDate + ", " + LookupDataAccess.Getdate(dtFifth.Value.ToShortDateString());

            //cb.ShowTextAligned(100, "Week Starting Dates : " + dtstartDate.Value.ToShortDateString() + strStartDate, 120, 530, 0);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Week Starting Dates" /*+ dtstartDate.Value.ToShortDateString() + strStartDate*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dtstartDate.Value.ToShortDateString()) + strStartDate, TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            //cb.ShowTextAligned(100, "Base Ages On : " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), 120, 510, 0);


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

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Base Ages On" /*+ (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text)*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase((rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell R7 = new PdfPCell(new Phrase("  " + "Print Adult meal Info?" /*+ (rdoYes.Checked == true ? "Yes" : "No")*/, TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            Headertable.AddCell(R7);

            PdfPCell R77 = new PdfPCell(new Phrase((rdoYes.Checked == true ? "Yes" : "No"), TableFont));
            R77.HorizontalAlignment = Element.ALIGN_LEFT;
            R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R77.PaddingBottom = 5;
            Headertable.AddCell(R77);

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Print a blank line between each Child" /*+ (chkPrint.Checked == true ? "Yes" : "No")*/, TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            Headertable.AddCell(R8);

            PdfPCell R88 = new PdfPCell(new Phrase((chkPrint.Checked == true ? "Yes" : "No"), TableFont));
            R88.HorizontalAlignment = Element.ALIGN_LEFT;
            R88.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R88.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R88.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R88.PaddingBottom = 5;
            Headertable.AddCell(R88);

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
                    switch (Dw)
                    {
                        case "MO":
                            strStatus = strMondayAtt;
                            break;
                        case "TU":
                            strStatus = strTuesAtt;
                            break;
                        case "WE":
                            strStatus = strWedAtt;
                            break;
                        case "TH":
                            strStatus = strThusAtt;
                            break;
                        case "FR":
                            strStatus = strFriAtt;
                            break;
                    }

                }
                else
                    strStatus = childattmsitefundstatus.AttM_AgyStatus;
            }
            //else
            //{
            //    ChildATTMSEntity childattmfundstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.Attm_Fund == Fund);
            //    if (childattmfundstatus != null)
            //    {
            //        if (childattmfundstatus.ATTMS_STATUS == "O")
            //        {
            //            switch (Dw)
            //            {
            //                case "MO":
            //                    strStatus = strMondayAtt;
            //                    break;
            //                case "TU":
            //                    strStatus = strTuesAtt;
            //                    break;
            //                case "WE":
            //                    strStatus = strWedAtt;
            //                    break;
            //                case "TH":
            //                    strStatus = strThusAtt;
            //                    break;
            //                case "FR":
            //                    strStatus = strFriAtt;
            //                    break;
            //            }

            //        }
            //        else
            //            strStatus = childattmfundstatus.AttM_AgyStatus;
            //    }
            //    else
            //    {
            //        ChildATTMSEntity childattmstatus = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.AttM_Site == Site && u.AttM_Room == Room && u.AttM_AMPM == Ampm);
            //        if (childattmstatus != null)
            //        {
            //            if (childattmstatus.ATTMS_STATUS == "O")
            //            {
            //                switch (Dw)
            //                {
            //                    case "MO":
            //                        strStatus = strMondayAtt;
            //                        break;
            //                    case "TU":
            //                        strStatus = strTuesAtt;
            //                        break;
            //                    case "WE":
            //                        strStatus = strWedAtt;
            //                        break;
            //                    case "TH":
            //                        strStatus = strThusAtt;
            //                        break;
            //                    case "FR":
            //                        strStatus = strFriAtt;
            //                        break;
            //                }

            //            }
            //            else
            //                strStatus = childattmstatus.AttM_AgyStatus;
            //        }
            //        else
            //        {
            //            ChildATTMSEntity childattmstatus2 = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.AttM_Site == Site && u.AttM_Room.Trim() == "");
            //            if (childattmstatus2 != null)
            //            {
            //                if (childattmstatus2.ATTMS_STATUS == "O")
            //                {
            //                    switch (Dw)
            //                    {
            //                        case "MO":
            //                            strStatus = strMondayAtt;
            //                            break;
            //                        case "TU":
            //                            strStatus = strTuesAtt;
            //                            break;
            //                        case "WE":
            //                            strStatus = strWedAtt;
            //                            break;
            //                        case "TH":
            //                            strStatus = strThusAtt;
            //                            break;
            //                        case "FR":
            //                            strStatus = strFriAtt;
            //                            break;
            //                    }
            //                }
            //                else
            //                    strStatus = childattmstatus2.AttM_AgyStatus;
            //            }
            //            else
            //            {
            //                ChildATTMSEntity childattmstatus3 = propchldAttmsEntityList.Find(u => Convert.ToInt32(u.ATTMS_DAY) == Day && Convert.ToInt32(u.ATTM_MONTH) == Month && u.ATTM_CALENDER_YEAR == CalYear && u.AttM_Site == "" && u.AttM_Room.Trim() == "");
            //                if (childattmstatus3 != null)
            //                {
            //                    if (childattmstatus3.ATTMS_STATUS == "O")
            //                    {
            //                        switch (Dw)
            //                        {
            //                            case "MO":
            //                                strStatus = strMondayAtt;
            //                                break;
            //                            case "TU":
            //                                strStatus = strTuesAtt;
            //                                break;
            //                            case "WE":
            //                                strStatus = strWedAtt;
            //                                break;
            //                            case "TH":
            //                                strStatus = strThusAtt;
            //                                break;
            //                            case "FR":
            //                                strStatus = strFriAtt;
            //                                break;
            //                        }
            //                    }
            //                    else
            //                        strStatus = childattmstatus3.AttM_AgyStatus;
            //                }
            //            }
            //        }
            //    }
            //}
            return strStatus;

        }

        public string strMealSpaceDetails = string.Empty;
        string strAttDetails = string.Empty;
        public string strMealDetails = string.Empty;
        string strMondayMeal = string.Empty;
        string strTuesMeal = string.Empty;
        string strWedMeal = string.Empty;
        string strThusMeal = string.Empty;
        string strFriMeal = string.Empty;
        string strMondayAtt = string.Empty;
        string strTuesAtt = string.Empty;
        string strWedAtt = string.Empty;
        string strThusAtt = string.Empty;
        string strFriAtt = string.Empty;
        private void MealsDefaultSettings(string strday)
        {
            strMealSpaceDetails = string.Empty;
            strMealDetails = string.Empty;
            strAttDetails = string.Empty;
            switch (strday)
            {

                case "MO":
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(0, 1) == "Y" ? "B__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(7, 1) == "Y" ? "A__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(14, 1) == "Y" ? "L__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(21, 1) == "Y" ? "P__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(28, 1) == "Y" ? "S__" : "   ");

                    strAttDetails = strAttDetails + (propMealTypes.Substring(0, 1) == "Y" ? "__" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(7, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(14, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(21, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(28, 1) == "Y" ? " __" : "  ");
                    strMondayAtt = strAttDetails + " ___";
                    strMondayMeal = strMealSpaceDetails;
                    break;
                case "TU":
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(1, 1) == "Y" ? "B__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(8, 1) == "Y" ? "A__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(15, 1) == "Y" ? "L__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(22, 1) == "Y" ? "P__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(29, 1) == "Y" ? "S__" : "   ");

                    strAttDetails = strAttDetails + (propMealTypes.Substring(1, 1) == "Y" ? "__" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(8, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(15, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(22, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(29, 1) == "Y" ? " __" : "  ");
                    strTuesAtt = strAttDetails + " ___";
                    strTuesMeal = strMealSpaceDetails;
                    break;
                case "WE":
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(2, 1) == "Y" ? "B__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(9, 1) == "Y" ? "A__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(16, 1) == "Y" ? "L__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(23, 1) == "Y" ? "P__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(30, 1) == "Y" ? "S__" : "   ");

                    strAttDetails = strAttDetails + (propMealTypes.Substring(2, 1) == "Y" ? "__" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(9, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(16, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(23, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(30, 1) == "Y" ? " __" : "  ");
                    strWedAtt = strAttDetails + " ___";
                    strWedMeal = strMealSpaceDetails;
                    break;
                case "TH":
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(3, 1) == "Y" ? "B__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(10, 1) == "Y" ? "A__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(17, 1) == "Y" ? "L__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(24, 1) == "Y" ? "P__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(31, 1) == "Y" ? "S__" : "   ");

                    strAttDetails = strAttDetails + (propMealTypes.Substring(3, 1) == "Y" ? "__" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(10, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(17, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(24, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(31, 1) == "Y" ? " __" : "  ");
                    strThusAtt = strAttDetails + " ___";
                    strThusMeal = strMealSpaceDetails;
                    break;
                case "FR":
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(4, 1) == "Y" ? "B__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(11, 1) == "Y" ? "A__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(18, 1) == "Y" ? "L__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(25, 1) == "Y" ? "P__" : "   ");
                    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(32, 1) == "Y" ? "S__" : "   ");

                    strAttDetails = strAttDetails + (propMealTypes.Substring(4, 1) == "Y" ? "__" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(11, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(18, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(25, 1) == "Y" ? " __" : "  ");
                    strAttDetails = strAttDetails + (propMealTypes.Substring(32, 1) == "Y" ? " __" : "  ");
                    strFriAtt = strAttDetails + " ___";
                    strFriMeal = strMealSpaceDetails;
                    break;
                //case "ST":
                //     strMealSpaceDetails = strMealSpaceDetails +  (propMealTypes.Substring(5, 1) == "Y" ? "B__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(12, 1) == "Y" ? "A__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(19, 1) == "Y" ? "L__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(26, 1) == "Y" ? "P__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(33, 1) == "Y" ? "S__" : "   ");                   
                //    break;
                //case "SU":
                //    strMealSpaceDetails = strMealSpaceDetails +  (propMealTypes.Substring(6, 1) == "Y" ? "B__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(13, 1) == "Y" ? "A__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(20, 1) == "Y" ? "L__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(27, 1) == "Y" ? "P__" : "   ");
                //    strMealSpaceDetails = strMealSpaceDetails + (propMealTypes.Substring(34, 1) == "Y" ? "S__" : "   ");                   
                //    break;

            }

        }


        #endregion
        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(txtFrom, null);
            _errorProvider.SetError(txtTo, null);
            _errorProvider.SetError(rdoMultipleSites, null);

           /* if (txtFrom.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Children From Age".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtFrom, null);
            }

            if (txtTo.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Children To Age".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTo, null);
            }*/

            if (dtstartDate.Checked == false)
            {
                _errorProvider.SetError(dtstartDate, "Week Starting Date is Required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtstartDate, null);
            }

            if (dtstartDate.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtstartDate.Text))
                {
                    _errorProvider.SetError(dtstartDate, "Week Starting Date is Required");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtstartDate, null);
                }
            }

            if (dtstartDate.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtstartDate.Text))
                {
                    _errorProvider.SetError(dtstartDate, "Week Starting Date is Required");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtstartDate, null);
                }
            }

            if (dtSecond.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtSecond.Text))
                {
                    _errorProvider.SetError(dtSecond, "Please Enter Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtSecond, null);
                }
            }

            if (dtthird.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtthird.Text))
                {
                    _errorProvider.SetError(dtthird, "Please Enter Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtthird, null);
                }
            }

            if (dtFourth.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtFourth.Text))
                {
                    _errorProvider.SetError(dtFourth, "Please Enter Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtFourth, null);
                }
            }

            if (dtFifth.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtFifth.Text))
                {
                    _errorProvider.SetError(dtFifth, "Please Enter Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtFifth, null);
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

            if (string.IsNullOrEmpty(txtFrom.Text.Trim()) || string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                if (string.IsNullOrWhiteSpace(txtFrom.Text) || string.IsNullOrWhiteSpace(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, "Please enter the missing From/To Children Age");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }
            }
            else
            {
                if (Convert.ToInt32(txtFrom.Text) > -1 && Convert.ToInt32(txtTo.Text) > -1)
                {
                    if (Convert.ToInt32(txtFrom.Text) > Convert.ToInt32(txtTo.Text))
                    {
                        _errorProvider.SetError(txtTo, "'To Age' should be Greater than or Equal to 'From Age'");
                        isValid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(txtTo, "Values of Children Age cannot be Negative");
                    isValid = false;
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
            _errorProvider.SetError(dtstartDate, null);
            _errorProvider.SetError(txtFrom, null);
            _errorProvider.SetError(txtTo, null);
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
            string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strBaseAge = rdoTodayDate.Checked == true ? "T" : "K";
            string strPrintAdult = rdoYes.Checked == true ? "Y" : "N";
            string strPrintBlank = chkPrint.Checked == true ? "Y" : "N";
            string startDt = string.Empty;
            string secondDt = string.Empty;
            string thirdDt = string.Empty;
            string FourthDt = string.Empty;
            string FifthDt = string.Empty;
            if (dtstartDate.Checked == true)
                startDt = dtstartDate.Value.ToShortDateString();
            if (dtSecond.Checked == true)
                secondDt = dtSecond.Value.ToShortDateString();
            if (dtthird.Checked == true)
                thirdDt = dtthird.Value.ToShortDateString();
            if (dtFourth.Checked == true)
                FourthDt = dtFourth.Value.ToShortDateString();
            if (dtFifth.Checked == true)
                FifthDt = dtFifth.Value.ToShortDateString();

            string strsiteRoomNames = string.Empty;
            if (rdoMultipleSites.Checked == true)
            {
                foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                {
                    if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                    strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                }
            }

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            //   PROG = \"" + Current_Hierarchy_DB.Substring(6, 2) + "\" 
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + Program_Year + "\" ChildAgeFrom = \"" + txtFrom.Text + "\" ChildAgeTo = \"" + txtTo.Text + "\" Active = \"" + Active + "\" FirstDt = \"" + startDt + "\" SecondDt = \"" + secondDt + "\" ThirdDt = \"" + thirdDt + "\" FourthDt = \"" + FourthDt + "\" FifthDt = \"" + FifthDt + "\" Site = \"" + strSite + "\" BaseAges = \"" + strBaseAge + "\" PrintAdult = \"" + strPrintAdult + "\" PrintBlank = \"" + strPrintBlank + "\" SiteNames = \"" + strsiteRoomNames + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());
                Program_Year = dr["YEAR"].ToString();
                CommonFunctions.SetComboBoxValue(cmbActive, dr["Active"].ToString());
                txtFrom.Text = dr["ChildAgeFrom"].ToString();
                txtTo.Text = dr["ChildAgeTo"].ToString();

                if (dr["Site"].ToString() == "A")
                    rdoAllSite.Checked = true;
                else
                {
                    rdoMultipleSites.Checked = true;
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
                    Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                    propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000");
                    Sel_REFS_List.Clear();
                    string strSites = dr["SiteNames"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                            Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }

                if (dr["BaseAges"].ToString() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["PrintAdult"].ToString() == "Y")
                    rdoYes.Checked = true;
                else
                    rdoNo.Checked = true;

                if (dr["PrintBlank"].ToString() == "Y")
                    chkPrint.Checked = true;
                else
                    chkPrint.Checked = false;

                if (dr["FirstDt"].ToString() != "")
                {
                    dtstartDate.Value = Convert.ToDateTime(dr["FirstDt"]);
                    dtstartDate.Checked = true;
                }
                else
                    dtstartDate.Checked = false;

                //if ()
                //{
                    if (dr["SecondDt"].ToString() != "")
                    {
                        dtSecond.Value = Convert.ToDateTime(dr["SecondDt"]);
                        dtSecond.Checked = true;
                    }
                    else
                        dtSecond.Checked = false;
                    if (dr["ThirdDt"].ToString() != "")
                    {
                        dtthird.Value = Convert.ToDateTime(dr["ThirdDt"]);
                        dtthird.Checked = true;
                    }
                    else
                        dtthird.Checked = false;

                    if (dr["FourthDt"].ToString() != "")
                    {
                        dtFourth.Value = Convert.ToDateTime(dr["FourthDt"]);
                        dtFourth.Checked = true;
                    }
                    else
                        dtFourth.Checked = false;

                    if (dr["FifthDt"].ToString() != "")
                    {
                        dtFifth.Value = Convert.ToDateTime(dr["FifthDt"]);
                        dtFifth.Checked = true;
                    }
                    else
                        dtFifth.Checked = false;
                //}
                Sel_REFS_List = Sel_REFS_List;

            }
        }

        private string DisplayMealElig(string strMealElig)
        {
            string strMeal = "Free";
            switch (strMealElig)
            {
                case "1":
                    strMeal = "Free";
                    break;
                case "2":
                    strMeal = "Reduced";
                    break;
                case "3":
                    strMeal = "Paid";
                    break;
            }
            return strMeal;
        }

        private void dtstartDate_LostFocus(object sender, EventArgs e)
        {
            if (dtstartDate.Checked == true)
            {
                _errorProvider.SetError(dtstartDate, null);
                string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtstartDate.Value.ToShortDateString());
                if (strMsg == "Y")
                {
                    MessageBox.Show("Automatically enter the Next 4 Weeks", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                }
                else
                {
                    AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                    dtstartDate.Checked = false;
                    dtSecond.Checked = false;
                    dtthird.Checked = false;
                    dtFourth.Checked = false;
                    dtFifth.Checked = false;
                }
            }
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
           // Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;
            dtSecond.LostFocus -= new EventHandler(dtSecond_LostFocus);
            dtthird.LostFocus -= new EventHandler(dtthird_LostFocus);
            dtFourth.LostFocus -= new EventHandler(dtFourth_LostFocus);
            dtFifth.LostFocus -= new EventHandler(dtFifth_LostFocus);
            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {


                    dtSecond.Value = dtstartDate.Value.AddDays(7);
                    dtSecond.Checked = true;
                    string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtSecond.Value.ToShortDateString());
                    if (strMsg != "Y")
                    {
                    AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                        dtSecond.Checked = false;
                    }
                    else
                    {
                        dtSecond.Enabled = false;
                        dtthird.Value = dtstartDate.Value.AddDays(14);
                        dtthird.Checked = true;
                        strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtthird.Value.ToShortDateString());
                        if (strMsg != "Y")
                        {
                        AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                            dtthird.Checked = false;
                        }
                        else
                        {
                            dtthird.Enabled = false;
                            dtFourth.Value = dtstartDate.Value.AddDays(21);
                            dtFourth.Checked = true;
                            strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtFourth.Value.ToShortDateString());
                            if (strMsg != "Y")
                            {
                                AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                                dtFourth.Checked = false;
                            }
                            else
                            {
                                dtFourth.Enabled = false;
                                dtFifth.Value = dtstartDate.Value.AddDays(28);
                                dtFifth.Checked = true;
                                strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtFifth.Value.ToShortDateString());
                                if (strMsg != "Y")
                                {
                                AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                                    dtFifth.Checked = false;
                                }
                                else
                                {
                                    dtFifth.Enabled = false;
                                }
                            }

                        }

                    }




                }
                else
                {
                    dtSecond.Enabled = true;
                    dtthird.Enabled = true;
                    dtFourth.Enabled = true;
                    dtFifth.Enabled = true;
                    dtSecond.Checked = false;
                    dtthird.Checked = false;
                    dtFourth.Checked = false;
                    dtFifth.Checked = false;

                }
            //}
            dtSecond.LostFocus += new EventHandler(dtSecond_LostFocus);
            dtthird.LostFocus += new EventHandler(dtthird_LostFocus);
            dtFourth.LostFocus += new EventHandler(dtFourth_LostFocus);
            dtFifth.LostFocus += new EventHandler(dtFifth_LostFocus);
        }



        void dtFifth_LostFocus(object sender, EventArgs e)
        {
            if (dtFifth.Checked == true)
            {
                string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtFifth.Value.ToShortDateString());
                if (strMsg != "Y")
                {
                    AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                    dtFifth.Checked = false;
                }
            }
        }

        private void dtSecond_LostFocus(object sender, EventArgs e)
        {
            if (dtSecond.Checked == true)
            {
                string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtSecond.Value.ToShortDateString());
                if (strMsg != "Y")
                {
                    AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                    dtSecond.Checked = false;
                }

            }

        }

        private void dtthird_LostFocus(object sender, EventArgs e)
        {
            if (dtthird.Checked == true)
            {
                string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtthird.Value.ToShortDateString());
                if (strMsg != "Y")
                {
                    AlertBox.Show(strMsg,MessageBoxIcon.Warning);
                    dtthird.Checked = false;
                }
            }
        }

        private void dtFourth_LostFocus(object sender, EventArgs e)
        {
            if (dtFourth.Checked == true)
            {
                string strMsg = _model.ChldAttnData.CheckHss2108StartingDate(Agency, Dept, Prog, Program_Year, dtFourth.Value.ToShortDateString());
                if (strMsg != "Y")
                {
                    AlertBox.Show(strMsg, MessageBoxIcon.Warning);
                    dtFourth.Checked = false;
                }
            }
        }

        private void dtFifth_Click(object sender, EventArgs e)
        {

        }



        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            //string SelRef_Name = null;
            string Sql_MSg = string.Empty;
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
            }
        }

        private void txtTo_Leave(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtTo, null);
            if ((txtFrom.Text.Trim() != string.Empty) && (txtTo.Text.Trim() != string.Empty))
            {
                if (Convert.ToInt32(txtFrom.Text) > Convert.ToInt32(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, "'To Age' should be Greater than or Equal to 'From Age'");
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }


            }
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdoMultipleSites, null);
            Sel_REFS_List.Clear();
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

        private void HSSB2108ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
        }

        private void rdoMultipleSites_LostFocus(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdoMultipleSites, null);
        }

        private void rdoMultipleSites_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2108");
        }



    }
}