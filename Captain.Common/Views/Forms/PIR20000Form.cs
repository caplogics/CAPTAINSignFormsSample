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
using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using iTextSharp.text.pdf;
using System.IO;
using Captain.Common.Utilities;
using iTextSharp.text;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using log4net.Repository.Hierarchy;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PIR20000Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public PIR20000Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text =/* privileges.Program + " - " +*/ privileges.PrivilegeName;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Prog = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;
            // propCaseEnrlList = _model.EnrollData.GetEnrollDetailsPIR2000(Agency, Dept, Prog, Program_Year);
            propPirworkList = _model.EnrollData.GetPirWorkData(Agency, Dept, Prog, Program_Year, "PIRWORK");
            propReportPath = _model.lookupDataAccess.GetReportPath();

        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }
        public List<PirWorkEntity> propPirworkList { get; set; }



        #endregion

        private void OnHelpClick(object sender, EventArgs e)
        {

        }


        private void rdoAllApps_CheckedChanged(object sender, EventArgs e)
        {
            gvwEnrollDetails.Rows.Clear();
            gvwFundCounts.Rows.Clear();


            if (propPirworkList.Count > 0)
            {


                if (rdoAllApps.Checked)
                {
                    gvwFundCounts.Rows.Add("HS", propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                    gvwFundCounts.Rows.Add("HS2", propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                    gvwFundCounts.Rows.Add("EHS", propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                    gvwFundCounts.Rows.Add("EHSCCEP", propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());

                    foreach (PirWorkEntity item in propPirworkList)
                    {
                        gvwEnrollDetails.Rows.Add(item.PIRWORK_APP_NO, LookupDataAccess.GetMemberName(item.PIRWORK_APP_FNAME, item.PIRWORK_APP_MNAME, item.PIRWORK_APP_LNAME, BaseForm.BaseHierarchyCnFormat), item.PIRWORK_FUND, LookupDataAccess.Getdate(item.PIRWORK_FATTN), LookupDataAccess.Getdate(item.PIRWORK_LATTN), LookupDataAccess.Getdate(item.PIRWORK_ENROLL_DATE), LookupDataAccess.Getdate(item.PIRWORK_WDRAW_DATE));
                    }
                }
                else
                {
                    List<PirWorkEntity> pirWorkList = propPirworkList.FindAll(u => u.PIRWORK_FATTN != string.Empty && u.PIRWORK_FUND != string.Empty);
                    if (pirWorkList.Count > 0)
                    {

                        gvwFundCounts.Rows.Add("HS", pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                        gvwFundCounts.Rows.Add("HS2", pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS2" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                        gvwFundCounts.Rows.Add("EHS", pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());
                        gvwFundCounts.Rows.Add("EHSCCEP", pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());


                        //txt1HsFund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txt1EhsFund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txt1EhsccepFund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count.ToString();

                        //txtEnroHsfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txtEnrolEhsfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txtEnrolEhsccepfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();

                        //txtwithHsfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txtwithEhsfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                        //txtwithEhsccepfund.Text = propPirworkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty && u.PIRWORK_FATTN != string.Empty).Count.ToString();
                    }
                    foreach (PirWorkEntity item in pirWorkList)
                    {
                        gvwEnrollDetails.Rows.Add(item.PIRWORK_APP_NO, LookupDataAccess.GetMemberName(item.PIRWORK_APP_FNAME, item.PIRWORK_APP_MNAME, item.PIRWORK_APP_LNAME, BaseForm.BaseHierarchyCnFormat), item.PIRWORK_FUND, LookupDataAccess.Getdate(item.PIRWORK_FATTN), LookupDataAccess.Getdate(item.PIRWORK_LATTN), LookupDataAccess.Getdate(item.PIRWORK_ENROLL_DATE), LookupDataAccess.Getdate(item.PIRWORK_WDRAW_DATE));
                    }
                }
            }
            if (gvwEnrollDetails.Rows.Count > 0)
            {
                btnGeneratePdf.Visible = true;
                gvwEnrollDetails.Rows[0].Selected = true;
                gvwFundCounts.Rows[0].Selected = true;
            }
            else
            {
                btnGeneratePdf.Visible = false;
            }
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
                propPirworkList = _model.EnrollData.GetPirWorkData(Agency, Dept, Prog, Program_Year, "PIRWORK");
                rdoAllApps_CheckedChanged(sender, e);
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "R");
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
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);
                    propReportPath = _model.lookupDataAccess.GetReportPath();
                    propPirworkList = _model.EnrollData.GetPirWorkData(Agency, Dept, Prog, Program_Year, "PIRWORK");
                    // propCaseEnrlList = _model.EnrollData.GetEnrollDetailsPIR2000(Agency, Dept, Prog, Program_Year);
                    rdoAllApps_CheckedChanged(sender, e);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(777, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(690, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(777, 25);
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


        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {


            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
            pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();

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
                iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                List<PirWorkEntity> pirWorkList = new List<PirWorkEntity>();
                try
                {

                    PrintHeaderPage(document,writer);

                    document.NewPage();
                    Y_Pos = 795;


                    PdfPTable hssb2109_Table = new PdfPTable(9);
                    hssb2109_Table.TotalWidth = 550f;
                    hssb2109_Table.WidthPercentage = 100;
                    hssb2109_Table.LockedWidth = true;
                    float[] widths = new float[] { 30f, 75f, 25f, 30f, 30f, 30f, 35f, 35f, 20f };
                    hssb2109_Table.SetWidths(widths);
                    hssb2109_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                    hssb2109_Table.HeaderRows = 1;
                    // List<CaseEnrlEntity> caseEnrlList;

                    if (rdoattnDay.Checked)
                        pirWorkList = propPirworkList.FindAll(u => u.PIRWORK_FATTN != string.Empty && u.PIRWORK_FUND != string.Empty);
                    //caseEnrlList = propCaseEnrlList.FindAll(u => u.Enrl_Stat_Attn_FDate != string.Empty);
                    else
                        pirWorkList = propPirworkList;

                    if (pirWorkList.Count > 0)
                    {


                        PdfPCell Header_1 = new PdfPCell(new Phrase("App#", TblFontBold));
                        Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header_1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        Header_1.FixedHeight = 15f;
                        hssb2109_Table.AddCell(Header_1);

                        PdfPCell Header = new PdfPCell(new Phrase("Applicant Name", TblFontBold));
                        Header.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header);

                        PdfPCell HeaderTop1 = new PdfPCell(new Phrase("Fund", TblFontBold));
                        HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeaderTop1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(HeaderTop1);

                        PdfPCell Header1 = new PdfPCell(new Phrase("1st Attn", TblFontBold));
                        Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header1);

                        PdfPCell Header2 = new PdfPCell(new Phrase("Last Attn", TblFontBold));
                        Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header2);

                        PdfPCell Header3 = new PdfPCell(new Phrase("Enroll Dt", TblFontBold));
                        Header3.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header3.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header3);

                        PdfPCell Header4 = new PdfPCell(new Phrase("Withdraw Dt", TblFontBold));
                        Header4.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header4.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header4);

                        PdfPCell Header5 = new PdfPCell(new Phrase("Site Room", TblFontBold));
                        Header5.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header5.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header5);

                        PdfPCell Header6 = new PdfPCell(new Phrase("Active", TblFontBold));
                        Header6.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header6.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        hssb2109_Table.AddCell(Header6);


                        foreach (PirWorkEntity pirworkrow in pirWorkList)
                        {

                            PdfPCell pdfAppldata = new PdfPCell(new Phrase(pirworkrow.PIRWORK_APP_NO, TableFont));
                            pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfAppldata.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfAppldata);



                            PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(pirworkrow.PIRWORK_APP_FNAME, pirworkrow.PIRWORK_APP_MNAME, pirworkrow.PIRWORK_APP_LNAME, BaseForm.BaseHierarchyCnFormat), TableFont));
                            pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfChildName);

                            PdfPCell pdfAge = new PdfPCell(new Phrase(pirworkrow.PIRWORK_FUND, TableFont));
                            pdfAge.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfAge.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfAge);

                            PdfPCell pdfGardian = new PdfPCell(new Phrase(LookupDataAccess.Getdate(pirworkrow.PIRWORK_FATTN), TableFont));
                            pdfGardian.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfGardian.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfGardian);

                            PdfPCell pdfTelephone = new PdfPCell(new Phrase(LookupDataAccess.Getdate(pirworkrow.PIRWORK_LATTN), TableFont));
                            pdfTelephone.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTelephone.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfTelephone);

                            PdfPCell pdfClass = new PdfPCell(new Phrase(LookupDataAccess.Getdate(pirworkrow.PIRWORK_ENROLL_DATE), TableFont));
                            pdfClass.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfClass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfClass);

                            PdfPCell pdfFundingDesc = new PdfPCell(new Phrase(LookupDataAccess.Getdate(pirworkrow.PIRWORK_WDRAW_DATE), TableFont));
                            pdfFundingDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfFundingDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfFundingDesc);

                            PdfPCell pdfsitedata = new PdfPCell(new Phrase(pirworkrow.PIRWORK_SITE + "/" + pirworkrow.PIRWORK_SITE_ROOM + "/" + pirworkrow.PIRWORK_SITE_AMPM, TableFont));
                            pdfsitedata.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfsitedata.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfsitedata);

                            PdfPCell pdfActivedata = new PdfPCell(new Phrase("", TableFont));
                            pdfActivedata.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfActivedata.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfActivedata);




                        }

                        PdfPCell pdfNewLine1 = new PdfPCell(new Phrase("  ", TableFont));
                        pdfNewLine1.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfNewLine1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfNewLine1.Colspan = 9;
                        hssb2109_Table.AddCell(pdfNewLine1);


                        PdfPCell pdfTotalattnData = new PdfPCell(new Phrase("Total attn 1 day HS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count.ToString() + "     Total attn 1 day EHS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count.ToString() + "     Total attn 1 day EHSCCEP Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count.ToString(), TableFont));
                        pdfTotalattnData.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTotalattnData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfTotalattnData.Colspan = 9;
                        hssb2109_Table.AddCell(pdfTotalattnData);

                        PdfPCell pdfTotalEnrollData = new PdfPCell(new Phrase("Total Enrolled HS Fund     : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString() + "     Total Enrolled EHS Fund      : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString() + "     Total Enrolled EHSCCEP Fund      : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString(), TableFont));
                        pdfTotalEnrollData.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTotalEnrollData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfTotalEnrollData.Colspan = 9;
                        hssb2109_Table.AddCell(pdfTotalEnrollData);

                        PdfPCell pdfTotalWithdrawnData = new PdfPCell(new Phrase("Total Withdrawn HS Fund  : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString() + "      Total Withdrawn EHS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString() + "      Total Withdrawn EHSCCEP Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString(), TableFont));
                        pdfTotalWithdrawnData.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTotalWithdrawnData.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        pdfTotalWithdrawnData.Colspan = 9;
                        hssb2109_Table.AddCell(pdfTotalWithdrawnData);


                    }

                    if (hssb2109_Table.Rows.Count > 0)
                    {
                        document.Add(hssb2109_Table);
                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                AlertBox.Show("Report Generated Successfully");
                document.Close();
                fs.Close();
                fs.Dispose();

                if (pirWorkList.Count > 0 && chkbExcel.Checked)
                {
                    //On_ExcelForm_Closed(pirWorkList, PdfName);
                    On_ExcelForm_Closed_DevExpress(pirWorkList, PdfName);

                }
            }
        }

        private void On_ExcelForm_Closed(List<PirWorkEntity> pirWorkList, string pdfname)
        {
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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

            ExcelDocument xlWorkSheet = new ExcelDocument();

            xlWorkSheet.ColumnWidth(0, 0);
            xlWorkSheet.ColumnWidth(1, 100);
            xlWorkSheet.ColumnWidth(2, 300);
            xlWorkSheet.ColumnWidth(3, 80);
            xlWorkSheet.ColumnWidth(4, 100);
            xlWorkSheet.ColumnWidth(5, 100);

            xlWorkSheet.ColumnWidth(6, 100);
            xlWorkSheet.ColumnWidth(7, 100);
            xlWorkSheet.ColumnWidth(8, 100);
            xlWorkSheet.ColumnWidth(9, 100);

            xlWorkSheet.ColumnWidth(10, 100);
            xlWorkSheet.ColumnWidth(11, 120);
            xlWorkSheet.ColumnWidth(12, 150);
            xlWorkSheet.ColumnWidth(13, 80);

            int excelcolumn = 0;
            try
            {
                if (pirWorkList.Count > 0)
                {
                    xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 1, "App#");

                    xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Applicant Name");

                    xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 3, "Fund");

                    xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 4, "1st Attn");

                    xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 5, "Last Attn");

                    xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 6, "Days Present");

                    xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 7, "Days Absent");

                    xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 8, "Total Days");

                    xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 9, "% Absent");

                    xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 10, "Enroll Dt");

                    xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 11, "Withdraw Dt");

                    xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 12, "Site Room");

                    xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 13, "Active");

                    foreach (PirWorkEntity pirworkrow in pirWorkList)
                    {
                        excelcolumn = excelcolumn + 1;

                        xlWorkSheet.WriteCell(excelcolumn, 1, pirworkrow.PIRWORK_APP_NO);
                        xlWorkSheet.WriteCell(excelcolumn, 2, LookupDataAccess.GetMemberName(pirworkrow.PIRWORK_APP_FNAME, pirworkrow.PIRWORK_APP_MNAME, pirworkrow.PIRWORK_APP_LNAME, BaseForm.BaseHierarchyCnFormat));
                        xlWorkSheet.WriteCell(excelcolumn, 3, pirworkrow.PIRWORK_FUND);
                        xlWorkSheet.WriteCell(excelcolumn, 4, LookupDataAccess.Getdate(pirworkrow.PIRWORK_FATTN));
                        xlWorkSheet.WriteCell(excelcolumn, 5, LookupDataAccess.Getdate(pirworkrow.PIRWORK_LATTN));
                        //xlWorkSheet.WriteCell(excelcolumn, 6, pirworkrow.Enroll_Days);
                        xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 6, pirworkrow.Present_Days);
                        xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 7, pirworkrow.Absent_Days);
                        
                        decimal Totdays = int.Parse(pirworkrow.Present_Days) + int.Parse(pirworkrow.Absent_Days);
                        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 8, Totdays);

                        decimal AbPercnt = 0;
                        if(Totdays>0)
                        {
                            decimal absent = decimal.Parse(pirworkrow.Absent_Days.Trim());

                            AbPercnt = absent / Totdays *100;
                        }
                        xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Right;
                        xlWorkSheet.WriteCell(excelcolumn, 9, AbPercnt.ToString("0.0"));
                        
                        xlWorkSheet.WriteCell(excelcolumn, 10, LookupDataAccess.Getdate(pirworkrow.PIRWORK_ENROLL_DATE));
                        xlWorkSheet.WriteCell(excelcolumn, 11, LookupDataAccess.Getdate(pirworkrow.PIRWORK_WDRAW_DATE));
                        xlWorkSheet.WriteCell(excelcolumn, 12, pirworkrow.PIRWORK_SITE + "/" + pirworkrow.PIRWORK_SITE_ROOM + "/" + pirworkrow.PIRWORK_SITE_AMPM);
                        xlWorkSheet.WriteCell(excelcolumn, 13, "");

                    }

                    excelcolumn = excelcolumn + 1;

                    excelcolumn = excelcolumn + 1;

                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total attn 1 day HS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total attn 1 day EHS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total attn 1 day EHSCCEP Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Enrolled HS Fund     : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Enrolled EHS Fund      : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Enrolled EHSCCEP Fund      : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Withdrawn HS Fund  : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Withdrawn EHS Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());

                    excelcolumn = excelcolumn + 1;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Total Withdrawn EHSCCEP Fund   : " + pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty).Count.ToString());


                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                xlWorkSheet.Save(stream);
                stream.Close();

            }
            catch (Exception ex) { }


        }


        private void On_ExcelForm_Closed_DevExpress(List<PirWorkEntity> pirWorkList, string pdfname)
        {
            #region FileNameBuild

            Random_Filename = null;

            string xlFileName = pdfname;

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

                    #region Custom Styles

                    #endregion

                    DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets[0];

                    SheetDetails.Name = "Data";

                    SheetDetails.ActiveView.TabColor = ColorTranslator.FromHtml("#ADD8E6");

                    SheetDetails.ActiveView.ShowGridlines = false;
                    wb.Unit = DevExpress.Office.DocumentUnit.Point;

                    #region Column Widths

                    SheetDetails.Columns[0].Width = 10;

                    SheetDetails.Columns[1].Width = 80;
                    SheetDetails.Columns[2].Width = 250;
                    SheetDetails.Columns[3].Width = 60;
                    SheetDetails.Columns[4].Width = 80;
                    SheetDetails.Columns[5].Width = 80;
                    SheetDetails.Columns[6].Width = 80;
                    SheetDetails.Columns[7].Width = 80;
                    SheetDetails.Columns[8].Width = 80;
                    SheetDetails.Columns[9].Width = 80;
                    SheetDetails.Columns[10].Width = 90;
                    SheetDetails.Columns[11].Width = 90;
                    SheetDetails.Columns[12].Width = 100;
                    SheetDetails.Columns[13].Width = 80;
                    SheetDetails.Columns[14].Width = 10;

                    #endregion

                    if (pirWorkList.Count > 0)
                    {
                        _Rowindex = 0;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName.Trim().ToUpper();
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 13, oDevExpress_Excel_Properties.gxlFrameStyleL);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;
                        _Columnindex = _Columnindex + 13;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 30;

                        _Rowindex++;
                        _Columnindex = 0;

                        #region Column Headers

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Applicant Name";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "1st Attn";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Last Attn";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Days Present";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Days Absent";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Days";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "% Absent";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Enroll Date";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Withdraw Date";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Site Room";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Active";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

                        #endregion

                        foreach (PirWorkEntity pirworkrow in pirWorkList)
                        {

                            _Rowindex++;
                            _Columnindex = 0;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.PIRWORK_APP_NO;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.GetMemberName(pirworkrow.PIRWORK_APP_FNAME, pirworkrow.PIRWORK_APP_MNAME, pirworkrow.PIRWORK_APP_LNAME, BaseForm.BaseHierarchyCnFormat);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.PIRWORK_FUND;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(pirworkrow.PIRWORK_FATTN);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = LookupDataAccess.Getdate(pirworkrow.PIRWORK_LATTN);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.Present_Days == "" ? 0 : Convert.ToInt32(pirworkrow.Present_Days);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.Absent_Days == "" ? 0 : Convert.ToInt32(pirworkrow.Absent_Days);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            decimal Totdays = int.Parse(pirworkrow.Present_Days) + int.Parse(pirworkrow.Absent_Days);

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = Totdays;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            decimal AbPercnt = 0;
                            if (Totdays > 0)
                            {
                                decimal absent = decimal.Parse(pirworkrow.Absent_Days.Trim());

                                AbPercnt = absent / Totdays * 100;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = AbPercnt.ToString("0.0");
                            }
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDeci_DBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.PIRWORK_ENROLL_DATE == "" ? "" : LookupDataAccess.Getdate(pirworkrow.PIRWORK_ENROLL_DATE);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.PIRWORK_WDRAW_DATE == "" ? "" : LookupDataAccess.Getdate(pirworkrow.PIRWORK_WDRAW_DATE);
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                            SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                            _Columnindex++;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirworkrow.PIRWORK_SITE + "/" + pirworkrow.PIRWORK_SITE_ROOM + "/" + pirworkrow.PIRWORK_SITE_AMPM;
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
                        }

                        #region Footer

                        _Rowindex++;
                        _Columnindex = 0;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "©2024 CAPSystems Inc";
                        oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 13, oDevExpress_Excel_Properties.gxlFrameFooterStyle);
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;
                        _Columnindex = _Columnindex + 13;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameStyleL;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 10;

                        #endregion

                        #region Bottom Data

                        _Rowindex = _Rowindex + 2;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total attn 1 day HS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_FATTN != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total attn 1 day EHS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_FATTN != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total attn 1 day EHSCCEP Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_FATTN != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex = _Rowindex + 2;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Enrolled HS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Enrolled EHS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_ENROLL_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Enrolled EHSCCEP Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_ENROLL_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex = _Rowindex + 2;
                        _Columnindex = 2;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Withdrawn HS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "HS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Withdrawn EHS Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHS" && u.PIRWORK_WDRAW_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        _Rowindex++;
                        _Columnindex = 2;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total Withdrawn EHSCCEP Fund";
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;
                        _Columnindex++;

                        SheetDetails.Rows[_Rowindex][_Columnindex].Value = pirWorkList.FindAll(u => u.PIRWORK_FUND == "EHSCCEP" && u.PIRWORK_WDRAW_DATE != string.Empty).Count;
                        SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNumb_DBCC_Blue_Borders;
                        SheetDetails.Rows[_Rowindex][_Columnindex].RowHeight = 20;

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

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }


        public string TaskDates(ChldMediEntity chldmedi)
        {
            string strDate = string.Empty;
            if (chldmedi.COMPLETED_DATE != string.Empty)
            {
                strDate = LookupDataAccess.Getdate(chldmedi.COMPLETED_DATE) + " C";
            }
            else
            {
                if (chldmedi.FOLLOWUP_DATE != string.Empty)
                {
                    strDate = LookupDataAccess.Getdate(chldmedi.FOLLOWUP_DATE) + " F";
                }
                else
                {
                    strDate = LookupDataAccess.Getdate(chldmedi.ADDRESSED_DATE) + " A";
                }
            }
            return strDate;

        }

        private void PrintHeaderPage(Document document,PdfWriter writer)
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
            float[] Headerwidths = new float[] { 15f, 100f };
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
            row3.MinimumHeight = 6;
            row3.PaddingBottom = 5;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(row3);

            /*string Agy = "Agency : All"; string Det = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = "Agency : " + Agency;
            if (Dept != "**") Det = "Dept : " + Dept;
            if (Prog != "**") Prg = "Program : " + Prog;*/

            string Agy = /*"Agency : */"All"; string Depart = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Dept != "**") Dept = /*"Dept : " + */Depart;
            if (Prog != "**") Prg = /*"Program : " +*/ Prog;

            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

           /* PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text + "  " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);*/

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.Colspan = 7;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.Colspan = 7;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "By"/* : " + (rdoAllApps.Checked == true ? rdoAllApps.Text : rdoattnDay.Text)*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase((rdoAllApps.Checked == true ? rdoAllApps.Text : rdoattnDay.Text), TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

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


        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();

        }

        private void btnGenerateData_Click(object sender, EventArgs e)
        {

            try
            {

                List<PirCntl> propPirCntl = _model.EnrollData.GetPirCntlData();
                if (propPirCntl.Count > 0)
                {
                    PirWorkEntity pirworkEntity = new PirWorkEntity();
                    pirworkEntity.PIRWORK_AGENCY = Agency;
                    pirworkEntity.PIRWORK_DEPT = Dept;
                    pirworkEntity.PIRWORK_PROG = Prog;
                    pirworkEntity.PIRWORK_YEAR = Program_Year;
                    propPirworkList = _model.EnrollData.GetGenerateworkDETAILS(Agency, Dept, Prog, Program_Year, string.Empty);
                    if (propPirworkList.Count > 0)
                    {
                        rdoAllApps_CheckedChanged(rdoAllApps, new EventArgs());
                    }
                }
                else
                {
                    AlertBox.Show("Please define HS/EHS/EHSCCEP Funds", MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }

        }

        private void btnDefine_Click(object sender, EventArgs e)
        {
            PIR2000FundSelection fundSelection = new PIR2000FundSelection(BaseForm, Privileges);
            fundSelection.StartPosition = FormStartPosition.CenterScreen;
            fundSelection.ShowDialog();

        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            PirWorkEntity pirworkEntity = new PirWorkEntity();
            pirworkEntity.PIRWORK_AGENCY = Agency;
            pirworkEntity.PIRWORK_DEPT = Dept;
            pirworkEntity.PIRWORK_PROG = Prog;
            pirworkEntity.PIRWORK_YEAR = Program_Year;
            if (rdoattnDay.Checked == true)
                pirworkEntity.PIRWORK_Show = "N";
            else
                pirworkEntity.PIRWORK_Show = "Y";
            pirworkEntity.Mode = "GENERATE";

            if (_model.EnrollData.InsertDelPirWorks(pirworkEntity))
            {
                
                propPirworkList = _model.EnrollData.GetPirWorkData(Agency, Dept, Prog, Program_Year, "PIRWORK");
                rdoAllApps_CheckedChanged(rdoAllApps, new EventArgs());
                List<PirWorkEntity> propPirworkExcelList = _model.EnrollData.GetPirWorkData(Agency, Dept, Prog, Program_Year, "DUPFUNDFAMILYID");
                if (propPirworkExcelList.Count > 0)
                {
                    GenerateExcelPIRWORK(propPirworkExcelList);
                }
                AlertBox.Show("Work File Generated Successfully");
            }
        }

        private void PIR20000Form_Load(object sender, EventArgs e)
        {
            rdoAllApps_CheckedChanged(rdoAllApps, new EventArgs());
        }

        private void PIR20000Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "PIR00000");
        }

        #region Excelfile

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
            s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s62.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s63.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Font.FontName = "Calibri";
            s64.Font.Size = 11;
            s64.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s64.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s65.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Font.Bold = true;
            s66.Font.FontName = "Calibri";
            s66.Font.Size = 11;
            s66.Font.Color = "#003366";
            s66.Interior.Color = "#FBE4D5";
            s66.Interior.Pattern = StyleInteriorPattern.Solid;
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s66.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Font.Bold = true;
            s67.Font.FontName = "Calibri";
            s67.Font.Size = 11;
            s67.Font.Color = "#003366";
            s67.Interior.Color = "#FBE4D5";
            s67.Interior.Pattern = StyleInteriorPattern.Solid;
            s67.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s67.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Font.Bold = true;
            s68.Font.FontName = "Calibri";
            s68.Font.Size = 11;
            s68.Font.Color = "#003366";
            s68.Interior.Color = "#FBE4D5";
            s68.Interior.Pattern = StyleInteriorPattern.Solid;
            s68.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s68.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Font.Bold = true;
            s69.Font.FontName = "Calibri";
            s69.Font.Size = 11;
            s69.Font.Color = "#003366";
            s69.Interior.Color = "#FBE4D5";
            s69.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Interior.Color = "#FFFF00";
            s71.Interior.Pattern = StyleInteriorPattern.Solid;
            s71.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s71.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Font.FontName = "Calibri";
            s76.Font.Size = 11;
            s76.Font.Color = "#FF0000";
            s76.Interior.Color = "#FFFF00";
            s76.Interior.Pattern = StyleInteriorPattern.Solid;            
            s76.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        }


        private void GenerateExcelPIRWORK(List<PirWorkEntity> propPirworkExcelList)
        {

            PdfName = "Bad_Work_Records"; //+ BaseForm.UserID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");

            Random_Filename = null;

            string strFileName = string.Empty;

            strFileName = PdfName;

            strFileName = strFileName + ".xls";

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




            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            Worksheet sheet; WorksheetCell cell;

            sheet = book.Worksheets.Add("Data");

            sheet.Table.DefaultRowHeight = 14.4F;

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 46;
            column0.StyleID = "s62";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 40;
            column1.StyleID = "s62";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 40;
            column2.StyleID = "s62";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 46;
            column3.StyleID = "s63";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 49;
            column4.StyleID = "s62";
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Width = 42;
            column5.StyleID = "s62";
            sheet.Table.Columns.Add(109);
            sheet.Table.Columns.Add(108);
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 52;
            column8.StyleID = "s62";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 76;
            column9.StyleID = "s64";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 43;
            column10.StyleID = "s65";           
            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(80);
            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(120);
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.AutoFitHeight = false;
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.AutoFitHeight = false;
            Row1.Cells.Add("AG", DataType.String, "s66");
            Row1.Cells.Add("DE", DataType.String, "s66");
            Row1.Cells.Add("PR", DataType.String, "s66");
            Row1.Cells.Add("YR", DataType.String, "s67");
            Row1.Cells.Add("APP#", DataType.String, "s68");
            Row1.Cells.Add("FUND", DataType.String, "s68");
            Row1.Cells.Add("FNAME", DataType.String, "s69");
            Row1.Cells.Add("LNAME", DataType.String, "s69");
            Row1.Cells.Add("SSN", DataType.String, "s66");
            Row1.Cells.Add("FAMILY_ID", DataType.String, "s67");
            Row1.Cells.Add("OMB", DataType.String, "s68");
            Row1.Cells.Add("Family Type", DataType.String, "s66");
            Row1.Cells.Add("Ethnicity", DataType.String, "s69");
            Row1.Cells.Add("Race", DataType.String, "s69");
            Row1.Cells.Add("Language", DataType.String, "s69");
            Row1.Cells.Add("Wdraw Description", DataType.String, "s66");
            Row1.Cells.Add("Housing Type", DataType.String, "s66");

            // -----------------------------------------------



            int intseq = 0;
            string strFamid = string.Empty; string strfund = string.Empty;
            bool boolbadData = true; bool boolfamiy = true; bool booleth = true; bool boolrace = true; bool boolwcode = true; bool boollang = true;bool boolhouseing = true;
            
                foreach (PirWorkEntity dritem in propPirworkExcelList)
            {
                if (strFamid != dritem.PIRWORK_FAMILY_ID && strfund != dritem.PIRWORK_FUND)
                {
                    List<PirWorkEntity> pirduplic = propPirworkExcelList.FindAll(u => u.PIRWORK_FAMILY_ID.Trim() == dritem.PIRWORK_FAMILY_ID.Trim() && u.PIRWORK_FUND.Trim() == dritem.PIRWORK_FUND.Trim());
                    boolbadData = true;
                    boolfamiy = booleth = boolrace = boolwcode = boollang = boolhouseing = true ;
                    
                    foreach (PirWorkEntity item in pirduplic)
                    {
                        if (item.PIRWORK_FAMILY_TYPE != dritem.PIRWORK_FAMILY_TYPE)
                        {
                            boolbadData = false;
                            boolfamiy = false;
                            // break;
                        }
                        if (item.PIRWORK_ETHNICITY != dritem.PIRWORK_ETHNICITY)
                        {
                            boolbadData = false;
                            booleth = false;
                            //break;
                        }
                        if (item.PIRWORK_RACE != dritem.PIRWORK_RACE)
                        {
                            boolbadData = false;
                            boolrace = false;
                            // break;
                        }
                        if (item.PIRWORK_LANGUAGE != dritem.PIRWORK_LANGUAGE)
                        {
                            boolbadData = false;
                            boollang = false;
                            //break;
                        }
                        if (item.PIRWORK_WDRAW_CODE != dritem.PIRWORK_WDRAW_CODE)
                        {
                            boolbadData = false;
                            boolwcode = false;
                            //break;
                        }
                        if (item.PIRWORK_HOUSING != dritem.PIRWORK_HOUSING)
                        {
                            boolbadData = false;
                            boolhouseing = false;
                            //break;
                        }

                    }
                }

                Row1 = sheet.Table.Rows.Add();
                Row1.Cells.Add(dritem.PIRWORK_AGENCY, DataType.String, "Default");
                Row1.Cells.Add(dritem.PIRWORK_DEPT, DataType.String, "Default");
                Row1.Cells.Add(dritem.PIRWORK_PROG, DataType.String, "Default");
                Row1.Cells.Add(dritem.PIRWORK_YEAR, DataType.String, "Default");

                Row1.Cells.Add(dritem.PIRWORK_APP_NO, DataType.String, "Default");


                Row1.Cells.Add(dritem.PIRWORK_FUND, DataType.String, "Default");


                Row1.Cells.Add(dritem.PIRWORK_APP_FNAME, DataType.String, "Default");


                Row1.Cells.Add(dritem.PIRWORK_APP_LNAME, DataType.String, "Default");


                Row1.Cells.Add(dritem.PIRWORK_APP_SSN, DataType.String, "Default");


                if (boolbadData)
                    Row1.Cells.Add(dritem.PIRWORK_FAMILY_ID, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_FAMILY_ID, DataType.String, "s76");

                Row1.Cells.Add(dritem.PIRWORK_POVERTY, DataType.String, "Default");


                if (boolfamiy)
                    Row1.Cells.Add(dritem.PIRWORK_FAMILYTYPEDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_FAMILYTYPEDESC, DataType.String, "s76");


                if (booleth)
                    Row1.Cells.Add(dritem.PIRWORK_ETHNICITYDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_ETHNICITYDESC, DataType.String, "s76");


                if (boolrace)
                    Row1.Cells.Add(dritem.PIRWORK_RACEDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_RACEDESC, DataType.String, "s76");

                if (boollang)
                    Row1.Cells.Add(dritem.PIRWORK_LANGUAGEDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_LANGUAGEDESC, DataType.String, "s76");

                if (boolwcode)
                    Row1.Cells.Add(dritem.PIRWORK_WDRAWDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_WDRAWDESC, DataType.String, "s76");

                if (boolhouseing)
                    Row1.Cells.Add(dritem.PIRWORK_HOUSINGDESC, DataType.String, "Default");
                else
                    Row1.Cells.Add(dritem.PIRWORK_HOUSINGDESC, DataType.String, "s76");




                // -----------------------------------------------
            }
            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();
            AlertBox.Show("Work file generated Successfully. \n Also see ‘Bad_Work_Records.xls’ for duplicate attributes");
        }
        #endregion
    }
}
