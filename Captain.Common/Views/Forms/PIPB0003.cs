#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

using Wisej.Web;
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

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;

#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PIPB0003 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private CustomFieldsControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public PIPB0003(BaseForm baseform, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim() == "I")
            {
                lblAgency.Visible = true;
                CmbAgency.Visible = true;
                FillAgencyCombo();
            }

            propReportPath = _model.lookupDataAccess.GetReportPath();
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");

            this.Text = Privileges.PrivilegeName;

            //if (propAgencyControlDetails.State == "TX") chkbExcel.Visible = false; else chkbExcel.Visible = true;

        }
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
        #endregion

        private void FillAgencyCombo()
        {
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("1", " ", " ");
            DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once

            CmbAgency.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            int TmpRows = 0;
            int AgyIndex = 0;
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if(dt.Rows.Count>1)
                        listItem.Add(new Captain.Common.Utilities.ListItem("** - All", "**"));

                    foreach (DataRow dr in dt.Rows)
                    {
                        listItem.Add(new Captain.Common.Utilities.ListItem(dr["Agy"] + " - " + dr["Name"], dr["Agy"]));
                        //if (DefAgy == dr["Agy"].ToString())
                        //    AgyIndex = TmpRows;

                        TmpRows++;
                    }
                    if (TmpRows > 0)
                    {
                        CmbAgency.Items.AddRange(listItem.ToArray());
                        //if (DefHieExist)
                        //    CmbAgency.SelectedIndex = AgyIndex;
                        //else
                        //{
                            //if (CmbAgency.Items.Count == 1)
                                CmbAgency.SelectedIndex = 0;
                        //}
                    }
                }
                //DefAgy = DefDept = DefProg = DefYear = null;
            }
            catch (Exception ex) { }
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void BtnGenFile_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
                //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormPdf);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
            //if (propAgencyControlDetails.State == "TX")
            //{
            //    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
            //    pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveFormClosed);
            //    pdfListForm.ShowDialog();
            //}
            //else
            //{

            //    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
            //    pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_DataFeed);
            //    if(chkbExcel.Checked)
            //        pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_ExcelFormClosed);
            //    pdfListForm.ShowDialog();
            //}
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            //if (rbDateRange.Checked)
            //{
            if (string.IsNullOrWhiteSpace(dtpFrmDate.Text))
            {
                _errorProvider.SetError(dtpFrmDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFrmDate, null);
            }
            if (string.IsNullOrWhiteSpace(dtpToDt.Text))
            {
                _errorProvider.SetError(dtpToDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDt, null);
            }
            if (dtpFrmDate.Value.Date > dtpToDt.Value.Date)
                {
                    _errorProvider.SetError(dtpToDt, "To Date should be greater than From Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDt, null);
                }
            //}

            
            return (isValid);
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        
        private void On_SaveFormClosedVer2(object sender, FormClosedEventArgs e)
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
                    CommonFunctions.MessageBoxDisplay("Error");
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
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontSize = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                //if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                //string Year = string.Empty;
                //if (CmbYear.Visible == true)
                //    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                //DataTable dtMst = new DataTable(); DataTable dtAttn = new DataTable();
                //DataSet dsMSt = DatabaseLayer.ARSDB.ARSb2120_MSTList(Agency, Depart, Program, Year);
                //if (dsMSt != null)
                //    dtMst = dsMSt.Tables[0];

                string Agy = string.Empty;
                

                DataTable dt = new DataTable();
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim() == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() != "**")
                        Agy = ((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim();

                    dt = GETPIPREGDATABYDATE(dtpFrmDate.Text.Trim(), dtpToDt.Text.Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName.Trim(),Agy);
                }
                else
                    dt = GETPIPREGDATABYDATE(dtpFrmDate.Text.Trim(), dtpToDt.Text.Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName.Trim(),Agy);

                //if(BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim()=="I")

                //DataSet dsFund = DatabaseLayer.Lookups.GetJobSchdule(dtpFrmDate.Text.Trim(), dtpToDt.Text.Trim());
                //DataTable dtFund = dsFund.Tables[0];

                try
                {
                    PrintHeaderPage(document);
                    document.NewPage();

                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(9);
                        table.TotalWidth = 570f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 15f, 50f, 25f, 50f, 35f, 25f, 30f, 45f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        string[] HeaderSeq4 = { "SNo", "Name", "DOB", "Email", "Mobile",  "Language", "Registration Date", "Last Logged in", "# in house" };
                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.FixedHeight = 15f;
                            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }

                        int j = 1; int TotalHH = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            PdfPCell T0 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                            T0.HorizontalAlignment = Element.ALIGN_LEFT;
                            T0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T0);

                            PdfPCell T1 = new PdfPCell(new Phrase(dr["PIPREG_LNAME"].ToString().Trim() + " " + dr["PIPREG_FNAME"].ToString().Trim(), TableFont));
                            T1.HorizontalAlignment = Element.ALIGN_LEFT;
                            T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T1);

                            PdfPCell T2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["PIPREG_DOB"].ToString().Trim()), TableFont));
                            T2.HorizontalAlignment = Element.ALIGN_LEFT;
                            T2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T2);

                            PdfPCell T3 = new PdfPCell(new Phrase(dr["PIPREG_EMAIL"].ToString().Trim(), TableFont));
                            T3.HorizontalAlignment = Element.ALIGN_LEFT;
                            T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T3);

                            MaskedTextBox mskPhn = new MaskedTextBox();
                            if (!string.IsNullOrEmpty(dr["PIPREG_MOBILE"].ToString().Trim()))
                            {
                                mskPhn.Mask = "(999) 000-0000";
                                mskPhn.Text = dr["PIPREG_MOBILE"].ToString().Trim();
                            }
                            PdfPCell T4 = new PdfPCell(new Phrase(mskPhn.Text, TableFont));
                            T4.HorizontalAlignment = Element.ALIGN_LEFT;
                            T4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T4);

                           

                            string Language = string.Empty;
                            Language = getLanguagedesc(dr["PIPREG_PREFD_LANG"].ToString().Trim());

                            PdfPCell T5 = new PdfPCell(new Phrase(Language, TableFont));
                            T5.HorizontalAlignment = Element.ALIGN_LEFT;
                            T5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T5);

                            PdfPCell T10 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["PIPREG_DATE"].ToString().Trim()), TableFont));
                            T10.HorizontalAlignment = Element.ALIGN_LEFT;
                            T10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T10);

                            PdfPCell T6 = new PdfPCell(new Phrase(dr["PIPREG_LAST_LOGDIN"].ToString().Trim(), TableFont));
                            T6.HorizontalAlignment = Element.ALIGN_LEFT;
                            T6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T6);

                            PdfPCell T7 = new PdfPCell(new Phrase(dr["PIPREG_NO_INHH"].ToString().Trim(), TableFont));
                            T7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            T7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T7);

                            if (!string.IsNullOrEmpty(dr["PIPREG_NO_INHH"].ToString().Trim()))
                                TotalHH = TotalHH + int.Parse(dr["PIPREG_NO_INHH"].ToString().Trim());

                            j++;
                        }

                        PdfPCell S00 = new PdfPCell(new Phrase("", TableFont));
                        S00.HorizontalAlignment = Element.ALIGN_LEFT;
                        S00.Colspan = 9;
                        S00.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S00);

                        PdfPCell S0 = new PdfPCell(new Phrase("", TableFont));
                        S0.HorizontalAlignment = Element.ALIGN_LEFT;
                        S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S0);

                        PdfPCell S1 = new PdfPCell(new Phrase("Total Users Registered:", TableFont));
                        S1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S1);

                        PdfPCell S2 = new PdfPCell(new Phrase((j - 1).ToString(), TableFont));
                        S2.HorizontalAlignment = Element.ALIGN_LEFT;
                        S2.Colspan = 7;
                        S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S2);

                        

                        PdfPCell S3 = new PdfPCell(new Phrase("", TableFont));
                        S3.HorizontalAlignment = Element.ALIGN_LEFT;
                        S3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S3);

                        PdfPCell S4 = new PdfPCell(new Phrase("Total # in House: ", TableFont));
                        S4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        S4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S4);

                        PdfPCell S5 = new PdfPCell(new Phrase(TotalHH.ToString(), TableFont));
                        S5.HorizontalAlignment = Element.ALIGN_LEFT;
                        S5.Colspan = 7;
                        S5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S5);

                        document.Add(table);
                    }

                    if (chkbExcel.Checked) OnSaveExcel_Report(dt, PdfName);

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();
            }
        }

        private void On_SaveFormPdf(object sender, FormClosedEventArgs e)
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
                    CommonFunctions.MessageBoxDisplay("Error");
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
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontSize = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
               

                string Agy = string.Empty;


                DataTable dt = new DataTable();
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim() == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() != "**")
                        Agy = ((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim();

                    dt = GETPIPREGDATABYDATE(dtpFrmDate.Text.Trim(), dtpToDt.Text.Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName.Trim(), Agy);
                }
                else
                    dt = GETPIPREGDATABYDATE(dtpFrmDate.Text.Trim(), dtpToDt.Text.Trim(), BaseForm.BaseAgencyControlDetails.AgyShortName.Trim(), Agy);

               

                try
                {
                    PrintHeaderPage(document);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();

                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable table;
                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        {
                            if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                            {
                                table = new PdfPTable(13);
                                table.TotalWidth = 790f;
                                table.WidthPercentage = 100;
                                table.LockedWidth = true;
                                float[] widths = new float[] { 15f, 18f, 30f, 50f, 45f, 25f, 28f, 25f, 23f, 23f, 42f, 23f,23f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                table.SetWidths(widths);
                                table.HorizontalAlignment = Element.ALIGN_CENTER;

                                string[] HeaderSeq4 = { "SNo","Agency", "Registered on", "Email", "Name", "DOB", "Mobile", "Language", "# in House", "#  in Intake", "Last Logged in", "Submitted", "Confirmn #" };
                                for (int i = 0; i < HeaderSeq4.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //cell.FixedHeight = 15f;
                                    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table.AddCell(cell);
                                }
                            }
                            else
                            {
                                table = new PdfPTable(12);
                                table.TotalWidth = 780f;
                                table.WidthPercentage = 100;
                                table.LockedWidth = true;
                                float[] widths = new float[] { 15f, 30f, 50f, 50f, 25f, 28f, 25f, 25f, 25f, 45f, 25f, 23f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                table.SetWidths(widths);
                                table.HorizontalAlignment = Element.ALIGN_CENTER;

                                string[] HeaderSeq4 = { "SNo", "Registered on", "Email", "Name", "DOB", "Mobile", "Language", "# in House", "#  in Intake", "Last Logged in", "Submitted", "Confirmn #" };
                                for (int i = 0; i < HeaderSeq4.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //cell.FixedHeight = 15f;
                                    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table.AddCell(cell);
                                }
                            }
                        }
                        else
                        {
                            table = new PdfPTable(12);
                            table.TotalWidth = 780f;
                            table.WidthPercentage = 100;
                            table.LockedWidth = true;
                            float[] widths = new float[] { 15f, 30f, 50f, 50f, 25f, 28f, 25f, 25f, 25f, 45f, 25f, 23f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table.SetWidths(widths);
                            table.HorizontalAlignment = Element.ALIGN_CENTER;

                            string[] HeaderSeq4 = { "SNo", "Registered on", "Email", "Name", "DOB", "Mobile", "Language", "# in House", "#  in Intake", "Last Logged in", "Submitted","Confirmn #" };
                            for (int i = 0; i < HeaderSeq4.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //cell.FixedHeight = 15f;
                                cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(cell);
                            }
                        }
                        
                        
                        int j = 1; int TotalHH = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            PdfPCell T0 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                            T0.HorizontalAlignment = Element.ALIGN_LEFT;
                            T0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T0);

                            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                            {
                                if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                                {
                                    PdfPCell T11 = new PdfPCell(new Phrase(dr["PIPREG_AGY"].ToString().Trim(), TableFont));
                                    T11.HorizontalAlignment = Element.ALIGN_LEFT;
                                    T11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(T11);
                                }
                            }

                            PdfPCell T10 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["PIPREG_DATE"].ToString().Trim()), TableFont));
                            T10.HorizontalAlignment = Element.ALIGN_LEFT;
                            T10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T10);

                            PdfPCell T3 = new PdfPCell(new Phrase(dr["PIPREG_EMAIL"].ToString().Trim(), TableFont));
                            T3.HorizontalAlignment = Element.ALIGN_LEFT;
                            T3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T3);

                            PdfPCell T1 = new PdfPCell(new Phrase(dr["PIPREG_FNAME"].ToString().Trim() + " " + dr["PIPREG_LNAME"].ToString().Trim(), TableFont));
                            T1.HorizontalAlignment = Element.ALIGN_LEFT;
                            T1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T1);

                            PdfPCell T2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["PIPREG_DOB"].ToString().Trim()), TableFont));
                            T2.HorizontalAlignment = Element.ALIGN_LEFT;
                            T2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T2);

                           

                            MaskedTextBox mskPhn = new MaskedTextBox();
                            if (!string.IsNullOrEmpty(dr["PIPREG_MOBILE"].ToString().Trim()))
                            {
                                mskPhn.Mask = "(999) 000-0000";
                                mskPhn.Text = dr["PIPREG_MOBILE"].ToString().Trim();
                            }
                            PdfPCell T4 = new PdfPCell(new Phrase(mskPhn.Text, TableFont));
                            T4.HorizontalAlignment = Element.ALIGN_LEFT;
                            T4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T4);



                            string Language = string.Empty;
                            Language = getLanguagedesc(dr["PIPREG_PREFD_LANG"].ToString().Trim());

                            PdfPCell T5 = new PdfPCell(new Phrase(Language, TableFont));
                            T5.HorizontalAlignment = Element.ALIGN_LEFT;
                            T5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T5);

                            PdfPCell T7 = new PdfPCell(new Phrase(dr["PIPREG_NO_INHH"].ToString().Trim(), TableFont));
                            T7.HorizontalAlignment = Element.ALIGN_LEFT;
                            T7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T7);

                            string Intakes = dr["INTAKES"].ToString().Trim();

                            if (dr["INTAKES"].ToString().Trim() == "0") Intakes = string.Empty;

                            PdfPCell T8 = new PdfPCell(new Phrase(Intakes, TableFont));
                            T8.HorizontalAlignment = Element.ALIGN_LEFT;
                            T8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T8);


                            PdfPCell T6 = new PdfPCell(new Phrase(dr["PIPREG_LAST_LOGDIN"].ToString().Trim(), TableFont));
                            T6.HorizontalAlignment = Element.ALIGN_LEFT;
                            T6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T6);

                            PdfPCell T9 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["PIPREG_SUBMITTED"].ToString().Trim()), TableFont));
                            T9.HorizontalAlignment = Element.ALIGN_LEFT;
                            T9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T9);

                            PdfPCell T12 = new PdfPCell(new Phrase(dr["PIPREG_CONFNO"].ToString().Trim(), TableFont));
                            T12.HorizontalAlignment = Element.ALIGN_LEFT;
                            T12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(T12);


                            if (!string.IsNullOrEmpty(dr["PIPREG_NO_INHH"].ToString().Trim()))
                                TotalHH = TotalHH + int.Parse(dr["PIPREG_NO_INHH"].ToString().Trim());

                            j++;
                        }

                        PdfPCell S00 = new PdfPCell(new Phrase("", TableFont));
                        S00.HorizontalAlignment = Element.ALIGN_LEFT;
                        S00.FixedHeight = 20f;
                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        {
                            if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                                S00.Colspan = 13;
                            else S00.Colspan = 12;
                        }
                        else
                            S00.Colspan = 12;
                        S00.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S00);

                        PdfPCell S0 = new PdfPCell(new Phrase("", TableFont));
                        S0.HorizontalAlignment = Element.ALIGN_LEFT;
                        S0.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S0);

                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        {
                            if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                            {
                                PdfPCell S10 = new PdfPCell(new Phrase("", TableFont));
                                S10.HorizontalAlignment = Element.ALIGN_LEFT;
                                S10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(S10);
                            }
                        }

                        PdfPCell S1 = new PdfPCell(new Phrase("Total Users Registered:", TableFont));
                        S1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S1);

                        PdfPCell S2 = new PdfPCell(new Phrase((j - 1).ToString(), TableFont));
                        S2.HorizontalAlignment = Element.ALIGN_LEFT;
                        S2.Colspan = 10;
                        S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S2);



                        PdfPCell S3 = new PdfPCell(new Phrase("", TableFont));
                        S3.HorizontalAlignment = Element.ALIGN_LEFT;
                        S3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S3);

                        if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        {
                            if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                            {
                                PdfPCell S10 = new PdfPCell(new Phrase("", TableFont));
                                S10.HorizontalAlignment = Element.ALIGN_LEFT;
                                S10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(S10);
                            }
                        }

                        PdfPCell S4 = new PdfPCell(new Phrase("Total # in House: ", TableFont));
                        S4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        S4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S4);

                        PdfPCell S5 = new PdfPCell(new Phrase(TotalHH.ToString(), TableFont));
                        S5.HorizontalAlignment = Element.ALIGN_LEFT;
                        S5.Colspan = 10;
                        S5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(S5);

                        document.Add(table);
                    }

                    if (chkbExcel.Checked) OnSaveExcel_Report(dt, PdfName);

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();
            }
        }

        private void PrintHeaderPage(Document document)
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
            iTextSharp.text.Font fnttimesRoman_Italic = new iTextSharp.text.Font(bf_TimesRomanI, 9, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#333333")));

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
            Headertable.TotalWidth = 550f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 12f, 88f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblFontBold));
            //row1.HorizontalAlignment = Element.ALIGN_CENTER;
            //row1.Colspan = 2;
            //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row1);

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
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


            //string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            //if (Agency != "**") Agy = "Agency : " + Agency;
            //if (Depart != "**") Dept = "Dept : " + Depart;
            //if (Program != "**") Prg = "Program : " + Program;
            //if (CmbYear.Visible == true)
            //    Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim()=="I")
            {

                PdfPCell cell_Content_Title1 = new PdfPCell(new Phrase("  " + lblAgency.Text.Trim() + "", TableFont));
                cell_Content_Title1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Title1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Title1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Title1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                cell_Content_Title1.PaddingBottom = 5;
                Headertable.AddCell(cell_Content_Title1);

                PdfPCell cell_Content_Desc1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Text.ToString().Trim(), TableFont));
                cell_Content_Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_Content_Desc1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                cell_Content_Desc1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                cell_Content_Desc1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                cell_Content_Desc1.PaddingBottom = 5;
                Headertable.AddCell(cell_Content_Desc1);
            }
           
            PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + "Registered", TableFont));
            cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title2);

            PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase("From: " + dtpFrmDate.Text.Trim() + "    To: " + dtpToDt.Text.Trim(), TableFont));
            cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc2);

            //PdfPCell R3 = new PdfPCell(new Phrase("Enroll Status :" + ((Captain.Common.Utilities.ListItem)cmbEntlStat.SelectedItem).Text.ToString(), TableFont));
            //R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //R3.Colspan = 2;
            //R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R3);

            document.Add(Headertable);


            PdfPTable Footertable = new PdfPTable(2);
            Footertable.TotalWidth = 550f;
            Footertable.WidthPercentage = 100;
            Footertable.LockedWidth = true;
            float[] Footerwidths = new float[] { 80f, 25f };
            Footertable.SetWidths(Footerwidths);
            Footertable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cellTitle1 = new PdfPCell(new Phrase("Generated By : ", TableFont));
            cellTitle1.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellTitle1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellTitle1.PaddingTop = 12;
            Footertable.AddCell(cellTitle1);

            PdfPCell cellDesc1 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic));
            cellDesc1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellDesc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellDesc1.PaddingTop = 12;
            Footertable.AddCell(cellDesc1);

            PdfPCell cellTitle2 = new PdfPCell(new Phrase("Generated On : ", TableFont));
            cellTitle2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellTitle2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Footertable.AddCell(cellTitle2);

            PdfPCell cellDesc2 = new PdfPCell(new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic));
            cellDesc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellDesc2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Footertable.AddCell(cellDesc2);
            document.Add(Footertable);
        }

        
        public DataTable GETPIPREGDATABYDATE(string FromDate, string Todate, string Agency,string AGY)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPB0003_GET";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (FromDate != string.Empty)
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    if (Todate != string.Empty)
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@Agency", Agency);

                    if (AGY != string.Empty)
                        cmd.Parameters.AddWithValue("@Agy", AGY);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public string getLanguagedesc(string Code)
        {
            string Desc = string.Empty;

            switch(Code.Trim())
            {
                case "af": Desc = "Afrikaans"; break;
                case "sq": Desc = "Albanian"; break;
                case "am": Desc = "Amharic"; break;
                case "ar": Desc = "Arabic"; break;
                case "hy": Desc = "Armenian"; break;
                case "az": Desc = "Azerbaijani"; break;
                case "eu": Desc = "Basque"; break;
                case "be": Desc = "Belarusian"; break;
                case "bn": Desc = "Bengali"; break;
                case "bs": Desc = "Bosnian"; break;
                case "bg": Desc = "Bulgarian"; break;
                case "ca": Desc = "Catalan"; break;
                case "ceb": Desc = "Cebuano"; break;
                case "zh-CN":
                case "zh": Desc = "Chinese (Simplified)"; break;
                case "zh-TW ": Desc = "Chinese (Traditional)"; break;
                case "co": Desc = "Corsican"; break;
                case "hr": Desc = "Croatian"; break;
                case "cs": Desc = "Czech"; break;
                case "da": Desc = "Danish"; break;
                case "nl": Desc = "Dutch"; break;
                case "en": Desc = "English"; break;
                case "eo": Desc = "Esperanto"; break;
                case "et": Desc = "Estonian"; break;
                case "fi": Desc = "Finnish"; break;
                case "fr": Desc = "French"; break;
                case "fy": Desc = "Frisian"; break;
                case "gl": Desc = "Galician"; break;
                case "ka": Desc = "Georgian"; break;
                case "de": Desc = "German"; break;
                case "el": Desc = "Greek"; break;
                case "gu": Desc = "Gujarati"; break;
                case "ht": Desc = "Haitian Creole"; break;
                case "ha": Desc = "Hausa"; break;
                case "haw": Desc = "Hawaiian"; break;
                case "iw":
                case "he": Desc = "Hebrew"; break;
                case "hi": Desc = "Hindi"; break;
                case "hmn": Desc = "Hmong"; break;
                case "hu": Desc = "Hungarian"; break;
                case "is": Desc = "Icelandic"; break;
                case "ig": Desc = "Igbo"; break;
                case "id": Desc = "Indonesian"; break;
                case "ga": Desc = "Irish"; break;
                case "it": Desc = "Italian"; break;
                case "ja": Desc = "Japanese"; break;
                case "jv": Desc = "Javanese"; break;
                case "kn": Desc = "Kannada"; break;
                case "kk": Desc = "Kazakh"; break;
                case "km": Desc = "Khmer"; break;
                case "rw": Desc = "Kinyarwanda"; break;
                case "ko": Desc = "Korean"; break;
                case "ku": Desc = "Kurdish"; break;
                case "ky": Desc = "Kyrgyz"; break;
                case "lo": Desc = "Lao"; break;
                case "la": Desc = "Latin"; break;
                case "lv": Desc = "Latvian"; break;
                case "lt": Desc = "Lithuanian"; break;
                case "lb": Desc = "Luxembourgish"; break;
                case "mk": Desc = "Macedonian"; break;
                case "mg": Desc = "Malagasy"; break;
                case "ms": Desc = "Malay"; break;
                case "ml": Desc = "Malayalam"; break;
                case "mt": Desc = "Maltese"; break;
                case "mi": Desc = "Maori"; break;
                case "mr": Desc = "Marathi"; break;
                case "mn": Desc = "Mongolian"; break;
                case "my": Desc = "Myanmar (Burmese)"; break;
                case "ne": Desc = "Nepali"; break;
                case "no": Desc = "Norwegian"; break;
                case "ny": Desc = "Nyanja (Chichewa)"; break;
                case "or": Desc = "Odia (Oriya)"; break;
                case "ps": Desc = "Pashto"; break;
                case "fa": Desc = "Persian"; break;
                case "pl": Desc = "Polish"; break;
                case "pt": Desc = "Portuguese (Portugal, Brazil)"; break;
                case "pa": Desc = "Punjabi"; break;
                case "ro": Desc = "Romanian"; break;
                case "ru": Desc = "Russian"; break;
                case "sm": Desc = "Samoan"; break;
                case "gd": Desc = "Scots Gaelic"; break;
                case "sr": Desc = "Serbian"; break;
                case "st": Desc = "Sesotho"; break;
                case "sn": Desc = "Shona"; break;
                case "sd": Desc = "Sindhi"; break;
                case "si": Desc = "Sinhala (Sinhalese)"; break;
                case "sk": Desc = "Slovak"; break;
                case "sl": Desc = "Slovenian"; break;
                case "so": Desc = "Somali"; break;
                case "es": Desc = "Spanish"; break;
                case "su": Desc = "Sundanese"; break;
                case "sw": Desc = "Swahili"; break;
                case "sv": Desc = "Swedish"; break;
                case "tl": Desc = "Tagalog (Filipino)"; break;
                case "tg": Desc = "Tajik"; break;
                case "ta": Desc = "Tamil"; break;
                case "tt": Desc = "Tatar"; break;
                case "te": Desc = "Telugu"; break;
                case "th": Desc = "Thai"; break;
                case "tr": Desc = "Turkish"; break;
                case "tk": Desc = "Turkmen"; break;
                case "uk": Desc = "Ukrainian"; break;
                case "ur": Desc = "Urdu"; break;
                case "ug": Desc = "Uyghur"; break;
                case "uz": Desc = "Uzbek"; break;
                case "vi": Desc = "Vietnamese"; break;
                case "cy": Desc = "Welsh"; break;
                case "xh": Desc = "Xhosa"; break;
                case "yi": Desc = "Yiddish"; break;
                case "yo": Desc = "Yoruba"; break;
                case "zu": Desc = "Zulu"; break;
                //case "af": Desc = "Afrikaans"; break;
                //case "af": Desc = "Afrikaans"; break;
                //case "af": Desc = "Afrikaans"; break;
                //case "af": Desc = "Afrikaans"; break;
            }

            return Desc;
        }

        //private void OnExcel_Report(DataTable dt, string pdfname)
        //{
        //    PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = PdfName + ".xls";

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


        //    //if (!string.IsNullOrEmpty(Random_Filename))
        //    //    PdfName = Random_Filename;
        //    //else
        //    //    PdfName += ".xls";


        //    ExcelDocument xlWorkSheet = new ExcelDocument();

        //    xlWorkSheet.ColumnWidth(0, 0);
        //    xlWorkSheet.ColumnWidth(1, 50);
        //    xlWorkSheet.ColumnWidth(2, 200);
        //    xlWorkSheet.ColumnWidth(3, 100);
        //    xlWorkSheet.ColumnWidth(4, 200);
        //    xlWorkSheet.ColumnWidth(5, 100);
        //    xlWorkSheet.ColumnWidth(6, 100);
        //    xlWorkSheet.ColumnWidth(7, 150);
        //    xlWorkSheet.ColumnWidth(8, 150);
        //    xlWorkSheet.ColumnWidth(9, 100);


        //    //List<CommonEntity> preassessMasterEntity = _model.lookupDataAccess.GetDimension();

        //    //preassessMasterEntity = preassessMasterEntity.OrderBy(U => Convert.ToInt32(U.Code)).ToList();
        //    //int intcolumn = 3;
        //    //for (int i = 0; i < (dt.Columns.Count - 2); i++)
        //    //{
        //    //    xlWorkSheet.ColumnWidth(intcolumn, 100);
        //    //    intcolumn = intcolumn + 1;
        //    //}
        //    //xlWorkSheet.ColumnWidth(intcolumn, 100);
        //    int excelcolumn = 0;


        //    try
        //    {

        //        //excelcolumn = excelcolumn + 1;
        //        xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 1, "SNo");
        //        xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 2, "Name");
        //        xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 3, "DOB");
        //        xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 4, "Email");
        //        xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 5, "Mobile");
        //        xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 6, "Language");
        //        xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 7, "Registration Date");
        //        xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 8, "Last Logged in");
        //        xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
        //        xlWorkSheet.WriteCell(excelcolumn, 9, "# in house");


        //        //intcolumn = 3;
        //        //foreach (DataColumn dc in dt.Columns)
        //        //{
        //        //    if (dc.ColumnName.ToString() != "PWR_NAME_IX_FIRST" && dc.ColumnName.ToString() != "PWR_NAME_IX_LAST" && dc.ColumnName.ToString() != "PWR_NAME_IX_MI" && dc.ColumnName.ToString() != "PWR_EMPLOYEE_NO")
        //        //    {
        //        //        xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
        //        //        xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
        //        //        xlWorkSheet.WriteCell(excelcolumn, intcolumn, dc.ColumnName.Trim());
        //        //        intcolumn = intcolumn + 1;
        //        //    }
        //        //}

        //        int Excelrow = 1;
        //        int j = 1; int TotalHH = 0;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            excelcolumn = excelcolumn + 1;
        //            xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Right;
        //            xlWorkSheet.WriteCell(excelcolumn, 1, j.ToString());
        //            xlWorkSheet.WriteCell(excelcolumn, 2, dr["PIPREG_LNAME"].ToString().Trim() + " " + dr["PIPREG_FNAME"].ToString().Trim());
        //            xlWorkSheet.WriteCell(excelcolumn, 3, LookupDataAccess.Getdate(dr["PIPREG_DOB"].ToString().Trim()));
        //            xlWorkSheet.WriteCell(excelcolumn, 4, dr["PIPREG_EMAIL"].ToString().Trim());
        //            MaskedTextBox mskPhn = new MaskedTextBox();
        //            if (!string.IsNullOrEmpty(dr["PIPREG_MOBILE"].ToString().Trim()))
        //            {
        //                mskPhn.Mask = "(999) 000-0000";
        //                mskPhn.Text = dr["PIPREG_MOBILE"].ToString().Trim();
        //            }
        //            xlWorkSheet.WriteCell(excelcolumn, 5, mskPhn.Text);

        //            string Language = string.Empty;
        //            Language = getLanguagedesc(dr["PIPREG_PREFD_LANG"].ToString().Trim());

        //            xlWorkSheet.WriteCell(excelcolumn, 6, Language);

        //            xlWorkSheet.WriteCell(excelcolumn, 7, LookupDataAccess.Getdate(dr["PIPREG_DATE"].ToString().Trim()));
        //            xlWorkSheet.WriteCell(excelcolumn, 8, dr["PIPREG_LAST_LOGDIN"].ToString().Trim());
        //            xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Right;
        //            xlWorkSheet.WriteCell(excelcolumn, 9, dr["PIPREG_NO_INHH"].ToString().Trim());

        //            if (!string.IsNullOrEmpty(dr["PIPREG_NO_INHH"].ToString().Trim()))
        //                TotalHH = TotalHH + int.Parse(dr["PIPREG_NO_INHH"].ToString().Trim());

        //            j++;

        //        }

        //        excelcolumn = excelcolumn + 3;
        //        xlWorkSheet.WriteCell(excelcolumn, 2, "Total Users Registered");
        //        xlWorkSheet.WriteCell(excelcolumn, 3, (j - 1).ToString());

        //        excelcolumn = excelcolumn + 1;
        //        xlWorkSheet.WriteCell(excelcolumn, 2, "Total # in House");
        //        xlWorkSheet.WriteCell(excelcolumn, 3, TotalHH.ToString());


        //        FileStream stream = new FileStream(PdfName, FileMode.Create);

        //        xlWorkSheet.Save(stream);
        //        stream.Close();

        //    }
        //    catch (Exception ex) { }


        //}

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
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



        }

        private void PIPB0003_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }

        private void OnSaveExcel_Report(DataTable dt, string pdfname)
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


            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);
           


         

            try
            {
                Worksheet sheet = book.Worksheets.Add("Parameters");

                #region Header Page
                WorksheetColumn columnHead =  sheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                sheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = sheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172";
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
                cell.StyleID = "s170";
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
                cell.StyleID = "s171";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s142";
                // -----------------------------------------------
                WorksheetRow Row2Head = sheet.Table.Rows.Add();
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
                WorksheetRow Row3Head = sheet.Table.Rows.Add();
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
                WorksheetRow Row4Head = sheet.Table.Rows.Add();
                Row4Head.Height = 12;
                Row4Head.AutoFitHeight = false;
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row5Head = sheet.Table.Rows.Add();
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
                WorksheetRow Row6Head = sheet.Table.Rows.Add();
                Row6Head.Height = 12;
                Row6Head.AutoFitHeight = false;
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                //WorksheetRow Row7Head = sheet.Table.Rows.Add();
                //Row7Head.Height = 12;
                //Row7Head.AutoFitHeight = false;
                //cell = Row7Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row7Head.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row7Head.Cells.Add();
                //cell.StyleID = "m2611536909324";
                //cell.Data.Type = DataType.String;
                //cell.Data.Text = "            Agency: " + Agency + " , Department : " + Depart + " , Program : " + Program;
                //cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row8 = sheet.Table.Rows.Add();
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
                WorksheetRow Row9 = sheet.Table.Rows.Add();
                Row9.Height = 12;
                Row9.AutoFitHeight = false;
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s143";
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s170";
                cell = Row9.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row10 = sheet.Table.Rows.Add();
                Row10.Height = 12;
                Row10.AutoFitHeight = false;
                cell = Row10.Cells.Add();
                cell.StyleID = "s139";
                cell = Row10.Cells.Add();
                cell.StyleID = "s143";
                cell = Row10.Cells.Add();
                cell.StyleID = "m2611540549592";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row11 = sheet.Table.Rows.Add();
                Row11.Height = 12;
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
                string SalCal = string.Empty;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim() == "I")
                {
                    SalCal = ((Utilities.ListItem)CmbAgency.SelectedItem).Text.Trim();
                    WorksheetRow Row15Head = sheet.Table.Rows.Add();
                    Row15Head.Height = 12;
                    Row15Head.AutoFitHeight = false;
                    cell = Row15Head.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Row15Head.Cells.Add();
                    cell.StyleID = "s143";
                    Row15Head.Cells.Add("            Type", DataType.String, "s144");
                    Row15Head.Cells.Add(" : " + SalCal, DataType.String, "s169");
                    cell = Row15Head.Cells.Add();
                    cell.StyleID = "s145";
                }
                // -----------------------------------------------
                //WorksheetRow Row16Head = sheet.Table.Rows.Add();
                //Row16Head.Height = 12;
                //Row16Head.AutoFitHeight = false;
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row16Head.Cells.Add();
                //cell.StyleID = "s145";
                
                // -----------------------------------------------
                WorksheetRow Row12 = sheet.Table.Rows.Add();
                Row12.Height = 12;
                Row12.AutoFitHeight = false;
                cell = Row12.Cells.Add();
                cell.StyleID = "s139";
                cell = Row12.Cells.Add();
                cell.StyleID = "s143";
                Row12.Cells.Add("            Registered", DataType.String, "s144");
                Row12.Cells.Add(": From " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtpFrmDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                            + "    To " +
                                            CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtpToDt.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), DataType.String, "s169");
                cell = Row12.Cells.Add();
                cell.StyleID = "s145";
                //WorksheetRow Row18 = sheet.Table.Rows.Add();
                //Row18.Height = 12;
                //Row18.AutoFitHeight = false;
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s143";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s139";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s170";
                //cell = Row18.Cells.Add();
                //cell.StyleID = "s145";
                // -----------------------------------------------
                // -----------------------------------------------
                WorksheetRow Row24 = sheet.Table.Rows.Add();
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
                WorksheetRow Row25 = sheet.Table.Rows.Add();
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
                WorksheetRow Row26Head = sheet.Table.Rows.Add();
                Row26Head.Height = 12;
                Row26Head.AutoFitHeight = false;
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row27Head = sheet.Table.Rows.Add();
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
                WorksheetRow Row28 = sheet.Table.Rows.Add();
                Row28.Height = 12;
                Row28.AutoFitHeight = false;
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s143";
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s170";
                cell = Row28.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row29 = sheet.Table.Rows.Add();
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
                WorksheetRow Row30 = sheet.Table.Rows.Add();
                Row30.Height = 12;
                Row30.AutoFitHeight = false;
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s143";
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s170";
                cell = Row30.Cells.Add();
                cell.StyleID = "s145";
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

                #endregion

                

                sheet = book.Worksheets.Add("Details");
                sheet.Table.DefaultRowHeight = 13.2F;
                sheet.Table.DefaultColumnWidth = 50.4F;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        sheet.Table.ExpandedColumnCount = 13;
                    }
                    else
                        sheet.Table.ExpandedColumnCount = 12;
                }
                else
                    sheet.Table.ExpandedColumnCount = 12;
                //sheet.Table.ExpandedRowCount = 33;
                sheet.Table.FullColumns = 1;
                sheet.Table.FullRows = 1;
                WorksheetColumn column0 = sheet.Table.Columns.Add();
                column0.Width = 43;
                column0.StyleID = "s31";
                if(BaseForm.BaseAgencyControlDetails.PIPSwitch=="I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        WorksheetColumn column11 = sheet.Table.Columns.Add();
                        column11.Width = 43;
                        column11.StyleID = "s31";
                    }
                }
                WorksheetColumn column1 = sheet.Table.Columns.Add();
                column1.Width = 85;
                column1.StyleID = "s32";
                WorksheetColumn column2 = sheet.Table.Columns.Add();
                column2.Width = 144;
                column2.StyleID = "s32";
                WorksheetColumn column3 = sheet.Table.Columns.Add();
                column3.Width = 115;
                column3.StyleID = "s31";
                WorksheetColumn column4 = sheet.Table.Columns.Add();
                column4.Width = 70;
                column4.StyleID = "s31";
                WorksheetColumn column5 = sheet.Table.Columns.Add();
                column5.Width = 75;
                column5.StyleID = "s31";
                sheet.Table.Columns.Add(114);
                sheet.Table.Columns.Add(52);
                sheet.Table.Columns.Add(55);
                sheet.Table.Columns.Add(120);
                WorksheetColumn column10 = sheet.Table.Columns.Add();
                column10.Width = 85;
                column10.StyleID = "s33";
                sheet.Table.Columns.Add(110);

                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Height = 21;
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch.Trim() == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() != "**")
                        Row0.Cells.Add("PIP Registrations for "+ ((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Text.ToString().Trim()+" from " + dtpFrmDate.Value.ToShortDateString() + " to " + dtpToDt.Value.ToShortDateString(), DataType.String, "s16");
                    else
                        Row0.Cells.Add("PIP Registrations from " + dtpFrmDate.Value.ToShortDateString() + " to " + dtpToDt.Value.ToShortDateString(), DataType.String, "s16");
                }
                else
                    Row0.Cells.Add("PIP Registrations from " + dtpFrmDate.Value.ToShortDateString() + " to " + dtpToDt.Value.ToShortDateString(), DataType.String, "s16");
                
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        cell = Row0.Cells.Add();
                        cell.StyleID = "s17";
                    }
                }
                cell = Row0.Cells.Add();
                cell.StyleID = "s17";
                cell = Row0.Cells.Add();
                cell.StyleID = "s17";
                cell = Row0.Cells.Add();
                cell.StyleID = "s17";
                cell = Row0.Cells.Add();
                cell.StyleID = "s17";
                cell = Row0.Cells.Add();
                cell.StyleID = "s18";
                cell = Row0.Cells.Add();
                cell.StyleID = "s19";
                cell = Row0.Cells.Add();
                cell.StyleID = "s19";
                cell = Row0.Cells.Add();
                cell.StyleID = "s19";
                cell = Row0.Cells.Add();
                cell.StyleID = "s19";
                cell = Row0.Cells.Add();
                cell.StyleID = "s20";
                cell = Row0.Cells.Add();
                cell.StyleID = "s20";

                // ---------------------HEADER--------------------------
                WorksheetRow Row1 = sheet.Table.Rows.Add();
                Row1.Height = 16;
                Row1.AutoFitHeight = false;
                Row1.Cells.Add("Sno", DataType.String, "s22");
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        Row1.Cells.Add("Agency", DataType.String, "s22");
                    }
                }
                Row1.Cells.Add("Registered on", DataType.String, "s22");
                Row1.Cells.Add("Email", DataType.String, "s22");
                Row1.Cells.Add("Name", DataType.String, "s21");
                Row1.Cells.Add("DOB", DataType.String, "s22");
                Row1.Cells.Add("Mobile", DataType.String, "s22");
                Row1.Cells.Add("Language", DataType.String, "s21");
                Row1.Cells.Add("# in House", DataType.String, "s21");
                Row1.Cells.Add("#  in Intake", DataType.String, "s21");
                Row1.Cells.Add("Last Logged In", DataType.String, "s21");
                Row1.Cells.Add("Submitted", DataType.String, "s21");
                Row1.Cells.Add("Confirmn #", DataType.String, "s21");

                int j = 1; int TotalHH = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    WorksheetRow Row2 = sheet.Table.Rows.Add();
                    Row2.Height = 18;
                    Row2.AutoFitHeight = false;

                    Row2.Cells.Add(j.ToString(), DataType.Number, "s24");
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                    {
                        if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                        {
                            Row2.Cells.Add(dr["PIPREG_AGY"].ToString().Trim(), DataType.String, "s24");
                        }
                    }
                    Row2.Cells.Add(LookupDataAccess.Getdate(dr["PIPREG_DATE"].ToString()), DataType.String, "s28");
                    Row2.Cells.Add(dr["PIPREG_EMAIL"].ToString().Trim(), DataType.String, "s26");
                    Row2.Cells.Add(dr["PIPREG_FNAME"].ToString().Trim() + " " + dr["PIPREG_LNAME"].ToString().Trim(), DataType.String, "s26");
                    Row2.Cells.Add(LookupDataAccess.Getdate(dr["PIPREG_DOB"].ToString().Trim()), DataType.String, "s26");
                    MaskedTextBox mskPhn = new MaskedTextBox();
                    if (!string.IsNullOrEmpty(dr["PIPREG_MOBILE"].ToString().Trim()))
                    {
                        mskPhn.Mask = "(999) 000-0000";
                        mskPhn.Text = dr["PIPREG_MOBILE"].ToString().Trim();
                    }

                    Row2.Cells.Add(mskPhn.Text, DataType.String, "s26");

                    string Language = string.Empty;
                    Language = getLanguagedesc(dr["PIPREG_PREFD_LANG"].ToString().Trim());

                    Row2.Cells.Add(Language, DataType.String, "s26");
                    Row2.Cells.Add(dr["PIPREG_NO_INHH"].ToString().Trim(), DataType.Number, "s41");
                    string Intakes = dr["INTAKES"].ToString().Trim();
                    if (dr["INTAKES"].ToString().Trim() == "0") Intakes = string.Empty;
                    Row2.Cells.Add(dr["INTAKES"].ToString().Trim(), DataType.Number, "s41");
                    //cell = Row2.Cells.Add();
                    //cell.StyleID = "s26";
                    Row2.Cells.Add(dr["PIPREG_LAST_LOGDIN"].ToString().Trim(), DataType.String, "s27");
                    Row2.Cells.Add(LookupDataAccess.Getdate(dr["PIPREG_SUBMITTED"].ToString().Trim()), DataType.String, "s28");
                    Row2.Cells.Add(dr["PIPREG_CONFNO"].ToString().Trim(), DataType.String, "s27");


                    if (!string.IsNullOrEmpty(dr["PIPREG_NO_INHH"].ToString().Trim()))
                        TotalHH = TotalHH + int.Parse(dr["PIPREG_NO_INHH"].ToString().Trim());

                    j++;

                }

                WorksheetRow Row3 = sheet.Table.Rows.Add();
                Row3 = sheet.Table.Rows.Add();
                Row3.Cells.Add("", DataType.String, "s26");
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        Row3.Cells.Add("", DataType.String, "s26");
                    }
                }
                Row3.Cells.Add("Total Users Registered", DataType.String, "s26");
                Row3.Cells.Add((j - 1).ToString(), DataType.Number, "s26");

                WorksheetRow Row4 = sheet.Table.Rows.Add();
                Row4.Cells.Add("", DataType.String, "s26");
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (((Captain.Common.Utilities.ListItem)CmbAgency.SelectedItem).Value.ToString().Trim() == "**")
                    {
                        Row4.Cells.Add("", DataType.String, "s26");
                    }
                }
                Row4.Cells.Add("Total # in House", DataType.String, "s26");
                Row4.Cells.Add(TotalHH.ToString(), DataType.Number, "s26");



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


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();


            }
            catch (Exception ex) { }


        }


    }
}