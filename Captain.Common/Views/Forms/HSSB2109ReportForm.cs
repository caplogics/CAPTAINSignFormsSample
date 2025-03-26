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
using System.IO;
using iTextSharp.text;
using Captain.Common.Exceptions;
using DevExpress.XtraEditors;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2109ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public HSSB2109ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
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
            txtApplicant.Validator = TextBoxValidation.IntegerValidator;
            txtNoofConsecDays.Validator = TextBoxValidation.IntegerValidator;
            List<CommonEntity> listSequence = _model.lookupDataAccess.GetHssb2108FormActiveDetails();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            propAppsentReasons = _model.lookupDataAccess.GetAgyTabRecordsByCode("00107");
            Member_NameFormat = BaseForm.BaseHierarchyCnFormat.ToString();
            CAseWorkerr_NameFormat = BaseForm.BaseHierarchyCwFormat.ToString();
            
            foreach (CommonEntity item in listSequence)
            {
                cmbActive.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbActive.SelectedIndex = 2;

            foreach (CommonEntity itemReason in propAppsentReasons)
            {
                cmbAbsentReason.Items.Add(new Captain.Common.Utilities.ListItem(itemReason.Desc, itemReason.Code));
            }
            cmbAbsentReason.Items.Insert(0, (new Captain.Common.Utilities.ListItem("  ", "0")));
            cmbAbsentReason.SelectedIndex = 0;
        }


        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<RankCatgEntity> propRankscategory { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }
        public string propMealTypes { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<CommonEntity> propAppsentReasons { get; set; }
        public List<ChldAttnEntity> propChldAttnList { get; set; }
        public List<ChldAttnEntity> propChldAttnCountList { get; set; }
     //   public List<ChldAttnEntity> propchldattnsummary { get; set; }
        

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

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {


            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                _errorProvider.SetError(dtForm, null);
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
                //**dtForm.Checked = false;
                //**dtTodate.Checked = false;
            }

        }


        
        string strApplicant = string.Empty;
        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {


            if (ValidateForm())
            {
                string strsiteRoomNames = string.Empty;
                if (rdoMultipleSites.Checked == true)
                {
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
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
                                                if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                                                strsiteRoomNames += casesite.SiteNUMBER + casesite.SiteROOM + casesite.SiteAM_PM;
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


                string strFundingCodes = string.Empty;
                if (rdoFundingSelect.Checked == true)
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                else
                {
                    strFundingCodes = "A";
                }

                if (rdoSelectApplicant.Checked == true)
                {
                    strApplicant = txtApplicant.Text;
                }
                else
                    strApplicant = string.Empty;

                string strSequence = string.Empty;
                if (rdoReportChildName.Checked)
                    strSequence = "Child Name";
                if (rdoApplicant.Checked)
                    strSequence = "Applicant";
                if (rdoClass.Checked)
                    strSequence = "Class";




                propCaseEnrlList = _model.EnrollData.GetEnrollDetails2109(Agency, Dept, Prog, Program_Year, (rdoTodayDate.Checked == true ? "T" : "K"), dtTodate.Value.ToShortDateString(), strsiteRoomNames, strFundingCodes, strApplicant, strSequence, (rdoConsecutive.Checked == true ? txtNoofConsecDays.Text : "0"), dtForm.Value.ToShortDateString());

               // propchldattnsummary = _model.ChldAttnData.GetChldAttn2109FundSummary(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strsiteRoomNames, strFundingCodes);
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
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
                StringBuilder strMstApplUpdate = new StringBuilder();
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


                    document.NewPage();
                    Y_Pos = 795;


                    PdfPTable hssb2109_Table = new PdfPTable(8);
                    hssb2109_Table.TotalWidth = 550f;
                    hssb2109_Table.WidthPercentage = 100;
                    hssb2109_Table.LockedWidth = true;
                    float[] widths = new float[] { 30f, 35f, 35f, 35f, 55f, 35f, 35f, 40f };
                    hssb2109_Table.SetWidths(widths);
                    hssb2109_Table.HorizontalAlignment = Element.ALIGN_CENTER;

                    List<CommonEntity> commonapplicant = new List<CommonEntity>();

                    List<ChldAttnEntity> chldattnSummaryAll = new List<ChldAttnEntity>();

                    List<CaseEnrlEntity> caseEnrlList;
                    if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }
                    else
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }
                    if (rdoChdRepeater.Checked == true)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Chld_Repeater.ToUpper() == "Y");//"TRUE");
                    if (rdoChdNonRepeater.Checked == true)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Chld_Repeater.ToUpper() == "N");//"FALSE");
                    if (rdoIncomeEligible.Checked == true)
                        caseEnrlList = caseEnrlList.FindAll(u => u.MST_Classification == "99");
                    if (rdoOverCome.Checked == true)
                        caseEnrlList = caseEnrlList.FindAll(u => u.MST_Classification == "98");
                    if (rdoConsecutive.Checked)
                        caseEnrlList = caseEnrlList.FindAll(u => u.ConsecutiveDays == "Y");

                    if (caseEnrlList.Count > 0)
                    {


                        PdfPCell Header_1 = new PdfPCell(new Phrase("Applicant", TblFontBold));
                        Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header_1.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header_1);

                        PdfPCell Header = new PdfPCell(new Phrase("Name", TblFontBold));
                        Header.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header.Colspan = 2;
                        Header.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header);

                        PdfPCell HeaderTop1 = new PdfPCell(new Phrase("Age", TblFontBold));
                        HeaderTop1.HorizontalAlignment = Element.ALIGN_CENTER;
                        HeaderTop1.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(HeaderTop1);

                        PdfPCell Header1 = new PdfPCell(new Phrase("Parent / Guardian", TblFontBold));
                        Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header1.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header1);

                        PdfPCell Header2 = new PdfPCell(new Phrase("Telephone", TblFontBold));
                        Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header2.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header2);

                        PdfPCell Header3 = new PdfPCell(new Phrase("Class", TblFontBold));
                        Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header3.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header3);

                        PdfPCell Header4 = new PdfPCell(new Phrase("Funding", TblFontBold));
                        Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header4.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header4);
                        string strAbsentReasonCode = string.Empty;
                        if (rdoReason.Checked == true && rdoAbsentSelect.Checked == true)
                        {
                            if (((Captain.Common.Utilities.ListItem)cmbAbsentReason.SelectedItem).Value.ToString().Trim() != "0")
                                strAbsentReasonCode = ((Captain.Common.Utilities.ListItem)cmbAbsentReason.SelectedItem).Value.ToString().Trim();
                        }
                        
                        propChldAttnList = _model.ChldAttnData.GetChldAttnBetweenDatehssb2109(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strApplicant, strAbsentReasonCode,string.Empty);
                        propChldAttnCountList = _model.ChldAttnData.GetChldAttnBetweenDatehssb2109Count(Agency, Dept, Prog, Program_Year, dtForm.Value.ToShortDateString(), dtTodate.Value.ToShortDateString(), strApplicant, strAbsentReasonCode);
                        foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                        {
                            List<ChldAttnEntity> chldattnList1 = propChldAttnCountList.FindAll(u => (u.SITE + u.ROOM + u.AMPM == enrlrow.Site + enrlrow.Room + enrlrow.AMPM) && (u.FUNDING_SOURCE == enrlrow.FundHie) && (u.APP_NO == enrlrow.App) && (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog));
                            if (chldattnList1.Count > 0)
                            {
                                bool boolPrintApplicant = true;
                                List<ChldAttnEntity> chldattnList = propChldAttnList.FindAll(u => (u.SITE + u.ROOM + u.AMPM == enrlrow.Site + enrlrow.Room + enrlrow.AMPM) && (u.FUNDING_SOURCE == enrlrow.FundHie) && u.PA == "A" && (u.APP_NO == enrlrow.App) && (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog));
                                chldattnList = chldattnList.OrderBy(u => Convert.ToDateTime(u.DATE)).ToList(); //propChldAttnList.FindAll(u => (u.SITE + u.ROOM + u.AMPM == enrlrow.Site + enrlrow.Room + enrlrow.AMPM) && (u.FUNDING_SOURCE == enrlrow.FundHie) && u.PA == "A" && (u.APP_NO == enrlrow.App) && (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog));

                                if (rdoReason.Checked == true && rdoAbsentSelect.Checked == true && ((Captain.Common.Utilities.ListItem)cmbAbsentReason.SelectedItem).Value.ToString().Trim() != "0")
                                {
                                    if (chldattnList.Count > 0)
                                    {
                                        boolPrintApplicant = true;
                                    }
                                    else
                                        boolPrintApplicant = false;
                                }
                                if (boolPrintApplicant)
                                {
                                   

                                    PdfPCell pdfAppldata = new PdfPCell(new Phrase(enrlrow.App, TableFont));
                                    pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfAppldata.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    hssb2109_Table.AddCell(pdfAppldata);


                                    PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name,Member_NameFormat), TableFont));
                                    pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfChildName.Colspan = 2;
                                    pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfChildName);

                                    PdfPCell pdfAge = new PdfPCell(new Phrase(enrlrow.Snp_AgeMonth, TableFont));
                                    pdfAge.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfAge.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfAge);

                                    PdfPCell pdfGardian = new PdfPCell(new Phrase(enrlrow.Snp_Gardian, TableFont));
                                    pdfGardian.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfGardian.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfGardian);

                                    PdfPCell pdfTelephone = new PdfPCell(new Phrase(enrlrow.Snp_TelePhone, TableFont));
                                    pdfTelephone.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfTelephone.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfTelephone);

                                    PdfPCell pdfClass = new PdfPCell(new Phrase(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, TableFont));
                                    pdfClass.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfClass);

                                    PdfPCell pdfFundingDesc = new PdfPCell(new Phrase((propfundingSource.Find(u => u.Code == enrlrow.FundHie).Desc.ToString()), TableFont));
                                    pdfFundingDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfFundingDesc.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    hssb2109_Table.AddCell(pdfFundingDesc);

                                    PdfPCell pdfNewSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfNewSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfNewSpace1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    pdfNewSpace1.Colspan = 8;
                                    pdfNewSpace1.FixedHeight = 4f;
                                    hssb2109_Table.AddCell(pdfNewSpace1);

                                    int intchldattnCount = 0;
                                    string intPresentDays = "0";
                                    string intApsentDays = "0";
                                    string intExcusedDays = "0";
                                    string intDaysOther = "0";
                                    string intBreakfast = "0";
                                    string intLunch = "0";
                                    string intAmSnaks = "0";
                                    string intPmSnaks = "0";
                                    string intSub = "0";
                                    //  int intChldattnassign = 0;

                                    //List<ChldAttnEntity> chldattnList = propChldAttnList.FindAll(u => (u.SITE + u.ROOM + u.AMPM == enrlrow.Site + enrlrow.Room + enrlrow.AMPM) && (u.FUNDING_SOURCE == enrlrow.FundHie) && u.PA == "A" && (u.APP_NO == enrlrow.App) && (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog));
                                    if (chldattnList.Count > 0)
                                    {
                                        intchldattnCount = chldattnList.Count;
                                       

                                        for (int intChldattnassign = 0; intChldattnassign < intchldattnCount; intChldattnassign++)
                                        {


                                            if (intchldattnCount > intChldattnassign)
                                            {
                                                PdfPCell pdfData1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldattnList[intChldattnassign].DATE) + "  " + (propAppsentReasons.Find(u => u.Code == chldattnList[intChldattnassign].REASON)).Desc.ToString(), TableFont));
                                                pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfData1.Colspan = 2;
                                                pdfData1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                hssb2109_Table.AddCell(pdfData1);
                                                intChldattnassign++;
                                            }
                                            else
                                            {

                                                break;
                                            }
                                            if (intchldattnCount > intChldattnassign)
                                            {
                                                PdfPCell pdfData2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldattnList[intChldattnassign].DATE) + "  " + (propAppsentReasons.Find(u => u.Code == chldattnList[intChldattnassign].REASON)).Desc.ToString(), TableFont));
                                                pdfData2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfData2.Colspan = 2;
                                                pdfData2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2109_Table.AddCell(pdfData2);
                                                intChldattnassign++;
                                            }
                                            else
                                            {
                                                PdfPCell pdfBorde2 = new PdfPCell(new Phrase("", TableFont));
                                                pdfBorde2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfBorde2.Colspan = 6;
                                                pdfBorde2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                hssb2109_Table.AddCell(pdfBorde2);
                                                break;
                                            }
                                            if (intchldattnCount > intChldattnassign)
                                            {
                                                PdfPCell pdfData3 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldattnList[intChldattnassign].DATE) + "  " + (propAppsentReasons.Find(u => u.Code == chldattnList[intChldattnassign].REASON)).Desc.ToString(), TableFont));
                                                pdfData3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfData3.Colspan = 2;
                                                pdfData3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2109_Table.AddCell(pdfData3);
                                                intChldattnassign++;
                                            }
                                            else
                                            {
                                                PdfPCell pdfBorde2 = new PdfPCell(new Phrase("", TableFont));
                                                pdfBorde2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfBorde2.Colspan = 4;
                                                pdfBorde2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                hssb2109_Table.AddCell(pdfBorde2);
                                                break;
                                            }
                                            if (intchldattnCount > intChldattnassign)
                                            {
                                                PdfPCell pdfData4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldattnList[intChldattnassign].DATE) + "  " + (propAppsentReasons.Find(u => u.Code == chldattnList[intChldattnassign].REASON)).Desc.ToString(), TableFont));
                                                pdfData4.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfData4.Colspan = 2;
                                                pdfData4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                hssb2109_Table.AddCell(pdfData4);
                                                // intChldattnassign++;
                                            }
                                            else
                                            {
                                                PdfPCell pdfBorde2 = new PdfPCell(new Phrase("", TableFont));
                                                pdfBorde2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                pdfBorde2.Colspan = 2;
                                                pdfBorde2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                hssb2109_Table.AddCell(pdfBorde2);
                                                break;
                                            }

                                        }

                                    }

                                    //List<ChldAttnEntity> chldattnList1 = propChldAttnCountList.FindAll(u => (u.SITE + u.ROOM + u.AMPM == enrlrow.Site + enrlrow.Room + enrlrow.AMPM) && (u.FUNDING_SOURCE == enrlrow.FundHie) && (u.APP_NO == enrlrow.App) && (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog));
                                    if (chldattnList1.Count > 0)
                                    {
                                        intPresentDays = chldattnList1[0].PresentDays;
                                        intApsentDays = chldattnList1[0].AbsentDays;
                                        intExcusedDays = chldattnList1[0].LegalDays;
                                        //intDaysOther = chldattnList[0].Day;
                                        intBreakfast = chldattnList1[0].BreakFast;
                                        intLunch = chldattnList1[0].Lunch;
                                        intAmSnaks = chldattnList1[0].AMSnacks;
                                        intPmSnaks = chldattnList1[0].PMSnacks;

                                    }

                                    commonapplicant.Add(new CommonEntity(enrlrow.FundHie, "1"));

                                    chldattnSummaryAll.Add(new ChldAttnEntity(enrlrow.FundHie.ToString(),intPresentDays.ToString(),intApsentDays.ToString(),intExcusedDays.ToString(),intBreakfast.ToString(),intLunch.ToString(),intAmSnaks.ToString(),intPmSnaks.ToString(),"0"));

                                    PdfPCell pdfNewLine2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfNewLine2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfNewLine2.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    pdfNewLine2.Colspan = 8;
                                    pdfNewLine2.FixedHeight = 6;
                                    hssb2109_Table.AddCell(pdfNewLine2);

                                    PdfPCell pdfttlData1 = new PdfPCell(new Phrase("Totals:  ", TableFont));
                                    pdfttlData1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    pdfttlData1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    hssb2109_Table.AddCell(pdfttlData1);

                                    PdfPCell pdfttlData2 = new PdfPCell(new Phrase(intPresentDays + "  Days Present", TableFont));
                                    pdfttlData2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlData2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfttlData2);

                                    PdfPCell pdfttlData3 = new PdfPCell(new Phrase(intApsentDays + "  Days Absent", TableFont));
                                    pdfttlData3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlData3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfttlData3);

                                    PdfPCell pdfttlData4 = new PdfPCell(new Phrase(intExcusedDays + "  Excused Absense", TableFont));
                                    pdfttlData4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlData4.Colspan = 2;
                                    pdfttlData4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2109_Table.AddCell(pdfttlData4);

                                    PdfPCell pdfttlData5 = new PdfPCell(new Phrase(intDaysOther + "  Days Other", TableFont));
                                    pdfttlData5.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfttlData5.Colspan = 3;
                                    pdfttlData5.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    hssb2109_Table.AddCell(pdfttlData5);

                                    if (rdoMealDataYes.Checked == true)
                                    {

                                        string strMealsData = string.Empty;
                                        if (intBreakfast != "0")
                                            strMealsData = strMealsData + intBreakfast + "  Breakfasts    ";
                                        else
                                            strMealsData = strMealsData + "            ";

                                        if (intLunch != "0")
                                            strMealsData = strMealsData + intLunch + "  Lunches  ";
                                        else
                                            strMealsData = strMealsData + "            ";

                                        if (intAmSnaks != "0")
                                            strMealsData = strMealsData + intAmSnaks + "  AM Snacks ";
                                        else
                                            strMealsData = strMealsData + "            ";

                                        if (intPmSnaks != "0")
                                            strMealsData = strMealsData + intPmSnaks + "  PM Snacks ";
                                        else
                                            strMealsData = strMealsData + "            ";

                                        if (intSub != "0")
                                            strMealsData = strMealsData + intSub + "  Sub ";
                                        else
                                            strMealsData = strMealsData + "            ";



                                        if (strMealsData.Trim() != string.Empty)
                                        {
                                            PdfPCell pdfttlData6 = new PdfPCell(new Phrase("Free Meals:  ", TableFont));
                                            pdfttlData6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            pdfttlData6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            hssb2109_Table.AddCell(pdfttlData6);


                                            PdfPCell pdfttlData7 = new PdfPCell(new Phrase(strMealsData, TableFont));
                                            pdfttlData7.HorizontalAlignment = Element.ALIGN_LEFT;
                                            pdfttlData7.Colspan = 7;
                                            pdfttlData7.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            hssb2109_Table.AddCell(pdfttlData7);
                                        }

                                    }
                                    PdfPCell pdfNewLine1 = new PdfPCell(new Phrase("", TableFont));
                                    pdfNewLine1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfNewLine1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    pdfNewLine1.Colspan = 8;
                                    pdfNewLine1.FixedHeight = 10;
                                    hssb2109_Table.AddCell(pdfNewLine1);

                                }
                            }
                        }
                    }
                    if (hssb2109_Table.Rows.Count > 0)
                    {
                        document.Add(hssb2109_Table);
                        hssb2109_Table.DeleteBodyRows();
                        document.NewPage();


                        // ******** Summary Report *************//

                        if (caseEnrlList.Count>0)
                        {

                          

                           int intApplicantsTot = 0;
                            int intdaypresentTot = 0;
                            int intdayabsentTot = 0;
                            int intExcusedabsentTot = 0;
                          
                            int intBreakfastTot = 0;
                            int intLunchTot = 0;
                            int intAmsnaksTot = 0;
                            int intPmsnaksTot = 0;

                            decimal intApplicants = 0;
                            decimal intdaypresent = 0;
                            decimal intdayabsent = 0;
                            decimal intExcusedabsent = 0;

                            decimal intBreakfast = 0;
                            decimal intLunch = 0;
                            decimal intAmsnaks = 0;
                            decimal intPmsnaks = 0;
                          
                            PdfPTable hssb2111_SummaryTable = new PdfPTable(11);
                            hssb2111_SummaryTable.TotalWidth = 550f;
                            hssb2111_SummaryTable.WidthPercentage = 100;
                            hssb2111_SummaryTable.LockedWidth = true;
                            float[] summarywidths = new float[] { 25f, 25f, 25f, 25f, 25f, 35f, 20f,20f, 20f, 20f, 20f };
                            hssb2111_SummaryTable.SetWidths(summarywidths);
                            hssb2111_SummaryTable.HorizontalAlignment = Element.ALIGN_CENTER;


                            //PdfPCell SumHeader = new PdfPCell(new Phrase("", TableFont));
                            //SumHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                            //SumHeader.Colspan = 2;
                            //SumHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            //hssb2111_SummaryTable.AddCell(SumHeader);



                            PdfPCell SumSiteHeader = new PdfPCell(new Phrase("FUNDING SOURCE", TableFont));
                            SumSiteHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumSiteHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            SumSiteHeader.Colspan = 2;
                            hssb2111_SummaryTable.AddCell(SumSiteHeader);

                            PdfPCell SumDailyAttHeader = new PdfPCell(new Phrase("Clients", TableFont));
                            SumDailyAttHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumDailyAttHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttHeader);

                            PdfPCell SumDailyAttFree = new PdfPCell(new Phrase("Days Present", TableFont));
                            SumDailyAttFree.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumDailyAttFree.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttFree);

                            PdfPCell SumReduced = new PdfPCell(new Phrase("Days Absent", TableFont));
                            SumReduced.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumReduced.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumReduced);

                            PdfPCell SumOverIncome = new PdfPCell(new Phrase("Excused Absense", TableFont));
                            SumOverIncome.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumOverIncome.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumOverIncome);

                            PdfPCell SumTotal = new PdfPCell(new Phrase("Days Other", TableFont));
                            SumTotal.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumTotal);

                            PdfPCell SumBreakfast = new PdfPCell(new Phrase("Breakfasts", TableFont));
                            SumBreakfast.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumBreakfast.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumBreakfast);

                            PdfPCell SumLunch = new PdfPCell(new Phrase("Lunches", TableFont));
                            SumLunch.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumLunch.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumLunch);

                            PdfPCell SumSupper = new PdfPCell(new Phrase("Am Snacks", TableFont));
                            SumSupper.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumSupper.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSupper);

                            PdfPCell SumSupleme = new PdfPCell(new Phrase("Pm Snacks", TableFont));
                            SumSupleme.HorizontalAlignment = Element.ALIGN_CENTER;
                            SumSupleme.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + +iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSupleme);

                            List<CommonEntity> lookUpfundingResource = SelFundingList;
                            if(rdoFundingAll.Checked)
                               lookUpfundingResource   = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

                            foreach (CommonEntity FundingCode in lookUpfundingResource)
                            {

                                List<ChldAttnEntity> chldattnSummary = chldattnSummaryAll.FindAll(u => u.FUNDING_SOURCE == FundingCode.Code);
                              
                                intApplicants = 0;
                                intdaypresent = 0;
                                intdayabsent = 0;
                                intExcusedabsent = 0;

                                intBreakfast = 0;
                                intLunch = 0;
                                intAmsnaks = 0;
                                intPmsnaks = 0;


                                if (commonapplicant.Count > 0)
                                {
                                    List<CommonEntity> caseEnrlappcount = commonapplicant.FindAll(u => u.Code == FundingCode.Code);
                                    intApplicants = caseEnrlappcount.Count;
                                }

                                if (chldattnSummary.Count > 0)
                                { 
                                  
                                    intdaypresent =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.PresentDays));
                                    intdayabsent =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.AbsentDays));
                                    intExcusedabsent =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.LegalDays));
                                    intBreakfast =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.BreakFast));
                                    intLunch =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.Lunch));
                                    intAmsnaks =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.AMSnacks));
                                    intPmsnaks =  chldattnSummary.Sum(u=>Convert.ToDecimal(u.PMSnacks));
                                }


                                   PdfPCell SumSiteData = new PdfPCell(new Phrase((propfundingSource.Find(u => u.Code == FundingCode.Code).Desc.ToString()), TableFont));
                                    SumSiteData.HorizontalAlignment = Element.ALIGN_LEFT;
                                    SumSiteData.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    SumSiteData.Colspan = 2;
                                    hssb2111_SummaryTable.AddCell(SumSiteData);


                                    PdfPCell SumDailyAttFreeData = new PdfPCell(new Phrase(intApplicants.ToString(), TableFont));
                                    SumDailyAttFreeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumDailyAttFreeData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumDailyAttFreeData);

                                    PdfPCell SumReducedData = new PdfPCell(new Phrase(intdaypresent.ToString(), TableFont));
                                    SumReducedData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumReducedData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumReducedData);

                                    PdfPCell SumOverIncomeData = new PdfPCell(new Phrase(intdayabsent.ToString(), TableFont));
                                    SumOverIncomeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumOverIncomeData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumOverIncomeData);

                                    PdfPCell SumTotalData = new PdfPCell(new Phrase(intExcusedabsent.ToString(), TableFont));
                                    SumTotalData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumTotalData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumTotalData);

                                    PdfPCell SumdaysotherData = new PdfPCell(new Phrase("0", TableFont));
                                    SumdaysotherData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumdaysotherData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumdaysotherData);

                                    PdfPCell SumBreakfastData = new PdfPCell(new Phrase(intBreakfast.ToString(), TableFont));
                                    SumBreakfastData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumBreakfastData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumBreakfastData);

                                    PdfPCell SumLunchData = new PdfPCell(new Phrase(intLunch.ToString(), TableFont));
                                    SumLunchData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumLunchData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumLunchData);

                                    PdfPCell SumSupperData = new PdfPCell(new Phrase(intAmsnaks.ToString(), TableFont));
                                    SumSupperData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumSupperData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumSupperData);

                                    PdfPCell SumSuplemeData = new PdfPCell(new Phrase(intPmsnaks.ToString(), TableFont));
                                    SumSuplemeData.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    SumSuplemeData.Border =  iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    hssb2111_SummaryTable.AddCell(SumSuplemeData);

                                    intApplicantsTot = Convert.ToInt32(intApplicants) + intApplicantsTot;
                                    intdaypresentTot = Convert.ToInt32(intdaypresent) + intdaypresentTot;
                                    intdayabsentTot = Convert.ToInt32(intdayabsent) + intdayabsentTot;
                                    intExcusedabsentTot = Convert.ToInt32(intExcusedabsent) + intExcusedabsentTot;
                                    intBreakfastTot = Convert.ToInt32(intBreakfast) + intBreakfastTot;
                                    intLunchTot = Convert.ToInt32(intLunch) + intLunchTot;
                                    intAmsnaksTot = Convert.ToInt32(intAmsnaks) + intAmsnaksTot;
                                    intPmsnaksTot = Convert.ToInt32(intPmsnaks) + intPmsnaksTot;
                                    
                                
                            }


                            PdfPCell SumSpace1 = new PdfPCell(new Phrase("", TableFont));
                            SumSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumSpace1.Border =  iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            SumSpace1.Colspan = 11;
                            hssb2111_SummaryTable.AddCell(SumSpace1);


                            PdfPCell SumSiteTotal = new PdfPCell(new Phrase("Total", TableFont));
                            SumSiteTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                            SumSiteTotal.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                            SumSiteTotal.Colspan = 2;
                            hssb2111_SummaryTable.AddCell(SumSiteTotal);

                            PdfPCell SumDailyAttTot = new PdfPCell(new Phrase(intApplicantsTot.ToString(), TableFont));
                            SumDailyAttTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttTot);

                            PdfPCell SumDailyAttFreeTot = new PdfPCell(new Phrase(intdaypresentTot.ToString(), TableFont));
                            SumDailyAttFreeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumDailyAttFreeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumDailyAttFreeTot);

                            PdfPCell SumReducedTot = new PdfPCell(new Phrase(intdayabsentTot.ToString(), TableFont));
                            SumReducedTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumReducedTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumReducedTot);

                            PdfPCell SumOverIncomeTot = new PdfPCell(new Phrase(intExcusedabsentTot.ToString(), TableFont));
                            SumOverIncomeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumOverIncomeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumOverIncomeTot);

                            PdfPCell SumTotalTot = new PdfPCell(new Phrase("0", TableFont));
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

                            PdfPCell SumSupperTot = new PdfPCell(new Phrase(intAmsnaksTot.ToString(), TableFont));
                            SumSupperTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSupperTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSupperTot);

                            PdfPCell SumSuplemeTot = new PdfPCell(new Phrase(intPmsnaksTot.ToString(), TableFont));
                            SumSuplemeTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            SumSuplemeTot.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                            hssb2111_SummaryTable.AddCell(SumSuplemeTot);


                          

                            if (hssb2111_SummaryTable.Rows.Count > 0)
                            {
                                document.Add(hssb2111_SummaryTable);
                                //hssb2111_SummaryTable.DeleteBodyRows();
                                //document.NewPage();
                            }
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

            /* BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
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

            /*PdfPCell row2 = new PdfPCell(new Phrase("Run By :" + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
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

            //string Report = Privileges.PrivilegeName;

            //PdfPCell R1 = new PdfPCell(new Phrase("Report Name : " + Report, TableFont));
            //R1.HorizontalAlignment = Element.ALIGN_LEFT;
            //R1.Colspan = 2;
            //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R1);

            string strReportType = string.Empty;
            if (rdoSummary.Checked)
                strReportType = "Summary Report";
            else if (rdoConsecutive.Checked)
                strReportType = txtNoofConsecDays.Text + " Consecutive Days Absent";
            else if (rdoSelectApplicant.Checked)
                strReportType = "Selected Applicant: " + txtApplicant.Text;


            PdfPCell R2 = new PdfPCell(new Phrase("  "+  "Report Type" /*+ strReportType*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(strReportType, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            string strChildName = string.Empty;
            string strChildReapeter = string.Empty;
            string strIncomeEligible = string.Empty;
            if (rdoIncomeEligible.Checked)
                strIncomeEligible = "Income Eligible Types ";
            else if (rdoOverCome.Checked)
                strIncomeEligible = "Over - Income Types ";
            else if (rdoIncomeAll.Checked)
                strIncomeEligible = "All Income Types ";

            if (rdoReportChildName.Checked)
                strChildName = "Child Name ";
            else if (rdoApplicant.Checked)
                strChildName = "Applicant ";
            else if (rdoClass.Checked)
                strChildName = "Class ";
            else if (rdoReason.Checked)
                strChildName = "Reason ";

            if (rdoChdRepeater.Checked)
                strChildReapeter = "Repeaters";
            else if (rdoChdNonRepeater.Checked)
                strChildReapeter = "Non - Repeaters";
            else if (rdoChdBoth.Checked)
                strChildReapeter = "Both Repeaters & Non - Repeaters";

            string strAbsentReasonCode = ((Captain.Common.Utilities.ListItem)cmbAbsentReason.SelectedItem).Text.ToString().Trim();

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Active/Inactive" /*+ ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString()*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString(), TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Report Sequence" /*+ strChildName*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(strChildName, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            PdfPCell RReson = new PdfPCell(new Phrase("   " + "Absent Reason" /*+ (rdoAbsentAll.Checked == true ? "All" : strAbsentReasonCode)*/, TableFont));
            RReson.HorizontalAlignment = Element.ALIGN_LEFT;
            RReson.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RReson.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RReson.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RReson.PaddingBottom = 5;
            Headertable.AddCell(RReson);

            PdfPCell RReson1 = new PdfPCell(new Phrase((rdoAbsentAll.Checked == true ? "All" : strAbsentReasonCode), TableFont));
            RReson1.HorizontalAlignment = Element.ALIGN_LEFT;
            RReson1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RReson1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RReson1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RReson1.PaddingBottom = 5;
            Headertable.AddCell(RReson1);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Report From Date" /*+ dtForm.Text.Trim() + "  To Date: " + dtTodate.Text.Trim()*/, TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            Headertable.AddCell(R6);

            PdfPCell R66 = new PdfPCell(new Phrase(dtForm.Text.Trim() + "   To Date: " + dtTodate.Text.Trim(), TableFont));
            R66.HorizontalAlignment = Element.ALIGN_LEFT;
            R66.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R66.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R66.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R66.PaddingBottom = 5;
            Headertable.AddCell(R66);

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Children Age" /*+ txtFrom.Text + " to " + txtTo.Text + ", Base on " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text)*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase("From: " + txtFrom.Text + "   To: " + txtTo.Text + ", Base on: " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell RChildren = new PdfPCell(new Phrase("  " + "Children" + strChildReapeter, TableFont));
            RChildren.HorizontalAlignment = Element.ALIGN_LEFT;
            RChildren.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RChildren.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RChildren.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RChildren.PaddingBottom = 5;
            Headertable.AddCell(RChildren);

            PdfPCell RChildren1 = new PdfPCell(new Phrase(strChildReapeter, TableFont));
            RChildren1.HorizontalAlignment = Element.ALIGN_LEFT;
            RChildren1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RChildren1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RChildren1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RChildren1.PaddingBottom = 5;
            Headertable.AddCell(RChildren1);

            PdfPCell R7 = new PdfPCell(new Phrase("", TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            Headertable.AddCell(R7);

            PdfPCell R77 = new PdfPCell(new Phrase(strIncomeEligible, TableFont));
            R77.HorizontalAlignment = Element.ALIGN_LEFT;
            R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R77.PaddingBottom = 5;
            Headertable.AddCell(R77);

            //PdfPCell R8 = new PdfPCell(new Phrase("Site :" + (rdoAllSite.Checked == true ? rdoAllSite.Text : rdoMultipleSites.Text), TableFont));
            //R8.HorizontalAlignment = Element.ALIGN_LEFT;
            //R8.Colspan = 2;
            //R8.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R8);

            //PdfPCell R9 = new PdfPCell(new Phrase("Funding Sources :" + (rdoFundingAll.Checked == true ? "All" : "Selected"), TableFont));
            //R9.HorizontalAlignment = Element.ALIGN_LEFT;
            //R9.Colspan = 2;
            //R9.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R9);


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

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Site", TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            Headertable.AddCell(R8);

            PdfPCell R88 = new PdfPCell(new Phrase(Site, TableFont));
            R88.HorizontalAlignment = Element.ALIGN_LEFT;
            R88.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R88.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R88.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R88.PaddingBottom = 5;
            Headertable.AddCell(R88);

            string strFundingSource = string.Empty;
            if (rdoFundingAll.Checked == true) strFundingSource = "All Funds";
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

            PdfPCell R9 = new PdfPCell(new Phrase("  " + "Funding Sources", TableFont));
            R9.HorizontalAlignment = Element.ALIGN_LEFT;
            R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R9.PaddingBottom = 5;
            Headertable.AddCell(R9);

            PdfPCell R99 = new PdfPCell(new Phrase(strFundingSource, TableFont));
            R99.HorizontalAlignment = Element.ALIGN_LEFT;
            R99.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R99.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R99.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R99.PaddingBottom = 5;
            Headertable.AddCell(R99);

            PdfPCell R10 = new PdfPCell(new Phrase("  " + "Include Meal Data" /*+ (rdoMealDataYes.Checked == true ? "Yes" : "No")*/, TableFont));
            R10.HorizontalAlignment = Element.ALIGN_LEFT;
            R10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R10.PaddingBottom = 5;
            Headertable.AddCell(R10);

            PdfPCell R101 = new PdfPCell(new Phrase((rdoMealDataYes.Checked == true ? "Yes" : "No"), TableFont));
            R101.HorizontalAlignment = Element.ALIGN_LEFT;
            R101.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R101.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R101.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R101.PaddingBottom = 5;
            Headertable.AddCell(R101);

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
            _errorProvider.SetError(btnBrowse, null);
            _errorProvider.SetError(txtNoofConsecDays, null);
            _errorProvider.SetError(rdoMultipleSites, null);
            _errorProvider.SetError(rdoFundingSelect, null);
            _errorProvider.SetError(dtForm, null);
            _errorProvider.SetError(dtTodate, null);
            _errorProvider.SetError(txtTo, null);

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

            /*if (dtForm.Checked == false)
            {
                _errorProvider.SetError(dtForm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReportFormdt.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtForm, null);
            }
            if (dtTodate.Checked == false)
            {
                _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtTodate, null);
            }

            if ((dtForm.Checked == true) && (dtTodate.Checked == true))
            {

                if (dtForm.Value.Date > dtTodate.Value.Date)
                {
                    _errorProvider.SetError(dtTodate, "From Date Greater than To date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }

            }
            */

            if (rdoMultipleSites.Checked == true)
            {
                if (Sel_REFS_List.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleSites, "Please Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleSites, null);
                }
            }

            if (rdoSelectApplicant.Checked == true)
            {

                if (txtApplicant.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(btnBrowse, "Please Select Applicant");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnBrowse, null);
                }
            }
            if (rdoConsecutive.Checked == true)
            {
                if (String.IsNullOrEmpty(txtNoofConsecDays.Text.Trim()))
                {
                    _errorProvider.SetError(txtNoofConsecDays, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblNumberOfConnect.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtNoofConsecDays.Text) > -1)
                        _errorProvider.SetError(txtNoofConsecDays, null);
                    else
                    {
                        _errorProvider.SetError(txtNoofConsecDays, "Number of Consecutive Days cannot be Negative");
                        isValid = false;
                    }
                }


            }

            if (rdoFundingSelect.Checked)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rdoFundingSelect, "Please Select at least One Fund");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoFundingSelect, null);
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
            string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            string AbsentReason = ((Captain.Common.Utilities.ListItem)cmbAbsentReason.SelectedItem).Value.ToString();
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strAbsRSel = rdoAbsentAll.Checked == true ? "A" : "S";
            string strReportType = string.Empty;
            string strReportSequence = string.Empty;
            string strChildren = string.Empty;
            string strIncomeEligible = string.Empty;
            // string strLabels = string.Empty;
            if (rdoSummary.Checked == true)
                strReportType = "S";
            else if (rdoConsecutive.Checked == true)
                strReportType = "C";
            else if (rdoSelectApplicant.Checked == true)
                strReportType = "A";


            if (rdoReportChildName.Checked == true)
                strReportSequence = "N";
            else if (rdoApplicant.Checked == true)
                strReportSequence = "A";
            else if (rdoClass.Checked == true)
                strReportSequence = "C";
            else if (rdoReason.Checked == true)
                strReportSequence = "R";

            string strBaseAge = rdoTodayDate.Checked ? "T" : "K";

            if (rdoChdRepeater.Checked == true)
                strChildren = "R";
            else if (rdoChdNonRepeater.Checked == true)
                strChildren = "N";
            else if (rdoChdBoth.Checked == true)
                strChildren = "B";


            if (rdoIncomeEligible.Checked == true)
                strIncomeEligible = "I";
            else if (rdoOverCome.Checked == true)
                strIncomeEligible = "O";
            else if (rdoIncomeAll.Checked == true)
                strIncomeEligible = "A";

            string strFundSource = rdoFundingAll.Checked ? "Y" : "N";
            string strMealData = rdoMealDataYes.Checked ? "Y" : "N";

            //if (rdoMailingLabels.Checked == true)
            //    strLabels = "M";
            //else if (rdoFileLabels.Checked == true)
            //    strLabels = "F";
            //else if (rdoNoLabels.Checked == true)
            //    strLabels = "N";


            //string strName = rdoChildName.Checked ? "Y" : "N";
            //string strPrint = rdoLaserLabels.Checked ? "Y" : "N";

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
            if (rdoFundingSelect.Checked == true)
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
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" ReportType = \"" + strReportType + "\"  ApplicantNo = \"" + txtApplicant.Text.Trim() + "\"  NoofConsective = \"" + txtNoofConsecDays.Text + "\" Active = \"" + Active + "\" ReportSequence = \"" + strReportSequence +
                            "\" AgeFrom = \"" + txtFrom.Text + "\" AgeTo = \"" + txtTo.Text + "\" BaseAge = \"" + strBaseAge + "\"  Children = \"" + strChildren + "\"   IncomeEligible = \"" + strIncomeEligible 
                            + "\" FundedSource = \"" + strFundSource + "\" MealData = \"" + strMealData + "\"  AbsentReason = \"" + AbsentReason + "\"  FromDate = \"" + dtForm.Value.Date 
                            + "\" ToDate = \"" + dtTodate.Value.Date + "\" SiteNames = \"" + strsiteRoomNames + "\" FundingCode = \"" + strFundingCodes + "\" AbsRSelect = \"" + strAbsRSel + "\" />");

            str.Append("</Rows>");
            //Labels = \"" + strLabels + "\" Name = \"" + strName + "\" Print = \"" + strPrint + "\"

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                if (dr["AbsRSelect"].ToString() == "A")
                    rdoAbsentAll.Checked = true;
                else
                    rdoAbsentSelect.Checked = true;

                if (rdoAbsentSelect.Checked)
                    CommonFunctions.SetComboBoxValue(cmbAbsentReason, dr["AbsentReason"].ToString());

                CommonFunctions.SetComboBoxValue(cmbActive, dr["Active"].ToString());

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
                Sel_REFS_List = Sel_REFS_List;
                if (dr["ReportType"].ToString() == "S")
                    rdoSummary.Checked = true;

                else if (dr["ReportType"].ToString() == "C")
                    rdoConsecutive.Checked = true;

                else if (dr["ReportType"].ToString() == "A")
                    rdoSelectApplicant.Checked = true;


                if (dr["ReportSequence"].ToString() == "N")
                    rdoReportChildName.Checked = true;
                else if (dr["ReportSequence"].ToString() == "A")
                    rdoApplicant.Checked = true;
                else if (dr["ReportSequence"].ToString() == "C")
                    rdoClass.Checked = true;
                else if (dr["ReportSequence"].ToString() == "R")
                    rdoReason.Checked = true;



                //if (dr["FundedDays"].ToString() == "Y")
                //    rdoAllFundYes.Checked = true;
                //else
                //    rdoAllFundNo.Checked = true;
                if (dr["FundedSource"].ToString() == "Y")
                    rdoFundingAll.Checked = true;
                else
                {
                    rdoFundingSelect.Checked = true;
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

                txtApplicant.Text = dr["ApplicantNo"].ToString();
                txtFrom.Text = dr["AgeFrom"].ToString();
                txtTo.Text = dr["AgeTo"].ToString();
                txtNoofConsecDays.Text = dr["NoofConsective"].ToString();

                if (dr["BaseAge"].ToString() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["Children"].ToString() == "R")
                    rdoChdRepeater.Checked = true;
                else if (dr["Children"].ToString() == "N")
                    rdoChdNonRepeater.Checked = true;
                else if (dr["Children"].ToString() == "B")
                    rdoChdBoth.Checked = true;

                if (dr["IncomeEligible"].ToString() == "I")
                    rdoIncomeEligible.Checked = true;
                else if (dr["IncomeEligible"].ToString() == "O")
                    rdoOverCome.Checked = true;
                else if (dr["IncomeEligible"].ToString() == "A")
                    rdoIncomeAll.Checked = true;

                if (dr["MealData"].ToString() == "Y")
                    rdoMealDataYes.Checked = true;
                else
                    rdoMealDataNo.Checked = true;

                //if (dr["Labels"].ToString() == "M")
                //    rdoMailingLabels.Checked = true;
                //else if (dr["Labels"].ToString() == "F")
                //    rdoFileLabels.Checked = true;
                //else if (dr["Labels"].ToString() == "N")
                //    rdoNoLabels.Checked = true;

                //if (dr["Name"].ToString() == "Y")
                //    rdoChildName.Checked = true;
                //else
                //    rdoParentName.Checked = true;

                //if (dr["Print"].ToString() == "Y")
                //    rdoLaserLabels.Checked = true;
                //else
                //    rdoLineLabels.Checked = true;

                dtForm.Value = Convert.ToDateTime(dr["FromDate"]);
                dtForm.Checked = true;
                dtTodate.Value = Convert.ToDateTime(dr["ToDate"]);
                dtTodate.Checked = true;

            }
        }

        private void rdoSelectApplicant_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(btnBrowse, null);
            if (rdoSelectApplicant.Checked == true)
            {
                txtApplicant.Enabled = true;
                btnBrowse.Visible = true;
                lblApplicantReq.Visible = true;
            }
            else
            {
                txtApplicant.Clear();
                txtApplicant.Enabled = false;
                btnBrowse.Visible = false;
                lblApplicantReq.Visible = false;

            }
        }

        private void rdoReason_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoReason.Checked == true)
            {
                pnAbsentReason.Enabled = true;
            }
            else
            {
                pnAbsentReason.Enabled = false;
                rdoAbsentAll.Checked = true;
                cmbAbsentReason.SelectedIndex = 0;
            }
        }

        private void rdoConsecutive_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtNoofConsecDays, null);
            if (rdoConsecutive.Checked == true)
            {
                txtNoofConsecDays.Enabled = true;
                lblNoofDaysReq.Visible = true;
            }
            else
            {
                txtNoofConsecDays.Clear();
                txtNoofConsecDays.Enabled = false;
                lblNoofDaysReq.Visible = false;
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
        private void rdoFundingSelect_Click(object sender, EventArgs e)
        {
            if (rdoFundingSelect.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        private void rdoNoLabels_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoLabels.Checked == true)
            {
                pnlName.Enabled = false;
                pnlPrinter.Enabled = false;
                rdoChildName.Checked = false;
                rdoParentName.Checked = false;
                rdoLaserLabels.Checked = false;
                rdoLineLabels.Checked = false;
            }
            else if (rdoFileLabels.Checked == true)
            {
                pnlName.Enabled = false;
                pnlPrinter.Enabled = true;
                rdoChildName.Checked = false;
                rdoParentName.Checked = false;
                rdoLaserLabels.Checked = true;
                rdoLineLabels.Checked = false;

            }
            else if (rdoMailingLabels.Checked == true)
            {
                pnlName.Enabled = true;
                pnlPrinter.Enabled = true;
                rdoChildName.Checked = true;
                rdoParentName.Checked = false;
                rdoLaserLabels.Checked = true;
                rdoLineLabels.Checked = false;
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //AdvancedMainMenuSearch advancedMainMenuSearch = new AdvancedMainMenuSearch(BaseForm, true);
            //advancedMainMenuSearch.FormClosed += new FormClosedEventHandler(advancedMainMenuSearch_FormClosed);
            //advancedMainMenuSearch.ShowDialog();
            BrowseApplicantForm BrowseApplcantForm = new BrowseApplicantForm(BaseForm, string.Empty, Privileges, Agency, Dept, Prog, Program_Year);
            BrowseApplcantForm.FormClosed += new FormClosedEventHandler(BrowseApplcantForm_FormClosed);
            BrowseApplcantForm.StartPosition = FormStartPosition.CenterScreen;
            BrowseApplcantForm.ShowDialog();
        }

        void BrowseApplcantForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BrowseApplicantForm BrowseApplication = sender as BrowseApplicantForm;
            if (BrowseApplication.DialogResult == DialogResult.OK)
            {

                CaseMstEntity caseMstData = BrowseApplication.MstData;
                if (caseMstData != null)
                {
                    txtApplicant.Text = caseMstData.ApplNo;
                }
            }
        }

        private void txtApplicant_Leave(object sender, EventArgs e)
        {
            if (txtApplicant.Text.Trim() != string.Empty)
            {
                txtApplicant.Text = SetLeadingZeros(txtApplicant.Text);
                List<HSSB2106Report_Entity> SeaarchReport_list = _model.ChldTrckData.GetChldTrck_Report(Agency, Dept, Prog, Program_Year, txtApplicant.Text, null, null, null, null, null, null, null, null);
                //CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
                //Search_Entity.Join_Mst_Snp = "N";
                //Search_Entity.Agy = Agency;
                //Search_Entity.Dept = Depart;
                //Search_Entity.Prog = Program;
                //Search_Entity.Year = Program_Year;
                //Search_Entity.App = txtAppNo.Text;
                //Search_Entity.Rec_Type = "H";
                //// Search_Entity.Status = "E";
                //List<CaseEnrlEntity> Transfer_Enroll_List = new List<CaseEnrlEntity>();
                //Transfer_Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");
                if (SeaarchReport_list.Count == 0)
                {
                     txtApplicant.Text = string.Empty;
                     AlertBox.Show("Applicant does not exist", MessageBoxIcon.Warning);
                }


            }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }

        private void rdoAbsentSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAbsentSelect.Checked == true)
                cmbAbsentReason.Enabled = true;
            else
                cmbAbsentReason.Enabled = false;
        }

        private void HSSB2109ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void rdoAbsentAll_CheckedChanged(object sender, EventArgs e)
        {
            cmbAbsentReason.SelectedIndex = 0;
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2109");
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllSite.Checked)
            { 
                _errorProvider.SetError(rdoMultipleSites, null);
                Sel_REFS_List.Clear();
            }
        }

        private void rdoFundingAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFundingAll.Checked)
            {
                _errorProvider.SetError(rdoFundingSelect, null);
                SelFundingList.Clear();
            }
        }
    }
}
