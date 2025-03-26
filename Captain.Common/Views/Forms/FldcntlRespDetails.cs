#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;
using Captain.Common.Model.Objects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Captain.Common.Views.Forms.Base;
using System.IO;
using Captain.Common.Utilities;
using Microsoft.Win32;
using System.Data.SqlClient;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class FldcntlRespDetails : Form
    {
        private CaptainModel _model = null;
        private PrivilegesControl _screenPrivileges = null;
        private ErrorProvider _errorProvider = null;
        public FldcntlRespDetails(string strQuesCode, string strQuesDesc, string strType, BaseForm baseForm, string strQuestionTypes)
        {
            //BaseForm = baseForm;
            //Privileges = privileges;
            _model = new CaptainModel();
            InitializeComponent();
            BaseForm = baseForm;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            propQuesCode = strQuesCode;
            propQuesDesc = txtQuesDesc.Text = strQuesDesc;
            propQuesType = strType;
            propReportType = strQuestionTypes;
            custRespDetails = _model.FieldControls.GetCustRespByCustCode(strQuesCode);
            listAddcustItems = _model.FieldControls.GETCUSTRESPAPPCOUNT(strQuesCode, strQuestionTypes, string.Empty);
            int intrespcount = 0;
            int introwindex = 0;
            if (strType == "D" || strType == "C")
            {
                btnRespSave.Visible = true;
                bool booldata;

                foreach (CustRespEntity item in custRespDetails)
                {
                    intrespcount = listAddcustItems.FindAll(u => u.ACTMULTRESP.Trim() == item.DescCode.Trim()).Count;
                    booldata = item.RspStatus == "A" ? true : false;
                    introwindex = gvwResponses.Rows.Add(booldata, false, item.RespDesc, intrespcount, item.DescCode.Trim());
                    gvwResponses.Rows[introwindex].Tag = item;

                }

            }
            else
            {
                gvtStatus.Visible = false;
                btnRespSave.Visible = false;
                string strResponse = string.Empty;
                if (propQuesType == "N")
                {
                    strResponse = "Numeric Responses";
                }
                else if (propQuesType == "T")
                {
                    strResponse = "Date Responses";
                }
                else if (propQuesType == "X")
                {
                    strResponse = "Text Responses";
                }
                gvwResponses.Rows.Add(true, true, strResponse, listAddcustItems.Count, string.Empty);
            }
        }

        string propQuesCode { get; set; }
        string propQuesDesc { get; set; }
        string propQuesType { get; set; }
        public BaseForm BaseForm { get; set; }
        string strReportName { get; set; }
        List<CustRespEntity> custRespDetails { get; set; }
        List<AddCustEntity> listAddcustItems { get; set; }
        string propReportType { get; set; }

        private void btnResponseDetails_Click(object sender, EventArgs e)
        {

            PdfContentByte cb;
            int X_Pos, Y_Pos;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            StringBuilder strMstApplUpdate = new StringBuilder();
            string PdfName = "ResponseDetails";
            string Random_Filename = string.Empty;
            string propReportPath = _model.lookupDataAccess.GetReportPath();
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
            strReportName = PdfName;

            FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
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
            List<DataGridViewRow> SelectedgvRows = (from c in gvwResponses.Rows.Cast<DataGridViewRow>().ToList()
                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvCheck"]).Value.ToString().ToUpper().Equals("TRUE"))
                                                    select (DataGridViewRow)c).ToList();
            if (SelectedgvRows.Count > 0)
            {
                try
                {


                    PdfPTable hssb2109_Table = new PdfPTable(5);
                    hssb2109_Table.TotalWidth = 550f;
                    hssb2109_Table.WidthPercentage = 100;
                    hssb2109_Table.LockedWidth = true;
                    float[] widths = new float[] { 30f, 35f, 55f, 20f, 35f };
                    hssb2109_Table.SetWidths(widths);
                    hssb2109_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                    hssb2109_Table.HeaderRows = 1;


                    PdfPCell Header_1 = new PdfPCell(new Phrase("Hierarchy", TblFontBold));
                    Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Header_1.Border = iTextSharp.text.Rectangle.BOX;
                    hssb2109_Table.AddCell(Header_1);


                    PdfPCell HeaderTop1 = new PdfPCell(new Phrase("App", TblFontBold));
                    HeaderTop1.HorizontalAlignment = Element.ALIGN_CENTER;
                    HeaderTop1.Border = iTextSharp.text.Rectangle.BOX;
                    hssb2109_Table.AddCell(HeaderTop1);

                    if (propReportType == "EMS00030")
                    {
                        PdfPCell Header = new PdfPCell(new Phrase("Name", TblFontBold));
                        Header.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(Header);

                        PdfPCell HeaderFund = new PdfPCell(new Phrase("Fund", TblFontBold));
                        HeaderFund.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeaderFund.Border = iTextSharp.text.Rectangle.BOX;
                        hssb2109_Table.AddCell(HeaderFund);
                    }
                    else
                    {
                        PdfPCell Header = new PdfPCell(new Phrase("Name", TblFontBold));
                        Header.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header.Border = iTextSharp.text.Rectangle.BOX;
                        Header.Colspan = 2;
                        hssb2109_Table.AddCell(Header);
                    }
                    PdfPCell Header1 = new PdfPCell(new Phrase("Response", TblFontBold));
                    Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Header1.Border = iTextSharp.text.Rectangle.BOX;
                    hssb2109_Table.AddCell(Header1);



                    string strName = string.Empty;
                    string strRespdesc = string.Empty;
                    List<AddCustEntity> filteraddcustdata = new List<AddCustEntity>();
                    filteraddcustdata = listAddcustItems;
                    foreach (DataGridViewRow gvquesRows in SelectedgvRows)
                    {

                        if (propQuesType == "D" || propQuesType == "C")
                        {
                            filteraddcustdata = listAddcustItems.FindAll(u => u.ACTMULTRESP.ToString().Trim() == gvquesRows.Cells["gvtRespCode"].Value.ToString());
                            strRespdesc = gvquesRows.Cells["gvtResponse"].Value.ToString();
                            filteraddcustdata = filteraddcustdata.OrderBy(u => u.ACTAGENCY).ThenBy(u => u.ACTDEPT).ThenBy(u => u.ACTPROGRAM).ThenBy(u => u.ACTYEAR).ThenBy(u => u.ACTAPPNO).ToList();
                        }
                        foreach (AddCustEntity custdata in filteraddcustdata)
                        {

                            PdfPCell pdfAppldata = new PdfPCell(new Phrase(custdata.ACTAGENCY + custdata.ACTDEPT + custdata.ACTPROGRAM + custdata.ACTYEAR, TableFont));
                            pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfAppldata.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            hssb2109_Table.AddCell(pdfAppldata);

                            PdfPCell pdfAge = new PdfPCell(new Phrase(custdata.ACTAPPNO, TableFont));
                            pdfAge.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfAge.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2109_Table.AddCell(pdfAge);

                            //if (custdata.ACTSNPFAMILYSEQ == "9999999")
                            //    strName = "Applicant";
                            //else if (custdata.ACTSNPFAMILYSEQ == "8888888")
                            //    strName = "House Hold Member";
                            //else
                            //    strName = LookupDataAccess.GetMemberName(custdata.FirstName, custdata.MIName, custdata.LastName, BaseForm.BaseHierarchyCnFormat);

                            if (propReportType == "EMS00030")
                            {
                                PdfPCell pdfChildName = new PdfPCell(new Phrase(custdata.FirstName, TableFont));
                                pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2109_Table.AddCell(pdfChildName);

                                PdfPCell pdfChildNameFund = new PdfPCell(new Phrase(custdata.ACTSNPFAMILYSEQ, TableFont));
                                pdfChildNameFund.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfChildNameFund.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2109_Table.AddCell(pdfChildNameFund);


                            }
                            else
                            {
                                PdfPCell pdfChildName = new PdfPCell(new Phrase(custdata.FirstName, TableFont));
                                pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                pdfChildName.Colspan = 2;
                                hssb2109_Table.AddCell(pdfChildName);
                            }


                            //if (propQuesType == "D" || propQuesType == "C")
                            //{

                            //    //CustRespEntity custrespdata = custRespDetails.Find(u => u.DescCode.Trim() == custdata.ACTMULTRESP.Trim());
                            //    //if (custrespdata != null)
                            //    //    strRespdesc = custrespdata.RespDesc;
                            //}
                            if (propReportType == "CASE0061" || propReportType == "CASE0063")
                            {
                                if (propQuesType == "N" || propQuesType == "T" || propQuesType == "X")
                                {
                                    strRespdesc = custdata.ACTMULTRESP.ToString();
                                }
                            }
                            else
                            {
                                if (propQuesType == "N")
                                {
                                    strRespdesc = custdata.ACTNUMRESP.ToString();
                                }
                                else if (propQuesType == "T")
                                {
                                    strRespdesc = LookupDataAccess.Getdate(custdata.ACTDATERESP);
                                }
                                else if (propQuesType == "X")
                                {
                                    strRespdesc = custdata.ACTALPHARESP;
                                }
                            }
                            PdfPCell pdfGardian = new PdfPCell(new Phrase(strRespdesc, TableFont));
                            pdfGardian.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfGardian.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            hssb2109_Table.AddCell(pdfGardian);
                        }
                    }
                    if (hssb2109_Table.Rows.Count > 0)
                    {
                        PdfPCell pdfGardian = new PdfPCell(new Phrase("", TableFont));
                        pdfGardian.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        pdfGardian.Colspan = 5;
                        hssb2109_Table.AddCell(pdfGardian);
                        document.Add(hssb2109_Table);

                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
                document.Close();
                fs.Close();
                fs.Dispose();

                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
                else
                {
                    FrmViewer objfrm = new FrmViewer(PdfName);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;
                    objfrm.ShowDialog();
                }
            }
            else
            {
                AlertBox.Show("Please select atleast one Response", MessageBoxIcon.Warning);
            }
        }

        public string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }

        private void btnRespSave_Click(object sender, EventArgs e)
        {
            bool boolsuccess = false;
            int intreccount = 0;
            foreach (DataGridViewRow gvitem in gvwResponses.Rows)
            {
                CustRespEntity Entity = gvitem.Tag as CustRespEntity;
                if (Entity != null)
                {
                    string strstatus = gvitem.Cells["gvtStatus"].Value.ToString().ToUpper() == "TRUE" ? "A" : "I";
                    List<SqlParameter> sqlParamList = new List<SqlParameter>();
                    sqlParamList.Add(new SqlParameter("@Row_Type", "C"));
                    sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", Entity.ScrCode));
                    sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", Entity.ResoCode));
                    sqlParamList.Add(new SqlParameter("@RSP_SEQ", int.Parse(Entity.RespSeq)));
                    sqlParamList.Add(new SqlParameter("@RSP_DESC", Entity.RespDesc));
                    sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", Entity.DescCode));
                    sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", Entity.AddOpr));
                    sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", Entity.ChgOpr));
                    if (Entity.Points != string.Empty)
                        sqlParamList.Add(new SqlParameter("@RSP_POINTS", Entity.Points));
                    if (strstatus != string.Empty)
                        sqlParamList.Add(new SqlParameter("@RSP_STATUS", strstatus));
                    boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCUSTRESP(sqlParamList);
                    if (boolsuccess)
                        intreccount = intreccount + 1;
                }

            }
            if (intreccount > 0)
            {
                AlertBox.Show("Responses Status Updated Successfully");
                //CommonFunctions.MessageBoxDisplay("Responses status updated successfully.");
            }

        }
        public bool UpdateCUSTRESP(List<CustRespEntity> RespList, string New_CUST_Code_Code)
        {
            bool boolsuccess = false;

            try
            {
                foreach (CustRespEntity Entity in RespList)
                {
                    if (Entity.Changed == "Y" && !(string.IsNullOrEmpty(Entity.DescCode)) && !(string.IsNullOrEmpty(Entity.RespDesc)))
                    {
                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@Row_Type", Entity.RecType));
                        sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", Entity.ScrCode));
                        sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", New_CUST_Code_Code));
                        sqlParamList.Add(new SqlParameter("@RSP_SEQ", int.Parse(Entity.RespSeq)));
                        sqlParamList.Add(new SqlParameter("@RSP_DESC", Entity.RespDesc));
                        sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", Entity.DescCode));
                        sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", Entity.AddOpr));
                        sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", Entity.ChgOpr));
                        if (Entity.Points != string.Empty)
                            sqlParamList.Add(new SqlParameter("@RSP_POINTS", Entity.Points));

                        boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCUSTRESP(sqlParamList);
                    }
                }

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

    }
}