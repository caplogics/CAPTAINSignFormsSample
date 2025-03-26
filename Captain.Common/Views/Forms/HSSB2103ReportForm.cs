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
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Text.RegularExpressions;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2103ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public HSSB2103ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
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
            List<CommonEntity> listTaskDetails = _model.lookupDataAccess.GetHssb2103FormTaskDetails();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");


            foreach (CommonEntity item in listSequence)
            {
                cmbActive.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbActive.SelectedIndex = 2;
            cmbMaxoftask.SelectedIndexChanged -= new EventHandler(cmbMaxoftask_SelectedIndexChanged);
            foreach (CommonEntity itemTasks in listTaskDetails)
            {
                cmbMaxoftask.Items.Add(new Captain.Common.Utilities.ListItem(itemTasks.Desc, itemTasks.Code));
            }
            cmbMaxoftask.SelectedIndex = 0;
            cmbMaxoftask.SelectedIndexChanged += new EventHandler(cmbMaxoftask_SelectedIndexChanged);
        }


        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<RankCatgEntity> propRankscategory { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        //public List<CaseMstEntity> propCaseMstList { get; set; }
        //public List<CaseSnpEntity> propCaseSnpList { get; set; }
        //public List<ChildATTMSEntity> propchldAttmsEntityList { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }
        public string propMealTypes { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<ChldAttnEntity> propChldAttnList { get; set; }
        public List<ChldAttnEntity> propChldAttnCountList { get; set; }
        public List<ZipCodeEntity> propZipCode { get; set; }
        public List<ChldMediEntity> propChldMediEntity { get; set; }
        

        #endregion

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




                string strSequence = string.Empty;
                if (rdoReportChildName.Checked)
                    strSequence = "Child Name";
                if (rdoApplicant.Checked)
                    strSequence = "Applicant";
                if (rdoClass.Checked)
                    strSequence = "Class";


                string strZipCodes = string.Empty;
                if (rdoZipAll.Checked)
                    strZipCodes = "A";
                if (rdoZipSelected.Checked)
                {
                    foreach (ZipCodeEntity Entity in selZipcodeList)
                    {
                        if (!strZipCodes.Equals(string.Empty)) strZipCodes += ",";
                        strZipCodes += Entity.Zcrzip;//(!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty);
                    }
                }

                string strTaskCodes = string.Empty;
                foreach (ChldTrckEntity Entity in SeltrackList)
                {
                    if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                    strTaskCodes += Entity.TASK;
                }
                string Chidrenwith = "B";
                if (rdoSelectask.Checked)
                    Chidrenwith = "S";
                if (rdoNoTask.Checked)
                    Chidrenwith = "N";
                propCaseEnrlList = _model.EnrollData.GetEnrollDetails2103(Agency, Dept, Prog, Program_Year, (rdoTodayDate.Checked == true ? "T" : "K"), strsiteRoomNames, strFundingCodes, strZipCodes, strTaskCodes, Chidrenwith, strSequence,txtApplicant.Text);
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
                    strxmlReportlog.Clear();

                    PrintHeaderPage(document, writer);
                   /// Report Log functionality
                   /// 
                    //ReportLogEntity replogEntity = new ReportLogEntity();
                    //replogEntity.REP_PROG_NAME = Privileges.Program;
                    //replogEntity.REP_EMP_CODE = BaseForm.UserID;
                    //replogEntity.REP_DATA = strxmlReportlog.ToString();//Get_XML_Format_for_Report_Controls();
                    //replogEntity.REP_FILE_NAME = form.GetFileName();
                    //replogEntity.REP_MODULE_CODE = BaseForm.BusinessModuleID;
                    //_model.AdhocData.InsertReportLog(replogEntity);

                    strxmlReportlog.Clear(); 

                    document.NewPage();
                    Y_Pos = 795;

                    string strTaskCodes = string.Empty;
                    foreach (ChldTrckEntity Entity in SeltrackList)
                    {
                        if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                        strTaskCodes += Entity.TASK;
                    }

                    if (((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString() == "1")
                    {

                        PdfPTable hssb2103_Table = new PdfPTable(10);

                        hssb2103_Table.WidthPercentage = 100;
                        // hssb2103_Table.LockedWidth = true;
                        float[] widths = new float[] { 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f,35f };
                        hssb2103_Table.SetWidths(widths);
                        hssb2103_Table.HorizontalAlignment = Element.ALIGN_CENTER;


                        PdfPTable hssb2103_Table2 = new PdfPTable(11);
                        hssb2103_Table2.TotalWidth = 550f;
                        hssb2103_Table2.WidthPercentage = 100;
                        hssb2103_Table2.LockedWidth = true;
                        float[] widths2 = new float[] { 30f, 60f, 30f, 15f, 15f, 15f, 30f, 30f, 30f,35f, 35f };
                        hssb2103_Table2.SetWidths(widths2);
                        hssb2103_Table2.HorizontalAlignment = Element.ALIGN_CENTER;
                        hssb2103_Table2.HeaderRows = 2;




                        List<CaseEnrlEntity> caseEnrlList;
                        if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                        {
                            caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                        }
                        else
                        {
                            caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                        }

                        if (rdoEnrolled.Checked)
                            caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("E"));
                        if (rdoWaitList.Checked)
                            caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("L"));

                        if (caseEnrlList.Count > 0)
                        {
                            if (!rdoNoTask.Checked)
                                propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, txtCompleTask.Text, string.Empty, string.Empty, strTaskCodes, "Task", (rdoOnlyLutd.Checked == true ? "UPDATEDONLY" : string.Empty));
                      
                            if (SeltrackList.Count > 0)
                            {

                                PdfPCell Header1 = new PdfPCell(new Phrase(SeltrackList[0].TASKDESCRIPTION, TblFontBold));
                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header1.Border = iTextSharp.text.Rectangle.BOX;
                                hssb2103_Table.AddCell(Header1);

                                if (SeltrackList.Count > 1)
                                {
                                    PdfPCell Header2 = new PdfPCell(new Phrase(SeltrackList[1].TASKDESCRIPTION, TblFontBold));
                                    Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header2.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header2);
                                }
                                else
                                {
                                    PdfPCell Header2 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header2.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header2);
                                }
                                if (SeltrackList.Count > 2)
                                {
                                    PdfPCell Header3 = new PdfPCell(new Phrase(SeltrackList[2].TASKDESCRIPTION, TblFontBold));
                                    Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header3.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header3);
                                }
                                else
                                {
                                    PdfPCell Header3 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header3.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header3);
                                }
                                if (SeltrackList.Count > 3)
                                {
                                    PdfPCell Header4 = new PdfPCell(new Phrase(SeltrackList[3].TASKDESCRIPTION, TblFontBold));
                                    Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header4.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header4);
                                }
                                else
                                {
                                    PdfPCell Header4 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header4.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header4);
                                }
                                if (SeltrackList.Count > 4)
                                {
                                    PdfPCell Header5 = new PdfPCell(new Phrase(SeltrackList[4].TASKDESCRIPTION, TblFontBold));
                                    Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header5.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header5);

                                }
                                else
                                {
                                    PdfPCell Header5 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header5.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header5);
                                }
                                if (SeltrackList.Count > 5)
                                {
                                    PdfPCell Header6 = new PdfPCell(new Phrase(SeltrackList[5].TASKDESCRIPTION, TblFontBold));
                                    Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header6.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header6);
                                }
                                else
                                {
                                    PdfPCell Header6 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header6.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header6);
                                }
                                if (SeltrackList.Count > 6)
                                {
                                    PdfPCell Header7 = new PdfPCell(new Phrase(SeltrackList[6].TASKDESCRIPTION, TblFontBold));
                                    Header7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header7.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header7);

                                }
                                else
                                {
                                    PdfPCell Header7 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header7.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header7);

                                }
                                if (SeltrackList.Count > 7)
                                {
                                    PdfPCell Header8 = new PdfPCell(new Phrase(SeltrackList[7].TASKDESCRIPTION, TblFontBold));
                                    Header8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header8.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header8);

                                }
                                else
                                {
                                    PdfPCell Header8 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header8.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header8);
                                }
                                if (SeltrackList.Count > 8)
                                {
                                    PdfPCell Header9 = new PdfPCell(new Phrase(SeltrackList[8].TASKDESCRIPTION, TblFontBold));
                                    Header9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header9.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header9);
                                }
                                else
                                {
                                    PdfPCell Header9 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header9.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header9);
                                }
                                if (SeltrackList.Count > 9)
                                {
                                    PdfPCell Header10 = new PdfPCell(new Phrase(SeltrackList[9].TASKDESCRIPTION.Trim(), TblFontBold));
                                    Header10.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header10.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header10);
                                }
                                else
                                {
                                    PdfPCell Header10 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header10.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header10.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header10);
                                }

                            }

                            //if (hssb2103_Table.Rows.Count > 0)
                            //{
                            //    document.Add(hssb2103_Table);
                            //}
                            string[] Headers = { "Child's Name    ", " DOB ", "R", "WL", " A/I", "Attended   ", "S Date ", "Site/Room" };

                            PdfPCell Headertop1 = new PdfPCell(hssb2103_Table);
                            Headertop1.Colspan = 11;
                            Headertop1.Padding = 0f;
                            hssb2103_Table2.AddCell(Headertop1);

                            PdfPCell Header21 = new PdfPCell(new Phrase("App #", TblFontBold));
                            Header21.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header21.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(Header21);

                            for (int i = 0; i < Headers.Length; i++)
                            {


                                PdfPCell Header22 = new PdfPCell(new Phrase(Headers[i], TblFontBold));
                                Header22.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2103_Table2.AddCell(Header22);
                            }

                            PdfPCell pdfroute = new PdfPCell(new Phrase("Bus / Rte", TblFontBold));
                            pdfroute.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfroute.Border =  iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(pdfroute);

                            PdfPCell Header23 = new PdfPCell(new Phrase("Funding", TblFontBold));
                            Header23.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header23.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(Header23);

                            string strZipcode = string.Empty;
                            string strRoomampm = string.Empty;
                            foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                            {
                                strZipcode = string.Empty;

                                if(rdoClass.Checked==true)
                                {
                                    if (strRoomampm != string.Empty)
                                    {
                                        if (strRoomampm != enrlrow.Site + enrlrow.Room + enrlrow.AMPM)
                                        {
                                            strRoomampm = enrlrow.Site + enrlrow.Room + enrlrow.AMPM;
                                            if (hssb2103_Table2.Rows.Count > 0)
                                            {
                                                document.Add(hssb2103_Table2);
                                                document.NewPage();
                                                hssb2103_Table2.DeleteBodyRows();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strRoomampm = enrlrow.Site + enrlrow.Room + enrlrow.AMPM;
                                    }
                                }
                                PdfPCell pdfAppldata = new PdfPCell(new Phrase(enrlrow.App, TableFont));
                                pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAppldata.Border = iTextSharp.text.Rectangle.NO_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                hssb2103_Table2.AddCell(pdfAppldata);

                                if (rdoZipAll.Checked == false)
                                    strZipcode = enrlrow.Mst_Zip;
                                PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat) + "  " + strZipcode, TableFont));
                                pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfChildName.Colspan = 1;
                                pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfChildName);

                                PdfPCell pdfDob = new PdfPCell(new Phrase(LookupDataAccess.Getdate(enrlrow.Snp_DOB), TableFont));
                                pdfDob.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfDob);


                                PdfPCell pdfR = new PdfPCell(new Phrase((enrlrow.Chld_Repeater.ToString().ToUpper()), TableFont));
                                pdfR.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfR.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfR);

                                PdfPCell pdfW = new PdfPCell(new Phrase((enrlrow.Status == "W" ? "Y" : "N"), TableFont));
                                pdfW.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfW.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfW);

                                PdfPCell pdfAi = new PdfPCell(new Phrase((""), TableFont));
                                pdfAi.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAi.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfAi);

                                PdfPCell pdfAttenDt = new PdfPCell(new Phrase(LookupDataAccess.Getdate(enrlrow.AttendedDt), TableFont));
                                pdfAttenDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAttenDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfAttenDt);

                                PdfPCell pdfSDt = new PdfPCell(new Phrase(enrlrow.Status + " " + LookupDataAccess.Getdate(enrlrow.Enrl_Date), TableFont));
                                pdfSDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfSDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfSDt);

                                PdfPCell pdfSiteRoom = new PdfPCell(new Phrase(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, TableFont));
                                pdfSiteRoom.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfSiteRoom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfSiteRoom);


                                PdfPCell pdfBusrtedesc = new PdfPCell(new Phrase(enrlrow.BusRoot, TableFont));
                                pdfBusrtedesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfBusrtedesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfBusrtedesc);


                                 CommonEntity commonfund =  propfundingSource.Find(u => u.Code == enrlrow.FundHie);
                                 if (commonfund != null)
                                 {

                                     PdfPCell pdfFundSource = new PdfPCell(new Phrase((commonfund.Desc), TableFont));
                                     pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                     pdfFundSource.Border = iTextSharp.text.Rectangle.NO_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                     hssb2103_Table2.AddCell(pdfFundSource);
                                 }
                                 else
                                 {
                                     PdfPCell pdfFundSource = new PdfPCell(new Phrase("", TableFont));
                                     pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                     pdfFundSource.Border = iTextSharp.text.Rectangle.NO_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                     hssb2103_Table2.AddCell(pdfFundSource);
                                 }
                                if (rdoNoTask.Checked == false)
                                {
                                    List<ChldMediEntity> chldMediListEntity = new List<ChldMediEntity>();

                                    List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog) && (u.APP_NO == enrlrow.App));


                                    PdfPTable hssb2103_TableData = new PdfPTable(10);
                                    hssb2103_TableData.WidthPercentage = 100;
                                    // hssb2103_Table.LockedWidth = true;
                                    float[] widthsData = new float[] { 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f };
                                    hssb2103_TableData.SetWidths(widthsData);
                                    hssb2103_TableData.HorizontalAlignment = Element.ALIGN_CENTER;


                                    PdfPTable nestedtask1 = new PdfPTable(1);
                                    nestedtask1.WidthPercentage = 100;
                                    float[] nestwidthstask1 = new float[] { 35f };
                                    nestedtask1.SetWidths(nestwidthstask1);


                                    PdfPTable nestedtask2 = new PdfPTable(1);
                                    float[] nestwidthstask2 = new float[] { 35f };
                                    nestedtask2.SetWidths(nestwidthstask2);
                                    nestedtask2.WidthPercentage = 100;

                                    PdfPTable nestedtask3 = new PdfPTable(1);
                                    float[] nestwidthstask3 = new float[] { 35f };
                                    nestedtask3.SetWidths(nestwidthstask3);
                                    nestedtask3.WidthPercentage = 100;

                                    PdfPTable nestedtask4 = new PdfPTable(1);
                                    float[] nestwidthstask4 = new float[] { 35f };
                                    nestedtask4.SetWidths(nestwidthstask4);
                                    nestedtask4.WidthPercentage = 100;

                                    PdfPTable nestedtask5 = new PdfPTable(1);
                                    float[] nestwidthstask5 = new float[] { 35f };
                                    nestedtask5.SetWidths(nestwidthstask5);
                                    nestedtask5.WidthPercentage = 100;

                                    PdfPTable nestedtask6 = new PdfPTable(1);
                                    float[] nestwidthstask6 = new float[] { 35f };
                                    nestedtask6.SetWidths(nestwidthstask6);
                                    nestedtask6.WidthPercentage = 100;

                                    PdfPTable nestedtask7 = new PdfPTable(1);
                                    float[] nestwidthstask7 = new float[] { 35f };
                                    nestedtask7.SetWidths(nestwidthstask7);
                                    nestedtask7.WidthPercentage = 100;


                                    PdfPTable nestedtask8 = new PdfPTable(1);
                                    float[] nestwidthstask8 = new float[] { 35f };
                                    nestedtask8.SetWidths(nestwidthstask8);
                                    nestedtask8.WidthPercentage = 100;

                                    PdfPTable nestedtask9 = new PdfPTable(1);
                                    float[] nestwidthstask9 = new float[] { 35f };
                                    nestedtask9.SetWidths(nestwidthstask9);
                                    nestedtask9.WidthPercentage = 100;

                                    PdfPTable nestedtask10 = new PdfPTable(1);
                                    float[] nestwidthstask10 = new float[] { 35f };
                                    nestedtask10.SetWidths(nestwidthstask10);
                                    nestedtask10.WidthPercentage = 100;




                                    if (chldMediEntity.Count > 0)
                                    {

                                        chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[0].TASK.Trim());


                                        if (chldMediListEntity.Count > 0)
                                        {
                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                nestedtask1.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell firsttask = new PdfPCell(nestedtask1);
                                            firsttask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            firsttask.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            hssb2103_TableData.AddCell(firsttask);
                                        }
                                        else
                                        {
                                            PdfPCell Header1 = new PdfPCell(new Phrase("", TableFont));
                                            Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            hssb2103_TableData.AddCell(Header1);
                                        }
                                        if (SeltrackList.Count > 1)
                                        {
                                            // chldmediFilter = chldMediEntity.Find(u => u.TASK.Trim() == SeltrackList[1].TASK.Trim());
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[1].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask2.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask2);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);

                                        }
                                        else
                                        {
                                            PdfPCell Header2 = new PdfPCell(new Phrase("", TableFont));
                                            Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header2);
                                        }
                                        if (SeltrackList.Count > 2)
                                        {

                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[2].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask3.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask3);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);
                                        }
                                        else
                                        {
                                            PdfPCell Header3 = new PdfPCell(new Phrase("", TableFont));
                                            Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header3);
                                        }
                                        if (SeltrackList.Count > 3)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[3].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask4.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask4);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);
                                        }
                                        else
                                        {
                                            PdfPCell Header4 = new PdfPCell(new Phrase("", TableFont));
                                            Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header4);
                                        }
                                        if (SeltrackList.Count > 4)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[4].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask5.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask5);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);

                                        }
                                        else
                                        {
                                            PdfPCell Header5 = new PdfPCell(new Phrase("", TableFont));
                                            Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header5);
                                        }
                                        if (SeltrackList.Count > 5)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[5].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask6.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask6);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);
                                        }
                                        else
                                        {
                                            PdfPCell Header6 = new PdfPCell(new Phrase("", TableFont));
                                            Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header6);
                                        }
                                        if (SeltrackList.Count > 6)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[6].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask7.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask7);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);

                                        }
                                        else
                                        {
                                            PdfPCell Header7 = new PdfPCell(new Phrase("", TableFont));
                                            Header7.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header7);

                                        }
                                        if (SeltrackList.Count > 7)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[7].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask8.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask8);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);

                                        }
                                        else
                                        {
                                            PdfPCell Header8 = new PdfPCell(new Phrase("", TableFont));
                                            Header8.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header8);
                                        }
                                        if (SeltrackList.Count > 8)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[8].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask9.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask9);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);
                                        }
                                        else
                                        {
                                            PdfPCell Header9 = new PdfPCell(new Phrase("", TableFont));
                                            Header9.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header9);
                                        }
                                        if (SeltrackList.Count > 9)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[9].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask10.AddCell(Header1);
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell secondTask = new PdfPCell(nestedtask10);
                                            secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(secondTask);
                                        }
                                        else
                                        {
                                            PdfPCell Header10 = new PdfPCell(new Phrase("", TableFont));
                                            Header10.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header10.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                            hssb2103_TableData.AddCell(Header10);
                                        }



                                        PdfPCell TaskData = new PdfPCell(hssb2103_TableData);
                                        TaskData.Colspan = 11;
                                        TaskData.Padding = 0f;
                                        TaskData.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        hssb2103_Table2.AddCell(TaskData);

                                        PdfPCell TaskSpace = new PdfPCell();
                                        TaskSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                        TaskSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        TaskSpace.Colspan = 11;
                                        hssb2103_Table2.AddCell(TaskSpace);

                                    }
                                    // }
                                    //................

                                    else
                                    {
                                        PdfPCell TaskData = new PdfPCell();
                                        TaskData.HorizontalAlignment = Element.ALIGN_CENTER;
                                        TaskData.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        TaskData.Colspan = 11;
                                        hssb2103_Table2.AddCell(TaskData);
                                        // hssb2103_TableData.DeleteBodyRows();
                                    }
                                }
                                else
                                {
                                    PdfPCell TaskData = new PdfPCell();
                                    TaskData.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TaskData.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    TaskData.Colspan = 11;
                                    hssb2103_Table2.AddCell(TaskData);

                                }

                            }
                        }
                        if (hssb2103_Table2.Rows.Count > 0)
                        {

                            document.Add(hssb2103_Table2);
                        }
                    }
                    // 6 Tasks loop ********

                        //*********************

                    else
                    {

                        PdfPTable hssb2103_Table = new PdfPTable(6);
                        hssb2103_Table.WidthPercentage = 100;
                        float[] widths = new float[] { 50f, 50f, 50f, 50f, 50f, 50f };
                        hssb2103_Table.SetWidths(widths);
                        hssb2103_Table.HorizontalAlignment = Element.ALIGN_CENTER;



                        PdfPTable hssb2103_Table2 = new PdfPTable(11);
                        hssb2103_Table2.TotalWidth = 550f;
                        hssb2103_Table2.WidthPercentage = 100;
                        hssb2103_Table2.LockedWidth = true;
                        float[] widths2 = new float[] { 30f, 60f, 30f, 15f, 15f, 15f, 30f, 30f, 30f, 35f, 35f };
                        hssb2103_Table2.SetWidths(widths2);
                        hssb2103_Table2.HorizontalAlignment = Element.ALIGN_CENTER;
                        hssb2103_Table2.HeaderRows = 2;


                        List<CaseEnrlEntity> caseEnrlList;
                        if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                        {
                            caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                        }
                        else
                        {
                            caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                        }

                        if (rdoEnrolled.Checked)
                            caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("E"));
                        if (rdoWaitList.Checked)
                            caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("L"));

                        if (caseEnrlList.Count > 0)
                        {                          

                            if (rdoNoTask.Checked==false && chkIncludeNotes.Checked == true)
                                propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, txtCompleTask.Text, string.Empty, string.Empty, strTaskCodes, "CaseNotes", (rdoOnlyLutd.Checked == true ? "UPDATEDONLY" : string.Empty));
                            else if (rdoNoTask.Checked == false && chkIncludeNotes.Checked == false)
                                propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, txtCompleTask.Text, string.Empty, string.Empty, strTaskCodes, "Task", (rdoOnlyLutd.Checked == true ? "UPDATEDONLY" : string.Empty));


                            if (SeltrackList.Count > 0)
                            {

                                PdfPCell Header1 = new PdfPCell(new Phrase(SeltrackList[0].TASKDESCRIPTION, TblFontBold));
                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header1.Border = iTextSharp.text.Rectangle.BOX;
                                hssb2103_Table.AddCell(Header1);

                                if (SeltrackList.Count > 1)
                                {
                                    PdfPCell Header2 = new PdfPCell(new Phrase(SeltrackList[1].TASKDESCRIPTION, TblFontBold));
                                    Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header2.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header2);
                                }
                                else
                                {
                                    PdfPCell Header2 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header2.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header2);
                                }
                                if (SeltrackList.Count > 2)
                                {
                                    PdfPCell Header3 = new PdfPCell(new Phrase(SeltrackList[2].TASKDESCRIPTION, TblFontBold));
                                    Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header3.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header3);
                                }
                                else
                                {
                                    PdfPCell Header3 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header3.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header3);
                                }
                                if (SeltrackList.Count > 3)
                                {
                                    PdfPCell Header4 = new PdfPCell(new Phrase(SeltrackList[3].TASKDESCRIPTION, TblFontBold));
                                    Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header4.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header4);
                                }
                                else
                                {
                                    PdfPCell Header4 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header4.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header4);
                                }
                                if (SeltrackList.Count > 4)
                                {
                                    PdfPCell Header5 = new PdfPCell(new Phrase(SeltrackList[4].TASKDESCRIPTION, TblFontBold));
                                    Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header5.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header5);

                                }
                                else
                                {
                                    PdfPCell Header5 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header5.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header5);
                                }
                                if (SeltrackList.Count > 5)
                                {
                                    PdfPCell Header6 = new PdfPCell(new Phrase(SeltrackList[5].TASKDESCRIPTION, TblFontBold));
                                    Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header6.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header6);
                                }
                                else
                                {
                                    PdfPCell Header6 = new PdfPCell(new Phrase("", TblFontBold));
                                    Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Header6.Border = iTextSharp.text.Rectangle.BOX;
                                    hssb2103_Table.AddCell(Header6);
                                }
                            }



                            string[] Headers = { "Child's Name    ", " DOB ", "R", "WL", " A/I", "Attended   ", "S Date ", "Site/Room" };

                            PdfPCell Headertop1 = new PdfPCell(hssb2103_Table);
                            Headertop1.Colspan = 11;
                            Headertop1.Padding = 0f;
                            hssb2103_Table2.AddCell(Headertop1);

                            PdfPCell Header21 = new PdfPCell(new Phrase("App #", TblFontBold));
                            Header21.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header21.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(Header21);

                            for (int i = 0; i < Headers.Length; i++)
                            {

                                PdfPCell Header22 = new PdfPCell(new Phrase(Headers[i], TblFontBold));
                                Header22.HorizontalAlignment = Element.ALIGN_CENTER;
                                Header22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2103_Table2.AddCell(Header22);
                            }

                            PdfPCell pdfroute = new PdfPCell(new Phrase("Bus / Rte", TblFontBold));
                            pdfroute.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfroute.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(pdfroute);

                            PdfPCell Header23 = new PdfPCell(new Phrase("Funding", TblFontBold));
                            Header23.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header23.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            hssb2103_Table2.AddCell(Header23);

                            string strZipcode = string.Empty;
                            string strRoomampm = string.Empty;
                            foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                            {
                                if (rdoClass.Checked == true)
                                {
                                    if (strRoomampm != string.Empty)
                                    {
                                        if (strRoomampm != enrlrow.Site + enrlrow.Room + enrlrow.AMPM)
                                        {
                                            strRoomampm = enrlrow.Site + enrlrow.Room + enrlrow.AMPM;
                                            if (hssb2103_Table2.Rows.Count > 0)
                                            {
                                                document.Add(hssb2103_Table2);
                                                document.NewPage();
                                                hssb2103_Table2.DeleteBodyRows();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strRoomampm = enrlrow.Site + enrlrow.Room + enrlrow.AMPM;
                                    }
                                }

                                strZipcode = string.Empty;
                                PdfPCell pdfAppldata = new PdfPCell(new Phrase(enrlrow.App, TableFont));
                                pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAppldata.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                hssb2103_Table2.AddCell(pdfAppldata);

                                if (rdoZipAll.Checked == false)
                                    strZipcode = enrlrow.Mst_Zip;

                                PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat) + "  " + strZipcode, TableFont));
                                pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfChildName.Colspan = 1;
                                pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfChildName);

                                PdfPCell pdfDob = new PdfPCell(new Phrase(LookupDataAccess.Getdate(enrlrow.Snp_DOB), TableFont));
                                pdfDob.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfDob);


                                PdfPCell pdfR = new PdfPCell(new Phrase((enrlrow.Chld_Repeater.ToString().ToUpper()), TableFont));//== "TRUE" ? "Y" : ""
                                pdfR.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfR.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfR);

                                PdfPCell pdfW = new PdfPCell(new Phrase((enrlrow.Status == "W" ? "Y" : "N"), TableFont));
                                pdfW.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfW.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfW);

                                PdfPCell pdfAi = new PdfPCell(new Phrase((""), TableFont));
                                pdfAi.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAi.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfAi);

                                PdfPCell pdfAttenDt = new PdfPCell(new Phrase((LookupDataAccess.Getdate(enrlrow.AttendedDt)), TableFont));
                                pdfAttenDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfAttenDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfAttenDt);

                                PdfPCell pdfSDt = new PdfPCell(new Phrase(enrlrow.Status + " " + LookupDataAccess.Getdate(enrlrow.Enrl_Date), TableFont));
                                pdfSDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfSDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfSDt);

                                PdfPCell pdfSiteRoom = new PdfPCell(new Phrase(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, TableFont));
                                pdfSiteRoom.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfSiteRoom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfSiteRoom);

                                PdfPCell pdfBusrtedesc = new PdfPCell(new Phrase(enrlrow.BusRoot, TableFont));
                                pdfBusrtedesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfBusrtedesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfBusrtedesc);

                                CommonEntity commonfund = propfundingSource.Find(u => u.Code == enrlrow.FundHie);
                                if(commonfund!=null)
                                {                            

                                    PdfPCell pdfFundSource = new PdfPCell(new Phrase(commonfund.Desc.ToString(), TableFont));
                                pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfFundSource.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                hssb2103_Table2.AddCell(pdfFundSource);
                                }
                                else
                                {
                                  PdfPCell pdfFundSource = new PdfPCell(new Phrase("", TableFont));
                                pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfFundSource.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                hssb2103_Table2.AddCell(pdfFundSource);
                                }

                                if (rdoNoTask.Checked == false)
                                {

                                    List<ChldMediEntity> chldMediListEntity = new List<ChldMediEntity>();

                                    List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog) && (u.APP_NO == enrlrow.App));

                                    if (chldMediEntity.Count > 0)
                                    {

                                        PdfPTable hssb2103_TableData = new PdfPTable(6);
                                        hssb2103_TableData.WidthPercentage = 100;
                                        float[] widthsData = new float[] { 50f, 50f, 50f, 50f, 50f, 50f };
                                        hssb2103_TableData.SetWidths(widthsData);
                                        hssb2103_TableData.HorizontalAlignment = Element.ALIGN_CENTER;

                                        PdfPTable nestedtask1 = new PdfPTable(1);
                                        float[] nestwidthstask1 = new float[] { 50f };
                                        nestedtask1.SetWidths(nestwidthstask1);
                                        nestedtask1.WidthPercentage = 100;

                                        PdfPTable nestedtask2 = new PdfPTable(1);
                                        float[] nestwidthstask2 = new float[] { 50f };
                                        nestedtask2.SetWidths(nestwidthstask2);
                                        nestedtask2.WidthPercentage = 100;

                                        PdfPTable nestedtask3 = new PdfPTable(1);
                                        float[] nestwidthstask3 = new float[] { 50f };
                                        nestedtask3.SetWidths(nestwidthstask3);
                                        nestedtask3.WidthPercentage = 100;

                                        PdfPTable nestedtask4 = new PdfPTable(1);
                                        float[] nestwidthstask4 = new float[] { 50f };
                                        nestedtask4.SetWidths(nestwidthstask4);
                                        nestedtask4.WidthPercentage = 100;

                                        PdfPTable nestedtask5 = new PdfPTable(1);
                                        float[] nestwidthstask5 = new float[] { 50f };
                                        nestedtask5.SetWidths(nestwidthstask5);
                                        nestedtask5.WidthPercentage = 100;

                                        PdfPTable nestedtask6 = new PdfPTable(1);
                                        float[] nestwidthstask6 = new float[] { 50f };
                                        nestedtask6.SetWidths(nestwidthstask6);
                                        nestedtask6.WidthPercentage = 100;


                                        chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[0].TASK.Trim());
                                        List<CaseNotesEntity> caseNotesEntity;

                                        if (chldMediListEntity.Count > 0)
                                        {
                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                nestedtask1.AddCell(Header1);
                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask1.AddCell(Header2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK +item.SEQ);
                                                        if (caseNotesEntity.Count>0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask1.AddCell(Header2);
                                                        }
                                                    }
                                                }
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }

                                            PdfPCell firsttask = new PdfPCell(nestedtask1);
                                            firsttask.Padding = 0f;
                                            firsttask.HorizontalAlignment = Element.ALIGN_CENTER;
                                            firsttask.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            hssb2103_TableData.AddCell(firsttask);
                                        }
                                        else
                                        {
                                            PdfPCell Header1 = new PdfPCell(new Phrase("", TableFont));
                                            Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            hssb2103_TableData.AddCell(Header1);
                                        }
                                        if (SeltrackList.Count > 1)
                                        {
                                            // chldmediFilter = chldMediEntity.Find(u => u.TASK.Trim() == SeltrackList[1].TASK.Trim());
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[1].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask2.AddCell(Header1);

                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask2.AddCell(Header2);
                                                        }
                                                    }
                                                    else {

                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK + item.SEQ);
                                                        if (caseNotesEntity.Count > 0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask2.AddCell(Header2);
                                                        }
                                                    }
                                                }

                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }
                                            if (chldMediListEntity.Count > 0)
                                            {
                                                PdfPCell secondTask = new PdfPCell(nestedtask2);
                                                secondTask.Padding = 0f;
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                            else
                                            {
                                                PdfPCell secondTask = new PdfPCell(new Phrase("", TableFont));
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }

                                        }
                                        else
                                        {
                                            PdfPCell Header2 = new PdfPCell(new Phrase("", TableFont));
                                            Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header2);
                                        }
                                        if (SeltrackList.Count > 2)
                                        {

                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[2].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask3.AddCell(Header1);
                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask3.AddCell(Header2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK + item.SEQ);
                                                        if (caseNotesEntity.Count > 0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask3.AddCell(Header2);
                                                        }
                                                    }
                                                }
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }
                                            if (chldMediListEntity.Count > 0)
                                            {
                                                PdfPCell secondTask = new PdfPCell(nestedtask3);
                                                secondTask.Padding = 0f;
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                            else
                                            {
                                                PdfPCell secondTask = new PdfPCell(new Phrase("", TableFont));
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell Header3 = new PdfPCell(new Phrase("", TableFont));
                                            Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header3);
                                        }
                                        if (SeltrackList.Count > 3)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[3].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask4.AddCell(Header1);
                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask4.AddCell(Header2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK + item.SEQ);
                                                        if (caseNotesEntity.Count > 0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask4.AddCell(Header2);
                                                        }
                                                    }
                                                }
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }
                                            if (chldMediListEntity.Count > 0)
                                            {
                                                PdfPCell secondTask = new PdfPCell(nestedtask4);
                                                secondTask.Padding = 0f;
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                            else
                                            {
                                                PdfPCell secondTask = new PdfPCell(new Phrase("", TableFont));
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell Header4 = new PdfPCell(new Phrase("", TableFont));
                                            Header4.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header4);
                                        }
                                        if (SeltrackList.Count > 4)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[4].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask5.AddCell(Header1);
                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask5.AddCell(Header2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK + item.SEQ);
                                                        if (caseNotesEntity.Count > 0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask5.AddCell(Header2);
                                                        }
                                                    
                                                    }
                                                }
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }
                                            if (chldMediListEntity.Count > 0)
                                            {
                                                PdfPCell secondTask = new PdfPCell(nestedtask5);
                                                secondTask.Padding = 0f;
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                            else
                                            {
                                                PdfPCell secondTask = new PdfPCell(new Phrase("", TableFont));
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }

                                        }
                                        else
                                        {
                                            PdfPCell Header5 = new PdfPCell(new Phrase("", TableFont));
                                            Header5.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header5);
                                        }
                                        if (SeltrackList.Count > 5)
                                        {
                                            chldMediListEntity = chldMediEntity.FindAll(u => u.TASK.Trim() == SeltrackList[5].TASK.Trim());

                                            foreach (ChldMediEntity item in chldMediListEntity)
                                            {
                                                PdfPCell Header1 = new PdfPCell(new Phrase(TaskDates(item), TableFont));
                                                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                nestedtask6.AddCell(Header1);
                                                if (chkIncludeNotes.Checked)
                                                {
                                                    if (rdoAllAplicants.Checked == true)
                                                    {
                                                        if (item.NotesDesc.ToString().Trim() != string.Empty)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(item.NotesDesc.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask6.AddCell(Header2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        caseNotesEntity = _model.TmsApcndata.GetCaseNotesScreenFieldName("HSS00134", item.AGENCY + item.DEPT + item.PROG + item.YEAR + item.APP_NO + item.TASK + item.SEQ);
                                                        if (caseNotesEntity.Count > 0)
                                                        {
                                                            PdfPCell Header2 = new PdfPCell(new Phrase(caseNotesEntity[0].Data.ToString(), TableFont));
                                                            Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            nestedtask6.AddCell(Header2);
                                                        }
                                                    }
                                                }
                                                if (rdoOnlyLutd.Checked)
                                                    break;
                                            }
                                            if (chldMediListEntity.Count > 0)
                                            {
                                                PdfPCell secondTask = new PdfPCell(nestedtask6);
                                                secondTask.Padding = 0f;
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                            else
                                            {
                                                PdfPCell secondTask = new PdfPCell(new Phrase("", TableFont));
                                                secondTask.HorizontalAlignment = Element.ALIGN_CENTER;
                                                secondTask.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                hssb2103_TableData.AddCell(secondTask);
                                            }
                                        }
                                        else
                                        {
                                            PdfPCell Header6 = new PdfPCell(new Phrase("", TableFont));
                                            Header6.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Header6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            hssb2103_TableData.AddCell(Header6);
                                        }

                                        PdfPCell TaskData = new PdfPCell(hssb2103_TableData);
                                        TaskData.Colspan = 11;
                                        TaskData.Padding = 0f;
                                        TaskData.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        hssb2103_Table2.AddCell(TaskData);

                                        PdfPCell TaskSpace = new PdfPCell();
                                        TaskSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                        TaskSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        TaskSpace.Colspan = 11;
                                        hssb2103_Table2.AddCell(TaskSpace);

                                    }

                                    else
                                    {
                                        PdfPCell TaskData = new PdfPCell();
                                        TaskData.HorizontalAlignment = Element.ALIGN_CENTER;
                                        TaskData.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + +iTextSharp.text.Rectangle.LEFT_BORDER;
                                        TaskData.Colspan = 11;
                                        hssb2103_Table2.AddCell(TaskData);
                                    }
                                }
                                else
                                {
                                    PdfPCell TaskData = new PdfPCell();
                                    TaskData.HorizontalAlignment = Element.ALIGN_CENTER;
                                    TaskData.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + +iTextSharp.text.Rectangle.LEFT_BORDER;
                                    TaskData.Colspan = 11;
                                    hssb2103_Table2.AddCell(TaskData);

                                }

                            }
                        }
                        if (hssb2103_Table2.Rows.Count > 0)
                        {

                            document.Add(hssb2103_Table2);
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


        public string TaskDates(ChldMediEntity chldmedi)
        {
            string strDate = string.Empty;
            string strAnswers = string.Empty;
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
            if (chldmedi.ANSWER1 != string.Empty)
            {
                strAnswers = chldmedi.ANSWER1;
            }
            else if (chldmedi.ANSWER2 != string.Empty)
            {
                if (Convert.ToDecimal(chldmedi.ANSWER2) > 0)
                {
                    strAnswers = chldmedi.ANSWER2;
                }
            }
            else if (chldmedi.ANSWER3 != string.Empty)
            {
                strAnswers = LookupDataAccess.Getdate(chldmedi.ANSWER3);
            }
            return strDate + "  " + strAnswers;

        }
        StringBuilder strxmlReportlog = new StringBuilder();
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

           

            strxmlReportlog.Append("<Rows>");
           
            PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);

            strxmlReportlog.Append("<Row>" + Txt_HieDesc.Text.Trim() + "     " + Header_year + "</Row>");*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Dept != "**") Dept = /*"Dept : " + */Dept;
            if (Prog != "**") Prg = /*"Program : " +*/ Prog;
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

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
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            string strChildName = string.Empty;
            string strChildwith = string.Empty;
            string strChildStatus = string.Empty;

            if (rdoEnrolled.Checked)
                strChildStatus = "Enrolled Children";
            else if (rdoWaitList.Checked)
                strChildStatus = "Wait List Children";
            else if (rdoChildren.Checked)
                strChildStatus = "All Childrens";

            if (rdoReportChildName.Checked)
                strChildName = "Child Name";
            else if (rdoApplicant.Checked)
                strChildName = "Applicant";
            else if (rdoClass.Checked)
                strChildName = "Class";
            if (rdoBoth.Checked)
                strChildwith = "Both Selected & No Task Data";
            else if (rdoSelectask.Checked)
                strChildwith = "Selected Task Data";
            else if (rdoNoTask.Checked)
                strChildwith = "No Task Data";

           
            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Report Sequence" /*+ strChildName*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(strChildName, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            //strxmlReportlog.Append("<Row> Report Sequence : " + strChildName + "</Row>");

            if (rdoAllAplicants.Checked)
            {
                PdfPCell RCompleted = new PdfPCell(new Phrase("  " + "Report Type" /*All Applicants "*/, TableFont));
                RCompleted.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RCompleted.PaddingBottom = 5;
                Headertable.AddCell(RCompleted);
                //strxmlReportlog.Append("<Row><![CDATA[ Report Type :  All Applicants ]]></Row>");

                PdfPCell RCompleted1 = new PdfPCell(new Phrase("All Applicants", TableFont));
                RCompleted1.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RCompleted1.PaddingBottom = 5;
                Headertable.AddCell(RCompleted1);
            }
            if (rdoSelectApplicant.Checked)
            {
                PdfPCell RCompleted = new PdfPCell(new Phrase("  " + "Report Type" /*:  Selected Applicant No " + txtApplicant.Text*/, TableFont));
                RCompleted.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RCompleted.PaddingBottom = 5;
                Headertable.AddCell(RCompleted);
                //strxmlReportlog.Append("<Row><![CDATA[ Report Type :  Selected Applicant No " + txtApplicant.Text + " ]]></Row>");


                PdfPCell RCompleted1 = new PdfPCell(new Phrase("Selected Applicant No: " + txtApplicant.Text, TableFont));
                RCompleted1.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RCompleted1.PaddingBottom = 5;
                Headertable.AddCell(RCompleted1);
            }


            PdfPCell Rchildren = new PdfPCell(new Phrase("  " + "Children With" /*+ strChildwith*/, TableFont));
            Rchildren.HorizontalAlignment = Element.ALIGN_LEFT;
            Rchildren.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rchildren.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rchildren.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Rchildren.PaddingBottom = 5;
            Headertable.AddCell(Rchildren);

            PdfPCell Rchildren1 = new PdfPCell(new Phrase(strChildwith, TableFont));
            Rchildren1.HorizontalAlignment = Element.ALIGN_LEFT;
            Rchildren1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rchildren1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rchildren1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            Rchildren1.PaddingBottom = 5;
            Headertable.AddCell(Rchildren1);

            //strxmlReportlog.Append("<Row><![CDATA[ Children With : " + strChildwith + "]]></Row>");

            if (rdoSelectask.Checked)
            {
                PdfPCell RCompleted = new PdfPCell(new Phrase("  " + "Completed/Not Completed Tasks for Year" /*+ txtCompleTask.Text*/, TableFont));
                RCompleted.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RCompleted.PaddingBottom = 5;
                Headertable.AddCell(RCompleted);
                // strxmlReportlog.Append("<Row><![CDATA[ Completed/Not Completed Tasks for Year : " + txtCompleTask.Text + "]]></Row>");

                PdfPCell RCompleted1 = new PdfPCell(new Phrase(txtCompleTask.Text, TableFont));
                RCompleted1.HorizontalAlignment = Element.ALIGN_LEFT;
                RCompleted1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCompleted1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCompleted1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RCompleted1.PaddingBottom = 5;
                Headertable.AddCell(RCompleted1);
            }

            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Max. # of Tasks" /*+ ((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Text.ToString()*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Text.ToString(), TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            //strxmlReportlog.Append("<Row><![CDATA[ Max. # of Tasks : " + ((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Text.ToString() + "]]></Row>");

            string strTaskCodes = string.Empty;
            foreach (ChldTrckEntity Entity in SeltrackList)
            {
                if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ", ";
                strTaskCodes += Entity.TASKDESCRIPTION.Trim();
            }

            PdfPCell RTasks = new PdfPCell(new Phrase("  " + "Selected Tasks" /*+ strTaskCodes*/, TableFont));
            RTasks.HorizontalAlignment = Element.ALIGN_LEFT;
            RTasks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RTasks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RTasks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RTasks.PaddingBottom = 5;
            Headertable.AddCell(RTasks);

            PdfPCell RTasks1 = new PdfPCell(new Phrase(strTaskCodes, TableFont));
            RTasks1.HorizontalAlignment = Element.ALIGN_LEFT;
            RTasks1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RTasks1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RTasks1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RTasks1.PaddingBottom = 5;
            Headertable.AddCell(RTasks1);

            //strxmlReportlog.Append("<Row><![CDATA[ Selected Tasks : " + strTaskCodes + "]]></Row>");

            if (((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString() == "2")
            {
                PdfPCell RCaseNotes = new PdfPCell(new Phrase("  " + "Include Case Notes" /*+ (chkIncludeNotes.Checked == true ? "Yes" : "No")*/, TableFont));
                RCaseNotes.HorizontalAlignment = Element.ALIGN_LEFT;
                RCaseNotes.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCaseNotes.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCaseNotes.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                RCaseNotes.PaddingBottom = 5;
                Headertable.AddCell(RCaseNotes);

                PdfPCell RCaseNotes1 = new PdfPCell(new Phrase((chkIncludeNotes.Checked == true ? "Yes" : "No"), TableFont));
                RCaseNotes1.HorizontalAlignment = Element.ALIGN_LEFT;
                RCaseNotes1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RCaseNotes1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RCaseNotes1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RCaseNotes1.PaddingBottom = 5;
                Headertable.AddCell(RCaseNotes1);
                //strxmlReportlog.Append("<Row><![CDATA[ Include Case Notes : " + (chkIncludeNotes.Checked == true ? "Yes" : "No") + "]]></Row>");
            }


            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Children Age" /*from " + txtFrom.Text + " to " + txtTo.Text + ", Base on " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text)*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase("From: " + txtFrom.Text + "   To: " + txtTo.Text + ", Base Age On: " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            //strxmlReportlog.Append("<Row><![CDATA[ Children age from " + txtFrom.Text + " to " + txtTo.Text + ", Base on " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text) + "]]></Row>");


            PdfPCell RDates = new PdfPCell(new Phrase("  " + "Show Task Dates" /*+ (rdoOnlyLutd.Checked == true ? rdoOnlyLutd.Text : rdoAlltaskDate.Text)*/, TableFont));
            RDates.HorizontalAlignment = Element.ALIGN_LEFT;
            RDates.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RDates.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RDates.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RDates.PaddingBottom = 5;
            Headertable.AddCell(RDates);

            PdfPCell RDates1 = new PdfPCell(new Phrase((rdoOnlyLutd.Checked == true ? rdoOnlyLutd.Text : rdoAlltaskDate.Text), TableFont));
            RDates1.HorizontalAlignment = Element.ALIGN_LEFT;
            RDates1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RDates1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RDates1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RDates1.PaddingBottom = 5;
            Headertable.AddCell(RDates1);

            //strxmlReportlog.Append("<Row><![CDATA[Show Task Dates : " + (rdoOnlyLutd.Checked == true ? rdoOnlyLutd.Text : rdoAlltaskDate.Text) + "]]></Row>");

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Children" /*+ strChildStatus*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase(strChildStatus, TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            //strxmlReportlog.Append("<Row><![CDATA[Children : " + strChildStatus + "]]></Row>");

            PdfPCell Ractive = new PdfPCell(new Phrase("  " + "Active/Inactive" /*+ ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString()*/, TableFont));
            Ractive.HorizontalAlignment = Element.ALIGN_LEFT;
            Ractive.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Ractive.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Ractive.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Ractive.PaddingBottom = 5;
            Headertable.AddCell(Ractive);

            PdfPCell Ractive1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString(), TableFont));
            Ractive1.HorizontalAlignment = Element.ALIGN_LEFT;
            Ractive1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Ractive1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Ractive1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            Ractive1.PaddingBottom = 5;
            Headertable.AddCell(Ractive1);

            //strxmlReportlog.Append("<Row><![CDATA[ Active/Inactive : " + ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString() + "]]></Row>");

            string strZip = string.Empty;
            if (rdoZipAll.Checked)
                strZip = "All";
            else if (rdoZipSelected.Checked)
            {
                string strselectzipcode = string.Empty;
                foreach (ZipCodeEntity Entity in selZipcodeList)
                {
                    strselectzipcode += Entity.Zcrzip + "  ";//(!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty);
                }
                strZip = "Selected ZIP Codes: " + strselectzipcode;
            }
            else if (rdoSkipSelected.Checked)
            {
                string strselectzipcode = string.Empty;
                foreach (ZipCodeEntity Entity in selZipcodeList)
                {
                    strselectzipcode += Entity.Zcrzip + "  ";//(!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty);
                }
                strZip = "Skip ZIP Codes: " + strselectzipcode;
            }

            PdfPCell RZip = new PdfPCell(new Phrase("  " + "ZIP Codes", TableFont));
            RZip.HorizontalAlignment = Element.ALIGN_LEFT;
            RZip.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RZip.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RZip.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RZip.PaddingBottom = 5;
            Headertable.AddCell(RZip);

            PdfPCell RZip1 = new PdfPCell(new Phrase(strZip, TableFont));
            RZip1.HorizontalAlignment = Element.ALIGN_LEFT;
            RZip1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RZip1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RZip1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RZip1.PaddingBottom = 5;
            Headertable.AddCell(RZip1);

            //strxmlReportlog.Append("<Row><![CDATA[" + strZip + "]]></Row>");

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
                Site = "Selected Sites: " + rdoMultipleSites.Text + " ( " + Selsites + " ) ";
            }

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Sites", TableFont));
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

            //strxmlReportlog.Append("<Row><![CDATA[" + Site + "]]></Row>");

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

            //strxmlReportlog.Append("<Row><![CDATA[" + strFundingSource + "]]></Row>");

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("'R' = Repeater, 'WL' = Waiting List, 'A/I' = Active/Inactive", TableFont), 47, 470, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("'S'=Status ('E' = Enrolled, 'W' = Withdrawn, 'N' = Not Enrolled)", TableFont), 47, 450, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Date Codes ('C' = Date Completed, 'F' = Date of Follow Up, 'A' = Date Addressed)", TableFont), 47, 430, 0);


            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

            /* PdfPCell R8 = new PdfPCell(new Phrase("'R' = Repeater, 'WL' = Waiting List, 'A/I' = Active/Inactive", TableFont));
             R8.HorizontalAlignment = Element.ALIGN_LEFT;
             R8.Colspan = 2;
             R8.Border = iTextSharp.text.Rectangle.NO_BORDER;
             Headertable.AddCell(R8);

             //strxmlReportlog.Append("<Row><![CDATA['R'=Repeater, WL=Waiting List, 'A/I'=Active/Inactive]]></Row>");

             PdfPCell R9 = new PdfPCell(new Phrase("'S'=Status ('E' = Enrolled, 'W' = Withdrawn, 'N' = Not Enrolled)", TableFont));
             R9.HorizontalAlignment = Element.ALIGN_LEFT;
             R9.Colspan = 2;
             R9.Border = iTextSharp.text.Rectangle.NO_BORDER;
             Headertable.AddCell(R9);

             //strxmlReportlog.Append("<Row><![CDATA['S'=Status ('E'=Enrolled, 'W'=Withdrawn, 'N'=Not Enrolled)]]></Row>");

             PdfPCell R10 = new PdfPCell(new Phrase("Date Codes ('C' = Date Completed, 'F' = Date of Follow Up, 'A' = Date Addressed)", TableFont));
             R10.HorizontalAlignment = Element.ALIGN_LEFT;
             R10.Colspan = 2;
             R10.Border = iTextSharp.text.Rectangle.NO_BORDER;
             Headertable.AddCell(R10);            
             document.Add(Headertable);*/

            // strxmlReportlog.Append("<Row><![CDATA[Date Codes ('C'=Date Completed, 'F'=Date of Follow Up, 'A'=Date Addressed)]]></Row>");

            //strxmlReportlog.Append("</Rows>");
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



        private void btnTask_Click(object sender, EventArgs e)
        {
            HSSB2111FundForm TaskForm = new HSSB2111FundForm(BaseForm, SeltrackList, Privileges, string.Empty, ((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString() == "1" ? "10" : "6", string.Empty);
            TaskForm.FormClosed += new FormClosedEventHandler(TaskForm_FormClosed);
            TaskForm.StartPosition = FormStartPosition.CenterScreen;
            TaskForm.ShowDialog();
        }

        List<ChldTrckEntity> SeltrackList = new List<ChldTrckEntity>();
        private void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SeltrackList = form.GetSelectedTracks();
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
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "A", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "A", "Reports", BaseForm.UserID);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(855, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(755, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(855, 25);
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

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(btnBrowse, null);
            _errorProvider.SetError(txtCompleTask, null);
            _errorProvider.SetError(rdoMultipleSites, null);
            _errorProvider.SetError(btnTasks, null);
            _errorProvider.SetError(rdoZipSelected, null);
            _errorProvider.SetError(rdoSkipSelected, null);
            _errorProvider.SetError(rdoFundingSelect, null);
            _errorProvider.SetError(txtTo, null);

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
            if (SeltrackList.Count == 0)
            {
                _errorProvider.SetError(btnTasks, "Select at least One Task");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(btnTasks, null);
            }
            if (rdoZipSelected.Checked == true)
            {
                if (selZipcodeList.Count == 0)
                {
                    _errorProvider.SetError(rdoZipSelected, "Select at least One ZIP Code");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoZipSelected, null);
                }
            }
            if (rdoSkipSelected.Checked == true)
            {
                if (selZipcodeList.Count == 0)
                {
                    _errorProvider.SetError(rdoSkipSelected, "Select at least One ZIP Code");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoSkipSelected, null);
                }
            }
            if (rdoFundingSelect.Checked == true)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rdoFundingSelect, "Select at least One Fund");
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
            string MaxTask = ((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString();
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strZipCodeType = string.Empty;
            string strReportSequence = string.Empty;
            string strChildrenwith = string.Empty;
            string strEnrollType = string.Empty;

            if (rdoZipAll.Checked == true)
                strZipCodeType = "A";
            else if (rdoZipSelected.Checked == true)
                strZipCodeType = "Z";
            else if (rdoSkipSelected.Checked == true)
                strZipCodeType = "S";


            if (rdoReportChildName.Checked == true)
                strReportSequence = "N";
            else if (rdoApplicant.Checked == true)
                strReportSequence = "A";
            else if (rdoClass.Checked == true)
                strReportSequence = "C";

            string strBaseAge = rdoTodayDate.Checked ? "T" : "K";

            if (rdoBoth.Checked == true)
                strChildrenwith = "B";
            else if (rdoSelectask.Checked == true)
                strChildrenwith = "S";
            else if (rdoNoTask.Checked == true)
                strChildrenwith = "N";


            if (rdoEnrolled.Checked == true)
                strEnrollType = "E";
            else if (rdoWaitList.Checked == true)
                strEnrollType = "W";
            else if (rdoChildren.Checked == true)
                strEnrollType = "A";

            string strFundSource = rdoFundingAll.Checked ? "Y" : "N";
            string strIncludeCaseNotes = chkIncludeNotes.Checked ? "Y" : "N";
            string strShow = rdoOnlyLutd.Checked ? "Y" : "N";
            string Casenotes = string.Empty;

            if (((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString() == "2")
            {
                if (chkIncludeNotes.Checked)
                    Casenotes = "Y";
                else
                    Casenotes = "N";
            }

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

            string strZipCodes = string.Empty;
            if (rdoZipSelected.Checked)
            {
                foreach (ZipCodeEntity Entity in selZipcodeList)
                {
                    if (!strZipCodes.Equals(string.Empty)) strZipCodes += ",";
                    strZipCodes += Entity.Zcrzip;//(!string.IsNullOrEmpty(Entity.Zcrplus4.Trim()) ? Entity.Zcrplus4 : string.Empty);
                }
            }

            string strTaskCodes = string.Empty;
            foreach (ChldTrckEntity Entity in SeltrackList)
            {
                if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                strTaskCodes += Entity.TASK;
            }



            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" ZipCodeType = \"" + strZipCodeType + "\"  CompletTask = \"" + txtCompleTask.Text.Trim() + "\"  MaxofTask = \"" + MaxTask + "\" Active = \"" + Active + "\" ReportSequence = \"" + strReportSequence +
                            "\" AgeFrom = \"" + txtFrom.Text + "\" AgeTo = \"" + txtTo.Text + "\" BaseAge = \"" + strBaseAge + "\"  Children = \"" + strChildrenwith + 
                            "\"   EnrollType = \"" + strEnrollType + "\" FundedSource = \"" + strFundSource + "\" IncludeCaseNotes = \"" + strIncludeCaseNotes +
                            "\"  Show = \"" + strShow + "\"  SiteNames = \"" + strsiteRoomNames + "\" FundingCode = \"" + strFundingCodes + "\" CaseNotes = \"" + Casenotes +
                            "\"  ZipCodeNames = \"" + strZipCodes + "\" TaskNames = \"" + strTaskCodes + "\" ApplicantNo = \"" + txtApplicant.Text + "\" />");

            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbMaxoftask, dr["MaxofTask"].ToString());
                CommonFunctions.SetComboBoxValue(cmbActive, dr["Active"].ToString());

                if (dr["ApplicantNo"].ToString().Trim() == string.Empty)
                {
                    rdoAllAplicants.Checked = true;
                }
                else
                {
                    rdoSelectApplicant.Checked = true;
                    txtApplicant.Text = dr["ApplicantNo"].ToString();
                }

                if (dr["CaseNotes"].ToString().Trim() == "Y")
                    chkIncludeNotes.Checked = true;
                else
                    chkIncludeNotes.Checked = false;

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
                            if (siteDetails != null)
                                Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }
                Sel_REFS_List = Sel_REFS_List;
                if (dr["ZipCodeType"].ToString() == "A")
                    rdoZipAll.Checked = true;

                else if (dr["ZipCodeType"].ToString() == "Z")
                {
                    rdoZipSelected.Checked = true;
                    propZipCode = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
                    selZipcodeList.Clear();
                    string strZipcode = dr["ZipCodeNames"].ToString();
                    List<string> ZipcodeList = new List<string>();
                    if (strZipcode != null)
                    {
                        string[] Zipplus = strZipcode.Split(',');
                        for (int i = 0; i < Zipplus.Length; i++)
                        {
                            ZipCodeEntity ZipDetails = propZipCode.Find(u => u.Zcrzip == Zipplus.GetValue(i).ToString());
                            if (ZipDetails != null)
                                selZipcodeList.Add(ZipDetails);
                        }
                    }
                    selZipcodeList = selZipcodeList;
                }

                else if (dr["ZipCodeType"].ToString() == "S")
                {
                    rdoSkipSelected.Checked = true;
                    propZipCode = _model.ZipCodeAndAgency.GetZipCodeSearch(string.Empty, string.Empty, string.Empty, string.Empty);
                    selZipcodeList.Clear();
                    string strZipcode = dr["ZipCodeNames"].ToString();
                    List<string> ZipcodeList = new List<string>();
                    if (strZipcode != null)
                    {
                        string[] Zipplus = strZipcode.Split(',');
                        for (int i = 0; i < Zipplus.Length; i++)
                        {
                            ZipCodeEntity ZipDetails = propZipCode.Find(u => u.Zcrzip == Zipplus.GetValue(i).ToString());
                            if (ZipDetails != null)
                                selZipcodeSkipList.Add(ZipDetails);
                        }
                    }
                    selZipcodeSkipList = selZipcodeSkipList;
                }


                if (dr["ReportSequence"].ToString() == "N")
                    rdoReportChildName.Checked = true;
                else if (dr["ReportSequence"].ToString() == "A")
                    rdoApplicant.Checked = true;
                else if (dr["ReportSequence"].ToString() == "C")
                    rdoClass.Checked = true;


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
                            if (fundDetails != null)
                                SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                List<ChldTrckEntity> chldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);

                string strTasks = dr["TaskNames"].ToString();
                List<string> siteTaskList = new List<string>();
                if (strTasks != null)
                {
                    string[] TaskCodes = strTasks.Split(',');
                    for (int i = 0; i < TaskCodes.Length; i++)
                    {
                        ChldTrckEntity TaskDetails = chldTrckList.Find(u => u.TASK.Trim() == TaskCodes.GetValue(i).ToString());
                        if (TaskDetails != null)
                            SeltrackList.Add(TaskDetails);
                    }
                }
                SeltrackList = SeltrackList;

                //txtApplicant.Text = dr["ApplicantNo"].ToString();
                txtFrom.Text = dr["AgeFrom"].ToString();
                txtTo.Text = dr["AgeTo"].ToString();
                txtCompleTask.Text = dr["CompletTask"].ToString();

                if (dr["BaseAge"].ToString() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["Children"].ToString() == "B")
                    rdoBoth.Checked = true;
                else if (dr["Children"].ToString() == "S")
                    rdoSelectask.Checked = true;
                else if (dr["Children"].ToString() == "N")
                    rdoNoTask.Checked = true;

                if (dr["EnrollType"].ToString() == "E")
                    rdoEnrolled.Checked = true;
                else if (dr["EnrollType"].ToString() == "W")
                    rdoWaitList.Checked = true;
                else if (dr["EnrollType"].ToString() == "A")
                    rdoChildren.Checked = true;

                if (dr["Show"].ToString() == "Y")
                    rdoOnlyLutd.Checked = true;
                else
                    rdoAlltaskDate.Checked = true;

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

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rdoZipSelected_Click(object sender, EventArgs e)
        {
            if (rdoZipSelected.Checked == true)
            {
                SelectZipSiteCountyForm zipcodeform = new SelectZipSiteCountyForm(BaseForm, selZipcodeList, Privileges, "Selected");
                zipcodeform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                zipcodeform.StartPosition = FormStartPosition.CenterScreen;
                zipcodeform.ShowDialog();
            }
        }
        List<ZipCodeEntity> selZipcodeList = new List<ZipCodeEntity>();
        List<ZipCodeEntity> selZipcodeSkipList = new List<ZipCodeEntity>();
        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "ZIPCODE")
                {
                    if (rdoZipSelected.Checked)
                    {
                        selZipcodeList = form.SelectedZipcodeEntity;
                        selZipcodeSkipList = form.SelectedZipcodeSkipEntity;
                    }
                    else if (rdoSkipSelected.Checked)
                    {
                        selZipcodeList = form.SelectedZipcodeEntity;
                        selZipcodeSkipList = form.SelectedZipcodeSkipEntity;
                    }
                }
            }
        }

        private void rdoSkipSelected_Click(object sender, EventArgs e)
        {
            if (rdoSkipSelected.Checked == true)
            {
                SelectZipSiteCountyForm zipcodeform = new SelectZipSiteCountyForm(BaseForm, selZipcodeSkipList, Privileges, "SkipSelected");
                zipcodeform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                zipcodeform.StartPosition = FormStartPosition.CenterScreen;
                zipcodeform.ShowDialog();
            }
        }

        private void rdoSelectask_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSelectask.Checked)
            {
                txtCompleTask.Enabled = true;
                txtCompleTask.Visible = true;
                lblcompTask.Visible = true;
            }
            else
            {
                txtCompleTask.Text = "****";
                txtCompleTask.Enabled = false;
                txtCompleTask.Visible = false;
                lblcompTask.Visible = false;
            }
        }

        private void cmbMaxoftask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbMaxoftask.SelectedItem).Value.ToString() == "2")
            {
                chkIncludeNotes.Enabled = true;
                chkIncludeNotes.Visible = true;
            }
            else
            {
                chkIncludeNotes.Enabled = false;
                chkIncludeNotes.Checked = false;
                chkIncludeNotes.Visible = false;
            }
        }

        private void txtCompleTask_Leave(object sender, EventArgs e)
        {
            if (txtCompleTask.Text != "****")
            {
                Regex regex = new Regex("[0-9]");
                if (regex.IsMatch(txtCompleTask.Text))
                {
                    if (txtCompleTask.TextLength == 4)
                    {
                        if (!((Convert.ToInt32(txtCompleTask.Text) > 1899) && (Convert.ToInt32(txtCompleTask.Text) < 2100)))
                        {
                            AlertBox.Show("Invalid Year", MessageBoxIcon.Warning);
                            txtCompleTask.Text = "****";
                        }
                    }
                    else
                    {
                        AlertBox.Show("Invalid Year", MessageBoxIcon.Warning);
                        txtCompleTask.Text = "****";
                    }
                }
                else
                {
                    txtCompleTask.Text = "****";
                }
            }
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2103");
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllSite.Checked)
            {
                _errorProvider.SetError(rdoSelectask, null);
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

        private void rdoZipAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoZipAll.Checked)
            {
                _errorProvider.SetError(rdoZipSelected, null);
                _errorProvider.SetError(rdoSkipSelected, null);
                selZipcodeSkipList.Clear();
                selZipcodeList.Clear();
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

        private void HSSB2103ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void rdoSkipSelected_CheckedChanged(object sender, EventArgs e)
        {
            selZipcodeList.Clear();
            _errorProvider.SetError(rdoZipSelected, null);
        }

        private void rdoZipSelected_CheckedChanged(object sender, EventArgs e)
        {
            selZipcodeSkipList.Clear();
            _errorProvider.SetError(rdoSkipSelected, null);
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

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
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

    }
}