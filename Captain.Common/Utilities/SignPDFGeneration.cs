/****************************************************************************************************
 * Class Name    :SignPDFGeneration
 * Author        : Kranthi
 * Created Date  : 
 * Version       : 1.0.0
 * Description   : Used to generated Signature need PDF Files
 ****************************************************************************************************/

using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.DatabaseLayer;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Web;
using Wisej.Web;
using System.Data;
using System.Globalization;
using Captain.Common.Views.Forms;
using System.Security.Cryptography.X509Certificates;
using Aspose.Cells;

namespace Captain.Common.Utilities
{
    public class SignPDFGeneration
    {
        #region properties
        // public BaseForm _baseForm { get; set; }
        public PrivilegeEntity _baseprivileges { get; set; }
        public List<CommonEntity> _baseAgyTabsEntity { get; set; }
        public List<CaseMstEntity> _baseCaseMstListEntity { get; set; }
        public List<CaseSnpEntity> _baseCaseSnpEntity { get; set; }
        public AgencyControlEntity _baseAgencyControlDetails { get; set; }
        public UserEntity _baseUserProfile { get; set; }

        public string _baseApplicationNo = "";
        public string _baseApplicationName = "";
        public string _baseUSERID = "";
        public string _baseAgency = "";
        public string _baseProg = "";
        public string _baseDept = "";
        public string _baseYear = "";
        public string _baseAgyShortName = "";
        public string _docCode = "";
        public string _isSignReqired = "";
        public string _baseAgencyName = "";
        public string propReportPath { get; set; }
        private CaptainModel _model = null;
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string _PdfName = "Pdf File";
        public List<CommonEntity> IncomeInterValList { get; set; }
        bool _isSendMail = false;
        string _fromApp = "";
        string _SignImagePath = "";
        string _docoutputname = "";
        int docSeqno = 0;
        #endregion

