#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using iTextSharp.text.pdf;
using iTextSharp.text;
#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PIP00001EmailHistoryForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        string _email = "";
        public PIP00001EmailHistoryForm(BaseForm baseForm, List<PIPDocEntity> propDocentitylist, DataTable dtpipIntake, string strEmail, string strRegid)
        {
            InitializeComponent();

            _model = new CaptainModel();
            BaseForm = baseForm;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _propDocentitylist = propDocentitylist;
            dtPIPIntake = dtpipIntake;
            _email = strEmail;

            _prop_Reg_id = strRegid;
            cmbShow.Items.Add(new System.Web.UI.WebControls.ListItem("All", "A"));
            cmbShow.Items.Add(new System.Web.UI.WebControls.ListItem("Messages to Caseworker", "U"));
            cmbShow.Items.Add(new System.Web.UI.WebControls.ListItem("Messages from User", "W"));
            cmbShow.Items.Add(new System.Web.UI.WebControls.ListItem("Verifications", "V"));
            cmbShow.SelectedIndex = 0;

            _propAgency = "00";

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I") _propAgency = dtPIPIntake.Rows[0]["PIPREG_AGY"].ToString();

            this.Text = "Email History";
            fillPipEmailHistory("A");

        }

        public BaseForm BaseForm { get; set; }
        List<PIPDocEntity> _propDocentitylist { get; set; }
        public string _propAgency { get; set; }
        DataTable dtPIPIntake = new DataTable();

        public string _prop_Reg_id { get; set; }
        private void panel1_Click(object sender, EventArgs e)
        {

        }

        void fillPipEmailHistory(string strshowType)
        {
            DataTable dt = PIPDATA.PIPEMAILHIST(BaseForm.BaseLeanDataBaseConnectionString, _prop_Reg_id.ToString());
            DataView view = dt.DefaultView;
            view.Sort = "PIPMAILH_DATE_SENT DESC";
            DataTable sortedDate = view.ToTable();

            string strMiscType = string.Empty;
            gvwEmailDetails.SelectionChanged -= gvwEmailDetails_SelectionChanged;
            gvwEmailDetails.Rows.Clear();

            foreach (DataRow dr in sortedDate.Rows)
            {
                strMiscType = "Remarks";
                if (dr["PIPMAILH_UPD_ID"].ToString() == "W")
                    strMiscType = "Message to User";
                else if (dr["PIPMAILH_UPD_ID"].ToString() == "U")
                    strMiscType = "Message to Worker";


                if (strshowType == "A")
                {
                    int index = gvwEmailDetails.Rows.Add(false, dr["PIPMAILH_DATE_SENT"].ToString(), dr["PIPMAILH_SENT_BY"].ToString(), strMiscType, dr["PIPMAILH_MISC_MSG"].ToString(), dr["PIPMAILH_UPD_ID"].ToString(), dr["PIPMAILH_READ"].ToString());
                    gvwEmailDetails.Rows[index].Tag = dr;
                    if (dr["PIPMAILH_UPD_ID"].ToString() == "U" && dr["PIPMAILH_READ"].ToString() == "0")
                    {
                        gvwEmailDetails.Rows[index].DefaultCellStyle.Font = Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);//FontStyle.Bold;  = new Font(gvwEmailDetails.Rows[index].DefaultCellStyle.Font, FontStyle.Bold);
                    }
                }
                else
                {
                    if (strshowType == dr["PIPMAILH_UPD_ID"].ToString())
                    {
                        int index = gvwEmailDetails.Rows.Add(false, dr["PIPMAILH_DATE_SENT"].ToString(), dr["PIPMAILH_SENT_BY"].ToString(), strMiscType, dr["PIPMAILH_MISC_MSG"].ToString(), dr["PIPMAILH_UPD_ID"].ToString(), dr["PIPMAILH_READ"].ToString());
                        gvwEmailDetails.Rows[index].Tag = dr;
                        if (dr["PIPMAILH_UPD_ID"].ToString() == "U" && dr["PIPMAILH_READ"].ToString() == "0")
                        {
                            gvwEmailDetails.Rows[index].DefaultCellStyle.Font = Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);//FontStyle.Bold;  = new Font(gvwEmailDetails.Rows[index].DefaultCellStyle.Font, FontStyle.Bold);

                        }
                    }
                    else if ((strshowType == "V") && (dr["PIPMAILH_UPD_ID"].ToString() != "U" && dr["PIPMAILH_UPD_ID"].ToString() != "W"))
                    {
                        int index = gvwEmailDetails.Rows.Add(false, dr["PIPMAILH_DATE_SENT"].ToString(), dr["PIPMAILH_SENT_BY"].ToString(), strMiscType, dr["PIPMAILH_MISC_MSG"].ToString(), dr["PIPMAILH_UPD_ID"].ToString(), dr["PIPMAILH_READ"].ToString());
                        gvwEmailDetails.Rows[index].Tag = dr;
                    }
                }
            }
            if (gvwEmailDetails.Rows.Count > 0)
            {
                gvwEmailDetails.Rows[0].Selected = true;
            }
            gvwEmailDetails.SelectionChanged += gvwEmailDetails_SelectionChanged;
            if (gvwEmailDetails.Rows.Count > 0)
            {
                gvwEmailDetails_SelectionChanged(gvwEmailDetails, new EventArgs());
            }
            else
            {
                htmlBox1.Html = "<HTML></HTML>";
            }

        }



        void fillPipEmailHistoryWorker(DataTable dt)
        {

            DataView view = dt.DefaultView;
            view.Sort = "PIPMAILH_DATE_SENT DESC";
            DataTable sortedDate = view.ToTable();

            string strMiscType = string.Empty;

            foreach (DataRow dr in sortedDate.Rows)
            {

                if (dr["PIPMAILH_UPD_ID"].ToString() == "U")
                {
                    strMiscType = "Message to Worker";

                    int index = gvwEmailDetails.Rows.Add(false, dr["PIPMAILH_DATE_SENT"].ToString(), dr["PIPMAILH_SENT_BY"].ToString(), strMiscType, dr["PIPMAILH_MISC_MSG"].ToString());
                    gvwEmailDetails.Rows[index].Tag = dr;
                }
            }
            if (gvwEmailDetails.Rows.Count > 0)
            {
                gvwEmailDetails.Rows[0].Selected = true;
            }

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmailSend_Click(object sender, EventArgs e)
        {

        }

        private void gvwEmailDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (gvwEmailDetails.Rows.Count > 0)
            {

                if (gvwEmailDetails.SelectedRows[0].Selected)
                {
                    DataRow dr = gvwEmailDetails.SelectedRows[0].Tag as DataRow;
                    if (dr != null)
                    {
                        if (gvwEmailDetails.SelectedRows[0].Cells["gvtTypeofmail"].Value == "Remarks")
                        {


                            htmlBox1.Html = ReviewDocumentload(dr["PIPMAILH_UPD_ID"].ToString(), dr["PIPMAILH_VER_ID"].ToString());
                        }
                        else
                        {
                            htmlBox1.Html = "<p style='padding:5px;'>" + gvwEmailDetails.SelectedRows[0].Cells["gvtRemarks"].Value.ToString() + "</p>";

                        }
                        if (dr["PIPMAILH_UPD_ID"].ToString() == "U" && gvwEmailDetails.SelectedRows[0].Cells["gvtEmailRead"].Value.ToString() == "0")
                        {
                            gvwEmailDetails.SelectedRows[0].DefaultCellStyle.Font = Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);//FontStyle.Bold;  = new Font(gvwEmailDetails.Rows[index].DefaultCellStyle.Font, FontStyle.Bold);
                            PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, dr["PIPMAILH_ID"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, "READMSG", string.Empty);
                            gvwEmailDetails.SelectedRows[0].Cells["gvtEmailRead"].Value = "1";
                        }
                    }
                }
            }
        }

        private string ReviewDocumentload(string strDocIds, string strVerIds)
        {

            StringBuilder strcontent = new StringBuilder();
            strcontent.Append("<div style='padding-top:1px; padding:5px;'><table border = '1' style='border:1px solid #333; border-collapse:collapse;width:100%'><tr style='padding:5px; background-color:#D3D3D3; color:#000000; font-size:15px; font-weight:bold;'><th style = 'width:20%'> Name of the Member </th><th style = 'width:10%'>Date</th> <th style = 'width:25%'>Name of the Document</th><th style = 'width:5%'> Pass </th ><th style = 'width:40%'> Remarks </th></tr> ");

            foreach (DataRow dritem in dtPIPIntake.Rows)
            {
                List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dritem["PIP_ID"].ToString());
                string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                if (pipdoccount.Count > 0)
                {
                    int intfirst = 0; int intverdocfirst = 0;
                    string[] strDocarrids = strDocIds.Split(',');
                    string[] strverarrids = strVerIds.Split(',');
                    foreach (string drpipdoc in strDocarrids)
                    {
                        PIPDocEntity pipsingledoccount = pipdoccount.Find(u => u.PIPDOCUPLD_ID == drpipdoc);
                        if (pipsingledoccount != null)
                        {
                            List<PIPDocVerEntity> pipdocVerlist = PIPDATA.GETPIPDOCVER(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, drpipdoc, string.Empty);
                            pipdocVerlist = pipdocVerlist.FindAll(u => u.PIPDOCVER_MAIL_ON != string.Empty);
                            if (pipdocVerlist.Count > 0)
                            {
                                intverdocfirst = 0;
                                foreach (string strveritem in strverarrids)
                                {
                                    if (strveritem != string.Empty)
                                    {
                                        foreach (PIPDocVerEntity docveritem in pipdocVerlist)
                                        {
                                            string strdocname = string.Empty;
                                            string strAppname = string.Empty;
                                            if (strveritem == docveritem.PIPDOCVER_ID)
                                            {
                                                if (intfirst == 0)
                                                {
                                                    strdocname = pipsingledoccount.PIPDOCUPLD_DOCNAME;
                                                    strAppname = ApplicantName;
                                                    intfirst = intfirst + 1;
                                                    intverdocfirst = intverdocfirst + 1;
                                                }
                                                else
                                                {
                                                    if (intverdocfirst == 0)
                                                    {
                                                        strdocname = pipsingledoccount.PIPDOCUPLD_DOCNAME;
                                                        intverdocfirst = intverdocfirst + 1;
                                                    }

                                                }
                                                strcontent.Append("<tr style='font-size:14px'><td>" + strAppname + "</td>");
                                                strcontent.Append("<td>" + LookupDataAccess.Getdate(docveritem.PIPDOCVER_DATE) + "</td>");
                                                strcontent.Append("<td>" + strdocname + "</td>");
                                                if (docveritem.PIPDOCVER_STATUS.ToString() == "C")
                                                {
                                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:25px; color:#40ba0c'>&#10003;</span></td>");
                                                }
                                                else
                                                {
                                                    strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:25px; color:#ff0000'> &#215;</span></td>");
                                                }
                                                strcontent.Append("<td>" + docveritem.PIPDOCVER_Remarks + "</td></tr>");

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            strcontent.Append("</table></div>");
            return strcontent.ToString();
        }

        private void cmbShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbShow.Items.Count > 0)
            {

                fillPipEmailHistory(((System.Web.UI.WebControls.ListItem)cmbShow.SelectedItem).Value.ToString());
            }
        }

        private void SendEmailContent()
        {
            DataTable dtMailConfig = PIPDATA.PIPMAILCONFIG_GET(BaseForm.BaseLeanDataBaseConnectionString, "DOCUMENT");
            DataTable dtcontent = PIPDATA.PIPMAILS_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPIntake.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPIntake.Rows[0]["PIPREG_AGY"].ToString(), "DOV", string.Empty);

            if (dtMailConfig.Rows.Count > 0)
            {
                try
                {

                    string[] args = PIPDATA.PIPAGENCY_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPIntake.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPIntake.Rows[0]["PIPREG_AGY"].ToString());

                    MailMessage mail = new MailMessage();
                    mail.To.Add(dtPIPIntake.Rows[0]["PIPREG_EMAIL"].ToString());
                    mail.From = new MailAddress(dtMailConfig.Rows[0]["MAILCONFIG_EMAILID"].ToString(), args[2].ToString());
                    mail.Subject = "Message from Case worker";

                    mail.Body = createEmailBody(string.Empty, dtPIPIntake.Rows[0]["PIPREG_FNAME"].ToString(), dtPIPIntake.Rows[0]["PIPREG_LNAME"].ToString(), txtMessage.Text, dtcontent);
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = dtMailConfig.Rows[0]["MAILCONFIG_HOST"].ToString();

                    smtp.EnableSsl = true;

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                    NetworkCred.UserName = dtMailConfig.Rows[0]["MAILCONFIG_EMAILID"].ToString();

                    NetworkCred.Password = dtMailConfig.Rows[0]["MAILCONFIG_PASSWORD"].ToString();

                    smtp.UseDefaultCredentials = true;

                    smtp.Credentials = NetworkCred;

                    smtp.Port = int.Parse(dtMailConfig.Rows[0]["MAILCONFIG_PORT"].ToString());

                    smtp.Send(mail);
                    AlertBox.Show("Message sent Successfully");//CommonFunctions.MessageBoxDisplay("Success");
                    PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, dtPIPIntake.Rows[0]["PIPREG_ID"].ToString(), "W", string.Empty, BaseForm.UserID, string.Empty, txtMessage.Text);
                    this.Close();
                }
                catch (Exception ex)
                {

                    CommonFunctions.MessageBoxDisplay(ex.Message);
                }
            }
        }

        private string createEmailBody(string userName, string strFirsName, string strLastName, string message, DataTable dtcontent)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Application.MapPath("~\\Resources\\PIPDOCVERmailFormat.html")))
            {
                body = reader.ReadToEnd();

            }

            string strMainLogopath = string.Empty;
            string CompanyName = string.Empty;
            string Address = string.Empty;
            string City = string.Empty;
            string State = string.Empty;
            string Country = string.Empty;
            string Pin = string.Empty;
            string strImagurl = string.Empty;
            string Message = string.Empty;
            string Prefix = string.Empty;
            string strName = string.Empty;
            string verfymaillink = string.Empty;
            string h2title = string.Empty;

            string strtblContent = string.Empty;
            Prefix = "Dear ";
            strName = "1";
            if (dtcontent.Rows.Count > 0)
            {
                CompanyName = dtcontent.Rows[0]["PIPMAIL_SENDER_NAME"].ToString();
                Address = dtcontent.Rows[0]["PIPMAIL_SENDER_ADDR"].ToString();
                strName = dtcontent.Rows[0]["PIPMAIL_NAME_FORMAT"].ToString();
            }
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Pin = string.Empty;
            if (strName == "1")
                userName = strLastName;
            if (strName == "2")
                userName = strFirsName;
            if (strName == "3")
                userName = strLastName + " " + strFirsName;
            if (strName == "4")
                userName = strFirsName + " " + strLastName;


            //replacing the required things  

            body = body.Replace("{Prefix}", Prefix);
            body = body.Replace("{Name}", userName);
            body = body.Replace("{CompanyName}", CompanyName);

            string strUrl = string.Empty;

            SqlConnection cn = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);
            cn.Open();
            using (SqlCommand cmdEmail = new SqlCommand("SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPAGY_AGY = '" + _propAgency + "' ", cn))
            {
                SqlDataAdapter daEmail = new SqlDataAdapter(cmdEmail);
                DataSet dsEmail = new DataSet();
                daEmail.Fill(dsEmail);
                if (dsEmail.Tables[0].Rows.Count > 0)
                {
                    strUrl = dsEmail.Tables[0].Rows[0]["PIPAGY_URL"].ToString();
                }

            }
            cn.Close();
            string strlink = "https://pip.capsystems.com/PIP20/" + strUrl;
            body = body.Replace("{Message}", string.Empty);
            body = body.Replace("{tbcontent}", "<br/>Please check Public Intake Portal for messages on " + LookupDataAccess.Getdate(DateTime.Now.Date.ToShortDateString()) + "<br/><b><a href=" + strlink + ">" + strlink + "</a></b>");
            // }
            body = body.Replace("{Address}", Address);
            body = body.Replace("{City}", City);
            body = body.Replace("{State}", State);

            body = body.Replace("{Country}", Country);
            body = body.Replace("{Pin}", Pin);


            return body;

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                SendEmailContent();
            }
            else
            {
                CommonFunctions.MessageBoxDisplay("Message to Applicant Should not be blank");
            }
        }

        string PdfName = "Pdf File";
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        private void btnEmailHistPrint_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> lstSelRows = gvwEmailDetails.Rows.Cast<DataGridViewRow>().
                        Where(row => Convert.ToBoolean(row.Cells["colSel"].Value) == true).ToList();
            if (lstSelRows.Count > 0)
            {
                PrintPdf(PdfName, EventArgs.Empty);
                if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                {
                    PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                    objfrm.StartPosition = FormStartPosition.CenterScreen;

                    objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                    objfrm.ShowDialog();
                }
            }
            else
                AlertBox.Show("Please select atleast one Message");
        }
        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void chkSelAll_CheckedChanged(object sender, EventArgs e)
        {
            List<DataGridViewRow> lstAllRows = gvwEmailDetails.Rows.Cast<DataGridViewRow>().ToList();
            if (chkSelAll.Checked)
            {
                lstAllRows.ForEach(x => x.Cells["colSel"].Value = true);
            }
            else
            {
                lstAllRows.ForEach(x => x.Cells["colSel"].Value = false);
            }
        }

        private void gvwEmailDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            chkSelAll.CheckedChanged -= chkSelAll_CheckedChanged;
            if (gvwEmailDetails.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {

                        List<DataGridViewRow> lstSelfalseRows = gvwEmailDetails.Rows.Cast<DataGridViewRow>().
                           Where(row => Convert.ToBoolean(row.Cells["colSel"].Value) == false).ToList();

                        if (lstSelfalseRows.Count > 0)
                            chkSelAll.Checked = false;
                        else
                            chkSelAll.Checked = true;
                    }
                }
            }
            chkSelAll.CheckedChanged += chkSelAll_CheckedChanged;
        }

        private void PrintPdf(object sender, EventArgs e)
        {
            Random_Filename = null;
            string propReportPath = _model.lookupDataAccess.GetReportPath();
            PdfName = "EmailHistory"; // + propAppNo;
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

            Document document = new Document(PageSize.A4, 30f, 30f, 60f, 30f);
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 16, 3, new iTextSharp.text.BaseColor(0, 0, 102));
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);


            float floatvalue = float.Parse("12"); //float.Parse(((Captain.Common.Utilities.ListItem)cmbsize.SelectedItem).Value.ToString());

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, floatvalue);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, floatvalue, 1);

            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);

            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            PdfPTable APP_details = new PdfPTable(4);
            APP_details.TotalWidth = 730f;//750f;
            APP_details.WidthPercentage = 100;
            APP_details.LockedWidth = true;
            float[] APP_details_Widths = new float[] { 50f, 100f, 40f, 40f };
            APP_details.SetWidths(APP_details_Widths);
            APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            APP_details.HeaderRows = 3;
            APP_details.SpacingBefore = 9f;

            string Name = _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAdminAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            string strHIEDesc = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " [" + Name + "]";

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase((strHIEDesc), TblFontBold), 33, 570, 0); //Hierarchy Print in Header
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Email Communication History", TblFontBold), 300, 570, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Email: " + _email, Times), 570, 570, 0);

            //if (Privilege.Program.Trim() == "CASE0012")
            //{
            //    string strClientName = string.Empty;
            //    if (chkPrintName.Checked)
            //    {
            //        strClientName = /*"Volunteer/Donor Name :" + */propAppName.Trim();
            //    }
            //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Code: " + propAppNo.Trim(), TblFontBold), 410, 570, 0);
            //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(strClientName, TblFontBold), 570, 570, 0);
            //}
            //else
            //{
            //    string strClientName = string.Empty;
            //    if (chkPrintName.Checked)
            //    {
            //        strClientName = /*"Client Name :" +*/ propAppName.Trim();
            //    }
            //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("App#: " + propAppNo.Trim(), TblFontBold), 410, 570, 0);
            //    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(strClientName, TblFontBold), 570, 570, 0);
            //}

            PdfPCell Date = new PdfPCell(new Phrase("", TblFontBold));
            Date.Colspan = 4;
            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Date);

            /*//border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();*/

            string Screen_Desc = null, Priv_ScreenDesc = null;

            PdfPCell Screen = new PdfPCell(new Phrase("Date", TblFontBold));
            Screen.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Screen.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(Screen);

            PdfPCell Description = new PdfPCell(new Phrase("Sender", TblFontBold));
            Description.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Description.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(Description);

            PdfPCell DateHeader = new PdfPCell(new Phrase("Type of Mail", TblFontBold));
            DateHeader.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            DateHeader.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            DateHeader.Colspan = 2;
            APP_details.AddCell(DateHeader);

            //PdfPCell UserHeader = new PdfPCell(new Phrase("User", TblFontBold));
            //UserHeader.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //UserHeader.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            //APP_details.AddCell(UserHeader);

            PdfPCell HSpace = new PdfPCell(new Phrase(" ", Times));
            HSpace.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HSpace.Colspan = 4;
            HSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(HSpace);

            List<DataGridViewRow> lstSelRows = gvwEmailDetails.Rows.Cast<DataGridViewRow>().
                        Where(row => Convert.ToBoolean(row.Cells["colSel"].Value) == true).ToList();


            foreach (DataGridViewRow dr in lstSelRows)
            {
                // if (dr.Cells["categorychk"].Value.ToString() == true.ToString())
                //{

                DataRow drRow = dr.Tag as DataRow;

                string strMiscType = "Remarks";
                if (drRow["PIPMAILH_UPD_ID"].ToString() == "W")
                    strMiscType = "Message to User";
                else if (drRow["PIPMAILH_UPD_ID"].ToString() == "U")
                    strMiscType = "Message to Worker";

                string strDate = dr.Cells["gvtDate"].Value.ToString();
                string strSender = dr.Cells["gvtSender"].Value.ToString();
                string strMailType = strMiscType;
                string strMessage = dr.Cells["gvtRemarks"].Value.ToString();


                //EMPLFUNCEntity row = dr.Tag as EMPLFUNCEntity;

                // string CaseNotes_Desc = row.CaseNotesData.Trim();
                //Screen_Desc = row.ScrDesc.Trim();

                PdfPCell DateCell = new PdfPCell(new Phrase(strDate, Times));   //LookupDataAccess.Getdate(row.CaseNotesDate)
                DateCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(DateCell);

                PdfPCell SenderCell = new PdfPCell(new Phrase(strSender, Times));
                SenderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                SenderCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(SenderCell);

                PdfPCell MailTypeCell = new PdfPCell(new Phrase(strMailType, Times));
                MailTypeCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                MailTypeCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(MailTypeCell);

                //PdfPCell User = new PdfPCell(new Phrase(BaseForm.UserID, Times));
                //User.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //User.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //APP_details.AddCell(User);

                PdfPCell spce = new PdfPCell(new Phrase("  ", Times));
                spce.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                spce.Border = iTextSharp.text.Rectangle.NO_BORDER;
                spce.Colspan = 4;
                APP_details.AddCell(spce);

                PdfPCell MessageCell = new PdfPCell(new Phrase("Message: " + strMessage, Times));
                MessageCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                MessageCell.Colspan = 4;
                MessageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(MessageCell);

                PdfPCell Space = new PdfPCell(new Phrase(" ", Times));
                Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Space.Colspan = 4;
                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Space);

                Space = new PdfPCell(new Phrase(" ", Times));
                Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Space.Colspan = 4;
                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Space);

                //}

            }
            document.Add(APP_details);
            document.Close();
            fs.Close();
            fs.Dispose();
        }

    }
}