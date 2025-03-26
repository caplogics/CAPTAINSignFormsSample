
/************************************************************************
 * Conversion On    :   01/02/2023      * Converted By     :   Kranthi
 * Modified On      :   01/02/2023      * Modified By      :   Kranthi
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.IO;
using System.Linq;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;
using System.Globalization;
using Wisej.Web;
using Captain.DatabaseLayer;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class LetterHistory : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private string strNameFormat = string.Empty;
        private string strCwFormat = string.Empty;
        private string strYear = "    ";
        private int strIndex = 0;

        #endregion

        public LetterHistory(BaseForm baseForm, PrivilegeEntity privileges, string Form_Name, string eligStatus)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Privileges = privileges;
            FormName = Form_Name;
            EligStatus = eligStatus;
            _model = new CaptainModel();

            this.Text = "Letters History";
            propReportPath = _model.lookupDataAccess.GetReportPath();
            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                AGYShortName = dsAgency.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
            }
            FillStatusCombo();
            FillLettersCombo();

            Getdata();
           // FillGrid();
            strNameFormat = BaseForm.BaseHierarchyCnFormat.ToString();
            strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";

            cmbStatusType_SelectedIndexChanged(this, new EventArgs());
        }

        void FillStatusCombo()
        {
            cmbStatusType.SelectedIndexChanged -= new EventHandler(cmbStatusType_SelectedIndexChanged);
            cmbStatusType.Items.Clear();

            Captain.Common.Utilities.ListItem li1 = new Captain.Common.Utilities.ListItem("Sent to Applicant", "S");
            cmbStatusType.Items.Add(li1);
            Captain.Common.Utilities.ListItem li2 = new Captain.Common.Utilities.ListItem("Printed for Reference", "R");
            cmbStatusType.Items.Add(li2);
            Captain.Common.Utilities.ListItem li0 = new Captain.Common.Utilities.ListItem("All", "All");
            cmbStatusType.Items.Add(li0);

            //if (strSelCmbValue != "")
            //{
            //    CommonFunctions.SetComboBoxValue(cmbStatusType, strSelCmbValue);
            //}
            //else
            //{
            //cmbFileStatus.SelectedIndex = 2;
            cmbStatusType.SelectedIndex = 0;
            //}
            cmbStatusType.SelectedIndexChanged += new EventHandler(cmbStatusType_SelectedIndexChanged);
        }
        void FillLettersCombo()
        {
            cmbLetterTypes.SelectedIndexChanged -= new EventHandler(cmbLetterTypes_SelectedIndexChanged);
            cmbLetterTypes.Items.Clear();
            DataSet ds = DatabaseLayer.MainMenu.GET_PRINAPPCNTL(string.Empty, string.Empty, string.Empty, "L");
            DataTable dt1 = ds.Tables[0];
            DataRow[] drAgy = dt1.Select("PAC_AGY='" + BaseForm.BaseAgency + "' AND PAC_ENABLE = 'Y' AND PAC_TYPE='L'");
            if (drAgy.Length == 0)
                drAgy = dt1.Select("PAC_AGY='**' AND PAC_ENABLE = 'Y'");

            if (drAgy.Length > 0)
            {
                Captain.Common.Utilities.ListItem li0 = new Captain.Common.Utilities.ListItem("All Letters", "All");
                cmbLetterTypes.Items.Add(li0);

                foreach (DataRow dr in drAgy)
                {
                    string _Name = dr["PAC_DISP_NAME"].ToString().Trim();
                    string _ID = dr["PAC_FORM"].ToString().Trim();

                    Captain.Common.Utilities.ListItem li1 = new Captain.Common.Utilities.ListItem(_Name, _ID);
                    cmbLetterTypes.Items.Add(li1);
                }

                cmbLetterTypes.SelectedIndex = 0;
            }
            cmbLetterTypes.SelectedIndexChanged += new EventHandler(cmbLetterTypes_SelectedIndexChanged);
        }

        private void cmbStatusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillHistoryGrid();
        }

        private void cmbLetterTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillHistoryGrid();
        }

        //Kranthi :: 02/28/2024 :: Created Newly
        void FillHistoryGrid()
        {
            gvApp.Rows.Clear();
            string statusType = ((Utilities.ListItem)cmbStatusType.SelectedItem).Value.ToString();
            string docCode = ((Utilities.ListItem)cmbLetterTypes.SelectedItem).Value.ToString();

            DataTable dtRecords = new DataTable();
            DataTable dt = AlienTXDB.GET_DOCSIGNHIS(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,
                      BaseForm.BaseYear, BaseForm.BaseApplicationNo, "", "", "Y", "GET");
            if (dt.Rows.Count > 0)
            {
                string statuscodition = "";
                if (statusType != "All")
                    statuscodition = "DCSN_PRINT_FOR = '" + statusType + "'";
                if (docCode != "All")
                {
                    if (statuscodition != "")
                        statuscodition += "AND DCSN_HIS_DOC_CODE='" + docCode + "'";
                    else
                        statuscodition = "DCSN_HIS_DOC_CODE='" + docCode + "'";
                }
                if (statuscodition != "")
                {
                    DataRow[] drRecords = dt.Select(statuscodition);
                    if (drRecords.Length > 0)
                        dtRecords = drRecords.CopyToDataTable();
                }
                else
                    dtRecords = dt;

                string _prvCode = "";
                foreach (DataRow dr in dtRecords.Rows)
                {
                    string strNameofLetter = "";

                    if (dr["PAC_DISP_NAME"].ToString() != _prvCode)
                        strNameofLetter = dr["PAC_DISP_NAME"].ToString();

                    string strprintedby = CaseworkerName(dr["DCSN_HIS_LSTC_OPERATOR"].ToString().Trim());// dr["DCSN_HIS_LSTC_OPERATOR"].ToString();
                    string strprintedOn = dr["DCSN_HIS_LSTC_DATE"].ToString();
                    string strDownloadedOn = dr["DCSN_HIS_DOWNLOAD"].ToString();

                    string strSignedON = dr["DCSN_HIS_SIGND_ON"].ToString();

                    string strMovedtoCAPTAIN = "";
                    string strMovedby = "";

                    if (dr["DCSN_MOVD_CAP"].ToString() == "M")
                    {
                        strMovedtoCAPTAIN = dr["DCSN_HIS_LSTC_DATE"].ToString();
                        strMovedby = CaseworkerName(dr["DCSN_HIS_LSTC_OPERATOR"].ToString().Trim());
                    }

                    gvApp.Rows.Add(strNameofLetter, strprintedby, strprintedOn, dr["DCSN_EMAIL_STATUS"].ToString().ToTitleCase(), strDownloadedOn, strSignedON, strMovedtoCAPTAIN, strMovedby);

                    _prvCode= dr["PAC_DISP_NAME"].ToString();
                }
            }
        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string FormName { get; set; }
        public string EligStatus { get; set; }
        public string PAgency { get; set; }
        public string PDept { get; set; }
        public string PProgram { get; set; }
        public string PYear { get; set; }
        public string PValue { get; set; }
        public string AGYShortName { get; set; }

        public List<CaseServicesEntity> PropSerViceEntity { get; set; }
        public string propReportPath { get; set; }
        public List<FldcntlHieEntity> preassesCntlEntity { get; set; }
        public List<TMS81ReportEntity> ReportDetails { get; set; }
        public List<AgyTabEntity> AgyList { get; set; }
        public AgyTabEntity AgyMain { get; set; }
        public List<CAMASTEntity> CAMASTList { get; set; }

        public List<CommonEntity> IncomeInterValList { get; set; }

        #endregion

        List<HierarchyEntity> propCaseworkerList = new List<HierarchyEntity>();
        HierarchyEntity CaseWorker = new HierarchyEntity();
        private void Getdata()
        {
            //CaseServicesEntity SearchEntity = new CaseServicesEntity(true);
            //SearchEntity.Agency = BaseForm.BaseAgency;
            //SearchEntity.Dept = BaseForm.BaseDept;
            //SearchEntity.Program = BaseForm.BaseProg;
            //SearchEntity.Application = "ES";
            //PropSerViceEntity = _model.CaseMstData.Browse_CASESER(SearchEntity, "Browse");

            propCaseworkerList = _model.CaseMstData.GetCaseWorker("I", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

            if (propCaseworkerList.Count > 0)
                CaseWorker = propCaseworkerList.Find(u => u.UserID == BaseForm.UserID);

            //CAMASTList = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);

            AgyList = _model.Agytab.GetAgyTab(string.Empty);

            //IncomeInterValList = CommonFunctions.AgyTabsFilterCodeStatus(BaseForm.BaseAgyTabsEntity, "S0015", string.Empty, string.Empty, string.Empty, string.Empty);


        }

        private string CaseworkerName(string WorkerCode)
        {
            string Desc = string.Empty;

            if (propCaseworkerList.Count > 0)
            {
                HierarchyEntity WorkerList = propCaseworkerList.Find(u => u.UserID.Trim() == WorkerCode);
                if (WorkerList != null)
                    Desc = WorkerList.HirarchyName.Trim();
                else Desc = WorkerCode;
            }

            return Desc;
        }

        string DEPState = string.Empty;
        private void FillGrid()
        {
            gvApp.Rows.Clear();
            int rowIndex = 0; DEPState = string.Empty;
            //DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //DataTable casedep = ds.Tables[0];

            List<LETRHISTCEntity> LetterList = _model.SPAdminData.GetLetrHistData(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear.Trim(), BaseForm.BaseApplicationNo);

            if (LetterList.Count > 0)
            {
                //LetterList = LetterList.OrderByDescending(u => u.DATE).ThenBy(u => u.LETR_CODE).ToList();
                string PrivCode = string.Empty; bool IsFirst = false;
                if (BaseForm.BaseAgencyControlDetails.AgyShortName.ToUpper() == "PCS")
                {
                    foreach (LETRHISTCEntity Entity in LetterList)
                    {
                        if (PrivCode != Entity.LETR_CODE.Trim()) IsFirst = true;

                        switch (Entity.LETR_CODE)
                        {
                            case "1": if (IsFirst) { gvApp.Rows.Add("Eligibilty Letter", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "1", "P"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "1", "P"); break;
                            case "2": if (IsFirst) { gvApp.Rows.Add("CEAP Priority Rating Form", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); break;
                            case "10": if (IsFirst) { gvApp.Rows.Add("Client Intake & SAVE Form", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); break;
                        }

                        PrivCode = Entity.LETR_CODE.Trim();
                    }
                }
                else
                {
                    foreach (LETRHISTCEntity Entity in LetterList)
                    {
                        if (PrivCode != Entity.LETR_CODE.Trim()) IsFirst = true;

                        switch (Entity.LETR_CODE)
                        {
                            case "1": if (IsFirst) { gvApp.Rows.Add("Delay-in-eligibility determination", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "1", "P"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "1", "P"); break;
                            case "2": if (IsFirst) { gvApp.Rows.Add("Eligibility-notification", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "2", "E"); break;
                            case "3": if (IsFirst) { gvApp.Rows.Add("Denial Notice", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "3", "D"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "3", "D"); break;
                            case "4": if (IsFirst) { gvApp.Rows.Add("Right to Appeal Notice", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "4", "D"); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "4", "D"); break;
                            case "5": if (IsFirst) { gvApp.Rows.Add("CEAP Benefit fulfillment form", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "5", ""); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "5", ""); break;
                            case "6": if (IsFirst) { gvApp.Rows.Add("Client satisfaction survey", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "6", ""); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "6", ""); break;
                            case "7": if (IsFirst) { gvApp.Rows.Add("Termination Notification", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "7", ""); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "7", ""); break;
                            case "8": if (IsFirst) { gvApp.Rows.Add("CEAP Priority Rating Form", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "8", ""); IsFirst = false; } else gvApp.Rows.Add("", Entity.DATE_ADD.Trim(), CaseworkerName(Entity.WORKER.Trim()), "8", ""); break;
                        }

                        PrivCode = Entity.LETR_CODE.Trim();
                    }
                }

            }


        }

        private void gvApp_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvApp_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {


        }

        //private void On_Delay_Eligibility()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    ReaderName = propReportPath + "\\" + "Delay_in_Eligibility_Determination.pdf";



        //    PdfName = "Delay_in_Eligibility_Determination";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos+300, Y_Pos, 0);

        //        X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        MaskedTextBox mskPhn = new MaskedTextBox();
        //        mskPhn.Mask = "(000)000-0000";
        //        mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch(Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Eligibility_Notification()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "Eligibility_Notification.pdf";



        //    PdfName = "Eligibility_Notification";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Denial_Notice()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "Denial_Notice.pdf";



        //    PdfName = "Denial_Notice";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Right_Appeal()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "Right_to_Appeal.pdf";



        //    PdfName = "Right_to_Appeal";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Benefit_Fullfilment_English()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "CEAP_Benefit_Fulfillment_Eng.pdf";



        //    PdfName = "CEAP_Benefit_Fulfillment";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Benefit_Fullfilment_Spanish()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "CEAP_Benefit_Fulfillment_Spa.pdf";



        //    PdfName = "CEAP_Benefit_Fulfillment";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Client_Satisfaction_Survey_English()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //        ReaderName = propReportPath + "\\" + "Client_Satisfaction_Survey_Eng.pdf";



        //    PdfName = "Client_Satisfaction_Survey";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 500; Y_Pos = 60;

        //        //int pageCount = Hreader.NumberOfPages;
        //        //for (int i = 1; i <= pageCount; i++)
        //        //{
        //        //    if(i==2)
        //        //    {
        //        //        cb = Hstamper.GetOverContent(i);
        //        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, TableFont), X_Pos, Y_Pos, 0);
        //        //    }
        //        //}

        //        ////TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //    //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //    //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //    //mskPhn.Mask = "(000)000-0000";
        //        //    //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //    //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Client_Satisfaction_Survey_Spanish()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "Client_Satisfaction_Survey_Spa.pdf";



        //    PdfName = "Client_Satisfaction_Survey";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_Termination_Notice()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    //if(BaseForm.BaseCaseMstListEntity[0].Language.Trim()=="01")
        //    ReaderName = propReportPath + "\\" + "Termination_Notification.pdf";



        //    PdfName = "Termination_Notification";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);

        //    try
        //    {
        //        X_Pos = 150; Y_Pos = 692;

        //        //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(ti.ToTitleCase(BaseForm.BaseApplicationName), TableFont), X_Pos, Y_Pos, 0);
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(DateTime.Now.ToShortDateString(), TableFont), X_Pos + 300, Y_Pos, 0);

        //        //X_Pos = 250; Y_Pos = Y_Pos - 25;
        //        //MaskedTextBox mskPhn = new MaskedTextBox();
        //        //mskPhn.Mask = "(000)000-0000";
        //        //mskPhn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim();
        //        //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, TableFont), X_Pos, Y_Pos, 0);

        //    }
        //    catch (Exception ex) { }


        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }


        //}

        //private void On_CEAP_PriorityRankingForm()
        //{
        //    Random_Filename = null;

        //    string ReaderName = string.Empty;

        //    ReaderName = propReportPath + "\\" + "CEAP_Priority_Rating.pdf";



        //    PdfName = "CEAP_Priority_Rating";//form.GetFileName();
        //    //PdfName = strFolderPath + PdfName;
        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //    }

        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    PdfReader Hreader = new PdfReader(ReaderName);

        //    PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
        //    Hstamper.Writer.SetPageSize(PageSize.A4);
        //    PdfContentByte cb = Hstamper.GetOverContent(1);


        //    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //    iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
        //    BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
        //    iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
        //    BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

        //    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
        //    iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
        //    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 11, 1);
        //    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
        //    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
        //    iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

        //    iTextSharp.text.Image _image_Tick = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Images\\Tick_icon.png"));
        //    // iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

        //    _image_Tick.ScalePercent(60f);
        //    //_image_Checked.ScalePercent(60f);

        //    List<CustomQuestionsEntity> custResponses = new List<CustomQuestionsEntity>();
        //    custResponses = _model.CaseMstData.CAPS_CASEUSAGE_GET(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

        //    ProgramDefinitionEntity programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
        //    int intfromage = 0; int inttoage = 0;
        //    if (programEntity != null)
        //    {
        //        intfromage = Convert.ToInt16(programEntity.DepSENFromAge == string.Empty ? "0" : programEntity.DepSENFromAge);
        //        inttoage = Convert.ToInt16(programEntity.DepSENToAge == string.Empty ? "0" : programEntity.DepSENToAge);
        //    }
        //    double doublesertotal = 0;
        //    CustomQuestionsEntity responsetot = custResponses.Find(u => u.USAGE_MONTH.Equals("TOT"));
        //    if (responsetot != null)
        //    {
        //        doublesertotal = Convert.ToDouble(responsetot.SER_ANNUAL == string.Empty ? "0" : responsetot.SER_ANNUAL);
        //    }

        //    double doubleTotalAmount = Convert.ToDouble(BaseForm.BaseCaseMstListEntity[0].FamIncome == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].FamIncome);
        //    double totaldive = (doublesertotal / doubleTotalAmount) * 100;
        //    totaldive = Math.Round(totaldive, 2);
        //    try
        //    {
        //        X_Pos = 30; Y_Pos = 760;

        //        X_Pos = 150; Y_Pos -= 90;

        //        int inttotalcount = 0;


        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TblFontBold), X_Pos, Y_Pos, 0);


        //        X_Pos = 500;


        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseCaseMstListEntity[0].FamilyId, TblFontBold), X_Pos, Y_Pos, 0);

        //        List<CaseSnpEntity> casesnpEligbulity = BaseForm.BaseCaseSnpEntity.FindAll(u => u.DobNa.Equals("0") && u.Status == "A");
        //        List<CaseSnpEntity> casesnpElder = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) >= Convert.ToDecimal(intfromage)) && (Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(inttoage))));
        //        int inteldercount = casesnpElder.Count * 4;
        //        inttotalcount = inttotalcount + inteldercount;

        //        List<CaseSnpEntity> casesnpyounger = casesnpEligbulity.FindAll(u => ((Convert.ToDecimal(u.Age == string.Empty ? 0 : Convert.ToDecimal(u.Age)) <= Convert.ToDecimal(5))));
        //        int intyoungercount = casesnpyounger.Count * 4;
        //        inttotalcount = inttotalcount + intyoungercount;

        //        List<CaseSnpEntity> casesnpdisable = casesnpEligbulity.FindAll(u => u.Disable.ToString().ToUpper() == "Y" && u.Status == "A");
        //        int intdisablecount = casesnpdisable.Count * 4;
        //        inttotalcount = inttotalcount + intdisablecount;

        //        int intNoneabove = 0;
        //        if (inttotalcount == 0)
        //        {
        //            inttotalcount = inttotalcount + intNoneabove;
        //            intNoneabove = 1;
        //        }
        //        int intfity = 0; int intsenvtyfive = 0; int inttwentyfive = 0; int inttwentytofifty = 0; int intfiftyone = 0;
        //        int intmstpoverty = Convert.ToInt32(BaseForm.BaseCaseMstListEntity[0].Poverty == string.Empty ? "0" : BaseForm.BaseCaseMstListEntity[0].Poverty);

        //        if (intmstpoverty <= 50)
        //        {
        //            inttotalcount = inttotalcount + 10;
        //            intfity = 10;
        //        }
        //        else if (intmstpoverty >= 51 && intmstpoverty <= 75)
        //        {
        //            inttotalcount = inttotalcount + 7;
        //            intsenvtyfive = 7;
        //        }
        //        else if (intmstpoverty >= 76 && intmstpoverty <= 125)
        //        {
        //            inttotalcount = inttotalcount + 3;
        //            inttwentyfive = 3;
        //        }
        //        else if (intmstpoverty >= 126 && intmstpoverty <= 150)
        //        {
        //            inttotalcount = inttotalcount + 1;
        //            inttwentytofifty = 1;
        //        }
        //        else if (intmstpoverty <= 151)
        //        {

        //            intfiftyone = 0;
        //        }

        //        int intExceedYes = 0; int intExceedNo = 0;
        //        if (doublesertotal > 1000)
        //        {
        //            inttotalcount = inttotalcount + 4;
        //            intExceedYes = 4;
        //        }
        //        else
        //        {
        //            inttotalcount = inttotalcount + 1;
        //            intExceedNo = 1;
        //        }


        //        int intthirty = 0; int inttwenty = 0; int inteleven = 0; int intsix = 0; int intfive = 0;
        //        if (doubleTotalAmount == 0 || doublesertotal == 0)
        //        {
        //            intfive = 0;
        //        }
        //        else
        //        {

        //            if (totaldive >= 30)
        //            {
        //                inttotalcount = inttotalcount + 12;
        //                intthirty = 12;
        //            }
        //            else if (totaldive >= 20 && totaldive <= 29.99)
        //            {
        //                inttotalcount = inttotalcount + 9;
        //                inttwenty = 9;
        //            }
        //            else if (totaldive >= 11 && totaldive <= 19.99)
        //            {
        //                inttotalcount = inttotalcount + 6;
        //                inteleven = 6;
        //            }
        //            else if (totaldive >= 6 && totaldive <= 10.99)
        //            {
        //                inttotalcount = inttotalcount + 3;
        //                intsix = 3;
        //            }
        //            else if (totaldive <= 5.99)
        //            {
        //                if (doubleTotalAmount == 0 || doublesertotal == 0)
        //                {
        //                    intfive = 0;
        //                }
        //                else
        //                {
        //                    inttotalcount = inttotalcount + 1;
        //                    intfive = 1;
        //                }
        //            }
        //        }

        //        X_Pos = 535;
        //        Y_Pos -= 45;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteldercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intdisablecount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intyoungercount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intNoneabove.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 42;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfity.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsenvtyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentyfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwentytofifty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfiftyone.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        X_Pos = 65; Y_Pos -= 45;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doublesertotal.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        X_Pos = 200;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(doubleTotalAmount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        X_Pos = 400;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(totaldive.ToString().ToUpper() == "NAN" ? string.Empty : totaldive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


        //        X_Pos = 535;
        //        Y_Pos -= 37;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intthirty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttwenty.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inteleven.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intsix.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intfive.ToString(), TblFontBold), X_Pos, Y_Pos, 0);


        //        Y_Pos -= 40;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedYes.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 17;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(intExceedNo.ToString(), TblFontBold), X_Pos, Y_Pos, 0);
        //        Y_Pos -= 30;
        //        ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(inttotalcount.ToString(), TblFontBold), X_Pos, Y_Pos, 0);

        //        if (inttotalcount >= 17)
        //        {
        //            X_Pos = 40;
        //            Y_Pos -= 30;
        //            _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
        //            cb.AddImage(_image_Tick);
        //        }
        //        else if (inttotalcount >= 11 && inttotalcount <= 16)
        //        {
        //            X_Pos = 40;
        //            Y_Pos -= 66;
        //            _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
        //            cb.AddImage(_image_Tick);
        //        }
        //        else if (inttotalcount <= 10)
        //        {
        //            X_Pos = 40;
        //            Y_Pos -= 93;
        //            _image_Tick.SetAbsolutePosition(X_Pos, Y_Pos - 10);
        //            cb.AddImage(_image_Tick);
        //        }


        //        StringBuilder strMstAppl = new StringBuilder();
        //        strMstAppl.Append("<Applicants>");
        //        strMstAppl.Append("<Details MSTApplDetails = \"" + BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + (BaseForm.BaseYear.Trim() == string.Empty ? "    " : BaseForm.BaseYear.Trim()) + BaseForm.BaseApplicationNo + "\" MST_RANK1 = \"" + inttotalcount.ToString() + "\" MST_RANK2 = \"" + "0" + "\" MST_RANK3 = \"" + "0" + "\" MST_RANK4 = \"" + "0" + "\" MST_RANK5 = \"" + "0" + "\" MST_RANK6 = \"" + "0" + "\"   />");
        //        strMstAppl.Append("</Applicants>");

        //        if (_model.CaseMstData.UpdateCaseMstRanks(strMstAppl.ToString(), "Single"))
        //        {
        //            BaseForm.BaseCaseMstListEntity[0].Rank1 = inttotalcount.ToString();
        //        }



        //    }
        //    catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

        //    Hstamper.Close();

        //    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
        //    {
        //        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }
        //    else
        //    {
        //        FrmViewer objfrm = new FrmViewer(PdfName);
        //        objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
        //        objfrm.ShowDialog();
        //    }

        //}

        //private void SavePrintRecord()
        //{
        //    LETRHISTCEntity Entity = new LETRHISTCEntity();
        //    string Msg = string.Empty;
        //    Entity.AGENCY = BaseForm.BaseAgency;
        //    Entity.DEPT = BaseForm.BaseDept;
        //    Entity.PROGRAM = BaseForm.BaseProg;
        //    Entity.YEAR = BaseForm.BaseYear;
        //    Entity.APPNO = BaseForm.BaseApplicationNo;
        //    Entity.LETR_CODE = gvApp.CurrentRow.Cells["gvCode"].Value.ToString();
        //    Entity.DATE = DateTime.Now.ToShortDateString();
        //    Entity.SEQ = "1";

        //    if(CaseWorker!=null)
        //        Entity.WORKER = CaseWorker.CaseWorker.Trim();
        //    Entity.ADD_OPERATOR = BaseForm.UserID;

        //    _model.SPAdminData.InsertLETRHIST(Entity, out Msg);
        //}

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void SetLine()
        {
            cb.SetLineWidth(2f);
            cb.SetLineCap(5);
            cb.MoveTo(X_Pos, Y_Pos);
            cb.LineTo(780, Y_Pos);
            cb.Stroke();
        }

        DataTable dtIncome = new DataTable();
        private string Get_IncomeType_Desc(string Type_Code)
        {
            string Income_Desc = string.Empty;
            foreach (DataRow drIncome in dtIncome.Rows)
            {
                if (Type_Code == drIncome["Code"].ToString().Trim())
                {
                    Income_Desc = drIncome["LookUpDesc"].ToString().Trim(); break;
                }
            }

            return Income_Desc;
        }

        DataTable dtCaseSNP = new DataTable();
        private string Get_Member_Name(string Mem_Seq, string NameFormat)
        {
            string Member_NAme = string.Empty;
            foreach (DataRow drCaseSnp in dtCaseSNP.Rows)
            {
                if (Mem_Seq == drCaseSnp["SNP_FAMILY_SEQ"].ToString().Trim())
                {
                    if (NameFormat == "First")
                    {
                        Member_NAme = drCaseSnp["SNP_NAME_IX_FI"].ToString().Trim(); break;
                    }
                    else
                        Member_NAme = LookupDataAccess.GetMemberName(drCaseSnp["SNP_NAME_IX_FI"].ToString().Trim(), drCaseSnp["SNP_NAME_IX_MI"].ToString().Trim(), drCaseSnp["SNP_NAME_IX_LAST"].ToString().Trim(), strNameFormat) + "  "; break;
                }
            }

            return Member_NAme;
        }



        int pageNumber = 1;
        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {
            if (Y_Pos <= 20)
            {
                cb.EndText();
                //cb.BeginText();
                //Y_Pos = 07;
                //X_Pos = 20;
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                ////PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                //Y_Pos = 07;
                //X_Pos = 550;
                //PrintRec("Page:", 28);
                //PrintRec(pageNumber.ToString(), 15);
                //cb.EndText();

                document.NewPage();
                pageNumber = writer.PageNumber - 1;

                //cb.BeginText();

                //X_Pos = 50;
                //Y_Pos -= 5;

                //cb.EndText();

                Y_Pos = 770;
                X_Pos = 40;                                                           //modified

                cb.BeginText();

            }
        }

        private void CheckBottomBorderReachedLetterHead(PdfStamper Hstamper)
        {
            if (Y_Pos <= 20)
            {
                cb.EndText();
                //cb.BeginText();
                //Y_Pos = 07;
                //X_Pos = 20;
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                ////PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                //Y_Pos = 07;
                //X_Pos = 550;
                //PrintRec("Page:", 28);
                //PrintRec(pageNumber.ToString(), 15);
                //cb.EndText();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                ////document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

                document.NewPage();
                //pageNumber = Hstamper.PageNumber - 1;

                //cb.BeginText();

                //X_Pos = 50;
                //Y_Pos -= 5;

                //cb.EndText();

                Y_Pos = 770;
                X_Pos = 90;                                                           //modified

                cb.BeginText();

            }
        }



        private void PrintSpaceCell(PdfPTable table, int Spacesnum, iTextSharp.text.Font TableFont, float Height)
        {
            if (Spacesnum == 1)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 2)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 2;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 3)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 3;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 4)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 4;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 6)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 6;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 7)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 7;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 10)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 10;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 15)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 15;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 12)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 12;
                if (Height > 0)
                    S2.FixedHeight = Height;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
        }

        public string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public string HumanisedDate(string date1)
        {
            DateTime date = Convert.ToDateTime(date1.Trim());

            string ordinal;

            switch (date.Day)
            {
                case 1:
                case 21:
                case 31:
                    ordinal = "st";
                    break;
                case 2:
                case 22:
                    ordinal = "nd";
                    break;
                case 3:
                case 23:
                    ordinal = "rd";
                    break;
                default:
                    ordinal = "th";
                    break;
            }

            return string.Format("{0:MMMM dd}{1} ", date, ordinal);
        }



        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 8: TmpCode = "0" + TmpCode; break;
                case 7: TmpCode = "00" + TmpCode; break;
                case 6: TmpCode = "000" + TmpCode; break;
                case 5: TmpCode = "0000" + TmpCode; break;
                case 4: TmpCode = "00000" + TmpCode; break;
                case 3: TmpCode = "000000" + TmpCode; break;
                case 2: TmpCode = "0000000" + TmpCode; break;
                case 1: TmpCode = "00000000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }



    }
}