        //PrivilegeEntity baseprivileges, List<CommonEntity> baseAgyTabsEntity, List<CaseMstEntity> baseCaseMstListEntity, List<CaseSnpEntity> baseCaseSnpEntity,
        //    AgencyControlEntity baseAgencyControlDetails, UserEntity baseUserProfile, , 
        public SignPDFGeneration(string baseUSERID, string baseAgency, string baseDept, string baseProg, string baseYear, string baseApplicationNo, string baseApplicationName,
            string baseAgyShortName, bool isSendMail, string SignImagePath, string fromApp, string docCode, string isSignReqired, string DocName, string AgencyName)
        {


            //_baseForm = baseForm;
            _baseApplicationNo = baseApplicationNo;
            _baseApplicationName = baseApplicationName;
            _baseUSERID = baseUSERID;
            _baseAgency = baseAgency;
            _baseDept = baseDept;
            _baseProg = baseProg;
            _baseYear = baseYear;
            _baseAgyShortName = baseAgyShortName;
            _baseAgencyName = AgencyName;

            _docCode = docCode;
            _isSignReqired = isSignReqired;
            _model = new CaptainModel();
            _baseprivileges = _model.UserProfileAccess.GetApplicationsByUserID(_baseUSERID, string.Empty)[0]; //baseprivileges;

            //CaseMstEntity caseMstEntity = _model.CaseMstData.GetCaseMST(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo);
            _baseCaseMstListEntity = new List<CaseMstEntity>();
            _baseCaseMstListEntity.Add(_model.CaseMstData.GetCaseMST(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo));

            _baseCaseSnpEntity = new List<CaseSnpEntity>();
            _baseCaseSnpEntity = _model.CaseMstData.GetCaseSnpadpyn(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo);

            _baseAgyTabsEntity = new List<CommonEntity>();
            _baseAgyTabsEntity = _model.lookupDataAccess.GetAgyTabs(string.Empty, string.Empty, string.Empty); //baseAgyTabsEntity;

            _baseAgencyControlDetails = new AgencyControlEntity();
            _baseAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");

            _baseUserProfile = new UserEntity();
            _baseUserProfile = _model.UserProfileAccess.GetUserProfileByID(_baseUSERID);

            _isSendMail = isSendMail;
            _fromApp = fromApp;
            _SignImagePath = SignImagePath;

            // _SignImagePath = @"C:\\SYSTEM-1\\E-DRIVE\\PCS-REPS\\SignForms\\Signatures\\01010100003016.jpg";

            if (fromApp == "PL")
            {
                if (_docCode != "")
                {
                    DataSet ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, string.Empty, "L");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] dr = ds.Tables[0].Select("PAC_FORM='" + _docCode + "' AND PAC_AGY='" + _baseAgency + "'");
                            if (dr.Length > 0)
                            {
                                docSeqno = AlienTXDB.getMaxDocNo(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, _docCode, "DOCSEQNO");
                                string dcname = dr[0]["PAC_DISP_NAME"].ToString().Replace("&", "").Replace(" ", "_").Replace("__", "_");

                                if (docSeqno > 1)
                                    _docoutputname = dcname + "_" + docSeqno.ToString();
                                else
                                    _docoutputname = dcname;
                            }
                        }
                    }
                }
            }
            else
            {
                _docoutputname = DocName;
            }


            propReportPath = _model.lookupDataAccess.GetReportPath();

            //if (_isSendMail)
            _isDocSigned = AlienTXDB.CheckIsDocSigned(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, _docCode, "CHKISDOCSIGN");
            //else
            //    _isDocSigned = true;
        }


        public bool _isDocSigned = false;

        DataTable dtCaseSNP = new DataTable();
        public void PrintAlienFormPDF()
        {
            //if (_isDocSigned)
            //{
            PrintAlienFormPDF1(true);
            //}
            //else
            //{
            //    MessageBox.Show("Document is already signed. Do you want to resend the document?","CAPTAIN", MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: PdfPrint);
            //}
        }
        public void PdfPrint(DialogResult dialogResult)
        {
            bool _issendEmail = false;
            if (dialogResult == DialogResult.Yes)
            {
                _issendEmail = true;
            }
            PrintAlienFormPDF1(_issendEmail);
        }
        public async Task PrintAlienFormPDF1(bool _emailSend)
        {
            try
            {
                IncomeInterValList = CommonFunctions.AgyTabsFilterCodeStatus(_baseAgyTabsEntity, "S0015", string.Empty, string.Empty, string.Empty, string.Empty);
                Random_Filename = null;


                _PdfName = _baseApplicationNo.ToString() + "Report";//form.GetFileName();
                _PdfName = propReportPath + _baseUSERID + "\\" + _PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + _baseUSERID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseUSERID.Trim()); }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay("Error");
                }

                try
                {
                    string Tmpstr = _PdfName + ".pdf";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = _PdfName + newFileName.Substring(0, length) + ".pdf";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    _PdfName = Random_Filename;
                else
                    _PdfName += ".pdf";


                FileStream fs = new FileStream(_PdfName, FileMode.Create);

                Document document = new Document();
                document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Arial.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bf_timesBold = BaseFont.CreateFont("c:/windows/fonts/ArialBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //BaseFont bf_times_Check = BaseFont.CreateFont("c:/windows/fonts/WINGDNG2.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times_Check = new iTextSharp.text.Font(bf_times_Check, 10);

                iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 8, 4, BaseColor.BLUE);
                BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 7);
                iTextSharp.text.Font TableFont10 = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont12 = new iTextSharp.text.Font(bf_times, 12);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 7, 3);

                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_timesBold, 7);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 7, 1);
                iTextSharp.text.Font TableFontBold12 = new iTextSharp.text.Font(bf_times, 12, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 7, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                //Agency Control Table
                string Attention = string.Empty, Roma_Switch = string.Empty;
                DataSet ds = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Attention = ds.Tables[0].Rows[0]["ACR_03_ATTESTATION"].ToString().Trim();
                    Roma_Switch = ds.Tables[0].Rows[0]["ACR_ROMA_SWITCH"].ToString().Trim();
                }

                //Mst Details Table
                DataSet dsCaseMST = DatabaseLayer.CaseSnpData.GetCaseMST(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo);
                DataRow drCaseMST = dsCaseMST.Tables[0].Rows[0];

                //Snp details Table
                DataSet dsCaseSNP = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, null);
                if (dsCaseSNP.Tables.Count > 0)
                {
                    dtCaseSNP = dsCaseSNP.Tables[0];
                    DataView dvSNP = new DataView(dtCaseSNP);
                    dvSNP.RowFilter = "SNP_STATUS<>'I'";
                    dtCaseSNP = dvSNP.ToTable();
                }

                //Casesite Table
                List<CaseSiteEntity> SiteList = new List<CaseSiteEntity>();
                CaseSiteEntity Search_Site = new CaseSiteEntity(true);
                Search_Site.SiteAGENCY = _baseAgency; Search_Site.SiteNUMBER = _baseCaseMstListEntity[0].Site;
                Search_Site.SiteROOM = "0000";
                SiteList = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");

                //County Name
                List<CommonEntity> COUNTY = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, "00525", _baseAgency, _baseDept, _baseProg, string.Empty);

                //CaseHie Table
                //DataSet dsCaseHie = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie("**-**-**",_baseAgency,_baseDept,_baseProg);
                DataSet dsCaseHie = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie("**-**-**", _baseUSERID, _baseAgency, string.Empty);
                DataTable dtCaseHie = dsCaseHie.Tables[0];

                //Getting CaseWorker
                DataSet dsVerifier = DatabaseLayer.CaseMst.GetCaseWorker("I", _baseAgency, _baseDept, _baseProg);
                DataTable dtVerifier = dsVerifier.Tables[0];

                //CaseIncome Table
                DataSet dsCaseIncome = DatabaseLayer.CaseMst.GetCASEINCOME(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo);
                DataTable dtCaseIncome = dsCaseIncome.Tables[0];
                DataSet dsIncome = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.INCOMETYPES);

                //CASEDIFF Table
                DataSet dsCaseDiff = DatabaseLayer.CaseMst.GetCASEDiffadpya(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo);
                DataTable dtCasediff = dsCaseDiff.Tables[0];

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = "S0060";  //List<AGYTABSEntity> TransportList = AgyTabs_List.FindAll(u => u.Tabs_Type.ToString().Trim().Equals("S0041"));
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                DataSet Relations = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.RELATIONSHIP);
                //DataTable dtrelation = Relations.Tables[0];
                List<CommonEntity> commonEntity = new List<CommonEntity>();
                if (Relations != null && Relations.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in Relations.Tables[0].Rows)
                        commonEntity.Add(new CommonEntity(dr["AGY_1"].ToString(), dr["Agy_8"].ToString(), dr["AGY_2"].ToString()));
                }

                CommonEntity MotherEntity = new CommonEntity(); List<CommonEntity> FatherEntity = new List<CommonEntity>();
                if (commonEntity.Count > 0)
                {
                    MotherEntity = commonEntity.Find(u => u.Hierarchy.Equals("G1"));
                    FatherEntity = commonEntity.FindAll(u => u.Hierarchy.Equals("G2"));
                }

                List<CommonEntity> lookInsuranceCategory = _model.lookupDataAccess.GetInsuranceCategory();

                DataSet dsFUND = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                DataTable dtFUND = dsFUND.Tables[0];

                DataTable dtdocs = DatabaseLayer.AlienTXDB.GET_DOCSIGNHIS(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, string.Empty, string.Empty, "N", "GET");
                if (dtdocs.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtdocs);
                    dv.RowFilter = "DCSN_MOVD_CAP='N'";
                    dtdocs = dv.ToTable();
                }


                cb.BeginText();
                X_Pos = 400; Y_Pos = 580;
                cb.SetFontAndSize(bf_helv, 13);
                //cb.SetColorFill(BaseColor.BLUE.Darker());
                string Header_Desc = string.Empty; string Form_Selection = string.Empty;

                //if (_privileges.ModuleCode == "03")
                //{
                string ShortName = string.Empty;
                string AgencyName = string.Empty; string SerHie = "N";

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ShortName = _baseAgyShortName; //ds.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
                    if (ds.Tables[0].Rows[0]["ACR_SERVINQ_CASEHIE"].ToString().Trim() == "1") SerHie = "Y"; else SerHie = "N";

                }

                if (_baseprivileges.ModuleCode != "05")
                {
                    if (dtCaseHie.Rows.Count > 0)
                    {
                        foreach (DataRow drCasehie in dtCaseHie.Rows)
                        {
                            if (drCasehie["Code"].ToString().Trim() == _baseAgency + _baseDept + _baseProg)
                            {
                                AgencyName = drCasehie["HIE_NAME"].ToString().Trim(); break;
                            }
                        }
                        cb.SetFontAndSize(bf_helv, 16);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ShortName + " - INTAKE APPLICATION", X_Pos, Y_Pos - 25, 0);
                        Header_Desc = ShortName + " - INTAKE APPLICATION";
                        Form_Selection = AgencyName;//"Casemanagement Application";

                        //cb.SetFontAndSize(bf_helv, 9);
                        //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant No: ", 30, Y_Pos - 40, 0);
                        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(_baseApplicationNo, Timesline), 30 + 72, Y_Pos-40, 0);

                        cb.SetFontAndSize(bf_helv, 13);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _baseProg + "  " + Form_Selection, X_Pos, Y_Pos - 40, 0);
                    }

                    cb.SetFontAndSize(bf_helv, 12);
                    //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date Printed: ", 740, Y_Pos - 40, 0);
                    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(DateTime.Now.ToShortDateString()), Times), 780, Y_Pos - 40, 0);
                    X_Pos = 30; Y_Pos -= 55;
                }


                cb.EndText();

                //Temp table not displayed on the screen
                PdfPTable head = new PdfPTable(1);
                head.HorizontalAlignment = Element.ALIGN_CENTER;
                head.TotalWidth = 50f;
                head.SpacingAfter = 40f;
                PdfPCell headcell = new PdfPCell(new Phrase(""));
                headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                head.AddCell(headcell);

                List<CaseSnpEntity> snpActivedetails = _baseCaseSnpEntity.FindAll(u => u.Status.Trim().ToUpper() == "A");

                #region To Print Main Details in the Table
                PdfPTable Main_Table1 = new PdfPTable(6);
                Main_Table1.TotalWidth = 740f;
                Main_Table1.WidthPercentage = 100;
                Main_Table1.LockedWidth = true;
                float[] widths = new float[] { 25f, 80f, 25f, 45f, 35f, 35f };
                Main_Table1.SetWidths(widths);
                Main_Table1.HorizontalAlignment = Element.ALIGN_CENTER;
                //Main_Table1.SpacingBefore = 40f;


                PdfPCell M1 = new PdfPCell(new Phrase("Applicant No:", TableFontBoldItalic));
                M1.HorizontalAlignment = Element.ALIGN_LEFT;
                M1.FixedHeight = 15f;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M1);

                PdfPCell M2 = new PdfPCell(new Phrase(_baseApplicationNo, TableFont));
                M2.HorizontalAlignment = Element.ALIGN_LEFT;
                M2.FixedHeight = 15f;
                M2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M2);

                PdfPCell M3 = new PdfPCell(new Phrase("Assigned Worker:", TableFontBoldItalic));
                M3.HorizontalAlignment = Element.ALIGN_LEFT;
                M3.FixedHeight = 15f;
                M3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M3);

                string CaseWorker = null;
                if (dtVerifier.Rows.Count > 0)
                {
                    foreach (DataRow drVerifier in dtVerifier.Rows)
                    {
                        if (_baseCaseMstListEntity[0].IntakeWorker.Trim() == drVerifier["PWH_CASEWORKER"].ToString().Trim())
                        {
                            CaseWorker = drVerifier["NAME"].ToString().Trim();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(CaseWorker.Trim()))
                    CaseWorker = "N/A";

                PdfPCell M4 = new PdfPCell(new Phrase(CaseWorker, TableFont));
                M4.HorizontalAlignment = Element.ALIGN_LEFT;
                M4.FixedHeight = 15f;
                M4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M4);

                PdfPCell M5 = new PdfPCell(new Phrase("", TableFont));
                M5.HorizontalAlignment = Element.ALIGN_LEFT;
                M5.FixedHeight = 15f;
                M5.Colspan = 2;
                M5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M5);

                PdfPCell M6 = new PdfPCell(new Phrase("Applicant Name:", TableFontBoldItalic));
                M6.HorizontalAlignment = Element.ALIGN_LEFT;
                M6.FixedHeight = 15f;
                M6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M6);


                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                PdfPCell M7 = new PdfPCell(new Phrase(ti.ToTitleCase(_baseApplicationName.ToLower()), TableFont));
                M7.HorizontalAlignment = Element.ALIGN_LEFT;
                M7.FixedHeight = 15f;
                M7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M7);

                PdfPCell M8 = new PdfPCell(new Phrase("Primary Language:", TableFontBoldItalic));
                M8.HorizontalAlignment = Element.ALIGN_LEFT;
                M8.FixedHeight = 15f;
                M8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M8);

                string Language = string.Empty;
                DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                DataTable dtLang = dsLang.Tables[0];
                foreach (DataRow drLang in dtLang.Rows)
                {
                    if (_baseCaseMstListEntity[0].Language.Trim() == drLang["Code"].ToString().Trim())
                    {
                        Language = drLang["LookUpDesc"].ToString().Trim(); break;
                    }
                }

                if (string.IsNullOrEmpty(Language.Trim()))
                    Language = "N/A";

                PdfPCell M9 = new PdfPCell(new Phrase(ti.ToTitleCase(Language.ToLower()), TableFont));
                M9.HorizontalAlignment = Element.ALIGN_LEFT;
                M9.FixedHeight = 15f;
                M9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M9);

                PdfPCell M10 = new PdfPCell(new Phrase("", TableFont));
                M10.HorizontalAlignment = Element.ALIGN_LEFT;
                M10.FixedHeight = 15f;
                M10.Colspan = 2;
                M10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M10);


                PdfPCell M11 = new PdfPCell(new Phrase("Address:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                string Apt = string.Empty; string Floor = string.Empty; string HN = string.Empty; string Suffix = string.Empty; string Street = string.Empty;
                string Zip = string.Empty; string strDirection = string.Empty;
                if (!string.IsNullOrEmpty(drCaseMST["MST_APT"].ToString().Trim()))
                    Apt = ", Apt  " + drCaseMST["MST_APT"].ToString().Trim() + "";
                if (!string.IsNullOrEmpty(drCaseMST["MST_Flr"].ToString().Trim()))
                    Floor = ", Flr  " + drCaseMST["MST_Flr"].ToString().Trim() + "";
                if (!string.IsNullOrEmpty(drCaseMST["MST_STREET"].ToString().Trim()))
                    Street = drCaseMST["MST_STREET"].ToString().Trim() + ", ";
                if (!string.IsNullOrEmpty(drCaseMST["MST_SUFFIX"].ToString().Trim()))
                    Suffix = drCaseMST["MST_SUFFIX"].ToString().Trim();
                if (!string.IsNullOrEmpty(drCaseMST["MST_HN"].ToString().Trim()))
                    HN = drCaseMST["MST_HN"].ToString().Trim() + " ";




                if (!string.IsNullOrEmpty(drCaseMST["MST_ZIP"].ToString().Trim()) && drCaseMST["MST_ZIP"].ToString() != "0")
                    Zip = "00000".Substring(0, 5 - drCaseMST["MST_ZIP"].ToString().Trim().Length) + drCaseMST["MST_ZIP"].ToString().Trim();

                if (!string.IsNullOrEmpty(drCaseMST["MST_DIRECTION"].ToString().Trim()))
                    strDirection = drCaseMST["MST_DIRECTION"].ToString().Trim() + " ";

                string Comma = string.Empty;
                //if (!string.IsNullOrEmpty(drCaseMST["MST_SUFFIX"].ToString().Trim()) && (!string.IsNullOrEmpty(drCaseMST["MST_APT"].ToString().Trim()) || !string.IsNullOrEmpty(drCaseMST["MST_Flr"].ToString().Trim())))
                //    Comma = ", ";

                string Address = ti.ToTitleCase(HN.ToLower()) + ti.ToTitleCase(strDirection.ToLower()) + ti.ToTitleCase(Street.ToLower()) + ti.ToTitleCase(Suffix.ToLower()) + ti.ToTitleCase(Apt.ToLower()) + ti.ToTitleCase(Floor.ToLower());

                if (!string.IsNullOrEmpty(Address.Trim()))
                {
                    if (Address.Trim().EndsWith(","))
                    {
                        Address = Address.Trim().Remove(Address.Trim().Length - 1);
                    }
                }

                if (drCaseMST["MST_CITY"].ToString().Trim() != string.Empty)
                    Address = Address + ", " + ti.ToTitleCase(drCaseMST["MST_CITY"].ToString().Trim().ToLower());
                if (drCaseMST["MST_STATE"].ToString().Trim() != string.Empty)
                    Address = Address + ", " + drCaseMST["MST_STATE"].ToString().Trim();


                PdfPCell M12 = new PdfPCell(new Phrase(Address + " " + Zip, TableFont));
                M12.HorizontalAlignment = Element.ALIGN_LEFT;
                M12.FixedHeight = 15f;
                M12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M12);

                PdfPCell M13 = new PdfPCell(new Phrase("County:", TableFontBoldItalic));
                M13.HorizontalAlignment = Element.ALIGN_LEFT;
                M13.FixedHeight = 15f;
                M13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M13);

                if (!string.IsNullOrEmpty(drCaseMST["MST_COUNTY"].ToString().Trim()))
                {
                    string countyName = COUNTY.Find(x => x.Code == drCaseMST["MST_COUNTY"].ToString().Trim()).Desc;

                    PdfPCell M14 = new PdfPCell(new Phrase(countyName, TableFont));
                    M14.HorizontalAlignment = Element.ALIGN_LEFT;
                    M14.FixedHeight = 15f;
                    M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M14);
                }
                else
                {
                    PdfPCell M14 = new PdfPCell(new Phrase("N/A", TableFont));
                    M14.HorizontalAlignment = Element.ALIGN_LEFT;
                    M14.FixedHeight = 15f;
                    M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M14);
                }

                PdfPCell M15 = new PdfPCell(new Phrase("                  Site:", TableFontBoldItalic));
                M15.HorizontalAlignment = Element.ALIGN_LEFT;
                M15.FixedHeight = 15f;
                M15.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M15);

                string SiteName = string.Empty;
                if (SiteList.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in SiteList)
                    {
                        if (_baseCaseMstListEntity[0].Site.Trim() == Entity.SiteNUMBER.Trim())
                        {
                            SiteName = _baseCaseMstListEntity[0].Site.Trim() + " - " + Entity.SiteNAME.Trim(); break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(SiteName.Trim()))
                    SiteName = "N/A";

                PdfPCell M16 = new PdfPCell(new Phrase(SiteName, TableFont));
                M16.HorizontalAlignment = Element.ALIGN_LEFT;
                M16.FixedHeight = 15f;
                M16.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M16);

                //Added by sudheer on 05/04/2022 as per WESTCOP document.
                PdfPCell M111 = new PdfPCell(new Phrase("Mailing Address:", TableFontBoldItalic));
                M111.HorizontalAlignment = Element.ALIGN_LEFT;
                M111.FixedHeight = 15f;
                M111.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M111);

                string House_NO = string.Empty, Street1 = string.Empty, city = string.Empty, state = string.Empty, zip = string.Empty, DApt = string.Empty; string DSuffix = string.Empty;
                if (dtCasediff.Rows.Count > 0)
                {
                    foreach (DataRow drCaseDiff in dtCasediff.Rows)
                    {
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_HN"].ToString().Trim()))
                            House_NO = drCaseDiff["DIFF_HN"].ToString().Trim() + " ";
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_APT"].ToString().Trim()))
                            DApt = drCaseDiff["DIFF_APT"].ToString().Trim() + " ";
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_SUFFIX"].ToString().Trim()))
                            DSuffix = " " + drCaseDiff["DIFF_SUFFIX"].ToString().Trim();
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STREET"].ToString().Trim()))
                            Street1 = drCaseDiff["DIFF_STREET"].ToString().Trim() + DSuffix + ",";
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_CITY"].ToString().Trim()))
                            city = drCaseDiff["DIFF_CITY"].ToString().Trim() + ",";
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STATE"].ToString().Trim()))
                            state = drCaseDiff["DIFF_STATE"].ToString().Trim();
                        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_ZIP"].ToString().Trim()))
                            zip = "00000".Substring(0, 5 - drCaseDiff["DIFF_ZIP"].ToString().Trim().Length) + drCaseDiff["DIFF_ZIP"].ToString().Trim();
                        if (zip == "00000") zip = ""; else zip = ", " + zip;
                    }

                    Comma = string.Empty;
                    if (!string.IsNullOrEmpty(DSuffix.Trim()) && (!string.IsNullOrEmpty(DApt.Trim())))
                        Comma = ", ";

                    PdfPCell M121 = new PdfPCell(new Phrase(ti.ToTitleCase(House_NO.ToLower()) + ti.ToTitleCase(Street1.ToLower()) + ti.ToTitleCase(DApt.ToLower()) + ti.ToTitleCase(city.ToLower()) + state.ToLower() + zip, TableFont));
                    M121.HorizontalAlignment = Element.ALIGN_LEFT;
                    M121.FixedHeight = 15f;
                    //M121.Colspan = 5;
                    M121.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M121);

                    //Added by Vikash on 01/31/2025 as per CACOST document

                    M13 = new PdfPCell(new Phrase("Email:", TableFontBoldItalic));
                    M13.HorizontalAlignment = Element.ALIGN_LEFT;
                    M13.FixedHeight = 15f;
                    M13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M13);

                    if (!string.IsNullOrEmpty(_baseCaseMstListEntity[0].Email.Trim()))
                    {
                        PdfPCell M14 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].Email.Trim(), TimesUnderline));
                        M14.HorizontalAlignment = Element.ALIGN_LEFT;
                        M14.FixedHeight = 15f;
                        M14.Colspan = 4;
                        M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Main_Table1.AddCell(M14);
                    }
                    else
                    {
                        PdfPCell M14 = new PdfPCell(new Phrase("N/A", TimesUnderline));
                        M14.HorizontalAlignment = Element.ALIGN_LEFT;
                        M14.FixedHeight = 15f;
                        M14.Colspan = 4;
                        M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Main_Table1.AddCell(M14);
                    }

                    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(House_NO + Street1 + DApt + city + state + zip, Timesline), X_Pos + 72, Y_Pos, 0);
                }
                else
                {
                    PdfPCell M121 = new PdfPCell(new Phrase("", TableFont));
                    M121.HorizontalAlignment = Element.ALIGN_LEFT;
                    M121.FixedHeight = 15f;
                    //M121.Colspan = 5;
                    M121.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M121);

                    //Added by Vikash on 01/31/2025 as per CACOST document

                    M13 = new PdfPCell(new Phrase("Email:", TableFontBoldItalic));
                    M13.HorizontalAlignment = Element.ALIGN_LEFT;
                    M13.FixedHeight = 15f;
                    M13.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Main_Table1.AddCell(M13);


                    if (!string.IsNullOrEmpty(_baseCaseMstListEntity[0].Email.Trim()))
                    {
                        PdfPCell M14 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].Email.Trim(), TimesUnderline));
                        M14.HorizontalAlignment = Element.ALIGN_LEFT;
                        M14.FixedHeight = 15f;
                        M14.Colspan = 4;
                        M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Main_Table1.AddCell(M14);
                    }
                    else
                    {
                        PdfPCell M14 = new PdfPCell(new Phrase("N/A", TimesUnderline));
                        M14.HorizontalAlignment = Element.ALIGN_LEFT;
                        M14.FixedHeight = 15f;
                        M14.Colspan = 4;
                        M14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Main_Table1.AddCell(M14);
                    }
                }



                Phrase pharsecell = new Phrase();
                pharsecell.Add(new Chunk("Home Phone:                  ", TableFontBoldItalic));
                if (!string.IsNullOrEmpty(drCaseMST["MST_PHONE"].ToString().Trim()))
                    pharsecell.Add(new Chunk(LookupDataAccess.GetPhoneFormat(drCaseMST["MST_AREA"].ToString().Trim() + drCaseMST["MST_PHONE"].ToString().Trim()), TableFont));
                else
                    pharsecell.Add(new Chunk("N/A", TableFont));
                pharsecell.Add(new Chunk("                        Cell:", TableFontBoldItalic));
                if (!string.IsNullOrEmpty(_baseCaseMstListEntity[0].CellPhone.Trim()))
                    pharsecell.Add(new Chunk(LookupDataAccess.GetPhoneFormat(_baseCaseMstListEntity[0].CellPhone), TableFont));
                else
                    pharsecell.Add(new Chunk("N/A", TableFont));

                M11 = new PdfPCell(pharsecell);
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Colspan = 2;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Date Printed:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                M11 = new PdfPCell(new Phrase(DateTime.Now.ToString("g"), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                M11 = new PdfPCell(new Phrase("                  Application Date:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                M11 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(_baseCaseMstListEntity[0].IntakeDate), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table1.AddCell(M11);

                int Adults = 0, Child = 0, under5 = 0;
                foreach (DataRow drsnp in dtCaseSNP.Rows)
                {
                    if (!string.IsNullOrEmpty(drsnp["SNP_AGE"].ToString()))
                    {
                        if (int.Parse(drsnp["SNP_AGE"].ToString()) >= 18)
                            Adults++;
                        else
                            Child++;
                        if (int.Parse(drsnp["SNP_AGE"].ToString()) < 5)
                            under5++;
                    }
                }

                document.Add(head);
                document.Add(Main_Table1);



                PdfPTable Main_Table = new PdfPTable(6);
                Main_Table.TotalWidth = 740f;
                Main_Table.WidthPercentage = 100;
                Main_Table.LockedWidth = true;
                widths = new float[] { 35f, 70f, 25f, 45f, 35f, 35f };
                Main_Table.SetWidths(widths);


                pharsecell = new Phrase();
                pharsecell.Add(new Chunk("Total No of Household Members:   ", TableFontBoldItalic));
                pharsecell.Add(new Chunk(_baseCaseMstListEntity[0].NoInhh, TableFont));
                pharsecell.Add(new Chunk("       Adults:     ", TableFontBoldItalic));
                pharsecell.Add(new Chunk(Adults.ToString(), TableFont));
                pharsecell.Add(new Chunk("       Children:   ", TableFontBoldItalic));
                pharsecell.Add(new Chunk(Child.ToString(), TableFont));

                M11 = new PdfPCell(pharsecell);
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Colspan = 2;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Family Type:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                //DataSet dsFamilyType = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.FAMILYTYPE);
                //DataTable dtFamilyType = dsFamilyType.Tables[0];
                //foreach (DataRow drFamilyType in dtFamilyType.Rows)
                //{
                //    if( !string.IsNullOrEmpty(drCaseMST["MST_FAMILY_TYPE"].ToString().Trim()) && drCaseMST["MST_FAMILY_TYPE"].ToString().Trim() == drFamilyType["Code"]))
                //    { }
                //}


                DataSet dsFamilytyp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FAMILYTYPE);
                DataTable dtfamilytyp = dsFamilytyp.Tables[0];
                string strFamilytypedesc = string.Empty;
                foreach (DataRow drLang in dtfamilytyp.Rows)
                {
                    if (drCaseMST["MST_FAMILY_TYPE"].ToString().Trim() == drLang["Code"].ToString().Trim())
                    {
                        strFamilytypedesc = drLang["LookUpDesc"].ToString().Trim(); break;
                    }
                }

                if (string.IsNullOrEmpty(strFamilytypedesc.Trim()))
                    strFamilytypedesc = "N/A";

                M11 = new PdfPCell(new Phrase(strFamilytypedesc, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Colspan = 2;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Household Non-Cash Benefits:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);


                List<CommonEntity> Noncashbenefit = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                string strNoncash = string.Empty;
                if (Noncashbenefit.Count > 0)
                {
                    string response = _baseCaseMstListEntity[0].MstNCashBen;
                    string[] arrResponse = null;
                    if (response.IndexOf(',') > 0)
                    {
                        arrResponse = response.Split(',');
                    }
                    else if (!response.Equals(string.Empty))
                    {
                        arrResponse = new string[] { response };
                    }
                    bool boolexist = false;
                    foreach (CommonEntity dr in Noncashbenefit)
                    {
                        boolexist = false;
                        if (dr.Code.Trim().ToString().ToUpper() != "N")
                        {
                            string resDesc = dr.Code.ToString().Trim();


                            if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                            {
                                boolexist = true;
                                if (strNoncash == string.Empty)
                                {
                                    strNoncash = dr.Desc;
                                }
                                else
                                {
                                    strNoncash = strNoncash + ", " + dr.Desc;
                                }
                            }
                        }

                        //gvwAlertCode.Rows[index].Tag = agyEntity;
                    }
                }

                if (string.IsNullOrEmpty(strNoncash.Trim()))
                    strNoncash = "N/A";

                M11 = new PdfPCell(new Phrase(strNoncash, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Colspan = 5;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Household Disconnected Youth:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                string strDisconnectdesc = string.Empty;

                foreach (CaseSnpEntity snpdisconect in snpActivedetails)
                {
                    if (snpdisconect.Age != string.Empty)
                    {
                        if ((Convert.ToInt32(snpdisconect.Age) >= 14 && Convert.ToInt32(snpdisconect.Age) <= 24) && snpdisconect.Youth.ToString().ToUpper().Trim() == "N")
                        {
                            if (strDisconnectdesc != string.Empty)
                            {
                                strDisconnectdesc = strDisconnectdesc + ", " + LookupDataAccess.GetMemberName(ti.ToTitleCase(snpdisconect.NameixFi.ToLower()), snpdisconect.NameixMi, ti.ToTitleCase(snpdisconect.NameixLast.ToLower()), "4");
                            }
                            else
                            {
                                strDisconnectdesc = LookupDataAccess.GetMemberName(ti.ToTitleCase(snpdisconect.NameixFi.ToLower()), snpdisconect.NameixMi, ti.ToTitleCase(snpdisconect.NameixLast.ToLower()), "4");
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(strDisconnectdesc.Trim()))
                    strDisconnectdesc = "N/A";

                M11 = new PdfPCell(new Phrase(strDisconnectdesc, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Housing Situation:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                List<CommonEntity> Housingsit = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.HOUSINGTYPES, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                string strhousing = string.Empty;

                if (Housingsit.Count > 0)
                {
                    CommonEntity descentity = Housingsit.Find(u => u.Code.Trim() == _baseCaseMstListEntity[0].Housing.Trim());
                    if (descentity != null)
                        strhousing = descentity.Desc;

                }

                if (string.IsNullOrEmpty(strhousing.Trim()))
                    strhousing = "N/A";

                M11 = new PdfPCell(new Phrase(strhousing, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                //M11.Colspan = 3;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("                  Dwelling Type:", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);

                List<CommonEntity> DwellingType = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.DWELLINGTYPE, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                string strDwellingType = string.Empty;

                if (DwellingType.Count > 0)
                {
                    CommonEntity descentity = DwellingType.Find(u => u.Code.Trim() == _baseCaseMstListEntity[0].Dwelling.Trim());
                    if (descentity != null)
                        strDwellingType = descentity.Desc;

                }

                if (string.IsNullOrEmpty(strDwellingType.Trim())) strDwellingType = "N/A";

                M11 = new PdfPCell(new Phrase(strDwellingType, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_LEFT;
                //M11.Colspan = 3;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Main_Table.AddCell(M11);



                document.Add(Main_Table);
                #endregion
                #region Snpmemberdata
                PdfPTable Member_Table = new PdfPTable(13);
                Member_Table.TotalWidth = 740f;
                Member_Table.WidthPercentage = 100;
                Member_Table.LockedWidth = true;
                float[] widthsMember = new float[] { 90f, 60f, 45f, 45f, 25f, 25f, 60f, 60f, 65f, 40f, 70f, 40f, 105f };
                Member_Table.SetWidths(widthsMember);
                Member_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                Member_Table.SpacingBefore = 10f;

                M11 = new PdfPCell(new Phrase("Name Last, First, Mi", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Relationship To Applicant", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Social Security", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase("Birth Date", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Age", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Sex", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Ethnicity", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Race", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Education", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Health Insurance", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Military", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Disabled", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase("Work Status", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 22f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Member_Table.AddCell(M11);

                //DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                //DataTable dtLang = dsLang.Tables[0];
                //foreach (DataRow drLang in dtLang.Rows)
                //{
                //    if (drCaseMST["MST_LANGUAGE"].ToString().Trim() == drLang["Code"].ToString().Trim())
                //    {
                //        Language = drLang["LookUpDesc"].ToString().Trim(); break;
                //    }
                //}

                List<CommonEntity> Relationent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.RELATIONSHIP, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> Ethinicent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.ETHNICODES, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> raceent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.RACE, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> Educationent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.EDUCATIONCODES, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> Healthientity = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.HEALTHINSURANCE, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> Militaryent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.MilitaryStatus, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> Disibledent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.DISABLED, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();
                List<CommonEntity> workent = CommonFunctions.AgyTabsFilterCode(_baseAgyTabsEntity, Consts.AgyTab.WorkStatus, _baseAgency, _baseDept, _baseProg, string.Empty); //_model.lookupDataAccess.GetHealthInsurance();



                foreach (CaseSnpEntity SNPitem in snpActivedetails)
                {
                    M11 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(ti.ToTitleCase(SNPitem.NameixFi.ToLower()), SNPitem.NameixMi, ti.ToTitleCase(SNPitem.NameixLast.ToLower()), "6"), TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);


                    string strRelation = string.Empty;

                    if (Relationent.Count > 0)
                    {
                        CommonEntity descentity = Relationent.Find(u => u.Code.Trim() == SNPitem.MemberCode.Trim());
                        if (descentity != null)
                            strRelation = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strRelation, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    M11 = new PdfPCell(new Phrase(LookupDataAccess.GetCardNo(SNPitem.Ssno, "1", "N", string.Empty), TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);


                    M11 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SNPitem.AltBdate), TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    M11 = new PdfPCell(new Phrase(SNPitem.Age, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    M11 = new PdfPCell(new Phrase(SNPitem.Sex, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strEthinic = string.Empty;

                    if (Ethinicent.Count > 0)
                    {
                        CommonEntity descentity = Ethinicent.Find(u => u.Code.Trim() == SNPitem.Ethnic.Trim());
                        if (descentity != null)
                            strEthinic = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strEthinic, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strrace = string.Empty;

                    if (raceent.Count > 0)
                    {
                        CommonEntity descentity = raceent.Find(u => u.Code.Trim() == SNPitem.Race.Trim());
                        if (descentity != null)
                            strrace = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strrace, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strEducation = string.Empty;

                    if (Educationent.Count > 0)
                    {
                        CommonEntity descentity = Educationent.Find(u => u.Code.Trim() == SNPitem.Education.Trim());
                        if (descentity != null)
                            strEducation = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strEducation, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strHealth = string.Empty;

                    if (Healthientity.Count > 0)
                    {
                        CommonEntity descentity = Healthientity.Find(u => u.Code.Trim() == SNPitem.HealthIns.Trim());
                        if (descentity != null)
                            strHealth = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strHealth, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strMilitary = string.Empty;

                    if (Militaryent.Count > 0)
                    {
                        CommonEntity descentity = Militaryent.Find(u => u.Code.Trim() == SNPitem.MilitaryStatus.Trim());
                        if (descentity != null)
                            strMilitary = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strMilitary, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                    string strDisible = string.Empty;

                    if (Disibledent.Count > 0)
                    {
                        CommonEntity descentity = Disibledent.Find(u => u.Code.Trim() == SNPitem.Disable.Trim());
                        if (descentity != null)
                            strDisible = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strDisible, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_CENTER;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);


                    string strWork = string.Empty;

                    if (workent.Count > 0)
                    {
                        CommonEntity descentity = workent.Find(u => u.Code.Trim() == SNPitem.WorkStatus.Trim());
                        if (descentity != null)
                            strWork = descentity.Desc;
                    }

                    M11 = new PdfPCell(new Phrase(strWork, TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //M11.FixedHeight = 13F;
                    M11.Border = iTextSharp.text.Rectangle.BOX;
                    Member_Table.AddCell(M11);

                }


                document.Add(Member_Table);

                #endregion
                #region Incometabledata

                List<CommonEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypesDeduction();

                PdfPTable Income_Table = new PdfPTable(10);
                Income_Table.TotalWidth = 740f;
                Income_Table.WidthPercentage = 100;
                Income_Table.LockedWidth = true;
                float[] widthsIncome = new float[] { 90f, 100f, 45f, 40f, 40f, 40f, 40f, 40f, 110f, 100f };
                Income_Table.SetWidths(widthsIncome);
                Income_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                Income_Table.SpacingBefore = 3f;

                M11 = new PdfPCell(new Phrase("Financial Information", TableFontBold12));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 17F;
                M11.Colspan = 10;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Household Member", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Income Type", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Interval", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase("Amount", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Amount", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Amount", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Amount", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Amount", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("How Verified?", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                Income_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Table.AddCell(M11);


                List<CaseIncomeEntity> incomedata = _model.CaseMstData.GetCaseIncomeadpynf(_baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, string.Empty);
                incomedata = incomedata.OrderBy(u => u.FamilySeq).ThenByDescending(u => u.Type).ToList();
                string strTypedesc = string.Empty;
                string strNamedefault = string.Empty;
                double doublettolagrant = 0;
                double doubleval1, doubleval2, doubleval3, doubleval4, doubleval5;
                foreach (CaseIncomeEntity incomeitem in incomedata)
                {
                    if (incomeitem.Exclude.ToUpper().Trim() != "Y")
                    {
                        CaseSnpEntity snpincome = _baseCaseSnpEntity.Find(u => u.FamilySeq == incomeitem.FamilySeq && u.Status.ToUpper().Trim() == "A");

                        if (snpincome != null)
                        {
                            strTypedesc = string.Empty;
                            CommonEntity commontypedesc = lookUpIncomeTypes.Find(u => u.Code.Trim() == incomeitem.Type.Trim());
                            if (commontypedesc != null)
                            {
                                strTypedesc = commontypedesc.Desc;
                            }
                            if (strNamedefault != LookupDataAccess.GetMemberName(snpincome.NameixFi, snpincome.NameixMi, snpincome.NameixLast, "6"))
                            {
                                strNamedefault = LookupDataAccess.GetMemberName(snpincome.NameixFi, snpincome.NameixMi, snpincome.NameixLast, "6");
                                M11 = new PdfPCell(new Phrase(ti.ToTitleCase(strNamedefault.ToLower()), TableFont));
                            }
                            else
                            {
                                M11 = new PdfPCell(new Phrase("", TableFont));
                                strNamedefault = LookupDataAccess.GetMemberName(snpincome.NameixFi, snpincome.NameixMi, snpincome.NameixLast, "6");
                            }

                            M11.HorizontalAlignment = Element.ALIGN_LEFT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            M11 = new PdfPCell(new Phrase(strTypedesc, TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_LEFT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            //M11 = new PdfPCell(new Phrase(LookupDataAccess.ShowIncomeInterval(incomeitem.Interval), TableFont));
                            M11 = new PdfPCell(new Phrase(GetIncomeIntervalDesc(incomeitem.Interval.Trim()), TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_CENTER;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            doubleval1 = Convert.ToDouble(incomeitem.Val1 == string.Empty ? "0" : incomeitem.Val1);
                            doubleval2 = Convert.ToDouble(incomeitem.Val2 == string.Empty ? "0" : incomeitem.Val2);
                            doubleval3 = Convert.ToDouble(incomeitem.Val3 == string.Empty ? "0" : incomeitem.Val3);
                            doubleval4 = Convert.ToDouble(incomeitem.Val4 == string.Empty ? "0" : incomeitem.Val4);
                            doubleval5 = Convert.ToDouble(incomeitem.Val5 == string.Empty ? "0" : incomeitem.Val5);
                            doublettolagrant = doublettolagrant + doubleval1;
                            doublettolagrant = doublettolagrant + doubleval2;
                            doublettolagrant = doublettolagrant + doubleval3;
                            doublettolagrant = doublettolagrant + doubleval4;
                            doublettolagrant = doublettolagrant + doubleval5;
                            if (doubleval1 > 0)
                                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doubleval1), TableFont));
                            else
                                M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            if (doubleval2 > 0)
                                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doubleval2), TableFont));
                            else
                                M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            if (doubleval3 > 0)
                                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doubleval3), TableFont));
                            else
                                M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            if (doubleval4 > 0)
                                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doubleval4), TableFont));
                            else
                                M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            if (doubleval5 > 0)
                                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doubleval5), TableFont));
                            else
                                M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);


                            M11 = new PdfPCell(new Phrase(incomeitem.HowVerified, TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_LEFT;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12F;
                            M11.Border = iTextSharp.text.Rectangle.BOX;
                            Income_Table.AddCell(M11);

                            M11 = new PdfPCell(new Phrase("", TableFont));
                            M11.HorizontalAlignment = Element.ALIGN_CENTER;
                            M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                            M11.FixedHeight = 12f;
                            M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Income_Table.AddCell(M11);
                        }
                    }
                }



                document.Add(Income_Table);

                #endregion
                #region Mstdetailsdata


                PdfPTable MstDetails_Table = new PdfPTable(8);
                MstDetails_Table.TotalWidth = 740f;
                MstDetails_Table.WidthPercentage = 100;
                MstDetails_Table.LockedWidth = true;
                float[] widthsdetails = new float[] { 120f, 100f, 20f, 120f, 120f, 60f, 60f, 160f };
                MstDetails_Table.SetWidths(widthsdetails);
                MstDetails_Table.HorizontalAlignment = Element.ALIGN_CENTER;
                MstDetails_Table.SpacingBefore = 10f;

                M11 = new PdfPCell(new Phrase("Grand Total", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doublettolagrant), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Number in Program", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].NoInProg, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Fed OMB", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                string Poverty = _baseCaseMstListEntity[0].Poverty.Trim();
                if (_baseAgencyControlDetails.ACR_POV_WITH_DEC == "N" && !string.IsNullOrEmpty(_baseCaseMstListEntity[0].Poverty.Trim()))
                    Poverty = Math.Abs((int)Convert.ToDecimal(_baseCaseMstListEntity[0].Poverty.Trim())).ToString();

                M11 = new PdfPCell(new Phrase(Poverty + "%", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase("30 Days Income", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                double doub30daysincome = 0;
                double doubletotalincome = Convert.ToDouble(_baseCaseMstListEntity[0].FamIncome == string.Empty ? "0" : _baseCaseMstListEntity[0].FamIncome);
                if (doubletotalincome > 0)
                {
                    doub30daysincome = doubletotalincome / 12;
                }


                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doub30daysincome), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Last Income Verficiation Date", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);


                M11 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(_baseCaseMstListEntity[0].EligDate), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("CMI", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].Cmi == string.Empty ? "" : _baseCaseMstListEntity[0].Cmi + "%", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);



                M11 = new PdfPCell(new Phrase("Annualized", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                double doublefamincome = Convert.ToDouble(_baseCaseMstListEntity[0].FamIncome == string.Empty ? "0" : _baseCaseMstListEntity[0].FamIncome);

                M11 = new PdfPCell(new Phrase("$" + String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", doublefamincome), TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_RIGHT;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Verifier", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                string verifyName = string.Empty;
                if (dtVerifier.Rows.Count > 0)
                {
                    foreach (DataRow drVerifier in dtVerifier.Rows)
                    {
                        if (_baseCaseMstListEntity[0].Verifier.Trim() == drVerifier["PWH_CASEWORKER"].ToString().Trim())
                        {
                            verifyName = drVerifier["NAME"].ToString().Trim();
                            break;
                        }
                    }
                }


                M11 = new PdfPCell(new Phrase(verifyName, TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("SMI", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].Smi == string.Empty ? "" : _baseCaseMstListEntity[0].Smi + "%", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Colspan = 5;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("HUD", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase(_baseCaseMstListEntity[0].Hud == string.Empty ? "" : _baseCaseMstListEntity[0].Hud + "%", TableFont));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.BOX;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.FixedHeight = 12f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.Colspan = 8;
                M11.FixedHeight = 10f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                M11 = new PdfPCell(new Phrase("Declaration & Agreement", TableFontBold12));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                M11.Colspan = 8;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                if (_baseAgyShortName == "WESTCOP" || _baseAgyShortName == "PCS" || _baseAgyShortName == "FTW" || _baseAgyShortName == "NCCAA" || _baseAgyShortName == "CSNT")
                {
                    M11 = new PdfPCell(new Phrase("I, the undersigned applicant, do solemnly swear that the above information is true, correct and complete to the best of my Knowledge. I understand that any false statements or misrepresentation may result in me being found ineligible for assistance.I consent to any inquiries to verify or confirm the information provided on this application.Because your personal information is held in the strictest of confidence, we will only share information with a signed Release of Information.", TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }
                else
                {
                    M11 = new PdfPCell(new Phrase("I certify that all information is true and accurate to the best of my knowledge. I understand that providing false information or misrepresentation may result in my being found ineligible for services.  I understand that by signing this application I am entitled to all " + _baseAgyShortName + " services and therefore information on this application may be provided to other " + _baseAgyShortName + " programs.", TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.Colspan = 8;
                M11.FixedHeight = 6f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                if (_baseAgyShortName == "WESTCOP" || _baseAgyShortName == "PCS" || _baseAgyShortName == "FTW" || _baseAgyShortName == "NCCAA" || _baseAgyShortName == "CSNT")
                {
                    M11 = new PdfPCell(new Phrase("By signing this, I am swearing under penalty of perjury, that all of the information contained on this CSBG Customer Intake Form is true to the best of my knowledge and that I am willing to cooperate with any efforts to verify the information provided.", TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }
                else if (_baseAgyShortName == "ACCORD")
                {
                    M11 = new PdfPCell(new Phrase("Domestic Violence Program Only: I certify that all information is true and accurate to the best of my knowledge. I understand that no conditions can be placed on clients to receive services under the law. By signing this application, I am entitled to all " + _baseAgyShortName + " services.", TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }
                else
                {
                    M11 = new PdfPCell(new Phrase("", TableFont));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    M11.Colspan = 8;
                    M11.FixedHeight = 18f;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }

                M11 = new PdfPCell(new Phrase("", TableFontBoldItalic));
                M11.HorizontalAlignment = Element.ALIGN_CENTER;
                M11.Colspan = 8;
                M11.FixedHeight = 15f;
                M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MstDetails_Table.AddCell(M11);

                if (_fromApp == "PL")
                {

                    M11 = new PdfPCell(new Phrase("Signature of Applicant:_________________________________________ Date:___________________ " + ShortName + " Staff Signature:_________________________________________ Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "", TableFontBoldItalic));
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }
                else if (_fromApp == "SD")
                {


                    //M11 = new PdfPCell(new Phrase("Signature of Applicant:_________________________________________ Date:___________________ " + ShortName + " Staff Signature:_________________________________________ Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "", TableFontBoldItalic));
                    //M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    //M11.Colspan = 8;
                    //M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //MstDetails_Table.AddCell();


                    PdfPTable tblSignatire = new PdfPTable(8);
                    tblSignatire.TotalWidth = 740f;
                    tblSignatire.WidthPercentage = 100;
                    tblSignatire.LockedWidth = true;
                    float[] widthsSign = new float[] { 40f, 80f, 15f, 30f, 50f, 55f, 35f, 50f };   //80f, 25f, 30f, 35f, 35f
                    tblSignatire.SetWidths(widthsSign);
                    tblSignatire.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tblSignatire.SpacingBefore = 20f;

                    PdfPCell Sn1 = new PdfPCell(new Phrase("Signature of Applicant:", TableFontBoldItalic));
                    Sn1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Sn1.FixedHeight = 15f;
                    Sn1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn1);


                    iTextSharp.text.Image _imgSignature = iTextSharp.text.Image.GetInstance(_SignImagePath); //Application.MapPath("~\\Resources\\Images\\StateofTexas_logo.png")
                    _imgSignature.ScalePercent(8f);
                    //_imgSignature.SetAbsolutePosition(660f, 480f);
                    //_document.Add(_imgTexasLogo);

                    PdfPCell Sn2 = new PdfPCell(_imgSignature);
                    Sn2.VerticalAlignment = Element.ALIGN_TOP;
                    Sn2.HorizontalAlignment = Element.ALIGN_LEFT;
                    Sn2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn2);

                    PdfPCell Sn3 = new PdfPCell(new Phrase("Date:", TableFontBoldItalic));
                    Sn3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Sn3.FixedHeight = 15f;
                    Sn3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn3);

                    PdfPCell Sn4 = new PdfPCell(new Phrase(DateTime.Now.ToString("MM/dd/yyyy"), TableFontBoldItalic));
                    Sn4.HorizontalAlignment = Element.ALIGN_LEFT;
                    Sn4.FixedHeight = 15f;
                    Sn4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn4);

                    PdfPCell Sn5 = new PdfPCell(new Phrase(ShortName + " Staff Signature:", TableFontBoldItalic));
                    Sn5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Sn5.FixedHeight = 15f;
                    Sn5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn5);

                    PdfPCell Sn6 = new PdfPCell(new Phrase("__________________________", TableFontBoldItalic));
                    Sn6.HorizontalAlignment = Element.ALIGN_LEFT;
                    Sn6.FixedHeight = 15f;
                    Sn6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn6);

                    PdfPCell Sn7 = new PdfPCell(new Phrase("Date: ", TableFontBoldItalic));
                    Sn7.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Sn7.FixedHeight = 15f;
                    Sn7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn7);

                    string StaffDate = string.Empty;
                    if (dtdocs.Rows.Count > 0)
                        StaffDate = LookupDataAccess.Getdate(dtdocs.Rows[0]["DCSN_HIS_ADD_DATE"].ToString());

                    PdfPCell Sn8 = new PdfPCell(new Phrase(StaffDate, TableFontBoldItalic));//DateTime.Now.ToString("MM/dd/yyyy")
                    Sn8.HorizontalAlignment = Element.ALIGN_LEFT;
                    Sn8.FixedHeight = 15f;
                    Sn8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tblSignatire.AddCell(Sn8);


                    M11 = new PdfPCell(tblSignatire);
                    M11.HorizontalAlignment = Element.ALIGN_LEFT;
                    M11.Colspan = 8;
                    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    MstDetails_Table.AddCell(M11);
                }



                document.Add(MstDetails_Table);



                #endregion

                head.DeleteBodyRows();
                Main_Table1.DeleteBodyRows();
                Main_Table.DeleteBodyRows();
                Income_Table.DeleteBodyRows();
                MstDetails_Table.DeleteBodyRows();
                // document.NewPage();

                if (_baseAgencyControlDetails.State == "TX" || _baseAgencyControlDetails.State == "IN")
                {
                    if (_baseAgencyControlDetails.TXAlienSwitch == "Y")
                    {
                        DownloadTXAlienCertificate(document, snpActivedetails, dtdocs);
                    }
                }

                document.Close();
                fs.Close();
                fs.Dispose();

                if (_fromApp == "PL")
                {
                    //if (_emailSend)
                    await CopytoSignFolder(_PdfName, _docoutputname);
                    if (_baseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                    {
                        PdfViewerNewForm objfrm = new PdfViewerNewForm(_PdfName);
                        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;
                        objfrm.ShowDialog();
                    }
                    else
                    {
                        FrmViewer objfrm = new FrmViewer(_PdfName);
                        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;
                        objfrm.ShowDialog();
                    }

                }
                else if (_fromApp == "SD")
                {
                    string fromPDFPath = _PdfName;
                    string pdfFileName = _docoutputname + ".pdf";// "ClientIntake_AlienForm" + "_" + _baseApplicationNo + ".pdf";

                    string AppFolderName = _baseAgency + _baseDept + _baseProg + _baseApplicationNo;
                    string Appno = AppFolderName;
                    //string AppFolderPath = Application.StartupPath + "\\custdocsignpdfs\\" + AppFolderName + "\\";
                    string SignFolder = propReportPath + "\\SignForms\\";
                    if (!Directory.Exists(SignFolder))
                        Directory.CreateDirectory(SignFolder);

                    string AppFolderPath = SignFolder + AppFolderName + "\\";
                    if (!Directory.Exists(AppFolderPath))
                        Directory.CreateDirectory(AppFolderPath);

                    string _toPDFPath = AppFolderPath + "\\" + pdfFileName;

                    File.Copy(fromPDFPath, _toPDFPath, true);
                }
            }
            catch(Exception ex)
            {
                //AlertBox.Show(ex.Message.ToString(), MessageBoxIcon.Warning);
            }
        }
        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(_PdfName);
        }

        #region SaveCertificate Form
        private void DownloadTXAlienCertificate(Document _document, List<CaseSnpEntity> _lstSnpEntity, DataTable dtdocs)
        {
            try
            {
                _document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                _document.NewPage();


                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Arial.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bf_timesBold = BaseFont.CreateFont("c:/windows/fonts/ArialBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font tblFont1 = new iTextSharp.text.Font(bf_timesBold, 10);
                iTextSharp.text.Font tblFont2 = new iTextSharp.text.Font(bf_timesBold, 8);
                iTextSharp.text.Font tblFont3 = new iTextSharp.text.Font(bf_timesBold, 9);
                iTextSharp.text.Font tblFontReg = new iTextSharp.text.Font(bf_times, 9);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_timesBold, 7);

                PdfPTable tblMainpnl = new PdfPTable(2);
                tblMainpnl.TotalWidth = 740f;
                tblMainpnl.WidthPercentage = 100;
                tblMainpnl.LockedWidth = true;
                float[] widths = new float[] { 100f, 80f };   //80f, 25f, 30f, 35f, 35f
                tblMainpnl.SetWidths(widths);
                tblMainpnl.HorizontalAlignment = Element.ALIGN_CENTER;
                tblMainpnl.SpacingBefore = 20f;

                PdfPCell M1 = new PdfPCell(new Phrase("TEXAS DEPARTMENT OF HOUSING AND COMMUNITY AFFAIRS", tblFont1));
                M1.HorizontalAlignment = Element.ALIGN_LEFT;
                M1.FixedHeight = 15f;
                M1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblMainpnl.AddCell(M1);


                PdfPCell CellTexasLogo = new PdfPCell(new Phrase("  ", tblFont2));
                CellTexasLogo.VerticalAlignment = Element.ALIGN_TOP;
                CellTexasLogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellTexasLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellTexasLogo.Rowspan = 6;
                tblMainpnl.AddCell(CellTexasLogo);

                PdfPCell M2 = new PdfPCell(new Phrase("Household Status Verification Form", tblFont2));
                M2.HorizontalAlignment = Element.ALIGN_LEFT;
                M2.FixedHeight = 15f;
                M2.Colspan = 2;
                M2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblMainpnl.AddCell(M2);

                //PdfPCell M5 = new PdfPCell(new Phrase(" ", tblFont2));
                //M5.HorizontalAlignment = Element.ALIGN_LEFT;
                //M5.FixedHeight = 15f;
                //M5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //tblMainpnl.AddCell(M5);


                PdfPCell M3 = new PdfPCell(new Phrase("Systematic Alien Verification for Entitlements (SAVE) System and US Citizenship/US National", tblFont1));
                M3.HorizontalAlignment = Element.ALIGN_CENTER;
                M3.FixedHeight = 15f;
                M3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                M3.Colspan = 2;
                tblMainpnl.AddCell(M3);

                PdfPCell Cell4 = new PdfPCell(new Phrase("Applicant Certification Form for CEAP, DOE-WAP, LIHEAP-WAP Subrecipients, and SHTF, ESG, HHSP, EH (political subdivision only)", tblFont2));
                Cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                Cell4.FixedHeight = 15f;
                Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4.Colspan = 2;
                tblMainpnl.AddCell(Cell4);

                _document.Add(tblMainpnl);
                tblMainpnl.DeleteBodyRows();

                iTextSharp.text.Image _imgTexasLogo = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Images\\StateofTexas_logo.png"));
                _imgTexasLogo.ScalePercent(12f);
                _imgTexasLogo.SetAbsolutePosition(660f, 480f);
                _document.Add(_imgTexasLogo);



                Paragraph paragraph = new Paragraph();
                paragraph.SpacingBefore = 5f;
                Chunk titleChunk = new Chunk("The program for which you are applying requires verification that you are a U.S. citizen, a non-citizen national, or a legal resident of the United States.\r\nDocumentation of your status is required. This agency uses the Systematic Alien Verification for Entitlements (SAVE) System to verify the status of non-citizens.\r\n", tblFontReg);
                paragraph.Add(titleChunk);

                _document.Add(paragraph);

                #region Applicant Table

                PdfPTable tblAppDets = new PdfPTable(5);
                tblAppDets.TotalWidth = 740f;
                tblAppDets.WidthPercentage = 100;
                tblAppDets.LockedWidth = true;
                float[] widths1 = new float[] { 80f, 30f, 20f, 35f, 35f };   //80f, 25f, 30f, 35f, 35f
                tblAppDets.SetWidths(widths1);
                tblAppDets.HorizontalAlignment = Element.ALIGN_CENTER;
                tblAppDets.SpacingBefore = 8f;

                PdfPCell AC1 = new PdfPCell(new Phrase("", tblFont1));
                AC1.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                AC1.BorderWidthBottom = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC1);

                PdfPCell AC2 = new PdfPCell(new Phrase("U.S. Citizen\r\n(Born or Naturalized)\r\nor U.S. National", tblFont1));
                AC2.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                AC2.BorderWidthBottom = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC2);

                PdfPCell AC3 = new PdfPCell(new Phrase("Qualified\r\nAlien", tblFont1));
                AC3.HorizontalAlignment = Element.ALIGN_CENTER;
                AC3.VerticalAlignment = Element.ALIGN_BOTTOM;
                //AC1.FixedHeight = 15f;
                AC3.BorderWidthBottom = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC3);

                PdfPCell AC4 = new PdfPCell(new Phrase("Documentation Provided for:", tblFont1));
                AC4.HorizontalAlignment = Element.ALIGN_CENTER;
                AC4.VerticalAlignment = Element.ALIGN_BOTTOM;
                //AC1.FixedHeight = 15f;
                //AC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AC4.Colspan = 2;
                tblAppDets.AddCell(AC4);


                PdfPCell AC11 = new PdfPCell(new Phrase("Household Member Name", tblFont1));
                AC11.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                AC11.BorderWidthTop = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC11);

                PdfPCell AC22 = new PdfPCell(new Phrase("(Yes/No)", tblFont3));
                AC22.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                AC22.BorderWidthTop = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC22);

                PdfPCell AC33 = new PdfPCell(new Phrase("(Yes/No)", tblFont3));
                AC33.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                AC33.BorderWidthTop = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC33);

                PdfPCell AC44 = new PdfPCell(new Phrase("Citizenship/Qualified Alien", tblFont3));
                AC44.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                //AC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC44);
                PdfPCell AC55 = new PdfPCell(new Phrase("Identification", tblFont3));
                AC55.HorizontalAlignment = Element.ALIGN_CENTER;
                //AC1.FixedHeight = 15f;
                //AC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblAppDets.AddCell(AC55);

                int RowsCnt = 11;
                int SnpRowCnt = _lstSnpEntity.Count;
                int balRowCnt = 0;
                if (SnpRowCnt < RowsCnt)
                {
                    balRowCnt = RowsCnt - SnpRowCnt;
                }
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                string strIsUSCitizen = ""; string strIsQualifiedAlien = "";
                DataTable _dtAlienVer = AlienTXDB.GET_ALIENVER("", _baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, "GET");
                foreach (CaseSnpEntity SNPitem in _lstSnpEntity)
                {
                    strIsUSCitizen = "No"; strIsQualifiedAlien = "";
                    if (SNPitem.Resident == "A")
                        strIsUSCitizen = "Yes";
                    else if (SNPitem.Resident == "B")
                        strIsQualifiedAlien = "Yes";

                    PdfPCell BC11 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(ti.ToTitleCase(SNPitem.NameixFi.ToLower()), SNPitem.NameixMi, ti.ToTitleCase(SNPitem.NameixLast.ToLower()), "6"), tblFontReg));
                    BC11.HorizontalAlignment = Element.ALIGN_LEFT;
                    BC11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    BC11.FixedHeight = 20f;
                    tblAppDets.AddCell(BC11);

                    PdfPCell BC22 = new PdfPCell(new Phrase(strIsUSCitizen, tblFontReg));
                    BC22.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC22.VerticalAlignment = Element.ALIGN_MIDDLE;
                    BC22.FixedHeight = 20f;
                    tblAppDets.AddCell(BC22);

                    PdfPCell BC33 = new PdfPCell(new Phrase(strIsQualifiedAlien, tblFontReg));
                    BC33.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC33.VerticalAlignment = Element.ALIGN_MIDDLE;
                    BC33.FixedHeight = 20f;
                    tblAppDets.AddCell(BC33);

                    string _strCitizen = " ";
                    string _strIsIdentity = " ";
                    if (_dtAlienVer.Rows.Count > 0)
                    {
                        DataRow[] drFamMem = _dtAlienVer.Select("ALN_VER_FAM_SEQ='" + SNPitem.FamilySeq + "'");
                        if (drFamMem.Length > 0)
                        {
                            _strCitizen = drFamMem[0]["ALN_VER_CITIZEN"].ToString();
                            _strIsIdentity = drFamMem[0]["ALN_VER_IDENT"].ToString();
                        }

                    }

                    PdfPCell BC44 = new PdfPCell(new Phrase(_strCitizen, tblFontReg));
                    BC44.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC44.VerticalAlignment = Element.ALIGN_MIDDLE;
                    BC44.FixedHeight = 20f;
                    tblAppDets.AddCell(BC44);

                    PdfPCell BC55 = new PdfPCell(new Phrase(_strIsIdentity, tblFontReg));
                    BC55.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC55.VerticalAlignment = Element.ALIGN_MIDDLE;
                    BC55.FixedHeight = 20f;
                    tblAppDets.AddCell(BC55);
                }

                for (int x = 0; x < balRowCnt; x++)
                {
                    PdfPCell BC11 = new PdfPCell(new Phrase(" ", tblFontReg));
                    BC11.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC11.FixedHeight = 20f;
                    tblAppDets.AddCell(BC11);

                    PdfPCell BC22 = new PdfPCell(new Phrase(" ", tblFontReg));
                    BC22.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC22.FixedHeight = 20f;
                    tblAppDets.AddCell(BC22);

                    PdfPCell BC33 = new PdfPCell(new Phrase(" ", tblFontReg));
                    BC33.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC33.FixedHeight = 20f;
                    tblAppDets.AddCell(BC33);

                    PdfPCell BC44 = new PdfPCell(new Phrase(" ", tblFontReg));
                    BC44.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC44.FixedHeight = 20f;
                    tblAppDets.AddCell(BC44);

                    PdfPCell BC55 = new PdfPCell(new Phrase(" ", tblFontReg));
                    BC55.HorizontalAlignment = Element.ALIGN_CENTER;
                    BC55.FixedHeight = 20f;
                    tblAppDets.AddCell(BC55);
                }

                PdfPCell BCNote = new PdfPCell(new Phrase("To add additional household members, use another copy of this form.", tblFontReg));
                BCNote.HorizontalAlignment = Element.ALIGN_LEFT;
                BCNote.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BCNote.Colspan = 5;
                tblAppDets.AddCell(BCNote);

                _document.Add(tblAppDets);


                #endregion


                Paragraph phrase2 = new Paragraph();
                phrase2.SpacingBefore = 8f;
                Chunk chnk2 = new Chunk("I AM AWARE THAT I AM SUBJECT TO PROSECUTION FOR PROVIDING FALSE OR FRAUDULANT INFORMATION.", tblFont1);
                phrase2.Add(chnk2);
                _document.Add(phrase2);


                PdfPTable tblSign1 = new PdfPTable(3);
                tblSign1.TotalWidth = 740f;
                tblSign1.WidthPercentage = 100;
                tblSign1.LockedWidth = true;
                float[] widths3 = new float[] { 100f, 35f, 35f };   //80f, 25f, 30f, 35f, 35f
                tblSign1.SetWidths(widths3);
                tblSign1.HorizontalAlignment = Element.ALIGN_CENTER;
                tblSign1.SpacingBefore = 5f;

                if (_fromApp == "PL")
                {
                    PdfPCell SC1 = new PdfPCell(new Phrase("", tblFont1));
                    SC1.HorizontalAlignment = Element.ALIGN_CENTER;
                    SC1.FixedHeight = 20f;
                    SC1.Colspan = 2;
                    tblSign1.AddCell(SC1);

                    PdfPCell SC2 = new PdfPCell(new Phrase(" ", tblFont1));
                    SC2.HorizontalAlignment = Element.ALIGN_CENTER;
                    SC2.FixedHeight = 20f;
                    tblSign1.AddCell(SC2);
                }
                else if (_fromApp == "SD")
                {
                    iTextSharp.text.Image _imgSignature = iTextSharp.text.Image.GetInstance(_SignImagePath); //Application.MapPath("~\\Resources\\Images\\StateofTexas_logo.png")
                    if (_imgSignature.ImageMask.PlainWidth > 1600)
                        _imgSignature.ScalePercent(6f);
                    else
                        _imgSignature.ScalePercent(8f);
                    //_imgSignature.SetAbsolutePosition(30f, 45f);

                    PdfPCell SC1 = new PdfPCell(_imgSignature);
                    SC1.HorizontalAlignment = Element.ALIGN_LEFT;
                    SC1.PaddingLeft = 15f;
                    SC1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    // SC1.FixedHeight = 20f;
                    SC1.Colspan = 2;
                    tblSign1.AddCell(SC1);

                    PdfPCell SC2 = new PdfPCell(new Phrase(DateTime.Now.ToString("MM/dd/yyyy"), tblFont1));
                    SC2.HorizontalAlignment = Element.ALIGN_LEFT;
                    SC1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    SC2.FixedHeight = 20f;
                    tblSign1.AddCell(SC2);

                }

                PdfPCell SC11 = new PdfPCell(new Phrase("Applicant's Signature", tblFont2));
                SC11.HorizontalAlignment = Element.ALIGN_LEFT;
                SC11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SC11.Colspan = 2;
                tblSign1.AddCell(SC11);

                PdfPCell SC22 = new PdfPCell(new Phrase("Date", tblFont2));
                SC22.HorizontalAlignment = Element.ALIGN_LEFT;
                SC22.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign1.AddCell(SC22);

                _document.Add(tblSign1);


                PdfPTable tblSign2 = new PdfPTable(3);
                tblSign2.TotalWidth = 740f;
                tblSign2.WidthPercentage = 100;
                tblSign2.LockedWidth = true;
                float[] widths4 = new float[] { 100f, 35f, 35f };   //80f, 25f, 30f, 35f, 35f
                tblSign2.SetWidths(widths4);
                tblSign2.HorizontalAlignment = Element.ALIGN_CENTER;
                tblSign2.SpacingBefore = 10;

                PdfPCell SC21 = new PdfPCell(new Phrase("", tblFont1)); //Signature
                SC21.HorizontalAlignment = Element.ALIGN_CENTER;
                SC21.FixedHeight = 20f;
                tblSign2.AddCell(SC21);

                PdfPCell SC22o = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(_baseUserProfile.FirstName.Trim(), "", _baseUserProfile.LastName.Trim(), "3"), tblFont1));   // print staff
                SC22o.HorizontalAlignment = Element.ALIGN_LEFT;
                SC22o.FixedHeight = 20f;
                tblSign2.AddCell(SC22o);

                string StaffDate = DateTime.Now.ToString("MM/dd/yyyy");
                if (dtdocs.Rows.Count > 0)
                    StaffDate = LookupDataAccess.Getdate(dtdocs.Rows[0]["DCSN_HIS_ADD_DATE"].ToString());

                PdfPCell SC23o = new PdfPCell(new Phrase(StaffDate, tblFont1));   // Date
                SC23o.HorizontalAlignment = Element.ALIGN_LEFT;
                SC23o.FixedHeight = 20f;
                tblSign2.AddCell(SC23o);

                PdfPCell SC211 = new PdfPCell(new Phrase("Signature of agency staff certifying they verified the above documents", tblFont2));
                SC211.HorizontalAlignment = Element.ALIGN_LEFT;
                SC211.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign2.AddCell(SC211);

                PdfPCell SC222 = new PdfPCell(new Phrase("Print Staff Name", tblFont2));
                SC222.HorizontalAlignment = Element.ALIGN_LEFT;
                SC222.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign2.AddCell(SC222);

                PdfPCell SC223 = new PdfPCell(new Phrase("Date", tblFont2));
                SC223.HorizontalAlignment = Element.ALIGN_LEFT;
                SC223.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign2.AddCell(SC223);
                _document.Add(tblSign2);



                PdfPTable tblSign3 = new PdfPTable(2);
                tblSign3.TotalWidth = 740f;
                tblSign3.WidthPercentage = 100;
                tblSign3.LockedWidth = true;
                float[] widths5 = new float[] { 100f, 35f };   //80f, 25f, 30f, 35f, 35f
                tblSign3.SetWidths(widths5);
                tblSign3.HorizontalAlignment = Element.ALIGN_CENTER;
                tblSign3.SpacingBefore = 35;

                PdfPCell SCu211 = new PdfPCell(new Phrase("HSV Form: Updated 12/2019", tblFont2));
                SCu211.HorizontalAlignment = Element.ALIGN_LEFT;
                SCu211.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign3.AddCell(SCu211);

                PdfPCell SCu223 = new PdfPCell(new Phrase("Previous Versions Obsolete", tblFont2));
                SCu223.HorizontalAlignment = Element.ALIGN_RIGHT;
                SCu223.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tblSign3.AddCell(SCu223);

                _document.Add(tblSign3);

            }
            catch (Exception ex)
            {
            }
        }
        #endregion


        public async Task CopytoSignFolder(string _PdfName, string outputFileName)
        {
            /***************************************** COPY PDF File to Signatures folder *******************************************************/
            /***********************************************************************************************************************************/
            // if (_isSendMail)
            //{
            string fromPDFPath = _PdfName;
            string pdfFileName = outputFileName + ".pdf"; // outputFileName + "_" + _baseApplicationNo + ".pdf";

            string AppFolderName = _baseAgency + _baseDept + _baseProg + _baseApplicationNo;
            string HIEAppno = AppFolderName;
            //string AppFolderPath = Application.StartupPath + "\\custdocsignpdfs\\" + AppFolderName + "\\";
            string SignFolder = propReportPath + "\\SignForms\\";
            if (!Directory.Exists(SignFolder))
                Directory.CreateDirectory(SignFolder);

            string AppFolderPath = SignFolder + AppFolderName + "\\";
            if (!Directory.Exists(AppFolderPath))
                Directory.CreateDirectory(AppFolderPath);

            string _toPDFPath = AppFolderPath + "\\" + pdfFileName;

            File.Copy(fromPDFPath, _toPDFPath, true);

            var model = new CaptainModel();
            var AgyCntrlDets = model.ZipCodeAndAgency.GetAgencyControlFile("00");

            string strStreet = "", strCity = "", strState = "", strZIP = "";
            if (AgyCntrlDets != null)
            {
                if (AgyCntrlDets.Street != "")
                    strStreet = AgyCntrlDets.Street + ", ";
                if (AgyCntrlDets.City != "")
                    strCity = AgyCntrlDets.City + ", ";
                if (AgyCntrlDets.State != "")
                    strState = AgyCntrlDets.State + ", ";
                if (AgyCntrlDets.Zip1 != "")
                    strZIP = AgyCntrlDets.Zip1 + ".";
            }

            string _officeAddress = strStreet + strCity + (strCity != "" ? ("" + strState) : strState) + strZIP;


            //if (_baseCaseMstListEntity[0].Email.Trim().ToLower() == "kranthivardhan@gmail.com" || _baseCaseMstListEntity[0].Email.Trim().ToLower() == "ram@capsystems.com"
            //        || _baseCaseMstListEntity[0].Email.Trim().ToLower() == "vikashchatla1516@gmail.com"
            //        || _baseCaseMstListEntity[0].Email.Trim().ToLower() == "bcayer@capsystems.com" || _baseCaseMstListEntity[0].Email.Trim().ToLower() == "sudheergedala@gmail.com")
            //    {
            if (_isSendMail)
                await SendEmail(_baseCaseMstListEntity[0].Email.Trim(), HIEAppno, "1", _officeAddress);

            // }
            if (SaveDocSignHist(pdfFileName, docSeqno.ToString()))
            {
                //if (_isSendMail)
                //  AlertBox.Show("Sent to client’s email: " + _baseCaseMstListEntity[0].Email.Trim());
            }
            //}
            /************************************************************************************************/
        }

        AlienTXData _oAlienTXData = new AlienTXData();
        bool SaveDocSignHist(string _DocName, string _docSeqno)
        {
            string _strPrintFor = "R";
            if (_isSendMail)
                _strPrintFor = "S";

            bool _isSaveFlag = _oAlienTXData.INSUPDEL_DOCSIGNHIS("", _baseAgency, _baseDept, _baseProg, _baseYear, _baseApplicationNo, _docCode,
                _baseCaseMstListEntity[0].Email.Trim(), _isSignReqired, "", "", _baseUSERID, _docSeqno, _DocName, _strPrintFor, (_isSendMail == true ? "Y" : "N"),_emailStatus,
               _emailStatusDate,"ADD");

            return _isSaveFlag;
        }

        public async Task SendEmail(string ToemailID, string HIEAppno, string Keycode, string _officeAddr)
        {
            try
            {
                Application.ShowLoader = true;

                var model = new CaptainModel();
                var dtMailConfig = model.UserProfileAccess.GetEMailSetting("SIGNPDF");

                if (dtMailConfig.Rows.Count > 0)
                {
                    //var mailMessage = new MailMessage();
                    //mailMessage.From = new MailAddress(dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString(), _baseAgyShortName);
                    //mailMessage.Subject = dtMailConfig.Rows[0]["MAIL_SUBJECT"].ToString();

                    /******************************* PARAMETERS FOR URL ***************************************/
                    /*****************************************************************************************/
                    List<SignParams> _signParams = new List<SignParams>();
                    SignParams _sgParms = new SignParams();
                    _sgParms.HIEAppno = HIEAppno;
                    _sgParms.baseApplicationNo = _baseApplicationNo;
                    _sgParms.baseApplicationName = _baseApplicationName;
                    _sgParms.baseUSERID = _baseUSERID;
                    _sgParms.baseAgency = _baseAgency;
                    _sgParms.baseDept = _baseDept;
                    _sgParms.baseProg = _baseProg;
                    _sgParms.baseYear = _baseYear;
                    _sgParms.baseAgyShortName = _baseAgyShortName;
                    _sgParms.keyCode = Keycode;
                    _signParams.Add(_sgParms);
                    var jsonParms = JsonConvert.SerializeObject(_signParams);

                    /**********************************************************************/
                    /**********************************************************************/
                    var body = dtMailConfig.Rows[0]["MAIL_CONTENT"].ToString() + " <a href=" + Application.Uri.AbsoluteUri + "/custdocsign.aspx?idparms=" + WM_EncryptUrl(jsonParms.ToString()) + "  target='_blank' style='background-color: #ff6a00; border-radius:15px; border: none;color: white;padding: 10px 20px;text-align: center;text-decoration: none;display: inline-block;font-size: 22px;margin: 4px 2px;cursor: pointer;'>Click here to sign on your documents</a>";
                    body = body + "<br/><br/><br/><br/><br/>Thank You,<br/><b>" + _baseAgencyName.ToUpper() + "</b><br/><b>" + _officeAddr + "</b>";

                    body = body.Replace("User,", _baseApplicationName);
                    //mailMessage.Body = body;
                    //mailMessage.IsBodyHtml = true;

                    //mailMessage.To.Add(ToemailID);


                    //var smtp = new SmtpClient();
                    //smtp.Host = dtMailConfig.Rows[0]["MAIL_HOST"].ToString();
                    //smtp.EnableSsl = true;
                    //var NetworkCred = new System.Net.NetworkCredential();
                    //NetworkCred.UserName = dtMailConfig.Rows[0]["MAIL_EMAILID"].ToString();
                    //NetworkCred.Password = dtMailConfig.Rows[0]["MAIL_PASSWORD"].ToString();
                    //smtp.UseDefaultCredentials = true;
                    //smtp.Credentials = NetworkCred;
                    //smtp.Port = int.Parse(dtMailConfig.Rows[0]["MAIL_PORT"].ToString());
                    //smtp.Send(mailMessage);


                    /*******************************************************/
                    TwilloSendGridEmail obj = new TwilloSendGridEmail();
                    await obj.SendEmailAsync(ToemailID, dtMailConfig.Rows[0]["MAIL_SUBJECT"].ToString(), body, _baseAgyShortName.ToUpper(),
                        _baseAgyShortName.ToUpper(), "Alien Form Sign Doc", "Sent to client’s email: " + ToemailID);
                    /*******************************************************/
                    //_isemailSent = "Y";
                    _emailStatus = obj.Emailstatus;
                    _emailStatusDate = obj.EmailstatusDate;

                    //if (_emailStatus == "delivered")
                    //    _isemailSent = "Y";
                    //else
                    //    _isemailSent = "N";
                }
            }
            catch (Exception ex)
            {
                //_isemailSent = "N";
                // pnlUserlogin.Visible = true;
                AlertBox.Show("Failed to send email.", MessageBoxIcon.Warning);
                //lblerrormsg.Text = ex.Message; //"email not delivered!";

                //lblerrormsg.Text = "email not delivered!";          
                //Response.Write(ex.Message);
            }
        }
        public string _emailStatus = "";
        public string _emailStatusDate = "";
        public string GetIncomeIntervalDesc(string Interval)
        {
            string Desc = string.Empty;
            if (IncomeInterValList.Count > 0)
            {
                CommonEntity IncInterval = IncomeInterValList.Find(u => u.Code.Trim().Equals(Interval.Trim()));
                if (IncInterval != null) Desc = IncInterval.Desc.Trim();
            }
            return Desc;
        }
        public string WM_EncryptUrl(string ourl)
        {
            string result = string.Empty;

            try
            {
                string _encrypturl = HttpUtility.UrlEncode(AlienTXDB.Encrypt(ourl.ToString()));
                result = _encrypturl;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }


        //THIS IS TEST AREA REMOVE AFTER COMPLETION OF PDF
        //public SignPDFGeneration(string SignImagePath)
        //{
        //    _model = new CaptainModel();
        //    propReportPath = _model.lookupDataAccess.GetReportPath();
        //    _PdfName = "1214" + "Report";//form.GetFileName();
        //    _PdfName = propReportPath + "SYSTEM-1\\" + _PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + "SYSTEM-1"))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + "SYSTEM-1"); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = _PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = _PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        _PdfName = Random_Filename;
        //    else
        //        _PdfName += ".pdf";


        //    FileStream fs = new FileStream(_PdfName, FileMode.Create);

        //    Document document = new Document();
        //    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();
        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Arial.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    BaseFont bf_timesBold = BaseFont.CreateFont("c:/windows/fonts/ArialBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    //BaseFont bf_times_Check = BaseFont.CreateFont("c:/windows/fonts/WINGDNG2.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    //iTextSharp.text.Font Times_Check = new iTextSharp.text.Font(bf_times_Check, 10);

        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 8, 4, BaseColor.BLUE);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 7);
        //    iTextSharp.text.Font TableFont10 = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont12 = new iTextSharp.text.Font(bf_times, 12);
        //    //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 7, 3);

        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_timesBold, 7);
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
        //    iTextSharp.text.Font TableFontBold12 = new iTextSharp.text.Font(bf_times, 12, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 7, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
        //    cb = writer.DirectContent;

        //    //Temp table not displayed on the screen
        //    PdfPTable head = new PdfPTable(1);
        //    head.HorizontalAlignment = Element.ALIGN_CENTER;
        //    head.TotalWidth = 50f;

        //    PdfPCell headcell = new PdfPCell(new Phrase(""));
        //    headcell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    head.AddCell(headcell);


        //    PdfPCell M11 = new PdfPCell(new Phrase("Signature of Applicant:_________________________________________ Date:___________________  TEST Staff Signature:_________________________________________ Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "", TableFontBoldItalic));
        //    M11.HorizontalAlignment = Element.ALIGN_LEFT;
        //    M11.Colspan = 8;
        //    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    head.AddCell(M11);

        //    PdfPTable tblSignatire = new PdfPTable(8);
        //    tblSignatire.TotalWidth = 740f;
        //    tblSignatire.WidthPercentage = 100;
        //    tblSignatire.LockedWidth = true;
        //    float[] widthsSign = new float[] { 40f, 80f, 20f, 50f, 100f, 50f, 20f, 50f };   //80f, 25f, 30f, 35f, 35f
        //    tblSignatire.SetWidths(widthsSign);
        //    tblSignatire.HorizontalAlignment = Element.ALIGN_CENTER;
        //    //tblSignatire.SpacingBefore = 20f;

        //    PdfPCell Sn1 = new PdfPCell(new Phrase("Signature of Applicant:", TableFontBoldItalic));
        //    Sn1.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn1.FixedHeight = 15f;
        //    Sn1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn1);


        //    iTextSharp.text.Image _imgSignature = iTextSharp.text.Image.GetInstance(SignImagePath); //Application.MapPath("~\\Resources\\Images\\StateofTexas_logo.png")
        //    _imgSignature.ScalePercent(12f);
        //    _imgSignature.SetAbsolutePosition(660f, 480f);
        //    //_document.Add(_imgTexasLogo);

        //    PdfPCell Sn2 = new PdfPCell(_imgSignature);
        //    Sn2.VerticalAlignment = Element.ALIGN_TOP;
        //    Sn2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    Sn2.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn2);

        //    PdfPCell Sn3 = new PdfPCell(new Phrase(" Date:", TableFontBoldItalic));
        //    Sn3.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn3.FixedHeight = 15f;
        //    Sn3.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn3);

        //    PdfPCell Sn4 = new PdfPCell(new Phrase("___________________ ", TableFontBoldItalic));
        //    Sn4.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn4.FixedHeight = 15f;
        //    Sn4.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn4);

        //    PdfPCell Sn5 = new PdfPCell(new Phrase(" Staff Signature:", TableFontBoldItalic));
        //    Sn5.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn5.FixedHeight = 15f;
        //    Sn5.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn5);

        //    PdfPCell Sn6 = new PdfPCell(new Phrase("_________________________________________", TableFontBoldItalic));
        //    Sn6.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn6.FixedHeight = 15f;
        //    Sn6.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn6);

        //    PdfPCell Sn7 = new PdfPCell(new Phrase("Date: ", TableFontBoldItalic));
        //    Sn7.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn7.FixedHeight = 15f;
        //    Sn7.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn7);

        //    PdfPCell Sn8 = new PdfPCell(new Phrase(DateTime.Now.ToString("MM/dd/yyyy"), TableFontBoldItalic));
        //    Sn8.HorizontalAlignment = Element.ALIGN_LEFT;
        //    Sn8.FixedHeight = 15f;
        //    Sn8.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    tblSignatire.AddCell(Sn8);


        //    M11 = new PdfPCell(tblSignatire);
        //    M11.HorizontalAlignment = Element.ALIGN_LEFT;
        //    M11.Colspan = 8;
        //    M11.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    head.AddCell(M11);

        //    document.Add(head);
        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();

        //    string fromPDFPath = _PdfName;
        //    string pdfFileName = "ClientIntake_AlienForm" + "_1214.pdf";

        //    string AppFolderName = "01010100100026";// _baseAgency + _baseDept + _baseProg + _baseApplicationNo;
        //    string Appno = AppFolderName;
        //    //string AppFolderPath = Application.StartupPath + "\\custdocsignpdfs\\" + AppFolderName + "\\";
        //    string SignFolder = propReportPath + "\\SignForms\\";
        //    if (!Directory.Exists(SignFolder))
        //        Directory.CreateDirectory(SignFolder);

        //    string AppFolderPath = SignFolder + AppFolderName + "\\";
        //    if (!Directory.Exists(AppFolderPath))
        //        Directory.CreateDirectory(AppFolderPath);

        //    string _toPDFPath = AppFolderPath + "\\" + pdfFileName;

        //    File.Copy(fromPDFPath, _toPDFPath, true);
        //}
    }
}

public class SignParams
{
    public string HIEAppno { get; set; }
    //public PrivilegeEntity baseprivileges { get; set; }
    //public List<CommonEntity> baseAgyTabsEntity { get; set; }
    //public List<CaseMstEntity> baseCaseMstListEntity { get; set; }
    //public List<CaseSnpEntity> baseCaseSnpEntity { get; set; }
    //public AgencyControlEntity baseAgencyControlDetails { get; set; }
    //public UserEntity baseUserProfile { get; set; }
    public string baseApplicationNo { get; set; }
    public string baseApplicationName { get; set; }
    public string baseUSERID { get; set; }
    public string baseAgency { get; set; }
    public string baseDept { get; set; }
    public string baseProg { get; set; }
    public string baseYear { get; set; }
    public string baseAgyShortName { get; set; }
    public string keyCode { get; set; }
}
