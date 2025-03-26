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
using System.Data.SqlClient;
using Captain.Common.Utilities;
using System.Net.Mail;
using System.Configuration;
using System.IO;

#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PIP0001SendMailForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        DataTable dtcontent = new DataTable();
        public PIP0001SendMailForm(BaseForm baseForm, PrivilegeEntity privilieges, string strConf, string strEmail, List<PIPDocEntity> propDocentitylist, DataTable dt, string strRegid)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privilieges;
            lblConf2.Text = strConf;
            _propConformnu = strConf;
            strMailsender = string.Empty;
            _propRegid = strRegid;
            lblEmail2.Text = strEmail;
            this.Text = /*Privileges.Program + " - */"Send Mail";
            _propAgency = "00";

            _propDocentitylist = propDocentitylist;
            dtPIPIntake = dt;

            if (dtPIPIntake.Rows.Count > 0)
            {
                if (dtPIPIntake.Rows[0]["PIPREG_SUBMITTED"].ToString() != "")
                {
                    btnSendReminder.Visible = false;
                }
                else
                {
                    btnSendReminder.Visible = true;
                }
            }

            dtPIPREG = PIPDATA.GETPIPREG(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, _propRegid, string.Empty, string.Empty, string.Empty, "ID");

            dtcontent = PIPDATA.PIPMAILS_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPREG.Rows[0]["PIPREG_AGY"].ToString(), "DOV", string.Empty);

            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I") _propAgency = dtPIPREG.Rows[0]["PIPREG_AGY"].ToString();
            if (dtcontent.Rows.Count > 0)
            {
                // txtMessage.Text = dtcontent.Rows[0]["PIPMAIL_CONTENT"].ToString();
            }
            pnlCreateMessage.Visible = false;
            pnlReview.Visible = true;
            // pnlReview.Location = new Point(15, 43);
            pnlReview.BringToFront();
            gvwDocumentreview.Visible = true;
            btnEmailSend.Enabled = false;
            if (gvwDocumentreview.Rows.Count > 0)
                btnEmailSend.Enabled = true;



            ReviewDocumentload();
        }
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        List<PIPDocEntity> _propDocentitylist { get; set; }
        DataTable dtPIPIntake = new DataTable();
        DataTable dtPIPREG = new DataTable();
        string _propRegid { get; set; }
        string _propAgency { get; set; }
        string _propConformnu { get; set; }
        public string strMailsender = string.Empty;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "&Back")
            {
                btnCreateMiscMsg.Text = "Create a &Misc. Message to Client";
                btnSendReminder.Text = "Send Submission/&Reminder Email";
                _errorProvider.SetError(pnlCreateMessage, null);
                lblAddmiscnote.Visible = true;
                lblAddmiscnote.Text = "View All Misc. Messages";
                //  lblAddmiscnote.Visible = false;
                pnlCreateMessage.Visible = false;
                btnCancel.Text = "&Close";
                pnlReview.Visible = true;

                pnlReview.BringToFront();
                gvwDocumentreview.Visible = true;
                btnEmailSend.Enabled = false;
                if (gvwDocumentreview.Rows.Count > 0)
                    btnEmailSend.Enabled = true;
            }
            else
            {
                this.Close();
            }
        }

        private void rdoClient_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdoClient.Checked)
            //{
            //    _errorProvider.SetError(pnlCreateMessage, null);
            //    pnlCreateMessage.Visible = true;
            //    txtMessage.Text = string.Empty;
            //    pnlReview.Visible = false;
            //    pnlCreateMessage.BringToFront();
            //    btnEmailSend.Enabled = true;

            //}
            //else
            //{
            //_errorProvider.SetError(pnlCreateMessage, null);
            //pnlCreateMessage.Visible = false;
            //pnlReview.Visible = true;
            //pnlReview.Location = new Point(15, 43);
            //pnlReview.BringToFront();
            //gvwDocumentreview.Visible = true;
            //btnEmailSend.Enabled = false;
            //if (gvwDocumentreview.Rows.Count > 0)
            //    btnEmailSend.Enabled = true;


            // }
        }

        private void btnEmailSend_Click(object sender, EventArgs e)
        {
            //if (rdoReview.Checked)
            //{
            if (gvwDocumentreview.Rows.Count > 0)
            {
                List<DataGridViewRow> SelectedgvRows = (from c in gvwDocumentreview.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvchkselect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();

                if (SelectedgvRows.Count > 0)
                    MessageBox.Show("Confirm you want to send the email", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxHandler);
                else
                    CommonFunctions.MessageBoxDisplay("Please select atleast one grid row");
            }
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(txtMessage.Text))
            //    {
            //        _errorProvider.SetError(pnlCreateMessage, null);
            //        MessageBox.Show("Are you sure to send Email?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxHandler, true);
            //    }
            //    else
            //    {
            //        _errorProvider.SetError(pnlCreateMessage, "Please fill message content");
            //    }
            //}
        }

        private void MessageBoxHandler(DialogResult dialogResult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    //if (_propConformnu != string.Empty)
                    //{


                    if (dtPIPREG.Rows.Count > 0)
                    {
                        try
                        {
                            List<DataGridViewRow> SelectedgvRows = (from c in gvwDocumentreview.Rows.Cast<DataGridViewRow>().ToList()
                                                                    where (((DataGridViewCheckBoxCell)c.Cells["gvchkselect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                                    select c).ToList();
                            bool boolSendVerMessge = false;
                            foreach (DataGridViewRow gvrowitem in SelectedgvRows)
                            {
                                if ((gvrowitem.Cells["gvtName"].Value.ToString() != "Misc. Message") && (gvrowitem.Cells["gvtName"].Value.ToString() != "Send Reminder Email"))
                                {
                                    boolSendVerMessge = true;
                                }
                            }

                            if (boolSendVerMessge)
                            {
                                if (dtcontent.Rows.Count > 0)
                                {
                                    SendEmailContent("Email");
                                }
                                else
                                {
                                    CommonFunctions.MessageBoxDisplay("Please define the ‘Mail Content after Document Verification’ in PIP Administration");
                                }
                            }
                            else
                            {
                                SendEmailContent(string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {

                            CommonFunctions.MessageBoxDisplay("Error is: " + ex.Message);
                        }
                    }
                    //}
                    //else
                    //{
                    //    CommonFunctions.MessageBoxDisplay("This applicant Pip data not existed");
                    //}
                //}
            }
        }

        private void SendEmailContent(string strEmailType)
        {
            DataTable dtMailConfig = PIPDATA.PIPMAILCONFIG_GET(BaseForm.BaseLeanDataBaseConnectionString, "DOCUMENT");

            if (dtMailConfig.Rows.Count > 0)
            {
                string[] args = PIPDATA.PIPAGENCY_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPREG.Rows[0]["PIPREG_AGY"].ToString());

                MailMessage mail = new MailMessage();
                mail.To.Add(dtPIPREG.Rows[0]["PIPREG_EMAIL"].ToString());
                mail.From = new MailAddress(dtMailConfig.Rows[0]["MAILCONFIG_EMAILID"].ToString(), args[2].ToString());
                if (strEmailType != string.Empty)
                    mail.Subject = dtMailConfig.Rows[0]["MAILCONFIG_SUBJECT"].ToString();
                else
                    mail.Subject = "Message from Case worker";

                _propstrUpddocid = string.Empty;
                _propstrDocverid = string.Empty;
                mail.Body = createEmailBody(string.Empty, dtPIPREG.Rows[0]["PIPREG_FNAME"].ToString(), dtPIPREG.Rows[0]["PIPREG_LNAME"].ToString(), txtMessage.Text, dtcontent);
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


                //if (rdoClient.Checked)
                //{
                //    smtp.Send(mail);
                //    CommonFunctions.MessageBoxDisplay("Success");
                //    PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_ID"].ToString(), string.Empty, string.Empty, BaseForm.UserID, string.Empty, txtMessage.Text);
                //}
                //else
                //{
                List<DataGridViewRow> SelectedgvRows = (from c in gvwDocumentreview.Rows.Cast<DataGridViewRow>().ToList()
                                                        where (((DataGridViewCheckBoxCell)c.Cells["gvchkselect"]).Value.ToString().Equals(Consts.YesNoVariants.True, StringComparison.CurrentCultureIgnoreCase))
                                                        select c).ToList();
                if (SelectedgvRows.Count > 0)
                {
                    smtp.Send(mail);
                    CommonFunctions.MessageBoxDisplay("Success");
                    strMailsender = "Mail";

                    string[] strIds = _propstrUpddocid.Split(',');
                    foreach (string stritem in strIds)
                    {
                        if (stritem != string.Empty)
                        {
                            PIPDATA.InsertUpdatePipDoc(BaseForm.BaseLeanDataBaseConnectionString, stritem, string.Empty, BaseForm.UserID, string.Empty, "EMAIL");
                        }
                    }
                    string[] strDocverIds = _propstrDocverid.Split(',');
                    foreach (string stritem in strDocverIds)
                    {
                        if (stritem != string.Empty)
                        {
                            PIPDATA.InsertUpdatePipDocVer(BaseForm.BaseLeanDataBaseConnectionString, stritem, string.Empty, string.Empty, BaseForm.UserID, string.Empty, "EMAIL", string.Empty, string.Empty);
                        }
                    }

                    if (_propstrUpddocid != string.Empty)
                        PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, dtPIPREG.Rows[0]["PIPREG_ID"].ToString(), _propstrUpddocid, _propstrDocverid, BaseForm.UserID, string.Empty, string.Empty);

                    foreach (DataGridViewRow gvEmailitem in SelectedgvRows)
                    {
                        if (gvEmailitem.Cells["gvtName"].Value.ToString().Trim() == "Misc. Message" || gvEmailitem.Cells["gvtName"].Value.ToString().Trim() == "Send Reminder Email")
                        {
                            PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, dtPIPREG.Rows[0]["PIPREG_ID"].ToString(), "W", string.Empty, BaseForm.UserID, string.Empty, gvEmailitem.Cells["gvtRemarks"].Value.ToString());

                        }
                    }

                    _propDocentitylist = PIPDATA.GetPIPDOCUPLOADS(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, string.Empty, string.Empty, string.Empty, _propRegid, "REGID");
                    ReviewDocumentload();
                    // rdoClient_CheckedChanged(sender, e);
                }
            }
        }
        string _propstrUpddocid = string.Empty;
        string _propstrDocverid = string.Empty;
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


            //// string selLanguage = Master.SelectedLanguage
            //verfymaillink = "<a  onmousedown='return false'  style='padding:10px; font-size:18px; background-color:#123dcc; border-radius:8px; color:#fff; text-decoration:none; text-transform:none; font-weight:bold;' href='https://pip.capsystems.com/PIP20/Regstatus.aspx?ln=en&dnurl=" + Session["DNUrl"].ToString() + "&typ=A&rud=" + userid + "'>Verify your email</a>";

            ////strMainLogopath = "http://capsystems.com/images/PIPlogos/" + Session["Agency"].ToString().ToUpper() + "MAINLOGO.png";
            //strMainLogopath = WebConfigurationManager.AppSettings["AGENCYLOGOSPATH"] + Session["Agency"].ToString().ToUpper() + "_" + Session["AgyCode"].ToString() + "_LOGO.png";


            //body = body.Replace("{bgcolor}", Session["bg-color-css"].ToString());
            //body = body.Replace("{imagelogo}", strMainLogopath);

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


            StringBuilder strcontent = new StringBuilder();
            //if (rdoReview.Checked)
            //{
            if (dtcontent.Rows.Count > 0)
            {
                string strgvName = string.Empty;
                strcontent.Append("<div style='padding-top:20px;'><table border = '1' style='border:1px solid #333; border-collapse:collapse;width:90%'><tr style='padding:5px; background-color:#4db2fb; color:#fff; font-size:15px; font-weight:bold;'><th style = 'width:20%'> Name of the Member </th><th style = 'width:10%'>Date</th> <th style = 'width:25%'>Name of the Document</th><th style = 'width:5%'> Pass </th ><th style = 'width:40%'> Remarks </th></tr> ");
                foreach (DataGridViewRow gvrowitem in gvwDocumentreview.Rows)
                {
                    if (gvrowitem.Cells["gvchkselect"].Value.ToString().ToUpper() == "TRUE")
                    {
                        if ((gvrowitem.Cells["gvtName"].Value.ToString() != "Misc. Message") && (gvrowitem.Cells["gvtName"].Value.ToString() != "Send Reminder Email"))
                        {
                            if (!_propstrUpddocid.Contains(gvrowitem.Cells["gvtdocId"].Value.ToString()))
                                _propstrUpddocid = _propstrUpddocid + gvrowitem.Cells["gvtdocId"].Value.ToString() + ",";
                            _propstrDocverid = _propstrDocverid + gvrowitem.Cells["gvtVerId"].Value.ToString() + ",";

                            if (strgvName.Trim() != gvrowitem.Cells["gvtName"].Value.ToString())
                            {
                                strcontent.Append("<tr><td>" + gvrowitem.Cells["gvtName"].Value.ToString() + "</td>");
                                strgvName = gvrowitem.Cells["gvtName"].Value.ToString();
                            }
                            else
                                strcontent.Append("<tr><td> </td>");

                            strcontent.Append("<td>" + gvrowitem.Cells["gvtverifyDate"].Value.ToString() + "</td>");
                            strcontent.Append("<td>" + gvrowitem.Cells["gvtFileName"].Value.ToString() + "</td>");
                            if (gvrowitem.Cells["gvtverstatus"].Value.ToString() == "C")
                            {
                                strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:20px; color:#40ba0c'>&#10003;</span></td>");
                            }
                            else
                            {
                                strcontent.Append("<td align='center'><span style='font - weight:bold; font - size:25px; color:#ff0000'> &#215;</span></td>");
                            }
                            strcontent.Append("<td>" + gvrowitem.Cells["gvtRemarks"].Value.ToString() + "</td></tr>");

                        }
                    }
                }
                strcontent.Append("</table></div>");
            }


            //replacing the required things  

            body = body.Replace("{Prefix}", Prefix);
            body = body.Replace("{Name}", userName);
            body = body.Replace("{CompanyName}", CompanyName);

            //if (rdoReview.Checked)
            //{
            //    body = body.Replace("{Message}", dtcontent.Rows[0]["PIPMAIL_CONTENT"].ToString());
            //    body = body.Replace("{tbcontent}", strcontent.ToString());
            //}
            //else
            //{
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

        private void PIP0001SendMailForm_ClientClosed(object objSender, KeyEventArgs objArgs)
        {

        }

        private void ReviewDocumentload()
        {
            btnEmailHist.Visible = false;

            DataTable dtPipEmail = PIPDATA.PIPEMAILHIST(BaseForm.BaseLeanDataBaseConnectionString, _propRegid.ToString());
            if (dtPipEmail.Rows.Count > 0)
            {
                btnEmailHist.Visible = true;
            }
            gvwDocumentreview.Rows.Clear();
            foreach (DataRow dritem in dtPIPIntake.Rows)
            {



                List<PIPDocEntity> pipdoccount = _propDocentitylist.FindAll(u => u.PIPDOCUPLD_PIP_ID == dritem["PIP_ID"].ToString());
                string ApplicantName = LookupDataAccess.GetMemberName(dritem["PIP_FNAME"].ToString(), dritem["PIP_MNAME"].ToString(), dritem["PIP_LNAME"].ToString(), BaseForm.BaseHierarchyCnFormat);

                if (pipdoccount.Count > 0)
                {
                    int intfirst = 0; int intverdocfirst = 0;
                    foreach (PIPDocEntity drpipdoc in pipdoccount)
                    {
                        List<PIPDocVerEntity> pipdocVerlist = PIPDATA.GETPIPDOCVER(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, drpipdoc.PIPDOCUPLD_ID, string.Empty);
                        pipdocVerlist = pipdocVerlist.FindAll(u => u.PIPDOCVER_MAIL_ON == string.Empty);
                        if (pipdocVerlist.Count > 0)
                        {
                            intverdocfirst = 0;
                            foreach (PIPDocVerEntity docveritem in pipdocVerlist)
                            {

                                //if (intfirst == 0)
                                //{

                                gvwDocumentreview.Rows.Add(true, ApplicantName, LookupDataAccess.Getdate(docveritem.PIPDOCVER_DATE), drpipdoc.PIPDOCUPLD_DOCNAME, docveritem.PIPDOCVER_Remarks, docveritem.PIPDOCVER_ID, drpipdoc.PIPDOCUPLD_ID, docveritem.PIPDOCVER_STATUS);

                                //    intfirst = intfirst + 1;
                                //    intverdocfirst = intverdocfirst + 1;
                                //}
                                //else
                                //{
                                //    string strdocname = string.Empty;
                                //    if (intverdocfirst == 0)
                                //    {
                                //        strdocname = drpipdoc.PIPDOCUPLD_DOCNAME;
                                //        intverdocfirst = intverdocfirst + 1;
                                //    }

                                //    gvwDocumentreview.Rows.Add(true, ApplicantName, LookupDataAccess.Getdate(docveritem.PIPDOCVER_DATE), drpipdoc.PIPDOCUPLD_DOCNAME, docveritem.PIPDOCVER_Remarks, docveritem.PIPDOCVER_ID, drpipdoc.PIPDOCUPLD_ID, docveritem.PIPDOCVER_STATUS);

                                //}
                            }
                        }
                        if(gvwDocumentreview.Rows.Count>0) gvwDocumentreview.Rows[0].Selected = true;
                    }
                }

            }
            btnEmailSend.Enabled = false;
            if (gvwDocumentreview.Rows.Count > 0)
                btnEmailSend.Enabled = true;
        }
        private void btnCreateMiscMsg_Click(object sender, EventArgs e)
        {
            if (pnlReview.Visible == true)
            {
                _errorProvider.SetError(pnlCreateMessage, null);
                btnCreateMiscMsg.Text = "Move Misc. Message to &Grid";
                pnlCreateMessage.Visible = true;
                lblAddmiscnote.Visible = true;
                lblAddmiscnote.Text = "Add a Misc. Note";
                txtMessage.Text = string.Empty;
                btnCancel.Text = "&Back";
                pnlReview.Visible = false;
                pnlCreateMessage.BringToFront();pnlCreateMessage.Dock = DockStyle.Fill;
                btnEmailSend.Enabled = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(txtMessage.Text))
                {
                    btnCreateMiscMsg.Text = "Create a &Misc. Message to Client";
                    _errorProvider.SetError(pnlCreateMessage, null);
                    lblAddmiscnote.Visible = true;
                    lblAddmiscnote.Text = "View All Misc. Messages";
                    //  lblAddmiscnote.Visible = false;
                    pnlCreateMessage.Visible = false;
                    btnCancel.Text = "&Close";
                    pnlReview.Visible = true;
                    //  pnlReview.Location = new Point(8, 39);
                    pnlReview.BringToFront();pnlReview.Dock = DockStyle.Fill;
                    gvwDocumentreview.Visible = true;
                    btnEmailSend.Enabled = false;
                    gvwDocumentreview.Rows.Add(true, "Misc. Message", LookupDataAccess.Getdate(DateTime.Now.Date.ToShortDateString()), "N/A", txtMessage.Text, string.Empty, string.Empty, string.Empty);

                    if (gvwDocumentreview.Rows.Count > 0)
                        btnEmailSend.Enabled = true;
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Please fill Message Content");
                }
               
            }
            if (gvwDocumentreview.Rows.Count > 0) gvwDocumentreview.Rows[0].Selected = true;
        }

        private void btnEmailHist_Click(object sender, EventArgs e)
        {
            PIP00001EmailHistoryForm emailhistory = new PIP00001EmailHistoryForm(BaseForm, _propDocentitylist, dtPIPIntake, lblEmail2.Text, _propRegid);
            emailhistory.StartPosition= FormStartPosition.CenterScreen;
            emailhistory.ShowDialog();
        }

        private void btnSendReminder_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Are you sure you would like to send the Submission/ Reminder email?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: MessageBoxRemainderHandler);

            //if (pnlReview.Visible == true)
            //{
            //    _errorProvider.SetError(pnlCreateMessage, null);
            //    btnSendReminder.Text = "Move Reminder. Message to Grid";
            //    pnlCreateMessage.Visible = true;
            //    lblAddmiscnote.Visible = true;
            //    lblAddmiscnote.Text = "Add a Reminder Note";

            //    txtMessage.Text = string.Empty;
            //   DataTable  dtremaindercontent = PIPDATA.PIPMAILS_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPREG.Rows[0]["PIPREG_AGY"].ToString(), "SRM", string.Empty);
            //    if(dtremaindercontent.Rows.Count>0)
            //    {
            //        txtMessage.Text = dtremaindercontent.Rows[0]["PIPMAIL_CONTENT"].ToString();
            //    }
            //    btnCancel.Text = "Back";
            //    pnlReview.Visible = false;
            //    pnlCreateMessage.BringToFront();
            //    btnEmailSend.Enabled = false;
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(txtMessage.Text))
            //    {
            //        btnSendReminder.Text = "Send Reminder Email";
            //        _errorProvider.SetError(pnlCreateMessage, null);
            //        lblAddmiscnote.Visible = true;
            //        lblAddmiscnote.Text = "View All Reminder. Messages";
            //        //  lblAddmiscnote.Visible = false;
            //        pnlCreateMessage.Visible = false;
            //        btnCancel.Text = "Close";
            //        pnlReview.Visible = true;
            //        //  pnlReview.Location = new Point(8, 39);
            //        pnlReview.BringToFront();
            //        gvwDocumentreview.Visible = true;
            //        btnEmailSend.Enabled = false;
            //        gvwDocumentreview.Rows.Add(true, "Send Reminder Email", LookupDataAccess.Getdate(DateTime.Now.Date.ToShortDateString()), "N/A", txtMessage.Text, string.Empty, string.Empty, string.Empty);

            //        if (gvwDocumentreview.Rows.Count > 0)
            //            btnEmailSend.Enabled = true;
            //    }
            //    else
            //    {
            //        CommonFunctions.MessageBoxDisplay("Please fill message content");
            //    }

            //}
        }

        private void MessageBoxRemainderHandler(DialogResult dialogResult)
        {
            // Get Gizmox.WebGUI.Forms.Form object that called MessageBox
            //Gizmox.WebGUI.Forms.Form senderForm = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderForm != null)
            //{
                // Set DialogResult value of the Form as a text for label
                if (dialogResult == DialogResult.Yes)
                {
                    DataTable dtMailConfig = PIPDATA.PIPMAILCONFIG_GET(BaseForm.BaseLeanDataBaseConnectionString, "DOCUMENT");

                    if (dtMailConfig.Rows.Count > 0)
                    {
                    try
                    {

                        string[] args = PIPDATA.PIPAGENCY_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPREG.Rows[0]["PIPREG_AGY"].ToString());

                        MailMessage mail = new MailMessage();
                        mail.To.Add(dtPIPREG.Rows[0]["PIPREG_EMAIL"].ToString());
                        mail.From = new MailAddress(dtMailConfig.Rows[0]["MAILCONFIG_EMAILID"].ToString(), args[2].ToString());

                        DataTable dtremaindercontent = PIPDATA.PIPMAILS_GET(BaseForm.BaseLeanDataBaseConnectionString, dtPIPREG.Rows[0]["PIPREG_AGENCY"].ToString(), dtPIPREG.Rows[0]["PIPREG_AGY"].ToString(), "SRM", string.Empty);
                        if (dtremaindercontent.Rows.Count > 0)
                        {
                            txtMessage.Text = dtremaindercontent.Rows[0]["PIPMAIL_CONTENT"].ToString();


                            mail.Subject = "Completion / Submission Reminder";


                            mail.Body = dtremaindercontent.Rows[0]["PIPMAIL_CONTENT"].ToString(); //createEmailBody(string.Empty, dtPIPREG.Rows[0]["PIPREG_FNAME"].ToString(), dtPIPREG.Rows[0]["PIPREG_LNAME"].ToString(), txtMessage.Text, dtcontent);
                            mail.IsBodyHtml = true;
                        }
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

                        PIPDATA.InsertUpdatePIPEmailHistory(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, dtPIPREG.Rows[0]["PIPREG_ID"].ToString(), "W", string.Empty, BaseForm.UserID, string.Empty, "Completion / Submission Reminder");

                        AlertBox.Show("Submission/Reminder email sent successfully ");
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    }

                }
            //}
        }

        private void gvwDocumentreview_Click(object sender, EventArgs e)
        {

        }
    }
}